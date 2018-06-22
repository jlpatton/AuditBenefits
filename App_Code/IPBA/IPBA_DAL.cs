using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace EBA.Desktop.IPBA
{
    public class IPBA_DAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public IPBA_DAL()
        {
        }

        public DataSet GetDetData(string yrmo, string plancode)
        {
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsFinal = new DataSet(); dsFinal.Clear();

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                dsFinal = ExecuteStoredProc("sp_IPBARptA", yrmo, plancode, "DET");
                dsTemp = ExecuteStoredProc("sp_IPBARptG", yrmo, plancode, "DET");
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    dsFinal.Merge(dsTemp); dsTemp.Clear();
                }
                dsTemp = ExecuteStoredProc("sp_IPBA_COBRA", yrmo, plancode, "COBDET");
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    dsFinal.Merge(dsTemp); dsTemp.Clear();
                }

                return dsFinal;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetAdjData(string yrmo, string plancode)
        {
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsFinal = new DataSet(); dsFinal.Clear();

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                dsFinal = ExecuteStoredProc("sp_IPBARptE", yrmo, plancode, "ADJ");
                dsTemp = ExecuteStoredProc("sp_IPBARptC", yrmo, plancode, "ADJ"); dsFinal.Merge(dsTemp); dsTemp.Clear(); 
                dsTemp = ExecuteStoredProc("sp_IPBARptD", yrmo, plancode, "ADJ"); dsFinal.Merge(dsTemp); dsTemp.Clear();

                return dsFinal;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetManAdjData(string yrmo, string plancode, string source)
        {
            DataSet ds = new DataSet(); ds.Clear();

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                switch (source)
                {
                    case "NEW":
                        ds = ExecuteStoredProc("sp_IPBAManAdjRetro", yrmo, plancode, "ADJ");
                        break;
                    case "TERM":
                        ds = ExecuteStoredProc("sp_IPBAManAdjTerm", yrmo, plancode, "ADJ");
                        break;
                    case "CHG":
                        ds = ExecuteStoredProc("sp_IPBAManAdjCHG", yrmo, plancode, "ADJ");
                        break;
                }                

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetCobraAdjData(string yrmo, string plancode)
        {
            DataSet ds = new DataSet(); ds.Clear();

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                ds = ExecuteStoredProc("sp_IPBA_COBRA_ADJ", yrmo, plancode, "ADJ");
               

                return ds;
            }
            finally
            {
                connect.Close();            
            }
        }

        DataSet ExecuteStoredProc(string spName, string yrmo, string plancode, string source)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();            
            ImportDAL iobj = new ImportDAL();

            command = new SqlCommand(spName, connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
            command.Parameters.Add("@plancode", SqlDbType.VarChar).Value = plancode;
            if (source.Equals("ADJ") || source.Equals("COBDET"))
                command.Parameters.Add("@prevyrmo", SqlDbType.VarChar).Value = iobj.getPrevYRMO(yrmo);
            da.SelectCommand = command;
            da.Fill(ds);
            command.Dispose();

            return ds;
        }

        public Decimal GetRate(string yrmo, string programcd, string plancd, string tiercd)
        {
            Decimal rate = 0;
            IPBA iobj = new IPBA();

            programcd = iobj.GetProgramCode(programcd);
            plancd = iobj.GetPlanCode(plancd, programcd);

            string cmdstr = "SELECT rate_companyRtamt "
                                + "FROM Rates,Planhier "
                                + "WHERE ph_progmcd = '" + programcd + "' "
                                + "AND ph_plancd = '" + plancd + "' "
                                + "AND ph_tiercd = '" + tiercd + "' "
                                + "AND rate_effyrmo = "
                                + "( "
                                + "SELECT MAX(rate_effyrmo) "
                                + "FROM Rates "
                                + "WHERE rate_effyrmo <= '" + yrmo + "' "
                                + ") "
                                + "AND (rate_plhr_id = ph_id) ";
                                

             try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                rate = Convert.ToDecimal(command.ExecuteScalar());

                return rate;
            }
            finally
            {
                connect.Close();
            }

        }

        public List<String> GetAllPlanCodes(string yrmo)
        {
            List<String> plancodes = new List<String>();
            SqlDataReader oRead = null;

            string cmdstr = "SELECT [codeid] FROM [Codes] WHERE ([source] = 'HMOBILLRPT')";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                oRead = command.ExecuteReader();

                while (oRead.Read())
                {
                    plancodes.Add(oRead.GetString(0));
                }
                oRead.Close();

                return plancodes;
            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean ManAdjDupExists(string plancode, string ssn, string yrmo)
        {
            int count = 0;
            string cmdstr =  "SELECT COUNT(*) "
                                + "FROM HTH_HMO_Billing " 
                                + "WHERE YRMO = '" + yrmo + "' " 
                                + "AND SSN='" + ssn + "' " 
                                + "AND plancdadj='" + plancode +"' "
                                + "AND ReportType='ADJ'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                count = (int)command.ExecuteScalar();

                if (count > 0)
                    return true;
                else
                    return false;

            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean ManAdjDupExists(string plancode, string ssn, string yrmo, int id)
        {
            int count = 0;
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM HTH_HMO_Billing "
                                + "WHERE YRMO = '" + yrmo + "' "
                                + "AND SSN='" + ssn + "' "
                                + "AND plancdadj='" + plancode + "' "
                                + "AND ReportType='ADJ' AND SeqNum <> " + id;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                count = (int)command.ExecuteScalar();

                if (count > 0)
                    return true;
                else
                    return false;

            }
            finally
            {
                connect.Close();
            }
        }
    }
}
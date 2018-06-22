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
    /// <summary>
    /// Summary description for IPBAImportDAL
    /// </summary>
    public class IPBAImportDAL
    {

        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public IPBAImportDAL()
        {
        }

        public static List<string> getHMOCodes()
        {
            string _cmdstr = "SELECT codeid FROM Codes WHERE source = 'HMOBILL'";
            List<string> _codes = new List<string>();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _codes.Add(reader[0].ToString());
                }
                reader.Close();
                return _codes;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static int insertADP(DataSet dsAC, string _yrmo)
        {
            string _planyear = _yrmo.Substring(0, 4);
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                foreach (DataRow dr in dsAC.Tables[0].Rows)
                {
                    command = new SqlCommand("sp_IPBAImportADP", connect); 

                    if (dr["plan"].ToString().Length > 2)
                    {
                        if (dr["plan"].ToString().Substring(0, (dr["plan"].ToString().Length - 2)).Contains("P"))
                        {
                            dr["bensts"] = "P";
                        }
                        dr["plan"] = dr["plan"].ToString().Remove(0, (dr["plan"].ToString().Length - 2));
                    }

                    command.Parameters.AddWithValue("@YRMO", _yrmo);
                    command.Parameters.AddWithValue("@PlanYear ", _planyear);
                    command.Parameters.AddWithValue("@PilotInd", dr["bensts"].ToString().Equals("") ? DBNull.Value : dr["bensts"]);
                    command.Parameters.AddWithValue("@SSN", dr["SSN"]);
                    command.Parameters.AddWithValue("@FirstName", dr["fname"]);
                    command.Parameters.AddWithValue("@LastName", dr["lname"]);
                    command.Parameters.AddWithValue("@PlanCode", dr["plan"]);
                    command.Parameters.AddWithValue("@TierCode", dr["covlvl"]);
                    command.Parameters.AddWithValue("@CoverageEffDt", dr["effdt"].ToString().Equals("") ? DBNull.Value : dr["effdt"]);
                    command.Parameters.AddWithValue("@CoverageTermDt", dr["termdt"].ToString().Equals("") ? DBNull.Value : dr["termdt"]);                                        
                    command.Parameters.AddWithValue("@ReportType", "COB");
                    object _rate = DBNull.Value;
                    if(!dr["premium"].ToString().Equals(""))
                    {
                        _rate = Convert.ToDecimal(dr["premium"]);
                    }
                    command.Parameters.AddWithValue("@Rate",_rate);
                    command.Parameters.AddWithValue("@Comments", dr["comments"].ToString().Equals("") ? DBNull.Value : dr["comments"]);
                    command.Parameters.AddWithValue("@CvgPrd", dr["covgperiod"].ToString());
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }

                return dsAC.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error Importing ADP Cobra Report"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static int insertGreenBar(DataSet dsGB, string _yrmo)
        {
            string _planyear = _yrmo.Substring(0, 4);
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                foreach (DataRow dr in dsGB.Tables[0].Rows)
                {
                    command = new SqlCommand("sp_IPBAImportGreenbar", connect);

                    command.Parameters.AddWithValue("@YRMO", _yrmo);
                    command.Parameters.AddWithValue("@PlanYear ", _planyear);                    
                    command.Parameters.AddWithValue("@PilotInd", dr["pInd"].ToString().Equals("") ? DBNull.Value : dr["pInd"]);                    
                    command.Parameters.AddWithValue("@SSN", dr["SSN"]);
                    command.Parameters.AddWithValue("@FirstName", dr["fname"]);
                    command.Parameters.AddWithValue("@LastName", dr["lname"]);
                    command.Parameters.AddWithValue("@Prior_PlanCode", dr["p_plancode"].ToString().Equals("") ? DBNull.Value : dr["p_plancode"]);
                    command.Parameters.AddWithValue("@Prior_TierCode", dr["p_covlvl"].ToString().Equals("") ? DBNull.Value : dr["p_covlvl"]);
                    command.Parameters.AddWithValue("@PlanCode", dr["plancode"].ToString().Equals("") ? DBNull.Value : dr["plancode"]);
                    command.Parameters.AddWithValue("@TierCode", dr["covlvl"].ToString().Equals("") ? DBNull.Value : dr["covlvl"]);
                    command.Parameters.AddWithValue("@CoverageEffDt", dr["effdt"].ToString().Equals("") ? DBNull.Value : dr["effdt"]);
                    command.Parameters.AddWithValue("@CoverageTermDt", dr["termdt"].ToString().Equals("") ? DBNull.Value : dr["termdt"]);
                    command.Parameters.AddWithValue("@ReportType", dr["report"]);
                    object _rate = DBNull.Value;
                    if (!dr["rate"].ToString().Equals(""))
                    {
                        _rate = Convert.ToDecimal(dr["rate"]);
                    }
                    command.Parameters.AddWithValue("@Rate", _rate);

                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(SqlException e)
                    {
                        throw e;
                    }
                }

                return dsGB.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                throw(new Exception("Error Importing Greenbar Report"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static bool pastImport(string _src, string _yrmo)
        {
            string _cmdstr = "";
            bool _imported = false;
            switch (_src)
            {
                case "ADP":
                    _cmdstr = "SELECT COUNT(*) FROM HTH_HMO_Billing WHERE ReportType = 'COB' AND YRMO = '" + _yrmo + "'";
                    break;
                case "Greenbar":
                    _cmdstr = "SELECT COUNT(*) FROM HTH_HMO_Billing WHERE ReportType IN ('A','C','D','E','F','G') AND YRMO = '" + _yrmo + "'";
                    break;
            }
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                int _count = Convert.ToInt32(command.ExecuteScalar());
                if (_count > 1)
                {
                    _imported = true;
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return _imported;
        }

        public static void rollback(string _src, string _yrmo)
        {
            string _cmdstr = "";            
            switch (_src)
            {
                case "ADP":
                    _cmdstr = "DELETE FROM HTH_HMO_Billing WHERE ReportType = 'COB' AND YRMO = '" + _yrmo + "'";
                    break;
                case "Greenbar":
                    _cmdstr = "DELETE FROM HTH_HMO_Billing WHERE ReportType IN ('A','C','D','E','F','G') AND YRMO = '" + _yrmo + "'";
                    break;
            }
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }           
        }

        public static int insertRates(DataSet ratesIm)
        {
            string _cmdstr = "",_cmdstr1="";
            int _count = 0;

            _cmdstr = "SELECT ph_id FROM [dbo].[Planhier] "
                        + " WHERE [ph_progmcd] = @prog AND [ph_plancd] = @pcode AND [ph_tiercd] = @tcode ";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            SqlTransaction ts;
            ts = connect.BeginTransaction();
            try
            {
                foreach (DataRow row in ratesIm.Tables["rateTemplate"].Rows)
                {
                    int _phId = 0; 
                    command = new SqlCommand(_cmdstr, connect, ts);
                    command.Parameters.AddWithValue("@prog", row[2]);
                    command.Parameters.AddWithValue("@pcode", row[5]);
                    command.Parameters.AddWithValue("@tcode", row[6]);
                    _phId = Convert.ToInt32(command.ExecuteScalar());
                    _cmdstr1 = " INSERT INTO [dbo].[Rates] "
                                + " ([rate_plhr_id],[rate_rateamt],[rate_companyRtamt],[rate_effyrmo],[rate_py]) "
                                + " VALUES "
                                + "( " + _phId + "," + Convert.ToDecimal(row[7]) + "," + Convert.ToDecimal(row[8]) + ",'" + row[9] + "','" + row[0] + "')";
                    command = new SqlCommand(_cmdstr1, connect, ts);
                    command.ExecuteNonQuery();
                    _count++;     
                }
                ts.Commit();
                return _count;
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw ex;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }          
            
        }
    }
}

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

/// <summary>
/// Summary description for DetailReports
/// </summary>
/// 
namespace EBA.Desktop.Anthem
{
    public class DetailReports
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;        
        static SqlConnection connect = new SqlConnection(connStr);       
        static SqlCommand command = null;

        public DetailReports()
        {
        }

        public static DataSet getCobraDetails(string _yrmo)
        {
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();
            DataSet ds1 = getMatchedCobraRecon(_yrmo);
            DataSet ds2 = getADPUnmatchedRecon(_yrmo);
            DataSet ds3 = getAnthCobUnmatchedRecon(_yrmo);

            dsFinal.Merge(ds1); ds1.Clear();
            dsFinal.Merge(ds2); ds2.Clear();
            dsFinal.Merge(ds3); ds3.Clear();

            //dsFinal.Tables["newTable1"].DefaultView.Sort = "Plan Code ASC";

            return sortCobraDetails(dsFinal,_yrmo);
        }

        protected static DataSet getMatchedCobraRecon(string _yrmo)
        {
            string _cmdstr = "SELECT anthcd_plancd AS [Plan Code],anthcd_covgcd AS [Tier Code],bill_ssn AS [SSN],bill_name AS [Name],count(*) AS [ADP Cobra Count],count(*) AS [Anthem Bill Count] "
                                + "FROM AnthCodes, ADP_Details,billing_details "
                                + "WHERE (bill_anthcd_id = anthcd_id AND bill_ssn = adp_ssn) "
                                + "AND bill_yrmo = @yrmo AND adp_covg_period = @yrmo AND bill_source = 'ANTH_COB' "
                                + "GROUP BY anthcd_plancd,anthcd_covgcd,bill_ssn,bill_name,adp_ssn,adp_EmpName";
            DataSet ds = new DataSet();
            ds.Clear();
            DataTable newTable;
            newTable = ds.Tables.Add("newTable1");
            //DataColumn col;

            //col = new DataColumn("Plan Code"); newTable.Columns.Add(col);
            //col = new DataColumn("Tier Code"); newTable.Columns.Add(col);
            //col = new DataColumn("SSN"); newTable.Columns.Add(col);
            //col = new DataColumn("Name"); newTable.Columns.Add(col);
            //col = new DataColumn("ADP Cobra Count"); newTable.Columns.Add(col);
            //col = new DataColumn("Anthem bill Count"); newTable.Columns.Add(col); 
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds.Tables["newTable1"]);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }

        protected static DataSet getADPUnmatchedRecon(string _yrmo)
        {
            string _cmdstr = "SELECT anthcd_plancd AS [Plan Code],anthcd_covgcd AS [Tier Code],adp_ssn AS [SSN],adp_EmpName AS [Name] ,count(*) AS [ADP Cobra Count],0 AS [Anthem Bill Count] "
                                + " FROM ADP_Details,AnthCodes,AnthPlanhier "
                                + " WHERE adp_ssn "
                                + " NOT IN "
                                + "     (select bill_ssn "
                                + "     FROM billing_details "
                                + "     WHERE  bill_yrmo = @yrmo "
                                + "     AND bill_source = 'ANTH_COB') "
                                + " AND adp_covg_period = @yrmo "
                                + " AND (plhr_anthcd_id = anthcd_id) "
                                + " AND (plhr_plancd = adp_plan) "
                                + " AND (plhr_tiercd = adp_covlvl) "
                                + " GROUP BY anthcd_plancd,anthcd_covgcd,adp_ssn,adp_EmpName ";

            DataSet ds = new DataSet();
            ds.Clear();
            DataTable newTable;
            newTable = ds.Tables.Add("newTable1");
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds.Tables["newTable1"]);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }

        protected static DataSet getAnthCobUnmatchedRecon(string _yrmo)
        {
            string _cmdstr = "SELECT anthcd_plancd AS [Plan Code],anthcd_covgcd AS [Tier Code],bill_ssn AS [SSN],bill_name AS [Name],0 AS [ADP Cobra Count],count(*) AS [Anthem Bill Count] "
                                + " FROM billing_details,AnthCodes "
                                + " WHERE bill_ssn "
                                + " NOT IN "
                                + " 	(select adp_ssn "
                                + " 	FROM ADP_Details "
                                + " 	WHERE  adp_covg_period = @yrmo) "
                                + " AND bill_yrmo = @yrmo "
                                + " AND bill_source = 'ANTH_COB' "
                                + " AND anthcd_id = bill_anthcd_id "
                                + " GROUP BY anthcd_plancd,anthcd_covgcd,bill_ssn,bill_name";
            DataSet ds = new DataSet();
            ds.Clear();
            DataTable newTable;
            newTable = ds.Tables.Add("newTable1");
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds.Tables["newTable1"]);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }
        protected static DataSet sortCobraDetails(DataSet dsRes, string yrmo)
        {
            string cmdStr = " select DISTINCT anthcd_plancd "
                            + " FROM Anthplanhier,AnthCodes WHERE anthcd_id = plhr_anthcd_id "
                            + " AND plhr_plandesc LIKE '%COB%'"
                            + " ORDER BY anthcd_plancd";

            List<string> _aCode = new List<string>();
            int _anthCount;
            int _ebaCount;
            SqlDataReader reader;
            DataRow[] rows;
            DataRow rowNew;
            DataSet dsTotal = new DataSet();
            DataTable tempTable, newTable;
            tempTable = dsRes.Tables["newTable1"];
            newTable = dsTotal.Tables.Add("newTable0");
            DataColumn col;
            col = new DataColumn("YRMO"); newTable.Columns.Add(col);
            col = new DataColumn("Plan Code"); newTable.Columns.Add(col);
            col = new DataColumn("Tier Code"); newTable.Columns.Add(col);
            col = new DataColumn("SSN"); newTable.Columns.Add(col);
            col = new DataColumn("Name"); newTable.Columns.Add(col);
            col = new DataColumn("ADP Cobra Count"); newTable.Columns.Add(col);
            col = new DataColumn("Anthem bill Count"); newTable.Columns.Add(col);

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            SqlCommand command = new SqlCommand(cmdStr, connect);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                _aCode.Add(reader[0].ToString());
            }
            reader.Close();
            connect.Close();

            foreach (string c1 in _aCode)
            {
                _anthCount = 0;
                _ebaCount = 0;
                string strFilter = "[Plan Code] = '" + c1 + "'";
                rows = tempTable.Select(strFilter);
                if (rows.Length > 0)
                {
                    foreach (DataRow r1 in rows)
                    {
                        rowNew = newTable.NewRow();
                        rowNew["YRMO"] = yrmo;
                        rowNew["Plan Code"] = r1["Plan Code"];
                        rowNew["Tier Code"] = r1["Tier Code"];
                        rowNew["SSN"] = r1["SSN"];
                        rowNew["Name"] = r1["Name"];
                        rowNew["ADP Cobra Count"] = r1["ADP Cobra Count"];
                        _ebaCount = _ebaCount + Convert.ToInt32(r1["ADP Cobra Count"]);
                        rowNew["Anthem Bill Count"] = r1["Anthem Bill Count"];
                        _anthCount = _anthCount + Convert.ToInt32(r1["Anthem Bill Count"]);
                        newTable.Rows.Add(rowNew);
                    }

                    rowNew = newTable.NewRow();
                    rowNew["YRMO"] = "";
                    rowNew["Plan Code"] = "";
                    rowNew["Tier Code"] = "";
                    rowNew["SSN"] = "";
                    rowNew["Name"] = "<font color=\"#036\"><b>Total: </b></font>";
                    rowNew["ADP Cobra Count"] = _ebaCount;
                    rowNew["Anthem Bill Count"] = _anthCount;
                    newTable.Rows.Add(rowNew);
                }
            }

            return dsTotal;
        }

        public static DataSet getCobraDetailsReport(string _yrmo)
        {
            string _cmdstr = "SELECT adp_plan AS [Plan],adp_planDesc AS [Plan Desc.], "
                                + "adp_ssn AS [Subscriber ID],adp_EmpName AS [Subscriber Name],adp_div AS [Division], "
                                + "CONVERT(VARCHAR(15),adp_covg_eff_dt,101) AS [Coverage EffDt.],adp_covlvl AS [Coverage Level], "
                                + "adp_covg_period AS [Coverage Period],adp_comments AS [Comments], "
                                + "adp_total_premium AS [Total Premium],adp_total_carrier AS [Total Carrier] "
                                + "FROM ADP_Details "
                                + "WHERE adp_covg_period = @yrmo";
                                
            DataSet ds = new DataSet();
            ds.Clear();            
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }

        public static DataSet getADPUnmatched(string _yrmo)
        {
            string _cmdstr = "SELECT adp_plan AS [Plan],adp_planDesc AS [Plan Desc.], "
                                + "adp_ssn AS [Subscriber ID],adp_EmpName AS [Subscriber Name],adp_div AS [Division], "
                                + "CONVERT(VARCHAR(15),adp_covg_eff_dt,101) AS [Coverage Effdt.],adp_covlvl AS [Coverage Level], "
                                + "adp_covg_period AS [Coverage Period],adp_comments AS [Comments], "
                                + "adp_total_premium AS [Total Premium],adp_total_carrier AS [Total Carrier] "
                                + "FROM ADP_Details "
                                + " WHERE adp_ssn "
                                + " NOT IN "
                                + "     (select bill_ssn "
                                + "     FROM billing_details "
                                + "     WHERE  bill_yrmo = @yrmo "
                                + "     AND bill_source = 'ANTH_COB') "
                                + " AND adp_covg_period = @yrmo ";

            DataSet ds = new DataSet();
            ds.Clear();            
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }

        public static DataSet getAnthCobUnmatched(string _yrmo)
        {
            string _cmdstr = "SELECT anthcd_plancd AS [Plan Code],bill_ssn AS [Subscriber ID],bill_name AS [Subscriber Name], anthcd_covgcd AS [Coverage Code],bill_yrmo  AS [Coverage Period],bill_premadj AS [Premium]"
                                + " FROM billing_details,AnthCodes "
                                + " WHERE bill_ssn "
                                + " NOT IN "
                                + " 	(select adp_ssn "
                                + " 	FROM ADP_Details "
                                + " 	WHERE  adp_covg_period = @yrmo) "
                                + " AND bill_yrmo = @yrmo "
                                + " AND bill_source = 'ANTH_COB' "
                                + " AND anthcd_id = bill_anthcd_id ";
            DataSet ds = new DataSet();
            ds.Clear();            
            SqlDataAdapter da = new SqlDataAdapter();

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                da.SelectCommand = command;
                da.Fill(ds);
            }
            finally
            {
                connect.Close();
            }
            return ds;
        }

         
    }

    
}

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
using EBA.Desktop.Anthem;

/// <summary>
/// Summary description for RecordCount
/// </summary>
/// 
namespace EBA.Desktop.Anthem
{
    public class RecordCount
    {

        private static string _constr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        private static SqlConnection connect = new SqlConnection(_constr);
        private static SqlCommand command = null;
        public RecordCount()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DataSet SummaryRecordsFinal(string yrmo)
        {
            DataSet dfRecordsDS = new DataSet(); dfRecordsDS.Clear();
            DataSet dfrfRecordsDS = new DataSet(); dfrfRecordsDS.Clear();
            dfRecordsDS = DFRecords(yrmo);
            dfrfRecordsDS = DFRFRecords(yrmo);
            DataSet sumFinalDS = new DataSet();
            sumFinalDS.Merge(dfRecordsDS);
            sumFinalDS.Tables[0].NewRow();
            sumFinalDS.Tables[0].NewRow();
            sumFinalDS.Merge(dfrfRecordsDS);

            return sumFinalDS;
        }

        protected static DataSet DFRecords(string yrmo)
        {
            DataSet dsMatched1 = ReconDAL.getReconM(yrmo);
            DataSet dfDups = getDFDups(yrmo);
            DataSet dsUnMatched1 = ReconDAL.getClaimsUM(yrmo);
            DataRow rowNew;
            DataSet dsDFTotal = new DataSet();
            DataTable tempTable1, newTable1;
            tempTable1 = dsMatched1.Tables[0];
            newTable1 = dsDFTotal.Tables.Add("newTable1");
            DataColumn col;
            col = new DataColumn("Type"); newTable1.Columns.Add(col);
            col = new DataColumn("RecordCount"); newTable1.Columns.Add(col);
            col = new DataColumn("DupCount"); newTable1.Columns.Add(col);
            col = new DataColumn("TotalCount"); newTable1.Columns.Add(col);

            int _grandTotal = 0;

            rowNew = newTable1.NewRow();
            rowNew["Type"] = "Anthem DF Records Summary";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";            
            rowNew["TotalCount"] = "";
            newTable1.Rows.Add(rowNew);

            rowNew = newTable1.NewRow();
            rowNew["Type"] = "Matched without Dups";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            _grandTotal = dsMatched1.Tables[0].Rows.Count - getDupsCountDFinMatched(yrmo, " > 1");
            rowNew["TotalCount"] = _grandTotal;
            newTable1.Rows.Add(rowNew);

            List<int> _dupCnt = new List<int>();
            _dupCnt = getRecordCountDFRFDup(yrmo);
            foreach (int x in _dupCnt)
            {
                int _temp = getDupsCountDFinMatched(yrmo, " = " + x);
                if (_temp != 0)
                {
                    rowNew = newTable1.NewRow();
                    rowNew["Type"] = "Dups on Matched Report";
                    rowNew["RecordCount"] = x;
                    rowNew["DupCount"] = _temp;
                    rowNew["TotalCount"] = (x * _temp);
                    _grandTotal = _grandTotal + (x * _temp);
                    newTable1.Rows.Add(rowNew);
                }
            }

            rowNew = newTable1.NewRow();
            rowNew["Type"] = "UnMatched";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = dsUnMatched1.Tables[0].Rows.Count;
            _grandTotal = _grandTotal + dsUnMatched1.Tables[0].Rows.Count;
            newTable1.Rows.Add(rowNew);

            rowNew = newTable1.NewRow();
            rowNew["Type"] = "Grand Total:";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = _grandTotal;
            newTable1.Rows.Add(rowNew);

            rowNew = newTable1.NewRow();
            rowNew["Type"] = "";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = "";
            newTable1.Rows.Add(rowNew);

            return dsDFTotal;
        }

        protected static DataSet DFRFRecords(string yrmo)
        {
            DataSet dsMatched = ReconDAL.getReconM(yrmo);
            DataSet dfrfDups =  getDFRFDups(yrmo);
            DataSet dsUnMatched = ReconDAL.getRFDFUM(yrmo);
            DataRow rowNew;
            DataSet dsDFRFTotal = new DataSet();
            DataTable tempTable, newTable;
            tempTable = dsMatched.Tables[0];
            newTable = dsDFRFTotal.Tables.Add("newTable1");
            DataColumn col;
            col = new DataColumn("Type"); newTable.Columns.Add(col);
            col = new DataColumn("RecordCount"); newTable.Columns.Add(col);
            col = new DataColumn("DupCount"); newTable.Columns.Add(col);
            col = new DataColumn("TotalCount"); newTable.Columns.Add(col);

            int _grandTotal = 0;

            rowNew = newTable.NewRow();
            rowNew["Type"] = "DFRF Records Summary";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = "";
            newTable.Rows.Add(rowNew);

            rowNew = newTable.NewRow();
            rowNew["Type"] = "Matched without Dups";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            _grandTotal = dsMatched.Tables[0].Rows.Count - getDupsCountDFRFinMatched(yrmo, " > 1");
            rowNew["TotalCount"] = _grandTotal;
            newTable.Rows.Add(rowNew);

            List<int> _dupCnt = new List<int>();
            _dupCnt = getRecordCountDFRFDup(yrmo);
            foreach (int x in _dupCnt)
            {
                int _temp = getDupsCountDFRFinMatched(yrmo, " = " + x);
                if (_temp != 0)
                {
                    rowNew = newTable.NewRow();
                    rowNew["Type"] = "Dups on Matched Report";
                    rowNew["RecordCount"] = x;
                    rowNew["DupCount"] = _temp;
                    rowNew["TotalCount"] = (x * _temp);
                    _grandTotal = _grandTotal + (x * _temp);
                    newTable.Rows.Add(rowNew);
                }
                
            }

            rowNew = newTable.NewRow();
            rowNew["Type"] = "UnMatched";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = dsUnMatched.Tables[0].Rows.Count;
            _grandTotal = _grandTotal + dsUnMatched.Tables[0].Rows.Count;
            newTable.Rows.Add(rowNew);

            rowNew = newTable.NewRow();
            rowNew["Type"] = "Grand Total:";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = _grandTotal;
            newTable.Rows.Add(rowNew);

            rowNew = newTable.NewRow();
            rowNew["Type"] = "";
            rowNew["RecordCount"] = "";
            rowNew["DupCount"] = "";
            rowNew["TotalCount"] = "";
            newTable.Rows.Add(rowNew);

            return dsDFRFTotal;
        }

        protected static DataSet getDFRFDups(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            string cmdStr = "SELECT count(rfdf_dcn), rfdf_dcn FROM RFDF "
                            + " WHERE rfdf_yrmo = '" + yrmo + "' AND rfdf_source = 'RF' "
                            + " GROUP BY rfdf_dcn "
                            + " HAVING COUNT(rfdf_dcn) > 1 ";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected static DataSet getDFDups(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            string cmdStr = "SELECT count(anbt_claimid), anbt_claimid FROM AnthBillTrans "
                            + " WHERE anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT' "
                            + " GROUP BY anbt_claimid "
                            + " HAVING COUNT(anbt_claimid) > 1";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected static Int32 getDupsCountDFRFinMatched(string yrmo, string _recCount)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            int _count = 0;
            string cmdStr = "select  a.anbt_yrmo, "
                            + " a.anbt_claimid,Convert(Numeric(10,2),SUM(a.anbt_claimsPdAmt)) AS claimsAmt , "
                            + " Convert(Numeric(10,2),rfAmt) AS rfAmt ,Convert(Numeric(10,2),(SUM(a.anbt_claimsPdAmt) - rfamt)) diff  "
                            + " FROM AnthBillTrans a "
                            + " INNER JOIN "
                            + " ( "
                            + " SELECT rfdf_dcn dcn,sum(rfdf_amt) rfAmt,rfdf_yrmo rfyrmo,rfdf_source rfsource "
                            + " FROM RFDF "
                            + " WHERE rfdf_yrmo = '" + yrmo + "' "
                            + " AND rfdf_cleared = 0 "
                            + " AND rfdf_source = 'RF' "
                            + " GROUP BY rfdf_dcn,rfdf_yrmo,rfdf_source "
                            + "  ) b "
                            + " ON (a.anbt_claimid = dcn) "
                            + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo = '" + yrmo + "' "
                            + " AND anbt_claimid IN "
                            + " ( "
                            + " SELECT  rfdf_dcn FROM RFDF "
                            + " WHERE rfdf_yrmo = '" + yrmo + "' AND rfdf_source = 'RF' "
                            + " GROUP BY rfdf_dcn "
                            + " HAVING COUNT(rfdf_dcn) " + _recCount + " "
                            + " ) "
                            + " GROUP BY a.anbt_yrmo,a.anbt_claimid,rfAmt "
                            + " ORDER BY diff";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                _count = ds.Tables[0].Rows.Count;
                return _count;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected static List<int> getRecordCountDFRFDup(string yrmo)
        {
            List<int> cnt = new List<int>();
            string cmdStr = " SELECT DISTINCT DFRFCnt "
                            + " FROM "
                            + " ( "
                            + " SELECT count(rfdf_dcn) AS DFRFCnt, rfdf_dcn FROM RFDF "
                            + " WHERE rfdf_yrmo = '" + yrmo + "' AND rfdf_source = 'RF' "
                            + " GROUP BY rfdf_dcn "
                            + " HAVING COUNT(rfdf_dcn) > 1 ) AS dupDFRFTable";

            SqlDataReader reader = null;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cnt.Add(Convert.ToInt32(reader[0]));
                }
                return cnt;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected static Int32 getDupsCountDFinMatched(string yrmo, string _recCount)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            int _count = 0;
            string cmdStr = "select  a.anbt_yrmo, "
                            + " a.anbt_claimid,Convert(Numeric(10,2),SUM(a.anbt_claimsPdAmt)) AS claimsAmt , "
                            + " Convert(Numeric(10,2),rfAmt) AS rfAmt ,Convert(Numeric(10,2),(SUM(a.anbt_claimsPdAmt) - rfamt)) diff  "
                            + " FROM AnthBillTrans a "
                            + " INNER JOIN "
                            + " ( "
                            + " SELECT rfdf_dcn dcn,sum(rfdf_amt) rfAmt,rfdf_yrmo rfyrmo,rfdf_source rfsource "
                            + " FROM RFDF "
                            + " WHERE rfdf_yrmo = '" + yrmo + "' "
                            + " AND rfdf_cleared = 0 "
                            + " AND rfdf_source = 'RF' "
                            + " GROUP BY rfdf_dcn,rfdf_yrmo,rfdf_source "
                            + "  ) b "
                            + " ON (a.anbt_claimid = dcn) "
                            + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo = '" + yrmo + "' "
                            + " AND anbt_claimid IN "
                            + " ( "
                            + " SELECT  anbt_claimid FROM AnthBillTrans "
                            + " WHERE anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT' "
                            + " GROUP BY anbt_claimid "
                            + " HAVING COUNT(anbt_claimid) " + _recCount + " "
                            + " ) "
                            + " GROUP BY a.anbt_yrmo,a.anbt_claimid,rfAmt "
                            + " ORDER BY diff";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                _count = ds.Tables[0].Rows.Count;
                return _count;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected static List<int> getRecordCountDFDup(string yrmo)
        {
            List<int> cnt = new List<int>();
            string cmdStr = " SELECT DISTINCT DFCnt "
                            + " FROM "
                            + " ( "
                            + " SELECT count(anbt_claimid) AS DFCnt, anbt_claimid FROM AnthBillTrans "
                            + " WHERE anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT' "
                            + " GROUP BY anbt_claimid "
                            + " HAVING COUNT(anbt_claimid) > 1 ) AS dupDFTable";

            SqlDataReader reader = null;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cnt.Add(Convert.ToInt32(reader[0]));
                }
                return cnt;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }
    }
}

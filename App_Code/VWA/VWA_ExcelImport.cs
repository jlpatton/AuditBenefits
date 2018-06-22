using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlTypes;
using System.Globalization;
using System.Data.OleDb;

namespace EBA.Desktop.VWA
{
    public class VWA_ExcelImport
    {
        public VWA_ExcelImport()
        {
           
        }

        public DataSet importRemittance(string _file, string source)
        {
            DataSet impData = new DataSet(); impData.Clear();
            DataTable dt = new DataTable(source);
            impData.Tables.Add(ConvertRangeXLS(_file, dt, "name", 0));
            return impData;
        }

        public int parseRemittance(DataSet ds, string _yrmo, string source)
        {
            string benName, ssn, sts;
            string accDtStr, vwaCtrlStr, dtPostedStr, paidVWAStr, paidClientStr, vwaFeesStr, dueClientStr;
            SqlDateTime accDt, dtPosted;
            int vwaCtrl;
            Decimal paidVWA, paidClient, vwaFees, dueClient; 
            int _count = 0;
            int[] cols = {0, 1, 2, 3, 4, 5, 6, 7, 8, 10 };
            string _tableName = source;

            for (int i = 0; i < (ds.Tables[_tableName].Rows.Count -1); i++)
            {
                benName = ds.Tables[_tableName].Rows[i][cols[0]].ToString().Trim().Replace("'", "''");
                
                ssn = ds.Tables[_tableName].Rows[i][cols[1]].ToString().Trim();
                if (ssn.Contains("-")) { ssn = ssn.Replace("-", ""); }

                accDtStr = ds.Tables[_tableName].Rows[i][cols[2]].ToString().Trim();
                if (accDtStr != null && accDtStr != string.Empty) { accDt = Convert.ToDateTime(accDtStr); }
                else { accDt = SqlDateTime.Null; }                    

                vwaCtrlStr = ds.Tables[_tableName].Rows[i][cols[3]].ToString().Trim();
                if (vwaCtrlStr != null && vwaCtrlStr != string.Empty) { vwaCtrl = Convert.ToInt32(vwaCtrlStr); }
                else { vwaCtrl = 0; }                   

                dtPostedStr = ds.Tables[_tableName].Rows[i][cols[4]].ToString().Trim();                
                if (dtPostedStr != null && dtPostedStr != string.Empty) { dtPosted = Convert.ToDateTime(dtPostedStr); }
                else { dtPosted = SqlDateTime.Null; }                    

                paidVWAStr = ds.Tables[_tableName].Rows[i][cols[5]].ToString().Trim();
                if (paidVWAStr != null && paidVWAStr != string.Empty) { paidVWA = Decimal.Parse(paidVWAStr, NumberStyles.Currency); }
                else { paidVWA = 0; }                    

                paidClientStr = ds.Tables[_tableName].Rows[i][cols[6]].ToString().Trim();
                if (paidClientStr != null && paidClientStr != string.Empty) { paidClient = Decimal.Parse(paidClientStr, NumberStyles.Currency); }
                else { paidClient = 0; }                    

                vwaFeesStr = ds.Tables[_tableName].Rows[i][cols[7]].ToString().Trim();
                if (vwaFeesStr != null && vwaFeesStr != string.Empty) { vwaFees = Decimal.Parse(vwaFeesStr, NumberStyles.Currency); }
                else { vwaFees = 0; }                    

                dueClientStr = ds.Tables[_tableName].Rows[i][cols[8]].ToString().Trim();
                if (dueClientStr != null && dueClientStr != string.Empty) { dueClient = Decimal.Parse(dueClientStr, NumberStyles.Currency); }
                else { dueClient = 0; }                   

                sts = ds.Tables[_tableName].Rows[i][cols[9]].ToString().Trim();

                VWAImportDAL.insertRemittance(benName, ssn, accDt, vwaCtrl, dtPosted, paidVWA, paidClient, vwaFees, dueClient, sts, source, _yrmo);
                _count++;
            }
            
            return _count;
        }

        public DataTable ConvertRangeXLS(string File, DataTable TableName, string _firstCol, int emptyLines)
        {
            string _sheet, _range;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            DataColumn dc;
            String sconn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + File + ";" +
                                    "Extended Properties=\"Excel 8.0;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(sconn);

            try
            {
                conn.Open();
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables_Info, new object[] { null, null, null, "TABLE" });
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                    dc = dt.Columns["TABLE_NAME"];
                    _sheet = dr[dc, DataRowVersion.Current].ToString();
                    _sheet = _sheet.Replace("'", "");
                    _sheet = _sheet.Replace("$", "");
                    dt1 = fillTable(conn, _sheet + "$", dt1);
                    _range = getRange(dt1, _firstCol, emptyLines);
                    _sheet = _sheet + "$" + _range;
                    dtReturn = fillTable(conn, _sheet, TableName);
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dtReturn;
        }

        string getRange(DataTable dt, String firstCol, int emptyLines)
        {
            int lastRow = dt.Rows.Count;
            int lastCol = dt.Columns.Count;
            DataRow dr;
            DataColumn dc;
            int startRow = 0;
            string range;

            for (int i = 0; i < lastRow; i++)
            {
                dr = dt.Rows[i];
                for (int j = 0; j < lastCol; j++)
                {
                    if (String.Compare(dr[j].ToString().Trim().ToLower(), firstCol, true) == 0)
                    {
                        startRow = i;
                        i = lastRow;
                        break;
                    }
                }
            }

            startRow = startRow + 2 + emptyLines;

            range = "A" + (startRow) + ":" + ((char)(lastCol + 64)).ToString() + (lastRow + 1 + emptyLines);
            return range;
        }

        DataTable fillTable(OleDbConnection conn, string _sheet, DataTable TableName)
        {
            String squery = "Select * FROM [" + _sheet + "]";
            OleDbCommand cmd = new OleDbCommand(squery, conn);
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.SelectCommand = cmd;

            ad.FillSchema(TableName, SchemaType.Source);

            ad.Fill(TableName);
            conn.Close();
            ad.Dispose();
            return TableName;
        }
    }
}

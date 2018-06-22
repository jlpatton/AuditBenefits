using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.OleDb;
using System.Data.Common;
using System.Collections.Generic;

/// <summary>
/// Summary description for ConvertXLS
/// </summary>
/// 
namespace EBA.Desktop
{
    public class ConvertExcel
    {
        public ConvertExcel()
        {

        }

        public static DataTable ConvertXLS(string File, DataTable TableName)
        {
            string _sheet;
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            DataColumn dc;            
            String sconn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + File + ";" +
                                    "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(sconn);
            try
            {
                conn.Open();
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dr = dt.Rows[i];
                        dc = dt.Columns["TABLE_NAME"];
                        _sheet = dr[dc, DataRowVersion.Current].ToString();
                        _sheet = _sheet.Replace("'", "");
                        _sheet = _sheet.Replace("$", "");
                        _sheet = _sheet.Replace("_", "");
                        dtReturn = fillTable(conn, _sheet, TableName);
                        break;
                    }
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
        public static DataTable ConvertXLS(string File, DataTable TableName, string sheetName )
        {
            string _sheet ="";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            DataColumn dc;
            bool _found = false;
            String sconn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                "Data Source=" + File + ";" +
                                    "Extended Properties=\"Excel 8.0;IMEX=1\"";
            OleDbConnection conn = new OleDbConnection(sconn);
            try
            {
                conn.Open();
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dr = dt.Rows[i];
                        dc = dt.Columns["TABLE_NAME"];
                        _sheet = dr[dc, DataRowVersion.Current].ToString();
                        _sheet = _sheet.Replace("'", "");
                        _sheet = _sheet.Replace("$", "");
                        _sheet = _sheet.Replace("_", "");
                        if (_sheet.Contains(sheetName))
                        {
                            dtReturn = fillTable(conn, _sheet, TableName);
                            _found = true;
                            break;
                        }

                    }
                }
                if (!_found)
                {
                    conn.Close();
                    throw (new Exception("SheetName :" + sheetName + " not found in Report"));
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

        public static DataTable ConvertXLS(string File, DataTable TableName, string src, string _yrmo)
        {
            
                string _sheet;
                DataTable dt = new DataTable();
                DataTable dtReturn = new DataTable();
                DataRow dr;
                DataColumn dc;
                String sconn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                                    "Data Source=" + File + ";" +
                                        "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
                OleDbConnection conn = new OleDbConnection(sconn);
            try
            {
                conn.Open();
                dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dr = dt.Rows[i];
                        dc = dt.Columns["TABLE_NAME"];
                        _sheet = dr[dc, DataRowVersion.Current].ToString();
                        _sheet = _sheet.Replace("'", "");
                        _sheet = _sheet.Replace("$", "");
                       // if (_sheet.Contains(_yrmo))
                        // do not check for the YRMO in the sheet
                        //R.A 9/4/4009
                        {
                            dtReturn = fillTable(conn, _sheet, TableName);
                        }
                        //else
                        //{
                        //    conn.Close();
                        //    throw (new Exception("YRMO doesnt match with the File!"));
                        //}
                    }
                }
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return dtReturn;
        }

        private static DataTable fillTable(OleDbConnection conn, string _sheet, DataTable TableName)
        {           
            String squery = "Select * FROM [" + _sheet + "$]";
            if (TableName.TableName.ToString().Equals("rxTable"))
            {
                squery = "Select GROUP_NBR,PAYMENT FROM [" + _sheet + "$] WHERE GROUP_NBR = 'Grand Total'";
            }
            OleDbCommand cmd = new OleDbCommand(squery, conn);
            OleDbDataAdapter ad = new OleDbDataAdapter();
            ad.SelectCommand = cmd;
            ad.Fill(TableName);
            conn.Close();
            return TableName;
        }
    }
}


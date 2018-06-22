using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

namespace EBA.Desktop.HRA
{
public class HRAConvertExcel
{
	public HRAConvertExcel()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DataTable ConvertXLS(string File, DataTable TableName)
        {
            
                string _sheet;
                DataTable dt = new DataTable();
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
                        _sheet = _sheet + "$";
                        dtReturn = fillTable(conn, _sheet, TableName);
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
                dt1= fillTable(conn, _sheet + "$", dt1);
                _range = getRange(dt1, _firstCol, emptyLines);
                //_sheet = _sheet + "$" + _range;       
                //5/7/2009 Changed the above statement to
                //include just the range and not the _sheet name
               dtReturn = fillTable(conn, _range, TableName);
                
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
                if (String.Compare(dr[j].ToString().Trim(), firstCol, true) == 0)
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
    
    }
}

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
using System.Text.RegularExpressions;

public class HRATextImport
{
	public HRATextImport()
	{
	}

    public static DataSet getTextFileData(string _file, string tablename, char delimiter)
    {
        string line = "";
        string[] fields = null;
        DataSet ds = new DataSet(); ds.Clear();
        DataTable dt = new DataTable(tablename);
        DataRow row;
        int i;
        
        StreamReader sr = new StreamReader(_file);
        try
        {
            // first line has headers
            line = sr.ReadLine();

            if (line == null)
                throw new Exception("Text file is not in valid format.<br/>First line doesn't have headers.");

            fields = line.Split(delimiter);

            foreach (string s in fields)
            {
                dt.Columns.Add(s);
            }

            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();
                fields = line.Split(delimiter);
                i = 0;

                if (fields[0] == null || fields[0].Trim() == String.Empty)
                    continue;
                foreach (string s in fields)
                {
                    row[i] = s.Replace("\"", String.Empty);
                    i++;
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);

            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sr.Close();
        }
    }

    public static string getTextFileDataNoHeader(string _file,char delimiter)
    {
        string line = "";
        int field = 0;
        DataRow row;
        int i;
        string _final = "";
        string _pattern = @"^\d{1,10}$";

        TextReader reader = new StreamReader(File.OpenRead(_file));
        try
        {
            while ((line = reader.ReadLine()) != null)
            {
                Match parsed = Regex.Match(line, _pattern);
                if (parsed.Success)
                {
                    field = Convert.ToInt32(line);
                    if (field == 0)
                        continue;
                    _final = _final + field.ToString() + ",";
                }
            }

            if (_final.Length > 0)
            {
                _final = _final.Remove(_final.Length - 1);
            }
            return _final;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            reader.Close();
        }
    }
}

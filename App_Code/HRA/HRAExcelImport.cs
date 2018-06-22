using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace EBA.Desktop.HRA
{
    public class HRAExcelImport
    {
        private DataSet impData = null;
        HRAConvertExcel iobj = new HRAConvertExcel();

        public HRAExcelImport()
        {
            impData = new DataSet();
        }

        public DataSet importPutnam(string _file)
        {
            DataTable dt = new DataTable("PutnamTable");
            impData.Tables.Add(iobj.ConvertRangeXLS(_file, dt, "First Name", 0));
            return impData;
        }

        public DataSet getWgwkInvData(string _file, string tablename)
        {
            DataTable dt = new DataTable(tablename);
            impData.Tables.Add(iobj.ConvertRangeXLS(_file, dt, "Last Name", 1));
            return impData;
        }
        public DataSet getCignaAdminFeeBillData(string _file, string tablename)
        {
            ///wire access - Importing Cigna Fee Bill Dat
            //DataTable dt = new DataTable(tablename);
            //impData.Tables.Add(iobj.ConvertRangeXLS(_file, dt, "Account Number", 9));
            //return impData;
            int _counter = 0;
            HRAConvertExcel tObj = new HRAConvertExcel();
            HRAParseData pObj = new HRAParseData();
            DataSet ds = new DataSet(); ds.Clear();
            DataTable dt = new DataTable("CignaAdminFeeTable");

            ds.Tables.Add(tObj.ConvertRangeXLS(_file, dt, "Account Number", 0));
            //_counter = pObj.parsePutnamPartData(ds, _filepath, _source, _qy);

            return ds;
        }

        public DataSet getExcelData(string _file, string tablename)
        {
            DataTable dt = new DataTable(tablename);
            impData.Tables.Add(iobj.ConvertXLS(_file, dt));
            return impData;
        }

        public void ConfirmPutnamYRMO(string _file, string _yrmo)
        {
            String _pattern = @"(?<Date1>((0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](20)\d\d))\s+TO\s+(?<Date2>((0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](20)\d\d))";
            Match _match;
            DateTime dt1, dt2;
            DateTime yrmodt = Convert.ToDateTime(_yrmo.Insert(4, "/"));
            Boolean _found = false;
            DataTable dt = new DataTable("putnamYrmo");

            impData.Tables.Add(iobj.ConvertXLS(_file, dt));
            foreach (DataRow row in impData.Tables["putnamYrmo"].Rows)
            {
                for (int col = 0; col < impData.Tables["putnamYrmo"].Columns.Count; col++)
                {                
                    if (row[col].ToString().ToUpper().Contains("DATE RANGE:"))
                    {
                        _found = true;
                        
                        _match = Regex.Match(row[col+1].ToString().ToUpper(), _pattern);
                        if (_match.Success)
                        {
                            dt1 = Convert.ToDateTime(_match.Groups["Date1"].Value);
                            dt2 = Convert.ToDateTime(_match.Groups["Date2"].Value);

                            if ((dt1.Month != dt2.Month) || (dt1.Year != dt2.Year) || (yrmodt.Month != dt1.Month) || (yrmodt.Year != dt1.Year))
                                throw new Exception("Spreadsheet periods from: " + _match.Groups["Date1"].Value + " through " + _match.Groups["Date2"].Value + " do not match Year-Month(" + _yrmo + ") entered!");
                            else return;

                        }
                        else throw new Exception("Spreadsheet does not contain a valid date range! Process halted!");

                    }
                }
            }
            if(!_found) throw new Exception("Spreadsheet does not contain a date range! Process halted!");
        }

        public void ConfirmWageworkYRMO(string _yrmo, DataSet ds, string tablename)
        {
            DateTime yrmodt = Convert.ToDateTime(_yrmo.Insert(4, "/"));
            DateTime createdt;
            foreach (DataRow row in ds.Tables[tablename].Rows)
            {
                createdt = Convert.ToDateTime(row[8]);
                if ((yrmodt.Month != createdt.Month) || (yrmodt.Year != createdt.Year))
                    throw new Exception("Spreadsheet Create Date(s) do not match Year-Month("+ _yrmo + ") entered!");
            }
        }

        public void ConfirmPutnamAdjYRMO(string _yrmo, DataSet ds, string tablename)
        {
            DateTime yrmodt = Convert.ToDateTime(_yrmo.Insert(4, "/"));
            DateTime distdt;
            foreach (DataRow row in ds.Tables[tablename].Rows)
            {
                distdt = Convert.ToDateTime(row[2]);
                if ((yrmodt.Month != distdt.Month) || (yrmodt.Year != distdt.Year))
                    throw new Exception("Spreadsheet Distribution Date(s) do not match Year-Month(" + _yrmo + ") entered!");
            }
        }
    }
}
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

namespace EBA.Desktop.VWA
{
    public class VWA
    {
        public VWA()
        {
        }

        public static string GetInputFileName(string yrmo, string source)
        {
            string inputFileName = "";

            switch (source)
            {
                case "BankStat":
                    inputFileName = yrmo + "_vwaboa.txt";
                    break;
                case "RemitOC":
                    inputFileName = yrmo + "_vwaremit_OldCases.xls";
                    break;
                case "RemitCigna":
                    inputFileName = yrmo + "_vwaremit_Cigna.xls";
                    break;
                case "RemitUHC":
                    inputFileName = yrmo + "_vwaremit_UHC.xls";
                    break;
                case "RemitDisab":
                    inputFileName = yrmo + "_vwaremit_Disability.xls";
                    break;
                case "RemitAnth":
                    inputFileName = yrmo + "_vwaremit_Anthem.xls";
                    break;
                case "TranDtl":
                    inputFileName = yrmo + "_vwa.txt";
                    break;
            }

            return inputFileName;
        }

        public static string getPrevYRMO(int months)
        {
            DateTime prevmondt = DateTime.Today.AddMonths(-months);
            string prevyear = prevmondt.Year.ToString();
            string prevmonth = prevmondt.Month.ToString();
            if (prevmonth.Length == 1)
                prevmonth = "0" + prevmonth;
            string prevYRMO = prevyear + prevmonth;

            return prevYRMO;
        }

        public static string getPrevYRMO(string yrmo)
        {
            yrmo = yrmo.Insert(4, "/");
            DateTime prevYRMO = Convert.ToDateTime(yrmo).AddMonths(-1);
            string prev_year = prevYRMO.Year.ToString();
            string prev_month = prevYRMO.Month.ToString();
            if (prev_month.Length == 1)
                prev_month = "0" + prev_month;
            string prev_yrmo = prev_year + prev_month;

            return prev_yrmo;
        }

        public static DateTime GetLastDayofYRMO(string yrmo)
        {
            yrmo = yrmo.Substring(0, 4) + "/" + yrmo.Substring(4);
            int _month = Convert.ToDateTime(yrmo).Month;
            int _year = Convert.ToDateTime(yrmo).Year;
            int numberOfDays = DateTime.DaysInMonth(_year, _month);

            DateTime lastDay = new DateTime(_year, _month, numberOfDays);

            return lastDay;
        }       

        public static string GetRemitClientCd(string sourcecd)
        {
            string _result="";

            switch (sourcecd)
            {
                case "RemitOC":
                    _result = "Old Cases 81800";
                    break;
                case "RemitCigna":
                    _result = "Cigna 81801";
                    break;
                case "RemitUHC":
                    _result = "UHC 80802";
                    break;
                case "RemitDisab":
                    _result = "Disabilty 80803";
                    break;
                case "RemitAnth":
                    _result = "Anthem 80804";
                    break;                   
            }

            return _result;
        }

        public static string replaceIllegalXMLCharacters(string _cnt)
        {
            _cnt = _cnt.Replace("“", "&#8220;");
            _cnt = _cnt.Replace("”", "&#8221;");
            _cnt = _cnt.Replace("‘", "&#8216;");
            _cnt = _cnt.Replace("’", "&#8217;");
            _cnt = _cnt.Replace("'", "&apos;");
            return _cnt;
        }

        public static string GetYRMO(DateTime dt)
        {
            string yrmo;

            int _month = dt.Month;
            int _year = dt.Year;

            if (_month < 10) { yrmo = _year + "0" + _month; }
            else { yrmo = _year.ToString() + _month.ToString(); }

            return yrmo;
        }

        public static string formatSSN(string _type, string _ssn)
        {
            string _regex = @"^\d{1,9}$";
            string _regex1 = @"^\d{3}-\d{2}-d{4}$";
            string _finalssn = "";
            Match _p1 = Regex.Match(_ssn, _regex);
            Match _p2 = Regex.Match(_ssn, _regex1);
            
            switch (_type)
            {
                case "secure":
                    if (_p1.Success)
                    {
                        _finalssn = "***-**-" + _ssn.Substring(5);
                    }
                    else if (_p2.Success)
                    {
                        _finalssn = "***-**-" + _ssn.Substring(7);
                    }
                    break;
                case "dashed":
                    if (_p1.Success)
                    {
                        _finalssn = _ssn.Substring(0, 3) + "-" + _ssn.Substring(3, 2) + "-" + _ssn.Substring(5, 4);
                    }
                    else if (_p2.Success)
                    {
                        _finalssn = _ssn;
                    }
                    break;
                case "removedash":
                    if (_p1.Success)
                    {
                        _finalssn = _ssn;
                    }
                    else if (_p1.Success)
                    {
                        _finalssn = _ssn.Replace("-", "");
                    }
                    break;
            }
            return _finalssn;                
        }
    }
}
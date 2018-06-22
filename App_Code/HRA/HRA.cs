using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EBA.Desktop.HRA
{
    public class HRA
    {
        public HRA()
        {
        }

        public string getPrevYRMO(int months)
        {
            DateTime prevmondt = DateTime.Today.AddMonths(-months);
            string prevyear = prevmondt.Year.ToString();
            string prevmonth = prevmondt.Month.ToString();
            if (prevmonth.Length == 1)
                prevmonth = "0" + prevmonth;
            string prevYRMO = prevyear + prevmonth;

            return prevYRMO;
        }

        public string getPrevQY(int i)
        {
            int quarter;
            if (i == 0)
                quarter = 0;
            else
                quarter = i*3;
            DateTime quarterdt = DateTime.Today.AddMonths(-quarter);
            string prevyear = quarterdt.Year.ToString();
            string prevquarter = GetQuarter(quarterdt.Month);
            string prevQY = prevquarter + "-" + prevyear;

            return prevQY;
        }

        public string getPrevYRMO(string yrmo)
        {            
            String temp = yrmo.Substring(0, 4) + "/" + yrmo.Substring(4);
            DateTime yrmodt = Convert.ToDateTime(temp);
            DateTime prev_yrmodt = yrmodt.AddMonths(-1);
            String prevYear = prev_yrmodt.Year.ToString();
            String prevMonth = prev_yrmodt.Month.ToString();
            if (prevMonth.Length == 1)
                prevMonth = "0" + prevMonth;

            return (prevYear + prevMonth);
        }

        string GetQuarter(int nMonth)
        {
            if (nMonth <= 3)
                return "Q1";

            if (nMonth <= 6)
                return "Q2";

            if (nMonth <= 9)
                return "Q3";

            return "Q4";

        }

        public static string GetQuarterYear(string yrmo)
        {
            int _month = Convert.ToInt32(yrmo.Substring(4));
            
            if (_month <= 3)
                return "Q1-" + yrmo.Substring(0,4);

            if (_month <= 6)
                return "Q2-" + yrmo.Substring(0, 4);

            if (_month <= 9)
                return "Q3-" + yrmo.Substring(0, 4);

            return "Q4-" + yrmo.Substring(0, 4);
        }

        public static string GetQuarterYear(DateTime _dt)
        {
            int _month = _dt.Month;

            if (_month <= 3)
                return "Q1-" + _dt.Year;

            if (_month <= 6)
                return "Q2-" + _dt.Year;

            if (_month <= 9)
                return "Q3-" + _dt.Year;

            return "Q4-" + _dt.Year;
        }

        public static string[] GetYRMOs(string _qy)
        {
            string[] YRMOs;
            string quarter = _qy.Substring(0, 2);
            string year = _qy.Substring(3);

            switch (quarter)
            {
                case "Q1":
                    YRMOs = new string[] { year + "01", year + "02", year + "03" };                    
                    break;
                case "Q2":
                    YRMOs = new string[] { year + "04", year + "05", year + "06" };
                    break;
                case "Q3":
                    YRMOs = new string[] { year + "07", year + "08", year + "09" };
                    break;
                default:
                    YRMOs = new string[] { year + "10", year + "11", year + "12" };
                    break;
            }

            return YRMOs;
        }

        public string GetYRMOformated(string yrmo)
        {
            string yy = yrmo.Substring(2, 2);
            string month = yrmo.Substring(4);
            string yrmoformated;

            switch (month)
            {
                case "01":
                    yrmoformated = "Jan-" + yy;
                    break;
                case "02":
                    yrmoformated = "Feb-" + yy;
                    break;
                case "03":
                    yrmoformated = "Mar-" + yy;
                    break;
                case "04":
                    yrmoformated = "Apr-" + yy;
                    break;
                case "05":
                    yrmoformated = "May-" + yy;
                    break;
                case "06":
                    yrmoformated = "Jun-" + yy;
                    break;
                case "07":
                    yrmoformated = "Jul-" + yy;
                    break;
                case "08":
                    yrmoformated = "Aug-" + yy;
                    break;
                case "09":
                    yrmoformated = "Sep-" + yy;
                    break;
                case "10":
                    yrmoformated = "Oct-" + yy;
                    break;
                case "11":
                    yrmoformated = "Nov-" + yy;
                    break;
                default:
                    yrmoformated = "Dec-" + yy;
                    break;
            }

            return yrmoformated;
        }

        public string GetYRMO(string period)
        {
            string yyyy = "20" + period.Substring(4);
            string month = period.Substring(0, 3);
            string yrmo;

            switch (month)
            {
                case "Jan":
                    yrmo = yyyy + "01" ;
                    break;
                case "Feb":
                    yrmo = yyyy + "02";
                    break;
                case "Mar":
                    yrmo = yyyy + "03";
                    break;
                case "Apr":
                    yrmo = yyyy + "04";
                    break;
                case "May":
                    yrmo = yyyy + "05";
                    break;
                case "Jun":
                    yrmo = yyyy + "06";
                    break;
                case "Jul":
                    yrmo = yyyy + "07";
                    break;
                case "Aug":
                    yrmo = yyyy + "08";
                    break;
                case "Sep":
                    yrmo = yyyy + "09";
                    break;
                case "Oct":
                    yrmo = yyyy + "10";
                    break;
                case "Nov":
                    yrmo = yyyy + "11";
                    break;
                default:
                    yrmo = yyyy + "12";
                    break;
            }

            return yrmo;
        }

        public string GetMaxQuarterYRMO(string _qy)
        {
            string maxyrmo;
            string quarter = _qy.Substring(0, 2);
            string yyyy = _qy.Substring(3, 4);
            switch (quarter)
            {
                case "Q1":
                    maxyrmo = yyyy + "03";
                    break;
                case "Q2":
                    maxyrmo = yyyy + "06";
                    break;
                case "Q3":
                    maxyrmo = yyyy + "09";
                    break;
                default:
                    maxyrmo = yyyy + "12";
                    break;
            }

            return maxyrmo;
        }

        public static string GetInputFileName(string period, string source)
        {
            string inputFileName = "";

            switch(source)
            {
                case "Putnam":
                    inputFileName = "Putnam_" + period + ".xls";
                    break;
                case "PutnamAdj":
                    inputFileName = period + " Putnam Adj" + ".xls";
                    break;
                case "Wageworks":
                    inputFileName = "Transaction_" + period + ".xls"; 
                    break;
                case "SOFO":
                    inputFileName = period + " SOFO" + ".csv";
                    break;
                case "ptnm_invoice":
                    inputFileName = "Invoice " + period.Replace("-", " ") + ".doc";
                    break;
                case "wgwk_invoice":
                    inputFileName = "Wageworks " + period + ".xls";
                    break;
                case "ptnm_partdata":
                    inputFileName = "Putnam " + period.Replace("-", " ") + ".xls";
                    break;               
            }

            return inputFileName;
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

        public static string GetProperCase(string strName)
        {
            string strproper = "";

            if (strName != null && strName.Length > 1)
            {
                strproper = strName.Substring(0, 1).ToUpper() + strName.Substring(1).ToLower();
            }

            return strproper;
        }

        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }

            return result;
        }
    }
}
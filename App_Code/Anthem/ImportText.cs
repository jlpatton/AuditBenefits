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
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;

/// <summary>
/// Summary description for ImportText
/// </summary>
/// 

namespace EBA.Desktop.Anthem
{
    public class ImportText
    {
        public ImportText()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void ParseGRS(string _fileName)
        {
            char[] _reportName      = new char[] { 'F', 'G', 'J' };
            string _patternLineText1=
                @"1REPORT NUMBER\s+";
            string _patternLineText2=
                @"\s+[\(HDCTACT\)]*";
            string _patternDate =
                @"\s+?(?<month>(JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER))[ \t]*(?<year>\d{4})";
            string _patternCoverageType =
                @"\s+(?<coverage>PILOT\s+(-)\s+[A-Za-z\s0-9]+)$";
            /* 
             * Report will be parsed with _patternLineText1 + _reportName + _patternLineText2 + _patternDate + _patternCoverageType
             * to Find the reports by name 'F', 'G', 'H', month, year and Coverage type
            */


            string _patternRecord   =
                @"^\s?0?\*+?\s+(?<Description>TOTAL\s+BY\s+[A-Za-z]+)\s+(?<code>\w+?)\s+(?<type>COUNT)\s+(?<value>\d+)$";
            string _patternEnd      =
                @"^0?\s+(GRAND\s+TOTAL)\s+(COUNT)\s+\d+$";
            
            string _month       = null;
            string _year        = null;
            string _coverage    = null;
            string _desc        = null;
            string _code        = null;
            string _type        = null;
            string _value       = null;
            string _yrmo        = null;            
            TextReader reader   = new StreamReader(File.OpenRead(_fileName));            
            string line         = string.Empty;            

            while ((line = reader.ReadLine()) != null)
            {
                foreach (char ch in _reportName)
                {
                    string _patternStart    = _patternLineText1 + ch + _patternLineText2 + _patternDate + _patternCoverageType;
                    Match parseStart        = Regex.Match(line, _patternStart);

                    if (parseStart.Success)
                    {
                        _month      = parseStart.Groups["month"].Value.ToString();
                        _year       = parseStart.Groups["year"].Value.ToString();
                        _coverage   = parseStart.Groups["coverage"].Value.ToString();
                        _yrmo       = calculateYRMO(_month, _year);
                        
                        while ((line = reader.ReadLine()) != null)
                        {
                            Match parseRecord   = Regex.Match(line, _patternRecord);
                            Match parseStop     = Regex.Match(line, _patternEnd);

                            if (parseRecord.Success)
                            {
                                _desc   = parseRecord.Groups["Description"].Value.ToString();
                                _code   = parseRecord.Groups["code"].Value.ToString();
                                _type   = parseRecord.Groups["type"].Value.ToString();
                                _value  = parseRecord.Groups["value"].Value.ToString();
                                ImportTextDAL storeData = new ImportTextDAL();

                                try
                                {
                                    storeData.storeGRS(_desc, _code, _type, _value, _yrmo, _coverage);
                                }
                                catch (Exception ex)
                                {
                                    reader.Close();
                                    throw (new Exception("Error Importing GRS Report!"));
                                }
                            }
                            else if(parseStop.Success)
                            {
                                break;
                            }
                        }
                    }                    
                }                
            }
            reader.Close();

        }       

        static string calculateYRMO(string _month, string _year)
        {
            string _yrmo = null;
            switch (_month)
            {
                case "JANUARY"  : _yrmo = _year + "01";
                    break;
                case "FEBRUARY" : _yrmo = _year + "02";
                    break;
                case "MARCH"    : _yrmo = _year + "03";
                    break;
                case "APRIL"    : _yrmo = _year + "04";
                    break;
                case "MAY"      : _yrmo = _year + "05";
                    break;
                case "JUNE"     : _yrmo = _year + "06";
                    break;
                case "JULY"     : _yrmo = _year + "07";
                    break;
                case "AUGUST"   : _yrmo = _year + "08";
                    break;
                case "SEPTEMBER": _yrmo = _year + "09";
                    break;
                case "OCTOBER"  : _yrmo = _year + "10";
                    break;
                case "NOVEMBER" : _yrmo = _year + "11";
                    break;
                case "DECEMBER" : _yrmo = _year + "12";
                    break;
            }
            return _yrmo;
        }

        public void ParseADP(string _fileName)
        {
            string _PatternDate =
                @"(?<month>\d{1,2})\/(?<year>\d{4})[ \t]*\*(All Plans)";
            string _Pattern     =
                @"^[ \t](?<text1>H - Medical)\s+(?<plan>[A-Z0-9]+)\s+(?<desc>([A-Za-z-.'()]+\s?)*)\s+(?<division>(\w{5})*)\s+(?<bensts>[A-Z]*)\s+(?<covlvl>[A-Z]*)\s+(?<empno>[\d]*)\s+(?<year>\d{4})\s+(?<month>\d{2})$";

            int _count      = 0;
            string _yrmo    = null;
            string _text1   = null;
            string _plan    = null;
            string _desc    = null;
            string _div     = null;
            string _bensts  = null;
            string _covlvl  = null;
            string _empno   = null;
            string _year    = null;
            string _month   = null;
            string line     = string.Empty;

            TextReader reader   = new StreamReader(File.OpenRead(_fileName));
            Regex r             = new Regex(_Pattern);

            while ((line = reader.ReadLine()) != null && _count == 0)
            {
                Match parsed = Regex.Match(line, _PatternDate);
                if (parsed.Success)
                {                   
                    string _pyear= parsed.Groups["year"].Value.ToString();
                    int _pmonth  = Int32.Parse(parsed.Groups["month"].Value.ToString());

                    string _monthnew;
                    if (_pmonth < 10)
                    {
                        _monthnew = "0" + Convert.ToString(_pmonth);
                    }
                    else
                    {
                        _monthnew = Convert.ToString(_pmonth);
                    }
                    _yrmo = _pyear + _monthnew;
                    _count++;
                }
            }            
            while ((line = reader.ReadLine()) != null)
            {                
                Match parsed = r.Match(line);
                if (parsed.Success)
                {
                    _text1  = parsed.Groups["text1"].Value.ToString();                    
                    _plan   = parsed.Groups["plan"].Value.ToString();
                    _desc   = parsed.Groups["desc"].Value.ToString();                    
                    _div    = parsed.Groups["division"].Value.ToString();                    
                    _bensts = parsed.Groups["bensts"].Value.ToString();                    
                    _covlvl = parsed.Groups["covlvl"].Value.ToString();                    
                    _empno  = parsed.Groups["empno"].Value.ToString();                   
                    _year   = parsed.Groups["year"].Value.ToString();                   
                    _month  = parsed.Groups["month"].Value.ToString();
                    ImportTextDAL storeData = new ImportTextDAL();
                    try
                    {
                        storeData.storeADP(_text1, _plan, _desc, _div, _bensts, _covlvl, _empno, _year, _month);
                    }
                    catch(Exception ex)
                    {
                        reader.Close();
                        throw (new Exception("Error Importing ADP Cobra Report!"));
                    }
                }

            }
            reader.Close();
        }

        public void parseBOA(string _fileName)
        {
            string _PatternDate =
                @"(Statement Period)\s+(?<month1>\d{1,2})\/(?<date1>\d{1,2})\/(?<year1>\d{4})\s+(-)\s+(?<month2>\d{1,2})\/(?<date2>\d{1,2})\/(?<year2>\d{4})";
            string _Pattern     = "^\"?" +
                @"\s+(?<chqno1>\d+\*?)\s+(?<amnt1>\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?))\s+(?<dt1>\d{1,2}\/\d{1,2})\s+(?<ref1>\d+)\s+(?<chqno2>\d+\*?)\s+(?<amnt2>\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?))\s+(?<dt2>\d{1,2}\/\d{1,2})\s+(?<ref2>\d+)\s+" +
                    "\"";
            string _Pattern1    = "^\"?" +
                @"\s+(?<chqno1>\d+\*?)\s+(?<amnt1>\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?))\s+(?<dt1>\d{1,2}\/\d{1,2})\s+(?<ref1>\d+)\s+" +
                    "\"";

            int _count  = 0;
            string _yrmo= null;            
            string line = string.Empty;
            
            TextReader reader = new StreamReader(File.OpenRead(_fileName));

            Regex r = new Regex(_Pattern);
            Regex r1= new Regex(_Pattern1);

            while ((line = reader.ReadLine()) != null && _count == 0)
            {
                Match parsed = Regex.Match(line, _PatternDate);
                if (parsed.Success)
                {
                    string _month   = parsed.Groups["month2"].Value.ToString();
                    string _year    = parsed.Groups["year2"].Value.ToString();                    
                    _yrmo = _year + _month;                    
                    _count++;
                }
            }            
            try
            {
                while ((line = reader.ReadLine()) != null)
                {                    
                    Match parsed = r.Match(line);
                    if (parsed.Success)
                    {
                        insertBOA(parsed, _yrmo, "two");
                    }
                    else
                    {
                        Match parsed1 = r1.Match(line);
                        if (parsed1.Success)
                        {
                            insertBOA(parsed1, _yrmo, "one");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                reader.Close();
                throw (new Exception("Error Importing BOA Account Statement!"));
            }
            reader.Close();
        }

        private static void insertBOA(Match regexMatch, string _yrmo, string _type)
        {
            string _chq1    = null;
            string _chq2    = null;
            string _temp1   = null;
            string _temp2   = null;
            decimal _amnt1 ;
            decimal _amnt2;
            DateTime _pdate1, _pdate2;
            string _bref1   = null;
            string _bref2   = null;
            string _pflag1  = null; 
            string _pflag2  = null;            
            char[] _chArr   = { '*', ' ' };

            ImportTextDAL storeData = new ImportTextDAL();

            if (_type.Equals("two"))
            {
                _temp1  = (regexMatch.Groups["chqno1"].Value.ToString());
                _temp2  = (regexMatch.Groups["chqno2"].Value.ToString());
                _amnt1  = Decimal.Parse(regexMatch.Groups["amnt1"].Value.ToString(), System.Globalization.NumberStyles.Currency);
                _amnt2  = Decimal.Parse(regexMatch.Groups["amnt2"].Value.ToString(), System.Globalization.NumberStyles.Currency);
                _pdate1 = DateTime.Parse(regexMatch.Groups["dt1"].Value.ToString());
                _pdate2 = DateTime.Parse(regexMatch.Groups["dt2"].Value.ToString());
                _bref1  = (regexMatch.Groups["ref1"].Value.ToString());
                _bref2  = (regexMatch.Groups["ref2"].Value.ToString());
                if (_temp1.Contains("*"))
                {
                    _pflag1 = "*";
                    _chq1 = _temp1.TrimEnd(_chArr);
                }
                else
                {
                    _chq1 = _temp1;
                }
                if (_temp2.Contains("*"))
                {
                    _pflag2 = "*";
                    _chq2 = _temp2.TrimEnd(_chArr);
                }
                else
                {
                    _chq2 = _temp2;
                }
                try
                {
                    storeData.insertBOARecords(_chq1, _amnt1, _pdate1, _bref1, _yrmo, _pflag1);
                    storeData.insertBOARecords(_chq2, _amnt2, _pdate2, _bref2, _yrmo, _pflag2);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (_type.Equals("one"))
            {
                _temp1  = (regexMatch.Groups["chqno1"].Value.ToString());
                _amnt1  = Decimal.Parse(regexMatch.Groups["amnt1"].Value.ToString(), System.Globalization.NumberStyles.Currency);
                _pdate1 = DateTime.Parse(regexMatch.Groups["dt1"].Value.ToString());
                _bref1  = (regexMatch.Groups["ref1"].Value.ToString());
                if (_temp1.Contains("*"))
                {
                    _pflag1 = "*";
                    _chq1   = _temp1.TrimEnd(_chArr);
                }
                else
                {
                    _chq1 = _temp1;
                }
                storeData.insertBOARecords(_chq1, _amnt1, _pdate1, _bref1, _yrmo, _pflag1);
            }
        }

        public void importGRSRecords(string _yrmo)
        {
            List<int> _colMEDID = new List<int>();
            List<int> _colStateID = new List<int>();

            ImportTextDAL getData = new ImportTextDAL();
            _colMEDID = getData.getMedPartCols();

            string _grsMEDCode;
            string _grsStateCode;

            int _startMColID = 0;
            int _endMColID = 0;
            int _startSColID = 0;
            int _endSColID = 0;
            try
            {
                for (int i = 0; i < _colMEDID.Count; i++)
                {
                    _endMColID = _colMEDID[i];
                    _startMColID = _startMColID + 1;
                    _grsMEDCode = getData.getGRSCode(_endMColID);
                    _colStateID = getData.getStateCols(_startMColID, _endMColID);

                    _startSColID = _startSColID + 1;
                    for (int a = 0; a < _colStateID.Count; a++)
                    {
                        _endSColID = _colStateID[a];
                        DataSet ds = new DataSet();
                        _grsStateCode = getData.getGRSCode(_endSColID);
                        ds = getData.getGRSRows(_startSColID, _endSColID);
                        storeCovid(_grsMEDCode, _grsStateCode, ds, _yrmo);

                        _startSColID = _colStateID[a];
                    }

                    _startMColID = _colMEDID[i];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void storeCovid(string _medcode, string _statecode, DataSet ds, string _yrmo)
        {
            string _tiercode;
            int _count;
            string _desc;
            ImportTextDAL obj = new ImportTextDAL();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _tiercode = dr["grsCode"].ToString();
                _count = Int32.Parse(dr["grsValue"].ToString());
                _desc = dr["grsCoverage"].ToString();
                if (_medcode.Contains("P0") || _medcode.Contains("P1") || _medcode.Contains("P2") || _medcode.Contains("P3") || _medcode.Contains("P4") || _medcode.Contains("P5"))
                {
                    obj.InsertHCGRS(_medcode, _statecode, _tiercode, _desc, _count, _yrmo);
                }
                else
                {
                    obj.InsertHMOGRS(_medcode, _statecode, _tiercode, _count, _yrmo);
                }
            }
        }

        public string matchYrmoTxt(string _fileName, string _source)
        {
            string _yrmo ="";
            switch (_source)
            {
                case "ADP": _yrmo = getADPYRMO(_fileName);
                    break;
                case "GRS": _yrmo = getGRSYRMO(_fileName);
                    break;
                case "BOA": _yrmo = getBOAYRMO(_fileName);
                    break;
            }
            return _yrmo;
        }

        /// <summary>
        /// Takes Filename and returns the year-month for ADP Cobra File
        /// </summary>
        /// <param name="_fileName">Report</param>
        /// <returns>Year Month</returns>
        private string getADPYRMO(string _fileName)
        {
            string _PatternDate =
                @"(?<month>\d{1,2})\/(?<year>\d{4})[ \t]*\*(All Plans)";
            string _yrmo = null;
            string line = string.Empty;
            int _count = 0;
            TextReader reader = new StreamReader(File.OpenRead(_fileName));           

            while ((line = reader.ReadLine()) != null && _count == 0)
            {
                Match parsed = Regex.Match(line, _PatternDate);
                if (parsed.Success)
                {
                    string _pyear = parsed.Groups["year"].Value.ToString();
                    int _pmonth = Int32.Parse(parsed.Groups["month"].Value.ToString());

                    string _monthnew;
                    if (_pmonth < 10)
                    {
                        _monthnew = "0" + Convert.ToString(_pmonth);
                    }
                    else
                    {
                        _monthnew = Convert.ToString(_pmonth);
                    }
                    _yrmo = _pyear + _monthnew;
                    _count++;
                }
            }
            reader.Close();
            return _yrmo;
        }

        /// <summary>
        /// Takes Filename and returns the year-month for GRS Pilot
        /// </summary>
        /// <param name="_fileName">Report Name</param>
        /// <returns>Year Month</returns>
        private string getGRSYRMO(string _fileName)
        {
            string _patternDate =
               @"\s+?(?<month>(JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER))[ \t]*(?<year>\d{4})";
            string _month       = null;
            string _year        = null;           
            string _yrmo        = null;
            int _count = 0;

            TextReader reader   = new StreamReader(File.OpenRead(_fileName));            
            string line         = string.Empty;

            while ((line = reader.ReadLine()) != null && _count == 0)
            {
                Match parseStart = Regex.Match(line, _patternDate);
                if (parseStart.Success)
                {
                    _month = parseStart.Groups["month"].Value.ToString();
                    _year = parseStart.Groups["year"].Value.ToString();
                    _yrmo = calculateYRMO(_month, _year);
                    _count++;
                }
            }
            reader.Close();
            return _yrmo;
        }

        /// <summary>
        /// Takes Filename and returns the year-month for BOA Statement
        /// </summary>
        /// <param name="_fileName">Report Name</param>
        /// <returns>Year Month</returns>
        private string getBOAYRMO(string _fileName)
        {
            string _PatternDate =
                @"(Statement Period)\s+(?<month1>\d{1,2})\/(?<date1>\d{1,2})\/(?<year1>\d{4})\s+(-)\s+(?<month2>\d{1,2})\/(?<date2>\d{1,2})\/(?<year2>\d{4})";
            
            int _count = 0;
            string _yrmo = null;
            string line = string.Empty;

            TextReader reader = new StreamReader(File.OpenRead(_fileName));            

            while ((line = reader.ReadLine()) != null && _count == 0)
            {
                Match parsed = Regex.Match(line, _PatternDate);
                if (parsed.Success)
                {
                    string _month = parsed.Groups["month2"].Value.ToString();
                    string _year = parsed.Groups["year2"].Value.ToString();
                    _yrmo = _year + _month;
                    _count++;
                }
            }
            reader.Close();
            return _yrmo;
        }
            
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using EBA.Desktop.Anthem;

namespace EBA.Desktop.IPBA
{
    /// <summary>
    /// Summary description for IPBAImport
    /// </summary>
    public class IPBAImport
    {
        public IPBAImport()
        {

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

        public static string getPrevYRMO(int months, string yrmo)
        {
            DateTime yrmodt = Convert.ToDateTime(yrmo.Insert(4,"/"));            
            DateTime prevmondt = yrmodt.AddMonths(-months);
            string prevyear = prevmondt.Year.ToString();
            string prevmonth = prevmondt.Month.ToString();
            if (prevmonth.Length == 1)
                prevmonth = "0" + prevmonth;
            string prevYRMO = prevyear + prevmonth;

            return prevYRMO;
        }

        public static int importADPDetail(string _yrmo, string _fileName)
        {
            string _Reportyrmo = null;
            int _count = 0;
            int _recCount = 0;
            string line = string.Empty;
            string _PatternDate =
                @"(?<month>\d{1,2})\/(?<year>\d{4})[ \t]*\*(All Plans)";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_PatternDate);

            ImportDAL iobj = new ImportDAL();
            string _prevyrmo = iobj.getPrevYRMO(_yrmo);

            DataSet dsADP = new DataSet();
            dsADP.Clear();
            try
            {
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
                        _Reportyrmo = _pyear + _monthnew;
                        _count++;
                    }
                }
                if (_Reportyrmo.Equals(_prevyrmo))
                {
                    dsADP = parseADPDetails(_fileName, _prevyrmo);
                    _recCount = IPBAImportDAL.insertADP(dsADP, _yrmo);
                }
                else
                {
                    throw (new Exception("YRMO entered doesnt match with Report, Please check the Report."));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return _recCount;
        }

        protected static DataSet parseADPDetails(string _fileName, string _yrmo)
        {
            string line = string.Empty;
            string _plan = "";
            bool _planfound = false;

            List<string> _hmoCodes = new List<string>();
            _hmoCodes = IPBAImportDAL.getHMOCodes();

            DataRow rowNew;
            DataSet dsTotal = new DataSet();
            DataTable newTable;
            newTable = dsTotal.Tables.Add("newTable1");
            DataColumn col;

            col = new DataColumn("plan"); newTable.Columns.Add(col);
            col = new DataColumn("fname"); newTable.Columns.Add(col);
            col = new DataColumn("lname"); newTable.Columns.Add(col);
            col = new DataColumn("SSN"); newTable.Columns.Add(col);
            col = new DataColumn("dob"); newTable.Columns.Add(col);
            col = new DataColumn("bensts"); newTable.Columns.Add(col);
            col = new DataColumn("effdt"); newTable.Columns.Add(col);
            col = new DataColumn("termdt"); newTable.Columns.Add(col);
            col = new DataColumn("covlvl"); newTable.Columns.Add(col);
            col = new DataColumn("comments"); newTable.Columns.Add(col);
            col = new DataColumn("premium"); newTable.Columns.Add(col);
            col = new DataColumn("covgperiod"); newTable.Columns.Add(col);


            string _planPattern =
                @"^\s+Plan:\s+(?<plan>[A-Z0-9]+)\s+";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_planPattern);

            try
            {
                while ((line = reader.ReadLine()) != null)
                {

                    Match parsed = Regex.Match(line, _planPattern);
                    if (parsed.Success)
                    {
                        _planfound = false;
                        _plan = parsed.Groups["plan"].Value.ToString();
                        foreach (string _cd in _hmoCodes)
                        {
                            if (_plan.Trim().Equals(_cd))
                            {
                                _planfound = true;
                                break;
                            }
                        }
                        continue;
                    }

                    if (_planfound)
                    {
                        bool _found = false;
                        if (line.Length > 27)
                        {
                            _found = checkSSN(line.Substring(27, 11));
                        }
                        if (_found)
                        {
                            string period = line.Substring(126, 7).Trim();
                            int _pmonth; string _year;
                            _pmonth = Convert.ToInt32(period.Substring(0, period.IndexOf("/")));
                            _year = period.Substring(period.IndexOf("/") + 1);
                            string _monthnew;
                            if (_pmonth < 10)
                            {
                                _monthnew = "0" + Convert.ToString(_pmonth);
                            }
                            else
                            {
                                _monthnew = Convert.ToString(_pmonth);
                            }
                            string _nyrmo = (_year + _monthnew);
                            //if (_nyrmo.Equals(_yrmo))
                            //{
                                rowNew = newTable.NewRow();  
                                rowNew["plan"] = _plan;

                                string _name = line.Substring(1, 20).Trim();
                                int _index = _name.IndexOf(',');
                                string _lname = _name.Substring(0, _index);
                                string _fname = _name.Substring(_index + 1);

                                rowNew["fname"] = _fname;
                                rowNew["lname"] = _lname;
                                rowNew["SSN"] = line.Substring(27, 11).Trim();
                                rowNew["dob"] = line.Substring(39, 11).Trim();
                                rowNew["bensts"] = line.Substring(81, 1).Trim();
                                rowNew["effdt"] = line.Substring(94, 10).Trim();
                                rowNew["termdt"] = line.Substring(105, 10).Trim();
                                string _nCode = "";
                                if (!line.Substring(116, 2).Trim().Equals(""))
                                {
                                    _nCode = cobraTiercode(line.Substring(116, 2).Trim());
                                }
                                rowNew["covlvl"] = _nCode;
                                rowNew["comments"] = line.Substring(146, 10).Trim();
                                string _rate = line.Substring(158, 10).Trim();
                                string _delimit = "CR";
                                object _frate = "";
                                if (_rate.Contains("CR"))
                                {
                                    _frate = (-1) * Convert.ToDecimal(_rate.Trim(_delimit.ToCharArray()));
                                }
                                else
                                {
                                    _frate = _rate;
                                }

                                rowNew["premium"] = _frate;
                                rowNew["covgperiod"] = _nyrmo;
                                newTable.Rows.Add(rowNew);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return dsTotal;
        }

        public static int importGreenBarDetail(string _yrmo, string _fileName)
        {
            string _Reportyrmo = "";
            int _count = 0;
            int _recCount = 0;
            string line = string.Empty;
            string _PatternDate =
                @"(?<month>JANUARY|FEBRUARY|MARCH|APRIL|MAY|JUNE|JULY|AUGUST|SEPTEMBER|OCTOBER|NOVEMBER|DECEMBER)\s+(?<year>\d{4})\s+PAYMENT\s*";


            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_PatternDate);

            DataSet dsGreenBar = new DataSet();
            dsGreenBar.Clear();
            try
            {
                while ((line = reader.ReadLine()) != null && _count == 0)
                {
                    Match parsed = Regex.Match(line, _PatternDate);
                    if (parsed.Success)
                    {
                        string _pyear = parsed.Groups["year"].Value.ToString();
                        string _pmonth = calucluateMonth(parsed.Groups["month"].Value.ToString());
                        _Reportyrmo = _pyear + _pmonth;
                        _count++;
                    }
                }
                if (_Reportyrmo.Equals(_yrmo))
                {
                    dsGreenBar = parseGreenBarDetails(_fileName, _yrmo);
                    _recCount = IPBAImportDAL.insertGreenBar(dsGreenBar, _yrmo);
                }
                else
                {
                    throw (new Exception("YRMO entered doesnt match with Report, Please check the Report."));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return _recCount;
        }

        protected static DataSet parseGreenBarDetails(string _fileName, string _yrmo)
        {
            string line = string.Empty;

            string _report = "";
            DataRow rowNew;
            DataSet dsTotal = new DataSet();
            DataTable newTable;
            newTable = dsTotal.Tables.Add("newTable1");
            DataColumn col;

            col = new DataColumn("report"); newTable.Columns.Add(col);
            col = new DataColumn("fname"); newTable.Columns.Add(col);
            col = new DataColumn("lname"); newTable.Columns.Add(col);
            col = new DataColumn("SSN"); newTable.Columns.Add(col);
            col = new DataColumn("P_plancode"); newTable.Columns.Add(col);
            col = new DataColumn("P_covlvl"); newTable.Columns.Add(col);
            col = new DataColumn("Plancode"); newTable.Columns.Add(col);
            col = new DataColumn("covlvl"); newTable.Columns.Add(col);
            col = new DataColumn("effdt"); newTable.Columns.Add(col);
            col = new DataColumn("termdt"); newTable.Columns.Add(col);
            col = new DataColumn("rate"); newTable.Columns.Add(col);
            col = new DataColumn("pInd"); newTable.Columns.Add(col);

            int _cnt = 0;



            string _planPattern =
                @"\s*SEQUENCE:\s+REPORT(?<report>A|C|D|E|F|G)\s+";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_planPattern);

            try
            {
                while ((line = reader.ReadLine()) != null)
                {

                    Match parsed = Regex.Match(line, _planPattern);
                    if (parsed.Success)
                    {
                        _report = parsed.Groups["report"].Value.ToString();
                        continue;
                    }
                    if (_report.Equals("A"))
                    {

                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(68, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(29, 18).Trim();
                            rowNew["fname"] = line.Substring(48, 18).Trim();
                            rowNew["SSN"] = line.Substring(2, 11).Trim();
                            rowNew["P_plancode"] = "";
                            rowNew["P_covlvl"] = "";
                            rowNew["Plancode"] = line.Substring(68, 2).Trim();
                            rowNew["covlvl"] = line.Substring(95, 1).Trim();
                            rowNew["effdt"] = line.Substring(80, 8).Trim();
                            rowNew["termdt"] = "";
                            rowNew["rate"] = line.Substring(99, 9).Trim();
                            rowNew["pInd"] = line.Substring(114, 1).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }
                    else if (_report.Equals("C"))
                    {

                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(66, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(27, 18).Trim();
                            rowNew["fname"] = line.Substring(46, 18).Trim();
                            rowNew["SSN"] = line.Substring(2, 11).Trim();
                            rowNew["P_plancode"] = "";
                            rowNew["P_covlvl"] = "";
                            rowNew["Plancode"] = line.Substring(66, 2).Trim();
                            rowNew["covlvl"] = line.Substring(94, 1).Trim();
                            rowNew["effdt"] = line.Substring(78, 8).Trim();
                            rowNew["termdt"] = "";
                            rowNew["rate"] = line.Substring(100, 9).Trim();
                            rowNew["pInd"] = line.Substring(116, 9).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }
                    else if (_report.Equals("D"))
                    {
                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(57, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(19, 18).Trim();
                            rowNew["fname"] = line.Substring(38, 18).Trim();
                            rowNew["SSN"] = line.Substring(2, 11);
                            rowNew["P_plancode"] = line.Substring(57, 2).Trim();
                            rowNew["P_covlvl"] = line.Substring(68, 1).Trim();
                            rowNew["Plancode"] = line.Substring(92, 2).Trim();
                            rowNew["covlvl"] = line.Substring(103, 1).Trim();
                            rowNew["effdt"] = "";
                            rowNew["termdt"] = line.Substring(77, 8).Trim();
                            rowNew["rate"] = line.Substring(109, 8).Trim();
                            rowNew["pInd"] = line.Substring(122, 9).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }
                    else if (_report.Equals("F"))
                    {
                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(91, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(20, 16).Trim();
                            rowNew["fname"] = line.Substring(37, 15).Trim();
                            rowNew["SSN"] = line.Substring(2, 11).Trim();
                            rowNew["P_plancode"] = line.Substring(56, 2).Trim();
                            rowNew["P_covlvl"] = line.Substring(67, 1).Trim();
                            rowNew["Plancode"] = line.Substring(91, 2).Trim();
                            rowNew["covlvl"] = line.Substring(102, 1).Trim();
                            rowNew["effdt"] = line.Substring(76, 8).Trim();
                            rowNew["rate"] = line.Substring(108, 8).Trim();
                            rowNew["pInd"] = line.Substring(122, 9).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }
                    else if (_report.Equals("E"))
                    {
                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(86, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(17, 16).Trim();
                            rowNew["fname"] = line.Substring(35, 16).Trim();
                            rowNew["SSN"] = line.Substring(2, 11).Trim();
                            rowNew["P_plancode"] = line.Substring(52, 2).Trim();
                            rowNew["P_covlvl"] = line.Substring(64, 1).Trim();
                            rowNew["Plancode"] = line.Substring(86, 2).Trim();
                            rowNew["covlvl"] = line.Substring(96, 1).Trim();
                            rowNew["effdt"] = line.Substring(71, 8).Trim();
                            rowNew["termdt"] = "";
                            rowNew["rate"] = line.Substring(105, 10).Trim();
                            rowNew["pInd"] = line.Substring(119, 9).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }
                    else if (_report.Equals("G"))
                    {
                        if (checkSSN(line.Substring(2, 11).Trim()) && CheckPlancode(line.Substring(84, 2).Trim()))
                        {
                            rowNew = newTable.NewRow();
                            rowNew["report"] = _report;
                            rowNew["lname"] = line.Substring(16, 16).Trim();
                            rowNew["fname"] = line.Substring(32, 16).Trim();
                            rowNew["SSN"] = line.Substring(2, 11).Trim();
                            rowNew["P_plancode"] = line.Substring(50, 2).Trim();
                            rowNew["P_covlvl"] = line.Substring(62, 1).Trim();
                            rowNew["Plancode"] = line.Substring(84, 2).Trim();
                            rowNew["covlvl"] = line.Substring(94, 1).Trim();
                            rowNew["effdt"] = "";
                            rowNew["termdt"] = line.Substring(69, 8).Trim();
                            rowNew["rate"] = line.Substring(103, 8).Trim();
                            rowNew["pInd"] = line.Substring(119, 9).Trim();
                            newTable.Rows.Add(rowNew);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
            return dsTotal;
        }

        protected static bool checkSSN(string _ssn)
        {
            string _match = @"\d{3}-\d{2}-\d{4}";
            Match parsed = Regex.Match(_ssn, _match);
            bool _matched = false;
            if (parsed.Success)
            {
                _matched = true;
            }
            return _matched;
        }

        protected static bool CheckPlancode(string _pcode)
        {
            if (_pcode.Equals("P5"))
                return true;
            else
                return false;
        }

        protected static string cobraTiercode(string _oldcode)
        {
            string _newCode = "";
            if (_oldcode.Equals("S"))
            {
                _newCode = "E";
            }
            else if (_oldcode.Equals("ES"))
            {
                _newCode = "S";
            }
            else if (_oldcode.Equals("EC"))
            {
                _newCode = "C";
            }
            else
            {
                _newCode = "F";
            }
            return _newCode;
        }

        protected static string calucluateMonth(string _month)
        {
            string _m = "";
            switch (_month)
            {
                case "JANUARY":
                    _m = "01";
                    break;
                case "FEBRUARY":
                    _m = "02";
                    break;
                case "MARCH":
                    return "03";
                    break;
                case "APRIL":
                    _m = "04";
                    break;
                case "MAY":
                    _m = "05";
                    break;
                case "JUNE":
                    _m = "06";
                    break;
                case "JULY":
                    _m = "07";
                    break;
                case "AUGUST":
                    _m = "08";
                    break;
                case "SEPTEMBER":
                    _m = "09";
                    break;
                case "OCTOBER":
                    _m = "10";
                    break;
                case "NOVEMBER":
                    _m = "11";
                    break;
                case "DECEMBER":
                    _m = "12";
                    break;
            }
            return _m;
        }

        public static int importRatesTemplate(string _file, string _year)
        {
            DataSet impData = new DataSet();
            DataTable dt = new DataTable("rateTemplate");
            if (_file.Contains(_year))
            {
                try
                {
                    impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt));
                    int _cnt = IPBAImportDAL.insertRates(impData);
                    return _cnt;
                }
                catch (Exception ex)
                {
                    throw ex; ;
                }
            }
            else
            {
                throw (new Exception("Selected file is not for year - '" + _year + "'. Please import correct file."));
            }
        }
    }
}

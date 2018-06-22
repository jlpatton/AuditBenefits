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

namespace EBA.Desktop.VWA
{
    /// <summary>
    /// Summary description for VWAImport
    /// </summary>
    public class VWAImport
    {
        private static DataTable newTable; 
        private static DataTable aTable;
        private static string _dataPattern =
                @"^\s+(?<date>\d{2}\/\d{2})\s+(?<customer>\w*)\s+(?<amount>\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?)\s+(?<desc>[A-Za-z0-9:\s]*)\s+(?<bankref>\d+)\s*$";
        private static string _descWD1 = @"(ID:005486009471)"; //VWA
        private static string _descWD2 = @"(ID:003751958246)"; //FDX
        private static string _descWD3 = @"(ID:6607607910177)"; //DisAb

        public VWAImport()
        {
            newTable = new DataTable();
            aTable = new DataTable();

        }

        /// <summary>
        /// Method to Import Bank Statement
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <param name="_fileName">Bank statement FileName</param>
        /// <returns></returns>
        public int importBOA_VWA(string _yrmo, string _fileName)
        {
            string _Reportyrmo = "";
            int _count = 0;
            int _recCount = 0;
            string line = string.Empty;
            string _PatternDate =
                @"^\s+(Statement Period)\s+(?<smonth>\d{2})\/(?<sday>\d{2})\/(?<syear>\d{2})\s+(through)\s+(?<emonth>\d{2})\/(?<eday>\d{2})\/(?<eyear>\d{2})";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_PatternDate);
            
            //string _prevyrmo = VWA.getPrevYRMO(_yrmo);

            DataSet dsBOA = new DataSet();
            dsBOA.Clear();
            try
            {
                while ((line = reader.ReadLine()) != null && _count == 0)
                {
                    Match parsed = Regex.Match(line, _PatternDate);
                    if (parsed.Success)
                    {
                        string _syear = parsed.Groups["syear"].Value.ToString();
                        string _smonth = parsed.Groups["smonth"].Value.ToString();
                        string _sday = parsed.Groups["sday"].Value.ToString();

                        string _eyear = parsed.Groups["eyear"].Value.ToString();
                        string _emonth = parsed.Groups["emonth"].Value.ToString();
                        string _eday = parsed.Groups["eday"].Value.ToString();

                        string _lastday = "";
                        int _lday = VWA.GetLastDayofYRMO("20" + _eyear + _emonth).Day;
                        if (_lday < 10)
                        {
                            _lastday = "0" + VWA.GetLastDayofYRMO("20" + _eyear + _emonth).Day.ToString();

                        }
                        else
                        {
                            _lastday = VWA.GetLastDayofYRMO("20" + _eyear + _emonth).Day.ToString();

                        }                       
                        if (_sday.Equals("01") && _eday.Equals(_lastday))
                        {
                            _Reportyrmo = "20" + _syear + _smonth;
                        }
                        _count++;
                    }
                }
                if (_Reportyrmo.Equals(_yrmo))
                {
                    dsBOA = parseBOA_VWA(_fileName, _yrmo);
                    _recCount = VWAImportDAL.storeBOAData(dsBOA);
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

        /// <summary>
        /// Method to Import VWA Monthly file
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <param name="_fileName">VWA Monthly File FileName</param>
        /// <returns></returns>
        public int import_VWA_Trans(string _yrmo, string _fileName)
        {
            string _Reportyrmo = "";
            int _count = 0;
            int _recCount = 0;
            string line = string.Empty;
            string _PatternDate =
                @"(?<year>\d{4})(?<month>\d{2})(_vwa)";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_PatternDate);

            

            DataSet dsVWA = new DataSet();
            dsVWA.Clear();
            try
            {            
                Match parsed = Regex.Match(_fileName, _PatternDate);
                if (parsed.Success)
                {
                    string _year = parsed.Groups["year"].Value.ToString();
                    string _month = parsed.Groups["month"].Value.ToString();
                    _Reportyrmo = _year + _month;
                }
                
                if (_Reportyrmo.Equals(_yrmo))
                {
                    dsVWA = parseVWA_Trans(_fileName, _yrmo);
                    _recCount = VWAImportDAL.storeVWATransData(dsVWA);
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

        /// <summary>
        /// Parse the file using regex patterns for required data
        /// </summary>
        /// <param name="_fileName">BOA Statement</param>
        /// <param name="_yrmo">YRMO</param>
        /// <returns>Dataset with required data</returns>
        protected DataSet parseBOA_VWA(string _fileName, string _yrmo)
        {
            string line = string.Empty;           
            DataSet dsBOATemp = new DataSet();
            //newTable = dsBOATemp.Tables.Add("newTable1");
            newTable.Clear();
            aTable.Clear();
            newTable.TableName = "detTable";
            aTable.TableName = "sumTable";
            DataColumn col;
            DataColumn col1;
            DataRow nrow;

            col = new DataColumn("PostDate"); newTable.Columns.Add(col);
            col = new DataColumn("Type"); newTable.Columns.Add(col);
            col = new DataColumn("Amount"); newTable.Columns.Add(col);
            col = new DataColumn("WireTo"); newTable.Columns.Add(col);
            col = new DataColumn("ReferenceNo"); newTable.Columns.Add(col);
            col = new DataColumn("yrmo"); newTable.Columns.Add(col);

            col1 = new DataColumn("sType"); aTable.Columns.Add(col1);
            col1 = new DataColumn("sAmnt"); aTable.Columns.Add(col1);
            col1 = new DataColumn("yrmo"); aTable.Columns.Add(col1);

            bool _p1F = false;
            bool _p2F = false;
            bool _p3F = false;
            bool _p4F = false;
            bool _pD = false;
            bool _pW = false;            

            decimal _balance, _deposit, _withdrawls, _endingBalance, _amnt;
            string _pdate, _desc, _type, _wireto, _refno;            

            string _p1 = @"(Statement Beginning Balance)\s+(?<sbbamt>\$([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?)\s+$";
            string _p2 = @"(Amount of Deposits/Credits)\s+(?<adcamt>\$([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?)\s+$";
            string _p3 = @"(Amount of Withdrawals/Debits)\s+(?<awdamt>\$([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?)\s+$";
            string _p4 = @"(Statement Ending Balance)\s+(?<sebamt>\$([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?)\s+$";

            string _depositStart = @"^\s+(Deposits and Credits)\s+$";
            string _withdrawalStart = @"^\s+(Withdrawals and Debits)\s+$";

            

            TextReader reader = new StreamReader(File.OpenRead(_fileName));           

            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!_p1F)
                    {
                        Match parsed = Regex.Match(line, _p1);
                        if (parsed.Success)
                        {
                            _p1F = true;
                            _balance = Decimal.Parse(parsed.Groups["sbbamt"].Value.Trim(), System.Globalization.NumberStyles.Currency);
                            nrow = aTable.NewRow();
                            nrow["yrmo"] = _yrmo;
                            nrow["sType"] = "Start Balance";
                            nrow["sAmnt"] = _balance;
                            aTable.Rows.Add(nrow);
                            continue;
                        }
                    }
                    else if (!_p2F)
                    {
                        Match parsed = Regex.Match(line, _p2);
                        if (parsed.Success)
                        {
                            _p2F = true;
                            _deposit = Decimal.Parse(parsed.Groups["adcamt"].Value.Trim(), System.Globalization.NumberStyles.Currency);
                            nrow = aTable.NewRow();
                            nrow["yrmo"] = _yrmo;
                            nrow["sType"] = "Total Deposits";
                            nrow["sAmnt"] = _deposit;
                            aTable.Rows.Add(nrow);
                            continue;
                        }
                    }
                    else if (!_p3F)
                    {
                        Match parsed = Regex.Match(line, _p3);
                        if (parsed.Success)
                        {
                            _p3F = true;
                            _withdrawls = Decimal.Parse(parsed.Groups["awdamt"].Value.Trim(), System.Globalization.NumberStyles.Currency);
                            nrow = aTable.NewRow();
                            nrow["yrmo"] = _yrmo;
                            nrow["sType"] = "Total Withdrawls";
                            nrow["sAmnt"] = _withdrawls;
                            aTable.Rows.Add(nrow);
                            continue;
                        }
                    }
                    else if (!_p4F)
                    {
                        Match parsed = Regex.Match(line, _p4);
                        if (parsed.Success)
                        {
                            _p4F = true;
                            _endingBalance = Decimal.Parse(parsed.Groups["sebamt"].Value.Trim(), System.Globalization.NumberStyles.Currency);
                            nrow = aTable.NewRow();
                            nrow["yrmo"] = _yrmo;
                            nrow["sType"] = "End Balance";
                            nrow["sAmnt"] = _endingBalance;
                            aTable.Rows.Add(nrow);
                            continue;
                        }
                    }

                    if (!_pD)
                    {
                        Match parsed = Regex.Match(line, _depositStart);
                        if (parsed.Success)
                        {
                            _pD = true;
                            continue;
                        }
                    }
                    else if(!_pW)
                    {
                        Match parsed1 = Regex.Match(line, _withdrawalStart);
                        if (parsed1.Success)
                        {
                            _pW = true;
                            continue;
                        }

                        Match parsed = Regex.Match(line, _dataPattern);
                        if (parsed.Success)
                        {
                            setData(parsed.Groups["date"].Value.ToString().Trim(), (parsed.Groups["desc"].Value.ToString().Trim().Equals("Deposit") ? "DEP" : "MSCC"),
                                    Decimal.Parse(parsed.Groups["amount"].Value.ToString().Trim(), System.Globalization.NumberStyles.Currency),
                                    "", (parsed.Groups["bankref"].Value.ToString().Trim()), _yrmo);                           
                            continue;
                        }
                    }
                    else if (_pW)
                    {                       
                        Match parsed = Regex.Match(line, _dataPattern);
                        if (parsed.Success)
                        {
                            _pdate = (parsed.Groups["date"].Value.ToString().Trim());
                            _desc = (parsed.Groups["desc"].Value.ToString().Trim());
                            _amnt = Decimal.Parse(parsed.Groups["amount"].Value.ToString().Trim(), System.Globalization.NumberStyles.Currency);
                            _refno = (parsed.Groups["bankref"].Value.ToString().Trim());
                            _type = "";
                            _wireto = "";                            
                            readData(reader, _pdate, _desc, _amnt, _refno, _yrmo);
                            
                            
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
            dsBOATemp.Tables.Add(aTable.Copy());
            dsBOATemp.Tables.Add(newTable.Copy());
            return dsBOATemp;
        }

        protected static DataSet parseVWA_Trans(string _fileName, string _yrmo)
        {
            string line = string.Empty;            

            DataRow rowNew;
            DataSet dsVWAF = new DataSet();
            DataTable newTable;
            newTable = dsVWAF.Tables.Add("newTable1");
            DataColumn col;

            col = new DataColumn("yrmo"); newTable.Columns.Add(col);
            col = new DataColumn("ClientID"); newTable.Columns.Add(col);
            col = new DataColumn("ContractNo"); newTable.Columns.Add(col);
            col = new DataColumn("Lname"); newTable.Columns.Add(col);
            col = new DataColumn("Fname"); newTable.Columns.Add(col);
            col = new DataColumn("SSNO"); newTable.Columns.Add(col);
            col = new DataColumn("GroupNo"); newTable.Columns.Add(col);
            col = new DataColumn("DisabCd"); newTable.Columns.Add(col);
            col = new DataColumn("PatientName"); newTable.Columns.Add(col);
            col = new DataColumn("RelCd"); newTable.Columns.Add(col);
            col = new DataColumn("AccDt"); newTable.Columns.Add(col);
            col = new DataColumn("OpenDt"); newTable.Columns.Add(col);
            col = new DataColumn("CloseDt"); newTable.Columns.Add(col);
            col = new DataColumn("RecDt"); newTable.Columns.Add(col);
            col = new DataColumn("StatusCd"); newTable.Columns.Add(col);
            col = new DataColumn("BenPaid"); newTable.Columns.Add(col);
            col = new DataColumn("RecAmt"); newTable.Columns.Add(col);
            col = new DataColumn("TotFees"); newTable.Columns.Add(col);
            col = new DataColumn("NetAmt"); newTable.Columns.Add(col);
            col = new DataColumn("SysDt"); newTable.Columns.Add(col);

            TextReader reader = new StreamReader(File.OpenRead(_fileName));  

            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 230)
                    {
                        rowNew = newTable.NewRow();
                        rowNew["yrmo"] = _yrmo;
                        rowNew["ClientID"] = line.Substring(0, 6).Trim();
                        rowNew["ContractNo"] = line.Substring(6, 7).Trim();
                        rowNew["Lname"] = line.Substring(13, 31).Trim();
                        rowNew["Fname"] = line.Substring(44, 29).Trim();
                        rowNew["SSNO"] = line.Substring(73, 9).Trim();
                        rowNew["GroupNo"] = line.Substring(82, 1).Trim();
                        rowNew["DisabCd"] = line.Substring(83, 3).Trim();
                        rowNew["PatientName"] = line.Substring(86, 30).Trim();
                        rowNew["RelCd"] = line.Substring(116, 1).Trim();
                        rowNew["AccDt"] = line.Substring(117, 10).Trim();
                        rowNew["OpenDt"] = line.Substring(127, 10).Trim();
                        rowNew["CloseDt"] = line.Substring(137, 10).Trim();
                        rowNew["RecDt"] = line.Substring(147, 10).Trim();
                        rowNew["StatusCd"] = line.Substring(157, 3).Trim();
                        rowNew["BenPaid"] = line.Substring(160, 13).Trim();
                        rowNew["RecAmt"] = line.Substring(173, 15).Trim();
                        rowNew["TotFees"] = line.Substring(188, 15).Trim();
                        rowNew["NetAmt"] = line.Substring(203, 16).Trim();
                        rowNew["SysDt"] = line.Substring(220, 10).Trim();
                        newTable.Rows.Add(rowNew);
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
            return dsVWAF;
        }

        /// <summary>
        /// Loop through reader to read withdrawls data.
        /// </summary>
        /// <param name="_reader">TextReader</param>
        /// <param name="_pdate2">Posted Date</param>
        /// <param name="_desc2">Description</param>
        /// <param name="_amt2">Amount</param>
        /// <param name="_refno2">Reference Number</param>
        /// <param name="_yrmo2">YRMO</param>
        private void readData(TextReader _reader,string _pdate2, string _desc2, decimal _amt2, string _refno2, string _yrmo2)
        {
            try
            {
                string _pdateL, _descL, _refnoL;
                decimal _amntL;
                string _type2 = "", _wireto2 = "";
                string line1 = string.Empty;
                while ((line1 = _reader.ReadLine()) != null)
                {
                    Match parsed = Regex.Match(line1, _dataPattern);
                    if (parsed.Success)
                    {
                        if (_type2.Equals(""))
                        {
                            setData(_pdate2, "MSCD", _amt2, "MSC", _refno2, _yrmo2);                           
                        }
                        _pdateL = (parsed.Groups["date"].Value.ToString().Trim());
                        _descL = (parsed.Groups["desc"].Value.ToString().Trim());
                        _amntL = Decimal.Parse(parsed.Groups["amount"].Value.ToString().Trim(), System.Globalization.NumberStyles.Currency);
                        _refnoL = (parsed.Groups["bankref"].Value.ToString().Trim());
                        readData(_reader, _pdateL, _descL, _amntL, _refnoL, _yrmo2);
                    }
                    Match parsed1 = Regex.Match(line1, _descWD1);
                    if (parsed1.Success)
                    {
                        _type2 = "VWA";
                        _wireto2 = "VWA";
                        setData(_pdate2, _type2, _amt2, _wireto2, _refno2, _yrmo2);                       
                        continue;
                    }
                    Match parsed2 = Regex.Match(line1, _descWD2);
                    if (parsed2.Success)
                    {
                        _type2 = "FDX";
                        _wireto2 = "FDX";
                        setData(_pdate2, _type2, _amt2, _wireto2, _refno2, _yrmo2);                        
                        continue;
                    }
                    Match parsed3 = Regex.Match(line1, _descWD3);
                    if (parsed3.Success)
                    {
                        _type2 = "DisAb";
                        _wireto2 = "DisAb";
                        setData(_pdate2, _type2, _amt2, _wireto2, _refno2, _yrmo2);                       
                        continue;
                    }                    
                }
                if (_type2.Equals("") && ((line1 = _reader.ReadLine()) == null))
                {
                    setData(_pdate2, "MSCD", _amt2, "MSC", _refno2, _yrmo2);
                }
            }
            catch (Exception ex)
            {
                _reader.Close();
                throw ex;
            }
            finally
            {
                
            }
        }
        
        /// <summary>
        /// Parsed columns are assigned to data table columns.
        /// </summary>
        /// <param name="_pdate1">Post Date</param>
        /// <param name="_type1">type</param>
        /// <param name="_amt1">Amount</param>
        /// <param name="_wireto1">Wire To</param>
        /// <param name="_refno1">Reference Number</param>
        /// <param name="_yrmo1">YRMO</param>
        private void setData(string _pdate1, string _type1, decimal _amt1, string _wireto1, string _refno1, string _yrmo1)
        {
            DataRow rowNew;
            rowNew = newTable.NewRow();
            rowNew["PostDate"] = _pdate1;
            rowNew["Type"] = _type1;
            rowNew["Amount"] = _amt1;
            rowNew["WireTo"] = _wireto1;
            rowNew["ReferenceNo"] = _refno1;
            rowNew["yrmo"] = _yrmo1;
            newTable.Rows.Add(rowNew);
        }
    }
}

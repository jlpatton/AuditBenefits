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
/// Summary description for ImportCobra
/// </summary>
/// 

namespace EBA.Desktop.Anthem
{
    public class ImportCobra
    {
        

        public ImportCobra()
        {
           
        }
        public static void importADPDetail(string _yrmo, string _fileName)
        {
            string _Reportyrmo = null;
            int _count = 0;
            string line = string.Empty;
            string _PatternDate =
                @"(?<month>\d{1,2})\/(?<year>\d{4})[ \t]*\*(All Plans)";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_PatternDate);
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
                if (_Reportyrmo.Equals(_yrmo))
                {
                    parseADPDetails(_fileName, _yrmo);
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
        }

        protected static void parseADPDetails(string _fileName,string _yrmo)
        {   
            string line     = string.Empty;            
            bool _foundPlan = false;
            string _planDesc = null;
            string _plan = null;
            string _pNum = null;

            string _Name = null;
            string _dob = null;
            string _ssn = null;
            string _unit = null;
            string _code = null;
            string _div = null;
            string _bensts = null;
            string _effdt = null;
            string _termdt = null;
            string _covlvl = null;
            string _class = null;
            string _period = null;
            string _paiddt = null;
            string _comments = null;
            string _premium = null;
            string _carrier = null;

            string _planPattern =
                @"^\s+Plan:\s+(?<plan>[A-Z0-9]+)\s+(?<pcode>\w+?)\s+(?<desc>([A-Za-z-.'()]+\s?)*)$";
            string _empDetails =
                //@"^\s+?(?<Name>([A-Z]\,?\s?\'?\.?)*)\s+?(?<ssn>\d{3}\-\d{2}\-\d{4})\s+?(?<bdate>\d{1,2}\/\d{2}\/\d{4})\s+(?<Unit>(\w{5})*)\s+(?<code>[A-Z]*)\s+(?<div>(\w{5})*)\s+(?<bensts>[A-Z]+)\s+(?<effdt>(\d{1,2}\/\d{2}\/\d{4}))\s+(?<termdt>(\d{1,2}\/\d{2}\/\d{4})*)\s+(?<covlvl>[A-Z]*)\s+(?<clas>[A-Za-z0-9]*)\s+(?<period>\d{1,2}\/\d{4})\s+(?<paiddt>(\d{1,2}\/\d{2}\/\d{4})*)\s+(?<comments>[A-Za-z]*)\s+(?<premium>\d*\.\d{2})\s+(?<carrier>\d*\.\d{2})$";
                @"^\s+(?<Name>([A-Z]\,?\s?\'?\.?\-?)*)\s+(?<ssn>\d{3}-\d{2}-\d{4})\s+(?<bdate>\d{1,2}\/\d{2}\/\d{4})\s+(?<Unit>([A-Z]+\d{3})*)\s+(?<code>([A-Z])*)\s+(?<div>([A-Z]+\d{3})*)\s+(?<bensts>[A-Z])\s+(?<effdt>(\d{1,2}\/\d{2}\/\d{4}))\s+(?<termdt>(\d{1,2}\/\d{2}\/\d{4})*)\s+(?<covlvl>[A-Z]+)\s+(?<clas>(\w)*)\s+(?<period>\d{1,2}\/\d{4})\s+(?<paiddt>(\d{1,2}\/\d{2}\/\d{4})*)\s+(?<comments>[A-Za-z]*)\s+(?<premium>(\d{1,3},)*\d*\.\d{2}(CR)*)\s+(?<carrier>(\d{1,3},)*\d*\.\d{2}(CR)*)$";

            TextReader reader = new StreamReader(File.OpenRead(_fileName));
            Regex r = new Regex(_planPattern);
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    Match parsed = Regex.Match(line, _planPattern);
                    if (_foundPlan)
                    {
                        Match parsed1 = Regex.Match(line, _empDetails);
                        if (parsed1.Success)
                        {
                            string period = parsed1.Groups["period"].Value.ToString();
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
                            if (_nyrmo.Equals(_yrmo))
                            {
                                _Name = parsed1.Groups["Name"].Value.ToString();
                                _ssn = parsed1.Groups["ssn"].Value.ToString();
                                _dob = parsed1.Groups["bdate"].Value.ToString();
                                _unit = parsed1.Groups["Unit"].Value.ToString();
                                _code = parsed1.Groups["code"].Value.ToString();
                                _div = parsed1.Groups["div"].Value.ToString();
                                _bensts = parsed1.Groups["bensts"].Value.ToString();
                                _effdt = parsed1.Groups["effdt"].Value.ToString();
                                _termdt = parsed1.Groups["termdt"].Value.ToString();
                                _covlvl = parsed1.Groups["covlvl"].Value.ToString();
                                _class = parsed1.Groups["clas"].Value.ToString();
                                _period = parsed1.Groups["period"].Value.ToString();
                                _paiddt = parsed1.Groups["paiddt"].Value.ToString();
                                _comments = parsed1.Groups["comments"].Value.ToString();
                                _premium = parsed1.Groups["premium"].Value.ToString();
                                _carrier = parsed1.Groups["carrier"].Value.ToString();
                                insertStageADPDetails(_plan, _pNum, _planDesc, _Name, _ssn, _dob, _unit, _code, _div, _bensts, _effdt,
                                                        _termdt, _covlvl, _class, _yrmo, _paiddt, _comments, _premium, _carrier);
                            }
                        }
                        else if (parsed.Success)
                        {
                            _foundPlan = false;
                        }
                    }
                    if (!_foundPlan)
                    {
                        if (parsed.Success)
                        {
                            _plan = parsed.Groups["plan"].Value.ToString();
                            _pNum = parsed.Groups["pcode"].Value.ToString();
                            _planDesc = parsed.Groups["desc"].Value.ToString();
                            if (_planDesc.Contains("Anthem"))
                            {
                                _foundPlan = true;
                            }
                        }
                    }
                }
                execADPSP(_yrmo);
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

        protected static void insertStageADPDetails(string plan,string planNum,string pDesc,string name,string ssn,string dob,
            string punit,string acctcd,string div,string bensts,string eff_dt,string term_dt,string covlvl,
            string clas,string cvgPeriod,string paidDt,string comments,string premium,string carrier)
        {    
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            string _cmdstr = "INSERT INTO [ADP_Details] "
                             + "([adp_plan],[adp_planNum],[adp_planDesc],[adp_EmpName],[adp_ssn],[adp_dob] " 
                             + ",[adp_unit],[adp_accountcd],[adp_div],[adp_bensts] "
                             + ",[adp_covg_eff_dt],[adp_covg_term_dt],[adp_covlvl],[adp_class] "
                             + ",[adp_covg_period],[adp_paid_dt],[adp_comments],[adp_total_premium],[adp_total_carrier]) "
                             + " VALUES "
                             + "(@plan,@pnum,@pdesc,@name,@pssn,@pdob,@unit,@acctcd,@div,@bensts, "
                             + " @ceffdt,@ctermdt,@covl,@class,@cperiod,@paiddt,@comments,@premium,@carrier)";
            command = new SqlCommand(_cmdstr, connect);
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command.Parameters.AddWithValue("@plan", plan);
                command.Parameters.AddWithValue("@pnum", planNum);
                command.Parameters.AddWithValue("@pdesc", pDesc);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@pssn", ssn.Replace("-",""));
                command.Parameters.AddWithValue("@pdob", Convert.ToDateTime(dob));
                command.Parameters.AddWithValue("@unit", punit);
                command.Parameters.AddWithValue("@acctcd", acctcd);
                command.Parameters.AddWithValue("@div", div);
                command.Parameters.AddWithValue("@bensts", bensts);
                command.Parameters.AddWithValue("@ceffdt", Convert.ToDateTime(eff_dt));
                
                if (term_dt.Equals(""))
                {
                    command.Parameters.AddWithValue("@ctermdt", DBNull.Value);
                }
                else
                {
                     command.Parameters.AddWithValue("@ctermdt",Convert.ToDateTime(term_dt));
                }
                
                command.Parameters.AddWithValue("@covl", covlvl);
                command.Parameters.AddWithValue("@class", clas);
                command.Parameters.AddWithValue("@cperiod", cvgPeriod);   
            
                if (paidDt.Equals(""))
                {
                    command.Parameters.AddWithValue("@paiddt", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@paiddt",  Convert.ToDateTime(paidDt));
                }
               
                command.Parameters.AddWithValue("@comments", comments);

                if (premium.Contains("CR"))
                {
                    premium = "-" + premium.Substring(0, premium.IndexOf("C"));
                }
                command.Parameters.AddWithValue("@premium", Convert.ToDecimal(premium));
                if (premium.Contains("CR"))
                {
                    carrier = "-" + carrier.Substring(0, carrier.IndexOf("C"));
                }
                command.Parameters.AddWithValue("@carrier", Convert.ToDecimal(carrier));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }                
            
        }

        protected static void execADPSP(string _yrmo)
        {
            string _year = _yrmo.Substring(0, 4);
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
            //string yrmo = Convert.ToDateTime(_yrmo.Insert(4, "/")).ToShortDateString();
                       
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand("sp_InsertADPHeadcounts", conn);                
                cmd.Parameters.AddWithValue("@planYear", _year);
                cmd.Parameters.AddWithValue("@presentyrmo", _yrmo);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }        
    }


}

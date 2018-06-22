using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;

namespace EBA.Desktop.HRA
{
    public class HRAImportDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRAImportDAL()
        {
        }

        public static Boolean CheckAutoImport(string _category, string source)
        {
            string cmdStr = "SELECT [autoimport] FROM [FileMaintainence] WHERE (([module] = 'HRA') AND ([classification] = 'Import') AND ([category] = @category) AND ([sourcecd] = @source))";
            Boolean autoimport = false;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@source", source);
                autoimport = Convert.ToBoolean(command.ExecuteScalar());

                return autoimport;
            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean PastImport(string source, string period)
        {
            string cmdStr = "SELECT COUNT(*) FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='HRA'";
            int count = 0;

            try
            { 
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);               
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                count = Convert.ToInt32(command.ExecuteScalar());               
                if (count == 0)
                    return false;
                else
                    return true;
            }
            finally
            {
                connect.Close();
            }
        }

        public void Rollback(string source, string period)
        {            
            string cmdStr1 = "";
            string cmdStr2 = "DELETE FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='HRA'";

            try
            {
                switch (source)
                {
                    case "Putnam":
                        cmdStr1 = "DELETE FROM Putnam WHERE ptnm_yrmo = @period";
                        break;
                    case "PutnamAdj":
                        cmdStr1 = "DELETE FROM PutnamAdj WHERE ptna_yrmo = @period";
                        break;
                    case "Wageworks":
                        cmdStr1 = "DELETE FROM Wageworks WHERE wgwk_yrmo = @period";
                        break;
                    case "ptnm_invoice":
                        cmdStr1 = "DELETE FROM hra_PutnamInvoice WHERE quarteryear = @period";
                        break;
                    case "ptnm_partdata":
                        cmdStr1 = "DELETE FROM hra_PartDatainvoice WHERE period = @period AND source = @source ";
                        break;
                    case "wgwk_invoice":
                        cmdStr1 = "DELETE FROM hra_PartDataInvoice WHERE period = @period AND source = @source ";
                        break;
                    case "HRAAUDITR":
                        cmdStr1 = "DELETE FROM [hra_AUDITR] WHERE [period] = @period";
                        break; 
                }
              
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();               
                command = new SqlCommand(cmdStr2, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();  
            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean insertPutnam(String yrmo, DateTime distdt, String transType, Decimal distamt, String ssn, String lname, String fname)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                //Issue 48 - Alerts with Exception file for Non-HRA participants
                if (!CheckHRAParticipant(ssn))
                {
                    string _emp = ssn + "\t" + lname + "\t" + fname;
                    createExceptionReport(_emp, "Putnam", yrmo);
                    return false;
                }

                command = new SqlCommand("INSERT INTO Putnam(ptnm_ssn, ptnm_lname, ptnm_fname, ptnm_yrmo, ptnm_tranclass, ptnm_transtype, ptnm_distribdt, ptnm_distamt) " 
                                                    + "VALUES(@ssn, @lname, @fname, @yrmo, 'TRN', @trantype, @distdt, @distamt)", connect);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@trantype", transType);
                command.Parameters.AddWithValue("@distdt", distdt);
                command.Parameters.AddWithValue("@distamt", distamt);
                command.ExecuteNonQuery();
                return true;
            }            
            finally
            {
                connect.Close();
            }
        }

        public void insertImportStatus(string yrmo, string source)
        {
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module) VALUES(@yrmo, @source, 'Import', 'HRA')";
            
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
            }            
            finally
            {
                connect.Close();
            }
        }

        public Boolean insertPutnamAdj(String yrmo, DateTime distdt, String transType, Decimal distamt, String ssn, String lname, String fname)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                //Issue 48 - Alerts with Exception file for Non-HRA participants
                if (!CheckHRAParticipant(ssn))
                {
                    string _emp = ssn + "\t" + lname + "\t" + fname;
                    createExceptionReport(_emp, "PutnamAdj", yrmo);
                    return false;
                }
                command = new SqlCommand("INSERT INTO PutnamAdj(ptna_ssn, ptna_lname, ptna_fname, ptna_yrmo, ptna_tranclass, ptna_transtype, ptna_amt, ptna_dt) "
                                                + "VALUES(@ssn, @lname, @fname, @yrmo, 'TRN', @trantype, @distamt, @distdt)", connect);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@trantype", transType);
                command.Parameters.AddWithValue("@distdt", distdt);
                command.Parameters.AddWithValue("@distamt", distamt);
                command.ExecuteNonQuery();
                return true;
            }           
            finally
            {
                connect.Close();
            }
        }

        public Boolean insertWageworks(String yrmo, DateTime createdt, Decimal amt, String transtype, String lname, String fname, String last4ssn)
        {
            string _ssn;

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                
                _ssn = GetSSN(last4ssn, lname.Substring(0, 3));
                //Issue 48 - Alerts with Exception file for Non-HRA participants
                if (_ssn == null)
                {
                    if (last4ssn.Equals("6173") || last4ssn.Equals("0707"))
                    {
                        last4ssn = last4ssn;
                    }
                    string _emp = last4ssn + "\t" + lname + "\t" + fname;
                    createExceptionReport(_emp, "Wageworks", yrmo);
                    return false;
                }
                command = new SqlCommand("INSERT INTO Wageworks(wgwk_ssn, wgwk_lname, wgwk_fname, wgwk_yrmo, wgwk_tranclass, wgwk_transtype, wgwk_createdt, wgwk_amt) "
                                                    + "VALUES(@ssn, @lname, @fname, @yrmo, 'TRN', @transtype, @createdt, @amt)", connect);
                command.Parameters.AddWithValue("@ssn", _ssn);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@transtype", transtype);
                command.Parameters.AddWithValue("@createdt", createdt);
                command.Parameters.AddWithValue("@amt", amt);
                command.ExecuteNonQuery();
                return true;
            }           
            finally
            {
                connect.Close();
            }
        }

        public void insertAUDITR(String _qy, String ssn, int empno, String name, Nullable<DateTime> dob, int age, Nullable<DateTime> statusdt, Nullable<DateTime> modifydt)
        {
            string cmdstr = "INSERT INTO [hra_AUDITR] "
                                   + "([empno] "
                                   + ",[period] "
                                   + ",[ssn] "
                                   + ",[name] "
                                   + ",[dob] "
                                   + ",[age] "
                                   + ",[statusdt] "
                                   + ",[modifydt]) "
                                   + "VALUES "
                                   + "(@empno "
                                   + ",@period "
                                   + ",@ssn "
                                   + ",@name "
                                   + ",@dob "
                                   + ",@age "
                                   + ",@statusdt "
                                   + ",@modifydt)";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@empno", empno);
                command.Parameters.AddWithValue("@period", _qy);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@dob", dob);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@statusdt", statusdt);
                command.Parameters.AddWithValue("@modifydt", modifydt);
                command.ExecuteNonQuery(); command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public void insertPutnamInvoice(String _qy, String vendor, String period, Int32 partcount, Decimal amount)
        {
            string cmdstr = "INSERT INTO hra_PutnamInvoice "
                                + "(quarteryear "
                                + ",vendor "
                                + ",period "
                                + ",partcount "
                                + ",amount) "
                                + "VALUES "
                                + "(@qy "
                                + ", @vendor "
                                + ", @period "
                                + ",@partcount "
                                + ",@amount) ";
            

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@vendor", vendor);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@partcount", partcount);
                command.Parameters.AddWithValue("@amount", amount);
                command.ExecuteNonQuery();
                command.Dispose();                
            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean insertPutnamPartData(String ssn, String fname, String lname, String partStatDesc, SqlDateTime dob, SqlDateTime termdt, Decimal balance, String source, String _qy)
        {
            string cmdstr;            

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                cmdstr = "INSERT INTO hra_PartDataInvoice "
                                + "([ssn] "
                                + ",[source] "
                                + ",[termdt] "
                                + ",[period] "
                                + ",[balance] "
                                + ",[lastname] "
                                + ",[firstname] "
                                + ",[partStatDesc] "
                                + ",[dob] "
                                + ") "
                                + "VALUES "
                                + "(@ssn "
                                + ", @source "
                                + ", @termdt "
                                + ", @qy "
                                + ", @balance "
                                + ", @lname "
                                + ", @fname "
                                + ", @partStatDesc "
                                + ", @dob "
                                + ")";

                //Issue 48 - Alerts with Exception file for Non-HRA participants
                if (!CheckHRAParticipant(ssn))
                {
                    string _emp = "" + ssn + "\t" + lname + "\t" + fname;
                    createExceptionReport(_emp, "PutnamParticipantData", _qy);
                    return false;
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@source", source);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@balance", balance);
                command.Parameters.AddWithValue("@partStatDesc", partStatDesc);                
                command.Parameters.AddWithValue("@termdt", termdt);
                command.Parameters.AddWithValue("@dob", dob);
               
                command.ExecuteNonQuery();
                command.Dispose();
                return true;
            }           
            finally
            {
                connect.Close();
            }
        }

        public Boolean insertWageworkInvoice(String lname, String fname, String last4ssn, String source, String yrmo)
        {
            string _ssn, cmdstr; 

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                _ssn = GetSSN(last4ssn, lname.Substring(0, 3));
                //Issue 48 - Alerts with Exception file for Non-HRA participants
                if (_ssn == null)
                {
                    string _emp = last4ssn + "\t" + lname + "\t" + fname;
                    createExceptionReport(_emp, "WageworksInvoice", yrmo);
                    return false;
                }                

                cmdstr = "INSERT INTO hra_PartDataInvoice "
                                + "([ssn] "
                                + ", [lastname] "
                                + ", [firstname] "
                                + ",[source] "
                                + ",[period] "
                                + ") "
                                + "VALUES "
                                + "(@ssn "
                                + ", @lname "
                                + ", @fname "
                                + ", @source "                               
                                + ", @yrmo )";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@ssn", _ssn);
                command.Parameters.AddWithValue("@lname", lname);
                command.Parameters.AddWithValue("@fname", fname);
                command.Parameters.AddWithValue("@source", source);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                command.Dispose();
                return true;
            }
            finally
            {
                connect.Close();
            }
        }
        
        string GetSSN(string last4ssn, string first3lname)
        {
            string _ssn = null;
            last4ssn = last4ssn.PadLeft(4, '0');
           
            string cmdstr = "SELECT empl_ssn AS [SSN] " 
                                + "FROM Employee  WHERE "
                                + "SUBSTRING(CONVERT(VARCHAR(10),empl_ssn), LEN(CONVERT(VARCHAR(10),empl_ssn)) - 3 , 4) = @last4ssn "
                                + "AND empl_hra_part = 1 "
                                + "AND SUBSTRING(empl_lname, 1, 3) = @first3lname";

            command = new SqlCommand(cmdstr, connect);
            command.Parameters.AddWithValue("@last4ssn", last4ssn);
            command.Parameters.AddWithValue("@first3lname", first3lname);
            object result = command.ExecuteScalar();

            if ((result != null) && (result != DBNull.Value))
            {
                _ssn = result.ToString();
            }

            return _ssn;
        }

        public static String GetClientFilePath(string _category, string source)
        {
            string cmdStr = "SELECT [filelocation] FROM [FileMaintainence] WHERE (([module] = 'HRA') AND ([classification] = 'Import') AND ([category] = @category) AND ([sourcecd] = @source))";
            string filepath = "";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@source", source);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    filepath = result.ToString();
                }

                return filepath;
            }
            finally
            {
                connect.Close();
            }
        }

        //Checks if particpant on HRA
        Boolean CheckHRAParticipant(string _ssn)
        {
            string cmdstr = "SELECT COUNT(*) FROM Employee WHERE "
                                + "CONVERT(INT, LTRIM(RTRIM(REPLACE(empl_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', '')))) "
                                + "AND empl_hra_part = 1";

            command = new SqlCommand(cmdstr, connect);
            command.Parameters.AddWithValue("@ssn", _ssn);
            int _count = Convert.ToInt32(command.ExecuteScalar());

            if (_count > 0) { return true; }
            else { return false; }
        }

        //exception report of Non-HRA participants on HRA file imports
        public static void createExceptionReport(string _emp, string _source, string yrmo)
        {
            string _header = "";
            if(_source.Contains("Wageworks")) {_header = "Last4SSN\tLast Name\tFirst Name";}
            else { _header = "SSN\tLast Name\tFirst Name"; }
            
            string _fname = HttpContext.Current.Server.MapPath("~/uploads/") + "Exceptions_" + _source + "_" + yrmo + ".txt";
            FileInfo f1 = new FileInfo(_fname);

            if (File.Exists(_fname))
            {
                StreamReader sr = new StreamReader(_fname);

                if (!sr.ReadToEnd().Contains(_emp))
                {
                    sr.Close();
                    StreamWriter sw = f1.AppendText();
                    sw.WriteLine(_emp);
                    sw.Close();
                }
                else
                {
                    sr.Close();
                }
            }
            else
            {
                StreamWriter sw1 = f1.AppendText();
                sw1.WriteLine(_header);
                sw1.WriteLine(_emp);
                sw1.Close();
            }
        }
    }
}
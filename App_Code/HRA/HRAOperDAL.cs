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
using System.Collections.Specialized;
using System.IO;

namespace EBA.Desktop.HRA
{
    public class HRAOperDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRAOperDAL()
        {  
        }

        public DataSet GetEmployeeInfo()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT  empl_empno AS [empno] "
                                + ", empl_ssn AS [ssn] "
	                            + ", empl_lname AS [lname] "
                                + ", empl_fname AS [fname] "
	                            + ", addr_addr1 As [addr1] "
                                + ", addr_addr2 As [addr2] "
                                + ", addr_city As [city] "
                                + ", addr_state As [state] "
                                + ", addr_zip As [zip] "
                                + ", empl_sex As [sex] "
                                + ", empl_dob As [dob] "
                                + ", empl_retire_dt As [retiredt] "
                                + ", empl_fulltime_dt As [permftdt] "
                                + ", empl_statcd As [statuscd] "
                                + ", empl_stat_dt As [statusdt] "
                                + "FROM  Employee " 
                                + "INNER JOIN Address "
                                + "ON (empl_empno = addr_empno)"
                                + "AND addr_type = '001' "
                                + "WHERE empl_hra_part = 1 " 
                                + "ORDER BY empl_empno";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetEligAddrData()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsFinal = new DataSet(); dsFinal.Clear();

            string cmdstr1 = "SELECT  empl_empno AS [empno] "  
                                + ", empl_lname + ', ' + empl_fname AS [name] " 
                                + ", ISNULL(addr_addr1,'') As [addr1] "   
                                + ", ISNULL(addr_addr2,'') As [addr2] "   
                                + ", ISNULL(addr_city,'') As [city] "   
                                + ", ISNULL(addr_state,'') As [state] "   
                                + ", ISNULL(addr_zip,'') As [zip] "  
                                + "FROM  Employee, Address "    
                                + "WHERE empl_empno = addr_empno "
                                + "AND empl_hra_part = 1 "
                                + "AND empl_statcd <> 'DTH' "
                                + "AND addr_type = '001' "
                                + "ORDER BY empl_empno"; 

            string cmdstr2 = "SELECT  empl_empno AS [empno] "  
                                    + ", empl_lname +  ', ' + empl_fname AS [name] "
                                    + ", (dbo.hra_getDepAddr('addr1', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [addr1] "
                                    + ", (dbo.hra_getDepAddr('addr2', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [addr2] "   
                                    + ", (dbo.hra_getDepAddr('city', dpnd_empno, dpnd_ssn, dpnd_add_diff)) As [city] "   
                                    + ", (dbo.hra_getDepAddr('state', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [state] "   
                                    + ", (dbo.hra_getDepAddr('zip', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [zip] "         
                                    + "FROM  Employee, Dependant "  
                                    + "WHERE empl_empno = dpnd_empno " 
                                    + "AND empl_hra_part = 1 "
                                    + "AND empl_statcd = 'DTH' "    
                                    + "AND (dpnd_owner = 1 AND dpnd_validated = 1) "
                                    + "ORDER BY empl_empno";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                da.SelectCommand = command;
                da.Fill(dsTemp); command.Dispose();
                command = new SqlCommand(cmdstr2, connect);
                da.SelectCommand = command;
                da.Fill(dsFinal); command.Dispose();

                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    dsFinal.Merge(dsTemp); dsTemp.Clear();
                }

                return dsFinal;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetEligStatData()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            string cmdstr = "SELECT empl_empno AS [empno] "  
                                + ", empl_lname + ', ' + empl_fname AS [name] "
                                + ", empl_statcd AS [statcd] "   
                                + ", CASE "
                                    + "WHEN empl_statcd = 'DTH' THEN CONVERT(VARCHAR(15),empl_stat_dt, 101) "
                                    + "WHEN empl_statcd = 'TRM' THEN CONVERT(VARCHAR(15),empl_retire_dt, 101) " 
	                                + "END AS [statdt] "
                                + "FROM Employee "
                                + "WHERE empl_hra_part = 1 "
                                + "ORDER BY empl_empno";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds); command.Dispose();               

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetDependantInfo()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            string cmdstr = "SELECT  dpnd_empno AS [dep_empno] "
                                + ", dpnd_ssn AS [dep_ssn] "
                                + ", dpnd_lname AS [dep_lname] "
                                + ", dpnd_fname AS [dep_fname] "
                                + ", dpnd_sex AS [dep_sex] "
                                + ", dpnd_dob AS [dep_dob] "
                                + ", dpnd_owner AS [ownerflg] "
                                + ", dpnd_validated As [validflg] "
                                + ", dpnd_owner_eff_dt AS [ownereffdt] "
                                + ", dpnd_relationship AS [dep_rel] "
                                + "FROM  Dependant";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetDependantInfo(string empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT  dpnd_lname AS [dep_lname] "  
                                    + ", dpnd_fname AS [dep_fname] "  
                                    + ", (dbo.hra_getDepAddr('addr1', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [dep_addr1] "   
                                    + ", (dbo.hra_getDepAddr('addr2', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [dep_addr2] "   
                                    + ", (dbo.hra_getDepAddr('city', dpnd_empno, dpnd_ssn, dpnd_add_diff)) As [dep_city] "   
                                    + ", (dbo.hra_getDepAddr('state', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [dep_state] "   
                                    + ", (dbo.hra_getDepAddr('zip', dpnd_empno, dpnd_ssn, dpnd_add_diff)) AS [dep_zip] "         
                                    + ", dpnd_sex AS [dep_sex] "   
                                    + ", dpnd_dob AS [dep_dob] "
                                    + ", dpnd_relationship AS [dep_rel] "
                                    + ", dpnd_owner AS [ownerflg] "   
                                    + ", dpnd_validated As [validflg] "   
                                    + ", dpnd_owner_eff_dt AS [ownereffdt] "
                                    + ", dpnd_owner_stop_dt AS [stopdt] "  
                                    + "FROM  Dependant "
                                    + "WHERE dpnd_empno = @empno";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@empno", empno);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetPrevEligData(string _source)
        {
            DataSet ds = new DataSet(); ds.Clear();
            StringReader _reader;
            object _temp;
            string cmdstr = "SELECT [content] FROM [hra_content] WHERE [category] = 'HRAElig' AND [source] = @source";

             try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@source", _source);
                _temp = command.ExecuteScalar();
                if (_temp != DBNull.Value && _temp != null)
                {
                    _reader = new StringReader(_temp.ToString());
                    ds.ReadXml(_reader);
                }

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetChgEligData(string _source)
        {
            DataSet ds = new DataSet(); ds.Clear();
            StringReader _reader;
            string _temp;
            string cmdstr = "SELECT [content] FROM [hra_content] WHERE [category] = 'HRAElig' AND [source] = @source";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@source", _source);
                _temp = command.ExecuteScalar().ToString();
                _reader = new StringReader(_temp);
                ds.ReadXml(_reader);

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }
       
        public static DataSet GetEligAuditData(string _source, string _period)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_Elig", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = _source;
                command.Parameters.Add("@period", SqlDbType.VarChar).Value = _period;
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();

                return ds;
            }
            finally
            {
                connect.Close();
            }            
        }

        public static String GetHRAPlanNum()
        {
            string HRAPlanNo = "";
            string cmdstr = "SELECT LTRIM(RTRIM(codeid)) FROM hra_codes WHERE description = 'HRAPLANNO'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                HRAPlanNo = command.ExecuteScalar().ToString();
                
                return HRAPlanNo;
            }
            finally
            {
                connect.Close();
            }
        }

        public static NameValueCollection GetCodeIds()
        {
            SqlDataReader dr = null;
            NameValueCollection codeids = new NameValueCollection();
            string cmdstr = "SELECT LTRIM(RTRIM(description)), LTRIM(RTRIM(codeid)) FROM hra_codes WHERE description <> 'HRAPLANNO'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                dr = command.ExecuteReader();
                                
                while (dr.Read())
                {
                    codeids.Add(dr.GetString(0), dr.GetString(1));                   
                }

                return codeids;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertEligFileContent()
        {
            HRAEligFile eobj = new HRAEligFile();
            DataSet ds1 = new DataSet(); ds1.Clear(); ds1 = GetEligAddrData();
            DataSet ds2 = new DataSet(); ds2.Clear(); ds2 = GetEligStatData();
            DataSet dschange1 = new DataSet(); dschange1 = eobj.GetAddrChgData(ds1);
            DataSet dschange2 = new DataSet(); dschange2 = eobj.GetStatChgData(ds2);
            string _filecontent1 = ds1.GetXml();
            string _filecontent2 = ds2.GetXml();
            string _filecontent3;
            if (dschange1.Tables[0].Rows.Count > 0)
            {
                _filecontent3 = dschange1.GetXml();
            }
            else
            {
                _filecontent3 = dschange1.GetXmlSchema();
            }
            string _filecontent4;
            if (dschange2.Tables[0].Rows.Count > 0)
            {
                _filecontent4 = dschange2.GetXml();
            }
            else
            {
                _filecontent4 = dschange2.GetXmlSchema();
            }
            string _uid = HttpContext.Current.User.Identity.Name;
            ldapClient lobj = new ldapClient();
            UserRecord ud = new UserRecord();
            
            
            ud = lobj.SearchUser(_uid);
            string _username = ud.FirstName + ", " + ud.LastName;
            
            string cmdstr1 = "DELETE FROM [hra_content] WHERE [category] = 'HRAElig'";
            string cmdstr2 = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[source] "
                                + ",[content] "
                                + ",[createduser]) "
                                + "VALUES "
                                + "('HRAElig' "
                                + ",'EligAddr' "
                                + ",@filecontent1 "
                                + ",@username)";
            string cmdstr3 = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[source] "
                                + ",[content] "
                                + ",[createduser]) "
                                + "VALUES "
                                + "('HRAElig' "
                                 + ",'EligStat' "
                                + ",@filecontent2 "
                                + ",@username)";
            string cmdstr4 = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[source] "
                                + ",[content] "
                                + ",[createduser]) "
                                + "VALUES "
                                + "('HRAElig' "
                                 + ",'ChgEligAddr' "
                                + ",@filecontent3 "
                                + ",@username)";
            string cmdstr5 = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[source] "
                                + ",[content] "
                                + ",[createduser]) "
                                + "VALUES "
                                + "('HRAElig' "
                                 + ",'ChgEligStat' "
                                + ",@filecontent4 "
                                + ",@username)";



            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.ExecuteNonQuery(); command.Dispose();
                command = new SqlCommand(cmdstr2, connect);
                command.Parameters.AddWithValue("@filecontent1", _filecontent1);
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery(); command.Dispose();
                command = new SqlCommand(cmdstr3, connect);
                command.Parameters.AddWithValue("@filecontent2", _filecontent2);
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery(); command.Dispose();
                command = new SqlCommand(cmdstr4, connect);
                command.Parameters.AddWithValue("@filecontent3", _filecontent3);
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery(); command.Dispose();
                command = new SqlCommand(cmdstr5, connect);
                command.Parameters.AddWithValue("@filecontent4", _filecontent4);
                command.Parameters.AddWithValue("@username", _username);
                command.ExecuteNonQuery(); command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }        

        public static NameValueCollection GetEligFileCreatedMsg()
        {
            SqlDataReader dr = null;
            NameValueCollection eligFileMsg = new NameValueCollection();
            string cmdstr = "SELECT [timecreated], [createduser] FROM [hra_content] WHERE [category] = 'HRAElig'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                dr = command.ExecuteReader();

                if (dr.Read())
                {
                    eligFileMsg.Add("timecreated", dr.GetDateTime(0).ToString());
                    eligFileMsg.Add("createduser", dr.GetString(1));
                }

                return eligFileMsg;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void DeleteEligFileContent()
        {
            string cmdstr = "DELETE FROM [hra_content] WHERE [source] = 'EligFile'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery(); command.Dispose();               
            }
            finally
            {
                connect.Close();
            }
        }        

        public static decimal GetHRABalance(string empno)
        {
            decimal HRABalance = 0;
            object result;
            string cmdstr = "SELECT balance FROM hra_PartDataInvoice WHERE source = 'ptnm_partdata' AND empno = @empno";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@empno", Convert.ToInt32(empno));
                result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    HRABalance = Convert.ToDecimal(result);
                }
                
                return HRABalance;
            }
            finally
            {
                connect.Close();
            }
        }

        //when dependant reaches age 24, insert stop date
        public static void InsertStopDate()
        {
            string cmdstr = "UPDATE Dependant " 
                                + "SET dpnd_owner_stop_dt = (SELECT DATEADD(yy, 24, dpnd_dob)) " 
                                + "WHERE dpnd_owner = 1 " 
                                + "AND dpnd_validated = 1 " 
                                + "AND dpnd_relationship = 'CH' "
                                + "AND dbo.getAge(dpnd_dob, GETDATE()) >= 24";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery(); command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertAuditR()
        {
            DataSet ds = new DataSet(); ds.Clear();
            ds = GetEligAuditData("HRAAUDITR", String.Empty);
            string cmdstr1, cmdstr2, ssn, name, status, _period;
            int empno, age, _uid;            
            Nullable<DateTime> createdt, statusdt, modifydt, dob;

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                _uid = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
                createdt = DateTime.Today;
                _period = HRA.GetQuarterYear(DateTime.Today);
                cmdstr1 = "DELETE FROM [hra_AUDITR] WHERE [period] = @period";
                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@period", _period);
                command.ExecuteNonQuery(); command.Dispose();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    empno = Convert.ToInt32(row["Emp#"]);
                    ssn = row["SSN"].ToString();
                    name = row["Name"].ToString();
                    dob = Convert.ToDateTime(row["Birth Date"]);
                    age = Convert.ToInt32(row["Age"]);
                    status = row["Status"].ToString();
                    statusdt = Convert.ToDateTime(row["Status Date"]);
                    modifydt = Convert.ToDateTime(row["Modify Date"]);
                    
                    cmdstr2 = "INSERT INTO [hra_AUDITR] "
                                   + "([empno] "
                                   + ",[period] "
                                   + ",[userID] "
                                   + ",[createdt] "
                                   + ",[ssn] "
                                   + ",[name] "
                                   + ",[dob] "
                                   + ",[age] "
                                   + ",[status] "
                                   + ",[statusdt] "
                                   + ",[modifydt]) "
                                   + "VALUES "
                                   + "(@empno "
                                   + ",@period "
                                   + ",@uid "
                                   + ",@createdt "
                                   + ",@ssn "
                                   + ",@name "
                                   + ",@dob "
                                   + ",@age "
                                   + ",@status "
                                   + ",@statusdt "
                                   + ",@modifydt)";
                    
                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@empno", empno);
                    command.Parameters.AddWithValue("@period", _period);
                    command.Parameters.AddWithValue("@uid", _uid);
                    command.Parameters.AddWithValue("@createdt", createdt);
                    command.Parameters.AddWithValue("@ssn", ssn);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@dob", dob);
                    command.Parameters.AddWithValue("@age", age);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@statusdt", statusdt);
                    command.Parameters.AddWithValue("@modifydt", modifydt);
                    command.ExecuteNonQuery(); command.Dispose();
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasElig()
        {
            Boolean hasElig;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE empl_hra_part = 1", connect);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasElig = true;
                }
                else
                {
                    hasElig = false;
                }
                command.Dispose();

                return hasElig;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasTerm()
        {
            Boolean hasTerm;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE empl_hra_part = 1 AND (empl_statcd = 'DTH' OR empl_statcd = 'TRM')", connect);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasTerm = true;
                }
                else
                {
                    hasTerm = false;
                }
                command.Dispose();

                return hasTerm;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasAddrChg()
        {
            Boolean hasAddrChg;
            DataSet ds = new DataSet(); ds.Clear();

            ds = HRAOperDAL.GetChgEligData("ChgEligAddr");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                hasAddrChg = true;
            }
            else
            {
                hasAddrChg = false;
            }

            return hasAddrChg;
        }

        public static Boolean hasStatChg()
        {
            Boolean hasStatChg;
            DataSet ds = new DataSet(); ds.Clear();

            ds = HRAOperDAL.GetChgEligData("ChgEligStat");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                hasStatChg = true;
            }
            else
            {
                hasStatChg = false;
            }

            return hasStatChg;
        }

        public static Boolean hasNoBen()
        {
            Boolean hasNoBen;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE empl_hra_part = 1 AND empl_statcd = 'DTH' AND ((SELECT COUNT(*) FROM Dependant WHERE dpnd_empno = empl_empno) = 0)", connect);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasNoBen = true;
                }
                else
                {
                    hasNoBen = false;
                }
                command.Dispose();

                return hasNoBen;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasBen24()
        {
            Boolean hasBen24;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Dependant WHERE dpnd_relationship = 'CH' AND (dpnd_owner = 1 AND dpnd_validated = 1) AND ((SELECT dbo.getAge(dpnd_dob, GETDATE())) >= 24)", connect);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasBen24 = true;
                }
                else
                {
                    hasBen24 = false;
                }
                command.Dispose();

                return hasBen24;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasNoBenOwner()
        {
            Boolean hasNoBenOwner;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Employee, vw_hra_BenLtr WHERE (empl_empno = EmpNum) AND empl_hra_part = 1 AND empl_statcd = 'DTH' AND ((SELECT COUNT(*) FROM Dependant WHERE dpnd_empno = empl_empno AND (dpnd_owner = 1 AND dpnd_validated = 1)) <> 1) AND empl_empno IN (SELECT EmpNum FROM vw_hra_BenLtr, Dependant WHERE DepSSN = dpnd_ssn AND dpnd_owner_stop_dt IS NULL )", connect);
                count = Convert.ToInt32(command.ExecuteScalar());

                command = new SqlCommand("SELECT COUNT(*) FROM  Employee WHERE empl_statcd = 'DTH' AND empl_hra_part = 1 AND ((SELECT COUNT(*) FROM Dependant WHERE dpnd_empno = empl_empno AND (dpnd_owner = 1 AND dpnd_validated = 1)) <> 1 ) AND empl_empno NOT IN (SELECT EmpNum FROM vw_hra_BenLtr, Dependant WHERE DepSSN = dpnd_ssn AND dpnd_owner_stop_dt IS NULL )", connect);
                count = count + Convert.ToInt32(command.ExecuteScalar());

                if (count != 0)
                {
                    hasNoBenOwner = true;
                }
                else
                {
                    hasNoBenOwner = false;
                }
                command.Dispose();

                return hasNoBenOwner;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasChildBenLtr()
        {
            Boolean hasChildBenLtr;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM Dependant, vw_hra_ChildBenLtr60 WHERE dpnd_validated = 0 AND (daysDiff > 60)", connect);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasChildBenLtr = true;
                }
                else
                {
                    hasChildBenLtr = false;
                }
                command.Dispose();

                return hasChildBenLtr;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasDeathXDays(string deathdays)
        {
            Boolean _result;
            int count = 0;
            string cmdstr = "SELECT  count(*) "
			                    + "FROM  Employee, Dependant "  
			                    + "WHERE  dpnd_empno = empl_empno "
			                    + "AND empl_statcd = 'DTH' "
			                    + "AND empl_hra_part = 1 "
                                + "AND DATEDIFF(dd, empl_stat_dt, GETDATE()) <= @deathdays";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@deathdays", deathdays);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }        
    }
}
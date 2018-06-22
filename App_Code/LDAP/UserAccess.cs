using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop;
using EBA.Desktop.Admin;
using System.Data.SqlClient;
using System.Data.SqlTypes;

/// <summary>
/// Summary description for UserAccess
/// </summary>
/// 

namespace EBA.Desktop
{
    public class UserAccess
    {
        private string connStr = null;
        private SqlConnection conn;

        public UserAccess()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        public bool hasAccess(string _userId)
        {
            bool _access = false;
            int _recCount;
            bool _isActive = false;
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string cmdStr = "SELECT count(*) FROM UserLogin_AD WHERE empID = @userid";
                cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@userid", SqlDbType.Int);
                cmd.Parameters["@userid"].Value = Int32.Parse(_userId);
                _recCount = Int32.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();
                if (_recCount != 0)
                {
                    cmdStr = "SELECT IsActive FROM UserLogin_AD WHERE empID = @userid";
                    cmd = new SqlCommand(cmdStr, conn);
                    cmd.Parameters.Add("@userid", SqlDbType.Int);
                    cmd.Parameters["@userid"].Value = Int32.Parse(_userId);
                    _isActive = bool.Parse(cmd.ExecuteScalar().ToString());
                    if (_isActive)
                    {
                        _access = true;
                    }
                }
                else
                {
                    throw (new Exception("You are not approved to access the application!"));
                }


            }
            catch (Exception ex)
            {
                throw (new Exception("Login Failed!"));
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return _access;
        }
        
        public bool isLocked(string _userId)
        {
            bool _islocked = false;
            loginRecord iRec = new loginRecord();
            iRec = getFlag(_userId);
            int _attempCount = iRec.AttemptCount;
            bool _accountLocked = Convert.ToBoolean(iRec.AccountLocked);
            if (_accountLocked)
            {
                _islocked = true;
            }            
            return _islocked;
        }

        //reset invalid flags if any on Valid login
        public void ValidLogin(string _userId)
        {
            loginRecord jRec = new loginRecord();
            jRec.AttemptCount = 0;
            jRec.AccountLocked = 0;          
            setFlag(_userId, jRec);
        }


        //set flags on invalid login
        public void InvalidLogin(string _userId)
        {
            loginRecord iRec = new loginRecord();
            iRec = getFlag(_userId);
            int _attempCount = iRec.AttemptCount;
            int _accountLocked = iRec.AccountLocked;            

            if (_attempCount <3)
            {
                loginRecord jRec = new loginRecord();
                jRec.AttemptCount = _attempCount + 1;
                jRec.AccountLocked = 0;
                setFlag(_userId, jRec);
            }
            else
            {
                loginRecord jRec = new loginRecord();
                jRec.AttemptCount = _attempCount;
                jRec.AccountLocked = 1;
                setFlag(_userId, jRec);
                throw (new Exception("Invalid Login! You have reached maximum number of login attempts.<br/> Your account is Locked<br/> Contact your administrator to unlock your account."));
            }
        }


        //get login flags from user record
        protected loginRecord getFlag(string _userId)
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = null;
            loginRecord iRec = new loginRecord();
            string cmdStr = "SELECT iAttempt, isLocked FROM UserLogin_AD WHERE empID = @userid";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@userid", SqlDbType.Int);
            cmd.Parameters["@userid"].Value = Int32.Parse(_userId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                iRec.AttemptCount = dr.GetInt32(0);
                iRec.AccountLocked = Convert.ToInt32(dr[1]);
            }
            dr.Close();
            conn.Close();
            return iRec;
        }

        //set login flags 
        protected void setFlag(string _userID, loginRecord iRec)
        {

            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = null;
            string cmdStr = "UPDATE UserLogin_AD SET iAttempt = @attempt, isLocked = @locked WHERE empID = @userid";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@userid", SqlDbType.Int);
            cmd.Parameters["@userid"].Value = Int32.Parse(_userID);
            cmd.Parameters.Add("@attempt", SqlDbType.Int);
            cmd.Parameters["@attempt"].Value = iRec.AttemptCount;
            cmd.Parameters.Add("@locked", SqlDbType.Int);
            cmd.Parameters["@locked"].Value = iRec.AccountLocked;
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd.Dispose();
        }
       
    }

    public class loginRecord
        {
            private int _attemptCount = 0;
            int _accountLocked = 0;

            public int AttemptCount
            {
                get
                {
                    return _attemptCount;
                }
                set
                {
                    _attemptCount = value;
                }
            }

            public int AccountLocked
            {
                get
                {
                    return _accountLocked;
                }
                set
                {
                    _accountLocked = value;
                }
            }
        }
}

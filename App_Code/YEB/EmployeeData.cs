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
using System.Data.SqlClient;
using System.Data.SqlTypes;

/// <summary>
/// Summary description for EmployeeData
/// </summary>
/// 
namespace EBA.Desktop.YEB
{
    public class EmployeeData
    {
        private string connStr = null;
        private SqlConnection conn;


        public EmployeeData()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        /// <summary>
        /// Checks whether Employee with given Employee SSN exists.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>True or False</returns>
        public bool checkExistRecord(string ssn)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Employee WHERE empl_ssn = @ssn";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }
        /// <summary>
        /// Gets Employees record from database for given Employee ssn.
        /// 
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>Employee Record</returns>
        public EmployeeRecord getEmployeeData(string ssn)
        {
            string _cmdstr = " SELECT [empl_empno],[empl_ssn],[empl_fname],[empl_lname],[empl_minit]"
                                + "FROM dbo.Employee WHERE (empl_ssn = @ssn)";

            EmployeeRecord eRec1 = new EmployeeRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);

            SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eRec1 = assignData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return eRec1;
        }

        public EmployeeRecord getEmployeeData(int empNo)
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }

            string _cmdstr = " SELECT [empl_empno],[empl_ssn],[empl_fname],[empl_lname],[empl_minit]"
                                + "FROM dbo.Employee WHERE (empl_empno = @empNo)";

            EmployeeRecord eRec1 = new EmployeeRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empNo", empNo);

            SqlDataReader reader;
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    eRec1 = assignData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return eRec1;
        }


        protected EmployeeRecord assignData(SqlDataReader reader1)
        {
            EmployeeRecord eRec = new EmployeeRecord();
            eRec.EmpNum = Convert.ToInt32(reader1["empl_empno"]);
            eRec.SSN = reader1["empl_ssn"].ToString();
            eRec.FirstName = reader1["empl_fname"].ToString();
            eRec.LastName = reader1["empl_lname"].ToString();
            eRec.MiddleInitial = reader1["empl_minit"].ToString();
            return eRec;
        }

        public void UpdateEmployeeData(EmployeeRecord updEmp)
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }

            string _cmdstr = " UPDATE dbo.Employee SET [empl_ssn] = '" + updEmp.SSN.Trim()
                                + "'  WHERE (empl_empno = @empNo)";

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empNo", updEmp.EmpNum);

            command.ExecuteNonQuery();
            conn.Close();

        }

        //public bool createAuditObject(object oldValue, object newValue)
        //{
        //    bool _check = false;
        //    if (!oldValue.ToString().Equals(newValue.ToString()))
        //    {
        //        _check = true;
        //    }
        //    return _check;
        //}

        public DataSet getEmployeeAuditData(string EmpNo)
        {

            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }

            string _cmdstr = " SELECT dbo.UserTasks_AU.tColumn, dbo.UserTasks_AU.tOldValue, dbo.UserTasks_AU.tNewValue, dbo.UserSession_AU.UserName, dbo.UserTasks_AU.taskDate,dbo.UserTasks_AU.tPrimaryKey,dbo.UserTasks_AU.moduleName ";
            _cmdstr = _cmdstr + " FROM dbo.UserTasks_AU INNER JOIN ";
            _cmdstr = _cmdstr + " dbo.UserSession_AU ON dbo.UserTasks_AU.sessionID = dbo.UserSession_AU.SessionId ";
            _cmdstr = _cmdstr + " WHERE (dbo.UserTasks_AU.moduleName = 'yeb') AND (dbo.UserTasks_AU.tColumn = 'SSN') ";
            _cmdstr = _cmdstr + " AND tPrimaryKey = " + EmpNo;

            DataSet dsAudit = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(_cmdstr, conn);

            sqlAdp.Fill(dsAudit);
            return dsAudit;

        }

        public DataSet getYEBAuditData()
        {

            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn = new SqlConnection(connStr);
                conn.Open();
            }

            string _cmdstr = " SELECT dbo.UserTasks_AU.tColumn, dbo.UserTasks_AU.tOldValue, dbo.UserTasks_AU.tNewValue, dbo.UserSession_AU.UserName, dbo.UserTasks_AU.taskDate,dbo.UserTasks_AU.tPrimaryKey,dbo.UserTasks_AU.moduleName ";
            _cmdstr = _cmdstr + " FROM dbo.UserTasks_AU INNER JOIN ";
            _cmdstr = _cmdstr + " dbo.UserSession_AU ON dbo.UserTasks_AU.sessionID = dbo.UserSession_AU.SessionId ";
            _cmdstr = _cmdstr + " WHERE (dbo.UserTasks_AU.moduleName = 'yeb') AND (dbo.UserTasks_AU.tColumn = 'SSN') ";

            DataSet dsAudit = new DataSet();
            SqlDataAdapter sqlAdp = new SqlDataAdapter(_cmdstr, conn);

            sqlAdp.Fill(dsAudit);
            return dsAudit;

        }

    }
}
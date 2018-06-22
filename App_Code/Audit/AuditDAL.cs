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

/// <summary>
/// Summary description for AuditDAL
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class AuditDAL
    {
        private string connStr;
        private SqlConnection connect = null;
        private SqlCommand command = null;

        public AuditDAL()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            connect = new SqlConnection(connStr);
        }

        public void userSession(string _userid, string _userName, string _sessionId)
        {
            string cmdstr = "INSERT INTO UserSession_AU (UserID, UserName, SessionValue) VALUES (@uid,@uname,@seid)";
            command = new SqlCommand(cmdstr, connect);
            command.Parameters.AddWithValue("@uid", _userid);
            command.Parameters.AddWithValue("@uname", _userName);
            command.Parameters.AddWithValue("@seid", _sessionId);
            try
            {
                connect.Open();
                command.ExecuteNonQuery();
            }            
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public void userTask(string _sessionid,string _modID,string _utaskID, string _modName, string _taskName,string _taskType,string _dType,string _table, string _column,string _primaryKey,string _primaryName, string _oldV, string _newV)
        {
            int seqNo = getCurrentSessionId(_sessionid);
            string cmdstr = "INSERT INTO UserTasks_AU (sessionID, moduleID,userTaskID,moduleName,taskName,taskType,tType,tTable,tColumn,tPrimaryKey,tPrimaryName,tOldValue,tNewValue) "
                            + " VALUES (@seid,@modid,@utid,@modname,@tname,@ttype,@type,@table,@col,@pkey,@pname,@ov,@nv)";
           
            SqlTransaction ts;

            connect.Open();
            ts = connect.BeginTransaction();
            try
            {                
                command = new SqlCommand(cmdstr, connect,ts);
                command.Parameters.AddWithValue("@seid", seqNo);
                command.Parameters.AddWithValue("@modid", _modID);
                command.Parameters.AddWithValue("@utid", _utaskID);
                command.Parameters.AddWithValue("@modname", _modName);
                command.Parameters.AddWithValue("@tname", _taskName);
                command.Parameters.AddWithValue("@ttype", _taskType);
                command.Parameters.AddWithValue("@type", _dType);
                command.Parameters.AddWithValue("@table", _table);
                command.Parameters.AddWithValue("@col", _column);                
                command.Parameters.AddWithValue("@pkey", _primaryKey);
                command.Parameters.AddWithValue("@pname", _primaryName);
                command.Parameters.AddWithValue("@ov", _oldV);
                command.Parameters.AddWithValue("@nv", _newV);                
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (SqlException sqlErr)
            {
                ts.Rollback();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        /// <summary>
        /// Audit Import Routine.
        /// </summary>
        /// <param name="_sessionid">User session id</param>
        /// <param name="_modID">Module Id</param>
        /// <param name="_utaskID">Task Id</param>
        /// <param name="_modName">Module Name</param>
        /// <param name="_taskName">Task Name</param>
        /// <param name="_taskType">RC</param>
        /// <param name="_dType">Insert</param>
        /// <param name="_table">Importing Table</param>
        /// <param name="_reconType">Import</param>
        /// <param name="_reconName">Importing Module Name</param>
        /// <param name="_yrmo">YRMO</param>
        /// <param name="_count">Count of records</param>
        public void userTaskImport(string _sessionid, string _modID, string _utaskID, string _modName, string _taskName, string _table, string _reconName, string _yrmo, int _count)
        {
            int seqNo = getCurrentSessionId(_sessionid);
            string cmdstr = "INSERT INTO UserTasks_AU (sessionID, moduleID,userTaskID,moduleName,taskName,taskType,tType,tTable,tReconType,tReconName,tYrmo,tCount) "
                            + " VALUES (@seid,@modid,@utid,@modname,@tname,@ttype,@type,@table,@rtype,@rname,@yrmo,@cnt)";

            SqlTransaction ts;
            connect.Open();
            ts = connect.BeginTransaction();

            try
            {
                command = new SqlCommand(cmdstr, connect, ts);
                command.Parameters.AddWithValue("@seid", seqNo);
                command.Parameters.AddWithValue("@modid", _modID);
                command.Parameters.AddWithValue("@utid", _utaskID);
                command.Parameters.AddWithValue("@modname", _modName);
                command.Parameters.AddWithValue("@tname", _taskName);
                command.Parameters.AddWithValue("@ttype", "RC");
                command.Parameters.AddWithValue("@type", "Insert");
                command.Parameters.AddWithValue("@table", _table);                
                command.Parameters.AddWithValue("@rtype", "Import");
                command.Parameters.AddWithValue("@rname", _reconName);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@cnt", _count);
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (SqlException sqlErr)
            {
                ts.Rollback();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        /// <summary>
        /// Audit Reconciliation Routine.
        /// </summary>
        /// <param name="_sessionid">User session Id</param>
        /// <param name="_modID">Module Id</param>
        /// <param name="_utaskID">Task Id</param>
        /// <param name="_modName">Module Name</param>
        /// <param name="_taskName">Task Name</param>
        /// <param name="_taskType">RC</param>
        /// <param name="_dType">Select</param>
        /// <param name="_reconType">Reconciliation</param>
        /// <param name="_reconName">Recon module name</param>
        /// <param name="_yrmo">YRMO</param>
        public void userTaskRecon(string _sessionid, string _modID, string _utaskID, string _modName, string _taskName, string _reconName, string _yrmo)
        {
            int seqNo = getCurrentSessionId(_sessionid);
            string cmdstr = "INSERT INTO UserTasks_AU (sessionID, moduleID,userTaskID,moduleName,taskName,taskType,tType,tReconType,tReconName,tYrmo) "
                            + " VALUES (@seid,@modid,@utid,@modname,@tname,@ttype,@type,@rtype,@rname,@yrmo)";
           
            SqlTransaction ts;
            connect.Open();
            ts = connect.BeginTransaction();

            try
            {               
                command = new SqlCommand(cmdstr, connect, ts);
                command.Parameters.AddWithValue("@seid", seqNo);
                command.Parameters.AddWithValue("@modid", _modID);
                command.Parameters.AddWithValue("@utid", _utaskID);
                command.Parameters.AddWithValue("@modname", _modName);
                command.Parameters.AddWithValue("@tname", _taskName);
                command.Parameters.AddWithValue("@ttype", "RC");               
                command.Parameters.AddWithValue("@type", "Select");                                
                command.Parameters.AddWithValue("@rtype", "Reconciliation");
                command.Parameters.AddWithValue("@rname", _reconName);
                command.Parameters.AddWithValue("@yrmo", _yrmo);                
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (SqlException sqlErr)
            {
                ts.Rollback();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        /// <summary>
        /// Audit Import Routine Non-Recon Type.
        /// </summary>
        /// <param name="_sessionid">User session id</param>
        /// <param name="_modID">Module Id</param>
        /// <param name="_utaskID">Task Id</param>
        /// <param name="_modName">Module Name</param>
        /// <param name="_taskName">Task Name</param>
        /// <param name="_taskType">RC</param>
        /// <param name="_dType">Insert</param>
        /// <param name="_table">Importing Table</param>
        /// <param name="_reconType">Import</param>
        /// <param name="_reconName">Importing Module Name</param>
        /// <param name="_yrmo">YRMO</param>
        /// <param name="_count">Count of records</param>
        public void userTaskImportNRC(string _sessionid, string _modID, string _utaskID, string _modName, string _taskName, string _table, string _reconName, string _yrmo, int _count)
        {
            int seqNo = getCurrentSessionId(_sessionid);
            string cmdstr = "INSERT INTO UserTasks_AU (sessionID, moduleID,userTaskID,moduleName,taskName,taskType,tType,tTable,tReconType,tReconName,tYrmo,tCount) "
                            + " VALUES (@seid,@modid,@utid,@modname,@tname,@ttype,@type,@table,@rtype,@rname,@yrmo,@cnt)";

            SqlTransaction ts;
            connect.Open();
            ts = connect.BeginTransaction();

            try
            {
                command = new SqlCommand(cmdstr, connect, ts);
                command.Parameters.AddWithValue("@seid", seqNo);
                command.Parameters.AddWithValue("@modid", _modID);
                command.Parameters.AddWithValue("@utid", _utaskID);
                command.Parameters.AddWithValue("@modname", _modName);
                command.Parameters.AddWithValue("@tname", _taskName);
                command.Parameters.AddWithValue("@ttype", "NRC");
                command.Parameters.AddWithValue("@type", "Insert");
                command.Parameters.AddWithValue("@table", _table);
                command.Parameters.AddWithValue("@rtype", "Import");
                command.Parameters.AddWithValue("@rname", _reconName);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@cnt", _count);
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (SqlException sqlErr)
            {
                ts.Rollback();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        protected Int32 getCurrentSessionId(string _sessionId)
        {
            int seqID;

            string cmdstr = "SELECT SessionID FROM UserSession_AU WHERE SessionValue = @seid";
            command = new SqlCommand(cmdstr, connect);            
            command.Parameters.AddWithValue("@seid", _sessionId);
            try
            {
                connect.Open();
                seqID = Convert.ToInt32(command.ExecuteScalar());
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            return seqID;
        }
    }
}

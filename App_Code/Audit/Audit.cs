using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Audit
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class Audit
    {
        public Audit()
        {

        }

        /// <summary>
        /// Audit user login and session.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userName">User Name</param>
        /// <param name="sessionId">Session ID</param>
        public static void auditUserSession(string userId, string userName, string sessionId)
        {
            AuditDAL aObj = new AuditDAL();
            aObj.userSession(userId, userName, sessionId);
        }

        /// <summary>
        /// Audit user tasks, and Database DDL, DML commands.
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <param name="moduleId">Module ID</param>
        /// <param name="moduleName">Module Name</param>
        /// <param name="taskName">Task Name</param>
        /// <param name="taskType">Task Type - NRC (non recon), RC (recon)</param>
        /// <param name="dbType">Insert/Delete/Update/Select</param>
        /// <param name="tableName">Table Name</param>
        /// <param name="columnName">Table Column Name</param>
        /// <param name="pkey">Primary key</param>        
        /// <param name="oldv">Old Value</param>
        /// <param name="newv">New Value</param>
        public static void auditUserTask(string sessionId, string moduleId, string utaskId, string moduleName, string taskName, string taskType, string dbType, string tableName, string columnName, string pkey,string pName,string oldv, string newv)
        {
            AuditDAL aObj = new AuditDAL();
            aObj.userTask(sessionId, moduleId, utaskId, moduleName, taskName, taskType, dbType, tableName, columnName, pkey,pName,oldv, newv);
        }
        
        public static void auditUserTaskI(string sessionId, string moduleId, string utaskId, string moduleName, string taskName,string tableName,  string reconName, string yrmo, int count)
        {
            AuditDAL aObj = new AuditDAL();
            aObj.userTaskImport(sessionId, moduleId, utaskId, moduleName, taskName, tableName, reconName, yrmo, count);
                
        }

        public static void auditUserTaskINRC(string sessionId, string moduleId, string utaskId, string moduleName, string taskName, string tableName, string reconName, string yrmo, int count)
        {
            AuditDAL aObj = new AuditDAL();
            aObj.userTaskImportNRC(sessionId, moduleId, utaskId, moduleName, taskName, tableName, reconName, yrmo, count);

        }

        public static void auditUserTaskR(string sessionId, string moduleId, string utaskId, string moduleName, string taskName, string reconName, string yrmo)
        {
            AuditDAL aObj = new AuditDAL();
            aObj.userTaskRecon(sessionId, moduleId, utaskId, moduleName, taskName, reconName, yrmo);

        }

    }
}

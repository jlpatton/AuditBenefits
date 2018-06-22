using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using System.IO;

/// <summary>
/// Summary description for auditPage
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class auditPageBLL
    {
        public auditPageBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //// Session expired handling
        //public static void isSessionActive()
        //{
        //    if (HttpContext.Current.Session["uid"].ToString().Equals(""))
        //    {
        //        LogoutBLL.sessionEnd();
        //        HttpContext.Current.Response.Redirect("~/login.aspx");
        //    }
        //}
        //Audit Page
        public static void auditPagevisit(string path)
        {
            string pName = GetCurrentPage(path);
            int pid = Convert.ToInt32(HttpContext.Current.Session["pid"]);
            auditEBA.auditPage(pName, HttpContext.Current.Session.SessionID, pid);
        }
        //Audit moudules.
        public static void auditmodule(string path)
        {
            string pName = GetCurrentPage(path);
            int mid = incrModule();
            int pid = Convert.ToInt32(HttpContext.Current.Session["pid"]);
            auditEBA.auditPage(pName, HttpContext.Current.Session.SessionID, pid);

        }

        //Audit Transactions
        public static void auditTransaction(string path, string module, string subTask)
        {
            string pName = GetCurrentPage(path);
            int pid = incrParent();
            int modseqno = Int32.Parse(HttpContext.Current.Session["mid"].ToString());
            auditEBA.auditPage(pName, HttpContext.Current.Session.SessionID, pid);
            auditEBA.auditTrans(HttpContext.Current.Session.SessionID, modseqno, pid, module, subTask);

        }

        //Audit child transactions
        public static void auditchildTransaction(string path, string childtask, string tasktype)
        {
            string pName = GetCurrentPage(path);
            int pid = Convert.ToInt32(HttpContext.Current.Session["pid"]);
            int cid = incrChild();
            int modseqno = Int32.Parse(HttpContext.Current.Session["mid"].ToString());
            auditEBA.auditPage(pName, HttpContext.Current.Session.SessionID, pid);
            auditEBA.auditChildTrans(pid, cid, modseqno, HttpContext.Current.Session.SessionID, childtask);
            auditEBA.auditMaint(pid, cid, modseqno, HttpContext.Current.Session.SessionID, tasktype);
        }

        //Audit reconciliation
        public static void auditImportRecon(string yrmo, int cnt, string report, string recontype)
        {
            int pid = Convert.ToInt32(HttpContext.Current.Session["pid"]);
            int cid = incrChild();
            int modseqno = Int32.Parse(HttpContext.Current.Session["mid"].ToString());
            auditEBA.auditChildTrans(pid, cid, modseqno, HttpContext.Current.Session.SessionID, report);
            auditEBA.auditRecon(pid, cid, modseqno, HttpContext.Current.Session.SessionID, recontype, yrmo, cnt);
        }


        //Method returns current page
        private static string GetCurrentPage(string path)
        {
            string sPath = path;
            //FileInfo oInfo = new FileInfo(sPath);
            string sRet = sPath;
            return sRet;
        }

        //increment module id
        private static int incrModule()
        {
            //New transaction increment mid-module id to 1
            Int32 mid = Int32.Parse(HttpContext.Current.Session["mid"].ToString()) + 1;
            HttpContext.Current.Session["mid"] = mid;
            HttpContext.Current.Session["pid"] = 0;
            HttpContext.Current.Session["cseqno"] = 0;
            return mid;
        }

        //increment parent id
        private static int incrParent()
        {
            //New transaction increment pid-parent id to 1
            Int32 pid = Int32.Parse(HttpContext.Current.Session["pid"].ToString()) + 1;
            HttpContext.Current.Session["pid"] = pid;
            HttpContext.Current.Session["cid"] = 0;
            return pid;
        }

        //increment child id
        private static int incrChild()
        {
            //New child transaction increment cid-child to 1
            int cid = Convert.ToInt32(HttpContext.Current.Session["cid"]) + 1;
            HttpContext.Current.Session["cid"] = cid;
            return cid;
        }
    }
}

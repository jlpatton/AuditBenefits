using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop.Audit;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //auditPageBLL.auditPagevisit(HttpContext.Current.Request.Url.AbsolutePath);
            endSession();
        }

    }

    protected void endSession()
    {
        //HttpContext.Current.Session["eTime"] = DateTime.Now;
        string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        string cmdStr = "UPDATE  UserSession_AU SET SessionEnd = @etime where SessionValue = @ssid";
        SqlConnection conn = new SqlConnection(connStr);
        conn.Open();
        SqlCommand cmd = new SqlCommand(cmdStr, conn);
        cmd.Parameters.Add("@ssid", SqlDbType.VarChar, 30);
        cmd.Parameters["@ssid"].Value = HttpContext.Current.Session.SessionID;
        cmd.Parameters.Add("@etime", SqlDbType.DateTime);
        cmd.Parameters["@etime"].Value = DateTime.Now;
        try
        {
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            
        }
        cmd.Dispose();
        conn.Dispose();
        //Clear session variables and end session
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        //to end previous session and start new session when logged in again within same browser
        HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
    }
}

<%@ Application Language="C#" %>
<%@ Import Namespace = "System.Data" %>
<%@ Import Namespace = "System.Web" %>
<%@ Import Namespace = "System.Data.SqlClient" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Session["UserSessionId"] = Guid.NewGuid();
        Session["mid"] = 0;

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
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
        Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        Response.Redirect("~/SessionTimedout.aspx");

    }
       
</script>

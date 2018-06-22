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

public partial class UserControls_TimerControl : System.Web.UI.UserControl
{
    private string _sessionExpiredRedirect;    

    protected void Page_Load(object sender, EventArgs e)
    {
        int milliseconds = 60000;
        TimerTimeout.Interval = (Session.Timeout) * milliseconds;
    }

    public string SessionExpiredRedirect
    {
        get { return _sessionExpiredRedirect; }
        set { _sessionExpiredRedirect = value; }
    }

    protected void TimerTimout_Tick(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(SessionExpiredRedirect))
        {
            if (SessionExpiredRedirect.IndexOf("~") == 0)
                Response.Redirect(
                    VirtualPathUtility.ToAppRelative(
                    SessionExpiredRedirect));
            else
            Response.Redirect(SessionExpiredRedirect);
        }       
    }
}

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

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            if (Session["userName"] == null || Session["pwd"] == null)
            {
                Response.Redirect("~/SessionTimedout.aspx");
            }

            lblName.Text = "Logged in as: " + "<b><font color=\"#036\">" + Session["userName"].ToString() + "</font></b>";
        }
    }
}

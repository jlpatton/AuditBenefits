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
using EBA.Desktop.Admin;

public partial class Anthem_Billing_Reconciliation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M100", "M101");
        }
    }
}

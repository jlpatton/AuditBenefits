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
using EBA.Desktop.HRA;
using System.Data.SqlTypes;
using EBA.Desktop.Admin;

public partial class PCaudit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager SM = ((ScriptManager)(Master.FindControl("ScriptManager1")));
        //SM.RegisterAsyncPostBackControl(btnSearch);
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M700", "M707");
        }
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        //string fromdt = string.IsNullOrEmpty(txtFromDt.Text) ? "" : txtFromDt.Text;
        DateTime dt_from = Convert.ToDateTime(txtFromDt.Text);
        //string todate = string.IsNullOrEmpty(txtToDt.Text) ? "" : txtToDt.Text;
        DateTime dt_to = Convert.ToDateTime(txtToDt.Text);
        

        DsktpData DsktpObj = new DsktpData();

        ds = DsktpObj.getPCAuditLog(dt_from,dt_to);
        if (ds.Tables[0].Rows.Count != 0)
        {
            GridView1.Visible = true;
            GridView1.DataSource = ds;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
        }
        else
        {
            clearPage();
            infoDiv1.Visible = true;
            lblInfo.Text = "There were no records returned for the search criteria.";
        }


    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    

    protected void clearPage()
    {

        infoDiv1.Visible = false;
        lblInfo.Text = "";
        txtFromDt.Text = "";
        txtToDt.Text = "";

        
        GridView1.Visible = false;

    }

    

}

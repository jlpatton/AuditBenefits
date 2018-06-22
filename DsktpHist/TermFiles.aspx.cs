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

public partial class TermFiles : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M700", "M705");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        int empno = Convert.ToInt32(txtEmpno.Text);
        

        DsktpData DtopData = new DsktpData();
        if (!string.IsNullOrEmpty(txtEmpno.Text))
        {
            if (DtopData.checkExistTermRecord(empno))
            {
                clearPage();
                TermFileRecord dRec = new TermFileRecord();
                dRec = DtopData.getTermFileData(empno);
                bindTermRecord(dRec);
            }
            else
            {
                clearPage();
                infoDiv1.Visible = true;
                lblInfo.Text = "There were no records returned for the search criteria.";
            }
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
        txtEmpno.Text = "";
        txtBoxNo.Text = "";
    }

    protected void bindTermRecord(TermFileRecord pRec1)
    {
        txtEmpno.Text = pRec1.EmpNo.ToString();
        txtBoxNo.Text = pRec1.StorageNo.ToString();

    }
    

}

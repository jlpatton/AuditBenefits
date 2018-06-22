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

public partial class RetPart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager SM = ((ScriptManager)(Master.FindControl("ScriptManager1")));
        //SM.RegisterAsyncPostBackControl(btnSearch);\
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M700", "M702");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        string lname = string.IsNullOrEmpty(txtSrchLname.Text) ? "" : txtSrchLname.Text;
        string fname = string.IsNullOrEmpty(txtSrchFname.Text) ? "" : txtSrchFname.Text;
        string empno = string.IsNullOrEmpty(txtSrchEmpno.Text) ? "" : txtSrchEmpno.Text;
        
        clearPage();
        

        string rtrvArg = txtSrchLname.Text;
        DsktpData DsktpObj = new DsktpData();
        
        ds = DsktpObj.searchRetirees(lname, fname, empno);
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

    protected void btnRetrieve_Click(object sender, EventArgs e)
    {
        
        DsktpData DtopData = new DsktpData();
        if (!string.IsNullOrEmpty(txtSSNo.Text))
        {
            if (DtopData.checkExistRetRecord(txtSSNo.Text))
            {
                string ssn = txtSSNo.Text;

                clearPage();
                RetPartRecord rRec = new RetPartRecord();
                rRec = DtopData.getRetPartData(ssn);
                bindRetPartRecord(rRec);

                DataSet ds = DtopData.searchRetElects(ssn);
                grvElections.DataSource = ds;
                grvElections.DataBind();

                int empno = Convert.ToInt32(txtEmpNo.Text);
                DataSet ds2 = DtopData.searchRetPayHist(ssn);
                grvPayments.DataSource = ds2;
                grvPayments.DataBind();

                DataSet ds3 = DtopData.searchRetPayrollHist(ssn);
                grvDeductions.DataSource = ds3;
                grvDeductions.DataBind();

            }
            else
            {
                clearPage();
                infoDiv1.Visible = true;
                lblInfo.Text = "There were no records returned for the search criteria.";
            }
        }
    }

    protected void clearPage()
    {
        infoDiv1.Visible = false;
        lblInfo.Text = "";
        lblEmpHeading.Text = "Employee - ";
        
        txtEmpNo.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtPType.Text = "";
        txtEStatus.Text = "";
        txtPathCd.Text = "";
        txtPathUserDate.Text = "";
        txtPathUserId.Text = "";
        txtDedAuthDate.Text = "";
        txtFormRecDate.Text = "";
        txtSSNo.Text = "";

        txtSrchFname.Text = "";
        txtSrchLname.Text = "";
        txtSrchEmpno.Text = "";

        GridView1.Visible = false;

    }

    protected void bindRetPartRecord(RetPartRecord pRec1)
    {
        lblEmpHeading.Text = "Employee - " + pRec1.EmpNo.ToString() + "/"
                                + pRec1.LastName + ", " + pRec1.FirstName;
        txtSSNo.Text = pRec1.SSN.ToString();
        txtEmpNo.Text = pRec1.EmpNo.ToString();
        txtFirstName.Text = pRec1.FirstName;
        txtLastName.Text = pRec1.LastName;
        txtPType.Text = pRec1.PType;
        txtEStatus.Text = pRec1.EStatus;
        txtDedAuthDate.Text = (pRec1.DedAuthDate.ToShortDateString() == "1/1/0001" ? "" : pRec1.DedAuthDate.ToShortDateString());
        txtFormRecDate.Text = (pRec1.FormRecDate.ToShortDateString() == "1/1/0001" ? "" : pRec1.FormRecDate.ToShortDateString());
        txtPathCd.Text = pRec1.PathCd;
        txtPathDate.Text = (pRec1.PathDt.ToShortDateString() == "1/1/0001" ? "" : pRec1.PathDt.ToShortDateString());
        txtPathUserId.Text = pRec1.PathUserID;
        txtPathUserDate.Text = (pRec1.PathUserDate.ToShortDateString() == "1/1/0001" ? "" : pRec1.PathUserDate.ToShortDateString());
        
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = GridView1.Rows[index];
            string dssn = Server.HtmlDecode(gvr.Cells[4].Text);
            txtSSNo.Text = dssn;
            
            DsktpData DtopData = new DsktpData();
            if (!string.IsNullOrEmpty(txtSSNo.Text))
            {
                if (DtopData.checkExistRetRecord(txtSSNo.Text))
                {
                    string ssn = txtSSNo.Text;

                    clearPage();
                    RetPartRecord dRec = new RetPartRecord();
                    dRec = DtopData.getRetPartData(ssn);
                    bindRetPartRecord(dRec);
                    DataSet ds = DtopData.searchRetElects(ssn);
                    grvElections.DataSource = ds;
                    grvElections.DataBind();

                    int empno = Convert.ToInt32(txtEmpNo.Text);
                    DataSet ds2 = DtopData.searchRetPayHist(ssn);
                    grvPayments.DataSource = ds2;
                    grvPayments.DataBind();

                    DataSet ds3 = DtopData.searchRetPayrollHist(ssn);
                    grvDeductions.DataSource = ds3;
                    grvDeductions.DataBind();
                }
                else
                {
                    infoDiv1.Visible = true;
                    lblInfo.Text = "There were no records returned for the search criteria.";
                }
            }
        }
    }
}

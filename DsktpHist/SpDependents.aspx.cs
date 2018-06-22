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

public partial class DsktpHist_SpDependents : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M700", "M701");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        string lname = string.IsNullOrEmpty(txtSrchLname.Text) ? "" : txtSrchLname.Text;
        string fname = string.IsNullOrEmpty(txtSrchFname.Text) ? "" : txtSrchFname.Text;
        string xempno = string.IsNullOrEmpty(txtSrchXEmpno.Text) ? "" : txtSrchXEmpno.Text;
        clearPage();

        string rtrvArg = txtSrchLname.Text;
        DsktpData DsktpObj = new DsktpData();
        
        ds = DsktpObj.searchSpDeps(lname, fname, xempno);
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
            lblInfo.Text = "There were no records returned for the search criteria.";
            infoDiv1.Visible = true;
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
            if (DtopData.checkExistRecord(txtSSNo.Text))
            {
                string ssn = txtSSNo.Text;

                clearPage();
                SpDepRecord dRec = new SpDepRecord();
                dRec = DtopData.getSpDepData(ssn);
                bindSpDepRecord(dRec);
                DataSet ds = DtopData.getSpDepElections(ssn);
                grvElections.DataSource = ds;
                grvElections.DataBind();

                int empno = Convert.ToInt32(txtXrefEmpno.Text);
                DataSet ds2 = DtopData.getSpDepNotes(empno, ssn);
                grvNotes.DataSource = ds2;
                grvNotes.DataBind();
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
        lblInfo.Text = "";
        infoDiv1.Visible = false;
        lblEmpHeading.Text = "Employee - ";
        txtXrefEmpno.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAddr1.Text = "";
        txtAddr2.Text = "";
        txtCity.Text = "";
        txtState.Text = "";
        txtZip.Text = "";
        txtDob.Text = "";
        txtRelCd.Text = "";
        txtPhone.Text = "";
        txtOrgCd.Text = "";
        txtSSNo.Text = "";
        txtSexCd.Text = "";

        txtSrchFname.Text = "";
        txtSrchLname.Text = "";
        txtSrchXEmpno.Text = "";

        GridView1.Visible = false;

    }

    protected void bindSpDepRecord(SpDepRecord pRec1)
    {
        lblEmpHeading.Text = "Employee - " + pRec1.EmpNum.ToString() + "/"
                                + pRec1.LastName + ", " + pRec1.FirstName + " " + pRec1.MiddleInitial;
        txtXrefEmpno.Text = pRec1.EmpNum.ToString();
        txtFirstName.Text = pRec1.FirstName;
        txtLastName.Text = pRec1.LastName;
        txtAddr1.Text = pRec1.Address1;
        txtAddr2.Text = pRec1.Address2;
        txtCity.Text = pRec1.City;
        txtState.Text = pRec1.State;
        txtZip.Text = pRec1.Zip;
        txtDob.Text = pRec1.DOB.ToShortDateString();
        txtRelCd.Text = pRec1.RelCode;
        txtPhone.Text = pRec1.Phone;
        txtOrgCd.Text = pRec1.OrgCode;

        txtSSNo.Text = pRec1.SSN.PadLeft(9, '0');
        string s = pRec1.SexCode;
        if (s.Equals("M"))
        {
            txtSexCd.Text = "Male";
        }
        else if (s.Equals("F"))
        {
            txtSexCd.Text = "Female";
        }
        
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
            string dssn = Server.HtmlDecode(gvr.Cells[3].Text);
            txtSSNo.Text = dssn;
            
            DsktpData DtopData = new DsktpData();
            if (!string.IsNullOrEmpty(txtSSNo.Text))
            {
                if (DtopData.checkExistRecord(txtSSNo.Text))
                {
                    string ssn = txtSSNo.Text;

                    clearPage();
                    SpDepRecord dRec = new SpDepRecord();
                    dRec = DtopData.getSpDepData(ssn);
                    bindSpDepRecord(dRec);
                    DataSet ds = DtopData.getSpDepElections(ssn);
                    grvElections.DataSource = ds;
                    grvElections.DataBind();

                    int empno = Convert.ToInt32(txtXrefEmpno.Text);
                    DataSet ds2 = DtopData.getSpDepNotes(empno, ssn);
                    grvNotes.DataSource = ds2;
                    grvNotes.DataBind();
                }
                else
                {
                    infoDiv1.Visible = true;
                    lblInfo.Text = "There were no records returned for the search criteria.";
                }
            }
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //if (e.NewPageIndex < 0) e.Cancel = true;
    }
}

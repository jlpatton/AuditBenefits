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

public partial class HipCert : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M700", "M703");
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
        
        ds = DsktpObj.searchHipCertParticipants(lname, fname, empno);
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
        infoDiv1.Visible = false;
        DsktpData DtopData = new DsktpData();
        if (!string.IsNullOrEmpty(txtSSNo.Text))
        {
            if (DtopData.checkExistHipCertRecord(txtSSNo.Text))
            {
                string ssn = txtSSNo.Text;

                clearPage();
                HipCertRecord rRec = new HipCertRecord();
                rRec = DtopData.getHipCertData(ssn);
                bindHipCertRecord(rRec);

                DataSet ds = DtopData.getHipHistCvg(ssn);
                grvCoverage.DataSource = ds;
                grvCoverage.DataBind();

                DataSet ds2 = DtopData.getHipDeps(ssn);
                grvDependents.DataSource = ds2;
                grvDependents.DataBind();

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
        lblEmpHeading.Text = "Employee - ";

        txtSSNo.Text = "";
        txtEmpno.Text = "";
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAddr1.Text = "";
        txtAddr2.Text = "";
        txtCity.Text = "";
        txtState.Text = "";
        txtZip.Text = "";
        txtDob.Text = "";
        txtLoadDt.Text = "";
        txtPrintDt.Text = "";
        txtQualCd.Text = "";
        txtQualDt.Text = "";
        txtSexCd.Text = "";
        txtMon18Flag.Text = "";

        txtSrchFname.Text = "";
        txtSrchLname.Text = "";
        txtSrchEmpno.Text = "";

        
        GridView1.Visible = false;

    }

    protected void bindHipCertRecord(HipCertRecord pRec1)
    {
        lblEmpHeading.Text = "Employee - " + pRec1.EmpNo.ToString() + "/"
                                + pRec1.LastName + ", " + pRec1.FirstName;
        txtSSNo.Text = pRec1.SSN.ToString();
        txtEmpno.Text = pRec1.EmpNo.ToString();
        txtFirstName.Text = pRec1.FirstName;
        txtLastName.Text = pRec1.LastName;
        txtAddr1.Text = pRec1.Address1;
        txtAddr2.Text = pRec1.Address2;
        txtCity.Text = pRec1.City;
        txtState.Text = pRec1.State;
        txtZip.Text = pRec1.Zip;
        txtDob.Text = (pRec1.DOB.ToShortDateString() == "1/1/0001" ? "" : pRec1.DOB.ToShortDateString());
        txtLoadDt.Text = (pRec1.LoadDate.ToShortDateString() == "1/1/0001" ? "" : pRec1.LoadDate.ToShortDateString());
        txtPrintDt.Text = (pRec1.PrintDt.ToShortDateString() == "1/1/0001" ? "" : pRec1.PrintDt.ToShortDateString());
        txtQualCd.Text = "";
        txtQualDt.Text = (pRec1.QualDt.ToShortDateString() == "1/1/0001" ? "" : pRec1.QualDt.ToShortDateString());
        txtSexCd.Text = "";
        txtMon18Flag.Text = (pRec1.Mon18Flag.ToString() == "1" ? "YES" : "NO");
               
    }


    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            infoDiv1.Visible = false;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = GridView1.Rows[index];
            string dssn = Server.HtmlDecode(gvr.Cells[4].Text);
            txtSSNo.Text = dssn;
            
            DsktpData DtopData = new DsktpData();
            if (!string.IsNullOrEmpty(txtSSNo.Text))
            {
                if (DtopData.checkExistHipCertRecord(txtSSNo.Text))
                {
                    string ssn = txtSSNo.Text;

                    clearPage();
                    HipCertRecord rRec = new HipCertRecord();
                    rRec = DtopData.getHipCertData(ssn);
                    bindHipCertRecord(rRec);

                    DataSet ds = DtopData.getHipHistCvg(ssn);
                    grvCoverage.DataSource = ds;
                    grvCoverage.DataBind();

                    DataSet ds2 = DtopData.getHipDeps(ssn);
                    grvDependents.DataSource = ds2;
                    grvDependents.DataBind();

                    txtSrchLname.Text = "";
                    txtSrchFname.Text = "";
                    txtSrchEmpno.Text = "";

                }
            }
        }
    }
}

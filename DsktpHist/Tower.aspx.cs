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

public partial class Tower : System.Web.UI.Page
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
        string empno = "";
        clearPage();

        string rtrvArg = txtSrchLname.Text;
        DsktpData DsktpObj = new DsktpData();

        ds = DsktpObj.searchTower(fname, lname, empno);
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

    protected void btnRetrieve_Click(object sender, EventArgs e)
    {
        
        DsktpData DtopData = new DsktpData();
        if (!string.IsNullOrEmpty(txtSSNo.Text))
        {
            if (DtopData.checkExistTowerRecord(txtSSNo.Text))
            {
                string ssn = txtSSNo.Text;

                clearPage();
                TowerEmpRecord dRec = new TowerEmpRecord();
                dRec = DtopData.getTowerEmpData(ssn);
                bindTowerRecord(dRec);

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
        txtFirstName.Text = "";
        txtLastName.Text = "";
        txtAddr1.Text = "";
        txtAddr2.Text = "";
        txtCity.Text = "";
        txtState.Text = "";
        txtZip.Text = "";
        txtDDOB.Text = "";
        txtRelCd.Text = "";
        txtPhone.Text = "";
        txtDfname.Text = "";
        txtDlname.Text = "";
        txtHealthInd.Text = "";
        txtSSNo.Text = "";

        txtSrchFname.Text = "";
        txtSrchLname.Text = "";
        

        GridView1.Visible = false;
    }

    protected void bindTowerRecord(TowerEmpRecord pRec1)
    {
        lblEmpHeading.Text = "Employee - " + pRec1.LastName + ", " + pRec1.FirstName;
        txtFirstName.Text = pRec1.FirstName;
        txtLastName.Text = pRec1.LastName;
        txtAddr1.Text = pRec1.Address1;
        txtAddr2.Text = pRec1.Address2;
        txtCity.Text = pRec1.City;
        txtState.Text = pRec1.State;
        txtZip.Text = pRec1.Zip;
        txtDDOB.Text = (pRec1.DDOB.ToShortDateString() == "1/1/0001" ? "" : pRec1.DDOB.ToShortDateString());
        txtPhone.Text = pRec1.Phone;
        txtDfname.Text = pRec1.DFname;
        txtDlname.Text = pRec1.DLname;
        txtRelCd.Text = pRec1.DRelCode;
        txtHealthInd.Text = pRec1.HealthInd;
        txtSSNo.Text = pRec1.SSN.PadLeft(9, '0');
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
                if (DtopData.checkExistTowerRecord(txtSSNo.Text))
                {
                    string ssn = txtSSNo.Text;

                    clearPage();
                    TowerEmpRecord dRec = new TowerEmpRecord();
                    dRec = DtopData.getTowerEmpData(ssn);
                    bindTowerRecord(dRec);

                }
                else
                {
                    clearPage();
                    infoDiv1.Visible = true;
                    lblInfo.Text = "There were no records returned for the search criteria.";
                }
            }
        }
    }
    
}

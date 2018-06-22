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
using EBA.Desktop;
using EBA.Desktop.Admin;
using EBA.Desktop.Audit;
using EBA.Desktop.YEB;
using EBA.Desktop.HRA;
using System.Collections.Generic;

public partial class YEB_Employee : System.Web.UI.Page
{

    EmployeeRecord eOldrec = new EmployeeRecord();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!this.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M600", "M601");

            this.displayRecord();
            change_mode("r");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearMessages();
        displayRecord();
        change_mode("r");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        clearMessages();
        if (Page.IsValid)
        {
            try
            {
                int _empno = Convert.ToInt32(txtEmpNo.Text);
                UpdateRecords(_empno);
                change_mode("r");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        change_mode("e");
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);

        switch (MultiView1.ActiveViewIndex)
        {

            case 0:
                break;

            case 1:
                this.DisplayAuditDetails();
                break;
        }

    }

    protected void DisplayAuditDetails()
    {
        EmployeeData objED = new EmployeeData();
        DataSet dsAudit = new DataSet();

        //dsAudit = objED.getEmployeeAuditData(this.txtEmpNo.Text.Trim());
        dsAudit = objED.getYEBAuditData();

        this.grdvAudit.DataSource = dsAudit.Tables[0].DefaultView;
        this.grdvAudit.DataBind();
    }


    // Diaplays Employee Record on Page Load.
    protected void displayRecord()
    {
        EmployeeRecord eRec = null;
        string _essn = null;



        EmployeeData eData = new EmployeeData();

        if (Session["ssn"] != null)
        {
            _essn = Session["ssn"].ToString();
        }
        else
        {
            Response.Redirect("~/YEB/UpdateSSN.aspx");
        }
        if (_essn != null)
        {
            eRec = eData.getEmployeeData(_essn);
        }
        bindEmployeeRecord(eRec);
        eOldrec = eRec;
    }


    // Bind the Employee Details
    protected void bindEmployeeRecord(EmployeeRecord eRec1)
    {
        lblEmpHeading.Text = "Employee - " + eRec1.EmpNum.ToString() + "/"
                                + eRec1.LastName + ", " + eRec1.FirstName + " " + eRec1.MiddleInitial;

        txtEmpNo.Text = eRec1.EmpNum.ToString();
        txtFirst.Text = eRec1.FirstName;
        txtMI.Text = eRec1.MiddleInitial;
        txtLast.Text = eRec1.LastName;
        txtSSN.Text = eRec1.SSN.PadLeft(9, '0');
    }




    private void change_mode(string type)
    {
        switch (type)
        {
            case "r":

                txtEmpNo.ReadOnly = true;
                txtFirst.ReadOnly = true;
                txtLast.ReadOnly = true;
                txtMI.ReadOnly = true;
                txtSSN.ReadOnly = true;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                break;
            case "e":

                // Allow SSN only to edit
                txtSSN.ReadOnly = false;

                txtEmpNo.ReadOnly = true;
                txtFirst.ReadOnly = true;
                txtLast.ReadOnly = true;
                txtMI.ReadOnly = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                break;
        }
    }




    // Updates the New Employee details 
    protected void UpdateRecords(int _empno)
    {
        EmployeeRecord pUrec = new EmployeeRecord();
        EmployeeData uData = new EmployeeData();
        EmployeeRecord finalUrec = uData.getEmployeeData(_empno);
        string strSSN = this.txtSSN.Text.Trim();
        string pKey = txtEmpNo.Text;
        if (!(finalUrec.SSN.Equals(this.txtSSN.Text.Trim())))
        {

            pUrec.EmpNum = Convert.ToInt32(pKey);
            pUrec.SSN = Convert.ToInt64(strSSN).ToString();
            pUrec.FirstName = txtFirst.Text;
            pUrec.LastName = txtLast.Text;
            pUrec.MiddleInitial = txtMI.Text;

            uData.UpdateEmployeeData(pUrec);

            string sessionId = Session.SessionID;
            string moduleId = Session["mid"].ToString();
            string taskId = Session["taskId"].ToString();

            Audit.auditUserTask(sessionId, moduleId, taskId, "YEB", "Update SSN", "SSN", "Update", "Employee", "SSN", pKey, "", finalUrec.SSN, pUrec.SSN);

            this.infoDiv1.Visible = true;
            this.lblInfo.Text = "SSN is updated successfully";
            change_mode("r");
        }

    }
    private void clearMessages()
    {

        infoDiv1.Visible = false;
        lblInfo.Text = "";
    }
}

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
using EBA.Desktop.HRA;

public partial class HRA_Maintenance_Employee_Search : System.Web.UI.Page
{
    static DataSet dsstore;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }        
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M214");
            Session["ssn"] = null;
            Session["empno"] = null;
            ViewState["tempdata"] = "";
            EmpListDiv.Visible = false;
        }

    }
    protected void btnSSN_Click(object sender, EventArgs e)
    {
        EmpListDiv.Visible = false;
        string ssn = txtSSN.Text.ToString();
        Session["ssn"] = ssn;
        PilotData pObj = new PilotData();
        bool status = pObj.checkExistRecord(ssn);
        if (status)
        {
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Response.Redirect("~/HRA/Maintenance/Pilot.aspx");
        }
        else
        {
            lblError.Text = "Employee SSN not found!";
        }
       
    }
    protected void btnEmpno_Click(object sender, EventArgs e)
    {
        EmpListDiv.Visible = false;
        int empno = Int32.Parse(txtEmpno.Text.ToString()); 
        Session["empno"] = empno;
        PilotData pObj = new PilotData();
        bool status = pObj.checkExistRecord(empno);
        if (status)
        {
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Response.Redirect("~/HRA/Maintenance/Pilot.aspx");
        }
        else
        {
            lblError.Text = "Employee number not found!";
        }
    }
    protected void btnLastInitial_Click(object sender, EventArgs e)
    {
        EmpListDiv.Visible = false;
        string _linitial = txtLastInitial.Text;
        PilotData pObj = new PilotData();
        DataSet ds = pObj.searchPilot(_linitial);
        dsstore = ds;
        ViewState["tempdata"] = ds;        
        grdvEmployee.DataSource = ds;
        grdvEmployee.DataBind();
        SortGridView("empl_lname ASC, empl_fname ASC , empl_empno ASC", "");
        EmpListDiv.Visible = true;
    }

    protected void btnName_Click(object sender, EventArgs e)
    {
        EmpListDiv.Visible = false;
        string _fname = txtFirst.Text;
        string _lname = txtLast.Text;
        PilotData pObj = new PilotData();
        DataSet ds = pObj.searchPilot(_fname,_lname);
        dsstore = ds;
        ViewState["tempdata"] = ds;       
        grdvEmployee.DataSource = ds;
        grdvEmployee.DataBind();
        SortGridView("empl_lname ASC, empl_fname ASC , empl_empno ASC", "");
    }

    protected void grdvEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvEmployee.DataSource = dsstore;
        grdvEmployee.PageIndex = e.NewPageIndex;
        grdvEmployee.DataBind();
    }

    protected void grdvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gr = grdvEmployee.Rows[index];
            string ssn = Server.HtmlDecode(gr.Cells[4].Text);
            Session["ssn"] = ssn;
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Response.Redirect("~/HRA/Maintenance/Pilot.aspx");            
        }
    }

    protected void grdvEmployee_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression);
    }

    void gridviewSort(string sortExpression)
    {
        try
        {
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, DESCENDING);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, ASCENDING);
            }
        }
        catch (Exception ex)
        {

        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt;
        DataView dv;        
        
        DataSet ds = (DataSet) ViewState["tempdata"];
        dt = ds.Tables[0];
        dv = new DataView(dt);
        dv.Sort = sortExpression + direction;
        grdvEmployee.DataSource = dv;
        grdvEmployee.DataBind();        
    }

        
}

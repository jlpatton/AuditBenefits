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
using EBA.Desktop.YEB;


public partial class YEB_UpdateSSN : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M600", "M601");
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
        EmployeeData eObj = new EmployeeData();
        bool status = eObj.checkExistRecord(ssn);
        if (status)
        {
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Response.Redirect("~/YEB/Employee.aspx");
        }
        else
        {
            lblError.Text = "Employee SSN was not found!";
        }
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
            Response.Redirect("~/YEB/Employee.aspx");
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
            throw ex;
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

        DataSet ds = (DataSet)ViewState["tempdata"];
        dt = ds.Tables[0];
        dv = new DataView(dt);
        dv.Sort = sortExpression + direction;
        grdvEmployee.DataSource = dv;
        grdvEmployee.DataBind();

    }








}

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
using EBA.Desktop.VWA;
using EBA.Desktop.Admin;

public partial class VWA_VWA_Cases_Search : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M400", "M405");
        }
    }

    protected void btnName_Click(Object sender, EventArgs e)
    {
        clearMessages();
        if (txtFirst.Text.Equals("") && txtLast.Text.Equals(""))
        {
            errorDiv1.Visible = true;
            lblError1.Text = "One of the two name fields is required.";
        }
        else
        {
            try
            {
                DataTable dt = new DataTable();
                dt = VWA_DAL.employeeData(txtLast.Text, txtFirst.Text);
                grdvEmployee.DataSource = dt;
                grdvEmployee.DataBind();
                EmpListDiv.Visible = true;
            }
            catch (Exception ex)
            {
                errorDiv1.Visible = true;
                lblError1.Text = ex.Message;
            }
        }
    }

    protected void btnContract_Click(Object sender, EventArgs e)
    {
        //Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Response.Redirect("~/VWA/VWA_Cases.aspx?id=" + txtContract.Text);
    }

    protected void grdvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gr = grdvEmployee.Rows[index];
            string _contract = Server.HtmlDecode(gr.Cells[5].Text);            
            //Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Response.Redirect("~/VWA/VWA_Cases.aspx?id=" + _contract);
        }
    }

    protected void grdvEmployee_onSorting(Object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression);
    }

    void gridviewSort(string sortExpression)
    {
        clearMessages();
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
            errorDiv1.Visible = true;
            lblError1.Text = ex.Message;
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
        DataTable dt1 = new DataTable();
        dt1 = VWA_DAL.employeeData(txtLast.Text, txtFirst.Text);
        DataView dv;
        dv = new DataView(dt1);
        dv.Sort = sortExpression + direction;
        grdvEmployee.DataSource = dv;
        grdvEmployee.DataBind();
        EmpListDiv.Visible = true;

    }

    void clearMessages()
    {
        EmpListDiv.Visible = false;
        errorDiv1.Visible = false;
        lblError1.Text = "";
    }
}

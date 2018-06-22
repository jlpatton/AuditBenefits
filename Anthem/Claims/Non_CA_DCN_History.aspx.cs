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
using EBA.Desktop.Anthem;
using EBA.Desktop;
using EBA.Desktop.Admin;


public partial class Anthem_Claims_Non_CA_DCN_History : System.Web.UI.Page
{
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
            AdminBLL.InRole(uname, "M100", "M102");            
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string dcn = txtSearch.Text;
        ClaimsRecon dobj = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        ds = dobj.GetDCNHistory(dcn);
        //if (ds.Tables[0].Rows.Count == 0)
        //{
        //    historyDiv.Attributes.Add("style", "width:680px;height:50px;margin-top:20px;");
        //}
        //else
        //{
        //    historyDiv.Attributes.Add("style", "width:700px;margin-top:20px;");
        //}
        grdvHistory.DataSource = ds;
        grdvHistory.DataBind();
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
        ClaimsRecon dobj = new ClaimsRecon();
        string dcn = txtSearch.Text;

        dt = dobj.GetDCNHistory(dcn).Tables[0];
        dv = new DataView(dt);
        dv.Sort = sortExpression + direction;
        grdvHistory.DataSource = dv;
        grdvHistory.DataBind();
              
    }
    protected void grdvHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            string sortExpression = e.SortExpression;

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
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_xl_Click(Object sender, EventArgs e)
    {
        ClaimsRecon dobj = new ClaimsRecon();
        string dcn = txtSearch.Text;
        DataSet ds = new DataSet();
        ds.Clear();
       
        string filename = "DCN_History";
        string[][] cols ={ new string[] { "Source", "YRMO", "Amount", "Reconciled" } };
        string[][] colsFormat ={ new string[] { "string", "string", "decimal", "string" } };
        string[] sheetnames = { "DCN_history" };
        string[] titles = { "DCN History" };
        

        try
        {
            ds = dobj.GetDCNHistory(dcn);
            ExcelReport.ExcelXMLRpt(ds, filename, sheetnames, titles, cols, colsFormat);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }
   

}

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
using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public partial class Anthem_Billing_Reconciliation_Domestic : System.Web.UI.Page
{   
    int counter = 0;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M100", "M101");
            ddlYrmoList();
            checkPastRecon();
            resultDiv.Visible = false;
        }
    }

    protected void ddlYrmoList()
    {
        ImportDAL iobj = new ImportDAL();

        string prevYRMO;
        for (int i = 0; i < 6; i++)
        {
            prevYRMO = iobj.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }
    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
        {
            txtPrevYRMO.Text = "";
            ddlYrmo.Visible = false;
            txtPrevYRMO.Visible = true;
            btnAddYrmo.Visible = true;
            btnCancelYrmo.Visible = true;
            MultiView1.SetActiveView(view_empty);
        }
        else
        {
            checkPastRecon();
        }
    }

    protected void btn_ADDYrmo(object sender, EventArgs e)
    {
        int index = 0;
        if (!ddlYrmo.Items.Contains(new ListItem(txtPrevYRMO.Text.Trim())))
        {
            ddlYrmo.Items.Add(txtPrevYRMO.Text.Trim());
        }
        for (int i = 0; i < ddlYrmo.Items.Count; i++)
        {
            if (ddlYrmo.Items[i].Text == txtPrevYRMO.Text)
            {
                index = i;
            }
        }
        ddlYrmo.SelectedIndex = index;
        ddlYrmo.Visible = true;
        txtPrevYRMO.Visible = false;
        btnAddYrmo.Visible = false;
        btnCancelYrmo.Visible = false;
        checkPastRecon();
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        txtPrevYRMO.Visible = false;
        btnAddYrmo.Visible = false;
        btnCancelYrmo.Visible = false;
        checkPastRecon();
    }

    protected void checkPastRecon()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text.ToString();
        resultDiv.Visible = false;
        lbl_error.Text = "";
        if (ReconDAL.pastReconcile(_yrmo, "DOM"))
        {
            MultiView1.SetActiveView(view_reconAgn);
            lbl_reconAgn.Text = "Reconciled already for year-month (YRMO): " + _yrmo + ".<br />&nbsp;&nbsp;Do you want to re-reconcile?";
        }
        else
        {
            lbl_error.Text = "";            
            MultiView1.SetActiveView(view_main);
        }
    }
    protected void btn_reconcile_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        if (Page.IsValid)
        {
            lbl_error.Text = "";
            try
            { 
                
               // if (RptsImported())
                //Removed this if condition as it is not applicable to Dom Recon
                //R.A 10/30/2009
                {
                    ReconDAL.DomesticReconcile(yrmo,"ANTH_ACT","GRS","ACT");
                    ReconDAL.DomesticReconcile(yrmo, "ANTH_RET", "RET%", "RET");
                    ReconDAL.DomesticReconcile(yrmo, "ANTH_COB", "ADP", "COB");
                    ShowResult();
                    auditRecon(yrmo);
                }
            }
            catch (Exception ex)
            {
                ReconDAL.pastReconcileDelete(yrmo, "DOM");
                lbl_error.Text = "Error in reconciling<br />" + ex.Message;
            }           
        }
    }
    protected void btn_reconAgn_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        try
        {
            ReconDAL.pastReconcileDelete(yrmo, "DOM");
            //if (RptsImported())
            //Removed this if condition as it is not applicable to Dom Recon
            //R.A 10/30/2009
            {
                ReconDAL.DomesticReconcile(yrmo, "ANTH_ACT", "GRS", "ACT");
                ReconDAL.DomesticReconcile(yrmo, "ANTH_RET", "RET%", "RET");
                ReconDAL.DomesticReconcile(yrmo, "ANTH_COB", "ADP", "COB");
                ShowResult();
                auditRecon(yrmo);
            }
        }
        catch (Exception ex)
        {
            ReconDAL.pastReconcileDelete(yrmo, "DOM");
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }       

    }    
    protected void btn_genRpt_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        string[][] cols = { new string[] { "Reconciliation Type", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance"}, 
                            new string[] { "YRMO", "Anthem Group Suffix", "Anthem Covg. Code", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance", "EBA vs Anthem % Count Variance", "Threshold", "Threshold Level" }, 
                            new string[] { "YRMO", "Anthem Group Suffix", "Anthem Covg. Code", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance", "EBA vs Anthem % Count Variance", "Threshold", "Threshold Level" },
                            new string[] { "YRMO", "Anthem Group Suffix", "Anthem Covg. Code", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance", "EBA vs Anthem % Count Variance", "Threshold", "Threshold Level" }};
        string[][] colsFormat = {   new string[] {"string","number","decimal","number","decimal","number"}, 
                                    new string[] { "string", "string", "string", "number", "decimal", "number", "decimal", "number", "decimal", "decimal", "string" },
                                    new string[] { "string", "string", "string", "number", "decimal", "number", "decimal", "number", "decimal", "decimal", "string" },
                                    new string[] { "string", "string", "string", "number", "decimal", "number", "decimal", "number", "decimal", "decimal", "string" }};
        string[] sheetnames = {"Summary", "Active", "Retiree", "Cobra" };
        string[] titles = { "Domestic Billing Reconciliation Summary for YRMO - " + yrmo, "Active Billing Reconciliation for YRMO - " + yrmo, "Retiree Billing Reconciliation for YRMO - " + yrmo, "Cobra Billing Reconciliation for YRMO - " + yrmo };

        try
        {
            ds = ReconDAL.GetDomSummary(yrmo);
            ds.Tables[0].TableName = "sumTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "sumTableF";
            ds.Clear();
            ds = ReconDAL.GetDomReconData(yrmo,"ACT");
            ds.Tables[0].TableName = "actTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "actTableF";
            ds.Clear();
            ds = ReconDAL.GetDomReconData(yrmo, "RET");
            ds.Tables[0].TableName = "retTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "retTableF";
            ds.Clear();
            ds = ReconDAL.GetDomReconData(yrmo, "COB");
            ds.Tables[0].TableName = "cobTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[3].TableName = "cobTableF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, "DomesticRecon_" + yrmo, sheetnames, titles, cols,colsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }

    }

    protected void btnCobDet_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        string[][] cols = { new string[] { "YRMO", "Anthem Group Suffix", "Anthem Covg. Code", "Subscriber Id", "Subscriber Name", "EBA Count", "Anthem Count" }, new string[] { "Plan", "Plan Desc", "Subscriber ID", "Subscriber Name", "Division", "Coverage EffDt", "Coverage Level", "Coverage Period", "Comments", "Total Premium", "Total Carrier" }, new string[] { "Plan Code", "Subscriber ID", "Subscriber Name", "Coverage Code", "Coverage Period", "Premium" }, new string[] { "Plan", "Plan Desc", "Subscriber ID", "Subscriber Name", "Division", "Coverage EffDt", "Coverage Level", "Coverage Period", "Comments", "Total Premium", "Total Carrier" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "number", "number" }, new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "decimal", "decimal" }, new string[] { "string", "string", "string", "string", "string", "decimal" }, new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "decimal", "decimal" } };
        string[] sheetnames = { "Cobra_Recon_Details", "Unmatched_ADP_Cobra", "Unmatched_Anthem_Cobra","ADP_Cobra" };
        string[] titles = { "Cobra Reconciliation Detailed Report for YRMO - " + yrmo, "Subscribers in ADP Cobra Report not in Anthem Bill for YRMO - " + yrmo, "Subscribers in Anthem Bill not in ADP Cobra Report for YRMO - " + yrmo, "ADP Cobra Subscribers Details for YRMO - " + yrmo };

        try
        {
            ds = DetailReports.getCobraDetails(yrmo);
            ds.Tables[0].TableName = "cobDetTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "cobTableF";
            ds.Clear();
            ds = DetailReports.getADPUnmatched(yrmo);
            ds.Tables[0].TableName = "adpTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "adpTableF";
            ds.Clear();
            ds = DetailReports.getAnthCobUnmatched(yrmo);
            ds.Tables[0].TableName = "anthTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "anthTableF";
            ds.Clear();
            ds = DetailReports.getCobraDetailsReport(yrmo);
            ds.Tables[0].TableName = "cobDetTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "cobTableF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, "CobraReconDetails_" + yrmo, sheetnames, titles, cols, colsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }
    
    void ShowResult()
    {
        bindResult("ACT");
        bindResult("RET");
        bindResult("COB");
        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = true;  
    }
    protected void bindResult(string _src)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string yrmo = ddlYrmo.SelectedItem.Text;
        switch (_src)
        {
            case "ACT":
                ds = ReconDAL.GetDomReconData(yrmo, "ACT");
                grdvActResult.DataSource = ds;
                grdvActResult.DataBind();  
                break;
            case "RET":
                ds = ReconDAL.GetDomReconData(yrmo, "RET");
                grdvRetResult.DataSource = ds;
                grdvRetResult.DataBind();  
                break;
            case "COB":
                ds = ReconDAL.GetDomReconData(yrmo, "COB");
                grdvCobResult.DataSource = ds;
                grdvCobResult.DataBind();  
                break;
        }   
    }    

    protected void grdvActResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvActResult.PageIndex = e.NewPageIndex;
        bindResult("ACT");
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
    protected void grdvActResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "ACT");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "ACT");
        }   
    }
    private void SortGridView(string sortExpression, string direction, string source)
    {
        //  You can cache the DataTable for improving performance
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataTable dt = ReconDAL.GetDomReconData(yrmo, source).Tables[0];

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        switch (source)
        {
            case "ACT":
                grdvActResult.DataSource = dv;
                grdvActResult.DataBind();
                break;
            case "RET":
                grdvRetResult.DataSource = dv;
                grdvRetResult.DataBind();
                break;
            case "COB":
                grdvCobResult.DataSource = dv;
                grdvCobResult.DataBind();
                break;
        }        
    }
    protected void grdvRetResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvRetResult.PageIndex = e.NewPageIndex;
        bindResult("RET");
    }

    protected void grdvRetResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "RET");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "RET");
        }   
    }
    protected void grdvCobResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvCobResult.PageIndex = e.NewPageIndex;
        bindResult("COB");
    }

    protected void grdvCobResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "COB");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "COB");
        }   
    }
    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;

        if (!AnthImport.PastImport("GRS", yrmo))
        {
            lbl_error.Text = "GRS report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if(!AnthImport.PastImport("ADP", yrmo))
        {
            lbl_error.Text = "ADP COBRA report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!AnthImport.PastImport("ANTH", yrmo))
        {
            lbl_error.Text = "Anthem Bill for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!AnthImport.PastImport("RET_M", yrmo))
        {
            lbl_error.Text = "Retiree Medicare Report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!AnthImport.PastImport("RET_NM", yrmo))
        {
            lbl_error.Text = "Retiree Non Medicare Report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        return true;
    }

    protected void auditRecon(string yrmo)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Billing Reconciliation", "Domestic Billing Reconciliation", yrmo);
    }
    
    //protected void grdvActResult_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    gridViewSortingstyle(sender, e);
    //}
    //protected void grdvCobResult_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    gridViewSortingstyle(sender, e);
    //}
    //protected void grdvRetResult_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    gridViewSortingstyle(sender, e);
    //}

    //protected void gridViewSortingstyle(object sender, GridViewRowEventArgs e)
    //{
    //    GridView gridView = (GridView)sender;

    //    if (gridView.SortExpression.Length > 0)
    //    {
    //        int cellIndex = -1;
    //        //  find the column index for the sorresponding sort expression
    //        foreach (DataControlField field in gridView.Columns)
    //        {
    //            if (field.SortExpression == gridView.SortExpression)
    //            {
    //                cellIndex = gridView.Columns.IndexOf(field);
    //                break;
    //            }
    //        }

    //        if (cellIndex > -1)
    //        {
    //            if (e.Row.RowType == DataControlRowType.Header)
    //            {
    //                //  this is a header row,
    //                //  set the sort style
    //                e.Row.Cells[cellIndex].CssClass +=
    //                    (gridView.SortDirection == SortDirection.Ascending
    //                    ? " sortascheader" : " sortdescheader");
    //            }
    //            else if (e.Row.RowType == DataControlRowType.DataRow)
    //            {
    //                //  this is a data row
    //                e.Row.Cells[cellIndex].CssClass +=
    //                    (e.Row.RowIndex % 2 == 0
    //                    ? " sortaltrow" : "sortrow");
    //            }
    //        }
    //    }
    //}
}

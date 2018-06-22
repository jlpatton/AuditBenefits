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
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public partial class Anthem_Billing_Reconciliation_EAP : System.Web.UI.Page
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
        if (ReconDAL.pastReconcile(_yrmo, "EAP"))
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

                if (RptsImported())
                {
                    ReconDAL.insertEAPRecon(yrmo);
                    ShowResult();
                    auditRecon(yrmo);
                }
            }
            catch (Exception ex)
            {
                ReconDAL.pastReconcileDelete(yrmo, "EAP");
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
            ReconDAL.pastReconcileDelete(yrmo, "EAP");
            if (RptsImported())
            {
                ReconDAL.insertEAPRecon(yrmo);
                ShowResult();
                auditRecon(yrmo);
            }
        }
        catch (Exception ex)
        {
            ReconDAL.pastReconcileDelete(yrmo, "EAP");
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }

    }
    protected void btn_genRpt_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        ds.Clear();
        dsFinal.Clear();
        string[][] cols = { new string[] { "YRMO", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance", "EBA vs Anthem % Count Variance", "Threshold", "Threshold Level" }, new string[] { "YRMO", "Source", "EBA Count", "Anthem Count"} };
        string[][] colsFormat = { new string[] { "string", "number", "decimal", "number", "decimal", "number", "decimal", "decimal", "string" }, new string[] { "string", "string", "number", "number" } };
        string[] sheetnames = { "EAP","EAP_Count_Details" };
        string[] titles = { "EAP Billing Reconciliation for YRMO - " + yrmo, "EAP Billing Headcounts Detail for YRMO - " + yrmo };

        try
        {
            ds = ReconDAL.GetEAPReconData(yrmo);            
            ds.Tables[0].TableName = "eapTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "eapTableF";
            ds.Clear();
            ds = ReconDAL.GetEAPReconDetails(yrmo);
            ds.Tables[0].TableName = "detTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "detTableF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, "EAPRecon_" + yrmo, sheetnames, titles, cols,colsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }

    void ShowResult()
    {       
        bindResult();
        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = true;
    }
    protected void bindResult()
    {
        DataSet ds = new DataSet();       
        string yrmo = ddlYrmo.SelectedItem.Text;
        ds = ReconDAL.GetEAPReconData(yrmo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            resultDiv.Visible = true;            
            grdvResult.DataSource = ds;
            grdvResult.DataBind();
        }
        else
        {
            resultDiv.Visible = false;            
        }              
    }

    protected void grdvResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvResult.PageIndex = e.NewPageIndex;
        bindResult();
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
    protected void grdvResult_Sorting(object sender, GridViewSortEventArgs e)
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
    private void SortGridView(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataTable dt = ReconDAL.GetEAPReconData(yrmo).Tables[0];

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        grdvResult.DataSource = dv;
        grdvResult.DataBind();
     
    }
    protected void grdvResult_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            MultiView2.SetActiveView(viewDetails);
        }
    }

    protected void lnkBack_OnClick(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(viewResult);
    }
    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;

        if (!AnthImport.PastImport("GRS", yrmo))
        {
            lbl_error.Text = "GRS report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!AnthImport.PastImport("ADP", yrmo))
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
        Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Billing Reconciliation", "EAP Billing Reconciliation", yrmo);
    }
}

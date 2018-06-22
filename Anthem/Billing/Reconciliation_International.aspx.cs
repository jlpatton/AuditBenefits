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

public partial class Anthem_Billing_Reconciliation_International : System.Web.UI.Page
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
            detailDiv.Visible = false;
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
        lbl_error.Text = "";
        resultDiv.Visible = false;
        detailDiv.Visible = false;
        
        if (ReconDAL.pastReconcile(_yrmo, "INTL"))
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
                    ReconDAL.IntlReconcile(yrmo);
                    ShowResult();
                    auditRecon(yrmo);
                }                
            }
            catch (Exception ex)
            {
                ReconDAL.pastReconcileDelete(yrmo, "INTL");
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
            ReconDAL.pastReconcileDelete(yrmo, "INTL");
            if (RptsImported())
            {
                ReconDAL.IntlReconcile(yrmo);
                ShowResult();
                auditRecon(yrmo);
            }   
        }
        catch (Exception ex)
        {
            ReconDAL.pastReconcileDelete(yrmo, "INTL");
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }   
    }
    
    protected void btn_genRpt_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds.Clear();
        string[][] cols = { new string[] { "YRMO", "Anthem Group Suffix", "Anthem Covg. Code", "EBA Count", "EBA Amount", "Anthem Count", "Anthem Amount", "EBA Count Variance", "EBA vs Anthem % Count Variance", "Threshold", "Threshold Level" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "number", "decimal", "number", "decimal", "number", "decimal", "decimal", "string" } };
        string[] sheetnames = { "Intl_Bill_Recon" };
        string[] titles = { "International Billing Reconciliation for YRMO - " + yrmo };

        try
        {       
        ds = ReconDAL.GetIntlReconData(yrmo);
        ExcelReport.ExcelXMLRpt(ds, "InternationalRecon_" + yrmo, sheetnames, titles, cols,colsFormat);       
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
        bindHTH();
        bindANTH();
        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = true;
    }

    protected void bindResult()
    {
        DataSet ds = new DataSet();
        string yrmo = ddlYrmo.SelectedItem.Text;
        ds = ReconDAL.GetIntlReconData(yrmo);
        grdvResult.DataSource = ds;
        grdvResult.DataBind();
    }
    protected void bindHTH()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds1 = null;
        
        ds1 = ReconDAL.getUnmatch(yrmo);
        if (ds1.Tables["HTHDATA"].Rows.Count > 0)
        {
            hthDiv.Visible = true;
            detailDiv.Visible = true;
            grdvHTH.DataSource = ds1.Tables["HTHDATA"];
            grdvHTH.DataBind();
        }
        
    }

    protected void bindANTH()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds1 = new DataSet();
        ds1 = ReconDAL.getUnmatch(yrmo);
        if (ds1.Tables["ANTHDATA"].Rows.Count > 0)
        {
            anthdiv.Visible = true;
            detailDiv.Visible = true;
            grdvAnth.DataSource = ds1.Tables["ANTHDATA"];
            grdvAnth.DataBind();

        }
    }
    

    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;

        if (!AnthImport.PastImport("HTH", yrmo))
        {
            lbl_error.Text = "HTH for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if(!AnthImport.PastImport("ANTH", yrmo))
        {
            lbl_error.Text = "Anthem Report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        return true;   
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
            SortGridView(sortExpression, DESCENDING, "INTL");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "INTL");
        }   
    }
    private void SortGridView(string sortExpression, string direction, string source)
    {
        //  You can cache the DataTable for improving performance
        string yrmo = ddlYrmo.SelectedItem.Text;
        switch (source)
        {
            case "INTL":
                DataTable dt = ReconDAL.GetIntlReconData(yrmo).Tables[0];
                DataView dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvResult.DataSource = dv;
                grdvResult.DataBind();
                break;

            case "INTL_HTH":
                DataSet ds1 = new DataSet();
                ds1 = ReconDAL.getUnmatch(yrmo);
                if (ds1.Tables["HTHDATA"].Rows.Count > 0)
                {
                    DataTable dt1 = ds1.Tables["HTHDATA"];
                    DataView dv1 = new DataView(dt1);
                    dv1.Sort = sortExpression + direction;
                    grdvHTH.DataSource = dv1;
                    grdvHTH.DataBind();
                }
                break;
                
            case "INTL_ANTH":
                DataSet ds2 = new DataSet();
                ds2 = ReconDAL.getUnmatch(yrmo);
                if (ds2.Tables["ANTHDATA"].Rows.Count > 0)
                {
                    DataTable dt2 = ds2.Tables["ANTHDATA"];
                    DataView dv2 = new DataView(dt2);
                    dv2.Sort = sortExpression + direction;
                    grdvAnth.DataSource = dv2;
                    grdvAnth.DataBind();
                }
                break;
        }       
    }

    protected void grdvHTH_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvHTH.PageIndex = e.NewPageIndex;
        bindHTH();
    }

    protected void grdvHTH_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "INTL_HTH");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "INTL_HTH");
        }           
    }

    protected void grdvAnth_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvAnth.PageIndex = e.NewPageIndex;
        bindANTH();
    }

    protected void grdvAnth_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "INTL_ANTH");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "INTL_ANTH");
        }           
    }

    protected void auditRecon(string yrmo)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Billing Reconciliation", "International Billing Reconciliation", yrmo);
    }
    
   
}

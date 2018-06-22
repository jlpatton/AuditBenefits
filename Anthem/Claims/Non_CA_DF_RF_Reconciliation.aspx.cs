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

public partial class Anthem_Claims_Non_CA_RF_DF_Reconciliation : System.Web.UI.Page
{
    int counter = 0;
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
            ddlYrmoList();
            checkPastRecon();
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
        DFAgingDiv.Visible = false;
        DetailsDiv.Visible = false;
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
        if (ReconDAL.pastReconcile(_yrmo, "RFDF"))
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
        checkPastRecon();
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = ""; 
        if (Page.IsValid)
        {
            ClaimsRecon cObj = new ClaimsRecon();
            try
            {
                string _monthList = AnthRecon.CheckReconOrder(yrmo, "Non-CA");
                if (_monthList.Equals(""))
                {
                    
                    if (RptsImported())
                    {
                        //AnthRecon.matchDFCl(yrmo);
                        cObj.rfdfFirstVerification(yrmo);
                        cObj.CompareFRD_DFClaims(yrmo);
                        ShowResult();
                        auditRecon(yrmo);
                    }
                }
                else
                {
                    lbl_error.Text = "Reconciliation for " + _monthList + " not completed!";                   
                }
            }         
            catch (Exception ex)
            {
                cObj.DeleteNCAReconData(yrmo);
                lbl_error.Text = "Error in reconciling<br />" + ex.Message;
            }
        }
    }
    protected void btn_reconAgn_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        ClaimsRecon cObj1 = new ClaimsRecon();
        try
        {
            cObj1.DeleteNCAReconData(yrmo);
            if (RptsImported())
            {               
                //AnthRecon.matchDFCl(yrmo);
                cObj1.rfdfFirstVerification(yrmo);
                cObj1.CompareFRD_DFClaims(yrmo);
                cObj1.retainForcedAdjNCA(yrmo);
                ShowResult();
                auditRecon(yrmo);
            }
        }
        catch (Exception ex)
        {
            cObj1.DeleteNCAReconData(yrmo);
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }    
       
    }
   
    protected void btn_genRpt_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds.Clear();
        string[][] cols = { new string[] { "YRMO", "FRD Amount", "DF Amount", "Carry Forward Amount", " Variance"} };
        string[][] colsFormat = { new string[] { "string", "decimal", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "DFRF_Recon" };
        string[] titles = { "Non CA Claims Reconciliation Report for YRMO - " + yrmo };
        ClaimsRecon eObj = new ClaimsRecon();
        try
        {
            ds = eObj.DF_DFRFClaimsRecon(yrmo);
            ExcelReport.ExcelXMLRpt(ds, "NonCA_Recon_" + yrmo, sheetnames, titles, cols,colsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }
    
    void ShowResult()
    {
        resultDiv.Visible = false;
        DFAgingDiv.Visible = false;
        DFRFAgingDiv.Visible = false;
        DetailsDiv.Visible = false;
        img_CFDFnoRF.ImageUrl = "~/styles/images/collapsed1.gif";
        img_CFAudit.ImageUrl = "~/styles/images/collapsed1.gif";
        img_CFDFRF.ImageUrl = "~/styles/images/collapsed1.gif";
        bindResult();
        MultiView1.SetActiveView(view_result);
    }

    protected void bindResult()
    {
        DataSet ds = new DataSet();
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        ClaimsRecon eObj1 = new ClaimsRecon();
        ds = eObj1.DF_DFRFClaimsRecon(yrmo);
        if (ds.Tables[0].Rows.Count > 0)
        {
            resultDiv.Visible = true;
            grdvResult.DataSource = ds;
            grdvResult.DataBind();
        }
        
    }
    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();

        if(!AnthImport.PastImport("CLMRPT",yrmo))
        {
            lbl_error.Text = "Anthem Claims DF Report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!AnthImport.PastImport("RFDF", yrmo))
        {
            lbl_error.Text = "DF/RF Report for yrmo - " + yrmo + " is not imported";
            return false;
        }
        if (!yrmo.Equals("200801"))
        {
            if (!AnthImport.PastImport("DF", yrmo))
            {
                lbl_error.Text = "DFnoRF Report for yrmo - " + yrmo + " is not imported";
                return false;
            }
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
            SortGridView(sortExpression, DESCENDING, "recon");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "recon");
        }  
    }
    private void SortGridView(string sortExpression, string direction, string source)
    {
        ClaimsRecon repObj = new ClaimsRecon();
        DataTable dt;
        DataView dv;
        string yrmo = ddlYrmo.SelectedItem.Text;
        switch (source)
        {
            case "recon":
                dt = repObj.DF_DFRFClaimsRecon(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvResult.DataSource = dv;
                grdvResult.DataBind();
                break;

            case "DFAging":
                //dt = ReconDAL.getDFAgingRpt(yrmo).Tables[0];
                dt = repObj.DFnoRFAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDFAging.DataSource = dv;
                grdvDFAging.DataBind();
                break;

            case "DFRFAging":
                //dt = ReconDAL.getDFAgingRpt(yrmo).Tables[0];
                dt = repObj.DFRFAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDFRFAging.DataSource = dv;
                grdvDFRFAging.DataBind();
                break;

            case "DFRFmismatchAging":
                //dt = ReconDAL.getDFAgingRpt(yrmo).Tables[0];
                dt = repObj.getMismatchCF(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDFRFmismatchAging.DataSource = dv;
                grdvDFRFmismatchAging.DataBind();
                break;

            case "recon_matched":
                dt = ReconDAL.getMatchedDCNAmnt(yrmo, "Matched").Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvMatched.DataSource = dv;
                grdvMatched.DataBind();
                break;

            case "recon_mismatched":
                dt = ReconDAL.getMatchedDCNAmnt(yrmo, "UnMatched").Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvMatched.DataSource = dv;
                grdvMatched.DataBind();
                break;

            case "recon_unmatchDFRF":
                dt = ReconDAL.getRFDFUM(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvUnmatchedDFRF.DataSource = dv;
                grdvUnmatchedDFRF.DataBind();
                break;

            case "recon_unmatchAnth":
                dt = ReconDAL.getClaimsUM(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvUnmatchedAnth.DataSource = dv;
                grdvUnmatchedAnth.DataBind();
                break;
        } 
    }

    protected void btn_xlDFAging_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);
        ClaimsRecon repObj1 = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        DataSet dsSummary = new DataSet();
        dsSummary.Clear();

        string filename = "DF_no_RF_Aging_" + yrmo;
        string[][] cols ={ new string[] { "YRMO", "SSN", "Claim #", "Name", "Service From Dt", "Service Thru Dt", "Paid date", "Claim Type", "Current YRMO (" + yrmo + ")", "Previous YRMO (" + _prevYrmo + ")", "Prior YRMO (" + _priorYrmo + " & less)" } };
        string[][] colsFormat ={ new string[] { "string", "string", "string", "string", "string", "string",  "string", "string", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "DF_no_RF_Aging" };
        string[] titles = { "Aging Report of Open DFs not on Anthem's DF no RF report for " + yrmo };
        string[] sumTitles = { "Summary Statistics for Aging Report", "Detail Aging Report"};
        string[][] sumColsFormat ={ new string[] {"string", "number", "number","decimal", "decimal" } };        

        try
        {
            ds = repObj1.DFnoRFAging(yrmo);
            dsSummary = repObj1.getSummary(yrmo, "DFnoRF");
            ExcelReport.ExcelXMLRpt(ds, filename, sheetnames, titles, cols, colsFormat,dsSummary,sumTitles, sumColsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }
    protected void btn_xlDFRFAging_Click(object sender, EventArgs e)
    { 
        string yrmo = ddlYrmo.SelectedItem.Text;
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);
        ClaimsRecon repObj2 = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        DataSet dsSummary = new DataSet();
        dsSummary.Clear();

        string filename = "DF_RF_Aging_" + yrmo;
        string[][] cols ={ new string[] { "YRMO", "Member ID", "DCN", "Last Update", "Current YRMO (" + yrmo + ")", "Previous YRMO (" + _prevYrmo + ")", "Prior YRMO (" + _priorYrmo + " & less)" } };
        string[][] colsFormat ={ new string[] { "string", "string", "string", "string", "decimal", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "DF_RF_Aging" };
        string[] titles = { "Aging Report of records cleared from DF no RF, not cleared in DF/RF for " + yrmo };
        string[] sumTitles = { "Summary Statistics for Aging Report", "Detail Aging Report" };
        string[][] sumColsFormat ={ new string[] { "string", "number", "number", "decimal", "decimal" } }; 

        try
        {
            ds = repObj2.DFRFAging(yrmo);
            dsSummary = repObj2.getSummary(yrmo, "DFRF");
            ExcelReport.ExcelXMLRpt(ds, filename, sheetnames, titles, cols, colsFormat, dsSummary, sumTitles, sumColsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }

    protected void btn_xlDFRFmismatchAging_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);
        ClaimsRecon repObj3 = new ClaimsRecon();        
        DataSet dsmismatchCF = new DataSet();
        dsmismatchCF.Clear();

        string filename = "DFRF_Amount_Mismatch_Aging_" + yrmo;
        string[][] cols ={ new string[] { "YRMO", "Claim ID", "Anthem DF Counter", "DFRF Counter", "Anthem DF Amount", "DFRF Amount", "Variance", "Current YRMO (" + yrmo + ")", "Previous YRMO (" + _prevYrmo + ")", "Prior YRMO (" + _priorYrmo + " & less)" } };
        string[][] colsFormat ={ new string[] { "string", "string","number","number", "decimal", "decimal", "decimal", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "Amount_mismatch_Aging" };
        string[] titles = { "Aging Report of Amount mismatch Records for " + yrmo };
        

        try
        {
            dsmismatchCF = repObj3.getMismatchCF(yrmo);
            ExcelReport.ExcelXMLRpt(dsmismatchCF, filename, sheetnames, titles, cols, colsFormat);
            
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }

    protected void btn_xldetails_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();        
        string filename = "DFRF_matched_mismatched_Details_" + yrmo;
        string[][] cols = { new string[] { "YRMO", "DCN", "Anthem Claims Amount", "DFRF Amount", "Variance" },
                            new string[] { "YRMO", "DCN", "Anthem Claims Amount", "DFRF Amount", "Variance"},
                            new string[] { "YRMO", "DCN", "Subscriber ID", "Paid Date", "DFRF Amount" }, 
                            new string[] { "YRMO", "DCN", "Subscriber ID", "Paid Date", "Anthem Claims Amount" },
                            new string[] { "Type", "Record Count", "Dups Count", "Total Count" } };
        string[][] colsFormat = {   new string[] { "string", "string", "decimal", "decimal", "decimal" }, 
                                    new string[] { "string", "string", "decimal", "decimal", "decimal" },
                                    new string[] { "string", "string", "string", "string", "decimal" }, 
                                    new string[] { "string", "string", "string", "string", "decimal" }, 
                                    new string[] { "string", "string", "string", "string"}};

        string[] sheetnames = { "Matched_Records","Mismatched_Records", "DFRF_with_no_matching_Anthem", "Anthem_with_no_matching_DFRF", "Records_Summary" };
        string[] titles = { "DFRF Reconciliation Matched Records by DCN and Amount for " + yrmo, 
                            "DFRF Reconciliation Mismatched Records by Amount for " + yrmo,
                            "DCN Records in DF/RF not in Anthem Claims for " + yrmo, 
                            "DCN records in Anthem Claims not in DF/RF for " + yrmo,
                            "Summary of records from Anthem DF Report & DFRF Report for " + yrmo};

        try
        {
            ds = ReconDAL.getMatchedDCNAmnt(yrmo, "Matched");
            ds.Tables[0].TableName = "matchedTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "matchedTableF";
            ds.Clear();
            ds = ReconDAL.getMatchedDCNAmnt(yrmo, "UnMatched");
            ds.Tables[0].TableName = "mismatchedTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "mismatchedTableF";
            ds.Clear();
            ds = ReconDAL.getRFDFUM(yrmo);
            ds.Tables[0].TableName = "unmatch_DFRFTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "unmatch_DFRFTableF";
            ds.Clear();
            ds = ReconDAL.getClaimsUM(yrmo);
            ds.Tables[0].TableName = "unmatch_AnthTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[3].TableName = "unmatch_AnthTableF";
            ds.Clear();
            ds = RecordCount.SummaryRecordsFinal(yrmo);
            ds.Tables[0].TableName = "summRecords";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[4].TableName = "summRecordsF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, cols,colsFormat);
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            lbl_error.Text = "Error in generating excel report" + ex.Message;
        }
    }
    protected void bindDFAging()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ClaimsRecon bobj = new ClaimsRecon();
        ds = bobj.DFnoRFAging(yrmo);

        if (ds.Tables[0].Rows.Count > 0)
        {           
            grdvDFAging.DataSource = ds;
            grdvDFAging.DataBind();
        }        
    }
    protected void bindDFRFAging()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ClaimsRecon bobj1 = new ClaimsRecon();
        ds = bobj1.DFRFAging(yrmo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvDFRFAging.DataSource = ds;
            grdvDFRFAging.DataBind();
        }
    }
    protected void bindDFRFmismatchAging()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ClaimsRecon bobj1 = new ClaimsRecon();
        ds = bobj1.getMismatchCF(yrmo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvDFRFmismatchAging.DataSource = ds;
            grdvDFRFmismatchAging.DataBind();
        }
    }
    protected void bindgrdvMatched()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds = ReconDAL.getMatchedDCNAmnt(yrmo, "Matched");

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvMatched.DataSource = ds;
            grdvMatched.DataBind();
        }
    }
    protected void bindgrdvmisMatched()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds = ReconDAL.getMatchedDCNAmnt(yrmo, "UnMatched");

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvmisMatched.DataSource = ds;
            grdvmisMatched.DataBind();
        }
    }
    protected void bindgrdvUnmatchedAnth()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds = ReconDAL.getClaimsUM(yrmo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvUnmatchedAnth.DataSource = ds;
            grdvUnmatchedAnth.DataBind();
        }
    }
    protected void bindgrdvUnmatchedDFRF()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        DataSet ds = new DataSet();
        ds = ReconDAL.getRFDFUM(yrmo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvUnmatchedDFRF.DataSource = ds;
            grdvUnmatchedDFRF.DataBind();
        }
    }

    protected void lnkDFAging_OnClick(object sender, EventArgs e)
    {
        if (DFAgingDiv.Visible.Equals(true))
        {
            DFAgingDiv.Visible = false;
            img_CFDFnoRF.ImageUrl = "~/styles/images/collapsed1.gif";
        }
        else
        {
            img_CFDFnoRF.ImageUrl = "~/styles/images/expanded1.gif";
            DFAgingDiv.Visible = true;
        }
        bindDFAging();
    }

    protected void lnkDFRFAging_OnClick(object sender, EventArgs e)
    {
        if (DFRFAgingDiv.Visible.Equals(true))
        {
            DFRFAgingDiv.Visible = false;
            img_CFDFRF.ImageUrl = "~/styles/images/collapsed1.gif";
        }
        else
        {
            img_CFDFRF.ImageUrl = "~/styles/images/expanded1.gif";
            DFRFAgingDiv.Visible = true;
        }
        bindDFRFAging();
    }

    protected void lnkDFRFmismatchAging_OnClick(object sender, EventArgs e)
    {
        if (DFRFmismatchAgingDiv.Visible.Equals(true))
        {
            DFRFmismatchAgingDiv.Visible = false;
            img_CFDFRFmismatch.ImageUrl = "~/styles/images/collapsed1.gif";
        }
        else
        {
            img_CFDFRFmismatch.ImageUrl = "~/styles/images/expanded1.gif";
            DFRFmismatchAgingDiv.Visible = true;
        }
        bindDFRFmismatchAging();
    }

    protected void lnkdetails_OnClick(object sender, EventArgs e)
    {
        if (DetailsDiv.Visible.Equals(true))
        {
            DetailsDiv.Visible = false;
            img_CFAudit.ImageUrl = "~/styles/images/collapsed1.gif";
        }
        else
        {
            img_CFAudit.ImageUrl = "~/styles/images/expanded1.gif";
            DetailsDiv.Visible = true;
        }
        bindgrdvMatched();
        bindgrdvmisMatched();
        bindgrdvUnmatchedAnth();
        bindgrdvUnmatchedDFRF();
    }

    protected void grdvDFAging_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "DFAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "DFAging");
        }
    }
    protected void grdvDFAging_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDFAging.PageIndex = e.NewPageIndex;
        bindDFAging();
    }

    protected void grdvDFRFAging_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "DFRFAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "DFRFAging");
        }
    }
    protected void grdvDFRFAging_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDFRFAging.PageIndex = e.NewPageIndex;
        bindDFRFAging();
    }

    protected void grdvDFRFmismatchAging_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "DFRFmismatchAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "DFRFmismatchAging");
        }
    }
    protected void grdvDFRFmismatchAging_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDFRFmismatchAging.PageIndex = e.NewPageIndex;
        bindDFRFmismatchAging();
    }

    protected void grdvMatched_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvMatched.PageIndex = e.NewPageIndex;
        bindgrdvMatched();
    }
    protected void grdvmisMatched_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvmisMatched.PageIndex = e.NewPageIndex;
        bindgrdvmisMatched();
    }
    protected void grdvUnmatchedAnth_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvUnmatchedAnth.PageIndex = e.NewPageIndex;
        bindgrdvUnmatchedAnth();
    }
    protected void grdvUnmatchedDFRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvUnmatchedDFRF.PageIndex = e.NewPageIndex;
        bindgrdvUnmatchedDFRF();
    }
    protected void grdvMatched_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "recon_matched");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "recon_matched");
        }
    }
    protected void grdvmisMatched_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "recon_mismatched");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "recon_mismatched");
        }
    }
    protected void grdvUnmatchedAnth_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "recon_unmatchAnth");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "recon_unmatchAnth");
        }
    }
    protected void grdvUnmatchedDFRF_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "recon_unmatchDFRF");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "recon_unmatchDFRF");
        }
    }

    protected void auditRecon(string yrmo)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Claims Reconciliation", "Non-CA DFRF Reconciliation", yrmo);
    }

}

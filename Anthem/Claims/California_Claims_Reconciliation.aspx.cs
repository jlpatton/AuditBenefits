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


public partial class Anthem_Claims_California_Claims_Reconciliation : System.Web.UI.Page
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
        resultDiv.Visible = false;
        lbl_error.Text = "";

        try
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
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    protected void btn_ADDYrmo(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
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
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        try
        {
            ddlYrmo.SelectedIndex = 0;
            ddlYrmo.Visible = true;
            txtPrevYRMO.Visible = false;
            btnAddYrmo.Visible = false;
            btnCancelYrmo.Visible = false;
            checkPastRecon();
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    protected void checkPastRecon()
    {
        CAClaimsDAL cobj = new CAClaimsDAL();
        string _yrmo = ddlYrmo.SelectedItem.Text.ToString();       

        if (cobj.Reconciled(_yrmo))
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
        CAClaimsDAL cobj = new CAClaimsDAL();
        string yrmo = ddlYrmo.SelectedItem.Text;

        if (Page.IsValid)
        {
            lbl_error.Text = "";
            try
            {
                string _monthList = AnthRecon.CheckReconOrder(yrmo, "CA");
                if (_monthList.Equals(""))
                {
                    if (RptsImported())
                    {
                        AnthRecon.CAClaimsRecon(yrmo);
                        ViewResult();
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
                MultiView1.SetActiveView(view_main);
                cobj.DeleteReconData(yrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }
        }
    }
    protected void btn_reconAgn_Click(object sender, EventArgs e)
    {
        CAClaimsDAL cobj = new CAClaimsDAL();
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = "";

        try
        {
            cobj.DeleteReconData(yrmo);
            if (RptsImported())
            {
                AnthRecon.CAClaimsRecon(yrmo);
                cobj.retainForcedAdj(yrmo);                
                ViewResult();
                auditRecon(yrmo);
            }
        }
        catch (Exception ex)
        {
            MultiView1.SetActiveView(view_main);
            cobj.DeleteReconData(yrmo);
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }
    }
    void ViewResult()
    {
        CAClaimsDAL cobj = new CAClaimsDAL();
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        Boolean balanced;

        balanced = cobj.Balanced(yrmo);       
        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = false;        
        CFDiv.Visible = false;
        img_CF.ImageUrl = "~/styles/images/collapsed1.gif";
        CFRptDiv.Visible = false;
        img_dtl.ImageUrl = "~/styles/images/collapsed1.gif";
        dtlDiv.Visible = false;
        dtlRptDiv.Visible = false;       
        
        resultDiv.Visible = true;            
        CFDiv.Visible = true;        
        dtlDiv.Visible = true;       
    }
    protected void lnkCF_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (CFRptDiv.Visible == true)
            {
                img_CF.ImageUrl = "~/styles/images/collapsed1.gif";
                CFRptDiv.Visible = false;
            }
            else
            {
                img_CF.ImageUrl = "~/styles/images/expanded1.gif";
                CFRptDiv.Visible = true;
                bindResult("CF");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    protected void lnk_genCFRpt_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        CAClaimsDAL caobj = new CAClaimsDAL();
        CAExcelRpt xlobj = new CAExcelRpt();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        DataSet ds = new DataSet();
        ds.Clear();
        string filename = "CA_Claims_CFAging_" + yrmo;
        string[] sheetnames = { "mismatched_amounts_CF", "unmatched_Anthem_CF", "unmatched_BOA_CF", "duplicate_checks_CF" };
        string[] titles = { "Mismatched Amount Carry Forwards with Aging for YRMO : " + yrmo, "Un-Matched Carry Forwards from Anthem with Aging for YRMO : " + yrmo, "Un-Matched Carry Forwards from BOA with Aging for YRMO : " + yrmo, "Duplicate Check Carry Forwards with Aging for YRMO : " + yrmo };
        string[][] colsFormat = { new string[] { "string", "string", "string", "checknum", "string", "string", "string", "string", "string", "decimal", "string", "string", "decimal", "decimal", "decimal", "decimal", "decimal" }, new string[] { "string", "string", "string", "checknum", "string", "string", "string", "string", "string", "decimal", "decimal", "decimal" }, new string[] { "string", "checknum", "string", "string", "decimal", "decimal", "decimal" }, new string[] { "string", "string", "string", "string", "checknum", "string", "string", "string", "string", "string", "decimal", "decimal", "decimal" } };

        try
        {
            ds = caobj.GetAmtMismatchAging(yrmo);
            ds.Tables[0].TableName = "matchTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "matchTableF";
            ds.Clear();
            ds = caobj.GetAnthMismatchAging(yrmo);
            ds.Tables[0].TableName = "AnthUnmatchTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "AnthUnmatchTableF";
            ds.Clear();
            ds = caobj.GetBOAMismatchAging(yrmo);
            ds.Tables[0].TableName = "BOAUnmatchTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "BOAUnmatchTableF";
            ds.Clear();
            ds = caobj.GetDupsAging(yrmo);
            ds.Tables[0].TableName = "DupTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "DupTableF";
            ds.Clear();
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Carry Forward report with aging for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }
    protected void lnkDtl_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (dtlRptDiv.Visible == true)
            {
                img_dtl.ImageUrl = "~/styles/images/collapsed1.gif";
                dtlRptDiv.Visible = false;
            }
            else
            {
                img_dtl.ImageUrl = "~/styles/images/expanded1.gif";
                dtlRptDiv.Visible = true;
                bindResult("detail");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    protected void lnk_genDtlRpt_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        CAExcelRpt xlobj = new CAExcelRpt();
        CAClaimsDAL caobj = new CAClaimsDAL();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        DataSet ds = new DataSet();
        ds.Clear();
        string filename = "CA_Claims_Recon_detail_" + yrmo;
        string[] sheetnames = { "matched_Check&Amt", "mismatched_Amounts", "unmatchedCheck_Anthem", "unmatchedCheck_BOA", "duplicate_checks" };
        string[] titles = { "California Claims Reconciliation Matched Records by Check# and Amount for " + yrmo, 
                            "California Claims Reconciliation Mismatched Amount Records for " + yrmo, 
                            "California Claims Reconciliation Un-Matched Check Records from Anthem for " + yrmo, 
                            "California Claims Reconciliation Un-Matched Check Records from BOA for " + yrmo, 
                            "California Claims Reconciliation Duplicate Check Record Details for " + yrmo };
        string[][] colsFormat = { new string[] { "string", "string", "checknum", "string", "string", "string", "string", "string", "decimal", "string", "string", "decimal", "decimal" }, 
                                        new string[] { "string", "string", "checknum", "string", "string", "string", "string", "string", "decimal", "string", "string", "decimal", "decimal" },
                                        new string[] { "checknum", "string", "string", "string", "string", "string", "string", "string", "decimal" }, 
                                        new string[] { "checknum", "string", "string", "decimal" }, 
                                        new string[] { "string", "checknum", "string", "string", "string", "string", "string", "string", "string", "string", "decimal", "decimal", "string" } };

        try
        {
            ds = caobj.GetMatchedChecknAmtRecords(yrmo);
            ds.Tables[0].TableName = "matchedTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "matchedTableF";
            ds.Clear();
            ds = caobj.GetAmtMismatchCheckRecords(yrmo);
            ds.Tables[0].TableName = "mismatchAmtTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "mismatchAmtTableF";
            ds.Clear();
            ds = caobj.GetAnthMismatch(yrmo);
            ds.Tables[0].TableName = "AnthUnmatchedTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "AnthUnmatchedTableF";
            ds.Clear();
            ds = caobj.GetBOAMismatch(yrmo);
            ds.Tables[0].TableName = "BOAUnmatchedTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[3].TableName = "BOAUnmatchedTableF";
            ds.Clear();
            ds = caobj.GetDupChecksDetails(yrmo);
            ds.Tables[0].TableName = "DupDtlTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[4].TableName = "DupDtlTableF";
            ds.Clear();
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating CA Claims Report of matched and mismatched records for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }
    protected void bindResult(string _src)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        string yrmo = ddlYrmo.SelectedItem.Text;
        CAClaimsDAL caobj = new CAClaimsDAL();
        
        switch (_src)
        {
            case "CF":
                mismatchCF.Visible = false;
                unmatchAnthCF.Visible = false;
                unmatchBOACF.Visible = false;
                DupCF.Visible = false;
                ds = caobj.GetAmtMismatchAging(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    mismatchCF.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    mismatchCF.Attributes.Add("style", "width:700px;height:250px");
                }
                mismatchCF.Visible = true;
                grdvCF_match.DataSource = ds;
                grdvCF_match.DataBind();                    
                
                ds.Clear();
                ds = caobj.GetAnthMismatchAging(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    unmatchAnthCF.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    unmatchAnthCF.Attributes.Add("style", "width:700px;height:250px");
                }
                unmatchAnthCF.Visible = true;
                grdvCF_unmatchAnth.DataSource = ds;
                grdvCF_unmatchAnth.DataBind();
               
                ds.Clear();
                ds = caobj.GetBOAMismatchAging(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    unmatchBOACF.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    unmatchBOACF.Attributes.Add("style", "width:700px;height:250px");
                }
                unmatchBOACF.Visible = true;
                grdvCF_unmatchBOA.DataSource = ds;
                grdvCF_unmatchBOA.DataBind();                
                ds.Clear();
                ds = caobj.GetDupsAging(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DupCF.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    DupCF.Attributes.Add("style", "width:700px;height:250px");
                }
                DupCF.Visible = true;
                grdvCF_Dup.DataSource = ds;
                grdvCF_Dup.DataBind();                
                ds.Clear();
                break;
            case "detail":
                matchDtl.Visible = false;
                mismatchAmt.Visible = false;
                unmatchAnthDtl.Visible = false;
                unmatchBOADtl.Visible = false;                
                ds = caobj.GetMatchedChecknAmtRecords(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    matchDtl.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    matchDtl.Attributes.Add("style", "width:700px;height:250px");
                }
                matchDtl.Visible = true;
                grdv_dtlmat.DataSource = ds;
                grdv_dtlmat.DataBind();

                ds.Clear();
                ds = caobj.GetAmtMismatchCheckRecords(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    mismatchAmt.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    mismatchAmt.Attributes.Add("style", "width:700px;height:250px");
                }
                mismatchAmt.Visible = true;
                grdv_dtlmismat.DataSource = ds;
                grdv_dtlmismat.DataBind();

                ds.Clear();
                ds = caobj.GetAnthMismatch(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    unmatchAnthDtl.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    unmatchAnthDtl.Attributes.Add("style", "width:700px;height:250px");
                }
                unmatchAnthDtl.Visible = true;
                grdv_dtlunmatAnth.DataSource = ds;
                grdv_dtlunmatAnth.DataBind();
                
                ds.Clear();
                ds = caobj.GetBOAMismatch(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    unmatchBOADtl.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    unmatchBOADtl.Attributes.Add("style", "width:700px;height:250px");
                }
                unmatchBOADtl.Visible = true;
                grdv_dtlunmatBOA.DataSource = ds;
                grdv_dtlunmatBOA.DataBind();
                
                ds.Clear();
                ds = caobj.GetDupChecksDetails(yrmo);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    DupDtl.Attributes.Add("style", "width:700px;height:50px");
                }
                else
                {
                    DupDtl.Attributes.Add("style", "width:700px;height:250px");
                }
                DupDtl.Visible = true;
                grdv_dtlDup.DataSource = ds;
                grdv_dtlDup.DataBind();
                break;            
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
    private void SortGridView(string sortExpression, string direction, string source)
    {
        DataTable dt;
        DataView dv;
        string yrmo = ddlYrmo.SelectedItem.Text;
        CAClaimsDAL caobj = new CAClaimsDAL();

        switch (source)
        {
            case "CF_match":
                dt = caobj.GetAmtMismatchAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvCF_match.DataSource = dv;
                grdvCF_match.DataBind();
                break;
            case "CF_unmatch_Anth":
                dt = caobj.GetAnthMismatchAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvCF_unmatchAnth.DataSource = dv;
                grdvCF_unmatchAnth.DataBind();
                break;
            case "CF_unmatch_BOA":
                dt = caobj.GetBOAMismatchAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvCF_unmatchBOA.DataSource = dv;
                grdvCF_unmatchBOA.DataBind();
                break;
            case "CF_Dup":
                dt = caobj.GetDupsAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvCF_Dup.DataSource = dv;
                grdvCF_Dup.DataBind();
                break;
            case "DtlMatched":
                dt = caobj.GetMatchedChecknAmtRecords(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdv_dtlmat.DataSource = dv;
                grdv_dtlmat.DataBind();
                break;
            case "DtlMismatch":
                dt = caobj.GetAmtMismatchCheckRecords(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdv_dtlmismat.DataSource = dv;
                grdv_dtlmismat.DataBind();
                break;
            case "DtlUnMatchedAnth":
                dt = caobj.GetAnthMismatch(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdv_dtlunmatAnth.DataSource = dv;
                grdv_dtlunmatAnth.DataBind();
                break;
            case "DtlUnMatchedBOA":
                dt = caobj.GetBOAMismatch(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdv_dtlunmatBOA.DataSource = dv;
                grdv_dtlunmatBOA.DataBind();
                break;
            case "DtlDup":
                dt = caobj.GetDupChecksDetails(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdv_dtlDup.DataSource = dv;
                grdv_dtlDup.DataBind();
                break;
        }
    }
    protected void grdvCF_match_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "CF_match"); 
    }   
    protected void grdvCF_unmatchAnth_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "CF_unmatch_Anth");
    }
    protected void grdvCF_unmatchBOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "CF_unmatch_BOA");
    }
    protected void grdvCF_Dup_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "CF_Dup");
    }
    protected void grdv_dtlmat_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "DtlMatched");  
    }
    protected void grdv_dtlmismat_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "DtlMismatch");
    }
    protected void grdv_dtlunmatAnth_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "DtlUnMatchedAnth");       
    }
    protected void grdv_dtlunmatBOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "DtlUnMatchedBOA");
    }
    protected void grdv_dtlDup_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "DtlDup");
    }
    void gridviewSort(string sortExpression, string source)
    {
        try
        {
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, DESCENDING, source);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, ASCENDING, source);
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }
    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        string[] source = { "CLMRPT", "BOA" };
        string[] rpts = { "Anthem Claims DF Report", "BOA Statement" };
        string rptsNotImported = ""; 

        for (int i = 0; i < source.Length; i++)
        {
            if (rptsNotImported.Equals("") && source[i].Equals("BOA") && yrmo.Equals("200801"))
                return true;
            if (!AnthImport.PastImport(source[i], yrmo))
            {
                rptsNotImported += rpts[i];
                if (i != (source.Length - 1))
                    rptsNotImported += "and ";
            }
        }

        if (rptsNotImported != "")
        {
            lbl_error.Text = rptsNotImported + " for yrmo - " + yrmo + " not imported";
            return false;
        }
        return true;
    }

    protected void auditRecon(string yrmo)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Claims Reconciliation", "CA Claims Reconciliation", yrmo);
    }
}

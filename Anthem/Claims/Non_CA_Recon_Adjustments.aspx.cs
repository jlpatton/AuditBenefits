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

public partial class Anthem_Claims_Non_CA_Recon_Adjustments : System.Web.UI.Page
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
            bindDFAging();
            bindDFRFAging();
            bindDFRFmismatchAging();
            ClaimsRecon yObj = new ClaimsRecon();
            string _yrmo = yObj.latestReconYrmo();
            if (!_yrmo.Equals("-1"))
            {
                lblH1.Text = " for YRMO - " + _yrmo;
                lblH2.Text = " for YRMO - " + _yrmo;
                lnkAdjCF1.Text = "Generate DFnoRF Carry Forward Report with Aging for YRMO - " + _yrmo;
                lnkAdjCF2.Text = "Generate DFRF Carry Forward Report with Aging for YRMO - " + _yrmo;
                lnkAdjCF3.Text = "Generate DFRF Amount mismatch Carry Forward Report with Aging for YRMO - " + _yrmo;
                lnkAdjMatched.Text = "Generate Detail Report of matched and mismatched records for YRMO - " + _yrmo;
            }
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);        
    }
   
    protected void bindDFAging()
    {        
        DataSet ds = new DataSet();
        ClaimsRecon bobj = new ClaimsRecon();
        string yrmo = bobj.latestReconYrmo();
        if (!yrmo.Equals("-1"))
        {
            ds = bobj.DFnoRFAging(yrmo);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvDFnoRF.DataSource = ds;
            grdvDFnoRF.DataBind();
            //}
        }
    }
    protected void bindDFRFAging()
    {      
        DataSet ds = new DataSet();
        ClaimsRecon bobj1 = new ClaimsRecon();
        string yrmo = bobj1.latestReconYrmo();
        if (!yrmo.Equals("-1"))
        {
            ds = bobj1.DFRFAging(yrmo);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvDFRF.DataSource = ds;
            grdvDFRF.DataBind();
            //}
        }
    }
    protected void bindDFRFmismatchAging()
    {
        DataSet ds = new DataSet();
        ClaimsRecon bobj1 = new ClaimsRecon();
        string yrmo = bobj1.latestReconYrmo();
        if (!yrmo.Equals("-1"))
        {
            ds = bobj1.getMismatchCF(yrmo);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvmismatchDFRF.DataSource = ds;
            grdvmismatchDFRF.DataBind();
            //}
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

    protected void grdvDFnoRF_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void grdvDFnoRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDFnoRF.PageIndex = e.NewPageIndex;
        bindDFAging();
    }

    protected void grdvDFRF_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void grdvDFRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDFRF.PageIndex = e.NewPageIndex;
        bindDFRFAging();
    }

    protected void grdvmismatchDFRF_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void grdvmismatchDFRF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvmismatchDFRF.PageIndex = e.NewPageIndex;
        bindDFRFmismatchAging();
    }


    private void SortGridView(string sortExpression, string direction, string source)
    {
        ClaimsRecon repObj = new ClaimsRecon();
        DataTable dt;
        DataView dv;
        string yrmo = repObj.latestReconYrmo();
        switch (source)
        {            

            case "DFAging":                
                dt = repObj.DFnoRFAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDFnoRF.DataSource = dv;
                grdvDFnoRF.DataBind();
                break;

            case "DFRFAging":               
                dt = repObj.DFRFAging(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDFRF.DataSource = dv;
                grdvDFRF.DataBind();
                break;

            case "DFRFmismatchAging":
                //dt = ReconDAL.getDFAgingRpt(yrmo).Tables[0];
                dt = repObj.getMismatchCF(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvmismatchDFRF.DataSource = dv;
                grdvmismatchDFRF.DataBind();
                break;
        }
    }

    protected void grdvDFnoRF_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvDFnoRF.Rows[index];
            string _yrmo = row.Cells[1].Text;
            string _ssn = row.Cells[2].Text;
            string _dcn = row.Cells[3].Text;
            string _paiddt = row.Cells[4].Text;
            bindDetailDFnoRF(_yrmo,_dcn,_ssn,_paiddt);
            MultiView2.SetActiveView(viewDFnoRFDetails);            
        }
    }

    protected void grdvDFRF_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvDFRF.Rows[index];
            string _yrmo = row.Cells[1].Text;
            string _ssn = row.Cells[2].Text;
            string _dcn = row.Cells[3].Text;
            string _updatedt = row.Cells[4].Text;
            bindDetailDFRF(_yrmo, _dcn, _ssn, _updatedt);
            MultiView3.SetActiveView(viewDFRFDetails);
        }
    }

    protected void grdvmismatchDFRF_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvmismatchDFRF.Rows[index];
            string _yrmo = row.Cells[1].Text;            
            string _dcn = row.Cells[2].Text;
            
            bindDetailDFRFMM(_yrmo, _dcn);
            MultiView4.SetActiveView(viewDFRFDetailsMM);
        }
    }

    protected void lnkDFnoRF_OnClick(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(viewDFnoRF);
        if (cbxReconcile1.Checked)
        {
            cbxReconcile1.Checked = false;
        }
        txtDesc1.Text = "";
        lblerr1.Text = "";
    }

    protected void lnkDFRFMM_OnClick(object sender, EventArgs e)
    {
        MultiView4.SetActiveView(viewDFRFMM);
        if (cbxReconcile3.Checked)
        {
            cbxReconcile3.Checked = false;
        }
        txtDesc3.Text = "";
        lblerr3.Text = "";
    }

    protected void lnkDFRF_OnClick(object sender, EventArgs e)
    {
        MultiView3.SetActiveView(viewDFRF);
        if (cbxReconcile2.Checked)
        {
            cbxReconcile2.Checked = false;
        }
        txtDesc2.Text = "";
        lblerr2.Text = "";
    }    

    protected void bindDetailDFnoRF(string prymo, string _dcn, string _ssn, string _pDate)
    {
        bindDfnoRfAdj(prymo, _dcn, _ssn, _pDate);
    }

    protected void bindDfnoRfAdj(string prymo,string _dcn,string _ssn,string _pDate)
    {
        lblDupDetDFnoRF.Visible = false;
        grdvDupDetails.Visible = false;
        lblNote1.Visible = false;

        ClaimsRecon detObj = new ClaimsRecon();
        string _yrmo = detObj.latestReconYrmo();
        DataSet ds1 = new DataSet();
        ds1.Clear();
        ds1 = detObj.DFnoRFAging(_yrmo, _dcn);
        if (ds1.Tables[0].Rows.Count > 0)
        {
            lblDupDetDFnoRF.Visible = true;
            grdvDupDetails.Visible = true;
            lblNote1.Visible = true;
            grdvDupDetails.DataSource = ds1;
            grdvDupDetails.DataBind();
        }
        else
        {
            lblDupDetDFnoRF.Visible = false;
            grdvDupDetails.Visible = false;
            lblNote1.Visible = false;
        }

        DataSet ds2 = new DataSet(); ds2 = detObj.DFnoRFAging(prymo, _ssn, _dcn, _pDate);
        if (ds2.Tables[0].Rows.Count == 1)
        {
            dtvDFnoRF.DataSource = ds2;
            dtvDFnoRF.DataBind();
        }
    }

    protected void grdvDupDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvDupDetails.Rows[index];
            string _yrmo = row.Cells[1].Text;
            string _ssn = row.Cells[2].Text;
            string _dcn = row.Cells[3].Text;
            string _paiddt = row.Cells[4].Text;
            bindDetailDFnoRF(_yrmo, _dcn, _ssn, _paiddt);
            
        }
    }

    protected void bindDetailDFRF(string prymo, string _dcn, string _ssn, string _uDate)
    {
        bindDfRfAdj(prymo, _dcn, _ssn, _uDate);
    }

    protected void bindDfRfAdj(string prymo, string _dcn, string _ssn, string _uDate)
    {
        lblDupDetDFRF.Visible = false;
        grdvDupDetailsDFRF.Visible = false;
        lblNote2.Visible = false;

        ClaimsRecon detObj1 = new ClaimsRecon();
        string _yrmo = detObj1.latestReconYrmo();
        DataSet ds1DFRF = new DataSet();
        ds1DFRF.Clear();
        ds1DFRF = detObj1.DFRFAging(_yrmo, _dcn);
        if (ds1DFRF.Tables[0].Rows.Count > 0)
        {
            lblDupDetDFRF.Visible = true;
            grdvDupDetailsDFRF.Visible = true;
            lblNote2.Visible = true;
            grdvDupDetailsDFRF.DataSource = ds1DFRF;
            grdvDupDetailsDFRF.DataBind();
        }
        else
        {
            lblDupDetDFRF.Visible = false;
            grdvDupDetailsDFRF.Visible = false;
            lblNote2.Visible = false;
        }

        DataSet ds2DFRF = new DataSet(); ds2DFRF = detObj1.DFRFAging(prymo, _ssn, _dcn, _uDate);
        if (ds2DFRF.Tables[0].Rows.Count == 1)
        {
            dtvDFRF.DataSource = ds2DFRF;
            dtvDFRF.DataBind();
        }
    }

    protected void grdvDupDetailsDFRF_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvDupDetails.Rows[index];
            string _yrmo = row.Cells[1].Text;
            string _ssn = row.Cells[2].Text;
            string _dcn = row.Cells[3].Text;
            string _updatedt = row.Cells[4].Text;
            bindDetailDFRF(_yrmo, _dcn, _ssn, _updatedt);

        }
    }

    protected void bindDetailDFRFMM(string prymo, string _dcn)
    {
        bindDfRfAdjMM(prymo, _dcn);
    }

    protected void bindDfRfAdjMM(string prymo, string _dcn)
    {
        lblDupDetmismatchDFRF.Visible = false;
        grdvDupDetailsmismatchDFRF.Visible = false;
        lblNote3.Visible = false;

        ClaimsRecon detObj2 = new ClaimsRecon();
        string _yrmo = detObj2.latestReconYrmo();
        DataSet ds2DFRF = new DataSet();
        ds2DFRF.Clear();
        ds2DFRF = detObj2.getMismatchCF(_yrmo, _dcn);
        if (ds2DFRF.Tables[0].Rows.Count > 1)
        {
            lblDupDetmismatchDFRF.Visible = true;
            grdvDupDetailsmismatchDFRF.Visible = true;
            lblNote3.Visible = true;
            grdvDupDetailsmismatchDFRF.DataSource = ds2DFRF;
            grdvDupDetailsmismatchDFRF.DataBind();
        }
        else
        {
            lblDupDetmismatchDFRF.Visible = false;
            grdvDupDetailsmismatchDFRF.Visible = false;
            lblNote3.Visible = false;
        }        
    }    

    protected void btnOK1_Click(object sender, EventArgs e)
    {
        string _Id;
        lblFErr.Text = "";
        
        try
        {
            string _notes = txtDesc1.Text;
            
            if (!cbxReconcile1.Checked)
            {
                lblerr1.Text = "* Required";
            }
            else
            {
                _Id = dtvDFnoRF.Rows[2].Cells[1].Text;
                ClaimsRecon adObject = new ClaimsRecon();
                adObject.updateForcedRecon(_Id, "DFnoRF", _notes, Session["userName"].ToString());
                MultiView2.SetActiveView(viewDFnoRF);
                bindDFAging();
            }
            
        }
        catch (Exception ex)
        {
            lblFErr.Text = ex.Message;
        }
        finally
        {
            if (cbxReconcile1.Checked)
            {
                cbxReconcile1.Checked = false;
            }
            txtDesc1.Text = "";
            lblerr1.Text = "";
        }
        
        
    }
    protected void btnClear1_Click(object sender, EventArgs e)
    {
        if (cbxReconcile1.Checked)
        {
            cbxReconcile1.Checked = false;
        }
        txtDesc1.Text = "";
        lblerr1.Text = "";
    }
    protected void btnOK2_Click(object sender, EventArgs e)
    {
        string _Id;
        lblFErr2.Text = "";
        
        try
        {
            string _notes = txtDesc2.Text;
            
            if (!cbxReconcile2.Checked)
            {
                lblerr2.Text = "* Required";
            }
            else
            {
                _Id = dtvDFRF.Rows[2].Cells[1].Text;
                ClaimsRecon adObject1 = new ClaimsRecon();
                adObject1.updateForcedRecon(_Id, "DFRF", _notes, Session["userName"].ToString());
                MultiView3.SetActiveView(viewDFRF);
                bindDFRFAging();
            }
            
        }
        catch (Exception ex)
        {
            lblFErr2.Text = ex.Message;
        }
        finally
        {
            if (cbxReconcile2.Checked)
            {
                cbxReconcile2.Checked = false;
            }
            txtDesc2.Text = "";
            lblerr2.Text = "";
        }
        
    }
    protected void btnClear2_Click(object sender, EventArgs e)
    {
        if (cbxReconcile2.Checked)
        {
            cbxReconcile2.Checked = false;
        }
        txtDesc2.Text = "";
        lblerr2.Text = "";
    }

    protected void btnOK3_Click(object sender, EventArgs e)
    {
        string _Id;
        lblFErr3.Text = "";

        try
        {
            string _notes = txtDesc3.Text;
          if (!cbxReconcile3.Checked)
            {
                lblerr3.Text = "* Required";
            }
            else
            {
                _Id = grdvDupDetailsmismatchDFRF.Rows[0].Cells[2].Text;
                ClaimsRecon adObject2 = new ClaimsRecon();
                adObject2.updateForcedRecon(_Id, "DFRFmismatch", _notes, Session["userName"].ToString());
                MultiView4.SetActiveView(viewDFRFMM);
                bindDFRFmismatchAging();
            }            
        }
        catch (Exception ex)
        {
            lblFErr3.Text = ex.Message;
        }
        finally
        {
            if (cbxReconcile3.Checked)
            {
                cbxReconcile3.Checked = false;
            }
            txtDesc3.Text = "";
            lblerr3.Text = "";
        }

    }
    protected void btnClear3_Click(object sender, EventArgs e)
    {
        if (cbxReconcile3.Checked)
        {
            cbxReconcile3.Checked = false;
        }
        txtDesc3.Text = "";
        lblerr3.Text = "";
    }

    protected void btn_Generate(object sender, EventArgs e)
    {
        string _yrmo = txtYRMO.Text;
        printReport(_yrmo);
    }

    protected void printReport(string _yrmo)
    {
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();

        ClaimsRecon repObj = new ClaimsRecon();

        string[][] cols = { new string[] { "YRMO", "Aging Report", "Claim ID", "Amount", "Source", "Notes", "User", "Date" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "decimal", "string", "string", "string", "string" } };
        string[] sheetnames = { "DFnoRF_CF_Adjustments", "DFRF_CF_Adjustments","Amount_Mismatch_CF_Adjustments" };
        string[] titles = { "DFnoRF - CarryForward Adjustments for YRMO - " + _yrmo, 
                            "DFRF - CarryForwards Adjustments for YRMO - " + _yrmo,
                            "DFRF Amount Mismatch - CarryForwards Adjustments for YRMO - " + _yrmo};

        try
        {
            ds = repObj.generateAuditAdjReport(_yrmo, "DFnoRF");
            ds.Tables[0].TableName = "dfnorfTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "dfnorfTableF";
            ds.Clear();
            ds = repObj.generateAuditAdjReport(_yrmo, "DFRF");
            ds.Tables[0].TableName = "dfrfTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "dfrfTableF";
            ds.Clear();
            ds = repObj.generateAuditAdjReport(_yrmo, "DFRFmismatch");
            ds.Tables[0].TableName = "dfrfMMTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[2].TableName = "dfrfMMTableF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, "NON_CA_CF_Recon_Adjustments_" + _yrmo, sheetnames, titles, cols, colsFormat);
        }
        finally
        {
        }
    }

    protected void lnkAdjCF1_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        ClaimsRecon repObj1 = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        DataSet dsSummary = new DataSet();
        dsSummary.Clear();

        string yrmo = repObj1.latestReconYrmo();
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);

        string filename = "DF_no_RF_Aging_" + yrmo;
        string[][] cols ={ new string[] { "YRMO", "SSN", "Claim #", "Name", "Service From Dt", "Service Thru Dt", "Paid date", "Claim Type", "Current YRMO (" + yrmo + ")", "Previous YRMO (" + _prevYrmo + ")", "Prior YRMO (" + _priorYrmo + " & less)" } };
        string[][] colsFormat ={ new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "DF_no_RF_Aging" };
        string[] titles = { "Aging Report of Open DFs not on Anthem's DF no RF report for " + yrmo };
        string[] sumTitles = { "Summary Statistics for Aging Report", "Detail Aging Report" };
        string[][] sumColsFormat ={ new string[] { "string", "number", "number", "decimal", "decimal" } };       
        
        
        try
        {
            ds = repObj1.DFnoRFAging(yrmo);
            dsSummary = repObj1.getSummary(yrmo, "DFnoRF");
            ExcelReport.ExcelXMLRpt(ds, filename, sheetnames, titles, cols, colsFormat, dsSummary, sumTitles, sumColsFormat);
        }
        catch (Exception ex)
        {           
            lblErrRep.Text = "Error in generating excel report" + ex.Message;
        }
    }

    protected void lnkAdjCF2_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        ClaimsRecon repObj2 = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        string yrmo = repObj2.latestReconYrmo();
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);

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
            lblErrRep.Text = "Error in generating excel report" + ex.Message;
        }
    }

    protected void lnkAdjCF3_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        ClaimsRecon repObj3 = new ClaimsRecon();
        DataSet ds = new DataSet();
        ds.Clear();
        string yrmo = repObj3.latestReconYrmo();
        string _prevYrmo = AnthRecon.prevYRMO(yrmo);
        string _priorYrmo = AnthRecon.prevYRMO(_prevYrmo);

        DataSet dsmismatchCF = new DataSet();
        dsmismatchCF.Clear();

        string filename = "DFRF_Amount_Mismatch_Aging_" + yrmo;
        string[][] cols ={ new string[] { "YRMO", "Claim ID", "Anthem DF Counter", "DFRF Counter", "Anthem DF Amount", "DFRF Amount", "Variance", "Current YRMO (" + yrmo + ")", "Previous YRMO (" + _prevYrmo + ")", "Prior YRMO (" + _priorYrmo + " & less)" } };
        string[][] colsFormat ={ new string[] { "string", "string", "number", "number", "decimal", "decimal", "decimal", "decimal", "decimal", "decimal" } };
        string[] sheetnames = { "Amount_mismatch_Aging" };
        string[] titles = { "Aging Report of Amount mismatch Records for " + yrmo };


        try
        {
            dsmismatchCF = repObj3.getMismatchCF(yrmo);
            ExcelReport.ExcelXMLRpt(dsmismatchCF, filename, sheetnames, titles, cols, colsFormat);

        }
        catch (Exception ex)
        {
            lblErrRep.Text = "Error in generating excel report" + ex.Message;
        }
    }


    protected void lnkAdjMatched_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        ClaimsRecon repObj3 = new ClaimsRecon();
        DataSet ds = new DataSet();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();

        string yrmo = repObj3.latestReconYrmo();

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

        string[] sheetnames = { "Matched_Records", "Mismatched_Records", "DFRF_with_no_matching_Anthem", "Anthem_with_no_matching_DFRF", "Records_Summary" };
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
            ExcelReport.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, cols, colsFormat);
        }
        catch (Exception ex)
        {
            lblErrRep.Text = "Error in generating excel report" + ex.Message;
        }
    }

   
}

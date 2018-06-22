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
using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;

public partial class Anthem_Claims_CA_Recon_Adjustments : System.Web.UI.Page
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
            bindDupsAging();
            bindUnmatchedAnthAging();
            bindUnmatchedBOAAging();
            bindAmntMisAging();
            CAClaimsDAL yObj = new CAClaimsDAL();
            string _yrmo = yObj.getLatestYRMOAdj();
            if (!_yrmo.Equals("-1"))
            {
                lblH1.Text = " for YRMO - " + _yrmo;
                lblH2.Text = " for YRMO - " + _yrmo;
                lblH3.Text = " for YRMO - " + _yrmo;
                lblH4.Text = " for YRMO - " + _yrmo;
                lnkAdjCF.Text = "Generate Carry Forward Report with Aging for YRMO - " + _yrmo;
                lnkAdjMatched.Text = "Generate Detail Report of matched and mismatched records for YRMO - " + _yrmo;
            }
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    #region DataBind

    protected void bindAmntMisAging()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        CAClaimsDAL misObj = new CAClaimsDAL();
        string yrmo = misObj.getLatestYRMOAdj();
        if (!yrmo.Equals("-1"))
        {
            ds = misObj.GetAmtMismatchAgingAdj(yrmo);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvAmntMis.DataSource = ds;
            grdvAmntMis.DataBind();
            //}
        }
    }
    protected void bindUnmatchedAnthAging()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        CAClaimsDAL unAnthObj = new CAClaimsDAL();
        string yrmo = unAnthObj.getLatestYRMOAdj();
        if (!yrmo.Equals("-1"))
        {
            ds = unAnthObj.GetAnthMismatchAgingAdj(yrmo);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvUnmatchedAnth.DataSource = ds;
            grdvUnmatchedAnth.DataBind();
            //}
        }
    }
    protected void bindUnmatchedBOAAging()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        CAClaimsDAL unBOAObj = new CAClaimsDAL();
        string yrmo = unBOAObj.getLatestYRMOAdj();
        if (!yrmo.Equals("-1"))
        {
            ds = unBOAObj.GetBOAMismatchAgingAdj(yrmo);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvUnmatchedBOA.DataSource = ds;
            grdvUnmatchedBOA.DataBind();
            //}
        }
    }
    protected void bindDupsAging()
    {
        DataSet ds = new DataSet();
        ds.Clear();
        CAClaimsDAL dupObj = new CAClaimsDAL();
        string yrmo = dupObj.getLatestYRMOAdj();
        if (!yrmo.Equals("-1"))
        {
            ds = dupObj.GetDistinctDupsAgingAdj(yrmo);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            grdvDups.DataSource = ds;
            grdvDups.DataBind();
            //}
        }
    }

    #endregion    

    #region Sorting

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

    protected void grdvAmntMis_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "AmntMisAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "AmntMisAging");
        }
    }

    protected void grdvUnmatchedAnth_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "UnmatchedAnthAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "UnmatchedAnthAging");
        }
    }

    protected void grdvUnmatchedBOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "UnmatchedBOAAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "UnmatchedBOAAging");
        }
    }

    protected void grdvDups_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING, "DupsAging");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING, "DupsAging");
        }
    }

    private void SortGridView(string sortExpression, string direction, string source)
    {       
        CAClaimsDAL sortObj = new CAClaimsDAL();
        DataTable dt;
        DataView dv;
        string yrmo = sortObj.getLatestYRMOAdj();
        switch (source)
        {

            case "AmntMisAging":
                dt = sortObj.GetAmtMismatchAgingAdj(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvAmntMis.DataSource = dv;
                grdvAmntMis.DataBind();
                break;

            case "UnmatchedAnthAging":
                dt = sortObj.GetAnthMismatchAgingAdj(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvUnmatchedAnth.DataSource = dv;
                grdvUnmatchedAnth.DataBind();
                break;
            case "UnmatchedBOAAging":
                dt = sortObj.GetBOAMismatchAgingAdj(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvUnmatchedBOA.DataSource = dv;
                grdvUnmatchedBOA.DataBind();
                break;
            case "DupsAging":
                dt = sortObj.GetDistinctDupsAgingAdj(yrmo).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDups.DataSource = dv;
                grdvDups.DataBind();
                break;
        }
    }

    #endregion    

    #region Paging

    protected void grdvAmntMis_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvAmntMis.PageIndex = e.NewPageIndex;
        bindAmntMisAging();
    }

    protected void grdvUnmatchedAnth_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvUnmatchedAnth.PageIndex = e.NewPageIndex;
        bindUnmatchedAnthAging();
    }

    protected void grdvUnmatchedBOA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvUnmatchedBOA.PageIndex = e.NewPageIndex;
        bindUnmatchedBOAAging();
    }

    protected void grdvDups_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDups.PageIndex = e.NewPageIndex;
        bindDupsAging();
    }

    #endregion

    #region Rowcommands

    protected void grdvAmntMis_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvAmntMis.Rows[index];
            string _yrmo = row.Cells[1].Text;
            int _chq = Convert.ToInt32(row.Cells[2].Text);
            bindDetailAmntMis(_yrmo, _chq);
            MultiView2.SetActiveView(viewAmntMisDetails);
        }
    }

    protected void grdvUnmatchedAnth_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvUnmatchedAnth.Rows[index];
            string _yrmo = row.Cells[1].Text;
            int _chq = Convert.ToInt32(row.Cells[2].Text);
            bindDetailAnth(_yrmo, _chq);
            MultiView3.SetActiveView(viewUnMatchedAnthDetails);
        }
    }

    protected void grdvUnmatchedBOA_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvUnmatchedBOA.Rows[index];
            string _yrmo = row.Cells[1].Text;
            int _chq = Convert.ToInt32(row.Cells[2].Text);
            bindDetailBOA(_yrmo, _chq);
            MultiView4.SetActiveView(viewUnmatchedBOADetails);
        }
    }

    protected void grdvDups_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = grdvDups.Rows[index];
            int _chq = Convert.ToInt32(row.Cells[2].Text);
            bindDetailDups(_chq);
            MultiView5.SetActiveView(viewDupsDetails);
        }
    }

    #endregion

    #region link back

    protected void lnkAmntMis_OnClick(object sender, EventArgs e)
    {
        MultiView2.SetActiveView(amountMismatch);
        if (cbxReconcile1.Checked)
        {
            cbxReconcile1.Checked = false;
        }
        txtDesc1.Text = "";
        lblerr1.Text = "";
    }

    protected void lnkUnmatchedAnth_OnClick(object sender, EventArgs e)
    {
        MultiView3.SetActiveView(unmatchedAnthem);
        if (cbxReconcile2.Checked)
        {
            cbxReconcile2.Checked = false;
        }
        txtDesc2.Text = "";
        lblerr2.Text = "";
    }

    protected void lnkUnmatchedBOA_OnClick(object sender, EventArgs e)
    {
        MultiView4.SetActiveView(unmatchedBOA);
        if (cbxReconcile3.Checked)
        {
            cbxReconcile3.Checked = false;
        }
        txtDesc3.Text = "";
        lblerr3.Text = "";
    }

    protected void lnkDups_OnClick(object sender, EventArgs e)
    {
        MultiView5.SetActiveView(dupAdj);
        if (cbxReconcile4.Checked)
        {
            cbxReconcile4.Checked = false;
        }
        txtDesc4.Text = "";
        lblerr4.Text = "";
    }

    #endregion    

    #region Detail DataBind
    protected void bindDetailAmntMis(string prymo, int _chq)
    {
        CAClaimsDAL detObj = new CAClaimsDAL();        
        DataSet ds1 = new DataSet();
        ds1.Clear();
        ds1 = detObj.GetAmtMismatchAging(prymo, _chq);        
        dtvAmntMis.DataSource = ds1;
        dtvAmntMis.DataBind();
        
    }

    protected void bindDetailAnth(string prymo, int _chq)
    {
        CAClaimsDAL detObj1 = new CAClaimsDAL();        
        DataSet ds1Anth = new DataSet();
        ds1Anth.Clear();
        ds1Anth = detObj1.GetAnthMismatchAging(prymo, _chq);       
        dtvAnth.DataSource = ds1Anth;
        dtvAnth.DataBind();        
    }

    protected void bindDetailBOA(string prymo, int _chq)
    {
        CAClaimsDAL detObj2 = new CAClaimsDAL();        
        DataSet ds1BOA = new DataSet();
        ds1BOA.Clear();
        ds1BOA = detObj2.GetBOAMismatchAging(prymo, _chq);      
        dtvUnmatchedBOA.DataSource = ds1BOA;
        dtvUnmatchedBOA.DataBind();
        
    }

    protected void bindDetailDups(int _chq)
    {
        lblDupDetDups.Visible = false;
        grdvDupsDupDetails.Visible = false;
        lblNote4.Visible = false;

        CAClaimsDAL detObj3 = new CAClaimsDAL();
        string _yrmo = detObj3.getLatestYRMOAdj();
        DataSet ds1Dups = new DataSet();
        ds1Dups.Clear();
        ds1Dups = detObj3.GetDupCheckDetails(_yrmo, _chq);
        if (ds1Dups.Tables[0].Rows.Count > 1)
        {
            lblDupDetDups.Visible = true;
            grdvDupsDupDetails.Visible = true;
            lblNote4.Visible = true;
            grdvDupsDupDetails.DataSource = ds1Dups;
            grdvDupsDupDetails.DataBind();
        }
        else
        {
            lblDupDetDups.Visible = false;
            grdvDupsDupDetails.Visible = false;
            lblNote4.Visible = false;
        }       
    }

    #endregion

    #region details RowCommand
    
    //protected void grdvDupUnmatchedAnthDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.Equals("Select"))
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        GridViewRow row = grdvDupUnmatchedAnthDetails.Rows[index];
    //        string _yrmo = row.Cells[1].Text;
    //        string _ssn = row.Cells[2].Text;
    //        string _dcn = row.Cells[3].Text;
    //        string _updatedt = row.Cells[4].Text;
    //        bindDetailAnth(_yrmo, _dcn, _ssn, _updatedt);

    //    }
    //}

    //protected void grdvUnmatchedBOADupDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.Equals("Select"))
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        GridViewRow row = grdvUnmatchedBOADupDetails.Rows[index];
    //        string _yrmo = row.Cells[1].Text;
    //        string _ssn = row.Cells[2].Text;
    //        string _dcn = row.Cells[3].Text;
    //        string _updatedt = row.Cells[4].Text;
    //        bindDetailBOA(_yrmo, _dcn, _ssn, _updatedt);

    //    }
    //}

    //protected void grdvDupsDupDetails_OnRowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.Equals("Select"))
    //    {
    //        int index = Convert.ToInt32(e.CommandArgument);
    //        GridViewRow row = grdvDupsDupDetails.Rows[index];
    //        string _yrmo = row.Cells[1].Text;
    //        string _ssn = row.Cells[2].Text;
    //        string _dcn = row.Cells[3].Text;
    //        string _updatedt = row.Cells[4].Text;
    //        bindDetailDups(_yrmo, _dcn, _ssn, _updatedt);

    //    }
    //}
    #endregion

    protected void btnOK1_Click(object sender, EventArgs e)
    {
        string _Id;
        lblFErr1.Text = "";

        try
        {
            string _notes = txtDesc1.Text;
            
            if (!cbxReconcile1.Checked)
            {
                lblerr1.Text = "* Required";
            }
            else
            {
                _Id = dtvAmntMis.Rows[3].Cells[1].Text;
                ClaimsRecon adObject = new ClaimsRecon();
                adObject.updateForcedRecon(_Id, "MismatchAmounts", _notes, Session["userName"].ToString());
                MultiView2.SetActiveView(amountMismatch);
                bindAmntMisAging();
            }
            
        }
        catch (Exception ex)
        {
            lblFErr1.Text = ex.Message;
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
                _Id = dtvAnth.Rows[3].Cells[1].Text;
                ClaimsRecon adObject1 = new ClaimsRecon();
                adObject1.updateForcedRecon(_Id, "AnthemMismatch", _notes, Session["userName"].ToString());
                MultiView3.SetActiveView(unmatchedAnthem);
                bindUnmatchedAnthAging();
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
                _Id = dtvUnmatchedBOA.Rows[1].Cells[1].Text;
                ClaimsRecon adObject2 = new ClaimsRecon();
                adObject2.updateForcedRecon(_Id, "BOAMismatch", _notes, Session["userName"].ToString());
                MultiView4.SetActiveView(unmatchedBOA);
                bindUnmatchedBOAAging();
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

    protected void btnOK4_Click(object sender, EventArgs e)
    {
        string _Id;
        lblFErr4.Text = "";

        try
        {
            string _notes = txtDesc4.Text;
            
            if (!cbxReconcile4.Checked)
            {
                lblerr4.Text = "* Required";
            }
            else
            {
                GridViewRow gr = grdvDupsDupDetails.Rows[0];
                _Id = gr.Cells[1].Text;
                ClaimsRecon adObject3 = new ClaimsRecon();
                adObject3.updateForcedRecon(_Id, "DuplicateChecks", _notes, Session["userName"].ToString());
                MultiView5.SetActiveView(dupAdj);
                bindDupsAging();
            }
            
        }
        catch (Exception ex)
        {
            lblFErr4.Text = ex.Message;
        }
        finally
        {
            if (cbxReconcile4.Checked)
            {
                cbxReconcile4.Checked = false;
            }
            txtDesc4.Text = "";
            lblerr4.Text = "";
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

    protected void btnClear2_Click(object sender, EventArgs e)
    {
        if (cbxReconcile2.Checked)
        {
            cbxReconcile2.Checked = false;
        }
        txtDesc2.Text = "";
        lblerr2.Text = "";
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

    protected void btnClear4_Click(object sender, EventArgs e)
    {
        if (cbxReconcile4.Checked)
        {
            cbxReconcile4.Checked = false;
        }
        txtDesc4.Text = "";
        lblerr4.Text = "";
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

        string[][] cols = { new string[] { "YRMO", "Aging Report", "Claim ID","Amount", "Source", "Notes", "User", "Date" }, 
                            new string[] { "YRMO", "Aging Report", "Claim ID","Amount", "Source", "Notes", "User", "Date" }, 
                            new string[] { "YRMO", "Aging Report", "Claim ID","Amount", "Source", "Notes", "User", "Date" }, 
                            new string[] { "YRMO", "Aging Report", "Claim ID","Amount", "Source", "Notes", "User", "Date" } };
        string[][] colsFormat = { new string[] { "string", "string", "string","decimal","string", "string", "string", "string" }, 
                                new string[] { "string", "string", "string","decimal","string", "string", "string", "string" }, 
                                new string[] { "string", "string", "string","decimal","string", "string", "string", "string" }, 
                                new string[] { "string", "string", "string","decimal","string", "string", "string", "string" } };
        string[] sheetnames = { "MismatchAmounts_CF_Adjustments", "UnmatchedAnthem_CF_Adjustments", "UnmatchedBOA_CF_Adjustments", "Duplicate_CF_Adjustments" };
        string[] titles = { "Mismatch Amount - CarryForward Adjustments for YRMO - " + _yrmo, "Unmatched Checks from Anthem - CarryForward Adjustments for YRMO - " + _yrmo, "Unmatched Checks from BOA - CarryForward Adjustments for YRMO - " + _yrmo, "Duplicate Check CarryForwards Adjustments for YRMO - " + _yrmo };

        try
        {
            ds = repObj.generateAuditAdjReport(_yrmo, "MismatchAmounts");
            ds.Tables[0].TableName = "amtMisTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[0].TableName = "amtMisTableF";
            ds.Clear();
            ds = repObj.generateAuditAdjReport(_yrmo, "AnthemMismatch");
            ds.Tables[0].TableName = "anthMisTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "anthMisTableF";
            ds.Clear();
            ds = repObj.generateAuditAdjReport(_yrmo, "BOAMismatch");
            ds.Tables[0].TableName = "boaMisTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "boaMisTableF";
            ds.Clear();
            ds = repObj.generateAuditAdjReport(_yrmo, "DuplicateChecks");
            ds.Tables[0].TableName = "dupsTable";
            dsFinal.Tables.Add(ds.Tables[0].Copy());
            dsFinal.Tables[1].TableName = "dupsTableF";
            ds.Clear();
            ExcelReport.ExcelXMLRpt(dsFinal, "CA_CF_Recon_Adjustments_" + _yrmo, sheetnames, titles, cols, colsFormat);
        }
        finally
        {
        }
    }

    protected void lnkAdjCF_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        CAClaimsDAL caobj = new CAClaimsDAL();
        CAExcelRpt xlobj = new CAExcelRpt();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        DataSet ds = new DataSet();
        ds.Clear();

        string yrmo = caobj.getLatestYRMOAdj();
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
            lblErrRep.Text = "Error in generating Carry Forward report with aging for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnkAdjMatched_OnClick(object sender, EventArgs e)
    {
        lblErrRep.Text = "";
        CAExcelRpt xlobj = new CAExcelRpt();
        CAClaimsDAL caobj = new CAClaimsDAL();
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();
        DataSet ds = new DataSet();
        ds.Clear();

        string yrmo = caobj.getLatestYRMOAdj();
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
            lblErrRep.Text = "Error in generating CA Claims Report of matched and mismatched records for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }
}

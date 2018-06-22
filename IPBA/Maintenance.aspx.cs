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
using System.Collections.Generic;
using EBA.Desktop;
using EBA.Desktop.IPBA;
using EBA.Desktop.Audit;
using EBA.Desktop.Anthem;
using EBA.Desktop.Admin;
using System.IO;

public partial class IPBA_Maintenance : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M300", "M304");            
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);        
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {        
        bindResult();
    }

    protected void btnQueryRate_Click(object sender, EventArgs e)
    {
        bindRateResult();
    }

    private void bindResult()
    {        
        string _progcd = ddlProgCd.SelectedItem.Text;        
        string _eyrmo = ddlEffYrmo.SelectedItem.Text;
        DataSet ds1 = new DataSet();
        lblHeading.Text = "Benefit Hierarchy Information for Program Code - '" + _progcd + "'";
        ds1 = BenhierDAL.getplanhierA(_progcd, _eyrmo);        
        //if (ds1.Tables[0].Rows.Count > 0)
        //{
            grdvBen.DataSource = ds1;
            grdvBen.DataBind();           
        //}
    }

    private void bindRateResult()
    {
        string _py = ddlRpy.SelectedItem.Text;
        string _Rprogcd = ddlRProgcd.SelectedItem.Text;
        //string _Rbencd = ddlRBencd.SelectedItem.Text;
        string _Reyrmo = ddlREffYrmo.SelectedItem.Text;

        DataSet dsR = new DataSet();
        lblRateHeading.Text = "Rates Information for Program Code - '" + _Rprogcd + "'";
        dsR = BenhierDAL.getplanRatesA(_py, _Reyrmo, _Rprogcd);
       
        grdvRates.DataSource = dsR;
        grdvRates.DataBind();
        bindratefooterplan();
        
    }

    
    protected void grdvBen_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvBen.PageIndex = e.NewPageIndex;
        bindResult();
    }

    protected void grdvRates_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvRates.PageIndex = e.NewPageIndex;
        bindRateResult();
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

    protected void grdvBen_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "Ben"); 
    }

    protected void grdvRates_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "Rate");        
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
            
        }
    }

    private void SortGridView(string sortExpression, string direction,string src)
    {        
        DataTable dt;
        DataView dv;
        if (src.Equals("Ben"))
        {
            string _progcd = ddlProgCd.SelectedItem.Text;
            //string _bencd = ddlBencd.SelectedItem.Text;
            string _eyrmo = ddlEffYrmo.SelectedItem.Text;
            dt = BenhierDAL.getplanhierA(_progcd, _eyrmo).Tables[0];
            dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            grdvBen.DataSource = dv;
            grdvBen.DataBind();
        }
        else if (src.Equals("Rate"))
        {
            string _py = ddlRpy.SelectedItem.Text;
            string _Rprogcd = ddlRProgcd.SelectedItem.Text;
            //string _Rbencd = ddlRBencd.SelectedItem.Text;
            string _Reyrmo = ddlREffYrmo.SelectedItem.Text;

            dt = BenhierDAL.getplanRatesA(_py, _Reyrmo, _Rprogcd).Tables[0];
            dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            grdvRates.DataSource = dv;
            grdvRates.DataBind();
            bindratefooterplan();
        }
        else if (src.Equals("PAudit"))
        {
            dt = BenhierDAL.getPlanhierAuditReport().Tables[0];
            dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            grdv_dtlBenAud.DataSource = dv;
            grdv_dtlBenAud.DataBind();            
        }
        else if (src.Equals("RAudit"))
        {
            dt = BenhierDAL.getRateAuditReport().Tables[0];
            dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            grdv_dtlrateAud.DataSource = dv;
            grdv_dtlrateAud.DataBind();
        }
    }

    protected void grdvBen_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdvBen.EditIndex = e.NewEditIndex;
        bindResult();
    }

    protected void grdvBen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdvBen.EditIndex = -1;
        bindResult();
    }

    protected void grdvRates_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdvRates.EditIndex = e.NewEditIndex;
        bindRateResult();
    }

    protected void grdvRates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdvRates.EditIndex = -1;
        bindRateResult();
    }

    //Bind effyrmo based on planyear selected
    protected void ddlRpy_onSelectedindexchange(object sender, EventArgs e)
    {
        string _py = ddlRpy.SelectedItem.Text;
        ddlREffYrmo.Items.Clear();
        ddlREffYrmo.Items.Add("--Select--");
        List<string> _yrmorate = new List<string>();
        _yrmorate = BenhierDAL.getPYyrmo(_py);
        foreach (string _x in _yrmorate)
        {
            ddlREffYrmo.Items.Add(_x);
        }
    }

    //Bind effyrmo based on planyear selected
    protected void ddlRPlancd_onSelectedindexchange(object sender, EventArgs e)
    {
        TextBox _descTB = grdvRates.FooterRow.FindControl("txtRateDescA") as TextBox;
        _descTB.Text="";
        string _pcode = ((DropDownList)grdvRates.FooterRow.FindControl("ddlRatePlancdA")).SelectedItem.Text;
        _descTB.Text = BenhierDAL.planDesc(_pcode);
        DropDownList _tierDDL = (DropDownList)grdvRates.FooterRow.FindControl("ddlRateTierA");
        List<string> _tr = BenhierDAL.getRateTiercodes(_pcode);
        _tierDDL.Items.Clear();
        _tierDDL.Items.Add("--Select--");
        foreach (string t in _tr)
        {
            _tierDDL.Items.Add(t);
        }
    }

    protected void bindratefooterplan()
    {
        string _prgcd = ddlRProgcd.SelectedItem.Text;
        DropDownList _planDDL = grdvRates.FooterRow.FindControl("ddlRatePlancdA") as DropDownList;
        List<string> _pln = BenhierDAL.getRatePlancodes(_prgcd);
        _planDDL.Items.Clear();
        _planDDL.Items.Add("--Select--");
        foreach (string p in _pln)
        {
            _planDDL.Items.Add(p);
        }
    }

    #region insert

    protected void Button1_Click(object sender, EventArgs e)
    {
        lblErrBen.Text = "";
        DropDownList _groupDL = grdvBen.FooterRow.FindControl("ddlGrpA") as DropDownList;
        DropDownList _tiercdDL = grdvBen.FooterRow.FindControl("ddlTierA") as DropDownList;
        TextBox _plancdTB = grdvBen.FooterRow.FindControl("txtPlancdA") as TextBox;
        TextBox _descTB = grdvBen.FooterRow.FindControl("txtDescA") as TextBox;
        TextBox _effyrmoTB = grdvBen.FooterRow.FindControl("txtYrmoA") as TextBox;

        int _benId = Convert.ToInt32(ddlProgCd.SelectedItem.Value);
        string _group = _groupDL.SelectedValue;
        string _tier = _tiercdDL.SelectedValue;
        string _plancd = _plancdTB.Text;
        string _desc = _descTB.Text;
        string _eyrmo = _effyrmoTB.Text;
        string _progcd = ddlProgCd.SelectedItem.Text;
        try
        {
            BenhierDAL.insertPlanhier(_benId,_progcd, _group, _plancd, _desc, _tier, _eyrmo);
            auditInsertBen(_benId, _progcd, _group, _plancd, _desc, _tier, _eyrmo);
            bindResult();
            
        }
        catch(Exception ex)
        {
            lblErrBen.Text = ex.Message;
        }
    }    

    protected void CancelButton1_Click(object sender, EventArgs e)
    {
        ((DropDownList)grdvBen.FooterRow.FindControl("ddlGrpA")).SelectedIndex = 0;
        ((DropDownList) grdvBen.FooterRow.FindControl("ddlTierA")).SelectedIndex = 0;
        ((TextBox)grdvBen.FooterRow.FindControl("txtPlancdA")).Text = "";
        ((TextBox)grdvBen.FooterRow.FindControl("txtDescA")).Text = "";
        ((TextBox)grdvBen.FooterRow.FindControl("txtYrmoA")).Text = "";
        lblErrBen.Text = "";
    }

    protected void Button1R_Click(object sender, EventArgs e)
    {
        lblErrRate.Text = "";
        string _progcd = ddlRProgcd.SelectedItem.Text;
        ratecolumns insertRec = new ratecolumns();
        insertRec.PlanCode = ((DropDownList)grdvRates.FooterRow.FindControl("ddlRatePlancdA")).SelectedItem.Text;
        insertRec.TierCode = ((DropDownList)grdvRates.FooterRow.FindControl("ddlRateTierA")).SelectedItem.Text;
        insertRec.EErate = Convert.ToDecimal(((TextBox)grdvRates.FooterRow.FindControl("txtRateamtA")).Text);
        insertRec.COrate = Convert.ToDecimal(((TextBox)grdvRates.FooterRow.FindControl("txtcoRateamtA")).Text);
        insertRec.PlanDesc = ((TextBox)grdvRates.FooterRow.FindControl("txtRateDescA")).Text;
        insertRec.EffYrmo = ((TextBox)grdvRates.FooterRow.FindControl("txtRateYrmoA")).Text;
        try
        {
            BenhierDAL.insertRates(insertRec,_progcd);
            auditInsertRates(insertRec);
            bindRateResult();
        }
        catch (Exception ex)
        {
            lblErrRate.Text = ex.Message;
        }
    }

    protected void CancelButton1R_Click(object sender, EventArgs e)
    {
        ((DropDownList)grdvRates.FooterRow.FindControl("ddlRatePlancdA")).SelectedIndex = 0;
        ((DropDownList)grdvRates.FooterRow.FindControl("ddlRateTierA")).SelectedIndex = 0;
        ((TextBox)grdvRates.FooterRow.FindControl("txtRateamtA")).Text = "";
        ((TextBox)grdvRates.FooterRow.FindControl("txtcoRateamtA")).Text = "";
        ((TextBox)grdvRates.FooterRow.FindControl("txtRateDescA")).Text = "";
        ((TextBox)grdvRates.FooterRow.FindControl("txtRateYrmoA")).Text = "";
        lblErrRate.Text = "";
    }

    #endregion    

    #region update

    //Update Planhier table
    protected void UpdateRecordBen(object sender, GridViewUpdateEventArgs e)
    {
        lblErrBen.Text = "";

        GridViewRow row = (GridViewRow)grdvBen.Rows[e.RowIndex];

        int _phid = Int32.Parse(grdvBen.DataKeys[e.RowIndex].Value.ToString());

        DropDownList _grpDDLU = (DropDownList)row.FindControl("ddlGrp");
        DropDownList _tierDDLU = (DropDownList)row.FindControl("ddlTier");
        TextBox _planTxtU = (TextBox)row.FindControl("txtPlancd");
        TextBox _dscTxtU = (TextBox)row.FindControl("txtDesc");
        TextBox _yrmoTxtU = (TextBox)row.FindControl("txtYrmo");

        string _grp = _grpDDLU.SelectedItem.Text;
        string _tier = _tierDDLU.SelectedItem.Text;
        string _plan = _planTxtU.Text;
        string _dsc = _dscTxtU.Text;
        string _yrmoeff = _yrmoTxtU.Text;
        string _bid = ddlProgCd.SelectedItem.Value;

        planhierRecord oldValues = BenhierDAL.oldplanhierValues(_phid);
        try
        {
            BenhierDAL.updatePlanhier(_phid, _grp, _plan, _dsc, _tier, _yrmoeff);
            auditUpdateBen(oldValues, _phid, _grp, _tier, _plan, _dsc, _yrmoeff,_bid);
            grdvBen.EditIndex = -1;
            bindResult();
        }
        catch (Exception ex)
        {
            lblErrBen.Text = ex.Message;
        }
    }

    protected void UpdateRecordRates(object sender, GridViewUpdateEventArgs e)
    {
        lblErrRate.Text = "";

        GridViewRow row = (GridViewRow)grdvRates.Rows[e.RowIndex];
        int _rid = Int32.Parse(grdvRates.DataKeys[e.RowIndex].Value.ToString());

        TextBox _rateTxt = (TextBox)row.FindControl("txtRateamtA");
        TextBox _corateTxt = (TextBox)row.FindControl("txtcoRateamtA");
        decimal rate1 = Convert.ToDecimal(_rateTxt.Text);
        decimal corate = Convert.ToDecimal(_corateTxt.Text);
        ratecolumns oldRValues = BenhierDAL.oldRatesValues(_rid);
        try
        {
            BenhierDAL.updateRates(_rid, rate1, corate);
            auditUpdateRates(oldRValues, _rid, rate1, corate);
            grdvRates.EditIndex = -1;
            bindRateResult();
        }
        catch (Exception ex)
        {
            lblErrRate.Text = ex.Message;
        }
    }

    #endregion

    #region delete

    //Delete Planhier table
    protected void DeleteRecordBen(object sender, GridViewDeleteEventArgs e)
    {

        lblErrBen.Text = "";

        GridViewRow row = (GridViewRow)grdvBen.Rows[e.RowIndex];

        int _phid = Int32.Parse(grdvBen.DataKeys[e.RowIndex].Value.ToString());
        planhierRecord delRec = new planhierRecord();

        Label _glbl = (Label)row.FindControl("lblGrp");
        Label _tlbl = (Label)row.FindControl("lblTier");
        Label _plbl = (Label)row.FindControl("lblPlancd");
        Label _dlbl = (Label)row.FindControl("lblDesc");
        Label _ylbl = (Label)row.FindControl("lblYrmo");

        delRec.PlanGroup = _glbl.Text;
        delRec.TierCode = _tlbl.Text;
        delRec.PlanCode = _plbl.Text;
        delRec.PlanDesc = _dlbl.Text;
        delRec.EffYrmo = _ylbl.Text;
        
        try
        {
            BenhierDAL.deletePlanhier(_phid);
            auditDeleteBen(delRec, _phid, ddlProgCd.SelectedItem.Value);
            //ddlEffYrmo.DataBind();
            //ddlProgCd.DataBind();
            bindResult();
        }
        catch (Exception ex)
        {
            lblErrBen.Text = ex.Message;
        }
    }

    protected void DeleteRecordRates(object sender, GridViewDeleteEventArgs e)
    {

        lblErrRate.Text = "";

        GridViewRow row = (GridViewRow)grdvRates.Rows[e.RowIndex];

        int _rid = Int32.Parse(grdvRates.DataKeys[e.RowIndex].Value.ToString());
        ratecolumns delRec = new ratecolumns();   
        Label _r1lbl = (Label)row.FindControl("lblRateamtA");
        Label _r2lbl = (Label)row.FindControl("lblcoRateamtA");
        Label _ylbl = (Label)row.FindControl("lblRateYrmoA");
        delRec.EErate = Convert.ToDecimal(_r1lbl.Text);
        delRec.COrate = Convert.ToDecimal(_r2lbl.Text);        
        delRec.EffYrmo = _ylbl.Text;

        try
        {
            BenhierDAL.deleteRates(_rid);
            auditDeleteRates(delRec, _rid);
            bindRateResult();
        }
        catch (Exception ex)
        {
            lblErrRate.Text = ex.Message;
        }
    }

    #endregion

    #region audit

    protected void auditInsertBen(int _bid,string _prog, string _grp, string _pcd, string _dsc, string _tier, string _mo)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        string _pid = Session["phid"].ToString();
        //Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_ben_id", _pid, "", "", _bid.ToString());
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_progmcd", _pid,_bid.ToString(), "", _prog);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_plangrp", _pid, _bid.ToString(), "", _grp);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_plandesc", _pid, _bid.ToString(), "", _dsc);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_plancd", _pid, _bid.ToString(), "", _pcd);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_tiercd", _pid, _bid.ToString(), "", _tier);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_yrmo", _pid, _bid.ToString(), "", _mo);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Planhier", "ph_py", _pid, _bid.ToString(), "", _mo.Substring(0, 4));
    }

    protected void auditUpdateBen(planhierRecord oValues, int _pid, string _grp1, string _tier1, string _plan1, string _dsc1, string _yrm1,string _bid)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        if (!oValues.PlanGroup.Equals(_grp1))
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Planhier", "ph_plangrp", _pid.ToString(), _bid.ToString(), oValues.PlanGroup, _grp1);
        }
        if (!oValues.PlanCode.Equals(_plan1))
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Planhier", "ph_plancd", _pid.ToString(), _bid.ToString(), oValues.PlanCode, _plan1);
        }
        if (!oValues.PlanDesc.Equals(_dsc1))
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Planhier", "ph_plandesc", _pid.ToString(), _bid.ToString(), oValues.PlanDesc, _dsc1);
        }
        if (!oValues.TierCode.Equals(_tier1))
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Planhier", "ph_tiercd", _pid.ToString(), _bid.ToString(), oValues.TierCode, _tier1);
        }
        if (!oValues.EffYrmo.Equals(_yrm1))
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Planhier", "ph_yrmo", _pid.ToString(), _bid.ToString(), oValues.EffYrmo, _yrm1);
        }
    }

    protected void auditDeleteBen(planhierRecord dValues,int _pid, string _progmcd)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Planhier", "ph_plangrp", _pid.ToString(), _progmcd, dValues.PlanGroup, "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Planhier", "ph_plancd", _pid.ToString(), _progmcd, dValues.PlanCode, "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Planhier", "ph_plandesc", _pid.ToString(), _progmcd, dValues.PlanDesc, "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Planhier", "ph_tiercd", _pid.ToString(), _progmcd, dValues.TierCode, "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Planhier", "ph_yrmo", _pid.ToString(), _progmcd, dValues.EffYrmo, "");

    }

    protected void auditInsertRates(ratecolumns iRec)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        string _rid = Session["rateid"].ToString();
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Rates", "rate_rateamt", _rid, Session["ratephid"].ToString(), "", iRec.EErate.ToString());
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Rates", "rate_companyRtamt", _rid, Session["ratephid"].ToString(), "", iRec.COrate.ToString());
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Rates", "rate_effyrmo", _rid, Session["ratephid"].ToString(), "", iRec.EffYrmo);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Insert", "Rates", "rate_py", _rid, Session["ratephid"].ToString(), "", iRec.EffYrmo.Substring(0, 4));
    }

    protected void auditUpdateRates(ratecolumns oValuesR, int _rid, decimal _rate1, decimal _corate)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        if (oValuesR.EErate!=_rate1)
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Rates", "rate_rateamt", _rid.ToString(), Session["ratephid"].ToString(), oValuesR.EErate.ToString(), _rate1.ToString());
        }
        if (oValuesR.COrate != _corate)
        {
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Update", "Rates", "rate_companyRtamt", _rid.ToString(), Session["ratephid"].ToString(), oValuesR.COrate.ToString(), _corate.ToString());
        }
    }

    protected void auditDeleteRates(ratecolumns dValuesR, int _rid)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Rates", "rate_rateamt", _rid.ToString(), Session["ratephid"].ToString(), dValuesR.EErate.ToString(), "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Rates", "rate_companyRtamt", _rid.ToString(), Session["ratephid"].ToString(), dValuesR.COrate.ToString(), "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Rates", "rate_effyrmo", _rid.ToString(), Session["ratephid"].ToString(), dValuesR.EffYrmo, "");
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Benefit Hierarchy", "NRC", "Delete", "Rates", "rate_py", _rid.ToString(), Session["ratephid"].ToString(), dValuesR.EffYrmo.Substring(0,4), "");       

    }

    #endregion

    #region reports

    protected void lnk_genBenRpt_OnClick(object sender, EventArgs e)
    {
        lblErr1.Text = "";
        string _filter = ddlFilter1.SelectedItem.Text;
        DataSet dsrptfinal = new DataSet(); dsrptfinal.Clear();
        if (_filter.Equals("All"))
        {
            dsrptfinal = BenhierDAL.benhierReport();
        }
        else
        {
            dsrptfinal = BenhierDAL.benhierReport(_filter);
        }

        string filename = "Benefits_Hierarchy";
        string[] sheetnames = { "ben_hierarchy" };
        string[][] titles = { new string[] { "Benefits Hierarchy Report" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "string","number" } };
        int[][] colsWidth = { new int[] { 100, 100, 70, 200, 80, 80, 100 } };
        CreateExcelRpt xlobj = new CreateExcelRpt();
        xlobj.ExcelXMLRpt(dsrptfinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
    }

    protected void lnk_genRateRpt_OnClick(object sender, EventArgs e)
    {
        lblErr1.Text = "";
        string _filterPY = ddlFilterPY.SelectedItem.Text;
        string _filterPgCd = ddlFilterPgCd.SelectedItem.Text;

        DataSet dsrptfinal1 = new DataSet(); dsrptfinal1.Clear();
        if (_filterPgCd.Equals("All"))
        {
            dsrptfinal1 = BenhierDAL.rateReport(_filterPY);
        }
        else
        {
            dsrptfinal1 = BenhierDAL.rateReport(_filterPY,_filterPgCd);
        }

        string filename = "Rates_" + _filterPY;
        string[] sheetnames = { "rates_" + _filterPY };
        string[][] titles = { new string[] { "Rates Information for Plan year " + _filterPY } };
        string[][] colsFormat = { new string[] { "number", "string", "string", "string", "string", "string", "string","decimal","decimal","number" } };
        int[][] colsWidth = { new int[] { 70, 100, 90, 70, 200, 80, 80, 75, 90, 100 } };
        CreateExcelRpt xlobj = new CreateExcelRpt();
        xlobj.ExcelXMLRpt(dsrptfinal1, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
    }

    protected void lnk_genTemplateRpt_OnClick(object sender, EventArgs e)
    {
        lblErr1.Text = "";
        string _PY = txtTempPlanYear.Text;
        bool _exists = BenhierDAL.checkRatePY(_PY);
        if (_exists)
        {
            string msg = "Rates for entered plan year already defined!";
            string alertScript = "<script language=JavaScript> alert('" + msg + "'); <" + "/script>";
            if (!ClientScript.IsClientScriptBlockRegistered("alert"))
                ClientScript.RegisterClientScriptBlock(typeof(Page), "alert", alertScript);
        
        }
        else
        {
            DataSet dsrptTemp1 = new DataSet(); dsrptTemp1.Clear();
            dsrptTemp1 = BenhierDAL.rateTemplate(_PY);
            string filename = "Rates_Template_" + _PY;
            string[] sheetnames = { "rates" + _PY };
            string[][] titles = { new string[] { "" } };
            string[][] colsFormat = { new string[] { "number", "string", "string", "string", "string", "string", "string", "decimal", "decimal", "number" } };
            int[][] colsWidth = { new int[] { 70, 100, 90, 70, 200, 80, 80, 75, 90, 100 } };
            CreateExcelRpt xlobj = new CreateExcelRpt();
            xlobj.ExcelXMLRpt(dsrptTemp1, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        }
    }

    protected void btn_import_Click(object sender, EventArgs e)
    {
        string strFilePath1 = "";
        lbl_error.Text = "";
        lbl_result.Text = "";
        string _yr = txtTempPlanYearImp.Text;
        if (FileUpload1.GotFile && Page.IsValid)
        {
            try
            {
                bool _pycheck = BenhierDAL.checkRatePY(_yr);
                if (!_pycheck)
                {
                    string fn = System.IO.Path.GetFileName(FileUpload1.FilePost.FileName);
                    if (fn.Contains(_yr))
                    {
                        strFilePath1 = Server.MapPath("~/uploads/") + fn;
                        if (File.Exists(strFilePath1))
                        {
                            File.Delete(strFilePath1);
                        }

                        FileUpload1.FilePost.SaveAs(strFilePath1);
                        int _cnt = IPBAImport.importRatesTemplate(strFilePath1, _yr);
                        lbl_result.Text = "Imported Rates successfully for year - " + _yr;
                        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                        Audit.auditUserTaskINRC(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "New Rates Import", "Rates", "New Rates", _yr, _cnt);
                    }
                    else
                    {
                        throw (new Exception("File Plan year doesn't match with entered Plan year!"));
                    }
                }
                else
                {
                    throw (new Exception("Rates for the entered Plan Year already defined!"));
                }
            }
            catch (Exception ex)
            {                
                lbl_error.Text = "Error in importing file. " + ex.Message;
            }
            finally
            {
                FileUpload1.FilePost.InputStream.Flush();
                FileUpload1.FilePost.InputStream.Close();
                FileUpload1.FilePost.InputStream.Dispose();
                if (File.Exists(strFilePath1))
                {
                    File.Delete(strFilePath1);
                }
            }
        }
    }

    #endregion

    protected void lnkAudBen_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (dtlRptDivA1.Visible == true)
            {
                img_dtl.ImageUrl = "~/styles/images/collapsed1.gif";
                dtlRptDivA1.Visible = false;
            }
            else
            {
                img_dtl.ImageUrl = "~/styles/images/expanded1.gif";
                dtlRptDivA1.Visible = true;
                bindAuditReport("Planhier");
            }
        }
        catch (Exception ex)
        {
            //lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkAudRate_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (dtlRptDivA2.Visible == true)
            {
                img_dtl1.ImageUrl = "~/styles/images/collapsed1.gif";
                dtlRptDivA2.Visible = false;
            }
            else
            {
                img_dtl1.ImageUrl = "~/styles/images/expanded1.gif";
                dtlRptDivA2.Visible = true;
                bindAuditReport("Rates");
            }
        }
        catch (Exception ex)
        {
            //lbl_error.Text = "Error: " + ex.Message;
        }
    }

    void bindAuditReport(string _src)
    {
        DataSet bindDs = new DataSet();
        bindDs.Clear();

        switch (_src)
        {
            case "Planhier":
                bindDs = BenhierDAL.getPlanhierAuditReport();
                grdv_dtlBenAud.DataSource = bindDs;
                grdv_dtlBenAud.DataBind();
                break;
            case "Rates":
                bindDs = BenhierDAL.getRateAuditReport();
                grdv_dtlrateAud.DataSource = bindDs;
                grdv_dtlrateAud.DataBind();
                break;
        }
    }

    protected void grdv_dtlBenAud_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "PAudit");
    }

    protected void grdv_dtlrateAud_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "RAudit");
    }

    protected void lnk_genDtlRptA1_OnClick(object sender, EventArgs e)
    {
        string filename = "Benefit_Hierarchy_Audit";
        string[] sheetnames = { "ben_audit"};
        string[] titles = { "Audit Information for Benefit Hierarchy" };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "string","string"}};
        CAExcelRpt xlobj = new CAExcelRpt();
        DataSet dsrptA1 = new DataSet();
        dsrptA1 = BenhierDAL.getPlanhierAuditReport();
        xlobj.ExcelXMLRpt(dsrptA1, filename, sheetnames, titles, colsFormat);                     
    }

    protected void lnk_genDtlRptA2_OnClick(object sender, EventArgs e)
    {
        string filename = "Planhier_Rate_Audit";
        string[] sheetnames = { "rate_audit" };
        string[] titles = { "Audit Information for Planhier Rates" };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string" } };
        CAExcelRpt xlobj = new CAExcelRpt();
        DataSet dsrptA2 = new DataSet();
        dsrptA2 = BenhierDAL.getRateAuditReport();
        xlobj.ExcelXMLRpt(dsrptA2, filename, sheetnames, titles, colsFormat);    
    }

    
}

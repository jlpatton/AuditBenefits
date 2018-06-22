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
using System.Data.SqlClient;
using EBA.Desktop;
using EBA.Desktop.Admin;
using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using System.Collections.Generic;

public partial class Anthem_Maintenance_CodesTable1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M100", "M103");
            hideDiv();
        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    //Select Plan to display rates
    protected void ddlPlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        hideDiv();
        if (ddlPlan.SelectedItem.Text.Equals("Domestic"))
        {
            hideDiv();
            domDiv.Visible = true;
            grdDom.DataBind();
        }
        else if (ddlPlan.SelectedItem.Text.Equals("International"))
        {
            hideDiv();
            intlDiv.Visible = true;
            grdIntl.DataBind();
        }
        else if (ddlPlan.SelectedItem.Text.Equals("EAP"))
        {
            hideDiv();
            divEAP.Visible = true;
            grdEAP.DataBind();
        }

    }

    protected void hideDiv()
    {
        domDiv.Visible = false;
        intlDiv.Visible = false;
        divEAP.Visible = false;
        MessageLabel.Text = "";
    }

    /// <summary>
    /// After adding a Threshold, hide the WebWindow
    /// </summary>
    protected void frmAdd_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        winAdd.Hide = true;
        grdThreshold.DataBind();
    }

    /// <summary>
    /// After clicking Cancel, hide the WebWindow
    /// </summary>
    protected void frmAdd_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winAdd.Hide = true;
        grdThreshold.DataBind();
    }


    /// <summary>
    /// After adding a Planhier, hide the WebWindow
    /// </summary>
    protected void frmAddP_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        if (e.Exception == null)
        {
            MessageLabel.Text = "Record inserted successfully.";
            insertAudit(e);
        }
        else
        {
            // Insert the code to handle the exception.
            MessageLabel.Text = e.Exception.InnerException.Message;

            // Use the ExceptionHandled property to indicate that the 
            // exception has already been handled.
            e.ExceptionHandled = true;
        }        
        winAddP.Hide = true;
    }

    /// <summary>
    /// After clicking Cancel, hide the WebWindow
    /// </summary>
    protected void frmAddP_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winAddP.Hide = true;
    }

    /// <summary>
    /// Update a Planhier in the database
    /// </summary>
    protected void grdvDom_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        // Refresh GridView
        grdDom.DataBind();
    }

    /// <summary>
    /// Update a Planhier in the database
    /// </summary>
    protected void grdvIntl_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        // Refresh GridView
        grdIntl.DataBind();
    }
    protected void grdvEAP_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        // Refresh GridView
        grdEAP.DataBind();
    }
    /// <summary>
    /// Hide the Edit WebWindow after clicking Cancel or Update
    /// </summary>
    protected void frmEditD_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winEdit.Hide = true;
    }

    protected void grdDom_SelectedIndexChanged(object sender, EventArgs e)
    {
        winEdit.Hide = false;
    }

    protected void grdIntl_SelectedIndexChanged(object sender, EventArgs e)
    {
        winEdit1.Hide = false;
    }

    protected void frmEditI_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winEdit1.Hide = true;
    }

    protected void grdEAP_SelectedIndexChanged(object sender, EventArgs e)
    {
        winEdit2.Hide = false;
    }

    protected void frmEditE_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winEdit2.Hide = true;
    }
    protected void grdDomResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Since we are changing the parameters we will not use the cached
        //object datasource we will call the select method again.
        if (srcDPlanhier.SelectParameters[0].DefaultValue == null ||
            srcDPlanhier.SelectParameters[1].DefaultValue == null ||
            srcDPlanhier.SelectParameters[0].DefaultValue !=
            e.SortExpression ||
            srcDPlanhier.SelectParameters[1].DefaultValue == "Desc")
        {
            //sort direction
            srcDPlanhier.SelectParameters[1].DefaultValue = "Asce";
        }
        else if (srcDPlanhier.SelectParameters[1].DefaultValue == "Asce")
        {
            srcDPlanhier.SelectParameters[1].DefaultValue = "Desc";
        }

        //Which column to sort on.
        srcDPlanhier.SelectParameters[0].DefaultValue = e.SortExpression;

        //We have to do this or we will get an exception
        e.Cancel = true;
    }

    protected void grdIntlResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Since we are changing the parameters we will not use the cached
        //object datasource we will call the selecte method again.
        if (srcIPlanhier.SelectParameters[0].DefaultValue == null ||
            srcIPlanhier.SelectParameters[1].DefaultValue == null ||
            srcIPlanhier.SelectParameters[0].DefaultValue !=
            e.SortExpression ||
            srcIPlanhier.SelectParameters[1].DefaultValue == "Desc")
        {
            //sort direction
            srcIPlanhier.SelectParameters[1].DefaultValue = "Asce";
        }
        else if (srcIPlanhier.SelectParameters[1].DefaultValue == "Asce")
        {
            srcIPlanhier.SelectParameters[1].DefaultValue = "Desc";
        }

        //Which column to sort on.
        srcIPlanhier.SelectParameters[0].DefaultValue = e.SortExpression;

        //We have to do this or we will get an exception
        e.Cancel = true;
    }

    protected void grdEAPResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Since we are changing the parameters we will not use the cached
        //object datasource we will call the selecte method again.
        if (srcIPlanhier.SelectParameters[0].DefaultValue == null ||
            srcIPlanhier.SelectParameters[1].DefaultValue == null ||
            srcIPlanhier.SelectParameters[0].DefaultValue !=
            e.SortExpression ||
            srcIPlanhier.SelectParameters[1].DefaultValue == "Desc")
        {
            //sort direction
            srcIPlanhier.SelectParameters[1].DefaultValue = "Asce";
        }
        else if (srcIPlanhier.SelectParameters[1].DefaultValue == "Asce")
        {
            srcIPlanhier.SelectParameters[1].DefaultValue = "Desc";
        }

        //Which column to sort on.
        srcIPlanhier.SelectParameters[0].DefaultValue = e.SortExpression;

        //We have to do this or we will get an exception
        e.Cancel = true;
    }

    protected void ddlReconType_SelectedIndexchange(object sender, EventArgs e)
    {
        string _reconType = ddlReconType.SelectedItem.Text;
        AddThreshold aObj = new AddThreshold();
        List<string> _pCodes = new List<string>();
        ddlSubType.Visible = false;
        lblSubType.Visible = false;
        ddlPlancode.Items.Clear();
        switch (_reconType)
        {
            case "INTERNATIONAL":
                _pCodes = aObj.getPlancodes("INTERNATIONAL");
                foreach (string pCd in _pCodes)
                {
                    ddlPlancode.Items.Add(new ListItem(pCd));
                }
                ddlPlancode.DataBind();
                break;
            case "DOMESTIC":
                lblSubType.Visible = true;
                ddlSubType.Visible = true;
                break;
        }
    }
    protected void ddlSubType_SelectedIndexchange(object sender, EventArgs e)
    {
        string _subType = ddlSubType.SelectedItem.Text;
        AddThreshold aObj = new AddThreshold();
        List<string> _pCodes = new List<string>();
        ddlPlancode.Items.Clear();
        switch (_subType)
        {            
            case "ACTIVE":
                _pCodes = aObj.getPlancodes("ACTIVE");
                break;
            case "RETIREE":
                _pCodes = aObj.getPlancodes("RETIREE");
                break;
            case "COBRA":
                _pCodes = aObj.getPlancodes("COBRA");
                break;
        }
        foreach (string pCd in _pCodes)
        {
            ddlPlancode.Items.Add(new ListItem(pCd));            
        }
        ddlPlancode.DataBind();
    }

    protected void btnAddThres_OnClick(object sender, EventArgs e)
    {
        string _plan = null;
        if (Page.IsValid)
        {
            if (ddlThres.SelectedItem.Text.Equals("Grand Total"))
            {
                if (ddlReconType.SelectedItem.Text.Equals("DOMESTIC"))
                {
                    switch (ddlSubType.SelectedItem.Text)
                    {
                        case "ACTIVE":
                            _plan = "DOMESTIC - ACTIVE";
                            break;
                        case "RETIREE":
                            _plan = "DOMESTIC - RETIREE";
                            break;
                        case "COBRA":
                            _plan = "DOMESTIC - COBRA";
                            break;
                    }
                }
                else if (ddlReconType.SelectedItem.Text.Equals("INTERNATIONAL"))
                {
                    _plan = "INTERNATIONAL";
                }
            }
            else if (ddlThres.SelectedItem.Text.Equals("Tier Codes"))
            {
                _plan = ddlPlancode.SelectedItem.Text + " - " + ddltiercode.SelectedItem.Text;
            }
            else
            {
                _plan = ddlPlancode.SelectedItem.Text;
            }
            string _thresType = ddlThres.SelectedItem.Text;
            decimal _thresvalue = Decimal.Parse(txtValue.Text.ToString());
            //DateTime _thresDate = DateTime.Parse(txtDate.Text.ToString());
            string _thresyrmo = txtDate.Text;
            try
            {
                AddThreshold a1Obj = new AddThreshold();
                a1Obj.insertNewThreshold(_plan, _thresType, _thresvalue, _thresyrmo);
                insertThresAudit(_plan, _thresType, _thresyrmo, _thresvalue.ToString());
                grdvPlanThres.DataBind();
            }
            catch (Exception ex)
            {
                lblErrThres.Text = ex.Message;
            }
            finally
            {
                addThresdiv.Visible = false;
            }
        }

    }

    protected void lnkAddThres_Onclick(object sender, EventArgs e)
    {
        if (addThresdiv.Visible)
        {
            addThresdiv.Visible = false;
        }
        else
        {
            addThresdiv.Visible = true;           
        }
        txtDate.Text = "";
        txtValue.Text = "";
        ddlReconType.SelectedIndex = 0;
        ddlSubType.SelectedIndex = 0;
        ddlThres.SelectedIndex = 0;
        ddlPlancode.Items.Clear();
        lblSubType.Visible = false;
        ddlSubType.Visible = false;
        ddlPlancode.Visible = false;
        lblPlancd.Visible = false;
        lblTierCd.Visible = false;
        ddltiercode.Visible = false;
       
    }

    protected void btnCancelThres_OnClick(object sender, EventArgs e)
    {
        if (addThresdiv.Visible)
        {
            addThresdiv.Visible = false;
        }
        txtDate.Text = "";
        txtValue.Text = "";       
        ddlReconType.SelectedIndex = 0;
        ddlSubType.SelectedIndex = 0;
        ddlThres.SelectedIndex = 0;
        ddlPlancode.Items.Clear();
        lblSubType.Visible = false;
        ddlSubType.Visible = false;
        ddlPlancode.Visible = false;
        lblPlancd.Visible = false;
        lblTierCd.Visible = false;
        ddltiercode.Visible = false;
    }

    protected void ddlThres_SelectedIndexchange(object sender, EventArgs e)
    {
        ddlPlancode.Visible = true;
        lblPlancd.Visible = true;
        lblTierCd.Visible = false;
        ddltiercode.Visible = false;

        string _selected = ddlThres.SelectedItem.Text;
        if (_selected.Equals("Grand Total"))
        {
            ddlPlancode.Visible = false;
            lblPlancd.Visible = false;           
        }
        if (_selected.Equals("Tier Codes"))
        {
            lblTierCd.Visible = true;
            ddltiercode.Visible = true;
        }
    }

    protected void grdThreshold_rowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        updateThresAudit(e,"Default");
    }

    protected void grdPlanThres_rowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        updateThresAudit(e,"Plan");
    }

    protected void grdPlanThres_rowDeleting(Object sender, GridViewDeleteEventArgs e)
    {
        deleteThresAudit(e);
    }
    protected void grdvDom_RowUpdating(Object sender, FormViewUpdateEventArgs e)
    {       
        updateAudit(e);
    }

    protected void grdvIntl_RowUpdating(Object sender, FormViewUpdateEventArgs e)
    {
        updateAudit(e);
    }

    protected void grdvEAP_RowUpdating(Object sender, FormViewUpdateEventArgs e)
    {
        updateAudit(e);
    }

    protected void updateAudit(FormViewUpdateEventArgs e)
    {
        string plhrid = e.OldValues["PlhrId"].ToString();
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) +  1;
        foreach (DictionaryEntry newValues in e.NewValues)
        {
            int i = 0;

            string newKeyCol = newValues.Key.ToString();
            foreach (DictionaryEntry oldVals in e.OldValues)
            {
                string oldKeyCol = oldVals.Key.ToString();

                if (oldKeyCol == newKeyCol)
                {
                    break;
                }
                i++;
            }
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Carrier Plan code table", "NRC", "Update", "AnthPlanhier", newKeyCol, plhrid,"",oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Carrier Plan code table", "NRC", "Update", "AnthPlanhier", newKeyCol, plhrid,"", oldVal, "");
                }
            }
        }
    }

    protected void insertAudit(FormViewInsertedEventArgs e)
    {
        string anthID = Session["anthID"].ToString();
        string plhrID = Session["plhrID"].ToString();
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        string breakcode = null;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();
            switch (newKeyCol)
            {
                case "PlanYear":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, anthID, "AnthCodes");
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
                case "Desc":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
                case "YRMO":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
                case "State":
                    if(newValues.Value.ToString().Equals("CA"))
                    {
                        breakcode = "C";
                    }
                    else if(newValues.Value.ToString().Equals("Non-CA"))
                    {
                        breakcode = "N";
                    }
                    break;
                case "Med":
                    if (newValues.Value.ToString().Equals("Medicare"))
                    {
                        breakcode = breakcode + "M";
                    }
                    else if (newValues.Value.ToString().Equals("Non-Medicare"))
                    {
                        breakcode = breakcode + "N";
                    }
                    insertauditdata("Carrier Plan code table", breakcode, newKeyCol, plhrID, "AnthPlanhier","");
                    break;
                case "AnthemPlanCode":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, anthID, "AnthCodes");
                    break;
                case "AnthemTierCode":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, anthID, "AnthCodes");
                    break;
                case "PlanCode":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
                case "TierCode":
                    insertauditdata("Carrier Plan code table", newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
                case "Rate":
                    insertauditdata("Carrier Plan code table",newValues, newKeyCol, plhrID, "AnthPlanhier");
                    break;
            }
        }
    }

    protected void updateThresAudit(GridViewUpdateEventArgs e,string src)
    {
        string thresId = null;
        string _pName = "";
        switch (src)
        {
            case "Plan":
                thresId = grdvPlanThres.DataKeys[e.RowIndex].Value.ToString();
                break;
            case "Default":
                thresId = grdThreshold.DataKeys[e.RowIndex].Value.ToString();
                break;
        }

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;

        foreach (DictionaryEntry oldVals in e.OldValues)
        {
            string oldKeyCol = oldVals.Key.ToString();

            if (oldKeyCol.Equals("thres_name"))
            {
                _pName = oldVals.Value.ToString();
            }            
        }

        foreach (DictionaryEntry newValues in e.NewValues)
        {
            int i = 0;

            string newKeyCol = newValues.Key.ToString();
            foreach (DictionaryEntry oldVals in e.OldValues)
            {
                string oldKeyCol = oldVals.Key.ToString();

                if (oldKeyCol == newKeyCol)
                {
                    break;
                }
                i++;
            }
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Threshold Table", "NRC", "Update", "threshold", newKeyCol, thresId,_pName, oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Threshold Table", "NRC", "Update", "threshold", newKeyCol, thresId, _pName, oldVal, "");
                }
            }
        }
    }

    protected void insertThresAudit(string _trname, string _trtype,string _trdate,string _trvalue)
    {
        string thresid = Session["thresID"].ToString();
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        insertauditdata("Threshold Table", _trname, "thres_name", thresid, "threshold",_trname);
        insertauditdata("Threshold Table", _trvalue, "thres_value", thresid, "threshold", _trname);
        insertauditdata("Threshold Table", _trdate, "thres_yrmo", thresid, "threshold", _trname);
        insertauditdata("Threshold Table", _trtype, "thres_type", thresid, "threshold", _trname);
    }

    protected void deleteThresAudit(GridViewDeleteEventArgs e)
    {
        string thresID = grdvPlanThres.DataKeys[e.RowIndex].Value.ToString();
        //GridViewRow gr = grdvPlanThres.Rows[e.RowIndex];
        //string _pName = gr.Cells[2].Text;
        string _pName = "";
        foreach (DictionaryEntry newValues in e.Values)
        {
            if (newValues.Key.ToString().Equals("thres_name"))
            {
                _pName = newValues.Value.ToString();
            }
        }
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();            
            deleteauditdata("Threshold Table",newValues,newKeyCol,thresID,_pName,"threshold");
            
        }
    }

    protected void insertauditdata(string _taskType,DictionaryEntry newValues,string _column, string _pkey,string _table)
    {
        if (newValues.Value != null)        
        {
            string newVal = newValues.Value.ToString();            
            if (newValues.Key.ToString().Equals("Desc"))
            {
                newVal = newVal.ToUpper();
            }
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", _taskType , "NRC", "Insert", _table, _column, _pkey,"", "", newVal);
        }        
    }

    protected void insertauditdata(string _taskType,string _value, string _column, string _pkey, string _table, string _pname)
    {        
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", _taskType, "NRC", "Insert", _table, _column, _pkey,_pname, "", _value);
    }

    protected void deleteauditdata(string _taskType, DictionaryEntry newValues, string _column, string _pkey,string _pName, string _table)
    {
        if (newValues.Value != null)
        {
            string newVal = newValues.Value.ToString();
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", _taskType, "NRC", "DELETE", _table, _column, _pkey,_pName,newVal, "");
        }
    }
    //protected void lnkAll_OnClick(object sender, EventArgs e)
    //{        
    //        allAudit.Visible = true;
    //        if (filterAudit.Visible)
    //        {
    //            filterAudit.Visible = false;
    //        }
    //}
    //protected void lnkFilter_OnClick(object sender, EventArgs e)
    //{
    //    filterAudit.Visible = true;
    //    if (allAudit.Visible)
    //    {
    //        allAudit.Visible = false;
    //    }
    //    if (txtFilter.Visible)
    //    {
    //        txtFilter.Visible = true;
    //    }
    //    ddlFilter.SelectedIndex = 0;
    //}

    protected void lnkPlanAudit_OnClick(object sender, EventArgs e)
    {
        if (planAudit.Visible)
        {
            planAudit.Visible = false;
        }
        else
        {
            planAudit.Visible = true;            
        }
        thresAudit.Visible = false;
    }
    protected void lnkThresAudit_OnClick(object sender, EventArgs e)
    {
        if (thresAudit.Visible)
        {
            thresAudit.Visible = false;
        }
        else
        {
            thresAudit.Visible = true;
        }
        planAudit.Visible = false;
    }

    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFilter.Visible = false;
        txtFilter.Text = "";
        ddlType.Visible = false;
        lblAuditerr.Visible = false;
        lblAuditerr.Text = "";
        string _item = ddlFilter.SelectedItem.Text;
        switch (_item)
        {
            case "Users":
                txtFilter.Visible = true;
                break;
            case "Type":
                ddlType.Visible = true;
                break;
        }
    }

    protected void btnExcel_onClick(object sender, EventArgs e)
    {
        lblAuditerr.Visible = false;
        string _item = ddlFilter.SelectedItem.Text;
        string _filterText;
        string cmdstr = "";
        string[] sheetnames = new string[1];
        string[] titles = new string[1];
        string fileName = null;
        string[][] cols = { new string[] { "Task Name", "Carrier Plan Code", "Carrier Coverage Code", "Type", "Column", "Old Value", "New Value", "User Name", "Task Date" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "string", "string","string", "string" } };
        switch (_item)
        {
            case "All":
                cmdstr = "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate " 
                            + "FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id ) "
                            + "UNION "
                            + "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes') ";
                sheetnames[0] = "complete_audit";
                titles[0] = "Complete Audit Details for Carrier Plan codes table";
                fileName = "Plan_Code_Complete_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Users":
                _filterText = txtFilter.Text;
                if (!_filterText.Equals(""))
                {
                    cmdstr = "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                                + "FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes "
                                + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                                + "AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id ) AND UserName LIKE '" + _filterText + "%'"
                                + "UNION "
                                + "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                                + "FROM UserSession_AU, UserTasks_AU,AnthCodes "
                                + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                                + "AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes') AND UserName LIKE '" + _filterText + "%'";
                    sheetnames[0] = "user_audit";
                    titles[0] = "Audit Details for Carrier Plan codes table of User: '" + _filterText + "'";
                    fileName = "Plan_Code_User_Audit_" + DateTime.Now.ToShortDateString();
                }
                else
                {
                    lblAuditerr.Visible = true;
                    lblAuditerr.Text = "Please enter valid Name!";
                }
                break;           
            case "Type":
                _filterText = ddlType.SelectedItem.Text;
                cmdstr = "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id ) AND tType = '" + _filterText + "'"
                            + "UNION "
                            + "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes') AND tType = '" + _filterText + "'";
                sheetnames[0] = _filterText + "_audit";
                titles[0] = _filterText + " Audit Details for Carrier Plan codes table";
                fileName = "Plan_Code_" + _filterText +"_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Rate":
                cmdstr = "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id AND tColumn = 'Rate') "
                            + "UNION "
                            + "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes' AND tColumn = 'Rate') ";
                sheetnames[0] = "rates_audit";
                titles[0] = "Audit Details for Carrier Plan codes table - Rates";
                fileName = "Plan_Code_Rates_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Effective YRMO":
                cmdstr = "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id AND tColumn = 'YRMO') "
                            + "UNION "
                            + "SELECT taskName,anthcd_plancd,anthcd_covgcd,tType,tColumn,tOldValue,tNewValue,UserName,taskDate "
                            + "FROM UserSession_AU, UserTasks_AU,AnthCodes "
                            + "WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId "
                            + "AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes' AND tColumn = 'YRMO') ";
                sheetnames[0] = "effyrmo_audit";
                titles[0] = "Audit Details for Carrier Plan codes table - Effective YRMO";
                fileName = "Plan_Code_EffDate_Audit_" + DateTime.Now.ToShortDateString();
                break;
        }

        if (!cmdstr.Equals(""))
        {
            DataSet ds1 = getFilterAuditData(cmdstr);
            //grdFilterAudit.DataSource = ds1;
            //grdFilterAudit.DataBind();
            generateAuditReports(ds1, sheetnames, titles, fileName, cols, colsFormat);
        }
    }

    protected DataSet getFilterAuditData(string cmdstr)
    {
        string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        SqlConnection connect = null;
        connect = new SqlConnection(connStr);
        SqlCommand command = new SqlCommand(cmdstr, connect);
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        da.SelectCommand = command;
        da.Fill(ds);
        return ds;
    }

    protected void ddlFilter1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFilter1.Visible = false;
        txtFilter1.Text = "";
        ddlType1.Visible = false;
        lblAuditerr.Visible = false;
        lblAuditerr.Text = "";
        string _item = ddlFilter1.SelectedItem.Text;
        switch (_item)
        {
            case "Users":
                txtFilter1.Visible = true;
                break;
            case "Type":
                ddlType1.Visible = true;
                break;
        }
    }

    protected void btnExcel1_onClick(object sender, EventArgs e)
    {
        lblAuditerr.Visible = false;
        string _item = ddlFilter1.SelectedItem.Text;
        string _filterText;
        string cmdstr = "";
        string[] sheetnames = new string[1];
        string[] titles = new string[1];
        string fileName = null;
        string[][] cols = { new string[] { "Task Name","Reconciliation Type","Type", "Column", "Old Value", "New Value", "User Name", "Task Date" } };
        string[][] colsFormat = { new string[] { "string","string" ,"string", "string", "string", "string", "string", "string" } };
        switch (_item)
        {
            case "All":
                cmdstr = "SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate"
                          + "   FROM UserSession_AU, UserTasks_AU"
                          + "   WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId"
                          + "   AND tTable = 'threshold' ";
                sheetnames[0] = "complete_audit";
                titles[0] = "Complete Audit Details for Threshold table";
                fileName = "Thresholds_Complete_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Users":
                _filterText = txtFilter1.Text;
                if (!_filterText.Equals(""))
                {
                    cmdstr = "SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate"
                          + "   FROM UserSession_AU, UserTasks_AU"
                          + "   WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId"
                          + "   AND tTable = 'threshold' AND UserName LIKE '" + _filterText + "%'";
                    sheetnames[0] = "user_audit";
                    titles[0] = "Audit Details for Threshold table of User: '" + _filterText + "'";
                    fileName = "Thresholds_User_Audit_" + DateTime.Now.ToShortDateString();
                }
                else
                {
                    lblAuditerr.Visible = true;
                    lblAuditerr.Text = "Please enter valid Name!";
                }
                break;
            case "Type":
                _filterText = ddlType1.SelectedItem.Text;
                cmdstr = "SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate"
                          + "   FROM UserSession_AU, UserTasks_AU"
                          + "   WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId"
                          + "   AND tTable = 'threshold' AND tType = '" + _filterText + "'";
                sheetnames[0] = _filterText + "_audit";
                titles[0] = _filterText + " Audit Details for Thresholds table";
                fileName = "Thresholds_" + _filterText + "_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Threshold Value":
                cmdstr = "SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate"
                          + "   FROM UserSession_AU, UserTasks_AU"
                          + "   WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId"
                          + "   AND tTable = 'threshold' AND tColumn = 'thres_value'";
                sheetnames[0] = "rates_audit";
                titles[0] = "Audit Details for Thresholds table - Values";
                fileName = "Thresholds_Values_Audit_" + DateTime.Now.ToShortDateString();
                break;
            case "Effective YRMO":
                cmdstr = "SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate"
                          + "   FROM UserSession_AU, UserTasks_AU"
                          + "   WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId"
                          + "   AND tTable = 'threshold' AND tColumn = 'thres_yrmo'";
                sheetnames[0] = "effyrmo_audit";
                titles[0] = "Audit Details for Thresholds table - Effective YRMO";
                fileName = "Thresholds_EffDate_Audit_" + DateTime.Now.ToShortDateString();
                break;
        }

        if (!cmdstr.Equals(""))
        {
            DataSet ds1 = getFilterAuditData(cmdstr);
            //grdFilterAudit.DataSource = ds1;
            //grdFilterAudit.DataBind();
            generateAuditReports(ds1, sheetnames, titles, fileName,cols,colsFormat);
        }
    }


    protected void generateAuditReports(DataSet dsData, string[] sheetnames, string[] titles, string fileName, string[][] cols, string[][] colsFormat)
    {
        
        DataSet dsFinal = new DataSet();
        dsFinal.Clear();        
        dsData.Tables[0].TableName = "audTable";
        dsFinal.Tables.Add(dsData.Tables[0].Copy());
        dsFinal.Tables[0].TableName = "audTableF";
        dsData.Clear();
        ExcelReport.ExcelXMLRpt(dsFinal, fileName, sheetnames, titles, cols,colsFormat);
    }

    /*grid view sorting styles and methods*/

    protected void grdDom_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle(sender, e);
    }

    protected void grdIntl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle(sender, e);
    }

    protected void grdEAP_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle(sender, e);
    }

    protected void grdThreshold_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle(sender, e);
    }

    protected void grdPlanThres_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle(sender, e);
    }

    protected void grdAuditAll_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle1(sender, e);
    }

    protected void grdAuditThres_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        gridViewSortingstyle1(sender, e);
    }

    protected void gridViewSortingstyle(object sender, GridViewRowEventArgs e)
    {
        GridView gridView = (GridView)sender;

        if (gridView.SortExpression.Length > 0)
        {
            int cellIndex = -1;
            //  find the column index for the sorresponding sort expression
            foreach (DataControlField field in gridView.Columns)
            {
                if (field.SortExpression == gridView.SortExpression)
                {
                    cellIndex = gridView.Columns.IndexOf(field);
                    break;
                }
            }

            if (cellIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //  this is a header row,
                    //  set the sort style
                    e.Row.Cells[cellIndex + 1].CssClass +=
                        (gridView.SortDirection == SortDirection.Ascending
                        ? " sortascheader" : " sortdescheader");
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //  this is a data row
                    e.Row.Cells[cellIndex + 1].CssClass +=
                        (e.Row.RowIndex % 2 == 0
                        ? " sortaltrow" : "sortrow");
                }
            }
        }           
    }

    protected void gridViewSortingstyle1(object sender, GridViewRowEventArgs e)
    {
        GridView gridView = (GridView)sender;

        if (gridView.SortExpression.Length > 0)
        {
            int cellIndex = -1;
            //  find the column index for the sorresponding sort expression
            foreach (DataControlField field in gridView.Columns)
            {
                if (field.SortExpression == gridView.SortExpression)
                {
                    cellIndex = gridView.Columns.IndexOf(field);
                    break;
                }
            }

            if (cellIndex > -1)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //  this is a header row,
                    //  set the sort style
                    e.Row.Cells[cellIndex].CssClass +=
                        (gridView.SortDirection == SortDirection.Ascending
                        ? " sortascheader" : " sortdescheader");
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //  this is a data row
                    e.Row.Cells[cellIndex].CssClass +=
                        (e.Row.RowIndex % 2 == 0
                        ? " sortaltrow" : "sortrow");
                }
            }
        }
    }
    
    
}

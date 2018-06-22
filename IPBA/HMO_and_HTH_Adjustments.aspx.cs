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
using EBA.Desktop.IPBA;
using EBA.Desktop.Audit;

public partial class HTH_HMO_and_HTH_adjustments : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (ViewState["filterAdj"] != null)
            SqlDataSourceAdj.FilterExpression = ViewState["filterAdj"].ToString();
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M300", "M303");

            ((TextBox)frmAddAdj.FindControl("txtASSN")).Attributes.Add("onKeyUp", "ShowSSNHistory('" + ((TextBox)frmAddAdj.FindControl("txtASSN")).ClientID + "')");
            ((TextBox)frmAddAdj.FindControl("txtAEffdt")).Attributes.Add("onKeyUp", "MaxMonthsIndicator('" + ((TextBox)frmAddAdj.FindControl("txtAEffdt")).ClientID + "', '" + ((TextBox)frmAddAdj.FindControl("txtAYRMO")).ClientID + "', '" + ((DropDownList)frmAddAdj.FindControl("ddlAHMO")).ClientID + "', '" + ((DropDownList)frmAddAdj.FindControl("ddlAtran")).ClientID + "')");            
            
            ddlYrmoList();            
        }
    }

    protected void ddlYrmoList()
    {
        ImportDAL iobj = new ImportDAL();
        string prevYRMO, filterexp;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = iobj.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
        filterexp = "yrmo='" + ddlYrmo.SelectedItem.Text + "'";
        SqlDataSourceAdj.FilterExpression = filterexp;
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        lbl_error1.Text = "";
        frmAddAdj.Visible = false;

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
                ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void ddlProgcd_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        lbl_error1.Text = "";

        ChangeFilterExpPlanCd(!ddlProgcd.SelectedItem.Text.Contains("Non-Pilot"));  
        ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
    }

    protected void ddlHMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        lbl_error1.Text = "";
        
        ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
    }

    protected void btn_ADDYrmo(object sender, EventArgs e)
    {
        lbl_error.Text = "";
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
            ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
            txtPrevYRMO.Visible = false;
            btnAddYrmo.Visible = false;
            btnCancelYrmo.Visible = false;
            MultiView1.SetActiveView(view_main);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        try
        {
            ddlYrmo.SelectedIndex = 0;
            ddlYrmo.Visible = true;
            ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
            txtPrevYRMO.Visible = false;
            btnAddYrmo.Visible = false;
            btnCancelYrmo.Visible = false;
            MultiView1.SetActiveView(view_main);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btnNewAdj_Click(object sender, EventArgs e)
    {
        lbl_error1.Text = "";
        lbl_error.Text = "";
        frmEditAdj.Visible = false;
        frmAddAdj.Visible = true;
        ((TextBox)frmAddAdj.FindControl("txtAYRMO")).Text = ddlYrmo.SelectedItem.Text;
        ((DropDownList)frmAddAdj.FindControl("ddlAProgcd")).SelectedIndex = ddlProgcd.SelectedIndex;
        ((DropDownList)frmAddAdj.FindControl("ddlAHMO")).SelectedIndex = ddlHMO.SelectedIndex;

        if (!ClientScript.IsClientScriptBlockRegistered("ScrollInto"))
            ClientScript.RegisterClientScriptBlock(typeof(Page), "ScrollInto", "ScrollInto('" + frmAddAdj.ClientID + "');", true);
    }

    protected void frmEditAdj_RowUpdated(Object sender, FormViewUpdatedEventArgs e)
    {
        lbl_error1.Text = "";

        if (e.Exception == null)
        {
            ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
            grdvAdj.DataBind();
        }
        else
        {
            lbl_error1.Text = e.Exception.Message;
            e.ExceptionHandled = true;
        }
        SqlDataSourceAddAdj.SelectParameters.Clear();
        frmEditAdj.Visible = false;
    }

    protected void frmAddAdj_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        lbl_error1.Text = "";

        if (e.Exception == null)
        {
            if (e.AffectedRows != 1)
            {
                lbl_error1.Text = "An error occurred during the insert operation.";
            }
            else
            {
                ResetAddControls();
                insertAudit(e);
                ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
                grdvAdj.DataBind();
            }
        }
        else
        {
            lbl_error1.Text = e.Exception.Message;
            e.ExceptionHandled = true;
        }
        SqlDataSourceAddAdj.InsertParameters.Clear();
        frmAddAdj.Visible = false;
        ((HtmlContainerControl)frmAddAdj.FindControl("SSN_history")).Style["display"] = "none";
    }

    protected void SqlDataSourceAddAdj_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        object newId = Convert.ToInt32(e.Command.Parameters["@NewID"].Value);
        Session["seqNum"] = newId;
    }

    protected void frmAddAdj_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        lbl_error1.Text = "";
        IPBA_DAL dal = new IPBA_DAL();

        string plancode = (frmAddAdj.Row.FindControl("ddlAHMO") as DropDownList).SelectedValue;
        string ssn = (frmAddAdj.Row.FindControl("txtASSN") as TextBox).Text;
        string yrmo = (frmAddAdj.Row.FindControl("txtAYRMO") as TextBox).Text;

        if (!dal.ManAdjDupExists(plancode, ssn, yrmo))
        {
            if (e.Values["progcd"].Equals("PA") || e.Values["progcd"].Equals("PR") || e.Values["progcd"].Equals("CP"))
            {
                if (!(plancode.Equals("P5")))
                    e.Values["plancd"] = plancode.Substring(3, 2);
                else
                    e.Values["plancd"] = plancode;
            }
            else
                e.Values["plancd"] = plancode.Substring(0, 2);
        }
        else
        {
            e.Cancel = true;
            ResetAddControls();
            frmAddAdj.Visible = false;
            lbl_error1.Text = "Record not inserted <br />" + "Record with Plan Code - " + plancode + " AND SSN - " + ssn + " for YRMO - " + yrmo + " already exists!";
        }
    }

    protected void frmEditAdj_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        lbl_error1.Text = "";
        IPBA_DAL dal = new IPBA_DAL();

        string plancode = (frmEditAdj.Row.FindControl("ddlEHMO") as DropDownList).SelectedValue;
        string ssn = (frmEditAdj.Row.FindControl("txtSSN") as TextBox).Text;
        string yrmo = (frmEditAdj.Row.FindControl("txtEYRMO") as TextBox).Text;
        int id = (int)grdvAdj.SelectedValue;

        if (!dal.ManAdjDupExists(plancode, ssn, yrmo, id))
        {
            if (e.NewValues["progcd"].Equals("PA") || e.NewValues["progcd"].Equals("PR") || e.NewValues["progcd"].Equals("CP"))
            {
                if (!(plancode.Equals("P5")))
                    e.NewValues["plancd"] = plancode.Substring(3, 2);
                else
                    e.NewValues["plancd"] = plancode;
            }
            else
                e.NewValues["plancd"] = plancode.Substring(0, 2);

            updateAudit(e, id.ToString());
        }
        else
        {
            e.Cancel = true;
            ResetAddControls();
            frmEditAdj.Visible = false;
            lbl_error1.Text = "Record not updated <br />" + "Record with Plan Code - " + plancode + " AND SSN - " + ssn + " for YRMO - " + yrmo + " already exists!";
        }
    }

    protected void grdvAdj_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lbl_error.Text = "";
        if (e.CommandName.Equals("Select"))
        {
            frmAddAdj.Visible = false;
            frmEditAdj.Visible = true;

            if (!ClientScript.IsClientScriptBlockRegistered("ScrollInto"))
                ClientScript.RegisterClientScriptBlock(typeof(Page), "ScrollInto", "ScrollInto('" + frmEditAdj.ClientID + "');", true);
        }
    }

    protected void frmAddAdj_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        lbl_error1.Text = "";
        if (e.CommandName.Equals("Cancel"))
        {
            ResetAddControls();
            frmAddAdj.Visible = false;
            ((HtmlContainerControl)frmAddAdj.FindControl("SSN_history")).Style["display"] = "none";

            if (!ClientScript.IsClientScriptBlockRegistered("ScrollOut"))
                ClientScript.RegisterClientScriptBlock(typeof(Page), "ScrollOut", "ScrollOut('" + frmAddAdj.ClientID + "');", true);
        }
        if (e.CommandName.Equals("Reset"))
        {
            ResetAddControls();
            ((HtmlContainerControl)frmAddAdj.FindControl("SSN_history")).Style["display"] = "none";
        }
        if (e.CommandName.Equals("close_ssnhistory"))
        {
            ((HtmlContainerControl)frmAddAdj.FindControl("SSN_history")).Style["display"] = "none";
        }
    }

    protected void frmEditAdj_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        lbl_error1.Text = "";
        if (e.CommandName.Equals("Cancel"))
        {
            frmEditAdj.DataBind();
            frmEditAdj.Visible = false;

            if (!ClientScript.IsClientScriptBlockRegistered("ScrollOut"))
                ClientScript.RegisterClientScriptBlock(typeof(Page), "ScrollOut", "ScrollOut('" + frmEditAdj.ClientID + "');", true);
        }
        if (e.CommandName.Equals("Reset"))
        {
            frmEditAdj.DataBind();
        }
    }

    protected void ResetAddControls()
    {
        ((TextBox)frmAddAdj.FindControl("txtASSN")).Text = "";
        ((TextBox)frmAddAdj.FindControl("txtAFname")).Text = "";
        ((TextBox)frmAddAdj.FindControl("txtALname")).Text = "";
        ((TextBox)frmAddAdj.FindControl("txtAEffdt")).Text = "";
        ((TextBox)frmAddAdj.FindControl("txtANotes")).Text = "";
        ((TextBox)frmAddAdj.FindControl("txtAYRMO")).Text = "";
        ((DropDownList)frmAddAdj.FindControl("ddlAType")).SelectedIndex = -1;
        ((DropDownList)frmAddAdj.FindControl("ddlAEvent")).SelectedIndex = -1;
        ((DropDownList)frmAddAdj.FindControl("ddlATran")).SelectedIndex = -1;
        ((DropDownList)frmAddAdj.FindControl("ddlAProgcd")).SelectedIndex = -1;
        ((DropDownList)frmAddAdj.FindControl("ddlAHMO")).SelectedIndex = -1;
    }

    protected void ChangeFilterExp(Boolean allprogcd, Boolean allhmo)
    {
        string filterexp;

        if (allprogcd && allhmo)
        {
            filterexp = "yrmo='" + ddlYrmo.SelectedItem.Text + "'";
            SqlDataSourceAdj.FilterExpression = filterexp;
        }
        else
        {
            if (allprogcd)
            {
                filterexp = "yrmo='" + ddlYrmo.SelectedItem.Text + "' AND plancdadj='" + ddlHMO.SelectedItem.Value + "'";
                SqlDataSourceAdj.FilterExpression = filterexp;                
            }
            else if (allhmo)
            {
                filterexp = "yrmo='" + ddlYrmo.SelectedItem.Text + "' AND progcd='" + ddlProgcd.SelectedItem.Value + "'";
                SqlDataSourceAdj.FilterExpression = filterexp;
            }
            else
            {
                filterexp = "yrmo='" + ddlYrmo.SelectedItem.Text + "' AND progcd='" + ddlProgcd.SelectedItem.Value + "' AND plancdadj='" + ddlHMO.SelectedItem.Value + "'";
                SqlDataSourceAdj.FilterExpression = filterexp;
            }
        }
        ViewState["filterAdj"] = SqlDataSourceAdj.FilterExpression;
    }    

    protected void ChangeFilterExpPlanCd(Boolean PilotSelected)
    {
        string filterexp;
        ListItem selecteditem = ddlHMO.SelectedItem;

        if (!PilotSelected)
        {
            filterexp = "source='HMOBILLRPT' AND codeid <> 'P5'";
        }
        else
        {
            filterexp = "source='HMOBILLRPT'";
        }

        ddlHMO.Items.Clear();
        ddlHMO.Items.Add("--All--");
        SqlDataSourceHMOcd.FilterExpression = filterexp;  
    }

    protected void grdvAdj_rowDeleting(Object sender, GridViewDeleteEventArgs e)
    {
        ChangeFilterExp(ddlProgcd.SelectedItem.Text.Equals("--All--"), ddlHMO.SelectedItem.Text.Equals("--All--"));
        deleteAudit(e);
    }

    protected void updateAudit(FormViewUpdateEventArgs e, string _id)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
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
            if (newKeyCol.Equals("plancd"))
            {
                break;
            }
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Manual Adjustments", "NRC", "Update", "HTH_HMO_Billing", newKeyCol, _id, "", oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Manual Adjustments", "NRC", "Update", "HTH_HMO_Billing", newKeyCol, _id, "", oldVal, "");
                }
            }
        }
    }

    protected void deleteAudit(GridViewDeleteEventArgs e)
    {
        string _ID = grdvAdj.DataKeys[e.RowIndex].Value.ToString();

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();
            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();
                Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Manual Adjustments", "NRC", "DELETE", "HTH_HMO_Billing", newKeyCol, _ID, "", newVal, "");
            }
        }
    }

    protected void insertAudit(FormViewInsertedEventArgs e)
    {
        string _pkey = Session["seqNum"].ToString();
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();
            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();
                Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Manual Adjustments", "NRC", "Insert", "HTH_HMO_Billing", newKeyCol, _pkey, "", "", newVal);
            }
        }
    }

    protected void lnk_genAdjRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string filename, plancode,yrmo;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;        
        CreateExcelRpt xlobj = new CreateExcelRpt();
        DataSet dsFinal = new DataSet(); dsFinal.Clear();
        DataTable tblAdj = dsFinal.Tables.Add("ManualAdj");

        yrmo = ddlYrmo.SelectedItem.Text;
        plancode = ddlHMO.SelectedItem.Value;
        
        try
        {
            if(!(plancode.Equals("--All--")))
                filename = yrmo + "_IPBA_ManAdj_" + plancode;
            else
                filename = yrmo + "_IPBA_ManAdj_All";

            titles = new string[1][];
            sheetnames = new string[1];
            colsFormat = new string[1][];
            colsWidth = new int[1][];

            if (plancode.Equals("P5"))
                titles[0] = new string[] { "HTH Manual Adjustments Report: P5", "Year-Month: " + yrmo };
            else
            {
                if (!(plancode.Equals("--All--")))
                    titles[0] = new string[] { "Local HMO Manual Adjustments Report: " + plancode, "Year-Month: " + yrmo };
                else
                    titles[0] = new string[] { "All HTH & HMO's Manual Adjustments Report", "Year-Month: " + yrmo };
            }

            sheetnames[0] = "ManualAdjs";
            colsFormat[0] = new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string"};
            colsWidth[0] = new int[] { 45, 35, 70, 35, 95, 95, 35, 50, 65, 80 };

            for( int i=2; i < grdvAdj.Columns.Count; i ++)
            {
                tblAdj.Columns.Add(grdvAdj.Columns[i].ToString());
            }
            foreach(GridViewRow row in grdvAdj.Rows)
            {
                DataRow dr = tblAdj.NewRow();               
                for(int i = 0; i < row.Cells.Count-2; i++)
                {
                    dr[i] = row.Cells[i + 2].Text;
                }
                tblAdj.Rows.Add(dr);                
            }
            
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Summary Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void txtASSN_TextChanged(object sender, EventArgs e)
    {
        ((GridView)frmAddAdj.FindControl("grdv_ssnhistory")).DataBind();
        ((HtmlContainerControl)frmAddAdj.FindControl("SSN_history")).Style["display"] = "block";
    }

    protected void itemCreated(Object sender, EventArgs e)
    {
        TextBox t1 = new TextBox(); TextBox t2 = new TextBox();
        DropDownList d1 = new DropDownList(); DropDownList d2 = new DropDownList();
        t1 = (TextBox)frmEditAdj.FindControl("txtEffdt");
        t2 = (TextBox)frmEditAdj.FindControl("txtEYRMO");
        d1 = (DropDownList)frmEditAdj.FindControl("ddlEHMO");
        d2 = (DropDownList)frmEditAdj.FindControl("ddlTran");
        t1.Attributes.Add("onKeyUp", "MaxMonthsIndicator('" + t1.ClientID + "', '" + t2.ClientID + "', '" + d1.ClientID + "', '" + d2.ClientID + "')");              
    }   
}

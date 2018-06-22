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
using EBA.Desktop.Admin;
using EBA.Desktop.Audit;

public partial class VWA_VWA_Adjustments : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M400", "M404");            
        }
    }

    protected void insertAdj(object sender, EventArgs e)
    {
        clearmessages();
        ResetAddControls();
        frmAdj.Visible = true;
    }

    protected void frmAddAdj_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        clearmessages();
        if (e.Exception == null)
        {
            if (e.AffectedRows != 1)
            {
                errorDiv1.Visible = true;
                lblError1.Text = "An error occurred during the insert operation.";
            }
            else
            {
                ResetAddControls();
                insertAudit(e);                
                grdvAdjustments.DataBind();                
            }
        }
        else
        {
            errorDiv1.Visible = true;
            lblError1.Text = e.Exception.Message;
            e.ExceptionHandled = true;
        }
        SqlDataSource2.InsertParameters.Clear();
        frmAdj.Visible = false;
    }

    protected void frmAddAdj_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        clearmessages();
        if (e.CommandName.Equals("Cancel"))
        {
            ResetAddControls();
            frmAdj.Visible = false;
        }
    }

    protected void grdvAdjustments_RowUpdating(Object sender, GridViewUpdateEventArgs e)
    {
        updateAudit(e);
    }

    protected void grdvAdjustments_rowDeleting(Object sender, GridViewDeleteEventArgs e)
    {        
        deleteAudit(e);
    }

    void clearmessages()
    {
        errorDiv1.Visible = false;        
        lblError1.Text = "";       
    }

    void ResetAddControls()
    {
        ((TextBox)frmAdj.FindControl("txtCnumA")).Text = "";
        ((TextBox)frmAdj.FindControl("txtYrmoA")).Text = "";
        ((TextBox)frmAdj.FindControl("txtAmount")).Text = "";
        ((TextBox)frmAdj.FindControl("txtNotesA")).Text = "";
        ((DropDownList)frmAdj.FindControl("ddlAdjTypeA")).SelectedIndex = 0;
    }

    protected void insertAudit(FormViewInsertedEventArgs e)
    {        
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();
            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();
                Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Manual Adjustments", "NRC", "Insert", "VWA_Adjustments", newKeyCol, "", "", "", newVal);
            }
        }
    }

    protected void deleteAudit(GridViewDeleteEventArgs e)
    {
        string _ID = grdvAdjustments.DataKeys[e.RowIndex].Value.ToString();

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        foreach (DictionaryEntry newValues in e.Values)
        {
            string newKeyCol = newValues.Key.ToString();
            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();
                Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Manual Adjustments", "NRC", "DELETE", "VWA_Adjustments", newKeyCol, _ID, "", newVal, "");
            }
        }
    }

    protected void updateAudit(GridViewUpdateEventArgs e)
    {
        string _id = grdvAdjustments.DataKeys[e.RowIndex].Value.ToString();
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
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Manual Adjustments", "NRC", "Update", "VWA_Adjustments", newKeyCol, _id, "", oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Manual Adjustments", "NRC", "Update", "VWA_Adjustments", newKeyCol, _id, "", oldVal, "");
                }
            }
        }
    }

    
}

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
using EBA.Desktop.HRA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.Text.RegularExpressions;

public partial class HRA_Administrative_Bill_Validation_Reconcile_Admin_Invoice : System.Web.UI.Page
{
    private const string _class = "Admin Bill Validation";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            ddlQYList();
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M205"); 
            checkPastRecon();
            txtPrevQY.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevQY.ClientID + "', '" + ddlQY.ClientID + "', /^Q[1234]-\\d{4}$/)");
        }
    }

    protected void ddlQYList()
    {
        HRA iobj = new HRA();
        string prevQY;

        for (int i = 0; i < 4; i++)
        {
            prevQY = iobj.getPrevQY(i);
            ddlQY.Items.Add(prevQY);
        }
        ddlQY.Items.Add("New Quarter-Year");
    }

    protected void ddlQY_selectedIndexchanged(object sender, EventArgs e)
    {
        resultDiv.Visible = false;
        lbl_error.Text = "";
        try
        {
            if (ddlQY.SelectedItem.Text.Equals("New Quarter-Year"))
            {
                txtPrevQY.Text = "";
                ddlQY.Visible = false;
                txtPrevQY.Visible = true;
                lbl_QYfrmt.Visible = true;
                btnCancelQY.Visible = true;
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

    protected void txtPrevQY_textChanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        Regex _r = new Regex(@"^Q[1234]-\d{4}$"); 

        try
        {
            if (!_r.Match(txtPrevQY.Text).Success)
                throw new Exception("Enter Quarter-Year in format 'Qd-yyyy'");

            if (!ddlQY.Items.Contains(new ListItem(txtPrevQY.Text.Trim())))
            {
                ddlQY.Items.Add(txtPrevQY.Text.Trim());
            }
            for (int i = 0; i < ddlQY.Items.Count; i++)
            {
                if (ddlQY.Items[i].Text == txtPrevQY.Text)
                {
                    ddlQY.SelectedIndex = i;
                }
            }

            ddlQY.Visible = true;
            txtPrevQY.Visible = false;
            lbl_QYfrmt.Visible = false;
            txtPrevQY.Text = "";
            btnCancelQY.Visible = false;
            checkPastRecon();
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelQY(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        ddlQY.SelectedIndex = 0;
        ddlQY.Visible = true;
        txtPrevQY.Visible = false;
        lbl_QYfrmt.Visible = false;
        txtPrevQY.Text = "";
        btnCancelQY.Visible = false;
        checkPastRecon();
    }

    protected void checkPastRecon()
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();

        if (HRAAdminDAL.pastReconcile(_qy))
        {
            MultiView1.SetActiveView(view_reconAgn);
            lbl_reconAgn.Text = "Reconciled already for Quarter-Year: " + _qy + ".Do you want to re-reconcile?";
        }
        else
        {
            MultiView1.SetActiveView(view_main);
        }
    }

    protected void btn_reconcile_Click(object sender, EventArgs e)
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        HRAAdminDAL robj = new HRAAdminDAL();
        
        if (Page.IsValid)
        {
            if (txtPrevQY.Visible)
            {
                lbl_error.Text = "Enter Quarter-Year in format 'Qd-yyyy'";
                return;
            }
            try
            {
                Reconcile(_qy);
                View_Result();
                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "Admin Invoice Reconciliation", _qy);
            }
            catch (Exception ex)
            {
                resultDiv.Visible = false;  
                MultiView1.SetActiveView(view_main);
                robj.ReconDelete(_qy);
                lbl_error.Text = "Error in reconciliation.<br />" + ex.Message;
            }
        }
    }

    protected void btn_results_Click(object sender, EventArgs e)
    {
        HRAAdminBill hobj = new HRAAdminBill();
        string _qy = ddlQY.SelectedItem.Text.ToString();
        lbl_error.Text = "";

        if (Page.IsValid)
        {
            if (txtPrevQY.Visible)
            {
                lbl_error.Text = "Enter Quarter-Year in format 'Qd-yyyy'";
                return;
            }
            try
            {
                hobj.CheckRptsImported(_qy);
                View_Result();
                
            }
            catch (Exception ex)
            {
                resultDiv.Visible = false;
                MultiView1.SetActiveView(view_main);
                lbl_error.Text = "Error in reconciliation.<br />" + ex.Message;
            }
        }
    }

    protected void btn_reconAgn_Click(object sender, EventArgs e)
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        HRAAdminDAL robj = new HRAAdminDAL();
        
        try
        {
            if (txtPrevQY.Visible)
            {
                lbl_error.Text = "Enter Quarter-Year in format 'Qd-yyyy'";
                return;
            }
            robj.ReconDelete(_qy);
            Reconcile(_qy);
            View_Result();
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "Admin Invoice Reconciliation", _qy);
        }
        catch (Exception ex)
        {
            resultDiv.Visible = false;
            MultiView1.SetActiveView(view_main);
            robj.ReconDelete(_qy);
            lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
        }
    }

    void View_Result()
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();
        SetNoRecsMsg(_qy);
        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = true;        
    }

    protected void lnk_genReconRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("AdminRecon", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Admin Invoice Reconciliation Report'<br />" + ex.Message;
        }
    }
    
    protected void lnk_genDispAP_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();        

        try
        {
            HRA_Results.GenerateReport("AdminDispAP", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'HRAAUDITR to Putnam Discrepancy Report'<br />" + ex.Message;
        }
    }    

    protected void lnk_genDispWP_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("AdminDispWP", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Wageworks to Putnam Discrepancy Report'<br />" + ex.Message;
        }
    }
    
    protected void lnk_genDispPA_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("AdminDispPA", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Putnam to HRAAUDITR Discrepancy Report'<br />" + ex.Message;
        }
    }

    protected void lnk_genDispPW_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("AdminDispPW", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Putnam to Wageworks Discrepancy Report'<br />" + ex.Message;
        }
    }

    protected void lnk_genWPnoBal_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("AdminWPnoBal", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Report of participant(s) in Wageworks with Zero Asset Balance in Putnam'<br />" + ex.Message;
        }
    }

    protected void lnk_genAUDITR_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("HRAAUDITR", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'HRAAUDITR Report'<br />" + ex.Message;
        }
    }

    protected void lnk_genPtnmPartData_Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _qy = ddlQY.SelectedItem.Text.ToString();

        try
        {
            HRA_Results.GenerateReport("PtnmPartData", _qy);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating 'Putnam Participant Data Report'<br />" + ex.Message;
        }
    }

    void Reconcile(string _qy)
    {
        HRAAdminDAL robj = new HRAAdminDAL();
        HRAAdminBill hobj = new HRAAdminBill();

        hobj.CheckRptsImported(_qy);
        robj.insertReconStatus(_qy);
        HRA_Results.SavePrintFiles(_class, _qy);
    }

    void SetNoRecsMsg(string _qy)
    {
        if (HRAAdminDAL.hasAuditR_PartData_Discp(_qy)) lbl_APNR.Visible = false;
        else lbl_APNR.Visible = true;

        if (HRAAdminDAL.hasPartData_AuditR_Discp(_qy)) lbl_PANR.Visible = false;
        else lbl_PANR.Visible = true;

        if (HRAAdminDAL.hasPartData_WgwkInv_Discp(_qy)) lbl_PWNR.Visible = false;
        else lbl_PWNR.Visible = true;

        if (HRAAdminDAL.hasWgwkInv_PartData_Discp(_qy)) lbl_WPNR.Visible = false;
        else lbl_WPNR.Visible = true;

        if (HRAAdminDAL.hasWgwkInv_PartData_noBal(_qy)) lbl_WPnoBalNR.Visible = false;
        else lbl_WPnoBalNR.Visible = true;

        if (HRAAdminDAL.hasPutnamPartData(_qy)) lbl_PtnmPartDataNR.Visible = false;
        else lbl_PtnmPartDataNR.Visible = true;

        if (HRAAdminDAL.hasAUDITR(_qy)) lbl_AUDITRNR.Visible = false;
        else lbl_AUDITRNR.Visible = true;
    }
}

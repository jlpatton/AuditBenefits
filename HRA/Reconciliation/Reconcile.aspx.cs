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
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class HRA_Reconcile : System.Web.UI.Page
{
    private const string _class = "Reconciliation";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M202");           
            ddlYrmoList();
            checkPastRecon();
            txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
        }
    }

    protected void ddlYrmoList()
    {
        HRA robj = new HRA();
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = robj.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        resultDiv.Visible = false;

        try
        {
            if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO.Text = "";
                ddlYrmo.Visible = false;
                txtPrevYRMO.Visible = true;
                lbl_yrmofrmt.Visible = true;
                btnCancelYRMO.Visible = true;
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

    protected void txtPrevYRMO_textChanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        Regex _r = new Regex(@"^20\d\d(0[1-9]|1[012])$");    

        try
        {
            if (!_r.Match(txtPrevYRMO.Text).Success)
                throw new Exception("Enter YRMO in format 'yyyymm'");

            if (!ddlYrmo.Items.Contains(new ListItem(txtPrevYRMO.Text)))
            {
                ddlYrmo.Items.Add(txtPrevYRMO.Text);
            }
            for (int i = 0; i < ddlYrmo.Items.Count; i++)
            {
                if (ddlYrmo.Items[i].Text == txtPrevYRMO.Text)
                {
                    ddlYrmo.SelectedIndex = i;
                }
            }

            ddlYrmo.Visible = true;
            txtPrevYRMO.Visible = false;
            lbl_yrmofrmt.Visible = false;
            txtPrevYRMO.Text = "";
            btnCancelYRMO.Visible = false;
            checkPastRecon();
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYRMO(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        txtPrevYRMO.Visible = false;
        lbl_yrmofrmt.Visible = false;
        txtPrevYRMO.Text = "";
        btnCancelYRMO.Visible = false;
        checkPastRecon();
    }
    
    protected void checkPastRecon()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;

        if (HRA_ReconDAL.pastReconcile(_yrmo, "Recon"))
        {
            MultiView1.SetActiveView(view_reconAgn);
            lbl_reconAgn.Text = "Reconciled already for year-month (YRMO): " + _yrmo + ".<br />Do you want to reconcile again?";
        }
        else
        {            
            MultiView1.SetActiveView(view_main);
        }
    }

    protected void btn_reconcile_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        
        if (Page.IsValid)
        {
            lbl_error.Text = "";

            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                if (RptsImported())
                {
                    HRA_ReconDAL.ReconDelete(yrmo);
                    HRAReconcile(yrmo);
                    View_Result();
                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "HRA Putnam - Wgwks Recon",yrmo );
                }
            }
            catch (Exception ex)
            {
                resultDiv.Visible = false;
                MultiView1.SetActiveView(view_main);
                HRA_ReconDAL.ReconDelete(yrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }
        }
    }

    protected void btn_results_Click(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                if (RptsImported())
                {
                    View_Result();                    
                    lbl_result.Text = lbl_result.Text.ToString().Replace("<br/>Saved / Printed file(s) that are selected to automatically save/print.", "");
                }
            }
            catch (Exception ex)
            {
                resultDiv.Visible = false;
                MultiView1.SetActiveView(view_main);
                lbl_error.Text = "Error - " + ex.Message;
            }
        }
    }

    protected void btn_reconAgn_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        lbl_error.Text = "";
        
        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {                
                if (RptsImported())
                {
                    HRA_ReconDAL.ReconDelete(yrmo);
                    HRAReconcile(yrmo);
                    View_Result();
                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "HRA Putnam - Wgwks Recon", yrmo);
                }
            }
            catch (Exception ex)
            {
                resultDiv.Visible = false;
                MultiView1.SetActiveView(view_main);
                HRA_ReconDAL.ReconDelete(yrmo);
                lbl_error.Text = "Error in re-reconciliation.<br />" + ex.Message;
            }
        }
    }

    void View_Result()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        
        Boolean hasVariance = HRA_ReconDAL.HasVariance(yrmo);

        if (hasVariance)
        {
            lbl_result.Text = "HRA records do not balance for " + yrmo + ".";
            lbl_reconNR.Visible = false;
        }
        else
        {
            lbl_result.Text = "All HRA records balance for " + yrmo + ".";
            lbl_reconNR.Visible = true;
        }

        lbl_result.Text += "<br/>Saved / Printed file(s) that are selected to automatically save/print.";

        SetNoRecsMsg(yrmo);

        MultiView1.SetActiveView(view_result);
        resultDiv.Visible = true;
    }

    protected void lnk_genReconRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            HRA_Results.GenerateReport("Recon", yrmo);
        }
        catch(Exception ex)
        {
            lbl_error.Text = "Error in generating Reconciliation Report<br />" + ex.Message;
        } 
    }

    protected void lnk_genCFRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            HRA_Results.GenerateReport("CF", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Carry Forward Report<br />" + ex.Message;
        } 
    }

    protected void lnk_genCFnotClearRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            HRA_Results.GenerateReport("CFnotCleared", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Carry Forward not Cleared Report<br />" + ex.Message;
        } 
    }

    protected void lnk_genTranRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
       try
        {
            HRA_Results.GenerateReport("Transaction", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Transaction Report<br />" + ex.Message;
        } 
    }
  
    Boolean RptsImported()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;        
        string[] source = {"Putnam", "Wageworks" };
        string[] rpts = {"Putnam Report", "Wageworks Report"};
        string rptsNotImported = "";        
        HRAImportDAL iobj = new HRAImportDAL();

        for(int i=0; i < source.Length; i++)
        {
            if(!iobj.PastImport(source[i], yrmo))
            {
                rptsNotImported  += rpts[i];
                if(i != (source.Length - 1))
                    rptsNotImported += ", ";
            }
        }

        if (rptsNotImported != "")
        {            
            lbl_error.Text =  rptsNotImported + " for yrmo - " + yrmo + " not imported";
            return false;
        }
        return true;
    }

    void HRAReconcile(string yrmo)
    {
        HRA_ReconDAL.Pass1_InsertPutnam(yrmo);
        HRA_ReconDAL.Pass2_InsertPutnamAdj(yrmo);
        HRA_Recon.Pass3_CurWageworks(yrmo);
        HRA_Recon.Pass4_PriWageworks(yrmo);
        HRA_ReconDAL.Pass5_InsertDiffAmt(yrmo);
        HRA_ReconDAL.insertReconStatus(yrmo, "Recon");
        HRA_Recon.InsertPrevCFData(yrmo);
        HRA_Results.SavePrintFiles(_class, yrmo);
    }

    void SetNoRecsMsg(string yrmo)
    {
        if (HRA_ReconDAL.HasCF(yrmo)) lbl_CFNR.Visible = false;
        else lbl_CFNR.Visible = true;

        if (HRA_ReconDAL.HasPriCF(yrmo)) lbl_CFnotClearedNR.Visible = false;
        else lbl_CFnotClearedNR.Visible = true;

        if (HRA_ReconDAL.HasTran(yrmo)) lbl_tranNR.Visible = false;
        else lbl_tranNR.Visible = true;
    }
}

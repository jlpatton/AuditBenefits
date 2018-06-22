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
using EBA.Desktop.VWA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.Text.RegularExpressions;


public partial class VWA_Create_VWA_Tran_Activity : System.Web.UI.Page
{
    private const string _category = "Transaction";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();            
            AdminBLL.InRole(uname, "M400", "M402");
            ddlYrmoList();
            checkTranImported();
            tbx_agedDays.Attributes.Add("onKeyUp", "DisplayAgedDate('" + tbx_agedDays.ClientID + "', '" + lbl_agedDays.ClientID + "');");
            txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
        }
    }

    protected void ddlYrmoList()
    {
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = VWA.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_err.Text = "";
        lbl_error.Visible = false;
        resultdiv.Visible = false;

        try
        {
            if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO.Text = "";
                lblPrevYrmo.Visible = false;
                ddlYrmo.Visible = false; 
                DivNewYRMO.Visible = true;
            }
            else
            {
                checkTranImported();
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = "Error: " + ex.Message;
        }
    }

    protected void txtPrevYRMO_textChanged(object sender, EventArgs e)
    {
        lbl_err.Text = "";
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

            lblPrevYrmo.Visible = true;
            ddlYrmo.Visible = true;
            txtPrevYRMO.Text = "";
            DivNewYRMO.Visible = false;
            checkTranImported();
        }
        catch (Exception ex)
        {
            lbl_err.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        lbl_err.Text = "";
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        lblPrevYrmo.Visible = true;
        txtPrevYRMO.Text = "";
        DivNewYRMO.Visible = false;
        checkTranImported();
    }

    protected void lnk_genRpts_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        lbl_err.Text = "";
        lbl_error.Visible = false;  

        Session["VWAAgingDays"] = "";
        try
        {
            SetNoRecsMsg(yrmo);
            Session["VWAAgingDays"] = tbx_agedDays.Text.ToString();
            VWA_Results.SavePrintFiles(_category, yrmo);
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Transactions", "Create VWA Transaction Reports", DateTime.Today.ToString("yyyyMMdd"));
            resultdiv.Visible = true;
        }
        catch (Exception ex)
        {
            lbl_err.Text = "Error in generation reports<br />" + ex.Message;
        }
    }

    protected void lnk_genMissRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false; 
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranMis", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_genClientRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false; 
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranClient", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_gen_grpRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranGrp", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_gen_cntRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranStat", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_gen_finRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranACCTG", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_gen_ageRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranAging", tbx_agedDays.Text.ToString());
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_gen_statCtrRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            VWA_Results.GenerateReport("TranStatCtr", yrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true; lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    void checkTranImported()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;

        if (VWAImportDAL.PastImport("TranDtl", _yrmo))
        {
            lbl_err.Text = "";  
        }
        else
        {
            lbl_err.Text = "VWA Transaction Detail for YRMO - " + _yrmo + " not imported!";
        }
    }

    void SetNoRecsMsg(string yrmo)
    {
        if (VWA_ExportDAL.HasRecords(yrmo, "TranMis"))
            lbl_miss.Visible = false;
        else
            lbl_miss.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "TranClient"))
            lbl_client.Visible = false;
        else
            lbl_client.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "TranGrp"))
            lbl_grp.Visible = false;
        else
            lbl_grp.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "TranStat"))
            lbl_cnt.Visible = false;
        else
            lbl_cnt.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "TranACCTG"))
            lbl_fin.Visible = false;
        else
            lbl_fin.Visible = true;

        if (VWA_ExportDAL.HasRecords(tbx_agedDays.Text.ToString(), "TranAging"))
            lbl_age.Visible = false;
        else
            lbl_age.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "TranStatCtr"))
            lbl_statCtr.Visible = false;
        else
            lbl_statCtr.Visible = true;       
    }
}

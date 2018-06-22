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
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Net;

public partial class HRA_Operations_Create_Eligibility_File : System.Web.UI.Page
{
    private const string _class = "Operation";
    private const string _source = "EligFile";      

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M203");
            NameValueCollection eligFileMsg = HRAOperDAL.GetEligFileCreatedMsg();
            if(eligFileMsg.Get("timecreated") != null)
                lbl_notice.Text = "Last time File Generated: " + eligFileMsg.Get("timecreated") + " by " + eligFileMsg.Get("createduser");
        }
    }

    protected void lnk_genRpts_OnClick(object sender, EventArgs e)
    {        
        lbl_err.Text = "";

        try
        {
            HRAOperDAL.InsertStopDate();            
            HRAOperDAL.InsertEligFileContent();
            HRAOperDAL.InsertAuditR();
            NameValueCollection eligFileMsg = HRAOperDAL.GetEligFileCreatedMsg();
            if (eligFileMsg.Get("timecreated") != null)
                lbl_notice.Text = "Last time File Generated: " + eligFileMsg.Get("timecreated") + " by " + eligFileMsg.Get("createduser");
            SetNoRecsMsg();
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Operations","Generate Eligibility File ", DateTime.Today.ToString("yyyyMMdd"));
            HRA_Results.SavePrintFiles(_class, tbx_deathsXdays.Text.ToString());
            resultdiv.Visible = true;
        }
        catch(Exception ex)
        {
            lbl_err.Text = "Error in generation reports<br />" + ex.Message;
        }
    }
    
    protected void lnk_genEligRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("Elig", String.Empty);            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HRA Eligibility File<br />" + ex.Message;
        }
    }

    protected void lnk_genEligAuditRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligAudit", String.Empty);            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Eligibilty Audit Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_termAuditRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligTermAudit", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Terminations Audit Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_eligBen24Rpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
       
        try
        {
            HRA_Results.GenerateReport("EligBen24", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Benificiary Turns 24 Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_childBenLtrRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligChildBenLtr", String.Empty);            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Child Beneficiary Letter Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_eligNoBenRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligNoBen", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HRA Died No Beneficiaries Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_noBenOwnerRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligNoBenOwner", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HRA Died No Owner Verified Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_addrChgRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
       
        try
        {
            HRA_Results.GenerateReport("EligAddrChg", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Address Changes Audit Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_statChgRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
           HRA_Results.GenerateReport("EligStatChg", String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Status Changes Audit Report<br />" + ex.Message;
        }
    }

    protected void lnk_gen_deathsXdaysRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        
        try
        {
            HRA_Results.GenerateReport("EligDeathsXdays", tbx_deathsXdays.Text.ToString());
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating Status Changes Audit Report<br />" + ex.Message;
        }
    }

    void SetNoRecsMsg()
    {               
        if (HRAOperDAL.hasElig())
        {
            lbl_elig.Visible = false;
            lbl_eligAudit.Visible = false;
        }
        else
        {
            lbl_elig.Visible = true;
            lbl_eligAudit.Visible = true;
        }

        if (HRAOperDAL.hasTerm())
            lbl_eligTermAudit.Visible = false;
        else
            lbl_eligTermAudit.Visible = true;

        if (HRAOperDAL.hasAddrChg())
            lbl_addrChg.Visible = false;
        else
            lbl_addrChg.Visible = true;

        if (HRAOperDAL.hasStatChg())
            lbl_statChg.Visible = false;
        else
            lbl_statChg.Visible = true;

        if (HRAOperDAL.hasBen24())
            lbl_eligBen24.Visible = false;
        else
            lbl_eligBen24.Visible = true;

        if (HRAOperDAL.hasChildBenLtr())
            lbl_childBenLtr.Visible = false;
        else
            lbl_childBenLtr.Visible = true;

        if (HRAOperDAL.hasNoBen())
            lbl_eligNoBen.Visible = false;
        else
            lbl_eligNoBen.Visible = true;

        if (HRAOperDAL.hasNoBenOwner())
            lbl_noBenOwner.Visible = false;
        else
            lbl_noBenOwner.Visible = true;

        if (HRAOperDAL.hasDeathXDays(tbx_deathsXdays.Text.ToString()))
            lbl_deathXDays.Visible = false;
        else
            lbl_deathXDays.Visible = true;
    }
}

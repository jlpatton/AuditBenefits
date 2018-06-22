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
using System.Text.RegularExpressions;
using System.Globalization;

public partial class HRA_Reconcile_SOFO : System.Web.UI.Page
{
    private const string _category = "SOFO";
    private const string source = "SOFO";

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
            checkPastRecon1();
            checkPastRecon2();
            initializePageControls();
            /* Alerts when HRA Reconciliation process is not performed or balanced */
            alertMsg1();
            alertMsg2();
        }       
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView2.ActiveViewIndex = Int32.Parse(e.Item.Value);       
    }

    protected void autoImport()
    {
        if (HRAImportDAL.CheckAutoImport(_category, source))
        {
            upload_auto.Visible = true;
            upload_manually.Visible = false;
            lbl_clientfile.Text = HRAImportDAL.GetClientFilePath(_category, source) + "\\" + HRA.GetInputFileName(ddlYrmo1.SelectedItem.Value, source);
        }
        else
        {
            upload_auto.Visible = false;
            upload_manually.Visible = true;
            lbl_clientfile.Text = "";
        }
    }

    protected void ddlYrmoList()
    {
        HRA robj = new HRA();
        string prevYRMO;
        
        for (int i = 0; i < 6; i++)
        {
            prevYRMO = robj.getPrevYRMO(i);
            ddlYrmo1.Items.Add(prevYRMO);
            ddlYrmo2.Items.Add(prevYRMO);
        }
        ddlYrmo1.Items.Add("New Yrmo");
        ddlYrmo2.Items.Add("New Yrmo");
    }    

    protected void ddlYRMO1_selectedIndexchanged(object sender, EventArgs e)
    {
        div1_error.Visible = false;
        resultDiv1.Visible = false;

        try
        {
            if (ddlYrmo1.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO1.Text = "";                
                lblPrevYrmo1.Visible = false;                
                ddlYrmo1.Visible = false;                
                DivNewYRMO1.Visible = true;                
            }
            else
            {
                checkPastRecon1();
                alertMsg1();
            }
        }
        catch (Exception ex)
        {
            div1_error.Visible = true;
            lbl_error1.Text = "Error: " + ex.Message;
        }
    }

    protected void ddlYRMO2_selectedIndexchanged(object sender, EventArgs e)
    {
        div2_error.Visible = false;
        resultDiv2.Visible = false;

        try
        {
            if (ddlYrmo2.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO2.Text = "";               
                lblPrevYrmo2.Visible = false;               
                ddlYrmo2.Visible = false;               
                DivNewYRMO2.Visible = true;
            }
            else
            {
                checkPastRecon2();
                alertMsg2();
            }
        }
        catch (Exception ex)
        {
            div2_error.Visible = true;
            lbl_error2.Text = "Error: " + ex.Message;
        }
    }

    protected void txtPrevYRMO1_textChanged(object sender, EventArgs e)
    {
        div1_error.Visible = false;
        Regex _r = new Regex(@"^20\d\d(0[1-9]|1[012])$"); 

        try
        {
            if (!_r.Match(txtPrevYRMO1.Text).Success)
                throw new Exception("Enter YRMO in format 'yyyymm'");

            if (!ddlYrmo1.Items.Contains(new ListItem(txtPrevYRMO1.Text)))
            {
                ddlYrmo1.Items.Add(txtPrevYRMO1.Text);
            }
            for (int i = 0; i < ddlYrmo1.Items.Count; i++)
            {
                if (ddlYrmo1.Items[i].Text == txtPrevYRMO1.Text)
                {
                    ddlYrmo1.SelectedIndex = i;
                }
            }

            lblPrevYrmo1.Visible = true;
            ddlYrmo1.Visible = true;
            txtPrevYRMO1.Text = "";
            DivNewYRMO1.Visible = false;
            checkPastRecon1();
            alertMsg1();
        }
        catch (Exception ex)
        {
            div1_error.Visible = true;
            lbl_error1.Text = "Error: " + ex.Message;
        }
    }

    protected void txtPrevYRMO2_textChanged(object sender, EventArgs e)
    {
        div2_error.Visible = false;        
        Regex _r = new Regex(@"^20\d\d(0[1-9]|1[012])$");

        try
        {
            if (!_r.Match(txtPrevYRMO2.Text).Success)
                throw new Exception("Enter YRMO in format 'yyyymm'");


            if (!ddlYrmo2.Items.Contains(new ListItem(txtPrevYRMO2.Text)))
            {
                ddlYrmo2.Items.Add(txtPrevYRMO2.Text);
            }
            for (int i = 0; i < ddlYrmo2.Items.Count; i++)
            {
                if (ddlYrmo2.Items[i].Text == txtPrevYRMO2.Text)
                {
                    ddlYrmo2.SelectedIndex = i;
                }
            }

            lblPrevYrmo2.Visible = true;
            ddlYrmo2.Visible = true;
            txtPrevYRMO2.Text = "";
            DivNewYRMO2.Visible = false;
            checkPastRecon2();
            alertMsg2();
        }
        catch (Exception ex)
        {
            div2_error.Visible = true;
            lbl_error2.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo1(object sender, EventArgs e)
    {
        div1_error.Visible = false;
        ddlYrmo1.SelectedIndex = 0;
        ddlYrmo1.Visible = true;
        lblPrevYrmo1.Visible = true;
        txtPrevYRMO1.Text = "";
        DivNewYRMO1.Visible = false;
        checkPastRecon1();
    }

    protected void btn_CancelYrmo2(object sender, EventArgs e)
    {
        div2_error.Visible = false;
        ddlYrmo2.SelectedIndex = 0;
        ddlYrmo2.Visible = true;
        lblPrevYrmo2.Visible = true;
        txtPrevYRMO2.Text = "";
        DivNewYRMO2.Visible = false;
        checkPastRecon2();
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        resultDiv2.Visible = false;
        div2_error.Visible = false;
        div2_alert.Visible = false;
        String yrmo = ddlYrmo2.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();
       
        if (Page.IsValid)
        {
            if (txtPrevYRMO2.Visible)
            {
                lbl_error2.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            try
            {                
                ManualReconcile();
                HRA_Results.SavePrintFiles(_category, yrmo);
                viewresult2(yrmo);
                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "SOFO Reconciliation", yrmo);
            }
            catch (Exception ex)
            {
                resultDiv2.Visible = false;
                MultiView1.SetActiveView(view_main2);
                robj.ReconDelete(yrmo);
                div2_error.Visible = true;
                lbl_error2.Text = "Error - " + ex.Message;
            }
        }       
    }

    protected void btn_manImport_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo1.SelectedItem.Text;
        div1_error.Visible = false;
        div1_alert.Visible = false;
        HRASofoDAL robj = new HRASofoDAL();

        if (Page.IsValid)
        {
            if (txtPrevYRMO1.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            string strFilePath1 = "";

            if (FileUpload1.GotFile)
            {
                try
                {
                    string fn = System.IO.Path.GetFileName(FileUpload1.FilePost.FileName);
                    strFilePath1 = Server.MapPath("~/uploads/") + fn;

                    if (File.Exists(strFilePath1))
                    {
                        File.Delete(strFilePath1);
                    }
                    FileUpload1.FilePost.SaveAs(strFilePath1);
                    AutoReconcile(strFilePath1);
                    HRA_Results.SavePrintFiles(_category, yrmo);
                    viewresult1(yrmo);
                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "SOFO Reconciliation", yrmo);
                }
                catch (Exception ex)
                {
                    resultDiv1.Visible = false;
                    MultiView3.SetActiveView(view_main1);
                    robj.ReconDelete(yrmo);
                    div1_error.Visible = true;
                    lbl_error1.Text = "Error - " + ex.Message;
                }
                finally
                {
                    if (File.Exists(strFilePath1))
                    {
                        File.Delete(strFilePath1);
                    }
                    FileUpload1.FilePost.InputStream.Flush();
                    FileUpload1.FilePost.InputStream.Close();
                    FileUpload1.FilePost.InputStream.Dispose();
                }
            }
        }
    }

    protected void btn_autoImport_Click(object sender, EventArgs e)
    {
        HRASofoDAL robj = new HRASofoDAL();
        Impersonation _imp = new Impersonation();
        string yrmo = ddlYrmo1.SelectedItem.Text;
        string clientfilename, clientfile, serverPath;
        string serverfile = "";
        div1_error.Visible = false;
        div1_alert.Visible = false;

        if (Page.IsValid)
        {
            if (txtPrevYRMO1.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            try
            {
                serverPath = Server.MapPath("~/uploads/");
                clientfilename = HRA.GetInputFileName(yrmo, source);
                serverfile = serverPath + clientfilename;

                if (_imp.impersonateValidUser(Session["uid"].ToString(), "CORP", EncryptDecrypt.Decrypt(Session["pwd"].ToString())))
                {
                    clientfile = HRAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename;
                    File.Copy(clientfile, serverfile);
                    _imp.undoImpersonation();
                }
                else
                {
                    throw new Exception("Error in accessing network location");
                }

                if (!(File.Exists(serverfile)))
                    throw new Exception("Unable to import file from specified location.<br/>Please check if file exists and you are logged in to the network.");

                AutoReconcile(serverfile);
                HRA_Results.SavePrintFiles(_category, yrmo);
                viewresult1(yrmo);
                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "SOFO Reconciliation", yrmo);
            }
            catch (Exception ex)
            {
                MultiView3.SetActiveView(view_main1);
                robj.ReconDelete(yrmo);
                div1_error.Visible = true;
                lbl_error1.Text = "Error - " + ex.Message;
            }
            finally
            {
                if (File.Exists(serverfile))
                {
                    File.Delete(serverfile);
                }
            }
        }
    }

    protected void btn_results1_Click(object sender, EventArgs e)
    {
        String yrmo = ddlYrmo1.SelectedValue.ToString();

        if (Page.IsValid)
        {
            if (txtPrevYRMO1.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                div1_error.Visible = false;
                viewresult1(yrmo);
            }
            catch (Exception ex)
            {
                resultDiv1.Visible = false;
                div1_error.Visible = true;
                lbl_error1.Text = "Error - " + ex.Message;
            }
        }
    }

    protected void btn_results2_Click(object sender, EventArgs e)
    {
        String yrmo = ddlYrmo2.SelectedValue.ToString();

        if (Page.IsValid)
        {
            if (txtPrevYRMO2.Visible)
            {
                lbl_error2.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                div2_error.Visible = false;
                viewresult2(yrmo);
            }
            catch (Exception ex)
            {
                resultDiv2.Visible = false;
                div2_error.Visible = true;
                lbl_error2.Text = "Error - " + ex.Message;
            }
        }
    }

    protected void btn_reconAgn1_Click(object sender, EventArgs e)
    {
        resultDiv1.Visible = false;
        div1_error.Visible = false;
        div1_alert.Visible = false;
        String yrmo = ddlYrmo1.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();

        if (Page.IsValid)
        {
            if (txtPrevYRMO1.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                MultiView3.SetActiveView(view_main1);
                robj.ReconDelete(yrmo);                
            }
            catch (Exception ex)
            {
                resultDiv1.Visible = false;
                div1_error.Visible = true;
                lbl_error1.Text = "Error - " + ex.Message;
            }
        }  
    }

    protected void btn_reconAgn2_Click(object sender, EventArgs e)
    {
        resultDiv2.Visible = false;
        div2_error.Visible = false;
        div2_alert.Visible = false;
        String yrmo = ddlYrmo2.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();

        if (Page.IsValid)
        {
            if (txtPrevYRMO2.Visible)
            {
                lbl_error2.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            try
            {
                MultiView1.SetActiveView(view_main2);
                robj.ReconDelete(yrmo);
            }
            catch (Exception ex)
            {
                resultDiv2.Visible = false;
                div2_error.Visible = true;
                lbl_error2.Text = "Error - " + ex.Message;
            }
        }
    }

    void ManualReconcile()
    {
        Decimal begBal, endBal, totContr, totDiv, termPay, withdraw, manAdjAmt;
        String begBalStr, endBalStr, totContrStr, totDivStr, termPayStr, withdrawStr, manAdjAmtStr, manAdjNotesStr;
        HRAReconSOFO sobj = new HRAReconSOFO();
        string yrmo = ddlYrmo2.SelectedItem.Text;

        begBalStr = txtBegBal.Text;
        endBalStr = txtEndBal.Text;
        totContrStr = txtTotContr.Text;
        totDivStr = txtTotDiv.Text;
        termPayStr = txtTermPay.Text;
        withdrawStr = txtWithDraw.Text;
        manAdjAmtStr = tbx_manAdjAmt.Text;
        manAdjNotesStr = tbx_manAdjNotes.Text;

        if (manAdjAmtStr == null || manAdjAmtStr == String.Empty || manAdjAmtStr.Equals("0.00"))
            manAdjAmt = 0;
        else
        {
            if (manAdjNotesStr == null || manAdjNotesStr == String.Empty)
            {
                throw new Exception("There is a Manual Adjustment Amount - " + manAdjAmtStr + " but Notes for it are not entered!");
            }
            manAdjAmt = Convert.ToDecimal(manAdjAmtStr);
        }

        if(begBalStr == null || begBalStr == String.Empty) 
            begBal = 0;
        else 
             begBal = Decimal.Parse(begBalStr, NumberStyles.Currency);

       if(endBalStr == null || endBalStr == String.Empty) 
            endBal = 0;
        else
            endBal = Decimal.Parse(endBalStr, NumberStyles.Currency);

        if(totContrStr == null || totContrStr == String.Empty) 
            totContr = 0;
        else
            totContr = Decimal.Parse(totContrStr, NumberStyles.Currency);

        if(totDivStr == null || totDivStr == String.Empty) 
            totDiv = 0;
        else
            totDiv = Decimal.Parse(totDivStr, NumberStyles.Currency);

        if(termPayStr == null || termPayStr == String.Empty) 
            termPay = 0;
        else
            termPay = Decimal.Parse(termPayStr, NumberStyles.Currency);

        if(withdrawStr == null || withdrawStr == String.Empty) 
            withdraw = 0;
        else
            withdraw = Decimal.Parse(withdrawStr, NumberStyles.Currency);       


         sobj.ReconcileSOFO(yrmo, begBal, endBal, totContr, totDiv, termPay, withdraw, manAdjAmt, manAdjNotesStr);
    }

    void viewresult1(String yrmo)
    {
        resultDiv1.Visible = true;        
        MultiView3.SetActiveView(view_result1);
        lbl_result1.Text = "Statement and all Putnam values are in balance for " + yrmo;
        lbl_result1.Text += "<br/>Saved / Printed report(s) that are selected to automatically save/print."; 
        bindResult1("summary", yrmo);
    }

    void viewresult2(String yrmo)
    {
        resultDiv2.Visible = true;
        MultiView1.SetActiveView(view_result2);
        lbl_result2.Text = "Statement and all Putnam values are in balance for " + yrmo;
        lbl_result2.Text += "<br/>Saved / Printed report(s) that are selected to automatically save/print.";
        bindResult2("summary", yrmo);
    }

    void errormessage1(String errMsg)
    {
        if (!ClientScript.IsClientScriptBlockRegistered("_errorMessage"))
            ClientScript.RegisterClientScriptBlock(typeof(Page), "_errorMessage", "alert('" + errMsg + "') ;", true);
        lbl_error1.Text = errMsg.Replace("\\n", "<br/>"); 
        MultiView3.SetActiveView(view_main1);
    }

    void errormessage2(String errMsg)
    {
        if (!ClientScript.IsClientScriptBlockRegistered("_errorMessage"))
            ClientScript.RegisterClientScriptBlock(typeof(Page), "_errorMessage", "alert('" + errMsg + "') ;", true);
        lbl_error2.Text = errMsg.Replace("\\n", "<br/>");
        MultiView1.SetActiveView(view_main2);
    }

    void initializePageControls()
    {
        txtPrevYRMO1.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO1.ClientID + "', '" + ddlYrmo1.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
        txtPrevYRMO2.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO2.ClientID + "', '" + ddlYrmo2.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
        txtBegBal.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtBegBal.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();}return true;");
        txtTotContr.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtTotContr.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();}return true;");
        txtTotDiv.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtTotDiv.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();}return true;");
        txtTermPay.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtTermPay.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();}return true;");
        txtWithDraw.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtWithDraw.Attributes.Add("onClick", "if(this.value == '0.00'){this.focus();this.select();}return true;");
        txtEndBal.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        txtEndBal.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();}return true;");
        tbx_manAdjAmt.Attributes.Add("onBlur", "if(this.value.length == 0) this.value = '0.00'; return true;");
        tbx_manAdjAmt.Attributes.Add("onClick", "if(this.value == '0.00') {this.focus();this.select();} return true;");
    }

    protected void btn_genRpt1_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo1.SelectedItem.Text;
        div1_error.Visible = false; 

        try
        {
            HRA_Results.GenerateReport("SOFO", yrmo);            
        }
        catch (Exception ex)
        {
            div1_error.Visible = true;
            lbl_error1.Text = "Error in generating HRA Putnam Balance Report<br />" + ex.Message;
        }
    }

    protected void btn_genRpt2_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo2.SelectedItem.Text;
        div2_error.Visible = false;

        try
        {
            HRA_Results.GenerateReport("SOFO", yrmo);
        }
        catch (Exception ex)
        {
            div2_error.Visible = true;
            lbl_error2.Text = "Error in generating HRA Putnam Balance Report<br />" + ex.Message;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        div2_error.Visible = false;
        ddlYrmo2.Items.Clear();
        ddlYrmoList();
        txtBegBal.Text = "";
        txtTotContr.Text = "";
        txtTotDiv.Text = "";
        txtTermPay.Text = "";
        txtWithDraw.Text = "";
        txtEndBal.Text = "";
        tbx_manAdjAmt.Text = "";
        tbx_manAdjNotes.Text = "";
    }

    protected void checkPastRecon1()
    {
        string _yrmo = ddlYrmo1.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();

        if (robj.pastReconcile(_yrmo))
        {
            MultiView3.SetActiveView(view_reconAgn1);
            lbl_reconAgn1.Text = "Reconciled already for year-month (YRMO): " + _yrmo + ".<br />Press 'View Results' to view results or Press 'Re-Reconcile' to reconcile again.";
        }
        else
        {
            MultiView3.SetActiveView(view_main1);
            autoImport();
        }
    }

    protected void checkPastRecon2()
    {
        string _yrmo = ddlYrmo2.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();

        if (robj.pastReconcile(_yrmo))
        {
            MultiView1.SetActiveView(view_reconAgn2);
            lbl_reconAgn2.Text = "Reconciled already for year-month (YRMO): " + _yrmo + ".<br />Press 'View Results' to view results or Press 'Re-Reconcile' to reconcile again.";
        }
        else
        {
            MultiView1.SetActiveView(view_main2);
        }
    }

    protected void bindResult1(string _src, string yrmo)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        HRASofoDAL sobj = new HRASofoDAL();       
        
        switch (_src)
        {
            case "summary":
                ds = sobj.getSOFOReconData(yrmo);
                grdvSum1.DataSource = ds;
                grdvSum1.DataBind();
                break;                       
        }
    }

    protected void bindResult2(string _src, string yrmo)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        HRASofoDAL sobj = new HRASofoDAL();

        switch (_src)
        {
            case "summary":
                ds = sobj.getSOFOReconData(yrmo);
                grdvSum2.DataSource = ds;
                grdvSum2.DataBind();
                break;
        }
    }

    void AutoReconcile(string strFilePath1)
    {
        HRAReconSOFO sobj = new HRAReconSOFO();
        string _yrmo = ddlYrmo1.SelectedItem.Text;
        Decimal begBal, endBal, totContr, totDiv, termPay, withdraw, manAdjAmt;
        DataSet ds = new DataSet(); ds.Clear();
        int[] cols = { 6, 7, 11, 21, 25, 36 };
        String _tableName = "SOFOTable";
        String manAdjAmtStr, manAdjNotesStr;

        manAdjAmtStr = tbx_adjAmt1.Text;
        manAdjNotesStr = tbx_adjNotes1.Text;

        if (manAdjAmtStr == null || manAdjAmtStr == String.Empty)
            manAdjAmt = 0;
        else
        {
            if (manAdjNotesStr == null || manAdjNotesStr == String.Empty)
            {
                throw new Exception("There is a Manual Adjustment Amount - " + manAdjAmtStr + " but Notes for it are not entered!");
            }
            manAdjAmt = Decimal.Parse(manAdjAmtStr, NumberStyles.Currency);
        }

        ds = HRATextImport.getTextFileData(strFilePath1, _tableName, ',');

        begBal = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[0]]);
        totContr = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[1]]);
        totDiv = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[2]]);
        termPay = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[3]]);
        withdraw = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[4]]);
        endBal = Convert.ToDecimal(ds.Tables[_tableName].Rows[0][cols[5]]);
        
        sobj.ReconcileSOFO(_yrmo, begBal, endBal, totContr, totDiv, termPay, withdraw, manAdjAmt, manAdjNotesStr);          
    }

    void alertMsg1()
    {
        string _yrmo = ddlYrmo1.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();
        string _alertmsg = robj.HRAReconBal(_yrmo);

        if (!_alertmsg.Equals(String.Empty))
        {
            div1_alert.Visible = true;
            lbl1_alert.Text = _alertmsg;
        }
        else
        {
            div1_alert.Visible = false;
        }
    }

    void alertMsg2()
    {
        string _yrmo = ddlYrmo2.SelectedValue;
        HRASofoDAL robj = new HRASofoDAL();
        string _alertmsg = robj.HRAReconBal(_yrmo);

        if (!_alertmsg.Equals(String.Empty))
        {
            div2_alert.Visible = true;
            lbl2_alert.Text = _alertmsg;
        }
        else
        {
            div2_alert.Visible = false;
        }
    }
}

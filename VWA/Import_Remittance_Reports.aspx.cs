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
using System.IO;
using EBA.Desktop;
using EBA.Desktop.VWA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class VWA_Import_Remittance_Reports : System.Web.UI.Page
{
    private const string _category = "Import";
    private const string source = "Remit";
    private const string source_OC = "RemitOC";
    private const string source_Cigna = "RemitCigna";
    private const string source_UHC = "RemitUHC";
    private const string source_Disab = "RemitDisab";
    private const string source_Anth = "RemitAnth";
    int _counter = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M400", "M401");            
            ddlYrmoList();
            txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
            checkPastImport();
            UpdateImportType();
        }
    }

    protected void UpdateImportType()
    {
        if (VWAImportDAL.CheckAutoImport(_category, source))
        {
            SetAutoImport();
        }
        else
        {
            SetManImport();
        }
    }

    protected void ddlYrmoList()
    {
        string prevYRMO;
        ddlYrmo.Items.Add("");
        for (int i = 0; i < 6; i++)
        {
            prevYRMO = VWA.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
       ddlYrmo.Items.Add("New Yrmo");
       ddlYrmo.SelectedIndex = 0;
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        ErrMsgDiv1.Visible = false;
        ErrMsgDiv2.Visible = false;
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
                checkPastImport();
                UpdateImportType();
            }
        }
        catch (Exception ex)
        {
            ErrMsgDiv1.Visible = true;
            lbl_error1.Text = "Error: " + ex.Message;
        }
    }

    protected void txtPrevYRMO_textChanged(object sender, EventArgs e)
    {
        ErrMsgDiv1.Visible = false;
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
            checkPastImport();
            UpdateImportType();
        }
        catch (Exception ex)
        {
            ErrMsgDiv1.Visible = true;
            lbl_error1.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        ErrMsgDiv1.Visible = false;
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        lblPrevYrmo.Visible = true;
        txtPrevYRMO.Text = "";
        DivNewYRMO.Visible = false;
        checkPastImport();
        UpdateImportType();
    }

    protected void checkPastImport()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;

        if (VWAImportDAL.PastImport(source_OC, _yrmo)) { lbl_suc_OC.Visible = true; }
        else { lbl_suc_OC.Visible = false; }
        if (VWAImportDAL.PastImport(source_Cigna, _yrmo)) { lbl_suc_Cigna.Visible = true; }
        else { lbl_suc_Cigna.Visible = false; }
        if (VWAImportDAL.PastImport(source_UHC, _yrmo)) { lbl_suc_UHC.Visible = true; }
        else { lbl_suc_UHC.Visible = false; }
        if (VWAImportDAL.PastImport(source_Disab, _yrmo)) { lbl_suc_Disab.Visible = true; }
        else { lbl_suc_Disab.Visible = false; }
        if (VWAImportDAL.PastImport(source_Anth, _yrmo)) { lbl_suc_Anth.Visible = true; }
        else { lbl_suc_Anth.Visible = false; }

        if (lbl_suc_OC.Visible || lbl_suc_Cigna.Visible || lbl_suc_UHC.Visible || lbl_suc_Disab.Visible || lbl_suc_Anth.Visible)
        {
            View_Result();
        }
    }

    protected void btn_manImport_Click(object sender, EventArgs e)
    {
        ErrMsgDiv1.Visible = false;
        ErrMsgDiv2.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;
        string fn1, fn2, fn3, fn4, fn5;
        string strFilePath1, strFilePath2, strFilePath3, strFilePath4, strFilePath5;
        List<string> filepaths = new List<string>();
        List<string> sources = new List<string>();
        string wiredt = tbx_manWiredt.Text;

        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            if (yrmo.Length == 0 || yrmo.Length != 6)
            {
                Alert.Show("Please Enter YRMO in format yyyymm");
                return;
            }
            if (tbx_manWiredt.Text == "" || tbx_manWiredt.Text.Length==0)
            {
                Alert.Show("Please Enter Wire Date in the format yyyymm");
                return;
            }

            strFilePath1 = "";
            strFilePath2 = "";
            strFilePath3 = "";
            strFilePath4 = "";
            strFilePath5 = "";

            if (FileUpload1_OC.GotFile || FileUpload1_Cigna.GotFile || FileUpload1_UHC.GotFile || FileUpload1_Disab.GotFile || FileUpload1_Anth.GotFile || wiredt != String.Empty)
            {
                try
                {
                    if (FileUpload1_OC.GotFile)
                    {
                        fn1 = System.IO.Path.GetFileName(FileUpload1_OC.FilePost.FileName);
                        strFilePath1 = Server.MapPath("~/uploads/") + fn1;
                        if (File.Exists(strFilePath1)) { File.Delete(strFilePath1); }
                        FileUpload1_OC.FilePost.SaveAs(strFilePath1);
                        filepaths.Add(strFilePath1);
                        sources.Add(source_OC);
                    }

                    if (FileUpload1_Cigna.GotFile)
                    {
                        fn2 = System.IO.Path.GetFileName(FileUpload1_Cigna.FilePost.FileName);
                        strFilePath2 = Server.MapPath("~/uploads/") + fn2;
                        if (File.Exists(strFilePath2)) { File.Delete(strFilePath2); }
                        FileUpload1_Cigna.FilePost.SaveAs(strFilePath2);
                        filepaths.Add(strFilePath2);
                        sources.Add(source_Cigna);
                    }

                    if (FileUpload1_UHC.GotFile)
                    {
                        fn3 = System.IO.Path.GetFileName(FileUpload1_UHC.FilePost.FileName);
                        strFilePath3 = Server.MapPath("~/uploads/") + fn3;
                        if (File.Exists(strFilePath3)) { File.Delete(strFilePath3); }
                        FileUpload1_UHC.FilePost.SaveAs(strFilePath3);
                        filepaths.Add(strFilePath3);
                        sources.Add(source_UHC);
                    }

                    if (FileUpload1_Disab.GotFile)
                    {
                        fn4 = System.IO.Path.GetFileName(FileUpload1_Disab.FilePost.FileName);
                        strFilePath4 = Server.MapPath("~/uploads/") + fn4;
                        if (File.Exists(strFilePath4)) { File.Delete(strFilePath4); }
                        FileUpload1_Disab.FilePost.SaveAs(strFilePath4);
                        filepaths.Add(strFilePath4);
                        sources.Add(source_Disab);
                    }

                    if (FileUpload1_Anth.GotFile)
                    {
                        fn5 = System.IO.Path.GetFileName(FileUpload1_Anth.FilePost.FileName);
                        strFilePath5 = Server.MapPath("~/uploads/") + fn5;
                        if (File.Exists(strFilePath5)) { File.Delete(strFilePath5); };
                        FileUpload1_Anth.FilePost.SaveAs(strFilePath5);
                        filepaths.Add(strFilePath5);
                        sources.Add(source_Anth);
                    }

                    if (ImportFile(filepaths, sources, wiredt))
                    {
                       View_Result();
                    }
                }
                catch (Exception ex)
                {
                    resultdiv.Visible = false;
                    VWAImportDAL.DeleteRemit(yrmo);
                    ErrMsgDiv1.Visible = true;
                    lbl_error1.Text = "Error - " + ex.Message;
                }
                finally
                {
                    if (File.Exists(strFilePath1)) 
                    { 
                        File.Delete(strFilePath1);
                        FileUpload1_OC.FilePost.InputStream.Flush(); FileUpload1_OC.FilePost.InputStream.Close(); FileUpload1_OC.FilePost.InputStream.Dispose();
                    }
                    if (File.Exists(strFilePath2)) 
                    { 
                        File.Delete(strFilePath2);
                        FileUpload1_Cigna.FilePost.InputStream.Flush(); FileUpload1_Cigna.FilePost.InputStream.Close(); FileUpload1_Cigna.FilePost.InputStream.Dispose();
                    }
                    if (File.Exists(strFilePath3)) 
                    { 
                        File.Delete(strFilePath3); 
                        FileUpload1_UHC.FilePost.InputStream.Flush(); FileUpload1_UHC.FilePost.InputStream.Close(); FileUpload1_UHC.FilePost.InputStream.Dispose();
                    }
                    if (File.Exists(strFilePath4)) 
                    {
                        File.Delete(strFilePath4);
                        FileUpload1_Disab.FilePost.InputStream.Flush(); FileUpload1_Disab.FilePost.InputStream.Close(); FileUpload1_Disab.FilePost.InputStream.Dispose();
                    }
                    if (File.Exists(strFilePath5)) 
                    { 
                        File.Delete(strFilePath5);
                        FileUpload1_Anth.FilePost.InputStream.Flush(); FileUpload1_Anth.FilePost.InputStream.Close(); FileUpload1_Anth.FilePost.InputStream.Dispose();
                    }
                }
            }
        }
    }

    protected void btn_autoImport_Click(object sender, EventArgs e)
    {
        Impersonation _imp = new Impersonation();
        string yrmo = ddlYrmo.SelectedItem.Text;
        string clientfilename1, clientfilename2, clientfilename3, clientfilename4, clientfilename5;
        string clientfile1, clientfile2, clientfile3, clientfile4, clientfile5;        
        string serverfile1, serverfile2, serverfile3, serverfile4, serverfile5;
        List<string> filepaths = new List<string>();
        List<string> sources = new List<string>();
        string wiredt = tbx_autoWiredt.Text;

        ErrMsgDiv1.Visible = false;
        ErrMsgDiv2.Visible = false;

        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error1.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            serverfile1 = "";
            serverfile2 = "";
            serverfile3 = "";
            serverfile4 = "";
            serverfile5 = "";

            try
            {
                clientfilename1 = VWA.GetInputFileName(yrmo, source_OC);
                clientfilename2 = VWA.GetInputFileName(yrmo, source_Cigna);
                clientfilename3 = VWA.GetInputFileName(yrmo, source_UHC);
                clientfilename4 = VWA.GetInputFileName(yrmo, source_Disab);
                clientfilename5 = VWA.GetInputFileName(yrmo, source_Anth);

                serverfile1 = Server.MapPath("~/uploads/") +clientfilename1;
                serverfile2 = Server.MapPath("~/uploads/") + clientfilename2;
                serverfile3 = Server.MapPath("~/uploads/") + clientfilename3;
                serverfile4 = Server.MapPath("~/uploads/") + clientfilename4;
                serverfile5 = Server.MapPath("~/uploads/") + clientfilename5;

                if (_imp.impersonateValidUser(Session["uid"].ToString(), "CORP", EncryptDecrypt.Decrypt(Session["pwd"].ToString())))
                {
                    clientfile1 = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename1;
                    clientfile2 = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename2;
                    clientfile3 = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename3;
                    clientfile4 = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename4;
                    clientfile5 = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename5;

                    if (File.Exists(clientfile1) || File.Exists(clientfile2) || File.Exists(clientfile3) || File.Exists(clientfile4) || File.Exists(clientfile5) || wiredt != String.Empty)
                    {
                        if (File.Exists(clientfile1))
                        {
                            File.Copy(clientfile1, serverfile1);
                            filepaths.Add(serverfile1);
                            sources.Add(source_OC);
                        }
                        if (File.Exists(clientfile2))
                        {
                            File.Copy(clientfile2, serverfile2);
                            filepaths.Add(serverfile2);
                            sources.Add(source_Cigna);
                        }
                        if (File.Exists(clientfile3))
                        {
                            File.Copy(clientfile3, serverfile3);
                            filepaths.Add(serverfile3);
                            sources.Add(source_UHC);
                        }
                        if (File.Exists(clientfile4))
                        {
                            File.Copy(clientfile4, serverfile4);
                            filepaths.Add(serverfile4);
                            sources.Add(source_Disab);
                        }
                        if (File.Exists(clientfile5))
                        {
                            File.Copy(clientfile5, serverfile5);
                            filepaths.Add(serverfile5);
                            sources.Add(source_Anth);
                        }

                        _imp.undoImpersonation();
                    }
                    else
                    {
                        throw new Exception("Error in accessing network location");
                    }

                    if (ImportFile(filepaths, sources, wiredt))
                    {                        
                        View_Result();
                    }
                    else
                    {
                        throw new Exception("Unable to parse reports.<br/>Please check reports type and format.");
                    }
                }
            }
            catch (Exception ex)
            {
                resultdiv.Visible = false;
                VWAImportDAL.DeleteRemit(yrmo);
                ErrMsgDiv1.Visible = true;
                lbl_error1.Text = "Error - " + ex.Message;
            }
            finally
            {
                if (File.Exists(serverfile1)) { File.Delete(serverfile1); };
                if (File.Exists(serverfile2)) { File.Delete(serverfile2); };
                if (File.Exists(serverfile3)) { File.Delete(serverfile3); };
                if (File.Exists(serverfile4)) { File.Delete(serverfile4); };
                if (File.Exists(serverfile5)) { File.Delete(serverfile5); };
            }
        }
    }

    Boolean ImportFile(List<string> filepaths, List<string> sources, string wiredt)
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        bool importStat = false;
        VWA_ExcelImport tObj = new VWA_ExcelImport();
        DataSet ds = new DataSet(); ds.Clear();

        int _totalcounter = 0;

        for(int i=0; i < filepaths.Count; i++)
        {
            VWAImportDAL.Rollback(sources[i], _yrmo);
            ds = tObj.importRemittance(filepaths[i], sources[i]);
            _counter = tObj.parseRemittance(ds, _yrmo, sources[i]);
            VWAImportDAL.insertImportStatus(_yrmo, sources[i]);
            SetImportStatus(sources[i]);
            _totalcounter += _counter;
        }

        VWAImportDAL.updateRemitWireDt(wiredt, _yrmo);

        VWA_Results.SavePrintFiles(source, _yrmo);

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "VWA Imports", "VWA_remit", "Remittance Reports", _yrmo, _totalcounter);
        importStat = true;

        return importStat;
    }

    void SetImportStatus(string _source)
    {
        switch(_source)
        {
            case "RemitOC":
                lbl_suc_OC.Visible = true;
                break;
            case "RemitCigna":
                lbl_suc_Cigna.Visible = true;
                break;
            case "RemitUHC":
                lbl_suc_UHC.Visible = true;
                break;
            case "RemitDisab":
                lbl_suc_Disab.Visible = true;
                break;
            case "RemitAnth":
                lbl_suc_Anth.Visible = true;
                break;
        }
    }

    protected void lnk_genDiscpRpt_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        ErrMsgDiv2.Visible = false;

        try
        {
            VWA_Results.GenerateReport("RemitDisp", yrmo);
        }
        catch (Exception ex)
        {
            ErrMsgDiv2.Visible = true;
            lbl_error2.Text = "Error in generating VWA Remittance Discrepancy Report<br />" + ex.Message;
        }
    }

    protected void lnk_genInputRpt_OnClick(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        ErrMsgDiv2.Visible = false;

        try
        {
            VWA_Results.GenerateReport("RemitInput", yrmo);
        }
        catch (Exception ex)
        {
            ErrMsgDiv2.Visible = true;
            lbl_error2.Text = "Error in generating VWA Remittance Summary & Detail Report<br />" + ex.Message;
        }
    }    

    protected void cbx_autoImport_OnCheckedChanged(object sender, EventArgs e)
    {
        SetAutoImport();
    }

    protected void cbx_manImport_OnCheckedChanged(object sender, EventArgs e)
    {
        SetManImport();
    }

    void SetAutoImport()
    {
        upload_auto.Visible = true;
        upload_manually.Visible = false;

        if (VWAImportDAL.GetClientFilePath(_category, source).Trim() == String.Empty)
        {
            lbl_clientfile_OC.Text = "";
            lbl_clientfile_Cigna.Text = "";
            lbl_clientfile_UHC.Text = "";
            lbl_clientfile_Disab.Text = "";
            lbl_clientfile_Anth.Text = "";
        }
        else
        {
            lbl_clientfile_OC.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source_OC);
            lbl_clientfile_Cigna.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source_Cigna);
            lbl_clientfile_UHC.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source_UHC);
            lbl_clientfile_Disab.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source_Disab);
            lbl_clientfile_Anth.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source_Anth);
        }
        cbx_autoImport.Checked = false;
    }

    void SetManImport()
    {
        upload_auto.Visible = false;
        upload_manually.Visible = true;

        lbl_clientfile_OC.Text = "";
        lbl_clientfile_Cigna.Text = "";
        lbl_clientfile_UHC.Text = "";
        lbl_clientfile_Disab.Text = "";
        lbl_clientfile_Anth.Text = "";
        cbx_manImport.Checked = false;
    }

    void View_Result()
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        SetNoRecsMsg(yrmo);
        resultdiv.Visible = true;
    }

    void SetNoRecsMsg(string yrmo)
    {
        if (VWA_ExportDAL.HasRecords(yrmo, "RemitDisp"))
            lbl_discp.Visible = false;
        else
            lbl_discp.Visible = true;

        if (VWA_ExportDAL.HasRecords(yrmo, "RemitInput"))
            lbl_input.Visible = false;
        else
            lbl_input.Visible = true;
    }
}

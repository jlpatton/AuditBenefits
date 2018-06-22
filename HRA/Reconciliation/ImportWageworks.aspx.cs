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
using EBA.Desktop.HRA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.Text.RegularExpressions;


public partial class HRA_ImportWageworks : System.Web.UI.Page
{
    private const string _category = "Reconciliation";
    private const string source = "Wageworks";
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
            AdminBLL.InRole(uname, "M200", "M202");
            ddlYrmoList();
            checkPastImport();
            txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");           
        }
    }

    protected void autoImport()
    {
        if (HRAImportDAL.CheckAutoImport(_category, source))
        {
            upload_auto.Visible = true;
            upload_manually.Visible = false;
            lbl_clientfile.Text = HRAImportDAL.GetClientFilePath(_category, source) + "\\" + HRA.GetInputFileName(ddlYrmo.SelectedItem.Value, source);
            lbl_clientfile.Text = lbl_clientfile.Text + "&nbsp;&nbsp;(<i>or</i>)<br/>" + lbl_clientfile.Text.Replace(".xls", ".txt");
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
        HRA iobj = new HRA();
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = iobj.getPrevYRMO(i);
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
                lblPrevYrmo.Visible = false;
                ddlYrmo.Visible = false;
                DivNewYRMO.Visible = true;
            }
            else
            {
                checkPastImport();
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

            lblPrevYrmo.Visible = true;
            ddlYrmo.Visible = true;
            txtPrevYRMO.Text = "";
            DivNewYRMO.Visible = false;
            checkPastImport();
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        lblPrevYrmo.Visible = true;
        txtPrevYRMO.Text = "";
        DivNewYRMO.Visible = false;
        checkPastImport();
    }

    protected void checkPastImport()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        lbl_error.Text = "";
        HRAImportDAL iobj = new HRAImportDAL();

        if (iobj.PastImport(source, _yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo + "<br />Do you want to re-import the file?";
        }
        else
        {
            MultiView1.SetActiveView(view_main);
            autoImport();
        }
    }

    protected void btn_manImport_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_Wageworks_" + yrmo + ".txt";

        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
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
                    if (ImportFile(strFilePath1))
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(logFilePath)) { File.Delete(logFilePath); }
                    resultDiv.Visible = false;
                    HRAImportDAL iObj = new HRAImportDAL();
                    iObj.Rollback(source, yrmo);
                    lbl_error.Text = "Error - " + ex.Message;
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
        HRAImportDAL iobj = new HRAImportDAL();
        Impersonation _imp = new Impersonation();
        string yrmo = ddlYrmo.SelectedItem.Text;
        string clientfilename, clientfile, serverPath;
        string serverfile = "";
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_Wageworks_" + yrmo + ".txt";

        if (Page.IsValid)
        {
            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }

            try
            {                
                serverPath = FilePaths.getFilePath("Uploads").Replace("\\\\", "\\");
                clientfilename = HRA.GetInputFileName(yrmo, source);
                serverfile = serverPath + clientfilename;

                if (_imp.impersonateValidUser(Session["uid"].ToString(), "CORP", EncryptDecrypt.Decrypt(Session["pwd"].ToString())))
                {
                    clientfile = HRAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename;
                    if (!File.Exists(clientfile))
                    {
                        clientfile = clientfile.Replace(".xls", ".txt");
                        serverfile = serverfile.Replace(".xls", ".txt");
                    }
                    File.Copy(clientfile, serverfile);
                    _imp.undoImpersonation();
                }
                else
                {
                    throw new Exception("Error in accessing network location");
                } 

                if (!(File.Exists(serverfile)))
                    throw new Exception("Unable to import file from specified location.<br/>Please check if file exists and you are logged in to the network.");

                if (ImportFile(serverfile))
                {
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
                }
                else
                    throw new Exception("Unable to parse file.<br/>Please check file type and format.");
            }
            catch (Exception ex)
            {
                if (File.Exists(logFilePath)) { File.Delete(logFilePath); }
                resultDiv.Visible = false;
                iobj.Rollback(source, yrmo);
                lbl_error.Text = "Error - " + ex.Message;
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

    protected void btn_reimport_Click(object sender, EventArgs e)
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
                HRAImportDAL iObj = new HRAImportDAL();
                iObj.Rollback(source, yrmo);
                MultiView1.SetActiveView(view_main);
                autoImport();
            }
            catch (Exception ex)
            {
                lbl_error.Text = "Error in re-importing file.<br />" + ex.Message;
            }
        }
    }

    protected void lnk_genLogRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string _yrmo = ddlYrmo.SelectedItem.Text;
        string logFileName = "Exceptions_Wageworks_" + _yrmo + ".txt";
        string logFilePath = Server.MapPath("~/uploads/") + logFileName;
        
        try
        {
            Response.Clear();
            Response.Charset = "";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/plain";
            Response.AddHeader("content-disposition", "attachment; filename=" + logFileName);
            Response.TransmitFile(logFilePath);
            Response.End();
        }
        catch (Exception ex)
        {
            resultDiv.Visible = false;
            lbl_error.Text = "Error in generating Exception Report<br />" + ex.Message;
        }
    }
    
    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_Wageworks_" + _yrmo + ".txt";
        bool importStat = false;
        HRAImportDAL iObj = new HRAImportDAL();
        HRAExcelImport tObj = new HRAExcelImport();
        HRAParseData pObj = new HRAParseData();
        DataSet ds = new DataSet();

        if (String.Compare((strFilePath1.Substring(strFilePath1.Length - 3)), "txt") == 0)
            ds = HRATextImport.getTextFileData(strFilePath1, "WageworksTable", '\t');
        else
            ds = tObj.getExcelData(strFilePath1, "WageworksTable");

        tObj.ConfirmWageworkYRMO(_yrmo, ds, "WageworksTable");

        if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

        iObj.Rollback(source, _yrmo);
        
        _counter = pObj.parseWageworks(ds, _yrmo);

        if (File.Exists(logFilePath)) { resultDiv.Visible = true; }

        importStat = true;

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Reconciliation", "HTH_HMO_Billing", "Wageworks Import", _yrmo, _counter);
        
        return importStat;
    }
}

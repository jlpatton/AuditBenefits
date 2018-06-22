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

public partial class HRA_Administrative_Bill_Validation_Import_Wageworks_invoice : System.Web.UI.Page
{
    private const string source = "wgwk_invoice";
    private const string _category = "Admin Bill Validation";
    int _counter = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            ddlYrmoList();
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M205"); 
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
        string _yrmo = ddlYrmo.SelectedItem.Text.ToString();
        HRAImportDAL iobj = new HRAImportDAL();

        if (iobj.PastImport(source, _yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo + ".<br />Do you want to re-import the file?";
        }
        else
        {
            MultiView1.SetActiveView(view_main);
            autoImport();
        }
    }

    protected void btn_manImport_Click(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        HRAAdminBill hobj = new HRAAdminBill();
        HRAImportDAL iObj = new HRAImportDAL();
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_WageworksInvoice_" + yrmo + ".txt";

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

                    if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

                    iObj.Rollback(source, yrmo);
                    _counter = hobj.ImportWgwkInvoice(strFilePath1, source, yrmo);

                    if (File.Exists(logFilePath)) { resultDiv.Visible = true; }

                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "hra_PartDataInvoice", "Wageworks Invoice Import", yrmo, _counter);
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";

                }
                catch (Exception ex)
                {
                    if (File.Exists(logFilePath)) { File.Delete(logFilePath); }
                    resultDiv.Visible = false;
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
        HRAAdminBill hobj = new HRAAdminBill();
        Impersonation _imp = new Impersonation();
        HRAImportDAL iobj = new HRAImportDAL();
        string yrmo = ddlYrmo.SelectedItem.Text;
        string clientfilename, clientfile, serverPath;
        string serverfile = "";
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_WageworksInvoice_" + yrmo + ".txt";

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
                    File.Copy(clientfile, serverfile);
                    _imp.undoImpersonation();
                }
                else
                {
                    throw new Exception("Error in accessing network location");
                } 

                if (!(File.Exists(serverfile)))
                    throw new Exception("Unable to copy file from specified location.<br/>Please check if file exists and you are logged in to the network.");

                if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

                iobj.Rollback(source, yrmo);
                _counter = hobj.ImportWgwkInvoice(serverfile, source, yrmo);

                if (File.Exists(logFilePath)) { resultDiv.Visible = true; }

                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "hra_PartDataInvoice", "Wageworks Invoice Import", yrmo, _counter);
                MultiView1.SetActiveView(view_result);
                lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
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
        string yrmo = ddlYrmo.SelectedItem.Text;
        string logFileName = "Exceptions_WageworksInvoice_" + yrmo + ".txt";
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
}

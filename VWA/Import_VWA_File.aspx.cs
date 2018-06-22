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

public partial class VWA_Import_Bank_Statement : System.Web.UI.Page
{
    private const string _category = "Import";
    private const string source = "TranDtl";
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

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = VWA.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";

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

        if (VWAImportDAL.PastImport(source, _yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo;
            
        }
        else
        {
            MultiView1.SetActiveView(view_main);
            UpdateImportType();
        }
    }

    protected void btn_manImport_Click(object sender, EventArgs e)
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
                    VWAImportDAL.Rollback(source, yrmo);
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
        Impersonation _imp = new Impersonation();
        string yrmo = ddlYrmo.SelectedItem.Text;
        string clientfilename, clientfile, serverPath;
        string serverfile = "";
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
                serverPath = Server.MapPath("~/uploads/");
                clientfilename = VWA.GetInputFileName(yrmo, source);
                serverfile = serverPath + clientfilename;

                if (_imp.impersonateValidUser(Session["uid"].ToString(), "CORP", EncryptDecrypt.Decrypt(Session["pwd"].ToString())))
                {
                    clientfile = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + clientfilename;
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
                VWAImportDAL.Rollback(source, yrmo);
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
                MultiView1.SetActiveView(view_main);
                UpdateImportType();
            }
            catch (Exception ex)
            {
                lbl_error.Text = "Error in re-importing file.<br />" + ex.Message;
            }
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

    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        bool importStat = false;
        VWAImport vobj = new VWAImport();
        VWAImportDAL.Rollback(source, _yrmo);
        _counter = vobj.import_VWA_Trans(_yrmo, strFilePath1);
        VWAImportDAL.insertImportStatus(_yrmo, source);
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "VWA Imports", "VWA_Trans1", "Transaction Report", _yrmo, _counter);
        importStat = true;
        return importStat;
    }

    void SetAutoImport()
    {
        upload_auto.Visible = true;
        upload_manually.Visible = false;
        if (VWAImportDAL.GetClientFilePath(_category, source).Trim() == String.Empty)
            lbl_clientfile.Text = "";
        else
            lbl_clientfile.Text = VWAImportDAL.GetClientFilePath(_category, source) + "\\" + VWA.GetInputFileName(ddlYrmo.SelectedItem.Value, source);
        cbx_autoImport.Checked = false;
    }

    void SetManImport()
    {
        upload_auto.Visible = false;
        upload_manually.Visible = true;
        lbl_clientfile.Text = "";
        cbx_manImport.Checked = false;
    }
}

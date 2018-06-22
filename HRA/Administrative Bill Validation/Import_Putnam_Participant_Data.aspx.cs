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


public partial class HRA_Administrative_Bill_Validation_Import_Putnam_Participant_Data : System.Web.UI.Page
{
    private const string source = "ptnm_partdata";
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
            ddlQYList();
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M205"); 
            txtPrevQY.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevQY.ClientID + "', '" + ddlQY.ClientID + "', /^Q[1234]-\\d{4}$/)");
            checkPastImport();            
        }
    }

    protected void autoImport()
    {
        if (HRAImportDAL.CheckAutoImport(_category, source))
        {
            upload_auto.Visible = true;
            upload_manually.Visible = false;
            lbl_clientfile.Text = HRAImportDAL.GetClientFilePath(_category, source) + "\\" + HRA.GetInputFileName(ddlQY.SelectedItem.Value, source);
        }
        else
        {
            upload_auto.Visible = false;
            upload_manually.Visible = true;
            lbl_clientfile.Text = "";
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
        lbl_error.Text = "";
        resultDiv.Visible = false;

        try
        {
            if (ddlQY.SelectedItem.Text.Equals("New Quarter-Year"))
            {
                txtPrevQY.Text = "";
                ddlQY.Visible = false;
                lblPrevQY.Visible = false;
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

            lblPrevQY.Visible = true;
            ddlQY.Visible = true;
            txtPrevQY.Text = "";
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
        ddlQY.SelectedIndex = 0;
        ddlQY.Visible = true;
        lblPrevQY.Visible = true;
        txtPrevQY.Text = "";
        DivNewYRMO.Visible = false;
        checkPastImport();
    }

    protected void checkPastImport()
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();
        HRAImportDAL iobj = new HRAImportDAL();

        if (iobj.PastImport(source, _qy))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for Quarter-Year: " + _qy + ". Do you want to re-import the file?";
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
        HRAAdminBill hobj = new HRAAdminBill();
        string _qy = ddlQY.SelectedItem.Text;
        HRAImportDAL iObj = new HRAImportDAL();
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_PutnamParticipantData_" + _qy + ".txt";

        if (Page.IsValid)
        {
            if (txtPrevQY.Visible)
            {
                lbl_error.Text = "Enter Quarter-Year in format 'Qd-yyyy'";
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

                    iObj.Rollback(source, _qy);
                    _counter = hobj.ImportPtnmPartData(strFilePath1, source, _qy);

                    if (File.Exists(logFilePath)) { resultDiv.Visible = true; }

                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "hra_PartDataInvoice", "Putnam Participant Data Import", _qy, _counter);
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";

                }
                catch (Exception ex)
                {
                    if (File.Exists(logFilePath)) { File.Delete(logFilePath); }
                    resultDiv.Visible = false;
                    iObj.Rollback(source, _qy);
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
        string _qy = ddlQY.SelectedItem.Text;
        string clientfilename, clientfile, serverPath;
        string serverfile = "";
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "Exceptions_PutnamParticipantData_" + _qy + ".txt";

        if (Page.IsValid)
        {
            if (txtPrevQY.Visible)
            {
                lbl_error.Text = "Enter Quarter-Year in format 'Qd-yyyy'";
                return;
            }

            try
            {
                serverPath = FilePaths.getFilePath("Uploads").Replace("\\\\", "\\");
                clientfilename = HRA.GetInputFileName(_qy, source);
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

                iobj.Rollback(source, _qy);
                _counter = hobj.ImportPtnmPartData(serverfile, source, _qy);

                if (File.Exists(logFilePath)) { resultDiv.Visible = true; }

                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "hra_PartDataInvoice", "Putnam Participant Data Import", _qy, _counter);
                MultiView1.SetActiveView(view_result);
                lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
            }
            catch (Exception ex)
            {
                if (File.Exists(logFilePath)) { File.Delete(logFilePath); }
                resultDiv.Visible = false;
                iobj.Rollback(source, _qy);
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
        string _qy = ddlQY.SelectedItem.Text;
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
                HRAImportDAL iObj = new HRAImportDAL();
                iObj.Rollback(source, _qy);
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
        string _qy = ddlQY.SelectedItem.Text;
        string logFileName = "Exceptions_PutnamParticipantData_" + _qy + ".txt";
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

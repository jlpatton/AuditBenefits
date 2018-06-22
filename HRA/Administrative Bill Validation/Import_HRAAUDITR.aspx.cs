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

public partial class HRA_Administrative_Bill_Validation_Import_HRAAUDITR : System.Web.UI.Page
{
    private const string source = "HRAAUDITR";
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

        try
        {
            if (ddlQY.SelectedItem.Text.Equals("New Quarter-Year"))
            {
                txtPrevQY.Text = "";
                ddlQY.Visible = false;
                lblPrevQY.Visible = false;
                DivNewYRMO.Visible = true;
                lbl_notice.Visible = false;
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

    protected void btn_manImport_Click(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        HRAAdminBill hobj = new HRAAdminBill();
        string _qy = ddlQY.SelectedItem.Text;
        HRAImportDAL iObj = new HRAImportDAL();

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
                    _counter = hobj.ImportHRAAUDITR(strFilePath1, source, _qy);
                    Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                    Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Administrative Bill Validation", "hra_AUDITR", "HRAAUDITR Import", _qy, _counter);
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";

                }
                catch (Exception ex)
                {
                    iObj.Rollback(source, _qy);
                    lbl_error.Text = "Error - " + ex.Message;
                    lbl_notice.Visible = false;
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

    protected void checkPastImport()
    {
        string _qy = ddlQY.SelectedItem.Text.ToString();
        HRAImportDAL iobj = new HRAImportDAL();

        if (iobj.PastImport(source, _qy))
        {
            lbl_notice.Text = "Imported already for Quarter-Year: " + _qy + "!";
            lbl_notice.Visible = true;
        }
        else
        { 
            lbl_notice.Visible = false;
        }
    }
}

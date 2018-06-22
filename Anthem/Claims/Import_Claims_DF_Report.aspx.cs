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
using EBA.Desktop.Admin;
using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;


public partial class Anthem_Claims_Import_Claims_Report1 : System.Web.UI.Page
{
    int counter = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M100", "M102");
            ddlYrmoList();
            checkPastImport();
        }
    }

    protected void ddlYrmoList()
    {
        ImportDAL iobj = new ImportDAL();

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
        lbl_error1.Text = "";
        if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
        {
            txtPrevYRMO.Text = "";
            ddlYrmo.Visible = false;
            txtPrevYRMO.Visible = true;
            btnAddYrmo.Visible = true;
            btnCancelYrmo.Visible = true;
            MultiView1.SetActiveView(view_empty);
        }
        else
        {
            checkPastImport();
        }
    }

    protected void btn_ADDYrmo(object sender, EventArgs e)
    {
        int index = 0;
        if (!ddlYrmo.Items.Contains(new ListItem(txtPrevYRMO.Text.Trim())))
        {
            ddlYrmo.Items.Add(txtPrevYRMO.Text.Trim());
        }
        for (int i = 0; i < ddlYrmo.Items.Count; i++)
        {
            if (ddlYrmo.Items[i].Text == txtPrevYRMO.Text)
            {
                index = i;
            }
        }
        ddlYrmo.SelectedIndex = index;
        ddlYrmo.Visible = true;
        txtPrevYRMO.Visible = false;
        btnAddYrmo.Visible = false;
        btnCancelYrmo.Visible = false;
        checkPastImport();
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        txtPrevYRMO.Visible = false;
        btnAddYrmo.Visible = false;
        btnCancelYrmo.Visible = false;
        checkPastImport();
    }

    protected void checkPastImport()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        if (AnthImport.PastImport("CLMRPT", _yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo + ".<br />&nbsp;&nbsp;Do you want to reload the file?";
        }
        else
        {
            lbl_error.Text = "";
            lbl_error1.Text = "";
            MultiView1.SetActiveView(view_main);
        }
    }

    protected void btn_import_Click(object sender, EventArgs e)
    {
        ImportDAL iObj = new ImportDAL();
        if (Page.IsValid)
        { 
            string strFilePath1 = "";
            lbl_error.Text = "";
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
                        lbl_result.Text = "Imported successfully for year-month (YRMO): " + ddlYrmo.SelectedItem.Text;
                        //auditImport(tbx_YRMO.Text.ToString(), "imp", counter);
                    }
                }
                catch (Exception ex)
                {
                    iObj.Rollback("CLMRPT", ddlYrmo.SelectedItem.Text);
                    lbl_error.Text = "Error in importing file.<br />" + ex.Message;
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
    protected void btn_reimport_Click(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        lbl_error.Text = "";
        try
        {
            CAClaimsDAL dObj = new CAClaimsDAL();
            dObj.DeleteReconData(yrmo);
            ClaimsRecon dObj1 = new ClaimsRecon();
            dObj1.DeleteNCAReconData(yrmo);
            dObj1.deleteForced(yrmo, "Anthem");
            ImportDAL iObj = new ImportDAL();
            iObj.Rollback("CLMRPT", yrmo);            
            MultiView1.SetActiveView(view_main);
        }
        catch (Exception ex)
        {
            lbl_error1.Text = "Error in re-importing file.<br />" + ex.Message;
        }

    }

    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        bool importStat = false;        
        ImportDAL iObj = new ImportDAL();
        try
        {
            DataSet ds = new DataSet();
            ImportExcelReport tObj = new ImportExcelReport();
            ds = tObj.importClaims(strFilePath1);
            parseDataSet pObj = new parseDataSet();
            counter = pObj.parseClaims(ds, _yrmo); 
            importStat = true;
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Claims Reconciliation", "AnthBillTrans", "Anthem Claims DF Report Import", _yrmo, counter);
        }
        catch (Exception ex)
        {
            iObj.Rollback("CLMRPT", _yrmo);
            throw ex;
        }
        finally
        {
            File.Delete(strFilePath1);
        }
        return importStat;
    }
}

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
using System.Data.SqlClient;
using System.Collections.Generic;
using EBA.Desktop;
using EBA.Desktop.Admin;
using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using System.IO;


public partial class Anthem_Billing_Import_ADP_Cobra : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M100", "M101");
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
        if (AnthImport.PastImport("ADP", _yrmo))
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
        string yrmo = ddlYrmo.Text.ToString();
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

                MatchYRMO(strFilePath1);
                if (importfile(strFilePath1))
                {
                    MultiView1.SetActiveView(view_result);
                    //auditImport(yrmo.ToString(), "imp");
                    lbl_result.Text = "Imported successfully for year-month (YRMO): " + yrmo;
                }

            }
            catch (Exception ex)
            {
                ImportDAL iObj = new ImportDAL();
                iObj.Rollback("ADP", yrmo);
                lbl_error.Text = "Error in importing file.<br />" + ex.Message;
            }
            finally
            {
                FileUpload1.FilePost.InputStream.Flush();
                FileUpload1.FilePost.InputStream.Close();
                FileUpload1.FilePost.InputStream.Dispose();
            }
        }
    }

    protected void btn_reimport_Click(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text.ToString();
        try
        {
            ImportDAL iObj = new ImportDAL();
            iObj.Rollback("ADP", yrmo);
            ReconDAL.pastReconcileDelete(yrmo, "DOM");
            MultiView1.SetActiveView(view_main);
        }
        catch (Exception ex)
        {
            lbl_error1.Text = "Error in re-importing file.<br />" + ex.Message;
        }     
    }    
   
    protected bool importfile(string strFilePath1)
    {       
        bool status = false;
        string yrmo = ddlYrmo.Text;
        //ImportText adpImport = new ImportText();
     
        try
        {            
            //adpImport.ParseADP(strFilePath1); 
            ImportCobra.importADPDetail(yrmo, strFilePath1);
            status = true;            
            ImportDAL iobj = new ImportDAL();
            int _cnt = iobj.getImportCount("ADP", yrmo);
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "ANTHEM", "Anthem Billing Reconciliation", "ADP_Details", "ADP Cobra Import", yrmo, _cnt);
        }
        catch (Exception e)
        {
            ImportDAL iObj = new ImportDAL();
            iObj.Rollback("ADP", yrmo);
            throw e;
        }
        finally
        {
            File.Delete(strFilePath1);
        }
        return status;
    }

    protected void MatchYRMO(string strFilePath1)
    {
        String yrmo_input = ddlYrmo.Text;      
        ImportText yrmoObj = new ImportText();
        String yrmo_file = yrmoObj.matchYrmoTxt(strFilePath1, "ADP");
        if (yrmo_file == "")
        {
            throw(new Exception("Cannot find Year Month in the file.<br />Please check the format"));            
        }
        if (String.Compare(yrmo_file, yrmo_input) != 0)
        {
            throw (new Exception("YRMO entered does not match with the file"));
        }
        
    }
    
}

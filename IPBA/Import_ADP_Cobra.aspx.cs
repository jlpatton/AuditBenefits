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
using EBA.Desktop.Admin;
using EBA.Desktop.IPBA;
using EBA.Desktop.Audit;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public partial class IPBA_Import_ADP_Cobra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M300", "M301");
            ddlYrmoList();
            checkPastImport();
        }
    }

    protected void ddlYrmoList()
    {
        string prevYRMO;
        for (int i = 0; i < 6; i++)
        {
            prevYRMO = IPBAImport.getPrevYRMO(i);
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
        if (IPBAImportDAL.pastImport("ADP",_yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo + ".<br />&nbsp;&nbsp;Do you want to reload the file?";
        }
        else
        {
            lbl_error.Text = "";            
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
            if (Page.IsValid)
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
                    if (ImportFile(strFilePath1, yrmo))
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Imported successfully for year-month (YRMO): " + yrmo;
                    }
                }
                catch (Exception ex)
                {
                    IPBAImportDAL.rollback("ADP", yrmo);
                    lbl_error.Text = "Error in importing file.<br />" + ex.Message;
                }
                finally
                {
                    FileUpload1.FilePost.InputStream.Flush();
                    FileUpload1.FilePost.InputStream.Close();
                    FileUpload1.FilePost.InputStream.Dispose();
                    File.Delete(strFilePath1);
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
            IPBAImportDAL.rollback("ADP", yrmo);
            MultiView1.SetActiveView(view_main);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in re-importing file.<br />" + ex.Message;
        }
    }

    protected bool ImportFile(string strFilePath1, string yrmo)
    {
        bool _success = false;
        try
        {
            int _cnt = IPBAImport.importADPDetail(yrmo, strFilePath1);
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskINRC(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IPBA", "Insured Plans Billings & Adjustments", "HTH_HMO_Billing", "ADP Cobra Import", yrmo, _cnt);
            _success = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return _success;
    }
}

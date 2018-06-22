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
using System.Xml;
using System.Collections.Generic;
using EBA.Desktop;
using EBA.Desktop.HRA;
using System.IO;
using System.Text;
using EBA.Desktop.Admin;

public partial class HRA_LettersGenerator_Template1 : System.Web.UI.Page
{
    string gvUniqueID = String.Empty;
    int gvNewPageIndex = 0;
    int gvEditIndex = -1;
    string gvSortExpr = String.Empty;
    private string gvSortDir
    {
        get { return ViewState["SortDirection"] as string ?? "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M211");
            Session["masterId"] = "";
            clearMessages();
            resetControls();
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
        clearMessages();
        resetControls();
    }

    //This procedure returns the Sort Direction
    private string GetSortDirection()
    {
        switch (gvSortDir)
        {
            case "ASC":
                gvSortDir = "DESC";
                break;

            case "DESC":
                gvSortDir = "ASC";
                break;
        }
        return gvSortDir;
    }

    /// <summary>
    /// Creates Templates for new Letter Types (Default Schema Employee.xml)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkCreate_onclick(Object sender, EventArgs e)
    {
        clearMessages();
        string _fpath = "";
        _fpath = FilePaths.getFilePath("Schema");
        _fpath = _fpath.Replace("%20", " ");
        lnkInstall.Attributes.Add("onclick", "InstallDocSchema('" + _fpath + "','" + "EBAnamespace" + "')");
       
        int _seq = 0;
        ClientScriptManager cs = Page.ClientScript;

        try
        {
            if (!cs.IsClientScriptBlockRegistered(_seq + "CreateFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "CreateFile", "InstallDocSchema('" + _fpath + "','" + "EBAnamespace" + "')", true);
            
        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lblErrorTemplates.Text = "Error Creating File - " + ex.Message;
        }
    }

    //Query to bind the child GridView
    private SqlDataSource ChildDataSource(int LetterId)
    {
        SqlDataSource sdSrc = new SqlDataSource();
        sdSrc.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        sdSrc.SelectCommand = "SELECT [tpId],[tpVersion],[tpDate] FROM hra_Ltrs_template"
                                + " WHERE [tpTypeId] = " + LetterId;

        return sdSrc;
    }

    #region GridviewLetter    

    protected void grdvLetter_rowcommand(Object sender, GridViewCommandEventArgs e)
    {
        childDiv.Visible = false;
        Session["masterId"] = "";
        lblChildHeading.Text = "";
        if (e.CommandName.Equals("Select"))
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gr = grdvLetter.Rows[index];
            string _id = grdvLetter.DataKeys[index].Value.ToString();
            string _lblText = Server.HtmlDecode(gr.Cells[2].Text);
            Session["masterId"] = _id;
            childDiv.Visible = true;
            lblChildHeading.Text = "Versions for Template - <b>" + _lblText + "</b>";
            grdvltrTemplates.DataBind();
        }
    }
    #endregion  

    #region Insert/Update Template

    protected void btn_Add_Click(object sender, EventArgs e)
    {
        int _id = LettersGenDAL.insertNewLetterType(txtletterType.Text, txtLetterCode.Text);
        ddlLetterType.DataBind();
        resetControls();
        //audit insert        
    }

    protected void ddlLetterType_DataBound(Object sender, EventArgs e)
    {
        ddlLetterType.Items.Insert(0, new ListItem("--Select Type--","--Select Type--"));        
    }

    protected void ddlLetterType_SelectedIndexChange(Object sender, EventArgs e)
    {
        string _type = ddlLetterType.SelectedItem.Text;
        txtVersion.Text = "";
        if (!_type.Equals("--Select Type--"))
        {
            getVersion();
        }
    }   

    protected void ddlUploadType_SelectedIndexChange(Object sender, EventArgs e)
    {        
        string _type = ddlLetterType.SelectedItem.Text;
        string _uptype = ddlUploadType.SelectedItem.Text;
        txtVersion.Text = "";
        if (!_type.Equals("--Select Type--") && !_uptype.Equals("--Select--"))
        {
            getVersion();
        }
    }

    protected void getVersion()
    {
        int _typeId = Convert.ToInt32(ddlLetterType.SelectedItem.Value);
        string _type = ddlLetterType.SelectedItem.Text;
        string _uptype = ddlUploadType.SelectedItem.Text;
        txtVersion.Text = "";
        if (!_type.Equals("--Select Type--") && !_uptype.Equals("--Select--"))
        {
            int _version = LettersGenDAL.getTemplateVersion(_typeId);
            txtVersion.Text = (_version + 1).ToString();
        }
    }

    protected void btn_import_Click(object sender, EventArgs e)
    {
        string strFilePath1 = "";
        clearMessages();
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
                    if (ImportFile(strFilePath1))
                    {
                        resultDiv.Visible = true;
                        lbl_result.Text = "Imported successfully!";
                        grdvStageTemplates.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    errorDiv2.Visible = true;
                    lbl_error.Text = ex.Message;
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

    private bool ImportFile(string strUrl)
    {
        int _typeId = Convert.ToInt32(ddlLetterType.SelectedItem.Value);
        string _type = ddlLetterType.SelectedItem.Text;
        string _uptype = ddlUploadType.SelectedItem.Text;
        int _version = Convert.ToInt32(txtVersion.Text);

        
        StreamReader _reader;
        //_reader = File.OpenText(strUrl);
        _reader = new StreamReader(strUrl,Encoding.UTF8);
         string _content =  _reader.ReadToEnd();
         _content = LetterGen.replaceIllegalXMLCharacters(_content);

        try
        {
            if (!_uptype.Equals("In Validation"))
            {
                if (!LettersGenDAL.checkStageExists(_typeId, _version))
                {
                    try
                    {                                             
                                               
                        LettersGenDAL.StoreStageTemplate(_typeId, _version, _content);
                        //audit here
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception("Error while importing Template.<br />" + ex.Message));
                    }
                }
                else
                {
                    throw (new Exception("A version of the Letter type - " + _type + " and Version - "
                                            + _version + " is already in validation stage "));
                }
            }
            else
            {
                if (LettersGenDAL.checkStageExists(_typeId, _version))
                {
                    try
                    {                        
                        LettersGenDAL.UpdateStageTemplate(_typeId, _version, _content);
                        //audit here
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception("Error while importing Template.<br />" + ex.Message));
                    }
                }
                else
                {
                    throw (new Exception("A version of the Letter type - " + _type + " and Version - "
                                            + _version + " is not in validation stage. <br/> Please select Different Upload Type."));
                }
            }           
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            _reader.Close();
        }
    }

    #endregion

    protected void lnkDownload_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        try
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int _lgid = Convert.ToInt32(grdvltrTemplates.DataKeys[row.RowIndex].Value);
            string _letterCode = LettersGenDAL.getLetterTypeCode(_lgid);
            string _template = LettersGenDAL.getTemplate(_lgid);

            string _schemaUri = "EBAnamespace";
            string _schemaAlias = "EBAHRA";

            string _schemaPath = "";
            if (_letterCode.Equals("HRB1") || _letterCode.Equals("HRB2"))
            {
                _schemaPath = FilePaths.getFilePath("SchemaValid");                
            }
            else
            {
                _schemaPath = FilePaths.getFilePath("Schema");
            }
            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Template.xml";

            int _seq = 0;
            ClientScriptManager cs = Page.ClientScript;

            if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);

            if (!cs.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _template.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

            if (!cs.IsClientScriptBlockRegistered(_seq + "DownloadWord"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "DownloadWord", "DownloadTemplate('" + _file.Replace("\\", "\\\\") + "', '"
                                    + _schemaPath + "','" + _schemaAlias + "','" + _schemaUri + "');", true);

        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lblErrorTemplates.Text = "Error downloading File - " + ex.Message;
        }
    }

    protected void lnkView_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        try
        {            

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int _lgid = Convert.ToInt32(grdvltrTemplates.DataKeys[row.RowIndex].Value);

            string _template = LettersGenDAL.getTemplate(_lgid);

            //string _filepath = FilePaths.getFilePath("Uploads");
            //string _file = _filepath + "Template.xml";

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Template.xml";

            int _seq = 0;
            ClientScriptManager cs = Page.ClientScript;


            if (!cs.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveTextFile('" + _template.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

            if (!cs.IsClientScriptBlockRegistered(_seq + "ViewWord"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "ViewWord", "OpenWord('" + _file.Replace("\\", "\\\\") + "');", true);


        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lblErrorTemplates.Text = "Error viewing File - " + ex.Message;
        }
    }

    protected void lnkViewStg_onclick(Object sender, EventArgs e)
    {
        clearMessages();
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;

        string _schemaUri = "EBAnamespace";
        string _schemaAlias = "EBAHRA";

        try
        {
            int _lgid = Convert.ToInt32(grdvStageTemplates.DataKeys[row.RowIndex].Value);
            string _letterCode = LettersGenDAL.getLetterTypeCodeStage(_lgid);

            string _schemaPath = "";
            if (_letterCode.Equals("HRB1") || _letterCode.Equals("HRB2"))
            {
                _schemaPath = FilePaths.getFilePath("SchemaValid");
            }
            else
            {
                _schemaPath = FilePaths.getFilePath("Schema");
            }

            string _template = LettersGenDAL.getStageTemplate(_lgid);

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Template.xml";           

            int _seq = 0;
            ClientScriptManager cs = Page.ClientScript;


            if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);

            if (!cs.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _template.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

            if (!cs.IsClientScriptBlockRegistered(_seq + "DownloadWord"))
                cs.RegisterClientScriptBlock(typeof(Page), _seq + "DownloadWord", "DownloadTemplate('" + _file.Replace("\\", "\\\\") + "', '"
                                    + _schemaPath + "','" + _schemaAlias + "','" + _schemaUri + "');", true);

        }
        catch (Exception ex)
        {
            errorDiv3.Visible = true;
            lblErr3.Text = "Error downloading File - " + ex.Message;
        }
    }

    protected void grdvStageTemplate_rowCommand(Object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("UpdateAlt"))
            {
                GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                int index = row.RowIndex;
                CheckBox cbx1 = row.FindControl("cbxAprrovedE") as CheckBox;

                if (cbx1.Checked)
                {
                    int _id = Convert.ToInt32(grdvStageTemplates.DataKeys[index].Value);
                    LettersGenDAL.StoreTemplate(_id);
                    grdvStageTemplates.EditIndex = -1;
                    grdvLetter.DataBind();
                }

            }
        }
        catch(Exception ex)
        {

        }
    }

    void clearMessages()
    {
        lblErrorTemplates.Text = "";
        lbl_error.Text = "";
        lblErr3.Text = "";
        lbl_result.Text = "";
        errorDiv1.Visible = false;
        errorDiv2.Visible = false;
        errorDiv3.Visible = false;
        resultDiv.Visible = false;
    }

    void resetControls()
    {             
        ddlUploadType.SelectedIndex = 0;
        //ddlLetterType.SelectedIndex = 0;
        txtletterType.Text = "";
        txtLetterCode.Text = "";
        txtVersion.Text = "";
    }

}

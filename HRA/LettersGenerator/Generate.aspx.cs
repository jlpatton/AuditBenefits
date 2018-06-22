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
using EBA.Desktop.Admin;
using EBA.Desktop.HRA;
using System.IO;
using System.Text;
using System.Data.SqlClient;

public partial class HRA_LettersGenerator_Generate : System.Web.UI.Page
{
    string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }        
        if (!Page.IsPostBack)
        {            
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M212");
            ListBox1.DataBind();
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        clearMessages();
        resetControls();
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);        
    }

    protected void ddlLetterType_DataBound(Object sender, EventArgs e)
    {
        ddlLetterType.Items.Insert(0, new ListItem("--Select Type--", "--Select Type--"));
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

    protected void getVersion()
    {
        string _typeCode = ddlLetterType.SelectedItem.Value;
        string _type = ddlLetterType.SelectedItem.Text;
        
        txtVersion.Text = "";
        if (!_type.Equals("--Select Type--"))
        {
            int _version = LettersGenDAL.getTemplateVersion(_typeCode);
            if (_version != 0)
            {
                txtVersion.Text = (_version).ToString();
            }
            else
            {
                txtVersion.Text = "N/A";
            }
        }
    }

    protected void rdbfilter_OnCheck(Object sender, EventArgs e)
    {
        if (rdbFilter.SelectedValue.Equals("0"))
        {
            empTable.Visible = false;
            lnkPreview.Visible = true;
        }
        else if (rdbFilter.SelectedValue.Equals("1"))
        {
            empTable.Visible = true;
            lnkPreview.Visible = false;
        }
    }    

    protected void lnkPrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvltrPending.DataKeys[row.RowIndex].Value);

        string _empNum = grdvltrPending.Rows[row.RowIndex].Cells[1].Text;
        string _dpndssn = grdvltrPending.Rows[row.RowIndex].Cells[2].Text;
        string _dpndRel = grdvltrPending.Rows[row.RowIndex].Cells[3].Text;
        string _letterType = "";

        switch (_dpndRel)
        {
            case "SP":
                _letterType = "HRB1";
                break;
            case "CH":
                _letterType = "HRB2";
                break;
        }       

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {
            int _version = LettersGenDAL.getTemplateVersion(_letterType);
            string[] _Letter = gObj.generateBenValidationLetter(Convert.ToInt32(_empNum), _dpndssn, _letterType, _version, _lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";            
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDiv2.Visible = true;
            lbl_error1.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvltrPending.DataBind();
            grdvLtrPendingreprint.DataBind();
        }
    }

    protected void lnkrePrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvLtrPendingreprint.DataKeys[row.RowIndex].Value);

        //string _tempId = grdvLtrPendingreprint.DataKeys[row.RowIndex].Values["lgLetterId"].ToString(); 
       // string _tempId = grdvLtrPendingreprint.Rows[row.RowIndex].Cells[1].Text;
       
        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();
        
        try
        {

            string[] _Letter = gObj.reprintLetters(_lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }           

        }
        catch (Exception ex)
        {
            errorDiv2.Visible = true;
            lbl_error1.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvltrPending.DataBind();
            grdvLtrPendingreprint.DataBind();
        }
    }

    protected void lnkConfPrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvConfPen.DataKeys[row.RowIndex].Value);

        string _empNum = grdvConfPen.Rows[row.RowIndex].Cells[1].Text;        
        string _letterType =  "HRC1";                

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {
            int _version = LettersGenDAL.getTemplateVersion(_letterType);
            string[] _Letter = gObj.generateLetter(_empNum,_letterType,_version, _lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDiv4.Visible = true;
            lbl_error4.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvConfPen.DataBind();
            grdvConfPenReprint.DataBind();
        }
    }

    protected void lnkConfrePrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvConfPenReprint.DataKeys[row.RowIndex].Value);       

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {

            string[] _Letter = gObj.reprintLetters(_lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDiv4.Visible = true;
            lbl_error4.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvConfPen.DataBind();
            grdvConfPenReprint.DataBind();
        }
    }

    protected void grdvReprint_Rowcommand(Object sender, GridViewCommandEventArgs e)
    {
        divEmp.Visible = false;
        if (e.CommandName.Equals("Select"))
        {
            int _index = Convert.ToInt32(e.CommandArgument);
            int _id = Convert.ToInt32(grdvReprint.DataKeys[_index].Value);
            divEmp.Visible = true;
            bindLetterList(_id);
            
        }
    }    

    void bindLetterList(int _id)
    {
        ListBox1.Items.Clear();
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr = "SELECT [lhEmpnum],[lhgenId] FROM [hra_Ltrs_History] WHERE ([lhgenId] = @lhgenId) ORDER BY [lhEmpnum]";
        if (conn == null || conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }
        SqlCommand cmd = new SqlCommand(cmdStr,conn);
        cmd.Parameters.AddWithValue("@lhgenId", _id);
        SqlDataReader dr;
        try
        {
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = Convert.ToString(dr[0]);
                ListBox1.Items.Add(item);
            }
            dr.Close();
        }
        catch (Exception e)
        {
        }
        finally
        {
            conn.Close();
        }
    }
    protected void EndProcess()
    {
        System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcesses();
        for (int l = 0; l < p.Length; l++)
        {
            if (p[l].ProcessName.ToLower() == "winword")
            {
                p[l].Kill();
            }
        }
    }

    protected void lnkImport_Click(Object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            clearMessages(); 

            string strFilePath1 = "";
            string _empList = "";
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

                    _empList = HRATextImport.getTextFileDataNoHeader(strFilePath1, '\n');
                    txtEmpNum.Text = _empList;
                }
                catch (Exception ex)
                {
                    clearMessages();
                    errorDiv1.Visible = true;
                    lbl_error.Text = "Error importing employee file - " + ex.Message;
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

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        string _ltrType = ddlLetterType.SelectedItem.Text;
        string _ltrCode = ddlLetterType.SelectedItem.Value;
        
        clearMessages();        

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();
        try
        {            
            int _version = LettersGenDAL.getTemplateVersion(_ltrCode);
            string[] _Letter = gObj.generateLetter(_ltrCode, _version, true);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {
                if (_seq == 2)
                {
                    break;
                }
                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            clearMessages();
            errorDiv1.Visible = true;
            lbl_error.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {

        }
    }

    protected void lnkGen1_Click(object sender, EventArgs e)
    {
        string _ltrType = ddlLetterType.SelectedItem.Text;
        string _ltrCode = ddlLetterType.SelectedItem.Value;
        string _empnums = txtEmpNum.Text.Trim();
        clearMessages();
        List<string> _elList0 = new List<string>();
        List<string> _elList1 = new List<string>();
        string _exceptionEmpl = "";
       
        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();
        try
        {
            if (rdbFilter.SelectedValue.Equals("0"))
            {
                int _version = LettersGenDAL.getTemplateVersion(_ltrCode);
                string[] _Letter = gObj.generateLetter(_ltrCode, _version, false);
                ClientScriptManager cs = Page.ClientScript;

                string _filepath = "C:\\EBATemp\\";
                string _file = _filepath + "Print.xml";

                int _seq = 0;

                foreach (string _lt in _Letter)
                {

                    try
                    {
                        if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                        if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);

                      }
                    finally
                    {
                        EndProcess();
                        if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    _seq++;
                }
            }
            else if (rdbFilter.SelectedValue.Equals("1"))
            {
                if (!_empnums.Equals(""))
                {
                    _elList0 = gObj.createEmpList(_empnums);
                    foreach (string _empl in _elList0)
                    {
                        PilotData pData = new PilotData();
                        int _order = pData.getBeneficiaryOrders(Convert.ToInt32(_empl));
                        if (_order == 0)
                        {
                            //The following logic was replaced due to an exception ..3/9/2009 R.A - as per Andrea D
                            // The Ben Designation letter should print even when there are no dependants for a Pilot
                            //_exceptionEmpl += " " + _empl + ",";
                            //Added this line..where Pilots with no deps are added to the _elist to print the letter
                            _elList1.Add(_empl);
                        }
                        else
                        {
                            _elList1.Add(_empl);
                        }
                        pData = null;
                    }
                    if (!_exceptionEmpl.Equals(""))
                    {
                        infoDiv1.Visible = true;
                        _exceptionEmpl = _exceptionEmpl.Remove(_exceptionEmpl.Length - 1);
                        lblInfo.Text = "Employee number(s) <br/>" + _exceptionEmpl + "<br/> don't have any Beneficiary Listed. <br/> No Letters were generated for above Employee Numbers";
                    }
                }

                string _empListFinal = gObj.createEmplCSV(_elList1);

                int _version = LettersGenDAL.getTemplateVersion(_ltrCode);
                string[] _Letter = gObj.generateLetter(_empListFinal, _ltrCode, _version);
                ClientScriptManager cs = Page.ClientScript;

                string _filepath = "C:\\EBATemp\\";
                string _file = _filepath + "Print.xml";
                             

                int _seq = 0;

                foreach (string _lt in _Letter)
                {

                    try
                    {
                        if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                        if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                        
                    }
                    finally
                    {
                        EndProcess();
                        if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    _seq++;
                }
            }            

        }
        catch (Exception ex)
        {
            clearMessages();
            errorDiv1.Visible = true;
            lbl_error.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            Alert.Show("The Summary Plan Description (SPD) Addendum can be printed by clicking on the Generate SPD Letter");
        }
    }

    protected void lnkrepPrint_Click(object sender, EventArgs e)
    {
        GridView gv = grdvReprint;
        int index = gv.SelectedIndex;
        int _genid = Convert.ToInt32(gv.DataKeys[index].Value.ToString());

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();
        List<string> _eList = new List<string>();

        try
        {
            foreach (ListItem Li in ListBox2.Items)
            {
                _eList.Add(Li.Text);
            }

            string _empList = gObj.createEmplCSV(_eList);

            string[] _Letter = gObj.reprintLetters(_genid,_empList);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveExcel"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveExcel", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDiv2.Visible = true;
            lbl_error1.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvltrPending.DataBind();
            grdvLtrPendingreprint.DataBind();
        }
        
    }

    protected void lnkrepCancel_Click(object sender, EventArgs e)
    {
        ListBox2.Items.Clear();
        if (!ListBox1.Enabled)
        {
            ListBox1.Enabled = true;
        }
    }

    protected void lnkrepClose_Click(object sender, EventArgs e)
    {
        clearMessages();
        resetControls();
    }   

    protected void btn1_Click(object sender, EventArgs e)
    {
        ListBox2.Items.Clear();
        //foreach (ListItem Li in ListBox1.Items)
        //{
        //    ListBox2.Items.Add(Li);
        //}
        for (int i = 0; i < (ListBox1.Items.Count); i++)
        {            
            string item = ListBox1.Items[i].Text.ToString();
            ListBox2.Items.Add(item);            
        }   
        ListBox1.Enabled = false;
    }

    protected void btn2_Click(object sender, EventArgs e)
    {       
        for (int i = ListBox1.Items.Count - 1 ; i >= 0; i--)
        {
            if (ListBox1.Items[i].Selected)
            {
                ListBox1.Items[i].Selected = false;
                string item = ListBox1.Items[i].Text.ToString();
                ListBox2.Items.Add(item);                   
            }
        }        
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        int i = ListBox2.SelectedIndex;
        if (i >= 0)
        {
            string item = ListBox2.Items[i].Text.ToString();
            ListBox2.Items.Remove(item);
        }
    }

    protected void btn4_Click(object sender, EventArgs e)
    {
        ListBox2.Items.Clear();
        ListBox1.Enabled = true;
    }

    void clearMessages()
    {       
        lbl_error.Text = "";
        lbl_error1.Text = "";
        lbl_error2.Text = "";
        lbl_error4.Text = "";
        lblInfo.Text = "";
        errorDiv1.Visible = false;
        errorDiv2.Visible = false;
        errorDiv3.Visible = false;
        errorDiv4.Visible = false;
        infoDiv1.Visible = false;
    }

    void resetControls()
    {
        ddlLetterType.SelectedIndex = 0;
        txtVersion.Text = "";
        divEmp.Visible = false;
        empTable.Visible = false;
        txtEmpNum.Text = "";
        rdbFilter.ClearSelection();
        ListBox1.Items.Clear();
        ListBox2.Items.Clear();
        ListBox1.Enabled = true;
        lnkPreview.Visible = false;
    }

    protected void PrintPDF(System.IO.MemoryStream PDFData)
    {
        // Clear response content & headers
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        Response.Charset = string.Empty;
        Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
        Response.AddHeader("Content-Disposition",
            "attachment; filename=" + Title.Replace(" ", "").Replace(":", "-") + ".pdf");

        Response.OutputStream.Write(PDFData.GetBuffer(), 0, PDFData.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        //Response.End();
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        // string _pdfpath = @"/Prod/HRA/LettersGenerator/Templates/HRA_SPD_Language.pdf";
        string _pdfpath = @"/HRA/LettersGenerator/Templates/HRA_SPD_Language.pdf";

      
        //try
       
        //{
        //    LettersGenDAL.openPDF(_pdfpath);
        //} 
        //    catch (Exception ex)
        //{
        //    clearMessages();
        //    errorDiv1.Visible = true;
        //    lbl_error.Text = "Some error occured while printing letters - " + ex.Message;
        //}
        //finally
        //{
        //    Alert.Show("file path " + _pdfpath);
        //}
    }
}

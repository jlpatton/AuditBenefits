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
using System.Threading;
using System.IO;
using EBA.Desktop;
using EBA.Desktop.HRA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.Text.RegularExpressions;
using EBA.Desktop.YEB;

public partial class YEB_ImportExpatGroup : System.Web.UI.Page
{
    private const string _category = "ImportYEB";
    private const string source = "YEB_EXPAT";
    int _counter = 0;
    private string usryrmo = "";
    private string pilotind = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M600", "M602");
            if (Session["yrmo"] != null)
            {
                usryrmo = Session["yrmo"].ToString();
            }

            if (Session["pilotind"] != null)
            {
                pilotind = Session["pilotind"].ToString();
            }

            //AdminBLL.InRole(uname, "M600", "M602");
            //ddlYrmoList();
            //txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
            //checkPastImport();
        }
        lbl_error.Text = "YRMO - " + usryrmo + "      Pilot Indicator - " + pilotind;

    }
    protected void btn_import_Click(object sender, EventArgs e)
    {
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (usryrmo != "")
        {
            usryrmo = usryrmo;
        }
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "YEB_Expat_" + usryrmo + ".xls";

        if (Page.IsValid)
        {

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
                    //resultDiv.Visible = false;
                    
                    ImportYEBData.Rollback(source, usryrmo);
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

    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = Session["yrmo"].ToString(); 
        string logFilePath = Server.MapPath("~/uploads/") + "YEB_Expat_" + _yrmo + ".xls";
        bool importStat = false;
        ImportYEBData iObj = new ImportYEBData();
        HRAExcelImport tObj = new HRAExcelImport();
        DataTable dtYEBEXPAT;
        DataSet ds = new DataSet(); ds.Clear();
        
        ds = tObj.getExcelData(strFilePath1, "YEBEXPATTable");
        dtYEBEXPAT = ds.Tables["YEBEXPATTable"];
        if (dtYEBEXPAT.Rows.Count > 0)
        {
            ImportYEBData.Rollback(source, _yrmo);
            ImportYEBData.PrintExpatProgressBar();
            //http://www.eggheadcafe.com/articles/20051223.asp

            _counter = iObj.insertYEBExpatData(dtYEBEXPAT);
            Thread.Sleep(2000);
            ImportYEBData.ClearProgressBar(_counter);
            //Now update the YEbDetailtable with the matches
            //found in the YEBExpat table
            ImportYEBData.updateYEBDetailData(_yrmo, "YEBEXPAT");
                   
        }

        if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

        importStat = true;

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "YEB", "ImportSourceFile", "YEB_EXPAT", "YEBEXPAT Import", _yrmo, _counter);

        return importStat;
    }
}

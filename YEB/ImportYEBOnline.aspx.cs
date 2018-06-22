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
public partial class YEB_ImportYEBOnline : System.Web.UI.Page
{
    private const string _category = "ImportYEB";
    private const string source = "YEB_OL";
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

        if (pilotind == "PI")
        {
            MultiView1.SetActiveView(view_result);
            lbl_result.Text = "Pilot Indicator - " + pilotind + "   No PBB Online Election for Pilots was taken, skip step";
        }

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
        string logFilePath = Server.MapPath("~/uploads/") + "YEB_OL_" + usryrmo + ".xls";

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
                    ImportYEBData iObj = new ImportYEBData();
                    //iObj.Rollback(source, usryrmo);
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
    //protected void checkPastImport()
    //{
    //    string _yrmo = Session("yrmo"); //ddlYrmo.SelectedItem.Text;
    //    HRAImportDAL iobj = new HRAImportDAL();

    //    if (iobj.PastImport(source, _yrmo))
    //    {
    //        MultiView1.SetActiveView(view_reimport);
    //        lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo + "<br />Do you want to re-import the file?";
    //    }
    //    else
    //    {
    //        MultiView1.SetActiveView(view_main);
    //        //autoImport();
    //    }
    //}
    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = Session["yrmo"].ToString(); //ddlYrmo.SelectedItem.Text;
        string logFilePath = Server.MapPath("~/uploads/") + "YEB_Online_" + _yrmo + ".xls";
        bool importStat = false;
        ImportYEBData iObj = new ImportYEBData();
        HRAExcelImport tObj = new HRAExcelImport();
        DataTable dtYEBOL;
        DataSet ds = new DataSet(); ds.Clear();

        ds = tObj.getExcelData(strFilePath1, "YEBOLTable");
        dtYEBOL = ds.Tables["YEBOLTable"];
        if (dtYEBOL.Rows.Count > 0)
        {
            ImportYEBData.Rollback(source,_yrmo);
            ImportYEBData.PrintOLProgressBar();
            //http://www.eggheadcafe.com/articles/20051223.asp

            _counter = iObj.insertYEBOLData(dtYEBOL);
            Thread.Sleep(2000);
            ImportYEBData.ClearProgressBar(_counter);
            //Now update the YEbDetailtable with the matches
            //found in the YEBOnline table
            ImportYEBData.updateYEBDetailData(_yrmo, "YEBOL");
        }

        if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

        importStat = true;

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "YEB", "ImportSourceFile", "YEB_ONLINE", "YEBOL Import", _yrmo, _counter);

        return importStat;
    }
        
}

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
using EBA.Desktop.YEB;
using System.Text.RegularExpressions;

public partial class YEB_ImportWWRetiree : System.Web.UI.Page
{
    private const string _category = "ImportYEB";
    private const string source = "EBAS_WW";
    private const string TypeCD = "EBARET";
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
        lbl_error.Text = "YRMO - " + usryrmo + "     Pilot Indicator - " + pilotind;
    }
    protected void btn_import_Click(object sender, EventArgs e)
    {
        if (Session["yrmo"] != null)
        {
             usryrmo = Session["yrmo"].ToString();
        }

        
        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        if (usryrmo.Equals("") || pilotind.Equals(""))
        {
            lbl_error.Text = "YRMO value cannot be empty -  " + usryrmo + "      Pilot Indicator value cannot be empty -  " + pilotind;
            return;
        }
        lbl_error.Text = "";
        string logFilePath = Server.MapPath("~/uploads/") + "WW_Retirees_" + usryrmo + ".xls";

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
                    ImportYEBData.Rollback(source+pilotind, usryrmo);
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
        string _yrmo = Session["yrmo"].ToString(); //ddlYrmo.SelectedItem.Text;
        string logFilePath = Server.MapPath("~/uploads/") + "YEB_WWRET_" + _yrmo + ".xls";
        bool importStat = false;
        string filterexp = "";
        ImportYEBData iObj = new ImportYEBData();
        HRAExcelImport tObj = new HRAExcelImport();
        HRAParseData pObj = new HRAParseData();
        ImportYEBData.Rollback(source+pilotind, _yrmo);
        DataTable dtWWret;
        DataSet ds = new DataSet(); ds.Clear();

        ds = tObj.getExcelData(strFilePath1, "WWRetTable");
        //testing Cigna Admin fee bill 6-8-2009
        //ds = tObj.getCignaAdminFeeBillData(strFilePath1, "CignaAdminFeeTable");
        //tObj.ConfirmPutnamYRMO(strFilePath1, _yrmo);
        ////ds.Tables.Add(tObj.ConvertRangeXLS(_filepath, dt, "SSN", 0));
       ///_counter = pObj.parsePutnamPartData(ds, _filepath, _source, _qy);
       ///end testing///
        dtWWret = ds.Tables["WWRetTable"];

        if (dtWWret.Rows.Count > 0)
        {
            if (pilotind == "PI")
          
                {
                    filterexp = "(HealthStatusCode not in ('RT','VT','RFB','RFG','RFI','RFO','RFP','RFS')) AND " +
                               "EligGroupID <> 0 AND ((Med_TierID)  not in (1,2,3,4,5,6)) AND ((Den_TierID) not in (1,2,3,4,5,6)) and ((Vis_TierID) not in (1,2,3,4,5,6)) AND " +
                                "((Med_OptionID <> 100) OR (Den_OptionID <>100) OR (Vis_OptionID <> 100)) AND (pilotflag='True') AND EEID >0";
                               //"(Med_OptionID <> '100' and Den_OptionID <>'100' and Vis_OptionID <> '100') AND (pilotflag='True') AND EEID >0";
                              
                    DataRow[] foundrows = dtWWret.Select(filterexp);
                    _counter = foundrows.Length;
                    //foreach (DataRow dr in foundrows)
                    {
                        //insert the row in the YEB Detail table
                        iObj.insertWWRetData(foundrows, usryrmo, pilotind, source, TypeCD);
                    }

                }
            
            else if (pilotind == "NP")
                {
                    filterexp = "(HealthStatusCode not in ('RT','VT','RFB','RFG','RFI','RFO','RFP','RFS')) AND " +
                               "EligGroupID <> 0 AND ((Med_TierID)  not in (1,2,3,4,5,6)) AND ((Den_TierID) not in (1,2,3,4,5,6)) and ((Vis_TierID) not in (1,2,3,4,5,6)) AND " +
                               "((Med_OptionID <> 100) OR (Den_OptionID <>100) OR (Vis_OptionID <> 100)) AND (pilotflag='False') AND EEID >0";
                               //"(Med_OptionID <> '100' AND Den_OptionID <>'100' AND Vis_OptionID <> '100') AND (pilotflag='False') AND EEID >0";
                    DataRow[] foundrows = dtWWret.Select(filterexp);
                    _counter = foundrows.Length;
                    //foreach (DataRow dr in foundrows)
                    {
                        //insert the row in the YEB Detail table
                        iObj.insertWWRetData(foundrows, usryrmo, pilotind, source, TypeCD);
                    }

                }
            }

        if (File.Exists(logFilePath)) { File.Delete(logFilePath); }

        importStat = true;

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "YEB", "ImportSourceFile", "YEB_WWRET", "WW Import", usryrmo, _counter);

        return importStat;
    }



    protected void btn_reimport_Click(object sender, EventArgs e)
    {

    }
}

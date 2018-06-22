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
using EBA.Desktop.Audit;
using EBA.Desktop.YEB;
using System.IO;

public partial class YEB_ImportCobra_ : System.Web.UI.Page

{
    private const string _category = "Import";
    private const string source = "CobraNP";
    private string usryrmo = "";
    private string pilotind = "";
    int _counter = 0;
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
            //txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
            //checkPastImport();
            ddlYrmoList();
            //set default pilotind value to "PI"
            Session["pilotind"] = "PI";

            if (ddlYrmo.SelectedIndex==0)
            {
                Session["yrmo"] = ddlYrmo.SelectedItem.Text;
            }
        }

        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        lbl_error.Text = "YRMO -  " + usryrmo +   "      Pilot Indicator - " + pilotind;

    }
    protected void btn_import_Click(object sender, EventArgs e)
    {

    }
    protected void btn_import_Click1(object sender, EventArgs e)
    {
        string yrmo = ddlYrmo.SelectedItem.Text;
        lbl_error.Text = "";
        //lblError1.Text = "";
        

        if (Page.IsValid)
        {
            if ( Session["pilotind"] == null)
            {
                lbl_error.Text = "Please Select a Pilot indictor value";
                return;
             }
            if (txtPrevYRMO.Visible)
            {
                lbl_error.Text = "Enter YRMO in format 'yyyymm'";
                return;
            }
            if (usryrmo.Equals("") || pilotind.Equals(""))
            {
                lbl_error.Text = "YRMO value cannot be empty -  " + usryrmo + "      Pilot Indicator value cannot be empty -  " + pilotind;
                return;
            }
            lbl_error.Text = "";
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
                        bindResults();
                        //ImportYEBData.SavePrintFiles(source, yrmo);
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
                    }
                }
                catch (Exception ex)
                {
                    //resultDiv.Visible = false;
                    ImportYEBData.Rollback("COB" + Session["pilotind"].ToString(), yrmo);
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

    protected void ddlYrmoList()
    {
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = ImportYEBData.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void bindResults()
    {
        //clearMessages();
        //resultDiv.Visible = true;
        string _sb1 = "", _td1 = "", _tw1 = "", _eb1 = "";
        try
        {
            DataSet dsresult = new DataSet();
            dsresult = ImportYEBData.getYEBData(ddlYrmo.SelectedItem.Text, _sb1);
            foreach (DataRow dr in dsresult.Tables[0].Rows)
            {
                string _type = dr["Type"].ToString();
                switch (_type)
                {
                    case "Start Balance":
                        _sb1 = Convert.ToDecimal(dr["Amount"]).ToString("C");
                        break;
                    case "Total Deposits":
                        _td1 = Convert.ToDecimal(dr["Amount"]).ToString("C");
                        break;
                    case "Total Withdrawls":
                        _tw1 = ((-1) * Convert.ToDecimal(dr["Amount"])).ToString("C");
                        break;
                    case "End Balance":
                        _eb1 = Convert.ToDecimal(dr["Amount"]).ToString("C");
                        break;
                }
            }
            //if (dsresult.Tables[0].Rows.Count > 0)
            //{
            //    //Summary
            //    lblSBalance.Text = _sb1;
            //    lblDeposits.Text = _td1;
            //    lblWithdrawls.Text = _tw1;
            //    lblEnding.Text = _eb1;
            //    decimal _tot1 = decimal.Parse(_sb1, System.Globalization.NumberStyles.Currency)
            //                        + decimal.Parse(_td1, System.Globalization.NumberStyles.Currency)
            //                        + decimal.Parse(_tw1, System.Globalization.NumberStyles.Currency);
            //    lblTotalV.Text = _tot1.ToString("C");
            //    decimal _tot2 = _tot1 - decimal.Parse(_eb1, System.Globalization.NumberStyles.Currency);
            //    lblFinalV.Text = _tot2.ToString("C");
            //    if (_tot2 != 0)
            //    {
            //        Image3.Visible = true;
            //    }
            //    else
            //    {
            //        Image1.Visible = true;
            //    }

            //    //Deposits
            //    decimal _dep1 =
            //        decimal.Parse((dsresult.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND ([Type] = 'DEP' OR [Type] = 'MSCC')")).ToString(), System.Globalization.NumberStyles.Currency);
            //    decimal _tot3 = decimal.Parse(_td1, System.Globalization.NumberStyles.Currency) - _dep1;
            //    lblDep11.Text = _dep1.ToString("C");
            //    lblDep12.Text = _td1;
            //    lblDeptotal.Text = _tot3.ToString("C");
            //    if (_tot2 != 0)
            //    {
            //        Image5.Visible = true;
            //    }
            //    else
            //    {
            //        Image4.Visible = true;
            //    }

            //    //Deposits
            //    decimal _withd1 =
            //        decimal.Parse((dsresult.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND ([Type] <> 'DEP' AND [Type] <> 'MSCC')")).ToString(), System.Globalization.NumberStyles.Currency);
            //    decimal _tot4 = decimal.Parse(_tw1, System.Globalization.NumberStyles.Currency) + _withd1;
            //    lblWithd11.Text = _withd1.ToString("C");
            //    lblWithd12.Text = ((-1) * decimal.Parse(_tw1, System.Globalization.NumberStyles.Currency)).ToString("C");
            //    lblWithdtotal.Text = _tot4.ToString("C");
            //    if (_tot4 != 0)
            //    {
            //        Image7.Visible = true;
            //    }
            //    else
            //    {
            //        Image6.Visible = true;
            //    }
            //}
        }
        catch (Exception ex)
        {
            //errorDiv1.Visible = true;
            //lblError1.Text = ex.Message;
        }
    }

    Boolean ImportFile(string strFilePath1)
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;
        bool importStat = false;
        ImportYEBData vobj = new ImportYEBData();
        ImportYEBData.Rollback("COB" + Session["pilotind"].ToString(), _yrmo);
        ImportYEBData.Rollback("COB", _yrmo);
        _counter = vobj.importCobraNP_YEB(_yrmo, strFilePath1, Session["pilotind"].ToString());
        ImportYEBData.insertImportStatus(_yrmo, source);
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        Audit.auditUserTaskI(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "YEB", "YEB Imports", "YEB_Cobra", "YEB Detail", _yrmo, _counter);
        importStat = true;
        return importStat;
    }

    protected void rbtnWireTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Int32.Parse(rbtnWireTypes.SelectedItem.Value) == 0)
        {
            Session["pilotind"] = "PI";
            pilotind = "PI";
        }
        else
        {
            Session["pilotind"] = "NP";
            pilotind = "NP";
        }
        lbl_error.Text = "YRMO -  " + usryrmo + "      Pilot Indicator - " + pilotind;

    }
    protected void checkPastImport()
    {
        string _yrmo = ddlYrmo.SelectedItem.Text;

        if (ImportYEBData.PastImport(source, _yrmo))
        {
            MultiView1.SetActiveView(view_reimport);
            lbl_reimport.Text = "Imported already for year-month (YRMO): " + _yrmo;
            bindResults();
        }
        else
        {
            MultiView1.SetActiveView(view_main);
            //UpdateImportType();
        }
    }

    private void clearMessages()
    {
        lbl_error.Text = "";
        //errorDiv1.Visible = false;
        //lblError1.Text = "";
        //Image1.Visible = false;
        //Image3.Visible = false;
        //Image4.Visible = false;
        //Image5.Visible = false;
        //Image6.Visible = false;
        //Image7.Visible = false;
    }


    protected void ddlYrmo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        //lblError1.Text = "";
        //resultDiv.Visible = false;

        try
        {
            if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO.Text = "";
                lblPrevYrmo.Visible = false;
                ddlYrmo.Visible = false;

                //DivNewYRMO.Visible = true;
            }
            else
            {
                Session["yrmo"] = ddlYrmo.SelectedItem.Text.ToString();
                usryrmo = ddlYrmo.SelectedItem.Text.ToString();
                //checkPastImport();
            }
            lbl_error.Text = "YRMO -  " + usryrmo + "      Pilot Indicator - " + pilotind;
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }

    }
}

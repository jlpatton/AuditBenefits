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

public partial class YEB_ImportLOAActives : System.Web.UI.Page
{
    private const string _category = "ImportYEB";
    private const string src = "EBNETHR";
    private string status = "";
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
    protected void btn_importLOA_Click(object sender, EventArgs e)
    {
        DataTable dtLOA;
        ImportYEBData iObj = new ImportYEBData();
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        status = "EBALOA";

        if (pilotind != "" && status != "")
            try
            {
                //Delete any previously imported data for the specified YRMO and criteria
                ImportYEBData.Rollback("LOA" + pilotind, usryrmo);
                //retrieve the datatable for the criteria specified based on status
                dtLOA = ImportYEBData.getYEBEmployeesbyStatus(pilotind, status.Substring(3, 3)).Tables[0];
                if (dtLOA.Rows.Count > 0)
                {
                    ImportYEBData.PrintLOAProgressBar();
                    _counter = iObj.insertYEB_ACT_LOA_Data(dtLOA, usryrmo, pilotind, src, status);
                    ImportYEBData.ClearProgressBar(_counter);
                    if (_counter > 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
                    }

                }
            }
            catch (Exception ex)
            {
               //resultDiv.Visible = false;
                //ImportYEBData iObj = new ImportYEBData();
                ImportYEBData.Rollback("LOA"+pilotind, usryrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }

       

    }
    protected void btn_reimport_Click(object sender, EventArgs e)
    {

    }
    protected void btn_importActives_Click(object sender, EventArgs e)
    {
        DataTable dtLOA;
        ImportYEBData iObj = new ImportYEBData();
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        status = "EBAACT";

        if (pilotind != "" && status != "")
            try
            {
                //Delete any previously imported data for the specified YRMO and criteria
                ImportYEBData.Rollback("ACT" + pilotind, usryrmo);
                //retrieve the datatable for the criteria specified based on status
                dtLOA = ImportYEBData.getYEBEmployeesbyStatus(pilotind, status.Substring(3, 3)).Tables[0];
                if (dtLOA.Rows.Count > 0)
                {
                    ImportYEBData.PrintACTProgressBar();
                    _counter = iObj.insertYEB_ACT_LOA_Data(dtLOA, usryrmo, pilotind, src, status);
                    ImportYEBData.ClearProgressBar(_counter);
                    if (_counter > 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Import completed successfully -- " + _counter + " records imported";
                    }

                }
            }
            catch (Exception ex)
            {
                //resultDiv.Visible = false;
                
                ImportYEBData.Rollback("ACT" + pilotind, usryrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }

    }
    protected void btnYEBOL_Click(object sender, EventArgs e)
    {
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }
       
        if (usryrmo != "")
            try
            {
                            
                    _counter =  ImportYEBData.updateYEBDetailData(usryrmo, "YEBOL");
                    
                    if (_counter >= 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Update completed successfully -- " + _counter + " records updated in YEBDetail";
                    }

             }
           
            catch (Exception ex)
            {
               lbl_error.Text = "Error - " + ex.Message;
            }

    }
    protected void btnYEBExpat_Click(object sender, EventArgs e)
    {
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (usryrmo != "")
            try
            {

                _counter = ImportYEBData.updateYEBDetailData(usryrmo, "YEBEXPAT");

                if (_counter >= 0)
                {
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "Update completed successfully -- " + _counter + " records updated in YEBDetail";
                }

            }

            catch (Exception ex)
            {
                lbl_error.Text = "Error - " + ex.Message;
            }
    }
    protected void btnDupSSN_Click(object sender, EventArgs e)
    {

        DataTable dtSSNDup;
        //DataSet dsSSN;
        ImportYEBData iObj = new ImportYEBData();
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        if ((usryrmo=="") )
        {
            
            lbl_error.Text = "Please select YRMO (year month) value to continue ";
            return;
        }
        if ((pilotind == ""))
        {
            
            lbl_error.Text = "Please select PilotIndicator (PI or NP) value to continue ";
            return;
        }
        status = "EBASSN";

        if (pilotind != "" && status != "")
            try
            {
                //retrieve the datatable for the criteria specified based on status
                dtSSNDup = ImportYEBData.getYEBDuplicateRecs(pilotind,usryrmo,"SSN").Tables[0];
                if (dtSSNDup.Rows.Count > 0)
                
                {
                    //Delete any previously imported data for the specified YRMO and criteria
                    ImportYEBData.Rollback("DUPSSN" + pilotind, usryrmo);
                    //loop thru the datatable rows for inserting to YEB_Dup_Detail table
                    ImportYEBData.DisplayProgressBar();
                    _counter = iObj.insertYEBDupData(dtSSNDup, "SSNO");
                    ImportYEBData.HideProgressBar(_counter);

                    if (_counter > 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Identified the Duplicate SSN records successfully -- " + _counter + "  Records identified for YEB Distribution from " + dtSSNDup.Rows.Count + " records";
                    }

                }
                else
                {
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "No Duplicate SSN records exist in the YEB Detail table currently";
                }
            }
            catch (Exception ex)
            {
                ImportYEBData.Rollback("DUPSSN" + pilotind, usryrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }


    }
    protected void btnAddrDup_Click(object sender, EventArgs e)
    {
        DataTable dtAddrDup;
        ImportYEBData iObj = new ImportYEBData();
        if (Session["yrmo"] != null)
        {
            usryrmo = Session["yrmo"].ToString();
        }

        if (Session["pilotind"] != null)
        {
            pilotind = Session["pilotind"].ToString();
        }
        if ((usryrmo == ""))
        {

            lbl_error.Text = "Please select YRMO (year month) value to continue ";
            return;
        }
        if ((pilotind == ""))
        {

            lbl_error.Text = "Please select PilotIndicator (PI or NP) value to continue ";
            return;
        }
        status = "EBAADDR";

        if (pilotind != "" && status != "")
            try
            {
                
                //retrieve the datatable for the criteria specified based on status
                dtAddrDup = ImportYEBData.getYEBDuplicateRecs(pilotind,usryrmo,"ADDR1").Tables[0];
                if (dtAddrDup.Rows.Count > 0)
               
                {
                    //Delete any previously imported data for the specified YRMO and criteria
                    ImportYEBData.Rollback("DUPADDR" + pilotind, usryrmo);
                    //loop thru the datatable rows for inserting to YEB_Dup_Detail table
                    ImportYEBData.DisplayProgressBar();
                    _counter = iObj.insertYEBDupData(dtAddrDup, "ADDR1");
                    ImportYEBData.HideProgressBar(_counter);
                    if (_counter > 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "Identified the Duplicate (Addr1+City) records successfully -- " + _counter + "  Records identified for YEB Distribution from " + dtAddrDup.Rows.Count + " records";
                    }
                    else if
                        (_counter == 0)
                    {
                        MultiView1.SetActiveView(view_result);
                        lbl_result.Text = "No Duplicate (Addr1+City) records were identified from the YEB Detail table based on the existing data ";
                    }

                }
                else
                {
                    MultiView1.SetActiveView(view_result);
                    lbl_result.Text = "No Duplicate ADDR1+City records exist in the YEBDetail table currently";
                }
            }
            catch (Exception ex)
            {
                //resultDiv.Visible = false;
               ImportYEBData.Rollback("DUPADDR" + pilotind, usryrmo);
                lbl_error.Text = "Error - " + ex.Message;
            }

    }
}

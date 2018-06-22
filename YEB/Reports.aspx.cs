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
using EBA.Desktop.HRA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using EBA.Desktop.YEB;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using EBA.Desktop.WireTemplates;
using System.Data.SqlClient;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

public partial class YEB_Reports : System.Web.UI.Page
    
{
    private const string _category = "ReportYEB";
    private const string src = "EBNETHR";
    private string status = "";
    int _counter = 0;
    private string usryrmo = "";
    private string pilotind = "";
    private const string _class = "Reports";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            if (Session["yrmo"] != null)
            {
                usryrmo = Session["yrmo"].ToString();
            }

            if (Session["pilotind"] != null)
            {
                pilotind = Session["pilotind"].ToString();
            }

            if (pilotind=="NP")
            {
                (rbtnEmpTypes.SelectedItem.Value) = Convert.ToString(1);
            }
            else
            {
                (rbtnEmpTypes.SelectedItem.Value) = Convert.ToString(0);
            }
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M600", "M603");
            //ddlYrmoList();
            //checkPastRecon();
            //txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
        }

        lbl_error.Text = "YRMO - " + usryrmo + "     Pilot Indicator - " + pilotind;

    }


    protected void lnk_HMYPS_OnClick(object sender, EventArgs e)
    {
        //lbl_error.Text = "";
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("HOME_YEB_RET", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_HMS_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("HOME_SAR_ONLY",pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_CMYPS_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("Active_YEB_Group1", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_CMS_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("Active_SAROnly", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_PDF_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("Active_YEB_Group2", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_PRACT_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("Active_PROnly", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_ACTNP_OnClick(object sender, EventArgs e)
    {
        string connect = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        try
        {

           // EBA.Desktop.WireTemplates.CustomITextReports wireTemplate = new EBA.Desktop.WireTemplates.CustomITextReports(connect);
            SqlDataReader rdr;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString))
            {

                //string query = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = '200904' and TypeCd IN ('EBACOB', 'EBALOA', 'EBARET') and YEBOnline =0";
                string query = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = '200904' and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0"; // and eeno<419477";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();

                rdr = cmd.ExecuteReader();
                //ImportYEBData.PrintACTProgressBar();
                //SendOutPDF(wireTemplate.CreatePDF("YEB NP HOME MAILING POP:", rdr));
               //ImportYEBData.ClearProgressBar(0);
                conn.Close();
            }

        }

        //catch (DocumentException dex)
        //{

        //    //Response.Write(dex.Message);
        //    lbl_error.Text = "Error in generating YEB Report<br />" + dex.Message;

        //}

        catch (IOException ioex)
        {

            //Response.Write(ioex.Message);
            lbl_error.Text = "Error in generating YEB Report<br />" + ioex.Message;
        }

        finally
        {



        }
    }

    protected void SendOutPDF(System.IO.MemoryStream PDFData)
    {
        // Clear response content & headers
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        Response.Charset = string.Empty;
        Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
        Response.AddHeader("Content-Disposition",
            "attachment; filename=YEB2009_ACTNP" + Title.Replace(" ", "").Replace(":", "-") + ".pdf");

        Response.OutputStream.Write(PDFData.GetBuffer(), 0, PDFData.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.End();
    }
    protected void lnk_SSNDUP_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("DUP_SSN", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void lnk_ADDRDUP_OnClick(object sender, EventArgs e)
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("DUP_ADDR1", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //lbl_error.Text = "";
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
            lbl_error.Text = "YRMO value cannot be empty- " + usryrmo + "      Pilot Indicator value cannot be empty - " + pilotind;
            return;
        }
        try
        {
            YEB_ReportsDef.GenerateReport("HOME_YEB_NONRET", pilotind, usryrmo);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating YEB Report<br />" + ex.Message;
        } 
    }
}

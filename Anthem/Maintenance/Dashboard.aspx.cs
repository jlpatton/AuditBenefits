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
using System.Data.SqlClient;

public partial class Anthem_Maintenance_Dashboard : System.Web.UI.Page
{
    static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
    static string connStr2 = ConfigurationManager.ConnectionStrings["FRDDB"].ConnectionString;
    static SqlConnection connect = new SqlConnection(connStr);
    static SqlConnection connectFRD = new SqlConnection(connStr2);
    static SqlCommand command = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M100", "M104");
            hideDivs();
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);        
    }

    protected void hideDivs()
    {
        Dom.Visible = false;
        Intl.Visible = false;
        CA.Visible = false;
        EAP.Visible = false;
        NCA.Visible = false;
        RX.Visible = false;
        stepFinal.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string month = ddlMonths.SelectedValue.ToString();
        string year = txtYear.Text.ToString();
        string yrmo = year + month;
        string module = ddlModule.SelectedValue.ToString();
        if (module.Equals("Not Selected"))
        {
            hideDivs();
        }
        switch (module)
        {
            case "Dom":
                domesticBillng(yrmo);
                break;
            case "Intl":
                internationalBillng(yrmo);
                break;
            case "EAP":
                eapBillng(yrmo);
                break;
            case "CA":
                CAClaims(yrmo);
                break;
            case "NCARFDF":
                NonCARFDFClaims(yrmo);
                break;
            case "RX":
                RXClaims(yrmo);
                break;           
        }
    }

    protected void domesticBillng(string _yrmo)
    {
        hideDivs();
        Dom.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("Dom");
        domesticData(_yrmo);
    }
    protected void internationalBillng(string _yrmo)
    {
        hideDivs();
        Intl.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("Intl");
        internationalData(_yrmo);
    }
    protected void eapBillng(string _yrmo)
    {
        hideDivs();
        EAP.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("EAP");
        eapData(_yrmo);
    }
    protected void CAClaims(string _yrmo)
    {
        hideDivs();
        CA.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("CA");
        caClaimsData(_yrmo);
    }
    protected void NonCARFDFClaims(string _yrmo)
    {
        hideDivs();
        NCA.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("NCARFDF");
        ncaClaimsData(_yrmo);
    }
    protected void RXClaims(string _yrmo)
    {
        hideDivs();
        RX.Visible = true;
        stepFinal.Visible = true;
        removeAttribute("RX");
        rxClaimsData(_yrmo);
    }

    protected void removeAttribute(string _src)
    {
        switch (_src)
        {
            case "Dom":
                l1ADP.Attributes.Remove("class");
                l1AnthemBill.Attributes.Remove("class");
                l1Grs.Attributes.Remove("class");
                l1Recon.Attributes.Remove("class");
                l1RetireeMC.Attributes.Remove("class");
                l1RetireeNM.Attributes.Remove("class");
                break;
            case "Intl":
                l2IntlHeadcount.Attributes.Remove("class");
                l2Recon.Attributes.Remove("class");
                break;
            case "EAP":
                L7ADP.Attributes.Remove("class");
                L7EAP.Attributes.Remove("class");
                L7GRS.Attributes.Remove("class");                
                L7HTH.Attributes.Remove("class");
                L7MC.Attributes.Remove("class");
                L7NM.Attributes.Remove("class");
                L7Recon.Attributes.Remove("class");
                break;
            case "CA":
                l3BOA.Attributes.Remove("class");
                l3CA.Attributes.Remove("class");
                l3Recon.Attributes.Remove("class");
                break;
            case "NCARFDF":
                L5DF.Attributes.Remove("class");
                L5NCA.Attributes.Remove("class");
                L5Recon.Attributes.Remove("class");
                l5RF.Attributes.Remove("class");
                break;
            case "RX":
                L6Pharmacy.Attributes.Remove("class");
                L6Recon.Attributes.Remove("class");
                break;
        }
    }
    protected void domesticData(string yrmo)
    {
        string[] src = { "GRS", "ADP", "ANTH_%" ,"RET_M","RET_NM" };
        for (int i = 0; i < src.Length; i++)
        {
            try
            {
                string cmdStr1 = "SELECT COUNT(*) FROM Headcount WHERE hdct_source LIKE '" + src[i] + "' AND hdct_yrmo = '" + yrmo + "'";
                command = new SqlCommand(cmdStr1, connect);
                connect.Open();
                int cnt = Convert.ToInt32(command.ExecuteScalar());
                if (cnt > 0)
                {
                    switch (src[i])
                    {
                        case "GRS": l1Grs.Attributes.Add("Class", "current");
                            break;
                        case "ADP": l1ADP.Attributes.Add("Class", "current");
                            break;
                        case "RET_M": l1RetireeMC.Attributes.Add("Class", "current");
                            break;
                        case "RET_NM": l1RetireeNM.Attributes.Add("Class", "current");
                            break;
                        case "ANTH_%": l1AnthemBill.Attributes.Add("Class", "current");
                            break;
                    }
                }
                string cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE (rcn_source = 'RET' OR rcn_source = 'ACT' OR rcn_source = 'COB') and rcn_yrmo = '" + yrmo + "'";
                command = new SqlCommand(cmdStr, connect);
                int cnt1 = Convert.ToInt32(command.ExecuteScalar());
                if (cnt1 > 0)
                {
                    l1Recon.Attributes.Add("Class", "current");
                }
            }
            catch
            {
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }        
    }
    protected void internationalData(string yrmo)
    {
        string cmdStr1 = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'ANTH_INTL' AND hdct_yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr1, connect);
        connect.Open();
        int cnt = Convert.ToInt32(command.ExecuteScalar());
        if (cnt > 0)
        {
            l2IntlHeadcount.Attributes.Add("Class", "current");
        }        
        command.Dispose();
        string cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE (rcn_source = 'INTL') and rcn_yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr, connect);
        int cnt1 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt1 > 0)
        {
           l2Recon.Attributes.Add("Class", "current");
        }
        command.Dispose();
        connect.Close();
    }

    protected void eapData(string yrmo)
    {
        string[] src = { "GRS", "ADP", "ANTH_EAP", "RET_M", "RET_NM", "HTH" };
        for (int i = 0; i < src.Length; i++)
        {
            try
            {
                string cmdStr1 = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = '" + src[i] + "' AND hdct_yrmo = '" + yrmo + "'";
                command = new SqlCommand(cmdStr1, connect);
                connect.Open();
                int cnt = Convert.ToInt32(command.ExecuteScalar());
                if (cnt > 0)
                {
                    switch (src[i])
                    {
                        case "GRS": L7GRS.Attributes.Add("Class", "current");
                            break;
                        case "ADP": L7ADP.Attributes.Add("Class", "current");
                            break;
                        case "RET_M": L7MC.Attributes.Add("Class", "current");
                            break;
                        case "RET_NM": L7NM.Attributes.Add("Class", "current");
                            break;
                        case "HTH": L7HTH.Attributes.Add("Class", "current");
                            break;
                        case "ANTH_EAP": L7EAP.Attributes.Add("Class", "current");
                            break;
                    }
                }
                command.Dispose();
                string cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE (rcn_source = 'EAP') and rcn_yrmo = '" + yrmo + "'";
                command = new SqlCommand(cmdStr, connect);
                int cnt1 = Convert.ToInt32(command.ExecuteScalar());
                if (cnt1 > 0)
                {
                    L7Recon.Attributes.Add("Class", "current");
                }
                command.Dispose();                
            }
            catch
            {
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }
    }
    protected void caClaimsData(string yrmo)
    {
        string cmdStr1 = "SELECT COUNT(*) FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";
        string cmdStr2 = "SELECT COUNT(*) FROM BOAStatement WHERE boaYRMO = '" + yrmo + "'";
        command = new SqlCommand(cmdStr1, connect);

        connect.Open();

        int cnt = Convert.ToInt32(command.ExecuteScalar());
        if (cnt > 0)
        {
            l3CA.Attributes.Add("Class", "current");
        }        
        command.Dispose();

        command = new SqlCommand(cmdStr2, connect);        
        int cnt1 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt1 > 0)
        {
            l3BOA.Attributes.Add("Class", "current");
        }
        command.Dispose();

        string cmdStr = "SELECT COUNT(*) FROM CAClaimsRecon WHERE yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr, connect);
        int cnt2 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt2 > 0)
        {
            l3Recon.Attributes.Add("Class", "current");
        }
        command.Dispose();

        connect.Close();
    }    
    protected void ncaClaimsData(string yrmo)
    {
        string cmdStr1 = "SELECT COUNT(*) FROM AnthBillTrans WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";
        string cmdStr2 = "SELECT COUNT(*) FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_yrmo = '" + yrmo + "'";
        string cmdStr3 = "SELECT COUNT(*) FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr1, connect);

        connect.Open();

        int cnt = Convert.ToInt32(command.ExecuteScalar());
        if (cnt > 0)
        {
            L5NCA.Attributes.Add("Class", "current");
        }       
        command.Dispose();

        command = new SqlCommand(cmdStr2, connect);        
        int cnt1 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt1 > 0)
        {
            l5RF.Attributes.Add("Class", "current");
        }       
        command.Dispose();

        command = new SqlCommand(cmdStr3, connect);        
        int cnt2 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt2 > 0)
        {
            L5DF.Attributes.Add("Class", "current");
        }
        command.Dispose();

        string cmdStr = "SELECT COUNT(*) FROM rf_recon WHERE rf_yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr, connect);
        int cnt3 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt3 > 0)
        {
            L5Recon.Attributes.Add("Class", "current");
        }
        command.Dispose();

        connect.Close();
    }
    protected void rxClaimsData(string yrmo)
    {
        string cmdStr1 = "SELECT COUNT(*) FROM AnthBillTrans WHERE anbt_sourcecd = 'RX' AND anbt_yrmo = '" + yrmo + "'";
        
        connect.Open();

        command = new SqlCommand(cmdStr1, connect);
        int cnt = Convert.ToInt32(command.ExecuteScalar());
        if (cnt > 0)
        {
            L6Pharmacy.Attributes.Add("Class", "current");
        }
        command.Dispose();

        string cmdStr = "SELECT COUNT(*) FROM rx_recon WHERE rx_rcn_yrmo = '" + yrmo + "'";
        command = new SqlCommand(cmdStr, connect);
        int cnt1 = Convert.ToInt32(command.ExecuteScalar());
        if (cnt1 > 0)
        {
            L6Recon.Attributes.Add("Class", "current");
        }
        command.Dispose();

        connect.Close();       
    }

    
}

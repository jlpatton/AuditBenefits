using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

/// <summary>
/// Summary description for AdminBLL
/// </summary>
/// 
namespace EBA.Desktop.Admin
{
    public class AdminBLL
    {
        public AdminBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //  M Code	    Module	        Type
        //--------------------------------------
        //  M1	        ADMIN	        M
        //  M2          AUDIT           M
        //  M100	    ANTHEM	        M
        //  M200	    HRA	            M
        //  M101	    Billing	        ANTH
        //  M102	    Claims	        ANTH
        //  M103        Report          ANTH
        //  M104	    Maintenance	    ANTH
        //  M201	    Maintenance	    HRA
        //  M202	    Reconciliation	HRA
        //  M203	    Operations	    HRA
        //  M400        VWA             M
        //  M500        ImputedIncome   M
        //  M600        YEB             M
        //  M700        EBADesktop      M
       

        public static void InRole(string _uName, string _pCode, string _cCode)
        {
            bool auth = admin.checkMod(_uName, _pCode, _cCode);

            if (!auth && (_pCode.Equals("M100") || _pCode.Equals("M200") || _pCode.Equals("M300") || _pCode.Equals("M400") || _pCode.Equals("M500") || _pCode.Equals("M600") || _pCode.Equals("M700") || _pCode.Equals("M1") || _pCode.Equals("M2")) && _cCode.Equals(""))
            {
                HttpContext.Current.Server.Transfer("~/NoAccess.aspx");
            }
            
            else if (!auth && _pCode.Equals("M100") && (_cCode.Equals("M101") || _cCode.Equals("M102") || _cCode.Equals("M103") || _cCode.Equals("M104")))
            {
                HttpContext.Current.Server.Transfer("~/Anthem/Denied.aspx");

            }
            else if (!auth && _pCode.Equals("M300") && (_cCode.Equals("M301") || _cCode.Equals("M302") || _cCode.Equals("M303") || _cCode.Equals("M304")))
            {
                HttpContext.Current.Server.Transfer("~/IPBA/Denied.aspx");
            }
            //Checks for HRA,VWA,ImputedIncome,YEB,EBADesktop modules code M200,M400,M500,M600,M700 and the individual sub menu authorizations for each user
            // to give access or deny access at the page level for a given role. 
            // Fix For Defect ID 120058 -(3,4,5,6,7)
            // Deepika Katam  08/02/09
            else if (!auth && _pCode.Equals("M200") && (_cCode.Equals("M201") || _cCode.Equals("M202") || _cCode.Equals("M203") || _cCode.Equals("M204") || _cCode.Equals("M205") || _cCode.Equals("M211") || _cCode.Equals("M212") || _cCode.Equals("M213") || _cCode.Equals("M214")))
            {
                HttpContext.Current.Server.Transfer("~/Hra/Denied.aspx");
            }
            else if (!auth && _pCode.Equals("M400") && (_cCode.Equals("M401") || _cCode.Equals("M402") || _cCode.Equals("M403") || _cCode.Equals("M404") || _cCode.Equals("M405") || _cCode.Equals("M406")))
            {
                HttpContext.Current.Server.Transfer("~/VWA/Denied.aspx");
            }
            else if (!auth && _pCode.Equals("M500") && (_cCode.Equals("M501") || _cCode.Equals("M502")))
            {
                HttpContext.Current.Server.Transfer("~/ImputedIncome/Denied.aspx");
            }
            else if (!auth && _pCode.Equals("M600") && (_cCode.Equals("M601") || _cCode.Equals("M602") || _cCode.Equals("M603")))
            {
                HttpContext.Current.Server.Transfer("~/YEB/Denied.aspx");
            }
            else if (!auth && _pCode.Equals("M700") && (_cCode.Equals("M701") || _cCode.Equals("M702") || _cCode.Equals("M703") || _cCode.Equals("M704") || _cCode.Equals("M705") || _cCode.Equals("M706") || _cCode.Equals("M707")))
            {
                HttpContext.Current.Server.Transfer("~/DsktpHist/Denied.aspx");
            }
        }

    }
}

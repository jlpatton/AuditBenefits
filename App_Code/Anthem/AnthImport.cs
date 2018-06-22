using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

namespace EBA.Desktop.Anthem
{
    public class AnthImport
    {
        static object nothing = System.Reflection.Missing.Value;
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;
        public AnthImport()
        {

        }        
        public static Boolean PastImport(string source, string yrmo)
        {
            
            string cmdStr = "";
            
            switch (source)
            {
                case "HTH":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'HTH' AND hdct_yrmo='" + yrmo + "'";
                    break;
                case "ANTH":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE (hdct_source LIKE 'ANTH%') AND hdct_yrmo='" + yrmo + "'";
                    break;                
                case "RET_M":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'RET_M' AND hdct_yrmo='" + yrmo + "'";
                    break;
                case "RET_NM":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'RET_NM' AND hdct_yrmo='" + yrmo + "'";
                    break;                             
                case "ADP":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'ADP' AND hdct_yrmo ='" + yrmo + "'";
                    break;
                case "GRS":
                    cmdStr = "SELECT COUNT(*) FROM Headcount WHERE hdct_source = 'GRS' AND hdct_yrmo ='" + yrmo + "'";
                    break;
                case "CLMRPT":
                    cmdStr = "SELECT COUNT(*) FROM AnthBillTrans WHERE (anbt_sourcecd = 'CA_CLMRPT' OR anbt_sourcecd = 'NCA_CLMRPT' OR anbt_sourcecd = 'ANRX') AND anbt_yrmo='" + yrmo + "'";
                    break;
                case "RFDF":
                    cmdStr = "SELECT COUNT(*) FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_yrmo='" + yrmo + "'";
                    break;
                case "DF":
                    cmdStr = "SELECT COUNT(*) FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_yrmo='" + yrmo + "'";
                    break;
                case "BOA":
                    cmdStr = "SELECT COUNT(*) FROM BOAStatement WHERE boaYRMO = '" + yrmo + "'";
                    break;
                case "ANTH_RX":
                    cmdStr = "SELECT COUNT(*) FROM anth_Rx WHERE rx_yrmo = '" + yrmo + "'";
                    break;
            }

            int count = 0;

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            command = new SqlCommand(cmdStr, connect);
            count = Convert.ToInt32(command.ExecuteScalar());
            connect.Close();
            if (count == 0)
                return false;
            else
                return true;
        }
        
        public static Boolean CheckFileYRMO(string source, string yrmo, string filename)
        {
            Boolean checkFileYRMO = false;
            string mm_dd_yy = Convert.ToDateTime(yrmo.Insert(4, "/")).ToString("MM-dd-yy");
            string yyyymmdd = Convert.ToDateTime(yrmo.Insert(4, "/")).ToString("yyyyMMdd");
            string mmyy = Convert.ToDateTime(yrmo.Insert(4, "/")).ToString("MMyy");

            switch (source)
            {
                case "HTH":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
                case "ANTH":
                    if (filename.Contains(mm_dd_yy))
                        checkFileYRMO = true;
                    break;
                case "RET_M":
                    if (filename.Contains(yyyymmdd))
                        checkFileYRMO = true;
                    break;
                case "RET_NM":
                    if (filename.Contains(yyyymmdd))
                        checkFileYRMO = true;
                    break;
                case "ADP":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
                case "GRS":
                    if (filename.Contains(mmyy))
                        checkFileYRMO = true;
                    break;
                case "DF":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
                case "BOA":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
                case "DFRF":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
                case "DFnoRF":
                    if (filename.Contains(yrmo))
                        checkFileYRMO = true;
                    break;
            }
            return checkFileYRMO;
        }

    }
}


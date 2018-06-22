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
using System.Data.OleDb;
using System.Data.Common;
using System.Collections.Generic;

/// <summary>
/// Summary description for ImportExcelReport
/// </summary>
/// 
namespace EBA.Desktop
{
    public class ImportExcelReport
    {
        private DataSet impData = null;

        public ImportExcelReport()
        {
            impData = new DataSet();

        }


        public DataSet importHTH(string _file, string _yrmo)
        {
            DataTable dt = new DataTable("hthTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, "EBA", _yrmo));
            return impData;
        }  

        public DataSet importClaims(string _file)
        {
            DataTable dt = new DataTable("claimTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt));
            return impData;
        }

        public DataSet importRXClaims(string _file)
        {
            DataTable dt = new DataTable("rxTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt));
            return impData;
        }

        public DataSet importDFNORF(string _file)
        {
            DataTable dt = new DataTable("dfnorfTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt));
            return impData;
        }

        public DataSet importMC(string _file, string _yrmo)
        {
            DataTable dt = new DataTable("mcTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, "EBA", _yrmo));
            return impData;
        }

        public DataSet importNM(string _file, string _yrmo)
        {
            DataTable dt = new DataTable("nmTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, "EBA", _yrmo));
            return impData;
        }
        public DataSet importMCNM(string _file, string _yrmo)
        {
            DataTable dt = new DataTable("mcnmTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, "EBA", _yrmo));
            return impData;
        }
        public DataSet importRF(string _file)
        {
            DataTable dt = new DataTable("rfTable");
            impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt));
            return impData;
        }

        public DataSet importAnthemBill(string _file)
        {
            DataTable dt = null;
            List<string> _sheetName = new List<string>();
            _sheetName.Add("International - Detail");
            _sheetName.Add("Active - Detail");
            _sheetName.Add("Retiree - Detail");
            _sheetName.Add("Cobra - Detail");
            _sheetName.Add("EAP - Detail");
            foreach (string sname in _sheetName)
            {
                switch (sname)
                {
                    case "International - Detail":
                        dt = new DataTable("intlTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                    case "Active - Detail":
                        dt = new DataTable("actTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                    case "Retiree - Detail":
                        dt = new DataTable("retTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                    case "Cobra - Detail":
                        dt = new DataTable("cobTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                    case "EAP - Detail":
                        dt = new DataTable("eapTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                }
            }
            return impData;

        }

        public DataSet importClaimsRX(string _file)
        {
            DataTable dt = null;
            List<string> _sheetName = new List<string>();
            _sheetName.Add("Medical Claims");
            _sheetName.Add("Rx Claims");
            foreach (string sname in _sheetName)
            {
                switch (sname)
                {
                    case "Medical Claims":
                        dt = new DataTable("medclaimsTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                    case "Rx Claims":
                        dt = new DataTable("medrxTable");
                        impData.Tables.Add(ConvertExcel.ConvertXLS(_file, dt, sname));
                        break;
                }
            }
            return impData;
        }

    }
}

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
using System.Globalization;
using EBA.Desktop.HRA;

namespace EBA.Desktop.VWA
{
    public class VWA_Results
    {
        public VWA_Results()
        {

        }

        public static DataSet GetRemitSumData(string yrmo)
        {            
            Decimal vwaAmt, acmeAmt, totalAmt, feePercent;
            Decimal disabVWA, disabacme, disabTotal;
            Decimal vwaGrandAmt, acmeGrandAmt, totalGrandAmt;
            string[] vendorCds = {"RemitOC", "RemitCigna", "RemitUHC", "RemitDisab", "RemitAnth" };
            DataSet ds = new DataSet(); ds.Clear();
            DataTable dsTable; dsTable = ds.Tables.Add("RemitSum");
            DataRow row;
            DataColumn col;

            col = new DataColumn("Vendor"); dsTable.Columns.Add(col);
            col = new DataColumn("VWA"); dsTable.Columns.Add(col);
            col = new DataColumn("acme"); dsTable.Columns.Add(col);
            col = new DataColumn("Total"); dsTable.Columns.Add(col);
            col = new DataColumn("fee %"); dsTable.Columns.Add(col);
            col = new DataColumn("Flag"); dsTable.Columns.Add(col);

            vwaGrandAmt = 0;
            acmeGrandAmt = 0;
            totalGrandAmt = 0;
            disabVWA = 0;
            disabacme = 0;
            disabTotal = 0;
            
            foreach (string vendorcd in vendorCds)
            {
                vwaAmt = VWAImportDAL.GetClientVWAFees(vendorcd, yrmo);
                acmeAmt = VWAImportDAL.GetClientDueClient(vendorcd, yrmo);
                totalAmt = VWAImportDAL.GetClientPaidVWA(vendorcd, yrmo);
                if (totalAmt == 0) { feePercent = 0; }
                else { feePercent = (vwaAmt / totalAmt); }                
                
                if(vendorcd.Equals("RemitDisab"))
                {
                    disabVWA = vwaAmt; disabacme = acmeAmt; disabTotal = totalAmt;
                }

                vwaGrandAmt = vwaGrandAmt + vwaAmt;
                acmeGrandAmt = acmeGrandAmt + acmeAmt;
                totalGrandAmt = totalGrandAmt + totalAmt;

                row = dsTable.NewRow();
                row["Vendor"] = VWA.GetRemitClientCd(vendorcd);
                row["VWA"] = vwaAmt;
                row["acme"] = acmeAmt;
                row["Total"] = totalAmt;
                row["fee %"] = feePercent;
                if ((vwaAmt + acmeAmt) != totalAmt)
                {
                    row["Flag"] = "!";
                }
                dsTable.Rows.Add(row);  
            }
            row = dsTable.NewRow();
            row["Vendor"] = "Total:";
            row["VWA"] = vwaGrandAmt;
            row["acme"] = acmeGrandAmt;
            row["Total"] = totalGrandAmt;
            dsTable.Rows.Add(row);

            //Non-disability row
            row = dsTable.NewRow();
            row["Vendor"] = "Non-Disability";
            row["VWA"] = (vwaGrandAmt- disabVWA);
            row["acme"] = (acmeGrandAmt - disabacme);
            row["Total"] = (totalGrandAmt - disabTotal);
            dsTable.Rows.Add(row);

            //Disability row
            row = dsTable.NewRow();
            row["Vendor"] = "Disability";
            row["VWA"] = disabVWA;
            row["acme"] = disabacme;
            row["Total"] = disabTotal;
            dsTable.Rows.Add(row); 
            
            //Total row
            row = dsTable.NewRow();
            row["Vendor"] = "Total:";
            row["VWA"] = vwaGrandAmt;
            row["acme"] = acmeGrandAmt;
            row["Total"] = totalGrandAmt;
            dsTable.Rows.Add(row);

            return ds;
        }

        public static DataSet GetRemitDetData(string yrmo)
        {
            DataSet dsFinal = new DataSet(); dsFinal.Clear();
            DataSet dsTemp;
            DataTable tblDet = new DataTable(); dsFinal.Tables.Add(tblDet);
            DataRow row;
            string[] vendorCds = { "RemitOC", "RemitCigna", "RemitUHC", "RemitDisab", "RemitAnth" };

            foreach(string vendorcd in vendorCds)
            {
                dsTemp = new DataSet(); dsTemp.Clear();
                dsTemp = VWAImportDAL.GetRemitDetailData(yrmo, vendorcd);
                if ((dsTemp != null) && (dsTemp.Tables.Count != 0))
                {
                    tblDet.Merge(dsTemp.Tables[0]);
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        row = tblDet.NewRow();
                        row["Beneficiary Name"] = "** SUB TOTAL:";
                        row["Paid VWA"] = dsTemp.Tables[0].Compute("SUM([Paid VWA])", String.Empty);
                        row["Paid Client"] = dsTemp.Tables[0].Compute("SUM([Paid Client])", String.Empty);
                        row["VWA Fees"] = dsTemp.Tables[0].Compute("SUM([VWA Fees])", String.Empty);
                        row["acme Remit"] = dsTemp.Tables[0].Compute("SUM([acme Remit])", String.Empty);
                        tblDet.Rows.Add(row);
                    }
                }                
            }
            if (tblDet.Rows.Count != 0)
            {
                row = tblDet.NewRow();
                row["Beneficiary Name"] = "** GRAND TOTAL:";
                row["Paid VWA"] = tblDet.Compute("SUM([Paid VWA])", "[Beneficiary Name] = '** SUB TOTAL:'");
                row["Paid Client"] = tblDet.Compute("SUM([Paid Client])", "[Beneficiary Name] = '** SUB TOTAL:'");
                row["VWA Fees"] = tblDet.Compute("SUM([VWA Fees])", "[Beneficiary Name] = '** SUB TOTAL:'");
                row["acme Remit"] = tblDet.Compute("SUM([acme Remit])", "[Beneficiary Name] = '** SUB TOTAL:'");
                tblDet.Rows.Add(row);
            }

            return dsFinal;
        }

        public static DataSet GetBankValidData(string yrmo)
        {
            Decimal startBal, totalDep, totalWD, endBal;
            Decimal detDepTot, detWDTot;
            string _type;
            DataSet dsResult = new DataSet();
            DataSet ds = new DataSet();
            ds = VWAImportDAL.getBOAData(yrmo);

            startBal = 0;
            totalDep = 0;
            totalWD = 0;
            endBal = 0;

            detDepTot = decimal.Parse((ds.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND ([Type] = 'DEP' OR [Type] = 'MSCC')")).ToString(), NumberStyles.Currency);
            detWDTot = decimal.Parse((ds.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND ([Type] <> 'DEP' AND [Type] <> 'MSCC')")).ToString(), NumberStyles.Currency);
            
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _type = dr["Type"].ToString();

                switch (_type)
                {
                    case "Start Balance":
                        startBal = decimal.Parse(dr["Amount"].ToString(), NumberStyles.Currency);
                        break;
                    case "Total Deposits":
                        totalDep = decimal.Parse(dr["Amount"].ToString(), NumberStyles.Currency);
                        break;
                    case "Total Withdrawls":
                        totalWD = (-1) * decimal.Parse(dr["Amount"].ToString(), NumberStyles.Currency);
                        break;
                    case "End Balance":
                        endBal = (-1) * decimal.Parse(dr["Amount"].ToString(), NumberStyles.Currency);
                        break;
                }
            }

            dsResult.Tables.Add(GetBankSumData(startBal, totalDep, totalWD, endBal));
            dsResult.Tables.Add(GetBankDepSum(totalDep, detDepTot));
            dsResult.Tables.Add(GetBankDepDet(yrmo, detDepTot));
            dsResult.Tables.Add(GetBankWDSum(totalWD, detWDTot));
            dsResult.Tables.Add(GetBankWDDet(yrmo, detWDTot));

            return dsResult;
        }

        public static DataSet GetCaseData(string _cnum)
        {
            DataSet dsResult = new DataSet();

            dsResult.Tables.Add(GetCaseSumData(VWA_DAL.getContractTabDetails(_cnum)));
            dsResult.Tables.Add(VWA_DAL.getFinanceTab1Details(_cnum));
            dsResult.Tables.Add(VWA_DAL.getFinanceTab2Details(_cnum));
            dsResult.Tables.Add(VWA_DAL.getHistory_4Cases(_cnum));

            return dsResult;
        }

        public static string GetFileName(string source, string yrmo)
        {
            string _filename = "";

            switch (source)
            {
                case "BankDisp":
                    _filename = yrmo + "_vwaboa_validation.xls";
                    break;
                case "RemitDisp":
                    _filename = yrmo + "_vwaremit_discrepancy.xls";
                    break;                
                case "RemitInput":
                    _filename = yrmo + "_vwaremit.xls";
                    break;
                case "TranMis":
                    _filename = yrmo + "_VWA_DELETED.xls";
                    break;
                case "TranClient":
                    _filename = yrmo + "_VWA_CLIENT.xls";
                    break;
                case "TranGrp":
                    _filename = yrmo + "_VWA_GROUP.xls";
                    break;
                case "TranStat":
                    _filename = yrmo + "_VWA_STATUS.xls";
                    break;
                case "TranACCTG":
                    _filename = yrmo + "_VWA_ACCTG.xls";
                    break;
                case "TranAging":
                    _filename = VWA.GetYRMO(DateTime.Today) + "_VWA_AGING.xls";
                    break;
                case "TranStatCtr":
                    _filename = yrmo + "_VWA_STATUSCTR_SUM.xls";
                    break;
                case "BalSumAudit":
                    _filename = yrmo + "_VWA_BALSUM.xls";
                    break;
                case "BalSum":
                    _filename = yrmo + "_VWA_BAL_XSUM.xls";
                    break;
                case "CaseInfo":
                    //yrmo is Contract number in this case
                    _filename = DateTime.Today.ToString("MMddyyyy") + "_CaseInfo_" + yrmo + ".xls";
                    break;
            }

            return _filename;
        }

        public static void GenerateReport(string _source, string _yrmo)
        {
            string _filename, file_extn, _content, _contenttype;
            _filename = GetFileName(_source, _yrmo);
            file_extn = _filename.Substring(_filename.Length - 4);
            _content = GetReportContent(_source, _filename, _yrmo);
            _contenttype = "application/vnd.ms-excel";            

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + _filename);
            HttpContext.Current.Response.ContentType = _contenttype;
            HttpContext.Current.Response.Write(_content);
            HttpContext.Current.Response.End();
        }

        public static void GenerateBalReport(string _source, string _yrmo, DataTable dsAuditSum1, DataTable dsAuditSum2, DataTable dsSum)
        {
            string _filename, file_extn, _content, _contenttype;
            _filename = GetFileName(_source, _yrmo);
            file_extn = _filename.Substring(_filename.Length - 4);
            _content = GetBalReportContent(_source, _filename, _yrmo, dsAuditSum1, dsAuditSum2, dsSum);
            _contenttype = "application/vnd.ms-excel";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + _filename);
            HttpContext.Current.Response.ContentType = _contenttype;
            HttpContext.Current.Response.Write(_content);
            HttpContext.Current.Response.End();
        }

        public static void SavePrintFiles(string _category, string yrmo)
        {
            string[] reports = GetReportCodes(_category);
            Page page = HttpContext.Current.Handler as Page;
            string _content, _file, _filepath, _filename;
            Boolean _checkedPrint, _checkedSave;
            int _seq = 0;
            StreamWriter sw;
            string agedDays;

            foreach (string report in reports)
            {
                _checkedPrint = VWA_ExportDAL.checkedPrint(report);
                _checkedSave = VWA_ExportDAL.checkedSave(report);

                if (!_checkedSave && !_checkedPrint)
                    continue;

                _filename = GetFileName(report, yrmo);

                if (report.Equals("TranAging")) 
                {
                    agedDays = HttpContext.Current.Session["VWAAgingDays"].ToString(); 
                    _content = GetReportContent(report, _filename, agedDays); 
                }
                else 
                { 
                    _content = GetReportContent(report, _filename, yrmo); 
                }
             
                _filepath = VWA_ExportDAL.GetFilePath(report);

                _file = _filepath + _filename;

                if (_checkedSave || _checkedPrint)
                {
                    if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _content.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                }

                if (_checkedPrint)
                {
                  
                    if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintExcel"))
                        page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintExcel", "PrintExcel('" + _file.Replace("\\", "\\\\") + "');", true);
                    
                    if (!_checkedSave)
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                }

                _seq++;
            }
        }

        public static void SavePrintBalFiles(string _category, string yrmo, DataTable dsAuditSum1, DataTable dsAuditSum2, DataTable dsSum)
        {
            string[] reports = GetReportCodes(_category);
            Page page = HttpContext.Current.Handler as Page;
            string _content, _file, _filepath, _filename;
            Boolean _checkedPrint, _checkedSave;
            int _seq = 0;
            StreamWriter sw;

            foreach (string report in reports)
            {
                _checkedPrint = VWA_ExportDAL.checkedPrint(report);
                _checkedSave = VWA_ExportDAL.checkedSave(report);

                if (!_checkedSave && !_checkedPrint)
                    continue;

                _filename = GetFileName(report, yrmo);
                _content = GetBalReportContent(report, _filename, yrmo, dsAuditSum1, dsAuditSum2, dsSum);
                _filepath = VWA_ExportDAL.GetFilePath(report);

                _file = _filepath + _filename;

                if (_checkedSave || _checkedPrint)
                {
                   if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _content.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                }

                if (_checkedPrint)
                {

                    if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintExcel"))
                        page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintExcel", "PrintExcel('" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!_checkedSave)
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                }

                _seq++;
            }
        }

        public static string[] GetReportCodes(string source)
        {
            string[] _reports;

            switch (source)
            {
                case "BankStat":
                    _reports = new string[] { "BankDisp" };
                    break;
                case "Remit":
                    _reports = new string[] { "RemitDisp", "RemitInput" };
                    break;
                case "Transaction":
                    _reports = new string[] { "TranMis", "TranClient", "TranGrp", "TranStat", "TranACCTG", "TranAging", "TranStatCtr" };
                    break;
                case "Balance":
                    _reports = new string[] { "BalSumAudit", "BalSum" };
                    break;
                case "Individual Case":
                    _reports = new string[] { "CaseInfo" };
                    break;
                default:
                    _reports = null;
                    break;
            }

            return _reports;
        }

        public static string GetReportContent(string _source, string _filename, string yrmo)
        {
            DataSet ds = new DataSet(); 
            DataSet dsTemp = new DataSet(); 
            string[] sheetnames;
            string[][] colsFormat, titles;
            int[][] colsWidth;
            string _content = "";
            DataRow rowNew;

            switch (_source)
            {
                case "RemitDisp":
                    sheetnames = new string[] { "Remit_Discrepancy" };
                    titles = new string[][] { new string[] { "VWA Remittance Discrepancy Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "number", "string", "currency", "currency", "currency", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 65, 140, 65, 65, 65, 65, 65, 65, 65, 65, 50 } };

                    ds = VWAImportDAL.GetRemitDiscData(yrmo); 

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "RemitInput":
                    sheetnames = new string[] { "Remit_Summary", "Remit_Detail" };
                    titles = new string[][] { new string[] { "VWA Remittance Summary Report for YRMO : " + yrmo }, new string[] { "VWA Remittance Detail Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "string", "currency", "currency", "currency", "percent", "string" }, new string[] { "string", "string", "string", "string", "number", "string", "currency", "currency", "currency", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 120, 65, 65, 65, 65, 40 }, new int[] { 80, 130, 65,  65, 65, 65, 65, 65, 65, 65, 40} };

                    dsTemp = GetRemitSumData(yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = GetRemitDetData(yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, "!    VWA Fees(VWA) plus Due Client(acme) does not equals Paid VWA(Total)");
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "BankDisp":                    
                    ds = GetBankValidData(yrmo);
                    _content = VWAExcelReport.BankDispExcelRpt(ds, yrmo);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranMis":
                    sheetnames = new string[] { "VWA_DELETED" };
                    titles = new string[][] { new string[] { "Missing Transactions Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "string", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency"} };
                    colsWidth = new int[][] { new int[] { 50, 80, 45, 80, 40, 150, 65, 130, 65, 130, 45, 65, 65, 65, 65, 65, 65, 65, 65 } };

                    ds = VWA_ExportDAL.GetTranOutputData("TranMis", yrmo);

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranClient":
                    sheetnames = new string[] { "VWA_CLIENT_SUM", "VWA_CLIENT_DET" };
                    titles = new string[][] { new string[] { "Change in Client Summary Report for YRMO : " + yrmo }, new string[] { "Change in Client Detail Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "number" }, new string[] { "number", "string", "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 50, 80, 50, 80, 45 }, new int[] { 50, 80, 50, 80, 45, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 65, 65, 65, 65 } };

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranClientSUM", yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranClientDET", yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranGrp":
                    sheetnames = new string[] { "VWA_GROUP_SUM", "VWA_GROUP_DET" };
                    titles = new string[][] { new string[] { "Change in Group Summary Report for YRMO : " + yrmo }, new string[] { "Change in Group Detail Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "number" }, new string[] { "number", "string", "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 40, 80, 40, 80, 45 }, new int[] { 40, 80, 40, 80, 50, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 65, 65, 65, 65 } };

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranGrpSUM", yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranGrpDET", yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranStat":                    
                    sheetnames = new string[] { "VWA_STATUS_SUM", "VWA_STATUS_OPEN", "VWA_STATUS_CLOSED" };
                    titles = new string[][] { new string[] { "Open, Closed, Pended Counts Summary Report for YRMO : " + yrmo }, new string[] { "New Cases Opened Detail Report for YRMO : " + yrmo }, new string[] { "New Cases Closed Detail Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "number", "number", "number" }, new string[] { "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency" }, new string[] { "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 50, 80, 45, 80, 50, 50, 50 }, new int[] { 50, 80, 45, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 65, 65, 65, 65 }, new int[] { 50, 80, 45, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 65, 65, 65, 65 } };

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranStatSUM", yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    rowNew = dsTemp.Tables[0].NewRow();
                    rowNew["Group Name"] = "TOTAL:";
                    rowNew["Open Counter"] = dsTemp.Tables[0].Compute("SUM([Open Counter])", String.Empty);
                    rowNew["Closed Counter"] = dsTemp.Tables[0].Compute("SUM([Closed Counter])", String.Empty);
                    rowNew["Pending Counter"] = dsTemp.Tables[0].Compute("SUM([Pending Counter])", String.Empty);
                    dsTemp.Tables[0].Rows.Add(rowNew);
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranStatOpenDET", yrmo); dsTemp.Tables[0].TableName = "Detail1Table";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "Detail1TableF"; dsTemp.Clear();
                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranStatCloseDET", yrmo); dsTemp.Tables[0].TableName = "Detail2Table";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "Detail2TableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranACCTG":
                    sheetnames = new string[] { "VWA_ACCTG_SUM", "VWA_ACCTG_DET" };
                    titles = new string[][] { new string[] { "Financial Activity Summary Report for YRMO : " + yrmo }, new string[] { "Financial Activity Detail Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "number", "currency", "currency", "currency" }, new string[] { "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency", "currency", "currency", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 50, 80, 45, 80, 45, 65, 65, 65 }, new int[] { 50, 80, 45, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 70, 70, 70, 70, 70, 70, 70, 150 } };

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranACCTGSUM", yrmo); dsTemp.Tables[0].TableName = "SummaryTable";

                    rowNew = dsTemp.Tables[0].NewRow();
                    rowNew["Group Name"] = "TOTAL:";
                    rowNew["Counter"] = dsTemp.Tables[0].Compute("SUM([Counter])", String.Empty);
                    rowNew["Recovery Amount"] = dsTemp.Tables[0].Compute("SUM([Recovery Amount])", String.Empty);
                    rowNew["Total Fees"] = dsTemp.Tables[0].Compute("SUM([Total Fees])", String.Empty);
                    rowNew["Net Amount"] = dsTemp.Tables[0].Compute("SUM([Net Amount])", String.Empty);
                    dsTemp.Tables[0].Rows.Add(rowNew);

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranACCTGDET", yrmo); dsTemp.Tables[0].TableName = "DetailTable";                    

                    rowNew = dsTemp.Tables[0].NewRow();
                    rowNew["Recovery Date"] = "TOTAL:";
                    rowNew["Recovered Amount Current"] = dsTemp.Tables[0].Compute("SUM([Recovered Amount Current])", String.Empty);
                    rowNew["TotFees Current"] = dsTemp.Tables[0].Compute("SUM([TotFees Current])", String.Empty);
                    rowNew["Net Amount Current"] = dsTemp.Tables[0].Compute("SUM([Net Amount Current])", String.Empty);
                    rowNew["Benefit Amount Total"] = dsTemp.Tables[0].Compute("SUM([Benefit Amount Total])", String.Empty);
                    rowNew["Recovered Amount Total"] = dsTemp.Tables[0].Compute("SUM([Recovered Amount Total])", String.Empty);
                    rowNew["TotFees Total"] = dsTemp.Tables[0].Compute("SUM([TotFees Total])", String.Empty);
                    rowNew["Net Amount Total"] = dsTemp.Tables[0].Compute("SUM([Net Amount Total])", String.Empty);
                    dsTemp.Tables[0].Rows.Add(rowNew);

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranAging":
                    if(yrmo == null || yrmo.Equals(String.Empty)) {yrmo = "0";}
                    sheetnames = new string[] { "VWA_AGING_SUM", "VWA_AGING_DET" };
                    titles = new string[][] { new string[] { "Aging Summary Report on " + DateTime.Today.ToString("MM/dd/yyyy") + " with aging days - " + yrmo }, new string[] { "Aging Detail Report on " + DateTime.Today.ToString("MM/dd/yyyy") + " with aging days - " + yrmo } };
                    colsFormat = new string[][] { new string[] { "number", "string", "number", "string", "number" }, new string[] { "number", "string", "number", "string", "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "currency", "currency", "currency", "currency", "number", "string" } };
                    colsWidth = new int[][] { new int[] { 50, 80, 45, 80, 50 }, new int[] { 50, 80, 45, 80, 65, 130, 65, 130, 45, 40, 150, 65, 65, 65, 65, 65, 65, 65, 65, 45, 75 } };

                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranAgingSUM", yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = VWA_ExportDAL.GetTranOutputData("TranAgingDET", yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "TranStatCtr":
                    sheetnames = new string[] { "VWA_STATUSCTR_SUM" };
                    titles = new string[][] { new string[] { "Status Counter Report for YRMO : " + yrmo } };
                    colsFormat = new string[][] { new string[] { "string", "string", "number" } };
                    colsWidth = new int[][] { new int[] { 40, 200, 50 } };

                    ds = VWA_ExportDAL.GetTranOutputData("TranStatCtr", yrmo);

                    rowNew = ds.Tables[0].NewRow();
                    rowNew["Status Description"] = "TOTAL:";
                    rowNew["Counter"] = ds.Tables[0].Compute("SUM([Counter])", String.Empty);
                    ds.Tables[0].Rows.Add(rowNew);
                    
                    _content = VWAExcelReport.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "CaseInfo":
                    //yrmo is Contract number in this case
                    ds = GetCaseData(yrmo);
                    _content = VWAExcelReport.CaseExcelRpt(ds, yrmo);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;
            }

            return _content;
        }

        public static string GetBalReportContent(string _source, string _filename, string yrmo, DataTable dsAuditSum1, DataTable dsAuditSum2, DataTable dsSum)
        {
            string[] sheetnames, titles;
            string[][] colsFormat;
            int[][] colsWidth;
            string _content = "";
            HRAExcelReport xlObj = new HRAExcelReport();

            switch (_source)
            {
                case "BalSumAudit":
                    DataSet ds1 = new DataSet(); ds1.Clear();
                    DataSet ds2 = new DataSet(); ds2.Clear();
                    sheetnames = new string[] { "VWA_BALSUM" };
                    titles =  new string[] { "Balance Audit Report for YRMO : " + yrmo };
                    string[][] subtitles = { new string[] { "Bank Statement Summary" , "Remittance Information Summary" } };
                    colsFormat = new string[][] { new string[] { "string", "number", "currency", "currency" }, new string[] { "string", "number", "string", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 85, 85, 85, 85, 85 } };

                    dsAuditSum1.TableName = "BalAuditSum1";  ds1.Tables.Add(dsAuditSum1.Copy());
                    dsAuditSum2.TableName = "BalAuditSum2";  ds2.Tables.Add(dsAuditSum2.Copy());

                    _content = xlObj.ExcelXMLRpt(ds1, ds2, _filename, sheetnames, titles, subtitles, colsFormat, colsWidth);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;

                case "BalSum":
                    string sheetname = "VWA_BAL_XSUM";
                    titles = new string[] { "Balance Summary Report for YRMO : " + yrmo };
                    colsFormat = new string[][] { new string[] { "string", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 150, 90, 100 } };

                    _content = VWAExcelReport.ExcelXMLRpt(dsSum, _filename, sheetname, titles, colsFormat, colsWidth);
                    _content = VWA.replaceIllegalXMLCharacters(_content);
                    break;
            }

            return _content;
        }

        static DataTable GetBankSumData(Decimal startBal, Decimal totalDep, Decimal totalWD, Decimal endBal)
        {
            Decimal total1 = startBal + totalDep + totalWD;
            Decimal total2 = total1 + endBal;
            DataTable dsTable = new DataTable("Summary");
            DataRow row;
            DataColumn col;

            col = new DataColumn("Title"); dsTable.Columns.Add(col);
            col = new DataColumn("Value"); dsTable.Columns.Add(col);

            row = dsTable.NewRow();
            row["Title"] = "Starting Balance:";
            row["Value"] = startBal;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Total Deposits:";
            row["Value"] = totalDep;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Total Withdrawls:";
            row["Value"] = totalWD;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Total:";
            row["Value"] = total1;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Ending Balance:";
            row["Value"] = (-1) * endBal;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Difference:";
            row["Value"] = total2;
            dsTable.Rows.Add(row);

            return dsTable;
        }

        static DataTable GetCaseSumData(DataTable dt)
        {
            DataTable dtOut = new DataTable("Contract");
            DataRow row;
            DataColumn col;

            col = new DataColumn("Title"); dtOut.Columns.Add(col);
            col = new DataColumn("Value"); dtOut.Columns.Add(col);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    row = dtOut.NewRow(); row["Title"] = "Contract#:";       row["Value"] = dr["Contract#"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Client:";          row["Value"] = dr["Client"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "SSN:";             row["Value"] = dr["SSN"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Group:";           row["Value"] = dr["Group"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Name:";            row["Value"] = dr["Name"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Patient:";         row["Value"] = dr["Patient"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Relationship:";    row["Value"] = dr["Relationship"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Disabled:";        row["Value"] = dr["Disabled"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Statuscd:";        row["Value"] = dr["Statuscd"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Accident Date:";   row["Value"] = dr["Accident Date"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Recovery Date:";   row["Value"] = dr["Recovery Date"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Open Date:";       row["Value"] = dr["Open Date"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Close Date:";      row["Value"] = dr["Close Date"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Last Update:";     row["Value"] = dr["Last Update"].ToString(); dtOut.Rows.Add(row);
                    row = dtOut.NewRow(); row["Title"] = "Notes:";           row["Value"] = dr["Notes"].ToString(); dtOut.Rows.Add(row);                      
                }
            }

            return dtOut;
        }

        static DataTable GetBankDepSum(Decimal SumDepTot, Decimal DetDepTot)
        {
            Decimal diffAmt = Math.Abs(SumDepTot - DetDepTot);
            DataTable dsTable = new DataTable("DepSum");
            DataRow row;
            DataColumn col;

            col = new DataColumn("Title"); dsTable.Columns.Add(col);
            col = new DataColumn("Value"); dsTable.Columns.Add(col);

            row = dsTable.NewRow();
            row["Title"] = "Summary Deposits Amount:";
            row["Value"] = SumDepTot;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Detail Deposits Amount:";
            row["Value"] =  DetDepTot;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Difference:";
            row["Value"] = diffAmt;
            dsTable.Rows.Add(row);

            return dsTable;
        }

        static DataTable GetBankDepDet(string yrmo, Decimal DetDepTot)
        {
            DataTable dt = VWAImportDAL.getBOADepDet(yrmo);
            DataRow row = dt.NewRow();
            row["Type"] = "Total:";
            row["Amount"] = DetDepTot;
            dt.Rows.Add(row);

            return dt;
        }

        static DataTable GetBankWDSum(Decimal SumWDTot, Decimal DetWDTot)
        {
            Decimal diffAmt = Math.Abs((-1 * SumWDTot) - DetWDTot); //12_26
            DataTable dsTable = new DataTable("WDSum");
            DataRow row;
            DataColumn col;

            col = new DataColumn("Title"); dsTable.Columns.Add(col);
            col = new DataColumn("Value"); dsTable.Columns.Add(col);

            row = dsTable.NewRow();
            row["Title"] = "Summary Withdrawals Amount:";
            row["Value"] = (-1) * SumWDTot;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Detail Withdrawals Amount:";
            row["Value"] = DetWDTot;
            dsTable.Rows.Add(row);

            row = dsTable.NewRow();
            row["Title"] = "Difference:";
            row["Value"] = diffAmt;
            dsTable.Rows.Add(row);

            return dsTable;
        }

        static DataTable GetBankWDDet(string yrmo, Decimal DetWDTot)
        {
            DataTable dt = VWAImportDAL.getBOAWDDet(yrmo);
            DataRow row = dt.NewRow();
            row["Type"] = "Total:";
            row["Amount"] = DetWDTot;
            dt.Rows.Add(row);

            return dt;
        }
    }
}
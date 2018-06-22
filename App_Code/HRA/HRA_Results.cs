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
using System.Collections.Generic;
using EBA.Desktop;

namespace EBA.Desktop.HRA
{
    public class HRA_Results
    {
        public HRA_Results()
        {

        }

        public static string GetReportContent(string _source, string _filename, string yrmo)
        {
            HRASofoDAL sobj = new HRASofoDAL();
            HRAEligFile eobj = new HRAEligFile();
            HRAAdminBill hobj = new HRAAdminBill();
            HRAExcelReport xlObj = new HRAExcelReport();
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            string[] sheetnames, titles, YRMOs;            
            string[][] colsFormat, colsName;
            int[][] colsWidth;
            string _content = "";            

            switch (_source)
            {
                case "Recon":
                    sheetnames = new string[] { "Summary", "Detail" };
                    titles = new string[] { "HRA Reconciliation Summary Report for YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString(), "HRA Reconciliation Detail Report for YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString()};
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Putnam Amount", "Wageworks Amount", "Adjustment Amount", "Current Diff Amount", "Total Diff Amount", "Putnam Record", "Wageworks Record" }, new string[] { "SSN", "Last Name", "First Name", "Transaction", "Transaction Date", "Putnam Amount", "Wageworks Amount", "Adjustment Amount" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "currency", "currency", "currency", "currency", "currency", "number", "number" }, new string[] { "number", "string", "string", "string", "string", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 100, 100, 65, 65, 65, 65, 65, 45, 60 }, new int[] { 55, 100, 100, 105, 65, 65, 65, 65 } };

                    dsTemp = HRA_ReconDAL.GetReconData(yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = HRA_ReconDAL.GetDetailReconData(yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();
                    
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName,  colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "CF":                    
                    sheetnames = new string[] { "Carry_Forwards_" + yrmo };
                    titles = new string[] { "HRA Detail Carry Forward Report for YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString()};
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Transaction", "Transaction Date", "Wageworks Amount" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 100, 100, 105, 65, 65 } };

                    ds = HRA_ReconDAL.GetCFData(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName,  colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "CFnotCleared":
                    sheetnames = new string[] { "Prev_Carry_Forwards_" + yrmo };
                    titles = new string[] { "HRA Detail Report of Carry Forwards from prior month that did not clear in YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString()};
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Transaction", "Transaction Date", "Wageworks Amount" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 100, 100, 105, 65, 65 } };

                    ds = HRA_ReconDAL.GetPrevCFRptData(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName,  colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "Transaction":
                    sheetnames = new string[] { "Summary", "Detail" };
                    titles = new string[] { "HRA Transaction Summary Report for YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString(), "HRA Transaction Detail Report for YRMO : " + yrmo + " Date Report Run : " + DateTime.Now.ToString()};
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Putnam Amount", "Putnam Adj Amount", "Current Wageworks", "Total Diff Amount", "Carry Forward" }, new string[] { "SSN", "Last Name", "First Name", "Transaction", "Transaction Date", "Putnam Amount", "PutnamAdj Amount", "Current Wageworks", "Carry Forward" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "currency", "currency", "currency", "currency", "currency" }, new string[] { "number", "string", "string", "string", "string", "currency", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 100, 100, 65, 65, 65, 65, 65 }, new int[] { 55, 100, 100, 105, 65, 65, 65, 65, 65 } };

                    dsTemp = HRA_ReconDAL.GetTransData(yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = HRA_ReconDAL.GetDtlTransData(yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "SOFO":
                    sheetnames = new string[] { "Summary", "Detail" };
                    titles = new string[] { "Putnam Summary of Fund Operations Reconciliation for YRMO: " + yrmo, "HRA Putnam Balance Detail report for YRMO: " + yrmo };
                    colsName = new string[][] { new string[] { "Title", "Value" }, new string[] { "SSN", "Last Name", "First Name", "Transaction", "Transaction Date", "Putnam Amount" } };
                    colsFormat = new string[][] { new string[] { "string", "string" }, new string[] { "number", "string", "string", "string", "string", "currency"} };
                    colsWidth = new int[][] { new int[] {100, 100 }, new int[] { 55, 100, 100, 105, 65, 65} };

                    dsTemp = sobj.getSOFOReconData(yrmo); dsTemp.Tables[0].TableName = "SummaryTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "SummaryTableF"; dsTemp.Clear();
                    dsTemp = sobj.getBalanceDtl(yrmo); dsTemp.Tables[0].TableName = "DetailTable";
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[1].TableName = "DetailTableF"; dsTemp.Clear();

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "Elig":
                    _content = eobj.GetEligFile();
                    break;
                
                case "EligAudit":                    
                    sheetnames = new string[] { "HRAAUDIT" };
                    titles = new string[] { "HRA Eligibility Audit Report" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Name", "SubCtr", "CodeID", "Code Desc", "Value" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "number", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 130, 40, 40, 100, 140 } };

                    ds = eobj.GetAuditFile();

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligTermAudit":                    
                    sheetnames = new string[] { "HRAAUDITR" };
                    titles = new string[] { "Report of Pilots who have Retired/Terminated/Died to date" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Name", "Birth Date", "Age", "Status", "Status Date", "Modify Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "number", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 140, 65, 40, 55, 65, 77 } };

                    ds = HRAOperDAL.GetEligAuditData("HRAAUDITR", String.Empty);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligAddrChg":
                    sheetnames = new string[] { "HRA_Audit_ADDR" };
                    titles = new string[] { "Report of Pilots who have Address Changes from the last time Eligibilty file is Generated" };
                    colsName = new string[][] { new string[] { "EE#", "Name", "Prior Addr1", "Prior Addr2", "Prior City", "Prior State", "Prior Zip", "New Addr1", "New Addr2", "New City", "New State", "New Zip" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 100, 50, 65, 50, 50, 100, 50, 65, 50, 50} };

                    ds = HRAOperDAL.GetChgEligData("ChgEligAddr");

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligStatChg":
                    sheetnames = new string[] { "HRA_Retire" };
                    titles = new string[] { "Report of Pilots who have Status Changes from the last time Eligibilty file is generated" };
                    colsName = new string[][] { new string[] { "EE#", "Name", "Status Code", "Status Date" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 40, 65 } };

                    ds = HRAOperDAL.GetChgEligData("ChgEligStat");

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligNoBen":
                    sheetnames = new string[] { "HRA_NoBen" };
                    titles = new string[] { "Report of Pilots who died with no beneficiaries" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Name", "Death Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 140, 65 } };

                    ds = HRAOperDAL.GetEligAuditData("NoBen", String.Empty);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligBen24":
                    sheetnames = new string[] { "HRA_Ben24" };
                    titles = new string[] { "Report of Benificiaries who turned 24" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Last Name", "First Name", "HRA Balance" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 100, 100, 65 } };

                    ds = HRAOperDAL.GetEligAuditData("Ben24", String.Empty);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligNoBenOwner":
                    sheetnames = new string[] { "HRA_NoBenOwner" };
                    titles = new string[] { "Report of Pilots who died with no owner verified" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Last Name", "First Name", "Print Date", "HRA Balance", "Death Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "string", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 100, 100, 65, 65, 65 } };

                    ds = HRAOperDAL.GetEligAuditData("NoBenOwner", String.Empty);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligChildBenLtr":
                    sheetnames = new string[] { "HRA_childBenLtr60" };
                    titles = new string[] { "Report on Child Beneficiary letter generated > 60 days ago where a validation date is not been entered" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Last Name", "First Name", "HRA Balance", "Letter Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "currency", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 100, 100, 65, 65 } };

                    ds = HRAOperDAL.GetEligAuditData("ChildBenLtr60", String.Empty);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "EligDeathsXdays":
                    sheetnames = new string[] { "HRA_deaths" + yrmo + "days" };
                    titles = new string[] { "Report on beneficiary details of pilots who died " + yrmo + " days from today" };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Last Name", "First Name", "Ben Order", "Ben DOB", "Relation", "Validation Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "number", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 100, 100, 40, 65, 50, 65 } };

                    ds = HRAOperDAL.GetEligAuditData("DeathsXdays", yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminRecon":
                    DataSet dsWgwk = new DataSet(); dsWgwk.Clear();
                    DataSet dsPtnm = new DataSet(); dsPtnm.Clear();
                    sheetnames = new string[] { "Reconciliation_" + yrmo };
                    titles = new string[] { "HRA Admin Invoice Reconciliation Report for Quarter/Year : " + yrmo };
                    string[][] subtitles = { new string[] { "Recon of active HRA(s) managed by Wageworks", "Recon of HRA records that have balance with Putnam" } };
                    colsFormat = new string[][] { new string[] { "string", "number", "number", "number", "number", "number", "number", "number", "currency", "currency", "currency"}, new string[] { "string", "number", "number", "number", "currency", "currency", "currency" } };
                    colsWidth = new int[][] { new int[] { 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65 } };

                    dsWgwk = hobj.GetWageworkSummaryRecon(yrmo);
                    dsPtnm = hobj.GetPutnamSummaryRecon(yrmo);
                    
                    _content = xlObj.ExcelXMLRpt(dsWgwk, dsPtnm, _filename, sheetnames, titles, subtitles, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminDispAP":
                    DataSet ds1 = new DataSet(); ds1.Clear();
                    DataSet ds2 = new DataSet(); ds2.Clear();                    
                    YRMOs = HRA.GetYRMOs(yrmo);
                    DateTime _date1 = HRA.GetLastDayofYRMO(YRMOs[0]);
                    DateTime _date2 = HRA.GetLastDayofYRMO(YRMOs[1]);
                    DateTime _date3 = HRA.GetLastDayofYRMO(YRMOs[2]);
                    sheetnames = new string[] { YRMOs[0], YRMOs[1], YRMOs[2] };
                    titles = new string[] { "HRAAUDITR to Putnam Discrepancy Report for YRMO : " + YRMOs[0], "HRAAUDITR to Putnam Discrepancy Report for YRMO : " + YRMOs[1], "HRAAUDITR to Putnam Discrepancy Report for YRMO : " + YRMOs[2] };
                    string[][] subtitles2 = { new string[] { "Mismatches with Modify Date <= " + _date1.ToString("MM/dd/yyyy"), "Mismatches with Modify Date > " + _date1.ToString("MM/dd/yyyy") }, new string[] { "Mismatches with Modify Date <= " + _date2.ToString("MM/dd/yyyy"), "Mismatches with Modify Date > " + _date2.ToString("MM/dd/yyyy") }, new string[] { "Mismatches with Modify Date <= " + _date3.ToString("MM/dd/yyyy"), "Mismatches with Modify Date > " + _date3.ToString("MM/dd/yyyy") } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "number", "string", "string" }, new string[] { "number", "number", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 100, 100, 55, 65, 65 }, new int[] { 55, 55, 100, 100, 55, 65, 65 }, new int[] { 55, 55, 100, 100, 55, 65, 65 } };

                    ds1 = hobj.GetRecon_HRAAUDITR_PartData1(yrmo);
                    ds2 = hobj.GetRecon_HRAAUDITR_PartData2(yrmo);                    

                    _content = xlObj.ExcelXMLRpt(ds1, ds2, _filename, sheetnames, titles, subtitles2, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminDispPA":
                    YRMOs = HRA.GetYRMOs(yrmo);
                    sheetnames = new string[] { YRMOs[0], YRMOs[1], YRMOs[2] };
                    titles = new string[] { "Putnam to HRAAUDITR Discrepancy Report for YRMO : " + YRMOs[0], "Putnam to HRAAUDITR Discrepancy Report for YRMO : " + YRMOs[1], "Putnam to HRAAUDITR Discrepancy Report for YRMO : " + YRMOs[2] };
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Birth Date", "Term Date", "Total Asset Balance" }, new string[] { "SSN", "Last Name", "First Name", "Birth Date", "Term Date", "Total Asset Balance" }, new string[] { "SSN", "Last Name", "First Name", "Birth Date", "Term Date", "Total Asset Balance" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string", "string", "currency" }, new string[] { "number", "string", "string", "string", "string", "currency" }, new string[] { "number", "string", "string", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 55, 100, 100, 65, 65, 65 }, new int[] { 55, 100, 100, 65, 65, 65 }, new int[] { 55, 100, 100, 65, 65, 65 } };

                    ds = hobj.GetRecon_PartData_HRAAUDITR(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminDispWP":
                    YRMOs = HRA.GetYRMOs(yrmo);
                    sheetnames = new string[] { YRMOs[0], YRMOs[1], YRMOs[2] };
                    titles = new string[] { "Wageworks to Putnam Discrepancy Report for YRMO : " + YRMOs[0], "Wageworks to Putnam Discrepancy Report for YRMO : " + YRMOs[1], "Wageworks to Putnam Discrepancy Report for YRMO : " + YRMOs[2] };
                    colsName = new string[][] { new string[] { "Last Name", "First Name", "SSN", "" }, new string[] { "Last Name", "First Name", "SSN", "" }, new string[] { "Last Name", "First Name", "SSN", "" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "number", "string" }, new string[] { "string", "string", "number", "string" }, new string[] { "string", "string", "number", "string" } };
                    colsWidth = new int[][] { new int[] { 100, 100, 65, 92 }, new int[] { 100, 100, 65, 92 }, new int[] { 100, 100, 65, 92 } };

                    ds = hobj.GetRecon_WgwkInv_PartData(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminDispPW":
                    YRMOs = HRA.GetYRMOs(yrmo);
                    sheetnames = new string[] { YRMOs[0], YRMOs[1], YRMOs[2] };
                    titles = new string[] { "Putnam to Wageworks Discrepancy Report for YRMO : " + YRMOs[0], "Putnam to Wageworks Discrepancy Report for YRMO : " + YRMOs[1], "Putnam to Wageworks Discrepancy Report for YRMO : " + YRMOs[2] };
                    colsName = new string[][] { new string[] { "Last Name", "First Name", "SSN", "Participant Status Description", "Birth Date", "Term Date", "Total Asset Balance" }, new string[] { "Last Name", "First Name", "SSN", "Participant Status Description", "Birth Date", "Term Date", "Total Asset Balance" }, new string[] { "Last Name", "First Name", "SSN", "Participant Status Description", "Birth Date", "Term Date", "Total Asset Balance" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "number", "string", "string", "string", "currency" }, new string[] { "string", "string", "number", "string", "string", "string", "currency" }, new string[] { "string", "string", "number", "string", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 100, 100, 55, 140, 65, 65, 65 }, new int[] { 100, 100, 55, 140, 65, 65, 65 }, new int[] { 100, 100, 55, 140, 65, 65, 65 } };

                    ds = hobj.GetRecon_PartData_WgwkInv(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "AdminWPnoBal":
                    DataSet ds3 = new DataSet();
                    YRMOs = HRA.GetYRMOs(yrmo);
                    sheetnames = new string[] { YRMOs[0], YRMOs[1], YRMOs[2] };
                    titles = new string[] { "Participant(s) in Wageworks Report with paid out in Putnam Report for YRMO : " + YRMOs[0], "Participant(s) in Wageworks Report with paid out in Putnam Report for YRMO : " + YRMOs[1], "Participant(s) in Wageworks Report with paid out in Putnam Report for YRMO : " + YRMOs[2] };
                    colsFormat = new string[][] { new string[] { "string", "string", "number", "currency" }, new string[] { "string", "string", "number", "currency" } };
                    colsWidth = new int[][] { new int[] { 100, 100, 55, 65 }, new int[] { 100, 100, 55, 65 }, new int[] { 100, 100, 55, 65 } };
                    string[][] subtitles3 = { new string[] { "Participants with Zero Balance", "Participants with status 'Terminated, Paid Out' and have balance" }, new string[] { "Participants with Zero Balance", "Participants with status 'Terminated, Paid Out' and have balance" }, new string[] { "Participants with Zero Balance", "Participants with status 'Terminated, Paid Out' and have balance" } };
                    
                    ds = hobj.GetNoBal_WgwkInv_PartData(yrmo);
                    ds3 = hobj.GetPaidOut_WgwkInv_PartData(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, ds3, _filename, sheetnames, titles, subtitles3, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "HRAAUDITR":
                    sheetnames = new string[] { "HRAAUDITR" };
                    titles = new string[] { "HRAAUDITR Report used for Admin Bill Reconciliation in Quarter-Year: " + yrmo };
                    colsName = new string[][] { new string[] { "SSN", "EE#", "Name", "Birth Date", "Age", "Status", "Status Date", "Modify Date" } };
                    colsFormat = new string[][] { new string[] { "number", "number", "string", "string", "number", "string", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 55, 140, 65, 40, 55, 65, 77 } };

                    ds = HRAAdminDAL.GetAUDITR_Data(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "PtnmPartData":
                    sheetnames = new string[] { "Putnam Participant Data" };
                    titles = new string[] { "Data from Putnam Participant Data Input Report used for Admin Bill Reconciliation in Quarter-Year: " + yrmo };
                    colsName = new string[][] { new string[] { "SSN", "Last Name", "First Name", "Participant Status Description", "Birth Date", "Term Date", "Total Asset Balance" } };
                    colsFormat = new string[][] { new string[] { "number", "string", "string", "string", "string", "string", "currency" } };
                    colsWidth = new int[][] { new int[] { 60, 100, 100, 145, 65, 65, 65 } };

                    ds = HRAAdminDAL.GetPutnamParticipant_Data(yrmo);

                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;
            }

            return _content;
        }

        public static string GetFileName(string source, string yrmo)
        {
            string _filename = "";

            switch (source)
            {
                case "Recon":
                    _filename = "HRA_Recon_" + yrmo + ".xls";
                    break;
                case "CF":
                    _filename = "HRA_CF_" + yrmo + ".xls";
                    break;
                case "CFnotCleared":
                    _filename = "HRA_PrevCF_" + yrmo + ".xls";
                    break;
                case "Transaction":
                    _filename = "HRA_Transaction_" + yrmo + ".xls";
                    break;
                case "SOFO":
                    _filename = "HRA_Putnam_Balance_" + yrmo + ".xls";
                    break;
                case "Elig":
                    _filename = "HRA_" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                    break;               
                case "EligAudit":
                    _filename = "HRAAUDIT_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligTermAudit":
                    _filename = "HRAAUDITR_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligAddrChg":
                    _filename = "HRA_Audit_ADDR_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligStatChg":
                    _filename = "HRA_Retire_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligNoBen":
                    _filename = "HRA_NoBen_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligBen24":
                    _filename = "HRA_BEN24_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligNoBenOwner":
                    _filename = "HRA_NoBenOwner_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligChildBenLtr":
                    _filename = "HRA_childBenLtr60_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "EligDeathsXdays":
                    _filename = "HRA_deaths" + yrmo + "days_" + DateTime.Today.ToString("yyyyMMdd") + ".xls";
                    break;
                case "AdminRecon":
                    _filename = "Admin_Invoice_Recon_" + yrmo + ".xls";
                    break;
                case "AdminDispAP":
                    _filename = "HRAAUDITR_Putnam_Diff_" + yrmo + ".xls";
                    break;
                case "AdminDispPA":
                    _filename = "Putnam_HRAAUDITR_Diff_" + yrmo + ".xls";
                    break;
                case "AdminDispWP":
                    _filename = "Wageworks_Putnam_Diff_" + yrmo + ".xls";
                    break;
                case "AdminDispPW":
                    _filename = "Putnam_Wageworks_Diff_" + yrmo + ".xls";
                    break;
                case "AdminWPnoBal":
                    _filename = "Putnam_Wageworks_NoBalance_" + yrmo + ".xls";
                    break;
                case "HRAAUDITR":
                    _filename = "HRAAUDITR_" + yrmo + ".xls";
                    break;
                case "PtnmPartData":
                    _filename = "PutnamPartData_" + yrmo + ".xls";
                    break;
            }

            return _filename;
        }

        public static string[] GetReportCodes(string source)
        {
            string[] _reports;

            switch (source)
            {
                case "Reconciliation":
                    _reports = new string[] { "Recon", "CF", "CFnotCleared", "Transaction" };
                    break;
                case "SOFO":
                    _reports = new string[] {"SOFO"};
                    break;
                case "Admin Bill Validation":
                    _reports = new string[] { "AdminRecon", "AdminDispAP", "AdminDispPA", "AdminDispWP", "AdminDispPW", "AdminWPnoBal" };
                    break;
                default:
                    _reports = new string[] { "Elig", "EligTermAudit", "EligAudit", "EligAddrChg", "EligStatChg", "EligNoBen", "EligBen24", "EligNoBenOwner", "EligChildBenLtr", "EligDeathsXdays" };
                    break;
            }

            return _reports;
        }

        public static void GenerateReport(string _source, string _yrmo)
        {
            string _filename, file_extn,  _content, _contenttype;            
            _filename = GetFileName(_source, _yrmo);
            file_extn = _filename.Substring(_filename.Length - 4);
            _content = GetReportContent(_source, _filename, _yrmo);
            
            if (file_extn.Equals(".xls")) _contenttype = "application/vnd.ms-excel";
            else _contenttype = "text/plain";

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
            
            foreach (string report in reports)
            {
                _checkedPrint = HRA_ReconDAL.checkedPrint(_category, report);
                _checkedSave = HRA_ReconDAL.checkedSave(_category, report);

                if(!_checkedSave && !_checkedPrint)
                    continue;

                _filename = GetFileName(report, yrmo);
                _content = GetReportContent(report, _filename, yrmo);
                _filepath = HRA_ReconDAL.GetFilePath(_category, report); 
                                
                _file = _filepath + _filename;                

                if (_checkedSave || _checkedPrint)
                {
                    if (report.Equals("Elig"))
                    {
                        _content = _content.Replace("'", "\\'");
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveTextFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveTextFile", "SaveTextFile('" + _content.Replace(Environment.NewLine, "\\n") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    else
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _content.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                } 

                if (_checkedPrint)
                {
                    if (report.Equals("Elig"))
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    else
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintExcel"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintExcel", "PrintExcel('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    if (!_checkedSave)
                    {                   
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                }
                
                _seq++;
            }            
        }
        
    }
}
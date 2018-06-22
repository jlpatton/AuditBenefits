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
using System.Text.RegularExpressions;

namespace EBA.Desktop.HRA
{
    public class HRAAdminBill
    {
        public HRAAdminBill()
        {          
        }

        public int ImportPtnmInvoice(string filepath, string _qy)
        {
            string vendor, period;            
            int partcount = 0;
            decimal amount = 0;
            HRA hraobj = new HRA();
            HRAImportDAL iobj = new HRAImportDAL();
            FileStream fs = File.OpenRead(filepath); 
            StreamReader d = new StreamReader(fs);
            string pattern = @"^(?<Vendor>WageWorks|Mercer)\s+(?<Period>(Q[1234]\s+\d{4})|(\w{3}\-\d{2}))\s+(?<Participants>(((\d{1,3},)+\d{3})|\d+))\s+(?<Amount>\$(((\d{1,3},)+\d{3})|\d+)\.\d{2})$";            
            Match match1;
            string txtline = "";
            int records = 0;

            try
            {
                d.BaseStream.Seek(0, SeekOrigin.Begin);
                while (d.Peek() > -1)
                {
                    txtline = d.ReadLine();
                    match1 = Regex.Match(txtline, pattern);
                    if (match1.Success)
                    {
                        records++;
                        vendor = match1.Groups["Vendor"].Value;                        
                        period = match1.Groups["Period"].Value;
                        if (vendor.Equals("WageWorks"))
                        {
                            period = hraobj.GetYRMO(period);
                        }
                        partcount = int.Parse(match1.Groups["Participants"].Value,System.Globalization.NumberStyles.AllowThousands);
                        amount = Decimal.Parse(match1.Groups["Amount"].Value, System.Globalization.NumberStyles.Currency);
                        iobj.insertPutnamInvoice(_qy, vendor, period, partcount, amount);
                    }
                }

                if (records != 4)
                {
                    throw new Exception("Cannot find all required records. Check format of the report");
                }

                iobj.insertImportStatus(_qy, "ptnm_invoice");
            }
            finally
            {
                d.Close();
                fs.Close();
            }

            return records;
        }

        public int ImportPtnmPartData(string _filepath, string _source, string _qy)
        {            
            int _counter = 0;
            HRAConvertExcel tObj = new HRAConvertExcel();
            HRAParseData pObj = new HRAParseData();
            DataSet ds = new DataSet(); ds.Clear();
            DataTable dt = new DataTable("PutnamPartTable");

            ds.Tables.Add(tObj.ConvertRangeXLS(_filepath, dt, "SSN", 0));
            _counter = pObj.parsePutnamPartData(ds, _filepath, _source, _qy);
            
            return _counter;
        }

        public int ImportHRAAUDITR(string _filepath, string _source, string _qy)
        {
            int _counter = 0;
            HRAImportDAL iobj = new HRAImportDAL();
            HRAConvertExcel tObj = new HRAConvertExcel();
            HRAParseData pObj = new HRAParseData();
            DataSet ds = new DataSet(); ds.Clear();
            DataTable dt = new DataTable("AUDITRTable");

            iobj.Rollback(_source, _qy);
            ds.Tables.Add(tObj.ConvertXLS(_filepath, dt));
            _counter = pObj.parseHRAAUDITR(ds, _qy);

            return _counter;
        }

        public int ImportWgwkInvoice(string _filepath, string _source, string yrmo)
        {
            int _counter = 0;
            HRAConvertExcel tObj = new HRAConvertExcel();
            HRAParseData pObj = new HRAParseData();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("WgwkInvTable");

            ds.Tables.Add(tObj.ConvertRangeXLS(_filepath, dt, "Last Name", 1));
            _counter = pObj.parseWageworkInvoice(ds, _filepath, _source, yrmo);            

            return _counter;
        }

        public void CheckRptsImported(string _qy)
        {
            string[] YRMOs = HRA.GetYRMOs(_qy);
            string errmsg = "";
            string[] source = { "ptnm_invoice", "ptnm_partdata", "wgwk_invoice" };
            HRAImportDAL iobj = new HRAImportDAL();

            if (!HRAAdminDAL.AUDITRInserted(_qy))
                errmsg = "HRAAUDITR data is not present for Quarter/Year - " + _qy + ". Run HRA Operations process!<br/>";
            
            if (!iobj.PastImport(source[0], _qy))
                errmsg += "Putnam Invoice report not imported for Quarter/Year - " + _qy + "<br />";

            if (!iobj.PastImport(source[1], _qy))
                errmsg += "Putnam's Participant Data report not imported<br />";

            string temp = "";
            for (int i = 0; i < 3; i++)
            {
                if (!iobj.PastImport(source[2], YRMOs[i]))
                    temp += YRMOs[i] + ", ";
            }
            if (temp != "")
                errmsg += "Wageworks Invoice report not imported for YRMO(s) - " + temp.Remove(temp.Length - 2) + "<br />";


            if (errmsg != "")
                throw new Exception(errmsg);
        }

        public DataSet GetWageworkSummaryRecon(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            int ptnminv1_cnt, wgwkinv_cnt, hraAuditR_cnt, ptnmPartData_cnt;
            decimal rate, ptnminv1_amt, ptnminv1_calcamt;
            Decimal totamt1, totamt2;
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsWgwkSum = new DataSet(); dsWgwkSum.Clear();
            DataTable dsTable; dsTable = dsWgwkSum.Tables.Add("WgwkSummary");            
            DataRow row;
            DataColumn col;            
            System.Type typeDecimal = System.Type.GetType("System.Decimal");

            col = new DataColumn("Period"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Invoice Count"); dsTable.Columns.Add(col);
            col = new DataColumn("Wageworks Invoice Count"); dsTable.Columns.Add(col);
            col = new DataColumn("Diff(B-C)"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Participant Data Count"); dsTable.Columns.Add(col);
            col = new DataColumn("Diff(C-E)"); dsTable.Columns.Add(col);
            col = new DataColumn("HRAAUDITR Count(as of " + DateTime.Today.ToString("MM/dd/yyyy") + ")"); dsTable.Columns.Add(col);
            col = new DataColumn("Diff(E-G)"); dsTable.Columns.Add(col);
            col = new DataColumn("Rate"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Invoice Amount", typeDecimal); dsTable.Columns.Add(col);
            col = new DataColumn("Amount(BxI)", typeDecimal); dsTable.Columns.Add(col);
           

            foreach (string yrmo in YRMOs)
            {
                ds = dobj.GetPutnamInvData(_qy); ptnminv1_cnt = GetPutnamInvPart1Count(ds, yrmo);
                wgwkinv_cnt = dobj.GetWgwkInvCount(yrmo);
                ptnmPartData_cnt = dobj.GetPutnamPartDataCount(yrmo);
                hraAuditR_cnt = dobj.GetHRAAuditRCount(yrmo);
                rate = dobj.GetWageworkHeadcountRate(yrmo);
                ptnminv1_amt = GetPutnamInvPart1Amt(ds, yrmo);
                ptnminv1_calcamt = ptnminv1_cnt * rate;

                row = dsTable.NewRow();
                row["Period"] = hobj.GetYRMOformated(yrmo);
                row["Putnam Invoice Count"] = ptnminv1_cnt;
                row["Wageworks Invoice Count"] = wgwkinv_cnt;
                row["Diff(B-C)"] = (ptnminv1_cnt - wgwkinv_cnt);
                row["Putnam Participant Data Count"] = ptnmPartData_cnt;
                row["Diff(C-E)"] = (wgwkinv_cnt - ptnmPartData_cnt);
                row["HRAAUDITR Count(as of " + DateTime.Today.ToString("MM/dd/yyyy") + ")"] = hraAuditR_cnt;
                row["Diff(E-G)"] = (ptnmPartData_cnt - hraAuditR_cnt);
                row["Rate"] = rate;
                row["Putnam Invoice Amount"] = ptnminv1_amt;
                row["Amount(BxI)"] = ptnminv1_calcamt;
                dsTable.Rows.Add(row);                
            }

            totamt1 = Convert.ToDecimal(dsWgwkSum.Tables[0].Compute("SUM([Putnam Invoice Amount])", String.Empty));
            totamt2 = Convert.ToDecimal(dsWgwkSum.Tables[0].Compute("SUM([Amount(BxI)])", String.Empty));
            
            row = dsTable.NewRow();
            row["Rate"] = "TOTAL:";
            row["Putnam Invoice Amount"] = totamt1;
            row["Amount(BxI)"] = totamt2;
            dsTable.Rows.Add(row);

            return dsWgwkSum;
        }

        public DataSet GetPutnamSummaryRecon(string _qy)
        {
            HRAAdminDAL dobj = new HRAAdminDAL();
            int ptnminv2_cnt, ptnmPart_cnt;
            decimal rate, ptnminv2_amt, ptnminv2_calcamt;
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsPtnmSum = new DataSet(); dsPtnmSum.Clear();
            DataTable dsTable; dsTable = dsPtnmSum.Tables.Add("PtnmSummary");            
            DataRow row;
            DataColumn col;            

            col = new DataColumn("Period"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Invoice Count"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Participant Data Count"); dsTable.Columns.Add(col);
            col = new DataColumn("Diff(B-C)"); dsTable.Columns.Add(col);
            col = new DataColumn("Rate"); dsTable.Columns.Add(col);
            col = new DataColumn("Putnam Invoice Amount"); dsTable.Columns.Add(col);
            col = new DataColumn("Amount(BxE)"); dsTable.Columns.Add(col);
            

            ds = dobj.GetPutnamInvData(_qy); ptnminv2_cnt = GetPutnamInvPart2Count(ds);
            ptnmPart_cnt = dobj.GetPtnmPartDataCountHavingBal(_qy);
            rate = dobj.GetPutnamHRARecordRate(_qy);
            ptnminv2_amt = GetPutnamInvPart2Amt(ds);
            ptnminv2_calcamt = ptnminv2_cnt * rate;

            row = dsTable.NewRow();            
            row["Period"] = _qy;            
            row["Putnam Invoice Count"] = ptnminv2_cnt;
            row["Putnam Participant Data Count"] = ptnmPart_cnt;
            row["Diff(B-C)"] = (ptnminv2_cnt - ptnmPart_cnt);
            row["Rate"] = rate;
            row["Putnam Invoice Amount"] = ptnminv2_amt;
            row["Amount(BxE)"] = ptnminv2_calcamt;                       
            dsTable.Rows.Add(row);            

            return dsPtnmSum;
        }

        public DataSet GetRecon_PartData_HRAAUDITR(string _qy)
        {
            HRA hobj = new HRA(); 
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsPartAudit = new DataSet();
            dsPartAudit.Clear();
            DataTable dsTable;
            

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetPartData_AuditR_Discp(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon1_" + yrmo;
                dsPartAudit.Tables.Add(dsTable);
            }

            return dsPartAudit;
        }

        public DataSet GetRecon_HRAAUDITR_PartData1(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsAuditPart1 = new DataSet(); dsAuditPart1.Clear();            
            DataTable dsTable;

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetAuditR_PartData_Discp1(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon2a_" + yrmo;
                dsAuditPart1.Tables.Add(dsTable);
            }

            return dsAuditPart1;
        }

        public DataSet GetRecon_HRAAUDITR_PartData2(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsAuditPart2 = new DataSet(); dsAuditPart2.Clear();            
            DataTable dsTable;

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetAuditR_PartData_Discp2(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon2b_" + yrmo;
                dsAuditPart2.Tables.Add(dsTable);
            }

            return dsAuditPart2;
        }

        public DataSet GetRecon_PartData_WgwkInv(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsPartWgwkInv = new DataSet();
            dsPartWgwkInv.Clear();
            DataTable dsTable;


            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetPartData_WgwkInv_Discp(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon1_" + yrmo;
                dsPartWgwkInv.Tables.Add(dsTable);
            }

            return dsPartWgwkInv;
        }

        public DataSet GetRecon_WgwkInv_PartData(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsWgwkInvPart = new DataSet(); dsWgwkInvPart.Clear();            
            DataTable dsTable;

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetWgwkInv_PartData_Discp(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon2_" + yrmo;
                dsWgwkInvPart.Tables.Add(dsTable);
            }

            return dsWgwkInvPart;
        }

        public DataSet GetNoBal_WgwkInv_PartData(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsWgwkPart = new DataSet(); dsWgwkPart.Clear();
            DataTable dsTable;

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetWgwkInv_PartData_noBal(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon3_" + yrmo;
                dsWgwkPart.Tables.Add(dsTable);
            }

            return dsWgwkPart;
        }

        public DataSet GetPaidOut_WgwkInv_PartData(string _qy)
        {
            HRA hobj = new HRA();
            HRAAdminDAL dobj = new HRAAdminDAL();
            string[] YRMOs = HRA.GetYRMOs(_qy);
            DataSet dsWgwkPart2 = new DataSet(); 
            DataTable dsTable;

            foreach (string yrmo in YRMOs)
            {
                dsTable = new DataTable();
                dsTable = dobj.GetWgwkInv_PartData_PaidOut(yrmo).Tables[0].Copy();
                dsTable.TableName = "recon4_" + yrmo;
                dsWgwkPart2.Tables.Add(dsTable);
            }

            return dsWgwkPart2;
        }

        int GetPutnamInvPart1Count(DataSet ds, string yrmo)
        {
           int ptnminv1_cnt = 0;
            string strExpr = "vendor = 'WageWorks' AND period = '" + yrmo + "'";
            DataTable myTable = ds.Tables[0];   
            DataRow [ ] foundRows = myTable.Select( strExpr);
            ptnminv1_cnt = Convert.ToInt32(foundRows[0]["count"]);

            return ptnminv1_cnt;  
        }

        decimal GetPutnamInvPart1Amt(DataSet ds, string yrmo)
        {
            decimal ptnminv1_amt = 0;
            string strExpr = "vendor = 'WageWorks' AND period = '" + yrmo + "'";
            DataTable myTable = ds.Tables[0];
            DataRow[] foundRows = myTable.Select(strExpr);
            ptnminv1_amt = Convert.ToDecimal(foundRows[0]["amount"]);

            return ptnminv1_amt;  
        }

        decimal GetPutnamInvPart2Amt(DataSet ds)
        {
            decimal ptnminv2_amt = 0;
            string strExpr = "vendor = 'Mercer'";
            DataTable myTable = ds.Tables[0];
            DataRow[] foundRows = myTable.Select(strExpr);
            ptnminv2_amt = Convert.ToDecimal(foundRows[0]["amount"]);

            return ptnminv2_amt;  
        }

        int GetPutnamInvPart2Count(DataSet ds)
        {
            int ptnminv2_cnt = 0;
            string strExpr = "vendor = 'Mercer'";
            DataTable myTable = ds.Tables[0];
            DataRow[] foundRows = myTable.Select(strExpr);
            ptnminv2_cnt = Convert.ToInt32(foundRows[0]["count"]);

            return ptnminv2_cnt;  
        }
    }
}
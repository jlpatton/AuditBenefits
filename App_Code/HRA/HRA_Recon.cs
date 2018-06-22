using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace EBA.Desktop.HRA
{
    public class HRA_Recon
    {
        public HRA_Recon()
        {
        }

        public static void Pass3_CurWageworks(string yrmo)
        {
            Nullable<DateTime> wgwkdt, wgwkLateDt, ptnmAdjLateDt, ptnmLateDt;
            Decimal totptnmamt, totwgwkamt, wgwkamt2, priCarryAmt, totPtnmAdjAmt;
            string trantype, CFids;
            DataSet dswgwk = new DataSet(); dswgwk.Clear();
                        
            dswgwk = HRA_ReconDAL.getWageworksData(yrmo);
            wgwkLateDt = HRA_ReconDAL.getWgwkCurLateDt(yrmo);
            CFids = "";

            foreach (DataRow row in dswgwk.Tables[0].Rows)
            {
                if (row["createdt"] == null) wgwkdt = null;
                else wgwkdt = Convert.ToDateTime(row["createdt"]);                               
                totptnmamt = HRA_ReconDAL.getPtnmCurTotAmt(yrmo, row["ssn"].ToString());
                totwgwkamt = (-1) * HRA_ReconDAL.getWgwkCurTotAmt(yrmo, row["ssn"].ToString());
                wgwkamt2 = (-1) * HRA_ReconDAL.getWgwkAmt2(yrmo, row["ssn"].ToString());
                priCarryAmt = HRA_ReconDAL.getPriCarryAmt(yrmo, row["ssn"].ToString());
                ptnmLateDt = HRA_ReconDAL.getPtnmCurLateDt(yrmo, row["ssn"].ToString());
                ptnmAdjLateDt = HRA_ReconDAL.getPtnmAdjCurLateDt(yrmo, row["ssn"].ToString());
                totPtnmAdjAmt = (-1) * HRA_ReconDAL.getPtnmAdjCurTotAmt(yrmo, row["ssn"].ToString());
                trantype = row["trantype"].ToString().Trim().ToUpper();

                priCarryAmt = (-1) * priCarryAmt;
                totwgwkamt = totwgwkamt + priCarryAmt;
                wgwkamt2 = wgwkamt2 + priCarryAmt;
                if (ptnmLateDt == null && ptnmAdjLateDt == null)
                {
                    if (totwgwkamt != 0)
                    {
                        CFids = CFids + ", " + Convert.ToInt32(row["id"]); continue;
                    }
                }
                else
                {
                    if (ptnmAdjLateDt == null)
                    {
                        if (totwgwkamt != totptnmamt)
                        {
                            if (wgwkamt2 == totptnmamt)
                            {
                                if (wgwkdt == wgwkLateDt)
                                {
                                    CFids = CFids + ", " + Convert.ToInt32(row["id"]); continue;
                                }
                            }
                            else
                            {
                                if (wgwkdt == ptnmLateDt)
                                {
                                    if (!((trantype == "CARD TRANSACTION") && (totptnmamt > wgwkamt2)))
                                    {
                                        CFids = CFids + ", " + Convert.ToInt32(row["id"]); continue;
                                    }
                                }
                                else
                                {
                                    if (wgwkdt > ptnmLateDt)
                                    {
                                        CFids = CFids + ", " + Convert.ToInt32(row["id"]); continue;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((totwgwkamt + totPtnmAdjAmt) != totptnmamt)
                        {
                            if (((wgwkdt > ptnmLateDt) && (wgwkdt > ptnmAdjLateDt)) || ((wgwkdt >= ptnmLateDt) && (wgwkdt >= ptnmAdjLateDt) && (wgwkdt == HRA.GetLastDayofYRMO(yrmo))))
                            {
                                CFids = CFids + ", " + Convert.ToInt32(row["id"]); continue;
                            }
                        }
                    }
                }
            }

            CFids = CFids.TrimStart(',');
            if (CFids == String.Empty)
            {
                CFids = "-1";
            }
            
            HRA_ReconDAL.InsertCFData(yrmo, CFids);
            HRA_ReconDAL.InsertWageworks(yrmo, CFids);
            HRA_ReconDAL.InsertCarryAmt(yrmo, CFids);
        }

        public static void Pass4_PriWageworks(string yrmo)
        {
            HRA hobj = new HRA();
            DataSet dswgwk = new DataSet(); dswgwk.Clear();
            Nullable<DateTime> wgwkdt, ptnmLateDt;
            Decimal wgwkamt, totCurPtnmAmt, totCurWgwkAmt;
            string priyrmo = hobj.getPrevYRMO(1);

            dswgwk = HRA_ReconDAL.getWageworksData(priyrmo);

            foreach (DataRow row in dswgwk.Tables[0].Rows)
            {
                totCurPtnmAmt = HRA_ReconDAL.getPtnmCurTotAmt(yrmo, row["ssn"].ToString());
                totCurWgwkAmt = HRA_ReconDAL.getWgwkCurTotAmt(yrmo, row["ssn"].ToString());

                if ((totCurPtnmAmt - totCurWgwkAmt) != 0) continue;
                
                if (row["createdt"] == null) wgwkdt = null;
                else wgwkdt = Convert.ToDateTime(row["createdt"]);
                
                if (row["amount"] == null) wgwkamt = 0;
                else wgwkamt = Convert.ToDecimal(row["amount"]); wgwkamt = (-1) * wgwkamt;
                
                ptnmLateDt = HRA_ReconDAL.getPtnmCurLateDt(priyrmo, row["ssn"].ToString());

                if (ptnmLateDt != null)
                {
                    // Important Logic! Skip record if the latest Putnam Date is Greater than the Wageworks Transaction Date for the prior YRMO
                    if (ptnmLateDt > wgwkdt) continue;
                }

                HRA_ReconDAL.InsertWageworks(yrmo, row["ssn"].ToString(), row["lname"].ToString(), row["fname"].ToString(), wgwkamt, 1);                
            }
        }

        public static void InsertPrevCFData(string yrmo)
        {
            Decimal priorWgwkAmt, curPtnmAmt;
            Nullable<DateTime> wgwkLateDt, ptnmFirstDt;
            DataSet ds = new DataSet(); ds.Clear();
            HRA hobj = new HRA();
            string priyrmo = hobj.getPrevYRMO(yrmo);
            List<string> _ssns = new List<string>();
            DataRow[] rows;
            object _temp;
            
            ds = HRA_ReconDAL.GetPrevCFUnBalanced(yrmo);

            if (ds.Tables[0].Rows.Count > 0)
            {

                wgwkLateDt = HRA_ReconDAL.getWgwkCurLateDt(priyrmo);
                ptnmFirstDt = HRA_ReconDAL.getPtnmCurFirstDt(yrmo);

                _ssns = HRA_ReconDAL.GetPrevCFUnBalancedSSNs(yrmo);

                foreach (string _ssn in _ssns)
                {
                    priorWgwkAmt = 0;
                    curPtnmAmt = 0;

                    if(wgwkLateDt != null)  
                    {
                        _temp = ds.Tables[0].Compute("SUM([Wageworks Amount])", "[SSN]= '" + _ssn + "' AND [Transaction Date] = '" + Convert.ToDateTime(wgwkLateDt).ToString("MM/dd/yyyy") + "'");
                        if(_temp != DBNull.Value) {priorWgwkAmt = Convert.ToDecimal(_temp); }                        
                    }

                    if (ptnmFirstDt != null)
                    {
                        curPtnmAmt = HRA_ReconDAL.getPtnmCurFirstAmt(yrmo, _ssn, Convert.ToDateTime(ptnmFirstDt).ToString("MM/dd/yyyy"));
                    }

                    if ((priorWgwkAmt != 0) && (curPtnmAmt != 0) && (priorWgwkAmt + curPtnmAmt) == 0)
                    {
                        rows = ds.Tables[0].Select("[SSN]= '" + _ssn + "'");
                        foreach (DataRow row in rows)
                        {
                            ds.Tables[0].Rows.Remove(row);
                        }
                    }
                }
            }

            HRA_ReconDAL.InsertPrevCFNotCleared(ds, yrmo);
        }
    }
}

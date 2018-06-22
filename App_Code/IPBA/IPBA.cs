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

namespace EBA.Desktop.IPBA
{
    public class IPBA
    {
        public IPBA()
        {
        }

        public string GetProgramCode(string program)
        {
            string progcd = "";

            switch (program)
            {
                case "Active":
                    progcd = "FA";
                    break;
                case "Active Pilot":
                    progcd = "PA";
                    break;
                case "COBRA":
                    progcd = "CA";
                    break;
                case "COBRA Pilot":
                    progcd = "CP";
                    break;
                case "Retiree":
                    progcd = "FR";
                    break;
                case "Retiree Pilot":
                    progcd = "PR";
                    break;
            }

            return progcd;
        }

        public string GetPlanCode(string plancd, string program)
        {
            string plancode = "";

            switch (program)
            {
                case "FA":
                    plancode = plancd.Substring(0,2);
                    break;
                case "PA":
                    plancode = plancd.Substring(3, 2);
                    break;
                case "CA":
                    plancode = plancd.Substring(0, 2);
                    break;
                case "CP":
                    plancode = plancd.Substring(3, 2);
                    break;
                case "FR":
                    plancode = plancd.Substring(0, 2);
                    break;
                case "PR":
                    plancode = plancd.Substring(0, 2);
                    break;
            }

            return plancode;
        }          

        public DataSet GetSummaryRpt(string yrmo, string plancode)
        {
            IPBA_DAL dobj = new IPBA_DAL();
            string tierexp;
            int count = 0;
            string[] typecds = { "STD", "CHG", "RETRO", "MANCHG", "MANRETRO" };
            string[] programs = { "Active", "Active Pilot", "COBRA", "COBRA Pilot", "Retiree", "Retiree Pilot"};
            string[] tiercds = {"E", "C", "S", "F" };            
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsTemp1 = new DataSet(); dsTemp1.Clear(); 
            DataSet dsSum = new DataSet(); dsSum.Clear();
            System.Type typeDecimal = System.Type.GetType("System.Decimal");
            System.Type typeInt = System.Type.GetType("System.Int32");
            DataTable tblSum = dsSum.Tables.Add("Summary");
            DataRow row;
            DataColumn col;
            col = new DataColumn("PlanYr"); tblSum.Columns.Add(col);
            col = new DataColumn("Classification"); tblSum.Columns.Add(col);
            col = new DataColumn("Coverage Tier"); tblSum.Columns.Add(col);
            col = new DataColumn("Emps", typeInt); tblSum.Columns.Add(col);            
            col = new DataColumn("Rate", typeDecimal); tblSum.Columns.Add(col);
            col = new DataColumn("Total", typeDecimal); tblSum.Columns.Add(col);

            dsTemp = dobj.GetDetData(yrmo, plancode);
            dsTemp1 = dobj.GetAdjData(yrmo, plancode); dsTemp.Merge(dsTemp1); dsTemp1.Clear();
            dsTemp1 = GetCobraAdjustments(yrmo, plancode); dsTemp.Merge(dsTemp1); dsTemp1.Clear();
            dsTemp1 = GetManualAdjustments(yrmo, plancode); dsTemp.Merge(dsTemp1); dsTemp1.Clear();

            foreach (string typecd in typecds)
            {
                foreach (string program in programs)
                {
                    foreach (string tiercd in tiercds)
                    {
                        tierexp = GetTierExp(tiercd, program);
                        count = GetEmpCount(typecd, program, tiercd, dsTemp);
                        if (count != 0)
                        {
                            row = tblSum.NewRow();
                            row["PlanYr"] = yrmo.Substring(0, 4);
                            row["Classification"] = GetClassification(typecd, program);
                            row["Coverage Tier"] = tierexp;
                            row["Emps"] = count;
                            row["Rate"] = dobj.GetRate(yrmo, program, plancode, tiercd);
                            if (typecd.Equals("CHG") || typecd.Equals("MANCHG"))
                                row["Total"] = count * dobj.GetRate(yrmo, program, plancode, tiercd);
                            else
                                row["Total"] = GetTotalAmt(typecd, program, tiercd, dsTemp);
                            tblSum.Rows.Add(row);
                        }
                        if (typecd.Equals("CHG") || typecd.Equals("MANCHG"))
                        {
                            tierexp = GetTierExp(tiercd, program);
                            count = GetEmpCountPriorCHG(typecd, program, tiercd, dsTemp);
                            if (count != 0)
                            {
                                row = tblSum.NewRow();
                                row["PlanYr"] = yrmo.Substring(0, 4);
                                row["Classification"] = GetClassification(typecd, program);
                                row["Coverage Tier"] = tierexp;
                                row["Emps"] = (-1) * count;
                                row["Rate"] = dobj.GetRate(yrmo, program, plancode, tiercd);
                                row["Total"] = (-1) * count * dobj.GetRate(yrmo, program, plancode, tiercd);
                                tblSum.Rows.Add(row);
                            }
                        }
                    }
                    count = GetEmpCount(typecd, program, dsTemp);
                    if (count != 0)
                    {
                        row = tblSum.NewRow();
                        row["Classification"] = "***";
                        row["Coverage Tier"] = "SUB TOTAL:";
                        row["Emps"] = GetSumEmpCount(GetClassification(typecd, program), dsSum); 
                        row["Total"] = GetTotalAmt(typecd, program, dsTemp);
                        tblSum.Rows.Add(row);
                    }
                }
            }
            if (tblSum.Rows.Count > 0)
            {
                row = tblSum.NewRow();
                row["Classification"] = "*****";
                row["Coverage Tier"] = "GRAND TOTAL:";
                row["Emps"] = GetSumEmpCount(dsSum);
                row["Total"] = GetTotalAmt(dsTemp);
                tblSum.Rows.Add(row);
            }

            return dsSum;
        }

        public DataSet GetDetailRpt(string yrmo, string plancode)
        {
            IPBA_DAL dobj = new IPBA_DAL();
            int count = 0;            
            string[] programs = { "Active", "Active Pilot", "COBRA", "COBRA Pilot" };
            string[] tiercds = { "E", "C", "S", "F" };
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsDet = new DataSet(); dsDet.Clear();
            DataTable tblDet;
            DataRow row;
            DataRow[] rows;
            
            
            dsTemp = dobj.GetDetData(yrmo, plancode);
            dsDet = dsTemp.Clone(); tblDet = dsDet.Tables[0];
            
            foreach (string program in programs)
            {
                foreach (string tiercd in tiercds)
                {
                    rows = GetDetRows(program, tiercd, dsTemp); dsDet.Merge(rows);
                    count = GetDetEmpCount(program, tiercd, dsTemp);
                       
                    if (count != 0)
                    {
                        row = tblDet.NewRow();
                        row["Name"] = "***";                        
                        row["Tier"] = count;
                        row["Rate"] = GetDetTotalAmt(program, tiercd, dsTemp);                      
                        tblDet.Rows.Add(row);
                    }
                }
                count = GetEmpCount(program, dsTemp);
                if (count != 0)
                {
                    row = tblDet.NewRow();
                    row["Class"] = "*** SUB TOTAL:";
                    row["Tier"] = count;
                    row["Rate"] = GetTotalAmt(program, dsTemp);
                    tblDet.Rows.Add(row);
                }               
            }
            if (tblDet.Rows.Count > 0)
            {
                row = tblDet.NewRow();
                row["Class"] = "*** GRAND TOTAL:";
                row["Rate"] = GetTotalAmt(dsTemp);
                tblDet.Rows.Add(row);
            }

            return dsDet;
        }

        public DataSet GetHTHAnthemRpt(string yrmo, string plancode)
        {            
            IPBA_DAL dobj = new IPBA_DAL();            
            string[] programs = { "Active", "Active Pilot", "COBRA", "COBRA Pilot" };
            string[] tiercds = { "E", "C", "S", "F" };
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsDet = new DataSet(); dsDet.Clear();                  
            DataRow[] rows;


            dsTemp = dobj.GetDetData(yrmo, plancode);
            dsDet = dsTemp.Clone();

            foreach (string program in programs)
            {
                foreach (string tiercd in tiercds)
                {
                    rows = GetDetRows(program, tiercd, dsTemp); dsDet.Merge(rows);                    
                }               
            }

            dsDet.Tables[0].Columns["Class"].ColumnName = "progcd";
            dsDet.Tables[0].Columns["Type"].ColumnName = "ptype";
            dsDet.Tables[0].Columns["SSN"].ColumnName = "ssno";
            dsDet.Tables[0].Columns["Name"].ColumnName = "name";
            dsDet.Tables[0].Columns["Tier"].ColumnName = "tiercd";
            dsDet.Tables[0].Columns["Eff Dt"].ColumnName = "effdt";
            dsDet.Tables[0].Columns["Rate"].ColumnName = "rate";

            return dsDet;
        }

        public DataSet GetAdjustmentRpt(string yrmo, string plancode)
        {
            IPBA_DAL dobj = new IPBA_DAL();
            int count = 0;
            string[] typecds = { "CHG", "RETRO" };
            string[] programs = { "Active", "Active Pilot", "COBRA", "COBRA Pilot", "Retiree", "Retiree Pilot" };            
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsAdj = new DataSet(); dsAdj.Clear();
            DataTable tblAdj = dsAdj.Tables.Add("Adjustments");
            System.Type typeDecimal = System.Type.GetType("System.Decimal");
            DataRow[] rows;
            DataRow row;
            DataColumn col;
            col = new DataColumn("Py"); tblAdj.Columns.Add(col);
            col = new DataColumn("Class"); tblAdj.Columns.Add(col);
            col = new DataColumn("Type"); tblAdj.Columns.Add(col);
            col = new DataColumn("SSN"); tblAdj.Columns.Add(col);
            col = new DataColumn("Name"); tblAdj.Columns.Add(col);
            col = new DataColumn("YRMO"); tblAdj.Columns.Add(col);
            col = new DataColumn("Comments"); tblAdj.Columns.Add(col);
            col = new DataColumn("Months"); tblAdj.Columns.Add(col);
            col = new DataColumn("Amount", typeDecimal); tblAdj.Columns.Add(col);
            col = new DataColumn("Flag"); tblAdj.Columns.Add(col);

            dsTemp = dobj.GetAdjData(yrmo, plancode);
            dsTemp.Merge(GetCobraAdjustments(yrmo, plancode));
            dsTemp.Merge(GetManualAdjustments(yrmo, plancode));            

            foreach (string program in programs)
            {
                foreach (string typecd in typecds)
                {
                    rows = GetAdjRows(typecd, program, dsTemp);
                    foreach (DataRow row1 in rows)
                    {
                        row = tblAdj.NewRow();
                        row["Py"] = row1["Py"];
                        row["Class"] = program;
                        row["Type"] = row1["Type"];
                        row["SSN"] = row1["SSN"];
                        row["Name"] = row1["Name"];
                        row["YRMO"] = row1["YRMO"];
                        row["Comments"] = row1["Comments"];
                        row["Months"] = row1["Months"];                        
                        row["Amount"] = row1["Rate"];
                        row["Flag"] = row1["flag"];
                        tblAdj.Rows.Add(row);
                    }
                    count = GetAdjEmpCount(typecd, program, dsTemp);
                    if (count != 0)
                    {
                        row = tblAdj.NewRow();
                        row["Comments"] = "***";
                        row["Amount"] = GetAdjTotalAmt(typecd, program, dsTemp);
                        tblAdj.Rows.Add(row);
                    }
                }
                count = GetEmpCount(program, dsTemp);
                if (count != 0)
                {
                    row = tblAdj.NewRow();
                    row["Class"] = "*** SUB TOTAL:";                    
                    row["Amount"] = GetTotalAmt(program, dsTemp);
                    tblAdj.Rows.Add(row);
                }               
            }
            if (tblAdj.Rows.Count > 0)
            {
                row = tblAdj.NewRow();
                row["Class"] = "*** GRAND TOTAL:";
                row["Amount"] = GetTotalAmt(dsTemp);
                tblAdj.Rows.Add(row);
            }

            return dsAdj;
        }        

        DataSet GetManualAdjustments(string yrmo, string plancode)
        {
            IPBA_DAL dobj = new IPBA_DAL();
            WashRules wobj = new WashRules();
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsManAdj = new DataSet(); dsManAdj.Clear();
            DataTable tblManAdj = dsManAdj.Tables.Add("Table");           
            DataRow row1;
            DataColumn col;
            System.Type typeDecimal = System.Type.GetType("System.Decimal");
            col = new DataColumn("Py"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Class"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Type"); tblManAdj.Columns.Add(col);
            col = new DataColumn("SSN"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Name"); tblManAdj.Columns.Add(col);
            col = new DataColumn("YRMO"); tblManAdj.Columns.Add(col);
            col = new DataColumn("PriorTier"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Tier"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Comments"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Months"); tblManAdj.Columns.Add(col);
            col = new DataColumn("Rate", typeDecimal); tblManAdj.Columns.Add(col);
            col = new DataColumn("typecd"); tblManAdj.Columns.Add(col);
            col = new DataColumn("flag"); tblManAdj.Columns.Add(col);

            DateTime effdt;
            string exprCHG, yrmocalc;
            int months;
            string _flag = "";

            dsTemp = dobj.GetManAdjData(yrmo, plancode, "NEW");
            foreach(DataRow row in dsTemp.Tables[0].Rows)
            {
                effdt = Convert.ToDateTime(row["Eff Dt"]);
                months = wobj.washRuleADD(effdt, yrmo, plancode);

                _flag = "";
                if (MaxMonExceeded(plancode, "NEW", yrmo, effdt))
                    _flag = "*";                

                if (months != 0)
                {
                    for (int i = 1; i <= months; i++)
                    {
                        row1 = tblManAdj.NewRow();
                        row1["Py"] = yrmo.Substring(0, 4);
                        row1["Class"] = row["Class"];
                        row1["Type"] = row["Type"];
                        row1["SSN"] = row["SSN"];
                        row1["Name"] = row["Name"];
                        row1["YRMO"] = GetYRMO(yrmo, (i - 1));
                        row1["Tier"] = row["Tier"];
                        if (row["Class"].ToString().Equals("COBRA") || row["Class"].ToString().Equals("COBRA Pilot"))
                        {
                            if (row["Comments"].ToString().Contains("NEW ENROLLEE EFF: "))
                                row1["Comments"] = row["Comments"].ToString().Replace("NEW ENROLLEE EFF: ", "COBRA RETRO PAYMENT - EFF DT: ");
                            else
                                row1["Comments"] = row["Comments"].ToString().Replace("RETRO CVG EFF: ", "COBRA RETRO PAYMENT - EFF DT: ");
                        }
                        else
                            row1["Comments"] = row["Comments"];
                        row1["Months"] = 1;
                        row1["Rate"] = dobj.GetRate(GetYRMO(yrmo, (i - 1)), row["Class"].ToString(), plancode, row["Tier"].ToString()); 
                        row1["typecd"] = row["typecd"];
                        row1["flag"] = _flag;
                        tblManAdj.Rows.Add(row1);
                    }
                }
            }
            dsTemp.Clear();
            dsTemp = dobj.GetManAdjData(yrmo, plancode, "TERM");
            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                effdt = Convert.ToDateTime(row["Eff Dt"]);
                months = wobj.washRuleTERM(effdt, yrmo, plancode);

                _flag = "";
                if (MaxMonExceeded(plancode, "TERM", yrmo, effdt))
                    _flag = "*";  

                if (months != 0)
                {
                    for (int i = 1; i <= System.Math.Abs(months); i++)
                    {
                        if (months < 0)
                            yrmocalc = GetYRMO(yrmo, i);
                        else
                            yrmocalc = yrmo;
                         
                        row1 = tblManAdj.NewRow();
                        row1["Py"] = yrmo.Substring(0, 4);
                        row1["Class"] = row["Class"];
                        row1["Type"] = row["Type"];
                        row1["SSN"] = row["SSN"];
                        row1["Name"] = row["Name"];
                        row1["YRMO"] = yrmocalc;                        
                        row1["Tier"] = row["Tier"];
                        if (row["Class"].ToString().Equals("COBRA") || row["Class"].ToString().Equals("COBRA Pilot"))
                        {
                            if (months < 0)
                                row1["Comments"] = row["Comments"].ToString().Replace("TERMINATED CVG EFF: ", "COBRA RETRO CREDIT - EFF DT: ");
                            else
                                row1["Comments"] = row["Comments"].ToString().Replace("TERMINATED CVG EFF: ", "COBRA RETRO PAYMENT - EFF DT: ");
                        }
                        else
                            row1["Comments"] = row["Comments"];
                        if (months < 0)
                        {
                            row1["Months"] = -1;
                            row1["Rate"] = (-1) * dobj.GetRate(yrmocalc, row["Class"].ToString(), plancode, row["Tier"].ToString()); 
                        }
                        else
                        {
                            row1["Months"] = 1;
                            row1["Rate"] = dobj.GetRate(yrmocalc, row["Class"].ToString(), plancode, row["Tier"].ToString()); 
                        }                        
                        row1["typecd"] = row["typecd"];
                        row1["flag"] = _flag;
                        tblManAdj.Rows.Add(row1);
                    }
                }
            }
            dsTemp.Clear();
            dsTemp = dobj.GetManAdjData(yrmo, plancode, "CHG");
            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                effdt = Convert.ToDateTime(row["Eff Dt"]);
                exprCHG = wobj.washRuleCHG(effdt, yrmo,plancode);
                months = GetMonthsManAdjCHG(exprCHG);

                _flag = "";
                if (MaxMonExceeded(plancode,"CHG", yrmo, effdt))
                    _flag = "*";  

                for (int i = 1; i <= months; i++)
                {
                    row1 = tblManAdj.NewRow();
                    row1["Py"] = yrmo.Substring(0, 4);
                    row1["Class"] = row["Class"];
                    row1["Type"] = row["Type"];
                    row1["SSN"] = row["SSN"];
                    row1["Name"] = row["Name"];
                    row1["YRMO"] = GetYRMO(yrmo, (i - 1));
                    row1["PriorTier"] = row["PriorTier"];
                    row1["Tier"] = row["Tier"];
                    row1["Comments"] = row["Comments"];
                    row1["Months"] = 1;
                    row1["Rate"] = GetManAdjAmtCHG(i, yrmo, exprCHG, row["Class"].ToString(), plancode, row["Tier"].ToString(), row["PriorTier"].ToString());
                    row1["typecd"] = row["typecd"];
                    row1["flag"] = _flag;
                    tblManAdj.Rows.Add(row1);
                    if (exprCHG.Equals("1-")) continue;
                }
            }
            dsTemp.Clear();
            return dsManAdj;
        }

        DataSet GetCobraAdjustments(string yrmo, string plancode)
        {
            IPBA_DAL dobj = new IPBA_DAL();            
            System.Type typeDecimal = System.Type.GetType("System.Decimal");
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            DataSet dsCobAdj = new DataSet(); dsCobAdj.Clear();
            DataTable tblCobAdj = dsCobAdj.Tables.Add("Table");
            DataRow row1;
            DataColumn col;
            Decimal amt, amt1, rate, adjamt;
            String prevyrmo, comments, recordYrmo;
            Boolean found;
            int months;

            col = new DataColumn("Py"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Class"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Type"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("SSN"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Name"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("YRMO"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Tier"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Comments"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Months"); tblCobAdj.Columns.Add(col);
            col = new DataColumn("Rate", typeDecimal); tblCobAdj.Columns.Add(col);
            col = new DataColumn("typecd"); tblCobAdj.Columns.Add(col);

            dsTemp = dobj.GetCobraAdjData(yrmo, plancode);
            foreach (DataRow row in dsTemp.Tables[0].Rows)
            {
                amt = Decimal.Parse(row["Rate"].ToString());
                
                if (amt == 0)
                    continue;

                adjamt = amt;
                recordYrmo = row["CoveragePeriod"].ToString();
                found = false;
                months = 1;

                for (int i = 1; i < 12; i++)
                {
                    amt1 = (System.Math.Abs(amt)) / i;
                    prevyrmo = IPBAImport.getPrevYRMO(i-1, recordYrmo);
                    rate = dobj.GetRate(prevyrmo, row["Class"].ToString(), plancode, row["Tier"].ToString());
                    
                    if (rate.Equals(amt1))
                    {
                        adjamt = (amt)/i;
                        months = i;
                        recordYrmo = prevyrmo;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    if (amt > 0)
                        comments = "COBRA RETRO PAYMENT - EFF DT: " + row["Eff Dt"].ToString();
                    else
                        comments = "COBRA RETRO CREDIT - EFF DT: " + row["Eff Dt"].ToString(); 
                }
                else
                    comments = "COBRA ADJUSTMENT - EFF DT: " + row["Eff Dt"].ToString();

                for (int j = 0; j < months; j++)
                {
                    row1 = tblCobAdj.NewRow();
                    row1["Py"] = yrmo.Substring(0, 4);
                    row1["Class"] = row["Class"];
                    row1["Type"] = row["Type"];
                    row1["SSN"] = row["SSN"];
                    row1["Name"] = row["Name"];
                    row1["YRMO"] = recordYrmo;
                    row1["Tier"] = row["Tier"];
                    row1["Comments"] = comments;
                    if(amt > 0 )
                        row1["Months"] = 1;
                    else
                        row1["Months"] = -1;
                    row1["Rate"] = adjamt;
                    row1["typecd"] = "RETRO";
                    tblCobAdj.Rows.Add(row1);
                }
            }
            dsTemp.Clear();
            return dsCobAdj;
        }

        int GetEmpCount(string typecd, string program, string tiercd, DataSet ds)
        {
            int count = 0;
             string filterexp;
             if (typecd.Equals("STD"))
             {
                 filterexp = "Class='" + program + "' AND Tier='" + tiercd + "' AND typecd IS NULL";
                 count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));
             }
             else
             {
                 filterexp = "typecd ='" + typecd + "' AND Class='" + program + "' AND Tier='" + tiercd + "' AND Months='1'";
                 count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));
                 filterexp = "typecd ='" + typecd + "' AND Class='" + program + "' AND Tier='" + tiercd + "' AND Months='-1'";
                 count -= Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));
             }           

            return count;
        }

        int GetEmpCountPriorCHG(string typecd, string program, string tiercd, DataSet ds)
        {
            int count = 0;
            string filterexp;
           
            filterexp = "typecd ='" + typecd + "' AND Class='" + program + "' AND PriorTier='" + tiercd + "'";
            count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));

            return count;
        }

        Decimal GetTotalAmtPriorCHG(string typecd, string program, string tiercd, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;

            filterexp = "typecd ='" + typecd + "' AND Class='" + program + "' AND PriorTier='" + tiercd + "'";
            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value))
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        int GetDetEmpCount(string program, string tiercd, DataSet ds)
        {
            int count = 0;
            string filterexp;
            
            filterexp = "Class='" + program + "' AND Tier='" + tiercd + "'";
            count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));

            return count;
        }

        int GetEmpCount(string typecd, string program, DataSet ds)
        {
            int count = 0;
            string filterexp;
            if (typecd.Equals("STD"))
                filterexp = "Class='" + program + "' AND typecd IS NULL";
            else
                filterexp = "typecd ='" + typecd + "' AND Class='" + program + "'";

            count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));

            return count;
        }

        int GetSumEmpCount(string classification, DataSet ds)
        {
            int count = 0;
            string filterexp;
           
            filterexp = "Classification ='" + classification + "'";

            object temp = ds.Tables["Summary"].Compute("SUM(Emps)", filterexp);
            if (!temp.Equals(DBNull.Value))
                count = Convert.ToInt32(temp);

            return count;
        }

        int GetSumEmpCount(DataSet ds)
        {
            int count = 0;
            string filterexp;

            filterexp = "[Coverage Tier] ='SUB TOTAL:'";

            object temp = ds.Tables["Summary"].Compute("SUM(Emps)", filterexp);
            if (!temp.Equals(DBNull.Value))
                count = Convert.ToInt32(temp);

            return count;
        }

        int GetAdjEmpCount(string typecd, string program, DataSet ds)
        {
            int count = 0;
            string filterexp;
            string mantypecd = "MAN" + typecd;

            if (typecd.Equals("STD"))
                filterexp = "Class='" + program + "' AND typecd IS NULL";
            else
                filterexp = "(typecd ='" + typecd + "' OR typecd='" + mantypecd + "') AND Class='" + program + "'";

            count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));

            return count;
        }

        int GetEmpCount(string program, DataSet ds)
        {
            int count = 0;
            string filterexp;
           
            filterexp = "Class='" + program + "'";
            count = Convert.ToInt32(ds.Tables[0].Compute("COUNT(Tier)", filterexp));

            return count;
        }

        Decimal GetTotalAmt(string typecd, string program, string tiercd, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;
            if (typecd.Equals("STD"))
                filterexp = "Class='" + program + "' AND Tier='" + tiercd + "' AND typecd IS NULL";
            else
                filterexp = "typecd ='" + typecd + "' AND Class='" + program + "' AND Tier='" + tiercd + "'";

            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value)) 
                amt = Convert.ToDecimal(temp);

            return amt;
        }        

        Decimal GetDetTotalAmt(string program, string tiercd, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;

            filterexp = "Class='" + program + "' AND Tier='" + tiercd + "'";
            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value)) 
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        Decimal GetTotalAmt(string typecd, string program, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;
            if (typecd.Equals("STD"))
                filterexp = "Class='" + program + "' AND typecd IS NULL";
            else
                filterexp = "typecd ='" + typecd + "' AND Class='" + program + "'";

            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if(!temp.Equals(DBNull.Value))                
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        Decimal GetAdjTotalAmt(string typecd, string program, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;
            string mantypecd = "MAN" + typecd;

            if (typecd.Equals("STD"))
                filterexp = "Class='" + program + "' AND typecd IS NULL";
            else
                filterexp = "(typecd ='" + typecd + "' OR typecd='" + mantypecd + "') AND Class='" + program + "'";

            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value)) 
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        Decimal GetTotalAmt(string program, DataSet ds)
        {
            Decimal amt = 0;
            string filterexp;
           
            filterexp = "Class='" + program + "'";
            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value)) 
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        Decimal GetTotalAmt(DataSet ds)
        {
            Decimal amt = 0;
            string filterexp = String.Empty;

            object temp = ds.Tables[0].Compute("SUM(Rate)", filterexp);
            if (!temp.Equals(DBNull.Value)) 
                amt = Convert.ToDecimal(temp);

            return amt;
        }

        string GetTierExp(string tiercd, string program)
        {
            string temp, tierexp;

            if (program.Contains("Pilot")) 
                temp = "Pilot";
            else 
                temp = "EE";

            switch (tiercd)
            {
                case "E":
                    tierexp = temp + " Only";
                    break;
                case "C":
                    tierexp = temp + " & Child";
                    break;
                case "S":
                    tierexp = temp + " & Spouse";
                    break;
                default:
                    tierexp = temp + " & Family";
                    break;
            }

            return tierexp;
        }

        string GetClassification(string typecd, string program)
        {
            string classification = "";

            //if (program.Contains("Pilot"))
            //    program = program.Remove(program.IndexOf(" "));

            switch (typecd)
            {
                case "STD":
                    classification = program;
                    break;
                case "CHG":
                    classification = "STATUS CHG: " + program;
                    break;
                case "RETRO":
                    classification = "RETRO: " + program;
                    break;
                case "MANCHG":
                    classification = "Manual Adj - STATUS CHG: " + program;
                    break;
                case "MANRETRO":
                    classification = "Manual Adj - RETRO: " + program;
                    break;
            }

            return classification;
        }

        DataRow[] GetDetRows(string program, string tiercd, DataSet ds)
        {
            DataRow[] filterrows;
            string filterexp = "Class='" + program + "' AND Tier='" + tiercd + "'";

            filterrows = ds.Tables[0].Select(filterexp, "SSN");           

            return filterrows;
        }

        DataRow[] GetAdjRows(string typecd, string program, DataSet ds)
        {
            DataRow[] filterrows;
            string mantypecd = "MAN" + typecd;

            string filterexp = "(typecd ='" + typecd + "' OR typecd='" + mantypecd + "') AND Class='" + program + "'";

            filterrows = ds.Tables[0].Select(filterexp, "SSN ASC, YRMO ASC");

            return filterrows;
        }       

        Decimal GetManAdjAmtCHG(int i, string yrmo, string exprCHG, string programcd, string plancode, string tier, string priortier)
        {
            Decimal amt = 0;
            Decimal amtprev, amtprev1, amtprev2, amtcur;
            string adjyrmo;
            IPBA_DAL dobj = new IPBA_DAL();

            switch (exprCHG)
            {
                case "-1":
                    amt = dobj.GetRate(yrmo, programcd, plancode, priortier);
                    break;
                case "1":
                    amt = dobj.GetRate(yrmo, programcd, plancode, tier);
                    break;
                case "1-":
                    adjyrmo = GetYRMO(yrmo, (-1));
                    amt = dobj.GetRate(yrmo, programcd, plancode, tier);
                    break;
                case "A+1":                    
                    adjyrmo = GetYRMO(yrmo, (- 1));
                    amtcur = dobj.GetRate(yrmo, programcd, plancode, tier);
                    amtprev = dobj.GetRate(adjyrmo, programcd, plancode, priortier);
                    
                    if (i == 1)
                        amt = dobj.GetRate(yrmo, programcd, plancode, tier);
                    else
                        amt = (amtcur - amtprev);   
                  
                    break;
                case "2A+1":                    
                    adjyrmo = GetYRMO(yrmo, (-1));
                    amtcur = dobj.GetRate(yrmo, programcd, plancode, tier);
                    amtprev = dobj.GetRate(adjyrmo, programcd, plancode, priortier);
                    adjyrmo = GetYRMO(yrmo, (-2));
                    amtprev1 = dobj.GetRate(adjyrmo, programcd, plancode, priortier);
                    
                    if (i == 1)
                        amt = dobj.GetRate(yrmo, programcd, plancode, tier);
                    else if(i == 2)
                        amt = amtcur - amtprev;
                    else
                        amt = amtcur - amtprev1;                   
                   
                    break;
                case "3A+1":
                    adjyrmo = GetYRMO(yrmo, (-1));
                    amtcur = dobj.GetRate(yrmo, programcd, plancode, tier);
                    amtprev = dobj.GetRate(adjyrmo, programcd, plancode, priortier);
                    adjyrmo = GetYRMO(yrmo, (-2));
                    amtprev1 = dobj.GetRate(adjyrmo, programcd, plancode, priortier);
                    adjyrmo = GetYRMO(yrmo, (-3));
                    amtprev2 = dobj.GetRate(adjyrmo, programcd, plancode, priortier);

                    if (i == 1)
                        amt = dobj.GetRate(yrmo, programcd, plancode, tier);
                    else if (i == 2)
                        amt = amtcur - amtprev;
                    else if(i == 3)
                        amt = amtcur - amtprev1;
                    else
                        amt = amtcur - amtprev2;

                    break;

            }            

            return amt;
        }

        int GetMonthsManAdjCHG(string exprCHG)
        {
            int months = 0;
            
            switch (exprCHG)
            {
                case "-1":
                    months = 1;
                    break;
                case "1":
                    months = 1;
                    break;
                case "1-":
                    months = 1;
                    break;
                case "A+1":
                    months = 2;
                    break;
                case "2A+1":
                    months = 3;
                    break;
                case "3A+1":
                    months = 4;
                    break;
            }

            return months;
        }        

        string GetYRMO(string yrmo, int months)
        {
            DateTime prevmondt = Convert.ToDateTime(yrmo.Insert(4,"/")).AddMonths(-months);
            string prevyear = prevmondt.Year.ToString();
            string prevmonth = prevmondt.Month.ToString();
            if (prevmonth.Length == 1)
                prevmonth = "0" + prevmonth;
            string prevYRMO = prevyear + prevmonth;

            return prevYRMO;
        }

        Boolean MaxMonExceeded(string plancode, string trancode, string yrmo, DateTime effdt)
        {
            WashRules wobj = new WashRules();

            int _mnthDiff = wobj.MonthDifference(effdt, yrmo);
            int _day = effdt.Day;
            int maxmonths;

            if (plancode.Contains("P5"))
                maxmonths = 4;
            else
                maxmonths = 3;

            if (_mnthDiff == maxmonths)
            {
                if (_day < 15 && (!trancode.Equals("TERM")))
                    return true;
                if (trancode.Equals("TERM"))
                    return true;
            }
            if (_mnthDiff > maxmonths)
            {
                return true;
            }

            return false;
        }
    }
}
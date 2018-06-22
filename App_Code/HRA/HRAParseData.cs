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
using System.Data.SqlTypes;

namespace EBA.Desktop.HRA
{
    public class HRAParseData
    {
        public HRAParseData()
        {            
        }

        public int parsePutnam(DataSet ds, string _yrmo, string _fname)
        {
            DateTime distdt;           
            Decimal distamt;
            String transType, ssn, lname, fname;            
            int _count = 0;            
            List<int> cols;
            String _tableName = "PutnamTable";           
            String[] colsH = new String[] { "First Name","Last Name", "SSN", "Distribution Date", "Transaction Code", "Distribution Amount" };
            HRAImportDAL iObj = new HRAImportDAL();
            
            cols = getColsIndices(ds, _tableName, colsH);

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                distdt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[3]]);
                transType = ds.Tables[_tableName].Rows[i][cols[4]].ToString();
                distamt = Decimal.Parse(ds.Tables[_tableName].Rows[i][cols[5]].ToString(), System.Globalization.NumberStyles.Currency);
                ssn = ds.Tables[_tableName].Rows[i][cols[2]].ToString();
                if (ssn.Contains("-"))
                {
                    ssn = ssn.Replace("-", "");
                }
                lname = ds.Tables[_tableName].Rows[i][cols[1]].ToString().Trim(); lname = HRA.GetProperCase(lname);
                fname = ds.Tables[_tableName].Rows[i][cols[0]].ToString().Trim(); fname = HRA.GetProperCase(fname);              
                
                if(iObj.insertPutnam(_yrmo, distdt,transType, distamt, ssn, lname, fname))
                _count++;                
            }
            iObj.insertImportStatus(_yrmo, "Putnam");
            return _count;
        }

        public int parsePutnamAdj(DataSet ds, string _yrmo)
        {
            DateTime distdt;
            string transType = "Adj";
            Decimal distamt;
            String ssn, name, lname, fname;
            int _count = 0;
            List<int> cols;
            string _tableName = "PutnamAdjTable";
            String[] colsH = new String[] { "TRADE", "Amount", "SSN", "Name" };
            HRAImportDAL iObj = new HRAImportDAL();

            cols = getColsIndices(ds, _tableName, colsH);

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                distdt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[0]]);
                distamt = Decimal.Parse(ds.Tables[_tableName].Rows[i][cols[1]].ToString(), System.Globalization.NumberStyles.Currency);
                ssn = ds.Tables[_tableName].Rows[i][cols[2]].ToString();
                if (ssn.Contains("-"))
                {
                    ssn = ssn.Replace("-", "");
                }
                name = ds.Tables[_tableName].Rows[i][cols[3]].ToString();
                lname = name.Substring(0, name.IndexOf(',')).Trim(); lname = HRA.GetProperCase(lname);
                fname = name.Substring(name.IndexOf(',') + 1).Trim(); fname = HRA.GetProperCase(fname);

                if(iObj.insertPutnamAdj(_yrmo, distdt, transType, distamt, ssn, lname, fname))
                _count++;

            }
            iObj.insertImportStatus(_yrmo, "PutnamAdj");
            return _count;
        }

        public int parseWageworks(DataSet ds, string _yrmo)
        {
            DateTime createdt;
            Decimal amt;
            String transtype, lname, fname, last4ssn;             
            int _count = 0;
            List<int> cols;             
            string _tableName = "WageworksTable";
            String[] colsH = new String[] { "CREATE_DATE", "Amount", "TRANSACTION_TYPE", "Last_Name", "First_Name", "LAST_4_SSN" };
            HRAImportDAL iObj = new HRAImportDAL();
            
            cols = getColsIndices(ds, _tableName, colsH);

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                createdt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[0]]);
                amt = Decimal.Parse(ds.Tables[_tableName].Rows[i][cols[1]].ToString(), System.Globalization.NumberStyles.Currency);
                transtype = ds.Tables[_tableName].Rows[i][cols[2]].ToString();
                lname = ds.Tables[_tableName].Rows[i][cols[3]].ToString().Trim(); lname = HRA.GetProperCase(lname);
                fname = ds.Tables[_tableName].Rows[i][cols[4]].ToString().Trim(); fname = HRA.GetProperCase(fname);
                last4ssn = ds.Tables[_tableName].Rows[i][cols[5]].ToString();
                if(iObj.insertWageworks(_yrmo, createdt, amt, transtype, lname, fname, last4ssn))
                _count++;

            }
            iObj.insertImportStatus(_yrmo, "Wageworks");          
            return _count;
        }

        public int parseHRAAUDITR(DataSet ds, string _qy)
        {
            Nullable<DateTime> createdt, statusdt, modifydt, dob;
            int empno, age, _uid;
            string ssn, name, status, _period;
            int _count = 0;
            List<int> cols;
            string _tableName = "AUDITRTable";
            String[] colsH = new String[] { "ssno", "empno", "name", "dob", "age", "retiredt", "modifydt" };
            HRAImportDAL iObj = new HRAImportDAL();

            cols = getColsIndices(ds, _tableName, colsH);

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                ssn = ds.Tables[_tableName].Rows[i][cols[0]].ToString();
                empno = Convert.ToInt32(ds.Tables[_tableName].Rows[i][cols[1]].ToString());
                name = ds.Tables[_tableName].Rows[i][cols[2]].ToString().Replace("'", "''");
                dob = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[3]]);
                age = Convert.ToInt32(ds.Tables[_tableName].Rows[i][cols[4]]);
                statusdt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[5]]);
                modifydt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][cols[6]]);

                iObj.insertAUDITR(_qy, ssn, empno, name, dob, age, statusdt, modifydt);
                _count++;

            }
            iObj.insertImportStatus(_qy, "HRAAUDITR");
            return _count;
        }

        public int parsePutnamPartData(DataSet ds, string _fname, string source, string _qy)
        {  
            Decimal balance;
            String ssn, lname, fname, partStatDesc, dob, termdt;
            SqlDateTime dobSql, termdtSql;
            int _count = 0;
            List<int> cols;
            List<string> excludeSSNs;
            Boolean excludeSSNfound;
            String _tableName = "PutnamPartTable";            
            String[] colsH = new String[] { "SSN", "First Name", "Last Name", "Participant Status Description", "Date of Birth", "Termination Date", "Total Asset Balance" };
            HRAImportDAL iObj = new HRAImportDAL();
            
            cols = getColsIndices(ds, _tableName, colsH);
            excludeSSNs = HRAAdminDAL.GetExcludeSSNs();
           
            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                ssn = ds.Tables[_tableName].Rows[i][cols[0]].ToString().Trim();
                if (ssn.Contains("-"))
                {
                    ssn = ssn.Replace("-", "");
                }
                ssn = ssn.TrimStart('0');

                excludeSSNfound = false;
                foreach (string excludeSSN in excludeSSNs)
                {
                    if (ssn.Equals(excludeSSN))
                    {
                        excludeSSNfound = true;
                        break;
                    }
                }
                if (excludeSSNfound)
                {
                    continue;
                }
                
                fname = ds.Tables[_tableName].Rows[i][cols[1]].ToString().Trim(); fname = HRA.GetProperCase(fname);
                if (fname.Contains("  "))
                {
                    fname = fname.Replace("  ", " ");
                    fname = fname.Replace("'", "");
                }

                lname = ds.Tables[_tableName].Rows[i][cols[2]].ToString().Trim(); lname = HRA.GetProperCase(lname);
                if (lname.Contains("  "))
                {
                    lname = lname.Replace("  ", " ");
                    lname = lname.Replace("'", "");
                }

                partStatDesc = ds.Tables[_tableName].Rows[i][cols[3]].ToString().Trim();

                dob = ds.Tables[_tableName].Rows[i][cols[4]].ToString().Trim();
                if (dob != null && dob != string.Empty)
                    dobSql = Convert.ToDateTime(dob);
                else
                    dobSql = SqlDateTime.Null;

                termdt = ds.Tables[_tableName].Rows[i][cols[5]].ToString().Trim();
                if (termdt != null && termdt != string.Empty)
                    termdtSql = Convert.ToDateTime(termdt);
                else
                    termdtSql = SqlDateTime.Null;

                balance = Decimal.Parse(ds.Tables[_tableName].Rows[i][cols[6]].ToString(), System.Globalization.NumberStyles.Currency);

                if(iObj.insertPutnamPartData(ssn, fname, lname, partStatDesc, dobSql, termdtSql, balance, source, _qy))
                _count++;
            }
            iObj.insertImportStatus(_qy, "ptnm_partdata");

            return _count;
        }

        public int parseWageworkInvoice(DataSet ds, string _fname, string source, string yrmo)
        {
            String last4ssn, lname, fname;
            int _count = 0;
            List<int> cols;
            String _tableName = "WgwkInvTable";           
            String[] colsH = new String[] { "Last Name", "First Name", "ID Code" };
            HRAImportDAL iObj = new HRAImportDAL();

            cols = getColsIndices(ds, _tableName, colsH);

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                lname = ds.Tables[_tableName].Rows[i][cols[0]].ToString().Trim(); lname = HRA.GetProperCase(lname);
                if (lname.Contains("  "))
                {
                    lname = lname.Replace("  ", " ");
                    lname = lname.Replace("'", "");
                }
                fname = ds.Tables[_tableName].Rows[i][cols[1]].ToString().Trim(); fname = HRA.GetProperCase(fname);
                if (fname.Contains("  "))
                {
                    fname = fname.Replace("  ", " ");
                    fname = fname.Replace("'", "");
                }
                last4ssn = ds.Tables[_tableName].Rows[i][cols[2]].ToString().Trim();

                if(iObj.insertWageworkInvoice(lname, fname, last4ssn, source, yrmo))
                _count++;
            }
            iObj.insertImportStatus(yrmo, "wgwk_invoice");
            return _count;
        }

        List<int> getColsIndices(DataSet ds, String _tableName, String[] colsH)
        {
            List<int> colsIndices = new List<int>();
            int lastCol = ds.Tables[_tableName].Columns.Count;
            DataRow dr = null;
            string colname;

            dr = ds.Tables[_tableName].Rows[0];
            foreach (String colH in colsH)
            {
                for (int j = 0; j < lastCol; j++)
                {
                    colname = ds.Tables[_tableName].Columns[j].ColumnName.ToString().Trim();                        
                    
                    if (String.Compare(colname, colH, true) == 0)
                    {
                        colsIndices.Add(j);
                        break;
                    }
                }
            }

            if (colsIndices.Count < colsH.Length)
            {
                throw new Exception("Cannot find required columns in the report.<br >Check format of the report");
            }

            return colsIndices;
        }
    }
}
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
using System.Collections.Generic;
using System.IO;



namespace EBA.Desktop.HRA
{
    public class HRA_ReconDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRA_ReconDAL()
        {
        }

        public static bool checkedSave(string _category, string report)
        {
            bool _checkedSave = false;
            string cmdStr = "SELECT autosave FROM FileMaintainence "
                                + "WHERE classification = 'Export' "
                                + "AND category = @category "
                                + "AND sourcecd = @report ";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@report", report);
                _checkedSave = Convert.ToBoolean(command.ExecuteScalar());

                return _checkedSave;
            }
            finally
            {
                connect.Close();
            }
        }

        public static bool checkedPrint(string _category, string report)
        {
            bool _checkedPrint = false;
            string cmdStr = "SELECT autoprint FROM FileMaintainence "
                                + "WHERE classification = 'Export' "
                                + "AND category = @category "
                                + "AND sourcecd = @report ";


            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@report", report);
                _checkedPrint = Convert.ToBoolean(command.ExecuteScalar());

                return _checkedPrint;
            }
            finally
            {
                connect.Close();
            }
        }

        public static string GetFilePath(string _category, string _source)
        {
            string _filepath = "";
            string cmdStr = "SELECT filelocation FROM FileMaintainence "
                                + "WHERE classification = 'Export' "
                                + "AND category = @category "
                                + "AND sourcecd = @source ";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@source", _source);
                _filepath = Convert.ToString(command.ExecuteScalar());

                if ((_filepath.Length > 1) && !(_filepath.Substring(_filepath.Length - 2).Equals("\\")))
                    _filepath = _filepath + "\\";

                return _filepath;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean pastReconcile(string yrmo, string source)
        {
            string cmdStr = "SELECT COUNT(*) FROM ImportRecon_status WHERE period=@yrmo AND source=@source AND type='Recon' AND module='HRA'";
            int count = 0;

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@source", source);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    return false;
                }
                return true;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void ReconDelete(string yrmo)
        {
            string cmdStr1 = "DELETE FROM hra_recon WHERE hrcn_yrmo = @yrmo";
            string cmdStr2 = "DELETE FROM ImportRecon_status WHERE period=@yrmo AND source='Recon' AND type='Recon' AND module='HRA'";
            string cmdStr3 = "DELETE FROM [hra_content] WHERE [category] = 'HRARecon' AND [period] = @yrmo AND [source] = 'CF'";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdStr2, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdStr3, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public static void insertReconStatus(string yrmo, string source)
        {
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module) VALUES(@yrmo, @source, 'Recon', 'HRA')";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetReconData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsResult = new DataSet(); dsResult.Clear();
            DataSet dsTotalDifference = new DataSet(); 
            DataRow row;
            double totDifferenceAmt = 0.00;
            //6/22/2009
            double CurYRMOtotDifferenceAmt = 0.00;
            UpdateTotalDifferenceAmountsforHRARecon(yrmo);
            string cmdstr = "SELECT hrcn_ssn AS [SSN], "
                                + "hrcn_lname AS [Last Name], "
                                + "hrcn_fname AS [First Name], "
                                + "hrcn_ptnmamt AS [Putnam Amount], "
                                + "CASE WHEN hrcn_wgwkamt IS NOT NULL THEN ((-1) * hrcn_wgwkamt) ELSE hrcn_wgwkamt END AS [WageWorks Amount], "
                                + "hrcn_ptnmAdjamt AS [Adjustment Amount], "
                                + "((-1) * (ISNULL(hrcn_ptnmamt,0) + ISNULL(((-1) * hrcn_wgwkamt),0) + ISNULL(hrcn_ptnmAdjamt,0))) AS [Current Difference], "
                                + "hrcn_diffamt AS [Total Difference], "
                                + "hrcn_ptnmCtr AS [Putnam Record Count], "
                                + "hrcn_wgwkCtr AS [WageWorks Record Count] "
                                + "FROM hra_recon "
                                + "WHERE ISNULL(hrcn_diffamt,0) <> 0 "
                                //AND ((-1) * (ISNULL(hrcn_ptnmamt,0) + ISNULL(((-1) * hrcn_wgwkamt),0) + ISNULL(hrcn_ptnmAdjamt,0))) <> 0 "  //Added  condition ISNULL(hrcn_ptnmAdjamt,0) <> 0 "Current Difference Amount" 3/25/2009 R.A 
                                //Removed the above condition since in the summary transaction tab Todd wants to see the transactions if Cyrrent Total amount is zero
                                //Remove the Detail transactions for employees where current diff amount is zero 3/27/2009 R.A
                                + "AND hrcn_yrmo = @yrmo "
                                + "ORDER BY [SSN] ASC ";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(ds);
                DataTable dtfilter = ds.Tables[0].Clone();
                DataRow[] results;
                results = ds.Tables[0].Select("[Total Difference] <> '0'", "[SSN] ASC");
                //populate new destination table
                foreach (DataRow dr in results)
                    dtfilter.ImportRow(dr);
                dtfilter.AcceptChanges();
                row = dtfilter.NewRow();
                row["Last Name"] = "** TOTAL:";
                row["Putnam Amount"] = dtfilter.Compute("SUM([Putnam Amount])", string.Empty);
                row["Adjustment Amount"] = dtfilter.Compute("SUM([Adjustment Amount])", string.Empty);
                row["WageWorks Amount"] = dtfilter.Compute("SUM([WageWorks Amount])", string.Empty);
                row["Current Difference"] = dtfilter.Compute("SUM([Current Difference])", string.Empty);
                row["Total Difference"] = dtfilter.Compute("SUM([Total Difference])", string.Empty);
                //Add the row to the dtfilter datatable
                dtfilter.Rows.Add(row);
                dtfilter.AcceptChanges();
                //Add the datatable to the dsresult dataset
                dsResult.Tables.Add(dtfilter);

                return dsResult;
                /// 7-6-2009 The following code is replaced by the updatetotaldifferenceamounts function called above
                //if (ds.Tables[0].Rows.Count > 0)
                //    //added this logic to check the current total difference amount for each employee as of current yrmo
                //    //R.A 3/26/2009
                //    //gets the current total difference amount and update the summary recon dataset with the new difference amount as of current yrmo
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        if (dr["SSN"] != DBNull.Value)
                //        {
                //            //if (dr["SSN"].ToString() == "047369756" || dr["SSN"].ToString() == "031318366")
                //            //{
                //            //    totDifferenceAmt = 0.00;
                //            //}
                //            dsTotalDifference = GetTotalDifferenceAmount(dr["SSN"].ToString(),yrmo);
                //            if (dsTotalDifference.Tables[0].Rows.Count > 0)
                //            {
                //                if (dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty)!= DBNull.Value)
                //                totDifferenceAmt = Convert.ToDouble(dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty));
                //                //Update the total difference amount in the hra_recon table 
                //                //As per Andrea D 6/22/2009
                //            UpdateTotalDifferenceAmount(dr["SSN"].ToString(), yrmo, totDifferenceAmt);
                //                if (totDifferenceAmt == 0.00)
                //                {
                //                   dr["Total Difference"] = Convert.ToString(totDifferenceAmt);
                //                }
                //                else
                //                {
                //                    dr["Total Difference"] = Convert.ToString(totDifferenceAmt);
                //                }
                //            }
                //        }
                        
                //    }
                //            ////accept the changes to the original dataset with the above changes
                //            ds.Tables[0].AcceptChanges();
                            
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetDetailReconData(string yrmo)
        {
            List<int> dispSSNs = new List<int>();
            DataSet ds1 = new DataSet(); ds1.Clear();
            DataSet ds2 = new DataSet(); ds2.Clear();
            DataSet dsFinal = new DataSet(); dsFinal.Clear();
            DataTable tblDet;
            DataRow row;
            DataRow[] rows;

            ds2 = GetPutnamDispDtlData(yrmo);
            ds1 = GetWageworksDtlnoCFData(yrmo);
            ds2.Merge(ds1); ds1.Clear();
            ds1 = GetPutnamAdjDispDtlData(yrmo);
            ds2.Merge(ds1, false, MissingSchemaAction.Add); ds1.Clear();
            dsFinal = ds2.Clone(); tblDet = dsFinal.Tables[0];

            dispSSNs = GetDispSSNs(yrmo);

            foreach (int dispSSN in dispSSNs)
            {
                //rows = ds2.Tables[0].Select("SSN = '" + dispSSN + "'", "[Last Name] ASC, [First Name] ASC, [Transaction Date] DESC");
                rows = ds2.Tables[0].Select("SSN = '" + dispSSN + "'", "[Transaction Date] DESC");
               dsFinal.Merge(rows);
                if (rows.Length != 0)
                {
                    row = tblDet.NewRow();
                    row["Last Name"] = "*** SUB TOTAL:";
                    row["Putnam Amount"] = ds2.Tables[0].Compute("SUM([Putnam Amount])", "SSN = '" + dispSSN + "'");
                    row["Adjustment Amount"] = ds2.Tables[0].Compute("SUM([Adjustment Amount])", "SSN = '" + dispSSN + "'");
                    row["Wageworks Amount"] = ds2.Tables[0].Compute("SUM([Wageworks Amount])", "SSN = '" + dispSSN + "'");
                    tblDet.Rows.Add(row);
                }
            }
            if (tblDet.Rows.Count > 0)
            {
                row = tblDet.NewRow();
                row["Last Name"] = "*** GRAND TOTAL:";
                row["Putnam Amount"] = tblDet.Compute("SUM([Putnam Amount])", "[Last Name] = '*** SUB TOTAL:'");
                row["Adjustment Amount"] = tblDet.Compute("SUM([Adjustment Amount])", "[Last Name] = '*** SUB TOTAL:'");
                row["Wageworks Amount"] = tblDet.Compute("SUM([Wageworks Amount])", "[Last Name] = '*** SUB TOTAL:'");
                tblDet.Rows.Add(row);
            }
            
            return dsFinal;
        }

        static DataSet GetPutnamDispDtlData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnam = new DataSet(); dsPutnam.Clear();

            string cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) AS [SSN], "
                                + "ptnm_lname AS [Last Name], "
                                + "ptnm_fname AS [First Name], "
                                + "ptnm_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),ptnm_distribdt,101) AS [Transaction Date], "
                                + "ptnm_distamt AS [Putnam Amount] "
                                + "FROM Putnam "
                                + "WHERE ptnm_yrmo = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) IN "
                                + "( "
                                + "SELECT DISTINCT CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo "
                                + "AND ISNULL(hrcn_diffamt,0) <> 0 "
                                + ") ORDER BY [Transaction Date] Desc";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnam);

                return dsPutnam;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetPutnamDtlData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnam = new DataSet(); dsPutnam.Clear();

            string cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) AS [SSN], "
                               + "ptnm_lname AS [Last Name], "
                               + "ptnm_fname AS [First Name], "
                               + "ptnm_transtype AS [Transaction], "
                               + "CONVERT(VARCHAR(15),ptnm_distribdt, 101) AS [Transaction Date], "
                               + "ptnm_distamt AS [Putnam Amount] "
                               + "FROM Putnam "
                               + "WHERE ptnm_yrmo = @yrmo ORDER BY [Transaction Date] Desc";


            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnam);

                return dsPutnam;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetPutnamAdjDispDtlData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnamAdj = new DataSet(); dsPutnamAdj.Clear();

            string cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ptna_ssn, '-', '')))) AS [SSN], "
                                + "ptna_lname AS [Last Name], "
                                + "ptna_fname AS [First Name], "
                                + "ptna_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),ptna_dt, 101) AS [Transaction Date], "
                                + "ptna_amt AS [Adjustment Amount] "
                                + "FROM PutnamAdj "
                                + "WHERE ptna_yrmo = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptna_ssn, '-', '')))) IN "
                                + "( "
                                + "SELECT DISTINCT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo "
                                + "AND ISNULL(hrcn_diffamt,0) <> 0 "
                                + ") ORDER BY [Transaction Date] Desc";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnamAdj);

                return dsPutnamAdj;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetPutnamAdjDtlData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnamAdj = new DataSet(); dsPutnamAdj.Clear();

            string cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ptna_ssn, '-', '')))) AS [SSN], "
                                + "ptna_lname AS [Last Name], "
                                + "ptna_fname AS [First Name], "
                                + "ptna_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),ptna_dt, 101) AS [Transaction Date], "
                                + "ptna_amt AS [PutnamAdj Amount] "
                                + "FROM PutnamAdj "
                                + "WHERE ptna_yrmo = @yrmo ORDER BY [Transaction Date] Desc";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnamAdj);

                return dsPutnamAdj;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetWageworksDtlnoCFData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWgwk = new DataSet(); dsWgwk.Clear();
            string CFids, cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                CFids = GetCFids(yrmo);

                cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) AS [SSN], "
                               + "wgwk_lname AS [Last Name], "
                               + "wgwk_fname AS [First Name], "
                               + "wgwk_transtype AS [Transaction], "
                               + "CONVERT(VARCHAR(15),wgwk_createdt, 101) AS [Transaction Date], "
                               + "wgwk_amt AS [Wageworks Amount] "
                               + "FROM Wageworks "
                               + "WHERE wgwk_yrmo = @yrmo "
                               + "AND wgwk_ID NOT IN (" + CFids + ") "
                               + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) IN "
                               + "( "
                               + "SELECT DISTINCT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) "
                               + "FROM hra_recon "
                               + "WHERE hrcn_yrmo = @yrmo "
                               + "AND ISNULL(hrcn_diffamt,0) <> 0 "
                               + ") ORDER BY [Transaction Date] Desc";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWgwk);

                return dsWgwk;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetWageworksDtlCFData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWgwkCF = new DataSet(); dsWgwkCF.Clear();
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) AS [SSN], "
                                + "wgwk_lname AS [Last Name], "
                                + "wgwk_fname AS [First Name], "
                                + "wgwk_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),wgwk_createdt, 101) AS [Transaction Date], "
                                + "wgwk_amt AS [Wageworks Amount] "
                                + "FROM Wageworks "
                                + "WHERE wgwk_yrmo = @yrmo ORDER BY [Transaction Date] Desc";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWgwkCF);

                return dsWgwkCF;
            }
            finally
            {
                connect.Close();
            }
        }
        static DataSet GetTotalDifferenceAmount(string ssn,string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsTotDiffAmt = new DataSet(); dsTotDiffAmt.Clear();
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = "select sum(ptnm_distamt)as DiffAmount, 'PutnamAmt' as PutnamAmount, ptnm_yrmo  from putnam where ptnm_ssn=" + ssn + " AND ptnm_yrmo <= " + yrmo + " group by ptnm_yrmo "
                               + " union "
                               + "select sum(wgwk_amt)as DiffAmount ,'WageWorksAmt' as WageWorksAmount, wgwk_yrmo from wageworks where wgwk_ssn=" + ssn + " AND wgwk_yrmo <= " + yrmo + " group by wgwk_yrmo "
                               + " union "
                               + "select sum(isnull(ptna_amt,0)) as DiffAmount,'PutnamAdjAmt' as PutnamAdjAmount, ptna_yrmo from putnamAdj where ptna_ssn=" + ssn + " AND ptna_yrmo <= " + yrmo + " group by ptna_yrmo";
                               

                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsTotDiffAmt);
                return dsTotDiffAmt;
            }
            finally
            {
                connect.Close();
            }
        }
        static DataSet GetMonthlyPaymentAmounts(string ssn, string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsTotDiffAmt = new DataSet(); dsTotDiffAmt.Clear();
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = "select sum(ptnm_distamt)as DistAmount,'PutnamAmt' as Colname, ptnm_yrmo  from putnam where ptnm_ssn=" + ssn + " AND ptnm_yrmo = " + yrmo + " group by ptnm_yrmo "
                               + " union "
                               + "select sum(wgwk_amt)as DistAmount ,'WgwkAmt' as Colname, wgwk_yrmo from wageworks where wgwk_ssn=" + ssn + " AND wgwk_yrmo = " + yrmo + " group by wgwk_yrmo "
                               + " union "
                               + "select sum(isnull(ptna_amt,0)) as DistAmount,'PutnamAdjAmt' as Colname, ptna_yrmo from PutnamAdj where ptna_ssn=" + ssn + " AND ptna_yrmo = " + yrmo + " group by ptna_yrmo";
                               


                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsTotDiffAmt);
                return dsTotDiffAmt;
            }
            finally
            {
                connect.Close();
            }
        }
        static void UpdateTotalDifferenceAmount(string ssn, string yrmo, double diffamt)
        {
            //6/22/2009 - Update the total difference amount 
            //from the GetTotalDifferenceAmount function above for each SSN
            //As per Andrea D 6/22/2009
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = " UPDATE dbo.hra_recon SET [hrcn_diffamt] =  @diffamt "
                                + " WHERE (hrcn_yrmo = @yrmo and hrcn_ssn=@ssn)";


                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@diffamt", diffamt);
                command.ExecuteNonQuery();
                connect.Close();
               
            }
            finally
            {
                connect.Close();
            }
        }
        static void UpdatePutnamAmount(string ssn, string yrmo, double diffamt)
        {
            //6/22/2009 - Update the Putnam amount 
            //in the HRA recon table above for each SSN
            //As per Andrea D 6/23/2009
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = " UPDATE dbo.hra_recon SET [hrcn_ptnmamt] =  @diffamt "
                                + " WHERE (hrcn_yrmo = @yrmo and hrcn_ssn=@ssn)";


                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@diffamt", diffamt);
                command.ExecuteNonQuery();
                connect.Close();

            }
            finally
            {
                connect.Close();
            }
        }
        static void UpdateWageWorksAmount(string ssn, string yrmo, double diffamt)
        {
            //6/22/2009 - Update the Wageworks amount 
            //in the HRA recon table above for each SSN
            //As per Andrea D 6/23/2009
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = " UPDATE dbo.hra_recon SET [hrcn_wgwkamt] =  @diffamt "
                                + " WHERE (hrcn_yrmo = @yrmo and hrcn_ssn=@ssn)";


                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@diffamt", diffamt);
                command.ExecuteNonQuery();
                connect.Close();

            }
            finally
            {
                connect.Close();
            }
        }
        static void UpdatePtnmAdjAmount(string ssn, string yrmo, double diffamt)
        {
            //6/22/2009 - Update the Putnam Adj amount 
            //in the HRA recon table above for each SSN
            //As per Andrea D 6/23/2009
            string cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = " UPDATE dbo.hra_recon SET [hrcn_ptnmAdjamt] =  @diffamt "
                                + " WHERE (hrcn_yrmo = @yrmo and hrcn_ssn=@ssn)";


                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@diffamt", diffamt);
                command.ExecuteNonQuery();
                connect.Close();

            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetWageworksTranData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWgwk = new DataSet(); dsWgwk.Clear();
            string CFids, cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                CFids = GetCFids(yrmo);

                cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) AS [SSN], "
                               + "wgwk_lname AS [Last Name], "
                               + "wgwk_fname AS [First Name], "
                               + "wgwk_transtype AS [Transaction], "
                               + "CONVERT(VARCHAR(15),wgwk_createdt, 101) AS [Transaction Date], "
                               + "wgwk_amt AS [Wageworks Amount] "
                               + "FROM Wageworks "
                               + "WHERE wgwk_yrmo = @yrmo "
                               + "AND wgwk_ID NOT IN (" + CFids + ") ORDER BY [Transaction Date] Desc";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWgwk);

                return dsWgwk;
            }
            finally
            {
                connect.Close();
            }
        }

        static DataSet GetCFTranData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsCF = new DataSet(); dsCF.Clear();
            string CFids, cmdstr;



            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                CFids = GetCFids(yrmo);

                cmdstr = "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) AS [SSN], "
                                + "wgwk_lname AS [Last Name], "
                                + "wgwk_fname AS [First Name], "
                                + "wgwk_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),wgwk_createdt, 101) AS [Transaction Date], "
                                + "wgwk_amt AS [Carry Forward Amount] "
                                + "FROM Wageworks "
                                + "WHERE wgwk_ID IN (" + CFids + ") "
                                + "AND wgwk_yrmo = @yrmo ";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsCF);

                return dsCF;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetCFData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsCF = new DataSet(); dsCF.Clear();
            DataRow row;
            string CFids, cmdstr;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                CFids = GetCFids(yrmo);

                cmdstr = "SELECT wgwk_ssn AS [SSN], "
                                + "wgwk_lname AS [Last Name], "
                                + "wgwk_fname AS [First Name], "
                                + "wgwk_transtype AS [Transaction], "
                                + "CONVERT(VARCHAR(15),wgwk_createdt, 101) AS [Transaction Date], "
                                + "wgwk_amt AS [Wageworks Amount] "
                                + "FROM Wageworks "
                                + "WHERE wgwk_ID IN (" + CFids + ") "
                                + "AND wgwk_yrmo = @yrmo "
                                + "ORDER BY [SSN]";
                                //+ "ORDER BY wgwk_lname, wgwk_fname, wgwk_createdt";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsCF);

                if (dsCF.Tables[0].Rows.Count > 0)
                {
                    row = dsCF.Tables[0].NewRow();
                    row["Last Name"] = "** TOTAL:";
                    row["WageWorks Amount"] = dsCF.Tables[0].Compute("SUM([WageWorks Amount])", string.Empty);
                    dsCF.Tables[0].Rows.Add(row);
                }

                return dsCF;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetPrevCFUnBalanced(string yrmo)
        {
            HRA hobj = new HRA();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsprevCF = new DataSet(); dsprevCF.Clear();
            string prevyrmo, prevCFids, cmdstr;


            try
            {
                prevyrmo = hobj.getPrevYRMO(yrmo);
                prevCFids = GetCFids(prevyrmo);

                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = "SELECT wgwk_ssn AS [SSN], "
                               + "wgwk_lname AS [Last Name], "
                               + "wgwk_fname AS [First Name], "
                               + "wgwk_transtype AS [Transaction], "
                               + "CONVERT(VARCHAR(15),wgwk_createdt,101) AS [Transaction Date], "
                               + "wgwk_amt AS [Wageworks Amount] "
                               + "FROM Wageworks "
                               + "WHERE wgwk_ID IN (" + prevCFids + ") "
                               + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) NOT IN ( "
                               + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) FROM hra_recon "
                               + "WHERE hrcn_yrmo = @yrmo AND ISNULL(hrcn_diffamt,0) = 0) "
                               + "AND wgwk_yrmo = @prevyrmo "
                               + "ORDER BY wgwk_lname, wgwk_fname, wgwk_createdt";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@prevyrmo", prevyrmo);
                da.SelectCommand = command;
                da.Fill(dsprevCF);

                return dsprevCF;
            }
            finally
            {
                connect.Close();
            }
        }

        public static List<string> GetPrevCFUnBalancedSSNs(string yrmo)
        {
            List<string> SSNs = new List<string>();
            SqlDataReader dr = null;
            HRA hobj = new HRA();
            string prevyrmo, prevCFids, cmdstr;


            try
            {
                prevyrmo = hobj.getPrevYRMO(yrmo);
                prevCFids = GetCFids(prevyrmo);


                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                cmdstr = "SELECT DISTINCT wgwk_ssn AS [SSN] "
                               + "FROM Wageworks "
                               + "WHERE wgwk_ID IN (" + prevCFids + ") "
                               + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) NOT IN ( "
                               + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) FROM hra_recon "
                               + "WHERE hrcn_yrmo = @yrmo AND ISNULL(hrcn_diffamt,0) = 0) "
                               + "AND wgwk_yrmo = @prevyrmo";

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@prevyrmo", prevyrmo);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    SSNs.Add(dr.GetString(0));
                }
                dr.Close();

                return SSNs;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed))
                {
                    dr.Close();
                }
                connect.Close();
            }
        }

        public static DataSet GetTransData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsTranSum = new DataSet(); dsTranSum.Clear();
            DataRow row;
            UpdateTotalDifferenceAmountsforHRARecon(yrmo);

            string cmdstr = "SELECT hrcn_ssn AS [SSN], "
                                    + "hrcn_lname AS [Last Name], "
                                    + "hrcn_fname AS [First Name], "
                                    + "hrcn_ptnmamt AS [Putnam Amount], "
                                    + "hrcn_ptnmAdjamt AS [Putnam Adj Amount], "
                                    + "CASE WHEN hrcn_wgwkamt IS NOT NULL THEN ((-1) * hrcn_wgwkamt) ELSE hrcn_wgwkamt END AS [WageWorks Amount], "
                                    + "hrcn_diffamt AS [Difference], "
                                    + "hrcn_carryamt AS [Carry Forward Amount] "
                                    + "FROM hra_recon "
                                    + "WHERE hrcn_yrmo =@yrmo "
                                    + "ORDER BY [SSN]";
            //+ "ORDER BY hrcn_lname, hrcn_fname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsTranSum);

                if (dsTranSum.Tables[0].Rows.Count > 0)
                {
                    row = dsTranSum.Tables[0].NewRow();
                    row["Last Name"] = "** TOTAL:";
                    row["Putnam Amount"] = dsTranSum.Tables[0].Compute("SUM([Putnam Amount])", string.Empty);
                    row["Putnam Adj Amount"] = dsTranSum.Tables[0].Compute("SUM([Putnam Adj Amount])", string.Empty);
                    row["WageWorks Amount"] = dsTranSum.Tables[0].Compute("SUM([WageWorks Amount])", string.Empty);
                    row["Difference"] = dsTranSum.Tables[0].Compute("SUM([Difference])", string.Empty);
                    row["Carry Forward Amount"] = dsTranSum.Tables[0].Compute("SUM([Carry Forward Amount])", string.Empty);
                    dsTranSum.Tables[0].Rows.Add(row);
                }
                return dsTranSum;

                //the following code is replaced by the UpdateTotalDifferenceAmountsforHRARecon(yrmo); 7-6-2009
            ////6-23-2009 - update all the ssns returned for the transactionreport
            ////with totaldifference amounts - Andrea
            //DataSet dsTotalDifference = new DataSet(); dsTotalDifference.Clear();
            //DataSet dsMonthlyAmounts = new DataSet(); dsMonthlyAmounts.Clear();
            //List<string> SSNs = new List<string>();
            //Double totDifferenceAmt = 0.00;
            //Double ptnmAmt = 0.00;
            //Double wgwkAmt = 0.00;
            //Double ptnmAdjAmt = 0.00;
            //DataRow[] results;
            //SSNs = GetStringSSNs(yrmo);
            //foreach (string SSN in SSNs)
            //{
            //    dsTotalDifference = GetTotalDifferenceAmount(SSN.ToString(),yrmo);
            //    if (dsTotalDifference.Tables[0].Rows.Count > 0)
            //    {
            //        if (dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty) != DBNull.Value)
            //            totDifferenceAmt = Convert.ToDouble(dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty));
            //        //Update the total difference amount in the hra_recon table 
            //        //As per Andrea D 6/23/2009
            //        UpdateTotalDifferenceAmount(SSN.ToString(), yrmo, totDifferenceAmt);
            //        totDifferenceAmt = 0.00;
                 
            //    }
            //    ///Monthly payments updates in hra_recon table
            //    dsMonthlyAmounts = GetMonthlyPaymentAmounts(SSN.ToString(), yrmo);
            //    if (dsMonthlyAmounts.Tables[0].Rows.Count > 0)
            //    {
            //        results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'PutnamAmt'");
            //        //populate new destination table
            //        foreach (DataRow dr in results)
            //       // if (dsMonthlyAmounts.Tables[0].Rows[0].IsNull("PtnmAmount") == false)
            //        {
            //            ptnmAmt = Convert.ToDouble(dr["DistAmount"].ToString());
            //            //Update the ptnm amount in the hra_recon table 
            //            //As per Andrea D 6/23/2009
            //            UpdatePutnamAmount(SSN.ToString(), yrmo, ptnmAmt);
            //            ptnmAmt = 0.00;
            //        }
            //        results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'PutnamAdjAmt'");
            //        //populate new destination table
            //        foreach (DataRow dr in results)
            //        {
            //            ptnmAdjAmt = Convert.ToDouble(dr["DistAmount"].ToString());
            //            //Update the ptnm amount in the hra_recon table 
            //            //As per Andrea D 6/23/2009
            //            UpdatePtnmAdjAmount(SSN.ToString(), yrmo, (-1) * (ptnmAdjAmt));
            //            ptnmAdjAmt = 0.00;
            //        }
            //        results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'WgwkAmt'");
            //        //populate new destination table
            //        foreach (DataRow dr in results)
            //        {
            //            wgwkAmt = Convert.ToDouble(dr["DistAmount"].ToString());
            //            //Update the ptnm amount in the hra_recon table 
            //            //As per Andrea D 6/23/2009
            //            UpdateWageWorksAmount(SSN.ToString(), yrmo, (-1) * (wgwkAmt));
            //            wgwkAmt = 0.00;
            //        }
            //      }
            //    }
            //    //end motnhly distribution amounts updates
            //7-6-2009 this routine will update the total difference amounts 
            //Putnam, Putnam ADJ and Wageworks amounts for all the pilots in 
            //Hrarecon table
            
            }
            finally
            {
                connect.Close();
            }
        }
        public static void UpdateTotalDifferenceAmountsforHRARecon(string yrmo)
        {
            //6-23-2009 - update all the ssns returned for the transaction report
            //in the hra_recon table with totaldifference amounts - This number was
            // was not calculating correctly during the hra_recon process
            //
            DataSet dsTotalDifference = new DataSet(); dsTotalDifference.Clear();
            DataSet dsMonthlyAmounts = new DataSet(); dsMonthlyAmounts.Clear();
            List<string> SSNs = new List<string>();
            Double totDifferenceAmt = 0.00;
            Double ptnmAmt = 0.00;
            Double wgwkAmt = 0.00;
            Double ptnmAdjAmt = 0.00;
            DataRow[] results;
            SSNs = GetStringSSNs(yrmo);
            foreach (string SSN in SSNs)
            {
                dsTotalDifference = GetTotalDifferenceAmount(SSN.ToString(), yrmo);
                if (dsTotalDifference.Tables[0].Rows.Count > 0)
                {
                    if (dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty) != DBNull.Value)
                        totDifferenceAmt = Convert.ToDouble(dsTotalDifference.Tables[0].Compute("SUM([DiffAmount])", string.Empty));
                    //Update the total difference amount in the hra_recon table 
                    //As per Andrea D 6/23/2009
                    UpdateTotalDifferenceAmount(SSN.ToString(), yrmo, totDifferenceAmt);
                    totDifferenceAmt = 0.00;

                }
                ///Monthly payments updates in hra_recon table
                dsMonthlyAmounts = GetMonthlyPaymentAmounts(SSN.ToString(), yrmo);
                if (dsMonthlyAmounts.Tables[0].Rows.Count > 0)
                {
                    results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'PutnamAmt'");
                    //populate new destination table
                    foreach (DataRow dr in results)
                    // if (dsMonthlyAmounts.Tables[0].Rows[0].IsNull("PtnmAmount") == false)
                    {
                        ptnmAmt = Convert.ToDouble(dr["DistAmount"].ToString());
                        //Update the ptnm amount in the hra_recon table 
                        //As per Andrea D 6/23/2009
                        UpdatePutnamAmount(SSN.ToString(), yrmo, ptnmAmt);
                        ptnmAmt = 0.00;
                    }
                    results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'PutnamAdjAmt'");
                    //populate new destination table
                    foreach (DataRow dr in results)
                    {
                        ptnmAdjAmt = Convert.ToDouble(dr["DistAmount"].ToString());
                        //Update the ptnm amount in the hra_recon table 
                        //As per Andrea D 6/23/2009
                        UpdatePtnmAdjAmount(SSN.ToString(), yrmo, (-1) * (ptnmAdjAmt));
                        ptnmAdjAmt = 0.00;
                    }
                    results = dsMonthlyAmounts.Tables[0].Select("[colname] = 'WgwkAmt'");
                    //populate new destination table
                    foreach (DataRow dr in results)
                    {
                        wgwkAmt = Convert.ToDouble(dr["DistAmount"].ToString());
                        //Update the ptnm amount in the hra_recon table 
                        //As per Andrea D 6/23/2009
                        UpdateWageWorksAmount(SSN.ToString(), yrmo, (-1) * (wgwkAmt));
                        wgwkAmt = 0.00;
                    }
                }
                //end motnhly distribution amounts updates
            }
        }

        public static DataSet GetDtlTransData(string yrmo)
        {
            List<int> SSNs = new List<int>();
            DataSet dsTemp1 = new DataSet(); dsTemp1.Clear();
            DataSet dsTemp2 = new DataSet(); dsTemp2.Clear();
            DataSet dsFinal = new DataSet(); dsFinal.Clear();
            DataTable tblDet;
            DataRow row;
            DataRow[] rows;

            dsTemp2 = GetPutnamDtlData(yrmo);
            dsTemp1 = GetPutnamAdjDtlData(yrmo);
            dsTemp2.Merge(dsTemp1); dsTemp1.Clear();
            dsTemp1 = GetWageworksTranData(yrmo);
            dsTemp2.Merge(dsTemp1); dsTemp1.Clear();
            dsTemp1 = GetCFTranData(yrmo);
            dsTemp2.Merge(dsTemp1); dsTemp1.Clear();
            dsFinal = dsTemp2.Clone(); tblDet = dsFinal.Tables[0];

            SSNs = GetSSNs(yrmo);
            SSNs.Sort();
            foreach (int SSN in SSNs)
            {
                rows = dsTemp2.Tables[0].Select("SSN = '" + SSN + "'", "[Last Name] ASC, [First Name] ASC, [Transaction Date] DESC");
                dsFinal.Merge(rows);
                if (rows.Length != 0)
                {
                    row = tblDet.NewRow();
                    row["Last Name"] = "** SUB TOTAL:";
                    row["Putnam Amount"] = dsTemp2.Tables[0].Compute("SUM([Putnam Amount])", "SSN = '" + SSN + "'");
                    row["PutnamAdj Amount"] = dsTemp2.Tables[0].Compute("SUM([PutnamAdj Amount])", "SSN = '" + SSN + "'");
                    row["Wageworks Amount"] = dsTemp2.Tables[0].Compute("SUM([Wageworks Amount])", "SSN = '" + SSN + "'");
                    row["Carry Forward Amount"] = dsTemp2.Tables[0].Compute("SUM([Carry Forward Amount])", "SSN = '" + SSN + "'");
                    tblDet.Rows.Add(row);
                }
            }
            if (tblDet.Rows.Count > 0)
            {
                row = tblDet.NewRow();
                row["Last Name"] = "** GRAND TOTAL:";
                row["Putnam Amount"] = tblDet.Compute("SUM([Putnam Amount])", "[Last Name] = '** SUB TOTAL:'");
                row["PutnamAdj Amount"] = tblDet.Compute("SUM([PutnamAdj Amount])", "[Last Name] = '** SUB TOTAL:'");
                row["Wageworks Amount"] = tblDet.Compute("SUM([Wageworks Amount])", "[Last Name] = '** SUB TOTAL:'");
                row["Carry Forward Amount"] = tblDet.Compute("SUM([Carry Forward Amount])", "[Last Name] = '** SUB TOTAL:'");
                tblDet.Rows.Add(row);
            }

            return dsFinal;
        }

        public static Nullable<DateTime> getWgwkPriLateDt(string yrmo)
        {
            HRA hobj = new HRA();
            Nullable<DateTime> wgwkPriLateDt = null;
            string cmdstr = "SELECT MAX(wgwk_createdt) FROM Wageworks WHERE wgwk_yrmo = @yrmo";
            yrmo = hobj.getPrevYRMO(yrmo);

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    wgwkPriLateDt = Convert.ToDateTime(result);
                }

                return wgwkPriLateDt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Nullable<DateTime> getWgwkCurLateDt(string yrmo)
        {
            Nullable<DateTime> wgwkCurLateDt = null;
            string cmdstr = "SELECT MAX(wgwk_createdt) FROM Wageworks WHERE wgwk_yrmo = @yrmo";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    wgwkCurLateDt = Convert.ToDateTime(result);
                }

                return wgwkCurLateDt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Nullable<DateTime> getPtnmCurFirstDt(string yrmo)
        {
            Nullable<DateTime> ptnmCurFirstDt = null;
            string cmdstr = "SELECT MIN(ptnm_distribdt) FROM Putnam WHERE ptnm_yrmo = @yrmo";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmCurFirstDt = Convert.ToDateTime(result);
                }

                return ptnmCurFirstDt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Nullable<DateTime> getPtnmCurLateDt(string yrmo, string _ssn)
        {
            Nullable<DateTime> ptnmCurLateDt = null;
            string cmdstr = "SELECT MAX(ptnm_distribdt) FROM Putnam WHERE ptnm_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmCurLateDt = Convert.ToDateTime(result);
                }

                return ptnmCurLateDt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Nullable<DateTime> getPtnmAdjCurLateDt(string yrmo, string _ssn)
        {
            Nullable<DateTime> ptnmAdjCurLateDt = null;
            string cmdstr = "SELECT MAX(ptna_dt) FROM PutnamAdj WHERE ptna_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptna_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmAdjCurLateDt = Convert.ToDateTime(result);
                }

                return ptnmAdjCurLateDt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getPtnmCurTotAmt(string yrmo, string _ssn)
        {
            Decimal ptnmCurTotAmt = 0;

            string cmdstr = "SELECT SUM(ptnm_distamt) FROM Putnam WHERE ptnm_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmCurTotAmt = Convert.ToDecimal(result);
                }

                return ptnmCurTotAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getWgwkCurTotAmt(string yrmo, string _ssn)
        {
            Decimal wgwkCurTotAmt = 0;

            string cmdstr = "SELECT SUM(wgwk_amt) FROM Wageworks WHERE wgwk_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    wgwkCurTotAmt = Convert.ToDecimal(result);
                }

                return wgwkCurTotAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static int getWgwkCtr(string yrmo, string _ssn)
        {
            int wgwkCtr = 0;

            string cmdstr = "SELECT COUNT(*) FROM Wageworks WHERE wgwk_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                wgwkCtr = Convert.ToInt32(command.ExecuteScalar());

                return wgwkCtr;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getWgwkAmt2(string yrmo, string _ssn)
        {
            Decimal wgwkAmt2 = 0;

            string cmdstr = "SELECT SUM([wgwk_amt]) "
                                + "FROM [Wageworks] "
                                + "WHERE [wgwk_yrmo] = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(wgwk_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', '')))) "
                                + "AND ([wgwk_createdt] < "
                                + "(Select MAX([wgwk_createdt]) "
                                + "FROM [Wageworks] "
                                + "WHERE [wgwk_yrmo] = @yrmo))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    wgwkAmt2 = Convert.ToDecimal(result);
                }

                return wgwkAmt2;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getPriCarryAmt(string yrmo, string _ssn)
        {
            HRA hobj = new HRA();
            yrmo = hobj.getPrevYRMO(yrmo);
            Decimal carryAmt = 0;

            string cmdstr = "SELECT hrcn_carryamt "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    carryAmt = Convert.ToDecimal(result);
                }

                return carryAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getPtnmAdjCurTotAmt(string yrmo, string _ssn)
        {
            Decimal ptnmAdjLateAmt = 0;

            string cmdstr = "SELECT SUM(ptna_amt) "
                                + "FROM PutnamAdj "
                                + "WHERE ptna_yrmo = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptna_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', ''))))";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmAdjLateAmt = Convert.ToDecimal(result);
                }

                return ptnmAdjLateAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void Pass1_InsertPutnam(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnam = new DataSet(); dsPutnam.Clear();
            string cmdstr2 = "";
            string cmdstr1 = "Select ptnm_ssn AS [ssn], "
                                + "ptnm_lname AS [lname], "
                                + "ptnm_fname AS [fname], "
                                + "SUM(ptnm_distamt) AS [amount], "
                                + "COUNT(*) AS [ctr] FROM Putnam "
                                + "WHERE ptnm_yrmo = @yrmo "
                                + "GROUP BY ptnm_ssn, ptnm_lname, ptnm_fname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnam);
                command.Dispose();

                foreach (DataRow row in dsPutnam.Tables[0].Rows)
                {
                    cmdstr2 = "INSERT INTO [hra_recon] "
                                        + "([hrcn_ssn] "
                                        + ",[hrcn_lname] "
                                        + ",[hrcn_fname] "
                                        + ",[hrcn_yrmo] "
                                        + ",[hrcn_ptnmamt] "
                                        + ",[hrcn_ptnmCtr]) "
                                  + "VALUES "
                                        + "('" + row["ssn"] + "' "
                                        + ",'" + row["lname"] + "' "
                                        + ",'" + row["fname"] + "' "
                                        + ",@yrmo "
                                        + "," + row["amount"] + " "
                                        + "," + row["ctr"] + ")";

                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static void Pass2_InsertPutnamAdj(string yrmo)
        {
            int _count;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsPutnamAdj = new DataSet(); dsPutnamAdj.Clear();
            int ptnaCtr;
            Decimal ptnaAmt;
            string cmdstr2, cmdstr3, cmdstr4;
            string cmdstr1 = "Select ptna_ssn AS [ssn], "
                                + "ptna_lname AS [lname], "
                                + "ptna_fname AS [fname], "
                                + "SUM(ptna_amt) AS [amount], "
                                + "COUNT(*) AS [ctr] "
                                + "FROM PutnamAdj "
                                + "WHERE ptna_yrmo = @yrmo "
                                + "GROUP BY ptna_ssn, ptna_lname, ptna_fname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsPutnamAdj);
                command.Dispose();

                foreach (DataRow row in dsPutnamAdj.Tables[0].Rows)
                {
                    _count = 0;
                    cmdstr2 = "SELECT COUNT(*) FROM [hra_recon] "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    _count = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();

                    if (_count == 0)
                    {
                        cmdstr3 = "INSERT INTO [hra_recon] "
                                          + "([hrcn_ssn] "
                                          + ",[hrcn_lname] "
                                          + ",[hrcn_fname] "
                                          + ",[hrcn_yrmo] "
                                          + ",[hrcn_ptnmAdjamt] "
                                          + ",[hrcn_ptnmCtr]) "
                                    + "VALUES "
                                          + "('" + row["ssn"] + "' "
                                          + ",'" + row["lname"] + "' "
                                          + ",'" + row["fname"] + "' "
                                          + ",@yrmo "
                                          + "," + row["amount"] + " "
                                          + "," + row["ctr"] + ")";

                        command = new SqlCommand(cmdstr3, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else
                    {
                        if ((row["ctr"] != null) && (row["ctr"] != DBNull.Value)) ptnaCtr = Convert.ToInt32(row["ctr"]);
                        else ptnaCtr = 0;
                        if ((row["amount"] != null) && (row["amount"] != DBNull.Value)) ptnaAmt = Convert.ToDecimal(row["amount"]);
                        else ptnaAmt = 0;

                        cmdstr4 = "UPDATE [hra_recon] "
                                       + "SET [hrcn_ptnmAdjamt] = " + ptnaAmt + " "
                                          + ",[hrcn_ptnmCtr] = (ISNULL([hrcn_ptnmCtr], 0) + " + ptnaCtr + ") "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                        command = new SqlCommand(cmdstr4, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static void Pass5_InsertDiffAmt(string yrmo)
        {
            HRA hobj = new HRA();
            List<int> SSNs = new List<int>();
            //List<string> strSSNs = new List<string>();
            string priyrmo = hobj.getPrevYRMO(yrmo);
            SSNs = GetSSNs(yrmo);
            string cmdstr = "UPDATE [hra_recon] "
                                + "SET [hrcn_diffamt] = (SELECT dbo.hra_getDiffAmt(ISNULL([hrcn_wgwkamt],0), ISNULL([hrcn_ptnmamt],0), ISNULL([hrcn_ptnmAdjamt],0), @yrmo, @priyrmo, @ssn)) "
                                + "WHERE [hrcn_yrmo] = @yrmo "
                                + "AND CONVERT(int, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = @ssn";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                foreach (int SSN in SSNs)
                {
                    command = new SqlCommand(cmdstr, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    command.Parameters.AddWithValue("@priyrmo", priyrmo);
                    command.Parameters.AddWithValue("@ssn", SSN);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertWageworks(string yrmo, string CFids)
        {
            int _count, wgwkCtr;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWageworks = new DataSet(); dsWageworks.Clear();
            string cmdstr2, cmdstr3, cmdstr4;
            Decimal wgwkAmt, wgwkCFAmt;
            string cmdstr1 = "Select wgwk_ssn AS [ssn], "
                                + "wgwk_lname AS [lname], "
                                + "wgwk_fname AS [fname], "
                                + "SUM(wgwk_amt) AS [wgwkAmt], "
                                + "COUNT(*) AS [ctr] "
                                + "FROM Wageworks "
                                + "WHERE wgwk_yrmo = @yrmo "
                                + "AND wgwk_ID NOT IN (" + CFids + ") "
                                + "GROUP BY wgwk_ssn, wgwk_lname, wgwk_fname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWageworks);
                command.Dispose();

                foreach (DataRow row in dsWageworks.Tables[0].Rows)
                {
                    _count = 0;
                    cmdstr2 = "SELECT COUNT(*) FROM [hra_recon] "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    _count = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();

                    if ((row["ctr"] != null) && (row["ctr"] != DBNull.Value)) wgwkCtr = Convert.ToInt32(row["ctr"]);
                    else wgwkCtr = 0;
                    if ((row["wgwkAmt"] != null) && (row["wgwkAmt"] != DBNull.Value)) wgwkAmt = (-1) * Convert.ToDecimal(row["wgwkAmt"]);
                    else wgwkAmt = 0;

                    if (_count == 0)
                    {
                        cmdstr3 = "INSERT INTO [hra_recon] "
                                           + "([hrcn_ssn] "
                                           + ",[hrcn_lname] "
                                           + ",[hrcn_fname] "
                                           + ",[hrcn_yrmo] "
                                           + ",[hrcn_wgwkamt] "
                                           + ",[hrcn_wgwkCtr]) "
                                     + "VALUES "
                                           + "('" + row["ssn"] + "' "
                                           + ",'" + row["lname"] + "' "
                                           + ",'" + row["fname"] + "' "
                                           + ",@yrmo "
                                           + "," + wgwkAmt + " "
                                           + "," + wgwkCtr + ")";

                        command = new SqlCommand(cmdstr3, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else
                    {
                        cmdstr4 = "UPDATE [hra_recon] "
                                       + "SET [hrcn_wgwkamt] = " + wgwkAmt + " "
                                          + ",[hrcn_wgwkCtr] = " + wgwkCtr + " "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                        command = new SqlCommand(cmdstr4, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertCarryAmt(string yrmo, string CFids)
        {
            int _count, wgwkCtr;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWageworks = new DataSet(); dsWageworks.Clear();
            string cmdstr2, cmdstr3, cmdstr4;
            Decimal wgwkAmt, wgwkCFAmt;
            string cmdstr1 = "Select wgwk_ssn AS [ssn], "
                                + "wgwk_lname AS [lname], "
                                + "wgwk_fname AS [fname], "
                                + "SUM(wgwk_amt) AS [CFAmt], "
                                + "COUNT(*) AS [ctr] "
                                + "FROM Wageworks "
                                + "WHERE wgwk_yrmo = @yrmo "
                                + "AND wgwk_ID IN (" + CFids + ") "
                                + "GROUP BY wgwk_ssn, wgwk_lname, wgwk_fname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWageworks);
                command.Dispose();

                foreach (DataRow row in dsWageworks.Tables[0].Rows)
                {
                    _count = 0;
                    cmdstr2 = "SELECT COUNT(*) FROM [hra_recon] "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    _count = Convert.ToInt32(command.ExecuteScalar());
                    command.Dispose();

                    if ((row["ctr"] != null) && (row["ctr"] != DBNull.Value)) wgwkCtr = Convert.ToInt32(row["ctr"]);
                    else wgwkCtr = 0;
                    if ((row["CFAmt"] != null) && (row["CFAmt"] != DBNull.Value)) wgwkCFAmt = Convert.ToDecimal(row["CFAmt"]);
                    else wgwkCFAmt = 0;

                    if (_count == 0)
                    {
                        cmdstr3 = "INSERT INTO [hra_recon] "
                                           + "([hrcn_ssn] "
                                           + ",[hrcn_lname] "
                                           + ",[hrcn_fname] "
                                           + ",[hrcn_yrmo] "
                                           + ",[hrcn_carryamt] "
                                           + ",[hrcn_wgwkCtr]) "
                                     + "VALUES "
                                           + "('" + row["ssn"] + "' "
                                           + ",'" + row["lname"] + "' "
                                           + ",'" + row["fname"] + "' "
                                           + ",@yrmo "
                                           + "," + wgwkCFAmt + " "
                                           + "," + wgwkCtr + ")";

                        command = new SqlCommand(cmdstr3, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                    else
                    {
                        cmdstr4 = "UPDATE [hra_recon] "
                                       + "SET [hrcn_carryamt] = " + wgwkCFAmt + " "
                                          + ",[hrcn_wgwkCtr] = ISNULL([hrcn_wgwkCtr],0) + " + wgwkCtr + " "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + row["ssn"] + "', '-', ''))))";

                        command = new SqlCommand(cmdstr4, connect);
                        command.Parameters.AddWithValue("@yrmo", yrmo);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertWageworks(string yrmo, string _ssn, string lname, string fname, Decimal _amt, int _ctr)
        {
            int _count = 0;
            string cmdstr1 = "SELECT COUNT(*) FROM [hra_recon] "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + _ssn + "', '-', ''))))";

            string cmdstr2 = "INSERT INTO [hra_recon] "
                                           + "([hrcn_ssn] "
                                           + ",[hrcn_lname] "
                                           + ",[hrcn_fname] "
                                           + ",[hrcn_yrmo] "
                                           + ",[hrcn_wgwkamt] "
                                           + ",[hrcn_wgwkCtr]) "
                                     + "VALUES "
                                           + "('" + _ssn + "' "
                                           + ",'" + lname + "' "
                                           + ",'" + fname + "' "
                                           + ",@yrmo "
                                           + "," + _amt + " "
                                           + "," + _ctr + ")";

            string cmdstr3 = "UPDATE [hra_recon] "
                                       + "SET [hrcn_wgwkamt] = (ISNULL([hrcn_wgwkamt], 0) + " + _amt + ") "
                                          + ",[hrcn_wgwkCtr] = (ISNULL([hrcn_wgwkCtr], 0) + " + _ctr + ") "
                                    + "WHERE hrcn_yrmo = @yrmo "
                                    + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE('" + _ssn + "', '-', ''))))";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                _count = Convert.ToInt32(command.ExecuteScalar());
                command.Dispose();

                if (_count == 0)
                {
                    command = new SqlCommand(cmdstr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                else
                {
                    command = new SqlCommand(cmdstr3, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet getWageworksData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsWgwk = new DataSet(); dsWgwk.Clear();

            string cmdstr = "SELECT [wgwk_ID] AS [id] "
                                  + ",[wgwk_transtype] AS [trantype] "
                                  + ",[wgwk_lname] AS [lname] "
                                  + ",[wgwk_fname] AS [fname] "
                                  + ",[wgwk_createdt] AS [createdt] "
                                  + ",[wgwk_amt] AS [amount] "
                                  + ",[wgwk_ssn] AS [ssn] "
                                + "FROM [Wageworks] "
                                + "WHERE [wgwk_yrmo] = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsWgwk);
                command.Dispose();

                return dsWgwk;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void InsertCFData(string yrmo, string CFids)
        {
            string cmdstr = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[period] "
                                + ",[source] "
                                + ",[content]) "
                                + "VALUES "
                                + "('HRARecon' "
                                + ",@yrmo "
                                + ",'CF' "
                                + ",'" + CFids + "')";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery(); command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        static string GetCFids(string yrmo)
        {
            string CFids = "-1";
            string cmdstr = "SELECT [content] "
                                + "FROM [hra_content] "
                                + "WHERE [category] = 'HRARecon' "
                                + "AND [period] = @yrmo "
                                + "AND [source] = 'CF'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    CFids = result.ToString();
                }

                return CFids;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean HasVariance(string yrmo)
        {
            Boolean hasVar;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM hra_recon WHERE hrcn_yrmo = @yrmo AND ISNULL(hrcn_diffamt,0) <> 0", connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasVar = true;
                }
                else
                {
                    hasVar = false;
                }
                command.Dispose();

                return hasVar;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean HasCF(string yrmo)
        {
            Boolean hasCF;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM hra_recon WHERE hrcn_yrmo = @yrmo AND ISNULL(hrcn_carryamt,0) <> 0", connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasCF = true;
                }
                else
                {
                    hasCF = false;
                }
                command.Dispose();

                return hasCF;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean HasPriCF(string yrmo)
        {
            Boolean hasPriCF = false;
            DataSet ds = new DataSet(); ds.Clear();

            ds = HRA_ReconDAL.GetPrevCFRptData(yrmo);

            if (ds.Tables[0].Rows.Count > 0) { return true; }
            else { return false; }
        }

        public static Boolean HasTran(string yrmo)
        {
            Boolean hasTran;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM hra_recon WHERE hrcn_yrmo = @yrmo", connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    hasTran = true;
                }
                else
                {
                    hasTran = false;
                }
                command.Dispose();

                return hasTran;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal getPtnmCurFirstAmt(string yrmo, string _ssn, string _trandt)
        {
            Decimal ptnmCurFirstAmt = 0;
            string cmdstr = "SELECT SUM(ptnm_distamt) FROM Putnam WHERE ptnm_yrmo = @yrmo AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ptnm_ssn, '-', '')))) = CONVERT(INT, LTRIM(RTRIM(REPLACE(@ssn, '-', '')))) AND ptnm_distribdt = @trandt";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@ssn", _ssn);
                command.Parameters.AddWithValue("@trandt", _trandt);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    ptnmCurFirstAmt = Convert.ToDecimal(result);
                }

                return ptnmCurFirstAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        static List<int> GetSSNs(string yrmo)
        {
            List<int> SSNs = new List<int>();
            SqlDataReader dr = null;
            string cmdstr = "SELECT DISTINCT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    SSNs.Add(dr.GetInt32(0));
                }
                dr.Close();

                return SSNs;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed))
                {
                    dr.Close();
                }
                connect.Close();
            }
        }
        static List<string> GetStringSSNs(string yrmo)
        {
            List<string> SSNs = new List<string>();
            SqlDataReader dr = null;
            string cmdstr = "SELECT DISTINCT CONVERT(varchar, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    SSNs.Add(dr[0].ToString());
                }
                dr.Close();

                return SSNs;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed))
                {
                    dr.Close();
                }
                connect.Close();
            }
        }
        static List<int> GetDispSSNs(string yrmo)
        {
            List<int> dispSSNs = new List<int>();
            //SortableList<int> displaySSNs = new SortableList<int>();
            SqlDataReader dr = null;
            string cmdstr = "SELECT DISTINCT CONVERT(INT, LTRIM(RTRIM(REPLACE(hrcn_ssn, '-', '')))) "
                                + "FROM hra_recon "
                                + "WHERE hrcn_yrmo = @yrmo "
                                + "AND ISNULL(hrcn_diffamt,0) <> 0 "
                                + "AND ((-1) * (ISNULL(hrcn_ptnmamt,0) + ISNULL(((-1) * hrcn_wgwkamt),0) + ISNULL(hrcn_ptnmAdjamt,0))) <> 0 ";
            //current diff amount - ((-1) * (ISNULL(hrcn_ptnmamt,0) + ISNULL(((-1) * hrcn_wgwkamt),0) + ISNULL(hrcn_ptnmAdjamt,0))) <> 0
            //Added the above condition since in the summary transaction tab Todd wants to see the transactions if Current Difference amount is zero
            //Remove the Detail transactions for employees where current diff amount is zero 
            //This will exclude all the employees SSN whose current diff amount is zero  3/27/2009 R.A

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    dispSSNs.Add(dr.GetInt32(0));
                 }
                dr.Close();
                dispSSNs.Sort();
               return dispSSNs;
               
            }
            finally
            {
                if (dr != null && !(dr.IsClosed))
                {
                    dr.Close();
                }
                connect.Close();
            }
        }



        public static void InsertPrevCFNotCleared(DataSet ds, string yrmo)
        {
            string _filecontent;
            string cmdstr = "INSERT INTO [hra_content] "
                                + "([category] "
                                + ",[period] "
                                + ",[source] "
                                + ",[content]) "
                                + "VALUES "
                                + "('HRARecon' "
                                + ",@yrmo "
                                + ",'PriCF' "
                                + ",@filecontent)";

            if (ds.Tables[0].Rows.Count > 0)
            {
                _filecontent = ds.GetXml();
            }
            else
            {
                _filecontent = ds.GetXmlSchema();
            }

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@filecontent", _filecontent);
                command.ExecuteNonQuery(); command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetPrevCFRptData(string yrmo)
        {
            DataSet ds = new DataSet(); ds.Clear();
            StringReader _reader;
            string _temp;
            //string cmdstr = "SELECT [content] "
            //                    + "FROM [hra_content] "
            //                    + "WHERE [category] = 'HRARecon' "
            //                    + "AND [period] = @yrmo "
            //                    + "AND [source] = 'PriCF'";
            // Changed the query to retrieve all the Carry forward amounts that did not clear as of prior month 
            // (< less than) current yrmo  //This was requestd the HRA Enhancements by Todd F 3/20/2009
            //R.A 3/27/2009
            string cmdstr = "SELECT [content] "
                                + "FROM [hra_content] "
                                + "WHERE [category] = 'HRARecon' "
                                + "AND [period] < @yrmo "
                                + "AND [source] = 'PriCF'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                _temp = command.ExecuteScalar().ToString();
                _reader = new StringReader(_temp);
                ds.ReadXml(_reader);

                return ds;
            }
            finally
            {
                connect.Close();
            }

        }
    }

   

}
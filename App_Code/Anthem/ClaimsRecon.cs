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
using EBA.Desktop;
using EBA.Desktop.Anthem;

namespace EBA.Desktop.Anthem
{

    /// <summary>
    /// Summary description for ClaimsRecon
    /// </summary>
    public class ClaimsRecon
    {
        private string connStr;
        private string connStr2;
        private SqlConnection connect = null;
        private SqlConnection connectFRD = null;
        private SqlCommand command = null;

        public ClaimsRecon()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            connect = new SqlConnection(connStr);
            connStr2 = ConfigurationManager.ConnectionStrings["FRDDB"].ConnectionString;
            connectFRD = new SqlConnection(connStr2);
        }


        /// <summary>
        /// Compare DF Report with DF/RF. Discepancies are then compared to DF no RF report
        /// if they are not appearing on this report then they are marked as discrepancy.
        /// Previous discrepancies are matched to present yrmo DF with no RF, DFRF matching report,
        /// if they appear they are marked reconciled.
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        public void rfdfFirstVerification(string _yrmo)
        {
            setPrevYRMOSummary(_yrmo, "DFnoRF");
            setPrevYRMOSummary(_yrmo, "DFRF");

            List<string> _anthDiscrepancies = matchDFwithDFRFDFnoRF(_yrmo); 
            //List<string> _prevDFnoRFDiscrepanciesCleared = prevClearedDiscrepancies(_yrmo);
            prevClearedDiscrepanciesUpdate(_yrmo);

            List<string> _droppedDFnoRFDiscrepancies = dfnorfDroppedoffUnmatchedDFRF(_yrmo);            
            List<string> _prevDFRFDiscrepanciesCleared = prevDFRFClearedDiscrepancies(_yrmo);


            updateDBClaims(_anthDiscrepancies, _droppedDFnoRFDiscrepancies, _prevDFRFDiscrepanciesCleared, _yrmo);
        }

        private void updateDBClaims(List<string> _updDCN,List<string> _rfdfDis, List<string> _updDFRFDCN, string _yrmo)
        {
            string _prevyrmo = AnthRecon.prevYRMO(_yrmo);

            string _cmdstr = "UPDATE AnthBillTrans SET anbt_Discp = 1"
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo = '" + _yrmo + "' AND anbt_claimid = @dcn";            

            string _cmdstr2 = "UPDATE RFDF SET rfdf_Discp = 1"
                                + "WHERE rfdf_source = 'DF' AND rfdf_yrmo = '" + _prevyrmo + "' AND rfdf_dcn = @dcn";

            string _cmdstr4 = "UPDATE RFDF SET rfdf_DiscpCleared = 1,rfdf_DiscpClearedYRMO = '" + _yrmo + "' "
                               + "WHERE rfdf_source = 'DF' AND rfdf_Discp = 1 AND rfdf_yrmo < '" + _prevyrmo + "' AND rfdf_dcn = @dcn";


            SqlTransaction ts;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                if (_updDCN.Count > 0)
                {
                    foreach (string _dcn in _updDCN)
                    {
                        command = new SqlCommand(_cmdstr, connect, ts);
                        command.Parameters.AddWithValue("@dcn", _dcn);
                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();

                        //command = new SqlCommand(_cmdstr0, connect, ts);
                        //command.Parameters.AddWithValue("@dcn", _dcn);
                        //command.ExecuteNonQuery();
                    }
                }

                //if (_updPrevDCN!=null)
                //{
                //    foreach (string _dcn in _updPrevDCN)
                //    {
                //        command = new SqlCommand(_cmdstr1, connect, ts);
                //        command.Parameters.AddWithValue("@dcn", _dcn);
                //        command.ExecuteNonQuery();

                //        command = new SqlCommand(_cmdstr10, connect, ts);
                //        command.Parameters.AddWithValue("@dcn", _dcn);
                //        command.ExecuteNonQuery();
                //    }
                //}

                if (_rfdfDis!=null)
                {
                    foreach (string _dcn in _rfdfDis)
                    {
                        command = new SqlCommand(_cmdstr2, connect, ts);
                        command.Parameters.AddWithValue("@dcn", _dcn);
                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();
                    }
                }

                if (_updDFRFDCN!=null)
                {
                    foreach (string _dcn in _updDFRFDCN)
                    {
                        command = new SqlCommand(_cmdstr4, connect, ts);
                        command.Parameters.AddWithValue("@dcn", _dcn);
                        command.CommandTimeout = 0;
                        command.ExecuteNonQuery();
                    }
                }
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw ex;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }
       
        /// <summary>
        /// Get list of DF DCNs not present in DFRF Report.
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <returns>List of unmatched DCNs</returns>        
        private List<string> matchDFwithDFRFDFnoRF(string _yrmo)
        {
            List<string> _anthDCN = new List<string>();
            string _cmdstr = "SELECT anbt_claimid FROM AnthBillTrans "
                                + "WHERE anbt_claimid NOT IN "
                                + "(select rfdf_dcn FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "') "
                                + "AND anbt_yrmo = '" + _yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT'";

            string _cmdstr1 = " SELECT anbt_claimid FROM "
                                + "(SELECT anbt_claimid, SUM(anbt_claimsPdAmt) AS totalDFAmt,totalRFAmount "
                                + "FROM "
                                + "AnthBillTrans a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, SUM(rfdf_amt) AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "' "
                                + "GROUP BY rfdf_dcn) r "
                                + "ON (a.anbt_claimid = r.rfdf_dcn) "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_yrmo = '" + _yrmo + "' "
                                + "GROUP BY a.anbt_claimid,totalRFAmount "
                                + "HAVING (SUM(anbt_claimsPdAmt) - totalRFAmount) <> 0) AS discp1 ";

            SqlDataReader reader;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _anthDCN.Add(reader[0].ToString());
                }
                reader.Close();

                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _anthDCN.Add(reader[0].ToString());
                }
                reader.Close();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            return _anthDCN;
        }

       

        /// <summary>
        /// Get List of previous discrepancies.
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <returns>List of previous discrepancies that cleared in present yrmo</returns>
        private List<string> prevClearedDiscrepancies(string _yrmo)
        {
            List<string> _prevDisc = new List<string>();           

            string _cmdstr1 = " SELECT anbt_claimid FROM "
                                + "(SELECT anbt_claimid, SUM(anbt_claimsPdAmt) AS totalDFAmt,totalRFAmount "
                                + "FROM "
                                + "AnthBillTrans a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, rfdf_amt AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "' AND rfdf_source = 'DF' "
                                + ") r "
                                + "ON (a.anbt_claimid = r.rfdf_dcn) "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_yrmo < '" + _yrmo + "' "
                                + "AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                                + "GROUP BY a.anbt_claimid,totalRFAmount "
                                + "HAVING (SUM(anbt_claimsPdAmt) - totalRFAmount) = 0) AS discp2 ";

            string _cmdstr = " SELECT anbt_claimid FROM "
                                + "(SELECT anbt_claimid, SUM(anbt_claimsPdAmt) AS totalDFAmt,totalRFAmount "
                                + "FROM "
                                + "AnthBillTrans a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, SUM(rfdf_amt) AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo <= '" + _yrmo + "' AND rfdf_source = 'RF' "
                                + "GROUP BY rfdf_dcn) r "
                                + "ON (a.anbt_claimid = r.rfdf_dcn) "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_yrmo < '" + _yrmo + "' "
                                + "AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                                + "GROUP BY a.anbt_claimid,totalRFAmount "
                                + "HAVING (SUM(anbt_claimsPdAmt) - totalRFAmount) = 0) AS discp6 ";
            
            SqlDataReader reader;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                    {
                        _prevDisc.Add(reader[0].ToString());
                    }
                }
                reader.Close();

                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                    {
                        _prevDisc.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            return _prevDisc;
        }

        protected void prevClearedDiscrepanciesUpdate(string _yrmo)
        {
            List<string> _prevDisc = new List<string>();

            string _cmdstr1 = " UPDATE AnthBillTrans SET anbt_DiscpCleared = 1,anbt_DiscpClearedYRMO = '" + _yrmo + "' "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo < '" + _yrmo + "' "
                                + " AND anbt_Discp = 1 AND anbt_claimid IN ( "
                                + " SELECT anbt_claimid FROM "
                                + "(SELECT anbt_claimid, SUM(anbt_claimsPdAmt) AS totalDFAmt,totalRFAmount "
                                + "FROM "
                                + "AnthBillTrans a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, rfdf_amt AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "' AND rfdf_source = 'DF' "
                                + ") r "
                                + "ON (a.anbt_claimid = r.rfdf_dcn) "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_yrmo < '" + _yrmo + "' "
                                + "AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                                + "GROUP BY a.anbt_claimid,totalRFAmount "
                                + "HAVING (SUM(anbt_claimsPdAmt) - totalRFAmount) = 0) AS discp2) ";

            string _cmdstr = " UPDATE AnthBillTrans SET anbt_DiscpCleared = 1,anbt_DiscpClearedYRMO = '" + _yrmo + "' "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo < '" + _yrmo + "' "
                                + " AND anbt_Discp = 1 AND anbt_claimid IN ( "
                                + " SELECT anbt_claimid FROM "
                                + "(SELECT anbt_claimid, SUM(anbt_claimsPdAmt) AS totalDFAmt,totalRFAmount "
                                + "FROM "
                                + "AnthBillTrans a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, SUM(rfdf_amt) AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo <= '" + _yrmo + "' AND rfdf_source = 'RF' "
                                + "GROUP BY rfdf_dcn) r "
                                + "ON (a.anbt_claimid = r.rfdf_dcn) "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_yrmo < '" + _yrmo + "' "
                                + "AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                                + "GROUP BY a.anbt_claimid,totalRFAmount "
                                + "HAVING (SUM(anbt_claimsPdAmt) - totalRFAmount) = 0) AS discp6) ";

            SqlTransaction ts;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr1, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                ts.Commit();

            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw ex;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            
        }

        /// <summary>
        /// Compare Last Months Aging report to Present month Aging report and noting the records
        /// that dropped off on the current YRMO and dont match with current RF
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <returns>Dropped off Records and unmatched with DFRF</returns>
        private List<string> dfnorfDroppedoffUnmatchedDFRF(string _yrmo)
        {
            List<string> _dfnorfRecords = new List<string>();

            string _prevyrmo = AnthRecon.prevYRMO(_yrmo);

            string _cmdstr = "SELECT a.rfdf_dcn FROM RFDF a "
                                + " WHERE a.rfdf_dcn NOT IN "
                                + "(select rfdf_dcn FROM RFDF "
                                + " WHERE rfdf_yrmo = '" + _yrmo + "') "
                                + " AND a.rfdf_yrmo = '" + _prevyrmo + "' AND a.rfdf_source = 'DF'";

            string _cmdstr1 = "SELECT rfdf_dcn FROM "
                                + "(SELECT a.rfdf_dcn, SUM(rfdf_amt) AS totalAmount,totalRFAmount  "
                                + "FROM RFDF a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, SUM(rfdf_amt) as totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "' "
                                + "AND rfdf_source = 'RF' "
                                + "GROUP BY rfdf_dcn) r "
                                + "ON (a.rfdf_dcn = r.rfdf_dcn) "
                                + "WHERE rfdf_source = 'DF' "
                                + "AND rfdf_yrmo = '" + _prevyrmo + "' "
                                + "AND a.rfdf_dcn NOT IN "
                                + "( "
                                + "SELECT rfdf_dcn "
                                + "FROM RFDF "
                                + "WHERE rfdf_source = 'DF' "
                                + "AND rfdf_yrmo = '" + _yrmo + "' "
                                + ") "
                                + "GROUP BY a.rfdf_dcn,totalRFAmount "
                                + "HAVING (SUM(rfdf_amt) - totalRFAmount) <> 0 ) as Discp4 ";

            SqlDataReader reader;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                    {
                        _dfnorfRecords.Add(reader[0].ToString());
                    }
                }
                reader.Close();

                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                    {
                        _dfnorfRecords.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            return _dfnorfRecords;
        }

       
       
        /// <summary>
        /// Get List of previous discrepancies.
        /// </summary>
        /// <param name="_yrmo">YRMO</param>
        /// <returns>List of previous discrepancies</returns>
        private List<string> prevDFRFClearedDiscrepancies(string _yrmo)
        {
            List<string> _prevDFRFDisc = new List<string>();
            string _prevyrmo = AnthRecon.prevYRMO(_yrmo);           

            string _cmdstr = "SELECT rfdf_dcn FROM "
                                + "(SELECT a.rfdf_dcn, rfdf_amt AS totalAmount,totalRFAmount  "
                                + "FROM RFDF a "
                                + "INNER JOIN "
                                + "(SELECT rfdf_dcn, SUM(rfdf_amt) AS totalRFAmount "
                                + "FROM RFDF "
                                + "WHERE rfdf_yrmo = '" + _yrmo + "' "
                                + "AND rfdf_source = 'RF' "
                                + "GROUP BY rfdf_dcn) r "
                                + "ON (a.rfdf_dcn = r.rfdf_dcn) "
                                + "WHERE rfdf_source = 'DF' "
                                + "AND rfdf_yrmo < '" + _prevyrmo + "' "                                
                                + "AND rfdf_Discp = 1 AND rfdf_DiscpCleared = 0 "                                
                                + "AND (rfdf_amt - totalRFAmount) = 0 ) as Discp5 ";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] != DBNull.Value)
                    {
                        _prevDFRFDisc.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            return _prevDFRFDisc;
        }      

       

        public void CompareFRD_DFClaims(string _yrmo)
        {
            decimal _frdAmt = getFRDClaimsTotal(_yrmo);
            decimal _dfAmt = getDFClaimsTotal(_yrmo);
            decimal _cfAmt = getDFRFCarryF(_yrmo);
            decimal _diffAmt = (_frdAmt + _cfAmt) - _dfAmt;
            insertDFClaimsRecon(_frdAmt, _dfAmt, _diffAmt, _cfAmt, _yrmo);
        }


        private Decimal getDFClaimsTotal(string _yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            string cmdStr = "SELECT sum(anbt_ClaimsPdAmt) "
                            + " FROM AnthbillTrans "
                            + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                            + "AND anbt_yrmo = '" + _yrmo + "'";
            decimal  _totDFamt;
            command = new SqlCommand(cmdStr, connect);
            object _temp = command.ExecuteScalar();

            if (_temp != DBNull.Value)
            {
                _totDFamt = Convert.ToDecimal(_temp);
            }
            else
            {
                _totDFamt = 0;
            }
            
            connect.Close();

            return _totDFamt;
        }

        private Decimal getFRDClaimsTotal(string _yrmo)
        {
            if (connectFRD == null || connectFRD.State == ConnectionState.Closed)
            {
                connectFRD.Open();
            }
            
            string cmdStr = "SELECT SUM(totamt) "
                            + " FROM fnpaytrans "
                            + "WHERE provid = 'ANTHEMCLAIM' "
                            + "AND yrmo = '" + _yrmo + "'";

            command = new SqlCommand(cmdStr, connectFRD);
            decimal _totFRDamt;
            object _temp = command.ExecuteScalar();

            if (_temp != DBNull.Value)
            {
                _totFRDamt = Convert.ToDecimal(_temp);
            }
            else
            {
                _totFRDamt = 0;
            }
            connectFRD.Close();

            return _totFRDamt;
        }

        private Decimal getDFRFCarryF(string _yrmo)
        {
            string _prevyrmo = AnthRecon.prevYRMO(_yrmo);
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            string cmdStr = "SELECT rf_diff "
                            + " FROM rf_recon "
                            + "WHERE rf_yrmo = '" + _prevyrmo + "'";
            decimal _CFamt;
            command = new SqlCommand(cmdStr, connect);
            object _temp = command.ExecuteScalar();

            if (_temp != DBNull.Value)
            {
                _CFamt = Convert.ToDecimal(_temp);
            }
            else
            {
                _CFamt = 0;
            }

            connect.Close();

            return _CFamt;
        }

        private void insertDFClaimsRecon(decimal _FRD, decimal _DF, decimal _DIFF, decimal _CF,string _yrmo)
        {
            string _cmdstr = "INSERT INTO rf_recon (rf_yrmo,rf_frd_amt,rf_df_amt,rf_diff,rf_CF_amt) "
                                + "VALUES (@yrmo, @frd, @df,@diff, @cf) ";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(_cmdstr, connect);
            command.Parameters.AddWithValue("@yrmo", _yrmo);
            command.Parameters.AddWithValue("@frd", _FRD);
            command.Parameters.AddWithValue("@df", _DF);
            command.Parameters.AddWithValue("@diff", _DIFF);
            command.Parameters.AddWithValue("@cf", _CF);
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
            connect.Close();
        }

        public DataSet DF_DFRFClaimsRecon(string yrmo)
        {
            string _prevyrmo = AnthRecon.prevYRMO(yrmo);

            string _cmdstr = "SELECT rf_yrmo AS [YRMO],CONVERT(VARCHAR(20),rf_frd_amt,1) AS [FRD Amount], "
                                + " CONVERT(VARCHAR(20),rf_df_amt,1) AS [DF Claim Report], "
                                + " CONVERT(VARCHAR(20),rf_CF_amt,1) AS [Carry Forward Amount], "
                                + " CONVERT(VARCHAR(20),rf_diff,1) AS [Variance] "
                                + " FROM rf_recon WHERE rf_yrmo = '" + yrmo + "'";

            string _cmdstr1 = "SELECT rf_yrmo AS [YRMO],CONVERT(VARCHAR(20),rf_frd_amt,1) AS [FRD Amount], "
                                + " CONVERT(VARCHAR(20),rf_df_amt,1) AS [DF Claim Report], "
                                + " CONVERT(VARCHAR(20),rf_CF_amt,1) AS [Carry Forward Amount], "
                                + " CONVERT(VARCHAR(20),rf_diff,1) AS [Variance] "
                                + " FROM rf_recon WHERE rf_yrmo = '" + yrmo + "' OR rf_yrmo = '" + _prevyrmo + "' "
                                + " ORDER BY rf_yrmo";

            string _cmdstr2 = "SELECT CONVERT(VARCHAR(20),SUM(rf_frd_amt),1), "
                                + " CONVERT(VARCHAR(20),SUM(rf_df_amt),1), "
                                + " CONVERT(VARCHAR(20),SUM(rf_frd_amt) - SUM(rf_df_amt),1) "
                                + " FROM rf_recon WHERE rf_yrmo <= '" + yrmo + "'";

            DataSet dsRecon = new DataSet();
            dsRecon.Clear();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlDataReader reader;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                if (yrmo.Equals("200801"))
                {
                    command = new SqlCommand(_cmdstr, connect);
                }
                else
                {
                    command = new SqlCommand(_cmdstr1, connect);
                }
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRecon);

                command = new SqlCommand(_cmdstr2, connect);
                command.CommandTimeout = 0;
                reader = command.ExecuteReader();                
                if(reader.Read())
                {                    
                    DataRow row1 = dsRecon.Tables[0].NewRow();
                    row1["YRMO"] = "200801 - " + yrmo;
                    row1["FRD Amount"] = reader[0];
                    row1["DF Claim Report"] = reader[1];
                    row1["Carry Forward Amount"] = "";
                    row1["Variance"] = reader[2];
                    dsRecon.Tables[0].Rows.Add(row1);
                }
                reader.Close();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsRecon;
        }

        public DataSet DFnoRFAging(string yrmo)
        {
            string prev_yrmo= AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);

            string _cmdstr = "SELECT anbt_yrmo AS [YRMO], anbt_subid_ssn AS [SSN]," 
                                + " anbt_claimid AS [Claim #], anbt_name AS [NAME],"
                                + " CONVERT(VARCHAR(15),anbt_servfromdt,101) AS [Service From Dt], "
                                + " CONVERT(VARCHAR(15),anbt_servthrudt,101) AS [Service Thru Dt], "
                                + " CONVERT(VARCHAR(15),anbt_datePd,101) AS [Paid Date],anbt_claimsType AS [Claims Type], "
                                + " CASE "
                                + " WHEN anbt_yrmo = '" + yrmo + "' "
                                + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                + " ELSE 0 "
                                + " END AS [Current YRMO] "
                                + " ,CASE "
                                + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                + " ELSE 0 "
                                + " END AS [Previous YRMO] "
                                + " ,CASE "
                                + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                + " ELSE 0 "
                                + " END AS [Prior YRMO] "                               
                                + " FROM AnthBillTrans "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' and anbt_Discp = 1 and anbt_DiscpCleared = 0"
                                + " AND anbt_yrmo <= '" + yrmo + "' "
                                + " ORDER BY anbt_yrmo, anbt_claimid"; 

            DataSet dsDFnoRF = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsDFnoRF);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            if (dsDFnoRF.Tables[0].Rows.Count > 0)
            {
                decimal _presentAmt = Convert.ToDecimal(dsDFnoRF.Tables[0].Compute("SUM([Current YRMO])", String.Empty));
                decimal _prevAmt = Convert.ToDecimal(dsDFnoRF.Tables[0].Compute("SUM([Previous YRMO])", String.Empty));
                decimal _priorAmt = Convert.ToDecimal(dsDFnoRF.Tables[0].Compute("SUM([Prior YRMO])", String.Empty));

                DataRow row = dsDFnoRF.Tables[0].NewRow();
                row["Claims Type"] = "Total: ";
                row["Current YRMO"] = _presentAmt;
                row["Previous YRMO"] = _prevAmt;
                row["Prior YRMO"] = _priorAmt;
                dsDFnoRF.Tables[0].Rows.Add(row);
            }
            
            return dsDFnoRF;
        }

        public DataSet DFRFAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);

            string _cmdstr = "SELECT   rfdf_yrmo AS [YRMO],rfdf_subid AS [MemberID] " 
                            + " ,rfdf_dcn AS [DCN],CONVERT(VARCHAR(15),rfdf_lastupdt,101) AS [LastUpdate], "                             
                            + " CASE "
                            + " WHEN rfdf_yrmo = '" + yrmo + "' "
                            + " THEN CONVERT(Numeric(10,2),rfdf_amt) "
                            + " ELSE 0 "
                            + " END AS [Current YRMO] "
                            + " ,CASE "
                            + " WHEN rfdf_yrmo = '" + prev_yrmo + "' "
                            + " THEN CONVERT(Numeric(10,2),rfdf_amt) "
                            + " ELSE 0 "
                            + " END AS [Previous YRMO] "
                            + " ,CASE "
                            + " WHEN rfdf_yrmo <= '" + prior_yrmo + "' "
                            + " THEN CONVERT(Numeric(10,2),rfdf_amt) "
                            + " ELSE 0 "
                            + " END AS [Prior YRMO] "
                            + " FROM RFDF "
                            + " WHERE rfdf_source = 'DF' and rfdf_Discp = 1 and rfdf_DiscpCleared = 0"
                            + " AND rfdf_yrmo <= '" + yrmo +"' "
                            + " ORDER BY rfdf_yrmo ";

            DataSet dsRFDF = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRFDF);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            if (dsRFDF.Tables[0].Rows.Count > 0)
            {
                decimal _presentAmt = Convert.ToDecimal(dsRFDF.Tables[0].Compute("SUM([Current YRMO])", String.Empty));
                decimal _prevAmt = Convert.ToDecimal(dsRFDF.Tables[0].Compute("SUM([Previous YRMO])", String.Empty));
                decimal _priorAmt = Convert.ToDecimal(dsRFDF.Tables[0].Compute("SUM([Prior YRMO])", String.Empty));

                DataRow row = dsRFDF.Tables[0].NewRow();
                row["LastUpdate"] = "Total: ";
                row["Current YRMO"] = _presentAmt;
                row["Previous YRMO"] = _prevAmt;
                row["Prior YRMO"] = _priorAmt;
                dsRFDF.Tables[0].Rows.Add(row);
            }
            
            return dsRFDF;
        }

        public DataSet DFnoRFAging(string yrmo,string _dcn)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);            

            string _cmdstr = "SELECT anbt_yrmo AS [YRMO], 'ANTHEM DF' AS [Source], anbt_claimid AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),anbt_datePd,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),0) AS [DFRF Amount] "
                                + " FROM AnthBillTrans "
                                + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' AND anbt_claimid = '" + _dcn + "' ";

            string _cmdstr1 = "SELECT rfdf_yrmo AS [YRMO], 'DFRF' AS [Source], rfdf_dcn AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),0) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),rfdf_amt) AS [DFRF Amount] "
                                + " FROM RFDF "
                                + " WHERE rfdf_source ='RF' AND rfdf_yrmo <= '" + yrmo + "' AND rfdf_dcn  = '" + _dcn + "'";

            string _cmdstr2 = "SELECT rfdf_yrmo AS [YRMO], 'DFnoRF' AS [Source], rfdf_dcn AS [Claim ID], "
                               + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS [Date Paid], "
                               + " CONVERT(Numeric(10,2),0) AS [Anthem DF Amount], "
                               + " CONVERT(Numeric(10,2),rfdf_amt) AS [DFRF Amount] "
                               + " FROM RFDF "
                               + " WHERE rfdf_source ='DF' AND rfdf_yrmo <= '" + yrmo + "' AND rfdf_dcn  = '" + _dcn + "'"; 

            DataSet dsDFnoRF1 = new DataSet();
            DataSet dsDFnoRF2 = new DataSet();
            DataSet dsDFnoRF3 = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsDFnoRF1);
                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsDFnoRF2);
                dsDFnoRF1.Merge(dsDFnoRF2);
                dsDFnoRF2.Clear();
                command = new SqlCommand(_cmdstr2, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsDFnoRF3);
                dsDFnoRF1.Merge(dsDFnoRF3);
                dsDFnoRF3.Clear();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsDFnoRF1;
        }

        public DataSet DFRFAging(string yrmo,string _dcn)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);            

            string _cmdstr = "SELECT anbt_yrmo AS [YRMO], 'ANTHEM DF' AS [Source], anbt_claimid AS [Claim ID], "
                               + " CONVERT(VARCHAR(15),anbt_datePd,101) AS [Date Paid], "
                               + " CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Anthem DF Amount], "
                               + " CONVERT(Numeric(10,2),0) AS [DFRF Amount] "
                               + " FROM AnthBillTrans "
                               + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' AND anbt_claimid = '" + _dcn + "' ";

            string _cmdstr1 = "SELECT rfdf_yrmo AS [YRMO], 'DFRF' AS [Source], rfdf_dcn AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),0) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),rfdf_amt) AS [DFRF Amount] "
                                + " FROM RFDF "
                                + " WHERE rfdf_source ='RF' AND rfdf_yrmo <= '" + yrmo + "' AND rfdf_dcn  = '" + _dcn + "'";

            string _cmdstr2 = "SELECT rfdf_yrmo AS [YRMO], 'DFnoRF' AS [Source], rfdf_dcn AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),0) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),rfdf_amt) AS [DFRF Amount] "
                                + " FROM RFDF "
                                + " WHERE rfdf_source ='DF' AND rfdf_yrmo <= '" + yrmo + "' AND rfdf_dcn  = '" + _dcn + "'"; 

            DataSet dsRFDF1 = new DataSet();
            DataSet dsRFDF2 = new DataSet();
            DataSet dsRFDF3 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRFDF1);
                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRFDF2);
                dsRFDF1.Merge(dsRFDF2);
                dsRFDF2.Clear();
                command = new SqlCommand(_cmdstr2, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRFDF3);
                dsRFDF1.Merge(dsRFDF3);
                dsRFDF3.Clear();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsRFDF1;
        }

        public DataSet DFnoRFAging(string yrmo,string _ssn, string _dcn,string _pdDate)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);

            string _cmdstr = "SELECT anbt_yrmo AS [YRMO], anbt_subid_ssn AS [SSN],"
                                + " anbt_claimid AS [Claim #], anbt_name AS [NAME],"
                                + " CONVERT(VARCHAR(15),anbt_servfromdt,101) AS [Service From Dt], "
                                + " CONVERT(VARCHAR(15),anbt_servthrudt,101) AS [Service Thru Dt], "
                                + " CONVERT(VARCHAR(15),anbt_datePd,101) AS [Paid Date],anbt_claimsType AS [Claims Type], "
                                + " CONVERT(VARCHAR(20),anbt_ClaimsPdAmt,1) AS [Claims Paid Amt] "                                
                                + " FROM AnthBillTrans "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' and anbt_Discp = 1 and anbt_DiscpCleared = 0"
                                + " AND anbt_yrmo = '" + yrmo + "' AND anbt_claimid = '" + _dcn + "' "
                                + " AND anbt_subid_ssn = '" + _ssn + "' AND anbt_datePd = '" + _pdDate + "' ";

            DataSet dsDFnoRF1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsDFnoRF1);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsDFnoRF1;
        }

        public DataSet DFRFAging(string yrmo,string _ssn,string _dcn, string _lastUpdate)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);

            string _cmdstr = "SELECT   rfdf_yrmo AS [YRMO],rfdf_subid AS [MemberID] "
                            + " ,rfdf_dcn AS [DCN],CONVERT(VARCHAR(15),rfdf_lastupdt,101) AS [LastUpdate], "
                            + " CONVERT(VARCHAR(20),rfdf_amt,1) AS [DFRF Amount]"                           
                            + " FROM RFDF "
                            + " WHERE rfdf_source = 'DF' and rfdf_Discp = 1 and rfdf_DiscpCleared = 0"
                            + " AND rfdf_yrmo = '" + yrmo + "' AND rfdf_dcn = '" + _dcn + "' "
                            + " AND rfdf_subid = '" + _ssn + "' AND rfdf_lastupdt = '" + _lastUpdate + "'";

            DataSet dsRFDF1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsRFDF1);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsRFDF1;
        }

        public string latestReconYrmo()
        {
            string _yrmo = "-1";
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand("SELECT MAX(rf_yrmo) FROM rf_recon", connect);
                command.CommandTimeout = 0;
                if (command.ExecuteScalar() != DBNull.Value)
                {
                    _yrmo = command.ExecuteScalar().ToString();
                }
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return _yrmo;
        }

        public void updateForcedRecon(string _id, string _src, string _notes,string userName)
        {
            string _yrmo = latestReconYrmo();
            CAClaimsDAL yObj = new CAClaimsDAL();
            string _yrmo1 = yObj.getLatestYRMOAdj();
            int _type = 0;
            string _cmdstr = "INSERT INTO Forced_Adjustments (fadj_yrmo, fadj_source,fadj_claimid,fadj_notes,fadj_user) "
                                   + " VALUES(@yrmo,@src,@id,@notes,@user)";
            string _cmdstr1 = "";
            string _cmdstr2 = "";
            
            switch (_src)
            {
                case "DFRF":
                    _type = 1;
                    _cmdstr1 = "UPDATE RFDF SET rfdf_DiscpCleared = 1 WHERE rfdf_dcn = '" + _id + "' " 
                                + "AND rfdf_yrmo <= '" + _yrmo + "'";
                    break;
                case "DFnoRF":
                    _type = 1;
                    _cmdstr1 = "UPDATE AnthBillTrans SET anbt_DiscpCleared = 1 " 
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_claimid = '" + _id + "' "
                                 + "AND anbt_yrmo <= '" + _yrmo + "'";                    
                    break;

                case "DFRFmismatch":
                    _type = 1;
                    _cmdstr1 = "UPDATE RFDF SET rfdf_cleared = 1 WHERE rfdf_dcn = '" + _id + "' "
                                + "AND rfdf_yrmo <= '" + _yrmo + "'";
                   
                    _cmdstr2 = "UPDATE AnthBillTrans SET anbt_Cleared = 1 "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_claimid = '" + _id + "' "
                                 + "AND anbt_yrmo <= '" + _yrmo + "'";
                    break;

                case "MismatchAmounts":
                    int _chq = Convert.ToInt32(_id);
                    _type = 2;
                    _cmdstr1 = "UPDATE AnthbillTrans SET anbt_CFCleared = 1 "
                                + "WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_checkNum = " + _chq + " "
                                + "AND anbt_yrmo <= '" + _yrmo1 + "'";
                    _cmdstr2 = "UPDATE BOAStatement SET boaCFCleared = 1 "
                                + "WHERE boaChqNo = " + _chq + " "
                                + "AND boaYRMO <= '" + _yrmo1 + "'";
                    break;
                case "AnthemMismatch":
                    _type = 2;
                    int _chq1 = Convert.ToInt32(_id);
                    _cmdstr1 = "UPDATE AnthbillTrans SET anbt_CFCleared = 1 "
                                + "WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_checkNum = " + _chq1 + " "
                                + "AND anbt_yrmo <= '" + _yrmo1 + "'";
                    break;
                case "BOAMismatch":
                    _type = 2;
                    int _chq2 = Convert.ToInt32(_id);
                    _cmdstr1 = "UPDATE BOAStatement SET boaCFCleared = 1 "
                                + "WHERE boaChqNo = " + _chq2 + " "
                                + "AND boaYRMO <= '" + _yrmo1 + "'";
                    break;
                case "DuplicateChecks":
                    _type = 2;
                    int _chq3 = Convert.ToInt32(_id);
                    _cmdstr1 = "UPDATE AnthbillTrans SET anbt_CFCleared = 1 "
                                + "WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_checkNum = " + _chq3 + " "
                                + "AND anbt_yrmo <= '" + _yrmo1 + "' AND anbt_CF = 1";
                    _cmdstr2 = "UPDATE BOAStatement SET boaCFCleared = 1 "
                                + "WHERE boaChqNo = " + _chq3 + " "
                                + "AND boaYRMO <= '" + _yrmo1 + "' AND boaCF = 1";
                    break;
            }
            
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            SqlTransaction ts = null;
            try
            {
                ts = connect.BeginTransaction();
                command = new SqlCommand(_cmdstr, connect, ts);
                if (_type == 1)
                {
                    command.Parameters.AddWithValue("@yrmo", _yrmo);
                }
                else if (_type == 2)
                {
                    command.Parameters.AddWithValue("@yrmo", _yrmo1);
                }
                command.Parameters.AddWithValue("@src", _src);
                command.Parameters.AddWithValue("@id", _id);
                command.Parameters.AddWithValue("@notes", _notes);
                command.Parameters.AddWithValue("@user", userName);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr1, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                if (!_cmdstr2.Equals(""))
                {
                    command = new SqlCommand(_cmdstr2, connect, ts);
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();
                }
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public void DeleteNCAReconData(string yrmo)
        {
            try
            {
                ResetPrevNCACF(yrmo);                
                DeleteFromNCAReconTable(yrmo);                
            }
            finally
            {
                if (connect != null && connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        protected void ResetPrevNCACF(string yrmo)
        {
            string _prevyrmo = AnthRecon.prevYRMO(yrmo);

            string _cmdstr = "UPDATE AnthBillTrans SET anbt_DiscpCleared = 0,anbt_DiscpClearedYRMO = NULL "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo < '" + yrmo + "' AND anbt_DiscpClearedYRMO = '" + yrmo + "'  "
                                + " AND anbt_Discp = 1 AND anbt_DiscpCleared = 1 AND anbt_claimid IN "
                                + "(SELECT rfdf_dcn FROM RFDF WHERE rfdf_yrmo >= '" + yrmo + "') ";                                                

            string _cmdstr1 = "UPDATE RFDF SET rfdf_DiscpCleared = 0,rfdf_DiscpClearedYRMO = NULL "
                                + " WHERE rfdf_source = 'DF' AND rfdf_yrmo < '" + yrmo + "' AND rfdf_DiscpClearedYRMO = '" + yrmo + "' "
                                + " AND rfdf_Discp = 1 AND rfdf_DiscpCleared = 1 AND rfdf_dcn IN "
                                + "(SELECT rfdf_dcn FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_yrmo >= '" + yrmo + "')";                                

            string _cmdstr2 = "UPDATE RFDF SET rfdf_Discp = 0 "
                                + " WHERE rfdf_source = 'DF' AND rfdf_yrmo = '" + _prevyrmo + "' AND rfdf_Discp = 1";
           
            string _cmdstr3 = "UPDATE AnthBillTrans SET anbt_Cleared = 0 "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo < '" + yrmo + "' "
                                + " AND anbt_Cleared = 1 AND anbt_claimid IN "
                                + "(SELECT anbt_claimid  FROM AnthBillTrans WHERE anbt_yrmo >= '" + yrmo + "' AND anbt_Cleared = 1) ";

            string _cmdstr4 = "UPDATE RFDF SET rfdf_cleared = 0 "
                                + " WHERE rfdf_source = 'RF' AND rfdf_yrmo < '" + yrmo + "' "
                                + " AND rfdf_cleared = 1 AND rfdf_dcn IN "
                                + "(SELECT rfdf_dcn FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_yrmo >= '" + yrmo + "' AND rfdf_cleared = 1) ";


            string _cmdstr5 = "UPDATE AnthBillTrans SET anbt_Discp = 0,anbt_DiscpCleared = 0, anbt_Cleared = 0,anbt_DiscpClearedYRMO = NULL "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo >= '" + yrmo + "' ";

            string _cmdstr6 = "UPDATE RFDF SET rfdf_Discp = 0,rfdf_DiscpCleared = 0,rfdf_DiscpClearedYRMO = NULL "
                                + " WHERE rfdf_source = 'DF' AND rfdf_yrmo >='" + yrmo + "' ";

            string _cmdstr7 = "UPDATE RFDF SET rfdf_cleared = 0 "
                                + " WHERE rfdf_source = 'RF' AND rfdf_yrmo >='" + yrmo + "' ";


            

            SqlTransaction ts;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {                
                command = new SqlCommand(_cmdstr, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();                
                command = new SqlCommand(_cmdstr1, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr2, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr3, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr4, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr5, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr6, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr7, connect, ts);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (Exception ex)
            {                
                ts.Rollback();
                throw ex;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

       

        protected void DeleteFromNCAReconTable(string yrmo)
        {
            string _cmdstr0 = "DELETE FROM rf_recon WHERE rf_yrmo >= '" + yrmo + "'";
            string _cmdstr1 = "DELETE FROM DFRF_Summary WHERE sum_yrmo >= '" + yrmo + "'";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }            
            try
            {
                command = new SqlCommand(_cmdstr0, connect);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
            }          
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet generateAuditAdjReport(string _yrmo, string _src)
        {
            string _cmdstr = "", _cmdstr1 = "";

            switch (_src)
            {
                case "DFnoRF":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Amount],'ANTHEM DF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,AnthBillTrans WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = anbt_claimid "
                                + " AND anbt_yrmo <= '" + _yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT' ";
                    break;
                case "DFRF":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),rfdf_amt) AS [Amount],'DFnoRF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,RFDF WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = rfdf_dcn " 
                                + " AND rfdf_source = 'DF' AND rfdf_yrmo < '" + _yrmo + "'";
                    break;
                case "DFRFmismatch":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Amount],'ANTHEM DF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,AnthBillTrans WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = anbt_claimid "
                                + " AND anbt_yrmo <= '" + _yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT'";

                    _cmdstr1 = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),rfdf_amt) AS [Amount],'DF/RF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,RFDF WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = rfdf_dcn "
                                + " AND rfdf_source = 'RF' AND rfdf_yrmo <= '" + _yrmo + "'";
                    break;

                case "MismatchAmounts":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Amount],'ANTHEM DF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,AnthBillTrans WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = anbt_CheckNum "
                                + " AND anbt_yrmo <= '" + _yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT'";

                    _cmdstr1 = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),boaAmnt) AS [Amount],'BOA Statement',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,BOAStatement WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = boaChqNo "
                                + " AND boaYRMO <= '" + _yrmo + "'";
                    break;

                case "AnthemMismatch":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Amount],'ANTHEM DF',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,AnthBillTrans WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = anbt_CheckNum"
                                + " AND anbt_yrmo <= '" + _yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT'";
                    break;

                case "BOAMismatch":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),boaAmnt) AS [Amount],'BOA Statement',fadj_notes,fadj_user,fadj_date "
                               + " FROM Forced_Adjustments,BOAStatement WHERE fadj_yrmo = '" + _yrmo + "' "
                               + " AND fadj_source = '" + _src + "' AND fadj_claimid = boaChqNo "
                               + " AND boaYRMO <= '" + _yrmo + "'";
                    break;

                case "DuplicateChecks":
                    _cmdstr = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Amount],'ANTHEM DF',fadj_notes,fadj_user,fadj_date "
                               + " FROM Forced_Adjustments,AnthBillTrans WHERE fadj_yrmo = '" + _yrmo + "' "
                               + " AND fadj_source = '" + _src + "' AND fadj_claimid = anbt_CheckNum "
                               + " AND anbt_yrmo <= '" + _yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT'";

                    _cmdstr1 = "SELECT fadj_yrmo,fadj_source,fadj_claimid,CONVERT(Numeric(10,2),boaAmnt) AS [Amount],'BOA Statement',fadj_notes,fadj_user,fadj_date "
                                + " FROM Forced_Adjustments,BOAStatement WHERE fadj_yrmo = '" + _yrmo + "' "
                                + " AND fadj_source = '" + _src + "' AND fadj_claimid = boaChqNo "
                                + " AND boaYRMO <= '" + _yrmo + "'";
                    break;
            }
        
            DataSet dsAdj = new DataSet();
            DataSet dsAdj1 = new DataSet();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(_cmdstr, connect);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = command;
            da.Fill(dsAdj);
            if (!_cmdstr1.Equals(""))
            {
                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsAdj1);
                dsAdj.Merge(dsAdj1);
                dsAdj1.Clear();
            }
            connect.Close();
            return dsAdj;
        }

        public void retainForcedAdjNCA(string yrmo)
        {
            string _cmdstr = "UPDATE AnthBillTrans SET anbt_DiscpCleared = 1 "
                                + " WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + " AND anbt_yrmo <= '" + yrmo + "' "
                                + " AND anbt_claimid IN "
                                + " (SELECT fadj_claimid FROM Forced_Adjustments "
                                + "  WHERE fadj_yrmo = '" + yrmo + "' "
                                + "  AND fadj_source = 'DFnoRF')";

            string _cmdstr0 = "UPDATE RFDF SET rfdf_DiscpCleared = 1 "
                                + " WHERE rfdf_yrmo <= '" + yrmo + "' "
                                + " AND rfdf_source = 'DF' AND rfdf_dcn IN "
                                + " (SELECT fadj_claimid FROM Forced_Adjustments "
                                + "  WHERE fadj_yrmo = '" + yrmo + "' "
                                + "  AND fadj_source = 'DFRF')";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(_cmdstr, connect);
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
            command = new SqlCommand(_cmdstr0, connect);
            command.CommandTimeout = 0;
            command.ExecuteNonQuery();
            connect.Close();
        }

        protected void setPrevYRMOSummary(string _yrmo,string _src)
        {
            string _cmdstr = "";
            switch (_src)
            {
                case "DFnoRF":
                    _cmdstr = "SELECT anbt_yrmo AS [claimYrmo], COUNT(anbt_claimid) AS [claimCnt], SUM(anbt_ClaimsPdAmt) AS [claimAmt] "
                           + " FROM AnthBillTrans "
                           + " WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                           + " AND anbt_yrmo < '" + _yrmo + "' "
                           + " AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                           + " GROUP BY anbt_yrmo ";
                    break;
                case "DFRF":
                    _cmdstr = "SELECT rfdf_yrmo AS [claimYrmo],COUNT(rfdf_dcn) as [claimCnt],SUM(rfdf_amt) AS [claimAmt] "
                            + " FROM RFDF "
                            + " WHERE rfdf_source = 'DF' "
                            + " AND rfdf_yrmo < '" + _yrmo + "' "
                            + " AND rfdf_Discp = 1 AND rfdf_DiscpCleared = 0 "
                            + " GROUP BY rfdf_yrmo ";
                    break;
            }

            string _cmdstr1 = "INSERT INTO DFRF_Summary "
                            + " (sum_yrmo, sum_source, sum_Prevyrmo,sum_Count,sum_Amount) "
                            + " VALUES (@yrmo,@src,@pyrmo,@cnt,@amt) ";

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    command = new SqlCommand(_cmdstr1, connect);
                    command.Parameters.AddWithValue("@yrmo", _yrmo);
                    command.Parameters.AddWithValue("@src", _src);
                    command.Parameters.AddWithValue("@pyrmo", dr["claimYrmo"]);
                    command.Parameters.AddWithValue("@cnt", dr["claimCnt"]);
                    command.Parameters.AddWithValue("@amt", dr["claimAmt"]);
                    command.CommandTimeout = 0;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet getSummary(string _yrmo, string _src)
        {
            string _cmdstr = "";
            string _cmdstr1 = "SELECT sum_Prevyrmo AS [claimYrmo],sum_Count AS [claimCnt], sum_Amount AS [claimAmt] "
                            + " FROM DFRF_Summary WHERE sum_yrmo = @yrmo AND sum_source = @src ORDER BY sum_Prevyrmo";
            switch (_src)
            {
                case "DFnoRF":
                    _cmdstr = "SELECT anbt_yrmo AS [claimYrmo], COUNT(anbt_claimid) AS [claimCnt], SUM(anbt_ClaimsPdAmt) AS [claimAmt] "
                            + " FROM AnthBillTrans "
                            + " WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                            + " AND anbt_yrmo <= '" + _yrmo + "' "
                            + " AND anbt_Discp = 1 AND anbt_DiscpCleared = 0 "
                            + " GROUP BY anbt_yrmo "
                            + " ORDER BY anbt_yrmo ";
                    break;
                case "DFRF":
                    _cmdstr = "SELECT rfdf_yrmo AS [claimYrmo],COUNT(rfdf_dcn) as [claimCnt],SUM(rfdf_amt) AS [claimAmt] "
                            + " FROM RFDF "
                            + " WHERE rfdf_source = 'DF' "
                            + " AND rfdf_yrmo <= '" + _yrmo + "' "
                            + " AND rfdf_Discp = 1 AND rfdf_DiscpCleared = 0 "
                            + " GROUP BY rfdf_yrmo "
                            + " ORDER BY rfdf_yrmo ";
                    break;
            }

            int _fPCount = 0;
            int _fCCount = 0;
            string _fPAmt;
            string _fCAmt;

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsCurrSum = new DataSet();
            dsCurrSum.Clear();
            DataSet dsPrevSum = new DataSet();
            dsPrevSum.Clear();
            DataSet dsFSum = new DataSet();
            dsFSum.Clear();
            DataTable tempTable;
            tempTable = dsFSum.Tables.Add("tempTable1");
            System.Type typeInt32 = System.Type.GetType("System.Int32");
            System.Type typeDecimal = System.Type.GetType("System.Decimal");

            DataColumn col;
            col = new DataColumn("YRMO"); tempTable.Columns.Add(col);
            col = new DataColumn("Previous Count", typeInt32); tempTable.Columns.Add(col);
            col = new DataColumn("Current Count", typeInt32); tempTable.Columns.Add(col);
            col = new DataColumn("Prior Amount",typeDecimal); tempTable.Columns.Add(col);
            col = new DataColumn("Current Amount",typeDecimal); tempTable.Columns.Add(col);
            DataRow newRow;
            bool found = false;

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsCurrSum);

                command = new SqlCommand(_cmdstr1, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("src", _src);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsPrevSum);

                foreach (DataRow dr1 in dsPrevSum.Tables[0].Rows)
                {
                    foreach (DataRow dr in dsCurrSum.Tables[0].Rows)
                    {
                        if(dr1["claimYrmo"].ToString().Equals(dr["claimYrmo"].ToString()))
                        {
                            newRow = tempTable.NewRow();
                            newRow["YRMO"] = dr["claimYrmo"];
                            newRow["Previous Count"] = dr1["claimCnt"];
                            newRow["Current Count"] = dr["claimCnt"];
                            newRow["Prior Amount"] = dr1["claimAmt"];
                            newRow["Current Amount"] = dr["claimAmt"];
                            tempTable.Rows.Add(newRow);
                            found = true;
                        }                        
                    }
                    if (!found)
                    {
                        newRow = tempTable.NewRow();
                        newRow["YRMO"] = dr1["claimYrmo"];
                        newRow["Previous Count"] = dr1["claimCnt"];
                        newRow["Current Count"] = 0;
                        newRow["Prior Amount"] = dr1["claimAmt"];
                        newRow["Current Amount"] = 0;
                        tempTable.Rows.Add(newRow);
                    }                   
                    found = false;
                }
                if (_src.Equals("DFnoRF"))
                {
                    foreach (DataRow dr in dsCurrSum.Tables[0].Rows)
                    {
                        if (dr["claimYrmo"].ToString().Equals(_yrmo))
                        {
                            newRow = tempTable.NewRow();
                            newRow["YRMO"] = dr["claimYrmo"];
                            newRow["Previous Count"] = 0;
                            newRow["Current Count"] = dr["claimCnt"];
                            newRow["Prior Amount"] = 0;
                            newRow["Current Amount"] = dr["claimAmt"];
                            tempTable.Rows.Add(newRow);
                            break;
                        }
                    }
                }
                else if (_src.Equals("DFRF"))
                {
                    string _prevyrmo = AnthRecon.prevYRMO(_yrmo);
                    foreach (DataRow dr in dsCurrSum.Tables[0].Rows)
                    {
                        if (dr["claimYrmo"].ToString().Equals(_prevyrmo))
                        {
                            newRow = tempTable.NewRow();
                            newRow["YRMO"] = dr["claimYrmo"];
                            newRow["Previous Count"] = 0;
                            newRow["Current Count"] = dr["claimCnt"];
                            newRow["Prior Amount"] = 0;
                            newRow["Current Amount"] = dr["claimAmt"];
                            tempTable.Rows.Add(newRow);
                            break;
                        }
                    }
                }

                if (tempTable.Rows.Count > 0)
                {
                    _fCAmt = Convert.ToDecimal(tempTable.Compute("SUM([Current Amount])", String.Empty)).ToString("F2");
                    _fPAmt = Convert.ToDecimal(tempTable.Compute("SUM([Prior Amount])", String.Empty)).ToString("F2");
                    _fPCount = Convert.ToInt32(tempTable.Compute("SUM([Previous Count])", String.Empty));
                    _fCCount = Convert.ToInt32(tempTable.Compute("SUM([Current Count])", String.Empty));

                    DataRow row = tempTable.NewRow();
                    row["YRMO"] = "TOTAL: ";
                    row["Previous Count"] = _fPCount;
                    row["Current Count"] = _fCCount;
                    row["Prior Amount"] = _fPAmt;
                    row["Current Amount"] = _fCAmt;
                    tempTable.Rows.Add(row);                    
                }
                return dsFSum;
            }             
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet getMismatchCF(string _yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(_yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);

            string _cmdstr = "SELECT [YRMO], anbt_claimid AS [Claim ID], "
                                + " dfCtr AS [Anthem DF Counter],  "
                                + " dfrfCtr AS [DFRF Counter], "
                                + " claimsAmt AS [Anthem DF Amount],  " 
                                + " rfAmt AS [DFRF Amount],diff AS [Variance], "
                                + " CASE "
                                + " WHEN YRMO = '" + _yrmo +"' "
                                + " THEN CONVERT(VARCHAR(20),diff,1) "
                                + " ELSE '0' "
                                + " END AS [Current YRMO], "
                                + " CASE "
                                + " WHEN YRMO = '" + prev_yrmo + "' "
                                + " THEN CONVERT(VARCHAR(20),diff,1) "
                                + " ELSE '0' "
                                + " END AS [Previous YRMO], "
                                + " CASE "
                                + " WHEN YRMO <= '" + prior_yrmo + "' "
                                + " THEN CONVERT(VARCHAR(20),diff,1) "
                                + " ELSE '0' "
                                + " END AS [Prior YRMO] "
                                + " FROM "
                                + " (SELECT " 
                                + " CASE " 
                                + " WHEN MAX(a.anbt_yrmo)< MAX(rfdfyrmo) THEN MAX(rfdfyrmo) "
                                + " ELSE MAX(a.anbt_yrmo) END AS [YRMO], "  
                                + " a.anbt_claimid,dcn,Convert(Numeric(10,2),SUM(a.anbt_claimsPdAmt)) AS claimsAmt , " 
                                + " Convert(Numeric(10,2),rfAmt) AS rfAmt ,Convert(Numeric(10,2),(SUM(a.anbt_claimsPdAmt) - rfamt)) AS [diff], "
                                + " COUNT(*) as dfCtr, b.dfrfCtr "
                                + " FROM AnthBillTrans a " 
                                + " INNER JOIN  "
                                + " ( "
                                + " SELECT rfdf_dcn dcn,sum(rfdf_amt) rfAmt, MAX(rfdf_yrmo) as rfdfyrmo, COUNT(*) as dfrfCtr  "
                                + " FROM RFDF "
                                + " WHERE rfdf_yrmo <= '" + _yrmo + "' " 
                                + " AND  rfdf_source = 'RF' AND rfdf_cleared = 0 "
                                + " GROUP BY rfdf_dcn "
                                + " ) b " 
                                + " ON (a.anbt_claimid = dcn) "
                                + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_Cleared = 0 AND anbt_yrmo <= '" + _yrmo + "' "
                                + " GROUP BY a.anbt_claimid, rfAmt,dcn, b.dfrfCtr  "                           
                                + " HAVING (SUM(a.anbt_claimsPdAmt) - rfamt) <> 0 "
                                + " ) AS mismatchCFAging "
                                + " ORDER BY YRMO ";

            DataSet dsMismatchCF = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsMismatchCF);
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return dsMismatchCF;
        }

        public DataSet getMismatchCF(string _yrmo, string _dcn)
        {
            DataSet dsOne = new DataSet(); dsOne.Clear();
            DataSet dsTwo = new DataSet(); dsTwo.Clear();
            DataSet dsFinal1 = new DataSet(); dsFinal1.Clear();

            string _cmdstr = "SELECT anbt_yrmo AS [YRMO], 'ANTHEM DF' AS [Source], anbt_claimid AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),anbt_datePd,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),0) AS [DFRF Amount] "
                                + " FROM AnthBillTrans "
                                + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo <= '" + _yrmo + "' AND anbt_claimid = '" + _dcn + "' ";

            string _cmdstr1 = "SELECT rfdf_yrmo AS [YRMO], 'DFRF' AS [Source], rfdf_dcn AS [Claim ID], "
                                + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS [Date Paid], "
                                + " CONVERT(Numeric(10,2),0) AS [Anthem DF Amount], "
                                + " CONVERT(Numeric(10,2),rfdf_amt) AS [DFRF Amount] "
                                + " FROM RFDF "
                                + " WHERE rfdf_source ='RF' AND rfdf_yrmo <= '" + _yrmo + "' AND rfdf_dcn  = '" + _dcn + "'";           

           
            SqlDataAdapter da = new SqlDataAdapter();
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsOne);
                dsFinal1.Merge(dsOne); dsOne.Clear();

                command = new SqlCommand(_cmdstr1, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsTwo);
                dsFinal1.Merge(dsTwo); dsTwo.Clear();

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }

            if (dsFinal1.Tables[0].Rows.Count > 0)
            {
                decimal _anthemAmount = Convert.ToDecimal(dsFinal1.Tables[0].Compute("SUM([Anthem DF Amount])", String.Empty));
                decimal _rfdfAmount = Convert.ToDecimal(dsFinal1.Tables[0].Compute("SUM([DFRF Amount])", String.Empty));

                DataRow row = dsFinal1.Tables[0].NewRow();
                row["Date Paid"] = "Total: ";
                row["Anthem DF Amount"] = _anthemAmount;
                row["DFRF Amount"] = _rfdfAmount;               
                dsFinal1.Tables[0].Rows.Add(row);
            }
            return dsFinal1;
        }

        public void deleteForced(string _yrmo,string _src)
        {
            string _cmdstr = null;
            switch (_src)
            {
                case "Anthem":
                    _cmdstr = "DELETE FROM Forced_Adjustments "
                                + " WHERE fadj_yrmo >= '" + _yrmo + "' ";
                    break;
                case "DFRF":
                    _cmdstr = "DELETE FROM Forced_Adjustments "
                                + " WHERE fadj_yrmo >= '" + _yrmo + "' AND fadj_source IN('DFRF','DFnoRF','DFRFmismatch')";
                    break;
                case "BOA":
                    _cmdstr = "DELETE FROM Forced_Adjustments "
                                + " WHERE fadj_yrmo >= '" + _yrmo + "' "
                                + " AND fadj_source IN('MismatchAmounts','AnthemMismatch','BOAMismatch','DuplicateChecks')";
                    break;
            }
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetDCNHistory(string dcn)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();

            string cmdstr1 = "SELECT 'DFRF' AS [Source], "
                                + "rfdf_yrmo AS [YRMO], "
                                + "CONVERT(VARCHAR(20),rfdf_amt,1) AS [Amount], "
                                + "CASE "
                                + "WHEN ((rfdf_Discp = 1 AND rfdf_DiscpCleared = 1) OR rfdf_Discp = 0) THEN 'true' "
                                + "ELSE 'false' "
                                + "END AS [Reconciled] "
                                + "FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_dcn = '" + dcn + "'";

            string cmdstr2 = "SELECT 'Anthem DF' AS [Source], "
                                + "anbt_yrmo AS [YRMO], "
                                + "CONVERT(VARCHAR(20),anbt_ClaimsPdAmt,1) AS [Amount], "
                                + "CASE "
                                + "WHEN ((anbt_Discp = 1 AND anbt_DiscpCleared = 1) OR anbt_Discp = 0) THEN 'true' "
                                + "ELSE 'false' "
                                + "END AS [Reconciled] "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_sourcecd = 'NCA_CLMRPT' "
                                + "AND anbt_claimid = '" + dcn + "'";

            string cmdstr3 = "SELECT 'DF no RF' AS [Source], "
                                + "rfdf_yrmo AS [YRMO], "
                                + "CONVERT(VARCHAR(20),rfdf_amt,1) AS [Amount], "
                                + "CASE "
                                + "WHEN ((rfdf_Discp = 1 AND rfdf_DiscpCleared = 1) OR rfdf_Discp = 0) THEN 'true' "
                                + "ELSE 'false' "
                                + "END AS [Reconciled] "
                                + "FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_dcn = '" + dcn + "'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(dsFinal);
                command = new SqlCommand(cmdstr2, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(ds);
                dsFinal.Merge(ds);
                ds.Clear();
                command = new SqlCommand(cmdstr3, connect);
                command.CommandTimeout = 0;
                da.SelectCommand = command;
                da.Fill(ds);
                dsFinal.Merge(ds);
                ds.Clear();

                return dsFinal;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }
    }
}

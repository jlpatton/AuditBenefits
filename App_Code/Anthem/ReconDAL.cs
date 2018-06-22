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

/// <summary>
/// Summary description for ReconDAL
/// </summary>
/// 
namespace EBA.Desktop.Anthem
{
    public class ReconDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static string connStr2 = ConfigurationManager.ConnectionStrings["FRDDB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlConnection connectFRD = new SqlConnection(connStr2);
        static SqlCommand command = null;

        public ReconDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //New Logic for RFDF Claims 04/22/2008
        public static void calcTotalAmt(string _yrmo, List<int> _rid)
        {
            string _cmdStr = "SELECT SUM(rfdf_amt) FROM RFDF WHERE rfdf_yrmo = '" + _yrmo + "' AND rfdf_cleared = 0";
            string _cmdStr1 = "SELECT SUM(anbt_ClaimsPdAmt) FROM AnthBillTrans WHERE anbt_yrmo = '" + _yrmo + "' AND anbt_sourcecd =  'NCA_CLMRPT' ";
            string prevYRMO = AnthRecon.prevYRMO(_yrmo);
            string _cmdStr0 = "SELECT rf_diff FROM rf_recon WHERE rf_yrmo = '" + prevYRMO + "'";

            decimal _prevdiff = 0;
            decimal _clmamt;
            decimal _rfamt;
            decimal _diff;
            decimal _per;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr, connect);
                _rfamt = Decimal.Parse(command.ExecuteScalar().ToString(), System.Globalization.NumberStyles.Currency);
                command.Dispose();
                command = new SqlCommand(_cmdStr1, connect);
                _clmamt = Decimal.Parse(command.ExecuteScalar().ToString(), System.Globalization.NumberStyles.Currency);
                command.Dispose();
                command = new SqlCommand(_cmdStr0, connect);
                object _res = command.ExecuteScalar();
                if (_res != null)
                {
                    _prevdiff = Decimal.Parse(_res.ToString(), System.Globalization.NumberStyles.Currency);
                }
                if (_prevdiff < 0)
                {
                    _rfamt = _rfamt + _prevdiff;
                }
                else if (_prevdiff > 0)
                {
                    _clmamt = _clmamt + _prevdiff;
                }
                command.Dispose();
                _diff = (_rfamt- _clmamt);
                if (_diff < 0)
                {
                    _per = ((_clmamt - _rfamt) / _clmamt) * 100;
                }
                else if (_diff > 0)
                {
                    _per = ((_rfamt - _clmamt) / _rfamt) * 100;
                }
                else
                {
                    _per = 0;
                }
                insertReconAmt(_yrmo, _clmamt, _rfamt, _diff,_per);
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        private static void insertReconAmt(string _yrmo, decimal _clmamt, decimal _rfamt, decimal _diff, decimal _per)
        {

            string _cmdStr = "INSERT INTO rf_recon VALUES (" + _yrmo + "," + _clmamt + "," + _rfamt + "," + _diff + "," + _per +")";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(_cmdStr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        //Anthem Pharmacy Recon
        public static decimal getFRDData(string _yrmo)
        {
            string _cmdStr = "SELECT SUM(car_amt) FROM banksum WHERE provid = 'ANTHEMRX' AND yrmo = '" + _yrmo + "'";
            decimal _frdamt;
            try
            {
                if (connectFRD == null || connectFRD.State == ConnectionState.Closed)
                {
                    connectFRD.Open();
                }
                command = new SqlCommand(_cmdStr, connectFRD);
                object temp = command.ExecuteScalar();
                if (temp != DBNull.Value)
                {
                    _frdamt = Decimal.Parse(temp.ToString(), System.Globalization.NumberStyles.Currency);
                }
                else
                {
                    _frdamt = -1;
                }
            }
            finally
            {
                connectFRD.Close();
                command.Dispose();
            }
            return _frdamt;
        }
        
        public static decimal getAnthemRX(string _yrmo)
        {
            string _cmdStr = "SELECT rx_totalpay FROM anth_Rx WHERE rx_yrmo = '" + _yrmo + "'";
            decimal _anthrxamt = 0;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr, connect);
                _anthrxamt = Decimal.Parse(command.ExecuteScalar().ToString(), System.Globalization.NumberStyles.Currency);
                command.Dispose();
                connect.Close();
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return _anthrxamt;
        }

        public static void insertRXRecon(string _yrmo, decimal _frdamt, decimal _anbtamt)
        {
            decimal _diff;
            string prevYRMO = AnthRecon.prevYRMO(_yrmo);
            string _cmdStr0 = "SELECT rx_rcn_diff FROM rx_recon WHERE rx_rcn_yrmo = '" + prevYRMO + "'";
            string _cmdStr = null;
            decimal _prevdiff = 0;
            decimal _per;

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr0, connect);
                object _res = command.ExecuteScalar();
                if (_res != null)
                {
                    _prevdiff = Decimal.Parse(_res.ToString(), System.Globalization.NumberStyles.Currency);
                }
                _anbtamt = _anbtamt + _prevdiff;
                _diff = (_anbtamt - _frdamt);
                command.Dispose();
                if (_diff > 0)
                {
                    _per = ((_anbtamt - _frdamt )/ _anbtamt) * 100;
                }
                else if (_diff < 0)
                {
                    _per = ((_frdamt - _anbtamt )/ _frdamt) * 100;
                }
                else
                {
                    _per = 0;
                }
                _cmdStr = "INSERT INTO rx_recon VALUES (" + _yrmo + "," + _anbtamt + "," + _frdamt + "," + _diff + "," + _per +")";
                command = new SqlCommand(_cmdStr, connect);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        //RFDF Claims Reconciliation
        public static List<int> matchCF(string _yrmo)
        {
            string _cmdStr = " SELECT a.rfdf_id,b.rfdf_id FROM RFDF a INNER JOIN(SELECT rfdf_id,rfdf_dcn FROM RFDF WHERE rfdf_cleared = 0 AND rfdf_yrmo = @yrmo AND rfdf_source ='RF' )b ON	(a.rfdf_dcn = b.rfdf_dcn)"
                                + " WHERE rfdf_source = 'DF' AND rfdf_yrmo < @yrmo";
            SqlDataReader dr = null;
            List<int> rfdfID = new List<int>();
            command = new SqlCommand(_cmdStr, connect);
            command.Parameters.Add("@yrmo", SqlDbType.VarChar);
            command.Parameters["@yrmo"].Value = _yrmo;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    int rid1 = Int32.Parse(dr[0].ToString());
                    int rid2 = Int32.Parse(dr[1].ToString());
                    rfdfID.Add(rid1);
                    rfdfID.Add(rid2);
                }
                dr.Close();
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return rfdfID;
        }

        public static void updatematchCF(List<int> _rid)
        {
            try
            {
                foreach (int id in _rid)
                {
                    string _cmdStr = "UPDATE RFDF set rfdf_cleared = 1 WHERE rfdf_id = " + id;
                    command = new SqlCommand(_cmdStr, connect);
                    if (connect == null || connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    command.ExecuteNonQuery();
                }
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet amountMatch(string _yrmo)
        {
            string _cmdStr = "SELECT anbt_claimid,SUM(anbt_ClaimsPdAmt) claimamt,b.rfdfamt "
                               + " FROM	AnthBillTrans a INNER JOIN (SELECT rfdf_dcn, SUM(rfdf_amt) rfdfamt FROM RFDF WHERE rfdf_yrmo = @yrmo AND rfdf_cleared = 0 GROUP BY rfdf_dcn) b"
                                + " ON (anbt_claimid = b.rfdf_dcn)"
                                + " WHERE	anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo = @yrmo GROUP BY anbt_claimid, b.rfdfamt";

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr, connect);
                command.Parameters.Add("@yrmo", SqlDbType.VarChar);
                command.Parameters["@yrmo"].Value = _yrmo;
                da.SelectCommand = command;
                da.Fill(ds, "reconTemp");
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                da.Dispose();
                command.Dispose();
            }
            return ds;
        }

        public static void insertRecon(string _yrmo, string _claimid, decimal _clmamt, decimal _rfdfamt, decimal _var)
        {
            string _cmdStr = "INSERT INTO rfdf_recon VALUES (@yrmo, @claimid, @clmamt,@rfdfamt,@variance)";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr, connect);
                command.Parameters.Add("@yrmo", SqlDbType.VarChar);
                command.Parameters["@yrmo"].Value = _yrmo;
                command.Parameters.Add("@claimid", SqlDbType.VarChar);
                command.Parameters["@claimid"].Value = _claimid;
                command.Parameters.Add("@clmamt", SqlDbType.Money);
                command.Parameters["@clmamt"].Value = _clmamt;
                command.Parameters.Add("@rfdfamt", SqlDbType.Money);
                command.Parameters["@rfdfamt"].Value = _rfdfamt;
                command.Parameters.Add("@variance", SqlDbType.Money);
                command.Parameters["@variance"].Value = _var;
                command.ExecuteNonQuery();
                command.Dispose();
                updateAmount(_claimid, _yrmo, _var);
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        private static void updateAmount(string _claimid, string _yrmo, decimal _var)
        {
            string _cmdStr1 = "UPDATE AnthBillTrans SET anbt_cleared = 1 "
                                    + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_claimid = '" + _claimid + "' AND anbt_yrmo = '" + _yrmo + "'";
            string _cmdStr2 = "UPDATE	RFDF SET rfdf_cleared = 1 "
                                + " WHERE rfdf_dcn = '" + _claimid + "' AND rfdf_yrmo = '" + _yrmo + "'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(_cmdStr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();

                if (!(_var == 0))
                {
                    string _cmdStr3 = "UPDATE AnthBillTrans SET anbt_CF = 1 "
                                    + " WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_claimid = '" + _claimid + "' AND anbt_yrmo = '" + _yrmo + "'";
                    string _cmdStr4 = "UPDATE	RFDF SET rfdf_CF = 1  "
                                        + " WHERE rfdf_dcn = '" + _claimid + "' AND rfdf_yrmo = '" + _yrmo + "'";

                    command = new SqlCommand(_cmdStr3, connect);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    command = new SqlCommand(_cmdStr4, connect);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void updateTablesCF(string _yrmo)
        {
            string _cmdStr1 = "UPDATE AnthBillTrans SET anbt_CF = 1 WHERE anbt_cleared = 0 AND anbt_sourcecd = 'NCA_CLMRPT' AND anbt_yrmo = '" + _yrmo + "'";
            string _cmdStr2 = "UPDATE RFDF SET rfdf_CF = 1 WHERE rfdf_cleared = 0 AND rfdf_yrmo = '" + _yrmo + "'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(_cmdStr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet updateCFFinal(string _yrmo)
        {
            string _cmdStr = "SELECT  anbt_claimid,SUM(anbt_ClaimsPdAmt) claimamt,b.rfdfamt FROM AnthBillTrans a"
                             + " INNER JOIN (SELECT rfdf_dcn,SUM(rfdf_amt) rfdfamt FROM RFDF WHERE  rfdf_CF = 1 GROUP BY rfdf_dcn) b"
                             + " ON	(anbt_claimid = b.rfdf_dcn)"
                             + " WHERE anbt_sourcecd = 'NCA_CLMRPT'AND anbt_CF = 1 GROUP BY anbt_claimid, b.rfdfamt";
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                command = new SqlCommand(_cmdStr, connect);
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                da.SelectCommand = command;
                da.Fill(ds, "CFTemp");
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return ds;
        }

        public static void updateReconCF(string _yrmo, string _claimid, decimal _clmamt, decimal _rfdfamt, decimal _var)
        {
            string _cmdStr1 = "UPDATE AnthBillTrans SET anbt_CF = 0 WHERE anbt_sourcecd = 'NCA_CLMRPT' AND anbt_claimid = '" + _claimid + "'";
            string _cmdStr2 = "UPDATE RFDF SET rfdf_CF = 0 WHERE rfdf_dcn ='" + _claimid + "'";
            string _cmdStr3 = "UPDATE	rfdf_recon SET	rf_rcn_clmamt = " + _clmamt + ",rf_rcn_rfamt = " + _rfdfamt + ","
                                + " rf_rcn_variance = " + _var + " WHERE	rf_rcn_dcn = '" + _claimid + "' AND rf_rcn_yrmo = '" + _yrmo + "'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdStr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(_cmdStr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(_cmdStr3, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            //catch
            //{
            //    throw (new Exception(""));
            //}
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }
        
        //get Percentage variance threshold from database
        public static decimal getPercentageVar(string src,string yrmo)
        {
            decimal _per;
            //string _yrmo = Convert.ToDateTime(yrmo.Insert(4, "/")).ToShortDateString();
            //string cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + src + "' AND '" + _yrmo + "' >= thres_date";

           
            string cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + src + "' AND thres_yrmo <= '" + yrmo + "'";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            object _per1 = command.ExecuteScalar();            
            if (_per1 == null)
            {
                cmdStr = "SELECT thres_value FROM threshold_History WHERE thres_name = '" + src + "' "
                            + "AND '" + yrmo + "' >= (SELECT max(thres_yrmo) FROM threshold_History WHERE thres_name = '" + src + "')";
                command = new SqlCommand(cmdStr, connect);
                _per1 = command.ExecuteScalar();
            }
            _per = decimal.Parse(_per1.ToString());
            connect.Close();
            return _per;
        }
        // International Recon Data
        public static DataSet GetIntlReconData(string yrmo)
        {
            decimal _per = getPercentageVar("International",yrmo);            
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string cmdStr = "SELECT rcn_yrmo, anthcd_plancd, anthcd_covgcd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt, rcn_var, "
                + "rcn_per,"
                + "CONVERT(Numeric(10,2)," + _per + ") AS threshold," 
                + "(CASE "
                + "WHEN rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                + "WHEN rcn_per < " + _per + " AND rcn_per <> 0 THEN '-' "
                + "WHEN rcn_per = 0 THEN '-' "
                + "END ) AS var_threshold "
                + " FROM billing_recon, AnthCodes WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND rcn_source = 'INTL' ORDER BY anthcd_plancd";
            //string cmdstr = "SELECT [plhr_carplancd], [plhr_plancd], [plhr_carcovgcd], [plhr_tiercd], [ircn_anth_count], [ircn_eba_count], [ircn_var] FROM [intl_recon], [planhier] WHERE [ircn_yrmo] = '" + yrmo + "' AND [plhr_id] = [ircn_plhr_id]";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            CalculateTotals cObj = new CalculateTotals();
            DataSet ds1 = cObj.calTotal(ds,yrmo,"INTL");
            return ds1;
        }
        
        //Domestic Recon Data
        public static DataSet GetDomReconData(string yrmo, string src)
        {
            decimal _per = getPercentageVar("Domestic",yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string cmdStr = null;
            switch (src)
            {
                case "ACT":
                    cmdStr = "SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt, rcn_var, "
                                + "rcn_per,"
                                + "CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                                + "WHEN rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                                + "WHEN rcn_per < " + _per + " AND rcn_per <> 0 THEN '-' "
                                + "WHEN rcn_per = 0 THEN '-' "
                                + "END ) AS var_threshold "
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)" 
                                + "AND plhr_plandesc LIKE 'ACTIVE%' AND plhr_plandesc NOT LIKE '%COB%'"
                                + " ORDER BY anthcd_plancd";
                    break;
                case "COB":
                    cmdStr = "SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt,rcn_var, "
                                + "rcn_per,"
                                + "CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                                + "WHEN rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                                + "WHEN rcn_per < " + _per + " AND rcn_per <> 0 THEN '-' "
                                + "WHEN rcn_per = 0 THEN '-' "
                                + "END ) AS var_threshold "
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)"
                                + "AND plhr_plandesc LIKE '%COB%'"
                                + " ORDER BY anthcd_plancd";
                    break;
                case "RET":
                    cmdStr = "SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt, rcn_var, "
                                + "rcn_per,"
                                + "CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                                + "WHEN rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                                + "WHEN rcn_per < " + _per + " AND rcn_per <> 0 THEN '-' "
                                + "WHEN rcn_per = 0 THEN '-' "
                                + "END ) AS var_threshold "
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)"
                                + "AND plhr_plandesc LIKE '%RET%' AND plhr_plandesc NOT LIKE '%COB%'"
                                + " ORDER BY anthcd_plancd";
                    break;
            }

                
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            CalculateTotals cObj = new CalculateTotals();
            DataSet ds1 = cObj.calTotal(ds, yrmo, src);
            return ds1;
        }

        public static DataSet GetDomSummary(string yrmo)
        {            
            DataSet dsDom = new DataSet();
            dsDom.Clear();
            DataTable sumTable;
            sumTable = dsDom.Tables.Add("tempTable");
            SqlDataAdapter da = new SqlDataAdapter();

            string _cmdStr = " SELECT SUM(rcn_prism_count) AS [EBA Count], "
                                + "SUM(rcn_prism_amt) [EBA Amount], "
                                + "SUM(rcn_anth_count) [Anthem Count], "
                                + "SUM(rcn_anth_amt) [Anthem Amount], "
                                + "(SUM(rcn_prism_count) - SUM(rcn_anth_count)) AS [Variance] "
                                + "FROM "   
                                + "(SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count, " 
                                + "CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt "                                
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)" 
                                + "AND plhr_plandesc LIKE 'ACTIVE%' AND plhr_plandesc NOT LIKE '%COB%')"
                                + " AS actTable";

            string _cmdStr1 = " SELECT SUM(rcn_prism_count) AS [EBA Count], "
                                + "SUM(rcn_prism_amt) [EBA Amount], "
                                + "SUM(rcn_anth_count) [Anthem Count], "
                                + "SUM(rcn_anth_amt) [Anthem Amount], "
                                + "(SUM(rcn_prism_count) - SUM(rcn_anth_count)) AS [Variance] "
                                + "FROM "
                                + "(SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt "                                
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)"
                                + "AND plhr_plandesc LIKE '%COB%')"
                                 + " AS cobTable";

            string _cmdStr2 = " SELECT SUM(rcn_prism_count) AS [EBA Count], "
                                + "SUM(rcn_prism_amt) [EBA Amount], "
                                + "SUM(rcn_anth_count) [Anthem Count], "
                                + "SUM(rcn_anth_amt) [Anthem Amount], "
                                + "(SUM(rcn_prism_count) - SUM(rcn_anth_count)) AS [Variance] "
                                + "FROM "
                                + "(SELECT DISTINCT( anthcd_covgcd),rcn_yrmo, anthcd_plancd, rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt, rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt "                                
                                + "FROM billing_recon, AnthCodes,AnthPlanhier WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_anthcd_id = anthcd_id) AND (plhr_anthcd_id = anthcd_id)"
                                + "AND plhr_plandesc LIKE '%RET%' AND plhr_plandesc NOT LIKE '%COB%')"
                                 + " AS retTable";

            System.Type typeInt32 = System.Type.GetType("System.Int32");
            System.Type typeDecimal = System.Type.GetType("System.Decimal");

            DataColumn col;
            col = new DataColumn("Reconciliation Type");    sumTable.Columns.Add(col);
            col = new DataColumn("EBA Count", typeInt32); sumTable.Columns.Add(col);
            col = new DataColumn("EBA Amount", typeDecimal); sumTable.Columns.Add(col);
            col = new DataColumn("Anthem Count", typeInt32); sumTable.Columns.Add(col);
            col = new DataColumn("Anthem Amount", typeDecimal); sumTable.Columns.Add(col);
            col = new DataColumn("EBA Count Variance", typeInt32); sumTable.Columns.Add(col);

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            SqlDataReader reader = null;
            command = new SqlCommand(_cmdStr, connect);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                DataRow row = sumTable.NewRow();
                row["Reconciliation Type"] = "Domestic - Active";
                row["EBA Count"] = reader[0];
                row["EBA Amount"] = reader[1];
                row["Anthem Count"] = reader[2];
                row["Anthem Amount"] = reader[3];
                row["EBA Count Variance"] = reader[4];
                sumTable.Rows.Add(row);
            }
            reader.Close();            
            command = new SqlCommand(_cmdStr1, connect);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                DataRow row1 = sumTable.NewRow();
                row1["Reconciliation Type"] = "Domestic - Cobra";
                row1["EBA Count"] = reader[0];
                row1["EBA Amount"] = reader[1];
                row1["Anthem Count"] = reader[2];
                row1["Anthem Amount"] = reader[3];
                row1["EBA Count Variance"] = reader[4];
                sumTable.Rows.Add(row1);
            }
            reader.Close();
            command = new SqlCommand(_cmdStr2, connect);
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                DataRow row2 = sumTable.NewRow();
                row2["Reconciliation Type"] = "Domestic - Retiree";
                row2["EBA Count"] = reader[0];
                row2["EBA Amount"] = reader[1];
                row2["Anthem Count"] = reader[2];
                row2["Anthem Amount"] = reader[3];
                row2["EBA Count Variance"] = reader[4];
                sumTable.Rows.Add(row2);
            }
            reader.Close();

            int _tEBA = 0, _tAnth = 0;
            string _tEBAAmnt, _tAnthAmt;

            if (sumTable.Rows.Count > 0)
            {
                _tEBA = Convert.ToInt32(sumTable.Compute("SUM([EBA Count])", String.Empty));
                _tAnth = Convert.ToInt32(sumTable.Compute("SUM([Anthem Count])", String.Empty));
                _tEBAAmnt = Convert.ToDecimal(sumTable.Compute("SUM([EBA Amount])", String.Empty)).ToString("F2");
                _tAnthAmt = Convert.ToDecimal(sumTable.Compute("SUM([Anthem Amount])", String.Empty)).ToString("F2");
                DataRow row4 = dsDom.Tables[0].NewRow();
                row4["Reconciliation Type"] = "Domestic Total:";
                row4["EBA Count"] = _tEBA;
                row4["EBA Amount"] = _tEBAAmnt;
                row4["Anthem Count"] = _tAnth;
                row4["Anthem Amount"] = _tAnthAmt;
                row4["EBA Count Variance"] = (_tEBA - _tAnth);
                sumTable.Rows.Add(row4);
            }

            return dsDom;
        }
        //CA BOA Claims Recon Data
        public static DataSet GetCAReconData(string yrmo)
        {
            decimal _per = getPercentageVar("CA BOA Claims",yrmo); 
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string cmdstr = "SELECT ca_rcn_checkno, CONVERT(Numeric(10,2),ca_rcn_clmamt) AS ca_rcn_clmamt , CONVERT(Numeric(10,2),ca_rcn_boaamt) AS ca_rcn_boaamt, ca_rcn_variance, "
                + "ca_rcn_varper,CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                + "WHEN ca_rcn_varper > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                + "WHEN ca_rcn_varper < " + _per + " AND ca_rcn_varper <> 0 THEN 'Less' "
                + "WHEN ca_rcn_varper = 0 THEN '-' "
                + "END ) AS var_threshold "
                + "FROM ca_recon WHERE ca_rcn_yrmo = '" + yrmo + "' AND ca_rcn_variance <> 0";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            return ds;
        }

        //public static DataSet boaUnmatched(string yrmo)
        //{
        //    decimal _per = getPercentageVar("CA BOA Claims", yrmo);
        //    if (connect == null || connect.State == ConnectionState.Closed)
        //    {
        //        connect.Open();
        //    }
        //    String cmdstr = "SELECT boaYRMO AS [YRMO],boaChqNo As [Cheque Number], "
        //                    + "CONVERT(Numeric(10,2),0 ) AS [Claims Amount], "
        //                    + "CONVERT(Numeric(10,2),[boaAmnt]) AS [BOA Amount], "
        //                    + "CONVERT(Numeric(10,2),(0 - boaAmnt)) AS [Variance], "
        //                    + "CONVERT(Decimal(10,2),100) AS [% Variance], "
        //                    + "CONVERT(Numeric(10,2)," + _per + ") AS [Threshold], "
        //                    + "'<font color=\"Red\">Greater</font>' AS [Threshold Level] "
        //                    + "FROM BOAStatement WHERE boaYRMO = '" + yrmo + "' "
        //                    + "AND boaChqNo NOT IN "
        //                    + "(SELECT anbt_checkNum FROM AnthBillTrans WHERE anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT')";

        //    command = new SqlCommand(cmdstr, connect);
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    DataSet ds1 = new DataSet();
        //    da.SelectCommand = command;
        //    da.Fill(ds1);
        //    connect.Close();
        //    return ds1;
        //}

        //public static DataSet claimsUnmatched(string yrmo)
        //{
        //    decimal _per = getPercentageVar("CA BOA Claims", yrmo);
        //    if (connect == null || connect.State == ConnectionState.Closed)
        //    {
        //        connect.Open();
        //    }
        //    String cmdstr = "SELECT anbt_yrmo AS [YRMO],anbt_checkNum As [Cheque Number], "
        //                    + "CONVERT(Numeric(10,2),[anbt_ClaimsPdAmt]) AS [Claims Amount], "
        //                    + "CONVERT(Numeric(10,2),0 ) AS [BOA Amount], "
        //                    + "CONVERT(Numeric(10,2),(anbt_ClaimsPdAmt - 0)) AS [Variance], "
        //                    + "CONVERT(Decimal(10,2),100) AS [% Variance], "
        //                    + "CONVERT(Numeric(10,2)," + _per + ") AS [Threshold], "
        //                    + "'<font color=\"Red\">Greater</font>' AS [Threshold Level] "
        //                    + "FROM AnthBillTrans WHERE anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT' "
        //                    + "AND anbt_checkNum NOT IN "
        //                    + "(SELECT boaChqNo FROM BOAStatement WHERE boaYRMO = '" + yrmo + "')";

        //    command = new SqlCommand(cmdstr, connect);
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    DataSet ds1 = new DataSet();
        //    da.SelectCommand = command;
        //    da.Fill(ds1);
        //    connect.Close();
        //    return ds1;
        //}
        
        //Non CA RFDF Recon Data

        public static DataSet GetDFRFReconData(string yrmo)
        {
            decimal _per = getPercentageVar("RF/DF Claims",yrmo); 
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string cmdstr = "SELECT CONVERT(Numeric(10,2),rf_clm_amt) AS rf_clm_amt,CONVERT(Numeric(10,2),rf_rfdf_amt) AS rf_rfdf_amt,rf_diff, "
                + "rf_per,CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                + "WHEN rf_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                + "WHEN rf_per < " + _per + " AND rf_per <> 0 THEN '-' "
                + "WHEN rf_per = 0 THEN '-' "
                + "END ) AS var_threshold "
                + "FROM rf_recon WHERE rf_yrmo = '" + yrmo + "'";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            return ds;
        }
        
        //Pharmacy Recon Data
        public static DataSet GetRxReconData(string yrmo)
        {
            decimal _per = getPercentageVar("Rx Claims",yrmo); 
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            string cmdstr = "SELECT CONVERT(Numeric(10,2),rx_rcn_clmamt) AS rx_rcn_clmamt, CONVERT(Numeric(10,2),rx_rcn_frdamt) AS rx_rcn_frdamt, rx_rcn_diff, "
                + "rx_rcn_per,CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                + "WHEN rx_rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                + "WHEN rx_rcn_per < " + _per + " AND rx_rcn_per <> 0 THEN '-' "
                + "WHEN rx_rcn_per = 0 THEN '-' "
                + "END ) AS var_threshold "
                + "FROM rx_recon WHERE rx_rcn_yrmo = '" + yrmo + "'";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            return ds;
        }

        //Domestic reconciliation
        public static void DomesticReconcile(string yrmo, string _anthsrc, string _ebasrc, string _rcnsrc)
        {            
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand("sp_DomesticRecon", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@yrmo", SqlDbType.VarChar);
            command.Parameters["@yrmo"].Value = yrmo;
            command.Parameters.Add("@anthsrc", SqlDbType.VarChar);
            command.Parameters["@anthsrc"].Value = _anthsrc;
            command.Parameters.Add("@ebasrc", SqlDbType.VarChar);
            command.Parameters["@ebasrc"].Value = _ebasrc;
            command.Parameters.Add("@reconsrc", SqlDbType.VarChar);
            command.Parameters["@reconsrc"].Value = _rcnsrc;
            command.ExecuteNonQuery();
            connect.Close();
        }


        //International Reconciliation
        public static void IntlReconcile(string yrmo)
        {            
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand("sp_IntlRecon", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@yrmo", SqlDbType.VarChar);
            command.Parameters["@yrmo"].Value = yrmo;
            command.ExecuteNonQuery();
            connect.Close();
        }

        public static DataSet getUnmatch(string _yrmo)
        {           
            List<int> plhridsPrism = new List<int>();
            List<int> plhridsAnth = new List<int>();
            plhridsPrism = getPrismPlhrIdWithVar(_yrmo);
            plhridsAnth = getAnthPlhrIdWithVar(_yrmo);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("HTHDATA");
            dt.Columns.Add("Plancd");
            dt.Columns.Add("Tiercd");
            dt.Columns.Add("SSN");
            dt.Columns.Add("Name");
            ds.Tables.Add(dt);
            DataTable dt1 = new DataTable("ANTHDATA");
            dt1.Columns.Add("Plancd");
            dt1.Columns.Add("Tiercd");
            dt1.Columns.Add("SSN");
            dt1.Columns.Add("Name");
            ds.Tables.Add(dt1);
            SqlDataReader reader = null;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                foreach (int plhrid in plhridsPrism)
                {
                    string cmdStr1 = "SELECT plhr_plancd, plhr_tiercd, bill_ssn, bill_name FROM billing_details, AnthPlanhier WHERE bill_anthcd_id = " + plhrid + " AND bill_source = 'HTH' AND bill_yrmo = '" + _yrmo + "' AND (bill_plhr_id = plhr_id) AND "
                                    + "bill_ssn NOT IN (SELECT bill_ssn FROM billing_details WHERE bill_anthcd_id = " + plhrid + " AND bill_source = 'ANTH_INTL' AND bill_yrmo = '" + _yrmo + "')";
                    command = new SqlCommand(cmdStr1, connect);
                    reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dr["Plancd"] = reader.GetString(0);
                        dr["Tiercd"] = reader.GetString(1);
                        dr["SSN"] = reader.GetString(2);
                        dr["Name"] = reader.GetString(3);
                        dt.Rows.Add(dr);
                    }
                    reader.Close();
                }


                foreach (int plhrid in plhridsAnth)
                {
                    string cmdStr1 = "SELECT anthcd_plancd, anthcd_covgcd, bill_ssn, bill_name FROM billing_details, AnthCodes WHERE bill_anthcd_id = " + plhrid + " AND bill_source = 'ANTH_INTL' AND bill_yrmo = '" + _yrmo + "' AND (bill_anthcd_id = anthcd_id) AND "
                                    + "bill_ssn NOT IN (SELECT bill_ssn FROM billing_details WHERE bill_anthcd_id = " + plhrid + " AND bill_source = 'HTH' AND bill_yrmo = '" + _yrmo + "')";
                    command = new SqlCommand(cmdStr1, connect);
                    reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        DataRow dr;
                        dr = dt1.NewRow();
                        dr["Plancd"] = reader.GetString(0);
                        dr["Tiercd"] = reader.GetString(1);
                        dr["SSN"] = reader.GetString(2);
                        dr["Name"] = reader.GetString(3);
                        dt1.Rows.Add(dr);
                    }
                    reader.Close();
                }
                
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
            
        }
        
        protected static List<int> getPrismPlhrIdWithVar(string _yrmo)
        {
            string cmdStr = "SELECT rcn_anthcd_id FROM billing_recon WHERE rcn_var > 0 AND rcn_yrmo = '" + _yrmo + "' AND rcn_source = 'INTL'";
            SqlDataReader dr = null;
            List<int> plhrids = new List<int>();
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                plhrids.Add(dr.GetInt32(0));
            }
            dr.Close();
            return plhrids;
        }

        protected static List<int> getAnthPlhrIdWithVar(string _yrmo)
        {
            string cmdStr = "SELECT rcn_anthcd_id FROM billing_recon WHERE rcn_var < 0 AND rcn_yrmo = '" + _yrmo + "' AND rcn_source = 'INTL'";
            SqlDataReader dr = null;
            List<int> plhrids = new List<int>();
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                plhrids.Add(dr.GetInt32(0));
            }
            dr.Close();
            return plhrids;
        }

        public static DataSet getSSNHTH(string ssn)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            string cmdStr = "SELECT bill_yrmo,plhr_plancd, plhr_tiercd, bill_ssn, bill_name FROM billing_details, AnthPlanhier WHERE bill_ssn = " + ssn + " AND bill_source = 'HTH'  AND (bill_plhr_id = plhr_id) ORDER BY bill_yrmo";
            command = new SqlCommand(cmdStr, connect);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            da.SelectCommand = command;
            da.Fill(ds1);            
            connect.Close();
            return ds1;
        }

        public static DataSet getSSNANTH(string ssn)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            string cmdStr = "SELECT bill_yrmo,anthcd_plancd, anthcd_covgcd, bill_ssn, bill_name FROM billing_details, AnthCodes WHERE bill_ssn = " + ssn + " AND bill_source = 'HTH'  AND (bill_anthcd_id = anthcd_id) ORDER BY bill_yrmo";
            command = new SqlCommand(cmdStr, connect);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            da.SelectCommand = command;
            da.Fill(ds1);
            connect.Close();
            return ds1;
        }
        
        // CA claims Reconciliation
        public static void CAReconcile(string yrmo)
        {            
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand("sp_InsertCARecon", connect);
            command.CommandTimeout = 1200;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@yrmo", SqlDbType.VarChar);
            command.Parameters["@yrmo"].Value = yrmo;
            command.ExecuteNonQuery();
            connect.Close();
        }

        public static DataSet getDupsBalance(string yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            List<int> chqNo = new List<int>();
            string cmdStr0 = "select anbt_checkNum FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' GROUP BY anbt_checkNum HAVING COUNT(anbt_checkNum) > 1 ORDER BY anbt_checkNum";
            command = new SqlCommand(cmdStr0, connect);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                chqNo.Add(Convert.ToInt32(dr[0]));
            }
            dr.Close();
            string cmdStr1 = "SELECT boaChqNo FROM BOAStatement WHERE boaYRMO <= '" + yrmo + "' GROUP BY boaChqNo HAVING COUNT(boaChqNo) > 1 ORDER BY boaChqNo";
            command = new SqlCommand(cmdStr1, connect);
            SqlDataReader dr1 = command.ExecuteReader();
            while (dr1.Read())
            {
                chqNo.Add(Convert.ToInt32(dr1[0]));
            }
            dr1.Close();
            string cmdStr = "SELECT ca_rcn_yrmo,ca_rcn_checkno,CONVERT(Numeric(10,2),ca_rcn_clmamt),CONVERT(Numeric(10,2),ca_rcn_boaamt),CONVERT(Numeric(10,2),ca_rcn_variance) FROM ca_recon WHERE ca_rcn_yrmo <= '" + yrmo + "' AND ca_rcn_checkno = @cheqno";
            DataSet ds = new DataSet();
            DataRow row;
            DataTable cl = ds.Tables.Add("DUP");
            cl.Columns.Add("YRMO");
            cl.Columns.Add("Cheque Number");
            cl.Columns.Add("Claims Amount");
            cl.Columns.Add("BOA Amount");
            cl.Columns.Add("Variance");
            foreach (int li in chqNo)
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@cheqno", li);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    row = ds.Tables["DUP"].NewRow();
                    row["YRMO"] = reader.GetString(0);
                    row["Cheque Number"] = reader.GetInt32(1);
                    row["Claims Amount"] = reader.GetDecimal(2);
                    row["BOA Amount"] = reader.GetDecimal(3);
                    row["Variance"] = reader.GetDecimal(4);
                    ds.Tables["DUP"].Rows.Add(row);
                }
                reader.Close();
            }
            connect.Close();
            return ds;
        }

        public static DataSet getBOADups(string yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            List<int> chqNo = new List<int>();
            string cmdStr0 = "SELECT boaChqNo FROM BOAStatement WHERE boaYRMO <= '" + yrmo + "' GROUP BY boaChqNo HAVING COUNT(boaChqNo) > 1 ORDER BY boaChqNo";
            command = new SqlCommand(cmdStr0, connect);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                chqNo.Add(Convert.ToInt32(dr[0]));
            }
            dr.Close();
            string cmdStr = "SELECT boaYRMO, boaBankRef, CONVERT(VARCHAR(15),boaPosted_dt,101), boaChqNo ,CONVERT(Numeric(10,2),boaAmnt) FROM BOAStatement WHERE boaChqNo = @chqno ORDER BY boaYRMO";
            DataSet ds = new DataSet();
            DataRow row;
            DataTable cl = ds.Tables.Add("BOA");
            cl.Columns.Add("YRMO");            
            cl.Columns.Add("Bank Reference");
            cl.Columns.Add("Posted Date");
            cl.Columns.Add("Cheque Number");
            cl.Columns.Add("BOA Amount");
            
            foreach (int li in chqNo)
            {
                 command = new SqlCommand(cmdStr, connect);
                 command.Parameters.AddWithValue("@chqno", li);
                 SqlDataReader reader = command.ExecuteReader();
                 while (reader.Read())
                 {
                     row = ds.Tables["BOA"].NewRow();
                     row["YRMO"] = reader.GetString(0);
                     row["Bank Reference"] = reader.GetString(1);
                     row["Posted Date"] = reader.GetString(2);
                     row["Cheque Number"] = reader.GetInt32(3);
                     row["BOA Amount"] = reader.GetDecimal(4);
                     ds.Tables["BOA"].Rows.Add(row);
                 }
                 reader.Close();
            }
            connect.Close();
            return ds;
        }

        public static DataSet getClaimsDups(string yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            List<int> chqNo = new List<int>();
            string cmdStr0 = "select anbt_checkNum FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' GROUP BY anbt_checkNum HAVING COUNT(anbt_checkNum) > 1 ORDER BY anbt_checkNum";
            command = new SqlCommand(cmdStr0, connect);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                chqNo.Add(Convert.ToInt32(dr[0]));
            }
            dr.Close();
            string cmdStr = "SELECT anbt_yrmo,anbt_claimid, CONVERT(VARCHAR(15),anbt_datePd,101), anbt_subid_ssn,anbt_checkNum, CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_checkNum = @chqno ORDER BY anbt_yrmo";
            DataSet ds = new DataSet();
            DataTable cl = ds.Tables.Add("Claims");
            cl.Columns.Add("YRMO");
            cl.Columns.Add("Claim ID");
            cl.Columns.Add("Subscriber ID");
            cl.Columns.Add("Posted Date");
            cl.Columns.Add("Cheque Number");
            cl.Columns.Add("Anthem Claims Amount");

            DataRow row;
            
            foreach (int li in chqNo)
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@chqno", li);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    row = ds.Tables["Claims"].NewRow();
                    row["YRMO"] = reader.GetString(0);
                    row["Claim ID"] = reader.GetString(1);
                    row["Subscriber ID"] = reader.GetString(2);
                    row["Posted Date"] = reader.GetString(3);
                    row["Cheque Number"] = Convert.ToInt32(reader[4]);
                    row["Anthem Claims Amount"] = reader.GetDecimal(5);
                    ds.Tables["Claims"].Rows.Add(row);
                }
                reader.Close();
            }
            connect.Close();
            return ds;
        }

        //FRD details
        public static DataSet getFRDDetails(string yrmo)
        {
            if (connectFRD == null || connectFRD.State == ConnectionState.Closed)
            {
                connectFRD.Open();
            }
            decimal _yrmo = Convert.ToDecimal(yrmo);
            string cmdStr = "SELECT yrmo, provid, CONVERT(Numeric(10,2),amt) AS amt, CONVERT(varchar(15),wiredt,101) AS wiredt, transno, bankacctno,benedesc FROM banksum where provid = 'ANTHEMRX' AND yrmo = " + _yrmo;
           
            command = new SqlCommand(cmdStr, connectFRD);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();            
            da.SelectCommand = command;
            da.Fill(ds);
            return ds;
        }                

        //EAP Reconciliation
        public static void insertEAPRecon(string yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            int _anthcount = 0;
            int _ebacount = 0;
            int _hmocount = 0;
            int _totaleba = 0;
            decimal _rate;
            decimal _diff;
            decimal _per;
            decimal _ebaAmt;
            decimal _anthAmt;
            string cmdStr1 = "select count(*) from billing_details where bill_source = 'ANTH_EAP' and bill_yrmo = '" + yrmo + "'";
            string cmdStr2 = "select SUM(hdct_count) from Headcount where hdct_source in ('HTH','GRS','ADP','RET_M','RET_NM') and hdct_yrmo = '" + yrmo + "'";
            string cmdStr5 = "SELECT SUM(hmo_count) FROM billing_HMO WHERE hmo_yrmo = '" + yrmo + "'";
            command = new SqlCommand(cmdStr1, connect);
            _anthcount = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            command = new SqlCommand(cmdStr2, connect);
            _ebacount = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            command = new SqlCommand(cmdStr5, connect);
            _hmocount = Convert.ToInt32(command.ExecuteScalar());
            command.Dispose();
            _totaleba = (_ebacount + _hmocount);
            _diff = ( _totaleba - _anthcount);
            if (_diff > 0)
            {
                _per = ((decimal)(_totaleba - _anthcount) / _totaleba) * 100;
            }
            else if (_diff < 0)
            {
                _per = ((decimal)(_anthcount - _totaleba) / _anthcount) * 100;
            }
            else
            {
                _per = 0;
            }
            string cmdStr4 = "select DISTINCT plhr_CompanyRate FROM AnthPlanhier WHERE plhr_plandesc LIKE 'EAP'";
            command = new SqlCommand(cmdStr4, connect);
            _rate = Convert.ToDecimal(command.ExecuteScalar());
            command.Dispose();
            _ebaAmt = (_totaleba * _rate);
            _anthAmt = (_anthcount * _rate);
            string cmdStr3 = "INSERT INTO billing_recon (rcn_source, rcn_yrmo,rcn_prism_count,rcn_anth_count,rcn_var,rcn_per,rcn_prism_amt,rcn_anth_amt) VALUES ('EAP' , '" + yrmo + "'," + _totaleba + "," + _anthcount + "," + _diff + "," + _per + "," + _ebaAmt + "," + _anthAmt + ")";
            command = new SqlCommand(cmdStr3, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            connect.Close();
        }

        public static DataSet GetEAPReconData(string yrmo)
        {
            decimal _per = getPercentageVar("EAP",yrmo);
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            string cmdStr = "SELECT rcn_yrmo,rcn_prism_count,CONVERT(Numeric(10,2),rcn_prism_amt) AS rcn_prism_amt,rcn_anth_count,CONVERT(Numeric(10,2),rcn_anth_amt) AS rcn_anth_amt,rcn_var, "
                + "rcn_per,CONVERT(Numeric(10,2)," + _per + ") AS threshold,( CASE "
                + "WHEN rcn_per > " + _per + " THEN '<font color=\"Red\">Exceeded</font>' "
                + "WHEN rcn_per < " + _per + " AND rcn_per <> 0 THEN '-' "
                + "WHEN rcn_per = 0 THEN '-' "
                + "END ) AS var_threshold "
                + "FROM billing_recon WHERE rcn_source = 'EAP' and rcn_yrmo = '" + yrmo + "'";
            command = new SqlCommand(cmdStr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            return ds;
        }

        public static DataSet GetEAPReconDetails(string yrmo)
        {
            DataSet dsDet = new DataSet();
            DataSet dsHMO = new DataSet();
            DataSet dsOptOuts = new DataSet();
            DataSet dsEAP = new DataSet();
            dsDet.Clear();
            dsHMO.Clear();
            dsOptOuts.Clear();
            dsEAP.Clear();

            int _totEBA = 0;
            int _totAnth = 0;
            //DataTable detTable = dsDet.Tables.Add("EAPDetails");
            //DataColumn col;
            //col = new DataColumn("YRMO"); detTable.Columns.Add(col);
            //col = new DataColumn("Source"); detTable.Columns.Add(col);
            //col = new DataColumn("Count"); detTable.Columns.Add(col);
            SqlDataAdapter da = new SqlDataAdapter();
            string _cmdstr = "SELECT [hdct_yrmo] AS [YRMO], "
                            + " CASE [hdct_source] "
                            + " WHEN 'ADP' THEN 'ADP Cobra' "
                            + " WHEN 'GRS' THEN 'GRS Pilot' "
                            + " WHEN 'HTH' THEN 'HTH International' "
                            + " WHEN 'RET_M' THEN 'Retiree Medicare' "
                            + " WHEN 'RET_NM' THEN 'Retiree Non Medicare' "
                            + " END AS [Source], "
                            + " SUM(hdct_count) AS [EBA Count], 0 AS [Anthem Count] " 
                            + " FROM [Headcount] WHERE [hdct_yrmo] = @yrmo  "
                            + " AND (hdct_source = 'ADP' or hdct_source = 'GRS' or hdct_source = 'HTH' or hdct_source = 'RET_NM' or hdct_source = 'RET_M')  "
                            + " GROUP BY [hdct_yrmo],[hdct_source]";

            string _cmdstr1 = "SELECT [hmo_yrmo]  AS [YRMO],"
                            + " CASE [hmo_source] "
                            + " WHEN 'GRS' THEN 'GRS Pilot - HMO' "
                            + " WHEN 'RET_M' THEN 'Retiree Medicare - HMO' "
                            + " WHEN 'RET_NM' THEN 'Retiree Non Medicare - HMO' "
                            + " END AS [Source], "
                            + " SUM(hmo_count) AS [EBA Count], 0 AS [Anthem Count]  " 
                            + " FROM billing_HMO "
                            + " WHERE hmo_yrmo = @yrmo AND hmo_plancd <> 'OP'"
                            + " GROUP BY hmo_yrmo,hmo_source";

            string _cmdstr2 = "SELECT [hmo_yrmo]  AS [YRMO],"
                            + " CASE [hmo_source] "
                            + " WHEN 'GRS' THEN 'GRS Pilot - OPT OUTS' "
                            + " WHEN 'RET_M' THEN 'Retiree Medicare - OPT OUTS' "
                            + " WHEN 'RET_NM' THEN 'Retiree Non Medicare - OPT OUTS' "
                            + " END AS [Source], "
                            + " SUM(hmo_count) AS [EBA Count], 0 AS [Anthem Count]  "
                            + " FROM billing_HMO "
                            + " WHERE hmo_yrmo = @yrmo AND hmo_plancd = 'OP'"
                            + " GROUP BY hmo_yrmo,hmo_source";

            string _cmdstr3 = "SELECT [bill_yrmo] AS [YRMO], "
                            + " CASE [bill_source] "
                            + " WHEN 'ANTH_EAP' THEN 'Anthem Bill EAP' "
                            + " END AS [Source], "
                            + " 0 AS [EBA Count] , COUNT(*) AS [Anthem Count] " 
                            + " FROM billing_details where bill_source = 'ANTH_EAP' and bill_yrmo = '" + yrmo + "'"
                            + " GROUP BY bill_yrmo,bill_source";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsDet);
                command = new SqlCommand(_cmdstr1, connect);
                command.Parameters.AddWithValue("yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsHMO);
                dsDet.Merge(dsHMO);
                command = new SqlCommand(_cmdstr2, connect);
                command.Parameters.AddWithValue("yrmo", yrmo);
                da.SelectCommand = command;
                da.Fill(dsOptOuts);
                dsDet.Merge(dsOptOuts);
                command = new SqlCommand(_cmdstr3, connect);                
                da.SelectCommand = command;
                da.Fill(dsEAP);
                dsDet.Merge(dsEAP);

                if (dsDet.Tables[0].Rows.Count > 0)
                {
                    _totEBA = Convert.ToInt32(dsDet.Tables[0].Compute("SUM([EBA Count])", String.Empty));
                    _totAnth = Convert.ToInt32(dsDet.Tables[0].Compute("SUM([Anthem Count])", String.Empty));
                    DataRow row = dsDet.Tables[0].NewRow();
                    row["Source"] = "Total: ";
                    row["EBA Count"] = _totEBA;
                    row["Anthem Count"] = _totAnth;
                    dsDet.Tables[0].Rows.Add(row);
                }
                return dsDet;
            }
            finally
            {
                connect.Close();
            }
        }

        //RFDF Reconciliation - DF no RF Cleared Report
        public static DataSet getDFRecords(string yrmo)
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            string cmdStr = "SELECT rfdf_plancd,rfdf_sccf,rfdf_subid,rfdf_dcn,rfdf_amt,rfdf_lastupdt FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_cleared = '1' AND rfdf_yrmo = '" + yrmo +"'" ;
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            command = new SqlCommand(cmdStr, connect);
            da.SelectCommand = command;
            da.Fill(ds);
            connect.Close();
            return ds;
        }

        public static DataSet getDFAgingRpt(string yrmo)
        {
            string yrmo_30 = AnthRecon.prevYRMO(yrmo);
            string yrmo_60 = AnthRecon.prevYRMO(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string cmdStr = "SELECT DISTINCT rfdf_dcn AS DCN, rfdf_subid AS MemberID, CONVERT(VARCHAR(15),rfdf_lastupdt,101) AS LastUpdate, rfdf_yrmo AS YRMO, "
                            + "CASE "
                            + "WHEN rfdf_yrmo = '" + yrmo + "' "
                            + "THEN rfdf_amt "
                            + "ELSE 0 "
                            + "END AS [" + yrmo + "] "
                            + ",CASE "
                            + "WHEN rfdf_yrmo = '" + yrmo_30 + "' "
                            + "THEN rfdf_amt "
                            + "ELSE 0 "
                            + "END AS [31 - 60 Days] "
                            + ",CASE "
                            + "WHEN rfdf_yrmo = '" + yrmo_60 + "' "
                            + "THEN rfdf_amt "
                            + "ELSE 0 "
                            + "END AS [61 - 90 Days] "
                            + ",CASE "
                            + "WHEN rfdf_yrmo < '" + yrmo_60 + "' "
                            + "THEN rfdf_amt "
                            + "ELSE 0 "
                            + "END AS [90+ Days] "
                            + "FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_cleared = 0 "
                            + " AND rfdf_yrmo <= '" + yrmo +"' "
                            + "ORDER BY YRMO, [0 - 30 Days],[31 - 60 Days],[61 - 90 Days], [90+ Days] ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public static DataSet getReconM(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string cmdStr = "select  a.anbt_yrmo, "
                            + "a.anbt_claimid,Convert(Numeric(10,2),SUM(a.anbt_claimsPdAmt)) AS claimsAmt , "
                            + " Convert(Numeric(10,2),rfAmt) AS rfAmt ,Convert(Numeric(10,2),(SUM(a.anbt_claimsPdAmt) - rfamt)) diff"
                            + " FROM AnthBillTrans a "
                            + "INNER JOIN "
                            + "("
                            + " SELECT rfdf_dcn dcn,sum(rfdf_amt) rfAmt,rfdf_yrmo rfyrmo,rfdf_source rfsource FROM RFDF WHERE rfdf_yrmo = '" + yrmo + "' AND rfdf_source = 'RF'"
                            + "GROUP BY rfdf_dcn,rfdf_yrmo,rfdf_source "
                            + " ) b "
                            + " ON (a.anbt_claimid = dcn)"
                            + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo = '" + yrmo + "' "
                            + " GROUP BY a.anbt_yrmo,a.anbt_claimid,rfAmt"                             
                            + " ORDER BY diff ";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public static DataSet getReconDCNAmtM(string yrmo,string _filter)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsMatched = new DataSet();

            string cmdStr = "select  '" + yrmo + "' AS anbt_yrmo, "
                            + "a.anbt_claimid,Convert(Numeric(10,2),SUM(a.anbt_claimsPdAmt)) AS claimsAmt , "
                            + " Convert(Numeric(10,2),rfAmt) AS rfAmt ,Convert(Numeric(10,2),(SUM(a.anbt_claimsPdAmt) - rfamt)) diff"
                            + " FROM AnthBillTrans a "
                            + "INNER JOIN "
                            + "("
                            + " SELECT rfdf_dcn dcn,sum(rfdf_amt) rfAmt,rfdf_source rfsource "
                            + " FROM RFDF "
                            + " WHERE rfdf_yrmo <= '" + yrmo + "' AND rfdf_source = 'RF'"
                            + " GROUP BY rfdf_dcn,rfdf_source "
                            + " ) b "
                            + " ON (a.anbt_claimid = dcn) "
                            + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' "
                            + " AND anbt_claimid IN ( "
                            + " SELECT DISTINCT anbt_claimid "
                            + " FROM AnthBillTrans a "
                            + " INNER JOIN "
                            + " ( "
                            + " 	SELECT DISTINCT rfdf_dcn dcn "
                            + " 	FROM RFDF "
                            + " 	WHERE rfdf_yrmo = '" + yrmo + "' "
                            + " 	AND  rfdf_source = 'RF' "
                            + " ) b  "
                            + " ON (a.anbt_claimid = b.dcn)  "
                            + " WHERE anbt_sourcecd ='NCA_CLMRPT' AND anbt_yrmo = '" + yrmo + "' ) "
                            + " GROUP BY a.anbt_claimid,rfAmt "
                            + " HAVING (SUM(a.anbt_claimsPdAmt) - rfamt) " + _filter; 
          
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(dsMatched);
              
                return dsMatched;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public static DataSet getMatchedDCNAmnt(string yrmo, string _type)
        {
            DataSet dsF1 = new DataSet(); dsF1.Clear();            
            string strFilter = null;
            switch (_type)
            {
                case "Matched":
                    strFilter = " = 0";
                    break;
                case "UnMatched":
                    strFilter = " <> 0";
                    break;
            }

            dsF1 = ReconDAL.getReconDCNAmtM(yrmo,strFilter);

            return dsF1;
        }

        public static DataSet getRFDFUM(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string cmdStr = "select rfdf_yrmo,rfdf_dcn,rfdf_subid, "
                            + " CONVERT(VARCHAR(15),rfdf_pddt,101) AS rfdf_pddt, "
                            + " CONVERT(Numeric(10,2),rfdf_amt) AS rfdf_amt"
                            + " from  rfdf where rfdf_yrmo = '" + yrmo + "' AND rfdf_source = 'RF'" 
                            + " AND rfdf_dcn NOT IN "
                            + " (select anbt_claimid from AnthBillTrans where anbt_sourcecd = 'NCA_CLMRPT' and anbt_yrmo = '" + yrmo + "') ORDER BY rfdf_amt DESC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public static DataSet getClaimsUM(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string cmdStr = " select anbt_yrmo,anbt_claimid,anbt_subid_ssn, "
                            + " CONVERT(VARCHAR(15),anbt_datePd,101) AS anbt_datePd, "
                            + " CONVERT(Numeric(10,2),anbt_claimsPdAmt) AS anbt_claimsPdAmt "
                            + " FROM AnthBillTrans where anbt_yrmo = '" + yrmo + "' AND anbt_sourcecd = 'NCA_CLMRPT' "
                            + " AND anbt_claimid NOT IN "
                            + " (select rfdf_dcn from  rfdf where rfdf_yrmo = '" + yrmo + "' and rfdf_source = 'RF') ORDER BY anbt_claimid";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }
        //Past Reconcile
        public static Boolean pastReconcile(string yrmo, string src)
        {
            string cmdStr = "";
            switch (src)
            {
                case "DOM":
                    cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "' AND (rcn_source = 'ACT' OR rcn_source = 'RET' OR rcn_source = 'COB')";
                    break;
                case "INTL":
                    cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "' AND rcn_source = 'INTL'" ;
                    break;
                case "CA":
                    cmdStr = "SELECT COUNT(*) FROM ca_recon WHERE ca_rcn_yrmo = '" + yrmo + "'";
                    break;
                case "RFDF":
                    cmdStr = "SELECT COUNT(*) FROM rf_recon WHERE rf_yrmo = '" + yrmo + "'";
                    break;
                case "Rx":
                    cmdStr = "SELECT COUNT(*) FROM rx_recon WHERE rx_rcn_yrmo = '" + yrmo + "'";
                    break;
                case "EAP":
                    cmdStr = "SELECT COUNT(*) FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "'AND rcn_source = 'EAP'";
                    break;
            }
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }           
            command = new SqlCommand(cmdStr, connect);
            int count = Convert.ToInt32(command.ExecuteScalar());  
            connect.Close();
            if (count == 0)
            {
                return false;
            }
            return true;
            
        }
        
        //Re Recon logic:
        public static void pastReconcileDelete(string yrmo, string src)
        {
            string cmdStr = "";            
            SqlDataReader dr = null;
            switch (src)
            {
                case "DOM":
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "DELETE FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "' AND rcn_source = 'ACT' OR rcn_source = 'RET' OR rcn_source = 'COB'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    cmdStr = "UPDATE Headcount " +
                                "SET hdct_cleared = 0 " +
                                "WHERE hdct_yrmo = '" + yrmo + "' AND (hdct_source = 'GRS' OR hdct_source = 'ADP' OR hdct_source LIKE 'RET%' OR hdct_source = 'ANTH_ACT' OR hdct_source = 'ANTH_RET' OR hdct_source = 'ANTH_COB')";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    break;
                case "INTL":
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "DELETE FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "' AND rcn_source = 'INTL'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    cmdStr = "UPDATE Headcount " +
				                "SET hdct_cleared = 0 " +
				                "WHERE hdct_yrmo = '" + yrmo + "' AND (hdct_source = 'HTH' OR hdct_source = 'ANTH_INTL')";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    break;
                case "CA":                 
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "SELECT ca_rcn_checkno FROM ca_recon WHERE ca_rcn_CFCleared = 1 AND ca_rcn_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    dr = command.ExecuteReader();                    
                    List<int> checknums = new List<int>();
                    while (dr.Read())
                    {
                        checknums.Add(dr.GetInt32(0));
                    }
                    dr.Close();
                    command.Dispose();
                    foreach (int checknum in checknums)
                    {
                        cmdStr = "UPDATE AnthBillTrans SET anbt_CF = 1 WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_checkNum = '" + checknum + "'";
                        command = new SqlCommand(cmdStr, connect);
                        command.ExecuteNonQuery();
                        cmdStr = "UPDATE BOAStatement SET boaCF = 1 WHERE boaChqNo = '" + checknum + "'";
                        command = new SqlCommand(cmdStr, connect);
                        command.ExecuteNonQuery();
                    }
                    cmdStr = "DELETE FROM ca_recon WHERE ca_rcn_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    cmdStr = "UPDATE AnthBillTrans SET anbt_CF = 0, anbt_cleared = 0 WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    cmdStr = "UPDATE BOAStatement SET boaCF = 0, boaCleared = 0 WHERE boaYRMO >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery(); 
                    break;                   
                case "RFDF":
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "DELETE FROM rf_recon WHERE rf_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    cmdStr = "SELECT rfdf_dcn FROM RFDF WHERE rfdf_source = 'RFDF' AND rfdf_cleared = 1 AND rfdf_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    dr = command.ExecuteReader();

                    List<string> dcns = new List<string>();
                    while (dr.Read())
                    {
                        dcns.Add(dr.GetString(0));
                    }
                    dr.Close();
                    command.Dispose();
                    foreach (string dcn in dcns)
                    {
                        cmdStr = "UPDATE RFDF SET rfdf_cleared = 0 WHERE rfdf_source = 'DF' AND rfdf_dcn = '" + dcn + "' AND rfdf_yrmo < (SELECT Max(rfdf_yrmo) FROM RFDF)";
                        command = new SqlCommand(cmdStr, connect);
                        command.ExecuteNonQuery();
                    }
                    cmdStr = "UPDATE RFDF SET rfdf_cleared = 0 WHERE rfdf_source = 'RFDF' AND rfdf_yrmo >= '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();

                    break;
                case "Rx":
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "DELETE FROM rx_recon WHERE rx_rcn_yrmo = '" + yrmo + "'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    break;
                case "EAP":
                    if (connect != null && connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    cmdStr = "DELETE FROM billing_recon WHERE rcn_yrmo = '" + yrmo + "' AND rcn_source = 'EAP'";
                    command = new SqlCommand(cmdStr, connect);
                    command.ExecuteNonQuery();
                    break;
            }
            connect.Close();
        }

        public static string GetLatestYRMO(string source)
        {            
            string _yrmo = "200801";
            string _cmdstr = "";

            switch (source)
            {
                case "CA":
                    _cmdstr = "SELECT max(yrmo) FROM CAClaimsRecon";
                    break;
                case "Non-CA":
                    _cmdstr = "SELECT max(rf_yrmo) FROM rf_recon";
                    break;
            }
           
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
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
    }
}

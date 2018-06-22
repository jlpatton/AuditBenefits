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

namespace EBA.Desktop.HRA
{
    public class HRASofoDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRASofoDAL()
        {
        }

        public Boolean pastReconcile(string yrmo)
        {
            string cmdStr = "SELECT COUNT(*) FROM hrasofo WHERE yrmo = @yrmo";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                int count = Convert.ToInt32(command.ExecuteScalar());
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

        public void ReconDelete(string yrmo)
        {
            string cmdstr = "DELETE FROM hrasofo WHERE yrmo = @yrmo";            

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                command.Dispose();               
            }
            finally
            {
                connect.Close();
            }
        }

        public Decimal getPrevEndBal(String yrmo)
        {
            Decimal prev_endbal = 0;            
            String cmdstr = "SELECT endbal FROM hrasofo WHERE yrmo = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }                
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                Object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    prev_endbal = Convert.ToDecimal(result);
                }
                
                return prev_endbal;
            }
            finally
            {
                connect.Close();
            }
        }

        public Boolean ExistsPriSOFO(String yrmo)
        {
            String cmdstr = "SELECT COUNT(*) FROM hrasofo WHERE yrmo = @yrmo";
            int count;
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    return true;
                }
                return false;
            }
            finally
            {
                connect.Close();
            }
        }

        public Decimal getTotPutnamAmt(String yrmo)
        {
            Decimal totPutnamAmt = 0;
            string cmdstr = "SELECT SUM(ptnm_distamt) FROM Putnam WHERE ptnm_yrmo = @yrmo";
            
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                Object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    totPutnamAmt = Convert.ToDecimal(result);
                }

                return totPutnamAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public Decimal getTotPutnamAdjAmt(String yrmo)
        {
            Decimal totPutnamAdjAmt = 0;
            string cmdstr = "SELECT SUM(ptna_amt) FROM PutnamAdj WHERE ptna_yrmo = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                Object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    totPutnamAdjAmt = Convert.ToDecimal(result);
                }

                return totPutnamAdjAmt;
            }
            finally
            {
                connect.Close();
            }
        }

        public void insertSOFOtbl(String yrmo, Decimal begBal, Decimal endBal, Decimal totContr, Decimal totDiv, Decimal termPay, Decimal withdraw, Decimal totPutnamAmt, Decimal totPutnamAdjAmt, Decimal diffamt, Decimal manAdjAmt, String manAdjNotes)
        {
            string _uid = HttpContext.Current.User.Identity.Name;
            string cmdstr = "INSERT INTO hrasofo "
                                + "(yrmo, startbal, contrib, dividends, termpmt, withdrawals, endbal, totptnm, totptnmadj, diffamt, manAdjAmt, manAdjNotes, userID) "
                                + "VALUES(@yrmo, @begBal, @totContr, @totDiv, @termPay, @withdraw, @endBal, @totPutnamAmt, @totPutnamAdjAmt, @diffamt, @manAdjAmt, @manAdjNotes, @uid )";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@begBal", begBal);
                command.Parameters.AddWithValue("@totContr", totContr);
                command.Parameters.AddWithValue("@totDiv", totDiv);
                command.Parameters.AddWithValue("@termPay", termPay);
                command.Parameters.AddWithValue("@withdraw", withdraw);
                command.Parameters.AddWithValue("@endBal", endBal);
                command.Parameters.AddWithValue("@totPutnamAmt", totPutnamAmt);
                command.Parameters.AddWithValue("@totPutnamAdjAmt", totPutnamAdjAmt);
                command.Parameters.AddWithValue("@diffamt", diffamt);
                command.Parameters.AddWithValue("@manAdjAmt", manAdjAmt);
                command.Parameters.AddWithValue("@manAdjNotes", manAdjNotes);
                command.Parameters.AddWithValue("@uid", _uid);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet getBalanceDtl(String yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            String cmdstr = "SELECT ptnm_ssn AS [SSN], "
                              + "ptnm_lname AS [Last Name], "
                              + "ptnm_fname AS [First Name], "
                              + "ptnm_transtype AS [Transaction], "
                              + "CONVERT(VARCHAR(15),ptnm_distribdt,101) AS [Transaction Date], "
                              + "ptnm_distamt AS [Putnam Amount] "
                              + "FROM Putnam "
                              + "WHERE ptnm_yrmo = @yrmo "
                              + "ORDER BY ptnm_lname, ptnm_fname, ptnm_distribdt";

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
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet getSOFOReconData(String yrmo)
        {
            SqlDataReader dr = null;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            DataTable dsTable;            
            dsTable = ds.Tables.Add("newTable1");           
            DataColumn col;
            String[] cols = new String[]{"Title","Value"};
            Object[] rows;
            String cmdstr = "SELECT startbal, contrib, dividends, termpmt, totptnm, withdrawals, totptnmadj, endbal, diffamt, manAdjAmt, manAdjNotes, userID FROM hrasofo "
                              + "WHERE yrmo = @yrmo";


            try
            {
                foreach(String colindex in cols)
                {
                    col = new DataColumn(colindex); 
                    dsTable.Columns.Add(col);
                }
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                dr = command.ExecuteReader();
                if (dr.Read())
                {
                    rows = new Object[] { "YRMO Balanced:", yrmo }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "UserID:", dr.GetInt32(11) }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Starting Balance:", dr.GetDecimal(0).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Contributions:", dr.GetDecimal(1).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Dividends:", dr.GetDecimal(2).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Termination Payments:", dr.GetDecimal(3).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Putnam Report Total:", dr.GetDecimal(4).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Withdrawals:", dr.GetDecimal(5).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Putnam Adj Total:", dr.GetDecimal(6).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Ending Balance:", dr.GetDecimal(7).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Manual Adj Amount:", dr.GetDecimal(9).ToString("C") }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Manual Adj Notes:", dr.GetString(10) }; addrow(rows, dsTable, cols);
                    rows = new Object[] { "Difference: Summary vs. Detail:", dr.GetDecimal(8).ToString("C") }; addrow(rows, dsTable, cols);                    
                }
                dr.Close();
                
                return ds;
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

        void addrow(Object[] rowValues, DataTable dsTable, String[] cols)
        {
            DataRow row;

            row = dsTable.NewRow();
            for (int i = 0; i < cols.Length; i++ )
            {
                row[i] = rowValues[i];
            }
            dsTable.Rows.Add(row);
        }

        public string HRAReconBal(string yrmo)
        {
            string _msg;
            string cmdStr1 = "SELECT COUNT(*) FROM ImportRecon_status WHERE period= @yrmo AND source='Recon' AND type='Recon' AND module='HRA'";
            string cmdStr2 = "SELECT COUNT(*) FROM hra_recon WHERE hrcn_yrmo = @yrmo AND hrcn_diffamt <> 0";
            int count = 0;

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar()); command.Dispose();
                if (count != 0)
                {
                    command = new SqlCommand(cmdStr2, connect);
                    command.Parameters.AddWithValue("@yrmo", yrmo);
                    count = Convert.ToInt32(command.ExecuteScalar()); command.Dispose();
                    if (count == 0)
                    {
                        _msg = String.Empty;
                    }
                    else
                    {
                        _msg = "HRA Reconciliation routine is performed but not balanced for Year-Month: " + yrmo + " !";
                    }
                }
                else
                {
                    _msg = "HRA Reconciliation routine is not performed for Year-Month: " + yrmo + " !"; ;
                }

                return _msg;
            }
            finally
            {
                connect.Close();
            }
        }
    }
}

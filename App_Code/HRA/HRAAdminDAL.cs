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

namespace EBA.Desktop.HRA
{
    public class HRAAdminDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRAAdminDAL()
        {           
        }

        public static Boolean pastReconcile(string _qy)
        {
            string cmdStr = "SELECT COUNT(*) FROM ImportRecon_status WHERE period=@qy AND source='Recon' AND type='AdminBill' AND module='HRA'";
            int count = 0;

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
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

        public void ReconDelete(string _qy)
        {
            string cmdstr = "DELETE FROM ImportRecon_status WHERE period=@qy AND source='Recon' AND type='AdminBill' AND module='HRA'";
           
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.ExecuteNonQuery();
                command.Dispose();                
            }
            finally
            {
                connect.Close();
            }
        }

        public int GetHRAAuditRCount(string yrmo)
        {
            int count = 0;
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            string cmdstr = "SELECT COUNT(*) FROM hra_AUDITR WHERE modifydt <= @date AND period= @qy";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                count = Convert.ToInt32(command.ExecuteScalar());
                
                return count;
            }
            finally
            {
                connect.Close();
            }
        }

        public int GetPutnamPartDataCount(string yrmo)
        {
            int count = 0;
            DateTime _date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'ptnm_partdata' " 
                                + "AND period = @qy "
                                + "AND termdt <= @date "
                                + "AND balance > 0 "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') ";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date",_date);
                count = Convert.ToInt32(command.ExecuteScalar());

                return count;
            }
            finally
            {
                connect.Close();
            }
        }

        public int GetWgwkInvCount(string yrmo)
        {
            int count = 0;
            string cmdstr = "SELECT COUNT(*) " 
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period = @yrmo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                count = Convert.ToInt32(command.ExecuteScalar());

                return count;
            }
            finally
            {
                connect.Close();
            }
        }

        public int GetPtnmPartDataCountHavingBal(string _qy)
        {
            int count = 0;
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND period = @qy "
                                + "AND balance > 0 "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') ";  
                               

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                count = Convert.ToInt32(command.ExecuteScalar());

                return count;
            }
            finally
            {
                connect.Close();
            }
        }

        public decimal GetWageworkHeadcountRate(string yrmo)
        {
            decimal rate = 0;
            string cmdstr = "SELECT rate FROM hra_rates "
                                + "WHERE yrmo = "
                                + "( "
                                + "SELECT MAX(yrmo) FROM hra_rates "
                                + "WHERE yrmo <= @yrmo AND type = 'Wageworks' "
                                + ") "
                                + "AND type = 'Wageworks'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result == null) || (result == DBNull.Value))
                    throw new Exception("Cannot find Wageworks Headcount Rate for YRMO - " + yrmo + " or less");
                rate = Decimal.Parse(result.ToString());

                return rate;
            }
            finally
            {
                connect.Close();
            }
        }

        public decimal GetPutnamHRARecordRate(string _qy)
        {
            HRA hobj = new HRA();
            decimal rate = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            string cmdstr = "SELECT rate FROM hra_rates " 
                                + "WHERE yrmo = " 
                                + "( "
                                + "SELECT MAX(yrmo) FROM hra_rates "
                                + "WHERE yrmo <= @yrmo AND type = 'Putnam' "
                                + ") "
                                + "AND type = 'Putnam'";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                object result = command.ExecuteScalar();
                if ((result == null) || (result == DBNull.Value))
                    throw new Exception("Cannot find Putnam HRA Record Rate for YRMO - " + yrmo + " or less");
                rate = Decimal.Parse(result.ToString());

                return rate;
            }
            finally
            {
                connect.Close();
            }
        }

        public void insertReconStatus(string _qy)
        {
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module) VALUES(@qy, 'Recon', 'AdminBill', 'HRA')";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetPutnamInvData(string _qy)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT vendor AS [vendor], "
                                + "period AS [period], "  
                                + "partcount AS [count], "  
                                + "amount AS [amount] "
                                + "FROM hra_PutnamInvoice "
                                + "WHERE quarteryear = @qy";
            
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                da.SelectCommand = command;
                da.Fill(ds);                
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetPartData_AuditR_Discp(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();   
            
            string cmdstr = "SELECT ssn AS [SSN], " 
                                + "lastname AS [Last Name], "  
                                + "firstname AS [First Name], "
                                + "CONVERT(VARCHAR(15),dob,101) AS [Birth Date], "
                                + "CONVERT(VARCHAR(15),termdt,101) AS [Termination Date], " 
                                + "balance AS [Total Asset Balance] "
                                + "FROM hra_PartDataInvoice "  
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND period = @qy "
                                + "AND termdt <= @date "
                                + "AND balance > 0 "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "  
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "  
	                                + "FROM hra_AUDITR "
                                    + "WHERE modifydt <= @date "
                                    + "AND period= @qy "
                                + ") "
                                 + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(dpnd_ssn, '-', '')))) "
                                    + "FROM Dependant "
                                    + "WHERE dpnd_owner = 1 AND dpnd_validated = 1 "
                                + ") " 
                                + "ORDER BY lastname, firstname";
            

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                da.SelectCommand = command;
                da.Fill(ds);
                
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetAuditR_PartData_Discp1(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
                       
            string cmdstr = "SELECT ssn AS [SSN], "
                                + "empno AS [EE#], "
                                + "substring(name,charindex(',', name)+1,len(name)) AS [Last Name], "
                                + "substring(name,1,charindex(',', name)-1) AS  [First Name], "
                                + "age AS [Age], "
                                + "CONVERT(VARCHAR(15),statusdt,101) AS [Status Date], "
                                + "CONVERT(VARCHAR(15),modifydt,101) AS [Modify Date] "
                                + "FROM hra_AUDITR "
                                + "WHERE modifydt <= @date "
                                + "AND period= @qy "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                    + "AND balance > 0 "
                                    + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "
                                + ") "
                                + "ORDER BY [Last Name], [First Name]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date",date);
                da.SelectCommand = command;
                da.Fill(ds);                

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetAuditR_PartData_Discp2(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            string cmdstr = "SELECT ssn AS [SSN], "
                                + "empno AS [EE#], "
                                + "substring(name,charindex(',', name)+1,len(name)) AS [Last Name], "
                                + "substring(name,1,charindex(',', name)-1) AS  [First Name], "
                                + "age AS [Age], "
                                + "CONVERT(VARCHAR(15),statusdt,101) AS [Status Date], "
                                + "CONVERT(VARCHAR(15),modifydt,101) AS [Modify Date] "
                                + "FROM hra_AUDITR "
                                + "WHERE modifydt > @date "
                                + "AND period= @qy "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                    + "AND balance > 0 "
                                    + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "
                                + ") "
                                + "ORDER BY [Last Name], [First Name]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                da.SelectCommand = command;
                da.Fill(ds);

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetPartData_WgwkInv_Discp(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            
            string cmdstr = "SELECT " 
                                + "lastname AS [Last Name], " 
                                + "firstname AS [First Name], "
                                + "ssn AS [SSN], "
                                + "partStatDesc AS [Participant Status Description], "
                                + "CONVERT(VARCHAR(15),dob,101) AS [Birth Date], "
                                + "CONVERT(VARCHAR(15),termdt,101) AS [Termination Date], " 
                                + "balance AS [Total Asset Balance] "
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND balance > 0 "
                                + "AND period = @qy "
                                + "AND termdt <= @date " 
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "  
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN " 
                                    + "( "
                                        + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) " 
                                        + "FROM hra_PartDataInvoice " 
                                        + "WHERE source = 'wgwk_invoice' " 
                                        + "AND period = @yrmo "
                                    + ") "
                                + "ORDER BY lastname, firstname";           

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
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

        public DataSet GetWgwkInv_PartData_Discp(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();   
            
            string cmdstr = "SELECT "
                                + "lastname AS [Last Name], "
                                + "firstname AS [First Name], "
                                + "ssn AS [SSN], "
                                + "NULL "
                                + "FROM hra_PartDataInvoice "                               
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                + ") "
                                + "UNION "
                                + "SELECT "
                                + "lastname AS [Last Name], "
                                + "firstname AS [First Name], "
                                + "ssn AS [SSN], "  
                                + "'Zero Balance' "
                                + "FROM hra_PartDataInvoice "                               
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                    + "AND balance = 0 "
                                + ") "
                                + "UNION "
                                + "SELECT "
                                + "lastname AS [Last Name], "
                                + "firstname AS [First Name], "
                                + "ssn AS [SSN], "
                                + "'Terminated, paid out' "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                    + "AND balance <> 0 "
                                    + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) = 'terminated, paid out') "  
                                + ") "
                                + "ORDER BY lastname, firstname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
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

        public DataSet GetWgwkInv_PartData_noBal(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            string cmdstr = "SELECT "  
                                + "lastname AS [Last Name], "   
                                + "firstname AS [First Name], "   
                                + "ssn AS [SSN], "   
                                + "0 AS [Total Asset Balance] " 
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'wgwk_invoice' "   
                                + "AND period = @yrmo "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) IN "  
                                + "( "
                                + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "   
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND period = @qy "
                                + "AND termdt <= @date "   
                                + "AND balance = 0 " 
                                + ") "
                                + "ORDER BY lastname, firstname";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
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

        public DataSet GetWgwkInv_PartData_PaidOut(string yrmo)
        {
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string _qy = HRA.GetQuarterYear(yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            string cmdstr = "SELECT "
                                + "lastname AS [Last Name], "
                                + "firstname AS [First Name], "
                                + "ssn AS [SSN], "
                                + "balance AS [Total Asset Balance] "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND balance > 0 "
                                + "AND period = @qy "
                                + "AND termdt <= @date "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) = 'terminated, paid out') "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) IN "
                                    + "( "
                                        + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                        + "FROM hra_PartDataInvoice "
                                        + "WHERE source = 'wgwk_invoice' "
                                        + "AND period = @yrmo "
                                    + ") "
                                + "ORDER BY lastname, firstname";
            

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
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

        public static void insertRates(string _yrmo, string _type, string _desc, decimal _rate)
        {
            string _cmdstr = "INSERT INTO hra_rates([yrmo], [type], [rate], [description]) VALUES(@yrmo, @type, @rate, @desc)";
            string _cmdstr1 = "SELECT [id] FROM hra_rates WHERE [yrmo] = @yrmo AND [type] = @type AND [rate] = @rate AND [description] = @desc";
            SqlTransaction ts;

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr, connect, ts);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@type", _type);
                command.Parameters.AddWithValue("@rate", _rate);
                command.Parameters.AddWithValue("@desc", _desc);
                command.ExecuteNonQuery();

                command = new SqlCommand(_cmdstr1, connect, ts);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@type", _type);
                command.Parameters.AddWithValue("@rate", _rate);
                command.Parameters.AddWithValue("@desc", _desc);
                int _hraId = Convert.ToInt32(command.ExecuteScalar());
                HttpContext.Current.Session["hraRateId"] = _hraId;
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                
                throw (new Exception("Error Inserting record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void insertSSN(string _ssn)
        {
            string _cmdstr = "INSERT INTO hra_excludeSSNAdmin([ssn]) VALUES(RIGHT('000000000'+REPLACE(@ssn,'-', ''), 9))";
            SqlTransaction ts;

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr, connect, ts);
                command.Parameters.AddWithValue("@ssn", _ssn);               
                command.ExecuteNonQuery();

                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();

                throw (new Exception("Error Inserting record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static string RateTypeDesc(string _type)
        {
            string _cmdstr = "SELECT [description] FROM hra_rates WHERE [type]= @type";
            string _desc = "";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@type", _type);
                _desc = command.ExecuteScalar().ToString();
                return _desc;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static Boolean AUDITRInserted(string _qy)
        {
            HRA hobj = new HRA();
            int count = 0;           
            string cmdstr = "SELECT COUNT(*) FROM [hra_AUDITR] WHERE [period] = @qy";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);                
                count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    if (Convert.ToInt32(hobj.GetMaxQuarterYRMO(_qy)) < 200804)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetAUDITR_Data(string _qy)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();            
            string cmdstr = "SELECT [ssn] "
                                + ",[empno] "
                                + ",[name] "
                                + ",CONVERT(VARCHAR(15),[dob],101) "
                                + ",[age] "
                                + ",[status] "
                                + ",CONVERT(VARCHAR(15),[statusdt],101) "
                                + ",CONVERT(VARCHAR(15),[modifydt],101) " 
                                + "FROM [hra_AUDITR] "
                                + "WHERE [period] = @qy";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                da.SelectCommand = command;
                da.Fill(ds);

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetPutnamParticipant_Data(string _qy)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            string cmdstr = "SELECT [ssn] "  
                                + ",[lastname] "
                                + ",[firstname] "  
                                + ",[partStatDesc] "
                                + ",CONVERT(VARCHAR(15),[dob],101) "
                                + ",CONVERT(VARCHAR(15),[termdt],101) "
                                + ",[balance] "
                                + "FROM [hra_PartDataInvoice] "
                                + "WHERE [period] = @qy";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                da.SelectCommand = command;
                da.Fill(ds);

                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        public static List<string> GetExcludeSSNs()
        {
            List<string> excludeSSNs = new List<string>();
            SqlDataReader dr = null;
            string cmdstr = "SELECT LTRIM(RTRIM(REPLACE([ssn], '-', ''))) FROM [hra_excludeSSNAdmin]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    excludeSSNs.Add(dr.GetString(0));
                }
                dr.Close();

                return excludeSSNs;
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

        public static Boolean hasAuditR_PartData_Discp(string _qy)
        {
            Boolean _result;
            HRA hobj = new HRA();
            int count = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_AUDITR " 
                                + "WHERE period= @qy "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN " 
                                + "( "
                                + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) " 
                                + "FROM hra_PartDataInvoice " 
                                + "WHERE source = 'ptnm_partdata' " 
                                + "AND period = @qy " 
                                + "AND termdt <= @date "
                                + "AND balance <> 0 "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "
                                + ") ";
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasPartData_AuditR_Discp(string _qy)
        {
            Boolean _result;
            HRA hobj = new HRA();
            int count = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_PartDataInvoice "   
                                + "WHERE source = 'ptnm_partdata' " 
                                + "AND period = @qy " 
                                + "AND termdt <= @date "
                                + "AND balance <> 0 "
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out') "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "   
                                + "( "
                                 + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "   
                                 + "FROM hra_AUDITR " 
                                 + "WHERE modifydt <= @date " 
                                 + "AND period= @qy " 
                                + ") "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN " 
                                + "( "
                                 + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(dpnd_ssn, '-', '')))) " 
                                 + "FROM Dependant " 
                                 + "WHERE dpnd_owner = 1 AND dpnd_validated = 1 "
                                + ")  ";
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasWgwkInv_PartData_Discp(string _qy)
        {
            Boolean _result;
            HRA hobj = new HRA();
            int count = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            string prevyrmo = hobj.getPrevYRMO(yrmo);
            string priyrmo = hobj.getPrevYRMO(prevyrmo);
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period IN (@yrmo, @prevyrmo, @priyrmo) "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                + "( "
                                    + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                    + "FROM hra_PartDataInvoice "
                                    + "WHERE source = 'ptnm_partdata' "
                                    + "AND period = @qy "
                                    + "AND termdt <= @date "
                                    + "AND balance <> 0 "
                                    + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out')"
                                + ") ";
                                

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@prevyrmo", prevyrmo);
                command.Parameters.AddWithValue("@priyrmo", priyrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasPartData_WgwkInv_Discp(string _qy)
        {
            Boolean _result;
            HRA hobj = new HRA();
            int count = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            string prevyrmo = hobj.getPrevYRMO(yrmo);
            string priyrmo = hobj.getPrevYRMO(prevyrmo);
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string cmdstr = "SELECT COUNT(*) "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND balance <> 0 "
                                + "AND period = @qy "
                                + "AND termdt <= @date "                               
                                + "AND (LOWER(RTRIM(LTRIM(partStatDesc))) <> 'terminated, paid out')"
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) NOT IN "
                                    + "( "
                                        + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                        + "FROM hra_PartDataInvoice "
                                        + "WHERE source = 'wgwk_invoice' "
                                        + "AND period IN (@yrmo, @prevyrmo, @priyrmo) "
                                    + ") ";
                                
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@prevyrmo", prevyrmo);
                command.Parameters.AddWithValue("@priyrmo", priyrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasWgwkInv_PartData_noBal(string _qy)
        {
            Boolean _result; 
            HRA hobj = new HRA();
            int count = 0;
            string yrmo = hobj.GetMaxQuarterYRMO(_qy);
            string prevyrmo = hobj.getPrevYRMO(yrmo);
            string priyrmo = hobj.getPrevYRMO(prevyrmo);
            DateTime date = HRA.GetLastDayofYRMO(yrmo);
            string cmdstr = "SELECT COUNT(DISTINCT ssn) "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'wgwk_invoice' "
                                + "AND period IN (@yrmo, @prevyrmo, @priyrmo) "
                                + "AND CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) IN "
                                + "( "
                                + "SELECT CONVERT(INT, LTRIM(RTRIM(REPLACE(ssn, '-', '')))) "
                                + "FROM hra_PartDataInvoice "
                                + "WHERE source = 'ptnm_partdata' "
                                + "AND period = @qy "
                                + "AND termdt <= @date "
                                + "AND (balance = 0 OR (LOWER(RTRIM(LTRIM(partStatDesc))) = 'terminated, paid out')) "
                                + ") ";
                                
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@qy", _qy);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@prevyrmo", prevyrmo);
                command.Parameters.AddWithValue("@priyrmo", priyrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasPutnamPartData(string _qy)
        {
            Boolean _result;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM [hra_PartDataInvoice] WHERE [period] = @qy", connect);
                command.Parameters.AddWithValue("@qy", _qy);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean hasAUDITR(string _qy)
        {
            Boolean _result;
            int count = 0;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("SELECT COUNT(*) FROM [hra_AUDITR] WHERE [period] = @qy", connect);
                command.Parameters.AddWithValue("@qy", _qy);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

    }
}
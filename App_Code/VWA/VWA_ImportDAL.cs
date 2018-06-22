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
using System.Data.SqlTypes;
using System.Globalization;

namespace EBA.Desktop.VWA
{
    public class VWAImportDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public VWAImportDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Boolean CheckAutoImport(string _category, string source)
        {
            string cmdStr = "SELECT [autoimport] FROM [FileMaintainence] WHERE (([module] = 'VWA') AND ([classification] = 'Import') AND ([category] = @category) AND ([sourcecd] = @source))";
            Boolean autoimport = false;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@source", source);
                autoimport = Convert.ToBoolean(command.ExecuteScalar());

                return autoimport;
            }
            finally
            {
                connect.Close();
            }
        }

        public static String GetClientFilePath(string _category, string source)
        {
            string cmdStr = "SELECT [filelocation] FROM [FileMaintainence] WHERE (([module] = 'VWA') AND ([classification] = 'Import') AND ([category] = @category) AND ([sourcecd] = @source))";
            string filepath = "";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@category", _category);
                command.Parameters.AddWithValue("@source", source);
                object result = command.ExecuteScalar();
                if ((result != null) && (result != DBNull.Value))
                {
                    filepath = result.ToString();
                }

                return filepath;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Boolean PastImport(string source, string period)
        {
            string cmdStr = "SELECT COUNT(*) FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='VWA'";
            int count = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
            finally
            {
                connect.Close();
            }
        }

        public static void DeleteRemit(string period)
        {
            string cmdStr1 = "DELETE FROM VWA_remit WHERE yrmo=@period";
            string cmdStr2 = "DELETE FROM ImportRecon_status WHERE period=@period AND source LIKE 'Remit%' AND type='Import' AND module='VWA'";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@period", period);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdStr2, connect);
                command.Parameters.AddWithValue("@period", period);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public static void Rollback(string source, string period)
        {
            string cmdStr1 = "";
            string cmdStr2 = "DELETE FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='VWA'";

            try
            {
                if (source.Contains("Remit"))
                {
                    cmdStr1 = "DELETE FROM VWA_remit WHERE yrmo = @period AND vendor=@source";
                }
                switch (source)
                {
                    case "BankStat":
                        cmdStr1 = "DELETE FROM vwa_bank WHERE yrmo = @period";
                        break;
                    case "TranDtl":
                        cmdStr1 = "DELETE FROM VWA_Trans1 WHERE yrmo = @period";
                        break;
                }

                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdStr2, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public static void insertImportStatus(string yrmo, string source)
        {
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module) VALUES(@yrmo, @source, 'Import', 'VWA')";

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

        public static void insertRemittance(string benName, string ssn, SqlDateTime accDt, int vwaCtrl, SqlDateTime dtPosted, Decimal paidVWA, Decimal paidClient, Decimal vwaFees, Decimal dueClient, string sts, string vendor, string yrmo)
        {
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_InsertRemit]", connect);
                command.Parameters.AddWithValue("@benName", benName);
                command.Parameters.AddWithValue("@ssn", ssn);
                command.Parameters.AddWithValue("@accidentDt", accDt);
                command.Parameters.AddWithValue("@vwaCtrl", vwaCtrl);
                command.Parameters.AddWithValue("@dtPosted", dtPosted);
                command.Parameters.AddWithValue("@pdVWA", paidVWA);
                command.Parameters.AddWithValue("@pdClient", paidClient);
                command.Parameters.AddWithValue("@VWAFees", vwaFees);
                command.Parameters.AddWithValue("@dueClient", dueClient);
                command.Parameters.AddWithValue("@sts", sts);
                command.Parameters.AddWithValue("@vendor", vendor);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }
        }
        
        public static void updateRemitWireDt(string wireDtStr, string yrmo)
        {
            SqlDateTime wireDt;            
            string cmdStr = "UPDATE VWA_remit "
                                + "SET [wiredt] = @wiredt "
                                + "WHERE yrmo = @yrmo";
            
            try
            {
                if (wireDtStr != null && wireDtStr != string.Empty) { wireDt = Convert.ToDateTime(wireDtStr); }
                else { wireDt = SqlDateTime.Null; }

                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@wiredt", wireDt);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetRemitDiscData(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();   

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "RemitDisp";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = String.Empty;
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetRemitDetailData(string yrmo, string clientcd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "RemitInput";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = clientcd;
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal GetClientVWAFees(string clientcd, string yrmo)
        {
            Decimal _result = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "ClientVWAFees";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = clientcd;
                if (command.ExecuteScalar() != DBNull.Value)
                    _result = Decimal.Parse(command.ExecuteScalar().ToString(), NumberStyles.Currency);

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal GetClientDueClient(string clientcd, string yrmo)
        {
            Decimal _result = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "ClientDueClient";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = clientcd;
                if (command.ExecuteScalar() != DBNull.Value)
                    _result = Decimal.Parse(command.ExecuteScalar().ToString(), NumberStyles.Currency);

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static Decimal GetClientPaidVWA(string clientcd, string yrmo)
        {
            Decimal _result = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "ClientPaidVWA";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = clientcd;
                if (command.ExecuteScalar() != DBNull.Value)
                    _result = Decimal.Parse(command.ExecuteScalar().ToString(), NumberStyles.Currency);

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static int storeBOAData(DataSet dsBOA_store)
        {
            int _counter = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                foreach (DataRow dr in dsBOA_store.Tables["sumTable"].Rows)
                {
                    command = new SqlCommand("[dbo].[sp_VWA_Bank]", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                    command.Parameters.AddWithValue("@PostDate", "");
                    command.Parameters.AddWithValue("@Type", dr["sType"].ToString());
                    command.Parameters.AddWithValue("@Amount", Convert.ToDecimal(dr["sAmnt"].ToString()));
                    command.Parameters.AddWithValue("@WireTo", "");
                    command.Parameters.AddWithValue("@RefNum", "");
                    command.Parameters.AddWithValue("@importType", "Summary");
                    command.ExecuteNonQuery();
                    _counter++;
                }
                foreach (DataRow dr in dsBOA_store.Tables["detTable"].Rows)
                {
                    command = new SqlCommand("[dbo].[sp_VWA_Bank]", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                    command.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(dr["PostDate"].ToString()));
                    command.Parameters.AddWithValue("@Type", dr["Type"].ToString());
                    command.Parameters.AddWithValue("@Amount", Convert.ToDecimal(dr["Amount"].ToString()));
                    command.Parameters.AddWithValue("@WireTo", dr["WireTo"].ToString());
                    command.Parameters.AddWithValue("@RefNum", dr["ReferenceNo"].ToString());
                    command.Parameters.AddWithValue("@importType", "Detail");
                    command.ExecuteNonQuery();
                    _counter++;
                }
                return _counter;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error loading Bank statement data"));
            }
            finally
            {
                connect.Close();
            }
        }

        public static int storeVWATransData(DataSet dsVWA_store)
        {
            int _counter = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                foreach (DataRow dr in dsVWA_store.Tables[0].Rows)
                {
                    command = new SqlCommand("[dbo].[sp_VWA_Trans1_Import]", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                    command.Parameters.AddWithValue("@ClientID", dr["ClientID"].ToString());
                    command.Parameters.AddWithValue("@ContractNo", Convert.ToInt32(dr["ContractNo"].ToString()));
                    command.Parameters.AddWithValue("@Lname", dr["Lname"].ToString());
                    command.Parameters.AddWithValue("@Fname", dr["Fname"].ToString());
                    command.Parameters.AddWithValue("@SSNO", dr["SSNO"].ToString());
                    command.Parameters.AddWithValue("@GroupNo", Convert.ToInt32(dr["GroupNo"].ToString()));
                    command.Parameters.AddWithValue("@DisabCd", dr["DisabCd"].ToString());
                    command.Parameters.AddWithValue("@PatientName", dr["PatientName"].ToString());
                    command.Parameters.AddWithValue("@RelCd", dr["RelCd"].ToString());
                    command.Parameters.AddWithValue("@AccDt", dr["AccDt"].ToString().Equals("") ? DBNull.Value : dr["AccDt"]);
                    command.Parameters.AddWithValue("@OpenDt", dr["OpenDt"].ToString().Equals("") ? DBNull.Value : dr["OpenDt"]);
                    command.Parameters.AddWithValue("@CloseDt", dr["CloseDt"].ToString().Equals("") ? DBNull.Value : dr["CloseDt"]);
                    command.Parameters.AddWithValue("@RecDt", dr["RecDt"].ToString().Equals("") ? DBNull.Value : dr["RecDt"]);
                    command.Parameters.AddWithValue("@StatusCd", dr["StatusCd"].ToString());
                    command.Parameters.AddWithValue("@BenPaid", Decimal.Parse(dr["BenPaid"].ToString(), System.Globalization.NumberStyles.Currency));
                    command.Parameters.AddWithValue("@RecAmt", Decimal.Parse(dr["RecAmt"].ToString(), System.Globalization.NumberStyles.Currency));
                    command.Parameters.AddWithValue("@TotFees", Decimal.Parse(dr["TotFees"].ToString(), System.Globalization.NumberStyles.Currency));
                    command.Parameters.AddWithValue("@NetAmt", Decimal.Parse(dr["NetAmt"].ToString(), System.Globalization.NumberStyles.Currency));
                    command.Parameters.AddWithValue("@SysDt", dr["SysDt"].ToString().Equals("") ? DBNull.Value : dr["SysDt"]);

                    command.ExecuteNonQuery();
                    _counter++;
                }

                return _counter;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error loading VWA Monthly Transaction File data"));
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet getBOAData(string _yrmo)
        {
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_bankResults]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@source", "Summary");
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Bank statement data"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }

        public static DataTable getBOADepDet(string _yrmo)
        {
            DataTable dsbresult = new DataTable("DepDet");
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_bankResults]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@source", "Deposits");
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Bank statement data"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }

        public static DataTable getBOAWDDet(string _yrmo)
        {
            DataTable dsbresult = new DataTable("WDDet");
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_bankResults]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@source", "Withdraws");
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Bank statement data"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
    }
}
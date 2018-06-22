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
using System.Collections.Generic;

namespace EBA.Desktop.VWA
{
    /// <summary>
    /// Summary description for VWA_DAL
    /// </summary>
    public class VWA_DAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public VWA_DAL()
        {

        }

        public static DataTable getContractDetails(string _cnum)
        {
            DataTable dtCinfo = new DataTable();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_ContractDetails]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@source","ContractInfo");
                command.Parameters.AddWithValue("@cnum", _cnum);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtCinfo);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting contract details"));
            }
            finally
            {
                connect.Close();
            }
            return dtCinfo;
        }

        public static DataTable getContractTabDetails(string _cnum)
        {
            DataTable dtCinfo = new DataTable("Contract");
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_ContractDetails]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@source", "ContractTab");
                command.Parameters.AddWithValue("@cnum", _cnum);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtCinfo);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting contract details"));
            }
            finally
            {
                connect.Close();
            }
            return dtCinfo;
        }

        public static DataTable getFinanceTab1Details(string _cnum)
        {
            DataTable dtCinfo = new DataTable("Financial1");
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_ContractDetails]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@source", "FinanceTab1");
                command.Parameters.AddWithValue("@cnum", _cnum);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtCinfo);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting contract details"));
            }
            finally
            {
                connect.Close();
            }
            return dtCinfo;
        }

        public static DataTable getFinanceTab2Details(string _cnum)
        {
            DataTable dtCinfo = new DataTable("Financial2");
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_VWA_ContractDetails]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@source", "FinanceTab2");
                command.Parameters.AddWithValue("@cnum", _cnum);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtCinfo);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting contract details"));
            }
            finally
            {
                connect.Close();
            }
            return dtCinfo;
        }   

        public static DataTable getRemitData_4Balancing(string yrmo)
        {
            DataTable dtremit = new DataTable();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "RemitSum";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = string.Empty;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtremit);

                return dtremit;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Remittance data"));
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataTable getTransactionData_4Balancing(string yrmo)
        {
            DataTable dttrans = new DataTable();
            string priyrmo = VWA.getPrevYRMO(yrmo);

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Trans_Output]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "TransSum";
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@priyrmo", SqlDbType.VarChar).Value = priyrmo;
                command.Parameters.Add("@agedDays", SqlDbType.Int).Value = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dttrans);

                return dttrans;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Transaction details"));
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataTable getAdjustmentData_4Balancing(string yrmo)
        {
            DataTable dtadj = new DataTable();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Remit]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = "AdjSum";
                command.Parameters.Add("@vendorcd", SqlDbType.VarChar).Value = string.Empty;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(dtadj);

                return dtadj;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Adjustment data"));
            }
            finally
            {
                connect.Close();
            }
        }

        public static void insertCaseNotes(string _notes, string _yrmo, string _contractnum)
        {
            string _cmdstr = "UPDATE VWA_Trans1 SET Notes = @notes,NotesDt = @date ,NotesUser = @user "                               
                                + " WHERE yrmo = @yrmo AND ContractNo = @cnum ";
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@notes", _notes);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@user", HttpContext.Current.User.Identity.Name);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@cnum", _contractnum);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (new Exception("Error storing notes."));
            }
            finally
            {
                connect.Close();
            }

        }

        public static DataTable employeeData(string _lname, string _fname)
        {
            string _cmdstr = "";
            DataTable _dtEmp = new DataTable();
            if (_fname.Equals(""))
            {
                _cmdstr =
                    "SELECT [ContractNo],[Lname], [Fname], [ClientId],[GroupNo] FROM [VWA_Trans1] "
                     + " WHERE [Lname] LIKE '" + _lname + "'"
                     + " AND yrmo = (SELECT MAX(b.yrmo) FROM VWA_Trans1 b WHERE b.ContractNo = ContractNo) ORDER BY [Fname] ASC";

            }
            else if (_lname.Equals(""))
            {
                _cmdstr =
                    "SELECT [ContractNo],[Lname], [Fname], [ClientId],[GroupNo] FROM [VWA_Trans1] "
                     + " WHERE [Fname] LIKE '" + _fname + "' " 
                     + " AND yrmo = (SELECT MAX(b.yrmo) FROM VWA_Trans1 b WHERE b.ContractNo = ContractNo) ORDER BY [Lname] ASC";
            }
            else
            {
                _cmdstr =
                    "SELECT [ContractNo],[Lname], [Fname], [ClientId],[GroupNo] FROM [VWA_Trans1] "
                     + " WHERE [Fname] = '" + _fname + "' AND [Lname] = '" + _lname + "' "
                     + "  AND yrmo = (SELECT MAX(b.yrmo) FROM VWA_Trans1 b WHERE b.ContractNo = ContractNo) ORDER BY [Fname] ASC, [Lname] ASC";
            }
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(_dtEmp);

                return _dtEmp;
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Employee data."));
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataTable getHistory_4Cases(string _contract)
        {
            DataTable dtHistory = new DataTable();
            DataTable dtTemp = new DataTable();
            List<string> _yrmos = new List<string>();
            SqlDataReader reader = null;
            string _cmdstr = "SELECT DISTINCT yrmo FROM VWA_Trans1 WHERE ContractNo = @contract ORDER BY yrmo DESC";
            string _yrmo;
            string _prevyrmo = "";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                //Get All yrmos for given contract
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@contract", _contract);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _yrmos.Add(reader[0].ToString());
                }
                reader.Close();
                foreach (string _y in _yrmos)
                {
                    _yrmo = _y;
                    _prevyrmo = VWA.getPrevYRMO(_yrmo);
                    command = new SqlCommand("[dbo].[sp_VWA_GetHistory]", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@yrmo", _yrmo);
                    command.Parameters.AddWithValue("@pyrmo", _prevyrmo);
                    command.Parameters.AddWithValue("@contract", _contract);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dtTemp);
                    dtHistory.Merge(dtTemp); dtTemp.Clear();
                }

                dtHistory.TableName = "History";
                return dtHistory;
            }
            catch (Exception ex)
            {                
                throw (new Exception("Error getting History data"));
            }
            finally
            {
                reader.Close();
                connect.Close();
            }
        }
    }
}

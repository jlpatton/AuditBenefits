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
using EBA.Desktop.Audit;

namespace EBA.Desktop.HRA
{
    /// <summary>
    /// Summary description for HRAdata
    /// </summary>
    public class HRAdata
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public HRAdata()
        {            
        }

        public static DataSet empTransactions(string _empssn)
        {
            DataSet dsFinal = new DataSet(); dsFinal.Clear();
            DataSet dsPtn = new DataSet(); dsPtn.Clear();
            DataSet dsWgwk = new DataSet(); dsWgwk.Clear();
            DataSet dsPtnAdj = new DataSet(); dsPtnAdj.Clear();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_EMPTransactions", connect);
                command.CommandType = CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@ssn", _empssn);
                command.Parameters.AddWithValue("@src", "PTN");
                da.SelectCommand = command;
                da.Fill(dsPtn);
                command.Dispose();

                command = new SqlCommand("sp_HRA_EMPTransactions", connect);
                command.CommandType = CommandType.StoredProcedure;
                //7-6-2009 Convert SSN to Int to remove leading zero
                //_empssn = Convert.ToInt64(_empssn).ToString();
                command.Parameters.AddWithValue("@ssn", _empssn);
                command.Parameters.AddWithValue("@src", "PTNADJ");
                da.SelectCommand = command;
                da.Fill(dsPtnAdj);
                command.Dispose();

                command = new SqlCommand("sp_HRA_EMPTransactions", connect);
                command.CommandType = CommandType.StoredProcedure;
                _empssn = Convert.ToInt64(_empssn).ToString();
                //need to convert ssn to int to remove the preceeding zeroes for the Wageworks table 
                ///R.A. 3/20/2009
                command.Parameters.AddWithValue("@ssn", _empssn);
                command.Parameters.AddWithValue("@src", "WGK");
                da.SelectCommand = command;
                da.Fill(dsWgwk);
                command.Dispose(); 
               
                dsFinal.Merge(dsPtn); dsPtn.Clear(); 
                dsFinal.Merge(dsPtnAdj); dsPtnAdj.Clear();
                dsFinal.Merge(dsWgwk); dsWgwk.Clear();
                
                return dsFinal;
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

        public static DataSet empDependants(int _empno)
        {
            DataSet dsEFinal = new DataSet(); dsEFinal.Clear();            
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRAgetDeps4ValidationsRecords", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empno", _empno);
                command.Parameters.AddWithValue("@dssn", "");
                da.SelectCommand = command;
                da.Fill(dsEFinal);
                command.Dispose();  
              
                return dsEFinal;
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

        public static DataSet empDependants(int _empno,string _dssn)
        {
            DataSet dsEFinal = new DataSet(); dsEFinal.Clear();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRAgetDeps4ValidationsRecords", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@empno", _empno);
                command.Parameters.AddWithValue("@dssn", _dssn);
                da.SelectCommand = command;
                da.Fill(dsEFinal);
                command.Dispose();

                return dsEFinal;
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

        public static void insertValidationRecord(int _empno, string _dssn, string _rel)
        {
            try
            {
                if (!checkExistsVRecord(_empno, _dssn, _rel))
                {
                    if (connect == null || connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    command = new SqlCommand("sp_HRAinsertLetterValidationsRecords", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@empno", _empno);
                    command.Parameters.AddWithValue("@dssn", _dssn);
                    command.Parameters.AddWithValue("@rel", _rel);
                    command.ExecuteNonQuery();
                }
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

        public static void insertConfirmationRecord(int _empno)
        {
            try
            {
                if (!checkExistsCRecord(_empno))
                {
                    if (connect == null || connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    command = new SqlCommand("sp_HRAinsertLetterConfirmationRecords", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@empno", _empno);
                    command.ExecuteNonQuery();
                }
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

        private static bool checkExistsVRecord(int _empno, string _dssn, string _rel)
        {
            bool _exists = false;
            string _cmdstr = "SELECT COUNT(*) FROM hra_Ltrs_Pending "
                                + " WHERE pnEmpNum = @empno AND pnDepSSN = @dssn AND pnDepRelationship = @rel AND pnLtrType = 'val'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);                
                command.Parameters.AddWithValue("@empno", _empno);
                command.Parameters.AddWithValue("@dssn", _dssn);
                command.Parameters.AddWithValue("@rel", _rel);
                object _value = command.ExecuteScalar();
                if (!_value.Equals(DBNull.Value) && Convert.ToInt32(_value) > 0)
                {
                    _exists = true;
                }
                return _exists;
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

        private static bool checkExistsCRecord(int _empno)
        {
            bool _exists = false;
            string _cmdstr = "SELECT COUNT(*) FROM hra_Ltrs_Pending "
                                + " WHERE pnEmpNum = @empno AND pnLtrType = 'conf'";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@empno", _empno);                
                object _value = command.ExecuteScalar();
                if (!_value.Equals(DBNull.Value) && Convert.ToInt32(_value) > 0)
                {
                    _exists = true;
                }
                return _exists;
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

        public static Decimal getPutnamHRAAmount(string _empno)
        {
            //Handle scramle
            string _cmdstr0 = "SELECT empl_ssn FROM Employee WHERE empl_empno = @empno ";
            string _essn = "";
            string _cmdstr = "SELECT balance "
                                + " FROM hra_PartDataInvoice "
                                + " WHERE source = 'ptnm_partdata' "
                                + " AND ssn = @ssn "
                                + " AND (SUBSTRING(period, 4, 4) + SUBSTRING(period, 2, 1)) = "
                                + " (Select MAX(SUBSTRING(period, 4, 4) + SUBSTRING(period, 2, 1)) FROM hra_PartDataInvoice WHERE source = 'ptnm_partdata' )";
            decimal _amt = 0;
            try
            {                
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr0, connect);
                command.Parameters.AddWithValue("@empno", _empno);
                object _value = command.ExecuteScalar();
                if ((_value != DBNull.Value) || (_value != null))
                {
                    _essn = _value.ToString();
                }
                command.Dispose();
                command = new SqlCommand(_cmdstr, connect); 
                command.Parameters.AddWithValue("@ssn", _essn);               
                object _value1 = command.ExecuteScalar();
                if ((_value1 != DBNull.Value) || (_value1 != null))
                {
                    _amt = Convert.ToDecimal(_value1);
                }                
                return _amt;
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

        public static void insertEmployeeAudit(string _primaryKey, string _colName, string _oldV, string _newV)
        {           
            try
            {                
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_EMPAudit", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pkey", _primaryKey);
                command.Parameters.AddWithValue("@colN", _colName);
                command.Parameters.AddWithValue("@oldV", _oldV);
                command.Parameters.AddWithValue("@newV", _newV);
                command.Parameters.AddWithValue("@user", HttpContext.Current.Session["userName"].ToString());
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

        public static DataSet getEmployeeAuditInsert(string _dt1, string _dt2)
        {
            DataSet dsIn = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {                
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_auditEmpreport", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@date1", _dt1);
                command.Parameters.AddWithValue("@date2", _dt2);
                command.Parameters.AddWithValue("@type", "INS");
                da.SelectCommand = command;
                da.Fill(dsIn);

                return dsIn;
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

        public static DataSet getEmployeeAuditUpdate(string _dt1, string _dt2)
        {
            DataSet dsUp = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_auditEmpreport", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@date1", _dt1);
                command.Parameters.AddWithValue("@date2", _dt2);
                command.Parameters.AddWithValue("@type", "UPD");
                da.SelectCommand = command;
                da.Fill(dsUp);

                return dsUp;
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

        public static DataSet getEmployeeAuditException(string _dt1, string _dt2)
        {
            DataSet dsEx = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("sp_HRA_auditEmpreport", connect);
                command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.AddWithValue("@date1", _dt1);                
                command.Parameters.AddWithValue("@date2", _dt2);
                command.Parameters.AddWithValue("@type", "EXC");
                da.SelectCommand = command;
                da.Fill(dsEx);

                return dsEx;
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
        
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;


namespace EBA.Desktop.HRA
{
    /// <summary>
    /// Summary description for QueryBuilder
    /// </summary>
    public class QueryBuilder
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection conn = new SqlConnection(connStr);
        static SqlCommand command = null;

        public QueryBuilder()
        {            
        }

        /// <summary>
        /// Creates a general SQL query string for given columns, tables.
        /// </summary>
        /// <param name="_tcList">List of Columns and tables of type ColumnsAndTables</param>
        /// <returns>SQL query string</returns>
        public static string GenerateSqlQuery(List<ColumnsAndTables> _tcList)
        {
            string _strSql = string.Empty;
            DataTable _IJTable = new DataTable();
            DataTable _CJTable = new DataTable();

            _strSql = "SELECT  " + CreateSelectString(_tcList)
                        + Environment.NewLine + " FROM Employee" + " ";

            List<string> _tablesList = new List<string>();
            foreach (ColumnsAndTables ct in _tcList)
            {
                _tablesList.Add(ct.Tbls);
            }

            string _tablesStr = CreateTableString(_tablesList);

            _IJTable = GetInnerJoin(_tablesStr);

            //Build the Inner Join 
            if (_IJTable.Rows.Count > 0)
            {
                for (int i = 0; i < _IJTable.Rows.Count; i++)
                {
                    string _foreignTbl = _IJTable.Rows[i][0].ToString();
                    string _primaryTbl = _IJTable.Rows[i][1].ToString();
                    string _foreignID = _IJTable.Rows[i][2].ToString();
                    string _primaryID = _IJTable.Rows[i][3].ToString();
                    _strSql += " INNER JOIN " + Environment.NewLine + "[" + _foreignTbl + "]" + " ON " + "[" + _foreignTbl + "]." + _foreignID + " = " + "[" + _primaryTbl + "]." + _primaryID;
                }
            } 

            return _strSql;
        }

        /// <summary>
        /// Creates SQL query for Ownership type letters
        /// </summary>
        /// <param name="_tcList">List of Columns and tables of type ColumnsAndTables</param>
        /// <returns>SQL query string</returns>
        public static string GenerateOwnershipSqlQuery(List<ColumnsAndTables> _tcList)
        {
            string _strSql = string.Empty;
            DataTable _IJTable = new DataTable();
            DataTable _CJTable = new DataTable();

            _strSql = "SELECT  " + CreateSelectString(_tcList)
                        + Environment.NewLine + " FROM Employee" + " ";

            List<string> _tablesList = new List<string>();
            foreach (ColumnsAndTables ct in _tcList)
            {
                _tablesList.Add(ct.Tbls);
            }

            string _tablesStr = CreateTableString(_tablesList);

            _IJTable = GetInnerJoin(_tablesStr);            
            
            //Build the Inner Join 
            if (_IJTable.Rows.Count > 0)
            {
                for (int i = 0; i < _IJTable.Rows.Count; i++)
                {
                    string _foreignTbl = _IJTable.Rows[i][0].ToString();
                    string _primaryTbl = _IJTable.Rows[i][1].ToString();
                    string _foreignID = _IJTable.Rows[i][2].ToString();
                    string _primaryID = _IJTable.Rows[i][3].ToString();
                    _strSql += " INNER JOIN " + Environment.NewLine + "[" + _foreignTbl + "]" + " ON " + "[" + _foreignTbl + "]." + _foreignID + " = " + "[" + _primaryTbl + "]." + _primaryID;
                }                    
            }
            
            //add this string to get address of dependents based on address type
            StringBuilder sb = new StringBuilder(_strSql);
            sb.Append(" WHERE Address.addr_type <> '004' ");
            sb.Append(" AND Dependant.dpnd_add_diff = 0 ");
            sb.Append(" UNION ");
            sb.Append(_strSql);
            sb.Append(" AND Address.addr_dpnd_ssn = Dependant.dpnd_ssn ");
            sb.Append(" AND Dependant.dpnd_add_diff = 0 ");

            return sb.ToString();           
        }

        /// <summary>
        /// Creates SQL query for Confirmation letters
        /// </summary>
        /// <param name="_tcList">List of Columns and tables of type ColumnsAndTables</param>
        /// <returns>SQL query string</returns>
        public static string GenerateConfirmationSqlQuery(List<ColumnsAndTables> _tcList)
        {
            string _strSql = string.Empty;
            DataTable _IJTable = new DataTable();
            DataTable _CJTable = new DataTable();

            string _selectStr = CreateSelectString(_tcList);

            // Get top 3 dependants
            StringBuilder sb = new StringBuilder(_selectStr);
            sb.Append(" , MAX(CASE [dpnd_order] WHEN 1 THEN Dependant.dpnd_fname + ' ' + Dependant.dpnd_lname END) Dependent1, ");
            sb.Append(" MAX(CASE [dpnd_order] WHEN 2 THEN Dependant.dpnd_fname + ' ' + Dependant.dpnd_lname END) Dependent2, ");
            sb.Append(" MAX(CASE [dpnd_order] WHEN 3 THEN Dependant.dpnd_fname + ' ' + Dependant.dpnd_lname END) Dependent3 ");

            _strSql = "SELECT  " + sb.ToString()
                        + Environment.NewLine + " FROM Employee" + " ";

            List<string> _tablesList = new List<string>();
            foreach (ColumnsAndTables ct in _tcList)
            {
                _tablesList.Add(ct.Tbls);
            }

            string _tablesStr = CreateTableString(_tablesList);

            _IJTable = GetInnerJoin(_tablesStr);

            //Build the Inner Join 
            if (_IJTable.Rows.Count > 0)
            {
                for (int i = 0; i < _IJTable.Rows.Count; i++)
                {
                    string _foreignTbl = _IJTable.Rows[i][0].ToString();
                    string _primaryTbl = _IJTable.Rows[i][1].ToString();
                    string _foreignID = _IJTable.Rows[i][2].ToString();
                    string _primaryID = _IJTable.Rows[i][3].ToString();
                    _strSql += " INNER JOIN " + Environment.NewLine + "[" + _foreignTbl + "]" + " ON " + "[" + _foreignTbl + "]." + _foreignID + " = " + "[" + _primaryTbl + "]." + _primaryID;
                }
            }

            //add dependant table to sql string if it does not exists
            if (!_strSql.Contains("INNER JOIN [Dependant] ON [Employee].empl_empno = [Dependant].dpnd_empno"))
            {
                _strSql = _strSql + "INNER JOIN [Dependant] ON [Employee].empl_empno = [Dependant].dpnd_empno";
            }
            _strSql = _strSql + " GROUP BY " + _selectStr;
            return _strSql;
        }

        private static string CreateTableString(List<string> _tblStr)
        {
            string _finalTbl = "";
            foreach (string _tb in _tblStr)
            {                
                _finalTbl += "'" + _tb + "',";
            }

            _finalTbl = _finalTbl.Remove(_finalTbl.Length - 1);
            return _finalTbl;
        }

        private static string CreateSelectString(List<ColumnsAndTables> _coltables)
        {
            string _var = "";
            bool _fnd = false;
            foreach (ColumnsAndTables ct1 in _coltables)
            {
                _fnd = false;
                switch (ct1.Schm)
                {
                    //case "Dependent1":
                    //    _var += ct1.Tbls + "." + "dpnd_fname" + " + " + ct1.Tbls + "." + "dpnd_fname AS [" + ct1.Schm + "],";
                    //    _fnd = true;
                    //    break;
                    //case "Dependent2":
                    //    _var += ct1.Tbls + "." + "dpnd_fname" + " + " + ct1.Tbls + "." + "dpnd_fname AS [" + ct1.Schm + "],";
                    //    _fnd = true;
                    //    break;
                    //case "Dependent3":
                    //    _var += ct1.Tbls + "." + "dpnd_fname" + " + " + ct1.Tbls + "." + "dpnd_fname AS [" + ct1.Schm + "],";
                    //    _fnd = true;
                    //    break;
                    case "Dependent_Name":
                        _var += ct1.Tbls + "." + "dpnd_fname" + " + " + ct1.Tbls + "." + "dpnd_fname AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "Pilot_Name":
                        _var += ct1.Tbls + "." + "empl_fname" + " + " + ct1.Tbls + "." + "empl_minit " + " + " + ct1.Tbls + "." + "empl_lname AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "Street_Address":
                        _var += ct1.Tbls + "." + "addr_addr1" + " + " + ct1.Tbls + "." + "addr_addr2 AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "City_State_Zip":
                        _var += ct1.Tbls + "." + "addr_city" + " + " + ct1.Tbls + "." + "addr_state " + " + " + ct1.Tbls + "." + "addr_zip AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "father_mother":
                        _var += " CASE " 
                                    +  " WHEN " +ct1.Tbls + "." + "empl_sex = 'M' THEN 'father' "
                                    +  " WHEN " +ct1.Tbls + "." + "empl_sex = 'F' THEN 'mother' "
                                + " END"
                                +  " AS [" + ct1.Schm + "],";
                        break;
                    case "fathers_mothers":
                        _var += " CASE "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'M' THEN 'father''s' "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'F' THEN 'mother''s' "
                                + " END"
                                + " AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "he_she":
                        _var += " CASE "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'M' THEN 'he' "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'F' THEN 'she' "
                                + " END"
                                + " AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                    case "his_her":
                        _var += " CASE "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'M' THEN 'his' "
                                    + " WHEN " + ct1.Tbls + "." + "empl_sex = 'F' THEN 'her' "
                                + " END"
                                + " AS [" + ct1.Schm + "],";
                        _fnd = true;
                        break;
                }

                if (_fnd)
                {
                    continue;
                }
                
                _var += ct1.Tbls + "." + ct1.Cols + " AS [" + ct1.Schm + "],";
            }

            //if (!_var.Contains("Employee.empl_empno AS [EmployeeNo],"))
            //{
            //    _var += "Employee.empl_empno AS [EmployeeNo],";
            //}
            //if (!_var.Contains("Employee.empl_fname AS [FirstName],"))
            //{
            //    _var += "Employee.empl_fname AS [FirstName],";
            //}
            //if (!_var.Contains("Employee.empl_lname AS [LastName],"))
            //{
            //    _var += "Employee.empl_lname AS [LastName],";
            //}
            //if (!_var.Contains("Employee.empl_ssn AS [EmployeeSSN],"))
            //{
            //    _var += "Employee.empl_ssn AS [EmployeeSSN],";
            //}
            //if (!_var.Contains("Address.addr_addr1 AS [Address1],"))
            //{
            //    _var += "Address.addr_addr1 AS [Address1],";
            //}
            //if (!_var.Contains("Address.addr_addr2 AS [Address2],"))
            //{
            //    _var += "Address.addr_addr2 AS [Address2],";
            //}
            //if (!_var.Contains("Address.addr_city AS [City],"))
            //{
            //    _var += "Address.addr_city AS [City],";
            //}
            //if (!_var.Contains("Address.addr_state AS [State],"))
            //{
            //    _var += "Address.addr_state AS [State],";
            //}
            //if (!_var.Contains("Address.addr_zip AS [Zip],"))
            //{
            //    _var += "Address.addr_zip AS [Zip],";
            //}          

            _var = _var.Remove(_var.Length - 1);
            return _var;
        }

        private static DataTable GetInnerJoin(string _tablesString)
        {
            try
            {
                StringBuilder sqlString = new StringBuilder();
                sqlString.Append("SELECT sof.name AS fTableName, sor.name AS rTableName, scf.name AS fColName, scr.name AS rColName ");
                sqlString.Append("FROM dbo.sysforeignkeys sfk INNER JOIN ");
                sqlString.Append("dbo.sysobjects sof ON sfk.fkeyid = sof.id INNER JOIN ");
                sqlString.Append("dbo.sysobjects sor ON sfk.rkeyid = sor.id INNER JOIN ");
                sqlString.Append("dbo.syscolumns scf ON sfk.fkey = scf.colid AND sof.id = scf.id INNER JOIN ");
                sqlString.Append("dbo.syscolumns scr ON sfk.rkey = scr.colid AND sor.id = scr.id ");
                sqlString.Append("AND sor.name in (" + _tablesString + ") ");
                sqlString.Append("AND sof.name in (" + _tablesString + ") ");
                sqlString.Append("AND sor.name <> sof.name");

                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(sqlString.ToString(), conn);
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable _ijTable = new DataTable();
                da.SelectCommand = command;
                da.Fill(_ijTable);

                return _ijTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        private static DataTable getColumnName(string _tableName)
        {
            try
            {
                String sqlString = String.Empty;
                sqlString = "SELECT DISTINCT column_name FROM information_schema.columns WHERE table_name = '" + _tableName + "'";
                command = new SqlCommand(sqlString.ToString(), conn);
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable _colTable = new DataTable();
                da.SelectCommand = command;
                da.Fill(_colTable);

                return _colTable;
            }
            catch (Exception e)
            {
                throw e;
            }
        }       

        public struct ColumnsAndTables
        {
            private string _cols;
            private string _tbls;
            private string _schemas;

            public string Schm
            {
                get
                {
                    return _schemas;
                }
                set
                {
                    _schemas = value;
                }
            }
            public string Cols
            {
                get
                {
                    return _cols;
                }
                set
                {
                    _cols = value;
                }
            }
            public string Tbls
            {
                get
                {
                    return _tbls;
                }
                set
                {
                    _tbls = value;
                }
            }
        }       
    }
}

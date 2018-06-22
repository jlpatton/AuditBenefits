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
using System.Collections.Generic;

/// <summary>
/// Summary description for DsktpData
/// </summary>
namespace EBA.Desktop.HRA
{
    public class DsktpData
    {
        private string connStr = null;
        private SqlConnection conn;

        public DsktpData()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }


        #region SpDependent Data

        /// <summary>
        /// Checks whether Employee with given Employee number exists.
        /// </summary>
        /// <param name="empno">Employee number</param>
        /// <returns>True or False</returns>
        public bool checkExistRecord(int empno)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_spdep WHERE xrefempno = @empno";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }

        /// <summary>
        /// Checks whether Employee with given Employee SSN exists.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>True or False</returns>
        public bool checkExistRecord(string ssn)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_spdep WHERE ssno = @ssn";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }

        /// <summary>
        /// Search Sponsored Dependents record with xref Empno 
        /// </summary>
        /// <param name="lnameFirstInitial">xrefempno</param>
        /// <returns>DataSet</returns>
        public DataSet searchSpDeps(int empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT * From Dsktp_spdep WHERE xrefempno = @empno";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@empno", empno);
            
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Search Sponsored Dependents record with given Last name 
        /// </summary>
        /// <param name="lnameFirstInitial">Last name first initial</param>
        /// <returns>DataSet</returns>
        public DataSet searchSpDeps(string lname)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT * From Dsktp_spdep WHERE lname LIKE '" + lname + "%'";
            cmd = new SqlCommand(cmdStr, conn);
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Search Sponsored Dependents record with given Last name and First name
        /// </summary>
        /// <param name="fname">First Name</param>
        /// <param name="lname">Last Name</param>
        /// <returns>DataSet</returns>
        public DataSet searchSpDeps(string fname, string lname)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT * From Dsktp_spdep WHERE lname = @lname AND fname = @fname";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@lname", lname);
            cmd.Parameters.AddWithValue("@fname", fname);
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Searches for Sponsored Dependents records from database by fname, lname or empno.
        /// </summary>
        /// <param name="ssn">LastName, FirstName, XrefEmpno</param>
        /// <returns>DataSet</returns>
        public DataSet searchSpDeps(string ln, string fn, string empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); 
            string _cmdstr = " SELECT ssno, lname, fname, xrefempno "
                                + "FROM Dsktp_spdep WHERE ";
            string _addl = string.IsNullOrEmpty(empno) ? (string.IsNullOrEmpty(ln) ?
                (string.IsNullOrEmpty(fn) ? "" : "fname LIKE '" + fn + "%'") :
                ("lname LIKE '" + ln + "%'" + (string.IsNullOrEmpty(fn) ? "" :
                " and fname LIKE '" + fn + "%'"))) : "xrefempno = " + empno;
            _cmdstr = _cmdstr + _addl;

            

            //SpDepSearchRecord SpDRec = new SpDepSearchRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);

            //SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = command;
                da.Fill(ds);
                //reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    SpDRec = assignSpDepData(reader);
                //}
                //reader.Close();
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Get Sponsored Dependents Notes by Ssno 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet getSpDepNotes(int empno, string ssno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT notes From Dsktp_spdep WHERE xrefempno = @empno and ssno = @ssn";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@empno", empno);
            cmd.Parameters.AddWithValue("@ssn", ssno);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Get Sponsored Dependents Elections by Ssno 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet getSpDepElections(string ssno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT plancd, startdt, enddt FROM Dsktp_spdepelect WHERE ssno = @ssno ORDER BY orderno";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssno", ssno);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Dependants Record for the given Employee Number
        /// </summary>
        /// <param name="empno">Dependent SSN</param>
        /// <returns>Depenedent Record</returns>
        public SpDepRecord getSpDepData(string dssn)
        {
            string _cmdstr = "SELECT ssno, lname, fname, mname, addr1, addr2, city"
                                + ", state, zipcd, sexcd, relcd, phoneno, dob, userdt"
                                + ", userid, altssno, notes, orgcd, carrierdt, lastplancd"
                                + ", laststartdt, lastenddt, xrefempno, plancd, startdt, enddt "
                                + "FROM Dsktp_spdep WHERE (ssno = @ssno)";

            SpDepRecord dRec = new SpDepRecord();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssno", dssn);
            SqlDataReader reader, reader1;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dRec.EmpNum = Convert.ToInt32(reader["xrefempno"]);
                    dRec.SSN = reader["ssno"].ToString();
                    dRec.FirstName = reader["fname"].ToString();
                    dRec.LastName = reader["lname"].ToString();
                    dRec.MiddleInitial = reader["mname"].ToString();
                    dRec.Address1 = reader["addr1"].ToString();
                    dRec.Address2 = reader["addr2"].ToString();
                    dRec.City = reader["city"].ToString();
                    dRec.State = reader["state"].ToString();
                    dRec.Zip = reader["zipcd"].ToString();
                    dRec.OrgCode = reader["orgcd"].ToString();
                    dRec.Phone = reader["phoneno"].ToString();
                    if (reader["relcd"] != DBNull.Value)
                    {
                        dRec.RelCode = reader["relcd"].ToString();
                    }
                    if (reader["sexcd"] != DBNull.Value)
                    {
                        dRec.SexCode = reader["sexcd"].ToString();
                    }
                    if (reader["dob"] != DBNull.Value)
                    {
                        dRec.DOB = Convert.ToDateTime(reader["dob"].ToString());
                    }

                }
                reader.Close();
            }
            conn.Close();
            return dRec;
        }


        /// <summary>
        /// Sets the SpDep data from database to SpDepSearchRecord object.
        /// </summary>
        /// <param name="reader1">DataReader</param>
        /// <returns>SpDepSearchRecord</returns>
        protected SpDepSearchRecord assignSpDepData(SqlDataReader reader1)
        {
            SpDepSearchRecord SpDrec = new SpDepSearchRecord();
            SpDrec.SSN = reader1["ssno"].ToString();
            SpDrec.FirstName = reader1["fname"].ToString();
            SpDrec.LastName = reader1["lname"].ToString();
            SpDrec.xRefEmpNum = Convert.ToInt32(reader1["xrefempno"]);

            return SpDrec;
        }

        #endregion



        #region TowerEmp Data

        /// <summary>
        /// Search Tower Emp records with given Last name and First name
        /// </summary>
        /// <param name="fname">First Name</param>
        /// <param name="lname">Last Name</param>
        /// <returns>DataSet</returns>
        public DataSet searchTower(string fn, string ln, string empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;

            string _cmdstr = " SELECT lname, fname, ssno "
                                + "FROM Dsktp_toweremp WHERE ";
            string _addl = string.IsNullOrEmpty(empno) ? (string.IsNullOrEmpty(ln) ?
                (string.IsNullOrEmpty(fn) ? "" : "fname LIKE '" + fn + "%'") :
                ("lname LIKE '" + ln + "%'" + (string.IsNullOrEmpty(fn) ? "" :
                " and fname LIKE '" + fn + "%'"))) : "empno = " + empno;
            _cmdstr = _cmdstr + _addl;

            //string cmdStr = "SELECT fname, lname, ssno From Dsktp_toweremp WHERE lname = @lname AND fname = @fname";
            cmd = new SqlCommand(_cmdstr, conn);
            //cmd.Parameters.AddWithValue("@lname", ln);
            //cmd.Parameters.AddWithValue("@fname", fn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Checks whether Tower Employee with given Employee SSN exists.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>True or False</returns>
        public bool checkExistTowerRecord(string ssn)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_toweremp WHERE ssno = @ssn";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }


        /// <summary>
        /// Gets TowerEmp record from database for given Employee ssn.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>TowerEmp Record</returns>
        public TowerEmpRecord getTowerEmpData(string ssn)
        {
            string _cmdstr = " SELECT ssno, lname, fname, addr1, addr2, city, state, zipcd, phoneno"
                                + ", dlname, dfname, ddob,"
                                + " case relcd when 'DP' then 'Domestic Partner' "
                                + "when 'AD' then 'Adult Dependent' "
                                + "when 'OT' then 'Other Dependent' "
                                + "else relcd end as relation, indhealthind "
                                + "FROM Dsktp_toweremp WHERE (ssno = @ssn)";

            TowerEmpRecord tRec1 = new TowerEmpRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);

            SqlDataReader reader;
            conn.ConnectionString = connStr;
            
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tRec1 = assignData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return tRec1;
        }

        /// <summary>
        /// Sets the TowerEmp data from database to TowerEmpRecord object.
        /// </summary>
        /// <param name="reader1">DataReader</param>
        /// <returns>TowerEmpRecord</returns>
        protected TowerEmpRecord assignData(SqlDataReader reader1)
        {
            TowerEmpRecord tRec = new TowerEmpRecord();
            tRec.SSN = reader1["ssno"].ToString();
            tRec.FirstName = reader1["fname"].ToString();
            tRec.LastName = reader1["lname"].ToString();
            tRec.Address1 = reader1["addr1"].ToString();
            tRec.Address2 = reader1["addr2"].ToString();
            tRec.State = reader1["state"].ToString();
            tRec.City = reader1["city"].ToString();
            tRec.Zip = reader1["zipcd"].ToString();
            if (reader1["ddob"] != DBNull.Value)
            {
                tRec.DDOB = Convert.ToDateTime(reader1["ddob"]);
            }
            tRec.Phone = reader1["phoneno"].ToString();
            tRec.DRelCode = reader1["relation"].ToString();
            tRec.HealthInd = reader1["indhealthind"].ToString();
            tRec.DFname = reader1["dfname"].ToString();
            tRec.DLname = reader1["dlname"].ToString();
            
            return tRec;
        }


        #endregion



        #region Retiree Data

        /// <summary>
        /// Checks whether Retiree with given SSN exists.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>True or False</returns>
        public bool checkExistRetRecord(string ssn)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_retpart WHERE ssno = @ssn";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }


        /// <summary>
        /// Searches for Retiree records from database by fname, lname or empno.
        /// </summary>
        /// <param name="ssn">LastName, FirstName, Empno</param>
        /// <returns>DataSet</returns>
        public DataSet searchRetirees(string ln, string fn, string empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string _cmdstr = " SELECT lname, fname, empno, ssno "
                                + "FROM Dsktp_retpart WHERE ";
            string _addl = string.IsNullOrEmpty(empno) ? (string.IsNullOrEmpty(ln) ?
                (string.IsNullOrEmpty(fn) ? "" : "fname LIKE '" + fn + "%'") :
                ("lname LIKE '" + ln + "%'" + (string.IsNullOrEmpty(fn) ? "" :
                " and fname LIKE '" + fn + "%'"))) : "empno = " + empno;
            _cmdstr = _cmdstr + _addl;

            SqlCommand command = new SqlCommand(_cmdstr, conn);

            //SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = command;
                da.Fill(ds);
                //reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    SpDRec = assignSpDepData(reader);
                //}
                //reader.Close();
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Gets RetPartRecord from database for given Employee ssn.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>RetPartRecord</returns>
        public RetPartRecord getRetPartData(string ssn)
        {
            string _cmdstr = " SELECT ssno, empno, lname, fname, ptype, estatus"
                                + ", dedauthdt, formrecdt, pathcd, pathdt, pathuserid, pathuserdt "
                                + "FROM Dsktp_retpart WHERE (ssno = @ssn)";

            RetPartRecord tRec1 = new RetPartRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);

            SqlDataReader reader;
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tRec1 = assignRPData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return tRec1;
        }

        /// <summary>
        /// Sets the RetPart data from database to RetPartRecord object.
        /// </summary>
        /// <param name="reader1">DataReader</param>
        /// <returns>RetPartRecord</returns>
        protected RetPartRecord assignRPData(SqlDataReader reader1)
        {
            RetPartRecord RpRec = new RetPartRecord();
            RpRec.SSN = reader1["ssno"].ToString();
            RpRec.FirstName = reader1["fname"].ToString();
            RpRec.LastName = reader1["lname"].ToString();
            RpRec.EmpNo = reader1["empno"].ToString();
            RpRec.PType = reader1["ptype"].ToString();
            RpRec.EStatus = reader1["estatus"].ToString();
            if (!String.IsNullOrEmpty(reader1["dedauthdt"].ToString()))
            {
                RpRec.DedAuthDate = Convert.ToDateTime(reader1["dedauthdt"]);
            }
            if (!String.IsNullOrEmpty(reader1["formrecdt"].ToString()))
            {
                RpRec.FormRecDate = Convert.ToDateTime(reader1["formrecdt"]);
            }
            RpRec.PathCd = reader1["pathcd"].ToString();
            if (!String.IsNullOrEmpty(reader1["pathdt"].ToString()))
            {
                RpRec.PathDt = Convert.ToDateTime(reader1["pathdt"]);
            }
            RpRec.PathUserID = reader1["pathuserid"].ToString();
            if (!String.IsNullOrEmpty(reader1["pathuserdt"].ToString()))
            {
                RpRec.PathUserDate = Convert.ToDateTime(reader1["pathuserdt"]);
            }

            return RpRec;
        }

        /// <summary>
        /// Search RetElect records with SSN 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet searchRetElects(string ssno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT seqno, bencd, ssno, eventcd, eventdt"
                            + ", etype, plancd, tiercd, eecost, startdt, stopdt, orderno "
                            + "FROM Dsktp_retelect "
                            + "WHERE (ssno = @ssn) "
                            + "ORDER BY eventdt DESC, orderno";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssn", ssno);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Search RetPayHist records with SSN 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet searchRetPayHist(string ssn)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT ssno, chkno, etype, recdt, payamt, startdt, enddt, userid, userdt, orderno, batchno "
                            + "FROM Dsktp_retpayhist "
                            + "WHERE (ssno = @ssn) ORDER BY orderno";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Search RetPayrollHist records with SSN 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet searchRetPayrollHist(string ssn)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT ssno, gendt, effdt, payamt, doecd, payrollflg, userid, userdt "
                            + "FROM Dsktp_retpayrollhist "
                            + "WHERE (ssno = @ssn) ORDER BY effdt desc";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        #endregion



        #region HIPAA Data

        /// <summary>
        /// Checks whether HipCert record with given Employee SSN exists.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>True or False</returns>
        public bool checkExistHipCertRecord(string ssn)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_hipcert WHERE ssno = @ssn";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }


        /// <summary>
        /// Searches for HipCert Participant records from database by fname, lname or empno.
        /// </summary>
        /// <param name="ssn">LastName, FirstName, Empno</param>
        /// <returns>DataSet</returns>
        public DataSet searchHipCertParticipants(string ln, string fn, string empno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string _cmdstr = " SELECT lname, fname, empno, ssno "
                                + "FROM Dsktp_hipcert WHERE ";
            string _addl = string.IsNullOrEmpty(empno) ? (string.IsNullOrEmpty(ln) ?
                (string.IsNullOrEmpty(fn) ? "" : "fname LIKE '" + fn + "%'") :
                ("lname LIKE '" + ln + "%'" + (string.IsNullOrEmpty(fn) ? "" :
                " and fname LIKE '" + fn + "%'"))) : "empno = " + empno;
            _cmdstr = _cmdstr + _addl;

            SqlCommand command = new SqlCommand(_cmdstr, conn);

            //SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = command;
                da.Fill(ds);
                //reader = command.ExecuteReader();
                //while (reader.Read())
                //{
                //    SpDRec = assignSpDepData(reader);
                //}
                //reader.Close();
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Gets HipCertRecord from database for given Employee ssn.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>HipCertRecord</returns>
        public HipCertRecord getHipCertData(string ssn)
        {
            string _cmdstr = " SELECT ssno, empno, lname, fname, addr1, addr2, city, state, zipcd"
                            + ", sexcd, mstat, dob, qualcd, qualdt, printflag, printdt, mon18flag "
                            + "FROM Dsktp_hipcert WHERE (ssno = @ssn)";

            HipCertRecord tRec1 = new HipCertRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);

            SqlDataReader reader;
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tRec1 = assignHipData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return tRec1;
        }

        /// <summary>
        /// Sets the HipCert data from database to HipCertRecord object.
        /// </summary>
        /// <param name="reader1">DataReader</param>
        /// <returns>HipCertRecord</returns>
        protected HipCertRecord assignHipData(SqlDataReader reader1)
        {
            HipCertRecord HCRec = new HipCertRecord();
            HCRec.SSN = reader1["ssno"].ToString();
            HCRec.FirstName = reader1["fname"].ToString();
            HCRec.LastName = reader1["lname"].ToString();
            if (reader1["empno"] != DBNull.Value)
            {
                HCRec.EmpNo = reader1["empno"].ToString();
            }
            if (reader1["addr1"] != DBNull.Value)
            {
                HCRec.Address1 = reader1["addr1"].ToString();
            }
            if (reader1["addr2"] != DBNull.Value)
            {
                HCRec.Address2 = reader1["addr2"].ToString();
            }
            if (reader1["city"] != DBNull.Value)
            {
                HCRec.City = reader1["city"].ToString();
            }
            if (reader1["state"] != DBNull.Value)
            {
                HCRec.State = reader1["state"].ToString();
            }
            if (reader1["zipcd"] != DBNull.Value)
            {
                HCRec.Zip = reader1["zipcd"].ToString();
            }
            if (reader1["sexcd"] != DBNull.Value)
            {
                HCRec.Sex = reader1["sexcd"].ToString();
            }
            if (reader1["mstat"] != DBNull.Value)
            {
                HCRec.Mstat = reader1["mstat"].ToString();
            }
            if (reader1["dob"] != DBNull.Value)
            {
                HCRec.DOB = Convert.ToDateTime(reader1["dob"]);
            }
            if (reader1["qualcd"] != DBNull.Value)
            {
                HCRec.QualCd = reader1["qualcd"].ToString();
            }
            if (reader1["qualdt"] != DBNull.Value)
            {
                HCRec.QualDt = Convert.ToDateTime(reader1["qualdt"]);
            }
            if (reader1["printflag"] != DBNull.Value)
            {
                HCRec.PrintFlag = reader1["printflag"].ToString();
            }
            if (reader1["printdt"] != DBNull.Value)
            {
                HCRec.PrintDt = Convert.ToDateTime(reader1["printdt"]);
            }
            if (reader1["mon18flag"] != DBNull.Value)
            {
                HCRec.Mon18Flag = reader1["mon18flag"].ToString();
            }
            
            return HCRec;
        }


        /// <summary>
        /// Get HipHist by Ssno 
        /// </summary>
        /// <param name="ssno">ssno</param>
        /// <returns>DataSet</returns>
        public DataSet getHipHistCvg(string ssn)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT altssno, Plancd, Fromdt, Thrudt, nodays "
                            + "FROM Dsktp_hiphist WHERE (altssno = @ssn) ORDER BY Fromdt DESC";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssn", ssn);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        /// <summary>
        /// Get HIPAA Dependents record with Participant ssn 
        /// </summary>
        /// <param name="ssn">ssn</param>
        /// <returns>DataSet</returns>
        public DataSet getHipDeps(string ssn)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT altssno, depname, dssno, relcd "
                            + "FROM Dsktp_hipdep WHERE (altssno = @ssn) ORDER BY relcd DESC";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@ssn", ssn);

            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        #endregion



        #region TermFile Data

        /// <summary>
        /// Checks whether TermFile record with given Employee Number exists.
        /// </summary>
        /// <param name="empno">empno</param>
        /// <returns>True or False</returns>
        public bool checkExistTermRecord(int empno)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_termfile WHERE empno = @empno";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }


        /// <summary>
        /// Gets TermFile record from database for given Employee number.
        /// </summary>
        /// <param name="empno">empno</param>
        /// <returns>TermFile Record</returns>
        public TermFileRecord getTermFileData(int empno)
        {
            string _cmdstr = "SELECT empno, name, storageno FROM Dsktp_termfile WHERE (empno = @empno)";

            TermFileRecord tRec1 = new TermFileRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);

            conn.ConnectionString = connStr;
            SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tRec1.EmpNo = Convert.ToInt32(reader["empno"]);
                    tRec1.Name = reader["name"].ToString();
                    tRec1.StorageNo = Convert.ToInt32(reader["storageno"]);
                }
                reader.Close();
            }
            conn.Close();
            return tRec1;
        }

        #endregion



        #region Translog Data

        /// <summary>
        /// Get Translog record with User ID 
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>DataSet</returns>
        public DataSet getTransLog(string uid)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT userid, entrydt, entrytime, "
                            + "case result when 'OK' then 'Result Successful' "
                            + "when 'P' then 'Login Unsuccessful - Password' "
                            + "when 'U' then 'Login Unsuccessful - User ID' "
                            + "else result end as rslt"
                            + ", routinename FROM Dsktp_translog WHERE (userid = @uid) "
                            + "ORDER BY entrydt desc, entrytime desc";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@uid", uid);

            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        #endregion



        #region PCAudit Data

        /// <summary>
        /// Get PCAudit records between from and to dates 
        /// </summary>
        /// <param name="fromdt">fromdt</param>
        /// <param name="todt">todt</param>
        /// <returns>DataSet</returns>
        public DataSet getPCAuditLog(DateTime fromdt, DateTime todt)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT ISNULL("
                            + "(SELECT dsc FROM Dsktp_codes WHERE (classcd = 'PCAUDIT') AND (codeid = Dsktp_pcaudit.typecd))"
                            + ", typecd) AS typecode"
                            + ", pcname, userid, userdt, usertime, comments "
                            + "FROM Dsktp_pcaudit WHERE (userdt >= @fromdt) AND (userdt <= @todt) "
                            + "ORDER BY userdt desc, usertime desc";
            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("@fromdt", fromdt);
            cmd.Parameters.AddWithValue("@todt", todt);

            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }

        #endregion



        #region WorkOrder Data

        /// <summary>
        /// Retrieve list of EBA Desktop historical work orders 
        /// </summary>
        /// <param name=""></param>
        /// <returns>DataSet</returns>
        public DataSet retrieveWorkOrders(string wonum, string proj)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;

            string _cmdstr = "SELECT seqno, woname, type, authorid, dtreq "
                                + "FROM Dsktp_wodetail ";
            string _addl = " WHERE " + (string.IsNullOrEmpty(wonum) ? (string.IsNullOrEmpty(proj) ? "" : "type like '" + proj + "%'") :
                (string.IsNullOrEmpty(proj) ? "seqno = " + wonum : "seqno = " + wonum + " and type like '" + proj + "%'"));

            string _order = " ORDER BY type, seqno";
                //(string.IsNullOrEmpty(proj) ? (string.IsNullOrEmpty(proj) ?
                //(string.IsNullOrEmpty(fn) ? "" : "fname LIKE '" + fn + "%'") :
                //("lname LIKE '" + ln + "%'" + (string.IsNullOrEmpty(fn) ? "" :
                //" and fname LIKE '" + fn + "%'"))) : "seqno = " + Convert.ToInt32(wonum).ToString());

            _addl = (string.Equals(_addl.Trim(),"WHERE") ? "" : _addl);

            _cmdstr = _cmdstr + _addl + _order;

            //string cmdStr = "SELECT seqno, woname, type, authorid, dtreq "
            //                + "From Dsktp_wodetail order by type, seqno";

            cmd = new SqlCommand(_cmdstr, conn);
            //cmd.Parameters.AddWithValue("@empno", empno);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Retrieve list of EBA Desktop historical work orders status changes
        /// </summary>
        /// <param name="wonum">wonum</param>
        /// <param name="proj">proj</param>
        /// <returns>DataSet</returns>
        public DataSet retrieveStatHist(int seqno, string proj)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;

            string _cmdstr = "SELECT status, "
                            + "(Select dsc from Dsktp_codes where classcd = 'WOSTATUS' and codeid = status) as descr, "
                            + "userid, userdt, usertime "
                            + "FROM Dsktp_wostathist Where (woseqno = @seqno) AND (type = @proj) "
                            + "ORDER BY seqno";
            

            //string cmdStr = "SELECT seqno, woname, type, authorid, dtreq "
            //                + "From Dsktp_wodetail order by type, seqno";

            cmd = new SqlCommand(_cmdstr, conn);
            cmd.Parameters.AddWithValue("@seqno", seqno);
            cmd.Parameters.AddWithValue("@proj", proj);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// Retrieve list of EBA Desktop historical work orders user access
        /// </summary>
        /// <param name="wonum">wonum</param>
        /// <param name="proj">proj</param>
        /// <returns>DataSet</returns>
        public DataSet retrieveUserHist(int seqno, string proj)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;

            string _cmdstr = "SELECT userid, userdt, usertime, process "
                                + "FROM Dsktp_wouserhist Where (woseqno = @seqno) AND (type = @proj) "
                                + "ORDER BY seqno";


            cmd = new SqlCommand(_cmdstr, conn);
            cmd.Parameters.AddWithValue("@seqno", seqno);
            cmd.Parameters.AddWithValue("@proj", proj);
            conn.ConnectionString = connStr;
            conn.Open();
            using (conn)
            {
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            conn.Close();
            return ds;
        }


        /// <summary>
        /// WorkOrder Record for the given WO sequence number and project
        /// </summary>
        /// <param name="seqno">WO sequence number</param>
        /// <returns>WOdetail Record</returns>
        public WOdetailRecord getWOdetailData(int woseqno, string proj)
        {
            string _cmdstr = "SELECT seqno, authorid, woname, probdesc, soldesc, spcldesc, "
                             + "type, dtreq, dtrespdue, reqlvl, prio "
                             + "FROM Dsktp_wodetail WHERE (seqno = @seqno) and (type = @proj)";

            WOdetailRecord dRec = new WOdetailRecord();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@seqno", woseqno);
            command.Parameters.AddWithValue("@proj", proj);
            SqlDataReader reader, reader1;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dRec.SeqNo = Convert.ToInt32(reader["seqno"]);
                    dRec.AuthorID = reader["authorid"].ToString();
                    dRec.WOname = reader["woname"].ToString();
                    if (reader["probdesc"] != DBNull.Value)
                    {
                        dRec.Description = reader["probdesc"].ToString();
                    }
                    if (reader["soldesc"] != DBNull.Value)
                    {
                        dRec.Justif = reader["soldesc"].ToString();
                    }
                    if (reader["spcldesc"] != DBNull.Value)
                    {
                        dRec.Comments = reader["spcldesc"].ToString();
                    }
                    if (reader["type"] != DBNull.Value)
                    {
                        dRec.Type = reader["type"].ToString();
                    }
                    if (reader["dtreq"] != DBNull.Value)
                    {
                        dRec.ReqDt = Convert.ToDateTime(reader["dtreq"].ToString());
                    }
                    if (reader["dtrespdue"] != DBNull.Value)
                    {
                        dRec.RespDt = Convert.ToDateTime(reader["dtrespdue"].ToString());
                    }
                    if (reader["reqlvl"] != DBNull.Value)
                    {
                        dRec.ReqLvl = Convert.ToInt32(reader["reqlvl"]);
                    }
                    if (reader["prio"] != DBNull.Value)
                    {
                        dRec.Priority = reader["prio"].ToString();
                    }

                }
                reader.Close();
            }
            conn.Close();
            return dRec;
        }



        /// <summary>
        /// WOresponse Record for the given WO sequence number
        /// </summary>
        /// <param name="seqno">WO sequence number</param>
        /// <returns>WOdetail Record</returns>
        public WOresponseRecord getWOresponseData(int woseqno, string proj)
        {
            string _cmdstr = "SELECT seqno, itdprojname, analyst, itdrecdesc, "
                             + "spclskillsets, risks, manmonths, manhours, dtwoassign, dtwocomp "
                             + "FROM Dsktp_wodetail WHERE (seqno = @seqno) and (type = @proj)";

            WOresponseRecord dRec = new WOresponseRecord();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@seqno", woseqno);
            command.Parameters.AddWithValue("@proj", proj);
            SqlDataReader reader, reader1;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dRec.SeqNo = Convert.ToInt32(reader["seqno"]);
                    dRec.ProjName = reader["itdprojname"].ToString();
                    dRec.Analyst = reader["analyst"].ToString();
                    if (reader["itdrecdesc"] != DBNull.Value) dRec.Results = reader["itdrecdesc"].ToString();
                    if (reader["spclskillsets"] != DBNull.Value) dRec.Considerations = reader["spclskillsets"].ToString();
                    if (reader["risks"] != DBNull.Value) dRec.Risks = reader["risks"].ToString();
                    if (reader["manmonths"] != DBNull.Value) dRec.ManMonth = Convert.ToInt32(reader["manmonths"]);
                    if (reader["manhours"] != DBNull.Value) dRec.ManHours = Convert.ToInt32(reader["manhours"]);
                    if (reader["dtwoassign"] != DBNull.Value)
                    {
                        dRec.AssignDt = Convert.ToDateTime(reader["dtwoassign"].ToString());
                    }
                    if (reader["dtwocomp"] != DBNull.Value)
                    {
                        dRec.CompleteDt = Convert.ToDateTime(reader["dtwocomp"].ToString());
                    }

                }
                reader.Close();
            }
            conn.Close();
            return dRec;
        }


        /// <summary>
        /// WOstatus Record for the given WO sequence number
        /// </summary>
        /// <param name="woseqno">WO sequence number</param>
        /// <returns>WOstatus Record</returns>
        public WOstatusRecord getWOstatusData(int woseqno, string proj)
        {
            string _cmdstr = "SELECT seqno, status,"
                             + "(Select dsc from Dsktp_codes where classcd = 'WOSTATUS' and codeid = status) as descr, "
                             + "dtstatus, woapproval, dtwoapprove, dtprojassigned, "
                             + "itddeveloper, reqdefdt, techspecdt, dtstart, dtclose, godt, comments "
                             + "FROM Dsktp_wodetail WHERE (seqno = @seqno) and (type = @proj)";

            WOstatusRecord dRec = new WOstatusRecord();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@seqno", woseqno);
            command.Parameters.AddWithValue("@proj", proj);
            SqlDataReader reader, reader1;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dRec.SeqNo = Convert.ToInt32(reader["seqno"]);
                    dRec.Status = reader["status"].ToString();
                    if (reader["dtstatus"] != DBNull.Value)
                    {
                        dRec.StatusDt = Convert.ToDateTime(reader["dtstatus"].ToString());
                    }
                    if (reader["descr"] != DBNull.Value)
                    {
                        dRec.Descr = reader["descr"].ToString();
                    }
                    if (reader["woapproval"] != DBNull.Value) dRec.Approved = Convert.ToInt32(reader["woapproval"]);
                    if (reader["dtwoapprove"] != DBNull.Value)
                    {
                        dRec.ReqApprovalDt = Convert.ToDateTime(reader["dtwoapprove"].ToString());
                    }
                    if (reader["dtprojassigned"] != DBNull.Value)
                    {
                        dRec.ProjAssignDt = Convert.ToDateTime(reader["dtprojassigned"].ToString());
                    }
                    dRec.Developer = reader["itddeveloper"].ToString();
                    if (reader["reqdefdt"] != DBNull.Value)
                    {
                        dRec.ReqDefDt = Convert.ToDateTime(reader["reqdefdt"].ToString());
                    }
                    if (reader["techspecdt"] != DBNull.Value)
                    {
                        dRec.TechSpecDt = Convert.ToDateTime(reader["techspecdt"].ToString());
                    }
                    if (reader["dtstart"] != DBNull.Value)
                    {
                        dRec.StartDt = Convert.ToDateTime(reader["dtstart"].ToString());
                    }
                    if (reader["dtclose"] != DBNull.Value)
                    {
                        dRec.CloseDt = Convert.ToDateTime(reader["dtclose"].ToString());
                    }
                    if (reader["godt"] != DBNull.Value)
                    {
                        dRec.GoDt = Convert.ToDateTime(reader["godt"].ToString());
                    } 
                    dRec.Comments = reader["comments"].ToString();
                    
                }
                reader.Close();
            }
            conn.Close();
            return dRec;
        }


        /// <summary>
        /// Checks whether WorkOrder with given WO number exists.
        /// </summary>
        /// <param name="wonum">WorkOrder number</param>
        /// <param name="proj">WorkOrder project</param>
        /// <returns>True or False</returns>
        public bool checkExistWOrecord(int wonum, string proj)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Dsktp_wodetail WHERE (seqno = @wonum) and (type = @proj)";
            bool _exists = false;

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@wonum", wonum);
            command.Parameters.AddWithValue("@proj", proj);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (_count > 0)
            {
                _exists = true;
            }
            conn.Close();
            return _exists;
        }


        #endregion


    }
}

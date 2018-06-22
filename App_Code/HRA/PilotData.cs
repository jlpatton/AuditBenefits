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
/// Summary description for PilotData
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class PilotData
    {
        private string connStr = null;
        private SqlConnection conn;

        public PilotData()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        /// <summary>
        /// Checks whether Employee with given Employee number exists.
        /// </summary>
        /// <param name="empno">Employee number</param>
        /// <returns>True or False</returns>
        public bool checkExistRecord(int empno)
        {
            int _count = 0;
            string _cmdstr = "SELECT COUNT(*) FROM Employee WHERE empl_empno = @empno AND empl_hra_part = 1";
            bool _exists = false;

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);
            conn.Open();
            using (conn)
            {
                _count = Convert.ToInt32(command.ExecuteScalar());
            }
            if(_count > 0)
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
            string _cmdstr = "SELECT COUNT(*) FROM Employee WHERE empl_ssn = @ssn  AND empl_hra_part = 1";
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
        /// Search Employees record with given Last name first initial
        /// </summary>
        /// <param name="lnameFirstInitial">Last name first initial</param>
        /// <returns>DataSet</returns>
        public DataSet searchPilot(string lnameFirstInitial)
        {
            SqlDataAdapter da = new SqlDataAdapter();             
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT * From Employee WHERE empl_lname LIKE '" + lnameFirstInitial + "%' AND empl_hra_part = 1";
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
        /// Search Employees record with given Last name and First name
        /// </summary>
        /// <param name="fname">First Name</param>
        /// <param name="lname">Last Name</param>
        /// <returns>DataSet</returns>
        public DataSet searchPilot(string fname,string lname)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlCommand cmd;
            string cmdStr = "SELECT * From Employee WHERE empl_lname = @lname AND empl_fname = @fname AND empl_hra_part = 1";
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
        /// Gets pilot record from database for given Employee number.
        /// </summary>
        /// <param name="empno">Employee Number</param>
        /// <returns>Pilot Record</returns>
        public PilotRecord getPilotData(int empno)
        {
            string _cmdstr = " SELECT [empl_empno],[empl_ssn],[empl_fname],[empl_lname],[empl_minit],[empl_sex],[empl_dob]"
                                + ",[empl_retire_dt],[empl_fulltime_dt],[empl_stat_dt],[empl_statcd],[addr_addr1]"
                                + ",[addr_addr2],[addr_city],[addr_state],[addr_zip],[hraamt1],[hraamt2] "
                                + " FROM [Employee],[Address],[HRA_Amounts] "
                                + " WHERE (addr_empno = empl_empno) AND (empno = empl_empno) AND empl_empno = @empno AND addr_type = '001'"; 
            
            PilotRecord pRec1 = new PilotRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);           

            SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pRec1 = assignData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return pRec1;
        }

        /// <summary>
        /// Gets pilot record from database for given Employee ssn.
        /// </summary>
        /// <param name="ssn">SSN</param>
        /// <returns>Pilot Record</returns>
        public PilotRecord getPilotData(string ssn)
        {
            string _cmdstr = " SELECT [empl_empno],[empl_ssn],[empl_fname],[empl_lname],[empl_minit],[empl_sex],[empl_dob]"
                                + ",[empl_retire_dt],[empl_fulltime_dt],[empl_stat_dt],[empl_statcd],[addr_addr1]"
                                + ",[addr_addr2],[addr_city],[addr_state],[addr_zip],[hraamt1],[hraamt2] "
                                + " FROM [Employee],[Address],[HRA_Amounts] "
                                + " WHERE (addr_empno = empl_empno) AND (empno = empl_empno) AND empl_ssn = @ssn AND addr_type = '001'"; 
            
            PilotRecord pRec1 = new PilotRecord();
            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@ssn", ssn);

            SqlDataReader reader;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    pRec1 = assignData(reader);
                }
                reader.Close();
            }
            conn.Close();
            return pRec1;
        }

        /// <summary>
        /// Sets the pilot data from database to PilotRecord object.
        /// </summary>
        /// <param name="reader1">DataReader</param>
        /// <returns>Pilot Record</returns>
        protected PilotRecord assignData(SqlDataReader reader1)
        {
            PilotRecord pRec = new PilotRecord();
            pRec.EmpNum = Convert.ToInt32(reader1["empl_empno"]);
            pRec.SSN = reader1["empl_ssn"].ToString();
            pRec.FirstName = reader1["empl_fname"].ToString();
            pRec.LastName = reader1["empl_lname"].ToString();
            pRec.MiddleInitial = reader1["empl_minit"].ToString();
            pRec.Address1 = reader1["addr_addr1"].ToString();
            pRec.Address2 = reader1["addr_addr2"].ToString();
            pRec.State = reader1["addr_state"].ToString();
            pRec.City = reader1["addr_city"].ToString();
            pRec.Zip = reader1["addr_zip"].ToString();
            pRec.DateBirth = Convert.ToDateTime(reader1["empl_dob"]);
            pRec.SexCode = reader1["empl_sex"].ToString();
            pRec.Status = reader1["empl_statcd"].ToString();
            if (reader1["empl_retire_dt"] != DBNull.Value)
            {
                pRec.RetDate = Convert.ToDateTime(reader1["empl_retire_dt"]); 
            }
            if (reader1["empl_fulltime_dt"] != DBNull.Value)
            {
                pRec.PermDate = Convert.ToDateTime(reader1["empl_fulltime_dt"]); 
            }
            if (reader1["empl_stat_dt"] != DBNull.Value)
            {
                pRec.DeathDate = Convert.ToDateTime(reader1["empl_stat_dt"]); 
            }
           
            if (reader1["hraamt1"]!=DBNull.Value)
            {
                pRec.LumpSum = Convert.ToDecimal(reader1["hraamt1"]);
            }
            if (reader1["hraamt2"] != DBNull.Value)
            {
                pRec.HRAAmount = Convert.ToDecimal(reader1["hraamt2"]);
            }
            return pRec;
        }

        /// <summary>
        /// Dependants Record for the given Employee Number
        /// </summary>
        /// <param name="empno">Employee Number</param>
        /// <returns>Depenedent Record</returns>
        public DepRecord getPilotDepData(int empno,string dssn)
        {
            string _cmdstr = "SELECT [dpnd_empno],[dpnd_relationship],[dpnd_fname],[dpnd_minit]"
                                +",[dpnd_lname],[dpnd_ssn],[dpnd_sex],[dpnd_dob],[dpnd_order]"
                                +",[dpnd_owner],[dpnd_ownernotElegb],[dpnd_validated]"
                                +",[dpnd_validated_dt],[dpnd_owner_eff_dt]"
                                +",[dpnd_owner_stop_dt],[dpnd_row_eff_dt],[dpnd_notElegbnotes]"
                                +" FROM [Dependant] WHERE [dpnd_empno] = @empno AND [dpnd_ssn] = @dssn";

            string _cmdstr1 = "SELECT [addr_empno],[addr_dpnd_ssn],[addr_type],[addr_addr1],[addr_addr2],[addr_city],[addr_state]"
	                            +",[addr_zip],[addr_phone],[addr_phone2],[addr_email],[addr_email2],[addr_row_eff_dt]"
                                +" FROM [Address] WHERE [addr_empno] = @empno AND [addr_type] = '004' AND [addr_dpnd_ssn] = @dssn";
            
            DepRecord dRec = new DepRecord();

            SqlCommand command = new SqlCommand(_cmdstr, conn);
            command.Parameters.AddWithValue("@empno", empno);
            command.Parameters.AddWithValue("@dssn", dssn); 
            SqlDataReader reader,reader1;
            conn.Open();
            using (conn)
            {
                reader = command.ExecuteReader();
                if(reader.Read())
                {
                    dRec.DEmpNum = Convert.ToInt32(reader["dpnd_empno"]);
                    dRec.DSSN = reader["dpnd_ssn"].ToString();
                    dRec.DFirstName = reader["dpnd_fname"].ToString();
                    dRec.DLastName = reader["dpnd_lname"].ToString();
                    dRec.DMiddleInitial = reader["dpnd_minit"].ToString();
                    if (reader["dpnd_order"] != DBNull.Value)
                    {
                        dRec.Order = Convert.ToInt32(reader["dpnd_order"]);
                    }
                    if (reader["dpnd_relationship"] != DBNull.Value)
                    {
                        dRec.Relation = reader["dpnd_relationship"].ToString();
                    }
                    if (reader["dpnd_sex"] != DBNull.Value)
                    {
                        dRec.DSexCode = reader["dpnd_sex"].ToString();
                    }
                    if (reader["dpnd_dob"] != DBNull.Value)
                    {
                        dRec.DDateBirth = Convert.ToDateTime(reader["dpnd_dob"]).ToString("MM/dd/yyyy");
                    }
                    if (reader["dpnd_owner"] != DBNull.Value)
                    {
                        dRec.Owner = Convert.ToBoolean(reader["dpnd_owner"]);
                    }
                    if (reader["dpnd_validated"] != DBNull.Value)
                    {
                        dRec.OwnershipValidated = Convert.ToBoolean(reader["dpnd_validated"]); //ownership validation status
                    }
                    if (reader["dpnd_ownernotElegb"] != DBNull.Value)
                    {
                        dRec.EligibilityStatus = Convert.ToBoolean(reader["dpnd_ownernotElegb"]); //ownership elegibility status
                    }
                    if (reader["dpnd_notElegbnotes"] != DBNull.Value)
                    {
                        dRec.ElegibilityNotes = reader["dpnd_notElegbnotes"].ToString(); //ownership elegibility status notes
                    }
                    if (reader["dpnd_owner_eff_dt"] != DBNull.Value)
                    {
                        dRec.OwnershipStartDate = Convert.ToDateTime(reader["dpnd_owner_eff_dt"]).ToString("MM/dd/yyyy");
                    }
                    if (reader["dpnd_owner_stop_dt"] != DBNull.Value)
                    {
                        dRec.OwnershipEndDate = Convert.ToDateTime(reader["dpnd_owner_stop_dt"]).ToString("MM/dd/yyyy");
                    }
                    if (reader["dpnd_validated_dt"] != DBNull.Value)
                    {
                        dRec.OwnershipValidDate = Convert.ToDateTime(reader["dpnd_validated_dt"]).ToString("MM/dd/yyyy");
                    }
                }
                reader.Close();

                SqlCommand command1 = new SqlCommand(_cmdstr1, conn);
                command1.Parameters.AddWithValue("@empno", empno);
                command1.Parameters.AddWithValue("@dssn", dssn); 
                reader1 = command1.ExecuteReader();
                if (reader1.Read())
                {
                    dRec.DAddress1 = reader1["addr_addr1"].ToString();
                    dRec.DAddress2 = reader1["addr_addr2"].ToString();
                    dRec.DState = reader1["addr_state"].ToString();
                    dRec.DCity = reader1["addr_city"].ToString();
                    dRec.DZip = reader1["addr_zip"].ToString();
                }
                reader1.Close();
            }
            conn.Close();
            return dRec;
        }

        public void insertPilotDepData(DepRecord insertRec)
        {
            string _cmdstr = "INSERT INTO Dependant (dpnd_empno,dpnd_relationship,dpnd_ssn,dpnd_fname,dpnd_minit,dpnd_lname,dpnd_sex,dpnd_dob,dpnd_order,dpnd_row_eff_dt,dpnd_add_diff)"
                                + " VALUES (@dempno,@drel,@dssn,@dfname,@dmint,@dlname,@dsex,@dob,@dorder,@roweffdt,@diff)";
            string _cmdstr1 = "INSERT INTO Address (addr_empno,addr_dpnd_ssn,addr_type,addr_addr1,addr_addr2,addr_city,addr_state,addr_zip,addr_row_eff_dt)"
                                + " VALUES(@empno,@dssn,@atype,@addr1,@addr2,@city,@state,@zip,@roweffdt)";
            SqlCommand cmd = null;
            SqlTransaction ts;
            conn.Open();
            ts = conn.BeginTransaction();
            try
            {                
                cmd = new SqlCommand(_cmdstr, conn, ts);
                cmd.Parameters.AddWithValue("@dempno", insertRec.DEmpNum);
                cmd.Parameters.AddWithValue("@drel", insertRec.Relation);
                cmd.Parameters.AddWithValue("@dssn", insertRec.DSSN);
                cmd.Parameters.AddWithValue("@dfname", insertRec.DFirstName);

                if (!(insertRec.DAddress1.Equals("") && insertRec.DCity.Equals("") && insertRec.DState.Equals("") && insertRec.DZip.Equals("")))
                {
                    cmd.Parameters.AddWithValue("@diff", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@diff", 0);
                }
                if (insertRec.DMiddleInitial.ToString().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@dmint", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dmint", insertRec.DMiddleInitial);
                }
                cmd.Parameters.AddWithValue("@dlname", insertRec.DLastName);
                cmd.Parameters.AddWithValue("@dsex", insertRec.DSexCode);
                cmd.Parameters.AddWithValue("@dob", insertRec.DDateBirth);
                cmd.Parameters.AddWithValue("@dorder", insertRec.Order);
                cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                cmd.ExecuteNonQuery();

                if (!(insertRec.DAddress1.Equals("") && insertRec.DCity.Equals("") && insertRec.DState.Equals("") && insertRec.DZip.Equals("")))
                {
                    cmd = new SqlCommand(_cmdstr1, conn, ts);
                    cmd.Parameters.AddWithValue("@empno", insertRec.DEmpNum);
                    cmd.Parameters.AddWithValue("@dssn", insertRec.DSSN);
                    cmd.Parameters.AddWithValue("@atype", "004");
                    cmd.Parameters.AddWithValue("@addr1", insertRec.DAddress1);
                    if (insertRec.DAddress2.Equals(""))
                    {
                        cmd.Parameters.AddWithValue("@addr2", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@addr2", insertRec.DAddress2);
                    }
                    cmd.Parameters.AddWithValue("@city", insertRec.DCity);
                    cmd.Parameters.AddWithValue("@state", insertRec.DState);
                    cmd.Parameters.AddWithValue("@zip", insertRec.DZip);
                    cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw (new Exception("Error in inserting new Beneficiary Records!"));
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        public int getBeneficiaryOrders(int empno)
        {
            string cmdstr = "SELECT MAX(dpnd_order) FROM Dependant WHERE dpnd_empno = @empno";
            SqlCommand cmd = null;
                 
            int _maxOrder = 0;
            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdstr, conn);
                cmd.Parameters.AddWithValue("@empno", empno);
                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    _maxOrder = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return _maxOrder;
        }

        public List<int> getBeneficiaryOrderNums(int empno)
        {
            List<int> _ord = new List<int>();
            string cmdstr = "SELECT DISTINCT dpnd_order FROM Dependant WHERE dpnd_empno = @empno";
            SqlCommand cmd = null;
            SqlDataReader reader = null;           
            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdstr, conn);
                cmd.Parameters.AddWithValue("@empno", empno);

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _ord.Add(Convert.ToInt32(reader[0].ToString()));
                }
            }
            finally
            {
                reader.Close();
                conn.Close();
                cmd.Dispose();
            }
            return _ord;
        }

        public bool validateBeneficiaryOwner(int empno)
        {
            string cmdStr = "SELECT COUNT(*) FROM Dependant WHERE dpnd_owner = 1 AND dpnd_empno = @empno";
            SqlCommand cmd = null;
            bool ownerstat = false;            

            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.AddWithValue("@empno", empno);
                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    int ownercnt = Convert.ToInt32(cmd.ExecuteScalar());

                    if (ownercnt > 0)
                    {
                        ownerstat = true;
                    }
                    else
                    {
                        ownerstat = false;
                    }
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }

            return ownerstat;
        }
        
        public bool validateBeneficiaryAge(int empno,string dssn)
        {
            string cmdStr = "SELECT dpnd_dob FROM Dependant WHERE dpnd_ssn = @ssn AND dpnd_empno = @empno";
            SqlCommand cmd = null;
            bool ownerstat = false;
            

            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@empno";
                param.Value = empno;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@ssn", dssn);

                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    DateTime depAge = Convert.ToDateTime(cmd.ExecuteScalar());

                    int diffAge = 0;
                    diffAge = DateTime.Now.Year - depAge.Year;
                    if (DateTime.Now.Month < depAge.Month || (DateTime.Now.Month == depAge.Month && DateTime.Now.Day < depAge.Day))
                    {
                        --diffAge;
                    }
                    if (diffAge >= 24)
                    {
                        ownerstat = true;
                    }
                    else
                    {
                        ownerstat = false;
                    }
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ownerstat;
        }

        public bool validateBeneficiaryElegb(int empno, string dssn)
        {
            string cmdStr = "SELECT dpnd_ownernotElegb FROM Dependant WHERE dpnd_ssn = @ssn AND dpnd_empno = @empno";
            SqlCommand cmd = null;
            bool ownerElegb = false;


            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@empno";
                param.Value = empno;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@ssn", dssn);

                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    if (Convert.ToBoolean(cmd.ExecuteScalar()).Equals(false))
                    {
                        ownerElegb = true;
                    }
                    else if (Convert.ToBoolean(cmd.ExecuteScalar()).Equals(true))
                    {
                        ownerElegb = false;
                    }
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ownerElegb;
        }

        public bool validateBeneficiaryOwnerStopdt(int empno, int ordNum)
        {
            string cmdStr = "SELECT dpnd_owner_stop_dt FROM Dependant WHERE dpnd_empno = @empno AND dpnd_order = @order";           
            SqlCommand cmd = null;
            bool ownerStpdt = false;


            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@empno";
                param.Value = empno;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@order", (ordNum - 1));

                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    ownerStpdt = true;
                }
                else
                {
                    ownerStpdt = false;
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ownerStpdt;
        }

        public string BeneficiaryOwnerStopdt(int empno, int ordNum)
        {
            string cmdStr = "SELECT dpnd_owner_stop_dt FROM Dependant WHERE dpnd_empno = @empno AND dpnd_order = @order";
            SqlCommand cmd = null;
            string ownerStopdt = "";
            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@empno";
                param.Value = empno;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@order", (ordNum - 1));

                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    ownerStopdt = cmd.ExecuteScalar().ToString();
                }               
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ownerStopdt;
        }

        public bool validateBeneficiaryOwnerEffdt(int empno, int ordNum)
        {
            string cmdStr = "SELECT dpnd_owner_eff_dt FROM Dependant WHERE dpnd_empno = @empno AND dpnd_order = @order";
            SqlCommand cmd = null;
            bool ownerEffdt = false;


            try
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@empno";
                param.Value = empno;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@order", (ordNum - 1));

                if (cmd.ExecuteScalar() != DBNull.Value)
                {
                    ownerEffdt = true;
                }
                else
                {
                    ownerEffdt = false;
                }
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ownerEffdt;
        }

        public void updateEmployee(string atype,PilotRecord iRec)
        {
            int empno = iRec.EmpNum;
            string ssn = iRec.SSN;
            string fName = iRec.FirstName;
            string lName = iRec.LastName;
            string minit = iRec.MiddleInitial;
            string sexCd = iRec.SexCode;
            
            DateTime birthDate = iRec.DateBirth;
            DateTime retDate = iRec.RetDate;
            DateTime permDate = iRec.PermDate;
            DateTime deathDate = iRec.DeathDate;
            string status = iRec.Status;           
            
            string addr1 = iRec.Address1;
            string addr2 = iRec.Address2;
            string state = iRec.State;
            string city = iRec.City;
            string zip = iRec.Zip;

            decimal amt1 = iRec.LumpSum;
            decimal amt2 = iRec.HRAAmount;
            
            string _cmdstr = "UPDATE Employee "
                              + " SET [empl_ssn] = @ssn,[empl_fname] = @fname"
                              + ",[empl_lname] = @lname,[empl_minit] = @minit,[empl_sex] = @sex,[empl_dob] = @dob "
                              + ",[empl_retire_dt] = @retdt,[empl_fulltime_dt] = @permdt"
                              + ",[empl_statcd] = @status,[empl_stat_dt] = @deathdt,[empl_row_eff_dt] = @roweffdt"
                              + " WHERE [empl_empno] = @empno";

            string _cmdstr1 = "UPDATE Address "
                             + " SET [addr_type] = @atype,[addr_addr1] = @addr1,[addr_addr2] = @addr2"
                             + ",[addr_city] = @city,[addr_state] = @state,[addr_zip] = @zip,[addr_row_eff_dt] = @roweffdt"
                             + " WHERE [addr_empno] = @empno";

            string _cmdstr2 = "UPDATE HRA_Amounts"
                              + " SET [hraamt1] = @amt1,[hraamt2] = @amt2,[row_eff_dt] = @roweffdt"
                              + " WHERE [empno] = @empno";

            SqlCommand cmd = null;
            SqlTransaction ts;
            SqlDateTime sqldbnull;
            conn.Open();            
            ts = conn.BeginTransaction();
            try
            {
                cmd = new SqlCommand(_cmdstr, conn,ts);
                cmd.Parameters.AddWithValue("@empno", empno);
                cmd.Parameters.AddWithValue("@ssn", ssn);
                cmd.Parameters.AddWithValue("@fname", fName);
                cmd.Parameters.AddWithValue("@lname", lName);
                if (!minit.Trim().Equals(""))
                {
                    cmd.Parameters.AddWithValue("@minit", minit);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@minit", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@sex", sexCd);
                if (birthDate.Equals(DateTime.MinValue))
                {
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dob", birthDate);
                }
                if (retDate.Equals(DateTime.MinValue))
                {
                    cmd.Parameters.AddWithValue("@retdt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@retdt", retDate);
                }
                if (permDate.Equals(DateTime.MinValue))
                {
                    cmd.Parameters.AddWithValue("@permdt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@permdt", permDate);
                }
                cmd.Parameters.AddWithValue("@status", status);
                if (deathDate.Equals(DateTime.MinValue))
                {
                    cmd.Parameters.AddWithValue("@deathdt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@deathdt", deathDate);
                }
                cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(_cmdstr1, conn,ts);
                cmd.Parameters.AddWithValue("@empno", empno);
                cmd.Parameters.AddWithValue("@atype", atype);
                cmd.Parameters.AddWithValue("@addr1", addr1);
                cmd.Parameters.AddWithValue("@addr2", addr2);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@zip", zip);
                cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand(_cmdstr2, conn,ts);
                cmd.Parameters.AddWithValue("@empno", empno);
                cmd.Parameters.AddWithValue("@amt1", amt1);
                cmd.Parameters.AddWithValue("@amt2", amt2);
                cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                cmd.ExecuteNonQuery();

                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw (new Exception("Error in Updating Records"));
            }
            finally
            {
                conn.Close();
            } 
        }

        public void updateDependant(string atype, DepRecord duRec, string _prevssn)
        {
            string _cmdstr = "UPDATE [Dependant] "
                            + " SET [dpnd_relationship] = @rel "
                            + " ,[dpnd_ssn] = @dssn "
                            + " ,[dpnd_fname] = @fname "
                            + " ,[dpnd_minit] = @mi "
                            + " ,[dpnd_lname] = @lname "                           
                            + " ,[dpnd_sex] = @dsexcd "
                            + " ,[dpnd_dob] = @dob "
                            + " ,[dpnd_order] = @order "
                            + " ,[dpnd_owner] = @owner "                //bit
                            + " ,[dpnd_ownernotElegb] = @oElegb  "      //bit
                            + " ,[dpnd_validated] = @valid "            //bit
                            + " ,[dpnd_validated_dt] = @validateddt "
                            + " ,[dpnd_owner_eff_dt] = @effdt "
                            + " ,[dpnd_owner_stop_dt] = @stopdt "
                            + " ,[dpnd_row_eff_dt] = @roweffdt "
                            + " ,[dpnd_notElegbnotes] = @enotes "
                            + " ,[dpnd_add_diff] = @diff "
                            + " WHERE [dpnd_empno] = @empno AND [dpnd_ssn] = @dssn1 ";

            string _cmdstr1 = "UPDATE Address "
                             + " SET [addr_dpnd_ssn] = @dssn,[addr_type] = @atype,[addr_addr1] = @addr1,[addr_addr2] = @addr2"
                             + ",[addr_city] = @city,[addr_state] = @state,[addr_zip] = @zip,[addr_row_eff_dt] = @roweffdt"
                             + " WHERE [addr_empno] = @empno AND [addr_dpnd_ssn] = @dssn1 AND [addr_type] = '004'";

            string _cmdstr2 = "SELECT COUNT(*) "
                                + " FROM Address "
                                + " WHERE [addr_empno] = @empno AND [addr_dpnd_ssn] = @dssn1 AND [addr_type] = @atype";

            string _cmdstr3 = "INSERT INTO [Address] "
                                + " ([addr_empno],[addr_dpnd_ssn],[addr_type],[addr_addr1], "
                                + " [addr_addr2],[addr_city],[addr_state],[addr_zip],[addr_row_eff_dt]) "
                                + " VALUES (@empno,@dssn,@atype,@addr1,@addr2,@city,@state,@zip,@roweffdt) ";

            SqlCommand cmd = null;
            SqlTransaction ts;
            SqlDateTime sqldbnull;
            conn.Open();            
            ts = conn.BeginTransaction();
            try
            {
                cmd = new SqlCommand(_cmdstr, conn, ts);
                cmd.Parameters.AddWithValue("@empno", duRec.DEmpNum);
                cmd.Parameters.AddWithValue("@dssn1", _prevssn);
                cmd.Parameters.AddWithValue("@dssn", duRec.DSSN);
                cmd.Parameters.AddWithValue("@fname", duRec.DFirstName);
                cmd.Parameters.AddWithValue("@lname", duRec.DLastName);
                
                //check if address is different
                if (atype.Equals("004"))
                {
                    cmd.Parameters.AddWithValue("@diff", 1);
                }
                else { cmd.Parameters.AddWithValue("@diff", 0); }

                cmd.Parameters.AddWithValue("@rel", duRec.Relation);
                if (!duRec.DMiddleInitial.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@mi", duRec.DMiddleInitial);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@mi", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@dsexcd", duRec.DSexCode);

                if (!duRec.DDateBirth.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@dob", duRec.DDateBirth);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@order", duRec.Order);
               
                if (duRec.Owner)
                {
                    cmd.Parameters.AddWithValue("@owner", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@owner", 0);
                }

                if (duRec.EligibilityStatus)
                {
                    cmd.Parameters.AddWithValue("@oElegb", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@oElegb", 0);
                }

                if (duRec.OwnershipValidated)
                {
                    cmd.Parameters.AddWithValue("@valid", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@valid", 0);
                }

                if (duRec.OwnershipValidDate.Equals("") || duRec.OwnershipValidDate.Equals(DateTime.MinValue.ToShortDateString()))
                {
                    cmd.Parameters.AddWithValue("@validateddt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@validateddt", duRec.OwnershipValidDate);
                }

                if (duRec.OwnershipStartDate.Equals("") || duRec.OwnershipStartDate.Equals(DateTime.MinValue.ToShortDateString()))
                {
                    cmd.Parameters.AddWithValue("@effdt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@effdt", duRec.OwnershipStartDate);
                }

                if (duRec.OwnershipEndDate.Equals("") || duRec.OwnershipEndDate.Equals(DateTime.MinValue.ToShortDateString()))
                {
                    cmd.Parameters.AddWithValue("@stopdt", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@stopdt", duRec.OwnershipEndDate);
                }

                if (!duRec.ElegibilityNotes.Equals(""))
                {
                    cmd.Parameters.AddWithValue("@enotes", duRec.ElegibilityNotes);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@enotes", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                cmd.ExecuteNonQuery();                

                if (atype.Equals("004"))
                {
                    cmd = new SqlCommand(_cmdstr2, conn, ts);
                    cmd.Parameters.AddWithValue("@empno", duRec.DEmpNum);
                    cmd.Parameters.AddWithValue("@dssn1", _prevssn);
                    cmd.Parameters.AddWithValue("@atype", atype);
                    int _count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (_count == 0)
                    {
                        cmd = new SqlCommand(_cmdstr3, conn, ts);
                    }
                    else
                    {
                        cmd = new SqlCommand(_cmdstr1, conn, ts);
                        cmd.Parameters.AddWithValue("@dssn1", _prevssn);
                    }
                    cmd.Parameters.AddWithValue("@empno",duRec.DEmpNum);
                    cmd.Parameters.AddWithValue("@dssn", duRec.DSSN);
                    cmd.Parameters.AddWithValue("@atype", atype);
                    cmd.Parameters.AddWithValue("@addr1", duRec.DAddress1);
                    cmd.Parameters.AddWithValue("@addr2", duRec.DAddress2);
                    cmd.Parameters.AddWithValue("@city", duRec.DCity);
                    cmd.Parameters.AddWithValue("@state", duRec.DState);
                    cmd.Parameters.AddWithValue("@zip", duRec.DZip);
                    cmd.Parameters.AddWithValue("@roweffdt", DateTime.Now);
                    cmd.ExecuteNonQuery();                  
                }

                ts.Commit(); 
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw (new Exception("Error in Updating Records"));
            }
            finally
            {
                conn.Close();
            }                      
        }

        /// <summary>
        /// Compares two values to check whether they changed; if different then return true else false
        /// </summary>
        /// <param name="oldValue">Old Value</param>
        /// <param name="newValue">New Value after Update</param>
        /// <returns>true or false</returns>
        public bool createAuditObject(object oldValue, object newValue)
        {
            bool _check = false;
            if (!oldValue.ToString().Equals(newValue.ToString()))
            {
                _check = true;
            }
            return _check;
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;

/// <summary>
/// Summary description for ImportTextDAL
/// </summary>
///
namespace EBA.Desktop.Anthem
{
    public class ImportTextDAL
    {
        private string connStr = null;
        private SqlConnection conn;
        public ImportTextDAL()
        {            
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            conn = new SqlConnection(connStr);
        }

        public void deleteStage(string src)
        {
            SqlCommand cmd = null;
            string cmdStr = null;
            switch (src)
            {
                case "GRS":
                    cmdStr = "DELETE FROM stageGRS";
                    break;
                case "ADP":
                    cmdStr = "DELETE FROM stage_ADP";
                    break;
            }
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }                
            
                cmd = new SqlCommand(cmdStr, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        public void spADP(string _yrmo)
        {
            DateTime yrmo = Convert.ToDateTime(_yrmo.Insert(4, "/"));
            SqlCommand cmd = null;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand("sp_InsertADP", conn);
                cmd.Parameters.AddWithValue("@yrmonth", yrmo);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
       

        public void storeGRS(string _desc, string _code, string _type, string _value, string _yrmo,string _coverage)
        {
            SqlCommand cmd = null;
            string cmdStr = "INSERT INTO stageGRS(grsDesc,grsCode,grsType,grsValue,grsYRMO,grsCoverage) "
                                + "VALUES(@desc,@code,@type,@value,@yrmo,@coverage)";
            cmd = new SqlCommand(cmdStr, conn);
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.Parameters.Add("@desc", SqlDbType.VarChar);
                cmd.Parameters.Add("@code", SqlDbType.VarChar);
                cmd.Parameters.Add("@type", SqlDbType.VarChar);
                cmd.Parameters.Add("@value", SqlDbType.VarChar);
                cmd.Parameters.Add("@yrmo", SqlDbType.VarChar);
                cmd.Parameters.Add("@coverage", SqlDbType.VarChar);

                cmd.Parameters["@desc"].Value = _desc;
                cmd.Parameters["@code"].Value = _code;
                cmd.Parameters["@type"].Value = _type;
                cmd.Parameters["@value"].Value = _value;
                cmd.Parameters["@yrmo"].Value = _yrmo;
                cmd.Parameters["@coverage"].Value = _coverage;

                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                cmdStr = "DELETE FROM stageGRS";
                cmd = new SqlCommand(cmdStr, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                throw ex;
            }
            finally
            {                
                conn.Close();
            }

        }

        public void storeADP(string _text1, string _plan, string _desc, string _div, string _bensts, string _covlvl, string _empno, string _year,string _month)
        {
            string cmdStr = "INSERT INTO [stage_ADP] "
                              + "([benefit_type],[plan1],[description],[division],[bensts],[covlvl],[empno],[year1],[month1])"
                                + " VALUES (@benefit, @plan, @desc, @div, @bensts, @covlvl, @empno, @year, @month)";
           SqlCommand cmd = new SqlCommand(cmdStr, conn);
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.Parameters.Add("@benefit", SqlDbType.VarChar);
                cmd.Parameters["@benefit"].Value = _text1;
                cmd.Parameters.Add("@plan", SqlDbType.Char);
                cmd.Parameters["@plan"].Value = _plan;
                cmd.Parameters.Add("@desc", SqlDbType.VarChar);
                cmd.Parameters["@desc"].Value = _desc;
                cmd.Parameters.Add("@div", SqlDbType.VarChar);
                cmd.Parameters["@div"].Value = _div;
                cmd.Parameters.Add("@bensts", SqlDbType.Char);
                cmd.Parameters["@bensts"].Value = _bensts;
                cmd.Parameters.Add("@covlvl", SqlDbType.Char);
                cmd.Parameters["@covlvl"].Value = _covlvl;
                cmd.Parameters.Add("@empno", SqlDbType.Int);
                cmd.Parameters["@empno"].Value = Int32.Parse(_empno);
                cmd.Parameters.Add("@year", SqlDbType.VarChar);
                cmd.Parameters["@year"].Value = _year;
                cmd.Parameters.Add("@month", SqlDbType.Char);
                cmd.Parameters["@month"].Value = _month;
                
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                cmdStr = "DELETE FROM stage_ADP";
                cmd = new SqlCommand(cmdStr, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

        }

        public void insertBOARecords(string _chq, decimal _amnt, DateTime _pdate, string _bref, string _yrmo, string _pflag)
        {           
            string cmdStr;            
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmdStr = "INSERT INTO BOAStatement (boaChqNo,boaYRMO,boaSource,boaAmnt,boaPosted_dt,boaBankRef,boaFlag) VALUES (@chq,@yrmo,'BOA', @amnt, @pdate, @bref,@pflag)";
                cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@chq", SqlDbType.Int);
                cmd.Parameters["@chq"].Value = Int32.Parse(_chq);
                cmd.Parameters.Add("@yrmo", SqlDbType.VarChar);
                cmd.Parameters["@yrmo"].Value = _yrmo;
                cmd.Parameters.Add("@amnt", SqlDbType.Money);
                cmd.Parameters["@amnt"].Value = _amnt;
                cmd.Parameters.Add("@pdate", SqlDbType.DateTime);
                cmd.Parameters["@pdate"].Value = _pdate;
                cmd.Parameters.Add("@bref", SqlDbType.VarChar);
                cmd.Parameters["@bref"].Value = _bref;
                cmd.Parameters.Add("@pflag", SqlDbType.VarChar);
                if (_pflag != null)
                {
                    cmd.Parameters["@pflag"].Value = _pflag;
                }
                else
                {
                    cmd.Parameters["@pflag"].Value = SqlString.Null;
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                cmdStr = "DELETE FROM BOAStatement WHERE boaYRMO = " + _yrmo;
                cmd = new SqlCommand(cmdStr, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        public List<int> getMedPartCols()
        {
            List<int> Cols = new List<int>();
            string cmdStr = "SELECT grsID FROM stageGRS where grsDesc = 'TOTAL BY MEDPART' ORDER BY grsID ASC";
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cols.Add(Int32.Parse(dr[0].ToString()));
                }
                dr.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();

            }
            return Cols;
        }

        public string getGRSCode(int _endID)
        {
            string _grsCode;
            string cmdStr1 = "SELECT grsCode FROM stageGRS where grsID = " + _endID + " ORDER BY grsID ASC";
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(cmdStr1, conn);
                _grsCode = cmd.ExecuteScalar().ToString();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return _grsCode;
        }

        public List<int> getStateCols(int _startID, int _endID)
        {
            List<int> Cols = new List<int>();
            string cmdStr = "SELECT grsID FROM stageGRS WHERE grsDesc = 'TOTAL BY STATE' AND grsID > " + _startID + " AND grsID < " + _endID + "ORDER BY grsID ASC";
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cols.Add(Int32.Parse(dr[0].ToString()));
                }
                dr.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();

            }
            return Cols;
        }

        public DataSet getGRSRows(int _startID, int _endID)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            string cmdStr = "SELECT * FROM stageGRS where grsID > " + _startID + " AND grsID < " + _endID;
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(cmdStr, conn);
                da.SelectCommand = cmd;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
            return ds;
        }

        public void InsertHCGRS(string _planCode, string _stateCode, string _tierCode, string _desc, int _count, string _yrmo)
        {
            string yrmo = Convert.ToDateTime(_yrmo.Insert(4, "/")).ToShortDateString();
            decimal _rate = 0;
            string _stateType = "N";
            string _medType = "N";
            SqlCommand cmd = null;
            if (_stateCode.Contains("CA"))
            {
                _stateType = "C";
            }
            if (_desc.Contains("MEDICARE"))
            {
                _medType = "M";
            }
            string _breakCD = _stateType + _medType;
            int _plhrid = 0;
            int _anthcdid = 0;
            string _plandesc = null;
            bool _hflag = false;
            string _cmdStr = "SELECT plhr_id,plhr_anthcd_id,plhr_plandesc,plhr_CompanyRate FROM AnthPlanhier WHERE plhr_plandesc NOT LIKE '%SSP%' AND plhr_plancd = '" + _planCode + "' AND plhr_tiercd = '" + _tierCode + "' AND plhr_breakCd = '" + _breakCD + "' AND '" + yrmo +"' >= plhr_eff_yrmo";
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(_cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    if (dr[3] != DBNull.Value)
                    {
                        _plhrid = dr.GetInt32(0);
                        _anthcdid = dr.GetInt32(1);
                        _plandesc = dr.GetString(2);
                        _rate = dr.GetDecimal(3);
                    }
                    else
                    {
                        _hflag = true;
                    }
                    
                }
                dr.Close();
                cmd.Dispose();
                if (_hflag)
                {
                    _cmdStr = "SELECT plhr_id,plhr_anthcd_id,plhr_plandesc,plhr_CompanyRate FROM AnthPlanhier_History WHERE plhr_plandesc NOT LIKE '%SSP%' AND plhr_plancd = '" + _planCode + "' AND plhr_tiercd = '" + _tierCode + "' AND plhr_breakCd = '" + _breakCD + "' AND '" + yrmo + "' >= ("
                                + "SELECT max(plhr_eff_yrmo) FROM AnthPlanhier_History WHERE plhr_plandesc NOT LIKE '%SSP%' AND plhr_plancd = '" + _planCode + "' AND plhr_tiercd = '" + _tierCode + "' AND plhr_breakCd = '" + _breakCD + "')";
                    try
                    {
                        if (conn == null || conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        cmd = new SqlCommand(_cmdStr, conn);
                        SqlDataReader dr1 = cmd.ExecuteReader();
                        if (dr1.Read())
                        {
                            if (dr1[3] != DBNull.Value)
                            {
                                _plhrid = dr1.GetInt32(0);
                                _anthcdid = dr1.GetInt32(1);
                                _plandesc = dr1.GetString(2);
                                _rate = dr1.GetDecimal(3);
                            }
                            else
                            {
                                throw (new Exception("Rates are missing! for given yrmo"));
                            }

                        }
                        dr.Close();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                               
                if (_anthcdid != 0 && !(_plandesc.Contains("INTERNATIONAL")))
                {
                    _cmdStr = "INSERT INTO Headcount (hdct_anthcd_id, hdct_plhr_id, hdct_source, hdct_yrmo,hdct_count,hdct_rate) VALUES (" + _anthcdid + "," + _plhrid + ",'GRS','" + _yrmo + "'," + _count + "," + _rate +")";
                    cmd = new SqlCommand(_cmdStr, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();                
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }

        }

        public void InsertHMOGRS(string _plancode, string _stateCode, string _tierCode, int _count,string _yrmo)
        {
            string _cmdstr = "INSERT INTO billing_HMO "
                            + "(hmo_yrmo, hmo_plancd, hmo_tiercd, hmo_statecd, hmo_source, hmo_count) "
                            + " VALUES (@yrmo,@plan,@tier,@state,@src,@cnt) ";
            SqlCommand cmd = null;
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand(_cmdstr, conn);
                cmd.Parameters.AddWithValue("@yrmo", _yrmo);
                cmd.Parameters.AddWithValue("@plan", _plancode);
                cmd.Parameters.AddWithValue("@tier", _tierCode);
                cmd.Parameters.AddWithValue("@state", _stateCode);
                cmd.Parameters.AddWithValue("@src", "GRS");
                cmd.Parameters.AddWithValue("@cnt", _count);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
    }
}

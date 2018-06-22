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
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for parseDataSet
/// </summary>
/// 
namespace EBA.Desktop
{
    public class parseDataSet
    {
        private string connStr;
        private SqlConnection connect = null;
        private SqlCommand command = null;
        public parseDataSet()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            connect = new SqlConnection(connStr);
        }

        //HTH Report
        public void parseHTH(DataSet ds,string _yrmo)
        {
            insertHTH(ds, "HTH", "hthTable",_yrmo);
        }


        private void insertHTH(DataSet ds, string _src, string _tableName, string _yrmo)
        {
            string _ssn;
            string _name;
            string _tiercd;
            DateTime _effdt;
            decimal _rate;
            decimal _frate;
            int _count = 0;
            int plhr_anbtcd_id;
            int plhr_id;

            String[] colsH = new String[] { "ssno", "name", "tiercd", "effdt","rate" };

            for (int j = 0; j < colsH.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if(dc.ColumnName.ToString().Contains(colsH[j]))
                    {
                        _count ++;
                    }
                }
            }
            if (_count != colsH.Length)
            {
                throw (new Exception("Cannot find required columns in HTH File"));
            }  
            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                int ctrEmpty = 0;
                for (int k = 0; k < ds.Tables[_tableName].Columns.Count; k++)
                {
                    if (ds.Tables[_tableName].Rows[i][k].ToString().Trim().Equals(""))
                    {
                        ctrEmpty++;
                    }
                }

                if (ctrEmpty == ds.Tables[_tableName].Columns.Count)
                    continue;

                _ssn = ds.Tables[_tableName].Rows[i][2].ToString();
                _name = ds.Tables[_tableName].Rows[i][3].ToString();
                _tiercd = ds.Tables[_tableName].Rows[i][4].ToString();
                _effdt = DateTime.Parse(ds.Tables[_tableName].Rows[i][5].ToString());
                _rate = Decimal.Parse(ds.Tables[_tableName].Rows[i][6].ToString(), System.Globalization.NumberStyles.Currency);
                string py = _yrmo.Substring(0, 4);
                decimal[] temparr = calcEBAplhr(_yrmo,py, _tiercd, _rate);
                plhr_id = Convert.ToInt32(temparr[0]);
                plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                ImportDAL iObj = new ImportDAL();
                iObj.insertHTH(plhr_id, plhr_anbtcd_id, _yrmo, _ssn, _name, _effdt,_frate);
            } 
        }

        private decimal[] calcEBAplhr(string yrmo,string py, string tiercd, decimal rate)
        {           
            SqlDataReader dr = null;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_plandesc = 'INTERNATIONAL'", connect);
            dr = command.ExecuteReader();
            decimal[] ids = new decimal[3];
            if (dr.Read())
            {
                ids[0] = Convert.ToDecimal(dr[0]);
                ids[1] = Convert.ToDecimal(dr[1]);               
            }
            else
            {
                dr.Close();
                throw (new Exception("Cannot match tiercd - '" + tiercd + "' from the report"));
            }
            dr.Close();
            ids[2] = getRatebyEdate(yrmo, "INTL", py, tiercd);
            connect.Close();
            return ids;
        }

        //Insert Medicare Report
        public int parseMC(DataSet ds, string _yrmo)
        {
           int count =  insertMC(ds, "RET_M", "mcTable", _yrmo);
           return count;
        }


        private int insertMC(DataSet ds, string _src, string _tableName, string _yrmo)
        {
            string _state;
            string _plancd;
            string _tiercd;
            string _ptype = null;
            int _eCount;
            decimal _frate;
            int _count = 0;
            int _count1 = 0;
            int _counter = 0;
            int plhr_id;
            int plhr_anbtcd_id;
            string _breakCd = "NM";

            
            String[] colsH = new String[] { "state", "plan cd","ptype","tier cd", "count" };
            String[] colsH1 = new String[] { "state", "plan cd", "tier cd", "count" };
            for (int j = 0; j < colsH.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH[j]))
                    {
                        _count++;
                    }
                }
            }

            for (int j = 0; j < colsH1.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH1[j]))
                    {
                        _count1++;
                    }
                }
            }

            if (_count == colsH.Length)
            {
                for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                {                

                    _state = ds.Tables[_tableName].Rows[i][0].ToString();
                    _plancd = ds.Tables[_tableName].Rows[i][1].ToString();               
                    _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                    if (_tiercd.Contains("RASN"))
                    {
                        continue;
                    }
                    _ptype = ds.Tables[_tableName].Rows[i][3].ToString();
                    _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][4].ToString());

                    if (_state.Trim().Equals("CA"))
                    {
                        _breakCd = "CM";
                    }
                    else
                    {
                        _breakCd = "NM";
                    }
                   
                    string py = _yrmo.Substring(0, 4);
                    ImportDAL iObj = new ImportDAL();
                    if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                    {
                        decimal[] temparr = calcMCplhr(_yrmo, py, _plancd, _ptype, _tiercd, _breakCd);
                        plhr_id = Convert.ToInt32(temparr[0]);
                        plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                        _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                        
                        if (plhr_anbtcd_id != 0)
                        {
                            iObj.insertMCHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                            _counter++;
                        }
                    }
                    else
                    {
                        iObj.insertMCHMO(_plancd, _state, _tiercd, _eCount, _yrmo);
                        _counter++;
                    }                    
                }
            }
            else
            {
                if (_count1 == colsH1.Length)
                {
                    for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                    {
                        _state = ds.Tables[_tableName].Rows[i][0].ToString();
                        _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                        _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                        if (_tiercd.Contains("RASN"))
                        {
                            continue;
                        }                       
                        _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][3].ToString());

                        if (_state.Trim().Equals("CA"))
                        {
                            _breakCd = "CM";
                        }
                        else
                        {
                            _breakCd = "NM";
                        }

                        string py = _yrmo.Substring(0, 4);
                        ImportDAL iObj = new ImportDAL();
                        if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                        {
                            decimal[] temparr = calcMCplhr(_yrmo, py, _plancd, _tiercd, _breakCd);
                            plhr_id = Convert.ToInt32(temparr[0]);
                            plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                            _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                            
                            if (plhr_anbtcd_id != 0)
                            {
                                iObj.insertMCHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                                _counter++;
                            }
                        }
                        else
                        {
                            iObj.insertMCHMO(_plancd, _state, _tiercd, _eCount, _yrmo);
                            _counter++;
                        }
                        
                    }
                }
                else
                {
                    throw (new Exception("Cannot find required columns in Medicare File"));
                }
            }            
               
            return _counter;
        }
        private bool DeleteMCNM(string _yrmo)
        {
            //This procedure will delete existing MCNM records for the  yrmo when reimporting the dat for the yrmo
           //10-9-2009 R.A
            string _cmdstr = "DELETE FROM billing_HMO "
                               + " WHERE hmo_yrmo = @yrmo";
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.ExecuteNonQuery();
                return true;
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

        private int insertMCNM(DataSet ds, string _src, string _tableName, string _yrmo)
        {
            //This procedure will replace the existing procedures
            // InsertMC (medicare) and InsertNM (non medicare) as WW will be producing one single
            //output file instead of 2 files //9-2-2009 R.A
            //modified the procedure insertMC
            string _state;
            string _plancd;
            string _tiercd;
            string _ptype = null;
            string _medicareflg;
            int _eCount;
            decimal _frate;
            int _count = 0;
            int _count1 = 0;
            int _counter = 0;
            int plhr_id;
            int plhr_anbtcd_id;
            string _breakCd_NM = "NN";
            string _breakCd_MC = "NM";

            String[] colsH = new String[] { "State", "PRISM Plan Code", "Person Type", "Tier Code", "Count","Medicare Flag" };
            String[] colsH1 = new String[] { "state", "plan cd", "tier cd", "count" };
            for (int j = 0; j < colsH.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH[j]))
                    {
                        _count++;
                    }
                }
            }

            for (int j = 0; j < colsH1.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH1[j]))
                    {
                        _count1++;
                    }
                }
            }

            if (_count == colsH.Length)
            {
                for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                {
                    //Check the Medicare flag in the file

                    if (ds.Tables[_tableName].Rows[i][4].ToString() == "M")
                    {

                        _state = ds.Tables[_tableName].Rows[i][0].ToString();
                        _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                        _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                        if (_tiercd.Contains("RASN"))
                        {
                            continue;
                        }
                        _ptype = ds.Tables[_tableName].Rows[i][3].ToString();
                        _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][5].ToString());

                        if (_state.Trim().Equals("CA"))
                        {
                            _breakCd_MC = "CM";
                        }
                        else
                        {
                            _breakCd_MC = "NM";
                        }

                        string py = _yrmo.Substring(0, 4);
                        ImportDAL iObj = new ImportDAL();
                        if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                        {
                            decimal[] temparr = calcMCplhr(_yrmo, py, _plancd, _ptype, _tiercd, _breakCd_MC);
                            plhr_id = Convert.ToInt32(temparr[0]);
                            plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                            _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);

                            if (plhr_anbtcd_id != 0)
                            {
                                iObj.insertMCHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                                _counter++;
                            }
                        }
                        else
                        {
                            iObj.insertMCNMHMO(_plancd, _state, _tiercd, _eCount, _yrmo,"RET_M");
                            _counter++;
                        }
                    }
                    else   //if the medicare flag is empty i.e Non Medicare (NM)
                        //this will be executed if the medicare flag is empty
                    {
                        _state = ds.Tables[_tableName].Rows[i][0].ToString();
                        _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                        _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                        if (_tiercd.Contains("RASN"))
                        {
                            continue;
                        }
                        _ptype = ds.Tables[_tableName].Rows[i][3].ToString();
                        _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][5].ToString());
                        if (_state.Trim().Equals("CA"))
                        {
                            _breakCd_NM = "CN";
                        }
                        else
                        {
                            _breakCd_NM = "NN";
                        }
                        string py = _yrmo.Substring(0, 4);
                        ImportDAL iObj = new ImportDAL();
                        if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                        {
                            decimal[] temparr = calcNMplhr(_yrmo, py, _plancd, _ptype, _tiercd, _breakCd_NM);
                            plhr_id = Convert.ToInt32(temparr[0]);
                            plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                            _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                            if (plhr_anbtcd_id != 0)
                            {
                                iObj.insertNMHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                                _counter++;
                            }
                        }
                        else
                        {
                            iObj.insertMCNMHMO(_plancd, _state, _tiercd, _eCount, _yrmo,"RET_NM");
                            _counter++;
                        }
                    }
                }
            }
            else
            {
                if (_count1 == colsH1.Length)
                {
                    for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                    {

                        _state = ds.Tables[_tableName].Rows[i][0].ToString();
                        _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                        _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                        if (_tiercd.Contains("RASN"))
                        {
                            continue;
                        }
                        _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][4].ToString());

                        if (_state.Trim().Equals("CA"))
                        {
                            _breakCd_MC = "CM";
                        }
                        else
                        {
                            _breakCd_MC = "NM";
                        }

                        string py = _yrmo.Substring(0, 4);
                        ImportDAL iObj = new ImportDAL();
                        if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                        {
                            decimal[] temparr = calcMCplhr(_yrmo, py, _plancd, _tiercd, _breakCd_MC);
                            plhr_id = Convert.ToInt32(temparr[0]);
                            plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                            _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);

                            if (plhr_anbtcd_id != 0)
                            {
                                iObj.insertMCHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                                _counter++;
                            }
                        }
                        else
                        {
                            iObj.insertMCHMO(_plancd, _state, _tiercd, _eCount, _yrmo);
                            _counter++;
                        }

                    }
                }
                else
                {
                    throw (new Exception("Cannot find required columns in Medicare File"));
                }
            }

            return _counter;
        }

        private decimal[] calcMCplhr(string yrmo,string py,string plancd,string ptype, string tiercd, string breakcd)
        { 
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            if (ptype.Equals("SSP") || ptype.Equals("SEP") || ptype.Equals("SCP") || ptype.Equals("SCEP"))
            {
                command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_plandesc LIKE '%SSP%' AND plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            }
            else
            {
                command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            }
            SqlDataReader dr = command.ExecuteReader();
            decimal[] ids = new decimal[3] { 0, 0, 0 };
            if (dr.Read())
            {
                ids[0] = Convert.ToDecimal(dr[0]);
                ids[1] = Convert.ToDecimal(dr[1]);  
            }
            else
            {
                dr.Close();
                //throw (new Exception("Cannot match tier code - '" + tiercd + "'  plan code - '" + plancd + "' from the report"));
                return ids;
            }
            dr.Close();
            ids[2] = getRatebyEdate(yrmo, "RET", py, tiercd,breakcd,plancd);
            connect.Close();
            return ids;
        }

        private decimal[] calcMCplhr(string yrmo, string py, string plancd,string tiercd, string breakcd)
        {
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
           
            command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            
            SqlDataReader dr = command.ExecuteReader();
            decimal[] ids = new decimal[3] { 0, 0, 0 };
            if (dr.Read())
            {
                ids[0] = Convert.ToDecimal(dr[0]);
                ids[1] = Convert.ToDecimal(dr[1]);
            }
            else
            {
                dr.Close();
                //throw (new Exception("Cannot match tier code - '" + tiercd + "'  plan code - '" + plancd + "' from the report"));
                return ids;
            }
            dr.Close();
            ids[2] = getRatebyEdate(yrmo, "RET", py, tiercd, breakcd, plancd);
            connect.Close();
            return ids;
        }

        //Insert Non Medicare Report
        public int parseNM(DataSet ds, string _yrmo)
        {
            int count = insertNM(ds, "RET_NM", "nmTable", _yrmo);
            return count;
        }
        //Insert Medicare / Non Medicare Report 9-2-2009
        public int parseMCNM(DataSet ds, string _yrmo)
        {
            if (DeleteMCNM(_yrmo))
            {
                int count = insertMCNM(ds, "", "mcnmTable", _yrmo);
                return count;
            }
            else
            {
                return 0;
            }
        }

        private int insertNM(DataSet ds, string _src, string _tableName, string _yrmo)
        {
            string _state;
            string _plancd;
            string _tiercd;
            string _ptype = null;
            int _eCount;
            decimal _rate;
            decimal _frate;
            int _count = 0;
            int _count1 = 0;
            int _counter = 0;
            int plhr_anbtcd_id;
            int plhr_id;
            string _breakCd = "NN";

            String[] colsH = new String[] { "state", "plan cd", "ptype", "tier cd", "count" };
            String[] colsH1 = new String[] { "state", "plan cd","tier cd", "count" };
            for (int j = 0; j < colsH.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH[j]))
                    {
                        _count++;
                    }
                }
            }

            for (int j = 0; j < colsH1.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(colsH1[j]))
                    {
                        _count1++;
                    }
                }
            }
            //check column length
            if (_count == colsH.Length)

            {
                for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                {
                    
                    _state = ds.Tables[_tableName].Rows[i][0].ToString();
                    _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                    _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                    if (_tiercd.Contains("RASN"))
                    {
                        continue;
                    }
                    _ptype = ds.Tables[_tableName].Rows[i][3].ToString();
                    _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][4].ToString());
                    if (_state.Trim().Equals("CA"))
                    {
                        _breakCd = "CN";
                    }
                    else
                    {
                        _breakCd = "NN";
                    }
                    string py = _yrmo.Substring(0, 4);
                    ImportDAL iObj = new ImportDAL();
                    if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                    {
                        decimal[] temparr = calcNMplhr(_yrmo, py, _plancd, _ptype, _tiercd, _breakCd);
                        plhr_id = Convert.ToInt32(temparr[0]);
                        plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                        _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                        if (plhr_anbtcd_id != 0)
                        {
                            iObj.insertNMHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                            _counter++;
                        }
                    }
                    else
                    {
                        iObj.insertNMHMO(_plancd, _state, _tiercd, _eCount, _yrmo);
                        _counter++;
                    }
                }
            }
            else
            {
                if (_count1 == colsH1.Length)
                {
                    for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
                    {
                        _state = ds.Tables[_tableName].Rows[i][0].ToString();
                        _plancd = ds.Tables[_tableName].Rows[i][1].ToString();
                        _tiercd = ds.Tables[_tableName].Rows[i][2].ToString();
                        if (_tiercd.Contains("RASN"))
                        {
                            continue;
                        }                        
                        _eCount = Int32.Parse(ds.Tables[_tableName].Rows[i][3].ToString());
                        if (_state.Trim().Equals("CA"))
                        {
                            _breakCd = "CN";
                        }
                        else
                        {
                            _breakCd = "NN";
                        }
                        string py = _yrmo.Substring(0, 4);
                        ImportDAL iObj = new ImportDAL();
                        if (_plancd.Contains("P0") || _plancd.Contains("P1") || _plancd.Contains("P2") || _plancd.Contains("P3") || _plancd.Contains("P4") || _plancd.Contains("P5"))
                        {
                            decimal[] temparr = calcNMplhr(_yrmo, py, _plancd, _tiercd, _breakCd);
                            plhr_id = Convert.ToInt32(temparr[0]);
                            plhr_anbtcd_id = Convert.ToInt32(temparr[1]);
                            _frate = Decimal.Parse(temparr[2].ToString(), System.Globalization.NumberStyles.Currency);
                            if (plhr_anbtcd_id != 0)
                            {
                                iObj.insertNMHeadcounts(plhr_anbtcd_id, plhr_id, _yrmo, _eCount, _frate);
                                _counter++;
                            }
                        }
                        else
                        {
                            iObj.insertNMHMO(_plancd, _state, _tiercd, _eCount, _yrmo);
                            _counter++;
                        }
                    }
                }
                else
                {
                    throw (new Exception("Cannot find required columns in Non Medicare File"));
                }
            }            
           
            return _counter;
        }

        private decimal[] calcNMplhr(string yrmo,string py, string plancd,string ptype, string tiercd, string breakcd)
        {
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            if (ptype.Equals("SSP") || ptype.Equals("SEP") || ptype.Equals("SCP") || ptype.Equals("SCEP"))
            {
                command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_plandesc LIKE '%SSP%' AND plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            }
            else
            {
                command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            }
            SqlDataReader dr = command.ExecuteReader();
            decimal[] ids = new decimal[3] { 0, 0, 0 };
            if (dr.Read())
            {
                ids[0] = Convert.ToDecimal(dr[0]);
                ids[1] = Convert.ToDecimal(dr[1]);
            }
            else
            {
                dr.Close();
                //throw (new Exception("Cannot match tier code - '" + tiercd + "'  plan code - '" + plancd + "' from the report"));
                return ids;
            }
            dr.Close();
            ids[2] = getRatebyEdate(yrmo, "RET", py, tiercd, breakcd,plancd);
            connect.Close();
            return ids;
        }

        private decimal[] calcNMplhr(string yrmo, string py, string plancd,string tiercd, string breakcd)
        {
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            } 
           
            command = new SqlCommand("SELECT plhr_id,plhr_anthcd_id FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "'", connect);
            
            SqlDataReader dr = command.ExecuteReader();
            decimal[] ids = new decimal[3] { 0, 0, 0 };
            if (dr.Read())
            {
                ids[0] = Convert.ToDecimal(dr[0]);
                ids[1] = Convert.ToDecimal(dr[1]);
            }
            else
            {
                dr.Close();
                //throw (new Exception("Cannot match tier code - '" + tiercd + "'  plan code - '" + plancd + "' from the report"));
                return ids;
            }
            dr.Close();
            ids[2] = getRatebyEdate(yrmo, "RET", py, tiercd, breakcd, plancd);
            connect.Close();
            return ids;
        }
        //Anthem Bill 
        public void parseAnthem(DataSet ds,string _yrmo)
        {
            insertAnthem(ds, "ANTH_INTL", "intlTable", "INTERNATIONAL", _yrmo);
            insertAnthem(ds, "ANTH_ACT", "actTable", "DOMESTIC", _yrmo);
            insertAnthem(ds, "ANTH_RET", "retTable", "DOMESTIC", _yrmo);
            insertAnthem(ds, "ANTH_COB", "cobTable", "DOMESTIC", _yrmo);
            insertAnthem(ds, "ANTH_EAP", "eapTable", "DOMESTIC", _yrmo);
        }

        private void insertAnthem(DataSet ds, string _src, string _tableName, string _type, string _yrmo)
        {
            string _adjStart = "ELIGIBILITY ADJUSTMENTS";
            string _adjStop = "Eligibility Adjustments Subtotal";
            string _memStart = "MEMBERSHIP DETAIL";
            string _memStop = "Membership Detail Subtotal";
            bool _adjFlag = true;
            bool _memFlag = true;
            DataRow dr;
            DataColumn dc;
            string _colName;
            int _adjstartRow = 0, _adjendRow = 0, _memstartRow = 0, _memendRow = 0;

            int _sheetstart = 0;

            for (int i = _sheetstart; i <= ds.Tables[_tableName].Rows.Count - 1; i++)
            {
                dr = ds.Tables[_tableName].Rows[i];
                dc = ds.Tables[_tableName].Columns[0];
                _colName = dr[dc, DataRowVersion.Current].ToString();
                if (_colName.Trim().Equals(_adjStart) && _adjFlag)
                {
                    _adjstartRow = i + 1;
                    _adjendRow = (ds.Tables[_tableName].Rows.Count - 1);
                    for (int j = _adjstartRow; j <= _adjendRow; j++)
                    {
                        dr = ds.Tables[_tableName].Rows[j];
                        if (dr[0].ToString().Equals(""))
                        {
                            foreach (DataColumn dc1 in ds.Tables[_tableName].Columns)
                            {
                                if (dr[dc1].ToString().Trim().Equals(_adjStop))
                                {
                                    _adjendRow = j;
                                    _adjFlag = false;
                                    i = j + 1;
                                }
                            }
                        }
                    }
                }
                if (_colName.Trim().Equals(_memStart) && _memFlag)
                {
                    _memstartRow = i + 1;
                    _memendRow = (ds.Tables[_tableName].Rows.Count - 1);
                    for (int j = _memstartRow; j <= _memendRow; j++)
                    {
                        dr = ds.Tables[_tableName].Rows[j];
                        if (dr[0].ToString().Equals(""))
                        {
                            foreach (DataColumn dc1 in ds.Tables[_tableName].Columns)
                            {
                                if (dr[dc1].ToString().Trim().Equals(_memStop))
                                {
                                    _memendRow = j;
                                    _memFlag = false;
                                    i = j + 1;
                                }
                            }
                        }
                    }
                }

            }
            insertAnthAdjustement(ds, _src, _tableName, _adjstartRow, _adjendRow, _type, _yrmo);
            insertAnthMembership(ds, _src, _tableName, _memstartRow, _memendRow, _type, _yrmo);
        }

        private void insertAnthAdjustement(DataSet ds, string _src, string _tableName, int _start, int _end, string _type,string _yrmo)
        {
            DataRow dr, dr1;
            DataColumn dc, dc1;
            int _count = 0;
            String[] colsAdj = new String[] { "ID No.", "Subscriber Name", "Cont Type", "Grp. No./Suffix", "From Date", "To Date", "Mo/Da", "Calc. Rate", "Prem. Adj", "Code" };
            string ssn;
            string name;
            DateTime frmdt;
            DateTime todt;
            string ctype;
            string grp;
            string mnths;
            decimal calcrate;
            decimal premadj;
            string code;

            
            for (int i = 0; i <= (ds.Tables[_tableName].Columns.Count - 1); i++)
            {
                object _col = ds.Tables[_tableName].Rows[_start][i];
                for (int j = 0; j < colsAdj.Length; j++)
                {
                    if (_col.ToString().Contains(colsAdj[j]))
                    {
                        _count++;
                    }
                }
            }  
            if (_count != colsAdj.Length)
            {
                throw (new Exception("Cannot find required columns in Adjustments of " + _type));
            }
            
            for (int i = _start + 1; i < _end; i++)
            {
                ssn = ds.Tables[_tableName].Rows[i][0].ToString();
                if (ssn.Equals(""))
                {
                    break;
                }
                ssn = ssn.Replace("-", "");
                name = ds.Tables[_tableName].Rows[i][3].ToString();
                ctype = ds.Tables[_tableName].Rows[i][4].ToString();
                grp = ds.Tables[_tableName].Rows[i][5].ToString().Substring(6);
                frmdt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][6].ToString());
                todt = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][7].ToString());
                mnths = ds.Tables[_tableName].Rows[i][8].ToString();
                calcrate = Decimal.Parse(ds.Tables[_tableName].Rows[i][9].ToString(), System.Globalization.NumberStyles.Currency);
                premadj = Decimal.Parse(ds.Tables[_tableName].Rows[i][10].ToString(), System.Globalization.NumberStyles.Currency);
                code = ds.Tables[_tableName].Rows[i][11].ToString();

                //workorder #1 - to handle S+DEP code as S+DE for rates
                if (ctype.Equals("S+DEP") || ctype.Equals("S+DEPS"))
                {
                    ctype = "S+DE";
                }

                int plhrId = calcplhr(ctype, grp);

                ImportDAL dObj = new ImportDAL();
                dObj.insertAdjData(plhrId, _yrmo, _src, ssn, name, frmdt, todt, mnths, calcrate, premadj, code);

            }
                        
            
        }

        private void insertAnthMembership(DataSet ds, string _src, string _tableName, int _start, int _end, string _type, string _yrmo)
        {
            DataRow dr, dr1;
            DataColumn dc, dc1;
            int _count = 0;
            String[] colsDtl = new String[] { "ID No.", "Subscriber Name", "Grp. No./Suffix", "Cont. Type" };
            string ssn;
            string name;            
            string ctype;
            string grp;
            decimal premadj;

            for (int i = 0; i <= (ds.Tables[_tableName].Columns.Count - 1); i++)
            {
                object _col = ds.Tables[_tableName].Rows[_start][i];
                for (int j = 0; j < colsDtl.Length; j++)
                {
                    if (_col.ToString().Contains(colsDtl[j]))
                    {
                        _count++;
                    }
                }
            }         
            if (_count != colsDtl.Length)
            {
                throw (new Exception("Cannot find required columns in Membership Details of " + _type));
            }
            
            for (int i = _start + 1; i < _end; i++)
            {
                ssn = ds.Tables[_tableName].Rows[i][0].ToString();
                ssn = ssn.Replace("-", "");
                name = ds.Tables[_tableName].Rows[i][3].ToString();
                grp = ds.Tables[_tableName].Rows[i][5].ToString().Substring(6);
                ctype = ds.Tables[_tableName].Rows[i][8].ToString();
                premadj = Decimal.Parse(ds.Tables[_tableName].Rows[i][10].ToString(), System.Globalization.NumberStyles.Currency);

                //workorder #1 - to handle S+DEP code as S+DE for rates
                if (ctype.Equals("S+DEP") || ctype.Equals("S+DEPS"))
                {
                    ctype = "S+DE";
                }

                int plhrId = calcplhr(ctype, grp);
                ImportDAL dObj = new ImportDAL();
                if (premadj != 0)
                {
                    dObj.insertDtlData(plhrId, _yrmo, _src, ssn, name, premadj);
                }
            }
            
            
        }

        private int calcplhr(string _ctype, string _grp)
        {
            int pid;
            string _cmdStr = "SELECT anthcd_id FROM AnthCodes WHERE anthcd_plancd = '" + _grp + "' AND anthcd_covgcd = '" + _ctype + "'";
            command = new SqlCommand(_cmdStr, connect);
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            object temp = command.ExecuteScalar();
            if (temp.Equals(null))
            {
                throw(new Exception("Couldnt find planhier data for plancode: " + _grp + " and coverage type: " + _ctype));
            }
            pid = Int32.Parse(temp.ToString());
            connect.Close();
            return pid;
        }

        //Insert Claims Report
        public int parseClaims(DataSet ds, string _yrmo)
        {
            int count = insertClaims(ds,"claimTable", _yrmo);
            return count;
        }


        private int insertClaims(DataSet ds, string _tableName, string _yrmo)
        {
            string caseID;
            string group;
            string suffix;
            string claimID;
            string subID;
            string membCd;
            string name;
            DateTime servFrom;
            DateTime servThru;            
            Decimal claimsPaid;
            string claimType;
            DateTime datePaid;
            string checkno;
            int _count = 0;
            int _counter = 0;

            int _ncaStart = 0;
            ImportDAL dObj = new ImportDAL();

            //String[] colsClms = new String[] { "CASE_", "SUFFIX", "CLAIM_ID", "SUB_ID", "MEM_CODE", "SERV_FROM", "SERV_THRU", "CLM_PAID", "CL_TYPE", "DATE_PAID" };
            
            //for (int j = 0; j < colsClms.Length; j++)
            //{
            //    foreach (DataColumn dc in ds.Tables[_tableName].Columns)
            //    {
            //        if (dc.ColumnName.ToString().Contains(colsClms[j]))
            //        {
            //            _count++;
            //        }
            //    }
            //}
           
            
            //if (_count != colsClms.Length)
            //{
            //    throw (new Exception("Cannot find required columns in Claims File"));
            //}
            _counter = 0;            
            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                int ctrEmpty = 0;
                for (int k = 0; k < ds.Tables[_tableName].Columns.Count; k++)
                {
                    if (ds.Tables[_tableName].Rows[i][k].ToString().Trim().Equals(""))
                    {
                        ctrEmpty++;
                    }
                }

                if (ctrEmpty == ds.Tables[_tableName].Columns.Count)
                    continue;

                caseID = ds.Tables[_tableName].Rows[i][0].ToString();
                if (caseID.Equals(""))
                {
                    continue;
                }
                group = caseID.Substring(0, caseID.Length - 1);
                suffix = ds.Tables[_tableName].Rows[i][1].ToString();
                claimID = ds.Tables[_tableName].Rows[i][2].ToString();
                subID = ds.Tables[_tableName].Rows[i][3].ToString();
                membCd = ds.Tables[_tableName].Rows[i][4].ToString();
                name = ds.Tables[_tableName].Rows[i][5].ToString();
                servFrom = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][6].ToString());
                servThru = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][7].ToString());
                string temp = Convert.ToString(ds.Tables[_tableName].Rows[i][8].ToString().Trim());
                if (temp.Equals("-"))
                    temp = temp.Replace("-", "0");
                claimsPaid = Decimal.Parse(temp, System.Globalization.NumberStyles.Currency);
                claimType = ds.Tables[_tableName].Rows[i][9].ToString();
                datePaid = Convert.ToDateTime(ds.Tables[_tableName].Rows[i][10].ToString());
                checkno = ds.Tables[_tableName].Rows[i][11].ToString();
                if (checkno.Equals(""))
                {
                    dObj.insertNonCAClaims(_yrmo, caseID, group, suffix, claimID, subID, membCd, name, servFrom, servThru, claimsPaid, claimType, datePaid);
                }
                else
                {
                    dObj.insertCAClaims(_yrmo, caseID, group, suffix, claimID, subID, membCd, name, servFrom, servThru, claimsPaid, claimType, datePaid, checkno);
                }
                _counter++;
            }            
            return _counter;
        }

        //Insert DFnoRF Report
        public int parseDFnoRF(DataSet ds, string _yrmo)
        {
            int count = insertDFnoRF(ds, "dfnorfTable", _yrmo);
            return count;
        }


        private int insertDFnoRF(DataSet ds, string _tableName, string _yrmo)
        {
            string dcn;
            string plancd;
            string subID;
            DateTime lastupdt;
            Decimal amt;
            string SCCF;
            int _counter =0;
            int _count = 0;

            ImportDAL dObj = new ImportDAL();

            String[] cols = new String[] { "DCN", "GroupNumber", "LastUpdate", "Paid", "ClaimType", "SCCF" };

            for (int j = 0; j < cols.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (dc.ColumnName.ToString().Contains(cols[j]))
                    {
                        _count++;
                    }
                }
            }


            if (_count != cols.Length)
            {
                throw (new Exception("Cannot find required columns in DFnoRF File"));
            }
            _counter = 0;

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                int ctrEmpty = 0;
                for (int k = 0; k < ds.Tables[_tableName].Columns.Count; k++)
                {
                    if (ds.Tables[_tableName].Rows[i][k].ToString().Trim().Equals(""))
                    {
                        ctrEmpty++;
                    }
                }

                if (ctrEmpty == ds.Tables[_tableName].Columns.Count)
                    continue;

                dcn = ds.Tables[_tableName].Rows[i][0].ToString().Trim();
                if (dcn.Equals(""))
                {
                    break;
                }
                plancd = ds.Tables[_tableName].Rows[i][2].ToString();
                subID = ds.Tables[_tableName].Rows[i][4].ToString();                
                string temp = ds.Tables[_tableName].Rows[i][5].ToString();
                if (!DateTime.TryParse(temp, out lastupdt))
                {
                    string pattern = @"^(?<year>20\d{2})(?<month>0[1-9]|1[012])(?<day>0[1-9]|[1-2][0-9]|3[01])$";
                    Regex rgx = new Regex(pattern);
                    Match match = rgx.Match(temp);
                    if (match.Success)
                    {
                        temp = match.Groups["month"].Value + "/" + match.Groups["day"].Value + "/" + match.Groups["year"].Value;
                        lastupdt = DateTime.Parse(temp);

                    }
                    else
                    {
                        throw(new Exception("'LastUpdate' Date is not in standard 'Date' format<br />Check format of the report"));
                        
                    }
                }
                temp = ds.Tables[_tableName].Rows[i][7].ToString().Trim();
                if (temp.Equals("-"))
                    temp = temp.Replace("-", "0");
                amt = Decimal.Parse(temp, System.Globalization.NumberStyles.Currency);
                SCCF = ds.Tables[_tableName].Rows[i][10].ToString();
                dObj.insertRFnoDF(_yrmo, SCCF, subID, lastupdt, amt, dcn, plancd);
                _counter++;
            }  
            return _counter;
        }

        //Insert RF Report
        public int parseRF(DataSet ds, string _yrmo)
        {
            int count = insertRF(ds, "rfTable", _yrmo);
            return count;
        }


        private int insertRF(DataSet ds, string _tableName, string _yrmo)
        {
            string dcn;
            string plancd;
            string subID;
            DateTime pddt;
            Decimal amt;
            string SCCF;
            int _counter = 0;
            int _count = 0;

            ImportDAL dObj = new ImportDAL();

            String[] cols = new String[] { "SCCF", "Subscriber ID", "Paid Date", "Total Approv", "dcn", "Plan Cd" };

            for (int j = 0; j < cols.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                {
                    if (string.Compare(dc.ColumnName.ToString().Trim(),cols[j],true)==0)
                    {
                        _count++;
                        break;
                    }
                }
            }


            if (_count != cols.Length)
            {
                throw (new Exception("Cannot find required columns in RF File"));
            }
            _counter = 0;

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                int ctrEmpty = 0;
                for (int k = 0; k < ds.Tables[_tableName].Columns.Count; k++)
                {
                    if (ds.Tables[_tableName].Rows[i][k].ToString().Trim().Equals(""))
                    {
                        ctrEmpty++;
                    }
                }

                if (ctrEmpty == ds.Tables[_tableName].Columns.Count)
                    continue;

                SCCF = ds.Tables[_tableName].Rows[i][0].ToString().Trim();
                if (SCCF.Equals(""))
                {
                    break;
                }

                subID = ds.Tables[_tableName].Rows[i][1].ToString();              
                string temp1 = ds.Tables[_tableName].Rows[i][2].ToString();
                if (!DateTime.TryParse(temp1, out pddt))
                {
                    string pattern = @"^(?<month>0[1-9]|1[012])(?<day>0[1-9]|[1-2][0-9]|3[01])(?<year>20\d{2})$";
                    Regex rgx = new Regex(pattern);
                    Match match = rgx.Match(temp1);
                    if (match.Success)
                    {
                        temp1 = match.Groups["month"].Value + "/" + match.Groups["day"].Value + "/" + match.Groups["year"].Value;
                        pddt = DateTime.Parse(temp1);

                    }
                    else
                    {
                        throw (new Exception("'PaidDate' Date is not in standard 'Date' format<br />Check format of the report"));

                    }
                }                
                string temp = ds.Tables[_tableName].Rows[i][4].ToString().Trim();
                if (temp.Equals("-"))
                    temp = temp.Replace("-", "0");
                amt = Decimal.Parse(temp, System.Globalization.NumberStyles.Currency);
                dcn = ds.Tables[_tableName].Rows[i][5].ToString().Trim();
                plancd  = ds.Tables[_tableName].Rows[i][6].ToString();
                dObj.insertRF(_yrmo, SCCF, subID, pddt, amt, dcn, plancd);
                _counter++;
            }
            return _counter;
        }

        //Insert RX Claims Report
        public int parseRXClaims(DataSet ds, string _yrmo)
        {
            int count = insertRX(ds, "rxTable", _yrmo);
            return count;
        }

        private int insertRX(DataSet ds, string _tableName, string _yrmo)
        {
                       
            Decimal amt;
           
            int _counter = 0;
            int _count = 0;
           

            ImportDAL dObj = new ImportDAL();

            String[] cols = new String[] { "GROUP_NBR", "PAYMENT" };

            for (int j = 0; j < cols.Length; j++)
            {
                foreach (DataColumn dc in ds.Tables[_tableName].Columns)
                { 
                    if (string.Compare(dc.ColumnName.ToString().Trim(), cols[j], true) == 0)
                    {
                        _count++;                        
                        break;
                    }
                }
            }
             if (_count != cols.Length)
            {
                throw (new Exception("Cannot find required columns in RX Claims File"));
            }
            _counter = 0;           

            for (int i = 0; i < ds.Tables[_tableName].Rows.Count; i++)
            {
                string temp = ds.Tables[_tableName].Rows[i][1].ToString().Trim();               
                amt = Decimal.Parse(temp, System.Globalization.NumberStyles.Currency);
                dObj.insertRX(_yrmo, amt);
                _counter++;
            }
            return _counter;
        }

        public decimal getRatebyEdate(string yrmo, string src,string py,string tiercd)
        {
            //string _yrmo = Convert.ToDateTime(yrmo.Insert(4, "/")).ToShortDateString();
            decimal _rate;
            string cmdStr = null;
            cmdStr = "SELECT plhr_CompanyRate FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_plandesc = 'INTERNATIONAL' AND plhr_eff_yrmo <= '" + yrmo + "'";
                  
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr,connect);
            object _rate1 = command.ExecuteScalar();
            if (_rate1 == null)
            {
                cmdStr = "SELECT plhr_CompanyRate FROM AnthPlanhier_History WHERE plhr_py = '" + py + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_plandesc = 'INTERNATIONAL' "
                        + "AND '" + yrmo + "' >= (SELECT max(plhr_eff_yrmo) FROM AnthPlanhier_History WHERE plhr_py = '" + py + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_plandesc = 'INTERNATIONAL')";
                command = new SqlCommand(cmdStr, connect);
                _rate1 = command.ExecuteScalar().ToString();                
            }
            _rate = decimal.Parse(_rate1.ToString());
            return _rate;
        }

        public decimal getRatebyEdate(string yrmo, string src, string py, string tiercd,string breakcd,string plancd)
        {            
            decimal _rate;
            string cmdStr = null;
            cmdStr = "SELECT plhr_CompanyRate FROM AnthPlanhier WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "' AND plhr_eff_yrmo <= '" + yrmo + "'";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            object _rate1 = command.ExecuteScalar();
            if (_rate1 == null)
            {
                cmdStr = "SELECT plhr_CompanyRate FROM AnthPlanhier_History WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "' " 
                         + "AND '" + yrmo + "' >= (SELECT max(plhr_eff_yrmo) FROM AnthPlanhier_History WHERE plhr_py = '" + py + "' AND plhr_plancd = '" + plancd + "' AND plhr_tiercd = '" + tiercd + "' AND plhr_breakCd = '" + breakcd + "')";
                command = new SqlCommand(cmdStr, connect);
                _rate1 = command.ExecuteScalar().ToString();
            }
            _rate = decimal.Parse(_rate1.ToString());
            return _rate;
        }
    }
}


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

/// <summary>
/// Summary description for CalculateTotals
/// </summary>
/// 
namespace EBA.Desktop
{
    public class CalculateTotals
    {
        string connStr;
        public CalculateTotals()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        }

        public DataSet calTotal(DataSet ds,string _yrmo,string _src)
        {           
            string strFilter;
            List<string> _aCode = new List<string>();            
            //string cmdStr =  "SELECT DISTINCT anthcd_plancd FROM AnthCodes " 
            //                    + "INNER JOIN " 
            //                    + "(SELECT DISTINCT rcn_anthcd_id FROM billing_recon " 
            //                    + " WHERE rcn_yrmo = '" + _yrmo + "' AND rcn_source = '" + _src + "') b "
            //                    + "ON (anthcd_id = b.rcn_anthcd_id) ORDER BY anthcd_plancd";

            string cmdStr = null;
            int _prismCount;
            int _anthCount;
            decimal _prismAmt;
            decimal _anthAmt;
            int _grandPCount = 0;
            int _grandACount = 0;
            decimal _grandPAmt = 0;
            decimal _grandAAmt = 0;
            string _sCode = null; //sub total code
            string _gCode = null; // grand total code           
            decimal _rcn_per;
            decimal _newThres = 0;

            switch (_src)
            {
                case "ACT":
                    _sCode = "Domestic";
                    cmdStr = " select DISTINCT anthcd_plancd "
                            + " FROM Anthplanhier,AnthCodes WHERE anthcd_id = plhr_anthcd_id "
                            + " AND plhr_plandesc LIKE 'ACTIVE%' AND plhr_plandesc NOT LIKE '%COB%'"
                            + " ORDER BY anthcd_plancd";
                    break;
                case "RET":
                    _sCode = "Domestic";
                    cmdStr = " select DISTINCT anthcd_plancd "
                            + " FROM Anthplanhier,AnthCodes WHERE anthcd_id = plhr_anthcd_id "
                            + " AND plhr_plandesc LIKE '%RET%' AND plhr_plandesc NOT LIKE '%COB%'"
                            + " ORDER BY anthcd_plancd";
                    break;
                case "COB":
                    _sCode = "Domestic";
                    cmdStr = " select DISTINCT anthcd_plancd "
                            + " FROM Anthplanhier,AnthCodes WHERE anthcd_id = plhr_anthcd_id "
                            + " AND plhr_plandesc LIKE '%COB%'"
                            + " ORDER BY anthcd_plancd";
                    break;
                case "INTL":
                    _sCode = "International";
                    cmdStr = "SELECT DISTINCT anthcd_plancd FROM AnthCodes "
                            + "INNER JOIN "
                            + "(SELECT DISTINCT rcn_anthcd_id FROM billing_recon "
                            + " WHERE rcn_yrmo = '" + _yrmo + "' AND rcn_source = '" + _src + "') b "
                            + "ON (anthcd_id = b.rcn_anthcd_id) ORDER BY anthcd_plancd";
                    break;
            }

            SqlConnection connect = new SqlConnection(connStr);
            SqlDataReader reader;
            DataRow[] rows;
            DataRow rowNew;
            DataSet dsTotal = new DataSet();
            DataTable tempTable,newTable;
            tempTable = ds.Tables[0];
            newTable = dsTotal.Tables.Add("newTable1");
            DataColumn col;

            col = new DataColumn("rcn_yrmo"); newTable.Columns.Add(col);
            col = new DataColumn("anthcd_plancd"); newTable.Columns.Add(col);
            col = new DataColumn("anthcd_covgcd"); newTable.Columns.Add(col);
            col = new DataColumn("rcn_prism_count"); newTable.Columns.Add(col);
            col = new DataColumn("rcn_prism_amt"); newTable.Columns.Add(col);
            col = new DataColumn("rcn_anth_count"); newTable.Columns.Add(col);
            col = new DataColumn("rcn_anth_amt"); newTable.Columns.Add(col); 
            col = new DataColumn("rcn_var"); newTable.Columns.Add(col);
            col = new DataColumn("rcn_per"); newTable.Columns.Add(col);
            col = new DataColumn("threshold"); newTable.Columns.Add(col);
            col = new DataColumn("var_threshold"); newTable.Columns.Add(col);

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            SqlCommand command = new SqlCommand(cmdStr,connect);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                _aCode.Add(reader[0].ToString());
            }
            reader.Close();
            connect.Close();

            foreach (string c1 in _aCode)
            {
                
                _anthAmt = 0;
                _anthCount = 0;
                _prismAmt = 0;
                _prismCount = 0;
                //strFilter = String.Format("WHERE anthcd_plancd = '{0}'",c1 );
                strFilter = "anthcd_plancd = '"+ c1 + "'";
                rows = tempTable.Select(strFilter);
                if (rows.Length > 0)
                {
                    foreach (DataRow r1 in rows)
                    {
                        rowNew = newTable.NewRow();
                        rowNew["rcn_yrmo"] = r1["rcn_yrmo"];
                        rowNew["anthcd_plancd"] = r1["anthcd_plancd"];
                        rowNew["anthcd_covgcd"] = r1["anthcd_covgcd"];
                        string _cvgCd = r1["anthcd_covgcd"].ToString();
                        rowNew["rcn_prism_amt"] = r1["rcn_prism_amt"];
                        _prismAmt = _prismAmt + Convert.ToDecimal(r1["rcn_prism_amt"]);

                        rowNew["rcn_anth_amt"] = r1["rcn_anth_amt"];
                        _anthAmt = _anthAmt + Convert.ToDecimal(r1["rcn_anth_amt"]);

                        rowNew["rcn_prism_count"] = r1["rcn_prism_count"];
                        _prismCount = _prismCount + Convert.ToInt32(r1["rcn_prism_count"]);

                        rowNew["rcn_anth_count"] = r1["rcn_anth_count"];
                        _anthCount = _anthCount + Convert.ToInt32(r1["rcn_anth_count"]);

                        rowNew["rcn_var"] = r1["rcn_var"];
                        rowNew["rcn_per"] = r1["rcn_per"];
                        _rcn_per = Convert.ToDecimal(r1["rcn_per"]);

                        rowNew["threshold"] = r1["threshold"];
                        rowNew["var_threshold"] = r1["var_threshold"];

                        decimal _newThres2 = getVariance(c1 + " - " + _cvgCd, "Tier Codes",_yrmo);
                        if (_newThres2 != 0)
                        {
                            string _thres = checkThres(_newThres2, _rcn_per);
                             if (!_thres.Equals(null))
                             {                                
                                 rowNew["threshold"] = _newThres2;
                                 rowNew["var_threshold"] = _thres;
                             }
                         }
                         else
                         {
                             _newThres = getVariance(c1, "Group Suffix",_yrmo);
                             if (_newThres != 0)
                             {
                                 string _thres = checkThres(_newThres, _rcn_per);
                                 if (!_thres.Equals(null))
                                 {
                                     rowNew["threshold"] = _newThres;
                                     rowNew["var_threshold"] = _thres;
                                 }
                             }
                         }

                        newTable.Rows.Add(rowNew);
                    }

                    rowNew = newTable.NewRow();
                    rowNew["rcn_yrmo"] = "";
                    rowNew["anthcd_plancd"] = "";
                    rowNew["anthcd_covgcd"] = "<font color=\"#036\"><b>Total: </b></font>";
                    rowNew["rcn_prism_amt"] = _prismAmt;
                    rowNew["rcn_anth_amt"] = _anthAmt;
                    rowNew["rcn_prism_count"] = _prismCount;
                    rowNew["rcn_anth_count"] = _anthCount;
                    rowNew["rcn_var"] = "";
                    rowNew["rcn_per"] = "";
                    rowNew["threshold"] = "";
                    rowNew["var_threshold"] = "";

                    decimal _newThres0 = getVariance(c1, "Group Suffix Total",_yrmo);
                    if (_newThres0 != 0)
                    {
                        decimal _var = (_prismCount - _anthCount);
                        decimal _rper = calculateVar(_var, _prismCount, _anthCount);
                        string _thres = checkThres(_newThres0, _rper);
                        if (!_thres.Equals(null))
                        {
                            rowNew["rcn_var"] = _var;
                            rowNew["rcn_per"] = _rper.ToString("#0.00");
                            rowNew["threshold"] = _newThres0;
                            rowNew["var_threshold"] = _thres;
                        }
                    }
                    else
                    {
                        _newThres = getVariance(c1, "Group Suffix",_yrmo);
                        if (_newThres != 0)
                        {
                            decimal _var = (_prismCount - _anthCount);
                            decimal _rper = calculateVar(_var, _prismCount, _anthCount);
                            string _thres = checkThres(_newThres, _rper);
                            if (!_thres.Equals(null))
                            {
                                rowNew["rcn_var"] = _var;
                                rowNew["rcn_per"] = _rper.ToString("#0.00");
                                rowNew["threshold"] = _newThres;
                                rowNew["var_threshold"] = _thres;
                            }
                        }
                        else
                        {
                            decimal _newThres1 = getVariance(_sCode,_yrmo);
                            if (_newThres1 != 0)
                            {
                                decimal _var = (_prismCount - _anthCount);
                                decimal _rper = calculateVar(_var, _prismCount, _anthCount);
                                string _thres = checkThres(_newThres1, _rper);
                                if (!_thres.Equals(null))
                                {
                                    rowNew["rcn_var"] = _var;
                                    rowNew["rcn_per"] = _rper.ToString("#0.00");
                                    rowNew["threshold"] = _newThres1;
                                    rowNew["var_threshold"] = _thres;
                                }
                            }
                        }
                    }

                    newTable.Rows.Add(rowNew);
                    _grandPAmt = _grandPAmt + _prismAmt;
                    _grandAAmt = _grandAAmt + _anthAmt;
                    _grandPCount = _grandPCount + _prismCount;
                    _grandACount = _grandACount + _anthCount;
                }
            }

            switch (_src)
            {
                case "ACT":
                    _gCode = "DOMESTIC - ACTIVE";
                    break;
                case "RET":
                    _gCode = "DOMESTIC - RETIREE";
                    break;
                case "COB":
                    _gCode = "DOMESTIC - COBRA";
                    break;
                case "INTL":
                    _gCode = "INTERNATIONAL";
                    break;
            }

            rowNew = newTable.NewRow();
            rowNew["rcn_yrmo"] = "";
            rowNew["anthcd_plancd"] = "";
            rowNew["anthcd_covgcd"] = "<font color=\"#036\"><b>Grand Total: </b></font>";
            rowNew["rcn_prism_amt"] = _grandPAmt;
            rowNew["rcn_anth_amt"] = _grandAAmt;
            rowNew["rcn_prism_count"] = _grandPCount;
            rowNew["rcn_anth_count"] = _grandACount;
            rowNew["rcn_var"] = "";
            rowNew["rcn_per"] = "";
            rowNew["threshold"] = "";
            rowNew["var_threshold"] = "";

            _newThres = getVariance(_gCode, "Grand Total",_yrmo);
            if (_newThres != 0)
            {
                decimal _var = (_grandPCount - _grandACount);
                decimal _rper = calculateVar(_var, _grandPCount, _grandACount);
                string _thres = checkThres(_newThres, _rper);
                if (!_thres.Equals(null))
                {
                    rowNew["rcn_var"] = _var;
                    rowNew["rcn_per"] = _rper.ToString("#0.00");
                    rowNew["threshold"] = _newThres;
                    rowNew["var_threshold"] = _thres;
                }
            }
            else
            {
                decimal _newThres1 = getVariance(_sCode,_yrmo);
                if (_newThres1 != 0)
                {
                    decimal _var = (_grandPCount - _grandACount);
                    decimal _rper = calculateVar(_var, _grandPCount, _grandACount);
                    string _thres = checkThres(_newThres1, _rper);
                    if (!_thres.Equals(null))
                    {
                        rowNew["rcn_var"] = _var;
                        rowNew["rcn_per"] = _rper.ToString("#0.00");
                        rowNew["threshold"] = _newThres1;
                        rowNew["var_threshold"] = _thres;
                    }
                }
            }

            newTable.Rows.Add(rowNew);
            return dsTotal;
        }

        protected decimal getVariance(string _code, string _type,string _yrmo)
        {
            decimal _per = 0; 
            string _year = _yrmo.Substring(0,4);
            string _month = _yrmo.Substring(4);            

            string cmdStr = null;
            SqlConnection connect = new SqlConnection(connStr);
            SqlCommand command;
            switch (_type)
            {
                case "Tier Codes":
                    cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + _code + "' AND thres_yrmo <= '" + _yrmo +"' AND thres_type = '"+ _type +"'";
                    break;
                case "Group Suffix Total":
                    cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + _code + "' AND thres_yrmo <= '" + _yrmo + "' AND thres_type = '" + _type + "'";
                    break;
                case "Group Suffix":
                    cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + _code + "' AND thres_yrmo <= '" + _yrmo + "' AND thres_type = '" + _type + "'";
                    break;
                case "Grand Total":
                    cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + _code + "' AND thres_yrmo <= '" + _yrmo + "' AND thres_type = '" + _type + "'";
                    break;
                //case "Default":
                //    cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + _code + "' AND YEAR(thres_date) = '" + _year + "' AND MONTH(thres_date) = '" + _month + "' AND thres_type = 'Default'";
                //    break;
            }                      
         
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            object perObj = command.ExecuteScalar();
            if(perObj!=null)
            {
                _per = decimal.Parse(perObj.ToString());
            }
            connect.Close();
            return _per;
        }

        //Default Threshold Value
        protected decimal getVariance(string src, string yrmo)
        {
            decimal _per = 0;
            string _yrmo = Convert.ToDateTime(yrmo.Insert(4, "/")).ToShortDateString();
            SqlConnection connect = new SqlConnection(connStr);
            SqlCommand command;

            string cmdStr = "SELECT thres_value FROM threshold WHERE thres_name = '" + src + "' AND '" + yrmo + "' >= thres_yrmo AND thres_type = 'Default'";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdStr, connect);
            object _per1 = command.ExecuteScalar();
            if (_per1 == null)
            {
                cmdStr = "SELECT thres_value FROM threshold_History WHERE thres_name = '" + src + "' "
                            + "AND '" + yrmo + "' >= (SELECT max(thres_yrmo) FROM threshold_History WHERE thres_name = '" + src + "' AND thres_type = 'Default') AND thres_type = 'Default'";
                command = new SqlCommand(cmdStr, connect);
                _per1 = command.ExecuteScalar();
            }
            _per = decimal.Parse(_per1.ToString());
            connect.Close();
            return _per;
        }

        protected decimal calculateVar(decimal _var, int hc1, int hc2)
        {
            decimal _rper = 0;            
            if (_var == 0)
            {
                _rper = 0;
            }
            else if (_var > 0)
            {
                decimal _diff = (decimal)(hc1 - hc2) / hc1;
                _rper = _diff * 100;
            }
            else if (_var < 0)
            {
                decimal _diff = (decimal) (hc2 - hc1) / hc2;
                _rper = _diff * 100;
            }
            return _rper;

        }
        protected string checkThres(decimal _per, decimal _rcnper)
        {
            string _thres = null;
            if(_rcnper > _per)
            {
                _thres = "<font color=\"Red\">Exceeded</font>";
            }
            else if(_rcnper < _per && _rcnper !=0)
            {
                _thres = "-";
            }
            else if(_rcnper == 0)
            {
                _thres = "-";
            }
            return _thres;
        }
    }
}

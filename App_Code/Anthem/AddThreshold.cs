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
/// Summary description for AddThreshold
/// </summary>
/// 
namespace EBA.Desktop.Anthem
{
    public class AddThreshold
    {
        private string connStr;
        private SqlConnection connect = null;
        private SqlCommand command = null;

        public AddThreshold()
        {
            connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            connect = new SqlConnection(connStr);
        }

        public List<string> getPlancodes(string src)
        {
            string cmdstr = null;
            List<string> _pCodes = new List<string>();
            switch (src)
            {
                case "ACTIVE":
                    cmdstr = "SELECT DISTINCT anthcd_plancd FROM AnthCodes, AnthPlanhier WHERE anthcd_id = plhr_anthcd_id AND plhr_plandesc LIKE 'ACT%' AND plhr_plandesc NOT LIKE '%COB%' ORDER BY anthcd_plancd";
                    break;
                case "RETIREE":
                    cmdstr = "SELECT DISTINCT anthcd_plancd FROM AnthCodes, AnthPlanhier WHERE anthcd_id = plhr_anthcd_id AND plhr_plandesc LIKE '%RET%' AND plhr_plandesc NOT LIKE '%COB%' ORDER BY anthcd_plancd";
                    break;
                case "COBRA":
                    cmdstr = "SELECT DISTINCT anthcd_plancd FROM AnthCodes, AnthPlanhier WHERE anthcd_id = plhr_anthcd_id AND plhr_plandesc LIKE '%COB%' AND plhr_plandesc NOT LIKE 'INTER%' ORDER BY anthcd_plancd";
                    break;
                case "INTERNATIONAL":
                    cmdstr = "SELECT DISTINCT anthcd_plancd FROM AnthCodes, AnthPlanhier WHERE anthcd_id = plhr_anthcd_id AND plhr_plandesc LIKE '%INTER%'  ORDER BY anthcd_plancd";
                    break;
            }
            SqlDataReader reader;
            connect.Open();
            command = new SqlCommand(cmdstr,connect);
            using (connect)
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _pCodes.Add(reader[0].ToString());
                }
                reader.Close();
            }
            connect.Close();
            return _pCodes;
        }

        public void insertNewThreshold(string _plan, string _threstype,decimal _thresvalue,string _thresyrmo)
        {
            HttpContext.Current.Session["thresID"] = 0;
            string cmdstr = "INSERT INTO threshold (thres_name,thres_type,thres_value,thres_yrmo) VALUES ('" + _plan + "','" + _threstype + "'," + _thresvalue + ",'" +  _thresyrmo+ "')";
            SqlTransaction ts; 
            
            connect.Open();
            ts = connect.BeginTransaction();
            try
            {
                string cmdstr2 = "SELECT thres_id FROM threshold WHERE thres_name = '" + _plan + "' AND thres_type = '" + _threstype + "' AND thres_value = " + _thresvalue + " AND thres_yrmo = '" + _thresyrmo + "'";
                command = new SqlCommand(cmdstr2, connect, ts);
                int cnt = Convert.ToInt32(command.ExecuteScalar());

                if (cnt == 0)
                {
                    command = new SqlCommand(cmdstr, connect, ts);
                    command.ExecuteNonQuery();

                    string cmdstr1 = "SELECT thres_id FROM threshold WHERE thres_name = '" + _plan + "' AND thres_type = '" + _threstype + "' AND thres_value = " + _thresvalue + " AND thres_yrmo = '" + _thresyrmo + "'";
                    command = new SqlCommand(cmdstr1, connect, ts);
                    HttpContext.Current.Session["thresID"] = Convert.ToInt32(command.ExecuteScalar());
                    ts.Commit();
                }
                else
                {
                    throw (new Exception("Error adding new Threshold value, Threshold already defined!"));
                }
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw ex;
            }
            finally
            {
                connect.Close();
            }
                        
        }

    }
}

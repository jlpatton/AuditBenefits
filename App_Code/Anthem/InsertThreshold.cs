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

/// <summary>
/// Summary description for InsertThreshold
/// </summary>
/// 
namespace EBA.Desktop
{
    public class InsertThreshold
    {
        private static readonly string _connStr;
        private string _reconName;
        private string _reconPer;
        private string _reconDate;

        public string ReconName
        {
            get
            {
                return _reconName;
            }
            set
            {
                _reconName = value;
            }
        }

        public string ReconPerc
        {
            get
            {
                return _reconPer;
            }
            set
            {
                _reconPer = value;
            }
        }

        public string ReconDate
        {
            get
            {
                return _reconDate;
            }
            set
            {
                _reconDate = value;
            }
        }

        static InsertThreshold()
        {
            _connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        }

        public static void Insert(string reconName, string reconPerc, string reconDate)
        {
            // Initialize command
            decimal per = Convert.ToDecimal(reconPerc);
            DateTime dt = Convert.ToDateTime(reconDate);
            SqlConnection con = new SqlConnection(_connStr);
            string cmdStr = "INSERT INTO threshold (thres_name, thres_value, thres_date) VALUES (@ReconName,@ReconPer,@ReconDate)";
            SqlCommand cmd = new SqlCommand(cmdStr, con);

            cmd.Parameters.AddWithValue("@ReconName", reconName);
            cmd.Parameters.AddWithValue("@ReconPer", per);
            cmd.Parameters.AddWithValue("@ReconDate", dt);
            // Execute command            

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }        
    }
}

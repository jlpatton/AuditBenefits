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
/// Summary description for InsertRole
/// </summary>
/// 
namespace EBA.Desktop
{
    public class InsertRole
    {
        private static readonly string _connStr;
        private string _roleName;
        private string _roleDesc;

        public string RoleName
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }
        public string RoleDesc
        {
            get
            {
                return _roleDesc;
            }
            set
            {
                _roleDesc = value;
            }
        }
        static InsertRole()
        {
            _connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        }

        /// <summary>
        /// Create a new Role
        /// </summary>
        public static void Insert(string roleName, string roleDesc)
        {
            // Initialize command
            SqlConnection con = new SqlConnection(_connStr);
            string cmdStr = "INSERT INTO ROLES_AD (RoleName, RoleDesc) VALUES (@RoleName,@RoleDesc)";
            SqlCommand cmd = new SqlCommand(cmdStr, con);

            cmd.Parameters.AddWithValue("@RoleName", roleName);            
            cmd.Parameters.AddWithValue("@RoleDesc", roleDesc);
            // Execute command            
            
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            
           
        }
    }
}

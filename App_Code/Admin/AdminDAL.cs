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
/// Summary description for admin
/// </summary>
/// 

namespace EBA.Desktop.Admin
{
    public class admin
    {
        public admin()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /* Code table ASIS in db */

        //  M Code	    Module	        Type
        //--------------------------------------
        //  M1	        ADMIN	        M
        //  M2          AUDIT           M
        //  M100	    ANTHEM	        M
        //  M200	    HRA	            M
        //  M101	    Billing	        ANTH
        //  M102	    Claims	        ANTH
        //  M103        Report          ANTH
        //  M104	    Maintenance	    ANTH
        //  M201	    Maintenance	    HRA
        //  M202	    Reconciliation	HRA
        //  M203	    Operations	    HRA
        //  M400        VWA             M
        //  M500        ImputedIncome   M
        //  M600        YEB             M
        //  M700        EBADesktop      M


        private static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;

        public static bool checkMod(string uname, string parentCode, string mcode)
        {
            bool auth;
            bool valid = false;
            int uid = getUserID(uname);
            string mtype = setModType(parentCode);
            auth = getModule(uid, parentCode);
            int count = 0;
            if (!mcode.Equals(""))
            {
                count = mcount(uid, mtype);
            }
            if (auth && parentCode.Equals("M1"))
            {
                valid = true;
            }
            else if (auth)
            {
                if (count > 0)
                {
                    valid = checkAuth(uid, mcode, mtype);
                }
                else
                {
                    valid = true;
                }
            }
            return valid;

        }


        private static int getUserID(string uname)
        {
            SqlConnection conn = new SqlConnection(connStr);
            //string cmdStr = "SELECT UserId FROM UserLogin WHERE UserName = @uname";
            string cmdStr = "SELECT UserId FROM UserLogin_AD WHERE empID = @uname";
            int uid = 0;
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@uname", SqlDbType.Int);
                cmd.Parameters["@uname"].Value = Convert.ToInt32(uname);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    uid = (dr.GetInt32(0));
                }
                dr.Close();
                return uid;
            }
            catch (Exception e)
            {
                return uid;
            }
            finally
            {
                conn.Close();
            }
        }

        private static string setModType(string pCode)
        {
            string mtype = "";
            switch (pCode)
            {
                case "M100": mtype = "ANTH";
                    break;
                case "M200": mtype = "HRA";
                    break;
                case "M300": mtype = "IPBA";
                    break;
                case "M400": mtype = "VWA";
                    break;
                case "M500": mtype = "IMPUT";
                    break;
                case "M600": mtype = "YEB";
                    break;
                case "M700": mtype = "EBADsktp";
                    break;

            }
            return mtype;
        }


        private static bool getModule(int uid, string mcode)
        {
            bool valid = false;
            SqlConnection conn = new SqlConnection(connStr);

            string cmdStr = "SELECT MCode,Mtype FROM Page_AD p, RoleOperations_AD r, UserRoles_AD u "
                                + " WHERE u.UserID = @uid and u.RoleID = r.RoleID and r.MID = p.MID and OID = 1";

            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(cmdStr, conn);
                cmd1.Parameters.Add("@uid", SqlDbType.Int);
                cmd1.Parameters["@uid"].Value = uid;
                SqlDataReader dr1;
                dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    string m = Convert.ToString(dr1[0]).Trim();
                    if (mcode.Equals(m))
                    {
                        valid = true;
                    }
                }
                dr1.Close();


            }
            catch (Exception e)
            {
                valid = false;
            }
            finally
            {
                conn.Close();
            }
            return valid;
        }

        private static int mcount(int uid, string mtype)
        {
            int count = 0;
            SqlConnection conn = new SqlConnection(connStr);

            string cmdStr = "SELECT COUNT (Mtype) FROM Page_AD p, RoleOperations_AD r, UserRoles_AD u "
                                + " WHERE u.UserID = @uid and u.RoleID = r.RoleID and r.MID = p.MID and OID = 1 and p.Mtype = @mtype";

            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(cmdStr, conn);
                cmd1.Parameters.Add("@uid", SqlDbType.Int);
                cmd1.Parameters["@uid"].Value = uid;
                cmd1.Parameters.Add("@mtype", SqlDbType.Char);
                cmd1.Parameters["@mtype"].Value = mtype;
                SqlDataReader dr1;
                dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    count = Convert.ToInt32(dr1[0]);
                }
                dr1.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                conn.Close();
            }
            return count;

        }

        private static bool checkAuth(int uid, string mcode, string mtype)
        {
            bool auth = false;
            SqlConnection conn = new SqlConnection(connStr);

            string cmdStr = "SELECT MCode,Mtype FROM Page_AD p, RoleOperations_AD r, UserRoles_AD u "
                                + " WHERE u.UserID = @uid and u.RoleID = r.RoleID and r.MID = p.MID and OID = 1";

            conn.Open();
            try
            {
                SqlCommand cmd1 = new SqlCommand(cmdStr, conn);
                cmd1.Parameters.Add("@uid", SqlDbType.Int);
                cmd1.Parameters["@uid"].Value = uid;
                SqlDataReader dr1;
                dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    string mcode1 = Convert.ToString(dr1[0]).Trim();
                    string mtype1 = Convert.ToString(dr1[1]).Trim();
                    if (mcode.Equals(mcode1) && mtype.Equals(mtype1))
                    {
                        auth = true;
                    }
                }
                dr1.Close();

            }
            catch (Exception e)
            {
            }
            return auth;

        }
    }
}

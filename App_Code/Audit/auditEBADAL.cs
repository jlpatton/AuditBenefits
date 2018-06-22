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
using System.Data.SqlClient;

/// <summary>
/// Summary description for auditEBA
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class auditEBA
    {
        public auditEBA()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        //get sequence number for a given session id
        protected static Int32 getSeqno(string ssid)
        {
            int seqno;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            SqlCommand cmd = new SqlCommand("Select se_seqno from Sessions_au where se_sid = @ssid", conn);
            cmd.Parameters.Add(new SqlParameter("@ssid", SqlDbType.VarChar));
            cmd.Parameters["@ssid"].Value = ssid;

            seqno = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            conn.Close();
            cmd.Dispose();

            return seqno;

        }

        //get sequence number for a child task
        protected static Int32 getChildSeqno(int sseqno, int mid, int tseqno, int cseqno)
        {
            int childseqno;
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            SqlCommand cmd = new SqlCommand("Select c_taskseqno from ChildTransactions_au where c_seseqno = @sseqno AND c_tseqno = @tseqno AND c_modseqno = @mseqno AND c_seqno = @cseqno", conn);
            cmd.Parameters.Add(new SqlParameter("@sseqno", SqlDbType.Int));
            cmd.Parameters["@sseqno"].Value = sseqno;
            cmd.Parameters.Add(new SqlParameter("@tseqno", SqlDbType.Int));
            cmd.Parameters["@tseqno"].Value = tseqno;
            cmd.Parameters.Add(new SqlParameter("@cseqno", SqlDbType.Int));
            cmd.Parameters["@cseqno"].Value = cseqno;
            cmd.Parameters.Add(new SqlParameter("@mseqno", SqlDbType.Int));
            cmd.Parameters["@mseqno"].Value = mid;

            childseqno = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            conn.Close();
            cmd.Dispose();

            return childseqno;

        }

        //Audit Users logins 
        public static void auditUser(String ssid, String uid)
        {

            string cmdStr = "INSERT INTO Sessions_au(se_sid, se_uid) VALUES ( @ssid, @uid)";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@ssid", SqlDbType.VarChar);
            cmd.Parameters["@ssid"].Value = ssid;
            cmd.Parameters.Add("@uid", SqlDbType.VarChar);
            cmd.Parameters["@uid"].Value = uid;
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            cmd.Dispose();
            conn.Dispose();
        }

        //Audit pages visted by a user in a session
        public static void auditPage(string pName, string ssid, Int32 pseqno)
        {
            int seqno = getSeqno(ssid);
            string cmdStr = "INSERT INTO PageVisited_au(p_sseqno, p_page,p_tseqno) VALUES ( @sseqno, @pvisit,@pseqno)";
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@sseqno", SqlDbType.Int);
                cmd.Parameters["@sseqno"].Value = seqno;
                cmd.Parameters.Add("@pvisit", SqlDbType.VarChar);
                cmd.Parameters["@pvisit"].Value = pName;
                cmd.Parameters.Add("@pseqno", SqlDbType.Int);
                cmd.Parameters["@pseqno"].Value = pseqno;


                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();
        }

        //Audit user Parent transactions
        public static void auditTrans(string ssid, int modseqno, int tseqno, string module, string ptask)
        {
            int seqno = getSeqno(ssid);
            string cmdStr = "INSERT INTO Transactions_au(t_seseqno,t_moduleSeqno,t_seqno,t_module,t_Ptask) VALUES "
                                + "( @sseqno,@mseqno,@tseqno,@module,@ptask)";
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@sseqno", SqlDbType.Int);
                cmd.Parameters["@sseqno"].Value = seqno;
                cmd.Parameters.Add("@mseqno", SqlDbType.Int);
                cmd.Parameters["@mseqno"].Value = modseqno;
                cmd.Parameters.Add("@tseqno", SqlDbType.Int);
                cmd.Parameters["@tseqno"].Value = tseqno;
                cmd.Parameters.Add("@module", SqlDbType.VarChar);
                cmd.Parameters["@module"].Value = module;
                cmd.Parameters.Add("@ptask", SqlDbType.VarChar);
                cmd.Parameters["@ptask"].Value = ptask;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();
        }

        //Audit user Child transactions
        public static void auditChildTrans(Int32 pid, Int32 cid, int modseqno, string ssid, string ctask)
        {
            int seqno = getSeqno(ssid);
            string cmdStr = "INSERT INTO ChildTransactions_au(c_seseqno,c_modseqno,c_tseqno,c_seqno,c_Ctask) VALUES "
                                + "( @sseqno,@mseqno, @tseqno,@cseqno,@child)";
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@sseqno", SqlDbType.Int);
                cmd.Parameters["@sseqno"].Value = seqno;
                cmd.Parameters.Add("@mseqno", SqlDbType.Int);
                cmd.Parameters["@mseqno"].Value = modseqno;
                cmd.Parameters.Add("@tseqno", SqlDbType.Int);
                cmd.Parameters["@tseqno"].Value = pid;
                cmd.Parameters.Add("@cseqno", SqlDbType.Int);
                cmd.Parameters["@cseqno"].Value = cid;
                cmd.Parameters.Add("@child", SqlDbType.VarChar);
                cmd.Parameters["@child"].Value = ctask;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();
        }

        //Audit Maintenance
        public static void auditMaint(Int32 pid, Int32 cid, int mid, string ssid, string mtype)
        {
            int seqno = getSeqno(ssid);
            int childseqno = getChildSeqno(seqno, mid, pid, cid);
            string cmdStr = "INSERT INTO Maintenance_au(m_taskseqno,m_type) VALUES "
                                + "( @cseqno,@type)";
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@cseqno", SqlDbType.Int);
                cmd.Parameters["@cseqno"].Value = childseqno;
                cmd.Parameters.Add("@type", SqlDbType.VarChar);
                cmd.Parameters["@type"].Value = mtype;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();

        }

        //Audit DataBase Inserts/Updates/Deletes/Select
        public static void auditDataFields(Int32 pid, Int32 cid, int mid, string ssid, string dbname, string tbname, string prkey, string type1, string field, string oldv, string newv)
        {
            int seqno = getSeqno(ssid);
            int childseqno = getChildSeqno(seqno, mid, pid, cid);
            int mainseqno;
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = null;

            conn.Open();

            cmd = new SqlCommand("Select m_seqno from Maintenance_au where m_taskseqno = @taskseqno", conn);
            cmd.Parameters.Add(new SqlParameter("@taskseqno", SqlDbType.Int));
            cmd.Parameters["@taskseqno"].Value = childseqno;

            mainseqno = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            conn.Close();
            cmd.Dispose();

            string cmdStr = "INSERT INTO DataFields_au(d_mseqno,d_dbname,d_table,d_pkey,d_type,d_columnname,d_oldv,d_newv) VALUES "
                                + "( @mseqno,@dbname,@tbname,@pkey,@type,@field,@oldv,@newv)";

            conn.Open();
            try
            {
                cmd = new SqlCommand(cmdStr, conn);

                cmd.Parameters.Add("@mseqno", SqlDbType.Int);
                cmd.Parameters["@mseqno"].Value = mainseqno;
                cmd.Parameters.Add("@dbname", SqlDbType.VarChar);
                cmd.Parameters["@dbname"].Value = dbname;
                cmd.Parameters.Add("@tbname", SqlDbType.VarChar);
                cmd.Parameters["@tbname"].Value = tbname;
                cmd.Parameters.Add("@pkey", SqlDbType.VarChar);
                cmd.Parameters["@pkey"].Value = prkey;
                cmd.Parameters.Add("@type", SqlDbType.VarChar);
                cmd.Parameters["@type"].Value = type1;
                cmd.Parameters.Add("@field", SqlDbType.VarChar);
                cmd.Parameters["@field"].Value = field;
                cmd.Parameters.Add("@oldv", SqlDbType.VarChar);
                cmd.Parameters["@oldv"].Value = oldv;
                cmd.Parameters.Add("@newv", SqlDbType.VarChar);
                cmd.Parameters["@newv"].Value = newv;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();

        }

        //Audit Recon 
        public static void auditRecon(Int32 pid, Int32 cid, int mid, string ssid, string rtype, string yrmo, int cnt)
        {
            int seqno = getSeqno(ssid);
            int childseqno = getChildSeqno(seqno, mid, pid, cid);

            string cmdStr = "INSERT INTO Reconciliation_au(r_taskseqno,r_type,r_yrmo,r_cnt) VALUES "
                                + "( @childseqno,@rtype,@yrmo,@cnt)";
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(cmdStr, conn);

                cmd.Parameters.Add("@childseqno", SqlDbType.Int);
                cmd.Parameters["@childseqno"].Value = childseqno;
                cmd.Parameters.Add("@rtype", SqlDbType.VarChar);
                cmd.Parameters["@rtype"].Value = rtype;
                cmd.Parameters.Add("@yrmo", SqlDbType.Char);
                cmd.Parameters["@yrmo"].Value = yrmo;
                cmd.Parameters.Add("@cnt", SqlDbType.Int);
                cmd.Parameters["@cnt"].Value = cnt;

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            conn.Dispose();

        }

    }
}

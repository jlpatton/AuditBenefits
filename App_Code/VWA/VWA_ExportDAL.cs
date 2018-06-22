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

namespace EBA.Desktop.VWA
{
    public class VWA_ExportDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;

        public VWA_ExportDAL()
        {
        }

        public static bool checkedPrint(string report)
        {
            bool _checkedPrint = false;
            string cmdStr = "SELECT autoprint FROM FileMaintainence "
                                + "WHERE module='VWA' "
                                + "AND classification = 'Export' "
                                + "AND sourcecd = @report ";


            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@report", report);
                _checkedPrint = Convert.ToBoolean(command.ExecuteScalar());

                return _checkedPrint;
            }
            finally
            {
                connect.Close();
            }
        }

        public static bool checkedSave(string report)
        {
            bool _checkedSave = false;
            string cmdStr = "SELECT autosave FROM FileMaintainence "
                                + "WHERE module='VWA' "
                                + "AND classification = 'Export' "
                                + "AND sourcecd = @report ";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@report", report);
                _checkedSave = Convert.ToBoolean(command.ExecuteScalar());

                return _checkedSave;
            }
            finally
            {
                connect.Close();
            }
        }

        public static string GetFilePath(string _source)
        {
            string _filepath = "";
            string cmdStr = "SELECT filelocation FROM FileMaintainence "
                                + "WHERE module='VWA' "
                                + "AND classification = 'Export' "
                                + "AND sourcecd = @source ";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@source", _source);
                _filepath = Convert.ToString(command.ExecuteScalar());

                if ((_filepath.Length > 1) && !(_filepath.Substring(_filepath.Length - 2).Equals("\\")))
                    _filepath = _filepath + "\\";

                return _filepath;
            }
            finally
            {
                connect.Close();
            }
        }

        //Checks if the output report has records
        public static Boolean HasRecords(string yrmo, string source)
        {
            string priyrmo;
            Boolean _result;
            int count = 0;            
            string cmdstr = "";

            if(source.Equals("TranAging")) {priyrmo = String.Empty;}
            else {priyrmo = VWA.getPrevYRMO(yrmo);}

            switch (source)
            {
                case "RemitDisp":
                    cmdstr = "SELECT COUNT(*) FROM VWA_remit WHERE yrmo = @yrmo AND pdClient > 0";
                    break;
                case "RemitInput":
                    cmdstr = "SELECT COUNT(*) FROM [VWA_remit] WHERE yrmo = @yrmo";
                    break;
                case "TranMis":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 WHERE yrmo = @priyrmo " 
			                    + "AND ContractNo NOT IN "
			                    + "( "
				                    + "SELECT ContractNo FROM VWA_Trans1 "
				                    + "WHERE yrmo = @yrmo "
			                    + ")";
                    break;
                case "TranClient":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 a, VWA_Trans1 b "
			                        + "WHERE a.yrmo = @yrmo AND b.yrmo = @priyrmo "
			                        + "AND (a.ContractNo = b.ContractNo) "
			                        + "AND (ISNULL(a.ClientId,0) <> ISNULL(b.ClientId,0)) "
			                        + "GROUP BY a.ClientId, b.ClientId	";
                    break;
                case "TranGrp":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 a, VWA_Trans1 b "
			                        + "WHERE a.yrmo = @yrmo AND b.yrmo = @priyrmo "
			                        + "AND (a.ContractNo = b.ContractNo) "
			                        + "AND (ISNULL(a.GroupNo,0) <> ISNULL(b.GroupNo,0)) "
                                    + "GROUP BY a.GroupNo, b.GroupNo ";
                    break;
                case "TranStat":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 "
                                + "WHERE yrmo = @yrmo "
			                    + "GROUP BY ClientId, GroupNo";
                    break;
                case "TranACCTG":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 a, VWA_Trans1 b "
			                    + "WHERE a.yrmo = @yrmo AND b.yrmo = @priyrmo "
			                    + "AND (a.ContractNo = b.ContractNo) "
			                    + "AND " 
			                    + "( "
				                    + "(ISNULL(a.RecAmt,0) <> ISNULL(b.RecAmt,0)) OR " 
				                    + "(ISNULL(a.NetAmt,0) <> ISNULL(b.NetAmt,0)) OR " 
				                    + "(ISNULL(a.TotFees,0) <> ISNULL(b.TotFees,0)) "
			                    + ") "
			                    +"GROUP BY a.ClientId, a.GroupNo";
                    break;
                case "TranAging":
                    if (yrmo == null || yrmo.Equals(String.Empty)) { yrmo = "0"; }
                    int agedDays = Convert.ToInt32(yrmo);
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 "
                                + "WHERE (OpenDt BETWEEN CONVERT(VARCHAR(15),(GetDate() - " + agedDays + "),101) And CONVERT(VARCHAR(15),GetDate(),101)) "
			                    + "AND CloseDt IS NULL";
                    break;
                case "TranStatCtr":
                    cmdstr = "SELECT COUNT(*) FROM VWA_Trans1 "
                                + "WHERE yrmo = @yrmo "
			                    + "GROUP BY StatusCd";
                    break;
            }

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@priyrmo", priyrmo);
                count = Convert.ToInt32(command.ExecuteScalar());
                
                if (count != 0) { _result = true;}
                else {_result = false;}
               
                command.Dispose();

                return _result;
            }
            finally
            {
                connect.Close();
            }
        }

        public static DataSet GetTranOutputData(string _source, string _yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet(); ds.Clear();
            string priyrmo;
            int agedDays;
            
            try
            {
                if (_source.Equals("TranAgingSUM") || _source.Equals("TranAgingDET")) 
                {
                    priyrmo = String.Empty;
                    agedDays = Convert.ToInt32(_yrmo);                   
                }
                else 
                {
                    agedDays = 0; 
                    priyrmo = VWA.getPrevYRMO(_yrmo);
                }

                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                command = new SqlCommand("[dbo].[sp_VWA_Trans_Output]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 50;
                command.Parameters.Add("@yrmo", SqlDbType.VarChar).Value = _yrmo;
                command.Parameters.Add("@source", SqlDbType.VarChar).Value = _source;
                command.Parameters.Add("@priyrmo", SqlDbType.VarChar).Value = priyrmo;
                command.Parameters.Add("@agedDays", SqlDbType.VarChar).Value = agedDays;
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();

                return ds;
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

        public static void insertReconDt(string yrmo)
        {
            string cmdstr0 = "DELETE FROM ImportRecon_status WHERE yrmo = @yrmo AND source = 'Balance' AND type = 'Recon' AND module = 'VWA'";
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module, recondt) VALUES(@yrmo, 'Balance', 'Recon', 'VWA', GETDATE())";
            SqlTransaction ts = null;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }            
            try
            {
                ts = connect.BeginTransaction();
                command = new SqlCommand(cmdstr0, connect, ts);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();

                command = new SqlCommand(cmdStr, connect, ts);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch 
            {
                ts.Rollback();
            }
            finally
            {               
                connect.Close();
            }
        }
    }
}
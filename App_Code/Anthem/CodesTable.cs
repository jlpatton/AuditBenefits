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
using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;

/// <summary>
/// Summary description for CodesTable
/// </summary>
/// 
namespace EBA.Desktop
{
    public class CodesTable
    {
        private static readonly string _connStr;
        
        private int plhrID;
        private string plhrDesc;
        private string plhrYear;
        private string plhrEdt;
        private string plhrPlancd = null;
        private string plhrTiercd = null;
        private string plhrRate;
        private string anthPlancd;
        private string anthTiercd;
        private string pstate;
        private string pmed;

        public int PlhrId
        {
            get
            {
                return plhrID;
            }
            set
            {
                plhrID = value;
            }
        }
        public string Desc
        {
            get
            {
                return plhrDesc;
            }
            set
            {
                plhrDesc = value;
            }
        }
        public string PlanYear
        {
            get
            {
                return plhrYear;
            }
            set
            {
                plhrYear = value;
            }
        }
        public string EDate
        {
            get
            {
                return plhrEdt;
            }
            set
            {
                plhrEdt = value;
            }
        }
        public string PlanCode
        {
            get
            {
                return plhrPlancd;
            }
            set
            {
                plhrPlancd = value;
            }
        }
        public string TierCode
        {
            get
            {
                return plhrTiercd;
            }
            set
            {
                plhrTiercd = value;
            }

        }
        public string Rate
        {
            get
            {
                return plhrRate;
            }
            set
            {
                plhrRate = value;
            }

        }
        public string AnthemPlanCode
        {
            get
            {
                return anthPlancd;
            }
            set
            {
                anthPlancd = value;
            }

        }
        public string AnthemTierCode
        {
            get
            {
                return anthTiercd;
            }
            set
            {
               anthTiercd = value;
            }

        }
        public string State
        {
            get
            {
                return pstate;
            }
            set
            {
                pstate = value;
            }
        }
        public string Med
        {
            get
            {
                return pmed;
            }
            set
            {
                pmed = value;
            }
        }

        static CodesTable()
        {
            _connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;            
        }

        public static void Insert(string Desc, string PlanYear, string EDate, string State, string Med, string PlanCode, string TierCode, string Rate, string AnthemPlanCode, string AnthemTierCode)
        {
            string _state = "N";
            string _med = "N";
            if (State.Equals("CA"))
            {
                _state = "C";
            }
            if (Med.Equals("Medicare"))
            {
                _med = "M";
            }
            string _breakcd = _state + _med;

            DateTime edate = Convert.ToDateTime(EDate);
            Decimal rate = Convert.ToDecimal(Rate); 
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "INSERT INTO AnthCodes (anthcd_py,anthcd_effdt,anthcd_plancd,anthcd_covgcd) VALUES (@pyear,@edate,@aPcode,@aTcode)";
            string cmdStr1 = "SELECT anthcd_id FROM AnthCodes WHERE anthcd_py = @pyear AND anthcd_effdt = @edate AND anthcd_plancd = @aPcode AND anthcd_covgcd = @aTcode";
            string cmdStr2 = "INSERT INTO AnthPlanhier (plhr_anthcd_id,plhr_plandesc,plhr_breakcd,plhr_effdt,plhr_py,plhr_plancd,plhr_tiercd,plhr_CompanyRate) VALUES "
                                + " (@aId,@pdesc,@bcode,@edate,@pyear,@pcode,@tcode,@rate)";
            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr, conn,ts);
                    cmd.Parameters.AddWithValue("@pyear", PlanYear);
                    cmd.Parameters.AddWithValue("@edate",edate);
                    cmd.Parameters.AddWithValue("@aPcode", AnthemPlanCode);
                    cmd.Parameters.AddWithValue("@aTcode", AnthemTierCode);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(cmdStr1, conn,ts);
                    cmd.Parameters.AddWithValue("@pyear", PlanYear);
                    cmd.Parameters.AddWithValue("@edate", edate);
                    cmd.Parameters.AddWithValue("@aPcode", AnthemPlanCode);
                    cmd.Parameters.AddWithValue("@aTcode", AnthemTierCode);
                    int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    cmd = new SqlCommand(cmdStr2, conn,ts);
                    cmd.Parameters.AddWithValue("@aId", anthID);
                    cmd.Parameters.AddWithValue("@pdesc", Desc);
                    cmd.Parameters.AddWithValue("@bcode", _breakcd); 
                    cmd.Parameters.AddWithValue("@edate", edate);
                    cmd.Parameters.AddWithValue("@pyear", PlanYear);
                    cmd.Parameters.AddWithValue("@pcode", PlanCode);
                    cmd.Parameters.AddWithValue("@tcode", TierCode);
                    cmd.Parameters.AddWithValue("@rate", rate);                   
                    cmd.ExecuteNonQuery();
                    ts.Commit();
                }
                catch(SqlException sqlErr)
                {
                    ts.Rollback();
                }
            }
        }

        //public static void Update(int pId,string pDesc, string pYear, string pEdt, string pCode, string tCode, string pRate, string aPCode, string aTCode)
        //{
        //    DateTime edate = Convert.ToDateTime(pEdt);
        //    Decimal rate = Convert.ToDecimal(pRate);
        //    SqlConnection conn = new SqlConnection(_connStr);
        //    SqlTransaction ts;
        //    SqlCommand cmd = null;
        //    string cmdStr = "UPDATE AnthPlanhier SET plhr_plancd = @pcode, plhr_effdt = @edate, plhr_py = @pyear, plhr_placd = @pcode, plhr_tiercd = @tcode, plhr_CompanyRate = @rate WHERE plhr_id = @pid";
        //    string cmdStr1 = "SELECT plhr_anthcd_id FROM AnthPlanhier WHERE plhr_id = @pid";
        //    string cmdStr2 = "UPDATE AnthCodes SET anthcd_effdt = @edate,anthcd_plancd = @aPcode,anthcd_covgcd =@aTcode WHERE anthcd_id = @aid";          
            
        //    using (conn)
        //    {
        //        conn.Open();
        //        ts = conn.BeginTransaction();
        //        try
        //        {
        //            cmd = new SqlCommand(cmdStr, conn, ts);
        //            cmd.Parameters.AddWithValue("@pid", pId);
        //            cmd.Parameters.AddWithValue("@pdesc", pDesc);
        //            cmd.Parameters.AddWithValue("@edate", edate);
        //            cmd.Parameters.AddWithValue("@pyear", pYear);
        //            cmd.Parameters.AddWithValue("@pcode", pCode);
        //            cmd.Parameters.AddWithValue("@tcode", tCode);
        //            cmd.Parameters.AddWithValue("@rate", rate);
        //            cmd = new SqlCommand(cmdStr1, conn, ts);                   
        //            cmd.Parameters.AddWithValue("@pid", pId);
        //            int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
        //            cmd = new SqlCommand(cmdStr2, conn, ts);
        //            cmd.Parameters.AddWithValue("@edate", edate);
        //            cmd.Parameters.AddWithValue("@aPcode", aPCode);
        //            cmd.Parameters.AddWithValue("@Tcode", aTCode);
        //            cmd.Parameters.AddWithValue("@aid", anthID);
        //            cmd.ExecuteNonQuery(); 
        //            ts.Commit();
        //            cmd.ExecuteNonQuery();
        //        }
        //        catch (SqlException sqlErr)
        //        {
        //            ts.Rollback();
        //        }
        //    }
        //}

        public static void Update(int PlhrId, string PlanYear, string Desc,string AnthemPlanCode, string AnthemTierCode,string PlanCode, string TierCode, string EDate, string Rate)
        {
            DateTime edate = Convert.ToDateTime(EDate);
            Decimal rate = Convert.ToDecimal(Rate);
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "UPDATE AnthPlanhier SET plhr_effdt = @edate,plhr_CompanyRate = @rate WHERE plhr_id = @pid";
            string cmdStr1 = "SELECT plhr_anthcd_id FROM AnthPlanhier WHERE plhr_id = @pid";
            string cmdStr2 = "UPDATE AnthCodes SET anthcd_effdt = @edate WHERE anthcd_id = @aid";

            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);                    
                    cmd.Parameters.AddWithValue("@edate", edate);                   
                    cmd.Parameters.AddWithValue("@rate", rate);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(cmdStr1, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    cmd = new SqlCommand(cmdStr2, conn, ts);
                    cmd.Parameters.AddWithValue("@edate", edate);                   
                    cmd.Parameters.AddWithValue("@aid", anthID);
                    cmd.ExecuteNonQuery();
                    ts.Commit();
                    
                }
                catch (SqlException sqlErr)
                {
                    ts.Rollback();
                }
            }
        }
        public static void UpdateE(int PlhrId, string PlanYear, string Desc, string AnthemPlanCode, string AnthemTierCode,string EDate, string Rate)
        {
            DateTime edate = Convert.ToDateTime(EDate);
            Decimal rate = Convert.ToDecimal(Rate);
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "UPDATE AnthPlanhier SET plhr_effdt = @edate,plhr_CompanyRate = @rate WHERE plhr_id = @pid";
            string cmdStr1 = "SELECT plhr_anthcd_id FROM AnthPlanhier WHERE plhr_id = @pid";
            string cmdStr2 = "UPDATE AnthCodes SET anthcd_effdt = @edate WHERE anthcd_id = @aid";

            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    cmd.Parameters.AddWithValue("@edate", edate);
                    cmd.Parameters.AddWithValue("@rate", rate);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(cmdStr1, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    cmd = new SqlCommand(cmdStr2, conn, ts);
                    cmd.Parameters.AddWithValue("@edate", edate);
                    cmd.Parameters.AddWithValue("@aid", anthID);
                    cmd.ExecuteNonQuery();
                    ts.Commit();

                }
                catch (SqlException sqlErr)
                {
                    ts.Rollback();
                }
            }
        }    

        public static List<CodesTable> SelectD(string p_sortExpression, string p_sortDirection)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesTable> cdObj = new List<CodesTable>();

            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt], [plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE ([plhr_plandesc] NOT LIKE 'INTERNATIONAL%' AND [plhr_plandesc] NOT LIKE 'EAP') AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cdObj.Add(new CodesTable(reader));
                reader.Close();
            }

            //We sort the generic list if requested too
            if (p_sortExpression != null && p_sortExpression != string.Empty)
            {
                cdObj.Sort(new ListObjectComparer(p_sortExpression));
            }

            //Now that we have sorted check to see if the sort direction is desc
            if (p_sortDirection != null && p_sortDirection == "Desc")
            {
                cdObj.Reverse();
            }

            return cdObj;
        }

        public static List<CodesTable> SelectI(string p_sortExpression, string p_sortDirection)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesTable> cdObj = new List<CodesTable>();

            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt], [plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_plandesc] LIKE 'INTERNATIONAL%' AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cdObj.Add(new CodesTable(reader));
                reader.Close();
            }

            //We sort the generic list if requested too
            if (p_sortExpression != null && p_sortExpression != string.Empty)
            {
                cdObj.Sort(new ListObjectComparer(p_sortExpression));
            }

            //Now that we have sorted check to see if the sort direction is desc
            if (p_sortDirection != null && p_sortDirection == "Desc")
            {
                cdObj.Reverse();
            }

            return cdObj;
        }        

        public static CodesTable SelectU(int PlhrId)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            CodesTable cdObj1 = null;
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt],CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],[plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_id] = @PlhrId AND [plhr_anthcd_id] = [anthcd_id]";

            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@PlhrId", SqlDbType.Int);
            cmd.Parameters["@PlhrId"].Value = PlhrId;
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    cdObj1 = new CodesTable(reader, "Update");
                reader.Close();

            }
            return cdObj1;
        }

        public static List<CodesTable> SelectE(string p_sortExpression, string p_sortDirection)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesTable> cdObj1 = new List<CodesTable>();
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt],[anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_plandesc] LIKE 'EAP' AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            cmd = new SqlCommand(cmdStr, conn);            
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                    cdObj1.Add(new CodesTable(reader, 0));
                reader.Close();

            }
            //We sort the generic list if requested too
            if (p_sortExpression != null && p_sortExpression != string.Empty)
            {
                cdObj1.Sort(new ListObjectComparer(p_sortExpression));
            }

            //Now that we have sorted check to see if the sort direction is desc
            if (p_sortDirection != null && p_sortDirection == "Desc")
            {
                cdObj1.Reverse();
            }
            return cdObj1;
        }

        public static CodesTable SelectEU(int PlhrId)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            CodesTable cdObj1 = null;
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt],CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],[anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_id] = @PlhrId AND [plhr_anthcd_id] = [anthcd_id]";

            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@PlhrId", SqlDbType.Int);
            cmd.Parameters["@PlhrId"].Value = PlhrId;
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    cdObj1 = new CodesTable(reader, 0);
                reader.Close();

            }
            return cdObj1;
        }


        public CodesTable(SqlDataReader reader)
        {
            plhrID = Convert.ToInt32(reader["plhr_id"]);
            plhrDesc = reader["plhr_plandesc"].ToString();
            plhrEdt = reader["plhr_effdt"].ToString();
            plhrPlancd = reader["plhr_plancd"].ToString();
            plhrRate = reader["plhr_CompanyRate"].ToString();
            plhrTiercd = reader["plhr_tiercd"].ToString();
            plhrYear = reader["plhr_py"].ToString();
            anthPlancd = reader["anthcd_plancd"].ToString();
            anthTiercd = reader["anthcd_covgcd"].ToString();
        }

        public CodesTable(SqlDataReader reader,string src)
        {
            plhrID = Convert.ToInt32(reader["plhr_id"]);
            plhrDesc = reader["plhr_plandesc"].ToString();
            plhrEdt = reader["plhr_effdt"].ToString();
            if (reader["plhr_plancd"] != DBNull.Value)
            {
                plhrPlancd = reader["plhr_plancd"].ToString();
            }
            plhrRate = reader["plhr_CompanyRate"].ToString();
            if (reader["plhr_tiercd"] != DBNull.Value)
            {
                plhrTiercd = reader["plhr_tiercd"].ToString();
            }
            plhrYear = reader["plhr_py"].ToString();
            anthPlancd = reader["anthcd_plancd"].ToString();
            anthTiercd = reader["anthcd_covgcd"].ToString();
        }

        public CodesTable(SqlDataReader reader, int type)
        {
            plhrID = Convert.ToInt32(reader["plhr_id"]);
            plhrDesc = reader["plhr_plandesc"].ToString();
            plhrEdt = reader["plhr_effdt"].ToString();            
            plhrRate = reader["plhr_CompanyRate"].ToString();            
            plhrYear = reader["plhr_py"].ToString();
            anthPlancd = reader["anthcd_plancd"].ToString();
            anthTiercd = reader["anthcd_covgcd"].ToString();
        }        
        
    }
}

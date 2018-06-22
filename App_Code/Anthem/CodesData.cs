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
/// Summary description for CodesData
/// </summary>
/// 

namespace EBA.Desktop
{
    public class CodesData
    {
        private static readonly string _connStr;

        private int plhrID;
        private string plhrDesc;
        private string plhrYear;        
        private string plhrPlancd = null;
        private string plhrTiercd = null;
        private string plhrRate;
        private string anthPlancd;
        private string anthTiercd;
        private string pstate;
        private string pmed;
        private string _yrmo;
        private string _comments;

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
        public string YRMO
        {
            get
            {
                return _yrmo;
            }
            set
            {
                _yrmo = value;
            }
        }
        public string Comm
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
            }
        }

        static CodesData()
        {
            _connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        }

        public static DataSet SelectD(string p_sortExpression, string p_sortDirection)
        {
            List<CodesData> cdObj = new List<CodesData>();
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);

            //CONVERT(VARCHAR(15),plhr_effdt,101) AS [plhr_effdt]
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(VARCHAR(20),plhr_CompanyRate,1) AS [plhr_CompanyRate],plhr_eff_yrmo, [plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE ([plhr_plandesc] NOT LIKE 'INTERNATIONAL%' AND [plhr_plandesc] NOT LIKE 'EAP') AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cdObj.Add(new CodesData(reader));
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

            DataSet dsDom = new DataSet();
            dsDom = addData(cdObj);
            return dsDom;
        }

        public static DataSet SelectI(string p_sortExpression, string p_sortDirection)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesData> cdObj = new List<CodesData>();

            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(Varchar(20),plhr_CompanyRate,1) AS [plhr_CompanyRate],plhr_eff_yrmo, [plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_plandesc] LIKE 'INTERNATIONAL%' AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            using (conn)
            {
                conn.Open();
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cdObj.Add(new CodesData(reader));
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

            DataSet dsIntl = new DataSet();
            dsIntl = addData(cdObj);
            return dsIntl;
        }

        public static DataSet SelectE(string p_sortExpression, string p_sortDirection)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesData> cdObj = new List<CodesData>();
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],CONVERT(Varchar(20),plhr_CompanyRate,1) AS [plhr_CompanyRate],plhr_eff_yrmo,[anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_plandesc] LIKE 'EAP' AND [plhr_anthcd_id] = [anthcd_id] ORDER BY [anthcd_plancd]";

            cmd = new SqlCommand(cmdStr, conn);
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    cdObj.Add(new CodesData(reader, 0));
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

            DataSet dsEAP = new DataSet();
            dsEAP = addData(cdObj);
            return dsEAP;
           
        }

        public static DataSet SelectU(int PlhrId)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesData> cdObj1 = new List<CodesData>();
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],plhr_eff_yrmo,CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],[plhr_plancd], [plhr_tiercd], [anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_id] = @PlhrId AND [plhr_anthcd_id] = [anthcd_id]";

            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@PlhrId", SqlDbType.Int);
            cmd.Parameters["@PlhrId"].Value = PlhrId;
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    cdObj1.Add(new CodesData(reader));
                reader.Close();

            }
            DataSet dsUp = new DataSet();
            dsUp= addData(cdObj1);
            return dsUp;
        }

        public static DataSet SelectEU(int PlhrId)
        {
            string cmdStr = null;
            SqlCommand cmd = null;
            SqlConnection conn = new SqlConnection(_connStr);
            List<CodesData> cdObj1 = new List<CodesData>();
            cmdStr = "SELECT [plhr_id],[plhr_plandesc],[plhr_py],plhr_eff_yrmo,CONVERT(Numeric(10,2),plhr_CompanyRate) AS [plhr_CompanyRate],[anthcd_covgcd],[anthcd_plancd]   FROM [AnthPlanhier],[AnthCodes] WHERE [plhr_id] = @PlhrId AND [plhr_anthcd_id] = [anthcd_id]";

            cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@PlhrId", SqlDbType.Int);
            cmd.Parameters["@PlhrId"].Value = PlhrId;
            using (conn)
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    cdObj1.Add(new CodesData(reader,0));
                reader.Close();

            }

            DataSet dsEAPU = new DataSet();
            dsEAPU = addData(cdObj1, 0);
            return dsEAPU;
        }

        public static void Insert(string Desc,string Comm, string PlanYear, string YRMO, string State, string Med, string PlanCode, string TierCode, string Rate, string AnthemPlanCode, string AnthemTierCode)
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
            HttpContext.Current.Session["anthID"] = 0;
            HttpContext.Current.Session["plhrID"] = 0;           
            Decimal rate = Convert.ToDecimal(Rate);
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "INSERT INTO AnthCodes (anthcd_py,anthcd_yrmo,anthcd_plancd,anthcd_covgcd) VALUES (@pyear,@eyrmo,@aPcode,@aTcode)";
            string cmdStr1 = "SELECT anthcd_id FROM AnthCodes WHERE anthcd_py = @pyear AND anthcd_yrmo = @eyrmo AND anthcd_plancd = @aPcode AND anthcd_covgcd = @aTcode";
            string cmdStr2 = "INSERT INTO AnthPlanhier (plhr_anthcd_id,plhr_plandesc,plhr_breakcd,plhr_eff_yrmo,plhr_py,plhr_plancd,plhr_tiercd,plhr_CompanyRate,plhr_comments) VALUES "
                                + " (@aId,@pdesc,@bcode,@eyrmo,@pyear,@pcode,@tcode,@rate,@comm)";
            string cmdStr3 = "SELECT plhr_id FROM AnthPlanhier WHERE "
                                + "plhr_anthcd_id = @aId AND plhr_plandesc = @pdesc AND plhr_breakcd = @bcode AND plhr_eff_yrmo = @eyrmo AND plhr_py = @pyear AND plhr_plancd = @pcode AND plhr_tiercd = @tcode AND plhr_CompanyRate = @rate AND plhr_comments = @comm";
            string cmdStr4 = "SELECT COUNT(*) FROM AnthCodes WHERE anthcd_py = @pyear AND anthcd_yrmo = @eyrmo AND anthcd_plancd = @aPcode AND anthcd_covgcd = @aTcode";
            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr4, conn, ts);
                    cmd.Parameters.AddWithValue("@pyear", PlanYear);
                    cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                    cmd.Parameters.AddWithValue("@aPcode", AnthemPlanCode.ToUpper());
                    cmd.Parameters.AddWithValue("@aTcode", AnthemTierCode.ToUpper());
                    int anthIDdup = Int32.Parse(cmd.ExecuteScalar().ToString());

                    if (anthIDdup == 0)
                    {
                        cmd = new SqlCommand(cmdStr, conn, ts);
                        cmd.Parameters.AddWithValue("@pyear", PlanYear);
                        cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                        cmd.Parameters.AddWithValue("@aPcode", AnthemPlanCode.ToUpper());
                        cmd.Parameters.AddWithValue("@aTcode", AnthemTierCode.ToUpper());
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand(cmdStr1, conn, ts);
                        cmd.Parameters.AddWithValue("@pyear", PlanYear);
                        cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                        cmd.Parameters.AddWithValue("@aPcode", AnthemPlanCode.ToUpper());
                        cmd.Parameters.AddWithValue("@aTcode", AnthemTierCode.ToUpper());
                        int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());

                        cmd = new SqlCommand(cmdStr2, conn, ts);
                        cmd.Parameters.AddWithValue("@aId", anthID);
                        cmd.Parameters.AddWithValue("@pdesc", Desc.ToUpper());
                        cmd.Parameters.AddWithValue("@bcode", _breakcd);
                        cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                        cmd.Parameters.AddWithValue("@pyear", PlanYear);                        
                        cmd.Parameters.AddWithValue("@pcode", PlanCode.ToUpper());
                        cmd.Parameters.AddWithValue("@tcode", TierCode.ToUpper());
                        cmd.Parameters.AddWithValue("@comm", Comm);                                               
                        cmd.Parameters.AddWithValue("@rate", rate);

                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand(cmdStr3, conn, ts);
                        cmd.Parameters.AddWithValue("@aId", anthID);
                        cmd.Parameters.AddWithValue("@pdesc", Desc.ToUpper());
                        cmd.Parameters.AddWithValue("@bcode", _breakcd);
                        cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                        cmd.Parameters.AddWithValue("@pyear", PlanYear);                        
                        cmd.Parameters.AddWithValue("@pcode", PlanCode.ToUpper());
                        cmd.Parameters.AddWithValue("@tcode", TierCode.ToUpper());
                        cmd.Parameters.AddWithValue("@comm", Comm);
                        cmd.Parameters.AddWithValue("@rate", rate);

                        int plhrID = Int32.Parse(cmd.ExecuteScalar().ToString());

                        ts.Commit();
                        HttpContext.Current.Session["anthID"] = anthID;
                        HttpContext.Current.Session["plhrID"] = plhrID;
                    }
                    else
                    {
                        throw (new Exception("Carrier Plan Code, Tier Code Already Exist!"));
                    }
                }                
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }                
            }
        }

        public static void Update(int PlhrId, string PlanYear, string Desc, string AnthemPlanCode, string AnthemTierCode, string PlanCode, string TierCode, string YRMO, string Rate)
        {            
            Decimal rate = Convert.ToDecimal(Rate);
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "UPDATE AnthPlanhier SET plhr_eff_yrmo = @eyrmo,plhr_CompanyRate = @rate WHERE plhr_id = @pid";
            string cmdStr1 = "SELECT plhr_anthcd_id FROM AnthPlanhier WHERE plhr_id = @pid";
            string cmdStr2 = "UPDATE AnthCodes SET anthcd_yrmo = @eyrmo WHERE anthcd_id = @aid";

            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                    cmd.Parameters.AddWithValue("@rate", rate);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(cmdStr1, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    cmd = new SqlCommand(cmdStr2, conn, ts);
                    cmd.Parameters.AddWithValue("@eyrmo", YRMO);
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

        public static void UpdateE(int PlhrId, string PlanYear, string Desc, string AnthemPlanCode, string AnthemTierCode, string YRMO, string Rate)
        {            
            Decimal rate = Convert.ToDecimal(Rate);
            SqlConnection conn = new SqlConnection(_connStr);
            SqlTransaction ts;
            SqlCommand cmd = null;
            string cmdStr = "UPDATE AnthPlanhier SET plhr_eff_yrmo = @eyrmo,plhr_CompanyRate = @rate WHERE plhr_id = @pid";
            string cmdStr1 = "SELECT plhr_anthcd_id FROM AnthPlanhier WHERE plhr_id = @pid";
            string cmdStr2 = "UPDATE AnthCodes SET anthcd_yrmo = @eyrmo WHERE anthcd_id = @aid";

            using (conn)
            {
                conn.Open();
                ts = conn.BeginTransaction();
                try
                {
                    cmd = new SqlCommand(cmdStr, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    cmd.Parameters.AddWithValue("@eyrmo", YRMO);
                    cmd.Parameters.AddWithValue("@rate", rate);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand(cmdStr1, conn, ts);
                    cmd.Parameters.AddWithValue("@pid", PlhrId);
                    int anthID = Int32.Parse(cmd.ExecuteScalar().ToString());
                    cmd = new SqlCommand(cmdStr2, conn, ts);
                    cmd.Parameters.AddWithValue("@eyrmo", YRMO);
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
   
        public CodesData(SqlDataReader reader)
        {
            plhrID = Convert.ToInt32(reader["plhr_id"]);
            plhrDesc = reader["plhr_plandesc"].ToString();
            _yrmo = reader["plhr_eff_yrmo"].ToString();
            plhrPlancd = reader["plhr_plancd"].ToString();
            plhrRate = reader["plhr_CompanyRate"].ToString();
            plhrTiercd = reader["plhr_tiercd"].ToString();
            plhrYear = reader["plhr_py"].ToString();
            anthPlancd = reader["anthcd_plancd"].ToString();
            anthTiercd = reader["anthcd_covgcd"].ToString();
        }
        public CodesData(SqlDataReader reader, int type)
        {
            plhrID = Convert.ToInt32(reader["plhr_id"]);
            plhrDesc = reader["plhr_plandesc"].ToString();
            _yrmo = reader["plhr_eff_yrmo"].ToString();            
            plhrRate = reader["plhr_CompanyRate"].ToString();            
            plhrYear = reader["plhr_py"].ToString();
            anthPlancd = reader["anthcd_plancd"].ToString();
            anthTiercd = reader["anthcd_covgcd"].ToString();
        }      

        protected static DataSet addData(List<CodesData> cdObj)
        {
            DataSet ds = new DataSet("Table");
            DataTable dt = new DataTable();
            DataColumn dc;
            dc = new DataColumn("PlhrId"); dt.Columns.Add(dc);
            dc = new DataColumn("Desc"); dt.Columns.Add(dc);
            dc = new DataColumn("PlanYear"); dt.Columns.Add(dc);
            dc = new DataColumn("Rate"); dt.Columns.Add(dc);
            dc = new DataColumn("YRMO"); dt.Columns.Add(dc);
            dc = new DataColumn("PlanCode"); dt.Columns.Add(dc);
            dc = new DataColumn("TierCode"); dt.Columns.Add(dc);
            dc = new DataColumn("AnthemTierCode"); dt.Columns.Add(dc);
            dc = new DataColumn("AnthemPlanCode"); dt.Columns.Add(dc);

            DataRow row;
            foreach (CodesData cd in cdObj)
            {
                row = dt.NewRow();
                row["PlhrId"] = cd.PlhrId;
                row["Desc"] = cd.Desc;
                row["PlanYear"] = cd.PlanYear;
                row["Rate"] = cd.Rate;
                row["YRMO"] = cd.YRMO;
                row["PlanCode"] = cd.PlanCode;
                row["TierCode"] = cd.TierCode;
                row["AnthemTierCode"] = cd.AnthemTierCode;
                row["AnthemPlanCode"] = cd.AnthemPlanCode;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            return ds;
        }

        protected static DataSet addData(List<CodesData> cdObj,int src)
        {
            DataSet ds = new DataSet("Table");
            DataTable dt = new DataTable();
            DataColumn dc;
            dc = new DataColumn("PlhrId"); dt.Columns.Add(dc);
            dc = new DataColumn("Desc"); dt.Columns.Add(dc);
            dc = new DataColumn("PlanYear"); dt.Columns.Add(dc);
            dc = new DataColumn("Rate"); dt.Columns.Add(dc);
            dc = new DataColumn("YRMO"); dt.Columns.Add(dc);            
            dc = new DataColumn("AnthemTierCode"); dt.Columns.Add(dc);
            dc = new DataColumn("AnthemPlanCode"); dt.Columns.Add(dc);

            DataRow row;
            foreach (CodesData cd in cdObj)
            {
                row = dt.NewRow();
                row["PlhrId"] = cd.PlhrId;
                row["Desc"] = cd.Desc;
                row["PlanYear"] = cd.PlanYear;
                row["Rate"] = cd.Rate;
                row["YRMO"] = cd.YRMO;                
                row["AnthemTierCode"] = cd.AnthemTierCode;
                row["AnthemPlanCode"] = cd.AnthemPlanCode;
                dt.Rows.Add(row);
            }

            ds.Tables.Add(dt);
            return ds;
        }

        public static DataSet ddlSelect()
        {
            DataSet dsState = new DataSet();
            DataRow dsRow;
            DataTable dsTbl = new DataTable("StateTb");
            dsState.Tables.Add(dsTbl);
            DataColumn dsCol = new DataColumn("StateName");
            dsTbl.Columns.Add(dsCol);
            dsRow = dsTbl.NewRow();
            dsRow["StateName"] = "CA";
            dsTbl.Rows.Add(dsRow);
            dsRow = dsTbl.NewRow();
            dsRow["StateName"] = "Non-CA";
            dsTbl.Rows.Add(dsRow);            
            return dsState;
        }
    }
}

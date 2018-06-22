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

namespace EBA.Desktop.IPBA
{
    /// <summary>
    /// Summary description for BenhierDAL
    /// </summary>
    public class BenhierDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;
        public BenhierDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DataSet getplanhierY(string _eyrmo)
        {
            string _cmdstr = "SELECT [ph_plangrp],[ph_plandesc],[ph_plancd],[ph_tiercd] "
                                            + " FROM [Planhier],[Benefithier] "
                                            + " WHERE [ph_yrmo]  = '" + _eyrmo + "' "
                                            + " AND [bnhr_id] = [ph_ben_id]";
            DataSet dsplan = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsplan);
                return dsplan;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getplanhierB(string _bencd, string _eyrmo)
        {
            string _cmdstr = "SELECT [ph_plangrp],[ph_plandesc],[ph_plancd],[ph_tiercd] "
                                            + " FROM [Planhier],[Benefithier] WHERE "
                                            + " [ph_yrmo]  = '" + _eyrmo + "' AND [bnhr_benft_grp] = '" + _bencd + "' "
                                            + " AND [bnhr_id] = [ph_ben_id]";
            DataSet dsplan = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsplan);
                return dsplan;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getplanhierA(string _progcd, string _eyrmo)
        {
            string _cmdstr = "SELECT [ph_id],[ph_plangrp],[ph_plandesc],[ph_plancd],[ph_tiercd],[ph_yrmo] "
                                            + " FROM [Planhier],[Benefithier] WHERE "
                                            + " [ph_yrmo]  = '" + _eyrmo + "' AND [ph_progmcd] = '" + _progcd + "' "
                                            + " AND [bnhr_id] = [ph_ben_id]";
            DataSet dsplan = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsplan);
                return dsplan;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getplanhierP(string _progcd, string _eyrmo, string _bencd)
        {
            string _cmdstr = "SELECT [ph_id],[ph_plangrp],[ph_plandesc],[ph_plancd],[ph_tiercd],[ph_yrmo] "
                                            + " FROM [Planhier],[Benefithier] WHERE [ph_progmcd] = '" + _progcd + "' "
                                            + " AND [ph_yrmo]  = '" + _eyrmo + "' AND [bnhr_benft_grp] = '" + _bencd + "' "
                                            + " AND [bnhr_id] = [ph_ben_id]";
            DataSet dsplan = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsplan);
                return dsplan;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getplanRatesA(string _py, string _eyrmo, string _progcd)
        {
            string _cmdstr = "SELECT [rate_id],[ph_plandesc],[ph_plancd],[ph_tiercd],[rate_rateamt],[rate_companyRtamt],[rate_effyrmo] "
                                            + " FROM [Planhier],[Rates],[Benefithier] WHERE [ph_progmcd] = '" + _progcd + "' "
                                            + " AND [rate_effyrmo]  = '" + _eyrmo + "'"
                                            + " AND [rate_py] = '" + _py + "' AND [rate_plhr_id] = [ph_id] "
                                            + " AND [bnhr_id] = [ph_ben_id] ";
            DataSet dsrate = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsrate);
                return dsrate;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getplanRates(string _py, string _eyrmo, string _progcd, string _bencd)
        {
            string _cmdstr = "SELECT [rate_id],[ph_plandesc],[ph_plancd],[ph_tiercd],[rate_rateamt],[rate_companyRtamt],[rate_effyrmo] "
                                            + " FROM [Planhier],[Rates],[Benefithier] WHERE [ph_progmcd] = '" + _progcd + "' "
                                            + " AND [rate_effyrmo]  = '" + _eyrmo + "' AND [bnhr_benft_grp] = '" + _bencd + "' "
                                            + " AND [rate_py] = '"+ _py + "' AND [rate_plhr_id] = [ph_id] "
                                            + " AND [bnhr_id] = [ph_ben_id] ";
            DataSet dsrate = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsrate);
                return dsrate;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }       

        public static string planDesc(string _pcode)
        {
            string _cmdstr = "SELECT ph_plandesc FROM [Planhier] WHERE ph_plancd= @pcode";
            string _d = "";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@pcode", _pcode);
                _d = command.ExecuteScalar().ToString();
                return _d;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static List<string> getPYyrmo(string _pyear)
        {
            string _cmdstr = "SELECT DISTINCT rate_effyrmo FROM [Rates] WHERE rate_py= @py";
            List<string> _yr = new List<string>();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@py", _pyear);
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _yr.Add(reader[0].ToString());
                }
                reader.Close();
                return _yr;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static List<string> getRatePlancodes(string _progmcd)
        {
            string _cmdstr = "SELECT DISTINCT [ph_plancd] FROM [Planhier] WHERE ph_progmcd = @pg";
            List<string> _plans = new List<string>();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@pg", _progmcd);
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _plans.Add(reader[0].ToString());
                }
                reader.Close();
                return _plans;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static List<string> getRateTiercodes( string _pcode)
        {
            string _cmdstr = "SELECT DISTINCT [ph_tiercd] FROM [Planhier] WHERE ph_plancd = @pcode";
            List<string> _tiers = new List<string>();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);                
                command.Parameters.AddWithValue("@pcode", _pcode);
                SqlDataReader reader;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    _tiers.Add(reader[0].ToString());
                }
                reader.Close();
                return _tiers;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void updatePlanhier(int _id, string _group, string _plancd, string _desc, string _tiercd, string _yrmo)
        {
            string _cmdstr = "UPDATE [Planhier] SET ph_plangrp = @pg, ph_plancd = @pcode, ph_plandesc = @dsc, ph_tiercd = @tcode, ph_yrmo = @yrmo "
                                + " WHERE ph_id = @id ";
            SqlTransaction ts;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {               
                command = new SqlCommand(_cmdstr, connect,ts);
                command.Parameters.AddWithValue("@pg", _group);
                command.Parameters.AddWithValue("@pcode", _plancd);
                command.Parameters.AddWithValue("@dsc", _desc);
                command.Parameters.AddWithValue("@tcode", _tiercd);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@id", _id);
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw (new Exception("Error Updating record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void deletePlanhier(int _id)
        {
            string _cmdstr = "DELETE [Planhier] WHERE ph_id = @id ";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }               
                command = new SqlCommand(_cmdstr, connect);                
                command.Parameters.AddWithValue("@id", _id);
                command.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                
                throw (new Exception("Error Deleting record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void insertPlanhier(int _benid, string _progcd, string _group, string _plancd, string _desc, string _tiercd, string _yrmo)
        {
            string _cmdstr = "INSERT INTO [Planhier] (ph_ben_id, ph_py,ph_progmcd, ph_plangrp, ph_plancd, ph_plandesc, ph_tiercd, ph_yrmo) "
                                + " VALUES (@bid,@py,@prog,@pg,@pcode,@dsc,@tcode,@yrmo) ";

            string _cmdstr0 = "SELECT Count(*) FROM [Planhier] "
                                + " WHERE ph_plangrp = @pg AND ph_plancd = @pcode AND ph_plandesc = @dsc AND ph_tiercd = @tcode "
                                + " AND ph_progmcd = @prog AND ph_ben_id = @bid";

            string _cmdstr1 = "SELECT ph_id FROM [Planhier] "
                                + " WHERE ph_plangrp = @pg AND ph_plancd = @pcode AND ph_plandesc = @dsc AND ph_tiercd = @tcode "
                                + " AND ph_progmcd = @prog AND ph_ben_id = @bid";
            int _foundId = 0;

            SqlTransaction ts;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr0, connect,ts);
                command.Parameters.AddWithValue("@pg", _group);               
                command.Parameters.AddWithValue("@prog", _progcd);
                command.Parameters.AddWithValue("@pcode", _plancd);
                command.Parameters.AddWithValue("@dsc", _desc);
                command.Parameters.AddWithValue("@tcode", _tiercd);               
                command.Parameters.AddWithValue("@bid", _benid);
                _foundId = Convert.ToInt32(command.ExecuteScalar());
                if (_foundId > 0)
                {
                    throw (new Exception(" Plan hierarchy already defined!"));
                }
                else
                {
                    command = new SqlCommand(_cmdstr, connect, ts);
                    command.Parameters.AddWithValue("@pg", _group);
                    command.Parameters.AddWithValue("@py", _yrmo.Substring(0, 4));
                    command.Parameters.AddWithValue("@prog", _progcd);
                    command.Parameters.AddWithValue("@pcode", _plancd);
                    command.Parameters.AddWithValue("@dsc", _desc);
                    command.Parameters.AddWithValue("@tcode", _tiercd);
                    command.Parameters.AddWithValue("@yrmo", _yrmo);
                    command.Parameters.AddWithValue("@bid", _benid);
                    command.ExecuteNonQuery();

                    command = new SqlCommand(_cmdstr1, connect,ts);
                    command.Parameters.AddWithValue("@pg", _group);
                    command.Parameters.AddWithValue("@py", _yrmo.Substring(0, 4));
                    command.Parameters.AddWithValue("@prog", _progcd);
                    command.Parameters.AddWithValue("@pcode", _plancd);
                    command.Parameters.AddWithValue("@dsc", _desc);
                    command.Parameters.AddWithValue("@tcode", _tiercd);
                    command.Parameters.AddWithValue("@yrmo", _yrmo);
                    command.Parameters.AddWithValue("@bid", _benid);
                    int _phid = Convert.ToInt32(command.ExecuteScalar());
                    HttpContext.Current.Session["phid"] = _phid;
                }
                ts.Commit();
            }                
            catch (Exception ex)
            {
                ts.Rollback();
                if (ex.Message.Contains("Plan hierarchy already defined!"))
                {
                    throw (new Exception("Error Inserting record - Plan hierarchy already defined!"));
                }
                else
                {
                    throw (new Exception("Error Inserting record!"));
                }
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void updateRates(int _id, decimal _rate1, decimal _corate)
        {
            string _cmdstr = "UPDATE [Rates] SET rate_rateamt= @rate1, rate_companyRtamt = @corate"
                                + " WHERE rate_id = @id ";
            string _cmdstr1 = "SELECT rate_plhr_id FROM [Rates] WHERE rate_id = @id ";
            SqlTransaction ts;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr1, connect,ts);
                command.Parameters.AddWithValue("@id", _id);
                HttpContext.Current.Session["ratephid"] = command.ExecuteScalar();

                command = new SqlCommand(_cmdstr, connect, ts);                
                command.Parameters.AddWithValue("@rate1", _rate1);
                command.Parameters.AddWithValue("@corate", _corate);
                command.Parameters.AddWithValue("@id", _id);
                command.ExecuteNonQuery();
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                throw (new Exception("Error Updating record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void deleteRates(int _id)
        {
            string _cmdstr = "SELECT rate_plhr_id FROM [Rates] WHERE rate_id = @id ";
            string _cmdstr1 = "DELETE [Rates] WHERE rate_id = @id ";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            
            try
            {                
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@id", _id);
                HttpContext.Current.Session["ratephid"] = command.ExecuteScalar();
                command = new SqlCommand(_cmdstr1, connect);
                command.Parameters.AddWithValue("@id", _id);
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw (new Exception("Error Deleting record!"));
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static void insertRates(ratecolumns inValues,string _pgcode)
        {
            string _cmdstr = "SELECT ph_id FROM [Planhier] "
                                + " WHERE ph_plancd = @pcode AND ph_plandesc = @dsc AND ph_tiercd = @tcode AND ph_progmcd = @prog";
 
            string _cmdstr1 = "SELECT COUNT(*) FROM [Rates] "
                                + " WHERE rate_plhr_id = @pid AND rate_rateamt = @r1 AND rate_companyRtamt = @r2 AND rate_effyrmo = @yrmo AND rate_py = @py";
            
            string _cmdstr2 = "INSERT INTO [Rates] (rate_plhr_id, rate_rateamt,rate_companyRtamt,rate_effyrmo,rate_py) "
                                + " VALUES (@pid,@r1,@r2,@yrmo,@py) ";

            string _cmdstr3 =  "SELECT rate_id FROM [Rates] "
                                + " WHERE rate_plhr_id = @pid AND rate_rateamt = @r1 AND rate_companyRtamt = @r2 AND rate_effyrmo = @yrmo AND rate_py = @py";
            
            int _plhrid = 0;
            int _foundid = 0;
            SqlTransaction ts;
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            ts = connect.BeginTransaction();
            try
            {
                command = new SqlCommand(_cmdstr, connect, ts);
                command.Parameters.AddWithValue("@pcode", inValues.PlanCode);
                command.Parameters.AddWithValue("@dsc", inValues.PlanDesc.Trim());
                command.Parameters.AddWithValue("@tcode", inValues.TierCode);
                command.Parameters.AddWithValue("@prog", _pgcode);
                if (!command.ExecuteScalar().Equals(DBNull.Value))
                {
                    _plhrid = Convert.ToInt32(command.ExecuteScalar());
                }
                if (_plhrid != 0)
                {
                    command = new SqlCommand(_cmdstr1, connect, ts);
                    command.Parameters.AddWithValue("@pid", _plhrid);
                    command.Parameters.AddWithValue("@r1", inValues.EErate);
                    command.Parameters.AddWithValue("@r2", inValues.COrate);
                    command.Parameters.AddWithValue("@yrmo", inValues.EffYrmo);
                    command.Parameters.AddWithValue("@py", inValues.EffYrmo.Substring(0,4));
                    _foundid = Convert.ToInt32(command.ExecuteScalar());
                    if (_foundid > 0)
                    {
                        throw (new Exception(" Rates for selected plan are already defined!"));
                    }
                    else
                    {
                        command = new SqlCommand(_cmdstr2, connect, ts);
                        command.Parameters.AddWithValue("@pid", _plhrid);
                        command.Parameters.AddWithValue("@r1", inValues.EErate);
                        command.Parameters.AddWithValue("@r2", inValues.COrate);
                        command.Parameters.AddWithValue("@yrmo", inValues.EffYrmo);
                        command.Parameters.AddWithValue("@py", inValues.EffYrmo.Substring(0, 4));
                        command.ExecuteNonQuery();

                        command = new SqlCommand(_cmdstr3, connect, ts);
                        command.Parameters.AddWithValue("@pid", _plhrid);
                        command.Parameters.AddWithValue("@r1", inValues.EErate);
                        command.Parameters.AddWithValue("@r2", inValues.COrate);
                        command.Parameters.AddWithValue("@yrmo", inValues.EffYrmo);
                        command.Parameters.AddWithValue("@py", inValues.EffYrmo.Substring(0, 4));
                        HttpContext.Current.Session["rateid"] = command.ExecuteScalar();
                        HttpContext.Current.Session["ratephid"] = _plhrid;
                    }
                }
                ts.Commit();
            }
            catch (Exception ex)
            {
                ts.Rollback();
                if (ex.Message.Contains("Rates for selected plan are already defined!"))
                {
                    throw (new Exception("Error Inserting record - Rates for selected plan are already defined!"));
                }
                else
                {
                    throw (new Exception("Error Inserting record!"));
                }
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static planhierRecord oldplanhierValues(int _phid)
        {
            planhierRecord oRec = new planhierRecord();
            string _cmdstr = "SELECT ph_plangrp,ph_plancd,ph_plandesc,ph_tiercd,ph_yrmo FROM [Planhier] "
                               + " WHERE ph_id = @pid";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@pid", _phid);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oRec.PlanGroup = reader[0].ToString();
                    oRec.PlanCode = reader[1].ToString();
                    oRec.PlanDesc = reader[2].ToString();
                    oRec.TierCode = reader[3].ToString();
                    oRec.EffYrmo = reader[4].ToString();
                }
                reader.Close();                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return oRec;
        }

        public static ratecolumns oldRatesValues(int _rateid)
        {
            ratecolumns oRec1 = new ratecolumns();
            string _cmdstr = "SELECT rate_rateamt, rate_companyRtamt FROM [Rates] "
                               + " WHERE rate_id = @rid";
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }

            try
            {
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@rid", _rateid);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oRec1.EErate = Convert.ToDecimal(reader[0]);
                    oRec1.COrate = Convert.ToDecimal(reader[1]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
            return oRec1;
        }

        public static DataSet benhierReport()
        {
            string _cmdstr = "SELECT [bnhr_progm] AS [Program],[bnhr_progcd] AS [Program Code],[ph_plangrp] AS [Group], " 
                                            + "[ph_plandesc] AS [Description],[ph_plancd] AS [Plan Code],[ph_tiercd] AS [Tier Code], "
                                            + " [ph_yrmo] AS [Effective YRMO] "
                                            + " FROM [Planhier],[Benefithier] "
                                            + " WHERE [bnhr_id] = [ph_ben_id] ORDER BY [bnhr_progcd]";
            DataSet dsbenRpt1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsbenRpt1);
                return dsbenRpt1;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet benhierReport(string _prgcd)
        {
            string _cmdstr = "SELECT [bnhr_progm] AS [Program],[bnhr_progcd] AS [Program Code],[ph_plangrp] AS [Group], "
                                            + "[ph_plandesc] AS [Description],[ph_plancd] AS [Plan Code],[ph_tiercd] AS [Tier Code], "
                                            + " [ph_yrmo] AS [Effective YRMO] "
                                            + " FROM [Planhier],[Benefithier] "
                                            + " WHERE [bnhr_id] = [ph_ben_id] AND [bnhr_progcd] = @prgcd ORDER BY [bnhr_progcd]";
            DataSet dsbenRpt2 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@prgcd", _prgcd);
                da.SelectCommand = command;
                da.Fill(dsbenRpt2);
                return dsbenRpt2;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet rateReport(string _pyear)
        {
            string _cmdstr = "SELECT [rate_py] AS [Plan Year],[bnhr_progm] AS [Program],[ph_progmcd] AS [Program Code], "
                                + " [ph_plangrp] AS [Group],[ph_plandesc] AS [Description],[ph_plancd] AS [Plan Code], "
                                + " [ph_tiercd] AS [Tier Code] , [rate_rateamt] AS [EE Rate],[rate_companyRtamt] AS [Premium], "
                                + " [rate_effyrmo] AS [Rate Effective YRMO] " 
                                + " FROM [Planhier],[Benefithier],[Rates] "
                                + " WHERE [bnhr_id] = [ph_ben_id] AND [rate_plhr_id] = [ph_id] "
                                + " AND [rate_py] = @py "
                                + " ORDER BY [ph_progmcd],[ph_plangrp],[ph_plancd],[rate_companyRtamt] ASC";

            DataSet dsrateRpt1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@py", _pyear);
                da.SelectCommand = command;
                da.Fill(dsrateRpt1);
                return dsrateRpt1;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet rateReport(string _pyear,string _prgcode)
        {
            string _cmdstr = "SELECT [rate_py] AS [Plan Year],[bnhr_progm] AS [Program],[ph_progmcd] AS [Program Code], "
                                + " [ph_plangrp] AS [Group],[ph_plandesc] AS [Description],[ph_plancd] AS [Plan Code], "
                                + " [ph_tiercd] AS [Tier Code] , [rate_rateamt] AS [EE Rate],[rate_companyRtamt] AS [Premium], "
                                + " [rate_effyrmo] AS [Rate Effective YRMO] "
                                + " FROM [Planhier],[Benefithier],[Rates] "
                                + " WHERE [bnhr_id] = [ph_ben_id] AND [rate_plhr_id] = [ph_id] "
                                + " AND [ph_progmcd] = @pgcode "
                                + " AND [rate_py] = @py "
                                + " ORDER BY [ph_progmcd],[ph_plangrp],[ph_plancd],[rate_companyRtamt] ASC";

            DataSet dsrateRpt2 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                command.Parameters.AddWithValue("@pgcode", _prgcode);
                command.Parameters.AddWithValue("@py", _pyear);
                da.SelectCommand = command;
                da.Fill(dsrateRpt2);
                return dsrateRpt2;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet rateTemplate(string _pyear)
        {
            string _pyrmo = _pyear + "01";
            string _cmdstr = "SELECT '" + _pyear + "' AS [Plan Year],[bnhr_progm] AS [Program],[ph_progmcd] AS [Program Code], "
                                + " [ph_plangrp] AS [Group],[ph_plandesc] AS [Description],[ph_plancd] AS [Plan Code], "
                                + " [ph_tiercd] AS [Tier Code] , '' AS [EE Rate],'' AS [Premium], "
                                + " '" + _pyrmo + "' AS [Rate Effective YRMO] "
                                + " FROM [Planhier],[Benefithier],[Rates] "
                                + " WHERE [bnhr_id] = [ph_ben_id] AND [rate_plhr_id] = [ph_id] "
                                + " ORDER BY [ph_progmcd],[ph_plangrp],[ph_plancd],[rate_companyRtamt] ASC";

            DataSet dsrateTemp1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);                
                da.SelectCommand = command;
                da.Fill(dsrateTemp1);
                return dsrateTemp1;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static bool checkRatePY(string _py)
        {
            bool _found = false;
            string _cmdstr = "SELECT COUNT(*) "
                                + " FROM [Rates] "
                                + " WHERE [rate_py] = @py";            
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);                
                command.Parameters.AddWithValue("@py", _py);
                int _cnt = Convert.ToInt32(command.ExecuteScalar());
                if (_cnt > 0)
                {
                    _found = true;
                }
                return _found;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getPlanhierAuditReport()
        {
            string _cmdstr = " SELECT [bnhr_progcd] AS [Program Code],tType AS [Task],tColumn AS [Column],tOldValue AS [Old Value], "
                            + " [tNewValue] AS [New Value],[UserName] As [User],taskDate AS [Date Time] "
                            + " FROM UserSession_AU, UserTasks_AU, Benefithier "
                            + " WHERE bnhr_id = tPrimaryName "
                            + " AND tTable = 'Planhier' AND ModuleName = 'IPBA' "
                            + " AND UserSession_AU.SessionId = UserTasks_AU.sessionID ";

            DataSet dsAudit1 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsAudit1);
                return dsAudit1;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

        public static DataSet getRateAuditReport()
        {
            string _cmdstr = "SELECT [ph_progmcd] AS [Program Code],[ph_plancd] AS [Plan Code], "
                            + " [ph_tiercd] AS [Tier Code],tType AS [Task],tColumn AS [Column],tOldValue AS [Old Value], "
                            + " [tNewValue] AS [New Value],[UserName] As [User],taskDate AS [Date Time] "
                            + " FROM UserSession_AU, UserTasks_AU, Planhier "
                            + " WHERE ph_id = tPrimaryName "
                            + " AND tTable = 'Rates' AND ModuleName = 'IPBA' "
                            + " AND UserSession_AU.SessionId = UserTasks_AU.sessionID";

            string _cmdstr1 = "SELECT [ph_progmcd] AS [Program Code],[ph_plancd] AS [Plan Code], "
                            + " [ph_tiercd] AS [Tier Code],tType AS [Task],tColumn AS [Column],tOldValue AS [Old Value], "
                            + " [tNewValue] AS [New Value],[UserName] As [User],taskDate AS [Date Time] "
                            + " FROM UserSession_AU, UserTasks_AU, Planhier_History "
                            + " WHERE ph_id = tPrimaryName "
                            + " AND tTable = 'Rates' AND ModuleName = 'IPBA' "
                            + " AND UserSession_AU.SessionId = UserTasks_AU.sessionID";

            DataSet dsAudit11 = new DataSet();
            DataSet dsAudit12 = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(dsAudit11);

                command = new SqlCommand(_cmdstr1, connect);
                da.SelectCommand = command;
                da.Fill(dsAudit12);

                dsAudit11.Merge(dsAudit12); dsAudit12.Clear();

                return dsAudit11;
            }
            finally
            {
                connect.Close();
                command.Dispose();
            }
        }

    }

    public class planhierRecord
    {
        private string _group = null;
        private string _pcode = null;
        private string _descr = null;
        private string _tiercd = null;
        private string _effyrmo = null;

        public string PlanGroup
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
            }
        }
        public string PlanCode
        {
            get
            {
                return _pcode;
            }
            set
            {
                _pcode = value;
            }
        }
        public string PlanDesc
        {
            get
            {
                return _descr;
            }
            set
            {
                _descr = value;
            }
        }
        public string TierCode
        {
            get
            {
                return _tiercd;
            }
            set
            {
                _tiercd = value;
            }
        }
        public string EffYrmo
        {
            get
            {
                return _effyrmo;
            }
            set
            {
                _effyrmo = value;
            }
        }
    }

    public class ratecolumns
    {        
        private string _pcode = null;
        private string _descr = null;
        private string _tiercd = null;
        private string _effyrmo = null;
        private decimal _amt1 = 0;
        private decimal _coamt = 0;

        
        public string PlanCode
        {
            get
            {
                return _pcode;
            }
            set
            {
                _pcode = value;
            }
        }
        public string PlanDesc
        {
            get
            {
                return _descr;
            }
            set
            {
                _descr = value;
            }
        }
        public string TierCode
        {
            get
            {
                return _tiercd;
            }
            set
            {
                _tiercd = value;
            }
        }
        public string EffYrmo
        {
            get
            {
                return _effyrmo;
            }
            set
            {
                _effyrmo = value;
            }
        }
        public decimal EErate
        {
            get
            {
                return _amt1;
            }
            set
            {
                _amt1 = value;
            }
        }
        public decimal COrate
        {
            get
            {
                return _coamt;
            }
            set
            {
                _coamt = value;
            }
        }
    }
}

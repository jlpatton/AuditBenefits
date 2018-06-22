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

namespace EBA.Desktop.Anthem
{
    public class CAClaimsDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);       
        static SqlCommand command = null;

        public CAClaimsDAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public Boolean Reconciled(string yrmo)
        {
            string cmdstr = "SELECT reconciled FROM CAClaimsRecon WHERE yrmo = '" + yrmo + "'";
            Boolean reconciled = false;
            
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                int i = Convert.ToInt32(command.ExecuteScalar());
                if (i == 1)
                    reconciled = true;
                command.Dispose();
                return reconciled;
            }
            finally
            {
                connect.Close();
            }
        }

        public void DeleteReconData(string yrmo)
        {
            try
            {
                ResetPrevCF(yrmo);
                ResetPrevDup(yrmo);
                DeleteFromReconTable(yrmo);
                ResetCurCFandDup(yrmo);
            }
            finally
            {
                if (connect != null && connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }

        public void SetAnthDup(string yrmo)
        {
            string cmdstr1 = "UPDATE AnthBillTrans SET anbt_Dup = 1, anbt_CF = 1 "
                            + " WHERE anbt_checkNum IN"
                            + " (select anbt_checkNum FROM AnthBillTrans WHERE"
                            + " anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'"
                            + " GROUP BY anbt_checkNum HAVING COUNT(anbt_checkNum) > 1 )"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";
            string cmdstr2 = "UPDATE AnthBillTrans SET anbt_DupPrev = 1"
                            + " WHERE anbt_checkNum IN (SELECT anbt_checkNum FROM AnthBillTrans"
                            + " WHERE anbt_checkNum"
                            + " IN (Select anbt_checkNum FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'"
                            + " GROUP BY anbt_checkNum HAVING COUNT(anbt_checkNum) >= 1)"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo < '" + yrmo + "')"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "'";
            string cmdstr3 = "UPDATE AnthBillTrans SET anbt_CF = 1"
                            + " WHERE anbt_checkNum IN (SELECT anbt_checkNum FROM AnthBillTrans"
                            + " WHERE anbt_checkNum"
                            + " IN (Select anbt_checkNum FROM AnthBillTrans WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'"
                            + " GROUP BY anbt_checkNum HAVING COUNT(anbt_checkNum) = 1)"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo < '" + yrmo + "')"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";
            string cmdstr4 = "UPDATE BOAStatement SET boaDupPrev = 1, anbtDupPrevYRMO = '" + yrmo + "' "
                            + "WHERE boaChqNo IN "
                            + "(select anbt_checkNum FROM AnthBillTrans WHERE "
                            + "anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "' "
                            + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) ) "
                            + "AND boaYRMO <= '" + yrmo + "' AND boaDup = 0 AND boaDupPrev = 0";
            string cmdstr5 = "UPDATE BOAStatement SET boaCF = 1 "
                            + "WHERE boaDupPrev = 1 "                            
                            + "AND boaYRMO = '" + yrmo + "'";
            string cmdstr6 = "UPDATE AnthBillTrans SET anbt_Dup = 1, anbt_CF = 1 "
                            + " WHERE anbt_checkNum IN (SELECT boaChqNo FROM BOAStatement WHERE boaYRMO < '" + yrmo + "' "
                            + "AND (boaDup = 1 OR boaDupPrev = 1) )"
                            + " AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";


            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr3, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr4, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr5, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr6, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public void SetBOADup(string yrmo)
        {
            string cmdstr1 = "UPDATE BOAStatement SET boaDup = 1, boaCF = 1 "
                            + " WHERE boaChqNo  IN"
                            + " ( SELECT boaChqNo FROM BOAStatement WHERE boaYRMO = '" + yrmo + "'"
                            + " GROUP BY boaChqNo HAVING COUNT(boaChqNo) > 1 )"
                            + " AND boaYRMO = '" + yrmo + "'";
            string cmdstr2 = "UPDATE BOAStatement"
                            + " SET boaDupPrev = 1"
                            + " WHERE boaChqNo  IN"
                            + " ( SELECT boaChqNo FROM BOAStatement WHERE boaChqNo IN"
                            + " (SELECT boaChqNo FROM BOAStatement WHERE boaYRMO = '" + yrmo + "' GROUP BY boaChqNo HAVING COUNT(boaChqNo) >= 1 )"
                            + " AND boaYRMO < '" + yrmo + "' ) AND boaYRMO <= '" + yrmo + "'";
            string cmdstr3 = "UPDATE BOAStatement"
                            + " SET boaCF = 1"
                            + " WHERE boaChqNo  IN"
                            + " ( SELECT boaChqNo FROM BOAStatement WHERE boaChqNo IN"
                            + " (SELECT boaChqNo FROM BOAStatement WHERE boaYRMO = '" + yrmo + "' GROUP BY boaChqNo HAVING COUNT(boaChqNo) = 1 )"
                            + " AND boaYRMO < '" + yrmo + "' ) AND boaYRMO = '" + yrmo + "'";
            string cmdstr4 = "UPDATE AnthBillTrans SET anbt_DupPrev = 1, boaDupPrevYRMO = '" + yrmo + "' "
                            + "WHERE anbt_checkNum IN "
                            + "( SELECT boaChqNo FROM BOAStatement WHERE boaYRMO = '" + yrmo + "' "
                            + "AND (boaDup = 1 OR boaDupPrev = 1) ) "
                            + "AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "' AND anbt_Dup = 0 AND anbt_DupPrev = 0";

            string cmdstr5 = "UPDATE AnthBillTrans SET anbt_CF = 1 WHERE anbt_DupPrev = 1 AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo = '" + yrmo + "'";

            string cmdstr6 = "UPDATE BOAStatement"
                            + " SET boaDup = 1, boaCF = 1 "
                            + " WHERE boaChqNo  IN"
                            + " (select anbt_checkNum FROM AnthBillTrans WHERE "
                            + "anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo < '" + yrmo + "' "
                            + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) ) AND boaYRMO = '" + yrmo + "'";


            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr3, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr4, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr5, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr6, connect);
                command.ExecuteNonQuery();
                command.Dispose();

            }
            finally
            {
                connect.Close();
            }
        }

        public void SetCurrentCF(string yrmo)
        {
            SetAmtMismatchCurCF(yrmo);
            SetAnthMismatchCurCF(yrmo);
            SetBOAMismatchCurCF(yrmo);
        }

        public void UpdatePrevCF(string yrmo)
        {            
            UpdateAnthPrevCF(yrmo);
            UpdateBOAPrevCF(yrmo);
        }

        public void SetReconciled(string yrmo)
        {
            string cmdstr = "INSERT INTO CAClaimsRecon(yrmo, reconciled) VALUES('" + yrmo + "', 1)";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        public DataSet GetAnthMismatch(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     anbt_checkNum AS [Check#], "
                                        + "anbt_subid_ssn AS [SSN], "
                                        + "anbt_claimid AS [Claim#], "
                                        + "anbt_name AS [Name], "
                                        + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                        + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                        + "anbt_claimsType AS [Claim Type], "
                                        + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                        + "CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) AS [Anthem Amount]  "
                            + "FROM     AnthBillTrans "
                            + "WHERE    anbt_yrmo = '" + yrmo + "' "
                                        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 AND anbt_CFCleared = 0 "
                                        + "AND anbt_checkNum "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT boaChqNo "
                                                + "FROM BOAStatement "
                                                + "WHERE boaYRMO = '" + yrmo + "' "
                                                + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                        + ") "
                            + "ORDER BY anbt_checkNum";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetBOAMismatch(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT         boaChqNo AS [Check Number], "
                                        + "boaBankRef AS [Bank Reference], "
                                        + "CONVERT(Varchar(15), boaPosted_dt,101) AS [BOA Posted Dt], "
                                        + "CONVERT(Varchar(20), boaAmnt,1) AS [BOA Amount] "                                       
                         + "FROM           BOAStatement "
                         + "WHERE          boaYRMO = '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                        + "AND boaChqNo "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT anbt_checkNum "
                                                + "FROM AnthBillTrans "
                                                + "WHERE anbt_yrmo = '" + yrmo + "' "
                                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 AND anbt_CFCleared = 0 "
                                        + ") "
                         + "ORDER BY      boaChqNo";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetMatchedChecknAmtRecords(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();
            string cmdstr1 = "SELECT   anbt_subid_ssn AS [SSN], "
                                    + "anbt_claimid AS [Claim#], "
                                    + "anbt_checkNum AS [Check#], "
                                    + "anbt_name AS [Name], "
                                    + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                    + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                    + "anbt_claimsType AS [Claim Type], "
                                    + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                    + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                    + "b.boaBankRef AS [Bank Reference], "
                                    + "CONVERT(Varchar(15), b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                    + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                    + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount] "
                                    + "FROM	AnthBillTrans "
                                    + "INNER JOIN "
                                    + "( "
                                            + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                            + "FROM BOAStatement "
                                            + "WHERE boaYRMO = '" + yrmo + "' "
                                            + "AND boaDup = 0 AND boaDupPrev = 0 "
                                    + ") b "
                           + "ON	   (anbt_checkNum = b.boaChqNo) "
                           + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                     + "AND anbt_yrmo = '" + yrmo + "' "
                                     + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + "AND (anbt_ClaimsPdAmt - b.boaAmnt) = 0 "
                                     + "ORDER BY [Diff Amount]";

            string cmdstr2 = "SELECT   a.anbt_subid_ssn AS [SSN], "
                                     + "a.anbt_claimid AS [Claim#], "
                                     + "a.anbt_checkNum AS [Check#], "
                                     + "a.anbt_name AS [Name], "
                                     + "CONVERT(Varchar(15),a.anbt_servfromdt, 101) AS [Service From Dt], "
                                     + "CONVERT(Varchar(15),a.anbt_servthrudt, 101) AS [Service Thru Dt], "
                                     + "a.anbt_claimsType AS [Claim Type], "
                                     + "CONVERT(Varchar(15),a.anbt_datePd, 101) AS [Anthem Paid Dt], "
                                     + "CONVERT(Varchar(20), a.anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                     + "boaBankRef AS [Bank Reference], "
                                     + "CONVERT(Varchar(15), boaPosted_dt, 101) AS [BOA Posted Dt], "
                                     + "CONVERT(Varchar(20), boaAmnt,1) AS [BOA Amount], "
                                     + "CONVERT(Varchar(20), (a.anbt_ClaimsPdAmt - boaAmnt),1) AS [Diff Amount] "
                                     + "FROM	BOAStatement "
                                     + "INNER JOIN "
                                     + "( "
                                             + "SELECT anbt_subid_ssn, anbt_claimid, anbt_checkNum, "
                                                + "anbt_name, anbt_servfromdt, anbt_servthrudt, anbt_claimsType, "
                                                + "anbt_datePd,  anbt_ClaimsPdAmt "
                                             + "FROM AnthBillTrans "
                                              + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                              + "AND anbt_yrmo < '" + yrmo + "' "
                                              + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + ") a "
                            + "ON	   (a.anbt_checkNum = boaChqNo) "
                                    + "WHERE boaYRMO = '" + yrmo + "' "
                                     + "AND boaDup = 0 AND boaDupPrev = 0 "
                                     + "AND (a.anbt_ClaimsPdAmt - boaAmnt) = 0 "
                                      + "ORDER BY  [Diff Amount],[Check#]";

            string cmdstr3 = "SELECT   anbt_subid_ssn AS [SSN], "
                                     + "anbt_claimid AS [Claim#], "
                                     + "anbt_checkNum AS [Check#], "
                                     + "anbt_name AS [Name], "
                                     + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                     + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                     + "anbt_claimsType AS [Claim Type], "
                                     + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                     + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                     + "b.boaBankRef AS [Bank Reference], "
                                     + "CONVERT(Varchar(15), b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                     + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                     + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount] "
                                     + "FROM	AnthBillTrans "
                                     + "INNER JOIN "
                                     + "(  "
                                             + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                             + "FROM BOAStatement "
                                             + "WHERE boaYRMO < '" + yrmo + "' "
                                             + "AND boaDup = 0 AND boaDupPrev = 0 "
                                     + ") b "
                            + "ON	   (anbt_checkNum = b.boaChqNo) "
                            + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                      + "AND anbt_yrmo = '" + yrmo + "' "
                                      + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                      + "AND (anbt_ClaimsPdAmt - b.boaAmnt) = 0 "
                                      + "ORDER BY  [Check#]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                da.SelectCommand = command;
                da.Fill(dsFinal);
                command = new SqlCommand(cmdstr2, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                dsFinal.Merge(ds);
                ds.Clear();
                command = new SqlCommand(cmdstr3, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                dsFinal.Merge(ds);
                ds.Clear();

                return dsFinal;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAmtMismatchCheckRecords(string yrmo)
        {
            SqlDataAdapter da = new SqlDataAdapter();           
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();
            string cmdstr1 = "SELECT   anbt_subid_ssn AS [SSN], "
                                    + "anbt_claimid AS [Claim#], "
                                    + "anbt_checkNum AS [Check#], "
                                    + "anbt_name AS [Name], "
                                    + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                    + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                    + "anbt_claimsType AS [Claim Type], "
                                    + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                    + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                    + "b.boaBankRef AS [Bank Reference], "
                                    + "CONVERT(Varchar(15), b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                    + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                    + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount] "
                                    + "FROM	AnthBillTrans "
                                    + "INNER JOIN "
                                    + "( "
                                            + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                            + "FROM BOAStatement "
                                            + "WHERE boaYRMO = '" + yrmo + "' "
                                            + "AND boaDup = 0 AND boaDupPrev = 0 "
                                    + ") b "
                           + "ON	   (anbt_checkNum = b.boaChqNo) "
                           + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                     + "AND anbt_yrmo = '" + yrmo + "' "
                                     + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 "
                                     + "ORDER BY [Diff Amount]";            

           

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                da.SelectCommand = command;
                da.Fill(dsFinal);                

                return dsFinal;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAmtMismatchAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT   anbt_yrmo AS [YRMO], "
                                    + "anbt_subid_ssn AS [SSN], "
                                    + "anbt_claimid AS [Claim#], "
                                    + "anbt_checkNum AS [Check#], "
                                    + "anbt_name AS [Name], "
                                    + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                    + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                    + "anbt_claimsType AS [Claim Type], "
                                    + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                    + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                    + "b.boaBankRef AS [Bank Reference], "
                                    + "CONVERT(Varchar(15),b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                    + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                    + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount], "
                                    + " CASE "
                                            + " WHEN anbt_yrmo = '" + yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Current YRMO (" + yrmo + ")] "
                                            + " ,CASE "
                                            + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Previous YRMO (" + prev_yrmo + ")] "
                                            + " ,CASE "
                                            + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Prior YRMO (" + prior_yrmo + " & less)] "           
                           + "FROM	AnthBillTrans "
                                    + "INNER JOIN "
                                    + "( "
                                            + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                            + "FROM BOAStatement "
                                            + "WHERE boaYRMO <= '" + yrmo + "' "
                                            + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                    + ")b "
                           + "ON	   (anbt_checkNum = b.boaChqNo) "
                           + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                     + "AND anbt_yrmo <= '" + yrmo + "' "
                                     + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 "
                                     + "AND anbt_CFCleared = 0 "
                                     + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAmtMismatchAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT   anbt_yrmo AS [YRMO], "
                                    + "anbt_subid_ssn AS [SSN], "
                                    + "anbt_claimid AS [Claim#], "
                                    + "anbt_checkNum AS [Check#], "
                                    + "anbt_name AS [Name], "
                                    + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                    + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                    + "anbt_claimsType AS [Claim Type], "
                                    + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                    + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                    + "b.boaBankRef AS [Bank Reference], "
                                    + "CONVERT(Varchar(15),b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                    + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                    + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount], "
                                    + " CASE "
                                            + " WHEN anbt_yrmo = '" + yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Current YRMO] "
                                            + " ,CASE "
                                            + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Previous YRMO] "
                                            + " ,CASE "
                                            + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                            + " THEN CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) "
                                            + " ELSE '0' "
                                            + " END AS [Prior YRMO] "
                           + "FROM	AnthBillTrans "
                                    + "INNER JOIN "
                                    + "( "
                                            + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                            + "FROM BOAStatement "
                                            + "WHERE boaYRMO <= '" + yrmo + "' "
                                            + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                    + ")b "
                           + "ON	   (anbt_checkNum = b.boaChqNo) "
                           + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                     + "AND anbt_yrmo <= '" + yrmo + "' "
                                     + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 "
                                     + "AND anbt_CFCleared = 0 "
                                     + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAmtMismatchAging(string yrmo,int _chqNo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT   anbt_yrmo AS [YRMO], "
                                    + "anbt_subid_ssn AS [SSN], "
                                    + "anbt_claimid AS [Claim#], "
                                    + "anbt_checkNum AS [Check#], "
                                    + "anbt_name AS [Name], "
                                    + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                    + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                    + "anbt_claimsType AS [Claim Type], "
                                    + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                    + "CONVERT(Varchar(20), anbt_ClaimsPdAmt,1) AS [Anthem Amount], "
                                    + "b.boaBankRef AS [Bank Reference], "
                                    + "CONVERT(Varchar(15),b.boaPosted_dt, 101) AS [BOA Posted Dt], "
                                    + "CONVERT(Varchar(20), b.boaAmnt,1) AS [BOA Amount], "
                                    + "CONVERT(Varchar(20), (anbt_ClaimsPdAmt - b.boaAmnt),1) AS [Diff Amount] "
                                    //+ " CASE "
                                    //        + " WHEN anbt_yrmo = '" + yrmo + "' "
                                    //        + " THEN CONVERT(Numeric(10,2), (anbt_ClaimsPdAmt - b.boaAmnt)) "
                                    //        + " ELSE 0 "
                                    //        + " END AS [Current YRMO] "
                                    //        + " ,CASE "
                                    //        + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                    //        + " THEN CONVERT(Numeric(10,2), (anbt_ClaimsPdAmt - b.boaAmnt)) "
                                    //        + " ELSE 0 "
                                    //        + " END AS [Previous YRMO] "
                                    //        + " ,CASE "
                                    //        + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                    //        + " THEN CONVERT(Numeric(10,2), (anbt_ClaimsPdAmt - b.boaAmnt)) "
                                    //        + " ELSE 0 "
                                    //        + " END AS [Prior YRMO] "
                           + "FROM	AnthBillTrans "
                                    + "INNER JOIN "
                                    + "( "
                                            + "SELECT boaChqNo, boaAmnt, boaBankRef, boaPosted_dt "
                                            + "FROM BOAStatement "
                                            + "WHERE boaYRMO = '" + yrmo + "' "
                                            + "AND boaCFCleared = 0 AND boaDup = 0 AND boaDupPrev = 0 AND boaChqNo = " + _chqNo + " "
                                    + ")b "
                           + "ON	   (anbt_checkNum = b.boaChqNo) "
                           + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                     + "AND anbt_yrmo = '" + yrmo + "' "
                                     + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                     + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 AND  anbt_checkNum = " + _chqNo +" "
                                     + "AND anbt_CFCleared = 0 "
                                     + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAnthMismatchAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     anbt_yrmo AS [YRMO], "                                    
                                        + "anbt_subid_ssn AS [SSN], "                                        
                                        + "anbt_claimid AS [Claim#], "
                                        + "anbt_checkNum AS [Check#], "
                                        + "anbt_name AS [Name], "
                                        + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                        + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                        + "anbt_claimsType AS [Claim Type], "
                                        + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                        //+ "CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) AS [Anthem Amount],  "
                                        + " CASE "
                                                + " WHEN anbt_yrmo = '" + yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Current YRMO (" + yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Previous YRMO (" + prev_yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Prior YRMO (" + prior_yrmo + " & less)] "  
                            + "FROM     AnthBillTrans "
                            + "WHERE    anbt_yrmo <= '" + yrmo + "' "
                                        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_CFCleared = 0 "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                        + "AND anbt_checkNum "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT boaChqNo "
                                                + "FROM BOAStatement "
                                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                                + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                        + ") "
                            + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAnthMismatchAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     anbt_yrmo AS [YRMO], "
                                        + "anbt_subid_ssn AS [SSN], "
                                        + "anbt_claimid AS [Claim#], "
                                        + "anbt_checkNum AS [Check#], "
                                        + "anbt_name AS [Name], "
                                        + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                        + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                        + "anbt_claimsType AS [Claim Type], "
                                        + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                //+ "CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) AS [Anthem Amount],  "
                                        + " CASE "
                                                + " WHEN anbt_yrmo = '" + yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Current YRMO] "
                                        + " ,CASE "
                                                + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Previous YRMO] "
                                        + " ,CASE "
                                                + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Prior YRMO] "
                            + "FROM     AnthBillTrans "
                            + "WHERE    anbt_yrmo <= '" + yrmo + "' "
                                        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_CFCleared = 0 "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                        + "AND anbt_checkNum "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT boaChqNo "
                                                + "FROM BOAStatement "
                                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                                + "AND boaDup = 0 AND boaDupPrev = 0 AND boaCFCleared = 0 "
                                        + ") "
                            + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetAnthMismatchAging(string yrmo,int _chqNo)
        {
            //string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            //string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     anbt_yrmo AS [YRMO], "
                                        + "anbt_subid_ssn AS [SSN], "
                                        + "anbt_claimid AS [Claim#], "
                                        + "anbt_checkNum AS [Check#], "
                                        + "anbt_name AS [Name], "
                                        + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                        + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                        + "anbt_claimsType AS [Claim Type], "
                                        + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Anthem Paid Dt], "
                                        + "CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) AS [Anthem Amount]  "
                                        //+ " CASE "
                                        //        + " WHEN anbt_yrmo = '" + yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Current YRMO] "
                                        //+ " ,CASE "
                                        //        + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Previous YRMO] "
                                        //+ " ,CASE "
                                        //        + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Prior YRMO] "
                            + "FROM     AnthBillTrans "
                            + "WHERE    anbt_yrmo = '" + yrmo + "' "
                                        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                        + "AND anbt_checkNum = " + _chqNo + " "
                                        + "AND anbt_CFCleared = 0 "
                                        //+ "NOT IN "
                                        //+ "( "
                                        //        + "SELECT boaChqNo "
                                        //        + "FROM BOAStatement "
                                        //        + "WHERE boaYRMO = '" + yrmo + "' "
                                        //        + "AND boaDup = 0 "
                                        //+ ") "
                            + "ORDER BY [YRMO] DESC, anbt_checkNum ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetBOAMismatchAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     boaYRMO AS [YRMO], "
                                        + "boaChqNo AS [Check#], "
                                        + "boaBankRef AS [Bank Reference], "                                        
                                        + "CONVERT(Varchar(15), boaPosted_dt,101) AS [BOA Posted Dt], "
                                        //+ "CONVERT(Numeric(10,2), boaAmnt) AS [BOA Amount], "                                        
                                        + " CASE "
                                                + " WHEN boaYRMO = '" + yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Current YRMO (" + yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO = '" + prev_yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Previous YRMO (" + prev_yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO <= '" + prior_yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Prior YRMO (" + prior_yrmo + " & less)] "  
                         + "FROM           BOAStatement "
                         + "WHERE          boaYRMO <= '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0 "
                                        + "AND boaCFCleared = 0 "
                                        + "AND boaChqNo "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT anbt_checkNum "
                                                + "FROM AnthBillTrans "
                                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 AND anbt_CFCleared = 0 "
                                        + ") "
                         + "ORDER BY   [YRMO] DESC, boaChqNo ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetBOAMismatchAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     boaYRMO AS [YRMO], "
                                        + "boaChqNo AS [Check#], "
                                        + "boaBankRef AS [Bank Reference], "
                                        + "CONVERT(Varchar(15), boaPosted_dt,101) AS [BOA Posted Dt], "
                //+ "CONVERT(Numeric(10,2), boaAmnt) AS [BOA Amount], "                                        
                                        + " CASE "
                                                + " WHEN boaYRMO = '" + yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Current YRMO] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO = '" + prev_yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Previous YRMO] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO <= '" + prior_yrmo + "' "
                                                + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                                + " ELSE '0' "
                                        + " END AS [Prior YRMO] "
                         + "FROM           BOAStatement "
                         + "WHERE          boaYRMO <= '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0 "
                                        + "AND boaCFCleared = 0 "
                                        + "AND boaChqNo "
                                        + "NOT IN "
                                        + "( "
                                                + "SELECT anbt_checkNum "
                                                + "FROM AnthBillTrans "
                                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 AND anbt_CFCleared = 0 "
                                        + ") "
                         + "ORDER BY   [YRMO] DESC, boaChqNo ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetBOAMismatchAging(string yrmo,int _chqNo)
        {
            //string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            //string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     boaYRMO AS [YRMO], "
                                        + "boaChqNo AS [Check#], "
                                        + "boaBankRef AS [Bank Reference], "
                                        + "CONVERT(Varchar(15), boaPosted_dt,101) AS [BOA Posted Dt], "
                                        + "CONVERT(Varchar(20), boaAmnt,1) AS [BOA Amount] "                                        
                                        //+ " CASE "
                                        //        + " WHEN boaYRMO = '" + yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Current YRMO] "
                                        //+ " ,CASE "
                                        //        + " WHEN boaYRMO = '" + prev_yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Previous YRMO] "
                                        //+ " ,CASE "
                                        //        + " WHEN boaYRMO <= '" + prior_yrmo + "' "
                                        //        + " THEN CONVERT(Numeric(10,2), boaAmnt) "
                                        //        + " ELSE 0 "
                                        //+ " END AS [Prior YRMO] "
                         + "FROM           BOAStatement "
                         + "WHERE          boaYRMO = '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0  "
                                        + "AND boaCFCleared = 0 "
                                        + "AND boaChqNo = " + _chqNo + " "
                                        //+ "NOT IN "
                                        //+ "( "
                                        //        + "SELECT anbt_checkNum "
                                        //        + "FROM AnthBillTrans "
                                        //        + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                        //        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        //        + "AND anbt_Dup = 0 "
                                        //+ ") "
                         + "ORDER BY   [YRMO] DESC, boaChqNo ASC";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetDupsAging(string yrmo)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();

            dsFinal = GetAnthDupsAging(yrmo);
            ds = GetBOADupsAging(yrmo);
            dsFinal.Merge(ds);
            ds.Clear();

            return dsFinal;
        }

        public DataSet GetDistinctDupsAging(string yrmo)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();

            dsFinal = GetDistinctDupsMatchAging(yrmo);
            ds = GetDistinctDupsUnMatchAnthAging(yrmo);
            dsFinal.Merge(ds);
            ds.Clear();
            ds = GetDistinctDupsUnMatchBOAAging(yrmo);
            dsFinal.Merge(ds);
            ds.Clear();
            return dsFinal;
        }

        public DataSet GetDistinctDupsAgingAdj(string yrmo)
        {
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();

            dsFinal = GetDistinctDupsMatchAgingAdj(yrmo);
            ds = GetDistinctDupsUnMatchAnthAgingAdj(yrmo);
            dsFinal.Merge(ds);
            ds.Clear();
            ds = GetDistinctDupsUnMatchBOAAgingAdj(yrmo);
            dsFinal.Merge(ds);
            ds.Clear();
            return dsFinal;
        }

        public DataSet GetDupChecksDetails(string yrmo)
        {
            DataSet ds1 = new DataSet();
            ds1.Clear();
            DataSet dsFinal1 = new DataSet();
            dsFinal1.Clear();
            List<int> checknos = new List<int>();
            SqlDataReader dr = null;

            string cmdstr1 = "SELECT DISTINCT anbt_checkNum FROM AnthBillTrans "
                                    + "WHERE (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                    + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                    + "AND anbt_yrmo = '" + yrmo + "' " 
                                    + "AND anbt_CheckNum "
                                    + "NOT IN " 
                                    + "( "
                                    + "SELECT DISTINCT boaChqNo  "
                                    + "FROM BOAStatement " 
                                    + "WHERE (boaDup = 1 OR boaDupPrev = 1) "
                                    + "AND boaYRMO = '" + yrmo + "' "
                                    + ") ";
            string cmdstr2 = "SELECT DISTINCT boaChqNo " 
                                    + "FROM BoaStatement " 
                                    + "WHERE (boaDup = 1 OR boaDupPrev = 1) "
                                    + "AND boaYRMO = '" + yrmo + "' AND boaChqNo " 
                                    + "NOT IN " 
                                    + "( "
                                    + "SELECT DISTINCT anbt_checkNum "
                                    + "FROM AnthBillTrans "
                                    + "WHERE anbt_sourcecd = 'CA_CLMRPT' " 
                                    + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                    + "AND anbt_yrmo = '" + yrmo + "' " 
                                    + ")";
            string cmdstr3 = "SELECT DISTINCT anbt_checkNum "
                                    + "FROM AnthBillTrans,BOAStatement "
                                    + "WHERE (boaChqNo = anbt_CheckNum) "
                                    + "AND (boaDup = 1 OR boaDupPrev = 1) "
                                    + "AND boaYRMO = '" + yrmo + "' "
                                    + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                    + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                    + "AND anbt_yrmo = '" + yrmo + "' ";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr1, connect);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    checknos.Add(dr.GetInt32(0));
                }
                dr.Close();

                command = new SqlCommand(cmdstr2, connect);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    checknos.Add(dr.GetInt32(0));
                }
                dr.Close();

                command = new SqlCommand(cmdstr3, connect);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    checknos.Add(dr.GetInt32(0));
                }
                dr.Close();

                foreach (int checkno in checknos)
                {
                    ds1 = GetDupCheckDetails(yrmo, checkno);
                    dsFinal1.Merge(ds1);
                    ds1.Clear();
                }

                return dsFinal1;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public DataSet GetDupCheckDetails(string yrmo, int checkno)
        {
            String TotAnthAmt, TotBOAAmt;
            DataSet ds = new DataSet();
            ds.Clear();
            DataSet dsFinal = new DataSet();
            dsFinal.Clear();

            dsFinal = GetAnthDupCheckDetails(yrmo, checkno);
            ds = GetBOADupCheckDetails(yrmo, checkno);
            dsFinal.Merge(ds);
            ds.Clear();

            if (dsFinal.Tables[0].Rows.Count > 0)
            {
                TotAnthAmt = Convert.ToDecimal(dsFinal.Tables[0].Compute("SUM([Anthem Amount])", String.Empty)).ToString("F2");
                TotBOAAmt = Convert.ToDecimal(dsFinal.Tables[0].Compute("SUM([BOA Amount])", String.Empty)).ToString("F2");
                DataRow row = dsFinal.Tables[0].NewRow();
                row["Paid/Posted Date"] = "TOTAL: ";
                row["Anthem Amount"] = TotAnthAmt;
                row["BOA Amount"] = TotBOAAmt;
                dsFinal.Tables[0].Rows.Add(row);
            }

            return dsFinal;
        }

        public Boolean Balanced(string yrmo)
        {
            Boolean balanced = false;
            string cmdstr = "SELECT  count(*) "
                                   + "FROM	AnthBillTrans "
                                   + "INNER JOIN "
                                   + "( "
                                           + "SELECT boaChqNo, boaAmnt  "
                                           + "FROM BOAStatement "
                                           + "WHERE boaYRMO = '" + yrmo + "' "
                                           + "AND boaDup = 0 AND boaDupPrev = 0 "
                                   + ") b "
                          + "ON	   (anbt_checkNum = b.boaChqNo) "
                          + "WHERE	    anbt_sourcecd = 'CA_CLMRPT' "
                                    + "AND anbt_yrmo = '" + yrmo + "' "
                                    + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                    + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 ";                                    

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                    balanced = true;
                
                return balanced;
            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        void SetAmtMismatchCurCF(string yrmo)
        {            
            string cmdstr1 = "UPDATE AnthBillTrans SET anbt_CF = 1 "
                                        + "WHERE anbt_checkNum IN ( "
                                        + "SELECT "
                                        + "anbt_checkNum AS [Check#] "
                                        + "FROM	AnthBillTrans "
                                        + "INNER JOIN "
                                        + "( "
                                        + "SELECT boaChqNo, boaAmnt "
                                        + "FROM BOAStatement "
                                        + "WHERE boaYRMO = '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0 "
                                        + ") b "
                                        + "ON	  (anbt_checkNum = b.boaChqNo) "
                                        + "WHERE  anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_yrmo = '" + yrmo + "' "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                        + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0 " 
                                        + ") "
                                        + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_yrmo = '" + yrmo + "' ";

            string cmdstr2 = "UPDATE BOAStatement SET boaCF = 1 "
                                        + "WHERE boaChqNo IN ( "
                                        + "SELECT "
                                        + "anbt_checkNum AS [Check#] "
                                        + "FROM	AnthBillTrans "
                                        + "INNER JOIN "
                                        + "( "
                                        + "SELECT boaChqNo, boaAmnt "
                                        + "FROM BOAStatement "
                                        + "WHERE boaYRMO = '" + yrmo + "' "
                                        + "AND boaDup = 0 AND boaDupPrev = 0 "
                                        + ") b "
                                        + "ON	  (anbt_checkNum = b.boaChqNo) "
                                        + "WHERE  anbt_sourcecd = 'CA_CLMRPT' "
                                        + "AND anbt_yrmo = '" + yrmo + "' "
                                        + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                        + "AND (anbt_ClaimsPdAmt - b.boaAmnt) <> 0  "
                                        + ") "
                                        + "AND boaYRMO = '" + yrmo + "' ";
            

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                } 
                command = new SqlCommand(cmdstr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdstr2, connect);
                command.ExecuteNonQuery();
                command.Dispose();           
            }
            finally
            {               
                connect.Close();
            }
        }

        void SetBOAMismatchCurCF(string yrmo)
        {
            string cmdstr = "UPDATE BOAStatement SET boaCF = 1 WHERE boaChqNo IN "
                                + "(SELECT boaChqNo AS [Check Number] "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO = '" + yrmo + "' "
                                + "AND boaDup = 0 AND boaDupPrev = 0 "
                                + "AND boaChqNo "
                                + "NOT IN "
                                + "( "
                                + "SELECT anbt_checkNum "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_yrmo = '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                + ") "
                                + ") "
                                + "AND boaYRMO = '" + yrmo + "' "
                                + "AND boaDup = 0 AND boaDupPrev = 0 ";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        void SetAnthMismatchCurCF(string yrmo)
        {
            string cmdstr = "UPDATE AnthBillTrans SET anbt_CF = 1 WHERE anbt_checkNum IN "
                                + "(SELECT anbt_checkNum AS [Check#] "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_yrmo = '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                + "AND anbt_checkNum "
                                + "NOT IN "
                                + "( "
                                + "SELECT boaChqNo "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO = '" + yrmo + "' "
                                + "AND boaDup = 0 AND boaDupPrev = 0 "
                                + ") "
                                + ") "
                                + "AND anbt_yrmo = '" + yrmo + "' "
                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                                + "AND anbt_sourcecd = 'CA_CLMRPT'";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        void UpdateBOAPrevCF(string yrmo)
        {
            string cmdstr = "UPDATE	BOAStatement "
                            + "SET		boaCFCleared = 1 "
                            + "WHERE	boaChqNo IN "
                            + "(SELECT  anbt_checkNum "
                            + "FROM	AnthBillTrans "
                            + "INNER JOIN "
                            + "( "
                            + "SELECT boaChqNo, boaAmnt "
                            + "FROM BOAStatement "
                            + "WHERE boaCF = 1 "
                            + "AND boaDup = 0 AND boaDupPrev = 0 "
                            + "AND boaYRMO <= '" + yrmo + "' "
                            + ") b "
                            + "ON		(anbt_checkNum = b.boaChqNo) "
                            + "WHERE	anbt_sourcecd = 'CA_CLMRPT' "
                            + "AND anbt_CF = 1 "
                            + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                            + "AND anbt_yrmo <= '" + yrmo + "' "
                            + "AND (anbt_ClaimsPdAmt - b.boaAmnt) = 0) "
                            + "AND	boaCF = 1 "
                            + "AND boaDup = 0 AND boaDupPrev = 0 "
                            + "AND boaYRMO <= '" + yrmo + "'";

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        void UpdateAnthPrevCF(string yrmo)
        {
            string cmdstr = "UPDATE	AnthBillTrans "
                            + "SET		anbt_CFCleared = 1 "
                            + "WHERE	anbt_checkNum IN "
                            + "(SELECT  anbt_checkNum "
                            + "FROM	AnthBillTrans "
                            + "INNER JOIN "
                            + "( "
                            + "SELECT boaChqNo, boaAmnt "
                            + "FROM BOAStatement "
                            + "WHERE boaCF = 1 "
                            + "AND boaDup = 0 AND boaDupPrev = 0 "
                            + "AND boaYRMO <= '" + yrmo + "' "
                            + ") b "
                            + "ON		(anbt_checkNum = b.boaChqNo) "
                            + "WHERE	anbt_sourcecd = 'CA_CLMRPT' "
                            + "AND anbt_CF = 1 "
                            + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                            + "AND anbt_yrmo <= '" + yrmo + "' "
                            + "AND (anbt_ClaimsPdAmt - b.boaAmnt) = 0) "
                            + "AND anbt_sourcecd = 'CA_CLMRPT'"
                            + "AND anbt_CF = 1 "
                            + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 "
                            + "AND anbt_yrmo <= '" + yrmo + "' ";


            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetAnthDupsAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT    anbt_yrmo AS [YRMO], "
                                            + "'Anthem' AS [Source], "
                                            + "anbt_subid_ssn AS [SSN], "
                                            + "anbt_claimid AS [Claim ID/Bank Ref], "
                                            + "anbt_checkNum AS [Check#], "
                                            + "anbt_name AS [Name], "
                                            + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                            + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                            + "anbt_claimsType AS [Claim Type], "
                                            + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Paid/Posted Date], "
                                            //+ "CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) AS [Amount],  "
                                            + " CASE "
                                                    + " WHEN anbt_yrmo = '" + yrmo + "' "
                                                    + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                    + " ELSE '0' "
                                            + " END AS [Current YRMO (" + yrmo + ")] "
                                            + " ,CASE "
                                                    + " WHEN anbt_yrmo = '" + prev_yrmo + "' "
                                                    + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                    + " ELSE '0' "
                                            + " END AS [Previous YRMO (" + prev_yrmo + ")] "
                                            + " ,CASE "
                                                    + " WHEN anbt_yrmo <= '" + prior_yrmo + "' "
                                                    + " THEN CONVERT(Varchar(20),anbt_ClaimsPdAmt,1) "
                                                    + " ELSE '0' "
                                            + " END AS [Prior YRMO (" + prior_yrmo + " & less)] "
                                + "FROM     AnthBillTrans "
                                + "WHERE    anbt_yrmo <= '" + yrmo + "' "
                                            + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                            + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                            + "AND anbt_CF = 1 "
                                            + "AND anbt_CFCleared = 0 "
                                + "ORDER BY anbt_checkNum";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetBOADupsAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT     boaYRMO AS [YRMO], "
                                        + "'BOA' AS [Source], "
                                        + "boaBankRef AS [Claim ID/Bank Ref], "
                                        + "boaChqNo AS [Check#], "
                                        + "CONVERT(Varchar(15), boaPosted_dt,101) AS [Paid/Posted Date], "
                                        //+ "CONVERT(Numeric(10,2), boaAmnt) AS [Amount], "
                                        + " CASE "
                                                + " WHEN boaYRMO = '" + yrmo + "' "
                                                + " THEN CONVERT(Varchar(20), boaAmnt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Current YRMO (" + yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO = '" + prev_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20), boaAmnt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Previous YRMO (" + prev_yrmo + ")] "
                                        + " ,CASE "
                                                + " WHEN boaYRMO <= '" + prior_yrmo + "' "
                                                + " THEN CONVERT(Varchar(20), boaAmnt,1) "
                                                + " ELSE '0' "
                                        + " END AS [Prior YRMO (" + prior_yrmo + " & less)] "
                         + "FROM           BOAStatement "
                         + "WHERE          boaYRMO <= '" + yrmo + "' "
                                        + "AND (boaDup = 1 OR boaDupPrev = 1) "
                                        + "AND boaCF = 1 "
                                        + "AND boaCFCleared = 0 "
                         + "ORDER BY boaChqNo";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        void ResetPrevCF(string yrmo)
        {
            string cmdstr1 = "UPDATE AnthBillTrans SET anbt_CFCleared = 0 "
                                + "WHERE anbt_checkNum IN "
                                + "( "
                                + "SELECT boaChqNo FROM BOAStatement "
                                + "WHERE boaCFCleared = 1 AND boaYRMO >= '" + yrmo + "' "
                                + "AND boaDup = 0 AND boaDupPrev = 0 "                               
                                +") "
                                + "AND anbt_yrmo < '" + yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_Dup = 0 AND anbt_DupPrev = 0";

            string cmdstr2 = "UPDATE BOAStatement SET boaCFCleared = 0 "
                                + "WHERE boaChqNo IN "
                                + "( "
                                + "SELECT anbt_checknum FROM AnthBillTrans "
                                + "WHERE anbt_CFCleared = 1 AND anbt_yrmo >= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND anbt_Dup = 0 AND anbt_DupPrev = 0 " 
                                + ") "
                                + "AND boaYRMO < '" + yrmo + "' AND boaDup = 0 AND boaDupPrev = 0";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr1, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SqlCommand(cmdstr2, connect);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        void ResetPrevDup(string yrmo)
        {
            string cmdstr1 = "UPDATE AnthBillTrans SET anbt_DupPrev = 0 "
                                + "WHERE anbt_checkNum IN "
                                + "( "
                                + "SELECT anbt_checknum FROM AnthBillTrans "
                                + "WHERE anbt_DupPrev = 1 AND anbt_yrmo >= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "                                
                                + ") "
                                + "AND anbt_yrmo < '" + yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT'";

            string cmdstr2 = "UPDATE AnthBillTrans SET anbt_DupPrev = 0, boaDupPrevYRMO = '' "
                                + "WHERE anbt_checkNum IN "
                                + "( SELECT anbt_checknum FROM AnthBillTrans "
                                + "WHERE  anbt_DupPrev = 1 AND boaDupPrevYRMO = '" + yrmo + "' AND anbt_sourcecd = 'CA_CLMRPT' ) "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo <= '" + yrmo + "'";

            string cmdstr3 = "UPDATE BOAStatement SET boaDupPrev = 0 "
                                + "WHERE boaChqNo IN "
                                + "( "
                                + "SELECT boaChqNo FROM BOAStatement "
                                + "WHERE boaDupPrev = 1 AND boaYRMO >= '" + yrmo + "' "
                                + ") "
                                + "AND boaYRMO < '" + yrmo + "'";
            string cmdstr4 = "UPDATE BOAStatement SET boaDupPrev = 0, anbtDupPrevYRMO = '' "
                                + "WHERE boaChqNo IN "
                                + "(select boaChqNo FROM BOAStatement WHERE "
                                + "boaDupPrev = 1 AND anbtDupPrevYRMO = '" + yrmo + "' "
                                + ") "
                                + "AND boaYRMO <= '" + yrmo + "'";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr1, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SqlCommand(cmdstr2, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SqlCommand(cmdstr3, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SqlCommand(cmdstr4, connect);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        void DeleteFromReconTable(string yrmo)
        {
            string cmdstr = "DELETE FROM CAClaimsRecon WHERE yrmo >= '" + yrmo + "'";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr, connect);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        void ResetCurCFandDup(string yrmo)
        {
            string cmdstr1 = "UPDATE AnthBillTrans SET anbt_CF = 0, anbt_CFCleared = 0, anbt_Dup = 0, anbt_DupPrev = 0, boaDupPrevYRMO = '' "
                                + "WHERE anbt_sourcecd = 'CA_CLMRPT' AND anbt_yrmo >= '" + yrmo + "'";
            string cmdstr2 = "UPDATE BOAStatement SET boaCF = 0, boaCFCleared = 0, boaDup = 0, boaDupPrev = 0, anbtDupPrevYRMO = '' "
                                + "WHERE boaYRMO >= '" + yrmo + "'";

            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(cmdstr1, connect);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SqlCommand(cmdstr2, connect);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        DataSet GetDistinctDupsMatchAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(anbt_yrmo) AS [YRMO] ,anbt_checkNum AS [Check#], COUNT(*) AS [Anthem Ctr], b.[BOA Ctr], "
                                + "CASE WHEN MAX(anbt_yrmo) = '" + yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Current YRMO (" + yrmo + ")]  "
                                + ",CASE WHEN MAX(anbt_yrmo) = '" + prev_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Previous YRMO (" + prev_yrmo + ")]  "
                                + ",CASE WHEN MAX(anbt_yrmo) <= '" + prior_yrmo + "'  "
                                + "THEN anbt_checkNum ELSE ''  "
                                + "END AS [Prior YRMO (" + prior_yrmo + " & less)] "
                                + "FROM AnthBillTrans "
                                + "INNER JOIN "
                                + "( "
                                + "SELECT DISTINCT boaChqNo, COUNT(*) AS [BOA Ctr] "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO <= '" + yrmo + "' AND (boaDup = 1 OR boaDupPrev = 1) AND boaCF = 1 AND boaCFCleared = 0 "
                                + "GROUP BY boaChqNo "
                                + ") b "
                                + "ON (anbt_checkNum = b.boaChqNo) "
                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' AND (anbt_Dup = 1 OR anbt_DupPrev = 1) AND anbt_CF = 1  "
                                + "AND anbt_CFCleared = 0  "
                                + "GROUP BY anbt_checkNum, b.[BOA Ctr] "
                                + "ORDER BY [YRMO], [Current YRMO (" + yrmo + ")], [Previous YRMO (" + prev_yrmo + ")], [Prior YRMO (" + prior_yrmo + " & less)] ";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetDistinctDupsMatchAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(anbt_yrmo) AS [YRMO] ,anbt_checkNum AS [Check#], COUNT(*) AS [Anthem Ctr], b.[BOA Ctr], "
                                + "CASE WHEN MAX(anbt_yrmo) = '" + yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Current YRMO]  "
                                + ",CASE WHEN MAX(anbt_yrmo) = '" + prev_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Previous YRMO]  "
                                + ",CASE WHEN MAX(anbt_yrmo) <= '" + prior_yrmo + "'  "
                                + "THEN anbt_checkNum ELSE ''  "
                                + "END AS [Prior YRMO] "
                                + "FROM AnthBillTrans "
                                + "INNER JOIN "
                                + "( "
                                + "SELECT DISTINCT boaChqNo, COUNT(*) AS [BOA Ctr] "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO <= '" + yrmo + "' AND (boaDup = 1 OR boaDupPrev = 1) AND boaCF = 1 AND boaCFCleared = 0 "
                                + "GROUP BY boaChqNo "
                                + ") b "
                                + "ON (anbt_checkNum = b.boaChqNo) "
                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' AND (anbt_Dup = 1 OR anbt_DupPrev = 1) AND anbt_CF = 1  "
                                + "AND anbt_CFCleared = 0  "
                                + "GROUP BY anbt_checkNum, b.[BOA Ctr] "
                                + "ORDER BY [YRMO], [Current YRMO], [Previous YRMO], [Prior YRMO] ";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetDistinctDupsUnMatchAnthAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(anbt_yrmo) AS [YRMO] ,anbt_checkNum AS [Check#], COUNT(*) AS [Anthem Ctr], 0 AS [BOA Ctr], "
                                + "CASE WHEN MAX(anbt_yrmo) = '" + yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Current YRMO (" + yrmo + ")] "
                                + ",CASE WHEN MAX(anbt_yrmo) = '" + prev_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Previous YRMO (" + prev_yrmo + ")] "
                                + ",CASE WHEN MAX(anbt_yrmo) <= '" + prior_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Prior YRMO (" + prior_yrmo + " & less)] "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                + "AND anbt_CF = 1 AND anbt_CFCleared = 0 "
                                + "AND anbt_checkNum "
                                + "NOT IN "
                                + "( "
                                + "SELECT DISTINCT boaChqNo "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                + "AND (boaDup = 1 OR boaDupPrev = 1) AND boaCF = 1 "
                                + "AND boaCFCleared = 0 "
                                + "GROUP BY boaChqNo "
                                + ") "
                                + "GROUP BY anbt_checkNum "
                                + "ORDER BY [YRMO], [Current YRMO (" + yrmo + ")], [Previous YRMO (" + prev_yrmo + ")], [Prior YRMO (" + prior_yrmo + " & less)]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetDistinctDupsUnMatchAnthAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(anbt_yrmo) AS [YRMO] ,anbt_checkNum AS [Check#], COUNT(*) AS [Anthem Ctr], 0 AS [BOA Ctr], "
                                + "CASE WHEN MAX(anbt_yrmo) = '" + yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Current YRMO] "
                                + ",CASE WHEN MAX(anbt_yrmo) = '" + prev_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Previous YRMO] "
                                + ",CASE WHEN MAX(anbt_yrmo) <= '" + prior_yrmo + "' "
                                + "THEN anbt_checkNum ELSE '' "
                                + "END AS [Prior YRMO] "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                + "AND anbt_CF = 1 AND anbt_CFCleared = 0 "
                                + "AND anbt_checkNum "
                                + "NOT IN "
                                + "( "
                                + "SELECT DISTINCT boaChqNo "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                + "AND (boaDup = 1 OR boaDupPrev = 1) AND boaCF = 1 "
                                + "AND boaCFCleared = 0 "
                                + "GROUP BY boaChqNo "
                                + ") "
                                + "GROUP BY anbt_checkNum "
                                + "ORDER BY [YRMO], [Current YRMO], [Previous YRMO], [Prior YRMO]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetDistinctDupsUnMatchBOAAging(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(boaYRMO) AS [YRMO] ,boaChqNo AS [Check#], 0 AS [Anthem Ctr], COUNT(*) AS [BOA Ctr], "
                                + "CASE "
                                + "WHEN MAX(boaYRMO) = '" + yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Current YRMO (" + yrmo + ")] "
                                + ",CASE "
                                + "WHEN MAX(boaYRMO) = '" + prev_yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Previous YRMO (" + prev_yrmo + ")] "
                                + ",CASE "
                                + "WHEN MAX(boaYRMO) <= '" + prior_yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Prior YRMO (" + prior_yrmo + " & less)] "
                                + "FROM     BOAStatement "
                                + "WHERE    boaYRMO <= '" + yrmo + "' "
                                + "AND (boaDup = 1 OR boaDupPrev = 1) "
                                + "AND boaCF = 1 "
                                + "AND boaCFCleared = 0 "
                                + "AND boaChqNo "
                                + "NOT IN "
                                + "( "
                                + "SELECT DISTINCT anbt_CheckNum "
                                + "FROM AnthBillTrans "
                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                + "AND anbt_CF = 1 "
                                + "AND anbt_CFCleared = 0 "
                                + "GROUP BY anbt_CheckNum "
                                + ") "
                                + "GROUP BY boaChqNo "
                                + "ORDER BY [YRMO], [Current YRMO (" + yrmo + ")], [Previous YRMO (" + prev_yrmo + ")], [Prior YRMO (" + prior_yrmo + " & less)]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetDistinctDupsUnMatchBOAAgingAdj(string yrmo)
        {
            string prev_yrmo = AnthRecon.prevYRMO(yrmo);
            string prior_yrmo = AnthRecon.prevYRMO(prev_yrmo);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT DISTINCT MAX(boaYRMO) AS [YRMO] ,boaChqNo AS [Check#], 0 AS [Anthem Ctr], COUNT(*) AS [BOA Ctr], "
                                + "CASE "
                                + "WHEN MAX(boaYRMO) = '" + yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Current YRMO] "
                                + ",CASE "
                                + "WHEN MAX(boaYRMO) = '" + prev_yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Previous YRMO] "
                                + ",CASE "
                                + "WHEN MAX(boaYRMO) <= '" + prior_yrmo + "' "
                                + "THEN boaChqNo "
                                + "ELSE '' "
                                + "END AS [Prior YRMO] "
                                + "FROM     BOAStatement "
                                + "WHERE    boaYRMO <= '" + yrmo + "' "
                                + "AND (boaDup = 1 OR boaDupPrev = 1) "
                                + "AND boaCF = 1 "
                                + "AND boaCFCleared = 0 "
                                + "AND boaChqNo "
                                + "NOT IN "
                                + "( "
                                + "SELECT DISTINCT anbt_CheckNum "
                                + "FROM AnthBillTrans "
                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND (anbt_Dup = 1 OR anbt_DupPrev = 1) "
                                + "AND anbt_CF = 1 "
                                + "AND anbt_CFCleared = 0 "
                                + "GROUP BY anbt_CheckNum "
                                + ") "
                                + "GROUP BY boaChqNo "
                                + "ORDER BY [YRMO], [Current YRMO], [Previous YRMO], [Prior YRMO]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                command.Dispose();
                return ds;
            }
            finally
            {
                connect.Close();
            }
        }

        DataSet GetAnthDupCheckDetails(string yrmo, int checkno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT anbt_yrmo AS [YRMO], "
                                + "anbt_checkNum AS [Check#], "
                                + "'Anthem' AS [Source], "
                                + "anbt_subid_ssn AS [SSN], "
                                + "anbt_claimid AS [Claim ID/Bank Ref], "
                                + "anbt_name AS [Name], "
                                + "CONVERT(Varchar(15),anbt_servfromdt, 101) AS [Service From Dt], "
                                + "CONVERT(Varchar(15),anbt_servthrudt, 101) AS [Service Thru Dt], "
                                + "anbt_claimsType AS [Claim Type], "
                                + "CONVERT(Varchar(15),anbt_datePd, 101) AS [Paid/Posted Date], "
                                + "CONVERT(Numeric(10,2),anbt_ClaimsPdAmt) AS [Anthem Amount], "
                                + "CONVERT(Numeric(10,2),0) AS [BOA Amount], "
                                + "CASE "
                                + "WHEN anbt_CFCleared = 0 "
                                + "THEN  'FALSE' "
                                + "ELSE 'TRUE' "
                                + "END AS [CF Cleared] "
                                + "FROM AnthBillTrans "
                                + "WHERE anbt_yrmo <= '" + yrmo + "' "
                                + "AND anbt_sourcecd = 'CA_CLMRPT' "
                                + "AND anbt_checkNum = " + checkno + " "
                                + "ORDER BY [Anthem Amount]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        DataSet GetBOADupCheckDetails(string yrmo, int checkno)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ds.Clear();
            string cmdstr = "SELECT boaYRMO AS [YRMO], "
                                + "boaChqNo AS [Check#], "
                                + "'BOA' AS [Source], "
                                + "boaBankRef AS [Claim ID/Bank Ref], "
                                + "CONVERT(Varchar(15), boaPosted_dt,101) AS [Paid/Posted Date], "
                                + "CONVERT(Numeric(10,2),0) AS [Anthem Amount], "
                                + "CONVERT(Numeric(10,2),boaAmnt) AS [BOA Amount], "
                                + "CASE "
                                + "WHEN boaCFCleared = 0 "
                                + "THEN  'FALSE' "
                                + "ELSE 'TRUE' "
                                + "END AS [CF Cleared] "
                                + "FROM BOAStatement "
                                + "WHERE boaYRMO <= '" + yrmo + "' "
                                + "AND boaChqNo = " + checkno + " "
                                + "ORDER BY [BOA Amount]";

            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdstr, connect);
                da.SelectCommand = command;
                da.Fill(ds);
                return ds;

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
        }

        public string getLatestYRMOAdj()
        {
            string _yrmo = "-1";
            string _cmdstr = "SELECT max(yrmo) FROM CAClaimsRecon";
            try
            {
                if (connect == null || connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(_cmdstr, connect);
                if (command.ExecuteScalar() != DBNull.Value)
                {
                    _yrmo = command.ExecuteScalar().ToString();
                }

            }
            finally
            {
                command.Dispose();
                connect.Close();
            }
            return _yrmo;
        }

        public void retainForcedAdj(string yrmo)
        {
            string _cmdstr = "UPDATE AnthBillTrans SET anbt_CFCleared = 1 "
                                + " WHERE anbt_sourcecd = 'CA_CLMRPT' "
                                + " AND anbt_yrmo <= '" + yrmo + "' "
                                + " AND anbt_checkNum IN "
                                + " (SELECT fadj_claimid FROM Forced_Adjustments "
                                + "  WHERE fadj_yrmo = '" + yrmo + "' "
                                + "  AND (fadj_source = 'MismatchAmounts' OR fadj_source= 'AnthemMismatch' "
                                + "  OR fadj_source = 'BOAMismatch' OR fadj_source= 'DuplicateChecks'))";

            string _cmdstr0 = "UPDATE BOAStatement SET boaCFCleared = 1 "
                                + " WHERE boaYRMO <= '" + yrmo + "' "
                                + " AND boaChqNo IN "
                                + " (SELECT fadj_claimid FROM Forced_Adjustments "
                                + "  WHERE fadj_yrmo = '" + yrmo + "' "
                                + "  AND (fadj_source = 'MismatchAmounts' OR fadj_source= 'AnthemMismatch' "
                                + "  OR fadj_source = 'BOAMismatch' OR fadj_source= 'DuplicateChecks'))";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            command = new SqlCommand(_cmdstr, connect);
            command.ExecuteNonQuery();
            command = new SqlCommand(_cmdstr0, connect);
            command.ExecuteNonQuery();
            connect.Close();
        }
    }
}
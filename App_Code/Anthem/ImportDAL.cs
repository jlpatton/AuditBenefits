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
/// Summary description for ImportDAL
/// </summary>
public class ImportDAL
{
    private string connStr;
    private SqlConnection connect = null;
    private SqlCommand command = null;
	public ImportDAL()
	{
        connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        connect = new SqlConnection(connStr);
	}   

    public string getPrevYRMO(int months)
    {
        DateTime prevmondt = DateTime.Today.AddMonths(-months);
        string prevyear = prevmondt.Year.ToString();
        string prevmonth = prevmondt.Month.ToString();
        if (prevmonth.Length == 1)
            prevmonth = "0" + prevmonth;
        string prevYRMO = prevyear + prevmonth;

        return prevYRMO;
    }

    public string getPrevYRMO(string yrmo)
    {
        yrmo = yrmo.Insert(4, "/");
        DateTime prevYRMO = Convert.ToDateTime(yrmo).AddMonths(-1);
        string prev_year = prevYRMO.Year.ToString();
        string prev_month = prevYRMO.Month.ToString();
        if (prev_month.Length == 1)
            prev_month = "0" + prev_month;
        string prev_yrmo = prev_year + prev_month;

        return prev_yrmo;
    }

    public void insertAdjData(int _plhrid,string _yrmo,string _src, string _ssn, string _name, DateTime _frmdt, DateTime _todt, string _month, decimal _calrate, decimal _premAdj, string _code)
    {
        string _cmdStr = "INSERT INTO billing_adjs VALUES ( " + _plhrid + ",'" + _yrmo + "','" + _src + "','" + _ssn + "','" + _name + "','" + _frmdt + "','" + _todt + "','" + _month + "'," + _calrate + "," + _premAdj + ",'" + _code + "')";
        command = new SqlCommand(_cmdStr, connect);
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing Adjustment Data!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertDtlData(int _plhrid, string _yrmo, string _src, string _ssn, string _name,decimal _premadj)
    {
        string _cmdStr = "INSERT INTO billing_details (bill_anthcd_id, bill_yrmo, bill_source, bill_ssn, bill_name,bill_premadj) VALUES ( " + _plhrid + ",'" + _yrmo + "','" + _src + "','" + _ssn + "','" + _name + "'," + _premadj + ")";
        command = new SqlCommand(_cmdStr, connect);
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing Membership Data!"));
        }
        finally
        {
            connect.Close();
        }
    }
    public void insertHTH(int plhr_id,int plhr_anthcd_id,string yrmo, string ssn, string name, DateTime effdt, Decimal rate)
    {
        command = new SqlCommand("INSERT INTO billing_details(bill_anthcd_id, bill_plhr_id, bill_yrmo, bill_source, bill_ssn, bill_name, bill_effdt,bill_premadj) VALUES(" + plhr_anthcd_id + ", " + plhr_id + ", '" + yrmo + "', 'HTH', '" + ssn + "', '" + name + "','" + effdt + "'," + rate + ")", connect);
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing HTH Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public int insertHTHHeadcounts(string yrmo)
    {      
        if (connect == null || connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("sp_InsertHTH", connect);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@yrmo", SqlDbType.VarChar);
        command.Parameters["@yrmo"].Value = yrmo;
        int count = command.ExecuteNonQuery();
        connect.Close();
        return count;
    }

    public int insertAnthemHeadcounts(string yrmo)
    {
        if (connect == null || connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("sp_InsertAnthBill", connect);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@yrmo", SqlDbType.VarChar);
        command.Parameters["@yrmo"].Value = yrmo;
        int count = command.ExecuteNonQuery();
        connect.Close();
        return count;
    }

    public void insertMCHeadcounts(int anthcdid, int plhr_id,string yrmo, int count,decimal rate)
    {
        if (connect == null || connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO Headcount(hdct_anthcd_id,hdct_plhr_id, hdct_source, hdct_yrmo, hdct_count,hdct_rate) VALUES(" + anthcdid + ", " + plhr_id + ", 'RET_M', '" + yrmo + "', " + count + ","+ rate +")", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing Medicare Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertNMHeadcounts(int anthcdid, int plhr_id, string yrmo, int count,decimal rate)
    {
        if (connect == null || connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO Headcount(hdct_anthcd_id,hdct_plhr_id, hdct_source, hdct_yrmo, hdct_count,hdct_rate) VALUES(" + anthcdid + ", " + plhr_id + ", 'RET_NM', '" + yrmo + "', " + count + "," + rate + ")", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing Non Medicare Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertCAClaims(string yrmo, string caseID, string group, string suffix, string claimID, string subID, string membCd, string name, DateTime servFrom, DateTime servThru, decimal claimsPaid, string claimType, DateTime datePaid, string checkno)
    {
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }   
        command = new SqlCommand("INSERT INTO AnthBillTrans(anbt_yrmo, anbt_sourcecd, anbt_casenum, anbt_group, anbt_suffix, anbt_claimid, anbt_subid_ssn, anbt_memcd,anbt_name, anbt_servfromdt, anbt_servthrudt, anbt_ClaimsPdAmt, anbt_claimsType, anbt_datePd, anbt_checkNum) VALUES('" + yrmo + "', 'CA_CLMRPT', '" + caseID + "', '" + group + "', '" + suffix + "', '" + claimID + "', '" + subID + "', '" + membCd + "', '" + name + "', '" + servFrom + "', '" + servThru + "', " + claimsPaid + ", '" + claimType + "', '" + datePaid + "','" + checkno + "')", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing California Claims Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertNonCAClaims(string yrmo, string caseID, string group, string suffix,string claimID, string subID, string membCd, string name, DateTime servFrom, DateTime servThru, decimal claimsPaid, string claimType, DateTime datePaid)
    {
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO AnthBillTrans(anbt_yrmo, anbt_sourcecd, anbt_casenum, anbt_group, anbt_suffix, anbt_claimid, anbt_subid_ssn, anbt_memcd,anbt_name, anbt_servfromdt, anbt_servthrudt, anbt_ClaimsPdAmt, anbt_claimsType, anbt_datePd) VALUES('" + yrmo + "', 'NCA_CLMRPT', '" + caseID + "', '" + group + "', '" + suffix + "', '" + claimID + "', '" + subID + "', '" + membCd + "', '" + name + "', '" + servFrom + "', '" + servThru + "', " + claimsPaid + ", '" + claimType + "', '" + datePaid + "')", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing Non California Claims Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertRFnoDF(string yrmo, string SCCF, string subID, DateTime lastupdt,  decimal amt, string dcn, string plancd)
    {
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO RFDF(rfdf_yrmo, rfdf_source, rfdf_sccf, rfdf_subid, rfdf_lastupdt, rfdf_amt, rfdf_dcn, rfdf_plancd) VALUES('" + yrmo + "', 'DF', '" + SCCF + "', '" + subID + "', '" + lastupdt + "', " + amt + ", '" + dcn + "', '" + plancd + "')", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing DF No RF Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertRF(string yrmo, string SCCF, string subID, DateTime pddt, decimal amt, string dcn, string plancd)
    {
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO RFDF(rfdf_yrmo, rfdf_source, rfdf_sccf, rfdf_subid, rfdf_pddt, rfdf_amt, rfdf_dcn, rfdf_plancd) VALUES('" + yrmo + "', 'RF', '" + SCCF + "', '" + subID + "', '" + pddt + "', " + amt + ", '" + dcn + "', '" + plancd + "')", connect);
        try
        {
            command.CommandTimeout = 600;            
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing  RF Report!"));
        }
        finally
        {
            connect.Close();
        }
    }

    public void insertMCHMO(string _plancode, string _stateCode, string _tierCode, int _count,string _yrmo)
    {
        string _cmdstr = "INSERT INTO billing_HMO "
                        + "(hmo_yrmo, hmo_plancd, hmo_tiercd, hmo_statecd, hmo_source, hmo_count) "
                        + " VALUES (@yrmo,@plan,@tier,@state,@src,@cnt) ";
        SqlCommand cmd = null;
        try
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            cmd = new SqlCommand(_cmdstr, connect);
            cmd.Parameters.AddWithValue("@yrmo", _yrmo);
            cmd.Parameters.AddWithValue("@plan", _plancode);
            cmd.Parameters.AddWithValue("@tier", _tierCode);
            cmd.Parameters.AddWithValue("@state", _stateCode);
            cmd.Parameters.AddWithValue("@src", "RET_M");
            cmd.Parameters.AddWithValue("@cnt", _count);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            connect.Close();
            cmd.Dispose();
        }
    }

    public void insertNMHMO(string _plancode, string _stateCode, string _tierCode, int _count, string _yrmo)
    {
        string _cmdstr = "INSERT INTO billing_HMO "
                        + "(hmo_yrmo, hmo_plancd, hmo_tiercd, hmo_statecd, hmo_source, hmo_count) "
                        + " VALUES (@yrmo,@plan,@tier,@state,@src,@cnt) ";
        SqlCommand cmd = null;
        try
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            cmd = new SqlCommand(_cmdstr, connect);
            cmd.Parameters.AddWithValue("@yrmo", _yrmo);
            cmd.Parameters.AddWithValue("@plan", _plancode);
            cmd.Parameters.AddWithValue("@tier", _tierCode);
            cmd.Parameters.AddWithValue("@state", _stateCode);
            cmd.Parameters.AddWithValue("@src", "RET_NM");
            cmd.Parameters.AddWithValue("@cnt", _count);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            connect.Close();
            cmd.Dispose();
        }
    }

    public void insertMCNMHMO(string _plancode, string _stateCode, string _tierCode, int _count, string _yrmo,string _source)
    {
        string _cmdstr = "INSERT INTO billing_HMO "
                        + "(hmo_yrmo, hmo_plancd, hmo_tiercd, hmo_statecd, hmo_source, hmo_count) "
                        + " VALUES (@yrmo,@plan,@tier,@state,@src,@cnt) ";
        SqlCommand cmd = null;
        try
        {
            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            cmd = new SqlCommand(_cmdstr, connect);
            cmd.Parameters.AddWithValue("@yrmo", _yrmo);
            cmd.Parameters.AddWithValue("@plan", _plancode);
            cmd.Parameters.AddWithValue("@tier", _tierCode);
            cmd.Parameters.AddWithValue("@state", _stateCode);
            cmd.Parameters.AddWithValue("@src", _source);
            cmd.Parameters.AddWithValue("@cnt", _count);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            connect.Close();
            cmd.Dispose();
        }
    }
    
    public void insertRX(string yrmo,Decimal totalPay)
    {
        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        command = new SqlCommand("INSERT INTO anth_Rx(rx_yrmo, rx_totalpay) VALUES('" + yrmo + "', " + totalPay + ")", connect);
        try
        {
            command.ExecuteNonQuery();
        }
        catch
        {
            throw (new Exception("Error in Importing RX Claims Report!"));
        }
        finally
        {
            connect.Close();
        }
    }
    public void Rollback(string source, string yrmo)
    {
        string cmdStr = "";
        string cmdStr1 = "";
        string cmdStr2 = "";

        switch (source)
        {
            case "HTH":
                cmdStr1 = "DELETE FROM billing_details WHERE bill_source = 'HTH' AND bill_yrmo = '" + yrmo + "'";
                cmdStr = "DELETE FROM Headcount WHERE hdct_source = 'HTH' AND hdct_yrmo='" + yrmo + "'";
                break;
            case "ANTH":
                cmdStr1 = "DELETE FROM billing_details WHERE bill_source LIKE 'ANTH_%' AND bill_yrmo = '" + yrmo + "'";
                cmdStr2 = "DELETE FROM billing_adjs WHERE bill_adj_source LIKE 'ANTH_%' AND bill_adj_yrmo = '" + yrmo + "'";
                cmdStr = "DELETE FROM Headcount WHERE hdct_source LIKE 'ANTH_%' AND hdct_yrmo='" + yrmo + "'";
                break;
            case "RET_M":
                cmdStr = "DELETE FROM Headcount WHERE hdct_source = 'RET_M' AND hdct_yrmo='" + yrmo + "'";
                cmdStr1 = "DELETE FROM billing_HMO WHERE hmo_source = 'RET_M' and hmo_yrmo = '" + yrmo + "'";
                break;
            case "RET_NM":
                cmdStr = "DELETE FROM Headcount WHERE hdct_source = 'RET_NM' AND hdct_yrmo='" + yrmo + "'";
                cmdStr1 = "DELETE FROM billing_HMO WHERE hmo_source = 'RET_NM' and hmo_yrmo = '" + yrmo + "'";
                break;
            case "ADP":
                cmdStr = "DELETE FROM Headcount WHERE hdct_source = 'ADP' AND hdct_yrmo ='" + yrmo + "'";
                cmdStr1 = "DELETE FROM ADP_Details WHERE adp_covg_period = '" + yrmo + "'";
                break;
            case "GRS":
                cmdStr = "DELETE FROM Headcount WHERE hdct_source = 'GRS' AND hdct_yrmo ='" + yrmo + "'";
                cmdStr1 = "DELETE FROM billing_HMO WHERE hmo_source = 'GRS' and hmo_yrmo = '" + yrmo + "'";
                break;
            case "CLMRPT":
                cmdStr = "DELETE FROM AnthBillTrans WHERE (anbt_sourcecd = 'CA_CLMRPT' OR anbt_sourcecd = 'NCA_CLMRPT' OR anbt_sourcecd = 'ANRX') AND anbt_yrmo='" + yrmo + "'";
                break;
            case "RFDF":
                cmdStr = "DELETE FROM RFDF WHERE rfdf_source = 'RF' AND rfdf_yrmo='" + yrmo + "'";
                break;
            case "DF":
                cmdStr = "DELETE FROM RFDF WHERE rfdf_source = 'DF' AND rfdf_yrmo='" + yrmo + "'";
                break;
            case "BOA":
                cmdStr = "DELETE FROM BOAStatement WHERE boaYRMO = '" + yrmo + "'";
                break;
            case "ANTH_RX":
                cmdStr = "DELETE FROM anth_Rx WHERE rx_yrmo = '" + yrmo + "'";
                break;
        }

        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        if (cmdStr1 != "")
        {
            command = new SqlCommand(cmdStr1, connect);
            command.ExecuteNonQuery();
        }
        if (cmdStr2 != "")
        {
            command = new SqlCommand(cmdStr2, connect);
            command.ExecuteNonQuery();
        }
        command = new SqlCommand(cmdStr, connect);
        command.ExecuteNonQuery();
        connect.Close();
    }

    public int getImportCount(string source, string yrmo)
    {
        string cmdStr = "";
        int count = 0;

        switch (source)
        {            
            case "ADP":
                cmdStr = "SELECT COUNT(*) FROM ADP_Details WHERE adp_covg_period ='" + yrmo + "'";
                break;
            case "GRS":
                cmdStr = "SELECT COUNT(*) FROM stageGRS WHERE grsYRMO ='" + yrmo + "'";
                break;           
            case "BOA":
                cmdStr = "SELECT COUNT(*) FROM BOAStatement WHERE boaYRMO = '" + yrmo + "'";
                break;  
            case "ANTH":
                cmdStr = "SELECT COUNT(*) FROM billing_details WHERE bill_yrmo = '" + yrmo + "' AND bill_source LIKE 'ANTH_%'";
                break;
            case "HTH":
                cmdStr = "SELECT COUNT(*) FROM billing_details WHERE bill_yrmo = '" + yrmo + "' AND bill_source LIKE 'HTH'";
                break; 

        }        

        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }

        command = new SqlCommand(cmdStr, connect);
        count = Convert.ToInt32(command.ExecuteScalar());
        connect.Close();

        return count;
    }
}

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

public class Imputed_DAL
{
    static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
    static SqlConnection connect = new SqlConnection(connStr);
    static SqlCommand command = null;

	public Imputed_DAL()
	{
	}
    [System.Web.Services.WebMethod]
    public static void CheckActivePilot(int _empno)
    {
        Page page = HttpContext.Current.Handler as Page;
        string cmdStr = "SELECT COUNT(*) FROM Employee WHERE "
                            + "((LTRIM(RTRIM(UPPER(empl_jobcd)))) = 'P' OR (LTRIM(RTRIM(UPPER(empl_jobcd)))) = 'Q') "
                            + "AND (empl_catcd2 = 3 OR empl_catcd2 = 0) "
            // + "AND (empl_catcd2 = 5 OR empl_catcd2 = 7 OR empl_catcd2 = 8) " 
                            + "AND empl_empno = @empno";
        int count = 0;

        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command = new SqlCommand(cmdStr, connect);
            command.Parameters.AddWithValue("@empno", _empno);
            count = Convert.ToInt32(command.ExecuteScalar());
            if (count == 0)
            {
                throw new Exception("Employee Number entered is not Active Pilot!");
            }            
        }
        finally
        {
            connect.Close();
        }
    }
    [System.Web.Services.WebMethod]
        public static DateTime GetDob(int _empno)
    {
        string cmdStr = "SELECT empl_dob FROM Employee WHERE empl_empno = @empno";

        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command = new SqlCommand(cmdStr, connect);
            command.Parameters.AddWithValue("@empno", _empno);
            object _result = command.ExecuteScalar();
            if (_result == null || _result == DBNull.Value)
            {
                throw new Exception("Date of Birth is not available for the entered Employee Number!");
            }

            return Convert.ToDateTime(_result);
        }
        finally
        {
            connect.Close();
        }
    }

    public static Decimal GetFactor(int _age, string _sourcecd)
    {
        string cmdStr = "SELECT ([dbo].[Imputed_getFactor](@age, @sourcecd, @effdt))";

        if (connect != null && connect.State == ConnectionState.Closed)
        {
            connect.Open();
        }
        try
        {
            command = new SqlCommand(cmdStr, connect);
            command.Parameters.AddWithValue("@age", _age);
            command.Parameters.AddWithValue("@sourcecd", _sourcecd);
            command.Parameters.AddWithValue("@effdt", DateTime.Today);
            object _result = command.ExecuteScalar();
            
            if (_result == null || _result == DBNull.Value) 
            {
                if (_sourcecd.Equals("RedFactor")) throw new Exception("Cannot find Age Reduction Factor for the given age. Please check Maintainence section!");
                if (_sourcecd.Equals("RateFactor")) throw new Exception("Cannot find Rate Factor for the given age. Please check Maintainence section!");                
            }

            return Convert.ToDecimal(_result);            
        }
        finally
        {
            connect.Close();
        }
    }
}

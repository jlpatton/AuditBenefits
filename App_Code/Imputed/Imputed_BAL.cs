using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;



public class Imputed_BAL
{
	public Imputed_BAL()
	{
		
	}

    public static NameValueCollection CalculateIncome(int _age)
    {
        NameValueCollection option_incomes = new NameValueCollection();
        decimal _redFactor, _rateFactor, option_300, option_400, option_500, option_800;

        _redFactor = Imputed_DAL.GetFactor(_age, "RedFactor"); 
        _rateFactor = Imputed_DAL.GetFactor(_age, "RateFactor");
               
        option_300 = ((300 * _redFactor) - 50) * _rateFactor; option_incomes.Add("option_300", option_300.ToString("C"));
        option_400 = ((400 * _redFactor) - 50) * _rateFactor; option_incomes.Add("option_400", option_400.ToString("C"));
        option_500 = ((500 * _redFactor) - 50) * _rateFactor; option_incomes.Add("option_500", option_500.ToString("C"));
        option_800 = ((800 * _redFactor) - 50) * _rateFactor; option_incomes.Add("option_800", option_800.ToString("C"));

        return option_incomes;    
    }

    public static int GetImputedAge(DateTime dobdt)
    {
        //DateTime today = GetLastDayofYear(DateTime.Today.Year);
        //int months, years;
        //int age;

        //// compute difference in total months
        //months = 12 * (today.Year - dobdt.Year) + (today.Month - dobdt.Month);

        //// based upon the 'days', adjust months 
        //if (today.Day < dobdt.Day)
        //{
        //    months--;
        //}

        //// compute years (age)
        //years = months / 12;
        //age = years;
        
        //return age;

        DateTime EOY = GetLastDayofYear(DateTime.Today.Year);
        return ((EOY.Year - dobdt.Year) - (dobdt.DayOfYear < EOY.DayOfYear ? 0 : 1));
    }

    static int GetAge(DateTime dateOfBirth)
    {
        DateTime EOY = GetLastDayofYear(DateTime.Today.Year);
        return ((EOY.Year - dateOfBirth.Year) - (dateOfBirth.DayOfYear < EOY.DayOfYear ? 0 : 1));
    }

    static DateTime GetLastDayofYear(int _year)
    {
        DateTime lastDay;
        int _days;
 
        _days = DateTime.DaysInMonth(_year, 12);
        lastDay = new DateTime(_year, 12, _days);

        return lastDay;
    }
}

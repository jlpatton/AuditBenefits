using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for DataBaseColumns
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class DataBaseColumns
    {
        public DataBaseColumns()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string getAnthemPlanCodecolumnName(string column)
        {
            switch (column)
            {
                case "PlhrID"   : return "plhr_id"; break;
                case "PlanYear" : return "plhr_py"; break;
                case "Desc" : return "plhr_plandesc"; break;
                case "AnthemPlanCode" : return "plhr_py"; break;
                case "AnthemTierCode" : return "plhr_py"; break;
                case "PlanCode" : return "plhr_py"; break;
                case "EDate" : return "plhr_py"; break;
                case "Rate": return "plhr_py"; break;
            }
            return "x";
        }
    }
}
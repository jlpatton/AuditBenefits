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
/// Summary description for UserRecord
/// </summary>
/// 
namespace EBA.Desktop
{
    public class UserRecord
    {
        private string fName = null;
        private string lName = null;
        private string email = null;
        private string depNum = null;
        private string depName = null;
        private string managerID = null;
        public string FirstName
        {
            get
            {
                return fName;
            }
            set
            {
                fName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lName;
            }
            set
            {
                lName = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string OrganizationCode
        {
            get
            {
                return depNum;
            }
            set
            {
                depNum = value;
            }
        }
        public string DepartmentName
        {
            get
            {
                return depName;
            }
            set
            {
                depName = value;
            }
        }
        public string ManagerId
        {
            get
            {
                return managerID;
            }
            set
            {
                managerID = value;
            }
        }
    }
}

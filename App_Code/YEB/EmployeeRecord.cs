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
/// Summary description for EmployeeRecord
/// </summary>
namespace EBA.Desktop.YEB
{
    public class EmployeeRecord
    {
        private int _empNo;
        private string _ssn;
        private string _fname;
        private string _mi;
        private string _lname;
        public string FirstName
        {
            get
            {
                return _fname;
            }
            set
            {
                _fname = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lname;
            }
            set
            {
                _lname = value;
            }
        }

        public string MiddleInitial
        {
            get
            {
                return _mi;
            }
            set
            {
                _mi = value;
            }
        }

        public int EmpNum
        {
            get
            {
                return _empNo;
            }
            set
            {
                _empNo = value;
            }
        }

        public string SSN
        {
            get
            {
                return _ssn;
            }
            set
            {
                _ssn = value;
            }
        }

       


        
    }
}
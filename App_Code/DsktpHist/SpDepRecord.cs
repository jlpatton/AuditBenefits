using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlTypes;

/// <summary>
/// Summary description for SpDepRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class SpDepRecord
    {
        private int _empNo;
        private string _ssn;
        private string _fname;
        private string _mi;
        private string _lname;
        private string _addr1;
        private string _addr2;
        private string _city;
        private string _state;
        private string _zip;
        private DateTime _dob;
        private string _sexCd;
        private string _relCd;
        private string _phone;
        private string _orgCd;
        private string _status;
        
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

        public string Address1
        {
            get
            {
                return _addr1;
            }
            set
            {
                _addr1 = value;
            }
        }

        public string Address2
        {
            get
            {
                return _addr2;
            }
            set
            {
                _addr2 = value;
            }
        }

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }

        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                _zip = value;
            }
        }

        public DateTime DOB
        {
            get
            {
                return _dob;
            }
            set
            {
                _dob = value;
            }
        }

        public string SexCode
        {
            get
            {
                return _sexCd;
            }
            set
            {
                _sexCd = value;
            }
        }

        public string RelCode
        {
            get
            {
                return _relCd;
            }
            set
            {
                _relCd = value;
            }
        }

        public string OrgCode
        {
            get
            {
                return _orgCd;
            }
            set
            {
                _orgCd = value;
            }
        }

        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
            }
        }

        
    }
}

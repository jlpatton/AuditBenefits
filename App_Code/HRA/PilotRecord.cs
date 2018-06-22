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
/// Summary description for PilotData
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class PilotRecord
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
        private DateTime _retDate;
        private DateTime _permDate;
        private string _sexCd;
        private string _status;
        private DateTime _deathDate;
        private decimal _lumpSum;
        private decimal _HRAAmt;

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

        public DateTime DateBirth
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

        public DateTime RetDate
        {
            get
            {
                return _retDate;
            }
            set
            {
                _retDate = value;
            }
        }

        public DateTime PermDate
        {
            get
            {
                return _permDate;
            }
            set
            {
                _permDate = value;
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

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public DateTime DeathDate
        {
            get
            {
                return _deathDate;
            }
            set
            {
                _deathDate = value;
            }
        }

        public Decimal LumpSum
        {
            get
            {
                return _lumpSum;
            }
            set
            {
                _lumpSum = value;
            }
        }

        public Decimal HRAAmount
        {
            get
            {
                return _HRAAmt;
            }
            set
            {
                _HRAAmt = value;
            }
        }
    }    
}

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
/// Summary description for HipCertRecord
/// </summary>
namespace EBA.Desktop.HRA
{
    public class HipCertRecord
    {
        private string _ssn;
        private string _empno;
        private string _fname;
        private string _lname;
        private string _addr1;
        private string _addr2;
        private string _city;
        private string _state;
        private string _zip;
        private string _sexcd;
        private string _mstat;
        private string _qualcd;
        private DateTime _qualdt;
        private DateTime _dob;
        private string _printflag;
        private DateTime _printdt;
        private DateTime _loaddt;
        private string _mon18flag;


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

        public string EmpNo
        {
            get
            {
                return _empno;
            }
            set
            {
                _empno = value;
            }
        }
        
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

        public string Sex
        {
            get
            {
                return _sexcd;
            }
            set
            {
                _sexcd = value;
            }
        }

        public string Mstat
        {
            get
            {
                return _mstat;
            }
            set
            {
                _mstat = value;
            }
        }

        public string QualCd
        {
            get
            {
                return _qualcd;
            }
            set
            {
                _qualcd = value;
            }
        }

        public DateTime QualDt
        {
            get
            {
                return _qualdt;
            }
            set
            {
                _qualdt = value;
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

        public string PrintFlag
        {
            get
            {
                return _printflag;
            }
            set
            {
                _printflag = value;
            }
        }

        public DateTime PrintDt
        {
            get
            {
                return _printdt;
            }
            set
            {
                _printdt = value;
            }
        }
        
        public string Mon18Flag
        {
            get
            {
                return _mon18flag;
            }
            set
            {
                _mon18flag = value;
            }
        }

        public DateTime LoadDate
        {
            get
            {
                return _loaddt;
            }
            set
            {
                _loaddt = value;
            }
        }
    }
}

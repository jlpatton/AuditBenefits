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
/// Summary description for TowerEmpRecord
/// </summary>
namespace EBA.Desktop.HRA
{
    public class TowerEmpRecord
    {
        private string _ssn;
        private string _fname;
        private string _lname;
        private string _addr1;
        private string _addr2;
        private string _city;
        private string _state;
        private string _zip;
        private string _phone;
        private string _dfname;
        private string _dlname;
        private string _dssno;
        private DateTime _ddob;
        private string _drelCd;
        private string _healthind;
        

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

        public DateTime DDOB
        {
            get
            {
                return _ddob;
            }
            set
            {
                _ddob = value;
            }
        }

        public string DRelCode
        {
            get
            {
                return _drelCd;
            }
            set
            {
                _drelCd = value;
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

        public string DFname
        {
            get
            {
                return _dfname;
            }
            set
            {
                _dfname = value;
            }
        }

        public string DLname
        {
            get
            {
                return _dlname;
            }
            set
            {
                _dlname = value;
            }
        }

        public string DSSNO
        {
            get
            {
                return _dssno;
            }
            set
            {
                _dssno = value;
            }
        }

        public string HealthInd
        {
            get
            {
                return _healthind;
            }
            set
            {
                _healthind = value;
            }
        }
    }
}

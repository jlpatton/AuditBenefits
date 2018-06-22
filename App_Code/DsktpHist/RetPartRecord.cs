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
/// Summary description for RetPartRecord
/// </summary>
namespace EBA.Desktop.HRA
{
    public class RetPartRecord
    {
        private string _empno; 
        private string _ssn;
        private string _fname;
        private string _lname;
        private string _ptype;
        private string _estatus;
        private DateTime _dedauthdate;
        private DateTime _formrecdate;
        private string _pathcd;
        private DateTime _pathdt;
        private string _pathuserid;
        private DateTime _pathuserdt;

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

        public string PType
        {
            get
            {
                return _ptype;
            }
            set
            {
                _ptype = value;
            }
        }

        public string EStatus
        {
            get
            {
                return _estatus;
            }
            set
            {
                _estatus = value;
            }
        }

        public DateTime DedAuthDate
        {
            get
            {
                return _dedauthdate;
            }
            set
            {
                _dedauthdate = value;
            }
        }

        public DateTime FormRecDate
        {
            get
            {
                return _formrecdate;
            }
            set
            {
                _formrecdate = value;
            }
        }

        public string PathCd
        {
            get
            {
                return _pathcd;
            }
            set
            {
                _pathcd = value;
            }
        }

        public DateTime PathDt
        {
            get
            {
                return _pathdt;
            }
            set
            {
                _pathdt = value;
            }
        }

        public string PathUserID
        {
            get
            {
                return _pathuserid;
            }
            set
            {
                _pathuserid = value;
            }
        }

        public DateTime PathUserDate
        {
            get
            {
                return _pathuserdt;
            }
            set
            {
                _pathuserdt = value;
            }
        }

    }
}

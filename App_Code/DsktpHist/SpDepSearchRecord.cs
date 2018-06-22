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
/// Summary description for SpDepSearchRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class SpDepSearchRecord
    {
        private int _empNo;
        private string _ssn;
        private string _fname;
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

        public int xRefEmpNum
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
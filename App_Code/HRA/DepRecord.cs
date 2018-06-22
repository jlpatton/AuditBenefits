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
/// Summary description for DepRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class DepRecord
    {
        private int _empNo = 0;
        private string _dssn = "";
        private string _dfname = "";
        private string _dmi = "";
        private string _dlname = "";
        private int _dOrder = 0;
        private string _ddob = "";
        private string _relation = "";
        private string _dsexCd = "";
        private bool _ownership;
        private string _ostartDate = "";
        private string _oendDate = "";
        private bool _oValid;
        private string _oValidDate = "";
        private bool _oElg;
        private string _oElgNotes = "";
        private string _daddr1 = "";
        private string _daddr2 = "";
        private string _dcity = "";
        private string _dstate = "";
        private string _dzip = "";

        public int DEmpNum
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

        public string DSSN
        {
            get
            {
                return _dssn;
            }
            set
            {
                _dssn = value;
            }
        }      

        public string DFirstName
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

        public string DLastName
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

        public string DMiddleInitial
        {
            get
            {
                return _dmi;
            }
            set
            {
                _dmi = value;
            }
        }

        public int Order
        {
            get
            {
                return _dOrder;
            }
            set
            {
                _dOrder = value;
            }
        }

        public string Relation
        {
            get
            {
                return _relation;
            }
            set
            {
                _relation = value;
            }
        }

        public string DDateBirth
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

        public string DSexCode
        {
            get
            {
                return _dsexCd;
            }
            set
            {
                _dsexCd = value;
            }
        }

        public string OwnershipStartDate
        {
            get
            {
                return _ostartDate;
            }
            set
            {
                _ostartDate = value;
            }
        }

        public string OwnershipEndDate
        {
            get
            {
                return _oendDate;
            }
            set
            {
                _oendDate = value;
            }
        }

        public string OwnershipValidDate
        {
            get
            {
                return _oValidDate;
            }
            set
            {
                _oValidDate = value;
            }
        }

        public bool Owner
        {
            get
            {
                return _ownership;
            }
            set
            {
                _ownership = value;
            }
        }
       
        public bool OwnershipValidated
        {
            get
            {
                return _oValid;
            }
            set
            {
                _oValid = value;
            }
        }

        public bool EligibilityStatus
        {
            get
            {
                return _oElg;
            }
            set
            {
                _oElg = value;
            }
        }

        public string ElegibilityNotes
        {
            get
            {
                return _oElgNotes;
            }
            set
            {
                _oElgNotes = value;
            }
        }

        public string DAddress1
        {
            get
            {
                return _daddr1;
            }
            set
            {
                _daddr1 = value;
            }
        }

        public string DAddress2
        {
            get
            {
                return _daddr2;
            }
            set
            {
                _daddr2 = value;
            }
        }

        public string DCity
        {
            get
            {
                return _dcity;
            }
            set
            {
                _dcity = value;
            }
        }

        public string DState
        {
            get
            {
                return _dstate;
            }
            set
            {
                _dstate = value;
            }
        }

        public string DZip
        {
            get
            {
                return _dzip;
            }
            set
            {
                _dzip = value;
            }
        }
    }
}

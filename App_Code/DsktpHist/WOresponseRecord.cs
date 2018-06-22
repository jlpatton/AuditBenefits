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
/// Summary description for WOresponseRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class WOresponseRecord
    {
        private int _seqno;
        private string _itdprojname;
        private string _analyst;
        private string _itdrecdesc;
        private int _manmonth;
        private int _manhours;
        private string _spclskillsets;
        private string _risks;
        private DateTime _dtwoassign;
        private DateTime _dtwocomp;
        
        
        public int SeqNo
        {
            get
            {
                return _seqno;
            }
            set
            {
                _seqno = value;
            }
        }

        public string ProjName
        {
            get
            {
                return _itdprojname;
            }
            set
            {
                _itdprojname = value;
            }
        }

        public string Analyst
        {
            get
            {
                return _analyst;
            }
            set
            {
                _analyst = value;
            }
        }

        public string Results
        {
            get
            {
                return _itdrecdesc;
            }
            set
            {
                _itdrecdesc = value;
            }
        }

        public int ManMonth
        {
            get
            {
                return _manmonth;
            }
            set
            {
                _manmonth = value;
            }
        }

        public int ManHours
        {
            get
            {
                return _manhours;
            }
            set
            {
                _manhours = value;
            }
        }

        public string Considerations
        {
            get
            {
                return _spclskillsets;
            }
            set
            {
                _spclskillsets = value;
            }
        }

        public string Risks
        {
            get
            {
                return _risks;
            }
            set
            {
                _risks = value;
            }
        }

        public DateTime AssignDt
        {
            get
            {
                return _dtwoassign;
            }
            set
            {
                _dtwoassign = value;
            }
        }

        public DateTime CompleteDt
        {
            get
            {
                return _dtwocomp;
            }
            set
            {
                _dtwocomp = value;
            }
        }

        
    }
}

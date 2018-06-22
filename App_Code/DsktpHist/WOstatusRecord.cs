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
/// Summary description for WOstatusRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class WOstatusRecord
    {
        private int _seqno;
        private string _status;
        private string _descr;
        private int _woapproval;
        private DateTime _dtstatus;
        private DateTime _dtwoapprove;
        private DateTime _dtprojassigned;
        private DateTime _reqdefdt;
        private DateTime _techspecdt;
        private DateTime _dtstart;
        private DateTime _dtclose;
        private DateTime _godt;
        private string _itddeveloper;
        private string _comments;
        
        
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

        public string Descr
        {
            get
            {
                return _descr;
            }
            set
            {
                _descr = value;
            }
        }

        public int Approved
        {
            get
            {
                return _woapproval;
            }
            set
            {
                _woapproval = value;
            }
        }

        public DateTime StatusDt
        {
            get
            {
                return _dtstatus;
            }
            set
            {
                _dtstatus = value;
            }
        }

        public DateTime ReqApprovalDt
        {
            get
            {
                return _dtwoapprove;
            }
            set
            {
                _dtwoapprove = value;
            }
        }

        public DateTime ProjAssignDt
        {
            get
            {
                return _dtprojassigned;
            }
            set
            {
                _dtprojassigned = value;
            }
        }

        public DateTime ReqDefDt
        {
            get
            {
                return _reqdefdt;
            }
            set
            {
                _reqdefdt = value;
            }
        }

        public DateTime TechSpecDt
        {
            get
            {
                return _techspecdt;
            }
            set
            {
                _techspecdt = value;
            }
        }

        public DateTime StartDt
        {
            get
            {
                return _dtstart;
            }
            set
            {
                _dtstart = value;
            }
        }
        public DateTime CloseDt
        {
            get
            {
                return _dtclose;
            }
            set
            {
                _dtclose = value;
            }
        }
        public DateTime GoDt
        {
            get
            {
                return _godt;
            }
            set
            {
                _godt = value;
            }
        }

        public string Developer
        {
            get
            {
                return _itddeveloper;
            }
            set
            {
                _itddeveloper = value;
            }
        }

        public string Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                _comments = value;
            }
        }
        
    }
}

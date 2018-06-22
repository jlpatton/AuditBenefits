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
/// Summary description for WOdetailRecord
/// </summary>
/// 
namespace EBA.Desktop.HRA
{
    public class WOdetailRecord
    {
        private int _seqno;
        private string _authorid;
        private string _woname;
        private int _reqlvl;
        private string _prio;
        private string _probdesc;
        private string _soldesc;
        private string _spcldesc;
        private string _type;
        private DateTime _dtreq;
        private DateTime _dtrespdue;
        
        
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

        public string AuthorID
        {
            get
            {
                return _authorid;
            }
            set
            {
                _authorid = value;
            }
        }

        public string WOname
        {
            get
            {
                return _woname;
            }
            set
            {
                _woname = value;
            }
        }

        public int ReqLvl
        {
            get
            {
                return _reqlvl;
            }
            set
            {
                _reqlvl = value;
            }
        }

        public string Priority
        {
            get
            {
                return _prio;
            }
            set
            {
                _prio = value;
            }
        }

        public string Description
        {
            get
            {
                return _probdesc;
            }
            set
            {
                _probdesc = value;
            }
        }

        public string Justif
        {
            get
            {
                return _soldesc;
            }
            set
            {
                _soldesc = value;
            }
        }

        public string Comments
        {
            get
            {
                return _spcldesc;
            }
            set
            {
                _spcldesc = value;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public DateTime ReqDt
        {
            get
            {
                return _dtreq;
            }
            set
            {
                _dtreq = value;
            }
        }

        public DateTime RespDt
        {
            get
            {
                return _dtrespdue;
            }
            set
            {
                _dtrespdue = value;
            }
        }

        
    }
}

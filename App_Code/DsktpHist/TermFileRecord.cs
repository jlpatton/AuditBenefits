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
/// Summary description for TermFileRecord
/// </summary>
namespace EBA.Desktop.HRA
{
    public class TermFileRecord
    {
        private int _empno;
        private string _name;
        private int _storageno;
        
        public int EmpNo
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

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int StorageNo
        {
            get
            {
                return _storageno;
            }
            set
            {
                _storageno = value;
            }
        }

    }
}

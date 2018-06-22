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
/// Summary description for auditTrail
/// </summary>
/// 
namespace EBA.Desktop.Audit
{
    public class auditTrail
    {
        string _actionType = "";
        string _propertyName = "";
        string _oldValue = "";
        string _newValue = "";
        string _userName = "";
        DateTime _auditDate = DateTime.Now;

        public auditTrail(string _action, string _columnName, string _oldVal, string _newVal, string _user)
        {
            _actionType = _action;
            _propertyName = _columnName;
            _oldValue = _oldVal;
            _newValue = _newVal;
            _userName = _user;
        }

        public string ActionType
        {
            get
            {
                return _actionType;
            }
        }
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
        public DateTime AuditDate
        {
            get
            {
                return _auditDate;
            }
        }
    }

    public class AuditFactory<BOType>
    {
        public auditTrail Add(BOType bo,string sPropertyName, string sNewValue, string sAction, string sUser)
        {
            System.Reflection.PropertyInfo boPropertyInfo = typeof(BOType).GetProperty(sPropertyName);
            string sOriginalValue = (string)boPropertyInfo.GetValue(bo, null);
            if (sOriginalValue != sNewValue)
            {
                auditTrail oAudit = new auditTrail(sAction, sPropertyName, sOriginalValue, sNewValue, sUser);                
                return oAudit;
            }
            else
            {
                return null;
            }
        }
    }
}

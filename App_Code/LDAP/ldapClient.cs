using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.DirectoryServices;

/// <summary>
/// Summary description for ldapQuery
/// </summary>
/// 
namespace EBA.Desktop
{
    public class ldapClient
    {
        private string _serverPath;
        private string _searchPath;
        private string _userAttr;

        public ldapClient()
        {            
            _serverPath = ConfigurationManager.AppSettings["ldapServer"].ToString();
            _searchPath = ConfigurationManager.AppSettings["ldapSearch"].ToString();
            _userAttr = ConfigurationManager.AppSettings["ldapAttrUserId"].ToString();
        }
        
        public UserRecord SearchUser(string userId)
        {

            //user id must supplied to search
            if (userId.Length < 1)
            {
                throw (new Exception("Please provide a UserId to search."));
            }                       

            UserRecord ldapUser = new UserRecord();

            DirectoryEntry dirEntry = new DirectoryEntry(_serverPath + "/" + _searchPath);
            dirEntry.AuthenticationType = AuthenticationTypes.ServerBind;
            DirectorySearcher dirSearch = new DirectorySearcher(dirEntry);
            dirSearch.Filter = "(" + _userAttr + "=" + userId + ")"; ;

            dirSearch.PropertiesToLoad.Add("givenname");
            dirSearch.PropertiesToLoad.Add("sn");
            dirSearch.PropertiesToLoad.Add("mail");
            dirSearch.PropertiesToLoad.Add("departmentnumber");
            dirSearch.PropertiesToLoad.Add("departmentname");
            dirSearch.PropertiesToLoad.Add("initials");
            dirSearch.PropertiesToLoad.Add("manager");

            SearchResult sr = dirSearch.FindOne();

            if (null != sr)
            {
                foreach (string propKey in sr.Properties.PropertyNames)
                {
                    ResultPropertyValueCollection propItems = sr.Properties[propKey];

                    foreach (Object propItem in propItems)
                    {
                       
                        switch (propKey)
                        {
                            case "givenname":
                                ldapUser.FirstName = propItem.ToString();
                                break;
                            case "sn":
                                ldapUser.LastName = propItem.ToString();
                                break;
                            case "mail":
                                ldapUser.Email = propItem.ToString();
                                break;
                            case "departmentnumber":
                                ldapUser.OrganizationCode = propItem.ToString();
                                break;
                            case "departmentname":
                                ldapUser.DepartmentName = propItem.ToString();
                                break;                            
                            case "manager":
                                string _manager = propItem.ToString();
                                int _i = _manager.IndexOf("=");
                                int _j = _manager.IndexOf(",");
                                ++_i;
                                _j = _j - 4;
                                ldapUser.ManagerId = _manager.Substring(_i, _j);
                                break;
                            default:
                                break;
                        }
                    }
                }
               return ldapUser;
            }
            else
            {   
                //throw user not found exception
                throw(new Exception("User Not Found!"));
            }
            
        }        
    }
}

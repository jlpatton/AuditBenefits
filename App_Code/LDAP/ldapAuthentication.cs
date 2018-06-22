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
/// Summary description for ldapAuthentication
/// </summary>
/// 
namespace EBA.Desktop
{
    public class ldapAuthentication
    {
        private string _serverPath;
        private string _searchPath;
        private string _userAttr;

        public ldapAuthentication()
        {
            _serverPath = ConfigurationManager.AppSettings["ldapServer"].ToString();
            _searchPath = ConfigurationManager.AppSettings["ldapSearch"].ToString();
            _userAttr = ConfigurationManager.AppSettings["ldapAttrUserId"].ToString();
        }

        public bool AuthenticateUser(string _userID, string _passWord)
        {
            bool _isValid = false;

            if (_userID == null)
            {
                throw (new Exception("Please Enter User ID!"));
            }

            if (_passWord == null)
            {
                throw (new Exception("Enter Password!"));
            }

            //user id must supplied to search
            if (_userID.Length < 1 || _passWord.Length < 1)
            {
                throw (new Exception("UserId and/or password are not Entered!"));
            }

            DirectoryEntry dirEntry = new DirectoryEntry(_serverPath + "/" + _searchPath);
            dirEntry.AuthenticationType = AuthenticationTypes.ServerBind;
            DirectorySearcher dirSearch = new DirectorySearcher(dirEntry);

            dirSearch.Filter = "(" + _userAttr + "=" + _userID + ")";
            SearchResult sr = dirSearch.FindOne();

            try
            {
                if (null != sr)
                {
                    dirEntry.Username = _userAttr + "=" + _userID + "," + _searchPath;
                    dirEntry.Password = _passWord;
                    dirEntry.AuthenticationType = AuthenticationTypes.ServerBind;
                    dirSearch.FindOne();
                    _isValid = true;
                }
                else
                {
                    throw (new Exception("Invalid User ID"));
                }
            }
            catch (Exception ex)
            {
                _isValid = false;
                throw (new Exception("Invalid Password"));
            }

            dirSearch = null;
            sr = null;

            return _isValid;

        }
    }
}


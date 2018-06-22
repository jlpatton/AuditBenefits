using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop;
using EBA.Desktop.Audit;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["mid"] = 0;
            Login1.UserNameLabelText = "Employee ID:";
            Login1.RememberMeSet.Equals(false);
            //auditPageBLL.auditPagevisit(HttpContext.Current.Request.Url.AbsolutePath);
            lblError.Text = "";
        }
    }
    protected void l_onAuthenticate(object sender, AuthenticateEventArgs e)
    {
        Login1.FailureText = "";
        lblError.Text = "";
        bool auth;
        ldapAuthentication ldAuth = new ldapAuthentication();
        UserAccess ua = new UserAccess();
        ldapClient userObject = new ldapClient();
        UserRecord ud;
        try
        {
            bool _locked = ua.isLocked(Login1.UserName.ToString());
            if (!_locked)
            {
                try
                {
                    auth = ldAuth.AuthenticateUser(Login1.UserName.ToString(), Login1.Password.ToString());
                    e.Authenticated = auth;
                    bool _access = ua.hasAccess(Login1.UserName.ToString());
                    if (auth == true && _access)
                    {
                        ua.ValidLogin(Login1.UserName.ToString());
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, Login1.UserName.ToString(), DateTime.Now, DateTime.Now.AddMinutes(60), false, "");
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        Response.Cookies.Add(authCookie);
                        Session["uid"] = Login1.UserName.ToString();
                        Session["pwd"] = EncryptDecrypt.Encrypt(Login1.Password.ToString());
                        ud = userObject.SearchUser(Login1.UserName.ToString());
                        Session["userName"] = ud.FirstName.ToString() + " " + ud.LastName.ToString();

                        Audit.auditUserSession(Session["uid"].ToString(), Session["userName"].ToString(), Session.SessionID.ToString());

                        // Redirect the user to the originally requested page
                        string redirectURL = FormsAuthentication.GetRedirectUrl(Login1.UserName.ToString(), false);                        
                        if (redirectURL.Equals(""))
                        {
                            Response.Redirect("~/Home.aspx");
                        }                        
                        //auditEBA.auditUser(HttpContext.Current.Session.SessionID, Login1.UserName.ToString());                
                    }
                    else
                    {
                        e.Authenticated = false;
                        throw (new Exception("You are not approved to access the application!"));
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        ua.InvalidLogin(Login1.UserName.ToString());
                    }
                    catch (Exception ex1)
                    {
                        lblError.Text = ex1.Message;
                    }
                    e.Authenticated = false;
                    throw ex;
                }
            }
            else
            {
                e.Authenticated = false;
                lblError.Text = "Account Locked. Contact your administrator to unlock your account.";
            }
        }
        catch(Exception ex)
        {
            Login1.FailureText = ex.Message;
        }
    }
    
}
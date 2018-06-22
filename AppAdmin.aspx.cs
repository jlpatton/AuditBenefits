using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Collections;
using System.Text;
using System.IO;

public partial class AppAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!checkAccess())
        {
            Response.Redirect("~/NoAccess.aspx");
        }
        if (!IsPostBack)
        {
            ReadWebConfig();
            ReadFile();
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
        }
    }
    protected void btnEncrypt_Click(object sender, EventArgs e)
    {
        EncryptAppSettings();
        ReadFile();
    }
    protected void btnDecrypt_Click(object sender, EventArgs e)
    {
        DecryptAppSettings();
        ReadFile();
    }

    private void ReadFile()
    {
        string strFileName = Server.MapPath("Web.Config");
        StreamReader strmRdr;
        string strTemp;
        strmRdr = File.OpenText(strFileName);
        strTemp = strmRdr.ReadLine();
        StringBuilder sb = new StringBuilder(String.Empty);
        while (strTemp != null)
        {
            sb.Append(strTemp + "\n");
            strTemp = strmRdr.ReadLine();
        }
        strmRdr.Close();
        
        TextBox1.Text = ConvertHTMLCharacters(sb.ToString());
        
    }
    private string ConvertHTMLCharacters(string strSourceString)
    {
        string strTargetString = null;
        if (strSourceString != "&nbsp;")
        {
            strTargetString = strSourceString.Replace("&", "&amp;");
            strTargetString = strSourceString.Replace("<", "&lt;");
            strTargetString = strSourceString.Replace(">", "&gt;");
        }
        return strTargetString;
    }
    private void EncryptAppSettings()
    {
        Configuration objConfig = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        ConfigurationSection section = objConfig.ConnectionStrings;        
        if (!section.SectionInformation.IsProtected)
        {
            section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
            section.SectionInformation.ForceSave = true;
            objConfig.Save(ConfigurationSaveMode.Modified);
        }
    }

    private void DecryptAppSettings()
    {
        Configuration objConfig = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
        ConfigurationSection section = objConfig.ConnectionStrings;        
        if (section.SectionInformation.IsProtected)
        {
            section.SectionInformation.UnprotectSection();
            section.SectionInformation.ForceSave = true;
            objConfig.Save(ConfigurationSaveMode.Modified);
        }
    }

    private void ReadWebConfig()
    {
        Configuration objConfig;
        objConfig = WebConfigurationManager.OpenWebConfiguration("~");               
        txtConn.Text = objConfig.ConnectionStrings.ConnectionStrings["EBADB"].ToString();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        btnUpdate.Visible = true;
        txtConn.ReadOnly = false;
        btnCancel.Visible = true;
        ReadFile();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {       
        Configuration objConfig;
        objConfig = WebConfigurationManager.OpenWebConfiguration("~");
        objConfig.ConnectionStrings.ConnectionStrings["EBADB"].ConnectionString = txtConn.Text.ToString();
        objConfig.Save();
        btnUpdate.Visible = false;
        txtConn.ReadOnly = true;
        btnCancel.Visible = false;
        ReadFile();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnUpdate.Visible = false;
        txtConn.ReadOnly = true;
        btnCancel.Visible = false;
        ReadFile();
    }

    protected bool checkAccess()
    {
        string _uNum = User.Identity.Name.ToString();
        bool _access = false;
        if (_uNum.Equals("428604") || _uNum.Equals("696375") || _uNum.Equals("681593"))
        {
            _access = true;
        }
        return _access;
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop.HRA;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;
using System.IO;

public partial class HRA_Operations_PGP_FTP_EligFile : System.Web.UI.Page
{
    private const string _class = "Operation";

    protected void Page_Load(object sender, EventArgs e)
    {
         if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string _filename, _filepath, _file;

            _filename = HRA_Results.GetFileName("Elig", String.Empty);
            _filepath = HRA_ReconDAL.GetFilePath(_class, "Elig");
            _file = _filepath + _filename;
            tbx_eligPath.Text = _file;
            tbx_fldrPath.Text = "C:\\EligPGP";

           string uname = HttpContext.Current.User.Identity.Name.ToString();
           AdminBLL.InRole(uname, "M200", "M203");
        }
    }

    protected void lnk_dwnPGP_OnClick(object sender, EventArgs e)
    {
        string _file = HttpContext.Current.Server.MapPath("~/GnuPG/EligPGP.zip");
        FileInfo fInfo = new FileInfo(_file);

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ContentType = "application/zip";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=EligPGP.zip");
        HttpContext.Current.Response.AddHeader("Content-Length", fInfo.Length.ToString());
        HttpContext.Current.Response.WriteFile(_file);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    protected void lnk_pgpElig_OnClick(object sender, EventArgs e)
    {
        lbl_err.Text = "";
    }

    protected void lnk_ftpElig_OnClick(object sender, EventArgs e)
    {
        HRAEligFile eobj = new HRAEligFile();
        lbl_err.Text = "";
        string strFilePath1 = "";

        try
        {
            if (FileUpload1.GotFile)
            {
                string fn = System.IO.Path.GetFileName(FileUpload1.FilePost.FileName);
                strFilePath1 = Server.MapPath("~/uploads/") + fn;

                if (File.Exists(strFilePath1))
                {
                    File.Delete(strFilePath1);
                }
                FileUpload1.FilePost.SaveAs(strFilePath1);
                eobj.FTPEligFile(strFilePath1);

                Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
                Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Operations", "FTP of Eligibility File ", DateTime.Today.ToString("yyyyMMdd"));
            }
        }
        catch (Exception ex)
        {
            lbl_err.Text = "Error: " + ex.Message;
        }
        finally
        {
            if (File.Exists(strFilePath1))
            {
                File.Delete(strFilePath1);
            }
            FileUpload1.FilePost.InputStream.Flush();
            FileUpload1.FilePost.InputStream.Close();
            FileUpload1.FilePost.InputStream.Dispose();
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }
}

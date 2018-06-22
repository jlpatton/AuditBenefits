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
using System.Collections.Specialized;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;


public partial class ImputedIncome_Calculate : System.Web.UI.Page

{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            tbx_empno.Attributes.Add("onblur", "javascript:CallMe('" + tbx_empno.ClientID + "', '" + tbx_dob.ClientID + "')");
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M500", "M501");            
        }
    }
    protected void lnk_Submit_OnClick(object sender, EventArgs e)
    {
        int _empno, _age;
        DateTime _dob;
        NameValueCollection option_incomes;
        lbl_err.Text = "";

        try
        {
            _empno = Convert.ToInt32(tbx_empno.Text);
            
            Imputed_DAL.CheckActivePilot(_empno);

            if ((tbx_dob.Text != null) && (tbx_dob.Text.Trim() != String.Empty))
            {
                _dob = Convert.ToDateTime(tbx_dob.Text);
            }
            else
            {
                _dob = Imputed_DAL.GetDob(_empno);
                tbx_dob.Text = _dob.ToString("MM/dd/yyyy");
            }

            _age = Imputed_BAL.GetImputedAge(_dob);

            tbx_age.Text = _age.ToString();

            option_incomes = Imputed_BAL.CalculateIncome(_age);

            tbx_300.Text = option_incomes.Get("option_300");
            tbx_400.Text = option_incomes.Get("option_400");
            tbx_500.Text = option_incomes.Get("option_500");
            tbx_800.Text = option_incomes.Get("option_800");
        }
        catch(Exception ex)
        {
            ClearControlsValue();
            lbl_err.Text = "Error: " + ex.Message;
        }
    }
    protected void lnk_Reset_OnClick(object sender, EventArgs e)
    {
        lbl_err.Text = "";
        ClearControlsValue();
    }

    void ClearControlsValue()
    {
        tbx_empno.Text = "";
        tbx_dob.Text = "";
        tbx_age.Text = "";
        tbx_300.Text = "";
        tbx_400.Text = "";
        tbx_500.Text = "";
        tbx_800.Text = "";
    }
    protected void tbx_empno_TextChanged(object sender, EventArgs e)
    {

    }
}

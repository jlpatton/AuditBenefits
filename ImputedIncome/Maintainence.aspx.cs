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
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;

public partial class ImputedIncome_Maintainence : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M500", "M502");
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    protected void InsertRateFactor_Click(object sender, EventArgs e)
    {
        lbl_errRate.Text = "";

        string _yrmo = ((TextBox)grdvRate.FooterRow.FindControl("tbxi_rateEffYrmo")).Text;
        string _frmAge = ((TextBox)grdvRate.FooterRow.FindControl("tbxi_rateFrmAge")).Text;
        string _toAge = ((TextBox)grdvRate.FooterRow.FindControl("tbxi_rateToAge")).Text;
        string _factor = ((TextBox)grdvRate.FooterRow.FindControl("tbxi_rateFactor")).Text;

        try
        {
            SqlDataSourceRate.InsertParameters["effdt"].DefaultValue = _yrmo;
            SqlDataSourceRate.InsertParameters["frmage"].DefaultValue = _frmAge;
            SqlDataSourceRate.InsertParameters["toage"].DefaultValue = _toAge;
            SqlDataSourceRate.InsertParameters["factor"].DefaultValue = _factor;
            //SqlDataSourceRate.InsertParameters["sourcecode"].DefaultValue = "RateFactor";
            //auditInsert(_yrmo, _type, _desc, _rate.ToString());
            SqlDataSourceRate.Insert();
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Rate", "NRC", "Insert", "ImputedIncome_Main", "effdt", "", "", "", _yrmo);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Rate", "NRC", "Insert", "ImputedIncome_Main", "frmAge", "", "", "", _frmAge);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Rate", "NRC", "Insert", "ImputedIncome_Main", "toAge", "", "", "", _toAge);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Rate", "NRC", "Insert", "ImputedIncome_Main", "factor", "", "", "", _factor);
        }
        catch (Exception ex)
        {
            lbl_errRate.Text = ex.Message;
        }
    }

    protected void InsertAgeFactor_Click(object sender, EventArgs e)
    {
        lbl_errAge.Text = "";

        string _yrmo = ((TextBox)grdv_age.FooterRow.FindControl("tbxi_ageEffYrmo")).Text;
        string _frmAge = ((TextBox)grdv_age.FooterRow.FindControl("tbxi_ageFrmAge")).Text;
        string _toAge = ((TextBox)grdv_age.FooterRow.FindControl("tbxi_ageToAge")).Text;
        string _factor = ((TextBox)grdv_age.FooterRow.FindControl("tbxi_redFactor")).Text;

        try
        {
            SqlDataSourceAge.InsertParameters["effdt"].DefaultValue = _yrmo;
            SqlDataSourceAge.InsertParameters["frmage"].DefaultValue = _frmAge;
            SqlDataSourceAge.InsertParameters["toage"].DefaultValue = _toAge;
            SqlDataSourceAge.InsertParameters["factor"].DefaultValue = _factor;            
           // SqlDataSourceAge.InsertParameters["sourcecode"].DefaultValue = "RedFactor";
            //auditInsert(_yrmo, _type, _desc, _rate.ToString());
            SqlDataSourceAge.Insert();
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Age", "NRC", "Insert", "ImputedIncome_Main", "effdt", "", "", "", _yrmo);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Age", "NRC", "Insert", "ImputedIncome_Main", "frmAge", "", "", "", _frmAge);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Age", "NRC", "Insert", "ImputedIncome_Main", "toAge", "", "", "", _toAge);
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "IMPUTED", "Imputed Maintenance Table - Age", "NRC", "Insert", "ImputedIncome_Main", "factor", "", "", "", _factor);
        }
        catch (Exception ex)
        {
            lbl_errAge.Text = ex.Message;
        }
    }    

    protected void auditUpdateAge(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = (GridViewRow)grdv_age.Rows[e.RowIndex];
        int _sid = Int32.Parse(grdv_age.DataKeys[e.RowIndex].Value.ToString());

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;

        foreach (DictionaryEntry newValues in e.NewValues)
        {
            int i = 0;

            string newKeyCol = newValues.Key.ToString();
            foreach (DictionaryEntry oldVals in e.OldValues)
            {
                string oldKeyCol = oldVals.Key.ToString();

                if (oldKeyCol == newKeyCol)
                {
                    break;
                }
                i++;
            }
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "Imputed", "Imputed Maintenance Table", "NRC", "Update", "ImputedIncome_Main", newKeyCol, _sid.ToString(), "", oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "Imputed", "Imputed Maintenance Table", "NRC", "Update", "ImputedIncome_Main", newKeyCol, _sid.ToString(), "", oldVal, "");
                }
            }
        }
    }    

    protected void auditUpdateRate(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = (GridViewRow)grdvRate.Rows[e.RowIndex];
        int _sid = Int32.Parse(grdvRate.DataKeys[e.RowIndex].Value.ToString());

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;

        foreach (DictionaryEntry newValues in e.NewValues)
        {
            int i = 0;

            string newKeyCol = newValues.Key.ToString();
            foreach (DictionaryEntry oldVals in e.OldValues)
            {
                string oldKeyCol = oldVals.Key.ToString();

                if (oldKeyCol == newKeyCol)
                {
                    break;
                }
                i++;
            }
            string oldVal = (string)e.OldValues[i];

            if (newValues.Value != null)
            {
                string newVal = newValues.Value.ToString();

                if (oldVal != newVal)
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "Imputed", "Imputed Maintenance Table", "NRC", "Update", "ImputedIncome_Main", newKeyCol, _sid.ToString(), "", oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "Imputed", "Imputed Maintenance Table", "NRC", "Update", "ImputedIncome_Main", newKeyCol, _sid.ToString(), "", oldVal, "");
                }
            }
        }
    }
}

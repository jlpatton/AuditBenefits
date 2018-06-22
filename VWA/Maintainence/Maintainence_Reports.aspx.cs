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
using System.Text.RegularExpressions;
using EBA.Desktop.Admin;

public partial class VWA_Maintainence_Maintainence_Reports : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M400", "M406");
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    protected void grdvImport_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string _pattern = @"^\\\\[\w-]+((\\[^\\/?<>|"":.]+)*\\?)$";

        Label lbl = new Label();
        lbl = (Label)(grdvImport.Rows[e.RowIndex].FindControl("lbl_importErr"));
        lbl.Visible = false;
        TextBox tbx = new TextBox();
        tbx = (TextBox)(grdvImport.Rows[e.RowIndex].FindControl("tbx_importfile"));
        Match _m = Regex.Match(tbx.Text, _pattern);
        //if (!_m.Success)
        //{
        //    lbl.Visible = true;
        //    e.Cancel = true;
        //}
    }

    protected void grdvExport_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string _pattern = @"^\\\\[\w-]+((\\[^\\/?<>|"":.]+)*\\?)$";

        Label lbl = new Label();
        lbl = (Label)(grdvExport.Rows[e.RowIndex].FindControl("lbl_exportErr"));
        lbl.Visible = false;
        TextBox tbx = new TextBox();
        tbx = (TextBox)(grdvExport.Rows[e.RowIndex].FindControl("tbx_exportfile"));
        Match _m = Regex.Match(tbx.Text, _pattern);
        if (!_m.Success)
        {
            lbl.Visible = true;
            e.Cancel = true;
        }
    }

    protected void cbx_export_OnChecked(object sender, EventArgs e)
    {
        string _pattern = @"^\\\\[\w-]+((\\[^\\/?<>|"":.]+)*\\?)$";
        CheckBox _cbx = (CheckBox)(sender);
        GridViewRow row = (GridViewRow)_cbx.NamingContainer;
        TextBox _tbx = grdvExport.Rows[row.RowIndex].FindControl("tbx_exportfile") as TextBox;
        Label lbl = grdvExport.Rows[row.RowIndex].FindControl("lbl_exportErr") as Label;
        Match _m = Regex.Match(_tbx.Text, _pattern);

        lbl.Visible = false;
        if (!_m.Success && _cbx.Checked)
        {
            _cbx.Checked = false;
            lbl.Visible = true;
        }
    }

    protected void cbx_import_OnChecked(object sender, EventArgs e)
    {
        string _pattern = @"^\\\\[\w-]+((\\[^\\/?<>|"":.]+)*\\?)$";
        CheckBox _cbx = (CheckBox)(sender);
        GridViewRow row = (GridViewRow)_cbx.NamingContainer;
        TextBox _tbx = grdvImport.Rows[row.RowIndex].FindControl("tbx_importfile") as TextBox;
        Label lbl = grdvImport.Rows[row.RowIndex].FindControl("lbl_importErr") as Label;
        Match _m = Regex.Match(_tbx.Text, _pattern);

        lbl.Visible = false;
        if (!_m.Success && _cbx.Checked)
        {
            _cbx.Checked = false;
            lbl.Visible = true;
        }
    }    
}

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
using System.Text.RegularExpressions;
using EBA.Desktop.Audit;
using EBA.Desktop.Admin;

public partial class HRA_Maintainence_Maintainence_Module : System.Web.UI.Page
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
            AdminBLL.InRole(uname, "M200", "M213");
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);       
    }

    protected void Insert_Click(object sender, EventArgs e)
    {
        lblErrRate.Text = "";

        string _yrmo = ((TextBox)grdvRates.FooterRow.FindControl("tbx_yrmo")).Text;
        string _type = ((DropDownList)grdvRates.FooterRow.FindControl("ddl_ftype")).SelectedItem.Text;
        string _desc = ((TextBox)grdvRates.FooterRow.FindControl("txt_fdesc")).Text;
        decimal _rate = Convert.ToDecimal(((TextBox)grdvRates.FooterRow.FindControl("tbx_frate")).Text);
        
        try
        {
            HRAAdminDAL.insertRates(_yrmo, _type, _desc, _rate);
            auditInsert(_yrmo, _type, _desc, _rate.ToString());
            grdvRates.DataBind();
        }
        catch (Exception ex)
        {
            lblErrRate.Text = ex.Message;
        }
    }

    protected void auditUpdateRates(object sender, GridViewUpdateEventArgs e)
    {
        string _pname = "";
        GridViewRow row = (GridViewRow)grdvRates.Rows[e.RowIndex];
        int _rid = Int32.Parse(grdvRates.DataKeys[e.RowIndex].Value.ToString());

        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;

        foreach (DictionaryEntry oldVals in e.OldValues)
        {
            string oldKeyCol = oldVals.Key.ToString();

            if (oldKeyCol.Equals("type"))
            {
                _pname = oldVals.Value.ToString();
            }
        }
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
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Update", "Rates", newKeyCol, _rid.ToString() , _pname, oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Update", "Rates", newKeyCol, _rid.ToString(), _pname, oldVal, "");
                }
            }
        }
    }

    private void auditInsert(string _yrmo, string _type, string _desc, string _rate)
    {
        Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
        string _rid = Session["hraRateId"].ToString();        
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Insert", "Rates", "yrmo", _rid,_type, "", _yrmo);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Insert", "Rates", "Type", _rid, _type, "", _type);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Insert", "Rates", "Desc", _rid, _type, "", _desc);
        Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Rates", "NRC", "Insert", "Rates", "Rate", _rid, _type, "", _rate);
    }

    protected void InsertSSN_Click(object sender, EventArgs e)
    {
        lblErrSSN.Text = "";

        string _ssn = ((TextBox)grdvSSN.FooterRow.FindControl("tbx_ssn")).Text;
        
        try
        {
            HRAAdminDAL.insertSSN(_ssn);
            //Audit
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Exclude SSN", "NRC", "Insert", "ExcludeSSN", "SSN", "", "", "", _ssn);
            grdvSSN.DataBind();
        }
        catch (Exception ex)
        {
            lblErrSSN.Text = ex.Message;
        }
    }

    protected void auditUpdateSSN(object sender, GridViewUpdateEventArgs e)
    {        
        GridViewRow row = (GridViewRow)grdvSSN.Rows[e.RowIndex];
        int _sid = Int32.Parse(grdvSSN.DataKeys[e.RowIndex].Value.ToString());

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
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Exclude SSN", "NRC", "Update", "ExcludeSSN", newKeyCol, _sid.ToString(), "", oldVal, newVal);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(oldVal))
                {
                    Audit.auditUserTask(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "HRA", "Admin Bill Validation Exclude SSN", "NRC", "Update", "ExcludeSSN", newKeyCol, _sid.ToString(), "", oldVal, "");
                }
            }
        }
    }

    protected void ddl_ftype_onSelectedindexchanged(object sender, EventArgs e)
    {
        TextBox _descTB = grdvRates.FooterRow.FindControl("txt_fdesc") as TextBox;
        _descTB.Text = "";
        string _type = ((DropDownList)grdvRates.FooterRow.FindControl("ddl_ftype")).SelectedItem.Text;
        _descTB.Text = HRAAdminDAL.RateTypeDesc(_type);
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
        if (!_m.Success)
        {
            lbl.Visible = true;
            e.Cancel = true;
        }
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

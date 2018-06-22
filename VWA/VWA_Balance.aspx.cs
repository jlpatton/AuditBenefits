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
using EBA.Desktop.VWA;
using EBA.Desktop.Admin;
using EBA.Desktop.Audit;
using System.Collections.Generic;

public partial class VWA_VWA_Balance : System.Web.UI.Page
{
    private const string _category = "Balance";
    DataTable tbSum1 = new DataTable();
    DataTable tbSum2 = new DataTable();
    DataTable tbFinal = new DataTable();
    decimal _bfdxT = 0, _bvwaT = 0, _rfdxT = 0, _rvwaT = 0, _dfdxT = 0, _dvwaT = 0;
    List <decimal> _badj = null, _radj = null, _dadj = null;
    List<string> _rnotes = null, _bnotes = null, _dnotes = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M400", "M403");            
            ddlYrmoList();
            txtPrevYRMO.Attributes.Add("onKeyUp", "EditableDropDown('" + txtPrevYRMO.ClientID + "', '" + ddlYrmo.ClientID + "', /^20\\d\\d(0[1-9]|1[012])$/)");
       }
    }

    protected void ddlYrmoList()
    {
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = VWA.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        errorDiv1.Visible = false;
        resultdiv.Visible = false;
        try
        {
            if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO.Text = "";
                lblPrevYrmo.Visible = false;
                ddlYrmo.Visible = false;
                DivNewYRMO.Visible = true;
            }            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void txtPrevYRMO_textChanged(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        Regex _r = new Regex(@"^20\d\d(0[1-9]|1[012])$");

        try
        {
            if (!_r.Match(txtPrevYRMO.Text).Success)
                throw new Exception("Enter YRMO in format 'yyyymm'");

            if (!ddlYrmo.Items.Contains(new ListItem(txtPrevYRMO.Text)))
            {
                ddlYrmo.Items.Add(txtPrevYRMO.Text);
            }
            for (int i = 0; i < ddlYrmo.Items.Count; i++)
            {
                if (ddlYrmo.Items[i].Text == txtPrevYRMO.Text)
                {
                    ddlYrmo.SelectedIndex = i;
                }
            }
            lblPrevYrmo.Visible = true;
            ddlYrmo.Visible = true;
            txtPrevYRMO.Text = "";
            DivNewYRMO.Visible = false;            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        ddlYrmo.SelectedIndex = 0;
        ddlYrmo.Visible = true;
        lblPrevYrmo.Visible = true;
        txtPrevYRMO.Text = "";
        DivNewYRMO.Visible = false;        
    }

    protected void btn_BalanceOnClick(Object sender, EventArgs e)
    {
        errorDiv1.Visible = false;
        lbl_error.Text = "";

        try
        {
            createTables();
            bankReport();
            remitReport();
            detailReport();
            getAdjustments();
            summaryReport();
            SetNoRecsMsg();
            //data build tables
            VWA_Results.SavePrintBalFiles(_category, ddlYrmo.SelectedItem.Text, tbSum1, tbSum2, tbFinal);
            VWA_ExportDAL.insertReconDt(ddlYrmo.SelectedItem.Text);
            Session["taskId"] = Convert.ToInt32(Session["taskId"]) + 1;
            Audit.auditUserTaskR(Session.SessionID, Session["mid"].ToString(), Session["taskId"].ToString(), "VWA", "Balancing", "Balancing", ddlYrmo.SelectedItem.Text);
            resultdiv.Visible = true;
        }
        catch (Exception ex)
        {            
            lbl_error.Text = ex.Message;
        }
    }

    protected void lnk_genBalSumRpt_OnClick(object sender, EventArgs e)
    {
        errorDiv1.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            createTables();
            bankReport();
            remitReport();
            detailReport();
            getAdjustments();
            summaryReport();
            VWA_Results.GenerateBalReport("BalSum", yrmo, tbSum1, tbSum2, tbFinal);
        }
        catch (Exception ex)
        {
            lblError1.Text = "Error in generating VWA Balance Summary Report<br />" + ex.Message;
        }
    }

    protected void lnk_genBalAuditRpt_OnClick(object sender, EventArgs e)
    {
        errorDiv1.Visible = false;
        string yrmo = ddlYrmo.SelectedItem.Text;

        try
        {
            createTables();
            bankReport();
            remitReport();
            VWA_Results.GenerateBalReport("BalSumAudit", yrmo, tbSum1, tbSum2, tbFinal);
        }
        catch (Exception ex)
        {
            lblError1.Text = "Error in generating VWA Balance Audit Report<br />" + ex.Message;
        }
    }

    private void createTables()
    {
        tbSum1.Clear();
        tbSum1.Columns.Clear();
        tbSum2.Clear();
        tbSum2.Columns.Clear();
        DataColumn col;
        //bank statement table
        col = new DataColumn("Bank Post Date"); tbSum1.Columns.Add(col);
        col = new DataColumn("Bank RefNum"); tbSum1.Columns.Add(col);
        col = new DataColumn("Bank FDX Amount"); tbSum1.Columns.Add(col);
        col = new DataColumn("Bank VWA Amount"); tbSum1.Columns.Add(col);
        
        //remit table
        col = new DataColumn("Remit WireDt"); tbSum2.Columns.Add(col);
        col = new DataColumn("Remit ClientId"); tbSum2.Columns.Add(col);
        col = new DataColumn("Remit Client Name"); tbSum2.Columns.Add(col);
        col = new DataColumn("Remit FDX Amount"); tbSum2.Columns.Add(col);
        col = new DataColumn("Remit VWA Amount"); tbSum2.Columns.Add(col);

        tbFinal.Clear();
        tbFinal.Columns.Clear();
        col = new DataColumn("Type"); tbFinal.Columns.Add(col);
        col = new DataColumn("Amount"); tbFinal.Columns.Add(col);
        col = new DataColumn("Comments"); tbFinal.Columns.Add(col);
        
    }

    private void bankReport()
    {
        DataSet dsresult = new DataSet();
        dsresult = VWAImportDAL.getBOAData(ddlYrmo.SelectedItem.Text);
        DataRow[] rows;
        DataRow rowNew;
        decimal _fdx, _vwa, _disab, _disabT;
        _bfdxT = 0; _bvwaT = 0; _disabT = 0;
        if (dsresult.Tables[0].Rows.Count > 0)
        {
            rows = dsresult.Tables[0].Select("[WireTo] IN ('FDX','VWA','DisAb')"); 
            foreach (DataRow dr in rows)
            {
                _fdx = 0; _vwa = 0; _disab = 0;
                rowNew = tbSum1.NewRow();
                rowNew["Bank Post Date"] = dr["PostDate"];
                if(dr["WireTo"].ToString().Equals("FDX"))
                {
                    _fdx = Convert.ToDecimal(dr["Amount"]);
                    
                }
                if (dr["WireTo"].ToString().Equals("VWA"))
                {
                    _vwa = Convert.ToDecimal(dr["Amount"]);
                }
                if (dr["WireTo"].ToString().Equals("DisAb"))
                {
                    _disab = Convert.ToDecimal(dr["Amount"]);
                    _fdx = _disab;
                }
                rowNew["Bank RefNum"] = dr["RefNum"];
                rowNew["Bank FDX Amount"] = _fdx.ToString().Equals("0") ? "" : _fdx.ToString();
                rowNew["Bank VWA Amount"] = _vwa.ToString().Equals("0") ? "" : _vwa.ToString();                
                tbSum1.Rows.Add(rowNew);
            }

            _bfdxT =
                    Convert.ToDecimal(dsresult.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND [WireTo] = 'FDX'").ToString());
            _bvwaT =
                    Convert.ToDecimal(dsresult.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND [WireTo] = 'VWA'").ToString());
            _disabT =
                    Convert.ToDecimal(dsresult.Tables[0].Compute("SUM([Amount])", "[importType] = 'Detail' AND [WireTo] = 'DisAb'").ToString());

            _bfdxT = _bfdxT + _disabT;

            rowNew = tbSum1.NewRow();
            rowNew["Bank RefNum"] = "Total:";
            rowNew["Bank FDX Amount"] = _bfdxT.ToString();
            rowNew["Bank VWA Amount"] = _bvwaT.ToString();
            tbSum1.Rows.Add(rowNew);
        }
    }

    private void remitReport()
    {
        DataTable dtsumTemp = new DataTable();
        DataRow rowNew;
        _rfdxT = 0; _rvwaT = 0;

        dtsumTemp = VWA_DAL.getRemitData_4Balancing(ddlYrmo.SelectedItem.Text);
        if (dtsumTemp.Rows.Count > 0)
        {
            foreach (DataRow dr in dtsumTemp.Rows)
            {
                rowNew = tbSum2.NewRow();
                rowNew["Remit WireDt"] = dr["WireDt"];
                rowNew["Remit ClientId"] = dr["ClientId"];
                rowNew["Remit Client Name"] = dr["ClientName"];
                rowNew["Remit FDX Amount"] = dr["Acme"].ToString().Equals("0") ? "" : dr["Acme"];
                rowNew["Remit VWA Amount"] = dr["VWA"].ToString().Equals("0") ? "" : dr["VWA"];
                tbSum2.Rows.Add(rowNew);
            }

            _rfdxT =
                   Convert.ToDecimal(dtsumTemp.Compute("SUM([Acme])",String.Empty).ToString());
            _rvwaT =
                    Convert.ToDecimal(dtsumTemp.Compute("SUM([VWA])", String.Empty).ToString());

            rowNew = tbSum2.NewRow();
            rowNew["Remit Client Name"] = "Total:";
            rowNew["Remit FDX Amount"] = _rfdxT;
            rowNew["Remit VWA Amount"] = _rvwaT;
            tbSum2.Rows.Add(rowNew);
        }
    }

    private void detailReport()
    {
        DataTable dttransTemp = new DataTable();        
        _dfdxT = 0; _dvwaT = 0;

        dttransTemp = VWA_DAL.getTransactionData_4Balancing(ddlYrmo.SelectedItem.Text);
        if (dttransTemp.Rows.Count > 0)
        {
            foreach (DataRow dr in dttransTemp.Rows)
            {
                _dfdxT = Convert.ToDecimal(dr["Acme"]);
                _dvwaT = Convert.ToDecimal(dr["VWA"]);
            }
        }
    }

    private void getAdjustments()
    {
        DataTable dtadjTemp = new DataTable();
        
        _radj = new List<decimal>(); _badj = new List<decimal>(); _dadj = new List<decimal>();
        _rnotes = new List<string>(); _bnotes = new List<string>(); _dnotes = new List<string>();

        dtadjTemp = VWA_DAL.getAdjustmentData_4Balancing(ddlYrmo.SelectedItem.Text);
        if (dtadjTemp.Rows.Count > 0)
        {
            foreach (DataRow dr in dtadjTemp.Rows)
            {
                string _src = dr["AdjType"].ToString();
                switch (_src)
                {
                    case "Remittance":
                        _radj.Add(Convert.ToDecimal(dr["Amount"]));
                        _rnotes.Add(dr["Notes"].ToString());
                        break;
                    case "Bank Statement":
                        _badj.Add(Convert.ToDecimal(dr["Amount"]));
                        _bnotes.Add(dr["Notes"].ToString());
                        break;
                    case "Detail File":
                        _dadj.Add(Convert.ToDecimal(dr["Amount"]));
                        _dnotes.Add(dr["Notes"].ToString());
                        break;
                }
            }
        }
    }

    private void summaryReport()
    {
        DataRow rowNew;

        //Bank Summary
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Bank Acme Amount";
        rowNew["Amount"] = _bfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Bank VWA Amount";
        rowNew["Amount"] = _bvwaT;
        tbFinal.Rows.Add(rowNew);
        
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Bank Total:";
        rowNew["Amount"] = _bfdxT + _bvwaT;
        tbFinal.Rows.Add(rowNew);

        //Adjustments
        for (int i = 0; i < _badj.Count; i++)
        {
            rowNew = tbFinal.NewRow();
            rowNew["Type"] = "Bank Adjustment Amount";
            rowNew["Amount"] = _badj[i];
            rowNew["Comments"] = _bnotes[i];
            tbFinal.Rows.Add(rowNew);
        }

        //Empty row
        rowNew = tbFinal.NewRow();
        tbFinal.Rows.Add(rowNew);

        //Remit Summary
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Remit Acme Amount";
        rowNew["Amount"] = _rfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Remit VWA Amount";
        rowNew["Amount"] = _rvwaT;
        tbFinal.Rows.Add(rowNew);
        
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Remit Total:";
        rowNew["Amount"] = _rfdxT + _rvwaT;
        tbFinal.Rows.Add(rowNew);

        //Adjustments
        for (int j = 0; j < _radj.Count; j++)
        {
            rowNew = tbFinal.NewRow();
            rowNew["Type"] = "Remit Adjustment Amount";
            rowNew["Amount"] = _radj[j];
            rowNew["Comments"] = _rnotes[j];
            tbFinal.Rows.Add(rowNew);
        }

        //Empty row
        rowNew = tbFinal.NewRow();
        tbFinal.Rows.Add(rowNew);

        //Detail Summary
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Detail Acme Amount";
        rowNew["Amount"] = _dfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Detail VWA Amount";
        rowNew["Amount"] = _dvwaT;
        tbFinal.Rows.Add(rowNew);
        
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Detail Total:";
        rowNew["Amount"] = _dfdxT + _dvwaT;
        tbFinal.Rows.Add(rowNew);

        //Adjustments
        for (int k = 0; k < _dadj.Count; k++)
        {
            rowNew = tbFinal.NewRow();
            rowNew["Type"] = "Detail Adjustment Amount";
            rowNew["Amount"] = _dadj[k];
            rowNew["Comments"] = _dnotes[k];
            tbFinal.Rows.Add(rowNew);
        }

        //Empty row
        rowNew = tbFinal.NewRow();
        tbFinal.Rows.Add(rowNew);

        //Difference Summary
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Bank Remit Acme Amount";
        rowNew["Amount"] = _bfdxT - _rfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Bank Remit VWA Amount";
        rowNew["Amount"] = _bvwaT - _rvwaT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Bank Detail Acme Amount";
        rowNew["Amount"] = _bfdxT - _dfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Bank Detail VWA Amount";
        rowNew["Amount"] = _bvwaT - _dvwaT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Remit Detail Acme Amount";
        rowNew["Amount"] = _rfdxT - _dfdxT;
        tbFinal.Rows.Add(rowNew);
        rowNew = tbFinal.NewRow();
        rowNew["Type"] = "Diff Remit Detail VWA Amount";
        rowNew["Amount"] = _rvwaT - _dvwaT;
        tbFinal.Rows.Add(rowNew);  
    }

    void SetNoRecsMsg()
    {
        if ((tbSum1.Rows.Count > 0) || (tbSum2.Rows.Count > 0))
            lbl_balAudit.Visible = false;
        else
            lbl_balAudit.Visible = true;

        if (tbFinal.Rows.Count > 0)
            lbl_balSum.Visible = false;
        else
            lbl_balSum.Visible = true;
    }
}

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
using EBA.Desktop.VWA;
using EBA.Desktop.Admin;

public partial class VWA_VWA_Cases : System.Web.UI.Page
{
    private const string _category = "Individual Case";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        }        
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M400", "M405");
            Session["yrmo"] = "";
            bindData();
            VWA_Results.SavePrintFiles(_category, Page.Request.QueryString["id"]);
        }
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
        clearMessages();
    }

    void bindData()
    {
        clearMessages();
        string _cnum = Page.Request.QueryString["id"];       
        DataTable dtcontract = new DataTable();
        DataTable dtFinance = new DataTable();
        DataRow rowNew;
        DataColumn col;

        col = new DataColumn("Total Benefit"); dtFinance.Columns.Add(col);
        col = new DataColumn("Total Recovery"); dtFinance.Columns.Add(col);
        col = new DataColumn("Total Fees"); dtFinance.Columns.Add(col);
        col = new DataColumn("Total Net"); dtFinance.Columns.Add(col);

        try
        {
            dtcontract = VWA_DAL.getContractDetails(_cnum);
            int i = 0;
            foreach (DataRow dr in dtcontract.Rows)
            {
                if (i < 1)
                {
                    // Contract Tab data
                    Session["yrmo"] = dr["yrmo"].ToString();
                    txtCon.Text = dr["ContractNo"].ToString();
                    txtClient.Text = dr["ClientId"].ToString();
                    txtClientName.Text = dr["Clientdsc"].ToString();                    
                    txtSSN.Text = VWA.formatSSN("dashed",dr["SSNO"].ToString());
                    txtGroup.Text = dr["GroupNo"].ToString();
                    txtGroupName.Text = dr["Groupdsc"].ToString();
                    txtName.Text = dr["Name"].ToString();
                    txtPatient.Text = dr["PatientName"].ToString();
                    txtRelation.Text = dr["RelCd"].ToString();
                    txtDisabled.Text = dr["DisabCd"].ToString();
                    txtStatusCd1.Text = dr["StatusCd"].ToString();
                    txtStatusCd2.Text = dr["Statusdsc"].ToString();
                    txtAcciDt.Text = dr["AccDt"].ToString();
                    txtRecoveryDt.Text = dr["RecDt"].ToString();
                    txtOpenDt.Text = dr["OpenDt"].ToString();
                    txtCloseDt.Text = dr["CloseDt"].ToString();
                    txtLastUpdate.Text = dr["SysDt"].ToString();

                    //Financial Tab data - First grid
                    rowNew = dtFinance.NewRow();
                    rowNew["Total Benefit"] = Convert.ToDecimal(dr["BenPaid"].ToString()).ToString("C");
                    rowNew["Total Recovery"] = Convert.ToDecimal(dr["RecAmt"].ToString()).ToString("C");
                    rowNew["Total Fees"] = Convert.ToDecimal(dr["TotFees"].ToString()).ToString("C");
                    rowNew["Total Net"] = Convert.ToDecimal(dr["NetAmt"].ToString()).ToString("C");
                    dtFinance.Rows.Add(rowNew);

                    grdvFinancial1.DataSource = dtFinance;
                    grdvFinancial1.DataBind();

                    //Financial Tab data - Second grid
                    SqlDataSource1.SelectParameters.Add("cnum", TypeCode.String, dr["ContractNo"].ToString());
                    SqlDataSource1.SelectParameters.Add("source", TypeCode.String, "FinanceTab2");
                    grdvFinancial2.DataBind();

                    //Notes Tab
                    txtNotes.Text = dr["Notes"].ToString();
                    i++;
                }
                else
                {
                    break;
                }
            }
            if(!Session["yrmo"].ToString().Equals(""))
            {
                DataTable dthis = VWA_DAL.getHistory_4Cases(_cnum);
                grdvHistory.DataSource = dthis;
                grdvHistory.DataBind();
            }           
        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lblError1.Text = ex.Message;
        }
    }

    protected void btnNotes_OnClick(Object sender, EventArgs e)
    {
        clearMessages();
        try
        {
            VWA_DAL.insertCaseNotes(txtNotes.Text, Session["yrmo"].ToString(), txtCon.Text);
            lbl_result.Visible = true;
        }
        catch(Exception ex)
        {
            errorDiv1.Visible = true;
            lblError1.Text = ex.Message;
        }
    }

    void clearMessages()
    {
        errorDiv1.Visible = false;
        lblError1.Text = "";
    }

    protected void lnk_genCaseRpt_OnClick(object sender, EventArgs e)
    {
        clearMessages();

        try
        {
            VWA_Results.GenerateReport("CaseInfo", Page.Request.QueryString["id"]);
        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lblError1.Text = ex.Message;
        }
    }
}

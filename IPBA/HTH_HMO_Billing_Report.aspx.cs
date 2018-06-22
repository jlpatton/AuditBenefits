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
using EBA.Desktop;
using EBA.Desktop.Admin;
using EBA.Desktop.Audit;
using EBA.Desktop.IPBA;

public partial class HTH_HTH_HMO_Billing_Report : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M300", "M302");
            ddlYrmoList();           
        }       
    }

    protected void ddlYrmoList()
    {
        ImportDAL iobj = new ImportDAL();
        string prevYRMO;

        for (int i = 0; i < 6; i++)
        {
            prevYRMO = iobj.getPrevYRMO(i);
            ddlYrmo.Items.Add(prevYRMO);
        }
        ddlYrmo.Items.Add("New Yrmo");
    }

    protected void ddlYRMO_selectedIndexchanged(object sender, EventArgs e)
    {
        resultDiv.Visible = false;
        AllRptDiv.Visible = false;
        lbl_error.Text = "";

        try
        {
            if (ddlYrmo.SelectedItem.Text.Equals("New Yrmo"))
            {
                txtPrevYRMO.Text = "";
                ddlYrmo.Visible = false;
                lblPlancd.Visible = false;
                ddlPlancd.Visible = false;
                btn_submit.Visible = false;
                txtPrevYRMO.Visible = true;
                btnAddYrmo.Visible = true;
                btnCancelYrmo.Visible = true; 
            }            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void ddlPlancd_selectedIndexchanged(object sender, EventArgs e)
    {
        resultDiv.Visible = false;
        AllRptDiv.Visible = false;
    }

    protected void btn_ADDYrmo(object sender, EventArgs e)
    {
        int index = 0;
        lbl_error.Text = "";

        try
        {
            if (!ddlYrmo.Items.Contains(new ListItem(txtPrevYRMO.Text.Trim())))
            {
                ddlYrmo.Items.Add(txtPrevYRMO.Text.Trim());
            }
            for (int i = 0; i < ddlYrmo.Items.Count; i++)
            {
                if (ddlYrmo.Items[i].Text == txtPrevYRMO.Text)
                {
                    index = i;
                }
            }
            ddlYrmo.SelectedIndex = index;
            ddlYrmo.Visible = true;
            lblPlancd.Visible = true;
            ddlPlancd.Visible = true;
            btn_submit.Visible = true;
            txtPrevYRMO.Visible = false;
            btnAddYrmo.Visible = false;
            btnCancelYrmo.Visible = false;            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_CancelYrmo(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        try
        {
            ddlYrmo.SelectedIndex = 0;
            ddlYrmo.Visible = true;
            lblPlancd.Visible = true;
            ddlPlancd.Visible = true;
            btn_submit.Visible = true;
            txtPrevYRMO.Visible = false;
            btnAddYrmo.Visible = false;
            btnCancelYrmo.Visible = false;            
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;

        if (Page.IsValid)
        {
            lbl_error.Text = "";
            try
            {
                if (RptsImported())
                {
                    ViewResult();
                }                
            }
            catch (Exception ex)
            {                        
                lbl_error.Text = "Error - " + ex.Message;
            }
        }
    }

    Boolean RptsImported()
    {
        //string yrmo = ddlYrmo.SelectedItem.Text;
        //string[] source = { "Greenbar", "ADP" };
        //string[] rpts = { "PRISM Greenbar Report", "ADP COBRA Detail Report" };
        //string rptsNotImported = "";

        //for (int i = 0; i < source.Length; i++)
        //{
        //    if (rptsNotImported.Equals("") && source[i].Equals("Greenbar") && yrmo.Equals("200801"))
        //        return true;
        //    if (!IPBAImportDAL.pastImport(source[i],yrmo))
        //    {
        //        rptsNotImported += rpts[i];
        //        if (i != (source.Length - 1))
        //            rptsNotImported += "and ";
        //    }
        //}

        //if (rptsNotImported != "")
        //{
        //    lbl_error.Text = rptsNotImported + " for yrmo - " + yrmo + " not imported";
        //    return false;
        //}
        return true;
    }

    void ViewResult()
    {        
        imgSum.ImageUrl = "~/styles/images/collapsed1.gif";
        SumRptDiv.Visible = false;
        imgDet.ImageUrl = "~/styles/images/collapsed1.gif";
        DetRptDiv.Visible = false;
        imgAdj.ImageUrl = "~/styles/images/collapsed1.gif";
        AdjRptDiv.Visible = false;
        imgHTH.ImageUrl = "~/styles/images/collapsed1.gif";
        HTHRptDiv.Visible = false;
        
        string yrmo = ddlYrmo.SelectedItem.Text;
        if (ddlPlancd.SelectedItem.Value.Equals("--All--"))
            AllRptDiv.Visible = true;
        else
        {
            resultDiv.Visible = true;
            if (!ddlPlancd.SelectedValue.Equals("P5"))
                HTHAnthemDiv.Visible = false;
            else
                HTHAnthemDiv.Visible = true;
        }
    }

    protected void lnkSum_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        try
        {
            if (SumRptDiv.Visible == true)
            {
                imgSum.ImageUrl = "~/styles/images/collapsed1.gif";
                SumRptDiv.Visible = false;
            }
            else
            {
                imgSum.ImageUrl = "~/styles/images/expanded1.gif";
                SumRptDiv.Visible = true;
                bindResult("Summary");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkDet_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        try
        {
            if (DetRptDiv.Visible == true)
            {
                imgDet.ImageUrl = "~/styles/images/collapsed1.gif";
                DetRptDiv.Visible = false;
            }
            else
            {
                imgDet.ImageUrl = "~/styles/images/expanded1.gif";
                DetRptDiv.Visible = true;
                bindResult("Detail");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkAdj_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        try
        {
            if (AdjRptDiv.Visible == true)
            {
                imgAdj.ImageUrl = "~/styles/images/collapsed1.gif";
                AdjRptDiv.Visible = false;
            }
            else
            {
                imgAdj.ImageUrl = "~/styles/images/expanded1.gif";
                AdjRptDiv.Visible = true;
                bindResult("Adjustment");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkHTH_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";

        try
        {
            if (HTHRptDiv.Visible == true)
            {
                imgHTH.ImageUrl = "~/styles/images/collapsed1.gif";
                HTHRptDiv.Visible = false;
            }
            else
            {
                imgHTH.ImageUrl = "~/styles/images/expanded1.gif";
                HTHRptDiv.Visible = true;
                bindResult("HTH");
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }

    protected void lnk_genSumRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;
        IPBA dobj = new IPBA();        
        CreateExcelRpt xlobj = new CreateExcelRpt();        
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {
            filename = yrmo + "_IPBA_SUM_" + plancode;
            titles = new string[1][];
            sheetnames = new string[1];
            colsFormat = new string[1][];
            colsWidth = new int[1][];
            
            if (plancode.Equals("P5")) plancode = "P5/P5";

            if (plancode.Contains("P5"))
                titles[0] = new string[] { "HTH Billing Summary Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };  
            else
                titles[0] = new string[] { "Local HMO Billing Summary Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };

            sheetnames[0] = "Summary";
            colsFormat[0] = new string[] { "number", "string", "string", "number_comma", "currency", "currency" };
            colsWidth[0] = new int[] { 40, 110, 80, 35, 75, 75 };

            dsFinal = dobj.GetSummaryRpt(yrmo, plancode);
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty); 
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Summary Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnk_genSumRptAll_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();
        DataSet ds = new DataSet(); ds.Clear();
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {            
            filename = yrmo + "_IPBA_SUM_All";
            titles = new string[ddlPlancd.Items.Count - 1][];
            sheetnames = new string[ddlPlancd.Items.Count - 1];
            colsFormat = new string[ddlPlancd.Items.Count - 1][];
            colsWidth = new int[ddlPlancd.Items.Count - 1][];

            for (int i = 1; i < (ddlPlancd.Items.Count); i++)
            {
                plancode = ddlPlancd.Items[i].Value;

                if (plancode.Equals("P5")) plancode = "P5/P5";

                if (plancode.Contains("P5"))
                    titles[i - 1] = new string[] { "HTH Billing Summary Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };
                else
                    titles[i - 1] = new string[] { "Local HMO Billing Summary Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };

                sheetnames[i - 1] = "Summary_" + plancode.Substring(0, 2);
                colsFormat[i - 1] = new string[] { "number", "string", "string", "number_comma", "currency", "currency" };
                colsWidth[i - 1] = new int[] { 40, 110, 80, 35, 75, 75 };

                ds = dobj.GetSummaryRpt(yrmo, plancode);
                ds.Tables[0].TableName = "Table_" + plancode.Substring(0, 2);
                dsFinal.Tables.Add(ds.Tables[0].Copy());
                dsFinal.Tables[i - 1].TableName = "TableF_" + plancode.Substring(0, 2);
                ds.Clear();
            }

            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Summary Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnk_genDetRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;        
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();        
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {            
            filename = yrmo + "_IPBA_DET_" + plancode;
            titles = new string[1][];
            sheetnames = new string[1];
            colsFormat = new string[1][];
            colsWidth = new int[1][];

            if (plancode.Equals("P5")) plancode = "P5/P5";

            if (plancode.Contains("P5"))
                titles[0] = new string[] { "HTH Billing Detail Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };
            else
                titles[0] = new string[] { "Local HMO Billing Detail Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };

            sheetnames[0] = "Detail";
            colsFormat[0] = new string[] { "string", "string", "string", "string", "string", "string", "currency" };
            colsWidth[0] = new int[] { 90, 35, 60, 125, 35, 55, 65 };

            dsFinal = dobj.GetDetailRpt(yrmo, plancode);
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty); 

        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Detail Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnk_genDetRptAll_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();
        DataSet ds = new DataSet(); ds.Clear();
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {            
            filename = yrmo + "_IPBA_DET_All";
            titles = new string[ddlPlancd.Items.Count - 1][];
            sheetnames = new string[ddlPlancd.Items.Count - 1];
            colsFormat = new string[ddlPlancd.Items.Count - 1][];
            colsWidth = new int[ddlPlancd.Items.Count - 1][];

            for (int i = 1; i < (ddlPlancd.Items.Count); i++)
            {
                plancode = ddlPlancd.Items[i].Value;

                if (plancode.Equals("P5")) plancode = "P5/P5";

                if (plancode.Contains("P5"))
                    titles[i-1] = new string[] { "HTH Billing Detail Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };
                else
                    titles[i-1] = new string[] { "Local HMO Billing Detail Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };

                sheetnames[i-1] = "Detail_" + plancode.Substring(0, 2);
                colsFormat[i-1] = new string[] { "string", "string", "string", "string", "string", "string", "currency" };
                colsWidth[i-1] = new int[] { 90, 35, 60, 125, 35, 55, 65 };

                ds = dobj.GetDetailRpt(yrmo, plancode);
                ds.Tables[0].TableName = "Table_" + plancode.Substring(0, 2);
                dsFinal.Tables.Add(ds.Tables[0].Copy());
                dsFinal.Tables[i-1].TableName = "TableF_" + plancode.Substring(0, 2);
                ds.Clear();            
            }
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Detail Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnk_genAdjRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename, footernotes;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;     
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();        
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {
            footernotes = "Exceeded maximum months adjustment that system can be applied. Additional adjustments need to be manually processed.";
            filename = yrmo + "_IPBA_ADJ_" + plancode;
            titles = new string[1][];
            sheetnames = new string[1];
            colsFormat = new string[1][];
            colsWidth = new int[1][];

            if (plancode.Equals("P5")) plancode = "P5/P5";

            if (plancode.Contains("P5"))
                titles[0] = new string[] { "HTH Billing Adjustment Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };
            else
                titles[0] = new string[] { "Local HMO Billing Adjustment Report: " + ddlPlancd.SelectedItem.Text, "Year-Month: " + yrmo };

            sheetnames[0] = "Adjustments";
            colsFormat[0] = new string[] { "number", "string", "string", "string", "string", "number", "string", "number", "currency", "string" };
            colsWidth[0] = new int[] { 30, 60, 30, 60, 100, 40, 180, 40, 65, 30 };

            dsFinal = dobj.GetAdjustmentRpt(yrmo, plancode);
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, footernotes);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Adjustment Report for YRMO - " + yrmo + "<br />" + ex.Message;
        } 
    }

    protected void lnk_genAdjRptAll_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        string filename, footernotes;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();
        DataSet ds = new DataSet(); ds.Clear();
        DataSet dsFinal = new DataSet(); dsFinal.Clear();

        try
        {
            footernotes = "Exceeded maximum months adjustment that system can be applied. Additional adjustments need to be manually processed.";
            filename = yrmo + "_IPBA_ADJ_All";
            titles = new string[ddlPlancd.Items.Count - 1][];
            sheetnames = new string[ddlPlancd.Items.Count - 1];
            colsFormat = new string[ddlPlancd.Items.Count - 1][];
            colsWidth = new int[ddlPlancd.Items.Count - 1][];

            for (int i = 1; i < (ddlPlancd.Items.Count); i++)
            {
                plancode = ddlPlancd.Items[i].Value;

                if (plancode.Equals("P5")) plancode = "P5/P5";

                if (plancode.Contains("P5"))
                    titles[i-1] = new string[] { "HTH Billing Adjustment Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };
                else
                    titles[i-1] = new string[] { "Local HMO Billing Adjustment Report: " + ddlPlancd.Items[i].Text, "Year-Month: " + yrmo };

                sheetnames[i-1] = "Adjustments_" + plancode.Substring(0, 2);
                colsFormat[i-1] = new string[] { "number", "string", "string", "string", "string", "number", "string", "number", "currency", "string" };
                colsWidth[i - 1] = new int[] { 30, 60, 30, 60, 100, 40, 180, 40, 65, 30 };

                ds = dobj.GetAdjustmentRpt(yrmo, plancode);
                ds.Tables[0].TableName = "Table_" + plancode.Substring(0, 2);
                dsFinal.Tables.Add(ds.Tables[0].Copy());
                dsFinal.Tables[i-1].TableName = "TableF_" + plancode.Substring(0, 2);
                ds.Clear();
            }
            xlobj.ExcelXMLRpt(dsFinal, filename, sheetnames, titles, colsFormat, colsWidth, footernotes);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH/HMO Billing Adjustment Report for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void lnk_genHTHRpt_OnClick(object sender, EventArgs e)
    {
        lbl_error.Text = "";
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = "P5/P5";        
        IPBA dobj = new IPBA();
        CreateExcelRpt xlobj = new CreateExcelRpt();
        DataSet ds = new DataSet();
        ds.Clear();
        string filename = yrmo.Substring(0, 4) + "_" + yrmo + "_HMODETSTD_P5";
        string[] sheetnames = { filename };
        string[][] titles = { new string[] { "" } };
        string[][] colsFormat = { new string[] { "string", "string", "string", "string", "string", "string", "currency" } };
        int[][] colsWidth = { new int[] { 60, 40, 65, 130, 35, 60, 65 } };

        try
        {
            ds = dobj.GetHTHAnthemRpt(yrmo, plancode);
            xlobj.ExcelXMLRpt(ds, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error in generating HTH International Report for Anthem module for YRMO - " + yrmo + "<br />" + ex.Message;
        }
    }

    protected void bindResult(string _src)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        if (plancode.Equals("P5"))
            plancode = "P5/P5";
        IPBA dobj = new IPBA();        

        switch (_src)
        {
            case "Summary":
                ds = dobj.GetSummaryRpt(yrmo, plancode);
                grdvSum.DataSource = ds;
                grdvSum.DataBind();
                ds.Clear();
                break;

            case "Detail":
                ds = dobj.GetDetailRpt(yrmo, plancode);
                grdvDet.DataSource = ds;
                grdvDet.DataBind();
                ds.Clear();
                break;

            case "Adjustment":
                ds = dobj.GetAdjustmentRpt(yrmo, plancode);
                grdvAdj.DataSource = ds;
                grdvAdj.DataBind();
                ds.Clear();
                break;

            case "HTH":
                ds = dobj.GetHTHAnthemRpt(yrmo, "P5");
                grdvHTH.DataSource = ds;
                grdvHTH.DataBind();
                ds.Clear();
                break;
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction, string source)
    {
        DataTable dt;
        DataView dv;
        string yrmo = ddlYrmo.SelectedItem.Text;
        string plancode = ddlPlancd.SelectedItem.Value;
        if (plancode.Equals("P5"))
            plancode = "P5/P5";
        IPBA dobj = new IPBA();        

        switch (source)
        {
            case "Summary":
                dt = dobj.GetSummaryRpt(yrmo, plancode).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvSum.DataSource = dv;
                grdvSum.DataBind();
                break;
            case "Detail":
                dt = dobj.GetDetailRpt(yrmo, plancode).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvDet.DataSource = dv;
                grdvDet.DataBind();
                break;
            case "Adjustment":
                dt = dobj.GetAdjustmentRpt(yrmo, plancode).Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvAdj.DataSource = dv;
                grdvAdj.DataBind();
                break;
            case "HTH":
                dt = dobj.GetHTHAnthemRpt(yrmo, "P5").Tables[0];
                dv = new DataView(dt);
                dv.Sort = sortExpression + direction;
                grdvHTH.DataSource = dv;
                grdvHTH.DataBind();
                break;
        }
    }

    protected void grdvSum_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "Summary");
    }

    protected void grdvDet_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "Detail");
    }

    protected void grdvAdj_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "Adjustment");
    }

    protected void grdvHTH_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "HTH");
    }

    void gridviewSort(string sortExpression, string source)
    {
        lbl_error.Text = "";

        try
        {
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, DESCENDING, source);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, ASCENDING, source);
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = "Error: " + ex.Message;
        }
    }    
}

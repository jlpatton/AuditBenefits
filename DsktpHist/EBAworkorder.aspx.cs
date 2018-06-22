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
using System.Data.SqlTypes;
using EBA.Desktop.Admin;

public partial class EBAworkorder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager SM = ((ScriptManager)(Master.FindControl("ScriptManager1")));
        //SM.RegisterAsyncPostBackControl(btnSearch);

        if (Session["mid"] == null || Int32.Parse(Session["mid"].ToString()) == 0)
        {
            Response.Redirect("~/Home.aspx");
        } 
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M700", "M704");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        string wonum = string.IsNullOrEmpty(txtSrchWOnum.Text) ? "" : txtSrchWOnum.Text;
        string proj = string.IsNullOrEmpty(txtSrchType.Text) ? "" : txtSrchType.Text;
        proj = proj.ToUpper();

        clearPage();

        DsktpData DsktpObj = new DsktpData();
        
        ds = DsktpObj.retrieveWorkOrders(wonum, proj);
        if (ds.Tables[0].Rows.Count != 0)
        {
            GridView1.Visible = true;
            GridView1.DataSource = ds;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
        }
        else
        {
            clearPage();
            lblInfo.Text = "There were no records returned for the search criteria.";
            infoDiv1.Visible = true;
        }

        
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    protected void btnRetrieve_Click(object sender, EventArgs e)
    {
        
        //DsktpData DtopData = new DsktpData();
        //if (!string.IsNullOrEmpty(txtSSNo.Text))
        //{
        //    if (DtopData.checkExistRecord(txtSSNo.Text))
        //    {
        //        string ssn = txtSSNo.Text;

        //        clearPage();
        //        SpDepRecord dRec = new SpDepRecord();
        //        dRec = DtopData.getSpDepData(ssn);
        //        bindSpDepRecord(dRec);
        //        DataSet ds = DtopData.getSpDepElections(ssn);
        //        grvElections.DataSource = ds;
        //        grvElections.DataBind();

        //        int empno = Convert.ToInt32(txtXrefEmpno.Text);
        //        DataSet ds2 = DtopData.getSpDepNotes(empno, ssn);
        //        grvNotes.DataSource = ds2;
        //        grvNotes.DataBind();
        //    }
        //    else
        //    {
        //        clearPage();
        //        infoDiv1.Visible = true;
        //        lblInfo.Text = "There were no records returned for the search criteria.";
        //    }

        //}
    }

    protected void clearPage()
    {
        lblInfo.Text = "";
        infoDiv1.Visible = false;
        lblEmpHeading.Text = "Work Order #: ";
        txtAnalyst.Text = "";
        txtApproval.Text = "";
        txtApproveDt.Text = "";
        txtAssignDt.Text = "";
        txtAuthor.Text = "";
        txtComments.Text = "";
        txtCompDt.Text = "";
        txtConsiderations.Text = "";
        txtDescription.Text = "";
        txtDevComplDt.Text = "";
        txtHours.Text = "";
        txtJustification.Text = "";
        txtLvl.Text = "";
        txtMonths.Text = "";
        txtPriority.Text = "";
        txtProdApprovDt.Text = "";
        txtProjLead.Text = "";
        txtProjName.Text = "";
        txtReqDate.Text = "";
        txtReqDefDt.Text = "";
        txtReqType.Text = "";
        txtRespDueDt.Text = "";
        txtResults.Text = "";
        txtRisks.Text = "";
        txtStatAssignDt.Text = "";
        txtStatComments.Text = "";
        txtStatDt.Text = "";
        txtStatus.Text = "";
        txtTechSpecDt.Text = "";
        txtTestComplDt.Text = "";
        txtTitle.Text = "";
        txtWOno.Text = "";

        txtSrchType.Text = "";
        txtSrchWOnum.Text = "";
        
        GridView1.Visible = false;
        grvStatHist.Visible = false;
        grvUserChanges.Visible = false;

    }

    protected void LoadView4(int wonum, string proj)
    {
        DataSet ds = new DataSet();
        ds.Clear();
        DataSet ds1 = new DataSet();
        ds1.Clear();
        proj = proj.ToUpper();

        DsktpData DsktpObj = new DsktpData();

        ds = DsktpObj.retrieveStatHist(wonum, proj);
        if (ds.Tables[0].Rows.Count != 0)
        {
            grvStatHist.Visible = true;
            grvStatHist.DataSource = ds;
            grvStatHist.DataBind();
            grvStatHist.SelectedIndex = -1;
        }

        ds1 = DsktpObj.retrieveUserHist(wonum, proj);
        if (ds1.Tables[0].Rows.Count != 0)
        {
            grvUserChanges.Visible = true;
            grvUserChanges.DataSource = ds1;
            grvUserChanges.DataBind();
            grvUserChanges.SelectedIndex = -1;
        }
        
    }

    protected void bindWORecords(WOdetailRecord dRec, WOresponseRecord rRec, WOstatusRecord sRec)
    {
        lblEmpHeading.Text = "WO #: " + dRec.SeqNo.ToString() + " / "
                                + dRec.WOname.ToString().ToUpper();

        txtWOno.Text = dRec.SeqNo.ToString();
        txtReqType.Text = dRec.Type;
        txtTitle.Text = dRec.WOname;
        txtReqDate.Text = (dRec.ReqDt.ToShortDateString() == "1/1/0001" ? "" : dRec.ReqDt.ToShortDateString());
        txtRespDueDt.Text = (dRec.RespDt.ToShortDateString() == "1/1/0001" ? "" : dRec.RespDt.ToShortDateString());
        txtAuthor.Text = dRec.AuthorID;
        txtLvl.Text = dRec.ReqLvl.ToString();
        txtPriority.Text = dRec.Priority;
        txtDescription.Text = dRec.Description;
        txtJustification.Text = dRec.Justif;
        txtComments.Text = dRec.Comments;

        txtProjName.Text = rRec.ProjName;
        txtAnalyst.Text = rRec.Analyst;
        txtAssignDt.Text = (rRec.AssignDt.ToShortDateString() == "1/1/0001" ? "" : rRec.AssignDt.ToShortDateString());
        txtCompDt.Text = (rRec.CompleteDt.ToShortDateString() == "1/1/0001" ? "" : rRec.CompleteDt.ToShortDateString());
        txtResults.Text = rRec.Results;
        txtMonths.Text = (rRec.ManMonth == 0 ? "" : rRec.ManMonth.ToString());
        txtHours.Text = (rRec.ManHours == 0 ? "" : rRec.ManHours.ToString());
        txtConsiderations.Text = rRec.Considerations;
        txtRisks.Text = rRec.Risks;

        txtStatus.Text = sRec.Descr;
        txtStatDt.Text = (sRec.StatusDt.ToShortDateString() == "1/1/0001" ? "" : sRec.StatusDt.ToShortDateString());
        txtApproval.Text = (sRec.Approved.ToString() == "1" ? "Yes" : (sRec.Approved.ToString() == "0" ? "Not Reported" : ""));
        txtApproveDt.Text = (sRec.ReqApprovalDt.ToShortDateString() == "1/1/0001" ? "" : sRec.ReqApprovalDt.ToShortDateString());
        txtStatAssignDt.Text = (sRec.ProjAssignDt.ToShortDateString() == "1/1/0001" ? "" : sRec.ProjAssignDt.ToShortDateString());
        txtProjLead.Text = sRec.Developer;
        txtReqDefDt.Text = (sRec.ReqDefDt.ToShortDateString() == "1/1/0001" ? "" : sRec.ReqDefDt.ToShortDateString());
        txtTechSpecDt.Text = (sRec.TechSpecDt.ToShortDateString() == "1/1/0001" ? "" : sRec.TechSpecDt.ToShortDateString());
        txtDevComplDt.Text = (sRec.StartDt.ToShortDateString() == "1/1/0001" ? "" : sRec.StartDt.ToShortDateString());
        txtTestComplDt.Text = (sRec.CloseDt.ToShortDateString() == "1/1/0001" ? "" : sRec.CloseDt.ToShortDateString());
        txtProdApprovDt.Text = (sRec.GoDt.ToShortDateString() == "1/1/0001" ? "" : sRec.GoDt.ToShortDateString());
        txtStatComments.Text = sRec.Comments;
        


    }
        
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gvr = GridView1.Rows[index];
            string seqno = Server.HtmlDecode(gvr.Cells[1].Text);
            string proj = Server.HtmlDecode(gvr.Cells[3].Text);
            txtWOno.Text = seqno;
            txtReqType.Text = proj;
            
            DsktpData DtopData = new DsktpData();
            if (!string.IsNullOrEmpty(txtWOno.Text) && !string.IsNullOrEmpty(txtReqType.Text))
            {
                if (DtopData.checkExistWOrecord(Convert.ToInt32(txtWOno.Text),proj))
                {
                    int wonum = Convert.ToInt32(txtWOno.Text);
                    
                    clearPage();
                    WOdetailRecord dRec = new WOdetailRecord();
                    dRec = DtopData.getWOdetailData(wonum,proj);
                    
                    WOresponseRecord rRec = new WOresponseRecord();
                    rRec = DtopData.getWOresponseData(wonum,proj);

                    WOstatusRecord sRec = new WOstatusRecord();
                    sRec = DtopData.getWOstatusData(wonum,proj);

                    bindWORecords(dRec, rRec, sRec);

                    LoadView4(wonum, proj);

                    //DataSet ds = DtopData.getSpDepElections(ssn);
                    //grvElections.DataSource = ds;
                    //grvElections.DataBind();

                    //int empno = Convert.ToInt32(txtXrefEmpno.Text);
                    //DataSet ds2 = DtopData.getSpDepNotes(empno, ssn);
                    //grvNotes.DataSource = ds2;
                    //grvNotes.DataBind();
                }
                else
                {
                    infoDiv1.Visible = true;
                    lblInfo.Text = "There were no records returned for the search criteria.";
                }
            }
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //if (e.NewPageIndex < 0) e.Cancel = true;
    }
}

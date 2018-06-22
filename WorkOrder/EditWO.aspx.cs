using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using KMobile.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
    string initPhase;
    string ls_proj;
    int li_wonum;
    int li_respNum;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager SM = ((ScriptManager)(Master.FindControl("ScriptManager1")));
        SM.RegisterAsyncPostBackControl(btnAddParticipant);
        SM.RegisterAsyncPostBackControl(FormView2);
        WorkOrderBLL woLogic = new WorkOrderBLL();
        WorkOrder.WorkOrderDataTable WrkOrds = woLogic.GetWorkOrdersByWOnumProj(Convert.ToInt32(Request.QueryString["word_WOnum"]),Request.QueryString["word_Proj"].ToString());
        WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];
        ls_proj = Request.QueryString["word_Proj"];
        li_wonum = Convert.ToInt32(Request.QueryString["word_WOnum"]);
        
        RolesBLL roleLogic = new RolesBLL();
        

        if (!IsPostBack)
        {
            if ((WrkOrd.word_Status == "IMPL" && WrkOrd.word_statFlag == 1) || 
                (WrkOrd.word_Status == "GONG")) // &&
            {
                if (!(WrkOrd.word_Status == "GONG" &&
                    roleLogic.IsUserApprover(Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)),li_wonum,ls_proj))) // AND IsUserPMorUp?RoleBLL
                {
                    FormView1.DefaultMode = FormViewMode.ReadOnly;
                    FormView1.DataSourceID = ObjectDataSource2.ID;
                    FormView1.DataBind();
                    ddlParticipant.Enabled = false;
                    ddlParticipant.Visible = false;
                    ddlRole.Enabled = false;
                    ddlRole.Visible = false;
                    btnAddParticipant.Visible = false;
                    lblParticipant.Visible = false;
                    lblRole.Visible = false;
                    GridView2.Enabled = false;
                    GridView2.Visible = false;
                    ((LinkButton)(FormView1.FindControl("RespondButton"))).Visible = false;
                    
                }
                else
                {
                    ((LinkButton)(FormView1.FindControl("Approvals"))).Visible = true;
                }
            }
            

            bool lb_rtn = false;
            
            WorkOrderTableAdapters.WOroleTableAdapter roleAdapter = new WorkOrderTableAdapters.WOroleTableAdapter();
            WorkOrder.WOroleDataTable roleDT = roleAdapter.GetWOroleByWOproj(li_wonum, ls_proj);
            foreach (DataRow roleRow in roleDT)
            {
                if (roleRow["worl_uid"].ToString() == WrkOrd.word_Author.ToString())
                {
                    lb_rtn = true;

                    break;
                }
            }

            if (!lb_rtn) lb_rtn = roleLogic.InsertWOroles(li_wonum, Convert.ToInt32(WrkOrd.word_Author), ls_proj, "AUTH", false, true);
            lb_rtn = false;

            foreach (DataRow roleRow in roleDT)
            {
                if (roleRow["worl_uid"].ToString() == WrkOrd.word_BusnOwner.ToString())
                {
                    lb_rtn = true;
                    break;
                }
            }

            if (!lb_rtn) lb_rtn = roleLogic.InsertWOroles(li_wonum, Convert.ToInt32(WrkOrd.word_BusnOwner), ls_proj, "BOWN", false, true);
            lb_rtn = false;
            foreach (DataRow roleRow in roleDT)
            {
                if (roleRow["worl_uid"].ToString() == WrkOrd.word_PMorSME.ToString())
                {
                    lb_rtn = true;
                    break;
                }
            }

            if (!lb_rtn) lb_rtn = roleLogic.InsertWOroles(li_wonum, Convert.ToInt32(WrkOrd.word_PMorSME), ls_proj, "PM", false, true);
            lb_rtn = false;
            //GridView2.DataBind();
        }

        woResponseBLL responseLogic = new woResponseBLL();
        if (responseLogic.GetWOresponseCountByWOandProj(li_wonum, ls_proj) > 0)
        {
            FormView2.Visible = true;
        }
        else
        {
            FormView2.Visible = false;
        }

    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        int statflag;
        int rws = e.AffectedRows;
        woPhaseBLL woPhaseLogic = new woPhaseBLL();
        
        // If Phase changed (or PhaseStat is 'Completed') then a new row is needed in Phase table and WO updated with new Phase/PhaseNumber
        if (!e.NewValues["word_Status"].ToString().Equals(e.OldValues["word_Status"].ToString()) ||
            (!e.NewValues["word_statFlag"].ToString().Equals(e.OldValues["word_statFlag"].ToString()) && 
            e.NewValues["word_statFlag"].ToString().Equals("1")) ) 
        {
            int PhaseNum = woPhaseLogic.InsertPhaseRecord(li_wonum, 
                ls_proj, 0, DateTime.Now, e.NewValues["word_Status"].ToString(), 
                Convert.ToInt32(e.NewValues["word_statFlag"].ToString()), 
                Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));
        }

        WorkOrderBLL woLogic = new WorkOrderBLL();
        WorkOrder.WorkOrderDataTable WrkOrds = woLogic.GetWorkOrdersByWOnumProj(li_wonum, ls_proj);
        WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

        //If Phase Status is 'Completed' insert a new row in Phase and update WO with new Phase/Status/PhaseNumber
        
        if (Convert.ToUInt32(e.NewValues["word_statFlag"]) == 1) 
        {
            statflag = 0;
            string NewPhase = ""; 
            switch (e.NewValues["word_Status"].ToString())
            {
                case "IWOR":
                    NewPhase = "WORA";
                    break;
                case "WORA":
                    NewPhase = "WORD";
                    break;
                case "WORM":
                    NewPhase = "WORA";
                    break;
                case "WORD":
                    NewPhase = "WOTS";
                    break;
                case "WOTS":
                    NewPhase = "WDEV";
                    break;
                case "WDEV":
                    NewPhase = "WTST";
                    break;
                case "WTST":
                    NewPhase = "GONG";
                    break;
                case "GONG":
                    NewPhase = "IMPL";
                    break;
                case "IMPL":
                    NewPhase = "IMPL";
                    statflag = 1;
                    break;
                default:

                    break;
            }


            int PhaseNum = woPhaseLogic.InsertPhaseRecord(li_wonum,
                ls_proj, 0, DateTime.Now, NewPhase, statflag,
                Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));

            
            WrkOrd.word_Status = NewPhase;
            WrkOrd.word_statFlag = statflag;
            WrkOrd.word_StatNum = PhaseNum;

            bool lb_rtn;
            
            lb_rtn = woLogic.UpdateWorkOrder(WrkOrd.word_WOnum, WrkOrd.word_Proj, WrkOrd.word_StatNum,
                WrkOrd.word_Status, WrkOrd.word_statFlag, WrkOrd.word_Date, WrkOrd.word_Author,
                WrkOrd.word_Title, "", "", WrkOrd.word_Priority,
                WrkOrd.word_Descr, WrkOrd.word_Justify, WrkOrd.word_Cmnts, WrkOrd.word_PMorSME,
                WrkOrd.word_BusnOwner, WrkOrd.word_reqDueDate, WrkOrd.word_WOnum, WrkOrd.word_Proj);

            string ls_to;
            if (Convert.ToString(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)) == WrkOrd.word_PMorSME.ToString())
            {
                ls_to = WrkOrd.word_BusnOwner.ToString();
            }
            else
            {
                ls_to = WrkOrd.word_PMorSME.ToString();
            }

            string host = Request.ServerVariables["SERVER_NAME"];  //HttpContext.Current.Request.Url.Host;
            SmtpEmail mailMessage = new SmtpEmail();

            switch (NewPhase) // This logic makes sure that the authorizers get emails during GO/NoGO phase.
            {
                case "GONG":

                    RolesBLL WOroleLogic = new RolesBLL();
                    WorkOrder.WOroleTextDisplayDataTable woRoleDT;
                    woRoleDT = WOroleLogic.GetWOrolesByWOnumProj(li_wonum, ls_proj);
                    //woApprovalBLL apprvLogic = new woApprovalBLL();
                    //WorkOrder.WOapprovalDataTable apprvlDT;
                    //apprvlDT = apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);

                    // this code ensures that every approval record is created
                    foreach (WorkOrder.WOroleTextDisplayRow roleRow in woRoleDT)
                    {
                        if (Convert.ToBoolean(roleRow["worl_aprvl"]) == true)
                        {
                            ls_to = ls_to + "," + roleRow["worl_uid"].ToString();
                        }
                    }






                    lb_rtn = mailMessage.Send(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1), ls_to, "Work Order Approval - " + WrkOrd.word_Proj.ToString()
                        + " WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString(),
                        "WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString() + " requires your Go/NoGo action." +
                        " Log into the EBS Work Order Module for details. Please do not respond to this email." +
                        Environment.NewLine + Environment.NewLine + "http://" + host + "/WorkOrder/Approvals.aspx?word_WOnum=" +
                        li_wonum.ToString() + "&word_Proj=" + WrkOrd.word_Proj.ToString() + Environment.NewLine + Environment.NewLine +
                        "The most recent comment on this work order is as follows:" +
                        Environment.NewLine + Environment.NewLine + WrkOrd.word_Cmnts.ToString(), li_wonum, 0, WrkOrd.word_Proj.ToString());

                    break;

                default:

                    lb_rtn = mailMessage.Send(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1), ls_to, "Work Order Modification - " + WrkOrd.word_Proj.ToString()
                        + " WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString(),
                        "WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString() + " requires your attention." +
                        " Log into the EBS Work Order Module for details. Please do not respond to this email." +
                        Environment.NewLine + Environment.NewLine + "http://" + host + "/WorkOrder/EditWO.aspx?word_WOnum=" +
                        li_wonum.ToString() + "&word_Proj=" + WrkOrd.word_Proj.ToString() + Environment.NewLine + Environment.NewLine +
                        "The most recent comment on this work order is as follows:" +
                        Environment.NewLine + Environment.NewLine + WrkOrd.word_Cmnts.ToString(), li_wonum, 0, WrkOrd.word_Proj.ToString());

                    break;

            }


        }

        if (!e.NewValues["word_PMorSME"].ToString().Equals(e.OldValues["word_PMorSME"].ToString()) ||
            !e.NewValues["word_BusnOwner"].ToString().Equals(e.OldValues["word_BusnOwner"].ToString()))
        {

            string ls_to, ls_msg;
            if (Convert.ToString(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)) == WrkOrd.word_PMorSME.ToString())
            {
                ls_to = WrkOrd.word_BusnOwner.ToString();
            }
            else
            {
                ls_to = WrkOrd.word_PMorSME.ToString();
            }

            if (!e.NewValues["word_PMorSME"].ToString().Equals(e.OldValues["word_PMorSME"].ToString()))
            {
                ls_msg = " The Business Owner of this work order has changed.";
            }
            else
            {
                ls_msg = " The Project manager for this work order has changed. ";
            }

            string host = Request.ServerVariables["SERVER_NAME"];  //HttpContext.Current.Request.Url.Host;
            SmtpEmail mailMessage = new SmtpEmail();
            bool lb_rtn = mailMessage.Send(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1), ls_to, "Work Order Modification - " + WrkOrd.word_Proj.ToString()
                + " WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString(),
                "WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString() + " requires your attention." + ls_msg +
                " Log into the EBS Work Order Module for details. Please do not respond to this email."
                + Environment.NewLine + Environment.NewLine + "http://" + host + "/WorkOrder/EditWO.aspx?word_WOnum=" +
                li_wonum.ToString() + "&word_Proj=" + WrkOrd.word_Proj.ToString() + Environment.NewLine + Environment.NewLine + //"http://www.google.com"
                "The most recent comment on this work order is as follows:" +
                Environment.NewLine + Environment.NewLine + WrkOrd.word_Cmnts.ToString(), li_wonum, 0, WrkOrd.word_Proj.ToString());
            
        }

        Response.Redirect("~/WorkOrder/Default.aspx?word_Proj=" + ls_proj);

    }


    protected void DDDLphase_DataBinding(object sender, EventArgs e)
    {
        DataView dv = (DataView)ObjectDataSource1.Select();
        DataRow dr = (DataRow)dv.Table.Rows[0];
        string ls_fltr = dr.ItemArray[3].ToString();
        initPhase = ls_fltr;

        DropDownList ddl = sender as DropDownList;
        HiddenField FilterArg1 = (HiddenField)ddl.Parent.FindControl("FilterArg1");
        HiddenField FilterArg2 = (HiddenField)ddl.Parent.FindControl("FilterArg2");
        HiddenField FilterArg3 = (HiddenField)ddl.Parent.FindControl("FilterArg3");

        
        if (FormView1.CurrentMode == FormViewMode.Edit)
        {
            switch (ls_fltr)
            {
                case "IWOR":
                    FilterArg1.Value = "IWOR";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "IWOR";

                    break;
                case "WORA":
                    FilterArg1.Value = "WORR";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORA";
                    // Logic needed to determine if PM is viewing page. Otherwise read-only.
                    break;
                case "WORD":
                    FilterArg1.Value = "WORM";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORD";

                    break;
                case "WOTS":
                    FilterArg1.Value = "WOTS";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";
                    break;
                case "WDEV":
                    FilterArg1.Value = "WDEV";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                case "WTST":
                    FilterArg1.Value = "WTST";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                case "GONG":
                    FilterArg1.Value = "GONG";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                case "IMPL":
                    FilterArg1.Value = "IMPL";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                case "WORM":
                    FilterArg1.Value = "WORM";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                case "WORR":
                    FilterArg1.Value = "WORR";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
                default:
                    FilterArg1.Value = "IWOR";
                    FilterArg2.Value = "SUSP";
                    FilterArg3.Value = "WORM";

                    break;
            }
        }
        
    }

    
    protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        // If there are no items changed then there should be no update performed.
        bool lb_update = false;

        //DataView dv = (DataView)ObjectDataSource1.Select();
        //DataRow dr = (DataRow)dv.Table.Rows[0];
        FormView fv = sender as FormView;

        DateTimePicker DTimePicker = (DateTimePicker)fv.Row.FindControl("CalendarExtender1");
        string ls_val = DTimePicker.Value.ToString();

        //TextBox TxtBxReqDate = (TextBox)fv.Row.FindControl("word_reqDueDateTextBox");
        //TxtBxReqDate.Text = ls_val;
        ls_val = Convert.ToDateTime(ls_val).ToString(); //.ToShortDateString();

        e.NewValues["word_reqDueDate"] = ls_val;
        e.NewValues["word_Doc"] = "";
        e.NewValues["word_DocVer"] = "";

        for (int i = 0; i < Convert.ToInt32((e.OldValues.Count)-1); i++)
        {
            if (!e.OldValues[i].Equals(null))
            {
                if (!e.NewValues[i].ToString().Equals(e.OldValues[i].ToString()))
                {
                    lb_update = true;
                    break;
                }
            }
            else
            {
                e.NewValues[i] = "";
            }
        }

        if (!lb_update) e.Cancel = true;

    }

    
    protected void ReturnButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/WorkOrder/Default.aspx?word_Proj=" + ls_proj);
    }

    protected void RespondButton_Click(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/Response.aspx?word_WOnum=" + li_wonum.ToString() + "&word_Proj=" + ls_proj;
        Response.Redirect(ls_response);
    }

    protected void RespondButton_Click1(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/Response.aspx?word_WOnum=" + li_wonum.ToString() + "&word_Proj=" + ls_proj
            + "&wrsp_RespNum=0";
        Response.Redirect(ls_response);
    }

    protected void UpdateCancelButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/WorkOrder/Default.aspx?word_Proj=" + ls_proj);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ddlRole.SelectedIndex > 0 && ddlParticipant.SelectedIndex > 0)
        {
            string roleVal = ddlRole.SelectedValue;
            int uid = Convert.ToInt32(ddlParticipant.SelectedValue);

            RolesBLL roleLogic = new RolesBLL();
            if (roleLogic.GetUIDcount(uid, li_wonum, ls_proj) == 0)
            {

                int ndx = roleLogic.GetMaxRolesIndex();
                if (roleLogic.InsertWOroles(li_wonum, uid, ls_proj, roleVal, false, true))
                {
                    ddlParticipant.Items.RemoveAt(ddlParticipant.SelectedIndex);
                    ddlParticipant.SelectedIndex = -1;
                    ddlRole.SelectedIndex = -1;
                    UpdatePanel1.Update();
                    GridView2.DataBind();
                }
            }
        }

    }
   

    protected void GridView2_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        bool lb_out = false;

        woApprovalBLL apprvLogic = new woApprovalBLL();
        WorkOrder.WOapprovalDataTable apprvlDT;
        foreach (ListItem partItem in ddlParticipant.Items)
        {
            if (partItem.Value.ToString() == e.Values["worl_uid"].ToString())
            {
                lb_out = true;
                break;
            }

        }

        if (!lb_out) ddlParticipant.Items.Insert(1, new ListItem(e.Values["FullName"].ToString(),e.Values["worl_uid"].ToString()));

        apprvlDT = apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);

        // this code ensures that every approval record is created
        foreach (WorkOrder.WOapprovalRow aprvlRow in apprvlDT)
        {
            if (aprvlRow["wapr_Approver"].ToString() == e.Values["worl_uid"].ToString())
            {
                int li_rtn = apprvLogic.DeleteByApprovalNum(Convert.ToInt32(aprvlRow["wapr_AprvNum"]));
                break;
            }
        }

    }


    protected void GridView2_DataBound(object sender, EventArgs e)
    {
        DataView dv = (DataView)ObjectDataSource5.Select();
        int li_val;
        int aprvlNum;
        bool lb_pass = true;
        woApprovalBLL apprvLogic = new woApprovalBLL();

        string ls_val1;
        string ls_val2;
        bool lb_rtn;
        WorkOrder.WOapprovalDataTable apprvlDT = new WorkOrder.WOapprovalDataTable(); //apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);
        
        foreach (DataRow dr in dv.Table.Rows)
        {
            if (ddlParticipant.Items.Count > 0)
            {
                ls_val2 = dr["worl_uid"].ToString();

                // this code ensures the dropdownlists at the bottom of the form are mutually exclusive of each other.
                foreach (ListItem partItem in ddlParticipant.Items)
                {
                    ls_val1 = partItem.Value.ToString();

                    if (ls_val2 == ls_val1)
                    {
                        ddlParticipant.Items.Remove(partItem);
                        break;
                    }
                }
            }

            aprvlNum = apprvLogic.GetNextApprovalNum();
            lb_pass = false;

            apprvlDT = apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);

            // this code ensures that every approval record is created
            foreach (WorkOrder.WOapprovalRow aprvlRow in apprvlDT)
            {
                if (dr["worl_uid"].ToString() == aprvlRow["wapr_Approver"].ToString()) //If approval record exists 
                {
                    lb_pass = true;
                }
            }

            if (!lb_pass) //approval record does NOT exist so insert record . . .
            {
                if ( dr["worl_aprvl"] != DBNull.Value && Convert.ToBoolean(dr["worl_aprvl"]) == true) //. . . but ONLY if AprvCode checkbox is checked
                {
                    li_val = Convert.ToInt32(dr["worl_uid"].ToString());
                    lb_rtn = apprvLogic.InsertNewWOApproval(aprvlNum, li_wonum, ls_proj, DateTime.Now,
                        li_val, false, DateTime.Now.AddYears(-100), "", false, true);
                }
            }
        }

        WorkOrderTableAdapters.WOapprovalTableAdapter woAprvlAdapter = new WorkOrderTableAdapters.WOapprovalTableAdapter();

        dv = (DataView)ObjectDataSource5.Select();
        apprvlDT.Clear();
        apprvlDT = apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);
        
        foreach (DataRow dr in dv.Table.Rows)
        {
            foreach (WorkOrder.WOapprovalRow aprvlRow1 in apprvlDT)
            {
                if (dr["worl_uid"].ToString() == aprvlRow1["wapr_Approver"].ToString())
                {
                    aprvlRow1["wapr_required"] = Convert.ToBoolean(dr["worl_aprvl"]);
                    
                    int li_rtn =  woAprvlAdapter.UpdateApprovalRecord(Convert.ToInt32(aprvlRow1.wapr_AprvNum), Convert.ToInt32(aprvlRow1.wapr_WOnum),
                        aprvlRow1.wapr_Proj.ToString(),Convert.ToDateTime(aprvlRow1.wapr_AprvDate),Convert.ToInt32(aprvlRow1.wapr_Approver),
                        Convert.ToBoolean(aprvlRow1.wapr_AprvCode), Convert.ToDateTime(aprvlRow1.wapr_AprvlDate),
                        aprvlRow1.wapr_comments.ToString(),Convert.ToBoolean(aprvlRow1.wapr_required),Convert.ToBoolean(aprvlRow1.wapr_emailFlag),
                        Convert.ToInt32(aprvlRow1.wapr_AprvNum));
                    break;
                }
            }
        }

    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //RolesBLL roleLogic = new RolesBLL();
        //foreach (GridViewRow row in GridView2.Rows)
        //{
        //    if (row.RowState == DataControlRowState.Normal)
        //    {
        //        bool lb_aprv = false;
        //        bool lb_email = false;
        //        // Access the CheckBox
        //        CheckBox cbAprv = (CheckBox)row.FindControl("aprvChkbx");//("aprvChkbx");
        //        CheckBox cbEmail = (CheckBox)row.FindControl("emailChkbx"); //("aprvChkbx");
        //        if (cbAprv != null && cbAprv.Checked) lb_aprv = true;
        //        if (cbEmail != null && cbEmail.Checked) lb_email = true;


        //        Image imgAprv = (Image)row.FindControl("ImageAprv");
        //        Image imgEmail = (Image)row.FindControl("imgEmail");

        //        if (lb_aprv)
        //        {
        //            imgAprv.ImageUrl = "~/Images/checked.bmp";
        //        }
        //        else
        //        {
        //            imgAprv.ImageUrl = "~/Images/unchecked.bmp";
        //        }
        //        if (lb_email)
        //        {
        //            imgEmail.ImageUrl = "~/Images/checked.bmp";
        //        }
        //        else
        //        {
        //            imgEmail.ImageUrl = "~/Images/unchecked.bmp";
        //        }
        //    }
        //}
    }
    //http://blog.evonet.com.au/post/dropdownlist1-has-a-SelectedValue-which-is-invalid-because-it-does-not-exist-in-the-list-of-items.aspx
    


    protected void FormView1_PreRender(object sender, EventArgs e)
    {
        if (FormView1.CurrentMode == FormViewMode.Edit)
        {
            DataRowView rowView = (DataRowView)(FormView1.DataItem);
            if ((rowView != null))
            {
                string ls_val = rowView["word_PMorSME"].ToString();
                if (((DropDownList)FormView1.FindControl("DropDownList2")).Items.FindByValue(ls_val) != null)
                {
                    ((DropDownList)FormView1.FindControl("DropDownList2")).SelectedValue = ls_val;
                }
                else
                {
                    ((DropDownList)FormView1.FindControl("DropDownList2")).Items.Add(new ListItem(ls_val,ls_val));
                }

                ls_val = rowView["word_BusnOwner"].ToString();
                if (((DropDownList)FormView1.FindControl("DropDownList3")).Items.FindByValue(ls_val) != null)
                {
                    ((DropDownList)FormView1.FindControl("DropDownList3")).SelectedValue = ls_val;
                }

                ls_val = rowView["word_Author"].ToString();
                if (Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)) != Convert.ToInt32(ls_val))
                {
                    ((TextBox)(FormView1.FindControl("word_DescrTextBox"))).ReadOnly = true;
                    ((TextBox)(FormView1.FindControl("word_JustifyTextBox"))).ReadOnly = true;
                    ((TextBox)(FormView1.FindControl("word_CmntsTextBox"))).ReadOnly = true;
                    //lbl1.Enabled = false;
                }

                    
            }
        }
    } 
 

    protected void ObjectDataSource1_Updating(object sender, ObjectDataSourceMethodEventArgs e)
    {
        //e.InputParameters["word_Doc"] = "";
        //e.InputParameters["word_DocVer"] = "";
        try
        {
            WorkOrderBLL woLogic = new WorkOrderBLL();
            bool lb_rtn = woLogic.UpdateWO(Convert.ToInt32(e.InputParameters["Original_word_WOnum"]),e.InputParameters["word_Proj"].ToString(),Convert.ToInt32(e.InputParameters["word_StatNum"]),e.InputParameters["word_Status"].ToString(),
                Convert.ToInt32(e.InputParameters["word_StatFlag"]),Convert.ToDateTime(e.InputParameters["word_Date"]),e.InputParameters["word_Author"].ToString(),e.InputParameters["word_Title"].ToString(),
                Convert.ToInt32(e.InputParameters["word_Priority"]),e.InputParameters["word_Descr"].ToString(),e.InputParameters["word_Justify"].ToString(),e.InputParameters["word_Cmnts"].ToString(),
                Convert.ToInt32(e.InputParameters["word_PMorSME"]),Convert.ToInt32(e.InputParameters["word_BusnOwner"]),Convert.ToDateTime(e.InputParameters["word_reqDueDate"]));
           /// ObjectDataSource1.Update();
        }
        catch (Exception ex)
        {
            string ls_val = ex.Message.ToString();
        }
        

    }

    protected void Approvals_Click(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/Approvals.aspx?word_WOnum=" + li_wonum.ToString() + "&word_Proj=" + ls_proj;
        Response.Redirect(ls_response);
    }


    protected void FormView2_PageIndexChanging(object sender, FormViewPageEventArgs e)
    {
        UpdatePanel2.Update();
    }

    protected void lnkBtnRespond2_Click(object sender, EventArgs e)
    {
        DataView dv = (DataView)ObjectDataSource8.Select();
        DataRow dr = dv.Table.Rows[0];

        li_respNum = Convert.ToInt32(dr["wrsp_RespNum"]);
           
        string ls_response = "~/WorkOrder/Response.aspx?word_WOnum=" + li_wonum.ToString() + 
            "&word_Proj=" + ls_proj + "&wrsp_RespNum=" + li_respNum.ToString();
        Response.Redirect(ls_response);
    }


    protected void FormView2_DataBound(object sender, EventArgs e)
    {
        if (FormView2.DataItemCount > 0)
        {
            
            int li_r2r;
            DataRowView rowView = (DataRowView)FormView2.DataItem;
            string ls_val = rowView["wrsp_uid"].ToString();
            li_respNum = Convert.ToInt32(rowView["wrsp_RespNum"]);
            if (rowView["wrsp_RespToResp"] != DBNull.Value)
            {
                li_r2r = Convert.ToInt32(rowView["wrsp_RespToResp"].ToString());
            }
            else
            {
                li_r2r = 0;
            }
            woUsersBLL userLogic = new woUsersBLL();
            ls_val = userLogic.GetFullNameByID(Convert.ToInt32(ls_val));
            ((Label)FormView2.FindControl("wrsp_uidLabel")).Text = ls_val;

            TextBox txtBx = (TextBox)(FormView2.FindControl("txtBxHeader"));
            
            if (li_r2r != 0)
            {
                txtBx.Text = "Response to Work Order Response # " +
                    li_r2r.ToString();
                txtBx.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                txtBx.Text = "Response to Work Order # " +
                    Request.QueryString["word_WOnum"].ToString();
            }
        }
    }
    
    protected void ObjectDataSource1_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        string ls_val = e.OutputParameters[0].ToString();
    }

    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        
        //RolesBLL roleLogic = new RolesBLL();
        //CodesBLL codeLogic = new CodesBLL();
        //GridViewRow row = GridView2.Rows[e.RowIndex];
        ////foreach (GridViewRow row in GridView2.Rows)
        ////{
        //    bool lb_aprv = false;
        //    bool lb_email = false;
        //    // Access the CheckBox
        //    CheckBox cbAprv = (CheckBox)row.FindControl("checkbox");//("aprvChkbx");
        //    CheckBox cbEmail = (CheckBox)row.FindControl("checkbox1"); //("aprvChkbx");
        //    if (cbAprv != null && cbAprv.Checked) lb_aprv = true;
        //    if (cbEmail != null && cbEmail.Checked) lb_email = true;

        //    Image imgAprv = (Image)row.FindControl("imgAprv");
        //    Image imgEmail = (Image)row.FindControl("imgEmail");

        //    string ls_role = codeLogic.GetRoleIDbyDesc(e.OldValues["RoleTxt"].ToString());    

        //    bool lb_rtn = roleLogic.UpdateRoles(li_wonum, ls_proj, Convert.ToInt32(e.OldValues["worl_uid"]),
        //        ls_role, lb_aprv, Convert.ToInt32(e.Keys["worl_index"]), lb_email);

        //    //e.Cancel = true;


        //}
    }

    protected void GridView2_PreRender(object sender, EventArgs e)
    {
        
    }
    protected void GridView2_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        //RolesBLL roleLogic = new RolesBLL();
        //foreach (GridViewRow row in GridView2.Rows)
        //{

        //    bool lb_aprv = false;
        //    bool lb_email = false;
        //    // Access the CheckBox
        //    CheckBox cbAprv = (CheckBox)row.FindControl("aprvChkbx");//("aprvChkbx");
        //    CheckBox cbEmail = (CheckBox)row.FindControl("emailChkbx"); //("aprvChkbx");
        //    if (cbAprv != null && cbAprv.Checked) lb_aprv = true;
        //    if (cbEmail != null && cbEmail.Checked) lb_email = true;


        //    Image imgAprv = (Image)row.FindControl("imgAprv");
        //    Image imgEmail = (Image)row.FindControl("imgEmail");

        //    if(lb_aprv)
        //    {
        //        imgAprv.ImageUrl = "~/Images/checked.bmp";
        //    }
        //    else
        //    {
        //        imgAprv.ImageUrl = "~/Images/unchecked.bmp";
        //    }
        //    if(lb_email)
        //    {
        //        imgEmail.ImageUrl = "~/Images/checked.bmp";
        //    }
        //    else
        //    {
        //        imgEmail.ImageUrl = "~/Images/unchecked.bmp";
        //    }
        //}
        //GridView2.DataBind();
    }
    
}

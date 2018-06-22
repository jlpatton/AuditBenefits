using System;
using System.Data;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using KMobile.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page 
   
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
                      
        LblWarning.Visible = false;
        
    }
    protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        woPhaseBLL woPhaseLogic = new woPhaseBLL();
        int PhaseNum = woPhaseLogic.GetNewPhaseNumber();
        
        String errMsg = "";
        string uname = User.Identity.Name;
        LblWarning.Text = "";

        e.Values["word_Date"] = DateTime.Today.Date.ToShortDateString();

        if (e.Values["word_Title"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg = "TITLE, ";
            //LblWarning.Text = errMsg;
        }
        if (e.Values["word_Proj"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg += "PROJECT, ";
            //LblWarning.Text = errMsg;
        }
        if (e.Values["word_Priority"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg += "PRIORITY, ";
        }

        FormView fv = sender as FormView;

        DateTimePicker DTimePicker = (DateTimePicker)fv.Row.FindControl("CalendarExtender1");
        string ls_val = DTimePicker.Value.ToString();
        e.Values["word_reqDueDate"] = ls_val;
        if (e.Values["word_reqDueDate"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg += "DUE DATE, ";
        }

        e.Values["word_Status"] = "IWOR";

        e.Values["word_StatNum"] = PhaseNum;

        e.Values["word_Author"] = User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1);
        

        if (e.Values["word_PMorSME"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg += "PM/SME, ";
        }
        if (e.Values["word_BusnOwner"].Equals(""))
        {
            e.Cancel = true;
            LblWarning.Visible = true;
            errMsg += "BUSINESS OWNER";
        }

        //e.Values[10] = PhaseNum;

        if (!errMsg.Equals(""))
        {

            if (errMsg.Substring(errMsg.Length - 2, 1) == ",")
            {
                errMsg = errMsg.Substring(0, errMsg.Length - 2);
            }
            errMsg = "You must enter the following: " + errMsg + " for this work order.";
            LblWarning.Text = errMsg;

        }
        
    }

    protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        int wo = (int)(e.NewValues["word_WOnum"]);
        

        if (e.Exception != null)
        {
            //Display a userfriendly message

            LblWarning.Visible = true;
            LblWarning.Text = "There was a problem creating the Work Order. ";
            if (e.Exception.InnerException != null)
            {
                Exception inner = e.Exception.InnerException;

                if (inner is System.Data.Common.DbException)
                {
                    LblWarning.Text += "Our database is currently experiencing problems. Please try again later.";
                }
                //else if (inner is NoNullAllowedException)
                //{
                //    LblWarning.Text += "There are one or more required fields that are missing.";
                //}
                else if (inner is ArgumentException)
                {
                    string paramName = ((ArgumentException)inner).ParamName;
                    LblWarning.Text += string.Concat("The ", paramName, " value is illegal.");
                }
                else if (inner is ApplicationException)
                {
                    LblWarning.Text += inner.Message;
                }

                // Indicate that the exception has been handled
                e.ExceptionHandled = true;
                // Keep the row in edit mode
                e.KeepInEditMode = true;
                

            }
        }
        
    }



    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        int rws = e.AffectedRows;

        woPhaseBLL woPhaseLogic = new woPhaseBLL();
        WorkOrderBLL woLogic = new WorkOrderBLL();

        int wonum = (woLogic.GetMaxWOnum(e.Values[3].ToString()) - 1);
        bool lb_rtn;
        //SmtpEmail mailMessage = new SmtpEmail();
        //bool lb_rtn = mailMessage.Send(e.Values["word_Author"].ToString(), e.Values["word_PMorSME"].ToString(), "Work Order Creation - " + e.Values[3].ToString()
        //    + " WO# " + wonum + " - " + e.Values["word_Title"].ToString(),
        //    "WO# " + wonum + " - " + e.Values["word_Title"].ToString() + " requires your attention." +
        //    " Log into the EBS Work Order Module for details. Please do not respond to this email.", wonum, 0, e.Values["word_Proj"].ToString());

        // Insert a WOphase record, update the WOrole table and update the WorkOrder table record as well.

        WorkOrder.WorkOrderDataTable WrkOrds = woLogic.GetWorkOrdersByWOnumProj(wonum, e.Values["word_Proj"].ToString());
        WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

        int PhaseNum = woPhaseLogic.InsertPhaseRecord(wonum, WrkOrd.word_Proj, 0, DateTime.Now,
            "IWOR", 0, Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));

        RolesBLL roleLogic = new RolesBLL();

        lb_rtn = roleLogic.InsertWOroles(wonum, Convert.ToInt32(WrkOrd.word_Author), WrkOrd.word_Proj, "AUTH",false,true);
        lb_rtn = roleLogic.InsertWOroles(wonum, Convert.ToInt32(WrkOrd.word_BusnOwner), WrkOrd.word_Proj, "BOWN",false,true);
        lb_rtn = roleLogic.InsertWOroles(wonum, Convert.ToInt32(WrkOrd.word_PMorSME), WrkOrd.word_Proj, "PM",false,true);


        DataView dv = (DataView)ObjectDataSource1.Select();
        DataRow dr = (DataRow)dv.Table.Rows[0];
        FormView fv = sender as FormView;
        
        if (fv.CurrentMode == FormViewMode.Insert)
        {
            DropDownList DropDownListPhaseStat = (DropDownList)fv.Row.FindControl("DropDownListPhaseStat");
            int PhaseStat = Convert.ToInt32(DropDownListPhaseStat.SelectedValue);

            if (PhaseStat == 1)
            {
                int PhaseNumb = woPhaseLogic.InsertPhaseRecord(wonum, WrkOrd.word_Proj, 0, DateTime.Now,
                    "WORA", 0, Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));

                WrkOrd.word_Status = "WORA";
                WrkOrd.word_statFlag = 0;
                WrkOrd.word_StatNum = PhaseNumb;

                //DateTimePicker DTimePicker = (DateTimePicker)fv.Row.FindControl("CalendarExtender1");
                //string ls_val = DTimePicker.Value.ToString();

                //TextBox TxtBxReqDate = (TextBox)fv.Row.FindControl("word_reqDueDateTextBox");
                //TxtBxReqDate.Text = ls_val;

                //WrkOrd.word_reqDueDate = Convert.ToDateTime(DTimePicker.Value.ToString());

                lb_rtn = woLogic.UpdateWorkOrder(WrkOrd.word_WOnum, WrkOrd.word_Proj, WrkOrd.word_StatNum,
                    WrkOrd.word_Status, WrkOrd.word_statFlag, WrkOrd.word_Date, WrkOrd.word_Author,
                    WrkOrd.word_Title, WrkOrd.word_Doc.ToString(), WrkOrd.word_DocVer.ToString(), WrkOrd.word_Priority,
                    WrkOrd.word_Descr, WrkOrd.word_Justify, WrkOrd.word_Cmnts, WrkOrd.word_PMorSME,
                    WrkOrd.word_BusnOwner, WrkOrd.word_reqDueDate, WrkOrd.word_WOnum, WrkOrd.word_Proj);
            }


        }

        Response.Redirect("~/WorkOrder/Default.aspx?word_Proj=" + WrkOrd.word_Proj.ToString());
        
    }

    protected void FormView1_PreRender(object sender, EventArgs e)
    {
        DataView dv = (DataView)ObjectDataSource1.Select();
        if (dv.Count > 0)
        {
            DataRow dr = (DataRow)dv.Table.Rows[0];
            FormView fv = sender as FormView;

            woUsersBLL userLogic = new woUsersBLL();
            string ls_usr = userLogic.GetFullNameByID(Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));

            if (fv.CurrentMode == FormViewMode.Insert)
            {
                string usr = User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1);

                Label LblAuthor = (Label)fv.Row.FindControl("LblAuthor");
                LblAuthor.Text = ls_usr;
                LblAuthor.Enabled = false;

                Label LblPhase = (Label)fv.Row.FindControl("LblPhase");
                LblPhase.Text = "Initial Work Order Request";
                LblPhase.Enabled = false;

                //DropDownList DropDownListPhaseStat = (DropDownList)fv.Row.FindControl("DropDownListPhaseStat");

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
                        ((DropDownList)FormView1.FindControl("DropDownList2")).Items.Add(new ListItem(ls_val, ls_val));
                    }

                    ls_val = rowView["word_BusnOwner"].ToString();
                    if (((DropDownList)FormView1.FindControl("DropDownList3")).Items.FindByValue(ls_val) != null)
                    {
                        ((DropDownList)FormView1.FindControl("DropDownList3")).SelectedValue = ls_val;
                    }
                }

            }
        }

        
    }
    protected void ObjectDataSource1_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //if (e.AffectedRows <= 0)
        //{
        //    LblWarning.Text = "There was a problem updating this page. Please contact EBS for support.";
        //    return;
        //}
    }

    protected void ObjectDataSource1_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        if (e.InputParameters["word_Status"].ToString() == "IWOR") e.InputParameters["word_statFlag"] = 0;
    }

    
    
}

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


public partial class Approvals : System.Web.UI.Page
{
    int aprvNum;
    int li_wonum;
    string ls_proj;
    string lblText;

    protected void Page_Load(object sender, EventArgs e)
    {
        ls_proj = Request.QueryString["word_Proj"];
        li_wonum = Convert.ToInt32(Request.QueryString["word_WOnum"]);
    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        DataView dv = (DataView)ObjectDataSource1.Select();
        
        string ls_val1;
        //string ls_val2;

        bool lb_rtn = false;
        bool lb_rtn2 = false;
        // this code checks to see if user is an Approver . . .
        foreach (DataRow dr in dv.Table.Rows)
        {
            ls_val1 = dr["wapr_Approver"].ToString();
            if (dr["wapr_required"] != DBNull.Value)
            {
                lb_rtn = Convert.ToBoolean(dr["wapr_required"]);
            }
            else
            {
                lb_rtn = false;
            }

            if (dr["wapr_AprvCode"] != DBNull.Value)
            {
                lb_rtn2 = Convert.ToBoolean(dr["wapr_AprvCode"]);
            }
            else
            {
                lb_rtn2 = false;
            }

            lnkBtnReturn.Visible = true;
            aprvNum = Convert.ToInt32(dr["wapr_AprvNum"]);
            if ((User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1).ToString() == ls_val1))
            {
                if (lb_rtn && !lb_rtn2)
                {
                    btnApprove.Visible = true;
                    //btnUpdate.Visible = true;
                    Label1.Visible = true;
                    //Label2.Visible = true;
                    //Label3.Visible = true;
                    //Label4.Visible = true;
                    //Label5.Visible = true;
                    //chkBxApproval.Visible = true;
                    //txtBxComments.Visible = true;
                    //txtBxUser.Visible = true;
                    //txtBxUser.Text = User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1).ToString();
                }

                break;
                
            }

        }
    }

    protected void  btnUpdate_Click(object sender, EventArgs e)
    {
        // First check their LDAP Password . . .
        ldapAuthentication ldapAuth = new ldapAuthentication();
        try 
        {
            ldapAuth.AuthenticateUser(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1).ToString(), txtBxPassword.Text);
        }
        catch (Exception ex)
        {
            lblText = Label1.Text;
            Label1.Text = ex.Message.ToString();//"Your LDAP UID and/or password did not authenticate. Please try again.";
            return;
        }
        
        lblText = Label1.Text;// = "Your LDAP UID and/or password did not authenticate. Please try again.";
        
        
        
        DataView dv = (DataView)ObjectDataSource1.Select();
        
        //string ls_val;
        //string ls_val2;
        woApprovalBLL apprvLogic = new woApprovalBLL();

        bool lb_rtn = false;

        foreach (DataRow dr in dv.Table.Rows)
        {
            if (Convert.ToInt32(dr["wapr_Approver"]) == Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)))
            {
                int li_val = Convert.ToInt32(dr["wapr_AprvNum"]);
                int li_val1 = li_wonum;
                string ls_val = ls_proj;
                DateTime ldt_val = Convert.ToDateTime(dr["wapr_AprvDate"].ToString());
                int li_val2 = Convert.ToInt32(dr["wapr_Approver"]);
                bool lb_val = chkBxApproval.Checked; //Convert.ToBoolean(dr["wapr_AprvCode"]);
                DateTime ldt_val2 = DateTime.Now;
                string ls_val2 = txtBxComments.Text;// dr["wapr_comments"].ToString();
                bool lb_val2 = Convert.ToBoolean(dr["wapr_required"]);
                bool lb_val3 = Convert.ToBoolean(dr["wapr_emailFlag"]);

                lb_rtn = apprvLogic.UpdateWOAprvlRecord(li_val, li_val1, ls_val, ldt_val, li_val2, lb_val, ldt_val2, ls_val2, lb_val2, lb_val3, li_val);
                GridView1.DataBind();

                btnUpdate.Visible = false;
                btnApprove.Visible = false;
                lnkBtnReturn.Visible = true;
                Label2.Visible = false;
                Label3.Visible = false;
                Label4.Visible = false;
                Label5.Visible = false;
                chkBxApproval.Visible = false;
                txtBxPassword.Visible = false;
                txtBxComments.Visible = false;
                txtBxUser.Visible = false;

                Label1.Text = "Update was successful.";


                

            }
        }

        WorkOrderBLL woLogic = new WorkOrderBLL();
        WorkOrder.WorkOrderDataTable WrkOrds = woLogic.GetWorkOrdersByWOnumProj(li_wonum, ls_proj);
        WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

        string host = Request.ServerVariables["SERVER_NAME"];  //HttpContext.Current.Request.Url.Host;
        SmtpEmail mailMessage = new SmtpEmail();

        string ls_to = "";
        RolesBLL WOroleLogic = new RolesBLL();
        WorkOrder.WOroleTextDisplayDataTable woRoleDT;
        woRoleDT = WOroleLogic.GetWOrolesByWOnumProj(li_wonum, ls_proj);

        foreach (WorkOrder.WOroleTextDisplayRow roleRow in woRoleDT)
        {
            if (Convert.ToBoolean(roleRow["worl_aprvl"]) == true)
            {
                ls_to = ls_to + "," + roleRow["worl_uid"].ToString();
            }
        }

        ldapClient userObject = new ldapClient();
        UserRecord ud;
        ud = userObject.SearchUser(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1));

        lb_rtn = mailMessage.Send(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1), ls_to, "GO/NoGO ACTION - " + WrkOrd.word_Proj.ToString()
            + " WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString(),
            "WO# " + li_wonum + " - " + WrkOrd.word_Title.ToString() + " has received a Go/NoGo action from " +
            ud.FirstName.ToUpper() + " " + ud.LastName.ToUpper() + "." +
            " Log into the EBS Work Order Module for details." +
            Environment.NewLine + Environment.NewLine + "http://" + host + "/WorkOrder/EditWO.aspx?word_WOnum=" +
            li_wonum.ToString() + "&word_Proj=" + WrkOrd.word_Proj.ToString() + Environment.NewLine + Environment.NewLine +
            "The comment left on this Go/NoGo action is as follows:" +
            Environment.NewLine + Environment.NewLine +
            (Convert.ToBoolean(chkBxApproval.Checked) ? "GO! " : "NO Go! ") + Environment.NewLine + Environment.NewLine +
            txtBxComments.Text + Environment.NewLine + Environment.NewLine +
            Environment.NewLine + Environment.NewLine + "PLEASE DO NOT RESPOND TO THIS EMAIL!"
            , li_wonum, 0, WrkOrd.word_Proj.ToString());

        



    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        woUsersBLL userLogic = new woUsersBLL();

        btnUpdate.Visible = true;
        Label1.Visible = true;
        Label2.Visible = true;
        Label3.Visible = true;
        Label4.Visible = true;
        Label5.Visible = true;
        chkBxApproval.Visible = true;
        txtBxPassword.Visible = true;
        txtBxComments.Visible = true;
        txtBxUser.Visible = true;
        txtBxUser.Text = userLogic.GetFullNameByID(Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));
    }
    protected void ListBox1_DataBinding(object sender, EventArgs e)
    {
        //DataView dv = (DataView)ObjectDataSource2.Select();
        //foreach (DataRow dr in dv.Table.Rows)
        //{
        //    string ls_val1 = dr["wusr_uid"].ToString();
        //}

        //ListBox lb1 = (ListBox)(GridView1.FindControl("ListBox1"));
        //DataView dv = (DataView)lb1
    }
    protected void lnkBtnReturn_Click(object sender, EventArgs e)
    {

        bool lb_complete = true;
        woApprovalBLL apprvLogic = new woApprovalBLL();
        WorkOrder.WOapprovalDataTable aprvDT = apprvLogic.GetWOApprovalDataByWOandProj(li_wonum, ls_proj);

        foreach (WorkOrder.WOapprovalRow aprvDR in aprvDT)
        {
            lb_complete = true; 
            if (Convert.ToBoolean(aprvDR["wapr_required"]) && !Convert.ToBoolean(aprvDR["wapr_AprvCode"]))
            {
                lb_complete = false;
                break;
            }
        }

        if (lb_complete)
        {
            woPhaseBLL woPhaseLogic = new woPhaseBLL();

            int PhaseNum = woPhaseLogic.InsertPhaseRecord(li_wonum,
            ls_proj, 0, DateTime.Now, "GONG", 1,
            Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));

            if (PhaseNum > 0)
            {
                PhaseNum = woPhaseLogic.InsertPhaseRecord(li_wonum,
                ls_proj, 0, DateTime.Now, "IMPL", 0,
                Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)));


                WorkOrderBLL woLogic = new WorkOrderBLL();
                WorkOrder.WorkOrderDataTable WrkOrds = woLogic.GetWorkOrdersByWOnumProj(li_wonum, ls_proj);
                WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

                bool lb_rtn;

                WrkOrd.word_Status = "IMPL";
                WrkOrd.word_statFlag = 0;
                WrkOrd.word_StatNum = PhaseNum;

                lb_rtn = woLogic.UpdateWorkOrder(WrkOrd.word_WOnum, WrkOrd.word_Proj, WrkOrd.word_StatNum,
                    WrkOrd.word_Status, WrkOrd.word_statFlag, WrkOrd.word_Date, WrkOrd.word_Author,
                    WrkOrd.word_Title, "", "", WrkOrd.word_Priority,
                    WrkOrd.word_Descr, WrkOrd.word_Justify, WrkOrd.word_Cmnts, WrkOrd.word_PMorSME,
                    WrkOrd.word_BusnOwner, WrkOrd.word_reqDueDate, WrkOrd.word_WOnum, WrkOrd.word_Proj);

            }
        }
        
        string ls_response = "~/WorkOrder/EditWO.aspx?word_WOnum=" + li_wonum.ToString() + "&word_Proj=" + ls_proj;
        Response.Redirect(ls_response);
    }
}


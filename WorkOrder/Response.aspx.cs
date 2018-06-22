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

public partial class Response : System.Web.UI.Page
{
    string ls_num;
    int li_wonum;
    string ls_proj;

    protected void Page_Load(object sender, EventArgs e)
    {
        string ls_num = Request.QueryString["word_WOnum"];
        int li_wonum = Convert.ToInt32(ls_num);
        string ls_proj = Request.QueryString["word_Proj"];
        
        
    }
    protected void FormView2_Load(object sender, EventArgs e)
    {
        //woResponseBLL respLogic = new woResponseBLL();
        //WorkOrderTableAdapters.WOresponseTableAdapter WOrespAdapter = new WorkOrderTableAdapters.WOresponseTableAdapter();
        //WorkOrder.WOresponseDataTable Responses = WOrespAdapter.GetMaxWOresponseByWOnum(Convert.ToString(Request.QueryString["word_Proj"]), Convert.ToInt32(Request.QueryString["word_WOnum"]));
        
        if (!IsPostBack)
        {
            FormView2.ChangeMode(FormViewMode.Insert);
            
            //string ls_resp2resp = Request.QueryString["wrsp_RespNum"];
            //if (ls_resp2resp != null)
            //{
            //    TextBox txtBx = ((TextBox)(FormView2.FindControl("wrsp_RespToRespTextBox")));
            //    txtBx.Text = ls_resp2resp;
            //}
        }
        //else
        //{
        //    if (Responses.Count > 0)
        //    {
        //        FormView2.ChangeMode(FormViewMode.ReadOnly);
        //        woUsersBLL userLogic = new woUsersBLL();
        //        string ls_usr = userLogic.GetFullNameByID(Convert.ToInt32(Responses[0].wrsp_uid));
        //        ((Label)(FormView2.FindControl("UIDlabel"))).Text = ls_usr;
        //    }
        //}
        //    FormView2.ChangeMode(FormViewMode.ReadOnly);
        //}
        //}
        
        FormViewMode fmode = FormView2.CurrentMode;
        //FormView2.ChangeMode(FormViewMode.Insert);
    }
    protected void FormView2_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        string ls_val = e.Values[0].ToString();
        e.Values[4] = Request.QueryString["word_WOnum"];
        e.Values[5] = Request.QueryString["word_Proj"];
        e.Values["wrsp_uid"] = (Convert.ToInt32(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)).ToString());
        if (Request.QueryString["wrsp_RespNum"].ToString() != "0") e.Values["wrsp_RespToResp"] = Request.QueryString["wrsp_RespNum"].ToString();
    }
    protected void FormView2_ItemUpdating(object sender, FormViewUpdateEventArgs e)
    {
        string ls_val = e.NewValues[0].ToString();
        e.NewValues[4] = Request.QueryString["word_WOnum"];
        e.NewValues[5] = Request.QueryString["word_Proj"];
    }
    protected void UpdateCancelButton_Click(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/EditWO.aspx?word_WOnum=" + Request.QueryString["word_WOnum"].ToString() + "&word_Proj=" + Request.QueryString["word_Proj"].ToString();
        Response.Redirect(ls_response);
    }
    protected void UpdateCancelButton_Click1(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/EditWO.aspx?word_WOnum=" + Request.QueryString["word_WOnum"].ToString() + "&word_Proj=" + Request.QueryString["word_Proj"].ToString();
        Response.Redirect(ls_response);
    }
    protected void FormView2_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        FormView2.ChangeMode(FormViewMode.ReadOnly);
    }
    protected void FormView2_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        int li_r2r;
        string ls_cmnts, ls_to, ls_title, ls_val2,ls_val3,ls_val4;

        if (FormView2.DataItemCount > 0)
        {
            DataRowView rowView = (DataRowView)FormView2.DataItem;
            string ls_val = rowView["wrsp_uid"].ToString();
            int li_respNum = Convert.ToInt32(rowView["wrsp_RespNum"]);
            li_r2r = Convert.ToInt32(rowView["wrsp_RespToResp"]);
            if (li_r2r == null) li_r2r = 0;

            WorkOrderBLL WOlogic = new WorkOrderBLL();
            WorkOrder.WorkOrderDataTable WOdt = WOlogic.GetWorkOrdersByWOnumProj(li_wonum, ls_proj);
            DataRow woRow = ((DataRow)(WOdt.Rows[0]));
            ls_title = woRow["word_Title"].ToString();
            if ((User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1)).ToString() == woRow["word_BusnOwner"].ToString())
            {
                ls_to = woRow["word_BusnOwner"].ToString();
            }
            else
            {
                ls_to = woRow["word_PMorSME"].ToString();
            }

            ls_val2 = woRow["wrsp_Results"].ToString();
            ls_val3 = woRow["wrsp_Consider"].ToString();
            ls_val4 = woRow["wrsp_Risks"].ToString();

            if (string.IsNullOrEmpty(ls_val2)) ls_val2 = "";
            if (string.IsNullOrEmpty(ls_val3)) ls_val2 = "";
            if (string.IsNullOrEmpty(ls_val4)) ls_val2 = "";

            SmtpEmail mailMessage = new SmtpEmail();
            bool lb_rtn = mailMessage.Send(User.Identity.Name.Substring(User.Identity.Name.IndexOf("\\") + 1), ls_to, "Work Order Response - " + ls_proj
                + " WO# " + ls_num + " - " + ls_title,
                "WO# " + ls_num + " - " + ls_title + " requires your attention." +
                " Log into the EBS Work Order Module for details. Please do not respond to this email."
                + Environment.NewLine + "http://www.google.com" + Environment.NewLine + Environment.NewLine +
                "The most recent comment on this work order is as follows:" + Environment.NewLine + Environment.NewLine +
                "Results:" + Environment.NewLine + Environment.NewLine + ls_val2 +
                Environment.NewLine + Environment.NewLine + "Considerations:"+ Environment.NewLine + Environment.NewLine + ls_val3
                + Environment.NewLine + Environment.NewLine + "Risks:" + Environment.NewLine + Environment.NewLine +
                ls_val4, li_wonum, li_respNum, ls_proj);


    }

        
        //WorkOrderTableAdapters.WOresponseTableAdapter WOrespAdapter = new WorkOrderTableAdapters.WOresponseTableAdapter();
        //WorkOrder.WOresponseDataTable Responses = WOrespAdapter.GetMaxWOresponseByWOnum(Convert.ToInt32(Request.QueryString["word_WOnum"]),Convert.ToString(Request.QueryString["word_Proj"]));

        //if (Responses.Count > 0)
        //{
        //    FormView2.ChangeMode(FormViewMode.ReadOnly);
        //    woUsersBLL userLogic = new woUsersBLL();
        //    string ls_usr = userLogic.GetFullNameByID(Convert.ToInt32(Responses[0].wrsp_uid));
        //    ((Label)(FormView2.FindControl("UIDlabel"))).Text = ls_usr;
        //}

        FormView2.ChangeMode(FormViewMode.ReadOnly);
    }
    protected void ObjectDataSource2_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        //FormView2.ChangeMode(FormViewMode.ReadOnly);
        //woUsersBLL userLogic = new woUsersBLL();
        //string ls_usr = userLogic.GetFullNameByID(Convert.ToInt32(e.OutputParameters["wapr_uid"]));
        //((Label)(FormView2.FindControl("wapr_uid"))).Text = ls_usr;

        
    }
    protected void lnkBtnReturn_Click(object sender, EventArgs e)
    {
        string ls_response = "~/WorkOrder/EditWO.aspx?word_WOnum=" + Request.QueryString["word_WOnum"].ToString() + "&word_Proj=" + Request.QueryString["word_Proj"].ToString();
        Response.Redirect(ls_response);
    }
    protected void ObjectDataSource2_Inserted(object sender, ObjectDataSourceStatusEventArgs e)
    {

    }
    protected void FormView2_DataBound(object sender, EventArgs e)
    {
        if (FormView2.CurrentMode == FormViewMode.ReadOnly)
        {
            if (FormView2.DataItemCount > 0)
            {
                DataRowView rowView = (DataRowView)FormView2.DataItem;
                string ls_val = rowView["wrsp_uid"].ToString();
                int li_respNum = Convert.ToInt32(rowView["wrsp_RespNum"]);
                woUsersBLL userLogic = new woUsersBLL();
                ls_val = userLogic.GetFullNameByID(Convert.ToInt32(ls_val));
                ((Label)FormView2.FindControl("UIDlabel")).Text = ls_val;
            }
        }

        if (Request.QueryString["wrsp_RespNum"].ToString() != "0")
        {
            ((TextBox)(FormView2.FindControl("txtBxHeader"))).Text = "Response to Work Order Response # " +
                Request.QueryString["wrsp_RespNum"].ToString();
        }
        else
        {
            ((TextBox)(FormView2.FindControl("txtBxHeader"))).Text = "Response to Work Order # " +
                Request.QueryString["word_WOnum"].ToString();
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using WorkOrderTableAdapters;

/// <summary>
/// Summary description for woRolesBLL
/// </summary>
/// 
[DataObject]
public class RolesBLL
{
	public RolesBLL()
	{}
	//
	// TODO: Add constructor logic here
	//
#region WOroles Table
    private WOroleTableAdapter _woRoleAdapter = null;
    protected WOroleTableAdapter RolesAdapter
    {
        get {
            if (_woRoleAdapter == null)
                _woRoleAdapter = new WOroleTableAdapter();
            return _woRoleAdapter;
        }
    }

    private WOroleTxtDsplyTableAdapter _woRoleTxtAdapter = null;
    protected WOroleTxtDsplyTableAdapter RolesTxtAdapter
    {
        get
        {
            if (_woRoleTxtAdapter == null)
                _woRoleTxtAdapter = new WOroleTxtDsplyTableAdapter();
            return _woRoleTxtAdapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOroleTextDisplayDataTable GetWOrolesByWOnumProj(int wonum, string proj)
    {

        //WorkOrder.WOroleDataTable roleTbl = new WorkOrder.WOroleDataTable();
        //WorkOrder.WOroleRow roleRow = new WorkOrder.WOroleRow();

        //roleRow.
        return RolesTxtAdapter.GetWOroleData(wonum, proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetMaxRolesIndex()
    {
        int rowsAffected = (int)RolesAdapter.MaxRoleIndexNum();
        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int IsUserPMorUp(int uid)
    {
        int rowsAffected = (int)RolesAdapter.IsUserPMorUp(uid);
        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public bool IsUserApprover(int uid, int wonum, string proj)
    {
        bool lb_rtn = Convert.ToBoolean(RolesAdapter.IsUserApprover(uid,proj,wonum));
        return lb_rtn;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertWOroles(int worl_WOnum, int worl_uid, string worl_proj, string worl_role, bool worl_aprvl, bool worl_email)
    {
        WorkOrder.WOroleDataTable RolesDT = new WorkOrder.WOroleDataTable();
        WorkOrder.WOroleRow roleRow = RolesDT.NewWOroleRow();

        int indexnum = Convert.ToInt32(RolesAdapter.MaxRoleIndexNum());
        if (indexnum == null)
        {
            indexnum = 1;
        }
        else
        {
            indexnum++;
        }

        roleRow.worl_index = indexnum;
        roleRow.worl_WOnum = worl_WOnum;
        roleRow.worl_proj = worl_proj;
        roleRow.worl_uid = worl_uid;
        roleRow.worl_role = worl_role;
        roleRow.worl_aprvl = Convert.ToBoolean(worl_aprvl);
        roleRow.worl_email = Convert.ToBoolean(worl_email);

        RolesDT.AddWOroleRow(roleRow);

        int rowsAffected = RolesAdapter.Update(RolesDT);
        

        return rowsAffected == 1;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetUIDcount(int uid, int wonum, string proj)
    {
        int rowsAffected = (int)RolesAdapter.GetUIDcountForWO(uid, wonum, proj);
        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Delete, true)]
    public int DeleteWOparticipant(int Original_worl_index)
    {
        int rowsAffected = (int)RolesAdapter.DeleteWOparticipant(Original_worl_index);
        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Update, false)]
    public bool UpdateRoles(int worl_WOnum, string worl_proj, int worl_uid, string worl_role, bool worl_aprvl, int Original_worl_index, bool worl_email)
    {
        int worl_index = Original_worl_index;
        WorkOrder.WOroleDataTable roles = RolesAdapter.GetRoleByIndexNum(Original_worl_index);
        WorkOrder.WOroleRow roleRow = roles[0];


        int rowsAffected = RolesAdapter.UpdateRoles(worl_index, roleRow.worl_WOnum, roleRow.worl_proj, roleRow.worl_uid, roleRow.worl_role, worl_aprvl, worl_email, Original_worl_index);

        return rowsAffected == 1;
    }


    #region oldWOcode
    //[DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    //public WorkOrder.WOListingDataTable GetWOListingByProj(string woproj)
    //{
    //    return WOListadapter.GetWOListingByProj(woproj);
    //}


    //[DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    //public WorkOrder.WorkOrderFullTextDataTable GetFullTextWOs(int wonum, string woproj)
    //{
    //    return WOFullTextWOAdapter.GetFullTextWObyWOnum(wonum, woproj);
    //}


    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public WorkOrder.WorkOrderDataTable GetWorkOrdersByWOnum(int wonum)
    //{
    //    return WOadapter.GetWorkOrdersByWOnum(wonum);
    //}

    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public WorkOrder.WorkOrderDataTable GetWorkOrdersByWOnumProj(int wonum, string proj)
    //{
    //    return WOadapter.GetWOByNumProj(wonum,proj);
    //}

    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public WorkOrder.WorkOrderDataTable GetFullTextWorkOrders(int wonum, string woproj)
    //{
    //    if (woproj == null) woproj = "EBA";
    //    return WOadapter.GetFullTextWorkOrderByWOnum(wonum, woproj);
    //}

    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public int GetMaxWOnum(string proj)
    //{
    //    int rowsAffected = Convert.ToInt32(WOadapter.MaxWOnum(proj));

    //    if (rowsAffected == 0)
    //        rowsAffected++;

    //    return rowsAffected;
    //}

    //[DataObjectMethodAttribute(DataObjectMethodType.Update, false)]
    //public bool UpdateNewWONum(int wonum,string projcd)
    //{
    //    WorkOrder.WOnumGenDataTable woGenNum = woNumGenAdapter.GetRowByParms(wonum,projcd);
    //    if (woGenNum.Count == 0)
    //        return false;

    //    WorkOrder.WOnumGenRow NewWOnum = woGenNum[0];
    //    NewWOnum.wngn_number = wonum;
    //    NewWOnum.wngn_proj = projcd;

    //    int rowsAffected = woNumGenAdapter.Update(NewWOnum);

    //    return rowsAffected == 1;
    //}


    //[DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    //public bool UpdateWorkOrder(int word_WOnum, string word_Proj, int? word_StatNum,
    //    string word_Status, int? word_statFlag, DateTime word_Date, string word_Author,
    //    string word_Title, string word_Doc, string word_DocVer, int? word_Priority,
    //    string word_Descr, string word_Justify, string word_Cmnts,
    //    int? word_PMorSME, int? word_BusnOwner, DateTime? word_reqDueDate)
    //{

    //    WorkOrder.WorkOrderDataTable WrkOrds = WOadapter.GetWorkOrdersByWOnum(word_WOnum);//new WorkOrder.WorkOrderDataTable();

    //    if (WrkOrds.Count == 0)
    //        return false;

    //    WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];
        
    //    // do data insertions
                
    //    WrkOrd.word_Proj = word_Proj;
    //    word_WOnum = GetMaxWOnum(word_Proj);
        
    //    if (word_StatNum == null) WrkOrd.Setword_StatNumNull();
    //    else WrkOrd.word_StatNum = word_StatNum.Value;

    //    if (word_Status == null) WrkOrd.Setword_StatusNull();
    //    else WrkOrd.word_Status = word_Status;

    //    if (word_statFlag == null) WrkOrd.word_statFlag = 0;
    //    else WrkOrd.word_statFlag = word_statFlag.Value;

    //    if (word_Date == null) WrkOrd.Setword_DateNull();
    //    else WrkOrd.word_Date = word_Date;

    //    if (word_Author == null) WrkOrd.Setword_AuthorNull();
    //    else WrkOrd.word_Author = word_Author;

    //    if (word_Title == null) WrkOrd.Setword_TitleNull();
    //    else WrkOrd.word_Title = word_Title;

    //    if (word_Doc == null) WrkOrd.Setword_DocNull();
    //    else WrkOrd.word_Doc = word_Doc;

    //    if (word_DocVer == null) WrkOrd.Setword_DocVerNull();
    //    else WrkOrd.word_DocVer = word_DocVer;

    //    if (word_Priority == null) WrkOrd.Setword_PriorityNull();
    //    else WrkOrd.word_Priority = word_Priority.Value;

    //    if (word_Descr == null) WrkOrd.Setword_DescrNull();
    //    else WrkOrd.word_Descr = word_Descr;

    //    if (word_Justify == null) WrkOrd.Setword_JustifyNull();
    //    else WrkOrd.word_Justify = word_Justify;

    //    if (word_Cmnts == null) WrkOrd.Setword_CmntsNull();
    //    else WrkOrd.word_Cmnts = word_Cmnts;

    //    if (word_PMorSME == null) WrkOrd.Setword_PMorSMENull();
    //    else WrkOrd.word_PMorSME = word_PMorSME.Value;

    //    if (word_BusnOwner == null) WrkOrd.Setword_BusnOwnerNull();
    //    else WrkOrd.word_BusnOwner = word_BusnOwner.Value;

    //    if (word_reqDueDate == null) WrkOrd.Setword_reqDueDateNull();
    //    else WrkOrd.word_reqDueDate = word_reqDueDate.Value;

    //    int rowsAffected = WOadapter.Update(WrkOrds);

    //    return rowsAffected == 1;
    //}


    //[DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    //public bool UpdateWorkOrder(WorkOrder.WorkOrderRow woRow)
    //{
    //    WorkOrder.WorkOrderDataTable WrkOrds = WOadapter.GetWorkOrdersByWOnum(woRow.word_WOnum);//new WorkOrder.WorkOrderDataTable();

    //    if (WrkOrds.Count == 0)
    //        return false;

    //    WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

    //    //WrkOrd.ItemArray = woRow.ItemArray;
    //    WrkOrd = woRow;

    //    int rowsAffected = WOadapter.Update(WrkOrds);

    //    return rowsAffected == 1;
    //}

    //[DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    //public bool InsertWorkOrder(int word_wonum, string word_proj, int? word_statnum,
    //    string word_status, int? word_statflag, DateTime? word_date, string word_author,
    //    string word_title, string word_doc, string word_docver, int? word_priority,
    //    string word_descr, string word_justify, string word_cmnts,
    //    int? word_pmorsme, int? word_busnowner, DateTime word_reqduedate)
    //{
    //    WorkOrder.WorkOrderDataTable WrkOrds = new WorkOrder.WorkOrderDataTable();
    //    WorkOrder.WorkOrderRow WrkOrd = WrkOrds.NewWorkOrderRow();

    //    // do data insertions
    //    // work out logic to get new work order number
    //    //string uname = User.Identity.Name;
        

    //    if (word_proj == null)
    //    {
    //        return false;
    //    }
    //    else
    //    {
            
    //        WrkOrd.word_Proj = word_proj;
    //        int wonum = GetMaxWOnum(word_proj);
    //        //if (wonum > 0 && wonum>word_wonum)
    //        //{
    //            WrkOrd.word_WOnum = wonum;
    //        //}
    //        //else
    //        //{
    //        //    return false;
    //        //}
    //    }


    //    //WrkOrd.word_StatNum = 9995;
    //    if (word_statnum == null) WrkOrd.Setword_StatNumNull();
    //    else WrkOrd.word_StatNum = word_statnum.Value;

    //    if (word_status == null) WrkOrd.Setword_StatusNull();
    //    else WrkOrd.word_Status = word_status;

    //    if (word_statflag == null) WrkOrd.word_statFlag = 0;
    //    else WrkOrd.word_statFlag = word_statflag.Value;

    //    WrkOrd.word_Date = DateTime.Today;

    //    if (word_author == null) WrkOrd.Setword_AuthorNull();
    //    else WrkOrd.word_Author = word_author;

    //    if (word_title == null) WrkOrd.Setword_TitleNull();
    //    else WrkOrd.word_Title = word_title;

    //    if (word_doc == null) WrkOrd.word_Doc = ""; //WrkOrd.Setword_DocNull();
    //    else WrkOrd.word_Doc = word_doc;

    //    if (word_docver == null) WrkOrd.word_DocVer = ""; //WrkOrd.Setword_DocVerNull();
    //    else WrkOrd.word_DocVer = word_docver;

    //    if (word_priority == null) WrkOrd.Setword_PriorityNull();
    //    else WrkOrd.word_Priority = word_priority.Value;

    //    if (word_descr == null) WrkOrd.word_Descr = ""; //WrkOrd.Setword_DescrNull();
    //    else WrkOrd.word_Descr = word_descr;

    //    if (word_justify == null) WrkOrd.word_Justify = ""; //WrkOrd.Setword_JustifyNull();
    //    else WrkOrd.word_Justify = word_justify;

    //    if (word_cmnts == null) WrkOrd.word_Cmnts = ""; //WrkOrd.Setword_CmntsNull();
    //    else WrkOrd.word_Cmnts = word_cmnts;

    //    if (word_pmorsme == null) WrkOrd.Setword_PMorSMENull();
    //    else WrkOrd.word_PMorSME = word_pmorsme.Value;

    //    if (word_busnowner == null) WrkOrd.Setword_BusnOwnerNull();
    //    else WrkOrd.word_BusnOwner = word_busnowner.Value;

    //    if (word_reqduedate == null || word_reqduedate.Year < 2000) WrkOrd.Setword_reqDueDateNull();
    //    else WrkOrd.word_reqDueDate = word_reqduedate;

    //    WrkOrds.AddWorkOrderRow(WrkOrd);
    //    int rowsAffected = WOadapter.Update(WrkOrds);

    //    return rowsAffected == 1;
    //}
    #endregion


#endregion
}




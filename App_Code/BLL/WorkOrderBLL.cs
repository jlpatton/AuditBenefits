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
/// Summary description for WorkOrderBLL
/// </summary>
/// 
[DataObject]
public class WorkOrderBLL
{
	public WorkOrderBLL()
	{}
	//
	// TODO: Add constructor logic here
	//
#region WorkOrder Table
    private WorkOrderTableAdapter _workorderAdapter = null;
    protected WorkOrderTableAdapter WOadapter
    {
        get {
            if (_workorderAdapter == null)
                _workorderAdapter = new WorkOrderTableAdapter();
            return _workorderAdapter;
        }
    }

    private WOemailTableAdapter _emailAdapter = null;
    protected WOemailTableAdapter EmailAdapter
    {
        get
        {
            if (_emailAdapter == null)
                _emailAdapter = new WOemailTableAdapter();
            return _emailAdapter;
        }
    }

    private WOnumGenTableAdapter _wonumGenAdapter = null;
    protected WOnumGenTableAdapter woNumGenAdapter
    {
        get{
            if (_wonumGenAdapter == null)
                _wonumGenAdapter = new WOnumGenTableAdapter();
            return _wonumGenAdapter;
        }
    }

    private WOListingTableAdapter _woListingAdapter = null;
    protected WOListingTableAdapter WOListadapter
    {
        get {
            if (_woListingAdapter == null)
                _woListingAdapter = new WOListingTableAdapter();
            return _woListingAdapter;
        }
    }

    private WorkOrderFullTextTableAdapter _woFullTextWOAdapter = null;
    protected WorkOrderFullTextTableAdapter WOFullTextWOAdapter
    {
        get
        {
            if (_woFullTextWOAdapter == null)
                _woFullTextWOAdapter = new WorkOrderFullTextTableAdapter();
            return _woFullTextWOAdapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetMaxEmailNum(int wonum, string proj)
    {
        int rowsAffected = Convert.ToInt32(EmailAdapter.MaxWOemialNum(wonum,proj));

        if (rowsAffected == 0)
            rowsAffected++;

        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertEmailRecord(int weml_emailnum, int weml_wonum,int weml_respnum, string weml_proj,
        string weml_from, string weml_to,string weml_cc,string weml_subj, string weml_body, 
        DateTime? weml_datetime)
    {
        int rowsAffected = EmailAdapter.InsertEmailRecord(weml_emailnum, weml_wonum, weml_respnum, weml_proj,
        weml_from, weml_to, weml_cc, weml_subj, weml_body, weml_datetime);

        return rowsAffected == 1;
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WorkOrderDataTable GetWorkOrders()
    {
        return WOadapter.GetWorkOrders();
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOListingDataTable GetWOListing()
    {
        return WOListadapter.GetWOListing();
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOListingDataTable GetWOListingByProj(string proj)
    {
        return WOListadapter.GetWOListingByProj(proj);
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WorkOrderFullTextDataTable GetFullTextWOs(int wonum, string woproj)
    {
        return WOFullTextWOAdapter.GetFullTextWObyWOnum(wonum, woproj);
    }


    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public WorkOrder.WorkOrderDataTable GetWorkOrdersByWOnum(int wonum)
    //{
    //    return WOadapter.GetWorkOrdersByWOnum(wonum);
    //}

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WorkOrderDataTable GetWorkOrdersByWOnumProj(int wonum, string proj)
    {
        return WOadapter.GetWOByNumProj(wonum,proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WorkOrderDataTable GetFullTextWorkOrders(int wonum, string woproj)
    {
        if (woproj == null) woproj = "EBA";
        return WOadapter.GetFullTextWorkOrderByWOnum(wonum, woproj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetMaxWOnum(string proj)
    {
        int rowsAffected = Convert.ToInt32(WOadapter.MaxWOnum(proj));

        if (rowsAffected == 0)
            rowsAffected++;

        return rowsAffected;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Update, false)]
    public bool UpdateNewWONum(int wonum,string projcd)
    {
        WorkOrder.WOnumGenDataTable woGenNum = woNumGenAdapter.GetRowByParms(wonum,projcd);
        if (woGenNum.Count == 0)
            return false;

        WorkOrder.WOnumGenRow NewWOnum = woGenNum[0];
        NewWOnum.wngn_number = wonum;
        NewWOnum.wngn_proj = projcd;

        int rowsAffected = woNumGenAdapter.Update(NewWOnum);

        return rowsAffected == 1;
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    public bool UpdateWorkOrder(int word_WOnum, string word_Proj, int? word_StatNum,
        string word_Status, int? word_statFlag, DateTime word_Date, string word_Author,
        string word_Title, string word_Doc, string word_DocVer, int? word_Priority,
        string word_Descr, string word_Justify, string word_Cmnts,
        int? word_PMorSME, int? word_BusnOwner, DateTime? word_reqDueDate,int Original_word_WOnum, string Original_word_Proj)
    {

        WorkOrder.WorkOrderDataTable WrkOrds = WOadapter.GetWOByNumProj(word_WOnum,word_Proj);//new WorkOrder.WorkOrderDataTable();

        if (WrkOrds.Count == 0)
            return false;

        WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];
        
        // do data insertions
                
        WrkOrd.word_Proj = word_Proj;
        word_WOnum = GetMaxWOnum(word_Proj);
        
        if (word_StatNum == null) WrkOrd.Setword_StatNumNull();
        else WrkOrd.word_StatNum = word_StatNum.Value;

        if (word_Status == null) WrkOrd.Setword_StatusNull();
        else WrkOrd.word_Status = word_Status;

        if (word_statFlag == null) WrkOrd.word_statFlag = 0;
        else WrkOrd.word_statFlag = word_statFlag.Value;

        if (word_Date == null) WrkOrd.Setword_DateNull();
        else WrkOrd.word_Date = word_Date;

        if (word_Author == null) WrkOrd.Setword_AuthorNull();
        else WrkOrd.word_Author = word_Author;

        if (word_Title == null) WrkOrd.Setword_TitleNull();
        else WrkOrd.word_Title = word_Title;

        if (word_Doc == null) WrkOrd.Setword_DocNull();
        else WrkOrd.word_Doc = word_Doc;

        if (word_DocVer == null) WrkOrd.Setword_DocVerNull();
        else WrkOrd.word_DocVer = word_DocVer;

        if (word_Priority == null) WrkOrd.Setword_PriorityNull();
        else WrkOrd.word_Priority = word_Priority.Value;

        if (word_Descr == null) WrkOrd.Setword_DescrNull();
        else WrkOrd.word_Descr = word_Descr;

        if (word_Justify == null) WrkOrd.Setword_JustifyNull();
        else WrkOrd.word_Justify = word_Justify;

        if (word_Cmnts == null) WrkOrd.Setword_CmntsNull();
        else WrkOrd.word_Cmnts = word_Cmnts;

        if (word_PMorSME == null) WrkOrd.Setword_PMorSMENull();
        else WrkOrd.word_PMorSME = word_PMorSME.Value;

        if (word_BusnOwner == null) WrkOrd.Setword_BusnOwnerNull();
        else WrkOrd.word_BusnOwner = word_BusnOwner.Value;

        if (word_reqDueDate == null) WrkOrd.Setword_reqDueDateNull();
        else WrkOrd.word_reqDueDate = word_reqDueDate.Value;

        int rowsAffected = WOadapter.Update(WrkOrds);

        //int rowsAffected = WOadapter.Update(WrkOrd.word_WOnum, WrkOrd.word_Proj, WrkOrd.word_StatNum,
        //WrkOrd.word_Status, WrkOrd.word_statFlag, WrkOrd.word_Date, WrkOrd.word_Author,
        //WrkOrd.word_Title, WrkOrd.word_Doc, WrkOrd.word_DocVer, WrkOrd.word_Priority,
        //WrkOrd.word_Descr, WrkOrd.word_Justify, WrkOrd.word_Cmnts,
        //WrkOrd.word_PMorSME, WrkOrd.word_BusnOwner, WrkOrd.word_reqDueDate, WrkOrd.word_WOnum, WrkOrd.word_Proj);

        return rowsAffected == 1;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    public bool UpdateWO(int word_WOnum, string word_Proj, int? word_StatNum, string word_Status,
        int? word_statFlag, DateTime word_Date, string word_Author, string word_Title, int? word_Priority,
        string word_Descr, string word_Justify, string word_Cmnts, int? word_PMorSME,
        int? word_BusnOwner, DateTime? word_reqDueDate)
    {
        int rowsAffected = WOadapter.Update(word_WOnum, word_Proj, word_StatNum,
        word_Status, word_statFlag, word_Date, word_Author,
        word_Title, word_Priority, word_Descr, word_Justify, word_Cmnts,
        word_PMorSME, word_BusnOwner, word_reqDueDate, word_WOnum, word_Proj);

        return rowsAffected == 1;
    }


    //[DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    //public bool UpdateWorkOrder(WorkOrder.WorkOrderRow woRow)
    //{
    //    WorkOrder.WorkOrderDataTable WrkOrds = WOadapter.GetWOByNumProj(woRow.word_WOnum,woRow.word_Proj);//new WorkOrder.WorkOrderDataTable();

    //    if (WrkOrds.Count == 0)
    //        return false;

    //    WorkOrder.WorkOrderRow WrkOrd = WrkOrds[0];

    //    //WrkOrd.ItemArray = woRow.ItemArray;
    //    WrkOrd = woRow;

    //    int rowsAffected = WOadapter.Update(WrkOrds);

    //    return rowsAffected == 1;
    //}

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertWorkOrder(int word_wonum, string word_proj, int? word_statnum,
        string word_status, int? word_statflag, DateTime? word_date, string word_author,
        string word_title, string word_doc, string word_docver, int? word_priority,
        string word_descr, string word_justify, string word_cmnts,
        int? word_pmorsme, int? word_busnowner, DateTime word_reqduedate)
    {
        WorkOrder.WorkOrderDataTable WrkOrds = new WorkOrder.WorkOrderDataTable();
        WorkOrder.WorkOrderRow WrkOrd = WrkOrds.NewWorkOrderRow();

        // do data insertions
        // work out logic to get new work order number
        //string uname = User.Identity.Name;
        

        if (word_proj == null)
        {
            return false;
        }
        else
        {
            
            WrkOrd.word_Proj = word_proj;
            int wonum = GetMaxWOnum(word_proj);
            //if (wonum > 0 && wonum>word_wonum)
            //{
                WrkOrd.word_WOnum = wonum;
            //}
            //else
            //{
            //    return false;
            //}
        }


        //WrkOrd.word_StatNum = 9995;
        if (word_statnum == null) WrkOrd.Setword_StatNumNull();
        else WrkOrd.word_StatNum = word_statnum.Value;

        if (word_status == null) WrkOrd.Setword_StatusNull();
        else WrkOrd.word_Status = word_status;

        if (word_statflag == null) WrkOrd.word_statFlag = 0;
        else WrkOrd.word_statFlag = word_statflag.Value;

        WrkOrd.word_Date = DateTime.Today;

        if (word_author == null) WrkOrd.Setword_AuthorNull();
        else WrkOrd.word_Author = word_author;

        if (word_title == null) WrkOrd.Setword_TitleNull();
        else WrkOrd.word_Title = word_title;

        if (word_doc == null) WrkOrd.word_Doc = ""; //WrkOrd.Setword_DocNull();
        else WrkOrd.word_Doc = word_doc;

        if (word_docver == null) WrkOrd.word_DocVer = ""; //WrkOrd.Setword_DocVerNull();
        else WrkOrd.word_DocVer = word_docver;

        if (word_priority == null) WrkOrd.Setword_PriorityNull();
        else WrkOrd.word_Priority = word_priority.Value;

        if (word_descr == null) WrkOrd.word_Descr = ""; //WrkOrd.Setword_DescrNull();
        else WrkOrd.word_Descr = word_descr;

        if (word_justify == null) WrkOrd.word_Justify = ""; //WrkOrd.Setword_JustifyNull();
        else WrkOrd.word_Justify = word_justify;

        if (word_cmnts == null) WrkOrd.word_Cmnts = ""; //WrkOrd.Setword_CmntsNull();
        else WrkOrd.word_Cmnts = word_cmnts;

        if (word_pmorsme == null) WrkOrd.Setword_PMorSMENull();
        else WrkOrd.word_PMorSME = word_pmorsme.Value;

        if (word_busnowner == null) WrkOrd.Setword_BusnOwnerNull();
        else WrkOrd.word_BusnOwner = word_busnowner.Value;

        if (word_reqduedate == null || word_reqduedate.Year < 2000) WrkOrd.Setword_reqDueDateNull();
        else WrkOrd.word_reqDueDate = word_reqduedate;

        WrkOrds.AddWorkOrderRow(WrkOrd);
        int rowsAffected = WOadapter.Update(WrkOrds);
        //Insert(WrkOrd.word_WOnum, word_proj, WrkOrd.word_StatNum,
            //WrkOrd.word_Status, WrkOrd.word_statFlag, WrkOrd.word_Date, WrkOrd.word_Author, WrkOrd.word_Title,
            //WrkOrd.word_Doc, WrkOrd.word_DocVer, WrkOrd.word_Priority, WrkOrd.word_Descr, WrkOrd.word_Justify, WrkOrd.word_Cmnts,
            //WrkOrd.word_PMorSME, WrkOrd.word_BusnOwner, WrkOrd.word_reqDueDate);   //.Update(WrkOrds);
        WorkOrder.WorkOrderDataTable WOdataT = new WorkOrder.WorkOrderDataTable();
        WOdataT.AddWorkOrderRow(WrkOrd);
        //return WOdataT;


        return rowsAffected == 1;
    }


#endregion
}

public class WorkOrderRec
{
    public WorkOrderRec() { }

    private int _word_WOnum;
    public int Word_WOnum
    {
        get { return _word_WOnum; }
        set { _word_WOnum = value; }
    }

    private string _word_Proj;
    public string Word_Proj
    {
        get { return _word_Proj; }
        set { _word_Proj = value; }
    }

    private int _word_StatNum;
    public int Word_StatNum
    {
        get { return _word_StatNum; }
        set { _word_StatNum = value; }
    }

    private string _word_Status;
    public string Word_Status
    {
        get { return _word_Status; }
        set { _word_Status = value; }
    }

    private int _word_statFlag;
    public int Word_statFlag
    {
        get { return _word_statFlag; }
        set { _word_statFlag = value; }
    }

    private DateTime _word_Date;
    public DateTime Word_Date
    {
        get { return _word_Date; }
        set { _word_Date = value; }
    }

    private string _word_Author;
    public string Word_Author
    {
        get { return _word_Author; }
        set { _word_Author = value; }
    }

    private string _word_Title;
    public string Word_Title
    {
        get { return _word_Title; }
        set { _word_Title = value; }
    }

    private string _word_Doc;
    public string Word_Doc
    {
        get { return _word_Doc; }
        set { _word_Doc = value; }
    }

    private string _word_DocVer;
    public string Word_DocVer
    {
        get { return _word_DocVer; }
        set { _word_DocVer = value; }
    }

    private int _word_Priority;
    public int Word_Priority
    {
        get { return _word_Priority; }
        set { _word_Priority = value; }
    }

    private string _word_Descr;
    public string Word_Descr
    {
        get { return _word_Descr; }
        set { _word_Descr = value; }
    }

    private string _word_Justify;
    public string Word_Justify
    {
        get { return _word_Justify; }
        set { _word_Justify = value; }
    }

    private string _word_Cmnts;
    public string Word_Cmnts
    {
        get { return _word_Cmnts; }
        set { _word_Cmnts = value; }
    }

    private int _word_PMorSME;
    public int Word_PMorSME
    {
        get { return _word_PMorSME; }
        set { _word_PMorSME = value; }
    }

    private int _word_BusnOwner;
    public int Word_BusnOwner
    {
        get { return _word_BusnOwner; }
        set { _word_BusnOwner = value; }
    }

    private DateTime _word_reqDueDate;
    public DateTime Word_reqDueDate
    {
        get { return _word_reqDueDate; }
        set { _word_reqDueDate = value; }
    }

    
    // Additional code for the class.
}


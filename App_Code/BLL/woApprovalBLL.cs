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

[System.ComponentModel.DataObject]
/// <summary>
/// Summary description for woApprovalBLL
/// </summary>
public class woApprovalBLL
{
	public woApprovalBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region WOapproval Table
    private WOapprovalTableAdapter _woapprovaladapter = null;
    protected WOapprovalTableAdapter WOApprovaladapter
    {
        get {
            if (_woapprovaladapter == null)
                _woapprovaladapter = new WOapprovalTableAdapter();
            return _woapprovaladapter;
        }
    }

    private WOApprovalsPageTableAdapter _woaprvlPageAdapter = null;
    protected WOApprovalsPageTableAdapter WOaprvlPageAdapter
    {
        get
        {
            if (_woaprvlPageAdapter == null)
                _woaprvlPageAdapter = new WOApprovalsPageTableAdapter();
            return _woaprvlPageAdapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOapprovalDataTable GetWOApprovalData(int aprvlNum)
    {
        ////Type _bool = System.Type.GetType("System.Boolean");
               
        //WorkOrder.WOapprovalDataTable wodt = new WorkOrder.WOapprovalDataTable();
        //wodt = WOApprovaladapter.GetWOapprovalByAprvNum(aprvlNum);
        //wodt.Columns.Add("wapr_required1");
        //wodt.Columns.Add("wapr_AprvCode1");

        //foreach (DataRow dr in wodt)
        //{
        //    dr["wapr_required1"] = dr["wapr_required"];
        //    dr["wapr_AprvCode1"] = dr["wapr_AprvCode"];
        //}

        //return wodt;// 
        return WOApprovaladapter.GetWOapprovalByAprvNum(aprvlNum);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOapprovalDataTable GetWOApprovalData2(int aprvlNum)
    {
        ////Type _bool = System.Type.GetType("System.Boolean");

        //WorkOrder.WOapprovalDataTable wodt = new WorkOrder.WOapprovalDataTable();
        //wodt = WOApprovaladapter.GetWOapprovalByAprvNum(aprvlNum);
        //wodt.Columns.Add("wapr_required1");
        //wodt.Columns.Add("wapr_AprvCode1");

        //foreach (DataRow dr in wodt)
        //{
        //    dr["wapr_required1"] = dr["wapr_required"];
        //    dr["wapr_AprvCode1"] = dr["wapr_AprvCode"];
        //}

        //return wodt;// 
        return WOApprovaladapter.GetWOapprovalByAprvNum(aprvlNum);
    }

    //[DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    //public WorkOrder.WOapprovalDataTable GetApprovalDataWithUIDFullName(int WOnum, string proj)
    //{
    //    return WOaprvlPageAdapter.GetApprovalDataWithUIDFullName(WOnum, proj);
    //}

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOapprovalDataTable GetWOApprovalDataByWOandProj(int WOnum, string proj)
    {
        return WOApprovaladapter.GetApprovalDataByWOandProj(WOnum, proj);
    }

    
    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOapproval1DataTable GetApprovalDataWithUIDFullNameText(int word_WOnum, string word_Proj)
    {
        return WOaprvlPageAdapter.GetApprovalDataWithUIDFullName(word_WOnum, word_Proj);
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetNextApprovalNum()
    {
        int rowsAffected = Convert.ToInt32(WOApprovaladapter.GetNextApprovalNum());
        if (rowsAffected == 0) rowsAffected++;
        return rowsAffected;
    }


    [DataObjectMethodAttribute(DataObjectMethodType.Delete, true)]
    public int DeleteByApprovalNum(int apprvlNum)
    {
        int rowsAffected = Convert.ToInt32(WOApprovaladapter.DeleteApprovalRecord(apprvlNum));
        return rowsAffected;
    }



    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertNewWOApproval(int wapr_AprvNum, int wapr_WOnum, string wapr_Proj,
            DateTime? wapr_AprvDate, int? wapr_Approver, bool wapr_AprvCode, DateTime? wapr_AprvlDate,
        string wapr_comments, bool wapr_required, bool wapr_emailFlag)
    {
        if (wapr_AprvDate == null) wapr_AprvDate = DateTime.Now;

        int rowsAffected = Convert.ToInt32(WOApprovaladapter.InsertNewWOapproval(wapr_AprvNum, wapr_WOnum, wapr_Proj,
            wapr_AprvDate, wapr_Approver, wapr_AprvCode, wapr_AprvlDate,
            wapr_comments, wapr_required, wapr_emailFlag));

        return rowsAffected == 1;

    }

    [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    public bool UpdateWOAprvlRecord(int wapr_AprvNum, int wapr_WOnum, string wapr_Proj,
            DateTime? wapr_AprvDate, int? wapr_Approver, bool wapr_AprvCode, DateTime? wapr_AprvlDate,
        string wapr_comments, bool wapr_required, bool wapr_emailFlag, int Original_wapr_AprvNum)
    {
        if (wapr_AprvDate == null) wapr_AprvDate = DateTime.Now;

        int rowsAffected = WOApprovaladapter.UpdateApprovalRecord(wapr_AprvNum, wapr_WOnum, wapr_Proj,
            wapr_AprvDate, wapr_Approver, wapr_AprvCode, wapr_AprvlDate,
            wapr_comments, wapr_required, wapr_emailFlag, Original_wapr_AprvNum);

        return rowsAffected == 1;

    }


    [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    public bool UpdateAprvlRecord(WorkOrder.WOapprovalRow woDataRow)
    {
        //DateTime dt_WOdatetime = Convert.ToDateTime(woDataTbl.Columns["wapr_AprvDate"]);
        //if (woDataTbl.Columns["wapr_AprvDate"] == null) dt_WOdatetime = DateTime.Now;
        WorkOrder.WOapprovalDataTable woDataTbl = new WorkOrder.WOapprovalDataTable(); //WOApprovaladapter.GetWOapprovalByAprvNum(Convert.ToInt32(woDataRow.wapr_AprvNum));
        woDataTbl.AddWOapprovalRow(woDataRow);
        //for (int i = 0; i < woDataRow.ItemArray.Length; i++)
        //{
        //    woDataTbl.Rows[0].ItemArray[i] = woDataRow.ItemArray[""];
        //}



        int rowsAffected = WOApprovaladapter.Update(woDataTbl);
            
            
            //.UpdateApprovalRecord(Convert.ToInt32(woDataRow.ItemArray["wapr_AprvNum"]), 
            //Convert.ToInt32(woDataRow.ItemArray["wapr_WOnum"]), woDataRow.ItemArray["wapr_Proj"].ToString(),
            //Convert.ToDateTime(woDataRow.ItemArray["wapr_AprvDate"]), Convert.ToInt32(woDataRow.ItemArray["wapr_Approver"]), 
            //Convert.ToBoolean(woDataRow.ItemArray["wapr_AprvCode"]), Convert.ToDateTime(woDataRow.ItemArray["wapr_AprvlDate"]),
            //woDataRow.ItemArray["wapr_comments"].ToString(), Convert.ToBoolean(woDataRow.ItemArray["wapr_required"]),
            //Convert.ToBoolean(woDataRow.ItemArray["wapr_emailFlag"]), Convert.ToInt32(woDataRow.ItemArray["wapr_AprvNum"]));

        return rowsAffected == 1;

    }

#endregion 

}



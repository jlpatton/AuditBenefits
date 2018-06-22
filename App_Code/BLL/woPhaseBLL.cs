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
/// Summary description for woPhaseBLL
/// </summary>
public class woPhaseBLL
{
	public woPhaseBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region WOPhaseTable
    private WOphaseTableAdapter _woPhaseAdapter = null;
    protected WOphaseTableAdapter WOphaseAdapter
    {
        get
        {
            if (_woPhaseAdapter == null)
                _woPhaseAdapter = new WOphaseTableAdapter();
            return _woPhaseAdapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetNewPhaseNumber()
    {
        int PhaseNum = Convert.ToInt32(WOphaseAdapter.GetNewPhaseNumber());

        return PhaseNum;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOphaseDataTable GetRecentPhaseByWOandProj(int wonum, string proj)
    {
        return WOphaseAdapter.GetRecentPhaseByWOandProj(wonum, proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public int InsertPhaseRecord(int wonum, string proj, int RespNum,
        DateTime PhaseDate, string Phase, int statFlag, int uid)
    {
        int PhaseNum = GetNewPhaseNumber();

        if (PhaseNum == 0) PhaseNum++;

        int rowsAffected = WOphaseAdapter.InsertPhaseRecord(PhaseNum, wonum, proj, RespNum,
        PhaseDate, Phase, statFlag, uid);

        if (rowsAffected == 1)
        {
            return PhaseNum;
        }
        else
        {
            return 0;
        }
    }

    //[DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    //public WorkOrder.WOstatusDataTable InsertWorkOrderStatus(int statnum, int? wonum, int? respnum,
    //    DateTime tmstmp, string? stat, int? statflag, int? uid, string? cmnt, string? doc, string? docver)
    //{

    //    WorkOrder.WOstatusDataTable WrkStatus = new WorkOrder.WOstatusDataTable();
    //    WorkOrder.WOstatusRow WrkStat = WrkStatus.NewWOstatusRow();

    //    // insert values here
    //    WrkStat.wsts_StatNum = statnum;

    //    if (wonum == null) WrkStat.Setwsts_WOnumNull();
    //    else WrkStat.wsts_WOnum = wonum.Value;

    //    if (respnum == null) WrkStat.Setwsts_RespNumNull();
    //    else WrkStat.wsts_RespNum = respnum.Value;

    //    WrkStat.wsts_Date = DateTime.Today;
        
    //    if (stat == null) WrkStat.Setwsts_StatusNull();
    //    else WrkStat.wsts_Status = stat.Value;

    //    if (statflag == null) WrkStat.Setwsts_StatFlagNull();
    //    else WrkStat.wsts_StatFlag = statflag.Value;

    //    if (uid == null) WrkStat.Setwsts_UidNull();
    //    else WrkStat.wsts_Uid = uid.Value;

    //    if (cmnt == null) WrkStat.Setwsts_CommentsNull();
    //    else WrkStat.wsts_Comments = cmnt.Value;

    //    if (doc == null) WrkStat.Setwsts_DocNull();
    //    else WrkStat.wsts_Doc = doc.Value;

    //    if (docver == null) WrkStat.Setwsts_DocVersionNull();
    //    else WrkStat.wsts_DocVersion = docver.Value;


    //    WrkStatus.AddWOstatusRow(WrkStat);
    //    int rowsAffected = WOstatadapter.Update(WrkStatus);

    //    return rowsAffected == 1;
    //}
    #endregion

}

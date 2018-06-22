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
/// Summary description for woResponseBLL
/// </summary>
public class woResponseBLL
{
	public woResponseBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private WOresponseTableAdapter _woresponseAdapter = null;
    protected WOresponseTableAdapter WOrespAdapter
    {
        get
        {
            if (_woresponseAdapter == null)
                _woresponseAdapter = new WOresponseTableAdapter();
            return _woresponseAdapter;
        }
    }

    private WOresponse1TableAdapter _woresponse1Adapter = null;
    protected WOresponse1TableAdapter WOresp1Adapter
    {
        get
        {
            if (_woresponse1Adapter == null)
                _woresponse1Adapter = new WOresponse1TableAdapter();
            return _woresponse1Adapter;
        }
    }

    //[DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    //public WorkOrder.WOresponseDataTable GetMaxResponseByWOproj(int wonum, string proj)
    //{
    //    return WOrespAdapter.GetMaxResponsebyWOproj(proj, wonum);
    //}


    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOresponse1DataTable GetRespByWOproj(int wonum, string proj)
    {
        return WOresp1Adapter.GetWOresponseDataByWOproj(wonum, proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOresponse1DataTable GetMaxRespByWOnumProj(int woNum, string proj)
    {
        return WOresp1Adapter.GetMaxRespByWOproj(woNum, proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertNewResponse(int wrsp_RespNum, int wrsp_WOnum, string wrsp_Proj, int? wrsp_RespToResp, int? wrsp_StatNum,
        string wrsp_Status, int? wrsp_statFlag, int? wrsp_EstMnths, int? wrsp_EstHrs, string wrsp_Results,
        string wrsp_Consider, string wrsp_Risks, int? wrsp_uid, DateTime wrsp_datetime)
    {
        WorkOrder.WOresponse1DataTable Responses = new WorkOrder.WOresponse1DataTable();
        WorkOrder.WOresponse1Row response = Responses.NewWOresponse1Row();

        WOresponse1TableAdapter respAdapter = new WOresponse1TableAdapter();
        int MaxrespNum = Convert.ToInt32(WOrespAdapter.MaxResponseNum(wrsp_WOnum, wrsp_Proj));

        response.wrsp_RespNum = MaxrespNum + 1;
        response.wrsp_WOnum = wrsp_WOnum;

        response.wrsp_Proj = wrsp_Proj;

        if (wrsp_RespToResp == null) response.Setwrsp_RespToRespNull();
        else response.wrsp_RespToResp = wrsp_RespToResp.Value;

        if (wrsp_StatNum == null) response.Setwrsp_StatNumNull();
        else response.wrsp_StatNum = wrsp_StatNum.Value;

        if (wrsp_Status == null) response.Setwrsp_StatusNull();
        else response.wrsp_Status = wrsp_Status;

        if (wrsp_statFlag == null) response.Setwrsp_statFlagNull();
        else response.wrsp_statFlag = wrsp_statFlag.Value;

        if (wrsp_EstMnths == null) response.Setwrsp_EstMnthsNull();
        else response.wrsp_EstMnths = wrsp_EstMnths.Value;

        if (wrsp_EstHrs == null) response.Setwrsp_EstHrsNull();
        else response.wrsp_EstHrs = wrsp_EstHrs.Value;

        if (wrsp_Results == null) response.Setwrsp_ResultsNull();
        else response.wrsp_Results = wrsp_Results;

        if (wrsp_Consider == null) response.Setwrsp_ConsiderNull();
        else response.wrsp_Consider = wrsp_Consider;

        if (wrsp_Risks == null) response.Setwrsp_RisksNull();
        else response.wrsp_Risks = wrsp_Risks;

        if (wrsp_uid == null) response.Setwrsp_uidNull();
        else response.wrsp_uid = wrsp_uid.Value;

        if (wrsp_datetime == null || wrsp_datetime.CompareTo(Convert.ToDateTime("1/1/0001")) == 0) response.Setwrsp_datetimeNull();
        else response.wrsp_datetime = wrsp_datetime;

        Responses.AddWOresponse1Row(response);
        int rowsAffected = WOresp1Adapter.Update(Responses);

        return rowsAffected == 1;

    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOresponseDataTable GetWOresponseByWOproj(int wonum, string proj)
    {
        return WOrespAdapter.GetWOresponseByWOproj(wonum, proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOresponseDataTable GetWOResponse()
    {
        return WOrespAdapter.GetWorkOrderResponse();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOresponseDataTable GetWOResponseByRespNum(int respNum, int wonum, string proj)
    {
        return WOrespAdapter.GetWorkOrderResponseByResponseNum(respNum,wonum,proj);
    }
    
    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public int GetWOresponseCountByWOandProj(int woNum, string proj)
    {
        int li_rtn = Convert.ToInt32(WOrespAdapter.GetWOresponseCountByWOandProj(woNum,proj));
        return li_rtn;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOresponseDataTable GetMaxWOResponseByWOnumProj(int woNum, string proj)
    {
        return WOrespAdapter.GetMaxWOresponseByWOnum(woNum,proj);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    public bool InsertNewWOresponse(int wrsp_RespNum, int wrsp_WOnum, string wrsp_Proj, int? wrsp_StatNum,
        string wrsp_Status, int? wrsp_statFlag, int? wrsp_EstMnths, int? wrsp_EstHrs, string wrsp_Results,
        string wrsp_Consider, string wrsp_Risks,int? wrsp_uid, DateTime wrsp_datetime)
    {
        WorkOrder.WOresponseDataTable Responses = new WorkOrder.WOresponseDataTable();
        WorkOrder.WOresponseRow response = Responses.NewWOresponseRow();

        WOresponseTableAdapter respAdapter = new WOresponseTableAdapter();
        int MaxrespNum = Convert.ToInt32(respAdapter.MaxResponseNum(wrsp_WOnum, wrsp_Proj));

        response.wrsp_RespNum = MaxrespNum + 1;
        response.wrsp_WOnum = wrsp_WOnum;

        response.wrsp_Proj = wrsp_Proj;

        if (wrsp_StatNum == null) response.Setwrsp_StatNumNull();
        else response.wrsp_StatNum = wrsp_StatNum.Value;

        if (wrsp_Status == null) response.Setwrsp_StatusNull();
        else response.wrsp_Status = wrsp_Status;

        if (wrsp_statFlag == null) response.Setwrsp_statFlagNull();
        else response.wrsp_statFlag = wrsp_statFlag.Value;

        if (wrsp_EstMnths == null) response.Setwrsp_EstMnthsNull();
        else response.wrsp_EstMnths = wrsp_EstMnths.Value;

        if (wrsp_EstHrs == null) response.Setwrsp_EstHrsNull();
        else response.wrsp_EstHrs = wrsp_EstHrs.Value;

        if (wrsp_Results == null) response.Setwrsp_ResultsNull();
        else response.wrsp_Results = wrsp_Results;

        if (wrsp_Consider == null) response.Setwrsp_ConsiderNull();
        else response.wrsp_Consider = wrsp_Consider;

        if (wrsp_Risks == null) response.Setwrsp_RisksNull();
        else response.wrsp_Risks = wrsp_Risks;

        if (wrsp_uid == null) response.Setwrsp_uidNull();
        else response.wrsp_uid = wrsp_uid.Value;

        if (wrsp_datetime == null || wrsp_datetime.CompareTo(Convert.ToDateTime("1/1/0001")) == 0) response.Setwrsp_datetimeNull();
        else response.wrsp_datetime = wrsp_datetime;

        Responses.AddWOresponseRow(response);
        int rowsAffected = WOrespAdapter.Update(Responses);

        return rowsAffected == 1;

    }

    [DataObjectMethodAttribute(DataObjectMethodType.Update, true)]
    public bool UpdateWOresponse(int wrsp_RespNum, int wrsp_WOnum, string wrsp_Proj, int? wrsp_StatNum,
        string wrsp_Status, int? wrsp_statFlag, int? wrsp_EstMnths, int? wrsp_EstHrs, string wrsp_Results,
        string wrsp_Consider, string wrsp_Risks)
    {
        WorkOrder.WOresponseDataTable Responses = WOrespAdapter.GetWorkOrderResponseByResponseNum(wrsp_RespNum, wrsp_WOnum, wrsp_Proj); // new WorkOrder.WOresponseDataTable();

        if (Responses.Count == 0) return false;
        
        WorkOrder.WOresponseRow response = Responses[0]; //.Rows.NewWOresponseRow();

        int MaxrespNum = Convert.ToInt32(WOrespAdapter.MaxResponseNum(wrsp_WOnum, wrsp_Proj));

        //if (MaxrespNum == null) MaxrespNum = 0;

        response.wrsp_RespNum = MaxrespNum + 1;
        response.wrsp_WOnum = wrsp_WOnum;

        response.wrsp_Proj = wrsp_Proj;

        if (wrsp_StatNum == null) response.Setwrsp_StatNumNull();
        else response.wrsp_StatNum = wrsp_StatNum.Value;

        if (wrsp_Status == null) response.Setwrsp_StatusNull();
        else response.wrsp_Status = wrsp_Status;

        if (wrsp_statFlag == null) response.Setwrsp_statFlagNull();
        else response.wrsp_statFlag = wrsp_statFlag.Value;

        if (wrsp_EstMnths == null) response.Setwrsp_EstMnthsNull();
        else response.wrsp_EstMnths = wrsp_EstMnths.Value;

        if (wrsp_EstHrs == null) response.Setwrsp_EstHrsNull();
        else response.wrsp_EstHrs = wrsp_EstHrs.Value;

        if (wrsp_Results == null) response.Setwrsp_ResultsNull();
        else response.wrsp_Results = wrsp_Results;

        if (wrsp_Consider == null) response.Setwrsp_ConsiderNull();
        else response.wrsp_Consider = wrsp_Consider;

        if (wrsp_Risks == null) response.Setwrsp_RisksNull();
        else response.wrsp_Risks = wrsp_Risks;

        //Responses.AddWOresponseRow(response);
        int rowsAffected = WOrespAdapter.Update(Responses);

        return rowsAffected == 1;

    }

}

using System;
using System.Data;
using System.Data.SqlClient;
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
/// Summary description for CodesBLL
/// </summary>
/// [System.ComponentModel.DataObject]
[System.ComponentModel.DataObject]
public class CodesBLL
{
	public CodesBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private codesTableAdapter _codesadapter = null;
    protected codesTableAdapter CodesAdapter
    {
        get
        {
            if (_codesadapter == null)
                _codesadapter = new codesTableAdapter();
            return _codesadapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.codesDataTable GetCodes()
    {
        return CodesAdapter.GetCodeData();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.codesDataTable GetPriorityCodes()
    {
        return CodesAdapter.GetPriorityCodes();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.codesDataTable GetProjectCodes()
    {
        return CodesAdapter.GetProjectCodes();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.codesDataTable GetRoleCodes()
    {
        return CodesAdapter.GetRoleCodes();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.codesDataTable GetPhaseStatus(string p1,string p2, string p3)
    {
        return CodesAdapter.GetPhaseStatus(p1,p2,p3);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.codesDataTable GetAllPhaseStatus()
    {
        return CodesAdapter.GetAllPhaseStatus();
    }

    //[DataObjectMethodAttribute(DataObjectMethodType.Insert, false)]
    //public WorkOrder.codesDataTable InsertNewPriorityCode(string priority, string codeid, 
    //    string? codedesc, int? actFlag, DateTime dtTmStmp)
    //{
    //    priority = "wopriority";
    //    WorkOrder.codesDataTable PriorityCodes = new WorkOrder.codesDataTable();
    //    WorkOrder.codesRow PriorityCode = PriorityCodes.NewcodesRow();

    //    PriorityCode.code_id = codeid;
    //    PriorityCode.code_type = priority;

    //    if (codedesc == null) PriorityCode.Setcode_descNull;
    //    else PriorityCode.code_desc = codedesc.Value;

    //    if (actFlag == null) PriorityCode.Setcode_ActiveFlagNull;
    //    else PriorityCode.code_ActiveFlag = actFlag.Value;

    //    PriorityCode.code_Date = DateTime.Today;

    //    PriorityCodes.AddcodesRow(PriorityCode);
    //    int rowsAffected = CodesAdapter.Update(PriorityCodes);

    //    return rowsAffected == 1;
    //}

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, false)]
    public bool InsertNewProjectCode(string project,
            string codeid, string codedesc, int actFlag, DateTime dtTmStmp)
    {
        project = "woproj";
        WorkOrder.codesDataTable ProjectCodes = new WorkOrder.codesDataTable();
        WorkOrder.codesRow ProjectCode = ProjectCodes.NewcodesRow();

        ProjectCode.code_id = codeid;
        ProjectCode.code_type = project;

        if (codedesc == null) ProjectCode.Setcode_descNull();
        else ProjectCode.code_desc = codedesc;

        //if (actFlag == null) ProjectCode.Setcode_ActiveFlagNull();
        //else 
        ProjectCode.code_ActiveFlag = actFlag;

        ProjectCode.code_Date = DateTime.Today;

        ProjectCodes.AddcodesRow(ProjectCode);
        int rowsAffected = CodesAdapter.Update(ProjectCodes);

        return rowsAffected == 1;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, false)]
    public bool InsertNewRoleCode(string role,
            string codeid, string codedesc, int actFlag, DateTime dtTmStmp)
    {
        role = "worole";
        WorkOrder.codesDataTable RoleCodes = new WorkOrder.codesDataTable();
        WorkOrder.codesRow RoleCode = RoleCodes.NewcodesRow();

        RoleCode.code_id = codeid;
        RoleCode.code_type = role;

        if (codedesc == null) RoleCode.Setcode_descNull();
        else RoleCode.code_desc = codedesc;

        //if (actFlag == null) RoleCode.Setcode_ActiveFlagNull();
        //else 
        RoleCode.code_ActiveFlag = actFlag;

        RoleCode.code_Date = DateTime.Today;

        RoleCodes.AddcodesRow(RoleCode);
        int rowsAffected = CodesAdapter.Update(RoleCodes);

        return rowsAffected == 1;
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Insert, false)]
    public bool InsertNewPhaseStatus(string phase,
            string codeid, string codedesc, int actFlag, DateTime dtTmStmp)
    {
        phase = "wostat";
        WorkOrder.codesDataTable PhaseStatusCodes = new WorkOrder.codesDataTable();
        WorkOrder.codesRow PhaseStatus = PhaseStatusCodes.NewcodesRow();

        PhaseStatus.code_id = codeid;
        PhaseStatus.code_type = phase;

        if (codedesc == null) PhaseStatus.Setcode_descNull();
        else PhaseStatus.code_desc = codedesc;

        //if (actFlag == null) PhaseStatus.Setcode_ActiveFlagNull();
        //else 
            PhaseStatus.code_ActiveFlag = actFlag;

        PhaseStatus.code_Date = DateTime.Today;

        PhaseStatusCodes.AddcodesRow(PhaseStatus);
        int rowsAffected = CodesAdapter.Update(PhaseStatusCodes);

        return rowsAffected == 1;
    }

}

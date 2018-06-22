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
/// Summary description for woUsersBLL
/// </summary>
public class woUsersBLL
{
	public woUsersBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private WOusersTableAdapter _wouseradapter = null;
    protected WOusersTableAdapter WOUserAdapter
    {
        get
        {
            if (_wouseradapter == null)
                _wouseradapter = new WOusersTableAdapter();
            return _wouseradapter;
        }
    }
    private UsersDDLTableAdapter _userDDLadapter = null;
    protected UsersDDLTableAdapter UserDDLadapter
    {
        get
        {
            if (_userDDLadapter == null)
                _userDDLadapter = new UsersDDLTableAdapter();
            return _userDDLadapter;
        }
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
    public WorkOrder.WOusersDataTable GetUsers()
    {
        return WOUserAdapter.GetUsers();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.WOusersDataTable GetUserByID(int uid)
    {
        return WOUserAdapter.GetUserByUserID(uid);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.UsersDDLDataTable GetUsersDDList()
    {
        return UserDDLadapter.GetUserDropDown();
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public WorkOrder.UsersDDLDataTable GetDDLusersByRole(string role)
    {
        return UserDDLadapter.GetDDLusersByRole(role);
    }

    [DataObjectMethodAttribute(DataObjectMethodType.Select, false)]
    public string GetFullNameByID(int uid)
    {
        string fname = (string)WOUserAdapter.GetFullNameByID(uid);
        return fname;
    }

    //[DataObjectMethodAttribute(DataObjectMethodType.Insert, true)]
    //public WorkOrder.WOusersDataTable InsertNewUser(int uid, string? fname,
    //    string? lname, string? dept, string? role, int? active, DateTime dttmstmp)
    //{
    //    WorkOrder.WOusersDataTable WOUsers = new WorkOrder.WOusersDataTable();
    //    WorkOrder.WOusersRow WOuser = WOUsers.NewWOusersRow();

    //    WOuser.wusr_uid = uid;

    //    if (fname == null) WOuser.Setwusr_FnameNull();
    //    else WOuser.wusr_Fname = fname.Value;

    //    if (lname == null) WOuser.Setwusr_LastNameNull();
    //    else WOuser.wusr_LastName = lname.Value;

    //    if (dept == null) WOuser.Setwusr_DeptNull();
    //    else WOuser.wusr_Dept = dept.Value;

    //    if (role == null) WOuser.Setwusr_woRoleNull();
    //    else WOuser.wusr_woRole = role.Value;

    //    if (active == null) WOuser.Setwusr_ActiveFlagNull();
    //    else WOuser.wusr_ActiveFlag = active.Value;

    //    WOuser.wusr_Date = DateTime.Today;

    //    WOUsers.AddWOusersRow(WOuser);
    //    int rowsAffected = WOUserAdapter.Update(WOUsers);

    //    return rowsAffected == 1;

    //}

}

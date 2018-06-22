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
using EBA.Desktop.Admin;
//using EBA.Desktop.Anthem;
using EBA.Desktop.Audit;
using EBA.Desktop;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class Admin_Console1 : System.Web.UI.Page
{
    string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M1", "");
            hideUserInfoDiv();
            roleManageDiv();
            ddlRoleName.DataBind();
            ddlRoleName.DataBind();
            grdRole.DataBind();
            grdUser.DataBind();
           
        }
        


    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
        if (e.Item.Value.Equals("0"))
        {
            hideUserInfoDiv();
        }
    }   

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblBeforeAdd.Visible = false;
        userAdded.Visible = false;
        try
        {
            string userId = tbxEmpno.Text;
            ldapClient userObject = new ldapClient();
            UserRecord ud = userObject.SearchUser(userId);
            ldapUserInfo.Visible = true;            
            lblLast.Text = ud.LastName.ToString();
            lblFirst.Text = ud.FirstName.ToString();
            lblEmail.Text = ud.Email.ToString();
            lblDepNo.Text = ud.OrganizationCode.ToString();
            lblDepName.Text = ud.DepartmentName.ToString();
            lblManager.Text = ud.ManagerId.ToString();
        }
        catch(Exception ex)
        {
            lblBeforeAdd.Visible = true;
            lblBeforeAdd.Text = ex.Message;
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr1 = "SELECT COUNT(*) FROM UserLogin_AD WHERE empID = @empid";
        string cmdStr = "INSERT INTO UserLogin_AD (empID,UserName)VALUES (@empid,@uname)";
        SqlCommand cmd = new SqlCommand(cmdStr, conn);
        SqlCommand cmd1 = new SqlCommand(cmdStr1, conn); 
        int _exsists = 0;
        cmd.Parameters.Add("@empid", SqlDbType.Int);
        cmd.Parameters.Add("@uname", SqlDbType.VarChar);
        cmd.Parameters["@empid"].Value = tbxEmpno.Text.ToString();
        cmd.Parameters["@uname"].Value = lblFirst.Text.ToString() + " " + lblLast.Text.ToString();
        cmd1.Parameters.Add("@empid", SqlDbType.Int);
        cmd1.Parameters["@empid"].Value = tbxEmpno.Text.ToString();
        try
        {
            conn.Open();
            _exsists = Int32.Parse(cmd1.ExecuteScalar().ToString());
            if (_exsists == 0)
            {
                cmd.ExecuteNonQuery();
                showAfteruserAdd("Successfully added user '" + tbxEmpno.Text + "' to the User list!");
            }
            else
            {
                showAfteruserAdd("User already exist!");
            }
        }
        catch (Exception ex)
        {
            showAfteruserAdd("Error adding User to the userlist!");
        }
        finally
        {
            cmd.Dispose();
            cmd1.Dispose();
            conn.Close();            
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hideUserInfoDiv();
    }

    protected void showAfteruserAdd(string message)
    {
        userAdded.Visible = true;
        lblafterAdd.Text = message;
    }

    protected void hideUserInfoDiv()
    {
        tbxEmpno.Text = "";
        ldapUserInfo.Visible = false;
        lblLast.Text = "";
        lblFirst.Text = "";
        lblEmail.Text = "";
        lblDepNo.Text = "";
        lblDepName.Text = "";
        lblManager.Text = "";
        userAdded.Visible = false;
        lblafterAdd.Text = "";
    }

    ////On row created - Roles table
    //protected void grdRole_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    e.Row.Cells[0].Visible = false;
    //}    

    /// <summary>
    /// After adding a product, hide the WebWindow
    /// </summary>
    protected void frmAdd_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        winAdd.Hide = true;
        grdRole.DataBind();
        ddlRoleName.DataBind();
    }

    /// <summary>
    /// After clicking Cancel, hide the WebWindow
    /// </summary>
    protected void frmAdd_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        winAdd.Hide = true;
        grdRole.DataBind();
        ddlRoleName.DataBind();
    }

    protected void ddlRoleName_OnselectedIndexchange(object sender, EventArgs e)
    {
        roleManageDiv();
        lblErr1.Text = "";
    }
    protected void btnRSel_Click(object sender, EventArgs e)
    {
        int i = lstUsers1.Items.Count;
        for (int j = 0; j < i; j++)
        {
            if (lstUsers1.Items[j].Selected)
            {
                string item = lstUsers1.Items[j].Text.ToString();
                lstUsers2.Items.Add(item);
            }
        }
    }
    protected void btnDSel_Click(object sender, EventArgs e)
    {
        try
        {
            int i = lstUsers2.SelectedIndex;
            string item = lstUsers2.Items[i].Text.ToString();
            lstUsers2.Items.Remove(item);
        }
        catch (Exception ex)
        {
            lblErr.Text = " (*) Please Select User to remove from List";
        }

    }
    protected void btnClear1_Click(object sender, EventArgs e)
    {
        lstUsers2.Items.Clear();
        lblErr.Text = "";        
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr = "SELECT RoleID FROM Roles_AD WHERE RoleName = @role";
        int roleid = 0;
        List<string> userList = new List<string>();

        try
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@role", SqlDbType.VarChar);
            cmd.Parameters["@role"].Value = ddlRoleName.SelectedItem.Text.ToString();
            roleid = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();

            string type1 = Session["type1"].ToString();
            if (type1.Equals("0"))
            {
                deleteUserFromRole(roleid);
            }
            else if (type1.Equals("1"))
            {
                userList = addUserToRole(roleid);
            }
            lblErr1.Text = "";
        }
        catch (Exception ex)
        {
            lblErr1.Text = ex.Message;
        }
        finally
        {
            grdRoleUser.DataBind();
            conn.Close();
            roleManageDiv();
            string uList = "";
            foreach (string li in userList)
            {
                uList = uList + li + ",";
            }
            if (!uList.Equals(""))
            {
                lblUserAdmin.Visible = true;
                lblUserAdmin.Text = uList + " Users are already defined as Admin and were not added to the role";
            }
            else
            {
                lblUserAdmin.Visible = false;
                lblUserAdmin.Text = "";
            }
        }
    }

    // Delete Users from Role
    protected void deleteUserFromRole(int roleid)
    {        
        string cmdStr = "DELETE FROM UserRoles_AD WHERE UserID = @UID AND RoleID = @RID ";
        string cmdStr1 = "SELECT UserId FROM UserLogin_AD WHERE UserName = @User";
        int i = lstUsers2.Items.Count;
        int userid;
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connStr);
        try
        {
            for (int j = 0; j < i; j++)
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //get UserId for the Username
                cmd = new SqlCommand(cmdStr1, conn);
                cmd.Parameters.Add("@User", SqlDbType.VarChar);
                cmd.Parameters["@User"].Value = lstUsers2.Items[j].Text.ToString();
                userid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                
                cmd = new SqlCommand(cmdStr, conn);
                cmd.Parameters.Add("@UID", SqlDbType.Int);
                cmd.Parameters["@UID"].Value = userid;
                cmd.Parameters.Add("@RID", SqlDbType.Int);
                cmd.Parameters["@RID"].Value = roleid;
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            throw (new Exception("Error Deleting Users From Role"));
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
        }
    }

    //Add user to role
    protected List<string> addUserToRole(int roleid)
    {
        string cmdStr = "INSERT INTO UserRoles_AD VALUES (@UID,@RID) ";
        string cmdStr1 = "SELECT UserId FROM UserLogin_AD WHERE UserName = @User";
        //string cmdStr2 = "SELECT COUNT(*) FROM UserRoles_AD WHERE UserID=@userid AND RoleID IN (SELECT RoleID FROM Roles_AD WHERE RoleName LIKE 'ADMIN%')";
        // 5/6/2009 - Check to see if the user is present in the role user is being added to instead of the ADMIN role as done above
        string cmdStr2 = "SELECT COUNT(*) FROM UserRoles_AD WHERE UserID=@userid AND RoleID IN (SELECT RoleID FROM Roles_AD WHERE roleid = "+  roleid + ")";
        int i = lstUsers2.Items.Count;
        int userid;
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection(connStr);
        int _exsists = 0;
        List<string> userList = new List<string>();

        try
        {
            for (int j = 0; j < i; j++)
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                //get UserId for the Username
                cmd = new SqlCommand(cmdStr1, conn);
                cmd.Parameters.Add("@User", SqlDbType.VarChar);
                cmd.Parameters["@User"].Value = lstUsers2.Items[j].Text.ToString();
                userid = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();

                //check if user is already a admin
                cmd = new SqlCommand(cmdStr2, conn);                
                cmd.Parameters.Add("@userid", SqlDbType.VarChar);
                cmd.Parameters["@userid"].Value = userid;
                _exsists = Int32.Parse(cmd.ExecuteScalar().ToString());
                cmd.Dispose();

                if (_exsists == 0)
                {
                    cmd = new SqlCommand(cmdStr, conn);
                    cmd.Parameters.Add("@UID", SqlDbType.Int);
                    cmd.Parameters["@UID"].Value = userid;
                    cmd.Parameters.Add("@RID", SqlDbType.Int);
                    cmd.Parameters["@RID"].Value = roleid;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                else
                {
                    userList.Add(lstUsers2.Items[j].Text.ToString());
                }
            }
        }
        catch
        {
            throw (new Exception("Error Adding Users to Role"));
        }
        finally
        {
            cmd.Dispose();
            conn.Close();
        }
        return userList;
    }

    protected void btnCancelrole_Click(object sender, EventArgs e)
    {
        roleManageDiv();
    }
    protected void lnkaddRuser(object sender, EventArgs e)
    {
        lstUsers1.Items.Clear();
        lstUsers2.Items.Clear();
        addremRoleUser.Visible = true;
        addRoleApp.Visible = false;
        lblUserAdmin.Visible = false;
        fill_userList(1);
        Session["type1"] = 1;
        lblRem.Text = "Please select Users from list to add to the role";
    }
    protected void lnkremRuser(object sender, EventArgs e)
    {
        lstUsers1.Items.Clear();
        lstUsers2.Items.Clear();
        addremRoleUser.Visible = true;
        addRoleApp.Visible = false;
        lblUserAdmin.Visible = false;
        fill_userList(0);
        Session["type1"] = 0;
        lblRem.Text = "Please select Users from list to remove from the role";
    }   
    protected void roleManageDiv()
    {
        addremRoleUser.Visible=false;
        addRoleApp.Visible = false;
        lblErr.Text = "";        
    }

    //Get List of users 0 - Users in Roles, 1 - Users not in Role
    protected void fill_userList(int stat)
    {
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr = "";
        if (stat == 0)
        {
            cmdStr = "SELECT UserName FROM UserLogin_AD u, UserRoles_AD l, Roles_AD r "
                        + " WHERE RoleName = @role and l.RoleID = r.RoleID and l.UserID = u.UserID";
        }
        else if (stat == 1)
        {
            cmdStr = "SELECT UserName from UserLogin_AD WHERE UserName NOT IN "
                        + " (SELECT UserName FROM UserLogin_AD u, UserRoles_AD l, Roles_AD r WHERE RoleName = @role and l.RoleID = r.RoleID and l.UserID = u.UserID)";
        }

        SqlCommand cmd = new SqlCommand(cmdStr, conn);
        if (conn == null || conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }
        cmd.Parameters.Add("@role", SqlDbType.VarChar);
        cmd.Parameters["@role"].Value = ddlRoleName.SelectedItem.Text.ToString();

        SqlDataReader dr;
        try
        {
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string item = Convert.ToString(dr[0]);
                lstUsers1.Items.Add(item);
            }
            dr.Close();
        }
        catch (Exception e)
        {
        }
        finally
        {
            conn.Close();
        }


    }

    //Application settings
    protected void lnkaddRapp(object sender, EventArgs e)
    {
        check_app();
        addRoleApp.Visible = true;
        addremRoleUser.Visible = false;
    }
    protected void btnAppEdit_click(object sender, EventArgs e)
    {        
        enabledisableApp("enable");
    }
    protected void btnAppClose_click(object sender, EventArgs e)
    {
        roleManageDiv();
        enabledisableApp("disable");
    }

    protected void btnAppUpdate_Click(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr = "SELECT RoleID FROM Roles_AD WHERE RoleName = @role";
        string cmdStr1 = "SELECT MID FROM Page_AD WHERE MCode =@code";
        string cmdStr2 = "DELETE FROM RoleOperations_AD WHERE RoleID = @role";
        string cmdStr3 = "INSERT INTO RoleOperations_AD VALUES(@role,@mid,'1')";
        string code = "";
        int roleid = 0;
        ArrayList mid = new ArrayList();

        SqlCommand cmd2;
        if (conn == null || conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }
        try
        {
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@role", SqlDbType.VarChar);
            cmd.Parameters["@role"].Value = ddlRoleName.SelectedItem.Text.ToString();
            roleid = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand(cmdStr2, conn);
            cmd1.Parameters.Add("@role", SqlDbType.Int);
            cmd1.Parameters["@role"].Value = roleid;
            cmd1.ExecuteNonQuery();

            cmd.Dispose();
            cmd1.Dispose();

            if (cbxAdmin.Checked)
            {
                code = "M1";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxAudit.Checked)
            {
                code = "M2";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxAnth.Checked)
            {
                code = "M100";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxBilling.Checked)
            {
                code = "M101";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxClaims.Checked)
            {
                code = "M102";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxReport.Checked)
            {
                code = "M103";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxAMaintenance.Checked)
            {
                code = "M104";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHRA.Checked)
            {
                code = "M200";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHMain.Checked)
            {
                code = "M201";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHRecon.Checked)
            {
                code = "M202";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }      
            if (cbxHOper.Checked)
            {
                code = "M203";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }            
            if (cbxHLtr.Checked)
            {
                code = "M204";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHAdminb.Checked)
            {
                code = "M205";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHltrTemp.Checked)
            {
                code = "M211";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHltrGen.Checked)
            {
                code = "M212";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHMCurrent.Checked)
            {
                code = "M213";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxHMPilot.Checked)
            {
                code = "M214";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }   
            if (cbxIPBA.Checked)
            {
                code = "M300";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIPImport.Checked)
            {
                code = "M301";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIPReport.Checked)
            {
                code = "M302";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIPAdjustments.Checked)
            {
                code = "M303";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIPMaintenance.Checked)
            {
                code = "M304";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWA.Checked)
            {
                code = "M400";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWAImprt.Checked)
            {
                code = "M401";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWATrans.Checked)
            {
                code = "M402";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWABal.Checked)
            {
                code = "M403";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWAAdj.Checked)
            {
                code = "M404";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWACases.Checked)
            {
                code = "M405";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxVWAMaintenance.Checked)
            {
                code = "M406";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIMP.Checked)
            {
                code = "M500";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIMPcalc.Checked)
            {
                code = "M501";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxIMPMaintenance.Checked)
            {
                code = "M502";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
         
            if (cbxYEB.Checked)
            {
                code = "M600";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }

            if (cbxYEBUpdateSSN.Checked)
            {
                code = "M601";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxYEBImports.Checked)
            {
                code = "M602";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxYEBReports.Checked)
            {
                code = "M603";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }
            if (cbxDsktp.Checked)
            {
                code = "M700";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }

            if (cbxDsktpTower.Checked)
            {
                code = "M701";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }

            if (cbxDsktpRetPart.Checked)
            {
                code = "M702";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();

            }
            if (cbxDsktpHipCert.Checked)
            {
                code = "M703";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();

            }
            if (cbxDsktpEBAworkorder.Checked)
            {
                code = "M704";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }

            if (cbxDsktpTermFiles.Checked)
            {
                code = "M705";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();
            }

            if (cbxDsktpSecurityLog.Checked)
            {
                code = "M706";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();

            }

            if (cbxDsktpPCaudit.Checked)
            {
                code = "M707";
                cmd2 = new SqlCommand(cmdStr1, conn);
                cmd2.Parameters.Add("@code", SqlDbType.Char);
                cmd2.Parameters["@code"].Value = code;
                mid.Add(Convert.ToInt32(cmd2.ExecuteScalar()));
                cmd2.Dispose();

            }


       
            for (int i = 0; i < mid.Count; i++)
            {
                SqlCommand cmd3 = new SqlCommand(cmdStr3, conn);
                cmd3.Parameters.Add("@role", SqlDbType.Int);
                cmd3.Parameters["@role"].Value = roleid;
                cmd3.Parameters.Add("@mid", SqlDbType.Int);
                cmd3.Parameters["@mid"].Value = Convert.ToInt32(mid[i]);

                cmd3.ExecuteNonQuery();
                cmd3.Dispose();
            }


        }
        catch (Exception ex)
        {
        }
        finally
        {
            conn.Close();
            enabledisableApp("disable");
            check_app();
        }
    }
    protected void btnAppCancel_Click(object sender, EventArgs e)
    {
        check_app();
        enabledisableApp("disable");
    }

    protected void anthem_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxAnth.Checked;
        cbxBilling.Checked = _checked;
        cbxClaims.Checked = _checked;
        cbxAMaintenance.Checked = _checked;
        cbxReport.Checked = _checked;
    }

    protected void HRA_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxHRA.Checked;
        cbxHMain.Checked = _checked;
        cbxHOper.Checked = _checked;
        cbxHRecon.Checked = _checked;
        cbxHLtr.Checked = _checked;
        cbxHAdminb.Checked = _checked;
        cbxHltrTemp.Checked = _checked;
        cbxHltrGen.Checked = _checked;
        cbxHMCurrent.Checked = _checked;
        cbxHMPilot.Checked = _checked;
    }

    protected void HRALtr_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxHLtr.Checked;
        cbxHltrTemp.Checked = _checked;
        cbxHltrGen.Checked = _checked;
    }

    protected void HRAM_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxHMain.Checked;
        cbxHMCurrent.Checked = _checked;
        cbxHMPilot.Checked = _checked;
    }

    protected void IPBA_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxIPBA.Checked;
        cbxIPImport.Checked = _checked;
        cbxIPReport.Checked = _checked;
        cbxIPAdjustments.Checked = _checked;
        cbxIPMaintenance.Checked = _checked;
    }

    protected void VWA_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxVWA.Checked;
        cbxVWAImprt.Checked = _checked;
        cbxVWATrans.Checked = _checked;
        cbxVWABal.Checked = _checked;
        cbxVWAAdj.Checked = _checked;
        cbxVWACases.Checked = _checked;
        cbxVWAMaintenance.Checked = _checked;
    }
    
    protected void IMP_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxIMP.Checked;
        cbxIMPcalc.Checked = _checked;
        cbxIMPMaintenance.Checked = _checked;
    }
   
    protected void YEB_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxYEB.Checked;
        cbxYEBUpdateSSN.Checked = _checked;
        cbxYEBImports.Checked = _checked;
        cbxYEBReports.Checked = _checked;
    }
    
    protected void Dsktp_OnChecked(object sender, EventArgs e)
    {
        bool _checked = cbxDsktp.Checked;
        cbxDsktpTower.Checked = _checked;
        cbxDsktpRetPart.Checked = _checked;
        cbxDsktpHipCert.Checked = _checked ;
        cbxDsktpEBAworkorder.Checked = _checked;
        cbxDsktpTermFiles.Checked = _checked;
        cbxDsktpSecurityLog.Checked = _checked;
        cbxDsktpPCaudit.Checked = _checked;

    }
    protected void enabledisableApp(string type)
    {
        switch (type)
        {
            case "enable":  cbxAdmin.Enabled = true;
                            cbxAMaintenance.Enabled = true;
                            cbxAnth.Enabled = true;
                            cbxAudit.Enabled = true;
                            cbxBilling.Enabled = true;
                            cbxClaims.Enabled = true;
                            cbxReport.Enabled = true;
                            cbxHMain.Enabled = true;
                            cbxHOper.Enabled = true;
                            cbxHRA.Enabled = true;
                            cbxHRecon.Enabled = true;
                            btnAppUpdate.Visible = true;
                            btnAppCancel.Visible = true;
                            cbxIPBA.Enabled = true;
                            cbxIPImport.Enabled = true;
                            cbxIPReport.Enabled = true;
                            cbxIPAdjustments.Enabled = true;
                            cbxIPMaintenance.Enabled = true;
                            cbxHLtr.Enabled = true;
                            cbxHltrGen.Enabled = true;
                            cbxHltrTemp.Enabled = true;
                            cbxHMCurrent.Enabled = true;
                            cbxHMPilot.Enabled = true;
                            cbxHAdminb.Enabled = true;
                            cbxVWAMaintenance.Enabled = true;
                            cbxVWAImprt.Enabled = true;
                            cbxVWACases.Enabled = true;
                            cbxVWABal.Enabled = true;
                            cbxVWAAdj.Enabled = true;
                            cbxVWA.Enabled = true;
                            cbxVWATrans.Enabled = true;
                            cbxIMP.Enabled = true;
                            cbxIMPcalc.Enabled = true;
                            cbxIMPMaintenance.Enabled = true;
                            cbxYEB.Enabled = true ;
                            cbxYEBUpdateSSN.Enabled = true;
                            cbxYEBImports.Enabled = true;
                            cbxYEBReports.Enabled = true;
                            cbxDsktp.Enabled = true;
                            cbxDsktpTower.Enabled = true;
                            cbxDsktpRetPart.Enabled = true;
                            cbxDsktpHipCert.Enabled = true;
                            cbxDsktpEBAworkorder.Enabled = true;
                            cbxDsktpTermFiles.Enabled = true;
                            cbxDsktpSecurityLog.Enabled = true;
                            cbxDsktpPCaudit.Enabled = true;

                            break;

            case "disable": btnAppUpdate.Visible = true;
                            btnAppCancel.Visible = true;
                            cbxAdmin.Enabled = false;
                            cbxAMaintenance.Enabled = false;
                            cbxAnth.Enabled = false;
                            cbxAudit.Enabled = false;
                            cbxBilling.Enabled = false;
                            cbxClaims.Enabled = false;
                            cbxReport.Enabled = false;
                            cbxHMain.Enabled = false;
                            cbxHOper.Enabled = false;
                            cbxHRA.Enabled = false;
                            cbxHRecon.Enabled = false;
                            cbxIPBA.Enabled = false;
                            cbxIPImport.Enabled = false;
                            cbxIPReport.Enabled = false;
                            cbxIPAdjustments.Enabled = false;
                            cbxIPMaintenance.Enabled = false;
                            cbxHLtr.Enabled = false;
                            cbxHltrGen.Enabled = false;
                            cbxHltrTemp.Enabled = false;
                            cbxHMCurrent.Enabled = false;
                            cbxHMPilot.Enabled = false;
                            cbxHAdminb.Enabled = false;
                            cbxVWAMaintenance.Enabled = false;
                            cbxVWAImprt.Enabled = false;
                            cbxVWACases.Enabled = false;
                            cbxVWABal.Enabled = false;
                            cbxVWAAdj.Enabled = false;
                            cbxVWA.Enabled = false;
                            cbxVWATrans.Enabled = false;
                            cbxIMP.Enabled = false;
                            cbxIMPcalc.Enabled = false;
                            cbxIMPMaintenance.Enabled = false;
                            cbxYEB.Enabled = false;
                            cbxYEBUpdateSSN.Enabled = false;
                            cbxYEBImports.Enabled = false;
                            cbxYEBReports.Enabled = false;
                            cbxDsktp.Enabled = false;
                            cbxDsktpTower.Enabled = false;
                            cbxDsktpRetPart.Enabled = false;
                            cbxDsktpHipCert.Enabled = false;
                            cbxDsktpEBAworkorder.Enabled = false;
                            cbxDsktpTermFiles.Enabled = false;
                            cbxDsktpSecurityLog.Enabled = false;
                            cbxDsktpPCaudit.Enabled = false;
                            break;
        }

         
    }

    protected void check_app()
    {
        SqlConnection conn = new SqlConnection(connStr);
        string cmdStr = "SELECT RoleID FROM Roles_AD WHERE RoleName = @role";
        string cmdStr1 = "SELECT MCode FROM Page_AD p, RoleOperations_AD r WHERE r.RoleID = @role and r.MID = p.MID and OID = 1";
        int roleid = 0;
        if (conn == null || conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }
        try
        {
            cbxAdmin.Checked = false;
            cbxAMaintenance.Checked = false;
            cbxAnth.Checked = false;
            cbxBilling.Checked = false;
            cbxClaims.Checked = false;
            cbxReport.Checked = false;
            cbxAMaintenance.Checked = false;
            cbxHRA.Checked = false;
            cbxHMain.Checked = false;
            cbxHRecon.Checked = false;
            cbxHOper.Checked = false;
            cbxHLtr.Checked = false;
            cbxHAdminb.Checked = false;
            cbxHltrTemp.Checked = false;
            cbxHltrGen.Checked = false;
            cbxHMCurrent.Checked = false;
            cbxHMPilot.Checked = false;
            cbxIPBA.Checked = false;
            cbxIPImport.Checked = false;
            cbxIPReport.Checked = false;
            cbxIPAdjustments.Checked = false;
            cbxIPMaintenance.Checked = false;
            cbxVWAMaintenance.Enabled = false;
            cbxVWAImprt.Enabled = false;
            cbxVWACases.Enabled = false;
            cbxVWABal.Enabled = false;
            cbxVWAAdj.Enabled = false;
            cbxVWA.Enabled = false;
            cbxVWATrans.Enabled = false;
            cbxIMP.Checked = false;
            cbxIMPcalc.Checked = false;
            cbxIMPMaintenance.Checked = false;
            cbxYEB.Checked = false;
            cbxYEBUpdateSSN.Checked = false;
            cbxYEBImports.Checked = false;
            cbxYEBReports.Checked = false;
            cbxDsktp.Checked = false;
            cbxDsktpTower.Checked = false;
            cbxDsktpRetPart.Checked = false;
            cbxDsktpHipCert.Checked = false;
            cbxDsktpEBAworkorder.Checked = false;
            cbxDsktpTermFiles.Checked = false;
            cbxDsktpSecurityLog.Checked = false;
            cbxDsktpPCaudit.Checked = false;
          
            SqlCommand cmd = new SqlCommand(cmdStr, conn);
            cmd.Parameters.Add("@role", SqlDbType.VarChar);
            cmd.Parameters["@role"].Value = ddlRoleName.SelectedItem.Text.ToString();
            roleid = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand(cmdStr1, conn);
            cmd1.Parameters.Add("@role", SqlDbType.Int);
            cmd1.Parameters["@role"].Value = roleid;

            SqlDataReader dr;
            dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                string module = Convert.ToString(dr[0]).Trim();
                switch (module)
                {
                    case "M1": cbxAdmin.Checked = true;
                        break;
                    case "M2": cbxAudit.Checked = true;
                        break;
                    case "M100": cbxAnth.Checked = true;
                        break;
                    case "M101": cbxBilling.Checked = true;
                        break;
                    case "M102": cbxClaims.Checked = true;
                        break;
                    case "M103": cbxReport.Checked = true;
                        break;
                    case "M104": cbxAMaintenance.Checked = true;
                        break;
                    case "M200": cbxHRA.Checked = true;
                        break;
                    case "M201": cbxHMain.Checked = true;
                        break;
                    case "M202": cbxHRecon.Checked = true;
                        break;
                    case "M203": cbxHOper.Checked = true;
                        break;
                    case "M204": cbxHLtr.Checked = true;
                        break;
                    case "M205": cbxHAdminb.Checked = true;
                        break;
                    case "M211": cbxHltrTemp.Checked = true;
                        break;
                    case "M212": cbxHltrGen.Checked = true;
                        break;
                    case "M213": cbxHMCurrent.Checked = true;
                        break;
                    case "M214": cbxHMPilot.Checked = true;
                        break;
                    case "M300": cbxIPBA.Checked = true;
                        break;
                    case "M301": cbxIPImport.Checked = true;
                        break;
                    case "M302": cbxIPReport.Checked = true;
                        break;
                    case "M303": cbxIPAdjustments.Checked = true;
                        break;
                    case "M304": cbxIPMaintenance.Checked = true;
                        break;
                    case "M400": cbxVWA.Checked = true;
                        break;
                    case "M401": cbxVWAImprt.Checked = true;
                        break;
                    case "M402": cbxVWATrans.Checked = true;
                        break;
                    case "M403": cbxVWABal.Checked = true;
                        break;
                    case "M404": cbxVWAAdj.Checked = true;
                        break;
                    case "M405": cbxVWACases.Checked = true;
                        break;
                    case "M406": cbxVWAMaintenance.Checked = true;
                        break;
                    case "M500": cbxIMP.Checked = true;
                        break;
                    case "M501": cbxIMPcalc.Checked = true;
                        break;
                    case "M502": cbxIMPMaintenance.Checked = true;
                        break;
                    case "M600": cbxYEB.Checked = true;
                        break;
                    case "M601": cbxYEBUpdateSSN.Checked = true;
                        break;
                    case "M602": cbxYEBImports.Checked = true;
                        break;
                    case "M603": cbxYEBReports.Checked = true;
                        break;
                    case "M700": cbxDsktp.Checked = true;
                        break;
                    case "M701": cbxDsktpTower.Checked = true;
                        break;
                    case "M702": cbxDsktpRetPart.Checked = true;
                        break;
                    case "M703": cbxDsktpHipCert.Checked = true;
                        break;
                    case "M704": cbxDsktpEBAworkorder.Checked = true;
                        break;
                    case "M705": cbxDsktpTermFiles.Checked = true;
                        break;
                    case "M706": cbxDsktpSecurityLog.Checked = true;
                        break;
                    case "M707": cbxDsktpPCaudit.Checked = true;
                        break;
                }
            }
            dr.Close();

        }
        catch (Exception e)
        {
        }
        finally
        {
            conn.Close();
        }
    }
}

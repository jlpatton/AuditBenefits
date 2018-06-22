<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="console.aspx.cs" Inherits="Admin_Console1" Title="Admin Console" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contentleft">     
        <div id = "verticalmenu">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/side_tab_up.gif"/>
            <ul>
                <li><asp:HyperLink ID="hypHome" runat="server" Font-Bold="false" NavigateUrl="~/Home.aspx" >HOME</asp:HyperLink></li>
                <%--<li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="" >HRA</asp:HyperLink></li>--%>
                <li><asp:HyperLink ID="hypVWA" runat="server" Font-Bold="false" NavigateUrl="~/VWA/Home.aspx">VWA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="~/HRA/Home.aspx" >HRA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypANTHEM" runat="server" Font-Bold="false" NavigateUrl="~/Anthem/Home.aspx">ANTHEM</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypIPBA" runat="server" Font-Bold="false" NavigateUrl="~/IPBA/Home.aspx">IPBA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypIMPUTED" runat="server" Font-Bold="false" NavigateUrl="~/ImputedIncome/Home.aspx">IMPUTED</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypYEB" runat="server" Font-Bold="false" NavigateUrl="~/YEB/Home.aspx">YEB</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypWorkOrder" runat="server" Font-Bold="false" NavigateUrl="~/WorkOrder/default.aspx">WORK ORDER</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypEBADSKTP" runat="server" Font-Bold="false" NavigateUrl="~/DsktpHist/Home.aspx">EBA Desktop</asp:HyperLink></li>
               <li><asp:HyperLink ID="hypADMIN" runat="server" Font-Bold="false" NavigateUrl="~/Admin/console.aspx">ADMIN</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAudit" runat="server" Font-Bold="false" NavigateUrl="">AUDIT</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="false" NavigateUrl="~/Reports/Output_Report.aspx">OUTPUT</asp:HyperLink></li></ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />  
        </div>
              
    </div>
    <div id="contentright"> 
        <div id = "introText">Admin Console</div>          
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Add Users" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Users Access" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Create Roles" Value="2"></asp:MenuItem>     
                    <asp:MenuItem Text="Manage Roles" Value="3"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                    <div class="userPara">
                        <p>Here you can add users to the list of users who can access this application.
                        Search users by their Employee id.</p>    
                    </div>                               
                    <div id="divAddUser">
                        <asp:Label ID="lblEmpno" runat="server" Text="Employee number:" style="display: block;width:120px;float: left;margin-bottom: 10px;"></asp:Label>
                        <asp:TextBox ID="tbxEmpno" runat="server" style="display: block;width: 150px;float: left;margin-bottom:10px;"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" style="display: block;width: 100px;float: left;margin-bottom: 10px;margin-left:10px;" OnClick="btnSearch_Click"/><br />
                        <div id ="ldapUserInfo" runat="server"> 
                            <div class="userForm">
                                <center><h3 class="headerStyle">User Information</h3></center>
                                <div style="float:left;width:500px;margin-left:100px;">                         
                                    <asp:Label ID="lblLast1" runat="server" Text="LastName:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblLast" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:10px;"></asp:Label><br />
                                    <asp:Label ID="lblFirst1" runat="server" Text="FirstName:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblFirst" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:10px;"/><br />
                                    <asp:Label ID="lblEmail1" runat="server" Text="Email:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblEmail" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:10px;"/><br />
                                    <asp:Label ID="lblDepNo1" runat="server" Text="Department Number:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblDepNo" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:20px;"/><br />
                                    <asp:Label ID="lblDepName1" runat="server" Text="Department Name:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblDepName" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:10px;"/><br />
                                    <asp:Label ID="lblManager1" runat="server" Text="Manager Id:" style="display: block;width:150px;float:left;margin-bottom:5px;font-size:10pt;font-weight:bold;"/>
                                    <asp:Label ID="lblManager" runat="server" Text="" style="display: block;width:350px;float:left;margin-bottom:10px;"/><br />
                                </div> 
                                <div style="float:left;width:600px;margin-left:120px;margin-top:10px;">
                                    <asp:Button ID="btnAdd" runat="server" Text="Add User" style="display: block;width: 100px;float: left;margin-bottom: 10px;margin-left:10px;" OnClick="btnAdd_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" style="display: block;width: 100px;float: left;margin-bottom: 10px;margin-left:10px;" OnClick="btnCancel_Click" />             
                                </div> 
                            </div>                                                  
                        </div> 
                        <div style="float:left;width:600px;margin-left:120px;color:red">
                            <asp:Label ID="lblBeforeAdd" runat="server" Text="" Visible="false"></asp:Label>
                        </div>
                        <div id="userAdded" runat="server">
                            <asp:Label ID="lblafterAdd" runat="server" Text="" style="float:left;font-weight:bold;color:red;width:650px"></asp:Label>
                        </div>
                    </div> 
                </asp:view>
                 <asp:View ID="View2" runat="server"> 
                    <div id = "introText">List of Users</div> 
                    <div class="userPara">
                        <p>List of users who have access to this application.Click 'Edit' to change access rights for the user.<br />
                           Active - User is on the list of approved users to access application and is active.Uncheck 'Active' to
                           make user inactive(User will be on the list of approved user but temperoraly inactive and cant login into
                           application).                            
                        </p>    
                    </div> 
                    <div id="divAccessUser">
                        <asp:GridView ID="grdUser" 
                        AutoGenerateColumns="False"
                        GridLines="None"
                        CssClass="customGrid" PagerSettings-Mode="NextPreviousFirstLast"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"
                        runat="server" AutoGenerateEditButton="True" DataKeyNames="empID"
                        AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource1">
                        <Columns>                                
                            <asp:BoundField DataField="empID" ReadOnly="true" HeaderText="Employee Id"  SortExpression ="empID"/>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression ="UserName"/>
                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" SortExpression="IsActive"  />
                            <asp:CheckBoxField DataField="isLocked" ReadOnly="true" HeaderText="Locked" SortExpression="isLocked"  />
                         </Columns>
                        <PagerStyle CssClass="customGridpaging" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand = " SELECT [empID], [UserName], [IsActive],[isLocked] FROM [UserLogin_AD]"
                            UpdateCommand = " UPDATE  [UserLogin_AD] SET [IsActive] = @IsActive,[UserName] = @UserName WHERE [empID] = @empID"> </asp:SqlDataSource>
                    </div>
                    
                    <div id = "introText">Locked Users</div> 
                    <div class="userPara">
                        <p>List of Users whose accounts are locked after 3 invalid login attempts.
                            Click edit and uncheck the Locked field to unlock the user.                            
                        </p>    
                    </div> 
                    <div id="divAccessUser">                    
                    <asp:GridView ID="GridView1" 
                        AutoGenerateColumns="False"
                        GridLines="None"
                        CssClass="customGrid" PagerSettings-Mode="NextPreviousFirstLast"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"
                        runat="server" AutoGenerateEditButton="True" DataKeyNames="empID"
                        AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource5">
                        <Columns>                                
                            <asp:BoundField DataField="empID" ReadOnly="true" HeaderText="Employee Id"  SortExpression ="empID"/>
                            <asp:BoundField DataField="UserName" ReadOnly="true" HeaderText="User Name" SortExpression ="UserName"/>
                            <asp:CheckBoxField DataField="isLocked" HeaderText="Locked" SortExpression="isLocked"  />
                        </Columns>
                        <PagerStyle CssClass="customGridpaging" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT [empID], [UserName], [isLocked] FROM [UserLogin_AD] WHERE [isLocked] = 1"
                            UpdateCommand ="UPDATE  [UserLogin_AD] SET [isLocked] = @isLocked, [iAttempt] = 0 WHERE [empID] = @empID">
                            <UpdateParameters>
                                <asp:Parameter Name="isLocked" Type = "Int32" />
                                <asp:Parameter Name="iAttempt" Type = "Int32" />
                            </UpdateParameters>
                            </asp:SqlDataSource>
                    </div>                      
                </asp:view>
                <asp:View ID="View3" runat="server"> 
                    <div class="userPara">
                        <p>List of Roles defined for the application. Create new roles using 'Add New Role' button.
                           After creating role you can assign users to the role under 'Manage Roles' tab and also define
                           rule for the role for application access.                         
                        </p>    
                    </div> 
                    <div id="divRoles">
                        <asp:GridView ID="grdRole" 
                        AutoGenerateColumns="False"
                        GridLines="None"
                        CssClass="customGrid" AutoGenerateDeleteButton="True"
                        runat="server" DataKeyNames="RoleID"  PagerSettings-Mode="NextPreviousFirstLast"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"                   
                        AllowPaging="True" AllowSorting="True" DataSourceID="SqlDataSource2">
                            <Columns>
                                <asp:BoundField DataField="RoleID" HeaderText="Role ID" />
                                <asp:BoundField DataField="RoleName" HeaderText="Role Name" SortExpression="RoleName" />
                                <asp:BoundField DataField="RoleDesc" HeaderText="Role Description" SortExpression="RoleDesc" />
                            </Columns>
                            <PagerStyle CssClass="customGridpaging" />
                        </asp:GridView>    
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT [RoleID],[RoleName], [RoleDesc] FROM [Roles_AD]"
                            DeleteCommand="DELETE FROM [Roles_AD] WHERE [RoleID] = @RoleID"></asp:SqlDataSource>
                        <br />
                        <custom:OpenWebWindow
                            id="lnkOpen"
                            Text="Add New Role"
                            WebWindowID="winAdd"
                            Runat="server" />
                        <custom:WebWindow
                            id="winAdd"
                            WindowTitleText="Add New Role"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                                <asp:FormView
                                id="frmAdd"
                                DefaultMode="Insert"
                                DataSourceID="srcRoles"
                                Runat="server" OnItemInserted="frmAdd_ItemInserted" OnItemCommand="frmAdd_ItemCommand">
                                <InsertItemTemplate>
                                <div class="scrollBox" style="height:300px">    
                                    <asp:Label
                                        id="lblName"
                                        Text="Role Name:"
                                        AssociatedControlID="txtName"
                                        Runat="server" />
                                    <asp:RequiredFieldValidator
                                        id="reqName"
                                        ControlToValidate="txtName"
                                        Text="(Required)"
                                        ValidationGroup="Add"
                                        Runat="server" />    
                                    <div class="instructions">
                                        Enter the name of the Role.
                                    </div>
                                    <asp:TextBox
                                        id="txtName"
                                        Text='<%# Bind("RoleName") %>'
                                        Runat="server" />    
                                    <br /><br />                                
                                    <asp:Label
                                        id="lblDescription"
                                        Text="Brief Description:"
                                        AssociatedControlID="txtDescription"
                                        Runat="server" />                                
                                    <div class="instructions">
                                        The brief description about the role
                                    </div>
                                    <asp:TextBox
                                        id="txtDescription"
                                        Text='<%#Bind("RoleDesc")%>'
                                        TextMode="multiLine"
                                        Columns="40"
                                        Rows="2"
                                        Runat="server" />    
                                    <br /><br />                                        
                                </div>
                                <br />
                                <asp:Button 
                                    id="btnAdd"
                                    Text="Add Role"
                                    CommandName="Insert"
                                    ValidationGroup="Add"
                                    Runat="server" />
                                <asp:Button 
                                    id="btnCancel"
                                    Text="Cancel"
                                    CausesValidation="false"
                                    CommandName="Cancel"
                                    Runat="server" />
                                </InsertItemTemplate>
                            </asp:FormView>   
                        </custom:WebWindow>
                            <asp:ObjectDataSource
                            id="srcRoles"
                            TypeName="EBA.Desktop.InsertRole"                            
                            InsertMethod="Insert" 
                            Runat="server" />
                    </div>                                                                                                         
                </asp:View>
                <asp:View ID="View4" runat="server"> 
                    <div class="userPara">
                        <p>Add users to the roles. Delete users from roles.Add and modify modules/application rights to roles.                         
                        </p>    
                    </div> 
                    <div id="rolesManage">
                        <asp:Label ID="lblRoleName" runat="server" Text="Role:"></asp:Label>
                        <asp:DropDownList ID="ddlRoleName" runat="server" DataSourceID="SqlDataSource3" 
                        AutoPostBack="true" DataTextField="RoleName" DataValueField="RoleName" OnSelectedIndexChanged="ddlRoleName_OnselectedIndexchange">
                        </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT [RoleName] FROM [Roles_AD]"></asp:SqlDataSource>
                        <div style="float:left;margin-top:10px;">
                            <asp:GridView ID="grdRoleUser" runat="server" CssClass="customGrid"
                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                            DataSourceID="SqlDataSource4" PagerSettings-Mode="NextPreviousFirstLast"
                            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  >
                            <Columns>
                                <asp:BoundField DataField="empID" HeaderText="Employee Id" SortExpression="empID" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:CheckBoxField DataField="IsActive" HeaderText="Active" SortExpression="IsActive" />
                            </Columns>
                            <PagerStyle CssClass="customGridpaging" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT u.empID,u.UserName, u.IsActive FROM Roles_AD AS r INNER JOIN UserRoles_AD AS l ON r.RoleID = l.RoleID INNER JOIN UserLogin_AD AS u ON l.UserID = u.UserId WHERE (r.RoleName = @RoleName)">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlRoleName" Name="RoleName" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div id="Rolelnk">
                            <asp:LinkButton ID="lnkAddRoleUser" runat="server" CssClass="lnkButton" OnClick="lnkaddRuser">Add user</asp:LinkButton>
                            <asp:LinkButton ID="lnkRemoveRoleUser" runat="server" CssClass="lnkButton" OnClick="lnkremRuser">Remove user</asp:LinkButton>
                            <asp:LinkButton ID="lnkApplicationRole" runat="server" CssClass="lnkButton" OnClick="lnkaddRapp">Applications/Module Access</asp:LinkButton>
                        </div>
                        <asp:Label ID="lblUserAdmin" runat="server" Text="" ForeColor="red" style="float:left;" Visible="false"></asp:Label>
                        <asp:Label ID="lblErr1" runat="server" Text="" ForeColor="red"></asp:Label>
                    </div>
                    <div id="addremRoleUser" runat="server">
                        <asp:Label ID="lblRem" runat="server" Text=""></asp:Label>
                            <table>
                                <tr>
                                    <td style="width:200px"> 
                                        <asp:ListBox ID="lstUsers1" runat="server" Width="180px" Height="200px"></asp:ListBox>
                                    </td>
                                    <td valign="middle" style="width:100px;">
                                        <asp:Button ID="btnRSel" Width="80px"  runat="server" Text="Select" OnClick="btnRSel_Click" /><br /><br />
                                        <asp:Button ID="btnDSel" Width="80px" runat="server" Text="DeSelect" OnClick="btnDSel_Click" /><br /><br />
                                        <asp:Button ID="btnClear1" Width="80px" runat="server" Text="Clear" OnClick="btnClear1_Click" />
                                    </td>
                                    <td style="width:200px">
                                        <asp:ListBox ID="lstUsers2" runat="server" Width="180px" Height="200px"></asp:ListBox>
                                    </td>
                                    <td valign="middle" style="width:100px">
                                        <asp:Button ID="btnOK1"  runat="server" Width="80px" Text="OK" OnClick="btnOK_Click" /><br /><br />
                                        <asp:Button ID="Button1"  runat="server" Width="80px" Text="Cancel" OnClick="btnCancelrole_Click" />
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </td>
                                </tr>
                            </table>                                                  
                    </div>                                  
                     <div id="addRoleApp" runat="server">
                        <asp:Label ID="lblMod" runat="server" Text="Application Access Information" style="margin-left:10px"></asp:Label>
                        <div id="adminApp">
                            <ul>
                                <li>HOME
                                    <ul>
                                        <li>
                                            <asp:CheckBox ID="cbxAdmin" runat="server" Enabled="false" Checked="false" />Admin
                                        </li>
                                        <li>
                                            <asp:CheckBox ID="cbxAudit" runat="server" Enabled="false" Checked="false" />Audit
                                        </li>
                                        <li>
                                            <asp:CheckBox ID="cbxAnth" runat="server" AutoPostBack="true" OnCheckedChanged="anthem_OnChecked"  Enabled="false" Checked="false" />Anthem
                                                <ul>
                                                    <li>
                                                        <asp:CheckBox ID="cbxBilling" runat="server" Enabled="false" Checked="false" />Billing
                                                    </li>
                                                    <li>
                                                        <asp:CheckBox ID="cbxClaims" runat="server" Enabled="false" Checked="false" />Claims
                                                    </li>
                                                    <li>
                                                        <asp:CheckBox ID="cbxReport" runat="server" Enabled="false" Checked="false" />Reports
                                                    </li>
                                                    <li>
                                                        <asp:CheckBox ID="cbxAMaintenance" runat="server" Enabled="false" Checked="false" />Maintenance
                                                    </li>
                                                </ul>
                                        </li>
                                        <li><asp:CheckBox ID="cbxHRA" runat="server" AutoPostBack="true" OnCheckedChanged="HRA_OnChecked" Enabled="false" Checked="false" />HRA
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxHOper" runat="server" Enabled="false" Checked="false" />Operations
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxHRecon" runat="server" Enabled="false" Checked="false" />Reconciliation
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxHAdminb" runat="server" Enabled="false" Checked="false" />Admin Bill Validation
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxHLtr" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="HRALtr_OnChecked" Checked="false" />Letters
                                                        <ul>
                                                            <li>
                                                                <asp:CheckBox ID="cbxHltrTemp" runat="server" Enabled="false" Checked="false" />Letter Templates
                                                            </li>
                                                             <li>
                                                                <asp:CheckBox ID="cbxHltrGen" runat="server" Enabled="false" Checked="false" />Generate Letter
                                                            </li>
                                                        </ul>
                                                </li> 
                                                <li>
                                                    <asp:CheckBox ID="cbxHMain" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="HRAM_OnChecked" Checked="false" />Maintenance
                                                        <ul>
                                                            <li>
                                                                <asp:CheckBox ID="cbxHMCurrent" runat="server" Enabled="false" Checked="false" />Current Module
                                                            </li>
                                                             <li>
                                                                <asp:CheckBox ID="cbxHMPilot" runat="server" Enabled="false" Checked="false" />Search Pilot
                                                            </li>
                                                        </ul>
                                                </li> 
                                            </ul>
                                        </li>
                                         <li><asp:CheckBox ID="cbxIPBA" runat="server" AutoPostBack="true" OnCheckedChanged="IPBA_OnChecked" Enabled="false" Checked="false" />IPBA
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxIPImport" runat="server" Enabled="false" Checked="false" />Imports
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxIPReport" runat="server" Enabled="false" Checked="false" />Reports
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxIPAdjustments" runat="server" Enabled="false" Checked="false" />Adjustments
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxIPMaintenance" runat="server" Enabled="false" Checked="false" />Maintenance
                                                </li>
                                            </ul>
                                        </li>
                                        <li><asp:CheckBox ID="cbxVWA" runat="server" AutoPostBack="true" OnCheckedChanged="VWA_OnChecked" Enabled="false" Checked="false" />VWA
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWAImprt" runat="server" Enabled="false" Checked="false" />Imports
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWATrans" runat="server" Enabled="false" Checked="false" />Transaction Report
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWABal" runat="server" Enabled="false" Checked="false" />Balancing
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWAAdj" runat="server" Enabled="false" Checked="false" />Adjustments
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWACases" runat="server" Enabled="false" Checked="false" />Individual Cases
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxVWAMaintenance" runat="server" Enabled="false" Checked="false" />Maintenance
                                                </li>
                                            </ul>
                                        </li>
                                        <li><asp:CheckBox ID="cbxIMP" runat="server" AutoPostBack="true" OnCheckedChanged="IMP_OnChecked" Enabled="false" Checked="false" />Imputed Income
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxIMPcalc" runat="server" Enabled="false" Checked="false" />Calculate Imputed Income
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxIMPMaintenance" runat="server" Enabled="false" Checked="false" />Maintenance
                                                </li>
                                            </ul>
                                        </li>
                                         <li><asp:CheckBox ID="cbxYEB" runat="server" AutoPostBack="true" OnCheckedChanged="YEB_OnChecked" Enabled="false" Checked="false" />YEB
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxYEBUpdateSSN" runat="server" Enabled="false" Checked="false" />Update SSN
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxYEBImports" runat="server" Enabled="false" Checked="false" />Imports
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxYEBReports" runat="server" Enabled="false" Checked="false" />Reports
                                                </li>
                                            </ul>
                                        </li>
                                         <li><asp:CheckBox ID="cbxDsktp" runat="server" AutoPostBack="true" OnCheckedChanged="Dsktp_OnChecked" Enabled="false" Checked="false" />EBA Desktop
                                            <ul>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpTower" runat="server" Enabled="false" Checked="false" />Tower Group
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpRetPart" runat="server" Enabled="false" Checked="false" />Retiree Health Data
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpHipCert" runat="server" Enabled="false" Checked="false" />HIPPA Certificates
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpEBAworkorder" runat="server" Enabled="false" Checked="false" />WorkOrders
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpTermFiles" runat="server" Enabled="false" Checked="false" />TermFiles
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpSecurityLog" runat="server" Enabled="false" Checked="false" />SecurityLog
                                                </li>
                                                <li>
                                                    <asp:CheckBox ID="cbxDsktpPCaudit" runat="server" Enabled="false" Checked="false" />PC Audit Data
                                                </li>
                                            </ul>
                                        </li>                                                                     
                                    </ul>
                                </li>
                            </ul>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td style="width:100px">
                                        <asp:LinkButton ID="lnlAppEdit" runat="server" OnClick="btnAppEdit_click">Edit</asp:LinkButton> 
                                        <asp:LinkButton ID="lnkAppClose" runat="server" OnClick="btnAppClose_click">Close</asp:LinkButton>
                                    </td>
                                    <td valign="middle">
                                         <asp:Button ID="btnAppUpdate" Width="80px" runat="server" Text="Update" Visible="false" OnClick="btnAppUpdate_Click" />
                                         <asp:Button ID="btnAppCancel" Width="80px" runat="server" Text="Cancel" Visible="false" OnClick="btnAppCancel_Click" />
                                    </td>
                                </tr>
                            </table>                                                                                  
                        </div>
                    </div>
                </asp:View>                 
            </asp:MultiView>                       
	    </div>                    
     </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


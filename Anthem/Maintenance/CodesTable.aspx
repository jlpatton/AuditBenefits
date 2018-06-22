<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CodesTable.aspx.cs" Inherits="Anthem_Maintenance_CodesTable1" Title="Carrier Plan Codes And Threshold values" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>ANTHEM</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Billing</a>
		            <ul>
		                <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/Anthem/Billing/Imports.aspx">Import</asp:HyperLink></li><li><asp:HyperLink ID="hypBillingRecon" runat="server" NavigateUrl="~/Anthem/Billing/Reconciliation.aspx">Reconciliation</asp:HyperLink></li></ul>
		        </li>
                <li><a>Claims</a>
                     <ul>
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server" NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li><li><asp:HyperLink ID="HypClaimsRecon" runat="server" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li></ul>
                </li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Anthem/Reports/Reports.aspx">Reports</asp:Hyperlink></li><li><a>Maintenance</a>
                    <ul id ="expand1">
		                <li><a>Module Maintenance</a>
		                    <ul id = "expand2">
		                        <li><asp:HyperLink ID="hypCodes" Font-Underline="true" runat="server" NavigateUrl="~/Anthem/Maintenance/CodesTable.aspx">Codes Table</asp:HyperLink></li><li><asp:HyperLink ID="hypConsole" runat="server" NavigateUrl="~/Anthem/Maintenance/ManagementConsole.aspx">Management Console</asp:HyperLink></li></ul>
		                </li>                        
                        <li><a>Current Users</a>
                            <ul>
                                <li><asp:HyperLink ID="hypDash" runat="server" NavigateUrl="~/Anthem/Maintenance/Dashboard.aspx">Dashboard</asp:HyperLink></li><li><asp:HyperLink ID="hypPref" runat="server" NavigateUrl="~/Anthem/Maintenance/Preferences.aspx">Preferences</asp:HyperLink></li></ul>
                        </li>		            
		            </ul>
		       </li>
		    </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
    <div id="contentright">
        <div id = "introText">Plan hierarchy Table with Rates And Threshold Table</div> 
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Plan Hierarchy" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Set Threshold" Value="1"></asp:MenuItem>  
                    <asp:MenuItem Text="Audit" Value="2"></asp:MenuItem> 
                    <asp:MenuItem Text="Audit Reports" Value="3"></asp:MenuItem>                                
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
                        <p>Select plan from the dropdown list to display plan hierarchy table and plan rates.
                           You can edit individual plans using edit options or Add plans using add option.                        
                        </p>    
                    </div>
                    <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                        <asp:Label ID="MessageLabel" runat="server" Text="" ForeColor="red"></asp:Label>
                    </div>
                    <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                        <custom:OpenWebWindow
                            id="lnkOpen"
                            Text="Add New Plan Code"
                            WebWindowID="winAddP"
                            Runat="server" />
                        <custom:WebWindow
                            id="winAddP"
                            WindowTitleText="Add New Plan Code"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                            <asp:FormView
                            id="frmAddP"
                            DefaultMode="Insert"
                            DataSourceID="srcInsertPlan"
                            Runat="server" OnItemInserted="frmAddP_ItemInserted" OnItemCommand="frmAddP_ItemCommand">
                            <InsertItemTemplate>
                                <div class="scrollBox" style="height:300px">    
                                <asp:Label
                                    id="lblPY"
                                    Text="Plan Year:"
                                    AssociatedControlID="txtPY"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="reqName"
                                    ControlToValidate="txtPY"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator9" 
                                    runat="server"
                                    ControlToValidate="txtPY"
                                    ValidationExpression="^\d{4}$"
                                    Display="Static" ValidationGroup="Add1"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter Plan Year in format 'yyyy'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                    Enter Plan year in format 'yyyy' (ex:2008)
                                </div>
                                <asp:TextBox
                                    id="txtPY"
                                    Text='<%# Bind("PlanYear") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPlantype"
                                    Text="Plan Description:"
                                    AssociatedControlID="ddlPDesc"
                                    Runat="server" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" 
                                    ControlToValidate="ddlPDesc"
                                    runat="server" 
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    InitialValue="--Select--">
                                </asp:RequiredFieldValidator>                              
                                <div class="instructions">
                                   Plan Description
                                </div> 
                                <asp:DropDownList ID="ddlPDesc"  
                                    runat="server"
                                    SelectedValue='<%# Bind("Desc") %>' >
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem>DOMESTIC - ACTIVE</asp:ListItem>
                                    <asp:ListItem>DOMESTIC - RETIREE</asp:ListItem>
                                    <asp:ListItem>DOMESTIC - COBRA</asp:ListItem>
                                    <asp:ListItem>EAP</asp:ListItem>
                                    <asp:ListItem>INTERNATIONAL</asp:ListItem>
                                </asp:DropDownList>
                                <br /><br />
                                <asp:Label
                                    id="lblPComments"
                                    Text="Comments:"
                                    AssociatedControlID="txtComments"
                                    Runat="server" />                                  
                                <div class="instructions">
                                   Comments for Plan Description
                                </div>
                                <asp:TextBox
                                    id="txtComments"
                                    Text='<%# Bind("Comm") %>'
                                    TextMode="multiLine"
                                    Columns="40"
                                    Rows="4"
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblEdate"
                                    Text="Plan Effective YRMO:"
                                    AssociatedControlID="txteDate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator2"
                                    ControlToValidate="txteDate"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator1" 
                                    runat="server"
                                    ControlToValidate="txteDate"
                                    ValidationExpression="^((\d{4}))(|(0[1-9])|(1[0-2]))$"
                                    Display="Static" ValidationGroup="Add1"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter YRMO in format 'yyyymm'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                    Enter effective YRMO in format 'yyyymm'
                                </div>
                                <asp:TextBox
                                    id="txteDate"
                                    Text='<%# Bind("YRMO") %>'
                                    Runat="server" /> 
                                <br /><br />
                                <asp:Label
                                    id="lblState"
                                    Text="State:"
                                    AssociatedControlID="stateDDL"
                                    Runat="server" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" 
                                    ControlToValidate="stateDDL"
                                    runat="server" 
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    InitialValue="--Select--">
                                </asp:RequiredFieldValidator>                              
                                <div class="instructions">
                                   Select State('CA or Non-CA')
                                </div> 
                                <asp:DropDownList ID="stateDDL"  
                                    runat="server"
                                    SelectedValue='<%# Bind("State") %>' >
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem>CA</asp:ListItem>
                                    <asp:ListItem>Non-CA</asp:ListItem>
                                </asp:DropDownList>
                                <br /><br />
                                <asp:Label
                                    id="lblMedType"
                                    Text="Medicare Type:"
                                    AssociatedControlID="medDDL"
                                    Runat="server" /> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" 
                                    ControlToValidate="medDDL"
                                    runat="server" 
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    InitialValue="--Select--">
                                </asp:RequiredFieldValidator>                                   
                                <div class="instructions">
                                    Select medicare type('Medicare or Non-Medicare')
                                </div> 
                                <asp:DropDownList ID="medDDL"  
                                    runat="server"
                                    SelectedValue='<%# Bind("Med") %>' >
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    <asp:ListItem>Medicare</asp:ListItem>
                                    <asp:ListItem>Non-Medicare</asp:ListItem>
                                </asp:DropDownList>
                                <br /><br />
                                <asp:Label
                                    id="lblApCode"
                                    Text="Carrier Plan Code:"
                                    AssociatedControlID="txtaPcode"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator3"
                                    ControlToValidate="txtaPcode"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" />    
                                <div class="instructions">
                                    Enter New Carrier Plan code (ex: M001, M002...)
                                </div>
                                <asp:TextBox
                                    id="txtaPcode"
                                    Text='<%# Bind("AnthemPlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblaTcode"
                                    Text="Carrier Coverage Code:"
                                    AssociatedControlID="txtaTcode"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator4"
                                    ControlToValidate="txtaTcode"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" />    
                                <div class="instructions">
                                    Enter New Carrier Coverage code (ex: S, FAM, 2P, S+DE...)
                                </div>
                                <asp:TextBox
                                    id="txtaTcode"
                                    Text='<%# Bind("AnthemTierCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPcode"
                                    Text="Prism Plan Code:"
                                    AssociatedControlID="txtPcode"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator5"
                                    ControlToValidate="txtPcode"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" />    
                                <div class="instructions">
                                    Enter Prism Plan code (ex: P0, P1,...)
                                </div>
                                <asp:TextBox
                                    id="txtPcode"
                                    Text='<%# Bind("PlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblTcode"
                                    Text="Prism Tier Code:"
                                    AssociatedControlID="txtTcode"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator6"
                                    ControlToValidate="txtTcode"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" />    
                                <div class="instructions">
                                    Enter Prism Tier Code (ex: E, S, D, F.....)
                                </div>
                                <asp:TextBox
                                    id="txtTcode"
                                    Text='<%# Bind("TierCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblRate"
                                    Text="Rate:"
                                    AssociatedControlID="txtRate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator7"
                                    ControlToValidate="txtRate"
                                    Text="(Required)"
                                    ValidationGroup="Add1"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator2" 
                                    runat="server"
                                    ControlToValidate="txtRate"
                                    ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                    Display="Static" ValidationGroup="Add1"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter Rate in format '000.00'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                   Enter new rate for the plan in format '000.00'
                                </div>
                                <asp:TextBox
                                    id="txtRate"
                                    Text='<%# Bind("Rate") %>'
                                    Runat="server" />    
                                <br /><br />
                                </div>
                                <asp:Button 
                                    id="btnAdd"
                                    Text="Add"
                                    CommandName="Insert"
                                    ValidationGroup="Add1"
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
                        <custom:WebWindow
                            id="winEdit"
                            WindowTitleText="Edit Domestic Plan Code"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                            <asp:FormView
                            id="frmEditD"
                            DefaultMode="Edit"
                            DataSourceID="srcEditDPlanhier" OnItemUpdating="grdvDom_RowUpdating"
                            Runat="server" OnItemCommand="frmEditD_ItemCommand">
                            <EditItemTemplate>
                                <div class="scrollBox" style="height:300px"> 
                                <asp:Label
                                    id="lblPid"
                                    Text="Plan ID:"
                                    AssociatedControlID="txtEdit"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator8"
                                    ControlToValidate="txtEdit"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan ID
                                </div>
                                <asp:TextBox
                                    id="txtEdit" ReadOnly="true"
                                    Text='<%# Bind("PlhrId") %>'
                                    Runat="server" />    
                                <br /><br />  
                                <asp:Label
                                    id="lblPY"
                                    Text="Plan Year:"
                                    AssociatedControlID="txtPY"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="reqName"
                                    ControlToValidate="txtPY"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan year in format 'yyyy' (ex:2008)
                                </div>
                                <asp:TextBox
                                    id="txtPY" ReadOnly="true"
                                    Text='<%# Bind("PlanYear") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPDesc"
                                    Text="Plan Description:"
                                    AssociatedControlID="txtDesc"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator1"
                                    ControlToValidate="txtDesc"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan Description
                                </div>
                                <asp:TextBox
                                    id="txtDesc" ReadOnly="true"
                                    Text='<%# Bind("Desc") %>'
                                    TextMode="multiLine"
                                    Columns="40"
                                    Rows="4"
                                    Runat="server" />    
                                <br /><br />                                 
                                <asp:Label
                                    id="lblApCode"
                                    Text="Carrier Plan Code:"
                                    AssociatedControlID="txtaPcode"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator3"
                                    ControlToValidate="txtaPcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Carrier Plan code
                                </div>
                                <asp:TextBox
                                    id="txtaPcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemPlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblaTcode"
                                    Text="Carrier Coverage Code:"
                                    AssociatedControlID="txtaTcode"
                                    Runat="server" />
                               <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator4"
                                    ControlToValidate="txtaTcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <div class="instructions">
                                    Carrier Coverage code
                                </div>
                                <asp:TextBox
                                    id="txtaTcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemTierCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPcode"
                                    Text="Prism Plan Code:"
                                    AssociatedControlID="txtPcode"
                                    Runat="server" />
                                <!--<asp:RequiredFieldValidator
                                    id="RequiredFieldValidator5"
                                    ControlToValidate="txtPcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  -->  
                                <div class="instructions">
                                    Prism Plan code
                                </div>
                                <asp:TextBox
                                    id="txtPcode" ReadOnly="true"
                                    Text='<%# Bind("PlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblTcode"
                                    Text="Prism Tier Code:"
                                    AssociatedControlID="txtTcode"
                                    Runat="server" />
                                <!--<asp:RequiredFieldValidator
                                    id="RequiredFieldValidator6"
                                    ControlToValidate="txtTcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  -->  
                                <div class="instructions">
                                    Prism Tier Code
                                </div>
                                <asp:TextBox
                                    id="txtTcode" ReadOnly="true"
                                    Text='<%# Bind("TierCode") %>'
                                    Runat="server" /> 
                                <br /><br />
                                <asp:Label
                                    id="lblEdate"
                                    Text="Plan Effective YRMO:"
                                    AssociatedControlID="txteDate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator2"
                                    ControlToValidate="txteDate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator3" 
                                    runat="server"
                                    ControlToValidate="txteDate"
                                    ValidationExpression="^((\d{4}))(|(0[1-9])|(1[0-2]))$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter YRMO in format 'yyyymm'
                                </asp:RegularExpressionValidator>     
                                <div class="instructions">
                                    Effective YRMO in format 'yyyymm'
                                </div>
                                <asp:TextBox
                                    id="txteDate"
                                    Text='<%# Bind("YRMO") %>'
                                    Runat="server" />     
                                <br /><br />
                                <asp:Label
                                    id="lblRate"
                                    Text="Rate:"
                                    AssociatedControlID="txtRate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator7"
                                    ControlToValidate="txtRate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator4" 
                                    runat="server"
                                    ControlToValidate="txtRate"
                                    ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter Rate in format '000.00'
                                </asp:RegularExpressionValidator>        
                                <div class="instructions">
                                   Rate for the plan code
                                </div>
                                <asp:TextBox
                                    id="txtRate"
                                    Text='<%# Bind("Rate") %>'
                                    Runat="server" />    
                                <br /><br />
                                </div>                                
                                <br />
                                <asp:Button 
                                    id="btnEdit"
                                    Text="Update Changes"
                                    CommandName="Update"
                                    ValidationGroup="Edit"
                                    Runat="server" />
                                <asp:Button 
                                    id="btnCancel"
                                    Text="Cancel"
                                    CausesValidation="false"
                                    CommandName="Cancel"
                                    Runat="server" />
                            </EditItemTemplate>                            
                            </asp:FormView>
                        </custom:WebWindow>
                        <custom:WebWindow
                            id="winEdit1"
                            WindowTitleText="Edit International Plan Code"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                            <asp:FormView
                            id="frmEditI"
                            DefaultMode="Edit"
                            DataSourceID="srcEditIPlanhier" OnItemUpdating="grdvIntl_RowUpdating"
                            Runat="server" OnItemCommand="frmEditI_ItemCommand">
                            <EditItemTemplate>
                                <div class="scrollBox" style="height:300px">  
                                <asp:Label
                                    id="lblPid"
                                    Text="Plan ID:"
                                    AssociatedControlID="txtEdit"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator8"
                                    ControlToValidate="txtEdit"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan ID
                                </div>
                                <asp:TextBox
                                    id="txtEdit" ReadOnly="true"
                                    Text='<%# Bind("PlhrId") %>'
                                    Runat="server" />    
                                <br /><br /> 
                                <asp:Label
                                    id="lblPY"
                                    Text="Plan Year:"
                                    AssociatedControlID="txtPY"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="reqName"
                                    ControlToValidate="txtPY"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan year in format 'yyyy' (ex:2008)
                                </div>
                                <asp:TextBox
                                    id="txtPY" ReadOnly="true"
                                    Text='<%# Bind("PlanYear") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPDesc"
                                    Text="Plan Description:"
                                    AssociatedControlID="txtDesc"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator1"
                                    ControlToValidate="txtDesc"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan Description
                                </div>
                                <asp:TextBox
                                    id="txtDesc" ReadOnly="true"
                                    Text='<%# Bind("Desc") %>'
                                    TextMode="multiLine"
                                    Columns="40"
                                    Rows="4"
                                    Runat="server" />    
                                <br /><br />                                
                                <asp:Label
                                    id="lblApCode"
                                    Text="Carrier Plan Code:"
                                    AssociatedControlID="txtaPcode"
                                    Runat="server" />
                                     <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator3"
                                    ControlToValidate="txtaPcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <div class="instructions">
                                    Carrier Plan code
                                </div>
                                <asp:TextBox
                                    id="txtaPcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemPlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblaTcode"
                                    Text="Carrier Coverage Code:"
                                    AssociatedControlID="txtaTcode"
                                    Runat="server" />
                               < <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator4"
                                    ControlToValidate="txtaTcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <div class="instructions">
                                    Carrier Coverage code
                                </div>
                                <asp:TextBox
                                    id="txtaTcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemTierCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPcode"
                                    Text="Prism Plan Code:"
                                    AssociatedControlID="txtPcode"
                                    Runat="server" />
                                <!--<asp:RequiredFieldValidator
                                    id="RequiredFieldValidator5"
                                    ControlToValidate="txtPcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" /> -->   
                                <div class="instructions">
                                    Prism Plan code
                                </div>
                                <asp:TextBox
                                    id="txtPcode" ReadOnly="true"
                                    Text='<%# Bind("PlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblTcode"
                                    Text="Prism Tier Code:"
                                    AssociatedControlID="txtTcode"
                                    Runat="server" />
                                <!--<asp:RequiredFieldValidator
                                    id="RequiredFieldValidator6"
                                    ControlToValidate="txtTcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  -->  
                                <div class="instructions">
                                    Prism Tier Code
                                </div>
                                <asp:TextBox
                                    id="txtTcode" ReadOnly="true"
                                    Text='<%# Bind("TierCode") %>'
                                    Runat="server" /> 
                                <br /><br />
                                <asp:Label
                                    id="lblEdate"
                                    Text="Plan Effective YRMO:"
                                    AssociatedControlID="txteDate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator2"
                                    ControlToValidate="txteDate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator5" 
                                    runat="server"
                                    ControlToValidate="txteDate"
                                    ValidationExpression="^((\d{4}))(|(0[1-9])|(1[0-2]))$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter YRMO in format 'yyyymm'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                    Effective YRMO in format 'yyyymm'
                                </div>
                                <asp:TextBox
                                    id="txteDate"
                                    Text='<%# Bind("YRMO") %>'
                                    Runat="server" />     
                                <br /><br />
                                <asp:Label
                                    id="lblRate"
                                    Text="Rate:"
                                    AssociatedControlID="txtRate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator7"
                                    ControlToValidate="txtRate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator6" 
                                    runat="server"
                                    ControlToValidate="txtRate"
                                    ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter Rate in format '000.00'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                   Rate for the plan code
                                </div>
                                <asp:TextBox
                                    id="txtRate" 
                                    Text='<%# Bind("Rate") %>'
                                    Runat="server" />    
                                <br /><br />
                                </div>                                
                                <br />
                                <asp:Button 
                                    id="btnEdit"
                                    Text="Update Changes"
                                    CommandName="Update"
                                    ValidationGroup="Edit"
                                    Runat="server" />
                                <asp:Button 
                                    id="btnCancel"
                                    Text="Cancel"
                                    CausesValidation="false"
                                    CommandName="Cancel"
                                    Runat="server" />
                            </EditItemTemplate>                            
                            </asp:FormView>
                        </custom:WebWindow>
                        <custom:WebWindow
                            id="winEdit2"
                            WindowTitleText="Edit EAP Plan Code"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                            <asp:FormView
                            id="frmEdit3"
                            DefaultMode="Edit"
                            DataSourceID="srcEditEPlanhier" OnItemUpdating="grdvEAP_RowUpdating"
                            Runat="server" OnItemCommand="frmEditE_ItemCommand">
                            <EditItemTemplate>
                                <div class="scrollBox" style="height:300px">  
                                <asp:Label
                                    id="lblPid"
                                    Text="Plan ID:"
                                    AssociatedControlID="txtEdit"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator8"
                                    ControlToValidate="txtEdit"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan ID
                                </div>
                                <asp:TextBox
                                    id="txtEdit" ReadOnly="true"
                                    Text='<%# Bind("PlhrId") %>'
                                    Runat="server" />    
                                <br /><br /> 
                                <asp:Label
                                    id="lblPY"
                                    Text="Plan Year:"
                                    AssociatedControlID="txtPY"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="reqName"
                                    ControlToValidate="txtPY"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan year in format 'yyyy' (ex:2008)
                                </div>
                                <asp:TextBox
                                    id="txtPY" ReadOnly="true"
                                    Text='<%# Bind("PlanYear") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblPDesc"
                                    Text="Plan Description:"
                                    AssociatedControlID="txtDesc"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator1"
                                    ControlToValidate="txtDesc"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />    
                                <div class="instructions">
                                    Plan Description
                                </div>
                                <asp:TextBox
                                    id="txtDesc" ReadOnly="true"
                                    Text='<%# Bind("Desc") %>'
                                    TextMode="multiLine"
                                    Columns="40"
                                    Rows="4"
                                    Runat="server" />    
                                <br /><br />                                
                                <asp:Label
                                    id="lblApCode"
                                    Text="Carrier Plan Code:"
                                    AssociatedControlID="txtaPcode"
                                    Runat="server" />
                                     <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator3"
                                    ControlToValidate="txtaPcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <div class="instructions">
                                    Carrier Plan code
                                </div>
                                <asp:TextBox
                                    id="txtaPcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemPlanCode") %>'
                                    Runat="server" />    
                                <br /><br />
                                <asp:Label
                                    id="lblaTcode"
                                    Text="Carrier Coverage Code:"
                                    AssociatedControlID="txtaTcode"
                                    Runat="server" />
                               < <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator4"
                                    ControlToValidate="txtaTcode"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <div class="instructions">
                                    Carrier Coverage code
                                </div>
                                <asp:TextBox
                                    id="txtaTcode" ReadOnly="true"
                                    Text='<%# Bind("AnthemTierCode") %>'
                                    Runat="server" />    
                                <br /><br />                                
                                <asp:Label
                                    id="lblEdate"
                                    Text="Plan Effective YRMO:"
                                    AssociatedControlID="txteDate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator2"
                                    ControlToValidate="txteDate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" /> 
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator7" 
                                    runat="server"
                                    ControlToValidate="txteDate"
                                    ValidationExpression="^((\d{4}))(|(0[1-9])|(1[0-2]))$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter YRMO in format 'yyyymm'
                                </asp:RegularExpressionValidator>    
                                <div class="instructions">
                                    Effective YRMO in format 'yyyymm'
                                </div>
                                <asp:TextBox
                                    id="txteDate"
                                    Text='<%# Bind("YRMO") %>'
                                    Runat="server" />     
                                <br /><br />
                                <asp:Label
                                    id="lblRate"
                                    Text="Rate:"
                                    AssociatedControlID="txtRate"
                                    Runat="server" />
                                <asp:RequiredFieldValidator
                                    id="RequiredFieldValidator7"
                                    ControlToValidate="txtRate"
                                    Text="(Required)"
                                    ValidationGroup="Edit"
                                    Runat="server" />  
                                <asp:RegularExpressionValidator 
                                    id="RegularExpressionValidator8" 
                                    runat="server"
                                    ControlToValidate="txtRate"
                                    ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                    Display="Static" ValidationGroup="Edit"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Please enter Rate in format '000.00'
                                </asp:RegularExpressionValidator>   
                                <div class="instructions">
                                   Rate for the plan code
                                </div>
                                <asp:TextBox
                                    id="txtRate" 
                                    Text='<%# Bind("Rate") %>'
                                    Runat="server" />    
                                <br /><br />
                                </div>                                
                                <br />
                                <asp:Button 
                                    id="btnEdit"
                                    Text="Update Changes"
                                    CommandName="Update"
                                    ValidationGroup="Edit"
                                    Runat="server" />
                                <asp:Button 
                                    id="btnCancel"
                                    Text="Cancel"
                                    CausesValidation="false"
                                    CommandName="Cancel"
                                    Runat="server" />
                            </EditItemTemplate>                            
                            </asp:FormView>
                        </custom:WebWindow>
                    </div> 
                    <div id="gridPlanOption">
                        <asp:Label ID="lblPlan" runat="server" Text="Select plan:"></asp:Label>
                        <asp:DropDownList ID="ddlPlan" AutoPostBack="true" runat="server" OnTextChanged="ddlPlan_SelectedIndexChanged">
                            <asp:ListItem Selected="True"/>
                            <asp:ListItem Text="Domestic"/> 
                            <asp:ListItem Text="EAP"/>                            
                            <asp:ListItem Text="International"/>
                        </asp:DropDownList>
                    </div>
                  <div id ="domDiv" runat="server" style="float:left;margin:10px;width:700px"> 
                      <asp:CompleteGridView runat="server" CssClass="tablestyle" ID="grdDom" 
                        DataKeyNames="PlhrId" GridLines="Both"  OnSelectedIndexChanged="grdDom_SelectedIndexChanged"                         
                        AutoGenerateColumns="False"  AllowSorting="True" OnSorting="grdDomResult_Sorting"
                        AllowPaging="True" PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grdDom_RowDataBound" 
                        DataSourceID="srcDPlanhier" PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" ShowFilter="True"> 
                            <PagerStyle CssClass="customGridpaging" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:TemplateField><ItemTemplate>
                                        <asp:LinkButton
                                            id="lnkEdit"
                                            Text="Edit"
                                            CommandName="Select"
                                            Runat="server" /> 
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PlhrId" HeaderText="ID" SortExpression="PlhrId" Visible="False"></asp:BoundField>
                                <asp:BoundField DataField="Desc" HeaderText="Plan Desc" SortExpression="Desc"></asp:BoundField>
                                <asp:BoundField DataField="PlanYear" HeaderText="Plan Year" SortExpression="PlanYear"></asp:BoundField>
                                <asp:BoundField DataField="AnthemPlanCode" HeaderText="Carrier Plan Code" SortExpression="AnthemPlanCode"></asp:BoundField>
                                <asp:BoundField DataField="AnthemTierCode" HeaderText="Carrier Coverage Code" SortExpression="AnthemTierCode"></asp:BoundField>
                                <asp:BoundField DataField="PlanCode" HeaderText="Prism Plan Code" SortExpression="PlanCode"></asp:BoundField>
                                <asp:BoundField DataField="TierCode" HeaderText="Prism Tier Code" SortExpression="TierCode"></asp:BoundField>
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate"></asp:BoundField>
                                <asp:BoundField DataField="YRMO" HeaderText="Effective YRMO" SortExpression="YRMO"></asp:BoundField>
                                </Columns>                         
                      </asp:CompleteGridView>               
                    </div>                   
                    </asp:GridView>
                    <div id ="intlDiv" runat="server" style="float:left;margin:10px;width:700px">                        
                        <asp:CompleteGridView ID="grdIntl" runat="server" CssClass="tablestyle"  
                        DataKeyNames="PlhrId" GridLines="Both" OnSelectedIndexChanged="grdIntl_SelectedIndexChanged"                      
                        AutoGenerateColumns="False"  AllowSorting="True" OnSorting="grdIntlResult_Sorting"
                        AllowPaging="True" PagerSettings-Mode="NumericFirstLast" OnRowDataBound="grdIntl_RowDataBound"
                        DataSourceID="srcIPlanhier" PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" ShowFilter="True">                
                            <PagerSettings Mode="NextPreviousFirstLast" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <Columns> 
                            <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            id="lnkEdit1"
                                            Text="Edit"
                                            CommandName="Select"
                                            Runat="server" />                                       
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                               <asp:BoundField DataField="PlhrId"  HeaderText="ID" SortExpression="PlhrId" Visible="False" />                    
                                <asp:BoundField DataField="Desc" HeaderText="Plan Desc" SortExpression="Desc" />
                                <asp:BoundField DataField="PlanYear" HeaderText="Plan Year" SortExpression="PlanYear"></asp:BoundField>
                                <asp:BoundField DataField="AnthemPlanCode" HeaderText="Carrier Plan Code" SortExpression="AnthemPlanCode" />
                                <asp:BoundField DataField="AnthemTierCode" HeaderText="Carrier Coverage Code" SortExpression="AnthemTierCode" />                                 
                                <asp:BoundField DataField="PlanCode" HeaderText="Prism Plan Code" SortExpression="PlanCode" />
                                <asp:BoundField DataField="TierCode" HeaderText="Prism Tier Code" SortExpression="TierCode" />
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                <asp:BoundField DataField ="YRMO" HeaderText="Effective YRMO" SortExpression ="YRMO" />
                            </Columns>
                        </asp:CompleteGridView>                                                                         
                    </div>
                     <div id ="divEAP" runat="server" style="float:left;margin:10px;width:700px">      
                        <asp:CompleteGridView ID="grdEAP" runat="server" CssClass="tablestyle"  
                        DataKeyNames="PlhrId" GridLines="Both"  OnSelectedIndexChanged="grdEAP_SelectedIndexChanged"                         
                        AutoGenerateColumns="False"  AllowSorting="True" OnSorting="grdEAPResult_Sorting"
                        AllowPaging="True" PagerSettings-Mode="NextPreviousFirstLast" OnRowDataBound="grdEAP_RowDataBound" 
                        DataSourceID="srcEPlanhier" PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" ShowFilter="True">                
                            <PagerSettings Mode="NextPreviousFirstLast" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <Columns> 
                              <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton
                                            id="lnkEdit"
                                            Text="Edit"
                                            CommandName="Select"
                                            Runat="server" />                                       
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                                <asp:BoundField DataField="PlhrId"  HeaderText="ID" SortExpression="PlhrId" Visible="False" />                    
                                <asp:BoundField DataField="Desc" HeaderText="Plan Desc" SortExpression="Desc" />
                                <asp:BoundField DataField="PlanYear" HeaderText="Plan Year" SortExpression="PlanYear"></asp:BoundField>
                                <asp:BoundField DataField="AnthemPlanCode" HeaderText="Carrier Plan Code" SortExpression="AnthemPlanCode" />
                                <asp:BoundField DataField="AnthemTierCode" HeaderText="Carrier Coverage Code" SortExpression="AnthemTierCode" /> 
                                <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" />
                                <asp:BoundField DataField ="YRMO" HeaderText="Effective YRMO" SortExpression ="YRMO" />
                            </Columns>
                        </asp:CompleteGridView>                                               
                    </div> 
                    <asp:ObjectDataSource
                        id = "srcInsertPlan"
                        TypeName = "EBA.Desktop.CodesData"                      
                        InsertMethod = "Insert"                       
                        runat = "server" >                                                                                 
                    </asp:ObjectDataSource>
                    <!-- <asp:ObjectDataSource
                        id = "ddlSource"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "ddlSelect"
                        CacheExpirationPolicy="Sliding"
                        CacheDuration="300"  
                        runat = "server" > 
                    </asp:ObjectDataSource> -->
                    <asp:ObjectDataSource
                        id = "srcDPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectD"
                        CacheExpirationPolicy="Sliding"
                        CacheDuration="300"  
                        runat = "server" > 
                        <SelectParameters>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortExpression">
                              </asp:Parameter>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortDirection">
                              </asp:Parameter>
                        </SelectParameters>                                    
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource
                        id = "srcEditDPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectU"
                        UpdateMethod = "Update"                                                                     
                        runat = "server" OnUpdated = "grdvDom_Updated">
                        <SelectParameters>
                           <asp:ControlParameter Name="PlhrId" ControlID="grdDom" Type="Int32" />                     
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:ControlParameter Name="PlhrId" ControlID="grdDom" Type="Int32" />                            
                        </UpdateParameters>                    
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource
                        id = "srcIPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectI"                         
                        CacheExpirationPolicy="Sliding"
                        CacheDuration="300"  
                        runat = "server" >
                        <SelectParameters>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortExpression">
                              </asp:Parameter>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortDirection">
                              </asp:Parameter>
                        </SelectParameters>                                        
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource
                        id = "srcEditIPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectU"
                        UpdateMethod = "Update"                                                
                        runat = "server" OnUpdated = "grdvIntl_Updated">
                        <SelectParameters>
                           <asp:ControlParameter Name="PlhrId" ControlID="grdIntl" Type="Int32" />                     
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:ControlParameter Name="PlhrId" ControlID="grdIntl" Type="Int32" />                             
                        </UpdateParameters>                    
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource
                        id = "srcEPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectE"                         
                        CacheExpirationPolicy="Sliding"
                        CacheDuration="300"  
                        runat = "server" > 
                        <SelectParameters>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortExpression">
                              </asp:Parameter>
                              <asp:Parameter Direction="input" Type="string" Name="p_sortDirection">
                              </asp:Parameter>
                        </SelectParameters>                                                                                    
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource
                        id = "srcEditEPlanhier"
                        TypeName = "EBA.Desktop.CodesData"
                        SelectMethod = "SelectEU"
                        UpdateMethod = "UpdateE"                                                
                        runat = "server" OnUpdated = "grdvEAP_Updated">
                        <SelectParameters>
                           <asp:ControlParameter Name="PlhrId" ControlID="grdEAP" Type="Int32" />                     
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:ControlParameter Name="PlhrId" ControlID="grdEAP" Type="Int32" />                             
                        </UpdateParameters>                    
                    </asp:ObjectDataSource>
                </asp:View>
                <asp:View ID="View2" runat="server"> 
                <div class="userPara">
                    <p>Default threshold values set at recon level. You can edit or change values.</p>    
                </div> 
                 <div style="float:left;margin:10px;width:700px">
                     <asp:GridView ID="grdThreshold" runat="server" CssClass="tablestyle"  AutoGenerateEditButton="True"                       
                        AutoGenerateColumns="False"  AllowSorting="True" DataKeyNames="thres_id"
                        AllowPaging="True" PagerSettings-Mode="NextPreviousFirstLast" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" 
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" OnRowUpdating="grdThreshold_rowUpdating" 
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" DataSourceID="SqlDataSource3">                
                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <columns>
                                <asp:BoundField DataField="thres_id" HeaderText="Threshold ID" ReadOnly="True" Visible="False"/>
                                <asp:BoundField DataField="thres_name" HeaderText="Reconciliation Type" ReadOnly="True" SortExpression="thres_name" />
                                <asp:BoundField DataField ="thres_type" HeaderText="Threshold Level" ReadOnly="True" SortExpression="thres_type" />
                                <asp:BoundField DataField="thres_value" HeaderText="Set Value" SortExpression="thres_value" ControlStyle-Width="50px"/> 
                                <asp:BoundField DataField ="thres_yrmo" HeaderText="YRMO" SortExpression="thres_yrmo" ControlStyle-Width="50px"/>
                            </columns>
                        </asp:GridView>    
                     <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                         SelectCommand="SELECT [thres_id],[thres_name],[thres_type],[thres_value],[thres_yrmo] FROM [threshold] WHERE [thres_type] = 'Default'"
                         UpdateCommand = "UPDATE [threshold] SET [thres_value] = @thres_value,[thres_yrmo] = @thres_yrmo WHERE [thres_id] = @thres_id"
                         DeleteCommand ="DELETE FROM [threshold] WHERE [thres_id] = @thres_id">
                         <UpdateParameters> 
                            <asp:Parameter Name="thres_id" />                            
                            <asp:Parameter Name="thres_value" />
                            <asp:Parameter Name="thres_yrmo" />
                        </UpdateParameters>
                         <DeleteParameters> <asp:Parameter Name="thres_id" /></DeleteParameters>
                    </asp:SqlDataSource>
                 </div>
                 <div style="float:left;margin:10px;width:700px" >
                        <asp:Label ID="lblErrThres" runat="server" Text="" ForeColor="red"></asp:Label>
                </div>
                 <div style="float:left;margin:10px;width:700px;margin-top:20px;" >
                 </div>
                  <div class="userPara">
                    <p>Add threshold values for Carrier Plan codes. You can set it at <b> 'Grand Total'</b>, <b> 'Group Suffix Level'</b>, <b> 'Group Suffix Total'</b>, <b>'Plan Group - Tier levels'</b> <br /></p>    
                </div> 
                <div style="float:left;margin:10px;width:700px">
                      <asp:LinkButton ID="lnkAddThres" runat="server" OnClick="lnkAddThres_Onclick">Add New Threshold</asp:LinkButton>
                </div>                                  
                  <div id = "addThresdiv" runat="server" visible = "false" style="float:left;margin:10px;width:700px">
                      <asp:Label ID="lblReconType" runat="server" Text="Reconciliation type:" AssociatedControlID="ddlReconType" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label> 
                      <asp:DropDownList ID="ddlReconType" runat="server" AutoPostBack="true" style="display:block;margin-left:30px;width:150px;" OnSelectedIndexChanged="ddlReconType_SelectedIndexchange">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="DOMESTIC"></asp:ListItem>
                        <asp:ListItem Text="INTERNATIONAL"></asp:ListItem>
                      </asp:DropDownList>                     
                      <asp:Label ID="lblSubType" runat="server" Text="Sub Type:" Visible="false" AssociatedControlID="ddlSubType" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                      <asp:DropDownList ID="ddlSubType" runat="server" Visible="false" AutoPostBack="true" style="display:block;margin-left:30px;width:150px;" OnSelectedIndexChanged="ddlSubType_SelectedIndexchange">                        
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="ACTIVE"></asp:ListItem>
                        <asp:ListItem Text="RETIREE"></asp:ListItem>
                        <asp:ListItem Text="COBRA"></asp:ListItem>
                      </asp:DropDownList>
                      <br /><br />
                      <asp:Label ID="lblThresType" runat="server" Text="Threshold Type:" AssociatedControlID="ddlThres" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                      <asp:DropDownList ID="ddlThres" runat="server" AutoPostBack="true" style="display:block;margin-left:30px;width:150px;" OnSelectedIndexChanged="ddlThres_SelectedIndexchange">                        
                        <asp:ListItem Text="Grand Total"></asp:ListItem>
                        <asp:ListItem Text="Group Suffix"></asp:ListItem>
                        <asp:ListItem Text="Group Suffix Total"></asp:ListItem>
                        <asp:ListItem Text="Tier Codes"></asp:ListItem>
                      </asp:DropDownList>
                      <br/>
                      <p style="margin-left:40px;color:#036">
                          <b>Grand Total</b> - will set Threshold for Grand Total. <br />
                          <b>Group Suffix</b> - will set Threshold for Selected Group Suffix<br />
                          <b>Group Suffix Total</b> - will set Threshold for Selected Group Suffix Total.<br />
                          <b>Tier Codes</b> - will set Threshold at tier codes level.<p>
                      <br /><br />
                      <asp:Label ID="lblPlancd" runat="server" Text="Group Suffix:" AssociatedControlID="ddlPlancode" Visible="false" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                      <asp:DropDownList ID="ddlPlancode" runat="server" AutoPostBack="true" Visible="false" style="margin-left:30px;width:150px;"> 
                      </asp:DropDownList>
                      <br /><br />  
                      <asp:Label ID="lblTierCd" runat="server" Text="Tier Code:" AssociatedControlID="ddlPlancode" Visible="false" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                      <asp:DropDownList ID="ddltiercode" runat="server" Visible="false" style="margin-left:30px;width:150px;"> 
                      <asp:ListItem Text="S"></asp:ListItem>
                      <asp:ListItem Text="2P"></asp:ListItem>
                      <asp:ListItem Text="FAM"></asp:ListItem>
                      <asp:ListItem Text="S+DE"></asp:ListItem>
                      </asp:DropDownList>
                      <br /><br />                    
                      <asp:Label ID="lblValue" runat="server" Text="Threshold Value:" AssociatedControlID="txtValue" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                      <asp:TextBox ID="txtValue" runat="server"  style="margin-left:30px;width:150px;"></asp:TextBox>
                      <asp:RequiredFieldValidator
                        id="RequiredFieldValidator7"
                        ControlToValidate="txtValue"
                        Text="(Required)" Display="Dynamic"
                        ValidationGroup="tr1"
                        Runat="server" /> 
                      <asp:RegularExpressionValidator 
                        id="RegularExpressionValidator2" 
                        runat="server"
                        ControlToValidate="txtValue"
                        ValidationExpression="^[-+]?\d+(\.\d+)?$"
                        Display="Dynamic" ValidationGroup="tr1"
                        Font-Names="verdana" Font-Size="10pt">
                        Please enter Rate in format '#00.00'
                    </asp:RegularExpressionValidator> 
                    <br /><br />
                    <asp:Label ID="lblDate" runat="server" Text="Threshold Eff YRMO:" AssociatedControlID="txtDate" style="display:block;margin-left:30px;width:150px;text-align:left"></asp:Label>
                    <asp:TextBox ID="txtDate" runat="server" ValidationGroup="tr1" style="display:block; margin-left:30px;width:150px;"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        id="RequiredFieldValidator11"
                        ControlToValidate="txtDate"
                        Text="(Required)" Display="Dynamic"
                        ValidationGroup="tr1"
                        Runat="server" />
                    <asp:RegularExpressionValidator 
                        id="revYRMO" 
                        runat="server"
                        ControlToValidate="txtDate"
                        ValidationExpression="^((\d{4}))(|(0[1-9])|(1[0-2]))$"
                        Display="Dynamic" ValidationGroup="tr1"
                        Font-Names="verdana" Font-Size="10pt">
                        Please enter Plan Year in format 'yyyymm'
                    </asp:RegularExpressionValidator> 
                      <br /><br />
                      <asp:Button ID="btnAddThres" runat="server" Text="ADD" ValidationGroup="tr1" OnClick="btnAddThres_OnClick" style="margin-left:30px;"/>
                      <asp:Button ID="btnCancelThres" runat="server" Text="Cancel" OnClick="btnCancelThres_OnClick" style="margin-left:30px;"/>
                  </div>                    
                     <div style="float:left;margin:10px;width:700px">
                     <asp:GridView ID="grdvPlanThres" runat="server" CssClass="tablestyle"  
                        GridLines="Both"   AutoGenerateEditButton="True"                       
                        AutoGenerateColumns="False"  AllowSorting="True" DataKeyNames="thres_id"
                        AllowPaging="True" PagerSettings-Mode="NextPreviousFirstLast" OnRowDataBound="grdPlanThres_RowDataBound"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" OnRowDeleting = "grdPlanThres_rowDeleting"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" OnRowUpdating="grdPlanThres_rowUpdating"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" 
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" DataSourceID="SqlDataSource1">                
                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <columns>
                                <asp:TemplateField HeaderText="Delete">
                                    <ItemTemplate>                                    
                                    <asp:LinkButton ID="btnDelete" Text="Delete" runat="server"
                                    OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                    CommandName="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="thres_id" HeaderText="Threshold ID" ReadOnly="True" Visible="false"/>
                                <asp:BoundField DataField="thres_name" HeaderText="Reconciliation Type" ReadOnly="True" SortExpression="thres_name" />
                                <asp:BoundField DataField ="thres_type" HeaderText="Threshold Level" ReadOnly="True" SortExpression="thres_type" />
                                <asp:BoundField DataField="thres_value" HeaderText="Set Value" SortExpression="thres_value" ControlStyle-Width="50px"/>
                                <asp:BoundField DataField ="thres_yrmo" HeaderText="Effective YRMO" SortExpression="thres_yrmo" ControlStyle-Width="50px" />                               
                            </columns>
                        </asp:GridView>    
                     <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                         SelectCommand="SELECT [thres_id],[thres_name],[thres_value],[thres_yrmo],[thres_type] FROM [threshold] WHERE [thres_type] <> 'Default'"
                         UpdateCommand = "UPDATE [threshold] SET [thres_value] = @thres_value,[thres_yrmo] = @thres_yrmo WHERE [thres_id] = @thres_id"
                         DeleteCommand ="DELETE FROM [threshold] WHERE [thres_id] = @thres_id">                         
                         <UpdateParameters> 
                            <asp:Parameter Name="thres_id" />                            
                            <asp:Parameter Name="thres_value" />
                            <asp:Parameter Name="thres_yrmo" />
                        </UpdateParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="thres_id" /> 
                        </DeleteParameters>
                        </asp:SqlDataSource>
                 </div>
                 <!-- <div style="float:left;margin:10px;width:700px">
                    <custom:OpenWebWindow
                            id="lnkOpen1"
                            Text="Add New Threshold"
                            WebWindowID="winAdd"
                            Runat="server" />
                        <custom:WebWindow
                            id="winAdd"
                            WindowTitleText="Add New Threshold"
                            Hide="true"
                            ShowCloseButton="false"
                            Runat="server">
                                <asp:FormView
                                id="frmAdd"
                                DefaultMode="Insert"
                                DataSourceID="srcThres"
                                Runat="server" OnItemInserted="frmAdd_ItemInserted" OnItemCommand="frmAdd_ItemCommand">
                                <InsertItemTemplate>
                                <div class="scrollBox" style="height:300px">    
                                    <asp:Label
                                        id="lblName"
                                        Text="Recon Name:"
                                        AssociatedControlID="txtName"
                                        Runat="server" />
                                    <asp:RequiredFieldValidator
                                        id="reqName"
                                        ControlToValidate="txtName"
                                        Text="(Required)"
                                        ValidationGroup="Add"
                                        Runat="server" />    
                                    <div class="instructions">
                                        Enter the name of the Recon.
                                    </div>
                                    <asp:TextBox
                                        id="txtName"
                                        Text='<%# Bind("ReconName") %>'
                                        Runat="server" />    
                                    <br /><br />                                
                                    <asp:Label
                                        id="lblDescription"
                                        Text="Recon Percentage:"
                                        AssociatedControlID="txtPer"
                                        Runat="server" /> 
                                    <asp:RequiredFieldValidator
                                        id="reqPer"
                                        ControlToValidate="txtPer"
                                        Text="(Required)"
                                        ValidationGroup="Add"
                                        Runat="server" />                                 
                                    <div class="instructions">
                                        The threshold percentage for above reconciliation.
                                    </div>
                                    <asp:TextBox
                                        id="txtPer"
                                        Text='<%#Bind("ReconPerc")%>'                                        
                                        Runat="server" />    
                                    <br /><br /> 
                                    <asp:Label
                                        id="lblDate"
                                        Text="Date:"
                                        AssociatedControlID="txtDate"
                                        Runat="server" /> 
                                    <asp:RequiredFieldValidator
                                        id="reqDate"
                                        ControlToValidate="txtDate"
                                        Text="(Required)"
                                        ValidationGroup="Add"
                                        Runat="server" />                                 
                                    <div class="instructions">
                                        Date in format 'mm/dd/yyyy'.
                                    </div>
                                    <asp:TextBox
                                        id="txtDate"
                                        Text='<%#Bind("ReconDate")%>'                                        
                                        Runat="server" />
                                    <br /><br />                                        
                                </div>
                                <br />
                                <asp:Button 
                                    id="btnAdd"
                                    Text="Add Threshold"
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
                            id="srcThres"
                            TypeName="EBA.Desktop.InsertThreshold"                            
                            InsertMethod="Insert" 
                            Runat="server" />
                        </div> -->
                        
                </asp:View>
                <asp:View ID="View3" runat="server">
                 <div id = "introText">Audit information</div> 
                <div style="float:left;margin:10px;width:700px" >
                <asp:LinkButton id="lnkPlanAudit"  runat="server" OnClick="lnkPlanAudit_OnClick">Carrier Plan codes Table Audit</asp:LinkButton>
                </div>
                 <!--   <div style="float:left;margin:10px;width:700px">
                        <table>
                            <tr>
                                <td style="width:200px;margin:20px;text-align:center;Color:#036;" >
                                    
                                </td>
                                 <td style="width:200px;margin:20px;text-align:center;Color:#036;">
                                    <a  runat="server" OnClick="lnkFilter_OnClick">Audit Reports</a>
                                </td>
                            </tr>
                        </table>
                    </div> -->
                    <div id = "planAudit" runat="server"  style="float:left;margin:10px;width:700px" visible="false">
                    <div class = "subIntroText"><h3>Audit information For Plan Codes table</h3></div>
                    <div  style="float:left;margin:10px;width:700px"> 
                        <asp:CompleteGridView ID="grdvAuditAll" runat="server" CssClass="tablestyle"  AllowPaging="True" PagerSettings-Mode="NumericFirstLast"  
                        DataSourceID="SqlDataSource2" PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" OnRowDataBound="grdAuditAll_RowDataBound"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" ShowFilter="True"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="False" AllowSorting="True"> 
                        <PagerStyle CssClass="customGridpaging" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                            <PagerSettings FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif"
                                Mode="NumericFirstLast" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <Columns>
                                <asp:BoundField DataField="taskName" HeaderText="Task Name" SortExpression="taskName" />
                                <asp:BoundField DataField="anthcd_plancd" HeaderText="Carrier Plan Code" SortExpression="anthcd_plancd" />
                                <asp:BoundField DataField="anthcd_covgcd" HeaderText="Carrier Tier Code" SortExpression="anthcd_covgcd" />
                                <asp:BoundField DataField="tType" HeaderText="Type" SortExpression="tType" />                                
                                <asp:BoundField DataField="tColumn" HeaderText="Column" SortExpression="tColumn" />
                                <asp:BoundField DataField="tOldValue" HeaderText="Old Value" SortExpression="tOldValue" />
                                <asp:BoundField DataField="tNewValue" HeaderText="New Value" SortExpression="tNewValue" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate" />
                            </Columns>
                        </asp:CompleteGridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                             SelectCommand="SELECT taskName,tType,anthcd_plancd,anthcd_covgcd,tColumn,tOldValue,tNewValue,UserName,taskDate 
                                            FROM UserSession_AU, UserTasks_AU,AnthPlanhier,AnthCodes 
                                            WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId 
                                            AND (plhr_id = tPrimaryKey AND tTable = 'AnthPlanhier' AND plhr_anthcd_id = anthcd_id ) 
                                            UNION
                                            SELECT taskName,tType,anthcd_plancd,anthcd_covgcd,tColumn,tOldValue,tNewValue,UserName,taskDate 
                                            FROM UserSession_AU, UserTasks_AU,AnthCodes 
                                            WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId 
                                            AND (anthcd_id = tPrimaryKey AND tTable='AnthCodes') ">
                        </asp:SqlDataSource>
                    </div>
                    </div>
                    
                    <div style="float:left;margin:10px;width:700px" >
                        <asp:LinkButton id="lnkThresAudit"  runat="server" OnClick="lnkThresAudit_OnClick">Threshold Table Audit</asp:LinkButton>
                    </div>
                    <div id = "thresAudit" runat="server"  style="float:left;margin:10px;width:700px" visible="false">
                    <div class = "subIntroText"><h3>Audit information For Threshold table</h3></div>
                    <div  style="float:left;margin:10px;width:700px">
                        <asp:CompleteGridView ID="grdvAuditThres" runat="server" CssClass="tablestyle"  AllowPaging="True" PagerSettings-Mode="NumericFirstLast"  
                        DataSourceID="SqlDataSource4" PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" OnRowDataBound="grdAuditThres_RowDataBound"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" ShowFilter="True"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="False" AllowSorting="True"> 
                        <PagerStyle CssClass="customGridpaging" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerSettings FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif"
                                Mode="NumericFirstLast" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <Columns>
                                <asp:BoundField DataField="taskName" HeaderText="Task Name" SortExpression="taskName" />                                                                 
                                <asp:BoundField DataField="tPrimaryName" HeaderText="Reconciliation Type" SortExpression="tPrimaryName" /> 
                                <asp:BoundField DataField="tType" HeaderText="Type" SortExpression="tType" />                                
                                <asp:BoundField DataField="tColumn" HeaderText="Column" SortExpression="tColumn" />
                                <asp:BoundField DataField="tOldValue" HeaderText="Old Value" SortExpression="tOldValue" />
                                <asp:BoundField DataField="tNewValue" HeaderText="New Value" SortExpression="tNewValue" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate" />
                            </Columns>
                        </asp:CompleteGridView>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                             SelectCommand="SELECT taskName,tPrimaryName,tType,tColumn,tOldValue,tNewValue,UserName,taskDate 
                                            FROM UserSession_AU, UserTasks_AU
                                            WHERE UserTasks_AU.sessionID = UserSession_AU.SessionId 
                                            AND tTable = 'threshold' ">
                        </asp:SqlDataSource>
                    </div>
                    </div>
                     </asp:View>
                     <asp:View ID="View4" runat="server">
                     <div id = "introText">Generate Audit Reports</div> 
                     <div id = "filterAudit" style="float:left;margin:10px;width:700px" visible="false">
                     <div class = "subIntroText">Generate Audit Reports for Plan Codes Table</div>
                        <div  style="float:left;margin:10px;width:700px">
                              <table>
                                <tr>
                                    <td style="width:50px;margin:10px;text-align:right;">
                                        <asp:Label ID="lblFilter" runat="server" ForeColor="#036" Text="Filter:"></asp:Label>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                         <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged">
                                            <asp:ListItem Text ="All"></asp:ListItem>
                                            <asp:ListItem Text="Users"></asp:ListItem>                                        
                                            <asp:ListItem Text="Type"></asp:ListItem>
                                            <asp:ListItem Text="Rate"></asp:ListItem>
                                            <asp:ListItem Text="Effective YRMO"></asp:ListItem>                            
                                         </asp:DropDownList>
                                    </td>
                                    <td style="width:100px;margin:10px;">
                                        <asp:TextBox ID="txtFilter" runat="server" Visible="false"></asp:TextBox>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:DropDownList ID="ddlType" runat="server" Visible="false" Width="150px">
                                            <asp:ListItem Text="Update"></asp:ListItem>
                                            <asp:ListItem Text="Insert"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:Button ID="btnExcel" runat="server" Text="Generate Excel" OnClick="btnExcel_onClick"/>
                                    </td>
                                </tr>
                            </table>                            
                        </div>
                        <div style="float:left;margin:10px;width:700px" >
                                <asp:Label ID="lblAuditerr" runat="server" Text="" ForeColor="red" Visible="false" style="margin-left:220px;"></asp:Label>
                        </div>
                        <div class = "subIntroText">Generate Audit Reports for Threshold Table</div>
                        <div  style="float:left;margin:10px;width:700px">
                         <table>
                                <tr>
                                    <td style="width:50px;margin:10px;text-align:right;">
                                        <asp:Label ID="lblFilter1" runat="server" ForeColor="#036" Text="Filter:"></asp:Label>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                         <asp:DropDownList ID="ddlFilter1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter1_SelectedIndexChanged">
                                            <asp:ListItem Text ="All"></asp:ListItem>
                                            <asp:ListItem Text="Users"></asp:ListItem>                                        
                                            <asp:ListItem Text="Type"></asp:ListItem>
                                            <asp:ListItem Text="Threshold Value"></asp:ListItem>
                                            <asp:ListItem Text="Effective YRMO"></asp:ListItem>                            
                                         </asp:DropDownList>
                                    </td>
                                    <td style="width:100px;margin:10px;">
                                        <asp:TextBox ID="txtFilter1" runat="server" Visible="false"></asp:TextBox>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:DropDownList ID="ddlType1" runat="server" Visible="false" Width="150px">
                                            <asp:ListItem Text="Update"></asp:ListItem>
                                            <asp:ListItem Text="Insert"></asp:ListItem>
                                            <asp:ListItem Text="Delete"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:Button ID="btnExcel1" runat="server" Text="Generate Excel" OnClick="btnExcel1_onClick"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    
                    <!--
                    <div style="float:left;margin:10px;width:700px" >
                        <asp:GridView ID="grdFilterAudit" runat="server" CssClass="customGrid"  AllowPaging="True" PagerSettings-Mode="NumericFirstLast"  
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="False" AllowSorting="True"> 
                        <PagerStyle CssClass="customGridpaging" />
                            <PagerSettings FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif"
                                Mode="NumericFirstLast" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <Columns>
                                <asp:BoundField DataField="taskName" HeaderText="Task Name" SortExpression="taskName" />
                                <asp:BoundField DataField="tType" HeaderText="Type" SortExpression="tType" />                                
                                <asp:BoundField DataField="tColumn" HeaderText="Column" SortExpression="tColumn" />
                                <asp:BoundField DataField="tOldValue" HeaderText="Old Value" SortExpression="tOldValue" />
                                <asp:BoundField DataField="tNewValue" HeaderText="New Value" SortExpression="tNewValue" />
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate" />
                            </Columns>
                        </asp:GridView>
                     </div> -->
                </asp:View>
            </asp:MultiView> 
        </div>             
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


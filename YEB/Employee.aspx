<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="YEB_Employee" Title="YEB Employee Page" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="contentleft">        
        <div id = "module_menu">
            <h1>YEB</h1> 		    
		    <ul id="sitemap">		               	      
		      <li><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/YEB/UpdateSSN.aspx">Update SSN</asp:Hyperlink></li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/YEB/Imports.aspx">Imports</asp:Hyperlink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/YEB/Reports.aspx">Reports</asp:Hyperlink></li>

		    </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
    <div id="contentright">
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Employee" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Audit" Value="1"></asp:MenuItem>     
                               
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>               
       <div id = "introText" style="margin-left:0px;">
           <asp:Label ID="lblEmpHeading" runat="server" Text=""></asp:Label>
        </div>
        <div style="float:left;width:650px;margin:5px;">
        </div>  
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                <div style="margin-top:10px;">
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup = "grp2" runat="server" DisplayMode="List" ShowMessageBox="True" />
                    <div class="info" id="infoDiv1" runat="server" visible="false">
                        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                    </div>           
                    <div id = "formHra">
                        <fieldset>
                            <legend>Employee Information</legend>
                            <asp:Label ID="lblEmpNo" runat="server" Text="Emp#" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtEmpNo" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>&nbsp;
                            <br class = "br_e" />
                            <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSSN" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup = "grp2" runat="server" ErrorMessage="SSN is required!" ControlToValidate="txtSSN">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblFirst" runat="server" Text="First Name" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFirst" runat="server" CssClass="inputHra"></asp:TextBox>&nbsp;
                            <br class = "br_e" />
                            <asp:Label ID="lblMI" runat="server" Text="MI" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtMI" runat="server" CssClass="inputHra"></asp:TextBox>                           
                            <br class = "br_e" />
                            <asp:Label ID="lblLast" runat="server" Text="Last Name" CssClass="labelHra"></asp:Label>                            
                            <asp:TextBox ID="txtLast" runat="server" CssClass="inputHra"></asp:TextBox>&nbsp;
                            <br class = "br_e" />

                         </fieldset>
                     </div>
                             <table style="margin-top:10px;margin-left:50px;">
                        <tr>                                                              
                            <td style="margin-left:100px;margin-right:20px;margin-top:20px;margin-bottom:20px;">
                                <asp:LinkButton ID="btnEdit" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" CssClass = "imgbutton" 
                                    OnClick="btnEdit_Click" ><span>Edit</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnSave" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" CssClass = "imgbutton" 
                                    ValidationGroup = "grp2" OnClick="btnSave_Click" Visible= "false" ><span>Save</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="this.blur();" 
                                Font-Underline="false" CssClass = "imgbutton" OnClick="btnCancel_Click" 
                                Visible= "false"><span>Cancel</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    </div></asp:View>
                <asp:View ID="View3" runat="server">     
                <div id = "Div1">
                    <fieldset>
                        <legend>Audit Information</legend>
                        <div id= "Div2" style="width:650px"> 
                           
	                    </div>
                        <div style="padding-top:3px;width:600px;margin-left:20px;max-height:450px;margin-top:10px;"  class="scroller">
                        <asp:CompleteGridView runat="server" ID="grdvAudit" CssClass="tablestyle"                       
                        AutoGenerateColumns="False"  AllowSorting="True" 
                        PagerSettings-Mode="NumericFirstLast" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" 
                        SortAscendingImageUrl="" SortDescendingImageUrl=""> 
                        <PagerSettings Mode="NextPreviousFirstLast" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />               
                        <PagerStyle CssClass="customGridpaging" />
                            <columns>                                                         
                                <asp:BoundField DataField="tColumn" HeaderText="Column Name" SortExpression="ColumnName"></asp:BoundField>
                                <asp:BoundField DataField="tPrimarykey" HeaderText="EmpNo" SortExpression="EmpNo"></asp:BoundField>
                                <asp:BoundField DataField="tOldValue" HeaderText="Old Value(SSN)" SortExpression="OldValue"></asp:BoundField>
                                <asp:BoundField DataField="tNewValue" HeaderText="New Value(SSN)" SortExpression="NewValue"></asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName"></asp:BoundField> 
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate"></asp:BoundField>
                            </columns>                    
                        </asp:CompleteGridView>
                        </div>
                    </fieldset>
                  
                                      
                                       
                    </div></asp:View>

</asp:MultiView>
</div>
<cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>
 
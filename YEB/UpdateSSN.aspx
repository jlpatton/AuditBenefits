    
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateSSN.aspx.cs" Inherits="YEB_UpdateSSN" Title="YEB UpdateSSNPage" %>

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
                <div id = "introText">Search Employee</div> 
        <div id = "importbox">
           <fieldset>
                <table style="margin-top:10px;margin-bottom:10px">
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblSSN" runat="server" Text="SSN:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSSN" runat="server" style="border:2px solid #e2e2e2" ValidationGroup="v1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" 
                            ControlToValidate="txtSSN" ValidationGroup ="v1" ErrorMessage="Missing SSN">* Required</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator 
                            id="RegvalSSN" 
                            runat="server"
                            ControlToValidate="txtSSN"
                            ValidationExpression="^\d{9}$"
                            Display="dynamic" ValidationGroup="v1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid SSN in format 'xxxxxxxxx'
                            </asp:RegularExpressionValidator>  
                        </td>
                        
                       <td width="20">
                       </td>
                        <td style="margin-left:10px;">
                            <asp:Button ID="btnSSN" runat="server" Text="Search"  ValidationGroup ="v1"  OnClick="btnSSN_Click" CssClass = "btnstyle" /> 
                        </td>                        
                    </tr></table></fieldset>
                    <div style="float:left;width:680px;color:red">
            <asp:Label ID="lblError" runat="server" Text="" style="margin-left:20px;"></asp:Label>
        </div> 
                            <div id = "EmpListDiv" runat="server" visible="false">
            <div id = "Div1">Employees List</div>
        </div> 
        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
            <asp:GridView ID="grdvEmployee" runat="server" 
            AutoGenerateColumns="False" OnRowCommand="grdvEmployee_RowCommand" 
            GridLines="None"  AllowSorting="true"   OnSorting="grdvEmployee_Sorting"
            CssClass="tablestyle"  PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" >
            <PagerSettings Mode="NextPreviousFirstLast" />
            <AlternatingRowStyle CssClass="altrowstyle" />
            <HeaderStyle CssClass="headerstyle" />
            <RowStyle CssClass="rowstyle" />               
            <PagerStyle CssClass="customGridpaging" /> 
                <Columns>                    
                    <asp:ButtonField Text="Select" commandname="Select" ButtonType="Link"/>
                    <asp:BoundField HeaderText="Empno" DataField="empl_empno" SortExpression="empl_empno"/>
                    <asp:BoundField HeaderText="First Name" DataField="empl_fname" SortExpression="empl_fname" />
                    <asp:BoundField HeaderText="Last Name" DataField="empl_lname" SortExpression ="empl_lname" />
                    <asp:BoundField HeaderText="SSN" DataField="empl_ssn" SortExpression ="empl_ssn" />       
                </Columns>                
            </asp:GridView>
        </div>             
    </div>
    </div>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VWA_Cases_Search.aspx.cs" Inherits="VWA_VWA_Cases_Search" Title="Search Vengroff William Cases By Contract# or Name" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "module_menu">
            <h1>VWA</h1>            
            <ul id="sitemap">
                <li><a>Import</a>
                    <ul>
                        <li><asp:HyperLink ID="hypBank" runat="server" NavigateUrl="~/VWA/Import_Bank_Statement.aspx">Bank Statement</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypRem" runat="server" NavigateUrl="~/VWA/Import_Remittance_Reports.aspx">Remittance Info</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypVWA" runat="server" NavigateUrl="~/VWA/Import_VWA_File.aspx">VWA File</asp:HyperLink></li>		                	                
                    </ul>
                </li> 
                <li><asp:HyperLink ID="hypTran" runat="server" NavigateUrl="~/VWA/Create_VWA_Tran_Activity.aspx">Transaction Reports</asp:HyperLink></li>                
                <li><asp:HyperLink ID="hypVWABalance" runat="server" NavigateUrl="~/VWA/VWA_Balance.aspx">Balance</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWAAdj" runat="server" NavigateUrl="~/VWA/VWA_Adjustments.aspx">Adjustments</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWACases" runat="server" Font-Underline="true" NavigateUrl="~/VWA/VWA_Cases_Search.aspx">Individual Cases</asp:HyperLink></li>
                <li><a>Maintainence</a>
                    <ul>
                        <li><asp:HyperLink ID="hypMainRpts" runat="server" NavigateUrl="~/VWA/Maintainence/Maintainence_Reports.aspx">Reports</asp:HyperLink></li>                    
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 	
    </div>
   <div id="contentright"> 
    <div id = "introText">Search VWA Cases</div> 
        <div id = "importbox">
            <fieldset>
                <table style="margin-top:10px;margin-bottom:10px">
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblcontract" runat="server" Text="Contract #:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContract" runat="server" style="border:2px solid #e2e2e2" ValidationGroup="v1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" 
                            ControlToValidate="txtContract" ValidationGroup ="v1" ErrorMessage="Missing Contract #">* Required</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator 
                            id="RegvalSSN" 
                            runat="server"
                            ControlToValidate="txtContract"
                            ValidationExpression="^\d+$"
                            Display="dynamic" ValidationGroup="v1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid Contract #
                            </asp:RegularExpressionValidator>  
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:LinkButton ID="btnContract" runat="server" OnClientClick="this.blur();" ValidationGroup ="v1" Font-Underline="false" CssClass="imgbutton" OnClick="btnContract_Click"><span>Search</span></asp:LinkButton>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblOr1" runat="server" Text="---- or ----" style="margin-left:50px;font-style:italic;color:#87CEEB"></asp:Label>
                        </td>
                    </tr>                    
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblFirst" runat="server" Text="First Name:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="txtFirst" runat="server" style="border:2px solid #e2e2e2" ></asp:TextBox> 
                        </td>
                        <td>                            
                        </td>
                    </tr>
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblLast" runat="server" Text="Last Name:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLast" runat="server" style="border:2px solid #e2e2e2" ></asp:TextBox>
                        </td>
                        <td>                            
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:LinkButton ID="btnName" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" OnClick="btnName_Click"><span>Search</span></asp:LinkButton>
                        </td>                        
                    </tr>
                </table>
            </fieldset>    
        </div>
         <div class="error" id="errorDiv1" runat="server" visible="false">
            <asp:Label ID="lblError1" runat="server" Text=""></asp:Label>
        </div> 
        <br /><br />
        <div id = "EmpListDiv" runat="server" visible="false">
            <div id = "introText">Employees List</div>
        </div> 
        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
            <asp:GridView ID="grdvEmployee" runat="server" 
            AutoGenerateColumns="False" OnRowCommand="grdvEmployee_RowCommand" 
            GridLines="None"  AllowSorting="true" OnSorting="grdvEmployee_onSorting"
            CssClass="tablestyle"  PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif">
            <PagerSettings Mode="NextPreviousFirstLast" />
            <AlternatingRowStyle CssClass="altrowstyle" />
            <HeaderStyle CssClass="headerstyle" />
            <RowStyle CssClass="rowstyle" />               
            <PagerStyle CssClass="customGridpaging" /> 
                <Columns>                    
                    <asp:ButtonField Text="Select" commandname="Select" ButtonType="Link"/>
                    <asp:BoundField HeaderText="Last Name" DataField="Lname" SortExpression="Lname"/>
                    <asp:BoundField HeaderText="First Name" DataField="Fname" SortExpression="Fname" />
                    <asp:BoundField HeaderText="Client#" DataField="ClientId" SortExpression ="ClientId" />
                    <asp:BoundField HeaderText="Group#" DataField="GroupNo" SortExpression ="GroupNo" />  
                    <asp:BoundField HeaderText="Contract#" DataField="ContractNo" SortExpression ="ContractNo" />      
                </Columns>                
            </asp:GridView>            
        </div>              
    </div>   
</asp:Content>


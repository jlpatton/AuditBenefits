<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Non_CA_DCN_History.aspx.cs" Inherits="Anthem_Claims_Non_CA_DCN_History" Title="DCN History" %>
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
                     <ul id = "expand1">
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server"  NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li><li><asp:HyperLink ID="HypClaimsRecon" runat="server" Font-Underline="true" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li></ul>
                </li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Anthem/Reports/Reports.aspx">Reports</asp:Hyperlink></li><li><a>Maintenance</a>
                    <ul>
		                <li><a>Module Maintenance</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypCodes" runat="server" NavigateUrl="~/Anthem/Maintenance/CodesTable.aspx">Codes Table</asp:HyperLink></li><li><asp:HyperLink ID="hypConsole" runat="server" NavigateUrl="~/Anthem/Maintenance/ManagementConsole.aspx">Management Console</asp:HyperLink></li></ul>
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
        <div id="Navigation">
            <div class="menuNav">
                <ul>
	                <li><a class="menuNavone" href="#">Reconciliation</a>
		                <table><tr><td>
		                <ul>
			                <li><a href="California_Claims_Reconciliation.aspx">CA Claims Reconciliation</a></li><li><a href="Non_CA_DF_RF_Reconciliation.aspx">Non-CA Claims Reconciliation</a></li></ul>
		                </td></tr></table>
	                </li>
	                <li><a class="menuNavtwo" href="#">Adjustments</a>
		                <table><tr><td>
		                    <ul>
			                    <li><a href="CA_Recon_Adjustments.aspx">CA Claims Recon Adjustments</a></li><li><a href="Non_CA_Recon_Adjustments.aspx">Non-CA Claims Recon Adjustments</a></li></ul>
		                </td></tr></table>
		            </li>
		            <li><a class="menuNavthree" href="Non_CA_DCN_History.aspx">DCN History</a></li></ul>	                            
            </div>
	    </div>
	    <div id = "introText">DCN History</div> 
        <div style="float:left;width:680px;margin-top:20px;">
        <table><tr><td>        
            <asp:Label ID="lblSearch" runat="server" Text="Search"></asp:Label>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
         </td>
         <td style="width: 30px">
             <asp:LinkButton ID="btnSearch" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="squarebutton" Width="22px" OnClick="btnSearch_Click"><span>OK</span></asp:LinkButton> 
         </td>
         <td width="100px">
         
         </td>
         <td style="margin:20px;">
            <asp:LinkButton ID="LinkButton4" runat="server" OnClick="btn_xl_Click" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
        </td>
         </tr></table>       
        </div>
        <div style="float:left;width:680px;margin-top:20px;" id="historyDiv" runat="server" >            
            <br />
            <asp:Label ID="lbl_error" runat="server" ></asp:Label>
            <br />            
            <asp:GridView ID="grdvHistory" runat="server" CssClass="customGrid" 
            AutoGenerateColumns="false" AllowSorting="true" OnSorting="grdvHistory_Sorting">          
                <Columns> 
                    <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" />                                      
                    <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />                    
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="Reconciled" HeaderText="Reconciled" SortExpression="Reconciled" />                    
                </Columns> 
                <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                         
            </asp:GridView>
        </div>
             <!-- <div style="float:left;width:300px;margin-top:20px;border:1px Solid #DEDEDE;padding:10px" visible="false" >
                <table>
                     <tr>
                        <td>
                            <asp:Label ID="lblDesc" runat="server" Text="Note:"></asp:Label>
                        </td>                                                
                    </tr>
                     <tr>
                        <td>
                            <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
                        </td>                                                
                    </tr>
                </table>
                 <br />
                <table>
                     <tr>
                        <td width="50">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                        </td>
                        <td width="50">
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        </td>
                    </tr>                                             
                </table>                                        
             </div> -->
        </div>                        

</asp:Content>


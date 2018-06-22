<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reconciliation.aspx.cs" Inherits="Anthem_Claims_Reconciliation" Title="California & Non-California Claims Reconciliation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>ANTHEM</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Billing</a>
		            <ul>
		                <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/Anthem/Billing/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypBillingRecon" runat="server" NavigateUrl="~/Anthem/Billing/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
		            </ul>
		        </li>
                <li><a>Claims</a>
                     <ul id = "expand1">
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server"  NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="HypClaimsRecon" runat="server" Font-Underline="true" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
		            </ul>
                </li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Anthem/Reports/Reports.aspx">Reports</asp:Hyperlink></li>
                <li><a>Maintenance</a>
                    <ul>
		                <li><a>Module Maintenance</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypCodes" runat="server" NavigateUrl="~/Anthem/Maintenance/CodesTable.aspx">Codes Table</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypConsole" runat="server" NavigateUrl="~/Anthem/Maintenance/ManagementConsole.aspx">Management Console</asp:HyperLink></li>		                        
		                    </ul>
		                </li>                        
                        <li><a>Current Users</a>
                            <ul>
                                <li><asp:HyperLink ID="hypDash" runat="server" NavigateUrl="~/Anthem/Maintenance/Dashboard.aspx">Dashboard</asp:HyperLink></li>
                                <li><asp:HyperLink ID="hypPref" runat="server" NavigateUrl="~/Anthem/Maintenance/Preferences.aspx">Preferences</asp:HyperLink></li>
                            </ul>
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
			                <li><a href="California_Claims_Reconciliation.aspx">CA Claims Reconciliation</a></li>
			                <li><a href="Non_CA_DF_RF_Reconciliation.aspx">Non-CA Claims Reconciliation</a></li>			                			                
		                </ul>
		                </td></tr></table>
	                </li>
	                <li><a class="menuNavtwo" href="#">Adjustments</a>
		                <table><tr><td>
		                    <ul>
			                    <li><a href="CA_Recon_Adjustments.aspx">CA Claims Recon Adjustments</a></li>
			                    <li><a href="Non_CA_Recon_Adjustments.aspx">Non-CA Claims Recon Adjustments</a></li></ul>
		                </td></tr></table>
		            </li>
		            <li><a class="menuNavthree" href="Non_CA_DCN_History.aspx">DCN History</a></li>	
	            </ul>	                            
            </div>
	    </div>	  
        <div id = "introText">Claims Reconciliation</div>                
    </div>
</asp:Content>


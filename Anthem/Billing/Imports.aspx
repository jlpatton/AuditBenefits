<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Imports.aspx.cs" Inherits="Anthem_Billing_Home" Title="Import International and Domestic Billing Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>ANTHEM</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Billing</a>
		            <ul id = "expand1">
		                <li><asp:HyperLink ID="hypBillingImp" runat="server" Font-Underline="true" NavigateUrl="~/Anthem/Billing/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypBillingRecon" runat="server" NavigateUrl="~/Anthem/Billing/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
		            </ul>
		        </li>
                <li><a>Claims</a>
                     <ul>
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server" NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="HypClaimsRecon" runat="server" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
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
	                <li><a class="menuNavone" href="#">Import Reports
	                <!--[if IE 7]><!--></a><!--<![endif]-->
		                <table><tr><td>
		                <ul>
			                <li><a href="Import_AnthemBill.aspx">Anthem Bill</a></li>
			                <li><a href="Import_HTH_Report.aspx">International HTH File</a></li>
			                <li><a href="Import_GRS_Active_Pilot.aspx">GRS Active Pilot HeadCounts Report</a></li>
			                <li><a href="Import_ADP_Cobra.aspx">ADP Cobra Headcounts Report</a></li>
			                <%--<li><a href="Import_Retiree_MC_Headcount.aspx">Retiree Medicare Headcount Report</a></li>--%>
			                <%-- Combined the Medicare Non Medicare imports into one file - Anthem WO 9-15-2009 --%>
			                <li><a href="Import_Retiree_NM_Headcount.aspx">Retiree Medicare Non-Medicare Headcount Report</a></li></ul>
			                <%--<li><a href="Import_Retiree_NM_Headcount.aspx">Retiree Non-Medicare Headcount Report</a></li></ul>--%>
		                </td></tr></table>
		                <!--[if lte IE 6]></a><![endif]-->
	                </li>
	            </ul>	                            
            </div>
	    </div>	   
        <div id = "introText">Import All the Required Reports</div>                
    </div>
</asp:Content>


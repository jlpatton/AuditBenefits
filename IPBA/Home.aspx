<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="IPBA_Import" Title="Insured Plans Billing & Adjustment Module Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>IPBA</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Import</a>
		            <ul>
		                <li><asp:HyperLink ID="hypGreenbarImp" runat="server" NavigateUrl="~/IPBA/Import_GreenBar_Report.aspx">Greenbar Report</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypADPImp" runat="server" NavigateUrl="~/IPBA/Import_ADP_Cobra.aspx">ADP Cobra Report</asp:HyperLink></li>
		            </ul>
		        </li>		         
                <li><asp:HyperLink ID="hypManAdj" runat="server" NavigateUrl="~/IPBA/HMO_and_HTH_Adjustments.aspx">Billing Adjustments</asp:HyperLink></li>		                
                <li><asp:HyperLink ID="hypHMOReport" runat="server" NavigateUrl="~/IPBA/HTH_HMO_Billing_Report.aspx">HTH/HMO Billing Report</asp:HyperLink></li> 
                <li><asp:HyperLink ID="hypMaintenance" runat="server" NavigateUrl="~/IPBA/Maintenance.aspx">Maintenance</asp:HyperLink></li>          		                     		            
            </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
     <div id="contentright">
        <div id = "introText">Insured Plans Billings & Adjustment Module</div>                
    </div>
</asp:Content>


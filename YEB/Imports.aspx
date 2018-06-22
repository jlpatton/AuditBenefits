<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Imports.aspx.cs" Inherits="YEB_Imports" Title="YEB Imports Page" %>

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
       <div id="Navigation">
            <div class="menuNav">
                <ul>
	                <li><a class="menuNavone" href="#">Imports </a>
		                <table><tr><td>
		                 <ul>
		                 <li><a href="ImportCobra .aspx">Import COBRA Participants</a></li>
		                   <li><a href="ImportExpatGroup.aspx">Import Expat Group</a></li>
		                   <li><a href="ImportYEBOnline.aspx">Import YEB OnlineGroup</a></li>
		                   <li><a href="ImportWWRetiree.aspx">Import WW Retiree</a></li>
		                   <li><a href="ImportLOAActives.aspx">Import LOA & Actives</a></li></ul>
		                </td></tr></table>
	                </li>
	            </ul>	                            
            </div>
	    </div>	  
        <div id = "introText">Imports</div>                
    </div>

</asp:Content>
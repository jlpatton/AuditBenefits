<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_Retiree_MC_Headcount.aspx.cs" Inherits="Anthem_Billing_Import_Retiree_MC_Headcount1" Title="Import Retiree Medicare Headcount File" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

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
			               <%-- <li><a href="Import_Retiree_MC_Headcount.aspx">Retiree Medicare Headcount Report</a></li>--%>
			                <li><a href="Import_Retiree_NM_Headcount.aspx">Retiree Non-Medicare Headcount Report</a></li></ul>
		                </td></tr></table>
		                <!--[if lte IE 6]></a><![endif]-->
	                </li>
	            </ul>	                            
            </div>
	    </div> 
	     <div id = "importbox">	     
            <fieldset>
                <legend>Import Retiree Medicare Headcount Report</legend>         
                   <div style="margin-top:20px;padding-left:12px">
                        <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:"></asp:Label>
                        <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                        </asp:DropDownList>
                        <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" ></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" ForeColor="#7D7D7D"
                        Text="(yyyymm)"></asp:Label>
                        <asp:Button ID="btnAddYrmo" runat="server" Text="ADD" ValidationGroup="yrmoGroup" OnClick="btn_ADDYrmo" Visible = "false"/>
                        <asp:Button ID="btnCancelYrmo" runat="server"  Text="Cancel" OnClick="btn_CancelYrmo" Visible = "false" />
                        <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        Display="Dynamic" 
                        Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup"
                        >
                        Please enter YRMO
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                        Display="Static"
                        Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                        Please enter YRMO in format 'yyyymm'
                        </asp:RegularExpressionValidator> 
                    </div>          
                    <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                        <asp:View id="view_main" runat="server">
                            <br /> 
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" ></asp:Label><br />
                            <br />                                
                            <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="xls"/>                             
                            <asp:Button ID="btn_import" runat="server" style="margin-left:50px;" ValidationGroup="fileGroup" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value; javascript:showWait();" Text="Import File" OnClick="btn_import_Click" /><br />
                            <br />
                            <br />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </asp:View> 
                        <asp:View ID="view_empty" runat="server">
                        <div class="userParaI">
                            <p>Please enter the YRMO not listed in the drop down list.</p>    
                            <br />
                        </div>                         
                        </asp:View>
                        <asp:View id="view_reimport" runat="server">
                            <br />
                            <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" ></asp:Label><br />
                            <br /> 
                            <asp:Label ID="lbl_reimport" runat="server"></asp:Label>&nbsp;<br />
                            <br />
                            <asp:Button ID="btn_reimport" runat="server" Text="Re-Import File" OnClick="btn_reimport_Click" />
                            &nbsp;&nbsp;                            
                            <br />
                            <br />
                        </asp:View>
                        <asp:View id="view_result" runat="server">
                            <br />
                            <br />
                            <asp:Label ID="lbl_result" runat="server"></asp:Label>
                            <br />
                            <br />  
                        </asp:View> 
                    </asp:MultiView>                      
            </fieldset>
	    </div>              
    </div>
</asp:Content>


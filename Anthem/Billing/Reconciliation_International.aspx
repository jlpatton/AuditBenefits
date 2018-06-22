<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reconciliation_International.aspx.cs" Inherits="Anthem_Billing_Reconciliation_International" Title="International Reconciliation" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>ANTHEM</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Billing</a>
		            <ul id = "expand1">
		                <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/Anthem/Billing/Imports.aspx">Import</asp:HyperLink></li><li><asp:HyperLink ID="hypBillingRecon" runat="server" Font-Underline="true" NavigateUrl="~/Anthem/Billing/Reconciliation.aspx">Reconciliation</asp:HyperLink></li></ul>
		        </li>
                <li><a>Claims</a>
                     <ul>
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server" NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li><li><asp:HyperLink ID="HypClaimsRecon" runat="server" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li></ul>
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
	                <li><a class="menuNavone" href="#">Reconciliation<!--[if IE 7]><!--></a><!--<![endif]-->
		                <table><tr><td>
		                <ul>
			                <li><a href="Reconciliation_Domestic.aspx">Domestic Reconciliation</a></li>
			                <li><a href="Reconciliation_EAP.aspx">EAP Reconciliation</a></li>
			                <li><a href="Reconciliation_International.aspx">International Reconciliation</a></li>
			            </ul>
		                </td></tr></table>
		                <!--[if lte IE 6]></a><![endif]-->
	                </li>
	            </ul>	                            
            </div>
	    </div> 
	     <div id = "importbox">	  
            <fieldset>
                <legend>International Reconciliation</legend>
                    <div style="margin-top:20px;padding-left:12px">
                        <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:"></asp:Label>
                        <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                        </asp:DropDownList>
                        <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" ></asp:TextBox>
                        <asp:Label ID="Label1" runat="server" ForeColor="#7D7D7D"
                        Text="(yyyymm)"></asp:Label>
                        <asp:Button ID="btnAddYrmo" runat="server" Text="ADD" ValidationGroup="yrmoGroup" OnClick="btn_ADDYrmo" Visible = "false"/>
                        <asp:Button ID="btnCancelYrmo" runat="server"  Text="Cancel" OnClick="btn_CancelYrmo" Visible = "false" />
                        <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        Display="Dynamic" 
                        Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup"
                        >
                        Please enter YRMO
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                        Display="Static"
                        Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                        Please enter YRMO in format 'yyyymm'
                        </asp:RegularExpressionValidator> 
                    </div> 
                            <br />            
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label><br />    
                     <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                        <asp:View id="view_main" runat="server">                            
                            <br />                                    
                            <br />           
                            <asp:Button ID="btn_reconcile" runat="server" Text="Reconcile" OnClick="btn_reconcile_Click" style="margin-left:50px;margin-bottom:20px;" /><br />
                            <br />
                            <br />            
                            <br />            
                        </asp:View>
                        <asp:View ID="view_empty" runat="server">
                            <div class="userParaI">
                                <p>Please enter the YRMO not listed in the drop down list.</p>    
                                <br />
                            </div>                         
                        </asp:View>
                        <asp:View id="view_reconAgn" runat="server">                           
                            <asp:Label ID="lbl_reconAgn" runat="server"></asp:Label>&nbsp;<br />
                            <br />
                            <asp:Button ID="btn_reconAgn" runat="server" Text="Reconcile" OnClick="btn_reconAgn_Click" style="margin-left:50px;margin-bottom:20px;"  /> 
                            <br />
                        </asp:View>
                        <asp:View id="view_result" runat="server">
                            <br />
                            <br />
                            <asp:Button ID="btn_genRpt" runat="server" Text="Generate Excel Report" OnClick="btn_genRpt_Click"  /><br />
                            <br />                                                   
                            
                        </asp:View> 
                </asp:MultiView>                    
            </fieldset>
	    </div>
	    <div id = "resultDiv" runat="server">
	        <div id = "introText">Reconciliation Report</div> 
	        <div style="float:left;width:650px;margin-top:20px;" >	    
	            <asp:GridView ID="grdvResult" runat="server"  AutoGenerateColumns="False" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanging="grdvResult_PageIndexChanging" OnSorting="grdvResult_Sorting" >
                <PagerStyle CssClass="customGridpaging"  />
                     <Columns>
                        <asp:BoundField DataField="rcn_yrmo" HeaderText="YRMO" SortExpression="rcn_yrmo" />
                        <asp:BoundField DataField="anthcd_plancd" HeaderText="Anthem Group Suffix" SortExpression="anthcd_plancd" />
                        <asp:BoundField DataField="anthcd_covgcd" HeaderText="Anthem Covg. Code" SortExpression="anthcd_covgcd" HtmlEncode="false"/>
                        <asp:BoundField DataField="rcn_prism_count" HeaderText="EBA Count" SortExpression="rcn_prism_count"/>
                        <asp:BoundField DataField="rcn_prism_amt" HeaderText="EBA Amount" SortExpression="rcn_prism_amt"/>
                        <asp:BoundField DataField="rcn_anth_count" HeaderText="Anthem Count" SortExpression="rcn_anth_count"/> 
                        <asp:BoundField DataField="rcn_anth_amt" HeaderText="Anthem Amount" SortExpression="rcn_anth_amt"/> 
                        <asp:BoundField DataField="rcn_var" HeaderText="EBA Variance" SortExpression="rcn_var" />  
                        <asp:BoundField DataField="rcn_per" HeaderText="EBA vs Anthem % Count Variance" SortExpression="rcn_per" />
                        <asp:BoundField DataField="threshold" HeaderText="Threshold" SortExpression="threshold" /> 
                        <asp:BoundField DataField="var_threshold" HeaderText="Threshold  Level" SortExpression="var_threshold" HtmlEncode="false" />                        
                    </Columns>
                </asp:GridView> 
            </div> 
        </div>
        <div id = "detailDiv" runat="server">
            <div id = "introText"  >Details Report</div> 
	        <div class="userParaR">
                <p>List of employees who are in HTH File from EBA and not in Anthem International File                            
                </p>    
            </div>  
	        <div id="hthDiv" runat="server" style="float:left;width:650px;margin-top:20px;" Visible="false" >	        
                <asp:GridView ID="grdvHTH" runat="server" AutoGenerateColumns="true"
                 CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateSelectButton="true" 
                AllowPaging="true" AllowSorting="true" OnPageIndexChanging="grdvHTH_PageIndexChanging" OnSorting="grdvHTH_Sorting">
                <PagerStyle CssClass="customGridpaging"  />
                </asp:GridView>
            </div>            
            <div class="userParaR">
                <p>List of employees who are in Anthem International File and not in HTH File                            
                </p>    
            </div>  
            <div id="anthdiv" runat="server" style="float:left;width:650px;margin-top:20px;" Visible="false">
                <asp:GridView ID="grdvAnth" runat="server" AutoGenerateColumns="true"
                 CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" 
                AutoGenerateSelectButton="true" AllowPaging="true" AllowSorting="true"
                OnPageIndexChanging="grdvAnth_PageIndexChanging" OnSorting="grdvAnth_Sorting">
                </asp:GridView>
	        </div>	       
	    </div> 	    
    </div>
    <cc2:SmartScroller ID="SmartScroller2" runat="server">
    </cc2:SmartScroller>
</asp:Content>


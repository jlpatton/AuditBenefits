<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reconciliation_EAP.aspx.cs" Inherits="Anthem_Billing_Reconciliation_EAP" Title="EAP Reconciliation" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

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
			                <li><a href="Reconciliation_Domestic.aspx">Domestic Reconciliation</a></li><li><a href="Reconciliation_EAP.aspx">EAP Reconciliation</a></li><li><a href="Reconciliation_International.aspx">International Reconciliation</a></li></ul>
		                </td></tr></table>
		                <!--[if lte IE 6]></a><![endif]-->
	                </li>
	            </ul>	                            
            </div>
	    </div>
	    <div id = "importbox">	  
            <fieldset>
                <legend>EAP Reconciliation</legend>
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
	        <asp:MultiView id="MultiView2" runat="server" ActiveViewIndex="0">
                    <asp:View id="viewResult" runat="server">
                        <div id = "introText">Reconciliation Report</div> 
	                    <div style="float:left;width:650px;margin-top:20px;" >
	                    <asp:GridView ID="grdvResult" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" DataKeyNames ="rcn_yrmo"
                            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" AutoGenerateSelectButton="true"
                            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" AllowPaging="true" 
                            AllowSorting="true" OnPageIndexChanging="grdvResult_PageIndexChanging" OnSorting="grdvResult_Sorting" OnRowCommand="grdvResult_OnRowCommand">
                            <PagerStyle CssClass="customGridpaging"  />
                            <Columns>
                               <asp:BoundField DataField="rcn_yrmo" HeaderText="YRMO" SortExpression="rcn_yrmo"/>
                                <asp:BoundField DataField="rcn_prism_count" HeaderText="EBA Count" SortExpression="rcn_prism_count"/>
                                <asp:BoundField DataField="rcn_prism_amt" HeaderText="EBA Amount" SortExpression="rcn_prism_amt"/>
                                <asp:BoundField DataField="rcn_anth_count" HeaderText="Anthem Count" SortExpression="rcn_anth_count"/> 
                                <asp:BoundField DataField="rcn_anth_amt" HeaderText="Anthem Amount" SortExpression="rcn_anth_amt"/> 
                                <asp:BoundField DataField="rcn_var" HeaderText="EBA Variance" SortExpression="rcn_var" />  
                                <asp:BoundField DataField="rcn_per" HeaderText="EBA vs Anthem % Count Variance" SortExpression="rcn_per" />
                               <asp:BoundField DataField="threshold" HeaderText="Threshold" SortExpression="threshold" /> 
                               <asp:BoundField DataField="var_threshold" HeaderText="Threshold Level" SortExpression="var_threshold" HtmlEncode="false" />              
                           </Columns>
                        </asp:GridView>
                        </div> 
                    </asp:View>
                    <asp:View id="viewDetails" runat="server">
                        <div style="float:left;width:650px;margin-top:20px;" >
                            <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_OnClick">Back</asp:LinkButton>
                        </div>
                        <div id = "introText">Headcount details from EBA reports </div>
                        <div style="float:left;width:650px;margin-top:20px;" >                     
	                        <asp:GridView ID="grdvPEAP" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" ShowHeader="true"
                            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="False" AllowPaging="True" 
                            AllowSorting="True" DataSourceID="SqlDataSource1">
                            <PagerStyle CssClass="customGridpaging"  />                            
                                <Columns>
                                    <asp:BoundField DataField="hdct_yrmo" HeaderText="YRMO" SortExpression="hdct_yrmo" />
                                    <asp:BoundField DataField="hdct_source" HeaderText="Source" SortExpression="hdct_source" />
                                    <asp:BoundField DataField="hdct_count" HeaderText="Count" SortExpression="hdct_count" />                                    
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [hdct_yrmo], 
                                CASE [hdct_source] 
                                    WHEN 'ADP' THEN 'ADP Cobra'
                                    WHEN 'GRS' THEN 'GRS Pilot'
                                    WHEN 'HTH' THEN 'HTH International' 
                                    WHEN 'RET_M' THEN 'Retiree Medicare'
                                    WHEN 'RET_NM' THEN 'Retiree Non Medicare'
                                END AS [hdct_source],
                                SUM(hdct_count) AS [hdct_count] FROM [Headcount] WHERE [hdct_yrmo] = @rcn_yrmo 
                                AND (hdct_source = 'ADP' or hdct_source = 'GRS' or hdct_source = 'HTH' or hdct_source = 'RET_NM' or hdct_source = 'RET_M')
                                GROUP BY [hdct_yrmo],[hdct_source]">
                                <SelectParameters>
                                <asp:ControlParameter ControlID="grdvResult" Name="rcn_yrmo" PropertyName="SelectedValue"
                                    Type="string" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div> 
                        <div id = "introText">HMO Headcount details from EBA reports </div>
                        <div style="float:left;width:650px;margin-top:20px;" >                     
	                        <asp:GridView ID="grdvHMO" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" ShowHeader="true"
                            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="False" AllowPaging="True" 
                            AllowSorting="True" DataSourceID="SqlDataSource2">
                            <PagerStyle CssClass="customGridpaging"  />                            
                                <Columns>
                                    <asp:BoundField DataField="hmo_yrmo" HeaderText="YRMO" SortExpression="hdct_yrmo" />
                                    <asp:BoundField DataField="hmo_source" HeaderText="Source" SortExpression="hdct_source" />
                                    <asp:BoundField DataField="hmo_count" HeaderText="Count" SortExpression="hdct_count" />                                    
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [hmo_yrmo], 
                                CASE [hmo_source]                                    
                                    WHEN 'GRS' THEN 'GRS Pilot - HMO'                                    
                                    WHEN 'RET_M' THEN 'Retiree Medicare - HMO'
                                    WHEN 'RET_NM' THEN 'Retiree Non Medicare - HMO'
                                END AS [hmo_source],
                                SUM(hmo_count) AS [hmo_count] FROM [billing_HMO] WHERE [hmo_yrmo] = @rcn_yrmo 
                                AND (hmo_source = 'GRS' or hmo_source = 'RET_NM' or hmo_source = 'RET_M')
                                GROUP BY [hmo_yrmo],[hmo_source]">
                                <SelectParameters>
                                <asp:ControlParameter ControlID="grdvResult" Name="rcn_yrmo" PropertyName="SelectedValue"
                                    Type="string" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div> 
                    </asp:View>
                </asp:MultiView>                
        </div>             
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>

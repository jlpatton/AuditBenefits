<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CA_Recon_Adjustments.aspx.cs" Inherits="Anthem_Claims_CA_Recon_Adjustments" Title="California Reconciliation Adjustements" %>
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
        <div id="containerTab">
              <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal" 
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Amount Mistmatch" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Unmatched from Anthem" Value="1"></asp:MenuItem> 
                    <asp:MenuItem Text="Unmatched from BOA" Value="2"></asp:MenuItem> 
                    <asp:MenuItem Text="Duplicate Checks" Value="3"></asp:MenuItem> 
                    <asp:MenuItem Text="Report" Value="4"></asp:MenuItem>                  
                </Items>
                <StaticMenuItemStyle CssClass="tabNav" />
                <StaticSelectedStyle CssClass="selectedNavTab" />
                <StaticHoverStyle CssClass="selectedNavTab1" />                     
            </asp:Menu>
            <div id = "navView">                
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View1" runat="server"> 
                     <div id = "introText">Mismatched Amount Carry Forwards with Aging
                         <asp:Label ID="lblH1" runat="server" Text=""></asp:Label></div> 
                        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                             <asp:View ID="amountMismatch" runat="server">
                                <div style="float:left;width:650px;margin-top:20px;" >                            
                                    <asp:GridView ID="grdvAmntMis" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  AllowPaging="true" 
                                    AllowSorting="true" AutoGenerateSelectButton="true"
                                    OnSorting="grdvAmntMis_Sorting" DataKeyNames="Check#"
                                    OnPageIndexChanging="grdvAmntMis_PageIndexChanging" 
                                    OnRowCommand="grdvAmntMis_OnRowCommand">
                                    <PagerStyle CssClass="customGridpaging"  />
                                    <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="Check#" HeaderText="Check#" SortExpression="Check#" />
                                            <asp:BoundField DataField="Anthem Amount" HeaderText="Anthem Amount" SortExpression="Anthem Amount" />
                                            <asp:BoundField DataField="BOA Amount" HeaderText="BOA Amount" SortExpression="BOA Amount" />
                                            <asp:BoundField DataField="Diff Amount" HeaderText="Diff Amount" SortExpression="Diff Amount" />
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO"  />
                                    </Columns>                                            
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="viewAmntMisDetails" runat="server">
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <asp:LinkButton ID="lnkAmntMis" runat="server" OnClick="lnkAmntMis_OnClick">Back to report</asp:LinkButton>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                     <asp:Label ID="lblNote1" runat="server" Text="Note: Forced Reconciliation will Reconcile all the above records with same Claim id"  ForeColor="red" Font-Bold="true" Visible = "false"></asp:Label>                                    
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <div id = "subIntroText">Selected Check Number Record Details</div>
                                    <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr1" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                   <div style="float:left;width:350px;margin-top:20px;" >
                                        <asp:DetailsView ID="dtvAmntMis" runat="server" Height="50px" Width="320px" 
                                            HeaderText="Detail Record" CssClass="tablestyle">                                                                                
                                            <AlternatingRowStyle CssClass="altrowstyle" />
                                            <HeaderStyle CssClass="headerstyleDetails" />
                                            <RowStyle CssClass="rowstyle" />                                            
                                        </asp:DetailsView>
                                    </div>
                                     <div style="float:left;width:300px;margin-top:20px;border:1px Solid #DEDEDE;padding:10px" >
                                         <asp:Label ID="lblHeading1" runat="server" Text="Forced Reconcile" Width="280px" CssClass="labelreconcile1"></asp:Label>
                                        <br /><br />
                                        <table>                                           
                                            <tr>
                                                <td>
                                                     <asp:Label ID="lblReconcile1" runat="server" Text="Reconciled:"></asp:Label>
                                                </td>
                                                <td>
                                                     <asp:CheckBox ID="cbxReconcile1" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblerr1" runat="server" Text="" ForeColor="red"></asp:Label>
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <table>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="lblDesc1" runat="server" Text="Note:"></asp:Label>
                                                </td>                                                
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDesc1" runat="server" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <table>
                                             <tr>
                                                <td width="50">
                                                    <asp:Button ID="btnOK1" ValidationGroup="dfnorfValid" runat="server" Text="Submit" OnClick="btnOK1_Click" />
                                                </td>
                                                <td width="50">
                                                    <asp:Button ID="btnClear1" runat="server" Text="Clear" OnClick="btnClear1_Click" />
                                                </td>
                                            </tr>                                             
                                        </table>                                        
                                     </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>                        
                    </asp:View>
                     <asp:View ID="View2" runat="server">                     
                        <div id = "introText">Un-Matched Carry Forwards from Anthem with Aging
                                            <asp:Label ID="lblH2" runat="server" Text=""></asp:Label></div>
                            <asp:MultiView ID="MultiView3" runat="server" ActiveViewIndex="0">
                                <asp:View ID="unmatchedAnthem" runat="server"> 
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:GridView ID="grdvUnmatchedAnth" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                                        AllowSorting="true" AutoGenerateSelectButton="true"
                                        OnSorting="grdvUnmatchedAnth_Sorting" DataKeyNames="Check#"
                                        OnPageIndexChanging="grdvUnmatchedAnth_PageIndexChanging" 
                                        OnRowCommand="grdvUnmatchedAnth_OnRowCommand">
                                        <PagerStyle CssClass="customGridpaging" /> 
                                        <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="Check#" HeaderText="Check#" SortExpression="Check#" />                                            
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO"  />
                                        </Columns>          
                                        </asp:GridView>
                                     </div> 
                                </asp:View>
                                <asp:View ID="viewUnMatchedAnthDetails" runat="server">
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:LinkButton ID="lnkUnmatchedAnth" runat="server" OnClick="lnkUnmatchedAnth_OnClick">Back to report</asp:LinkButton>
                                    </div>                                
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <div id = "subIntroText">Selected Check Number Record Details</div>
                                    <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr2" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                   <div style="float:left;width:350px;margin-top:20px;" >
                                        <asp:DetailsView ID="dtvAnth" runat="server" Height="50px" Width="320px" 
                                            HeaderText="Detail Record" CssClass="tablestyle">                                                                                
                                            <AlternatingRowStyle CssClass="altrowstyle" />
                                            <HeaderStyle CssClass="headerstyleDetails" />
                                            <RowStyle CssClass="rowstyle" />                                            
                                        </asp:DetailsView>
                                    </div>
                                     <div style="float:left;width:300px;margin-top:20px;border:1px Solid #DEDEDE;padding:10px" >
                                         <asp:Label ID="lblHeading2" runat="server" Text="Forced Reconcile" Width="280px" CssClass="labelreconcile1"></asp:Label>
                                        <br /><br />
                                        <table>                                           
                                            <tr>
                                                <td>
                                                     <asp:Label ID="lblReconcile2" runat="server" Text="Reconciled:"></asp:Label>
                                                </td>
                                                <td>
                                                     <asp:CheckBox ID="cbxReconcile2" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblerr2" runat="server" Text="" ForeColor="red"></asp:Label>
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <table>
                                             <tr>
                                                <td>
                                                    <asp:Label ID="lblDesc2" runat="server" Text="Note:"></asp:Label>
                                                </td>                                                
                                            </tr>
                                             <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDesc2" runat="server" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <table>
                                             <tr>
                                                <td width="50">
                                                    <asp:Button ID="btnOK2" ValidationGroup="anthValid" runat="server" Text="Submit" OnClick="btnOK2_Click" />
                                                </td>
                                                <td width="50">
                                                    <asp:Button ID="btnClear2" runat="server" Text="Clear" OnClick="btnClear2_Click" />
                                                </td>
                                            </tr>                                             
                                        </table>                                        
                                     </div>
                                </div>
                                </asp:View>
                            </asp:MultiView> 
                    </asp:View>
                    <asp:View ID="View3" runat="server">                     
                        <div id = "introText">Un-Matched Carry Forwards from BOA with Aging
                                        <asp:Label ID="lblH3" runat="server" Text=""></asp:Label></div>
                        <asp:MultiView ID="MultiView4" runat="server" ActiveViewIndex="0">
                            <asp:View ID="unmatchedBOA" runat="server"> 
                                <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:GridView ID="grdvUnmatchedBOA" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                                        AllowSorting="true" AutoGenerateSelectButton="true"
                                        OnSorting="grdvUnmatchedBOA_Sorting" DataKeyNames="Check#"
                                        OnPageIndexChanging="grdvUnmatchedBOA_PageIndexChanging" 
                                        OnRowCommand="grdvUnmatchedBOA_OnRowCommand">
                                        <PagerStyle CssClass="customGridpaging" /> 
                                        <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="Check#" HeaderText="Check#" SortExpression="Check#" />                                            
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO"  />
                                        </Columns>          
                                        </asp:GridView>
                                     </div> 
                            </asp:View>
                            <asp:View ID="viewUnmatchedBOADetails" runat="server">
	                            <div style="float:left;width:680px;margin-top:20px;" >
		                            <asp:LinkButton ID="lnkUnmatchedBOA" runat="server" OnClick="lnkUnmatchedBOA_OnClick">Back to report</asp:LinkButton>
	                            </div>
	                            <div style="float:left;width:680px;margin-top:20px;" >
		                            <div id = "subIntroText">Selected Check Number Record Details</div>
		                             <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr3" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
	                               <div style="float:left;width:350px;margin-top:20px;" >
			                            <asp:DetailsView ID="dtvUnmatchedBOA" runat="server" Height="50px" Width="320px" 
				                            HeaderText="Detail Record" CssClass="tablestyle">                                                                                
				                            <AlternatingRowStyle CssClass="altrowstyle" />
				                            <HeaderStyle CssClass="headerstyleDetails" />
				                            <RowStyle CssClass="rowstyle" />                                            
			                            </asp:DetailsView>
		                            </div>
		                             <div style="float:left;width:300px;margin-top:20px;border:1px Solid #DEDEDE;padding:10px" >
			                             <asp:Label ID="lblHeading3" runat="server" Text="Forced Reconcile" Width="280px" CssClass="labelreconcile1"></asp:Label>
			                            <br /><br />
			                            <table>                                           
				                            <tr>
					                            <td>
						                             <asp:Label ID="lblReconcile3" runat="server" Text="Reconciled:"></asp:Label>
					                            </td>
					                            <td>
						                             <asp:CheckBox ID="cbxReconcile3" runat="server" />
					                            </td>
					                            <td>
						                            <asp:Label ID="lblerr3" runat="server" Text="" ForeColor="red"></asp:Label>
					                            </td>                                                
				                            </tr>
			                            </table>
			                            <table>
				                             <tr>
					                            <td>
						                            <asp:Label ID="lblDesc3" runat="server" Text="Note:"></asp:Label>
					                            </td>					                            
				                            </tr>
				                             <tr>
					                            <td>
						                            <asp:TextBox ID="txtDesc3" runat="server" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
					                            </td>                                                
				                            </tr>
			                            </table>
			                            <table>
				                             <tr>
					                            <td width="50">
						                            <asp:Button ID="btnOK3" ValidationGroup="boaValid" runat="server" Text="Submit" OnClick="btnOK3_Click" />
					                            </td>
					                            <td width="50">
						                            <asp:Button ID="btnClear3" runat="server" Text="Clear" OnClick="btnClear3_Click" />
					                            </td>
				                            </tr>                                             
			                            </table>                                        
		                             </div>
	                            </div>
                            </asp:View>
                        </asp:MultiView>
                    </asp:View>
                    <asp:View ID="View4" runat="server">                     
                        <div id = "introText">Duplicate Check Carry Forwards with Aging
                                    <asp:Label ID="lblH4" runat="server" Text=""></asp:Label></div>
                        <asp:MultiView ID="MultiView5" runat="server" ActiveViewIndex="0">
                            <asp:View ID="dupAdj" runat="server"> 
                                 <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:GridView ID="grdvDups" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                                        AllowSorting="true" AutoGenerateSelectButton="true"
                                        OnSorting="grdvDups_Sorting" DataKeyNames="Check#"
                                        OnPageIndexChanging="grdvDups_PageIndexChanging" 
                                        OnRowCommand="grdvDups_OnRowCommand">
                                        <PagerStyle CssClass="customGridpaging" /> 
                                        <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="Check#" HeaderText="Check#" SortExpression="Check#" />
                                            <asp:BoundField DataField="Anthem Ctr" HeaderText="Anthem Ctr" SortExpression="Anthem Ctr" />
                                            <asp:BoundField DataField="BOA Ctr" HeaderText="BOA Ctr" SortExpression="BOA Ctr" />
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO"  />
                                        </Columns>          
                                        </asp:GridView>
                                     </div> 
                            </asp:View>
                            <asp:View ID="viewDupsDetails" runat="server">
                                <div style="float:left;width:680px;margin-top:20px;" >
	                                <asp:LinkButton ID="lnkDups" runat="server" OnClick="lnkDups_OnClick">Back to report</asp:LinkButton>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
	                                <asp:Label ID="lblDupDetDups" 
	                                style="border-bottom: 1px dotted #DEDEDE;	
			                                height:18px;color: #40353D;
			                                font-family: Georgia, Verdana, Arial, sans-serif; 
			                                font-size:14px;	" runat="server" Text="Selected Check Number Record Details"  Visible = "false"></asp:Label>
	                                <br /><br />
	                                 <div class="scroller" style ="width:700px; height:250px;" >
	                                <asp:GridView ID="grdvDupsDupDetails" runat="server" Visible="false" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
	                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="true"
	                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
	                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
	                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  AllowPaging="true" 	                                
	                                DataKeyNames="Check#">
	                                <PagerStyle CssClass="customGridpaging"  /> 
	                                </asp:GridView>
	                                </div>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
	                                 <asp:Label ID="lblNote4" runat="server" Text="Note: Forced Reconciliation will Reconcile all the above records with same Check No"  ForeColor="red" Font-Bold="true" Visible = "false"></asp:Label>                                    
                                </div>
                                 <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr4" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                <div style="float:left;width:680px;margin-top:20px;" >			
	                                 <div style="float:left;width:300px;margin-top:20px;border:1px Solid #DEDEDE;padding:10px" >
		                                 <asp:Label ID="lblHeading4" runat="server" Text="Forced Reconcile" Width="280px" CssClass="labelreconcile1"></asp:Label>
		                                <br /><br />
		                                <table>                                           
			                                <tr>
				                                <td>
					                                 <asp:Label ID="lblReconcile4" runat="server" Text="Reconciled:"></asp:Label>
				                                </td>
				                                <td>
					                                 <asp:CheckBox ID="cbxReconcile4" runat="server" />
				                                </td>
				                                <td>
					                                <asp:Label ID="lblerr4" runat="server" Text="" ForeColor="red"></asp:Label>
				                                </td>                                                
			                                </tr>
		                                </table>
		                                <table>
			                                 <tr>
				                                <td>
					                                <asp:Label ID="lblDesc4" runat="server" Text="Note:"></asp:Label>
				                                </td>				                                
			                                </tr>
			                                 <tr>
				                                <td>
					                                <asp:TextBox ID="txtDesc4" runat="server" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
				                                </td>                                                
			                                </tr>
		                                </table>
		                                <table>
			                                 <tr>
				                                <td width="50">
					                                <asp:Button ID="btnOK4" ValidationGroup="dupsValid" runat="server" Text="Submit" OnClick="btnOK4_Click" />
				                                </td>
				                                <td width="50">
					                                <asp:Button ID="btnClear4" runat="server" Text="Clear" OnClick="btnClear4_Click" />
				                                </td>
			                                </tr>                                             
		                                </table>                                        
	                                 </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </asp:View>
                     <asp:View ID="View5" runat="server">
                     <div id = "introText">CA Claims Reports after Adjustments(if any)</div> 
                        <div style="float:left;margin-top:20px;padding-left:12px">
                            <table>
                                <tr>
                                    <td>
                                         <asp:Label ID="lblErrRep" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </td>  
                                </tr>
                                <tr>
                                    <td height="30px">
                                        <asp:LinkButton ID="lnkAdjCF" runat="server" ForeColor="#5D478B" OnClick="lnkAdjCF_OnClick"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30px">
                                         <asp:LinkButton ID="lnkAdjMatched" runat="server" ForeColor="#5D478B" OnClick="lnkAdjMatched_OnClick"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <br /><br />  
                        </div>  
                        <div id = "introText">Adjustments Audit Report by YRMO</div> 
                          <div style="float:left;margin-top:20px;padding-left:12px">
                            <table>
                            <tr>
                                 <td>
                                    <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtYRMO"
                                        Display="Dynamic" 
                                        Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup"
                                        >
                                        Please enter YRMO
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>                                   
                                    <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtYRMO"
                                    ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                                    Display="Dynamic"
                                    Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                                    Please enter YRMO in format 'yyyymm'
                                    </asp:RegularExpressionValidator> 
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td style="text-align:right">
                                    <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:"></asp:Label> 
                                </td>
                                <td width="300px">
                                    <asp:TextBox ID="txtYRMO" runat="server"></asp:TextBox>
                                    <asp:Label ID="Label1" runat="server" ForeColor="#7D7D7D"
                                    Text="(yyyymm)"></asp:Label>                        
                                </td> 
                                <td>
                                   <asp:Button ID="btnReport" runat="server" Text="Generate Report" ValidationGroup="yrmoGroup" OnClick="btn_Generate" />                             
                                </td>                                
                            </tr>
                          </table> 
                          <br /><br />  
                        </div> 
                     
                     </asp:View>
                </asp:MultiView>
            </div>           
        </div>
    </div>
</asp:Content>
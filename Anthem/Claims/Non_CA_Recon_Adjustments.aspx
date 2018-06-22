<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Non_CA_Recon_Adjustments.aspx.cs" Inherits="Anthem_Claims_Non_CA_Recon_Adjustments" Title="Non-California Reconciliation Adjustments" %>
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
                    <asp:MenuItem Text="DF no RF Aging" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="DF/RF Aging" Value="1"></asp:MenuItem> 
                    <asp:MenuItem Text="DFRF Amount mismatch Aging" Value="2"></asp:MenuItem> 
                    <asp:MenuItem Text="Report" Value="3"></asp:MenuItem>                  
                </Items>
                <StaticMenuItemStyle CssClass="tabNav" />
                <StaticSelectedStyle CssClass="selectedNavTab" />
                <StaticHoverStyle CssClass="selectedNavTab1" />                     
            </asp:Menu>
            <div id = "navView">                
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View1" runat="server"> 
                     <div id = "introText">DF no RF Aging Report & Adjustment 
                         <asp:Label ID="lblH1" runat="server" Text=""></asp:Label></div> 
                        <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                             <asp:View ID="viewDFnoRF" runat="server">
                                <div style="float:left;width:650px;margin-top:20px;" >                            
                                    <asp:GridView ID="grdvDFnoRF" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  AllowPaging="true" 
                                    AllowSorting="true" AutoGenerateSelectButton="true"
                                    OnSorting="grdvDFnoRF_Sorting" DataKeyNames="Claim #"
                                    OnPageIndexChanging="grdvDFnoRF_PageIndexChanging" 
                                    OnRowCommand="grdvDFnoRF_OnRowCommand">
                                    <PagerStyle CssClass="customGridpaging"  /> 
                                        <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" />
                                            <asp:BoundField DataField="Claim #" HeaderText="Claim #" SortExpression="Claim #" />
                                            <asp:BoundField DataField="Paid Date" HeaderText="Paid Date" SortExpression="Paid Date" />
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO" dataformatstring = "{0:N}"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO" dataformatstring = "{0:N}"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO" dataformatstring = "{0:N}"  />
                                        </Columns>   
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="viewDFnoRFDetails" runat="server">
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <asp:LinkButton ID="lnkDFnoRF" runat="server" OnClick="lnkDFnoRF_OnClick">Back to report</asp:LinkButton>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <asp:Label ID="lblDupDetDFnoRF" 
                                    style="border-bottom: 1px dotted #DEDEDE;	
	                                        height:18px;color: #40353D;
	                                        font-family: Georgia, Verdana, Arial, sans-serif; 
	                                        font-size:14px;	" runat="server" Text="All Records with same Claimid"  Visible = "false"></asp:Label>
                                    <br /><br />
                                    <asp:GridView ID="grdvDupDetails" runat="server" Visible="false" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="true"
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                    AllowPaging="true">
                                    <PagerStyle CssClass="customGridpaging"  />                                     
                                    </asp:GridView>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                     <asp:Label ID="lblNote1" runat="server" Text="Note: Forced Reconciliation will Reconcile all the above records with same Claim id from source Anthem DF"  ForeColor="red" Font-Bold="true" Visible = "false"></asp:Label>                                    
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <div id = "subIntroText">Selected Claim Id Record Details</div>
                                    <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                   <div style="float:left;width:350px;margin-top:20px;" >
                                        <asp:DetailsView ID="dtvDFnoRF" runat="server" Height="50px" Width="320px" 
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
                        <div id = "introText">DF/RF Aging Report & Adjustment
                            <asp:Label ID="lblH2" runat="server" Text=""></asp:Label></div>
                            <asp:MultiView ID="MultiView3" runat="server" ActiveViewIndex="0">
                                <asp:View ID="viewDFRF" runat="server"> 
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:GridView ID="grdvDFRF" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="false"
                                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                                        AllowSorting="true" AutoGenerateSelectButton="true"
                                        OnSorting="grdvDFRF_Sorting" DataKeyNames="DCN"
                                        OnPageIndexChanging="grdvDFRF_PageIndexChanging" 
                                        OnRowCommand="grdvDFRF_OnRowCommand">
                                        <PagerStyle CssClass="customGridpaging" /> 
                                        <Columns>                                            
                                            <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                            <asp:BoundField DataField="MemberID" HeaderText="MemberID" SortExpression="MemberID" />
                                            <asp:BoundField DataField="DCN" HeaderText="DCN" SortExpression="DCN" />
                                            <asp:BoundField DataField="LastUpdate" HeaderText="LastUpdate" SortExpression="LastUpdate" />
                                            <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO" dataformatstring = "{0:N}"  />
                                            <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO" dataformatstring = "{0:N}"  />
                                            <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO" dataformatstring = "{0:N}"  />
                                        </Columns>          
                                        </asp:GridView>
                                     </div> 
                                </asp:View>
                                <asp:View ID="viewDFRFDetails" runat="server">
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:LinkButton ID="lnkDFRF" runat="server" OnClick="lnkDFRF_OnClick">Back to report</asp:LinkButton>
                                    </div>
                                    <div style="float:left;width:680px;margin-top:20px;" >
                                    <asp:Label ID="lblDupDetDFRF" 
                                    style="border-bottom: 1px dotted #DEDEDE;	
	                                        height:18px;color: #40353D;
	                                        font-family: Georgia, Verdana, Arial, sans-serif; 
	                                        font-size:14px;	" runat="server" Text="All Records with same DCN Number"  Visible = "false"></asp:Label>
                                    
                                    <asp:GridView ID="grdvDupDetailsDFRF" runat="server" Visible="false" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="true"
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                    AllowPaging="true">
                                    <PagerStyle CssClass="customGridpaging"  />                                          
                                    </asp:GridView>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                     <asp:Label ID="lblNote2" runat="server" Text="Note: Forced Reconciliation will Reconcile all the above records with same Claim id from source DFnoRF"  ForeColor="red" Font-Bold="true" Visible = "false"></asp:Label>                                    
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                    <div id = "subIntroText">Selected Claim Id Record Details</div>
                                    <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr2" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </div>
                                   <div style="float:left;width:350px;margin-top:20px;" >
                                        <asp:DetailsView ID="dtvDFRF" runat="server" Height="50px" Width="320px" 
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
                                                    <asp:Button ID="btnOK2" ValidationGroup="dfrfValid" runat="server" Text="Submit" OnClick="btnOK2_Click" />
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
                        <div id = "introText">DFRF Amount Mismatch Aging Report & Adjustment
                            <asp:Label ID="lblH3" runat="server" Text=""></asp:Label></div>
                            <asp:MultiView ID="MultiView4" runat="server" ActiveViewIndex="0">
                                <asp:View ID="viewDFRFMM" runat="server"> 
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:GridView ID="grdvmismatchDFRF" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" AutoGenerateColumns="true"
                                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AllowPaging="true" 
                                        AllowSorting="true" AutoGenerateSelectButton="true"
                                        OnSorting="grdvmismatchDFRF_Sorting" 
                                        OnPageIndexChanging="grdvmismatchDFRF_PageIndexChanging" 
                                        OnRowCommand="grdvmismatchDFRF_OnRowCommand">
                                        <PagerStyle CssClass="customGridpaging" />                                                 
                                        </asp:GridView>
                                     </div> 
                                </asp:View>
                                <asp:View ID="viewDFRFDetailsMM" runat="server">
                                    <div style="float:left;width:650px;margin-top:20px;" >
                                        <asp:LinkButton ID="lnkDFRFMM" runat="server" OnClick="lnkDFRFMM_OnClick">Back to report</asp:LinkButton>
                                    </div>
                                    <div style="float:left;width:680px;margin-top:20px;" >
                                    <asp:Label ID="lblDupDetmismatchDFRF" 
                                    style="border-bottom: 1px dotted #DEDEDE;	
	                                        height:18px;color: #40353D;
	                                        font-family: Georgia, Verdana, Arial, sans-serif; 
	                                        font-size:14px;	" runat="server" Text="All Records with same DCN Number"  Visible = "false"></asp:Label>
                                    
                                    <asp:GridView ID="grdvDupDetailsmismatchDFRF" runat="server" Visible="false" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" 
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" GridLines="None"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  AllowPaging="true" 
                                    AutoGenerateColumns = "true">
                                    <PagerStyle CssClass="customGridpaging"  />                                          
                                    </asp:GridView>
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >
                                     <asp:Label ID="lblNote3" runat="server" Text="Note: Forced Reconciliation will Reconcile all the above records with same Claim id"  ForeColor="red" Font-Bold="true" Visible = "false"></asp:Label>                                    
                                </div>
                                <div style="float:left;width:680px;margin-top:20px;" >                                    
                                    <div style="float:left;width:680px;margin-top:20px;">
                                        <asp:Label ID="lblFErr3" runat="server" Text="" ForeColor="red"></asp:Label>
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
                                                    <asp:Button ID="btnOK3" ValidationGroup="dfrfValid" runat="server" Text="Submit" OnClick="btnOK3_Click" />
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
                     <div id = "introText">Non CA Claims Reports after Adjustments(if any)</div> 
                        <div style="float:left;margin-top:20px;padding-left:12px">
                            <table>
                                 <tr>
                                    <td>
                                         <asp:Label ID="lblErrRep" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td height="30px">
                                        <asp:LinkButton ID="lnkAdjCF1" runat="server" ForeColor="#5D478B" OnClick="lnkAdjCF1_OnClick"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30px">
                                        <asp:LinkButton ID="lnkAdjCF2" runat="server" ForeColor="#5D478B" OnClick="lnkAdjCF2_OnClick"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30px">
                                        <asp:LinkButton ID="lnkAdjCF3" runat="server" ForeColor="#5D478B" OnClick="lnkAdjCF3_OnClick"></asp:LinkButton>
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


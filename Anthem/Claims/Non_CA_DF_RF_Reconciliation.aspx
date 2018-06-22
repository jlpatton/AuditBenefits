<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Non_CA_DF_RF_Reconciliation.aspx.cs" Inherits="Anthem_Claims_Non_CA_RF_DF_Reconciliation" Title="Non CA DF/RF Claims Reconciliation" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
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
        <div id = "importbox">	  
        <fieldset>
            <legend>DF/RF Claims Reconciliation</legend>
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
                        <asp:Label ID="lbl_warning" runat="server" Font-Italic="True" Font-Names="Arial"
                            Font-Size="10pt" ForeColor="Red">Re-reconciliation deletes reconciliation data from future YRMO's if present</asp:Label><br />
                        <br />
                    </asp:View>
                    <asp:View id="view_result" runat="server">
                        <br />
                        <br />
                        <asp:Label ID="lbl_result" runat="server"></asp:Label>&nbsp;  
                    </asp:View> 
                </asp:MultiView>                      
            </fieldset> 
	    </div>
	     <div id = "resultDiv" runat="server" visible="false" style="margin-top:10px">
	     <div id= "hraIntroText">
	    <table> 
	        <tr><td style="width:550px;margin:20px;"><div style="color:#5D478B">Reconciliation Amount Mismatch Report</div></td> 
                <td style="margin:20px;"><asp:LinkButton ID="btn_genRpt" runat="server" 
                OnClientClick="this.blur();" Font-Underline="false" 
                CssClass="imgbutton"  OnClick="btn_genRpt_Click" ><span>Excel Report</span></asp:LinkButton></td>
	        </tr>
	    </table>
	    </div>
	        <div style="float:left;width:650px;margin-top:20px;" >	        
	        <asp:GridView ID="grdvResult" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  AllowPaging="true" 
                AllowSorting="true" OnPageIndexChanging="grdvResult_PageIndexChanging" OnSorting="grdvResult_Sorting">
                <PagerStyle CssClass="customGridpaging"  /> 
                <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate>                
            </asp:GridView>
            </div>            
        <div id = "hraIntroText"> 
        <table>
            <tr>
                <td style="width:550px;margin:20px;">
                    <asp:Image ID="img_CFDFnoRF" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                    <asp:LinkButton ID="LinkButton1" runat="server" Font-Underline="false" ForeColor="#5D478B" OnClick="lnkDFAging_OnClick">View DFnoRF Aging Report</asp:LinkButton>
                </td>
                <td style="margin:20px;">
                    <asp:LinkButton ID="btn_xlDFAging" runat="server" OnClick="btn_xlDFAging_Click" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>
	    <div id = "DFAgingDiv" runat="server" visible="false">
	        <div id = "subIntroText">DF no RF Aging Report</div> 
	        <div style="float:left;width:700px;margin-top:20px;" >
	        <div class="scroller" style ="width:700px; max-height:300px;" >
	            <asp:GridView ID="grdvDFAging" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" 
                AllowSorting="true" OnPageIndexChanging="grdvDFAging_PageIndexChanging" OnSorting="grdvDFAging_Sorting">
                <PagerStyle CssClass="customGridpaging"  />
                 <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate> 
                <Columns>
                    <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                    <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" />
                    <asp:BoundField DataField="Claim #" HeaderText="Claim #" SortExpression="Claim #" />
                    <asp:BoundField DataField="NAME" HeaderText="NAME" SortExpression="NAME" />
                    <asp:BoundField DataField="Service From Dt" HeaderText="Service From Dt" SortExpression="Service From Dt" />
                    <asp:BoundField DataField="Service Thru Dt" HeaderText="Service Thru Dt" SortExpression="Service Thru Dt" />
                    <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Current YRMO" />
                    <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Previous YRMO" />
                    <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Prior YRMO" />
               </Columns>            
            </asp:GridView>
            </div>
	        </div>
	    </div>	   
	    <div id = "hraIntroText"> 
        <table>
            <tr>
                <td style="width:550px;margin:20px;">
                    <asp:Image ID="img_CFDFRF" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                    <asp:LinkButton ID="LinkButton2" runat="server" Font-Underline="false" ForeColor="#5D478B" OnClick="lnkDFRFAging_OnClick">View DF/RF Aging Report</asp:LinkButton>
                </td>
                <td style="margin:20px;">
                    <asp:LinkButton ID="btn_xlDFRFAging" runat="server" OnClick="btn_xlDFRFAging_Click" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>
	    <div id = "DFRFAgingDiv" runat="server" visible="false">
	        <div id = "subIntroText">DF/RF Aging Report</div> 
	        <div style="float:left;width:700px;margin-top:20px;" >
	        <div class="scroller" style ="width:700px; max-height:300px;" >
	            <asp:GridView ID="grdvDFRFAging" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" 
                AllowSorting="true" OnPageIndexChanging="grdvDFRFAging_PageIndexChanging" OnSorting="grdvDFRFAging_Sorting">
                <PagerStyle CssClass="customGridpaging"  /> 
                 <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate> 
                <Columns>
                    <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                    <asp:BoundField DataField="MemberID" HeaderText="MemberID" SortExpression="MemberID" />
                    <asp:BoundField DataField="DCN" HeaderText="DCN" SortExpression="DCN" />
                    <asp:BoundField DataField="LastUpdate" HeaderText="LastUpdate" SortExpression="LastUpdate" />                    
                    <asp:BoundField DataField="Current YRMO" HeaderText="Current YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Current YRMO" />
                    <asp:BoundField DataField="Previous YRMO" HeaderText="Previous YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Previous YRMO" />
                    <asp:BoundField DataField="Prior YRMO" HeaderText="Prior YRMO" 
                        dataformatstring = "{0:N}"
                        SortExpression="Prior YRMO" />
               </Columns>               
            </asp:GridView>
            </div>
	        </div>
	    </div>
	     <div id = "hraIntroText"> 
        <table>
            <tr>
                <td style="width:550px;margin:20px;">
                    <asp:Image ID="img_CFDFRFmismatch" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Underline="false" ForeColor="#5D478B" OnClick="lnkDFRFmismatchAging_OnClick">View Amount Mismatch Aging Report</asp:LinkButton>
                </td>
                <td style="margin:20px;">
                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick="btn_xlDFRFmismatchAging_Click" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>
	    <div id = "DFRFmismatchAgingDiv" runat="server" visible="false">
	        <div id = "subIntroText">DFRF Amount Mismatch Aging Report</div> 
	        <div style="float:left;width:700px;margin-top:20px;" >
	        <div class="scroller" style ="width:700px; max-height:300px;" >
	            <asp:GridView ID="grdvDFRFmismatchAging" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="true" 
                AllowSorting="true" OnPageIndexChanging="grdvDFRFmismatchAging_PageIndexChanging" OnSorting="grdvDFRFmismatchAging_Sorting">
                <PagerStyle CssClass="customGridpaging"  />
                 <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate>                   
            </asp:GridView>
            </div>
	        </div>
	    </div>
	    <div id = "hraIntroText"> 
        <table>
            <tr>
                <td style="width:550px;margin:20px;">
                    <asp:Image ID="img_CFAudit" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                    <asp:LinkButton ID="lnkdetails" runat="server" Font-Underline="false" ForeColor="#5D478B" OnClick="lnkdetails_OnClick">View Audit report of matched and mismatched records</asp:LinkButton>
                </td>
                <td style="margin:20px;">
                    <asp:LinkButton ID="btn_xldetails" runat="server" OnClick="btn_xldetails_Click" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>         
	    <div id = "DetailsDiv" runat="server" visible="false">	         
             <div id = "subIntroText">Matched Records by DCN and Amount</div> 
             <div style="float:left;width:700px;margin-top:20px;" > 
             <div class="scroller" style ="width:700px; max-height:300px;" >    
                <asp:GridView ID="grdvMatched" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" 
                AllowSorting="true" OnPageIndexChanging="grdvMatched_PageIndexChanging" OnSorting="grdvMatched_Sorting" >
                <PagerStyle CssClass="customGridpaging"  />
                 <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate>
                <Columns>
                   <asp:BoundField DataField="anbt_yrmo" HeaderText="YRMO" SortExpression="anbt_yrmo" />
                   <asp:BoundField DataField="anbt_claimid" HeaderText="DCN" SortExpression="anbt_claimid" />
                   <asp:BoundField DataField="claimsAmt" HeaderText="Anthem Claims Amount" dataformatstring = "{0:N}" SortExpression="claimsAmt" />
                   <asp:BoundField DataField="rfAmt" HeaderText="DFRF Amount" dataformatstring = "{0:N}" SortExpression="rfAmt" />
                   <asp:BoundField DataField="diff" HeaderText="Variance" SortExpression="diff" />
               </Columns>
                </asp:GridView>
                </div>
            </div>
            <div id = "subIntroText">Mismatched Amount Records</div> 
             <div style="float:left;width:700px;margin-top:20px;" > 
             <div class="scroller" style ="width:700px; max-height:300px;" >    
                <asp:GridView ID="grdvmisMatched" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" 
                AllowSorting="true" OnPageIndexChanging="grdvmisMatched_PageIndexChanging" OnSorting="grdvmisMatched_Sorting" >
                <PagerStyle CssClass="customGridpaging"  />
                 <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                <EmptyDataTemplate>
                    No records.
                </EmptyDataTemplate>
                <Columns>
                   <asp:BoundField DataField="anbt_yrmo" HeaderText="YRMO" SortExpression="anbt_yrmo" />
                   <asp:BoundField DataField="anbt_claimid" HeaderText="DCN" SortExpression="anbt_claimid" />
                   <asp:BoundField DataField="claimsAmt" HeaderText="Anthem Claims Amount" dataformatstring = "{0:N}" SortExpression="claimsAmt" />
                   <asp:BoundField DataField="rfAmt" HeaderText="DFRF Amount" dataformatstring = "{0:N}" SortExpression="rfAmt" />
                   <asp:BoundField DataField="diff" HeaderText="Variance" SortExpression="diff" />
               </Columns>
                </asp:GridView>
                </div>
            </div>
            <div id = "subIntroText">DCN Records from Anthem DF not in DF/RF</div> 
            <div style="float:left;width:700px;margin-top:20px;" >
            <div class="scroller" style ="width:700px; max-height:300px;" >
                <asp:GridView ID="grdvUnmatchedAnth" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false" 
                    AllowSorting="true" OnPageIndexChanging="grdvUnmatchedAnth_PageIndexChanging" OnSorting="grdvUnmatchedAnth_Sorting">
                    <PagerStyle CssClass="customGridpaging"  />
                    <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                    <EmptyDataTemplate>
                    No records.
                    </EmptyDataTemplate>
                     <Columns>
                       <asp:BoundField DataField="anbt_yrmo" HeaderText="YRMO" SortExpression="anbt_yrmo" />
                       <asp:BoundField DataField="anbt_claimid" HeaderText="DCN" SortExpression="anbt_claimid" />
                       <asp:BoundField DataField="anbt_subid_ssn" HeaderText="Subscriber ID" SortExpression="anbt_subid_ssn" />
                       <asp:BoundField DataField="anbt_datePd" HeaderText="Paid Date" SortExpression="anbt_datePd" />
                       <asp:BoundField DataField="anbt_claimsPdAmt" HeaderText="Anthem Claims Amount" dataformatstring = "{0:N}" SortExpression="anbt_claimsPdAmt" />
                   </Columns>               
                </asp:GridView>
                </div>
            </div> 
            <div id = "subIntroText">DCN Records from DF/RF not in Anthem DF</div> 
            <div style="float:left;width:700px;margin-top:20px;" >
            <div class="scroller" style ="width:700px; max-height:300px;" >
                <asp:GridView ID="grdvUnmatchedDFRF" runat="server" CssClass="customGrid" PagerSettings-Mode="NumericFirstLast"
                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif" EmptyDataText="No Records"
                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" AutoGenerateColumns="false"  
                    AllowSorting="true" OnPageIndexChanging="grdvUnmatchedDFRF_PageIndexChanging" OnSorting="grdvUnmatchedDFRF_Sorting">
                    <PagerStyle CssClass="customGridpaging"  />
                     <emptydatarowstyle BorderColor="LightBlue"
                        forecolor="Red"/>
                    <EmptyDataTemplate>
                    No records.
                    </EmptyDataTemplate>
                    <Columns>
                       <asp:BoundField DataField="rfdf_yrmo" HeaderText="YRMO" SortExpression="rfdf_yrmo" />
                       <asp:BoundField DataField="rfdf_dcn" HeaderText="DCN" SortExpression="rfdf_dcn" />
                       <asp:BoundField DataField="rfdf_subid" HeaderText="Subscriber ID" SortExpression="rfdf_subid" />
                       <asp:BoundField DataField="rfdf_pddt" HeaderText="Paid Date" SortExpression="rfdf_pddt" />
                       <asp:BoundField DataField="rfdf_amt" HeaderText="DFRF Amount" dataformatstring = "{0:N}" SortExpression="rfdf_amt" />
                   </Columns>
                </asp:GridView> 
                </div>
            </div>
	    </div>             
	    </div>	    	                
    </div>  
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>  
</asp:Content>


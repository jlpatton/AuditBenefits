<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="California_Claims_Reconciliation.aspx.cs" Inherits="Anthem_Claims_California_Claims_Reconciliation" Title="California Claims Reconciliation" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>


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
	    <div id = "importbox">	  
        <fieldset>
            <legend>California Claims Reconciliation</legend>
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
                    Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
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
                 <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label><br />     
                 <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View id="view_main" runat="server">
                        <br />                          
                        <asp:Button ID="btn_reconcile" runat="server" Text="Reconcile" OnClick="btn_reconcile_Click" style="margin-left:50px;"  /><br />
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
                        <br />
                        <br />                                    
                    </asp:View> 
                </asp:MultiView>                     
            </fieldset>              
	    </div>
	    <div id = "resultDiv" runat="server" visible="false" >
            <div id = "CFDiv" runat="server" visible="false">
            <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">
                         <asp:Image ID="img_CF" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                <asp:LinkButton ID="lnkCF" runat="server" OnClick="lnkCF_OnClick" Font-Underline="false" ForeColor="#5D478B">View Carry Forward Report with Aging</asp:LinkButton>
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genCFRpt" runat="server" OnClick="lnk_genCFRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div> 
            <div id = "CFRptDiv" runat="server" visible="false">
                <div id="hraSubIntroText">Mismatched Amount Carry Forwards with Aging</div> 
                <div style="float:left;width:650px;margin-top:20px;" >
                    <div class="scroller" style ="width:700px; max-height:250px;" id = "mismatchCF" runat="server">
                        <asp:GridView ID="grdvCF_match" runat="server" CssClass="customGrid" AutoGenerateColumns="true" 
                        AllowSorting="true" OnSorting="grdvCF_match_Sorting" > 
                        <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                                     
                        </asp:GridView>
                    </div>
                </div> 
                <div id="hraSubIntroText">Un-Matched Carry Forwards from Anthem with Aging</div>
                <div style="float:left;width:650px;margin-top:20px;"  >
                    <div class="scroller" style ="width:700px; max-height:250px;" id = "unmatchAnthCF" runat="server">
                        <asp:GridView ID="grdvCF_unmatchAnth" runat="server" CssClass="customGrid" AutoGenerateColumns="true" 
                        AllowSorting="true" OnSorting="grdvCF_unmatchAnth_Sorting" > 
                        <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                                      
                        </asp:GridView>
                    </div>
                </div> 
                <div id="hraSubIntroText">Un-Matched Carry Forwards from BOA with Aging</div>
                <div style="float:left;width:650px;margin-top:20px;"  >
                    <div class="scroller" style ="width:700px; max-height:250px;" id = "unmatchBOACF" runat="server">
                        <asp:GridView ID="grdvCF_unmatchBOA" runat="server" CssClass="customGrid" AutoGenerateColumns="true" 
                        AllowSorting="true" OnSorting="grdvCF_unmatchBOA_Sorting" > 
                        <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                                      
                        </asp:GridView>
                    </div>
                </div> 
                <div id="hraSubIntroText">Duplicate Check Carry Forwards with Aging</div>
                <div style="float:left;width:650px;margin-top:20px;"  >
                    <div class="scroller" style ="width:700px; max-height:250px;" id = "DupCF" runat="server">
                        <asp:GridView ID="grdvCF_Dup" runat="server" CssClass="customGrid" AutoGenerateColumns="true" 
                        AllowSorting="true" OnSorting="grdvCF_Dup_Sorting" > 
                        <emptydatarowstyle  BorderColor="LightBlue"
                            forecolor="Red"/>
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                                      
                        </asp:GridView>
                    </div>
                </div>
            </div>
            </div>           
            <div id = "dtlDiv" runat="server" visible="false">
                <div id = "hraIntroText"> 
                    <table>
                        <tr>
                            <td style="width:550px;margin:20px;">
                                <asp:Image ID="img_dtl" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>      
                                <asp:LinkButton ID="lnk_Dtl" runat="server" OnClick="lnkDtl_OnClick" Font-Underline="false" ForeColor="#5D478B">View Detail Report of matched and mismatched records</asp:LinkButton>
                            </td>
                            <td style="margin:20px;">
                                <asp:LinkButton ID="lnk_genDtlRpt" runat="server" OnClick="lnk_genDtlRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id = "dtlRptDiv" runat="server" visible="false">                
                    <div id="hraSubIntroText">Detail Report of matched records by Check# and Amount</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="matchDtl" runat="server">  
                            <asp:GridView ID="grdv_dtlmat" runat="server" CssClass="customGrid" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlmat_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate>                   
                            </asp:GridView>
                        </div>
                    </div>	
                     <div id="hraSubIntroText">Detail Report of mismatched Amount records</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="mismatchAmt" runat="server">  
                            <asp:GridView ID="grdv_dtlmismat" runat="server" CssClass="customGrid" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlmismat_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate>                   
                            </asp:GridView>
                        </div>
                    </div>                
	                <div id="hraSubIntroText">Detail Report of unmatched records from Anthem</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="unmatchAnthDtl" runat="server">  
                            <asp:GridView ID="grdv_dtlunmatAnth" runat="server" CssClass="customGrid" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlunmatAnth_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate>                   
                            </asp:GridView>
                        </div>
                    </div>	                  
                    <div id="hraSubIntroText">Detail Report of unmatched records from BOA</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="unmatchBOADtl" runat="server">  
                            <asp:GridView ID ="grdv_dtlunmatBOA" runat="server" CssClass="customGrid" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlunmatBOA_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate>                   
                            </asp:GridView>
                        </div>
                    </div>	
                    <div id="hraSubIntroText">Detail Report of Duplicate Records</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="DupDtl" runat="server">  
                            <asp:GridView ID ="grdv_dtlDup" runat="server" CssClass="customGrid" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlDup_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate>                   
                            </asp:GridView>
                        </div>
                    </div>	                 
	            </div>
	        </div>        
        </div>
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


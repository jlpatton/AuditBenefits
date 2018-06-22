<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reconcile.aspx.cs" Inherits="HRA_Reconcile" Title="HRA Reconciliation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>HRA</h1>            
            <ul id="sitemap"> 
                <li><a>Operations</a>
                    <ul>
                        <li><asp:HyperLink ID="hypEligFile" runat="server" NavigateUrl="~/HRA/Operations/Create_Eligibility_File.aspx">Eligibility File</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypPGP_FTP" runat="server" NavigateUrl="~/HRA/Operations/PGP_FTP_EligFile.aspx">PGP & FTP File</asp:HyperLink></li>		                
                    </ul>
                </li>            
                <li><a>Reconciliation</a>
                    <ul id="expand1">
                        <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPutnam" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnam.aspx">Putnam</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypPutnamAdj" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnamAdj.aspx">PutnamAdj</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypWageworks" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportWageworks.aspx">Wageworks</asp:HyperLink></li>	                        
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypReconcile" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Reconciliation/Reconcile.aspx">Reconcile</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypReconcileSOFO" runat="server" NavigateUrl="~/HRA/Reconciliation/ReconcileSOFO.aspx">Reconcile SOFO</asp:HyperLink></li>   
                    </ul>
               </li>
               <li><a>Admin bill validation</a>
                    <ul>
		                <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPtnmInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_invoice.aspx">Putnam Invoice</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypWgwkInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Wageworks_invoice.aspx">Wageworks Invoice</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypPtnmPartData" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_Participant_Data.aspx">Putnam Part Data</asp:HyperLink></li>	                        
		                        <li><asp:HyperLink ID="hypAUDITR" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_HRAAUDITR.aspx">HRAAUDITR</asp:HyperLink></li>
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypAdminRecon" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Reconcile_Admin_Invoice.aspx">Reconcile Admin Invoice</asp:HyperLink></li>
		            </ul>
                </li>  
               <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/HRA/LettersGenerator/Home.aspx">Letters</asp:HyperLink></li>               
               <li><a>Maintenance</a>
                    <ul>
                        <li><asp:HyperLink ID="hypCurMod" runat="server"  NavigateUrl="~/HRA/Maintenance/Maintainence_Module.aspx">Current Module</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/HRA/Maintenance/Pilot_Search.aspx">Search Pilot</asp:HyperLink></li>                        
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 		
    </div>
    <div id="contentright">        
       <div id = "importbox">	  
        <fieldset>
            <legend>HRA Reconciliation</legend>
                <div style="margin-top:20px;padding-left:9px">
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" style="display:block; margin-bottom:5px;" ></asp:Label><br />                    
                    <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO :" style="margin-top:5px;"></asp:Label>
                    <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                    </asp:DropDownList>
                    <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" Width="100px" OnTextChanged="txtPrevYRMO_textChanged"></asp:TextBox>
                    <asp:Label ID="lbl_yrmofrmt" runat="server" ForeColor="#7D7D7D" Text="(yyyymm)" Visible="false" style="margin-right:5px;"></asp:Label>
                    <asp:Button ID="btnCancelYRMO" runat="server" CausesValidation="false"  Text="Cancel" Visible="false" OnClick="btn_CancelYRMO"/> 
                    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        Display="Dynamic" 
                        Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">                        
                        Enter YRMO
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txtPrevYRMO"
                        ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                        Display="Dynamic"
                        Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                        Enter YRMO in format 'yyyymm'
                    </asp:RegularExpressionValidator>   
                </div>
                <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View id="view_main" runat="server">
                        <asp:Button ID="btn_reconcile" runat="server" Text="Reconcile" OnClick="btn_reconcile_Click" style="margin-top:20px; margin-bottom:30px; margin-left:50px; display:block;"/>
                    </asp:View>
                    <asp:View id="view_reconAgn" runat="server">                        
                        <asp:Label ID="lbl_reconAgn" runat="server" style="margin-top:20px; padding-left:8px; display:block;"></asp:Label>                      
                        <asp:Button ID="btn_results" runat="server" Text="View Results" OnClick="btn_results_Click" style="margin-top:10px;margin-left:7px;margin-bottom:30px;"/>                  
                        <asp:Button ID="btn_reconAgn" runat="server" Text="Re-Reconcile" OnClick="btn_reconAgn_Click" style="margin-top:10px;margin-left:7px; margin-bottom:30px;"/>
                        <asp:Label ID="lbl_note" runat="server" Text="Note: <i>Clicking 'Re-Reconcile' deletes reconciliation data for the selected Year-Month</i>" style="margin-bottom:15px; display:block;"></asp:Label>
                    </asp:View>
                    <asp:View id="view_result" runat="server">
                        <asp:Label ID="lbl_result" runat="server" style="margin-top:20px; margin-bottom:30px; display:block; padding-left:8px; color:#5D478B;"></asp:Label>
                    </asp:View> 
                </asp:MultiView>                     
            </fieldset>              
	    </div>
        <div id = "resultDiv" runat="server" visible="false">
            <div id="introText" style="margin-top:50px; width:640px; color:#5D478B; margin-bottom:-10px;">HRA Reconciliation Reports:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_recon" runat="server" OnClick="lnk_genReconRpt_OnClick" Text="Reconciliation Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_reconNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lbl_CF" runat="server" OnClick="lnk_genCFRpt_OnClick" Text="Carry Forward Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_CFNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lbl_CFnotCleared" runat="server" OnClick="lnk_genCFnotClearRpt_OnClick" Text="Carry Forward Not Cleared Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_CFnotClearedNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lbl_tran" runat="server" OnClick="lnk_genTranRpt_OnClick" Text="Transaction Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="lbl_tranNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                </table>  
            </div>
        </div>
	</div>
</asp:Content>


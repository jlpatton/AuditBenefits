<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reconcile_Admin_Invoice.aspx.cs" Inherits="HRA_Administrative_Bill_Validation_Reconcile_Admin_Invoice" Title="HRA Admin Bill Reconciliation" %>

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
                    <ul>
                        <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPutnam" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnam.aspx">Putnam</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypPutnamAdj" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnamAdj.aspx">PutnamAdj</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypWageworks" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportWageworks.aspx">Wageworks</asp:HyperLink></li>	                        
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypReconcile" runat="server" NavigateUrl="~/HRA/Reconciliation/Reconcile.aspx">Reconcile</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypReconcileSOFO" runat="server" NavigateUrl="~/HRA/Reconciliation/ReconcileSOFO.aspx">Reconcile SOFO</asp:HyperLink></li>   
                    </ul>
               </li>
               <li><a>Admin bill validation</a>
                    <ul id="expand1">
		                <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPtnmInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_invoice.aspx">Putnam Invoice</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypWgwkInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Wageworks_invoice.aspx">Wageworks Invoice</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypPtnmPartData" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_Participant_Data.aspx">Putnam Part Data</asp:HyperLink></li>	                        
		                        <li><asp:HyperLink ID="hypAUDITR" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_HRAAUDITR.aspx">HRAAUDITR</asp:HyperLink></li>
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypAdminRecon" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Administrative Bill Validation/Reconcile_Admin_Invoice.aspx">Reconcile Admin Invoice</asp:HyperLink></li>
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
            <legend>Admin Invoice Reconciliation</legend>
            <div style="margin-top:20px;padding-left:9px">
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" style="display:block; margin-bottom:5px;" ></asp:Label><br />
                <asp:Label ID="lblPrevQY" runat="server" Text="Quarter-Year:" style="margin-top:5px;"></asp:Label>
                <asp:DropDownList ID="ddlQY" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlQY_selectedIndexchanged">                        
                </asp:DropDownList>
                <asp:TextBox ID="txtPrevQY" runat="server" Visible="false" Width="120px" OnTextChanged="txtPrevQY_textChanged"></asp:TextBox>
                <asp:Label ID="lbl_QYfrmt" runat="server" ForeColor="#7D7D7D" Visible="false"
                Text="(Qd-yyyy)" style="margin-right:5px;"></asp:Label>
                <asp:Button ID="btnCancelQY" runat="server" CausesValidation="false"  Text="Cancel" Visible="false" OnClick="btn_CancelQY"/>                        
                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                ControlToValidate="txtPrevQY"
                Display="Dynamic" 
                Font-Names="Verdana" Font-Size="10pt" ValidationGroup="QYGroup"
                >
                (Required)
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                ControlToValidate="txtPrevQY"
                ValidationExpression="^Q[1234]-\d{4}$"
                Display="Dynamic" 
                Font-Names="verdana" Font-Size="10pt" ValidationGroup="QYGroup">
                Enter in format 'Qd-yyyy'
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
                    <asp:Label ID="lbl_note" runat="server" Text="Note: <i>Clicking 'Re-Reconcile' deletes reconciliation data for the selected Quarter-Year</i>" style="margin-bottom:15px; display:block;"></asp:Label>
                </asp:View>
                <asp:View id="view_result" runat="server">
                    <asp:Label ID="lbl_result" runat="server" style="margin-top:20px; margin-bottom:30px; display:block; padding-left:8px; color:#5D478B;"></asp:Label>
                </asp:View> 
            </asp:MultiView>                     
        </fieldset>    
        </div>
        <div id = "resultDiv" runat="server" visible="false">
            <div id="introText" style="margin-top:50px; width:640px; color:#5D478B; margin-bottom:-10px;">HRA Administrative Bill Validation Reports:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_genReconRpt" runat="server" OnClick="lnk_genReconRpt_OnClick" Text="Admin Invoice Reconciliation Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_genDispAP_Rpt" runat="server" OnClick="lnk_genDispAP_Rpt_OnClick" Text="HRAAUDITR to Putnam Discrepancy Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_APNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_genDispPA_Rpt" runat="server" OnClick="lnk_genDispPA_Rpt_OnClick" Text="Putnam to HRAAUDITR Discrepancy Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_PANR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr> 
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_genDispWP_Rpt" runat="server" OnClick="lnk_genDispWP_Rpt_OnClick" Text="Wageworks to Putnam Discrepancy Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_WPNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_genDispPW_Rpt" runat="server" OnClick="lnk_genDispPW_Rpt_OnClick" Text="Putnam to Wageworks Discrepancy Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_PWNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_genWPnoBal_Rpt" runat="server" OnClick="lnk_genWPnoBal_Rpt_OnClick" Text="Report of participant(s) in Wageworks with paid out in Putnam" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_WPnoBalNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr> 
                     <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_genAUDITR_Rpt" runat="server" OnClick="lnk_genAUDITR_Rpt_OnClick" Text="HRAAUDITR Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_AUDITRNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_genPtnmPartData_Rpt" runat="server" OnClick="lnk_genPtnmPartData_Rpt_OnClick" Text="Putnam Participant Data Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_PtnmPartDataNR" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>                   
                </table>  
            </div>
        </div>
	</div> 
</asp:Content>


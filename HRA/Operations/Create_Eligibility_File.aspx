<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Eligibility_File.aspx.cs" Inherits="HRA_Operations_Create_Eligibility_File" Title="Create Eligibility File" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>HRA</h1>            
            <ul id="sitemap"> 
                <li><a>Operations</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypEligFile" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Operations/Create_Eligibility_File.aspx">Eligibility File</asp:HyperLink></li>		                
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
                <legend>Pilot HRA Operation</legend>
                <div style="margin-top:30px; margin-bottom:40px; padding-left:9px;">
                    <asp:Label ID="lbl_notice" runat="server" style="margin-bottom:25px; display:block;color:#5D478B;"></asp:Label>
                    <asp:Label ID="lbl_err" runat="server" style="margin-bottom:15px; color:Red; display:block;"></asp:Label>
                    <div style="display:block; padding-bottom:10px;">
                        <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server"
                            ControlToValidate="tbx_deathsXdays"
                            Display="Dynamic" 
                            Font-Names="Verdana" Font-Size="10pt" ValidationGroup="daysGrp"
                            >
                            Enter days
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server"
                            ControlToValidate="tbx_deathsXdays"
                            ValidationExpression="^\d+$"
                            Display="Dynamic" 
                            Font-Names="verdana" Font-Size="10pt" ValidationGroup="daysGrp">
                            Enter valid days
                        </asp:RegularExpressionValidator>                             
                        <asp:Label ID="Label2" runat="server" Text="Days for Death History:" ></asp:Label>
                        <asp:TextBox ID="tbx_deathsXdays" runat="server" Width="65px"></asp:TextBox> 
                    </div>
                    <asp:LinkButton ID="lnk_genRpts" runat="server" style="margin-top:5px;margin-bottom:20px;" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" OnClick="lnk_genRpts_OnClick"><span>Generate Reports</span></asp:LinkButton>
                    <br class = "br_e" />
                </div>
            </fieldset>
        </div>
	    <div id = "ErrMsgDiv" style="float:left;width:650px;margin-top:15px;margin-left:5px;">
            <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red"></asp:Label>
	    </div>
	    <div id = "resultdiv" runat="server" visible="false">
	        <div class="userPara">
                        <p>Saved / Printed file(s) that are selected to automatically save/print.</p>    
            </div> 
            <div id="introText" style="margin-top:30px; width:640px; color:#5D478B; margin-bottom:-10px;">Pilot HRA Operation Reports:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_elig" runat="server" OnClick="lnk_genEligRpt_OnClick" Text="HRA Eligibility File" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_white_text.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_elig" runat="server" Text="No-Records!" Visible="false"  style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_eligAudit" runat="server" OnClick="lnk_genEligAuditRpt_OnClick" Text="Eligibilty Audit Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_eligAudit" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_eligTermAudit" runat="server" OnClick="lnk_gen_termAuditRpt_OnClick" Text="Retirement/Death/Terminations Audit Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_eligTermAudit" runat="server" Text="No-Records!" Visible="false"  style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_addrChg" runat="server" OnClick="lnk_gen_addrChgRpt_OnClick" Text="Address Changes Audit Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_addrChg" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_statChg" runat="server" OnClick="lnk_gen_statChgRpt_OnClick" Text="Status Changes Audit Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_statChg" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>                  
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_eligBen24" runat="server" OnClick="lnk_gen_eligBen24Rpt_OnClick" Text="Beneficiary Turns 24 Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_eligBen24" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_childBenLtr" runat="server" OnClick="lnk_gen_childBenLtrRpt_OnClick" Text="Beneficiary Letter Generated > 60 days Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_childBenLtr" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="noBenRpt" style="padding-top:20px;padding-left:12px;" runat="server">         
                            <asp:LinkButton ID="lnk_eligNoBen" runat="server" OnClick="lnk_gen_eligNoBenRpt_OnClick" Text="HRA Died No Beneficiaries Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_eligNoBen" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="noBenOwnerRpt" style="padding-top:20px;padding-left:12px;" runat="server">         
                            <asp:LinkButton ID="lnk_noBenOwner" runat="server" OnClick="lnk_gen_noBenOwnerRpt_OnClick" Text="HRA Died No Owner Identified/Verified Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_noBenOwner" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td id="deathsXdaysRpt" style="padding-top:20px;padding-left:12px; " runat="server">                            
                            <asp:LinkButton ID="lnk_deathsXdays" runat="server" OnClick="lnk_gen_deathsXdaysRpt_OnClick" ValidationGroup="daysGrp" Text="Deaths ‘X’ days from today Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>                                
                            <asp:Label ID="lbl_deathXDays" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>                   
                </table>  
            </div>
        </div>
	</div>
</asp:Content>


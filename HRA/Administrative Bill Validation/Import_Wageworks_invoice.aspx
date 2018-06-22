<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_Wageworks_invoice.aspx.cs" Inherits="HRA_Administrative_Bill_Validation_Import_Wageworks_invoice" Title="Import Wageworks Invoice Report" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

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
		                    <ul id="expand2">
		                        <li><asp:HyperLink ID="hypPtnmInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_invoice.aspx">Putnam Invoice</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypWgwkInv" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Wageworks_invoice.aspx">Wageworks Invoice</asp:HyperLink></li>	
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
                <legend>Import Wageworks Invoice Report</legend>
                    <div style="margin-top:20px;padding-left:9px">
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" style="display:block; margin-bottom:5px;" ></asp:Label><br />
                        <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO :" style="margin-top:5px;"></asp:Label>
                        <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                        </asp:DropDownList>
                        <div id="DivNewYRMO" runat="server" visible="false" style="background-color:#F7F7F7; display:block; width:600px; padding-bottom:10px; padding-top:10px;margin-left:-10px;">
                            <asp:Label ID="Label1" runat="server" Text="YRMO :" style="margin-left:10px;"></asp:Label>
                            <asp:TextBox ID="txtPrevYRMO" runat="server" Width="100px" OnTextChanged="txtPrevYRMO_textChanged" ValidationGroup="yrmoGroup"></asp:TextBox>
                            <asp:Label ID="lbl_yrmofrmt" runat="server" ForeColor="#7D7D7D" Text="(yyyymm)"></asp:Label>
                            <asp:Button ID="btnCancelYrmo" runat="server" CausesValidation="false"  Text="Cancel" OnClick="btn_CancelYrmo" style="margin-left:10px; margin-right:5px;"/>
                            <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                            ControlToValidate="txtPrevYRMO"
                            ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                            Display="Dynamic"
                            Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                            Enter YRMO in format 'yyyymm'
                            </asp:RegularExpressionValidator>
                            <p style="padding-top:10px; padding-left:10px;">Enter the YRMO not listed in the drop down list.</p>
                        </div>
                    </div>
                    <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                        <asp:View id="view_main" runat="server">
                            <div id="upload_manually" style="padding-top:15px;" runat="server">
                                <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="xls"/>                             
                                <asp:Button ID="btn_manImport" runat="server" style="margin-left:50px; margin-bottom:30px;" ValidationGroup="fileGroup" Text="Import File" OnClick="btn_manImport_Click"/> 
                            </div>
                            <div id="upload_auto" style="margin-top:10px;" runat="server">
                                <asp:Label ID="lblFile" runat="server" Text="File :" style="padding-left:23px;" />
                                <asp:Label ID="lbl_clientfile" runat="server" Width="600px" style="font-size:105%;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <asp:Button ID="btn_autoImport" runat="server" style="margin-left:50px; margin-top:25px; margin-bottom:30px; display:block;" Text="Import File" OnClick="btn_autoImport_Click"/>                                                           
                            </div>
                        </asp:View>
                        <asp:View id="view_reimport" runat="server">                                                                                     
                            <asp:Label ID="lbl_reimport" runat="server" style="margin-top:20px; padding-left:8px; display:block;"></asp:Label>
                            <asp:Button ID="btn_reimport" runat="server" Text="Re-Import File" style="margin-top:10px;display:block;margin-left:7px; margin-bottom:30px;" OnClick="btn_reimport_Click" />
                            <asp:Label ID="lbl_note" runat="server" Text="Note: <i>Clicking 'Re-Import File' deletes data for the selected Year-Month</i>" style="margin-bottom:15px; display:block;"></asp:Label>
                        </asp:View>
                        <asp:View id="view_result" runat="server">
                            <asp:Label ID="lbl_result" runat="server" style="margin-top:20px; margin-bottom:30px; display:block; padding-left:8px; color:#003399;"></asp:Label>
                        </asp:View> 
                    </asp:MultiView>                      
            </fieldset>
	    </div>
	    <div id = "resultDiv" runat="server" visible="false">
            <div id="introText" style="margin-top:50px; width:640px; color:#5D478B; margin-bottom:-10px;">Non-HRA Participants Exception Report:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_logRpt" runat="server" OnClick="lnk_genLogRpt_OnClick" Text="Non-HRA Participants Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_white_text.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>                            
                        </td>  
                    </tr>
                </table>
            </div>
        </div>               
    </div>
</asp:Content>


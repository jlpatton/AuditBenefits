<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_HRAAUDITR.aspx.cs" Inherits="HRA_Administrative_Bill_Validation_Import_HRAAUDITR" Title="Import HRAAUDITR Report" %>
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
		                        <li><asp:HyperLink ID="hypWgwkInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Wageworks_invoice.aspx">Wageworks Invoice</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypPtnmPartData" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_Participant_Data.aspx">Putnam Part Data</asp:HyperLink></li>	                        
		                        <li><asp:HyperLink ID="hypAUDITR" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Administrative Bill Validation/Import_HRAAUDITR.aspx">HRAAUDITR</asp:HyperLink></li>
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
                <legend>Import HRAAUDITR Report</legend>
                    <div style="margin-top:20px;">
                        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" style="display:block; margin-bottom:5px;" ></asp:Label><br />
                        <asp:Label ID="lbl_notice" runat="server" Visible="false" style="background-image:url(../../styles/images/warning1.png); background-color:Transparent; background-position:left; padding-left:25px; padding-top:3px; background-repeat:no-repeat;margin-bottom:15px; display:block; color:#5D478B;"></asp:Label>
                        <asp:Label ID="lblPrevQY" runat="server" Text="Quarter-Year:" style="margin-top:5px;"></asp:Label>
                        <asp:DropDownList ID="ddlQY" runat="server" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="ddlQY_selectedIndexchanged">                        
                        </asp:DropDownList>
                        <div id="DivNewYRMO" runat="server" visible="false" style="background-color:#F7F7F7; display:block; width:600px; padding-bottom:10px; padding-top:10px;margin-left:-10px;">
                            <asp:Label ID="Label1" runat="server" Text="Quarter-Year:" style="margin-left:10px;"></asp:Label>
                            <asp:TextBox ID="txtPrevQY" runat="server" Width="120px" OnTextChanged="txtPrevQY_textChanged"></asp:TextBox>
                            <asp:Label ID="lbl_QYfrmt" runat="server" ForeColor="#7D7D7D" Text="(Qd-yyyy)" style="margin-right:5px;"></asp:Label>
                            <asp:Button ID="btnCancelYrmo" runat="server" CausesValidation="false"  Text="Cancel" OnClick="btn_CancelYrmo" style="margin-left:10px; margin-right:5px;"/>
                            <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                            ControlToValidate="txtPrevQY"
                            ValidationExpression="^Q[1234]-\d{4}$"
                            Display="Dynamic"
                            Font-Names="verdana" Font-Size="10pt" ValidationGroup="QYGroup">
                            Enter in format 'Qd-yyyy'
                            </asp:RegularExpressionValidator>
                            <p style="padding-top:10px; padding-left:10px;">Enter Quarter-Year not listed in the drop down list.</p>
                        </div> 
                    </div>
                    <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                        <asp:View id="view_main" runat="server">
                            <div id="upload_manually" style="padding-top:15px;padding-left:25px;" runat="server">
                                <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="xls"/>                             
                                <asp:Button ID="btn_manImport" runat="server" style="margin-left:50px; margin-bottom:30px;" ValidationGroup="fileGroup" Text="Import File" OnClick="btn_manImport_Click"/> 
                            </div>
                        </asp:View>
                        <asp:View id="view_result" runat="server">
                            <asp:Label ID="lbl_result" runat="server" style="margin-top:20px; margin-bottom:40px; display:block; padding-left:8px; color:#003399;"></asp:Label>
                        </asp:View>  
                    </asp:MultiView>                      
            </fieldset>
	    </div>              
    </div>
</asp:Content>


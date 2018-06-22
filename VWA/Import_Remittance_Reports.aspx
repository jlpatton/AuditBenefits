<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_Remittance_Reports.aspx.cs" Inherits="VWA_Import_Remittance_Reports" Title="Import Remittance Information" %>
<%@ Register TagPrefix="ucl" TagName="FileUpload2" Src="~/UserControls/FileUploadControl2.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "module_menu">
            <h1>VWA</h1>            
            <ul id="sitemap">
                <li><a>Import</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypBank" runat="server" NavigateUrl="~/VWA/Import_Bank_Statement.aspx">Bank Statement</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypRem" runat="server" Font-Underline="true" NavigateUrl="~/VWA/Import_Remittance_Reports.aspx">Remittance Info</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypVWA" runat="server" NavigateUrl="~/VWA/Import_VWA_File.aspx">VWA File</asp:HyperLink></li>		                	                
                    </ul>
                </li> 
                <li><asp:HyperLink ID="hypTran" runat="server" NavigateUrl="~/VWA/Create_VWA_Tran_Activity.aspx">Transaction Reports</asp:HyperLink></li>                
                <li><asp:HyperLink ID="hypVWABalance" runat="server" NavigateUrl="~/VWA/VWA_Balance.aspx">Balance</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWAAdj" runat="server" NavigateUrl="~/VWA/VWA_Adjustments.aspx">Adjustments</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWACases" runat="server" NavigateUrl="~/VWA/VWA_Cases_Search.aspx">Individual Cases</asp:HyperLink></li>
                <li><a>Maintainence</a>
                    <ul>
                        <li><asp:HyperLink ID="hypMainRpts" runat="server" NavigateUrl="~/VWA/Maintainence/Maintainence_Reports.aspx">Reports</asp:HyperLink></li>                    
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 	
    </div>
   <div id="contentright">       
	     <div id = "importbox">	     
            <fieldset>
                <legend>Import Remittance Information</legend>         
                   <div style="margin-top:20px;padding-left:9px">
                        <div id = "ErrMsgDiv1" runat="server" visible="false" class="error" style="display:block; margin-bottom:5px;">
                            <asp:Label ID="lbl_error1" runat="server" Text=""></asp:Label>
	                    </div>
	                    <br class = "br_e" />     
                        <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO :" AssociatedControlID="ddlYrmo" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:5px 20px 0px 0px;text-align:left;"></asp:Label>
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
                            <div id="upload_manually" style="padding-top:15px; padding-left:9px;margin-bottom:10px;" runat="server">
                                <ucl:FileUpload2 ID="FileUpload1_OC" runat="server" Vgroup="fileGroup" FileTitle="Old Cases:" FileTypeRange="xls" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>  
                                <asp:Label ID="lbl_suc_OC" runat="server" Text="Imported" Visible="false" style="background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                                <br class = "br_e" />                           
                                <ucl:FileUpload2 ID="FileUpload1_Cigna" runat="server" Vgroup="fileGroup" FileTitle="Cigna:" FileTypeRange="xls" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>                             
                                <asp:Label ID="lbl_suc_Cigna" runat="server" Text="Imported" Visible="false" style="background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                                <br class = "br_e" /> 
                                <ucl:FileUpload2 ID="FileUpload1_UHC" runat="server" Vgroup="fileGroup" FileTitle="UHC:" FileTypeRange="xls" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>                             
                                <asp:Label ID="lbl_suc_UHC" runat="server" Text="Imported" Visible="false" style="background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                                <br class = "br_e" /> 
                                <ucl:FileUpload2 ID="FileUpload1_Disab" runat="server" Vgroup="fileGroup" FileTitle="Disability:" FileTypeRange="xls" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>                             
                                <asp:Label ID="lbl_suc_Disab" runat="server" Text="Imported" Visible="false" style="background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                                <br class = "br_e" /> 
                                <ucl:FileUpload2 ID="FileUpload1_Anth" runat="server" Vgroup="fileGroup" FileTitle="Anthem:" FileTypeRange="xls" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>                             
                                <asp:Label ID="lbl_suc_Anth" runat="server" Text="Imported" Visible="false" style="background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                                <br class = "br_e" />
                                <asp:Label ID="lbl_manWiredt" runat="server" Text="Wire Date:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="tbx_manWiredt"/>
                                <asp:TextBox ID="tbx_manWiredt" runat="server" style="width:120px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" style="margin:15px 0px 0px 0px;padding:1px 2px 0px 5px;float:left;" ForeColor="#7D7D7D" Text="(mm/dd/yyyy)"></asp:Label>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"
                                ControlToValidate="tbx_manWiredt"
                                ValidationExpression="^((0[1-9])|(1[0-2]))\/((0[1-9])|([12][0-9])|([3][01]))\/(\d{4})$"
                                Display="Dynamic"
                                Font-Names="verdana" Font-Size="10pt" ValidationGroup="fileGroup">
                                Enter valid date in format mm/dd/yyyy
                                </asp:RegularExpressionValidator>
                                <br class = "br_e" /> 
                                <asp:LinkButton ID="btn_manImport" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" style="margin-left:120px; margin-top:20px; margin-bottom:22px;" ValidationGroup="fileGroup" OnClick="btn_manImport_Click"><span>Import Data</span></asp:LinkButton> 
                                <br class = "br_e" /> 
                                <asp:CheckBox ID="cbx_autoImport" runat="server" style="margin-top:7px; float:left;display:block;" OnCheckedChanged="cbx_autoImport_OnCheckedChanged" AutoPostBack="true" />
                                <asp:Label ID="lbl_checkAutoImport" runat="server" Text="Check to Import Automatically" style="color:#5D478B; margin-top:10px;float:left;"></asp:Label>
                                <br class = "br_e" />                                
                            </div>
                            <div id="upload_auto" style="margin-top:10px;padding-left:9px;margin-bottom:10px;" runat="server">
                                <asp:Label ID="lblFile_OC" runat="server" Text="Old Cases:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="lbl_clientfile_OC"/>
                                <asp:Label ID="lbl_clientfile_OC" runat="server" style="font-size:105%; width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <br class = "br_e" /> 
                                <asp:Label ID="lblFile_Cigna" runat="server" Text="Cigna:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="lbl_clientfile_Cigna" />
                                <asp:Label ID="lbl_clientfile_Cigna" runat="server" style="font-size:105%; width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <br class = "br_e" /> 
                                <asp:Label ID="lblFile_UHC" runat="server" Text="UHC:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="lbl_clientfile_UHC" />
                                <asp:Label ID="lbl_clientfile_UHC" runat="server" style="font-size:105%; width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <br class = "br_e" /> 
                                <asp:Label ID="lblFile_Disab" runat="server" Text="Disability:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="lbl_clientfile_Disab" />
                                <asp:Label ID="lbl_clientfile_Disab" runat="server" style="font-size:105%; width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <br class = "br_e" /> 
                                <asp:Label ID="lblFile_Anth" runat="server" Text="Anthem:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="lbl_clientfile_Anth" />
                                <asp:Label ID="lbl_clientfile_Anth" runat="server" style="font-size:105%; width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                <br class = "br_e" />
                                <asp:Label ID="lbl_autoWiredt" runat="server" Text="Wire Date:" style="float:left;width:100px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;" AssociatedControlID="tbx_autoWiredt"/>
                                <asp:TextBox ID="tbx_autoWiredt" runat="server" style="width:120px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" ></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" style="margin:15px 0px 0px 0px;padding:1px 2px 0px 5px;float:left;" ForeColor="#7D7D7D" Text="(mm/dd/yyyy)"></asp:Label>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server"
                                ControlToValidate="tbx_autoWiredt"
                                ValidationExpression="^((0[1-9])|(1[0-2]))\/((0[1-9])|([12][0-9])|([3][01]))\/(\d{4})$"
                                Display="Dynamic"
                                Font-Names="verdana" Font-Size="10pt" ValidationGroup="fileGroup">
                                Enter valid date in format mm/dd/yyyy
                                </asp:RegularExpressionValidator>
                                <br class = "br_e" />  
                                <asp:LinkButton ID="btn_autoImport" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" style="margin-left:120px; margin-top:20px; margin-bottom:22px;" ValidationGroup="fileGroup" OnClick="btn_autoImport_Click"><span>Import Data</span></asp:LinkButton>                                                           
                                <br class = "br_e" /> 
                                <asp:CheckBox ID="cbx_manImport" runat="server" style="margin-top:7px; float:left;display:block;" OnCheckedChanged="cbx_manImport_OnCheckedChanged" AutoPostBack="true" />
                                <asp:Label ID="lbl_checkManImport" runat="server" Text="Check to Import Manually" style="color:#5D478B; margin-top:10px;float:left;"></asp:Label>
                                <br class = "br_e" /> 
                            </div>                            
                        </asp:View>
                </asp:MultiView>                                    
            </fieldset>
	    </div>
	    <div id = "ErrMsgDiv2" class="error" visible="false"  runat="server" style="float:left;width:650px;margin-top:15px;margin-left:5px;">
            <asp:Label ID="lbl_error2" runat="server" Text=""></asp:Label>
	    </div>
	    <div id = "resultdiv" runat="server" visible="false">
	        <div class="userPara">
                        <p>Saved / Printed file(s) that are selected to automatically save/print.</p>    
            </div> 
            <div id="introText" style="margin-top:30px; width:640px; color:#5D478B; margin-bottom:-10px;">VWA Remittance Reports:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_discp" runat="server" OnClick="lnk_genDiscpRpt_OnClick" Text="VWA Remittance Discrepancy Report" ForeColor="#5D478B" style="background-image:url(../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_discp" runat="server" Text="No-Records!" Visible="false"  style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_input" runat="server" OnClick="lnk_genInputRpt_OnClick" Text="VWA Remittance Summary & Detail Report" ForeColor="#5D478B" style="background-image:url(../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_input" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>                               
                </table>  
            </div>
        </div>              
    </div>    
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_VWA_File.aspx.cs" Inherits="VWA_Import_Bank_Statement" Title="Import VWA Monthly Transaction File" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "module_menu">
            <h1>VWA</h1>            
            <ul id="sitemap">
                <li><a>Import</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypBank" runat="server" NavigateUrl="~/VWA/Import_Bank_Statement.aspx">Bank Statement</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypRem" runat="server" NavigateUrl="~/VWA/Import_Remittance_Reports.aspx">Remittance Info</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypVWA" runat="server" Font-Underline="true" NavigateUrl="~/VWA/Import_VWA_File.aspx">VWA File</asp:HyperLink></li>		                	                
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
                <legend>Import VWA monthly File</legend>         
                   <div style="margin-top:20px;padding-left:9px">
                        <asp:Label ID="lbl_error" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;display:block; margin-bottom:5px;"></asp:Label><br /> 
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
                            <div id="upload_manually" style="padding-top:15px;margin-bottom:10px;" runat="server">
                                <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="txt"/> 
                                <asp:LinkButton ID="btn_manImport" runat="server" style="margin-left:50px; margin-bottom:30px;" ValidationGroup="fileGroup" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" OnClick="btn_manImport_Click"><span>Import File</span></asp:LinkButton>
                                <br class = "br_e" />   
                                <asp:CheckBox ID="cbx_autoImport" runat="server" style="margin-top:5px; float:left;display:block;" OnCheckedChanged="cbx_autoImport_OnCheckedChanged" AutoPostBack="true" />
                                <asp:Label ID="lbl_checkAutoImport" runat="server" Text="Check to Import Automatically" style="color:#5D478B; margin-top:10px;float:left;"></asp:Label>
                                <br class = "br_e" />   
                            </div>
                            <div id="upload_auto" style="margin-top:10px;margin-bottom:10px;" runat="server">
                                <asp:Label ID="lblFile" runat="server" Text="File :" style="padding-left:23px;" />
                                <asp:Label ID="lbl_clientfile" runat="server" Width="600px" style="font-size:105%;" ForeColor="#000099" Font-Size="12px"></asp:Label>                                                       
                                <asp:LinkButton ID="btn_autoImport" runat="server" style="margin-left:50px; margin-top:25px; margin-bottom:30px; display:block;" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" OnClick="btn_autoImport_Click"><span>Import File</span></asp:LinkButton>
                                <br class = "br_e" /> 
                                <asp:CheckBox ID="cbx_manImport" runat="server" style="margin-top:5px; float:left;display:block;" OnCheckedChanged="cbx_manImport_OnCheckedChanged" AutoPostBack="true" />
                                <asp:Label ID="lbl_checkManImport" runat="server" Text="Check to Import Manually" style="color:#5D478B; margin-top:10px;float:left;"></asp:Label>
                                <br class = "br_e" /> 
                            </div>
                        </asp:View>
                        <asp:View id="view_reimport" runat="server"> 
                            <asp:Label ID="lbl_reimport" runat="server"   style="float:left;background-image:url(../styles/images/success.png); background-color:Transparent; padding-left:20px;margin-top:10px;background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>                                                                                                              
                             <br class = "br_e" />
                            <asp:LinkButton ID="lnk_reimport" runat="server" Text="Re-Import File"  OnClick="btn_reimport_Click" ToolTip="Click to re-import Bank statement" style="float:left;background-image:url(../styles/images/reload.png);margin-top:10px;background-color:Transparent; padding-left:20px; background-position:left;background-repeat:no-repeat; margin-left:5px; color:#4169E1; margin-top:20px; margin-bottom:30px;"></asp:LinkButton>                                                                                                              
                            <br class = "br_e" /> 
                        </asp:View>
                        <asp:View id="view_result" runat="server">
                            <asp:Label ID="lbl_result" runat="server" style="float:left;background-image:url(../styles/images/success.png);margin-top:20px;margin-bottom:30px;background-color:Transparent; padding-left:20px;margin-top:10px;background-position:left;background-repeat:no-repeat; margin-left:5px; color:#006633;"></asp:Label>
                            <br class = "br_e" />
                        </asp:View> 
                </asp:MultiView>                                    
            </fieldset>
	    </div>    
    </div>    
</asp:Content>


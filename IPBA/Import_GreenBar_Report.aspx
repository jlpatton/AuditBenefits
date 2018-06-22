<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Import_GreenBar_Report.aspx.cs" Inherits="IPBA_Import_GreenBar_Report" Title="Import Selfbilling Greenbar Report" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>IPBA</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Import</a>
		            <ul id="expand1">
		                <li><asp:HyperLink ID="hypGreenbarImp" runat="server" Font-Underline="true" NavigateUrl="~/IPBA/Import_GreenBar_Report.aspx">Greenbar Report</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypADPImp" runat="server" NavigateUrl="~/IPBA/Import_ADP_Cobra.aspx">ADP Cobra Report</asp:HyperLink></li>
		            </ul>
		        </li>
                <li><asp:HyperLink ID="hypManAdj" runat="server" NavigateUrl="~/IPBA/HMO_and_HTH_Adjustments.aspx">Billing Adjustments</asp:HyperLink></li>		                
                <li><asp:HyperLink ID="hypHMOReport" runat="server" NavigateUrl="~/IPBA/HTH_HMO_Billing_Report.aspx">HTH/HMO Billing Report</asp:HyperLink></li> 
                <li><asp:HyperLink ID="hypMaintenance" runat="server" NavigateUrl="~/IPBA/Maintenance.aspx">Maintenance</asp:HyperLink></li>                  
		    </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
     <div id="contentright">
         <div id = "importbox">                
            <fieldset>
                <legend>Import GreenBar Report</legend>         
                <div style="margin-top:20px;padding-left:12px">
                <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:"></asp:Label>
                <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                </asp:DropDownList>
                <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" ></asp:TextBox>
                <asp:Label ID="Label3" runat="server" ForeColor="#7D7D7D"
                Text="(yyyymm)"></asp:Label>
                <asp:Button ID="btnAddYrmo" runat="server" Text="ADD" ValidationGroup="yrmoGroup" OnClick="btn_ADDYrmo" Visible = "false"/>
                <asp:Button ID="btnCancelYrmo" runat="server"  Text="Cancel" OnClick="btn_CancelYrmo" Visible = "false" />
                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                ControlToValidate="txtPrevYRMO"
                Display="Dynamic" 
                Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup"
                >
                Please enter YRMO
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                ControlToValidate="txtPrevYRMO"
                ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                Display="Static"
                Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                Please enter YRMO in format 'yyyymm'
                </asp:RegularExpressionValidator> 
            </div>          
            <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                <asp:View id="view_main" runat="server">
                    <br />                                                    
                    <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="txt"/>                             
                    <asp:Button ID="btn_import" runat="server" style="margin-left:50px;" ValidationGroup="fileGroup" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value; javascript:showWait();" Text="Import File" OnClick="btn_import_Click" /><br />
                    <br />
                    <br />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                </asp:View> 
                <asp:View ID="view_empty" runat="server">
                <div class="userParaI">
                    <p>Please enter the YRMO not listed in the drop down list.</p>    
                    <br />
                </div>                         
                </asp:View>
                <asp:View id="view_reimport" runat="server">
                    <br />                    
                    <asp:Label ID="lbl_reimport" runat="server"></asp:Label>&nbsp;<br />
                    <br />
                    <asp:Button ID="btn_reimport" runat="server" Text="Re-Import File" OnClick="btn_reimport_Click" />
                    &nbsp;&nbsp;                            
                    <br />
                    <br />
                </asp:View>
                <asp:View id="view_result" runat="server">
                    <br />
                    <br />
                    <asp:Label ID="lbl_result" runat="server"></asp:Label>
                    <br />
                    <br />  
                </asp:View> 
            </asp:MultiView>                      
            </fieldset>
	    </div> 
	    <div style="float:left;margin:20px;width:600px;">
	        <asp:Label ID="lbl_error" runat="server" ForeColor="Red" ></asp:Label>
	    </div>                     
    </div>
</asp:Content>


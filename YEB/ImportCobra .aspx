<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportCobra .aspx.cs" Inherits="YEB_ImportCobra_" Title="YEB ImportCOBRA Page" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="contentleft">        
        <div id = "module_menu">
            <h1>YEB</h1> 		    
		    <ul id="sitemap">		               	      
		      		        <li><asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/YEB/UpdateSSN.aspx">Update SSN</asp:Hyperlink></li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/YEB/Imports.aspx">Imports</asp:Hyperlink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/YEB/Reports.aspx">Reports</asp:Hyperlink></li>

		    </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
    <div id="contentright">
       <div id="Navigation">
            <div class="menuNav">
                <ul>
	                <li><a class="menuNavone" href="#">Imports </a>
		                <table><tr><td>
		                <ul>
		                 <li><a href="ImportCobra .aspx">Import COBRA Participants</a></li>
		                   <li><a href="ImportExpatGroup.aspx">Import Expat Group</a></li>
		                   <li><a href="ImportYEBOnline.aspx">Import YEB OnlineGroup</a></li>
		                   <li><a href="ImportWWRetiree.aspx">Import WW Retiree</a></li>
		                   <li><a href="ImportLOAActives.aspx">Import LOA & Actives</a></li></ul>
		                </td></tr></table>
	                </li>
	            </ul>	                            
            </div>
	    </div>	  
        <div id = "introText">Import COBRA Participants</div>
        <div id = "importbox">	    
            <fieldset>
                <legend>Import Cobra Participants</legend>  
                    <div style="margin-top:20px;padding-left:12px">
                        <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:"></asp:Label>
                        <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYrmo_SelectedIndexChanged" >                        
                        </asp:DropDownList>
                        <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" ></asp:TextBox>
                        <asp:Label ID="Label3" runat="server" ForeColor="#7D7D7D"
                        Text="(yyyymm)"></asp:Label>
                        <asp:Button ID="btnAddYrmo" runat="server" Text="ADD" ValidationGroup="yrmoGroup"  Visible = "false"/>
                        <asp:Button ID="btnCancelYrmo" runat="server"  Text="Cancel"  Visible = "false" />
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
                        <asp:RadioButtonList ID="rbtnWireTypes" runat="server" RepeatDirection="Horizontal" CssClass="inputHra1" Width="204px" OnSelectedIndexChanged="rbtnWireTypes_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="0">Pilots(PI)</asp:ListItem>
                                <asp:ListItem Value="1">Non-Pilot(NP)</asp:ListItem>
                                </asp:RadioButtonList>
                    </div>          
                    <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                        <asp:View id="view_main" runat="server">
                            <br /> 
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" ></asp:Label><br />
                            <br />                                
                            <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="txt"/>                             
                            <asp:Button ID="btn_import" runat="server" style="margin-left:50px;" ValidationGroup="fileGroup" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value; javascript:showWait();" Text="Import File" OnClick="btn_import_Click1"  /><br />
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
                            <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" ></asp:Label><br />
                            <br /> 
                            <asp:Label ID="lbl_reimport" runat="server"></asp:Label>&nbsp;<br />
                            <br />
                            <asp:Button ID="btn_reimport" runat="server" Text="Re-Import File"  />
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
    </div>

</asp:Content>

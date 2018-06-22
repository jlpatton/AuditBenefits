<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ImportYEBOnline.aspx.cs" Inherits="YEB_ImportYEBOnline" Title="YEB ImportOnline Page" %>

<%--<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>--%>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src='<%= ResolveClientUrl("~/ClientScripts/Common.js") %>'></script> 

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
        &nbsp;<div id="Navigation">
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
        <div id = "introText">Import YEB OnlineGroup</div>    
        <div id = "importbox">	    
            <fieldset>
                <legend>Import YEB OnlineGroup</legend>
              <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">                                            
                        <asp:View id="view_main" runat="server">
                            <br /> 
                            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" ></asp:Label><br />
                            <br />                                
                            <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="xls"/>                             
                            <asp:Button ID="btn_import" runat="server" style="margin-left:50px;" ValidationGroup="fileGroup" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value;javascript:showWait();" Text="Import File" OnClick="btn_import_Click"/>&nbsp;
                            <br />
                           <%-- <asp:Button ID="Button1" runat="server" style="margin-left:50px;" ValidationGroup="fileGroup" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value; javascript:showWait();" Text="Import File" OnClick="btn_import_Click"  /><br />--%>
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
                        <table>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <span runat="server" id='myHiddenDiv' style='display:none'> 
                                                        <img runat="server" src="../styles/images/progress-wheel.gif" 
                                                        id='myAnimatedImage'/>
                                                        <b class="upload" style="color:#8CC63E;font-size:x-large;"> Importing....
                                                        </b>
                                                    </span> 
                                                </td>
                                            </tr>
                        </table>        
                
                
                </fieldset>        
                
    </div>
                            
</asp:Content>



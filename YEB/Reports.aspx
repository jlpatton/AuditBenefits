<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="YEB_Reports" Title="YEB Reports Page" %>

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
    <div id = "importbox">	  
        <fieldset>
           <legend>Generate Distribution Lists </legend>
                <div style="margin-top:20px;padding-left:9px">
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red" style="display:block; margin-bottom:5px;" ></asp:Label><br />                    
                    <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO :" style="margin-top:5px;" Visible="False"></asp:Label>
                    <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" Visible="False" >                        
                    </asp:DropDownList>
                    <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" Width="100px" ></asp:TextBox>
                    <asp:Label ID="lbl_yrmofrmt" runat="server" ForeColor="#7D7D7D" Text="(yyyymm)" Visible="false" style="margin-right:5px;"></asp:Label>
                    <asp:Button ID="btnCancelYRMO" runat="server" CausesValidation="false"  Text="Cancel" Visible="false" /> 
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
                    <br />
                    <br />
                    <br />
                    <asp:RadioButtonList ID="rbtnEmpTypes" runat="server" RepeatDirection="Horizontal" CssClass="inputHra1" Width="204px"  AutoPostBack="True" Visible="False">
                                <asp:ListItem Selected="True" Value="0">Pilots(PI)</asp:ListItem>
                                <asp:ListItem Value="1">Non-Pilot(NP)</asp:ListItem>
                    </asp:RadioButtonList></div>
                  
              <div id = "reportdiv" runat="server" visible="true">
            <div id="introText" style="margin-top:50px; width:640px; color:#5D478B; margin-bottom:-10px;">Reports :</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_HMYPS" runat="server" OnClick="lnk_HMYPS_OnClick" Text="YEB/PBB/SAR/RET (Home Mail) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_HMYPS" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="LinkButton1" runat="server"  Text="YEB/PBB/SAR/NONRET (Home Mail) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;" OnClick="LinkButton1_Click"></asp:LinkButton>
                            <asp:Label ID="Label5" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_HMS" runat="server"  OnClick="lnk_HMS_OnClick" Text="SAR (Home Mail) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_HMS" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_CMYPS" runat="server" OnClick="lnk_CMYPS_OnClick" Text="YEB/PBB/SAR (CoMail) Group I Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_CMYPS" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lnk_PDF" runat="server" OnClick="lnk_PDF_OnClick" Text="YEB/PBB/SAR (CoMail) Group II Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="Label1" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lnk_CMS" runat="server" OnClick="lnk_CMS_OnClick" Text="SAR (CoMail) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="lbl_CMS" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lnk_PRACT" runat="server" OnClick="lnk_PRACT_OnClick" Text="YEB/ACT/PR (CoMail) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="Label2" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lnk_SSNDUP" runat="server" OnClick="lnk_SSNDUP_OnClick" Text="YEB/SSN (Duplicates) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="Label3" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">                                            
                            <asp:LinkButton ID="lnk_ADDRDUP" runat="server" OnClick="lnk_ADDRDUP_OnClick" Text="YEB/(ADDR1+CITY) (Duplicates) Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton> 
                            <asp:Label ID="Label4" runat="server" Text="No-Records!"  Visible="False" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                </table>  
            </div>
        </div> 
  </fieldset></div>

</asp:Content>

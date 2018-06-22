<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_VWA_Tran_Activity.aspx.cs" Inherits="VWA_Create_VWA_Tran_Activity" Title="Create Vengroff Transaction Activity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "module_menu">
            <h1>VWA</h1>            
            <ul id="sitemap">
                <li><a>Import</a>
                    <ul>
                        <li><asp:HyperLink ID="hypBank" runat="server" NavigateUrl="~/VWA/Import_Bank_Statement.aspx">Bank Statement</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypRem" runat="server" NavigateUrl="~/VWA/Import_Remittance_Reports.aspx">Remittance Info</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypVWA" runat="server" NavigateUrl="~/VWA/Import_VWA_File.aspx">VWA File</asp:HyperLink></li>		                	                
                    </ul>
                </li> 
                <li><asp:HyperLink ID="hypTran" runat="server" Font-Underline="true" NavigateUrl="~/VWA/Create_VWA_Tran_Activity.aspx">Transaction Reports</asp:HyperLink></li>                
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
                <legend>Create Vengroff Transaction Activity</legend>
                <div style="margin-top:30px; margin-bottom:40px; padding-left:9px;">
                    <asp:Label ID="lbl_err" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;margin-bottom:15px;display:block;"></asp:Label>
                    <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO :" style="margin-top:5px; padding-left:25px;"></asp:Label>
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
                    <div style="display:block; padding-bottom:10px;">
                        <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server"
                            ControlToValidate="tbx_agedDays"
                            ValidationExpression="^\d+$"
                            Display="Dynamic" 
                            Font-Names="verdana" Font-Size="10pt" ValidationGroup="daysGrp">
                            Enter valid days
                        </asp:RegularExpressionValidator>                             
                        <asp:Label ID="Label2" runat="server" Text="Aged Days :" ></asp:Label>
                        <asp:TextBox ID="tbx_agedDays" runat="server" Width="80px" style="margin-top:10px;"></asp:TextBox> 
                        <asp:Label ID="lbl_agedDays" runat="server"></asp:Label>
                    </div>
                    <asp:LinkButton ID="lnk_genRpts" runat="server" style="margin-top:10px;margin-bottom:20px;margin-left:30px;" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" OnClick="lnk_genRpts_OnClick"><span>Generate Reports</span></asp:LinkButton>
                    <br class = "br_e" />
                </div>
            </fieldset>
        </div>        
        <asp:Label ID="lbl_error" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;width:650px;margin-top:15px;margin-left:5px;display:block;" Visible="false"></asp:Label>
	    <div id = "resultdiv" runat="server" visible="false">
	        <div class="userPara">
                        <p>Saved / Printed file(s) that are selected to automatically save/print.</p>    
            </div> 
            <div id="introText" style="margin-top:30px; width:640px; color:#5D478B; margin-bottom:-10px;">VWA Transaction Reports:</div>
            <div style="float:left;width:640px; margin-top:10px; padding-bottom:20px;">
                <table>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">
                            <asp:LinkButton ID="lnk_miss" runat="server" OnClick="lnk_genMissRpt_OnClick" Text="Missing Transactions Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_miss" runat="server" Text="No-Records!" Visible="false"  style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>  
                    </tr>
                    <tr>
                        <td style="padding-top:20px; padding-left:12px;">                                          
                            <asp:LinkButton ID="lnk_client" runat="server" OnClick="lnk_genClientRpt_OnClick" Text="Change in Client Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_client" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_grp" runat="server" OnClick="lnk_gen_grpRpt_OnClick" Text="Change in Group Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_grp" runat="server" Text="No-Records!" Visible="false"  style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_cnt" runat="server" OnClick="lnk_gen_cntRpt_OnClick" Text="Open, Closed, Pended Counts Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_cnt" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_fin" runat="server" OnClick="lnk_gen_finRpt_OnClick" Text="Financial Activity Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_fin" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>                  
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_age" runat="server" OnClick="lnk_gen_ageRpt_OnClick" Text="Aging Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_age" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top:20px;padding-left:12px;">         
                            <asp:LinkButton ID="lnk_statCtr" runat="server" OnClick="lnk_gen_statCtrRpt_OnClick" Text="Status Counter Report" ForeColor="#5D478B" style="background-image:url(../../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>
                            <asp:Label ID="lbl_statCtr" runat="server" Text="No-Records!"  Visible="false" style="margin-left:5px; color:ActiveCaption;"></asp:Label>
                        </td>
                    </tr>      
                </table>  
            </div>
        </div>  
    </div>  
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TermFiles.aspx.cs" Inherits="TermFiles" Title="Term File Storage Box #" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentleft">
        <div id="module_menu">
            <h1>
                EBA Desktop Data</h1>
            <ul id="sitemap">
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/DsktpHist/SpDependents.aspx">Tower Group</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/DsktpHist/RetPart.aspx">Retiree Health Data</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/DsktpHist/HipCert.aspx">HIPAA Certificates</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/DsktpHist/EBAworkorder.aspx">Work Orders</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/DsktpHist/TermFiles.aspx">Term Files</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/DsktpHist/SecurityLog.aspx">Security Log</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/DsktpHist/PCaudit.aspx">PC Audit Data</asp:HyperLink>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />
        </div>
    </div>
    
    <div style="float: left; width: 650px; margin: 5px;">
    </div>
    <div id="multiView">
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <div style="margin-top: 10px;">
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="grp2" runat="server"
                        DisplayMode="List" ShowMessageBox="True" />
                    <div class="info" id="infoDiv1" runat="server" visible="false">
                        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="formHra">
                        <fieldset>
                            <legend>Term File Storage Box Numbers</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEmpNo" runat="server" Text="Employee #:" CssClass="labelHra"></asp:Label>
                                        <asp:TextBox ID="txtEmpno" runat="server" CssClass="inputHra"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv4" ValidationGroup="grp2" runat="server" ErrorMessage="Employee Number is required!"
                                            ControlToValidate="txtEmpno">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 10px">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnSearch" runat="server" OnClientClick="this.blur();" Font-Underline="false"
                                            CssClass="imgbutton" ValidationGroup="grp2" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton></td>
                                </tr>
                            </table>
                            <br class="br_e" />
                            <asp:Label ID="lblBoxNo" runat="server" Text="Storage Box #:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtBoxNo" runat="server" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <br class="br_e" />
                        </fieldset>
                    </div>
                </div>
                <br class="br_e" />
                <br class="br_e" />
            </asp:View>
            
            &nbsp;
        </asp:MultiView>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="HipCert.aspx.cs" Inherits="HipCert" Title="HIPAA Certificates" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentleft">
        <div id="module_menu">
            <h1>
                EBA Desktop Data</h1>
            <ul id="sitemap">
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/DsktpHist/Tower.aspx">Tower Group</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/DsktpHist/RetPart.aspx">Retiree Health Data</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink4" runat="server">HIPAA Certificates</asp:HyperLink>
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
    <div id="contentright">
        <div class="Menu">
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Participant" Value="0" Selected="True"></asp:MenuItem>
                    <asp:MenuItem Text="Coverage" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Dependents" Value="2"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />
            </asp:Menu>
        </div>
    </div>
    <div id="introText" style="margin-left: 0px;">
        <asp:Label ID="lblEmpHeading" runat="server" Text=""></asp:Label>
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
                            <legend>PARTICPANT Information</legend>
                            <asp:Label ID="lblSSNo" runat="server" Text="SSN:" CssClass="labelHra"></asp:Label>
                            <table><tr><td><asp:TextBox ID="txtSSNo" runat="server" CssClass="inputHra"></asp:TextBox></td><td style="width:10px"></td><td><asp:LinkButton ID="btnRetrieve" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" CssClass = "imgbutton" 
                                    ValidationGroup = "v1" OnClick="btnRetrieve_Click" ><span>Get Record</span></asp:LinkButton></td></tr>
                                    <tr><td></td>
                                    <asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" 
                                    ControlToValidate="txtSSNo" ValidationGroup ="v1" ErrorMessage="Missing SSN">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator 
                                    id="RegvalSSN" 
                                    runat="server"
                                    ControlToValidate="txtSSNo"
                                    ValidationExpression="^\d{9}$"
                                    Display="dynamic" ValidationGroup="v1"
                                    Font-Names="verdana" Font-Size="10pt">
                                    Enter valid SSN in format 'xxxxxxxxx'
                                    </asp:RegularExpressionValidator><td></td></tr></table>
                            <br class = "br_e" />
                            <asp:Label ID="lblEmpno" runat="server" Text="Emp#" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtEmpno" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblSexCd" runat="server" Text="Sex:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSexCd" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblDob" runat="server" Text="DOB:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtDob" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblAddr1" runat="server" Text="Addr1:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr1" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblAddr2" runat="server" Text="Addr2:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr2" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblCity" runat="server" Text="City:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtCity" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblState" runat="server" Text="State:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtState" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblZip" runat="server" Text="ZIP:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtZip" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblQualCd" runat="server" Text="Qual Event:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtQualCd" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblQualDt" runat="server" Text="Qual Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtQualDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblLoadDt" runat="server" Text="Load Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLoadDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <%--<asp:CheckBox ID = "chkbxPrintFlag" runat="server" Text="Printed?" CssClass="checkHra"></asp:CheckBox>
                            <br class = "br_e" />--%> 
                            <asp:Label ID="lblPrintDt" runat="server" Text="Print Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPrintDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                            <asp:Label ID="lblMon18" runat="server" Text="18 Mnth Crdtbl Cvg:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtMon18Flag" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class = "br_e" />
                        </fieldset>
                        &nbsp;
                        <fieldset>
                            <legend>Name Search</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSrchLname" runat="server" Text="Last Name" CssClass="labelHra"></asp:Label>
                                        <asp:TextBox ID="txtSrchLname" runat="server" CssClass="inputHra"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv4" ValidationGroup="grp2" runat="server" ErrorMessage="Part or all of Last Name is required!"
                                            ControlToValidate="txtSrchLname">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 10px">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnSearch" runat="server" OnClientClick="this.blur();" Font-Underline="false"
                                            CssClass="imgbutton" ValidationGroup="grp2" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton></td>
                                </tr>
                            </table>
                            <asp:Label ID="lblSrchFname" runat="server" Text="First Name" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSrchFname" runat="server" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblSrchXEmpno" runat="server" Text="Emp#" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSrchEmpno" runat="server" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <br class="br_e" />
                            <asp:CompleteGridView id="GridView1" runat="server" Width="640px" AutoGenerateColumns="False"
                                AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                SortAscendingImageUrl="" SortDescendingImageUrl="">
                                <headerstyle cssclass="headerstyle"></headerstyle>
                                <selectedrowstyle backcolor="SkyBlue" font-bold="True" />
                                <alternatingrowstyle cssclass="altrowstyle"></alternatingrowstyle>
                                <columns>
                                    <asp:CommandField ShowSelectButton="True">
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:CommandField>
                                    <asp:BoundField DataField="fname" HeaderText="First Name" SortExpression="fname"></asp:BoundField>
                                    <asp:BoundField DataField="lname" HeaderText="Last Name" SortExpression="lname"></asp:BoundField>
                                    <asp:BoundField DataField="empno" HeaderText="Empno" SortExpression="empno"></asp:BoundField>
                                    <asp:BoundField DataField="ssno" HeaderText="SSN" SortExpression="ssno"></asp:BoundField>
                                    </columns>
                                <pagersettings mode="NextPreviousFirstLast" firstpageimageurl="~/styles/images/firstPage.gif"
                                    lastpageimageurl="~/styles/images/lastPage.gif" nextpageimageurl="~/styles/images/nextPage.gif"
                                    previouspageimageurl="~/styles/images/prevPage.gif" />
                                <rowstyle cssclass="rowstyle"></rowstyle>
                            </asp:CompleteGridView>
                        </fieldset>
                    </div>
                </div>
                <br class="br_e" />
                <br class="br_e" />
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Coverage</legend>
                        <asp:CompleteGridView ID="grvCoverage" runat="server" AutoGenerateColumns="False"
                            Width="641px" Height="4px" SortAscendingImageUrl="" SortDescendingImageUrl="">
                            <headerstyle cssclass="headerstyle" />
                            <columns>
                                <asp:BoundField DataField="plancd" HeaderText="Carrier" SortExpression="plancd" />
                                <asp:BoundField DataField="fromdt" HeaderText="From Dt" SortExpression="fromdt" />
                                <asp:BoundField DataField="thrudt" DataFormatString="{0:d}" HeaderText="Thru Dt"
                                    SortExpression="thrudt" />
                                <asp:BoundField DataField="nodays" HeaderText="# of Days"
                                    SortExpression="nodays" />
                            </columns>
                            <rowstyle cssclass="rowstyle" />
                        </asp:CompleteGridView>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>dependents</legend>
                        <asp:CompleteGridView ID="grvDependents" runat="server" AutoGenerateColumns="False"
                            Width="641px" Height="4px">
                            <headerstyle cssclass="headerstyle" />
                            <columns>
                        <asp:BoundField DataField="dssno" HeaderText="Dep SSN" SortExpression="dssno">
                        <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="depname" HeaderText="Dependent Name" SortExpression="depname"></asp:BoundField>
                        <asp:BoundField DataField="relcd" HeaderText="Rel Cd" SortExpression="relcd"></asp:BoundField>
                        </columns>
                            <rowstyle cssclass="rowstyle" />
                        </asp:CompleteGridView>
                    </fieldset>
                </div>
            </asp:View>
            &nbsp;
        </asp:MultiView>
    </div>
</asp:Content>

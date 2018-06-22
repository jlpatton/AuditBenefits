<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Tower.aspx.cs" Inherits="Tower" Title="Tower Employee Group" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentleft">
        <div id="module_menu">
            <h1>
                EBA Desktop Data</h1>
            <ul id="sitemap">
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="hypTowerGroup" runat="server" NavigateUrl="">Tower Group</asp:HyperLink>
                </li>
                <li>
                    <ul id="expand1">
                        <li>
                            <asp:HyperLink ID="hypTowerEmps" runat="server" Font-Underline="true">Employees</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hypSpDeps" runat="server" NavigateUrl="~/DsktpHist/SpDependents.aspx">Sponsored Dependents</asp:HyperLink></li>
                    </ul>
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
    <div id="contentright">
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
                            <legend>Employee Information</legend>
                            <asp:Label ID="lblSSNo" runat="server" Text="SSN:" CssClass="labelHra"></asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSSNo" runat="server" CssClass="inputHra"></asp:TextBox></td>
                                    <td style="width: 10px">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnRetrieve" runat="server" OnClientClick="this.blur();" Font-Underline="false"
                                            CssClass="imgbutton" ValidationGroup="v1" OnClick="btnRetrieve_Click"><span>Get Record</span></asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" ControlToValidate="txtSSNo"
                                        ValidationGroup="v1" ErrorMessage="Missing SSN">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                            ID="RegvalSSN" runat="server" ControlToValidate="txtSSNo" ValidationExpression="^\d{9}$"
                                            Display="dynamic" ValidationGroup="v1" Font-Names="verdana" Font-Size="10pt">
                                    Enter valid SSN in format 'xxxxxxxxx'
                                        </asp:RegularExpressionValidator><td>
                                        </td>
                                </tr>
                            </table>
                            <br class="br_e" />
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblAddr1" runat="server" Text="Addr1:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr1" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblAddr2" runat="server" Text="Addr2:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr2" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblCity" runat="server" Text="City:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtCity" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblState" runat="server" Text="State:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtState" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblZip" runat="server" Text="ZIP:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtZip" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPhone" runat="server" Text="Phone:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPhone" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblDfname" runat="server" Text="Dep Fname:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtDfname" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblDlname" runat="server" Text="Dep Lname:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtDlname" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblDDOB" runat="server" Text="Dep DOB:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtDDOB" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblRelCd" runat="server" Text="Relationship:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtRelCd" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblHealthInd" runat="server" Text="Health Ind:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtHealthInd" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
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
                            <br class="br_e" />
                            <asp:CompleteGridView ID="GridView1" runat="server" Width="640px" AutoGenerateColumns="False"
                                AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                                <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" />
                                <AlternatingRowStyle CssClass="altrowstyle"></AlternatingRowStyle>
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True">
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:CommandField>
                                    <asp:BoundField DataField="fname" HeaderText="First Name" SortExpression="fname"></asp:BoundField>
                                    <asp:BoundField DataField="lname" HeaderText="Last Name" SortExpression="lname"></asp:BoundField>
                                    <asp:BoundField DataField="ssno" HeaderText="SSN" SortExpression="ssno"></asp:BoundField>
                                </Columns>
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif"
                                    LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif"
                                    PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <RowStyle CssClass="rowstyle"></RowStyle>
                            </asp:CompleteGridView>
                        </fieldset>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>

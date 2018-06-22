<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RetPart.aspx.cs" Inherits="RetPart" Title="Retiree Health Processing" %>

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
                    <asp:HyperLink ID="HyperLink3" runat="server">Retiree Health Data</asp:HyperLink>
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
        <div class="Menu">
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Participant" Value="0" Selected="true"></asp:MenuItem>
                    <asp:MenuItem Text="Elections" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Payment History" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Pension Deduction History" Value="3"></asp:MenuItem>
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
                            <legend>RETIREE PARTICPANT Information</legend>
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
                            <asp:Label ID="lblEmpNo" runat="server" Text="EmpNo:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtEmpNo" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblLastName" runat="server" Text="Last Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLastName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblFirstName" runat="server" Text="First Name:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFirstName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPType" runat="server" Text="Person Type:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPType" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblEStatus" runat="server" Text="Emp Status:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtEStatus" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblDedAuthDate" runat="server" Text="Pension Dedctn Auth Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtDedAuthDate" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblFormRecDate" runat="server" Text="Election Form Received Date:"
                                CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFormRecDate" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPathCd" runat="server" Text="Path Code:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPathCd" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPathDt" runat="server" Text="Path Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPathDate" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPathUserId" runat="server" Text="Path UserID:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPathUserId" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPathUserDate" runat="server" Text="Path User Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPathUserDate" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                        </fieldset>
                        &nbsp;
                        <fieldset>
                            <legend>Retiree Name Search</legend>
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
                <%--<div id="updp" visible="true">
                            <asp:UpdatePanel id="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <contenttemplate>
                                &nbsp; &nbsp;&nbsp;
                                    <asp:CompleteGridView id="GridView12" runat="server" Width="640px"
                                    AutoGenerateColumns="False"  AllowSorting="True" AllowPaging="True"> 
                                    <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif"/>
                                    <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                                    <RowStyle CssClass="rowstyle"></RowStyle>
                                    <AlternatingRowStyle CssClass="altrowstyle"></AlternatingRowStyle>
                                    <Columns>
                                    <asp:BoundField DataField="fname" HeaderText="First Name" SortExpression="fname"></asp:BoundField>
                                    <asp:BoundField DataField="lname" HeaderText="Last Name" SortExpression="lname"></asp:BoundField>
                                    <asp:BoundField DataField="ssno" HeaderText="SSN" SortExpression="ssno"></asp:BoundField>
                                    <asp:BoundField DataField="xrefempno" HeaderText="Xref Empno" SortExpression="xrefempno"></asp:BoundField>
                                    </Columns>
                                    </asp:CompleteGridView> 
                                </contenttemplate>
                            </asp:UpdatePanel>
                            </div>--%>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Retiree Elections</legend>
                        <asp:CompleteGridView ID="grvElections" runat="server" AutoGenerateColumns="False"
                            Width="641px" Height="4px">
                            <headerstyle cssclass="headerstyle" />
                            <columns>
                                <asp:BoundField DataField="eventcd" HeaderText="EventCd" SortExpression="eventcd" />
                                <asp:BoundField DataField="eventdt" DataFormatString="{0:d}" HeaderText="EventDt"
                                    SortExpression="eventdt" />
                                <asp:BoundField DataField="etype" HeaderText="PayCd" SortExpression="etype" />
                                <asp:BoundField DataField="bencd" HeaderText="BenCd" SortExpression="bencd" />
                                <asp:BoundField DataField="plancd" HeaderText="Plan" SortExpression="plancd" />
                                <asp:BoundField DataField="tiercd" HeaderText="Tier" SortExpression="tiercd" />
                                <asp:BoundField DataField="eecost" HeaderText="Cost" SortExpression="eecost" />
                                <asp:BoundField DataField="startdt" DataFormatString="{0:d}" HeaderText="Cvg Eff"
                                    SortExpression="startdt" />
                                <asp:BoundField DataField="stopdt" DataFormatString="{0:d}" HeaderText="Cvg End"
                                    SortExpression="stopdt" />
                            </columns>
                            <rowstyle cssclass="rowstyle" />
                        </asp:CompleteGridView>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Retiree Payments</legend>
                        <asp:CompleteGridView ID="grvPayments" runat="server" AutoGenerateColumns="False"
                            Width="641px" Height="4px">
                            <headerstyle cssclass="headerstyle" />
                            <columns>
                        <asp:BoundField DataField="recdt" DataFormatString="{0:d}" HeaderText="Receive Dt" SortExpression="recdt">
                        <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="etype" HeaderText="Bill Type" SortExpression="etype"></asp:BoundField>
                        <asp:BoundField DataField="chkno" HeaderText="Check#" SortExpression="chkno"></asp:BoundField>
                        <asp:BoundField DataField="payamt" HeaderText="Amount" SortExpression="payamt"></asp:BoundField>
                        <asp:BoundField DataField="startdt" DataFormatString="{0:d}" HeaderText="Start Dt" SortExpression="startdt"></asp:BoundField>
                        <asp:BoundField DataField="enddt" DataFormatString="{0:d}" HeaderText="End Dt" SortExpression="enddt"></asp:BoundField>
                        <asp:BoundField DataField="batchno" HeaderText="Batch #" SortExpression="batchno"></asp:BoundField>
                        </columns>
                            <rowstyle cssclass="rowstyle" />
                        </asp:CompleteGridView>
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View4" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Retiree Pension Deductions</legend>
                        <asp:CompleteGridView ID="grvDeductions" runat="server" AutoGenerateColumns="False"
                            Width="641px" Height="4px">
                            <headerstyle cssclass="headerstyle" />
                            <columns>
<asp:BoundField DataField="effdt" DataFormatString="{0:d}" HeaderText="Pay Eff Dt" SortExpression="effdt">
<HeaderStyle CssClass="headerstyle"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="payamt" HeaderText="Pay Amt" SortExpression="payamt"></asp:BoundField>
<asp:CheckBoxField DataField="payrollflg" HeaderText="Sent to Payroll?" SortExpression="payrollflg">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:CheckBoxField>
<asp:BoundField DataField="gendt" DataFormatString="{0:d}" HeaderText="Date Sent" SortExpression="gendt"></asp:BoundField>
</columns>
                            <rowstyle cssclass="rowstyle" />
                        </asp:CompleteGridView>
                    </fieldset>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EBAworkorder.aspx.cs" Inherits="EBAworkorder" Title="EBA Historical Work Orders" %>

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
                <%--<li>
                    <ul id="expand1">
                        <li>
                            <asp:HyperLink ID="hypTowerEmps" runat="server" NavigateUrl="~/DsktpHist/Tower.aspx">Employees</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hypSpDeps" runat="server" Font-Underline="False" NavigateUrl="~/DsktpHist/SpDependents.aspx">Sponsored Dependents</asp:HyperLink></li>
                    </ul>
                </li>--%>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/DsktpHist/RetPart.aspx">Retiree Health Data</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/DsktpHist/HipCert.aspx">HIPAA Certificates</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink5" runat="server">Work Orders</asp:HyperLink>
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
                    <asp:MenuItem Text="WO Request Detail" Value="0" Selected="True"></asp:MenuItem>
                    <asp:MenuItem Text="Response Detail" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Status Detail" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="History" Value="3"></asp:MenuItem>
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
                            <legend>Work Order Search</legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblWOnum" runat="server" Text="Work Order Number:" CssClass="labelHra"></asp:Label>
                                        <asp:TextBox ID="txtSrchWOnum" runat="server" CssClass="inputHra"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfv4" ValidationGroup="grp2" runat="server" ErrorMessage="WO Number is required!"
                                            ControlToValidate="txtSrchWOnum">*</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td style="width: 10px">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnSearch" runat="server" OnClientClick="this.blur();" Font-Underline="false"
                                            CssClass="imgbutton" ValidationGroup="grp2" OnClick="btnSearch_Click"><span>Search</span></asp:LinkButton></td>
                                </tr>
                            </table>
                            <asp:Label ID="lblSrchType" runat="server" Text="Type:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSrchType" runat="server" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <br class="br_e" />
                            <asp:CompleteGridView ID="GridView1" runat="server" Width="640px" AutoGenerateColumns="False"
                                AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                OnPageIndexChanging="GridView1_PageIndexChanging" SortAscendingImageUrl="" SortDescendingImageUrl="">
                                <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                                <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" />
                                <AlternatingRowStyle CssClass="altrowstyle"></AlternatingRowStyle>
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True"></asp:CommandField>
                                    <asp:BoundField DataField="seqno" HeaderText="WO#" SortExpression="seqno"></asp:BoundField>
                                    <asp:BoundField DataField="woname" HeaderText="Work Order Name" SortExpression="woname">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type"></asp:BoundField>
                                    <asp:BoundField DataField="authorid" HeaderText="Author" SortExpression="authorid"></asp:BoundField>
                                    <asp:BoundField DataField="dtreq" HeaderText="Request Date" SortExpression="dtreq"></asp:BoundField>
                                </Columns>
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif"
                                    LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif"
                                    PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <RowStyle CssClass="rowstyle"></RowStyle>
                            </asp:CompleteGridView>
                        </fieldset>
                        &nbsp;
                        <fieldset>
                            <legend>Work order Information</legend>
                            <asp:Label ID="lblWOno" runat="server" Text="WO Req #:" CssClass="labelHra"></asp:Label>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtWOno" runat="server" CssClass="inputHra"></asp:TextBox></td>
                                    <td style="width: 10px">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnRetrieve" runat="server" OnClientClick="this.blur();" Font-Underline="false"
                                            CssClass="imgbutton" ValidationGroup="v1" OnClick="btnRetrieve_Click" Visible="False"><span>Get Record</span></asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <%--<asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" ControlToValidate="txtSSNo"
                                        ValidationGroup="v1" ErrorMessage="Missing SSN">* Required</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                            ID="RegvalSSN" runat="server" ControlToValidate="txtSSNo" ValidationExpression="^\d{9}$"
                                            Display="dynamic" ValidationGroup="v1" Font-Names="verdana" Font-Size="10pt">
                                    Enter valid SSN in format 'xxxxxxxxx'
                                        </asp:RegularExpressionValidator>--%>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblReqType" runat="server" Text="Request Type:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtReqType" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblReqDate" runat="server" Text="Request Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtReqDate" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblRespDueDt" runat="server" Text="Response Due Date:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtRespDueDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblAuthor" runat="server" Text="Author:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAuthor" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblTitle" runat="server" Text="Request Title:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtTitle" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblLvl" runat="server" Text="Request Level:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLvl" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblPriority" runat="server" Text="Priority:" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtPriority" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblDescription" runat="server" Text="WO Description:" CssClass="labelMultiHra"></asp:Label>
                            <asp:TextBox ID="txtDescription" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                                TextMode="MultiLine"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblJustification" runat="server" Text="Justification:" CssClass="labelMultiHra"></asp:Label>
                            <asp:TextBox ID="txtJustification" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                                TextMode="MultiLine"></asp:TextBox>
                            <br class="br_e" />
                            <asp:Label ID="lblComments" runat="server" Text="Comments:" CssClass="labelMultiHra"></asp:Label>
                            <asp:TextBox ID="txtComments" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                                TextMode="MultiLine"></asp:TextBox>
                            <br class="br_e" />
                        </fieldset>
                    </div>
                </div>
                <br class="br_e" />
                <br class="br_e" />
            </asp:View>
            <asp:View ID="View2" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Work Order Response Information</legend>
                        <asp:Label ID="lblProjName" runat="server" Text="Project Name:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtProjName" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblAnalyst" runat="server" Text="Analyst:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtAnalyst" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblAssignDt" runat="server" Text="Assigned Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtAssignDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblCompDt" runat="server" Text="Analysis Completed:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtCompDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblResults" runat="server" Text="Results:" CssClass="labelMultiHra"></asp:Label>
                        <asp:TextBox ID="txtResults" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                            TextMode="MultiLine"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblMonths" runat="server" Text="Estimated Months:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtMonths" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblHours" runat="server" Text="Estimated Hours:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtHours" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblConsiderations" runat="server" Text="Considerations:" CssClass="labelMultiHra"></asp:Label>
                        <asp:TextBox ID="txtConsiderations" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                            TextMode="MultiLine"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblRisks" runat="server" Text="Risks:" CssClass="labelMultiHra"></asp:Label>
                        <asp:TextBox ID="txtRisks" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                            TextMode="MultiLine"></asp:TextBox>
                        <br class="br_e" />
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <div id="formHra">
                    <fieldset>
                        <legend>Work Order Status Information</legend>
                        <asp:Label ID="lblStatus" runat="server" Text="Status:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtStatus" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblStatDt" runat="server" Text="Status Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtStatDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblApproval" runat="server" Text="Request Approved:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtApproval" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblApproveDt" runat="server" Text="Req Approval Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtApproveDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblStatAssignDt" runat="server" Text="Assign Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtStatAssignDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblProjLead" runat="server" Text="Project Lead:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtProjLead" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblReqDefDt" runat="server" Text="Req Def Compl Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtReqDefDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblTechSpecDt" runat="server" Text="SRS Compl Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtTechSpecDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblDevComplDt" runat="server" Text="Dev Compl Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtDevComplDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblTestComplDt" runat="server" Text="Test Compl Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtTestComplDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblProdApprovDt" runat="server" Text="Prod Approval Date:" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtProdApprovDt" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblStatComments" runat="server" Text="Comments:" CssClass="labelMultiHra"></asp:Label>
                        <asp:TextBox ID="txtStatComments" runat="server" ReadOnly="true" CssClass="inputMultiHra"
                            TextMode="MultiLine"></asp:TextBox>
                        <br class="br_e" />
                    </fieldset>
                </div>
            </asp:View>
            <asp:View ID="View4" runat="server">
                <div id="formHra">
                    <fieldset>
                    <legend>Work Order Status History Information</legend>
                    <asp:CompleteGridView ID="grvStatHist" runat="server" Width="640px" AutoGenerateColumns="False"
                                AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                OnPageIndexChanging="GridView1_PageIndexChanging">
                                <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                                <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" />
                                <AlternatingRowStyle CssClass="altrowstyle"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="descr" HeaderText="Description" SortExpression="descr">
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="userid" HeaderText="User" SortExpression="userid">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="userdt" HeaderText="UserDt" SortExpression="userdt" DataFormatString="{0:d}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usertime" HeaderText="Time" SortExpression="usertime" DataFormatString="{0:T}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif"
                                    LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif"
                                    PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <RowStyle CssClass="rowstyle"></RowStyle>
                            </asp:CompleteGridView>
                    </fieldset>
                    <br class="br_e" />
                    <br class="br_e" />
                    <fieldset>
                    <legend>Work Order User Change History</legend>
                    <asp:CompleteGridView ID="grvUserChanges" runat="server" Width="640px" AutoGenerateColumns="False"
                                AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                OnPageIndexChanging="GridView1_PageIndexChanging">
                                <HeaderStyle CssClass="headerstyle"></HeaderStyle>
                                <SelectedRowStyle BackColor="SkyBlue" Font-Bold="True" />
                                <AlternatingRowStyle CssClass="altrowstyle"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="userid" HeaderText="User" SortExpression="userid">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="userdt" HeaderText="Date" SortExpression="userdt" DataFormatString="{0:d}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="usertime" HeaderText="Time" SortExpression="usertime" DataFormatString="{0:T}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="process" HeaderText="Process" SortExpression="process">
                                        <ItemStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif"
                                    LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif"
                                    PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <RowStyle CssClass="rowstyle"></RowStyle>
                            </asp:CompleteGridView>
                    </fieldset>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>

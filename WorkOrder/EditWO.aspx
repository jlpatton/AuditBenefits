<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EditWO.aspx.cs" Title="Edit Work Order" Inherits="_Default" %>

<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>
<%--<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="word_WOnum" DefaultMode="Edit"
            Height="559px" Width="990px" CellPadding="1" OnItemUpdated="FormView1_ItemUpdated"
            OnItemUpdating="FormView1_ItemUpdating" DataSourceID="ObjectDataSource1" OnPreRender="FormView1_PreRender">
            <ItemTemplate>
                <table style="font-size: 8pt; font-family: Verdana">
                    <tr style="height: 20px" valign="bottom">
                        <td style="width: 273px; height: 15px; font-size: 8pt; font-family: Verdana" valign="bottom">
                            <span style="font-family: Verdana"><strong>Work Order:</strong></span>
                            <asp:Label ID="word_WOnumLabel1" runat="server" Text='<%# Eval("word_WOnum") %>'
                                Font-Names="Verdana" Font-Size="8pt" Width="90px" BorderStyle="Solid" BorderWidth="1px"
                                Height="19px"></asp:Label><br />
                        </td>
                        <td style="width: 105px; height: 15px" valign="bottom">
                        </td>
                        <td style="width: 240px; height: 15px" align="right" valign="bottom">
                            <strong><span style="font-family: Verdana; text-align: left">Create Date</span>:</strong>
                            <asp:Label ID="word_DateLabel" runat="server" Text='<%# Eval("word_Date", "{0:d}") %>'
                                Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px" Width="95px"
                                Height="19px"></asp:Label><br />
                        </td>
                    </tr>
                </table>
                <span style="font-family: Verdana; font-size: 8pt"><strong>Title: </strong></span>
                <asp:Label ID="word_TitleLabel" runat="server" Text='<%# Eval("word_Title") %>' Width="630px"
                    Height="22px" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid"
                    BorderWidth="1px"></asp:Label><br />
                <table>
                    <tr valign="bottom">
                        <td style="height: 20px; width: 217px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Project:</span>
                            <asp:Label ID="ProjLabel" runat="server" SelectedValue='<%# Eval("word_Proj") %>'
                                Width="155px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"
                                BorderColor="DarkGray" Text='<%# Eval("word_Proj") %>' Font-Bold="False" Height="19px"></asp:Label>
                        </td>
                        <td style="height: 20px; width: 193px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana; text-align: left">Priority:</span>
                            <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("word_Priority") %>'
                                Width="101px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"
                                BorderColor="DarkGray" Height="19px"></asp:Label>
                        </td>
                        <td style="height: 20px; width: 284px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Due Date:</span>
                            <asp:Label ID="ReqDueDateTxtBx" runat="server" Text='<%# Eval("word_reqDueDate", "{0:d}") %>'
                                Width="120px" Font-Names="Verdana" Font-Size="8pt" Height="19px" BorderStyle="Solid"
                                BorderWidth="1px" BorderColor="DarkGray"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="bottom">
                        <td style="height: 20px; width: 217px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana"></span>
                            <asp:TextBox ID="word_StatNumTextBox" runat="server" Text='<%# Eval("word_StatNum") %>'
                                Width="111px" Font-Names="Verdana" Font-Size="8pt" Visible="False"></asp:TextBox>
                        </td>
                        <td style="height: 20px; width: 193px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Phase:</span>
                            <asp:Label ID="PhaseLabel" runat="server" Text='<%# Eval("word_Status") %>' Width="135px"
                                Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray"
                                Height="19px"></asp:Label>
                        </td>
                        <td style="height: 20px; width: 284px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Status:</span>
                            <asp:Label ID="PhaseStatLabel" runat="server" Text='<%# Eval("word_statFlag") %>'
                                Width="120px" Font-Names="Verdana" Font-Size="8pt" Height="19px" BorderStyle="Solid"
                                BorderWidth="1px" BorderColor="DarkGray"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td style="height: 20px; width: 217px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Author:</span>
                            <asp:Label ID="dddlAuthor" runat="server" Text='<%# Eval("word_Author") %>' Width="120px"
                                Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray"
                                Height="19px"></asp:Label></td>
                        <td style="height: 20px; width: 193px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">PM/SME:</span>
                            <asp:Label ID="PMorSMELabel" runat="server" Text='<%# Eval("word_PMorSME") %>' Width="120px"
                                Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px" BorderColor="DarkGray"
                                Height="19px"></asp:Label>
                        </td>
                        <td style="height: 20px; width: 284px;" align="left" valign="bottom">
                            <span style="font-size: 8pt; font-family: Verdana">Bsn Owner:</span>
                            <asp:Label ID="BusnOwnerLabel" runat="server" Text='<%# Eval("word_BusnOwner") %>'
                                Width="120px" Font-Names="Verdana" Font-Size="8pt" Height="19px" BorderStyle="Solid"
                                BorderWidth="1px" BorderColor="DarkGray"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 217px">
                        </td>
                        <td style="width: 193px">
                            <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("word_Doc") %>' />
                        </td>
                        <td style="width: 284px">
                            <asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Eval("word_DocVer") %>' />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:HiddenField ID="FilterArg1" runat="server" Value="IWOR" />
                <asp:HiddenField ID="FilterArg2" runat="server" Value="SUSP" />
                <asp:HiddenField ID="FilterArg3" runat="server" Value="WORA" />
                <span style="font-size: 8pt; font-family: Verdana">Description:</span><br />
                <asp:Label ID="word_DescrLabel" runat="server" Text='<%# Eval("word_Descr") %>' Height="93px"
                    Width="630px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                <span style="font-size: 8pt; font-family: Verdana">Justification:</span><br />
                <asp:Label ID="word_JustifyLabel" runat="server" Text='<%# Eval("word_Justify") %>'
                    Height="93px" Width="630px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid"
                    BorderWidth="1px"></asp:Label><br />
                <span style="font-size: 8pt; font-family: Verdana">Comments:</span><br />
                <asp:Label ID="word_CmntsLabel" runat="server" Text='<%# Eval("word_Cmnts") %>' Height="93px"
                    Width="629px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                <br />
                <asp:LinkButton ID="ReturnButton" runat="server" CausesValidation="False" CommandName="Return"
                    Text="Return" Font-Names="Verdana" Font-Size="8pt" OnClick="ReturnButton_Click"></asp:LinkButton>
                <asp:LinkButton ID="RespondButton" runat="server" CausesValidation="False" Text="Respond"
                    Font-Names="Verdana" Font-Size="8pt" OnClick="RespondButton_Click"></asp:LinkButton>
                <asp:LinkButton ID="ApproveButton" runat="server" CausesValidation="False" Text="Approval . . ."
                    Font-Names="Verdana" Font-Size="8pt" Visible="false"></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <table style="font-size: 8pt; font-family: Verdana; vertical-align: middle;" width="968">
                    <tr style="height: 20px">
                        <td style="width: 811px; height: 17px; font-size: 8pt; font-family: Verdana">
                            <span style="font-family: Verdana"><strong>Work Order:</strong></span>
                            <asp:Label ID="word_WOnumLabel1" runat="server" Text='<%# Eval("word_WOnum") %>'
                                Font-Names="Verdana" Font-Size="10pt" Width="90px"></asp:Label><br />
                        </td>
                        <td style="width: 115px; height: 17px">
                        </td>
                        <td style="width: 221px; height: 17px" align="right">
                            <strong><span style="font-family: Verdana">Create Date</span>:</strong>
                            <asp:Label ID="word_DateLabel" runat="server" Text='<%# Bind("word_Date", "{0:d}") %>'
                                Font-Names="Verdana" Font-Size="8pt" BorderStyle="None" Width="65px"></asp:Label><br />
                        </td>
                    </tr>
                    <tr style="height: 20px">
                    </tr>
                    <tr>
                        <td style="width: 811px; height: 17px" valign="top">
                            <span style="font-family: Verdana; vertical-align: top; font-size: 8pt"><strong>Title:</strong></span>
                            <asp:TextBox ID="word_TitleTextBox" runat="server" Text='<%# Bind("word_Title") %>'
                                Width="702px" Font-Bold="True" Font-Names="Verdana" Font-Size="10pt" Style="position: absolute"></asp:TextBox>
                        </td>
                        <td style="width: 115px; height: 17px">
                        </td>
                        <td style="width: 221px; height: 27px" valign="top" align="right">
                        </td>
                    </tr>
                </table>
                <table style="width: 970px">
                    <tr>
                        <td style="height: 21px; width: 45px;" align="left" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; text-align: left;">Phase:</span></td>
                        <td style="height: 21px">
                            <asp:DropDownList ID="DDDLphase" runat="server" DataSourceID="DDDLphaseDS" DataTextField="code_desc"
                                DataValueField="code_id" SelectedValue='<%# Bind("word_Status") %>' Width="153px"
                                Font-Names="Verdana" Font-Size="8pt" OnDataBinding="DDDLphase_DataBinding">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px; width: 205px;" align="right" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; text-align: right">Phase Status:</span></td>
                        <td style="height: 21px">
                            <asp:DropDownList ID="DropDownList1" runat="server" Width="125px" Height="19px" Font-Names="Verdana"
                                Font-Size="8pt" SelectedValue='<%# Bind("word_statFlag") %>'>
                                <asp:ListItem Value="0">In Progress</asp:ListItem>
                                <asp:ListItem Value="1">Completed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px; width: 150px;" align="right" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana"></span>
                        </td>
                        <td style="height: 21px">
                            <asp:TextBox ID="word_StatNumTextBox" runat="server" Text='<%# Bind("word_StatNum") %>'
                                Width="111px" Font-Names="Verdana" Font-Size="8pt" Visible="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px; width: 45px;" align="left" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; text-align: right; vertical-align: middle">
                                Project:</span></td>
                        <td>
                            <asp:DropDownList ID="DDDLproj" runat="server" DataSourceID="DDDLprojDS" DataTextField="code_desc"
                                DataValueField="code_id" SelectedValue='<%# Bind("word_Proj") %>' Width="164px"
                                Font-Names="Verdana" Font-Size="8pt" Height="20px">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px; width: 213px;" align="right" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; vertical-align: middle; text-align: right;">
                                Priority:</span></td>
                        <td>
                            <asp:DropDownList ID="DDDLpriority" runat="server" DataSourceID="DDDLpriorityDS"
                                DataTextField="code_desc" DataValueField="code_id" SelectedValue='<%# Bind("word_Priority") %>'
                                Width="101px" Font-Names="Verdana" Font-Size="8pt" Height="20px">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px; width: 150px;" align="right" valign="middle">
                            <%--<span style="font-size: 8pt; font-family: Verdana; vertical-align:top">Due Date:</span>
                            <asp:TextBox ID="word_reqDueDateTextBox" runat="server" Text='<%# Bind("word_reqDueDate", "{0:d}") %>' Width="98px" Font-Names="Verdana" Font-Size="8pt" Visible="false" Height="20px"&gt;--%&gt; <%--<cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="word_reqDueDateTextBox">
                            </cc1:CalendarExtender>
                            <asp:DateTimePicker ID="CalendarExtender1" runat="server" AutoFormat="Professional" Width="1px" Value='<%# Bind("word_reqDueDate", "{0:d}") %>' Font-Names="Verdana" Font-Size="8pt"/>--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 21px; width: 45px;" align="left" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana">Author:</span></td>
                        <td>
                            <asp:DropDownList ID="dddlAuthor" runat="server" DataSourceID="DDDLobjectDS" DataTextField="FullName"
                                DataValueField="wusr_uid" SelectedValue='<%# Bind("word_Author") %>' Width="125px"
                                Font-Names="Verdana" Font-Size="8pt">
                            </asp:DropDownList></td>
                        <td style="height: 21px; width: 213px;" align="right" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; vertical-align: middle; text-align: right">
                                EBS Project Manager:</span></td>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="DDLpmDS" DataTextField="FullName"
                                DataValueField="wusr_uid" SelectedValue='<%# Bind("word_PMorSME") %>' Width="125px"
                                Font-Names="Verdana" Font-Size="8pt">
                            </asp:DropDownList>
                        </td>
                        <td style="height: 21px; width: 150px;" align="right" valign="middle">
                            <span style="font-size: 8pt; font-family: Verdana; text-align: right">Business Owner:</span></td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="DDLbsnsownerDS"
                                DataTextField="FullName" DataValueField="wusr_uid" SelectedValue='<%# Bind("word_BusnOwner") %>'
                                Width="125px" Font-Names="Verdana" Font-Size="8pt" Height="19px" CssClass="label1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%--<tr><td></td>
                    <td><asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Bind("word_Doc") %>' /></td>
                    <td><asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Bind("word_DocVer") %>' /></td></tr>--%>
                </table>
                <br />
                <br />
                <span style="font-size: 8pt; font-family: Verdana; vertical-align: top; text-align: left;">
                    Requested Due Date:</span><asp:DateTimePicker ID="CalendarExtender1" runat="server"
                        AutoFormat="Professional" Width="1px" Value='<%# Bind("word_reqDueDate", "{0:d}") %>'
                        Font-Names="Verdana" Font-Size="8pt" style="z-index: 101; left: 123px; top: 233px" />
                <br />
                <asp:HiddenField ID="FilterArg1" runat="server" Value="IWOR" />
                <asp:HiddenField ID="FilterArg2" runat="server" Value="SUSP" />
                <asp:HiddenField ID="FilterArg3" runat="server" Value="WORA" />
                <asp:ObjectDataSource ID="DDDLobjectDS" runat="server" SelectMethod="GetUsersDDList"
                    TypeName="woUsersBLL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="DDLpmDS" runat="server" SelectMethod="GetDDLusersByRole"
                    TypeName="woUsersBLL" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="PM" Name="role" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="DDLbsnsownerDS" runat="server" SelectMethod="GetDDLusersByRole"
                    TypeName="woUsersBLL" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="BOWN" Name="role" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="DDDLprojDS" runat="server" SelectMethod="GetProjectCodes"
                    TypeName="CodesBLL" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="DDDLpriorityDS" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetPriorityCodes" TypeName="CodesBLL"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="DDDLphaseDS" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetPhaseStatus" TypeName="CodesBLL">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="FilterArg1" Name="p1" PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="FilterArg2" Name="p2" PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="FilterArg3" Name="p3" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:Label ID="word_TitleLabel" runat="server" Text='(Add additional work order participants at the bottom of the page.)'
                    Width="630px" ForeColor="Gray" Font-Names="Verdana" Font-Size="10pt"></asp:Label><br />
                <br />
                <span style="font-size: 8pt; font-family: Verdana">Description:<br />
                </span>
                <asp:TextBox ID="word_DescrTextBox" runat="server" Text='<%# Bind("word_Descr") %>'
                    Height="51px" Width="970px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                <span style="font-size: 8pt; font-family: Verdana">Justification:<br />
                </span>
                <asp:TextBox ID="word_JustifyTextBox" runat="server" Text='<%# Bind("word_Justify") %>'
                    Height="51px" Width="970px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                <span style="font-size: 8pt; font-family: Verdana">Comments:<br />
                </span>
                <asp:TextBox ID="word_CmntsTextBox" runat="server" Text='<%# Bind("word_Cmnts") %>'
                    Height="51px" Width="970px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                <br />
                <asp:LinkButton ID="UpdateButton" runat="server" CommandName="Update" Text="Update"
                    Font-Names="Verdana" Font-Size="8pt" CausesValidation="False"></asp:LinkButton>
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" Font-Names="Verdana" Font-Size="8pt" OnClick="UpdateCancelButton_Click"></asp:LinkButton>
                <asp:LinkButton ID="RespondButton" runat="server" CausesValidation="False" Text="Respond"
                    Font-Names="Verdana" Font-Size="8pt" OnClick="RespondButton_Click1"></asp:LinkButton>
                <asp:LinkButton ID="Approvals" runat="server" Text="Approvals" Font-Names="Verdana"
                    Font-Size="8pt" Visible="false" OnClick="Approvals_Click"></asp:LinkButton>
            </EditItemTemplate>
        </asp:FormView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetWorkOrdersByWOnumProj"
            TypeName="WorkOrderBLL" UpdateMethod="UpdateWO" OldValuesParameterFormatString="original_{0}"
            InsertMethod="InsertWorkOrder" OnUpdating="ObjectDataSource1_Updating" OnUpdated="ObjectDataSource1_Updated">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="" Name="wonum" QueryStringField="word_WOnum"
                    Type="Int32" />
                <asp:QueryStringParameter Name="proj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="word_WOnum" Type="Int32" />
                <asp:Parameter Name="word_Proj" Type="String" />
                <asp:Parameter Name="word_StatNum" Type="Int32" />
                <asp:Parameter Name="word_Status" Type="String" />
                <asp:Parameter Name="word_statFlag" Type="Int32" />
                <asp:Parameter Name="word_Date" Type="DateTime" />
                <asp:Parameter Name="word_Author" Type="String" />
                <asp:Parameter Name="word_Title" Type="String" />
                <asp:Parameter Name="word_Priority" Type="Int32" />
                <asp:Parameter Name="word_Descr" Type="String" />
                <asp:Parameter Name="word_Justify" Type="String" />
                <asp:Parameter Name="word_Cmnts" Type="String" />
                <asp:Parameter Name="word_PMorSME" Type="Int32" />
                <asp:Parameter Name="word_BusnOwner" Type="Int32" />
                <asp:Parameter Name="word_reqDueDate" Type="DateTime" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="word_wonum" Type="Int32" />
                <asp:Parameter Name="word_proj" Type="String" />
                <asp:Parameter Name="word_statnum" Type="Int32" />
                <asp:Parameter Name="word_status" Type="String" />
                <asp:Parameter Name="word_statflag" Type="Int32" />
                <asp:Parameter Name="word_date" Type="DateTime" />
                <asp:Parameter Name="word_author" Type="String" />
                <asp:Parameter Name="word_title" Type="String" />
                <asp:Parameter Name="word_doc" Type="String" />
                <asp:Parameter Name="word_docver" Type="String" />
                <asp:Parameter Name="word_priority" Type="Int32" />
                <asp:Parameter Name="word_descr" Type="String" />
                <asp:Parameter Name="word_justify" Type="String" />
                <asp:Parameter Name="word_cmnts" Type="String" />
                <asp:Parameter Name="word_pmorsme" Type="Int32" />
                <asp:Parameter Name="word_busnowner" Type="Int32" />
                <asp:Parameter Name="word_reqduedate" Type="DateTime" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetFullTextWOs" TypeName="WorkOrderBLL" DataObjectTypeName="WorkOrder+WorkOrderRow"
            InsertMethod="InsertWorkOrder" UpdateMethod="UpdateWorkOrder">
            <SelectParameters>
                <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                <asp:QueryStringParameter Name="woproj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="word_wonum" Type="Int32" />
                <asp:Parameter Name="word_proj" Type="String" />
                <asp:Parameter Name="word_statnum" Type="Int32" />
                <asp:Parameter Name="word_status" Type="String" />
                <asp:Parameter Name="word_statflag" Type="Int32" />
                <asp:Parameter Name="word_date" Type="DateTime" />
                <asp:Parameter Name="word_author" Type="String" />
                <asp:Parameter Name="word_title" Type="String" />
                <asp:Parameter Name="word_doc" Type="String" />
                <asp:Parameter Name="word_docver" Type="String" />
                <asp:Parameter Name="word_priority" Type="Int32" />
                <asp:Parameter Name="word_descr" Type="String" />
                <asp:Parameter Name="word_justify" Type="String" />
                <asp:Parameter Name="word_cmnts" Type="String" />
                <asp:Parameter Name="word_pmorsme" Type="Int32" />
                <asp:Parameter Name="word_busnowner" Type="Int32" />
                <asp:Parameter Name="word_reqduedate" Type="DateTime" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <contenttemplate>
<TABLE width=650><TBODY><TR><TD style="WIDTH: 145px; HEIGHT: 280px">&nbsp;<asp:Label style="LEFT: 1px; POSITION: relative; TOP: -20px" id="lblParticipant" runat="server" Text="Participant" Width="138px" BackColor="Gray" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True"></asp:Label> <asp:Label style="LEFT: 5px; POSITION: relative; TOP: 27px" id="lblRole" runat="server" Text="Role" Width="138px" BackColor="Gray" Font-Size="8pt" Font-Names="Verdana" Font-Bold="True"></asp:Label> <asp:DropDownList style="LEFT: 5px; POSITION: relative; TOP: -36px" id="ddlParticipant" runat="server" DataSourceID="ObjectDataSource3" Width="138px" AppendDataBoundItems="true" DataValueField="wusr_uid" DataTextField="FullName">
                                <asp:ListItem Value="-1">-(None)-</asp:ListItem>
                            </asp:DropDownList> <asp:DropDownList style="LEFT: 5px; POSITION: relative; TOP: 8px" id="ddlRole" runat="server" DataSourceID="ObjectDataSource4" Width="138px" AppendDataBoundItems="True" DataValueField="code_id" DataTextField="code_desc">
                                <asp:ListItem Value="-1">-(None)-</asp:ListItem>
                            </asp:DropDownList></TD><TD style="WIDTH: 37px; HEIGHT: 280px; TEXT-ALIGN: center"><BR /><asp:Button style="LEFT: 1px; POSITION: relative; TOP: 6px" id="btnAddParticipant" onclick="Button1_Click" runat="server" Text=">" Width="24px"></asp:Button><BR /><BR /><BR /></TD><TD style="WIDTH: 651px; HEIGHT: 280px"><asp:GridView id="GridView2" runat="server" OnPreRender="GridView2_PreRender" DataSourceID="ObjectDataSource5" Width="732px" DataKeyNames="worl_index" OnRowUpdated="GridView2_RowUpdated" OnRowUpdating="GridView2_RowUpdating" ForeColor="Black" OnRowDeleted="GridView2_RowDeleted" OnDataBound="GridView2_DataBound" AutoGenerateColumns="False" OnRowDataBound="GridView2_RowDataBound">
<RowStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"></RowStyle>
<Columns>
<asp:CommandField ShowDeleteButton="True" ShowEditButton="True">
<ItemStyle Font-Names="Verdana" Font-Size="8pt" Width="20px"></ItemStyle>
</asp:CommandField>
<asp:BoundField DataField="worl_uid" HeaderText="UID" ReadOnly="True" SortExpression="worl_uid">
<HeaderStyle HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt" Width="55px"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="FullName" HeaderText="FullName" ReadOnly="True" SortExpression="FullName">
<HeaderStyle Font-Names="Verdana" Font-Size="8pt"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Font-Names="Verdana" Font-Size="8pt" Width="200px"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="RoleTxt" HeaderText="Role" ReadOnly="True" SortExpression="RoleTxt">
<HeaderStyle Font-Names="Verdana" Font-Size="8pt"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Font-Names="Verdana" Font-Size="8pt" Width="125px"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Require Their Approval?"><EditItemTemplate>
                                           <asp:CheckBox runat="server" ID="checkbox" Checked='<%# Bind("worl_aprvl") %>' ForeColor="Black" />
                                       
</EditItemTemplate>
<ItemTemplate>
<asp:CheckBox id="aprvChkbx" runat="server" Text="Yes" Checked='<%# Bind("worl_aprvl") %>' Visible="False"></asp:CheckBox>&nbsp; <asp:Image id="ImageAprv" runat="server" ImageUrl='<%# (bool.Parse(Eval("worl_aprvl").ToString())?"~/WorkOrder/Images/checked.bmp":"~/WorkOrder/Images/unchecked.bmp") %>'></asp:Image> 
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<ItemStyle Width="100px"></ItemStyle>
</asp:TemplateField>
<asp:BoundField DataField="worl_index" HeaderText="worl_index" ReadOnly="True" SortExpression="worl_index" Visible="False"></asp:BoundField>
<asp:TemplateField HeaderText="Email?" SortExpression="worl_email"><EditItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("worl_email") %>' />
                                        
</EditItemTemplate>
<ItemTemplate>
<asp:CheckBox id="emailChkbx" runat="server" Text="Yes" ForeColor="Black" Visible="False" Checked='<%# Bind("worl_email") %>'></asp:CheckBox>&nbsp; <asp:Image id="imgEmail" runat="server" ImageUrl='<%# (bool.Parse(Eval("worl_email").ToString())?"~/WorkOrder/Images/checked.bmp":"~/WorkOrder/Images/unchecked.bmp") %>'></asp:Image> 
</ItemTemplate>

<ControlStyle Font-Names="Verdana" ForeColor="Black"></ControlStyle>

<HeaderStyle HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt"></HeaderStyle>

<ItemStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Width="55px"></ItemStyle>
</asp:TemplateField>
    <asp:BoundField DataField="worl_role" SortExpression="worl_role" Visible="False" />
</Columns>

<EditRowStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Red"></EditRowStyle>
</asp:GridView> </TD></TR></TBODY></TABLE><asp:ObjectDataSource id="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}" TypeName="woUsersBLL" SelectMethod="GetUsersDDList"></asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource4" runat="server" OldValuesParameterFormatString="original_{0}" TypeName="CodesBLL" SelectMethod="GetRoleCodes"></asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource5" runat="server" InsertMethod="InsertWOroles" OldValuesParameterFormatString="original_{0}" TypeName="RolesBLL" SelectMethod="GetWOrolesByWOnumProj" DeleteMethod="DeleteWOparticipant" UpdateMethod="UpdateRoles">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                        <asp:QueryStringParameter Name="proj" QueryStringField="word_Proj" Type="String" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="Original_worl_index" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="worl_WOnum" Type="Int32" />
                        <asp:Parameter Name="worl_uid" Type="Int32" />
                        <asp:Parameter Name="worl_proj" Type="String" />
                        <asp:Parameter Name="worl_role" Type="String" />
                        <asp:Parameter Name="worl_aprvl" Type="Boolean" />
                        <asp:Parameter Name="worl_email" Type="Boolean" />
                    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="worl_WOnum" Type="Int32" />
        <asp:Parameter Name="worl_proj" Type="String" />
        <asp:Parameter Name="worl_uid" Type="Int32" />
        <asp:Parameter Name="worl_role" Type="String" />
        <asp:Parameter Name="worl_aprvl" Type="Boolean" />
        <asp:Parameter Name="Original_worl_index" Type="Int32" />
        <asp:Parameter Name="worl_email" Type="Boolean" />
    </UpdateParameters>
                </asp:ObjectDataSource> <asp:ObjectDataSource id="ObjectDataSource6" runat="server" InsertMethod="InsertWOroles" OldValuesParameterFormatString="original_{0}" UpdateMethod="UpdateRoles" TypeName="RolesBLL" SelectMethod="GetWOrolesByWOnumProj" DeleteMethod="DeleteWOparticipant">
                    <DeleteParameters>
                        <asp:Parameter Name="Original_worl_index" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="worl_WOnum" Type="Int32" />
                        <asp:Parameter Name="worl_proj" Type="String" />
                        <asp:Parameter Name="worl_uid" Type="Int32" />
                        <asp:Parameter Name="worl_role" Type="String" />
                        <asp:Parameter Name="worl_aprvl" Type="Boolean" />
                        <asp:Parameter Name="Original_worl_index" Type="Int32" />
                        <asp:Parameter Name="worl_aprvl1" Type="Boolean" />
                        <asp:Parameter Name="worl_email" Type="Boolean" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                        <asp:QueryStringParameter Name="proj" QueryStringField="word_Proj" Type="String" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="worl_WOnum" Type="Int32" />
                        <asp:Parameter Name="worl_uid" Type="Int32" />
                        <asp:Parameter Name="worl_proj" Type="String" />
                        <asp:Parameter Name="worl_role" Type="String" />
                        <asp:Parameter Name="worl_aprvl" Type="Boolean" />
                        <asp:Parameter Name="worl_email" Type="Boolean" />
                    </InsertParameters>
                </asp:ObjectDataSource> 
</contenttemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;<br />
        &nbsp;<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <contenttemplate>
<asp:FormView id="FormView2" runat="server" DataSourceID="ObjectDataSource8" DataKeyNames="wrsp_RespNum,wrsp_WOnum,wrsp_Proj" Font-Size="8pt" Font-Names="Verdana" OnDataBound="FormView2_DataBound" BorderWidth="2px" BorderStyle="Inset" OnPageIndexChanging="FormView2_PageIndexChanging" AllowPaging="True">
<FooterStyle Font-Names="Verdana"></FooterStyle>
<ItemTemplate>
<TABLE><TBODY><TR><TD>Response Number: <asp:Label id="wrsp_RespNumLabel" runat="server" Text='<%# Eval("wrsp_RespNum") %>'></asp:Label></TD><TD style="WIDTH: 190px"></TD><TD style="WIDTH: 209px" align=right>Respondant:&nbsp; <asp:Label id="wrsp_uidLabel" runat="server" Text='<%# Bind("wrsp_uid") %>' Width="92px"></asp:Label></TD></TR></TBODY></TABLE><BR /><BR />Estimated Hours: <asp:Label id="wrsp_EstHrsLabel" runat="server" Text='<%# Bind("wrsp_EstHrs") %>' Width="50px"></asp:Label><BR /><BR />Results:<BR /><asp:TextBox id="wrsp_ResultsLabel" runat="server" Text='<%# Bind("wrsp_Results") %>' Width="650px" Height="93px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox><BR />Consider:<BR /><asp:TextBox id="wrsp_ConsiderLabel" runat="server" Text='<%# Bind("wrsp_Consider") %>' Width="650px" Height="93px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox><BR />Risks:<BR /><asp:TextBox id="wrsp_RisksLabel" runat="server" Text='<%# Bind("wrsp_Risks") %>' Width="650px" Height="93px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox><BR />
</ItemTemplate>
<FooterTemplate>
                        <asp:LinkButton ID="lnkBtnRespond2" runat="server" CausesValidation="False" OnClick="lnkBtnRespond2_Click">Respond</asp:LinkButton>
                    
</FooterTemplate>
<HeaderTemplate>
                        <asp:TextBox ID="txtBxHeader" runat="server" Style="font-weight: bold; font-size: 8pt;
                            vertical-align: middle; border-top-style: none; font-family: Verdana; border-right-style: none;
                            border-left-style: none; text-align: center; border-bottom-style: none" Width="638px"></asp:TextBox>
                    
</HeaderTemplate>
</asp:FormView> &nbsp; 
</contenttemplate>
        </asp:UpdatePanel>
        &nbsp;
        <asp:ObjectDataSource ID="ObjectDataSource8" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetRespByWOproj" TypeName="woResponseBLL" InsertMethod="InsertNewWOresponse"
            UpdateMethod="UpdateWOresponse">
            <SelectParameters>
                <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                <asp:QueryStringParameter Name="proj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="wrsp_RespNum" Type="Int32" />
                <asp:Parameter Name="wrsp_WOnum" Type="Int32" />
                <asp:Parameter Name="wrsp_Proj" Type="String" />
                <asp:Parameter Name="wrsp_StatNum" Type="Int32" />
                <asp:Parameter Name="wrsp_Status" Type="String" />
                <asp:Parameter Name="wrsp_statFlag" Type="Int32" />
                <asp:Parameter Name="wrsp_EstMnths" Type="Int32" />
                <asp:Parameter Name="wrsp_EstHrs" Type="Int32" />
                <asp:Parameter Name="wrsp_Results" Type="String" />
                <asp:Parameter Name="wrsp_Consider" Type="String" />
                <asp:Parameter Name="wrsp_Risks" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="wrsp_RespNum" Type="Int32" />
                <asp:Parameter Name="wrsp_WOnum" Type="Int32" />
                <asp:Parameter Name="wrsp_Proj" Type="String" />
                <asp:Parameter Name="wrsp_StatNum" Type="Int32" />
                <asp:Parameter Name="wrsp_Status" Type="String" />
                <asp:Parameter Name="wrsp_statFlag" Type="Int32" />
                <asp:Parameter Name="wrsp_EstMnths" Type="Int32" />
                <asp:Parameter Name="wrsp_EstHrs" Type="Int32" />
                <asp:Parameter Name="wrsp_Results" Type="String" />
                <asp:Parameter Name="wrsp_Consider" Type="String" />
                <asp:Parameter Name="wrsp_Risks" Type="String" />
                <asp:Parameter Name="wrsp_uid" Type="Int32" />
                <asp:Parameter Name="wrsp_datetime" Type="DateTime" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>

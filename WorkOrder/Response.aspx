<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Response.aspx.cs" Title="Work Order Response" Inherits="Response" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetFullTextWOs" TypeName="WorkOrderBLL">
            <SelectParameters>
                <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                <asp:QueryStringParameter Name="woproj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:FormView ID="FormView1" runat="server" DataSourceID="ObjectDataSource1" CssClass="Label" Font-Names="Verdana" Font-Size="10pt" BorderStyle="Inset" BorderWidth="1px" Caption="Work Order Detail" CaptionAlign="Top">
            <ItemTemplate>
            <table>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Work Order Number:</span></td>
                <td><asp:Label ID="WOnumLabel" runat="server" Text='<%# Bind("word_WOnum") %>'></asp:Label><br /></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Project:</span></td>
                <td><asp:Label ID="ProjLabel" runat="server" Text='<%# Bind("word_Proj") %>'></asp:Label><br /></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Title:</span></td>
                <td><asp:Label ID="TitleLabel" runat="server" Text='<%# Bind("word_Title") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Author:</span></td>
                <td><asp:Label ID="AuthorLabel" runat="server" Text='<%# Bind("word_Author") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Create Date:</span></td>
                <td><asp:Label ID="DateLabel" runat="server" Text='<%# Bind("word_Date", "{0:d}") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Requested Due Date:</span></td>
                <td><asp:Label ID="reqDueDateLabel" runat="server" Text='<%# Bind("word_reqDueDate", "{0:d}") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Phase:</span></td>
                <td><asp:Label ID="StatusLabel" runat="server" Text='<%# Bind("word_Status") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Status:</span></td>
                <td><asp:Label ID="statFlagLabel" runat="server" Text='<%# Bind("word_statFlag") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Priority:</span></td>
                <td><asp:Label ID="PriorityLabel" runat="server" Text='<%# Bind("word_Priority") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Project Manager:</span></td>
                <td><asp:Label ID="PMorSMELabel" runat="server" Text='<%# Bind("word_PMorSME") %>'></asp:Label></td></tr>
                <tr>
                <td style="width: 144px"><span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Business Owner:</span></td>
                <td><asp:Label ID="BusnOwnerLabel" runat="server" Text='<%# Bind("word_BusnOwner") %>'></asp:Label></td></tr>
           </table>
                <span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Description:<br /></span>
                <asp:Label ID="DescrLabel" runat="server" Text='<%# Bind("word_Descr") %>' Height="82px" Width="731px" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                <span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Justification:<br /></span>
                <asp:Label ID="JustifyLabel" runat="server" Text='<%# Bind("word_Justify") %>' Height="82px" Width="731px" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                <span style="font-size: 9pt; font-weight:bold; font-family: Verdana">Comments:<br /></span>
                <asp:Label ID="CmntsLabel" runat="server" Text='<%# Bind("word_Cmnts") %>' Height="82px" Width="731px" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
            </ItemTemplate>
            <RowStyle CssClass="Label" />
        </asp:FormView>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server"
            SelectMethod="GetMaxRespByWOnumProj" TypeName="woResponseBLL" InsertMethod="InsertNewResponse" OnUpdated="ObjectDataSource2_Updated" OldValuesParameterFormatString="original_{0}" OnInserted="ObjectDataSource2_Inserted" UpdateMethod="UpdateWOresponse">
            <SelectParameters>
                <asp:QueryStringParameter Name="wonum" QueryStringField="word_WOnum" Type="Int32" />
                <asp:QueryStringParameter Name="proj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="wrsp_RespNum" Type="Int32" />
                <asp:Parameter Name="wrsp_WOnum" Type="Int32" />
                <asp:Parameter Name="wrsp_Proj" Type="String" />
                <asp:Parameter Name="wrsp_RespToResp" Type="Int32" />
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
        </asp:ObjectDataSource>
        <br />
        <br />
        <asp:FormView ID="FormView2" runat="server" DataKeyNames="wrsp_RespNum" DataSourceID="ObjectDataSource2" Width="733px" Height="160px" Font-Names="Verdana" Font-Size="10pt" BorderStyle="Groove" OnItemInserting="FormView2_ItemInserting" OnItemUpdating="FormView2_ItemUpdating" OnLoad="FormView2_Load" OnItemInserted="FormView2_ItemInserted" OnItemUpdated="FormView2_ItemUpdated" AllowPaging="True" Caption="Work Order Response" CaptionAlign="Top" OnDataBound="FormView2_DataBound">
            <EditItemTemplate>
                Estimated Hours:
                <asp:TextBox ID="wrsp_EstHrsTextBox" runat="server" Text='<%# Bind("wrsp_EstHrs") %>'></asp:TextBox><br />
                Results:
                <asp:TextBox ID="wrsp_ResultsTextBox" runat="server" Text='<%# Bind("wrsp_Results") %>' Height="82px" Width="731px" TextMode="MultiLine"></asp:TextBox><br />
                Considerations:
                <asp:TextBox ID="wrsp_ConsiderTextBox" runat="server" Text='<%# Bind("wrsp_Consider") %>' Height="82px" Width="731px" TextMode="MultiLine"></asp:TextBox><br />
                Risks:
                <asp:TextBox ID="wrsp_RisksTextBox" runat="server" Text='<%# Bind("wrsp_Risks") %>' Height="82px" Width="731px" TextMode="MultiLine"></asp:TextBox><br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" OnClick="UpdateCancelButton_Click"></asp:LinkButton>
                <asp:Label ID="wrsp_RespNumLabel1" runat="server" Visible="false" Text='<%# Eval("wrsp_RespNum") %>'></asp:Label><br />
                <asp:TextBox ID="wrsp_WOnumTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_WOnum") %>'></asp:TextBox><br />
                <asp:TextBox ID="wsrp_ProjTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_Proj") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_StatNumTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_StatNum") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_StatusTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_Status") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_statFlagTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_statFlag") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_EstMnthsTextBox" runat="server" Visible="False" Text='<%# Bind("wrsp_EstMnths") %>'></asp:TextBox><br />
            </EditItemTemplate>
            <InsertItemTemplate>
                Estimated Hours:
                <asp:TextBox ID="wrsp_EstHrsTextBox" runat="server" Text='<%# Bind("wrsp_EstHrs") %>' BorderStyle="Double" BorderWidth="1px" Width="58px"></asp:TextBox><span style="color:Red">  Numbers only, no alphas</span><br />
                Results:
                <asp:TextBox ID="wrsp_ResultsTextBox" runat="server" Text='<%# Bind("wrsp_Results") %>' Height="82px" Width="731px" BorderStyle="Double" BorderWidth="1px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                Considerations:
                <asp:TextBox ID="wrsp_ConsiderTextBox" runat="server" Text='<%# Bind("wrsp_Consider") %>' Height="82px" Width="731px" BorderStyle="Double" BorderWidth="1px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                Risks:
                <asp:TextBox ID="wrsp_RisksTextBox" runat="server" Text='<%# Bind("wrsp_Risks") %>' Height="82px" Width="731px" BorderStyle="Double" BorderWidth="1px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Insert"
                    Text="Update"></asp:LinkButton>
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" OnClick="UpdateCancelButton_Click1"></asp:LinkButton>
                <br />
                <asp:Label ID="wrsp_RespNumLabel1" runat="server" Visible="false" Text="0"></asp:Label><br />
                <asp:TextBox ID="wrsp_WOnumTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_WOnum") %>'></asp:TextBox><br />
                <asp:TextBox ID="wsrp_ProjTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_Proj") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_RespToRespTextBox" runat="server" Visible="False" Text='<%# Bind("wrsp_RespToResp") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_StatNumTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_StatNum") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_StatusTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_Status") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_statFlagTextBox" runat="server" Visible="false" Text='<%# Bind("wrsp_statFlag") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_EstMnthsTextBox" runat="server" Visible="False" Text='<%# Bind("wrsp_EstMnths") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_UIDTextBox" runat="server" Visible="False" Text='<%# Bind("wrsp_uid") %>'></asp:TextBox><br />
                <asp:TextBox ID="wrsp_datetimeTextBox" runat="server" Visible="False" Text='<%# Bind("wrsp_datetime") %>'></asp:TextBox><br />
            </InsertItemTemplate>
            <ItemTemplate>
            <table><tr><td style="width: 393px; height: 34px;" valign="middle">
                Estimated Hours:
                <asp:Label ID="wrsp_EstHrsLabel" runat="server" Text='<%# Bind("wrsp_EstHrs") %>' Height="21px" Width="72px"></asp:Label></td>
                <td style="width: 362px; height: 34px" valign="middle"></td><td style="width: 481px; height: 34px;" align="right" valign="middle">Respondant:
                <asp:Label ID="UIDlabel" runat="server" Text='<%# Bind("wrsp_uid") %>' Height="21px" Width="72px"></asp:Label></td>
                </tr></table><br />
                <br />
                Results:
                <asp:TextBox ID="wrsp_ResultsLabel" runat="server" Text='<%# Bind("wrsp_Results") %>' Height="82px" Width="731px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox><br />
                Consider:
                <asp:TextBox ID="wrsp_ConsiderLabel" runat="server" Text='<%# Bind("wrsp_Consider") %>' Height="82px" Width="731px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox><br />
                Risks:
                <asp:TextBox ID="wrsp_RisksLabel" runat="server" Text='<%# Bind("wrsp_Risks") %>' Height="82px" Width="731px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox><br />
                <asp:Label ID="wrsp_RespNumLabel" runat="server" Visible="False" Text='<%# Eval("wrsp_RespNum") %>'></asp:Label><br />
                <asp:Label ID="wrsp_WOnumLabel" runat="server" Visible="false" Text='<%# Bind("wrsp_WOnum") %>'></asp:Label><br />
                <asp:Label ID="wsrp_ProjLabel" runat="server" Visible="False" Text='<%# Bind("wrsp_Proj") %>'></asp:Label><br />
                <asp:Label ID="wrsp_RespToRespLabel" runat="server" Visible="false" Text='<%# Bind("wrsp_RespToResp") %>'></asp:Label><br />
                <asp:Label ID="wrsp_StatNumLabel" runat="server" Visible="False" Text='<%# Bind("wrsp_StatNum") %>'></asp:Label><br />
                <asp:Label ID="wrsp_StatusLabel" runat="server" Visible="False" Text='<%# Bind("wrsp_Status") %>'></asp:Label><br />
                <asp:Label ID="wrsp_statFlagLabel" runat="server" Visible="False" Text='<%# Bind("wrsp_statFlag") %>'></asp:Label><br />
                <asp:Label ID="wrsp_EstMnthsLabel" runat="server" Visible="False" Text='<%# Bind("wrsp_EstMnths") %>'></asp:Label><br />
                <asp:Label ID="wrsp_datetime" runat="server" Visible="False" Text='<%# Bind("wrsp_datetime") %>'></asp:Label><br />
                <asp:LinkButton ID="lnkBtnReturn" runat="server" OnClick="lnkBtnReturn_Click">Return</asp:LinkButton><br />
                
            </ItemTemplate>
            <EmptyDataTemplate>
                <span style="font-family: Verdana"><strong>No Response Yet Created For This Work Order.</strong></span>
            </EmptyDataTemplate>
            <PagerSettings Mode="NextPreviousFirstLast" />
            <RowStyle Font-Names="Verdana" />
            <InsertRowStyle Font-Names="Verdana" />
            <EditRowStyle Font-Names="Verdana" />
            <HeaderTemplate>
                <asp:TextBox ID="txtBxHeader" runat="server" Font-Bold="True" ReadOnly="True" Style="font-family: Verdana;
                    text-align: center" Width="713px"></asp:TextBox>
            </HeaderTemplate>
        </asp:FormView>
    </asp:Content>

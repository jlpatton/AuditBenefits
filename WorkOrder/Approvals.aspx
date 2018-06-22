<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Approvals.aspx.cs" Title="Work Order Approvals" Inherits="Approvals" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
    <span style="font-family:Verdana; font-size:14pt; color:Gray; font-weight:bold">The following is a list of the participants responsible for approving this work order:</span>
        <br />
        <br />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="wapr_AprvNum"
            DataSourceID="ObjectDataSource1" CellPadding="4" ForeColor="#333333" OnPreRender="GridView1_PreRender" Font-Names="Verdana" Font-Size="8pt">
            <Columns>
                <asp:BoundField DataField="wapr_UidFullname" HeaderText="Approver" SortExpression="wapr_UidFullname" />
                <asp:BoundField DataField="wapr_Approver" HeaderText="UID" SortExpression="wapr_Approver" Visible="False" />
                <asp:CheckBoxField DataField="wapr_required" HeaderText="Approval Required?" SortExpression="wapr_required" >
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:CheckBoxField>
                <asp:CheckBoxField DataField="wapr_AprvCode" HeaderText="GO?" SortExpression="wapr_AprvCode" >
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:CheckBoxField>
                <asp:BoundField DataField="wapr_comments" HeaderText="Comments" SortExpression="wapr_comments" >
                    <ItemStyle Width="400px" />
                </asp:BoundField>
                <asp:BoundField DataField="wapr_AprvlDate" HeaderText="Approval Date" SortExpression="wapr_AprvlDate" DataFormatString="{0:d}" >
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="wapr_AprvNum" HeaderText="wapr_AprvNum" ReadOnly="True"
                    SortExpression="wapr_AprvNum" Visible="False" />
                <asp:BoundField DataField="wapr_AprvDate" HeaderText="wapr_AprvDate" SortExpression="wapr_AprvDate"
                    Visible="False" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="DarkGray" ForeColor="#284775" />
        </asp:GridView>
    </div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetApprovalDataWithUIDFullNameText" TypeName="woApprovalBLL" DeleteMethod="DeleteByApprovalNum" InsertMethod="InsertNewWOApproval" UpdateMethod="UpdateWOAprvlRecord">
            <SelectParameters>
                <asp:QueryStringParameter Name="word_WOnum" QueryStringField="word_WOnum" Type="Int32" />
                <asp:QueryStringParameter Name="word_Proj" QueryStringField="word_Proj" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="apprvlNum" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="wapr_AprvNum" Type="Int32" />
                <asp:Parameter Name="wapr_WOnum" Type="Int32" />
                <asp:Parameter Name="wapr_Proj" Type="String" />
                <asp:Parameter Name="wapr_AprvDate" Type="DateTime" />
                <asp:Parameter Name="wapr_Approver" Type="Int32" />
                <asp:Parameter Name="wapr_AprvCode" Type="Boolean" />
                <asp:Parameter Name="wapr_AprvlDate" Type="DateTime" />
                <asp:Parameter Name="wapr_comments" Type="String" />
                <asp:Parameter Name="wapr_required" Type="Boolean" />
                <asp:Parameter Name="wapr_emailFlag" Type="Boolean" />
                <asp:Parameter Name="Original_wapr_AprvNum" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="wapr_AprvNum" Type="Int32" />
                <asp:Parameter Name="wapr_WOnum" Type="Int32" />
                <asp:Parameter Name="wapr_Proj" Type="String" />
                <asp:Parameter Name="wapr_AprvDate" Type="DateTime" />
                <asp:Parameter Name="wapr_Approver" Type="Int32" />
                <asp:Parameter Name="wapr_AprvCode" Type="Boolean" />
                <asp:Parameter Name="wapr_AprvlDate" Type="DateTime" />
                <asp:Parameter Name="wapr_comments" Type="String" />
                <asp:Parameter Name="wapr_required" Type="Boolean" />
                <asp:Parameter Name="wapr_emailFlag" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <br />
        <%--<asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetUsersDDList" TypeName="woUsersBLL">
        </asp:ObjectDataSource>--%>
        <br />
        <span id="span4" style="font-family:Verdana; font-size:12pt; color:Red; font-weight:normal">
            <asp:Label ID="Label1" runat="server" Text="To approve and/or comment on this work order click the Approval Button. You will be prompted for your LDAP password in order to continue with the approval. This will serve as an electronic signature of your Approval or Non-Approval."
                Visible="False"></asp:Label><br />
            <br />
            <asp:LinkButton ID="lnkBtnReturn" runat="server" OnClick="lnkBtnReturn_Click" Visible="False">Return</asp:LinkButton><br />
        </span><br />
        <asp:Button ID="btnApprove" runat="server" Font-Size="Larger" Text="Approval" Visible="False" OnClick="btnApprove_Click" CausesValidation="False" /><br />
        <br />
        <span id="span3" style="font-family:Verdana; font-size:12pt; color:Black; font-weight:normal">
            <asp:Label ID="Label2" runat="server" Text="Type in your LDAP password and check the checkbox to approve or leave checked to explicitly 'Not Approve' this work order. You may leave a comment in the Comments box and then press the Update button. "
                Visible="False"></asp:Label></span><br />
        <span id="span2" style="font-family: Verdana"><strong>
            <br />
            <asp:Label ID="Label3" runat="server" Text="User:" Visible="False" style="position: relative"></asp:Label>
            <asp:TextBox ID="txtBxUser" runat="server" Style="position: relative" Width="99px" Visible="False"></asp:TextBox><br />
            <asp:Label ID="Label4" runat="server" Text="Password:" Visible="False" style="position: relative"></asp:Label></strong></span>
        <asp:TextBox ID="txtBxPassword" runat="server" TextMode="Password" Visible="False"
            Width="80px" style="position: relative"></asp:TextBox><br />
        <br />
        <span style="font-family: Verdana"><strong>
            <asp:CheckBox ID="chkBxApproval" runat="server" Text="Approval:" TextAlign="Left" Visible="False" />
            <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="10pt" ForeColor="Red"
                Text="(Checking this box is an indication of a 'YES'.)" Visible="False"></asp:Label><span id="span1" style="font-size: 10pt; color: red"><br />
                <asp:TextBox ID="txtBxComments" runat="server" BorderColor="Gray" BorderStyle="Solid"
                    Font-Names="Verdana" Font-Size="8pt" Height="88px" Width="1038px" TextMode="MultiLine" Visible="False"></asp:TextBox><br />
                <br />
                <asp:Button ID="btnUpdate" runat="server" Font-Size="Larger" Text="Update" Font-Bold="True" OnClick="btnUpdate_Click" Visible="False" /><br />
                <br />
            </span></strong></span>
</asp:Content>

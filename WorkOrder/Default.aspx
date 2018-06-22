<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Title="EBS Work Orders Module" Inherits="_Default" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <br class = "br_e" />
        <br class = "br_e" />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="10pt"
            Text="Review the list below or . . . "></asp:Label>
        <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Verdana" Font-Size="10pt"
            PostBackUrl="~/WorkOrder/CreateWO.aspx">Create a New Work Order</asp:LinkButton>&nbsp;
    
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
            SelectCommand="SELECT * FROM [WorkOrder]" ProviderName="<%$ ConnectionStrings:EBADB.ProviderName %>"></asp:SqlDataSource>
        
    </div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetWOListingByProj" TypeName="WorkOrderBLL">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlbProject" Name="proj" PropertyName="SelectedValue"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Font-Size="10pt" Text="Please choose a project . . ."></asp:Label>
        <asp:DropDownList ID="ddlbProject" runat="server" DataSourceID="ObjectDataSource2"
            DataTextField="code_desc" DataValueField="code_id" Font-Names="Verdana" Font-Size="10pt"
            Width="181px" AutoPostBack="True">
        </asp:DropDownList><br />
        <br />
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetProjectCodes" TypeName="CodesBLL"></asp:ObjectDataSource>
        &nbsp; &nbsp;
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" DataKeyNames="word_WOnum,Project" DataSourceID="ObjectDataSource1" Font-Names="Verdana"
            Font-Size="8pt" ForeColor="#333333" GridLines="None" Width="990px" AllowSorting="True" CaptionAlign="Top" Height="60px" HorizontalAlign="Justify">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="27px" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="word_WOnum,word_Proj" DataNavigateUrlFormatString="EditWO.aspx?word_WOnum={0}&amp;word_Proj={1}"
                    NavigateUrl="~/WorkOrder/EditWO.aspx" Text="Edit" >
                    <ItemStyle Width="30px" />
                </asp:HyperLinkField>
                <asp:CommandField ShowSelectButton="True" >
                    <ItemStyle Width="45px" />
                </asp:CommandField>
                <asp:BoundField DataField="word_WOnum" HeaderText="Work Order Number" ReadOnly="True"
                    SortExpression="word_WOnum" >
                    <ItemStyle HorizontalAlign="Center" Width="75px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="word_Title" HeaderText="Title" SortExpression="word_Title" >
                    <ItemStyle HorizontalAlign="Left" Width="500px" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="word_Date" DataFormatString="{0:d}" HeaderText="Create Date"
                    SortExpression="word_Date" >
                    <ItemStyle HorizontalAlign="Right" Width="45px" />
                </asp:BoundField>
                <asp:BoundField DataField="Phase" HeaderText="Phase" SortExpression="Phase">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="95px" />
                </asp:BoundField>
                <asp:BoundField DataField="Author" HeaderText="Author" ReadOnly="True" SortExpression="Author" >
                    <ItemStyle HorizontalAlign="Left" Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="word_reqDueDate" DataFormatString="{0:d}" HeaderText="Requested Due Date"
                    SortExpression="word_reqDueDate" >
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" Width="45px" />
                </asp:BoundField>
                <asp:BoundField DataField="word_Proj" SortExpression="word_Proj" Visible="False" />
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="Silver" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <EmptyDataTemplate>
                <span style="font-size: 16pt; border-left-color: gray; border-bottom-color: gray;
                    color: white; border-top-style: solid; border-top-color: gray; border-right-style: solid;
                    border-left-style: solid; background-color: #294775; border-right-color: gray;
                    border-bottom-style: solid">There are currently no Work Orders for this Project.</span>
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Verdana"
            Font-Size="11pt" ForeColor="Red" Height="43px" Text="Label" Width="872px" Visible="False"></asp:Label><br /><br />
        <div>
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="word_WOnum" DataSourceID="ObjectDataSourceA" Height="420px" Width="869px">
                <ItemTemplate>
                    <table style="width: 666px">
                        <tr>
                            <td style="height: 40px; width: 245px;" align="left" valign="middle">
                                <strong><span style="font-size: 8pt; font-family: Verdana">PM/SME:</span></strong>
                                <asp:Label ID="word_PMorSMELabel" runat="server" Text='<%# Eval("word_PMorSME") %>'
                                    Width="142px" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                            </td>
                            <td style="height: 40px; width: 298px;" align="left" valign="middle">
                                <strong><span style="font-size: 8pt; font-family: Verdana">Bsn Owner:</span></strong>
                                <asp:Label ID="word_BusnOwnerLabel" runat="server" Text='<%# Eval("word_BusnOwner") %>'
                                    Width="142px" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                            </td>
                            <td style="width: 234px; height: 40px"></td>
                        </tr>
                    
                        <tr>
                            <td style="height: 30px; width: 245px;" align="left">
                                <strong><span style="font-size: 8pt; font-family: Verdana">Priority:</span></strong>
                                <asp:Label ID="PriorityLabel" runat="server" Text='<%# Eval("word_Priority") %>'
                                    Width="142px" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                            </td>
                            <td style="height: 30px; width: 298px;" align="left">
                                <strong><span style="font-size: 8pt; font-family: Verdana">Phase:</span></strong>
                                <asp:Label ID="PhaseLabel" runat="server" Text='<%# Eval("word_Status") %>'
                                    Width="172px" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                            </td>
                            <td style="height: 30px; width: 234px;" align="left">
                                <strong><span style="font-size: 8pt; font-family: Verdana">Status:</span></strong>
                                <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("word_statFlag") %>'
                                    Width="142px" Font-Names="Verdana" Font-Size="8pt"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    
                    <strong><span style="font-size: 8pt; font-family: Verdana">Description:</span></strong><br />
                    <asp:Label ID="word_DescrLabel" runat="server" Text='<%# Bind("word_Descr") %>'
                        Height="93px" Width="846px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                    <strong><span style="font-size: 8pt; font-family: Verdana;";>Justification:</span></strong><br />
                    <asp:Label ID="word_JustifyLabel" runat="server" Text='<%# Bind("word_Justify") %>'
                        Height="93px" Width="847px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                    <strong><span style="font-size: 8pt; font-family: Verdana">Comments:</span></strong><br />
                    <asp:Label ID="word_CmntsLabel" runat="server" Text='<%# Bind("word_Cmnts") %>'
                        Height="93px" Width="847px" Font-Names="Verdana" Font-Size="8pt" BorderStyle="Solid" BorderWidth="1px"></asp:Label><br />
                    <br />
                    
                </ItemTemplate>
            </asp:FormView>
            <asp:ObjectDataSource ID="ObjectDataSourceA" runat="server" SelectMethod="GetFullTextWOs"
                TypeName="WorkOrderBLL" OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="GridView1" DefaultValue="" Name="wonum" PropertyName="SelectedValue"
                        Type="Int32" />
                    <asp:ControlParameter ControlID="ddlbProject" Name="woproj" PropertyName="SelectedValue"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>        
    
  

</asp:Content>
<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateWO.aspx.cs" Title="Create Work Order" Inherits="_Default" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>--%>
    <%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
    
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<head></head>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="WorkOrderBLL" SelectMethod="GetWorkOrders"
                InsertMethod="InsertWorkOrder" OnInserted="ObjectDataSource1_Inserted" OnInserting="ObjectDataSource1_Inserting" OldValuesParameterFormatString="original_{0}" UpdateMethod="UpdateWorkOrder">
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
                <UpdateParameters>
                    <asp:Parameter Name="word_WOnum" Type="Int32" />
                    <asp:Parameter Name="word_Proj" Type="String" />
                    <asp:Parameter Name="word_StatNum" Type="Int32" />
                    <asp:Parameter Name="word_Status" Type="String" />
                    <asp:Parameter Name="word_statFlag" Type="Int32" />
                    <asp:Parameter Name="word_Date" Type="DateTime" />
                    <asp:Parameter Name="word_Author" Type="String" />
                    <asp:Parameter Name="word_Title" Type="String" />
                    <asp:Parameter Name="word_Doc" Type="String" />
                    <asp:Parameter Name="word_DocVer" Type="String" />
                    <asp:Parameter Name="word_Priority" Type="Int32" />
                    <asp:Parameter Name="word_Descr" Type="String" />
                    <asp:Parameter Name="word_Justify" Type="String" />
                    <asp:Parameter Name="word_Cmnts" Type="String" />
                    <asp:Parameter Name="word_PMorSME" Type="Int32" />
                    <asp:Parameter Name="word_BusnOwner" Type="Int32" />
                    <asp:Parameter Name="word_reqDueDate" Type="DateTime" />
                </UpdateParameters>
            </asp:ObjectDataSource>
    
        
        
        <asp:Label ID="LblWarning" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Verdana"
            Font-Size="11pt" ForeColor="Red" Height="47px" Text="Label" Width="629px"></asp:Label>
        
        <div>
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="word_WOnum word_Proj" DataSourceID="ObjectDataSource1"
                DefaultMode="Insert" Height="591px" Width="596px" CellPadding="1" OnItemInserting="FormView1_ItemInserting" OnItemUpdated="FormView1_ItemUpdated" OnItemInserted="FormView1_ItemInserted" OnPreRender="FormView1_PreRender">
                <InsertItemTemplate>
                    <table style="font-size: 8pt; font-family: Verdana">
                        <tr style="height: 20px">
                            <td style="width: 307px; height: 17px; font-size: 8pt; font-family: Verdana">
                                <asp:Label ID="word_WOnumLabel" runat="server" Text='<%# Bind("word_WOnum") %>'
                                    Font-Names="Verdana" Font-Size="10pt" Width="90px" Visible="False"></asp:Label>
                            </td>
                            
                            <td style="width: 211px; height: 17px" align="right">
                                <asp:Label ID="word_DateLabel" runat="server" Text='<%# Bind("word_Date", "{0:d}") %>'
                                    Font-Names="Verdana" Font-Size="10pt" BorderStyle="None" Width="65px" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <span style="font-family: Verdana; font-size: 8pt"><strong>Title:</span></STRONG>
                    <asp:TextBox ID="word_TitleTextBox" runat="server" Text='<%# Bind("word_Title") %>'
                        Width="630px" Font-Bold="True" Font-Names="Verdana" Font-Size="10pt"></asp:TextBox><br />
                    <table>
                        <tr>
                            <td style="height: 20px; width: 227px;" align="left">
                                <span style="font-size: 8pt; font-family: Verdana">Project:</span>
                                <asp:DropDownList ID="DDDLproj" runat="server" DataSourceID="DDDLprojDS" DataTextField="code_desc"
                                    DataValueField="code_id" SelectedValue='<%# Bind("word_Proj") %>' Width="164px"
                                    Font-Names="Verdana" Font-Size="8pt" AppendDataBoundItems="True" Height="19px" CssClass="inputHra">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="height: 20px; width: 210px;" align="center">
                                <span style="font-size: 8pt; font-family: Verdana">Priority:</span>
                                <asp:DropDownList ID="DDDLpriority" runat="server" DataSourceID="DDDLpriorityDS"
                                    DataTextField="code_desc" DataValueField="code_id" SelectedValue='<%# Bind("word_Priority") %>'
                                    Width="101px" Font-Names="Verdana" Font-Size="8pt" AppendDataBoundItems="True" Height="19px">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="height: 20px; width: 119px;" align="left">
                                <span style="font-size: 8pt; font-family: Verdana; text-align:left">Due Date:</span><asp:TextBox ID="word_reqDueDateTextBox" runat="server" Text='<%# Bind("word_reqDueDate", "{0:d}") %>'
                                    Width="90px" Font-Names="Verdana" Font-Size="8pt" Visible="false" style="position: static;" Height="16px" ></asp:TextBox>
                                    
                                <%--<cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="word_reqDueDateTextBox">
                                </cc1:CalendarExtender>--%>
                                <asp:DateTimePicker ID="CalendarExtender1" runat="server" AutoFormat="Professional"   />
                                
                            </td>
                        </tr>
                        <tr>
                            
                            <td style="height: 30px; width: 227px;" align="center">
                                <span style="font-size: 8pt; font-family: Verdana">Phase:</span>
                                <asp:Label ID="LblPhase" runat="server" Text='<%# Bind("word_Status") %>' Width="153px"
                                    Font-Names="Verdana" Font-Size="8pt"  BorderWidth="1pt" BorderStyle="Solid" BorderColor="CornflowerBlue" Height="19px"></asp:Label>
                            </td>
                            <td style="height: 30px; width: 210px;" align="center">
                                <span style="font-size: 8pt; font-family: Verdana">Status:</span>
                                <asp:DropDownList ID="DropDownListPhaseStat" runat="server" Width="125px" Height="19px" Font-Names="Verdana"
                                    Font-Size="8pt" AppendDataBoundItems="True">
                                    <asp:ListItem Value="0">In Progress</asp:ListItem>
                                    <asp:ListItem Value="1">Completed</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="height: 30px; width: 119px;" align="left">
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("word_StatNum") %>'
                                Width="125px" Font-Bold="True" Font-Names="Verdana" Font-Size="10pt" Visible="false" BorderColor="LightSkyBlue"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 30px; width: 227px;" align="left" valign="middle">
                                <span style="font-size: 8pt; font-family: Verdana">Author:</span>
                                <asp:Label ID="LblAuthor" runat="server" Text='<%# Bind("word_Author") %>' Width="125px"
                                    Font-Names="Verdana" Font-Size="8pt" BorderWidth="1pt" BorderStyle="Solid" BorderColor="CornflowerBlue" Height="19px"></asp:Label>
                            <td style="height: 30px; width: 210px;" align="center" valign="middle">
                                <span style="font-size: 8pt; font-family: Verdana">PM/SME:</span>
                                <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ObjectDataSource2" DataTextField="FullName"
                                    DataValueField="wusr_uid" SelectedValue='<%# Bind("word_PMorSME") %>' Width="125px"
                                    Font-Names="Verdana" Font-Size="8pt" AppendDataBoundItems="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="height: 30px; width: 126px;" align="left" valign="middle">
                                <span style="font-size: 8pt; font-family: Verdana; text-align:left" >Bsn Owner:</span><asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="DDDLobjectDS" DataTextField="FullName"
                                    DataValueField="wusr_uid" SelectedValue='<%# Bind("word_BusnOwner") %>' Width="117px"
                                    Font-Names="Verdana" Font-Size="8pt" Height="19px" AppendDataBoundItems="True" style="position: static;">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                        <asp:ObjectDataSource ID="DDDLobjectDS" runat="server" SelectMethod="GetDDLusersByRole"
                            TypeName="woUsersBLL" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="BOWN" Name="role" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetDDLusersByRole"
                            TypeName="woUsersBLL" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="PM" Name="role" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetDDLusersByRole"
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
                            SelectMethod="GetAllPhaseStatus" TypeName="CodesBLL"></asp:ObjectDataSource>
                    <span style="font-size: 8pt; font-family: Verdana">Description:<br />
                    </span>
                    <asp:TextBox ID="word_DescrTextBox" runat="server" Text='<%# Bind("word_Descr") %>'
                        Height="93px" Width="630px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                    <span style="font-size: 8pt; font-family: Verdana">Justification:<br />
                    </span>
                    <asp:TextBox ID="word_JustifyTextBox" runat="server" Text='<%# Bind("word_Justify") %>'
                        Height="93px" Width="630px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                    <span style="font-size: 8pt; font-family: Verdana">Comments:<br />
                    </span>
                    <asp:TextBox ID="word_CmntsTextBox" runat="server" Text='<%# Bind("word_Cmnts") %>'
                        Height="93px" Width="629px" TextMode="MultiLine" Font-Names="Verdana" Font-Size="8pt"></asp:TextBox><br />
                    <br />
                    <asp:LinkButton ID="InsertButton" runat="server" CommandName="Insert"
                    Text="Insert" Font-Names="Verdana" Font-Size="8pt"></asp:LinkButton>
                    <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="Cancel" Font-Names="Verdana" Font-Size="8pt" PostBackUrl="~/WorkOrder/Default.aspx"></asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;
                    
                </InsertItemTemplate>
            </asp:FormView>

            &nbsp;&nbsp;<br />
            &nbsp;</div>
        


</asp:Content>


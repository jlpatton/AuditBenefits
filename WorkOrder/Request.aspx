<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Request.aspx.cs" Inherits="Request" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:FormView ID="FormView1" runat="server" DataKeyNames="word_WOnum" DataSourceID="SqlDtSrcFVrequest" style="position: static">
        <EditItemTemplate>
        <div class="Form1">
            word_WOnum:
            <asp:Label ID="word_WOnumLabel1" runat="server" Text='<%# Eval("word_WOnum") %>' style="width:500px; display:block;">
            </asp:Label><br />
            word_Proj:&nbsp;
            <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="" DataTextField="" DataValueField="" >
            </asp:DropDownList><br />
            word_StatNum:
            <asp:TextBox ID="word_StatNumTextBox" runat="server" Text='<%# Bind("word_StatNum") %>'>
            </asp:TextBox><br />
            word_Date:
            <asp:TextBox ID="word_DateTextBox" runat="server" Text='<%# Bind("word_Date") %>'>
            </asp:TextBox><br />
            word_Author:
            <asp:TextBox ID="word_AuthorTextBox" runat="server" Text='<%# Bind("word_Author") %>'>
            </asp:TextBox><br />
            word_Title:
            <asp:TextBox ID="word_TitleTextBox" runat="server" Text='<%# Bind("word_Title") %>'>
            </asp:TextBox><br />
            word_Doc:
            <asp:TextBox ID="word_DocTextBox" runat="server" Text='<%# Bind("word_Doc") %>'>
            </asp:TextBox><br />
            word_DocVer:
            <asp:TextBox ID="word_DocVerTextBox" runat="server" Text='<%# Bind("word_DocVer") %>'>
            </asp:TextBox><br />
            word_Priority:
            <asp:TextBox ID="word_PriorityTextBox" runat="server" Text='<%# Bind("word_Priority") %>'>
            </asp:TextBox><br />
            word_Descr:
            <asp:TextBox ID="word_DescrTextBox" runat="server" Text='<%# Bind("word_Descr") %>'>
            </asp:TextBox><br />
            word_Justify:
            <asp:TextBox ID="word_JustifyTextBox" runat="server" Text='<%# Bind("word_Justify") %>'>
            </asp:TextBox><br />
            word_Cmnts:
            <asp:TextBox ID="word_CmntsTextBox" runat="server" Text='<%# Bind("word_Cmnts") %>'>
            </asp:TextBox><br />
            word_PMorSME:
            <asp:TextBox ID="word_PMorSMETextBox" runat="server" Text='<%# Bind("word_PMorSME") %>'>
            </asp:TextBox><br />
            word_BusnOwner:
            <asp:TextBox ID="word_BusnOwnerTextBox" runat="server" Text='<%# Bind("word_BusnOwner") %>'>
            </asp:TextBox><br />
            word_reqDueDate:
            <asp:TextBox ID="word_reqDueDateTextBox" runat="server" Text='<%# Bind("word_reqDueDate") %>'>
            </asp:TextBox><br />
            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                Text="Update">
            </asp:LinkButton>
            <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                Text="Cancel">
            </asp:LinkButton>
            </div>
        </EditItemTemplate>
        <InsertItemTemplate>
            <div class="Form1">
            Work Order Number:
            <asp:TextBox ID="word_WOnumTextBox" runat="server" Text='<%# Bind("word_WOnum") %>'></asp:TextBox><br />
            Project:
            <asp:TextBox ID="word_ProjTextBox" runat="server" Text='<%# Bind("word_Proj") %>'></asp:TextBox>
            <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("word_Proj") %>'
                Style="z-index: 100; left: 277px; position: absolute; top: 168px">
            </asp:DropDownList>
            <br />
            StatusNum:
            <asp:TextBox ID="word_StatNumTextBox" runat="server" Text='<%# Bind("word_StatNum") %>'></asp:TextBox><br />
            Creation Date:
            <asp:TextBox ID="word_DateTextBox" runat="server" Text='<%# Bind("word_Date") %>'></asp:TextBox><br />
            Author:
            <asp:TextBox ID="word_AuthorTextBox" runat="server" Text='<%# Bind("word_Author") %>'></asp:TextBox><br />
            Title:
            <asp:TextBox ID="word_TitleTextBox" runat="server" Text='<%# Bind("word_Title") %>'></asp:TextBox><br />
            Priority:
            <asp:TextBox ID="word_PriorityTextBox" runat="server" Text='<%# Bind("word_Priority") %>'></asp:TextBox><br />
            Description:
            <asp:TextBox ID="word_DescrTextBox" runat="server" Text='<%# Bind("word_Descr") %>'></asp:TextBox><br />
            Justification:
            <asp:TextBox ID="word_JustifyTextBox" runat="server" Text='<%# Bind("word_Justify") %>'></asp:TextBox><br />
            Comments:
            <asp:TextBox ID="word_CmntsTextBox" runat="server" Text='<%# Bind("word_Cmnts") %>'></asp:TextBox><br />
            PM or SME:
            <asp:TextBox ID="word_PMorSMETextBox" runat="server" Text='<%# Bind("word_PMorSME") %>'></asp:TextBox><br />
            Business Owner:
            <asp:TextBox ID="word_BusnOwnerTextBox" runat="server" Text='<%# Bind("word_BusnOwner") %>'></asp:TextBox><br />
            Request Due Date:
            <asp:TextBox ID="word_reqDueDateTextBox" runat="server" Text='<%# Bind("word_reqDueDate") %>'></asp:TextBox><br />
            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                Text="Insert"></asp:LinkButton>
            <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                Text="Cancel"></asp:LinkButton>
            </div>    
        </InsertItemTemplate>
        <ItemTemplate>
        <div class="Form1"> 
            <strong>Work Order:</strong>
            <asp:Label ID="word_WOnumLabel" runat="server" Text='<%# Eval("word_WOnum") %>'></asp:Label><br />
            <strong>Project:</strong>
            <asp:Label ID="word_ProjLabel" runat="server" Text='<%# Bind("word_Proj") %>'></asp:Label><br />
            <strong>StatusNum:</strong>
            <asp:Label ID="word_StatNumLabel" runat="server" Text='<%# Bind("word_StatNum") %>'></asp:Label><br />
            <strong>Create Date:</strong>
            <asp:Label ID="word_DateLabel" runat="server" Text='<%# Bind("word_Date") %>'></asp:Label><br />
            <strong>Author:</strong>
            <asp:Label ID="word_AuthorLabel" runat="server" Text='<%# Bind("word_Author") %>'></asp:Label><br />
            <strong>Title:</strong>
            <asp:Label ID="word_TitleLabel" runat="server" Text='<%# Bind("word_Title") %>'></asp:Label><br />
            <strong>Priority:</strong>
            <asp:Label ID="word_PriorityLabel" runat="server" Text='<%# Bind("word_Priority") %>'></asp:Label><br />
            <strong>Description:</strong>
            <asp:Label ID="word_DescrLabel" runat="server" Text='<%# Bind("word_Descr") %>'></asp:Label><br />
            <strong>Justification:</strong>
            <asp:Label ID="word_JustifyLabel" runat="server" Text='<%# Bind("word_Justify") %>'></asp:Label><br />
            <strong>Comments:</strong>
            <asp:Label ID="word_CmntsLabel" runat="server" Text='<%# Bind("word_Cmnts") %>'></asp:Label><br />
            <strong>PM\SME:</strong>
            <asp:Label ID="word_PMorSMELabel" runat="server" Text='<%# Bind("word_PMorSME") %>'></asp:Label><br />
            <strong>Business Owner:</strong>
            <asp:Label ID="word_BusnOwnerLabel" runat="server" Text='<%# Bind("word_BusnOwner") %>'></asp:Label><br />
            <strong>Request Due Date:</strong>
            <asp:Label ID="word_reqDueDateLabel" runat="server" Text='<%# Bind("word_reqDueDate") %>'></asp:Label><br />
        </div>
        </ItemTemplate>
    </asp:FormView>
   
    <asp:SqlDataSource ID="SqlDtSrcFVrequest" runat="server" ConnectionString="<%$ ConnectionStrings:DEV EBAConnectionString %>"
        SelectCommand="GetWOrequest" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>


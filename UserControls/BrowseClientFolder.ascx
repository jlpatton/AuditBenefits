<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BrowseClientFolder.ascx.cs" Inherits="BrowseClientFolder" %>
<div style="display:block;">
    <asp:TextBox ID="text_path" runat="server"></asp:TextBox>    
    <asp:Button ID="browse_folder" runat="server" Text="Browse" OnClick="browse_folder_Click" />
    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
        ControlToValidate="text_path"
        Display="Dynamic">
        Select path
    </asp:RequiredFieldValidator>    
    <asp:RegularExpressionValidator 
        id="RegularExpressionValidator1" 
        runat="server"
        ControlToValidate="text_path"
        ValidationExpression="^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>|]*))+)$"
        Display="dynamic">
        Enter valid filepath
    </asp:RegularExpressionValidator>         
</div>
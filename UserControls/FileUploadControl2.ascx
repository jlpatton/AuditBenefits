<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUploadControl2.ascx.cs" Inherits="UserControls_FileUploadControl2" %>
<asp:Label ID="lblFile" runat="server" Text="File :" AssociatedControlID="FilUpl" >
</asp:Label>
<asp:FileUpload ID="FilUpl" runat="server" size="50"/>
<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                       ControlToValidate="FilUpl"
                       Display="Dynamic"
                       Font-Names="Verdana" Font-Size="10pt" 
                       >
                       Select file
                       </asp:RequiredFieldValidator> 
<asp:CustomValidator ID="ErrorMsg" runat="server" 
ErrorMessage="CustomValidator" 
OnServerValidate="ErrorMsg_ServerValidate">
</asp:CustomValidator>

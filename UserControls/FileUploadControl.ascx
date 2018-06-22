<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FileUploadControl.ascx.cs" Inherits="FileUploadControl" %>
<asp:Label ID="lblFile" runat="server" Text="File :" style="padding-left:22px">
</asp:Label>
<asp:FileUpload ID="FilUpl" runat="server" Width="360px" size="50"/>
<br />
<div style="margin-left:50px;margin-top:10px;width:600px;">
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
</div>


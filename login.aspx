<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="_Default" Title="Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id = "login">
        <asp:Login ID="Login1" runat="server" DisplayRememberMe="false" OnAuthenticate="l_onAuthenticate" >        
        </asp:Login>
        <div style="float:left;margin-left:0px;color:red;margin-top:10px;">
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
    </div>
    
</asp:Content>

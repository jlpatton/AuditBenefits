<%@ Page Language="C#" MasterPageFile="~/MasterPagelogout.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="AppAdmin.aspx.cs" Inherits="AppAdmin" Title="Application Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="float:left;width:940px;margin-top:20px">
        <asp:Button ID="btnEdit" runat="server" Text="Edit Settings" style="margin-left:30px" OnClick="btnEdit_Click" />
        <asp:Button ID="btnUpdate" runat="server" Text="Update Settings" style="margin-left:30px" Visible="false" OnClick="btnUpdate_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel Settings" style="margin-left:30px" Visible="false" OnClick="btnCancel_Click" />
        <br /><br />        
        <asp:Label ID="lblConn" runat="server" Text="Connection String for Database EBADB:" style="font-family:Tahoma;font-weight:bold;margin-left:30px"></asp:Label>
        <asp:TextBox ID="txtConn" runat="server" ReadOnly="True" TextMode="MultiLine" Width="100%" Height="30px" style="margin-left:30px"></asp:TextBox>        
    </div>    
    <div style="float:left;width:940px;margin-top:20px">
        <asp:Button ID="btnEncrypt" runat="server" Text="Encrypt" style="margin-left:300px" OnClick="btnEncrypt_Click" />
        <asp:Button ID="btnDecrypt" runat="server" Text="Decrypt" style="margin-left:150px" OnClick="btnDecrypt_Click" />
    </div>
    <div style="float:left;width:940px;margin-top:20px;margin-left:10px;">
        <strong>
            <span style="font-size: 10pt">
                <span style="font-family: Tahoma">
                    <span style="color: #0099ff;text-decoration: underline">Web.Config<br />
                            <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" Height="374px" ReadOnly="True"
                            TextMode="MultiLine" Width="100%" Wrap="False" BackColor="#E0E0E0" EnableViewState="False" ForeColor="Blue">
                            </asp:TextBox>
                    </span>
                </span>
            </span>
        </strong>
    </div>    
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NoAccess.aspx.cs" Inherits="NoAccess" Title="Access Denied" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contentleft">     
        <div id = "verticalmenu">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/side_tab_up.gif"/>
            <ul>
                <li><asp:HyperLink ID="hypHome" runat="server" Font-Bold="false" NavigateUrl="~/Home.aspx" >HOME</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWA" runat="server" Font-Bold="false" NavigateUrl="~/VWA/Home.aspx">VWA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="~/HRA/Home.aspx" >HRA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypANTHEM" runat="server" Font-Bold="false" NavigateUrl="~/Anthem/Home.aspx">ANTHEM</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypIPBA" runat="server" Font-Bold="false" NavigateUrl="~/IPBA/Home.aspx">IPBA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypIMPUTED" runat="server" Font-Bold="false" NavigateUrl="~/ImputedIncome/Home.aspx">IMPUTED</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypYEB" runat="server" Font-Bold="false" NavigateUrl="~/YEB/Home.aspx">YEB</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypWorkOrder" runat="server" Font-Bold="false" NavigateUrl="~/WorkOrder/default.aspx">WORK ORDER</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypEBADSKTP" runat="server" Font-Bold="false" NavigateUrl="~/DsktpHist/Home.aspx">EBA Desktop</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypADMIN" runat="server" Font-Bold="false" NavigateUrl="~/Admin/console.aspx">ADMIN</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAudit" runat="server" Font-Bold="false" NavigateUrl="">AUDIT</asp:HyperLink></li></ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />  
        </div>
                 
    </div>
    <div id="contentright">
        <div style="margin:20px;color:Red"><h3>You do not have privileges to access the requested page!</h3></div>                
     </div>
</asp:Content>


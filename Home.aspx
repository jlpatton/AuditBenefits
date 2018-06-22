<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="_Default" Title="EBA Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "verticalmenu">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/side_tab_up.gif"/>
            <ul>
                <li><asp:HyperLink ID="hypHome" runat="server" Font-Bold="false" NavigateUrl="~/Home.aspx" >HOME</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWA" runat="server" Font-Bold="false" NavigateUrl="~/VWA/Home.aspx">VWA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="~/HRA/Home.aspx" >HRA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAnthem" runat="server" Font-Bold="false" NavigateUrl="~/Anthem/Home.aspx">ANTHEM</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="false" NavigateUrl="~/IPBA/Home.aspx">IPBA</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink4" runat="server" Font-Bold="false" NavigateUrl="~/ImputedIncome/Home.aspx">IMPUTED</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink6" runat="server" Font-Bold="false" NavigateUrl="~/YEB/Home.aspx">YEB</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypWorkOrder" runat="server" Font-Bold="false" NavigateUrl="~/WorkOrder/default.aspx">WORK ORDER</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink7" runat="server" Font-Bold="false" NavigateUrl="~/DsktpHist/Home.aspx">EBA Desktop</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="false" NavigateUrl="~/Admin/console.aspx">ADMIN</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAudit" runat="server" Font-Bold="false" NavigateUrl="~/Audit/Home.aspx">AUDIT</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="false" NavigateUrl="~/Reports/Output_Report.aspx">OUTPUT</asp:HyperLink></li></ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />  
        </div>
              
    </div>
    <div id="contentright">
        <div id = "introText">Welcome to EBA</div>                
     </div>
</asp:Content>


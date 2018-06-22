<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Denied.aspx.cs" Inherits="DsktpHist_Denied" Title="Denied" %>

<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentleft">
        <div id="module_menu">
            <h1>
                EBA Desktop Data</h1>
            <ul id="sitemap">
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="hypTowerGroup" runat="server" NavigateUrl="">Tower Group</asp:HyperLink>
                </li>
                <li>
                    <ul id="expand1">
                        <li>
                            <asp:HyperLink ID="hypTowerEmps" runat="server" >Employees</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hypSpDeps" runat="server" NavigateUrl="~/DsktpHist/SpDependents.aspx">Sponsored Dependents</asp:HyperLink></li>
                    </ul>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/DsktpHist/RetPart.aspx">Retiree Health Data</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/DsktpHist/HipCert.aspx">HIPAA Certificates</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/DsktpHist/EBAworkorder.aspx">Work Orders</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/DsktpHist/TermFiles.aspx">Term Files</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/DsktpHist/SecurityLog.aspx">Security Log</asp:HyperLink>
                </li>
                <li style="left: 0px; top: 0px">
                    <asp:HyperLink ID="HyperLink8" runat="server" NavigateUrl="~/DsktpHist/PCaudit.aspx">PC Audit Data</asp:HyperLink>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />
        </div>
    </div>
    <div id="contentright">
   <div style="margin:20px;color:Red"><h3>You dont have Privileges to access the requested page!</h3></div>                
    </div>
</asp:Content>
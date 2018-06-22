<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Audit_Home" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contentleft">     
        <div id = "verticalmenu">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/side_tab_up.gif"/>
        <ul>
                <li><asp:HyperLink ID="hypHome" runat="server" Font-Bold="false" NavigateUrl="~/Home.aspx" >HOME</asp:HyperLink></li>
                <%--<li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="" >HRA</asp:HyperLink></li>--%>
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
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Logins" Value="0" Selected = "true"></asp:MenuItem>                    
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                <div id = "EmpListDiv">
                    <div id = "introText">User Logins</div>
                </div> 
                <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                    <asp:GridView ID="grdLogins" runat="server" CssClass="tablestyle" AllowSorting="True" 
                        PagerSettings-Mode="NextPreviousFirstLast" AutoGenerateColumns="true" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif" 
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif" 
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" DataSourceID="SqlDataSource1">                
                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />                           
                        </asp:GridView>   
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                        SelectCommand="SELECT [UserID] AS [User ID], [UserName] AS [User Name], [SessionStart], [SessionEnd] FROM [UserSession_AU]">
                    </asp:SqlDataSource>
                </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>


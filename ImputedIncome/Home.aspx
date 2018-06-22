<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="ImputedIncome_Home" Title="Imputed Income Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div id="contentleft">       
        <div id="Div1">        
            <div id = "module_menu">
                <h1>Imputed Income</h1>            
                <ul id="sitemap">
                    <li><asp:HyperLink ID="hypCalc" runat="server" NavigateUrl="~/ImputedIncome/Calculate.aspx">Calculate</asp:HyperLink></li>
                    <li><asp:HyperLink ID="hypMain" runat="server" NavigateUrl="~/ImputedIncome/Maintainence.aspx">Maintainence</asp:HyperLink></li>
                </ul>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
            </div> 	
        </div>          
    </div>
    <div id="contentright">
        <div id = "introText">Pilot Imputed Income Calculation</div>                
    </div>
</asp:Content>


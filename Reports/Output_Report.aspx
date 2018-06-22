<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Output_Report.aspx.cs" Inherits="HRA_Reports_Default" Title="ITG/WW Import Output Reports" %>

<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div id="contentleft">     
        <div id = "verticalmenu">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/styles/images/side_tab_up.gif"/>
            <ul>
                <li><asp:HyperLink ID="hypHome" runat="server" Font-Bold="false" NavigateUrl="~/Home.aspx" >HOME</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypHRA" runat="server" Font-Bold="false" NavigateUrl="~/HRA/Home.aspx" >HRA</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAnthem" runat="server" Font-Bold="false" NavigateUrl="~/Anthem/Home.aspx">ANTHEM</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="false" NavigateUrl="~/IPBA/Home.aspx">IPBA</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="false" NavigateUrl="~/Admin/console.aspx">ADMIN</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypAudit" runat="server" Font-Bold="false" NavigateUrl="~/Audit/Home.aspx">AUDIT</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="false" NavigateUrl="~/Reports/Output_Report.aspx">OUTPUT</asp:HyperLink></li></ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />  
        </div>              
    </div>
	<div id="contentright">
	    <div id= "introText"> 
            <asp:Label ID="lblSubheading2" runat="server" Text="Reports" ForeColor="#5D478B"></asp:Label>
        </div>
        <div style="width:680px;float:left;">
            <table>
                <tr> 
                    <td style="padding-top:10px;padding-left:10px;">
                       <p>Select Start Date:</p>
                        <asp:DateTimePicker ID="DateTimePicker1" runat="server" AutoFormat="Professional"   />
                    </td> 
                     <td style="padding-top:10px;padding-left:10px;">
                        <asp:Label ID="Label1" runat="server" Text="(Required)" Visible="false" ForeColor="red"></asp:Label>
                    </td> 
                    <td style="padding-top:10px;padding-left:10px;">
                       <p>Select End Date:</p>
                        <asp:DateTimePicker ID="DateTimePicker2" runat="server" AutoFormat="Professional"   />
                    </td> 
                    <td style="padding-top:10px;padding-left:10px;">
                        <asp:Label ID="Label2" runat="server" Text="(Required)" Visible="false" ForeColor="red"></asp:Label>
                    </td>                        
                    <td style="padding-top:10px;padding-left:20px;">
                        <asp:LinkButton ID="lnk_genTranRpt" runat="server" OnClick="lnk_genRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;">
            
        </div> 
        <div class="error" id="errorDiv1" runat="server" visible="false">
            <asp:Label ID="lbl_error1" runat="server" Text=""></asp:Label>
        </div>
	</div>
</asp:Content>


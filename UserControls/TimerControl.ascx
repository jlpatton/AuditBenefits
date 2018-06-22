<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TimerControl.ascx.cs" Inherits="UserControls_TimerControl" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" RenderMode="Inline">    
    <ContentTemplate>
        <asp:Timer ID="TimerTimeout" runat="server" Interval="10" OnTick="TimerTimout_Tick" /> 
    </ContentTemplate>    
</asp:UpdatePanel>
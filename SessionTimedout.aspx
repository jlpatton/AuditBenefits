<%@ Page Language="C#" MasterPageFile="~/MasterPagelogout.master" AutoEventWireup="true" CodeFile="SessionTimedout.aspx.cs" Inherits="_Default" Title="Session Expired" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id = "logout">
        <h2>Session Expired</h2>
        <p>
            Your session has expired at <%= DateTime.Now.ToLongTimeString()%>  
            Please <a href="Home.aspx">return to the home page</a> 
            and log in again to continue accessing the application.
        </p>
    </div>
</asp:Content>


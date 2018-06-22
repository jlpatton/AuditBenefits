<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Anthem_Maintenance_Dashboard" Title="User Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>ANTHEM</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Billing</a>
		            <ul>
		                <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/Anthem/Billing/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypBillingRecon" runat="server" NavigateUrl="~/Anthem/Billing/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
		            </ul>
		        </li>
                <li><a>Claims</a>
                     <ul>
		                <li><asp:HyperLink ID="hypClaimsImp" runat="server" NavigateUrl="~/Anthem/Claims/Imports.aspx">Import</asp:HyperLink></li>
		                <li><asp:HyperLink ID="HypClaimsRecon" runat="server" NavigateUrl="~/Anthem/Claims/Reconciliation.aspx">Reconciliation</asp:HyperLink></li>
		            </ul>
                </li>
                <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Anthem/Reports/Reports.aspx">Reports</asp:Hyperlink></li>
                <li><a>Maintenance</a>
                    <ul id = "expand1">
		                <li><a>Module Maintenance</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypCodes" runat="server" NavigateUrl="~/Anthem/Maintenance/CodesTable.aspx">Codes Table</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypConsole" runat="server" NavigateUrl="~/Anthem/Maintenance/ManagementConsole.aspx">Management Console</asp:HyperLink></li>		                        
		                    </ul>
		                </li>                        
                        <li><a>Current Users</a>
                            <ul id = "expand2">
                                <li><asp:HyperLink ID="hypDash" Font-Underline="true" runat="server" NavigateUrl="~/Anthem/Maintenance/Dashboard.aspx">Dashboard</asp:HyperLink></li>
                                <li><asp:HyperLink ID="hypPref" runat="server" NavigateUrl="~/Anthem/Maintenance/Preferences.aspx">Preferences</asp:HyperLink></li>
                            </ul>
                        </li>		            
		            </ul>
		       </li>
		    </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
    <div id="contentright">
        <div id = "introText">User Dashboard</div>  
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Reconciliation status" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="" Value="1"></asp:MenuItem>                    
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                    <div class="userPara">
                        <p>Following shows the status of Files Imported and Reconciliation Under each Anthem module</p>    
                    </div> 
                     <div id = "selectYRMO">
                    <asp:Label ID="lblYrmo" runat="server" Text="Period:"></asp:Label>
                    <asp:DropDownList ID="ddlMonths" runat="server">
                        <asp:ListItem Value = "01">January</asp:ListItem>
                        <asp:ListItem Value = "02">February</asp:ListItem>
                        <asp:ListItem Value = "03">March</asp:ListItem>
                        <asp:ListItem Value = "04">April</asp:ListItem>
                        <asp:ListItem Value = "05">May</asp:ListItem>
                        <asp:ListItem Value = "06">June</asp:ListItem>
                        <asp:ListItem Value = "07">July</asp:ListItem>
                        <asp:ListItem Value = "08">August</asp:ListItem>
                        <asp:ListItem Value = "09">September</asp:ListItem>
                        <asp:ListItem Value = "10">October</asp:ListItem>
                        <asp:ListItem Value = "11">November</asp:ListItem>
                        <asp:ListItem Value = "12">December</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtYear" runat="server" width="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtYear" ErrorMessage="*"></asp:RequiredFieldValidator> 
                    <asp:Label ID="lblHint" runat="server" Text="(yyyy)" Font-Italic="true"></asp:Label>
                    <asp:Label ID="lblModule" runat="server" Text="Select Module:" style="margin-left:20px;"></asp:Label>
                    <asp:DropDownList ID="ddlModule" runat="server">
                        <asp:ListItem Value = "Not Selected"></asp:ListItem>
                        <asp:ListItem Value = "Dom">Billing - Domestic</asp:ListItem>
                        <asp:ListItem Value = "Intl">Billing - International</asp:ListItem>
                        <asp:ListItem Value = "EAP">Billing - EAP </asp:ListItem>
                        <asp:ListItem Value = "CA">Claims - California BOA/Claims</asp:ListItem>
                        <asp:ListItem Value = "NCARFDF">Claims - Non California RFDF/Claims</asp:ListItem>
                        <asp:ListItem Value = "RX">Claims - Pharmacy</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" style="margin-left:15px" OnClick="btnSubmit_Click"/>
                </div>                
                <div id = "progressBar">
                    <div id="Dom" runat = "server" style="float:left;width:750px">
                        <br />
                        <h4>Domestic Billing</h4>
                        <br />
                        <ul id="mainNav" class="SixStep">
                            <li id = "l1Grs" runat = "server"><a title=""><em>Import:</em><span>GRS Active Pilot</span></a></li>
                            <li id = "l1ADP" runat = "server"><a title=""><em>Import:</em><span>ADP Cobra</span></a></li>
                            <li id = "l1RetireeMC" runat = "server"><a title=""><em>Import:</em><span>Retiree Medicare</span></a></li>
                            <li id = "l1RetireeNM" runat = "server"><a title=""><em>Import:</em><span>Retiree Non-Medicare</span></a></li>
                            <li id = "l1AnthemBill" runat = "server"><a title=""><em>Import:</em><span>Anthem Bill</span></a></li>                         
                            <li id = "l1Recon" runat = "server" class="mainNavNoBg"><a title=""><em>Reconciliation</em><span></span></a></li>
                        </ul>
                    </div>
                    <div id="Intl" runat = "server" style="float:left;width:750px">
                        <br />
                        <h4>International Billing</h4>
                        <br />
                        <ul id="mainNav" class="TwoStep">
                          <li id = "l2IntlHeadcount" runat = "server"><a title=""><em>Import:</em><span>HTH Details File</span></a></li>
                          <li id = "l2Recon" runat = "server" class="mainNavNoBg"><a title=""><em></em><span>Reconciliation</span></a></li>
                        </ul>
                    </div> 
                    <div id="EAP" runat = "server" style="float:left;width:750px">
                        <br />
                        <h4>EAP Reconciliation</h4>
                        <br />
                        <ul id="mainNav" class="SevenStep">
                            <li id = "L7GRS" runat = "server"><a title=""><em>Import:</em><span>GRS Active Pilot</span></a></li>
                            <li id = "L7ADP" runat = "server"><a title=""><em>Import:</em><span>ADP Cobra</span></a></li>
                            <li id = "L7HTH" runat = "server"><a title=""><em>Import:</em><span>HTH Details File</span></a></li>
                            <li id = "L7MC" runat = "server"><a title=""><em>Import:</em><span>Retiree Medicare</span></a></li>
                            <li id = "L7NM" runat = "server"><a title=""><em>Import:</em><span>Retiree Non-Medicare</span></a></li>
                            <li id = "L7EAP" runat = "server"><a title=""><em>Import:</em><span>Anthem Bill EAP</span></a></li>                         
                            <li id = "L7Recon" runat = "server" class="mainNavNoBg"><a title=""><em>Reconciliation</em><span></span></a></li>
                        </ul>
                    </div>
                    <div id="CA" runat = "server" style="float:left;width:750px">
                        <br />
                        <h4>CA Claims</h4>
                        <br />
                        <ul id="mainNav" class="ThreeStep">
                            <li id = "l3BOA" runat = "server"><a title=""><em>Import:</em><span>BOA bank Statement</span></a></li>
                            <li id = "l3CA" runat = "server"><a title=""><em>Import:</em><span>CA Claims Report</span></a></li>
                            <li id = "l3Recon" runat = "server" class="mainNavNoBg"><a title=""><em></em><span>CA Reconciliation</span></a></li>
                        </ul>
                    </div>                     
                    <div id="NCA" runat = "server" style="float:left;width:750px">
                        <br />
                        <h4>Non CA RF/DF Claims</h4>
                        <br />
                        <ul id="mainNav" class="FourStep">
                          <li id = "l5RF" runat = "server"><a title=""><em>Import:</em><span>RF/DF Claims</span></a></li>
                          <li id = "L5DF" runat = "server"><a title=""><em>Import:</em><span>DF no RF Claims</span></a></li>
                          <li id = "L5NCA" runat = "server"><a title=""><em>Import:</em><span>Non CA Claims Report</span></a></li>
                          <li id = "L5Recon" runat = "server" class="mainNavNoBg"><a title=""><em></em><span>RFDF Reconciliation</span></a></li>
                        </ul>
                    </div> 
                    <div id="RX" runat="server" style="float:left;width:750px">
                        <br />
                        <h4>Pharmacy Claims</h4>
                        <br />
                        <ul id="mainNav" class="TwoStep"> 
                              <li id = "L6Pharmacy" runat = "server"><a title=""><em>Import:</em><span>Pharmacy Paid claims</span></a></li>                          
                              <li id = "L6Recon" runat = "server" class="mainNavNoBg"><a title=""><em></em><span>RX Reconciliation</span></a></li>
                        </ul>
                    </div>                                             
                </div>                
                <div id = "stepFinal" runat = "server" style="float:left;margin-top:20px;margin-left:100px;">
                    <div id = "box1">
                    </div>
                    <div id = "lab1">
                        Completed
                    </div>
                    <div id = "box2">
                    </div>
                    <div id = "lab2">
                        Not Completed
                    </div>                    
                </div>
                </asp:View>
            </asp:MultiView>
        </div>                        
    </div>
</asp:Content>


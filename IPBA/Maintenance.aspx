<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Maintenance.aspx.cs" Inherits="IPBA_Maintenance" Title="Plan Codes and Rates Maintenance" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>IPBA</h1> 		    
		    <ul id="sitemap">		               	      
		        <li><a>Import</a>
		            <ul>
		                <li><asp:HyperLink ID="hypGreenbarImp" runat="server" NavigateUrl="~/IPBA/Import_GreenBar_Report.aspx">Greenbar Report</asp:HyperLink></li>
		                <li><asp:HyperLink ID="hypADPImp" runat="server" NavigateUrl="~/IPBA/Import_ADP_Cobra.aspx">ADP Cobra Report</asp:HyperLink></li>
		            </ul>
		        </li>		         
                <li><asp:HyperLink ID="hypManAdj" runat="server" NavigateUrl="~/IPBA/HMO_and_HTH_Adjustments.aspx">Billing Adjustments</asp:HyperLink></li>		                
                <li><asp:HyperLink ID="hypHMOReport" runat="server" NavigateUrl="~/IPBA/HTH_HMO_Billing_Report.aspx">HTH/HMO Billing Report</asp:HyperLink></li> 
                <li><asp:HyperLink ID="hypMaintenance" runat="server" Font-Underline="true" NavigateUrl="~/IPBA/Maintenance.aspx">Maintenance</asp:HyperLink></li>          		                     		            
            </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
     <div id="contentright">
        <div id = "introText">HMO Plan codes and Rates table.</div> 
        <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Benefit Hierarchy" Value="0" Selected = "true"></asp:MenuItem> 
                    <asp:MenuItem Text="Rates" Value="1" ></asp:MenuItem>   
                    <asp:MenuItem Text="Import/Export Reports" Value="2" ></asp:MenuItem>    
                    <asp:MenuItem Text="Audit" Value="3" ></asp:MenuItem>                                              
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <div class="userPara" runat="server" >
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEffYrmo" runat="server" Text="Eff. YRMO:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEffYrmo" runat="server" 
                                        AppendDataBoundItems="true" 
                                        DataTextField="ph_yrmo" DataSourceID="SqlDataSource1">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqval1" 
                                        ControlToValidate="ddlEffYrmo"
                                        runat="server" 
                                        Text="(Required)"
                                        ValidationGroup="q1"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>                                    
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT ph_yrmo FROM [Planhier]">
                                    </asp:SqlDataSource>
                                </td>
                                <td>
                                    <asp:Label ID="lblProgcd" runat="server" Text="Program Code:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProgCd" runat="server" AppendDataBoundItems="true"
                                    DataTextField="bnhr_progcd" DataValueField = "bnhr_id" Width="80px"
                                    DataSourceID="SqlDataSource2">
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                        ControlToValidate="ddlProgCd"
                                        runat="server" 
                                        Text="(Required)"
                                        ValidationGroup="q1"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>                                    
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [bnhr_progcd],[bnhr_id] FROM [Benefithier]">
                                    </asp:SqlDataSource>
                                </td>                                
                                <td>
                                    <asp:Button ID="btnQuery" runat="server" ValidationGroup="q1" Text="Query" OnClick="btnQuery_Click" />
                                </td>                               
                            </tr>
                        </table>
                    </div>
                    <div id= "subIntroText"> 
                        <asp:Label ID="lblHeading" runat="server" Text="Benefit Hierarchy Information" ForeColor="#5D478B"></asp:Label>
	                </div>
	                <asp:UpdatePanel runat="server" ID="updatePanel" UpdateMode="always">
				    <ContentTemplate>
                    <div class="scroller" style ="width:700px; max-height:400px;" id = "grddivscroll" runat="server">
                    <asp:GridView runat="server" ID="grdvBen" OnRowEditing="grdvBen_RowEditing" ShowFooter="True"
                        OnRowCancelingEdit="grdvBen_RowCancelingEdit" AutoGenerateColumns="False"
                        CssClass="tablestyle" AllowSorting="True" OnSorting="grdvBen_Sorting" 
                        DataKeyNames="ph_id" OnRowUpdating="UpdateRecordBen" OnRowDeleting="DeleteRecordBen"
                        AllowPaging="False" PagerSettings-Mode="NumericFirstLast"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"
                        OnPageIndexChanging="grdvBen_PageIndexChanging">                        
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" 
                                    CommandName="Edit" Text="Edit" Width="50px" ></asp:LinkButton>
                                    <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Delete"
                                    OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbUpdate" runat="server" CausesValidation="True" Width="50px"
                                    CommandName="Update" Text="Update" ></asp:LinkButton> 
                                    <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
                                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Width="50px"
                                    ValidationGroup="i1" OnClick="Button1_Click">Insert</asp:LinkButton> 
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" Width="50px"
                                    CommandName="Cancel" Text="Cancel" 
                                    OnClick="CancelButton1_Click"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ph_id" Visible = "false" />                             
                            <asp:TemplateField HeaderText="Group" SortExpression="ph_plangrp">                                
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGrp" runat="server" SelectedValue='<%# Bind("ph_plangrp") %>'
                                    DataTextField="ph_plangrp" DataSourceID="SqlDataSource4" Width="80px">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGrp" runat="server" 
                                    Text='<%# Bind("ph_plangrp") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlGrpA" runat="server" Width="80px" AppendDataBoundItems="true"
                                     DataTextField="ph_plangrp" DataSourceID="SqlDataSource4">
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvGrpA" 
                                        ControlToValidate="ddlGrpA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="i1"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Plan Code" SortExpression="ph_plancd">                                
                                <EditItemTemplate>                              
                                    <asp:TextBox ID="txtPlancd" runat="server" Text='<%# Bind("ph_plancd") %>' Width="80px"></asp:TextBox> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPlancd" runat="server" 
                                    Text='<%# Bind("ph_plancd") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>                             
                                    <asp:TextBox ID="txtPlancdA" runat="server" Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvPlanA" 
                                        ControlToValidate="txtPlancdA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="i1">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" SortExpression="ph_plandesc">                                 
                                <EditItemTemplate>                               
                                    <asp:TextBox ID="txtDesc" runat="server" Text='<%# Bind("ph_plandesc") %>' Width="250px"></asp:TextBox> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDesc" runat="server" 
                                    Text='<%# Bind("ph_plandesc") %>' Width="250px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtDescA" runat="server" Width="250px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvDescA" 
                                        ControlToValidate="txtDescA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="i1">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tiercode" SortExpression="ph_tiercd">                                
                                <EditItemTemplate>                               
                                    <asp:DropDownList ID="ddlTier" runat="server" SelectedValue='<%# Bind("ph_tiercd") %>'
                                    DataTextField="ph_tiercd" DataSourceID="SqlDataSource5" Width="80px">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTier" runat="server" Width="80px"
                                    Text='<%# Bind("ph_tiercd") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlTierA" runat="server" Width="80px" AppendDataBoundItems="true"
                                    DataTextField="ph_tiercd" DataSourceID="SqlDataSource5">
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvtierA" 
                                        ControlToValidate="ddlTierA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="i1"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Eff. YRMO" SortExpression="ph_yrmo">                                 
                                <EditItemTemplate>                               
                                    <asp:TextBox ID="txtYrmo" runat="server" Text='<%# Bind("ph_yrmo") %>' Width="80px"></asp:TextBox> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblYrmo" runat="server" Width="80px"
                                    Text='<%# Bind("ph_yrmo") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtYrmoA" runat="server" Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvYrmoA" 
                                        ControlToValidate="txtYrmoA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="i1">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>                           
                        </Columns>
                    </asp:GridView> 
                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                    ConnectionString="<%$ ConnectionStrings:EBADB %>"
                    SelectCommand="SELECT codeid as [ph_plangrp] FROM [Codes] WHERE [source] = 'plangrp' 
                    AND codeid IN ('LMO', 'INT')" >
                    </asp:SqlDataSource>                               
                    <asp:SqlDataSource ID="SqlDataSource5" runat="server"
                    ConnectionString="<%$ ConnectionStrings:EBADB %>"
                    SelectCommand="SELECT codeid as [ph_tiercd] FROM [Codes] WHERE [source] = 'tiercd' AND codeid IN ('E','C','S','F')" >
                    </asp:SqlDataSource>                    
                    </div>
                     <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                        <asp:Label ID="lblErrBen" runat="server" Text="" ForeColor="red" style="margin-left:20px;"></asp:Label>
                    </div>
                    <ajaxscroll:PersistentScrollPosition runat="server" ControlToPersist="grddivscroll" />
                    </ContentTemplate>
                    </asp:UpdatePanel>                               
                </asp:View>
                <asp:View ID="View2" runat="server">
                <div id="Div1" class="userPara" runat="server" >
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRpy" runat="server" Text="Plan Yr:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRpy" runat="server" AppendDataBoundItems="true"
                                        DataTextField="rate_py" DataSourceID="SqlDataSource9" 
                                        AutoPostBack="true" OnSelectedIndexChanged = "ddlRpy_onSelectedindexchange">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" 
                                        ControlToValidate="ddlRpy"
                                        runat="server" 
                                        Text="(Required)"
                                        ValidationGroup="q2"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>                                    
                                    <asp:SqlDataSource ID="SqlDataSource9" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT rate_py FROM [Rates]">
                                    </asp:SqlDataSource>
                                </td>
                                <td>
                                    <asp:Label ID="lblREYrmo" runat="server" Text="Eff. YRMO:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlREffYrmo" runat="server">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" 
                                        ControlToValidate="ddlREffYrmo"
                                        runat="server" 
                                        Text="(Required)"
                                        ValidationGroup="q2"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator> 
                                </td>
                                <td>
                                    <asp:Label ID="lblRProgcd" runat="server" Text="Program Code:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlRProgcd" runat="server" AppendDataBoundItems="true"
                                    DataTextField="ph_progmcd" Width="80px"
                                    DataSourceID="SqlDataSource7">
                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" 
                                        ControlToValidate="ddlRProgcd"
                                        runat="server" 
                                        Text="(Required)"
                                        ValidationGroup="q2"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>                                    
                                    <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [ph_progmcd] FROM [Planhier]">
                                    </asp:SqlDataSource>
                                </td>                               
                                <td>
                                    <asp:Button ID="btnQueryRate" runat="server" ValidationGroup="q2" Text="Query" OnClick="btnQueryRate_Click" />
                                </td>                               
                            </tr>
                        </table>
                    </div>
                    <div id= "subIntroText"> 
                        <asp:Label ID="lblRateHeading" runat="server" Text="Rates Information" ForeColor="#5D478B"></asp:Label>
	                </div>
	                <asp:UpdatePanel runat="server" ID="updatePanel1" UpdateMode="always">
				    <ContentTemplate>
	                <div class="scroller" style ="width:700px; max-height:400px;" id = "grdvRatescroll" runat="server">
                    <asp:GridView runat="server" ID="grdvRates" OnRowEditing="grdvRates_RowEditing" ShowFooter="True"
                        OnRowCancelingEdit="grdvRates_RowCancelingEdit" AutoGenerateColumns="False"
                        CssClass="tablestyle" AllowSorting="True" OnSorting="grdvRates_Sorting"
                        AllowPaging="False" PagerSettings-Mode="NumericFirstLast"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"
                        OnPageIndexChanging="grdvRates_PageIndexChanging"
                        DataKeyNames="rate_id" OnRowUpdating="UpdateRecordRates" OnRowDeleting="DeleteRecordRates">                        
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="False" 
                                    CommandName="Edit" Text="Edit" Width="50px" ></asp:LinkButton>
                                    <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" 
                                    CommandName="Delete" Text="Delete"
                                    OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lbUpdate" runat="server" CausesValidation="True" Width="50px"
                                    CommandName="Update" Text="Update" ></asp:LinkButton> 
                                    <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
                                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Width="50px"
                                    ValidationGroup="ir" OnClick="Button1R_Click">Insert</asp:LinkButton> 
                                    <asp:LinkButton ID="lbCancelI" runat="server" CausesValidation="False" Width="50px"
                                    CommandName="Cancel" Text="Cancel" 
                                    OnClick="CancelButton1R_Click"></asp:LinkButton>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:BoundField DataField="rate_id" Visible = "false" /> 
                            <asp:TemplateField HeaderText="Plan Code" SortExpression="ph_plancd">
                                <EditItemTemplate>                              
                                    <asp:Label ID="lblRatePlancdE" runat="server" Text='<%# Bind("ph_plancd") %>' Width="80px"></asp:Label> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRatePlancd" runat="server" 
                                    Text='<%# Bind("ph_plancd") %>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>                             
                                    <asp:DropDownList ID="ddlRatePlancdA" runat="server" AppendDataBoundItems="true"                                        
                                        AutoPostBack="true" OnSelectedIndexChanged = "ddlRPlancd_onSelectedindexchange">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList> 
                                    <asp:RequiredFieldValidator ID="rfvRatePlanA" 
                                        ControlToValidate="ddlRatePlancdA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" SortExpression="ph_plandesc">                                 
                                <EditItemTemplate>                               
                                    <asp:Label ID="lblRateDescE" runat="server" Text='<%# Bind("ph_plandesc") %>' Width="250px"></asp:Label> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRateDesc" runat="server" 
                                    Text='<%# Bind("ph_plandesc") %>' Width="250px"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtRateDescA" runat="server" Width="250px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvRateDescA" 
                                        ControlToValidate="txtRateDescA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Tiercode" SortExpression="ph_tiercd">                                
                                <EditItemTemplate>                               
                                    <asp:Label ID="lblRateTierE" runat="server" Text='<%# Bind("ph_tiercd") %>' Width="80px"></asp:Label> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRateTier" runat="server" Width="80px"
                                    Text='<%# Bind("ph_tiercd") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlRateTierA" runat="server"
                                        AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                    </asp:DropDownList> 
                                    <asp:RequiredFieldValidator ID="rfvRatetierA" 
                                        ControlToValidate="ddlRateTierA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="EE Rate" SortExpression="rate_rateamt" ItemStyle-HorizontalAlign="right">                                 
                                <EditItemTemplate>                               
                                    <asp:TextBox ID="txtRateamtA" runat="server" Text='<%# Bind("rate_rateamt") %>' Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvRateamt" 
                                        ControlToValidate="txtRateamtA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="Edit">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator 
                                        id="RegularExpressionValidator3" 
                                        runat="server"
                                        ControlToValidate="txtRateamtA"
                                        ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                        Display="Dynamic" ValidationGroup="Edit"
                                        Font-Names="verdana">
                                        F:'000.0000'
                                    </asp:RegularExpressionValidator> 
                                </EditItemTemplate>                                
                                <ItemTemplate>
                                    <asp:Label ID="lblRateamtA" runat="server" Width="80px"
                                    Text='<%# Bind("rate_rateamt") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtRateamtA" runat="server" Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvRateamtA" 
                                        ControlToValidate="txtRateamtA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator 
                                        id="RegularExpressionValidator2" 
                                        runat="server"
                                        ControlToValidate="txtRateamtA"
                                        ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                        Display="Dynamic" ValidationGroup="Edit"
                                        Font-Names="verdana">
                                        F:'000.00'
                                    </asp:RegularExpressionValidator> 
                                 </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Premium" SortExpression="rate_companyRtamt" ItemStyle-HorizontalAlign="right">                                 
                                <EditItemTemplate>                               
                                    <asp:TextBox ID="txtcoRateamtA" runat="server" Text='<%# Bind("rate_companyRtamt") %>' Width="80px"></asp:TextBox> 
                                     <asp:RequiredFieldValidator ID="rfvcoRateamt" 
                                        ControlToValidate="txtcoRateamtA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="Edit">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator 
                                        id="RegularExpressionValidator4" 
                                        runat="server"
                                        ControlToValidate="txtcoRateamtA"
                                        ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                        Display="Dynamic" ValidationGroup="Edit"
                                        Font-Names="verdana">
                                        F:'000.0000'
                                    </asp:RegularExpressionValidator> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblcoRateamtA" runat="server" Width="80px"
                                    Text='<%# Bind("rate_companyRtamt") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtcoRateamtA" runat="server" Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvcoRateamtA" 
                                        ControlToValidate="txtcoRateamtA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator 
                                        id="RegularExpressionValidator1" 
                                        runat="server"
                                        ControlToValidate="txtcoRateamtA"
                                        ValidationExpression="^[-+]?\d+(\.\d+)?$"
                                        Display="Dynamic" ValidationGroup="Edit"
                                        Font-Names="verdana">
                                        F:'000.00'
                                    </asp:RegularExpressionValidator>   
                                 </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Eff Yrmo" SortExpression="rate_effyrmo">                                 
                                <EditItemTemplate>                               
                                    <asp:Label ID="lblRateYrmoE" runat="server" Text='<%# Bind("rate_effyrmo") %>' Width="80px"></asp:Label> 
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRateYrmoA" runat="server" Width="80px"
                                    Text='<%# Bind("rate_effyrmo") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                             
                                    <asp:TextBox ID="txtRateYrmoA" runat="server" Width="80px"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="rfvRateyrmoA" 
                                        ControlToValidate="txtRateYrmoA"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="ir">
                                    </asp:RequiredFieldValidator>
                                 </FooterTemplate>
                            </asp:TemplateField>                              
                        </Columns>
                    </asp:GridView>  
                    <asp:SqlDataSource ID="SqlDataSource11" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [ph_tiercd] FROM [Planhier]">
                    </asp:SqlDataSource>                                    
                    </div>
                     <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                        <asp:Label ID="lblErrRate" runat="server" Text="" ForeColor="red" style="margin-left:20px;"></asp:Label>
                    </div>
                    <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition1" runat="server" ControlToPersist="grdvRatescroll" />   
                    </ContentTemplate>
                    </asp:UpdatePanel>                                     
                </asp:View>
                <asp:View ID="View3" runat="server">
                    <div id= "subIntroText"> 
                        <asp:Label ID="lblBenRpt" runat="server" Text="Generate Benefits Hierarchy Report" ForeColor="#5D478B"></asp:Label>
	                </div>
                    <div  style="float:left;margin:10px;width:700px">
                        <table>
                            <tr>                            
                                <td style="width:100px;margin:10px;text-align:right;margin-left:20px;">
                                     <asp:Label ID="lblFilter1" runat="server" ForeColor="#036" Text="Program Cd:"></asp:Label>
                                </td>
                                <td style="width:100px;margin:10px;margin-left:20px;">
                                     <asp:DropDownList ID="ddlFilter1" runat="server" DataTextField="ph_progmcd"
                                     DataSourceID="SqlDataSource12" AppendDataBoundItems="true" Width="80px" ValidationGroup="pg1">
                                        <asp:ListItem Text ="All" Selected="True"></asp:ListItem>                                                                   
                                     </asp:DropDownList>
                                     <asp:SqlDataSource ID="SqlDataSource12" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [ph_progmcd] FROM [Planhier]">
                                    </asp:SqlDataSource>
                                </td>
                                <td style="width:300px;margin:10px;margin-left:20px;"></td>
                                <td style="margin:20px;margin-left:20px;">
                                    <asp:LinkButton ID="lnk_genBenRpt" runat="server" ValidationGroup="pg1" OnClick="lnk_genBenRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div> 
                    <div id= "subIntroText"> 
                        <asp:Label ID="Label1" runat="server" Text="Generate Rates Information Report" ForeColor="#5D478B"></asp:Label>
	                </div>
                    <div  style="float:left;margin:10px;width:700px">
                        <table>
                            <tr>
                                <td style="width:100px;margin:10px;text-align:right;">
                                     <asp:Label ID="lblRatePY" runat="server" ForeColor="#036" Text="Plan Year:"></asp:Label>
                                </td>
                                <td style="width:100px;margin:10px;">
                                     <asp:DropDownList ID="ddlFilterPY" runat="server" DataTextField="rate_py"
                                     AppendDataBoundItems="true" DataSourceID="SqlDataSource6" Width="80px">
                                        <asp:ListItem Selected="True">--Select--</asp:ListItem>                                                                   
                                     </asp:DropDownList>
                                     <asp:RequiredFieldValidator ID="rfvRatepy" 
                                        ControlToValidate="ddlFilterPY"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="f1"
                                        InitialValue="--Select--">
                                    </asp:RequiredFieldValidator> 
                                     <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [rate_py] FROM [Rates]">
                                    </asp:SqlDataSource>
                                </td>                            
                                <td style="width:100px;margin:10px;text-align:right;margin-left:20px;">
                                     <asp:Label ID="lblRatePcd" runat="server" ForeColor="#036" Text="Program Cd:"></asp:Label>
                                </td>
                                <td style="width:100px;margin:10px;">
                                     <asp:DropDownList ID="ddlFilterPgCd" runat="server" DataTextField="ph_progmcd"
                                     DataSourceID="SqlDataSource14" AppendDataBoundItems="true" Width="80px">
                                        <asp:ListItem Text ="All" Selected="True"></asp:ListItem>                                                                   
                                     </asp:DropDownList>
                                     <asp:SqlDataSource ID="SqlDataSource14" runat="server"
                                          ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                          SelectCommand="SELECT DISTINCT [ph_progmcd] FROM [Planhier]">
                                    </asp:SqlDataSource>
                                </td>
                                <td style="width:100px;margin:10px;margin-left:20px;"></td>
                                <td style="margin:20px;margin-left:20px;">
                                    <asp:LinkButton ID="LinkButton1" runat="server" ValidationGroup="f1" OnClick="lnk_genRateRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id= "subIntroText"> 
                        <asp:Label ID="Label2" runat="server" Text="Generate Rates Template" ForeColor="#5D478B"></asp:Label>
	                </div>
                    <div  style="float:left;margin:10px;width:700px">
                        <table>
                            <tr> 
                                <td style="width:100px;margin:10px;text-align:right;">
                                     <asp:Label ID="Label3" runat="server" ForeColor="#036" Text="Plan Year:"></asp:Label>
                                </td>
                                <td style="width:100px;margin:10px;">
                                    <asp:TextBox ID="txtTempPlanYear" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width:100px;margin:10px;">
                                    <asp:RequiredFieldValidator ID="rfvTemplatePY" ControlToValidate="txtTempPlanYear"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="temp1">
                                    </asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator 
                                        id="regvTemplatePY" 
                                        runat="server"
                                        ControlToValidate="txtTempPlanYear"
                                        ValidationExpression="\d{4}"
                                        Display="Dynamic" ValidationGroup="temp1">
                                        Format: 'YYYY'
                                    </asp:RegularExpressionValidator> 
                                </td>
                                <td style="width:200px;margin:10px;margin-left:20px;"></td>
                                <td style="margin:20px;margin-left:20px;">
                                    <asp:LinkButton ID="lnk_genTemplate" runat="server" ValidationGroup="temp1" OnClick="lnk_genTemplateRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Template</span></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div> 
                    <div style="float:left;margin:10px;width:700px;margin-top:10px;">
                        <asp:Label ID="lblErr1" runat="server" Text="" ForeColor="red" Font-Size="12px" style="margin-left:20px;"></asp:Label>
                    </div>
                    <div id= "subIntroText"> 
                        <asp:Label ID="Label5" runat="server" Text="Import Rates Template" ForeColor="#5D478B"></asp:Label>
	                </div>
                    <div  style="float:left;margin:10px;width:700px">
                        <table>
                            <tr> 
                                <td style="width:100px;margin:10px;text-align:right;">
                                     <asp:Label ID="Label4" runat="server" ForeColor="#036" Text="Plan Year:"></asp:Label>
                                </td>
                                <td style="width:100px;margin:10px;">
                                    <asp:TextBox ID="txtTempPlanYearImp" runat="server" Width="80px"></asp:TextBox>
                                </td>
                                <td style="width:100px;margin:10px;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="txtTempPlanYearImp"
                                        runat="server" Display="Dynamic"
                                        Text="(Required)"
                                        ValidationGroup="tempImp">
                                    </asp:RequiredFieldValidator>
                                     <asp:RegularExpressionValidator 
                                        id="RegularExpressionValidator5" 
                                        runat="server"
                                        ControlToValidate="txtTempPlanYearImp"
                                        ValidationExpression="\d{4}"
                                        Display="Dynamic" ValidationGroup="tempImp">
                                        Format: 'YYYY'
                                    </asp:RegularExpressionValidator> 
                                </td>                              
                            </tr>
                        </table>
                        <div style="float:left;width:600px;margin-top:10px;margin-left:53px">
                            <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" 
                                Vgroup="tempImp"  FileTypeRange="xls" /> 
                        </div>
                        <div style="float:left;width:600px;margin-left:75px">
                            <asp:LinkButton ID="btn_import" runat="server" CssClass="imgbutton" ValidationGroup="tempImp" 
                            OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value;" 
                            OnClick="btn_import_Click" Font-Underline="false" ><span>Import</span></asp:LinkButton>
                        </div>
                        <asp:HiddenField ID="HiddenField1" runat="server" /> 
                    </div>
                    <div style="float:left;margin:10px;width:700px;margin-top:10px;">
                        <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="red" Font-Size="12px" style="margin-left:20px;"></asp:Label>
                        <br />
                        <asp:Label ID="lbl_result" runat="server" Text=""></asp:Label>
                    </div>    
                </asp:View>
                <asp:View ID="View4" runat="server">
                <div id = "hraIntroText"> 
                    <table>
                        <tr>
                            <td style="width:550px;margin:20px;">
                                <asp:Image ID="img_dtl" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>      
                                <asp:LinkButton ID="lnk_AudBen" runat="server" OnClick="lnkAudBen_OnClick" Font-Underline="false" ForeColor="#5D478B">View Audit Report for Benefit Hierarchy</asp:LinkButton>
                            </td>
                            <td style="margin:20px;">
                                <asp:LinkButton ID="lnk_genDtlRptA1" runat="server" OnClick="lnk_genDtlRptA1_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id = "dtlRptDivA1" runat="server" visible="false">                
                    <div id="hraSubIntroText">Detail Audit Report</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="matchDtl" runat="server">  
                            <asp:GridView ID="grdv_dtlBenAud" runat="server" CssClass="tablestyle" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlBenAud_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate> 
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <FooterStyle CssClass="rowstyle" />                  
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id = "hraIntroText"> 
                    <table>
                        <tr>
                            <td style="width:550px;margin:20px;">
                                <asp:Image ID="img_dtl1" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>      
                                <asp:LinkButton ID="lnk_AudRate" runat="server" OnClick="lnkAudRate_OnClick" Font-Underline="false" ForeColor="#5D478B">View Audit Report for Plan Hierarchy Rates</asp:LinkButton>
                            </td>
                            <td style="margin:20px;">
                                <asp:LinkButton ID="lnk_genDtlRptA2" runat="server" OnClick="lnk_genDtlRptA2_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id = "dtlRptDivA2" runat="server" visible="false">                
                    <div id="hraSubIntroText">Detail Audit Report</div>
                    <div style="float:left;width:650px;margin-top:20px;" >     
                        <div class="scroller" style ="width:700px; max-height:250px;" id="Div6" runat="server">  
                            <asp:GridView ID="grdv_dtlrateAud" runat="server" CssClass="tablestyle" AutoGenerateColumns="true"
                            AllowSorting="true" OnSorting="grdv_dtlrateAud_Sorting" > 
                            <emptydatarowstyle BorderColor="LightBlue"
                            forecolor="Red"/>
                            <EmptyDataTemplate>
                                No records.
                            </EmptyDataTemplate> 
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />
                            <PagerStyle CssClass="customGridpaging" />
                            <FooterStyle CssClass="rowstyle" />                  
                            </asp:GridView>
                        </div>
                    </div>
                </div>		
                </asp:View>
            </asp:MultiView>
            <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                <asp:Label ID="lblErr" runat="server" Text="" ForeColor="red" style="margin-left:20px;"></asp:Label>
            </div>
        </div>               
    </div>    
</asp:Content>


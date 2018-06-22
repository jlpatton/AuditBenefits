<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Maintainence_Reports.aspx.cs" Inherits="VWA_Maintainence_Maintainence_Reports" Title="VWA Reports Maintainence" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">     
        <div id = "module_menu">
            <h1>VWA</h1>            
            <ul id="sitemap">
                <li><a>Import</a>
                    <ul>
                        <li><asp:HyperLink ID="hypBank" runat="server" NavigateUrl="~/VWA/Import_Bank_Statement.aspx">Bank Statement</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypRem" runat="server" NavigateUrl="~/VWA/Import_Remittance_Reports.aspx">Remittance Info</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypVWA" runat="server" NavigateUrl="~/VWA/Import_VWA_File.aspx">VWA File</asp:HyperLink></li>		                	                
                    </ul>
                </li> 
                <li><asp:HyperLink ID="hypTran" runat="server" NavigateUrl="~/VWA/Create_VWA_Tran_Activity.aspx">Transaction Reports</asp:HyperLink></li>                
                <li><asp:HyperLink ID="hypVWABalance" runat="server" NavigateUrl="~/VWA/VWA_Balance.aspx">Balance</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWAAdj" runat="server" NavigateUrl="~/VWA/VWA_Adjustments.aspx">Adjustments</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWACases" runat="server" NavigateUrl="~/VWA/VWA_Cases_Search.aspx">Individual Cases</asp:HyperLink></li>
                <li><a>Maintainence</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypMainRpts" runat="server" Font-Underline="true" NavigateUrl="~/VWA/Maintainence/Maintainence_Reports.aspx">Reports</asp:HyperLink></li>                    
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 	
    </div>
    <div id="contentright"> 
        <div id = "introText" style="width:740px;padding-left:-10px;">VWA Reports Maintainence</div> 
        <div class = "Menu" style="width:750px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Input File(s)" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Output File(s)" Value="1"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView" style="padding-bottom:15px; padding-top:10px; width:750px;">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                    <div class="userPara">
                        <p>Maintenance routine allows the user to define input reports file path and set them to automatically import.</p>                            
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel4" UpdateMode="always">
                        <ContentTemplate>
                            <div id="div_import"  runat="server" style ="width:720px; padding-top:15px; padding-left:5px;">
                                <asp:GridView ID="grdvImport" runat="server" DataKeyNames="id" OnRowUpdating="grdvImport_OnRowUpdating" CssClass="tablestyle" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSourceImport" AutoGenerateEditButton="true">
                                    <AlternatingRowStyle CssClass="altrowstyle" />
                                    <HeaderStyle CssClass="headerstyle" />
                                    <RowStyle CssClass="rowstyle" />
                                    <FooterStyle CssClass="rowstyle" />                        
                                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                                    <EmptyDataTemplate>
                                        <div class="userPara">
                                            <p>No records</p>
                                        </div>
                                    </EmptyDataTemplate> 
                                    <Columns>                                        
                                        <asp:BoundField DataField="filetype" HeaderText="Input File" SortExpression="filetype" ReadOnly="true"/>
                                        <asp:BoundField DataField="filename" HeaderText="File Name" SortExpression="filename" ReadOnly="true"/>                                                            
                                        <asp:TemplateField HeaderText="Import" SortExpression="autoimport">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="cbx_import" runat="server" Checked='<%# Bind("autoimport") %>' OnCheckedChanged="cbx_import_OnChecked" AutoPostBack="true"/>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbxi_import" runat="server" Checked='<%# Bind("autoimport") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Location" SortExpression="filelocation">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_importfile" runat="server" Text='<%# Bind("filelocation") %>' width="240"></asp:TextBox> 
                                                <asp:Label ID="lbl_importErr" ForeColor="red" runat="server" Text="Enter path in format \\server\folder\subfolder" Visible="false"></asp:Label>                                                                                   
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("filelocation") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>  
                                    </Columns>
                                </asp:GridView>                    
                                <asp:SqlDataSource ID="SqlDataSourceImport" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand="SELECT [id], [filetype], [filename], [filelocation], [autoimport] FROM [FileMaintainence] WHERE (([module] = @module) AND ([classification] = @classification)) ORDER BY [filetype]"
                                    UpdateCommand="UPDATE FileMaintainence SET [autoimport]=@autoimport, [filelocation]=@filelocation WHERE ([id]=@id)">
                                    <SelectParameters>
                                        <asp:Parameter Name="module" Type="String" DefaultValue="VWA" />
                                        <asp:Parameter Name="classification" Type="String" DefaultValue="Import" />
                                    </SelectParameters>
                                    <UpdateParameters>                               
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="autoimport" Type="Boolean" />
                                        <asp:Parameter Name="filelocation" Type="String" />
                                    </UpdateParameters>   
                                </asp:SqlDataSource>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:View>
                <asp:View ID="View2" runat="server">
                     <div class="userPara">
                        <p>Maintainence routine allows the user to define output reports file path and set them to automatically save, print or both.</p>    
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel3" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_export" class="scroller" runat="server" style ="width:720px; max-height:460px; padding-top:15px; padding-left:5px;">
                                <asp:GridView ID="grdvExport" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" AutoGenerateColumns="False" OnRowUpdating="grdvExport_OnRowUpdating" DataSourceID="SqlDataSourceExport" AutoGenerateEditButton="true">
                                    <AlternatingRowStyle CssClass="altrowstyle" />
                                    <HeaderStyle CssClass="headerstyle" />
                                    <RowStyle CssClass="rowstyle" />
                                    <FooterStyle CssClass="rowstyle" />                        
                                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                                    <EmptyDataTemplate>
                                        <div class="userPara">
                                            <p>No records</p>
                                        </div>
                                    </EmptyDataTemplate> 
                                    <Columns>
                                        <asp:BoundField DataField="category" HeaderText="Category" SortExpression="category" ReadOnly="true"/>
                                        <asp:BoundField DataField="filetype" HeaderText="Output File" SortExpression="filetype" ReadOnly="true"/>
                                        <asp:BoundField DataField="filename" HeaderText="File Name" SortExpression="filename" ReadOnly="true"/>                                                                
                                        <asp:TemplateField HeaderText="Save" SortExpression="autosave">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="cbx_save" runat="server" Checked='<%# Bind("autosave") %>' OnCheckedChanged="cbx_export_OnChecked" AutoPostBack="true"/>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# Bind("autosave") %>' Enabled="false"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Print" SortExpression="autoprint">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="cbx_print" runat="server" Checked='<%# Bind("autoprint") %>' OnCheckedChanged="cbx_export_OnChecked" AutoPostBack="true"/>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox6" runat="server" Checked='<%# Bind("autoprint") %>' Enabled="false"/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Location" SortExpression="filelocation">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_exportfile" runat="server" Text='<%# Bind("filelocation") %>' width="255"></asp:TextBox>                                                                               
                                                <asp:Label ID="lbl_exportErr" ForeColor="red" runat="server" Text="Enter path in format \\server\folder\subfolder" Visible="false"></asp:Label>                                                                                   
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("filelocation") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    </Columns>
                                </asp:GridView>                    
                                <asp:SqlDataSource ID="SqlDataSourceExport" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand="SELECT [id], [category], [filetype], [filename], [filelocation], [autosave], [autoprint] FROM [FileMaintainence] WHERE (([module] = @module) AND ([classification] = @classification)) ORDER BY [category]"
                                    UpdateCommand="UPDATE FileMaintainence SET [autosave]=@autosave, [autoprint]=@autoprint, [filelocation]=@filelocation WHERE ([id]=@id)">
                                    <SelectParameters>
                                        <asp:Parameter Name="module" Type="String" DefaultValue="VWA" />
                                        <asp:Parameter Name="classification" Type="String" DefaultValue="Export" />
                                    </SelectParameters>
                                    <UpdateParameters>                               
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="autosave" Type="Boolean" />
                                        <asp:Parameter Name="autoprint" Type="Boolean" />
                                        <asp:Parameter Name="filelocation" Type="String" />
                                    </UpdateParameters>   
                                </asp:SqlDataSource>
                            </div>
                            <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition3" runat="server" ControlToPersist="div_export" /> 
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:View>
            </asp:MultiView>
        </div> 
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server"></cc1:SmartScroller>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Maintainence_Module.aspx.cs" Inherits="HRA_Maintainence_Maintainence_Module" Title="HRA Module Maintainence" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">        
        <div id = "module_menu">
            <h1>HRA</h1>            
            <ul id="sitemap"> 
                <li><a>Operations</a>
                    <ul>
                        <li><asp:HyperLink ID="hypEligFile" runat="server" NavigateUrl="~/HRA/Operations/Create_Eligibility_File.aspx">Eligibility File</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypPGP_FTP" runat="server" NavigateUrl="~/HRA/Operations/PGP_FTP_EligFile.aspx">PGP & FTP File</asp:HyperLink></li>		                
                    </ul>
                </li>            
                <li><a>Reconciliation</a>
                    <ul>
                        <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPutnam" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnam.aspx">Putnam</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypPutnamAdj" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnamAdj.aspx">PutnamAdj</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypWageworks" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportWageworks.aspx">Wageworks</asp:HyperLink></li>	                        
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypReconcile" runat="server" NavigateUrl="~/HRA/Reconciliation/Reconcile.aspx">Reconcile</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypReconcileSOFO" runat="server" NavigateUrl="~/HRA/Reconciliation/ReconcileSOFO.aspx">Reconcile SOFO</asp:HyperLink></li>   
                    </ul>
               </li>
               <li><a>Admin bill validation</a>
                    <ul>
		                <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPtnmInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_invoice.aspx">Putnam Invoice</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypWgwkInv" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Wageworks_invoice.aspx">Wageworks Invoice</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypPtnmPartData" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_Putnam_Participant_Data.aspx">Putnam Part Data</asp:HyperLink></li>	                        
		                        <li><asp:HyperLink ID="hypAUDITR" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Import_HRAAUDITR.aspx">HRAAUDITR</asp:HyperLink></li>
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypAdminRecon" runat="server" NavigateUrl="~/HRA/Administrative Bill Validation/Reconcile_Admin_Invoice.aspx">Reconcile Admin Invoice</asp:HyperLink></li>
		            </ul>
                </li>  
               <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/HRA/LettersGenerator/Home.aspx">Letters</asp:HyperLink></li>               
               <li><a>Maintenance</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypCurMod" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Maintenance/Maintainence_Module.aspx">Current Module</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/HRA/Maintenance/Pilot_Search.aspx">Search Pilot</asp:HyperLink></li>                        
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 			
    </div>
    <div id="contentright"> 
        <div id = "introText" style="width:720px;padding-left:-10px;">HRA Module Maintainence</div>          
        <div class = "Menu" style="width:730px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Input File(s)" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Output File(s)" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Codes" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Rates" Value="3"></asp:MenuItem>
                    <asp:MenuItem Text="Exclude SSNs(Admin Bill)" Value="4"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView" style="padding-bottom:15px; padding-top:10px; width:730px;">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                    <div class="userPara">
                        <p>Maintainence routine that allows to define input reports file path and which of them automatically import.</p>                            
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel4" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_import" class="scroller" runat="server" style ="width:720px; max-height:460px; padding-top:15px; padding-left:5px;">
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
                                        <asp:BoundField DataField="category" HeaderText="Category" SortExpression="category" ReadOnly="true"/>
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
                                    SelectCommand="SELECT [id], [category], [filetype], [filename], [filelocation], [autoimport] FROM [FileMaintainence] WHERE (([module] = @module) AND ([classification] = @classification)) ORDER BY [category]"
                                    UpdateCommand="UPDATE FileMaintainence SET [autoimport]=@autoimport, [filelocation]=@filelocation WHERE ([id]=@id)">
                                    <SelectParameters>
                                        <asp:Parameter Name="module" Type="String" DefaultValue="HRA" />
                                        <asp:Parameter Name="classification" Type="String" DefaultValue="Import" />
                                    </SelectParameters>
                                    <UpdateParameters>                               
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="autoimport" Type="Boolean" />
                                        <asp:Parameter Name="filelocation" Type="String" />
                                    </UpdateParameters>   
                                </asp:SqlDataSource>
                            </div>
                            <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition4" runat="server" ControlToPersist="div_import" />
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:View>
                <asp:View ID="View2" runat="server">
                     <div class="userPara">
                        <p>Maintainence routine that allows to define output reports file path and which of them automatically save or print or both.</p>    
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
                                        <asp:Parameter Name="module" Type="String" DefaultValue="HRA" />
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
                <asp:View ID="View3" runat="server">
                    <div class="userPara">
                        <p>Maintainence routine that allows to edit Codes used for HRA Operations.</p>    
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel2" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_codes" class="scroller" runat="server" style ="width:700px; max-height:460px; padding-top:15px; padding-left:5px;">
                                <asp:GridView ID="grdvCodes" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSourceCodes" AutoGenerateEditButton="true">
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
                                        <asp:BoundField DataField="description" HeaderText="Description" SortExpression="description" ReadOnly="true"/>
                                        <asp:TemplateField HeaderText="Code" SortExpression="codeid">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_codeid" Width="100px" runat="server" Text='<%# Bind("codeid") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator
                                                    id="reqval_codeid" ControlToValidate="tbx_codeid"
                                                    Text="(Required)" Display="Dynamic"
                                                    ValidationGroup="codesgrp"
                                                    Runat="server" /> 
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_codeid" runat="server" Width="100px" Text='<%# Bind("codeid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Comments" SortExpression="comments">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_comments" Width="300px" runat="server" Text='<%# Bind("comments") %>' TextMode="MultiLine"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_comments" runat="server" Width="300px" Text='<%# Bind("comments") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>                    
                                <asp:SqlDataSource ID="SqlDataSourceCodes" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand="SELECT [id], [description], [codeid], [comments] FROM [hra_codes]"
                                    UpdateCommand="UPDATE hra_codes SET [codeid]=@codeid, [comments]=@comments WHERE ([id]=@id)">
                                    <UpdateParameters>                               
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="codeid" Type="String" />
                                        <asp:Parameter Name="comments" Type="String" />
                                    </UpdateParameters>   
                                </asp:SqlDataSource>
                            </div>
                            <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition2" runat="server" ControlToPersist="div_codes" />
                        </ContentTemplate>
                    </asp:UpdatePanel>  
                </asp:View>
                <asp:View ID="View4" runat="server">
                    <div class="userPara">
                        <p>Maintainence routine that allows to edit Rates used for HRA Administrative Bill Validation.</p>    
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel1" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_rates" class="scroller" runat="server" style ="width:720px; max-height:460px; padding-top:15px; padding-left:5px;">
                                <asp:GridView ID="grdvRates" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" 
                                AutoGenerateColumns="False" DataSourceID="SqlDataSourceRates" 
                                ShowFooter="True" OnRowUpdating="auditUpdateRates">
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
                                                CommandName="Update" Text="Update" ValidationGroup="EditGrp"></asp:LinkButton> 
                                                <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
                                                CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton ID="lbInsert" runat="server" CommandName="Insert" Width="50px"
                                                ValidationGroup = "InsertGrp" OnClick="Insert_Click">Insert</asp:LinkButton> 
                                                <asp:LinkButton ID="lbReset" runat="server" CausesValidation="False" Width="50px"
                                                CommandName="Cancel" Text="Reset"></asp:LinkButton>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eff YRMO" SortExpression="yrmo">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_iyrmo" Width="70px" runat="server" Text='<%# Bind("yrmo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_eyrmo" Width="70px" runat="server" Text='<%# Bind("yrmo") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbx_yrmo" Width="70px" runat="server" Text='<%# Bind("yrmo") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvRateamt" 
                                                    ControlToValidate="tbx_yrmo"
                                                    runat="server" Display="Dynamic"
                                                    Text="(Required)"
                                                    ValidationGroup = "InsertGrp">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator3" 
                                                    runat="server"
                                                    ControlToValidate="tbx_yrmo"
                                                    ValidationGroup = "InsertGrp"
                                                    ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                                                    Display="Dynamic" 
                                                    Font-Names="verdana">
                                                    Enter valid YRMO
                                                </asp:RegularExpressionValidator> 
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" SortExpression="type">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_itype" runat="server" Text='<%# Bind("type") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_etype" runat="server" Text='<%# Bind("type") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddl_ftype" runat="server"                                        
                                                    AutoPostBack="true" OnSelectedIndexChanged = "ddl_ftype_onSelectedindexchanged">
                                                    <asp:ListItem Selected="True">--Select--</asp:ListItem>
                                                    <asp:ListItem>Wageworks</asp:ListItem>
                                                    <asp:ListItem>Putnam</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvType" 
                                                    ControlToValidate="ddl_ftype"
                                                    runat="server" Display="Dynamic"
                                                    Text="(Required)"
                                                    ValidationGroup="InsertGrp">
                                                </asp:RequiredFieldValidator>                                                
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="description">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_edesc" Width="320px" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lbl_idesc" Width="320px" runat="server" Text='<%# Bind("description") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt_fdesc" runat="server" Width="320px" TextMode="MultiLine"></asp:TextBox> 
                                                <asp:RequiredFieldValidator ID="rfv_fdesc" 
                                                    ControlToValidate="txt_fdesc"
                                                    runat="server" Display="Dynamic"
                                                    Text="(Required)"
                                                    ValidationGroup="InsertGrp">
                                                </asp:RequiredFieldValidator>
                                            </FooterTemplate>
                                        </asp:TemplateField>                                   
                                        <asp:TemplateField HeaderText="Rate" SortExpression="rate">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_erate" Width="60px" runat="server" Text='<%# Bind("rate") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="tbx_erate"
                                                    ValidationGroup="EditGrp"
                                                    Display="Dynamic" Text="(Required)">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator1" 
                                                    runat="server"
                                                    ControlToValidate="tbx_erate"
                                                    ValidationGroup="EditGrp"
                                                    ValidationExpression="^\$?(-)?\d+(\.\d{0,2})?$"
                                                    Display="dynamic">
                                                    Enter valid amount
                                                </asp:RegularExpressionValidator> 
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_rate" Width="60px" runat="server" Text='<%# Bind("rate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="tbx_frate" Width="60px" runat="server" Text='<%# Bind("rate") %>'></asp:TextBox>
                                                <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="tbx_frate"
                                                    ValidationGroup = "InsertGrp"
                                                    Display="Dynamic" Text="(Required)">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator1" 
                                                    runat="server"
                                                    ControlToValidate="tbx_frate"
                                                    ValidationGroup = "InsertGrp"
                                                    ValidationExpression="^\$?(-)?\d+(\.\d{0,2})?$"
                                                    Display="dynamic">
                                                    Enter valid amount
                                                </asp:RegularExpressionValidator> 
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>                    
                                <asp:SqlDataSource ID="SqlDataSourceRates" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand= "select a.type,a.yrmo,a.id,'$' + CONVERT(varchar(12), a.rate, 0) AS [rate],a.description
                                                        from hra_rates a
                                                        INNER JOIN hra_rates b ON
                                                        a.type = b.type and a.yrmo &lt;= b.yrmo
                                                        GROUP BY a.type, a.yrmo,a.id,a.rate,a.description
                                                        HAVING COUNT(DISTINCT b.yrmo) &lt;=3
                                                        ORDER BY a.type,a.yrmo DESC"
                                    UpdateCommand="UPDATE hra_rates SET [rate]=CONVERT(money,@rate) WHERE ([id]=@id)"
                                    DeleteCommand="DELETE FROM hra_rates WHERE ([id]=@id)">                                    
                                    <UpdateParameters>                               
                                        <asp:Parameter Name="id" Type="Int32" />
                                        <asp:Parameter Name="rate" Type="String" />
                                    </UpdateParameters>                           
                                </asp:SqlDataSource>  
                            </div>
                            <div style="float:left;margin:10px;width:700px;margin-top:20px;">
                                <asp:Label ID="lblErrRate" runat="server" ForeColor="red" style="margin-left:20px;"></asp:Label>
                            </div>
                            <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition1" runat="server" ControlToPersist="div_rates" />   
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:View>
                <asp:View ID="View5" runat="server">
                    <div class="userPara">
                        <p>Maintainence routine that allows to add, edit and delete a list of SSNs to exclude from the Putnam Participant Data Report for HRA Administrative Bill validation.</p>    
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel5" UpdateMode="always">
	                    <ContentTemplate>
	                        <div id="div_ssn" class="scroller" runat="server" style ="width:500px; max-height:460px; padding-top:15px; padding-left:5px;">
	                            <asp:GridView ID="grdvSSN" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" 
	                            AutoGenerateColumns="False" DataSourceID="SqlDataSourceSSN" 
	                            ShowFooter="True" OnRowUpdating="auditUpdateSSN">
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
					                    <asp:TemplateField ShowHeader="False">
						                    <ItemTemplate>
							                    <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="False" 
								                CommandName="Edit" Text="Edit" Width="50px" ></asp:LinkButton>
							                    <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" 
								                    CommandName="Delete" Text="Delete"
								                    OnClientClick="return confirm('Are you sure you want to delete this record?');">
							                    </asp:LinkButton>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="True" Width="50px"
							                    CommandName="Update" Text="Update" ValidationGroup="EditsGrp"></asp:LinkButton> 
							                    <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
							                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:LinkButton ID="lnkInsert" runat="server" CommandName="Insert" Width="50px"
							                    ValidationGroup = "InsertsGrp" OnClick="InsertSSN_Click">Insert</asp:LinkButton> 
							                    <asp:LinkButton ID="lnkReset" runat="server" CausesValidation="False" Width="50px"
							                    CommandName="Cancel" Text="Reset"></asp:LinkButton>
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="SSN Excluded" SortExpression="ssn" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="200px" FooterStyle-Width="200px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_issn" Width="200px" runat="server" Text='<%# Bind("ssn") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbx_essn" Width="100px" runat="server" Text='<%# Bind("ssn") %>' style="display:block;"></asp:TextBox>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
							                        ControlToValidate="tbx_essn"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
							                        id="RegularExpressionValidator1" 
							                        runat="server"
							                        ControlToValidate="tbx_essn"
							                        ValidationGroup="EditsGrp"
							                        ValidationExpression="^(\d{3}-\d{2}-\d{4})|(\d{1,9})$"
							                        Display="dynamic">
							                        Enter SSN in format xxx-xx-xxxx or xxxxxxxxx
						                        </asp:RegularExpressionValidator>
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbx_ssn" Width="100px" runat="server" Text='<%# Bind("ssn") %>' style="display:block;"></asp:TextBox>
							                    <asp:RequiredFieldValidator ID="rfvSSN" 
								                    ControlToValidate="tbx_ssn"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator3" 
								                    runat="server"
								                    ControlToValidate="tbx_ssn"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^(\d{3}-\d{2}-\d{4})|(\d{1,9})$"
								                    Display="Dynamic" 
								                    Font-Names="verdana">
								                    Enter SSN in format xxx-xx-xxxx or xxxxxxxxx
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
				                    </Columns>
			                    </asp:GridView>
			                    <asp:SqlDataSource ID="SqlDataSourceSSN" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
				                    SelectCommand= "SELECT [id], SUBSTRING([ssn],1,3) + '-' + SUBSTRING([ssn],4,2) + '-' + SUBSTRING([ssn],6,9) AS [ssn] FROM [hra_excludeSSNAdmin]"
				                    UpdateCommand="UPDATE [hra_excludeSSNAdmin] SET [ssn]=RIGHT('000000000'+REPLACE(@ssn,'-', ''), 9) WHERE ([id]=@id)"
				                    DeleteCommand="DELETE FROM [hra_excludeSSNAdmin] WHERE ([id]=@id)">                                    
				                    <UpdateParameters>                               
					                    <asp:Parameter Name="id" Type="Int32" />
					                    <asp:Parameter Name="ssn" Type="String" />
				                    </UpdateParameters>                           
			                    </asp:SqlDataSource>                      
	                        </div>
	                        <div style="float:left;margin:10px;width:700px;margin-top:20px;">
			                    <asp:Label ID="lblErrSSN" runat="server" ForeColor="red" style="margin-left:20px;"></asp:Label>
		                    </div>
		                    <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition5" runat="server" ControlToPersist="div_ssn" /> 
	                    </ContentTemplate>
	                </asp:UpdatePanel>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>    
    <cc1:SmartScroller ID="SmartScroller1" runat="server"></cc1:SmartScroller>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Templates.aspx.cs" Inherits="HRA_LettersGenerator_Template1" Title="Create,Edit,View Letter Templates" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>
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
                    <ul>
                        <li><asp:HyperLink ID="hypCurMod" runat="server"  NavigateUrl="~/HRA/Maintenance/Maintainence_Module.aspx">Current Module</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypBillingImp" runat="server" NavigateUrl="~/HRA/Maintenance/Pilot_Search.aspx">Search Pilot</asp:HyperLink></li>                        
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div>    
    </div>
    <div id="contentright">
     <div id="Navigation">
            <div class="menuNav">
                <ul>
	                <li><a class="menuNavone" href="#">Letters</a>
		                <table><tr><td>
		                <ul>			                
			                <li><a href="Templates.aspx">Letter Templates</a></li>
			                <li><a href="Generate.aspx">Generate Letter</a></li>	
			            </ul>		               
		                </td></tr></table>
	                </li>
	            </ul>	                            
            </div>
	    </div>	
	    <div id="containerTab">
              <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal" 
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Templates" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Upload" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Confirm" Value="2"></asp:MenuItem>                                    
                </Items>
                <StaticMenuItemStyle CssClass="tabNav" />
                <StaticSelectedStyle CssClass="selectedNavTab" />
                <StaticHoverStyle CssClass="selectedNavTab1" />                     
            </asp:Menu>
            <div id = "navView">                
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">                    
                    <asp:View ID="View1" runat="server"> 
                        <div id= "subIntroText"> 
                            <asp:Label ID="lblHeading1" runat="server" Text="Create Template" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div  style="float:left;margin:10px;width:700px">
                            <table>
                                <tr> 
                                    <td style="margin:10px;">
                                        <asp:LinkButton ID="lnkInstall" runat="server" OnClick="lnkCreate_onclick" >Create a New Template</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id= "subIntroText"> 
                            <asp:Label ID="lblHeading2" runat="server" Text="Available Templates" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">                        
                            <asp:GridView runat="server" ID="grdvLetter" CssClass="tablestyle"                       
                            AutoGenerateColumns="False"  AllowSorting="True" 
                            OnRowCommand="grdvLetter_rowcommand"
                            AllowPaging="True" PagerSettings-Mode="NumericFirstLast" 
                            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                            DataKeyNames="ltrId" DataSourceID="SqlDataSource4"> 
                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                            <AlternatingRowStyle CssClass="altrowstyle" />
                            <HeaderStyle CssClass="headerstyle" />
                            <RowStyle CssClass="rowstyle" />               
                            <PagerStyle CssClass="customGridpaging" />   
                                <Columns>
                                    <asp:ButtonField CommandName="Select" ButtonType="Link" Text="Select" />
                                    <asp:BoundField DataField="ltrId" HeaderText="Id" InsertVisible="False" Visible="False" ReadOnly="True"
                                        SortExpression="ltrId" />
                                    <asp:BoundField DataField="ltrType" HeaderText="Letter Type" SortExpression="ltrType" />
                                    <asp:BoundField DataField="ltrTypeCode" HeaderText="Letter Code" SortExpression="ltrTypeCode" />  	                                
	                            </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [ltrId], [ltrType], [ltrTypeCode] FROM [hra_Ltrs_Type]">
                            </asp:SqlDataSource>
	                    </div>
	                    <div id="childDiv" runat="server" visible="false">
	                        <div id= "subIntroText"> 
                                <asp:Label ID="lblChildHeading" runat="server" Text="" ForeColor="#5D478B"></asp:Label>
	                        </div>	
	                        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                            <asp:GridView runat="server" ID="grdvltrTemplates" CssClass="tablestyle"                       
                                AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource2"
                                AllowPaging="True" PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="tpId"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                    <Columns>
                                        <asp:BoundField DataField="tpId" HeaderText="Id" InsertVisible="False" Visible="false" ReadOnly="True"
                                            SortExpression="tpId" />               
                                        <asp:BoundField DataField="tpVersion" HeaderText="Version" SortExpression="tpVersion" />
                                         <asp:BoundField DataField="tpDate" HeaderText="Date" SortExpression="tpDate" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkView_onclick">View</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_onclick">Download</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>    
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand="SELECT [tpId],[tpVersion],[tpDate] FROM hra_Ltrs_template WHERE tpTypeId = @lId">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="lId" SessionField="masterId" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div> 
                        </div>                   
	                    <div class="error" id="errorDiv1" runat="server" visible="false">
                             <asp:Label ID="lblErrorTemplates" runat="server" Text=""></asp:Label>
	                    </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server"> 
                        <div id= "introText"> 
                            <asp:Label ID="lblHeading3" runat="server" Text="Import Letters Template" ForeColor="#5D478B"></asp:Label>
	                    </div>
                        <div  style="float:left;margin:10px;width:700px">
                             <div id= "subIntroText"> 
                                <asp:Label ID="lblsubHeading1" runat="server" Text="Add New Letter Type" ForeColor="#5D478B"></asp:Label>
	                        </div>
                            <table style="margin-top:10px">
                                <tr>
                                    <td style="width:140px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblLetterAdd" runat="server" ForeColor="#036" Text="New Letter Type:"></asp:Label>
                                    </td>
                                    <td style="width:100px;margin:10px;">
                                        <asp:TextBox ID="txtletterType" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtletterType"
                                            runat="server" Display="Dynamic"
                                            Text="(Required)" 
                                            ValidationGroup="tempAdd">
                                        </asp:RequiredFieldValidator>                                         
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:140px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblLetterCode" runat="server" ForeColor="#036" Text="Letter Code:"></asp:Label>
                                    </td>
                                    <td style="width:100px;margin:10px;">
                                        <asp:TextBox ID="txtLetterCode" runat="server"></asp:TextBox>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtLetterCode"
                                            runat="server" Display="Dynamic"
                                            Text="(Required)" 
                                            ValidationGroup="tempAdd">
                                        </asp:RequiredFieldValidator> 
                                        <asp:RegularExpressionValidator ID="regExpV1" ControlToValidate="txtLetterCode"
                                            runat="server" Display="Dynamic" ValidationExpression="^[a-zA-Z0-9]{1,5}$"
                                            Text="(Max 5 Letter Alphanumeric code)" 
                                            ValidationGroup="tempAdd"></asp:RegularExpressionValidator>                                        
                                    </td>
                                    <td style=";margin:10px;">
                                         <asp:LinkButton ID="lnkbtnAdd" runat="server" CssClass="imgbutton" 
                                            ValidationGroup="tempAdd"  
                                            OnClick="btn_Add_Click" Font-Underline="false" ><span>Add New</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                         <div  style="float:left;margin:10px;width:700px">
                            <div id= "subIntroText"> 
                                <asp:Label ID="lblsubHeading2" runat="server" Text="Import Templates" ForeColor="#5D478B"></asp:Label>
	                        </div>
                            <table style="margin-top:10px">
                                <tr> 
                                    <td style="width:150px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblLettertype" runat="server" ForeColor="#036" Text="Letter Type:"></asp:Label>
                                    </td>
                                    <td style="width:200px;margin:10px;">
                                        <asp:DropDownList ID="ddlLetterType" runat="server" AutoPostBack="true" AppendDataBoundItems="false" 
                                        OnSelectedIndexChanged="ddlLetterType_SelectedIndexChange" OnDataBound="ddlLetterType_DataBound"
                                        DataSourceID="SqlDataSource1" DataTextField="ltrType" DataValueField="ltrId">                                            
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                        SelectCommand="SELECT [ltrId],[ltrType] FROM [hra_Ltrs_Type]"></asp:SqlDataSource>
                                    </td>
                                    <td style="width:80px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlLetterType" 
                                            runat="server" Display="static"
                                            Text="(Required)" InitialValue="--Select Type--"
                                            ValidationGroup="tempImp">
                                        </asp:RequiredFieldValidator>                                         
                                    </td>                                    
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td style="width:150px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblUplType" runat="server" ForeColor="#036" Text="Upload Type:"></asp:Label>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:DropDownList ID="ddlUploadType" runat="server" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlUploadType_SelectedIndexChange">
                                            <asp:ListItem Selected="true">--Select--</asp:ListItem>
                                            <asp:ListItem>New</asp:ListItem>
                                            <asp:ListItem>Newly Edited</asp:ListItem>
                                            <asp:ListItem>In Validation</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width:80px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddlUploadType"
                                            runat="server" Display="Dynamic"
                                            Text="(Required)" InitialValue="--Select--"
                                            ValidationGroup="tempImp">
                                        </asp:RequiredFieldValidator>                                         
                                    </td> 
                                    <td style="width:100px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblVersion" runat="server" ForeColor="#036" Text="Version:"></asp:Label>
                                    </td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:TextBox ID="txtVersion" runat="server" ReadOnly="true" Width="120px"></asp:TextBox>
                                    </td>                                                                 
                                </tr>
                            </table>
                            <div style="float:left;width:600px;margin-top:20px;margin-left:103px">
                                <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" 
                                    Vgroup="tempImp"  FileTypeRange="xml,XML" /> 
                            </div>
                            <div style="float:left;width:600px;margin-left:155px">
                                <asp:LinkButton ID="btn_import" runat="server" CssClass="imgbutton" ValidationGroup="tempImp" 
                                OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_HiddenField1').value = document.getElementById('ctl00_ContentPlaceHolder1_FileUpload1_FilUpl').value;" 
                                OnClick="btn_import_Click" Font-Underline="false" ><span>Import</span></asp:LinkButton>
                            </div>
                            <asp:HiddenField ID="HiddenField1" runat="server" /> 
                        </div>
                        <div class="error" id="errorDiv2" runat="server" visible="false">
                            <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="success" id="resultDiv" runat="server" visible="false">
                            <asp:Label ID="lbl_result" runat="server" Text=""></asp:Label>
                        </div>    
                    </asp:View>
                    <asp:View ID="View3" runat="server"> 
                        <div id= "subIntroText"> 
                            <asp:Label ID="Label1" runat="server" Text="Letter Templates under Validation" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                     <div  style="float:left;margin:10px;width:700px">
	                         <asp:GridView runat="server" ID="grdvStageTemplates" CssClass="tablestyle"                       
                                AutoGenerateColumns="False"  AllowSorting="True" OnRowCommand="grdvStageTemplate_rowCommand"
                                AllowPaging="True" PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="tpStageId" DataSourceID="SqlDataSource3"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                <Columns>
                                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="60px">
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
                                            CommandName="UpdateAlt" Text="Update" CommandArgument='<%# Bind("tpStageId") %>' ></asp:LinkButton> 
                                            <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
                                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" SortExpression="tpStageId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Bind("tpStageId") %>' ></asp:Label>                                            
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblIDE" runat="server" Visible="false" Text='<%# Bind("tpStageId") %>' ></asp:Label> 
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Letter Type" SortExpression="ltrType" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("ltrType") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblTypeE" runat="server" Text='<%# Bind("ltrType") %>' ></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>                                     
                                    <asp:TemplateField HeaderText="Version" SortExpression="tpStageVersion" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersion" runat="server" Text='<%# Bind("tpStageVersion") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblVersionE" runat="server" Text='<%# Bind("tpStageVersion") %>' ></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Letter" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkViewStg_onclick">View Content</asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                             <asp:LinkButton ID="lnkViewE" runat="server" OnClick="lnkViewStg_onclick">View Content</asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="DateTime" SortExpression="tpStagedate" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("tpStagedate") %>' ></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblDateE" runat="server" Text='<%# Bind("tpStagedate") %>' ></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Approved?">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbxAprroved" runat="server" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbxAprrovedE" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>                                    
                                </Columns>                                                    
                            </asp:GridView>
                             <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                 SelectCommand="SELECT [tpStageId],[ltrType],[tpStageVersion], [tpStagedate]
                                                 FROM [hra_Ltrs_template_staging],[hra_Ltrs_Type] WHERE ltrId = tpStageTypeId"
                                 DeleteCommand="DELETE FROM [hra_Ltrs_template_staging] WHERE tpStageId = @tpStageId">
                             </asp:SqlDataSource>
	                     </div>
	                     <div class="error" id="errorDiv3" runat="server" visible="false">
                             <asp:Label ID="lblErr3" runat="server" Text=""></asp:Label>
	                     </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div>  
    </div>
</asp:Content>


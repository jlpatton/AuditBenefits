<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Generate.aspx.cs" Inherits="HRA_LettersGenerator_Generate" Title="Generate Letters" %>
<%@ Register TagPrefix="FileUpload" TagName="FileUpload1" Src="~/UserControls/FileUploadControl.ascx" %>
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
                    <asp:MenuItem Text="Adhoc Letters" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Validation Letters" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Confirmation Letters" Value="2"></asp:MenuItem>   
                    <asp:MenuItem Text="Reprint Letters" Value="3"></asp:MenuItem>                                 
                </Items>
                <StaticMenuItemStyle CssClass="tabNav" />
                <StaticSelectedStyle CssClass="selectedNavTab" />
                <StaticHoverStyle CssClass="selectedNavTab1" />                     
            </asp:Menu>
	        <div id = "navView">                
                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View1" runat="server"> 
                        <div id= "introText"> 
                            <asp:Label ID="lblHeading1" runat="server" Text="Select Template" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div  style="float:left;margin:10px;width:700px">                           
                            <table style="margin-top:10px">
                                <tr> 
                                    <td style="width:150px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblLettertype" runat="server" ForeColor="#036" Text="Letter Type:"></asp:Label>
                                    </td>
                                    <td style="width:200px;margin:10px;">
                                        <asp:DropDownList ID="ddlLetterType" runat="server" AutoPostBack="true" AppendDataBoundItems="false" 
                                        OnSelectedIndexChanged="ddlLetterType_SelectedIndexChange" OnDataBound="ddlLetterType_DataBound"
                                        DataSourceID="SqlDataSource1" DataTextField="ltrType" DataValueField="ltrTypeCode">                                            
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                        SelectCommand="SELECT [ltrTypeCode],[ltrType] FROM [hra_Ltrs_Type] 
                                                        WHERE ltrTypeCode NOT IN ('HRB1','HRB2')"></asp:SqlDataSource>
                                    </td>
                                    <td style="width:80px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlLetterType" 
                                            runat="server" Display="Dynamic"
                                            Text="(Required)" InitialValue="--Select Type--"
                                            ValidationGroup="ltrGen">
                                        </asp:RequiredFieldValidator>                                         
                                    </td>                                    
                                </tr>                            
                                <tr>                                                                       
                                    <td style="width:150px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblVersion" runat="server" ForeColor="#036" Text="Version:"></asp:Label>
                                    </td>
                                    <td style="width:200px;margin:10px;">
                                        <asp:TextBox ID="txtVersion" runat="server" ReadOnly="true" Width="120px"></asp:TextBox>
                                    </td>                                                                 
                                </tr>                            
                                <tr>
                                    <td style="width:150px;margin:10px;text-align:right;">
                                         <asp:Label ID="lblFilter" runat="server" ForeColor="#036" Text="Filter:"></asp:Label>
                                    </td>
                                    <td style="width:200px;margin:10px;">
                                        <asp:RadioButtonList ID="rdbFilter" runat="server" RepeatDirection="Vertical"
                                            AutoPostBack="true" OnSelectedIndexChanged="rdbfilter_OnCheck" >
                                            <asp:ListItem Text="All Employees" Value="0" />
                                            <asp:ListItem Text="Enter Employee Numbers" Value="1" />
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="width:80px;margin:10px;">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ErrorMessage="(Required)" Display="dynamic" ValidationGroup="ltrGen"
                                            ControlToValidate="rdbFilter"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <div id="empTable" runat="server" visible="false">
                                <table   style="margin-top:5px">
                                    <tr>
                                        <td style="width:150px;margin:10px;text-align:right;"></td>
                                        <td style="width:300px;margin:10px;">
                                            <asp:Label ID="Label3" runat="server" ForeColor="Blue" Text="Import Employee List"></asp:Label>
                                        </td>
                                        <td style="width:80px;margin:10px;"></td>
                                    </tr>
                                </table> 
                                <div style="float:left;margin-left:105px;margin-top:10px">
                                    <FileUpload:FileUpload1 ID="FileUpload1" Vgroup="ltrImp" runat="server" Required="true"  FileTypeRange="txt,csv"/>                             
                                </div>
                                <table  style="margin-top:-10px;">
                                    <tr>
                                        <td style="width:50px;margin:5px;text-align:right;"></td>
                                        <td style="width:150px;margin:5px;">                                       
                                        </td>
                                        <td style="width:150px;margin:5px;">
                                            <asp:LinkButton ID="lnkImport" CssClass="imgbutton" runat="server" 
                                            ValidationGroup="ltrImp" OnClick="lnkImport_Click"><span>Import File</span></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <table  style="margin-top:5px">
                                    <tr>
                                        <td style="width:150px;margin:10px;text-align:right;"></td>
                                        <td>
                                            <asp:Label ID="lblOr2" runat="server" Text="---- or ----" style="margin-left:50px;font-style:italic;color:#87CEEB"></asp:Label>    
                                        </td>
                                        <td style="width:80px;margin:10px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width:150px;margin:10px;text-align:right;"></td>
                                        <td style="width:300px;margin:10px;">
                                            <asp:Label ID="lblNote" runat="server" ForeColor="Blue" Text="Enter comma seperated list of Employee numbers:"></asp:Label>
                                        </td>
                                        <td style="width:80px;margin:10px;"></td>
                                    </tr>
                                </table>                                
                                <table>                                    
                                    <tr>
                                        <td style="width:150px;margin:10px;text-align:right;">
                                             <asp:Label ID="lblEmp" runat="server" ForeColor="#036" Text="Employee:" Visible="false"></asp:Label>
                                        </td>
                                        <td style="width:300px;margin:10px;">
                                            <asp:TextBox ID="txtEmpNum" runat="server" TextMode="MultiLine"
                                                Rows="5" Columns="50"></asp:TextBox>
                                        </td>
                                        <td style="width:80px;margin:10px;">
                                            <asp:RequiredFieldValidator ID="rfvEmp" runat="server" 
                                            ErrorMessage="(Required)" Display="dynamic"
                                            ControlToValidate="txtEmpNum"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regvEmp" runat="server" 
                                            ErrorMessage="Invalid format!" Display="Dynamic" ValidationExpression="([0-9]+,?)+"
                                            ControlToValidate="txtEmpNum" ></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table>
                                <tr>
                                    <td style="width:50px;margin:10px;text-align:right;"></td>
                                    <td style="width:150px;margin:10px;">
                                        <asp:LinkButton ID="lnkPreview" CssClass="imgbutton" runat="server" Visible="false"
                                        ValidationGroup="ltrGen" OnClick="lnkPrev_Click"><span>Preview Letter</span></asp:LinkButton>
                                    </td>
                                    <td style="width:349px;margin:10px;">
                                        <asp:LinkButton ID="lnkGen1" CssClass="imgbutton" runat="server" 
                                        ValidationGroup="ltrGen" OnClick="lnkGen1_Click"><span>Generate Letter</span></asp:LinkButton>
                                        </td>
                                    <td style="width:349px;margin:10px;">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="imgbutton" OnClick="LinkButton1_Click"
                                            ValidationGroup="ltrGen" Width="132px"><span>Generate SPD Letter</span></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="error" id="errorDiv1" runat="server" visible="false">
                            <asp:Label ID="lbl_error" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="info" id="infoDiv1" runat="server" visible="false">
                            <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                        </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server"> 
                        <div id= "introText"> 
                            <asp:Label ID="lblSubheading2" runat="server" Text="Validation Letters - Pending" ForeColor="#5D478B"></asp:Label>
                         </div>
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
	                      <table>
                                <tr>
                                    <td style="width:50px;margin:10px;text-align:right;"></td>
                                    <td style="width:349px;margin:10px;">
                                        <asp:LinkButton ID="LinkButton4" runat="server" CssClass="imgbutton" OnClick="LinkButton1_Click"
                                            ValidationGroup="ltrGen" Width="132px"><span>Generate SPD Letter</span></asp:LinkButton>
                                    </td>
                               </tr>
                            </table>
                            <asp:GridView runat="server" ID="grdvltrPending" CssClass="tablestyle"                       
                                AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource2"
                                PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="pnId"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                <Columns>
                                    <asp:BoundField DataField="pnId" HeaderText="Id" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="pnId" />               
                                    <asp:BoundField DataField="pnEmpNum" HeaderText="Employee #" SortExpression="pnEmpNum" />
                                    <asp:BoundField DataField="pnDepSSN" HeaderText="Dependant SSN" SortExpression="pnDepSSN" />
                                    <asp:BoundField DataField="pnDepRelationship" HeaderText="Relationship" SortExpression="pnRelationship" />
                                    <asp:BoundField DataField="pnDate" HeaderText="Date" SortExpression="pnDate" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkPrint_onclick">Print</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                         
                                </Columns>    
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [pnId],[pnEmpNum],[pnDepSSN],[pnDepRelationship],[pnDate] FROM hra_Ltrs_Pending WHERE [pnStatus] = '1'  AND [pnLtrType] = 'val'">                                   
                            </asp:SqlDataSource>
                        </div> 
                        <div class="error" id="errorDiv2" runat="server" visible="false">
                            <asp:Label ID="lbl_error1" runat="server" Text=""></asp:Label>
                        </div>
                        <div id= "introText"> 
                            <asp:Label ID="Label2" runat="server" Text="Reprint Validation Letters" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                            <asp:GridView runat="server" ID="grdvLtrPendingreprint" CssClass="tablestyle"                       
                                AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource5"
                                PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="lgId"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                <Columns>
                                    <asp:BoundField DataField="pnId" HeaderText="Id" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="pnId" />                                     
                                    <asp:BoundField DataField="lgId" HeaderText="genId" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="lgId" />               
                                    <asp:BoundField DataField="pnEmpNum" HeaderText="Employee #" SortExpression="pnEmpNum" />
                                    <asp:BoundField DataField="pnDepSSN" HeaderText="Dependant SSN" SortExpression="pnDepSSN" />
                                    <asp:BoundField DataField="pnDepRelationship" HeaderText="Relationship" SortExpression="pnRelationship" />
                                    <asp:BoundField DataField="lgdate" HeaderText="Print Date" SortExpression="lgdate" />   
                                    <asp:BoundField DataField="lgUser" HeaderText="User" SortExpression="lgUser" />                                                                        
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReprint" runat="server" OnClick="lnkrePrint_onclick">Re-Print</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>    
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [pnId],[pnEmpNum],[pnDepSSN],[pnDepRelationship],[lgId],[lgdate],[lgUser]
                                                FROM hra_Ltrs_Pending,hra_Ltrs_Generated WHERE [pnStatus] = '0' AND [pnGenId] = [lgId]  AND [pnLtrType] = 'val'">                                   
                            </asp:SqlDataSource>
                        </div>
                    </asp:View>
                     <asp:View ID="View3" runat="server"> 
                        <div id= "introText"> 
                            <asp:Label ID="lblHeading3" runat="server" Text="Confirmation Letters  - Pending" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                            <asp:GridView runat="server" ID="grdvConfPen" CssClass="tablestyle"                       
                                AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource6"
                                PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="pnId"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                <Columns>
                                    <asp:BoundField DataField="pnId" HeaderText="Id" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="pnId" />               
                                    <asp:BoundField DataField="pnEmpNum" HeaderText="Employee #" SortExpression="pnEmpNum" />                                    
                                    <asp:BoundField DataField="pnDate" HeaderText="Date" SortExpression="pnDate" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkView" runat="server" OnClick="lnkConfPrint_onclick">Print</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                                         
                                </Columns>    
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [pnId],[pnEmpNum],[pnDate] FROM hra_Ltrs_Pending WHERE [pnStatus] = '1' AND [pnLtrType] = 'conf'">                                   
                            </asp:SqlDataSource>
                        </div> 
                        <div class="error" id="errorDiv4" runat="server" visible="false">
                            <asp:Label ID="lbl_error4" runat="server" Text=""></asp:Label>
                        </div>
                   
                        <div id= "introText"> 
                            <asp:Label ID="lblHeading4" runat="server" Text="Reprint Confirmation Letters" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                            <asp:GridView runat="server" ID="grdvConfPenReprint" CssClass="tablestyle"                       
                                AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource7"
                                PagerSettings-Mode="NumericFirstLast" 
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif"  
                                DataKeyNames="lgId"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" /> 
                                <Columns>
                                    <asp:BoundField DataField="pnId" HeaderText="Id" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="pnId" />                                     
                                    <asp:BoundField DataField="lgId" HeaderText="genId" InsertVisible="False" Visible="false" ReadOnly="True"
                                        SortExpression="lgId" />               
                                    <asp:BoundField DataField="pnEmpNum" HeaderText="Employee #" SortExpression="pnEmpNum" />                                    
                                    <asp:BoundField DataField="lgdate" HeaderText="Print Date" SortExpression="lgdate" />   
                                    <asp:BoundField DataField="lgUser" HeaderText="User" SortExpression="lgUser" />                                                                        
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReprint" runat="server" OnClick="lnkConfrePrint_onclick">Re-Print</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>    
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [pnId],[pnEmpNum],[pnDepSSN],[pnDepRelationship],[lgId],[lgdate],[lgUser]
                                                FROM hra_Ltrs_Pending,hra_Ltrs_Generated WHERE [pnStatus] = '0' AND [pnGenId] = [lgId]  AND [pnLtrType] = 'conf'">                                   
                            </asp:SqlDataSource>
                        </div>
                    </asp:View>
                    <asp:View ID="View4" runat="server"> 
                        <div id= "introText"> 
                            <asp:Label ID="Label1" runat="server" Text="Reprint Letters" ForeColor="#5D478B"></asp:Label>
	                    </div>
	                    <div id= "subIntroText"> 
                            <asp:Label ID="lblReprintList" runat="server" Text="Select Letter Type/Print Batch" ForeColor="#5D478B"></asp:Label>
                        </div>	
	                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">	                        
                            <asp:GridView runat="server" ID="grdvReprint" CssClass="tablestyle"                       
                                AutoGenerateColumns="False"  AllowSorting="True" 
                                PagerSettings-Mode="NumericFirstLast" OnRowCommand="grdvReprint_Rowcommand"
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" DataKeyNames="lgId" DataSourceID="SqlDataSource3"> 
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                <AlternatingRowStyle CssClass="altrowstyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <RowStyle CssClass="rowstyle" />               
                                <PagerStyle CssClass="customGridpaging" />                                
                                <Columns>
                                    <asp:ButtonField CommandName="Select" ButtonType="Link" Text="Select" />
                                    <asp:BoundField DataField="lgId" HeaderText="lgId" Visible="false" InsertVisible="False" ReadOnly="True"
                                        SortExpression="lgId" />
                                    <asp:BoundField DataField="ltrType" HeaderText="Letter Type" SortExpression="ltrType" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="ltrTypeCode" HeaderText="Letter Code" SortExpression="ltrTypeCode" />
                                    <asp:BoundField DataField="tpVersion" HeaderText="Version" SortExpression="tpVersion" />                                    
                                    <asp:BoundField DataField="lgdate" HeaderText="Date" SortExpression="lgdate" />
                                    <asp:BoundField DataField="lgPrintBatchnum" HeaderText="PrintBatch" SortExpression="lgPrintBatchnum" />
                                    <asp:BoundField DataField="lgUser" HeaderText="User" SortExpression="lgUser" />
                                    
                                </Columns>
                            </asp:GridView>                            
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT dbo.hra_Ltrs_Generated.lgId, 
                                                CONVERT(VARCHAR(20),dbo.hra_Ltrs_Generated.lgdate,101) as [lgdate],
                                                dbo.hra_Ltrs_Generated.lgPrintBatchnum,dbo.hra_Ltrs_Generated.lgUser, 
                                                dbo.hra_Ltrs_Type.ltrType,
                                                dbo.hra_Ltrs_Type.ltrTypeCode,
                                                dbo.hra_Ltrs_template.tpVersion 
                                                FROM dbo.hra_Ltrs_Generated,dbo.hra_Ltrs_template,dbo.hra_Ltrs_Type
                                                WHERE dbo.hra_Ltrs_Generated.lgLetterId = dbo.hra_Ltrs_template.tpId 
                                                AND dbo.hra_Ltrs_template.tpTypeId = dbo.hra_Ltrs_Type.ltrId
                                                AND dbo.hra_Ltrs_Type.ltrTypeCode NOT IN ('HRB1', 'HRB2')">
                            </asp:SqlDataSource>
                        </div> 
                        <div class="error" id="errorDiv3" runat="server" visible="false">
                            <asp:Label ID="lbl_error2" runat="server" Text=""></asp:Label>
                        </div>
                        <div id="divEmp" visible="false" style="float:left;width:680px;margin:20px;" runat="server">
                            <div id= "subIntroText"> 
                                <asp:Label ID="lblEmployeeList" runat="server" Text="Select All or Employee number(s)" ForeColor="#5D478B"></asp:Label>
                            </div>	
                            <div style="float:left;width:650px;margin:20px;">
                                <table>
                                    <tr style="border-bottom:solid 1px #DEDEDE">
                                        <td style="width:200px;margin:10px;">
                                            <asp:Label ID="lblE1" runat="server" ForeColor="Blue" Text="Employee Numbers"></asp:Label>
                                        </td> 
                                        <td style="width:80px;margin:10px;">
                                        </td> 
                                        <td style="width:200px;margin:10px;">
                                            <asp:Label ID="lblE2" runat="server" ForeColor="Blue" Text="Selected Employee Number(s)"></asp:Label>
                                        </td> 
                                        <td style="width:50px;margin:10px;">
                                        </td>                                      
                                    </tr>
                                     <tr>
                                        <td style="width:200px;margin:10px;">
                                            <asp:ListBox ID="ListBox1" AutoPostBack="false" runat="server" Width="150px" Height="200px"
                                                DataTextField="lhEmpnum" DataValueField="lhgenId" SelectionMode="Multiple"></asp:ListBox>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"></asp:SqlDataSource>                                            
                                        </td> 
                                        <td valign="middle" style="width:80px;margin:10px;">
                                            <asp:Button Width="50px" ID="btn1" runat="server" Text=">>" ToolTip="Selects All" OnClick="btn1_Click" /><br /><br />
                                            <asp:Button ID="btn2" runat="server" Text=">" ToolTip="Select" Width="50px" OnClick="btn2_Click" /><br /><br />
                                            <asp:Button ID="btn3" runat="server" Text="<" ToolTip="DeSelect" Width="50px" OnClick="btn3_Click" /><br /><br />
                                            <asp:Button ID="btn4" runat="server" Text="<<" ToolTip="DeSelects All" Width="50px" OnClick="btn4_Click" /><br />
                                        </td> 
                                        <td style="width:200px;margin:10px;">
                                            <asp:ListBox ID="ListBox2" runat="server" Width="150px" Height="200px" SelectionMode="Multiple"></asp:ListBox>
                                        </td>
                                        <td style="width:50px;margin:10px;">
                                            <asp:RequiredFieldValidator ID="rfvEmpList" runat="server" ErrorMessage="(Required)"
                                                ControlToValidate="ListBox2" ValidationGroup="ltrrep"></asp:RequiredFieldValidator>
                                        </td>      
                                    </tr>
                                </table>
                                <table style="margin-top:10px">
                                    <tr>
                                        <td style="width:100px;margin:20px;">
                                        </td> 
                                        <td style="width:100px;margin:20px;">
                                            <asp:LinkButton ID="lnkBtnReprint" CssClass="imgbutton" runat="server" 
                                                ValidationGroup="ltrrep" OnClick="lnkrepPrint_Click"><span>Print</span></asp:LinkButton>
                                        </td> 
                                        <td style="width:100px;margin:20px;">
                                            <asp:LinkButton ID="lnkBtnReCancel" CssClass="imgbutton" runat="server" 
                                                 OnClick="lnkrepCancel_Click"><span>Cancel</span></asp:LinkButton>
                                        </td> 
                                        <td style="width:100px;margin:20px;">
                                            <asp:LinkButton ID="lnkBtnReClose" CssClass="imgbutton" runat="server" 
                                                 OnClick="lnkrepClose_Click"><span>Close</span></asp:LinkButton>
                                        </td>
                                        <td style="width:100px;margin:20px;">
                                        </td>                                        
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
        </div> 
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


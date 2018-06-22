<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Maintainence.aspx.cs" Inherits="ImputedIncome_Maintainence" Title="Imputed Income Maintainence" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">       
        <div id="Div1">        
            <div id = "module_menu">
                <h1>Imputed Income</h1>            
                <ul id="sitemap">
                    <li><asp:HyperLink ID="hypCalc" runat="server" NavigateUrl="~/ImputedIncome/Calculate.aspx">Calculate</asp:HyperLink></li>
                    <li><asp:HyperLink ID="hypMain" runat="server" Font-Underline="true" NavigateUrl="~/ImputedIncome/Maintainence.aspx">Maintenance</asp:HyperLink></li>
                </ul>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
            </div> 	
        </div>          
    </div>
    <div id="contentright"> 
        <div id = "introText" style="width:720px;padding-left:-10px;">Imputed Income Maintenance>          
        <div class = "Menu" style="width:730px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Rate" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Age Reduction" Value="1"></asp:MenuItem>
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
                        <p>Maintainence routine of Rate Factor for Pilot Imputed Income Calculation</p>                            
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel4" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_rate" runat="server" style ="width:650px; padding-top:15px; padding-left:5px;">
	                            <asp:Label ID="lbl_errRate" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;margin-bottom:15px;display:block;"></asp:Label>     
	                            <asp:GridView ID="grdvRate" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" 
	                            AutoGenerateColumns="False" DataSourceID="SqlDataSourceRate" OnRowUpdating="auditUpdateRate"
	                            ShowFooter="True">
				                    <AlternatingRowStyle CssClass="altrowstyle" />
				                    <HeaderStyle CssClass="headerstyle" />
				                    <RowStyle CssClass="rowstyle" />
				                    <FooterStyle CssClass="rowstyle" />                        
				                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
				                    <EmptyDataTemplate>
					                    <div class="userPara">
						                    <p>No Records</p>
					                    </div>
				                    </EmptyDataTemplate> 
				                    <Columns>
					                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="160px" FooterStyle-Width="160px">
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
							                    CommandName="Update"  Text="Update" ValidationGroup="EditsGrp"></asp:LinkButton> 
							                    <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" Width="50px"
							                    CommandName="Cancel" Text="Cancel"></asp:LinkButton>
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:LinkButton ID="lnkInsert" runat="server"  CommandName="Insert" OnClick="InsertRateFactor_Click" Width="50px"
							                    ValidationGroup = "InsertsGrp" CausesValidation="True">Insert</asp:LinkButton> 
							                    <asp:LinkButton ID="lnkReset" runat="server" CausesValidation="False" Width="50px"
							                    CommandName="Cancel" Text="Reset"></asp:LinkButton>
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="Effective Date" SortExpression="effdt" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_rateEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_rateEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>' style="display:block;"></asp:TextBox>
						                        <asp:Label ID="Label100" runat="server" Text="(mm/dd/yyyy)" ></asp:Label>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator17" runat="server"
							                        ControlToValidate="tbxe_rateEffYrmo"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator1" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_rateEffYrmo"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}"
                                                    Display="Dynamic" >
                                                    Enter in format "mm/dd/yyyy"
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_rateEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>' style="display:block;"></asp:TextBox>
							                    <asp:Label ID="Label60" runat="server" Text="(mm/dd/yyyy)" ></asp:Label>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
								                    ControlToValidate="tbxi_rateEffYrmo"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator2" 
								                    runat="server"
								                    ControlToValidate="tbxi_rateEffYrmo"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}"
								                    Display="Dynamic">
								                    Enter in format "mm/dd/yyyy"
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="From Age" SortExpression="frmAge" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_rateFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_rateFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>' style="display:block;"></asp:TextBox>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server"
							                        ControlToValidate="tbxe_rateFrmAge"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator15" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_rateFrmAge"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+$"
                                                    Display="Dynamic" >
                                                    Enter valid number
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_rateFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>' style="display:block;"></asp:TextBox>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" 
								                    ControlToValidate="tbxi_rateFrmAge"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator4" 
								                    runat="server"
								                    ControlToValidate="tbxi_rateFrmAge"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+$"
								                    Display="Dynamic">
								                    Enter valid number
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="To Age" SortExpression="toAge" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_rateToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_rateToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>' style="display:block;"></asp:TextBox>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator4" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_rateToAge"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+$"
                                                    Display="Dynamic" >
                                                    Enter valid number
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_rateToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>' style="display:block;"></asp:TextBox>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator5" 
								                    runat="server"
								                    ControlToValidate="tbxi_rateToAge"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+$"
								                    Display="Dynamic">
								                    Enter valid number
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="Rate Factor" SortExpression="factor" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_rateFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_rateFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>' style="display:block;"></asp:TextBox>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server"
							                        ControlToValidate="tbxe_rateFactor"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator6" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_rateFactor"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+(\.\d{0,4})?$"
                                                    Display="Dynamic" >
                                                    Enter valid number (max 4 decimal places)
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_rateFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>' style="display:block;"></asp:TextBox>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" 
								                    ControlToValidate="tbxi_rateFactor"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator7" 
								                    runat="server"
								                    ControlToValidate="tbxi_rateFactor"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+(\.\d{0,4})?$"
								                    Display="Dynamic">
								                    Enter valid number (max 4 decimal places)
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
				                    </Columns>
			                    </asp:GridView>
			                    <asp:SqlDataSource ID="SqlDataSourceRate" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
				                    SelectCommand= "SELECT [id],CONVERT(VARCHAR(15),[effdt],101) AS [effdt], [frmAge], [toAge], [factor] FROM [ImputedIncome_Main] WHERE ([sourcecd]= @sourcecode)"
				                    UpdateCommand="UPDATE [ImputedIncome_Main] SET [effdt]=@effdt, [frmAge]=@frmage, [toAge]=@toage, [factor]=@factor WHERE ([id]=@id) AND ([sourcecd]= @sourcecode)"
				                    InsertCommand="INSERT INTO [ImputedIncome_Main]([effdt],[frmAge],[toAge],[factor],[sourcecd]) VALUES(@effdt, @frmage, @toage, @factor, @sourcecode)"
				                    DeleteCommand="DELETE FROM [ImputedIncome_Main] WHERE ([id]=@id)">                                    
				                    <SelectParameters>
				                        <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RateFactor" />
				                    </SelectParameters>
				                    <UpdateParameters>                               
					                    <asp:Parameter Name="id" Type="Int32" />
					                    <asp:Parameter Name="effdt" Type="String" />
					                    <asp:Parameter Name="frmage" Type="Int32" />
					                    <asp:Parameter Name="toage" Type="Int32" />
					                    <asp:Parameter Name="factor" Type="Decimal" />
					                    <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RateFactor" />
				                    </UpdateParameters> 
				                    <InsertParameters>
				                        <asp:Parameter Name="effdt" Type="String" />
					                    <asp:Parameter Name="frmage" Type="Int32" />
					                    <asp:Parameter Name="toage" Type="Int32" />
					                    <asp:Parameter Name="factor" Type="Decimal" />
					                    <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RateFactor" />				                        
				                    </InsertParameters>      
			                    </asp:SqlDataSource>                      
	                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:View>
                <asp:View ID="View2" runat="server">
                    <div class="userPara">
                        <p>Maintainence routine of Age Reduction Factor for Pilot Imputed Income Calculation</p>                            
                    </div>
                    <asp:UpdatePanel runat="server" ID="updatePanel1" UpdateMode="always">
				        <ContentTemplate>
                            <div id="div_age" runat="server" style ="width:650px; padding-top:15px; padding-left:5px;">
                                <asp:Label ID="lbl_errAge" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;margin-bottom:15px;display:block;"></asp:Label>     
                                <asp:GridView ID="grdv_age" runat="server" DataKeyNames="id" CssClass="tablestyle" AllowSorting="True" 
	                            AutoGenerateColumns="False" DataSourceID="SqlDataSourceAge" OnRowUpdating="auditUpdateAge"
	                            ShowFooter="True">
				                    <AlternatingRowStyle CssClass="altrowstyle" />
				                    <HeaderStyle CssClass="headerstyle" />
				                    <RowStyle CssClass="rowstyle" />
				                    <FooterStyle CssClass="rowstyle" />                        
				                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
				                    <EmptyDataTemplate>
					                    <div class="userPara">
						                    <p>No Records</p>
					                    </div>
				                    </EmptyDataTemplate> 
				                    <Columns>
					                    <asp:TemplateField ShowHeader="False" ItemStyle-Width="160px" FooterStyle-Width="160px">
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
							                    <asp:LinkButton ID="lnkInsert" runat="server" CommandName="Insert" OnClick="InsertAgeFactor_Click" Width="50px"
							                    ValidationGroup = "InsertsGrp" CausesValidation="True">Insert</asp:LinkButton> 
							                    <asp:LinkButton ID="lnkReset" runat="server" CausesValidation="False" Width="50px"
							                    CommandName="Cancel" Text="Reset"></asp:LinkButton>
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="Effective Date" SortExpression="effdt" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_ageEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_ageEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>' style="display:block;"></asp:TextBox>
						                        <asp:Label ID="Label80" runat="server" Text="(mm/dd/yyyy)" ></asp:Label>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator9" runat="server"
							                        ControlToValidate="tbxe_ageEffYrmo"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator8" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_ageEffYrmo"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}"
                                                    Display="Dynamic" >
                                                    Enter in format "mm/dd/yyyy"
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_ageEffYrmo" Width="70px" runat="server" Text='<%# Bind("effdt") %>' style="display:block;"></asp:TextBox>
							                    <asp:Label ID="Label90" runat="server" Text="(mm/dd/yyyy)" ></asp:Label>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" 
								                    ControlToValidate="tbxi_ageEffYrmo"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator9" 
								                    runat="server"
								                    ControlToValidate="tbxi_ageEffYrmo"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}"
								                    Display="Dynamic">
								                    Enter in format "mm/dd/yyyy"
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="From Age" SortExpression="frmAge" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_ageFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_ageFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>' style="display:block;"></asp:TextBox>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator11" runat="server"
							                        ControlToValidate="tbxe_ageFrmAge"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator10" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_ageFrmAge"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+$"
                                                    Display="Dynamic" >
                                                    Enter valid number
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_ageFrmAge" Width="70px" runat="server" Text='<%# Bind("frmAge") %>' style="display:block;"></asp:TextBox>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" 
								                    ControlToValidate="tbxi_ageFrmAge"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator11" 
								                    runat="server"
								                    ControlToValidate="tbxi_ageFrmAge"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+$"
								                    Display="Dynamic">
								                    Enter valid number
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="To Age" SortExpression="toAge" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_ageToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_ageToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>' style="display:block;"></asp:TextBox>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator12" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_ageToAge"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+$"
                                                    Display="Dynamic" >
                                                    Enter valid number
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_ageToAge" Width="70px" runat="server" Text='<%# Bind("toAge") %>' style="display:block;"></asp:TextBox>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator13" 
								                    runat="server"
								                    ControlToValidate="tbxi_ageToAge"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+$"
								                    Display="Dynamic">
								                    Enter valid number
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
					                    <asp:TemplateField HeaderText="Reduction Factor" SortExpression="factor" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-Width="140px" FooterStyle-Width="140px">
						                    <ItemTemplate>
							                    <asp:Label ID="lbl_redFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>'></asp:Label>
						                    </ItemTemplate>
						                    <EditItemTemplate>
							                    <asp:TextBox ID="tbxe_redFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>' style="display:block;"></asp:TextBox>
						                        <asp:RequiredFieldValidator id="RequiredFieldValidator15" runat="server"
							                        ControlToValidate="tbxe_redFactor"
							                        ValidationGroup="EditsGrp"
							                        Display="Dynamic" Text="(Required)">
						                        </asp:RequiredFieldValidator>
						                        <asp:RegularExpressionValidator 
                                                    id="RegularExpressionValidator14" 
                                                    runat="server"
                                                    ControlToValidate="tbxe_redFactor"
                                                    ValidationGroup = "EditsGrp"
                                                    ValidationExpression="^\d+(\.\d{0,4})?$"
                                                    Display="Dynamic" >
                                                    Enter valid number (max 4 decimal places)
                                                </asp:RegularExpressionValidator> 
						                    </EditItemTemplate>
						                    <FooterTemplate>
							                    <asp:TextBox ID="tbxi_redFactor" Width="70px" runat="server" Text='<%# Bind("factor") %>' style="display:block;"></asp:TextBox>
							                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" 
								                    ControlToValidate="tbxi_redFactor"
								                    runat="server" Display="Dynamic"
								                    Text="(Required)"
								                    ValidationGroup = "InsertsGrp">
							                    </asp:RequiredFieldValidator>
							                    <asp:RegularExpressionValidator 
								                    id="RegularExpressionValidator15" 
								                    runat="server"
								                    ControlToValidate="tbxi_redFactor"
								                    ValidationGroup = "InsertsGrp"
								                    ValidationExpression="^\d+(\.\d{0,4})?$"
								                    Display="Dynamic">
								                    Enter valid number (max 4 decimal places)
							                    </asp:RegularExpressionValidator> 
						                    </FooterTemplate>
					                    </asp:TemplateField>
				                    </Columns>
			                    </asp:GridView>
			                    <asp:SqlDataSource ID="SqlDataSourceAge" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
				                    SelectCommand= "SELECT [id], CONVERT(VARCHAR(15),[effdt],101) AS [effdt], [frmAge], [toAge], [factor] FROM [ImputedIncome_Main] WHERE ([sourcecd]= @sourcecode)"
				                    UpdateCommand="UPDATE [ImputedIncome_Main] SET [effdt]=@effdt, [frmAge]=@frmage, [toAge]=@toage, [factor]=@factor WHERE ([id]=@id) AND ([sourcecd]= @sourcecode)"
				                    InsertCommand="INSERT INTO [ImputedIncome_Main]([effdt],[frmAge],[toAge],[factor],[sourcecd]) VALUES(@effdt, @frmage, @toage, @factor, @sourcecode)"
				                    DeleteCommand="DELETE FROM [ImputedIncome_Main] WHERE ([id]=@id)">                                    
				                    <SelectParameters>
				                        <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RedFactor" />
				                    </SelectParameters>
				                    <UpdateParameters>                               
					                    <asp:Parameter Name="id" Type="Int32" />
					                    <asp:Parameter Name="effdt" Type="String" />
					                    <asp:Parameter Name="frmage" Type="Int32" />
					                    <asp:Parameter Name="toage" Type="Int32" />
					                    <asp:Parameter Name="factor" Type="Decimal" />
					                    <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RedFactor" />
				                    </UpdateParameters> 
				                    <InsertParameters>
				                        <asp:Parameter Name="effdt" Type="String" />
					                    <asp:Parameter Name="frmage" Type="Int32" />
					                    <asp:Parameter Name="toage" Type="Int32" />
					                    <asp:Parameter Name="factor" Type="Decimal" />
					                    <asp:Parameter Name="sourcecode" Type="String" DefaultValue="RedFactor" />				                        
				                    </InsertParameters>                             
			                    </asp:SqlDataSource>                      
	                        </div>                            
                        </ContentTemplate>
                    </asp:UpdatePanel> 
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>


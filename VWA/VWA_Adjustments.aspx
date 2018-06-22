<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VWA_Adjustments.aspx.cs" Inherits="VWA_VWA_Adjustments" Title="Manual Adjustments" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
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
                <li><asp:HyperLink ID="hypVWAAdj" runat="server" Font-Underline="true" NavigateUrl="~/VWA/VWA_Adjustments.aspx">Adjustments</asp:HyperLink></li>
                <li><asp:HyperLink ID="hypVWACases" runat="server" NavigateUrl="~/VWA/VWA_Cases_Search.aspx">Individual Cases</asp:HyperLink></li>
                <li><a>Maintainence</a>
                    <ul>
                        <li><asp:HyperLink ID="hypMainRpts" runat="server" NavigateUrl="~/VWA/Maintainence/Maintainence_Reports.aspx">Reports</asp:HyperLink></li>                    
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div> 	
    </div>
    <div id="contentright">
        <div id = "introText"> Adjustments </div>
        <asp:UpdatePanel runat="server" ID="updatePanel1" UpdateMode="always">
        <ContentTemplate>
         <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller" id="grdvAdjscroller" runat="server">
            <asp:GridView ID="grdvAdjustments" runat="server" 
            AutoGenerateColumns="False"  DataSourceID="SqlDataSource1"
            GridLines="None" AllowSorting="true" DataKeyNames="Id"
            OnRowDeleting="grdvAdjustments_rowDeleting" OnRowUpdating="grdvAdjustments_RowUpdating"
            CssClass="tablestyle"  PagerSettings-Mode="NumericFirstLast"
            PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
            PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
            PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
            PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif">
            <PagerSettings Mode="NextPreviousFirstLast" />
            <AlternatingRowStyle CssClass="altrowstyle" />
            <HeaderStyle CssClass="headerstyle" />
            <RowStyle CssClass="rowstyle" />               
            <PagerStyle CssClass="customGridpaging" />
                <Columns>                    
                    <asp:TemplateField ShowHeader="false">
                        <ItemTemplate>
                            <asp:LinkButton id="lnkEdit" Text="Edit" CommandName="Edit" ForeColor="#282828" Runat="server" /> 
                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" Text="Delete" ForeColor="#282828" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lbUpdate" runat="server" CausesValidation="True"  ValidationGroup="edit"
                            CommandName="Update" Text="Update" ></asp:LinkButton> 
                            <asp:LinkButton ID="lbCancel" runat="server" CausesValidation="False" 
                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="Id" Visible = "false" />
                    <asp:TemplateField HeaderText="Contract#" SortExpression="ContractNum">                                
                        <EditItemTemplate>                              
                            <asp:Label ID="txtCnum" runat="server" Text='<%# Bind("ContractNum") %>' Width="50px"></asp:Label> 
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCnum" runat="server" 
                            Text='<%# Bind("ContractNum") %>' Width="50px"></asp:Label>
                        </ItemTemplate>                          
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="YRMO" SortExpression="yrmo">                                
                        <EditItemTemplate>                              
                            <asp:Label ID="txtYrmo" runat="server" Text='<%# Bind("yrmo") %>' Width="50px"></asp:Label> 
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblYrmo" runat="server" 
                            Text='<%# Bind("yrmo") %>' Width="50px"></asp:Label>
                        </ItemTemplate>                         
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="AdjustmentType" SortExpression="AdjType">                                
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlAdjType" runat="server" SelectedValue='<%# Bind("AdjType") %>'
                             Width="100px">
                                <asp:ListItem Value="--Select--" />
                                <asp:ListItem Value="Remittance" />
                                <asp:ListItem Value="Bank Statement" />
                                <asp:ListItem Value="Detail File" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvAdjTypeA" 
                                ControlToValidate="ddlAdjType"
                                runat="server" Display="Dynamic"
                                Text="(Required)"
                                ValidationGroup="add"
                                InitialValue="--Select--">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGrp" runat="server" 
                            Text='<%# Bind("AdjType") %>' Width="80px"></asp:Label>
                        </ItemTemplate>                       
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount">                                
                        <EditItemTemplate>                              
                            <asp:TextBox ID="txtAmount" runat="server" Text='<%# Bind("Amount") %>' Width="80px"></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="rfvAmount" 
                                ControlToValidate="txtAmount"
                                runat="server" Display="Dynamic"
                                Text="(Required)"
                                ValidationGroup="edit">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regvAmount"
                                ControlToValidate="txtAmount"
                                runat="server" Display="Dynamic"
                                Text="#,000.00" ValidationExpression="^-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup="edit">
                            </asp:RegularExpressionValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server" 
                            Text='<%# Bind("Amount") %>' Width="80px"></asp:Label>
                        </ItemTemplate>                          
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notes" SortExpression="Notes">                                
                        <EditItemTemplate>                              
                            <asp:TextBox ID="txtNotes" TextMode="multiLine" Rows = "5" runat="server" Text='<%# Bind("Notes") %>' Width="80px"></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="rfvNotes" 
                                ControlToValidate="txtNotes"
                                runat="server" Display="Dynamic"
                                Text="(Required)"
                                ValidationGroup="edit">
                            </asp:RequiredFieldValidator> 
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNotes" runat="server" 
                            Text='<%# Bind("Notes") %>' Width="80px"></asp:Label>                             
                        </ItemTemplate>                          
                    </asp:TemplateField>                           
                </Columns>                
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                  ConnectionString="<%$ ConnectionStrings:EBADB %>"
                  SelectCommand="SELECT [Id],[ContractNum],[yrmo],[AdjType], CONVERT(NUMERIC(10,2),[Amount]) AS [Amount],[Notes]
                                 FROM [dbo].[VWA_Adjustments] ORDER BY [yrmo] DESC, [ContractNum] ASC"                  
                  UpdateCommand ="UPDATE [dbo].[VWA_Adjustments] 
                                  SET [AdjType] = @AdjType, [Amount] = @Amount,[Notes] = @Notes
                                  WHERE ID = @Id"
                  DeleteCommand="DELETE FROM [dbo].[VWA_Adjustments] WHERE ID = @Id">               
                <UpdateParameters>
                    <asp:Parameter Name="Id" Type="int32" />
                    <asp:Parameter Name="AdjType" Type="String" />
                    <asp:Parameter Name="Amount" Type="Decimal" />
                    <asp:Parameter Name="Notes" Type="String" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="int32" />
                </DeleteParameters>
            </asp:SqlDataSource>  
        </div> 
        <ajaxscroll:PersistentScrollPosition ID="PersistentScrollPosition1" runat="server" ControlToPersist="grdvAdjscroller" />   
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <div style="float:left;width:650px;margin-left:20px;margin-top:20px;">
            <asp:LinkButton ID="lnkButtonNew" runat="server" OnClick="insertAdj">Insert new Adjustment</asp:LinkButton>
        </div>
        <br class = "br_e" />
        <div class="error" id="errorDiv1" runat="server" visible="false">
            <asp:Label ID="lblError1" runat="server" Text=""></asp:Label>
        </div>         
        <div style="float:left;width:650px;">
            <asp:FormView ID="frmAdj" Visible="false" runat="server" DataSourceID="SqlDataSource2" 
            DefaultMode="Insert" OnItemInserted="frmAddAdj_ItemInserted"  OnItemCommand="frmAddAdj_ItemCommand">
                <InsertItemTemplate>
                    <br class = "br_e" />
                    <asp:Label ID="lblCnumA" runat="server" Text="Contract#:" CssClass="labelHra"></asp:Label>
			        <asp:TextBox ID="txtCnumA" runat="server" Text='<%# Bind("ContractNum") %>'  CssClass="inputHra1"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvCnumA" 
                        ControlToValidate="txtCnumA"
                        runat="server" Display="Dynamic"
                        Text="(Required)"
                        ValidationGroup="add1">
                    </asp:RequiredFieldValidator>			
        			<br class = "br_e" />
        			<asp:Label ID="lblYrmo" runat="server" Text="Yrmo:" CssClass="labelHra"></asp:Label>
			        <asp:TextBox ID="txtYrmoA" runat="server" Text='<%# Bind("yrmo") %>' CssClass="inputHra1"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvYrmoA" 
                        ControlToValidate="txtYrmoA"
                        runat="server" Display="Dynamic"
                        Text="(Required)"
                        ValidationGroup="add1">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regvYrmoA"
                        ControlToValidate="txtYrmoA"
                        runat="server" Display="Dynamic"
                        Text="* YYYYMM" ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                        ValidationGroup="add1">
                    </asp:RegularExpressionValidator>
        			<br class = "br_e" />
        			<asp:Label ID="lblAdjType" runat="server" Text="Adjustment Type:" CssClass="labelHra"></asp:Label>
		            <asp:DropDownList ID="ddlAdjTypeA" runat="server" CssClass="dropHra" Width="150px" SelectedValue='<%# Bind("AdjType") %>'>
                        <asp:ListItem Selected="true" Value="Remittance" />
                        <asp:ListItem Value="Bank Statement" />
                        <asp:ListItem Value="Detail File" />
                    </asp:DropDownList>
        			<br class = "br_e" />
        			<asp:Label ID="lblAmount" runat="server" Text="Amount:" CssClass="labelHra"></asp:Label>
		            <asp:TextBox ID="txtAmount" runat="server" Text='<%# Bind("Amount") %>' CssClass="inputHra1"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvAmountA" 
                        ControlToValidate="txtAmount"
                        runat="server" Display="Dynamic"
                        Text="(Required)"
                        ValidationGroup="add1">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regvAmountA"
                        ControlToValidate="txtAmount"
                        runat="server" Display="Dynamic"
                        Text="#,000.00" ValidationExpression="^-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                        ValidationGroup="add1">
                    </asp:RegularExpressionValidator>
        			<br class = "br_e" />
        			<asp:Label ID="lblNotes" runat="server" Text="Notes:" CssClass="labelHra"></asp:Label>
		            <asp:TextBox ID="txtNotesA" runat="server" Text='<%# Bind("Notes") %>' CssClass="inputHra1" TextMode="multiLine" Rows = "5"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvNotesA" 
                        ControlToValidate="txtNotesA"
                        runat="server" Display="Dynamic"
                        Text="(Required)"
                        ValidationGroup="add1">
                    </asp:RequiredFieldValidator>  
			        <br class = "br_e" />
			        <asp:Button 
                        id="btnAdd"
                        Text="Insert"                                
                        CommandName="Insert"
                        ValidationGroup="add1"
                        Runat="server" 
                        style="width:80px;margin-left:100px;margin-top:20px;margin-bottom:20px;"/>                    
                    <asp:Button 
                        id="btnACancel"
                        Text="Cancel"                                 
                        CausesValidation="false"
                        CommandName="Cancel"                                           
                        Runat="server" 
                        style="width:80px;margin-left:20px;margin-top:20px;margin-bottom:20px;"/>
                </InsertItemTemplate>
            </asp:FormView>
             <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                  ConnectionString="<%$ ConnectionStrings:EBADB %>"                 
                  InsertCommand="INSERT INTO [dbo].[VWA_Adjustments] (ContractNum,yrmo,AdjType,Amount,Notes)
                                 VALUES (@ContractNum,@yrmo,@AdjType,@Amount,@Notes)">                 
                <InsertParameters>
                    <asp:Parameter Name="ContractNum" Type="int32" />
                    <asp:Parameter Name="yrmo" Type="String" />
                    <asp:Parameter Name="Amount" Type="Decimal" />
                    <asp:Parameter Name="Notes" Type="String" />
                </InsertParameters>                
            </asp:SqlDataSource>  
        </div>
    </div>
</asp:Content>


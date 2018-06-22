<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VWA_Cases.aspx.cs" Inherits="VWA_VWA_Cases" Title="View Vengroff Williams Cases" %>
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
                <li><asp:HyperLink ID="hypVWACases" runat="server" Font-Underline="true" NavigateUrl="~/VWA/VWA_Cases_Search.aspx">Individual Cases</asp:HyperLink></li>
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
        <div style="float:left;width:640px; margin-top:10px; padding-bottom:5px;">
            <table>
                <tr>
                    <td style="padding-top:20px; padding-left:12px;">
                        <asp:LinkButton ID="lnk_discp" runat="server" OnClick="lnk_genCaseRpt_OnClick" Text="VWA Individual Case Report" ForeColor="#5D478B" style="background-image:url(../styles/images/page_excel.png); background-color:Transparent; background-position:left; padding-left:20px; padding-top:2px; background-repeat:no-repeat;"></asp:LinkButton>  
                    </td>  
                </tr>
            </table>
        </div>
        <div class="error" id="errorDiv1" runat="server" visible="false">
            <asp:Label ID="lblError1" runat="server" Text=""></asp:Label>
        </div>  
        <br class = "br_e" /> 
        <div class = "Menu" style="width:730px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Contract" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Financial" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="History" Value="2"></asp:MenuItem>
                    <asp:MenuItem Text="Notes" Value="3"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView" style="padding-bottom:15px; padding-top:10px; width:730px;">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">                
                <asp:View ID="View1" runat="server">                                         
                    <asp:Label ID="lblCon" runat="server" Text="Contract#:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtCon" runat="server" CssClass="inputHra1" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblClient" runat="server" Text="Client:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtClient" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox> 
                    <asp:TextBox ID="txtClientName" runat="server" CssClass="inputHra0" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblSSN" runat="server" Text="SSN:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtSSN" runat="server" CssClass="inputHra1" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblGroup" runat="server" Text="Group:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtGroup" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>  
                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="inputHra0" ReadOnly="true"></asp:TextBox>                   
                    <br class = "br_e" />  
                    <asp:Label ID="lblName" runat="server" Text="Name:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="inputHra0" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblPatient" runat="server" Text="Patient:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtPatient" runat="server" CssClass="inputHra0" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblRelation" runat="server" Text="Relationship:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtRelation" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblDisabled" runat="server" Text="Disabled:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtDisabled" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblStatusCd" runat="server" Text="StatusCd:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtStatusCd1" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>  
                    <asp:TextBox ID="txtStatusCd2" runat="server" CssClass="inputHra0" ReadOnly="true"></asp:TextBox>                  
                    <br class = "br_e" />  
                    <asp:Label ID="lblAcciDt" runat="server" Text="Accident Dt:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtAcciDt" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblRecoveryDt" runat="server" Text="Recovery Dt:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtRecoveryDt" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblOpenDt" runat="server" Text="Open Dt:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtOpenDt" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblCloseDt" runat="server" Text="Close Dt:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtCloseDt" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />  
                    <asp:Label ID="lblLastUpdate" runat="server" Text="Last Update:" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtLastUpdate" runat="server" CssClass="inputHra2" ReadOnly="true"></asp:TextBox>                    
                    <br class = "br_e" />        
                </asp:View>
                <asp:View ID="View2" runat="server"> 
                <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                    <asp:GridView ID="grdvFinancial1" runat="server" 
                    AutoGenerateColumns="False" 
                    GridLines="None"  AllowSorting="true" 
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
                            <asp:BoundField HeaderText="Total Benefit" DataField="Total Benefit" SortExpression="Total Benefit"/>
                            <asp:BoundField HeaderText="Total Recovery" DataField="Total Recovery" SortExpression="Total Recovery" />
                            <asp:BoundField HeaderText="Total Fees" DataField="Total Fees" SortExpression ="Total Fees" />
                            <asp:BoundField HeaderText="Total Net" DataField="Total Net" SortExpression ="Total Net" />                              
                        </Columns>                
                    </asp:GridView>                    
                </div>
                <div id = "introText"></div>    
                 <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                    <asp:GridView ID="grdvFinancial2" runat="server" 
                    AutoGenerateColumns="False" 
                    GridLines="None"  AllowSorting="true" DataSourceID="SqlDataSource1"
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
                        <asp:BoundField HeaderText="YRMO" DataField="YRMO" SortExpression ="YRMO" />                              
                            <asp:BoundField HeaderText="Benefits" DataField="Benefits" SortExpression="Benefits" DataFormatString="{0:c}"/>
                            <asp:BoundField HeaderText="Recovery" DataField="Recovery" SortExpression="Recovery" DataFormatString="{0:c}"/>
                            <asp:BoundField HeaderText="Fees" DataField="Fees" SortExpression ="Fees" DataFormatString="{0:c}"/>
                            <asp:BoundField HeaderText="Net Amount" DataField="Net Amount" SortExpression ="Net Amount" DataFormatString="{0:c}"/>     
                            <asp:BoundField HeaderText="System Date" DataField="System Date" SortExpression ="System Date" />                              
                            <asp:BoundField HeaderText="Recon Dt" DataField="Recon Date" SortExpression ="Recon Date" />                                                       
                        </Columns>                
                    </asp:GridView>                    
                </div> 
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                    SelectCommand="sp_VWA_ContractDetails" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>         
                </asp:View>
                <asp:View ID="View3" runat="server"> 
                    <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                    <asp:GridView ID="grdvHistory" runat="server" 
                    AutoGenerateColumns="False" 
                    GridLines="None"  AllowSorting="true" 
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
                            <asp:BoundField HeaderText="Name" DataField="Name" SortExpression ="Name" />                              
                            <asp:BoundField HeaderText="Old Value" DataField="Old Value" SortExpression="Old Value"/>
                            <asp:BoundField HeaderText="New Value" DataField="New Value" SortExpression="New Value"/> 
                            <asp:BoundField HeaderText="System Dt" DataField="SysDt" SortExpression ="SysDt" />                              
                                                                                
                        </Columns>                
                    </asp:GridView>                    
                </div> 
                </asp:View>
                <asp:View ID="View4" runat="server"> 
                    <asp:Label ID="lblNotes" runat="server" Text="Notes:" CssClass="labelHra" AssociatedControlID="txtNotes"></asp:Label>
                    <asp:TextBox ID="txtNotes" runat="server" Width="550px" Rows="5" TextMode="multiline" CssClass="inputHra"></asp:TextBox>
                    <br class = "br_e" />  
                    <asp:Button ID="btnNotes" runat="server" Text="Submit" OnClick="btnNotes_OnClick"
                    ValidationGroup="add" style="Margin-left:120px; margin-top:20px;" />
                    <asp:Label ID="lbl_result" Visible="false" runat="server" Text="Succesfully Submitted!" style="background-image:url(../styles/images/success.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat; margin-left:100px; color:#006633;margin-bottom:20px;"></asp:Label>
                    <br class = "br_e" />
                </asp:View>
            </asp:MultiView>
        </div>
   </div>
</asp:Content>


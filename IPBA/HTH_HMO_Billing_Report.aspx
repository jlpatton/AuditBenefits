<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HTH_HMO_Billing_Report.aspx.cs" Inherits="HTH_HTH_HMO_Billing_Report" Title="Generate HTH/HMO Billing Report" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>

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
                <li><asp:HyperLink ID="hypHMOReport" runat="server" Font-Underline="true" NavigateUrl="~/IPBA/HTH_HMO_Billing_Report.aspx">HTH/HMO Billing Report</asp:HyperLink></li> 
                <li><asp:HyperLink ID="hypMaintenance" runat="server" NavigateUrl="~/IPBA/Maintenance.aspx">Maintenance</asp:HyperLink></li>          		                     		            
            </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
    </div>
	<div id="contentright">
	  <div id="form1">	  
        <fieldset>
            <legend>Create HTH/HMO Billing Report</legend>
                <div>
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label><br /><br />
                    <asp:Label ID="lblPrevYrmo" runat="server" Text="YRMO:" style="margin-right:3px;display:block;float:left; width:60px;"></asp:Label>
                    <asp:DropDownList ID="ddlYrmo" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO_selectedIndexchanged">                        
                    </asp:DropDownList>
                    <asp:TextBox ID="txtPrevYRMO" runat="server" Visible="false" ></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" ForeColor="#7D7D7D"
                    Text="(yyyymm)"></asp:Label>
                    <asp:Button ID="btnAddYrmo" runat="server" Text="ADD" ValidationGroup="yrmoGroup" OnClick="btn_ADDYrmo" Visible = "false"/>
                    <asp:Button ID="btnCancelYrmo" runat="server"  Text="Cancel" OnClick="btn_CancelYrmo" Visible = "false" />
                    <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                    ControlToValidate="txtPrevYRMO"
                    Display="Dynamic" 
                    Font-Names="Verdana" Font-Size="10pt" ValidationGroup="yrmoGroup"
                    >
                    Please enter YRMO
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"
                    ControlToValidate="txtPrevYRMO"
                    ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                    Display="Static"
                    Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                    Please enter YRMO in format 'yyyymm'
                    </asp:RegularExpressionValidator><br /><br />
                    <asp:Label ID="lblPlancd" runat="server" Text="HTH/HMO:" style="margin-right:3px; display:block;float:left; width:60px;"></asp:Label>
                    <asp:DropDownList ID="ddlPlancd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlancd_selectedIndexchanged"
                    DataSourceID="SqlDataSourcePlancd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true">  
                    <asp:ListItem Selected="True">--All--</asp:ListItem>                            
                    </asp:DropDownList>                    
                    <br />
                    <asp:Button ID="btn_submit" runat="server" Text="Generate Reports" ValidationGroup="q1"
                    OnClick="btn_submit_Click" style="margin-left:60px;margin-top:20px;margin-bottom:27px;"  />                     
                </div>                                     
            </fieldset> 
        </div>
        <div id = "resultDiv" runat="server" visible="false" >
            <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">
                         <asp:Image ID="imgSum" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                <asp:LinkButton ID="lnkSum" runat="server" OnClick="lnkSum_OnClick" Font-Underline="false" ForeColor="#5D478B">View HTH/HMO Billing Summary Report</asp:LinkButton>
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genSumRpt" runat="server" OnClick="lnk_genSumRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div> 
            <div id = "SumRptDiv" runat="server" visible="false">                
               <div class="scroller" style ="width:700px; max-height:400px;">                    
                 <asp:GridView ID="grdvSum" runat="server" CssClass="tablestyle"
                    AutoGenerateColumns="false"  AllowSorting="true" 
                    OnSorting="grdvSum_Sorting">
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />                        
                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                    <EmptyDataTemplate>
                        No records.
                    </EmptyDataTemplate> 
                     <Columns>
                        <asp:BoundField DataField="PlanYr" HeaderText="PlanYr" SortExpression="PlanYr" />
                        <asp:BoundField DataField="Classification" HeaderText="Classification" SortExpression="Classification" />
                        <asp:BoundField DataField="Coverage Tier" HeaderText="Coverage Tier" SortExpression="Coverage Tier" />
                        <asp:BoundField DataField="Emps" HeaderText="Emps" SortExpression="Emps" />
                        <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" DataFormatString="{0:C}"  />
                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C}"/>
                    </Columns>                                           
                    </asp:GridView>
                </div>
            </div>
            <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">
                         <asp:Image ID="imgDet" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                <asp:LinkButton ID="lnkDet" runat="server" OnClick="lnkDet_OnClick" Font-Underline="false" ForeColor="#5D478B">View HTH/HMO Billing Detail Report</asp:LinkButton>
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genDetRpt" runat="server" OnClick="lnk_genDetRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div> 
            <div id = "DetRptDiv" runat="server" visible="false">                
               <div class="scroller" style ="width:700px; max-height:400px;">                    
                 <asp:GridView ID="grdvDet" runat="server" CssClass="tablestyle"
                    AutoGenerateColumns="false"  AllowSorting="true" 
                    OnSorting="grdvDet_Sorting">
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />
                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                    <EmptyDataTemplate>
                        No records.
                    </EmptyDataTemplate> 
                     <Columns>
                        <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" ItemStyle-Width="90" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Tier" HeaderText="Tier" SortExpression="Tier" />
                        <asp:BoundField DataField="Eff Dt" HeaderText="Eff Dt" SortExpression="Eff Dt" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                        <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" DataFormatString="{0:C}" />
                    </Columns>                          
                    </asp:GridView>
                </div>
            </div>
               <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">
                         <asp:Image ID="imgAdj" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                <asp:LinkButton ID="lnkAdj" runat="server" OnClick="lnkAdj_OnClick" Font-Underline="false" ForeColor="#5D478B">View HTH/HMO Billing Adjustment Report</asp:LinkButton>
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genAdjRpt" runat="server" OnClick="lnk_genAdjRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div> 
            <div id = "AdjRptDiv" runat="server" visible="false">                
               <div class="scroller" style ="width:700px; max-height:400px;">                    
                 <asp:GridView ID="grdvAdj" runat="server" CssClass="tablestyle"
                    AutoGenerateColumns="false"  AllowSorting="true" 
                    OnSorting="grdvAdj_Sorting"> 
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />
                     <Columns>
                        <asp:BoundField DataField="Py" HeaderText="Py" SortExpression="Py" />
                        <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" ItemStyle-Width="80" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                        <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                        <asp:BoundField DataField="Months" HeaderText="Months" SortExpression="Months" />
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:C}"/>
                        <asp:BoundField DataField="Flag" HeaderText="Flag" SortExpression="Flag" ItemStyle-ForeColor="Red" />
                    </Columns>    
                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                    <EmptyDataTemplate>
                        No records.
                    </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <div id="HTHAnthemDiv" runat="server" visible="true">
              <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">
                         <asp:Image ID="imgHTH" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/>  
                <asp:LinkButton ID="lnkHTH" runat="server" OnClick="lnkHTH_OnClick" Font-Underline="false" ForeColor="#5D478B">View HTH International Report for Anthem module</asp:LinkButton>
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnkgenHTHRpt" runat="server" OnClick="lnk_genHTHRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
            </div> 
            <div id = "HTHRptDiv" runat="server" visible="false"> 
            <div class="scroller" style ="width:700px; max-height:400px;">                  
               <div style="padding-top:20px;">                    
                 <asp:GridView ID="grdvHTH" runat="server" CssClass="tablestyle"
                    AutoGenerateColumns="false"  AllowSorting="true" 
                    OnSorting="grdvHTH_Sorting">
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />
                        <PagerStyle CssClass="customGridpaging" />
                        <FooterStyle CssClass="rowstyle" />
                     <Columns>
                        <asp:BoundField DataField="progcd" HeaderText="progcd" SortExpression="progcd" />
                        <asp:BoundField DataField="ptype" HeaderText="ptype" SortExpression="Type" />
                        <asp:BoundField DataField="ssno" HeaderText="ssno" SortExpression="ssno" ItemStyle-Width="90"/>
                        <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                        <asp:BoundField DataField="tiercd" HeaderText="tiercd" SortExpression="tiercd" />
                        <asp:BoundField DataField="effdt" HeaderText="effdt" SortExpression="effdt" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false" />
                        <asp:BoundField DataField="rate" HeaderText="rate" SortExpression="rate" DataFormatString="{0:C}" />
                    </Columns>    
                    <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                    <EmptyDataTemplate>
                        No records.
                    </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    </div>
        <div id = "AllRptDiv" runat="server" visible="false"> 
         <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">  
                        <font color="#5D478B">All HTH/HMO Billing Summary Report</font> 
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genSumRptAll" runat="server" OnClick="lnk_genSumRptAll_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">  
                        <font color="#5D478B">All HTH/HMO Billing Detail Report</font> 
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genDetRptAll" runat="server" OnClick="lnk_genDetRptAll_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div> 
        <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">  
                        <font color="#5D478B">All HTH/HMO Billing Adjustment Report</font> 
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genAdjRptAll" runat="server" OnClick="lnk_genAdjRptAll_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
           <div id = "hraIntroText"> 
            <table>
                <tr>
                    <td style="width:550px;margin:20px;">  
                        <font color="#5D478B">HTH International Report for Anthem module</font> 
                    </td>
                    <td style="margin:20px;">
                        <asp:LinkButton ID="lnk_genHTHRpt" runat="server" OnClick="lnk_genHTHRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
    
    <asp:SqlDataSource ID="SqlDataSourcePlancd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
        <SelectParameters>
            <asp:Parameter DefaultValue="HMOBILLRPT" Name="source" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>                  
</asp:Content>


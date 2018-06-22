<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="HMO_and_HTH_Adjustments.aspx.cs" Inherits="HTH_HMO_and_HTH_adjustments" Title="Manual Adjustments" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="DivScroller" Namespace="DivScroller" TagPrefix="cc2" %>

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
                <li><asp:HyperLink ID="hypManAdj" runat="server" Font-Underline="true" NavigateUrl="~/IPBA/HMO_and_HTH_Adjustments.aspx">Billing Adjustments</asp:HyperLink></li>		                
                <li><asp:HyperLink ID="hypHMOReport" runat="server" NavigateUrl="~/IPBA/HTH_HMO_Billing_Report.aspx">HTH/HMO Billing Report</asp:HyperLink></li> 
                <li><asp:HyperLink ID="hypMaintenance" runat="server" NavigateUrl="~/IPBA/Maintenance.aspx">Maintenance</asp:HyperLink></li>          		                     		            
            </ul> 
		    <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                        
		</div>			
	</div>
	<div id="contentright">
	  <div id="form1">	  
        <fieldset>
            <legend style="font-family: Georgia, Verdana, Arial, sans-serif; font-size:16px">HTH International and Local HMOs Adjustments</legend>
                <div style="margin-top:5px;">
                    <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
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
                    </asp:RegularExpressionValidator>
                </div>
                 <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                    <asp:View id="view_main" runat="server">
                    <br />
                    <asp:Label ID="lblProgcd" runat="server" Text="Program:" style="margin-right:3px;display:block;float:left; width:60px;"></asp:Label>
                    <asp:DropDownList ID="ddlProgcd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProgcd_selectedIndexchanged"
                    DataSourceID="SqlDataSourceProgcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true">    
                    <asp:ListItem Selected="True">--All--</asp:ListItem>                    
                    </asp:DropDownList>
                    <br /><br />
                    <asp:Label ID="lblHMO" runat="server" Text="HTH/HMO:" style="margin-right:3px; display:block;float:left; width:60px;"></asp:Label>
                    <asp:DropDownList ID="ddlHMO" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHMO_selectedIndexchanged"
                    DataSourceID="SqlDataSourceHMOcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true">  
                    <asp:ListItem Selected="True">--All--</asp:ListItem>                            
                    </asp:DropDownList>
                    <table>
                        <tr>                           
                            <td style="padding-top:10px;padding-left:600px;">
                                <asp:LinkButton ID="lnk_genAdjRpt" runat="server" OnClick="lnk_genAdjRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <div style="padding-top:3px;width:700px; max-height:250px;"  class="scroller" id="divscroll">                    
                     <asp:GridView ID="grdvAdj" runat="server" CssClass="GridViewStyle" DataSourceID="SqlDataSourceAdj"
                        AutoGenerateColumns="false"  DataKeyNames="SeqNum" AllowSorting="true" AllowPaging="false"
                        BorderColor="White" BackColor="White" OnRowCommand="grdvAdj_RowCommand"
                        PagerSettings-Mode="NumericFirstLast" 
                        OnRowDeleting="grdvAdj_rowDeleting"
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif">                         
                        <PagerStyle CssClass="PagerStyle" />
                        <AlternatingRowStyle CssClass="AltRowStyle" />
                        <HeaderStyle CssClass="HeaderStyle" />
                        <RowStyle CssClass="RowStyle" />                                              
                        <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton id="lnkEdit" Text="Edit" CommandName="Select" ForeColor="#282828" Runat="server" /> 
                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" Text="Delete" ForeColor="#282828" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SeqNum" HeaderText="Num" Visible="False"></asp:BoundField>
                        <asp:BoundField DataField="progcd" HeaderText="Program" SortExpression="progcd" />
                        <asp:BoundField DataField="PlanCode" HeaderText="Plan" SortExpression="PlanCode"/>
                        <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" ItemStyle-Width="80px"/>
                        <asp:BoundField DataField="typecd" HeaderText="Type" SortExpression="typecd" />
                        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="eventcd" HeaderText="Event" SortExpression="eventcd" />
                        <asp:BoundField DataField="trancd" HeaderText="Tran" SortExpression="trancd" />
                        <asp:BoundField DataField="CoverageEffDt" HeaderText="Eff Date" SortExpression="CoverageEffDt" dataformatstring="{0:MM/dd/yyyy}" HtmlEncode="false"/>
                        <asp:BoundField DataField="Comments" HeaderText="Notes" SortExpression="Comments" ItemStyle-Width="90px"/>
                        </Columns>
                        <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                        <EmptyDataTemplate>
                            No records.
                        </EmptyDataTemplate>                         
                        </asp:GridView>
                        <cc2:DivScroller ID="DivScroller1" runat="server" DivID="divscroll">
                        </cc2:DivScroller>
                        </div> 
                        <div style="padding-top:20px;">
                            <asp:LinkButton ID="btnNewAdj" runat="server" Text="Add New Adjustment.." OnClick="btnNewAdj_Click" ForeColor="#5d478b" Visible="true"/>
                        </div>                     
                     </asp:View>
                    <asp:View ID="view_empty" runat="server">
                        <div class="userParaI">
                            <p>Please enter the YRMO not listed in the drop down list.</p>    
                            <br />
                        </div>                         
                    </asp:View>                    
                </asp:MultiView>
                </fieldset>
            <div style="margin-top:25px;width:650px;">    
                <asp:Label ID="lbl_error1" runat="server" ForeColor="Red"></asp:Label><br/>         
                <asp:FormView id="frmAddAdj" DataSourceID="SqlDataSourceAddAdj"  Visible="false" DataKeyNames="SeqNum" runat="server" DefaultMode="Insert" OnItemInserting="frmAddAdj_ItemInserting" OnItemInserted="frmAddAdj_ItemInserted" OnItemCommand="frmAddAdj_ItemCommand" >
                    <InsertItemTemplate>
                        <div id="hraSubIntroText" style="color:#5D478B; border-bottom:2px solid #CDC9C9; margin-bottom:2px;font-size:16px;">New Adjustment</div>
                        <div class="webformIPBA" style="background-color:#F7F7F7;">                        
                        <asp:Label
                            id="lblAyrmo" Text="YRMO:" AssociatedControlID="txtAYRMO" runat="server" />                                
                        <asp:TextBox
                            id="txtAYRMO"  
                            Text='<%# Bind("YRMO") %>'                                  
                            Runat="server" />
                        <asp:Label ID="Label1" runat="server" ForeColor="#7D7D7D"
                        Text="(yyyymm)"></asp:Label>
                        <asp:RequiredFieldValidator
                            id="reqvalAYrmo" ControlToValidate="txtAYRMO"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            Runat="server" /> 
                        <asp:RegularExpressionValidator 
                            id="RegularExpressionValidator1" 
                            runat="server"
                            ControlToValidate="txtAYRMO"
                            ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                            Display="dynamic" ValidationGroup="add1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid YRMO in format YYYYMM
                        </asp:RegularExpressionValidator>    
                        <br />
                        <asp:Label ID="lblAProgcd" runat="server" Text="Program:" AssociatedControlID="ddlAProgcd"></asp:Label>                               
                        <asp:DropDownList ID="ddlAProgcd" runat="server"
                        DataSourceID="SqlDataSourceProgcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("progcd") %>'>    
                        <asp:ListItem Selected="True">--Select--</asp:ListItem>                    
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="reqvalAProgcd" 
                            ControlToValidate="ddlAProgcd"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblHMO" runat="server" Text="HTH/Local HMO:" AssociatedControlID="ddlAHMO"></asp:Label>                                
                        <asp:DropDownList ID="ddlAHMO" runat="server" 
                        DataSourceID="SqlDataSourceHMOcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("plancdadj") %>'>  
                        <asp:ListItem Selected="True">--Select--</asp:ListItem>                            
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="regvalAHMO" 
                            ControlToValidate="ddlAHMO"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />                
                        <asp:Label
                            id="lblASSN" Text="SSN:" AssociatedControlID="txtASSN" runat="server" />                                
                        <asp:TextBox 
                            id="txtASSN"   
                            Text='<%# Bind("SSN") %>'  OnTextChanged="txtASSN_TextChanged"                                  
                            Runat="server"/>                        
                        <asp:Label ID="Label2" runat="server" ForeColor="#7D7D7D"
                            Text="(xxx-xx-xxxx)"></asp:Label>
                        <asp:RequiredFieldValidator
                            id="reqvalASSN" ControlToValidate="txtASSN"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            Runat="server" /> 
                        <asp:RegularExpressionValidator 
                            id="RegvalASSN" 
                            runat="server"
                            ControlToValidate="txtASSN"
                            ValidationExpression="^\d{3}-\d{2}-\d{4}$"
                            Display="dynamic" ValidationGroup="add1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid SSN in format 'xxx-xx-xxxx'
                        </asp:RegularExpressionValidator>      
                         <div id="SSN_history" style="padding-left:5px; padding-top:10px; padding-bottom:10px; margin:auto; display:none;background-color:White" runat="server">
                             <asp:GridView ID="grdv_ssnhistory" runat="server" CssClass="GridViewStyle" DataSourceID="SqlDataSourceSSN"
                                AutoGenerateColumns="false"  DataKeyNames="SeqNum" AllowSorting="true" AllowPaging="true" 
                                BorderColor="White" BackColor="White" 
                                PagerSettings-Mode="NumericFirstLast"                         
                                PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif">                         
                                <PagerStyle CssClass="PagerStyle" />
                                <AlternatingRowStyle CssClass="AltRowStyle" />
                                <HeaderStyle CssClass="HeaderStyle" />
                                <RowStyle CssClass="RowStyle" />                                                             
                                <Columns> 
                                    <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO"/>
                                    <asp:BoundField DataField="progcd" HeaderText="Program" SortExpression="progcd"/>
                                    <asp:BoundField DataField="PlanCode" HeaderText="Plan" SortExpression="PlanCode"/>
                                    <asp:BoundField DataField="SSN" HeaderText="SSN" SortExpression="SSN" ItemStyle-Width="80px"/>
                                    <asp:BoundField DataField="typecd" HeaderText="Type" SortExpression="typecd" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" ItemStyle-Width="90px" />
                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" ItemStyle-Width="90px" />
                                    <asp:BoundField DataField="eventcd" HeaderText="Event" SortExpression="eventcd" />
                                    <asp:BoundField DataField="trancd" HeaderText="Tran" SortExpression="trancd" />
                                    <asp:BoundField DataField="CoverageEffDt" HeaderText="Effective Date" SortExpression="CoverageEffDt" dataformatstring="{0:MM/dd/yyyy}" HtmlEncode="false"/>
                                    <asp:BoundField DataField="Comments" HeaderText="Notes" SortExpression="Comments" ItemStyle-Width="100px"/>
                                </Columns>
                                <emptydatarowstyle BorderColor="LightBlue" forecolor="Red"/>                        
                                <EmptyDataTemplate>
                                No records.
                                </EmptyDataTemplate>                         
                            </asp:GridView>
                            <asp:LinkButton ID="lnkClose_ssnhistory" runat="server" CommandName="close_ssnhistory" style="padding-left:600px;">Close</asp:LinkButton>
                        </div>                                             
                        <br />
                        <asp:Label
                            id="lblAType"
                            Text="Type:"
                            AssociatedControlID="ddlAType"
                            Runat="server" />                               
                        <asp:DropDownList ID="ddlAType"  
                            runat="server"
                           DataSourceID="SqlDataSourceTypeCd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("typecd") %>'>  
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                                                 
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="reqvalAType" 
                            ControlToValidate="ddlAType"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblAFname"
                            Text="First Name:"
                            AssociatedControlID="txtAFname"
                            Runat="server" />                                
                        <asp:TextBox 
                            id="txtAFname"   
                            Text='<%# Bind("FirstName") %>'                                 
                            Runat="server" />  
                            <asp:RequiredFieldValidator
                            id="reqvalAFname"
                            ControlToValidate="txtAFname"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            Runat="server" />                        
                        <br />
                        <asp:Label
                            id="lblALname"
                            Text="Last Name:"
                            AssociatedControlID="txtALname"
                            Runat="server" />                                
                        <asp:TextBox
                            id="txtALname" 
                            Text='<%# Bind("LastName") %>'                                   
                            Runat="server" />
                        <asp:RequiredFieldValidator
                            id="reqvalALname"
                            ControlToValidate="txtALname"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            Runat="server" />                        
                        <br />
                        <asp:Label
                            id="lblAEvent"
                            Text="Event:"
                            AssociatedControlID="ddlAEvent"
                            Runat="server" />                                 
                        <asp:DropDownList ID="ddlAEvent"  
                            runat="server" DataSourceID="SqlDataSourceEvent"
                             DataTextField="codedsc"
                             DataValueField="codeid"
                             AppendDataBoundItems="true"                                     
                             SelectedValue='<%# Bind("eventcd") %>' >                               
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                    
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvalAEvent" 
                            ControlToValidate="ddlAEvent"
                            runat="server"  Display="Dynamic"
                            Text="(Required)"
                            ValidationGroup="add1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblATran"
                            Text="Tran:"
                            AssociatedControlID="ddlATran"
                            Runat="server" />                                
                        <asp:DropDownList ID="ddlAtran"  
                            runat="server" DataSourceID="SqlDataSourceTran"
                            DataTextField="codedsc"
                            DataValueField="codeid" 
                            AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("trancd") %>' >
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                    
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvalATran" 
                            ControlToValidate="ddlATran"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblAEffdt"
                            Text="Effective Date:"
                            AssociatedControlID="txtAEffdt"
                            Runat="server" />                               
                        <asp:TextBox
                            id="txtAEffdt"  
                            Text='<%# Bind("CoverageEffDt") %>'                                    
                            Runat="server" /> 
                        <asp:Label ID="Label3" runat="server" ForeColor="#7D7D7D"
                            Text="(mm/dd/yyyy)"></asp:Label>
                        <asp:RequiredFieldValidator
                            id="reqAEffdt"
                            ControlToValidate="txtAEffdt"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="add1"
                            Runat="server" />
                        <asp:RegularExpressionValidator 
                            id="regvalAEffdt" 
                            runat="server"
                            ControlToValidate="txtAEffdt"
                            ValidationExpression="^((0[1-9])|(1[0-2]))\/((0[1-9])|([12][0-9])|([3][01]))\/(\d{4})$"
                            Display="Dynamic" ValidationGroup="add1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid date in format mm/dd/yyyy
                        </asp:RegularExpressionValidator>                        
                        <br />
                        <asp:Label
                            id="lblANotes"
                            Text="Notes:"
                            AssociatedControlID="txtANotes"
                            Runat="server" />
                        <asp:TextBox
                            id="txtANotes" 
                            Text='<%# Bind("Comments") %>'                                     
                            Runat="server" />    
                        <br />                        
                                                       
                            <asp:Button 
                                id="btnAdd"
                                Text="Add"                                
                                CommandName="Insert"
                                ValidationGroup="add1"
                                Runat="server" 
                                style="width:80px;margin-left:170px;margin-top:20px;margin-bottom:20px;"/>
                            <asp:Button 
                                id="btnAReset"
                                Text="Reset"                                
                                CommandName="Reset"
                                CausesValidation="false"
                                Runat="server" 
                                style="width:80px;margin-left:20px;margin-top:20px;margin-bottom:20px;"/>
                            <asp:Button 
                                id="btnACancel"
                                Text="Cancel"                                 
                                CausesValidation="false"
                                CommandName="Cancel"                                           
                                Runat="server" 
                                style="width:80px;margin-left:20px;margin-top:20px;margin-bottom:20px;"/>
                            <asp:SqlDataSource ID="SqlDataSourceSSN" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                SelectCommand="SELECT [SeqNum], [YRMO], [progcd], [PlanCode],  [plancdadj], [SSN], [typecd], [FirstName], [LastName], [eventcd], [trancd], [CoverageEffDt], [Comments], [YRMO] FROM [HTH_HMO_Billing] WHERE ([ReportType]=@rpttype) AND ([SSN]=@SSN)">                    
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="ADJ" Name="rpttype" Type="String" />
                                    <asp:ControlParameter ControlID="txtASSN" Name="SSN" PropertyName="Text" Type="String" />
                                </SelectParameters>                    
                            </asp:SqlDataSource>  
                        </div>
                         <asp:SqlDataSource ID="SqlDataSourceProgcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source) AND [codeid] NOT IN ('CA','CP')">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="progcd" Name="source" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceHMOcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="HMOBILLRPT" Name="source" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </InsertItemTemplate>
                    </asp:FormView>                
                    <asp:FormView id="frmEditAdj" DataSourceID="SqlDataSourceEditAdj" 
                    DataKeyNames="SeqNum" runat="server" DefaultMode="Edit"  
                    OnItemUpdated="frmEditAdj_RowUpdated" Visible="false" OnItemCreated="itemCreated"
                    OnItemCommand="frmEditAdj_ItemCommand" OnItemUpdating="frmEditAdj_ItemUpdating">                   
                    <EditItemTemplate>                    
                        <div id="hraSubIntroText" style="color:#5D478B; border-bottom:2px solid #CDC9C9; margin-bottom:2px;font-size:16px;">Edit Adjustment</div>
                        <div class="webformIPBA" style="background-color:#F7F7F7;">
                        <asp:Label
                            id="lblEyrmo" Text="YRMO:" AssociatedControlID="txtEYRMO" runat="server" />                                
                        <asp:TextBox
                            id="txtEYRMO"  
                            Text='<%# Bind("YRMO") %>'                                  
                            Runat="server" />
                        <asp:Label ID="Label3" runat="server" ForeColor="#7D7D7D"
                            Text="(yyyymm)"></asp:Label>
                        <asp:RequiredFieldValidator
                            id="reqvalAYrmo" ControlToValidate="txtEYRMO"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            Runat="server" /> 
                        <asp:RegularExpressionValidator 
                            id="RegularExpressionValidator1" 
                            runat="server"
                            ControlToValidate="txtEYRMO"
                            ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                            Display="dynamic" ValidationGroup="grp1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid YRMO in format YYYYMM
                        </asp:RegularExpressionValidator>    
                        <br />
                        <asp:Label ID="lblEProgcd" runat="server" Text="Program:" AssociatedControlID="ddlEProgcd"></asp:Label>                                    
                        <asp:DropDownList ID="ddlEProgcd" runat="server"
                        DataSourceID="SqlDataSourceProgcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("progcd") %>'>    
                        <asp:ListItem Selected="True">--Select--</asp:ListItem>                    
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvalEProgcd" 
                            ControlToValidate="ddlEProgcd"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblEHMO" runat="server" Text="HTH/Local HMO:" AssociatedControlID="ddlEHMO"></asp:Label>                                    
                        <asp:DropDownList ID="ddlEHMO" runat="server" 
                        DataSourceID="SqlDataSourceHMOcd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                        SelectedValue='<%# Bind("plancdadj") %>'>  
                        <asp:ListItem Selected="True">--Select--</asp:ListItem>                            
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="regvalEHMO" 
                            ControlToValidate="ddlEHMO"
                            runat="server" 
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblSSN" Text="SSN:" AssociatedControlID="txtSSN" runat="server" />                                    
                        <asp:TextBox
                            id="txtSSN"
                            Text='<%# Bind("SSN") %>'
                            Runat="server" /> 
                        <asp:Label ID="Label4" runat="server" ForeColor="#7D7D7D"
                            Text="(xxx-xx-xxxx)"></asp:Label>  
                        <asp:RequiredFieldValidator
                            id="reqvalSSN" ControlToValidate="txtSSN"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            Runat="server" /> 
                        <asp:RegularExpressionValidator 
                            id="RegvalSSN" 
                            runat="server"
                            ControlToValidate="txtSSN"
                            ValidationExpression="^\d{3}-\d{2}-\d{4}$"
                            Display="Dynamic" ValidationGroup="grp1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid SSN in format 'xxx-xx-xxxx'
                        </asp:RegularExpressionValidator> 
                        <br />
                        <asp:Label
                            id="lblType"
                            Text="Type:"
                            AssociatedControlID="ddlType"
                            Runat="server" />                                   
                        <asp:DropDownList ID="ddlType"  
                            runat="server" 
                            DataSourceID="SqlDataSourceTypeCd" DataTextField="codedsc" DataValueField="codeid" AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("typecd") %>' >
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                                                      
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="reqvalType" 
                            ControlToValidate="ddlType"
                            runat="server" Display="Dynamic"
                            Text="(Required)"
                            ValidationGroup="grp1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblFname"
                            Text="First Name:"
                            AssociatedControlID="txtFname"
                            Runat="server" />                                    
                        <asp:TextBox
                            id="txtFname"
                            Text='<%# Bind("FirstName") %>'
                            Runat="server" /> 
                        <asp:RequiredFieldValidator
                            id="reqvalFname"
                            ControlToValidate="txtFname"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            Runat="server" />                           
                        <br />
                        <asp:Label
                            id="lblLname"
                            Text="Last Name:"
                            AssociatedControlID="txtLname"
                            Runat="server" />                                    
                        <asp:TextBox
                            id="txtLname"
                            Text='<%# Bind("LastName") %>'
                            Runat="server" /> 
                        <asp:RequiredFieldValidator
                            id="reqvalLname"
                            ControlToValidate="txtLname"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="grp1"
                            Runat="server" />                        
                        <br />
                        <asp:Label
                            id="lblEvent"
                            Text="Event:"
                            AssociatedControlID="ddlEvent"
                            Runat="server" />                                    
                        <asp:DropDownList ID="ddlEvent"  
                            runat="server" DataSourceID="SqlDataSourceEvent"
                            DataTextField="codedsc"
                            DataValueField="codeid"
                            AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("eventcd") %>' >
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                    
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvalEvent" 
                            ControlToValidate="ddlEvent"
                            runat="server" Display="Dynamic"
                            Text="(Required)"
                            ValidationGroup="grp1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblTran"
                            Text="Tran:"
                            AssociatedControlID="ddlTran"
                            Runat="server" />                                    
                        <asp:DropDownList ID="ddlTran"  
                            runat="server" DataSourceID="SqlDataSourceTran"
                            DataTextField="codedsc"
                            DataValueField="codeid"
                            AppendDataBoundItems="true"
                            SelectedValue='<%# Bind("trancd") %>' >
                            <asp:ListItem Selected="True">--Select--</asp:ListItem>                                    
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqvalTran" 
                            ControlToValidate="ddlTran"
                            runat="server" Display="Dynamic"
                            Text="(Required)"
                            ValidationGroup="grp1"
                            InitialValue="--Select--">
                        </asp:RequiredFieldValidator>
                        <br />
                        <asp:Label
                            id="lblEffdt"
                            Text="Effective Date:"
                            AssociatedControlID="txtEffdt"
                            Runat="server" />                                   
                        <asp:TextBox
                            id="txtEffdt"
                            Text='<%# Bind("CoverageEffDt") %>'
                            Runat="server" /> 
                        <asp:Label ID="Label5" runat="server" ForeColor="#7D7D7D"
                            Text="(mm/dd/yyyy)"></asp:Label>
                        <asp:RequiredFieldValidator
                            id="reqEffdt" Display="Dynamic"
                            ControlToValidate="txtEffdt"
                            Text="(Required)"
                            ValidationGroup="grp1"
                            Runat="server" />
                        <asp:RegularExpressionValidator 
                            id="regvalEffdt" 
                            runat="server"
                            ControlToValidate="txtEffdt"
                            ValidationExpression="^((0[1-9])|(1[0-2]))\/((0[1-9])|([12][0-9])|([3][01]))\/(\d{4})$"
                            Display="Dynamic" ValidationGroup="grp1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid date in format mm/dd/yyyy
                        </asp:RegularExpressionValidator>     
                        <br />
                        <asp:Label
                            id="lblNotes"
                            Text="Notes:"
                            AssociatedControlID="txtNotes"
                            Runat="server" />
                        <asp:TextBox
                            id="txtNotes"
                            Text='<%# Bind("Comments") %>'
                            Runat="server" />    
                        <br />
                        <asp:Button 
                            id="btnSubmit"
                            Text="Submit"
                            CommandName="Update"                                    
                            ValidationGroup="grp1"
                            Runat="server" 
                            style="width:80px;margin-left:170px;margin-top:20px;margin-bottom:20px;"/>
                        <asp:Button 
                            id="btnReset"
                            Text="Reset"
                            CommandName="Reset"
                            CausesValidation="false"
                            Runat="server" 
                            style="width:80px;margin-left:20px;margin-top:20px;margin-bottom:20px;"/>
                        <asp:Button 
                            id="btnCancel"
                            Text="Cancel" 
                            CausesValidation="false"
                            CommandName="Cancel"                                           
                            Runat="server" 
                            style="width:80px;margin-left:20px;margin-top:20px;margin-bottom:20px;"/>
                       
                        </div>
                         <asp:SqlDataSource ID="SqlDataSourceProgcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source) AND [codeid] NOT IN ('CA','CP')">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="progcd" Name="source" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSourceHMOcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="HMOBILLRPT" Name="source" Type="String" />
                            </SelectParameters>
                        </asp:SqlDataSource>                                  
                    </EditItemTemplate>
                    </asp:FormView>                            
                </div>                     
                <asp:SqlDataSource ID="SqlDataSourceAdj" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:EBADB %>"
                    SelectCommand="SELECT [SeqNum], [progcd],  [plancdadj], [PlanCode], [SSN], [typecd], [FirstName], [LastName], [eventcd], [trancd], [CoverageEffDt], [Comments], [YRMO] FROM [HTH_HMO_Billing] WHERE ([ReportType]=@rpttype)"
                    DeleteCommand="DELETE FROM [HTH_HMO_Billing] WHERE [SeqNum] = @SeqNum">
                     <SelectParameters>
                        <asp:Parameter DefaultValue="ADJ" Name="rpttype" Type="String" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="SeqNum" Type="String" />
                    </DeleteParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceEditAdj" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:EBADB %>"
                    SelectCommand="SELECT [SeqNum],[YRMO],[progcd], [plancdadj], [SSN], [typecd], [FirstName], [LastName], [eventcd], [trancd], CONVERT(VARCHAR(20),[CoverageEffDt],101) AS [CoverageEffDt], [Comments] FROM [HTH_HMO_Billing] WHERE ([SeqNum]=@SeqNum)"
                     UpdateCommand="UPDATE [HTH_HMO_Billing] SET [SSN]=@SSN, [progcd]=@progcd, [PlanCode]=@plancd, [plancdadj]=@plancdadj, [typecd]=@typecd, [FirstName]=@FirstName, [LastName]=@LastName, [eventcd]=@eventcd, [trancd]=@trancd, [CoverageEffDt]=@CoverageEffDt, [Comments]=@Comments,[YRMO] = @YRMO WHERE ([SeqNum]=@SeqNum)" >                 
                   <SelectParameters>
                        <asp:ControlParameter ControlID="grdvAdj" Name="SeqNum" PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="grdvAdj" Name="SeqNum" PropertyName="SelectedValue" Type="Int32" />
                        <asp:Parameter Name="plancd" Type="String" />
                    </UpdateParameters>
            </asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDataSourceAddAdj" runat="server" OnInserted="SqlDataSourceAddAdj_Inserted"
                    ConnectionString="<%$ ConnectionStrings:EBADB %>"                   
                     InsertCommand="INSERT INTO [HTH_HMO_Billing]([YRMO], [ReportType], [progcd], [plancdadj], [SSN], [typecd], [FirstName], [LastName], [eventcd], [trancd], [CoverageEffDt], [Comments], [PlanCode]) values (@YRMO, @rpttype, @progcd, @plancdadj, @SSN, @typecd, @FirstName, @LastName, @eventcd, @trancd, @CoverageEffDt, @Comments, @plancd)
                     ;SET @NewID = Scope_Identity()" >                 
                   <InsertParameters> 
                        <asp:Parameter Name="NewID" Type="Int32" Direction="Output"/> 
                        <asp:Parameter Name="YRMO" Type="String" /> 
                        <asp:Parameter Name="rpttype" Type="String" DefaultValue="ADJ" />  
                        <asp:Parameter Name="progcd" Type="String" />
                        <asp:Parameter Name="PlanCode" Type="String" />      
                        <asp:Parameter Name="SSN" Type="String" />
                        <asp:Parameter Name="typecd" Type="String" />
                        <asp:Parameter Name="FirstName" Type="String" />
                        <asp:Parameter Name="LastName" Type="String" />
                        <asp:Parameter Name="eventcd" Type="String" />
                        <asp:Parameter Name="trancd" Type="String" />
                        <asp:Parameter Name="CoverageEffDt" Type="DateTime" />                   
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="plancd" Type="String" />
                    </InsertParameters>                                       
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceEvent" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="eventcd" Name="source" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceTran" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="trancd" Name="source" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceProgcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source) AND [codeid] NOT IN ('CA','CP')">
                <SelectParameters>
                    <asp:Parameter DefaultValue="progcd" Name="source" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceHMOcd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [source], [dsc], [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="HMOBILLRPT" Name="source" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDataSourceTypeCd" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>" SelectCommand="SELECT [codeid], (codeid + ' - ' + dsc) AS [codedsc] FROM [Codes] WHERE ([source] = @source)">
                <SelectParameters>
                    <asp:Parameter DefaultValue="typecd" Name="source" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>            
	    </div>
	</div>
	<cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


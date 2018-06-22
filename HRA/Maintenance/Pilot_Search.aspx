<%@ Page Language="C#"  MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="Pilot_Search.aspx.cs" Inherits="HRA_Maintenance_Employee_Search" Title="HRA Pilot Search" %>
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
                        <li><asp:HyperLink ID="hypCurMod" runat="server"  NavigateUrl="~/HRA/Maintenance/Maintainence_Module.aspx">Current Module</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypBillingImp" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Maintenance/Pilot_Search.aspx">Search Pilot</asp:HyperLink></li>                        
                    </ul>
                </li>
            </ul>
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
        </div>        
    </div>
    <div id="contentright">
        <div id = "introText">Search Employee</div> 
        <div id = "importbox">
            <fieldset>
                <table style="margin-top:10px;margin-bottom:10px">
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblSSN" runat="server" Text="SSN:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSSN" runat="server" style="border:2px solid #e2e2e2" ValidationGroup="v1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvSSN" runat="server" Display="Dynamic" 
                            ControlToValidate="txtSSN" ValidationGroup ="v1" ErrorMessage="Missing SSN">* Required</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator 
                            id="RegvalSSN" 
                            runat="server"
                            ControlToValidate="txtSSN"
                            ValidationExpression="^\d{9}$"
                            Display="dynamic" ValidationGroup="v1"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid SSN in format 'xxxxxxxxx'
                            </asp:RegularExpressionValidator>  
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:Button ID="btnSSN" runat="server" Text="Search"  ValidationGroup ="v1" 
                            CssClass = "btnstyle" OnClick="btnSSN_Click"/> 
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblOr1" runat="server" Text="---- or ----" style="margin-left:50px;font-style:italic;color:#87CEEB"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblEmpno" runat="server" Text="Employee Number:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmpno" runat="server" style="border:2px solid #e2e2e2" ValidationGroup="v2"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvEmpno" runat="server" Display="Dynamic" 
                            ControlToValidate="txtEmpno" ValidationGroup ="v2" ErrorMessage="Missing Employee No.">* Required</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator 
                            id="RegvalEmpno" 
                            runat="server"
                            ControlToValidate="txtEmpno"
                            ValidationExpression="^[0-9]+$"
                            Display="dynamic" ValidationGroup="v2"
                            Font-Names="verdana" Font-Size="10pt">
                            Enter valid Employee Number
                            </asp:RegularExpressionValidator> 
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:Button ID="btnEmpno" runat="server" Text="Search" ValidationGroup ="v2" CssClass = "btnstyle" OnClick="btnEmpno_Click"/> 
                        </td>                         
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblOr2" runat="server" Text="---- or ----" style="margin-left:50px;font-style:italic;color:#87CEEB"></asp:Label>    
                        </td>
                    </tr>
                    <tr>
                        <td width="125" align="right">
                             <asp:Label ID="lblLastInitial" runat="server" Text="Last Name Initial:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastInitial" runat="server" style="border:2px solid #e2e2e2" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvInitial" runat="server" Display="Dynamic" 
                            ControlToValidate="txtLastInitial" ValidationGroup ="v3" ErrorMessage="Missing Last Name First initial">* Required</asp:RequiredFieldValidator>
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:Button ID="btnLastInitial" runat="server" Text="Search" ValidationGroup ="v3" CssClass = "btnstyle" OnClick="btnLastInitial_Click"/>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblOr3" runat="server" Text="---- or ----" style="margin-left:50px;font-style:italic;color:#87CEEB"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblFirst" runat="server" Text="First Name:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="txtFirst" runat="server" style="border:2px solid #e2e2e2" ></asp:TextBox> 
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvFirst" runat="server" Display="Dynamic" 
                            ControlToValidate="txtFirst" ValidationGroup ="v4" ErrorMessage="Missing First Name">* Required</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="125" align="right">
                            <asp:Label ID="lblLast" runat="server" Text="Last Name:" ForeColor="#036"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLast" runat="server" style="border:2px solid #e2e2e2" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvLast" runat="server" Display="Dynamic" 
                            ControlToValidate="txtLast" ValidationGroup ="v4" ErrorMessage="Missing Last Name">* Required</asp:RequiredFieldValidator>
                        </td>
                        <td width="20px">
                        </td>
                        <td style="margin-left:10px;">
                            <asp:Button ID="btnName" runat="server" Text="Search" ValidationGroup ="v4" CssClass = "btnstyle" OnClick="btnName_Click"/> 
                        </td>                        
                    </tr>
                </table>
            </fieldset>    
        </div>
        <div style="float:left;width:680px;color:red">
            <asp:Label ID="lblError" runat="server" Text="" style="margin-left:20px;"></asp:Label>
        </div>
        <br /><br />
        <div id = "EmpListDiv" runat="server" visible="false">
            <div id = "introText">Employees List</div>
        </div> 
        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
            <asp:GridView ID="grdvEmployee" runat="server" 
            AutoGenerateColumns="False" OnRowCommand="grdvEmployee_RowCommand" 
            GridLines="None" OnSorting="grdvEmployee_Sorting" AllowSorting="true"
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
                    <asp:ButtonField Text="Select" commandname="Select" ButtonType="Link"/>
                    <asp:BoundField HeaderText="Empno" DataField="empl_empno" SortExpression="empl_empno"/>
                    <asp:BoundField HeaderText="First Name" DataField="empl_fname" SortExpression="empl_fname" />
                    <asp:BoundField HeaderText="Last Name" DataField="empl_lname" SortExpression ="empl_lname" />
                    <asp:BoundField HeaderText="SSN" DataField="empl_ssn" SortExpression ="empl_ssn" />       
                </Columns>                
            </asp:GridView>
        </div>              
    </div>
</asp:Content>


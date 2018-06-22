<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Pilot.aspx.cs" Inherits="HRA_Maintenance_Pilot" Title="HRA Pilot Maintenance" %>
<%@ Register Assembly="sstchur.web.SmartNav" Namespace="sstchur.web.SmartNav" TagPrefix="cc1" %>
<%@ Register Assembly="KMobile.Web" Namespace="KMobile.Web.UI.WebControls" TagPrefix="asp" %>
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
     <div class = "Menu">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="Pilot" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Beneficiaries" Value="1"></asp:MenuItem>
                    <asp:MenuItem Text="Audit" Value="2"></asp:MenuItem>     
                    <asp:MenuItem Text="Transactions" Value="3"></asp:MenuItem>  
                    <asp:MenuItem Text="Letters History" Value="4"></asp:MenuItem>                   
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
       <div id = "introText" style="margin-left:0px;">
           <asp:Label ID="lblEmpHeading" runat="server" Text=""></asp:Label>
        </div>
        <div style="float:left;width:650px;margin:5px;">
        </div>  
        <div id = "multiView">  
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server"> 
                <div style="margin-top:10px;">
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup = "grp2" runat="server" DisplayMode="List" ShowMessageBox="True" />
                    <div class="info" id="infoDiv1" runat="server" visible="false">
                        <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                    </div>           
                    <div id = "formHra">
                        <fieldset>
                            <legend>Personal Information</legend>
                            <asp:Label ID="lblEmpNo" runat="server" Text="Emp#" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtEmpNo" runat="server" ReadOnly="true" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup = "grp2" runat="server" ErrorMessage="Employee Number is required!" ControlToValidate="txtEmpNo">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtSSN" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup = "grp2" runat="server" ErrorMessage="SSN is required!" ControlToValidate="txtSSN">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblFirst" runat="server" Text="First Name" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtFirst" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv1" ValidationGroup = "grp2" runat="server" ErrorMessage="First Name is required!" ControlToValidate="txtFirst">*</asp:RequiredFieldValidator>  
                            <br class = "br_e" />
                            <asp:Label ID="lblMI" runat="server" Text="MI" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtMI" runat="server" CssClass="inputHra"></asp:TextBox>                           
                            <br class = "br_e" />
                            <asp:Label ID="lblLast" runat="server" Text="Last Name" CssClass="labelHra"></asp:Label>                            
                            <asp:TextBox ID="txtLast" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv2" ValidationGroup = "grp2" runat="server" ErrorMessage="Last Name is required!" ControlToValidate="txtLast">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblSex" runat="server" Text="Sex" CssClass="labelHra"></asp:Label>                            
                            <asp:RadioButtonList ID="rdbSex" runat="server" CssClass="inputHra">
                                <asp:ListItem Value="0" Text="Male"/>
                                <asp:ListItem Value="1" Text="Female"/>
                            </asp:RadioButtonList> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup = "grp2" runat="server" ErrorMessage="Sex code is required!" ControlToValidate="rdbSex">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />                           
                            <asp:Label ID="lblBirth" runat="server" Text="Birth Dt" CssClass="labelHra"></asp:Label>                            
                            <asp:TextBox ID="txtBirth" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv3" ValidationGroup = "grp2" runat="server" ErrorMessage="Birth Date is required!" ControlToValidate="txtBirth">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revBirth1" ControlToValidate = "txtBirth" 
                            ValidationExpression="^(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}$" 
                            runat="server" Display="Dynamic" ValidationGroup = "grp2" ErrorMessage = "Correct Date format is MM/dd/yyyy" />
                            <br class = "br_e" />                            
                            <asp:Label ID="lblPerm" runat="server" Text="Perm FT Dt" CssClass="labelHra"></asp:Label> 
                            <asp:TextBox ID="txtPerm" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv9" runat="server" ValidationGroup = "grp2"  ErrorMessage="Permanent Full Time Date is required!" Text="*" ControlToValidate="txtPerm"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPerm" ControlToValidate = "txtPerm" 
                            ValidationExpression="^(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}$" 
                            runat="server" Display="Dynamic" ValidationGroup = "grp2" ErrorMessage = "Correct Date format is MM/dd/yyyy" />                            
                            <br class = "br_e" />
                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="labelHra"></asp:Label>                            
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="inputHra">
                                <asp:ListItem Value="0" Text="ACTIVE"></asp:ListItem>                               
                                <asp:ListItem Value="1" Text="TERMINATED"></asp:ListItem>
                                <asp:ListItem Value="2" Text="DIED"></asp:ListItem>                                                                  
                            </asp:DropDownList><br />
                            <br class = "br_e" />
                            <asp:Label ID="lblRetire" runat="server" Text="Termination Dt" CssClass="labelHra"></asp:Label>                            
                            <asp:TextBox ID="txtRetire" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revRetire" ControlToValidate = "txtRetire" 
                            ValidationExpression="^(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}$" 
                            runat="server" Display="Dynamic" ValidationGroup = "grp2" ErrorMessage = "Correct Date format is MM/dd/yyyy" />                            
                            <br class = "br_e" />
                            <asp:Label ID="lblDeath" runat="server" Text="Date of Death" CssClass="labelHra"></asp:Label>                           
                            <asp:TextBox ID="txtDeath" runat="server" CssClass="inputHra"></asp:TextBox>                                
                            <asp:RegularExpressionValidator ID="revDeath" ControlToValidate = "txtDeath" 
                            ValidationExpression="^(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}$" 
                            runat="server" Display="Dynamic" ErrorMessage = "Correct Date format is MM/dd/yyyy" ValidationGroup = "grp2"/>                                    
                            <br class = "br_e" /> <br class = "br_e" /><br class = "br_e" />              
                            <asp:CustomValidator ID="ctvRetire" Display="Dynamic" ValidationGroup = "grp2"  runat="server" ControlToValidate="txtRetire" OnServerValidate="check_Rstatus"></asp:CustomValidator>
                            <asp:CustomValidator ID="ctvStatus" runat="server" Display="Dynamic" ValidationGroup = "grp2"  ControlToValidate="ddlStatus" OnServerValidate="check_dates"></asp:CustomValidator>
                            <asp:CustomValidator ID="ctvDeath" runat="server" Display="Dynamic" ValidationGroup = "grp2"  ControlToValidate="txtDeath" OnServerValidate="check_Dstatus"></asp:CustomValidator>                              
                        </fieldset>
                        <fieldset>
                            <legend>Address Information</legend>
                            <asp:Label ID="lblAddr1" runat="server" Text="Address 1" Cssclass = "labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr1" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv4" ValidationGroup = "grp2" runat="server" ErrorMessage="Address1 is required!" ControlToValidate="txtAddr1">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblAddr2" runat="server" Text="Address 2" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtAddr2" runat="server" CssClass="inputHra"></asp:TextBox>                                 
                            <br class = "br_e" />
                            <asp:Label ID="lblCity" runat="server" Text="City" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtCity" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv6" ValidationGroup = "grp2" runat="server" ErrorMessage="City is required!" ControlToValidate="txtCity">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />   
                            <asp:Label ID="lblState" runat="server" Text="State" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtState" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv7" ValidationGroup = "grp2" runat="server" ErrorMessage="State is required!" ControlToValidate="txtState">*</asp:RequiredFieldValidator>
                            <br class = "br_e" />
                            <asp:Label ID="lblZip" runat="server" Text="Zip" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtZip" runat="server" CssClass="inputHra"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv8" ValidationGroup = "grp2" runat="server" ErrorMessage="Zip Code is required!" ControlToValidate="txtZip">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revZip" ControlToValidate = "txtZip" 
                            ValidationExpression="^\d{1,5}([\-]\d{4})?$" 
                            runat="server" Display="Dynamic" ErrorMessage = "Zip format is wrong!" ValidationGroup = "grp2" Text="*"/>
                        </fieldset>    
                        <fieldset>
                            <legend>HRA Amount Information</legend> 
                            <asp:Label ID="lblLump" runat="server" Text="Lump Sum $" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtLump" runat="server" CssClass="inputHra" ReadOnly="true"></asp:TextBox>  
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate = "txtLump" 
                            ValidationExpression="^([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$" 
                            runat="server" Display="Dynamic" ErrorMessage = "Format is #.##" ValidationGroup = "grp2" Text="*"/>                          
                            <br class = "br_e" />
                            <asp:Label ID="lblHRA" runat="server" Text="HRA $" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtHRA" runat="server" CssClass="inputHra" ReadOnly="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate = "txtHRA" 
                            ValidationExpression="^([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$" 
                            runat="server" Display="Dynamic" ErrorMessage = "Format is #.##" ValidationGroup = "grp2" Text="*"/>
                            <br class = "br_e" />
                            <asp:Label ID="lblTHRA" runat="server" Text="Total HRA $" CssClass="labelHra"></asp:Label>
                            <asp:TextBox ID="txtTHRA" runat="server" CssClass="inputHra" ReadOnly="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate = "txtTHRA" 
                            ValidationExpression="^([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$" 
                            runat="server" Display="Dynamic" ErrorMessage = "Format is #.##" ValidationGroup = "grp2" Text="*"/>
                        </fieldset>
                    </div>           
                    <table style="margin-top:10px;margin-left:50px;">
                        <tr>                                                              
                            <td style="margin-left:100px;margin-right:20px;margin-top:20px;margin-bottom:20px;">
                                <asp:LinkButton ID="btnEdit" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" CssClass = "imgbutton" 
                                    OnClick="btnEdit_Click" ><span>Edit</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnSave" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" CssClass = "imgbutton" 
                                    ValidationGroup = "grp2" OnClick="btnSave_Click" Visible= "false" ><span>Save</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="this.blur();" 
                                Font-Underline="false" CssClass = "imgbutton" OnClick="btnCancel_Click" 
                                Visible= "false"><span>Cancel</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>                    
                </div>
                </asp:View>
                <asp:View ID="View2" runat="server">                
                <div id = "hraIntroText"> 
                   <table>
                        <tr>
                            <td style="width:550px;margin:20px;">
                                <asp:Image ID="img_CF" runat="server" ImageUrl="~/styles/images/collapsed1.gif"/> 
                                <asp:LinkButton ID="lnkAddDependent" runat="server" Font-Underline="false" ForeColor="#5D478B" OnClick="lnkAddDependant_Click">Add a Beneficiary</asp:LinkButton>
                            </td>
                        </tr>
                    </table> 
                </div> 
                <div class="error" id="errDivBenAdd" runat="server" visible="false">
                    <asp:Label ID="lblAError" runat="server" Text=""></asp:Label>
                </div>                    
                <div id="divBenAdd" runat="server" style="float:left;margin:10px;width:700px;margin-top:20px;border-bottom: 1px inset #E6E6FA;" visible = "false">
                    <asp:Label ID="lblBSSN" runat="server" Text="SSN" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBSSN" runat="server" CssClass="inputHra"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSSN" ControlToValidate="txtBSSN" CssClass="errorHra" Display="Dynamic"
                        ValidationGroup = "grp3" runat="server" ErrorMessage="Please enter SSN!">* Required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate = "txtBSSN" 
                    ValidationExpression="^\d{9}$" 
                    runat="server" Display="Dynamic" ValidationGroup = "grp3" ErrorMessage = "Correct SSN format is 'xxxxxxxxx'"/> 
                    <br class="br_e" />                                
                    <asp:Label ID="lblOrder" runat="server" Text="Order No" CssClass="labelHra"></asp:Label>                                
                    <asp:DropDownList ID="ddlOrder" AutoPostBack="true" OnSelectedIndexChanged="ddlOrder_Indexchange" runat="server" CssClass="dropHra">                        
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvOrder" ControlToValidate="ddlOrder" CssClass="errorHra" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" ErrorMessage="Please enter Order!">* Required</asp:RequiredFieldValidator> 
                    <br class="br_e" />                              
                    <asp:Label ID="lblBFirst" runat="server" Text="First Name" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBFirst" runat="server" CssClass="inputHra"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBFirst" CssClass="errorHra" ControlToValidate="txtBFirst" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" ErrorMessage="Please enter First Name!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" />                               
                    <asp:Label ID="lblBMI" runat="server" Text="MI" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBMI" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <br class="br_e" />                               
                    <asp:Label ID="lblBLast" runat="server" Text="Last Name" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBLast" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBLast" CssClass="errorHra" ControlToValidate="txtBLast" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" ErrorMessage="Please enter Last Name!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" /> 
                    <asp:Label ID="lblBBirth" runat="server" Text="Birth Dt" CssClass="labelHra"></asp:Label>                               
                    <asp:TextBox ID="txtBBirth" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBBirth" ControlToValidate="txtBBirth" runat="server" Display="Dynamic"
                        ValidationGroup = "grp3" CssClass="errorHra" ErrorMessage="Please enter Birth Date!">* Required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revbBirth" ControlToValidate = "txtBBirth" 
                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}" 
                    runat="server" Display="Dynamic" ValidationGroup = "grp3" ErrorMessage = "Correct Date format is MM/dd/yyyy"/> 
                    <br class="br_e" />                             
                    <asp:Label ID="lblRelation" runat="server" Text="Relationship" CssClass="labelHra"></asp:Label>                                
                    <asp:DropDownList ID="ddlRelation" AutoPostBack="true" runat="server" OnSelectedIndexChanged="checkRelation" CssClass="dropHra" >
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem Value="0" Text = "SP" />
                        <asp:ListItem Value="1" Text = "CH" />
                        <asp:ListItem Value="2" Text = "OTH" />
                    </asp:DropDownList> 
                    <asp:RequiredFieldValidator ID="rfvBRelation" CssClass="errorHra" Display="Dynamic"
                        ControlToValidate="ddlRelation" runat="server" ValidationGroup = "grp3" InitialValue="--Select--">* Required</asp:RequiredFieldValidator> 
                    <br class="br_e" /> 
                    <div id = "relOtherDiv" runat="server" style="" visible="false">
                        <asp:Label ID="lblOther" runat="server" Text="If selected other:" CssClass="centerHra1"></asp:Label>
                        <br class="br_e" />
                        <asp:TextBox ID="txtOther" runat="server" CssClass="centerHra2"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="Label6" runat="server" Text="  (Max 200 characters)" CssClass="centerHra1"></asp:Label>
                    </div>
                    <br class="br_e" />  
                    <br class="br_e" />                             
                    <asp:Label ID="lblBSex" runat="server" Text="Sex" CssClass="labelHra"></asp:Label>                                
                    <asp:RadioButtonList ID="rdbBSex" runat="server" CssClass="radioHra" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Text="Male"/>
                        <asp:ListItem Value="1" Text="Female"/>
                    </asp:RadioButtonList> 
                    <asp:RequiredFieldValidator ID="rfvBSex" CssClass="centerHra2" ControlToValidate="rdbBSex" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" ErrorMessage="Please select Gender!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" />
                    <asp:Label ID="lblBAddrDiff" runat="server" Text="Address Different?" CssClass="labelHra"></asp:Label>                                
                    <asp:CheckBox ID="cbxBAddrDiff" runat="server" CssClass="checkHra" 
                        AutoPostBack="true" OnCheckedChanged="cbxBAddrDiff_OnCheckedChanged" />
                    <br class="br_e" />                              
                    <asp:Label ID="lblBAddr" runat="server" Text="Address (if Diff)" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBAddr" runat="server" CssClass="inputHra" Enabled="false" ></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBAddr" ControlToValidate="txtBAddr" Enabled="false" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                    <br class="br_e" />                             
                    <asp:Label ID="lblBAddr2" runat="server" Text="Address 2" CssClass="labelHra"></asp:Label>                               
                    <asp:TextBox ID="txtBAddr2" runat="server" CssClass="inputHra" Enabled="false"></asp:TextBox>
                    <br class="br_e" />
                    <asp:Label ID="lblBCity" runat="server" Text="City" CssClass="labelHra"></asp:Label> 
                    <asp:TextBox ID="txtBCity" runat="server" CssClass="inputHra" Enabled="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvBCity" ControlToValidate="txtBCity" Enabled="false" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                    <br class="br_e" />
                    <asp:Label ID="lblBState" runat="server" Text="State" CssClass="labelHra"></asp:Label> 
                    <asp:TextBox ID="txtBState" runat="server" CssClass="inputHra" Enabled="false"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBState" ControlToValidate="txtBState" Enabled="false" Display="Dynamic"
                        runat="server" ValidationGroup = "grp3" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                    <br class="br_e" />  
                    <asp:Label ID="lblBZip" runat="server" Text="Zip" CssClass="labelHra"></asp:Label>    
                    <asp:TextBox ID="txtBzip" runat="server" CssClass="inputHra" Enabled="false"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBZip" ControlToValidate="txtBZip" Enabled="false" Display="Dynamic" 
                        runat="server" ValidationGroup = "grp3" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>     
                    <br class="br_e" />                     
                    <br class="br_e" />
                    <br class="br_e" /> 
                    <table> 
	                    <tr>                        
                            <td style="margin-left:100px;margin-right:20px;margin-top:20px;margin-bottom:20px;">
                                <asp:LinkButton ID="btn1Save" runat="server" 
                                    OnClientClick="this.blur();" Font-Underline="false" 
                                    CssClass="imgbutton" ValidationGroup = "grp3" OnClick="btn1Save_Click"><span>Save</span></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="btn1Cancel" runat="server"  
                                    OnClientClick="this.blur();" Font-Underline="false" 
                                    CssClass="imgbutton" OnClick="btn1Cancel_Click" ><span>Cancel</span></asp:LinkButton>
                            </td>
	                    </tr>
	                </table>
                </div>
                <div class="info" id="infoDiv2" runat="server" visible="false">
                    <asp:Label ID="lblInfo1" runat="server" Text=""></asp:Label>
                </div> 
                <div style="float:left;margin:10px;width:700px;margin-top:20px;"></div>                
                <div id = "formHra">
                    <fieldset>
                        <legend>Beneficiaries Information</legend>
                        <asp:CompleteGridView runat="server" ID="grdvDependants" CssClass="tablestyle"                       
                        AutoGenerateColumns="False"  AllowSorting="True" 
                        AllowPaging="True" PagerSettings-Mode="NumericFirstLast" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" 
                        DataSourceID="SqlDataSource1" DataKeyField="dpnd_ssn"
                        DataKeyNames="dpnd_ssn"
                        OnRowDeleting="grdvDependants_onDeleting"
                        OnRowCommand="grdvDependants_rowCommand"> 
                        <PagerSettings Mode="NextPreviousFirstLast" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />               
                        <PagerStyle CssClass="customGridpaging" />
                            <columns>
                                <asp:ButtonField Text="Select" commandname="Select" ButtonType="Link"/> 
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>                                        
                                        <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="False" 
                                        CommandName="Delete" Text="Delete"
                                        OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>                         
                                <asp:BoundField DataField="dpnd_ssn" HeaderText="SSN" SortExpression="dpnd_ssn"></asp:BoundField>
                                <asp:BoundField DataField="dpnd_fname" HeaderText="First Name" SortExpression="dpnd_fname"></asp:BoundField>
                                <asp:BoundField DataField="dpnd_minit" HeaderText="Middle Initial" SortExpression="dpnd_minit"></asp:BoundField>
                                <asp:BoundField DataField="dpnd_lname" HeaderText="Last Name" SortExpression="dpnd_lname"></asp:BoundField>                            
                                <asp:CheckBoxField DataField="dpnd_owner" HeaderText="Owner" SortExpression="dpnd_owner"></asp:CheckBoxField>
                                <asp:BoundField DataField="dpnd_owner_eff_dt" HeaderText="Effective Date" SortExpression="dpnd_owner_eff_dt"></asp:BoundField>
                                <asp:BoundField DataField="dpnd_order" HeaderText="Order" SortExpression="dpnd_order"></asp:BoundField> 
                            </columns>                    
                        </asp:CompleteGridView><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT [dpnd_ssn], [dpnd_fname], [dpnd_minit], [dpnd_lname], [dpnd_order], [dpnd_owner],CONVERT(VARCHAR(15),[dpnd_owner_eff_dt],101) AS [dpnd_owner_eff_dt] FROM [Dependant] WHERE ([dpnd_empno] = @dpnd_empno) ORDER BY [dpnd_order]"
                            DeleteCommand="DELETE FROM [Dependant] WHERE ([dpnd_ssn] = @dpnd_ssn)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtEmpNo" Name="dpnd_empno" PropertyName="Text"
                                    Type="Int32" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="dpnd_ssn" Type="String" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </fieldset>
                </div>                                 
                <div id="divBenEdit" runat="server" style="float:left;margin:10px;width:700px;margin-top:20px;" visible = "false">
                    <div id = "formHra">
                        <fieldset>
                            <legend>Selected Beneficiary Detail Information</legend>
                        </fieldset>
                    </div>                    
                    <asp:Label ID="lblBSSNE" runat="server" Text="SSN" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBSSNE" runat="server" CssClass="inputHra"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSSNE" ControlToValidate="txtBSSNE" CssClass="errorHra" Display="Dynamic"
                        ValidationGroup = "grpEdit" runat="server" ErrorMessage="Please enter SSN!">* Required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate = "txtBSSNE" 
                    ValidationExpression="^\d{9}$" 
                    runat="server" Display="Dynamic" ValidationGroup = "grpEdit" ErrorMessage = "Correct SSN format is 'xxxxxxxxx'"/> 
                    <br class="br_e" />                                
                    <asp:Label ID="lblorderE" runat="server" Text="Order No" CssClass="labelHra"></asp:Label>                                
                    <asp:DropDownList ID="ddlOrderE" runat="server" CssClass="dropHra">                        
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvOrderE" ControlToValidate="ddlOrderE" CssClass="errorHra" Display="Dynamic"
                        runat="server" ValidationGroup = "grpEdit" >* Required</asp:RequiredFieldValidator> 
                    <br class="br_e" />                              
                    <asp:Label ID="lblBFirstE" runat="server" Text="First Name" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBFirstE" runat="server" CssClass="inputHra"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFirstE" CssClass="errorHra" ControlToValidate="txtBFirstE" Display="Dynamic"
                        runat="server" ValidationGroup = "grpEdit" ErrorMessage="Please enter First Name!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" />                               
                    <asp:Label ID="lblBMIE" runat="server" Text="MI" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBMIE" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <br class="br_e" />                               
                    <asp:Label ID="lblBLastE" runat="server" Text="Last Name" CssClass="labelHra"></asp:Label>                                
                    <asp:TextBox ID="txtBLastE" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvLastE" CssClass="errorHra" ControlToValidate="txtBLastE" Display="Dynamic"
                        runat="server" ValidationGroup = "grpEdit" ErrorMessage="Please enter Last Name!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" /> 
                    <asp:Label ID="lblBBirthE" runat="server" Text="Birth Dt" CssClass="labelHra"></asp:Label>                               
                    <asp:TextBox ID="txtBBirthE" runat="server" CssClass="inputHra"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="rfvBirthE" ControlToValidate="txtBBirthE" runat="server" Display="Dynamic"
                        ValidationGroup = "grpEdit" CssClass="errorHra" ErrorMessage="Please enter Birth Date!">* Required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate = "txtBBirth" 
                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}" 
                    runat="server" Display="Dynamic" ValidationGroup = "grpEdit" ErrorMessage = "Correct Date format is mm/dd/yyyy"/> 
                    <br class="br_e" />                             
                    <asp:Label ID="lblRelationE" runat="server" Text="Relationship" CssClass="labelHra"></asp:Label>                                
                    <asp:DropDownList ID="ddlRelationE" AutoPostBack="true" runat="server" CssClass="dropHra" 
                        OnSelectedIndexChanged ="ddlRelationE_SelectedIndexchange">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem Value="0" Text = "SP" />
                        <asp:ListItem Value="1" Text = "CH" />
                        <asp:ListItem Value="2" Text = "OTH" />
                    </asp:DropDownList> 
                    <asp:RequiredFieldValidator ID="rfvRelationE" CssClass="errorHra" Display="Dynamic"
                        ControlToValidate="ddlRelationE" runat="server" ValidationGroup = "grpEdit" InitialValue="--Select--">* Required</asp:RequiredFieldValidator> 
                    <br class="br_e" /> 
                    <div id = "relOtherDivE" runat="server" style="" visible="false">
                        <asp:Label ID="lblOtherE" runat="server" Text="If selected other:" CssClass="centerHra1"></asp:Label>
                        <br class="br_e" />
                        <asp:TextBox ID="txtOtherE" runat="server" CssClass="centerHra2"></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="Label5" runat="server" Text="  (Max 200 characters)" CssClass="centerHra1"></asp:Label>
                    </div>
                    <br class="br_e" />  
                    <br class="br_e" />                             
                    <asp:Label ID="lblBSexE" runat="server" Text="Sex" CssClass="labelHra"></asp:Label>                                
                    <asp:RadioButtonList ID="rdbBSexE" runat="server" CssClass="radioHra" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Text="Male"/>
                        <asp:ListItem Value="1" Text="Female"/>
                    </asp:RadioButtonList> 
                    <asp:RequiredFieldValidator ID="rfvSexE" CssClass="centerHra2" ControlToValidate="rdbBSexE" Display="Dynamic"
                        runat="server" ValidationGroup = "grpEdit" ErrorMessage="Please select Gender!">* Required</asp:RequiredFieldValidator>
                    <br class="br_e" />   
                    <div id = "BValidDivE" runat="server" style="" visible="true">
                        <asp:Label ID="lblBValidE" runat="server" Text="Validation" CssClass="labelHra"></asp:Label>
                        <asp:DropDownList ID="ddlBValidE" runat="server" AutoPostBack="true" CssClass="dropHra" OnSelectedIndexChanged="validDate">
                            <asp:ListItem>--Select--</asp:ListItem>                             
                            <asp:ListItem Text="Valid"></asp:ListItem>                            
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="rfvBValidE" CssClass="errorHra" Display="Dynamic" Enabled = "false"
                            ControlToValidate="ddlBValidE" runat="server" ValidationGroup = "grpEdit" InitialValue="--Select--">* Required</asp:RequiredFieldValidator>
                        <br class="br_e" />                    
                        <asp:Label ID="lblVDateE" runat="server" Text="Validation Date" CssClass="labelHra"></asp:Label>                                            
                        <asp:TextBox ID="txtVDateE" runat="server" CssClass="inputHra"></asp:TextBox>
                        <asp:Label ID="Label4" runat="server" Text="MM/DD/YYYY" ></asp:Label>
                        <asp:RequiredFieldValidator ID="rfvVDateE" ControlToValidate="txtVDateE" Display="Dynamic" Enabled="false"
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                        <asp:RegularExpressionValidator ID="revVDateE" ControlToValidate = "txtVDateE" 
                            ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}" 
                            runat="server" Display="Dynamic" ValidationGroup = "grpEdit" CssClass="errorHra" ErrorMessage = "Correct Date format is mm/dd/yyyy" Text="*"/>    
                         <br class="br_e" />
                        <asp:Label ID="lblElegbE" runat="server" Text="Not Eligible" CssClass="labelHra"></asp:Label>                                
                        <asp:CheckBox ID="cbxElegbE" runat="server" CssClass="checkHra" AutoPostBack="true" 
                            OnCheckedChanged="cbxElegbE_OnCheckedChanged"/> 
                        <br class="br_e" />                    
                        <asp:Label ID="lblElegbEnotes" runat="server" Text="Notes" CssClass="labelHra"></asp:Label>                                            
                        <asp:TextBox ID="txtElegbEnotes" runat="server" CssClass="inputHra" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvElgbNote" ControlToValidate="txtElegbEnotes" Display="Dynamic" Enabled="false"
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>                        
                    </div> 
                    <br class="br_e" />                          
                    <asp:Label ID="lblOwnerE" runat="server" Text="Current Owner" CssClass="labelHra"></asp:Label>                                
                    <asp:CheckBox ID="cbxOwnerE" runat="server" CssClass="checkHra" AutoPostBack="true" 
                        OnCheckedChanged="cbxOwnerE_OnCheckedChanged"/> 
                    <asp:Label ID="lblownerError" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>                                       
                    <br class="br_e" />                    
                    <asp:Label ID="lblEDate" runat="server" Text="Effective Dt" CssClass="labelHra"></asp:Label>
                    <asp:TextBox ID="txtEDate" runat="server" CssClass="inputHra"></asp:TextBox>
                    <asp:Label ID="lblDatemsg" runat="server" Text="MM/DD/YYYY" ></asp:Label>                                        
                    <asp:RegularExpressionValidator ID="revbEffdt" ControlToValidate = "txtEDate" 
                    ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}" 
                    runat="server" Display="Dynamic" ValidationGroup = "grpEdit" CssClass="errorHra" ErrorMessage = "Correct Date format is mm/dd/yyyy" Text="*"/>                    
                    <br class="br_e" />
                    <div id = "BOwnerStopdtDiv" runat="server" style="" visible="false">
                        <asp:Label ID="lblBStopDtE" runat="server" Text="Stop Dt" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtBStopDtE" runat="server" CssClass="inputHra"></asp:TextBox>
                        <asp:Label ID="Label2" runat="server" Text="MM/DD/YYYY" ></asp:Label>                                        
                        <asp:RegularExpressionValidator ID="revBStopDtE" ControlToValidate = "txtBStopDtE" 
                        ValidationExpression="(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}" 
                        runat="server" Display="Dynamic" ValidationGroup = "grpEdit" CssClass="errorHra" ErrorMessage = "Correct Date format is mm/dd/yyyy" Text="*"/>                    
                        <br class="br_e" />
                    </div>                                     
                    <br class="br_e" />
                    <asp:Label ID="lblAddDiffE" runat="server" Text="Address Different?" CssClass="labelHra"></asp:Label>                                
                    <asp:CheckBox ID="cbxBAddrDiffE" runat="server" CssClass="checkHra" 
                        AutoPostBack="true" OnCheckedChanged="cbxBAddrDiffE_OnCheckedChanged" />
                    <br class="br_e" />
                    <div id = "BAddrDivE" runat="server" style="">                    
                        <asp:Label ID="lblBAddrE" runat="server" Text="Address (if Diff)" CssClass="labelHra"></asp:Label>
                        <asp:TextBox ID="txtBAddrE" runat="server" CssClass="inputHra"  ></asp:TextBox> 
                        <asp:RequiredFieldValidator ID="rfvAddrE" ControlToValidate="txtBAddrE" Display="Dynamic" Enabled="false"
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                        <br class="br_e" />                             
                        <asp:Label ID="lblBAddr2E" runat="server" Text="Address 2" CssClass="labelHra"></asp:Label>                               
                        <asp:TextBox ID="txtBAddr2E" runat="server" CssClass="inputHra" ></asp:TextBox>
                        <br class="br_e" />
                        <asp:Label ID="lblBCityE" runat="server" Text="City" CssClass="labelHra"></asp:Label> 
                        <asp:TextBox ID="txtBCityE" runat="server" CssClass="inputHra" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCityE" ControlToValidate="txtBCityE"  Display="Dynamic" Enabled="false"
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                        <br class="br_e" />
                        <asp:Label ID="lblBStateE" runat="server" Text="State" CssClass="labelHra"></asp:Label> 
                        <asp:TextBox ID="txtBStateE" runat="server" CssClass="inputHra" ></asp:TextBox> 
                        <asp:RequiredFieldValidator ID="rfvStateE" ControlToValidate="txtBStateE"  Display="Dynamic" Enabled="false"
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator>  
                        <br class="br_e" />  
                        <asp:Label ID="lblBZipE" runat="server" Text="Zip" CssClass="labelHra"></asp:Label>    
                        <asp:TextBox ID="txtBZipE" runat="server" CssClass="inputHra" ></asp:TextBox> 
                        <asp:RequiredFieldValidator ID="rfvZipE" ControlToValidate="txtBZipE"  Display="Dynamic" Enabled="false" 
                            runat="server" ValidationGroup = "grpEdit" CssClass="errorHra" >* Required</asp:RequiredFieldValidator> 
                    </div>    
                    <br class="br_e" /> 
                    <br class="br_e" /> 
                    <table> 
	                <tr> 
                        <td style="margin-left:100px;margin-right:20px;margin-top:20px;margin-bottom:20px;">
                            <asp:LinkButton ID="btn2Edit" runat="server" 
                                OnClientClick="this.blur();" Font-Underline="false" 
                                CssClass="imgbutton"  OnClick="btn2Edit_Click" ><span>Edit</span></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btn2Save" runat="server" 
                                OnClientClick="this.blur();" Font-Underline="false" 
                                CssClass="imgbutton" ValidationGroup = "grpEdit" OnClick="btn2Save_Click"><span>Save</span></asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btn2Cancel" runat="server"  
                                OnClientClick="this.blur();" Font-Underline="false" 
                                CssClass="imgbutton" OnClick="btn2Cancel_Click" ><span>Cancel</span></asp:LinkButton>
                        </td>
                         <td>
                            <asp:LinkButton ID="btn2Close" runat="server"  
                                OnClientClick="this.blur();" Font-Underline="false" 
                                CssClass="imgbutton" OnClick="btn2Close_Click" ><span>Close</span></asp:LinkButton>
                        </td>
	                </tr>
	            </table>
                </div>  
                <div class="error" id="errorDivBen" runat="server" visible="false">
                    <asp:Label ID="lblEError" runat="server" Text=""></asp:Label>                    
                </div>  
                </asp:View>
                <asp:View ID="View3" runat="server">     
                <div id = "formHra">
                    <fieldset>
                        <legend>Audit Information</legend>
                        <div id= "introText" style="width:650px"> 
                            <asp:Label ID="Label1" runat="server" Text="Pilot Audit Information" ForeColor="#5D478B"></asp:Label>
	                    </div>
                        <div style="padding-top:3px;width:600px;margin-left:20px;max-height:450px;margin-top:10px;"  class="scroller">
                        <asp:CompleteGridView runat="server" ID="grdvAudit" CssClass="tablestyle"                       
                        AutoGenerateColumns="False"  AllowSorting="True" 
                        PagerSettings-Mode="NumericFirstLast" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" 
                        DataSourceID="SqlDataSource2" 
                        SortAscendingImageUrl="" SortDescendingImageUrl=""> 
                        <PagerSettings Mode="NextPreviousFirstLast" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />               
                        <PagerStyle CssClass="customGridpaging" />
                            <columns>                                                         
                                <asp:BoundField DataField="ColumnName" HeaderText="Column Name" SortExpression="ColumnName"></asp:BoundField>
                                <asp:BoundField DataField="OldValue" HeaderText="Old Value" SortExpression="OldValue"></asp:BoundField>
                                <asp:BoundField DataField="NewValue" HeaderText="New Value" SortExpression="NewValue"></asp:BoundField>
                                <asp:BoundField DataField="UserId" HeaderText="User Name" SortExpression="UserId"></asp:BoundField> 
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate"></asp:BoundField>
                            </columns>                    
                        </asp:CompleteGridView><asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT ColumnName, OldValue,NewValue,CONVERT(VARCHAR(20),[taskDate],101) AS [taskDate],[UserId] FROM hra_auditEmployeeChanges
                                            WHERE [PrimaryKey] = @enum">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtEmpNo" Name="enum" PropertyName="Text"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        </div>
                        <div id= "introText" style="width:650px"> 
                            <asp:Label ID="Label3" runat="server" Text="Pilot - Beneficiary Audit Information" ForeColor="#5D478B"></asp:Label>
	                    </div>
                        <div style="padding-top:3px;width:600px;margin-left:20px;max-height:450px;margin-top:10px;"  class="scroller">
                        <asp:CompleteGridView runat="server" ID="grdvDepAudit" CssClass="tablestyle"                       
                        AutoGenerateColumns="False"  AllowSorting="True" 
                        PagerSettings-Mode="NumericFirstLast" 
                        PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                        PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                        PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                        PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" 
                        DataSourceID="SqlDataSource10" 
                        SortAscendingImageUrl="" SortDescendingImageUrl=""> 
                        <PagerSettings Mode="NextPreviousFirstLast" />
                        <AlternatingRowStyle CssClass="altrowstyle" />
                        <HeaderStyle CssClass="headerstyle" />
                        <RowStyle CssClass="rowstyle" />               
                        <PagerStyle CssClass="customGridpaging" />
                            <columns> 
                                <asp:BoundField DataField="tPrimaryName" HeaderText="Dependent SSN" SortExpression="tPrimaryName"></asp:BoundField>                                                        
                                <asp:BoundField DataField="tColumn" HeaderText="Column Name" SortExpression="tColumn"></asp:BoundField>
                                <asp:BoundField DataField="tOldValue" HeaderText="Old Value" SortExpression="tOldValue"></asp:BoundField>
                                <asp:BoundField DataField="tNewValue" HeaderText="New Value" SortExpression="tNewValue"></asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"></asp:BoundField> 
                                <asp:BoundField DataField="taskDate" HeaderText="Task Date" SortExpression="taskDate"></asp:BoundField>
                            </columns>                    
                        </asp:CompleteGridView><asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                            SelectCommand="SELECT [tPrimaryName],[tColumn],[tOldValue],[tNewValue],[UserName],CONVERT(VARCHAR(20),[taskDate],101) AS [taskDate] FROM UserTasks_AU b,UserSession_AU a
                                            WHERE b.moduleName = 'HRA' AND b.sessionID = a.SessionId AND b.tPrimaryKey = @enum AND tTable IN ('Dependant', 'Address')">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtEmpNo" Name="enum" PropertyName="Text"
                                    Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        </div>
                    </fieldset>
                </div>                        
                </asp:View>
                <asp:View ID="View4" runat="server"> 
                 <div id = "formHra">
                    <table>
                        <tr> 
                            <td style="padding-top:10px;padding-left:450px;">
                                <asp:ImageButton ID="ImageButton1" runat="server" 
                                    OnClick="lnk_genTranRpt_OnClick" ImageUrl="~/styles/images/Excel-32.gif" />
                            </td>                          
                            <td style="padding-top:10px;padding-left:10px;">
                                <asp:LinkButton ID="lnk_genTranRpt" runat="server" OnClick="lnk_genTranRpt_OnClick" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" ><span>Excel Report</span></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <fieldset>
                        <legend>Transactions Information</legend>                         
                        <div style="padding-top:3px;width:600px;margin-left:20px;max-height:450px;margin-top:10px;"  class="scroller">
                            <asp:GridView runat="server" ID="grdvTransactions" CssClass="tablestyle"                       
                            AutoGenerateColumns="false"  AllowSorting="True"                        
                            OnSorting = "grdvTransactions_Sorting"
                            PagerSettings-Mode="NumericFirstLast" 
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
                                <asp:BoundField DataField="YRMO" HeaderText="YRMO" SortExpression="YRMO" />
                                <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" />
                                <asp:BoundField DataField="Tran Code" HeaderText="Tran Code" SortExpression="Tran Code" />
                                <asp:BoundField DataField="Tran Date" HeaderText="Tran Date" SortExpression="Tran Date" />
                                <asp:BoundField DataField="Putnam Amount" HeaderText="Putnam Amount" SortExpression="Putnam Amount" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="PutnamAdj Amount" HeaderText="PutnamAdj Amount" SortExpression="PutnamAdj Amount" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Wageworks Amount" HeaderText="Wageworks Amount" SortExpression="Wageworks Amount" ItemStyle-HorizontalAlign="Right" />
                             </Columns>                                         
                            </asp:GridView>
                        </div>
                    </fieldset>
                </div>      
                </asp:View>
                <asp:View ID="View5" runat="server"> 
                    <div id = "formHra">
                        <fieldset>
                            <legend>Letters History Information</legend>  
                            <div id= "introText" style="width:650px"> 
                                <asp:Label ID="lblHeading1" runat="server" Text="Generated Letters" ForeColor="#5D478B"></asp:Label>
	                        </div>
	                        <div style="padding-top:3px;width:600px;margin-left:20px;max-height:450px;margin-top:10px;"  class="scroller">
                                <asp:GridView runat="server" ID="grdvLetters1" CssClass="tablestyle"                       
                                AutoGenerateColumns="False"  AllowSorting="True" 
                                PagerSettings-Mode="NumericFirstLast" DataSourceID="SqlDataSource3"
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
                                    <asp:BoundField DataField="lgId" HeaderText="genId" InsertVisible="False" Visible="False" ReadOnly="True"
                                        SortExpression="lgId" />               
                                    <asp:BoundField DataField="ltrType" HeaderText="Letter Type" SortExpression="ltrType" />
                                    <asp:BoundField DataField="ltrTypeCode" HeaderText="Letter Code" SortExpression="ltrTypeCode" />
                                    <asp:BoundField DataField="tpVersion" HeaderText="Version" SortExpression="tpVersion" />
                                    <asp:BoundField DataField="lhdate" HeaderText="Print Date" SortExpression="lhdate" />   
                                    <asp:BoundField DataField="lgUser" HeaderText="User" SortExpression="lgUser" />                                                                        
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReprint" runat="server" OnClick="lnkrePrint_onclick">Re-Print</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                             
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                    SelectCommand="SELECT lgId,lguser,ltrType,ltrTypeCode,tpVersion,lhdate
                                        FROM hra_Ltrs_template,hra_Ltrs_History,hra_Ltrs_Generated,hra_Ltrs_Type
                                        WHERE hra_Ltrs_History.lhgenId = hra_Ltrs_Generated.lgId
                                        AND hra_Ltrs_Generated.lgLetterId = hra_Ltrs_template.tpId
                                        AND hra_Ltrs_template.tpTypeId = hra_Ltrs_Type.ltrId AND ([lhEmpnum] = @lhEmpnum)" 
                                        ConnectionString="<%$ ConnectionStrings:EBADB %>">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtEmpNo" Name="lhEmpnum" PropertyName="Text" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                            <div id= "introText"> 
                                <asp:Label ID="lblSubheading2" runat="server" Text="Validation Letters - Pending" ForeColor="#5D478B"></asp:Label>
	                        </div>
	                        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                                <asp:GridView runat="server" ID="grdvltrPending" CssClass="tablestyle"                       
                                    AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource4"
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
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommand="SELECT [pnId],[pnEmpNum],[pnDepSSN],[pnDepRelationship],[pnDate] FROM hra_Ltrs_Pending 
                                                    WHERE [pnStatus] = '1'  AND [pnLtrType] = 'val' AND [pnEmpNum] = @lhEmpnum">  
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtEmpNo" Name="lhEmpnum" PropertyName="Text" Type="Int32" />
                                    </SelectParameters>                                 
                                </asp:SqlDataSource>
                            </div>
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
                                    SelectCommand="SELECT [pnId],[pnEmpNum],[pnDate] FROM hra_Ltrs_Pending 
                                                    WHERE [pnStatus] = '1' AND [pnLtrType] = 'conf' AND [pnEmpNum] = @lhEmpnum">   
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtEmpNo" Name="lhEmpnum" PropertyName="Text" Type="Int32" />
                                    </SelectParameters>                                 
                                </asp:SqlDataSource>
                            </div> 
                            <div class="error" id="errorDivl" runat="server" visible="false">
                                <asp:Label ID="lbl_errorl" runat="server" Text=""></asp:Label>                    
                            </div>  
	                        <div id= "introText" style="width:650px"> 
                                <asp:Label ID="lblHeading2" runat="server" Text="Archived Letters" ForeColor="#5D478B"></asp:Label>
	                        </div>  
	                        <div style="padding-top:20px;width:680px;margin-left:20px;max-height:450px;"  class="scroller">
                                <asp:GridView runat="server" ID="grdvArchive" CssClass="tablestyle"                       
                                    AutoGenerateColumns="false"  AllowSorting="True" DataSourceID="SqlDataSource11"
                                    PagerSettings-Mode="NumericFirstLast" 
                                    PagerSettings-FirstPageImageUrl="~/styles/images/firstPage.gif"
                                    PagerSettings-NextPageImageUrl="~/styles/images/nextPage.gif"
                                    PagerSettings-PreviousPageImageUrl="~/styles/images/prevPage.gif"
                                    PagerSettings-LastPageImageUrl="~/styles/images/lastPage.gif" > 
                                    <PagerSettings Mode="NextPreviousFirstLast" FirstPageImageUrl="~/styles/images/firstPage.gif" LastPageImageUrl="~/styles/images/lastPage.gif" NextPageImageUrl="~/styles/images/nextPage.gif" PreviousPageImageUrl="~/styles/images/prevPage.gif" />
                                    <AlternatingRowStyle CssClass="altrowstyle" />
                                    <HeaderStyle CssClass="headerstyle" />
                                    <RowStyle CssClass="rowstyle" />               
                                    <PagerStyle CssClass="customGridpaging" /> 
                                    <Columns>
                                        <asp:BoundField DataField="Type"  HeaderText = "Letter Type" SortExpression="Type" />                                                   
                                        <asp:BoundField DataField="Date1" HeaderText="Date1" SortExpression="Date1" />   
                                        <asp:BoundField DataField="Date2" HeaderText="Date2" SortExpression="Date2" />                                  
                                        <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink ID = "archivePath" Target="_blank" runat="server" NavigateUrl='<%# Bind("FilePath") %>'>Open</asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                         
                                    </Columns>    
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:EBADB %>"
                                    SelectCommandType="StoredProcedure" SelectCommand="sp_HRAgetEmployeeArchive">   
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtEmpNo" Name="empNum" PropertyName="Text" Type="Int32" />
                                    </SelectParameters>                                 
                                </asp:SqlDataSource>
                            </div>                
                        </fieldset>
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
    <cc1:SmartScroller ID="SmartScroller1" runat="server">
    </cc1:SmartScroller>
</asp:Content>


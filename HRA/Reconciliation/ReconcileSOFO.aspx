<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ReconcileSOFO.aspx.cs" Inherits="HRA_Reconcile_SOFO" Title="HRA SOFO Reconciliation"  %>
<%@ Register TagPrefix = "sstchur" Namespace = "sstchur.web.SmartNav" Assembly = "sstchur.web.smartnav" %>
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
                    <ul id="expand1">
                        <li><a>Import</a>
		                    <ul>
		                        <li><asp:HyperLink ID="hypPutnam" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnam.aspx">Putnam</asp:HyperLink></li>
		                        <li><asp:HyperLink ID="hypPutnamAdj" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportPutnamAdj.aspx">PutnamAdj</asp:HyperLink></li>	
		                        <li><asp:HyperLink ID="hypWageworks" runat="server" NavigateUrl="~/HRA/Reconciliation/ImportWageworks.aspx">Wageworks</asp:HyperLink></li>	                        
		                    </ul>
		                </li>                        
                        <li><asp:HyperLink ID="hypReconcile" runat="server" NavigateUrl="~/HRA/Reconciliation/Reconcile.aspx">Reconcile</asp:HyperLink></li>	
                        <li><asp:HyperLink ID="hypReconcileSOFO" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Reconciliation/ReconcileSOFO.aspx">Reconcile SOFO</asp:HyperLink></li>   
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
        <div id = "introText" style="width:720px;padding-left:-10px;">Reconcile HRA Summary of Fund Operations</div>          
        <div class = "Menu" style="width:730px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="File Data Import" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="Web Form Data Import" Value="1"></asp:MenuItem>                    
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
	    <div id = "multiView" style="padding-bottom:15px; padding-top:10px; width:730px;"> 
            <asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
                <asp:View id="view1" runat="server">
                    <div class="warning" id="div1_alert" runat="server" visible="false" style="margin-top:5px;">
                        <asp:Label ID="lbl1_alert" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="error" id="div1_error" runat="server" visible="false" style="margin-top:5px;">
                        <asp:Label ID="lbl_error1" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="fileSOFO" runat="server" style="float:left;">                        
                        <div style="margin-top:10px;padding-left:19px">                            
                            <asp:Label ID="lblPrevYrmo1" runat="server" Text="YRMO :" style="margin-top:5px;"></asp:Label>
                            <asp:DropDownList ID="ddlYrmo1" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO1_selectedIndexchanged">                        
                            </asp:DropDownList>
                            <div id="DivNewYRMO1" runat="server" visible="false" style="background-color:#F7F7F7; display:block; width:600px; padding-bottom:10px; padding-top:10px;margin-left:-10px;">
                                <asp:Label ID="Label3" runat="server" Text="YRMO :" style="margin-left:10px;"></asp:Label>
                                <asp:TextBox ID="txtPrevYRMO1" runat="server" Width="100px" OnTextChanged="txtPrevYRMO1_textChanged" ValidationGroup="yrmoGroup"></asp:TextBox>
                                <asp:Label ID="lbl_yrmofrmt1" runat="server" ForeColor="#7D7D7D" Text="(yyyymm)"></asp:Label>
                                <asp:Button ID="btnCancelYrmo1" runat="server" CausesValidation="false"  Text="Cancel" OnClick="btn_CancelYrmo1" style="margin-left:10px; margin-right:5px;"/>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator8" runat="server"
                                ControlToValidate="txtPrevYRMO1"
                                ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                                Display="Dynamic"
                                Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                                Enter YRMO in format 'yyyymm'
                                </asp:RegularExpressionValidator>
                                <p style="padding-top:10px; padding-left:10px;">Enter the YRMO not listed in the drop down list.</p>
                            </div>
                        </div>
                        <asp:MultiView id="MultiView3" runat="server" ActiveViewIndex="0">                                            
                            <asp:View id="view_main1" runat="server">
                                <div id="upload_manually" style="padding-top:15px; padding-left:10px;" runat="server">
                                    <FileUpload:FileUpload1 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup"  FileTypeRange="csv"/> 
                                    <fieldset style="width:600px; margin-left:9px; padding-bottom:15px;">
                                        <legend style="color:#5D478B;">Manual Adjustment</legend>                                        
                                        <asp:Label ID="lbl_adjAmt1" runat="server" Text="Amount:" CssClass="labelHra1" AssociatedControlID="tbx_adjAmt1"></asp:Label>
                                        <asp:TextBox ID="tbx_adjAmt1" runat="server" CssClass="inputHra1"></asp:TextBox>
                                        <asp:RegularExpressionValidator id="RegularExpressionValidator9" runat="server"
                                        ControlToValidate="tbx_adjAmt1" Display="Dynamic"
                                        ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                        ValidationGroup = "fileGroup" style="margin-left:5px;"
                                        >Not a valid currency format!                   
                                        </asp:RegularExpressionValidator> 
                                        <br class = "br_e" /> 
                                        <asp:Label ID="lbl_adjNotes1" runat="server" Text="Notes:" CssClass="labelHra1" AssociatedControlID="tbx_adjNotes1"></asp:Label>
                                        <asp:TextBox ID="tbx_adjNotes1" runat="server" TextMode="MultiLine" CssClass="inputHra1" style="width:200px; height:60px;"></asp:TextBox>                                        
                                        <asp:RegularExpressionValidator id="RegularExpressionValidator14" runat="server"
                                        ControlToValidate="tbx_adjNotes1" Display="Dynamic"
                                        ValidationExpression="^[&quot;a-zA-Z0-9\040]+$"
                                        Font-Names="Verdana" Font-Size="10pt"
                                        ValidationGroup = "fileGroup" style="margin-left:5px;"
                                        >Accepts words or quoted phrases!                   
                                        </asp:RegularExpressionValidator>
                                        <br class = "br_e" /> 
                                    </fieldset>                           
                                    <asp:LinkButton ID="btn_manImport" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" style="margin-left:19px; margin-bottom:40px; margin-top:15px;" ValidationGroup="fileGroup" OnClick="btn_manImport_Click"><span>Reconcile</span></asp:LinkButton> 
                                </div>
                                <div id="upload_auto" style="margin-top:15px;padding-left:10px;" runat="server">
                                    <asp:Label ID="lblFile" runat="server" Text="File :" style="padding-left:23px; float:left;" />
                                    <asp:Label ID="lbl_clientfile" runat="server" Width="600px" style="font-size:105%; float:left;" ForeColor="#000099" Font-Size="12px"></asp:Label>
                                    <br class = "br_e" /> 
                                    <fieldset style="width:600px; margin-left:9px; padding-bottom:15px; margin-top:20px; float:left;">
                                        <legend style="color:#5D478B;">Manual Adjustment</legend>                                        
                                        <asp:Label ID="lbl_adjAmt2" runat="server" Text="Amount:" CssClass="labelHra1" AssociatedControlID="tbx_adjAmt2"></asp:Label>
                                        <asp:TextBox ID="tbx_adjAmt2" runat="server" CssClass="inputHra1"></asp:TextBox>
                                        <asp:RegularExpressionValidator id="RegularExpressionValidator10" runat="server"
                                        ControlToValidate="tbx_adjAmt2" Display="Dynamic"
                                        ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                        ValidationGroup = "fileGroup" style="margin-left:5px;"
                                        >Not a valid currency format!                   
                                        </asp:RegularExpressionValidator> 
                                        <br class = "br_e" /> 
                                        <asp:Label ID="lbl_adjNotes2" runat="server" Text="Notes:" CssClass="labelHra1" AssociatedControlID="tbx_adjNotes2"></asp:Label>
                                        <asp:TextBox ID="tbx_adjNotes2" runat="server" TextMode="MultiLine" CssClass="inputHra1" style="width:200px; height:60px;"></asp:TextBox>                                        
                                        <asp:RegularExpressionValidator id="RegularExpressionValidator13" runat="server"
                                        ControlToValidate="tbx_adjNotes2" Display="Dynamic"
                                        ValidationExpression="^[&quot;a-zA-Z0-9\040]+$"
                                        Font-Names="Verdana" Font-Size="10pt"
                                        ValidationGroup = "fileGroup" style="margin-left:5px;"
                                        >Accepts words or quoted phrases!                   
                                        </asp:RegularExpressionValidator> 
                                        <br class = "br_e" /> 
                                    </fieldset>  
                                    <asp:LinkButton ID="btn_autoImport" ValidationGroup = "fileGroup" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="imgbutton" style="margin-left:19px; margin-bottom:40px; margin-top:15px; display:block;" OnClick="btn_autoImport_Click"><span>Reconcile</span></asp:LinkButton>                                                           
                                </div>
                            </asp:View>
                            <asp:View id="view_result1" runat="server">                        
                                <asp:Label ID="lbl_result1" runat="server" style="margin-top:10px; display:block; padding-left:20px; color:#5D478B;width:650px;float:left;"></asp:Label>
                                <asp:Button ID="btn_genRpt1" runat="server" Text="Excel Report" OnClick="btn_genRpt1_Click" style="margin-bottom:30px;margin-top:15px;display:block;margin-left:20px;"/>                                                                                            
                            </asp:View> 
                            <asp:View id="view_reconAgn1" runat="server">                        
                                <asp:Label ID="lbl_reconAgn1" runat="server" style="margin-top:10px; padding-left:20px; display:block;width:650px;float:left;"></asp:Label>
                                <asp:Button ID="btn_results1" runat="server" Text="View Results" OnClick="btn_results1_Click" style="margin-top:15px;margin-left:20px;margin-bottom:30px;"/>                  
                                <asp:Button ID="btn_reconAgn1" runat="server" Text="Re-Reconcile" OnClick="btn_reconAgn1_Click" style="margin-top:15px;margin-left:10px; margin-bottom:30px;"/>
                                <asp:Label ID="lbl_note1" runat="server" Text="Note: <i>Clicking 'Re-Reconcile' deletes reconciliation data for the selected Year-Month</i>" style="margin-bottom:15px; display:block;"></asp:Label>
                            </asp:View>
                        </asp:MultiView> 
	                </div>
	                <div id = "resultDiv1" runat="server" visible="false">
                        <div id = "summaryDiv1" runat="server">
                            <div style="margin-top:20px; color:#5D478B;width:400px; margin-left:5px;">Putnam Summary of Fund Operations Reconciliation Results</div>               
                            <div style="float:left;width:550px;background-color:#FCFCFC; margin-left:5px; margin-top:8px;">
                                <asp:GridView ID="grdvSum1" runat="server"  Width="550px" CssClass="tablestyle" AutoGenerateColumns="false"
                                CellPadding="4" ForeColor="#333333" GridLines="None">                               
                                    <AlternatingRowStyle CssClass="altrowstyle" HorizontalAlign="Left"/>
                                    <HeaderStyle CssClass="headerstyle"/>
                                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left"/>
                                    <Columns>
                                        <asp:BoundField DataField="Title" HeaderStyle-Font-Bold="false" ItemStyle-ForeColor="#2F2F4F" HeaderStyle-ForeColor="#302B54" HeaderText="Summary vs. Detail"/>
                                        <asp:BoundField DataField="Value" ItemStyle-ForeColor="#42426F" />
                                    </Columns>                         
                                </asp:GridView>
                            </div>
                        </div>                    
                    </div>
                </asp:View>
                <asp:View id="view2" runat="server">
                     <div class="warning" id="div2_alert" runat="server" visible="false" style="margin-top:5px;">
                        <asp:Label ID="lbl2_alert" runat="server" Text=""></asp:Label>
                    </div>
                     <div class="error" id="div2_error" runat="server" visible="false" style="margin-top:5px;">
                        <asp:Label ID="lbl_error2" runat="server" Text=""></asp:Label>
                    </div>
                    <div id = "formSOFO" runat="server" style="float:left;margin-top:5px;"> 
                        <asp:MultiView id="MultiView1" runat="server" ActiveViewIndex="0">
                            <asp:View id="view_main2" runat="server"> 
                                <asp:Label ID="lblPrevYrmo2" runat="server" AssociatedControlID="ddlYrmo2" Text="Year-Month:" CssClass="labelHra1"></asp:Label>
                                <asp:DropDownList ID="ddlYrmo2" runat="server" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYRMO2_selectedIndexchanged" CssClass="inputHra1"></asp:DropDownList>
                                <div id="DivNewYRMO2" runat="server" visible="false" style="background-color:#F7F7F7; display:block; width:600px; padding-bottom:10px; padding-top:10px; margin-left:40px;">
                                    <asp:Label ID="Label5" runat="server" Text="YRMO :" style="margin-left:10px;" AssociatedControlID="txtPrevYRMO2"></asp:Label>
                                    <asp:TextBox ID="txtPrevYRMO2" runat="server" Width="100px" OnTextChanged="txtPrevYRMO2_textChanged" ValidationGroup="yrmoGroup"></asp:TextBox>
                                    <asp:Label ID="lbl_yrmofrmt2" runat="server" ForeColor="#7D7D7D" Text="(yyyymm)"></asp:Label>
                                    <asp:Button ID="btnCancelYrmo2" runat="server" CausesValidation="false"  Text="Cancel" OnClick="btn_CancelYrmo2" style="margin-left:10px; margin-right:5px;"/>
                                    <asp:RegularExpressionValidator id="RegularExpressionValidator7" runat="server"
                                        ControlToValidate="txtPrevYRMO2"
                                        ValidationExpression="^20\d\d(0[1-9]|1[012])$"
                                        Display="Dynamic"
                                        Font-Names="verdana" Font-Size="10pt" ValidationGroup="yrmoGroup">
                                        Enter YRMO in format 'yyyymm'
                                    </asp:RegularExpressionValidator>
                                    <p style="padding-top:10px; padding-left:10px;">Enter the YRMO not listed in the drop down list.</p>
                                </div>   
                                <br class = "br_e" />                            
                                <asp:Label ID="lblBegBal" AssociatedControlID="txtBegBal" runat="server" Text="Beginning Balance:" CssClass="labelHra1"></asp:Label>
                                <asp:TextBox ID="txtBegBal" runat="server" CssClass="inputHra1" ></asp:TextBox>                            
                                <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
                                ControlToValidate="txtBegBal" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;"
                                >Not a valid currency format!                  
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />                            
                                <asp:Label ID="lblTotContr" AssociatedControlID="txtTotContr" runat="server" Text="Total Contributions:" CssClass="labelHra1"></asp:Label>
                                <asp:TextBox ID="txtTotContr" runat="server" CssClass="inputHra1"></asp:TextBox>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"
                                ControlToValidate="txtTotContr" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;"
                                >Not a valid currency format!                   
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" /> 
                                <asp:Label ID="lblTotDiv" AssociatedControlID="txtTotDiv" runat="server" Text="Total Dividends:" CssClass="labelHra1"></asp:Label>
                                <asp:TextBox ID="txtTotDiv" runat="server" CssClass="inputHra1"></asp:TextBox>                            
                                <asp:RegularExpressionValidator id="RegularExpressionValidator3" runat="server"
                                ControlToValidate="txtTotDiv" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;"
                                >Not a valid currency format!                  
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />                           
                                <asp:Label ID="lblTermPay" AssociatedControlID="txtTermPay" runat="server" Text="Termination Payments:" CssClass="labelHra1"></asp:Label>
                                <asp:TextBox ID="txtTermPay" runat="server" CssClass="inputHra1"></asp:TextBox>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator4" runat="server"
                                ControlToValidate="txtTermPay" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;"
                                >Not a valid currency format!                   
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />
                                <asp:Label ID="lblWithDraw" AssociatedControlID="txtWithDraw" runat="server" Text="Withdrawals:" CssClass="labelHra1"></asp:Label>                            
                                <asp:TextBox ID="txtWithDraw" runat="server" CssClass="inputHra1"></asp:TextBox>                            
                                <asp:RegularExpressionValidator id="RegularExpressionValidator5" runat="server"
                                ControlToValidate="txtWithDraw" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;" 
                                >Not a valid currency format!                 
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />
                                <asp:Label ID="lblEndBal" AssociatedControlID="txtEndBal" runat="server" Text="Ending Balance:" CssClass="labelHra1"></asp:Label>     
                                <asp:TextBox ID="txtEndBal" runat="server" CssClass="inputHra1"></asp:TextBox>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator6" runat="server"
                                ControlToValidate="txtEndBal" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "grp1" style="margin-left:5px;"
                                >Not a valid currency format!                
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />           
                                <asp:Label ID="lbl_manAdjAmt" runat="server" Text="Manual Adj Amount:" CssClass="labelHra1" AssociatedControlID="tbx_manAdjAmt"></asp:Label>
                                <asp:TextBox ID="tbx_manAdjAmt" runat="server" CssClass="inputHra1"></asp:TextBox>
                                <asp:RegularExpressionValidator id="RegularExpressionValidator11" runat="server"
                                ControlToValidate="tbx_manAdjAmt" Display="Dynamic"
                                ValidationExpression="^\-?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$"
                                ValidationGroup = "fileGroup" style="margin-left:5px;"
                                >Not a valid currency format!                   
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" /> 
                                <asp:Label ID="lbl_manAdjNotes" runat="server" Text="Manual Adj Notes:" CssClass="labelHra1" AssociatedControlID="tbx_manAdjNotes"></asp:Label>
                                <asp:TextBox ID="tbx_manAdjNotes" runat="server" TextMode="MultiLine" CssClass="inputHra1" style="width:200px; height:60px;"></asp:TextBox>                                
                                <asp:RegularExpressionValidator id="RegularExpressionValidator12" runat="server"
                                ControlToValidate="tbx_manAdjNotes" Display="Dynamic"
                                ValidationExpression="^[&quot;a-zA-Z0-9\040]+$"
                                ValidationGroup = "fileGroup" style="margin-left:5px;"
                                >Accepts words or quoted phrases!                   
                                </asp:RegularExpressionValidator> 
                                <br class = "br_e" />
                                <div style="margin-top:25px;margin-left:180px;margin-bottom:30px">
                                     <asp:LinkButton ID="btnSubmit" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="squarebutton" ValidationGroup="grp1" OnClick="btnSubmit_Click" style="margin-right:30px"><span>Submit</span></asp:LinkButton> 
                                     <asp:LinkButton ID="btnReset" runat="server" OnClientClick="this.blur();" Font-Underline="false" CssClass="squarebutton" OnClick="btnReset_Click" ><span>Reset</span></asp:LinkButton>
                                </div>
                            </asp:View>
                            <asp:View id="view_result2" runat="server">                        
                                <asp:Label ID="lbl_result2" runat="server" style="margin-top:10px; display:block; padding-left:20px; color:#5D478B;float:left;width:650px;"></asp:Label>
                                <asp:Button ID="btn_genRpt2" runat="server" Text="Excel Report" OnClick="btn_genRpt2_Click" style="margin-bottom:30px;margin-top:15px;display:block;margin-left:20px;"/>                                                                                            
                            </asp:View> 
                            <asp:View id="view_reconAgn2" runat="server">                        
                                <asp:Label ID="lbl_reconAgn2" runat="server" style="margin-top:10px; padding-left:20px; display:block;width:650px;float:left;"></asp:Label>
                                <asp:Button ID="btn_results2" runat="server" Text="View Results" OnClick="btn_results2_Click" style="margin-top:15px;margin-left:20px;margin-bottom:30px;"/>                  
                                <asp:Button ID="btn_reconAgn2" runat="server" Text="Re-Reconcile" OnClick="btn_reconAgn2_Click" style="margin-top:15px;margin-left:10px; margin-bottom:30px;"/>
                                <asp:Label ID="lbl_note2" runat="server" Text="Note: <i>Clicking 'Re-Reconcile' deletes reconciliation data for the selected Year-Month</i>" style="margin-bottom:15px; display:block;"></asp:Label>
                            </asp:View>                            
                        </asp:MultiView> 
                    </div>
                     <div id = "resultDiv2" runat="server" visible="false">
                        <div id = "summaryDiv2" runat="server">
                            <div style="margin-top:20px; color:#5D478B;width:400px; margin-left:5px;">Putnam Summary of Fund Operations Reconciliation Results</div>               
                            <div style="float:left;width:550px;background-color:#FCFCFC; margin-left:5px; margin-top:8px;">
                                <asp:GridView ID="grdvSum2" runat="server"  Width="550px" CssClass="tablestyle" AutoGenerateColumns="false"
                                CellPadding="4" ForeColor="#333333" GridLines="None">                               
                                    <AlternatingRowStyle CssClass="altrowstyle" HorizontalAlign="Left"/>
                                    <HeaderStyle CssClass="headerstyle"/>
                                    <RowStyle CssClass="rowstyle" HorizontalAlign="Left"/>
                                    <Columns>
                                        <asp:BoundField DataField="Title" HeaderStyle-Font-Bold="false" ItemStyle-ForeColor="#2F2F4F" HeaderStyle-ForeColor="#302B54" HeaderText="Summary vs. Detail"/>
                                        <asp:BoundField DataField="Value" ItemStyle-ForeColor="#42426F" />
                                    </Columns>                         
                                </asp:GridView>
                            </div>
                        </div>                    
                    </div>
                </asp:View>
            </asp:MultiView>
	    </div>
	    <sstchur:SmartScroller ID="SmartScroller1" runat = "server" /> 
    </div>
</asp:Content>


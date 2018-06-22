<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PGP_FTP_EligFile.aspx.cs" Inherits="HRA_Operations_PGP_FTP_EligFile" Title="PGP & FTP Elig File" %>
<%@ Register TagPrefix="ucl" TagName="FileUpload2" Src="~/UserControls/FileUploadControl2.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contentleft">        
        <div id = "module_menu">
            <h1>HRA</h1>            
            <ul id="sitemap"> 
                <li><a>Operations</a>
                    <ul id="expand1">
                        <li><asp:HyperLink ID="hypEligFile" runat="server" NavigateUrl="~/HRA/Operations/Create_Eligibility_File.aspx">Eligibility File</asp:HyperLink></li>		                
                        <li><asp:HyperLink ID="hypPGP_FTP" runat="server" Font-Underline="true" NavigateUrl="~/HRA/Operations/PGP_FTP_EligFile.aspx">PGP & FTP File</asp:HyperLink></li>		                
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
            <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif"/>                       
        </div> 	
    </div>
    <div id="contentright">
        <div id = "introText" style="width:720px;padding-left:-10px;">PGP & FTP Eligibility File</div>
	    <div class = "Menu" style="width:732px;">
            <asp:Menu
                ID="Menu1"                
                runat="server"
                Orientation="Horizontal"
                StaticEnableDefaultPopOutImage="False" OnMenuItemClick="Menu1_MenuItemClick">
                <Items>
                    <asp:MenuItem Text="PGP" Value="0" Selected = "true"></asp:MenuItem>
                    <asp:MenuItem Text="FTP" Value="1"></asp:MenuItem>                    
                </Items>
                <StaticMenuItemStyle CssClass="tab" />
                <StaticSelectedStyle CssClass="selectedTab" />
                <StaticHoverStyle CssClass="selectedTab1" />                     
            </asp:Menu>        
        </div>
        <div id = "multiView" style="float:left;width:732px; margin-top:10px; padding-bottom:20px;"> 
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View id="view1" runat="server">
                    <div id = "importbox" style="margin-left:12px;">
	                      <fieldset runat="server" id="fs_PGP">
                            <legend style=" border:none;color:#5D478B;">PGP Encrypt Eligibility File</legend>
                            <div class="userPara" style="width:600px;margin-left:-1px;">
                                <p>Instructions : &nbsp; &nbsp; &nbsp;Download PGP folder if not done earlier</p>  
                                <p style="padding-left:88px;">Check Eligibilty and PGP Folder file names and file locations</p>                          
                            </div>
                            <asp:LinkButton ID="lnk_dwnPGP" runat="server" style="background-image:url(../../styles/images/Folder_Download_20.png); background-color:Transparent; background-position:left; padding-left:22px; padding-top:2px; color:#5D478B;margin-top:20px;float:left; margin-bottom:20px; display:block; background-repeat:no-repeat;" OnClick="lnk_dwnPGP_OnClick" >Download PGP Folder</asp:LinkButton>                                       
                            <br class = "br_e" /> 
                            <asp:Label ID="lbl_eligPath" runat="server" Text="Eligibility File:" CssClass="labelHra1" AssociatedControlID="tbx_eligPath" style="width:120px; margin-left:1px"></asp:Label>
                            <asp:TextBox ID="tbx_eligPath" runat="server" CssClass="inputHra1" style="width:450px;"></asp:TextBox>
                            <br class = "br_e" />                        
                            <asp:RequiredFieldValidator
                            id="reqval_codeid" ControlToValidate="tbx_eligPath"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="PGPGroup"
                            Runat="server" />                      
                            <br class = "br_e" /> 
                            <asp:Label ID="lbl_fldrPath" runat="server" Text="PGP Folder Path:" CssClass="labelHra1" AssociatedControlID="tbx_fldrPath" style="width:120px;margin-left:1px"></asp:Label>
                            <asp:TextBox ID="tbx_fldrPath" runat="server" CssClass="inputHra1" style="width:450px;"></asp:TextBox>     
                            <br class = "br_e" />
                            <asp:RequiredFieldValidator
                            id="RequiredFieldValidator2" ControlToValidate="tbx_fldrPath"
                            Text="(Required)" Display="Dynamic"
                            ValidationGroup="PGPGroup"
                            Runat="server" />
                            <br class = "br_e" />                        
                            <asp:LinkButton ID="lnk_pgpElig" runat="server" style="margin-top:10px;margin-left:140px; margin-bottom:35px;" OnClientClick="PGPofFile('ctl00_ContentPlaceHolder1_tbx_eligPath', 'ctl00_ContentPlaceHolder1_tbx_fldrPath');" Font-Underline="false" CssClass="imgbutton" ValidationGroup="PGPGroup" OnClick="lnk_pgpElig_OnClick"><span>PGP Encrypt File</span></asp:LinkButton>
                            <br class = "br_e" />                
                        </fieldset>
                        <br class = "br_e" />
                    </div>
                </asp:View>
                <asp:View id="view2" runat="server">
                   <div id = "importbox" style="margin-left:12px;">
                        <fieldset runat="server" id="fs_FTP">
                            <legend style=" border:none;">FTP Encrypted Eligibility File to Mercer</legend>
                            <div class="userPara" style="width:600px; margin-left:-1px;">
                                <p>Instructions : &nbsp; &nbsp; &nbsp;Select PGP Encrypted Eligibilty File with extension .pgp;</p>
                                 <p style="padding-left:88px;">File will be  automatically re-named to 'fa00760.in.pgp'</p>      
                            </div>
                            <br class = "br_e" />
                            <asp:Label ID="lbl_err" runat="server" style="margin-bottom:15px; color:Red; display:block;"></asp:Label>
                            <br class = "br_e" />
                            <ucl:FileUpload2 ID="FileUpload1" runat="server" Required="true" Vgroup="fileGroup" FileTitle="Encrypted Elig File:" FileTypeRange="pgp" FileUplStyle="width:400px;margin:15px 0px 0px 0px;padding:1px 2px;float:left;" LblStyle="float:left;width:120px;background-color:#F7F7F7;display:block;margin:20px 20px 0px 0px;text-align:left;"/>
                            <br class = "br_e" />
                            <asp:LinkButton ID="lnk_ftpElig" runat="server" style="margin-top:18px;margin-left:140px; margin-bottom:25px;" Font-Underline="false" CssClass="imgbutton" ValidationGroup="fileGroup" OnClick="lnk_ftpElig_OnClick"><span>FTP File</span></asp:LinkButton>                        
                            <br class = "br_e" />                
                        </fieldset> 
                    </div>
                </asp:View>
            </asp:MultiView>
        </div>
	</div>
</asp:Content>


<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Calculate.aspx.cs" Inherits="ImputedIncome_Calculate" Title="Calculate Imputed Income" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contentleft">       
        <div id="Div1">        
            <div id = "module_menu">
                <h1>Imputed Income</h1>            
                <ul id="sitemap">
                    <li><asp:HyperLink ID="hypCalc" runat="server" Font-Underline="true" NavigateUrl="~/ImputedIncome/Calculate.aspx">Calculate</asp:HyperLink></li>
                    <li><asp:HyperLink ID="hypMain" runat="server" NavigateUrl="~/ImputedIncome/Maintainence.aspx">Maintainence</asp:HyperLink></li>
                </ul>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/styles/images/side_tab_down.gif" />                       
            </div> 	
        </div>          
    </div>
    <div id="contentright"> 
        <div id = "importbox" style="margin-left:12px;margin-top:25px;">
            <fieldset runat="server" >
                <legend style=" border:none;color:#5D478B;background:none;font-family: Georgia, Verdana, Arial, sans-serif; font-size:16px;	">Calculate Imputed Income for Pilots</legend>
                <div class="userPara" style="width:620px;margin-left:-1px;color:#5D478B; margin-bottom:10px; margin-top:20px;">
                    <p style="margin-bottom:2px;">This calculation assumes first $50,000 is exempt from imputed income!</p>  
                    <p>This calculation mirrors PRISM imputed income calculation.</p>
                </div> 
                <br class = "br_e" />  
                <asp:Label ID="lbl_err" runat="server" style="background-image:url(../styles/images/error.png);background-color:Transparent; padding-left:20px;background-position:left;background-repeat:no-repeat;color:Red;margin-bottom:15px;display:block;"></asp:Label>     
                <div style="float:left;	width:300px;">                    
                    <br class = "br_e" />              
                    <asp:Label ID="lbl_empno" runat="server" Text="Empno:" CssClass="labelHra1" AssociatedControlID="tbx_empno" style="width:50px; margin-left:1px;background:none;text-align:right;"></asp:Label>
                    <asp:TextBox ID="tbx_empno" runat="server" CssClass="inputHra1" style="width:100px;" OnTextChanged="tbx_empno_TextChanged"></asp:TextBox>
                    <asp:RequiredFieldValidator
                    id="reqval_empno" ControlToValidate="tbx_empno"
                    Text="(Required)" Display="Dynamic"
                    ValidationGroup="CalcGroup"
                    Runat="server" />
                    <asp:RegularExpressionValidator 
                        id="rgx_empno" 
                        runat="server"
                        ControlToValidate="tbx_empno"
                        ValidationGroup = "CalcGroup"
                        ValidationExpression="^\d+$"
                        Display="Dynamic" >
                        Enter valid number
                    </asp:RegularExpressionValidator>                       
                    <br class = "br_e" /> 
                    <asp:Label ID="lbl_dob" runat="server" Text="Dob:" CssClass="labelHra1" AssociatedControlID="tbx_dob" style="width:50px;margin-left:1px;background:none;text-align:right;"></asp:Label>
                    <asp:TextBox ID="tbx_dob" runat="server" CssClass="inputHra1" style="width:100px;"></asp:TextBox>
                    <asp:Label ID="Label4" runat="server" Text="(mm/dd/yyyy)" ></asp:Label>
                    <asp:RegularExpressionValidator ID="rgxDob" ControlToValidate = "tbx_dob" 
                    ValidationExpression="^(0[1-9]|1[012])[/](0[1-9]|[12][0-9]|3[01])[/]\d{4}$" 
                    runat="server" Display="Dynamic" ErrorMessage = "Enter as MM/DD/YYYY" ValidationGroup = "CalcGroup"/>
                    <br class = "br_e" /> 
                    <asp:Label ID="lbl_age" runat="server" Text="Age:" CssClass="labelHra1" AssociatedControlID="tbx_age" style="width:50px;margin-left:1px; background:none;text-align:right;"></asp:Label>
                    <asp:TextBox ID="tbx_age" runat="server" CssClass="inputHra1" style="width:80px;border:none; background-color:#F7F7F7;" ReadOnly="true"></asp:TextBox>     
                    <br class = "br_e" />
                </div>  
                <div style="float:left; width:300px;">
                    <br class = "br_e" />              
                    <asp:Label ID="lbl_300" runat="server" Text="$300,000 Option:" CssClass="labelHra1" AssociatedControlID="tbx_300" style="width:100px; margin-left:1px; background:none;"></asp:Label>
                    <asp:TextBox ID="tbx_300" runat="server" CssClass="inputHra1" ReadOnly="true" style="width:100px;border:none; background-color:#F7F7F7;"></asp:TextBox>
                    <br class = "br_e" />
                    <asp:Label ID="lbl_400" runat="server" Text="$400,000 Option:" CssClass="labelHra1" AssociatedControlID="tbx_400" style="width:100px; margin-left:1px;background:none;"></asp:Label>
                    <asp:TextBox ID="tbx_400" runat="server" CssClass="inputHra1" ReadOnly="true" style="width:100px;border:none; background-color:#F7F7F7;"></asp:TextBox>
                    <br class = "br_e" /> 
                    <asp:Label ID="lbl_500" runat="server" Text="$500,000 Option:" CssClass="labelHra1" AssociatedControlID="tbx_500" style="width:100px; margin-left:1px; background:none;"></asp:Label>
                    <asp:TextBox ID="tbx_500" runat="server" CssClass="inputHra1" ReadOnly="true" style="width:100px;border:none; background-color:#F7F7F7;"></asp:TextBox>
                    <br class = "br_e" /> 
                    <asp:Label ID="lbl_800" runat="server" Text="$800,000 Option:" CssClass="labelHra1" AssociatedControlID="tbx_800" style="width:100px; margin-left:1px; background:none;"></asp:Label>
                    <asp:TextBox ID="tbx_800" runat="server" CssClass="inputHra1" ReadOnly="true" style="width:100px;border:none; background-color:#F7F7F7;"></asp:TextBox>
                    <br class = "br_e" /> 
                </div>  
                <br class = "br_e" />                    
                <asp:LinkButton ID="lnk_submit" CssClass="imgbutton"  runat="server" style="margin-top:18px;margin-left:90px; margin-bottom:35px;" ValidationGroup = "CalcGroup"  OnClick="lnk_Submit_OnClick"><span>Calculate</span></asp:LinkButton>                
                <asp:LinkButton ID="lnk_Reset" CssClass="imgbutton"  runat="server" style="margin-top:18px;margin-left:20px; margin-bottom:35px;" OnClick="lnk_Reset_OnClick"><span>Clear</span></asp:LinkButton>                
                <br class = "br_e" />
                
            </fieldset>
            <br class = "br_e" />
        </div>
    </div>    
</asp:Content>


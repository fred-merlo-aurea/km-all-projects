<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardContent_SMS.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardContent_SMS" %>

<div class="section">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">     
        <tr>
            <td class="headingTwo" valign="top" colspan="2" style="padding-top: 10px;">
                Message:&nbsp;</td>
        </tr>
        
      
        <tr>
            <td class="tableContent" colspan="2" align="center">
                <br/>
                <asp:TextBox ID="ContentText" CssClass="formfield" runat="server" EnableViewState="true"
                    Width="790" TextMode="multiline" Columns="126" Rows="15"></asp:TextBox><br/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
	runat="server" ControlToValidate="ContentText" 
	ErrorMessage="* Max Length: 145 Characters"
 SetFocusOnError="true" ValidationExpression="^[\s\S]{0,145}$">
	</asp:RegularExpressionValidator>      <br/>
                <asp:Label ID="Label1" runat="server" CssClass="formLabel" Text="Max Length: 145 Characters"></asp:Label>
                     </td>
        </tr>
          <tr>
            <td class="headingTwo" valign="top" colspan="2" style="padding-top: 10px;">
               Auto Welcome Message:&nbsp;</td>
        </tr>
          <tr>
            <td class="tableContent" colspan="2" align="center">
                <br/>
                <asp:TextBox ID="WelcomeText" CssClass="formfield" runat="server" EnableViewState="true"
                    Width="790" TextMode="multiline" Columns="126" Rows="15"></asp:TextBox><br/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
	runat="server" ControlToValidate="WelcomeText" 
	ErrorMessage="* Max Length: 145 Characters"
 SetFocusOnError="true" ValidationExpression="^[\s\S]{0,145}$">
	</asp:RegularExpressionValidator>      <br/>
                <asp:Label ID="Label2" runat="server" CssClass="formLabel" Text="Max Length: 145 Characters"></asp:Label>
                     </td>
        </tr>
        
    </table>
</div>
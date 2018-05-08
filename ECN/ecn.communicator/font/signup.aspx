<%@ Page Language="C#" Inherits="ecn.communicator.front.signup" CodeBehind="signup.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="True" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="questionPanel" runat="Server">
        <table width="100%" border='0' cellspacing="0" cellpadding="3">
            <tr>
                <td class="tableContent" colspan="2" align="middle">
                    <asp:ValidationSummary ID="ValidationSummary" runat="Server" EnableClientValidation="True"
                        ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                </td>
            </tr>
            <tr>
                <td class="tableContent" align='right'>
                    Name:
                </td>
                <td class="tableContent" align="left">
                    <asp:TextBox ID="Name" runat="Server" size="30" CssClass="formfield" />
                    <asp:RequiredFieldValidator runat="Server" ID="val_name" ControlToValidate="Name"
                        ErrorMessage="Name is a required field." Display="Static"><--</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tableContent" align='right'>
                    Company:
                </td>
                <td class="tableContent" align="left">
                    <asp:TextBox ID="Company" runat="Server" size="30" CssClass="formfield" />
                </td>
            </tr>
            <tr>
                <td class="tableContent" align='right'>
                    Phone Number:
                </td>
                <td class="tableContent" align="left">
                    <asp:TextBox ID="PhoneNumber" runat="Server" size="12" CssClass="formfield" />
                    <asp:RequiredFieldValidator runat="Server" ID="val_phoneNo" ControlToValidate="PhoneNumber"
                        ErrorMessage="Phone Number is a required field" Display="Static"><--</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tableContent" align='right'>
                    Email Address:
                </td>
                <td class="tableContent" align="left">
                    <asp:TextBox ID="EmailAddress" runat="Server" size="30" CssClass="formfield" />
                    <asp:RequiredFieldValidator runat="Server" ID="val_emailAddress" ControlToValidate="EmailAddress"
                        ErrorMessage="Email Address is a required field" Display="Static"><--</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valValidEmail" runat="Server" ControlToValidate="EmailAddress"
                        ValidationExpression=".*@.*\..*" ErrorMessage="Email address must be valid" Display="Static"><--</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="tableContent" valign="top" align='right'>
                    Service Level Interest:
                </td>
                <td class="tableContent" align="left">
                    <asp:DropDownList EnableViewState="true" ID="ServiceLevel" runat="Server" CssClass="formfield"
                        Visible="true">
                        <asp:ListItem Value="Self">Self Service</asp:ListItem>
                        <asp:ListItem Value="Full" Selected="true">Full Service</asp:ListItem>
                        <asp:ListItem Value="Channel">Channel Partner</asp:ListItem>
                        <asp:ListItem Value="Install">Local Install</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td class="tableContent" valign="top" align='right'>
                    How did you hear about us?
                </td>
                <td class="tableContent" align="left">
                    <asp:DropDownList EnableViewState="true" ID="HowHear" runat="Server" CssClass="formfield"
                        Visible="true">
                        <asp:ListItem Value="Magazine Ads">Magazine Ads</asp:ListItem>
                        <asp:ListItem Value="Received an EMail powered by ECN">Received an EMail powered by ECN</asp:ListItem>
                        <asp:ListItem Value="Newspaper Article">Newspaper Article</asp:ListItem>
                        <asp:ListItem Value="Conference or Convention">Conference or Convention</asp:ListItem>
                        <asp:ListItem Value="Surfing the Web">Surfing the Web</asp:ListItem>
                        <asp:ListItem Value="Press Release">Press Release</asp:ListItem>
                        <asp:ListItem Value="Other" Selected="true">Other</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td class="tableContent" valign="top" align='right'>
                    Question or Comment:
                </td>
                <td class="tableContent" align="left">
                    <asp:TextBox ID="QuestionComment" runat="Server" cols="40" Rows="7" CssClass="formfield"
                        TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="tableContent" colspan="2" align="middle">
                    <asp:Button ID="Submit" runat="Server" Text="Submit" CssClass="formbutton" OnServerClick="CheckSend">
                    </asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="responseLabel" runat="Server" Visible="false">
	Thank you for your response.. You will be contacted by one our representatives soon.
    </asp:Label>
</asp:Content>

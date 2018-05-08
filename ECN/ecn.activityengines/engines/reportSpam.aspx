<%@ Page Language="c#" Inherits="ecn.activityengines.reportSpam" CodeBehind="reportSpam.aspx.cs"
    ValidateRequest="false"  MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellspacing="0" cellpadding="5" border='0'>
		<tr>
			<td>
				<asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                    <table cellspacing="0" cellpadding="0" width="100%" align="center">
                        <tr>
                            <td id="errorTop"></td>
                        </tr>
                        <tr>
                            <td id="errorMiddle">
                                <table height="67" width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img style="padding: 0 0 0 15px;" src="http://images.ecn5.com/images/errorEx.jpg"/>
                                        </td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="errorBottom"></td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:Panel ID="pnlMain" runat="server">
                    <asp:Label ID="lblEmailAddress" runat="server" CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                    <asp:Label ID="lblViolation" runat="server" Text="Please select the Violation:" CssClass="ECN-Label-Heading"></asp:Label><br />
                    <asp:DropDownList ID="ddlViolation" runat="server" CssClass="ECN-DropDown-Medium">
                        <asp:ListItem Selected="True" Value="Spam">Spam</asp:ListItem>
                        <asp:ListItem Value="Harassment">Harassment</asp:ListItem>
                        <asp:ListItem Value="Impersonation">Impersonation</asp:ListItem>
                        <asp:ListItem Value="Obscenity">Obscenity</asp:ListItem>
                        <asp:ListItem Value="Copyright">Copyright</asp:ListItem>
                        <asp:ListItem Value="Fraud">Fraud</asp:ListItem>
                        <asp:ListItem Value="Personally identifiable information">Personally identifiable information</asp:ListItem>
                        <asp:ListItem Value="Company confidential information">Company confidential information</asp:ListItem>
                        <asp:ListItem Value="Threats of physical violence">Threats of physical violence</asp:ListItem>
                        <asp:ListItem Value="Defamation">Defamation</asp:ListItem>
                        <asp:ListItem Value="Illegal content">Illegal content</asp:ListItem>
                        <asp:ListItem Value="Other Violation">Other Violation</asp:ListItem>
                    </asp:DropDownList><br /><br />
                    <asp:Label ID="lblReason" runat="server" Text="Please enter your Feedback [150 characters max]:" CssClass="ECN-Label-Heading"></asp:Label><br />
                    <asp:TextBox ID="txtReason" runat="server" CssClass="ECN-TextBox-Medium" TextMode="MultiLine" Width="100%" Height="100px" MaxLength="150" ></asp:TextBox><br /><br /><br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Changes" 
                        onclick="btnSubmit_Click" CssClass="ECN-Button-Medium" /><br />
                </asp:Panel>
                <asp:Panel ID="pnlThankYou" runat="server">
                    <asp:Label ID="lblConfirmation" runat="server" Text="Your abuse report has been submitted. We take reports of spam abuse very seriously and the Administrator will investigate this matter promptly. Be assured that you will no longer receive any email communications from the sender of the email you received." CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                </asp:Panel>
            </td>
		</tr>
	</table> 
</asp:Content>

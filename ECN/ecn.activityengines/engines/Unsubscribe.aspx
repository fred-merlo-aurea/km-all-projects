<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unsubscribe.aspx.cs" Inherits="ecn.activityengines.engines.Unsubscribe" MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<form id="Form1" method="post" runat="Server">--%>
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
                    <asp:Label ID="lblPageInfo" runat="server"  CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                    <%--<asp:Label ID="Label1" runat="server" CssClass="ECN-Label-Heading" Text="Subscription changes for: "></asp:Label>--%>
                    <asp:Label ID="lblEmailAddress" runat="server" CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                    <%--<div style="border-width: 1px; border-style: solid;">--%>
                        <asp:Label ID="lblMainInfo" runat="server" Text="" CssClass="ECN-Label-Heading"></asp:Label><br />
                        <asp:CheckBox ID="cbGroupUnsubscribe" runat="server" Text="" Checked="true" Enabled="false" CssClass="ECN-Label"/><br />
                        <asp:CheckBox ID="cbCustomerUnsubscribe" runat="server" Text="" CssClass="ECN-Label"/><br /><br />
                    <%--</div>--%>
                    <asp:Label ID="lblReason" runat="server" Text="" CssClass="ECN-Label-Heading"></asp:Label><br />
                    <asp:DropDownList ID="ddlReason" runat="server" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged" AutoPostBack="true" CssClass="ECN-DropDown-Medium">
                        <asp:ListItem Selected="True">Email frequency</asp:ListItem>
                        <asp:ListItem>Email volume</asp:ListItem>
                        <asp:ListItem>Content not relevant</asp:ListItem>
                        <asp:ListItem>Signed up for one-time email</asp:ListItem>
                        <asp:ListItem>Circumstances changed(moved, married, changed jobs, etc.)</asp:ListItem>
                        <asp:ListItem>Prefer to get information another way</asp:ListItem>
                        <asp:ListItem Value="other">Other(Please specify)</asp:ListItem>
                    </asp:DropDownList><asp:TextBox ID="txtReason" runat="server" CssClass="ECN-TextBox-Medium" MaxLength="90" TextMode="SingleLine" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvReason" ValidationGroup="formValidation" ControlToValidate="txtReason"
                        runat="server" CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                    <br /><br /><br />
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Changes" 
                        onclick="btnSubmit_Click" CssClass="ECN-Button-Medium" /><br />
                    <asp:Label ID="lblMoreInfo" runat="server" Text="* If you choose to continue to receive mailings, simply close this window." CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                </asp:Panel>
                <asp:Panel ID="pnlThankYou" runat="server">
                    <asp:Label ID="lblConfirmation" runat="server" Text="You have successfully unsubscribed." CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                    <asp:Label ID="lblThankYou" runat="server" Text="" CssClass="ECN-Label-Heading"></asp:Label><br />
                </asp:Panel>
			</td>
		</tr>
	</table>            
<%--</form>--%>
    </asp:Content>


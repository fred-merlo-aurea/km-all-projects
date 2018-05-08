<%@ Page language="c#" Inherits="ecn.activityengines.emailtoafriend" Codebehind="emailtofriend.aspx.cs" MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../scripts/twemoji.js"></script>
    <script type="text/javascript" src="../scripts/jquery-1.7.1.js"></script>
    <script type="text/javascript" src="../scripts/twemoji-picker.js"></script>
    <link rel="stylesheet" href="../scripts/twemoji-picker.css" />
    <script type="text/javascript">
        if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
        else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
        else { document.addEventListener('load', pageloaded, false); }

        function pageloaded() {
            $('.ECN-Label-Heading, .subject').each(function () {
                var initialString = $(this).html();
                try {
                    if (!initialString.includes('<img')) {
                        initialString = twemoji.parse(eval("\"" + initialString + "\""), { size: "16x16" });
                    }
                }
                catch (err) {

                }

                $(this).html(initialString);
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
	<asp:panel id="pnlMain" runat="Server">
		<TABLE class="tablecontent" cellspacing="0" cellpadding="5" border='0' width="100%">
			<tr>
				<td align='right'><asp:Label id="lblFromLabel" runat="Server" CssClass="ECN-Label-Heading">From</asp:Label></td>
				<td colspan="2">
					<asp:Label id="lblFrom" runat="Server" CssClass="ECN-Label-Heading">Label</asp:Label></td>
			</tr>
			<tr>
				<td align='right' ><asp:Label id="lblSubjectLabel" runat="Server" CssClass="ECN-Label-Heading">Subject</asp:Label></td>
				<td colspan="2">
					<asp:Label id="lblSubject" runat="Server" CssClass="ECN-Label-Heading subject">Label</asp:Label></td>
			</tr>
			<tr>
				<td align='right' valign="top" ><asp:Label id="lblNoteLabel" runat="Server" CssClass="ECN-Label-Heading">Note</asp:Label></td>
				<td align="left" colspan="2">
					<asp:TextBox ID="txtNote" runat="server" CssClass="ECN-TextBox-Medium" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
			</tr>
			<tr>
				<td align='right' ><asp:Label id="lblToLabel" runat="Server" CssClass="ECN-Label-Heading">To</asp:Label></td>
				<td align="left" ><asp:Label id="lblFriendAddressLabel" runat="Server" CssClass="ECN-Label-Heading">Your Friend's Email Address</asp:Label></td>
				<td align="left" ><asp:Label id="lblFriendNameLabel" runat="Server" CssClass="ECN-Label-Heading">Your Friend's Name</asp:Label></td>
			</tr>
			<tr>
				<td align='right'>&nbsp;</td>
				<td>
					<asp:TextBox id="txtEmail1" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
				<td>
					<asp:TextBox id="txtName1" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
			</tr>
			<tr>
				<td align='right'>&nbsp;</td>
				<td>
					<asp:TextBox id="txtEmail2" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
				<td>
					<asp:TextBox id="txtName2" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
			</tr>
			<tr>
				<td align='right'>&nbsp;</td>
				<td>
					<asp:TextBox id="txtEmail3" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
				<td>
					<asp:TextBox id="txtName3" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
			</tr>
			<tr>
				<td align='right'>&nbsp;</td>
				<td>
					<asp:TextBox id="txtEmail4" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
				<td>
					<asp:TextBox id="txtName4" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
			</tr>
			<tr>
				<td align='right'>&nbsp;</td>
				<td>
					<asp:TextBox id="txtEmail5" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
				<td>
					<asp:TextBox id="txtName5" runat="Server" CssClass="ECN-Label-Heading" Width=200></asp:TextBox></td>
			</tr>
			<tr>
				<td align="center" colspan='3'>
					<asp:Button id="btnSubmit" runat="Server" Text="Send" OnClick="btnSubmit_Click" CssClass="ECN-Button-Medium"></asp:Button>
					<%--<asp:TextBox id="txtBlastID" runat="Server" Visible="false"></asp:TextBox>
					<asp:TextBox id="txtEmailID" runat="Server" Visible="false"></asp:TextBox>--%></td>
			</tr>
		</TABLE>
	</asp:panel>
    <asp:Panel ID="pnlThankYou" runat="server">
        <asp:Label ID="lblConfirmation" runat="server" Text="You have successfully forwarded this email." CssClass="ECN-Label-Heading"></asp:Label><br /><br />
    </asp:Panel>
</asp:Content>

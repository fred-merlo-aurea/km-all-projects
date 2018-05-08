<%@ Page language="c#" Inherits="ecn.activityengines.whitelistEmailAddress" Codebehind="whitelistEmailAddress.aspx.cs" MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <TABLE class="tablecontent" cellspacing="0" cellpadding="5" border='0' width="100%">
	    <tr>
		    <td class="tablecontent" width=100%><div style="PADDING-LEFT:15px">
                <asp:Label ID="lblDirections" runat="server" Text="Add Us to Your Email Address Book" CssClass="ECN-Label-Heading"></asp:Label><br /><br />
                <asp:Label ID="lblDirections2" runat="server" Text="To ensure that you continue to receive timely and relevant email communications 
				from us, simply add " CssClass="ECN-Label-Heading"></asp:Label>
				<asp:Label id="lblFromEmail" runat="Server" Font-Italic="True"
					CssClass="ECN-Label-Heading">"From" Email address from your email </asp:Label>
                <asp:Label ID="lblDirections3" runat="server" Text="to your address book. " CssClass="ECN-Label-Heading"></asp:Label>
				<br />                
                <br />
                <asp:Label ID="lblISP" runat="server" Text="Follow the directions for your Internet Service Provider (ISP) or email program:" CssClass="ECN-Label-Heading"></asp:Label>
				<ul>
					<li>
						<A href="http://www.aol.com/">AOL</A></li>
					<li>
						<A href="http://www.att.com/">AT&amp;T</A></li>
					<li>
						<A href="http://www.comcast.com">Comcast</A></li>
					<li>
						<A href="http://webcenters.netscape.compuserve.com">CompuServe</A></li>
					<li>
						<A href="http://www.earthlink.net/">EarthLink</A></li>
					<li>
						<A href="http://www.hotmail.com">Hotmail</A></li>
					<li>
						<A href="http://www.juno.com/">Juno/Netzero</A></li>
					<li>
						<A href="http://outlook.com">Microsoft® Outlook</A></li>
					<li>
						<A href="http://www.msn.com/">MSN</A></li>
					<li>
						<A href="http://mysbcglobalnet.com/">SBC</A></li>
					<li>
						<A href="http://www.yahoo.com/">Yahoo! Mail</A></li>

				</ul>

		    </td>
	    </tr>
    </TABLE>
</asp:Content>

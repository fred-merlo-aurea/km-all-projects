<%@ Page language="c#" Inherits="ecn.communicator.engines.whitelistEmailAddress" Codebehind="whitelistEmailAddress.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>whitelistEmailAddress</title>
		
		
		
		
		<style type="text/css">@import url( /ecn.images/images/stylesheet.css ); 
		</style>
	</head>
	<body>
		<asp:label id="HeaderLbl" runat="Server"></asp:label>
		<form id="Form1" method="post" runat="Server">
			<table width="700" border='0'>
				<tr>
					<td class="tablecontent" width=100%><div style="PADDING-LEFT:15px">
							<h3>Add Us to Your Email Address Book</h3>
							To ensure that you continue to receive timely and relevant email communications 
							from us, simply add
							<asp:Label id="FromEmailAddressLbl" runat="Server" Font-Italic="True" Font-Names="Arial" ForeColor="Red"
								Font-Bold="True">"From" Email adderss from your email</asp:Label>
							&nbsp;to your address book:<br />
							<br />
							<b>Follow the directions for your Internet Service Provider (ISP) or email program: </b>
							<ul>
								<li>
									<A href="aol.html">AOL</A></li>
								<li>
									<A href="att.html">AT&amp;T</A></li>
								<li>
									<A href="comcast.html">Comcast</A></li>
								<li>
									<A href="compuserve.html">CompuServe</A></li>
								<li>
									<A href="earthlink.html">EarthLink</A></li>
								<li>
									<A href="hotmail.html">Hotmail</A></li>
								<li>
									<A href="juno.html">Juno/Netzero</A></li>
								<li>
									<A href="outlook.html">Microsoft® Outlook 2003</A></li>
								<li>
									<A href="msn.html">MSN</A></li>
								<li>
									<A href="sbc.html">SBC</A></li>
								<li>
									<A href="yahoo.html">Yahoo! Mail</A></li></UL>
						</div>
					</td>
				</tr>
			</table>
		</form>
		<asp:Label id="FooterLbl" runat="Server"></asp:Label>
	</body>
</html>

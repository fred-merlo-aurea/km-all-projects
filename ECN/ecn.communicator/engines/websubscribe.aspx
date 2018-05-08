<%@ Page language="c#" Inherits="ecn.communicator.engines.websubscribe" Codebehind="websubscribe.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Subscription Status Change</title>
		
		<style type="text/css">
@import url( /ecn.accounts/assets/channelID_1/images/ecnstyle.css ); @import url( /ecn.accounts/assets/channelID_1/stylesheet.css ); 
		</style>
	</head>
	<body>
		<asp:Label id="lblHeader" runat="Server"></asp:Label>
		<form id="Form1" method="post" runat="Server">
			<P>
				<span class="TableHeader"><b>Subscription Changes for: <i>
							<asp:Label id="DisplayEmailAddress" runat="Server"></asp:Label></i></b></span>
				<asp:Label id="ReturnURL" runat="Server" Visible="False"></asp:Label><asp:Label id="DisplayEmailID" runat="Server" Visible="False"></asp:Label><br />
				<br />
				<font color="#ff0000">
					<asp:Label id="UnsubscribeMessage" runat="Server" Visible="False">Thank you - We have received your unsubscribe request and have removed you from our list. </asp:Label>
					<asp:Label id="SubscribeMessage" runat="Server" Visible="False">Thank you - We have received your subscription request and have added you to our list. </asp:Label>
				</font>
			</P>
			<P>
				<asp:Panel ID="SubscriptionPanel" runat="Server" Visible="True">
					<TABLE border='0'>
						<tr>
							<td class="TableContent" colspan="2"><B>Subscription Status</B></td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:RadioButton id="Subscribed" runat="Server" Visible="false" CssClass="TableContent" Text="Subscribe"
									GroupName="Subscribe" Checked="True"></asp:RadioButton></td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:RadioButton id="Unsubscribed" runat="Server" CssClass="TableContent" Text="Unsubscribe" GroupName="Subscribe"></asp:RadioButton></td>
						</tr>
<!--						
						<tr>
							<td colspan="2" height="10"></td>
						</tr>
						<tr>
							<td class="TableContent">Unsubscribe me from all the Lists and future mailings:</td>
							<td class="TableContent">
								<asp:RadioButtonList id="MasterSuppression" runat="Server" RepeatDirection="Horizontal" class="TableContent">
									<asp:ListItem value="Y">YES</asp:ListItem>
									<asp:ListItem value="N">NO</asp:ListItem>
								</asp:RadioButtonList></td>
						</tr>
-->						
					</TABLE>
					<br />
					<asp:Panel id="DeliveryTypePanel" Visible="False" runat="Server">
						<TABLE>
							<tr>
								<TH class="TableContent">
									<B>Delivery Type</B></TH></tr>
							<tr>
								<td>
									<asp:RadioButton id="HTMLFormat" runat="Server" CssClass="TableContent" Text="HTML" GroupName="FormatType"
										Checked="True"></asp:RadioButton></td>
							</tr>
							<tr>
								<td>
									<asp:RadioButton id="TextFormat" runat="Server" CssClass="TableContent" Text="Text" GroupName="FormatType"></asp:RadioButton></td>
							</tr>
						</TABLE>
					</asp:Panel>
					<P>
						<asp:Button id="Button1" runat="Server" CssClass="formButton" Text="Submit Changes"></asp:Button></P>
				</asp:Panel>
		</form>
		<asp:Label id="Footer" runat="Server"></asp:Label>
	</body>
</html>

<%@ Page Language="C#" Trace="false" Codebehind="ecnProfileManager.aspx.cs" Inherits="ecn.activityengines.engines.ecnProfileManager" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
	Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>ECN Profile Manager</title>
	<link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet.css" type="text/css">

	<script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/overlib/overlib.js"></script>

</head>
<body style="MARGIN-TOP: 0px; BACKGROUND-COLOR: #BCBCBC">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="profileScriptManager" runat="server" />
		<asp:UpdatePanel ID="profileUpdatePanel" runat="server" UpdateMode="Conditional">
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="splashLoader" />
			</Triggers>
			<ContentTemplate>
				<center>
					<div style="PADDING: 0px 0px 0px 0px; MARGIN: 10px 0px; BACKGROUND-COLOR: #FFFFFF;
						WIDTH: 860px">
						<div style="BACKGROUND-IMAGE: url(/ecn.images/images/bg_body_top_866x5.gif); BACKGROUND-REPEAT: no-repeat;">
							&nbsp;
						</div>
						<div>
							<table cellspacing="0" cellpadding="0" border='0' width="800px">
								<tr>
									<td>
										<asp:Label ID="messageLabel" runat="Server" Visible="False" CssClass="errormsg" Font-Bold="True"></asp:Label></td>
								</tr>
								<tr>
									<td class="gradient" align="left" style="WIDTH: 100%">
										<div style="FLOAT: left; FONT-SIZE: 12px; PADDING-LEFT: 5px;">
											<div style="FONT-WEIGHT: bold; PADDING: 6px 0px 10px 0px">PROFILE QUICK INFO:</div>
											<div style="HEIGHT: 40px">
												<div style=" FLOAT:left; PADDING: 0px 0px 4px 0px"><asp:Label ID="headerLabel" runat="Server"></asp:Label></div>
												<div style="FLOAT: right;FONT-SIZE:9px; PADDING:15px 2px 0px 0px; ">
													<asp:UpdateProgress ID="udProgress" runat="server" DisplayAfter="100" Visible="true"
														DynamicLayout="true">
														<ProgressTemplate>
															<sub>
																<img border="0" src="/ecn.images/images/animated-loading-orange.gif" /></sub>&nbsp;loading...
														</ProgressTemplate>
													</asp:UpdateProgress>
												</div>
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td valign="bottom" align="left" style="PADDING-TOP: 10px">
										<table cellspacing="0" cellpadding="0" border='0' width="100%">
											<tr>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnProfileDetails" runat="Server" Text="<span>Profile Info</span>"
														OnClick="btnProfileDetails_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnCampaigns" runat="Server" Text="<span>Email Activity</span>"
														OnClick="btnCampaigns_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnSurveys" runat="Server" Text="<span>Survey Activity</span>"
														OnClick="btnSurveys_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnDEs" runat="Server" Text="<span>Dig. Edition Activity</span>"
														OnClick="btnDEs_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnListSubs" runat="Server" Text="<span>List Subscriptions</span>"
														OnClick="btnListSubs_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnUDF" runat="Server" Text="<span>User Defined Fields</span>"
														OnClick="btnUDF_Click"></asp:LinkButton>
												</td>
												<td class="wizTabs" valign="bottom">
													<asp:LinkButton ID="btnNotes" runat="Server" Text="<span>Subscription Notes</span>"
														OnClick="btnNotes_Click"></asp:LinkButton>
												</td>
												<td>
													<asp:LinkButton ID="splashLoader" Visible="false" runat="server"></asp:LinkButton></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td class="offWhite greySidesB" width="100%" align="left" style="PADDING-LEFT: 7px;
										PADDING-BOTTOM: 7px; PADDING-RIGHT: 7px;">
										<asp:Panel ID="ecnProfileControlPanel" runat="Server">
										</asp:Panel>
									</td>
								</tr>
								<tr>
									<td class="gradient">
										&nbsp;</td>
								</tr>
							</table>
						</div>
						<div style="BACKGROUND-IMAGE: url(/ecn.images/images/bg_body_bottom_866x17.gif);
							BACKGROUND-REPEAT: no-repeat; MARGIN-LEFT: -4px; HEIGHT: 17px">
							&nbsp;</div>
					</div>
				</center>
			</ContentTemplate>
		</asp:UpdatePanel>
	</form>
</body>
</html>

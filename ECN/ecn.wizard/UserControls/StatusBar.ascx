<%@ Control Language="c#" AutoEventWireup="True" Codebehind="StatusBar.ascx.cs" Inherits="ecn.wizard.UserControls.StatusBar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<!--status bar-->
<table width="750" border="0" cellspacing="0" cellpadding="0" ID="Table2">
	<tr>
		<td align="center" style="PADDING-RIGHT:16px; PADDING-LEFT:16px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
			<div style="FONT-SIZE:12px; BACKGROUND:url(<%=UrlBase%>images/bg_status.gif) no-repeat; WIDTH:718px;; HEIGHT:37px">
				<table width="100%" cellpadding="0" cellspacing="0">
					<tr>
						<td width="25%" style="PADDING-LEFT:10px; padding-top:10px;" align='left'>
							<a href="<%=UrlBase%>default.aspx" ><img src="<%=UrlBase%>images/btn_back_to_main_menu.gif" border="0"></a>
						</td>
						<td width="61%" align="left" style="padding-left:100px; padding-top:10px;">
							You are Logged In as: <span style="COLOR:red">
								<asp:Label runat="server" ID="email" />
						</td>
						<td width="14%" style="padding-top:10px;">
							<a href="<%=UrlBase%>Logout.aspx"><img src="<%=UrlBase%>images/btn_logout.gif" border="0"></a>
						</td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
</table>
<!--eof status bar-->

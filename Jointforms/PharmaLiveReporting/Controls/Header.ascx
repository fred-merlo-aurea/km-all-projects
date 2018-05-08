<%@ Control Language="c#" AutoEventWireup="True" Inherits="Header" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" Codebehind="Header.ascx.cs" %>
<table width="100%" cellpadding="0" cellspacing="0" align="center" id="Table4">
	<tr>
		<td bgcolor="#000000" valign="bottom" width="60%" align="left" style="padding-top:10px">
			&nbsp;&nbsp;<img src="http://www.ecn5.com/ecn.accounts/images/ecn_logo2.gif" ></td>
		<td bgcolor="#000000" valign="middle" align="right" width="40%" style="padding-right:25px;font-size:11px;COLOR:#EFD999">
		&nbsp;
		</td>
	</tr>
	<tr>
		<td bgcolor="gray" height="22" style="PADDING-LEFT: 50px" colspan="2">
			<table cellpadding="0" cellspacing="0" border="0" width="100%" ID="Table7">
				<tr>
					<td align="left">
					    <asp:HyperLink ID="HyperLink1" CssClass=menu2 runat=server NavigateUrl="~/main/3aReport.aspx" Text="3A Report"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:HyperLink ID="HyperLink2" CssClass=menu2 runat=server NavigateUrl="~/main/3bReport.aspx" Text="3B Report"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:HyperLink ID="HyperLink3" CssClass=menu2 runat=server NavigateUrl="~/main/EffortKey.aspx" Text="Effort Key Report"></asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					</td>
					<td align='right' style='PADDING-RIGHT:25px'>
						<asp:Label ID="lblUser" Runat="server" CssClass="current_user"></asp:Label>&nbsp;&nbsp;
                        <asp:LoginStatus ID="LoginStatus1" runat="server"  CssClass="current_user" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<br>

<%@ Page language="c#" Codebehind="EventsCalendar.aspx.cs" AutoEventWireup="false" Inherits="ecn.creator.includes.EventsCalendar" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>EventsCalendar</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
  </HEAD>
	<body MS_POSITIONING="GridLayout" bgProperties="fixed">
	<table><tr><td>
			<table cellSpacing="2" cellPadding="2" width="780" align="left" border="0">
				<tr>
					<td width="30%" bgColor="#ffcc66" height="20" class="input" align="center">
					<div align=center 2px?><font size=1 face=verdana><b>Training Events</b></font></div></td>
					<td width="30%" bgColor="#aa9900" height="20" class="input" align="center">
					<div align=center 2px?><font size=1 face=verdana><b>show Events</b></font></div></td>
					<td width="30%" bgColor="#ccffcc" height="20" class="input" align="center">
					<div align=center 2px?><font size=1 face=verdana><b>Other Events</b></font></div></td>
				</tr>
				</table>
			</td></tr>
		<tr><td>		
		<form id="Form1" method="post" runat="server">
			<cpanel:DataPanel id="calendarPanel" runat="server" ExpandImageUrl="./assets/images/expand.gif" CollapseImageUrl="./assets/images/collapse.gif"
				CollapseText="Click to hide Calendar" ExpandText="Click to expand Calendar" TitleText="" Collapsed="False"
				AllowTitleExpandCollapse="True" Width="770px" Height="80%" BorderStyle="Solid" TitleStyle-BackColor="#E0E0E0"
				TitleStyle-BorderColor="#404040" TitleStyle-BorderWidth="1px" TitleStyle-Font-Names="Tunga" HorizontalAlign="Center"
				BorderWidth="1px" BorderColor="#E0E0E0">
<asp:calendar id=eCal runat="server" BorderColor="#FFCC66" BorderWidth="1px" Width="770px" NextPrevFormat="FullMonth" BackColor="#FFFFCC" DayNameFormat="FirstLetter" ForeColor="#663399" Font-Size="9pt" Font-Names="Verdana" ShowGridLines="True">
					<SelectorStyle BackColor="#FFCC66"></SelectorStyle>
					<NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC"></NextPrevStyle>
					<DayHeaderStyle Height="1px" BackColor="#FFCC66"></DayHeaderStyle>
					<SelectedDayStyle Font-Bold="True" BackColor="#CCCCFF"></SelectedDayStyle>
					<TitleStyle Font-Size="9pt" Font-Bold="True" ForeColor="#FFFFCC" BackColor="#990000"></TitleStyle>
					<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
				</asp:calendar>
			</cpanel:DataPanel>
			<p></p>
			<cpanel:DataPanel id="detailsPanel" runat="server" ExpandImageUrl="./assets/images/expand.gif" CollapseImageUrl="./assets/images/collapse.gif"
				CollapseText="Click to hide Calendar" ExpandText="Click to expand Calendar" Collapsed="False"
				TitleText="Event Details" AllowTitleExpandCollapse="True" BorderWidth="1px" Width="770px" Height="100%"
				BorderColor="#E0E0E0" TitleStyle-BackColor="#E0E0E0" TitleStyle-BorderColor="#404040" TitleStyle-BorderWidth="1px"
				HorizontalAlign="Center"><A name=aDetail></A>
<asp:Label id=lblDetails runat="server" Height="100%" Width="100%" BackColor="White" Font-Size="9pt" Font-Names="Verdana"></asp:Label>
			</cpanel:DataPanel>
		</form>
	</td></tr></table>	
	</body>
</HTML>

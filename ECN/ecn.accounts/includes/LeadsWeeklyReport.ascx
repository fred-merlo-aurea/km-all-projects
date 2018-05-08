<%@ Control Language="c#" Inherits="ecn.accounts.includes.LeadsWeeklyReport" Codebehind="LeadsWeeklyReport.ascx.cs" %>
<style>TD.SundayHeader { BORDER-BOTTOM: black 1px solid; TEXT-ALIGN: center;color:black; }
	TD.MonToSatHeader { BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid; TEXT-ALIGN: center;color:black; }
	span.overview {margin-left:5px;}
</style>
<table class="tableContent" id="tblLeadsReport" style="BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BACKGROUND: #f4f4f4; MARGIN-BOTTOM: 10px; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid"
	cellSpacing="0" cellPadding="0" width="900" runat="server">
	<tr>
		<td style="BORDER-BOTTOM: black 1px solid" colSpan="2"><STRONG>From:</STRONG>
			<asp:label id="lblFrom" runat="server" Width="88px"></asp:label><STRONG>To:</STRONG>
			<asp:label id="lblTo" runat="server" Width="104px"></asp:label></td>
	</tr>
	<tr>
		<td style="WIDTH: 700px" align="center">
			<table style="FONT-SIZE: 11px" cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td class="SundayHeader">Sunday</td>
					<td class="MonToSatHeader">Monday</td>
					<td class="MonToSatHeader">Tuesday</td>
					<td class="MonToSatHeader">Wednesday</td>
					<td class="MonToSatHeader">Thursday</td>
					<td class="MonToSatHeader">Friday</td>
					<td class="MonToSatHeader">Saturday</td>
				</tr>
				<tr>
					<td style="color:black;" align="center"><asp:label id="lblSunday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblMonday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblTuesday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblWednesday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblThursday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblFriday" runat="server" Width="48px"></asp:label></td>
					<td style="BORDER-LEFT: black 1px solid;color:black;" align="center"><asp:label id="lblSaturday" runat="server" Width="48px"></asp:label></td>
				</tr>
			</table>
		</td>
		<td style="BORDER-LEFT: black 1px solid" align="left">
			<span class="overview">Invites:
			<asp:label id="lblLeadsCount" runat="server" Width="48px"></asp:label>
			</span><br>
			
			<span  class="overview">
			Demos:
			<asp:label id="lblDemoCount" runat="server" Width="48px"></asp:label>			
			Rate:
			<asp:label id="lblDemoRate" runat="server" Width="48px"></asp:label>
			</span><br>
			<span class="overview">
			Quotes:
			<asp:Label id="lblQuotes" runat="server" Width="48px"></asp:Label>
			Rate:
			<asp:label id="lblQuoteRate" runat="server" Width="48px"></asp:label></span>
		</td>
	</tr>
</table>

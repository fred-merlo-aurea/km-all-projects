<%@ Page language="c#" Codebehind="ReportingLogs.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingLogs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>ReportingLogs</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body MS_POSITIONING="GridLayout" style="MARGIN-TOP: 20px">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px" cellSpacing="1"
				cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td align="center" valign="middle"><asp:Label ID="lblMsg" Runat="server"></asp:Label><BR>
					</td>
				</tr>
				<TR>
					<TD align="center" style="MARGIN-TOP: 20px" vAlign="middle">
						<asp:DataGrid id="dgLogs" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="650px">
<AlternatingItemStyle BackColor="#EDFFC7">
</AlternatingItemStyle>

<ItemStyle Font-Size="10pt">
</ItemStyle>

<HeaderStyle Font-Size="11pt" HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4">
</HeaderStyle>

<Columns>
<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle Font-Size="10pt" HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Success" HeaderText="Success">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="SendTime" HeaderText="Send Time">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
</Columns>

<PagerStyle NextPageText="&lt;span style=&quot;color:white;&quot;&gt;Next&lt;/span&gt;" Font-Size="11pt" PrevPageText="&lt;span style=&quot;color:white;&quot;&gt;Prev&lt;/span&gt;" HorizontalAlign="Right" ForeColor="White" BackColor="#299EE4">
</PagerStyle>
						</asp:DataGrid></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

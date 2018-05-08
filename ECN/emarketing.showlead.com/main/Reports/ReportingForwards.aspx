<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingForwards.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingForwards" %>
<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body>
		<form method="get" action="#" runat="server">
			<div align="center">
				<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
					<table width="752" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
						</tr>
						<tr>
							<td width="265" align="right" valign="top"><img src="../../images/img_reporting_forwards.gif" alt="" width="255" height="186"></td>
							<td width="487" align="left" valign="top"><br><br>
								<!--<p class="title">Email Reporting :
									<asp:Label ID="email" ForeColor="#FF0000" runat="server"></asp:Label></p>-->
								<p class="rightpaddingfortext">
									<b>Forwards</b><br>
									Forwards are 
      emails that were forwarded by a recipient to another recipient by using 
      the Forward to a Friend feature at the bottom of the email.<BR>
									                   
									     <br>
									<b> Displays:</b>
								</p>
								<ul>
									<li><div class="rightpaddingfortext">	Date and time that the email was forwarded</div>
									<li><div class="rightpaddingfortext">The email address of the sender</div>
									<li><div class="rightpaddingfortext">The email address of the recipient</div></li>
								</ul>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<!-- tables go here. make sure it's 650px wide-->
								<asp:DataGrid Width="650px" ID="dgForwards" AutoGenerateColumns="False" Runat="server" AllowPaging="True">
<AlternatingItemStyle BackColor="#EDFFC7">
</AlternatingItemStyle>

<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4">
</HeaderStyle>

<Columns>
<asp:BoundColumn DataField="ForwardTime" HeaderText="Forward Time">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="EmailAddress" HeaderText="Sender Email Address">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="ForwardTo" HeaderText="Forward To">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Referral" HeaderText="Receiver Email Address">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
</Columns>

<PagerStyle NextPageText="&lt;span style=&quot;color:white;&quot;&gt;&amp;nbsp;Next&lt;/span&gt;" PrevPageText="&lt;span style=&quot;color:white&quot;&gt;Prev&amp;nbsp;&lt;/span&gt;" HorizontalAlign="Right" ForeColor="White" BackColor="#299EE4">
</PagerStyle>
								</asp:DataGrid>
								<!-- end table -->
								<div align="center" style="MARGIN:20px 0px">
									<table width="340" border="0" cellspacing="0" cellpadding="0">
										<tr>
											<td><sc:button id="sc1" runat=server></sc:button></td>
											<td><img src="../../images/line_separator.gif" width="8" height="41"></td>
											<td><asp:ImageButton ID="backtoMain" ImageUrl="../../images/btn_backto_main.gif" runat="server"></asp:ImageButton></td>
										</tr>
									</table>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center" height="22" class="footer">
								Need 
      Help? Click here for <A style="COLOR: #ffff00" 
      href="http://www.showlead.com/help.aspx" target=_blank>
								    assistance <a href="http://www.showlead.com/help.aspx" style="COLOR:#ffff00">.</a>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>

<%@ register tagprefix="sc" TagName="button" src="btnShowcare.ascx"%>
<%@ Page Language="c#" ContentType="text/html" ResponseEncoding="iso-8859-1" CodeBehind="ReportingOpens.aspx.cs" AutoEventWireup="false" Inherits="ecn.showcare.wizard.main.Reports.ReportingOpens" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
		<title>Showcare</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<link rel="stylesheet" href="../style.css" type="text/css">
  </HEAD>
	<body>
		<form method="post" runat="server">
			<div align="center">
				<div style="BACKGROUND:url(../../images/bg.gif) repeat-y; WIDTH:752px">
					<table width="752" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td colspan="2"><img src="../../images/header.gif" alt="Show Care" width="752" height="65"></td>
						</tr>
						<tr>
							<td width="265" align="right" valign="top"><img src="../../images/img_reporting_opens.gif" alt="" width="255" height="186"></td>
							<td width="487" valign="top"><br>
								<!--<p class="title">Email Reporting :
									<asp:Label ID="email" ForeColor="#FF0000" runat="server"></asp:Label></p>-->
								<p align="left"  class="rightpaddingfortext"><B>Opens</B><br>
									Lists information 
      about who has opened your email. You may 
									also use the Download feature below to create a file from the Open information.<br><BR><B>
      Displays:</B>
      <UL>
        <LI>
        <DIV class=rightpaddingfortext align=left>Date and 
        time that the email was opened</DIV>
        <LI>
        <DIV class=rightpaddingfortext align=left>The email address of the 
        recipient</DIV>
        <LI>
        <DIV class=rightpaddingfortext align=left>First name of the 
        recipient</DIV>
        <LI>
        <DIV class=rightpaddingfortext align=left>Last name of the 
        recipient</DIV>
        <LI>
        <DIV class=rightpaddingfortext align=left>Phone number of the 
        recipient</DIV></LI></UL></td>
    <P></P>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<!-- tables go here. make sure it's 650px wide-->
								<table width="650" border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td align="left">
											<b>Most Active Opens</b>
										</td>
									</tr>
									<tr>
										<td class="leftpaddingfortables">
											<asp:DataGrid ID="dgActive" AutoGenerateColumns="False" Width="645px" Runat="server">
<AlternatingItemStyle BackColor="#EDFFC7">
</AlternatingItemStyle>

<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4">
</HeaderStyle>

<Columns>
<asp:BoundColumn DataField="ActionCount" HeaderText="Count">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FirstName" HeaderText="First Name">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="LastName" HeaderText="Last Name">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Phone" HeaderText="Phone">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
</Columns>
											</asp:DataGrid>
										</td>
									</tr>
								</table>
								<br>
								<br>
								<table width="650" cellpadding="0" border="0" cellspacing="0">
									<tr>
										<td align="left">
											<b>List of Opens</b>
										</td>
										<td align="right">
											<b>Download Email Addresses as &nbsp;&nbsp;</b>
											<asp:DropDownList EnableViewState="True" ID="ddlDLType" Runat="server">
												<asp:ListItem Selected="True" Value=".csv">CSV File</asp:ListItem>
												<asp:ListItem Value=".txt">Text File</asp:ListItem>
											</asp:DropDownList>
											<asp:ImageButton ID="btnDl" Runat="server" ImageUrl="../../images/btn_download.gif" />
										</td>
									</tr>
									<tr>
										<td colspan="2" class="leftpaddingfortables">
											<asp:DataGrid ID="dgOpens" AutoGenerateColumns="False" Runat="server" Width="645px" AllowPaging="True">
<FooterStyle BackColor="#299EE4">
</FooterStyle>

<AlternatingItemStyle BackColor="#EDFFC7">
</AlternatingItemStyle>

<HeaderStyle HorizontalAlign="Center" ForeColor="White" BackColor="#299EE4">
</HeaderStyle>

<Columns>
<asp:BoundColumn DataField="OpenTime" HeaderText="Open Time">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="FirstName" HeaderText="First Name">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="LastName" HeaderText="Last Name">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Left">
</ItemStyle>
</asp:BoundColumn>
<asp:BoundColumn DataField="Phone" HeaderText="Phone">
<HeaderStyle ForeColor="White">
</HeaderStyle>

<ItemStyle HorizontalAlign="Center">
</ItemStyle>
</asp:BoundColumn>
</Columns>

<PagerStyle VerticalAlign="Middle" NextPageText="&lt;span style=&quot;color:white;&quot;&gt;&amp;nbsp;Next&lt;/span&gt;" PrevPageText="&lt;span style=&quot;color:white&quot;&gt;Prev&amp;nbsp;&lt;/span&gt;" HorizontalAlign="Right" ForeColor="White" BackColor="#299EE4" PageButtonCount="20" CssClass="pagerLink">
</PagerStyle>
											</asp:DataGrid>
										</td>
									</tr>
								</table>
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
								Need Help? Click here for <a href="http://www.showlead.com/help.aspx" target="_blank" style="COLOR:#ffff00"> assistance.</a>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</HTML>

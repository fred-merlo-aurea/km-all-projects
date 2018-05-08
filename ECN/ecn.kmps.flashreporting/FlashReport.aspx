<%@ Page language="c#" Codebehind="FlashReport.aspx.cs" AutoEventWireup="True" Inherits="ecn.kmps.FlashReporting.FlashReport" %>
<%@ register tagprefix="ecn" TagName="footer" src="~/includes/footer.ascx" runat="server"%>
<%@ register tagprefix="ecn" TagName="header" src="~/includes/header.ascx" runat="server"%>
		<ecn:header id="pageheader" divHelpBox="" divHelpBoxTitle="" divContentTitle="" ecnMenu="" runat="server"></ecn:header>
		<form id="Form1" method="post" runat="server">
			<table width="770" align="center" border="0">
				<TR>
					<TD class="tableHeader1" vAlign="top" align="center" width="15%">From Date</TD>
					<TD class="tableHeader1" vAlign="top" align="center" width="15%">To Date</TD>
					<TD class="tableHeader1" vAlign="top" align="center" width="35%">Publication</TD>
					<TD class="tableHeader1" vAlign="top" align="center" width="25%">Promotion Code</TD>
				</TR>
				<tr>
					<td colSpan="4" height="3"></td>
				</tr>
				<TR>
					<TD class="tableHeader" vAlign="top" align="center" width="15%"><asp:textbox class="formfield" id="FromDate" runat="server" Width=70 MaxLength=10></asp:textbox><br>
						<font color="#ff0000" size="1">MM/DD/YYYY</font></TD>
					<TD class="tableHeader" vAlign="top" align="center" width="15%"><asp:textbox class="formfield" id="ToDate" runat="server" Width=70 MaxLength=10></asp:textbox><br>
						<font color="#ff0000" size="1">MM/DD/YYYY</font></TD>
					<TD class="tableHeader" vAlign="top" align="center" width="35%"><asp:dropdownlist class="formfield" id="PubGroupID" runat="server" DataTextField="GroupName" DataValueField="GroupID"></asp:dropdownlist></TD>
					<TD class="tableHeader" vAlign="top" align="center" width="25%"><asp:textbox class="formfield" id="PromoCode" runat="server"></asp:textbox></TD>
				</TR>
				<tr>
					<td colspan="4" height="3"><hr size="1" color="#000000">
					</td>
				</tr>
				<tr>
					<td colspan="4" height="3" align="center">
						<asp:Button id="SubmitBtn" runat="server" Text="Show Results" class="formfield" onclick="SubmitBtn_Click"></asp:Button></td>
				</tr>
				<tr>
					<td colspan="4" height="7"></td>
				</tr>
				<tr>
					<td colspan="4">
						<asp:datagrid id="ResultsGrid" runat="server" HorizontalAlign="Center" AutoGenerateColumns="false"
							width="75%" BackColor="#eeeeee">
							<ItemStyle CssClass="tableContent" height="22"></ItemStyle>
							<HeaderStyle CssClass="tableHeader1" HorizontalAlign=Center></HeaderStyle>
							<FooterStyle CssClass="tableHeader1"></FooterStyle>
							<Columns>
								<asp:BoundColumn ItemStyle-Width="33%" DataField="PromoCode" HeaderText="Promotion Code" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
								<asp:BoundColumn ItemStyle-Width="33%" DataField="UniqueEmails" HeaderText="Unique Emails" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
								<asp:BoundColumn ItemStyle-Width="33%" DataField="TotalEmails" HeaderText="Total Emails" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
							</Columns>
							<AlternatingItemStyle BackColor="White" />
						</asp:datagrid>
					</td>
				</tr>
			</table>
<ecn:footer id="pagefooter" runat="server"></ecn:footer>
		</form>

<%@ Reference Control="~/includes/ContactEditor2.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor" Src="./ContactEditor.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ContactEditor2" Src="ContactEditor2.ascx" %>
<%@ Control Language="c#" Inherits="ecn.accounts.includes.CustomerInfoCollector" Codebehind="CustomerInfoCollector.ascx.cs" %>
<table>
	<TR>
		<TD bgcolor="#f4f4f4" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; BORDER-LEFT:#cccccc 1px solid; PADDING-TOP:25px; BORDER-BOTTOM:#cccccc 1px solid"><div align="center"><span style="FONT-SIZE: 12pt; COLOR: #336699; FONT-FAMILY: Arial"><STRONG>Please 
						fill in your contact information and click Next.</STRONG><FONT style="BACKGROUND-COLOR: #f4f4f4" face="Times New Roman" color="#000000"><BR>
						<BR>
					</FONT></span>
			</div>
		</TD>
	</TR>
	<TR>
		<TD>&nbsp;</TD>
	</TR>
	<tr>
		<td><span style="FONT-SIZE: 14pt; COLOR: #6699cc; FONT-FAMILY: Arial">General Address</span></td>
	</tr>
	<tr>
		<td>
			<table width="100%" bgColor="#f4f4f4" style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; BORDER-LEFT: #cccccc 1px solid; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 78px">
				<tr>
					<td><uc1:ContactEditor2 id="GeneralContact" ShowSameAsTechContact="false" ShowSameAsBillingAddress="false"
							runat="server"></uc1:ContactEditor2></td>
				</tr>
			</table>
		</td>
	</tr>
	<TR>
		<TD></TD>
	</TR>
	<tr>
		<td><span style="FONT-SIZE: 14pt; COLOR: #6699cc; FONT-FAMILY: Arial">Billing Address</span>&nbsp;&nbsp;
			<asp:linkbutton id="lnkCopy" runat="server" CssClass="tableContent" Font-Underline="True" BorderStyle="None"
				CausesValidation="False" Font-Names="Arial" Font-Size="11px" onclick="lnkCopy_Click">Copy From General Address</asp:linkbutton>
		</td>
	</tr>
	<tr>
		<td>
			<table width="100%">
				<tr>
					<td style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; BORDER-LEFT: #cccccc 1px solid; BORDER-BOTTOM: #cccccc 1px solid; HEIGHT: 78px"
						bgColor="#f4f4f4"><uc1:ContactEditor2 id="BillingContact" ShowSameAsTechContact="false" ShowSameAsBillingAddress="false"
							runat="server"></uc1:ContactEditor2></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td colSpan="2"><span style="FONT-SIZE: 14pt; COLOR: #6699cc; FONT-FAMILY: Arial">Technical 
				Information</span></td>
	</tr>
	<tr>
		<td>
			<table style="BORDER-RIGHT: #cccccc 1px solid; BORDER-TOP: #cccccc 1px solid; FONT-SIZE: 11px; BORDER-LEFT: #cccccc 1px solid; BORDER-BOTTOM: #cccccc 1px solid; FONT-FAMILY: arial; HEIGHT: 78px"
				bgColor="#f4f4f4" width="100%">
				<tr>
					<td class="tableHeader" align="right" width="122" style="WIDTH: 122px">Technical 
						Contact</td>
					<td><asp:textbox id="techContact" runat="server" Width="272px" Size="50"></asp:textbox></td>
				</tr>
				<tr>
					<td class="tableHeader" align="right" width="122" style="WIDTH: 122px">Email</td>
					<td><asp:textbox id="techEmail" runat="server" Width="273px" Size="50"></asp:textbox></td>
				</tr>
				<tr>
					<td class="tableHeader" align="right" width="122" style="WIDTH: 122px">Phone</td>
					<td><asp:textbox id="techPhone" runat="server" Width="273px" Size="50"></asp:textbox></td>
				</tr>
			</table>
		</td>
	</tr>
</table>

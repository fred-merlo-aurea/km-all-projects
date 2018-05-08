<%@ Page language="c#" Codebehind="ListEditor.aspx.cs" AutoEventWireup="True" Inherits="ecn.wizard.ListEditor" %>
<center>
	<form name="frm" method="post" id="Form1" class="body_bg" runat="server">
		<table width="718" cellpadding="0" cellspacing="0" border="0" ID="Table4">
			<TBODY>
				<tr>
					<td valign="top" width="154"><img src="images/img_list.gif"></td>
					<td width="564" valign="top">
						<table cellSpacing="0" cellPadding="0" width="100%" style="PADDING-LEFT:10px" border="0">
							<tr>
								<td align="left" valign="middle">List Name:
									<asp:label id="lblListName" ForeColor="red" Font-Bold="True" Runat="server"></asp:label>
									&nbsp;
									<asp:textbox id="txtListName" class="blue_border_box" runat="server" visible="false"></asp:textbox>
								</td>
								<td valign="middle" align="left" width="50%">
									<asp:imagebutton id="btnSave" visible="false" runat="server" ImageUrl="images/btn_save.gif"></asp:imagebutton>
								</td>
							</tr>
							<asp:panel id="pnlError" runat="server" visible="false">
        <TR>
          <TD vAlign=middle align=left width="100%">
<asp:label id=lblError Runat="server" Font-Bold="True" ForeColor="red"></asp:label></TD></TR>
							</asp:panel>
							<tr>
								<td vAlign="top" align="right" colspan="2"><asp:imagebutton id="btnExport" runat="server" ImageUrl="images/btn_export_list.gif"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="btnRename" runat="server" ImageUrl="images/btn_rename_list.gif"></asp:imagebutton>&nbsp;
									<asp:imagebutton id="btnDelete" runat="server" ImageUrl="images/btn_delete_list.gif"></asp:imagebutton></td>
							</tr>
						</table>
						<br>
						<br>
						<div style="PADDING-RIGHT: 0px; PADDING-LEFT: 10px; FONT-SIZE: 10px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px">
							<table width="100%">
								<TBODY>
									<tr>
										<td align="left" width="70%">Click <img src="images/icon-unsubscribe.gif" border="0">
											to unsubscribe an email address.</td>
										<td align="right">Total Email List:</td>
										<td align="right"><span style="COLOR: red"><asp:label id="lblListTotal" runat="server"></asp:label>
											</span></td>
									</tr>
									<tr>
										<td align="left" width="75%">Click <img src="images/icon-delete1.gif" border="0"> to 
											Delete an email address.</td>
										<td align="right"><asp:label id="lblStatus" runat="server"></asp:label>
											Email List:</td>
										<td align="right"><span style="COLOR: red"><asp:label id="lblActiveTotal" runat="server"></asp:label>
											</span></td>
									</tr>
								</TBODY>
							</table>
						</div>
					</td>
				</tr>
				<!--reports tables row-->
				<tr>
					<td colSpan="2" align="right"><font style="FONT-SIZE: 11px">Please select a list to view:</font>&nbsp;
						<asp:dropdownlist id="ddlEmailStatus" runat="server" CssClass="blue_border_box" Autopostback="true" onselectedindexchanged="ddlEmailStatus_SelectedIndexChanged">
							<asp:listitem value="S" selected="true">Active</asp:listitem>
							<asp:listitem value="U">Unsubscribe</asp:listitem>
						</asp:dropdownlist>
					</td>
				</tr>
				<tr>
					<td colSpan="2">
						<div align="center" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; FONT-SIZE:12px; PADDING-BOTTOM:10px; PADDING-TOP:10px">
							<!--reports table-->
							<asp:DataGrid ID="dgList" AutoGenerateColumns="False" Runat="server" PageSize="25" AllowPaging="True"
								DataKeyField="EmailID" width="100%">
								<AlternatingItemStyle Font-Size="12px" BackColor="#EDF0F5"></AlternatingItemStyle>
								<ItemStyle Font-Size="12px"></ItemStyle>
								<HeaderStyle Font-Size="12px" HorizontalAlign="Left" ForeColor="White" BackColor="#A6A6A6"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left" CssClass="gridPadding"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FirstName" HeaderText="First Name">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left" Width="12%"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="LastName" HeaderText="Last Name">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="PhoneNumber" HeaderText="Phone Number">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="Company" HeaderText="Company">
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn Visible="False" DataField="EmailID" HeaderText="EmailID"></asp:BoundColumn>
									<asp:TemplateColumn>
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="4%" />
										<ItemTemplate>
											<asp:ImageButton CommandArgument="Unsubscribe" CommandName="Unsubscribe" ID="cmdUnsubscribe" ImageUrl="images/icon-unsubscribe.gif" AlternateText="Unsubscribe" Runat="server"></asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderStyle ForeColor="White"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center" Width="4%" />
										<ItemTemplate>
											<asp:ImageButton CommandArgument="Delete" CommandName="Delete" ID="cmddelete" ImageUrl="images/icon-delete1.gif" AlternateText="Delete Email &amp; associated Profile information" Runat="server"></asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateColumn>
								</Columns>
								<PagerStyle NextPageText="Next &gt;" Font-Size="12px" Font-Bold="True" PrevPageText="&lt; Previous"
									HorizontalAlign="Center" ForeColor="White" BackColor="#A6A6A6"></PagerStyle>
							</asp:DataGrid>
						</div>
						<br>
						<br>
					</td>
				</tr>
				<!--eof reports table-->
				<div style="PADDING-TOP: 10px" align="center">
				</div>
			</TBODY>
		</table>
		<table cellSpacing="0" cellPadding="0">
			<tr>
				<td valign="top"><asp:ImageButton ImageUrl="images/btn_back_to_previous_page.gif" runat="server" ID="btnPrevious"></asp:ImageButton></td>
				<td valign="top"><asp:imagebutton id="btnSendMail" runat="server" ImageUrl="images/btn_send_email.gif"></asp:imagebutton></td>
			</tr>
		</table>
		<DIV></DIV>
		<br>
		<br>
		<br>
		<br>
		<br>
		<br></TD></TR></TABLE>
	</form>
</center></TR></TBODY>
<DIV></DIV></TR></TBODY></TABLE></FORM>
<CENTER></CENTER>

<%@ Page language="c#" Inherits="ecn.communicator.engines.ManageSubscriptions" Codebehind="ManageSubscriptions.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Manage my subscription</title>
		<style type="text/css">.pghdr {
	FONT-WEIGHT: bold; FONT-SIZE: 20px; FONT-FAMILY: Arial, Helvetica, sans-serif
}
.schdr {
	FONT-WEIGHT: bold; FONT-SIZE: 14px; FONT-FAMILY: Arial, Helvetica, sans-serif; BACKGROUND-COLOR: #eeeeee
}
.label {
	FONT-SIZE: 12px; FONT-FAMILY: Arial, Helvetica, sans-serif
}
.labelsm {
	FONT-SIZE: 10px; FONT-FAMILY: Arial, Helvetica, sans-serif
}
</style>
	</head>
	<body text="#000000" vlink="#ffffff" alink="#ffffff" link="#ffffff" bgcolor="#ffffff"
		leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="frmManager" runat="Server">
			<center><asp:label id="MyHeader" runat="Server" EnableViewState="True"></asp:label>
				<table id="Table1" style="BORDER-RIGHT: #999999 1px solid; BORDER-TOP: #999999 1px solid; FONT-SIZE: 11px; BORDER-LEFT: #999999 1px solid; BORDER-BOTTOM: #999999 1px solid; FONT-FAMILY: Arial, Helvetica, sans-serif"
					cellspacing="0" cellpadding="3" width="750" height="200" border='0'>
					<tr valign="top">
						<td align="center"><asp:label id="lbMessage" runat="Server" Font-Bold="True" ForeColor="red"></asp:label></td>
					</tr>
					<tr>
						<td align="center">
							<asp:panel id="pnlManange" runat="Server" Visible="False">
								<table cellspacing="0" cellpadding="0" width="700" border='0'>
									<tr>
										<td valign="top">
											<h2 class="pghdr">Manage your Preferences</h2>
											<table cellspacing="0" cellpadding="5" width="100%" border='0'>
												<tr>
													<td class="schdr" colspan='3'>Your Profile:</td>
												</tr>
												<tr>
													<td colspan='3' height="5"></td>
												</tr>
												<tr>
													<td width="5%">&nbsp;</td>
													<td class="label" width="20%">Name:</td>
													<td width="75%">
														<asp:Label id="lblName" runat="Server" cssclass="label"></asp:Label></td>
												</tr>
												<tr>
													<td width="5%">&nbsp;</td>
													<td class="label" width="20%">Email Address:</td>
													<td width="75%">
														<asp:Label id="lblEmail" runat="Server" cssclass="label"></asp:Label></td>
												</tr>
												<tr>
													<td colspan='3' height="5"></td>
												</tr>
												<asp:PlaceHolder ID="plList" runat="Server">
													<tr>
														<td class="schdr" colspan='3'><asp:Label id="lbllistHeader" runat="Server"></asp:Label>&nbsp;</td>
													</tr>
													<tr>
														<td class="labelsm" valign="bottom" align='right' colspan='3' height="10">* If you 
															wish to unsubscribe, UnCheck the check box under Subscribed Column.
														</td>
													</tr>
													<tr>
														<td colspan='3'></td>
													</tr>
													<tr>
														<td colspan='3' height="5">
															<asp:DataGrid id="dgSubscriptionGrid" runat="Server" DataKeyField="EmailGroupID" AutoGenerateColumns="False"
																width="100%" CssClass="grid" GridLines="None">
																<HeaderStyle CssClass="gridheader"></HeaderStyle>
																<ItemStyle></ItemStyle>
																<AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
																<Columns>
																	<asp:BoundColumn ItemStyle-Width="30%" DataField="GroupName" HeaderText="NewsLetter" ItemStyle-Font-Bold="True"></asp:BoundColumn>
																	<asp:BoundColumn ItemStyle-Width="40%" DataField="Description" HeaderText="Description"></asp:BoundColumn>
																	<asp:TemplateColumn HeaderText="Subscribed" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
																		<ItemTemplate>
																			<asp:CheckBox id="chksubscription" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsSubscribed"))%>'>
																			</asp:CheckBox>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="HTML" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
																		ItemStyle-Width="10%">
																		<ItemTemplate>
																			<asp:Radiobutton id="rbHTML" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isHTML"))%>'>
																			</asp:Radiobutton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																	<asp:TemplateColumn HeaderText="Text" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
																		ItemStyle-Width="10%">
																		<ItemTemplate>
																			<asp:Radiobutton id="rbText" runat="Server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isText"))%>'>
																			</asp:Radiobutton>
																		</ItemTemplate>
																	</asp:TemplateColumn>
																</Columns>
															</asp:DataGrid><br />
														</td>
													</tr>
												</asp:PlaceHolder>
												<asp:PlaceHolder ID="plEmail" runat="Server">
													<tr>
														<td class="schdr" colspan='3'><asp:Label id="lblemailHeader" runat="Server"></asp:Label>&nbsp;</td>
													</tr>
													<tr>
														<td colspan='3'></td>
													</tr>
													<tr>
														<td colspan='3' height="5">
															<asp:DataList id="dtSubscriptionGrid" runat="Server" RepeatColumns="3" RepeatDirection="Horizontal"
																Width="100%" GridLines="both" cellpadding="5" cellspacing="0" DataKeyField="groupID">
																<ItemStyle Font-Size="11px" Font-Bold="True" VerticalAlign="Top"></ItemStyle>
																<ItemTemplate>
																	<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "GroupName"))%>
																	<br />
																	<asp:CheckBoxList id="cbList" runat="Server" Font-Size="10px"></asp:CheckBoxList>
																</ItemTemplate>
															</asp:DataList>
														</td>
													</tr>
												</asp:PlaceHolder>
												<tr>
													<td align="center" colspan='3'>
														<asp:Button id="btnSubmit" runat="Server" Text="Click here to submit your changes" onclick="btnSubmit_Click"></asp:Button></td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</asp:panel>
						</td>
					</tr>
				</table>
				<asp:label id="MyFooter" runat="Server" EnableViewState="True"></asp:label></center>
		</form>
	</body>
</html>

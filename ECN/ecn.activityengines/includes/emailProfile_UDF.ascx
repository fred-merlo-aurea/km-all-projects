<%@ Control Language="c#" Inherits="ecn.activityengines.includes.emailProfile_UDF"
	Codebehind="emailProfile_UDF.ascx.cs" %>
<br>
<table cellspacing="0" cellpadding="0" width="100%" border="0">
	<tr>
		<td colspan="2">
			<div align="left" style="FONT-WEIGHT: bold; COLOR: red">
				<asp:Label ID="messageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
					Visible="False"></asp:Label></div>
			<table cellspacing="0" cellpadding="0" border='0'>
				<tr>
					<td class="wizTabsSub" valign="bottom">
						<asp:LinkButton ID="simpleUDFLink" runat="Server" Text="<span>Simple</span>" OnClick="simpleUDFLink_Click"></asp:LinkButton>
					</td>
					<td class="wizTabsSub" valign="bottom">
						<asp:LinkButton ID="transactionalUDFLink" runat="Server" Text="<span>Transactional</span>"
							OnClick="transactionalUDFLink_Click"></asp:LinkButton>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
        <td class="greySidesB" width="100%" align="left" style="PADDING: 5px 5px 5px 5px;
            BORDER-TOP: 1px #A4A2A3 solid; BACKGROUND-COLOR: #FFFFFF">
			<asp:Panel ID="simpleUDFPanel" runat="server" Visible="true">
				<asp:DataList ID="simpleUDFDataList" runat="server" Width="100%" OnItemDataBound="simpleUDFDataList_ItemDataBound"
					DataKeyField="GroupID" CellPadding="4" CellSpacing="0">
					<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
					<SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
					<ItemStyle ForeColor="#333333" />
					<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
					<ItemTemplate>
						<table cellpadding="0" cellspacing="0" width="100%" style="BACKGROUND-COLOR: White"
							border="0">
							<tr>
								<td class="gradient" width="100%" style="FONT-WEIGHT: bold; FONT-SIZE: 12px">
									<div style="PADDING-TOP: 8px;">
										&nbsp;&nbsp;<asp:Label ID="listName" runat="Server" Text="">UDF's for <i><%# DataBinder.Eval(Container.DataItem,"GroupName") %></i></asp:Label>
									</div>
									<div style="PADDING: 11px 4px 4px 4px;">
										<asp:Label ID="noSimpleUDFLabel" runat="server" Visible="false" Font-Bold="false"
											Font-Size="11px"><img src="http://images.ecn5.com/images/small-alertIcon.gif" />&nbsp;<sup>No Simple UDF's available at this time</sup></asp:Label>
										<asp:DataGrid ID="simpleUDFGrid" runat="server" AutoGenerateColumns="False"
											Visible="True" CssClass="gridWizard" Width=100%>
											<HeaderStyle CssClass="gridheader"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="ShortName" HeaderText="UDF Name" ReadOnly="true" ItemStyle-Width="25%"
													ItemStyle-Font-Bold="false"></asp:BoundColumn>
												<asp:BoundColumn DataField="LongName" HeaderText="UDF Description" ReadOnly="true"
													ItemStyle-Font-Bold="false" ItemStyle-Width="50%"></asp:BoundColumn>
												<asp:BoundColumn DataField="DataValue" HeaderText="UDF Profile Value" ItemStyle-Width="25%"
													ItemStyle-Font-Bold="false"></asp:BoundColumn>
											</Columns>
											<AlternatingItemStyle CssClass="gridaltrow" />
										</asp:DataGrid>
									</div>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:DataList>
			</asp:Panel>
			<asp:Panel ID="transUDFPanel" runat="server" Visible="false">
				<asp:DataList ID="transUDFDataList" runat="server" CellPadding="4" Width="100%" DataKeyField="GroupID"
					OnItemDataBound="transUDFDataList_ItemDataBound">
					<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
					<SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
					<ItemStyle ForeColor="#333333" />
					<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
					<ItemTemplate>
						<table cellpadding="0" cellspacing="0" width="100%" style="BACKGROUND-COLOR: White">
							<tr>
								<td class="gradient" width="100%" style="FONT-WEIGHT: bold; FONT-SIZE: 12px">
									<div style="PADDING-TOP: 8px;">
										&nbsp;&nbsp;<asp:Label ID="Label1" runat="Server" Text="">UDF's for <i><%# DataBinder.Eval(Container.DataItem,"GroupName") %></i></asp:Label>
									</div>
									<div style="PADDING: 11px 4px 4px 4px">
										<asp:Label ID="noTransUDFLabel" runat="server" Visible="false" Font-Bold="false"
											Font-Size="11px"><img src="http://images.ecn5.com/images/small-alertIcon.gif" />&nbsp;<sup>No Transactional UDF's available at this time</sup></asp:Label>
										<asp:Repeater ID="dataFieldsSetsRepeater" runat="server">
											<ItemTemplate>
												<asp:Label ID="dataFieldSetIDLabel" runat="server" Visible="false"><%# DataBinder.Eval(Container.DataItem, "DFSID")%></asp:Label>
												<asp:Label ID="dataFieldSetNameLabel" runat="server"><%# DataBinder.Eval(Container.DataItem, "DFSName")%></asp:Label><br />
												<div style="PADDING-BOTTOM: 10px; PADDING-TOP: 4px">
													<asp:DataGrid ID="transUDFGrid" runat="server" AutoGenerateColumns="true" Width=100%
														Visible="True" CssClass="gridWizard" ItemStyle-Font-Bold="false">
														<HeaderStyle CssClass="gridheader"></HeaderStyle>
														<AlternatingItemStyle CssClass="gridaltrow" />
													</asp:DataGrid>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</td>
							</tr>
						</table>
					</ItemTemplate>
				</asp:DataList>
			</asp:Panel>
		</td>
	</tr>
</table>

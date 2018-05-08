<%@ Control Language="C#" AutoEventWireup="true" Codebehind="emailProfile_DigitalEdition.ascx.cs"
	Inherits="ecn.activityengines.includes.emailProfile_DigitalEdition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ecnAjax" %>
<br>
<div align="left" style="FONT-WEIGHT: bold; COLOR: red">
	<asp:Label ID="messageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
		Visible="False"></asp:Label></div>
<asp:DataList ID="digEdDataList" runat="server" CellPadding="0" CellSpacing="0" Width="100%"
	OnItemDataBound="digEdDataList_ItemDataBound" DataKeyField="EditionID">
	<FooterStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="White" />
	<SelectedItemStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="#FFFFFF" />
	<ItemStyle ForeColor="#FFFFFF" />
	<HeaderStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="White" />
	<ItemTemplate>
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td align="left" class="formLabel gradient" style="PADDING: 1px 1px 1px 1px; BACKGROUND-COLOR: #FFFFFF">
					<asp:Panel ID="headerPanel" runat="server">
						<div style="PADDING-BOTTOM: 2px">
							<table border="0" width="100%">
								<tr>
									<td align="left" style="FONT-SIZE: 13px; FONT-WEIGHT: bold;">
										EDITION:&nbsp;<%# DataBinder.Eval(Container.DataItem, "EditionName")%></td>
									<td align="right">
										<asp:ImageButton ID="Image1" runat="server" ImageUrl="/ecn.images/images/collapse_minus.gif" /></td>
								</tr>
							</table>
						</div>
						<table cellspacing="2" cellpadding="2" border="0" width="100%" style="BACKGROUND-COLOR: #FFFFFF">
							<tr>
								<td rowspan="3" align="left" valign=middle style="WIDTH:5%; PADDING:3px 3px 3px 3px">
									<img src="<%# DataBinder.Eval(Container.DataItem, "ThumbNailImg")%>" style="BORDER: solid 1px #000000;
										HEIGHT: 75px" onmouseover="return overlib('<div class=baloon><img src=\'<%# DataBinder.Eval(Container.DataItem, "ThumbNailImg450")%>\' style=\'BORDER: solid 1px #000000;HEIGHT:450px\' /> <div>', FULLHTML, VAUTO, RIGHT);"
										onmouseout="return nd();" />
								</td>
								<td align='right' valign=middle class="dataTwo" style="PADDING: 5px 5px; WIDTH: 15%">
									Publication:&nbsp;</td>
								<td class="formLabel" width="50%">
									<%# DataBinder.Eval(Container.DataItem, "PublicationName")%>
								</td>
							</tr>
							<tr>
								<td align='right' valign=middle class="dataTwo" width="15%" style="PADDING: 5px 5px;">
									Average Time Spent:&nbsp;</td>
								<td class="formLabel" width="50%">
									<%# DataBinder.Eval(Container.DataItem, "AvgTimeSpent")%>
									&nbsp;minutes</td>
							</tr>
							<tr>
								<td align='right' valign=middle class="dataTwo" width="15%" style="PADDING: 5px 5px;">
									Pages in Publication:&nbsp;</td>
								<td class="formLabel" width="50%">
									<%# DataBinder.Eval(Container.DataItem, "Pages")%>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="itemPanel" runat="server">
						<table cellspacing="2" cellpadding="2" border="0" width="100%" style="BACKGROUND-COLOR: #FFFFFF">
							<tr>
								<td colspan="3">
									<ecnAjax:TabContainer ID="activityTabsContainer" runat="server" ActiveTabIndex="0"
										AutoPostBack="true" CssClass="newTabsSub">
										<ecnAjax:TabPanel ID="visitActivityTab" runat="server" HeaderText="Visits">
											<ContentTemplate>
												<asp:DataGrid ID="visitActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="PageNumber" HeaderText="Page Number" ReadOnly="true"
															ItemStyle-Width="15%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionCount" HeaderText="Count" ItemStyle-Width="5%"
															ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
										<ecnAjax:TabPanel ID="clickActivityTab" runat="server" HeaderText="Clicks">
											<ContentTemplate>
												<asp:DataGrid ID="clickActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionValue" HeaderText="Action Value" ReadOnly="true"
															ItemStyle-Width="45%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionCount" HeaderText="Count" ItemStyle-Width="5%"
															ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
										<ecnAjax:TabPanel ID="referralsActivityTab" runat="server" HeaderText="Referrals">
											<ContentTemplate>
												<asp:DataGrid ID="referralsActivityTabGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionValue" HeaderText="Action Value" ReadOnly="true"
															ItemStyle-Width="45%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
										<ecnAjax:TabPanel ID="subscriptionsActivityTab" runat="server" HeaderText="Subscriptions">
											<ContentTemplate>
												<asp:DataGrid ID="subscriptionsActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionValue" HeaderText="Action Value" ReadOnly="true"
															ItemStyle-Width="45%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
										<ecnAjax:TabPanel ID="printsActivityTab" runat="server" HeaderText="Prints">
											<ContentTemplate>
												<asp:DataGrid ID="printsActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionValue" HeaderText="Printed Pages" ReadOnly="true"
															ItemStyle-Width="45%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
										<ecnAjax:TabPanel ID="searchesActivityTab" runat="server" HeaderText="Searches">
											<ContentTemplate>
												<asp:DataGrid ID="searchesActivityGrid" runat="server" Visible="True" AutoGenerateColumns="False"
													Width="100%" CssClass="grid">
													<ItemStyle Font-Bold="false"></ItemStyle>
													<HeaderStyle CssClass="gridheader"></HeaderStyle>
													<FooterStyle CssClass="tableHeader1"></FooterStyle>
													<Columns>
														<asp:BoundColumn DataField="EmailSubject" HeaderText="Email Title" ReadOnly="true"
															ItemStyle-Width="30%"></asp:BoundColumn>
														<asp:BoundColumn DataField="SendTime" HeaderText="Email Sent On" ReadOnly="true"
															ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
														<asp:BoundColumn DataField="ActionValue" HeaderText="Action Value" ReadOnly="true"
															ItemStyle-Width="45%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center">
														</asp:BoundColumn>
													</Columns>
													<AlternatingItemStyle CssClass="gridaltrow" />
												</asp:DataGrid>
											</ContentTemplate>
										</ecnAjax:TabPanel>
									</ecnAjax:TabContainer>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<ecnAjax:CollapsiblePanelExtender ID="digEditionCollapsePanelMgr" runat="Server"
						CollapsedSize="0" TargetControlID="itemPanel" ExpandControlID="headerPanel" CollapseControlID="headerPanel"
						Collapsed="false" ImageControlID="Image1" ExpandedText="Hide Survey Details..."
						CollapsedText="Show Survey Details..." ExpandedImage="/ecn.images/images/collapse_minus.gif"
						CollapsedImage="/ecn.images/images/collapse_plus.gif" SuppressPostBack="true" />
				</td>
			</tr>
		</table>
	</ItemTemplate>
</asp:DataList>
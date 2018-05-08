<%@ Control Language="c#" Inherits="ecn.activityengines.includes.emailProfile_Survey"
	Codebehind="emailProfile_Survey.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ecnAjax" %>
<div align="left" style="FONT-WEIGHT: bold; COLOR: red">
	<asp:Label ID="messageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
		Visible="False"></asp:Label></div>
<asp:DataList ID="surveysDataList" runat="server" CellPadding="0" CellSpacing="0"
	Width="100%" OnItemDataBound="surveysDataList_ItemDataBound" DataKeyField="SurveyID">
	<FooterStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="White" />
	<SelectedItemStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="#FFFFFF" />
	<ItemStyle ForeColor="#FFFFFF" />
	<HeaderStyle BackColor="#FFFFFF" Font-Bold="True" ForeColor="White" />
	<ItemTemplate>
		<div  style="PADDING-TOP:7px;">
		<table cellspacing="0" cellpadding="0" width="100%" border="0">
			<tr>
				<td align="left" class="formLabel gradient" style="PADDING: 1px 1px 1px 1px; BACKGROUND-COLOR: #FFFFFF">
					<asp:Panel ID="headerPanel" runat="server">
						<div style="PADDING-BOTTOM: 2px">
							<table border="0" width="100%" style="PADDING-BOTTOM: 3px">
								<tr>
									<td align="left" style="FONT-SIZE: 13px; FONT-WEIGHT: bold;">
										SURVEY NAME:&nbsp;<%# DataBinder.Eval(Container.DataItem, "SurveyTitle")%></td>
									<td align="right">
										<asp:ImageButton ID="Image1" runat="server" ImageUrl="/ecn.images/images/collapse_minus.gif" /></td>
								</tr>
							</table>
						</div>
						<table border="0" cellpadding="0" cellspacing="0" width="100%" style="BACKGROUND-COLOR: #FFFFFF">
							<tr>
								<td style="BACKGROUND-COLOR: White">
									<table cellspacing="0" cellpadding="0" width="100%">
										<tr>
											<td align='right' class="dataTwo" width="22%" style="PADDING: 5px 5px;">
												Survey completed on&nbsp;:&nbsp;</td>
											<td width="78%" class="formLabel">
												<%# DataBinder.Eval(Container.DataItem, "CompletedDate")%>
											</td>
										</tr>
										<tr>
											<td align='right' class="dataTwo" width="22%" style="PADDING: 5px 5px;">
												Survey Email Blast Subject&nbsp;:&nbsp;</td>
											<td width="78%" class="formLabel">
												<%# DataBinder.Eval(Container.DataItem, "EmailSubject")%>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="itemPanel" runat="server">
						<table border="0" cellpadding="0" cellspacing="0" width="100%" style="PADDING: 5px 3px 3px 3px;
							BACKGROUND-COLOR: #FFFFFF">
							<tr>
								<td width="100%">
									<asp:Repeater ID="repQuestions" runat="Server" OnItemDataBound="repQuestions_ItemDataBound">
										<ItemTemplate>
											<table cellpadding="0" cellspacing="0" width="100%" border="0">
												<tr>
													<td align="center" class="bubble" style="PADDING-BOTTOM: 5px; WIDTH: 48px">
														<div>
														</div>
														<strong>&nbsp;Q&nbsp;<%# DataBinder.Eval(Container.DataItem, "number") %></strong>
													<td>
													<td valign="bottom" style="PADDING: 0 0 5px 5px; WIDTH: 100%" class="headingTwo">
														<%# DataBinder.Eval(Container.DataItem, "Question_Text") %>
														<asp:Label ID="lblQuestionID" runat="Server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "Question_ID") %>'></asp:Label>
													</td>
												</tr>
												<tr bgcolor="#ffffff">
													<td style="PADDING: 5px; BORDER: solid 1px #999999" class='formLabel' colspan="3">
														<asp:PlaceHolder ID="plresponse" runat="Server"></asp:PlaceHolder>
														<asp:DataGrid ID="dgGridResponse" runat="Server" AutoGenerateColumns="true" CssClass="gridWizard"
															Width="100%" OnItemDataBound="dgGridResponse_ItemDataBound">
															<HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
															<ItemStyle HorizontalAlign="Center"></ItemStyle>
															<AlternatingItemStyle HorizontalAlign="Center" CssClass="gridaltrowWizard" />
														</asp:DataGrid>
													</td>
												</tr>
											</table>
										</ItemTemplate>
										<SeparatorTemplate>
											<div style="PADDING: 3px">
											</div>
										</SeparatorTemplate>
									</asp:Repeater>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<ecnAjax:CollapsiblePanelExtender ID="surveyCollapsePanelMgr" runat="Server" TargetControlID="itemPanel"
						ExpandControlID="headerPanel" CollapseControlID="headerPanel" Collapsed="true"
						CollapsedSize="0" TextLabelID="Label1" ImageControlID="Image1" ExpandedText="Hide Survey Details..."
						CollapsedText="Show Survey Details..." ExpandedImage="/ecn.images/images/collapse_minus.gif"
						CollapsedImage="/ecn.images/images/collapse_plus.gif" SuppressPostBack="true" />
				</td>
			</tr>
		</table>
		</div>
	</ItemTemplate>
</asp:DataList>

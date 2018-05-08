<%@ Control Language="c#" Inherits="ecn.accounts.includes.QuoteOptionSelector" Codebehind="QuoteOptionSelector.ascx.cs" %>
<table cellSpacing="0" width="100%">
	<tr>
		<td class="SectionHeader" align="center"><asp:label id="lblTitle" CssClass="tableHeader" Width="183px" runat="server"></asp:label></td>
		<td class="SectionHeader" align="right"><asp:linkbutton id="btnAdd" runat="server" CausesValidation="False" Text="Add" Font-Underline="True" onclick="btnAdd_Click">New quote item</asp:linkbutton></td>
	</tr>
	<tr>
		<td colSpan="2"><asp:datagrid id="dgdQuoteItemList" Width="100%" CssClass="grid" Runat="server" AutoGenerateColumns="False" OnItemCreated="dgdQuoteItemList_ItemCreated"
				OnItemCommand="dgdQuoteItemList_ItemCommand" OnItemDataBound="dgdQuoteItemList_OnItemDataBound">
				<AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
				<ItemStyle></ItemStyle>
				<HeaderStyle Font-Bold="True" HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Qty">
						<ItemStyle Width="10px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblQuantity">
								<%# DataBinder.Eval(Container.DataItem, "Quantity") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditQuantity" Runat="server" Width="50px" />
							<asp:RangeValidator ControlToValidate="txtEditQuantity" Type="Integer" MinimumValue="0" MaximumValue="2147483647"
								Runat="server" ID="Rangevalidator2">*</asp:RangeValidator>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Name">
						<ItemStyle Width="300px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblName" Title='<%# DataBinder.Eval(Container.DataItem, "Description") %>' >
								<%# DataBinder.Eval(Container.DataItem, "Name") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:DropDownList ID="ddlEditCode" AutoPostBack="True" Width="280px" Runat="server"></asp:DropDownList>
							<asp:Label runat="server" ID="lblEditDescription" Font-Bold="True" ForeColor="Red">?</asp:Label>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Rate">
						<ItemStyle Width="80px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblRate">
								<%# DataBinder.Eval(Container.DataItem, "Rate", "${0:##,##0.00}") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							$
							<asp:TextBox ID="txtEditRate" Runat="server" Width="50px" />
							<asp:RangeValidator ControlToValidate="txtEditRate" Type="Double" MinimumValue="0" MaximumValue="2147483647"
								Runat="server" ID="Rangevalidator1">*</asp:RangeValidator>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="CCredit">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblCustomerCredit" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBox ID="chkEditCustomerCredit" Runat="server"></asp:CheckBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="One Time">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblOneTime" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:RadioButton ID="rdoEditOneTime" runat="server" GroupName="Frequency" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Monthly">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblMonthly" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:RadioButton ID="rdoEditMonthly" runat="server" GroupName="Frequency" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Quarterly">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblQuarterly" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:RadioButton ID="rdoEditQuarterly" runat="server" GroupName="Frequency" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Annual">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblAnnual" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:RadioButton ID="rdoEditAnnual" runat="server" GroupName="Frequency" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Total">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblTotal">
								<%# DataBinder.Eval(Container.DataItem, "ItemPrice", "${0:###,##0.00}") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Label runat="server" ID="lblEditTotal"/>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Active">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblIsActive" Title='<%# DataBinder.Eval(Container.DataItem, "RecurringProfileID") %>'>
								<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBox ID="chkEditIsActive" Runat="server"></asp:CheckBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle Width="20px"></ItemStyle>
						<ItemTemplate>
							<asp:LinkButton runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" ID="btnDelete"></asp:LinkButton>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle Width="20px"></ItemStyle>
						<ItemTemplate>
							<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="false"></asp:LinkButton>&nbsp;
							<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false"></asp:LinkButton>
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>

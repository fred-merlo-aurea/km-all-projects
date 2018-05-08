<%@ Control Language="c#" Inherits="ecn.accounts.includes.PriceItemEditor" Codebehind="PriceItemEditor.ascx.cs" %>
<table cellSpacing="0" width="100%">
	<tr>
		<td class="SectionHeader" align="center"><asp:label id="lblTitle" runat="server" Width="183px" CssClass="tableHeader"></asp:label></td>
		<td class="SectionHeader" align="right"><asp:linkbutton id="btnAdd" runat="server" Font-Underline="True" Text="Add" CausesValidation="False" onclick="btnAdd_Click">New price item</asp:linkbutton></td>
	</tr>
	<tr>
		<td colSpan="2">
			<asp:datagrid id="dgdQuoteOptionsList" Width="100%" CssClass="grid" OnItemCommand="dgdQuoteOptionsList_ItemCommand"
				OnItemCreated="dgdQuoteOptionsList_ItemCreated" OnItemDataBound="dgdQuoteOptionsList_ItemDataBind"
				AutoGenerateColumns="False" Runat="server">
				<EditItemStyle VerticalAlign="Top"></EditItemStyle>
				<AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
				<ItemStyle></ItemStyle>
				<HeaderStyle Font-Bold="True" HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Code">
						<ItemStyle Width="50px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="Label1">
								<%# DataBinder.Eval(Container.DataItem, "Code") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditCode" Runat="server" Width="50px" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Name">
						<ItemStyle Width="150px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblName" Width="150px" Title='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
								<%# DataBinder.Eval(Container.DataItem, "Name") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditName" Runat="server" Width="150px" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Description">
						<ItemStyle Width="150px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblDescription" Width=150px Title='<%# DataBinder.Eval(Container.DataItem, "Description") %>' >
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditDescription" TextMode="MultiLine" Height="100px" Runat="server" Width="150px" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Rate">
						<ItemStyle Width="80px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblRate">
								<%# DataBinder.Eval(Container.DataItem, "Rate", RateFormatString) %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditRate" Runat="server" Width="50px" />
							<asp:RangeValidator ControlToValidate="txtEditRate" Type="Double" MinimumValue="0" MaximumValue="2147483647"
								Runat="server" ID="Rangevalidator1">*</asp:RangeValidator>
						</EditItemTemplate>
					</asp:TemplateColumn>
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
								Runat="server">*</asp:RangeValidator>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Item Price">
						<ItemStyle Width="10px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblItemPrice">
								<%# DataBinder.Eval(Container.DataItem, "ItemPrice","${0:##,##0.00}") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox ID="txtEditItemPrice" Enabled="False" Runat="server" Width="50px" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" HeaderText="CCredit">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblCustomerCredit">
								<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsCustomerCredit"))?"X":"" %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBox ID="chkEditCustomerCredit" Runat="server"></asp:CheckBox>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Allowed Frequency">
						<ItemStyle HorizontalAlign="Center" Width="150px"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblAllowedFrequency" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBoxList CssClass="tableContent" ID="chkEditAllowedFrequency" Width="150px" Runat="server">
								<asp:ListItem Value="1">Annual</asp:ListItem>
								<asp:ListItem Value="2">Quarter</asp:ListItem>
								<asp:ListItem Value="4">Month</asp:ListItem>
								<asp:ListItem Value="8">One Time</asp:ListItem>
								<asp:ListItem Value="16">Every two weeks</asp:ListItem>
							</asp:CheckBoxList>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Price Type">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblPriceType">
								<%# DataBinder.Eval(Container.DataItem, "PriceType") %>
							</asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:DropDownList ID="ddlEditPriceType" Runat="server">
								<asp:ListItem Value="1">Recurring</asp:ListItem>
								<asp:ListItem Value="2">Usage</asp:ListItem>
								<asp:ListItem Value="3">One Time</asp:ListItem>
							</asp:DropDownList>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Products">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblProducts" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBoxList ID="chkEditProducts" CssClass="tableContent" Runat="server"></asp:CheckBoxList>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Product Features">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblProductFeatures" />
						</ItemTemplate>
						<EditItemTemplate>
							<asp:CheckBoxList ID="chkEditProductFeatures" Width="150" CssClass="tableContent" Runat="server"></asp:CheckBoxList>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False" HeaderText="Service">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" ID="lblServices" />
						</ItemTemplate>
						<EditItemTemplate>
							Client Inquiries:
							<asp:TextBox ID="txtEditClientInquirieCount" Runat="server" Width="50"></asp:TextBox><br />
							Rate for additional inquiries:
							<asp:TextBox ID="txtEditAdditionalRate" Runat="server" Width="50"></asp:TextBox><br />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle Width="20px"></ItemStyle>
						<ItemTemplate>
							<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="true" ID="Linkbutton1"
								NAME="Linkbutton1"></asp:LinkButton>&nbsp;
							<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" ID="Linkbutton2"
								NAME="Linkbutton2"></asp:LinkButton>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn>
						<ItemStyle Width="20px"></ItemStyle>
						<ItemTemplate>
							<asp:LinkButton runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" ID="btnDelete"></asp:LinkButton>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:datagrid></td>
	</tr>
</table>

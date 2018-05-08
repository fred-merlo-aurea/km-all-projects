<%@ Control Language="c#" Inherits="ecn.accounts.includes.EmailQuoteItemEditor" Codebehind="EmailQuoteItemEditor.ascx.cs" %>
<table width="100%" border="0" cellpadding="1" class="tableContent" style="BORDER-BOTTOM:black 1px solid">
	<TBODY>
		<tr>
			<td class="tableHeader1"><!--<span style="COLOR:darkgray">-->
					<asp:Label id="lblTitle" runat="server" Width="152px"></asp:Label><!--</span></td>
			<td width="10"></td>
			<td width="360" align="right"><asp:ImageButton ID="btnListOptions" CausesValidation="False" Height="17" Runat="server"></asp:ImageButton>--></td>
		</tr>
		<tr>
			<td colspan="3">
				<table class="tableContent" cellpadding="0" cellspacing="1" width="100%">
					<tr >
						<td class="tableHeader" vAlign="top" width="100%" bgColor="#eeeeee">
							<asp:DataList Runat="server" id="dltEmailOptions" CssClass="grid" width="100%" GridLines=Vertical>
								<HeaderTemplate>
									<tr class="gridheader">
										<td width="30%">Type</td>
										<td width="10%" align=center>Frequency</td>
										<td width="15%" align=center>Rate</td>
										<td width="10%" align=center>Discount %</td>
										<td width="15%" align=center>Total</td>
										<td width="10"></td>
									</tr>
								</HeaderTemplate>
								<ItemTemplate>
									<tr class="gridaltrow">
										<td width="30%" title='<%# DataBinder.Eval(Container.DataItem, "RecurringProfileID")%>'><%# DataBinder.Eval(Container.DataItem, "Name") %>
										</td>
										<td width="10%" align=center><%# DataBinder.Eval(Container.DataItem, "Frequency")  %></td>
										<td width="15%" align=right><%# DataBinder.Eval(Container.DataItem, "Rate", "${0:##,##0.0000}") %></td>
										<td width="10%" align=center><%# DataBinder.Eval(Container.DataItem, "DiscountRate", "{0:0%}") %></td>
										<td align=right width="15%">
											<asp:Label id="lblItemPrice" runat="server">
												<%# DataBinder.Eval(Container.DataItem, "ActualItemPrice", "${0:##,##0.00}") %>
											</asp:Label></td>
										<td width="10%" align="right">
											<asp:LinkButton ID="btnEdit" runat="server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Quote Item' border=0&gt;" CommandName="Edit" CausesValidation="false"></asp:LinkButton>
											<asp:LinkButton ID="btnDelete" runat="server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Quote Item' border=0&gt;" CommandName="Delete" CausesValidation="false"></asp:LinkButton></td>
									</tr>
								</ItemTemplate>
								<EditItemTemplate>
									<tr class="gridaltrow">
										<td width="30%">
											<asp:DropDownList ID="ddlEditType" Runat="server" Width="180" AutoPostBack="True" OnSelectedIndexChanged="ddlEditOptions_SelectedIndexChanged"  class="formfield"></asp:DropDownList></td>
										<td width="10%" align=center>
											<asp:DropDownList ID="ddlEditFrequency" Runat="server" Width="80" AutoPostBack="True" OnSelectedIndexChanged="ddlEditFrequency_SelectedIndexChanged"  class="formfield"></asp:DropDownList></td>
										<td width="15%" align=right>$
											<asp:TextBox ID="txtEditRate" Runat="server" Width="50px" Enabled="False"  class="formfield"></asp:TextBox>
											<asp:RequiredFieldValidator id="Requiredfieldvalidator2" ControlToValidate="txtEditRate" runat="server" ErrorMessage="Rate is required.">*</asp:RequiredFieldValidator>
											<asp:RangeValidator ControlToValidate="txtEditRate" Type="Double" MinimumValue="0" MaximumValue="2147483647"
												Runat="server" ID="Rangevalidator2">*</asp:RangeValidator>
										</td>
										<td width="10%" align=center>
											<asp:TextBox ID="txtEditDiscountRate" Runat="server" Width="26px" MaxLength="3"  class="formfield"></asp:TextBox>%
											<asp:RequiredFieldValidator id="Requiredfieldvalidator4" ControlToValidate="txtEditDiscountRate" runat="server"
												ErrorMessage="Rate is required.">*</asp:RequiredFieldValidator>
											<asp:RangeValidator ControlToValidate="txtEditDiscountRate" Type="Double" MinimumValue="0" MaximumValue="2147483647"
												Runat="server" ID="Rangevalidator4">*</asp:RangeValidator>
										</td>
										<td align=right width="15%">$
											<asp:Label ID="lblEditTotal" Runat="server"></asp:Label></td>
										<td width="10%" align="right">
											<asp:LinkButton runat="server" Text="&lt;img src=/ecn.images/images/icon-save.gif alt='Save Quote Item' border=0&gt;" CommandName="Update" CausesValidation="false" ID="Linkbutton1"></asp:LinkButton>&nbsp;
											<asp:LinkButton runat="server" Text="&lt;img src=/ecn.images/images/icon-cancel.gif alt='Cancel Edit on Quote Item' border=0&gt;" CommandName="Cancel" CausesValidation="false" ID="Linkbutton2"></asp:LinkButton></td>
									</tr>
								</EditItemTemplate>
								<FooterTemplate>
									<tr><td colspan=6 height=10></td></tr>
									<tr>
										<td width="30%" valign=bottom>
											<asp:DropDownList ID="ddlAddOptions" Runat="server" Width="180" AutoPostBack="True" OnSelectedIndexChanged="ddlAddOptions_SelectedIndexChanged"  class="formfield"></asp:DropDownList></td>
										<td width="10%" align=center>
											<asp:DropDownList ID="ddlAddFrequency" Runat="server" Width="80" AutoPostBack="True" OnSelectedIndexChanged="ddlAddFrequency_OnSelectedIndexChanged" class="formfield"></asp:DropDownList></td>
										<td width="15%" align=right>$
											<asp:TextBox ID="txtAddRate" Runat="server" Width="50px" Enabled="False"  class="formfield"></asp:TextBox>
											<asp:RequiredFieldValidator id="RequiredFieldValidator1" ControlToValidate="txtAddRate" runat="server" ErrorMessage="Rate is required.">*</asp:RequiredFieldValidator>
											<asp:RangeValidator ControlToValidate="txtAddRate" Type="Double" MinimumValue="0" MaximumValue="2147483647"
												Runat="server" ID="Rangevalidator1">*</asp:RangeValidator>
										</td>
										<td width="10%" align=center>
											<asp:TextBox ID="txtAddDiscountRate" Runat="server" Width="26px" MaxLength="3"  class="formfield">0</asp:TextBox>%
											<asp:RequiredFieldValidator id="Requiredfieldvalidator3" ControlToValidate="txtAddDiscountRate" runat="server"
												ErrorMessage="Rate is required.">*</asp:RequiredFieldValidator>
											<asp:RangeValidator ControlToValidate="txtAddDiscountRate" Type="Double" MinimumValue="0" MaximumValue="100"
												Runat="server" ID="Rangevalidator3">*</asp:RangeValidator>
										</td>
										<td align=right width="15%">$
											<asp:Label ID="lblAddTotal" Runat="server"></asp:Label></td>
										<td width="10%" align="right">
											<asp:LinkButton ID="btnAdd" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add New Quote Item' border=0&gt;" OnClick="btnAdd_Click" Runat="server"></asp:LinkButton>
										</td>
									</tr>
								</FooterTemplate>
							</asp:DataList>
						</TD>
					</TR>
				</TABLE>							
				<asp:Label id="lblDebug" Width="664px" runat="server"></asp:Label></td>
		</tr>
	</TBODY>
</table>

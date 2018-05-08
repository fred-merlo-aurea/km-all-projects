<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.billDetail" CodeBehind="billDetail.aspx.cs"
    Title="Invoice"  MasterPageFile="~/MasterPages/Accounts.Master"  %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 		
			<table class="tableContent" cellspacing="0" cellpadding="0" width="800">
				<tr>
					<td width="640"><asp:label id="lblErrorMessage" runat="Server" Visible="False" CssClass="errormsg" Width="679px"></asp:label></td>
				</tr>
				<tr>
					<td width="640">
						<table cellspacing="0" width="640">
							<tr>
								<td valign="top" rowSpan="2">
									<address>Knowledge Marketing, LLC<br />
										14505 21st Avenue N #210<br />
										Plymouth, MN 55447
									</address>
								</td>
								<td align='right'>
									<H2>Invoice</H2>
								</td>
							</tr>
							<tr>
								<td align='right'>
									<table cellspacing="0">
										<tr>
											<td style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; WIDTH: 126px; BORDER-BOTTOM: black 1px solid"
												align="center">Date</td>
											<td style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-BOTTOM: black 1px solid"
												align="center">Invoice #</td>
										</tr>
										<tr>
											<td style="BORDER-RIGHT: black 1px solid; BORDER-LEFT: black 1px solid; WIDTH: 126px; BORDER-BOTTOM: black 1px solid"><asp:label id="lblDate" runat="Server" Width="120px"></asp:label></td>
											<td style="BORDER-RIGHT: black 1px solid; BORDER-BOTTOM: black 1px solid"><asp:label id="lblBillCode" runat="Server" Width="144px"></asp:label></td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align="left" width="640">
						<table cellspacing="0">
							<tr>
								<td style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; WIDTH: 126px; BORDER-BOTTOM: black 1px solid"
									width="640">BILL TO</td>
							</tr>
							<tr>
								<td style="BORDER-RIGHT: black 1px solid; BORDER-LEFT: black 1px solid; WIDTH: 126px; BORDER-BOTTOM: black 1px solid">
									<address><asp:label id="lblCustomerName" Width="264px" runat="Server"></asp:label><asp:label id="lblAddress" Width="288px" runat="Server"></asp:label><asp:label id="lblCityStateZip" Width="256px" runat="Server"></asp:label></address>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td width="640" height="40"></td>
				</tr>
				<tr>
					<td width="640">
						<table cellpadding="0" cellspacing="0">
							<tr>
								<td width="280"></td>
								<td width="80" align="center" style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid">P.O. NO.</td>
								<td width="200" align="center" style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid;BORDER-BOTTOM: black 1px solid">TERMS</td>
								<td width="80" align="center" style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-BOTTOM: black 1px solid">PROJECT</td>
							</tr>
							<tr>
								<td></td>
								<td align="center" style="BORDER-RIGHT: black 1px solid; BORDER-LEFT: black 1px solid;">&nbsp;</td>
								<td align="center" style="BORDER-RIGHT: black 1px solid;">Due Upon Receipt</td>
								<td align="center" style="BORDER-RIGHT: black 1px solid;">&nbsp;</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align='right' width="640"><asp:datagrid id="dgdBillItems" runat="Server" BorderColor="Black" GridLines="Both" AutoGenerateColumns="False">
							<HeaderStyle />
							<ItemStyle />
							<Columns>
								<asp:TemplateColumn HeaderText="QUANTITY" HeaderStyle-Width="60px">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "Quantity") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<Columns>
								<asp:TemplateColumn HeaderText="DESCRIPTION" HeaderStyle-Width="420px">
									<ItemTemplate>
										<%# DataBinder.Eval(Container.DataItem, "Description") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<Columns>
								<asp:TemplateColumn HeaderText="RATE" HeaderStyle-Width="80px">
									<ItemTemplate>
										$<%# DataBinder.Eval(Container.DataItem, "Rate", "{0:##,##0.00}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<Columns>
								<asp:TemplateColumn HeaderText="AMOUNT" HeaderStyle-Width="80px">
									<ItemTemplate>
										$<%# DataBinder.Eval(Container.DataItem, "Total", "{0:##,###,###,###.00}") %>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
				<tr>
					<td width="640">
						<table cellspacing="0">
							<tr>
								<td style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid"
									width="480">Thank you for your business.</td>
								<td style="BORDER-TOP: black 1px solid; FONT-WEIGHT: bold; FONT-SIZE: 16pt; BORDER-BOTTOM: black 1px solid"
									valign="bottom" width="60">Total
								</td>
								<td style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-BOTTOM: black 1px solid"
									align='right' width="100">
									<asp:Label id="lblTotal" runat="Server" Width="87px" /></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
</asp:content>

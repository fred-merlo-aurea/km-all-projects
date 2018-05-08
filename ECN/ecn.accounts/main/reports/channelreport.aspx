<%@ Page Language="c#" Inherits="ecn.accounts.main.reports.channelreport" CodeBehind="channelreport.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<br />
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'
        align="center">
        <tr>
            <td class="tableHeader" align='right'>
                Customer&nbsp;Type&nbsp;:&nbsp;<asp:dropdownlist class="formfield" id="ddlCustomerType" DataValueField="codeValue" DataTextField="codeName"
                    runat="Server" autopostback="True" width="215px" visible="true" onselectedindexchanged="ddlCustomerType_SelectedIndexChanged"></asp:dropdownlist>
            </td>
        </tr>
        <tr>
            <td>
                <table cellspacing="0" cellpadding="1" width="100%" align="center" border='0'>
                    <asp:repeater id="rptCustomerType" runat="Server">
						<itemtemplate>
							<tr>
								<td align="left">&nbsp;<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "CustomerType") %>' runat="Server" ID="lblType" Font-Size="x-small" Font-Bold=True/></td>
							</tr> 
							<tr>
								<td width="100%" style="BORDER: solid 1px #ccc">
									<asp:datalist id="dlChannels" runat="Server" DataKeyField="BaseChannelID"
										width="100%" OnItemDataBound="dlChannels_ItemDataBound" OnItemCommand="dlChannels_ItemCommand">
										<HeaderTemplate>
											<table class="grd_content" cellspacing="0" cellpadding="0" width="100%" align="center"
												border='0'>
												<tr>
													<td class="grd_row_header" align="left" width="40%">Channel Name</td>
													<td class="grd_row_header" align='right' width="10%">Customers</td>
													<td class="grd_row_header" align='right' width="10%">Users</td>
													<td class="grd_row_header" align='right' width="10%">MTD Blast</td>
													<td class="grd_row_header" align='right' width="10%">YTD Blast</td>
													<td class="grd_row_header" align='right' width="10%">MTD Usage</td>
													<td class="grd_row_header" align='right' width="10%">YTD Usage</td>
												</tr>
											</table>
										</HeaderTemplate>
										<ItemTemplate>
											<table class="grd_content" cellspacing="0" cellpadding="0" width="100%" align="center"
												border='0'>
												<tr>
													<td width="3%"><asp:ImageButton CommandName="Select" ImageUrl="/ecn.images/images/click-down.gif" Width="16" Height="17"
															runat="Server" AlternateText="Click to view customers" ID="Imagebutton3" /></td>
													<td width="37%"><%# DataBinder.Eval(Container.DataItem,"BaseChannelName") %>&nbsp;</asp:Label><asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "CustomerType") %>' runat="Server" ID="lblcType" Visible="False"/></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"cCount") %></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"uCount") %></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"mtdcount") %></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"ytdcount") %></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"mtdsent") %></td>
													<td width="10%" align='right'><%# DataBinder.Eval(Container.DataItem,"ytdsent") %></td>
												</tr>
											</table>
										</ItemTemplate>
										<AlternatingItemTemplate>
											<table class="grd_content" cellspacing="0" cellpadding="0" width="100%" align="center"
												border='0'>
												<tr>
													<td width="3%" class="grd_alternate"><asp:ImageButton CommandName="Select" ImageUrl="/ecn.images/images/click-down.gif" Width="16" Height="17"
															runat="Server" AlternateText="Click to view customers" ID="Imagebutton1" /></td>
													<td width="37%" class="grd_alternate"><%# DataBinder.Eval(Container.DataItem,"BaseChannelName") %><asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "CustomerType") %>' runat="Server" ID="lblcType"  Visible="False"/></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"cCount") %></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"uCount") %></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"mtdcount") %></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"ytdcount") %></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"mtdsent") %></td>
													<td width="10%" class="grd_alternate" align='right'><%# DataBinder.Eval(Container.DataItem,"ytdsent") %></td>
												</tr>
											</table>
										</AlternatingItemTemplate>
										<SelectedItemTemplate>
											<table class="grd_content" cellspacing="0" cellpadding="0" width="100%" align="center"
												border="1" bordercolor="a9a9a9">
												<tr style="background-color:#a9a9a9;color:white;">
													<td width="3%"><asp:ImageButton CommandName="UnSelect" ImageUrl="/ecn.images/images/click-up.gif" Width="16" Height="17"
															runat="Server" AlternateText="Click to hide customers" ID="Imagebutton2" /></td>
													<td width="37%"><b><%# DataBinder.Eval(Container.DataItem,"BaseChannelName") %></b>&nbsp;<asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "CustomerType") %>' runat="Server" ID="lblcType" Visible="False"/></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"cCount") %></b></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"uCount") %></b></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"mtdcount") %></b></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"ytdcount") %></b></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"mtdsent") %></b></td>
													<td width="10%" align='right'><b><%# DataBinder.Eval(Container.DataItem,"ytdsent") %></b></td>
												</tr>
												<tr>
													<td colspan="8">
														<asp:DataGrid id="dgCustomers" runat="Server" BorderStyle="none" BorderWidth="0" BackColor="white"
															cellpadding="3" cellspacing="0" DataKeyField="CustomerID" Width="100%" AutoGenerateColumns="False"
															CssClass="grd_content" ShowHeader="False" ShowFooter="False">
															<ItemStyle BackColor="White" Horizontalalign='right'></ItemStyle>
															<AlternatingItemStyle BackColor="#ece9d8" />
															<Columns>
																<asp:BoundColumn DataField="CustomerName" HeaderText="Customer Name" ItemStyle-HorizontalAlign="left"
																	HeaderStyle-HorizontalAlign="left" itemstyle-width="50%"></asp:BoundColumn>
																<asp:BoundColumn DataField="Usercount" HeaderText="Users" itemstyle-width="10%"></asp:BoundColumn>
																<asp:BoundColumn DataField="mtdcount" HeaderText="MTD Blast" itemstyle-width="10%"></asp:BoundColumn>
																<asp:BoundColumn DataField="ytdcount" HeaderText="YTD Blast" itemstyle-width="10%"></asp:BoundColumn>
																<asp:BoundColumn DataField="MTDsent" HeaderText="MTD usage" itemstyle-width="10%"></asp:BoundColumn>
																<asp:BoundColumn DataField="YTDsent" HeaderText="YTD usage" itemstyle-width="10%"></asp:BoundColumn>
															</Columns>
														</asp:DataGrid>
													</td>
												</tr>
											</table>
										</SelectedItemTemplate>
									</asp:datalist>
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
						</itemtemplate>
					</asp:repeater>
                </table>
            </td>
        </tr>
    </table>
</asp:content>

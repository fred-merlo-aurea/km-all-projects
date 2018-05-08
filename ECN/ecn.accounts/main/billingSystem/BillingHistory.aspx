<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.BillingHistory"
    CodeBehind="BillingHistory.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tableContent" width="100%" border="0">
        <!-- Top level Rows -->
        <tbody>
            <tr>
                <td>
                    <table class="tableHeader" width="100%">
                        <tr>
                            <td align='right' style="width: 75px; height: 13px">
                                Channel
                            </td>
                            <td style="width: 236px; height: 13px">
                                <asp:DropDownList ID="ddlChannels" runat="Server" Width="104px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlChannels_SelectedIndexChanged" />
                                <asp:CheckBox ID="chkShowAll" runat="Server" Width="128px" Text="All customers" CssClass="tableContent">
                                </asp:CheckBox>
                            </td>
                            <td width="100" style="height: 13px">
                            </td>
                            <td align='right' style="width: 106px; height: 14px">
                                From:
                            </td>
                            <td align="left">
                                <au:activedatetime id="startDate" runat="Server" format="MONTH;/;DAY;/;YEAR;" timestyle-backcolor="Linen"
                                    datestyle-backcolor="Lavender" basestyle-font-bold="true" basestyle-font-name="Tahoma"></au:activedatetime>
                            </td>
                        </tr>
                        <tr>
                            <td align='right' style="width: 75px">
                                Customer
                            </td>
                            <td style="width: 236px" align="left">
                                <asp:DropDownList ID="ddlCustomers" runat="Server" Width="168px" />
                            </td>
                            <td width="100">
                            </td>
                            <td align='right' style="width: 106px">
                                Thru:
                            </td>
                            <td style="height: 14px" align="left">
                                <au:activedatetime id="endDate" runat="Server" format="MONTH;/;DAY;/;YEAR;" timestyle-backcolor="Linen"
                                    datestyle-backcolor="Lavender" basestyle-font-bold="true" basestyle-font-name="Tahoma"></au:activedatetime>
                            </td>
                        </tr>
                        <tr>
                            <td align='right' style="width: 75px; height: 13px">
                                Bill Type
                            </td>
                            <td style="width: 236px; height: 13px" align="left">
                                <asp:DropDownList ID="ddlBillType" runat="Server" Width="168px">
                                    <asp:ListItem Value="Both">Both Type</asp:ListItem>
                                    <asp:ListItem Value="CreditCard">Credit Card</asp:ListItem>
                                    <asp:ListItem Value="Invoice">Invoice</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td style="width: 106px">
                            </td>
                            <td align="left">
                                <asp:Button ID="btnView" runat="Server" Text="View" OnClick="btnView_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align="center" colspan="10">
                    <hr width="100%" color="#000000" size="1">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataList ID="dltCustomers" runat="Server" Width="100%">
                        <ItemTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="SectionHeader">
                                        Customer Name:
                                        <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataList ID="dltBills" DataSource='<%# GetBillsByCustomerID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ID"))) %>'
                                            runat="Server" Width="100%" OnItemCommand="dltBills_OnItemCommand">
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="subSectionHeader">
                                                            Bill Code: <span style="font-weight: normal" title="ChanneID_customerID_CreateDate_BillID">
                                                                <asp:LinkButton CommandName="ShowBill" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                                    runat="Server"><%# DataBinder.Eval(Container.DataItem, "Code") %></asp:LinkButton>
                                                            </span>
                                                        </td>
                                                        <td class="subSectionHeader">
                                                            CreateDate :<span style="font-weight: normal"><%# DataBinder.Eval(Container.DataItem, "CreatedDate") %></span>
                                                        </td>
                                                        <td class="subSectionHeader">
                                                            Total :<span style="font-weight: normal"><%# DataBinder.Eval(Container.DataItem, "Total", "${0:###,##0.00}") %></span>
                                                        </td>
                                                        <td class="subSectionHeader">
                                                            Status :<span style="font-weight: normal"><%# DataBinder.Eval(Container.DataItem, "Status") %></span>
                                                        </td>
                                                        <td class="subSectionHeader" align="left">
                                                            <span style="font-weight: normal">
                                                                <asp:LinkButton ID="btnViewQuote" runat="Server" CommandName="ViewQuote" ForeColor="#0000ff"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CustomerID") + "," + DataBinder.Eval(Container.DataItem, "QuoteID") %>'>View Quote</asp:LinkButton></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5" align="left">
                                                            <asp:DataGrid ID="dgdBillItems" DataSource='<%# ecn.common.classes.billing.BillItem.GetBillItemsByBillID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ID"))) %>'
                                                                AutoGenerateColumns="False" runat="Server" Width="100%">
                                                                <HeaderStyle CssClass="tableHeader" />
                                                                <ItemStyle CssClass="tableContent" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Item Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillItemCode" runat="Server" Title='<%# "Bill Item ID:" + DataBinder.Eval(Container.DataItem, "ID") %>'>
																						<%# DataBinder.Eval(Container.DataItem, "ItemCode") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Start">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStart" runat="Server">
																						<%# DataBinder.Eval(Container.DataItem, "StartDate") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="End">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnd" runat="Server">
																						<%# ecn.common.classes.DateTimeInterpreter.InterpretEndDate(Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "EndDate"))) %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="Server">
																						<%# DataBinder.Eval(Container.DataItem, "Total", "${0:###,##0.00}") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Changed Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblChangedDate" runat="Server">
																						<%# DataBinder.Eval(Container.DataItem, "ChangedDate") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillType" runat="Server">
																						<%# DataBinder.Eval(Container.DataItem, "BillType") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="Server" Title='<%# "Transaction ID:" + DataBinder.Eval(Container.DataItem, "TransactionID") %>'>
																						<%# DataBinder.Eval(Container.DataItem, "Status") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <!-- Top level Rows -->
            <tr>
            </tr>
            <!-- Top level Rows -->
            <tr>
            </tr>
            <!-- Top level Rows -->
            <tr>
            </tr>
        </tbody>
    </table>
</asp:Content>

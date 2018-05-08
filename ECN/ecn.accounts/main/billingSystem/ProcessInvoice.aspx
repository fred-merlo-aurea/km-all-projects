<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.ProcessInvoice"
    CodeBehind="ProcessInvoice.aspx.cs" Title="Process Invoice" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">   
  <table class="tableContent" width="900" border="0">
            <!-- Top level Rows -->
            <tr>
                <td style="height: 18px" align="left">
                    Base Channel:
                    <asp:DropDownList ID="ddlBaseChannels" runat="Server" Width="144px">
                    </asp:DropDownList><asp:Button ID="btnShowInvoice" runat="Server" Width="96px" Text="Show Invoice"
                        Height="22px" OnClick="btnShowInvoice_Click"></asp:Button></td>
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
                                        <asp:DataList ID="dltBills" DataSource='<%# GetUnPaidBillsByCustomerID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ID"))) %>'
                                            runat="Server" Width="100%" OnItemCreated="dltBills_OnItemCreated" OnItemCommand="dltBills_OnItemCommand">
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="subSectionHeader">
                                                            Bill ID:
                                                            <asp:LinkButton CommandName="ShowBill" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                                runat="Server" ID="Linkbutton1">
																	<%# DataBinder.Eval(Container.DataItem, "Code") %>
                                                            </asp:LinkButton></td>
                                                        <td class="subSectionHeader">
                                                            Status :<%# DataBinder.Eval(Container.DataItem, "Status") %></td>
                                                        <td class="subSectionHeader">
                                                            <span style="font-weight: normal">
                                                                <asp:LinkButton ID="btnViewQuote" runat="Server" CommandName="ViewQuote" ForeColor="#0000ff"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CustomerID") + "," + DataBinder.Eval(Container.DataItem, "QuoteID") %>'>View Quote</asp:LinkButton></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan='3' align="left">
                                                            <asp:DataGrid ID="dgdBillItems" DataSource='<%# GetBillItemsByBillID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ID"))) %>'
                                                                AutoGenerateColumns="False" runat="Server" Width="100%" OnItemCommand="dgdBillItems_OnItemCommand"
                                                                OnItemCreated="dgdBillItems_OnItemCreated">
                                                                <HeaderStyle CssClass="tableHeader" />
                                                                <ItemStyle CssClass="tableContent" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillItemID" runat="Server">
																					<%# DataBinder.Eval(Container.DataItem, "ID") %>
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
                                                                            <asp:Label ID="lblStatus" runat="Server">
																					<%# DataBinder.Eval(Container.DataItem, "Status") %>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlEditStatus" runat="Server">
                                                                                <asp:ListItem Value="0">Pending</asp:ListItem>
                                                                                <asp:ListItem Value="1">Charged To Credit Card</asp:ListItem>
                                                                                <asp:ListItem Value="2">Invoice Paid</asp:ListItem>
                                                                                <asp:ListItem Value="3">Canceled</asp:ListItem>
                                                                                <asp:ListItem Value="4">Customer Credit</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:TemplateColumn ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton runat="Server" Text="Edit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                                                CausesValidation="false" ID="btnEditBillItem">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:LinkButton runat="Server" Text="Update" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                                                CausesValidation="false" ID="btnUpdateBillItem">
                                                                            </asp:LinkButton>&nbsp;
                                                                            <asp:LinkButton runat="Server" Text="Cancel" CommandName="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                                                                CausesValidation="false" ID="btnCancelBillItem">
                                                                            </asp:LinkButton>
                                                                        </EditItemTemplate>
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
                    </asp:DataList></td>
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
        </table>        
</asp:content>


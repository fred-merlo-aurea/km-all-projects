<%@ Page Language="c#" Inherits="ecn.accounts.main.billingSystem.Default" CodeBehind="Default.aspx.cs"
    Title="Quote List" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        $("#<%= txtStart.ClientID %>").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onSelect: function (selectedDate) {
                $("#<%= txtEnd.ClientID %>").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#<%= txtEnd.ClientID %>").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 3,
            onSelect: function (selectedDate) {
                $("#<%= txtStart.ClientID %>").datepicker("option", "maxDate", selectedDate);
            }
        });
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tableContent" width="100%" cellpadding="3" style="border-right: gray 1px solid;
        border-top: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid;
        padding-left: 20px; padding-right: 20px; padding-top: 20px; padding-bottom: 20px;">
        <tr bgcolor="#f4f4f4">
            <td colspan="2" align="left">
                Created By:
                <asp:DropDownList ID="ddlCreatedBy" runat="Server" Width="272px" AutoPostBack="True"
                    CssClass="formfield" OnSelectedIndexChanged="ddlCreatedBy_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr bgcolor="#f4f4f4">
            <td colspan="2" align="left">
                Account Manager:&nbsp;&nbsp;
                <asp:DropDownList ID="ddlAccountManager" runat="Server" Width="272px" AutoPostBack="True"
                    CssClass="formfield" OnSelectedIndexChanged="ddlAccountManager_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr bgcolor="#f4f4f4">
            <td align="left">
                Filter By:
                <asp:DropDownList ID="ddlCustomerType" runat="Server" class="formfield">
                    <asp:ListItem Value="2"  Selected="True">All Customers</asp:ListItem>
                    <asp:ListItem Value="0">New Customers</asp:ListItem>
                    <asp:ListItem Value="1">Existing Customers</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlQuoteStatus" runat="Server" Width="136px" class="formfield">
                    <asp:ListItem Value="3">ALL</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">Pending</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Denied</asp:ListItem>
                </asp:DropDownList>
                From:<asp:TextBox ID="txtStart" runat="Server" class="formfield"></asp:TextBox>
                &nbsp;To:
                <asp:TextBox ID="txtEnd" runat="Server" class="formfield"></asp:TextBox>&nbsp;&nbsp;
                <asp:Button ID="btnRefine" runat="Server" Text="Search" class="formfield" OnClick="btnSearch_Click">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td class="tableHeader1" colspan="2">
                <asp:Label ID="lblQuote" runat="Server">Quotes for new customers</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="dgQuotes" GridLines="Both" runat="Server" AutoGenerateColumns="False"
                    Width="100%" CssClass="grid" OnSortCommand="dgQuotes_SortCommand" OnItemCommand="dgQuotes_ItemCommand" OnItemCreated="dgQuotes_OnItemCreated"
                    AllowSorting="True">
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="center" />
                    <Columns>
                        <asp:BoundColumn DataField="QuoteCode" HeaderText="QuoteID" ItemStyle-Width="20%">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="custname" SortExpression="custname" HeaderText="Customer"
                            ItemStyle-Width="15%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FirstName" HeaderText="Contact" ItemStyle-Width="10%">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Phone" HeaderText="Phone" ItemStyle-Width="10%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Email" SortExpression="Email" HeaderText="Email" ItemStyle-Width="10%">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CreatedDate" SortExpression="CreatedDate" HeaderText="Created Date"
                            ItemStyle-Width="10%"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Approve Date" SortExpression="ApproveDate" ItemStyle-Width="10%"
                            ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:Label runat="Server" Text='<%# ecn.common.classes.DateTimeInterpreter.InterpretActionDate(Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.ApproveDate"))) %>'
                                    ID="Label1" NAME="Label5">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-Width="8%">
                        </asp:BoundColumn>
                        <asp:TemplateColumn ItemStyle-Width="3%" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit / View Quote Details' border='0'&gt;"
                                    CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.QuoteID")%>'
                                    CausesValidation="false" ID="Linkbutton5" NAME="Linkbutton1">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-Width="3%" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Edit Group / View Emails' border='0'&gt;"
                                    CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.QuoteID")%>'
                                    CausesValidation="false" ID="btnDelete" NAME="btnDelete">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <AU:PagerBuilder ID="QuotesPager" runat="Server" Width="100%" HorizontalAlign="Center"
                    PageSize="25" ControlToPage="dgQuotes" OnIndexChanged="QuotesPager_IndexChanged">
                    <PagerStyle CssClass="gridpager"></PagerStyle>
                </AU:PagerBuilder>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>

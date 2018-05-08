<%@ Page Language="c#" Inherits="ecn.accounts.customersmanager.customers_main" MasterPageFile="~/MasterPages/Accounts.Master"
    CodeBehind="default.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function deleteCustomer(theID) {
            if (confirm('Are you Sure?\nSelected Customer will be permanently deleted.')) {
                window.location = "default.aspx?CustomerID=" + theID;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'
        style="padding-left: 20px; padding-right: 20px; padding-top: 20px; padding-bottom: 20px;">
        <tr>
            <td class="tableHeader" align="left">
                <asp:Panel ID="pnlBaseChannel" runat="Server" Visible="true">
                    <table width="100%" border='0'>
                        <tr>
                            <td class="tableHeader">
                                Base Channels
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList class="formfield" ID="ddlBaseChannels" Width="215px" runat="Server"
                                    Visible="true" OnSelectedIndexChanged="ddlBaseChannels_OnSelectedIndexChanged"
                                    AutoPostBack="true" DataTextField="BaseChannelName" DataValueField="BaseChannelID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:GridView ID="grdCustomers" AllowPaging="false" CssClass="grid" Width="100%"
                    AutoGenerateColumns="false" DataKeyNames="CustomerID, UsersCount" runat="server"
                    GridLines="None" OnRowDataBound="grdCustomers_RowDataBound" OnRowCommand="grdCustomers_RowCommand">
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right"></PagerStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="CustomerName" HeaderText="Name" HeaderStyle-Width="30%"
                            ItemStyle-Width="30%" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created" HeaderStyle-Width="12%"
                            ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="12%">
                        </asp:BoundField>
                        <asp:BoundField DataField="ActiveFlag" HeaderText="Is Active" HeaderStyle-Width="5%"
                            ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                        </asp:BoundField>
                        <asp:BoundField DataField="DemoFlag" HeaderText="Is Demo" HeaderStyle-Width="5%"
                            ItemStyle-Width="5%" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Login" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                            ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="Login" CausesValidation="false" ID="LoginBtn"
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Customer Details' border='0'&gt;"
                            DataNavigateUrlFields="CustomerID" DataNavigateUrlFormatString="customerdetail.aspx?CustomerID={0}"
                            HeaderText="Edit" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-Width="5%" ItemStyle-Width="5%"></asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Customer' border='0'&gt;"
                                    CausesValidation="false" ID="lbtnDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>' OnClientClick="alert('Contact Tech team to remove Customers');return false;"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contacts" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                            ItemStyle-Width="5%">
                            <ItemTemplate>
                                <a href="javascript:void window.open('Contacts.aspx?CustomerID=<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>',null,'height=400,width=600,status=yes,toolbar=no');"
                                    style="border: 0">
                                    <img src="/ecn.images/images/folderGroup.gif" alt='Add Contacts' border='0' /></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notes" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"
                            ItemStyle-Width="5%">
                            <ItemTemplate>
                                <a href="javascript:void window.open('Notes.aspx?CustomerID=<%# DataBinder.Eval(Container, "DataItem.CustomerID") %>',null,'height=300,width=500,status=yes,toolbar=no');"
                                    style="border: 0">
                                    <img src="/ecn.images/images/folderDesc.gif" alt='Add Notes' border='0' /></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

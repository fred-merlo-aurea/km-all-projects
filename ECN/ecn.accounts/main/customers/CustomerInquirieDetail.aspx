<%@ Reference Page="~/main/customers/license.aspx" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.customers.CustomerInquirieDetail" MasterPageFile="~/MasterPages/Accounts.Master"
    CodeBehind="CustomerInquirieDetail.aspx.cs" Title="CustomerInquirie" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<table class="tableContent" cellspacing="0" cellpadding="0" width="100%" border='0' align="left">
            <tr>
                <td colspan="2">
                    <table align="left" cellpadding="2" cellspacing="1" style="padding: 5px 5px 5px 5px;">
                        <tr>
                            <td class="tableHeader" align='right'>
                                Base Channel</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlChannels" runat="Server" class="formfield" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlChannels_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tableHeader" align='right'>
                                Customer</td>
                            <td align="left">
                                <asp:DropDownList ID="ddlCustomers" runat="Server" class="formfield" AutoPostBack="True"
                                    Width="160px" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblErrorMessage" runat="Server" CssClass="errormsg"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="sectionHeader">
                    Customer Inquiry List
                </td>
                <td class="sectionHeader" align='right'>
                    <asp:LinkButton ID="btnAdd" runat="Server" Text="Add Customer Inquirie" OnClick="btnAdd_Click">Add Customer Inquiry</asp:LinkButton></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DataList ID="dltCustomerInquiries" GridLines="Both" runat="Server">
                        <ItemStyle CssClass="tableContent" />
                        <AlternatingItemStyle CssClass="tableContentAlt1" />
                        <HeaderTemplate>
                            <table class="tableHeader" cellspacing="0" cellpadding="1" width="100%">
                                <tr>
                                    <td width="50" align="center">
                                        LID</td>
                                    <td width="85" align="center">
                                        FName</td>
                                    <td width="80" align="center">
                                        LName</td>
                                    <td width="100" align="center">
                                        NBD</td>
                                    <td width="100" align="center">
                                        Date</td>
                                    <td width="400" align="center">
                                        Notes</td>
                                    <td width="50">
                                    </td>
                                    <td width="50">
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table class="tableContent" cellspacing="0" cellpadding="1" width="100%">
                                <tr>
                                    <td width="50" align="center">
                                        <asp:Label ID="lblLicense" runat="Server" Title='<%# DataBinder.Eval(Container.DataItem, "InquirieLicense")%>'>
												<%# DataBinder.Eval(Container.DataItem, "LicenseID")%>
                                        </asp:Label></td>
                                    <td width="85" align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "FirstName")%>
                                    </td>
                                    <td width="80" align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "LastName")%>
                                    </td>
                                    <td width="100" align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "CustomerServiceName")%>
                                    </td>
                                    <td width="100" align="center">
                                        <%# DataBinder.Eval(Container.DataItem, "DateOfInquirie")%>
                                    </td>
                                    <td width="400">
                                        <%# DataBinder.Eval(Container.DataItem, "Notes")%>
                                    </td>
                                    <td width="50">
                                        <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                            runat="Server">Delete</asp:LinkButton></td>
                                    <td width="50">
                                        <asp:LinkButton ID="Linkbutton1" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                            runat="Server">Edit</asp:LinkButton></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <table class="tableContent" cellspacing="0" cellpadding="1" width="60%">
                                <tr>
                                    <td style="border-top: 1px double red" class="TableHeader" colspan="2">
                                        Edit/Add Customer Inquirie</td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align='right' width="100">
                                        License ID:</td>
                                    <td class="TableHeader">
                                        <asp:Label ID="Label1" runat="Server" Title='<%# DataBinder.Eval(Container.DataItem, "InquirieLicense")%>'>
												<%# DataBinder.Eval(Container.DataItem, "LicenseID")%>
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align='right' width="100">
                                        Date:</td>
                                    <td class="TableHeader">
                                        <asp:Label ID="Label2" Width="300" runat="Server">
												<%# DataBinder.Eval(Container.DataItem, "DateOfInquirie")%>
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align='right' width="100">
                                        First Name:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstName" runat="Server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align='right' width="100">
                                        Last Name:</td>
                                    <td>
                                        <asp:TextBox ID="txtLastName" runat="Server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align='right' width="120">
                                        Cust. SVC Rep.:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCustomerServiceRep" runat="Server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="TableHeader" align="left">
                                        Notes</td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom: 1px double red">
                                        <asp:TextBox ID="txtNotes" runat="Server" TextMode="MultiLine" Height="80" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="50">
                                        <asp:LinkButton ID="btnUpdate" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                            runat="Server">Update</asp:LinkButton></td>
                                    <td width="50">
                                        <asp:LinkButton ID="btnCancel" CommandName="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")%>'
                                            runat="Server">Cancel</asp:LinkButton></td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:DataList> <br /></td>
            </tr>
        </table>
</asp:content>

<%@ Page Language="c#" Inherits="ecn.publisher.main.Publication._default" CodeBehind="default.aspx.cs"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblErrorMessage" runat="Server" Visible="False" ForeColor="Red" Font-Size="x-small"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdPublication" AllowPaging="true" CssClass="grid" Width="100%"
                    AutoGenerateColumns="false" runat="server" GridLines="None" DataKeyNames="PublicationID"
                     OnSorting="grdPublication_Sorting" OnRowCommand="grdPublication_RowCommand"
                      AllowSorting="true" OnPageIndexChanging="grdPublication_PageIndexChanging"  OnRowDeleting="grdPublication_RowDeleting">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="PublicationName" HeaderText="Name" SortExpression="PublicationName" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="PublicationCode" SortExpression="PublicationCode" HeaderText="Alias" ItemStyle-HorizontalAlign="Left">
                        </asp:BoundField>
                        <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" ItemStyle-Width="5%">
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="PublicationID" DataNavigateUrlFormatString="../edition/default.aspx?type=Active&amp;PublicationID={0}"
                            DataTextField="ActiveEdition" SortExpression="active" HeaderText="Active Editions"
                            ItemStyle-Width="5%"></asp:HyperLinkField>
                        <asp:HyperLinkField DataNavigateUrlFields="PublicationID" DataNavigateUrlFormatString="../edition/default.aspx?type=Archieve&amp;PublicationID={0}"
                            DataTextField="ArchievedEdition" SortExpression="ArchievedEdition" HeaderText="Archived Editions"
                            ItemStyle-Width="5%"></asp:HyperLinkField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-manageEdition.gif alt='Manage Editions' border='0'&gt;"
                            DataNavigateUrlFields="PublicationID" DataNavigateUrlFormatString="../edition/default.aspx?type=active&PublicationID={0}"
                            HeaderText="Manage Editions" ItemStyle-Width="5%"></asp:HyperLinkField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Publication' border='0'&gt;"
                            DataNavigateUrlFields="PublicationID" DataNavigateUrlFormatString="SetupPublication.aspx?PublicationID={0}"
                            HeaderText="Edit" ItemStyle-Width="5%"></asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete Publication" OnClientClick="return confirm('Are You Sure You want to delete?');"
                                    ImageUrl="/ecn.images/images/icon-delete1.gif" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.PublicationID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

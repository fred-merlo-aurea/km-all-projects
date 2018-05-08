<%@ Page EnableEventValidation="false" Language="c#" Inherits="ecn.accounts.channelsmanager.channels_main"
    CodeBehind="default.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td>
                <asp:GridView ID="grdChannels" AllowPaging="false" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                    runat="server" GridLines="None" DataKeyNames="BaseChannelID" AllowSorting="false" 
                    OnRowDeleting="grdChannels_RowDeleting" OnPageIndexChanging="grdChannels_PageIndexChanging"
                    OnRowCommand="grdChannels_RowCommand">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="BaseChannelName" HeaderText="Channel Partner Name" ItemStyle-HorizontalAlign="Left"
                            ></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Templates" ItemStyle-Width="10%" ItemStyle-VerticalAlign="middle">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlTemplates" runat="server" NavigateUrl='<%# Eval("BaseChannelID", "templates.aspx?ChannelID={0}") %>'
                                    Text="<center><img src=/ecn.images/images/icon-templates.gif border='0' alt='Edit / View Channel Partner Templates'></center>" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# Eval("BaseChannelID", "basechanneleditor.aspx?baseChannelID={0}") %>'
                                    Text="<center><img src=/ecn.images/images/icon-edits1.gif border='0' alt='Edit Channel Partner Details'></center>" />
                            </ItemTemplate>
                        </asp:TemplateField>
<%--                        <asp:TemplateField HeaderText="" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDelete" CommandArgument='<%# Eval("BaseChannelID") %>' CommandName="Delete" OnClientClick="return confirm('Are you Sure?\nSelected Channel will be permanently deleted. \n All Customers will be deleted.');"
                                    runat="server"><img src="/ecn.images/images/icon-delete1.gif" border='0' alt='Delete Channel Partner'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

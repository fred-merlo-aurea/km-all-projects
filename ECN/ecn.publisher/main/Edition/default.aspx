<%@ Page Language="c#" Inherits="ecn.publisher.main.Edition._default" CodeBehind="default.aspx.cs"
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
            <td class="tablecontent" valign="middle" align='right' height="30">
                Filter By:&nbsp;
                <asp:DropDownList class="formfield" ID="ddlPublication" runat="Server" AutoPostBack="true"
                    DataTextField="PublicationName" DataValueField="PublicationID" Visible="true"
                    OnSelectedIndexChanged="ddlPublication_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="ddlType" runat="Server" AutoPostBack="true" CssClass="formfield"
                    OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    <asp:ListItem Value="">----- Select -----</asp:ListItem>
                    <asp:ListItem Value="active">Active</asp:ListItem>
                    <asp:ListItem Value="inactive">InActive</asp:ListItem>
                    <asp:ListItem Value="archieve">Archive</asp:ListItem>
                    <asp:ListItem Value="Pending">Pending</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grdEdition" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                    runat="server" GridLines="None" DataKeyNames="EditionID" AllowSorting="true"
                    OnSorting="grdEdition_Sorting" OnPageIndexChanging="grdEdition_PageIndexChanging"
                    OnRowCommand="grdEdition_RowCommand" OnRowDataBound="grdEdition_RowDataBound"
                    OnRowDeleting="grdEdition_RowDeleting">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="EditionName" HeaderText="Edition Name" SortExpression="EditionName"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%"
                            ItemStyle-Width="25%"></asp:BoundField>
                        <asp:BoundField DataField="PublicationName" HeaderText="Publication Name" SortExpression="PublicationName"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%"
                            ItemStyle-Width="25%"></asp:BoundField>
                        <asp:BoundField DataField="EnableDate" HeaderText="Activation Date" SortExpression="EnableDate"
                            ItemStyle-HorizontalAlign="center" HeaderStyle-Width="10%" ItemStyle-Width="15%">
                        </asp:BoundField>
                        <asp:BoundField DataField="DisableDate" HeaderText="De-Activation Date" SortExpression="DisableDate"
                            ItemStyle-HorizontalAlign="center" HeaderStyle-Width="10%" ItemStyle-Width="15%">
                        </asp:BoundField>
                        <asp:BoundField DataField="Pages" HeaderText="Total Pages" ItemStyle-HorizontalAlign="Center"
                            SortExpression="Pages" HeaderStyle-Width="10%" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                            SortExpression="Status" HeaderStyle-Width="8%" ItemStyle-Width="8%"></asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Create Blast" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlCreateBlast" runat="server" NavigateUrl='<%# Eval("EditionID", "SelectTemplate.aspx?EditionID={0}") %>'
                                    Text="&lt;img src=/ecn.images/images/active-opens.gif alt='Create Blast' border='0'&gt;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="URL" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <a href="javascript:void(0);" onclick="javascript:window.open('EditionLinks.aspx?eid=<%# DataBinder.Eval(Container, "DataItem.EditionID")%>', '','left=100,top=100,height=240,width=600,resizable=no,scrollbar=yes,status=no')">
                                    <img src="/ecn.images/images/icon-linkTracking.gif" border='0'></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Preview" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <a href="javascript:void(0);" onclick="javascript:window.open('preview.aspx?eid=<%# DataBinder.Eval(Container, "DataItem.EditionID")%>', '','left=100,top=100,height=600,width=800,location=no,resizable=yes')">
                                    <img src="/ecn.images/images/icon-preview.gif" border='0'></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Links" ItemStyle-Width="3%">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlLinks" runat="server" NavigateUrl='<%# Eval("EditionID", "AddLinks.aspx?EditionID={0}") %>'
                                    Text="&lt;img src=/ecn.images/images/button.link.gif alt='View Links' border='0'&gt;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-linkAlias2.gif alt='Link Alias' border='0'&gt;"
                            DataNavigateUrlFields="EditionID" DataNavigateUrlFormatString="LinkAlias.aspx?EditionID={0}"
                            HeaderText="Alias" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="3%">
                        </asp:HyperLinkField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-reports.gif alt='View Reports' border='0'&gt;"
                            DataNavigateUrlFields="EditionID" DataNavigateUrlFormatString="Report.aspx?EditionID={0}"
                            HeaderText="Report" ItemStyle-Width="3%"></asp:HyperLinkField>
                        <asp:HyperLinkField Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Edition' border='0'&gt;"
                            DataNavigateUrlFields="EditionID" DataNavigateUrlFormatString="SetupEdition.aspx?EditionID={0}"
                            HeaderText="Edit" ItemStyle-Width="3%"></asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibDelete" runat="Server" CommandName="Delete" AlternateText="Delete Edition"
                                    OnClientClick="return confirm('Are You Sure You want to delete?');" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                    CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EditionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>

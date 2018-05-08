<%@ Page Language="c#" Inherits="ecn.accounts.main.Digital._default" CodeBehind="default.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader">
            </td>
        </tr>
        <tr>
            <td class="tableHeader" align='right'>
                Base Channels&nbsp;
                <asp:DropDownList class="formfield" ID="drpChannels" runat="Server" Visible="true"
                    Width="215px" AutoPostBack="true" DataTextField="BaseChannelName" DataValueField="BaseChannelID"
                    EnableViewState="true" OnSelectedIndexChanged="BaseChannelList_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Customers&nbsp;&nbsp;
                <asp:DropDownList class="formfield" ID="drpCustomers" runat="Server" Visible="true"
                    EnableViewState="true" DataValueField="CustomerID" DataTextField="CustomerName"
                    AutoPostBack="true" Width="215px" OnSelectedIndexChanged="CustomerList_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Status&nbsp;&nbsp;
                <asp:DropDownList ID="drpStatus" runat="Server" AutoPostBack="true" CssClass="formfield"
                    OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                    <asp:ListItem Value="">--- ALL ---</asp:ListItem>
                    <asp:ListItem Value="active">Active</asp:ListItem>
                    <asp:ListItem Value="inactive">InActive</asp:ListItem>
                    <asp:ListItem Value="archieve">Archieve</asp:ListItem>
                    <asp:ListItem Value="pending">Pending</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:DataGrid ID="dgEditions" CssClass="grid" runat="Server" HorizontalAlign="Center"
                    AutoGenerateColumns="False" AllowSorting="False" DataKeyField="EditionID" Width="100%"
                    OnDeleteCommand="dgEditions_DeleteCommand" OnItemDataBound="dgEditions_ItemDataBound">
                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="BaseChannelName" SortExpression="BaseChannelName" HeaderText="Channel">
                            <ItemStyle Width="15%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="CustomerName" SortExpression="CustomerName" HeaderText="Customer">
                            <ItemStyle Width="15%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EditionName" SortExpression="EditionName" HeaderText="Edition Name">
                            <ItemStyle Width="15%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="PublicationName" SortExpression="PublicationName" HeaderText="Publication Name">
                            <ItemStyle Width="15%"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="EnableDate" SortExpression="EnableDate" DataFormatString="{0:MM/dd/yyyy}"
                            HeaderText="Activation Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="DisableDate" SortExpression="DisableDate" DataFormatString="{0:MM/dd/yyyy}"
                            HeaderText="De-Activation Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Pages" SortExpression="Pages" HeaderText="Total Pages"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Status" SortExpression="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="URL" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <a href="javascript:void(0);" onclick="javascript:window.open('/ecn.publisher/main/Edition/EditionLinks.aspx?eid=<%# DataBinder.Eval(Container, "DataItem.EditionID")%>', '','left=100,top=100,height=240,width=600,resizable=no,scrollbar=yes,status=no')">
                                    <img src="/ecn.images/images/icon-linkTracking.gif" border='0'></a></ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-preview.gif alt='View Digital Edtion' border='0'&gt;"
                            DataNavigateUrlField="EditionID" DataNavigateUrlFormatString="javascript:var w =window.open('/ecn.publisher/main/Edition/Preview.aspx?eID={0}',null,'width=800,height=600,location=no,resizable=yes')"
                            HeaderText="View" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                        </asp:HyperLinkColumn>
                        <asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Edition' border='0'&gt;"
                            DataNavigateUrlField="EditionID" DataNavigateUrlFormatString="EditEdition.aspx?EditionID={0}"
                            HeaderText="Edit" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                        </asp:HyperLinkColumn>
                        <asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Convert to Digital Edition' border='0'&gt;"
                            DataNavigateUrlField="EditionID" DataNavigateUrlFormatString="ConvertDE.aspx?EditionID={0}"
                            HeaderText="Convert" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                        </asp:HyperLinkColumn>
                        <asp:TemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="Server" AlternateText="Delete Edition" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                    CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EditionID")%>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <au:pagerbuilder id="EditionsPager" runat="Server" width="100%" horizontalalign="Center"
                    pagesize="25" controltopage="dgEditions" onindexchanged="EditionsPager_IndexChanged">
                    <PagerStyle CssClass="gridpager"></PagerStyle>
                </au:pagerbuilder>
            </td>
        </tr>
    </table>
</asp:Content>

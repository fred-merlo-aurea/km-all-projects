<%@ Page Language="c#" Inherits="ecn.creator.events.EventList" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>

<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function deleteEvent(theID) {
            if (confirm('Are you Sure?\n Selected Event will be permanently deleted.')) {
                window.location = "default.aspx?EventID=" + theID;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="90%" border='0'>
        <tr>
            <td class="tableHeader" align='right' width="95%">Event Type:
                <asp:DropDownList EnableViewState="true" ID="EventTypeCode" runat="Server" DataValueField="CodeValue"
                    DataTextField="CodeDisplay" OnSelectedIndexChanged="EventType_SelectedIndexChanged"
                    AutoPostBack="True" CssClass='formfield'>
                </asp:DropDownList>

            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="EventsGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                    CssClass="grid">
                    <ItemStyle Height="22"></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="49%" DataField="EventName" HeaderText="Name"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="5%" DataField="DisplayFlag" HeaderText="Display" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="12%" DataField="StartDate" HeaderText="From" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="12%" DataField="EndDate" HeaderText="Until" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="12%" DataField="Times" HeaderText="Timing" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <a href='EventDetail.aspx?EventID=<%# DataBinder.Eval(Container.DataItem, "EventID") %>&amp;action=Edit'>
                                    <center>
                                        <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Event' border='0'></center>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:HyperLinkColumn ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete Event' border='0'>"
                            DataNavigateUrlField="EventID" DataNavigateUrlFormatString="javascript:deleteEvent({0});" ItemStyle-HorizontalAlign="center"
                            HeaderStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                    </Columns>
                    <AlternatingItemStyle CssClass="gridaltrow" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>


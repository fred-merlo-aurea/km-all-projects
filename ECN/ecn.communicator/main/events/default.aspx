<%@ Page Language="c#" Inherits="ecn.communicator.main.events._default" CodeBehind="default.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="label" colspan="2">
                    &nbsp;Blast Plans<br />
                    <asp:DataGrid ID="BlastEvents" runat="Server" AutoGenerateColumns="False" Width="100%"
                        CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="BlastID" HeaderText="BlastID"></asp:BoundColumn>
                            <asp:BoundColumn DataField="EventType" HeaderText="Type of Event"></asp:BoundColumn>
                            <asp:BoundColumn DataField="PlanType" HeaderText="Period or Day of Month"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Period" HeaderText="Period"></asp:BoundColumn>
                            <asp:BoundColumn DataField="BlastDay" HeaderText="Day of Month"></asp:BoundColumn>
                            <asp:HyperLinkColumn Text="Edit Blast" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="blasteditor.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn Text="Delete" DataNavigateUrlField="BlastPlanID" DataNavigateUrlFormatString="BlastPlanner.aspx?BlastPlanID={0}&e=delete">
                            </asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid>
                    <br />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

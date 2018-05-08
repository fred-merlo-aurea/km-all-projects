<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarketPenetration.ascx.cs" Inherits="KMPS.MD.Main.Widgets.MarketPenetration" %>


<asp:UpdatePanel ID="UpdatePanelMarketPenetration" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="PnlChart" runat="server">

            <telerik:RadHtmlChart runat="server" ID="RadHtml1" Height="400px" Width="600px" Legend-Appearance-Position="Bottom">
                <PlotArea>
                    <Series>
                        <telerik:PieSeries DataFieldY="Counts" NameField="Markets">
                            <LabelsAppearance DataFormatString="{0}">
                            </LabelsAppearance>
                            <TooltipsAppearance Color="White" ClientTemplate="#=dataItem.Markets#: #=dataItem.Counts#"></TooltipsAppearance>
                        </telerik:PieSeries>
                    </Series>
                    <YAxis>
                    </YAxis>
                </PlotArea>
                <ChartTitle Text="">
                </ChartTitle>
            </telerik:RadHtmlChart>

        </asp:Panel>

        <asp:Panel ID="PnlSettings" runat="server" Visible="false">
            <table>
                <tr>
                    <td>Saved Reports:
                        &nbsp;
                             <asp:DropDownList ID="drpdownReports" runat="server">
                             </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="lbtnApplyFilters" runat="server"
                                OnClick="lbtnApplyFilters_Click">Apply Filters</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="Label1" runat="server"></asp:Label>

    </ContentTemplate>
</asp:UpdatePanel>

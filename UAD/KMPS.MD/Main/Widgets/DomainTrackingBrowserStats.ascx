<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainTrackingBrowserStats.ascx.cs" Inherits="KMPS.MD.Main.Widgets.DomainTrackingBrowserStats" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadHtmlChart runat="server" ID="PieChart1" Width="600" Height="500" Transitions="true">
            <ChartTitle Text="Domain Tracking Browser Stats">
                <Appearance Align="Center" Position="Top">
                </Appearance>
            </ChartTitle>
            <Legend>
                <Appearance Position="Right" Visible="true">
                </Appearance>
            </Legend>
            <PlotArea>
                <Series>
                    <telerik:PieSeries StartAngle="90">
                        <LabelsAppearance Position="OutsideEnd" DataFormatString="{0} %">
                        </LabelsAppearance>
                        <TooltipsAppearance Color="White" DataFormatString="{0} %"></TooltipsAppearance>
                        <SeriesItems>
                            <telerik:PieSeriesItem BackgroundColor="#ff9900" Exploded="true" Name="Internet Explorer"
                                Y="18.3"></telerik:PieSeriesItem>
                            <telerik:PieSeriesItem BackgroundColor="#cccccc" Exploded="false" Name="Firefox"
                                Y="35.8"></telerik:PieSeriesItem>
                            <telerik:PieSeriesItem BackgroundColor="#999999" Exploded="false" Name="Chrome" Y="38.3"></telerik:PieSeriesItem>
                            <telerik:PieSeriesItem BackgroundColor="#666666" Exploded="false" Name="Safari" Y="4.5"></telerik:PieSeriesItem>
                            <telerik:PieSeriesItem BackgroundColor="#333333" Exploded="false" Name="Opera" Y="2.3"></telerik:PieSeriesItem>
                        </SeriesItems>
                    </telerik:PieSeries>
                </Series>
            </PlotArea>
        </telerik:RadHtmlChart>
    </ContentTemplate>
</asp:UpdatePanel>

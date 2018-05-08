<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TotalVisits.ascx.cs" Inherits="KMPS.MD.Main.Widgets.TotalVisits" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        From Date  
        <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="RadMonthYearPickerStart" runat="server" Width="150px">
        </telerik:RadMonthYearPicker>
        To Date
            <telerik:RadMonthYearPicker RenderMode="Lightweight" ID="RadMonthYearPickerEnd" runat="server" Width="150">
            </telerik:RadMonthYearPicker>
        <asp:Button ID="btnrefresh" runat="server" CssClass="button" OnClick="btnrefresh_Click" Text="Refresh" />
        <br />
        <telerik:RadHtmlChart runat="server" ID="RadHtmlChart1" Width="800" Height="500" Transitions="true">
            <Appearance>
                <FillStyle BackgroundColor="Transparent"></FillStyle>
            </Appearance>
            <Legend>
                <Appearance BackgroundColor="Transparent" Position="Bottom">
                </Appearance>
            </Legend>
            <PlotArea>
                <Series>
                </Series>
                <Appearance>
                    <FillStyle BackgroundColor="Transparent"></FillStyle>
                </Appearance>
                <YAxis>
                    <LabelsAppearance DataFormatString="{0:N0}">
                    </LabelsAppearance>
                </YAxis>
            </PlotArea>
        </telerik:RadHtmlChart>
    </ContentTemplate>
</asp:UpdatePanel>

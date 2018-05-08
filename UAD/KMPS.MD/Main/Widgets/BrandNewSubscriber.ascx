<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrandNewSubscriber.ascx.cs" Inherits="KMPS.MD.Main.Widgets.BrandNewSubscriber" %>

<script type="text/javascript">
    function RadHtmlChartBNS_OnClientSeriesClicked(sender, args) {
        var ajaxManager = $find("<%=RadAjaxManager1.ClientID%>");
        //alert(args.get_category());

        ajaxManager.ajaxRequest(args.get_category());
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="float: left; width: 65%; overflow: hidden;">
            From Date  
        <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDateStart" Width="100px" runat="server" DateInput-Label="" EnableAjaxSkinRendering="true" ZIndex="99999">
        </telerik:RadDatePicker>
            To Date
            <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDateEnd" CssClass="toDate" Width="100px" runat="server" DateInput-Label="" EnableAjaxSkinRendering="true" ZIndex="99999">
            </telerik:RadDatePicker>
            <asp:Button ID="btnrefresh" runat="server" CssClass="button" OnClick="btnrefresh_Click"  Text="Refresh"  />
        </div>

        <div style="width: 35%; overflow: hidden; vertical-align: middle; text-align: right;">
            <asp:CheckBox ID="chkShowLegend" runat="server" OnCheckedChanged="chkShowLegend_Checked" AutoPostBack="true" Text=" Show Legend" Font-Size="X-Small" />
            <div style="text-align:right"><img src="../Images/icon-DrillI.png" style="border:none" /></div>
        </div>

        <div class="demo-container size-wide" style="text-align:right">
            <telerik:RadHtmlChart runat="server" ID="RadHtmlChartBNS" Width="600" Height="500" OnClientSeriesClicked="RadHtmlChartBNS_OnClientSeriesClicked" IntelligentLabelsEnabled ="true" Legend-Appearance-Position="Bottom">
                <PlotArea>
                    <Series>
                        <telerik:DonutSeries>
                            <LabelsAppearance  Position="OutsideEnd">
                            </LabelsAppearance>
                            <TooltipsAppearance Color="White"></TooltipsAppearance>
                        </telerik:DonutSeries>
                    </Series>
                    <YAxis>
                    </YAxis>
                </PlotArea>
            </telerik:RadHtmlChart>
        </div>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadHtmlChartBNS" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" Height="77px" Width="113px" runat="server">
        </telerik:RadAjaxLoadingPanel>
    </ContentTemplate>
</asp:UpdatePanel>

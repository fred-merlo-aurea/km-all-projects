<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitActivity.ascx.cs" Inherits="KMPS.MD.Main.Widgets.VisitActivity" %>
<style type="text/css"> 
   .RadComboBoxDropDown 
   {     
      font-size:11px !important; 
   }  
</style> 
<asp:UpdatePanel ID="UpdatePanelBrandTrends" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        From Date  
        <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDateStart" Width="100px" runat="server" DateInput-Label="" EnableAjaxSkinRendering="true">
        </telerik:RadDatePicker>
        To Date
            <telerik:RadDatePicker RenderMode="Lightweight" ID="RadDateEnd" CssClass="toDate" Width="100px" runat="server" DateInput-Label="" EnableAjaxSkinRendering="true">
            </telerik:RadDatePicker>
        &nbsp;
        <telerik:RadComboBox RenderMode="Lightweight" ID="rcbDomains" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
            Width="350" Label="Select Domains:" DataTextField="DomainName" DataValueField="DomainTrackingID"  >
        </telerik:RadComboBox>
        <asp:Button ID="btnrefresh" runat="server" CssClass="button" OnClick="btnrefresh_Click" Text="Refresh" />

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
                <XAxis DataLabelsField="Date"></XAxis>
                <YAxis>
                    <LabelsAppearance DataFormatString="{0:N0}">
                    </LabelsAppearance>
                </YAxis>
            </PlotArea>
        </telerik:RadHtmlChart>
    </ContentTemplate>
</asp:UpdatePanel>

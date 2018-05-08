<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterVennDiagram.ascx.cs" Inherits="KMPS.MD.Controls.FilterVennDiagram" %>
<asp:Panel ID="bpChart" runat="server" CssClass="collapsePanelHeader" Height="28px" Visible="true">
    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
        <div style="float: left;">
            Filter Venn Diagram
        </div>
        <div style="float: left; margin-left: 20px;">
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="bpChartbody" runat="server" CssClass="collapsePanel" BorderColor="#5783BD"
    Visible="true" BorderWidth="1">
    <div id="venn" runat="server"></div>
    <asp:HiddenField ID="hidvennparam" runat="server" />
</asp:Panel>
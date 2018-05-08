<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="KMPS.MD.Main.Dashboard" %>

<%@ Register Assembly="Kalitte.Dashboard.Framework" Namespace="Kalitte.Dashboard.Framework" TagPrefix="kalitte" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        function doOnLoad() {
            if (typeof (attachWidgetHandlers) != 'undefined') {
                attachWidgetHandlers();
            };
        }

        function doUnLoad() {
            if (typeof (ClearAllAutoRefresh) != 'undefined') {
                ClearAllAutoRefresh();
            };
        }
    </script>

    <style>
        .kdashstyle {
            padding: 10px 10px 10px 10px;
        }
    </style>

    <kalitte:ScriptManager ID="ScriptManager1" runat="server">
    </kalitte:ScriptManager>

    <asp:Panel ID="pnlBrand" runat="server" Visible="false">
        <table border="0" cellpadding="1" cellspacing="1" width="100%" style="padding-left: 10px; padding-right: 10px; padding-bottom: 5px;">
            <tr>
                <td valign="middle" align="left" width="50%">
                    <font style="font-family: Segoe UI Semibold; font-size: 1.2rem; font-weight: bold; color: #178fb7;">Brand : </font>
                    <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Segoe UI Semibold" Font-Size="medium"
                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                        DataValueField="BrandID" Width="250px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td valign="middle" align="right" width="50%">
                    <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image><br />
                </td>
            </tr>
        </table>
    </asp:Panel>
  <%--  <div style="text-align: right; font-style: 100%">
        <asp:DropDownList ID="drpKTheme" runat="server" Font-Names="Segoe UI Semibold" Font-Size="medium" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="drpKTheme_SelectedIndexChanged"></asp:DropDownList>
    </div>--%>
    <kalitte:DashboardSurface ID="DashboardSurface1" runat="server" CssClass="kdashstyle"
        OnDashboardCommand="DashboardSurface1_DashboardCommand"
        OnDashboardToolbarPrepare="DashboardSurface1_DashboardToolbarPrepare"
        CreateWidgetsButtonText="Add Reports"
        WidgetTypePanelTitle="Mark reports to add" OnInit="OnInit"
        OnWidgetAdded="DashboardSurface1_WidgetAdded" onwidgetCommand="DashboardSurface1_WidgetCommand" OnAfterWidgetCommand="DashboardSurface1_OnAfterWidgetCommand" LoadingDashboardMask="Loading..." />
</asp:Content>

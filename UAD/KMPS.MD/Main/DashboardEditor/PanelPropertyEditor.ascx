<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PanelPropertyEditor.ascx.cs"
    Inherits="KMPS.MD.Main.DashboardEditor.PanelPropertyEditor" %>
<asp:Panel ID="Panel1" runat="server" Visible="false">

<style type="text/css">
    .style1
    {
        width: 195px;
    }
</style>
<table style="width:100%">
    <tr>
        <td class="style1">
            <asp:Label ID="Label2" runat="server" Text="Icon" 
                meta:resourcekey="Label2Resource1"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ctlDashboardIcon" runat="server" Width="100%" 
                meta:resourcekey="ctlDashboardIconResource1">
            </asp:DropDownList>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label1" runat="server" Text="Header" 
                meta:resourcekey="Label1Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlDashboardHeader" runat="server" Checked="True" 
                meta:resourcekey="ctlDashboardHeaderResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
             <asp:Label ID="Label3" runat="server" Text="Border" 
                 meta:resourcekey="Label3Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlBorder" runat="server" 
                meta:resourcekey="ctlBorderResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
             <asp:Label ID="Label4" runat="server" Text="BodyBorder" 
                 meta:resourcekey="Label4Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlBodyBorder" runat="server" 
                meta:resourcekey="ctlBodyBorderResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label5" runat="server" Text="Collapsible" 
                meta:resourcekey="Label5Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlCollapsible" runat="server" 
                meta:resourcekey="ctlCollapsibleResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label6" runat="server" Text="TitleCollapse" 
                meta:resourcekey="Label6Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlTitleCollapse" runat="server" 
                meta:resourcekey="ctlTitleCollapseResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label7" runat="server" Text="HideCollapseTool" 
                meta:resourcekey="Label7Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlHideCollapseTool" runat="server" 
                meta:resourcekey="ctlHideCollapseToolResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label8" runat="server" Text="AutoWidth" 
                meta:resourcekey="Label8Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlAutoWidth" runat="server" 
                meta:resourcekey="ctlAutoWidthResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label9" runat="server" Text="AutoHeight" 
                meta:resourcekey="Label9Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlAutoHeight" runat="server" 
                meta:resourcekey="ctlAutoHeightResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
<tr>
        <td class="style1">
            <asp:Label ID="Label10" runat="server" Text="Dragable" 
                meta:resourcekey="Label10Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlDragable" runat="server" 
                meta:resourcekey="ctlDragableResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label11" runat="server" Text="AutoScroll" 
                meta:resourcekey="Label11Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlAutoScroll" runat="server" 
                meta:resourcekey="ctlAutoScrollResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label12" runat="server" Text="FitLayout" 
                meta:resourcekey="Label12Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlFitLayout" runat="server" 
                meta:resourcekey="ctlFitLayoutResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label13" runat="server" Text="Stretch" 
                meta:resourcekey="Label13Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlStretch" runat="server" 
                meta:resourcekey="ctlStretchResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    
    <tr>
        <td class="style1">
            <asp:Label ID="Label14" runat="server" Text="Padding" 
                meta:resourcekey="Label14Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlPadding" runat="server" Width="100%" 
                meta:resourcekey="ctlPaddingResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label15" runat="server" Text="PaddingSummary" 
                meta:resourcekey="Label15Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlPaddingSummary" runat="server" 
                ValidationGroup="PanelEditor" Width="100%" 
                meta:resourcekey="ctlPaddingSummaryResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    
    

        <tr>
        <td class="style1">
            <asp:Label ID="Label16" runat="server" Text="Enabled" 
                meta:resourcekey="Label16Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlEnabled" runat="server" Checked="True" 
                meta:resourcekey="ctlEnabledResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label17" runat="server" Text="DisabledClass" 
                meta:resourcekey="Label17Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlDisabledClass" runat="server" Width="100%" 
                meta:resourcekey="ctlDisabledClassResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
        <tr>
        <td class="style1">
            <asp:Label ID="Label18" runat="server" Text="CtCls" 
                meta:resourcekey="Label18Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlCtCls" runat="server" Width="100%" 
                meta:resourcekey="ctlCtClsResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label19" runat="server" Text="Frame" 
                meta:resourcekey="Label19Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlFrame" runat="server" 
                meta:resourcekey="ctlFrameResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label20" runat="server" Text="Shim" 
                meta:resourcekey="Label20Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlShim" runat="server" meta:resourcekey="ctlShimResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label21" runat="server" Text="Unstyled" 
                meta:resourcekey="Label21Resource1"></asp:Label>
        </td>
        <td>
            <asp:CheckBox ID="ctlUnstyled" runat="server" 
                meta:resourcekey="ctlUnstyledResource1" />
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    
    <tr>
        <td class="style1">
            <asp:Label ID="Label22" runat="server" Text="Width" 
                meta:resourcekey="Label22Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlDashboardWidth" runat="server" ValidationGroup="PanelEditor"
                Width="100%" meta:resourcekey="ctlDashboardWidthResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label23" runat="server" Text="Height" 
                meta:resourcekey="Label23Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlDashboardHeight" runat="server" ValidationGroup="PanelEditor"
                Width="100%" meta:resourcekey="ctlDashboardHeightResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label24" runat="server" Text="BodyStyle" 
                meta:resourcekey="Label24Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlBodyStyle" runat="server" Rows="3" TextMode="MultiLine" ValidationGroup="PanelEditor"
                Width="100%" meta:resourcekey="ctlBodyStyleResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label25" runat="server" Text="BodyCssClass" 
                meta:resourcekey="Label25Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlBodyCssClass" runat="server" ValidationGroup="PanelEditor" 
                Width="100%" meta:resourcekey="ctlBodyCssClassResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label26" runat="server" Text="BaseCls" 
                meta:resourcekey="Label26Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlBaseCls" runat="server" ValidationGroup="PanelEditor" 
                Width="100%" meta:resourcekey="ctlBaseClsResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label27" runat="server" Text="Cls" 
                meta:resourcekey="Label27Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlCls" runat="server" ValidationGroup="PanelEditor" 
                Width="100%" meta:resourcekey="ctlClsResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label28" runat="server" Text="CollapsedCls" 
                meta:resourcekey="Label28Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlCollapsedCls" runat="server" ValidationGroup="PanelEditor" 
                Width="100%" meta:resourcekey="ctlCollapsedClsResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label29" runat="server" Text="IconCls" 
                meta:resourcekey="Label29Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlIconCls" runat="server" ValidationGroup="PanelEditor" 
                Width="100%" meta:resourcekey="ctlIconClsResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="style1">
            <asp:Label ID="Label30" runat="server" Text="StyleSpec" 
                meta:resourcekey="Label30Resource1"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="ctlStyleSpec" runat="server" Rows="3" TextMode="MultiLine" ValidationGroup="PanelEditor"
                Width="100%" meta:resourcekey="ctlStyleSpecResource1"></asp:TextBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
</asp:Panel>
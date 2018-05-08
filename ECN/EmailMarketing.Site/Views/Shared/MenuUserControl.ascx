<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuUserControl.ascx.cs" Inherits="EmailMarketing.Site.MenuUserControl" %>

    <asp:Menu 
        id="Menu" 
        DataSourceID="SiteMapMain" 
        runat="server" 
        DynamicHorizontalOffset="0"
        StaticSubMenuIndent="10px" 
        Orientation="Horizontal" 
        OnMenuItemDataBound="Menu_MenuItemDataBound"
        StaticEnableDefaultPopOutImage="False" 
        DynamicEnableDefaultPopOutImage="false" 
        CssClass="MenuClass" 
        RenderingMode="List" 
        ItemWrap="false">
        <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="level1"/></LevelMenuItemStyles>
        <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="level2"/>
        </LevelMenuItemStyles>
        <LevelMenuItemStyles>
            <asp:MenuItemStyle CssClass="level3"/>
        </LevelMenuItemStyles>
    </asp:Menu>
    <asp:SiteMapDataSource ID="SiteMapMain" runat="server" ShowStartingNode="False"/>
    <asp:SiteMapDataSource ID="SiteMapSecondLevel" runat="server" ShowStartingNode="False"/>
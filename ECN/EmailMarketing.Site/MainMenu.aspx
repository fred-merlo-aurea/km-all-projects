<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.aspx.cs" Inherits="EmailMarketing.Site.MainMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
    </div>
    </form>
</body>
</html>

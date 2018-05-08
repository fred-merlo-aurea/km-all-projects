<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Page Language="c#" Inherits="ecn.creator.pages.pagelist" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>

<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="../../includes/folderSystem.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function deleteContent(theID) {
            if (confirm('Are you Sure?\n Selected content will be permanently deleted.')) {
                window.location = "default.aspx?ContentID=" + theID + "&action=delete";
            }
        }
        function deletePage(theID) {
            if (confirm('Are you Sure?\n Selected Page will be permanently deleted.')) {
                window.location = "default.aspx?PageID=" + theID + "&action=delete";
            }
        }
        function openContentWindow(cID) {
            window.open('/ecn.communicator/main/content/contentPreview.aspx?ContentID=' + cID + '&amp;type=html', '', 'width=800,height=600,resizable=yes,scrollbars=yes')
        }
        function openPageWindow(pID) {
            window.open('preview.aspx?PageID=' + pID + '&amp;type=html', '', 'width=800,height=600,resizable=yes,scrollbars=yes,status=yes')
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:Label ID="ErrorLabel" runat="Server" Text="" Visible="false" CssClass="tableHeader"> </asp:Label>
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader" align="left" bgcolor='#eeeeee' width="100%" colspan="2">
                <asp:UpdatePanel ID="upContent" runat="server" UpdateMode ="conditional">
                    <ContentTemplate>

                <cpanel:DataPanel ID="DataPanel1" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                    CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Contents"
                    ExpandText="Click to list Contents" Collapsed="False" TitleText="View Contents"
                    AllowTitleExpandCollapse="True">
                    <table width="100%" border='0' bgcolor="#FFFFFF">
                        <tr>
                            <td class="tableHeader">
                                <div style="border: 1px #000 solid; width: 475px; height: 88px; overflow: auto;">
                                    <ecn:FolderSys ID="ContentFolderID" runat="Server" Width="467" Height="80"></ecn:FolderSys>
                                </div>
                            </td>
                            <td align='right' valign="bottom">
                                <asp:DropDownList ID="ContentUserID" AutoPostBack="true" OnSelectedIndexChanged="ContentUserID_SelectedIndexChanged1" runat="Server" DataValueField="UserID"
                                    DataTextField="UserName" CssClass="formfield">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataGrid ID="ContentGrid" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False"
                                    AllowPaging="True" PageSize="15" OnItemDataBound="ContentGrid_ItemDataBound">
                                    <ItemStyle Height="22"></ItemStyle>
                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                    <Columns>
                                        <asp:BoundColumn ItemStyle-Width="70%" DataField="ContentTitle" HeaderText="Content Title"></asp:BoundColumn>
                                        <asp:HyperLinkColumn ItemStyle-Width="6%" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Content as HTML' border='0'>" DataNavigateUrlField="ContentID" DataNavigateUrlFormatString="javascript:openContentWindow({0});" ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                                        <asp:TemplateColumn HeaderText="" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <center>&nbsp;<img src='/ecn.images/images/<%# DataBinder.Eval(Container.DataItem, "LockedFlag") .Equals("N") ? "icon-nolock.gif' alt='Content NOT Locked'":"icon-lock.gif' alt='Content Locked'" %>' border='0'>&nbsp;</center>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <a href='contentdetail.aspx?ContentID=<%# DataBinder.Eval(Container.DataItem, "ContentIDplus") %>&amp;action=Edit'>
                                                    <center>
                                                        <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Content' border='0'></center>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDeleteContent" ItemStyle-Width="5%" runat="server" ImageURL="/ecn.images/images/icon-delete1.gif" OnClick="imgbtnDeleteContent_Click" ></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                    </Columns>
                                    <AlternatingItemStyle CssClass="gridaltrow" />
                                </asp:DataGrid>
                                <AU:PagerBuilder ID="ContentPager" OnIndexChanged="ContentPager_IndexChanged" runat="Server" Width="100%" PageSize="10" ControlToPage="ContentGrid">
                                    <PagerStyle CssClass="gridpager"></PagerStyle>
                                </AU:PagerBuilder>
                            </td>
                        </tr>
                    </table>
                </cpanel:DataPanel>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" colspan="4" height="5"></td>
        </tr>
        <tr>
            <td class="tableHeader" align="left" bgcolor='#eeeeee' width="100%" colspan="2">
                <asp:UpdatePanel ID="upPages" runat="server" UpdateMode="conditional">
                    <ContentTemplate>

                <cpanel:DataPanel ID="Datapanel2" runat="Server" ExpandImageUrl="/ecn.images/images/collapse2.gif"
                    CollapseImageUrl="/ecn.images/images/collapse2.gif" CollapseText="Click to hide Pages"
                    ExpandText="Click to list Pages" Collapsed="False" TitleText="View Pages" AllowTitleExpandCollapse="True">
                    <table width="100%" border='0' bgcolor="#FFFFFF">
                        <tr>
                            <td class="tableHeader">
                                <div style="border: 1px #000 solid; width: 475px; height: 88px; overflow: auto;">
                                    <ecn:FolderSys ID="PageFolderID" runat="Server" Width="467" Height="80" CssClass="formfield"></ecn:FolderSys>
                                </div>
                            </td>
                            <td align='right' valign="bottom">
                                <asp:DropDownList ID="PageUserID" AutoPostBack="true" OnSelectedIndexChanged="PageUserID_SelectedIndexChanged1" runat="Server" DataValueField="UserID"
                                    DataTextField="UserName" CssClass="formfield">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataGrid ID="PagesGrid" runat="Server" Width="100%" CssClass="grid" AutoGenerateColumns="False"
                                    AllowPaging="True" PageSize="15">
                                    <ItemStyle Height="22"></ItemStyle>
                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                    <Columns>
                                        <asp:BoundColumn ItemStyle-Width="30%" DataField="PageName" HeaderText="Page Name"></asp:BoundColumn>
                                        <asp:BoundColumn ItemStyle-Width="30%" DataField="QueryValue" HeaderText="Page Identifier"></asp:BoundColumn>
                                        <asp:BoundColumn ItemStyle-Width="9%" DataField="PageSize" HeaderText="Page Size" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                                        <asp:BoundColumn ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%" DataField="SlotsTotal" HeaderText="Slots" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
                                        <asp:HyperLinkColumn ItemStyle-Width="6%" Text="<img src=/ecn.images/images/icon-preview-HTML.gif alt='Preview Content as HTML' border='0'>" DataNavigateUrlField="PageID" DataNavigateUrlFormatString="javascript:openPageWindow({0});" ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                                        <asp:TemplateColumn HeaderText="" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <a href='pagedetail.aspx?PageID=<%# DataBinder.Eval(Container.DataItem, "PageID") %>&amp;action=Edit'>
                                                    <center>
                                                        <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Content' border='0'></center>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:HyperLinkColumn ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete Page' border='0'>" DataNavigateUrlField="PageID" DataNavigateUrlFormatString="javascript:deletePage({0});" ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                                    </Columns>
                                    <AlternatingItemStyle CssClass="gridaltrow" />
                                </asp:DataGrid>
                                <AU:PagerBuilder ID="PagesPager" runat="Server" Width="100%" OnIndexChanged="PagesPager_IndexChanged" PageSize="10" ControlToPage="PagesGrid">
                                    <PagerStyle CssClass="gridpager"></PagerStyle>
                                </AU:PagerBuilder>
                            </td>
                        </tr>
                    </table>
                </cpanel:DataPanel>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>


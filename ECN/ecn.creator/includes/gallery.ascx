<%@ Reference Control="~/includes/uploader.ascx" %>
<%@ Control Language="c#" Inherits="ecn.creator.includes.gallery" Codebehind="gallery.ascx.cs" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="uploader.ascx" %>
<asp:Panel ID="PanelTabs" Visible="true" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td align="left" width="25">
                <br>
                <asp:Label ID="browse_img" runat="server" Text="browse_img"></asp:Label></td>
            <td valign="bottom" width="120">
                <asp:LinkButton ID="tabBrowse" OnClick="showBrowse" Text="Browse Images" CssClass="menu1"
                    runat="server"></asp:LinkButton>&nbsp;</td>
            <td align="left" width="25">
                <br>
                <asp:Label ID="browse_other" runat="server" Text="browse_img"></asp:Label></td>
            <td valign="bottom" width="150">
                <asp:LinkButton ID="tabBrowseOther" OnClick="showBrowseOther" runat="server" Text="Browse Other Media"
                    CssClass="menu1"></asp:LinkButton>&nbsp;</td>
            <td valign="bottom" align="left" width="25">
                <asp:Label ID="upload_img" runat="server" Text="upload_img"></asp:Label></td>
            <td valign="bottom" width="80">
                <asp:LinkButton ID="tabUpload" OnClick="showUpload" Text="Upload" CssClass="menu1"
                    runat="server"></asp:LinkButton>&nbsp;</td>
            <td width="*">
                &nbsp;</td>
            <td valign="bottom" width="200">
                <asp:LinkButton ID="tabPreview" OnClick="showPreview" Visible="False" Text="preview"
                    CssClass="menu1" runat="server"></asp:LinkButton>&nbsp;</td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PanelBrowse" Visible="true" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="1">
        <tr>
            <td align="center">
                <asp:TextBox ID="imagepath" runat="server" Visible="False"></asp:TextBox>
                <asp:TextBox ID="imagesize" runat="server" Visible="False">100</asp:TextBox>
                <asp:DataList ID="imagerepeater" runat="server" CellSpacing="0" CellPadding="4" GridLines="Both"
                    BorderWidth="1" BorderColor="black" RepeatColumns="4" OnItemCommand="imagerepeater_ItemCommand" RepeatLayout="Table">
                    <ItemTemplate>
                        <table class="tableContent" align="center" border="0">
                            <tr>
                                <td valign="middle" align="center" height='<%=thumbnailSize%>'>
                                    <asp:ImageButton runat="server" Width="150" ID="BrowseImage" CommandName="previewImage" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ImagePath")%>'
                                        ImageUrl='<%#DataBinder.Eval(Container.DataItem,"ImageKey")%>'></asp:ImageButton>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center" class="tableContent">
                                    <asp:LinkButton runat="server" ID="BrowseImageLink" CommandName="previewLink" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"ImagePath")%>'
                                        Text='<%#DataBinder.Eval(Container.DataItem,"ImageName")%>' CssClass="menu1">
                                    </asp:LinkButton>
                                    <br>
                                    '<%#DataBinder.Eval(Container.DataItem,"ImageSize")%>' - '<%#DataBinder.Eval(Container.DataItem,"ImageDtModified")%>'
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PanelPreview" Visible="false" runat="server">
    <table style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid;
        border-bottom: 1px solid" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td>
                <table cellpadding="10" width="100%" border="0">
                    <tr>
                        <td colspan="2">
                            <asp:HyperLink ID="fullsizeLink" runat="server" Text="full size image" CssClass="menu1"
                                Target="_blank"></asp:HyperLink>&nbsp;|&nbsp;
                            <asp:LinkButton ID="deleteLink" runat="server" Text="delete image" CssClass="menu1"
                                OnCommand="deleteImage"></asp:LinkButton>&nbsp;| <a class="menu1" href="?imagefile=">
                                    back</a></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tableContent" valign="top" align="center" width="300" height="300">
                <asp:Image ID="ImagePreview" runat="server" CssClass="menu1"></asp:Image></td>
            <td class="tablecontent" valign="top">
                <br>
                &nbsp;Name:
                <asp:Label ID="previewName" runat="server" Text="Name"></asp:Label><br>
                &nbsp;Size:
                <asp:Label ID="previewSize" runat="server" Text="Size"></asp:Label><br>
                &nbsp;Date:
                <asp:Label ID="previewDate" runat="server" Text="Date"></asp:Label><br>
                &nbsp;Height:
                <asp:Label ID="previewHeight" runat="server" Text="Height"></asp:Label><br>
                &nbsp;Width:
                <asp:Label ID="previewWidth" runat="server" Text="Width"></asp:Label><br>
                &nbsp;Resolution:
                <asp:Label ID="previewResolution" runat="server" Text="Width"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PanelBrowseOther" Visible="False" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="1">
        <tr>
            <td>
                <asp:DataGrid ID="FileGrid" Visible="True" runat="server" Width="100%" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="15" OnPageIndexChanged="Grid_Change" CssClass="grid">
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <ItemStyle></ItemStyle>
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <Columns>
                        <asp:BoundColumn DataField="FileName" HeaderText="File"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Size" HeaderText="Size"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Date" HeaderText="Date"></asp:BoundColumn>
                        <asp:HyperLinkColumn Text="Preview" DataNavigateUrlField="BrowsePath"></asp:HyperLinkColumn>
                        <asp:HyperLinkColumn Text="Delete" DataNavigateUrlField="FileName" DataNavigateUrlFormatString="../main/media/default.aspx?deletefile={0}">
                        </asp:HyperLinkColumn>
                    </Columns>
                </asp:DataGrid></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="PanelUpload" Visible="false" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="1">
        <tr>
            <td align="center">
                <br>
                <ecn:uploader ID="pageuploader" runat="server"></ecn:uploader>
                <br>
                <br>
            </td>
        </tr>
    </table>
</asp:Panel>

<%@ Reference Control="~/includes/uploader.ascx" %>
<%@ Control Language="c#" Inherits="ecn.communicator.includes.gallery" Codebehind="gallery.ascx.cs" %>
<%@ Register TagPrefix="ecn" TagName="uploader" Src="uploader.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td width="40%">
                    </td>
                    <td width="60%" valign="bottom" align="right">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right" valign="bottom">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td class="tablft" height="19" width="6">
                                            </td>
                                            <td class="tabbg" width="15">
                                            </td>
                                            <td class="tabbg">
                                                <img border="0" src='/ecn.images/icons/browse_img.gif'></td>
                                            <td class="tabbg" width="5">
                                            </td>
                                            <td class="tabbg">
                                                <asp:LinkButton ID="tabBrowse" OnClick="showBrowse" Text="" runat="server">Browse Images</asp:LinkButton></td>
                                            <td class="tabbg" width="5">
                                            </td>
                                            <td class="tabrt" width="6">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="2">
                                </td>
                                <td align="right" valign="bottom">
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td class="tablft" height="19" width="6">
                                            </td>
                                            <td class="tabbg" width="15">
                                            </td>
                                            <td class="tabbg">
                                                <img border="0" src='/ecn.images/icons/upload_img.gif'></td>
                                            <td class="tabbg" width="5">
                                            </td>
                                            <td class="tabbg">
                                                <asp:LinkButton ID="tabUpload" OnClick="showUpload" Text="" runat="server">Upload Images</asp:LinkButton></td>
                                            <td class="tabbg" width="5">
                                            </td>
                                            <td class="tabrt" width="6">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="2">
                                </td>
                                <asp:Panel ID="showPreviewPanel" runat="server" Visible="False">
                                    <td width="2">
                                    </td>
                                    <td align="right" valign="bottom">
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                            <tr>
                                                <td class="tablft" height="19" width="6">
                                                </td>
                                                <td class="tabbg" width="15">
                                                </td>
                                                <td class="tabbg">
                                                    <img border="0" src='/ecn.images/icons/preview_img.gif'></td>
                                                <td class="tabbg" width="5">
                                                </td>
                                                <td class="tabbg">
                                                    <asp:LinkButton ID="tabPreview" OnClick="showPreview" Text="" runat="server">Preview Image</asp:LinkButton></td>
                                                <td class="tabbg" width="5">
                                                </td>
                                                <td class="tabrt" width="6">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="PanelBrowse" Visible="true" runat="server">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr class="gridaltrow">
                        <td align="left" style="border-top: #B6BCC6 1px solid; border-left: #B6BCC6 1px solid;
                            padding-left: 7px; padding-top: 4px" valign="top" height="100%" width="20%">
                            <table class="tableContent" align="center" border="0" cellspacing="0" height="100%"
                                align="left">
                                <asp:Label ID="HomeImageURL" runat="server"></asp:Label>
                                <asp:Label ID="HomeFolderName" runat="server"></asp:Label>
                                <asp:Label ID="SelectedImageURL" runat="server"></asp:Label>
                                <asp:Label ID="SelectedFolderName" runat="server"></asp:Label>
                            </table>
                        </td>
                        <td align="left" style="border-top: #B6BCC6 1px solid; border-right: #B6BCC6 1px solid;
                            padding-left: 7px; padding-top: 4px" valign="bottom" width="80%">
                            &nbsp;
                            <asp:DataList ID="foldersrepeater" runat="server" CellSpacing="0" CellPadding="0"
                                GridLines="Both" RepeatLayout="table" RepeatDirection="Horizontal" RepeatColumns="5"
                                ItemStyle-VerticalAlign="Bottom">
                                <ItemTemplate>
                                    <table align="left" class="tableContent" border="0" cellspacing="0" width="100%">
                                        <%#DataBinder.Eval(Container.DataItem,"ImageURL")%>
                                        <%#DataBinder.Eval(Container.DataItem,"FolderName")%>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="border-bottom: #B6BCC6 1px solid; border-left: #B6BCC6 1px solid;
                            border-right: #B6BCC6 1px solid;" colspan="2">
                            <asp:TextBox ID="imagepath" runat="server" Visible="False"></asp:TextBox>
                            <asp:TextBox ID="imagesize" runat="server" Visible="False">100</asp:TextBox>
                            <asp:DataList ID="imagerepeater" runat="server" CellSpacing="0" CellPadding="4" GridLines="Both"
                                BorderWidth="1" BorderColor="black" RepeatLayout="Table">
                                <ItemTemplate>
                                    <table class="tableContent" align="center">
                                        <tr>
                                            <td valign="center" align="center" height="<%=thumbnailSize%>">
                                                <asp:ImageButton runat="server" ID="BrowseImage" OnCommand="previewImage" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Path")%>'
                                                    ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Image")%>'></asp:ImageButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center">
                                                <asp:LinkButton runat="server" ID="BrowseImageLink" OnCommand="previewImage" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"Path")%>'
                                                    Text='<%#DataBinder.Eval(Container.DataItem,"FileName")%>'></asp:LinkButton>
                                                <br>
                                                <%#DataBinder.Eval(Container.DataItem,"Size")%>
                                                -
                                                <%#DataBinder.Eval(Container.DataItem,"Date")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="PanelPreview" Visible="false" runat="server">
        <tr>
            <td colspan="2">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="grd_panel_borders">
                            <table width="100%" cellpadding="10">
                                <tr>
                                    <td colspan="2">
                                        <table width="100%">
                                            <tr>
                                                <td width="33%" class="tableContent" align="left">
                                                    <asp:HyperLink runat="server" ID="fullsizeLink" Text="full size image" Target="_blank"></asp:HyperLink>
                                                </td>
                                                <td width="33%" class="tableContent" align="center">
                                                    <asp:LinkButton runat="server" ID="deleteLink" OnCommand="deleteImage" Text="delete image"></asp:LinkButton>
                                                </td>
                                                <td width="33%" class="tableContent" align="right">
                                                    <a href="?imagefile=">back</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="300" width="300" align="center" valign="top" class="tableContent">
                                        <asp:Image ID="ImagePreview" runat="server"></asp:Image>
                                    </td>
                                    <td valign="top" class="tableContent">
                                        <br>
                                        Name:
                                        <asp:Label ID="previewName" runat="server" Text="Name"></asp:Label>
                                        <br>
                                        Size:
                                        <asp:Label ID="previewSize" runat="server" Text="Size"></asp:Label>
                                        <br>
                                        Date:
                                        <asp:Label ID="previewDate" runat="server" Text="Date"></asp:Label>
                                        <br>
                                        Height:
                                        <asp:Label ID="previewHeight" runat="server" Text="Height"></asp:Label>
                                        <br>
                                        Width:
                                        <asp:Label ID="previewWidth" runat="server" Text="Width"></asp:Label>
                                        <br>
                                        Resolution:
                                        <asp:Label ID="previewResolution" runat="server" Text="Width"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="PanelFolders" Visible="false" runat="server">
        <tr>
            <td colspan="2">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="center" class="grd_panel_borders">
                            <br>
                            Folders list.
                            <br>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
    <asp:Panel ID="PanelUpload" Visible="false" runat="server">
        <tr>
            <td colspan="2">
                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td align="center" class="grd_panel_borders">
                            <br>
                            <ecn:uploader ID="pageuploader" runat="server" uploadDirectory="e:\\http\\dev\\ECNblaster\\assets\\eblaster\\customers\\1\\images\\">
                            </ecn:uploader>
                            <br>
                            <br>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>
</table>

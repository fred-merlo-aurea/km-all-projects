<%@ Page Language="c#" Inherits="ecn.creator.media.filemanager" CodeBehind="default.aspx.cs" MasterPageFile="~/Creator.Master" %>
<%@ MasterType VirtualPath="~/Creator.Master" %>
<%@ Register TagPrefix="ecn" TagName="gallery" Src="../../includes/gallery.ascx" %>
<%@ Register TagPrefix="ecn" TagName="footer" Src="../../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../../includes/header.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader" colspan="2">&nbsp;Media</td>
        </tr>
        <tr>
            <td class="tableHeader" align='right'>&nbsp;</td>
            <td align="left">
                <ecn:gallery ID="maingallery" runat="Server" thumbnailSize="100" imagesPerColumn="3"
                    borderWidth="0"></ecn:gallery>
            </td>
        </tr>
    </table>
</asp:Content>


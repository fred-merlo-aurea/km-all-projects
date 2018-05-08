<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="folderSystem.ascx.cs" Inherits="ecn.editor.ckeditor.controls.folderSystem" %>
<table style="border:none;" >
    <tr>
        <td style="vertical-align:bottom; background-color:#FFFFFF;">
            <asp:TreeView ID="trvFolders" ShowLines="true" runat="server" ExpandDepth="1"
                SelectedNodeStyle-ForeColor="darkblue" SelectedNodeStyle-BackColor="yellow" 
                onselectednodechanged="trvFolders_SelectedNodeChanged">

            </asp:TreeView>
        </td>
    </tr>
</table>
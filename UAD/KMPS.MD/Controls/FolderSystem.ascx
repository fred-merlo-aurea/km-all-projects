<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FolderSystem.ascx.cs" Inherits="KMPS.MD.Controls.FolderSystem" %>
<table style="border:none;" >
    <tr>
        <td style="vertical-align:bottom; background-color:#FFFFFF;">
            <asp:TreeView ID="trvFolders" ShowLines="true" runat="server" ExpandDepth="1"
                SelectedNodeStyle-BackColor="#bbeaf3" 
                onselectednodechanged="trvFolders_SelectedNodeChanged" ForeColor="Black">
            </asp:TreeView>
        </td>
    </tr>
</table>

<%@ Control Language="c#" Inherits="ecn.creator.includes.folderSystem" Codebehind="folderSystem.ascx.cs" %>
<table border="0">
    <tr>
        <td valign="bottom" bgcolor="ffffff">
            <asp:TreeView ID="trvFolders" ShowLines="true" ExpandDepth="1" runat="server" SelectedNodeStyle-ForeColor="darkblue" SelectedNodeStyle-BackColor="yellow" onselectednodechanged="trvFolders_SelectedNodeChanged"></asp:TreeView>
        </td>
    </tr>
</table>

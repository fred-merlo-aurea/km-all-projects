<%@ Control Language="c#" Inherits="ecn.communicator.includes.folderSystem" Codebehind="folderSystem.ascx.cs" %>
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

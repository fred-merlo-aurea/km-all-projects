<%@ Control Language="c#" Inherits="ecn.creator.includes.uploader" Codebehind="uploader.ascx.cs" %>
<table border="0">
    <tr>
        <td>
            <input class="formfield" id="FindFile" style="width: 274px; height: 22px" type="file"
                size="26" runat="server">
        </td>
    </tr>
    <tr>
        <td>
            <asp:ListBox ID="FilesListBox" runat="server" CssClass="formfield" Height="100px"
                Width="274px" Font-Size="XX-Small"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="AddFile" runat="server" CssClass="formbutton" Height="23px" Width="72px"
                Text="Add" OnClick="AddFile_Click"></asp:Button>
            <asp:Button ID="RemvFile" runat="server" CssClass="formbutton" Height="23px" Width="72px"
                Text="Remove" OnClick="RemvFile_Click"></asp:Button>
            <input class="formbutton" id="Upload" style="width: 71px; height: 24px" type="submit"
                value="Upload" runat="server" onserverclick="Upload_ServerClick">
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="MessageLabel" runat="server" Height="25px" Width="249px" class="tableContent"></asp:Label>
            <asp:TextBox ID="uploadpath" runat="server" Visible="False"></asp:TextBox>
        </td>
    </tr>
</table>

<%@ Page Language="c#" Inherits="ecn.communicator.main.lists.download" CodeBehind="download.aspx.cs" %>

<body>
    <form runat="server">
        <table width="80%" height="80%" align="center">
            <tr>
                <td width="80%" align="center">
                    <br />

                    <br />
                    <font face="arial" size="2">Processing Complete.</font></td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <asp:Button ID="btnDownloadLink" runat="Server" OnClick="downloadFile" Text="Download" />
                   </td>
            </tr>
            <tr>
                <td align="center" width="100%">
                    <asp:Label ID="lblMessage" runat="server" Text="" />
                </td>
            </tr>
        </table>

    </form>
</body>


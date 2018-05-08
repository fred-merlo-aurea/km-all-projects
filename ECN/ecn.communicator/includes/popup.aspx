<%@ Page Language="c#" Inherits="ecn.communicator.includes.popup" Codebehind="popup.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ERROR :: OCCURED</title>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <table height="100%" cellspacing="0" cellpadding="0" width="100%" border='0'>
            <tr>
                <td>
                    <table border='0' bgcolor="#000000" cellpadding="0" height="100%" width="100%">
                        <tr>
                            <td>
                                <table border='0' cellpadding="5" width="100%" height="100%" bgcolor="#ffffff">
                                    <tr>
                                        <td>
                                            <asp:Label ID="ImgLabel" runat="Server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="MsgLabel" runat="Server" Width="400" Font-Name="verdana" Font-Size="10"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <br />
                                            <asp:Label ID="MsgFooterLabel" runat="Server" Width="500" Font-Name="verdana" Font-Size="10">Please report this Error with Details to <a href="mailto:support@teckman.com">
														support@teckman.com</a></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <br />
                                            <input type="button" value="Close" onclick="window.close()">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

<%@ Page Language="c#" Inherits="ecn.communicator.main.lists.groupsubscribePrePopScript" Codebehind="groupsubscribePrePopScript.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pre Populate smartForm Scripts</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <style type="text/css">@import url( /ecn.images/images/stylesheet.css );
</style>
</head>
<body style="margin-top: 0px; margin-left: 0px">
    <div align="center">
        <asp:Label ID="lblHeader" runat="Server"></asp:Label>
        <form id="Form1" method="post" runat="Server">

            <table style="padding-left: 0px; padding-top: 0px" cellspacing="2" cellpadding="0"
                width="580" border='0'>
                <tr>
                    <td class="tableContent">
                        Copy the following lines of code in to the web page where the Pre-Pop smartForm
                        will be hosted &amp; Pre-Populated.
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox CssClass="formtextfield" ID="PrePopWebPageScriptTXT" runat="Server"
                            TextMode="MultiLine" Rows="3" Columns="110" ReadOnly="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align='right' height="4">
                    </td>
                </tr>
                <tr>
                    <td class="tableContent">
                        The following lines of code needs to be copied in to the Email Campaign that will
                        be sent out. %%WEB_PAGE_URL_HOSTING_THE_ABOVE_SCRIPT%% should be replaced with the
                        URL of the web page where the Pre-Pop smartForm will be hosted</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox CssClass="formtextfield" ID="PrePopBlastScriptTXT" runat="Server" TextMode="MultiLine"
                            Rows="3" Columns="110" ReadOnly="true"></asp:TextBox></td>
                </tr>
                <tr>
                    <td align='right' height="19">
                        <hr size="1" color='#000000' />
                    </td>
                </tr>
                <tr>
                    <td height="3" align="center">
                        <input type="button" class="formbuttonsmall" value="Close window" onclick="window.close()" /><td>
                        </td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>

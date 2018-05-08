<%@ Page Language="c#" Inherits="ecn.publisher.main.Edition.EditionLinks" Codebehind="EditionLinks.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Digital Edition Links</title>
    <style type="text/css">@import url( /ecn.images/images/stylesheet.css );</style>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <table style="padding-left: 0px; padding-top: 0px" cellspacing="2" cellpadding="0"
            width="580" border='0'>
            <tr>
                <td class="tableContent">
                    URL (Direct link to Digital Edition):
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox class="formtextfield" ID="tbURL" runat="Server" TextMode="MultiLine"
                        Rows="3" Columns="110" ReadOnly></asp:TextBox></td>
            </tr>
            <tr>
                <td align='right' height="4">
                </td>
            </tr>
            <tr>
                <td class="tableContent">
                    URL (For Email Blast):</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox class="formtextfield" ID="tbBlastURL" runat="Server" TextMode="MultiLine"
                        Rows="3" Columns="110" ReadOnly></asp:TextBox></td>
            </tr>
            <tr>
                <td align='right' height="19">
                    <hr size="1" color='#000000'>
                </td>
            </tr>
            <tr>
                <td height="3" align="center">
                    <input type="button" class="formbuttonsmall" value="Close window" onclick="window.close()">
                    <td>
                    </td>
            </tr>
        </table>
    </form>
</body>
</html>

<%@ Page Language="c#" Inherits="ecn.digitaledition.F2F" CodeBehind="F2F.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
"http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <title>Forward to a Friend</title>
    <style type="text/css">
        BODY
        {
            font-size: 10px;
            font-family: Arial, Helvetica, sans-serif;
        }
        #f2fEmailRow td
        {
            padding-top: 30px;
        }
    </style>
    <link href="Images/stylesheet.css" rel="stylesheet" type="text/css">
</head>
<body style="background-color: #fff;">
    <form id="FriendForm" runat="server">
    <asp:Label ID="lblLoginRequired" runat="server" Visible="False"></asp:Label>
    <asp:Panel ID="pnlf2f" runat="server">
        <div align="center">
            <table cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td id="topLeft">
                        &nbsp;
                    </td>
                    <td id="topCenter">
                        &nbsp;
                    </td>
                    <td id="topRight">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td id="middleLeft">
                        &nbsp;
                    </td>
                    <td align="center" bgcolor="#FFFFFF">
                        <table cellspacing="0" cellpadding="0" width="600" border="0">
                            <tr>
                                <td valign="middle" align="center" width="50" height="50">
                                    <img src="Images/f2f-icon.gif">
                                </td>
                                <td class="fwd2FrdTitle" valign="middle" align="left" width="550">
                                    <div style="width: 100%; border-bottom: #ccc 1px solid">
                                        Forward to a Friend</div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <p class="pMarg">
                                        This feature allows you to tell a friend about this Publication. A link will automatically
                                        be sent to their email address that they can use to view this edition.</p>
                                    <p class="pMarg">
                                        All information, including Email addresses are safe and confidential. We will never
                                        sell or distribute this information to any other party. Please view our <a href="#">
                                            Privacy Policy</a> to find out more.</p>
                                </td>
                            </tr>
                        </table>
                        <table class="tablecontent" cellspacing="0" cellpadding="3" width="600" border="0">
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td valign="middle" align="right" width="150">
                                    <b>Your Name:</b>&nbsp;
                                </td>
                                <td align="left" width="150">
                                    <asp:TextBox ID="txtFrom" runat="server" MaxLength="50" CssClass="formSize"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtFrom" runat="server" CssClass="ffAlert" Font-Size="X-Small"
                                        ControlToValidate="txtFrom">>> Required</asp:RequiredFieldValidator>
                                </td>
                                <td valign="middle" align="center" rowspan="3">
                                    <table class="imgContainer" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Image ID="imgThumbnail" runat="server"></asp:Image>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="f2fEmailRow">
                                <td valign="top" align="right">
                                    <b>Your Email Address:</b>&nbsp;
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" CssClass="formSize"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" CssClass="ffAlert"
                                        Font-Size="X-Small" ControlToValidate="txtEmail">>> Required</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" CssClass="ffAlert"
                                        Font-Size="X-Small" ControlToValidate="txtEmail" ValidationExpression=".*@.*\..*"
                                        Display="Static">>> InValid Email</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Subject:</b>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSubject" runat="server" MaxLength="50" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="right">
                                    <b>Note:</b>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtcomments" runat="server" MaxLength="500" CssClass="formNote"
                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>To:</b>&nbsp;
                                </td>
                                <td align="left">
                                    Your Friend's Email Address
                                </td>
                                <td class="leftPad" align="left">
                                    Your Friend's Name
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:RegularExpressionValidator ID="valEmail1" runat="server" ControlToValidate="Email1"
                                        ValidationExpression=".*@.*\..*" Display="Static" Font-Size="X-Small" ErrorMessage="Invalid Email">>> Invalid Email</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email1" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                                <td class="leftPad">
                                    <asp:TextBox ID="Name1" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                    <asp:RegularExpressionValidator ID="valEmail2" runat="server" ControlToValidate="Email2"
                                        ValidationExpression=".*@.*\..*" Display="Static" Font-Size="X-Small" ErrorMessage="Invalid Email Address #2">>> Invalid Email</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email2" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                                <td class="leftPad">
                                    <asp:TextBox ID="Name2" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                    <asp:RegularExpressionValidator ID="valEmail3" runat="server" ControlToValidate="Email3"
                                        ValidationExpression=".*@.*\..*" Display="Static" Font-Size="X-Small" ErrorMessage="Invalid Email Address #3">>> Invalid Email</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email3" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                                <td class="leftPad">
                                    <asp:TextBox ID="Name3" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                    <asp:RegularExpressionValidator ID="valEmail4" runat="server" ControlToValidate="Email4"
                                        ValidationExpression=".*@.*\..*" Display="Static" Font-Size="X-Small" ErrorMessage="Invalid Email Address #4">>> Invalid Email</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email4" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                                <td class="leftPad">
                                    <asp:TextBox ID="Name4" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                    <asp:RegularExpressionValidator ID="valEmail5" runat="server" ControlToValidate="Email5"
                                        ValidationExpression=".*@.*\..*" Display="Static" Font-Size="X-Small" ErrorMessage="Invalid Email Address #5">>> Invalid Email</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:TextBox ID="Email5" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                                <td class="leftPad">
                                    <asp:TextBox ID="Name5" runat="server" CssClass="formSize"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left" colspan="3">
                                    <hr width="100%" color="#cccccc" size="1">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td valign="top" align="left" colspan="2">
                                    <asp:Label ID="lblError" runat="server" CssClass="style1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Button ID="EmailButton" runat="server" CssClass="formButton" Text="Send" OnClick="EmailButton_Click">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td id="middleRight">
                    </td>
                </tr>
                <tr>
                    <td id="bottomLeft">
                    </td>
                    <td id="bottomCenter">
                    </td>
                    <td id="bottomRight">
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMessage" runat="server" Visible="False">
        <table height="600" cellspacing="0" cellpadding="0" width="100%" bgcolor="#ffffff"
            border="0">
            <tr>
                <td valign="middle" align="center">
                    <table cellspacing="0" cellpadding="0" width="456" bgcolor="#ffffff" border="0">
                        <tr>
                            <td width="31">
                                <img height="30" alt="" src="images/f2f-art_01.gif" width="30">
                            </td>
                            <td width="100%">
                                <img height="30" alt="" src="images/f2f-art_02.gif" width="100%">
                            </td>
                            <td width="31">
                                <img height="30" alt="" src="images/f2f-art_03.gif" width="31">
                            </td>
                        </tr>
                        <tr>
                            <td height="100%" width="30" style="background: url(Images/f2f-art_04.gif) top right repeat-y;">
                                &nbsp;
                            </td>
                            <td valign="middle">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <table height="150" cellspacing="0" cellpadding="0" width="500" border="0">
                                                <tr>
                                                    <td valign="middle" align="center">
                                                        <p class="style1">
                                                            Thank you.<br>
                                                            <br>
                                                            Your message has been
                                                            <asp:Label ID="lblMessage" runat="server" CssClass="style1"></asp:Label>
                                                            to your friend(s).
                                                            <br>
                                                            <br>
                                                            <span class="style3"><a href="javascript:window.close();">Close this window. </a>
                                                            </span>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td height="100%" width="31" style="background: url(Images/f2f-art_06.gif) top left repeat-y;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img height="31" alt="" src="images/f2f-art_07.gif" width="30">
                            </td>
                            <td width="100%">
                                <img height="31" alt="" src="images/f2f-art_08.gif" width="100%">
                            </td>
                            <td>
                                <img height="31" alt="" src="images/f2f-art_09.gif" width="31">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>

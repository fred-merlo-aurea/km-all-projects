<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="ecn.accounts.main.customers.Notes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Notes </title>
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="http://images.ecn5.com/images/stylesheet_default.css"
        type="text/css">
    <script language="javascript" type="text/javascript">
        function closewindow(refreshparent) {
            if (refreshparent == 1)
                if (window.opener && !window.opener.closed) {
                    window.opener.location.reload();
                }
            self.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="section">
        <table width="100%" border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td colspan="2">
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="674" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
                            </tr>
                            <tr>
                                <td id="errorMiddle">
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="errorBottom">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td width="15%" valign="top" class="formLabel">
                    Notes :
                </td>
                <td width="85%" align="left">
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Columns="80" Rows="8"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="15%" valign="top" class="formLabel">
                    Billing Notes :
                </td>
                <td width="85%" align="left">
                    <asp:CheckBox ID="chkbillingnotes" runat="server" />
                </td>
            </tr>
            <asp:PlaceHolder ID="pnlForEdit" runat="server">
                <tr>
                    <td class="formLabel">
                        Updated by :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblUpdatedby" runat="server" CssClass="label10"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="formLabel">
                        Updated Date :
                    </td>
                    <td align="left">
                        <asp:Label ID="lblUpdatedDate" runat="server" CssClass="label10"></asp:Label>
                    </td>
                </tr>
            </asp:PlaceHolder>
            <tr>
                <td>
                </td>
                <td align="left">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100" TabIndex="20" OnClick="btnSave_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="100" TabIndex="20" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

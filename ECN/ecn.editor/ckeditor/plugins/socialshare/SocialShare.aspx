<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SocialShare.aspx.cs" Inherits="ecn.communicator.contentmanager.ckeditor.dialog.SocialShare" ValidateRequest="false" %>

<html>
<head>

    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <base target="_self" />
    <style type="text/css">
        body, td, input, select, textarea, button {
            font-size: 11px;
            font-family: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;
        }

        .style1 {
            width: 100%;
        }
    </style>
    <script src="/common/fck_dialog_common.js" type="text/javascript"></script>
    <link href="/common/fck_dialog_common.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

    </script>
    <script language="JavaScript" type="text/JavaScript">




        function ok() {

            var finalMedia = "";
            var _objconv = document.getElementById("hfMedia");
            finalMedia = _objconv.value.toString();
            if (finalMedia.length == 0) {
                cancel();
            } else {
                if (window.opener) {
                    if (window.opener.setValue != undefined) {
                        window.opener.setValue(finalMedia);
                    }
                }

                this.close();
            }
        }

        function cancel() {
            window.returnValue = "";
            this.close();
        }
        function getobj(id) {
            if (document.all && !document.getElementById)
                obj = eval('document.all.' + id);
            else if (document.layers)
                obj = eval('document.' + id);
            else if (document.getElementById)
                obj = document.getElementById(id);

            return obj;
        }
    </script>
    <title>Insert Social Share
    </title>

</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
    <table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table2">
        <tr>
            <td align="left" bgcolor="#FF6600">
                <div style="padding: 2px;">
                    <img src="warning.gif">
                    <span style="color: #000">Use content without User Defined Fields [UDFs] as all UDFs will be 
            redacted</span>
                </div>
            </td>
        </tr>
    </table>
    <form id="BlastForm" runat="server">
        <asp:Table ID="tblMedia" runat="server" CellSpacing="0" CellPadding="0" Width="100%" border="0">
        </asp:Table>
        <asp:HiddenField ID="hfMedia" runat="server" />
        <br />
        <br />
        <asp:Button ID="btnSave" runat="server" Text="OK"
            OnClick="btnSave_Click" />
        <input type="button" value="Cancel" onclick="cancel();">
    </form>
</body>
</html>


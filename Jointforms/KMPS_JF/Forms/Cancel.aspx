<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cancel.aspx.cs" Inherits="KMPS_JF.Forms.Cancel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/ecnHighslide.css" />
    <link rel="stylesheet" type="text/css" href="../css/ecnHighslide-styles.css" />

    <script src="../scripts/highslide/highslide-full.js" type="text/javascript"></script>

    <script src="../scripts/jQuery/jquery-1.4.3.js" type="text/javascript"></script>

    <script src="../scripts/jQuery/jquery.maskedinput.js" type="text/javascript"></script>

    <script type="text/javascript">
        hs.graphicsDir = '../scripts/highslide/graphics/';
        hs.outlineType = 'rounded-white';
        hs.allowSizeReduction = 'false';
        hs.objectLoadTime = 'after';
    </script>

    <style type="text/css">
        .labelAnswer input {
            vertical-align: middle;
        }

        .labelAnswer label {
            padding-left: 5px;
            padding-right: 10px;
            vertical-align: middle;
        }
    </style>
                <div id="divcss" runat="server">
    </div>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="container">
            <div id="innerContainer">
                <div>
                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                    <asp:Panel id="pnlContent" runat="server">
                        <table>
                            <tr>
                                <td>
                                    Your payment was not processed due to cancellation.
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hlBackToForm" runat="server" Text="Click here to go back to the subscription form" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <br />
                    <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

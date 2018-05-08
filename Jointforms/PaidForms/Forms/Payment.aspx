<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="PaidForms.Forms.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head lang="en" runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Styles/Site.css" rel="stylesheet" />

    <title>Paid Form</title>
    <script src="../Scripts/jquery-1.9.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
</head>
<body>
    <br />
    <div class="container" style="max-width: 800px; padding: 40px 20px;">
        <div id="banner">
            <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
        </div>
        <asp:Panel ID="phError" runat="Server" Visible="false" BorderColor="#A30100" BorderStyle="Solid"
            BorderWidth="1px">
            <div style="padding-left: 65px; padding-right: 5px; padding-top: 15px; padding-bottom: 5px; height: 55px; background-image: url('images/errorEx.jpg'); background-repeat: no-repeat">
                <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlContent" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

        <br />
        <!-- end container-content -->
        <div id="footer">
            <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
        </div>
        <!--end footer-->
    </div>
    <br />
</body>
</html>


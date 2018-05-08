<%@ Page Language="C#" EnableEventValidation="false"  AutoEventWireup="true"  CodeBehind="thankyou.aspx.cs" Inherits="PaidPub.thankyou" %>

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
            <br />
           
            <div id="footer">
                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
            
    </div>
    <br />
</body>
</html>

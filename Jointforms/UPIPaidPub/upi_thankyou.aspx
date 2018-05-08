<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upi_thankyou.aspx.cs" Inherits="UPIPaidPub.upi_thankyou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://www.kmpsgroup.com/subforms/kmpsMain.css" rel="stylesheet" type="text/css">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>
<body>
    <div id="container" runat="server">
        <div id="innerContainer">
            <div id="container-content">
                <div id="banner">
                    <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                    &nbsp;<br />
                    <br />
                    <p align="left">
                        <asp:PlaceHolder ID="phBody" runat="server"></asp:PlaceHolder>
                    </p>
                </div>
            </div>
        </div>
        <!-- end container-content -->
        <div id="footer">
        </div>
        <!--end footer-->
    </div>
</body>
</html>
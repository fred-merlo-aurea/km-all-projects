<%@ Page Language="C#" EnableEventValidation="false" Theme="Default" AutoEventWireup="true"
    CodeBehind="thankyou.aspx.cs" Inherits="PaidPub.thankyou" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Thank You</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">

</head>
<body>
   <form id="form1" runat="server">
    <div id="container" align="left">
        <div id="innerContainer">
            <div>
                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                <br /><br />
                <asp:PlaceHolder ID="phThankYouHTML" runat="server"></asp:PlaceHolder>
				<br /><br />
				<p><a href="JAVASCRIPT:history.go(-1)">Return to complete form</a></p>
                <br />
                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

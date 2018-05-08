<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="KMPS_JF.Forms.FAQ" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>FAQ</title>
    <link href="../CSS/styles.css" rel="stylesheet" type="text/css">
    <div id="divcss" runat="server">
    </div>
   <style>
   .minheight
   {
   	min-height: 500px;
   	}
   </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <div id="innerContainer">
            <div>
                <asp:PlaceHolder ID="phHeader" runat="server"></asp:PlaceHolder>
                <asp:Panel ID="pnlPageDesc" runat="server" Visible="true" CssClass="minheight">
                    <asp:Label ID="lblPageDesc" runat="server"></asp:Label>
                </asp:Panel>
                <BR><BR><BR><BR><BR><BR>

                <asp:PlaceHolder ID="phFooter" runat="server"></asp:PlaceHolder>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

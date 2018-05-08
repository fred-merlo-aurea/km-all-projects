<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="wattpaidpub.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="ctl00_Head1">
    <title>Error Page </title>
    <link href="http://eforms.kmpsgroup.com/jointformssetup/App_Themes/Default/Default.css"
        type="text/css" rel="stylesheet" />
</head>
<body style="background-color: #cccccc;">
 <form id="form1" runat="server">
    <div style="height:200px"></div>
    <center>
        <table class="pageborder" cellpadding="0" cellspacing="0">
           
            <tr style="height: 400px">
                <td width="5">&nbsp;</td>
                <td width="35%">
                    <img src="images/error.png" />
                </td>
                <td width="60%" valign="middle">
                    <asp:label ID="lblmessage" runat="server" Font-Bold="true" Font-Italic=true Font-Names="arial" Font-Size=Large>Error</asp:label>.
                </td>
            </tr>           
        </table>
    </center>
    </form>
  
</body>
</html>

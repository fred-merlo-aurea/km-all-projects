<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearCache.aspx.cs" Inherits="KMPS_JF.Forms.ClearCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnClearCache"  runat="server" Text="Clear Cache" 
            onclick="btnClearCache_Click" />
    </div>
    
    </form>
</body>
</html>

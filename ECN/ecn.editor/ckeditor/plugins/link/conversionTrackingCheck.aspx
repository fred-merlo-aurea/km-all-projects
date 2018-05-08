<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="conversionTrackingCheck.aspx.cs" Inherits="ecn.editor.ckeditor.plugins.link.conversionTrackingCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
   
</head>
<body>
    <form id="form1" runat="server">    
        <asp:HiddenField ID="HiddenField1" runat="server" /> 
        <script language="JavaScript" type="text/JavaScript">
            var _objconv = document.getElementById("HiddenField1");      
            window.returnValue = _objconv.value.toString();
            window.close();				
	    </script>   
    </form>
</body>
</html>

<%@ Page language="c#" Inherits="ecn.activityengines.publicPreview" Codebehind="publicPreview.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>preview</title>
		
		<link rel="stylesheet" type="text/css" href="../App_Themes/jquery.qtip.css">
        <style type="text/css">
            .qtip-content {
                font-size:20px;
                line-height:20px;
            }
        </style>
		
		
	</head>
	<body>
		<form id="FormPreview" method="post" runat="Server">
			<%--<asp:Label ID="LabelPreview" runat="Server" Text="Label"></asp:Label>--%>
            <asp:Literal ID="literalPreview" runat="server" EnableViewState="false"></asp:Literal>

		</form>
	</body>
</html>

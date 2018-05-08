<%@ Register TagPrefix="uc1" TagName="SurveyBuilder" Src="../includes/SurveyBuilder.ascx" %>

<%@ Page Language="c#" Inherits="ecn.collector.front.SurveyDefault" Codebehind="default.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Survey</title>

    <script type="text/javascript" language="javascript">
			var fv = new Array();
    </script>

    <style type="text/css">
	    HTML, BODY {MARGIN:0PX;PADDING:0PX;HEIGHT:100%}
    </style>
</head>
<body style="margin: 0px">
    <form id="frmSurvey" method="post" runat="Server">
        <uc1:SurveyBuilder ID="SurveyBuilder" runat="Server" IsViewOnly="false"></uc1:SurveyBuilder>
    </form>
</body>
</html>

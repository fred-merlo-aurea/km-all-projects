<%@ Page language="c#" Inherits="ecn.collector.front.view_survey" Codebehind="view_survey.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="SurveyBuilder" Src="../includes/SurveyBuilder.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Survey</title>
<script language=javascript>
			var fv = new Array();
</script>
		
	</head>
	<body style="MARGIN: 0px">
		<form id="frmSurvey" method="post" runat="Server">
			<uc1:SurveyBuilder id="SurveyBuilder" IsViewOnly="true" runat="Server"></uc1:SurveyBuilder>
		</form>
	</body>
</html>

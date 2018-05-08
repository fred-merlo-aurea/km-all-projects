<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Web.SessionState" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>optinHTMLPreview</title>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />

    <script language="C#" runat="Server">
        private void Page_Load(object sender, System.EventArgs e)
        {
            string optinCode = HttpContext.Current.Session["OptinHTMLCode"].ToString();
            Response.Write(optinCode);
        }
    </script>

</head>
<body ms_positioning="GridLayout">
    <form id="Form2" method="post" runat="Server">
    </form>
</body>
</html>

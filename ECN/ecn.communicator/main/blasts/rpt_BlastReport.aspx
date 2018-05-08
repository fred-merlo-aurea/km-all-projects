<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<%@ Page Language="c#" EnableTheming="False" StylesheetTheme="" Theme="" Inherits="ecn.communicator.main.blasts.rpt_BlastReport" CodeBehind="rpt_BlastReport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>rpt_BlastReport</title>
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
</head>
<body>
    <form id="Form1" method="post" runat="Server">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False"  ShowRefreshButton="false">
    </rsweb:ReportViewer>
    </form>
</body>
</html>

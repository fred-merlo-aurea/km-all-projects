<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRWIFax.aspx.cs" Inherits="CanonESubscriptionForm.forms.CRWIFax" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>"
     <cr:CrystalReportViewer ID="crv" runat="server" Width="100%" SeparatePages="False" Visible="false"
                                DisplayGroupTree="False" EnableViewState="false" EnableDrillDown="False" DisplayToolbar="False">
                            </cr:CrystalReportViewer>
    </div>
    </form>
</body>
</html>

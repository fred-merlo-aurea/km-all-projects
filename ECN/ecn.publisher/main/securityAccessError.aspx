<%@ Page Language="c#" Inherits="ecn.publisher.securityAccessError" Codebehind="securityAccessError.aspx.cs" %>

<%@ Register TagPrefix="ecn" TagName="footer" Src="../includes/footer.ascx" %>
<%@ Register TagPrefix="ecn" TagName="header" Src="../includes/header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>error</title>
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <div align="center">
            <ecn:header ID="pageheader" runat="Server" divHelpBox="" divHelpBoxTitle="Help Box"
                divContentTitle="" ecnMenu=""></ecn:header>
        </div>
        <br />
        <table id="Table1" style="width: 800px; height: 96px" cellspacing="1" cellpadding="1"
            width="800" border="1">
            <tr>
                <td style="height: 23px" class="errormsg">
                    <asp:Label ID="lblMsgTitle" runat="Server" Font-Bold="True" Font-Names="Verdana"
                        Font-Size="13px" ForeColor="Black">
							<br />&nbsp;	SECURITY VIOLATION Occured !!
							<br /><br />
							&nbsp;You are in this page because you are not Authorized to view this information.<br />&nbsp;Please contact customer service at (763) 746-2780 to resolve the problem.<br /><br /></asp:Label></td>
            </tr>
        </table>
        <br />
        <ecn:footer ID="footer" runat="Server"></ecn:footer>
    </form>
</body>
</html>

<%@ Page Language="c#" Inherits="ecn.accounts.securityAccessError" CodeBehind="securityAccessError.aspx.cs"
    Title="error" MasterPageFile="~/MasterPages/Pu.Master"%>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">      
    <br />
    <table id="Table2" style="width: 800px; height: 96px" cellspacing="1" cellpadding="1"
        width="800" border="1">
        <tr>
            <td style="height: 23px" class="errormsg">
                <asp:Label ID="lblMsgTitle" runat="Server" Font-Bold="True" Font-Names="Verdana"
                    Font-Size="13px" ForeColor="Black">
					    <br />&nbsp;	SECURITY VIOLATION Occured !!
					    <br /><br />
					    &nbsp;You are in this page because you are not Authorized to view this information.<br />&nbsp;Please contact customer service at (763) 746-2780 to resolve the problem.<br /><br /></asp:Label></td>
        </tr>
    </table>
    <br />
</asp:content>

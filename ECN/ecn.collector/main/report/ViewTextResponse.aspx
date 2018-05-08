<%@ Page Language="c#" Inherits="ecn.collector.main.report.ViewTextResponse" Codebehind="ViewTextResponse.aspx.cs" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="cr" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ViewTextResponse</title>
    <link rel="stylesheet" href="/ecn.images/images/stylesheet.css" type="text/css">
    <link rel="stylesheet" href="/ecn.images/images//stylesheet_default.css"
        type="text/css">
</head>
<body>
    <form id="Form1" method="post" runat="Server">
        <table cellpadding="5" cellspacing="0" border='0' width="100%">
            <tr>
                <td class="label" valign="top">
                    <b>Question:</b><cr:CrystalReportViewer ID="crv" runat="Server" Width="350px" Height="50px"
                        ViewStateMode="Disabled" Visible="false"></cr:CrystalReportViewer>
                </td>
            </tr>
            <tr>
                <td style="padding-bottom: 10px">
                    <asp:Label ID="lblQuestion" CssClass="label" runat="Server"></asp:Label></td>
            </tr>
            <tr>
                <td style="padding-bottom: 15px" align='right' class="label">
                    Download as&nbsp;:&nbsp;
                    <asp:ImageButton ID="lnkToPDF" runat="Server" ImageUrl="/ecn.images/images/icon-pdf.gif"
                        CausesValidation="false"></asp:ImageButton>&nbsp;
                    <asp:ImageButton ID="lnktoExl" runat="Server" ImageUrl="/ecn.images/images/icon-xls.gif"
                        CausesValidation="false"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="dgResponses" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False"
                        HorizontalAlign="left" Width="100%">
                        <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        <ItemStyle HorizontalAlign="left"></ItemStyle>
                        <AlternatingItemStyle HorizontalAlign="left" CssClass="gridaltrowWizard" />
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="40%" DataField="EmailAddress" HeaderText="Email Address">
                            </asp:BoundColumn>
                            <asp:BoundColumn ItemStyle-Width="60%" DataField="DataValue" HeaderText="Response"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <AU:PagerBuilder ID="ResponsesPager" runat="Server" ControlToPage="dgResponses" PageSize="25"
                        Width="100%" OnIndexChanged="ResponsesPager_IndexChanged">
                        <PagerStyle CssClass="gridpager"></PagerStyle>
                    </AU:PagerBuilder>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

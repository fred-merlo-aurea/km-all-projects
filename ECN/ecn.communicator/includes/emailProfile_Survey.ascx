<%@ Control Language="c#" Inherits="ecn.communicator.includes.emailProfile_Survey" Codebehind="emailProfile_Survey.ascx.cs" %>
<br>
<table style="border-right: #281163 1px solid; border-top: #281163 1px solid; border-left: #281163 1px solid;
    border-bottom: #281163 1px solid" cellspacing="2" cellpadding="2" width="770"
    align="center" border="0">
    <tr>
        <td bgcolor="#281163" colspan="3">
            <div align="center">
                <font face="Verdana" color="#ffffff" size="2"><strong>
                    <asp:Label ID="SurveyNameLabel" runat="server"></asp:Label></strong></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <div align="left">
                <font style="font-weight: bold; color: red">
                    <asp:Label ID="MessageLabel" runat="server" CssClass="errormsg" Font-Bold="True"
                        Visible="False"></asp:Label></font></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="SurveyDetailsLabel" runat="server">Label</asp:Label>
        </td>
    </tr>
</table>

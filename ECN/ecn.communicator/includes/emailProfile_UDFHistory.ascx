<%@ Control Language="c#" Inherits="ecn.communicator.includes.emailProfile_UDFHistory" Codebehind="emailProfile_UDFHistory.ascx.cs" %>
<br>
<table style="border-right: #281163 1px solid; border-top: #281163 1px solid; border-left: #281163 1px solid;
    border-bottom: #281163 1px solid" cellspacing="2" cellpadding="2" width="770"
    align="center" border="0">
    <tr>
        <td bgcolor="#281163" colspan="3">
            <div align="center">
                <font face="Verdana" color="#ffffff" size="2"><strong>
                    <asp:Label ID="UDFHistoryNameLabel" runat="server">Label</asp:Label></strong></font></div>
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
            <asp:DataGrid ID="UDFHistoryGrid" runat="server" Width="100%" AutoGenerateColumns="true"
                CssClass="grid">
                <ItemStyle HorizontalAlign="center"></ItemStyle>
                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                <AlternatingItemStyle CssClass="gridaltrow" />
            </asp:DataGrid>
        </td>
    </tr>
</table>

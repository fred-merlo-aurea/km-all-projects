<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupExportUdfSetting.ascx.cs" Inherits="ecn.communicator.main.lists.reports.GroupExportUdfSetting" %>
<table style="width:100%;">
    <tr>
        <td style="text-align:left;">
            <asp:DropDownList ID="ddlUDFSelect" OnSelectedIndexChanged="ddlUDFSelect_SelectedIndexChanged" AutoPostBack="true" runat="server">
                <asp:ListItem Value="ProfileOnly" Selected ="true">Profile Only</asp:ListItem>
                
                <asp:ListItem Value="ProfilePlusStandalone">Profile plus Standalone UDFs</asp:ListItem>
                <asp:ListItem value="ProfilePlusAllUDFs">Profile plus Standalone and Transactional UDFs</asp:ListItem>
                </asp:DropDownList>
                            <!-- removing this option for now JWelter 08292014
                <asp:ListItem value="ProfilePlusAllUDFs" Selected="true">Profile plus Standalone and Transactional UDFs</asp:ListItem>-->

        </td>
    </tr>

</table>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetIssueURL.ascx.cs" Inherits="ecn.webservices.client.WATTAPI.GetIssueURL" %>
<table>
    <tr>
        <td class="label" style="text-align: right; vertical-align: top; width: 300px;">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 80px;" rowspan="12">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton"
                OnClick="btnSubmit_Click" />
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 80px;" rowspan="12">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="430px"
                Height="280px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align: right; vertical-align: top; width: 300px;">
            <asp:Label ID="lblIssueID" runat="server" Text="Issue ID:" />
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtIssueID" runat="server" CssClass="formfield"  />
        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
    <tr>
        <td>

        </td>
    </tr>
</table>

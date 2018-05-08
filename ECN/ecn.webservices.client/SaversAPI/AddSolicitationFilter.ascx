<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddSolicitationFilter.ascx.cs" Inherits="ecn.webservices.client.SaversAPI.AddSolicitationFilter" %>
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
            <asp:Label ID="lblGroupID" runat="server" Text="Group ID:" />
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtGroupID" runat="server" CssClass="formfield" Width="268" />
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align: right; vertical-align: top; width: 300px;">
            <asp:Label ID="lblStartDate" runat="server" Text="Solicitation Start Date:" />
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtStartDate" CssClass="formfield" runat="server" Width="268" />
            <ajaxToolkit:CalendarExtender ID="ceStartDate" TargetControlID="txtStartDate" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align: right; vertical-align: top; width: 300px;">
            <asp:Label ID="lblEndDate" runat="server" Text="Solicitation End Date:" />
        </td>
        <td class="label" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtEndDate" CssClass="formfield" runat="server" Width="268" />
            <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtEndDate" />
        </td>
    </tr>
    <tr>
        <td class="label" rowspan="4" style="text-align: right; vertical-align: top; width: 300px;">
            <asp:Label ID="lblZips" Text="Zip Codes" runat="server" />
        </td>
        <td class="label" rowspan="4" style="text-align: right; vertical-align: top; width: 270px;">
            <asp:TextBox ID="txtZips" CssClass="formfield" TextMode="MultiLine" runat="server" Height="100" Width="268" />
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>

</table>

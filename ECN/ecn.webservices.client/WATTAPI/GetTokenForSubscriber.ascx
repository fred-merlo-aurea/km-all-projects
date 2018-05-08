﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetTokenForSubscriber.ascx.cs" Inherits="ecn.webservices.client.WATTAPI.GetTokenForSubscriber" %>
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
        <td class="label" style="text-align:right; vertical-align:top; width:300px;">
            <asp:Label ID="lblGroupID" runat="server" Text="GroupID:" />
        </td>
        <td class="label" style="text-align:right; vertical-align:top; width:270px;">
            <asp:TextBox ID="txtGroupID" runat="server" CssClass="formfield" Width="268" />
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align:right; vertical-align:top; width:300px;" >
            <asp:Label ID="lblIssueID" Text="Issue ID:" runat="server" />
        </td>
        <td class="label" style="text-align:right; vertical-align:top; width:300px;" >
            <asp:TextBox ID="txtIssueID" runat="server" CssClass="formfield" Width="268" />
        </td>
    </tr>
    <tr>
        <td class="label" style="text-align:right; vertical-align:top; width:300px;" >
            <asp:Label ID="lblEmailAddress" Text="Email Address:" runat="server" />
        </td>
        <td class="label" style="text-align:right; vertical-align:top; width:300px;" >
            <asp:TextBox ID="txtEmailAddress" runat="server" Width="268" CssClass="formfield" />
        </td>
    </tr>
</table>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateMessage.ascx.cs" Inherits="ecn.webservices.client.ContentServices.UpdateMessage" %>
<table cellspacing="1" cellpadding="1" width="100%" align="center">
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td align='center' class="label" valign="top" width="80" rowspan="16">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton" 
                onclick="btnSubmit_Click"/>
            </td>
        <td align='center' class="label" valign="top" width="80" rowspan="16">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="430px" 
                Height="391px"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblLayout" runat="server" Text="Layout Name:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtLayout" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblMessageID" runat="server" Text="Message ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtMessageID" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblTableBorder" runat="server" Text="Table Border:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtTableBorder" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblTemplate" runat="server" Text="Template ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtTemplate" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblAddress" runat="server" Text="Address:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAddress" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblDept" runat="server" Text="Department ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtDept" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent1" runat="server" Text="Content(1) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent1" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent2" runat="server" Text="Content(2) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent2" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent3" runat="server" Text="Content(3) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent3" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent4" runat="server" Text="Content(4) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent4" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent5" runat="server" Text="Content(5) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent5" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent6" runat="server" Text="Content(6) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent6" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent7" runat="server" Text="Content(7) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent7" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent8" runat="server" Text="Content(8) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent8" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContent9" runat="server" Text="Content(9) ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContent9" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
</table>
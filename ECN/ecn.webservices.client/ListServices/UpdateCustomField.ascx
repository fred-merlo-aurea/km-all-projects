<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateCustomField.ascx.cs" Inherits="ecn.webservices.client.ListServices.UpdateCustomField" %>
<table cellspacing="1" cellpadding="1" width="100%" align="center">
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td align='center' class="label" valign="top" width="80" rowspan="12">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton" 
                onclick="btnSubmit_Click"/>
            </td>
        <td align='center' class="label" valign="top" width="80" rowspan="12">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="430px" 
                Height="280px"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblList" runat="server" Text="List ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtList" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblUDFID" runat="server" Text="Custom Field ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtUDFID" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblField" runat="server" Text="Field Name:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtField" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtDescription" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblPublic" runat="server" Text="Is Public:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtPublic" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>    
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
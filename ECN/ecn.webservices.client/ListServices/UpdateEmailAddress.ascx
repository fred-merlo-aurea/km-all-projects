<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateEmailAddress.ascx.cs" Inherits="ecn.webservices.client.ListServices.UpdateEmailAddress" %>
<table cellspacing="1" cellpadding="1" width="100%" align="center">
    <tr>
        <td align='right' class="label" valign="top" width="120">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td align='center' class="label" valign="top" width="80" rowspan="13">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton" 
                onclick="btnSubmit_Click"/>
            </td>
        <td align='center' class="label" valign="top" width="80" rowspan="13">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="420px" 
                Height="280px"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="120">
            <asp:Label ID="lblList" runat="server" Text="List ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtList" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="120">
            <asp:Label ID="lblOldEmail" runat="server" Text="Old Email Address:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtOldEmail" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="120">
            <asp:Label ID="lblNewEmail" runat="server" Text="New Email Address:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtNewEmail" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
     <tr>
        <td>
            SmartFormID</td>
        <td>
            <asp:TextBox ID="txtSFID" runat="server" Width="268px" 
                style="margin-bottom: 0px"></asp:TextBox></td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="120">
            <asp:Label ID="lblXMLString" runat="server" Text="XML String:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270" rowspan="8">
            <asp:TextBox ID="txtXMLString" runat="server" TextMode="MultiLine" Width="268px" 
                Height="162px" style="margin-bottom: 0px"></asp:TextBox>
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
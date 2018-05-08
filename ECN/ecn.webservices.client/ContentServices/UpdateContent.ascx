<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateContent.ascx.cs" Inherits="ecn.webservices.client.ContentServices.UpdateContent" %>
<table cellspacing="1" cellpadding="1" width="100%" align="center">
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td align='center' class="label" valign="top" width="80" rowspan="14">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton" 
                onclick="btnSubmit_Click"/>
            </td>
        <td align='center' class="label" valign="top" width="80" rowspan="14">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="430px" 
                Height="280px"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblTitle" runat="server" Text="Content Title:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtTitle" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>    
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblContentID" runat="server" Text="Content ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtContentID" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>        
        <td align='right' class="label" valign="top" width="110" rowspan="4">
            <asp:Label ID="lblContentText" runat="server" Text="Content Text:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270" rowspan="4">
            <asp:TextBox ID="txtContentText" runat="server" TextMode="MultiLine" Width="268px"
                Height="63px" style="margin-bottom: 0px"></asp:TextBox>
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
        
        <td align='right' class="label" valign="top" width="110" rowspan="5">
            <asp:Label ID="lblContentHTML" runat="server" Text="Content HTML:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270" rowspan="5">
            <asp:TextBox ID="txtContentHTML" runat="server" TextMode="MultiLine" Width="268px" 
                Height="88px" style="margin-bottom: 0px"></asp:TextBox>
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
</table>

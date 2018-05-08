<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddRegularScheduledAdvancedBlast.ascx.cs" Inherits="ecn.webservices.client.BlastServices.AddRegularScheduledAdvancedBlast" %>
<table cellspacing="1" cellpadding="1" width="100%" align="center">
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblAccessKey" runat="server" Text="Access Key:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtAccessKey" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
        <td align='center' class="label" valign="top" width="80" rowspan="15">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="formbutton" 
                onclick="btnSubmit_Click"/>
            </td>
        <td align='center' class="label" valign="top" width="80" rowspan="15">
            <asp:TextBox ID="txtReturn" runat="server" TextMode="MultiLine" Width="430px" 
                Height="365px"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblMessage" runat="server" Text="Message ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtMessage" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
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
            <asp:Label ID="lblDept" runat="server" Text="Department ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtDept" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblFilter" runat="server" Text="Filter ID:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtFilter" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblSubject" runat="server" Text="Subject:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtSubject" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblFromEmail" runat="server" Text="From Email:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtFromEmail" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblFromName" runat="server" Text="From Name:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtFromName" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblReplyEmail" runat="server" Text="Reply Email:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtReplyEmail" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblIsTest" runat="server" Text="Is Test:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtIsTest" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110">
            <asp:Label ID="lblRefBlasts" runat="server" Text="Ref Blasts:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270">
            <asp:TextBox ID="txtRefBlasts" runat="server" CssClass="formfield" Width="268"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align='right' class="label" valign="top" width="110" rowspan="4">
            <asp:Label ID="lblXMLSchedule" runat="server" Text="XML Schedule:"></asp:Label>
        </td>
        <td align='left' class="label" valign="top" width="270" rowspan="4">
            <asp:TextBox ID="txtXMLSchedule" runat="server" TextMode="MultiLine" Width="268px"
                Height="100px" style="margin-bottom: 0px"></asp:TextBox>
        </td>
    </tr>    
</table>
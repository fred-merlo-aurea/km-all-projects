<%--<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Unsubscribe.ascx.cs" Inherits="ecn.accounts.main.customers.LandingPage.Unsubscribe" %>
<table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0' align="left">
    <tr>
        <td class="tableHeader" valign="top" align='left' width="30%">
            <asp:CheckBox ID="cbPageLabel" runat="server" Text="Enable Page Label" OnCheckedChanged="cbPageLabel_CheckedChanged" />
        </td>
        <td align="left" width="70%">
            <asp:TextBox ID="txtPageLabel" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='left'>
            <asp:CheckBox ID="cbMainLabel" runat="server" Text="Enable Main Label" OnCheckedChanged="cbMainLabel_CheckedChanged" />
        </td>
        <td align="left">
            <asp:TextBox ID="txtMainLabel" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='left'>
            <asp:CheckBox ID="cbGroupLabel" runat="server" Checked="true" Enabled="false" Text="Enable Group Checkbox" />
        </td>
        <td align="left">
            <asp:TextBox ID="txtGroupLabel" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='left'>
            <asp:CheckBox ID="cbMSLabel" runat="server" Text="Enable Master Suppression Checkbox" OnCheckedChanged="cbMSLabel_CheckedChanged" />
        </td>
        <td align="left">
            <asp:TextBox ID="txtMSLabel" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='left'>
            Unsubscribe Reason 
            <asp:RadioButtonList ID="rblReason" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblReason_SelectedIndexChanged">
                <asp:ListItem Selected="True">Use Drop Down List</asp:ListItem>
                <asp:ListItem>Use Text Box</asp:ListItem>
                <asp:ListItem>Don&#39;t Display Reason</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td align="left">
            <asp:TextBox ID="txtReason" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" valign="top" align='left'>
            <asp:CheckBox ID="cbThankYou" runat="server" Text="Enable Thank You Label" OnCheckedChanged="cbThankYou_CheckedChanged" />
        </td>
        <td align="left">
            <asp:TextBox ID="txtThankYou" runat="server" Width="600px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" align="center" colspan='3'>
            <asp:Button class="formbutton" ID="btnSave" OnClick="btnSave_Click" runat="Server"
                Visible="true" Text="Create"></asp:Button>
        </td>
    </tr>
</table>--%>

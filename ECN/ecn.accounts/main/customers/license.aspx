<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveDateTime" %>

<%@ Page Language="c#" Inherits="ecn.accounts.main.customers.license" CodeBehind="license.aspx.cs"
    MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tableHeader" valign="top" align='right' width="20%">
                Base Channel&nbsp;
            </td>
            <td align="left">
                <asp:DropDownList EnableViewState="true" ID="BaseChannelList" runat="Server" DataValueField="BaseChannelID"
                    DataTextField="BaseChannelName" class="formfield" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align='right'>
                Channel&nbsp;
            </td>
            <td align="left">
                <asp:DropDownList EnableViewState="true" ID="ChannelList" runat="Server" DataValueField="channelID"
                    DataTextField="channelName" class="formfield" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align='right'>
                Customer&nbsp;
            </td>
            <td align="left">
                <asp:DropDownList EnableViewState="true" ID="CustomerID" runat="Server" DataValueField="CustomerID"
                    DataTextField="CustomerName" class="formfield" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align='right'>
                Type&nbsp;
            </td>
            <td align="left">
                <asp:DropDownList EnableViewState="true" ID="Dropdownlist1" runat="Server" DataValueField="CustomerID"
                    DataTextField="CustomerName" class="formfield" Visible="true" />
            </td>
        </tr>
    </table>
</asp:Content>

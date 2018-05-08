<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls.HtmlTextBox" Assembly="ActiveUp.WebControls.HtmlTextBox" %>

<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.accounts.channelsmanager.channelseditor"
    CodeBehind="channelseditor.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td colspan="2">
                    <br />
                    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                        <table cellspacing="0" cellpadding="0" width="674" align="center">
                            <tr>
                                <td id="errorTop">
                                </td>
                            </tr>
                            <tr>
                                <td id="errorMiddle">
                                    <table height="67" width="80%">
                                        <tr>
                                            <td valign="top" align="center" width="20%">
                                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                            </td>
                                            <td valign="middle" align="left" width="80%" height="100%">
                                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td id="errorBottom">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="tableHeader" align="left">
                    &nbsp;Channel
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Name&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox EnableViewState="true" ID="tbChannelName" runat="Server" size="50" ReadOnly
                        CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    ChannelURL
                </td>
                <td align="left">
                    <asp:TextBox ID="tbChannelURL" runat="Server" size="50" ReadOnly CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    VirtualPath
                </td>
                <td align="left">
                    <asp:TextBox ID="tbVirtualPath" runat="Server" size="25" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    AssetsPath
                </td>
                <td align="left">
                    <asp:TextBox ID="tbAssetsPath" runat="Server" size="35" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    PickupPath
                </td>
                <td align="left">
                    <asp:TextBox ID="tbPickupPath" runat="Server" size="35" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    MailingIP
                </td>
                <td align="left">
                    <asp:TextBox ID="tbMailingIP" runat="Server" size="35" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    BounceDomain
                </td>
                <td align="left">
                    <asp:TextBox ID="tbBounceDomain" runat="Server" size="30" value="bounce2.com" ReadOnly
                        CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;Header&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="tbHeaderSource" runat="Server" Wrap="false" Columns="90" Rows="15"
                        TextMode="multiline" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;Footer&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="tbFooterSource" runat="Server" Wrap="false" Columns="90" Rows="15"
                        TextMode="multiline" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" colspan='2' align="center">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="ChannelID" runat="Server"
                        CssClass="formfield"></asp:TextBox>
                    <asp:Button ID="btnSave" OnClick="btnSave_Click" Visible="true" Text="Create" class="formbutton"
                        runat="Server" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

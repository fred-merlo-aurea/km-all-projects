<%@ Page Language="c#" AutoEventWireup="True" Inherits="ecn.publisher.error" Codebehind="error.aspx.cs"
    EnableViewState="false" MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table id="errorTable" style="width: 800px;" cellspacing="1" cellpadding="1" width="800"
            border='0'>
            <tr>
                <td style="height: 23px">
                    <asp:Image ID="errorMsgImage" runat="Server"></asp:Image></td>
            </tr>
            <asp:Panel ID="errMsgWithSCD_Pnl" runat="Server" Visible="False">
                <tr>
                    <td style="height: 23px; font-size: 12px;" align="center">
                        <b>
                            <asp:Label ID="lblMsgWithSCDReason" runat="Server"></asp:Label></b><br />
                        Please verify that your desired URL is correct. <a href="mailto:accountmanagers@knowledgemarketing.com&subject=Need Help">
                            Click here</a> to contact your Account Manager for furtner assistance
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="errMsgWithOutSCD_Pnl" runat="Server" Visible="False">
                <tr>
                    <td style="height: 23px; font-size: 12px;" align="center">
                        <b>
                            <asp:Label ID="lblMsgWithOutSCDReason" runat="Server"></asp:Label></b><br />
                        This error has been logged. If this is an emergency, please call your Account Manager
                        at (866) 844-6275 [or] <a href="mailto:accountmanagers@knowledgemarketing.com&subject=Need Help">
                            Click here</a> to send an email.
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td>
                    <asp:TextBox ID="txtErr" runat="Server" TextMode="MultiLine" Visible="false" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
</asp:Content>

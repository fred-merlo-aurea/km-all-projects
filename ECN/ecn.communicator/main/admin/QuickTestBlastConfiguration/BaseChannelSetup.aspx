<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="BaseChannelSetup.aspx.cs" Inherits="ecn.communicator.main.admin.QuickTestBlastConfiguration.BaseChannelSetup" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table style="height:67px; width:80%" >
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
            <asp:Panel runat="server" ID="pnlNoAccess">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>
            <br />
            <asp:Panel runat="server" ID="pnlSettings">
                <table id="layoutWrapper" cellpadding="0" cellspacing="0" width="100%" border='0'>
                    <tr>
                        <td>
                            <table style="width:100%;margin-bottom: 30px;">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblHeading" runat="server" Text="Blast Configuration Base Channel Setup" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:40%;">Do you want to override default settings?
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblOverrideDefaultSettings" runat="server" RepeatDirection="Horizontal" onchange="disableButton()">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Do you want to allow Customers to override Basechannel Settings?
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rblAllowCustomerOverrideSettings" runat="server" RepeatDirection="Horizontal" onchange="disableButton()">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 100%;margin-bottom: 30px;">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label2" runat="server" Text="Quick Test Blasts" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbAllowAdhocEmails" runat="server" Text="Allow Adhoc Emails" AutoPostBack="true" OnCheckedChanged="cbAllowAdhocEmails_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:CheckBox ID="cbAutoCreateGroup" runat="server" Text="Auto-Create Group" AutoPostBack="true" OnCheckedChanged="cbAutoCreateGroup_CheckedChanged"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px;">
                                        <asp:CheckBox ID="cbAutoArchiveGroup" runat="server" Text="Auto-Archive Group" AutoPostBack="true" OnCheckedChanged="cbAutoArchiveGroup_CheckedChanged"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="text-align:center; height:40px;">
                        <td>
                            <asp:Button ID="btSave" runat="server" Text="Save" OnClick="btnSave_onclick" />
                            <asp:Button ID="btCancel" runat="server" Text="Cancel" OnClick="btnCancel_onclick" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

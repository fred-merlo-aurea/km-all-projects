<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="BaseChannelUpdateEmail.aspx.cs" Inherits="ecn.communicator.main.admin.LandingPages.BaseChannelUpdateEmail" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
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
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <asp:Panel runat="server" ID="pnlNoAccess">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>
    <asp:Panel runat="server" ID="pnlSettings" CssClass="label">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10" Visible="true" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                    <ProgressTemplate>
                        <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                            <asp:Panel ID="Panel2" CssClass="loader" runat="server">
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
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width:90%;" cellspacing="10">
                            <tr>
                                <td style="width:15%;">

                                </td>
                                <td style="width:85%;text-align:left;">
                                    <asp:Label ID="lblPageSettings" Text="Page Settings" Font-Size="Medium" Font-Bold="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Override default
                                </td>
                                <td style="text-align:left;">
                                    <asp:CheckBox ID="chkOverrideDefault" runat="server" Checked="false" />
                                </td>
                            </tr>
                            
                            <tr>
                                <td style="text-align:right;">
                                    Page Header
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPageHeader" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Page Footer
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPageFooter" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Page Text
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPageText" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:15%;text-align:right;">
                                                Old Email Label
                                            </td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="txtOldEmailLabel" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvOldEmailLabel" ControlToValidate="txtOldEmailLabel" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>

                                            <td style="width:15%;text-align:right;">
                                                New Email Label
                                            </td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="txtNewEmailLabel" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvNewEmailLabel" ControlToValidate="txtNewEmailLabel" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:15%;text-align:right;">
                                                Button Label
                                            </td>
                                            <td style="width:20%;">
                                                <asp:TextBox ID="txtButtonLabel" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvButtonLabel" ControlToValidate="txtButtonLabel" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>
                                            <td style="width:20%; text-align:center;">
                                                <asp:CheckBox ID="chkReEntryRequired" runat="server" OnCheckedChanged="chkReEntryRequired_CheckedChanged" Text="New Email Re-Entry Required" />
                                            </td>
                                            <td style="width:15%;text-align:right;">
                                                Re-Enter Email Label
                                            </td>
                                            <td style="width:30%;">
                                                <asp:TextBox ID="txtReEnterEmailLabel" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvReEnterEmailLabel" ControlToValidate="txtReEnterEmailLabel" Enabled="false" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    URL
                                </td>
                                <td>
                                    <asp:Label ID="lblURL" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Change Email Submission Page Text
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmationPageText" runat="server" Width="100%" TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ID="rfvConfirmationPageText" ControlToValidate="txtConfirmationPageText" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Change Email Confirmation Page Text
                                    </td>
                                <td>
                                    <asp:TextBox ID="txtFinalConfirmationText" runat="server" Width="100%" TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ID="rfvFinalConfirmationText" ControlToValidate="txtFinalConfirmationText" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                </td>
                            </tr>
                            <tr>
                                <td>

                                </td>
                                <td style="text-align:left;">
                                    <asp:Label ID="lblEmailHeading" runat="server" Font-Size="Medium" Text="Email Settings" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Email Header
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailHeader" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Email Footer
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailFooter" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;">
                                    Email Body
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailBody" runat="server" Width="100%" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                               
                                <td colspan="2">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:15%;text-align:right;">
                                                From Email
                                            </td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="txtFromEmail" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvFromEmail" ControlToValidate="txtFromEmail" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>
                                            <td style="width:15%;text-align:right;">
                                                Email Subject
                                            </td>
                                            <td style="width:35%;">
                                                <asp:TextBox ID="txtEmailSubject" Width="100%" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvEmailSubject" ControlToValidate="txtEmailSubject" runat="server" ErrorMessage="*" Display="Dynamic" ValidationGroup="Save" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width:100%; padding-top:15px;">
                                        <tr>
                                            <td style="width:33%;text-align:center;">
                                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" ValidationGroup="Save" Text="Save" />
                                            </td>
                                            <td style="width:33%;text-align:center;">
                                                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" />
                                            </td>
                                            <td style="width:33%;text-align:center;">
                                                <asp:Button ID="btnPreview" runat="server" OnClick="btnPreview_Click"  Text="Preview" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
        </asp:Panel>
</asp:Content>

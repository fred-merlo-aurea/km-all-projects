<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateEmailAddress.aspx.cs" Inherits="ecn.activityengines.engines.UpdateEmailAddress" MasterPageFile="~/MasterPages/Activity.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Activity.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
            <table cellspacing="0" cellpadding="0" style="background-color: white; width: 100%; text-align: center;">
                <tr>
                    <td id="errorTop"></td>
                </tr>
                <tr>
                    <td id="errorMiddle">
                        <table style="height: 67px; width: 80%;">
                            <tr>
                                <td style="vertical-align: top; text-align: center; width: 20%;">
                                    <img style="padding: 0 0 0 15px;" src="http://images.ecn5.com/images/errorEx.jpg" />
                                </td>
                                <td style="vertical-align: middle; text-align: left; width: 80%; height: 100%;">
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
        <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
            <section>
                <article>
                    <asp:Literal ID="litConfirmation" runat="server" />
                </article>
            </section>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server">
            <section>
                <article>
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessageText" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="text-align: right; width: 45%;">
                                            <asp:Label ID="lblOldEmail" Text="Old Email" runat="server" />
                                        </td>
                                        <td style="text-align: left; width: 55%;">
                                            <asp:TextBox ID="txtOldEmail" Width="200px" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvOldEmail" runat="server" ControlToValidate="txtOldEmail" ErrorMessage="*" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblNewEmail" Text="New Email" runat="server" />
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtNewEmail" Width="200px" runat="server" />
                                             <asp:RequiredFieldValidator ID="rfvNewEmail" runat="server" ControlToValidate="txtNewEmail" ErrorMessage="*" Display="Dynamic" />
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlReEnter" Visible="false" runat="server">
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblReEnter" Text="Re-Enter New Email" runat="server" />
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:TextBox ID="txtReEnter" Width="200px" runat="server" />
                                                 <asp:RequiredFieldValidator ID="rfvReEnter" runat="server" ControlToValidate="txtReEnter" ErrorMessage="*" Display="Dynamic" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td colspan="2" style="text-align: center;">
                                            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </article>
            </section>
        </asp:Panel>
    </asp:Content>
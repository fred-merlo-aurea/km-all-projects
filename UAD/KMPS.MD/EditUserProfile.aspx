<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="EditUserProfile.aspx.cs" Inherits="KMPS.MD.EditUserProfile" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="upEditProfile" runat="server">
        <ContentTemplate>
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
                                        <asp:Label ID="lblErrorMessagePhError" runat="Server"></asp:Label>
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
            <center>
                <div style="width: 50%; text-align: center; padding-left: 10px;">
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server">Edit User Profile</asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                        Height="100%" BorderWidth="1">
                        <table style="width: 80%;">
                            <tr>
                                <td style="text-align: left; width: 40%; vertical-align: top;"></td>
                                <td style="width: 60%;" align="left">
                                    <p style="color: red;">Note: If you make a change to your Username or Password, upon saving, you will be automatically Logged out and required to Log in again with your new credentials</p>
                                </td>
                            </tr>
                        </table>
                        <table style="width: 80%;" cellpadding="5" cellspacing="5">
                            <tr>
                                <td width="40%" align="right">UserName
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtUserName" AutoCompleteType="Disabled" autocomplete="off" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Password
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPassword" autocomplete="off" AutoCompleteType="Disabled" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">First Name
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFirstName" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvFirstName" ControlToValidate="txtFirstName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Last Name
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLastName" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvLastName" ControlToValidate="txtLastName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Email Address
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEmail" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvEmail" ControlToValidate="txtEmail" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Phone
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPhone" MaxLength="20" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Default BaseChannel
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlBaseChannel" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Default Customer
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlCustomer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Access Key</td>
                                <td align="left">
                                    <asp:Label ID="lblAccessKey" runat="server" /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="left">
                                    <asp:Button ID="btnSaveEditProfile" runat="server" Text="Save" CssClass="button" OnClick="btnSaveEditProfile_Click" ValidationGroup="EditProfile" UseSubmitBehavior="False" />
                                    &nbsp;&nbsp; 
                                <asp:Button ID="btnCancelEditProfile" runat="server" CssClass="button" OnClick="btnCancelEditProfile_Click" Text="Cancel" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

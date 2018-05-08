<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="EditUserProfile.aspx.cs" Inherits="ecn.accounts.main.users.EditUserProfile" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            <asp:Panel ID="pnlEditProfile" Width="580px" runat="server" BorderStyle="None">
                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: left; width: 50%; vertical-align: top;">
                            <h2>Edit User Profile</h2>
                        </td>
                        <td style="width: 50%;">
                            <p style="color: red;">Note: If you make a change to your Username or Password, upon saving, you will be automatically Logged out and required to Log in again with your new credentials</p>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td>UserName
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" AutoCompleteType="Disabled" autocomplete="off" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr>
                        <td>Password
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" autocomplete="off" AutoCompleteType="Disabled" Enabled="false" runat="server" />                            
                            <%--<asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />--%>
                            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" />
                        </td>
                    </tr>
                    <tr runat="server" id="newPassword" visible="false">
                        <td>New Password
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPassword" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvNewPassword" ControlToValidate="txtNewPassword" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr runat="server" id="confirmPassword" visible="false">
                        <td>Confirm Password
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" ControlToValidate="txtConfirmPassword" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr>
                        <td>First Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvFirstName" ControlToValidate="txtFirstName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr>
                        <td>Last Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvLastName" ControlToValidate="txtLastName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr>
                        <td>Email Address
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvEmail" ControlToValidate="txtEmail" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                        </td>
                    </tr>
                    <tr>
                        <td>Phone
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhone" MaxLength="20" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Default BaseChannel
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBaseChannel" OnSelectedIndexChanged="ddlBaseChannel_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Default Customer
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCustomer" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Access Key</td>
                        <td><asp:Label ID="lblAccessKey" runat="server" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table style="width:100%;">
                                <tr>
                                    <td style="text-align: center; padding-top: 20px;">
                                        <asp:Button ID="btnSaveEditProfile" runat="server" OnClick="btnSaveEditProfile_Click" ValidationGroup="EditProfile" Text="Save" />
                                    </td>
                                    <td style="text-align: center; padding-top: 20px;">
                                        <asp:Button ID="btnCancelEditProfile" runat="server"  OnClick="btnCancelEditProfile_Click" CausesValidation="false" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </td>

                    </tr>

                </table>


            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

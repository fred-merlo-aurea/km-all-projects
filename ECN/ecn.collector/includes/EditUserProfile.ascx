<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditUserProfile.ascx.cs" Inherits="ecn.collector.includes.EditUserProfile" %>
<asp:UpdatePanel ID="upEditProfile" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlEditProfile" Height="300px" Width="580px" runat="server" BorderStyle="None" CssClass="ECN-ModalPopup">
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
                        <asp:TextBox ID="txtUserName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
                    </td>
                </tr>
                <tr>
                    <td>Password
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ValidationGroup="EditProfile" runat="server" ErrorMessage="Required" />
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


            </table>


        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

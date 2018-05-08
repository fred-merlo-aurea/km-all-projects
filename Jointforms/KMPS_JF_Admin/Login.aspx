<%@ Page MasterPageFile="~/MasterPages/Login.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="KMPS_JF_Setup._Login" Title="KMPS Form Builder - Login" %>

<%@ MasterType VirtualPath="~/MasterPages/Login.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="40%" cellpadding="5" cellspacing="0" border="0" style="border: solid 0px black">
                <tr>
                    <td style="text-align: right;">
                        User Name :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtUserName" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqfldUserName" runat="server" ControlToValidate="txtUserName"
                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Password :
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtPassword" runat="server" Width="150px" TextMode="Password" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqfldPassword" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="*" Font-Bold="false"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button" OnClick="btnLogin_Click" />
                        <asp:SqlDataSource ID="SqlDataSourcePLoginConnect" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>"
                            SelectCommand="sp_UserLogin" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter Name="UserName" ControlID="txtUserName" DefaultValue="0" Type="String" />
                                <asp:ControlParameter Name="Password" ControlID="txtPassword" DefaultValue="0" Type="String" />
                                <asp:Parameter Name="iMod" Type="Int32" DefaultValue="4" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

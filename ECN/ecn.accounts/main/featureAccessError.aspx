<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="featureAccessError.aspx.cs" Inherits="ecn.accounts.main.featureAccessError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                        <asp:Label ID="lblErrorMessagePhError" runat="Server">Your customer level does not allow you to use this functionality</asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td id="errorBottom"></td>
                    </tr>
                </table>
</asp:Content>

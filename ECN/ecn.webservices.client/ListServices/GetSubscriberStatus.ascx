<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetSubscriberStatus.ascx.cs"
    Inherits="ecn.webservices.client.ListServices.GetSubscriberStatus" %>
<table>
    <tr>
        <td valign="top">
            <table border="0" width="100%">
                <tr>
                    <td>
                        Email Address:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailAddress" runat="server" Width="250" ValidationGroup="valgrpSubscriberStatus" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                            ControlToValidate="txtEmailAddress" ValidationGroup="valgrpSubscriberStatus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Access Key:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccessKey" runat="server" Width="250" ValidationGroup="valgrpSubscriberStatus" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                            ControlToValidate="txtAccessKey" ValidationGroup="valgrpSubscriberStatus"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            ValidationGroup="valgrpSubscriberStatus" />
                    </td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <asp:TextBox ID="txtOutputXml" runat="server" TextMode="MultiLine" Width="500" Height="400"
                ReadOnly="true" />
        </td>
    </tr>
</table>

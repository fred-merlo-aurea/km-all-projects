<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardPreview_SMS.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardPreview_SMS" %>


<fieldset>
    <legend>
        <table>
            <tr>
                <td>
                    Message
                </td>
            </tr>
        </table>
    </legend>
    Message Name: &nbsp&nbsp
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

<br />
<br />
 Auto Welcome Message: &nbsp&nbsp
    <asp:Label ID="lblWelcome" runat="server" Text=""></asp:Label>
</fieldset>

<fieldset>
    <legend>
        <table>
            <tr>
                <td>
                    Group
                </td>
            </tr>
        </table>
    </legend>
    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 30px">
        <tr>
            <td class="formLabel">
                <asp:Repeater ID="rpterGroupDetails" runat="server">
                    <HeaderTemplate>
                        <table>
                            <tr>
                                <td>
                                    Group Name
                                </td>
                                <td>
                                    Filter Name
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "GroupName")%>
                            </td>
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "FilterName")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
</fieldset>

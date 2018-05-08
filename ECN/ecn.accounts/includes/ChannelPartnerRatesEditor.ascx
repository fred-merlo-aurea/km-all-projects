<%@ Control Language="c#" Inherits="ecn.accounts.includes.ChannelPartnerRatesEditor" Codebehind="ChannelPartnerRatesEditor.ascx.cs" %>
<table class="tableContent" width="100%">
    <tr>
        <td>
            <asp:Label ID="lblTitle" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="20">
                        &nbsp;</td>
                    <td width="50">
                        &nbsp;</td>
                    <td width="20">
                        &nbsp;</td>
                    <td width="50">
                        &nbsp;</td>
                    <td width="30">
                        Rate</td>
                    <td width="80">
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="20">
                        From:</td>
                    <td width="50">
                        <asp:TextBox ID="txtNewLowerBoundary" runat="server"></asp:TextBox></td>
                    <td width="20">
                        To:</td>
                    <td width="50">
                        <asp:TextBox ID="txtNewUppderBoundary" runat="server"></asp:TextBox></td>
                    <td width="30">
                        <asp:TextBox ID="txtNewRate" runat="server"></asp:TextBox>
                    </td>
                    <td width="80">
                        <asp:LinkButton ID="btnNew" runat="server">Add</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DataList runat="server" Width="100%" ID="dltRates">
                <ItemTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="20">
                                From:</td>
                            <td width="50">
                                <%# DataBinder.Eval(Container.DataItem, "LowerBoundary") %>
                            </td>
                            <td width="20">
                                To:</td>
                            <td width="50">
                                <%# DataBinder.Eval(Container.DataItem, "UppderBoundary") %>
                            </td>
                            <td width="30">
                                <%# DataBinder.Eval(Container.DataItem, "Rate") %>
                            </td>
                            <td width="80">
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="20">
                                From:</td>
                            <td width="50">
                                <asp:TextBox ID="txtLowerBoundary" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LowerBoundary") %>'></asp:TextBox>
                            </td>
                            <td width="20">
                                To:</td>
                            <td width="50">
                                <asp:TextBox ID="txtUpperBoundary" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UppderBoundary") %>'></asp:TextBox>
                            </td>
                            <td width="30">
                                <asp:TextBox ID="txtRate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rate") %>'></asp:TextBox>
                            </td>
                            <td width="80">
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td width="20">
                        Default:</td>
                    <td width="50">
                        <asp:TextBox ID="txtDefaultRate" runat="server"></asp:TextBox></td>
                    <td width="20">
                    </td>
                    <td width="50">
                    </td>
                    <td width="30">
                    </td>
                    <td width="80">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
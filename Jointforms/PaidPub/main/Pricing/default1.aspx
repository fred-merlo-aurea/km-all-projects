<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default1.aspx.cs"
    Inherits="PaidPub.main.Pricing._default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div class="contentheader">
        Combo Pricing Discounts
    </div>
    <br />
    <div class="padding20">
        <div class="box">
            <div class="boxheader">
                Discounts</div>
            <div class="boxcontent">
                <table cellpadding="5" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td width="10%" align="left">
                            &nbsp;
                        </td>
                        <td width="30%" align="left">
                            1 year
                        </td>
                        <td width="30%" align="left">
                            2 year
                        </td>
                        <td width="30%" align="left">
                            3 year
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            2
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo2yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo2yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo2yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            3
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo3yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo3yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo3yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            4
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo4yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo4yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo4yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            5
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo5yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo5yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo5yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            6
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo6yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo6yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo6yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            7
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo7yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo7yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo7yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            8
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo8yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo8yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo8yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            9
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo9yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo9yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo9yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            10+
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo10yr1" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo10yr2" runat="server" Width="50"></asp:TextBox>%
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCombo10yr3" runat="server" Width="50"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <br />
                            <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="error"></asp:Label>
                            <br />
                            <asp:Button CssClass="blackButton" ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click">
                            </asp:Button>&nbsp;
                            <asp:Button CssClass="blackButton" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                OnClick="btnCancel_Click"></asp:Button>
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

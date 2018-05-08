<%@ Page Language="c#" CodeBehind="FlashReport.aspx.cs" AutoEventWireup="false" Inherits="ecn.communicator.main.lists.reports.FlashReport"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <table width="770" align="center" border="0">
        <tr>
            <td class="tableHeader1" valign="top" align="center" width="15%">
                From Date
            </td>
            <td class="tableHeader1" valign="top" align="center" width="15%">
                To Date
            </td>
            <td class="tableHeader1" valign="top" align="center" width="35%">
                Publication
            </td>
            <td class="tableHeader1" valign="top" align="center" width="25%">
                Promotion Code
            </td>
        </tr>
        <tr>
            <td colspan="4" height="3">
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="center" width="15%">
                <asp:TextBox class="formfield" ID="FromDate" runat="server" Width="70" MaxLength="10"></asp:TextBox><br>
                <font color="#ff0000" size="1">MM/DD/YYYY</font>
            </td>
            <td class="tableHeader" valign="top" align="center" width="15%">
                <asp:TextBox class="formfield" ID="ToDate" runat="server" Width="70" MaxLength="10"></asp:TextBox><br>
                <font color="#ff0000" size="1">MM/DD/YYYY</font>
            </td>
            <td class="tableHeader" valign="top" align="center" width="35%">
                <asp:DropDownList class="formfield" ID="PubGroupID" runat="server" DataTextField="GroupName"
                    DataValueField="GroupID">
                </asp:DropDownList>
            </td>
            <td class="tableHeader" valign="top" align="center" width="25%">
                <asp:TextBox class="formfield" ID="PromoCode" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4" height="3">
                <hr size="1" color="#000000">
            </td>
        </tr>
        <tr>
            <td colspan="4" height="3" align="center">
                <asp:Button ID="SubmitBtn" runat="server" Text="Show Results" class="formfield">
                </asp:Button>
            </td>
        </tr>
        <tr>
            <td colspan="4" height="7">
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:DataGrid ID="ResultsGrid" runat="server" HorizontalAlign="Center" AutoGenerateColumns="false"
                    Width="75%" BackColor="#eeeeee">
                    <ItemStyle CssClass="tableContent" Height="22"></ItemStyle>
                    <HeaderStyle CssClass="tableHeader1" HorizontalAlign="Center"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundColumn ItemStyle-Width="33%" DataField="PromoCode" HeaderText="Promotion Code"
                            ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="33%" DataField="UniqueEmails" HeaderText="Unique Emails"
                            ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                        <asp:BoundColumn ItemStyle-Width="33%" DataField="TotalEmails" HeaderText="Total Emails"
                            ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                    </Columns>
                    <AlternatingItemStyle BackColor="White" />
                </asp:DataGrid>
            </td>
        </tr>
    </table>
</asp:Content>

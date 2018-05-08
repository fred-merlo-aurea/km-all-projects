<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTemplate.aspx.cs"
    EnableEventValidation="false" Inherits="ecn.publisher.main.Edition.SelectTemplate"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop">
                </td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table height="67" width="80%">
                        <tr>
                            <td valign="top" align="center" width="20%">
                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                            </td>
                            <td valign="middle" align="left" width="80%" height="100%">
                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="errorBottom">
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td class="tablecontent" valign="middle" align='center' height="30">
                <asp:GridView ID="grdDETemplate" AllowSorting="True" AllowPaging="False" runat="server"
                    ShowHeader="false" GridLines="none" AutoGenerateColumns="False" Width="80%" EmptyDataText="No Templates Found."
                    OnRowDataBound="grdDETemplate_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Template" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="80%" ItemStyle-Width="80%">
                            <ItemTemplate>
                                <asp:Label ID="lblTemplate" runat="server" Text='<%# Eval("Html") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Template" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="20%" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Button ID="btnSelect" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="create"
                                    Text="select" OnCommand="btnSelect_Command" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

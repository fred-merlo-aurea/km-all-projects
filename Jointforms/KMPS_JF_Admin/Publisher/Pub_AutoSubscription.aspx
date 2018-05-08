<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="Pub_AutoSubscription.aspx.cs" Inherits="KMPS_JF_Setup.Publisher.Pub_AutoSubscription" %>

<%@ Register TagPrefix="JF" Namespace="KMPS_JF_Objects.Controls" Assembly="KMPS_JF_Objects" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="ContentAutoSub" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpAutoSub" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblMessage" Text="" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: left; width: 50%; padding-bottom: 20px;">
                        <b>Customer :</b>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px" AppendDataBoundItems="true"
                            DataSourceID="SqlDataSourcePCustomerConnect" DataTextField="CustomerName" DataValueField="CustomerId"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                            <asp:ListItem Text="ALL " Value="0">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourcePCustomerConnect" runat="server" ConnectionString="<%$ ConnectionStrings:ecn5_accounts %>"
                            SelectCommandType="Text"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td width="45%" valign="top">
                        <b>Available Groups</b>
                        <asp:ListBox ID="lstAvailableGroups" SelectionMode="Multiple" runat="server" Width="100%"
                            Height="500px"></asp:ListBox>
                    </td>
                    <td width="10%" align="center">
                        <asp:Button ID="btnAddSelectedGroups" runat="server" Text=">>" Width="50px" CssClass="buttonSmall"
                            OnClick="btnAddSelectedGroups_Click" /><br />
                        <asp:Button ID="btnRemoveSelectedGroups" runat="server" Text="<<" Width="50px" CssClass="buttonSmall"
                            OnClick="btnRemoveSelectedGroups_Click" />
                    </td>
                    <td width="45%" valign="top">
                        <b>Selected Groups</b>
                        <asp:ListBox ID="lstSelectedGroups" runat="server" SelectionMode="Multiple" Width="100%"
                            Height="500px"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonSmall" OnClick="btnSave_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonSmall" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

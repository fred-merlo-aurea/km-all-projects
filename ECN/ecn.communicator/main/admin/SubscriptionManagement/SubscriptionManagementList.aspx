<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SubscriptionManagementList.aspx.cs" Inherits="ecn.communicator.main.admin.SubscriptionManagement.SubscriptionManagementList" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
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
                                <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="errorBottom"></td>
            </tr>
        </table>
    </asp:PlaceHolder>
        <asp:Panel ID="pnlNoPages" runat="server" Visible="false" Height="400px">
        <div style="padding-top:150px; text-align:center;font-size:large;">
             <asp:Label ID="Label1" runat="server" Text="You do not have any subscription management pages."></asp:Label>
        <asp:HyperLink ID="hlAddPage" runat="server" NavigateUrl="/ecn.communicator/main/admin/subscriptionmanagement/SubscriptionManagementEdit.aspx" Font-Size="Large">Click Here </asp:HyperLink><asp:Label ID="Label2" runat="server" Text=" to add one."></asp:Label>        
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Visible="false">

    <table style="width:100%; padding-top:20px;">
        <tr>
            <td style="text-align:left;">
                <asp:Label ID="lblHeading" Text="Subscription Management" CssClass="Page_Title" runat="server" />
            </td>
            <td style="text-align:right;">
                <asp:Button ID="btnCreateNewSMPage" runat="server" Text="Create new page" OnClick="btnCreateNewSMPage_Click" />
            </td>
        </tr>
        <tr>
            <td style="margin-top:10px;" colspan="2">
                <ecnCustom:ecnGridView ID="gvPages" CssClass="grid" Width="100%" runat="server" OnRowDataBound="gvPages_RowDataBound" OnRowCommand="gvPages_RowCommand" AutoGenerateColumns="false" >
                    <HeaderStyle CssClass="gridheader" />
                    <FooterStyle CssClass="gridheader" />
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="Name" ItemStyle-Width="30%" HeaderText="Name" />
                        <asp:TemplateField HeaderText="URL" ItemStyle-Width="50%">
                            <ItemTemplate>
                                <asp:Label ID="lblSMURL" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEditPage" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="editpage" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDeletePage" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletepage" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ecnCustom:ecnGridView>
            </td>
        </tr>
    </table>
        
    </asp:Panel>
    
</asp:Content>

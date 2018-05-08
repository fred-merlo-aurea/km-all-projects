<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="CustomerMain.aspx.cs" Inherits="ecn.communicator.main.admin.landingpages.CustomerMain" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

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
    <table style="width:100%;">
        <tr>
            <td style="padding-top:20px;">
                <asp:Label ID="lblHeading" runat="server" Text="Landing Pages" CssClass="Page_Title" />
            </td>
        </tr>
        <tr>
            <td style="padding-top:20px;width:100%;">
                <ecnCustom:ecnGridView ID="gvLandingPage" AutoGenerateColumns="false" CssClass="grid" OnRowDataBound="gvLandingPage_RowDataBound" OnRowCommand="gvLandingPage_RowCommand" Width="100%" runat="server">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="gridheader"></FooterStyle>
                    <AlternatingRowStyle CssClass="gridaltrow"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="Landing Page" DataField="Name" ItemStyle-Width="20%" />
                        <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Width="60%" />
                        <asp:TemplateField HeaderText="Edit" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEditLandingPage" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LPID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Preview" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlBtnPreview" Target="_blank" runat="server" ImageUrl="/ecn.images/images/icon-preview-HTML.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </ecnCustom:ecnGridView>
            </td>
        </tr>
    </table>
    
</asp:Content>

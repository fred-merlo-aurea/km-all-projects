<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.contentmanager.linkAlias"
    CodeBehind="linkAlias.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
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
        <tbody>
            <tr>
                <td class="tableHeader" valign="bottom" align='right'>
                    <br />
                    <asp:Label ID="referenceLabel" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                    <asp:DataGrid ID="NewLinkAliasGrid" runat="Server" CssClass="grid" Width="100%" PageSize="9999"
                        autogenerate="true" OnItemDataBound="NewLinkAliasGrid_ItemDataBound" ItemStyle-HorizontalAlign="Left">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="bottom" align="center">
                    <br />
                    <asp:TextBox ID="TotalLinks" runat="Server" Visible="false" />
                    <asp:Button ID="SaveButton" Visible="true" Text="Save Link Alias" class="formbutton"
                        runat="Server" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>

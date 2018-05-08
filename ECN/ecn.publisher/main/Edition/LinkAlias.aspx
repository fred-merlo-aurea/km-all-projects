<%@ Page Language="c#" Inherits="ecn.publisher.main.Edition.txtAlias" CodeBehind="LinkAlias.aspx.cs"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

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
    <table cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tr>
            <td class="tableHeader" valign="top" align='right'>
                <asp:GridView ID="grdAlias" AllowPaging="true" CssClass="grid" Width="100%" AutoGenerateColumns="false"
                    runat="server" GridLines="None" DataKeyNames="LinkID" AllowSorting="false"  OnPageIndexChanging="grdAlias_PageIndexChanging">
                    <HeaderStyle CssClass="gridheader" ></HeaderStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Link/URL" ItemStyle-Width="50%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblURL" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.LinkURL") %>'
                                    MaxLength="50">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="50%" HeaderText="Link Alias"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAlias" runat="Server" Width="350px" Text='<%# DataBinder.Eval(Container, "DataItem.Alias") %>'
                                    MaxLength="50">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="bottom" align="center">
                <br />
                <asp:Button ID="btnsave" Visible="true" Text="Save Link Alias" class="formbutton"
                    runat="Server" OnClick="btnsave_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

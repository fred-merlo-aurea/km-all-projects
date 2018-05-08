<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeBehind="UserBrandSetup.aspx.cs" Inherits="KMPS.MD.Tools.UserBrandSetup" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div style="width: 90%; text-align: left; padding-left: 10px;">

                    <asp:GridView ID="gvUserBrand" runat="server" AllowPaging="True" AllowSorting="false"
                        AutoGenerateColumns="False" DataKeyNames="UserID" OnPageIndexChanging="gvUserBrand_PageIndexChanging"
                        OnRowDeleting="gvUserBrand_RowDeleting" OnRowCommand="gvUserBrand_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="80%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("UserID") + "/" +  Eval("UserName")%>'
                                        OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("UserID")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server">User Brand Setup</asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="100%" BorderWidth="1">
                        <div id="divError" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="674" align="center">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="20%">
                                                    <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="80%" height="100%">
                                                    <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        <table cellspacing="5" cellpadding="10" border="0">
                            <tr>
                                <td align="left">User Name :&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblUsername" runat="Server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="drpUsers" runat="server" CssClass="formfield" DataTextField="UserName"
                                        DataValueField="UserID" Width="200px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvusers" runat="server" ControlToValidate="drpUsers"
                                        ErrorMessage="*" ForeColor="Red" Font-Bold="true" InitialValue="0" ValidationGroup="save"></asp:RequiredFieldValidator>
                                    <asp:Label ID="lblUserID" runat="Server" Visible="false" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table cellspacing="5" cellpadding="5" border="0">
                            <asp:Panel ID="pnlBrandPermission" runat="server">
                                <tr>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Available Brands</b>
                                    </td>
                                    <td></td>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <b>Selected Brands</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstSourceFields" runat="server" Rows="20" Style="text-transform: uppercase;"
                                            DataValueField="BrandID" DataTextField="BrandName" SelectionMode="Multiple"
                                            Font-Size="X-Small" Font-Names="Arial" Width="400px"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnremove" runat="server" CssClass="button" Text="<<" OnClick="btnRemove_Click" />
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstDestFields" runat="server" Rows="20" Style="text-transform: uppercase"
                                            DataValueField="BrandID" DataTextField="BrandName" SelectionMode="Multiple"
                                            Font-Size="X-Small" Font-Names="Arial" Width="400px"></asp:ListBox>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"
                                        ValidationGroup="save" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                        CssClass="button" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

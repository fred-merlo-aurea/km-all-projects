<%--<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true"
    CodeBehind="UserSetup.aspx.cs" Inherits="KMPS.MD.Administration.UserSetup" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="ecn" %>
<%@ MasterType VirtualPath="Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divError" runat="Server" visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table width="80%">
                                <tr>
                                    <td valign="top" align="center" width="20%">
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                            alt="" />
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
                <br />
            </div>
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">User Setup</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="100%" BorderWidth="1">
                <table cellspacing="5" cellpadding="10" border="0" align="left" width="80%">
                    <tr>
                        <td align="left">User Name :&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblUsername" runat="Server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drpUsers" runat="server" CssClass="formfield" DataTextField="UserName"
                                DataValueField="UserID" Width="200px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpUsers"
                                ErrorMessage="* Please select user" InitialValue="0" ValidationGroup="save" ForeColor="Red" Font-Bold="True"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblUserID" runat="Server" Visible="false"></asp:Label>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td align="left">Permission :&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drpPermission" runat="server" CssClass="formfield" AutoPostBack="True"
                                OnSelectedIndexChanged="drpPermission_SelectedIndexChanged">
                                <asp:ListItem Value="admin">Admin</asp:ListItem>
                                <asp:ListItem Value="fullpermission">Full Permission</asp:ListItem>
                                <asp:ListItem Value="readonly">Readonly</asp:ListItem>
                                <asp:ListItem Value="no edit">No Edit</asp:ListItem>
                                <asp:ListItem Value="clientadmin">Client Admin</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSalesViewText" runat="server" Text="Show Sales View"></asp:Label>
                            &nbsp;
                            <asp:CheckBox ID="chkShowSaleView" runat="server" />
                            &nbsp;&nbsp; <asp:Label ID="lblLicenseCount" runat="server" Text=""></asp:Label>

                        </td>
                    </tr>
                    <asp:Panel ID="pnlExportPermission" runat="server">
                        <tr>
                            <td>Base Channel :
                                <asp:DropDownList ID="drpBaseChannels" runat="server" CssClass="formfield" DataTextField="BaseChannelName"
                                    DataValueField="BaseChannelId" AutoPostBack="True" Width="200px" OnSelectedIndexChanged="drpBaseChannel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                <b>Available Customers</b>
                            </td>
                            <td></td>
                            <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                <b>Selected Customers</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ListBox ID="lstSourceFields" runat="server" Rows="20" Style="text-transform: uppercase;"
                                    DataValueField="CustomerID" DataTextField="CustomerName" SelectionMode="Multiple"
                                    Font-Size="X-Small" Font-Names="Arial" Width="400px"></asp:ListBox>
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                <br />
                                <br />
                                <asp:Button ID="btnremove" runat="server" CssClass="button" Text="<<" OnClick="btnremove_Click" />
                            </td>
                            <td>
                                <asp:ListBox ID="lstDestFields" runat="server" Rows="20" Style="text-transform: uppercase"
                                    DataValueField="CustomerID" DataTextField="CustomerName" SelectionMode="Multiple"
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
            <br />
            <asp:GridView ID="gvUsers" runat="server" AllowPaging="True" AllowSorting="false"
                AutoGenerateColumns="False" DataKeyNames="UserID" OnPageIndexChanging="gvUsers_PageIndexChanging"
                OnRowDeleting="gvUsers_RowDeleting" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Permission" HeaderText="Permission" SortExpression="Permission"
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="20%" ItemStyle-Width="20%" HeaderText="ShowSalesView"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToBoolean(Eval("ShowSalesView")) == true || Eval("Permission").ToString() == "admin" ? "Yes" : "No"%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("UserID") + "/" +  Eval("UserName")%>'
                                OnCommand="lnkEdit_Command"><img src="Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DomainTrackerEdit.aspx.cs" Inherits="ecn.communicator.main.lists.DomainTrackerEdit" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: left">



        <br />
        <asp:Label ID="lblDomainTracker" runat="server" Text="Domain Tracking" Font-Bold="true" Font-Size="Medium"></asp:Label>
        <br />
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
        <asp:HiddenField ID="hfGroupID" runat="server" Value="0" />
        <br />
        <table width="100%">
            <tr>
                <td>Domain Name
                </td>
                <td colspan="6">
                    <asp:TextBox ID="txtDomainName" runat="server" Width="580"></asp:TextBox>
                    <asp:Label ID="lblDomainName" runat="server" Text="Label" Visible="false" Font-Size="Small"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Standard Data Points" Font-Size="Medium" Font-Bold="true"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="7" align="left">
                    <ul style="font-size:medium;">
                        <li style="padding-bottom:0.5em;">Email Address</li>
                        <li style="padding-bottom:0.5em;">Page URL</li>
                        <li style="padding-bottom:0.5em;">TimeStamp</li>
                        <li style="padding-bottom:0.5em;">Operating System</li>
                        <li style="padding-bottom:0.5em;">Browser Info</li>
                        <li style="padding-bottom:0.5em;">IP Address</li>
                    </ul>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Additional Data Points (Optional)" Font-Size="Medium" Font-Bold="true"></asp:Label>
                    <br />
                </td>
            </tr>
            <tr>
                <td>Data Point Name
                </td>
                <td>
                    <asp:TextBox ID="txtFieldName" runat="server" Width="150"></asp:TextBox>
                </td>
                <td>Source
                </td>
                <td>
                    <asp:DropDownList ID="drpSource" runat="server" Width="150">
                        <asp:ListItem Text="-Select-" Value="-Select-"></asp:ListItem>
                        <asp:ListItem Text="Query String" Value="QueryString"></asp:ListItem>
                        <asp:ListItem Text="Cookie" Value="Cookie"></asp:ListItem>
                        <asp:ListItem Text="HTML Control" Value="HTMLElement"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>Source ID 
                </td>
                <td>
                    <asp:TextBox ID="txtSourceID" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAddDomainTrackerFields_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <ecnCustom:ecnGridView ID="gvDomainTrackerFields" runat="server" AllowSorting="false" AutoGenerateColumns="false"
                        Width="100%" AllowPaging="false" ShowFooter="false" DataKeyNames="DomainTrackerFieldsID" CssClass="grid"
                        OnRowCommand="gvDomainTrackerFields_RowCommand">
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblDomainTrackerFieldsID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.DomainTrackerFieldsID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Data Point Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FieldName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="Source" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.Source") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderText="SourceID" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSourceID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.SourceID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnParamDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif"
                                        CommandName="FieldDelete" OnClientClick="return confirm('Are you sure, you want to delete this Field?')"
                                        CausesValidation="false" CommandArgument='<%#Eval("DomainTrackerFieldsID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ecnCustom:ecnGridView>
                </td>
            </tr>
            <tr>
                <td colspan="6" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

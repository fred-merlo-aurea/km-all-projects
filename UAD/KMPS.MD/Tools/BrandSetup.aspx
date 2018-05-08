<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeBehind="BrandSetup.aspx.cs" Inherits="KMPS.MD.Tools.BrandSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        .popitmenu {
            position: absolute;
            background-color: white;
            border: 1px solid black;
            font: normal 12px Verdana;
            line-height: 18px;
            z-index: 100;
            visibility: hidden;
        }

            .popitmenu a {
                text-decoration: none;
                padding-left: 6px;
                color: #5783BD;
                display: block;
                font-weight: bold;
            }

                .popitmenu a:hover {
                    /*hover background color*/
                    background-color: #FF7F00;
                    color: #ffffff;
                }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ModalWindow {
            border: solid 1px#c0c0c0;
            background: #ffffff;
            padding: 0px 10px 10px 10px;
            position: absolute;
            top: -1000px;
        }

        .modalPopup {
            background-color: transparent;
            padding: 1em 6px;
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
        }
    </style>

    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="btnRemoveLogo" />
        </Triggers>
        <ContentTemplate>
            <center>
                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <asp:GridView ID="gvBrand" runat="server" AllowPaging="True" AllowSorting="false"
                        AutoGenerateColumns="False" DataKeyNames="BrandID" OnPageIndexChanging="gvBrand_PageIndexChanging" OnRowDeleting="gvBrand_RowDeleting" OnRowCommand="gvBrand_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="BrandName" HeaderText="Brand Name" SortExpression="BrandName"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="90%" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("BrandID")%>'
                                        OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("BrandID")%>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="" style="border:none;" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                            <div style="float: left;">
                                <asp:Label ID="lblpnlHeader" runat="Server">Brand Setup</asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel"
                        Height="100%" BorderWidth="1">
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
                        </div>
                        <table cellpadding="5" cellspacing="5" border="0">
                            <tr>
                                <td align="right"><b>Brand Name </b></td>
                                <td>
                                    <asp:TextBox ID="txtBrandName" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBrandName" runat="server" ControlToValidate="txtBrandName"
                                        ValidationGroup="Save" ErrorMessage="*" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top"><b>Image</b>
                                </td>
                                <td>
                                    <input id="FileSelector" type="file" name="FileSelector" runat="server" style="width: 400px;" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload Logo" CssClass="button" OnClick="btnUpload_Click" CausesValidation="false" UseSubmitBehavior="False"></asp:Button>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnRemoveLogo" runat="server" Text="Remove Logo" CssClass="button" OnClick="btnRemoveLogo_Click"></asp:Button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image>
                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfImage" runat="server" />
                                    <asp:HiddenField ID="hfCreatedUserID" runat="server" />
                                    <asp:HiddenField ID="hfCreatedDate" runat="server" />
                                </td>
                            </tr>
                            <%--                    <tr>
                        <td align="right">Is this a Brand Group :</td>
                        <td>
                            <asp:RadioButtonList ID="rbIsBrandGroup" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbIsBrandGroup_SelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>--%>
                            <tr>
                                <td align="right"><b>Product</b></td>
                                <td>
                                <asp:TextBox ID="txtSearch" Width="180px" runat="server"></asp:TextBox>&nbsp;
                            <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="button" 
                                OnClick="btnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel1" runat="server">
                            <table cellpadding="5" cellspacing="5" border="0">
                                <tr>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <asp:Label ID="lblAvailable" runat="Server" Text="Available Products" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td align="center" style="font-size: 15px; vertical-align: bottom;">
                                        <asp:Label ID="lblSelected" runat="Server" Text="Selected Products" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ListBox ID="lstSourceFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="400px" Height="300px"
                                            EnableViewState="True"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" UseSubmitBehavior="False" />
                                        <br>
                                        <br>
                                        <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click" UseSubmitBehavior="False"
                                            Text="<<" />
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstDestFields" runat="server" Rows="10" Style="text-transform: uppercase;" DataTextField="PubName" DataValueField="PubID"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="400px" Height="300px"></asp:ListBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" ValidationGroup="Save" UseSubmitBehavior="False" />
                                        &nbsp;&nbsp; 
                                <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel" ValidationGroup="Cancel" UseSubmitBehavior="False" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:Panel>
                    <br />

                </div>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdhocSort.aspx.cs" Inherits="KMPS.MD.Administration.AdhocSort" MasterPageFile="Site.Master" %>

<%@ MasterType VirtualPath="Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
            <div style="text-align: right">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                        Processing....
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
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
                                        <img id="Img1" style="padding: 0 0 0 15px;" src="images/errorEx.jpg" runat="server"
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
            <asp:GridView ID="gvCategory" runat="server" AutoGenerateColumns="False" DataKeyNames="CategoryID"
                EnableModelValidation="True" AllowSorting="false" OnRowDataBound="gvCategory_RowDataBound"
                AllowPaging="True" OnRowCommand="gvCategory_RowCommand" RowStyle-BackColor="#EBEBEB">
                <Columns>
                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" 
                        HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left" Width="30%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" 
                         ItemStyle-HorizontalAlign="Center" >
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                    </asp:BoundField>
                    <asp:ButtonField HeaderStyle-Width="10%" ItemStyle-Width="10%" ButtonType="Link"
                        Text="<img src='Images/ic-edit.gif' style='border:none;'>" CommandName="Select"
                        ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Edit" />
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="50%" ItemStyle-Width="88%"
                        ItemStyle-HorizontalAlign="left"  HeaderStyle-HorizontalAlign="left"  ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <tr>
                                <td colspan="100%" align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                        <tr>
                                            <td width="5%"></td>
                                            <td width="50%" valign="top">
                                                <asp:GridView ShowHeader="false" ID="grdAdhoc" Width="100%" runat="server"
                                                    AutoGenerateColumns="False" GridLines="Both">
                                                    <Columns>
                                                        <asp:BoundField DataField="AdhocName" HeaderText="Adhoc">
                                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" Width="20%" />
                                                        </asp:BoundField>
                                                    </Columns>

                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="pnlHeader" runat="server" Style="background-color: #5783BD;" CssClass="collapsePanelHeader">
                <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                    <div style="float: left;">
                        <asp:Label ID="lblpnlHeader" runat="Server">Add Adhoc</asp:Label>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBody" runat="server" Style="border-color: #5783BD;" CssClass="collapsePanel" Height="260px" BorderWidth="1">
                <table cellspacing="5" cellpadding="5" border="0">
                    <tr>
                        <td align="right">Category Name :
                        </td>
                        <td>
                            <asp:Label ID="lblCategoryID" Visible="false" runat="Server" Text="0"></asp:Label>
                            <asp:TextBox ID="txtCategoryName" runat="server" Width="148px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqfldCategoryName" runat="server" ControlToValidate="txtCategoryName"
                            ErrorMessage="*" Font-Bold="false" ValidationGroup="save"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr><td align="right">
                        Sorting Order :
                        </td>
                        <td>
                            <asp:DropDownList ID="drpSortingOrder" runat="server" CssClass="formfield" >
                            </asp:DropDownList>
                       </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">Adhoc :
                        </td>
                        <td>
                            <asp:ListBox ID="lstSourceFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="200px"
                                DataTextField="DisplayName"
                                DataValueField="ColumnValue"
                                EnableViewState="True" AutoPostBack="True"></asp:ListBox>
                        </td>
                        <td>
                            <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                            <br>
                            <br>
                            <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click"
                                Text="<<" />
                        </td>

                        <td>
                            <asp:ListBox ID="lstDestFields" runat="server" Rows="10" Style="text-transform: uppercase;"
                                SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="200px"
                                DataTextField="DisplayName"
                                DataValueField="ColumnValue"></asp:ListBox>

                        </td>
                        <td>
                            <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" />
                            <br>
                            <br>
                            <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                Text="Move Down" />
                        </td>

                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" CssClass="button" ValidationGroup="save"/>
                            <asp:Button ID="btnCancel" runat="server" Text="CANCEL" OnClick="btnCancel_Click"
                                CssClass="button" />
                        </td>
                    </tr>

                    <tr>
                        <td>&nbsp;
                        </td>
                        <td align="center">
                            <asp:Label ID="lblMessage" Visible="false" runat="Server"></asp:Label></td>
                    </tr>

                </table>
            </asp:Panel>
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>

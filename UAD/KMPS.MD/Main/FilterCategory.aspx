<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FilterCategory.aspx.cs" Inherits="KMPS.MD.Main.FilterCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ModalWindow {
            border: solid 1px #c0c0c0;
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
    <center>
        <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
            <ProgressTemplate>
                <div class="TransparentGrayBackground">
                </div>
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnloadSaveFilterCategoryPopup" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="mdlPopSaveFilterCategory" runat="server" TargetControlID="btnloadSaveFilterCategoryPopup"
                    PopupControlID="pnlSaveFilterCategory" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender6" runat="server" TargetControlID="pnlRoundSaveFilterCategory"
                    Radius="6" Corners="All" />
                <asp:Panel ID="pnlSaveFilterCategory" runat="server" Width="400px" CssClass="modalPopup">
                    <asp:Panel ID="pnlRoundSaveFilterCategory" runat="server" Width="400px" CssClass="modalPopup2">
                        <br />
                        <div align="center" style="text-align: center; padding: 0px 10px 0px 10px;">
                            <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                                <tr style="background-color: #5783BD;">
                                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Save Filter Category
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div id="divPopupMessage" runat="Server" visible="false">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td valign="top" align="center" width="20%">
                                                        <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                            alt="" />
                                                    </td>
                                                    <td valign="middle" align="left" width="80%" height="100%">
                                                        <asp:Label ID="lblPopupMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">Category Name                                  
                                    <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName"
                                            ValidationGroup="Save" ForeColor="red">*</asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hfFilterCategoryID" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfParentFilterCategoryID" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click" ValidationGroup="Save" UseSubmitBehavior="False" />
                                        &nbsp;&nbsp; 
                                <asp:Button ID="btnCancel" runat="server" CssClass="button" OnClick="btnCancel_Click" Text="Cancel" ValidationGroup="Cancel" UseSubmitBehavior="False" /></td>
                                </tr>
                            </table>
                            <br />
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <center>
                    <div style="width: 90%; text-align: center; padding-left: 10px;">
                        <div id="divError" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="674" align="left">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="20%">
                                                    <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
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
                        </div>
                        <br />
                        <div id="dvFilterCategory" runat="server" align="center" style="width: 50%; clear: both">
                            <div align="left">
                                <asp:LinkButton ID="lnkExpand" runat="server" OnClick="lnkExpand_click" Font-Bold="True" ForeColor="Black">Expand All</asp:LinkButton>
                                /
                                <asp:LinkButton ID="lnkCollapse" runat="server" OnClick="lnkCollapse_click" Font-Bold="True" ForeColor="Black">Collapse All</asp:LinkButton>
                            </div>
                            <br />
                            <telerik:RadTreeList runat="server" ID="rtlFilterCategory" AutoGenerateColumns="false" AllowPaging="true" PageSize="25" DataKeyNames="FilterCategoryID" ParentDataKeyNames="ParentID" GridLines="None" OnNeedDataSource="rtlFilterCategory_NeedDataSource" OnItemDataBound ="rtlFilterCategory_ItemDataBound">
                                <Columns>
                                    <telerik:TreeListTemplateColumn UniqueName="CategoryName" HeaderStyle-Width="63%" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("CategoryName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkAddRoot" runat="server" CommandArgument="0"
                                                OnCommand="lnkAdd_Command"><img src="../Images/icon-add.gif" alt="Add" style="border:none; vertical-align:middle" />&nbsp;&nbsp;Category</asp:LinkButton>
                                        </HeaderTemplate>
                                    </telerik:TreeListTemplateColumn>
                                    <telerik:TreeListTemplateColumn HeaderStyle-Width="17%" ItemStyle-HorizontalAlign="Center" HeaderText="Add SubCategory" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"  UniqueName="AddSubCategory">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAdd" runat="server" OnCommand="lnkAdd_Command" CommandArgument='<%# Eval("FilterCategoryID")%>'><img src="../Images/icon-add.gif" alt="Edit" style="border:none;" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:TreeListTemplateColumn>
                                    <telerik:TreeListTemplateColumn HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("FilterCategoryID")%>'
                                                OnCommand="lnkEdit_Command"><img src="../Images/ic-edit.gif" alt="" style="border:none;" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:TreeListTemplateColumn>
                                    <telerik:TreeListTemplateColumn HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" OnCommand="lnkDelete_Command" CommandArgument='<%# Eval("FilterCategoryID") %>' OnClientClick="return confirm('Are you sure you want to delete?')"><img src="../Images/icon-delete.gif" alt="Delete" style="border:none;" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:TreeListTemplateColumn>
                                </Columns>
                            </telerik:RadTreeList>
                        </div>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
</asp:Content>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterSavePanel.ascx.cs" Inherits="KMPS.MD.Controls.FilterPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Filters" TagPrefix="uc" Src="~/Controls/FiltersListPanel.ascx" %>

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

    div.rddtPopup {
        border: 2px;
        background-color: white;
    }

        div.rddtPopup .rddtScroll {
            border: 1px solid #ccc;
            width: auto;
            height: 300px;
        }

    div.rddtSlide, .rddtDropDownSlide {
        float: left;
        display: none;
        position: absolute !important;
        z-index: 999999;
        width: 400px;
    }
</style>
<asp:UpdatePanel ID="UpdatePanelFilter" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Button ID="btnloadSaveFilterPopup" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="mdlPopSaveFilter" runat="server" TargetControlID="btnloadSaveFilterPopup"
            PopupControlID="pnlSaveFilter" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlEmpty" RepositionMode="RepositionOnWindowResize" />
        <asp:RoundedCornersExtender ID="RoundedCornersExtender6" runat="server" TargetControlID="pnlRoundSaveFilter"
            Radius="6" Corners="All" />
        <asp:Panel ID="pnlEmpty" runat="server" Style="display: none"></asp:Panel>
        <asp:Panel ID="pnlSaveFilter" runat="server" Width="600px" CssClass="modalPopup">
            <asp:Panel ID="pnlRoundSaveFilter" runat="server" Width="600px" CssClass="modalPopup2">
                <br />
                <div align="center" style="text-align: center; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Save Filter
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%" border="0" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td colspan="2">
                                            <div id="divError" runat="Server" visible="false" style="width: 400px">
                                                <table cellspacing="0" cellpadding="0" width="400px" align="center">
                                                    <tr>
                                                        <td id="errorMiddle" width="100%">
                                                            <table width="100%">
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
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:RadioButtonList ID="rbNewExisting" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rbNewExisting_SelectedIndexChanged" AutoPostBack="true" Font-Bold="True">
                                                <asp:ListItem Text=" New  " Value="New" Selected="true"></asp:ListItem>
                                                <asp:ListItem Text=" Existing" Value="Existing"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cbIsLocked" runat="server" Checked="false" />
                                            &nbsp;Locked for sharing
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="40px"><b>Filter Name</b></td>
                                        <td width="60px">
                                            <asp:TextBox ID="txtFilterName" Width="200px" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rvtxtFilterName" runat="server" Font-Size="xx-small" ForeColor="red"
                                                ControlToValidate="txtFilterName" ErrorMessage="*" Font-Italic="True"
                                                Font-Bold="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="ImgFilterList" runat="server" ImageUrl="~/Images/ic-lookup.jpg" OnClick="ImgFilterList_Click" Visible="false" />
                                            <asp:HiddenField ID="hfFilterID" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right"><b>Filter Category</b></td>
                                        <td>
                                            <telerik:RadDropDownTree ID="rddtFilterCategory" runat="server" Width="200px" DefaultMessage="Select Category" DataFieldID="FilterCategoryID" DataFieldParentID="ParentID" TextMode="FullPath"
                                                DataTextField="CategoryName" DataValueField="FilterCategoryID">
                                                <DropDownSettings AutoWidth="Enabled" CloseDropDownOnSelection="true" />
                                            </telerik:RadDropDownTree>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right"><b>Notes</b></td>
                                        <td>
                                            <asp:TextBox ID="txtNotes" Width="300px" runat="server" MaxLength="250" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="cbAddtoSalesView" runat="server" AutoPostBack="true" OnCheckedChanged="cbAddtoSalesView_CheckedChanged" />
                                            &nbsp; <b>Add to Sales View</b>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlQuestion" runat="server" Visible="false">
                                        <tr>
                                            <td align="right"><b>Question Name</b></td>
                                            <td>
                                                <asp:TextBox ID="txtQuestionName" Width="200px" runat="server" value="" MaxLength="200"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rvFilterQuestion" runat="server" Font-Size="xx-small"
                                                    ControlToValidate="txtQuestionName" ErrorMessage="*" Font-Italic="True" ForeColor="red"
                                                    Font-Bold="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hfQuestionCategoryID" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right"><b>Question Category</b></td>
                                            <td>
                                                <telerik:RadDropDownTree ID="rddtQuestionCategory" runat="server" Width="200px" DefaultMessage="Select Category" DataFieldID="QuestionCategoryID" DataFieldParentID="ParentID" TextMode="FullPath"
                                                    DataTextField="CategoryName" DataValueField="QuestionCategoryID">
                                                    <DropDownSettings CloseDropDownOnSelection="true" AutoWidth="Enabled" />
                                                </telerik:RadDropDownTree>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSaveFilter" Text="Save" CssClass="button"
                                                OnClick="btnSaveFilter_Click" runat="server" ValidationGroup="save" />
                                            <asp:Button ID="btnCloseFilter" Text="Cancel" CssClass="button" runat="server" OnClick="btnCloseFilter_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <uc:Filters runat="server" ID="FiltersList" Visible="false"></uc:Filters>
            </asp:Panel>
        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>

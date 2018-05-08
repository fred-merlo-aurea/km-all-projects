<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="filtergrid.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Group.filtergrid" EnableViewState="true" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Src="~/main/ECNWizard/Group/filters.ascx" TagName="filters" TagPrefix="uc1" %>
<style type="text/css">
    .accordionHeader {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        color: white;
        font-weight: bold;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }

    .accordionHeaderSelected {
        border: 1px solid #2F4F4F;
        background-color: Gray;
        font-family: Arial, Sans-Serif;
        font-weight: bold;
        color: white;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
    }

    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .modalPopupFull {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        width: 100%;
        height: 100%;
        overflow: auto;
    }

    .modalPopup {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
    }

    .modalPopupImport {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 60%;
        overflow: auto;
    }

    .style1 {
        width: 100%;
    }

    .buttonMedium {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }

    .TransparentGrayBackground {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }

    .overlay {
        position: fixed;
        z-index: 99;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }

    * html .overlay {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }

    .loader {
        z-index: 100;
        position: fixed;
        width: 120px;
        margin-left: -60px;
        background-color: #F4F3E1;
        font-size: x-small;
        color: black;
        border: solid 2px Black;
        top: 40%;
        left: 50%;
    }

    * html .loader {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
</style>
<%--<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<asp:UpdatePanel ID="upMain" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <table style="width: 100%;">
            <tr>
                <td style="width:10%;">
                    <asp:ImageButton ID="imgbtnAddFilter" ImageUrl="/ecn.images/images/icon-add.gif" CausesValidation="false" CommandName="AddFilter" runat="server" />
                </td>
                <td style="width:90%;text-align:left;">
                    <asp:Label ID="lblFilterHeader" runat="server" Text="Filters" Font-Size="12px" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <ecnCustom:ecnGridView ID="gvFilters" runat="server" Width="100%" GridLines="None" RowStyle-Font-Size="11px" OnRowDataBound="gvFilters_RowDataBound" OnRowCommand="gvFilters_RowCommand" ShowHeader="false" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:Label ID="lblFilterType" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:Label ID="lblFilterName" CssClass="subjectdefault" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:GridView ID="gvBlastFilters" GridLines="None" DataKeyNames="BlastID" runat="server" ShowHeader="false" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="EmailSubject" ItemStyle-CssClass="subjectdefault" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ImageButton ID="imgbtnEditFilter" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditCustomFilter" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-VerticalAlign="Top">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnDeleteFilter" ImageUrl="/ecn.images/images/icon-delete1.gif" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </ecnCustom:ecnGridView>
                </td>
            </tr>
        </table>
        <%--<asp:HiddenField ID="hfAddFilter" runat="server" Value="0" />
        <ajaxToolkit:ModalPopupExtender ID="mpeAddFilter" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlAddFilter" TargetControlID="hfAddFilter" />
        <asp:Panel runat="server" ID="pnlAddFilter" Width="500px" Height="300px" CssClass="modalPopup">
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="10"
                Visible="true" AssociatedUpdatePanelID="pnlFilterConfig" DynamicLayout="true">
                <ProgressTemplate>
                    <asp:Panel ID="upProgressFilterEditControl3" CssClass="overlay" runat="server">
                        <asp:Panel ID="upProgressFilterEditControlP3" CssClass="loader" runat="server">
                            <div>
                                <center>
                        <br />
                        <br />
                        <b>Processing...</b><br />
                        <br />
                        <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                        <br />
                        <br />
                        <br />
                        </center>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="pnlFilterConfig" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table style="background-color: white; width:100%; max-width: 98%;height:100%;max-height:98%;">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblFilterType" runat="server" OnSelectedIndexChanged="rblFilterType_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="SMART" Value="smart" Selected="True" />
                                    <asp:ListItem Text="CUSTOM" Value="custom" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlSmartSegment" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlSmartSegment" runat="server" AutoPostBack="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRefBlastError" runat="server" ForeColor="Red" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ListBox ID="lstboxBlast" runat="server" Width="90%" SelectionMode="Multiple" />
                                            </td>
                                        </tr>

                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlCustomFilter" Visible="false" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:ImageButton ID="imgbtnCreateFilter" runat="server" CommandName="createcustomfilter" ImageUrl="/ecn.images/images/icon-add.gif" />
                                                <asp:Label ID="lblCreateFilter" Text="Add New Filter" runat="server" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div style="height: 150px; overflow: auto;">
                                                    <asp:ListBox ID="lbAvailableFilters" Width="100%" runat="server" AutoPostBack="false" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width:100%;">
                                    <tr>
                                        <td style="text-align:center;">
                                            <asp:Button ID="btnCancelFilter" runat="server" OnClick="btnCancelFilter_Click" Text="Cancel" />
                                        </td>
                                        <td style="text-align:center;">
                                            <asp:Button ID="btnSaveFilter" runat="server" OnClick="btnSaveFilter_Click" CommandName="savefilter" Text="Save" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:Panel>--%>

    </ContentTemplate>
</asp:UpdatePanel>



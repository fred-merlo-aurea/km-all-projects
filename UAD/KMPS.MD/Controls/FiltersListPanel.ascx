<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiltersListPanel.ascx.cs" Inherits="KMPS.MD.Controls.FiltersListPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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

    div.RadGrid .rgMasterTable .rgSelectedCell,
    div.RadGrid .rgSelectedRow {
        color: #333333;
        background: #bbeaf3;
    }

        div.RadGrid .rgSelectedCell a,
        div.RadGrid .rgSelectedRow a {
            color: #333333;
        }

    div.RadGrid .rgPager .rgAdvPart {
        display: none;
    }

    .RadGriditemwrap {
        word-break: break-all !important;
        word-wrap: break-word !important;
        vertical-align: top;
    }
</style>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelFilter" DynamicLayout="true">
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
<asp:UpdatePanel ID="UpdatePanelFilter" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Button ID="btnloadFilterPopup" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="mdlPopShowFilter" runat="server" TargetControlID="btnloadFilterPopup"
            PopupControlID="pnlFilter" BackgroundCssClass="modalBackground" />
        <asp:RoundedCornersExtender ID="RoundedCornersExtender6" runat="server" TargetControlID="pnlRoundFilter"
            Radius="6" Corners="All" />
        <asp:Panel ID="pnlFilter" runat="server" Width="1000px" CssClass="modalPopup">
            <asp:Panel ID="pnlRoundFilter" runat="server" Width="1000px" CssClass="modalPopup2">
                <br />
                <div align="center" style="text-align: center; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Filters
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2" align="center">
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
                                        <td colspan="2"><b>
                                            <asp:RadioButtonList ID="rblListType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblListType_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="Filters" Selected="True">Filters</asp:ListItem>
                                            </asp:RadioButtonList>&nbsp;&nbsp;</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <table>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td><b>
                                                        <asp:Label ID="lblSearch" runat="server" Text="Filter Name or Question Name"></asp:Label></b>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlIsRecentData" runat="server" Visible="false">
                                                            <asp:CheckBox ID="cbIsRecentData" runat="server" Enabled="false" Text="Most Recent Data" Style="vertical-align: middle" TextAlign="Left" Font-Bold="True" />
                                                        </asp:Panel>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="cbAllFilters" runat="server" Text="Show filters created by all users" Checked="false" OnCheckedChanged="cbAllFilters_OnCheckedChanged" AutoPostBack="true" Style="vertical-align: middle" TextAlign="Left" Font-Bold="True" /></td>
                                                    <td>
                                                        <asp:DropDownList ID="drpSearch" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                            Width="100">
                                                            <asp:ListItem Selected="true" Value="Contains">CONTAINS</asp:ListItem>
                                                            <asp:ListItem Value="Equal">EQUAL</asp:ListItem>
                                                            <asp:ListItem Value="Start With">START WITH</asp:ListItem>
                                                            <asp:ListItem Value="End With">END WITH</asp:ListItem>
                                                        </asp:DropDownList>&nbsp;</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" Width="210px" runat="server"></asp:TextBox>&nbsp;</td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="button" ValidationGroup="search"
                                                            OnClick="btnSearch_Click" />&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width: 30%; float: left;">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border: solid 1px #5783BD">
                                                    <tr style="background-color: #5783BD;">
                                                        <td style="padding: 5px; font-size: 14px; color: #ffffff; font-weight: bold; text-align: left;">Filters Category
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 5px 5px 5px 5px;">
                                                            <telerik:RadTreeView ID="rtvFilterCategory" runat="server" IsExpanded="False" IsSelected="False" Expand="True" OnNodeClick="rtvFilterCategory_OnNodeClick" DataTextField="CategoryName" DataFieldID="FilterCategoryID" DataFieldParentID="ParentID" DataValueField="FilterCategoryID" Height="409px">
                                                                <DataBindings>
                                                                    <telerik:RadTreeNodeBinding></telerik:RadTreeNodeBinding>
                                                                </DataBindings>
                                                            </telerik:RadTreeView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="width: 69%; float: right; padding-left: 1px;">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr style="background-color: #5783BD;">
                                                        <td style="padding: 5px; font-size: 14px; color: #ffffff; font-weight: bold; text-align: left;">Select Filter
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:PlaceHolder ID="phFilters" runat="server">
                                                            <div align="center" style="text-align: center; height: 420px;">
                                                            <telerik:RadGrid AutoGenerateColumns="False" ID="rgFilters" runat="server" Visible="true" AllowPaging="true" OnNeedDataSource="rgFilters_NeedDataSource" AllowMultiRowSelection="false" Height="419px" BorderColor="#5783BD" width="661px">
                                                                <PagerStyle Mode="NextPrevAndNumeric" Position="Bottom"></PagerStyle>
                                                                <ClientSettings EnableRowHoverStyle="true" AllowExpandCollapse="true" Scrolling-AllowScroll="true" >
                                                                    <Resizing AllowColumnResize="true" EnableRealTimeResize ="true" AllowRowResize="false" ResizeGridOnColumnResize="True" />
                                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                                    <Scrolling UseStaticHeaders="true" />
                                                                </ClientSettings>
                                                                <MasterTableView DataKeyNames="FilterID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No Records" PageSize="15">
                                                                    <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" BorderStyle="Dotted" BorderWidth="1px" BorderColor="#5783BD" />
                                                                    <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn HeaderText="Filter Name" DataField="Name" UniqueName="Name" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="30%" ItemStyle-Width="30%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridTemplateColumn HeaderText="Notes" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("Notes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("Notes") == "" || Eval("Notes") == null? "display:none":"display:inline" %>'  />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn> 
                                                                        <telerik:GridBoundColumn HeaderText="Question Name" DataField="QuestionName" UniqueName="QuestionName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="23%" ItemStyle-Width="23%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridBoundColumn HeaderText="Created By" DataField="CreatedName" UniqueName="CreatedName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="25%" ItemStyle-Width="25%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridBoundColumn HeaderText="Created Date" DataField="CreatedDate" UniqueName="CreatedDate" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="12%" ItemStyle-Width="12%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                            </div>
                                                            </asp:PlaceHolder>
                                                            <asp:PlaceHolder ID="phFilterSegmentations" runat="server" Visible="false">
                                                            <div align="center" style="text-align: center; height: 420px;">
                                                            <telerik:RadGrid AutoGenerateColumns="False" ID="rgFilterSegmentations" runat="server" Visible="true" AllowPaging="true" OnNeedDataSource="rgFilterSegmentations_NeedDataSource" AllowMultiRowSelection="false" Height="419px" BorderColor="#5783BD" width="661px">
                                                                <PagerStyle Mode="NextPrevAndNumeric" Position="Bottom"></PagerStyle>
                                                                <ClientSettings EnableRowHoverStyle="true" AllowExpandCollapse="true" Scrolling-AllowScroll="true" >
                                                                    <Resizing AllowColumnResize="true" EnableRealTimeResize ="true" AllowRowResize="false" ResizeGridOnColumnResize="True" />
                                                                    <Selecting AllowRowSelect="true"></Selecting>
                                                                    <Scrolling UseStaticHeaders="true" />
                                                                </ClientSettings>
                                                                <MasterTableView DataKeyNames="FilterSegmentationID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No Records" PageSize="15">
                                                                    <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" BorderStyle="Dotted" BorderWidth="1px" BorderColor="#5783BD" />
                                                                    <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                                    <Columns>
                                                                        <telerik:GridBoundColumn HeaderText="Filter Name" DataField="Name" UniqueName="Name" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="25%" ItemStyle-Width="25%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridBoundColumn HeaderText="Filter Segmentation" DataField="FilterSegmentationName" UniqueName="FilterSegmentationName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="28%" ItemStyle-Width="28%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridTemplateColumn HeaderText="Notes" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="ImgNotes" runat="server" title='<%# Eval("Notes") %>'  ImageUrl="../Images/ic-note.jpg" style='<%# Eval("Notes") == "" || Eval("Notes") == null? "display:none":"display:inline" %>'  />
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>                                                                
                                                                        <telerik:GridBoundColumn HeaderText="Created By" DataField="CreatedName" UniqueName="CreatedName" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="25%" ItemStyle-Width="25%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                        <telerik:GridBoundColumn HeaderText="Created Date" DataField="CreatedDate" UniqueName="CreatedDate" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="12%" ItemStyle-Width="12%" ItemStyle-CssClass="RadGriditemwrap" />
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                            </div>
                                                            </asp:PlaceHolder>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnSelectFilter" Text="Select Filter" runat="server" CssClass="buttonMedium" ValidationGroup="select"
                                    OnClick="btnSelectFilter_Click" />
                                <asp:Button ID="btnCloseFilter" Text="Close" CssClass="button" runat="server" OnClick="btnCloseFilter_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

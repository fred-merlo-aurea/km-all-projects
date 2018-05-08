<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Questions.aspx.cs" Inherits="KMPS.MD.Main.Questions" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register TagName="Download" TagPrefix="uc" Src="~/Controls/DownloadPanel.ascx" %>
<%@ Register TagName="Venn" TagPrefix="uc" Src="~/Controls/FilterVennDiagram.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <center>
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
                z-index: 10001 !important;
            }

            .modalPopup2 {
                background-color: #ffffff;
                width: 270px;
                vertical-align: top;
                z-index: 10001 !important;
            }

            .modalPopupGeo {
                background-color: transparent;
                padding: 1em 6px;
                z-index: 10002 !important;
            }

            .modalPopup2Geo {
                background-color: #ffffff;
                vertical-align: top;
                z-index: 10002 !important;
            }

            .infobox {
                position: absolute;
                border: solid 2px black;
                background-color: White;
                z-index: 1000;
                padding: 5px;
                width: 180px;
            }

            .handleImage {
                width: 15px;
                height: 16px;
                background-image: url(../../images/HandleGrip.png);
                overflow: hidden;
                cursor: se-resize;
            }

            .resizingText {
                padding: 0px;
                border-style: solid;
                border-width: 2px;
                border-color: #7391BA;
            }

            #myMap {
                cursor: crosshair !important;
            }
        </style>
        <script type="text/javascript" src="../Scripts/BingMaps/mapcontrol.js?dt=10042017"></script>

        <script type="text/javascript">

            Sys.Application.add_load(pageLoad);
            function pageLoad() {

                var mpeBehaviorHandle = $find("Modal_Geocode_Behavior");
                mpeBehaviorHandle.add_shown(LoadMap);
            }

            function SetReloadValue() {
                document.getElementById("<%= hfReloadValue.ClientID %>").value = 'false';
            }
        </script>
        <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="upnlGridView" DynamicLayout="true">
            <ProgressTemplate>
                <div class="TransparentGrayBackground">
                </div>
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 100005; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnDownloadQuestions" />
                <asp:AsyncPostBackTrigger ControlID="btnCloseFilterPopup" />
            </Triggers>
            <ContentTemplate>
                <div id="divError" runat="Server" visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="errorMiddle">
                                <table width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
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
                <br />

                <%-- Pop up for Filter  --%>
                <asp:Button ID="btnShowPopupFilter" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="mdlPopupFilter" runat="server" TargetControlID="btnShowPopupFilter"
                    PopupControlID="pnlPopupFilter" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender5" runat="server" BehaviorID="RoundedCornersBehavior5"
                    TargetControlID="pnlPopupRoundFilter" Radius="6" Corners="All" />
                <asp:Panel ID="pnlErroFilter" runat="server" Visible="false">
                    <asp:Label ID="lblErrorFilter" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlPopupFilter" runat="server" Width="1200px" Height="500px" Style="display: none"
                    CssClass="modalPopup">
                    <asp:Panel ID="pnlPopupRoundFilter" runat="server" Width="1200px" Height="500px" CssClass="modalPopup2">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSamplePdf" />
                                <asp:PostBackTrigger ControlID="grdFilters" />
                            </Triggers>
                            <ContentTemplate>
                                <%-- Pop up for GeoLocation Reports --%>
                                <asp:Button ID="btnShowPopupGeo" runat="server" Style="display: none" />
                                <asp:ModalPopupExtender ID="mdlPopupGeo" runat="server" TargetControlID="btnShowPopupGeo"
                                    PopupControlID="pnlPopupGeo" CancelControlID="btnCloseGeo" BackgroundCssClass="modalBackground" />
                                <asp:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" BehaviorID="RoundedCornersBehavior3"
                                    TargetControlID="pnlPopupRoundGeo" Radius="6" Corners="All" />
                                 <asp:Panel ID="pnlPopupGeo" runat="server" Width="1050px" Style="display: none" CssClass="modalPopupGeo">
                                    <asp:Panel ID="pnlPopupRoundGeo" runat="server" Width="1050px" CssClass="modalPopup2Geo">
                                        <br />
                                        <div align="center" style="text-align: center; height: 400px; padding: 0px 10px 0px 10px; overflow: scroll">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD">
                                                <tr style="background-color: #5783BD;">
                                                    <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">REPORT
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding: 10px">
                                                        <rsweb:ReportViewer ID="rvGeoLocation" runat="server" BackColor="White" DocumentMapWidth="25%"
                                                            Width="1000px" Visible="False" AsyncRendering="false">
                                                        </rsweb:ReportViewer>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div align="center" style="width: 100%">
                                            <asp:Button ID="btnCloseGeo" runat="server" Text="Close" CssClass="button" />
                                        </div>
                                        <br />
                                    </asp:Panel>
                                </asp:Panel>
                                <%-- Pop up for GeoMap Reports --%>
                                <asp:Button ID="btnShowPopupGeoMap" runat="server" Style="display: none" />
                                <asp:ModalPopupExtender ID="mdlPopupGeoMap" runat="server" TargetControlID="btnShowPopupGeoMap"
                                    PopupControlID="pnlPopupGeoMap" CancelControlID="btnCloseGeoMap" BackgroundCssClass="modalBackground"
                                    BehaviorID="Modal_Geocode_Behavior" />
                                <asp:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" BehaviorID="RoundedCornersBehavior4"
                                    TargetControlID="pnlPopupRoundGeoMap" Radius="6" Corners="All" />
                                <asp:Panel ID="pnlPopupGeoMap" runat="server" Width="1050px" Style="display: none"
                                    CssClass="modalPopupGeo">
                                    <asp:Panel ID="pnlPopupRoundGeoMap" runat="server" Width="1050px" CssClass="modalPopup2Geo">
                                       <br />
                                        <div align="center" style="text-align: center; height: 500px; padding: 0px 10px 0px 10px;">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD">
                                                <tr style="background-color: #5783BD;">
                                                    <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">REPORT
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="myMapCoords" runat="server" Style="display: none;"></asp:Label>
                                                        <div id="myMap" style="position: relative; width: 100%; height: 400px; cursor: crosshair !important; z-index: 1"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div align="center" style="width: 100%">
                                            <asp:Button ID="btnCloseGeoMap" runat="server" Text="Close" CssClass="button" />
                                        </div>
                                        <br />
                                    </asp:Panel>
                                </asp:Panel>
                                <div align="right" style="width: 100%">
                                    <br />
                                </div>
                                <div align="center" style="text-align: center; padding: 0px 10px 0px 10px;">
                                    <br />
                                    <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold" colspan="2">Results
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2">
                                                <br />
                                                <div class="labelbold">
                                                    Question Name :
                                                <asp:Label ID="lblQuestionName" runat="Server"></asp:Label>
                                                    <asp:HiddenField ID="hfSampleSubscriptionID" runat="server" Value="0" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <div class="labelbold">
                                                    Total Unique Records :
                                                        <asp:Label ID="lblCounts" runat="Server"></asp:Label>
                                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                                                        Width="1000px" Visible="False">
                                                    </rsweb:ReportViewer>
                                                    <rsweb:ReportViewer ID="ReportViewer2" runat="server" BackColor="White" DocumentMapWidth="25%"
                                                        Width="1000px" Visible="False">
                                                    </rsweb:ReportViewer>
                                                </div>
                                            </td>
                                            <td align="right" width="25%">
                                                <asp:Button ID="btnSamplePdf" runat="server" Text="Sample PDF" CssClass="button" OnClick="btnSamplePdf_Click" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div align="center" style="text-align: center; overflow: auto; height: 300px">
                                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                                        <tr>
                                                            <td valign="top" width="60%">
                                                                <asp:HiddenField ID="hfSelectedFilterNos" runat="server" />
                                                                <asp:HiddenField ID="hfSuppressedFilterNos" runat="server" />
                                                                <asp:HiddenField ID="hfSelectedFilterOperation" runat="server" />
                                                                <asp:HiddenField ID="hfSuppressedFilterOperation" runat="server" />
                                                                <asp:HiddenField ID="hfFilterCombination" runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfDownloadCount" Value="0" runat="server"></asp:HiddenField>
                                                                <asp:GridView ID="grdFilters" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    Height="100%" ShowFooter="true" AllowPaging="false" DataKeyNames="FilterNo" OnRowDataBound="grdFilters_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="#" HeaderStyle-Width="2%" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("FilterNo")%>' Font-Bold="true"></asp:Label>
                                                                                <asp:Label ID="lblViewType" runat="server" Text='<%# Eval("ViewType") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblBrandID" runat="server" Text='<%# Eval("BrandID") %>' Visible="false"></asp:Label>
                                                                                <asp:Label ID="lblPubID" runat="server" Text='<%# Eval("PubID") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Filters" HeaderStyle-Width="86%" ItemStyle-Width="86%"
                                                                            ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                                            <ItemTemplate>
                                                                                <asp:Panel ID="pnlResultHeader" runat="server" Height="28px" CssClass="collapsePanelHeader"
                                                                                    BackColor="#eeeeee" ForeColor="Black">
                                                                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                                                                        <div style="float: left;">
                                                                                        </div>
                                                                                        <div style="float: left; margin-left: 20px;">
                                                                                            <asp:Label ID="pnlResultLabel" runat="server">(Show Filters...)</asp:Label>
                                                                                        </div>
                                                                                        <div style="float: right; vertical-align: middle;">
                                                                                            <asp:ImageButton ID="pnlResultImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                                                                AlternateText="(Show Filters...)" />
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:Panel ID="pnlResultBody" runat="server" CssClass="collapsePanel" Height="0"
                                                                                    BorderColor="#eeeeee" BorderWidth="2">
                                                                                    <div style="width: 600px; overflow-x:auto;">
                                                                                    <asp:GridView ShowHeader="false" ID="grdFilterValues" Width="100%" runat="server"
                                                                                        AutoGenerateColumns="False" GridLines="Both">
                                                                                        <Columns>
                                                                                            <asp:BoundField DataField="name" HeaderText="Column">
                                                                                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="right" Width="20%" />
                                                                                            </asp:BoundField>
                                                                                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="3%" ItemStyle-Width="3%" ItemStyle-HorizontalAlign="center"
                                                                                                ItemStyle-VerticalAlign="Middle">
                                                                                                <ItemTemplate>
                                                                                                    =
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="77%" ItemStyle-Width="77%" ItemStyle-HorizontalAlign="left"
                                                                                                ItemStyle-VerticalAlign="Middle">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblFilterText" runat="server" Text='<%# Eval("Text") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblAdhocCondition" runat="server" Text='<%# Eval("name").ToString() == "Adhoc" || Eval("name").ToString() == "Open Activity" ||  Eval("name").ToString() == "Click Activity" ? Eval("name").ToString() == "Open Activity" ||  Eval("name").ToString() == "Click Activity" ? " - " + Eval("SearchCondition") :  " - " + Eval("SearchCondition") + " - " + Eval("Values") : "" %>'></asp:Label>
                                                                                                    <asp:Label ID="lblFilterValues" runat="server" Text='<%# Eval("Values") %>' Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblFiltername" runat="server" Text='<%# Eval("name") %>' Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblSearchCondition" runat="server" Text='<%# Eval("SearchCondition") %>' Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblFilterType" runat="server" Text='<%# Eval("FilterType") %>' Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblGroup" runat="server" Text='<%# Eval("Group") %>' Visible="false"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:CollapsiblePanelExtender ID="cpeDemo3" runat="Server" TargetControlID="pnlResultBody"
                                                                                    ExpandControlID="pnlResultHeader" CollapseControlID="pnlResultHeader" Collapsed="true"
                                                                                    TextLabelID="pnlResultLabel" ImageControlID="pnlResultImage" ExpandedText="(Hide Filters...)"
                                                                                    CollapsedText="(Show Filters...)" ExpandedImage="~/images/collapse_blue.jpg"
                                                                                    CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                            FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>'
                                                                                    Text='<%# Eval("Count") %>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                    OnCommand="lnkdownload_Command"></asp:LinkButton>
                                                                            </ItemTemplate>

                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <br />
                                                            </td>
                                                            <td valign="top" width="20%">
                                                                <asp:GridView ID="grdFilterCounts" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                    Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="SelectedFilterNo">
                                                                    <Columns>
                                                                        <asp:BoundField HeaderText="Filters" DataField="FilterDescription" HeaderStyle-Width="33%"
                                                                            ItemStyle-Width="33%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                                            FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                                        <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                            FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>'
                                                                                    Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                    OnCommand="lnkCount_Command"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                            FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                            <ItemTemplate>
<%--                                                                                <asp:HiddenField ID="hfFilterID" runat="server" Value='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") %>' />--%>
                                                                                <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                    <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                            CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                            CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                            CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                            Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")  + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                            OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                            Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")  + "/" + Eval("FilterDescription")  + "/" + Eval("Count") %>'
                                                                                            OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                            Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")  + "/" + Eval("FilterDescription")  + "/" + Eval("Count") %>'
                                                                                            OnCommand="lnkGeoReport_Command" />
                                                                                        <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                            CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")  + "/" + Eval("FilterDescription")  + "/" + Eval("Count") %>' OnCommand="lnkGeoMaps_Command" />
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                    PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                            <td align="center" valign="top" width="20%">
                                                                <uc:Venn runat="server" ID="ctrlVenn" Visible="false"></uc:Venn>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:HiddenField ID="hfReloadValue" runat="server" Value="true" />
                                                <asp:Button ID="btnCloseFilterPopup" runat="server" Text="Close" CssClass="button" OnClick="btnCloseFilterPopup_Click" OnClientClick="SetReloadValue();" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <br />

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </asp:Panel>

                <div style="width: 90%; text-align: left; padding-left: 10px;">
                    <table border="0" cellpadding="5" cellspacing="3" align="left" width="100%">
                        <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                            <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                <td align="right">
                                    <b>Brand</b>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                    <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                        DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td width="9%" align="right">
                                <b>Question Name</b>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtQuestionName" runat="server" MaxLength="100" Width="200px" Text=""></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                                    Enabled="true" TargetControlID="txtQuestionName" WatermarkCssClass="watermarked"
                                    WatermarkText="exact match, partial match, keyword" />
                                &nbsp;
                                
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="button" OnClick="btnSearch_Click" ValidationGroup="Search" />
                            </td>
                            <td align="right"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div align="right">
                                    <asp:Button ID="btnDownloadQuestions" runat="server" Text="Download Questions" CssClass="buttonMedium" OnClick="btnDownloadQuestions_Click" /></br></br>
                                </div>
                                <div style="width: 20%; float: left;">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="border: solid 1px #5783BD">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 14px; color: #ffffff; font-weight: bold; text-align: left;">Questions Category</td>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="30%">
                                                <telerik:RadTreeView ID="rtvQuestionCategory" runat="server" IsExpanded="False" IsSelected="False" Expand="True" OnNodeClick="rtvQuestionCategory_OnNodeClick" DataTextField="CategoryName" DataFieldID="QuestionCategoryID" DataFieldParentID="ParentID" DataValueField="QuestionCategoryID" Height="504px" MultipleSelect="True" BackColor="White">
                                                    <DataBindings>
                                                        <telerik:RadTreeNodeBinding></telerik:RadTreeNodeBinding>
                                                    </DataBindings>
                                                </telerik:RadTreeView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="width: 79%; float: right; padding-left: 1px;">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 14px; color: #ffffff; font-weight: bold; text-align: left;">&nbsp;Questions
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <telerik:RadGrid ID="rgQuestions" runat="server" PageSize="20" AlternatingItemStyle-BackColor="#EBEBEB"
                                                    AllowSorting="True" AllowMultiRowSelection="True" AllowPaging="True" ShowGroupPanel="True"
                                                    AutoGenerateColumns="False" GridLines="none" OnNeedDataSource="rgQuestions_NeedDataSource" BorderColor="#5783BD">
                                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                    <MasterTableView Width="100%" PageSize="25" PagerStyle-Mode="NextPrevAndNumeric" PagerStyle-AlwaysVisible="true">
                                                        <GroupByExpressions>
                                                            <telerik:GridGroupByExpression>
                                                                <SelectFields>
                                                                    <telerik:GridGroupByField FieldAlias="QuestionCategory" FieldName="QuestionCategoryName" HeaderText="Question Category"></telerik:GridGroupByField>
                                                                </SelectFields>
                                                                <GroupByFields>
                                                                    <telerik:GridGroupByField FieldName="QuestionCategoryName" SortOrder="Descending"></telerik:GridGroupByField>
                                                                </GroupByFields>
                                                            </telerik:GridGroupByExpression>
                                                        </GroupByExpressions>
                                                        <Columns>
                                                            <telerik:GridBoundColumn SortExpression="QuestionCategoryName" HeaderText="Question Category" DataField="QuestionCategoryName" HeaderStyle-Font-Bold="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Question Name" SortExpression="QuestionName" DataField="QuestionName" HeaderStyle-Font-Bold="true">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkFilterQuestion" runat="server" Text='<%# Eval("QuestionName")%>' OnCommand="lnkFilterQuestion_Command" CommandName="download" CommandArgument='<%# Eval("FilterID") + "/" + Eval("QuestionName")%>'>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridBoundColumn SortExpression="BrandName" HeaderText="Brand Name" DataField="BrandName" HeaderStyle-Font-Bold="true">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="PubName" HeaderText="Product" DataField="PubName" HeaderStyle-Font-Bold="true">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                                                            ResizeGridOnColumnResize="False"></Resizing>
                                                    </ClientSettings>
                                                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>
            <uc:Download runat="server" ID="DownloadPanel" Visible="false"></uc:Download>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

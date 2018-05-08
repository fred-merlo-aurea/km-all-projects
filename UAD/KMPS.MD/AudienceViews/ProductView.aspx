<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductView.aspx.cs" Inherits="KMPS.MD.Main.ProductView"
    MasterPageFile="../MasterPages/Site.Master"  ValidateRequest="false" %>

<%@ Register TagName="ShowFilter" TagPrefix="uc" Src="~/Controls/FiltersListPanel.ascx" %>
<%@ Register TagName="FilterSave" TagPrefix="uc" Src="~/Controls/FilterSavePanel.ascx" %>
<%@ Register TagName="Download" TagPrefix="uc" Src="~/Controls/DownloadPanel.ascx" %>
<%@ Register TagName="CLDownload" TagPrefix="uc" Src="~/Controls/DownloadPanel_CLV.ascx" %>
<%@ Register TagName="EVDownload" TagPrefix="uc" Src="~/Controls/DownloadPanel_EV.ascx" %>
<%@ Register TagName="Adhoc" TagPrefix="uc" Src="~/Controls/Adhoc.ascx" %>
<%@ Register TagName="Activity" TagPrefix="uc" Src="~/Controls/Activity.ascx" %>
<%@ Register TagName="Circulation" TagPrefix="uc" Src="~/Controls/Circulation.ascx" %>
<%@ Register TagName="Venn" TagPrefix="uc" Src="~/Controls/FilterVennDiagram.ascx" %>
<%@ Register TagName="FilterSegmentation" TagPrefix="uc" Src="~/Controls/FilterSegmentation.ascx" %>
<%@ Register TagName="DataCompareSummary" TagPrefix="uc" Src="~/Controls/DataCompareSummary.ascx" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript" src="../Scripts/BingMaps/mapcontrol.js?dt=10042017"></script>

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
            border: 1px#c0c0c0;
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

        .AutoExtender {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: .8em;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 24px;
            padding: 1px;
            background-color: White;
            width: 300px !important;
            text-align: left;
            list-style-type: none;
        }

        .AutoExtenderList {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            color: Maroon;
            z-index: 100;
        }

        .AutoExtenderHighlight {
            color: #c8c8c8;
            background-color: #006699;
            cursor: pointer;
        }

        .custom-combobox {
            position: relative;
            display: inline-block;
        }

        .custom-combobox-toggle {
            position: absolute;
            top: 0;
            bottom: 0;
            margin-left: -1px;
            padding: 0;
            *height: 1.8em;
            *top: 0.1em;
        }

        .custom-combobox-input {
            margin: 0;
            padding: 0.3em;
            width: 150px !important;
        }

        .ui-widget {
            font-family: Tahoma, Arial, sans-serif;
            font-size: x-small;
        }

        div.RadComboBox_Default table .rcbInputCell,
        div.RadComboBox_Default table .rcbArrowCell {
            border: Solid 1px grey !important;
            height: 14px;
            line-height: 13px;
            padding: 0;
        }

            div.RadComboBox_Default table .rcbInputCell input {
                background-color: white !important;
                height: 14px;
                line-height: 13px;
                padding: 0;
            }

        .RadComboBox * {
            height: 14px !important;
        }

        .RadPivotGrid td.rpgDataCell {
            text-align: center;
        }

        .RadPivotGrid td.rpgRowHeaderGrandTotal{
            white-space: nowrap;
        }

        .RadPivotGrid td.rpgRowHeaderField.rpgRowHeader{
            white-space: nowrap;
        }

        .RadListBox span.rlbText em {
            background-color: #E5E5E5;
            font-weight: bold;
            font-style: normal;
        }

        .ui-dialog
        {
            z-index: 1000001;
            font-size: 12px;
        }

        .ui-dialog-titlebar-close
        {
           visibility: hidden;

        }
    </style>

    <script type="text/javascript">
        var map = null;
        var shapeLayer;
        var lineLayer;
        var deleteLayer;
        var polyPoints = new Array();
        var down = 0;
        var X = 0, Y = 0;
        var origin;
        var undo;

        Sys.Application.add_load(pageLoad);
        var listbox;
        var filterTextBox;

        $(document).ready(function () {
            $(this).bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
        });

        function confirmPopupPurchase() {
            try
            {
                var dialog = $("#paymentconfirm").dialog({
                    autoOpen: false,
                    bgiframe: true,
                    modal: true,
                    open: function (type, data) {
                        var markup = 'Are you sure you want to conduct this download? Confirming will result in pre-agreed cost that will need to be paid in the near future.';
                        $(this).html(markup);
                        $(this).parent().appendTo("form1");
                    },
                    buttons: {
                        Yes: function () {
                            dialog.dialog("close");
                            if ($('#ctl00_Content_drpIsBillable').val() == 'true') {
                                $('#ctl00_Content_drpIsBillable').prop('disabled', true);
                                $('#ctl00_Content_btnDCDownload').prop('disabled', true);
                            }
                             <%=Page.ClientScript.GetPostBackEventReference(btnDCDownload, "") %>
                        },
                        Cancel: function () {
                            dialog.dialog("close");
                        }
                    }
                });
           
                dialog.dialog("open");
                return false;
            }
            catch (ex) {
            }
        }

        function pageLoad() {
            try {
                listbox = $find("<%= rlbDimensionAvailable.ClientID %>");
                filterTextBox = document.getElementById("<%= rtbDimSearch.ClientID %>");

                // set on anything but text box
                listbox._getGroupElement().focus();

                var mpeBehaviorHandle = $find("Modal_Geocode_Behavior");
                mpeBehaviorHandle.add_shown(LoadMap);

                var rcbEmailcombo = $find("<%= this.rcbEmail.ClientID %>");

                var rcbEmailinput = rcbEmailcombo.get_inputDomElement();

                rcbEmailinput.onchange = function ()
                {
                    emailValue = $('#' + '<%=this.rcbEmail.ClientID  %>').val();

                    var combo = $find('<%=this.rcbEmailStatus.ClientID  %>');

                    combo.trackChanges();

                    for (var i = 0; i < combo.get_items().get_count() ; i++) {
                        combo.get_items().getItem(i).set_checked(false);
                    }

                    if (emailValue.indexOf('Yes') > -1 || emailValue.indexOf('All items checked') > -1) {
                        combo.get_items().getItem(0).set_checked(true);
                    }

                    combo.commitChanges();
                }
            }
            catch (err) {
            }
        }

        function OnClientDroppedHandler(sender, eventArgs) {

            // clear emphasis from matching characters
            eventArgs.get_sourceItem().set_text(clearTextEmphasis(eventArgs.get_sourceItem().get_text()));
        }

        function filterList() {
            clearListEmphasis();
            createMatchingList();
        }

        // clear emphasis from matching characters for entire list
        function clearListEmphasis() {
            var re = new RegExp("</{0,1}em>", "gi");
            var items = listbox.get_items();
            var itemText;

            items.forEach
           (
               function (item) {
                   itemText = item.get_text();
                   item.set_text(clearTextEmphasis(itemText));
               }
           )
        }

        // clear emphasis from matching characters for given text
        function clearTextEmphasis(text) {
            var re = new RegExp("</{0,1}em>", "gi");
            return text.replace(re, "");
        }

        // hide listboxitems without matching characters
        function createMatchingList(e) {

            var items = listbox.get_items();
            var startPosition = filterTextBox.value.slice(0, filterTextBox.selectionStart).length;
            var endPosition = filterTextBox.value.slice(0, filterTextBox.selectionEnd).length;

            evt = e || widow.event;
            var keyID = (evt.charCode) ? evt.charCode : ((evt.which) ? evt.which : event.keyCode);

            if (keyID == 8) {
                if (startPosition == endPosition) {
                    var filterText = filterTextBox.value.substring(0, startPosition - 1) + filterTextBox.value.substring(endPosition, filterTextBox.value.length);
                }
                else {
                    var filterText = filterTextBox.value.substring(0, endPosition - (endPosition - startPosition)) + filterTextBox.value.substring(endPosition, filterTextBox.value.length);
                }
            }
            else if (keyID == 16 || keyID == 17 || keyID == 18 || keyID == 13) {
                var filterText = filterTextBox.value;
            }
            else if (keyID == 46) {
                if (startPosition == endPosition) {
                    var filterText = filterTextBox.value.substring(0, startPosition) + filterTextBox.value.substring(endPosition + 1, filterTextBox.value.length);
                }
                else {
                    var filterText = filterTextBox.value.substring(0, startPosition) + filterTextBox.value.substring(endPosition, filterTextBox.value.length);
                }
            }
            else {

                var filterText = filterTextBox.value.substring(0, startPosition) + String.fromCharCode((evt.charCode) ? evt.charCode : ((evt.which) ? evt.which : evt.keyCode)) + filterTextBox.value.substring(endPosition, filterTextBox.value.length);
            }

            var re = new RegExp(filterText, "i");

            items.forEach
            (
                function (item) {
                    var itemText = item.get_text();
                    if (itemText.toLowerCase().indexOf(filterText.toLowerCase()) != -1) {
                        item.set_text(itemText.replace(re, "<em>" + itemText.match(re) + "</em>"));
                        item.set_visible(true);
                        
                    }
                    else {
                        item.set_visible(false);
                    }
                }
            )
        }

        function handleKeyDown(inputElement, e) {
            clearListEmphasis();
            createMatchingList(e);
        }

        $(this).bind("keydown", function (e) {
            // Allow: backspace, delete

            evt = e || widow.event;
            var keyID = (evt.charCode) ? evt.charCode : ((evt.which) ? evt.which : event.keyCode);
               
            if (keyID == 46 || keyID == 8) {
                $('#' + '<%=this.rtbDimSearch.ClientID  %>').trigger('keypress');
            }
        });

        function rlbDimensionAvailable_OnClientTransferring(sender, e) {
            e.set_cancel(true);
            var filterTextBox = document.getElementById("<%= rtbDimSearch.ClientID %>");
            var re = new RegExp(filterTextBox.value, "i");

            //manually transfer the items
            var items = e.get_items();
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                if (item.get_visible()) {
                    item.set_text(item.get_text().replace("<em>", ""));
                    item.set_text(item.get_text().replace("</em>", ""));
                    sender.transferItem(item, e.get_sourceListBox(), e.get_destinationListBox());
                    var target = e.get_domEvent().target.parentNode;
                    while (target.nodeName != "A")
                        target = target.parentNode;

                    if (target.className.indexOf("rlbTransferTo") > -1 || target.className.indexOf("rlbTransferAllTo") > -1) {
                        if (item.get_text().toLowerCase().indexOf(filterTextBox.value.toLowerCase()) != -1) {
                            item.set_text(item.get_text().replace(re, "<em>" + item.get_text().match(re) + "</em>"));
                            item.set_visible(true);
                        }
                        else
                        {
                            item.set_visible(false);
                        }
                    }
                }
            }
        }
    </script>
    <div>
        <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
            AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
            <progresstemplate>
                <div class="TransparentGrayBackground">
                </div>
                <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </progresstemplate>
        </asp:UpdateProgress>
        <%-- Pop up for DataCompareDownload --%>
        <asp:Button ID="btnShowPopupDC" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="mdlPopupDCDownload" runat="server" TargetControlID="btnShowPopupDC"
            PopupControlID="pnlPopupDataCompare" CancelControlID="btnDCClose" BackgroundCssClass="modalBackground" />
        <asp:RoundedCornersExtender ID="RoundedCornersExtender9" runat="server" BehaviorID="RoundedCornersBehavior9"
            TargetControlID="pnlPopupRoundDataCompare" Radius="6" Corners="All" />
        <asp:Panel ID="pnlErroDataCompare" runat="server" Visible="false">
            <asp:Label ID="Label1" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlPopupDataCompare" runat="server" Width="500px" Style="display: none" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupRoundDataCompare" runat="server" Width="500px" CssClass="modalPopup2">
                <div align="center" style="text-align: center; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">Download Records
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="plKmStaff" runat="server">
                            <tr>
                                <td colspan="2" align="left">Is Import Billable?
                                    <asp:DropDownList ID="drpIsBillable" Width="100" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="drpIsBillable_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="true" Value="true">Yes</asp:ListItem>
                                        <asp:ListItem Value="false">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="plNotes" runat="server" Visible="false">
                            <tr>
                                <td colspan="2" align="left" style="vertical-align:middle">Notes&nbsp;
                                    <asp:TextBox ID="txtNotes" runat="server" Columns="5" Width="300px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                        <tr>
                            <td align="left">
                                <telerik:RadCaptcha ID="RadCaptcha1" runat="server" ErrorMessage="Page not valid. The code you entered is not valid."
                                    ValidationGroup="Group" ForeColor="Red" EnableRefreshImage="true">
                                    <CaptchaImage ImageCssClass="imageClass" />
                                </telerik:RadCaptcha>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="paymentconfirm" title="Confirmation Box"></div>
                                <asp:Button ID="btnDCDownload" runat="server" Text="Download" CssClass="button" OnClick="btnDCDownload_Click" ValidationGroup="Group" />
                                &nbsp; 
                                <asp:Button ID="btnDCClose" runat="server" Text="Close" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </asp:Panel>
        </asp:Panel>
        <%-- Pop up for GeoLocation Reports --%>
        <asp:Button ID="btnShowPopupGeo" runat="server" Style="display: none" />
        <asp:ModalPopupExtender ID="mdlPopupGeo" runat="server" TargetControlID="btnShowPopupGeo"
            PopupControlID="pnlPopupGeo" CancelControlID="btnCloseGeo" BackgroundCssClass="modalBackground" />
        <asp:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" BehaviorID="RoundedCornersBehavior3"
            TargetControlID="pnlPopupRoundGeo" Radius="6" Corners="All" />
        <asp:Panel ID="pnlErroGeo" runat="server" Visible="false">
            <asp:Label ID="lblErrorGeo" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlPopupGeo" runat="server" Width="1050px" Style="display: none" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupRoundGeo" runat="server" Width="1050px" CssClass="modalPopup2">
                <asp:Label ID="lbl" Visible="false" runat="server"></asp:Label>
                <div align="center" style="text-align: center; height: 500px; padding: 0px 10px 0px 10px; overflow:auto;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">REPORT
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                                    Width="1000px" Visible="False">
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
            PopupControlID="pnlPopupGeoMap" BackgroundCssClass="modalBackground"
            BehaviorID="Modal_Geocode_Behavior" />
        <asp:RoundedCornersExtender ID="RoundedCornersExtender5" runat="server" BehaviorID="RoundedCornersBehavior5"
            TargetControlID="pnlPopupRoundGeoMap" Radius="6" Corners="All" />
        <asp:Panel ID="pnlErroGeoMap" runat="server" Visible="false">
            <asp:Label ID="lblErrorGeoMap" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlPopupGeoMap" runat="server" Width="1050px" Style="display: none"
            CssClass="modalPopup">
            <asp:Panel ID="pnlPopupRoundGeoMap" runat="server" Width="1050px" CssClass="modalPopup2">
                <asp:Label ID="Label2" Visible="false" runat="server"></asp:Label>
                <div align="center" style="text-align: center; height: 500px; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">REPORT
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="myMapCoords" runat="server" Style="display: none;" EnableViewState="false"></asp:Label>
                                <div id="myMap" style="position: relative; width: 100%; height: 400px; cursor: crosshair !important; z-index: 1"></div>
                                <div id="listOfPins" style="max-height:250px;width:250px;overflow-y:scroll;"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div align="center" style="width: 100%">
                    <asp:Button ID="btnCloseGeoMap" runat="server" Text="Close" CssClass="button" OnClick="btnCloseGeoMap_Click" />
                </div>
                <br />
            </asp:Panel>
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <triggers>
                <asp:PostBackTrigger ControlID="TabContainer$tpFilters$grdFilters" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilters$grdFilterCounts" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilters$btnOpenSaveFilterPopup" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilterIntersection$tcFilterCalcualtion$tpIntersect$grdFilterInterSectCount" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilterIntersection$tcFilterCalcualtion$tpUnion$grdFilterUnionCount" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilterIntersection$tcFilterCalcualtion$tpNotIn$grdFilterNotInCount" />
                <asp:PostBackTrigger ControlID="btnPopupSaveCampaign" />
                <asp:PostBackTrigger ControlID="btnPopupCloseCampaign" />
                <asp:PostBackTrigger ControlID="btnPopupCloseDimension" />
                <asp:PostBackTrigger ControlID="TabContainer$tpFilterSegmentation" />
                <asp:AsyncPostBackTrigger ControlID="ShowFilter" EventName="CausePostBack" />
                <asp:AsyncPostBackTrigger ControlID="FilterSave" EventName="CausePostBack" />
                <asp:PostBackTrigger ControlID="lnkMatchedRecords" />
                <asp:PostBackTrigger ControlID="lnkNonMatchedRecords" />
                <asp:PostBackTrigger ControlID="lnkMatchedNotIn" />
                <asp:PostBackTrigger ControlID="btnDCDownload" />
            </triggers>
            <contenttemplate>
                <!-- hidden Variables to save the state -->
                <asp:Label ID="lblSelectedFilterNos" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblSuppressedFilterNos" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblSelectedFilterOperation" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblSuppressedFilterOperation" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblAddFilters" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblCodeSheetID" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblPermission" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblPremissionType" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblFilterCombination" Visible="false" runat="server"></asp:Label>
                <asp:Label ID="lblDownloadCount" Visible="false" Text="0" runat="server"></asp:Label>
                <asp:Label ID="lblIsPopupDimension" Visible="false" runat="server" Text="false"></asp:Label>
                <asp:Label ID="lblIsPopupCrossTab" Visible="false" runat="server" Text="false"></asp:Label>
                <asp:HiddenField ID="hfHasFilterSegmentation" runat="server" Value="false" />
                <%-- Pop up for Demographic Reports --%>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="mdlPopupDimensionReport" runat="server" TargetControlID="btnShowPopup"
                    PopupControlID="pnlPopup" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" BehaviorID="RoundedCornersBehavior1"
                    TargetControlID="pnlPopupRound" Radius="6" Corners="All" />
                <asp:Panel ID="pnlPopup" runat="server" Width="1100px" Style="display: none" CssClass="modalPopup">
                    <asp:Panel ID="pnlPopupRound" runat="server" Width="1000px" CssClass="modalPopup2">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnDownload" />
                                <asp:PostBackTrigger ControlID="btnDownloadDetails" />
                                <asp:PostBackTrigger ControlID="grdSubReport" />
                                <asp:AsyncPostBackTrigger ControlID="chkIncludePermissions" />
                            </Triggers>
                            <ContentTemplate>
                                <div align="center" style="text-align: center; height: 500px; overflow: scroll; padding: 0px 10px 0px 10px;">
                                    <br />
                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">
                                                <asp:HiddenField ID="hfReportFor" runat="server" />
                                                <asp:Label ID="lblReportFor" runat="server" Style="text-transform: uppercase" Text="Demographic Report"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:DropDownList ID="drpDimension" runat="server" Width="250px" DataTextField="DisplayName" DataValueField="ResponseGroupID" OnSelectedIndexChanged="drpDimension_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <asp:PlaceHolder ID="phReport" runat="server" Visible="false">
                                            <tr>
                                                <td align="left">
                                                    <br />
                                                    Description : &nbsp; &nbsp;
                                                <asp:TextBox ID="txtDescription" Width="300px" runat="server" value=""></asp:TextBox>&nbsp;
                                                &nbsp;
                                                <asp:Button ID="btnSearch" runat="server" CssClass="button" OnClick="btnSearch_Click"
                                                    Text="Search" />&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                                <asp:CheckBox ID="chkIncludePermissions" runat="server" Text=" Include Permissions"
                                                    Checked="false" OnCheckedChanged="chkIncludePermissions_CheckedChanged" AutoPostBack="true" />&nbsp; &nbsp;
                                                <asp:DropDownList ID="drpExport" Width="100" runat="server">
                                                    <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                                                    <asp:ListItem Value="xls">Excel</asp:ListItem>
                                                    <asp:ListItem Value="doc">Word</asp:ListItem>
                                                </asp:DropDownList>
                                                    &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click"
                                                        CssClass="buttonMedium" Visible="true"></asp:Button>
                                                    <asp:ModalPopupExtender ID="mdlPopupProgress" runat="server" TargetControlID="pnlPopupProgress"
                                                        PopupControlID="pnlPopupProgress" BackgroundCssClass="blur" />
                                                    <asp:Panel ID="pnlPopupProgress" runat="server" CssClass="progress" Style="display: none">
                                                        <div class="TransparentGrayBackground">
                                                        </div>
                                                        <div id="div2" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                                                            <br />
                                                            <b>Processing...</b><br />
                                                            <br />
                                                            <img src="../images/loading.gif" />
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                                <tr>
                                                    <td align="left" style="padding: 10px"></td>
                                                </tr>
                                                <asp:Panel ID="pnlDemographicPopupError" runat="server" Visible="false">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDemographicPopupError" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </asp:Panel>
                                                <tr>
                                                    <td style="padding: 10px">
                                                        <asp:GridView ID="grdSubReport" runat="server" Width="400px" AutoGenerateColumns="False"
                                                            ShowFooter="false" AllowPaging="true" DataKeyNames="ID, Count, Desc"
                                                            AllowSorting="True" OnSorting="grdSubReport_Sorting" OnPageIndexChanging="grdSubReport_PageIndexChanging">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectDownload" runat="server" Text="" Checked="false" Visible='<%# Convert.ToInt32(Eval("Mail")) == 0 ? false : true %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Value" HeaderText="Value" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="10px" ItemStyle-Width="10px" SortExpression="Value">
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Desc" HeaderText="Description" HeaderStyle-HorizontalAlign="Left"
                                                                    HeaderStyle-Width="210px" ItemStyle-Width="210px" SortExpression="Desc">
                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Mail" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Mail">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload1" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID")  + "/" + "Mail"  + "/" + Eval("Mail") %>'
                                                                            Text='<%# Eval("Mail") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Mail")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Email" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Email">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload2" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID")  + "/" + "Email"  + "/" + Eval("Email")%>'
                                                                            Text='<%# Eval("Email") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Email")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Phone" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Phone">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload3" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID")  + "/" + "Phone"  + "/" + Eval("Phone")%>'
                                                                            Text='<%# Eval("Phone") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Phone")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mail & Phone" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Mail_Phone">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload4" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID")  + "/" + "Mail_Phone"  + "/" + Eval("Mail_Phone")%>'
                                                                            Text='<%# Eval("Mail_Phone") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Mail_Phone")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Email & Phone" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Email_Phone">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload5" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID") + "/" + "Email_Phone"  + "/" + Eval("Email_Phone") %>'
                                                                            Text='<%# Eval("Email_Phone") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Email_Phone")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Mail & Email" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Mail_Email">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload6" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID") + "/" + "Mail_Email" + "/" + Eval("Mail_Email")%>'
                                                                            Text='<%# Eval("Mail_Email") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("Mail_Email")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="All" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="All_Records">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload7" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID") + "/" + "All_Records"  + "/" + Eval("All_Records")%>'
                                                                            Text='<%# Eval("All_Records") %>' OnCommand="lnkPermissionDownload_Command" Enabled='<%# Convert.ToInt32(Eval("All_Records")) == 0 ? false : true %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Count" HeaderStyle-Width="15px" ItemStyle-Width="15px"
                                                                    ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right"
                                                                    SortExpression="Count">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkPermissionDownload8" runat="server" CommandName="download"
                                                                            CommandArgument='<%# Eval("ID") + "/"  + "/" + Eval("Count") %>' Text='<%# Eval("Count") %>'
                                                                            Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                            OnCommand="lnkPermissionDownload_Command"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <CR:CrystalReportViewer ID="crv" runat="server" Width="100%" SeparatePages="False"
                                                            Visible="false" EnableDrillDown="False" DisplayToolbar="False" BestFitPage="True"></CR:CrystalReportViewer>
                                                    </td>
                                                </tr>
                                        </asp:PlaceHolder>
                                    </table>
                                    <br />
                                </div>
                                <div align="center" style="width: 100%">
                                    <br />
                                    <asp:Button ID="btnSaveCampaign" runat="server" CssClass="buttonMedium" OnClick="btnSaveToCampaign_Click"
                                        Text="Save to campaign" Visible="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnDownloadDetails" runat="server" CssClass="buttonMedium" OnClick="btnDownloadDetails_Click"
                                        Text="Download Details" Visible="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnPopupCloseDimension" runat="server" CssClass="button" OnClick="btnPopupCloseDimension_Click"
                                        Text="Close" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </asp:Panel>
                </asp:Panel>
                <%-- Pop up for CrossTab Reports --%>
                <asp:Button ID="btnShowCrossTabPopup" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="ModalPopupCrossTabReport" runat="server" TargetControlID="btnShowCrossTabPopup"
                    PopupControlID="pnlCrossTabPopup" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender7" runat="server" BehaviorID="RoundedCornersBehavior7"
                    TargetControlID="pnlCrossTabPopupRound" Radius="6" Corners="All" />
                <asp:Panel ID="pnlCrossTabPopup" runat="server" Width="1100px" Style="display: none" CssClass="modalPopup">
                    <asp:Panel ID="pnlCrossTabPopupRound" runat="server" Width="1000px" CssClass="modalPopup2">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="rpgCrossTab" />
                                <asp:PostBackTrigger ControlID="btnCloseCrossTab" />
                                <asp:PostBackTrigger ControlID="btnCrossTabXls" />
                                <asp:AsyncPostBackTrigger ControlID="btnLoadReport" />
                                <asp:AsyncPostBackTrigger ControlID="drpCrossTabReport" />
                            </Triggers>
                            <ContentTemplate>
                                <div align="center" style="text-align: center; height: 500px; padding: 0px 10px 0px 10px;">
                                    <br />
                                    <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold" colspan="2">CROSSTAB REPORT
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left" style="font-size: small">
                                                <asp:DropDownList ID="drpCrossTabReport" runat="server" Width="250px" DataTextField="CrossTabReportName" DataValueField="CrossTabReportID" OnSelectedIndexChanged="drpCrossTabReport_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpCrossTabReport" InitialValue="0"
                                                    ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="report"></asp:RequiredFieldValidator>
                                                <asp:HiddenField ID="hfRow" runat="server" Value="" />
                                                <asp:HiddenField ID="hfColumn" runat="server" Value="" />
                                                <asp:HiddenField ID="hfRowSearchValue" runat="server" Value="" />
                                                <asp:HiddenField ID="hfColumnSearchValue" runat="server" Value="" />
                                                <asp:HiddenField ID="hfSelectedCrossTabLink" runat="server" Value="" />
                                                <asp:PlaceHolder ID="phColFields" runat="server" Visible="false">
                                                    <asp:Label ID="lblColField" runat="server" />&nbsp;
                                                    <asp:TextBox ID="txtColSearchText" runat="server" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server"
                                                        Enabled="true" TargetControlID="txtColSearchText" WatermarkCssClass="watermarked"
                                                        WatermarkText="exact match, partial match, keyword" />
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phColZipFields" runat="server" Visible="false">
                                                    <asp:Label ID="lblColZipField" runat="server" />&nbsp;
                                                    <asp:TextBox ID="txtColZipFrom" runat="server" Width="60px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtColZipFrom"
                                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="report"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="* Enter valid Number" ValidationGroup="report" Display="Dynamic"
                                                        ControlToValidate="txtColZipFrom" ValidationExpression="^[0-9]*$" Font-Bold="True" ForeColor="Red"></asp:RegularExpressionValidator>
                                                    To&nbsp;&nbsp;<asp:TextBox ID="txtColZipTo" runat="server" Width="60px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtColZipTo"
                                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="report"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="* Enter valid Number" ValidationGroup="report" Display="Dynamic"
                                                        ControlToValidate="txtColZipTo" ValidationExpression="^[0-9]*$" Font-Bold="True" ForeColor="Red"></asp:RegularExpressionValidator>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phRowFields" runat="server" Visible="false">
                                                    <asp:Label ID="lblRowField" runat="server" />&nbsp;
                                                    <asp:TextBox ID="txtRowSearchText" runat="server" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                                        Enabled="true" TargetControlID="txtRowSearchText" WatermarkCssClass="watermarked"
                                                        WatermarkText="exact match, partial match, keyword" />
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phRowZipFields" runat="server" Visible="false">
                                                    <asp:Label ID="lblRowZipField" runat="server" />&nbsp;
                                                    <asp:TextBox ID="txtRowZipFrom" runat="server" Width="60px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtRowZipFrom"
                                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="report"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="* Enter valid Number" ValidationGroup="report" Display="Dynamic"
                                                        ControlToValidate="txtRowZipFrom" ValidationExpression="^[0-9]*$" Font-Bold="True" ForeColor="Red"></asp:RegularExpressionValidator>
                                                    To&nbsp;&nbsp;<asp:TextBox ID="txtRowZipTo" runat="server" Width="60px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRowZipTo"
                                                        ErrorMessage="*" Font-Bold="false" ForeColor="Red" ValidationGroup="report"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="* Enter valid Number" ValidationGroup="report" Display="Dynamic"
                                                        ControlToValidate="txtRowZipTo" ValidationExpression="^[0-9]*$" Font-Bold="True" ForeColor="Red"></asp:RegularExpressionValidator>
                                                </asp:PlaceHolder>
                                                &nbsp; &nbsp;<asp:Button ID="btnLoadReport" runat="server" CssClass="button" Text="Load Report" OnClick="btnLoadReport_Click" ValidationGroup="report" Visible="false" />
                                            </td>
                                        </tr>
                                        <asp:PlaceHolder ID="PhCrossTabReport" runat="server" Visible="false">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCrossTabReportName" runat="server" Style="text-transform: uppercase" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Button ID="btnCrossTabXls" runat="server" CssClass="button" Text="Download xls" OnClick="btnCrossTabXls_Click" />&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2">
                                                    <telerik:RadPivotGrid RenderMode="Lightweight" AllowPaging="false" PageSize="10" Height="365px" Width="955px" EnableAjaxSkinRendering="true"
                                                        ID="rpgCrossTab" runat="server" ColumnHeaderZoneText="ColumnHeaderZone" ShowColumnHeaderZone="false" ShowFilterHeaderZone="false" ShowDataHeaderZone="false" ShowRowHeaderZone="false" OnNeedDataSource="rpgCrossTab_NeedDataSource" OnCellDataBound="rpgCrossTab_CellDataBound">
                                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                        <TotalsSettings RowsSubTotalsPosition="First" RowGrandTotalsPosition="First" ColumnsSubTotalsPosition="None" ColumnGrandTotalsPosition="First" />
                                                        <ClientSettings EnableFieldsDragDrop="false">
                                                            <Scrolling AllowVerticalScroll="true"></Scrolling>
                                                        </ClientSettings>
                                                        <Fields>
                                                            <telerik:PivotGridRowField DataField="RowDesc"></telerik:PivotGridRowField>
                                                            <telerik:PivotGridColumnField DataField="ColDesc"></telerik:PivotGridColumnField>
                                                            <telerik:PivotGridAggregateField DataField="Counts" Aggregate="Sum">
                                                                <CellTemplate>
                                                                    <asp:LinkButton ID="lnkCrossTabCounts" runat="server" Text='<%# Container.DataItem %>' OnCommand="lnkCrossTabReportCounts_Command"></asp:LinkButton>
                                                                </CellTemplate>
                                                                <%--                                                                <ColumnGrandTotalCellTemplate>
                                                                    <asp:LinkButton ID="lnkCrossTabRowTotalCounts" runat="server" Text='<%# Container.DataItem %>' OnCommand="lnkCrossTabReportCounts_Command"></asp:LinkButton>
                                                                </ColumnGrandTotalCellTemplate>
                                                                <RowGrandTotalCellTemplate>
                                                                    <asp:LinkButton ID="lnkCrossTabColumnTotalCounts" runat="server" Text='<%# Container.DataItem %>' OnCommand="lnkCrossTabReportCounts_Command"></asp:LinkButton>
                                                                </RowGrandTotalCellTemplate>--%>
                                                            </telerik:PivotGridAggregateField>
                                                        </Fields>
                                                    </telerik:RadPivotGrid>
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                    </table>
                                    <br />
                                </div>
                                <div align="center" style="width: 100%">
                                    <asp:Label ID="lblCrossTabMsg" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label></br></br>
                                    <asp:Button ID="btnCloseCrossTab" runat="server" CssClass="button" OnClick="btnCloseCrossTab_Click" Text="Close" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </asp:Panel>
                </asp:Panel>
                <%-- Pop up for Dimensions --%>
                <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="mdlPopupDimension" runat="server" TargetControlID="btnShowPopup2"
                    PopupControlID="pnlPopupDimensions" CancelControlID="btnCloseDimensions" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" BehaviorID="RoundedCornersBehavior2"
                    TargetControlID="pnlPopupDimensionsRound" Radius="6" Corners="All" />
                <asp:Panel ID="pnlPopupDimensions" runat="server" Width="935px" Style="display: none"
                    CssClass="modalPopup">
                    <asp:Panel ID="pnlPopupDimensionsRound" runat="server" Width="935px" CssClass="modalPopup2">
                        <br />
                        <div align="left" style="text-align: center; padding: 0px 10px 0px 10px;">
                            <table width="100%" border="0" cellpadding="5" cellspacing="0" style="border: solid 1px #5783BD">
                                <tr style="background-color: #5783BD;">
                                    <td style="padding: 5px; font-size: 28px; color: #ffffff; font-weight: bold">
                                        <asp:Label ID="lblDimensionControl" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblDimensionType" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblDimension" runat="server" Text="" Style="text-transform: uppercase"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkSortByDescription" runat="server" Text="Sort By Description" CommandName="SortByDescription" OnCommand="lnkSort_Command" Visible="False"></asp:LinkButton>&nbsp&nbsp;
                                        <asp:LinkButton ID="lnkSortByValue" runat="server" Text="Sort By Value" CommandName="SortByValue" OnCommand="lnkSort_Command" Visible="False"></asp:LinkButton>&nbsp;&nbsp;
                                        <b>Search </b> 
                                        <telerik:RadTextBox ID="rtbDimSearch" runat="server" onkeypress="handleKeyDown(this,event)" SelectionLength="0" Visible="False">
                                        </telerik:RadTextBox>
                                        <asp:HiddenField runat="server" ID="hfResponseValue" Value="0"></asp:HiddenField>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <telerik:RadListBox ID="rlbDimensionAvailable" runat="server" Width="465px" Height="500px" SelectionMode="Multiple" OnClientDropped="OnClientDroppedHandler" OnClientTransferring="rlbDimensionAvailable_OnClientTransferring">
                                        </telerik:RadListBox>
                                        <telerik:RadListBox ID="rlbDimensionSelected" runat="server" Width="430px" Height="500px" SelectionMode="Multiple">
                                        </telerik:RadListBox>
                                    </td>
                                </tr>
                            </table>
                            <div align="center" style="width: 100%">
                            </div>
                            <br />
                        </div>
                        <div align="center" style="width: 100%">
                            <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="button" OnClick="btnSelect_Click" />
                            &nbsp;
                            <asp:Button ID="btnCloseDimensions" runat="server" Text="Close" CssClass="button" />
                        </div>
                        <br />
                    </asp:Panel>
                </asp:Panel>
                <%-- Pop up for save campaigns --%>
                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="mdlPopupSaveCampaign" runat="server" TargetControlID="Button1"
                    PopupControlID="Panel1" BackgroundCssClass="modalBackground" BehaviorID="BPopup4" />
                <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" BehaviorID="RoundedCornersBehavior4"
                    TargetControlID="Panel2" Radius="6" Corners="All" />
                <asp:Panel ID="Panel1" runat="server" Width="500px" Style="display: none" CssClass="modalPopup">
                    <asp:Panel ID="Panel2" runat="server" Width="500px" CssClass="modalPopup2">
                        <div id="Div3" style="text-align: center; padding: 10px 10px 10px 10px;"
                            runat="server">
                            <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                                <tr style="background-color: #5783BD;">
                                    <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">
                                        <asp:Label ID="lblPopupMasterID" Visible="false" runat="server"></asp:Label>
                                        Save Campaign
                                    </td>
                                </tr>
                                <tr>
                                    <td>PromoCode
                                        <asp:TextBox ID="txtPromocode" runat="server" Width="100" MaxLength="10"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="Server" ForeColor="Red" Visible="false"></asp:Label>
                                        <div id="divSaveCampaignPopupError" runat="Server" visible="false" style="width: 400px">
                                            <table cellspacing="0" cellpadding="0" width="400px" align="center">
                                                <tr>
                                                    <td id="Td1"></td>
                                                </tr>
                                                <tr>
                                                    <td id="Td2" width="100%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td valign="top" align="center" width="20%">
                                                                    <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                                        alt="" />
                                                                </td>
                                                                <td valign="middle" align="left" width="80%" height="100%">
                                                                    <asp:Label ID="lblSaveCampaignPopupError" runat="Server" ForeColor="Red"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="Td3"></td>
                                                </tr>
                                            </table>
                                            <br />
                                        </div>
                                    </td>
                                </tr>
                                <asp:PlaceHolder ID="PlaceHolder5" runat="server" Visible="true">
                                    <tr>
                                        <td style="padding-left: 30px" align="left">
                                            <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                            <tr>
                                                                <td align="left" width="15%">
                                                                    <asp:RadioButton ID="rbPopupNewCampaign" runat="Server" Text=" Create new Campaign</span>"
                                                                        GroupName="grpPopupCampaignList" AutoPostBack="true" OnCheckedChanged="rbPopupNewCampaign_CheckedChanged"></asp:RadioButton>
                                                                </td>
                                                            </tr>

                                                            <asp:PlaceHolder ID="plPopupNewCampaign" runat="server" Visible="false">
                                                                <tr>
                                                                    <td style="padding-left: 30px">
                                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                            <tr>
                                                                                <td>Campaign Name
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPopupCampaignName" runat="server" Width="200" MaxLength="100"></asp:TextBox>&nbsp;
                                                                                    <asp:RequiredFieldValidator ID="rvPopupCampaignName" runat="server" Font-Size="xx-small"
                                                                                        ControlToValidate="txtPopupCampaignName" ErrorMessage="* required" Font-Italic="True"
                                                                                        Font-Bold="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </asp:PlaceHolder>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                            <tr>
                                                                <td align="left" width="15%">
                                                                    <asp:RadioButton ID="rbPopupExistingCampaign" runat="Server" Text=" Use Existing Campaign"
                                                                        GroupName="grpPopupCampaignList" AutoPostBack="true" OnCheckedChanged="rbPopupExistingCampaign_CheckedChanged"
                                                                        Visible="true"></asp:RadioButton>
                                                                </td>
                                                            </tr>
                                                            <asp:PlaceHolder ID="plPopupExistingCampaign" runat="server" Visible="false">
                                                                <tr>
                                                                    <td style="padding-left: 30px">
                                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                            <tr>
                                                                                <td>Campaign Name
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpPopupCampaignName" runat="server" DataTextField="CampaignName"
                                                                                        DataValueField="CampaignID" Width="200px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </asp:PlaceHolder>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblPopupResult" runat="server" Text="" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnPopupSaveCampaign" Text="Save" CssClass="button" runat="server" OnClick="btnPopupSaveCampaign_click"
                                            ValidationGroup="save" />
                                        <asp:Button ID="btnPopupCloseCampaign" Text="Close" CssClass="button" runat="server" OnClick="btnPopupCloseCampaign_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <div style="width: 100%">
                    <table width="100%" cellpadding="1" cellspacing="1" border="1">
                        <tr>
                            <td width="15%" valign="top" style="padding: 3px 3px 3px 3px">
                                <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                    <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                                        <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                            <td valign="middle" align="center">
                                                <b>Brand
                                                    <asp:HiddenField ID="hfConfirmValue" runat="server" />
                                                    <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label>
                                                    <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label><br />
                                                    <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                                        DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfBrandName" runat="server" Value="" /></td>
                                        </tr>
                                        <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                            <td valign="middle" align="center">
                                                <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image><br />
                                        </tr>
                                    </asp:Panel>
                                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                        <td valign="middle" align="center">
                                            <b>Product<br />
                                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="rcbProduct" DataTextField="PubName" DataValueField="PubID" autopostback ="true" 
                                                    EnableLoadOnDemand="True" OnItemsRequested="rcbProduct_ItemsRequested" Height="200px" Width="200px" DropDownAutoWidth="Enabled" OnSelectedIndexChanged="rcbProduct_SelectedIndexChanged" Filter="Contains">
                                                </telerik:RadComboBox>
                                                <br />
                                                <%--<asp:TextBox runat="server" ID="txtProduct" Width="150px" OnTextChanged="txtProduct_TextChanged"
                                                    AutoPostBack="true" Font-Names="Arial" Font-Size="x-small" />
                                                <asp:AutoCompleteExtender runat="server" BehaviorID="AutoCompleteEx1" ID="AutoCompleteExtender2"
                                                    TargetControlID="txtProduct" ServicePath="~/Main/Widgets/SearchProduct.asmx"
                                                    ServiceMethod="GetProductList" MinimumPrefixLength="1" CompletionInterval="100"
                                                    EnableCaching="true" CompletionSetCount="20" FirstRowSelected="true" CompletionListCssClass="AutoExtender"
                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight" OnClientItemSelected="ace_itemSelected">
                                                </asp:AutoCompleteExtender>--%>
                                                <asp:HiddenField ID="hfProductID" runat="server" Value="0" /></td>
                                    </tr>
                                    <asp:Panel ID="pnlDataCompare" runat="server" Visible="false">
                                        <tr>
                                            <td valign="middle" align="center">
                                                <br /><br />
                                                <b>Data Compare File
                                                <asp:Label ID="lblFile" runat="server" Visible="false"></asp:Label><br />
                                                    <asp:DropDownList ID="drpDataCompareSourceFile" runat="server" Font-Names="Arial" 
                                                        AutoPostBack="true" Style="text-transform: uppercase" DataTextField="FileName"
                                                        DataValueField="ProcessCode" Width="200px" OnSelectedIndexChanged="drpDataCompareSourceFile_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfDataCompareProcessCode" runat="server" Value="" />
                                                    <asp:HiddenField ID="hfDcRunID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfIsBillable" runat="server" Value="false" />
                                                    <asp:HiddenField ID="hfDcViewID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfDcTargetCodeID" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hfDataCompareLinkSelected" runat="server" Value="0" /></td>
                                        </tr>
                                        <asp:PlaceHolder ID="plDataCompareData" runat="server" Visible="false">
                                            <tr>
                                                <td valign="middle" align="center">
                                                    <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                                        <tr>
                                                            <td  align="right" width="70%">
                                                                <asp:Label ID="lblTotalRecordsHeader" runat="server" Text="Total Records : "></asp:Label>
                                                            </td>
                                                            <td  width="15%">
                                                                <asp:Label ID="lblTotalFileRecords" runat="server" Text=""></asp:Label>
                                                            </td>
                                                            <td width="15%"></td>
                                                        </tr>
                                                        <tr>
                                                            <td  align="right" >
                                                                <asp:Label ID="lblMatchedRecordHeader" runat="server" Text="Matched in <br/> Selected Product : "></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkMatchedRecords" runat="server" Text="" OnCommand="lnkDataCompare_Command" CommandArgument="MatchedRecords" />
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkMatchedSummary" runat="server" Text="Summary" OnCommand="lnkDataCompareSummary_Command" CommandArgument="Matched"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td  align="right">
                                                                <asp:Label ID="lblMatchedNotInHeader" runat="server" Text="Matched Not in <br/> Selected Product : "></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkMatchedNotIn" runat="server" Text="" OnCommand="lnkDataCompare_Command" CommandArgument="MatchedNotInProduct" />
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkMatchedNotInSummary" runat="server" Text="Summary" OnCommand="lnkDataCompareSummary_Command" CommandArgument="MatchedNotInProduct"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td  align="right">
                                                                <asp:Label ID="lblNonMatchedRecordsHeader" runat="server" Text="Non Matched in <br/>Selected Product : "></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkNonMatchedRecords" runat="server" Text="" OnCommand="lnkDataCompare_Command" CommandArgument="NonMatchedRecords" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:RadioButtonList ID="rblDataCompareOperation" runat="server" RepeatDirection="Horizontal" DataValueField="CodeID" DataTextField="CodeName" style= "vertical-align:top;">
                                                    </asp:RadioButtonList><br />
                                                </td>
                                            </tr>
                                        </asp:PlaceHolder>
                                    </asp:Panel>
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:DataList ID="dlDimensions" runat="server" AlternatingItemStyle-Width="50%" ItemStyle-Width="50%"
                                                RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Table" ItemStyle-BorderWidth="0"
                                                Width="100%" OnItemDataBound="dlDimensions_ItemDataBound">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="lnkDimensionPopup" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlDimHeader" runat="server" Height="28px" Width="100%" BackColor="#e3e3e3" ForeColor="Black">
                                                                <div style="padding: 5px 5px 5px 5px; width: 240px; text-align: center;">
                                                                    <asp:LinkButton ID="lnkDimensionPopup" runat="server" Text='<%# Eval("DisplayName")%>' CommandArgument='<%#Eval("ResponseGroupID") + "|DIMENSION|" + Eval("DisplayName")%>' OnCommand="lnkPopup_Command"></asp:LinkButton>
                                                                    <asp:Label ID="lblResponseGroup" Text='<%#Eval("DisplayName") %>' runat="server" Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hfResponseGroupID" runat="server" Value='<%#Eval("ResponseGroupID") %>' />
                                                                    <asp:LinkButton ID="lnkDimensionShowHide" runat="server" Text="(Show...)" Font-Size="XX-Small" ForeColor="Black" Font-Underline="false" CommandArgument='<%#Eval("ResponseGroupID") + "|DIMENSION|" + Eval("DisplayName")%>' OnCommand="lnkDimensionShowHide_Command"></asp:LinkButton>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlDimBody" runat="server" Visible="false">
                                                                <div style="padding: 2px 5px 5px 3px;">
                                                                    <asp:ListBox ID="lstResponse" runat="server" SelectionMode="Multiple" Width="100%"
                                                                        Rows="5" Font-Names="Arial" Font-Size="x-small"></asp:ListBox>
                                                                </div>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="85%" valign="top" style="padding: 3px 3px 3px 3px">
                                <asp:Panel ID="pnlGlobalFilterHeader" runat="server" CssClass="collapsePanelHeader"
                                    Height="28px">
                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                        <div style="float: left;">
                                            Standard Filters
                                        </div>
                                        <div style="float: left; margin-left: 20px;">
                                            <asp:Label ID="pnlGlobalFilterLabel" runat="server">(Show Details...)</asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:ImageButton ID="pnlGlobalFilterImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                AlternateText="(Show Details...)" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlGlobalFilterBody" runat="server" CssClass="collapsePanel" Height="0"
                                    BorderColor="#5783BD" BorderWidth="1">
                                    <table border="0" bordercolor="#cccccc" cellpadding="1" cellspacing="1" width="100%">
                                        <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                            <td valign="middle" align="center">
                                                <asp:LinkButton ID="lnkGeoCode" runat="server" CommandName="lstGeoCode" CommandArgument="Geo Code"
                                                    Text="Geo Code" OnCommand="lnkPopup_Command" Font-Bold="true"></asp:LinkButton>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:LinkButton ID="lnkState" runat="server" CommandName="lstState" CommandArgument="State"
                                                    Text="State" OnCommand="lnkPopup_Command" Font-Bold="true"></asp:LinkButton>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:LinkButton ID="lnkCountryRegions" runat="server" CommandName="lstCountryRegions"
                                                    CommandArgument="Country Regions" Text="Country Regions" OnCommand="lnkPopup_Command"
                                                    Font-Bold="true"></asp:LinkButton>
                                            </td>
                                            <td valign="middle" align="center">
                                                <asp:LinkButton ID="lnkCountry" runat="server" CommandName="lstCountry" CommandArgument="Country"
                                                    Text="Country" OnCommand="lnkPopup_Command" Font-Bold="true"></asp:LinkButton>
                                            </td>
                                            <td valign="middle" align="center">
                                                <b>
                                                    <asp:Label ID="lblGeolocation" runat="server" Text="GeoLocation" Font-Bold="true" /></b>
                                            </td>
                                            <td align="center">
                                                <b>Filters</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="center">
                                                <asp:ListBox ID="lstGeoCode" runat="server" Rows="6" DataValueField="RegionGroupID" DataTextField="RegionGroupName"
                                                    Style="text-transform: uppercase" AutoPostBack="true" SelectionMode="Multiple"
                                                    Font-Size="x-small" Font-Names="Arial" Width="130px" OnSelectedIndexChanged="lstGeoCode_SelectedIndexChanged"></asp:ListBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:ListBox ID="lstState" runat="server" Rows="6" DataValueField="RegionCode" DataTextField="RegionCode"
                                                    Style="text-transform: uppercase" SelectionMode="Multiple" Font-Size="x-small"
                                                    Font-Names="Arial" Width="55px"></asp:ListBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:ListBox ID="lstCountryRegions" runat="server" Rows="6" DataValueField="Area"
                                                    DataTextField="Area" Style="text-transform: uppercase" AutoPostBack="true"
                                                    SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial" Width="130px"
                                                    OnSelectedIndexChanged="lstCountryRegions_SelectedIndexChanged"></asp:ListBox>
                                            </td>
                                            <td valign="top" align="center">
                                                <asp:ListBox ID="lstCountry" runat="server" Rows="6" DataValueField="CountryID" DataTextField="ShortName"
                                                    Style="text-transform: uppercase" SelectionMode="Multiple" Font-Size="x-small"
                                                    Font-Names="Arial" Width="150px"></asp:ListBox>
                                            </td>
                                            <td width="16%">
                                                <table border="0" cellspacing="1" cellpadding="0" width="100%">
                                                    <tr>
                                                        <td style="font-size: x-small" width="45%" align="right">COUNTRY&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="drpCountry" runat="server" OnSelectedIndexChanged="drpCountry_SelectedIndexChanged" AutoPostBack="true">
                                                                <asp:ListItem Value="UNITED STATES">United States</asp:ListItem>
                                                                <asp:ListItem Value="CANADA">Canada</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: x-small" width="45%" align="right">ZIP CODE&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <telerik:RadMaskedTextBox ID="RadMtxtboxZipCode" Width="70px" Height="18px" runat="server" Mask="#####">
                                                            </telerik:RadMaskedTextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: x-small" align="right">RANGE (MIN)&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRadiusMin" runat="server" Width="40px"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" MaximumValue="600"
                                                                MinimumValue="0" ControlToValidate="txtRadiusMin" Type="Integer"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: x-small" align="right">RANGE (MAX)&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRadiusMax" runat="server" Width="40px"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="*" MaximumValue="600"
                                                                MinimumValue="1" ControlToValidate="txtRadiusMax" Type="Integer"></asp:RangeValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="font-size: xx-small" align="middle" colspan="2">Range (0 to 600 miles)&nbsp;&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center" valign="middle">
                                                <asp:LinkButton ID="lnkCirculation" runat="server" CommandName="lnkCirculation"
                                                    Text="Circulation" OnCommand="lnkCirculation_Command"></asp:LinkButton>&nbsp; 
                                                <asp:LinkButton ID="lnkActivity" runat="server" CommandName="lnkActivity"
                                                    Text="Activity" OnCommand="lnkActivity_Command"></asp:LinkButton>&nbsp;   
                                                <asp:LinkButton ID="lnkAdhoc" runat="server" CommandName="lnkAdhoc"
                                                    Text="Adhoc" OnCommand="lnkAdhoc_Command"></asp:LinkButton>&nbsp;
                                                <asp:LinkButton ID="lnkSavedFilter" runat="server" CommandName="lnkFilter"
                                                    Text="Saved" OnCommand="lnkSavedFilter_Command"></asp:LinkButton>&nbsp;  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="11">
                                                <table border="0" bordercolor="#cccccc" cellpadding="2" cellspacing="1" width="100%">
                                                    <tr style="background-color: #5783BD; color: White; padding: 0px 0px 0px 0px; height: 20px;">
                                                        <td class="labelsmall" style="color: white" colspan="4" align="center">
                                                            <b>Contact Fields </b>
                                                        </td>
                                                        <td class="labelsmall" style="color: white" colspan="7" align="center">
                                                            <b>Permissions</b>
                                                        </td>
                                                        <td class="labelsmall" style="color: white" align="center">
                                                            <b></b>
                                                        </td>
                                                    </tr>
                                                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                        <td align="center">
                                                            <b>Email</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Phone</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Fax</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Media</b>
                                                        </td>
                                                        <%--                                                        <td align="center">
                                                            <b>Is Mailable</b>
                                                        </td>--%>
                                                        <td align="center">
                                                            <b>Mail</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Fax</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Phone</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Other Products</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>3rd Party</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Email Renew</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Text</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>Email Status</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbEmail" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbPhone" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbFax" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbMedia" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="PRINT" Value="A" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="DIGITAL" Value="B" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="BOTH" Value="C" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="OPT OUT" Value="O" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbMailPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbFaxPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbPhonePermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbOtherProductsPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbThirdPartyPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbEmailRenewPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbTextPermission" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="Blank" Value="-1" Font-Names="Arial" Font-Size="x-small" />
                                                                </Items>
                                                            </telerik:RadComboBox>
                                                        </td>
                                                        <td align="center" valign="top">
                                                            <telerik:RadComboBox ID="rcbEmailStatus" runat="server" CheckBoxes="true" DataValueField="EmailStatusID" DataTextField="Status" Font-Names="Arial" Font-Size="x-small">
                                                            </telerik:RadComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlButtons" runat="server" BorderColor="#5783BD" BorderWidth="0">
                                    <table border="0" bordercolor="#cccccc" cellpadding="5" cellspacing="1" width="100%">
                                        <tr>
                                            <td align="center" valign="middle">
                                                <asp:Button ID="btnAddFilter" Text="Add Filters" runat="server" CssClass="button"
                                                    OnClick="btnAddFitler_Click"></asp:Button>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkResetAll" runat="server" Text="&lt;img src='../images/btnResetAllFilters.png' border='0' style='vertical-align:middle' &gt;" OnClick="lnkResetAll_Click" ></asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <div id="divErrorMsg" runat="Server" visible="false">
                                                    <asp:Label ID="lblErrorMsg" runat="Server" ForeColor="Red"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlGlobalFilterBody"
                                    ExpandControlID="pnlGlobalFilterHeader" CollapseControlID="pnlGlobalFilterHeader"
                                    Collapsed="false" TextLabelID="pnlGlobalFilterLabel" ImageControlID="pnlGlobalFilterImage"
                                    ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                    CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                <br />
                                <ajaxToolkit:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" Style="text-align: left"
                                    AutoPostBack="true" OnActiveTabChanged="TabContainer_OnActiveTabChanged">
                                    <ajaxToolkit:TabPanel ID="tpFilters" runat="server" HeaderText="Filters">
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="bpResults" runat="server" Width="100%">
                                                        <table border="0" cellpadding="1" cellspacing="1" width="100%">
                                                            <tr>
                                                                <td></td>
                                                                <td colspan="2" align="center">
                                                                    <asp:RadioButtonList ID="rblLoadType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblLoadType_SelectedIndexChanged" AutoPostBack="true">
                                                                        <asp:ListItem Value="Auto Load" Selected="True">Auto Load</asp:ListItem>
                                                                        <asp:ListItem Value="Manual Load">Manual Load</asp:ListItem>
                                                                    </asp:RadioButtonList>&nbsp;&nbsp;
                                                                    <asp:Button ID="btnLoadComboVenn" runat="server" CssClass="button" OnClick="btnLoadComboVenn_Click" Text="Load Report"  Visible="false" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="55%" valign="top">
                                                                    <asp:HiddenField ID="hfFilterNo" runat="server" />
                                                                    <asp:HiddenField ID="hfFilterName" runat="server" />
                                                                    <asp:HiddenField ID="hfFilterGroupName" runat="server" />
                                                                    <asp:GridView ID="grdFilters" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        Height="100%" OnRowDataBound="grdFilters_RowDataBound" OnRowDeleting="grdFilters_RowDeleting" OnRowEditing="grdFilters_RowEditing" OnRowCancelingEdit="grdFilters_RowCancelingEdit"
                                                                        ShowFooter="true" AllowPaging="false" DataKeyNames="FilterNo" OnRowCommand="grdFilters_RowCommand" OnPreRender="grdFilters_PreRender">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="2%" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("FilterNo") %>' Font-Bold="true"></asp:Label>
                                                                                    <asp:Label ID="lblBrandID" runat="server" Text='<%# Eval("BrandID") %>' Visible="false"></asp:Label>
                                                                                    <asp:HiddenField ID="hfViewType" runat="server" Value='<%# Eval("ViewType") %>'></asp:HiddenField>
                                                                                    <asp:HiddenField ID="hfFilterGroupName" runat="server" Value='<%# Eval("FilterGroupName") %>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Product" HeaderStyle-Width="2%" ItemStyle-Width="2%" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPubID" runat="server" Text='<%# Eval("PubID") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Filters" HeaderStyle-Width="84%" ItemStyle-Width="84%"
                                                                                ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel ID="pnlResultHeader" runat="server" Height="28px" CssClass="collapsePanelHeader"
                                                                                        BackColor="#eeeeee" ForeColor="Black">
                                                                                        <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                                                                            <div style="float: left;">
                                                                                            </div>
                                                                                            <div style="float: left; margin-left: 20px;">
                                                                                                <asp:Label ID="lblFiltername" runat="server" Text='<%# Eval("FilterName") %>'></asp:Label>
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
                                                                                        <div style="width: 524px; overflow-x:auto;">
                                                                                        <asp:GridView ShowHeader="false" ID="grdFilterValues" Width="100%" runat="server"
                                                                                            AutoGenerateColumns="False" GridLines="Both"  OnRowDataBound="grdFilterValues_RowDataBound">
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
                                                                                                        <asp:Label ID="lblFilterText" runat="server" Text='<%# Eval("name").ToString() == "Open Activity" ||  Eval("name").ToString() == "Click Activity" ||  Eval("name").ToString() == "Open Email Sent Date" ||  Eval("name").ToString() == "Click Email Sent Date" ||  Eval("name").ToString() == "Visit Activity"   ? "" : Eval("Text") %>'></asp:Label>
                                                                                                        <asp:Label ID="lblAdhocCondition" runat="server" Text='<%# Eval("name").ToString() == "Adhoc" || Eval("name").ToString() == "Open Activity" ||  Eval("name").ToString() == "Click Activity"  ||  Eval("name").ToString() == "Open Email Sent Date" ||  Eval("name").ToString() == "Click Email Sent Date" ||  Eval("name").ToString() == "Visit Activity"  ? Eval("name").ToString() == "Open Activity" ||  Eval("name").ToString() == "Click Activity" ||  Eval("name").ToString() == "Open Email Sent Date" ||  Eval("name").ToString() == "Click Email Sent Date" ||  Eval("name").ToString() == "Visit Activity"  ? Eval("SearchCondition")  + " - " + Eval("Values") :  " - " + Eval("SearchCondition") + " - " + Eval("Values") : "" %>'></asp:Label>
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
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>'
                                                                                        Text='<%# Eval("Count") %>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                        OnCommand="lnkdownload_Command"></asp:LinkButton>
                                                                                </ItemTemplate>
<%--                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblAllUnion" runat="server" Text=""></asp:Label>&nbsp;
                                                                                    <asp:LinkButton ID="lnkdownloadAllUnion" runat="server" OnCommand="lnkdownloadAllUnion_Command"></asp:LinkButton>&nbsp; &nbsp; 
                                                                                    <asp:Label ID="lblAllIntersect" runat="server" Text=""></asp:Label>&nbsp;
                                                                                    <asp:LinkButton ID="lnkdownloadAllIntersect" runat="server" OnCommand="lnkdownloadAllIntersect_Command"></asp:LinkButton>
                                                                                </FooterTemplate>--%>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("FilterNo")%>'
                                                                                        Text="<img src='../images/ic-edit.gif' style='border:none;'>"> </asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" CommandArgument='<%# Eval("FilterNo")%>'
                                                                                        Text="<img src='../images/ic-cancel.gif' style='border:none;'>" Visible="false"> </asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField ButtonType="Link" Text="<img src='../images/icon-delete.gif' style='border:none;'>"
                                                                                CommandName="Delete" HeaderText="Remove" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                                ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="2%" ItemStyle-Width="2%" />
                                                                            <%-- <asp:ButtonField ButtonType="Link" Text="<img src='../images/icon-save.gif' style='border:none;'>"
                                                            CommandName="Save" HeaderText="Save Filter" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="2%" ItemStyle-Width="2%" />--%>
                                                                            <asp:TemplateField HeaderText="Save" HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelectFilter" runat="server" Text="" Checked="false" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                                                                        <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                                                                    <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                        <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                                Text="Geographical Permission - Canada" CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                                Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                                Text="Geographical Permission - International" CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                                CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkGeoMaps_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                                    <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                                                                    <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("FilterNo") + "//Single/" + "/" + Eval("FilterNo")  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <br />
                                                                    <div id="Div1" runat="server" style="text-align: right">
                                                                        <asp:Button ID="btnOpenSaveFilterPopup" runat="server" CssClass="buttonMedium" OnClick="btnOpenSaveFilterPopup_Click"
                                                                            Text="Save Filter" />
                                                                    </div>
                                                                </td>
                                                                <td width="20%" valign="top">
                                                                    <asp:GridView ID="grdFilterCounts" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="SelectedFilterNo" OnRowDataBound="grdFilterCounts_RowDataBound"
                                                                        OnRowCommand="grdFilterCounts_RowCommand" OnRowCreated="grdFilterCounts_RowCreated"  Visible="false">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Filters" DataField="FilterDescription" HeaderStyle-Width="33%"
                                                                                ItemStyle-Width="33%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                                                FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                                            <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                        Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                        OnCommand="lnkCount_Command"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                                                                        <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                                                <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                                                    PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                                                                    <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                        <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")+ "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                                Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                                Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                                Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription") + "/" + Eval("Count")%>' OnCommand="lnkGeoMaps_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                    PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                                    <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                                <td width="25%" align="center" valign="top">
                                                                    <uc:Venn runat="server" ID="ctrlVenn1" Visible="false"></uc:Venn>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel ID="tpFilterSegmentation" runat="server" HeaderText="Filter Segmentation">
                                        <ContentTemplate>
                                                <uc:FilterSegmentation runat="server" ID="FilterSegmentation" Visible="false"></uc:FilterSegmentation>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                    <ajaxToolkit:TabPanel ID="tpFilterIntersection" runat="server" HeaderText="Filter Calculations">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="1" cellspacing="1" width="50%">
                                                <tr>
                                                    <td align="center" colspan="3">
                                                        <ajaxToolkit:TabContainer ID="tcFilterCalcualtion" runat="server" ActiveTabIndex="0" Style="text-align: left" AutoPostBack="true">
                                                                <ajaxToolkit:TabPanel ID="tpIntersect" runat="server" HeaderText="Intersect">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="grdCrossTabIntersect" SkinID="skingridAuto" runat="server" Width="100%"
                                                                            AutoGenerateColumns="true" Height="100%" ShowFooter="false" AllowPaging="false" OnRowDataBound="grdCrossTabIntersect_RowDataBound"
                                                                            Visible="true">
                                                                        </asp:GridView>
                                                                        <br /><br />
                                                                        <asp:GridView ID="grdFilterInterSectCount" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="SelectedFilterNo" OnRowDataBound="grdFilterCounts_RowDataBound"
                                                                        OnRowCommand="grdFilterCounts_RowCommand" OnRowCreated="grdFilterCounts_RowCreated" Visible="false">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Filters" DataField="FilterDescription" HeaderStyle-Width="33%"
                                                                                ItemStyle-Width="33%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                                                FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                                            <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                        Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                        OnCommand="lnkCount_Command"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                                                                        <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                                                                    <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                        <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")+ "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                                Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                                Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                                Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoMaps_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                                                        <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                                                                    <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    </ContentTemplate>
                                                                </ajaxToolkit:TabPanel>
                                                                <ajaxToolkit:TabPanel ID="tpUnion" runat="server" HeaderText="Union">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="grdCrossTabUnion" SkinID="skingridAuto" runat="server" Width="100%"
                                                                            AutoGenerateColumns="true" Height="100%" ShowFooter="false" AllowPaging="false" OnRowDataBound="grdCrossTabUnion_RowDataBound"
                                                                            Visible="true">
                                                                        </asp:GridView>
                                                                        <br /><br />
                                                                        <asp:GridView ID="grdFilterUnionCount" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="SelectedFilterNo" OnRowDataBound="grdFilterCounts_RowDataBound"
                                                                        OnRowCommand="grdFilterCounts_RowCommand" OnRowCreated="grdFilterCounts_RowCreated" Visible="false">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Filters" DataField="FilterDescription" HeaderStyle-Width="33%"
                                                                                ItemStyle-Width="33%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                                                FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                                            <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                        Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                        OnCommand="lnkCount_Command"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                                                                        <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                                                                    <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                        <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")+ "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                                Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                                Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                                Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoMaps_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                                                        <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                                                                    <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </ajaxToolkit:TabPanel>
                                                                <ajaxToolkit:TabPanel ID="tpNotIn" runat="server" HeaderText="Not In">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="grdCrossTabNotIn" SkinID="skingridAuto" runat="server" Width="100%"
                                                                            AutoGenerateColumns="true" Height="100%" ShowFooter="false" AllowPaging="false"
                                                                            Visible="true">
                                                                        </asp:GridView>
                                                                        <br /><br />
                                                                        <asp:GridView ID="grdFilterNotInCount" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                        Height="100%" ShowHeader="true" AllowPaging="false" DataKeyNames="SelectedFilterNo" OnRowDataBound="grdFilterCounts_RowDataBound"
                                                                        OnRowCommand="grdFilterCounts_RowCommand" OnRowCreated="grdFilterCounts_RowCreated" Visible="false">
                                                                        <Columns>
                                                                            <asp:BoundField HeaderText="Filters" DataField="FilterDescription" HeaderStyle-Width="33%"
                                                                                ItemStyle-Width="33%" ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle"
                                                                                FooterStyle-HorizontalAlign="center" FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left" />
                                                                            <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                        Text='<%# Eval("Count")%>' Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>'
                                                                                        OnCommand="lnkCount_Command"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Reports" HeaderStyle-Width="33%" ItemStyle-Width="33%"
                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" FooterStyle-HorizontalAlign="center"
                                                                                FooterStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Panel CssClass="popitmenu" ID="PopupMenu" runat="server">
                                                                                        <div id="divFilterCountsReport" runat="server" style="border: 1px outset white; padding: 2px; width: 175px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="DimensionReport" Text="Demographic Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkDimensionReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton9" runat="server" CommandName="CrossTabReport" Text="CrossTab Reports"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCrossTabReport_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgReport" runat="server" ImageUrl="~/images/icon-reports.gif" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme2" runat="Server" HoverCssClass="popupHover" PopupControlID="PopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgReport" PopDelay="25" />
                                                                                    <asp:Panel CssClass="popitmenu" ID="GeoPopupMenu" runat="server">
                                                                                        <div id="divGeoReport" runat="server" style="border: 1px outset white; padding: 2px; width: 300px; background-color: #eeeeee; text-align: left">
                                                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="GeoCanada" Text="Geographical BreakDown - Canada"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="GeoDomestic" Text="Geographical BreakDown - Domestic"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="GeoInternational" Text="Geographical BreakDown - International"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation")+ "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="GeoPermissionCanada"
                                                                                                Text="Geographical Permission - Canada" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="GeoPermissionDomestic"
                                                                                                Text="Geographical Permission - Domestic" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="GeoPermissionInternational"
                                                                                                Text="Geographical Permission - International" CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>'
                                                                                                OnCommand="lnkGeoReport_Command" />
                                                                                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="GeoLocation" Text="GeoLocation (Maps)"
                                                                                                CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkGeoMaps_Command" />
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                    <asp:Image ID="imgGeo" runat="server" ImageUrl="~/images/ic-geo.jpg" />&nbsp;&nbsp;
                                                                                    <asp:HoverMenuExtender ID="hme3" runat="Server" HoverCssClass="popupHover" PopupControlID="GeoPopupMenu"
                                                                                        PopupPosition="Right" TargetControlID="imgGeo" PopDelay="25" />
                                                                                                        <asp:LinkButton ID="lnkCompamyLocationView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkCompanyLocationView_Command"><img src="../Images/ic-companyview.png" alt="" style="border:none;" /></asp:LinkButton>&nbsp;
                                                                                    <asp:LinkButton ID="lnkEmailView" runat="server" Enabled='<%# Convert.ToInt32(Eval("Count")) == 0 ? false : true %>' CommandArgument='<%# Eval("SelectedFilterNo") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SelectedFilterOperation")  + "/" + Eval("SuppressedFilterOperation") + "/" + Eval("FilterDescription")  + "/" + Eval("Count")%>' OnCommand="lnkEmailView_Command"><img src="../Images/ic-EmailView.png" alt="" style="border:none;" /></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    </ContentTemplate>
                                                                </ajaxToolkit:TabPanel>
                                                        </ajaxToolkit:TabContainer>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </ajaxToolkit:TabPanel>
                                </ajaxToolkit:TabContainer>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <asp:TextBox ID="txtShowQuery" runat="server" Visible="false" TextMode="MultiLine"
                    Rows="10" Width="85%"></asp:TextBox>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <contenttemplate>
                <uc:Download runat="server" ID="DownloadPanel1" Visible="false"></uc:Download>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
            <contenttemplate>
                <uc:CLDownload runat="server" ID="CLDownloadPanel" Visible="false"></uc:CLDownload>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
            <contenttemplate>
                <uc:EVDownload runat="server" ID="EVDownloadPanel" Visible="false"></uc:EVDownload>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <contenttemplate>
                <uc:Adhoc runat="server" ID="AdhocFilter" Visible="false"></uc:Adhoc>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <contenttemplate>
                <uc:Activity runat="server" ID="ActivityFilter" Visible="false"></uc:Activity>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <contenttemplate>
                <uc:FilterSave runat="server" ID="FilterSave" Visible="false"></uc:FilterSave>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <contenttemplate>
                <uc:ShowFilter runat="server" ID="ShowFilter" Visible="false"></uc:ShowFilter>
            </contenttemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <uc:Circulation runat="server" ID="CirculationFilter" Visible="false"></uc:Circulation>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <uc:DataCompareSummary runat="server" ID="DataCompareSummary" Visible="false"></uc:DataCompareSummary>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <telerik:RadTreeView ID="RadTreeView1" runat="server" Style="display: none;">
    </telerik:RadTreeView>
    <telerik:RadDropDownTree ID="RadDropDownTree1" runat="server" Style="display: none;">
    </telerik:RadDropDownTree>
    <telerik:RadGrid ID="RadGrid" runat="server" Style="display: none;">
    </telerik:RadGrid>
</asp:Content>

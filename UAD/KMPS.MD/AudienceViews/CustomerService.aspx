<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="CustomerService.aspx.cs" Inherits="KMPS.MD.Main.CustomerService" ValidateRequest="false" %>

<%@ Register TagName="Adhoc" TagPrefix="uc" Src="~/Controls/Adhoc.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Activity" TagPrefix="uc" Src="~/Controls/Activity.ascx" %>
<%@ Register TagName="Circulation" TagPrefix="uc" Src="~/Controls/Circulation.ascx" %>
<%@ Register TagName="ShowFilter" TagPrefix="uc" Src="~/Controls/FiltersListPanel.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
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
            border: solid 1px #c0c0c0;
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

        .modalPopupUA1 {
            background-color: transparent;
            padding: 1em 6px;
            z-index: 10002 !important;
        }

        .modalPopupUA2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
            z-index: 10002 !important;
        }

        .popupborder {
            border: solid 2px #5783BD;
        }

        .poptableborder {
            border: solid 1px #5783BD;
        }

        select[disabled='disabled'] {
            font-weight: bold;
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

        .RadListBox span.rlbText em {
            background-color: #E5E5E5;
            font-weight: bold;
            font-style: normal;
        }

        /*.rgDataDiv
        {
            overflow-y: hidden !important;
        }*/
    </style>

    <script type="text/javascript">

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

        function pageLoad() {
            listbox = $find("<%= rlbDimensionAvailable.ClientID %>");
            filterTextBox = document.getElementById("<%= rtbDimSearch.ClientID %>");

            // set on anything but text box
            listbox._getGroupElement().focus();

            var rcbEmailcombo = $find("<%= this.rcbEmail.ClientID %>");

            var rcbEmailinput = rcbEmailcombo.get_inputDomElement();

            rcbEmailinput.onchange = function () {
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

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function cbSelectAllProduct_CheckedChanged() {
            if ($("#<%=gvProductAddress.ClientID%> input[id*='cbSelectAllProduct']:checkbox").is(':checked')) {
                $("#<%=gvProductAddress.ClientID%> input[id*='cbSelectProduct']:checkbox").prop('checked', true);
            }
            else {
                $("#<%=gvProductAddress.ClientID%> input[id*='cbSelectProduct']:checkbox").prop('checked', false);
            }
        }

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
            DynamicLayout="true">
            <ProgressTemplate>
                <div class="TransparentGrayBackground">
                </div>
                <div class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="../images/loading.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ShowFilter" EventName="CausePostBack" />
            </Triggers>
            <ContentTemplate>
                <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="mdlPopupDimension" runat="server" TargetControlID="btnShowPopup2"
                    PopupControlID="pnlPopupDimensions" CancelControlID="btnCloseDimensions" BackgroundCssClass="modalBackground" />
                <asp:RoundedCornersExtender ID="RoundedCornersExtender2" runat="server" BehaviorID="RoundedCornersBehavior2"
                    TargetControlID="pnlPopupDimensionsRound" Radius="6" Corners="All" />
                <asp:Panel ID="pnlPopupDimensions" runat="server" Width="935px" Style="display: none"
                    CssClass="modalPopup">
                    <asp:Panel ID="pnlPopupDimensionsRound" runat="server" Width="935px" CssClass="modalPopup2">
                        <br />
                        <div align="left" style="text-align: center; padding: 0px 10px 0px 10px;  z-index: 10">
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
                                        <asp:HiddenField runat="server" ID="hfMasterValue" Value="0"></asp:HiddenField>
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
                                                    <asp:HiddenField ID="hfBrandID" runat="server" Value="0" /></td>
                                        </tr>
                                        <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                            <td valign="middle" align="center">
                                                <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image><br />
                                        </tr>
                                    </asp:Panel>
                                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                        <td valign="middle" align="center">
                                            <asp:Panel ID="pnlmarketHeader" runat="server" Height="28px" Width="100%" BackColor="#e3e3e3" ForeColor="Blue">
                                            <div style="padding: 5px 5px 5px 5px; width: 250px; text-align: center; float: left;">
                                                <asp:LinkButton ID="lnkMarkets" runat="server" CommandName="lstMarket" CommandArgument="Markets"
                                                    Text="MARKETS" OnCommand="lnkPopup_Command" Font-Bold="true"></asp:LinkButton></b>
                                                    <asp:LinkButton ID="lnkMarketShowHide" runat="server" Text="(Show...)" Font-Size="XX-Small" ForeColor="Black" Font-Underline="false" CommandArgument="|Markets|" OnCommand="lnkDimensionShowHide_Command"></asp:LinkButton>
                                            </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlMarket" runat="server" Visible="false">   
                                        <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                            <td valign="middle" align="center">
                                                <asp:ListBox ID="lstMarket" runat="server" Rows="5" DataValueField="MarketID" DataTextField="MarketName"
                                                    AutoPostBack="true" Style="text-transform: uppercase; width: 240px;" SelectionMode="Multiple"
                                                    Font-Size="x-small" Font-Names="Arial" OnSelectedIndexChanged="lstMarket_SelectedIndexChanged"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Repeater ID="PubTypeRepeater" runat="server" OnItemDataBound="PubTypeRepeater_ItemDataBound">
                                        <ItemTemplate>
                                            <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                <td valign="middle" align="center">
                                                    <asp:Panel ID="pnlpubtypeHeader" runat="server" Height="28px" Width="100%" BackColor="#e3e3e3" ForeColor="Blue">
                                                    <div style="padding: 5px 5px 5px 5px; width: 250px; text-align: center; float: left;">
                                                        <asp:HiddenField ID="hfPubTypeID" runat="server" Value='<%#Eval("PubTypeID") %>' />
                                                        <asp:LinkButton ID="PubTypeLinkButton" runat="server" Text='<%# Eval("PubTypeDisplayName")%>' CommandArgument='<%#Eval("PubTypeID") + "|PUBTYPE|" + Eval("PubTypeDisplayName")%>' OnCommand="lnkPopup_Command" Font-Bold="true"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkPubTypeShowHide" runat="server" Text="(Show...)" Font-Size="XX-Small" ForeColor="Black" Font-Underline="false" CommandArgument='<%#Eval("PubTypeID") + "|PUBTYPE|" + Eval("PubTypeDisplayName")%>' OnCommand="lnkDimensionShowHide_Command"></asp:LinkButton>
                                                    </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <asp:Panel ID="pnlPubTypeBody" runat="server" Visible="false">
                                            <tr>
                                                <td valign="top" align="center">
                                                    <asp:ListBox ID="PubTypeListBox"
                                                        runat="server"
                                                        Rows="5"
                                                        Style="text-transform: uppercase; width: 240px"
                                                        SelectionMode="Multiple"
                                                        Font-Size="x-small"
                                                        Font-Names="Arial"></asp:ListBox>
                                                </td>
                                            </tr>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:Repeater>
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
                                                                <div style="padding: 5px 5px 5px 5px; width: 250px; text-align: center; float: left;">
                                                                    <asp:LinkButton ID="lnkDimensionPopup" runat="server" Text='<%# Eval("DisplayName")%>' CommandArgument='<%#Eval("MasterGroupID") + "|DIMENSION|" + Eval("DisplayName")%>' OnCommand="lnkPopup_Command"></asp:LinkButton>
                                                                    <asp:Label ID="lblResponseGroup" Text='<%#Eval("DisplayName") %>' runat="server" Visible="false"></asp:Label>
                                                                    <asp:HiddenField ID="hfMasterGroup" runat="server" Value='<%#Eval("MasterGroupID") %>' />
                                                                    <asp:LinkButton ID="lnkDimensionShowHide" runat="server" Text="(Show...)" Font-Size="XX-Small" ForeColor="Black" Font-Underline="false" CommandArgument='<%#Eval("MasterGroupID") + "|DIMENSION|" + Eval("DisplayName")%>' OnCommand="lnkDimensionShowHide_Command"></asp:LinkButton>
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlDimBody" runat="server" Visible="false">
                                                                <div style="padding: 2px 5px 5px 5px;">
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
                                <asp:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlGlobalFilterBody"
                                    ExpandControlID="pnlGlobalFilterHeader" CollapseControlID="pnlGlobalFilterHeader"
                                    Collapsed="false" TextLabelID="pnlGlobalFilterLabel" ImageControlID="pnlGlobalFilterImage"
                                    ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                    CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
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
                                                    SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial" Width="120px"
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
                                                    <tr style="background-color: #eeeeee; color: White; padding: 0px 0px 0px 0px; height: 20px;">
                                                        <td class="labelsmall" colspan="5" align="center">
                                                            <b>Contact Fields </b>
                                                        </td>
                                                        <td class="labelsmall" colspan="7" align="center">
                                                            <b>Permissions</b>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                        <td class="labelsmall" align="center">
                                                            <b>Email</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Phone</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Fax</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Media</b>
                                                        </td>
                                                        <td align="center">
                                                            <b>GeoLocated</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Mail</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Fax</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Phone</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>Other Products</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
                                                            <b>3rd Party</b>
                                                        </td>
                                                        <td class="labelsmall" align="center">
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
                                                            <telerik:RadComboBox ID="rcbIsLatLonValid" runat="server" CheckBoxes="true" Font-Names="Arial" Font-Size="x-small" Width="75px">
                                                                <Items>
                                                                    <telerik:RadComboBoxItem Text="Yes" Value="1" Font-Names="Arial" Font-Size="x-small" />
                                                                    <telerik:RadComboBoxItem Text="No" Value="0" Font-Names="Arial" Font-Size="x-small" />
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
                                                    <tr>
                                                        <td valign="top" colspan="15">
                                                            <table border="0" bordercolor="#cccccc" cellpadding="2" cellspacing="1" width="100%">
                                                                <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                                    <asp:PlaceHolder ID="phlblLastName" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <b>
                                                                                <asp:Label ID="lblLastName" runat="server" Text="Last Name"></asp:Label></b>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phlblFirstName" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <b>
                                                                                <asp:Label ID="lblFirstName" runat="server" Text="First Name"></asp:Label></b>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phlblCompany" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <b>
                                                                                <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label></b>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phlblPhone" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <b>
                                                                                <asp:Label ID="lblPhone" runat="server" Text="Phone"></asp:Label></b>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phlblEmail" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <b>
                                                                                <asp:Label ID="lblEmail" runat="server" Text="Email Address"></asp:Label></b>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                </tr>
                                                                <tr>
                                                                    <asp:PlaceHolder ID="phtxtLastName" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phtxtFirstName" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phtxtCompany" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phtxtPhone" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                    <asp:PlaceHolder ID="phtxtEmail" runat="server">
                                                                        <td class="labelsmall" align="center">
                                                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </asp:PlaceHolder>
                                                                </tr>
                                                            </table>
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
                                            <td align="center">
                                                <div id="divError" runat="Server" visible="false" style="width: 674px">
                                                    <table cellspacing="0" cellpadding="0" width="674px" align="center">
                                                        <tr>
                                                            <td id="errorTop"></td>
                                                        </tr>
                                                        <tr>
                                                            <td id="errorMiddle" width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td valign="top" align="center" width="20%">
                                                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                                                alt="" />
                                                                        </td>
                                                                        <td valign="middle" align="left" width="80%" height="100%">
                                                                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
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
                                                <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="button" OnClick="btnSearch_Click"></asp:Button>
                                                &nbsp;
                                                <asp:Button ID="btnResetAll" Text="Reset All Filters" runat="server" CssClass="buttonMedium"
                                                    OnClick="btnResetAll_Click"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlResultsHeader" runat="server" CssClass="collapsePanelHeader" Height="28px">
                                    <div style="padding: 5px; cursor: pointer; vertical-align: middle;">
                                        <div style="float: left;">
                                            Results
                                        </div>
                                        <div style="float: left; margin-left: 20px;">
                                            <asp:Label ID="pnlResultLabel" runat="server">(Show Details...)</asp:Label>
                                        </div>
                                        <div style="float: right; vertical-align: middle;">
                                            <asp:ImageButton ID="pnlResultImage" runat="server" ImageUrl="~/images/expand_blue.jpg"
                                                AlternateText="(Show Details...)" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:CollapsiblePanelExtender ID="cpResult" runat="Server" TargetControlID="pnlResultBody"
                                    ExpandControlID="pnlResultsHeader" CollapseControlID="pnlResultsHeader" Collapsed="false"
                                    TextLabelID="pnlResultLabel" ImageControlID="pnlResultImage" ExpandedText="(Hide Details...)"
                                    CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
                                    CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                <asp:Panel ID="pnlResultBody" runat="server" CssClass="collapsePanel" Height="0"
                                    BorderColor="#5783BD" BorderWidth="1">
                                    <br />
                                    <div id="dvResults" style="padding: 10px; display: none; width: 100%; table-layout: fixed;" runat="server">
                                        <telerik:RadGrid AutoGenerateColumns="False" ID="rgSubscriberList" runat="server" Visible="true" AllowPaging="true" OnNeedDataSource="rgSubscriberList_NeedDataSource" BorderColor="#5783BD" Width="100%" Height="580px">
                                            <PagerStyle Mode="NextPrevAndNumeric" Position="Bottom"></PagerStyle>
                                            <ClientSettings EnableRowHoverStyle="true" AllowExpandCollapse="true">
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="3"></Scrolling>
                                            </ClientSettings>
                                            <MasterTableView DataKeyNames="SubscriptionID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No Records" PageSize="15" AllowSorting="True">
                                                <HeaderStyle Font-Bold="true" Font-Size="13px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <%# Container.ItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn HeaderText="Last Name" DataField="lname" UniqueName="lname" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-CssClass="RadGriditemwrap"  />
                                                    <telerik:GridBoundColumn HeaderText="First Name" DataField="fname" UniqueName="fname" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-CssClass="RadGriditemwrap"  />
                                                    <telerik:GridBoundColumn HeaderText="Company" DataField="company" UniqueName="company" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-CssClass="RadGriditemwrap" />
                                                    <telerik:GridBoundColumn HeaderText="Title" DataField="Title" UniqueName="Title" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="15%" ItemStyle-Width="15%" ItemStyle-CssClass="RadGriditemwrap"  />
                                                    <telerik:GridBoundColumn HeaderText="Email" DataField="email" UniqueName="email" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="20%" ItemStyle-Width="20%" ItemStyle-CssClass="RadGriditemwrap"  />
                                                    <telerik:GridBoundColumn HeaderText="Phone" DataField="phone" UniqueName="phone" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-CssClass="RadGriditemwrap" />
                                                    <telerik:GridBoundColumn HeaderText="Score" DataField="Score" UniqueName="Score" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-CssClass="RadGriditemwrap" />
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkShow" runat="server" SkinID="ViewButton" OnCommand="lnkShow_Command" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEditAddress" runat="server" OnCommand="lnkEditAddress_Command"><img src="../Images/Edit.png" alt="" style="border:none;" /></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </asp:Panel>
                                <asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="mdlPopupEditSubscriber" runat="server" TargetControlID="btnShowPopup1"
                                    PopupControlID="pnlPopup1" CancelControlID="lnkClose" BackgroundCssClass="modalBackground" />
                                <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" BehaviorID="RoundedCornersBehavior1"
                                    TargetControlID="pnlPopupRound1" Radius="6" Corners="All" />
                                <asp:Panel ID="pnlPopup1" runat="server" Width="1150px" Style="display: none" CssClass="modalPopup">
                                    <asp:Panel ID="pnlPopupRound1" runat="server" Width="1150px" CssClass="modalPopup2">
                                        <div align="right">
                                            <asp:LinkButton runat="server" ID="lnkClose" Style="padding: 20px 10px 0px 10px; font-size: 12px;">Close</asp:LinkButton>
                                        </div>
                                        <div align="right" style="padding: 0px 10px 10px 10px;">
                                            <ajaxToolkit:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0" Style="text-align: left"
                                                AutoPostBack="false">
                                                <ajaxToolkit:TabPanel ID="TabSubscriberDetails" runat="server" HeaderText="Subscriber Details">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnDownload" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <div style="text-align: center; padding: 0px 10px 0px 10px; height: 400px; overflow: auto;">
                                                                    <table width="100%" border="0" cellpadding="5" cellspacing="0" class="popupborder">
                                                                        <tr class="popupheader">
                                                                            <td class="popupheader" colspan="3">
                                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="popupborder">
                                                                                    <tr>
                                                                                        <td class="popupheader">
                                                                                            <font color="#FFFFFF">Subscriber Detail</font>
                                                                                            <asp:Label ID="lblRowIndex" Text="" runat="server" Visible="false"></asp:Label>
                                                                                            <asp:HiddenField ID="hfSubscriptionID" runat="server" Value="0" />
                                                                                            <asp:Label ID="lblEmailID" Text="" runat="server" Visible="false"></asp:Label></td>
                                                                                        <td align="right">
                                                                                            <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="button" OnClick="btnDownload_click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td class="popupheader"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="5%" valign="middle" align="center">
                                                                                <asp:LinkButton ID="lnkPrevious" runat="server" SkinID="PreviousButton" OnClick="lnkPrevious_click" />
                                                                            </td>
                                                                            <td align="center" width="90%" valign="middle">
                                                                                <table id="table2" style="border-collapse: collapse;" cellspacing="0" cellpadding="1"
                                                                                    width="100%" align="center" border="1" class="popupborder">
                                                                                    <tr>
                                                                                        <td valign="top" colspan="3" align="center">
                                                                                            <table id="table5" style="border-collapse: collapse;" cellspacing="0" cellpadding="1"
                                                                                                width="100%" align="center" border="1">
                                                                                                <tr>
                                                                                                    <td align="left" valign="top" width="35%" class="popupborder">
                                                                                                        <table id="table4" cellspacing="2" cellpadding="2" border="0" style="height: 150px;"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>First Name :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_FirstName" runat="server" ReadOnly="true" MaxLength="100" Width="210px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Last Name :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_LastName" runat="server" ReadOnly="true" MaxLength="100" Width="210px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Title :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Title" runat="server" Width="210px" ReadOnly="true" MaxLength="100"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Email :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:TextBox ID="txt_Email" runat="server" Width="210px" ReadOnly="true" MaxLength="100"></asp:TextBox>
                                                                                                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="Save"
                                                                                                                        ErrorMessage="<br/>Not a valid email address" ControlToValidate="txt_Email" ForeColor="Red"
                                                                                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Phone :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:TextBox ID="txt_Phone" runat="server" ReadOnly="true" MaxLength="100"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Fax :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:TextBox ID="txt_Fax" runat="server" ReadOnly="true" MaxLength="100"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Mobile :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:TextBox ID="txt_Mobile" runat="server" ReadOnly="true" MaxLength="30"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td width="35%" class="popupborder">
                                                                                                        <table id="table3" cellspacing="2" cellpadding="2" border="0" style="height: 150px;"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="right" width="50%">
                                                                                                                    <b>Company :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Company" runat="server" ReadOnly="true" MaxLength="100" Width="236px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Address1 :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Address" runat="server" ReadOnly="true" MaxLength="255" Width="236px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Address2 :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Address2" runat="server" ReadOnly="true" MaxLength="255" Width="236px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Address3 :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Address3" runat="server" ReadOnly="true" MaxLength="255" Width="236px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Country :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_Country" runat="server" Width="150px" Enabled="false" OnSelectedIndexChanged="drp_Country_SelectedIndexChanged" DataValueField="CountryID" DataTextField="ShortName" AutoPostBack="true" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>City :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_City" runat="server" ReadOnly="true" MaxLength="50" Width="100px"></asp:TextBox>&nbsp;
                                                                                                                    <asp:PlaceHolder ID="pltxt_State" runat="server" Visible="false">
                                                                                                                        <b>State :&nbsp;</b>
                                                                                                                        <asp:TextBox ID="txt_State" runat="server" Width="50px" ReadOnly="true" MaxLength="50">&nbsp;&nbsp;</asp:TextBox>
                                                                                                                    </asp:PlaceHolder>
                                                                                                                    <asp:PlaceHolder ID="pldrp_State" runat="server" Visible="false">
                                                                                                                        <b>State :&nbsp;</b>
                                                                                                                        <asp:DropDownList ID="drp_State" runat="server" Width="60px" Enabled="false" DataValueField="RegionCode" DataTextField="RegionCode" />
                                                                                                                    </asp:PlaceHolder>

                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Zip :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Zip" runat="server" Width="50px" ReadOnly="true" MaxLength="10"></asp:TextBox>
                                                                                                                    <b>Plus4 :&nbsp;</b>
                                                                                                                    <asp:TextBox ID="txt_Plus4" runat="server" ReadOnly="true" MaxLength="4" onkeypress="return isNumber(event)" Width="50px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>For Zip :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:TextBox ID="txt_ForZip" runat="server" ReadOnly="true" MaxLength="50"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td align="left" valign="top" width="30%" class="popupborder">
                                                                                                        <table id="table1" cellspacing="2" cellpadding="2" border="0" style="height: 150px;"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="right" style="width: 70%; color: #FF0000; font-size: medium">Score :&nbsp;
                                                                                                                </td>
                                                                                                                <td align="left" style="color: #FF0000; font-size: medium">
                                                                                                                    <asp:Label ID="lbl_Score" runat="server"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>MailPermissions :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_MailPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>FaxPermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_FaxPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>PhonePermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_PhonePermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>OtherProductsPermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_OtherProductsPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>ThirdPartyPermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_ThirdPartyPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>EmailRenewPermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_EmailRenewPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>TextPermission :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_TextPermission" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                        Style="text-transform: uppercase" Enabled="false">
                                                                                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                                        <asp:ListItem Value="-1">Blank</asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <%--                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Email Status :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="drp_EmailStatus" runat="server" Font-Names="Arial" Font-Size="x-small" Width="100px" AutoPostBack="true" Enabled="false"
                                                                                                                        Style="text-transform: uppercase" DataValueField="EmailStatusID" DataTextField="Status" OnSelectedIndexChanged="drp_EmailStatus_SelectedIndexChanged">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>--%>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="middle" colspan="3" class="poptableborder">
                                                                                            <table id="table6" cellspacing="0" cellpadding="1"
                                                                                                width="100%" align="center" border="0" class="popupborder">
                                                                                                <tr>
                                                                                                    <td width="50%">
                                                                                                        <table id="table7" cellspacing="2" cellpadding="2" border="0" style="height: 150px;" width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="middle" align="right" width="40%"><b>IGroupNo :&nbsp;</b></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbl_IGrp_No" runat="server" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>QDate :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="lbl_QDate" runat="server" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Transaction Date :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="lbl_TDate" runat="server" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Date Created :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="lbl_DateCreated" runat="server" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Date Updated :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="lbl_DateUpdated" runat="server" Text=""></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Opens :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbl_Opens" Text="" runat="server"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="right">
                                                                                                                    <b>Clicks :&nbsp;</b>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbl_Clicks" Text="" runat="server"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td valign="middle">
                                                                                                        <table id="table8" cellspacing="2" cellpadding="2" border="0" style="height: 150px;" width="100%">
                                                                                                            <tr>
                                                                                                                <td width="15%"><b>Notes :&nbsp;</b></td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txt_Notes" runat="server" ReadOnly="true" Columns="5" Width="300px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                                                                                                    <font color="Red">
                                                                                                                        <asp:RegularExpressionValidator ID="rvNotes" ControlToValidate="txt_Notes" ValidationExpression="^[\s\S]{0,2000}$" runat="server" Text="Only 2000 characters are allowed" SetFocusOnError="True" />
                                                                                                                    </font>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>

                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" colspan="3" class="poptableborder" align="center">
                                                                                            <asp:DataList ID="dlDetails" runat="server" AlternatingItemStyle-Width="50%" ItemStyle-Width="50%"
                                                                                                RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" ItemStyle-BorderWidth="1"
                                                                                                Width="100%" ItemStyle-BorderColor="#CCCCCC">
                                                                                                <AlternatingItemStyle Width="50%" />
                                                                                                <ItemStyle BorderWidth="1px" Width="50%" />
                                                                                                <ItemTemplate>
                                                                                                    <table style="table-layout: fixed; width: 100%">
                                                                                                        <tr>
                                                                                                            <td width="35%" align="right" style="word-wrap: break-word">
                                                                                                                <asp:Label runat="server" ID="lblKey" Text='<%#Eval("Key") %>' Font-Bold="true"></asp:Label>
                                                                                                                :&nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td width="65%" align="left" style="word-wrap: break-word">
                                                                                                                <asp:Label runat="server" ID="lblValue" Text='<%#Eval("value") %>'></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:DataList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" colspan="3" class="poptableborder" align="center">
                                                                                            <div runat="server" align="left">
                                                                                                <asp:Label runat="server" ID="lblAdhocDetails" Text="Adhoc Details" Font-Bold="true" Visible="false"></asp:Label>
                                                                                            </div>
                                                                                            <asp:DataList ID="dlAdhocDetails" runat="server" AlternatingItemStyle-Width="50%" ItemStyle-Width="50%"
                                                                                                RepeatColumns="2" RepeatDirection="Horizontal" RepeatLayout="Table" ItemStyle-BorderWidth="1"
                                                                                                Width="100%" ItemStyle-BorderColor="#CCCCCC">
                                                                                                <AlternatingItemStyle Width="50%" />
                                                                                                <ItemStyle BorderWidth="1px" Width="50%" />
                                                                                                <ItemTemplate>
                                                                                                    <table style="table-layout: fixed; width: 100%">
                                                                                                        <tr>
                                                                                                            <td width="35%" align="right" style="word-wrap: break-word">
                                                                                                                <asp:Label runat="server" ID="Label1" Text='<%#Eval("Key") %>' Font-Bold="true"></asp:Label>
                                                                                                                :&nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td width="65%" align="left" style="word-wrap: break-word">
                                                                                                                <asp:Label runat="server" ID="Label2" Text='<%#Eval("value") %>'></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:DataList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="top" colspan="3" class="popupborder" align="center">
                                                                                            <table id="table10" style="border-collapse: collapse;" cellspacing="0" cellpadding="1"
                                                                                                width="100%">
                                                                                                <%--                                                                                                <asp:Repeater ID="ProductRepeater" runat="server" OnItemDataBound="ProductRepeater_ItemDataBound">
                                                                                                    <ItemTemplate>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <b>
                                                                                                                    <asp:Label runat="server" ID="lblProduct" Text='<%# Eval("ColumnReference") %>'></asp:Label></b>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:ListBox ID="lstProduct" runat="server" Rows="5" Width="250px"></asp:ListBox>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:GridView ID="grdProduct" runat="server" Width="400px" AutoGenerateColumns="False"
                                                                                                                    ShowFooter="false" DataKeyNames="PubID">
                                                                                                                    <Columns>
                                                                                                                        <asp:BoundField DataField="PubName" HeaderText="Pub" HeaderStyle-HorizontalAlign="Center"
                                                                                                                            HeaderStyle-Width="10px" ItemStyle-Width="10px" SortExpression="PubName" ItemStyle-Wrap="false">
                                                                                                                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                                                                                        </asp:BoundField>
                                                                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                                                                                                            ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:DropDownList ID="drpEmailStatus" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                                    Style="text-transform: uppercase" DataValueField="EmailStatusID" DataTextField="Status">
                                                                                                                                </asp:DropDownList>
                                                                                                                            </ItemTemplate>
                                                                                                                        </asp:TemplateField>
                                                                                                                    </Columns>
                                                                                                                </asp:GridView>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>--%>

                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:GridView ID="grdProduct" runat="server" Width="400px" AutoGenerateColumns="False"
                                                                                                            ShowFooter="false" DataKeyNames="PubSubscriptionID" OnRowDataBound="grdProduct_OnRowDataBound">
                                                                                                            <Columns>
                                                                                                                <asp:BoundField DataField="ColumnReference" HeaderText="Pub Type" HeaderStyle-HorizontalAlign="left"
                                                                                                                    HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="ColumnReference" ItemStyle-Wrap="false">
                                                                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="PubName" HeaderText="Product" HeaderStyle-HorizontalAlign="left"
                                                                                                                    SortExpression="PubName" ItemStyle-Wrap="false">
                                                                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:TemplateField HeaderText="Email" HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                                                                                                    ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtPubEmail" runat="server" Text='<%# Bind("Email") %>' Width="200px"></asp:TextBox>
                                                                                                                        <asp:RegularExpressionValidator ID="revPubEmail" runat="server" Display="Dynamic" ForeColor="Red"
                                                                                                                            ErrorMessage="<br/>Not a valid email address" ControlToValidate="txtPubEmail" ValidationGroup="Save"
                                                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Email Status" HeaderStyle-Width="10%" ItemStyle-Width="10%"
                                                                                                                    ItemStyle-HorizontalAlign="left" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="left">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:DropDownList ID="drpPubEmailStatus" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                            Style="text-transform: uppercase" DataValueField="EmailStatusID" DataTextField="Status">
                                                                                                                        </asp:DropDownList>
                                                                                                                        <asp:Label ID="lblPubEmailStatusID" runat="server" Visible="false" Text='<%# Bind("EmailStatusID") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:BoundField DataField="StatusUpdatedDate" HeaderText="Status Updated" HeaderStyle-HorizontalAlign="left"
                                                                                                                    HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="StatusUpdatedDate" ItemStyle-Wrap="false" DataFormatString="{0:MM/dd/yyyy}">
                                                                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                                                                </asp:BoundField>
                                                                                                                <asp:BoundField DataField="Qualificationdate" HeaderText="QDate" HeaderStyle-HorizontalAlign="left"
                                                                                                                    HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="Qualificationdate" ItemStyle-Wrap="false" DataFormatString="{0:MM/dd/yyyy}">
                                                                                                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                                                                                                </asp:BoundField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td width="5%" valign="middle" align="center">
                                                                                <asp:LinkButton ID="lnkNext" runat="server" SkinID="NextButton" OnClick="lnkNext_click" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="3">
                                                                                <div id="dvMessage" runat="Server" visible="false">
                                                                                    <asp:Label ID="lblMessage" runat="Server" ForeColor="Red"></asp:Label>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="3">
                                                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_click" ValidationGroup="Save"
                                                                                    Visible="false" />
                                                                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                                                                                    Width="1000px" Visible="False">
                                                                                </rsweb:ReportViewer>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabActivity" runat="server" HeaderText="Activity Details">
                                                    <ContentTemplate>
                                                        <div style="text-align: center; padding: 0px 10px 0px 10px; height: 400px; overflow: scroll;">
                                                            <asp:GridView ID="gvActivityHistory" runat="server">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="Activity" HeaderText="Activity" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="ActivityDate" HeaderText="Activity Date" DataFormatString="{0:MM/dd/yyyy}"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"
                                                                        ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="BlastID" HeaderText="BlastID" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="SendTime" HeaderText="Send Time" HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="EmailSubject" HeaderText="Email Subject" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="link" HeaderText="Link" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" ItemStyle-Width="20%" />
                                                                    <asp:BoundField DataField="linkAlias" HeaderText="Link Alias" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                                <ajaxToolkit:TabPanel ID="TabWebSiteTracking" runat="server" HeaderText="Website Tracking">
                                                    <ContentTemplate>
                                                        <div style="text-align: center; padding: 0px 10px 0px 10px; height: 400px; overflow: auto;">
                                                            <asp:GridView ID="gvWebSiteTracking" runat="server">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ActivityDate" HeaderText="Activity Date" DataFormatString="{0:MM/dd/yyyy}"
                                                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%"
                                                                        ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="DomainName" HeaderText="DomainName" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
                                                                    <asp:BoundField DataField="URL" HeaderText="URL" HeaderStyle-HorizontalAlign="Left"
                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50%" ItemStyle-Width="50%" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </ContentTemplate>
                                                </ajaxToolkit:TabPanel>
                                            </ajaxToolkit:TabContainer>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                                <%--Edit Address Popup--%>
                                <asp:Button ID="btnShowUpdateAddress" runat="server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender ID="mdlPopupUpdateAddress" runat="server" TargetControlID="btnShowUpdateAddress"
                                    PopupControlID="pnlPopupAddress" CancelControlID="lnkCloseUpdateAddress" BackgroundCssClass="modalBackground" />
                                <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender3" runat="server" BehaviorID="RoundedCornersBehavior3"
                                    TargetControlID="pnlPopupRound1" Radius="6" Corners="All" />
                                <asp:Panel ID="pnlPopupAddress" runat="server" Width="1000px" Height="500px" Style="display: none;" CssClass="modalPopupUA1">
                                    <asp:Panel ID="Panel2" runat="server" Width="1000px" CssClass="modalPopupUA2">
                                        <div align="right">
                                            <asp:LinkButton runat="server" ID="lnkCloseUpdateAddress" Style="padding: 20px 10px 0px 10px; font-size: 12px;">Close</asp:LinkButton>
                                        </div>
                                        <div align="right" style="padding: 0px 10px 10px 10px;">
                                            <table width="100%" border="0" cellpadding="5" cellspacing="0" class="popupborder">
                                                <tr class="popupheader">
                                                    <td class="popupheader" colspan="3">
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="popupborder">

                                                            <tr>
                                                                <td class="popupheader">
                                                                    <font color="#FFFFFF">Edit Address &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;
                                                                        First Name : &nbsp;<asp:Label ID="lblMasterFirstName" runat="server"></asp:Label>
                                                                        &nbsp; &nbsp;
                                                                        Last Name : &nbsp;<asp:Label ID="lblMasterLastName" runat="server"></asp:Label>
                                                                    </font>
                                                                    <asp:HiddenField ID="hfSubID" runat="server" Value="0" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="popupheader"></td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        <table id="table12" style="border-collapse: collapse;" cellspacing="0" cellpadding="1"
                                                            width="100%" align="center" border="1" class="popupborder">
                                                            <tr>
                                                                <td valign="top" colspan="3" align="center">
                                                                    <table id="table15" style="border-collapse: collapse;" cellspacing="0" cellpadding="1"
                                                                        width="100%" align="center" border="1">
                                                                        <tr>
                                                                            <td align="left" valign="top" width="35%" class="popupborder">
                                                                                <table id="table14" cellspacing="2" cellpadding="2" border="0" style="height: 75px;" width="100%">
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Address1 :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterAddress" runat="server" MaxLength="100" Width="200px"></asp:TextBox>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Address2 :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterAddress2" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Address3 :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterAddress3" runat="server" MaxLength="100" Width="200px"></asp:TextBox>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td width="30%" class="popupborder">
                                                                                <table id="table17" cellspacing="2" cellpadding="2" border="0" style="height: 75px;" width="100%">
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Country :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="drpMasterCountry" runat="server" Width="150px" OnSelectedIndexChanged="drpMasterCountry_SelectedIndexChanged" DataValueField="CountryID" DataTextField="ShortName" AutoPostBack="true" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>City :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterCity" runat="server" MaxLength="50"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>State :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:PlaceHolder ID="pltxtMasterState" runat="server" Visible="false">
                                                                                                <asp:TextBox ID="txtMasterState" runat="server" Width="50px" MaxLength="50">&nbsp;&nbsp;</asp:TextBox>
                                                                                            </asp:PlaceHolder>
                                                                                            <asp:PlaceHolder ID="pldrpMasterState" runat="server" Visible="false">
                                                                                                <asp:DropDownList ID="drpMasterState" runat="server" Width="50px" DataValueField="RegionCode" DataTextField="RegionCode" />
                                                                                            </asp:PlaceHolder>
                                                                                            <b>Zip :&nbsp;</b>
                                                                                            <asp:TextBox ID="txtMasterZip" runat="server" Width="50px" MaxLength="10"></asp:TextBox>&nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td width="30%" class="popupborder">
                                                                                <table id="table16" cellspacing="2" cellpadding="2" border="0" style="height: 75px;" width="100%">
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Phone :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterPhone" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Fax :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterFax" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <b>Email :&nbsp;</b>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMasterEmail" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                                                                            <asp:RegularExpressionValidator ID="revMasterEmail" runat="server" Display="Dynamic" ForeColor="Red"
                                                                                                ErrorMessage="<br/>Not a valid email address" ControlToValidate="txtMasterEmail" ValidationGroup="EditAddressSave"
                                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>

                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" colspan="3" class="popupborder" align="center">
                                                                    <div style="text-align: center; padding: 0px 10px 0px 10px; height: 250px; overflow: auto;">
                                                                        <table id="table11" cellspacing="0" cellpadding="0"
                                                                            width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="gvProductAddress" runat="server" Width="400px" AutoGenerateColumns="False"
                                                                                        ShowFooter="false" DataKeyNames="PubSubscriptionID" OnRowCommand="gvProductAddress_RowCommand" OnRowDataBound="gvProductAddress_RowDataBound">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:CheckBox ID="cbSelectAllProduct" runat="server" Text="" Checked="false" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="cbSelectProduct" runat="server" Text="" Checked="false" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Product Address" HeaderStyle-Width="10px" ItemStyle-Width="10px" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                                                <ItemTemplate>
                                                                                                    <table id="table16" style="border-collapse: collapse;" cellspacing="0" cellpadding="1" width="100%" align="center" border="1">
                                                                                                        <tr>
                                                                                                            <td colspan="3">
                                                                                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("PubName") %>' Font-Bold="True"></asp:Label>
                                                                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                                First Name :&nbsp;<asp:Label ID="lblProductFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>&nbsp;&nbsp;
                                                                                                                Last Name :&nbsp;<asp:Label ID="lblProductLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" valign="top" width="35%">
                                                                                                                <table id="table17" cellspacing="2" cellpadding="2" border="0" style="height: 75px;"
                                                                                                                    width="100%">
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Address1 :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductAddress" runat="server" MaxLength="100" Width="150px" Text='<%# Bind("Address1") %>'></asp:TextBox>&nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Address2 :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductAddress2" runat="server" MaxLength="100" Width="150px" Text='<%# Bind("Address2") %>'></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Address3 :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductAddress3" runat="server" MaxLength="100" Width="150px" Text='<%# Bind("Address3") %>'></asp:TextBox>&nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td align="left" valign="top" width="30%">
                                                                                                                <table id="table13" cellspacing="2" cellpadding="2" border="0" style="height: 75px;"
                                                                                                                    width="100%">
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Country :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:DropDownList ID="drpProductCountry" runat="server" Width="150px" OnSelectedIndexChanged="drpProductCountry_SelectedIndexChanged" DataValueField="CountryID" DataTextField="ShortName" AutoPostBack="true" />
                                                                                                                            <asp:HiddenField ID="hfProductCountry" runat="server" Value='<%# Bind("CountryID")%>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>City :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductCity" runat="server" MaxLength="50" Text='<%# Bind("City") %>'></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>State :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:PlaceHolder ID="pltxtProductState" runat="server" Visible="false">
                                                                                                                                <asp:TextBox ID="txtProductState" runat="server" Width="50px" MaxLength="50" Text='<%# Bind("RegionCode") %>'>&nbsp;&nbsp;</asp:TextBox>
                                                                                                                            </asp:PlaceHolder>
                                                                                                                            <asp:PlaceHolder ID="pldrpProductState" runat="server" Visible="false">
                                                                                                                                <asp:DropDownList ID="drpProductState" runat="server" Width="50px" DataValueField="RegionCode" DataTextField="RegionCode" />
                                                                                                                                <asp:HiddenField ID="hfProductState" runat="server" Value='<%# Bind("RegionCode")%>' />
                                                                                                                            </asp:PlaceHolder>
                                                                                                                            <b>Zip :&nbsp;</b>
                                                                                                                            <asp:TextBox ID="txtProductZip" runat="server" Width="50px" MaxLength="10" Text='<%# Bind("ZipCode") %>'></asp:TextBox>&nbsp;
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                            <td width="35%">
                                                                                                                <table id="table18" cellspacing="2" cellpadding="2" border="0" style="height: 75px;" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Phone :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductPhone" runat="server" MaxLength="50" Text='<%# Bind("Phone") %>' Width="150px"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Fax :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductFax" runat="server" MaxLength="50" Text='<%# Bind("Fax") %>' Width="150px"></asp:TextBox>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Email :&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td align="left">
                                                                                                                            <asp:TextBox ID="txtProductEmail" runat="server" Width="150px" MaxLength="100" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                                                                                            <asp:RegularExpressionValidator ID="revProductEmail" runat="server" Display="Dynamic" ForeColor="Red"
                                                                                                                                ErrorMessage="<br/>Not a valid email address" ControlToValidate="txtProductEmail" ValidationGroup='<%# "EditProductAddressSave" + Eval("PubSubscriptionID") %>'
                                                                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="right">
                                                                                                                            <b>Email Status:&nbsp;</b>
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <asp:DropDownList ID="drpProductEmailStatus" runat="server" Font-Names="Arial" Font-Size="x-small"
                                                                                                                                Style="text-transform: uppercase" DataValueField="EmailStatusID" DataTextField="Status">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <asp:HiddenField ID="hfProductEmailStatusID" runat="server" Value='<%# Bind("EmailStatusID")%>' />
                                                                                                                        </td>

                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="2%" ItemStyle-Width="2%"
                                                                                                ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="center">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkProductAddressUpdate" runat="server" CommandName="ProductAddressUpdate" CommandArgument='<%# Eval("PubSubscriptionID")%>'
                                                                                                        Text="<img src='../images/ic-Save.png' style='border:none;'>" ValidationGroup='<%# "EditProductAddressSave" + Eval("PubSubscriptionID") %>' OnClientClick="return confirm('This transaction may affect your audit data if the selected products are audited. Are you sure you want to continue with update?');" > </asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
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
                                                    <td align="center" colspan="3">
                                                        <div id="divEditAddress" runat="Server" visible="false">
                                                            <asp:Label ID="lblMsgEditAddress" runat="Server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="3">
                                                        <asp:Button ID="btnUpdateInSelected" runat="server" Text="Update in Selected Products" CssClass="" OnClick="btnUpdateInSelected_click" ValidationGroup="EditAddressSave" OnClientClick="return confirm('This transaction may affect your audit data if the selected products are audited. Are you sure you want to continue with update?');" />
                                                        &nbsp;
                                                        <%--<asp:Button ID="btnUpdateAllProducts" runat="server" Text="Update in All Products" CssClass="" OnClick="btnUpdateAllProducts_click" ValidationGroup="EditAddressSave" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <uc:Adhoc runat="server" ID="AdhocFilter" Visible="false"></uc:Adhoc>
                <uc:Activity runat="server" ID="ActivityFilter" Visible="false"></uc:Activity>
                <uc:ShowFilter runat="server" ID="ShowFilter" Visible="false"></uc:ShowFilter>
                <uc:Circulation runat="server" ID="CirculationFilter" Visible="false"></uc:Circulation>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="geocode.aspx.cs" Inherits="KMPS.MD.Tools.geocode" %>

<%@ Register TagName="Download" TagPrefix="uc" Src="~/Controls/DownloadPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        /* Define a style used for infoboxes */
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
            box-shadow: 3px 3px 3px #888888;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ModalWindow {
            border: solid1px#c0c0c0;
            background: #ffffff;
            padding: 0px10px10px10px;
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
    <script type='text/javascript' src='http://www.bing.com/api/maps/mapcontrol?callback=GetMap' async="async" defer="defer"></script>
    <script type="text/javascript">
        var polyline;
        var circlepoints;
        var drawingManager;
        var searchArea;
        var tools;
        var pinLayer;
        var infobox;
        var pins = [];
        var clusterLayer;
        var shape;

        function GetMap() {
            map = new Microsoft.Maps.Map('#myMap', {
                credentials: 'Ah5_pIcVoon_WSCITtrzU5005iKKX4vNbK5IRAFZwEQ6wdwi7rZDefywKssr5y6o',
            });

            //Load the Drawing Tools and Spatial Math modules.
            Microsoft.Maps.loadModule('Microsoft.Maps.DrawingTools', function () {
                var dt = Microsoft.Maps.DrawingTools;
                var da = dt.DrawingBarAction;
                tools = new dt(map);

                tools.showDrawingManager(function (manager) {
                    //Store a reference to the drawing manager.
                    manager.setOptions({
                        //drawingBarActions: da.polygon | da.erase
                    });

                    drawingManager = manager;
                });

                //When the user presses 'esc', take the polygon out of edit mode and re-add to base map.
                document.getElementById('myMap').onkeypress = function (e) {
                    if (e.charCode === 27) {
                        tools.finish(searchArea);
                    }
                };

                //When ever the polygon search area has changed, a new Location added, or is editted, do a search.
                Microsoft.Maps.Events.addHandler(tools, 'drawingChanged', findIntersectingData);
            });

            Microsoft.Maps.loadModule('Microsoft.Maps.SpatialMath');
        }

        function loadMap(dt, myLatitude, myLongitude, radius) {

            document.getElementById("<%= hfselected.ClientID %>").value = '';

            for (var i = map.entities.getLength() - 1; i >= 0; i--) {
                var polygon = map.entities.get(i);
                if (polygon instanceof Microsoft.Maps.Polygon) {
                    map.entities.removeAt(i);
                }
            }

            drawingManager.clear();
            map.entities.clear();
            map.layers.clear();
            pins = [];

            if ((radius > 0) && (radius < 51)) {
                map.setView({ center: new Microsoft.Maps.Location(myLatitude, myLongitude), zoom: 13 });
            }
            else if ((radius > 50) && (radius < 101)) {
                map.setView({ center: new Microsoft.Maps.Location(myLatitude, myLongitude), zoom: 11 });
            }
            else if ((radius > 100) && (radius < 201)) {
                map.setView({ center: new Microsoft.Maps.Location(myLatitude, myLongitude), zoom: 10 });
            }
            else {
                map.setView({ center: new Microsoft.Maps.Location(myLatitude, myLongitude), zoom: 8 });
            }

            pinLayer = new Microsoft.Maps.Layer();
            map.layers.insert(pinLayer);

            infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
                visible: false
            });

            //Assign the infobox to a map instance.
            infobox.setMap(map);
            
            for (var i = 0; i < dt.MapPoints.length; i++) {
                var point = new Microsoft.Maps.Location(dt.MapPoints[i].Lt, dt.MapPoints[i].Lg);
                shape = new Microsoft.Maps.Pushpin(point, { icon: dt.MapPoints[i].MI });
                //pinLayer.add(shape);

                if ((dt.MapPoints[i].Lt != myLatitude) && (dt.MapPoints[i].Lg != myLongitude)) {

                    shape.metadata = {
                        title: 'Subscribers: ' + dt.MapPoints[i].Sc,
                    };

                    Microsoft.Maps.Events.addHandler(shape, 'mouseover', function (e) {
                        infobox.setOptions({
                            location: e.target.getLocation(),
                            visible: true,
                            offset: new Microsoft.Maps.Point(0, 20)
                        });
                    });

                    Microsoft.Maps.Events.addHandler(shape, 'mousedown', function (e) {
                        infobox.setOptions({ visible: false });
                    });

                    Microsoft.Maps.Events.addHandler(shape, 'mouseout', function (e) {
                        infobox.setOptions({ visible: false });
                    });
                }
                else
                    map.setView({ center: point });

                pins.push(shape);
            }

            Microsoft.Maps.loadModule("Microsoft.Maps.Clustering", function () {
                styles: [{
                    textSize: 0
                }],
                clusterLayer = new Microsoft.Maps.ClusterLayer(pins, {
                    callback: createPushpinList,
                    clusteredPinCallback: function (cluster) {
                        cluster.setOptions({
                            icon: '../Images/red-pin.png',
                            //anchor: new Microsoft.Maps.Point(8, -5),
                        });
                    }
                });

                Microsoft.Maps.Events.addHandler(clusterLayer, 'mouseover', clusterClicked);

                Microsoft.Maps.Events.addHandler(clusterLayer, 'mousedown', function (e) {
                    infobox.setOptions({ visible: false });
                });

                Microsoft.Maps.Events.addHandler(clusterLayer, 'mouseout', function (e) {
                    infobox.setOptions({ visible: false });
                });
                map.layers.insert(clusterLayer);
            });

            //map.entities.push(pins);

            circlepoints = getCircle(myLatitude, myLongitude, radius);
            var polylineColor = new Microsoft.Maps.Color(0.39, 100, 0, 100);
            polyline = new Microsoft.Maps.Polygon(circlepoints, { strokeColor: polylineColor });
            map.entities.push(polyline);

            drawSearchArea();
        }

        function clusterClicked(e) {
            showInfobox(e.target);
        }

        function createPushpinList() {
            //Create a list of displayed pushpins each time clustering layer updates.
            if (clusterLayer != null) {
                infobox.setOptions({ visible: false });
                //Get all pushpins that are currently displayed.
                var data = clusterLayer.getDisplayedPushpins();
                var output = [];
                //Create a list of links for each pushpin that opens up the infobox for it.
                for (var i = 0; i < data.length; i++) {
                    output.push("<a href='javascript:void(0);' onclick='showInfoboxByGridKey(", data[i].gridKey, ");'>");
                    output.push(data[i].getTitle(), "</a><br/>");
                }
                document.getElementById('listOfPins').innerHTML = output.join('');
            }
        }

        function showInfoboxByGridKey(gridKey) {
            //Look up the cluster or pushpin by gridKey.
            var clusterPin = clusterLayer.getClusterPushpinByGridKey(gridKey);
            //Show an infobox for the cluster or pushpin.
            showInfobox(clusterPin);
        }
        function showInfobox(pin) {
            var description = [];
            var title1 = [];
            //Check to see if the pushpin is a cluster.
            if (pin.containedPushpins) {
                description.push('Zoom in for more details');
                title1.push('Subscribers: ' + pin.containedPushpins.length)
                title1 = title1.join('');

                //Display an infobox for the pushpin.
                infobox.setOptions({
                    title: title1,
                    location: pin.getLocation(),
                    description: description.join(''),
                    visible: true
                });

            }
            else{
                infobox.setOptions({
                    title: 'Subscribers : 1',
                    location: pin.getLocation(),
                    description: description.join(''),
                    visible: true
                });
            }
        }

         function getCircle(myLatitude, myLongitude, radius) {
            radius = radius + (radius * .016);
            var R = 6371; // earth's mean radius in km
            var lat = (myLatitude * Math.PI) / 180; //rad
            var lon = (myLongitude * Math.PI) / 180; //rad
            var d = parseFloat(radius) / R;  // d = angular distance covered on earth's surface
            var locs = new Array();
            for (x = 0; x <= 360; x++) {
                //var p2 = new VELatLong(0, 0)
                //var p2 = new Microsoft.Maps.Location(0, 0);
                brng = x * Math.PI / 180; //rad
                Latitude = Math.asin(Math.sin(lat) * Math.cos(d) + Math.cos(lat) * Math.sin(d) * Math.cos(brng));
                Longitude = ((lon + Math.atan2(Math.sin(brng) * Math.sin(d) * Math.cos(lat), Math.cos(d) - Math.sin(lat) * Math.sin(Latitude))) * 180) / Math.PI;
                Latitude = (Latitude * 180) / Math.PI;
                locs.push(new Microsoft.Maps.Location(Latitude, Longitude));
            }
            return locs;
        }


         function drawSearchArea() {
            //Complete any current drawing and complete a search for it.
            if (searchArea) {
                tools.finish(searchArea);
                findIntersectingData();
                searchArea = null;
            }
            //Create a new polygon.
            tools.create(Microsoft.Maps.DrawingTools.ShapeType.polygon, function (s) {
                searchArea = s;
            });
        }
        //Find all pushpins on the map that intersect with the drawn search area.
        function findIntersectingData() {
            document.getElementById("<%= hfselected.ClientID %>").value = '';

            //Ensure that the search area is a valid polygon, should have 4 Locations in it's ring as it automatically closes.
            if (searchArea && searchArea.getLocations().length >= 4) {
                //Get all the pushpins from the pinLayer.

                //var pins = clusterLayer.getPrimitives();
                
                //Using spatial math find all pushpins that intersect with the drawn search area.
                //The returned data is a copy of the intersecting data and not a reference to the original shapes, 
                //so making edits to them will not cause any updates on the map.
                var intersectingPins = Microsoft.Maps.SpatialMath.Geometry.intersection(pins, searchArea);

                //The data returned by the intersection function can be null, a single shape, or an array of shapes. 
                if (intersectingPins) {
                    //For ease of use wrap indivudal shapes in an array.
                    if (intersectingPins && !(intersectingPins instanceof Array)) {
                        intersectingPins = [intersectingPins];
                    }
                    
                    var selectedPins = [];

                    //Loop through and map the intersecting pushpins back to their original pushpins by comparing their coordinates.
                    for (var i = 0; i < pins.length; i++) {
                        for (var j = 0; j < intersectingPins.length; j++) {
                            if (Microsoft.Maps.Location.areEqual(pins[i].getLocation(), intersectingPins[j].getLocation())) {
                                selectedPins.push(pins[i]);
                                document.getElementById("<%= hfselected.ClientID %>").value += intersectingPins[j].getLocation().latitude.toString() + "|" + intersectingPins[j].getLocation().longitude.toString() + ":";
                            }
                        }
                    }

                    document.getElementById("<%= hfselected.ClientID %>").value = document.getElementById("<%= hfselected.ClientID %>").value.substring(0, document.getElementById("<%= hfselected.ClientID %>").value.length - 1)
                }
            }
        }

        function removepolygon() {
            drawingManager.clear();
            pinLayer.clear();
            map.entities.clear();
            document.getElementById("<%= hfselected.ClientID %>").value = "";
        }

    </script>
    <table width="100%">
        <tr>
            <td align="right"></td>
            <td align="right">
                <table width="100%">
                    <tr>
                        <td align="left"></td>
                        <td align="right">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnDownload" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnUndo" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>

                                    <asp:Button ID="btnUndo" Text="Reset" runat="server" CssClass="button" Visible="true"
                                        OnClick="btnUndo_Click"></asp:Button>&nbsp
                                        <asp:Button ID="btnDownload" Text="Download" runat="server" CssClass="button" OnClick="btnDownload_Click"
                                            Visible="false"></asp:Button>
                                    <uc:Download runat="server" ID="DownloadPanel1" Visible="false" ViewType="ConsensusView"></uc:Download>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr valign="top">
            <td width="16%">
                <asp:Panel ID="Panel1" runat="server" BackColor="FloralWhite" Height="750px">
                    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
                        AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="TransparentGrayBackground">
                            </div>
                            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10002; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                                <br />
                                <b>Processing...</b><br />
                                <br />
                                <img src="../images/loading.gif" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table>
                                <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                                    <tr>
                                        <td valign="middle" align="center" colspan="2">
                                            <br />
                                            <b>Brand
                                                <asp:Label ID="lblColon" runat="server" Visible="false" Text=":"></asp:Label></b>&nbsp;&nbsp;
                                                <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                                AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                                DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="ReqFldBrand" runat="server" ControlToValidate="drpBrand"
                                                ErrorMessage="*" ValidationGroup="Validate" InitialValue="-1"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center" colspan="2">
                                            <asp:Image ID="imglogo" runat="server" Visible="false"></asp:Image><br />
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td colspan="2" align="left">
                                        <br />
                                        <u><b>Enter an address </u></b><br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><b>Street Address</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="textboxAddress" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><b>City</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="textboxCity" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><b>Country</b>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownListCountry" runat="server" OnSelectedIndexChanged="DropDownListCountry_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="UNITED STATES">United States</asp:ListItem>
                                            <asp:ListItem Value="CANADA">Canada</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><b>State</b>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="DropDownListState" runat="server" DataValueField="RegionCode" DataTextField="RegionCode" Width="145px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><b>Zip Code</b>
                                    </td>
                                    <td align="left">
                                        <telerik:RadMaskedTextBox ID="RadMtxtboxZipCode" Width="145px" runat="server" Mask="#####">
                                        </telerik:RadMaskedTextBox></td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top"><b>Radius</b>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="textboxRadius" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="textboxRadius"
                                            ValidationGroup="Validate" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" MaximumValue="300"
                                            MinimumValue="1" ControlToValidate="textboxRadius" ValidationGroup="Validate"
                                            Type="Integer"></asp:RangeValidator>
                                        <br />
                                        <font style="font-size: x-small">Range (0 to 300 miles)</font>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="2" align="center">
                                        <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="button"
                                            ValidationGroup="Validate" />&nbsp&nbsp
                                               <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="SaveLocation_Click"
                                                   CssClass="button" ValidationGroup="ValidateAddress" />
                                        <asp:Button ID="btnMdlPopupSave" Style="display: none" runat="server"></asp:Button>
                                        <ajaxToolkit:ModalPopupExtender ID="mdlPopSave" runat="server" TargetControlID="btnMdlPopupSave"
                                            PopupControlID="pnlSave" CancelControlID="btnClose" BackgroundCssClass="modalBackground" />
                                        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" TargetControlID="pnlRoundSave"
                                            Radius="6" Corners="All" />
                                        <asp:Panel ID="pnlSave" runat="server" Width="370px" Height="100px" Style="display: none"
                                            CssClass="modalPopup">
                                            <asp:Panel ID="pnlRoundSave" runat="server" Width="370" CssClass="modalPopup2">
                                                <br />
                                                <div align="center" style="text-align: center; height: 150px; padding: 0px 10px 0px 10px;">
                                                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                                                        <tr style="background-color: #5783BD;">
                                                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Save Location
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="padding: 10px" align="left"><b>Location Name</b>
                                                                <asp:TextBox ID="txtLocationName" Width="300px" runat="server" value="" ValidationGroup="SaveLocation" MaxLength="50"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLocationName"
                                                                    ErrorMessage="* required" ValidationGroup="SaveLocation" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnPopupSaveLocation" Text="Save" CssClass="button" ValidationGroup="SaveLocation"
                                                                    runat="server" OnClick="btnPopupSaveLocation_Click" />
                                                                <asp:Button ID="btnClose" Text="Cancel" CssClass="button" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lbMsg" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <table cellpadding="5" border="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <u><b>Saved Locations</b> </u>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <asp:DropDownList ID="drpdownLocations" runat="server" Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpdownLocations"
                                            ErrorMessage="*" ForeColor="Red" Font-Bold="true" InitialValue="*" ValidationGroup="savedlocs"></asp:RequiredFieldValidator>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnLoadLocation" runat="server" CssClass="button" OnClick="btnLoadLocation_Click"
                                            Text="Load" ValidationGroup="savedlocs" />&nbsp;&nbsp;
                                    <asp:Button ID="btnDelReport" runat="server" CssClass="button" OnClick="btnDelLocation_Click"
                                        Text="Delete" ValidationGroup="savedlocs" />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Repeater ID="gvSubscibersByRadius" runat="server" Visible="false">
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr style="background-color: #5783BD; color: White; padding: 0px 0px 0px 0px; height: 20px; font-weight: bold; font-size: medium;">
                                                        <td width="50%">
                                                            <asp:Label ID="lnlRange" runat="server" Text="Range (In Miles)"></asp:Label>
                                                        </td>
                                                        <td width="50%">
                                                            <asp:Label ID="lblSubscribers" runat="server" Text="Subscribers"></asp:Label>
                                                        </td>
                                                    </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td width="50%">
                                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Range") %>'></asp:Label>
                                                    </td>
                                                    <td width="50%">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Subscribers") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="minLatHiddenField" runat="server" />
                            <asp:HiddenField ID="maxLatHiddenField" runat="server" />
                            <asp:HiddenField ID="minLonHiddenField" runat="server" />
                            <asp:HiddenField ID="maxLonHiddenField" runat="server" />
                            <asp:HiddenField ID="countsHiddenField" runat="server" />
                            <asp:HiddenField ID="hfselected" runat="server" />
                            <asp:HiddenField ID="MyLocationLatitude" runat="server" />
                            <asp:HiddenField ID="MyLocationLongitude" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

                <ajaxToolkit:DropShadowExtender ID="uProgress_DropShadowExtender" runat="server" Opacity="0.5" Rounded="true"
                    Enabled="True" TargetControlID="Panel1">
                </ajaxToolkit:DropShadowExtender>
            </td>
            <td width="84%">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <div id="myMap" style="position: relative; width: 100%; height: 750px; cursor: crosshair !important; z-index: 1">
                                <div id="listOfPins" style="max-height:250px;width:250px;overflow-y:scroll;"></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

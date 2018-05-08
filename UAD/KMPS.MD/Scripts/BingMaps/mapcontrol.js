var map;
var greenLayer;
var GeoList = [];
var clusterLayer;
var pins = [];
var infobox;

var GeoDataModel = function (name, subscribers, latitude, longitude) {
    this.Name = name;
    this.Subscribers = subscribers;
    this.Latitude = latitude;
    this.Longitude = longitude;
};

function LoadMap() {
    if (!map) {
        var mapScriptUrl = 'https://www.bing.com/api/maps/mapcontrol?callback=GetMap';
        var script = document.createElement("script");
        script.setAttribute('defer', '');
        script.setAttribute('async', '');
        script.setAttribute("type", "text/javascript");
        script.setAttribute("src", mapScriptUrl);
        document.body.appendChild(script);
    }
    else {
        if (typeof (map) != 'undefined' && map != null) {
            map.dispose();
            map = null;
        }

        var mapScriptUrl = 'https://www.bing.com/api/maps/mapcontrol?callback=GetMap';
        var script = document.createElement("script");
        script.setAttribute('defer', '');
        script.setAttribute('async', '');
        script.setAttribute("type", "text/javascript");
        script.setAttribute("src", mapScriptUrl);
        document.body.appendChild(script);
    }
}

function GetMap() {

    var myMapCoords = $("[id$='myMapCoords']");
    var dt;

    if (myMapCoords.length > 0) {
        if (myMapCoords[0].innerHTML != '')
            dt = eval('(' + myMapCoords[0].innerHTML + ')');
        else
            dt = myMapCoords[0].innerHTML;

        map = new Microsoft.Maps.Map(document.getElementById("myMap"), { credentials: "Ah5_pIcVoon_WSCITtrzU5005iKKX4vNbK5IRAFZwEQ6wdwi7rZDefywKssr5y6o" });
    }

    map.entities.clear();
    pins = [];

    map.setView({ mapTypeId: Microsoft.Maps.MapTypeId.road });

    infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
        visible: false
    });

    //Assign the infobox to a map instance.
    infobox.setMap(map);

    if (dt.MapPoints != undefined) {
        for (var i = 0; i < dt.MapPoints.length; i++) {
            var point = new Microsoft.Maps.Location(dt.MapPoints[i].Lt, dt.MapPoints[i].Lg);
            var shape = new Microsoft.Maps.Pushpin(point, { icon: '../Images/blue-pin.png' });

            shape.metadata = {
                title: 'Subscribers: ' + dt.MapPoints[i].Sc,
            };

            Microsoft.Maps.Events.addHandler(shape, 'mouseover', function (e) {
                infobox.setOptions({
                    location: e.target.getLocation(),
                    visible: true
                });
            });

            Microsoft.Maps.Events.addHandler(shape, 'mousedown', function (e) {
                infobox.setOptions({ visible: false });
            });

            Microsoft.Maps.Events.addHandler(shape, 'mouseout', function (e) {
                infobox.setOptions({ visible: false });
            });

            pins.push(shape);
        }
    }

    map.setView({ center: new Microsoft.Maps.Location(38.8225909761771, -96.05621337890625), zoom: 4 });

    Microsoft.Maps.loadModule("Microsoft.Maps.Clustering", function () {
        clusterLayer = new Microsoft.Maps.ClusterLayer(pins, {
            clusteredPinCallback: function (cluster) {
                callback: createPushpinList,
                cluster.setOptions({
                    icon: '../Images/red-pin.png'
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
    var title2 = [];
    //Check to see if the pushpin is a cluster.

    if (pin.containedPushpins) {
        description.push('Zoom in for more details');
        title2.push('Subscribers: ' + pin.containedPushpins.length)
        title2 = title2.join('');

        //Display an infobox for the pushpin.
        infobox.setOptions({
            title: title2,
            location: pin.getLocation(),
            description: description.join(''),
            visible: true,
            offset: new Microsoft.Maps.Point(0, 20)
        });
    }
    else {
        infobox.setOptions({
            title: 'Subscribers : 1',
            location: pin.getLocation(),
            description: description.join(''),
            visible: true,
            offset: new Microsoft.Maps.Point(0, 20)
        });
    }
}

function getCircle(myLatitude, myLongitude, radius) {
    var R = 6371; // earth's mean radius in km
    var lat = (myLatitude * Math.PI) / 180; //rad
    var lon = (myLongitude * Math.PI) / 180; //rad
    var d = parseFloat(radius) / R;  // d = angular distance covered on earth's surface
    var circlePoints = new Array();

    for (x = 0; x <= 360; x += 5) {
        var p2 = new Microsoft.Maps.Location(0, 0);
        brng = x * Math.PI / 180;
        p2.latitude = Math.asin(Math.sin(lat) * Math.cos(d) + Math.cos(lat) * Math.sin(d) * Math.cos(brng));
        p2.longitude = ((lon + Math.atan2(Math.sin(brng) * Math.sin(d) * Math.cos(lat),
                         Math.cos(d) - Math.sin(lat) * Math.sin(p2.latitude))) * 180) / Math.PI;
        p2.latitude = (p2.latitude * 180) / Math.PI;
        circlePoints.push(p2);
    }

    return circlePoints;
}

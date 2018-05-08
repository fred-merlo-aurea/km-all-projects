
hs.graphicsDir = 'http://www.ecn5.com/highslide/graphics/';
hs.outlineType = 'rounded-white';
hs.allowSizeReduction = 'false';

function toastrNotification(NotificationTitle, NotificationMessage) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-ECN-style",
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "10000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    toastr.info(NotificationMessage);
    //$("[id*='toast-container'] img").css("background-color", "red");
    var allImages = $("[id*='toast-container'] img");
    for (var i = 0; i < allImages.length; i++) {
        if (allImages[i].width > 490) {
            var curImg = $("img[src$='" + allImages[i].src + "']");
            curImg.attr("width", "490");
        }
    }

    SetColors();

    $(".toast").css("position", "relative");
    $(".toast").css("margin-top", "-26%");
    $(".toast").css("left", "33%");
}

function SetColors() {
    $(".toast-info").css("background-color", $("[id*='colorTwo']").val());
    $(".toast-close-button").css("color", $("[id*='colorOne']").val());
    $(".toast-close-button:hover").css("color", $("[id*='colorOne']").val());
}

function OnClientColorChangeRcp1(sender, eventArgs) {
    var color = sender.get_selectedColor();
    $("[id*='colorOne']").val(color);
    SetColors();
}
function OnClientColorChangeRcp2(sender, eventArgs) {
    var color = sender.get_selectedColor();
    $("[id*='colorTwo']").val(color);
    SetColors();
}


function show() {
    getobj("divhelp").style.display = (getobj("divhelp").style.display == 'none') ? "block" : "none";
}

function getobj(id) {
    if (document.all && !document.getElementById)
        obj = eval('document.all.' + id);
    else if (document.layers)
        obj = eval('document.' + id);
    else if (document.getElementById)
        obj = document.getElementById(id);
    return obj;
}


function flvFPW1() {
    var v1 = arguments, v2 = v1[2].split(","), v3 = (v1.length > 3) ? v1[3] : false, v4 = (v1.length > 4) ? parseInt(v1[4]) : 0, v5 = (v1.length > 5) ? parseInt(v1[5]) : 0, v6, v7 = 0, v8, v9, v10, v11, v12, v13, v14, v15, v16, v17, v18;
    if (v4 > 1) {
        v10 = screen.width;
        for (v6 = 0; v6 < v2.length; v6++) {
            v18 = v2[v6].split("=");
            if (v18[0] == "width") {
                v8 = parseInt(v18[1]);
            }
            if (v18[0] == "left") {
                v9 = parseInt(v18[1]); v11 = v6;
            }
        }
        if (v4 == 2) {
            v7 = (v10 - v8) / 2; v11 = v2.length;
        } else if (v4 == 3) {
            v7 = v10 - v8 - v9;
        }
        v2[v11] = "left=" + v7;
    }
    if (v5 > 1) {
        v14 = screen.height;
        for (v6 = 0; v6 < v2.length; v6++) {
            v18 = v2[v6].split("=");
            if (v18[0] == "height") {
                v12 = parseInt(v18[1]);
            } if (v18[0] == "top") {
                v13 = parseInt(v18[1]); v15 = v6;
            }
        }
        if (v5 == 2) {
            v7 = (v14 - v12) / 2; v15 = v2.length;
        } else if (v5 == 3) {
            v7 = v14 - v12 - v13;
        }
        v2[v15] = "top=" + v7;
    }
    v16 = v2.join(",");
    v17 = window.open(v1[0], v1[1], v16);
    if (v3) {
        v17.focus();
    }
    document.MM_returnValue = false;
}



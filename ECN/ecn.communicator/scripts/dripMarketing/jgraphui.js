(function($){$.fn._t=function(nodeId,dgrm)
{
var self=$(this);
self.value=self.text();
self.bind('dblclick', function () {
    var initWidth = $(this)
        .width();
    var initHeight = $(this)
        .height();
    self.html('<textarea style="background: url(\'images/transparent.gif\')">' + $.trim(self.value.replace(/<br\s?\/?>/g, "\n")) + '</textarea>')
        .find('textarea')
        .bind('blur', function (event) {
        self.value = $.trim($(this)
            .val()
            .replace(/\r\n|\r|\n/g, "<br />"));
        self.html(self.value);
        dgrm.updateNodeContent(nodeId, self.value)
    })
        .keydown(function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 13) {} else if (keyCode == 27) {
            $(this)
                .val(self.value);
            $(this)
                .blur()
        }
    })
        .width(initWidth)
        .height(initHeight)
        .css({
        'border': '0'
    })
        .focus()
})
}
})(jQuery);

function dtc(node, direction, port, canvasobj, line) {
    direction = node + "_" + direction;
    var startx;
    var starty;
    var endx = port.position()
        .left + $("#" + node)
        .position()
        .left + $(canvasobj.getCanvas)
        .scrollLeft();
    var endy = port.position()
        .top + $("#" + node)
        .position()
        .top + $(canvasobj.getCanvas)
        .scrollTop();
    var nodex = $("#" + node)
        .position()
        .left + $(canvasobj.getCanvas)
        .scrollLeft();
    var nodey = $("#" + node)
        .position()
        .top + $(canvasobj.getCanvas)
        .scrollTop();
    var nw = $("#" + node)
        .width();
    var nh = $("#" + node)
        .height();
    var hnw = Math.floor(nw / 2);
    var hnh = Math.floor(nh / 2);
    var zone;
    var portNumber;
    if (direction.match(/_n$/)) {
        startx = nodex + hnw;
        starty = nodey;
        portNumber = 1
    } else if (direction.match(/_e$/)) {
        startx = nodex + nw;
        starty = nodey + hnh;
        portNumber = 2
    } else if (direction.match(/_s$/)) {
        startx = nodex + hnw;
        starty = nodey + nh;
        portNumber = 3
    } else if (direction.match(/_w$/)) {
        startx = nodex;
        starty = nodey + hnh;
        portNumber = 4
    }
    if (endx <= (nodex + hnw)) {
        if (endy <= (nodey + hnh)) {
            if (endy <= nodey) {
                if (endy >= nodey - 10 && endx > nodex) {
                    zone = "Z1A10";
                    line.clear()
                } else {
                    if (endx <= nodex - 10) {
                        zone = "Z8";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z8";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z8";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z8";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                            line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                            line.drawLine(endx, nodey + nh + 10, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z8";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, endx, nodey + hnh);
                            line.drawLine(endx, nodey + hnh, endx, endy);
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z1A";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z1A";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z1A";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z1A";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                            line.drawLine((nodex) + hnw, nodey + nh + 10, nodex - 10, nodey + nh + 10);
                            line.drawLine(nodex - 10, nodey + nh + 10, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z1A";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    }
                }
            } else {
                if (endx <= nodex - 10) {
                    zone = "Z7B";
                    switch (portNumber) {
                    case 1:
                        zone = "N ---> Z7B";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                        line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break;
                    case 2:
                        zone = "E ---> Z7B";
                        line.clear();
                        line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                        line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, nodey - 10);
                        line.drawLine((nodex) + nw + 10, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break;
                    case 3:
                        zone = "S ---> Z7B";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                        line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break;
                    case 4:
                        zone = "W ---> Z7B";
                        line.clear();
                        line.drawLine(nodex, nodey + hnh, endx, nodey + hnh);
                        line.drawLine(endx, nodey + hnh, endx, endy);
                        line.paint();
                        break
                    }
                } else {
                    zone = "Z7B10";
                    line.clear()
                }
            }
        } else {
            if (endy >= nodey + nh) {
                if ((endy <= nodey + nh + 10) && (endx >= nodex)) {
                    zone = "Z5B10";
                    line.clear()
                } else {
                    if (endx <= nodex - 10) {
                        zone = "Z6";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z6";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                            line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                            line.drawLine(endx, nodey - 10, endx, endy - 10);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z6";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z6";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z6";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, endx, nodey + hnh);
                            line.drawLine(endx, nodey + hnh, endx, endy);
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z5B";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z5B";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                            line.drawLine((nodex) + hnw, nodey - 10, nodex - 20, nodey - 10);
                            line.drawLine(nodex - 20, nodey - 10, nodex - 20, endy);
                            line.drawLine(nodex - 20, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z5B";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z5B";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z5B";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    }
                }
            } else {
                if (endx <= nodex - 10) {
                    switch (portNumber) {
                    case 1:
                        zone = "N ---> Z7A";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                        line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break;
                    case 2:
                        zone = "E ---> Z7A";
                        line.clear();
                        line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                        line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, nodey + nh + 10);
                        line.drawLine((nodex) + nw + 10, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break;
                    case 3:
                        zone = "S ---> Z7A";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                        line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break;
                    case 4:
                        zone = "W ---> Z7A";
                        line.clear();
                        line.drawLine(nodex, nodey + hnh, endx, nodey + hnh);
                        line.drawLine(endx, nodey + hnh, endx, endy);
                        line.paint();
                        break
                    }
                } else {
                    zone = "Z7A10";
                    line.clear()
                }
            }
        }
    } else {
        if (endy <= (nodey + hnh)) {
            if (endy <= nodey) {
                if (endy >= nodey - 10 && endx <= nw) {
                    zone = "Z1B10";
                    line.clear()
                } else {
                    if (endx <= nodex + nw) {
                        zone = "Z1B";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z1B";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z1B";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z1B";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                            line.drawLine((nodex) + hnw, nodey + nh + 10, nodex + nw + 10, nodey + nh + 10);
                            line.drawLine(nodex + nw + 10, nodey + nh + 10, nodex + nw + 10, endy);
                            line.drawLine(nodex + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z1B";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z2";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z2";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z2";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, endx, nodey + hnh);
                            line.drawLine(endx, nodey + hnh, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z2";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                            line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                            line.drawLine(endx, nodey + nh + 10, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z2";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    }
                }
            } else {
                if (endx <= nodex + nw + 10) {
                    zone = "Z3A10";
                    line.clear()
                } else {
                    zone = "Z3A";
                    switch (portNumber) {
                    case 1:
                        zone = "N ---> Z3A";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                        line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break;
                    case 2:
                        zone = "E ---> Z3A";
                        line.clear();
                        line.drawLine((nodex) + nw, nodey + hnh, endx, nodey + hnh);
                        line.drawLine(endx, nodey + hnh, endx, endy);
                        line.paint();
                        break;
                    case 3:
                        zone = "S ---> Z3A";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                        line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break;
                    case 4:
                        zone = "W ---> Z3A";
                        line.clear();
                        line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                        line.drawLine(nodex - 10, nodey + hnh, nodex - 10, nodey - 10);
                        line.drawLine(nodex - 10, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break
                    }
                }
            }
        } else {
            if (endy >= nodey + nh) {
                if ((endy <= nodey + nh + 10) && (endx <= nodex + nw)) {
                    zone = "Z5A10";
                    line.clear()
                } else {
                    if (endx <= nodex + nw) {
                        zone = "Z5A";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z5A";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                            line.drawLine((nodex) + hnw, nodey - 10, nodex + nw + 10, nodey - 10);
                            line.drawLine(nodex + nw + 10, nodey - 10, nodex + nw + 10, endy);
                            line.drawLine(nodex + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z5A";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, (nodex) + nw + 10, nodey + hnh);
                            line.drawLine((nodex) + nw + 10, nodey + hnh, (nodex) + nw + 10, endy);
                            line.drawLine((nodex) + nw + 10, endy, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z5A";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z5A";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z4";
                        switch (portNumber) {
                        case 1:
                            zone = "N ---> Z4";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                            line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                            line.drawLine(endx, nodey - 10, endx, endy);
                            line.paint();
                            break;
                        case 2:
                            zone = "E ---> Z4";
                            line.clear();
                            line.drawLine((nodex) + nw, nodey + hnh, endx, nodey + hnh);
                            line.drawLine(endx, nodey + hnh, endx, endy);
                            line.paint();
                            break;
                        case 3:
                            zone = "S ---> Z4";
                            line.clear();
                            line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, endy);
                            line.drawLine((nodex) + hnw, endy, endx, endy);
                            line.paint();
                            break;
                        case 4:
                            zone = "W ---> Z4";
                            line.clear();
                            line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                            line.drawLine(nodex - 10, nodey + hnh, nodex - 10, endy);
                            line.drawLine(nodex - 10, endy, endx, endy);
                            line.paint();
                            break
                        }
                    }
                }
            } else {
                if (endx <= nodex + nw + 10) {
                    zone = "Z3B10";
                    line.clear()
                } else {
                    zone = "Z3B";
                    switch (portNumber) {
                    case 1:
                        zone = "N ---> Z3B";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey, (nodex) + hnw, nodey - 10);
                        line.drawLine((nodex) + hnw, nodey - 10, endx, nodey - 10);
                        line.drawLine(endx, nodey - 10, endx, endy);
                        line.paint();
                        break;
                    case 2:
                        zone = "E ---> Z3B";
                        line.clear();
                        line.drawLine((nodex) + nw, nodey + hnh, endx, nodey + hnh);
                        line.drawLine(endx, nodey + hnh, endx, endy);
                        line.paint();
                        break;
                    case 3:
                        zone = "S ---> Z3B";
                        line.clear();
                        line.drawLine((nodex) + hnw, nodey + nh, (nodex) + hnw, nodey + nh + 10);
                        line.drawLine((nodex) + hnw, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break;
                    case 4:
                        zone = "W ---> Z3B";
                        line.clear();
                        line.drawLine(nodex, nodey + hnh, nodex - 10, nodey + hnh);
                        line.drawLine(nodex - 10, nodey + hnh, nodex - 10, nodey + nh + 10);
                        line.drawLine(nodex - 10, nodey + nh + 10, endx, nodey + nh + 10);
                        line.drawLine(endx, nodey + nh + 10, endx, endy);
                        line.paint();
                        break
                    }
                }
            }
        }
    }
};
Toolbar=function(dgrm,params)
{
	var toolbar=this;

	if(!params)
	{	params={'empty':'empty'}	}

	toolbar.xPosition=(params["xPosition"])?params["xPosition"]:20;toolbar.yPosition=(params["yPosition"])?params["yPosition"]:30;
	toolbar.width=(params["width"])?params["width"]:1000;

	toolbar.add_button=(params["add_button"]==false)?params["add_button"]:true;
	toolbar.save_button=(params["save_button"]==false)?params["save_button"]:true;	
	toolbar.load_button=(params["load_button"]==false)?params["load_button"]:true;
	
	toolbar.configure_button=(params["configure_button"]==false)?params["configure_button"]:true;
	toolbar.delete_button=(params["delete_button"]==false)?params["delete_button"]:true;
	toolbar.background_color_button=(params["background_color_button"]==false)?params["background_color_button"]:true;
	toolbar.border_color_button=(params["border_color_button"]==false)?params["border_color_button"]:true;
	toolbar.font_color_button=(params["font_color_button"]==false)?params["font_color_button"]:true;
	toolbar.font_size_button=(params["font_size_button"]==false)?params["font_size_button"]:true;
	toolbar.font_family_button=(params["font_family_button"]==false)?params["font_family_button"]:true;
	toolbar.border_width_button=(params["border_width_button"]==false)?params["border_width_button"]:true;

	//Under AddAction Button
	toolbar.defaultNodePalette=[
	//{ 'order': '1', 'color': '000000', 'icon': dgrm.imagesPath + 'node.gif', 'nodeType': 'NODE', 'nodeContent': 'Node Content', 'width': '100', 'height': '100', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 40, 'maxHeight': 200, 'minWidth': 40, 'maxWidth': 200, 'nPort': true, 'ePort': true, 'sPort': true, 'wPort': true, 'image': '', 'draggable': true, 'resizable': true, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': true, 'ePortMakeConnection': true, 'sPortMakeConnection': true, 'wPortMakeConnection': true, 'nPortAcceptConnection': true, 'ePortAcceptConnection': true, 'sPortAcceptConnection': true, 'wPortAcceptConnection': true },

	//{'order':'2','color':'572342','icon':dgrm.imagesPath+'text2.gif','nodeType':'TEXT','nodeContent':'Node //Content','width':'100','height':'40','bgColor':'#FFFFFF','borderColor':'#AAAAAA','borderWidth':'1','fontColor':'#000000','fontSize':'','fontType':'','minHeight':40,'maxHeight':200,'minWidth':40,'maxWidth':200,'nPort':true,'//ePort':true,'sPort':true,'wPort':true,'image':'','draggable':true,'resizable':true,'editable':true,'selectable':true,'deletable':true,'nPortMakeConnection':true,'ePortMakeConnection':true,'sPortMakeConnection':true,'wPortMa//keConnection':true,'nPortAcceptConnection':true,'ePortAcceptConnection':true,'sPortAcceptConnection':true,'wPortAcceptConnection':true},

	{'order': '3', 'color': '123456', 'icon': dgrm.imagesPath + 'start_small.png', 'nodeType': 'IMAGE', 'nodeContent': 'START', 'width': '40', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': false, 'image': dgrm.imagesPath + 'start.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': true, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': false },

	//{'order':'4','color':'876543','icon':dgrm.imagesPath+'timer.png','nodeType':'IMAGE','nodeContent':'TIMER','width':'50','height':'50','bgColor':'#FFFFFF','borderColor':'#AAAAAA','borderWidth':'1','fontColor':'#000000','fontSize':'','fontType':'','minHeight':50,'maxHeight':85,'minWidth':50,'maxWidth':85,'nPort':false,'ePort':true,'sPort':false,'wPort':true,'image':dgrm.imagesPath+'timer.png','imageWidth':50,'imageHeight':50,'draggable':true,'resizable':false,'editable':true,'selectable':true,'deletable':true,'nPortMakeConnection':true,'ePortMakeConnection':true,'sPortMakeConnection':true,'wPortMakeConnection':true,'nPortAcceptConnection':true,'ePortAcceptConnection':true,'sPortAcceptConnection':true,'wPortAcceptConnection':true},

	{'order': '5', 'color': '0e2532', 'icon': dgrm.imagesPath + 'stop_small.png', 'nodeType': 'IMAGE', 'nodeContent': 'STOP', 'width': '40', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': false, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'stop.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': false, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': true },

	{ 'order': '6', 'color': '0e2532', 'icon': dgrm.imagesPath + 'click_old.png', 'nodeType': 'IMAGE', 'nodeContent': 'CLICK', 'width': '85', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'click.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': true, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': true },

    { 'order': '7', 'color': '0e2532', 'icon': dgrm.imagesPath + 'open_small.png', 'nodeType': 'IMAGE', 'nodeContent': 'OPEN', 'width': '40', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'open.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': true, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': true },

    { 'order': '8', 'color': '0e2532', 'icon': dgrm.imagesPath + 'noclick_small.png', 'nodeType': 'IMAGE', 'nodeContent': 'NOCLICK', 'width': '40', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'noclick.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': true, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': true },

    { 'order': '9', 'color': '0e2532', 'icon': dgrm.imagesPath + 'noopen_small.png', 'nodeType': 'IMAGE', 'nodeContent': 'NOOPEN', 'width': '40', 'height': '40', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 65, 'maxHeight': 65, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'noopen.png', 'imageWidth': 60, 'imageHeight': 65, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': false, 'ePortMakeConnection': true, 'sPortMakeConnection': false, 'wPortMakeConnection': false, 'nPortAcceptConnection': false, 'ePortAcceptConnection': false, 'sPortAcceptConnection': false, 'wPortAcceptConnection': true },


	{ 'order': '10', 'color': '0e2532', 'icon': dgrm.imagesPath + 'email.png', 'nodeType': 'IMAGE', 'nodeContent': 'EMAIL', 'width': '60', 'height': '60', 'bgColor': '#FFFFFF', 'borderColor': '#AAAAAA', 'borderWidth': '1', 'fontColor': '#000000', 'fontSize': '', 'fontType': '', 'minHeight': 60, 'maxHeight': 60, 'minWidth': 60, 'maxWidth': 60, 'nPort': false, 'ePort': true, 'sPort': false, 'wPort': true, 'image': dgrm.imagesPath + 'email.png', 'imageWidth': 60, 'imageHeight': 60, 'draggable': true, 'resizable': false, 'editable': true, 'selectable': true, 'deletable': true, 'nPortMakeConnection': true, 'ePortMakeConnection': true, 'sPortMakeConnection': true, 'wPortMakeConnection': true, 'nPortAcceptConnection': true, 'ePortAcceptConnection': true, 'sPortAcceptConnection': true, 'wPortAcceptConnection': true}];


	toolbar.getNodeByOrder = function (order) {
	    for (i = 0; i < toolbar.defaultNodePalette.length; i++) {
	        if (toolbar.defaultNodePalette[i]['order'] == order) {
	            return (toolbar.defaultNodePalette[i])
	        }
	    }
    };
	toolbarExists=false;
	if(toolbar.add_button||toolbar.save_button||toolbar.load_button||toolbar.delete_button||toolbar.configure_button||toolbar.background_color_button||toolbar.border_color_button||toolbar.font_color_button||toolbar.font_size_button||toolbar.border_width_button||toolbar.font_family_button)
	{	
		toolbarExists=true
	}

	if(toolbarExists)
	{
		var bar='<div id="toolbar_'+dgrm.canvasID+'" style="position: absolute;left:'+toolbar.xPosition+'px; top:'+toolbar.yPosition+'px; width:'+toolbar.width+'px; height:30px;padding:2px;" class="ui-widget-header ui-corner-all">';

		if(toolbar.add_button)
		{			bar+='<button id="add_button_'+dgrm.canvasId+'">Add Action</button>'		}

		if(toolbar.delete_button)
		{			bar+='<button id="delete_button_'+dgrm.canvasId+'">Delete Action</button>'		}

		if(toolbar.save_button)
		{				bar+='<button id="save_button_'+dgrm.canvasId+'">Save Campaign</button>'		}
		
		if(toolbar.load_button)
		{				bar+='<button id="load_button_'+dgrm.canvasId+'">Load Campaign</button>'		}
		
		if(toolbar.configure_button)
		{				bar+='<button id="configure_button_'+dgrm.canvasId+'">Configure Action</button>'		}


		if(toolbar.background_color_button)
		{			bar+='<button id="bgcolor_button_'+dgrm.canvasId+'">Background Color</button>'		}
		if(toolbar.border_color_button)
		{			bar+='<button id="bcolor_button_'+dgrm.canvasId+'">Border Color</button>'		}
		if(toolbar.font_color_button)
		{bar+='<button id="fcolor_button_'+dgrm.canvasId+'">Font Color</button>'}
		if(toolbar.font_size_button)
		{bar+='<button id="fsize_button_'+dgrm.canvasId+'">Font Size</button>'}
		if(toolbar.font_family_button)
		{bar+='<button id="ffamily_button_'+dgrm.canvasId+'">Font Family</button>'}
		if(toolbar.border_width_button)
		{bar+='<button id="bwidth_button_'+dgrm.canvasId+'">Border Width</button>'}

		bar+='</div>';$('body').append(bar);

		if(toolbar.add_button)
		{$("#add_button_"+dgrm.canvasId).button({icons:{primary:"add_node"}}).simpleNodePalette(dgrm,toolbar.defaultNodePalette)}

		if(toolbar.delete_button)
		{$("#delete_button_"+dgrm.canvasId).button({icons:{primary:"delete_node"}}).click(function(){dgrm.deleteSelectedNodes()})}

		if(toolbar.save_button)
		{$("#save_button_"+dgrm.canvasId).button({icons:{primary:"save_diagram"}}).click(function(){dgrm.toXML()})}
		
		if(toolbar.load_button)
		{$("#load_button_"+dgrm.canvasId).button({icons:{primary:"load_diagram"}}).click(function(){dgrm.loadTemplate()})}
		
		if(toolbar.configure_button)
		{$("#configure_button_"+dgrm.canvasId).button({icons:{primary:"configure_diagram"}}).click(function(){dgrm.configureAction()})}


		if(toolbar.border_color_button)
		{$("#bgcolor_button_"+dgrm.canvasId).button({icons:{primary:"background_color"}}).simpleColorPalette(dgrm,'bgcolor')}

		if(toolbar.border_color_button)
		{$("#bcolor_button_"+dgrm.canvasId).button({icons:{primary:"border_color"}}).simpleColorPalette(dgrm,'bordercolor')}

		if(toolbar.font_color_button)
		{$("#fcolor_button_"+dgrm.canvasId).button({icons:{primary:"font_color"}}).simpleColorPalette(dgrm,'fontcolor')}

		if(toolbar.font_size_button)
		{$("#fsize_button_"+dgrm.canvasId).button({icons:{primary:"font_size"}}).simpleFontSizePalette(dgrm)}

		if(toolbar.font_family_button)
		{$("#ffamily_button_"+dgrm.canvasId).button({icons:{primary:"font_family"}}).simpleFontFamilyPalette(dgrm)}

		if(toolbar.border_width_button)
		{$("#bwidth_button_"+dgrm.canvasId).button({icons:{primary:"border_width"}}).simpleBorderWidthPalette(dgrm)}
	}
};
(function ($) {
    $.fn.simpleNodePalette = function (dgrm, defaultNodes) {
        var self = $(this);
        var id = self.attr("id") + "_" + dgrm.canvasId;
        var container = '<div id="' + id + '" class="simple_node_palette"></div>';
        $("body")
            .append(container);
        $("#" + id)
            .hide();
        self.click(function () {
            var x = self.offset()
                .left;
            var y = self.offset()
                .top + self.height() + 8;
            $("#" + id)
                .css('left', x);
            $("#" + id)
                .css('top', y);
            $("#" + id)
                .show()
        });
        hidePalette = function () {
            $(".simple_node_palette")
                .hide()
        };
        checkMouse = function (event) {
            var selector = ".simple_node_palette";
            var selectorParent = $(event.target)
                .parents(selector)
                .length;
            if (event.target == $(selector)[0] || selectorParent > 0) {
                return
            }
            hidePalette()
        };
        $(document)
            .bind("mousedown", checkMouse);
        $.each(defaultNodes, function (i) {
            swatch = $("<div id='" + this['order'] + "_" + dgrm.canvasId + "' class='node_palette_item'><img width=40px height=40px border=0 src='" + this['icon'] + "'/></div>");
            swatch.css("background", "#EEEEEE");
            swatch.draggable({
                helper: "clone",
                stop: function (event, ui) {
                    hidePalette()
                }
            });
            swatch.bind("mouseover", function (e) {
                $(this)
                    .css("border", "1px solid #AAAAAA")
            });
            swatch.bind("mouseout", function (e) {
                $(this)
                    .css("border", "1px solid #EEEEEE")
            });
            swatch.appendTo("#" + id)
        })
    }
})(jQuery);
(function ($) {
    $.fn.simpleColorPalette = function (dgrm, color) {
        var self = $(this);
        defaultColors = ['000000', '585858', '6E6E6E', '848484', 'A4A4A4', 'BDBDBD', 'D8D8D8', 'E6E6E6', 'FFFFFF', '610B21', '8A0829', 'B40431', 'DF013A', 'FF0040', 'FE2E64', 'FA5882', 'F7819F', 'F5A9BC', '610B5E', '8A0886', 'B404AE', 'DF01D7', 'FF00FF', 'FE2EF7', 'FA58F4', 'F781F3', 'F5A9F2', '0B0B61', '08088A', '0404B4', '0101DF', '0000FF', '2E2EFE', '5858FA', '8181F7', 'A9A9F5', '0B3861', '084B8A', '045FB4', '0174DF', '0080FF', '2E9AFE', '58ACFA', '81BEF7', 'A9D0F5', '088A85', '04B4AE', '01DFD7', '00FFFF', '2EFEF7', '58FAF4', '81F7F3', 'A9F5F2', 'CEF6F5', '0B610B', '088A08', '04B404', '01DF01', '00FF00', '2EFE2E', '58FA58', '81F781', 'A9F5A9', '21610B', '298A08', '31B404', '3ADF00', '40FF00', '64FE2E', '82FA58', '9FF781', 'BCF5A9', '5E610B', '868A08', 'AEB404', 'D7DF01', 'FFFF00', 'F7FE2E', 'F4FA58', 'F3F781', 'F2F5A9', '61380B', '8A4B08', 'B45F04', 'DF7401', 'FF8000', 'FE9A2E', 'FAAC58', 'F7BE81', 'F5D0A9', '610B0B', '8A0808', 'B40404', 'DF0101', 'FF0000', 'FE2E2E', 'FA5858', 'F78181', 'F5A9A9'];
        var id = self.attr("id") + "_" + dgrm.canvasId;
        var container = '<div id="' + id + '" class="simple_color_palette"></div>';
        $("body")
            .append(container);
        $("#" + id)
            .hide();
        self.click(function () {
            var x = self.offset()
                .left;
            var y = self.offset()
                .top + self.height() + 8;
            $("#" + id)
                .css('left', x);
            $("#" + id)
                .css('top', y);
            $("#" + id)
                .show()
        });
        hideColorPalette = function () {
            $(".simple_color_palette")
                .hide()
        };
        checkColorMouse = function (event) {
            var selector = ".simple_color_palette";
            var selectorParent = $(event.target)
                .parents(selector)
                .length;
            if (event.target == $(selector)[0] || selectorParent > 0) {
                return
            }
            hideColorPalette()
        };
        $(document)
            .bind("mousedown", checkColorMouse);
        $.each(defaultColors, function (i) {
            swatch = $("<div id='" + this + "' class='color_palette_item'>&nbsp;</div>");
            swatch.css("background-color", "#" + this);
            swatch.click(function (event) {
                if (color == 'bgcolor') {
                    dgrm.updateSelectedNodesBGColor("#" + event.target.id);
                    hideColorPalette()
                }
                if (color == 'bordercolor') {
                    dgrm.updateSelectedNodesBorderColor("#" + event.target.id);
                    hideColorPalette()
                }
                if (color == 'fontcolor') {
                    dgrm.updateSelectedNodesFontColor("#" + event.target.id);
                    hideColorPalette()
                }
            });
            swatch.bind("mouseover", function (e) {
                $(this)
                    .css("border", "1px solid #AAAAAA")
            });
            swatch.bind("mouseout", function (e) {
                $(this)
                    .css("border", "1px solid #EEEEEE")
            });
            swatch.appendTo("#" + id)
        })
    }
})(jQuery);
(function ($) {
    $.fn.simpleFontSizePalette = function (dgrm) {
        var self = $(this);
        defaultFontSizes = ['7pt', '9pt', '11pt', '13pt', '15pt', '17pt'];
        var id = self.attr("id") + "_" + dgrm.canvasId;
        var container = '<div id="' + id + '" class="simple_font_size_palette"></div>';
        $("body")
            .append(container);
        $("#" + id)
            .hide();
        self.click(function () {
            var x = self.offset()
                .left;
            var y = self.offset()
                .top + self.height() + 8;
            $("#" + id)
                .css('left', x);
            $("#" + id)
                .css('top', y);
            $("#" + id)
                .show()
        });
        hideFontSizePalette = function () {
            $(".simple_font_size_palette")
                .hide()
        };
        checkFontSizeMouse = function (event) {
            var selector = ".simple_font_size_palette";
            var selectorParent = $(event.target)
                .parents(selector)
                .length;
            if (event.target == $(selector)[0] || selectorParent > 0) {
                return
            }
            hideFontSizePalette()
        };
        $(document)
            .bind("mousedown", checkFontSizeMouse);
        $.each(defaultFontSizes, function (i) {
            swatch = $("<div id='" + this + "' class='font_size_palette_item' style='font-size:" + this + "'>Font Size</div>");
            swatch.click(function (event) {
                dgrm.updateSelectedNodesFontSize(event.target.id);
                hideFontSizePalette()
            });
            swatch.bind("mouseover", function (e) {
                $(this)
                    .css("border", "1px solid #AAAAAA")
            });
            swatch.bind("mouseout", function (e) {
                $(this)
                    .css("border", "1px solid #EEEEEE")
            });
            swatch.appendTo("#" + id)
        })
    }
})(jQuery);
(function ($) {
    $.fn.simpleFontFamilyPalette = function (dgrm) {
        var self = $(this);
        defaultFontFamilies = ['Arial', 'Georgia', 'Impact', 'Verdana', 'Monospace', 'Tahoma', 'Serif'];
        var id = self.attr("id") + "_" + dgrm.canvasId;
        var container = '<div id="' + id + '" class="simple_font_family_palette"></div>';
        $("body")
            .append(container);
        $("#" + id)
            .hide();
        self.click(function () {
            var x = self.offset()
                .left;
            var y = self.offset()
                .top + self.height() + 8;
            $("#" + id)
                .css('left', x);
            $("#" + id)
                .css('top', y);
            $("#" + id)
                .show()
        });
        hideFontFamilyPalette = function () {
            $(".simple_font_family_palette")
                .hide()
        };
        checkFontFamilyMouse = function (event) {
            var selector = ".simple_font_family_palette";
            var selectorParent = $(event.target)
                .parents(selector)
                .length;
            if (event.target == $(selector)[0] || selectorParent > 0) {
                return
            }
            hideFontFamilyPalette()
        };
        $(document)
            .bind("mousedown", checkFontFamilyMouse);
        $.each(defaultFontFamilies, function (i) {
            swatch = $("<div id='" + this + "' class='font_family_palette_item' style='font-family:" + this + "'>" + this + "</div>");
            swatch.click(function (event) {
                dgrm.updateSelectedNodesFontType(event.target.id);
                hideFontFamilyPalette()
            });
            swatch.bind("mouseover", function (e) {
                $(this)
                    .css("border", "1px solid #AAAAAA")
            });
            swatch.bind("mouseout", function (e) {
                $(this)
                    .css("border", "1px solid #EEEEEE")
            });
            swatch.appendTo("#" + id)
        })
    }
})(jQuery);
(function ($) {
    $.fn.simpleBorderWidthPalette = function (dgrm) {
        var self = $(this);
        defaultBorderWidths = ['0', '1', '2', '3', '4', '5', '6', '7'];
        var id = self.attr("id") + "_" + dgrm.canvasId;
        var container = '<div id="' + id + '" class="simple_border_width_palette"></div>';
        $("body")
            .append(container);
        $("#" + id)
            .hide();
        self.click(function () {
            var x = self.offset()
                .left;
            var y = self.offset()
                .top + self.height() + 8;
            $("#" + id)
                .css('left', x);
            $("#" + id)
                .css('top', y);
            $("#" + id)
                .show()
        });
        hideBorderWidthPalette = function () {
            $(".simple_border_width_palette")
                .hide()
        };
        checkBorderWidthMouse = function (event) {
            var selector = ".simple_border_width_palette";
            var selectorParent = $(event.target)
                .parents(selector)
                .length;
            if (event.target == $(selector)[0] || selectorParent > 0) {
                return
            }
            hideBorderWidthPalette()
        };
        $(document)
            .bind("mousedown", checkBorderWidthMouse);
        $.each(defaultBorderWidths, function (i) {
            hrfor = this;
            swatch = $("<div id='" + this + "' class='border_width_palette_item'><hr id='" + this + "_hr' style='border:0;height:" + this + "px;background-color: #000000;'/></div>");
            swatch.click(function (event) {
                dgrm.updateSelectedNodesBorderWidth((event.target.id)
                    .replace('_hr', ''));
                hideBorderWidthPalette()
            });
            swatch.bind("mouseover", function (e) {
                $(this)
                    .css("border", "1px solid #AAAAAA")
            });
            swatch.bind("mouseout", function (e) {
                $(this)
                    .css("border", "1px solid #EEEEEE")
            });
            swatch.appendTo("#" + id)
        })
    }
})(jQuery);

Node=function(params)
{
var node=this;
if(!params)params={'empty':'empty'};
node.nodeId = (params["nodeId"]) ? params["nodeId"] : new Date().getTime() + Math.floor(Math.random() * 11111) +'-'+ params["nodeContent"];
node.nodeType=(params["nodeType"])?params["nodeType"]:"NODE";
node.nodeContent=(params["nodeContent"])?params["nodeContent"]:"Edit";
node.xPosition=(params["xPosition"])?params["xPosition"]:100;
node.yPosition=(params["yPosition"])?params["yPosition"]:100;
node.width=(params["width"])?params["width"]:100;
node.height=(params["height"])?params["height"]:100;
node.bgColor=(params["bgColor"])?params["bgColor"]:"#FFFFFF";
node.borderColor=(params["borderColor"])?params["borderColor"]:"#AAAAAA";
node.borderWidth=(params["borderWidth"])?params["borderWidth"]:1;
node.fontColor=(params["fontColor"])?params["fontColor"]:"#0072bc";
node.fontSize=(params["fontSize"])?params["fontSize"]:"7pt";
node.fontType=(params["fontType"])?params["fontType"]:"verdana";
node.minHeight=(params["minHeight"])?params["minHeight"]:40;
node.maxHeight=(params["maxHeight"])?params["maxHeight"]:400;
node.minWidth=(params["minWidth"])?params["minWidth"]:40;
node.maxWidth=(params["maxWidth"])?params["maxWidth"]:400;
node.nPort=(params["nPort"]==false)?params["nPort"]:true;
node.ePort=(params["ePort"]==false)?params["ePort"]:true;
node.sPort=(params["sPort"]==false)?params["sPort"]:true;
node.wPort=(params["wPort"]==false)?params["wPort"]:true;
node.nPortMakeConnection=(params["nPortMakeConnection"]==false)?params["nPortMakeConnection"]:true;
node.ePortMakeConnection=(params["ePortMakeConnection"]==false)?params["ePortMakeConnection"]:true;
node.sPortMakeConnection=(params["sPortMakeConnection"]==false)?params["sPortMakeConnection"]:true;
node.wPortMakeConnection=(params["wPortMakeConnection"]==false)?params["wPortMakeConnection"]:true;
node.nPortAcceptConnection=(params["nPortAcceptConnection"]==false)?params["nPortAcceptConnection"]:true;
node.ePortAcceptConnection=(params["ePortAcceptConnection"]==false)?params["ePortAcceptConnection"]:true;
node.sPortAcceptConnection=(params["sPortAcceptConnection"]==false)?params["sPortAcceptConnection"]:true;
node.wPortAcceptConnection=(params["wPortAcceptConnection"]==false)?params["wPortAcceptConnection"]:true;
node.image=(params["image"])?params["image"]:"images/defaultimage.png";
node.imageWidth=(params["imageWidth"])?params["imageWidth"]:"100";
node.imageHeight=(params["imageHeight"])?params["imageHeight"]:"100";
node.draggable=(params["draggable"]==false)?params["draggable"]:true;
node.resizable=(params["resizable"]==false)?params["resizable"]:true;
node.editable=(params["editable"]==false)?params["editable"]:true;
node.selectable=(params["selectable"]==false)?params["selectable"]:true;
node.deletable=(params["deletable"]==false)?params["deletable"]:true;
if(node.width>node.maxWidth)node.width=node.maxWidth;
if(node.width<node.minWidth)node.width=node.minWidth;
if(node.height>node.maxHeight)node.height=node.maxHeight;
if(node.height<node.minHeight)node.height=node.minHeight;
if(node.nodeType=="NODE"){}
else if(node.nodeType=="IMAGE"){node.borderWidth=0;node.editable=false;if(node.imageWidth<node.minWidth)node.imageWidth=node.minWidth;if(node.imageWidth>node.maxWidth)node.imageWidth=node.maxWidth;if(node.imageHeight<node.minHeight)node.imageHeight=node.minHeight;if(node.imageHeight>node.maxHeight)node.imageHeight=node.maxHeight;node.width=parseInt(node.imageWidth)+24;node.height=parseInt(node.imageHeight)+24}
else if(node.nodeType=="TEXT"){node.borderWidth=0;node.nPort=false;node.ePort=false;node.sPort=false;node.wPort=false}
node.isNew=true;
node.isModified=false;
node.isSelected=false;
node.affectedConnections=[]
};

Connection=function(nodeFrom,portFrom,nodeTo,portTo,color,stroke)
{
	this.nodeFrom=nodeFrom;
	this.portFrom = portFrom;
    this.nodeTo=nodeTo;
	this.portTo=portTo;
	this.stroke=(stroke)?stroke:3;
	this.color = (color) ? color : "#969696";
	this.connectionId=''
};

Diagram = function (params) {

    var self = this; if (!params) params = { 'empty': 'empty' };
    this.canvid = (params["id"]) ? params["id"] : "dgrm_" + new Date().getTime() + Math.floor(Math.random() * 11111);
    this.xPosition = (params["xPosition"]) ? params["xPosition"] : 20;
    this.yPosition = (params["yPosition"]) ? params["yPosition"] : 30;
    this.width = (params["width"]) ? params["width"] : 1000;
    this.height = (params["height"]) ? params["height"] : 500;
    this.connectionWidth = (params["connectionWidth"]) ? params["connectionWidth"] : 3;
    this.connectionColor = (params["connectionColor"]) ? params["connectionColor"] : '#888888';
    this.height = (params["height"]) ? params["height"] : 500;
    this.imagesPath = (params["imagesPath"]) ? params["imagesPath"] : 'images/';

    this.noToolbar = (params["noToolbar"] == true) ? params["noToolbar"] : false;

    this.toolbar_add_button = (params["toolbar_add_button"] == false) ? params["toolbar_add_button"] : true;

    this.toolbar_save_button = (params["toolbar_save_button"] == false) ? params["toolbar_save_button"] : true;


    this.toolbar_load_button = (params["toolbar_load_button"] == false) ? params["toolbar_load_button"] : true;

    this.toolbar_delete_button = (params["toolbar_delete_button"] == false) ? params["toolbar_delete_button"] : true;

    this.toolbar_background_color_button = (params["toolbar_background_color_button"] == false) ? params["toolbar_background_color_button"] : true;

    this.toolbar_border_color_button = (params["toolbar_border_color_button"] == false) ? params["toolbar_border_color_button"] : true;

    this.toolbar_font_color_button = (params["toolbar_font_color_button"] == false) ? params["toolbar_font_color_button"] : true;

    this.toolbar_font_size_button = (params["toolbar_font_size_button"] == false) ? params["toolbar_font_size_button"] : true;

    this.toolbar_font_family_button = (params["toolbar_font_family_button"] == false) ? params["toolbar_font_family_button"] : true;

    this.toolbar_border_width_button = (params["toolbar_border_width_button"] == false) ? params["toolbar_border_width_button"] : true;

    this.onSave = (params["onSave"]) ? params["onSave"] : null;

    this.loadTemplates = (params["loadTemplates"]) ? params["loadTemplates"] : null;

    this.configureActions = (params["configureActions"]) ? params["configureActions"] : null;

    this.deleteNodes = (params["deleteNodes"]) ? params["deleteNodes"] : null;

    this.canvasId = this.canvid;

    this.nodes = [];

    this.connections = [];

    this.hoverEffect = true;

    this.tempLine = null;

    var diagramcontainer = '<div style="position:absolute; left:' + self.xPosition + 'px;top:' + (self.yPosition + 40) + 'px;width:' + (self.width + 4) + 'px;height:' + (self.height + 4) + 'px;z-index:-2;padding:0px; margin:0px;" class="ui-widget-header ui-corner-all"></div>';

    diagramcontainer += '<div id="' + self.canvid + '" style="position:absolute; left:' + (self.xPosition + 2) + 'px;top:' + (self.yPosition + 42) + 'px;width:' + (self.width) + 'px;height:' + (self.height) + 'px;overflow:scroll;border:1px solid #AAAAAA; background: url(\'' + self.imagesPath + 'bg.gif\') repeat;z-index:0;"><div id="' + self.canvid + '_templine" style="position: absolute;z-index:1000;"></div></div>'; $('body').append(diagramcontainer); tmplin = new jsGraphics(self.canvid + '_templine'); tmplin.setColor(self.connectionColor); tmplin.setStroke(self.connectionWidth); self.tempLine = tmplin;

    if (this.noToolbar == false) {
        this.mytoolbar = new Toolbar(this, { 'xPosition': self.xPosition, 'yPosition': self.yPosition, 'width': self.width, 'add_button': self.toolbar_add_button, 'save_button': self.toolbar_save_button, 'load_button': self.toolbar_load_button, 'delete_button': this.toolbar_delete_button, 'background_color_button': this.toolbar_background_color_button, 'border_color_button': this.toolbar_border_color_button, 'font_color_button': this.toolbar_font_color_button, 'font_size_button': this.toolbar_font_size_button, 'font_family_button': this.toolbar_font_family_button, 'border_width_button': this.toolbar_border_width_button })
    }

    this.getCanvas = $("#" + self.canvid); $("#" + self.canvid).click(function (event) { if (!((event.target.id).match(/_content$/)) && !((event.target.id).match(/_imagenode$/))) { self.clearSelection() } }); $("#" + self.canvid).droppable({ accept: '.node_palette_item', drop: function (event, ui) {
        if (($(ui.draggable).attr("id")).indexOf(self.canvid) != -1) {
            arr = self.mytoolbar.getNodeByOrder($(ui.draggable).attr("id").replace("_" + self.canvasId, ""));
            self.addNode(new Node({
                'nodeType': arr['nodeType'],
                'nodeContent': arr['nodeContent'],
                'xPosition': (ui.position.left + $("#" + self.canvid)
          .scrollLeft()),
                'yPosition': (ui.position.top + $("#" + self.canvid)
          .scrollTop()),
                'width': arr['width'],
                'height': arr['height'],
                'bgColor': arr['bgColor'],
                'borderColor': arr['borderColor'],
                'borderWidth': arr['borderWidth'],
                'fontColor': arr['fontColor'],
                'fontSize': arr['fontSize'],
                'fontType': arr['fontType'],
                'minHeight': arr['minHeight'],
                'maxHeight': arr['maxHeight'],
                'minWidth': arr['minWidth'],
                'maxWidth': arr['maxWidth'],
                'nPort': arr['nPort'],
                'ePort': arr['ePort'],
                'sPort': arr['sPort'],
                'wPort': arr['wPort'],
                'image': arr['image'],
                'imageWidth': arr['imageWidth'],
                'imageHeight': arr['imageHeight'],
                'draggable': arr['draggable'],
                'resizable': arr['resizable'],
                'editable': arr['editable'],
                'selectable': arr['selectable'],
                'deletable': arr['deletable'],
                'nPortMakeConnection': arr['nPortMakeConnection'],
                'ePortMakeConnection': arr['ePortMakeConnection'],
                'sPortMakeConnection': arr['sPortMakeConnection'],
                'wPortMakeConnection': arr['wPortMakeConnection'],
                'nPortAcceptConnection': arr['nPortAcceptConnection'],
                'ePortAcceptConnection': arr['ePortAcceptConnection'],
                'sPortAcceptConnection': arr['sPortAcceptConnection'],
                'wPortAcceptConnection': arr['wPortAcceptConnection']
            }))
        }

    }
    });
    var portImage = self.imagesPath + 'redport.png';
    var activePortImage = self.imagesPath + 'greenport.png';
    var portStyle = "<style> ." + self.canvid + "port {width: 12px;height: 12px;border: 1px solid #000;background-color: red;background-image:url('" + portImage + "');background-repeat:repeat;visibility: hidden;cursor:pointer;} ." + self.canvid + "activeport {width: 12px;height: 12px;border: 1px solid #000;background-color: green;background-image:url('" + activePortImage + "');background-repeat:repeat;}</style>"; $('body').append(portStyle);
    this.addNode = function (node) {
        //MAKING SURE ONLY ONE START AND STOP EXISTS
        if (node.nodeContent == 'START') {
            var startExists = this.checkExistingNodeType('START');
            if (startExists == true)
                return;
        }
        else if (node.nodeContent == 'STOP') {
            var startExists = this.checkExistingNodeType('STOP');
            if (startExists == true)
                return;
        }
        var callerObj = this;
        var container = '<div id="' + node.nodeId + '" ';
        container += 'class="ui-widget-content node" style="width: ' + node.width + 'px; height: ' + node.height + 'px;left:' + node.xPosition + 'px; top:' + node.yPosition + 'px ;border:0px;">';
        container += '<table width="100%" height="100%" cellpadding=0 cellspacing=0><tr><td colspan=3 width="100%" align=center valign=bottom >';
        if (node.nPort) container += '<div id="' + node.nodeId + '_n" class="' + callerObj.canvasId + 'port"><img src="' + self.imagesPath + 'redport.png"></div>';
        else container += '<div style="width:12px;height:12px;overflow:hidden">&nbsp;</div>';
        container += '</td></tr><tr style="height: 100%;"><td width="1%" valign=middle align=right>';
        if (node.wPort) container += '<div id="' + node.nodeId + '_w" class="' + callerObj.canvasId + 'port"><img src="' + self.imagesPath + 'redport.png"></div>';
        else container += '<div style="width:12px;height:12px;overflow:hidden">&nbsp;</div>';
        container += '</td><td id="' + node.nodeId + '_content"  valign=middle align=center style="border: ' + node.borderWidth + 'px solid ' + node.borderColor + ';background-color: ' + node.bgColor + ';FONT-FAMILY: ' + node.fontType + '; COLOR: ' + node.fontColor + '; FONT-SIZE: ' + node.fontSize + '; TEXT-DECORATION: none; ';
        if (node.nodeType == 'TEXT' || node.nodeType == 'IMAGE') container += 'background: url(\'' + self.imagesPath + 'transparent.gif\') repeat; ';
        container += '">';
        if (node.nodeType == 'IMAGE') {
            container += '<img id="' + node.nodeId + '_imagenode" src="' + node.image + '" width=' + node.imageWidth + 'px height=' + node.imageHeight + 'px border=0><br>';
            //ADDING CUSTOM TEXT UNDER IMAGE USING NODE CONTENT
            if (node.nodeId.indexOf('EMAIL') != -1) { container += '<div class="EMAIL' + node.nodeId + '">' + node.nodeContent + '</div>'; }
        }
        else container += node.nodeContent;
        container += '</td><td width="1%" valign=middle align=left>';
        if (node.ePort) container += '<div id="' + node.nodeId + '_e" class="' + callerObj.canvasId + 'port"><img src="' + self.imagesPath + 'redport.png"></div>';
        else container += '<div style="width:12px;height:12px;overflow:hidden">&nbsp;</div>';
        container += '</td></tr><tr><td colspan=3 width="100%" align=center valign=top>';
        if (node.sPort) container += '<div id="' + node.nodeId + '_s" class="' + callerObj.canvasId + 'port"><img src="' + self.imagesPath + 'redport.png"></div>';
        else container += '<div style="width:12px;height:12px;overflow:hidden">&nbsp;</div>';
        container += '</td></tr></table></div>';
        var ports = "";
        if (node.nPort) ports += "#" + node.nodeId + "_n, ";
        if (node.ePort) ports += "#" + node.nodeId + "_e, ";
        if (node.sPort) ports += "#" + node.nodeId + "_s, ";
        if (node.wPort) ports += "#" + node.nodeId + "_w, ";
        $(callerObj.getCanvas)
            .append(container);
        $("#" + node.nodeId).hover(function () {
            if (callerObj.hoverEffect) {
                $(ports).css("visibility", "visible")
            }
        },
           function () {
               if (callerObj.hoverEffect) {
                   $(ports).css("visibility", "hidden")
               }
           });
        if (node.draggable) {
            $("#" + node.nodeId)
            .draggable({
                start: function (event, ui) {
                    callerObj.setNodeAffectedConnections(node.nodeId)
                },
                drag: function () {
                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                },
                stop: function () {
                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                    .position()
                    .left + $(callerObj.getCanvas)
                    .scrollLeft(), $("#" + node.nodeId)
                    .position()
                    .top + $(callerObj.getCanvas)
                    .scrollTop(), $("#" + node.nodeId)
                    .width(), $("#" + node.nodeId)
                    .height())
                }
            })
        }
        if (node.resizable) {
            if (node.nodeType == 'IMAGE') $("#" + node.nodeId)
            .resizable({
                handles: "all",
                autoHide: true,
                alsoResize: "#" + node.nodeId + "_imagenode",
                maxHeight: node.maxHeight,
                maxWidth: node.maxWidth,
                minHeight: node.minHeight,
                minWidth: node.minWidth,
                start: function () {
                    callerObj.setNodeAffectedConnections(node.nodeId)
                },
                resize: function () {
                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                },
                stop: function () {
                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                    .position()
                    .left + $(callerObj.getCanvas)
                    .scrollLeft(), $("#" + node.nodeId)
                    .position()
                    .top + $(callerObj.getCanvas)
                    .scrollTop(), $("#" + node.nodeId)
                    .width(), $("#" + node.nodeId)
                    .height())
                }
            });
            else $("#" + node.nodeId)
            .resizable({
                handles: "all",
                autoHide: true,
                maxHeight: node.maxHeight,
                maxWidth: node.maxWidth,
                minHeight: node.minHeight,
                minWidth: node.minWidth,
                start: function () {
                    callerObj.setNodeAffectedConnections(node.nodeId)
                },
                resize: function () {
                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                },
                stop: function () {
                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                    .position()
                    .left + $(callerObj.getCanvas)
                    .scrollLeft(), $("#" + node.nodeId)
                    .position()
                    .top + $(callerObj.getCanvas)
                    .scrollTop(), $("#" + node.nodeId)
                    .width(), $("#" + node.nodeId)
                    .height())
                }
            })
        }
        if (node.selectable) {
            $("#" + node.nodeId)
            .click(function (event, ui) {
                callerObj.selectNode(node.nodeId)
            })
        }
        if (node.nPort) {
            if (node.nPortMakeConnection) {
                $("#" + node.nodeId + "_n")
                .draggable({
                    revert: true,
                    revertDuration: 1,
                    start: function () {
                        $("#" + node.nodeId + "")
                        .resizable('destroy');
                        $("#" + node.nodeId + "")
                        .draggable('destroy');
                        $("#" + node.nodeId + "")
                        .unbind('mouseenter mouseleave');
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "visible");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "0.5");
                        callerObj.hoverEffect = false
                    },
                    drag: function () {
                        dtc("" + node.nodeId, (this.id + "")
                        .charAt((this.id + "")
                        .length - 1), $(this), callerObj, callerObj.tempLine)
                    },
                    stop: function () {
                        $(ports)
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "1");
                        callerObj.hoverEffect = true;
                        $("#" + node.nodeId)
                        .hover(function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "visible")
                            }
                        }, function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "hidden")
                            }
                        });
                        if (node.draggable) {
                            $("#" + node.nodeId)
                            .draggable({
                                start: function (event, ui) {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                drag: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.resizable) {
                            if (node.nodeType == 'IMAGE') $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                alsoResize: "#" + node.nodeId + "_imagenode",
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            });
                            else $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.selectable) {
                            $("#" + node.nodeId)
                            .click(function (event, ui) {
                                callerObj.selectNode(node.nodeId)
                            })
                        }
                        callerObj.clearTempLine()
                    }
                })
            }
            if (node.nPortAcceptConnection) {
                $("#" + node.nodeId + "_n")
                .droppable({
                    accept: "." + callerObj.canvasId + "port",
                    hoverClass: callerObj.canvasId + "activeport",
                    drop: function (event, ui) {
                        var sourceNode = $(ui.draggable)
                        .attr("id")
                        .substring(0, $(ui.draggable)
                        .attr("id")
                        .indexOf('_'));
                        var targetNode = $(this)
                        .attr("id")
                        .substring(0, $(this)
                        .attr("id")
                        .indexOf('_'));
                        var sourceNodePort = $(ui.draggable)
                        .attr("id");
                        var targetNodePort = $(this)
                        .attr("id");
                        callerObj.clearTempLine();
                        var connection = new Connection(sourceNode, sourceNodePort.charAt(sourceNodePort.length - 1), targetNode, targetNodePort.charAt(targetNodePort.length - 1), callerObj.connectionColor, callerObj.connectionWidth);
                        callerObj.addConnection(connection)
                    }
                })
            }
        }
        if (node.ePort) {
            if (node.ePortMakeConnection) {
                $("#" + node.nodeId + "_e")
                .draggable({
                    revert: true,
                    revertDuration: 1,
                    start: function () {
                        $("#" + node.nodeId + "")
                        .resizable('destroy');
                        $("#" + node.nodeId + "")
                        .draggable('destroy');
                        $("#" + node.nodeId + "")
                        .unbind('mouseenter mouseleave');
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "visible");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "0.5");
                        callerObj.hoverEffect = false
                    },
                    drag: function () {
                        dtc("" + node.nodeId, (this.id + "")
                        .charAt((this.id + "")
                        .length - 1), $(this), callerObj, callerObj.tempLine)
                    },
                    stop: function () {
                        $(ports)
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "1");
                        callerObj.hoverEffect = true;
                        $("#" + node.nodeId)
                        .hover(function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "visible")
                            }
                        }, function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "hidden")
                            }
                        });
                        if (node.draggable) {
                            $("#" + node.nodeId)
                            .draggable({
                                start: function (event, ui) {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                drag: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.resizable) {
                            if (node.nodeType == 'IMAGE') $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                alsoResize: "#" + node.nodeId + "_imagenode",
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            });
                            else $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.selectable) {
                            $("#" + node.nodeId)
                            .click(function (event, ui) {
                                callerObj.selectNode(node.nodeId)
                            })
                        }
                        callerObj.clearTempLine()
                    }
                })
            }
            if (node.ePortAcceptConnection) {
                $("#" + node.nodeId + "_e")
                .droppable({
                    accept: "." + callerObj.canvasId + "port",
                    hoverClass: callerObj.canvasId + "activeport",
                    drop: function (event, ui) {
                        var sourceNode = $(ui.draggable)
                        .attr("id")
                        .substring(0, $(ui.draggable)
                        .attr("id")
                        .indexOf('_'));
                        var targetNode = $(this)
                        .attr("id")
                        .substring(0, $(this)
                        .attr("id")
                        .indexOf('_'));
                        var sourceNodePort = $(ui.draggable)
                        .attr("id");
                        var targetNodePort = $(this)
                        .attr("id");
                        callerObj.clearTempLine();
                        var connection = new Connection(sourceNode, sourceNodePort.charAt(sourceNodePort.length - 1), targetNode, targetNodePort.charAt(targetNodePort.length - 1), callerObj.connectionColor, callerObj.connectionWidth);
                        callerObj.addConnection(connection)
                    }
                })
            }
        }
        if (node.sPort) {
            if (node.sPortMakeConnection) {
                $("#" + node.nodeId + "_s")
                .draggable({
                    revert: true,
                    revertDuration: 1,
                    start: function () {
                        $("#" + node.nodeId + "")
                        .resizable('destroy');
                        $("#" + node.nodeId + "")
                        .draggable('destroy');
                        $("#" + node.nodeId + "")
                        .unbind('mouseenter mouseleave');
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "visible");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "0.5");
                        callerObj.hoverEffect = false
                    },
                    drag: function () {
                        dtc("" + node.nodeId, (this.id + "")
                        .charAt((this.id + "")
                        .length - 1), $(this), callerObj, callerObj.tempLine)
                    },
                    stop: function () {
                        $(ports)
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "1");
                        callerObj.hoverEffect = true;
                        $("#" + node.nodeId)
                        .hover(function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "visible")
                            }
                        }, function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "hidden")
                            }
                        });
                        if (node.draggable) {
                            $("#" + node.nodeId)
                            .draggable({
                                start: function (event, ui) {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                drag: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.resizable) {
                            if (node.nodeType == 'IMAGE') $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                alsoResize: "#" + node.nodeId + "_imagenode",
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            });
                            else $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.selectable) {
                            $("#" + node.nodeId)
                            .click(function (event, ui) {
                                callerObj.selectNode(node.nodeId)
                            })
                        }
                        callerObj.clearTempLine()
                    }
                })
            }
            if (node.sPortAcceptConnection) {
                $("#" + node.nodeId + "_s")
                .droppable({
                    accept: "." + callerObj.canvasId + "port",
                    hoverClass: callerObj.canvasId + "activeport",
                    drop: function (event, ui) {
                        var sourceNode = $(ui.draggable)
                        .attr("id")
                        .substring(0, $(ui.draggable)
                        .attr("id")
                        .indexOf('_'));
                        var targetNode = $(this)
                        .attr("id")
                        .substring(0, $(this)
                        .attr("id")
                        .indexOf('_'));
                        var sourceNodePort = $(ui.draggable)
                        .attr("id");
                        var targetNodePort = $(this)
                        .attr("id");
                        callerObj.clearTempLine();
                        var connection = new Connection(sourceNode, sourceNodePort.charAt(sourceNodePort.length - 1), targetNode, targetNodePort.charAt(targetNodePort.length - 1), callerObj.connectionColor, callerObj.connectionWidth);
                        callerObj.addConnection(connection)
                    }
                })
            }
        }
        if (node.wPort) {
            if (node.wPortMakeConnection) {
                $("#" + node.nodeId + "_w")
                .draggable({
                    revert: true,
                    revertDuration: 1,
                    start: function () {
                        $("#" + node.nodeId + "")
                        .resizable('destroy');
                        $("#" + node.nodeId + "")
                        .draggable('destroy');
                        $("#" + node.nodeId + "")
                        .unbind('mouseenter mouseleave');
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "visible");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "0.5");
                        callerObj.hoverEffect = false
                    },
                    drag: function () {
                        dtc("" + node.nodeId, (this.id + "")
                        .charAt((this.id + "")
                        .length - 1), $(this), callerObj, callerObj.tempLine)
                    },
                    stop: function () {
                        $(ports)
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .css("visibility", "hidden");
                        $("." + callerObj.canvasId + "port")
                        .fadeTo('fast', "1");
                        callerObj.hoverEffect = true;
                        $("#" + node.nodeId)
                        .hover(function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "visible")
                            }
                        }, function () {
                            if (callerObj.hoverEffect) {
                                $(ports)
                                .css("visibility", "hidden")
                            }
                        });
                        if (node.draggable) {
                            $("#" + node.nodeId)
                            .draggable({
                                start: function (event, ui) {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                drag: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.resizable) {
                            if (node.nodeType == 'IMAGE') $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                alsoResize: "#" + node.nodeId + "_imagenode",
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            });
                            else $("#" + node.nodeId)
                            .resizable({
                                handles: "all",
                                autoHide: true,
                                maxHeight: node.maxHeight,
                                maxWidth: node.maxWidth,
                                minHeight: node.minHeight,
                                minWidth: node.minWidth,
                                start: function () {
                                    callerObj.setNodeAffectedConnections(node.nodeId)
                                },
                                resize: function () {
                                    callerObj.redrawNodeAffectedConnections(node.nodeId)
                                },
                                stop: function () {
                                    callerObj.updateNodeCoordinates(node.nodeId, $("#" + node.nodeId)
                                    .position()
                                    .left + $(callerObj.getCanvas)
                                    .scrollLeft(), $("#" + node.nodeId)
                                    .position()
                                    .top + $(callerObj.getCanvas)
                                    .scrollTop(), $("#" + node.nodeId)
                                    .width(), $("#" + node.nodeId)
                                    .height())
                                }
                            })
                        }
                        if (node.selectable) {
                            $("#" + node.nodeId)
                            .click(function (event, ui) {
                                callerObj.selectNode(node.nodeId)
                            })
                        }
                        callerObj.clearTempLine()
                    }
                })
            }
            if (node.wPortAcceptConnection) {
                $("#" + node.nodeId + "_w")
                .droppable({
                    accept: "." + callerObj.canvasId + "port",
                    hoverClass: callerObj.canvasId + "activeport",
                    drop: function (event, ui) {
                        var sourceNode = $(ui.draggable)
                        .attr("id")
                        .substring(0, $(ui.draggable)
                        .attr("id")
                        .indexOf('_'));
                        var targetNode = $(this)
                        .attr("id")
                        .substring(0, $(this)
                        .attr("id")
                        .indexOf('_'));
                        var sourceNodePort = $(ui.draggable)
                        .attr("id");
                        var targetNodePort = $(this)
                        .attr("id");
                        callerObj.clearTempLine();
                        var connection = new Connection(sourceNode, sourceNodePort.charAt(sourceNodePort.length - 1), targetNode, targetNodePort.charAt(targetNodePort.length - 1), callerObj.connectionColor, callerObj.connectionWidth);
                        callerObj.addConnection(connection)
                    }
                })
            }
        }
        if (node.editable) $("#" + node.nodeId + "_content")
        ._t(node.nodeId, callerObj);
        this.nodes[this.nodes.length] = node
    };

    this.addConnection = function (connection) {
        caller = this;

        //OPENS CAN ACCEPT CONNECTIONS ONLY FROM ONE EMAIL NODE  
        if (connection.nodeTo.indexOf("OPEN") != -1) {
            if (connection.nodeFrom.indexOf("START") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("OPEN") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("CLICK") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("EMAIL") != -1) {
                var inputNodes = this.getInputNodes(connection.nodeTo);
                if (inputNodes.length > 0) {
                    alert("Only one Blast input allowed");
                    return;
                }
            }
        }
        //CLICK CAN ACCEPT CONNECTIONS ONLY FROM ONE EMAIL NODE  
        if (connection.nodeTo.indexOf("CLICK") != -1) {
            if (connection.nodeFrom.indexOf("START") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("OPEN") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("CLICK") != -1) {
                return;
            }
            if (connection.nodeFrom.indexOf("EMAIL") != -1) {
                var inputNodes = this.getInputNodes(connection.nodeTo);
                if (inputNodes.length > 0) {
                    alert("Only one Blast input allowed");
                    return;
                }
            }
        }

        //STOP WILL NOT ACCEPT CONNECTIONS FROM START(ATLEAST 1 BLAST)
        if (connection.nodeTo.indexOf("STOP") != -1) {
            if (connection.nodeTo.indexOf("START") != -1) {
                return;
            }
        }

        //EMAIL CAN HAVE ONLY ONE OUTPUT & INPUT
        //        if (fromNodeContent == 'EMAIL') {
        //            var outputNodes = this.getOutputNodes(connection.nodeFrom);
        //            if (outputNodes.length > 0) {
        //                alert("There can be only one output from this Blast");
        //                return;
        //            }
        //        }
        if (connection.nodeTo.indexOf("EMAIL") != -1) {
            var inputNodes = this.getInputNodes(connection.nodeTo);
            if (inputNodes.length > 0) {
                alert("Only one input can be provided to this Blast");
                return;
            }
        }

        var connectionId = "" + new Date().getTime() + Math.floor(Math.random() * 11111);
        var connectionDiv = '<div id="' + connectionId + '" ></div>';
        $(caller.getCanvas).append(connectionDiv);
        var connectionLine = new jsGraphics(document.getElementById(connectionId));
        $("#" + connectionId).click(function () { caller.deleteConnection(connectionId) });
        connectionLine.setColor(connection.color);
        connectionLine.setStroke(connection.stroke);
        connection.line = connectionLine;
        connection.connectionId = connectionId;
        this.connections[this.connections.length] = connection;
        dc(connection.nodeFrom, connection.nodeTo, connection.portFrom, connection.portTo, connection.line, caller)
    };

    this.deleteConnection = function (connectionId) {
        //        if (confirm('Delete Connection?')) {
        //            var conn = '';
        //            for (i = 0; i < this.connections.length; i++) {
        //                if (this.connections[i].connectionId == connectionId) {
        //                    this.connections[i].line.clear();
        //                    this.connections.splice(i, 1);
        //                    break
        //                }
        //            }
        //        }
    };

    this.setNodeAffectedConnections = function (nodeId) { affected = []; for (i = 0; i < this.connections.length; i++) if (this.connections[i].nodeFrom == nodeId || this.connections[i].nodeTo == nodeId) affected.push(this.connections[i]); for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].nodeId == nodeId) this.nodes[i].affectedConnections = affected };

    this.redrawNodeAffectedConnections = function (nodeId) { var caller = this; var node = ''; for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].nodeId == nodeId) node = this.nodes[i]; var affectedConnections = node.affectedConnections; for (i = 0; i < affectedConnections.length; i++) { dc(affectedConnections[i].nodeFrom, affectedConnections[i].nodeTo, affectedConnections[i].portFrom, affectedConnections[i].portTo, affectedConnections[i].line, caller) } };

    this.selectNode = function (nodeId) { for (i = 0; i < this.nodes.length; i++) { if (this.nodes[i].nodeId == nodeId) { this.nodes[i].isSelected = true; $("#" + this.nodes[i].nodeId + "_content").css({ "border": "4px dotted orange" }) } } };

    this.clearSelection = function () { for (i = 0; i < this.nodes.length; i++) { if (this.nodes[i].isSelected) { this.nodes[i].isSelected = false; $("#" + this.nodes[i].nodeId + "_content").css({ "border": this.nodes[i].borderWidth + "px solid " + this.nodes[i].borderColor }) } } };

    this.getSelectedNodes = function () { var selections = []; for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].isSelected) selections.push(this.nodes[i]); return selections };

    this.updateNodeCoordinates = function (nodeId, xPosition, yPosition, width, height) {
        var node = this.getNodeById(nodeId);
        node.xPosition = xPosition;
        node.yPosition = yPosition;
        node.width = width;
        node.height = height
    };
    this.updateNodeBGColor = function (nodeId, bgcolor) {
        var node = this.getNodeById(nodeId);
        node.bgColor = bgcolor;
        $("#" + nodeId + "_content")
          .css('background-color', bgcolor)
    };
    this.updateNodeBorderColor = function (nodeId, bordercolor) {
        var node = this.getNodeById(nodeId);
        node.borderColor = bordercolor;
        $("#" + nodeId + "_content")
          .css('border-color', bordercolor)
    };
    this.updateNodeBorderWidth = function (nodeId, borderWidth) {
        var node = this.getNodeById(nodeId);
        node.borderWidth = borderWidth;
        $("#" + nodeId + "_content")
          .css('border-width', borderWidth)
    };
    this.updateNodeFontColor = function (nodeId, fontColor) {
        var node = this.getNodeById(nodeId);
        node.fontColor = fontColor;
        $("#" + nodeId + "_content")
          .css('color', fontColor)
    };
    this.updateNodeFontSize = function (nodeId, fontSize) {
        var node = this.getNodeById(nodeId);
        node.fontSize = fontSize;
        $("#" + nodeId + "_content")
          .css('font-size', fontSize)
    };
    this.updateNodeFontType = function (nodeId, fontType) {
        var node = this.getNodeById(nodeId);
        node.fontType = fontType;
        $("#" + nodeId + "_content")
          .css('font-family', fontType)
    };
    this.updateNodeContent = function (nodeId, nodeContent) {
        var node = this.getNodeById(nodeId);
        node.nodeContent = nodeContent;
    };
    this.updateSelectedNodesBGColor = function (bgcolor) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeBGColor(selectedNodes[index].nodeId, bgcolor)
        }
    };
    this.updateSelectedNodesBorderColor = function (bordercolor) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeBorderColor(selectedNodes[index].nodeId, bordercolor)
        }
    };
    this.updateSelectedNodesBorderWidth = function (borderWidth) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeBorderWidth(selectedNodes[index].nodeId, borderWidth)
        }
    };
    this.updateSelectedNodesFontColor = function (fontcolor) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeFontColor(selectedNodes[index].nodeId, fontcolor)
        }
    };
    this.updateSelectedNodesFontSize = function (fontSize) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeFontSize(selectedNodes[index].nodeId, fontSize)
        }
    };
    this.updateSelectedNodesFontType = function (fontType) {
        var selectedNodes = this.getSelectedNodes();
        for (index = 0; index < selectedNodes.length; index++) {
            this.updateNodeFontType(selectedNodes[index].nodeId, fontType)
        }
    };

    this.deleteSelectedNodes = function () {
        var selectedNodes = this.getSelectedNodes();
        if (selectedNodes.length > 0) {
            if (confirm("Selected nodes will be deleted, continue ...")) {
                for (index = 0; index < selectedNodes.length; index++) {
                    // this.deleteNode(selectedNodes[index].nodeId);
                    this.deleteNodes(selectedNodes[index].nodeId);
                }
            }
        }
    };

    this.configureAction = function () {
        var selectedNodes = this.getSelectedNodes();
        if (selectedNodes.length == 1) {
            var ruleNodeID = '';
            var ruleType = '';
            var inputNodes = this.getInputNodes(selectedNodes[0].nodeId);
            var ouputNodes = this.getOutputNodes(selectedNodes[0].nodeId);
            for (i = 0; i < inputNodes.length; i++) {
                if (inputNodes[i].toString().indexOf("NOOPEN") != -1) {
                    ruleNodeID = inputNodes[i].toString();
                    ruleType = 'NOOPEN';
                }
                else if (inputNodes[i].toString().indexOf("NOCLICK") != -1) {
                    ruleNodeID = inputNodes[i].toString();
                    ruleType = 'NOCLICK';
                }
                else if (inputNodes[i].toString().indexOf("OPEN") != -1) {
                    ruleNodeID = inputNodes[i].toString();
                    ruleType = 'OPEN';
                }
                else if (inputNodes[i].toString().indexOf("CLICK") != -1) {
                    ruleNodeID = inputNodes[i].toString();
                    ruleType = 'CLICK';
                }
            }
            if (ruleNodeID != '') {
                inputNodes = this.getInputNodes(ruleNodeID);
                inputNodes[0] = inputNodes[0].toString() + '-' + ruleType;
            }

            var allNodes = [];
            var xml = '<diagram id="' + $(this.getCanvas).attr("id") + '">\n';
            // var xml = '';
            //Node Properties
            for (i = 0; i < this.nodes.length; i++) {
                xml += '<node nodeId="' + this.nodes[i].nodeId + '" nodeType="' + this.nodes[i].nodeType + '" nodeContent="' + this.nodes[i].nodeContent + '" xPosition="' + this.nodes[i].xPosition + '" yPosition="' + this.nodes[i].yPosition + '" width="' + this.nodes[i].width + '" height="' + this.nodes[i].height + '" bgColor="' + this.nodes[i].bgColor + '" borderColor="' + this.nodes[i].borderColor + '" borderWidth="' + this.nodes[i].borderWidth + '" fontColor="' + this.nodes[i].fontColor + '" fontSize="' + this.nodes[i].fontSize + '" fontType="' + this.nodes[i].fontType + '" minHeight="' + this.nodes[i].minHeight + '" maxHeight="' + this.nodes[i].maxHeight + '" minWidth="' + this.nodes[i].minWidth + '" maxWidth="' + this.nodes[i].maxWidth + '" nPort="' + this.nodes[i].nPort + '" ePort="' + this.nodes[i].ePort + '" sPort="' + this.nodes[i].sPort + '" wPort="' + this.nodes[i].wPort + '" nPortMakeConnection="' + this.nodes[i].nPortMakeConnection + '" ePortMakeConnection="' + this.nodes[i].ePortMakeConnection + '" sPortMakeConnection="' + this.nodes[i].sPortMakeConnection + '" wPortMakeConnection="' + this.nodes[i].wPortMakeConnection + '" nPortAcceptConnection="' + this.nodes[i].nPortAcceptConnection + '" ePortAcceptConnection="' + this.nodes[i].ePortAcceptConnection + '" sPortAcceptConnection="' + this.nodes[i].sPortAcceptConnection + '" wPortAcceptConnection="' + this.nodes[i].wPortAcceptConnection + '" image="' + this.nodes[i].image + '" imageWidth="' + this.nodes[i].imageWidth + '" imageHeight="' + this.nodes[i].imageHeight + '" draggable="' + this.nodes[i].draggable + '" resizable="' + this.nodes[i].resizable + '" editable="' + this.nodes[i].editable + '" selectable="' + this.nodes[i].selectable + '" deletable="' + this.nodes[i].deletable + '" />\n';
                allNodes.push(this.nodes[i].nodeId);
            }
            //Connection Properties
            for (i = 0; i < this.connections.length; i++) xml += '<connection connectionId="' + this.connections[i].connectionId + '" nodeFrom="' + this.connections[i].nodeFrom + '" nodeTo="' + this.connections[i].nodeTo + '" portFrom="' + this.connections[i].portFrom + '" portTo="' + this.connections[i].portTo + '" color="' + this.connections[i].color + '"  stroke="' + this.connections[i].stroke + '"/>\n';
            xml += '</diagram>';

            this.configureActions(selectedNodes[0].nodeContent, selectedNodes[0].nodeId, inputNodes, ouputNodes, xml, allNodes);
        }
        else if (selectedNodes.length > 1) {
            alert("Please select only one action");
        }
        else {
            alert("No Actions Selected");
        }
    };

    this.getInputNodes = function (nodeId) {
        var fromNodes = [];
        for (i = 0; i < this.connections.length; i++) {
            if (this.connections[i].nodeTo == nodeId) {
                fromNodes.push(this.connections[i].nodeFrom);
            }
        }
        return fromNodes;
    };


    this.checkExistingNodeType = function (contentType) {
        for (i = 0; i < this.nodes.length; i++) {
            if (this.nodes[i].nodeContent == contentType) { return true; }
        }
        return false;
    };

    this.getOutputNodes = function (nodeId) {
        var toNodes = [];
        for (i = 0; i < this.connections.length; i++) {
            if (this.connections[i].nodeFrom == nodeId) {
                toNodes.push(this.connections[i].nodeTo);
            }
        }
        return toNodes;
    };

    this.deleteNode = function (nodeId) {
        this.setNodeAffectedConnections(nodeId);
        var node = this.getNodeById(nodeId);
        var nodeType = node.nodeType;
        for (i = 0; i < node.affectedConnections.length; i++) { for (j = 0; j < this.connections.length; j++) { if (this.connections[j].nodeFrom == node.affectedConnections[i].nodeFrom && this.connections[j].nodeTo == node.affectedConnections[i].nodeTo && this.connections[j].portFrom == node.affectedConnections[i].portFrom && this.connections[j].portTo == node.affectedConnections[i].portTo) { this.connections[j].line.clear(); this.connections.splice(j, 1); break } } }

        for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].nodeId == nodeId) this.nodes.splice(i, 1); $("#" + nodeId).remove()
    };

    this.getNodeById = function (nodeId) { for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].nodeId == nodeId) return this.nodes[i] };

    this.getNodeTypeById = function (nodeId) { for (i = 0; i < this.nodes.length; i++) if (this.nodes[i].nodeId == nodeId) return this.nodes[i].nodeContent };


    this.toXML = function () {
        var allNodes = [];
        var xml = '<diagram id="' + $(this.getCanvas).attr("id") + '">\n';
        // var xml = '';
        //Node Properties
        for (i = 0; i < this.nodes.length; i++) {
            xml += '<node nodeId="' + this.nodes[i].nodeId + '" nodeType="' + this.nodes[i].nodeType + '" nodeContent="' + this.nodes[i].nodeContent + '" xPosition="' + this.nodes[i].xPosition + '" yPosition="' + this.nodes[i].yPosition + '" width="' + this.nodes[i].width + '" height="' + this.nodes[i].height + '" bgColor="' + this.nodes[i].bgColor + '" borderColor="' + this.nodes[i].borderColor + '" borderWidth="' + this.nodes[i].borderWidth + '" fontColor="' + this.nodes[i].fontColor + '" fontSize="' + this.nodes[i].fontSize + '" fontType="' + this.nodes[i].fontType + '" minHeight="' + this.nodes[i].minHeight + '" maxHeight="' + this.nodes[i].maxHeight + '" minWidth="' + this.nodes[i].minWidth + '" maxWidth="' + this.nodes[i].maxWidth + '" nPort="' + this.nodes[i].nPort + '" ePort="' + this.nodes[i].ePort + '" sPort="' + this.nodes[i].sPort + '" wPort="' + this.nodes[i].wPort + '" nPortMakeConnection="' + this.nodes[i].nPortMakeConnection + '" ePortMakeConnection="' + this.nodes[i].ePortMakeConnection + '" sPortMakeConnection="' + this.nodes[i].sPortMakeConnection + '" wPortMakeConnection="' + this.nodes[i].wPortMakeConnection + '" nPortAcceptConnection="' + this.nodes[i].nPortAcceptConnection + '" ePortAcceptConnection="' + this.nodes[i].ePortAcceptConnection + '" sPortAcceptConnection="' + this.nodes[i].sPortAcceptConnection + '" wPortAcceptConnection="' + this.nodes[i].wPortAcceptConnection + '" image="' + this.nodes[i].image + '" imageWidth="' + this.nodes[i].imageWidth + '" imageHeight="' + this.nodes[i].imageHeight + '" draggable="' + this.nodes[i].draggable + '" resizable="' + this.nodes[i].resizable + '" editable="' + this.nodes[i].editable + '" selectable="' + this.nodes[i].selectable + '" deletable="' + this.nodes[i].deletable + '" />\n';
            allNodes.push(this.nodes[i].nodeId);
        }
        //Connection Properties
        for (i = 0; i < this.connections.length; i++) xml += '<connection connectionId="' + this.connections[i].connectionId + '" nodeFrom="' + this.connections[i].nodeFrom + '" nodeTo="' + this.connections[i].nodeTo + '" portFrom="' + this.connections[i].portFrom + '" portTo="' + this.connections[i].portTo + '" color="' + this.connections[i].color + '"  stroke="' + this.connections[i].stroke + '"/>\n';
        xml += '</diagram>';
        //xml += '';
        if (this.onSave != null) {
            this.onSave(xml, allNodes);
        }
        else
        { }
    };



    this.loadTemplate = function () {
        this.loadTemplates();
    };




    this.setTempLine = function (tempLine) { this.tempLine = tempLine }; this.clearTempLine = function () { this.tempLine.clear() }
};
function dc(sourceNode, targetNode, sourcePort, targetPort, line, canvasobj) {
    sourcePort = sourceNode + "_" + sourcePort;
    targetPort = targetNode + "_" + targetPort;
    var startx = $("#" + sourcePort).position().left + $("#" + sourceNode).position().left + $(canvasobj.getCanvas).scrollLeft();
    var starty = $("#" + sourcePort).position().top + $("#" + sourceNode).position().top + $(canvasobj.getCanvas).scrollTop();
    var endx = $("#" + targetPort).position().left + $("#" + targetNode).position().left + $(canvasobj.getCanvas).scrollLeft();
    var endy = $("#" + targetPort).position().top + $("#" + targetNode).position().top + $(canvasobj.getCanvas).scrollTop();
    var sourceNodeX = $("#" + sourceNode).position().left + $(canvasobj.getCanvas).scrollLeft();
    var sourceNodeY = $("#" + sourceNode).position().top + $(canvasobj.getCanvas).scrollTop();
    var sourcenw = $("#" + sourceNode).width(); 
    var sourcenh = $("#" + sourceNode).height();
    var halfSourcenw = Math.floor(sourcenw / 2); 
    var halfSourcenh = Math.floor(sourcenh / 2);
    var targetNodeX = $("#" + targetNode).position().left + 10 + $(canvasobj.getCanvas).scrollLeft();
    var targetNodeY = $("#" + targetNode).position().top + 10 + $(canvasobj.getCanvas).scrollTop();
    var targetnw = $("#" + targetNode).width(); 
    var targetnh = $("#" + targetNode).height();
    var halfTargetnw = Math.floor(targetnw / 2);
     var halfTargetnh = Math.floor(targetnh / 2);
     var zone;
     var sourcePortNumber; var targetPortNumber; var halfVerticalDistance; var halfHorizontalDistance;
     var maxRightDistance = ((sourceNodeX + sourcenw) > (targetNodeX + targetnw)) ? sourceNodeX + sourcenw : targetNodeX + targetnw;
     var minLeftDistance = (sourceNodeX < targetNodeX) ? sourceNodeX : targetNodeX;
     var minTopDistance = (sourceNodeY < targetNodeY) ? sourceNodeY : targetNodeY;
     var maxBottomDistance = ((sourceNodeY + sourcenh) > (targetNodeY + targetnh)) ? sourceNodeY + sourcenh : targetNodeY + targetnh;
          if (sourcePort.match(/_n$/)) {
            sourcePortNumber = 1
        } else if (sourcePort.match(/_e$/)) {
            sourcePortNumber = 2
        } else if (sourcePort.match(/_s$/)) {
            sourcePortNumber = 3
        } else if (sourcePort.match(/_w$/)) {
            sourcePortNumber = 4
        }
        if (targetPort.match(/_n$/)) {
            targetPortNumber = 1
        } else if (targetPort.match(/_e$/)) {
            targetPortNumber = 2
        } else if (targetPort.match(/_s$/)) {
            targetPortNumber = 3
        } else if (targetPort.match(/_w$/)) {
            targetPortNumber = 4
        }
        var connectionType = parseInt(sourcePortNumber + "" + targetPortNumber);
        if (endx <= (sourceNodeX + halfSourcenw)) {
            if (endy <= (sourceNodeY + halfSourcenh)) {
                if (endy <= sourceNodeY) {
                    if (endy >= sourceNodeY - 10 && endx > sourceNodeX) {
                        zone = "Z1A10"
                    } else {
                        if (endx <= sourceNodeX - 10) {
                            zone = "Z8";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                                line.drawLine(endx, minTopDistance - 15, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), targetNodeX - 10, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(targetNodeX - 10, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10, endx, endy - 10);
                                line.drawLine(endx, endy - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z8";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10);
                                line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10, endx, targetNodeY - 10);
                                line.drawLine(endx, targetNodeY - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, (targetNodeX + targetnw) + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                                line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z8";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        } else {
                            zone = "Z1A";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, sourceNodeY - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY - 10, minLeftDistance - 10, sourceNodeY - 10);
                                line.drawLine(minLeftDistance - 10, sourceNodeY - 10, minLeftDistance - 10, endy - 10);
                                line.drawLine(minLeftDistance - 10, endy - 10, endx, endy - 10);
                                line.drawLine(endx, endy - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), targetNodeX - 10, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(targetNodeX - 10, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, minLeftDistance - 10, sourceNodeY + sourcenh + 10);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + sourcenh + 10, minLeftDistance - 10, minTopDistance - 10);
                                line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX - 10, sourceNodeY + sourcenh + 10);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + 10, sourceNodeX - 10, targetNodeY + targetnh + 10);
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z1A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, minTopDistance - 10);
                                line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z1A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        }
                    }
                } else {
                    if (endx <= sourceNodeX - 10) {
                        zone = "Z7B";
                        switch (connectionType) {
                        case 11:
                            zone = "NN ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                            line.drawLine(endx, minTopDistance - 15, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 12:
                            zone = "NE ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                            line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                            line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 13:
                            zone = "NS ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                            line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 14:
                            zone = "NW ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, minTopDistance - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, minTopDistance - 10, targetNodeX - 10, minTopDistance - 10);
                            line.drawLine(targetNodeX - 10, minTopDistance - 10, targetNodeX - 10, endy);
                            line.drawLine(targetNodeX - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 21:
                            zone = "EN ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, minTopDistance - 10);
                            line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                            line.drawLine(endx, minTopDistance - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 22:
                            zone = "EE ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY - 10);
                            line.drawLine(maxRightDistance + 10, sourceNodeY - 10, endx + 10, sourceNodeY - 10);
                            line.drawLine(endx + 10, sourceNodeY - 10, endx + 10, endy);
                            line.drawLine(endx + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 23:
                            zone = "ES ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, maxBottomDistance + 10);
                            line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 24:
                            zone = "EW ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, minTopDistance - 10);
                            line.drawLine(sourceNodeX + sourcenw + 10, minTopDistance - 10, targetNodeX - 10, minTopDistance - 10);
                            line.drawLine(targetNodeX - 10, minTopDistance - 10, targetNodeX - 10, endy);
                            line.drawLine(targetNodeX - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 31:
                            zone = "SN ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10, endx, endy - 10);
                            line.drawLine(endx, endy - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 32:
                            zone = "SE ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 33:
                            zone = "SS ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 34:
                            zone = "SW ---> Z7B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, minLeftDistance - 10, maxBottomDistance + 10);
                            line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, minLeftDistance - 10, endy);
                            line.drawLine(minLeftDistance - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 41:
                            zone = "WN ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10);
                            line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10, endx, targetNodeY - 10);
                            line.drawLine(endx, targetNodeY - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 42:
                            zone = "WE ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, (targetNodeX + targetnw) + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 43:
                            zone = "WS ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                            line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 44:
                            zone = "WW ---> Z7B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY - 10, endx - 10, targetNodeY - 10);
                            line.drawLine(endx - 10, targetNodeY - 10, endx - 10, endy);
                            line.drawLine(endx - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z7B10"
                    }
                }
            } else {
                if (endy >= sourceNodeY + sourcenh) {
                    if ((endy <= sourceNodeY + sourcenh + 10) && (endx >= sourceNodeX)) {
                        zone = "Z5B10"
                    } else {
                        if (endx <= sourceNodeX - 10) {
                            zone = "Z6";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                                line.drawLine(endx, minTopDistance - 15, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                                line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                                line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, minTopDistance - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, minTopDistance - 10, targetNodeX - 10, minTopDistance - 10);
                                line.drawLine(targetNodeX - 10, minTopDistance - 10, targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z6";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx - 10, endy);
                                line.drawLine(endx - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                                line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, (targetNodeX + targetnw) + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                                line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy + 10);
                                line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy + 10, endx, endy + 10);
                                line.drawLine(endx, endy + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z6";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        } else {
                            zone = "Z5B";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, sourceNodeY - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY - 10, sourceNodeX - 10, sourceNodeY - 10);
                                line.drawLine(sourceNodeX - 10, sourceNodeY - 10, sourceNodeX - 10, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, minLeftDistance - 10, sourceNodeY - 10);
                                line.drawLine(minLeftDistance - 10, sourceNodeY - 10, minLeftDistance - 10, targetNodeY + targetnh + 10);
                                line.drawLine(minLeftDistance - 10, targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, minLeftDistance - 10, sourceNodeY - 10);
                                line.drawLine(minLeftDistance - 10, sourceNodeY - 10, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy + 10);
                                line.drawLine(targetNodeX - 10, endy + 10, endx, endy + 10);
                                line.drawLine(endx, endy + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z5B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx - 10, endy);
                                line.drawLine(endx - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z5B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        }
                    }
                } else {
                    if (endx <= sourceNodeX - 10) {
                        zone = "Z7A";
                        switch (connectionType) {
                        case 11:
                            zone = "NN ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                            line.drawLine(endx, minTopDistance - 15, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 12:
                            zone = "NE ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                            line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                            line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 13:
                            zone = "NS ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY - 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                            line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 14:
                            zone = "NW ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, minTopDistance - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, minTopDistance - 10, targetNodeX - 10, minTopDistance - 10);
                            line.drawLine(targetNodeX - 10, minTopDistance - 10, targetNodeX - 10, endy);
                            line.drawLine(targetNodeX - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 21:
                            zone = "EN ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, minTopDistance - 10);
                            line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                            line.drawLine(endx, minTopDistance - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 22:
                            zone = "EE ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + sourcenh + 10);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + sourcenh + 10, endx + 10, sourceNodeY + sourcenh + 10);
                            line.drawLine(endx + 10, sourceNodeY + sourcenh + 10, endx + 10, endy);
                            line.drawLine(endx + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 23:
                            zone = "ES ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, maxBottomDistance + 10);
                            line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 24:
                            zone = "EW ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, maxBottomDistance + 10);
                            line.drawLine(sourceNodeX + sourcenw + 10, maxBottomDistance + 10, targetNodeX - 10, maxBottomDistance + 10);
                            line.drawLine(targetNodeX - 10, maxBottomDistance + 10, targetNodeX - 10, endy);
                            line.drawLine(targetNodeX - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 31:
                            zone = "SN ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy - 10, endx, endy - 10);
                            line.drawLine(endx, endy - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 32:
                            zone = "SE ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 33:
                            zone = "SS ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 34:
                            zone = "SW ---> Z7A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, minLeftDistance - 10, maxBottomDistance + 10);
                            line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, minLeftDistance - 10, endy);
                            line.drawLine(minLeftDistance - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 41:
                            zone = "WN ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                            line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 42:
                            zone = "WE ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, (targetNodeX + targetnw) + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 43:
                            zone = "WS ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy + 10);
                            line.drawLine(sourceNodeX - Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), endy + 10, endx, endy + 10);
                            line.drawLine(endx, endy + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 44:
                            zone = "WW ---> Z7A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), sourceNodeY + halfSourcenh, targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(targetNodeX + targetnw + Math.floor((sourceNodeX - (targetNodeX + targetnw)) / 2), targetNodeY + targetnh + 10, endx - 10, targetNodeY + targetnh + 10);
                            line.drawLine(endx - 10, targetNodeY + targetnh + 10, endx - 10, endy);
                            line.drawLine(endx - 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break
                        }
                    } else {
                        zone = "Z7A10"
                    }
                }
            }
        } else {
            if (endy <= (sourceNodeY + halfSourcenh)) {
                if (endy <= sourceNodeY) {
                    if (endy >= sourceNodeY - 10 && endx <= sourcenw) {
                        zone = "Z1B10"
                    } else {
                        if (endx <= sourceNodeX + sourcenw) {
                            zone = "Z1B";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, sourceNodeY - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY - 10, maxRightDistance + 10, sourceNodeY - 10);
                                line.drawLine(maxRightDistance + 10, sourceNodeY - 10, maxRightDistance + 10, endy - 10);
                                line.drawLine(maxRightDistance + 10, endy - 10, endx, endy - 10);
                                line.drawLine(endx, endy - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx + 15, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx + 15, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx + 15, endy);
                                line.drawLine(endx + 15, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine(sourceNodeX + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, maxRightDistance + 10, sourceNodeY + sourcenh + 10);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + sourcenh + 10, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + 10);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + 10, targetNodeY + targetnh + 10);
                                line.drawLine(sourceNodeX + sourcenw + 10, targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z1B";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, minTopDistance - 10);
                                line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z1B";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        } else {
                            zone = "Z2";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                                line.drawLine(endx, minTopDistance - 15, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx + 15, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx + 15, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx + 15, endy);
                                line.drawLine(endx + 15, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine((sourceNodeX) + halfSourcenw, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)));
                                line.drawLine(endx, (sourceNodeY - Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2)), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine(sourceNodeX + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), minTopDistance - 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                                line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10, endx, targetNodeY - 10);
                                line.drawLine(endx, targetNodeY - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z2";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, minTopDistance - 10);
                                line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                                line.drawLine(endx, minTopDistance - 10, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx + 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(sourceNodeX - 10, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2));
                                line.drawLine(endx, targetNodeY + targetnh + Math.floor((sourceNodeY - (targetNodeY + targetnh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z2";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        }
                    }
                } else {
                    if (endx <= sourceNodeX + sourcenw + 10) {
                        zone = "Z3A10"
                    } else {
                        zone = "Z3A";
                        switch (connectionType) {
                        case 11:
                            zone = "NN ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                            line.drawLine(endx, minTopDistance - 15, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 12:
                            zone = "NE ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                            line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                            line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 13:
                            zone = "NS ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                            line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 14:
                            zone = "NW ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 21:
                            zone = "EN ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), minTopDistance - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), minTopDistance - 10, endx, minTopDistance - 10);
                            line.drawLine(endx, minTopDistance - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 22:
                            zone = "EE ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10, endx + 10, targetNodeY - 10);
                            line.drawLine(endx + 10, targetNodeY - 10, endx + 10, endy);
                            line.drawLine(endx + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 23:
                            zone = "ES ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                            line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 24:
                            zone = "EW ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 31:
                            zone = "SN ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10, endx, targetNodeY - 10);
                            line.drawLine(endx, targetNodeY - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 32:
                            zone = "SE ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                            line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                            line.drawLine(maxRightDistance + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 33:
                            zone = "SS ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 34:
                            zone = "SW ---> Z3A";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 41:
                            zone = "WN ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, minTopDistance - 10);
                            line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                            line.drawLine(endx, minTopDistance - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 42:
                            zone = "WE ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, minTopDistance - 10);
                            line.drawLine(sourceNodeX - 10, minTopDistance - 10, maxRightDistance + 10, minTopDistance - 10);
                            line.drawLine(maxRightDistance + 10, minTopDistance - 10, maxRightDistance + 10, endy);
                            line.drawLine(maxRightDistance + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 43:
                            zone = "WS ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, maxBottomDistance + 10);
                            line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 44:
                            zone = "WW ---> Z3A";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY - 10);
                            line.drawLine(sourceNodeX - 10, sourceNodeY - 10, targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                            line.drawLine(targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break
                        }
                    }
                }
            } else {
                if (endy >= sourceNodeY + sourcenh) {
                    if ((endy <= sourceNodeY + sourcenh + 10) && (endx <= sourceNodeX + sourcenw)) {
                        zone = "Z5A10"
                    } else {
                        if (endx <= sourceNodeX + sourcenw) {
                            zone = "Z5A";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, sourceNodeY - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + 10, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY - 10, sourceNodeX + sourcenw + 10, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, (sourceNodeY + sourcenh) + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, maxRightDistance + 10, minTopDistance - 10);
                                line.drawLine(maxRightDistance + 10, minTopDistance - 10, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, maxRightDistance + 10, sourceNodeY - 10);
                                line.drawLine(maxRightDistance + 10, sourceNodeY - 10, maxRightDistance + 10, targetNodeY + targetnh + 10);
                                line.drawLine(maxRightDistance + 10, targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, minLeftDistance - 10, sourceNodeY - 10);
                                line.drawLine(minLeftDistance - 10, sourceNodeY - 10, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, maxBottomDistance + 10);
                                line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX + sourcenw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX - 10, endy);
                                line.drawLine(targetNodeX - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX + targetnw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(targetNodeX + targetnw + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), targetNodeX + targetnw + 10, endy + 10);
                                line.drawLine(targetNodeX + targetnw + 10, endy + 10, endx, endy + 10);
                                line.drawLine(endx, endy + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z5A";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z5A";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        } else {
                            zone = "Z4";
                            switch (connectionType) {
                            case 11:
                                zone = "NN ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                                line.drawLine(endx, minTopDistance - 15, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 12:
                                zone = "NE ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                                line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                                line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 13:
                                zone = "NS ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                                line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 14:
                                zone = "NW ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                                line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 21:
                                zone = "EN ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                                line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 22:
                                zone = "EE ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, maxRightDistance + 10, sourceNodeY + halfSourcenh);
                                line.drawLine(maxRightDistance + 10, sourceNodeY + halfSourcenh, maxRightDistance + 10, endy);
                                line.drawLine(maxRightDistance + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 23:
                                zone = "ES ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy + 10);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy + 10, endx, endy + 10);
                                line.drawLine(endx, endy + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 24:
                                zone = "EW ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                                line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 31:
                                zone = "SN ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 32:
                                zone = "SE ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 33:
                                zone = "SS ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                                line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 34:
                                zone = "SW ---> Z4";
                                line.clear();
                                line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, endy);
                                line.drawLine((sourceNodeX) + halfSourcenw, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 41:
                                zone = "WN ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx, endy);
                                line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                                line.paint();
                                break;
                            case 42:
                                zone = "WE ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2));
                                line.drawLine(endx + 10, sourceNodeY + sourcenh + Math.floor((targetNodeY - (sourceNodeY + sourcenh)) / 2), endx + 10, endy);
                                line.drawLine(endx + 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break;
                            case 43:
                                zone = "WS ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, maxBottomDistance + 10);
                                line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                                line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                                line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                                line.paint();
                                break;
                            case 44:
                                zone = "WW ---> Z4";
                                line.clear();
                                line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                                line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, endy);
                                line.drawLine(minLeftDistance - 10, endy, endx, endy);
                                line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                                line.paint();
                                break
                            }
                        }
                    }
                } else {
                    if (endx <= sourceNodeX + sourcenw + 10) {
                        zone = "Z3B10"
                    } else {
                        zone = "Z3B";
                        switch (connectionType) {
                        case 11:
                            zone = "NN ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 15);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 15, endx, minTopDistance - 15);
                            line.drawLine(endx, minTopDistance - 15, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 12:
                            zone = "NE ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY, (sourceNodeX) + halfSourcenw, minTopDistance - 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, minTopDistance - 10, targetNodeX + targetnw + 10, minTopDistance - 10);
                            line.drawLine(targetNodeX + targetnw + 10, minTopDistance - 10, targetNodeX + targetnw + 10, endy);
                            line.drawLine(targetNodeX + targetnw + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx - 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 13:
                            zone = "NS ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10, endx, targetNodeY + targetnh + 10);
                            line.drawLine(endx, targetNodeY + targetnh + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 14:
                            zone = "NW ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY, sourceNodeX + halfSourcenw, sourceNodeY - 10);
                            line.drawLine(sourceNodeX + halfSourcenw, sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY - 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 21:
                            zone = "EN ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, endx, sourceNodeY + halfSourcenh);
                            line.drawLine(endx, sourceNodeY + halfSourcenh, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 22:
                            zone = "EE ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY + targetnh + 10, endx + 10, targetNodeY + targetnh + 10);
                            line.drawLine(endx + 10, targetNodeY + targetnh + 10, endx + 10, endy);
                            line.drawLine(endx + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 23:
                            zone = "ES ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy + 10, endx, endy + 10);
                            line.drawLine(endx, endy + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 24:
                            zone = "EW ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX + sourcenw, sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + halfSourcenh, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 31:
                            zone = "SN ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), targetNodeY - 10, endx, targetNodeY - 10);
                            line.drawLine(endx, targetNodeY - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 32:
                            zone = "SE ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                            line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                            line.drawLine(maxRightDistance + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 33:
                            zone = "SS ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, maxBottomDistance + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 34:
                            zone = "SW ---> Z3B";
                            line.clear();
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh, (sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10);
                            line.drawLine((sourceNodeX) + halfSourcenw, sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(sourceNodeX + sourcenw + Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 41:
                            zone = "WN ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, minTopDistance - 10);
                            line.drawLine(minLeftDistance - 10, minTopDistance - 10, endx, minTopDistance - 10);
                            line.drawLine(endx, minTopDistance - 10, endx, endy);
                            line.fillPolygon(new Array(endx - 5, endx + 5, endx), new Array(endy, endy, endy + 5));
                            line.paint();
                            break;
                        case 42:
                            zone = "WE ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, maxBottomDistance + 10);
                            line.drawLine(sourceNodeX - 10, maxBottomDistance + 10, maxRightDistance + 10, maxBottomDistance + 10);
                            line.drawLine(maxRightDistance + 10, maxBottomDistance + 10, maxRightDistance + 10, endy);
                            line.drawLine(maxRightDistance + 10, endy, endx, endy);
                            line.fillPolygon(new Array(endx + 5, endx + 5, endx), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break;
                        case 43:
                            zone = "WS ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, minLeftDistance - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(minLeftDistance - 10, sourceNodeY + halfSourcenh, minLeftDistance - 10, maxBottomDistance + 10);
                            line.drawLine(minLeftDistance - 10, maxBottomDistance + 10, endx, maxBottomDistance + 10);
                            line.drawLine(endx, maxBottomDistance + 10, endx, endy);
                            line.fillPolygon(new Array(endx, endx - 5, endx + 5), new Array(endy - 5, endy + 5, endy + 5));
                            line.paint();
                            break;
                        case 44:
                            zone = "WW ---> Z3B";
                            line.clear();
                            line.drawLine(sourceNodeX, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + halfSourcenh);
                            line.drawLine(sourceNodeX - 10, sourceNodeY + halfSourcenh, sourceNodeX - 10, sourceNodeY + sourcenh + 10);
                            line.drawLine(sourceNodeX - 10, sourceNodeY + sourcenh + 10, targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10);
                            line.drawLine(targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), sourceNodeY + sourcenh + 10, targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy);
                            line.drawLine(targetNodeX - Math.floor((targetNodeX - (sourceNodeX + sourcenw)) / 2), endy, endx, endy);
                            line.fillPolygon(new Array(endx, endx, endx + 5), new Array(endy - 5, endy + 5, endy));
                            line.paint();
                            break
                        }
                    }
                }
            }
        }
    }
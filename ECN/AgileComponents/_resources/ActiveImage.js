var AIE_dom = (document.getElementById) ? true : false; 
var AIE_ns5 = ((navigator.userAgent.indexOf("Gecko")>-1) && AIE_dom) ? true: false; 
var AIE_ie5 = ((navigator.userAgent.indexOf("MSIE")>-1) && AIE_dom) ? true : false; 
var AIE_ns4 = (document.layers && !AIE_dom) ? true : false; 
var AIE_ie4 = (document.all && !AIE_dom) ? true : false; 
var AIE_nodyn = (!AIE_ns5 && !AIE_ns4 && !AIE_ie4 && !AIE_ie5) ? true : false; 
var AIE_mouseDown = false;
var AIE_mouseX, AIE_mouseY; 
	
/*function debugTrace(str)
{
	getObject("AU_Debug").value = str + "\n" + getObject("AU_Debug").value;
}*/	

function AIE_trackMouse(id, evt)
{
	var positions = AIE_getPositions(id, evt);
	AIE_getObject(id + "xPosLabel").innerHTML = positions[0];
	AIE_getObject(id + "yPosLabel").innerHTML = positions[1];

	if ((AIE_ie5 && window.event.button == 1) || ((AIE_ns4 || AIE_ns5) && AIE_mouseDown))
	{
		var oldPos = AIE_getObject(id + "positions").value.split(',');
		var strPos = oldPos[0] + "," + oldPos[1] + "," + positions[0] + "," + positions[1];
		AIE_setStoredPositions(id, strPos);
		AIE_redrawPositions(id);
	}
}

function AIE_setPosition(id, evt)
{
	var positions = AIE_getPositions(id, evt);
	var strPos = positions[0] + "," + positions[1];
	AIE_setStoredPositions(id, strPos);
	AIE_redrawPositions(id);
	AIE_mouseDown = true;
}

function AIE_resetMouse()
{
	AIE_mouseDown = false;
}

function AIE_resetPositions(id)
{
	AIE_getObject(id + "xPosLabel").innerHTML = AIE_getObject(id + "yPosLabel").innerHTML = '*';
}

function AIE_getObject(id)
{
	return document.getElementById(id);
}

function AIE_moveObject(id, xpos, ypos)
{
	AIE_getObject(id).style.top = ypos;
	AIE_getObject(id).style.left = xpos;
}

function AIE_hideObject(id)
{
	AIE_getObject(id).style.display = 'none';
	AIE_getObject(id).style.visibility = 'hidden';
}

function AIE_showObject(id)
{
	AIE_getObject(id).style.display = 'block';
	AIE_getObject(id).style.visibility = 'visible';
}

function AIE_getPositions(id, evt)
{
	if (AIE_ns4 || AIE_ns5)
		return new Array(evt.pageX - AIE_findPosX(AIE_getObject(id + "workImage")),evt.pageY - AIE_findPosY(AIE_getObject(id + "workImage")));
	else
		return new Array(window.event.offsetX,window.event.offsetY);
}

function AIE_findPosX(obj)
{	
	var curleft = 0; 
	if (obj.offsetParent) 
	{ 
		while (obj.offsetParent) 
		{ 
			if (obj.tagName != 'DIV')
				curleft += obj.offsetLeft; 
			obj = obj.offsetParent; 
		} 
	} 
	else if (obj.x)
		curleft += obj.x; 
	return curleft; 
}

function AIE_findPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			if (obj.tagName != 'DIV')
				curtop += obj.offsetTop;
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)
		curtop += obj.y;
	return curtop;
}

function AIE_setStoredPositions(id, str)
{
	AIE_getObject(id + "storedPos").innerHTML = str;
	AIE_getObject(id + "positions").value = str;
}

function AIE_redrawPositions(id)
{
	var positions = AIE_getObject(id + "positions").value.split(',');
	//hideObject(id + "posA");
	//hideObject(id + "posB");
		
	if (positions.length == 4)
	{
		AIE_drawRectangle(id, positions[0],positions[1],positions[2],positions[3]);
		AIE_showRectangle(id);
	}
	else if (positions.length == 2)
	{
		AIE_hideRectangle(id);
		/*var imageX = findPosX(getObject(id + "workImage")) - 7;
		var imageY = findPosY(getObject(id + "workImage")) - 7;
		showObject(id + "posA");
		moveObject(id + "posA", parseInt(positions[0]) + imageX, parseInt(positions[1]) + imageY);*/
	}
	else
		AIE_hideRectangle(id);
}

function AIE_showToolbar(id)
{
	var imageX = AIE_findPosX(AIE_getObject(id + "workImage"));
	var imageY = AIE_findPosY(AIE_getObject(id + "workImage"));
	AIE_showObject(id + "toolbar");
	AIE_moveObject(id + "toolbar", imageX + 5, imageY + 5);
}

function AIE_hideToolbar(id)
{
	AIE_hideObject(id + "toolbar");
}

function AIE_showRectangle(id)
{
	/*showObject(id + "selectTop");
	showObject(id + "selectLeft");
	showObject(id + "selectRight");
	showObject(id + "selectBottom");*/
	
	AIE_showObject(id + "rubber");
	AIE_showObject(id + "rubber2");
}

function AIE_hideRectangle(id)
{
	/*hideObject(id + "selectTop");
	hideObject(id + "selectLeft");
	hideObject(id + "selectRight");
	hideObject(id + "selectBottom");*/

	AIE_hideObject(id + "rubber");
	AIE_hideObject(id + "rubber2");
}

function AIE_drawRectangle(id, xFrom, yFrom, xTo, yTo)
{
	var imageX = AIE_findPosX(AIE_getObject(id + "workImage"));
	var imageY = AIE_findPosY(AIE_getObject(id + "workImage"));
	
	xFrom = parseInt(xFrom) + imageX;
	yFrom = parseInt(yFrom) + imageY;
	xTo = parseInt(xTo) + imageX;
	yTo = parseInt(yTo) + imageY;
	
	AIE_moveObject(id + "rubber", xFrom, yFrom);
	AIE_moveObject(id + "rubber2", xFrom, yFrom);

	AIE_getObject(id + "rubber").style.width = Math.abs(xTo - xFrom - (xTo - xFrom < 0 ? 1 : -1));
	AIE_getObject(id + "rubber2").style.width = AIE_getObject(id + "rubber").style.width;

	AIE_getObject(id + "rubber").style.height = Math.abs(yTo - yFrom + 1 - (yTo - yFrom < 0 ? 2 : 0));
	AIE_getObject(id + "rubber2").style.height = AIE_getObject(id + "rubber").style.height;
	
	AIE_getObject(id + "rubber").style.top = (yTo - yFrom > 0 ? yFrom : yTo);
	AIE_getObject(id + "rubber2").style.top = AIE_getObject(id + "rubber").style.top;
	
	AIE_getObject(id + "rubber").style.left = (xTo - xFrom > 0 ? xFrom : xTo);
	AIE_getObject(id + "rubber2").style.left = AIE_getObject(id + "rubber").style.left;

	/*getObject(id + "selectTop").style.top = yFrom;
	getObject(id + "selectTop").style.left = (xTo - xFrom > 0 ? xFrom : xTo);
	getObject(id + "selectLeft").style.top = (yTo - yFrom > 0 ? yFrom : yTo);
	getObject(id + "selectLeft").style.left = xFrom;
	//getObject(id + "selectRight").style.top = (yTo - yFrom > 0 ? yFrom : yTo);
	getObject(id + "selectRight").style.top = getObject(id + "selectLeft").style.top;
	getObject(id + "selectRight").style.left = xTo;
	getObject(id + "selectBottom").style.top = (yTo - yFrom > 0 ? yTo : yTo);
	//getObject(id + "selectBottom").style.left = (xTo - xFrom > 0 ? xFrom : xTo);
	getObject(id + "selectBottom").style.left = getObject(id + "selectTop").style.left;
	
	getObject(id + "selectTop").style.width = Math.abs(xTo - xFrom - (xTo - xFrom < 0 ? 1 : 0));
	getObject(id + "selectBottom").style.width = Math.abs(xTo - xFrom + (xTo - xFrom >= 0 ? 1 : -1));
	getObject(id + "selectLeft").style.height = Math.abs(yTo - yFrom);
	getObject(id + "selectRight").style.height = Math.abs(yTo - yFrom);*/
}

/*function updateOnClickPositions(id, evt)
{
	var oldValues = getObject(id + "positions").value.split(',');
	var newValues;
	var positions = getPositions(id, evt);
	//alert(positions[0] + "-" + positions[1]);
	var imageX = findPosX(getObject(id + "workImage")) - 7;
	var imageY = findPosY(getObject(id + "workImage")) - 7;
	
	if (oldValues.length == 4)
	{
		newValues = "";
		hideObject(id + "posA");
		hideObject(id + "posB");
	}
	else if (oldValues.length == 2)
	{
		//alert((parseInt(oldValues[0]) + imageX) + "-" + imageX);
		newValues = oldValues + "," + positions;
		showObject(id + "posA");
		showObject(id + "posB");
		moveObject(id + "posA", parseInt(oldValues[0]) + imageX, parseInt(oldValues[1]) + imageY);
		moveObject(id + "posB", parseInt(positions[0]) + imageX, parseInt(positions[1]) + imageY);
	}
	else
	{
		newValues = positions;
		showObject(id + "posA");
		moveObject(id + "posA", parseInt(positions[0]) + imageX, parseInt(positions[1]) + imageY);
		hideObject(id + "posB");
	}
	
	setStoredPositions(id, newValues);
}	*/


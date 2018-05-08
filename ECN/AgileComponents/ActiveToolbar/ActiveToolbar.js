
// ActiveToolbar v1 Client Side Code
// Copyright (c) 2004 Active Up S.P.R.L. - All Right Reserved
// http://www.activeup.com/?r=atb2

// LIB (API)

var ATB_ie5 = (document.all) ? true : false;
var ATB_ns6 = (navigator.appName == "Netscape") ? true: false;
var ATB_currentIDWindow = null; 
var ATB_currentIDIFrame = null;
var ATB_currentResize = null; 
var ATB_currentTool = null;
var ATB_currentContainer = null;
var ATB_xoff = 0; 
var ATB_yoff = 0;
var ATB_rsxoff = 0; 
var ATB_rsyoff = 0;
var ATB_oldActive = null; 
var ATB_zindex = 1; 
var ATB_mx = 0; 
var ATB_my = 0; 
var ATB_currentOpenedDDLID = null;

function ATB_getHeight(id)
{
	var window = document.getElementById(id + '_window');
	return parseInt(window.style.height);
}

function ATB_getWidth(id)
{
	var window = document.getElementById(id + '_window');
	return parseInt(window.style.width);
}

function ATB_hidePopup(id)
{
    document.getElementById(id + '_window').style.display = 'none';
    document.getElementById(id + '_shadow').style.display = 'none';
    document.getElementById(id + '_iframe').style.display = 'none';
}

function ATB_showPopup(id)
{
	  var window = document.getElementById(id + '_window').style;
      var shadow = document.getElementById(id + '_shadow').style;
     
      if (window.display != 'block')
      {
      window.display = 'block';
      shadow.display = 'block';
      shadow.zIndex = ++ATB_zindex;
      window.zIndex = ++ATB_zindex;
      }
      
      ATB_setPopupActive(document.getElementById(id + '_window'));
}

function ATB_dragablePopup(isDragable)
{
	if (isDragable)
	{
		this.IDTitle.onmousedown = ATB_mousedown;
		this.IDTitle.onmouseup = ATB_mouseup;
	}
	
	else
	{
		this.IDTitle.onmousedown = null;
		this.IDTitle.onmouseup = null;
	}
}

function ATB_setTitle(id, text)
{
	document.getElementById(id + '_titleText').innerHTML = text;
}

function ATB_setContents(id, text)
{
	document.getElementById(id+ '_contents').innerHTML = text;
}

function ATB_setSize(id, width, height)
{
	width = Math.max(width, 100);
	height = Math.max(height, 80);

	var window = document.getElementById(id + '_window');
	window.style.width = width;
	window.style.height = height;		
	
	var shadow = document.getElementById(id + '_shadow');
	shadow.style.width = (ATB_ie5) ? width : width + 4;
	shadow.style.height = (ATB_ie5) ? height : height + 6;
	
	var title = document.getElementById(id + '_title');
	title.style.width = (ATB_ie5) ? width - 8 : width - 5;
	
	var button = document.getElementById(id + '_button');
	button.style.left = parseInt(title.style.width) - 20;
	
	var contents = document.getElementById(id + '_contents');
	contents.style.width = (ATB_ie5) ? width - 7 : width - 13;
	contents.style.height = (ATB_ie5) ? height - 36 : height - 36;	
	
	if (document.getElementById(id + '_isTitleShowed').value == 'False')
	{
		window.style.height = parseInt(window.style.height) - 16;
	}
}

function ATB_showTitle(id,show)
{
	var isTitleShowed = document.getElementById(id + '_isTitleShowed');
	
	if (show)
	{
		if (isTitleShowed.value == 'False')
		{
	
			var title = document.getElementById(id + '_title');
			title.style.visibility = 'visible';
	
			var contents = document.getElementById(id + '_contents');
			contents.style.top = 24;
	
			var window = document.getElementById(id + '_window');
			window.style.height = parseInt(window.style.height) + 16;
			
			document.getElementById(id + '_isTitleShowed').value = 'True';
		}
	}
	
	else
	{
		if (isTitleShowed.value == 'True')
		{
	
			var title = document.getElementById(id + '_title');
			title.style.visibility = 'hidden';
	
			var contents = document.getElementById(id + '_contents');
			contents.style.top = 8;
	
			var window = document.getElementById(id + '_window');
			window.style.height = parseInt(window.style.height) - 16;
			
			document.getElementById(id + '_isTitleShowed').value = 'False';
		}
	}
	
}

function ATB_mousemouve(evt)
{
    //document.getElementById('_text').value = event.clientX + ':' + event.clientY; 

    ATB_mx = (ATB_ie5) ? event.clientX + document.body.scrollLeft:evt.pageX;
    ATB_my = (ATB_ie5) ? event.clientY + document.body.scrollTop:evt.pageY;
    
    if(!ATB_ns6)
        ATB_moveObject();
        
    if((ATB_currentIDWindow != null) || (ATB_currentResize != null) || (ATB_currentTool != null) || (ATB_currentIDIFrame != null))
        return false;
}

function ATB_movePopupTo(id,x,y)
{
      var window = document.getElementById(id + '_window');
      var shadow = document.getElementById(id + '_window');
      window.style.left = x;
      shadow.style.left = x + 8;
      window.style.top = y
      shadow.style.top = y + 8;
}

function ATB_moveObject()
{
	if (ATB_currentTool != null)
    {
      	var x = ATB_mx + ATB_xoff;
        var y = ATB_my + ATB_yoff;
        
    	var isOverContainer = false;
    	var containers = document.getElementById("Containers").value; 
    	if (containers != null && containers != "")
    	{
    		var reg=new RegExp('[,]+', 'g');
	    	
    		var containerItems = containers.split(reg); 
    		for (i = 0 ; i < containerItems.length ; i++)
    		{
    			var container = document.getElementById(containerItems[i]); 
				var coL = container.offsetLeft;
				var coT = container.offsetTop;
				var coW = container.clientWidth;
				var coH = container.clientHeight;
			   
				var medH = ATB_currentTool.clientHeight/2;
			   
				if (x > coL && x < coL + coW)
    				if (y + medH > coT && y + medH < coT + coH)
    	   				isOverContainer = true;
		    	   	
    			if (isOverContainer == true)
				{
				    var backgroundColorDock = document.getElementById(containerItems[i] + "_backColorDock");
				    if (backgroundColorDock != null)
						container.style.backgroundColor = backgroundColorDock.value;
					ATB_currentContainer = container; break;
				}
				else
				{   var backgroundColor = document.getElementById(containerItems[i] + "_backColor");
					if (backgroundColor != null)
					   container.style.backgroundColor = backgroundColor.value;
					else
					   container.style.backgroundColor = "transparent";
					ATB_currentContainer = null;
				}	
   			}
   		}
    	
        if (x > 15)
           ATB_currentTool.style.left = x + 'px';
        else
           ATB_currentTool.style.left = 0 + 'px';
        
        if (y > 15)
           ATB_currentTool.style.top = y + 'px';
        else
           ATB_currentTool.style.top = 0 + 'px';
    }

    if (ATB_currentIDWindow != null)
    {
       var x = ATB_mx + ATB_xoff;
       var y = ATB_my + ATB_yoff;
       ATB_currentIDWindow.style.left = x + 'px';
       currIDShadow.style.left = x + 8 + 'px';
       ATB_currentIDWindow.style.top = y + 'px';
       currIDShadow.style.top = y + 8 + 'px';
       ATB_currentIDIFrame.style.left = x + 'px';
       ATB_currentIDIFrame.style.top = y + 'px';
    }
    
    if(ATB_currentResize != null)
    {
       var rx = ATB_mx + ATB_rsxoff;
       var ry = ATB_my + ATB_rsyoff;
       var c = ATB_currentResize;
       c.style.left = Math.max(rx,((ATB_ie5) ? 88 : 92)) + 'px';
       c.style.top = Math.max(ry,((ATB_ie5) ? 68 : 72)) + 'px';
       c.IDS[0].style.width = Math.max(rx + ((ATB_ie5) ? 12 : 8), 100) + 'px';
       c.IDS[0].style.height = Math.max(ry + ((ATB_ie5) ? 12 : 8), 80) + 'px';
       c.IDS[1].style.width = Math.max(rx + ((ATB_ie5) ? 4 : 3), ((ATB_ns6) ? 95 : 92)) + 'px';
       c.IDS[5].style.left = parseInt(c.IDS[1].style.width) - 20 + 'px';
       c.IDS[3].style.width = Math.max(rx + 12,((ATB_ie5) ? 100 : 104)) + 'px';
       c.IDS[3].style.height = Math.max(ry + ((ATB_ie5) ? 12 : 13),((ATB_ie5) ? 80 : 86)) + 'px';
       c.IDS[2].style.width = Math.max(rx - ((ATB_ie5) ? - 5 : 5),((ATB_ie5) ? 92 : 87)) + 'px';
       c.IDS[2].style.height = Math.max(ry - ((ATB_ie5) ? 24 : 28), 44) + 'px';
       c.IDS[8] = parseInt(c.IDS[0].style.height);
   }
}

function ATB_startResize(evt)
{
    var ex = (ATB_ie5) ? event.clientX+document.body.scrollLeft:evt.pageX;
    var ey = (ATB_ie5) ? event.clientY+document.body.scrollTop:evt.pageY;
    
    ATB_rsxoff = parseInt(this.style.left) - ex;
    ATB_rsyoff = parseInt(this.style.top) - ey;
    ATB_currentResize = this;
    
    if(ATB_ns6)
        this.IDS[2].style.overflow = 'hidden';
        
    return false;
}

function ATB_mouseup()
{
    ATB_currentIDWindow = null;
    ATB_currentIDIFrame = null;
    self.resizeBy(0,1);
    self.resizeBy(0,-1);
}

function ATB_mousedown(evt)
{
    var ex = (ATB_ie5) ? event.clientX + document.body.scrollLeft : evt.pageX;
    var ey = (ATB_ie5) ? event.clientY + document.body.scrollTop : evt.pageY;
    
    ATB_xoff = parseInt(this.IDS[0].style.left) - ex;
    ATB_yoff = parseInt(this.IDS[0].style.top) - ey;
    
    ATB_currentIDWindow = this.IDS[0];
    ATB_currentIDIFrame = this.IDS[9];    
    currIDShadow = this.IDS[3];
    
    return false;
}

function ATB_createDiv(x,y,width,height,bgc,id)
{
   var div = document.createElement('div');
   div.setAttribute('id',id); 
   div.style.position = 'absolute';
   div.style.left = x + 'px';
   div.style.top = y + 'px';
   div.style.width = width + 'px';
   div.style.height = height + 'px';
   div.style.backgroundColor = bgc;
   div.style.visibility = 'visible';
   div.style.padding = '0px 0px 0px 0px';
   return div;
}

function ATB_createPopup(id,x,y,width,height,title,text,winbgcolor,winbordercolor,winborderstyle,winborderwidth,titlebgcolor,inactivetitlebgcolor,titlebordercolor,titleborderstyle,titleborderwidth,titleforecolor,contentbgcolor,contentbordercolor,contentborderstyle,contentborderwidth,scrollcolor,dragable,showed)
{
  if (document.getElementById(id + '_window') != null)
	return;
  
   var tw, th; 
   width = Math.max(width,100);
   height = Math.max(height,80);
   var rdiv=new ATB_createDiv(width - ((ATB_ie5) ? 12 : 8), height - ((ATB_ie5) ? 12 : 8), 7, 7, '', id + '_resize');
   tw = (ATB_ie5) ? width : width + 4;
   th = (ATB_ie5) ? height : height + 6;
   var shadow = new ATB_createDiv(x + 8, y + 8, tw, th, "black", id + '_shadow');
       
   if(ATB_ie5)
      shadow.style.filter = "alpha(opacity = 50)";
   else 
      shadow.style.MozOpacity = .5;
      
   shadow.style.visibility = 'hidden';          
   shadow.style.zIndex = ++ATB_zindex;
   
   var outerdiv = new ATB_createDiv(x,y,width,height,winbgcolor,id + '_window');
   if (winborderstyle.toUpperCase() != "NOTSET")
   {
	outerdiv.style.borderStyle = winborderstyle;
	outerdiv.style.borderWidth = winborderwidth + 'px';
	outerdiv.style.borderColor = winbordercolor;
   }
   outerdiv.style.zIndex = ++ATB_zindex;
   outerdiv.style.visibility = 'visible';          
   
   var isTitleShowed = document.createElement("input") 
   isTitleShowed.setAttribute("type","hidden");
   isTitleShowed.setAttribute("name", id + "_isTitleShowed");
   isTitleShowed.setAttribute("id", id + "_isTitleShowed");
   isTitleShowed.setAttribute("value", "True");
   document.body.appendChild(isTitleShowed);
   
   tw=(ATB_ie5) ? width - 7 : width - 5;
   th=(ATB_ie5) ? height + 4 : height - 4;
   var titlebar = new ATB_createDiv(1,1,tw,20,titlebgcolor,id + '_title');	
   if (titleborderstyle.toUpperCase() != "NOTSET")
   {
    titlebar.style.borderColor = titlebordercolor;
	titlebar.style.borderStyle = titleborderstyle;
	titlebar.style.borderWidth = titleborderwidth + 'px';
   }
   titlebar.style.overflow = "hidden";
   titlebar.style.cursor = "default";
   titlebar.style.visibility = 'visible';          
   titlebar.innerHTML = '<span id="'+id+'_titleText" style="position:absolute; left:3px; top:1px; font:bold 10pt sans-serif; color:'+ titleforecolor + '; height:18px; overflow:hidden; clip-height:16px;">' + title + '</span><span id="'+id+'_button" style="position:absolute; width:48px; height:16px; left:'+(tw-18)+'px; top:2px;"><img src="icons/close.gif" width="16" height="16" id="'+id+'_close"></span>';
   
   tw = (ATB_ie5) ? width - 10 : width - 13;
   th=(ATB_ie5) ? height - 30 : height - 30;
   var contents = new ATB_createDiv(2,24,tw,th,contentbgcolor,id + '_contents');
   if (contentborderstyle.toUpperCase() != "NOTSET")
   {
	contents.style.borderColor = contentbordercolor;
	contents.style.borderStyle = contentborderstyle;
	contents.style.borderWidth = contentborderwidth;
   }
   contents.style.overflow = "auto";
   contents.style.padding = "0px 2px 0px 4px";

   //contents.style.color = textcolor;
   contents.style.visibility = 'visible';          
      
   if(ATB_ie5)
      contents.style.scrollbarBaseColor = scrollcolor;
      
   contents.innerHTML = text;
   outerdiv.appendChild(titlebar);
   outerdiv.appendChild(contents);
   outerdiv.appendChild(rdiv);
   var iframe = document.createElement("IFRAME");
   iframe.setAttribute("id", id + "_iframe");
   iframe.style.border = 0;
   iframe.width = 0;
   iframe.height = 0;
   iframe.style.position = "absolute";
   document.body.appendChild(iframe);
   document.body.appendChild(shadow);
   document.body.appendChild(outerdiv);
      
   var IDS = new Array();
   IDS[0] = document.getElementById(id+'_window');
   IDS[1] = document.getElementById(id+'_title');
   IDS[2] = document.getElementById(id+'_contents');
   IDS[3] = document.getElementById(id+'_shadow');
   IDS[4] = document.getElementById(id+'_resize');
   IDS[5] = document.getElementById(id+'_button');
   IDS[6] = document.getElementById(id+'_close');
   IDS[7] = id;
   IDS[8] = height;
   IDS[9] = iframe;
   
   this.IDWindow = IDS[0]; 
   this.IDWindow.IDS = IDS;
   
   this.IDTitle = IDS[1]; 
   this.IDTitle.IDS = IDS;
   
   this.IDContent = IDS[2]; 
   this.IDContent.IDS = IDS;
   
   this.IDShadow = IDS[3]; 
   this.IDShadow.IDS = IDS;
   
   this.IDResize = IDS[4]; 
   this.IDResize.IDS = IDS;
   
   this.IDButton = IDS[5]; 
   this.IDButton.IDS = IDS;
   
   this.IDClose = IDS[6]; 
   this.IDClose.IDS = IDS;
   
   this.IDWindow.activecolor = titlebgcolor;
   this.IDWindow.inactivecolor = inactivetitlebgcolor;
      
   if(ATB_oldActive != null)
      ATB_oldActive.IDS[1].style.backgroundColor = ATB_oldActive.inactivecolor;
      
   ATB_oldActive = this.IDWindow;
   this.IDClose.onclick = new Function("ATB_hidePopup('"+id+"');");
       
   this.IDWindow.onmousedown = function()
   {
	 	ATB_setPopupActive(this);
   }
    
   if(dragable == 'True')
   {
      this.IDTitle.onmousedown = ATB_mousedown;
      this.IDTitle.onmouseup = ATB_mouseup;
   }
      
   if(ATB_ns6)setInterval('ATB_moveObject()',40);

/*   document.onmousemove = ATB_mousemouve;
   document.onmouseup = new Function("ATB_currentResize = null");*/
   
   if (showed == 'False')
   {
     ATB_hidePopup(id);
   }

}

function ATB_setPopupActive(popup)
{
	
	   if(ATB_oldActive!=null)
       {
          ATB_oldActive.IDS[1].style.backgroundColor=ATB_oldActive.inactivecolor;
          popup.IDS[0].style.zIndex -= 10000;
       }
         
       if(ATB_ns6)
         popup.IDS[2].style.overflow='auto';
            
       ATB_oldActive=popup;
       popup.IDS[1].style.backgroundColor = popup.activecolor;
       popup.IDS[3].style.zIndex = ++ATB_zindex;
       popup.style.zIndex = ++ATB_zindex;
       popup.IDS[0].style.display = "block";
       popup.IDS[9].style.width = popup.IDS[0].offsetWidth;
       popup.IDS[9].style.height = popup.IDS[0].offsetHeight;
       popup.IDS[9].style.top = popup.IDS[0].style.top;
       popup.IDS[9].style.left = popup.IDS[0].style.left;
       popup.IDS[9].style.zIndex = popup.IDS[0].style.zIndex+1000;
       popup.IDS[9].style.display = "block";
       popup.IDS[9].style.zIndex += 10000;
       popup.IDS[0].style.zIndex += 100001;
    
}

function ATB_setTitleGradient(id, firstColor, lastColor)
{
  var titlebar = document.getElementById(id + '_title');
  if (titlebar != null)
  {
	  titlebar.style.filter = "progid:DXImageTransform.Microsoft.Gradient(endColorstr='" + lastColor + "', startColorstr='" + firstColor + "', gradientType='1')";
  }
}

document.onmousemove = ATB_mousemouve;
document.onmouseup = new Function("ATB_currentResize = null");

function ATB_dockmousedown(id)
{
 ATB_currentTool = document.getElementById(id); ATB_currentTool.style.zIndex = 999;
     
 direction = document.getElementById(id + '_direction').value;    
      
 ATB_xoff = ATB_currentTool.clientLeft; 
 ATB_yoff = ATB_currentTool.clientTop;
 
 if (direction == 'Horizontal')
 {
    ATB_yoff -= ATB_currentTool.clientHeight/2; 
 }
 
 else
 {
   ATB_xoff -= ATB_currentTool.clientWidth/2;
 }
  
 var containers = document.getElementById("Containers").value; 
 if (containers != null && containers != "")
 {
	var reg=new RegExp('[,]+', 'g');
	var containerItems = containers.split(reg); 
	for (i = 0 ; i < containerItems.length ; i++)
	{
		var containerToolbars = document.getElementById(containerItems[i] + '_toolbars').value; 
	    
		if (containerToolbars.indexOf(id) >= 0)
		{
		var newContainerToolbars = ATB_removeSubString(containerToolbars,id);
		document.getElementById(containerItems[i] + '_toolbars').value = newContainerToolbars;
	       
		var totHeight = 0; 
		var container = document.getElementById(containerItems[i]);
		if (newContainerToolbars != "")
		{
			var values = newContainerToolbars.split(reg);
     		if (values.length > 0)
    		{
    			var top = parseInt(container.style.top) + parseInt(container.style.borderWidth);
	   	   
   				for (i = 0 ; i < values.length ; i++)
   				{
   	   				var currentToolbar = document.getElementById(values[i]); 
   	   				currentToolbar.style.top = top;
   	   				totHeight += parseInt(currentToolbar.clientHeight);
   	   				top += parseInt(currentToolbar.clientHeight);
      			} 
			}
		}
		container.style.height = totHeight + parseInt(container.style.borderWidth)*2;
		}
	}
  }
}

function ATB_dockmouseup(id)
{
  
  if (ATB_currentContainer != null)
  {
     ATB_addToolToContainer(ATB_currentContainer.id, ATB_currentTool.id);
     ATB_currentContainer = null;
  }
  if (ATB_currentTool.style != null && ATB_currentTool.style.zIndex != null)
     ATB_currentTool.style.zIndex = 1;
     
  document.getElementById(ATB_currentTool.id + '_left').value = ATB_currentTool.style.left;
  document.getElementById(ATB_currentTool.id + '_top').value = ATB_currentTool.style.top;
       
  ATB_currentTool = null;
}

function ATB_addToolToContainer(idContainer, idToolbar)
{
     var totHeight = 0; 
     var container = document.getElementById(idContainer);
     var containerToolbars = document.getElementById(idContainer + '_toolbars').value;
     var tool = document.getElementById(idToolbar);
     
     var top = parseInt(container.style.top);
     
     if (containerToolbars != "")
     {
     	if (containerToolbars.indexOf(idToolbar) != -1) return ;
     	
        var reg=new RegExp('[,]+', 'g');
     	var values = containerToolbars.split(reg); 
     	if (values.length > 0)
     	{
     	    for (i = 0 ; i < values.length ; i++)
     	    {
     	       top += parseInt(document.getElementById(values[i]).clientHeight);
     	       totHeight += parseInt(document.getElementById(values[i]).clientHeight);
     	    }
     	}
     }
     
     tool.style.top = top + parseInt(container.style.borderWidth);
     tool.style.left = parseInt(container.style.left) + parseInt(container.style.borderWidth);
     document.getElementById(idContainer + '_toolbars').value += "," + idToolbar;
     totHeight += tool.clientHeight;
     
     container.style.height = totHeight + parseInt(container.style.borderWidth)*2;
     var backgroundColor = document.getElementById(idContainer + '_backColor'); 
     if (backgroundColor != null)
		container.style.backgroundColor = backgroundColor.value;
	else 
		container.style.backgroundColor = 'transparent';
}

function ATB_removeSubString(s,stringToRemove)
{
	var reg=new RegExp('[,]+', 'g');
	var tab = s.split(reg);
	var res = '';
	for (var i=0; i<tab.length; i++) 
	{
		if (tab[i] != stringToRemove)
		{
			res = res.concat(tab[i]);
			res = res.concat(',');
		}
	}

	return res;
}
 
function ATB_RegisterEvent(szEventName, pEventHandler)
{
	// get the existing registered handler
	var handler1 = eval("this." + szEventName);

	if(typeof(attachEvent) != "undefined")
	{
		// this is IE 5.0 or later. Cooperative Registration is easy
		eval("this.attachEvent('" + szEventName + "', " + pEventHandler + ");");
		return;
	}
	else if(typeof(addEventListener) != "undefined")
	{
		// this is Netscape 6.0 or later. Cooperative Registration is easy
		eval("this.addEventListener('" + (szEventName.substring(2)) + "', " + pEventHandler + ", false);");
		return;
	}
	// Custom event Registration layer required for cooperation, but first
	// check if there is an existing function attached to the event.
	else if(!handler1)
	{
		// This will be the first function attached to the event
		// Registration is easy and does not need to cooperate
		eval("this." + szEventName + " = pEventHandler");
		return;
	}
	// There is an existing function, we need it as a string.
	else
	{
		handler1 = handler1.toString();
	}

	// Get the new function to attach as a string.
	var handler2 = pEventHandler.toString();

	// Combine the 2 functions into 1 new function.
	var ev;
	ev=handler1.substring(handler1.indexOf("{") + 1, handler1.lastIndexOf("}")) + handler2.substring(handler2.indexOf("{") + 1, handler2.lastIndexOf("}"));
	ev=new Function(ev);

	// Assign the new function to the event.
	eval("this." + szEventName + " = ev");
}

if(typeof(window.RegisterEvent) == "undefined")
	window.RegisterEvent = ATB_RegisterEvent;
if(typeof(document.RegisterEvent) == "undefined")
	document.RegisterEvent = ATB_RegisterEvent; 

function ATB_swap(img, fname) 
{
	document[img].src = fname;
}

function ATB_findPosX(obj)
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
	{
		curleft += obj.x; 
	}
	return curleft; 
}

function ATB_findPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		//alert(obj.tagName + ':' + obj.id + ':' + obj.clientTop + ':' + obj.offsetTop + ':' + obj.y + ':' + obj.scrollTop);
		//alert(obj.id + ':' + obj.style.top + ':' + obj.scrollTop + ':' +obj.offsetTop + ':' + obj.clientTop);
		
		while (obj.offsetParent)
		{
			if (obj.tagName != 'DIV')
			{
			   //alert(obj.offsetTop + ':' +obj.tagName + ':' + obj.id);
			   alert(obj.tagName + ':' + obj.id + ':' + obj.style.top + ':' + obj.scrollTop + ':' +obj.offsetTop + ':' + obj.clientTop);
			   curtop += obj.offsetTop;
			}
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)curtop += obj.y;
	//alert('final:' + curtop);
	return curtop;
}

function ATB_changeSelectedIndex(id, newIndex, close)
{
	document.getElementById(id + '_selectedIndex').value = newIndex;
	if (close == 'True')
		ATB_closeDropDownList(id);
	
	if  (document.getElementById(id + '_changeToSelectedText').value == "Text")
		document.getElementById(id + '_text').innerHTML = document.getElementById(id + '_item' + newIndex).innerHTML;
		
	else if (document.getElementById(id + '_changeToSelectedText').value == "Value")
		document.getElementById(id + '_text').innerHTML = document.getElementById(id + '_item' + newIndex).value;
	
	if (document.getElementById(id + '_doPostBackWhenClick').value == "True")
		__doPostBack(id,'');
		
	//clientSideOnClick(id);
}

function ATB_getSelectedIndex(id)
{
	return document.getElementById(id + '_selectedIndex').value;
}


function ATB_getSelectedText(id)
{
	return document.getElementById(id + '_item' + ATB_getSelectedIndex(id)).innerHTML;
}

function ATB_getSelectedValue(id)
{
	return document.getElementById(id + '_item' + ATB_getSelectedIndex(id)).value;
}

function ATB_setDropDownListText(id,text)
{
   document.getElementById(id + '_text').innerHTML = text;
}

function ATB_createContainersInput()
{
   var alreadyExist = document.getElementById('Containers');
   if (alreadyExist == null)
   {
     var container = document.createElement('input');
     container.setAttribute('id','Containers'); 
     container.setAttribute('name','Containers'); 
     container.setAttribute('value',''); 
     document.body.appendChild(container);
   }
}

function ATB_addContainer(id)
{
  var containers = document.getElementById('Containers');
  if (containers != null && containers.value.indexOf(id) < 0)
  {
	  if (containers != "")
		containers.value = containers.value.concat(",");
		
      document.getElementById('Containers').value = containers.value.concat(id);
  }
}

function ATB_openDropDownList(id)
{

   if (ATB_currentOpenedDDLID != null)
   {
     ATB_closeDropDownList(ATB_currentOpenedDDLID);
   }
   ATB_currentOpenedDDLID = id;

   var ddl = document.getElementById(id + '_ddl'); 
   var items = document.getElementById(id + '_items');
   
   //if (items.style.display == 'none')
   //{
     //alert(ddl.clientTop + ':' + ddl.style.top + ':' + ddl.scrollTop + ':' + ddl.offsetTop);
	     
     items.style.position='absolute';
     //items.style.top = 2 + ATB_findPosY(ddl) + ddl.offsetHeight;
     items.style.top = 2 + ddl.offsetTop + ddl.offsetHeight;
     items.style.left = ddl.style.left;
     items.style.width =ddl.offsetWidth-1;
     items.style.display = 'block';
     items.style.zIndex += 100000;
   
     var mask = document.getElementById(id + '_mask');
     mask.style.width = items.offsetWidth;
     mask.style.height = items.offsetHeight;
     mask.style.top = items.style.top;
     mask.style.left = items.style.left;
     mask.style.zIndex = items.style.zIndex-1;
     mask.style.display = 'block';
   //}
}

function ATB_closeDropDownList(id)
{
  var items = document.getElementById(id + '_items');
  if (items.style.display == 'block' || items.style.display == '')
  {
    items.style.display = 'none';
    items.style.zIndex -= 100000;
    var mask = document.getElementById(id + '_mask');
    mask.style.display = 'none';
  }
  
}

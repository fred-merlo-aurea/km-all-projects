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
var ATB_leftFilePicker = 10;
var ATB_topFilePicker = 356;
var ATB_zIndexTool = 0;
var ATB_timer = null;
var scrX = 0;
var scrY = 0;
var ATB_backup0nmousemove = '';
var ATB_backup0nmouseup = '';

function ATB_testIfScriptPresent()
{
}

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

function ATB_isShown(id)
{
	var popup = document.getElementById(id + '_window');
	if (popup.style.display == 'block')
		return true;
	else
		return false;
}

function ATB_hidePopup(id)
{
    document.getElementById(id + '_window').style.display = 'none';
    document.getElementById(id + '_shadow').style.display = 'none';
    document.getElementById(id + '_iframe').style.display = 'none';
    
    if (document.getElementById(id + '_window').IDS[10] != '')
	{
			var filepicker = document.getElementById(document.getElementById(id + '_window').IDS[10]);
			filepicker.style.display = 'none';
	}
}

function ATB_showPopup(id)
{
	if (document.getElementById(id + '_window') != null)
	{
	  var window = document.getElementById(id + '_window').style;
      var shadow = document.getElementById(id + '_shadow').style;
     
      if (window.display != 'block')
      {
		window.display = 'block';
		shadow.display = 'block';
		shadow.zIndex = ++ATB_zindex;
		window.zIndex = ++ATB_zindex;
	      
	    
		if (document.getElementById(id + '_window').IDS[10] != '')
		{
			var filepicker = document.getElementById(document.getElementById(id + '_window').IDS[10]);
			filepicker.style.top = parseFloat(window.top) + parseFloat(ATB_topFilePicker) + 'px';
			filepicker.style.left = parseInt(window.left) + parseFloat(ATB_leftFilePicker) + 'px';
			filepicker.style.display = '';
		}
      
      }
      
      ATB_setPopupActive(document.getElementById(id + '_window'));
    }
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

function ATB_setContent(id, text)
{
	document.getElementById(id+ '_contents').innerHTML = text;
}

function ATB_setOpacity(id, value) 
{
	var window = document.getElementById(id + '_window');
	window.style.filter = 'alpha(opacity='+ value +')';
	window.style.opacity = '.' + value +'';   

	var iframe = document.getElementById(id + '_iframe');
	iframe.style.filter = 'alpha(opacity=' + value + ')';
	iframe.style.opacity = '.' + value + '';
  
  ATB_resizeIframe(id);
}

function ATB_setSize(id, width, height)
{
	width = Math.max(parseFloat(width), 100);
	height = Math.max(parseFloat(height), 80);

	var window = document.getElementById(id + '_window');
	window.style.width = width + 'px';
	window.style.height = height + 'px';		
	
	var shadow = document.getElementById(id + '_shadow');
	shadow.style.width = (ATB_ie5) ? width : width + 4 + 'px';
	shadow.style.height = (ATB_ie5) ? height : height + 6 + 'px';
	
	var title = document.getElementById(id + '_title');
	title.style.width = (ATB_ie5) ? width - 8 : width - 5 + 'px';
	
	var button = document.getElementById(id + '_button');
	button.style.left = parseInt(title.style.width) - 20 + 'px';
	
	var contents = document.getElementById(id + '_contents');
	contents.style.width = (ATB_ie5) ? width - 7 : width - 13 + 'px';
	contents.style.height = (ATB_ie5) ? height - 36 : height - 36 + 'px';	
	
	if (document.getElementById(id + '_isTitleShowed').value == 'False')
	{
		window.style.height = parseInt(window.style.height) - 16 + 'px';
	}
	
	ATB_resizeIframe(id);
}

function ATB_dragContent(id,drag)
{
	content = document.getElementById(id + '_contents');
	if (content != null)
	{
		if (drag == 'True')
		{
			content.onmousedown = ATB_mousedown;
			content.onmouseup = ATB_mouseup;

		}
		else
		{
			content.onmousedown = null;
			content.onmouseup = null;
		}
	}
}

function ATB_showTitle(id,show)
{
	var isTitleShowed = document.getElementById(id + '_isTitleShowed'); 
	var isWindowShowed = document.getElementById(id + '_isWindowShowed');
	var isTitleMustBeShowedOnWindowShow = document.getElementById(id + '_isTitleMustBeShowedOnWindowShow');
		
	if (show == 'True')
	{
		if (isTitleShowed.value == 'False' && isWindowShowed.value == 'True')
		{
	
			var title = document.getElementById(id + '_title');
			title.style.visibility = 'visible';
	
			var contents = document.getElementById(id + '_contents');
			//contents.style.top = 24;
			contents.style.top = 4 + parseInt(title.clientHeight) + 'px';
	
			var window = document.getElementById(id + '_window');
			//window.style.height = parseInt(window.style.height) + 16;
			window.style.height = parseInt(window.style.height) + parseInt(title.clientHeight) + 'px';
			
			document.getElementById(id + '_isTitleShowed').value = 'True';
			
			ATB_resizeIframe(id);
		}
	}
	
	else
	{
		if (isTitleShowed.value == 'True')
		{
	
			var title = document.getElementById(id + '_title');
			title.style.visibility = 'hidden';
	
			var contents = document.getElementById(id + '_contents');
			contents.style.top = 3 + 'px';
	
			var window = document.getElementById(id + '_window');
			//window.style.height = parseInt(window.style.height) - 16;
			window.style.height = parseInt(window.style.height) - parseInt(title.clientHeight) + 'px';
			
			document.getElementById(id + '_isTitleShowed').value = 'False';
			
			
			ATB_resizeIframe(id);
		}
	}
	
}

function ATB_showWindow(id,show)
{
	var isWindowShowed = document.getElementById(id + '_isWindowShowed');
	var isTitleShowed = document.getElementById(id + '_isTitleShowed'); 
	var isTitleMustBeShowedOnWindowShow = document.getElementById(id + '_isTitleMustBeShowedOnWindowShow');
	
	if (show == 'True')
	{
		if (isWindowShowed.value == 'False')
		{

			var window = document.getElementById(id + '_window');
			window.style.visibility = 'visible';
		
			document.getElementById(id + '_isWindowShowed').value = 'True';
			
			if (isTitleMustBeShowedOnWindowShow.value == 'True')
				ATB_showTitle(id,show);
			
			ATB_resizeIframe(id);
		}
	}
	
	else
	{
		if (isWindowShowed.value == 'True')
		{
		
			if (isTitleShowed.value == 'True')
				isTitleMustBeShowedOnWindowShow.value = 'True';
			else
				isTitleMustBeShowedOnWindowShow.value = 'False';
				
			ATB_showTitle(id,show);	
	
			var window = document.getElementById(id + '_window');
			window.style.visibility = 'hidden';
			
			document.getElementById(id + '_isWindowShowed').value = 'False';
			
			ATB_resizeIframe(id);
		}
	}
	
}

function ATB_resizeIframe(id)
{
	var window = document.getElementById(id + '_window');
	var titleShowed = document.getElementById(id + '_isTitleShowed'); 
	var windowShowed = document.getElementById(id + '_isWindowShowed');
	var shadowed =  document.getElementById(id + '_isShadowed');
			
	var shadow = 0;
	if (shadowed.value == 'True')
		shadow = 0;
	else
		shadow = 0;
			
		
	if (window != null && titleShowed != null && windowShowed != null)
	{
	
		if (titleShowed.value == 'False' && windowShowed.value == 'True')
		{
	
			window.IDS[9].style.width = parseFloat(window.IDS[0].offsetWidth) + parseFloat(shadow) + 'px';
     			window.IDS[9].style.height = parseFloat(window.IDS[0].offsetHeight) + parseFloat(shadow) + 'px';
		     	window.IDS[9].style.top = parseFloat(window.IDS[0].style.top) + 'px';
			window.IDS[9].style.left = parseFloat(window.IDS[0].style.left) + 'px';
			window.IDS[9].style.display = "block";
		}
		
		else if (windowShowed.value == 'False')
		{
	
			window.IDS[9].style.width = parseFloat(window.IDS[2].offsetWidth) + parseFloat(shadow) + 'px';
     			window.IDS[9].style.height = parseFloat(window.IDS[2].offsetHeight) + parseFloat(shadow) + 'px';
     			window.IDS[9].style.top = parseInt(window.IDS[2].style.top) + parseInt(window.IDS[0].style.top) + 2 + 'px';
			window.IDS[9].style.left = parseInt(window.IDS[2].style.left) + parseInt(window.IDS[0].style.left) + 2 + 'px';
     			window.IDS[9].style.display = "block";
		}
		
		else
		{
			window.IDS[9].style.width = parseFloat(window.IDS[0].offsetWidth) + parseFloat(shadow) + 'px';
     			window.IDS[9].style.height = parseFloat(window.IDS[0].offsetHeight) + parseFloat(shadow) + 'px';
     			window.IDS[9].style.top = parseFloat(window.IDS[0].style.top) + 'px';
			window.IDS[9].style.left = parseFloat(window.IDS[0].style.left) + 'px';
      			window.IDS[9].style.display = "block";
		}
	}
}

function ATB_mousemouve(evt)
{
    //document.getElementById('_text').value = event.clientX + ':' + event.clientY; 

    ATB_mx = (ATB_ie5) ? event.clientX + document.body.scrollLeft:parseFloat(evt.pageX) + 'px';
    ATB_my = (ATB_ie5) ? event.clientY + document.body.scrollTop:parseFloat(evt.pageY) + 'px';
    
    //if(!ATB_ns6)
        ATB_moveObject();
        
    if((ATB_currentIDWindow != null) || (ATB_currentResize != null) || (ATB_currentTool != null) || (ATB_currentIDIFrame != null))
        return false;
}

function ATB_movePopupTo(id,x,y)
{

	  var popupShowed = false;
      	  var window = document.getElementById(id + '_window');
    	  var shadow = document.getElementById(id + '_shadow');
      
	  if (window.style.display == 'block')
	  {
		popupShowed = true;
		ATB_hidePopup(id);
	  }
        window.style.left = parseFloat(x) + 'px';
        shadow.style.left = parseFloat(x) + 4 + 'px';
        window.style.top = parseFloat(y) + 'px';
        shadow.style.top = parseFloat(y) + 4 + 'px';
      
      if (popupShowed == true)
		ATB_showPopup(id);
		

}

function ATB_Position(evenement)
{
	scrX = evenement.screenX;
	scrY = evenement.screenY;
}

function ATB_movePopupFrom(popupId, decalX, decalY){
	if (navigator.userAgent.indexOf('MSIE') >= 0){
		if (event.clientX < 100){
			decalX = 0;
		}
		poScrollX = document.body.scrollLeft;
		poScrollY = document.body.scrollTop;
		ATB_movePopupTo(popupId, event.clientX + poScrollX + decalX, event.clientY + poScrollY + decalY);
	} else if (navigator.userAgent.indexOf('Safari') >= 0){
		if (event.clientX < 100){
			decalX = 0;
		}
		ATB_movePopupTo(popupId, event.clientX + decalX, event.clientY + decalY);
	} else {
		document.onmousemove = ATB_Position;
		poScrollX = parseFloat(window.pageXOffset) + 'px';
		poScrollY = parseFloat(window.pageYOffset) + 'px';
		if (scrX < 100){
			decalX = 0;
		}
		//setTimeout(ATB_movePopupTo(popupId, parseFloat(scrX) + parseFloat(poScrollX) + parseFloat(decalX), parseFloat(scrY) + parseFloat(poScrollY) + parseFloat(decalY) - 130),10);
		//setInterval("ATB_movePopupTo('Popup1',20,20)",40);
		ATB_movePopupTo(popupId, parseFloat(scrX) + parseFloat(poScrollX) + parseFloat(decalX), parseFloat(scrY) + parseFloat(poScrollY) + parseFloat(decalY) - 130)
	}
}

function ATB_moveObject()
{
	if (ATB_currentTool != null)
    {
      	var x = parseFloat(ATB_mx) + 'px';
        var y = parseFloat(ATB_my) + 'px';
        
/*        ATB_currentTool.style.left = x;
        ATB_currentTool.style.top = y;
        
        return;*/
        
    	var isOverContainer = false;
    	var containers = document.getElementById("Containers").value; 
    	if (containers != null && containers != "")
    	{
    		var reg=new RegExp('[,]+', 'g');
	    	
    		var containerItems = containers.split(reg); 
    		for (i = 0 ; i < containerItems.length ; i++)
    		{
    				var container = document.getElementById(containerItems[i]); 
				var coL = parseFloat(container.offsetLeft);
				var coT = parseFloat(container.offsetTop);
				var coW = parseFloat(container.clientWidth);
				var coH = parseFloat(container.clientHeight);
			   
				var medH = ATB_currentTool.clientHeight/2;
			   
				if (x > coL && x < coL + coW)
    				if (y + medH > coT && y + medH < coT + coH)
    	   				isOverContainer = true;
		    	   	
    			if (isOverContainer == true)
				{
				    var backgroundColorDock = document.getElementById(containerItems[i] + "_backColorDock");
				    //var backgroundColorDock = eval(containerItems[i] + "_backColorDock");
				    if (backgroundColorDock != null)
						container.style.backgroundColor = backgroundColorDock.value;
					ATB_currentContainer = container; break;
				}
				else
				{   var backgroundColor = document.getElementById(containerItems[i] + "_backColor");
					 //var backgroundColor = eval(containerItems[i] + "_backColor");
					if (backgroundColor != null)
					   container.style.backgroundColor = backgroundColor.value;
					else
					   container.style.backgroundColor = "transparent";
					ATB_currentContainer = null;
				}	
   			}
   		}
    	
        if (parseInt(x) > 15)
           ATB_currentTool.style.left = parseInt(x) + 'px';
        else
           ATB_currentTool.style.left = 0 + 'px';
        
        if (parseInt(y) > 15)
        
           ATB_currentTool.style.top = parseInt(y) + 'px';
        else
           ATB_currentTool.style.top = 0 + 'px';
        
        var mask = document.getElementById(ATB_currentTool.id + '_mask');
        if (mask != null)
        {
			mask.style.left = parseFloat(ATB_currentTool.style.left) + 'px';
			mask.style.top = parseFloat(ATB_currentTool.style.top) + 'px';
				}
    }

    if (ATB_currentIDWindow != null)
    {
       var x = parseFloat(ATB_mx) + parseFloat(ATB_xoff);
       var y = parseFloat(ATB_my) + parseFloat(ATB_yoff);
       ATB_currentIDWindow.style.left = x + 'px';
       currIDShadow.style.left = x + 4 + 'px';
       ATB_currentIDWindow.style.top = y + 'px';
       currIDShadow.style.top = y + 4 + 'px';
       
       var popupId = ATB_StringReplace(ATB_currentIDWindow.id,'_window',''); 
       if (document.getElementById(popupId + '_isWindowShowed').value == 'False')
       {
        ATB_currentIDIFrame.style.left = x + parseInt(ATB_currentIDWindow.IDS[2].style.left) + 2 +'px';
       	ATB_currentIDIFrame.style.top = y + parseInt(ATB_currentIDWindow.IDS[2].style.top) + 2 +'px';
       }
       else
       {
       	ATB_currentIDIFrame.style.left = x + 'px';
       	ATB_currentIDIFrame.style.top = y + 'px';
       }
       
       if (ATB_currentIDWindow.IDS[10] != '')
       {
			var filepicker = document.getElementById(ATB_currentIDWindow.IDS[10]); 
			filepicker.style.top = parseInt(ATB_currentIDWindow.style.top) + parseFloat(ATB_topFilePicker) + 'px';
			filepicker.style.left = parseInt(ATB_currentIDWindow.style.left) + parseFloat(ATB_leftFilePicker) + 'px';
       }
    }
    
    if(ATB_currentResize != null)
    {
       var rx = parseFloat(ATB_mx) + parseFloat(ATB_rsxoff);
       var ry = parseFloat(ATB_my) + parseFloat(ATB_rsyoff);
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
       ATB_resizeIframe(c.IDS[7]);
   }
}

function ATB_startResize(evt)
{
    var ex = (ATB_ie5) ? event.clientX+document.body.scrollLeft:parseFloat(evt.pageX);
    var ey = (ATB_ie5) ? event.clientY+document.body.scrollTop:parseFloat(evt.pageY);
    
    ATB_rsxoff = parseInt(this.style.left) - ex;
    ATB_rsyoff = parseInt(this.style.top) - ey;
    ATB_currentResize = this;
    
    //if(ATB_ns6)
    //    this.IDS[2].style.overflow = 'hidden';
        
    return false;
}

function ATB_mouseup()
{
    ATB_currentIDWindow = null;
    ATB_currentIDIFrame = null;
    if (ATB_ie5)
    {
		self.resizeBy(0,1);
		self.resizeBy(0,-1);
	}
	
	if (this.IDS[11])
    {
    	ATB_setOpacity(this.IDS[7],100);
    }
}

function ATB_mousedown(evt)
{
    var ex = (ATB_ie5) ? event.clientX + document.body.scrollLeft : parseFloat(evt.pageX);
    var ey = (ATB_ie5) ? event.clientY + document.body.scrollTop : parseFloat(evt.pageY);
    
    ATB_xoff = parseInt(this.IDS[0].style.left) - ex;
    ATB_yoff = parseInt(this.IDS[0].style.top) - ey;
    
    ATB_currentIDWindow = this.IDS[0];
    ATB_currentIDIFrame = this.IDS[9];    
    currIDShadow = this.IDS[3];
    
    if (this.IDS[11])
    {
    	ATB_setOpacity(this.IDS[7],20);
    }
    
    return false;
}

function ATB_createDiv(x,y,width,height,bgc,id)
{
   var div = document.createElement('div');
   div.setAttribute('id',id); 
   div.style.position = 'absolute';
   div.style.left = parseFloat(x) + 'px';
   div.style.top = parseFloat(y) + 'px';
   if (width != null)
	div.style.width = parseFloat(width) + 'px';
   if (height != null)	
	div.style.height = parseFloat(height) + 'px';
   div.style.backgroundColor = bgc;
   div.style.visibility = 'visible';
   div.style.padding = '0px 0px 0px 0px';
   return div;
}

function ATB_createPopup(id,x,y,width,height,title,text,winbgcolor,winbordercolor,winborderstyle,winborderwidth,titlebgcolor,inactivetitlebgcolor,titlebordercolor,titleborderstyle,titleborderwidth,titleforecolor,contentbgcolor,contentbordercolor,contentborderstyle,contentborderwidth,scrollcolor,dragable,showed,overflow)
{
	ATB_createPopup(id,x,y,width,height,title,text,winbgcolor,winbordercolor,winborderstyle,winborderwidth,titlebgcolor,inactivetitlebgcolor,titlebordercolor,titleborderstyle,titleborderwidth,titleforecolor,contentbgcolor,contentbordercolor,contentborderstyle,contentborderwidth,scrollcolor,dragable,showed,overflow,'False','False','False');
}

function ATB_createPopup(id,x,y,width,height,title,text,winbgcolor,winbordercolor,winborderstyle,winborderwidth,titlebgcolor,inactivetitlebgcolor,titlebordercolor,titleborderstyle,titleborderwidth,titleforecolor,contentbgcolor,contentbordercolor,contentborderstyle,contentborderwidth,scrollcolor,dragable,showed,overflow,enableSsl,allowResize, showShadow)
{
  if (document.getElementById(id + '_window') != null)
	return;
  
   var isTitleShowed = document.createElement("input");
   isTitleShowed.setAttribute("type","hidden");
   isTitleShowed.setAttribute("name", id + "_isTitleShowed");
   isTitleShowed.setAttribute("id", id + "_isTitleShowed");
   isTitleShowed.setAttribute("value", "True");
   document.body.appendChild(isTitleShowed);
  
   var isWindowShowed = document.createElement("input");
   isWindowShowed.setAttribute("type","hidden");
   isWindowShowed.setAttribute("name", id + "_isWindowShowed");
   isWindowShowed.setAttribute("id", id + "_isWindowShowed");
   isWindowShowed.setAttribute("value", "True");
   document.body.appendChild(isWindowShowed);
   
   var isWindowShowed = document.createElement("input");
   isWindowShowed.setAttribute("type","hidden");
   isWindowShowed.setAttribute("name", id + "_isTitleMustBeShowedOnWindowShow");
   isWindowShowed.setAttribute("id", id + "_isTitleMustBeShowedOnWindowShow");
   isWindowShowed.setAttribute("value", "True");
   document.body.appendChild(isWindowShowed);
   
    var isShadowed = document.createElement("input");
   isShadowed.setAttribute("type","hidden");
   isShadowed.setAttribute("name", id + "_isShadowed");
   isShadowed.setAttribute("id", id + "_isShadowed");
   isShadowed.setAttribute("value", showShadow);
   document.body.appendChild(isShadowed);
  
   var tw, th; 
   width = Math.max(width,100);
   height = Math.max(height,80);
   var rdiv=new ATB_createDiv(width - ((ATB_ie5) ? 12 : 8), height - ((ATB_ie5) ? 12 : 8), 7, 7, '', id + '_resize');
   if (allowResize == 'True')
   {
	 	rdiv.innerHTML='<img src="resize.gif" width="7" height="7">';
	 	rdiv.style.cursor='se-resize';
   }
	 
   tw = (ATB_ie5) ? width : width + 4;
   th = (ATB_ie5) ? height : height + 6;
   var shadow = new ATB_createDiv(x + 4, y + 4, tw, th, "black", id + '_shadow');
       
   if(ATB_ie5)
      shadow.style.filter = "alpha(opacity = 50)";
   else 
      shadow.style.MozOpacity = .5;
      
   if (showShadow == 'False')
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
      
   tw=(ATB_ie5) ? width - 7 : width - 5;
   th=(ATB_ie5) ? height + 4 : height - 4;
   var titlebar = new ATB_createDiv(2,1,tw,null,titlebgcolor,id + '_title');	
   if (titleborderstyle.toUpperCase() != "NOTSET")
   {
    titlebar.style.borderColor = titlebordercolor;
	titlebar.style.borderStyle = titleborderstyle;
	titlebar.style.borderWidth = titleborderwidth + 'px';
   }
   if (overflow != undefined)
       titlebar.style.overflow = overflow;
   titlebar.style.cursor = "default";
   titlebar.style.visibility = 'visible';          
   
   var titleContents = '<table border=0 width=100%>';
   titleContents += '<tr>';
   titleContents += '<td>';
   titleContents += '<span id="'+id+'_titleText" style="color:'+ titleforecolor + ';">' + title + '</span>';
   titleContents += '</td>';
   titleContents += '<td align=right valign=top>';
   titleContents += '<span id="'+id+'_button"><img src="resources/images/tools/close.gif" width="16" height="16" id="' + id + '_close"></span>';
   titleContents += '</td>';
   
   titlebar.innerHTML = titleContents;
   
   tw = (ATB_ie5) ? width - 10 : width - 13;
   th=(ATB_ie5) ? height - 30 : height - 30;
   var contents = new ATB_createDiv(0,24,tw,th,contentbgcolor,id + '_contents');
   if (contentborderstyle.toUpperCase() != "NOTSET")
   {
	contents.style.borderColor = contentbordercolor;
	contents.style.borderStyle = contentborderstyle;
	contents.style.borderWidth = contentborderwidth;
   }
   if (overflow != undefined)
       contents.style.overflow = overflow;
   contents.style.padding = "0px 2px 0px 4px";

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
   if (enableSsl == 'True')
   iframe.src = 'blank.htm';
   
   document.body.appendChild(iframe);
   document.body.appendChild(shadow);
   document.body.appendChild(outerdiv);
   
   ATB_autoUpdatePopupSize(id);   
   /*contents.style.top = 4 + parseInt(titlebar.clientHeight);   
   contents.style.height = parseInt(outerdiv.clientHeight) - parseInt(titlebar.clientHeight) - 8; */
      
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
   IDS[10] = '';
   IDS[11] = false;
   
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
   
   if (allowResize == 'True')
   {
   	this.IDResize.onmousedown=ATB_startResize;
 	this.IDResize.onmouseup=new Function("ATB_currentResize=null");
   }
   
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
       
   if (ATB_ie5)
   {
		this.IDWindow.onmousedown = function()
		{
				ATB_setPopupActive(this);
		}
   }
   
   //this.IDWindow.onselectstart = new Function('return false');
    
   if(dragable == 'True')
   {
      this.IDTitle.onmousedown = ATB_mousedown;
      this.IDTitle.onmouseup = ATB_mouseup;
   }
      
   //if(ATB_ns6)setInterval('ATB_moveObject()',40);

/* document.onmousemove = ATB_mousemouve;
   document.onmouseup = new Function("ATB_currentResize = null"); */
   
   if (showed == 'False')
   {
     ATB_hidePopup(id);
   }

	ATB_resizeIframe(id);
}



function ATB_setCloseImage(id,image)
{
	var close = document.getElementById(id + '_close');
	if (close != null)
		close.src = image;
}

function ATB_setResizeImage(id,image)
{
	var resize = document.getElementById(id + '_resize');
	if (resize != null)
	{
		resize.innerHTML='<img src="' + image + '" width="7" height="7">';
	 	resize.style.cursor='se-resize';
	}
}

function ATB_setTitleFont(popupid, fontFamily, fontSize,bold, italic, textDecoration)
{
   var titleText = document.getElementById(popupid + '_titleText');	
   if (titleText != null)
   {
  	ATB_setFontObject(titleText,fontFamily,fontSize,bold,italic,textDecoration);	
   	ATB_autoUpdatePopupSize(popupid);
   }
}

function ATB_setContentFont(popupid,fontFamily, fontSize,bold, italic, textDecoration)
{
	var contents = document.getElementById(popupid + '_contents');
	if (contents != null)
	{
		ATB_setFontObject(contents,fontFamily,fontSize,bold,italic,textDecoration);	
	}
}

function ATB_setFontObject(object,fontFamily, fontSize,bold, italic, textDecoration)
{
  	if (fontFamily != null)
   		object.style.fontFamily = fontFamily;
   	if (fontSize != null)
   		object.style.fontSize = fontSize;
   	if (bold != null)
   	{
   		if (bold.toUpperCase() == 'TRUE')
   			object.style.fontWeight = 'bold';
   		else
   			object.style.fontWeight = 'normal';
   	}
   	
   	if (italic != null)
   	{
   		if (italic.toUpperCase() == 'TRUE')
   			object.style.fontStyle = 'italic';
   		else
   			object.style.fontStyle = 'normal';
   	}
   		
   	if (textDecoration != null)
   	{
   		object.style.textDecoration = textDecoration;
   	}	
}

function ATB_autoUpdatePopupSize(popupid)
{
	var contents = document.getElementById(popupid + '_contents');
	var titlebar = document.getElementById(popupid + '_title');
	var window = document.getElementById(popupid + '_window');
	
	contents.style.top = 4 + parseInt(titlebar.clientHeight) + 'px';   
   	contents.style.height = parseInt(window.clientHeight) - parseInt(titlebar.clientHeight) - 8 + 'px';
}

function ATB_SetIsImageLibrary(popupid,filepickerid)
{
	var popup = document.getElementById(popupid + '_window');
	popup.IDS[10] = filepickerid;
}

function ATB_setPopupActive(popup)
{
	
       if(ATB_oldActive!=null)
       {
          ATB_oldActive.IDS[1].style.backgroundColor=ATB_oldActive.inactivecolor;
          popup.IDS[0].style.zIndex -= 10000;
       }
         
       // Forcait le overflow auto sur netscape meme si on mettait hidden
       //if(ATB_ns6)
       //  popup.IDS[2].style.overflow='auto';
            
       ATB_oldActive=popup;
       popup.IDS[1].style.backgroundColor = popup.activecolor;
       popup.IDS[3].style.zIndex = ++ATB_zindex;
       popup.style.zIndex = ++ATB_zindex;
       popup.IDS[0].style.display = "block";
       
       /*popup.IDS[9].style.width = popup.IDS[0].offsetWidth;
       popup.IDS[9].style.height = popup.IDS[0].offsetHeight;
       popup.IDS[9].style.top = popup.IDS[0].style.top;
       popup.IDS[9].style.left = popup.IDS[0].style.left;
       popup.IDS[9].style.display = "block";*/
       
       var popupId = ATB_StringReplace(popup.id,'_window',''); 
       ATB_resizeIframe(popupId);
       popup.IDS[9].style.zIndex = popup.IDS[0].style.zIndex+1000;
       popup.IDS[9].style.display = "block";
       popup.IDS[9].style.zIndex += 10000;
       popup.IDS[0].style.zIndex += 100001;
    
}

function ATB_StringReplace(str1, str2, str3)
{
	str1 = str1.split(str2).join(str3);
	return str1;
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

function ATB_safeId(id)
{
	return ATB_stringReplace(id, ':', '_');
}

function ATB_stringReplace(str1, str2, str3)
{
	str1 = str1.split(str2).join(str3);
	return str1;
}

function ATB_dockmousedown(id)
{
	ATB_currentTool = document.getElementById(id); 
	
	if (!ATB_ie5)
	{
	ATB_backup0nmousemove = document.onmousemove;
	document.onmousemove=ATB_mousemouve;
	ATB_backup0nmouseup = document.onmouseup;
	document.onmouseup=ATB_dockmouseup;
	}
		 
	if (ATB_zIndexTool < 10000)
		ATB_zIndexTool = 10000;
	 
	ATB_currentTool.style.zIndex = ATB_zIndexTool + 10;
	    
	var mask = document.getElementById(ATB_currentTool.id + '_mask');
	mask.style.width = (ATB_ie5) ? parseFloat(ATB_currentTool.clientWidth) + 'px' : parseFloat(ATB_currentTool.clientWidth) - 3 + 'px';
	mask.style.height = (ATB_ie5) ? parseFloat(ATB_currentTool.clientHeight) + 'px' : parseFloat(ATB_currentTool.clientHeight) - 3 + 'px';
	mask.style.zIndex = ATB_currentTool.style.zIndex - 1;
	mask.style.top = parseFloat(ATB_currentTool.offsetTop) + 'px';
	mask.style.left = parseFloat(ATB_currentTool.offsetLeft) + 'px';
	mask.style.display = 'block';
	     
	//direction = document.getElementById(id + '_direction').value;    
	direction = eval(ATB_safeId(id) + '_direction');    
	      
	ATB_xoff = parseFloat(ATB_currentTool.clientLeft); 
	ATB_yoff = parseFloat(ATB_currentTool.clientTop);
	 
	if (direction == 'Horizontal')
	{
		ATB_yoff -= parseFloat(ATB_currentTool.clientHeight)/2; 
	}
	 
	else
	{
	ATB_xoff -= parseFloat(ATB_currentTool.clientWidth)/2;
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
    				var top = parseInt(container.offsetTop) + parseInt(container.style.borderWidth);
		   	   
   					for (i = 0 ; i < values.length ; i++)
   					{
   	   					var currentToolbar = document.getElementById(values[i]); 
   	   					currentToolbar.style.top = top;
   	   					totHeight += parseInt(currentToolbar.clientHeight);
   	   					top += parseInt(currentToolbar.clientHeight);
      				} 
				}
			}
			container.style.height = parseFloat(totHeight) + parseInt(container.style.borderWidth)*2 + 'px';
			}
		}
	}
	
	return false; 
}

function ATB_dockmouseup(id)
{
  if (ATB_currentContainer != null)
  {
     ATB_addToolToContainer(ATB_currentContainer.id, ATB_currentTool.id);
     ATB_currentContainer = null;
  }
  
  if (ATB_currentTool.style != null && ATB_currentTool.style.zIndex != null)
  {
	 //var mask = document.getElementById(ATB_currentTool.id + '_mask');
	 //mask.style.display = 'none';
     //ATB_currentTool.style.zIndex = 1;
  }
     
  if (ATB_currentTool != null)
  {   
  document.getElementById(ATB_currentTool.id + '_left').value = parseFloat(ATB_currentTool.style.left) + 'px';
  document.getElementById(ATB_currentTool.id + '_top').value = parseFloat(ATB_currentTool.style.top) + 'px';
  }    
  
  ATB_currentTool = null;
  
  if (!ATB_ie5)
  {
  	document.onmousemove = ATB_backup0nmousemove;
  	document.onmouseup = ATB_backup0nmouseup;
  }
}

function ATB_isInEditor(obj)
{
	if (obj.id.indexOf('__AREA__') >= 0)
		return true;

	if (obj == null || obj.parentElement == null)
		return false;
		
	else return ATB_isInEditor(obj.parentElement);
	
}

function ATB_addToolToContainer(idContainer, idToolbar)
{
return;
     var totHeight = 0; 
     var container = document.getElementById(idContainer);
     if (container != null)
     {
		if (document.getElementById(idContainer + '_toolbars') != null)
		{
		var containerToolbars = document.getElementById(idContainer + '_toolbars').value;
		var tool = document.getElementById(idToolbar);
	     
		var top = 0;
		if (ATB_isInEditor(tool) == false)
     		top = parseInt(container.offsetTop);
	     	
		var left = 0;
		if (ATB_isInEditor(tool) == false)
     		left = parseInt(container.offsetLeft);
	     
		if (containerToolbars != "")
		{
		    if (containerToolbars.indexOf(idToolbar) != -1) return ;
	     	
	     	if (containerToolbars.charAt(0) == ',')
     			        containerToolbars = containerToolbars.substr(1, containerToolbars.length - 1);
     			        
			var reg = new RegExp('[,]+', 'g');
     		var values = containerToolbars.split(reg); 
     		alert('-' + containerToolbars + '-');
     		if (values.length > 0)
     		{
     		    alert('values lengh: ' + values.length);
     		    for (i = 0 ; i < values.length ; i++)
     			{
     			    alert('-' + values[i] + '-');
     			    var reg2 =new RegExp("(,)", "g");
     			    values[i] = values[i].replace(reg2, '');
     			    
     			    alert('-' + values[i] + '-');
     			    if (values[i] != null && values[i] != '')
     			    {
     			        alert('adjusting');
     			        top += parseInt(document.getElementById(values[i]).clientHeight);
     			        totHeight += parseInt(document.getElementById(values[i]).clientHeight);
     			    }
     			}
     		}
		}
	     
		tool.style.top = top + parseInt(container.style.borderWidth);
		tool.style.left = left + parseInt(container.style.borderWidth);
		var mask = document.getElementById(tool.id + '_mask');
        if (mask != null)
        {
			mask.style.left = tool.style.left;
			mask.style.top = tool.style.top;
		}		
		
		document.getElementById(idContainer + '_toolbars').value += "," + idToolbar;
		totHeight += tool.clientHeight;
	     
		container.style.height = totHeight + parseInt(container.style.borderWidth)*2;
		//var backgroundColor = eval(idContainer + '_backColor'); 
		var backgroundColor = document.getElementById(idContainer + '_backColor'); 
		if (backgroundColor != null)
			container.style.backgroundColor = backgroundColor.value;
		else 
			container.style.backgroundColor = 'transparent';
		}
	}
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
			   curleft += parseFloat(obj.offsetLeft); 
			obj = obj.offsetParent; 
		} 
	} 
	else if (obj.x) 
	{
		curleft += parseFloat(obj.x); 
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
			   //alert(obj.tagName + ':' + obj.id + ':' + obj.style.top + ':' + obj.scrollTop + ':' +obj.offsetTop + ':' + obj.clientTop);
			   curtop += parseFloat(obj.offsetTop);
			}
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)curtop += parseFloat(obj.y);
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

function ATB_FindPosXDiv(obj)
{	
   var curleft = 0; 
	if (obj.offsetParent) 
	{ 
		while (obj.offsetParent) 
		{ 
			if (obj.tagName.toUpperCase() != 'DIV')
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

function ATB_openDropDownList(id)
{
   if (ATB_currentOpenedDDLID != null)
   {
     ATB_closeDropDownList(ATB_currentOpenedDDLID);
   }
   ATB_currentOpenedDDLID = id;

   var ddl = document.getElementById(id + '_ddl'); 
   var items = document.getElementById(id + '_items');
   var divItems = document.getElementById(id + '_divItems');
   
   items.style.position='absolute';
   //items.style.top = 2 + ATB_findPosY(ddl) + ddl.offsetHeight;
   
   items.style.display = 'block';
   
   var adjustDropDownItemsArea = document.getElementById(id + '_ItemsAreaHeightToAdjust'); 
   if (adjustDropDownItemsArea != null && adjustDropDownItemsArea.value.toUpperCase() == 'TRUE')
	 ATB_adjustDropDownItemsAreaHeight(id);

   if (ATB_ie5)
      items.style.top = 2 + ddl.offsetTop + ddl.offsetHeight;
   else
   {
      var parent = ddl.offsetParent;
      if (parent != null && parent.id.indexOf('_isToolbar') != -1)
      	items.style.top = 1 + parseInt(parent.offsetTop) + parseInt(ddl.offsetHeight) + 'px'; 
      else
      {
         var coords = ATB_GetPageCoords(ddl);
         items.style.top = 1 + parseInt(ddl.offsetHeight) + parseInt(coords.y) + 'px';
      }
   }
   if (ATB_ie5)
   {
   	items.style.left = ddl.style.left;
   }
  	else
  	{
  		//items.style.left = ATB_FindPosXDiv(ddl) + 'px'; //ddl.offsetLeft;
  		items.style.left = ddl.style.left;
  	}
   if (ddl.offsetWidth > items.offsetWidth)
   	items.style.width = parseInt(ddl.offsetWidth) + 'px';
   else
   	items.style.width = parseInt(items.offsetWidth) + 'px';

   items.style.zIndex = items.style.zIndex + 100000;

   if (ATB_ns6)
   {
     var divItems = document.getElementById(id + '_divItems'); 
     divItems.style.width = parseInt(items.clientWidth) + 'px';
   }
   
   var mask = document.getElementById(id + '_mask');
   if (ATB_ie5)
   {
   mask.style.width = items.offsetWidth;
   mask.style.height = items.offsetHeight;
   mask.style.top = items.style.top;
   mask.style.left = items.style.left;
   mask.style.zIndex = items.style.zIndex-1;
   mask.style.display = 'block';
   }
   else
   {
   mask.style.width = parseInt(items.offsetWidth) + 'px';
   mask.style.height = parseInt(items.offsetHeight) + 'px';
   mask.style.top = parseInt(items.style.top) + 'px';
   mask.style.left = parseInt(items.style.left) + 'px';
   mask.style.zIndex = items.style.zIndex-1;
   mask.style.display = 'block';
   }
}

function ATB_GetPageCoords(element) 
{
	var coords = {x: 0, y: 0};
	while (element) 
	{
		coords.x += parseFloat(element.offsetLeft);
		coords.y += parseFloat(element.offsetTop);
		element = element.offsetParent;
	} return coords;
}

function ATB_findPosX(obj)
{
	var curleft = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += parseFloat(obj.offsetLeft);
			obj = obj.offsetParent;
		}
	}
	else if (obj.x)
		curleft += obj.x;
	return curleft;
}

function ATB_findPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curtop += obj.offsetTop;
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)
		curtop += obj.y;
	return curtop;
}


function ATB_adjustDropDownItemsAreaHeight(dropdownid)
{
	var divItems = document.getElementById(dropdownid + '_divItems');
	var tableItems = document.getElementById(dropdownid + '_tableItems');
		
	if (divItems != null && tableItems != null)
	{
		if (parseInt(tableItems.clientHeight) < parseInt(divItems.clientHeight))
		{
			divItems.style.height = parseInt(tableItems.offsetHeight) + 'px';
		}
	}
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

function ATB_destroyContextMenu(id)
{
	if (document.getElementById(id + '_window') != null)
	{
		document.body.removeChild(document.getElementById(id + '_window'));
		document.body.removeChild(document.getElementById(id + '_iframe'));		
	}
}

function ATB_createContextMenu(id,x,y,text,winbgcolor,winbordercolor,winborderstyle,winborderwidth)
{
  if (document.getElementById(id + '_window') != null) 
	return;
	  
   var outerdiv = new ATB_createDiv(x,y,0,0,winbgcolor,id + '_window');
   if (winborderstyle.toUpperCase() != "NOTSET")
   {
	outerdiv.style.borderStyle = winborderstyle;
	outerdiv.style.borderWidth = winborderwidth + 'px';
	outerdiv.style.borderColor = winbordercolor;
   }
   outerdiv.style.zIndex = ++ATB_zindex;
   outerdiv.style.visibility = 'visible';          
   
   //outerdiv.innerHTML = '<table width=100%><tbody><tr align=\'center\'><td><table><tbody></tbody></table></td></tr></tbody></table>' ;
   outerdiv.innerHTML = text;
   var iframe = document.createElement("IFRAME");
   iframe.setAttribute("id", id + "_iframe");
   iframe.style.border = 0;
   iframe.width = 0;
   iframe.height = 0;
   iframe.style.position = "absolute";
   document.body.appendChild(iframe);
   document.body.appendChild(outerdiv); 

   var IDS = new Array();
   IDS[0] = document.getElementById(id+'_window');
   IDS[1] = id;
   IDS[2] = iframe;
   
   this.IDWindow = IDS[0]; 
   this.IDWindow.IDS = IDS;
      
   outerdiv.style.zIndex = ++ATB_zindex;
   IDS[0].style.display = "block";
   IDS[2].style.width = IDS[0].offsetWidth;
   IDS[2].style.height = IDS[0].offsetHeight;
   IDS[2].style.top = IDS[0].style.top;
   IDS[2].style.left = IDS[0].style.left;
   IDS[2].style.zIndex = IDS[0].style.zIndex+1000;
   IDS[2].style.display = "block";
   IDS[2].style.zIndex += 10000;
   IDS[0].style.zIndex += 100001;
   
   this.IDWindow.focus();
   
   /*this.IDWindow.onblur = function()
   {
	ATB_hideContextMenu(id);
   }*/
   document.onmouseup = function()
   {
	ATB_hideContextMenu(id);
   }
}

function ATB_moveContextMenuTo(id,x,y)
{
      var window = document.getElementById(id + '_window');
      var iframe = document.getElementById(id + '_iframe');
      window.style.left = x;
      iframe.style.left = x;
      window.style.top = y;
      iframe.style.top = y;
}

function ATB_hideContextMenu(id)
{
	if (document.getElementById(id + '_window') != null)
	{
		document.getElementById(id + '_window').style.display = 'none';
		document.getElementById(id + '_iframe').style.display = 'none';
	}
}

function ATB_showContextMenu(id)
{
	  var window = document.getElementById(id + '_window').style;
      var iframe = document.getElementById(id + '_iframe').style;
     
      if (window.display != 'block')
      {
      window.display = 'block';
      iframe.display = 'block';
      iframe.zIndex = ++ATB_zindex;
      window.zIndex = ++ATB_zindex;
      }
}

function ATB_setContentFromDiv(popupid,divid)
{
	var content = document.getElementById(popupid + '_contents');
	var div = document.getElementById(divid);
	
	if (content != null && div != null)
	{
		content.innerHTML = div.innerHTML;	
		div.outerHTML = '';
	}
}

function ATB_setToolbarAbsolute(toolbarid)
{
	toolbar = document.getElementById(toolbarid);
	
	if (toolbar != null)
	{
		toolbar.style.position = 'absolute';
	}
}

function ATB_changeStyle(id,backColor,backImage,borderColor,borderStyle,borderWidth,margin)
{
	var object = document.getElementById(id);
	if (object != null)
	{
		if (backColor != null)
			object.style.backgroundColor = backColor;
			
		if (backImage != null)
			object.style.backgroundImage = backImage;
			
		if (borderColor != null)
			object.style.borderColor = borderColor;
			
		if (borderStyle != null && borderStyle.toUpperCase() != 'NOTSET')
			object.style.borderStyle = borderStyle;
			
		if (borderWidth != null)
		{
			if (borderWidth != '')
				object.style.borderWidth = borderWidth;
			else
				object.style.borderWidth = '0px';
		}
		
		if (margin != null)
			object.style.margin = margin;
	}
		
}

function ATB_changeStyleFromid(id,styleid)
{
	//var style = document.getElementById(styleid);
	var style = null;
	
	try
	{
		style = eval(ATB_safeId(styleid)); 
	}
	catch (e) {}
	
	if (style != null)
	{
		var styletab = style.split(','); 
		if (styletab.length == 6)
		{
			ATB_changeStyle(id,styletab[0],styletab[1],styletab[2],styletab[3],styletab[4],styletab[5]);
		}
	}
}

function ATB_toolButtonMouseOver(id)
{
	if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
	{

		if (ATB_getButtonType(id).toUpperCase() == 'CHECKBOX')
		{
			if (ATB_getChecked(id).toUpperCase() == 'FALSE')
			{
				ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOver');	
			}
		}
		
		else
		{
			if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
				ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOver');
		}
	}

}

function ATB_toolButtonMouseOut(id)
{
	if (ATB_getButtonType(id).toUpperCase() == 'CHECKBOX')
	{
		if (ATB_getChecked(id).toUpperCase() == 'FALSE')
		{
			ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOut');	
		}
	}
	
	else
	{
		ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOut');	
	}

}

function ATB_toolButtonMouseDown(id)
{
	if (ATB_getButtonType(id).toUpperCase() == 'CHECKBOX')
	{
		var groupName = ATB_getGroupName(id);
		if (groupName == '')
		{
	
			if (ATB_getChecked(id).toUpperCase() == 'FALSE')
			{
				ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseDown');
				ATB_setChecked(id,'True');
			}
			
			else
			{
				if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
					ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOver');
				else
					ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOut');
				ATB_setChecked(id,'False');
			}
		}
		else
		{
			var checkedButtonId = ATB_getGroupNameCheckedButton(groupName);			
			if (checkedButtonId != '')
			{
				ATB_changeStyleFromid(checkedButtonId + '_toolButtonTable',checkedButtonId + '_MouseOut');
				ATB_setChecked(checkedButtonId,'False');	
			}
			
			ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseDown');
			ATB_setChecked(id,'True');
			ATB_setGroupNameCheckedButton(groupName,id);
		}
	}
	
	else
	{
		ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseDown');
	}
}

function ATB_toolButtonMouseUp(id)
{
	var type = ATB_getButtonType(id);
	
	if (ATB_getButtonType(id).toUpperCase() == 'NORMAL')
	{
		if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
		{
			var mouseover = null;
			try
			{
				mouseover = eval(id + '_MouseOver');
			}
			
			catch (e) {}
						
			if (mouseover != null)
				ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOver');
			else
				ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOut');
		}
		else
		{
			ATB_changeStyleFromid(id + '_toolButtonTable',id + '_MouseOut');
		}
		
	}
}

function ATB_getButtonType(id)
{
	//var type = document.getElementById(id + '_Type'); 
	var type = null;
	try
	{
		eval(ATB_safeId(id) + '_Type');
	}
	catch (e) {}
	
	if (type != null)
		return type.value;
	else	
		return 'Normal';
}

function ATB_getChecked(id)
{
	var check = document.getElementById(id + '_Checked');
	
	if (check  != null)
	{
		if (check.value.toUpperCase() == 'TRUE')
			return 'True';
	}
	
	return 'False';
}

function ATB_setChecked(id, val)
{
	var check = document.getElementById(id + '_Checked'); 
	
	if (check != null)
	{
		check.value = val;
	}
	
}

function ATB_allowRollOver(id)
{
//var allowRollOver = document.getElementById(id + '_AllowRollOver');
	var allowRollOver = null;
	try
	{
		allowRollOver = eval(ATB_safeId(id) + '_AllowRollOver');
	}
	catch (e) {}

	if (allowRollOver != null && ATB_enableState(id).toUpperCase() == 'TRUE')
	{
		if (allowRollOver.toUpperCase() == 'TRUE')
			return 'True';
			
		else
			return 'False';
	}
	return 'False';
}

function ATB_swapButton(img, fname) 
{
	if (ATB_allowRollOver(img).toUpperCase() == 'TRUE')
	{	
		document[img].src = fname;
	}
}

function ATB_enableTransparentOnMove(id) 
{
	document.getElementById(id + '_window').IDS[11] = true;
}

function ATB_enableState(id)
{
	var enableState = null;
	try
	{
		enableState = document.getElementById(ATB_safeId(id) + '_Enabled');
	}
	catch (e) {}
	
	if (enableState != null)
	{
		if (enableState.value.toUpperCase() == 'TRUE')
			return 'True';
			
		else
			return 'False';
	}
	
	return 'False';
}

function ATB_setValueState(id,val)
{
	var enableState = null;
	try
	{
		enableState = document.getElementById(ATB_safeId(id) + '_Enabled');
	}
	catch (e) {}
	
	if (enableState != null)
	{
		enableState.value = val;
	}
}

function ATB_addGroupName(groupName)
{
	if (document.getElementById('ATB_GroupName_' + groupName) != null)
		return;
		
	var groupNameObject = document.createElement("input");
   	groupNameObject.setAttribute("type","hidden");
   	groupNameObject.setAttribute("name", "ATB_GroupName_" + groupName);
   	groupNameObject.setAttribute("id", "ATB_GroupName_" + groupName);
   	groupNameObject.setAttribute("value", "True");
   	document.body.appendChild(groupNameObject);
} 

function ATB_getGroupNameCheckedButton(groupName)
{
	var groupNameCheckedButton = document.getElementById('ATB_GroupName_' + groupName);
	
	if (groupNameCheckedButton != null)
		return groupNameCheckedButton.value;
		
	return "";
}

function ATB_setGroupNameCheckedButton(groupName,val)
{
	var groupNameCheckedButton = document.getElementById('ATB_GroupName_' + groupName);
	
	if (groupNameCheckedButton != null)
		groupNameCheckedButton.value = val;
}

function ATB_getGroupName(id)
{
	var groupName = document.getElementById(id + '_GroupName');
	if (groupName != null)
		return groupName.value;
		
	return "";
}

function ATB_toolButtonDownMouseOver(id)
{
	if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
	{

		ATB_changeStyleFromid(id + '_toolButtonDownTable',id + '_DownMouseOver');
	}

}

function ATB_toolButtonDownMouseOut(id)
{
	ATB_changeStyleFromid(id + '_toolButtonDownTable',id + '_DownMouseOut');	
}

function ATB_toolButtonDownMouseDown(id)
{
	ATB_changeStyleFromid(id + '_toolButtonDownTable',id + '_MouseDown');
}

function ATB_toolButtonDownMouseUp(id)
{
	if (ATB_allowRollOver(id).toUpperCase() == 'TRUE')
		ATB_changeStyleFromid(id + '_toolButtonDownTable',id + '_MouseOver');
	else
		ATB_changeStyleFromid(id + '_toolButtonDownTable',id + '_MouseOut');
}

function ATB_autoContent(id,val)
{
    var window = document.getElementById(id + '_window');
    var title = document.getElementById(id + '_title');
    var contents = document.getElementById(id + '_contents');
        
    if (window != null && title != null && contents != null)
    {
    
    		var ns6Width = 0;
    		if (ATB_ns6)
    		{
			//alert(contents.offsetWidth + ':' + contents.style.width + ':' + contents.scrollWidth);
			ns6Width = parseInt(contents.scrollWidth);
			ns6Height = parseInt(contents.scrollHeight);
		}
    
		if (val.toUpperCase() == 'TRUE')
   	   		contents.style.overflow = '';
		else
    			contents.style.overflow = 'auto';
	      
	      	if (ATB_ns6)
	      	{
	      		contents.style.width = 0;
    			contents.style.height = 0;
	      		window.style.width = parseInt(ns6Width) + 2;
	      		title.style.width = parseInt(ns6Width) - 2;
	      		window.style.height = parseInt(ns6Height) + parseInt(title.clientHeight) + 8;
	      	}
	      	else
	        {
			window.style.width = parseInt(contents.offsetWidth) + 6; 
			title.style.width = parseInt(contents.offsetWidth) - 2;
			window.style.height = parseInt(contents.offsetHeight) + parseInt(title.clientHeight) + 12;
		}

	}
}

function ATB_existPopup(id)
{
	if (document.getElementById(id + '_window') != null)
		return true;
	else
		return false;
}

function ATB_enableSSL(id)
{
	var iframe = document.getElementById(id + '_iframe');
	iframe.src = 'blank.htm';
}

function ATB_moveFade(id,x,y)
{
	if (ATB_timer != null)
		clearTimeout(ATB_timer);
		
	ATB_moveFadeTimer(id,x,y);
}

function ATB_moveFadeTimer(id,x,y)
{
	var popupX = parseInt(ATB_popupLeft(id));
	var popupY = parseInt(ATB_popupTop(id));
	
	if (popupX + 2 < x)
		popupX += 2;
	else
		popupX = x;
		
	if (popupY + 2 < y)
		popupY += 2;
	else
		popupY = y;
		
	if (popupX != x || popupY != y)
	{
		ATB_movePopupTo(id,popupX, popupY);	
		d = new Date();
		mill=new Date();
		var frameTime = 10;
		ATB_timer = setTimeout("ATB_moveFadeTimer('" + id + "','" + x + "','" + y + "' )", (frameTime - (mill-d)));
	}
	else
	{
		ATB_timer = null;
	}
}


function ATB_popupLeft(id)
{
    var window = document.getElementById(id + '_window');
    return window.style.left;
}

function ATB_popupTop(id)
{
    var window = document.getElementById(id + '_window');
    return window.style.top;
}

function ATB_OnColorOver(objTable,borderColor,backColor)
{
	/*ATB_SetBorderColor(objTable, '#0A246A');
	ATB_SetBackColor(objTable, '#B6BDD2');*/
	ATB_SetBorderColor(objTable, borderColor);
	ATB_SetBackColor(objTable, backColor);
}

function ATB_OnColorOff(objTable,borderColor,backColor)
{
	/*ATB_SetBorderColor(objTable, '#F9F8F7');
	ATB_SetBackColor(objTable, '#F9F8F7');*/
	ATB_SetBorderColor(objTable, borderColor);
	ATB_SetBackColor(objTable, backColor);
}

function ATB_SetBackColor(obj, color)
{
	obj.style.backgroundColor = color;
}

function ATB_SetBorderColor(obj, color)
{
	obj.style.borderColor = color;
}

function ATB_DisableButton(id)
{
	var button = document.getElementById(id + '_toolButtonTable');
	
	if (button != null)
	{
		ATB_setValueState(id,'False');
		button.style.filter = 'alpha(opacity=30)';
		button.style.opacity = '.30';
	}
}

function ATB_EnableButton(id)
{
	
	var button = document.getElementById(id + '_toolButtonTable');
	if (button != null)
	{
		ATB_setValueState(id,'True');
		button.style.filter = 'alpha(opacity=100)';
		button.style.opacity = '.100';
	}
}

function ATB_SetColorTableOnClick(id)
{
	ATB_SetSelectedColor(id,document.getElementById(id + '_Color').value);
	ATB_hidePopup(id + '_CustomColors');
}

function ATB_GetSelectedColor(id)
{
   return document.getElementById(id + '_selectedColor').value;
}

function ATB_SetSelectedColor(id,color)
{
	document.getElementById(id + '_selectedColor').value = color;
	
	var squareColor = eval(id + '_useSquareColor');	
	if (squareColor != null && squareColor.toUpperCase() == 'TRUE')
	{
		document.getElementById(id + '_squareColor').style.backgroundColor = color;
	}
	
	ATB_closeDropDownList(id);
}

function ATB_BuildColorTable(id,onClick, disableCustom,typeColor)
{
	var str, color216 = new Array('00','33','66','99','CC','FF');
	var colorLen = color216.length, color = '';
	cellWidth = 12;
	cellHeight = 12;

	str = '<table><tr><td><table width=225 cellspacing=0 cellpadding=0 onselectstart=\'return false\'>';
	
	for(var f=0;f<2;f++)
	{
		for (var r=0;r<colorLen;r++) {
			str += '<tr>';
			for (var g=colorLen-(1+(f*3));g>=3-(f*3);g--) {
				for (var b=colorLen-1;b>=0;b--) {
					
					color = color216[r]+color216[g]+color216[b];
					
					str +='<td><table width=' + cellWidth + ' height=' + cellHeight + ' cellpadding=0 cellspacing=0><tr><td style=\'cursor:hand\''
						+ ' bgcolor=\'#' + color + '\''
						+ ' title=\'#' + color + '\''
						+ ' onmouseover=\"ATB_SetValue(document.getElementById(\''+ id + '_Color\'), \'' + color + '\');ATB_SetBackColor(document.getElementById(\'' + id + '_SampleColor\'), \'#' + color + '\')\" '
						+ (onClick ? 'onclick=\"' + ATB_StringReplace(onClick, '$color$', '\'#' + color + '\'') + '\" ' : '')
						+ '></td></tr></table></td>\n';
				}
			}
			str += '</tr>';
		}
	}
	
	str += '<tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
	str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
	str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' id=\'' + id + '_Color\' maxlength=6 size=7 onkeyup=\'ATB_CustomColorKeyUp(this,\"' + id + '\");\'' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\'ATB_SetColorTableOnClick(\"' + id + '\");\'></span></td></tr></table>';
	str += '</td></tr></table></td></tr></table>';
	return str;
	
}

function ATB_BuildColorTableMozilla(id,onClick, disableCustom,typeColor,spacer)
{
	var str, color216 = new Array('00','33','66','99','CC','FF');
	var colorLen = color216.length, color = '';
	cellWidth = 12;
	cellHeight = 12;

	str = '<table><tr><td><table width=212 cellspacing=0 cellpadding=0 onselectstart=\'return false\'>';
	
	for(var f=0;f<2;f++)
	{
		for (var r=0;r<colorLen;r++) {
			str += '<tr>';
			for (var g=colorLen-(1+(f*3));g>=3-(f*3);g--) {
				for (var b=colorLen-1;b>=0;b--) {
					
					color = color216[r]+color216[g]+color216[b];
					
					str +='<td><table width=' + cellWidth + ' height=' + cellHeight + ' cellpadding=0 cellspacing=0><tr><td style=\'cursor:pointer\''
						+ ' bgcolor=\'#' + color + '\''
						+ ' title=\'#' + color + '\''
						+ ' onmouseover=\"ATB_SetValue(document.getElementById(\''+ id + '_Color\'), \'' + color + '\');ATB_SetBackColor(document.getElementById(\'' + id + '_SampleColor\'), \'#' + color + '\')\" '
						+ (onClick ? 'onclick=\"' + ATB_StringReplace(onClick, '$color$', '\'#' + color + '\'') + '\" ' : '')
						+ '><img height=10 width=10 src=\'' + spacer + '\'/></td></tr></table></td>\n';
				}
			}
			str += '</tr>';
		}
	}
	
	str += '</td></tr></table></td></tr></table>';
	str += '<table><tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
	str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
	str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' id=\'' + id + '_Color\' maxlength=6 size=7 onkeyup=\'ATB_CustomColorKeyUp(this,\"' + id + '\");\'' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\'ATB_SetColorTableOnClick(\"' + id + '\");\'></span></td></tr></table></table>';
	return str;
	
}

function ATB_SetValue(obj, value)
{
	obj.value = value;
}

function ATB_StringReplace(str1, str2, str3)
{
	str1 = str1.split(str2).join(str3);
	return str1;
}

function ATB_CustomColorKeyUp(input,id)
{
	input.value = input.value.toUpperCase();
	input.value = input.value.replace(/[^\dA-F]*/gi,"");
	if(input.value.length == 6)
		ATB_SetBackColor(document.getElementById(id + '_SampleColor'), '#' + document.getElementById(id + '_Color').value);
	else
		ATB_SetBackColor(document.getElementById(id + '_SampleColor'), '#FFFFFF');
	
}

function ATB_setZeroPaddingContents(id)
{
	var contents = document.getElementById(id + '_contents');
	
	if (contents != null)
	{
		contents.style.paddingLeft = '0px';
		contents.style.paddingRight = '0px';
		contents.style.paddingTop = '0px';
		contents.style.paddingBottom = '0px';
	}
}

function ATB_enableSelection(id,allowSelection)
{
	var popup = document.getElementById(id + '_window');
	if (popup != null)
	{
		if (allowSelection.toUpperCase() == 'FALSE')
			popup.onselectstart = new Function('return false');
		else
			popup.onselectstart = null;
	}
}

function ATB_GetXMLHttp()
{

    var xmlHttp=false;
    
    // IE
    try 
    {
        xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
    } 
    catch (errorMsxml2) 
    {
        try 
        {
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (errorMicrosoft)
        {
            xmlHttp = false;
        }
    }

    // If Mozzilla
    if (!xmlHttp && typeof XMLHttpRequest!='undefined')
    {
       xmlHttp = new XMLHttpRequest();
    }
    
    return xmlHttp;
}

function ATB_DoCallBack(editorId)
{
  var theData = '';
  var theform = document.forms[0];
  var thePage = window.location.pathname + window.location.search;
  var eName = '';

  var eventTarget = '';
  var eventArgument = '';	 

  theData  = '__EVENTTARGET='  + escape(eventTarget.split("$").join(":")) + '&';
  theData += '__EVENTARGUMENT=' + eventArgument + '&';
  theData += '__VIEWSTATE='    + escape(theform.__VIEWSTATE.value).replace(new RegExp('\\+', 'g'), '%2b') + '&';
  theData += 'HTB_IsCallBack=true&';
  theData += 'HTB_ClientId=' + '' + '&';
  theData += 'HTB_Argument=' + eventArgument + '&';
  
  var xmlHttp = ATB_GetXMLHttp();
  
   if(xmlHttp.readyState == 4 || xmlHttp.readyState == 0 )
    {
      xmlHttp.open('POST', thePage, true);
      xmlHttp.onreadystatechange = function()
      { 
      };
      xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      xmlHttp.send(theData);
    }
}
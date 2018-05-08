var AME_ie5 = (document.all) ? true : false;
var AME_ns6 = (navigator.appName == "Netscape") ? true: false;
var AME_currentItem = null;
var AME_openedSubMenu = null;
 
function AME_testIfScriptPresent()
{
}

function AME_OverMenu(obj,id)
{
    if (AME_ie5)
	    obj.style.cursor = 'hand';
	else 
	    obj.style.cursor = 'pointer';
	
	if (obj.AllowRollOver.toUpperCase() == 'TRUE')
	{
		obj.className = obj.ClassOver;
		if (obj.ImageOver != null)
		{
			var image = document.getElementById(obj.id + '_IMG');
			image.src = obj.ImageOver;
		}
	}	
	
	if (id != '')
	{
		var subMenu = document.getElementById(id);
		if (AME_ie5)
		{
			subMenu.style.left = AME_FindPosX(obj);
			subMenu.style.top = AME_FindPosY(obj) + parseInt(obj.clientHeight);
		}
		else
		{
			subMenu.style.left = parseInt(AME_FindPosX(obj)) + 'px';
			subMenu.style.top = parseInt(AME_FindPosY(obj)) + parseInt(obj.clientHeight) - 3 + 'px';
		}
		subMenu.style.display = 'block';
	}
}

function AME_OutMenuStyle(obj)
{
	if (obj.AllowRollOver.toUpperCase() == 'TRUE')
	{
		obj.className = obj.Class;
		if (obj.Image != null)
		{
			var image = document.getElementById(obj.id + '_IMG');
			image.src = obj.Image;
		}
	}
}

function AME_OutMenu(obj,id,evt)
{
	//document.getElementById('Textarea1').value += 'OUT MENU : ' + obj.id + ':' + id + '\n';

	obj.style.cursor = 'default';
	
	/*if (obj.AllowRollOver.toUpperCase() == 'TRUE')
	{
		obj.className = obj.Class;
		if (obj.Image != null)
		{
			var image = document.getElementById(obj.id + '_IMG');
			image.src = obj.Image;
		}
	}*/	
	
	if (id != '')
	{
		var subMenu = document.getElementById(id);
	
		/*document.getElementById('Textarea1').value += event.clientX + ':' + event.clientY + '\n';
		document.getElementById('Textarea1').value += subMenu.offsetLeft + ':' + subMenu.offsetTop + ':'  + (subMenu.offsetLeft + subMenu.offsetWidth) + ':' + (subMenu.offsetTop + subMenu.offsetHeight) +'\n';*/
	
		if (!AME_PointerInZone(new AME_ClPoint(subMenu.offsetLeft,subMenu.offsetTop),new AME_ClPoint((subMenu.offsetLeft + subMenu.offsetWidth),(subMenu.offsetTop + subMenu.offsetHeight)),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? event.clientY : evt.pageY))
		{
			if (subMenu.Parent.Type == 'master')
			{
				var master = subMenu.Parent;
				
				//document.getElementById('Textarea1').value += event.clientX + ':' + event.clientY + '\n';
				//document.getElementById('Textarea1').value += AME_FindPosX(master) + ':' + AME_FindPosY(master) + ':'  + (AME_FindPosX(master) + master.offsetWidth) + ':' + (AME_FindPosY(master) + master.offsetHeight) +'\n';
				
				if (!AME_PointerInZone(new AME_ClPoint(AME_FindPosX(master)+1,AME_FindPosY(master)+1),new AME_ClPoint(AME_FindPosX(master) + master.offsetWidth,AME_FindPosY(master) + master.offsetHeight),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
				{
					//document.getElementById('Textarea1').value += 'Hide\n';
					AME_OutMenuStyle(obj);
					subMenu.style.display = 'none';
					
				}
			}
			
			else
			{
				AME_OutMenuStyle(obj);
				subMenu.style.display = 'none';
			}
		}
	}
	else
	{
		AME_OutMenuStyle(obj);
	}
}

function AME_OverItem(item,evt)
{
	//document.getElementById('Textarea1').value += "OVER ITEM : " + item.id + "\n";

	AME_currentItem = item;
	
	 if (AME_ie5)
	    item.style.cursor = 'hand';
	else 
	    item.style.cursor = 'pointer';
	
	if (item.Parent.AllowRollOver.toUpperCase() == 'TRUE')
	{
		if (item.Parent.Parent.Type == 'master')
			AME_OutMenuStyle(item.Parent.Parent);
	
		item.className = item.Parent.ClassItemOver;
		if (item.ImageOver != null && item.ImageOver != '')
		{
			var img = document.getElementById(item.id + '_IMG');
			img.src = item.ImageOver;
		}
	}
	
	if (item.SubMenu != null)
	{
		if (AME_ie5)
		{
			item.SubMenu.style.left = item.Parent.offsetLeft + item.Parent.offsetWidth - 2;
			item.SubMenu.style.top = item.Parent.offsetTop + AME_FindPosY(item);
		}
		else
		{
			item.SubMenu.style.left = parseInt(item.Parent.offsetLeft) + parseInt(item.Parent.offsetWidth) + 'px';
			item.SubMenu.style.top = parseInt(item.Parent.offsetTop) + parseInt(AME_FindPosY(item)) + 'px';
		}
		item.SubMenu.style.display = 'block';
		AME_openedSubMenu = item.SubMenu;
	}
	else
	{
		AME_openedSubMenu = null;
	}
}

function AME_OutItem(item, evt)
{
	//document.getElementById('Textarea1').value += "OUT ITEM : " + item.id + "\n";

    item.style.cursor = 'default';
    	   
	if (!AME_PointerInZone(new AME_ClPoint(AME_FindPosXElemItem(item)+1,AME_FindPosYElemItem(item)+1),new AME_ClPoint(AME_FindPosXElemItem(item) + item.offsetWidth,AME_FindPosYElemItem(item) + item.offsetHeight + (AME_ie5) ? 2 : 0),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? event.clientY : evt.pageY))
	{
		 //document.getElementById('Textarea1').value += event.clientX + ':' + event.clientY + '\n';
	    //document.getElementById('Textarea1').value += (AME_FindPosXElemItem(item)+1) + ':' + (AME_FindPosYElemItem(item) + 1)  + ':' + (AME_FindPosXElemItem(item) + item.offsetWidth) + ':' + (AME_FindPosYElemItem(item) + item.offsetHeight + 2) + '\n';
		if (item.Parent.AllowRollOver.toUpperCase() == 'TRUE')
		{
			item.className = item.Parent.ClassItem;
			if (item.Image != null && item.Image != '')
			{
				var img = document.getElementById(item.id + '_IMG');
				img.src = item.Image;
			}
		}
	}
}
	
function AME_OutBlock(block,evt)
{
	// Any opened sub menu exist
	if (AME_openedSubMenu == null)
	{
		if (block.Parent.Type == 'master')
		{
			var master = block.Parent;
			
			if (block.offsetWidth > master.offsetWidth)
			{
					if (!AME_PointerInZone(new AME_ClPoint(AME_FindPosX(master),AME_FindPosY(master)),new AME_ClPoint(AME_FindPosX(master) + master.offsetWidth,AME_FindPosY(master) + master.offsetHeight),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
						if (!AME_PointerInZone(new AME_ClPoint(block.offsetLeft + 2,block.offsetTop + 2),new AME_ClPoint(block.offsetLeft + block.offsetWidth,block.offsetTop + block.offsetHeight),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
							block.style.display = 'none';
			}
			
		}
		
		else if (block.Parent.Type == 'item')
		{
			if (block.offsetHeight >= block.Parent.offsetHeight)
			{
				if (!AME_PointerInZone(new AME_ClPoint(block.Parent.Parent.offsetLeft + AME_FindPosX(block.Parent),block.Parent.Parent.offsetTop + AME_FindPosY(block.Parent)),new AME_ClPoint(AME_currentItem.offsetLeft + block.offsetLeft + AME_currentItem.offsetWidth,block.Parent.Parent.offsetTop + AME_FindPosY(block.Parent) + block.Parent.offsetHeight),(AME_ie5) ? evt.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
					if (!AME_PointerInZone(new AME_ClPoint(block.offsetLeft + 2,block.offsetTop + 2),new AME_ClPoint(block.offsetLeft + block.offsetWidth,block.offsetTop + block.offsetHeight),(AME_ie5) ? evt.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
					{
						block.style.display = 'none';
						
						//block.Parent.Parent.style.display = 'none';
						AME_RemoveParentIfNotInZone(block.Parent.Parent,evt);
					}
			}
			
			
		}
		
	}
	// an opened sub menu
	else
	{
		if (AME_currentItem.offsetHeight < AME_openedSubMenu.offsetHeight)
		{
			//document.getElementById('Textarea1').value += event.clientX + ':' + event.clientY + '\n';
			//document.getElementById('Textarea1').value += (AME_currentItem.Parent.offsetLeft + AME_FindPosX(AME_currentItem)) + ':' + (AME_currentItem.Parent.offsetTop + AME_FindPosY(AME_currentItem)) + ':' + (AME_openedSubMenu.offsetLeft + AME_openedSubMenu.offsetWidth+1) + ':' + ((AME_currentItem.Parent.offsetTop + AME_FindPosY(AME_currentItem) + AME_currentItem.offsetHeight+1)) + '\n';
			if (!AME_PointerInZone(new AME_ClPoint(AME_currentItem.Parent.offsetLeft + AME_FindPosX(AME_currentItem),AME_currentItem.Parent.offsetTop + AME_FindPosY(AME_currentItem)),new AME_ClPoint((AME_openedSubMenu.offsetLeft + AME_openedSubMenu.offsetWidth+1),(AME_currentItem.Parent.offsetTop + AME_FindPosY(AME_currentItem) + AME_currentItem.offsetHeight+2)),(AME_ie5) ? evt.clientX : evt.pageX,(AME_ie5) ? evt.clientY : evt.pageY))
			{
				AME_openedSubMenu.style.display = 'none';
				AME_openedSubMenu = null;
			}
		}
	}
}	

function AME_RemoveParentIfNotInZone(parent,evt)
{
		if (!AME_PointerInZone(new AME_ClPoint(parent.offsetLeft + 2,parent.offsetTop + 2),new AME_ClPoint(parent.offsetLeft + parent.offsetWidth,parent.offsetTop + parent.offsetHeight),(AME_ie5) ? event.clientX : evt.pageX,(AME_ie5) ? event.clientY : evt.pageY))
		{
			parent.style.display = 'none';
			if (parent.Parent.Type != 'master')
			{
				if (parent.Parent.Type == 'item')
					AME_RemoveParentIfNotInZone(parent.Parent.Parent,evt);
				else
					AME_RemoveParentIfNotInZone(parent.Parent,evt);
			}
		}
}
	
function AME_ClPoint(x,y)
{
	this.x = x;
	this.y = y;
}

function AME_PointerInZone(pTopLeft,pBottomRight,x,y)
{
	if (x > pTopLeft.x && x < pBottomRight.x)
		if (y > pTopLeft.y && y < pBottomRight.y)
			return true;
	return false;				
}

function AME_FindPosX(obj)
{	
    var mustBeReseted = false;
    if (obj.style.position == '')
    {
        obj.style.position = 'relative';
        mustBeReseted = true;
    }
    
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
	
	if (mustBeReseted)
	{
	    obj.style.position = '';
	}
	
	return curleft; 

}

function AME_FindPosXElemItem(obj)
{	
	var curleft = 0; 
	if (obj.offsetParent) 
	{ 
		while (obj.offsetParent) 
		{ 
			if (obj.tagName != 'BODY')
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

function AME_FindPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			if (obj.tagName != 'DIV')
			{
				curtop += obj.offsetTop;
			}
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)curtop += obj.y;
	return curtop;
}

function AME_FindPosYElemItem(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			if (obj.tagName != 'BODY')
			{
				curtop += obj.offsetTop;
			}
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)curtop += obj.y;
	return curtop;
}


function AME_OnClickClientSide(obj)
{
	if (obj != null && obj.OnClickClient != null && obj.OnClickClient != '')
		window.setTimeout(obj.OnClickClient,1);
	return false;
}

function AME_ClickItem(obj)
{
	AME_OnClickClientSide(obj);
}
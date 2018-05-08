//collapseArea.style.display = (collapseArea.style.display == "block") ? "none" : "block";

function APN_TestIfScriptPresent()
{
}

function APN_CollapseExpand(id)
{
	var collapseArea = document.getElementById(id + "_CollapseArea");
		   
	if (collapseArea != null)
	{
		var button = document.getElementById(id + "_Button");
		var state = document.getElementById(id + "_State");
		var title = document.getElementById(id + "_Title");
		var collapsedImage = eval(id + "_CollapsedImage");
		var expandedImage = eval(id + "_ExpandedImage");
		var titleBackCollapsed = eval(id + "_TitleBackColorCollapsed");
		var titleBackExpanded = eval(id + "_TitleBackColorExpanded");
		
		var newState = (collapseArea.style.display == "") ? "Collapsed" : "Expanded";
		state.value = newState;
		
		if (collapsedImage != '' && expandedImage != '')
			button.src = (newState == "Collapsed") ? collapsedImage : expandedImage;
			
		if (title != null)
		{
			title.style.backgroundColor = (newState == "Collapsed") ? titleBackCollapsed : titleBackExpanded;
			//title.style.cursor = 'hand';
		}
		
		if (eval(id + '_ScrollEffect').toUpperCase() == 'FALSE')
		{
			collapseArea.style.display = (newState == "Collapsed") ? "none" : "";
		}
		
		else
		{
			// expanded
			if (collapseArea.style.display == "")
			{
				if (APN_CurrentOperation == '' || APN_CurrentOperation == 'Expand')
					APN_Collapse(id);
				else
					APN_Expand(id);
			}
			
			// collapsed
			else
			{
				APN_Expand(id);
			}
		}
		
	}
}

function APN_OnTitleClientSide(id)
{
	var onTitleClickClientSide = eval(id + '_OnTitleClickClientSide'); 
	if (onTitleClickClientSide != null && onTitleClickClientSide != '')
		window.setTimeout(eval(id + '_OnTitleClickClientSide'),1);
		
	return false;
}

var APN_Opacity = 0;
var APN_CurrentOperation = '';

function APN_Collapse(id)
{
	var scroll = document.getElementById(id + '_Scroll');
	if (scroll.style.height == '')
		scroll.style.height = scroll.offsetHeight;
		
	APN_Opacity = 100;	
		
	APN_CurrentOperation = 'Collapse';	
	APN_DoCollapse(id);
}

function APN_DoCollapse(id)
{
	if (APN_CurrentOperation != 'Collapse') return;
	
	var scroll = document.getElementById(id + '_Scroll');
	var newValue = parseInt(scroll.style.height) - parseInt(eval(id + '_Frame'));
	if (scroll.clientHeight > 1 && newValue > 0)
	{
		scroll.style.height = newValue;
		scroll.scrollTop = scroll.scrollHeight; 
		if (eval(id + '_Fade').toUpperCase() == 'TRUE')
		{
			scroll.style.filter = 'alpha(opacity=' + APN_Opacity + ')';
			var fadeStep = parseInt(document.getElementById(id + '_FadeStep').value);
			APN_Opacity -= fadeStep;
			
			if (APN_Opacity < 0)
				APN_Opacity = 0;
		}
		setTimeout("APN_DoCollapse('" + id + "');",parseInt(eval(id + '_Speed')));
	}
	else
	{
		scroll.scrollTop= scroll.scrollHeight;
		var collapseArea = document.getElementById(id + '_CollapseArea');
		collapseArea.style.display = 'none';
		APN_CurrentOperation = '';
	}
}

function APN_Expand(id)
{
	var scroll = document.getElementById(id + '_Scroll');
	var collapseArea = document.getElementById(id + '_CollapseArea');
	if (collapseArea.style.display == 'none')
	{
		if (APN_CurrentOperation != 'Collapse')
		{
			scroll.style.height = 0;
			collapseArea.style.display = 'block';
		}
		
		if (eval(id + '_Fade').toUpperCase() == 'TRUE')
		{
			APN_Opacity = 0;
			scroll.style.filter = 'alpha(opacity=' + APN_Opacity + ')';
		}
	}
	
	APN_Opacity = 0;
	
	APN_CurrentOperation = 'Expand';	
	APN_DoExpand(id);
}

function APN_DoExpand(id) 
{
	if (APN_CurrentOperation != 'Expand') return;
	
	var scroll = document.getElementById(id + '_Scroll');
	
	var realSize = parseInt(document.getElementById(id + '_RealSize').value); 
	var newValue = parseInt(scroll.style.height) + parseInt(eval(id + '_Frame'));
	if (parseInt(scroll.style.height) < (realSize - 4) && newValue < realSize)
	{
	
		scroll.style.height = newValue;
		scroll.scrollTop= scroll.scrollHeight;
		if (eval(id + '_Fade').toUpperCase() == 'TRUE')
		{
			scroll.style.filter = 'alpha(opacity=' + APN_Opacity + ')';
			var fadeStep = parseInt(document.getElementById(id + '_FadeStep').value);
			APN_Opacity += fadeStep;
			
			if (APN_Opacity > 100)
				APN_Opacity = 100;
		}
		setTimeout("APN_DoExpand('" + id + "');",parseInt(eval(id + '_Speed')));
	}
	else
	{
		scroll.style.height = realSize;
		APN_CurrentOperation = '';
	}
}

function APN_RegisterEvent(szEventName, pEventHandler)
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
	window.RegisterEvent = APN_RegisterEvent;
if(typeof(document.RegisterEvent) == "undefined")
	document.RegisterEvent = APN_RegisterEvent; 

var ATV_ie = (document.all) ? true:false;

function ATV_testIfScriptPresent()
{
}

function toggleNode(node, base)
{
	nodeDiv = document.getElementById(node + '_div');
	nodeImg = document.getElementById(node + '_img');
	nodeSta = document.getElementById(node + '_expanded');

	//nodeTable = document.getElementById(node + '_table');
	//alert(nodeTable.style.background);	
	//nodeTable.style.background = '#FFEEBB';
	 
	if (nodeDiv.style.display == 'block')
	{
		nodeDiv.style.display = 'none';
		nodeImg.src = eval('atv_' + base + '_co');
		nodeSta.value = 'False';	 
	}
	else
	{
		nodeDiv.style.display = 'block';
		nodeImg.src = eval('atv_' + base + '_ex');
		nodeSta.value = 'True';
	}
}

function loadNode(tree, node, content, base,path)
{
	var xmlhttp = getXMLHttp();
	var div = document.getElementById(content + '_div');
	
	if (div.innerHTML == "")
	{
		var span = document.getElementById(content + '_text');
		var saveData = span.innerHTML;
		span.innerHTML = "Loading...";
		
		xmlhttp.open("GET", path + "?tree=" + tree + "&node=" + node, true);
		xmlhttp.onreadystatechange=function() 
		{
			if (xmlhttp.readyState==4)
			{
				span.innerHTML = saveData;
				div.innerHTML = xmlhttp.responseText;
				toggleNode(content,base);
			}
		}
		xmlhttp.send(null);
	}
	
	else
	{
		toggleNode(content,base);
	}
}

function getXMLHttp()
{
    var xmlhttp=false;
    
    try 
    {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } 
    catch (e) 
    {
        try 
        {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (E)
        {
            xmlhttp = false;
        }
    }

    // Mozilla then?
    if (!xmlhttp && typeof XMLHttpRequest!='undefined') {
       xmlhttp = new XMLHttpRequest();
    }
    
    return xmlhttp;
}

function checkChildren(name)
{
	var obj = document.getElementById(name);
	var objs = obj.parentNode.parentNode.parentNode.getElementsByTagName('INPUT');
	var index = 0;
	var realID = name.substr(0, name.length-4);

	for(index;index<objs.length;index++)
	{
		if (objs[index].name.indexOf(realID) > -1)
			objs[index].checked = obj.checked;
	}
	
}

function clickNode(id,url,target)
{
	var table = document.getElementById(id + '_table');  

	var base = id.substring(0,id.indexOf('_',0)); 
	var curSelNode = document.getElementById('atv_' + base + '_curSelNode').value;
	

	var curSelId = '';
	var curSelStyle = '';
	if (curSelNode != '')
	{
		var index = curSelNode.toString().indexOf(';',0);
		if (index != -1)
		{
			curSelId = curSelNode.substring(0,index); 
			curSelStyle = curSelNode.substring(index+1); 
			
			var cur = document.getElementById(curSelId);
			if (cur != null)
				cur.style.backgroundColor = curSelStyle;
		}
	}
	document.getElementById('atv_' + base + '_curSelNode').value = id + '_table' + ';' + table.style.backgroundColor;
	table.style.backgroundColor = '#FFEEBB';
}

function selectNode(base,id)
{
	/*var base = id.substring(0,id.indexOf('_',0)); 
	var curSelNodeID = document.getElementById('atv_' + base + '_curSelNode'); */
	//var base = id.substring(0,id.indexOf('_',0)); 
	var nodeText = document.getElementById(id + '_nodeText');  
	var selectedNode = document.getElementById(id);
	
	var curSelNodeID = document.getElementById('atv_' + base + '_curSelNode');
	var curSelNodeStyleOriginal = document.getElementById(base + '_curSelNodeStyleOriginal');
	
	if (curSelNodeID.value != '')
		copyStyle(curSelNodeStyleOriginal,document.getElementById(curSelNodeID.value + '_nodeText'));
	
	curSelNodeID.value = selectedNode.id;
	copyStyle(nodeText,curSelNodeStyleOriginal); 
	
	var nodesStyleSelected = document.getElementById(base + '_nodesStyleSelected');
	copyStyle(nodesStyleSelected,nodeText);
	
	changeStyleSelectedNode(document.getElementById(id),document.getElementById(id + '_nodeText'),curSelNodeID,document.getElementById(base + '_curSelNodeStyleOriginal'),document.getElementById(curSelNodeID.value + '_nodeText'),document.getElementById(base + '_nodesStyleSelected'));
}

function changeStyleSelectedNode(selectedNode, nodeText, curSelNodeID, curSelNodeStyleOriginal, oldSelectedNode,nodeStyleSelected)
{
	if (curSelNodeID.value != '')
		copyStyle(curSelNodeStyleOriginal,oldSelectedNode);
	curSelNodeID.value = selectedNode.id;
	copyStyle(nodeText,curSelNodeStyleOriginal);
	copyStyle(nodeStyleSelected,nodeText);
}

function copyStyle(source, dest)
{
	dest.style.cssText = '';
	dest.className = '';
	
	dest.style.backgroundColor = source.style.backgroundColor;
	dest.style.borderColor = source.style.borderColor;
	dest.style.borderStyle = source.style.borderStyle;
	dest.style.borderWidth = source.style.borderWidth;
	dest.style.color = source.style.color;
	dest.style.height = source.style.height;
	dest.style.width = source.style.width
	
	dest.style.fontFamily = source.style.fontFamily;
	dest.style.fontSize = source.style.fontSize;
	dest.style.fontStyle = source.style.fontStyle;
	dest.style.fontWeight = source.style.fontWeight;
	
	dest.className = source.className;
}

function selectedGrayed(name)
{
	var obj = document.getElementById(name);
	
	var objs = obj.parentNode.parentNode.parentNode.getElementsByTagName('INPUT');
	var index = 0;
	var realID = name.substr(0, name.length-4);
	var allChecked = true;
	var count = 0;

	if (obj.checked == true)
	{
		for(index;index<objs.length;index++)
		{
			if (objs[index].name.indexOf(realID) > -1)
			{
				if (objs[index].checked == false)
				{
					allChecked = false;
					//break;
				}
				else
					count++;
			}
		}
		
		if (!allChecked && count > 0)
		{
			if (ATV_ie)
				obj.style.filter = 'alpha(opacity=30)';
			else
				obj.style.opacity = '.30';
		}
		else
		{
			if (ATV_ie)
				obj.style.filter = 'alpha(opacity=100)';
			else
				obj.style.opacity = '1';
		}
	}
	else
	{
		if (ATV_ie)
			obj.style.filter = 'alpha(opacity=100)';
		else
			obj.style.opacity = '1';
	}
}

function ATV_RegisterEvent(szEventName, pEventHandler)
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
	window.RegisterEvent = ATV_RegisterEvent;
if(typeof(document.RegisterEvent) == "undefined")
	document.RegisterEvent = ATV_RegisterEvent; 
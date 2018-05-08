var AAS_ie = (document.all) ? true:false;
var AAS_row = -1;
var AAS_currentRow = -1;
var AAS_TopZIndex = 0;
var AAS_currentObject = null;

function AAS_TestIfScriptPresent() 
{
}

function AAS_GetXMLHttp()
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

function AAS_AutoSuggest(id) 
{
	this.id = id;
	this.xmlHttp = AAS_GetXMLHttp();
	this.topZIndex = 0;
	
	this.textbox = document.getElementById(id); 
	this.textbox.AAS_AutoSuggest = this;
	this.textbox.onkeydown = this.OnKeyDown; 
	this.textbox.onkeyup = this.OnKeyUp;
	
	if (AAS_ie)
	{
	    this.textbox.style.position = 'absolute';
	}
}

AAS_AutoSuggest.prototype.ReadyStateChange = function()
{
	switch (this.xmlHttp.readyState)
	{
		// uninitialized
		case 0:
		{
			this.OnUninitialized();
		} break;
		
		// loading
		case 1:
		{
			this.OnLoading();
		} break;
		
		// loaded
		case 2:
		{
			this.OnLoaded();
		} break;
		
		// interactive
		case 3:
		{
			this.OnInteractive();
		} break;
		
		// complete
		case 4:
		{
			 if(this.xmlHttp.status == 0 )
				this.OnAbort();
			else if( this.xmlHttp.status == 200)
			{
				this.OnComplete(this.xmlHttp.responseText, this.xmlHttp.responseXML);
			}
			else
				this.OnError(this.xmlHttp.status, this.xmlHttp.statusText, this.xmlHttp.responseText);   
		} break;
	}
}

AAS_AutoSuggest.prototype.OnUninitialized = function()
{
}

AAS_AutoSuggest.prototype.OnLoading = function()
{
}
 
AAS_AutoSuggest.prototype.OnLoaded = function()
{
}
 
AAS_AutoSuggest.prototype.OnInteractive = function()
{
}
 
AAS_AutoSuggest.prototype.OnComplete = function(responseText, responseXml)
{
	if (responseText && responseText != '')
	{
		var reg = new RegExp('[;]+', 'g');
		var values = responseText.split(reg);

		var suggest = document.getElementById(this.textbox.id + '_Suggest');
		
		try
		{
			var oldTable = document.getElementById(this.textbox.id + '_Table');
			if (oldTable)
			{
				suggest.removeChild(oldTable);
			}
		}
		catch(ex) {}
		
		var newTable = document.createElement("table");
		newTable.setAttribute("id",this.textbox.id + '_Table');
		newTable.setAttribute("cellpadding",this.SuggestCellPadding);
		newTable.setAttribute("cellspacing",this.SuggestCellSpacing);
		newTable.setAttribute("width","100%");
		newTable.setAttribute("border","0");
						
		var newTableBody = document.createElement("tbody");

		for (var i=0; i < values.length; i++) 
		{
			var newRow = document.createElement("tr");
			var newCell = document.createElement("td");
			newCell.setAttribute("id",this.textbox.id + i);
			newCell.tag = values[i];
			newCell.index = i;
			if (AAS_ie)
			{
			    newCell.onMouseMove = "AAS_row = this.index;AAS_currentObject.ChangeSelectedItem();this.style.cursor = 'hand';";
			}
			else
			{
			    newCell.addEventListener("mousemove", function (e) 
				{
					AAS_row = this.index;
					AAS_currentObject.ChangeSelectedItem();
					this.style.cursor = 'pointer';
			
				}, true);
			}
			
			if (AAS_ie)
			{
    		    newCell.onMouseOut = "AAS_currentRow = this.index;this.style.cursor = 'default';";
		    }
		    else
		    {
		        newCell.addEventListener("mouseout", function (e) 
				{
				    AAS_currentRow = this.index;
				    this.style.cursor = 'default';
				}, true);
		    }
			   
			if (AAS_ie)
			{
			    newCell.onClick = "SelectByClick(this);";
			}
			else
			{
			    newCell.addEventListener("click", function (e) 
				{
				    SelectByClick(this);
				}, true);
			}
									
			var value = ''; 
			if (this.BoldSearchPattern)
				value = AAS_BoldSearchPattern(values[i],this.textbox.value,this.IgnoreCase);
			else 
				value = values[i];
			
			if (this.NoWrap);
				newCell.noWrap = true;
			newCell.innerHTML = value;
						
			newRow.appendChild(newCell);
			newTableBody.appendChild(newRow);
		}
		
		newTable.appendChild(newTableBody);
		suggest.appendChild(newTable); 

		suggest.outerHTML = suggest.outerHTML;

		var suggestBox = document.getElementById(this.textbox.id + '_Suggest');	
		suggestBox.style.height = newTable.style.height;

		this.OpenSuggest();
	}
	
	else 
	{
		var suggest = document.getElementById(this.textbox.id + '_Suggest');
		var oldTable = document.getElementById(this.textbox.id + '_Table');
		if (oldTable)
		{
			suggest.removeChild(oldTable);
		}
		this.CloseSuggest();
	}
}

AAS_AutoSuggest.prototype.OpenSuggest = function()
{
    var suggest = document.getElementById(this.textbox.id + '_Suggest');
	suggest.style.zIndex = parseInt(suggest.style.zIndex) + 10000;
	suggest.style.zIndex = parseInt(suggest.style.zIndex) + parseInt(AAS_TopZIndex);
	this.topZIndex = AAS_TopZIndex;
	suggest.style.left = parseInt(AAS_FindPosX(this.textbox)) + 'px';
	suggest.style.top = parseInt(AAS_FindPosY(this.textbox)) + parseInt(this.textbox.clientHeight) + 5 + 'px';
	suggest.style.visibility = 'visible';
		
	if (AAS_ie)
	{
	var mask = document.getElementById(this.textbox.id + '_mask');     
	mask.style.width = parseInt(suggest.clientWidth) + 'px' ; 
	mask.style.height = parseInt(suggest.clientHeight) + 'px';
	mask.style.top = parseInt(suggest.offsetTop) + 'px';
	mask.style.left = parseInt(suggest.offsetLeft) + 'px';
	mask.style.zIndex = parseInt(suggest.style.zIndex) - 1;
	mask.style.display = 'block';
	}
	
	AAS_currentObject = this;
}

AAS_AutoSuggest.prototype.CloseSuggest = function()
{
	var suggest = document.getElementById(this.textbox.id + '_Suggest');
	suggest.style.zIndex = parseInt(suggest.style.zIndex) - 10000;
	suggest.style.zIndex = parseInt(suggest.style.zIndex) - parseInt(this.topZIndex);
	suggest.style.visibility  = 'hidden';
	
	if (AAS_ie)
	{
	var mask = document.getElementById(this.textbox.id + '_mask');
	mask.style.display = 'none';
	}
	
	AAS_currentObject = null;
}
 
AAS_AutoSuggest.prototype.OnAbort = function()
{
}
 
AAS_AutoSuggest.prototype.OnError = function(status, statusText)
{
}

AAS_AutoSuggest.prototype.DoActiveCallBack = function(eventTarget, eventArgument)
{
  var theData = '';
  var theform = document.forms[0];
  var thePage = window.location.pathname + window.location.search;
  var eName = '';
 
  theData  = '__EVENTTARGET='  + escape(eventTarget.split("$").join(":")) + '&';
  try 
  {
    theData += '__EVENTVALIDATION=' + encodeURIComponent(theform.__EVENTVALIDATION.value) + '&';
  }
  catch (e) {}
  theData += '__EVENTARGUMENT=' + eventArgument + '&';
  theData += '__VIEWSTATE='    + escape(theform.__VIEWSTATE.value).replace(new RegExp('\\+', 'g'), '%2b') + '&';
  theData += 'AAS_AutoSuggestIsCallBack=true&';
  theData += 'AAS_Target=' + escape(eventTarget.split("$").join(":")) + '&';
  
  /*for( var i=0; i<theform.elements.length; i++ )
  {
    eName = theform.elements[i].name;
    if( eName && eName != '')
    {
      if( eName == '__EVENTTARGET' || eName == '__EVENTARGUMENT' || eName == '__VIEWSTATE' )
      {
        // Do Nothing
      }
      else
      {
        theData = theData + escape(eName.split("$").join(":")) + '=' + theform.elements[i].value;
        if( i != theform.elements.length - 1 )
          theData = theData + '&';
      }
    }
  }*/
  
  if (this.PostAutoSuggestOnly)
  {
	theData += this.PostCustomElements(this.PostAutoSuggestOnly);
	theData += this.PostCustomElements(this.PostId);
  }
  
  else
  {
		for( var i=0; i<theform.elements.length; i++ )
		{
			theData += this.RetrieveState(theform.elements[i]);
		}
  }

  if( this.xmlHttp )
  {
    if( this.xmlHttp.readyState == 4 || this.xmlHttp.readyState == 0 )
    {
      theData = theData.replace(new RegExp( '\\$', 'gi' ), '_');
      
      var oThis = this;
      this.xmlHttp.open('POST', thePage, true);
      this.xmlHttp.onreadystatechange = function(){ oThis.ReadyStateChange(); };
      this.xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      this.xmlHttp.send(theData);
    }
  }
}

AAS_AutoSuggest.prototype.PostCustomElements = function(elementsToPost)
{
	var postedElements = '';
	if (elementsToPost != null)
	{
		var reg=new RegExp('[,]+', 'g');
		var elementsTab = elementsToPost.split(reg);

		for (var i=0; i< elementsTab.length; i++) 
		{
			var elementForm = document.forms[0].elements[elementsTab[i]];

			if (elementForm && elementForm != '')
				postedElements += this.RetrieveState(elementForm);
		}
	}
	
	return postedElements;
}

AAS_AutoSuggest.prototype.RetrieveState = function(element)
{
	var theData = '';
	eName = element.name; 
	if( eName && eName != '')
	{
		if( eName == '__EVENTTARGET' || eName == '__EVENTARGUMENT' || eName == '__VIEWSTATE' || eName == '__EVENTVALIDATION')
		{
			// Do Nothing
		}
		else
		{
			var tagName = element.tagName.toLowerCase();
			if (tagName == 'input')
			{
				var inputType = element.type.toLowerCase();
				
				if (inputType == 'text' || inputType == 'password' || inputType == 'hidden')
				{
					theData += this.AddDataElement(element.name,element.value);	
				}
				
				else if (inputType == 'checkbox' || inputType == 'radio')
				{
					if (element.checked)
					{
						theData += this.AddDataElement(element.name,element.value);
					}
				}
			}
				
			else if (tagName == 'select')
			{
			 	var options = element.options;
			 	for (var j = 0 ; j < options.length ; j++)
				{
					if (options[j].selected)
					{
						theData += this.AddDataElement(options.name,options[j].value);
					}
				}
			}
				
			else if (tagName == 'textarea')
			{
				theData += this.AddDataElement(element.name,element.value);			
			}
		}
	}
	
	return theData;
}

AAS_AutoSuggest.prototype.AddDataElement = function(name,value)
{
	value = encodeURIComponent(value);
	return '&' + name + '=' + value;
}

AAS_AutoSuggest.prototype.OnKeyUp = function(e)
{
	var key = !(window.event) ? key = e.which : key = window.event.keyCode;
	if (key != 38 && key != 40 && key != 13 && key != 27)
	{
		AAS_currentRow = -1;
		AAS_row = -1;
		this.AAS_AutoSuggest.DoActiveCallBack(this.id,'');
	}
}

AAS_AutoSuggest.prototype.OnKeyDown = function(e)
{
    var key = !(window.event) ? key = e.which : key = window.event.keyCode;
        
	// left key
	if (key == 37)
	{
	}

	// up arrow
	else if (key == 38) 
	{  if (AAS_row > 0) 
		{
			AAS_row--;
			
		}
		this.AAS_AutoSuggest.ChangeSelectedItem();
	} 
	
	// right key
	else if (key == 39)
	{
	}

	// down arrow
	else if (key == 40) 
	{  
		var suggest = document.getElementById(this.AAS_AutoSuggest.textbox.id + '_Table');
        if (AAS_row < suggest.childNodes[0].childNodes.length -1 ) 
        {
			AAS_row++;
			this.AAS_AutoSuggest.ChangeSelectedItem();
		}
    }
    
    // enter key
    else if (key == 13)
    {
    
		this.AAS_AutoSuggest.textbox.value = document.getElementById(this.AAS_AutoSuggest.textbox.id + AAS_currentRow).tag;
    
		this.AAS_AutoSuggest.CloseSuggest();
    
		if (!this.AAS_AutoSuggest.DoPostBackAfterSelection)
			return false;
    }
    
    // escape key
    else if (key == 27) 
    {
		AAS_currentRow = -1;
		AAS_row = -1;
		this.AAS_AutoSuggest.CloseSuggest();
		return false;
    }
}

AAS_AutoSuggest.prototype.ChangeSelectedItem = function()
{
	var newRow = document.getElementById(this.textbox.id + AAS_row);
	
	if (AAS_row == -1)
	{
		AAS_row = 0;
	}
	
	if (AAS_currentRow == -1)
	{
		AAS_currentRow = AAS_row;
	}	
	var tdCurrent = document.getElementById(this.textbox.id + AAS_currentRow);
	tdCurrent.style.backgroundColor = this.SuggestBackColor;
	
	var tdRow = document.getElementById(this.textbox.id + AAS_row);
	tdRow.style.backgroundColor = this.SelectedItemBackColor;
	
    AAS_currentRow = AAS_row;
}

function SelectByClick(itemObj)
{
	var id = AAS_currentObject.id;
	AAS_currentObject.textbox.value = document.getElementById(itemObj.id).tag;

	AAS_currentObject.CloseSuggest();
	
	var doPostBackAfterSelection = eval('ActiveAutoSuggest_' + id).DoPostBackAfterSelection;
        
	if (!doPostBackAfterSelection != null && doPostBackAfterSelection == true)
	{
		__doPostBack(id,'');
	}
}

function AAS_GetAutoSuggest(id)
{
	return eval('ActiveAutoSuggest_' + id);
}

function AAS_BoldSearchPattern(text,pattern,ignoreCase)
{
	var index = -1;
	if (ignoreCase == true)
		index = text.toUpperCase().indexOf(pattern.toUpperCase(),0);
	else
		index = text.indexOf(pattern,0);
	
	if (index >= 0)
	{
		var ret = '';
		
		if (index == 0)
		{
			ret += text.substr(index,pattern.length).bold();
		}
		
		else
		{
			ret += text.substring(0,index);
			ret += text.substr(index, pattern.length).bold();
			
		}
		ret += text.substring(index + pattern.length,text.length);
		
		return ret;
	}
	
	return text;	
}

function AAS_FindPosX(obj)
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

function AAS_FindPosY(obj)
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



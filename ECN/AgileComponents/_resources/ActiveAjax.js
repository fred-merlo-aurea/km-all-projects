function AA_TestIfScriptPresent() 
{
}

function AA_GetXMLHttp()
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

function AA_CallBackObject(id) 
{
	this.id = id;
	this.xmlHttp = AA_GetXMLHttp();
	this.cache = new Array();
	this.argument = '';
	this.ie = (document.all) ? true : false;
}

AA_CallBackObject.prototype.Log = function(text) 
{
	try
	{
		if (this.Debug && !this.ie)
			console.log(AA_GetDate() + ':' + text);
	}
	catch (e)
	{
	}
}

AA_CallBackObject.prototype.ReadyStateChange = function()
{
	switch (this.xmlHttp.readyState)
	{
		// uninitialized
		case 0:
		{
			this.Log('unitialized');
			this.OnUninitialized();
		} break;
		
		// loading
		case 1:
		{
			this.Log('loading');
			if (this.LoadingDisplay)
			{
				var panel = document.getElementById(this.id);
				if (panel != null)
				{
					panel.style.height = panel.clientHeight;
					panel.style.width = panel.clientWidth + 2;
					panel.innerHTML = this.LoadingDisplay;
				}
				
			}
			this.OnLoading();
		} break;
		
		// loaded
		case 2:
		{
			this.Log('loaded');
			this.OnLoaded();
		} break;
		
		// interactive
		case 3:
		{
			this.Log('interactive');
			this.OnInteractive();
		} break;
		
		// complete
		case 4:
		{
			this.Log('complete');
			 if(this.xmlHttp.status == 0)
				this.OnAbort();
			else if( this.xmlHttp.status == 200)
			{
				if (this.UseCache)
				{
					if (this.cache[this.argument] == null)
					{
						this.cache[this.argument] = this.xmlHttp.responseText;
					}
				}
								
				document.getElementById(this.id).innerHTML = this.xmlHttp.responseText;
				this.OnComplete(this.xmlHttp.responseText, this.xmlHttp.responseXML);
				this.Log(document.getElementById(this.id).outerHTML);
			}
			else
				this.OnError(this.xmlHttp.status, this.xmlHttp.statusText, this.xmlHttp.responseText);   
		} break;
	}
}

AA_CallBackObject.prototype.OnUninitialized = function()
{
}

AA_CallBackObject.prototype.OnLoading = function()
{
}
 
AA_CallBackObject.prototype.OnLoaded = function()
{
}
 
AA_CallBackObject.prototype.OnInteractive = function()
{
}
 
AA_CallBackObject.prototype.OnComplete = function(responseText, responseXml)
{
}
 
AA_CallBackObject.prototype.OnAbort = function()
{
}
 
AA_CallBackObject.prototype.OnError = function(status, statusText)
{

}

AA_CallBackObject.prototype.DoActiveCallBack = function(eventTarget, eventArgument)
{
  this.argument = eventArgument;	

  if (this.UseCache)
  {
	if (this.cache[eventArgument] != null)
	{
		document.getElementById(this.id).innerHTML = this.cache[eventArgument];
		return;
	}
  }

  var theData = '';
  var theform = document.forms[0];
  var thePage = window.location.pathname + window.location.search;
  var eName = '';
 
  theData  = '__EVENTTARGET='  + escape(eventTarget.split("$").join(":")) + '&'; 
  theData += '__EVENTARGUMENT=' + eventArgument + '&';
  try 
  {
    theData += '__EVENTVALIDATION=' + encodeURIComponent(theform.__EVENTVALIDATION.value) + '&';
  }
  catch (e) {}
  theData += '__VIEWSTATE='    + escape(theform.__VIEWSTATE.value).replace(new RegExp('\\+', 'g'), '%2b') + '&';
  theData += 'AA_IsCallBack=true&';
  theData += 'AA_ClientId=' + this.id + '&';
  theData += 'AA_Argument=' + eventArgument/* + '&'*/;

  if (this.PostPanelOnly && this.SaveState)
  {
	theData += this.PostCustomElements(this.PostPanelOnly);
	theData += this.PostCustomElements(this.PostId);
  }
  
  else
  {
	if (this.SaveState)
	{
		for( var i=0; i<theform.elements.length; i++ )
		{
			theData += this.RetrieveState(theform.elements[i]);
		}
	}
  }
  //alert(theData);
  
  if( this.xmlHttp )
  {
    this.xmlHttp.abort();
    if( this.xmlHttp.readyState == 4 || this.xmlHttp.readyState == 0 )
    {
      var oThis = this;
      this.xmlHttp.open('POST', thePage, true);
      this.xmlHttp.onreadystatechange = function(){ oThis.ReadyStateChange(); };
      this.xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      this.xmlHttp.send(theData);
    }
  }
}

AA_CallBackObject.prototype.PostCustomElements = function(elementsToPost)
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

AA_CallBackObject.prototype.RetrieveState = function(element)
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

AA_CallBackObject.prototype.AddDataElement = function(name,value)
{
	value = (this.EncodeURI) ? encodeURIComponent(value) : escape(value);
	return '&' + name + '=' + value;
}

function AA_GetAjaxPanel(id)
{
	return eval('ActiveAjax_' + id);
}

function AA_DoPostBack(id,eventArgument)
{
	AA_GetAjaxPanel(id).DoActiveCallBack(id,eventArgument);
}		
		
function AA_GetDate()
{
	var date = new Date();
	
	return date.toLocaleTimeString();
}


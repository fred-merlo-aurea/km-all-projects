 // 0 up, 1 down, 2 left, 3 right
var AIP_dom = (document.getElementById) ? true : false;
var AIP_ns5 = ((navigator.userAgent.indexOf("Gecko")>-1) && AIP_dom) ? true: false;
var AIP_ie5 = ((navigator.userAgent.indexOf("MSIE")>-1) && AIP_dom) ? true : false;
var AIP_ns4 = (document.layers && !AIP_dom) ? true : false;
var AIP_ie4 = (document.all && !AIP_dom) ? true : false;

function AIP_testIfScriptPresent()
{
}
 
function AIP_moveItemUp(id)
{
	AIP_moveItem(id, 0);
}

function AIP_moveItemDown(id)
{
	AIP_moveItem(id, 1);
}

function AIP_moveItemLeft(id)
{
	AIP_moveItem(id, 2);
}

function AIP_moveItemRight(id)
{
	AIP_moveItem(id, 3);
}

function AIP_removeItem(listbox, index)
{
	if (AIP_ns5)
		listbox.options[index]==null;
	else
		listbox.options.remove(index);
}

function AIP_getSelection(id)
{
	var listbox = AIP_getObject(id + '_select');
	var str = '';
	
	for(index = 0;index<listbox.options.length;index++)
		if (listbox.options[index].selected)
			str += index + ",";
	
	return str;
}

function AIP_moveItem(id, direction)
{
	var listbox = AIP_getObject(id + '_select');
	selectedIndex = listbox.selectedIndex;
	
	if (direction == 0)
	{
		for(index = 0;index<listbox.options.length;index++)
		{
			var item = listbox.options[index];
			
			if (item.selected == true && index > 0 && item.getAttribute('locked') == null)
			{
				AIP_removeItem(listbox, index);
				listbox.options.add(item, index-1);
			}
		}
	}
	else if (direction == 1)
	{
		for(index=listbox.options.length-1;index>=0;index--)
		{
			var item = listbox.options[index];
			if (item.selected == true && index < listbox.options.length && item.getAttribute('locked') == null)
			{
				AIP_removeItem(listbox, index);
				listbox.options.add(item, index + (AIP_ns5 ? 2 : 1));
			}
		}
	}
	else if (direction == 2 || direction == 3)
	{
		var linkedListbox = AIP_getObject(listbox.getAttribute((direction == 2 ? 'leftlistbox' : 'rightlistbox')) + '_select');
	
		for(index=listbox.options.length-1;index>=0;index--)
		{
			var item = listbox.options[index];
			if (item.selected == true && index < listbox.options.length && item.getAttribute('locked') == null)
			{
				AIP_removeItem(listbox, index);
				var strTxt = (item.text.indexOf('. ') > 0 ? item.text.substring(item.text.indexOf('.') + 2) : item.text);
				item.text = strTxt;
				linkedListbox.options.add(item);
				//listbox.options.add(item, index+1);
				
				if (linkedListbox.getAttribute('enumerate') != null)
					AIP_enumerate(linkedListbox.id);
			}
		}
	}
	
	AIP_saveOrder(id);
	
	if (listbox.getAttribute('enumerate') != null)
		AIP_enumerate(listbox.id);
}

function AIP_enumerate(id)
{
	var listbox = AIP_getObject(id);
	
	var index = 0;
	
	for(index = 0;index<listbox.options.length;index++)
	{
		var item = listbox.options[index];
		var strTxt = (item.text.indexOf('. ') > 0 ? item.text.substring(item.text.indexOf('.') + 2) : item.text);
		
		item.text = (index+1) + '. ' + strTxt;
	}
}

function AIP_getObject(id)
{
	return document.getElementById(id);
}

function AIP_saveOrder(id)
{
	var order = '';
	var listbox = AIP_getObject(id + '_select');
	
	for(index = 0;index<listbox.options.length;index++)
		order += listbox.options[index].value + '~' + listbox.options[index].text +  '~' + listbox.options[index].selected + '|';
		
	AIP_getObject(id).value = order;
}

// Start Numeric
function AIP_compare (a,b)
{
var a=AIP_rightTrim(AIP_leftTrim(a+''));
var b=AIP_rightTrim(AIP_leftTrim(b+''));
if (a.length==0) return 0; // vracamo da su jednaki?!
if (b.length==0) return 0;
if (a.charAt(0)=='-' && b.charAt(0)=='-')  //if both are negative
	{
	a=a.substring(1,a.length);
	b=b.substring(1,b.length);
	return -AIP_compare(a,b);
	}
else if (a.charAt(0)=='-') //a negative
	{
	a=a.substring(1,a.length);
	if (AIP_compare(a,0)==0 && AIP_compare(b,0)==0) return 0;
	else return -1;
	}
else if (b.charAt(0)=='-') //b negative
	{
	b=b.substring(1,b.length);
	if (AIP_compare(a,0)==0 && AIP_compare(b,0)==0) return 0;
	else return 1;
	}
// they are both positive

ai=0;
bi=0;
if (a.indexOf('.') == -1) atacka=a.length;
else atacka=a.indexOf('.');
if (b.indexOf('.') == -1) btacka=b.length;
else btacka=b.indexOf('.');

promena=1;
priva=0;
privb=0;
while (promena)
{
	promena=0;
	//uzimamo vrednost za a
	if (ai<a.length)
	{
		promena=1;
		priva=a.charAt(ai);
		if (priva=='.') priva=0;
		stepa=atacka-ai;
	}
	if (bi<b.length)
	{
		promena=1;
		privb=b.charAt(bi);
		if (privb=='.') privb=0;
		stepb=btacka-bi;
	}


	if ('1'<=priva && priva<='9')
	{
		if ((stepa>stepb&&stepb>=0) || (stepa==stepb && priva>privb)) return 1;
		if (bi>=b.length&&stepa<0&&stepa!=stepb&&ai<a.length) return 1;
	}
	if ('1'<=privb && privb<='9')
	{
		if ((stepb>stepa&&stepa>=0) || (stepb==stepa && privb>priva)) return -1;
		if (ai>=a.length&&stepb<0&&stepa!=stepb&&bi<b.length) return -1;
	}

	
	if (stepa>stepb&&ai<a.length) ai+=1;
	else if (stepb>stepa&&bi<b.length) bi+=1;
	else {ai+=1;bi+=1;}
}
return 0;
}

function AIP_numeralsOnly(evt,input,mini,maxi,deci,point) 
{
	if (AIP_compare(mini,0)==-1) {var reg = /^(\s)*(\-)?(0|[1-9]\d*)?(\.\d*)?(\s)*$/;	var decireg = /^(\s)*(\-)?(0|[1-9]\d*)?(\s)*$/;}
	else {var reg = /^(\s)*(0|[1-9]\d*)?(\.\d*)?(\s)*$/;	var decireg = /^(\s)*(0|[1-9]\d*)?(\s)*$/;}
    evt = (evt) ? evt : event;
	var charCode = (evt.charCode) ? evt.charCode :0;;
	var keyCode = (evt.keyCode) ? evt.keyCode :0;
	if (!evt.ctrlKey &&!evt.altKey && ((AIP_ie5 && keyCode > 31) || (AIP_ns5 && keyCode==0)))
		{
			if (AIP_ie5) charCode=keyCode; 
			
			if ((charCode<48 || charCode> 57) && charCode!=point.charCodeAt(0) && charCode!=point.charCodeAt(point.length-1) &&charCode!=45) return false;
			//now we need caret position. eeeek!
			if (AIP_ns5)
			{	
				var caret = 0;
				var textsele = 0;
				if (input.createTextRange) {
					var rg = document.selection.createRange().duplicate();
					rg.moveStart('textedit',-1);
					caret = rg.text.length;
					textsele=rg.text.length;
					} else if (input.setSelectionRange) {
					caret = input.selectionStart;
					textsele= input.selectionEnd;
					}
				var potval=input.value.substring(0,caret)+String.fromCharCode(charCode)+input.value.substring(textsele,input.value.length);
				if (point.indexOf(',') != -1)	
					potval=potval.replace(',','.');
				var result = (deci) ? reg.test(potval):decireg.test(potval);
				if (!result) return false; // wrong value
				if ((AIP_compare(potval,mini)==-1&&AIP_compare(mini,0)==-1)||(AIP_compare(maxi,potval)==-1&&AIP_compare(maxi,0)==1)) return false; //overflow
			}
			else if(AIP_ie5)
			{
				var rg= document.selection.createRange();
				var old=input.value;
				
				if (rg.text) {rg.text=String.fromCharCode(charCode);var potval=input.value;}
				else
				
				{
					//alert('efef');
					var rg1 = document.selection.createRange().duplicate();
					rg1.moveStart('textedit',-1);
					var caret = rg1.text.length;
					if (input.setSelectionRange) 
						caret= input.selectionEnd;
						var potval=input.value.substring(0,caret)+String.fromCharCode(charCode)+input.value.substring(caret,input.value.length);
				}
				if (point.indexOf(',') != -1)	
					potval=potval.replace(',','.');
				
				var result = (deci) ? reg.test(potval):decireg.test(potval);
				if (!result) { input.value=old;return false}// wrong value
				if ((AIP_compare(potval,mini)==-1&&AIP_compare(mini,0)==-1)||(AIP_compare(maxi,potval)==-1&&AIP_compare(maxi,0)==1)) { input.value=old;return false} //overflow
			}
		 }
	return true;
}
function AIP_leftTrim( strValue ) {
var objRegExp = /^(\s*)(\b[\w\W]*)$/;

      if(objRegExp.test(strValue)) {
       //remove leading a whitespace characters
       strValue = strValue.replace(objRegExp, '$2');
    }
  return strValue;
}
function AIP_rightTrim( strValue ) 
{
	var objRegExp = /^([\w\W]*)(\b\s*)$/;
    if(objRegExp.test(strValue)) 
    strValue = strValue.replace(objRegExp, '$1');
	return strValue;
}

spechar = /([\$\(\)\*\+\.\[\]\?\\\/\^\{\}\|])/g;

function AIP_numeralsAfter(input,currency,mini,maxi,deci,point) 
{
	var signval="";
	if (AIP_compare(mini,0)==-1) signval="-?";
	var decimust="";
	var deciopt="";
	if (deci>0)
		{
			var decimust="\\.\\d{1,"+deci+"}(?=\\d*$)";
			var deciopt="\\.\\d{0,"+deci+"}(?=\\d*$)";
			var string4regex="(^"+signval+"\\d+"+deciopt+")|(^"+signval+"\\d+$)|(^"+signval+decimust+")";
		}
	
	else var string4regex="(^"+signval+"\\d+"+deciopt+")|(^"+signval+"\\d+$)";
	
	var objRegExp= new RegExp(string4regex,"g");
	//zarazi i sl.
	//var objRegExp[0]  = /(?=^(\s)*)((-?\d+\.\d*)|(-?\d+)|(-?\.\d+))(?=(\s)*$)/g; //all mini<0 deci>0
	var priv = AIP_leftTrim(input.value);
	var priv = AIP_rightTrim(input.value);
	var tacka=String.fromCharCode(point.charCodeAt(0));
	if (point.indexOf('.') == -1)	priv=priv.replace('.','#');
	if (point.indexOf(',') != -1)	priv=priv.replace(',','.');
	var priv = priv.match(objRegExp);
	if (priv &&AIP_compare(priv[0],mini)>=0&&AIP_compare(priv[0],maxi)<=0)
		if (deci>0)
			{
				var indexoftackam1=priv[0].indexOf('.')
				var addzeros="";
				if (indexoftackam1>=0) n=indexoftackam1+deci+1-priv[0].length; 
				else n=deci;
				for (var i=1;i<=n;i++) addzeros+="0";
				var objRegExp2= new RegExp("^"+tacka.replace(spechar,"\\$1"),"g");
				var objRegExp3= new RegExp("^-"+tacka.replace(spechar,"\\$1"),"g");
				priv[0]=priv[0].replace(objRegExp2,"0"+tacka);
				//priv[0]=priv[0].replace(objRegExp3,"-0"+tacka);
				if (indexoftackam1==-1)	priv[0]=currency+(priv[0]+"."+addzeros).replace(".",tacka);
				else priv[0]=currency+priv[0].replace(".",tacka)+addzeros;
				input.value=priv[0];
			}
		else
			input.value=currency+priv[0];
			
	else input.value="";
	return true;
}
function AIP_numeralsBefore(input,currency) 
{
//	User is entering into the textarea
//	We just need to remove $ which we added before
	var objRegExp= new RegExp("^"+currency.replace(spechar,"\\$1"),"g");
	input.value=input.value.replace(objRegExp,"");
	return true;
}
// Masked TextBox

	  //*******************************************************************************
	  //Function called when a key is press down get the key that was press by code and
	  //check for it if it match the current specifications for the control
	  //*******************************************************************************
      function AIP_pressing( e,control )
      {
		if (control.readOnly)
			return false;
		var keyCode;
		var returnedValue;
		//check for what the curret browser support
        keyCode = window.event?event.keyCode:e.which;
        returnedValue = false;

		if (keyCode != 9)
		{

			//get the clean code for the key pad and , and .o
			var keyCharacter = AIP_cleanKeyCode(keyCode,e);
			//get the current position of the cursor in the control
			var position = AIP_getCursorPosition(control);
	        
			var mask;
			mask = AIP_concatenateMask(control);
			
			//get the current mode
			var mode = control.getAttribute("modeInput");
			
			//these scenarious are for the controls that has defined mask or customFormat attribute
			if ( mask != null )
			{
				returnedValue = AIP_operateForMask(control, e);
			}
			else
			{
				//operate if a mask attribute is not specified
				returnedValue = AIP_operateForNoMask(control, e);
			}
			return false;
		}
		return true;
      }
      
      //*******************************************************************************
      //the function that will operate when a mask or customFormat attributes are 
      //presented.
      //*******************************************************************************
      //control		- - the current textbox control
      //e				- the event used in the Mozilla
      //*******************************************************************************
      function AIP_operateForMask(control, e)
      {
		//check for what the curret browser support
        keyCode = window.event?event.keyCode:e.which;
        var returnedValue ;
        returnedValue = false;

		//get the clean code for the key pad and , and .o
        var keyCharacter = AIP_cleanKeyCode(keyCode,e);
        //get the current position of the cursor in the control
        var position = AIP_getCursorPosition(control);
        
        var mask;
        mask = AIP_concatenateMask(control);
        
		//get the current mode
		var mode = control.getAttribute("modeInput");
		switch (keyCode)
			{
				//TAB key
				case 9:returnedValue=true; break;
				//ENTER key
				case 13:returnedValue=true;break;
				//END key
				case 35: AIP_setCursorAtPosition(control,control.value.length); break;
				//HOME key
				case 36: AIP_setCursorAtPosition(control,0); break;
				//SHIFT key
				case 16: return false; break;
				//CTRL key
				case 17: return false; break;
				//ALT key
				case 18: return false; break;
				//DEL key
				case 46:
					if(position > -1) {
						//get the current mask character
						var currentMaskChar = mask.charAt(position);
						// only allow delete if it's a valid char
						if(currentMaskChar == '#' ) 
						{
							//perform delete of the current symbol and replace it with _
							if (document.selection)
							{
								AIP_showCurrentCharacter(control, position, false, '_')
							}
							else
							{
								//support for Mozilla. the double _ is needed because of key handling in Mozilla
								AIP_showCurrentCharacter(control, position, true, '__')
							}
						}
						else
						{
							//ONLY FOR MOZILLA. This is needed because the Mozilla catch all keys without 
							//looking if they are catched by user or not. In ie if you don't tell what should 
							//be done by a specific key IE exclude it
							if (!document.selection)
							{
								var tempValue = control.value.charAt(position)+ control.value.charAt(position);
								AIP_showCurrentCharacter(control, position, true, tempValue)
							}
						}
					}
				break;
				//BACKSPACE key
				case 8:
					if(position > -1) 
					{
						var currentMaskChar;
						var gone;
						gone = false;
						// get next available char to delete except mask chars
						while(position > 0 ) 
						{
							
							position--;
							currentMaskChar = mask.charAt(position);
							//check for a valid mask string
							if(currentMaskChar == '#' ) 
							{
								if (document.selection)
								{
									AIP_showCurrentCharacter(control, position, false, '_', 0 );
								}
								else
								{
									AIP_showCurrentCharacter(control, position, true, '__', 1 );
								}
								
								break;
							}
							else
							{
								if (!document.selection && position == 0)
								{
									AIP_showCurrentCharacter(control, position, true, control.value.charAt(position), 0 );
									break;
								}
							}
							
						}
					}
				break;
				//right arrow
				case 37:
					//this must be est only for IE. In Mozilla it knows what should be happened when a right arrow is pressed
					if ( document.selection )
					{
						AIP_setCursorAtPosition(control,position-1);
						control.curPost = position-1;
					}
				break
				//left arrow
				case 39:
					//this must be est only for IE. In Mozilla it knows what should be happened when a left arrow is pressed
					if ( document.selection )
					{
						AIP_setCursorAtPosition(control,position+1);
						control.curPost = position+1;
					}
				break;
				//others key
				default:
					if ( mask.charAt(position) != '#' )
						position = AIP_getPositionOfMaskString(control, position );
					//execute it if the current position must be replaced
					if ( mask.charAt(position) == '#' )
					{   
						var value = control.value;
						var numerics = '0123456789';
						var letters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
						var numericsAndLetters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
						if ( position < mask.length )
						{
							switch (mode)
							{
								//every character is allowed
								case "none": AIP_setCharacterAtPosition(control,keyCharacter, position, e); break;
								//only letters
								case "character":
									AIP_showCharacterInMask( letters, control, position, keyCharacter, e );
								break;
								//only numbers
								case "numeric":
									AIP_showCharacterInMask( numerics, control, position, keyCharacter, e );
								break;
								case "mask":
									var customformat = control.getAttribute("customFormat");
									var customPosition = customformat.charAt(position);
									switch ( customPosition)
									{
										case 'X':
											AIP_showCharacterInMask( numericsAndLetters, control, position, keyCharacter,e  );
										break;
										case '9':
											AIP_showCharacterInMask( numerics, control, position, keyCharacter, e );
										break;
										case 'A':
											AIP_showCharacterInMask( letters, control, position, keyCharacter, e );
										break;
									}
								break;
							}
						}
					}
					else
					{
						if (!document.selection)
						{
							AIP_showCurrentCharacter(control, position, true, control.value.charAt(position), 0 );
  						}
					}
				break;
			}
		return returnedValue;
      }
      
      //*******************************************************************************
      //the function that will operate when a mask or customFormat attributes are not
      //presented.
      //*******************************************************************************
      //control		- - the current textbox control
      //e				- the event used in the Mozilla
      //*******************************************************************************
      function AIP_operateForNoMask( control, e )
      {
		//check for what the curret browser support
        keyCode = window.event?event.keyCode:e.which;
        var returnedValue ;
        returnedValue = false;

		//get the clean code for the key pad and , and .o
        var keyCharacter = AIP_cleanKeyCode(keyCode,e);
        //get the current position of the cursor in the control
        var position = AIP_getCursorPosition(control);
        
        var mask;
        mask = AIP_concatenateMask(control);
		
		//get the current mode
		var mode = control.getAttribute("modeInput");
		var value = control.value;
		switch ( keyCode )
		{
			//RIGHT ARROW KEY
			case 37:
				if ( document.selection )
				{
					AIP_setCursorAtPosition(control,position-1);
					control.curPost = position;
				}
			break
			//LEFT ARROW KEY
			case 39:
				if ( document.selection )
				{
					AIP_setCursorAtPosition(control,position+1);
					control.curPost = position;
				}
			break;
			//BACKSPACE KEY
			case 8:
				if ( document.selection )
				{
					AIP_showCurrentCharacter ( control, position-1, true, '');
					control.maxLength = control.maxLength - 1;
					break;
				}
				else
				{
					var x;
					var y;
					if (control.getAttribute("delimiter")!= null )
					{
						if ( position <= control.value.indexOf(control.getAttribute("delimiter")))
						{
							x = control.value.substring(0,position-1);
							y = control.value.substring(position,control.value.length);
							position--;
						}
						else
						{
							if ( position == control.value.length )
							{
								x = control.value.substring(0,position);
								y = control.value.substring(position+1,control.value.length);
							}
							else
							{
								x = control.value.substring(0,position-1);
								y = control.value.substring(position,control.value.length);
							}
						}
					}
					else
					{
						if ( position < control.value.length )
						{
							x = control.value.substring(0,position-1);
							y = control.value.substring(position,control.value.length);
							position--;
						}
						else
						{
							x = control.value.substring(0,position);
							y = control.value.substring(position+1,control.value.length);
						}
					}
					control.maxLength = control.maxLength+1;
					control.value = x + control.value.charAt(position) + y;
					control.maxLength = control.maxLength-1;
					if ( control.getAttribute("delimiter") != null )
					{
						if (position < control.value.indexOf(control.getAttribute("delimiter")) )
							AIP_setCursorAtPosition(control,position+1);
						else
							AIP_setCursorAtPosition(control,position);
					}
					else
						AIP_setCursorAtPosition(control,position+1);
					
					control.curPost = position+1;
					break;
				}
				
			//DEL KEY
			case 46:
				if ( document.selection )
				{
					AIP_showCurrentCharacter ( control, position, true, '');
					control.maxLength = control.maxLength - 1;
				}
				else
				{
					var x = control.value.substring(0,position);
					var y = control.value.substring(position+1,control.value.length);
					control.maxLength = control.maxLength;
					control.value = x + control.value.charAt(position) + y;
					control.maxLength = control.maxLength - 1;
					AIP_setCursorAtPosition(control,position);
					control.curPost = position;
				}
			//SHIFT KEY
			case 16: return false; break;
			//CTRL KEY
			case 17: return false; break;
			//ALT KEY
			case 18: return false; break;
			//END KEY
			case 35: AIP_setCursorAtPosition(control,control.value.length); break;
			//HOME KEY
			case 36: AIP_setCursorAtPosition(control,0); break;
			//OTHER KEY
			default:
				//check mode
				var shift = false
				if (!window.event )
				{
					if ( e.shiftKey )
						shift = true;
				}
				switch (mode)
				{
					//all characters are allowed
					case "none": 
						if ( control.value.length == 0 )
							control.maxLength = 1;
						else
						{
							if (document.selection || (!document.selection && position == control.value.length ))
								control.maxLength = control.maxLength + 1;
						}
						
						if ( document.selection )
							AIP_setCharacterAtPositionWithoutMask(control,keyCharacter, position, e);
						else
							AIP_showCurrentCharacter(control, position, true, keyCharacter, control.value.length-position+1);
						
					break;
					//only letters are allowed
					case "character":
						if('abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'.indexOf(keyCharacter) != -1)
						{
							if ( control.value.length == 0 )
								control.maxLength = 1;
							else
							{
								if (document.selection || (!document.selection && position == control.value.length ))
									control.maxLength = control.maxLength + 1;
							}
							if ( document.selection )
							{
								AIP_setCharacterAtPositionWithoutMask(control,keyCharacter, position, e);
								returnedValue = true;
							}
							else 
							{
								AIP_showCurrentCharacter(control, position, true, keyCharacter, control.value.length-position+1);
							}
								
						}
						else
						{	
							if ( !document.selection )
							{
								control.maxLength = position-1;
							}
						}
					break;
					//only numeric are allowed
					case "numeric":
						AIP_workWithNumbers (control, keyCharacter, position, e, null);
					break;
					//only numbers are excepted - like in the numberic
					//but at the end of the value a % symbol appears
					case "percent": 
						AIP_workWithNumbers (control, keyCharacter, position, e, false);
						return false;
					break;
					//accept only numbers, but performs some basic checks for the data
					//like the day can neot start with more than 3 and the month can not start with more than 1
					case "datetime":
						if('0123456789'.indexOf(keyCharacter) != -1 && !shift)
						{
							var format = control.getAttribute("format");
							if ( format != null )
								control.maxLength = format.length;
							else
								control.maxLength = 0;

							if ( position < control.maxLength )
							{
								var typeIt = false;
								//check if the user is entering day, mont, year
								switch ( format.charAt(position)) 
								{
									case 'M':
										if ( format.charAt(position-1) == 'M' )
											typeIt = true;
										else
										{
											if ( keyCharacter == '0' || keyCharacter == '1' )
												typeIt = true;
										}
									break;
									case 'D':
										if ( format.charAt(position-1) == 'D' )
											typeIt = true;
										else
										{
											if ( keyCharacter == '0' || keyCharacter == '1' || keyCharacter == '2' || keyCharacter == '3' )
												typeIt = true;
										}
									break;
									case 'Y':
										typeIt = true;
									break;
									//if the user is selected the / symbol then we have to modify the next symbol
									//so we have to check what is next - day, month or year
									case '/':
										if ( control.value.charAt(position) != '/' )
										{
											control.value = control.value.substring(0, position) + '/' + control.value.substring(position);
										}
										position++;
										if ( format.charAt(position) == 'D' && (keyCharacter == '0' || keyCharacter == '1' || keyCharacter == '2' || keyCharacter == '3'))
											typeIt = true;
										else if ( format.charAt(position) == 'M' && (keyCharacter == '0' || keyCharacter == '1'))
											typeIt = true;
										else if ( format.charAt(position) == 'Y' )
											typeIt = true;
										else
											position--;
									break;
								}
								if (format.charAt(position) == '/' )
								{
									if(document.selection)
										AIP_setCharacterAtPositionWithoutMask(control,'/', position, e);
									else
									{
										if (!typeIt)
											position++;
										control.value = control.value.substring(0, position-1) + '/' + control.value.substring(position);
									}
								}
								if ( typeIt )
								{
									if (!document.selection)
									{
										if ( position < control.value.length )
										{
											control.value = control.value.substring(0, position) + keyCharacter + control.value.substring(position+1);
										}
										else
										{
											control.value = control.value.substring(0, position) + keyCharacter + control.value.substring(position);
										}
										
										control.maxLength = control.value.length;
										if ( position < control.value.length )
											control.maxLength = control.maxLength - 1;
										if (!AIP_isTextSelected(control) && position < control.value.length)
											control.maxLength = control.maxLength - 1;
										returnedValue = false;

									}
									else
									{
										AIP_setCharacterAtPositionWithoutMask(control,keyCharacter, position, e);
										returnedValue = true;
									}
								}
								else
								{
									control.maxLength = control.value.length;
									if (!document.selection)
									{
										AIP_setCursorAtPosition(control, control.value.length);
									}
								}
								
								//check if the next symbol is / and if it is then go the symbol after /
								if ( format.charAt(position+1) == '/' )
								{
									if (document.selection)
									{
										position++;
										AIP_setCharacterAtPositionWithoutMask(control,'/', position, e);
									}
									else
									{
										if ( position != 0 )
											control.maxLength = control.value.length;
										control.value = control.value.substring(0, position) + keyCharacter + '/' + control.value.substring(position+2);
										control.maxLength = control.value.length;
										break;
									}
								}

							}
						}
						else
						{
							if ( !document.selection )
							{
								control.maxLength = control.value.length-1;
								AIP_setCursorAtPosition(control, position);
							}
						}
					break;
					//currency - like the numeric and percent but at the fron of the value appears a string for currency
					case "currency":
						returnedValue = false;
						AIP_workWithNumbers (control, keyCharacter, position, e, true);
					break;
				}
			break;
		}
		return returnedValue;
      }
      
      //*******************************************************************************
      //The function that is used when you have to use numeric, percent or currency
      //textfield. The function can receive front as null, true and false that will 
      //describe if the current type is numeric or you will use some character in front
      //or in the end of the entered value
      //*******************************************************************************
      //control			- the control that we are working with
      //keyCharacter	- the key value that should be displayed
      //position		- the current position
      //e				- the event used in the Mozilla
      //front			- if it is null then no special characters will be added to the 
	  //					entered value; If it is true - the special characters will be
	  //					added in the begining of the entered value separate with the
	  //					value with ' '; if it is false - the special characters will be
	  //					added in the end again separate with ' '
      //*******************************************************************************
      function AIP_workWithNumbers ( control, keyCharacter, position, e, front )
	  {
		var numerics = '1234567890';
		var val;
		var inPosition = 0;
		var delimiter = control.getAttribute("delimiter");
		var specialSymbol = control.getAttribute("specialSymbol");
		var precision = control.getAttribute("precision");
		var shownSymbolLength = 0;
		var typeIt = false;
		
		var selected = AIP_isTextSelected(control);
		
		if ( precision != null && delimiter != null)
			numerics = numerics + delimiter;
		
		val = AIP_getOnlyNumber(control.value, numerics);
		
		inPosition = position;
		if ( control.value.length != val.length )
		{
			shownSymbolLength = specialSymbol.length + 1;
			if (front==true)
			{
				inPosition = position-control.value.length+val.length;
			}
		}
		//alert(val);
		if (val.length==0)
			control.maxLength=0;
			
		if(numerics.indexOf(keyCharacter) != -1)
		{
			if ( keyCharacter != delimiter )
			{
				//no delimiter
				if ( val.indexOf(delimiter) == -1 )
				{
					typeIt = true;
					if (selected)
						val = val.substring(0, inPosition) + keyCharacter + val.substring(inPosition+1);
					else
					{
						control.maxLength++;
						val = val.substring(0, inPosition) + keyCharacter + val.substring(inPosition);
					}
				}
				//there is a delimiter
				else
				{
					if ( inPosition <= val.indexOf(delimiter) )
					{
					
						control.maxLength++;
						val = val.substring(0, inPosition) + keyCharacter + val.substring(inPosition);
					}
					else
					{
						if (val.length-val.indexOf(delimiter)-1 < parseInt(precision) )
						{
							control.maxLength++;
							val = val.substring(0, inPosition) + keyCharacter + val.substring(inPosition);
						}
						else
						{
							if ( inPosition < val.length )
								val = val.substring(0, inPosition) + keyCharacter + val.substring(inPosition+1);
						}
					}
				}
			}
			//a delimiter is pressed
			else
			{
				if ( val.indexOf(delimiter) == -1 )
				{
					control.maxLength++;
					val = val.substring(0, inPosition) + delimiter + val.substr(inPosition,parseInt(precision));
					if ( inPosition != val.length )
						control.maxLength = val.length;
						
					typeIt = true;
				}
			}
			
			if ( typeIt && front == true && val.length == 1 )
			{
				position += specialSymbol.length + 1;
			}
			
			//determine if a special symbol should be added to the front or to the end
			if ( front != null && typeIt)
			{
				control.maxLength = val.length + specialSymbol.length + 1;
				if ( front )
				{
					control.value = specialSymbol + ' ' + val;
				}
				else
				{
					control.value = val + ' ' + specialSymbol;
				}	
			}
			else if ( front == null )
			{
				control.value = val;
			}
			AIP_setCursorAtPosition( control, position+1);
		}
		else
		{
			if ( !document.selection )
			{
				control.maxLength = control.value.length;
				AIP_setCursorAtPosition(control, control.value.length);
			}
		}
	  }
      
    //*******************************************************************************
    //Help function for determing if the current browser should see the press key or 
    //not. For Mozilla separately is put if the pressed key is not available for showing
    //what the browser should do
    //*******************************************************************************
	//allowedSymbols	- the keys that are allowed for this textbox
	//control			- the curent textbox control that is edited
	//position			- the position at wich the key should be set
	//keyCharacter		- the value that will be displayed
	//e					- the event used in the Mozilla
    //*******************************************************************************
    function AIP_showCharacterInMask ( allowedSymbols, control, position, keyCharacter, e )
    {
	if(allowedSymbols.indexOf(keyCharacter) != -1)
		AIP_setCharacterAtPosition(control,keyCharacter, position, e);
	else
	{
		if (!document.selection)
			AIP_showCurrentCharacter(control, position, true, control.value.charAt(position));
	}
    }
      
    //*******************************************************************************
    //used to set a specific character in a text control when the control
    //does not use mask attribute
    //*******************************************************************************
	//control		- the curent textbox control that is edited
	//key			- the key pressed from the keyboard
	//position		- the position at wich the key should be placed
	//e				- the event used in the Mozilla
    //*******************************************************************************
    function AIP_setCharacterAtPositionWithoutMask ( control, key, position, e )
    {
	var prefixString, suffixString;
	prefixString = control.value.substring(0,position);
	suffixString = control.value.substring(position+1,control.value.length);
	
	if ( document.selection)
		control.value = prefixString + key  + suffixString;
	else
	{
		//if the user use Mozilla
		//if the user was changing something in the middle of the value
		if ( position < control.value.length )
			control.value = prefixString + key  + suffixString;
		else
		//if the user has typed at the end of the value
			control.value = prefixString + suffixString;
	}
		
	position = AIP_getPositionOfMaskString(control, position);
	AIP_setCursorAtPosition(control,position);
	
	control.curPos = position;
	return false;
    }
      
      
    //*******************************************************************************
    //set character at a specific position when use control with mask attribute
    //*******************************************************************************
	//control		- the curent textbox control that is edited
	//key			- the key pressed from the keyboard
	//position		- the position at wich the key should be placed
	//e				- the event used in the Mozilla
    //*******************************************************************************
    function AIP_setCharacterAtPosition ( control, key, position, e )
    {
	var x, y;
	var mask = AIP_concatenateMask(control);
	x = control.value.substring(0,position);
	y = control.value.substring(position+1,control.value.length);

	//IE
	if ( document.selection )
	{
		//check if the current character in the mask is #. If yes, put in the value the pressed key
		if (mask.charAt(position) == '#' )
			control.value = x + key + y;
		else
		//if no , put in the value, the coresponding character from the mask
			control.value = x + mask.charAt(position) + y;
	}
	else
	{
		//MOZILLA
		//check if the current value can be changed e.g. it is # (in the control value it will be _
		if ( control.value.charAt(position+1) != '_')
		{
			//check if the current character in the mask is #. If yes, put in the value the pressed key
			if (mask.charAt(position) == '#' )
				control.value = x + key + y;
			else
				//if no , put in the value, the coresponding character from the mask
				control.value = x + mask.charAt(position) + y;
		}
		else
			control.value = x + key + y;
	}

	position = AIP_getPositionOfMaskString(control, position);
	AIP_setCursorAtPosition(control,position);
	
	control.curPos = position;
	return false;
    }
      
      
    //*******************************************************************************
    //called this function from the controls that are using mask attribute
    //to set the mask in the value by replacing the # symbols with _ symbols
    //*******************************************************************************
	//control		- the curent textbox control that is edited
    //*******************************************************************************
    function AIP_controlFocus(control)
    {
    var mask = AIP_concatenateMask(control);
    if ( mask != null )
    {
		control.maxLength = mask.length;
		if(control.value.length == 0 || control.value == null)
		{
			if ( mask != null )
			{
				var normalizedmask = ''
				var counter
				counter = 0
				while ( counter < mask.length )
				{
					if ( mask.charAt(counter) == '#' )
						normalizedmask = normalizedmask + '_';
					else
						normalizedmask = normalizedmask + mask.charAt(counter);
						
					counter++;
				}
				control.value = normalizedmask;
			}
			AIP_setCursorAtPosition(control, AIP_getPositionOfMaskString(control,0));
		}
		else
		{
			AIP_setCursorAtPosition(control, AIP_getCursorPosition(control));
		}
	}
    }
      
    //*******************************************************************************
    //If a mask attribute is set in the html tag then it is returned
    //if the modeInput is set to mask, then to use the current functionality
    //the custom format is used for creating the mask
    //*******************************************************************************
	//control		- the curent textbox control that is edited
    //*******************************************************************************
    function AIP_concatenateMask(control)
    {
	var mask;
	//get the mask if there is defined any or if the modeInput is mask
    //define the mask based on the customFormat
    if ( control.getAttribute("mask") != null )
		mask = control.getAttribute("mask");
	else if ( control.getAttribute("customFormat") != null && control.getAttribute("modeInput") == "mask" )
	{
		//create the mask variable based on the custom format
		var customFormat = control.getAttribute("customFormat");
		var index = 0;
		mask = "";
		while ( index < customFormat.length )
		{
			if ( customFormat.charAt(index) == "9" ||
					customFormat.charAt(index) == "A" ||
					customFormat.charAt(index) == "X" )
				mask += "#";
			else
				mask += customFormat.charAt(index);
			index++;
		}
	}
	return mask;
    }
      
    //*******************************************************************************
    //search for the next character from the mask that can be modified e.g. #
    //*******************************************************************************
	//control		- the curent textbox control that is edited
	//lastposition	- the position from wich the searching will be started
    //*******************************************************************************
    function AIP_getPositionOfMaskString (control, lastposition)
    {
		var position=lastposition;
		while(position < control.value.length)
		{
			if ( control.value.charAt(position) == '_' )
				break;
			position++;
		}
		//alert(position);
		return position;
    }
      
    //*******************************************************************************
	//set the cursor at a specific position     
	//*******************************************************************************
	//control	- the curent textbox control that is edited
	//position	- the current position
	//*******************************************************************************
	function AIP_setCursorAtPosition(control,position) 
	{
			//IE
			if ( document.selection && control.createTextRange )
			{
				var r = control.createTextRange();
				r.moveStart('character', position);
				r.collapse();
				r.select();
			}
			//MOZILLA
			else if (control.selectionStart || control.selectionStart == '0') 
			{
				control.curPos = position;
				control.selectionStart = position;
				control.selectionEnd = position;
			} 
			return true;
	}
		
	//*******************************************************************************
	//get the position of the currsor
	//*******************************************************************************
	//control	- the curent textbox control that is edited
	//*******************************************************************************
	function AIP_getCursorPosition(control)
	{
		var selection, rng, r2, i=-1;
		//IE
		if(document.selection && control.createTextRange) 
		{
			selection=document.selection;
			if(selection)
			{
				r2=selection.createRange();
				rng=control.createTextRange();
				rng.setEndPoint("EndToStart", r2);
				i=rng.text.length;
			}
		}
		//MOZILLA/NETSCAPE support
		else 
		{
			i = control.selectionStart;
		}
		return i;
	}
		
	//*******************************************************************************
	//check if a character in the control is selected so an edit or insert can be 
	//perfomred the descision is made if a character is selected and if it is selected 
	//before or after the delimiter symbol
	//*******************************************************************************
	//control	- the curent textbox control that is edited
	//*******************************************************************************
	function AIP_isTextSelected(control)
	{
		var selected = false, rng1, rng2, r1, r2;
		if (document.selection)
		{
			r1=document.selection.createRange();
			r2=document.selection.createRange();
			rng1=control.createTextRange();
			rng1.setEndPoint("EndToStart", r1);
			rng2=control.createTextRange();
			rng2.setEndPoint("EndToEnd", r2);
			if (rng1.text.length!=rng2.text.length)
				selected = true;
		}
		else
		{
			if ( control.selectionStart != control.selectionEnd )
				selected = true;
		}
		
		var precision = control.getAttribute("precision")
		var delimiter = control.getAttribute("delimiter");
		if ( precision != null && delimiter != null && !selected )
		{
			if (document.selection)
			{
				if (rng1.text.length >control.value.indexOf(delimiter) && 
					control.value.substring(control.value.indexOf(delimiter)+1, control.value.length).length == parseInt(precision))
					selected = true;
			}
			else
			{
				//alert(control.value.substring(control.value.indexOf(delimiter)+1, control.value.length).length);
				if ( control.selectionEnd > control.value.indexOf(delimiter)&& 
					control.value.substring(control.value.indexOf(delimiter)+1, control.value.length).length == parseInt(precision) &&
					control.value.indexOf(delimiter) != -1)
					{
						selected = true;
					}
			}				
		}
		return selected;
	}
		
	//*******************************************************************************
	//modify the codes for ',', '.', and the numbers from key pad. Also transform the
	//character to small or capital letter
	//*******************************************************************************
	//key	- the keyCode of the pressed key
	//e		- the event used in Mozilla for determine if a special key is pressed.
	//		  This feature is used for determine if a small or capital letter should be
	//		  shown on the screen.
	//*******************************************************************************
	function AIP_cleanKeyCode(key,e)
	{
		switch(key)
		{
			case 96: return "0"; break;
			case 97: return "1"; break;
			case 98: return "2"; break;
			case 99: return "3"; break;
			case 100: return "4"; break;
			case 101: return "5"; break;
			case 102: return "6"; break;
			case 103: return "7"; break;
			case 104: return "8"; break;
			case 105: return "9"; break;
			case 188: return ","; break;
			case 190: return "."; break;
			default: 
				var shift;
				var caps;
				var letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
				var numerics = "0123456789";
				if ( window.event )
				{
					shift = event.shiftKey;
					caps = event.capsKey;
				}
				else
				{
					shift = e.shiftKey;
					caps = e.capsKey;
				}
				var ret, keyTemp;
				keyTemp = String.fromCharCode(key); 
				
				if ( letters.indexOf(keyTemp) != -1 )
					if ( shift || caps )
						return  keyTemp; 
					else
						return String.fromCharCode(key+32);
				else
				{
					if ( numerics.indexOf(keyTemp) != -1 )
						return keyTemp;
				}
				
				//return ret;
				return String.fromCharCode(key); 
			break;
		}
	}
		
	//*******************************************************************************
	//For the boxes that uses some specifix character like currency 
	//symbol or percent. THe function is used in the process of timing
	//to get only the numbers from the text field so the logic of the 
	//main function do not have to change depends on the special character
	//*******************************************************************************
	//stringToTrim	- the value of the control
	//strings		- a string value that represents all strings that are
	//				  allowded to be entered in the current field 
	//				  (numerics, letters)
	//*******************************************************************************
	function AIP_getOnlyNumber ( stringToTrim, strings )
	{
		var index=0;
		var returnedValue = '';
		
		//check if the special character is in front
		var firstChar = false;
		if ( strings.indexOf(stringToTrim.charAt(0)) == -1 )
			firstChar = true;
			
		//moves through every character
		//if the special symbol is at the end, the while will stops when
		//the first no match occurs.
		//if the special symbol is in front, the while will start 
		//from there its position and will stopo when the first no match
		//after that occurs
		while( index < stringToTrim.length )
		{
			if (!firstChar)
			{
				if ( strings.indexOf(stringToTrim.charAt(index)) == -1 )
					break;
				returnedValue += stringToTrim.charAt(index);
			}
			else
			{
				if ( strings.indexOf(stringToTrim.charAt(index)) != -1 )
				{
					returnedValue += stringToTrim.charAt(index);
					firstChar = false;
				}
			}
			index++;
		}
		
		return returnedValue;
	}
		
	//*******************************************************************************
	//Show the pressed key related with the type of the browser. Also related with the
	//browser the maxLength property can be increase or decrease or both. This is mainly
	//included because of Mozilla.
	//*******************************************************************************
	//control			-  the textbox control that value should be changed
	//position			-  the current position at wich the character will be changed
	//changeLength		-  boolean variable, that indicate if the maxLength should be increase/decrease
	//increasePosition	-  after modification the control.value property, set if the current cursor
	//					   should be moved forward/backward related with the current position
	//*******************************************************************************
	function AIP_showCurrentCharacter( control, position, changeLength, valueToDisplay, increasePosition )
	{
		var x = control.value.substring(0,position);
		var y = control.value.substring(position+1,control.value.length);
		var increase;
		if ( increasePosition == null )
			increase = 0;
		else
			increase = increasePosition;
		if ( changeLength )
			control.maxLength = control.maxLength + 1;
		control.value = x + valueToDisplay + y;
		if ( changeLength )
			control.maxLength = control.maxLength - 1;
		AIP_setCursorAtPosition(control,position+increase);
		control.curPost = position+increase;
	}
		function AIP_paste(control)
		{
			// grab the textBox value and the mask
			var pastedVal = window.clipboardData.getData("Text");
			
			var mask = AIP_concatenateMask(control);
			var modeInput = control.getAttribute("modeInput");
			var newVal = '';
			var curPastedVal = 0;

			for(var i=0;i<mask.length;i++)
			{
				var currentMaskChar = mask.charAt(i);
				// if current mask pos allows entry
				if(currentMaskChar == '#')
				{
					var currentPastedChar = pastedVal.charAt(curPastedVal);
					// check each current mask char against new keystroke
					// return false if any are out of sync
					if(modeInput=="numeric") 
					{
						if('0123456789'.indexOf(currentPastedChar) == -1)
							return false;
					} 
					else if(modeInput=="character") 
					{
						if('abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'.indexOf(currentPastedChar) == -1)
							return false;
					} 
					else 
					{
						return false;
					}

					// add new key
					newVal += currentPastedChar;
					curPastedVal++;
				}
				else 
				{
					// add mask literal
					newVal += currentMaskChar;
				}
			}
			control.value = newVal;
			return false;
		}
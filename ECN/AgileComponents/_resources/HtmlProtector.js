var HPT_isIE4 = false;
var HPT_isNS4 = false;

if (document.layers)
	HPT_isNS4 = true;
	
if (document.all&&!document.getElementById)
	HPT_isIE4 = true;

function HPT_testIfScriptPresent()
{
}

function HPT_ClickIE4(){
	if (event.button==2){
		return false;
	}
}
 
function HPT_ClickNS4(zcb3dbc5516){
	if (document.layers || document.getElementById&&!document.all){
		if (zcb3dbc5516.which==2 || zcb3dbc5516.which==3){
			return false; 
		}
	}
}

function HPT_Test(){
	return true;
}

function HPT_ReturnFalse(zcb3dbc5516){
	return false;
}

function HPT_ReturnTrue(){
	return true;
}

function HPT_HidePrint(){
  for(z071c7b1348=0;z071c7b1348<document.all.length;z071c7b1348++)
  {
    if(document.all[z071c7b1348].style.visibility!="hidden")
    {
      document.all[z071c7b1348].style.visibility="hidden";document.all[z071c7b1348].id="bPrint";
    }
  }
}

function HPT_ShowPrint(){
  for(z071c7b1348=0;z071c7b1348<document.all.length;z071c7b1348++)
  {
    if(document.all[z071c7b1348].id=="bPrint")document.all[z071c7b1348].style.visibility="";
  }
}

function HPT_CopyNothing()
{
   if (HPT_isIE4)
   {
   document.getElementById("copiedNothing").createTextRange().execCommand("Copy");
   setTimeout("HPT_CopyNothing()",300);
   }
}

function HPT_HideStatus(){
	window.status='';
	return true;
}

function HPT_DisableDragAndDrop()
{
	document.ondragstart = HPT_ReturnFalse;
}

function HPT_DisableStatusLinks()
{
	if (HPT_isNS4)
		document.captureEvents(Event.MOUSEOVER | Event.MOUSEOUT);
	
	document.onmouseover = HPT_HideStatus;
	document.onmouseout = HPT_HideStatus;
}

function HPT_DisableClipBoard()
{
	//if(HPT_isIE4)
	HPT_CopyNothing();
}

function HPT_DisablePagePrinting()
{
	window.onbeforeprint = HPT_HidePrint;
	window.onafterprint = HPT_ShowPrint;
}

function DisableSelection(target)
{
    if (typeof target.onselectstart!="undefined") //IE route
	    target.onselectstart=function(){return false}
    else if (typeof target.style.MozUserSelect!="undefined") //Firefox route
	    target.style.MozUserSelect="none"
    else //All other route (ie: Opera)
	    target.onmousedown=function(){return false}
    target.style.cursor = "default"
}


function HPT_DisableRightClick()
{
	if (HPT_isNS4){
		document.captureEvents(Event.MOUSEDOWN);;
		document.onmousedown = HPT_ClickNS4;
	}
	else if (HPT_isIE4)
		document.onmousedown = HPT_ClickIE4;;

	document.oncontextmenu=new Function("return false");
}


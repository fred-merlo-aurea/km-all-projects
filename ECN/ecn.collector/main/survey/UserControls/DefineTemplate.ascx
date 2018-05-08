<%@ Control Language="c#" Inherits="ecn.collector.main.survey.UserControls.DefineTemplate"
    CodeBehind="DefineTemplate.ascx.cs" %>

<script type="text/javascript" src="../../scripts/Templatestyle.js"></script>

<style>
    .selcolor
    {
        cursor: hand;
    }
</style>
<style>
    #dimmer
    {
        position: fixed;
        top: 0;
        display: none;
        left: 0px;
        width: 100%;
        z-index: 10;
    }
    * html #dimmer
    {
        position: absolute;
    }
    html > /**/ body #dimmer
    {
        background: url(/ecn.images/images/gray.png) top left repeat;
    }
    * #dimmerIE
    {
        position: absolute;
        top: 0;
        display: none;
        left: 0px;
        width: 100%;
        z-index: 2;
        _background-image: none;
        _filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled=true, sizingMethod=scale src= '/ecn.images/images/gray.png' );
    }
    #dimmerContainer
    {
        position: fixed;
        top: 0;
        display: none;
        left: 0px;
        font-family: Arial;
        font-weight: bold;
        width: 100%;
        z-index: 100;
    }
    * html #dimmerContainer
    {
        position: absolute;
    }
    div.dimming
    {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 11px;
        font-style: normal;
        position: relative;
    }
    #divTemplate, #divSaveTemplate
    {
        /*display:none;*/
    }
    .descWiz
    {
        font-family: "Arial";
        font-size: 12px;
        padding: 6px;
    }
    .descWizsmall
    {
        font-family: "Arial";
        font-size: 11px;
        padding: 6px;
    }
    .dimmerInner
    {
        max-height: 300px;
        overflow: auto;
    }
    *html .dimmerInner
    {
        height: 300px;
        overflow: auto;
    }
    #SurveyWizard_dlTemplates input, #SurveyWizard_dlTemplates img
    {
        border: 1px #ccc solid;
    }
</style>
<!--[if gte IE 5.5]>
		<style type="text/css">
div#dimmerContainer {
left: expression( ( 0 + ( ignoreMe2 = document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ) ) + 'px' );
top: expression( ( 0 + ( ignoreMe = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ) ) + 'px' );
}

		</style>
		<![endif]-->

<script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/showHideTag.js"></script>

<script language="JavaScript">

function clearFormData () {
if (document.getElementById("SurveyWizard_txtTemplateName")) {
	document.getElementById("SurveyWizard_txtTemplateName").value = "";
  }
  
if (document.getElementById("SurveyWizard_chkTDefault")) {
	document.getElementById("SurveyWizard_chkTDefault").checked = false;
  }
  
  
  
}

var linkIDs = new Array("ButtonOne","ButtonTwo","ButtonThree","ButtonFour")
   
    function switchClass(d)
    {
        document.getElementById("link" + d).className='sel';
        for (var i in linkIDs)
            if (linkIDs[i] != d)
                document.getElementById("link" + linkIDs[i]).className='def';
      
    }

function getobj(id) {
    if (id.indexOf("SurveyWizard_") != -1) {
        id = 'ctl00_ContentPlaceHolder1_' + id;
    }
  if (document.all && !document.getElementById) 
    obj = eval('document.all.' + id);
  else if (document.layers) 
    obj = eval('document.' + id);
  else if (document.getElementById) 
    obj = document.getElementById(id);

  return obj;
}

function checkKeyPressForInteger(control, oEvent){
		var keyCode;
		
		if(window.event) {
			// for IE, e.keyCode or window.event.keyCode can be used
			keyCode = oEvent.keyCode; 
		}
		else if(oEvent.which)
		{	// for Netscape and Mozilla
			keyCode = oEvent.which;
		}
				
		//Minus is allowed only as first char
		if (keyCode == 45){
			oEvent.returnValue = false;
			return false;
		}

		//only 0..9,-,backspace and non-keycode chars(del, arrows, ...)
		if((keyCode < 48 || keyCode > 57) && (keyCode != 45) && (keyCode != 8) && (keyCode != 0)) {
			oEvent.returnValue = false;
			return false;
		}
}

function ColorTable_onclick(e)
{
	if (!e)  e = window.event;
	
	if (e.target) 
		targ = e.target;
	else if (e.srcElement) 
		targ = e.srcElement;

	getobj('selhicolor').style.backgroundColor = targ.title;
	getobj('selcolor').value = targ.title;
}
function ColorTable_onmouseover(e)
{
	if (!e)  e = window.event;
	
	if (e.target) 
		targ = e.target;
	else if (e.srcElement) 
		targ = e.srcElement;
		
	getobj('hicolortext').innerText = targ.title;
	getobj('hicolor').style.backgroundColor = targ.title;
}
function ColorTable_onmouseout(e)
{
	if (!e)  e = window.event;
	
	if (e.target) 
		targ = e.target;
	else if (e.srcElement) 
		targ = e.srcElement;
		
  getobj('hicolortext').innerText = "";
  getobj('hicolor').style.backgroundColor = "";
}

function selcolor_onpropertychange()
{
  try
	{
		getobj('selhicolor').style.backgroundColor = selcolor.value;
	}
  catch(e) {}
  }
			
dFeatures = 'dialogHeight: 250px; dialogWidth: 370px; dialogTop: 200px; dialogLeft: 4px; edge: Raised; center: Yes; help: no; resizable: no; status: no;';//default features
modalWin = "";

var g_img;
var g_txt;
var g_tag;
var g_class;
var g_prop;
var g_def;

function opencolorpallette(img, txt, tag, cls, prop, def)
{
	g_img = img;
	g_txt = txt;
	g_tag = tag;
	g_class = cls;
	g_prop = prop;
	g_def = def;
	
	var x = getPosition(img).x + 25;
	var y = getPosition(img).y - 100;

	getobj('divpalette').style.left = x;
	getobj('divpalette').style.top = y;		
	getobj('divpalette').style.display='block';

}

function setcolor(img, txt, tag, cls, prop, def)
{
	var selcolor = txt.value;
	if(selcolor!="") 
	{	
		var imgctrl = getobj(img);
		try
		{
			imgctrl.style.backgroundColor=  selcolor;
			setStyleByClass(tag, cls, prop, selcolor + def);
		}
		 catch(e) 
		{
			alert('Invalid Color');
		}
	}
}

function apply()
{
	var selcolor = getobj('selcolor').value;
	if(selcolor!="undefined" && selcolor.substring(0,1) == "#") 
	{	
		g_img.style.backgroundColor=  selcolor;
		var txtbox = getobj(g_txt);
		txtbox.value = selcolor;
		setStyleByClass(g_tag, g_class, g_prop, selcolor + g_def);
	}
	cleanup();
}
function btncancel_onclick()
{
	cleanup();
}
function cleanup()
{
	g_img = null;
	g_txt = "";
	g_tag = "";
	g_class = "";
	g_prop = "";
	g_def = "";
	getobj('hicolortext').innerText = "";
	getobj('hicolor').style.backgroundColor = "";
	getobj('selhicolor').style.backgroundColor = '';
	getobj('selcolor').value='';
	getobj('divpalette').style.display='none';
}

function getPosition(img) {
	// This function will return an Object with x and y properties
	var useWindow=false;
	var coordinates=new Object();
	var x=0,y=0;
	// Browser capability sniffing
	var use_gebi=false, use_css=false, use_layers=false;
	if (document.getElementById) { use_gebi=true; }
	else if (document.all) { use_css=true; }
	else if (document.layers) { use_layers=true; }
	// Logic to find position
 	if (use_gebi && document.all) {
		x=getPageOffsetLeft(img);
		y=getPageOffsetTop(img);
		}
	else if (use_gebi) {
		var o=img;
		x=getPageOffsetLeft(o);
		y=getPageOffsetTop(o);
		}
 	else if (use_css) {
		x=getPageOffsetLeft(img);
		y=getPageOffsetTop(img);
		}
	else {
		coordinates.x=0; coordinates.y=0; return coordinates;
		}
	coordinates.x=x;
	coordinates.y=y;
	return coordinates;
	}


// Functions for IE to get position of an object
function getPageOffsetLeft (el) {
	var ol=el.offsetLeft;
	while ((el=el.offsetParent) != null) { ol += el.offsetLeft; }
	return ol;
	}
function AnchorPosition_getWindowOffsetLeft (el) {
	return getPageOffsetLeft(el)-document.body.scrollLeft;
	}	
function getPageOffsetTop (el) {
	var ot=el.offsetTop;
	while((el=el.offsetParent) != null) { ot += el.offsetTop; }
	return ot;
	}
function AnchorPosition_getWindowOffsetTop (el) {
	return getPageOffsetTop(el)-document.body.scrollTop;
	}

function getwidthvalue(v)
{
	if (v=='600px') 
		return "600px";
	else if (v=='700px') 
		return "700px";
	else if (v=='800px') 
		return "800px";
	else 
		return "100%";
}

	
function loadfunctions()
{
	setStyleByClass('div', 'outertable', 'width', getwidthvalue(getobj('SurveyWizard_pWidth').value))
	try{getobj('IMGpbgcolor').style.backgroundColor=  getobj('SurveyWizard_pbgcolor').value;}catch(e){}
	try{getobj('IMGpBordercolor').style.backgroundColor=  getobj('SurveyWizard_pBordercolor').value;}catch(e){}
	try{getobj('IMGbbgcolor').style.backgroundColor=  getobj('SurveyWizard_bbgcolor').value;}catch(e){}
	try{getobj('IMGhbgcolor').style.backgroundColor=  getobj('SurveyWizard_hbgcolor').value;}catch(e){}
	try{getobj('IMGfbgcolor').style.backgroundColor=  getobj('SurveyWizard_fbgcolor').value;}catch(e){}
	try{getobj('IMGphbgcolor').style.backgroundColor=  getobj('SurveyWizard_phbgcolor').value;}catch(e){}
	try{getobj('IMGphcolor').style.backgroundColor=  getobj('SurveyWizard_phcolor').value;}catch(e){}
	try{getobj('IMGpdbgcolor').style.backgroundColor=  getobj('SurveyWizard_pdbgcolor').value;}catch(e){}
	try{getobj('IMGpdcolor').style.backgroundColor=  getobj('SurveyWizard_pdcolor').value;}catch(e){}
	try{getobj('IMGqcolor').style.backgroundColor=  getobj('SurveyWizard_qcolor').value;}catch(e){}
	try{getobj('IMGacolor').style.backgroundColor=  getobj('SurveyWizard_acolor').value;}catch(e){}
	
}

function showPreSection(d)
{
	var divs=new Array("divSurveyStyles", "divheaderfooter", "divPageheaderdesc", "divQuestionAnswer");
	
	for(i=0; i<divs.length; i++)
	{
		if (divs[i] == d)
		{
			getobj(divs[i]).style.display='block';	
		}	
		else
			getobj(divs[i]).style.display='none';		
	}
	
}

function setDimmerSize()
	{	
		var winW = 630, winH = 460;

	if (parseInt(navigator.appVersion)>3) {
	 if (navigator.appName=="Netscape") {
	  winW = window.innerWidth;
	  winH = window.innerHeight;
	 }
	 if (navigator.appName.indexOf("Microsoft")!=-1) {
	  winW = document.body.offsetWidth;
	  winH = document.body.offsetHeight;
	 }
	}
	
	Dimmer = document.getElementById("dimmer")
	Dimmer.style.height = winH + 'px';
	Dimmer.style.width = winW + 'px';
	
	if (document.getElementById("dimmerIE")){
		DimmerTwo = document.getElementById("dimmerIE")
		DimmerTwo.style.height = winH + 'px';
		DimmerTwo.style.width = winW + 'px';
	}
	
	DimmerContainer = document.getElementById("dimmerContainer")
	DimmerContainer.style.height = winH + 'px';
	DimmerContainer.style.width = winW + 'px';
	
	ContCell = document.getElementById("containerCell")
	ContCell.style.height = winH + 'px';
	}
	
	function hideBody()
	{
		var browser=navigator.appName
		if (browser=="Microsoft Internet Explorer")
		{	
			scroll(0,0);
			bodyElement = document.getElementsByTagName('body')[0];
			bodyElement.style.overflow = 'hidden';
			document.getElementById("dimmerIE").style.display="block";
		}
	}
	
	function showBody()
	{
		var browser=navigator.appName
		if (browser=="Microsoft Internet Explorer")
		{
			bodyElement = document.getElementsByTagName('body')[0];
			bodyElement.style.overflow = 'auto';
			document.getElementById("dimmerIE").style.display="none";
		}
	}



function ShowTemplate()
{
	getobj("dimmer").style.display='block';
	getobj("dimmerContainer").style.display='block';
	getobj("divTemplate").style.display='block';
	setDimmerSize();
	hideBody();
	hideAllByTag('select');
}

function CloseTemplate()
{
	getobj("divTemplate").style.display='none';
	getobj("dimmerContainer").style.display='none';
	getobj("dimmer").style.display='none';
	showBody();
	showAllByTag('select','inline');
}

function ShowSaveTemplate()
{
	getobj("dimmer").style.display='block';
	getobj("dimmerContainer").style.display='block';
	getobj("divSaveTemplate").style.display='block';
	setDimmerSize();
	hideBody();
	hideAllByTag('select');
}

function CloseSaveTemplate()
{
	getobj("divSaveTemplate").style.display='none';
	getobj("dimmerContainer").style.display='none';
	getobj("dimmer").style.display='none';
	showBody();
	showAllByTag('select','inline');
}

function ShowConfirmReset()
{
	getobj("dimmer").style.display='block';
	getobj("dimmerContainer").style.display='block';
	getobj("divReset").style.display='block';
	setDimmerSize();
	hideBody();
	hideAllByTag('select');
}

function CloseConfirmReset()
{
	getobj("divReset").style.display='none';
	getobj("dimmerContainer").style.display='none';
	getobj("dimmer").style.display='none';
	showBody();
	showAllByTag('select','inline');
}


String.prototype.trim = function() { return this.replace(/^\s+|\s+$/, ''); };

	function tValidate()	
	{
	
		if (getobj("SurveyWizard_txtTemplateName").value.trim() == "")
		{
			getobj("spntemplatename").style.visibility="visible";
			return false;	
		}
		
		return true;	
		
	}

var imagetype="";
function BrowseImages(mode)
{
	imagetype = mode;
	window.open('/ecn.editor/editor/filemanager/fileManager.aspx','','scrollbars=yes,resizable=yes,status=yes,width=950,height=550')
}

function setImage(url)
{
	var newImg = new Image();
	newImg.src = url;
	var height = newImg.height;
	var width = newImg.width;

	//alert(width + ' / ' +  height );

	if (imagetype == 'header')
	{
		getobj("SurveyWizard_hImage").value = newImg.src;
		getobj("SurveyWizard_imgHeader").src= url;
	}	
	else
	{
		getobj("SurveyWizard_fImage").value = newImg.src;	
		getobj("SurveyWizard_imgFooter").src = url;
	}
}

</script>

<script language='javascript'>onresize=function(){setDimmerSize()};</script>

<asp:Label ID="plstyles" runat="server" EnableViewState="true"></asp:Label>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="center" valign="top">
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <asp:PlaceHolder ID="plTemplates" runat="server">
                    <tr>
                        <td width="20">
                            <div style="width: 20px;">
                            </div>
                        </td>
                        <td height="40" valign="middle">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="50%" align="left">
                                        <a href="javascript:void(0);" alt="" style="cursor: hand;" class="ltButton" onclick="return ShowTemplate();">
                                            <span>Select Template</span></a>
                                    </td>
                                    <td width="50%" align="right" style="padding-right: 20px;">
                                        <a href="javascript:void(0);" class="ltButton" onclick="ShowConfirmReset();"><span>Reset
                                            Template</span></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" height="40" valign="middle">
                            <a href="javascript:void(0);" style="cursor: hand;" onclick="return ShowSaveTemplate();"
                                class="ltButton"><span>Save As Template</span></a>
                        </td>
                    </tr>
                </asp:PlaceHolder>
                <tr>
                    <td width="20">
                        <div style="width: 20px;">
                        </div>
                    </td>
                    <asp:PlaceHolder ID="plToobar" runat="server">
                        <td valign="top" align="left" width="328">
                            <div style="width: 328px;" id="templateUI">
                                <table cellspacing="0" width="308" cellpadding="0" border="0" class="survStepThree">
                                    <tr class="top">
                                        <td width="153" height="23" align="center">
                                            <a href="javascript:void(0);" id="linkButtonOne" class="sel" onclick="javascript:showPreSection('divSurveyStyles');javascript:switchClass('ButtonOne');">
                                                Survey Layout</a>
                                        </td>
                                        <td width="153" height="23" align="center">
                                            <a href="javascript:void(0);" id="linkButtonTwo" class="def" onclick="javascript:showPreSection('divheaderfooter');javascript:switchClass('ButtonTwo');">
                                                Survey Header/Footer</a>
                                        </td>
                                    </tr>
                                    <tr class="top">
                                        <td align="center" height="23">
                                            <a href="javascript:void(0);" id="linkButtonThree" class="def" onclick="javascript:showPreSection('divPageheaderdesc');javascript:switchClass('ButtonThree');">
                                                Page Header/Desc.</a>
                                        </td>
                                        <td align="center" height="23">
                                            <a href="javascript:void(0);" id="linkButtonFour" class="def" onclick="javascript:showPreSection('divQuestionAnswer');javascript:switchClass('ButtonFour');">
                                                Q. &amp; A.</a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" height="5" style="background: #808080;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="greySidesB" style="background: #fff;" valign="top" height="560">
                                            <div id="divSurveyStyles" style="display: block">
                                                <table cellspacing="0" width="100%" cellpadding="2" border="0">
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Window Background Color
                                                        </td>
                                                        <td align="left" width="153">
                                                            <asp:TextBox ID="pbgcolor" onblur="javascript:setcolor('IMGpbgcolor', this, 'table', 'surveybody', 'background', '');"
                                                                runat="server" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGpbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_pbgcolor', 'table', 'surveybody', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td class="surveyStylesLabel">
                                                            Page Font
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pfont" runat="server" CssClass="survFormSize" onChange="javascript:setStyleByClass('table', 'surveybody', 'fontFamily', this.value);">
                                                                <asp:ListItem Value="arial">Arial</asp:ListItem>
                                                                <asp:ListItem Value="Courier New">Courier New</asp:ListItem>
                                                                <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                                                                <asp:ListItem Value="Trebuchet MS">Trebuchet MS</asp:ListItem>
                                                                <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                                                <asp:ListItem Value="Tahoma">Tahoma</asp:ListItem>
                                                                <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="surveyStylesLabel">
                                                            Page Border
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pborder" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'outertable', 'border', getobj('SurveyWizard_pBordercolor').value==''?'#000000':getobj('SurveyWizard_pBordercolor').value + ' ' + this.value + 'px solid');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td class="surveyStylesLabel">
                                                            Page Border Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="pBordercolor" onblur="javascript:setcolor('IMGpBordercolor',this, 'div', 'outertable', 'border', ' ' + getobj('SurveyWizard_pborder').value + 'px solid')"
                                                                runat="server" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGpBordercolor" style="background-color: #000000"
                                                                        onclick="javascript:opencolorpallette(this,'SurveyWizard_pBordercolor', 'div', 'outertable', 'border', ' ' + getobj('SurveyWizard_pborder').value + 'px solid')"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="surveyStylesLabel">
                                                            Page Width
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pWidth" runat="server" CssClass="survFormSize" onChange="javascript:setStyleByClass('div', 'outertable', 'width', getwidthvalue(this.value));">
                                                                <asp:ListItem Value="600px">600px</asp:ListItem>
                                                                <asp:ListItem Value="700px">700px</asp:ListItem>
                                                                <asp:ListItem Value="800px">800px</asp:ListItem>
                                                                <asp:ListItem Value="100%">Full Screen</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td class="surveyStylesLabel">
                                                            Page Alignment
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pAlign" runat="server" CssClass="surveyDropDowns" onChange="javascript:getobj('surveybodytd').style.textAlign=this.value;setStyleByClass('div', 'outertable', 'margin', (this.value=='left'?'0px':'0px auto'));">
                                                                <asp:ListItem Value="left">left</asp:ListItem>
                                                                <asp:ListItem Value="center">center</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="surveyStylesLabel">
                                                            Page Background Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="bbgcolor" onblur="javascript:setcolor('IMGbbgcolor', this, 'div', 'surveytable', 'background', '');"
                                                                runat="server" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGbbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_bbgcolor', 'div', 'surveytable', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divheaderfooter" style="display: none">
                                                <table cellspacing="0" cellpadding="2" width="100%" border="0" style="font-size: 10px;
                                                    font-family: Arial">
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Survey Header</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Choose Image
                                                        </td>
                                                        <td align="left" width="153">
                                                            <a href="javascript:void(0);" onclick="javascript:BrowseImages('header');" class="ltButton"
                                                                style="width: 150px;"><span>Browse Header Image</span></a>
                                                            <div style="display: none;">
                                                                <asp:TextBox ID="hImage" runat="server" CssClass="survFormSize"></asp:TextBox>&nbsp;</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Background Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="hbgcolor" onblur="javascript:setcolor('IMGhbgcolor', this, 'div', 'divHeader', 'background', '');"
                                                                runat="server" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGhbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_hbgcolor', 'div', 'divHeader', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Margin
                                                        </td>
                                                        <td align="left">
                                                            <div class="marginContainer">
                                                                <div class="marginTop">
                                                                    <label class="survMarginLabel" for="htopmargin">
                                                                        Top:</label>
                                                                    <asp:TextBox ID="htopmargin" onblur="javascript:setStyleByClass('img', 'divHeaderIMG', 'marginTop', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" CssClass="survMarginFeild" MaxLength="2"></asp:TextBox>
                                                                </div>
                                                                <div class="marginLeft">
                                                                    <label class="survMarginLabel" for="hleftmargin">
                                                                        Left:</label>
                                                                    <asp:TextBox ID="hleftmargin" onblur="javascript:setStyleByClass('img', 'divHeaderIMG', 'marginLeft', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" CssClass="survMarginFeild" MaxLength="2"></asp:TextBox>
                                                                </div>
                                                                <div class="marginRight">
                                                                    <label class="survMarginLabel" for="hrightmargin">
                                                                        Right:</label>
                                                                    <asp:TextBox ID="hrightmargin" onblur="javascript:setStyleByClass('img', 'divHeaderIMG', 'marginRight', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" CssClass="survMarginFeild" MaxLength="2"></asp:TextBox>
                                                                </div>
                                                                <div class="marginBottom">
                                                                    <label class="survMarginLabel" for="hbottommargin">
                                                                        Bottom:</label>
                                                                    <asp:TextBox ID="hbottommargin" onblur="javascript:setStyleByClass('img', 'divHeaderIMG', 'marginBottom', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" CssClass="survMarginFeild" MaxLength="2"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Alignment
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="halign" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divHeader', 'textAlign', this.value);">
                                                                <asp:ListItem Value="left">left</asp:ListItem>
                                                                <asp:ListItem Value="center">center</asp:ListItem>
                                                                <asp:ListItem Value="right">right</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Survey Footer</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Choose Image
                                                        </td>
                                                        <td align="left">
                                                            <a href="javascript:void(0);" onclick="javascript:BrowseImages('footer');" class="ltButton"
                                                                style="width: 150px;"><span>Browse Footer Image</span></a>
                                                            <div style="display: none;">
                                                                <asp:TextBox ID="fImage" runat="server" CssClass="survFormSize"></asp:TextBox>&nbsp;</div>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Background Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="fbgcolor" onblur="javascript:setcolor('IMGfbgcolor', this, 'div', 'divFooter', 'background', '');"
                                                                runat="server" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGfbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_fbgcolor', 'div', 'divFooter', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Margin
                                                        </td>
                                                        <td align="left">
                                                            <div class="marginContainer">
                                                                <div class="marginTop">
                                                                    <label class="survMarginLabel" for="ftopmargin">
                                                                        Top:</label>
                                                                    <asp:TextBox ID="ftopmargin" onblur="javascript:setStyleByClass('img', 'divFooterIMG', 'marginTop', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" Width="20px" CssClass="survMarginFeild"></asp:TextBox>
                                                                </div>
                                                                <div class="marginLeft">
                                                                    <label class="survMarginLabel" for="fleftmargin">
                                                                        Left:</label>
                                                                    <asp:TextBox ID="fleftmargin" onblur="javascript:setStyleByClass('img', 'divFooterIMG', 'marginLeft', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" Width="20px" CssClass="survMarginFeild"></asp:TextBox>
                                                                </div>
                                                                <div class="marginRight">
                                                                    <label class="survMarginLabel" for="frightmargin">
                                                                        Right:</label>
                                                                    <asp:TextBox ID="frightmargin" onblur="javascript:setStyleByClass('img', 'divFooterIMG', 'marginRight', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" Width="20px" CssClass="survMarginFeild"></asp:TextBox>
                                                                </div>
                                                                <div class="marginBottom">
                                                                    <label class="survMarginLabel" for="fbottommargin">
                                                                        Bottom:</label>
                                                                    <asp:TextBox ID="fbottommargin" onblur="javascript:setStyleByClass('img', 'divFooterIMG', 'marginBottom', (this.value==''?'0px':this.value+'px'));"
                                                                        runat="server" Width="20px" CssClass="survMarginFeild"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Alignment
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="falign" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divFooter', 'textAlign', this.value);">
                                                                <asp:ListItem Value="left">left</asp:ListItem>
                                                                <asp:ListItem Value="center">center</asp:ListItem>
                                                                <asp:ListItem Value="right">right</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divPageheaderdesc" style="display: none">
                                                <table cellspacing="0" cellpadding="2" border="0" width="100%" style="font-size: 10px;
                                                    font-family: Arial">
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Page Header</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Background Color
                                                        </td>
                                                        <td align="left" width="153">
                                                            <asp:TextBox ID="phbgcolor" onblur="javascript:setcolor('IMGphbgcolor', this, 'div', 'divpageHeader', 'background', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGphbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_phbgcolor', 'div', 'divpageHeader', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="phcolor" onblur="javascript:setcolor('IMGphcolor', this, 'div', 'divpageHeader', 'color', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGphcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_phcolor', 'div', 'divpageHeader', 'color', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Size
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="phfontsize" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divpageHeader', 'fontSize', this.value);">
                                                                <asp:ListItem Value="10px">10 px</asp:ListItem>
                                                                <asp:ListItem Value="11px">11 px</asp:ListItem>
                                                                <asp:ListItem Value="12px">12 px</asp:ListItem>
                                                                <asp:ListItem Value="13px">13 px</asp:ListItem>
                                                                <asp:ListItem Value="14px">14 px</asp:ListItem>
                                                                <asp:ListItem Value="15px">15 px</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Bold
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="phbold" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divpageHeader', 'fontWeight', this.value==0?'normal':'bold');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Page Description</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Background Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="pdbgcolor" onblur="javascript:setcolor('IMGpdbgcolor', this, 'div', 'divpageDesc', 'background', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGpdbgcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_pdbgcolor', 'div', 'divpageDesc', 'background', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="pdcolor" onblur="javascript:setcolor('IMGpdcolor', this, 'div', 'divpageDesc', 'color', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGpdcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_pdcolor', 'div', 'divpageDesc', 'color', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Size
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pdfontsize" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divpageDesc', 'fontSize', this.value);">
                                                                <asp:ListItem Value="10px">10 px</asp:ListItem>
                                                                <asp:ListItem Value="11px">11 px</asp:ListItem>
                                                                <asp:ListItem Value="12px">12 px</asp:ListItem>
                                                                <asp:ListItem Value="13px">13 px</asp:ListItem>
                                                                <asp:ListItem Value="14px">14 px</asp:ListItem>
                                                                <asp:ListItem Value="15px">15 px</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Bold
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="pdbold" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'divpageDesc', 'fontWeight', this.value==0?'normal':'bold');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divQuestionAnswer" style="display: none">
                                                <table cellspacing="0" cellpadding="2" border="0" width="100%" style="font-size: 10px;
                                                    font-family: Arial">
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Question</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Display Question #
                                                        </td>
                                                        <td align="left">
                                                         <asp:DropDownList ID="drpShowQuestionNO" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('span', 'questionno', 'visibility', this.value==0?'hidden':'visible');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1" Selected=True>Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Color
                                                        </td>
                                                        <td align="left" width="153">
                                                            <asp:TextBox ID="qcolor" onblur="javascript:setcolor('IMGqcolor',this,  'div', 'question', 'color', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGqcolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_qcolor', 'div', 'question', 'color', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Size
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="qfontsize" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'question', 'fontSize', this.value);">
                                                                <asp:ListItem Value="10px">10 px</asp:ListItem>
                                                                <asp:ListItem Value="11px">11 px</asp:ListItem>
                                                                <asp:ListItem Value="12px">12 px</asp:ListItem>
                                                                <asp:ListItem Value="13px">13 px</asp:ListItem>
                                                                <asp:ListItem Value="14px">14 px</asp:ListItem>
                                                                <asp:ListItem Value="15px">15 px</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Bold
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="qbold" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'question', 'fontWeight', this.value==0?'normal':'bold');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel" colspan="2" style="padding-right: 15px;">
                                                            <p style="border-bottom: 1px #A4A2A3 solid; margin: 0; padding: 0;">
                                                                <strong>Answer</strong></p>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Color
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="acolor" onblur="javascript:setcolor('IMGacolor', this, 'div', 'answer', 'color', '');"
                                                                runat="server" Width="75px" CssClass="survFormSize"></asp:TextBox><a href="javascript:void(0);"
                                                                    class="colorPallette"><img id="IMGacolor" style="background-color: #000000" onclick="javascript:opencolorpallette(this, 'SurveyWizard_acolor', 'div', 'answer', 'color', '');"
                                                                        src="/ecn.images/images/ColorPad.gif" align="middle" border="0"></a>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Size
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="afontsize" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'answer', 'fontSize', this.value);">
                                                                <asp:ListItem Value="10px">10 px</asp:ListItem>
                                                                <asp:ListItem Value="11px">11 px</asp:ListItem>
                                                                <asp:ListItem Value="12px">12 px</asp:ListItem>
                                                                <asp:ListItem Value="13px">13 px</asp:ListItem>
                                                                <asp:ListItem Value="14px">14 px</asp:ListItem>
                                                                <asp:ListItem Value="15px">15 px</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="survStyleAltRow">
                                                        <td align="left" class="surveyStylesLabel">
                                                            Font Bold
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="abold" runat="server" CssClass="surveyDropDowns" onChange="javascript:setStyleByClass('div', 'answer', 'fontWeight', this.value==0?'normal':'bold');">
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </asp:PlaceHolder>
                    <td valign="top" align="left" width="100%" height="100%">
                        <table class="surveybody" cellspacing="0" cellpadding="0" width="100%" height="614"
                            border="0">
                            <tr>
                                <td align="left" class="headingTwo gradient">
                                    <p style="padding: 0 0 0 5px; margin: 0">
                                        Style Preview</p>
                                </td>
                            </tr>
                            <tr>
                                <td id="surveybodytd" class="greySidesB" width="100%" valign="top">
                                    <div id="templatePreviewer" style="width: 530px; overflow: auto; height: 584px;">
                                        <div style="width: 840px;">
                                            <div class="outertable">
                                                <div class="divHeader">
                                                    <asp:Image ID="imgHeader" CssClass="divHeaderIMG" runat="server"></asp:Image></div>
                                                <div class="divpageHeader">
                                                    Page Header</div>
                                                <div class="divpageDesc">
                                                    Page Description</div>
                                                <div class="surveytable">
                                                    <div class="question">
                                                        <span class="questionno">1.</span> Question 1?</div>
                                                    <div class="answer">
                                                        <input type="checkbox">Yes
                                                        <input type="checkbox">No</div>
                                                    <br>
                                                    <div class="question">
                                                        <span class="questionno">2.</span> Question 2?</div>
                                                    <div class="answer">
                                                        <input type="text" width="100"></div>
                                                    <br>
                                                    <br>
                                                </div>
                                                <div class="divFooter">
                                                    <asp:Image ID="imgFooter" CssClass="divFooterIMG" runat="server"></asp:Image></div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20">
                        <div style="width: 20px;">
                        </div>
                    </td>
                </tr>
            </table>
            <div id="divpalette" style="border: 1px solid black; padding: 5px; display: none; z-index: 101; background-color: rgb(204, 204, 204); left: 795px; width: 370px; position: absolute; height: 225px; top: 303px; background-position: initial initial; background-repeat: initial initial;">
            <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td id="ColorTableCell" valign="top" nowrap align="left">
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td valign="top" nowrap align="center">
                            <input style="margin-bottom: 6px; width: 75px; height: 22px" onclick="apply();" type="button"
                                value="OK" name="btnOK"><br>
                            <input style="margin-bottom: 6px; width: 75px; height: 22px" onclick="javascript:btncancel_onclick();"
                                type="button" value="Cancel" name="btnCancel"><br>
                            <span>Highlight</span>:
                            <div id="hicolor" style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid;
                                width: 74px; border-bottom: 1px solid; height: 20px">
                            </div>
                            <div id="hicolortext" style="margin-bottom: 7px; width: 75px; text-align: right">
                            </div>
                            <span>Selected</span>:
                            <div id="selhicolor" style="border-right: 1px solid; border-top: 1px solid; border-left: 1px solid;
                                width: 74px; border-bottom: 1px solid; height: 20px">
                            </div>
                            <input id="selcolor" style="margin-top: 0px; margin-bottom: 7px; width: 75px; height: 20px"
                                type="text" maxlength="20" onchange="selcolor_onpropertychange()">
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
<br>

<script type="text/javascript">
	var ac=['00','33','66','99','cc','ff'];
	var txt='<table ID="ColorTable" border="0" cellspacing="0" cellpadding="0" width="270" class="selcolor" onclick="javascript:ColorTable_onclick(event)" onmouseover="javascript:ColorTable_onmouseover(event)" onmouseout="javascript:ColorTable_onmouseout(event)">';

	for (var i=0; i<3;i++){txt+='<tr>';for (var j=0; j<3;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
	for (var i=3; i<6;i++){txt+='<tr>';for (var j=0; j<3;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
	for (var i=0; i<3;i++){txt+='<tr>';for (var j=3; j<6;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
	for (var i=3; i<6;i++){txt+='<tr>';for (var j=3; j<6;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
	txt+='<tr>';for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[n]+ac[n]+ac[n]+'"title="#'+ac[n]+ac[n]+ac[n]+'"></td>';}
	for (var i=0;i<12;i++){txt+='<td style="height:15px;width:15px;" bgcolor="#000000" title="#000000"></td>';}
	txt+='</tr>';
	txt+='</table>';
	getobj('ColorTableCell').innerHTML = txt ;
</script>

<script type="text/javascript">
    var browser = navigator.appName;
	if (browser=="Microsoft Internet Explorer") {
	  document.write('<div id="dimmerIE"></div>');
   	}
</script>

<div id="dimmer">
    <div id="dimmerContainer">
        <table cellpadding="0" cellspacing="0" width="100%" height="100%" style="position: relative;">
            <tr>
                <td align="center" valign="middle" id="containerCell">
                    <div class="dimming" id="divTemplate" style="display: none; width: 750px; height: 400px;">
                        <table style="width: 750px" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Select Template</span>
                                </td>
                                <td class="dimTopRight">
                                    <a href="javascript:void(0);" onclick="javascript:CloseTemplate();" style="cursor: pointer;">
                                        <img src="/ecn.images/images/divs_05.gif" name="Image2" id="closee" alt="Close" border="0"></a>
                                </td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft">
                                </td>
                                <td class="dimMiddleCenter">
                                    <div class="overFlowDimmer" style="position: relative; left: 25px; margin-top: 5px;">
                                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
                                            <tr>
                                                <td bgcolor="#ffffff" valign="top">
                                                    <div style="padding-left: 25px; font-size: 11px; padding-bottom: 5px; padding-top: 5px"
                                                        align="left">
                                                        Choose a template below by clicking on the thumbnail.</div>
                                                    <div id="templatesDiv" style="padding-right: 0px; padding-left: 0px; padding-bottom: 10px;
                                                        padding-top: 10px" align="left">
                                                        <asp:DataList ID="dlTemplates" runat="server" CellPadding="3" RepeatColumns="3" RepeatDirection="Horizontal"
                                                            OnItemCommand="dlTemplates_itemcommand" Width="100%" DataKeyField="TemplateID"
                                                            CssClass="dimmerLabel" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ImageUrl='<%# DataBinder.Eval(Container.DataItem, "TemplateImage") %>'
                                                                    AlternateText='' runat="server" CommandName="SELECT" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'
                                                                    BorderColor="#000000" BorderStyle="Solid" BorderWidth="1" ID="Imagebutton1">
                                                                </asp:ImageButton><br>
                                                                <%# DataBinder.Eval(Container.DataItem, "TemplateName") %>
                                                                <br>
                                                                <br />
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td class="dimMiddleRight" width="53">
                                </td>
                            </tr>
                            <tr>
                                <td class="dimBottomLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimBottomCenter">
                                </td>
                                <td class="dimBottomRight">
                                    <div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="dimming" id="divSaveTemplate" style="display: none; width: 300px; height: 200px;">
                        <table style="width: 300px" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Save Template
                                </td>
                                <td class="dimTopRight">
                                    <a href="javascript:void(0);" onclick="javascript:CloseSaveTemplate();clearFormData();"
                                        style="cursor: pointer">
                                        <img src="/ecn.images/images/divs_05.gif" name="Image2" alt="Close" border="0">
                                </td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft">
                                </td>
                                <td class="dimMiddleCenter">
                                    <table align="center" border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr>
                                            <td valign="top">
                                                <br>
                                                <div align="left" class="dimmerLabel">
                                                    Template Name:&nbsp;<span id="spntemplatename" style="visibility: hidden; color: red;
                                                        font-size: 10px">(required)</span><br>
                                                    <asp:TextBox runat="server" ID="txtTemplateName" Style="width: 210px; position: fixed;"
                                                        Visible="True" CssClass="dimmerLabel"></asp:TextBox>
                                                </div>
                                                <br>
                                                <div align="left" class="dimmerLabel">
                                                    Set as Default Template:&nbsp;
                                                    <asp:CheckBox runat="server" ID="chkTDefault" CssClass="dimmerLabel"></asp:CheckBox>
                                                </div>
                                                <br>
                                                <div align="left" class="dimmerLabel">
                                                    <asp:Label runat="server" ID="lblTemplateErrorMessage" Style="color: red; font-size: 10px"
                                                        Visible="False"></asp:Label>
                                                </div>
                                                <br>
                                                <div align="center">
                                                    <asp:ImageButton ID="btnTemplateSave" ImageUrl="/ecn.images/images/save.gif" AlternateText="Save"
                                                        runat="server"></asp:ImageButton></div>
                                                <br>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="dimMiddleRight" width="53">
                                </td>
                            </tr>
                            <tr>
                                <td class="dimBottomLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimBottomCenter">
                                </td>
                                <td class="dimBottomRight">
                                    <div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="dimming" id="divReset" style="width: 450px; height: 300px; display: none;">
                        <table width="450" align="center" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dimTopLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimTopCenter" width="100%">
                                    <span>Reset Template</span>
                                </td>
                                <td class="dimTopRight">
                                    <a href="javascript:CloseConfirmReset();">close</a>
                                </td>
                            </tr>
                            <tr>
                                <td class="dimMiddleLeft">
                                </td>
                                <td class="dimMiddleCenter">
                                    <div class="dimmerLabel">
                                        <img src="/ecn.images/images/questionConfirm.jpg" align="left" />
                                        <p style="margin: 10px;">
                                            Are you sure you want to reset the template?</p>
                                        <p class="ltButtonSmall clearfix" style="margin: 0 0 0 100px;">
                                            <asp:LinkButton ID="btnReset" runat="server" Style="float: left; margin: 0 10px;"
                                                CssClass="ltButton" Text="<span>OK</span>" OnClick="btnReset_Click"></asp:LinkButton>
                                            <a href="javascript:CloseConfirmReset();" style="float: left; margin: 0 10px;"><span>
                                                Cancel</span></a>
                                        </p>
                                    </div>
                                </td>
                                <td class="dimMiddleRight" width="53">
                                </td>
                            </tr>
                            <tr>
                                <td class="dimBottomLeft">
                                    <div>
                                    </div>
                                </td>
                                <td class="dimBottomCenter">
                                </td>
                                <td class="dimBottomRight">
                                    <div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>

<script type="text/javascript">

function sizePreview() {
if (!document.getElementById) return false;
if (!document.getElementById("templateUI")) {
  	
	if (document.getElementById('templatePreviewer')) {
	  document.getElementById('templatePreviewer').style.width = "100%";
	
	}
  }
}




sizePreview();
</script>

<script type="text/javascript" src="http://www.ecn5.com/ecn.accounts/js/browser.js"></script>

<script type="text/javascript">
if(BrowserDetect.browser=="Safari"){
  if(document.getElementById("SurveyWizard_txtTemplateName")){
    document.getElementById("SurveyWizard_txtTemplateName").style.position="static";	
  }
}
</script>


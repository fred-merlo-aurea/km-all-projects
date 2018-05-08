var message = "Function Disabled!";

//function clickIE4() {
//    if (event.button == 2) {
//        alert(message);
//        return false;
//    }
//}

//function clickNS4(e) {
//    if (document.layers || document.getElementById && !document.all) {
//        if (e.which == 2 || e.which == 3) {
//            alert(message);
//            return false;
//        }
//    }
//}

//if (document.layers) {
//    document.captureEvents(Event.MOUSEDOWN);
//    document.onmousedown = clickNS4;
//}
//else if (document.all && !document.getElementById) {
//    document.onmousedown = clickIE4;
//}

//document.oncontextmenu = new Function("alert(message);return false")

function getImagePath(s,p,m) {
	var imglinks = getImageLinks(s,m);

	if (imglinks != '') 
		return '<img src="' + Edition.path + aSz[s] + '/' + p + '.jpg' + '" onclick="javascript:Edition.clickzoom(this,event);" usemap="#m' + m + '" id="i' + m + '" /><map id="m' + m + '" name="m' + m + '">' + imglinks + '</map>';
	else
		return '<img src="' + Edition.path + aSz[s] + '/' + p + '.jpg' + '" onclick="javascript:Edition.clickzoom(this,event);" id="i' + m + '" />';
}

function getImageLinks(s,m)
{
	return eval('if (m'+m+'['+s+'] != undefined && m'+m+'['+s+']!= null && m'+m+'['+s+'] != "") { m'+m+'['+s+']} else ""');
}
function ShowThumbnails()
{
	createThumbnails();
	resetPage();
}

function resetPage(b)
{
	setvisibility("divpage", (b)?'block':'none');
	setvisibility("divThumb", (b)?'none':'block');
}


/*
var lnkWnd;
function Redirect(url, pageno, linkID)
{
	logActivity(pageno, linkID,  "click", "");
	
	if (url.indexOf("mailto") != 0)
	{
        url = url.replace(/%%emailid%%/i, Edition.EmailID)   
        url = url.replace(/%%blastid%%/i, Edition.BlastID)   
	        	    
	    //url = url.toLowerCase().replace(%%blastid%%", Edition.BlastID)       
		if (lnkWnd == undefined || lnkWnd.closed)	
			lnkWnd = window.open(url,'lnkWindow');
		else
			lnkWnd.location.href = url;
			
			
	}	
	lnkWnd.focus();

}
*/


function Redirect(url, pageno, linkID) {

    var lnkWnd;
    logActivity(pageno, linkID, "click", "");

    if (url.indexOf("mailto") != 0) {
        url = url.replace(/%%emailid%%/i, Edition.EmailID)
        url = url.replace(/%%blastid%%/i, Edition.BlastID)

        lnkWnd = window.open(url);

        lnkWnd.focus();
    }
}

function logActivity(pageno, linkID, action, actionvalue)
{

	if (UpdateDB)
	{
	    if (Edition.IsSecured == 1)
	    {
	        if (Edition.EmailID == 0 || Edition.EmailID == "")
	        {
	            window.location.href= window.location.href;
			    return;
	        }
	    }
	    
		if (Edition.SessionID != '')
			AjCalls.LogActivity(Edition.id, Edition.EmailID, Edition.BlastID, pageno, linkID, action, actionvalue, Edition.IP, Edition.SessionID, ActivityCallback);
			
	}
}

function ActivityCallback()
{}

function f2f(url)
{

	var w = 800;
	var h = 770;
	var LeftPosition = (screen.width) ? (screen.width-w)/2 : 0;
	var TopPosition = (screen.height) ? (screen.height-h)/2 : 0;
	var settings = 'height='+h+',width='+w+',top='+TopPosition+',left='+LeftPosition+',toolbar=no,menubar=no,location=no,resizable=no,scrollbars=yes';

	url = 'f2f.aspx?eID=' + Edition.id + '&b='+ Edition.BlastID +'&e=' + Edition.EmailID + '&s=' + Edition.SessionID;		
	
	var fwin = window.open(url,"f2f",settings);
	fwin.focus;
	
}

function getdefaultsize()
{
  var myWidth = 0, myHeight = 0;
  if( typeof( window.innerWidth ) == 'number' ) {
    //Non-IE
    myWidth = window.innerWidth;
    myHeight = window.innerHeight;
  } else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
    //IE 6+ in 'standards compliant mode'
    myWidth = document.documentElement.clientWidth;
    myHeight = document.documentElement.clientHeight;
  } else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
    //IE 4 compatible
    myWidth = document.body.clientWidth;
    myHeight = document.body.clientHeight;
  }
    myHeight = myHeight - 34;
	//window.status = myHeight + ' ' + myWidth;
    //alert(myHeight);
	if (myHeight<=600)
		return 0;
	else if (myHeight>600 && myHeight<=768)
		return 1;
	else if (myHeight>768 && myHeight<=900)
		return 2;
	else if (myHeight>900 && myHeight<=1024)
		return 3;
	else if (myHeight>1024 && myHeight<=1200)
		return 4;
	else if (myHeight>1200 && myHeight<=1280)
		return 5;
	else if (myHeight>1280 && myHeight<=1440)
		return 6;

}

function getobj(id) 
{
  if (document.all && !document.getElementById) 
    obj = eval('document.all.' + id);
  else if (document.layers) 
    obj = eval('document.' + id);
  else if (document.getElementById) 
    obj = document.getElementById(id);

  return obj;
}


function retrieveCookie( cookieName ) {
	/* retrieved in the format: cookieName4=value; only cookies for this domain and path will be retrieved */
	var cookieJar = document.cookie.split( "; " );
	for( var x = 0; x < cookieJar.length; x++ ) {
		var oneCookie = cookieJar[x].split( "=" );
		if( oneCookie[0] == escape( cookieName ) ) { return unescape( oneCookie[1] ); }
	}
	return null;
}

function setCookie( cookieName, cookieValue, lifeTime, path, domain, isSecure ) {
	if( !cookieName ) { return false; }
	if( lifeTime == "delete" ) { lifeTime = -10; } //this is in the past. Expires immediately.
	/* This next line sets the cookie but does not overwrite other cookies.
	syntax: cookieName=cookieValue[;expires=dataAsString[;path=pathAsString[;domain=domainAsString[;secure]]]]
	Because of the way that document.cookie behaves, writing this here is equivalent to writing
	document.cookie = whatIAmWritingNow + "; " + document.cookie; */
	document.cookie = escape( cookieName ) + "=" + escape( cookieValue ) +
		( lifeTime ? ";expires=" + ( new Date( ( new Date() ).getTime() + ( 1000 * lifeTime ) ) ).toGMTString() : "" ) +
		( path ? ";path=" + path : "") + ( domain ? ";domain=" + domain : "") + 
		( isSecure ? ";secure" : "");
	//check if the cookie has been set/deleted as required
	if( lifeTime < 0 ) { if( typeof( retrieveCookie( cookieName ) ) == "string" ) { return false; } return true; }
	if( typeof( retrieveCookie( cookieName ) ) == "string" ) { return true; } return false;
}

function setvisibility(id, visibility)
{
	var obj = getobj(id);
	obj.style.display = visibility;
}

function prtwindow()
{
	var w = 800;
	var h = 600;
	var LeftPosition = (screen.width) ? (screen.width-w)/2 : 0;
	var TopPosition = (screen.height) ? (screen.height-h)/2 : 0;
	var sp = Edition.img1;
	var ep = (Edition.img2==0)?Edition.img1:Edition.img2;
	var settings = 'height='+h+',width='+w+',top='+TopPosition+',left='+LeftPosition+',toolbar=no,menubar=no,location=no,resizable=no,scrollbars=yes';
	
	var url = 'print.aspx?eID=' + Edition.id + '&b='+ Edition.BlastID +'&e=' + Edition.EmailID + '&s=' + Edition.SessionID+ "&sp=" + sp + "&ep=" + ep;		

	var win = window.open(url,"windowprint",settings);
	win.focus;
}

function findPosX(obj)
{
	var curleft = obj.offsetWidth-1;
	
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += obj.offsetLeft;
			obj = obj.offsetParent;
		}
	}
	
	else if (obj.x)
	curleft += obj.x;
	return curleft;
}

function findPosY(obj)
{
	var curtop = 0;
	var curHeight = 0;
	
	if (obj.offsetParent)
	{
		curHeight = obj.offsetHeight;
		
		while (obj.offsetParent)
		{
			curtop =curtop+obj.offsetTop;
			obj = obj.offsetParent;
		}

	}
	
	else if (obj.y)
		curtop = obj.y;
	curtop= curtop ;
		
	return curtop;
}

function getAnchorPosition(anchorname) {
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
		x=getPageOffsetLeft(document.all[anchorname]);
		y=getPageOffsetTop(document.all[anchorname]);
		}
	else if (use_gebi) {
		var o=getobj(anchorname);
		x=getPageOffsetLeft(o);
		y=getPageOffsetTop(o);
		}
 	else if (use_css) {
		x=getPageOffsetLeft(document.all[anchorname]);
		y=getPageOffsetTop(document.all[anchorname]);
		}
	else if (use_layers) {
		var found=0;
		for (var i=0; i<document.anchors.length; i++) {
			if (document.anchors[i].name==anchorname) { found=1; break; }
			}
		if (found==0) {
			coordinates.x=0; coordinates.y=0; return coordinates;
			}
		x=document.anchors[i].x;
		y=document.anchors[i].y;
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


function btngo()
{
	var txtVal = getobj("txtpageno").value;
	if (parseInt(txtVal) > 0 && parseInt(txtVal) <= Edition.tp)
		Edition.gotoPage(parseInt(txtVal));
	else
		alert('Please enter a valid page number between 1 and ' + Edition.tp + '.');
}

function CheckKey(e)
{
	
	var targ;		
	if (!e)  e = window.event;

     if(window.event)
          key = window.event.keyCode;     //IE
     else
          key = e.which;     //firefox
	
	if (key == 13 || key == 3)
	{	
		if (e.target) 
			targ = e.target;
		else if (e.srcElement) 
			targ = e.srcElement;

		if (targ.tagName == 'INPUT')
		{
			if (targ.name == 'txtpageno')
			{
				btngo();
			}
			else if (targ.name == 'txtsearch')
			{
				search();
			}
		}
		e.returnValue = false;
		return false;
	}
}
/*
function borderit(which, mode, img)
{
	if (document.all && document.getElementById)
	{

		if (mode==0)
		{
			getobj('olay').style.top=(parseInt(getobj("i" +img).offsetTop+which.offsetTop)-3) + 'px';
			getobj('olay').style.left=parseInt(getPageOffsetLeft(getobj("i" +img))+which.offsetLeft) + 'px';
			
			//alert(which.id + " / " + which.offsetWidth + " / " + which.offsetHeight);
			
			getobj('olay').style.width=which.offsetWidth + 'px';;
			getobj('olay').style.height=which.offsetHeight + 'px';;
			getobj('olay').style.visibility="visible";

			//alert( getobj('olay').style.top + ' ' + getobj('olay').style.left );
			//getobj('olay').style.width + ' ' + getobj('olay').style.height + ' ' +
			if (document.all||document.getElementById)
				which.style.borderColor="black";
				
		}
		else
		{
			which.style.borderColor="white";
			getobj('olay').style.visibility="hidden";
		}
	}
}
*/

function borderit(which, mode, img)
{
		if (mode==0)
		{
		    var coords = which.coords.split(",");
		    var x1 = coords[0];
		    var x2 = coords[1];
		    var y1 = coords[2];
		    var y2 = coords[3];

            //alert ("TAG : " + which.tagName + '\n' + "coords : " + which.coords + '\n' + "x,y : " + x1 + ',' + x2 + ','+ y1 + ','+ y2  + '\n' + "offsetTop : " +  which.offsetTop + '\n' + "offsetLeft : " +  which.offsetLeft + '\n' + "olay top: " + getobj('olay').style.top + '\n' + "olay left: " + getobj('olay').style.left + '\n' + "offsetWidth : " + which.offsetWidth + '\n' + "clientWidth : " + which.clientWidth + '\n' + "offsetHeight : " + which.offsetHeight  + '\n' + "clientHeight  : " + which.clientHeight ); 
            getobj('olay').href = which.href;
 			getobj('olay').style.top=  parseInt(x2-3) + 'px';
			getobj('olay').style.left= parseInt(parseInt(getPageOffsetLeft(getobj("i" +img))) + parseInt(x1)) + 'px';
			getobj('olay').style.width=parseInt(y1-x1) + 'px';
			getobj('olay').style.height=parseInt(y2-x2) + 'px';
			getobj('olay').style.visibility="visible";
		}
		else
		{
			getobj('olay').style.visibility="hidden";
		}
}

function setVisible(v)
{
    getobj('olay').style.visibility=v;
}

var toolbarOpened = 1;

function setToolbarClosed()
{
	toolbarOpened = 0;	
}

function setToolbarOpened()
{
	toolbarOpened = 1;	
}

function sizeFrameBottom ()
{	
	//alert('ant');
	var winW = 630, winH = 460;

	if (parseInt(navigator.appVersion)>3) {
	 if (navigator.appName=="Netscape") {
	  winW = window.innerWidth;
	  winH = window.innerHeight;
	  }
	 if (navigator.appName.indexOf("Microsoft")!=-1) {
	  winW = document.body.clientWidth;
	  winH = document.body.clientHeight;
	 }
	}

	var winWaNew = winW - 231;
	var winWbNew = winW - 33;
	
	
	var bottomSize = getobj('mainContent');
	var gradientDiv = getobj('hLine');
	
	if (toolbarOpened == 1)
	{
		bottomSize.style.width = winWaNew;
		gradientDiv.style.width = winWaNew-15;
		gradientDiv.style.left = "231px";
	} else if (toolbarOpened == 0)
	{
		bottomSize.style.width = winWbNew;
		gradientDiv.style.width = winWbNew-15;
		gradientDiv.style.left = "33px";

	}
	
	
	var overlayMargin = getobj('olay')
	if (toolbarOpened == 1)
	{
		overlayMargin.style.margin = '2px 0 0 -230px';
	} else if (toolbarOpened == 0)
	{
		overlayMargin.style.margin = '2px 0 0 -32px';
	}
}


function sizeFrame (ReloadImages)
{	
	setSideBar();
	var winW = 630, winH = 460;

	if (parseInt(navigator.appVersion)>3) {
	 if (navigator.appName=="Netscape") {
	  winW = window.innerWidth;
	  winH = window.innerHeight;
	  }
	 if (navigator.appName.indexOf("Microsoft")!=-1) {
	  winW = document.body.clientWidth;
	  winH = document.body.clientHeight;
	 }
	}


	var winHNew = winH - 34;
	var winHNewB = winH - 94;
	var winWaNew = winW - 181;
	var winWbNew = winW - 33;
	var scrlHgt = winH - 200;
	
	/*if (toolbarOpened == 1){
		var scroller  = null;
		var scrollbar = null;
		scroller  = new jsScroller(document.getElementById("Scroller-1"), 180, scrlHgt);
		scrollbar = new jsScrollbar (document.getElementById("Scrollbar-Container"), scroller, false);
	
		var scrollArea = getobj('Scroller-1')
		scrollArea.style.height = scrlHgt;
	}*/

	//alert('ant 7');
	
	//alert (winW + ' X ' + winH );
	var bottomSize = getobj('mainContent')
	bottomSize.style.height = winHNew;
	
	//var toolbarHeight = getobj('Scroller-1')
	//toolbarHeight.style.height = winHNewB;
	
	sizeFrameBottom ();	
	if (Edition != null)
	{
		Edition.defaultsize = getdefaultsize();
		
		getobj("zb").src = "http://www.ecndigitaledition.com/images/zoom" + Edition.defaultsize + ".gif";
		getobj("zb2").src = "http://www.ecndigitaledition.com/images/zoomA" + Edition.defaultsize + ".gif";
		Edition.zoomsize = Edition.defaultsize;
		
		if (ReloadImages && getobj("divpage").style.display == 'block')
			loadimages();	
		
		if (getobj("divThumb").style.display == 'block')
			ShowThumbnails();	
	}
	
}

var whichBtn = new Array("infoBtn")

function setwhichBtn(x) {
  whichBtn=x;
}

function expandSideBar() {
  show1('TABS');
  getobj('infoMenu').style.display="none";
  var scrollBar = getobj("Scroller-1");
  scrollBar.scrollTop = 0;
  
  if (whichBtn=="infoBtn"){
    show2('information');
    show3('OverInfo');
    show4('Info');
	getobj('infoMenu').style.display="block";
  }
  
  if (whichBtn=="pagesBtn"){
    show2('pages');
    show3('OverPages');
    show4('Pages');
  }
  
  if (whichBtn=="tableOfContentsBtn"){
    show2('tableOfContents');
    show3('OverTableOfContents');
    show4('TOC');
  }  
  
  if (whichBtn=="searchBtn"){
    show2('search');
    show3('OverSearch');
    show4('Search');
  }
  
  if (whichBtn=="linksBtn"){
    show2('links');
    show3('OverLinks');
    show4('Links');
  }
  
  if (whichBtn=="backIssuesBtn"){
    show2('backIssues');
    show3('OverBackIssues');
    show4('BackIssues');
  }
 
   
  setToolbarOpened();
  sizeFrameBottom();
  setSideBar();
  setvisibility('closeBtn', 'block');
  setvisibility('openBtn', 'none');
}

function closeSideBar() {
  show1('EMPTY');
  show3('OverFiller');
  setToolbarClosed();
  sizeFrameBottom();
  setvisibility('closeBtn', 'none');
  setvisibility('openBtn', 'block');
}

function setSideBar (){
  
  var winW = 630, winH = 460;
  if (parseInt(navigator.appVersion)>3) {
    if (navigator.appName=="Netscape") {
	  winW = window.innerWidth;
	  winH = window.innerHeight;
	}
	if (navigator.appName.indexOf("Microsoft")!=-1) {
	  winW = document.body.clientWidth;
	  winH = document.body.clientHeight;
	}
  }
	
  var scrollTop = 60;	
  var scrollTrimHeight = 94;
  if (whichBtn=="infoBtn"){
	var btnHeight = 22;		/*the pixel height of the buttons in the information tab*/
	var infoBtnContainer = getobj("infoMenu");
	var infoBtns = infoBtnContainer.getElementsByTagName("a");
	var infoDivs = infoBtnContainer.getElementsByTagName("div");
	
	for (var i=0; i<infoBtns.length; i++) {
	  if (infoBtns[i].getAttribute("class") == "infoBtn" || infoBtns[i].getAttribute("class") == "infoBtn selected" ||
	      infoBtns[i].getAttribute("className") == "infoBtn" || infoBtns[i].getAttribute("className") == "infoBtn selected"){
		
		if (infoBtns[i].style.display != "none"){
		  scrollTop += btnHeight;
		  scrollTrimHeight += btnHeight;
	    }
	  }
    }
	
  }
  winH = winH-scrollTrimHeight;
  getobj("Scroller-1").style.top=scrollTop;
  getobj("Scroller-1").style.height=winH;
  
 /* if (getobj("infoMenu")){
    getobj("infoMenu").style.height=winH;
  
    if (infoDivs){
	  for (var i=0; i<infoDivs.length; i++) {
	      if (infoDivs[i].getAttribute("class") == "infoContentArea" || infoDivs[i].getAttribute("className") == "infoContentArea"){
		    infoDivs[i].style.height = winH;
	      }
        }
	  }
   
   }*/
	
}

   
    function infoSwitch(d)
    {
        if (d == "clear") {
		/*getobj("infoLinkF2F").className = "def";
		getobj("infoLinkPrint").className = "def";
		getobj("infoLinkSubscribe").className = "def";
		getobj("infoLinkContact").className = "def";
		getobj("infoLinkNavInst").className = "def";*/
		} else {
		
		var linkIDs = new Array("Subscribe","Contact","NavInst")
		
        getobj("infoLink" + d).className='infoBtn selected';
		getobj("infoDiv" + d).style.display='block';
        for (var i in linkIDs)
            if (linkIDs[i] != d)
                if (getobj("infoLink" + linkIDs[i])){
				  getobj("infoLink" + linkIDs[i]).className='infoBtn';
				  getobj("infoDiv" + linkIDs[i]).style.display='none';
				}
		}
    
    }
	
	
function focusSearch() {
  var searchBox = getobj("txtsearch");
  searchBox.focus();
}

function processSubForm(x) 
{
	// asign variable names to fields
	var fFname = getobj("fname");
	var fLname = getobj("lname");
	var fEmail = getobj("email");
	var fPhone = getobj("phone");
	var fFax = getobj("fax");
	var fCountry = getobj("country");
	var fAddress = getobj("stAddress");
	var fCity = getobj("city");
	var fState = getobj("state");
	var fZip = getobj("zc");
 
	if (x == "reset")
	{
		fFname.value = "";
		fLname.value = "";
		fEmail.value = "";
		fPhone.value = "";
		fFax.value = "";
		fCountry.selectedIndex = 0;
		getobj("addressContainer").style.display="none";
		fAddress.value = "";
		fCity.value = "";
		fState.selectedIndex = 0;
		fZip.value = "";		
	} 
	else 
	{
		var okSoFar=true;

		// asign variable names to alerts
		var alertFname = getobj("fnameAlert");
		var alertLname = getobj("lnameAlert");
		var alertEmailA = getobj("emailAlert");
		var alertEmailB = getobj("emailInvalidAlert");


		//clear all alerts
		alertFname.style.display="none";
		alertLname.style.display="none";
		alertEmailA.style.display="none";
		alertEmailB.style.display="none";
	 
		var subscr = document.subscription;
	 
		if (fFname.value=="")
		{
		okSoFar=false
		alertFname.style.display="inline";
		}
	  
		if (fLname.value=="")
		{
		okSoFar=false
		alertLname.style.display="inline";
		}
	  
		if (fEmail.value=="")
		{
		okSoFar=false
		alertEmailA.style.display="inline";
		}
	  
		if (!(fEmail.value==""))
		{

			var foundAt = fEmail.value.indexOf("@",0)
			if (foundAt < 1)
			{
			okSoFar = false
			alertEmailB.style.display="inline";
			}
		}
		 
		if (okSoFar==true)  
			AjCalls.Subscribe(Edition.id, Edition.BlastID, fFname.value, fLname.value, fEmail.value, fPhone.value, fFax.value, fCountry.value, fAddress.value, fCity.value, fState.value, fZip.value, Edition.IP, Edition.SessionID, SubscriptionCallback);
	}	
}

function SubscriptionCallback(Response)
{
	var EmailID = 0;
	if (Response != null && Response != "") //Response.value != null && Response.value != ""
	{
	
		gobjDatabaseDom = new XMLDoc(Response);
		gobjDatabaseDomTree = gobjDatabaseDom.docNode;	

		var emailNode = gobjDatabaseDomTree.selectNode("/EmailID");
    	if (emailNode != null && emailNode != undefined)
		{
			EmailID = parseInt(emailNode.getText());
			if (EmailID > 0)
			{
				Edition.EmailID = EmailID;
				getobj('infoLinkSubscribe').style.display = 'none';
			}
		}		
	
		var errNode = gobjDatabaseDomTree.selectNode("/Error");
		if (errNode != null && errNode != undefined)
		{
			getobj("subscFormThankYou").innerHTML = "<p>" + errNode.getText() + "</p>";
		}
		else
		{
			processSubForm('reset');//clear fields after processing variables
		}	
		showSubscThankYou();
	}
}

function countryChange() {
  var countryValue = getobj("country").value;
  if (countryValue == "United States"){
    
	getobj("addressContainer").style.display="block";
	getobj("zipLabel").innerHTML = "Zip Code:";
	getobj("stateContainer").style.display="inline";
  
  } else if (!(countryValue == "United States")&&!(countryValue == "")){
    
	getobj("addressContainer").style.display="block";
	getobj("zipLabel").innerHTML = "Internation Postal Code:";
	getobj("stateContainer").style.display="none";
  
  } 
}

function initSubscForm() {

	if (getobj('infoLinkSubscribe')){
		if (getobj('infoLinkSubscribe').style.display != 'none')
		{
			getobj("subscFormContainer").style.display="block";
			getobj("subscFormThankYou").style.display="none";
		}
	}
}

function showSubscThankYou() {
	getobj("subscFormContainer").style.display="none";
	getobj("subscFormThankYou").style.display="block";
	setSideBar();
}

/* shower scripts */

   
    function show1(d)
    {
	    var divs1 = new Array("EMPTY","TABS")

        getobj("div" + d).style.display = 'block';
        for (var i in divs1)
            if (divs1[i] != d)
                getobj("div" + divs1[i]).style.display = 'none';
      
    }
	
    function show2(d)
    {
		var divs2 = new Array("information","pages","tableOfContents","search","links","backIssues")
		
        getobj("div" + d).style.display = 'block';
        for (var i in divs2)
            if (divs2[i] != d)
                getobj("div" + divs2[i]).style.display = 'none';
      
    }
	
    function show3(d)
    {
	    var divs3 = new Array("OverFiller","OverInfo","OverPages","OverTableOfContents","OverSearch","OverLinks","OverBackIssues")

        getobj("div" + d).style.display = 'block';
        for (var i in divs3)
            if (divs3[i] != d)
                getobj("div" + divs3[i]).style.display = 'none';
      
    }
	
    function show4(d)
    {
		var divs4 = new Array("Info","Pages","TOC","Search","Links","BackIssues")

        getobj("abv" + d).style.display = 'block';
        for (var i in divs4)
            if (divs4[i] != d)
                getobj("abv" + divs4[i]).style.display = 'none';
      
    }

	var currentselected = '';
    function show(d)
    {
	    var divs = new Array("information","pages","tableOfContents","search","links","backIssues")

        getobj("div" + d).style.display = 'block';
		setObjprop(getobj("Image" + d), 'src', 'http://www.ecndigitaledition.com/images/' + d + '_off.gif');
		currentselected = d;
		
        for (var i in divs)
            if (divs[i] != d)
            {
	   			getobj("div" + divs[i]).style.display = 'none';
				setObjprop(getobj("Image" + divs[i]), 'src', 'http://www.ecndigitaledition.com/images/' + divs[i] + '_off.gif');
      		}
    }
   
    function setObjprop(obj, prop, val)
	{
		for (p in obj) if (p == prop) obj[p] = val;
	}

function buttonOver(linkID) {
		
}

function buttonOver(linkID){
	if(document.getElementById(linkID)){
		document.getElementById(linkID).className="selected";
	}
}

function buttonOut(linkID) {
	if(document.getElementById(linkID)){
		document.getElementById(linkID).className="default";
	}
}

function highlightBorder(x,cellName) {
  if (x == "none") {
		if(getobj(cellName)){
			getobj(cellName).className="highlightOff";
		}
		
	} else {
	
		if(getobj(cellName)){
			getobj(cellName).className="highlightOn";
		}
	}
}
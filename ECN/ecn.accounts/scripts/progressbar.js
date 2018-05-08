var initProgressPanel=false;
var prgCounter=0;
var strLoadingMessage ='Loading...';
   
function initLoader(strSplash)
{
try
   {
	if (strSplash) strLoadingMessage =strSplash;
	var myNewObj= document.getElementById('divContainer');
	if (!myNewObj )
	{
		strID='divContainer';
		strClass='divContainer';

		myNewObj =  createNewDiv( strID,strClass);
		document.body.appendChild(myNewObj);
     }
   
	var myNewObj= document.getElementById('divLoadingStat');
	if (!myNewObj )
	{
		strID='divLoadingStat';  
		strClass='divLoadingStat';
		myNewObj =  createNewDiv( strID,strClass);        
		var mytext=document.createTextNode(strLoadingMessage);
		myNewObj.appendChild(mytext);
		document.getElementById('divContainer').appendChild(myNewObj);
	}
  
	var myNewObj= document.getElementById('divLoaderBack');
	if (!myNewObj )
	{
		strID='divLoaderBack';
		strClass='divLoaderBack';

		myNewObj =  createNewDiv( strID,strClass);
		document.getElementById('divContainer').appendChild(myNewObj);
     }
     
     var myNewObj= document.getElementById('divLoaderProgress');
     if (!myNewObj )
	{
		strID='divLoaderProgress';
		strClass='divLoaderProgress'
		myNewObj =  createNewDiv( strID,strClass);
		document.getElementById('divLoaderBack').appendChild(myNewObj);
	}
	initProgressPanel=true;
}
catch(err)
   {}	
   
}
       
function initDownLoader() {
  try {
  	var myNewObj= document.getElementById('divContainer');
  	if (!myNewObj )	{
  		strID='divContainer';
  		strClass='divContainer';
  
  		myNewObj =  createNewDiv( strID,strClass);
  		document.body.appendChild(myNewObj);
  		var innerHTMLScript = "";
  		innerHTMLScript += "<table border=0 width=80% height=80% align=center>";
  		innerHTMLScript += "<tr><td align=center><br><br><font face=arial size=2>Processing records. Please wait..</font></td></tr>";
  		innerHTMLScript += "<tr><td align=center width=100%>";
  		innerHTMLScript += "<img src='http://www.ecndigitaledition.com/images/prog-bar.gif' border=0'></td></tr>";
  		innerHTMLScript += "</table>";
  		document.getElementById('divContainer').innerHTML = innerHTMLScript;
    }
  }catch(err){}	 
}

function setProgress(intPrc,strMessage)
{
    try
   {
    if (!initProgressPanel) initLoader('Loading...');
    if (strMessage)  strLoadingMessage=strMessage;
    if(!intPrc) return
    var mytext=document.createTextNode( strLoadingMessage+' ' + prgCounter +'%');
    var lodStat= document.getElementById('divLoadingStat');
    lodStat.removeChild(lodStat.lastChild );
    lodStat.appendChild(mytext);
    prgCounter++;
    prgDiv= document.getElementById('divLoaderProgress');
    prgDiv.style.width=prgCounter*5+'px';
    if (prgCounter<=intPrc) 
    {
        setTimeout( 'setProgress('+intPrc+')',0.1);
    }
    else if(prgCounter>100)
    {
            completed();
    }
    }
    catch(err)
   {}	
}

function completed()
{
  document.body.removeChild(document.getElementById('divContainer'));
}

function createNewDiv()
{
	newDiv = document.createElement('div');
	newDiv.id=strID;
	var styleCollection = newDiv.style;
	newDiv.className=strClass;
	return newDiv;
}

function resetProgress()
{
	prgCounter=0;
	strLoadingMessage ='Loading...';
}

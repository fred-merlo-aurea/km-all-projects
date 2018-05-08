var gobjDatabaseDom;
var gobjDatabaseDomTree;

var Edition = {
    Page: { First: 1, Previous: 2, Next: 3, Last: 4 },
    IsMaximized: false,
    id: 0,
    cp: 1,
    tp: 0,
    path: '',
    type: '',
    EmailID: 0,
    BlastID: 0,
    SessionID: '',
    IP: '',
    i1: '',
    i2: '',
    img1: 0,
    img2: 0,
    zoomsize: 1,
    pagemode: 1,
    IsSecured: 0,
    onSuccess: function (response) {

        if (response != null) { // && response.value != null && response.value != ""

            response = response.replace(/this./gi, "Edition.")
            eval(response);
        }
        else {
            window.location.href = "http://www.ecndigitaledition.com/digital/error.aspx";
            return;
        }


        Edition.i1 = getobj("td1");
        Edition.i2 = getobj("td2");

        sizeFrame(false);

        Edition.defaultsize = getdefaultsize();
        getobj("zb").src = "http://www.ecndigitaledition.com/images/zoom" + Edition.defaultsize + ".gif";
        getobj("zb2").src = "http://www.ecndigitaledition.com/images/zoomA" + Edition.defaultsize + ".gif";
        Edition.zoomsize = Edition.defaultsize;

        if (Edition.cp > Edition.tp)
            Edition.cp = Edition.tp;

        if (Edition.type.toLowerCase() == '1flyer')
            Edition.pagemode = 1;
        else if (Edition.type.toLowerCase() == '2flyer')
            Edition.pagemode = 2;
        else if (retrieveCookie("pagemode") != null)
            Edition.pagemode = retrieveCookie("pagemode");

        loadimages();

    },
    onFailed: function (error) {
        alert('Failed');
    },
    Init: function () {

        this.id = parseInt(getobj("lblEditionID").innerHTML);
        this.IsSecured = parseInt(getobj("lblIsSecured").innerHTML);

        this.cp = parseInt(getobj("txtpageno").value);
        var response = AjCalls.GetEditionProps(this.id, Edition.onSuccess, Edition.onFailed);


    },
    Nav: function (action) {
        switch (action) { case this.Page.First: { this.gotoPage(1); break; } case this.Page.Previous: { this.gotoPage(this.cp - parseInt(this.pagemode)); break; } case this.Page.Next: { this.gotoPage(this.cp + 1); break; } case this.Page.Last: { this.gotoPage(this.tp); break; } }
    },
    gotoPage: function (pageno) {
        resetPage(true);
        if (parseInt(pageno) <= 0)
            pageno = 1;
        else if (parseInt(pageno) > this.tp)
            pageno = this.tp;

        if (parseInt(pageno) == this.cp)
            return;

        this.cp = pageno;

        loadimages()
    },
    setpref: function (mode) {
        resetPage(true);

        if (Edition.pagemode == mode)
            return;

        setCookie("pagemode", mode, 5000);
        this.pagemode = mode;
        loadimages();
    },
    clickzoom: function (img, e) {
        var targ;

        if (!e) e = window.event;

        if (e.target)
            targ = e.target;
        else if (e.srcElement)
            targ = e.srcElement;

        if (targ.tagName != 'IMG') return;

        if (this.IsMaximized) {
            if (Edition.zoomsize == 0) return;
            this.zoom(0);
        }
        else {
            if (Edition.zoomsize == 6) return;
            this.zoom(1);
        }
        this.IsMaximized = !(this.IsMaximized);

    },
    zoombar: function (size) {
        if (Edition.zoomsize != size) {
            this.IsMaximized = false;
            Edition.zoomsize = size;
            this.zoom(3);
        }
    }
	,
    zoom: function (mode) {
        if (mode == 1) {
            if (Edition.zoomsize < 6)
                Edition.zoomsize++;
            else
                return;
        }
        else if (mode == 0) {
            if (Edition.zoomsize > 0)
                Edition.zoomsize--;
            else
                return;
        }

        Edition.defaultsize = Edition.zoomsize;
        getobj("zb").src = "http://www.ecndigitaledition.com/images/zoom" + Edition.zoomsize + ".gif";
        getobj("zb2").src = "http://www.ecndigitaledition.com/images/zoomA" + Edition.zoomsize + ".gif";

        window.scrollTo(0, 0);

        Edition.i1.innerHTML = '';
        Edition.i2.innerHTML = '';

        Edition.i1.innerHTML = getImagePath(Edition.zoomsize, Edition.img1, 1);
        if (Edition.img2 != 0) Edition.i2.innerHTML = getImagePath(Edition.zoomsize, Edition.img2, 2);

        preload((Edition.cp == 1) ? 1 : (Edition.cp - 1), Edition.zoomsize, 0)

    },
    IsLastPage: function (no) {
        if (parseInt(no) < Edition.tp)
            return false;
        else
            return true;
    }
}

var aSz=new Array(450,618,750,874,1050,1130,1290)
var i1=new Array(7);
var i2=new Array(7);
var m1=new Array(7);
var m2=new Array(7);
var pageVisits=new Array(Edition.tp);

function preload(p,s,c) 
{

	var img1 = new Image();
	var img2 = new Image();
	var lastpage = Edition.IsLastPage(p+1);

	if (c==0)
	{
		img1.src = Edition.path + aSz[s] + "/" + p + ".jpg";
		
		if (!lastpage)
			img2.src = Edition.path + aSz[s] + "/" + parseInt(p+1) + ".jpg";

		i1[s] = img1;
		i2[s] = img2;	
		
		setTimeout("preload(" + p +"," + s + ",1)",1000);
	}
	else
	{
		var st = (s==0)?0:(s-1)
		var ed = (s==6)?6:(s+1)
	
		for (var i=st;i<=ed;i++)
		{
			img1 = new Image();
			img2 = new Image();
			
			img1.src = Edition.path + aSz[i] + "/" + p + ".jpg";
			if (!lastpage)
				img2.src = Edition.path + aSz[i] + "/" + parseInt(p+1) + ".jpg";
			
			i1[i] = img1;
			i2[i] = img2;
		}
		
		var pst = 1;
		
		if (!Edition.IsLastPage(p))
			pst = p+1;
		
		for (var pno=pst;pno<=pst+5;pno++)
		{
			img1 = new Image();
			
			if (!Edition.IsLastPage(pno))
				img1.src = Edition.path + aSz[s] + "/" + pno + ".jpg";
			else
				break;
		}
	}

}

function loadimages() {
	var pageno = Edition.cp;
	
	window.scrollTo(0,0);	
	getobj("divlinkcontent").innerHTML ="";
	Edition.i1.innerHTML = '';
	Edition.i2.innerHTML = '';

	getobj('olay').style.visibility="hidden";
	
	if (Edition.type.toLowerCase() == '1flyer')
	{
		Edition.img1 = pageno;
		getobj("txtpageno").value=pageno;

		if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
		{
			pageVisits[pageno-1] = 'Y';
			logActivity(pageno, 0,  "visit", "");

		}
		
		Edition.img2=0;
		preload(pageno, Edition.defaultsize, 0);
	}
	else if (Edition.type.toLowerCase() == '2flyer')
	{
		// check page is even or odd number- if even number subtract 1
		if ((pageno != 1) && ((parseInt(pageno)%2)== 0))
			pageno--;
			
		Edition.img1 = pageno;
		if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
		{
			pageVisits[pageno-1] = 'Y';
			logActivity(pageno, 0,  "visit", "");
		}
		
		preload(pageno, Edition.defaultsize, 0);
		getobj("txtpageno").value=pageno;

		if (parseInt(pageno)+1 <= Edition.tp)
		{
			pageno++;
			Edition.img2=pageno;
			if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
			{
				pageVisits[pageno-1] = 'Y';
				logActivity(pageno, 0,  "visit", "");
			}
		}
		else
		{
			Edition.img2=0;
		}
	}
	else
	{
		if (Edition.pagemode == 2) 
		{		
			// check page is even or odd number- if even number subtract 1
			if ((pageno != 1) && ((parseInt(pageno)%2)== 1))
				pageno--;
				
			Edition.img1 = pageno;

			if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
			{
				pageVisits[pageno-1] = 'Y';
				logActivity(pageno, 0,  "visit", "");
			}
						

			preload(pageno, Edition.defaultsize, 0);
			getobj("txtpageno").value=pageno;
			
			if (parseInt(pageno)+1 <= Edition.tp && (pageno!= 1))
			{
				pageno++;
				if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
				{
					pageVisits[pageno-1] = 'Y';
					logActivity(pageno, 0,  "visit", "");
				}
				
				Edition.img2=pageno;
			}
			else
					Edition.img2=0;
		}
		else
		{
			Edition.img1 = pageno;
			getobj("txtpageno").value=pageno;
			
			if (pageVisits[pageno-1]==undefined || pageVisits[pageno-1] == '')
			{
				pageVisits[pageno-1] = 'Y';
				logActivity(pageno, 0,  "visit", "");
			}
			
			Edition.img2=0;
			preload(pageno, Edition.defaultsize, 0);
		}
	}

	Edition.cp = pageno;
	AjCalls.GetLinks(Edition.id, (Edition.img2==0)?Edition.img1:Edition.img1+','+Edition.img2, Links_Callback);
}

function Links_Callback(Response)
{ 
	var lnks='';
	

	m1=new Array(7);
	m2 = new Array(7);

	if (Response != null && Response != "") // && Response != null && Response.value != ""
	{
		gobjDatabaseDom = new XMLDoc(Response);
		gobjDatabaseDomTree = gobjDatabaseDom.docNode;

		var o1 = gobjDatabaseDomTree.selectNode("/Image[0]");
		if (o1 != null && o1 != undefined)
		{
			lnks += GetLinksHTML(o1, 1);
			GetAreaHTML(o1,1);
		}	
		
		var o2 = gobjDatabaseDomTree.selectNode("/Image[1]");
    	if (o2 != null && o2 != undefined)
		{
			lnks += GetLinksHTML(o2, 2);
			GetAreaHTML(o2,2);
		}
		
		if (lnks!= "")
			getobj("divlinkcontent").innerHTML = "<ul>" + lnks  + "</ul>";
		else
			getobj("divlinkcontent").innerHTML = "<ul><li>No links found</li></ul>";
	}
	
	Edition.i1.innerHTML = getImagePath(Edition.defaultsize,Edition.img1, 1);
	
	if (Edition.img2==0)
		Edition.i2.innerHTML = '';
	else
		Edition.i2.innerHTML = getImagePath(Edition.defaultsize,Edition.img2, 2);

	setvisibility("divpage","block");		
}

function search() 
{
	var txtsearch = getobj("txtsearch").value;
	if (txtsearch != "")
	{
		getobj("divsearchcontent").innerHTML = "<div id='progBar'><img src='http://www.ecndigitaledition.com/images/prog-bar.gif' border=0'></div>";
		logActivity(0, 0,  "search", txtsearch);
		AjCalls.Search(Edition.id, txtsearch, search_Callback);
	}
}

function search_Callback(response)
{ 
	getobj("divsearchcontent").innerHTML = "";;
	if (response.error == null)
	{
	    if (response != null && response  != "") //response.value != null && response.value != ""
			getobj("divsearchcontent").innerHTML = response;
	}
}

function createSPThumbnails()
{
    var str= "";
 
    for(var i=1;i<=Edition.tp;i++)
	{
    	str += '<a alt=Page "' + i + '" title="Page ' + i + '" href="javascript:Edition.gotoPage(' + i + ');"><img src="' + Edition.path + '150/' + i + '.png"></a>Page :&nbsp; ' + i;
	}
	getobj("divpages").innerHTML = str;
}

function createThumbnails()
{
	var strTable="<table cellspacing='15' cellpadding='2' border='0'>";
	var col=0;
	var colcount=0;
    var bfirst = true;

	if (Edition.pagemode == 2)
	{
		col=5;
		switch(getdefaultsize()) {case 2:{col = 4;break;};case 1:{col = 3;break;}case 0:{col = 2;break;}}
	}
	else
	{
		col=8;
		switch(getdefaultsize()) {case 2:{col = 8;break;};case 1:{col = 6;break;}case 0:{col = 4;break;}}
	}
			
	for(var i=1;i<=Edition.tp;i++)
	{
		if (colcount==0)
			strTable += "<TR>";

		if (i==1)
		{
			strTable += "<TD class='pageWithImg' align='right'><a href='javascript:Edition.gotoPage(" + i + ");' alt='Page " + i + "'><img src='" + Edition.path + "150/" + i + ".png' alt='Page "+ i + "' border='0'/></a></TD>";
			colcount++;
		}
		else if (i!=Edition.tp || !bfirst)
		{
			if (Edition.pagemode == 2)
			{
				if (bfirst)
				{
					bfirst = false;
					strTable += "<TD class='pageWithImg'  align='left'>"
					strTable += "<a href='javascript:Edition.gotoPage(" + i + ");' alt='Page " + i + "'><img src='" + Edition.path + "150/" + i + ".png' alt='Page "+ i + "' border='0'/></a>";
					colcount=  colcount+.5;
				}
				else 
				{
					strTable += "<a href='javascript:Edition.gotoPage(" + i + ");' alt='Page " + i + "'><img src='" + Edition.path + "150/" + i + ".png' alt='Page "+ i + "' border='0'/></a>";
					bfirst = true;
					strTable += "</TD>"
					colcount=  colcount+.5;
				}			
			}
			else
			{
				strTable += "<TD class='pageWithImg' align='left'><a href='javascript:Edition.gotoPage(" + i + ");' alt='Page " + i + "'><img src='" + Edition.path + "150/" + i + ".png' alt='Page "+ i + "' border='0'/></a></td>";
				colcount++;
			}
		}
		else if (i==Edition.tp)
		{
			if (!bfirst) 
		    {
		        strTable += "</TD>";
                bfirst=true;
                colcount=  colcount+.5; 
		    }		
			strTable += "<TD class='pageWithImg' align='left'><a href='javascript:Edition.gotoPage(" + i + ");' alt='Page " + i + "'><img src='" + Edition.path + "150/" + i + ".png' alt='Page "+ i + "' border='0'/></a></TD>";	
			colcount++;
		}		
		
		if (colcount == col || i == Edition.tp)
		{
			for(var j=colcount;j<col;j++)
			{
			    strTable += "<TD align='left' class='pageWithImg'></TD>";
			}
			colcount=0;
			strTable += "</TR>"
		}		
	} 
	strTable += "</table>";
	getobj("divThumb").innerHTML = strTable;

}

function GetLinksHTML(o, m)
{
	var lnks = '';
	var lastlink = '';
	var Alias = '';
	
	if (o.selectNode("/Link") != undefined) {

		for (var i=0; i<o.children.length; i++) {

		    if (o.children[i].getAttribute("type") == 'uri' || o.children[i].getAttribute("type") == 'URI') {
			    
				if (lastlink != o.children[i].getText())
				{
					Alias = o.children[i].getAttribute("Alias") == ''?o.children[i].getText():o.children[i].getAttribute("Alias");
					Alias = Alias.replace(/&apos;/g,"'");
					
					if (o.children[i].getText().indexOf("mailto") == 0)
						lnks += "<li><a onMouseover=javascript:borderit(getobj('area_" + o.children[i].getAttribute("LinkID") + "'),0," + m +") onMouseout=javascript:borderit(getobj('area_" + o.children[i].getAttribute("LinkID") + "'),1," + m +") href='" + o.children[i].getText() + "' onclick='javascript:Redirect(\"" + o.children[i].getText() + "\"," + o.getAttribute("PageNo") + "," + o.children[i].getAttribute("LinkID") + ");'>" + Alias + "</a></li>";
					else
						lnks += "<li><a onMouseover=javascript:borderit(getobj('area_" + o.children[i].getAttribute("LinkID") + "'),0," + m +") onMouseout=javascript:borderit(getobj('area_" + o.children[i].getAttribute("LinkID") + "'),1," + m +") href='javascript:Redirect(\"" + o.children[i].getText() + "\"," + o.getAttribute("PageNo") + "," + o.children[i].getAttribute("LinkID") + ");'>" + Alias + "</a></li>";	
				}	
				lastlink = o.children[i].getText();
			}
		}
	}
	return lnks;
}

function GetAreaHTML(o, m)
{
	var mp = '';
	if (o.selectNode("/Link") != undefined)
	{
		var h = parseInt(o.getAttribute("Height"));
	
		for (var a = 0; a < aSz.length; a++)
		{
			mp="";
			for (var i=0; i<o.children.length; i++)
			{

			    if (o.children[i].getAttribute("type") == 'goto' || o.children[i].getAttribute("type") == 'GoTo')
					mp += "<area shape='rect' id='area_" + o.children[i].getAttribute("LinkID") + "' onMouseover='javascript:borderit(this,0," + m +")'  coords='" + gc(o.children[i].getAttribute("x1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("x2"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y2"), h, aSz[a]) + "' href='javascript:Edition.gotoPage(" + o.children[i].getText() + ");'/>";
				else if (o.children[i].getAttribute("type") == 'uri' || o.children[i].getAttribute("type") == 'URI')
				{
			    
					if (o.children[i].getText().indexOf("mailto") == 0)
					    mp += "<area shape='rect' id='area_" + o.children[i].getAttribute("LinkID") + "' onMouseover='javascript:borderit(this,0," + m + ")' coords='" + gc(o.children[i].getAttribute("x1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("x2"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y2"), h, aSz[a]) + "' href='" + o.children[i].getText() + "' onclick='javascript:Redirect(\"" + o.children[i].getText().replace("%22", "%27") + "\"," + o.getAttribute("PageNo") + "," + o.children[i].getAttribute("LinkID") + ");' />"; 
					else
					    mp += "<area shape='rect' id='area_" + o.children[i].getAttribute("LinkID") + "' onMouseover='javascript:borderit(this,0," + m + ")' coords='" + gc(o.children[i].getAttribute("x1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y1"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("x2"), h, aSz[a]) + "," + gc(o.children[i].getAttribute("y2"), h, aSz[a]) + "' href='javascript:Redirect(\"" + o.children[i].getText().replace("%22", "%27") + "\"," + o.getAttribute("PageNo") + "," + o.children[i].getAttribute("LinkID") + ");'/>"; 	
				}
			}
			
			if (m == 1) m1[a] = mp; else m2[a] = mp;
		}
	}
}

function gc(x, h, r)
{
	return parseInt((x * ((r*100)/h))/100);
}

function xmlError(e) {
   alert("There has been an error accessing the XML Database. The error is:\n" + e)
} 


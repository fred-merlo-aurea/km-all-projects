
function SetFocus()
{
	if (BrowserInfo.IsIE55OrMore)
		objContent.DOM.focus() ;
	else
		objContent.focus() ;
}

function decCommand(cmdId, cmdExecOpt, url)
{
	var status = objContent.QueryStatus(cmdId) ;
	
	if ( status != DECMDF_DISABLED && status != DECMDF_NOTSUPPORTED )
	{
		if (cmdExecOpt == null) cmdExecOpt = OLECMDEXECOPT_DODEFAULT ;
		objContent.ExecCommand(cmdId, cmdExecOpt, url) ;
	}
	SetFocus() ;
}

function docCommand(command)
{
	objContent.DOM.execCommand(command) ;
	SetFocus();
}

function doStyle(command)
{
	var oSelection = objContent.DOM.selection ;
	var oTextRange = oSelection.createRange() ;
	
	if (oSelection.type == "Text")
	{
		decCommand(DECMD_REMOVEFORMAT);
		if (!FCKFormatBlockNames) loadFormatBlockNames() ;
		doFormatBlock( FCKFormatBlockNames[0] );	// This value is loaded at CheckFontFormat()
 
		var oFont = document.createElement("FONT") ;
		oFont.innerHTML = oTextRange.htmlText ;
		
		var oParent = oTextRange.parentElement() ;
		var oFirstChild = oFont.firstChild ;
		
		if (oFirstChild.nodeType == 1 && oFirstChild.outerHTML == oFont.innerHTML && 
				(oFirstChild.tagName == "SPAN"
				|| oFirstChild.tagName == "FONT"
				|| oFirstChild.tagName == "P"
				|| oFirstChild.tagName == "DIV"))
		{
			oParent.className = command.value ;
		}
		else
		{
			oFont.className = command.value ;
			oTextRange.pasteHTML( oFont.outerHTML ) ;
		}
	}
	else if (oSelection.type == "Control" && oTextRange.length == 1)
	{
		var oControl = oTextRange.item(0) ;
		oControl.className = command.value ;
	}
	
	command.selectedIndex = 0 ;
	
	SetFocus();
}

function doFormatBlock(combo)
{
	if (combo.value == null || combo.value == "")
	{
		if (!FCKFormatBlockNames) loadFormatBlockNames() ;
		objContent.ExecCommand(DECMD_SETBLOCKFMT, OLECMDEXECOPT_DODEFAULT, FCKFormatBlockNames[0]);
	}
	else
		objContent.ExecCommand(DECMD_SETBLOCKFMT, OLECMDEXECOPT_DODEFAULT, combo.value);
	
	SetFocus();
}

function doFontName(combo)
{
	if (combo.value == null || combo.value == "")
	{
		// TODO: Remove font name attribute.
	}
	else
		objContent.ExecCommand(DECMD_SETFONTNAME, OLECMDEXECOPT_DODEFAULT, combo.value);
	
	SetFocus();
}

function doFontSize(combo)
{
	if (combo.value == null || combo.value == "")
	{
		// TODO: Remove font size attribute (Now it works with size 3. Will it work forever?)
		objContent.ExecCommand(DECMD_SETFONTSIZE, OLECMDEXECOPT_DODEFAULT, 3);
	}
	else
		objContent.ExecCommand(DECMD_SETFONTSIZE, OLECMDEXECOPT_DODEFAULT, parseInt(combo.value));
	
	SetFocus();
}

function dialogImage()
{
	var html = FCKShowDialog("dialog/fck_image.html", window, 430, 380);
	// The response is the IMG tag HTML
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function dialogTable(searchParentTable)
{
	if (searchParentTable)
	{
		var oRange  = objContent.DOM.selection.createRange() ;
		var oParent = oRange.parentElement() ;
		
		while (oParent && oParent.nodeName != "TABLE")
		{
			oParent = oParent.parentNode ;
		}
		
		if (oParent && oParent.nodeName == "TABLE")
		{
			var oControlRange = objContent.DOM.body.createControlRange();
			oControlRange.add( oParent ) ;
			oControlRange.select() ;
		}
		else
			return ;
	}

	FCKShowDialog("dialog/fck_table.html", window, 400, 210);
	SetFocus() ;
}

function dialogTableCell()
{
	FCKShowDialog("dialog/fck_tablecell.html", window, 580, 220);
	SetFocus() ;
}

function dialogLink()
{
	if (checkDecCommand(DECMD_HYPERLINK) != OLE_TRISTATE_GRAY)
	{
		FCKShowDialog("dialog/fck_link.html", window, 400, 190);
		SetFocus() ;
	}
}

// insertHtml(): Insert HTML at the current document position.
function insertHtml(html)
{
	if (objContent.DOM.selection.type.toLowerCase() != "none")
		objContent.DOM.selection.clear() ;
	objContent.DOM.selection.createRange().pasteHTML(html) ; 
	SetFocus() ;
}

function foreColor()
{
	var color = FCKShowDialog("dialog/fck_selcolor.html", window, 370, 240);
	if (color) objContent.ExecCommand(DECMD_SETFORECOLOR,OLECMDEXECOPT_DODEFAULT, color) ;
	SetFocus();
}

function backColor()
{
	var color = FCKShowDialog("dialog/fck_selcolor.html", window, 370, 240);
	if (color) objContent.ExecCommand(DECMD_SETBACKCOLOR,OLECMDEXECOPT_DODEFAULT, color) ;
	SetFocus();
}

function insertSpecialChar()
{
	var html = FCKShowDialog("dialog/fck_specialchar.html", window, 400, 250);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function insertSmiley()
{
	var html = FCKShowDialog("dialog/fck_smiley.html", window, config.SmileyWindowWidth, config.SmileyWindowHeight) ;
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function FCKShowDialog(pagePath, args, width, height)
{
	return showModalDialog(pagePath, args, "dialogWidth:" + width + "px;dialogHeight:" + height + "px;help:no;scroll:no;status:no");
}

function about()
{
	FCKShowDialog("dialog/fck_about.html", window, 460, 290);
}

function pastePlainText()
{
	var sText = HTMLEncode( clipboardData.getData("Text") ) ;
	sText = sText.replace(/\n/g,'<BR>') ;
	insertHtml(sText) ;
}

function pasteFromWord()
{
	if (BrowserInfo.IsIE55OrMore)
		cleanAndPaste( GetClipboardHTML() ) ;
	else if ( confirm( lang["NotCompatiblePaste"] ) )
		decCommand(DECMD_PASTE) ;
}

function cleanAndPaste( html )
{
	// Remove all SPAN tags
	html = html.replace(/<\/?SPAN[^>]*>/gi, "" );
	// Remove Class attributes
	html = html.replace(/<(\w[^>]*) class=([^ |>]*)([^>]*)/gi, "<$1$3") ;
	// Remove Style attributes
	html = html.replace(/<(\w[^>]*) style="([^"]*)"([^>]*)/gi, "<$1$3") ;
	// Remove Lang attributes
	html = html.replace(/<(\w[^>]*) lang=([^ |>]*)([^>]*)/gi, "<$1$3") ;
	// Remove XML elements and declarations
	html = html.replace(/<\\?\?xml[^>]*>/gi, "") ;
	// Remove Tags with XML namespace declarations: <o:p></o:p>
	html = html.replace(/<\/?\w+:[^>]*>/gi, "") ;
	// Replace the &nbsp;
	html = html.replace(/&nbsp;/, " " );
	// Transform <P> to <DIV>
	var re = new RegExp("(<P)([^>]*>.*?)(<\/P>)","gi") ;	// Different because of a IE 5.0 error
	html = html.replace( re, "<div$2</div>" ) ;
	
	insertHtml( html ) ;
}

function GetClipboardHTML()
{
	var oDiv = document.getElementById("divTemp")
	oDiv.innerHTML = "" ;
	
	var oTextRange = document.body.createTextRange() ;
	oTextRange.moveToElementText(oDiv) ;
	oTextRange.execCommand("Paste") ;
	
	var sData = oDiv.innerHTML ;
	oDiv.innerHTML = "" ;
	
	return sData ;
}

function HTMLEncode(text)
{
	text = text.replace(/&/g, "&amp;") ;
	text = text.replace(/"/g, "&quot;") ;
	text = text.replace(/</g, "&lt;") ;
	text = text.replace(/>/g, "&gt;") ;
	text = text.replace(/'/g, "&#146;") ;
	text = text.replace(/\xE9/g, "&eacute;") ;

	return text ;
}

function showTableBorders()
{
	objContent.ShowBorders = !objContent.ShowBorders ;
	SetFocus() ;
}

function showDetails()
{
	objContent.ShowDetails = !objContent.ShowDetails ;
	SetFocus() ;
}

var FCKFormatBlockNames ;

function loadFormatBlockNames()
{
	var oNamesParm = new ActiveXObject("DEGetBlockFmtNamesParam.DEGetBlockFmtNamesParam") ;
	objContent.ExecCommand(DECMD_GETBLOCKFMTNAMES, OLECMDEXECOPT_DODEFAULT, oNamesParm);
	var vbNamesArray = new VBArray(oNamesParm.Names) ;

	FCKFormatBlockNames = vbNamesArray.toArray() ;
}

function doZoom( sizeCombo ) 
{
	if (sizeCombo.value != null || sizeCombo.value != "")
		objContent.DOM.body.runtimeStyle.zoom = sizeCombo.value + "%" ;
}

function insertList( type )
{
	var oDoc = objContent.DOM ;
	if ( !config.UseBROnCarriageReturn || oDoc.queryCommandState( 'InsertOrderedList' ) || oDoc.queryCommandState( 'InsertUnorderedList' ) )
	{
		if ( type == 'ul' )
			decCommand( DECMD_UNORDERLIST ) ;
		else
			decCommand( DECMD_ORDERLIST ) ;
	}
	else
	{
		insertHtml('<' + type + '><li id="____tempLI">.</li></' + type + '>') ;
		
		var oLI = oDoc.getElementById( '____tempLI' ) ;
		oLI.removeAttribute("id") ;
		
		var oRange = oDoc.selection.createRange() ;
		oRange.moveToElementText( oLI ) ;
		oRange.findText( '.' ) ;
		oRange.select() ;
		oDoc.selection.clear() ;
	}
}

//function to perform spell check
var SpellCheck = function() 
{
	if ( ! config.SpellCheckerWord || ! SpellCheck.RunWord() )
	{
		if ( ! config.SpellCheckerIeSpell || ! SpellCheck.RunIeSpell() )
			alert( LangEntry( 'NoSpellCheck' ) ) ;
	}
}

SpellCheck.RunIeSpell = function()
{
	try 
	{
		var tmpis = new ActiveXObject( "ieSpell.ieSpellExtension" ) ;
		tmpis.CheckAllLinkedDocuments( objContent );
		return true ;
	}
	catch(exception) 
	{
		if( exception.number == -2146827859 )
		{
			if ( confirm( LangEntry( 'DownloadSpellChecker' ) ) )
			{
				window.open( config.SpellCheckerDownloadUrl , "SpellCheckerDownload" );
				return true ;
			}
		}
		else
			alert( "Error Loading ieSpell: Exception " + exception.number );

		return false ;
	}
}

// By EdwardRF to try use MS word spell checker.
SpellCheck.RunWord = function()
{
	try
	{
		var pstr = SpellCheck.Word_ExtractText( objContent.DOM.body.innerHTML ) ;
		var oWord = new ActiveXObject("Word.Application") ;
		oWord.WordBasic.FileNew() ;
		oWord.WordBasic.Insert(pstr) ;
		oWord.WordBasic.ToolsSpelling() ;
		oWord.WordBasic.EditSelectAll() ;
		oWord.Visible = false ;
		oWord.WordBasic.SetDocumentVar( "test", oWord.WordBasic.Selection ) ;
		str=oWord.WordBasic.GetDocumentVar("test") ;
		oWord.Documents.Close(0) ;
		oWord.Quit() ;
		str = SpellCheck.Word_FitIn( objContent.DOM.body.innerHTML, str ) ;
		objContent.DOM.body.innerHTML = str ;
		alert( LangEntry( 'SpellCheckFinished' ) ) ;
		
		return true ;
	}
	catch(e)
	{
		if ( oWord != null )
		oWord.Quit() ;
		return false ;
	}
}

//sub function for spell checking. By EdwardRF
//put back the spell checked text back to the text box
SpellCheck.Word_FitIn = function(ori,spl)
{
	spl = HTMLEncode(spl) ;
	var txt = "" ;
	var i = 0 ;
	var u = 0 ;
	var ue = 0 ;
	for( i = 0 ; i < spl.length ; i++ )
	{
		if( spl.charCodeAt(i) == 8203 )
		{
			u = ori.indexOf('<',u) ;
			ue = ori.indexOf('>',u) ;
			txt += ori.substring(u,ue+1) ;
			u++ ;
		}
		else if( spl.charCodeAt(i) == 65279 )
			txt += "&nbsp;" ;
		else
			txt += spl.charAt(i) ;
	}
	return txt;
}

//sub function for spell checking. By EdwardRF
//extract the text from then html of the text box and encode them.
//*NOTE: this function will subtitude all the html attributes with Unicode 8203(non-display space)
//	 and &nbsp; will be changed to Unicode 65279(non-display breaking space, just another kind of space)
//*IMPORTANT: So the document should not contain any Unicode Charactor 8203 or 65279.
//*REFERENCE: www.unicode.org
SpellCheck.Word_ExtractText = function(html)
{
	var txt="";
	var i=0;
	for(i=0;i<html.length;i++){
		if(html.charAt(i)=='<'){
			while(html.charAt(i)!='>') i++;
			txt+=String.fromCharCode(8203);
		}else if(html.charAt(i)=='&'){
			if(html.indexOf("&amp;",i)==i){
				txt+='&';
				i+=4;
			}
			if(html.indexOf("&quot;",i)==i){
				txt+='"';
				i+=5;
			}
			if(html.indexOf("&lt;",i)==i){
				txt+='<';
				i+=3;
			}
			if(html.indexOf("&gt;",i)==i){
				txt+='>';
				i+=3;
			}
			if(html.indexOf("&#146;",i)==i){
				txt+="'";
				i+=5;
			}
			if(html.indexOf("&nbsp;",i)==i){
				txt+=String.fromCharCode(65279);
				i+=5;
			}
		}else{
			txt+=html.charAt(i);
		}
	}
	return txt;
}

function checkbox(){
	var html = FCKShowDialog("dialog/fck_checkbox.html",window, 250, 120);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function radio(){
	var html = FCKShowDialog("dialog/fck_radio.html",window, 250, 120);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function textarea(){
	var html = FCKShowDialog("dialog/fck_textarea.html",window, 250, 100);
	if (html) insertHtml(html);
	SetFocus() ;
}

function textfield(){
	var html = FCKShowDialog("dialog/fck_textfield.html",window, 400, 120);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function form(){
	var html = FCKShowDialog("dialog/fck_form.html", window, 300, 170);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function button(){
	var html = FCKShowDialog("dialog/fck_button.html",window, 250, 120);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function hidden(){
	var html = FCKShowDialog("dialog/fck_texthidden.html",window, 250, 100);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function selectField()
{
	var html = FCKShowDialog("dialog/fck_select.html", window, 370, 320) ;
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function imageButton()
{
	var html = FCKShowDialog("dialog/fck_image.html?ImageButton", window, 430, 380);
	// The response is the IMG tag HTML
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function preview()
{
     var preview_window = window.open('', '', 'toolbar=no,location=no,status=yes,menubar=no,scrollbars=yes,resizable=yes,titlebar=1') ;
     preview_window.document.write( objContent.DOM.body.innerHTML );
     preview_window.document.close();
     preview_window.document.createStyleSheet( config.EditorAreaCSS );
}

function anchor()
{
	var html = FCKShowDialog("dialog/fck_anchor.html",window, 250, 120);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

/*function newPage()
{
	objContent.DOM.body.innerHTML = "" ;
	SetFocus() ;
}*/

function newPage() {	
	//var html = FCKShowDialog("dialog/codeSnippets.aspx",window, 300, 140);
	var html = FCKShowDialog("dialog/codeSnippetsIFrame.htm",window, 310, 227);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function replace() 
{
	objContent.focus();
	if ( typeof(rewin) == "undefined" || rewin.closed )
		rewin = window.open("dialog/fck_replace.html" ,"rewin","scrollbars=no,width=340,height=100, resizable=no, border=no") ;
	rewin.focus();
}

function save()
{
	var oLinkedField = parent.document.getElementsByName(URLParams['FieldName'])[0] ;
	oLinkedField.form.submit();
}

function insertUniversalKey()
{
	var html = FCKShowDialog("dialog/fck_universalkey.html", window, 415, 240);
	if (html) insertHtml(html) ;
	SetFocus() ;
}

function dialogTableAutoFormat(searchParentTable)
{
	if (searchParentTable)
	{
		var oRange  = objContent.DOM.selection.createRange() ;
		var oParent = oRange.parentElement() ;
		
		while (oParent && oParent.nodeName != "TABLE")
		{
			oParent = oParent.parentNode ;
		}
		
		if (oParent && oParent.nodeName == "TABLE")
		{
			var oControlRange = objContent.DOM.body.createControlRange();
			oControlRange.add( oParent ) ;
			oControlRange.select() ;
		}
		else
			return ;
	}

	FCKShowDialog("dialog/fck_tableautoformat.html", window, 350, 150);
	SetFocus() ;
}

function dialogList()
{
	if (checkDecCommand(DECMD_UNORDERLIST) == OLE_TRISTATE_CHECKED)
	{
		FCKShowDialog("dialog/fck_list.html", window, 300, 150);
		SetFocus() ;
	}
}
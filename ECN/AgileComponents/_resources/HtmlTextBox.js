var nv = navigator.appVersion.toUpperCase();
var isIE5p = false;
if (nv.indexOf("MSIE 5.0") != -1 || nv.indexOf("MSIE 5.5") != -1 || nv.indexOf("MSIE 6.0") != -1)
    isIE5p = true;
var HTB_IE_aspectRatio = 1.00;
var HTB_F_textRange = null;
var HTB_F_count = 0;
var HTB_ie5 = (document.all) ? true : false;
var HTB_ns6 = (navigator.appName == "Netscape") ? true : false;
//var HTB_F_textToFind= '';
var EditorID;

var HTB_emptyElements = {
    HR: true, BR: true, IMG: true, INPUT: true
};
var HTB_specialElements = {
    TEXTAREA: true
};

var BrowserDetect = {
    init: function() {
        this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
        this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
        this.OS = this.searchString(this.dataOS) || "an unknown OS";
    },
    searchString: function(data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            this.versionSearchString = data[i].versionSearch || data[i].identity;
            if (dataString) {
                if (dataString.indexOf(data[i].subString) != -1)
                    return data[i].identity;
            }
            else if (dataProp)
                return data[i].identity;
        }
    },
    searchVersion: function(dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },
    dataBrowser: [
		{ string: navigator.userAgent,
		    subString: "OmniWeb",
		    versionSearch: "OmniWeb/",
		    identity: "OmniWeb"
		},
		{
		    string: navigator.vendor,
		    subString: "Apple",
		    identity: "Safari"
		},
		{
		    prop: window.opera,
		    identity: "Opera"
		},
		{
		    string: navigator.vendor,
		    subString: "iCab",
		    identity: "iCab"
		},
		{
		    string: navigator.vendor,
		    subString: "KDE",
		    identity: "Konqueror"
		},
		{
		    string: navigator.userAgent,
		    subString: "Firefox",
		    identity: "Firefox"
		},
		{
		    string: navigator.vendor,
		    subString: "Camino",
		    identity: "Camino"
		},
		{		// for newer Netscapes (6+)
		    string: navigator.userAgent,
		    subString: "Netscape",
		    identity: "Netscape"
		},
		{
		    string: navigator.userAgent,
		    subString: "MSIE",
		    identity: "Explorer",
		    versionSearch: "MSIE"
		},
		{
		    string: navigator.userAgent,
		    subString: "Gecko",
		    identity: "Mozilla",
		    versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
		    string: navigator.userAgent,
		    subString: "Mozilla",
		    identity: "Netscape",
		    versionSearch: "Mozilla"
		}
	],
    dataOS: [
		{
		    string: navigator.platform,
		    subString: "Win",
		    identity: "Windows"
		},
		{
		    string: navigator.platform,
		    subString: "Mac",
		    identity: "Mac"
		},
		{
		    string: navigator.platform,
		    subString: "Linux",
		    identity: "Linux"
		}
	]

};
BrowserDetect.init();

if ("HTMLElement" in window) {
    HTMLElement.prototype.__defineGetter__("outerHTML", function() {
        var span = document.createElement("span"); span.appendChild(this.cloneNode(true));
        return span.innerHTML;
    });

    HTMLElement.prototype.__defineSetter__("outerHTML", function(html) {
        var range = document.createRange();
        this.innerHTML = html;
        range.selectNodeContents(this);
        var frag = range.extractContents();
        this.parentNode.insertBefore(frag, this);
        this.parentNode.removeChild(this);
    });
}

function HTB_getOuterHTML(node) {
    var html = '';
    switch (node.nodeType) {
        case Node.ELEMENT_NODE:
            html += '<';
            html += node.nodeName;
            if (!HTB_specialElements[node.nodeName]) {
                for (var a = 0; a < node.attributes.length; a++)
                    html += ' ' + node.attributes[a].nodeName.toUpperCase() +
                  '="' + node.attributes[a].nodeValue + '"';
                html += '>';
                if (!HTB_emptyElements[node.nodeName]) {
                    html += node.innerHTML;
                    html += '<\/' + node.nodeName + '>';
                }
            }
            else switch (node.nodeName) {
                case 'TEXTAREA':
                    for (var a = 0; a < node.attributes.length; a++)
                        if (node.attributes[a].nodeName.toLowerCase() != 'value')
                        html += ' ' + node.attributes[a].nodeName.toUpperCase() +
                      '="' + node.attributes[a].nodeValue + '"';
                    else
                        var content = node.attributes[a].nodeValue;
                    html += '>';
                    html += content;
                    html += '<\/' + node.nodeName + '>';
                    break;
            }
            break;
        case Node.TEXT_NODE:
            html += node.nodeValue;
            break;
        case Node.COMMENT_NODE:
            html += '<!' + '--' + node.nodeValue + '--' + '>';
            break;
    }
    return html;
}

function HTB_GetEditor(id) {
    if (HTB_ie5)
        return OBJNAME(eval(id + "_Editor"));
    else
        return HTB_GetObj(id + "_Editor").contentWindow;
}

function OBJNAME(n) { ''; return n; }

function HTB_GetContainer(id) {
    return HTB_GetObj(id);
}

function HTB_GetMode(id) {
    return HTB_GetObj(id + "_Mode");
}

function HTB_GetObj(id) {
    return document.getElementById(id)
}

function HTB_InitEditor(id) {
    editor = HTB_GetEditor(id);
    editor.document.open();
    editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
    editor.document.close();

    if (HTB_ns6 && eval(id + '_ContentCssFile') != '')
        document.write('<link rel="stylesheet" type="text/css" href="' + eval(id + '_ContentCssFile') + '">');

    enabled = true;

    if (enabled)
        HTB_EnableEditor(id);

    HTB_SetDesignMode(id, false);
    var mode = HTB_GetMode(id).value;
    HTB_SetMode(id, mode, true);

    if (eval(id + '_StartupFocus'))
        HTB_SetFocus(id);

    if (enabled)
        HTB_EnableEditor(id);
}

function HTB_PreloadIcons(id) {
    var icons = document.getElementById(id + '_Icons').value.split(',');
    var iconsPreloaded = document.getElementById('HTB_IconsPreloaded').value;
    for (index = 0; index < icons.length; index++) {
        if (icons[index] != '' && iconsPreloaded.indexOf(icons[index]) == -1) {
            var image = new Image();
            image.src = icons[index];

            if (iconsPreloaded != '')
                iconsPreloaded += ',';
            iconsPreloaded += icons[index];
        }
    }

    document.getElementById('HTB_IconsPreloaded').value = iconsPreloaded;
}

function HTB_SetEvalValue(obj, value) {
    eval("document.forms[0]." + obj + ".value") = value;
}

function HTB_EnableEditor(id) {
    editor = HTB_GetEditor(id);
    editor.document.contentEditable = 'True';
    editor.document.designMode = 'on';
}

function HTB_DisableEditor(id) {
    HTB_SaveData(id);
    HTB_ClearContent(id);

    editor = HTB_GetEditor(id);

    editor.document.open();
    editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
    editor.document.close();

    editor.document.contentEditable = 'False';
    editor.document.designMode = 'off';
}

function HTB_ClearContent(id) {
    editor = HTB_GetEditor(id);

    editor.document.body.innerHTML = '';
    if (HTB_ie5)
        editor.document.body.innerText = '';
}

function HTB_SetMode(id, mode, focus) {
    mode = mode.toUpperCase();

    if (mode == "HTML")
        HTB_SetHtmlMode(id, focus);
    else if (mode == "PREVIEW")
        HTB_SetPreviewMode(id, focus);
    else
        HTB_SetDesignMode(id, focus);
}

function HTB_ShowToolbarsContainer(id) {
    if (document.getElementById(id + ':_container') != null)
        document.getElementById(id + ':_container').style.display = '';
}

function HTB_HideToolbarsContainer(id) {
    if (document.getElementById(id + ':_container') != null)
        document.getElementById(id + ':_container').style.display = 'none';
}
function HTB_SetDesignMode(id, focus) {
    if (BrowserDetect.browser == "Explorer")
        HTB_SetDesignModeIE(id, focus);
    else
        HTB_SetDesignModeFF(id, focus);
}

/*FIREFOX*/
function HTB_SetDesignModeFF(id, focus) {
var size = HTB_GetSize(id);
var buffer;
mode = HTB_GetMode(id);

mode.value = "Design";

if (eval(id + '_PopupToolBars') == false)
HTB_HideToolbarsContainer(id);
else
HTB_HidePopupMenu(id);

HTB_ShowTab(id, '_design', '_preview,_html');

editor = HTB_GetEditor(id);
var hasInnerText = (editor.document.body.innerText != undefined) ? true : false;
    if(hasInnerText) {
        buffer = editor.document.body.innerText.replace(/\n/gi, ' ');
    } else {
        buffer = editor.document.body.textContent.replace(/\n/gi, ' ');
    }

editor.document.open();

if (HTB_ie5) {
editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
} else {
editor.document.write(HTB_Decode(buffer));
}

editor.document.close();

if (HTB_ie5) {
editor.document.contentEditable = 'True';
editor.document.designMode = 'on';
}
else {
editor.document.execCommand('readonly', false, 'true');
}

if (HTB_ie5)
editor.document.createStyleSheet(eval(id + '_ContentCssFile'));

HTB_InitEvents(id);

var maxLength = eval(id + '_MaxLength');
if (maxLength != null || maxLength > 0)
HTB_UpdateLength(id);

HTB_SetSize(id, size);

//if (focus)
HTB_SetFocus(id);

}
/*IE*/

function HTB_SetDesignModeIE(id, focus) {
    if (eval(id + '_AutoHideToolBars') == true || eval(id + '_PopupToolBars') == true) {
        HTB_HideToolbarsContainer(id);
        if (eval(id + '_PopupToolBars') == true)
            HTB_ShowPopupMenu(id);
    }
    else {
        HTB_ShowToolbarsContainer(id);
    }

    HTB_ShowTab(id, '_design', '_preview,_html');

    var buffer;
    mode = HTB_GetMode(id);

    editor = HTB_GetEditor(id);
    var bufferToWrite = false;
    if (mode.value.toUpperCase() == "HTML" || (mode.value.toUpperCase() == "PREVIEW" && HTB_ns6)) {
        if (HTB_ie5)
            buffer = editor.document.body.innerText;
        else
            buffer = editor.document.body.innerHTML;
        bufferToWrite = true;
    }

    if (HTB_ie5) {
        editor.document.contentEditable = 'True';
        editor.document.designMode = 'on';

    }
    else {
        editor.document.execCommand('readonly', false, 'true');
    }

    editor.document.open();
    if (bufferToWrite) {
        if (HTB_ie5)
            editor.document.write(buffer);
        else
            editor.document.write(HTB_Decode(buffer));
    }
    else
        editor.document.write(HTB_Decode(HTB_GetContainer(id).value));

    editor.document.close();
    if (HTB_ie5)
        editor.document.createStyleSheet(eval(id + '_ContentCssFile'));

    mode.value = "Design";

    HTB_InitEvents(id);

    var maxLength = eval(id + '_MaxLength');
    if (maxLength != null || maxLength > 0)
        HTB_UpdateLength(id);

    //if (focus)
        HTB_SetFocus(id);

}//*/

function HTB_ToggleMode(id) {
    mode = HTB_GetMode(id);
    if (mode.value == "HTML")
        HTB_SetDesignMode(id, true);
    else
        HTB_SetHtmlMode(id, true);
}

function HTB_SetHtmlMode(id, focus) {
    var size = HTB_GetSize(id);
    var buffer;
    mode = HTB_GetMode(id);

    mode.value = "HTML";

    if (eval(id + '_PopupToolBars') == false)
        HTB_HideToolbarsContainer(id);
    else
        HTB_HidePopupMenu(id);

    HTB_ShowTab(id, '_html', '_design,_preview');

    editor = HTB_GetEditor(id);
    var buffer = editor.document.body.innerHTML;
    editor.document.open();
    if (HTB_ie5)
        editor.document.write(HTB_GetContainer(id).value);
    else {
        editor.document.write(HTB_Encode(buffer));
    }
    editor.document.close();
    if (HTB_ie5) {
        editor.document.contentEditable = 'True';
        editor.document.designMode = 'On';
    }
    else {
        editor.document.execCommand('readonly', false, 'true');
    }

    HTB_ForceUseBR(id);

    if (focus)
        HTB_SetFocus(id);

    HTB_SetSize(id, size);
}

function HTB_GetSize(id) {
    var area = document.getElementById(id + '__AREA__');
    return area.clientWidth;
}

function HTB_SetSize(id, size) {
    var area = document.getElementById(id + '__AREA__');
    area.style.width = parseInt(size);
}


function HTB_SetPreviewMode(id, focus) {
    var size = HTB_GetSize(id);

    mode = HTB_GetMode(id);
    mode.value = "Preview";

    if (eval(id + '_PopupToolBars') == false)
        HTB_HideToolbarsContainer(id);
    else
        HTB_HidePopupMenu(id);

    HTB_ShowTab(id, '_preview', '_design,_html');

    var buffer;
    mode = HTB_GetMode(id);

    editor = HTB_GetEditor(id);

    if (HTB_ns6) {
        buffer = editor.document.body.innerHTML;
        previewMode = mode.value;
    }

    mode.value = "Preview";

   
    var hasInnerText = (editor.document.body.innerText != undefined) ? true : false;
    if (hasInnerText) {
        buffer = editor.document.body.innerText;
    } else {
        buffer = editor.document.body.textContent.replace(/\n/gi, ' ');
    }
    editor.document.open();

    if (BrowserDetect.browser != 'Firefox') {
        editor.document.contentEditable = 'False';
        editor.document.designMode = 'Off';
    }

    if (HTB_ie5) {
        //editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
        editor.document.write(HTB_Decode(buffer));
    }
    else {
        if (previewMode.toUpperCase() == 'HTML') {
            editor.document.write(HTB_Encode(buffer));
        } else {
            editor.document.write(HTB_Decode(buffer));
        }
    }

    editor.document.close();
    if (HTB_ie5) {
        editor.document.contentEditable = 'False';
        editor.document.designMode = 'Off';
        editor.document.createStyleSheet(eval(id + '_ContentCssFile'));
    }
    else {
        editor.document.execCommand('readonly', false, 'false');
    }
    
    if (BrowserDetect.browser != 'Firefox') {
        editor.document.close();
    }
    
    HTB_SetSize(id, size);

   // if (focus)
        HTB_SetFocus(id);
}

function HTB_ShowPopupMenu(id) {
    if (ATB_existPopup(id + '_' + id + '_ToolbarsPopup'))
        ATB_showPopup(id + '_' + id + '_ToolbarsPopup');
}

function HTB_HidePopupMenu(id) {
    if (ATB_existPopup(id + '_' + id + '_ToolbarsPopup'))
        ATB_hidePopup(id + '_' + id + '_ToolbarsPopup');
}

function HTB_ResetTabs(id, tohide) {
    if (tohide && tohide.length > 0) {
        aTabs = tohide.split(',');
        for (index = 0; index < aTabs.length; index++)
            HTB_HideTab(id, aTabs[index]);
    }
}

function HTB_HideTab(id, name) {
    tab = HTB_GetObj(id + name);
    tab.style.borderRight = '';
    tab.style.borderTop = '';
    tab.style.borderLeft = '';
    tab.style.borderBottom = '';
    tab.style.backgroundColor = '';
}

function HTB_ShowTab(id, name, tohide) {
    try {
        HTB_ResetTabs(id, tohide)
        tab = HTB_GetObj(id + name);
        tab.style.borderRight = 'black 1px solid';
        tab.style.borderTop = 'black 1px solid';
        tab.style.borderLeft = 'black 1px solid';
        tab.style.borderBottom = 'black 1px solid';
        tab.style.backgroundColor = 'white';
    }
    catch (e) {
    }
}

function HTB_InitEvents(editorid) {
    var iframes = null;
    if (HTB_ie5)
        iframes = document.all.tags("IFRAME");
    else
        iframes = document.getElementsByTagName("IFRAME");
    for (index = 0; index < iframes.length; index++) {
        if (iframes[index].id == editorid + '_Editor') {
            iframe = iframes[index];
            break;
        }
    }
    HTB_InitEventHandlers(iframe, editorid);
}

function HTB_Decode(str) {
    str = HTB_StringReplace(str, '&lt;', '<');
    str = HTB_StringReplace(str, '&gt;', '>');
    return str;
}

function HTB_Encode(str) {
    str = HTB_StringReplace(str, '<', '&lt;');
    str = HTB_StringReplace(str, '>', '&gt;');
    return str;
}

function HTB_StringReplace(str1, str2, str3) {
    str1 = str1.split(str2).join(str3);
    return str1;
}

function HTB_SetHtml(id, str) {
    mode = HTB_GetMode(id).value.toUpperCase();
    if (mode == "HTML") {
        HTB_SetDesignMode(id, false);
        HTB_GetEditor(id).document.body.innerHTML = str;
        HTB_SetHtmlMode(id, false);
    }
    else
        HTB_GetEditor(id).document.body.innerHTML = str;
}

function HTB_GetHtml(id) {
    mode = HTB_GetMode(id).value.toUpperCase();
    if (mode == "HTML") {
        //HTB_SetDesignMode(id, false);
        txt = HTB_GetEditor(id).document.body.innerHTML;
        //HTB_SetHtmlMode(id, false);
        return txt;
    }
    else {
        return HTB_GetEditor(id).document.body.innerHTML;
    }
}

function HTB_GetText(id) {
    mode = HTB_GetMode(id).value.toUpperCase();

    if (mode == "HTML") {
        HTB_SetDesignMode(id, false);
        txt = HTB_GetEditor(id).document.body.innerText;
        HTB_SetHtmlMode(id, false);
        return txt;
    }
    else {
        txt = HTB_GetEditor(id).document.body.innerText;
        return txt;
    }
}

function HTB_SaveData(id) {
    if (HTB_ie5)
        eval(id + '_State').SaveSelection(id);

    var content = HTB_GetHtml(id);
    if (HTB_ns6)
        content = HTB_EscapeVal(content, '');

    if (content == '<P>&nbsp;</P>')
        content = '';
    if (eval(id + "_HackProtection"))
        content = HTB_ProtectString(id, content);
    document.getElementById(id).value = HTB_Encode(content);

    /*container = HTB_GetContainer(id);
    container.value = HTB_Encode(HTB_GetHtml(id));

	if (container.value == '&lt;P&gt;&nbsp;&lt;/P&gt;')
    container.value = '';*/
}

function HTB_ProtectString(id, content) {
    content = HTB_StringReplace(content, 'javascript:', 'javascript :');
    var RegExp = /<script.*>(.|\n)*<\/script.*>/ig;
    content = content.replace(RegExp, "<!-- Script Filtered -->");
    return content;
}

function HTB_GetId(id) {
    return stringReplace(id, ':', '_');
}

function HTB_CommandBuilder(id, name, arg) {
    //HTB_DebugTrace(id, name)
    state = eval(id + '_State');
    state.RestoreSelection();
    editor = HTB_GetEditor(id);
    editor.focus();

    if (arg != null && arg.toLowerCase() == 'true')
        editor.document.execCommand(name, true, '');
    else if (name == 'uploadedimage')
        editor.document.execCommand('insertimage', false, arg);
    else if (arg == null)
        editor.document.execCommand(name, '', null);
    else if (arg != null)
        editor.document.execCommand(name, false, arg);
    else
        editor.document.execCommand(name, '', null);

}

function HTB_DebugTrace(id, str) {
    debugWindows = HTB_GetObj(id + '_Debug');
    debugWindows.value = str + "\r\n" + debugWindows.value;
}

// Public API

function HTB_SetFocus(id) {
    editor = HTB_GetEditor(id);
    editor.focus();
}

// Temp

function HTB_SetBold(id) {
    HTB_CommandBuilder(id, 'bold', null);
}

function HTB_SetItalic(id) {
    HTB_CommandBuilder(id, 'italic', null);
}

function HTB_SetUnderline(id) {
    HTB_CommandBuilder(id, 'underline', null);
}

function HTB_OnColorOver(objTable) {
    HTB_SetBorderColor(objTable, '#0A246A');
    HTB_SetBackColor(objTable, '#B6BDD2');
}

function HTB_OnColorOff(objTable) {
    HTB_SetBorderColor(objTable, '#F9F8F7');
    HTB_SetBackColor(objTable, '#F9F8F7');
}

function HTB_SetBackColor(obj, color) {
    if (obj.id == 'HTB_SampleColor') {

    }
    obj.style.backgroundColor = color;
}

function HTB_SetBorderColor(obj, color) {
    obj.style.borderColor = color;
}

function HTB_BuildColorTable(editorId, id, onClick, disableCustom, typeColor) {
    var str, color216 = new Array('00', '33', '66', '99', 'CC', 'FF');
    var colorLen = color216.length, color = '';
    cellWidth = 12;
    cellHeight = 12;

    str = '<table><tr><td><table width=225 cellspacing=0 cellpadding=0 onselectstart=\'return false\'>';

    for (var f = 0; f < 2; f++) {
        for (var r = 0; r < colorLen; r++) {
            str += '<tr>';
            for (var g = colorLen - (1 + (f * 3)); g >= 3 - (f * 3); g--) {
                for (var b = colorLen - 1; b >= 0; b--) {

                    color = color216[r] + color216[g] + color216[b];

                    str += '<td><table width=' + cellWidth + ' height=' + cellHeight + ' cellpadding=0 cellspacing=0><tr><td style=\'cursor:hand\''
						+ ' bgcolor=\'#' + color + '\''
						+ ' title=\'#' + color + '\''
						+ ' onmouseover=\"HTB_SetValue(document.getElementById(\'' + id + '_Color\'), \'' + color + '\');HTB_SetBackColor(document.getElementById(\'' + id + '_SampleColor\'), \'#' + color + '\')\" '
						+ (onClick ? 'onclick=\"' + HTB_StringReplace(onClick, '$color$', '\'#' + color + '\'') + '\" ' : '')
						+ '></td></tr></table></td>\n';
                }
            }
            str += '</tr>';
        }
    }

    str += '<tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
    str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
    str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' id=\'' + id + '_Color\' maxlength=6 size=7 onkeyup=\'HTB_CustomColorKeyUp(this,\"' + id + '\");\'' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\' + HTB_SetColorTableOnClick(\"' + editorId + '\",\"' + id + '\",\"' + typeColor + '\");\'></span></td></tr></table>';
    str += '</td></tr></table></td></tr></table>';
    return str;

}

function HTB_BuildColorTableMozilla(editorId, id, onClick, disableCustom, typeColor, spacer) {
    var str, color216 = new Array('00', '33', '66', '99', 'CC', 'FF');
    var colorLen = color216.length, color = '';
    cellWidth = 12;
    cellHeight = 12;

    str = '<table><tr><td><table width=212 cellspacing=0 cellpadding=0 onselectstart=\'return false\'>';

    for (var f = 0; f < 2; f++) {
        for (var r = 0; r < colorLen; r++) {
            str += '<tr>';
            for (var g = colorLen - (1 + (f * 3)); g >= 3 - (f * 3); g--) {
                for (var b = colorLen - 1; b >= 0; b--) {

                    color = color216[r] + color216[g] + color216[b];

                    str += '<td><table width=' + cellWidth + ' height=' + cellHeight + ' cellpadding=0 cellspacing=0><tr><td style=\'cursor:pointer\''
						+ ' bgcolor=\'#' + color + '\''
						+ ' title=\'#' + color + '\''
						+ ' onmouseover=\"HTB_SetValue(document.getElementById(\'' + id + '_Color\'), \'' + color + '\');HTB_SetBackColor(document.getElementById(\'' + id + '_SampleColor\'), \'#' + color + '\')\" '
						+ (onClick ? 'onclick=\"' + HTB_StringReplace(onClick, '$color$', '\'#' + color + '\'') + '\" ' : '')
						+ '><img height=10 width=10 src=\'' + spacer + '\'/></td></tr></table></td>\n';
                }
            }
            str += '</tr>';
        }
    }

    /*str += '<tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
    str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1px;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
    str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' id=\'' + id + '_Color\' maxlength=6 size=7 onkeyup=\'HTB_CustomColorKeyUp(this,\"' + id + '\");\'' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\' + HTB_SetColorTableOnClick(\"' + editorId + '\",\"' + id + '\",\"' + typeColor + '\");\'></span></td></tr></table>';
    str += '</td></tr></table></td></tr></table>';*/

    str += '</td></tr></table></td></tr></table>';
    str += '<table><tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
    str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
    str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' id=\'' + id + '_Color\' maxlength=6 size=7 onkeyup=\'HTB_CustomColorKeyUp(this,\"' + id + '\");\'' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\' + HTB_SetColorTableOnClick(\"' + editorId + '\",\"' + id + '\",\"' + typeColor + '\");\'></span></td></tr></table></table>';

    return str;
}

function HTB_SetColorTableOnClick(editorId, popupId, typeColor) {
    HTB_SetColorEditor(editorId, typeColor, document.getElementById(popupId + '_Color').value, false);
    ATB_hidePopup(popupId + '_CustomColors');
}

function HTB_CustomColorKeyUp(input, editorid) {
    input.value = input.value.toUpperCase();
    input.value = input.value.replace(/[^\dA-F]*/gi, "");
    if (input.value.length == 6)
        HTB_SetBackColor(document.getElementById(editorid + '_SampleColor'), '#' + document.getElementById(editorid + '_Color').value);
    else
        HTB_SetBackColor(document.getElementById(editorid + '_SampleColor'), '#FFFFFF');

}

function HTB_BuildTableEditor(id, editorid, popupid) {
    var str = '';

    str += '<table class=\'HTB_clsPopup\'>';
    str += '	<tr>';
    str += '		<td>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 63) + '</span></td>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td width=\'5\'></td>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 74) + ':</span></td>';
    str += '					<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'' + editorid + '_HTB_TE_tbRows\'></td>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 75) + ':</span></td>';
    str += '					<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'' + editorid + '_HTB_TE_tbCols\'></td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 76) + '</span></td>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td width=\'5\'></td>';
    str += '					<td>';
    str += '						<table cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 59) + ':</span></td>';
    str += '								<td><select style=\'width:129px;height:21px\' id=\'' + editorid + '_HTB_TE_alignment\'>';
    str += '										<option value=\'Default\'>' + HTB_GetTextLabel(editorid, 101) + '</option>';
    str += '										<option value=\'Left\'>' + HTB_GetTextLabel(editorid, 106) + '</option>';
    str += '										<option value=\'Right\'>' + HTB_GetTextLabel(editorid, 108) + '</option>';
    str += '										<option value=\'Center\'>' + HTB_GetTextLabel(editorid, 111) + '</option>';
    str += '									</select></td>';
    str += '								<td>';
    str += '									<input type=\'checkbox\' id=\'' + editorid + '_HTB_TE_specifyWidth\' onclick=\"HTB_EnableDisableSpecify(\'' + editorid + '_HTB_TE\',\'Width\');\">';
    str += '								</td>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 77) + ':</span></td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 78) + ':</span></td>';
    str += '								<td><select style=\'width:129px;height:21px\' id=\'' + editorid + '_HTB_TE_float\'>';
    str += '										<option value=\'Default\'>' + HTB_GetTextLabel(editorid, 101) + '</option>';
    str += '										<option value=\'Left\'>' + HTB_GetTextLabel(editorid, 106) + '</option>';
    str += '										<option value=\'Right\'>' + HTB_GetTextLabel(editorid, 108) + '</option>';
    str += '									</select></td>';
    str += '								<td>&nbsp;</td>';
    str += '								<td valign=\'middle\'>';
    str += '									<table cellpadding=\'0\' cellspacing=\'0\'>';
    str += '										<tr>';
    str += '											<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:60px;height:20px\' id=\'' + editorid + '_HTB_TE_specifyValueWidth\'></td>';
    str += '											<td><span class=\'HTB_clsFont\'><input type=\'radio\' name=\'' + editorid + '_HTB_TE_gWidth\' id=\'' + editorid + '_HTB_TE_specifyInPixelsWidth\' value=\'Pixels\'> ' + HTB_GetTextLabel(editorid, 67) + '<br>';
    str += '								<input type=\'radio\' name=\'' + editorid + 'HTB_TE_gWidth\' id=\'' + editorid + '_HTB_TE_specifyInPercentWidth\' value=\'Percent\'> ' + HTB_GetTextLabel(editorid, 68) + '</span></td>';
    str += '										</tr>';
    str += '									</table>';
    str += '								</td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 79) + ':</span></td>';
    str += '								<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:61px;height:20px\' id=\'' + editorid + '_HTB_TE_cellPadding\'></td>';
    str += '								<td><input type=\'checkbox\' id=\'' + editorid + '_HTB_TE_specifyHeight\' onclick=\"HTB_EnableDisableSpecify(\'' + editorid + '_HTB_TE\',\'Height\');\"></td>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 81) + ':</span></td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 80) + ':</span></td>';
    str += '								<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:61px;height:20px\' id=\'' + editorid + '_HTB_TE_cellSpacing\'></td>';
    str += '								<td>&nbsp;</td>';
    str += '								<td valign=\'middle\'>';
    str += '									<table cellpadding=\'0\' cellspacing=\'0\'>';
    str += '										<tr>';
    str += '											<td><input value=\'0\' type=\'text\' size=\'3\' style=\'width:60px;height:20px\' id=\'' + editorid + '_HTB_TE_specifyValueHeight\'></td>';
    str += '											<td><span class=\'HTB_clsFont\'><input type=\'radio\' name=\'' + editorid + '_HTB_TE_gHeight\' id=\'' + editorid + '_HTB_TE_specifyInPixelsHeight\' value=\'Pixels\'> ' + HTB_GetTextLabel(editorid, 67) + '<br>';
    str += '								<input disabled type=\'radio\' name=\'' + editorid + '_HTB_TE_gHeight\' id=\'' + editorid + '_HTB_TE_specifyInPercentHeight\' value=\Percent\'> ' + HTB_GetTextLabel(editorid, 68) + '</span></td>';
    str += '										</tr>';
    str += '									</table>';
    str += '								</td>';
    str += '							</tr>';
    str += '						</table>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 82) + '</span></td>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td width=\'5\'></td>';
    str += '					<td>';
    str += '						<table width=\'100%\' cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 83) + ':</span></td>';
    str += '								<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'' + editorid + '_HTB_TE_borderSize\'></td>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 85) + ':</span></td>';
    str += '								<td>' + HTB_CreateColoredDropDown(editorid, editorid + '_HTB_TE_lightBorderColor') + '</td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 84) + ':</span></td>';
    str += '								<td>' + HTB_CreateColoredDropDown(editorid, editorid + '_HTB_TE_borderColor') + '</td>';
    str += '								<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 86) + ':</span></td>';
    str += '								<td>' + HTB_CreateColoredDropDown(editorid, editorid + '_HTB_TE_darkBorderColor') + '</td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td colspan=\'4\'><input type=\'checkbox\' id=\'' + editorid + '_HTB_TE_collapseTableBorder\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 87) + '</span></td>';
    str += '							</tr>';
    str += '						</table>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 88) + '</span></td>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td width=\'5\'></td>';
    str += '					<td>';
    str += '						<table width=\'100%\' cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>';
    str += '							<tr>';
    str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 84) + ':</span>';
    str += HTB_CreateColoredDropDown(editorid, editorid + '_HTB_TE_bgColor') + '</td>';
    str += '								<td><input type=\'checkbox\' id=\'' + editorid + '_HTB_TE_useBackgroundPicture\' onclick=\"HTB_EnableDisableBackgroundImage(\'' + editorid + '\');\"><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 89) + ':</span><br>';
    str += '								<input type=\'text\' style=\'width:100%;height:20px\' id=\'' + editorid + '_HTB_TE_backgroundPictureValue\'></td>';
    str += '							</tr>';
    str += '						</table>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 141) + '</span></td>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\' cellpadding=\'0\' cellspacing=\'0\'>';
    str += '				<tr>';
    str += '					<td width=\'15\'></td>';
    str += '					<td>';
    str += '					<td>';
    str += '						<table width=\'100%\' cellpadding=\'0\' cellspacing=\'0\' border=\'0\'>';
    str += '							<tr>';
    str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 65) + ':</span>';
    str += '								</td>';
    str += '								<td><input type=\'text\' style=\'width:100px;height:20px\' id=\'' + editorid + '_HTB_TE_widthCell\'></td>';
    str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 142) + ':</span>';
    str += '								</td>';
    str += '								<td><select style=\'width:129px;height:21px\' id=\'' + editorid + '_HTB_TE_horizAlignment\'>';
    str += '										<option value=\'Default\'>' + HTB_GetTextLabel(editorid, 101) + '</option>';
    str += '										<option value=\'Left\'>' + HTB_GetTextLabel(editorid, 106) + '</option>';
    str += '										<option value=\'Right\'>' + HTB_GetTextLabel(editorid, 108) + '</option>';
    str += '										<option value=\'Center\'>' + HTB_GetTextLabel(editorid, 111) + '</option>';
    str += '									</select></td>';
    str += '							</tr>';
    str += '							<tr>';
    str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 66) + ':</span>';
    str += '								</td>';
    str += '								<td><input type=\'text\' style=\'width:100px;height:20px\' id=\'' + editorid + '_HTB_TE_heightCell\'></td>';
    str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>' + HTB_GetTextLabel(editorid, 143) + ':</span>';
    str += '								</td>';
    str += '								<td><select style=\'width:129px;height:21px\' id=\'' + editorid + '_HTB_TE_vertAlignment\'>';
    str += '										<option value=\'Default\'>' + HTB_GetTextLabel(editorid, 101) + '</option>';
    str += '										<option value=\'Baseline\'>' + HTB_GetTextLabel(editorid, 104) + '</option>';
    str += '										<option value=\'Bottom\'>' + HTB_GetTextLabel(editorid, 105) + '</option>';
    str += '										<option value=\'Middle\'>' + HTB_GetTextLabel(editorid, 107) + '</option>';
    str += '										<option value=\'Top\'>' + HTB_GetTextLabel(editorid, 110) + '</option>';
    str += '									</select></td>';
    str += '							</tr>';
    str += '						</table>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '			<table width=\'100%\'>';
    str += '				<tr>';
    str += '					<td align=\'right\'>';
    str += '						<input type=\'button\' value=\'' + HTB_GetTextLabel(editorid, 72) + '\' style=\'width:75px;height:23px\' onclick=\"HTB_CreateTableFromEditor(\'' + editorid + '\',\'' + popupid + '\')\">&nbsp;&nbsp;';
    str += '						<input type=\'button\' value=\'' + HTB_GetTextLabel(editorid, 73) + '\' style=\'width:75px;height:23px\' onclick=\"ATB_hidePopup(\'' + id + '_TableEditor\');\">';
    str += '					</td>';
    str += '				</tr>';
    str += '			</table>';
    str += '		</td>';
    str += '	</tr>';
    str += '</table>';
    return str;
}

function HTB_GetSelectedRadioValue(group) {
    for (var i = 0; i < group.length; i++)
        if (group[i].checked)
        return group[i].value;
}

function HTB_GetSelectedRadioNS(buttonGroup) {
    if (buttonGroup[0]) {
        for (var i = 0; i < buttonGroup.length; i++) {
            if (buttonGroup[i].checked) {
                return i
            }
        }
    }
    else {
        if (buttonGroup.checked) {
            return 0;
        }
    }

    return -1;
}

function HTB_GetSelectedRadioValueNS(buttonGroup) {
    var i = HTB_GetSelectedRadioNS(buttonGroup);
    if (i == -1) {
        return "";
    }
    else {
        if (buttonGroup[i]) {
            return buttonGroup[i].value;
        }
        else {
            return buttonGroup.value;
        }
    }
}

function HTB_CreateTableFromEditor(id, popupid) {
    var h = '', w = '';

    if (document.getElementById(id + '_HTB_TE_specifyHeight').checked) {
        h = document.getElementById(id + '_HTB_TE_specifyValueHeight').value;
        if (h != '' && h != 0) {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(document.getElementById(id + '_HTB_TE_gHeight'));
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName(id + '_HTB_TE_gHeight'));
            if (selectedValue == 'Pixels') {
                h += 'px';
            }
            else {
                h += '%';
            }
        }
    }

    if (document.getElementById(id + '_HTB_TE_specifyWidth')) {
        w = document.getElementById(id + '_HTB_TE_specifyValueWidth').value;
        if (w != '' && w != '0') {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(document.getElementById(id + '_HTB_TE_gWidth'));
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName(id + '_HTB_TE_gWidth'));
            if (selectedValue == 'Pixels') {
                w += 'px';
            }
            else {
                w += '%';
            }
        }

    }

    var background = '';
    if (document.getElementById(id + '_HTB_TE_useBackgroundPicture').checked) {
        background = document.getElementById(id + '_HTB_TE_backgroundPictureValue').value;
    }

    var floatAlign = HTB_GetSelectedOptionText(id + '_HTB_TE_float');
    if (floatAlign == 'Default')
        floatAlign = '';

    var align = HTB_GetSelectedOptionText(id + '_HTB_TE_alignment');
    if (align == 'Default')
        align = '';

    var hAlignCell = HTB_GetSelectedOptionText(id + '_HTB_TE_horizAlignment');
    if (hAlignCell == 'Default')
        hAlignCell = '';

    var vAlignCell = HTB_GetSelectedOptionText(id + '_HTB_TE_vertAlignment');
    if (vAlignCell == 'Default')
        vAlignCell = '';

    if (HTB_ie5) {
        state = eval(id + '_State');
        sel = state.GetSelection(id);
        if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'TABLE') {
            table = sel.item(0);
            table.style.cssText = '';

            if (h != null && h != '' && h != 0)
                table.height = h;
            else
                table.height = '';

            if (w != null && w != '' && w != 0)
                table.width = w;
            else
                table.width = '';

            var border = document.getElementById(id + '_HTB_TE_borderSize').value;
            if (border != null && border != '' && border != 0) {
                table.border = border;
            }
            else {
                table.border = '0';
                table.style.borderRight = 'silver 1px dotted';
                table.style.borderLeft = 'silver 1px dotted';
                table.style.borderTop = 'silver 1px dotted';
                table.style.borderBottom = 'silver 1px dotted';
            }

            var bordercolor = HTB_GetSelectedColoredDropDown(id + '_HTB_TE_borderColor');
            if (bordercolor != null && bordercolor != '' && border != 0)
                table.borderColor = bordercolor;
            else
                table.borderColor = '';

            var bgcolor = HTB_GetSelectedColoredDropDown(id + '_HTB_TE_bgColor');
            if (bgcolor != null && bgcolor != '')
                table.bgColor = bgcolor;

            if (background != null && background != '')
                table.background = background;

            var bordercolorlight = HTB_GetSelectedColoredDropDown(id + '_HTB_TE_lightBorderColor');
            if (bordercolorlight != null && bordercolorlight != '')
                table.borderColorLight = bordercolorlight;

            var bordercolordark = HTB_GetSelectedColoredDropDown(id + '_HTB_TE_darkBorderColor');
            if (bordercolordark != null && bordercolordark != '')
                table.borderColorDark = bordercolordark;

            var cellspacing = document.getElementById(id + '_HTB_TE_cellSpacing').value;
            if (cellspacing)
                table.cellspacing = cellspacing;

            var cellpadding = document.getElementById(id + '_HTB_TE_cellSpacing').value;
            if (cellpadding)
                table.cellpadding = cellpadding;

            if (align)
                table.align = align;

            if (document.getElementById(id + '_HTB_TE_collapseTableBorder').checked) {
                table.style.borderCollapse = 'collapse';
            }
            else
                table.style.borderCollapse = 'separate';

            var base = table.getElementsByTagName("tr").item(0).parentNode;
            for (var i = 0; i < base.childNodes.length; i++) {
                var tr = base.childNodes[i];

                for (var j = 0; j < tr.childNodes.length; j++) {
                    var td = tr.childNodes[j];
                    td.style.cssText = '';

                    if (border == null || border == '' || border == 0) {
                        td.style.borderRight = 'silver 1px dotted';
                        td.style.borderLeft = 'silver 1px dotted';
                        td.style.borderTop = 'silver 1px dotted';
                        td.style.borderBottom = 'silver 1px dotted';
                    }

                    var cellWidth = HTB_TE_widthCell.value;
                    if (cellWidth != null && cellWidth != '' && cellWidth != '0')
                        td.width = cellWidth;

                    var cellHeight = HTB_TE_heightCell.value;
                    if (cellHeight != null && cellHeight != '' && cellHeight != '0')
                        td.height = cellHeight;

                    var hCellAlign = hAlignCell;
                    if (hCellAlign != '')
                        td.align = hCellAlign;

                    var vCellAlign = vAlignCell;
                    if (vCellAlign != '')
                        td.vAlign = vCellAlign;
                }
            }
        }
        else {
            HTB_InsertTable(id, document.getElementById(id + '_HTB_TE_tbCols').value, document.getElementById(id + '_HTB_TE_tbRows').value, w, h, document.getElementById(id + '_HTB_TE_borderSize').value, HTB_GetSelectedColoredDropDown(id + '_HTB_TE_borderColor'), HTB_GetSelectedColoredDropDown(id + '_HTB_TE_lightBorderColor'), HTB_GetSelectedColoredDropDown(id + '_HTB_TE_darkBorderColor'), document.getElementById(id + '_HTB_TE_cellSpacing').value, document.getElementById(id + '_HTB_TE_cellPadding').value, floatAlign, align, background, HTB_GetSelectedColoredDropDown(id + '_HTB_TE_bgColor'), document.getElementById(id + '_HTB_TE_collapseTableBorder').checked, 1, document.getElementById(id + '_HTB_TE_widthCell').value, document.getElementById(id + '_HTB_TE_heightCell').value, hAlignCell, vAlignCell);
        }
    }

    else {
        HTB_InsertTable(id, document.getElementById(id + '_HTB_TE_tbCols').value, document.getElementById(id + '_HTB_TE_tbRows').value, w, h, document.getElementById(id + '_HTB_TE_borderSize').value, HTB_GetSelectedColoredDropDown(id + '_HTB_TE_borderColor'), HTB_GetSelectedColoredDropDown(id + '_HTB_TE_lightBorderColor'), HTB_GetSelectedColoredDropDown(id + '_HTB_TE_darkBorderColor'), document.getElementById(id + '_HTB_TE_cellSpacing').value, document.getElementById(id + '_HTB_TE_cellPadding').value, floatAlign, align, background, HTB_GetSelectedColoredDropDown(id + '_HTB_TE_bgColor'), document.getElementById(id + '_HTB_TE_collapseTableBorder').checked, 1, document.getElementById(id + '_HTB_TE_widthCell').value, document.getElementById(id + '_HTB_TE_heightCell').value, hAlignCell, vAlignCell);
    }

    ATB_hidePopup(popupid);
}

function HTB_InitTableEditor(editorid) {
    try {
        if (HTB_ie5) {
            state = eval(editorid + '_State');
            sel = state.GetSelection(editorid);
            if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'TABLE') {
                table = sel.item(0);

                document.getElementById(editorid + '_HTB_TE_tbRows').value = table.rows.length;
                document.getElementById(editorid + '_HTB_TE_tbRows').disabled = true;
                document.getElementById(editorid + '_HTB_TE_tbCols').value = table.rows[0].cells.length;
                document.getElementById(editorid + '_HTB_TE_tbCols').disabled = true;
                document.getElementById(editorid + '_HTB_TE_alignment').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_float').selectedIndex = 0;

                var w = table.width;
                if (w != '') {
                    document.getElementById(editorid + '_HTB_TE_specifyWidth').checked = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPercentWidth').disabled = false;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').disabled = false;
                    document.getElementById(editorid + '_HTB_TE_specifyValueWidth').disabled = false;
                    if (w.indexOf('%') > 0) {
                        document.getElementById(editorid + '_HTB_TE_specifyInPercentWidth').checked = true;
                    }
                    else {
                        document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').checked = true;
                    }
                }
                else {
                    document.getElementById(editorid + '_HTB_TE_specifyValueWidth').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyWidth').checked = false;
                    document.getElementById(editorid + '_HTB_TE_specifyInPercentWidth').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').checked = true;
                }

                var h = table.height;
                if (h != '') {
                    document.getElementById(editorid + '_HTB_TE_specifyHeight').checked = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPercentHeight').disabled = false;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').disabled = false;
                    document.getElementById(editorid + '_HTB_TE_specifyValueHeight').disabled = false;
                    if (h.indexOf('%') > 0) {
                        document.getElementById(editorid + '_HTB_TE_specifyInPercentHeight').checked = true;
                    }
                    else {
                        document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').checked = true;
                    }
                }
                else {
                    document.getElementById(editorid + '_HTB_TE_specifyValueHeight').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyHeight').checked = false;
                    document.getElementById(editorid + '_HTB_TE_specifyInPercentHeight').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').checked = true;
                }

                if (table.cellPadding == '')
                    document.getElementById(editorid + '_HTB_TE_cellPadding').value = '0';
                else
                    document.getElementById(editorid + '_HTB_TE_cellPadding').value = table.cellPadding;
                if (table.cellSpacing == '')
                    document.getElementById(editorid + '_HTB_TE_cellSpacing').value = '0';
                else
                    document.getElementById(editorid + '_HTB_TE_cellSpacing').value = table.cellSpacing;

                if (table.style.borderCollapse.toUpperCase() == 'COLLAPSE')
                    document.getElementById(editorid + '_HTB_TE_collapseTableBorder').checked = true;
                else
                    document.getElementById(editorid + '_HTB_TE_collapseTableBorder').checked = false;

                if (table.border == '')
                    document.getElementById(editorid + '_HTB_TE_borderSize').value = '0';
                else
                    document.getElementById(editorid + '_HTB_TE_borderSize').value = table.border;

                if (table.borderColor == '')
                    document.getElementById(editorid + '_HTB_TE_borderColor').selectedIndex = 0;
                else
                    HTB_SelectColoredDropDownFromValue(document.getElementById(editorid + '_HTB_TE_borderColor'), table.borderColor);

                if (table.borderColorLight == '')
                    document.getElementById(editorid + '_HTB_TE_lightBorderColor').selectedIndex = 0;
                else
                    HTB_SelectColoredDropDownFromValue(document.getElementById(editorid + '_HTB_TE_lightBorderColor'), table.borderColorLight);

                if (table.borderColorDark == '')
                    document.getElementById(editorid + '_HTB_TE_darkBorderColor').selectedIndex = 0;
                else
                    HTB_SelectColoredDropDownFromValue(document.getElementById(editorid + '_HTB_TE_darkBorderColor'), table.borderColorDark);

                if (table.bgColor == '')
                    document.getElementById(editorid + '_HTB_TE_bgColor').selectedIndex = 0;
                else
                    HTB_SelectColoredDropDownFromValue(document.getElementById(editorid + '_HTB_TE_bgColor'), table.bgColor);

                if (table.background != '') {
                    document.getElementById(editorid + '_HTB_TE_useBackgroundPicture').checked = true;
                    document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = false;
                    document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').value = table.background;
                }

                else {
                    document.getElementById(editorid + '_HTB_TE_useBackgroundPicture').checked = false;
                    document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = true;
                    document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').value = '';
                }
            }

            else {
                document.getElementById(editorid + '_HTB_TE_tbRows').value = '3';
                document.getElementById(editorid + '_HTB_TE_tbRows').disabled = false;
                document.getElementById(editorid + '_HTB_TE_tbCols').value = '3';
                document.getElementById(editorid + '_HTB_TE_tbCols').disabled = false;
                document.getElementById(editorid + '_HTB_TE_alignment').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_float').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_specifyValueWidth').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyValueWidth').value = '0';
                document.getElementById(editorid + '_HTB_TE_specifyWidth').checked = false;
                document.getElementById(editorid + '_HTB_TE_specifyInPercentWidth').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').checked = true;
                document.getElementById(editorid + '_HTB_TE_specifyValueHeight').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyValueHeight').value = '0';
                document.getElementById(editorid + '_HTB_TE_specifyHeight').checked = false;
                document.getElementById(editorid + '_HTB_TE_specifyInPercentHeight').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').disabled = true;
                document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').checked = true;
                document.getElementById(editorid + '_HTB_TE_cellPadding').value = '2';
                document.getElementById(editorid + '_HTB_TE_cellSpacing').value = '1';
                document.getElementById(editorid + '_HTB_TE_borderSize').value = '0';
                document.getElementById(editorid + '_HTB_TE_borderColor').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_lightBorderColor').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_darkBorderColor').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_collapseTableBorder').checked = false;
                document.getElementById(editorid + '_HTB_TE_bgColor').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_useBackgroundPicture').checked = false;
                document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = true;
                document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').value = '';
                document.getElementById(editorid + '_HTB_TE_widthCell').value = '';
                document.getElementById(editorid + '_HTB_TE_heightCell').value = '';
                document.getElementById(editorid + '_HTB_TE_horizAlignment').selectedIndex = 0;
                document.getElementById(editorid + '_HTB_TE_vertAlignment').selectedIndex = 0;
            }
        }
        else {
            document.getElementById(editorid + '_HTB_TE_tbRows').value = '3';
            document.getElementById(editorid + '_HTB_TE_tbRows').disabled = false;
            document.getElementById(editorid + '_HTB_TE_tbCols').value = '3';
            document.getElementById(editorid + '_HTB_TE_tbCols').disabled = false;
            document.getElementById(editorid + '_HTB_TE_alignment').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_float').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_specifyValueWidth').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyValueWidth').value = '0';
            document.getElementById(editorid + '_HTB_TE_specifyWidth').checked = false;
            document.getElementById(editorid + '_HTB_TE_specifyInPercentWidth').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyInPixelsWidth').checked = true;
            document.getElementById(editorid + '_HTB_TE_specifyValueHeight').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyValueHeight').value = '0';
            document.getElementById(editorid + '_HTB_TE_specifyHeight').checked = false;
            document.getElementById(editorid + '_HTB_TE_specifyInPercentHeight').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').disabled = true;
            document.getElementById(editorid + '_HTB_TE_specifyInPixelsHeight').checked = true;
            document.getElementById(editorid + '_HTB_TE_cellPadding').value = '2';
            document.getElementById(editorid + '_HTB_TE_cellSpacing').value = '1';
            document.getElementById(editorid + '_HTB_TE_borderSize').value = '0';
            document.getElementById(editorid + '_HTB_TE_borderColor').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_lightBorderColor').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_darkBorderColor').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_collapseTableBorder').checked = false;
            document.getElementById(editorid + '_HTB_TE_bgColor').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_useBackgroundPicture').checked = false;
            document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = true;
            document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').value = '';
            document.getElementById(editorid + '_HTB_TE_widthCell').value = '';
            document.getElementById(editorid + '_HTB_TE_heightCell').value = '';
            document.getElementById(editorid + '_HTB_TE_horizAlignment').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_TE_vertAlignment').selectedIndex = 0;
        }
    }
    catch (e) { }
}

function HTB_CreateImage(editorid) {
    var image = '';

    image += '<img';

    if (document.getElementById('HTB_IE_picture') != null && document.getElementById('HTB_IE_picture').value != '')
        image += ' src=\'' + document.getElementById('HTB_IE_picture').value + '\'';

    if (document.getElementById('HTB_IE_text') != null)
        image += ' alt=\'' + document.getElementById('HTB_IE_text').value + '\'';

    if (document.getElementById('HTB_IE_borderThickness') != null && document.getElementById('HTB_IE_borderThickness').value != '')
        image += ' border=\'' + document.getElementById('HTB_IE_borderThickness').value + '\'';

    if (document.getElementById('HTB_IE_horizontalSpacing') != null && document.getElementById('HTB_IE_horizontalSpacing').value != '')
        image += ' hspace=\'' + document.getElementById('HTB_IE_horizontalSpacing').value + '\'';

    if (document.getElementById('HTB_IE_verticalSpacing') != null && document.getElementById('HTB_IE_verticalSpacing').value != '')
        image += ' vspace=\'' + document.getElementById('HTB_IE_verticalSpacing').value + '\'';

    if (document.getElementById('HTB_IE_cssClass') != null && document.getElementById('HTB_IE_cssClass').value != '')
        image += ' class=\'' + document.getElementById('HTB_IE_cssClass').value + '\'';

    var align = HTB_GetSelectedOptionValue('HTB_IE_alignment');
    if (align != '' && align != 'default')
        image += ' align=\'' + align + '\'';

    var h = '', w = '';

    if (document.getElementById('HTB_IE_specifySize').checked) {
        w = document.getElementById('HTB_IE_specifyValueWidth').value;
        if (w != '') {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(HTB_IE_gWidth);
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName('HTB_IE_gWidth'));

            if (selectedValue == 'Pixels') {
                w += 'px';
            }

            else {
                w += '%';
            }

            image += ' width=\'' + w + '\'';
        }

        h = document.getElementById('HTB_IE_specifyValueHeight').value;
        if (h != '') {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(HTB_IE_gHeight);
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName('HTB_IE_gHeight'));

            if (selectedValue == 'Pixels') {
                h += 'px';
            }

            else {
                h += '%';
            }

            image += ' height=\'' + h + '\'';
        }
    }

    image += ' id=\'HTB_tempIdImage\'';
    image += '>';
    HTB_SetSnippet(editorid, image);

    var imageObject =
	HTB_GetEditor(editorid).document.getElementById('HTB_tempIdImage');
    imageObject.src = document.getElementById('HTB_IE_picture').value;
    imageObject.removeAttribute('id');
}


function HTB_SetImageAspectRatio() {
    if (HTB_IE_keepAspectRatio.checked == true) {
        var w = 1;
        if (HTB_IE_specifyValueWidth.value != '') {
            w = parseInt(HTB_IE_specifyValueWidth.value);
            if (w == 0) w = 1;
        }
        else
            w = 1;

        var h = 1;
        if (HTB_IE_specifyValueHeight.value != '') {
            h = parseInt(HTB_IE_specifyValueHeight.value);
            if (h == 0) h = 1;
        }
        else
            h = 1;

        HTB_IE_aspectRatio = h / w;
    }
}

function HTB_KeepImageAspectRatio(e) {
    if (HTB_IE_keepAspectRatio.checked == true && HTB_IE_keepAspectRatio.disabled == false) {
        if (e.id.indexOf('Width') >= 0) {
            var w = 0;
            if (e.value != '')
                w = parseInt(e.value);

            HTB_IE_specifyValueHeight.value = Math.round(w * HTB_IE_aspectRatio);
        }

        else {
            var h = 0;
            if (e.value != '')
                h = parseInt(e.value);

            HTB_IE_specifyValueWidth.value = Math.round(h / HTB_IE_aspectRatio);
        }
    }
}

function HTB_InitImageEditor(editorid) {
    try {
        if (HTB_ie5) {
            state = eval(editorid + '_State');
            sel = state.GetSelection(editorid);
            if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'IMG') {
                var image = sel.item(0);

                HTB_IE_picture.value = image.src;
                HTB_IE_text.value = image.alt;
                HTB_IE_borderThickness.value = image.border;
                HTB_IE_horizontalSpacing.value = image.hspace;
                HTB_IE_verticalSpacing.value = image.vspace;

                if (image.align != null && image.align != '') {
                    var index = HTB_GetIndexOptionFromValue('HTB_IE_alignment', image.align)
                    if (index != -1)
                        HTB_IE_alignment.selectedIndex = index;
                }
                else
                    HTB_IE_alignment.selectedIndex = 0;

                var w = HTB_GetFromOuterHTML(image.outerHTML, 'width');
                var h = HTB_GetFromOuterHTML(image.outerHTML, 'height');

                if (w != '' || h != '') {
                    HTB_IE_specifyInPercentWidth.disabled = false;
                    HTB_IE_specifyInPixelsWidth.disabled = false;
                    HTB_IE_specifyValueWidth.disabled = false;

                    HTB_IE_specifyInPercentHeight.disabled = false;
                    HTB_IE_specifyInPixelsHeight.disabled = false;
                    HTB_IE_specifyValueHeight.disabled = false;

                    HTB_IE_specifySize.disabled = false;
                    HTB_IE_specifySize.checked = true;

                    HTB_IE_keepAspectRatio.disabled = false;
                }

                if (w != '') {
                    if (w.indexOf('%') >= 0) {
                        HTB_IE_specifyInPercentWidth.checked = true;
                    }
                    else {
                        HTB_IE_specifyInPixelsWidth.checked = true;
                    }
                    HTB_IE_specifyValueWidth.value = parseInt(w);
                }

                if (h != '') {
                    if (h.indexOf('%') >= 0) {
                        HTB_IE_specifyInPercentHeight.checked = true;
                    }
                    else {
                        HTB_IE_specifyInPixelsHeight.checked = true;
                    }
                    HTB_IE_specifyValueHeight.value = parseInt(h);
                }
                HTB_IE_VerifyAspectRationCanBeUsed();
                HTB_SetImageAspectRatio();
                HTB_IE_cssClass.value = image.className;

            }

            else {

                HTB_IE_picture.value = "";
                HTB_IE_text.value = "";

                HTB_IE_alignment.selectedIndex = 0;
                HTB_IE_borderThickness.value = "";
                HTB_IE_horizontalSpacing.value = "0";
                HTB_IE_verticalSpacing.value = "0";

                HTB_IE_specifySize.checked = false;
                HTB_IE_specifyValueWidth.disabled = true;
                HTB_IE_specifyValueWidth.value = '0';
                HTB_IE_specifyInPercentWidth.disabled = true;
                HTB_IE_specifyInPixelsWidth.disabled = true;
                HTB_IE_specifyInPixelsWidth.checked = true;
                HTB_IE_specifyValueHeight.disabled = true;
                HTB_IE_specifyValueHeight.value = '0';
                HTB_IE_specifyInPercentHeight.disabled = true;
                HTB_IE_specifyInPixelsHeight.disabled = true;
                HTB_IE_specifyInPixelsHeight.checked = true;
                HTB_IE_keepAspectRatio.checked = true;
                HTB_IE_keepAspectRatio.disabled = true;

                HTB_IE_cssClass.value = "";
            }
        }
        else {
            document.getElementById('HTB_IE_picture').value = "";
            document.getElementById('HTB_IE_text').value = "";

            document.getElementById('HTB_IE_alignment').selectedIndex = 0;
            document.getElementById('HTB_IE_borderThickness').value = "";
            document.getElementById('HTB_IE_horizontalSpacing').value = "0";
            document.getElementById('HTB_IE_verticalSpacing').value = "0";

            document.getElementById('HTB_IE_specifySize').checked = false;
            document.getElementById('HTB_IE_specifyValueWidth').disabled = true;
            document.getElementById('HTB_IE_specifyValueWidth').value = '0';
            document.getElementById('HTB_IE_specifyInPercentWidth').disabled = true;
            document.getElementById('HTB_IE_specifyInPixelsWidth').disabled = true;
            document.getElementById('HTB_IE_specifyInPixelsWidth').checked = true;
            document.getElementById('HTB_IE_specifyValueHeight').disabled = true;
            document.getElementById('HTB_IE_specifyValueHeight').value = '0';
            document.getElementById('HTB_IE_specifyInPercentHeight').disabled = true;
            document.getElementById('HTB_IE_specifyInPixelsHeight').disabled = true;
            document.getElementById('HTB_IE_specifyInPixelsHeight').checked = true;
            document.getElementById('HTB_IE_keepAspectRatio').checked = true;
            document.getElementById('HTB_IE_keepAspectRatio').disabled = true;

            document.getElementById('HTB_IE_cssClass').value = "";
        }
    }
    catch (e) { }
}

function HTB_InsertTable(id, cols, rows, height, width, border, bordercolor, bordercolorlight, bordercolordark, cellspacing, cellpadding, floatalign, align, background, bgcolor, bordercollapse, preview, cellWidth, cellHeight, hCellAlign, vCellAlign) {
    var table = '';
    var styleDef = 'BORDER-RIGHT: silver 1px dotted; BORDER-LEFT: silver 1px dotted; BORDER-TOP: silver 1px dotted; BORDER-BOTTOM: silver 1px dotted;';

    if (floatalign != '') {
        table += '<div align=' + floatalign + '>';
    }

    table += '<table';

    if (height != null && height != '')
        table += ' height=' + height;

    if (width != null && width != '')
        table += ' width=' + width;

    if (border != null && border != '')
        table += ' border=' + border;

    if (bordercolor != null && bordercolor != '')
        table += ' borderColor=' + bordercolor;

    if (bgcolor != null && bgcolor != '')
        table += ' bgcolor=' + bgcolor;

    if (background != null && background != '')
        table += ' background=' + background;

    if (bordercolorlight != null && bordercolorlight != '')
        table += ' borderColorLight=' + bordercolorlight;

    if (bordercolordark != null && bordercolordark != '')
        table += ' borderColorDark=' + bordercolordark

    table += ' style="';

    table += 'BORDER-COLLAPSE:';
    if (bordercollapse) {
        table += 'collapse';
    }
    else
        table += 'separate';

    table += ';';

    if (preview == 1 && border == 0)
        table += styleDef;
    //table += 'class="HTB_table"';

    table += '"';

    if (cellspacing)
        table += ' cellspacing=' + cellspacing;
    if (cellpadding)
        table += ' cellpadding=' + cellpadding;
    if (align)
        table += ' align=' + align;

    table += '>';

    for (index = 0; index < rows; index++) {
        table += '<tr>';
        for (index2 = 0; index2 < cols; index2++) {
            table += '<td';
            if (cellWidth != null && cellWidth != '' && cellWidth != '0')
                table += ' width=' + cellWidth;

            if (cellHeight != null && cellHeight != '' && cellHeight != '0')
                table += ' height=' + cellHeight;

            if (hCellAlign != '')
                table += ' align=' + hCellAlign;

            if (vCellAlign != '')
                table += ' valign=' + vCellAlign;

            if (preview == 1 && border == 0)
                table += ' style="' + styleDef + '"';
            //table += 'class="HTB_tableTD"';
            table += '></td>';
        }
        table += '</tr>';
    }

    table += '</table>';

    if (floatalign != '')
        table += '</div>';

    HTB_SetSnippet(id, table);
}

function HTB_StringReplace(str1, str2, str3) {
    str1 = str1.split(str2).join(str3);
    return str1;
}

function HTB_SetValue(obj, value) {
    obj.value = value;
}

function HTB_SetSelectedColor(id, color) {
    var s = '<table style=\"background-color:' + color + ';\" width=\"12\" height=\"12\"><tr><td></td></tr></table>';
    document.getElementById(id + '_selectedColor').value = color;
    //ATB_setDropDownListText(id,s);
    ATB_closeDropDownList(id);
}

function HTB_GetSelectedColor(id) {
    return document.getElementById(id + '_selectedColor').value;
}

function HTB_State(id) {
    this.selection = null
    this.Id = id
    this.RestoreSelection = HTB_State_RestoreSelection
    this.GetSelection = HTB_State_GetSelection
    this.SaveSelection = HTB_State_SaveSelection
}

function HTB_State_RestoreSelection() {
    if (this.selection) this.selection.select();
}

function HTB_State_GetSelection(id) {
    var sel = null;

    if (HTB_ie5) {
        sel = this.selection;
        if (!sel) {
            sel = HTB_GetEditor(id).document.selection.createRange();
            sel.type = HTB_GetEditor(id).document.selection.type;
        }
    }
    else {
        sel = HTB_GetEditor(id).window.getSelection();
    }
    return sel;
}

function HTB_State_SaveSelection(id) {
    state = eval(id + '_State');
    /*alert(HTB_GetEditor(id).document.selection.createRange().text);
    alert(HTB_GetEditor(id).document.selection.text);*/
    state.selection = HTB_GetEditor(id).document.selection.createRange();
    state.selection.type = HTB_GetEditor(id).document.selection.type;
}

function HTB_RestoreSelection(id) {
    state = eval(id + '_State');
    state.RestoreSelection();
}

function HTB_SetColorEditor(id, type, color) {
    if (type == 'f')
        cmd = 'forecolor';
    else
        cmd = 'backcolor';

    HTB_CommandBuilder(id, cmd, color);
}

function HTB_CreateSpecialCharsTable(editorid, id) {
    /*var item = document.getElementById(id + "_item0");
    var str = '', td;
    cols = 12;
    charNames = document.getElementById(id + '_specialCharsNames').value.split(',');
    charCodes = document.getElementById(id + '_specialCharsCodes').value.split(',');		
    str += '<table border=\'0\' cellspacing=\'2\' width=\'100%\' height=\'100%\' cellpadding=\'2\' onselectstart=\'return false\'><tr><td>';

	for(var i=0;i<charNames.length;i++)
    {
    td = '<td style=\'cursor:hand\' ' + ' width=\'' + parseInt(100 / cols) + '\' onclick="" onmouseover="HTB_OnColorOver(this);" oumouseout="HTB_OnColorOff(this);">' + charNames[i] + '</td>';
    if ((i) % cols == 0 || i == 0)
    str += '<tr>' + td;
    else if ((i + 1) % cols == 0)
    str += td + '</tr>';
    else
    str += td;
    }

	str += '</td></tr></table>'; 
    item.innerHTML = str;*/

    var item = document.getElementById(id + "_item0");
    var str = '', td;
    cols = 10;
    charNames = document.getElementById(id + '_specialCharsNames').value.split(',');
    charCodes = document.getElementById(id + '_specialCharsCodes').value.split(',');
    str += '<table class=\'HTB_clsBackColor\'><tr><td><table width=\'100%\' cellpadding=\'0\' cellspacing=\'0\'>';
    for (var i = 0; i < charNames.length; i++) {
        var charName = charNames[i].substring(1, charNames[i].length - 1);
        var charCode = charCodes[i].substring(1, charCodes[i].length - 1);
        td = '<td><table class=\'HTB_clsColorCont\' width=\'30\' height=\'35\' cellpadding=\'0\' cellspacing=\'0\' onclick="HTB_SetSnippet(\'' + editorid + '\', \'' + charCode + '\');HTB_OnColorOff(this);" onmouseover=\'HTB_OnColorOver(this);\' onmouseout=\'HTB_OnColorOff(this);\'><tr><td align=\'center\'><table class=\'HTB_clsDropDownItem\' style=\'background-color: white;\' width=\'22\' height=\'24\'><tr align=center valign=middle><td>' + charName + '</td></tr></table></td></tr></table></td>';
        if ((i) % cols == 0 || i == 0)
            str += '<tr>' + td;
        else if ((i + 1) % cols == 0)
            str += td + '</tr>';
        else
            str += td;
    }

    str += '</table></td></tr></table>';
    item.innerHTML = str;
}

/*function HTB_FindAndReplace(id,popupid,find,replace,caseSensitive, wholeWord, queryPrompt)
{
var count = 0;
	
if (find)
{
var args = HTB_GetArgs(caseSensitive, wholeWord); 
rng = HTB_GetEditor(id).document.body.createTextRange();
rng.moveToElementText(HTB_GetEditor(id).document.body);
HTB_ClearUndoBuffer();
for (var i = 0; rng.findText(find, 1000000, args); i++)
{
if (!queryPrompt)
{
rng.text = replace;
HTB_PushUndoNew(rng, find, replace);
count++;
}
else
{
rng.select();
rng.scrollIntoView();
if (confirm('Replace?'))
{
rng.text = replace;
HTB_PushUndoNew(rng, find, replace);
count++;
}
}
rng.collapse(false);
}
}

ATB_hidePopup(popupid);	

alert(count + ' occurence(s) replaced.');
HTB_SetFocus(id);
}*/

function HTB_ReplaceAll(editorid, find, replace, caseSensitive, wholeWord) {
    var count = 0;

    if (find) {
        var args = HTB_GetArgs(caseSensitive, wholeWord);
        rng = HTB_GetEditor(editorid).document.body.createTextRange();
        rng.moveToElementText(HTB_GetEditor(editorid).document.body);
        HTB_ClearUndoBuffer();
        for (var i = 0; rng.findText(find, 1000000, args); i++) {

            rng.text = replace;
            HTB_PushUndoNew(rng, find, replace);
            count++;
            rng.collapse(false);
        }
    }

    //alert(count + ' occurence(s) replaced.');
    alert(count + ' ' + HTB_GetTextLabel(editorid, 44));

}


function HTB_InitFind(editorid) {
    try {
        HTB_F_textRange = null;
        HTB_F_count = 0;
        if (HTB_ie5)
            document.getElementById(editorid + '_HTB_F_find').value = '';
        else
            document.getElementById(editorid + '_HTB_F_find').value = '';
        if (HTB_ie5)
            HTB_F_caseSensitive.checked = false;
        else {
            document.getElementById(editorid + '_HTB_F_caseSensitive').checked = false;
            document.getElementById(editorid + '_HTB_F_caseSensitive').disabled = true;
        }
        if (HTB_ie5)
            HTB_F_wholeWord.checked = false;
        else {
            document.getElementById(editorid + '_HTB_F_wholeWord').checked = false;
            document.getElementById(editorid + '_HTB_F_wholeWord').disabled = true;
        }
        HTB_F_CheckFind();
    }
    catch (e) { }
}

function HTB_InitFindAndReplace(editorid) {
    try {
        HTB_F_textRange = null;
        HTB_F_count = 0;
        HTB_F_textToFind = '';

        document.getElementById(editorid + '_HTB_FR_find').value = '';
        document.getElementById(editorid + '_HTB_FR_replace').value = '';
        document.getElementById(editorid + '_HTB_FR_caseSensitive').checked = false;
        document.getElementById(editorid + '_HTB_FR_wholeWord').checked = false;
        HTB_FR_CheckFindAndReplace(editorid);
    }
    catch (e) { }
}

function HTB_Find(editorid, textToFind, caseSensitive, wholeWord) {
    if (textToFind) {
        var strFound = 0;

        var args = HTB_GetArgs(caseSensitive, wholeWord);

        if (HTB_ie5 && HTB_F_textRange != null) {
            HTB_F_textRange.collapse(false);
            strFound = HTB_F_textRange.findText(textToFind, 1000000, args);
            if (strFound) {
                HTB_F_textRange.select();
                HTB_F_count++;
            }
        }

        if (HTB_ie5 && HTB_F_textRange == null && strFound == 0) {
            HTB_F_textRange = HTB_GetEditor(editorid).document.body.createTextRange();
            HTB_F_textRange.moveToElementText(HTB_GetEditor(editorid).document.body);
            strFound = HTB_F_textRange.findText(textToFind, 1000000, args);
            if (strFound) {
                HTB_F_textRange.select();
                HTB_F_count++;
            }
        }

        else if (HTB_ns6) {
            if (HTB_F_count == 0)
                HTB_ResetCaret(editorid);
            strFound = HTB_GetEditor(editorid).find(textToFind, false, false);

            if (strFound)
                HTB_F_count++;
        }

        if (!strFound && HTB_F_count == 0) {
            HTB_F_textRange = null;
            HTB_F_count = 0;
            alert(HTB_GetTextLabel(editorid, 37).replace('$FIND$', textToFind));
        }
        else if (!strFound && HTB_F_count != 0) {
            alert(HTB_GetTextLabel(editorid, 38));
            HTB_F_textRange = null;
            HTB_F_count = 0;
        }
    }
}

function HTB_Replace(id, find, replace, caseSensitive, wholeWord) {
    if (HTB_F_textRange == null) {
        HTB_Find(id, find, caseSensitive, wholeWord);
    }

    HTB_F_textRange.text = replace;
    HTB_PushUndoNew(HTB_F_textRange, find, replace);
    HTB_Find(id, find, caseSensitive, wholeWord);
}

function HTB_GetArgs(caseSensitive, wholeWord) {
    var isCaseSensitive = (caseSensitive) ? 4 : 0;
    var isWholeWord = (wholeWord) ? 2 : 0;
    return isCaseSensitive ^ isWholeWord;
}

var undoObject = { origSearchString: "", newRanges: [] };

function HTB_PushUndoNew(rng, findString, replaceString) {
    undoObject.origSearchString = findString;
    rng.moveStart("character", -replaceString.length);
    undoObject.newRanges[undoObject.newRanges.length] = rng.getBookmark();
}

function HTB_ClearUndoBuffer() {
    undoObject.origSearchString = "";
    undoObject.newRanges.length = 0;
}

function HTB_UndoReplace() {
    if (undoObject.newRanges.length && undoObject.origSearchString) {
        for (var i = 0; i < underObject.newRanges.length; i++) {
            rng.moveToBookmark(undoObject.newRanges[i]);
            rng.text = undoObject.origSearchString;
        }
        HTB_ClearUndoBuffer();
    }
}

function HTB_CreateTableQuick(id, cols, rows) {
    HTB_CreateTableFull(id, cols, rows);
}

function HTB_CreateTableFull(id, cols, rows, backColor) {
    /*state = eval(id + '_State');
    sel = state.GetSelection(id);
    if (sel.type == 'Control')
    {
    alert(sel.item(0).outerHTML);
    alert(sel.item(0).tagName);
    }*/

    HTB_InsertTable(id, cols, rows, '', '', '0', '', '', '', '1', '2', '', '', '', '', false, true)
}
function HTB_TableBuilderOver(obj, col, row) {
    //HTB_TableBuilderClear();
    var colindex, rowindex;
    for (colindex = 0; colindex <= col; colindex++) {
        for (rowindex = 0; rowindex <= row; rowindex++) {
            HTB_SetBackColor(document.getElementById('cell' + colindex + rowindex), '#0A246A');
            document.getElementById('tableInfo').innerText = (rowindex + 1) + ' by ' + (colindex + 1) + ' Table';
        }
    }
}
function HTB_TableBuilderClear() {

    var colindex, rowindex;
    for (colindex = 0; colindex < 5; colindex++) {
        for (rowindex = 0; rowindex < 4; rowindex++) {
            HTB_SetBackColor(document.getElementById('cell' + colindex + rowindex), '');
            //document.getElementById('tableInfo').innerText = 'Cancel';
        }
    }
}

function HTB_OpenTableEditor(popupid, editorid) {
    HTB_InitTableEditor(editorid);
    ATB_showPopup(popupid);
}

function HTB_SetSnippet(id, code) {
    if (HTB_ie5) {
        HTB_SetFocus(id);
        var state = eval(id + '_State');
        var editable = eval(id + '_State');

        state.RestoreSelection();
        var selection = HTB_GetEditor(id).document.selection.createRange();
        selection.type = HTB_GetEditor(id).document.selection.type;

        if (selection.type == 'Control') {
            selection.item(0).outerHTML = code;
        }
        else {
            HTB_SetFocus(id);
            selection.pasteHTML(code);
        }

        /*var selection = HTB_GetEditor(id).window.getSelection();
        selection.pasteHTML(code);*/
    }
    else {

        var editor = HTB_GetEditor(id);
        editor.focus();
        selection = editor.window.getSelection();
        if (selection) {
            range = selection.getRangeAt(0);
        }
        else {
            range = editor.document.createRange();
        }

        var fragment = editor.document.createDocumentFragment();
        var div = editor.document.createElement("div");
        div.innerHTML = code;

        while (div.firstChild) {
            fragment.appendChild(div.firstChild);
        }

        selection.removeAllRanges();
        range.deleteContents();

        var node = range.startContainer;
        var pos = range.startOffset;

        switch (node.nodeType) {
            case 1:
                {
                    node = node.childNodes[pos];
                    node.parentNode.insertBefore(fragment, node);
                    range.setEnd(node, pos + fragment.length);
                    range.setStart(node, pos + fragment.length);
                }
                break;

            case 3:
                {
                    if (fragment.nodeType == 3) {
                        node.insertData(pos, fragment.data);
                        range.setEnd(node, pos + fragment.length);
                        range.setStart(node, pos + fragment.length);
                    }
                    else {
                        node = node.splitText(pos);
                        node.parentNode.insertBefore(fragment, node);
                        range.setEnd(node, pos + fragment.length);
                        range.setStart(node, pos + fragment.length);
                    }
                }
                break;

        }

        selection.addRange(range);
    }

    //	HTB_UpdateLength(id);
}

function HTB_CleanCode(id) {
    editor = HTB_GetEditor(id);

    // 0bject based cleaning
    var body = editor.document.body;
    for (var index = 0; index < body.all.length; index++) {
        tag = body.all[index];
        tag.removeAttribute("className", "", 0);
        tag.removeAttribute("style", "", 0);
        tag.removeAttribute("lang", "", 0);
    }

    // Regex based cleaning
    var html = editor.document.body.innerHTML;
    html = html.replace(/<o:p>&nbsp;<\/o:p>/g, "");
    html = html.replace(/&nbsp;<o:p><\/o:p>/g, "");
    html = html.replace(/<o:p><\/o:p>/g, "");
    html = html.replace(/o:/g, "");
    html = html.replace(/<st1:.*?>/g, "");
    html = HTB_ReplaceAllOccurence(html, "<SPAN>", "");
    html = HTB_ReplaceAllOccurence(html, "</SPAN>", "");
    editor.document.body.innerHTML = html;

    HTB_SetFocus(id);
}

function HTB_EnableDisableSpecify(prefix, name) {
    if (document.getElementById(prefix + '_specify' + name).checked) {
        document.getElementById(prefix + '_specifyValue' + name).disabled = false;
        document.getElementById(prefix + '_specifyInPixels' + name).disabled = false;
        document.getElementById(prefix + '_specifyInPercent' + name).disabled = false;
    }

    else {
        document.getElementById(prefix + '_specifyValue' + name).disabled = true;
        document.getElementById(prefix + '_specifyInPixels' + name).disabled = true;
        document.getElementById(prefix + '_specifyInPercent' + name).disabled = true;
    }
}

function HTB_EnableDisableBackgroundImage(editorid) {
    if (document.getElementById(editorid + '_HTB_TE_useBackgroundPicture').checked) {
        document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = false;
    }

    else {
        document.getElementById(editorid + '_HTB_TE_backgroundPictureValue').disabled = true;
    }
}

function HTB_ComposeOption(value, label, style, toCompare) {
    return '<option value="' + value + '"' + (style != null ? ' style="' + style + '"' : '') + '>' + label + '</option>';
}

function HTB_CreateColoredDropDown(editorid, id) {
    var colors = new Array(HTB_GetTextLabel(editorid, 124), HTB_GetTextLabel(editorid, 125), HTB_GetTextLabel(editorid, 126), HTB_GetTextLabel(editorid, 127), HTB_GetTextLabel(editorid, 128), HTB_GetTextLabel(editorid, 129), HTB_GetTextLabel(editorid, 130), HTB_GetTextLabel(editorid, 131), HTB_GetTextLabel(editorid, 132), HTB_GetTextLabel(editorid, 133), HTB_GetTextLabel(editorid, 134), HTB_GetTextLabel(editorid, 135), HTB_GetTextLabel(editorid, 136), HTB_GetTextLabel(editorid, 137), HTB_GetTextLabel(editorid, 138));
    var colorsHexa = new Array("#000000", "#800000", "#808000", "#008000", "#008080", "#000080", "#800080", "#ffffff", "#c0c0c0", "#ff0000", "#ffff00", "#00ff00", "#00ffff", "#0000ff", "#ff00ff");
    var str = '';

    str += '<select id=' + id + '>';
    str += '<option value=\"\">---</option>';

    for (index = 0; index < colors.length; index++) {
        forecolor = ';';
        if (colors[index] == 'Black')
            forecolor += 'color:White;';
        str += HTB_ComposeOption(colorsHexa[index], colors[index], 'background-color:' + colors[index] + forecolor, '');
    }

    str += '</select>';
    str += '<br>';

    return str;
}

function HTB_GetSelectedColoredDropDown(id) {
    var ddl = document.getElementById(id);
    if (ddl.selectedIndex > 0)
        return ddl.options[ddl.selectedIndex].value;
    else
        return '';
}

function HTB_GetSelectedOptionText(id) {
    var ddl = document.getElementById(id);
    return ddl.options[ddl.selectedIndex].text;
}

function HTB_GetSelectedOptionValue(id) {
    var ddl = document.getElementById(id);
    return ddl.options[ddl.selectedIndex].value;
}

function HTB_GeOptionIndexFromValue(ddl, val) {
    for (i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].value == val)
            return i;
    }

    return -1;
}

function HTB_SelectOptionFromValue(ddl, val) {
    index = HTB_GeOptionIndexFromValue(ddl, val);

    if (index != -1)
        ddl.selectedIndex = index;
    else
        ddl.selectedIndex = 0;
}

function HTB_GetOptionIndexFromText(ddl, text) {
    for (i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].text == text)
            return i;
    }

    return -1;
}

function HTB_SelectOptionFromText(ddl, text) {
    index = HTB_GetOptionIndexFromText(ddl, text);
    if (index != -1)
        ddl.selectedIndex = index;
    else
        ddl.selectedIndex = 0;
}

function HTB_GetColoredDropDownIndexFromValue(ddl, val) {
    var colors = new Array("Black", "Maroon", "Olive", "Green", "Teal", "Navy", "Purple", "White", "Silver", "Red", "Yellow", "Lime", "Aqua", "Blue", "Fuchsia");

    for (i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].value == val)
            return i;
    }

    return -1;
}

function HTB_SelectColoredDropDownFromValue(ddl, val) {
    index = HTB_GetColoredDropDownIndexFromValue(ddl, val);

    if (index != -1)
        ddl.selectedIndex = index;
    else
        ddl.selectedIndex = 0;

}

function HTB_IE_VerifyAspectRationCanBeUsed() {
    if (((HTB_IE_specifyInPercentWidth.checked && HTB_IE_specifyInPercentHeight.checked) ||
		(HTB_IE_specifyInPixelsWidth.checked && HTB_IE_specifyInPixelsHeight.checked)) &&
		HTB_IE_specifySize.checked == true)
        HTB_IE_keepAspectRatio.disabled = false;
    else
        HTB_IE_keepAspectRatio.disabled = true;
}

function HTB_IE_EnableDisableSpecifySize() {
    if (document.getElementById('HTB_IE_specifySize').checked == true) {
        document.getElementById('HTB_IE_specifyValueWidth').disabled = false;
        document.getElementById('HTB_IE_specifyInPercentWidth').disabled = false;
        document.getElementById('HTB_IE_specifyInPixelsWidth').disabled = false;

        document.getElementById('HTB_IE_specifyValueHeight').disabled = false;
        document.getElementById('HTB_IE_specifyInPercentHeight').disabled = false;
        document.getElementById('HTB_IE_specifyInPixelsHeight').disabled = false;

        document.getElementById('HTB_IE_keepAspectRatio').disabled = false;
    }
    else {
        document.getElementById('HTB_IE_specifyValueWidth').disabled = true;
        document.getElementById('HTB_IE_specifyInPercentWidth').disabled = true;
        document.getElementById('HTB_IE_specifyInPixelsWidth').disabled = true;
        document.getElementById('HTB_IE_specifyInPixelsWidth').checked = true;

        document.getElementById('HTB_IE_specifyValueHeight').disabled = true;
        document.getElementById('HTB_IE_specifyInPercentHeight').disabled = true;
        document.getElementById('HTB_IE_specifyInPixelsHeight').disabled = true;
        document.getElementById('HTB_IE_specifyInPixelsHeight').checked = true;

        document.getElementById('HTB_IE_keepAspectRatio').disabled = true;
    }
}

function HTB_GetFromOuterHTML(outerHTML, toGet) {
    var saveOuterHTML = outerHTML;
    var outerHTML = outerHTML.toUpperCase();
    var index = outerHTML.indexOf(toGet.toUpperCase());
    var isFoundEqual = false;
    var startIndex = -1;
    var separator = 'N/A';

    if (index >= 0) {
        for (i = index + toGet.length; i < outerHTML.length; i++) {

            if (isFoundEqual == true) {
                if (startIndex == -1) {
                    if (outerHTML.charAt(i) != ' ') {
                        startIndex = i;
                        if (outerHTML.charAt(i) == '\"' || outerHTML.charAt(i) == '\'') {
                            separator = outerHTML.charAt(i);
                        }
                    }
                }
                else {
                    if (separator != 'N/A') {
                        if (outerHTML.charAt(i) == separator)
                            return saveOuterHTML.substring(startIndex + 1, i);
                    }

                    else {
                        if (outerHTML.charAt(i) == ' ')
                            return saveOuterHTML.substring(startIndex, i);
                    }
                }
            }

            else {
                if (outerHTML.charAt(i) == '=') {
                    isFoundEqual = true;
                }
            }

        }

        return '';
    }


    else return '';
}

function HTB_RemoveFromOuterHTML(outerHTML, toRemove) {
    var index = outerHTML.toUpperCase().indexOf(toRemove.toUpperCase());
    var isFoundEqual = false;
    var startIndex = -1, startIndexValue = -1;
    var separator = 'N/A';

    if (index >= 0) {
        startIndex = index;
        for (i = index + toRemove.length; i < outerHTML.length; i++) {
            if (isFoundEqual == true) {
                if (startIndexValue == -1) {
                    if (outerHTML.charAt(i) != ' ') {
                        startIndexValue = i;
                        if (outerHTML.charAt(i) == '\"' || outerHTML.charAt(i) == '\'') {
                            separator = outerHTML.charAt(i);
                        }
                    }
                }
                else {
                    if (separator != 'N/A') {
                        if (outerHTML.charAt(i) == separator) {

                            if (startIndex - 1 > 0 && outerHTML.charAt(startIndex - 1) == ' ')
                                startIndex--;

                            return outerHTML.substring(0, startIndex) + outerHTML.substring(i + 1);
                        }
                    }

                    else {
                        if (outerHTML.charAt(i) == ' ' || outerHTML.charAt(i) == '>') {
                            if (startIndex - 1 > 0 && outerHTML.charAt(startIndex - 1) == ' ')
                                startIndex--;

                            return outerHTML.substring(0, startIndex) + outerHTML.substring(i);
                        }

                    }
                }
            }

            else {
                if (outerHTML.charAt(i) == '=') {
                    isFoundEqual = true;
                }
            }

        }

        return outerHTML;
    }

    else return outerHTML;
}

function HTB_GetIndexOptionFromValue(id, value) {
    var ddl = document.getElementById(id);
    if (ddl != null) {
        for (i = 0; i < ddl.options.length; i++) {
            if (ddl.options[i].value == value)
                return i;
        }
    }

    return -1;
}

function HTB_SetValueInitLink(editorid, link) {
    document.getElementById(editorid + '_HTB_L_address').value = HTB_GetFromOuterHTML(link.outerHTML, 'href');
    if (link.innerHTML != '')
        document.getElementById(editorid + '_HTB_L_text').value = link.innerHTML;
    else
        document.getElementById(editorid + '_HTB_L_text').value = link.innerText;

    document.getElementById(editorid + '_HTB_L_anchor').value = link.name;
    if (document.getElementById(editorid + '_HTB_L_target').value != '') {
        HTB_SelectOptionFromText(document.getElementById(editorid + '_HTB_L_preselectedTarget'), link.target);
        document.getElementById(editorid + '_HTB_L_target').value = link.target;
    }
    else {
        document.getElementById(editorid + '_HTB_L_preselectedTarget').selectedIndex = 0;
        document.getElementById(editorid + '_HTB_L_target').value = '';
    }

    if (sel.title != null && sel.title != '')
        document.getElementById(editorid + '_HTB_L_tooltip').value = link.title;
    else
        document.getElementById(editorid + '_HTB_L_tooltip').value = '';

    if (sel.alt != null && link.alt != '')
        document.getElementById(editorid + '_HTB_L_altText').value = link.alt;
    else
        document.getElementById(editorid + '_HTB_L_altText').value = '';
}

function HTB_InitLink(editorid) {
    try {
        if (HTB_ie5) {
            document.getElementById(editorid + '_HTB_L_removeLink').disabled = false;

            state = eval(editorid + '_State');
            sel = state.GetSelection(editorid)
            var currentSel = sel;

            if (sel.type != 'Control') {
                sel = sel.parentNode;
                if (sel.parentNode && sel.parentNode.tagName.toUpperCase() == 'A')
                    sel = sel.parentNode;

                if (sel.tagName.toUpperCase() == 'A') {
                    HTB_SetValueInitLink(editorid, sel);
                }

                else {
                    HTB_ResetLinkValue(editorid);
                    if (currentSel.type == 'Text') {
                        if (currentSel != null)
                            document.getElementById(editorid + '_HTB_L_text').value = currentSel.text;
                    }
                }
            }

            else if (sel.item(0).parentNode.tagName.toUpperCase() == 'A') {
                HTB_SetValueInitLink(editorid, sel.item(0).parentNode);
            }

            else
                HTB_ResetLinkValue(editorid);
        }
        else {
            document.getElementById(editorid + '_HTB_L_removeLink').disabled = true;
            HTB_ResetLinkValue(editorid);
            var editor = HTB_GetEditor(editorid);
            selection = editor.window.getSelection();
            document.getElementById(editorid + '_HTB_L_text').value = selection;
        }
    }
    catch (err) { }

}

function HTB_ResetLinkValue(editorid) {
    document.getElementById(editorid + '_HTB_L_address').value = '';
    document.getElementById(editorid + '_HTB_L_text').value = '';
    document.getElementById(editorid + '_HTB_L_anchor').value = '';
    document.getElementById(editorid + '_HTB_L_preselectedTarget').selectedIndex = 0;
    document.getElementById(editorid + '_HTB_L_target').value = '';
    document.getElementById(editorid + '_HTB_L_tooltip').value = '';
    document.getElementById(editorid + '_HTB_L_altText').value = '';
}

function HTB_CreateLink(editorid, popupid) {
    if (document.getElementById(editorid + '_HTB_L_address').value == '') return;

    HTB_InsertLink(editorid, document.getElementById(editorid + '_HTB_L_address').value, document.getElementById(editorid + '_HTB_L_text').value, document.getElementById(editorid + '_HTB_L_anchor').value, document.getElementById(editorid + '_HTB_L_target').value, document.getElementById(editorid + '_HTB_L_tooltip').value, document.getElementById(editorid + '_HTB_L_altText').value);

    ATB_hidePopup(popupid);
}

function HTB_InsertLink(editorid, address, text, anchor, target, tooltip, alt) {
    var link = '';

    link += '<a';
    link += ' href=' + address;

    if (target != '')
        link += ' target=' + target;

    if (anchor != '')
        link += ' name=' + anchor;

    if (tooltip != '')
        link += ' title=' + tooltip;

    if (alt != '')
        link += ' alt=' + alt;
    else
        link += ' alt=\'\'';

    link += ' id=\'HTB_tempIdLink\''

    link += '>';
    if (text != '')
        link += text;
    else
        link += address;
    link += '</a>';

    editor = HTB_GetEditor(editorid);
    state = eval(editorid + '_State');
    state.RestoreSelection();
    editor = HTB_GetEditor(editorid);
    editor.focus();
    var sel = state.GetSelection(editorid);

    if (sel.type == null || sel.type == 'None') {
        HTB_SetSnippet(editorid, link);

        var linkObject = HTB_GetEditor(editorid).document.getElementById('HTB_tempIdLink');
        linkObject.href = address;
        linkObject.removeAttribute('id');

    }
    else {
        editor.document.execCommand('createlink', false, address);
        var newlink = state.GetSelection(editorid);

        if (newlink.type != 'Control')
            newlink = newlink.parentNode;

        try {
            if (anchor != '')
                newlink.name = anchor;
            else
                newlink.removeAttribute('name');

            if (tooltip != '')
                newlink.title = tooltip;
            else
                newlink.removeAttribute('tooltip');

            if (alt != '')
                newlink.alt = alt;
            else
                newlink.removeAttribute('alt');

            if (target != '')
                newlink.target = target;
            else
                newlink.removeAttribute('target');
        }

        catch (e) {
        }
    }
}

function HTB_InsertCustomLink(editorid, customlinkid, address, text, anchor, target, tooltip) {
    if (HTB_ie5) {
        state = eval(editorid + '_State');
        sel = state.GetSelection(editorid)
        var currentSel = sel;

        if (sel.type != 'Control' && sel.type != 'None') {
            sel = sel.parentNode;
            if (sel.parentNode && sel.parentNode.tagName.toUpperCase() == 'A')
                sel = sel.parentNode;

            if (sel.tagName.toUpperCase() == 'A') {
                if (sel.innerHTML != '')
                    text = sel.innerHTML;
                else
                    text = sel.innerText;
            }
            else {
                if (currentSel.type == 'Text') {
                    if (currentSel != null)
                        text = currentSel.text;
                }
            }
        }
    }
    else {
        var editor = HTB_GetEditor(editorid);
        selection = editor.window.getSelection();

        if (selection != null && selection != '')
            text = selection;

    }
    HTB_InsertLink(editorid, address, text, anchor, target, tooltip);
    ATB_closeDropDownList(customlinkid);
}

function HTB_L_ChangeTarget(editorid) {
    if (HTB_GetSelectedOptionValue(editorid + '_HTB_L_preselectedTarget') == 'reset') {
        document.getElementById(editorid + '_HTB_L_target').value = '';
        document.getElementById(editorid + '_HTB_L_preselectedTarget').selectedIndex = 0;
    }
    else {
        document.getElementById(editorid + '_HTB_L_target').value = HTB_GetSelectedOptionText(editorid + '_HTB_L_preselectedTarget');

    }
}

/*function HTB_IU_RemoveFilesContents()
{
var table = document.getElementById("HTB_IU_tableContents");
var tablebody = document.getElementById("HTB_UI_tableContentsBody");
if (tablebody != null)
table.removeChild(tablebody);
}*/

function HTB_IU_FillFilesContent(editorid, directory, postback, imageDir, deleteIcon, disdelete) {
    var div = document.getElementById(editorid + "_HTB_IU_tableContents");
    div.contentWindow.document.body.innerHTML = '';
    var contents = '<table id=\'' + editorid + '_HTB_IU_tableContents\' style=\'width:100%;border-collapse: collapse;\' borderColor=#808080 height=0 cellSpacing=1 cellPadding=1 border=1">';

    var serverPath = document.getElementById(editorid + '_HTB_IU_curServerPath').value;

    var filesHiddenInput = document.getElementById(editorid + '_HTB_IU_dir_' + directory);
    if (filesHiddenInput != null)
        document.getElementById(editorid + "_HTB_IU_curSelDirFullPath").value = filesHiddenInput.name;

    if (filesHiddenInput != null && filesHiddenInput.value != '') {
        //div.contentWindow.document.body.innerHTML = 'Loading in progress....';

        var reg = new RegExp('[;]+', 'g');
        var filesInfos = filesHiddenInput.value.split(reg);

        for (i = 0; i < filesInfos.length; i++) {
            if (filesInfos[i] != null && filesInfos[i].value != '') {
                reg = new RegExp('[|]+', 'g');
                fileInfos = filesInfos[i].split(reg);

                contents += "<tr style=\"cursor:hand;\" onclick=\"parent.HTB_IU_PreviewPicture('" + editorid + "','" + filesHiddenInput.name + "','" + fileInfos[0] + "'," + fileInfos[3] + "," + fileInfos[4] + ");\">";
                contents += "<td>";
                contents += fileInfos[0];
                contents += "</td>";
                contents += "<td>";
                contents += fileInfos[1];
                contents += "</td>";

                if (disdelete.toUpperCase() == 'FALSE') {
                    contents += "<td>";
                    if (filesHiddenInput.name != '') {
                        contents += "<img src='" + imageDir + deleteIcon + "' alt='delete' onclick=\"" + HTB_StringReplace(postback, "$FILETODEL$", "DELETEFILE:" + serverPath + "\\\\" + filesHiddenInput.name + "\\\\" + fileInfos[0]) + "\">";
                    }
                    else
                        contents += "<img src='" + imageDir + deleteIcon + "' alt='delete' onclick=\"" + HTB_StringReplace(postback, "$FILETODEL$", "DELETEFILE:" + serverPath + "\\\\" + fileInfos[0]) + ">";
                    contents += "</td>";
                }
                contents += "</tr>";
            }
        }
    }

    contents += '</table>';
    div.contentWindow.document.write(contents);

    //HTB_IU_RemoveFilesContents();

    /*		var filesHiddenInput = document.getElementById('HTB_IU_dir_' + directory);
    if (filesHiddenInput != null)
    {
    var reg=new RegExp('[;]+', 'g');
    var filesInfo = filesHiddenInput.value.split(reg);

    var table = document.getElementById("HTB_IU_tableContents");
    var tablebody = document.createElement("TBODY");
    tablebody.setAttribute('id','HTB_UI_tableContentsBody'); 
    for(i=0 ; i < filesInfo.length ; i++) 
    {
    reg=new RegExp('[|]+', 'g');
    fileInfo = filesInfo[i].split(reg);				

    current_row=document.createElement("TR");

    current_cell=document.createElement("TD");
    currenttext=document.createTextNode(fileInfo[0]);
    current_cell.appendChild(currenttext);
    current_row.appendChild(current_cell);
    tablebody.appendChild(current_row);

    current_cell=document.createElement("TD");

    currenttext=document.createTextNode(fileInfo[1]);
    current_cell.appendChild(currenttext);
    current_row.appendChild(current_cell);
    tablebody.appendChild(current_row);

    current_cell=document.createElement("TD");
    currenttext=document.createTextNode("IMAGE");
    current_cell.appendChild(currenttext);
    current_row.appendChild(current_cell);
    tablebody.appendChild(current_row);

    }
    table.appendChild(tablebody);
    }*/

}

function HTB_ToggleNode(framename, node, base) {
    var iframe = document.getElementById(framename);
    nodeDiv = iframe.contentWindow.document.getElementById(node + '_div');
    nodeImg = iframe.contentWindow.document.getElementById(node + '_img');
    nodeSta = iframe.contentWindow.document.getElementById(node);

    if (nodeDiv.style.display == 'block') {
        nodeDiv.style.display = 'none';
        nodeImg.src = eval('atv_' + base + '_co');
        nodeSta.value = 'False';
    }
    else {
        nodeDiv.style.display = 'block';
        nodeImg.src = eval('atv_' + base + '_ex');
        nodeSta.value = 'True';
    }
}

function HTB_LoadNode(framename, tree, node, content, base, path) {
    var iframe = document.getElementById(framename);

    var xmlhttp = getXMLHttp();
    var div = iframe.contentWindow.document.getElementById(content + '_div');

    if (div.innerHTML == "") {
        var span = iframe.contentWindow.document.getElementById(content + '_text');
        var saveData = span.innerHTML;
        span.innerHTML = "Loading...";

        xmlhttp.open("GET", path + "?tree=" + tree + "&node=" + node, true);
        xmlhttp.onreadystatechange = function() {
            if (xmlhttp.readyState == 4) {
                span.innerHTML = saveData;
                div.innerHTML = xmlhttp.responseText;
                div.innerHTML = HTB_ReplaceAllOccurence(div.innerHTML, "toggleNode(", "parent.HTB_ToggleNode('" + framename + "',");
                div.innerHTML = HTB_ReplaceAllOccurence(div.innerHTML, "selectNode(", "parent.HTB_IU_SelectNode('" + framename + "',");
                HTB_ToggleNode(framename, content, base);

            }
        }
        xmlhttp.send(null);
    }

    else {
        HTB_ToggleNode(framename, content, base);
    }
}

function HTB_SelectNode(id) {

}

function HTB_ReplaceAllOccurence(stringToReplace, oldvalue, newvalue) {
    //var str = 'maria joão';
    //var replace = str.replace('maria', 'joão');
    //alert(replace);
    //alert(oldvalue);
    //alert(newvalue);
    //alert(stringToReplace);


    var result = stringToReplace.replace(oldvalue, newvalue);
    //alert(result);
    var ndx = stringToReplace.indexOf(oldvalue);
    if (ndx != -1) {
        return HTB_ReplaceAllOccurence(result, oldvalue, newvalue);
    }
    return result;
}

function HTB_IU_PreviewPicture(editorid, directoryName, fileName, width, height) {
    if (width > 216)
        width = 216;

    if (height > 144)
        height = 144;

    document.getElementById(editorid + '_HTB_IU_preview').src = directoryName + '/' + fileName;
    document.getElementById(editorid + '_HTB_IU_preview').width = width;
    document.getElementById(editorid + '_HTB_IU_preview').height = height;

}

function HTB_IU_AddImage(editorid) {
    var align = HTB_GetSelectedOptionValue(editorid + '_HTB_IU_alignment');
    var src = document.getElementById(editorid + '_HTB_IU_preview').src;
    var realSrc = HTB_GetRealSrc(editorid + '_HTB_IU_preview', 'src');
    if (realSrc != null)
        src = realSrc;
    HTB_AddImage(editorid, src, document.getElementById(editorid + '_HTB_IU_text').value, null, null, document.getElementById(editorid + '_HTB_IU_borderThikness').value, document.getElementById(editorid + '_HTB_IU_horizontalSpacing').value, document.getElementById(editorid + '_HTB_IU_verticalSpacing').value, align, null);
}

function HTB_GetRealSrc(id, attribute) {
    var obj = document.getElementById(id);
    if (obj != null) {
        var outerHTML = obj.outerHTML;
        var indexAttribute = outerHTML.indexOf(attribute);
        if (indexAttribute != -1) {
            var startIndex = -1;
            var endIndex = -1;

            startIndex = outerHTML.indexOf("\"", indexAttribute);
            if (startIndex != -1) {
                endIndex = outerHTML.indexOf("\"", startIndex + 1);

                if (startIndex != -1 && endIndex != -1)
                    return outerHTML.substring(startIndex + 1, endIndex);

                else

                    return null;
            }

            return null;
        }

    }

    else
        return null;
}

function HTB_AddImage(editorid, source, text, width, height, borderThickness, horizontalSpacing, verticalSpacing, align, cssClass) {
    var image = '<img';

    if (source != null && source != '')
        image += ' src=\'' + source + '\'';
    if (text != null && text != '')
        image += ' alt=\'' + text + '\'';
    if (borderThickness != null && borderThickness != '')
        image += ' border=\'' + borderThickness + '\'';
    if (horizontalSpacing != null && horizontalSpacing != '')
        image += ' hspace=\'' + horizontalSpacing + '\'';
    if (verticalSpacing != null && verticalSpacing != '')
        image += ' vspace=\'' + verticalSpacing + '\'';
    if (cssClass != null && cssClass != '')
        image += ' class=\'' + cssClass + '\'';

    if (align != '' && align != 'default')
        image += ' align=\'' + align + '\'';

    if (width != null && width != '')
        image += ' width=\'' + width + '\'';

    if (height != null && height != '')
        image += ' height=\'' + height + '\'';

    image += ' id=\'HTB_tempIdImage\'';

    image += '>';
    HTB_SetSnippet(editorid, image);

    var imageObject = HTB_GetEditor(editorid).document.getElementById('HTB_tempIdImage');
    imageObject.src = source;
    imageObject.removeAttribute('id');
}


function HTB_IU_SelectNode(framename, id) {
    var iframe = document.getElementById(framename);
    var base = id.substring(0, id.indexOf('_', 0));
    if (base == '') {
        base = id;
    }
    var curSelNodeID = document.getElementById('atv_' + base + '_curSelNode');

    //changeStyleSelectedNode(iframe.contentWindow.document.getElementById(id),iframe.contentWindow.document.getElementById(id + '_nodeText'),curSelNodeID,document.getElementById(base + '_curSelNodeStyleOriginal'),iframe.contentWindow.document.getElementById(curSelNodeID.value + '_nodeText'),document.getElementById(base + '_nodesStyleSelected'));
}

function HTB_InitImageLibrary(editorid) {
    try {
        //HTB_IU_uploadInDirectory.outerHTML = HTB_RemoveFromOuterHTML(HTB_IU_uploadInDirectory.outerHTML,"value");
        document.getElementById(editorid + '_HTB_IU_preview').width = 0;
        document.getElementById(editorid + '_HTB_IU_preview').height = 0;
        document.getElementById(editorid + '_HTB_IU_text').value = '';
        document.getElementById(editorid + '_HTB_IU_alignment').selectedIndex = 0;
        document.getElementById(editorid + '_HTB_IU_borderThikness').value = '';
        document.getElementById(editorid + '_HTB_IU_horizontalSpacing').value = '0';
        document.getElementById(editorid + '_HTB_IU_verticalSpacing').value = '0';

    }
    catch (e) { }
}


function HTB_IU_GetCurrentSelectedDir(editorid) {
    return document.getElementById(editorid + "_HTB_IU_curSelDirFullPath").value;
}

function HTB_UpdateLength(id) {

    var maxLength = eval(id + '_MaxLength');

    if (maxLength != null && maxLength > 0) {
        editorLength = HTB_GetHtml(id).length;

        if (editorLength >= maxLength) {
            HTB_SetHtml(id, HTB_GetHtml(id).substring(0, maxLength));
            return false;
        }

        document.getElementById(id + '_counterText').value = maxLength - editorLength - 1;
    }

    return true;
}

function HTB_UpdateLengthText(id) {
    maxLength = eval(id + '_MaxLength');
    if (maxLength != null || maxLength > 0) {
        editorLength = HTB_GetHtml(id).length;
        //document.getElementById(id + '_counterText').value = maxLength - editorLength;
        var counter = document.getElementById(id + '_counterText');
        if (counter != null)
            counter.value = maxLength - editorLength;
    }
}


function HTB_InitEventHandlers(iframeObj, id) {
    if (HTB_ns6) {
        editor.addEventListener("blur", function(e) {
            HTB_SaveData(id);

        }, true);

        editor.addEventListener("focus", function(e) {
            HTB_SaveData(id);

        }, true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow = document.frames[iframeObj.id];
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onkeypress = function() {

            //return HTB_UpdateLength(id);  
        }
    }
    else {
        //editor.addEventListener("keypress", function () {return HTB_UpdateLength(id);},true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onkeyup = function() {

            if (iframeObj.frameWindow.event.keyCode == 86 && iframeObj.frameWindow.event.ctrlKey && eval(id + "_CleanOnPaste")) {
                try {
                    HTB_CleanCode(id, false);
                }
                catch (e) {
                }
            }
        }
    }
    else {
        editor.addEventListener("keydown", function(e) {

            if (e.which == 86 && e.modifiers == 2 && eval(id + "_CleanOnPaste")) {
                try {
                    HTB_CleanCode(id, false);

                }

                catch (e) {
                }
            }

        }, true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.oncontextmenu = function() {
            if (!iframeObj.frameWindow.event.ctrlKey) {
                HTB_ShowContextMenu(iframeObj.frameWindow.event, id);
                iframeObj.frameWindow.event.cancelBubble = true;
                iframeObj.frameWindow.event.returnValue = false;
            }
            return false;
        }
    }
    else {
        //editor.addEventListener("contextmenu", function(e)
        editor.addEventListener("oncontextmenu", function() {
            //HTB_ShowContextMenu(iframeObj.frameWindow.event,id);
            //iframeObj.frameWindow.event.cancelBubble = true; 
            //iframeObj.frameWindow.event.returnValue = false;
            //HTB_ShowContextMenu(e, id);
            e.cancelBubble = true;
            e.returnValue = false;

            return false;
        }, true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onmouseup = function() {
            ATB_hideContextMenu(id + '_context');
        }
    }
    else {
        editor.addEventListener("mouseup", function() {
            ATB_hideContextMenu(id + '_context');
        }, true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onmousemove = function() {
            HTB_AutoHideToolBars(id, iframeObj.frameWindow.event);
        }
    }
    else {
        editor.addEventListener("mousemove", function(e) {
            HTB_AutoHideToolBars(id, e);
        }, true);
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onbeforepaste = function() {
            alert(' before paste');
        }
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onkeyup = function() {

            if (iframeObj.frameWindow.event.keyCode == 45 ||
				iframeObj.frameWindow.event.keyCode == 46 ||
				iframeObj.frameWindow.event.keyCode == 36 ||
				iframeObj.frameWindow.event.keyCode == 33 ||
				iframeObj.frameWindow.event.keyCode == 35 ||
				iframeObj.frameWindow.event.keyCode == 34 ||
				iframeObj.frameWindow.event.keyCode == 8 ||
				iframeObj.frameWindow.event.keyCode == 37 ||
				iframeObj.frameWindow.event.keyCode == 38 ||
				iframeObj.frameWindow.event.keyCode == 39 ||
				iframeObj.frameWindow.event.keyCode == 40) {
                HTB_UpdateLengthText(id);
            }
        }
    }

    if (HTB_ie5) {
        iframeObj.frameWindow.document.onkeydown = function() {
            //alert(iframeObj.frameWindow.event.keyCode);
            if (iframeObj.frameWindow.event.keyCode != 45 &&
				iframeObj.frameWindow.event.keyCode != 46 &&
				iframeObj.frameWindow.event.keyCode != 36 &&
				iframeObj.frameWindow.event.keyCode != 33 &&
				iframeObj.frameWindow.event.keyCode != 35 &&
				iframeObj.frameWindow.event.keyCode != 34 &&
				iframeObj.frameWindow.event.keyCode != 8 &&
				iframeObj.frameWindow.event.keyCode != 37 &&
				iframeObj.frameWindow.event.keyCode != 38 &&
				iframeObj.frameWindow.event.keyCode != 39 &&
				iframeObj.frameWindow.event.keyCode != 40) {
                if (HTB_UpdateLength(id) == false)
                    return false;
            }
            else
                HTB_UpdateLengthText(id);


            var shift = false;
            var ctrl = false;
            var alt = false;
            var keysDisabled = eval(id + '_KeysDisabled');
            keysDisabledTab = keysDisabled.split(',');

            for (index = 0; index < keysDisabledTab.length; index++)
                if (keysDisabledTab[index] == iframeObj.frameWindow.event.keyCode)
                return false;
            else if (keysDisabledTab[index] == 16)
                shift = true;
            else if (keysDisabledTab[index] == 17)
                ctrl = true;
            else if (keysDisabledTab[index] == 18)
                alt = true;

            if (alt == true && (iframeObj.frameWindow.event.altKey && iframeObj.frameWindow.event.keyCode))
                return false;
            if (ctrl == true && (iframeObj.frameWindow.event.ctrlKey && iframeObj.frameWindow.event.keyCode))
                return false;
            if (shift == true && (iframeObj.frameWindow.event.shiftKey && iframeObj.frameWindow.event.keyCode))
                return false;

            if (iframeObj.frameWindow.event.keyCode == 75 && iframeObj.frameWindow.event.ctrlKey) {
                if (HTB_GetBR(id).value.toUpperCase() == "TRUE")
                    HTB_GetBR(id).value = "false";

                else
                    HTB_GetBR(id).value = "true";
            }

            if (iframeObj.frameWindow.event.keyCode == 13) {
                if (HTB_GetBR(id) && !IsInList(id)) {

                    HTB_SetFocus(id);
                    sel = HTB_GetEditor(id).document.selection.createRange();
                    sel.pasteHTML("<BR>");

                    sel.select();
                    sel.moveEnd("character", 1);
                    sel.moveStart("character", 1);
                    sel.collapse(false);

                    iframeObj.frameWindow.event.cancelBubble = true;
                    iframeObj.frameWindow.event.returnValue = false;

                    return false;
                }
            }
        }
    }
    else {
        editor.addEventListener("keydown", function(e) {
            /*var keysDisabled = eval(id + '_KeysDisabled');				
            keysDisabledTab = keysDisabled.split(',');
            for(index=0;index<keysDisabledTab.length;index++)
            if (keysDisabledTab[index] == e.which)
            return false;*/

            var shift = false;
            var ctrl = false;
            var alt = false;
            var keysDisabled = eval(id + '_KeysDisabled');
            keysDisabledTab = keysDisabled.split(',');

            for (index = 0; index < keysDisabledTab.length; index++)
                if (keysDisabledTab[index] == e.which)
                return false;
            else if (keysDisabledTab[index] == 16)
                shift = true;
            else if (keysDisabledTab[index] == 17)
                ctrl = true;
            else if (keysDisabledTab[index] == 18)
                alt = true;

            if (alt == true && (iframeObj.frameWindow.event.altKey && e.which))
                return false;
            if (ctrl == true && (iframeObj.frameWindow.event.ctrlKey && e.which))
                return false;
            if (shift == true && (iframeObj.frameWindow.event.shiftKey && e.which))
                return false;

            if (e.which == 75 && modifiers == 2) {
                if (HTB_GetBR(id).value.toUpperCase() == "TRUE")
                    HTB_GetBR(id).value = "false";

                else
                    HTB_GetBR(id).value = "true";
            }

            if (e.which == 13) {
                if (HTB_GetBR(id) && !IsInList(id)) {
                    /*state = eval(id + '_State');
                    state.RestoreSelection();
                    sel = state.GetSelection(id);
                    if (sel) 
                    {
                    range = sel.getRangeAt(0);
                    } 
                    else 
                    {
                    range = editor.document.createRange();
                    }
                    range.insertNode(document.createTextNode('<br>'));*/
                    //HTB_SetSnippet(id, "<br>");

                    e.cancelBubble = true;
                    e.returnValue = false;

                    return false;
                }
            }

        }, true);
    }
}

function IsInList(id) {
    state = eval(id + '_State');
    sel = state.GetSelection(id);
    if (sel != null && sel.type != 'Control' && sel.parentNode) {
        if (sel.parentNode.tagName.toUpperCase() == "UL" ||
			sel.parentNode.tagName.toUpperCase() == "OL" ||
			sel.parentNode.tagName.toUpperCase() == "LI")
            return true;
    }

    return false;
}

function HTB_GetBR(id) {
    return eval(id + '_UseBR');
}

function HTB_ShowContextMenu(e, editorid) {
    HTB_SaveData(editorid);
    state = eval(editorid + '_State');
    sel = state.GetSelection(editorid);

    tag = (HTB_ie5) ? e.srcElement.tagName.toUpperCase() : e.target.tagName.toUpperCase();
    element = (HTB_ie5) ? e.srcElement : e.target;
    if (element.parentNode.tagName) {
        tagParent = element.parentNode.tagName.toUpperCase();
    }
    else {
        tagParent = null;
    }
    elementParent = element.parentNode;

    HTB_DestroyContextMenu(editorid);

    var items = '';
    var isItemsPresent = false;
    var copyOK = false;
    if (HTB_ie5) {
        if (sel.type.toUpperCase() != 'NONE') {
            copyOK = true;
        }
    }
    else {
        if (sel.anchorNode.nodeValue != null) {
            copyOK = true;
        }
    }

    if (copyOK) {
        items += HTB_AddItemContextMenu(editorid, 'Copy', 'javascript:HTB_CommandBuilder(\'' + editorid + '\',\'copy\',null);');
    }

    items += HTB_AddItemContextMenu(editorid, 'Paste', 'javascript:HTB_CommandBuilder(\'' + editorid + '\',\'paste\',null);');
    isItemsPresent = true;

    if (tag == 'TABLE' || tag == 'TD' || tagParent == 'TABLE' || tagParent == 'TD') {
        items += HTB_AddContextMenuTable(editorid, isItemsPresent);
        //isItemsPresent = true;
    }
    if ((tag == 'TABLE' && element.border == 0) || (tag == 'TD' && element.parentNode.parentNode.parentNode.border == 0) || (tagParent == 'TABLE' && element.border == 0) || (tagParent == 'TD' && element.parentNode.parentNode.parentNode.border == 0)) {
        items += HTB_AddContextMenuToggle(editorid, isItemsPresent);
        //isItemsPresent = true;
    }
    if (tag == 'A' || tagParent == 'A') {
        items += HTB_AddContextMenuLink(editorid, isItemsPresent);
        //isItemsPresent = true;
    }
    if (tag == 'IMG' || tagParent == 'IMG') {
        items += HTB_AddContextMenuImage(editorid, isItemsPresent);
        //isItemsPresent = true;
    }

    items += HTB_AddContextMenuCustom(editorid, isItemsPresent);

    if (items != '') {
        HTB_CreateContextMenu(editorid, items);

        var area = document.getElementById(editorid + 'AREA');
        var container = document.getElementById(editorid + ':_container');
        /*alert(container.clientHeight + ':' + container.scrollHeight + ':' + container.offsetHeight);
        alert(area.clientTop + ':' + area.scrollTop + ':' + area.offsetTop);
        alert(e.screenY + ':' + e.clientY + ':' + e.offsetX + ':' + e.x);*/
        HTB_MoveContextMenuTo(editorid, e.screenX, e.screenY - 120);
        //HTB_MoveContextMenuTo(editorid,area.offsetLeft + e.clientX, area.offsetTop + container.offsetHeight + e.clientY);
    }

    HTB_SaveData(editorid);

}

function HTB_MoveContextMenuTo(editorid, x, y) {
    ATB_moveContextMenuTo(editorid + '_context', x - 7, y - document.getElementById(editorid + '_context_window').offsetHeight / 2);
}

function HTB_DestroyContextMenu(editorid) {
    ATB_destroyContextMenu(editorid + '_context');
}

function HTB_CreateContextMenu(editorid, items) {
    var contents = '<table width=100%><tr align=\'center\'><td><table>';
    contents += items;
    contents += '</table></td></tr></table>';
    ATB_createContextMenu(editorid + '_context', 10, 10, contents, eval(editorid + '_ContextMenuBackColor'), eval(editorid + '_ContextMenuBorderColor'), eval(editorid + '_ContextMenuBorderStyle'), eval(editorid + '_ContextMenuBorderWidth'));
}

function HTB_AddContextMenuTable(editorid, isItemsPresent) {
    var contextMenuItems = '';
    if (isItemsPresent)
        contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', '');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Insert Row', 'javascript:parent.HTB_InsertRow(\'' + editorid + '\');');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Delete Row', 'javascript:parent.HTB_DeleteRow(\'' + editorid + '\');');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', null);
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Insert Column', 'javascript:parent.HTB_InsertColumn(\'' + editorid + '\');');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Delete Column', 'javascript:parent.HTB_DeleteColumn(\'' + editorid + '\');');
    return contextMenuItems;
}

function HTB_AddContextMenuToggle(editorid, isItemsPresent) {
    var contextMenuItems = '';
    if (isItemsPresent)
        contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', '');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Toggle Border', 'javascript:parent.HTB_ToggleBorder(\'' + editorid + '\');');
    return contextMenuItems;
}

function HTB_AddContextMenuLink(editorid, isItemsPresent) {
    var contextMenuItems = '';
    if (isItemsPresent)
        contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', '');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Follow Link', 'javascript:parent.HTB_FollowLink(\'' + editorid + '\');');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Remove Link', 'javascript:parent.HTB_CommandBuilder(\'' + editorid + '\',\'unlink\');');
    return contextMenuItems;
}

function HTB_AddContextMenuImage(editorid, isItemsPresent) {
    var contextMenuItems = '';
    if (isItemsPresent)
        contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', '');
    contextMenuItems += HTB_AddItemContextMenu(editorid, 'Modify Image', 'javascript:parent.HTB_ModifyImageContextMenu(\'' + editorid + '\');');
    return contextMenuItems;
}

function HTB_AddContextMenuCustom(editorid, isItemsPresent) {
    var contextMenuItems = '';
    var separatorAdded = false;

    var itemsValue = eval(editorid + '_ContextMenuCustom').split('|');
    for (index = 0; index < itemsValue.length; index++) {
        var itemObject = itemsValue[index].split('$');
        var onclick = '';
        var text = '';

        if (itemObject[0] != null && itemObject[0] != '')
            text = itemObject[0];
        if (itemObject[1] != null && itemObject[1] != '')
            onclick = itemObject[1];

        if (text != '' && onclick != '') {
            if (isItemsPresent && separatorAdded == false) {
                contextMenuItems += HTB_AddItemContextMenu(editorid, 'separator', '');
                separatorAdded = true;
            }

            contextMenuItems += HTB_AddItemContextMenu(editorid, text, onclick);
        }
    }
    return contextMenuItems;
}

function HTB_AddItemContextMenu(editorid, text, onclick) {
    var item = '<tr align=\'center\'><td nowrap';

    if (onclick != null && onclick != '')
        item += ' onclick=' + onclick + 'ATB_hideContextMenu(\'' + editorid + '_context\');';
    var cssClass = eval(editorid + '_ContextMenuCssClass');
    if (cssClass != '')
        item += ' class=' + cssClass;

    if (text.toUpperCase() == 'SEPARATOR') {
        item += '>';
        item += '<hr>';
    }
    else {
        var foreColor = eval(editorid + '_ContextMenuForeColor');
        item += ' style=\'cursor:hand;'
        if (foreColor != '')
            item += 'color:' + foreColor + ';';
        item += '\'';
        item += HTB_GetContextMenuRollOver(editorid) + '>';
        item += text;
    }

    return item += '</td></tr>\n';
}

function HTB_GetContextMenuRollOver(editorid) {
    var contextMenuRollOver = '';
    contextMenuRollOver += ' onmouseover=\'this.style.backgroundColor="' + eval(editorid + '_ContextMenuBackColorRollOver') + '";this.style.color="' + eval(editorid + '_ContextMenuForeColorRollOver') + '";\'';
    contextMenuRollOver += ' onmouseout=\'this.style.backgroundColor="' + eval(editorid + '_ContextMenuBackColor') + '";this.style.color="' + eval(editorid + '_ContextMenuForeColor') + '";\'';
    return contextMenuRollOver;
}

function HTB_InsertRow(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();

    var eTable = HTB_GetTableElement();

    if (eTable != null) {
        if (HTB_GetTdElement() == null)
            eRow = eTable.insertRow();
        else {
            eRow = eTable.insertRow(HTB_GetTdElement().parentNode.rowIndex);
        }

        for (index = 0; index < eTable.rows[(eRow.rowIndex == 0 ? 1 : 0)].cells.length; index++) {
            newCell = eRow.insertCell();
            if (eTable.border == 0 && eTable.style.borderLeft == 'silver 1px dotted')
                HTB_TogglePreviewStyle(newCell);
        }
    }

    state.RestoreSelection();
    HTB_SetFocus(editorid);
}

function HTB_DeleteRow(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();

    try {
        tdElement = HTB_GetTdElement();

        rowIndex = tdElement.parentNode.rowIndex;
        eTable = tdElement.parentNode.parentNode.parentNode;
        eTable.deleteRow(rowIndex);

        if (eTable.rows.length == 0)
            eTable.removeNode(true);
    }
    catch (e) { }

    state.RestoreSelection();
    HTB_SetFocus(editorid);
}

function HTB_InsertColumn(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();
    sel = state.GetSelection(editorid);

    var eTable = HTB_GetTableElement();

    if (!HTB_GetTdElement())
        cellIndex = -1;
    else
        cellIndex = HTB_GetTdElement().cellIndex;

    if (eTable != null) {
        for (index = 0; index < eTable.rows.length; index++) {
            newCell = eTable.rows[index].insertCell(cellIndex);
            if (eTable.border == 0 && eTable.style.borderLeft == 'silver 1px dotted')
                HTB_TogglePreviewStyle(newCell);
        }
    }

    state.RestoreSelection();
    HTB_SetFocus(editorid);
}

function HTB_DeleteColumn(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();

    try {
        tdElement = HTB_GetTdElement();

        cellIndex = tdElement.cellIndex;
        eTable = tdElement.parentNode.parentNode.parentNode;
        for (index = 0; index < eTable.rows.length; index++)
            eTable.rows[index].deleteCell(cellIndex);

        if (eTable.cells.length == 0)
            eTable.removeNode(true);
    }
    catch (e) { }

    state.RestoreSelection();
    HTB_SetFocus(editorid);
}

function HTB_GetTableElement() {
    if (element.tagName.toUpperCase() == 'TABLE')
        return element;
    else if (element.tagName.toUpperCase() == 'TD')
        return element.parentNode.parentNode.parentNode;
    else if (elementParent.tagName.toUpperCase() == 'TABLE')
        return elementParent;
    else if (elementParent.tagName.toUpperCase() == 'TD')
        return elementParent.parentNode.parentNode.parentNode;

    return null;
}

function HTB_GetTdElement() {
    if (element.tagName.toUpperCase() == 'TD')
        return element;
    else if (elementParent.tagName.toUpperCase() == 'TD')
        return elementParent;

    return null;
}

function HTB_TogglePreviewStyle(obj) {
    try {
        if (obj.style.borderLeft == 'silver 1px dotted')
            obj.style.borderLeft = obj.style.borderRight = obj.style.borderTop = obj.style.borderBottom = '';
        else
            obj.style.borderLeft = obj.style.borderRight = obj.style.borderTop = obj.style.borderBottom = 'silver 1px dotted';
    }
    catch (e)
	{ }
}

function HTB_ToggleBorder(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();

    var eTable = null;

    if (element.tagName.toUpperCase() == 'TABLE')
        eTable = element;
    else if (element.tagName.toUpperCase() == 'TD')
        eTable = element.parentNode.parentNode.parentNode;
    else if (elementParent.tagName.toUpperCase() == 'TABLE')
        eTable = elementParent;
    else if (elementParent.tagName.toUpperCase() == 'TD')
        eTable = elementParent.parentNode.parentNode.parentNode;

    HTB_TogglePreviewStyle(eTable);

    for (i = 0; i < eTable.cells.length; i++)
        HTB_TogglePreviewStyle(eTable.cells[i]);

    state.RestoreSelection();
    HTB_SetFocus(editorid);
}

function HTB_FollowLink(editorid) {
    state = eval(editorid + '_State');
    state.RestoreSelection();

    link = element.getAttribute('HREF');
    if (link.length > 0)
        window.open(link, '_blank');
    else
        alert('No link to follow. Check the code.');
}

function HTB_F_CheckFind(editorid) {
    if (HTB_ie5) {
        if (document.getElementById(editorid + '_HTB_F_find').value == '')
            document.getElementById(editorid + '_HTB_F_next').disabled = true;
        else {
            if (document.getElementById(editorid + '_HTB_F_next').disabled == true)
                document.getElementById(editorid + '_HTB_F_next').disabled = false;
        }
    }
    else {
        if (document.getElementById(editorid + '_HTB_F_find').value == '')
            document.getElementById(editorid + '_HTB_F_next').disabled = true;
        else {
            if (document.getElementById(editorid + '_HTB_F_next').disabled == true)
                document.getElementById(editorid + '_HTB_F_next').disabled = false;
        }
    }
}


function HTB_FR_CheckFindAndReplace(editorid) {
    if (HTB_ie5) {
        if (document.getElementById(editorid + '_HTB_FR_find').value == '') {
            document.getElementById(editorid + '_HTB_FR_replaceAll').disabled = true;
            document.getElementById(editorid + '_HTB_FR_replaceOne').disabled = true;
            document.getElementById(editorid + '_HTB_FR_next').disabled = true;
        }
        else {
            if (document.getElementById(editorid + '_HTB_FR_replaceAll').disabled == true)
                document.getElementById(editorid + '_HTB_FR_replaceAll').disabled = false;
            if (document.getElementById(editorid + '_HTB_FR_replaceOne').disabled == true)
                document.getElementById(editorid + '_HTB_FR_replaceOne').disabled = false;
            if (document.getElementById(editorid + '_HTB_FR_next').disabled == true)
                document.getElementById(editorid + '_HTB_FR_next').disabled = false;
        }
    }
    else {
        document.getElementById(editorid + '_HTB_FR_caseSensitive').disabled = true;
        document.getElementById(editorid + '_HTB_FR_wholeWord').disabled = true;
    }
}

function HTB_AutoHideToolBars(editorid, e) {

    if (eval(editorid + '_AutoHideToolBars') == true) {
        var container = document.getElementById(editorid + ':_container');

        if (e.clientY < 10 && container.style.display == 'none')
            HTB_ShowToolbarsContainer(editorid);
        else if (e.clientY > 0 && container.style.display == '')
            HTB_HideToolbarsContainer(editorid);
    }
}

function HTB_FL_InitFlashEditor(editorid) {
    /*try
    {*/
    if (HTB_ie5) {
        state = eval(editorid + '_State');
        sel = state.GetSelection(editorid);

        if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'EMBED') {
            var index = 0;
            var flash = sel.item(0);

            document.getElementById(editorid + '_HTB_FL_file').value = flash.src;

            if (flash.outerHTML.indexOf('classid') == -1) {
                document.getElementById(editorid + '_HTB_FL_specifyClassID').checked = false;
                document.getElementById(editorid + '_HTB_FL_textClassID').disabled = true;
                document.getElementById(editorid + '_HTB_FL_classID').value = 'clsid:';
                document.getElementById(editorid + '_HTB_FL_classID').disabled = true;
                document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = true;
                document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = true;
                document.getElementById(editorid + '_HTB_FL_flashVersion').selectedIndex = 0;
            }

            else {
                document.getElementById(editorid + '_HTB_FL_specifyClassID').checked = true;
                document.getElementById(editorid + '_HTB_FL_textClassID').disabled = false;
                document.getElementById(editorid + '_HTB_FL_classID').value = HTB_GetFromOuterHTML(flash.outerHTML, 'classid');
                document.getElementById(editorid + '_HTB_FL_classID').disabled = false;
                document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = false;
                document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = false;

                var beforeversion = 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=';
                var indexversion = flash.outerHTML.indexOf(beforeversion);
                var version = '';
                if (indexversion != -1) {
                    version = flash.outerHTML.substring(indexversion + beforeversion.length, indexversion + beforeversion.length + 1);
                    versionindex = HTB_GetIndexOptionFromValue('HTB_FL_flashVersion', version);
                    if (versionindex == -1)
                        document.getElementById(editorid + '_HTB_FL_flashVersion').selectedIndex = 0;
                    else
                        document.getElementById(editorid + '_HTB_FL_flashVersion').selectedIndex = versionindex;
                }
            }

            index = HTB_GetIndexOptionFromValue(editorid + '_HTB_FL_alignment', HTB_GetFromOuterHTML(flash.outerHTML, 'align'));
            if (index != -1)
                document.getElementById(editorid + '_HTB_FL_alignment').selectedIndex = index;
            else
                document.getElementById(editorid + '_HTB_FL_alignment').selectedIndex = 0;

            index = HTB_GetIndexOptionFromValue(editorid + '_HTB_FL_flashAlignment', HTB_GetFromOuterHTML(flash.outerHTML, 'salign'));
            if (index != -1)
                document.getElementById(editorid + '_HTB_FL_flashAlignment').selectedIndex = index;
            else
                document.getElementById(editorid + '_HTB_FL_flashAlignment').selectedIndex = 0;

            var w = HTB_GetFromOuterHTML(flash.outerHTML, 'width');
            var h = HTB_GetFromOuterHTML(flash.outerHTML, 'height');

            if (w != '' || h != '') {
                document.getElementById(editorid + '_HTB_FL_textWidth').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = false;
                document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = false;
                document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = false;

                document.getElementById(editorid + '_HTB_FL_textHeight').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = false;
                document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = false;
                document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = false;
                document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = false;
            }
            else {
                document.getElementById(editorid + '_HTB_FL_textWidth').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyValueWidth').value = '0';
                document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = true;
                document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = true;
                document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = true;

                document.getElementById(editorid + '_HTB_FL_textHeight').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyValueHeight').value = '0';
                document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = true;
                document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = true;
                document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = true;
                document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = true;
            }

            if (w != '') {
                if (w.indexOf('%') >= 0)
                    document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').checked = true;
                else
                    document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').checked = true;

                document.getElementById(editorid + '_HTB_FL_specifyValueWidth').value = parseInt(w);
            }

            if (h != '') {
                if (h.indexOf('%') >= 0)
                    document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').checked = true;
                else
                    document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').checked = true;

                document.getElementById(editorid + '_HTB_FL_specifyValueHeight').value = parseInt(h);
            }

            index = HTB_GetIndexOptionFromValue(editorid + '_HTB_FL_quality', HTB_GetFromOuterHTML(flash.outerHTML, 'quality'));
            if (index != -1)
                document.getElementById(editorid + '_HTB_FL_quality').selectedIndex = index;
            else
                document.getElementById(editorid + '_HTB_FL_quality').selectedIndex = 0;

            if (flash.outerHTML.indexOf('loop') == -1)
                document.getElementById(editorid + '_HTB_FL_loopYes').checked = true;
            else {
                var loop = HTB_GetFromOuterHTML(flash.outerHTML, 'loop');
                if (loop.toLowerCase() == 'true')
                    document.getElementById(editorid + '_HTB_FL_loopYes').checked = true;
                else
                    document.getElementById(editorid + '_HTB_FL_loopNo').checked = true;

            }

            if (flash.outerHTML.indexOf('play') == -1)
                document.getElementById(editorid + '_HTB_FL_autoReadYes').checked = true;
            else {
                var play = HTB_GetFromOuterHTML(flash.outerHTML, 'play');
                if (play.toLowerCase() == 'true')
                    document.getElementById(editorid + '_HTB_FL_autoReadYes').checked = true;
                else
                    document.getElementById(editorid + '_HTB_FL_autoReadNo').checked = true;

            }

        }

        else {
            document.getElementById(editorid + '_HTB_FL_file').value = '';
            document.getElementById(editorid + '_HTB_FL_specifyClassID').checked = false;
            document.getElementById(editorid + '_HTB_FL_textClassID').disabled = true;
            document.getElementById(editorid + '_HTB_FL_classID').value = 'clsid:';
            document.getElementById(editorid + '_HTB_FL_classID').disabled = true;
            document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = true;
            document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = true;
            document.getElementById(editorid + '_HTB_FL_flashVersion').selectedIndex = 0;

            document.getElementById(editorid + '_HTB_FL_alignment').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_FL_flashAlignment').selectedIndex = 0;

            document.getElementById(editorid + '_HTB_FL_specifySize').checked = false;

            document.getElementById(editorid + '_HTB_FL_textWidth').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyValueWidth').value = '0';
            document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').checked = true;
            document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = true;
            document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = true;

            document.getElementById(editorid + '_HTB_FL_textHeight').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyValueHeight').value = '0';
            document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').checked = true;
            document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = true;
            document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = true;
            document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = true;

            document.getElementById(editorid + '_HTB_FL_quality').selectedIndex = 0;
            document.getElementById(editorid + '_HTB_FL_loopYes').checked = true;
            document.getElementById(editorid + '_HTB_FL_autoReadYes').checked = true;
        }
    }
    else {
        document.getElementById(editorid + '_HTB_FL_file').value = '';
        document.getElementById(editorid + '_HTB_FL_specifyClassID').checked = false;
        document.getElementById(editorid + '_HTB_FL_textClassID').disabled = true;
        document.getElementById(editorid + '_HTB_FL_classID').value = 'clsid:';
        document.getElementById(editorid + '_HTB_FL_classID').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = true;
        document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = true;
        document.getElementById(editorid + '_HTB_FL_flashVersion').selectedIndex = 0;

        document.getElementById(editorid + '_HTB_FL_alignment').selectedIndex = 0;
        document.getElementById(editorid + '_HTB_FL_flashAlignment').selectedIndex = 0;

        document.getElementById(editorid + '_HTB_FL_specifySize').checked = false;

        document.getElementById(editorid + '_HTB_FL_textWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyValueWidth').value = '0';
        document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').checked = true;
        document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = true;

        document.getElementById(editorid + '_HTB_FL_textHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyValueHeight').value = '0';
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').checked = true;
        document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = true;

        document.getElementById(editorid + '_HTB_FL_quality').selectedIndex = 0;
        document.getElementById(editorid + '_HTB_FL_loopYes').checked = true;
        document.getElementById(editorid + '_HTB_FL_autoReadYes').checked = true;
    }
    /*}
    catch (e) {}*/
}

function HTB_FL_EnableDisableSpecifySize(editorid) {
    if (document.getElementById(editorid + '_HTB_FL_specifySize').checked == true) {
        document.getElementById(editorid + '_HTB_FL_textWidth').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = false;
        document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = false;
        document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = false;

        document.getElementById(editorid + '_HTB_FL_textHeight').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = false;
        document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = false;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = false;
        document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = false;

    }

    else {
        document.getElementById(editorid + '_HTB_FL_textWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyValueWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsWidth').checked = true;
        document.getElementById(editorid + '_HTB_FL_textInPixelWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentWidth').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textInPercentWidth').disabled = true;

        document.getElementById(editorid + '_HTB_FL_textHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyValueHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPixelsHeight').checked = true;
        document.getElementById(editorid + '_HTB_FL_textInPixelHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_specifyInPercentHeight').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textInPercentHeight').disabled = true;
    }
}

function HTB_FL_EnableDisableSpecifyClassID(editorid) {
    if (document.getElementById(editorid + '_HTB_FL_specifyClassID').checked == true) {
        document.getElementById(editorid + '_HTB_FL_textClassID').disabled = false;
        document.getElementById(editorid + '_HTB_FL_classID').disabled = false;
        document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = false;
        document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = false;
    }

    else {
        document.getElementById(editorid + '_HTB_FL_textClassID').disabled = true;
        document.getElementById(editorid + '_HTB_FL_classID').disabled = true;
        document.getElementById(editorid + '_HTB_FL_textFlashVersion').disabled = true;
        document.getElementById(editorid + '_HTB_FL_flashVersion').disabled = true;

    }
}

function HTB_FL_CreateFlash(editorid) {
    var flash = '';

    flash += '<EMBED pluginspage=http://www.macromedia.com/go/getflashplayer type=application/x-shockwave-flash';

    if (document.getElementById(editorid + '_HTB_FL_file') != null && document.getElementById(editorid + '_HTB_FL_file') != '')
        flash += ' src=\'' + document.getElementById(editorid + '_HTB_FL_file').value + '\'';

    if (document.getElementById(editorid + '_HTB_FL_specifyClassID').checked == true && document.getElementById(editorid + '_HTB_FL_classID') != '') {
        flash += ' classid=\'' + document.getElementById(editorid + '_HTB_FL_classID').value + '\'';
        flash += ' codebase=\'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=' + HTB_GetSelectedOptionValue(editorid + '_HTB_FL_flashVersion') + ',0,0,0\'';
    }

    flash += ' align=\'' + HTB_GetSelectedOptionValue(editorid + '_HTB_FL_alignment') + '\'';

    flash += ' salign=\'' + HTB_GetSelectedOptionValue(editorid + '_HTB_FL_flashAlignment') + '\'';

    var h = ''; w = '';

    if (document.getElementById(editorid + '_HTB_FL_specifySize').checked == true) {
        w = document.getElementById(editorid + '_HTB_FL_specifyValueWidth').value;
        if (w != '') {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(document.getElementById(editorid + '_HTB_FL_gWidth'));
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName(editorid + '_HTB_FL_gWidth'));

            if (selectedValue == 'Pixels') {
                w += 'px';
            }
            else {
                w += '%';
            }

            flash += ' width=\'' + w + '\'';
        }

        h = document.getElementById(editorid + '_HTB_FL_specifyValueHeight').value;
        if (h != '') {
            var selectedValue = '';
            if (HTB_ie5)
                selectedValue = HTB_GetSelectedRadioValue(document.getElementById(editorid + '_HTB_FL_gHeight'));
            else
                selectedValue = HTB_GetSelectedRadioValueNS(document.getElementsByName(editorid + '_HTB_FL_gHeight'));

            if (selectedValue == 'Pixels') {
                h += 'px';
            }
            else {
                h += '%';
            }

            flash += ' height=\'' + h + '\'';
        }
    }

    flash += ' quality=\'' + HTB_GetSelectedOptionValue(editorid + '_HTB_FL_quality') + '\'';

    var loop = '';
    if (HTB_ie5) {
        if (HTB_GetSelectedRadioValue(document.getElementById(editorid + '_HTB_FL_gLoop')) == 'yes')
            loop = 'true';
        else
            loop = 'false';
    }
    else {
        if (HTB_GetSelectedRadioValueNS(document.getElementsByName(editorid + '_HTB_FL_gLoop')) == 'yes')
            loop = 'true';
        else
            loop = 'false';
    }

    flash += ' loop=\'' + loop + '\'';

    var autoread = '';

    if (HTB_ie5) {
        if (HTB_GetSelectedRadioValue(document.getElementById(editorid + '_HTB_FL_gAutoRead')) == 'yes')
            autoread = 'true';
        else
            autoread = 'false';
    }
    else {
        if (HTB_GetSelectedRadioValueNS(document.getElementsByName(editorid + '_HTB_FL_gAutoRead')) == 'yes')
            autoread = 'true';
        else
            autoread = 'false';
    }

    flash += ' play=\'' + autoread + '\'';

    flash += '></EMBED>';

    HTB_SetSnippet(editorid, flash);
}

function HTB_GetTextLabel(editorid, id) {
    var textLabels = eval(editorid + '_textLabels');
    if (textLabels != null) {
        if (id * 2 < textLabels.length) {
            return textLabels[id * 2 + 1];
        }
    }

    else return '';

}

function HTB_ModifyImageContextMenu(editorid) {
    HTB_InitImageEditor(editorid);
    ATB_showPopup(editorid + 'EditorPopup');
}

function HTB_ForceUseBR(editorid) {
    if (HTB_ie5) {
        var iframes = document.all.tags("IFRAME");
        var iframe = null;
        for (index = 0; index < iframes.length; index++) {
            if (iframes[index].id == editorid + '_Editor') {
                iframe = iframes[index];
                break;
            }
        }

        if (iframe != null) {
            iframe.frameWindow.document.onkeydown = function() {
                if (iframe.frameWindow.event.keyCode == 13) {
                    HTB_SetFocus(editorid);
                    sel = HTB_GetEditor(editorid).document.selection.createRange();
                    sel.pasteHTML("<BR>");

                    sel.select();
                    sel.moveEnd("character", 1);
                    sel.moveStart("character", 1);
                    sel.collapse(false);
                    iframe.frameWindow.event.cancelBubble = true;
                    iframe.frameWindow.event.returnValue = false;

                    return false;
                }
            }
        }
    }
}

function HTB_Ns6FontSizeClicked(editorid, clientid) {
    var index = ATB_getSelectedIndex(clientid);
    if (index != -1) {
        var values = eval(clientid + '_Values')
        if (values != null) {
            var reg = new RegExp('[;]+', 'g');
            var tabValues = values.split(reg);
            if (tabValues.length >= index)
                HTB_CommandBuilder(editorid, 'fontsize', tabValues[index].toString());
        }
    }
}

function HTB_Ns6FontFaceClicked(editorid, clientid) {
    var index = ATB_getSelectedIndex(clientid);
    if (index != -1) {
        var values = eval(clientid + '_Values')
        if (values != null) {
            var reg = new RegExp('[;]+', 'g');
            var tabValues = values.split(reg);
            if (tabValues.length >= index)
                HTB_CommandBuilder(editorid, 'fontname', tabValues[index].toString());
        }
    }
}

function HTB_Ns6ParagraphClicked(editorid, clientid) {
    var index = ATB_getSelectedIndex(clientid);
    if (index != -1) {
        var values = eval(clientid + '_Values');
        if (values != null) {
            var reg = new RegExp('[;]+', 'g');
            var tabValues = values.split(reg);
            if (tabValues.length >= index)
                HTB_CommandBuilder(editorid, 'formatblock', tabValues[index].toString());
        }
    }
}

function HTB_ResetCaret(id) {
    var editor = HTB_GetEditor(id);
    editor.focus();
    selection = editor.window.getSelection();
    if (selection) {
        range = selection.getRangeAt(0);
    }
    else {
        range = editor.document.createRange();
    }
    var node = range.startContainer;
    var pos = range.startOffset;

    switch (node.nodeType) {
        case 1:
            {
                node = node.childNodes[pos];
                node.parentNode.insertBefore(fragment, node);
                range.setEnd(node, 0);
                range.setStart(node, 0);
                range.collapse(true);
                selection.removeAllRanges();
                range.deleteContents();

            }
            break;

        case 3:
            {
                node = node.splitText(pos);
                range.setEnd(node, 0);
                range.setStart(node, 0);
                range.collapse(true);
                selection.removeAllRanges();
                range.deleteContents();
            }
            break;

    }
}

function HTB_ExecuteCustomTags(editorid, codeToExecute) {
    /*state = eval(editorid + '_State');
    state.RestoreSelection();
    selection = state.GetSelection(ideditorid);
    editor = HTB_GetEditor(editorid);*/

    if (codeToExecute != null && codeToExecute != '')
        window.setTimeout(codeToExecute);
}

function HTB_SetCustomTag(id, name, attribute, value) {
    state = eval(id + '_State');
    state.RestoreSelection();
    selection = state.GetSelection(id);
    editor = HTB_GetEditor(id);

    var text = selection.text;
    var newtext = '<' + name;
    if (attribute.length != null && attribute.length > 0 && value != null && value.length > 0)
        newtext += ' ' + attribute + '=\'' + value + '\'';
    newtext += '>' + text + '</' + name + '>';
    selection.pasteHTML(newtext);
}

function HTB_SetPopupPosition(editorid, popupid) {
    var area = document.getElementById(editorid + '__AREA__');

    if (area.parentNode.tagName.toUpperCase() == 'BODY' || area.parentNode.tagName.toUpperCase() == 'FORM')
        ATB_movePopupTo(popupid, area.offsetLeft + 20, area.offsetTop + 20);
    else
        ATB_movePopupTo(popupid, HTB_GetEditorOffsetLeft(area, 0) + 20, HTB_GetEditorOffsetTop(area, 0) + area.offsetTop);


}

function HTB_GetEditorOffsetLeft(control, current) {
    var parent = control.parentNode;
    if (parent.tagName.toUpperCase() == 'BODY' || parent.tagName.toUpperCase() == 'FORM')
        return current;
    current += parseInt(parent.offsetLeft);
    return HTB_GetEditorOffsetLeft(parent, current);
}

function HTB_GetEditorOffsetTop(control, current) {
    var parent = control.parentNode;
    if (parent.tagName.toUpperCase() == 'BODY' || parent.tagName.toUpperCase() == 'FORM')
        return current;
    if (parent.tagName.toUpperCase() != 'TD')
        current += parseInt(parent.offsetTop);
    return HTB_GetEditorOffsetTop(parent, current);
}

function HTB_EscapeVal(str, replaceWith) {
    str = escape(str);
    for (i = 0; i < str.length; i++) {
        if (str.indexOf("%0D%0A") > -1) {
            str = str.replace("%0D%0A", replaceWith);
        }
        else if (str.indexOf("%0A") > -1) {
            str = str.replace("%0A", replaceWith);
        }
        else if (str.indexOf("%0D") > -1) {
            str = str.replace("%0D", replaceWith);
        }
    }
    str = unescape(str);
    return str;
}

function HTB_ShowBottomToolbar(id) {
    document.getElementById(id + '_bottomToolbar').style.display = '';
}

function HTB_HideBottomToolbar(id) {
    document.getElementById(id + '_bottomToolbar').style.display = 'none';
}

var HTB_SC_textRange = null;
var HTB_SC_nextIndex = 0;

function HTB_InitSpellChecker(editorid, popupid) {

    /*HTB_F_textRange=null;
    HTB_F_count= 0;
    if (HTB_ie5)
    document.getElementById(editorid + '_HTB_F_find').value = '';
    else
    document.getElementById(editorid + '_HTB_F_find').value = '';
    if (HTB_ie5)
    HTB_F_caseSensitive.checked = false;*/

    //HTB_SetPopupPosition(editorid,popupid);
    //ATB_movePopupTo(popupid,100,100);


    /*var area = document.getElementById(editorid + '__AREA__');
	
		if (area.parentNode.tagName.toUpperCase() == 'BODY' || area.parentNode.tagName.toUpperCase() == 'FORM')
    {
    alert(area.offsetLeft + ':' + area.offsetTop);
    ATB_movePopupTo(popupid,area.offsetLeft + 20,area.offsetTop + 20);		
    }
    else
    {
    ATB_movePopupTo(popupid,HTB_GetEditorOffsetLeft(area,0) + 20,HTB_GetEditorOffsetTop(area,0) + area.offsetTop);	
    }*/

    HTB_SetPopupPosition(editorid, popupid);
    ATB_setPopupActive(document.getElementById(popupid + '_window'));
    document.getElementById(editorid + '_Select').style.visibility = 'visible';
    HTB_SC_nextIndex = 0;
    HTB_SC_textRange = null;
    //SC_SetSpellPopupContents(editorid);	
    HTB_SC_FindNext(editorid);
}

function HTB_SC_SetSpellPopupContents(editorid) {
    var reg = new RegExp('[,]+', 'g');
    var words = document.getElementById(editorid + '_words').value.split(reg);
    document.getElementById(editorid + '_NotInDictionary').value = words[HTB_SC_nextIndex];

    var variants = document.getElementById(editorid + '_variants_' + words[HTB_SC_nextIndex].toUpperCase());

    reg = new RegExp('[;]+', 'g');
    var variantstab = variants.value.split(reg);

    var selectObj = document.getElementById(editorid + '_Select');

    for (var i = selectObj.options.length - 1; i >= 0; i--) {
        selectObj.remove(i);
    }

    for (var i = 0; i < variantstab.length; i++) {
        var element = document.createElement("option");
        element.appendChild(document.createTextNode(variantstab[i]));
        element.value = variantstab[i];
        selectObj.appendChild(element);
    }

    selectObj.selectedIndex = 0;
    document.getElementById(editorid + '_ChangeTo').value = selectObj.options[0].text;

    HTB_SC_nextIndex++;
}

function HTB_SC_OnChangeChoice(editorid, selectObj) {
    document.getElementById(editorid + '_ChangeTo').value = selectObj.options[selectObj.selectedIndex].text;
}

function HTB_SC_Close(popupid) {
    ATB_hidePopup(popupid);
}


function HTB_SC_FindNext(editorid) {
    var reg = new RegExp('[,]+', 'g');
    var word = document.getElementById(editorid + '_words').value.split(reg);
    //alert(word.length + ':' + HTB_SC_nextIndex);

    if (HTB_SC_nextIndex < word.length && word.length > 0) {
        HTB_SC_SetSpellPopupContents(editorid);

        var textToFind = word[HTB_SC_nextIndex - 1];

        var strFound = 0;
        var args = HTB_GetArgs(false, true);

        if (HTB_ie5 && HTB_SC_textRange != null) {
            HTB_SC_textRange.collapse(false);
            strFound = HTB_SC_textRange.findText(textToFind, 1000000, args);
            if (strFound) {
                HTB_SC_textRange.select();
            }
        }

        if (HTB_ie5 && HTB_SC_textRange == null && strFound == 0) {
            HTB_SC_textRange = HTB_GetEditor(editorid).document.body.createTextRange();
            HTB_SC_textRange.moveToElementText(HTB_GetEditor(editorid).document.body);
            //alert(textToFind);
            strFound = HTB_SC_textRange.findText(textToFind, 1000000, args);

            if (strFound) {
                HTB_SC_textRange.select();
            }
        }


        if (!strFound) {
            HTB_SC_textRange = null;
            HTB_SC_nextIndex = 0;
            alert(HTB_GetTextLabel(editorid, 144));
        }
        else if (!strFound && HTB_F_count != 0) {
            HTB_SC_textRange = null;
            HTB_SC_nextIndex = 0;
            alert(HTB_GetTextLabel(editorid, 144));
        }
    }

    else {
        alert(HTB_GetTextLabel(editorid, 144));
        HTB_SC_nextIndex = 0;
        HTB_SC_textRange = null;
    }
}

function HTB_SC_Ignore(editorid) {
    HTB_SC_FindNext(editorid);
}

function HTB_SC_IgnoreAll(editorid) {
    HTB_SC_nextIndex = 0;
    HTB_SC_textRange = null;
    alert(HTB_GetTextLabel(editorid, 144));
}

function HTB_SC_Replace(editorid) {
    if (HTB_SC_textRange == null) {
        HTB_SC_FindNext(editorid);
    }

    if (HTB_SC_textRange != null) {
        HTB_SC_textRange.text = document.getElementById(editorid + '_ChangeTo').value;
        HTB_SC_FindNext(editorid);
    }
}

function HTB_SC_ReplaceAll(editorid) {
    var word = eval(editorid + '_words');

    if (HTB_SC_nextIndex > 0)
        HTB_SC_nextIndex--;
    else
        HTB_SC_nextIndex = 0;

    if (HTB_SC_textRange != null) {
        var variants = document.getElementById(editorid + '_variants_' + word[HTB_SC_nextIndex].toUpperCase());

        var reg = new RegExp('[;]+', 'g');
        var variantstab = variants.value.split(reg);

        if (HTB_SC_textRange != null) {
            HTB_SC_textRange.text = variantstab[0];
        }
    }

    for (i = HTB_SC_nextIndex; i < word.length; i++) {
        var textToFind = word[HTB_SC_nextIndex];
        var args = HTB_GetArgs(false, true);
        var strFound = false;
        if (HTB_ie5 && HTB_SC_textRange != null) {
            HTB_SC_textRange.collapse(false);
            var strFound = HTB_SC_textRange.findText(textToFind, 1000000, args);
            if (strFound) {
                HTB_SC_textRange.select();
            }
        }

        if (HTB_ie5 && HTB_SC_textRange == null && strFound == 0) {
            HTB_SC_textRange = HTB_GetEditor(editorid).document.body.createTextRange();
            HTB_SC_textRange.moveToElementText(HTB_GetEditor(editorid).document.body);
            strFound = HTB_SC_textRange.findText(textToFind, 1000000, args);
            if (strFound) {
                HTB_SC_textRange.select();
            }
        }

        if (strFound) {
            var variants = document.getElementById(editorid + '_variants_' + word[HTB_SC_nextIndex].toUpperCase());

            var reg = new RegExp('[;]+', 'g');
            var variantstab = variants.value.split(reg);

            if (HTB_SC_textRange != null) {
                HTB_SC_textRange.text = variantstab[0];
            }
        }

        HTB_SC_nextIndex++;
    }

    HTB_SC_nextIndex = 0;
    HTB_SC_textRange = null;

}

function HTB_Rtrim(chaine) {
    chaine = unescape(chaine);
    return chaine.replace(/(\s*$)/, "");
}

function HTB_GetXMLHttp() {

    var xmlHttp = false;

    // IE
    try {
        xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
    }
    catch (errorMsxml2) {
        try {
            xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (errorMicrosoft) {
            xmlHttp = false;
        }
    }

    // If Mozzilla
    if (!xmlHttp && typeof XMLHttpRequest != 'undefined') {
        xmlHttp = new XMLHttpRequest();
    }

    return xmlHttp;
}

function HTB_DoCallBack(clientId) {
    var theData = '';
    var theform = document.forms[0];
    var thePage = window.location.pathname + window.location.search;
    var eName = '';

    var eventTarget = '';
    var eventArgument = '';

    theData = '__EVENTTARGET=' + escape(eventTarget.split("$").join(":")) + '&';
    theData += '__EVENTARGUMENT=' + eventArgument + '&';
    theData += '__VIEWSTATE=' + escape(theform.__VIEWSTATE.value).replace(new RegExp('\\+', 'g'), '%2b') + '&';
    theData += 'HTB_IsCallBack=true&';
    theData += 'HTB_ClientId=' + clientId + '&';
    theData += 'HTB_Argument=' + eventArgument + '&';
    theData += 'HTB_Contents=' + HTB_GetEditor(clientId).document.body.createTextRange().text + '&';

    for (var i = 0; i < theform.elements.length; i++) {
        eName = theform.elements[i].name;
        if (eName && eName != '') {
            if (eName == '__EVENTTARGET' || eName == '__EVENTARGUMENT' || eName == '__VIEWSTATE') {
                // Do Nothing
            }
            else {

                var tagName = theform.elements[i].tagName.toLowerCase();
                if (tagName == 'input') {
                    var inputType = theform.elements[i].type.toLowerCase();

                    if (inputType == 'text' || inputType == 'password' || inputType == 'hidden') {
                        theData += HTB_AddDataElement(theform.elements[i].name, theform.elements[i].value);
                    }

                    else if (inputType == 'checkbox' || inputType == 'radio') {
                        if (theform.elements[i].checked) {
                            theData += HTB_AddDataElement(theform.elements[i].name, theform.elements[i].value);
                        }
                    }
                }

                else if (tagName == 'select') {
                    var options = theform.elements[i].options;
                    for (var j = 0; j < options.length; j++) {
                        if (options[j].selected) {
                            theData += HTB_AddDataElement(options.name, options[j].value);
                        }
                    }
                }

                else if (tagName == 'textarea') {
                    theData += HTB_AddDataElement(theform.elements[i].name, theform.elements[i].value);
                }
            }
        }
    }

    var xmlHttp = HTB_GetXMLHttp();

    if (xmlHttp.readyState == 4 || xmlHttp.readyState == 0) {
        xmlHttp.open('POST', thePage, true);
        xmlHttp.onreadystatechange = function() {
            if (xmlHttp.readyState == 4) {
                //document.getElementById('_area').value = xmlHttp.responseText;
                window.setTimeout(xmlHttp.responseText, 1);

            }
        };
        xmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        //alert(theData);
        xmlHttp.send(theData);
    }
}

function HTB_AddDataElement(name, value) {
    value = encodeURIComponent(value)
    return '&' + name + '=' + value;
}

function HTB_PopulateList(editorid) {
    alert('entrou');
    var obj = document.getElementById(editorid + '_HTB_IU_uploadInDirectory');
    var filePath = obj.value;
    alert(filePath);
    CallServer(filePath, '');
}

function HTB_ReceiveServerData(value) {
    alert(value);
}

function HTB_PopulateList(editorid) {
    var obj = document.getElementById(editorid + '_HTB_IU_uploadInDirectory');
    EditorID = editorid;
    CallServer(obj.value, '');
}

function HTB_ReceiveServerData(libraryid) {
    __doPostBack(libraryid, HTB_IU_GetCurrentSelectedDir(EditorID));
}
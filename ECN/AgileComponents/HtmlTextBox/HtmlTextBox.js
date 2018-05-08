nv=navigator.appVersion.toUpperCase();
var isIE5p = false;
if (nv.indexOf("MSIE 5.0")!=-1||nv.indexOf("MSIE 5.5")!=-1||nv.indexOf("MSIE 6.0")!=-1)
	isIE5p=true;
var HTB_IE_aspectRatio = 1.00;

function HTB_GetEditor(id)
{
	if (isIE5p)
		return OBJNAME(eval(id + "_Editor"));
	else
		return HTB_GetObj(id + "_Editor").contentWindow;
}

function OBJNAME(n) { '';return n; }

function HTB_GetContainer(id)
{
	return HTB_GetObj(id); 
}

function HTB_GetMode(id)
{ 
	return HTB_GetObj(id + "_Mode");
} 

function HTB_GetObj(id)
{
	return document.getElementById(id)
}

function HTB_InitEditor(id)
{ 
	editor = HTB_GetEditor(id);

	editor.document.open();
	editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
	editor.document.close();

	enabled = true;
	
	if (enabled) 
		HTB_EnableEditor(id);
	
	HTB_SetMode(id, HTB_GetMode(id).value, true);
}

function HTB_EnableEditor(id)
{
	editor = HTB_GetEditor(id);
	editor.document.contentEditable = 'True';
	editor.document.designMode = 'on';
}

function HTB_DisableEditor(id)
{
	HTB_SaveData(id);
	HTB_ClearContent(id);

	editor = HTB_GetEditor(id);

	editor.document.open();
	editor.document.write(HTB_Decode(HTB_GetContainer(id).value));
	editor.document.close();
	
	editor.document.contentEditable = 'False';
	editor.document.designMode = 'off';	
}

function HTB_ClearContent(id)
{
	editor = HTB_GetEditor(id);
	
	editor.document.body.innerHTML = '';
	if (isIE5p)
		editor.document.body.innerText = '';
}

function HTB_SetMode(id, mode, focus)
{	
	mode = mode.toUpperCase();
	
	if (mode == "HTML")
		HTB_SetHtmlMode(id, focus);
	else if (mode == "PREVIEW")
	    HTB_SetPreviewMode(id, focus);
	else
		HTB_SetDesignMode(id, focus);  
}

function HTB_SetDesignMode(id, focus)
{
	var buffer;
	mode = HTB_GetMode(id);
	
	if (mode.value.toUpperCase() == "HTML" || mode.value.toUpperCase() == "PREVIEW")
	{
		editor = HTB_GetEditor(id);
		
	    if (isIE5p)
	    {
			buffer = editor.document.body.innerText;
			editor.document.body.innerHTML=buffer;
		}
		else
		{
			content = editor.document.body.ownerDocument.createRange();
			content.selectNodeContents(editor.document.body);
			editor.document.body.innerHTML = content.toString();
		}
		
	}

	mode.value = "Design";

	document.getElementById(id + ':_cont').style.display = '';
	
	HTB_ShowTab(id, '_design', '_preview,_html');

	if (focus)
	    HTB_SetFocus(id);
	    
	HTB_EnableEditor(id);	
}

function HTB_ToggleMode(id)
{
	mode = HTB_GetMode(id);
	if (mode.value == "HTML")
		HTB_SetDesignMode(id,true);
	else
		HTB_SetHtmlMode(id,true);
}

function HTB_SetHtmlMode(id, focus)
{
	var buffer;
	mode = HTB_GetMode(id);
	
	if (mode.value.toUpperCase() == "DESIGN" || mode.value.toUpperCase() == "PREVIEW")
	{
		editor = HTB_GetEditor(id);
		
		if (isIE5p)
		{
			buffer = editor.document.body.innerHTML;
			editor.document.body.innerText = buffer;		
		}
		else
		{
			content = document.createTextNode(editor.document.body.innerHTML);
			editor.document.body.innerHTML = "";
			editor.document.body.appendChild(content);
		}
	}

	mode.value = "HTML";
	
	document.getElementById(id + ':_cont').style.display = 'none';
		
	HTB_ShowTab(id, '_html', '_design,_preview');
	
	if (focus)
		HTB_SetFocus(id);

}

function HTB_SetPreviewMode(id, focus)
{
    var buffer;
    mode = HTB_GetMode(id);

    if (mode.value.toUpperCase() == "HTML" || mode.value.toUpperCase() == "DESIGN") {
        
        editor = HTB_GetEditor(id);

        if (isIE5p) {
            buffer = editor.document.body.innerText;
            editor.document.body.innerHTML = buffer;
        }
        else 
        {
            content = editor.document.body.ownerDocument.createRange();
            content.selectNodeContents(editor.document.body);
            editor.document.body.innerHTML = content.toString();
        }

        document.getElementById(id + ':_cont').style.display = '';

        if (focus)
            HTB_SetFocus(id);
    }
			
	mode.value = "Preview";

	HTB_ShowTab(id, '_preview', '_design,_html');

	HTB_DisableEditor(id);
}

function HTB_ResetTabs(id, tohide)
{
	if (tohide && tohide.length > 0)
	{
		aTabs = tohide.split(',');
		for(index=0;index<aTabs.length;index++)
			HTB_HideTab(id, aTabs[index]);
	}
}

function HTB_HideTab(id, name)
{
	tab = HTB_GetObj(id + name);
	tab.style.borderRight = '';
	tab.style.borderTop = '';
	tab.style.borderLeft = '';
	tab.style.borderBottom = '';
	tab.style.backgroundColor = '';
}

function HTB_ShowTab(id, name, tohide)
{
	try
	{
		HTB_ResetTabs(id, tohide)
		tab = HTB_GetObj(id + name);
		tab.style.borderRight = 'black 1px solid';
		tab.style.borderTop = 'black 1px solid';
		tab.style.borderLeft = 'black 1px solid';
		tab.style.borderBottom = 'black 1px solid';
		tab.style.backgroundColor = 'white';
	}
	catch (e)
	{
	}
}

function HTB_Decode(str)
{
	str = HTB_StringReplace(str, '&lt;', '<');
	str = HTB_StringReplace(str, '&gt;', '>');
	return str;
}

function HTB_Encode(str)
{
	str = HTB_StringReplace(str, '<', '&lt;');
	str = HTB_StringReplace(str, '>', '&gt;');
	return str;
}

function HTB_StringReplace(str1, str2, str3)
{
	str1 = str1.split(str2).join(str3);
	return str1;
}

function HTB_SetHtml(id, str)
{
	mode = HTB_GetMode(id).value.toUpperCase();
	if (mode == "HTML")
	{
		HTB_SetDesignMode(id, false);
		HTB_GetEditor(id).document.body.innerHTML = str;
		HTB_SetHtmlMode(id, false);
	}
	else
		HTB_GetEditor(id).document.body.innerHTML = str;
}

function HTB_GetHtml(id)
{
	mode = HTB_GetMode(id).value.toUpperCase();
	if (mode == "HTML")
	{
		HTB_SetDesignMode(id, false);
		txt = HTB_GetEditor(id).document.body.innerHTML;
		HTB_SetHtmlMode(id, false);
		return txt;
	}
	else
		return HTB_GetEditor(id).document.body.innerHTML;
}

function HTB_GetText(id)
{
	mode = HTB_GetMode(id).value.toUpperCase();
	
	if (mode == "HTML")
	{	
		HTB_SetDesignMode(id, false);
		txt = HTB_GetEditor(id).document.body.innerText;
		HTB_SetHtmlMode(id, false);
		return txt;
	}
	else
	{
		txt = HTB_GetEditor(id).document.body.innerText;
		return txt;
	}
}

function HTB_SaveData(id)
{
	eval(id + '_State').SaveSelection(id);
	/*container = HTB_GetContainer(id);
	container.value = HTB_Encode(HTB_GetHtml(id));

	if (container.value == '&lt;P&gt;&nbsp;&lt;/P&gt;')
		container.value = '';*/
}

function HTB_GetId(id)
{
	return stringReplace(id, ':', '_');
}

function HTB_CommandBuilder(id, name, arg)
{
	//HTB_DebugTrace(id, name)
	state = eval(id + '_State');
	state.RestoreSelection();
	editor = HTB_GetEditor(id);
	editor.focus();
	
	if (arg == true)
		editor.document.execCommand(name,true,''); 
	else if (name == 'uploadedimage')
		editor.document.execCommand('insertimage',false,arg);
	else if (arg == null)
		editor.document.execCommand(name,'', null); 
	else if (arg != null)
		editor.document.execCommand(name, false, arg);
	else
		editor.document.execCommand(name,'',null);
}

function HTB_DebugTrace(id, str)
{
	debugWindows = HTB_GetObj(id + '_Debug');
	debugWindows.value = str + "\r\n" + debugWindows.value;
}

// Public API

function HTB_SetFocus(id)
{
	editor = HTB_GetEditor(id);
	editor.focus();
}

// Temp

function HTB_SetBold(id)
{
	HTB_CommandBuilder(id, 'bold', null);
}

function HTB_SetItalic(id)
{
	HTB_CommandBuilder(id, 'italic', null);
}

function HTB_SetUnderline(id)
{
	HTB_CommandBuilder(id, 'underline', null);
}

function HTB_OnColorOver(objTable)
{
	HTB_SetBorderColor(objTable, '#0A246A');
	HTB_SetBackColor(objTable, '#B6BDD2');
}

function HTB_OnColorOff(objTable)
{
	HTB_SetBorderColor(objTable, '#F9F8F7');
	HTB_SetBackColor(objTable, '#F9F8F7');
}

function HTB_SetBackColor(obj, color)
{
	if (obj.id == 'HTB_SampleColor')
	{
		
	}
	obj.style.backgroundColor = color;
}

function HTB_SetBorderColor(obj, color)
{
	obj.style.borderColor = color;
}

function HTB_BuildColorTable(id,onClick, disableCustom)
{
	var str, color216 = new Array('00','33','66','99','CC','FF');
	var colorLen = color216.length, color = '';
	cellWidth = 12;
	cellHeight = 12;

	str = '<table><tr><td><table width=225 cellspacing=0 cellpadding=0 onselectstart=\'return false\'>';
	
	for(var f=0;f<2;f++)
	{
		for (var r=0;r<colorLen;r++) {
			str += '<tr>';
			for (var g=colorLen-(1+(f*3));g>=3-(f*3);g--) {
				for (var b=colorLen-1;b>=0;b--) {
					
					color = color216[r]+color216[g]+color216[b];
					
					str +='<td><table width=' + cellWidth + ' height=' + cellHeight + ' cellpadding=0 cellspacing=0><tr><td style=\'cursor:hand\''
						+ ' bgcolor=\'#' + color + '\''
						+ ' title=\'#' + color + '\''
						+ ' onmouseover=\"HTB_SetValue(document.getElementById(\''+ id + '_Color\'), \'#' + color + '\');HTB_SetBackColor(document.getElementById(\'' + id + '_SampleColor\'), \'#' + color + '\')\" '
						+ (onClick ? 'onclick=\"' + HTB_StringReplace(onClick, '$color$', '\'#' + color + '\'') + '\" ' : '')
						+ '></td></tr></table></td>\n';
				}
			}
			str += '</tr>';
		}
	}
	
	str += '<tr><td colspan=36 align=center valign=middle><table><tr><td valign=middle>';
	str += '<table id=\'' + id + '_SampleColor\' style=\'border: solid #666666 1;background-color: #FFFFFF;\' width=40 height=20><tr><td valign=middle></td></tr></table></td><td valign=middle>';
	str += '<span>&nbsp;&nbsp;<b>Custom</b>: <input type=text name=\'' + id + '_Color\' maxlength=7 size=7 ' + (disableCustom ? 'disabled' : '') + '><input type=button value=\'OK\' onclick=\"' + HTB_StringReplace(onClick, '$color$', '\'' + id + '_Color.value\'') + '\"></span></td></tr></table>';
	str += '</td></tr></table></td></tr></table>';

	return str;
	
}

function HTB_BuildTableEditor(id,editorid,popupid)
{
	var str = '';
	
	str += '<table class=\'HTB_clsPopup\'>';
	str += '	<tr>';
	str += '		<td>';
	str += '			<table width=\'100%\'>';
	str += '				<tr>';
	str += '					<td><span class=\'HTB_clsFont\'>Size</span></td>';
	str += '					<td width=\'100%\'><hr size=\'2\' width=\'100%\'>';
	str += '					</td>';
	str += '				</tr>';
	str += '			</table>';
	str += '			<table width=\'100%\'>';
	str += '				<tr>';
	str += '					<td width=\'5\'></td>';
	str += '					<td><span class=\'HTB_clsFont\'>Rows:</span></td>';
	str += '					<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'HTB_TE_tbRows\'></td>';
	str += '					<td><span class=\'HTB_clsFont\'>Cols:</span></td>';
	str += '					<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'HTB_TE_tbCols\'></td>';
	str += '				</tr>';
	str += '			</table>';
	str += '			<table width=\'100%\'>';
	str += '				<tr>';
	str += '					<td><span class=\'HTB_clsFont\'>Layout</span></td>';
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
	str += '								<td><span class=\'HTB_clsFont\'>Alignment:</span></td>';
	str += '								<td><select style=\'width:129px;height:21px\' id=\'HTB_TE_alignment\'>';
	str += '										<option value=\'Default\'>Default</option>';
	str += '										<option value=\'Left\'>Left</option>';
	str += '										<option value=\'Right\'>Right</option>';
	str += '										<option value=\'Center\'>Center</option>';
	str += '									</select></td>';
	str += '								<td>';
	str += '									<input type=\'checkbox\' id=\'HTB_TE_specifyWidth\' onclick=\"HTB_EnableDisableSpecify(\'HTB_TE\',\'Width\');\">';
	str += '								</td>';
	str += '								<td><span class=\'HTB_clsFont\'>Specify width:</span></td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><span class=\'HTB_clsFont\'>Float:</span></td>';
	str += '								<td><select style=\'width:129px;height:21px\' id=\'HTB_TE_float\'>';	
	str += '										<option value=\'Default\'>Default</option>';
	str += '										<option value=\'Left\'>Left</option>';
	str += '										<option value=\'Right\'>Right</option>';
	str += '									</select></td>';
	str += '								<td>&nbsp;</td>';
	str += '								<td valign=\'middle\'>';
	str += '									<table cellpadding=\'0\' cellspacing=\'0\'>';
	str += '										<tr>';
	str += '											<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:60px;height:20px\' id=\'HTB_TE_specifyValueWidth\'></td>';
	str += '											<td><span class=\'HTB_clsFont\'><input type=\'radio\' name=\'HTB_TE_gWidth\' id=\'HTB_TE_specifyInPixelsWidth\' value=\'Pixels\'> In Pixels<br>';
	str += '								<input type=\'radio\' name=\'HTB_TE_gWidth\' id=\'HTB_TE_specifyInPercentWidth\' value=\'Percent\'> In Percent</span></td>';
	str += '										</tr>';
	str += '									</table>';
	str += '								</td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><span class=\'HTB_clsFont\'>Cell Padding:</span></td>';
	str += '								<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:61px;height:20px\' id=\'HTB_TE_cellPadding\'></td>';
	str += '								<td><input type=\'checkbox\' id=\'HTB_TE_specifyHeight\' onclick=\"HTB_EnableDisableSpecify(\'HTB_TE\',\'Height\');\"></td>';
	str += '								<td><span class=\'HTB_clsFont\'>Specify Height:</span></td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><span class=\'HTB_clsFont\'>Cell Spacing:</span></td>';
	str += '								<td><input type=\'text\' size=\'3\' value=\'0\' style=\'width:61px;height:20px\' id=\'HTB_TE_cellSpacing\'></td>';
	str += '								<td>&nbsp;</td>';
	str += '								<td valign=\'middle\'>';
	str += '									<table cellpadding=\'0\' cellspacing=\'0\'>';
	str += '										<tr>';
	str += '											<td><input value=\'0\' type=\'text\' size=\'3\' style=\'width:60px;height:20px\' id=\'HTB_TE_specifyValueHeight\'></td>';
	str += '											<td><span class=\'HTB_clsFont\'><input type=\'radio\' name=\'HTB_TE_gHeight\' id=\'HTB_TE_specifyInPixelsHeight\' value=\'Pixels\'> In Pixels<br>';
	str += '								<input disabled type=\'radio\' name=\'HTB_TE_gHeight\' id=\'HTB_TE_specifyInPercentHeight\' value=\Percent\'> In Percent</span></td>';
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
	str += '					<td><span class=\'HTB_clsFont\'>Borders</span></td>';
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
	str += '								<td><span class=\'HTB_clsFont\'>Size:</span></td>';
	str += '								<td><input type=\'text\' size=\'3\' style=\'width:61px;height:20px\' id=\'HTB_TE_borderSize\'></td>';
	str += '								<td><span class=\'HTB_clsFont\'>Light border:</span></td>';
	str += '								<td>' + HTB_CreateColoredDropDown('HTB_TE_lightBorderColor') + '</td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><span class=\'HTB_clsFont\'>Color:</span></td>';
	str += '								<td>' + HTB_CreateColoredDropDown('HTB_TE_borderColor') + '</td>';
	str += '								<td><span class=\'HTB_clsFont\'>Dark border:</span></td>';
	str += '								<td>' + HTB_CreateColoredDropDown('HTB_TE_darkBorderColor') + '</td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td colspan=\'4\'><input type=\'checkbox\' id=\'HTB_TE_collapseTableBorder\'><span class=\'HTB_clsFont\'>Collapse table border</span></td>';
	str += '							</tr>';
	str += '						</table>';
	str += '					</td>';
	str += '				</tr>';
	str += '			</table>';
	str += '			<table width=\'100%\'>';
	str += '				<tr>';
	str += '					<td><span class=\'HTB_clsFont\'>Background</span></td>';
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
	str += '								<td valign=\'middle\'><span class=\'HTB_clsFont\'>Color:</span>';
	str += 									HTB_CreateColoredDropDown('HTB_TE_bgColor') + '</td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><input type=\'checkbox\' id=\'HTB_TE_useBackgroundPicture\' onclick=\"HTB_EnableDisableBackgroundImage();\"><span class=\'HTB_clsFont\'>Use background picture:</span></td>';
	str += '							</tr>';
	str += '							<tr>';
	str += '								<td><input type=\'text\' style=\'width:100%;height:20px\' id=\'HTB_TE_backgroundPictureValue\'></td>';
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
	str += '						<input type=\'button\' value=\'OK\' style=\'width:75px;height:23px\' onclick=\"HTB_CreateTableFromEditor(\'' + editorid + '\',\'' + popupid + '\')\">&nbsp;&nbsp;';
	str += '						<input type=\'button\' value=\'Cancel\' style=\'width:75px;height:23px\' onclick=\"ATB_hidePopup(\'' + id + '_TableEditor\');\">';
	str += '					</td>';
	str += '				</tr>';
	str += '			</table>';
	str += '		</td>';
	str += '	</tr>';
	str += '</table>';
	
	return str;
}

function HTB_GetSelectedRadioValue(group) 
{
  for(var i=0;i<group.length;i++)
    if(group[i].checked)
	  return group[i].value;
} 

function HTB_CreateTableFromEditor(id,popupid)
{
	var h = '',w = '';
	
	if (HTB_TE_specifyWidth.checked)
	{
		h = HTB_TE_specifyValueWidth.value;
		if (h != '')
		{
			var selectedValue = HTB_GetSelectedRadioValue(HTB_TE_gWidth);
			if (selectedValue == 'Pixels')
			{
				h += 'px';
			}
			else
			{
				h += '%';
			}
		}
	}
	
	if (HTB_TE_specifyHeight)
	{
		w = HTB_TE_specifyValueHeight.value;
		if (w != '')
		{
			var selectedValue = HTB_GetSelectedRadioValue(HTB_TE_gHeight);
			if (selectedValue == 'Pixels')
			{
				w += 'px';
			}
			else
			{
				w += '%';
			}
		}

	}
	
	var background = '';
	if (HTB_TE_useBackgroundPicture.checked)
	{
		background = HTB_TE_backgroundPictureValue.value;
	}
	
	var floatAlign = HTB_GetSelectedOptionText('HTB_TE_float');
	if (floatAlign == 'Default')
		floatAlign = '';
		
	var align = HTB_GetSelectedOptionText('HTB_TE_alignment');
	if (align == 'Default')
		align = '';
			
	HTB_InsertTable(id, HTB_TE_tbCols.value, HTB_TE_tbRows.value, w, h, HTB_TE_borderSize.value, HTB_GetSelectedColoredDropDown('HTB_TE_borderColor'),HTB_GetSelectedColoredDropDown('HTB_TE_lightBorderColor'),HTB_GetSelectedColoredDropDown('HTB_TE_darkBorderColor'),HTB_TE_cellSpacing.value, HTB_TE_cellPadding.value,floatAlign,align, background, HTB_GetSelectedColoredDropDown('HTB_TE_bgColor'),HTB_TE_collapseTableBorder.checked, 1);
	
	ATB_hidePopup(popupid);
}

function HTB_InitTableEditor(id)
{
	state = eval(id + '_State');
	sel = state.GetSelection(id);
	if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'TABLE')
	{
		table = sel.item(0);
		
		HTB_TE_tbRows.value = table.rows.length;
		HTB_TE_tbRows.disabled = true;
		HTB_TE_tbCols.value = table.rows[0].cells.length;
		HTB_TE_tbCols.disabled = true;
		HTB_TE_alignment.selectedIndex = 0;
		HTB_TE_float.selectedIndex = 0;
		
		var w = table.width;
		if (w != '')
		{
			HTB_TE_specifyWidth.checked = true;
			HTB_TE_specifyInPercentWidth.disabled = false;
			HTB_TE_specifyInPixelsWidth.disabled = false;
			HTB_TE_specifyValueWidth.disabled = false;
		    if (w.indexOf('%') > 0)
		    {
				HTB_TE_specifyInPercentWidth.checked = true;
		    }
		    else
		    {
				HTB_TE_specifyInPixelsWidth.checked = true;
		    }
		}
		else
		{
			HTB_TE_specifyValueWidth.disabled = true;
			HTB_TE_specifyWidth.checked = false;
			HTB_TE_specifyInPercentWidth.disabled = true;
			HTB_TE_specifyInPixelsWidth.disabled = true;
			HTB_TE_specifyInPixelsWidth.checked = true;
		}	
		
		var h = table.height;
		if (h != '')
		{
			HTB_TE_specifyHeight.checked = true;
			HTB_TE_specifyInPercentHeight.disabled = false;
			HTB_TE_specifyInPixelsHeight.disabled = false;
			HTB_TE_specifyValueHeight.disabled = false;
		    if (h.indexOf('%') > 0)
		    {
				HTB_TE_specifyInPercentHeight.checked = true;
		    }
		    else
		    {
				HTB_TE_specifyInPixelsHeight.checked = true;
		    }
		}
		else
		{
			HTB_TE_specifyValueHeight.disabled = true;
			HTB_TE_specifyHeight.checked = false;
			HTB_TE_specifyInPercentHeight.disabled = true;
			HTB_TE_specifyInPixelsHeight.disabled = true;
			HTB_TE_specifyInPixelsHeight.checked = true;
		}	

		if (table.cellPadding == '')
			HTB_TE_cellPadding.value = '0';
		else
			HTB_TE_cellPadding.value = table.cellPadding;
		if (table.cellSpacing == '')
			HTB_TE_cellSpacing.value = '0';
		else
			HTB_TE_cellSpacing.value = table.cellSpacing;

		if (table.style.borderCollapse.toUpperCase() == 'COLLAPSE')
			HTB_TE_collapseTableBorder.checked = true;
		else
			HTB_TE_collapseTableBorder.checked = false;
		
		if (table.border != '')
			HTB_TE_borderSize.value = '0';
		else
			HTB_TE_borderSize.value = table.border;
			
		if (table.borderColor == '')
			HTB_TE_borderColor.selectedIndex = 0;
		else
			HTB_SelectColoredDropDownFromValue(HTB_TE_borderColor,table.borderColor);
		
		if (table.borderColorLight == '')
			HTB_TE_lightBorderColor.selectedIndex = 0;
		else
			HTB_SelectColoredDropDownFromValue(HTB_TE_lightBorderColor,table.borderColorLight);
		
		if (table.borderColorDark == '')
			HTB_TE_darkBorderColor.selectedIndex = 0;
		else
			HTB_SelectColoredDropDownFromValue(HTB_TE_darkBorderColor,table.borderColorDark);
			
		if (table.bgColor == '')
			HTB_TE_bgColor.selectedIndex = 0;
		else
			HTB_SelectColoredDropDownFromValue(HTB_TE_bgColor,table.bgColor);		
		
		if (table.background != '')
		{
			HTB_TE_useBackgroundPicture.checked = true;
			HTB_TE_backgroundPictureValue.disabled = false;
			HTB_TE_backgroundPictureValue.value = table.background;
		}
		
		else
		{
			HTB_TE_useBackgroundPicture.checked = false;
			HTB_TE_backgroundPictureValue.disabled = true;
			HTB_TE_backgroundPictureValue.value = '';
		}
	}
	
	else
	{
		HTB_TE_tbRows.value = '3';
		HTB_TE_tbRows.disabled = false;
		HTB_TE_tbCols.value = '3';
		HTB_TE_tbCols.disabled = false;
		HTB_TE_alignment.selectedIndex = 0;
		HTB_TE_float.selectedIndex = 0;
		HTB_TE_specifyValueWidth.disabled = true;
		HTB_TE_specifyValueWidth.value = '0';
		HTB_TE_specifyWidth.checked = false;
		HTB_TE_specifyInPercentWidth.disabled = true;
		HTB_TE_specifyInPixelsWidth.disabled = true;
		HTB_TE_specifyInPixelsWidth.checked = true;
		HTB_TE_specifyValueHeight.disabled = true;
		HTB_TE_specifyValueHeight.value = '0';
		HTB_TE_specifyHeight.checked = false;
		HTB_TE_specifyInPercentHeight.disabled = true;
		HTB_TE_specifyInPixelsHeight.disabled = true;
		HTB_TE_specifyInPixelsHeight.checked = true;
		HTB_TE_cellPadding.value = '2';
		HTB_TE_cellSpacing.value = '1';
		HTB_TE_borderSize.value = '0';
		HTB_TE_borderColor.selectedIndex = 0;
		HTB_TE_lightBorderColor.selectedIndex = 0;
		HTB_TE_darkBorderColor.selectedIndex = 0;
		HTB_TE_collapseTableBorder.checked = false;
		HTB_TE_bgColor.selectedIndex = 0;
		HTB_TE_useBackgroundPicture.checked = false;
		HTB_TE_backgroundPictureValue.disabled = true;
		HTB_TE_backgroundPictureValue.value = '';
	}
				
}

function HTB_AddImage(editorid,source,text,width,height,borderThickness,horizontalSpacing,verticalSpacing,align,cssClass)
{
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
	
	image += '>';			
	HTB_SetSnippet(editorid, image);
}

function HTB_CreateImage(editorid)
{
	var image = '';
	
	image += '<img';
	
	if (HTB_IE_picture != null && HTB_IE_picture.value != '')
		image += ' src=\'' + HTB_IE_picture.value + '\'';
	if (HTB_IE_text != null && HTB_IE_text.value != '')
		image += ' alt=\'' + HTB_IE_text.value + '\'';
	if (HTB_IE_borderThickness != null && HTB_IE_borderThickness.value != '')
		image += ' border=\'' + HTB_IE_borderThickness.value + '\'';
	if (HTB_IE_horizontalSpacing != null && HTB_IE_horizontalSpacing.value != '')
		image += ' hspace=\'' + HTB_IE_horizontalSpacing.value + '\'';
	if (HTB_IE_verticalSpacing != null && HTB_IE_verticalSpacing.value != '')
		image += ' vspace=\'' + HTB_IE_verticalSpacing.value + '\'';
	if (HTB_IE_cssClass != null && HTB_IE_cssClass.value != '')
		image += ' class=\'' + HTB_IE_cssClass.value + '\'';
		
	var align = HTB_GetSelectedOptionValue('HTB_IE_alignment');
	if (align != '' && align != 'default')
		image += ' align=\'' + align + '\'';
		
	var h = '',w = '';
	
	if (HTB_IE_specifySize.checked)
	{
		w = HTB_IE_specifyValueWidth.value;
		if (w != '')
		{
			var selectedValue = HTB_GetSelectedRadioValue(HTB_IE_gWidth);
			if (selectedValue == 'Pixels')
			{
				w += 'px';
			}
			else
			{
				w += '%';
			}
			
			image += ' width=\'' + w + '\'';
		}
		
		h = HTB_IE_specifyValueHeight.value;
		if (w != '')
		{
			var selectedValue = HTB_GetSelectedRadioValue(HTB_IE_gHeight);
			if (selectedValue == 'Pixels')
			{
				h += 'px';
			}
			else
			{
				h += '%';
			}
			image += ' height=\'' + h + '\'';
		}
	}
	
	image += '>';		

	HTB_SetSnippet(editorid, image);
	
}

function HTB_SetImageAspectRatio()
{
	if (HTB_IE_keepAspectRatio.checked == true)
	{
		var w = 1;
		if (HTB_IE_specifyValueWidth.value != '')
		{
			w = parseInt(HTB_IE_specifyValueWidth.value);
			if (w == 0) w = 1;
		}
		else
			w = 1;
	
		var h = 1;
		if (HTB_IE_specifyValueHeight.value != '')
		{
			h = parseInt(HTB_IE_specifyValueHeight.value);
			if (h == 0) h = 1;
		}
		else
			h = 1;
	
		HTB_IE_aspectRatio = h / w;	
	}
}

function HTB_KeepImageAspectRatio(e)
{
	if (HTB_IE_keepAspectRatio.checked == true && HTB_IE_keepAspectRatio.disabled == false)
	{
		if (e.id.indexOf('Width') >= 0)
		{
			var w = 0;
			if (e.value != '')
				w = parseInt(e.value);
			 
			HTB_IE_specifyValueHeight.value = Math.round(w * HTB_IE_aspectRatio);
		}
		
		else
		{
			var h = 0;
			if (e.value != '')
				h = parseInt(e.value);
			 
			HTB_IE_specifyValueWidth.value = Math.round(h / HTB_IE_aspectRatio);
		}
	}
}

function HTB_InitImageEditor(editorid)
{
	state = eval(editorid + '_State');
	sel = state.GetSelection(editorid);
	if (sel.type == 'Control' && sel.item(0).tagName.toUpperCase() == 'IMG')
	{
		var image = sel.item(0); 
				
		HTB_IE_text.value = image.alt;
		HTB_IE_borderThickness.value = image.border;
		HTB_IE_horizontalSpacing.value = image.hspace;
		HTB_IE_verticalSpacing.value = image.vspace;
		
		if (image.align != null && image.align != '')
		{
			var index = HTB_GetIndexOptionFromValue('HTB_IE_alignment',image.align)
			if (index != -1)
				HTB_IE_alignment.selectedIndex = index;
		}
		else
			HTB_IE_alignment.selectedIndex = 0;

		var w = HTB_GetFromOuterHTML(image.outerHTML,'width');
		var h = HTB_GetFromOuterHTML(image.outerHTML,'height');
		
		if (w != '' || h != '')
		{
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
		 
		if (w != '')
		{
			if (w.indexOf('%') >= 0)
			{
				HTB_IE_specifyInPercentWidth.checked = true;
			}
			else
			{
				HTB_IE_specifyInPixelsWidth.checked = true;
			}
			HTB_IE_specifyValueWidth.value = parseInt(w);
		}
		
		if (h != '')
		{
			if (h.indexOf('%') >= 0)
			{
				HTB_IE_specifyInPercentHeight.checked = true;
			}
			else
			{
				HTB_IE_specifyInPixelsHeight.checked = true;
			}
			HTB_IE_specifyValueHeight.value = parseInt(h);
		}
		HTB_IE_VerifyAspectRationCanBeUsed();
		HTB_SetImageAspectRatio();	
		HTB_IE_cssClass.value = image.className;
		
	}
	
	else
	{

		HTB_IE_picture.value = "";
		HTB_IE_text.value = "";
		
		HTB_IE_alignment.selectedIndex = 0;
		HTB_IE_borderThickness.value = "";
		HTB_IE_horizontalSpacing.value = "0";
		HTB_IE_verticalSpacing.value = "0";

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

function HTB_InsertTable(id, cols, rows, height, width, border, bordercolor,bordercolorlight,bordercolordark, cellspacing, cellpadding, floatalign,align, background, bgcolor, bordercollapse, preview)
{
	var table = '';
	var styleDef = 'BORDER-RIGHT: silver 1px dotted; BORDER-LEFT: silver 1px dotted; BORDER-TOP: silver 1px dotted; BORDER-BOTTOM: silver 1px dotted;';

	if (floatalign != '') 
	{
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
		table += ' bordercolor=' + bordercolor;
		
	if (bgcolor != null && bgcolor != '')
		table += ' bgcolor=' + bgcolor;
		
	if (background != null && background != '')
		table += ' background=' + background;
		
	if (bordercolorlight != null && bordercolorlight != '')
		table += ' bordercolorlight=' + bordercolorlight;
	
	if (bordercolordark != null && bordercolordark != '')
		table += ' bordercolordark=' + bordercolordark
	
	table += ' style="';
	
	table += 'BORDER-COLLAPSE:';
	if (bordercollapse)
	{
		table += 'collapse';
	}
	else
		table += 'separate';
	
	table += ';';
	
	if (preview == 1 && border == 0)
		table += styleDef;
		
	table += '"';	
	
	if (cellspacing)
		table += ' cellspacing=' + cellspacing;
	if (cellpadding)
		table += ' cellpadding=' + cellpadding;
	if (align)
		table += ' align=' + align;
		
	table += '>';
	
	for(index=0;index<rows;index++)
	{
		table += '<tr>';
		for(index2=0;index2<cols;index2++)
		{
			table += '<td';
			if (preview == 1 && border == 0)
				table += ' style="' + styleDef + '"';
			table += '></td>';
		}
		table += '</tr>';
	}
	
	table += '</table>';
	
	if (floatalign != '')
		table += '</div>';

	HTB_SetSnippet(id, table);
}

function HTB_StringReplace(str1, str2, str3)
{
	str1 = str1.split(str2).join(str3);
	return str1;
}

function HTB_SetValue(obj, value)
{
	obj.value = value;
}

function HTB_setSelectedColor(id,color)
{
	var s = '<table style=\"background-color:' + color + ';\" width=\"12\" height=\"12\"><tr><td></td></tr></table>';
	document.getElementById(id + '_selectedColor').value = color;
	ATB_setDropDownListText(id,s);
	ATB_closeDropDownList(id);
}

function HTB_getSelectedColor(id)
{
   return document.getElementById(id + '_selectedColor').value;
}

function HTB_State(id)
{
	this.selection = null
	this.Id = id
	this.RestoreSelection = HTB_State_RestoreSelection
	this.GetSelection = HTB_State_GetSelection
	this.SaveSelection = HTB_State_SaveSelection
}

function HTB_State_RestoreSelection() 
{
	if (this.selection) this.selection.select();
}

function HTB_State_GetSelection(id) 
{
	var sel = this.selection;

	if (!sel) {
		sel = HTB_GetEditor(id).document.selection.createRange();
		sel.type = HTB_GetEditor(id).document.selection.type;
	}
	return sel;
}

function HTB_State_SaveSelection(id) 
{
	state = eval(id + '_State'); 
	/*alert(HTB_GetEditor(id).document.selection.createRange().text);
	alert(HTB_GetEditor(id).document.selection.text);*/
	state.selection = HTB_GetEditor(id).document.selection.createRange();
	state.selection.type = HTB_GetEditor(id).document.selection.type;
}

function HTB_RestoreSelection(id)
{
	state = eval(id + '_State');
	state.RestoreSelection();
}

function HTB_SetColorEditor(id, type, color)
{
	if (type == 'f')
		cmd = 'forecolor';
	else
		cmd = 'backcolor';
		
	HTB_CommandBuilder(id, cmd, color);
}

function HTB_CreateSpecialCharsTable(id)
{
    var item = document.getElementById(id + "_item0");
   	var str = '', td;
	cols = 12;
	charNames = document.getElementById(id + '_specialCharsNames').value.split(',');
	charCodes = document.getElementById(id + '_specialCharsCodes').value.split(',');		
	str += '<table border=\'0\' cellspacing=\'2\' width=\'100%\' height=\'100%\' cellpadding=\'2\' onselectstart=\'return false\'><tr><td>';

	for(var i=0;i<charNames.length;i++)
	{
		td = '<td style=\'cursor:hand\' ' + ' width=\'' + parseInt(100 / cols) + '\' onclick="" onmouseover="HTB_OnColorOver(this)" oumouseout="HTB_OnColorOff(this)">' + charNames[i] + '</td>';
		if ((i) % cols == 0 || i == 0)
			str += '<tr>' + td;
		else if ((i + 1) % cols == 0)
			str += td + '</tr>';
		else
			str += td;
	}
	str += '</td></tr></table>'; 
	item.innerHTML = str;
}

function HTB_FindAndReplace(id,popupid,find,replace,caseSensitive, wholeWord, queryPrompt)
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
}

function HTB_GetArgs(caseSensitive, wholeWord)
{
	var isCaseSensitive = (caseSensitive) ? 4 : 0;
	var isWholeWord = (wholeWord) ? 2 : 0;
	return isCaseSensitive ^ isWholeWord;
}

var undoObject = {origSearchString:"",newRanges :[]};

function HTB_PushUndoNew(rng, findString, replaceString)
{
	undoObject.origSearchString = findString;
	rng.moveStart("character", -replaceString.length);
	undoObject.newRanges[undoObject.newRanges.length] = rng.getBookmark();
}

function HTB_ClearUndoBuffer()
{
	undoObject.origSearchString = "";
	undoObject.newRanges.length = 0;
}

function HTB_UndoReplace()
{
	if (undoObject.newRanges.length && undoObject.origSearchString)
	{
		for (var i = 0;i<underObject.newRanges.length;i++)
		{
			rng.moveToBookmark(undoObject.newRanges[i]);
			rng.text = undoObject.origSearchString;
		}
		HTB_ClearUndoBuffer();
	}
}

function HTB_CreateTableQuick(id, cols, rows)
{
	HTB_CreateTableFull(id, cols, rows);
}

function HTB_CreateTableFull(id, cols, rows, backColor)
{
	/*state = eval(id + '_State');
	sel = state.GetSelection(id);
	if (sel.type == 'Control')
	{
		alert(sel.item(0).outerHTML);
		alert(sel.item(0).tagName);
	}*/
	
	HTB_InsertTable(id, cols, rows, '', '', '0', '', '', '','1','2' ,'', '', '', '', false, true)
}
function HTB_TableBuilderOver(obj, col, row)
{
	HTB_TableBuilderClear();
	var colindex, rowindex;
	for(colindex=0;colindex<=col;colindex++)
	{
		for(rowindex=0;rowindex<=row;rowindex++)
		{
			HTB_SetBackColor(document.getElementById('cell' + colindex + rowindex), '#0A246A');
			document.getElementById('tableInfo').innerText = (rowindex + 1) + ' by ' + (colindex + 1) + ' Table';
		}
	}
}
function HTB_TableBuilderClear()
{

	var colindex, rowindex;
	for(colindex=0;colindex<5;colindex++)
	{
		for(rowindex=0;rowindex<4;rowindex++)
		{
			HTB_SetBackColor(document.getElementById('cell' + colindex + rowindex), '');
			document.getElementById('tableInfo').innerText = 'Cancel';
		}
	}
}

function HTB_OpenTableEditor(popupid,editorid)
{
	HTB_InitTableEditor(editorid);
	ATB_showPopup(popupid);
}

function HTB_SetSnippet(id, code)
{
	HTB_SetFocus(id);
	var state = eval(id + '_State');
	var editable = eval(id + '_State');

	state.RestoreSelection();

	var selection = HTB_GetEditor(id).document.selection.createRange();
	selection.type = HTB_GetEditor(id).document.selection.type;

	if (selection.type == 'Control')
	{
		selection.item(0).outerHTML = code;
	}
	else
	{
		HTB_SetFocus(id);
		selection.pasteHTML(code);
	}
	
	HTB_UpdateLength(id);
}

function HTB_CleanCode(id)
{
	editor = HTB_GetEditor(id);

	// 0bject based cleaning
	var body = editor.document.body;
	for (var index = 0; index < body.all.length; index++) 
	{
		tag = body.all[index];
		tag.removeAttribute("className","",0);
		tag.removeAttribute("style","",0);
	}

	// Regex based cleaning
	var html = editor.document.body.innerHTML;
	html = html.replace(/<o:p>&nbsp;<\/o:p>/g, "");
	html = html.replace(/o:/g, "");
	html = html.replace(/<st1:.*?>/g, "");

	editor.document.body.innerHTML = html;

	HTB_SetFocus(id);
}

function HTB_EnableDisableSpecify(prefix,name)
{
   if (document.getElementById(prefix + '_specify' + name).checked)
   {
   		document.getElementById(prefix + '_specifyValue' + name).disabled = false;
		document.getElementById(prefix + '_specifyInPixels' + name).disabled = false;
	   	document.getElementById(prefix + '_specifyInPercent' + name).disabled = false;
   }
   
   else
   {
      	document.getElementById(prefix + '_specifyValue' + name).disabled = true;
      	document.getElementById(prefix + '_specifyInPixels' + name).disabled = true;
	  	document.getElementById(prefix + '_specifyInPercent' + name).disabled = true;
   }
}

function HTB_EnableDisableBackgroundImage()
{
	if (HTB_TE_useBackgroundPicture.checked)
	{
		HTB_TE_backgroundPictureValue.disabled = false;
	}
	
	else
	{
		HTB_TE_backgroundPictureValue.disabled = true;
	}
}

function HTB_ComposeOption(value, label, style, toCompare)
{
	return  '<option value="' + value + '"' + (style != null ? ' style="' + style + '"' : '')+ '>' + label + '</option>';
}

function HTB_CreateColoredDropDown(id)
{
  var colors =  new Array("Black","Maroon","Olive","Green","Teal","Navy","Purple","White","Silver","Red","Yellow","Lime","Aqua","Blue","Fuchsia");
  var colorsHexa = new Array("#000000","#800000","#808000","#008000","#008080","#000080","#800080","#ffffff","#c0c0c0","#ff0000","#ffff00","#00ff00","#00ffff","#0000ff","#ff00ff");
  var str = '';
 
  str += '<select id=' + id + '>';
  str += '<option value=\"\">---</option>';
 	
  for(index=0;index<colors.length;index++)
  {
    forecolor = ';';
    if (colors[index] == 'Black')
    	forecolor += 'color:White;';
    str += HTB_ComposeOption(colorsHexa[index], colors[index], 'background-color:' + colors[index] + forecolor,''); 
  }
	
  str += '</select>';
  str += '<br>';
  
  return str;
}

function HTB_GetSelectedColoredDropDown(id)
{
 	var ddl = document.getElementById(id); 
 	if (ddl.selectedIndex > 0)
 		return ddl.options[ddl.selectedIndex].value; 
 	else
 		return '';
}

function HTB_GetSelectedOptionText(id)
{
	var ddl = document.getElementById(id);
	return ddl.options[ddl.selectedIndex].text;
}

function HTB_GetSelectedOptionValue(id)
{
	var ddl = document.getElementById(id);
	return ddl.options[ddl.selectedIndex].value;
}

function HTB_GeOptionIndexFromValue(ddl,val)
{
	for (i = 0 ; i < ddl.options.length ; i++)
	{
	   if (ddl.options[i].value == val)
	   	return i;
	}
	
	return -1;
}

function HTB_SelectOptionFromValue(ddl,val)
{
	index = HTB_GeOptionIndexFromValue(ddl,val);

	if (index != -1)
		ddl.selectedIndex = index;
	else
		ddl.selectedIndex = 0;
}

function HTB_GetOptionIndexFromText(ddl,text)
{
	for (i = 0 ; i < ddl.options.length ; i++)
	{
	   if (ddl.options[i].text == text)
	   	return i;
	}
	
	return -1;
}

function HTB_SelectOptionFromText(ddl,text)
{
	index = HTB_GetOptionIndexFromText(ddl,text);
	if (index != -1)
		ddl.selectedIndex = index;
	else
		ddl.selectedIndex = 0;
}

function HTB_GetColoredDropDownIndexFromValue(ddl,val)
{
	var colors =  new Array("Black","Maroon","Olive","Green","Teal","Navy","Purple","White","Silver","Red","Yellow","Lime","Aqua","Blue","Fuchsia");
		
	for (i = 0 ; i < ddl.options.length ; i++)
	{
	   if (ddl.options[i].value == val)
	   	return i;
	}
	
	return -1;
}

function HTB_SelectColoredDropDownFromValue(ddl,val)
{
	index = HTB_GetColoredDropDownIndexFromValue(ddl,val);

	if (index != -1)
		ddl.selectedIndex = index;
	else
		ddl.selectedIndex = 0;
		
}

function HTB_IE_VerifyAspectRationCanBeUsed()
{
	if (((HTB_IE_specifyInPercentWidth.checked && HTB_IE_specifyInPercentHeight.checked) ||
		(HTB_IE_specifyInPixelsWidth.checked && HTB_IE_specifyInPixelsHeight.checked)) &&
		HTB_IE_specifySize.checked == true)
			HTB_IE_keepAspectRatio.disabled = false;
	else
		HTB_IE_keepAspectRatio.disabled = true;
}

function HTB_IE_EnableDisableSpecifySize()
{
	if (HTB_IE_specifySize.checked == true)
	{
		HTB_IE_specifyValueWidth.disabled = false;
		HTB_IE_specifyInPercentWidth.disabled = false;
		HTB_IE_specifyInPixelsWidth.disabled = false;
		
		HTB_IE_specifyValueHeight.disabled = false;
		HTB_IE_specifyInPercentHeight.disabled = false;
		HTB_IE_specifyInPixelsHeight.disabled = false;
		
		HTB_IE_keepAspectRatio.disabled = false;	
	}
	else
	{
		HTB_IE_specifyValueWidth.disabled = true;
		HTB_IE_specifyInPercentWidth.disabled = true;
		HTB_IE_specifyInPixelsWidth.disabled = true;
		HTB_IE_specifyInPixelsWidth.checked = true;
		
		HTB_IE_specifyValueHeight.disabled = true;
		HTB_IE_specifyInPercentHeight.disabled = true;
		HTB_IE_specifyInPixelsHeight.disabled = true;
		HTB_IE_specifyInPixelsHeight.checked = true;		
		
		HTB_IE_keepAspectRatio.disabled = true;
	}
}

function HTB_GetFromOuterHTML(outerHTML,toGet)
{
	var outerHTML = outerHTML.toUpperCase();
	var index = outerHTML.indexOf(toGet.toUpperCase());
	var isFoundEqual = false;
	var startIndex=-1;
	var separator = 'N/A';

	if (index >= 0)
	{
		for (i = index+toGet.length ; i < outerHTML.length; i++)
		{
			
			if (isFoundEqual == true)
			{
				if (startIndex == -1)
				{
					if (outerHTML.charAt(i) != ' ')
					{
						startIndex = i;
						if (outerHTML.charAt(i) == '\"' || outerHTML.charAt(i) == '\'')
						{
							separator = outerHTML.charAt(i); 
						}
					}
				}
				else
				{
					if (separator != 'N/A')
					{
						if (outerHTML.charAt(i) == separator)
							return outerHTML.substring(startIndex+1,i);
					}
					
					else
					{
						if (outerHTML.charAt(i) == ' ')
							return outerHTML.substring(startIndex,i);
					}
				}
			}
			
			else
			{
				if (outerHTML.charAt(i) == '=')
				{
					isFoundEqual = true;
				}
			}
			
		}
		
		return '';
	}
	

	else return '';	
}

function HTB_RemoveFromOuterHTML(outerHTML,toRemove)
{
	var index = outerHTML.toUpperCase().indexOf(toRemove.toUpperCase());
	var isFoundEqual = false;
	var startIndex = -1, startIndexValue = -1;
	var separator = 'N/A'; 

	if (index >= 0)
	{
		startIndex = index;
		for (i = index+toRemove.length; i < outerHTML.length; i++)
		{
			if (isFoundEqual == true)
			{
				if (startIndexValue == -1)
				{
					if (outerHTML.charAt(i) != ' ')
					{
						startIndexValue = i;
						if (outerHTML.charAt(i) == '\"' || outerHTML.charAt(i) == '\'')
						{
							separator = outerHTML.charAt(i); 
						}
					}
				}
				else
				{
					if (separator != 'N/A')
					{
						if (outerHTML.charAt(i) == separator)
						{

							if (startIndex-1 > 0 && outerHTML.charAt(startIndex-1) == ' ')
								startIndex--;
							
							return outerHTML.substring(0,startIndex) + outerHTML.substring(i+1);
						}
					}
					
					else
					{
						if (outerHTML.charAt(i) == ' ' || outerHTML.charAt(i) == '>')
						{
							if (startIndex-1 > 0 && outerHTML.charAt(startIndex-1) == ' ')
								startIndex--;
								
							return outerHTML.substring(0,startIndex) + outerHTML.substring(i);
						}
						
					}
				}
			}
			
			else
			{
				if (outerHTML.charAt(i) == '=')
				{
					isFoundEqual = true;
				}
			}
			
		}
		
		return outerHTML;
	}
	
	else return outerHTML;	
}

function HTB_GetIndexOptionFromValue(id,value)
{
	var ddl = document.getElementById(id);
	if (ddl != null)
	{
		for (i = 0 ; i < ddl.options.length ; i++)
		{
			if (ddl.options[i].value == value)
				return i;
		}
	}
	
	return -1;
}

function HTB_InitLink(editorid)
{
	state = eval(editorid + '_State');
	sel = state.GetSelection(editorid)
	var currentSel = sel;
	if (sel.type != 'Control')
	{
		sel = sel.parentElement();
		if (sel.parentElement && sel.parentElement.tagName.toUpperCase() == 'A')
			sel = sel.parentElement;
		
		if (sel.tagName.toUpperCase() == 'A')
		{
			HTB_L_address.value = sel.href;
			if (sel.innerHTML != '')
				HTB_L_text.value = sel.innerHTML;
			else
				HTB_L_text.value = sel.innerText;
			
			HTB_L_anchor.value = sel.name;
			if (HTB_L_target != '')
			{
				HTB_SelectOptionFromText(HTB_L_preselectedTarget,sel.target);
				HTB_L_target.value = sel.target;
			}
			else
			{
				HTB_L_preselectedTarget.selectedIndex = 0;
				HTB_L_target.value = '';
			}
			
			if (HTB_L_tooltip.value != '')
				HTB_L_tooltip.value = sel.title;
		}
		else
		{
			HTB_ResetLinkValue();
			if (currentSel.type == 'Text')
			{
				if (currentSel != null) 
					HTB_L_text.value = currentSel.text;
			}
		}
	}
	else
		HTB_ResetLinkValue();

}

function HTB_ResetLinkValue()
{
	HTB_L_address.value = '';
	HTB_L_text.value = '';
	HTB_L_anchor.value = '';
	HTB_L_preselectedTarget.selectedIndex = 0;
	HTB_L_target.value = '';
	HTB_L_tooltip.value = '';
}

function HTB_CreateLink(editorid,popupid)
{

	if (HTB_L_address.value == '') return;

	HTB_InsertLink(editorid,HTB_L_address.value,HTB_L_text.value,HTB_L_anchor.value,HTB_L_target.value,HTB_L_tooltip);
	
	ATB_hidePopup(popupid);
}

function HTB_InsertLink(editorid, address, text,anchor,target,tooltip)
{
	var link = '';
	
	link += '<a';
	
	link += ' href=' + address;
	
	if (target != '')
		link += ' target=' + target;
		
	if (anchor != '')
		link += ' name=' + anchor;
	
	if (tooltip != '')
		link += ' title=' + tooltip;
	
	link += '>';
	if (text != '')
		link += text;
	else
		link += address;
	link += '</a>';
	
	HTB_SetSnippet(editorid, link);	
}

function HTB_InsertCustomLink(editorid, customlinkid,address, text,anchor,target, tooltip)
{
	HTB_InsertLink(editorid,address,text,anchor,target,tooltip);
	ATB_closeDropDownList(customlinkid);
}

function HTB_L_ChangeTarget()
{
	if (HTB_GetSelectedOptionValue('HTB_L_preselectedTarget')  == 'reset')
	{
		HTB_L_target.value = '';
		HTB_L_preselectedTarget.selectedIndex = 0;
	}
	else
	{
		HTB_L_target.value = HTB_GetSelectedOptionText('HTB_L_preselectedTarget');
		
	}
}

/*function HTB_IU_RemoveFilesContents()
{
	var table = document.getElementById("HTB_IU_tableContents");
	var tablebody = document.getElementById("HTB_UI_tableContentsBody");
	if (tablebody != null)
		table.removeChild(tablebody);
}*/


function HTB_IU_FillFilesContent(directory)
{
		var div= document.getElementById("HTB_IU_tableContents");
		div.contentWindow.document.body.innerHTML = '';
		var contents = '<table id=\'HTB_IU_tableContents\' border=1">';
		
		var filesHiddenInput = document.getElementById('HTB_IU_dir_' + directory);
		if (filesHiddenInput != null)
			document.getElementById("HTB_IU_curSelDirFullPath").value = filesHiddenInput.name;	
			
		if (filesHiddenInput != null && filesHiddenInput.value != '')
		{
			//div.contentWindow.document.body.innerHTML = 'Loading in progress....';
						
			var reg=new RegExp('[;]+', 'g');
		    var filesInfos = filesHiddenInput.value.split(reg);
		    
		    for(i=0 ; i < filesInfos.length ; i++) 
	        {
				if (filesInfos[i] != null && filesInfos[i].value != '')
				{
					reg=new RegExp('[|]+', 'g');
					fileInfos = filesInfos[i].split(reg);
					
					contents += "<tr onclick=\"parent.HTB_IU_PreviewPicture('" + filesHiddenInput.name + "','" + fileInfos[0] + "'," + fileInfos[3] + "," + fileInfos[4] + ");\">";
					contents += "<td>";
					contents += fileInfos[0];
					contents += "</td>";
					contents += "<td>";
					contents += fileInfos[1];
					contents += "</td>";
					contents += "<td>";
					contents += "<img src='icons/delete.gif' alt='delete' onclick=\"alert('a')\">";
					contents += "</td>";
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

function HTB_ToggleNode(framename,node, base)
{
	var iframe = document.getElementById(framename); 	
	nodeDiv = iframe.contentWindow.document.getElementById(node + '_div');
	nodeImg = iframe.contentWindow.document.getElementById(node + '_img');
	nodeSta = iframe.contentWindow.document.getElementById(node);
	
	if (nodeDiv.style.display == 'block')
	{
		nodeDiv.style.display = 'none';
		nodeImg.src = eval('atv_' + base + '_co');
		nodeSta.value = 'False';	
	}
	else
	{
		nodeDiv.style.display = 'block';
		nodeImg.src = eval('atv_' + base + '_ex');
		nodeSta.value = 'True';
	}
}

function HTB_ReplaceAllOccurence(stringToReplace,oldvalue,newvalue)
{
	var result = stringToReplace.replace(oldvalue,newvalue);
	var ndx = stringToReplace.indexOf(oldvalue);
	if (ndx != -1)	
	{
		return 	HTB_ReplaceAllOccurence(result,oldvalue,newvalue);				
	}
	return result;
}


function HTB_IU_PreviewPicture(directoryName, fileName, width, height)
{
	var completeFile = directoryName + '\\' + fileName;
	
	HTB_IU_preview.src = completeFile;
	
	if (width > 216)
		HTB_IU_preview.width = 216;
	else
		HTB_IU_preview.width = width;
		
	if (height > 144)
		HTB_IU_preview.height = 144;
	else
		HTB_IU_preview.height = height;
}

function HTB_IU_SelectNode(framename,id)
{
	var iframe = document.getElementById(framename); 	
	var base = id.substring(0,id.indexOf('_',0)); 
	if (base == '') 
	{
		// root node
		base = id;
	}
	var curSelNodeID = document.getElementById('atv_' + base + '_curSelNode');
		
	changeStyleSelectedNode(iframe.contentWindow.document.getElementById(id),iframe.contentWindow.document.getElementById(id + '_nodeText'),curSelNodeID,document.getElementById(base + '_curSelNodeStyleOriginal'),iframe.contentWindow.document.getElementById(curSelNodeID.value + '_nodeText'),document.getElementById(base + '_nodesStyleSelected'));
} 

function HTB_InitImageLibrary(editorid)
{
	document.getElementById(editorid + '_HTB_IU_uploadInDirectory').outerHTML = HTB_RemoveFromOuterHTML(document.getElementById(editorid + '_HTB_IU_uploadInDirectory').outerHTML,"value");
	document.getElementById(editorid + '_HTB_IU_preview').width = 0;
	document.getElementById(editorid + '_HTB_IU_preview').height = 0;
	document.getElementById(editorid + '_HTB_IU_text').value = '';
	document.getElementById(editorid + '_HTB_IU_alignment').selectedIndex = 0;
	document.getElementById(editorid + '_HTB_IU_borderThikness').value = '';
	document.getElementById(editorid + '_HTB_IU_horizontalSpacing').value = '0';
	document.getElementById(editorid + '_HTB_IU_verticalSpacing').value = '0';
}

function HTB_IU_AddImage(editorid)
{
	var align = HTB_GetSelectedOptionValue('HTB_IU_alignment');
	if (align == '' || align == 'default')
		align = null;
					
	HTB_AddImage(editorid,HTB_IU_preview.src,HTB_IU_text.value,null,null,HTB_IU_borderThikness.value,HTB_IU_horizontalSpacing.value,HTB_IU_verticalSpacing.value,align,null);
}

function HTB_IU_GetCurrentSelectedDir()
{
	return document.getElementById("HTB_IU_curSelDirFullPath").value;
}

function HTB_UpdateLength(id)
{
	if (eval(id + '_MaxLength'))
	{
		maxLength = eval(id + '_MaxLength');
		editorLength = HTB_GetHtml(id).length;
		if (editorLength > maxLength){
			HTB_SetHtml(id, HTB_GetHtml(id).substring(0, maxLength));
			return false;
		}
	}
	
	return true;
}

function HTB_InitEventHandlers(iframeObj,id) 
{
	iframeObj.frameWindow = document.frames[iframeObj.id];
	
	iframeObj.frameWindow.document.onkeypress = function() 
	{
		return HTB_UpdateLength(id);
	}
	
	iframeObj.frameWindow.document.onkeyup = function () 
	{

		if (iframeObj.frameWindow.event.keyCode == 86 && iframeObj.frameWindow.event.ctrlKey && eval(id + "_CleanOnPaste"))
		{
			try
			{
				HTB_CleanCode(id, false);
			}
			catch (e)
			{
			}
		}
	}
	
	iframeObj.frameWindow.document.onkeydown = function () 
	{
		if (eval(id + '_DisableCtrl') && iframeObj.frameWindow.event.ctrlKey)
			return false;	
				
		if (iframeObj.frameWindow.event.keyCode == 75 && iframeObj.frameWindow.event.ctrlKey)
		{
			if (HTB_GetBR(id).value.toUpperCase() == "TRUE")
				HTB_GetBR(id).value = "false";
				
			else
				HTB_GetBR(id).value = "true";
		}
		
		if (iframeObj.frameWindow.event.keyCode == 13) 
		{
			if (HTB_GetBR(id)) 
			{
					
				HTB_SetFocus(id);
				/*sel = HTB_GetEditor(id).document.selection.createRange(); alert(sel);
				sel.pasteHTML("<BR>");

				sel.select();
				sel.moveEnd("character", 1);
				sel.moveStart("character", 1);
				sel.collapse(false);

				iframeObj.frameWindow.event.cancelBubble = true; 
				iframeObj.frameWindow.event.returnValue = false; 

				return false;*/
			}

		}
	}
}

function HTB_GetBR(id)
{
	return eval(id + '_UseBR');
}
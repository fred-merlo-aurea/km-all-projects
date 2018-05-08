<SCRIPT LANGUAGE="javascript">
var $CLIENT_ID$_MaxLength = $MAX_LENGTH$;
var $CLIENT_ID$_MaxLengthMode = "$MAX_LENGTH_MODE$";
var $CLIENT_ID$_HackProtection = $HACK_PROTECTION$;
var $CLIENT_ID$_CleanOnPaste = $CLEAN_ON_PASTE$; 
var $CLIENT_ID$_UseBR = $USE_BR$;
var $CLIENT_ID$_ImagesDir = '$IMAGESDIR$';
var $CLIENT_ID$_KeysDisabled = "$KEYS_DISABLED$";
var $CLIENT_ID$_StartupFocus = $STARTUP_FOCUS$;
var $CLIENT_ID$_ContentCssFile = "$CONTENTCSSFILE$";
var $CLIENT_ID$_ContextMenuBackColor = "$CONTEXT_MENU_BACK_COLOR$";
var $CLIENT_ID$_ContextMenuBackColorRollOver = "$CONTEXT_MENU_BACK_COLOR_ROLLOVER$";
var $CLIENT_ID$_ContextMenuForeColor = "$CONTEXT_MENU_FORE_COLOR$";
var $CLIENT_ID$_ContextMenuForeColorRollOver = "$CONTEXT_MENU_FORE_COLOR_ROLLOVER$";
var $CLIENT_ID$_ContextMenuCssClass = "$CONTEXT_MENU_CSSCLASS$";
var $CLIENT_ID$_ContextMenuBorderColor = "$CONTEXT_MENU_BORDER_COLOR$";
var $CLIENT_ID$_ContextMenuBorderStyle = "$CONTEXT_MENU_BORDER_STYLE$";
var $CLIENT_ID$_ContextMenuBorderWidth = "$CONTEXT_MENU_BORDER_WIDTH$";
var $CLIENT_ID$_ContextMenuCustom = "$CONTEXT_MENU_CUSTOM$"; 
var $CLIENT_ID$_AutoHideToolBars = "$AUTO_HIDE_TOOLBARS$";
var $CLIENT_ID$_PopupToolBars = "$POPUP_TOOLBARS$";
var $CLIENT_ID$_ScriptDirectory = "$SCRIPT_DIRECTORY$";

try {
				
HTB_InitEditor('$CLIENT_ID$');
var $CLIENT_ID$_State = new HTB_State();

$STARTUPSCRIPT$

} 
catch (awc)
{ 
	
	var scriptDirectoryInfo = '';
	if (HTB_Rtrim($CLIENT_ID$_ScriptDirectory) != '')
	{
		scriptDirectoryInfo = '[' + $CLIENT_ID$_ScriptDirectory + '] directory or change the';
	}
	alert('Could not find script file. Please ensure that the Javascript files are deployed in the ' + scriptDirectoryInfo + 'ScriptDirectory and/or ExternalScript properties to point to the directory where the files are.'); 
}


</SCRIPT>

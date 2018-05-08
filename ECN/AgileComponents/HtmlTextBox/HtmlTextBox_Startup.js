<SCRIPT LANGUAGE="javascript">
HTB_InitEditor('$CLIENT_ID$');
var $CLIENT_ID$_State = new HTB_State();

var iframes = document.all.tags("IFRAME");
for(index=0;index<iframes.length;index++) 
{
	if (iframes[index].id == '$CLIENT_ID$_Editor')
	{
		$CLIENTID$_IFrameObj = iframes[index];
		break;
	}
}
HTB_InitEventHandlers($CLIENTID$_IFrameObj,'$CLIENT_ID$');

var $CLIENT_ID$_DisableCtrl = $CTRL_DISABLED$;
var $CLIENT_ID$_MaxLength = $MAX_LENGTH$;
var $CLIENT_ID$_HackProtection = $HACK_PROTECTION$;
var $CLIENT_ID$_CleanOnPaste = $CLEAN_ON_PASTE$;
var $CLIENT_ID$_UseBR = $USE_BR$;
</SCRIPT>

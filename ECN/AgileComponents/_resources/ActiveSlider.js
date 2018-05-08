var ASD_mustStepPlus=false;var ASD_mustStepMinus=false;var ASD_mustOut=false;var
ASD_mustLiftMove=false;var ASD_outLiftX=-((0x71c+5566-0x19a3)+
(0x191a+4379-0x2083)-(0x1dd1+3429-0x1e4e));var ASD_outLiftY=-(
(0x2385+6673-0x1fe5)+565-8165);var ASD_currentSliderId=null;var ASD_dom=(
document.getElementById)?true:false;var ASD_ns4=(document.layers&&!AU37b7)?true:
false;var
ASD_ns5=((navigator.userAgent.indexOf("Gecko")>-(5045+(0x2591+383-0x1ff4)-6864))
&&ASD_dom)
?true:false;var ASD_ie5=((navigator.userAgent.indexOf("MSIE")>-(4505+
(0x2587+503-0x2219)-5885))&&ASD_dom)?true:false;document.onmousemove=
ASD_MouseMove;document.onmouseup=ASD_MouseUp;function ASD_MoveObjectX(id,AU3288)
{document.getElementById(id).style.left=AU3288;}function ASD_MoveObjectY(id,
AU0671)
{document.getElementById(id).style.top=AU0671;}function ASD_Swap(AU494f,AUfd46)
{document[AU494f].src=AUfd46;}function ASD_FindPosX(AU97cd)
{var AUed49=((0x20e7+1270-0x24bc)+(0xc1c+8959-0x2475)-(0x17d0+561-0xe3a));if(
AU97cd.offsetParent)
{while(AU97cd.offsetParent)
{if(AU97cd.tagName!='DIV')
AUed49+=AU97cd.offsetLeft;AU97cd=AU97cd.offsetParent;}}else if(AU97cd.x)
{AUed49+=AU97cd.x;}return AUed49;}function ASD_FindPosY(AU97cd)
{var AUfa02=((0xe1f+5036-0x2198)+8241-8292);if(AU97cd.offsetParent)
{while(AU97cd.offsetParent)
{if(AU97cd.tagName!='DIV')
{AUfa02+=AU97cd.offsetTop;}AU97cd=AU97cd.offsetParent;}}else if(AU97cd.y)AUfa02
+=AU97cd.y;return AUfa02;}function ASD_MinusArrowDown(id)
{ASD_Swap(id+'_minusArrow',eval(id+'_minusArrowOnImage'));ASD_mustStepMinus=true
;ASD_StepMinus(id);}function ASD_StopStepMinus(id)
{ASD_Swap(id+'_minusArrow',eval(id+'_minusArrowOffImage'));ASD_mustStepMinus=
false;}function ASD_StepMinus(id)
{var AU46e2=document.getElementById(id+'_plusArrow');var AUbeb8=document.
getElementById(id+'_minusArrow');var AUfaa5=document.getElementById(id+'_lift');
var AU2ef1=((0x1837+3493-0x23e7)+(0x2576+7408-0x2043)-9140)/((eval(id+'_max')-
eval(id+'_min'))/eval(id+'_step'));if(eval(id+'_direction').toUpperCase()==
'VERTICAL')
{var AU6fbb=parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight);var
AUf1c4=parseInt(ASD_FindPosY(AU46e2)-parseInt(AUfaa5.clientHeight));var AU8f44=(
(AUf1c4-AU6fbb)*AU2ef1)/((0x1fcf+3416-0x229a)+(0x1b3a+6936-0x1ec2)-8633);var 
AUc3d0=Math.round((parseInt(ASD_FindPosY(AUfaa5))-(parseInt(ASD_FindPosY(AUbeb8)
)+parseInt(AUbeb8.clientHeight)))/AU8f44);AUc3d0--;if(AU6fbb+(AUc3d0*AU8f44)>=
AU6fbb)
{ASD_MoveObjectY(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else if(parseInt(AUfaa5.style.top)!=AU6fbb)
{ASD_MoveObjectY(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}else
{var AU6fbb=parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth);var
AUf1c4=parseInt(ASD_FindPosX(AU46e2)-parseInt(AUfaa5.clientWidth));var AU8f44=((
AUf1c4-AU6fbb)*AU2ef1)/((0x12e6+2921-0xd57)+(0xf41+6875-0x24d5)-
(0x1e93+3561-0x16a1));var AUc3d0=Math.round((parseInt(ASD_FindPosX(AUfaa5))-(
parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth)))/AU8f44);AUc3d0--;
if(AU6fbb+(AUc3d0*AU8f44)>=AU6fbb)
{ASD_MoveObjectX(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else if(parseInt(AUfaa5.style.left)!=AU6fbb)
{ASD_MoveObjectX(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}window.setTimeout("ASD_StepMinusAgain('"+id+"')",eval(id+'_speedArrow'));}
function ASD_StepMinusAgain(id)
{if(ASD_mustStepMinus)
ASD_StepMinus(id);}function ASD_PlusArrowDown(id)
{ASD_Swap(id+'_plusArrow',eval(id+'_plusArrowOnImage'));ASD_mustStepPlus=true;
ASD_StepPlus(id);}function ASD_StopStepPlus(id)
{ASD_Swap(id+'_plusArrow',eval(id+'_plusArrowOffImage'));ASD_mustStepPlus=false;
}function ASD_StepPlus(id)
{var AU46e2=document.getElementById(id+'_plusArrow');var AUbeb8=document.
getElementById(id+'_minusArrow');var AUfaa5=document.getElementById(id+'_lift');
var AU2ef1=((0xc87+2907-0x1289)+(0x9f5+6487-0x1acc)-(0x1944+2421-0x1544))/((eval
(id+'_max')-eval(id+'_min'))/eval(id+'_step'
));if(eval(id+'_direction').toUpperCase()=='VERTICAL')
{var AU6fbb=parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight);var
AUf1c4=parseInt(ASD_FindPosY(AU46e2)-parseInt(AUfaa5.clientHeight));var AU8f44=(
(AUf1c4-AU6fbb)*AU2ef1)/((0x1a5f+1241-0x131f)+(0x1464+4135-0x23a4)-
(0x186c+6640-0x25c0));var AUc3d0=Math.round((parseInt(ASD_FindPosY(AUfaa5))-(
parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight)))/AU8f44);if(AU6fbb
+((AUc3d0+(6407+(0x11eb+1145-0xf87)-(0x26ca+6549-0x207c)))*AU8f44)<AUf1c4)
{AUc3d0++;ASD_MoveObjectY(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue
(id,AU6fbb,AUf1c4,AUfaa5);}else if((AU6fbb+((AUc3d0)*AU8f44))!=AUf1c4)
{ASD_MoveObjectY(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}else
{var AU6fbb=parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth);var
AUf1c4=parseInt(ASD_FindPosX(AU46e2)-parseInt(AUfaa5.clientWidth));var AU8f44=((
AUf1c4-AU6fbb)*AU2ef1)/((0x175a+5045-0x1fad)+(0x1294+3260-0x1688)-5062);var 
AUc3d0=Math.round((parseInt(ASD_FindPosX(AUfaa5))-(parseInt(ASD_FindPosX(AUbeb8)
)+parseInt(AUbeb8.clientWidth)))/AU8f44);if(AU6fbb+((AUc3d0+((0x692+6840-0x1e1d)
+7162-7974))*AU8f44)<AUf1c4)
{AUc3d0++;ASD_MoveObjectX(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue
(id,AU6fbb,AUf1c4,AUfaa5);}else if((AU6fbb+((AUc3d0)*AU8f44))!=AUf1c4)
{ASD_MoveObjectX(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}window.setTimeout("ASD_StepPlusAgain('"+id+"')",eval(id+'_speedArrow'));}
function ASD_StepPlusAgain(id)
{if(ASD_mustStepPlus)
ASD_StepPlus(id);}function ASD_OutLiftDown(id,AU45b5)
{ASD_BackSliderDown(AU45b5,id);ASD_mustOut=true;ASD_OutLift(id);}function
ASD_OutLiftMove()
{ASD_outLiftX=window.event.clientX;ASD_outLiftY=window.event.clientY;}function
ASD_OutLiftStop(id,AU45b5)
{ASD_BackSliderUp(AU45b5,id);ASD_StopLiftAgain();}function ASD_OutLift(id)
{var AU46e2=document.getElementById(id+'_plusArrow');var AUbeb8=document.
getElementById(id+'_minusArrow');var AUfaa5=document.getElementById(id+'_lift');
var AU2ef1=((0x238d+1227-0x1ddd)+(0x1933+2616-0x1052)-(0x2629+4442-0x1a53))/((
eval(id+'_max')-eval(id+'_min'))/eval(id+'_step'));if(eval(id+'_direction').
toUpperCase()=='VERTICAL')
{var AU6fbb=parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight);var
AUf1c4=parseInt(ASD_FindPosY(AU46e2))-parseInt(AUfaa5.clientHeight);var AU8f44=(
(AUf1c4-AU6fbb)*AU2ef1)/((0x1c2b+1025-0x1d4b)+(0xf2+8952-0x23a1)-
(0x893+2251-0xe98));var AUc3d0=Math.round((parseInt(ASD_FindPosY(AUfaa5))-(
parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight)))/AU8f44);if(
ASD_outLiftY==-((0xe23+2425-0xb7d)+(0x1a9a+5731-0x1f61)-(0x2318+5139-0x1971)))
ASD_outLiftY=window.event.clientY;var AU979b=parseInt(ASD_outLiftY)-(parseInt(
ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight)+(AUfaa5.clientHeight/(
(0x1557+4194-0x243a)+(0x1c0c+4164-0x1934)-(0x1ea6+5134-0x1e1b))));var AU020b=
Math.round(AU979b/AU8f44);if(eval(id+'_directStep').toUpperCase()=='TRUE')
{if(AU020b>AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)<AUf1c4)
{ASD_MoveObjectY(id+'_lift',AU6fbb+(AU020b*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectY(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}else if(AU020b<AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)>=AU6fbb)
{ASD_MoveObjectY(id+'_lift',AU6fbb+(AU020b*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectY(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}}else
{if(AU020b>AUc3d0)
{if(AU6fbb+((AUc3d0+((0xa34+8759-0x26b9)+(0xb0a+38-0x861)-(0xe9c+4168-0x1664)))*
AU8f44)<AUf1c4)
{AUc3d0++;ASD_MoveObjectY(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue
(id,AU6fbb,AUf1c4,AUfaa5);}else if((AU6fbb+((AUc3d0)*AU8f44))!=AUf1c4)
{ASD_MoveObjectY(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}window.setTimeout("ASD_OutLiftAgain('"+id+"')",eval(id+'_speedOutLift'));}else
if(AU020b<AUc3d0)
{AUc3d0--;if(AU6fbb+(AUc3d0*AU8f44)>=AU6fbb)
{ASD_MoveObjectY(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else if(parseInt(AUfaa5.style.top)!=AU6fbb)
{ASD_MoveObjectY(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}window.setTimeout("ASD_OutLiftAgain('"+id+"')",eval(id+'_speedOutLift'));}}}
else
{var AU6fbb=parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth);var
AUf1c4=parseInt(ASD_FindPosX(AU46e2))-parseInt(AUfaa5.clientWidth);var AU8f44=((
AUf1c4-AU6fbb)*AU2ef1)/((0x1e37+1237-0x1c32)+(0x14bd+1576-0xdf5)-4966);var 
AUc3d0=Math.round((parseInt(ASD_FindPosX(AUfaa5))-(parseInt(ASD_FindPosX(AUbeb8)
)+parseInt(AUbeb8.clientWidth)))/AU8f44);if(ASD_outLiftX==-(4829+
(0x2385+1613-0x2641)-(0x18ca+7399-0x1f44)))
ASD_outLiftX=window.event.clientX;var AU7ad0=parseInt(ASD_outLiftX)-(parseInt(
ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth)+(AUfaa5.clientWidth/(
(0x1659+4089-0x1b2e)+(0x1394+6564-0x2468)-(0x22a0+4292-0x1f72))));var AU020b=
Math.round(AU7ad0/AU8f44);if(eval(id+'_directStep').toUpperCase()=='TRUE')
{if(AU020b>AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)<AUf1c4)
{ASD_MoveObjectX(id+'_lift',AU6fbb+(AU020b*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectX(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}else if(AU020b<AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)>=AU6fbb)
{ASD_MoveObjectX(id+'_lift',AU6fbb+(AU020b*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectX(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}}}else
{if(AU020b>AUc3d0)
{if(AU6fbb+((AUc3d0+((0x1690+3111-0x1e1e)+(0x1e27+5455-0x1d2c)-6882))*AU8f44)<
AUf1c4)
{AUc3d0++;ASD_MoveObjectX(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue
(id,AU6fbb,AUf1c4,AUfaa5);}else if((AU6fbb+((AUc3d0)*AU8f44))!=AUf1c4)
{ASD_MoveObjectX(id+'_lift',AUf1c4);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}window.setTimeout("ASD_OutLiftAgain('"+id+"')",eval(id+'_speedOutLift'));}else
if(AU020b<AUc3d0)
{AUc3d0--;if(AU6fbb+(AUc3d0*AU8f44)>=AU6fbb)
{ASD_MoveObjectX(id+'_lift',AU6fbb+(AUc3d0*AU8f44));ASD_SetCurrentValue(id,
AU6fbb,AUf1c4,AUfaa5);}else if(parseInt(AUfaa5.style.left)!=AU6fbb)
{ASD_MoveObjectX(id+'_lift',AU6fbb);ASD_SetCurrentValue(id,AU6fbb,AUf1c4,AUfaa5)
;}window.setTimeout("ASD_OutLiftAgain('"+id+"')",eval(id+'_speedOutLift'));}}}}
function ASD_MouseMove(AUf0a1)
{if(ASD_currentSliderId!=null)
{var AU4155=window.event.clientX;var AUd1e7=window.event.clientY;if((ASD_ie5&&
window.event.button==((0x2205+1668-0x1fb8)+(0x1a33+1623-0x774)-8678))||((ASD_ns4
||ASD_ns5)&&ASD_mustLiftMove
))
{var AU46e2=document.getElementById(ASD_currentSliderId+'_plusArrow');var AUbeb8
=document.getElementById(ASD_currentSliderId+'_minusArrow');var AUfaa5=document.
getElementById(ASD_currentSliderId+'_lift');if(eval(ASD_currentSliderId+
'_direction').toUpperCase()=='VERTICAL')
{var AU6fbb=parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight);var
AUf1c4=parseInt(ASD_FindPosY(AU46e2)-parseInt(AUfaa5.clientHeight));if(eval(
ASD_currentSliderId+'_directStep').toUpperCase()=='TRUE')
{var AU2ef1=((0x1f58+1260-0x1522)+(0x1453+5502-0x1efc)-6547)/((eval(
ASD_currentSliderId+'_max')-eval(ASD_currentSliderId+'_min'))/eval(
ASD_currentSliderId+'_step'));var AU8f44=((AUf1c4-AU6fbb)*AU2ef1)/(
(0x1ba3+1071-0x1ed9)+7702-7851);var AUc3d0=Math.round((parseInt(ASD_FindPosY(
AUfaa5))-(parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight)))/AU8f44)
;var AU979b=parseInt(window.event.clientY)-(parseInt(ASD_FindPosY(AUbeb8))+
parseInt(AUbeb8.clientHeight)+(AUfaa5.clientHeight/((0x1d3a+4431-0x144f)+
(0xc71+2515-0xb81)-9467)));var AU020b=Math.round(AU979b/AU8f44);if(AU020b>AUc3d0
)
{if(AU6fbb+(AU020b*AU8f44)<AUf1c4)
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AU6fbb+(AU020b*AU8f44));
ASD_SetCurrentValue(ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AUf1c4);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}else if(AU020b<AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)>=AU6fbb)
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AU6fbb+(AU020b*AU8f44));
ASD_SetCurrentValue(ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AU6fbb);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}}else
{if(AUd1e7-((0x1fb1+6426-0x259a)+(0xbbb+1951-0xb66)-(0x1db8+6374-0x1b83))>AU6fbb
&&AUd1e7-((0x23c0+2684-0x1509)+(0x6fd+5930-0x1c83)-6861)<AUf1c4)
{if(parseInt(AUfaa5.style.top)!=AUd1e7-((0x1c06+754-0x15bf)+5255-
(0x1ee9+4609-0x1334)))
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AUd1e7-((0x16e8+7828-0x211b)+
(0xe4b+3771-0x15a4)-(0x1f65+9019-0x26e7)));ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}else if(AUd1e7-((0x1e57+4201-0x1e67)
+4885-9060)<=AU6fbb)
{if(parseInt(AUfaa5.style.top)!=AU6fbb)
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AU6fbb);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}else if(AUd1e7-((0x12b6+2923-0x1790)
+(0x145a+3527-0x205e)-(0x13b8+4047-0x1b3d))>=AUf1c4)
{if(parseInt(AUfaa5.style.top)!=AUf1c4)
{ASD_MoveObjectY(ASD_currentSliderId+'_lift',AUf1c4);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}}}else
{var AU6fbb=parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth);var
AUf1c4=parseInt(ASD_FindPosX(AU46e2)-parseInt(AUfaa5.clientWidth));if(eval(
ASD_currentSliderId+'_directStep').toUpperCase()=='TRUE')
{var AU2ef1=((0x1c36+4932-0x251f)+(0x1d68+1899-0x1376)-6996)/((eval(
ASD_currentSliderId+'_max')-eval(ASD_currentSliderId+'_min'))/eval(
ASD_currentSliderId+'_step'));var AU8f44=((AUf1c4-AU6fbb)*AU2ef1)/(
(0x1ac0+7171-0x1f2c)+(0x15c4+2443-0x1c5f)-(0x1f4b+4277-0x15dd));var AUc3d0=Math.
round((parseInt(ASD_FindPosX(AUfaa5))-(parseInt(ASD_FindPosX(AUbeb8))+parseInt(
AUbeb8.clientWidth)))/AU8f44);var AU7ad0=parseInt(window.event.clientX)-(
parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth)+(AUfaa5.clientWidth/
((0x1ca3+2706-0x199c)+(0x1966+3239-0x1dbd)-(0x1834+8710-0x2453))));var AU020b=
Math.round(AU7ad0/AU8f44);if(AU020b>AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)<AUf1c4)
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AU6fbb+(AU020b*AU8f44));
ASD_SetCurrentValue(ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AUf1c4);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}else if(AU020b<AUc3d0)
{if(AU6fbb+(AU020b*AU8f44)>=AU6fbb)
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AU6fbb+(AU020b*AU8f44));
ASD_SetCurrentValue(ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}else
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AU6fbb);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}}else
{if(AU4155-((0xa93+1058-0x7ff)+(0x1fac+6618-0x216b)-7879)>AU6fbb&&AU4155-(
(0x189a+4947-0x1e79)+(0x184c+6155-0x1dd7)-8170)<AUf1c4)
{if(parseInt(AUfaa5.style.left)!=AU4155-((0xd33+1017-0x1102)+
(0x1222+3903-0x198c)-(0x1094+3121-0x14d0)))
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AU4155-((0x11f8+1757-0xa43)+
(0x1ad0+2622-0x1207)-8591));ASD_SetCurrentValue(ASD_currentSliderId,AU6fbb,
AUf1c4,AUfaa5);}}else if(AU4155-(4303+(0xbeb+1913-0x945)-6884)<=AU6fbb)
{if(parseInt(AUfaa5.style.left)!=AU6fbb)
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AU6fbb);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}else if(AU4155-((0xc4d+8744-0x2252)+
(0x1543+6750-0x1b78)-8258)>=AUf1c4)
{if(parseInt(AUfaa5.style.left)!=AUf1c4)
{ASD_MoveObjectX(ASD_currentSliderId+'_lift',AUf1c4);ASD_SetCurrentValue(
ASD_currentSliderId,AU6fbb,AUf1c4,AUfaa5);}}}}}}}function ASD_MouseUp(AUf0a1)
{ASD_currentSliderId=null;ASD_mustLiftMove=true;event.cancelBubble=true;}
function ASD_MouseDown(AUf0a1,id)
{ASD_currentSliderId=id;ASD_mustLiftMove=false;AUf0a1.cancelBubble=true;}
function ASD_OutLiftAgain(id)
{if(ASD_mustOut)
ASD_OutLift(id);}function ASD_StopLiftAgain()
{ASD_mustOut=false;ASD_outLiftX=-(9110+(0xd1a+773-0xf26)-9358);ASD_outLiftY=-(
1351+8422-9772);}function ASD_BackSliderDown(AU97cd,id)
{AU97cd.background=eval(id+'_liftOn');}function ASD_BackSliderUp(AU97cd,id)
{AU97cd.background=eval(id+'_liftOff');}function ASD_ExecuteOnValueChanged(id)
{var AU8478=eval(id+'_onValueChanged');if(AU8478!=null&&AU8478!='')
window.setTimeout(AU8478,((0x18ed+5849-0x1f2f)+(0x2059+3601-0x1a3e)-9410));}
function ASD_SetCurrentValue(id,start,AUba09,AUfaa5)
{var min=eval(id+'_min');var max=eval(id+'_max');var AUd9e8=(max-min)/(AUba09-
start);var AU80c0=parseInt(document.getElementById(id+'_currentValue').value);
var newValue=-(4793+(0x19af+803-0x1394)-7158);if(eval(id+'_direction').
toUpperCase()=='VERTICAL')
newValue=Math.round(((parseInt(AUfaa5.style.top)-start)*AUd9e8)+min);else
newValue=Math.round(((parseInt(AUfaa5.style.left)-start)*AUd9e8)+min);if(AU80c0
!=newValue)
{document.getElementById(id+'_currentValue').value=newValue;
ASD_ExecuteOnValueChanged(id);}}function ASD_SetValue(id,value)
{var AU46e2=document.getElementById(id+'_plusArrow');var AUbeb8=document.
getElementById(id+'_minusArrow');var AUfaa5=document.getElementById(id+'_lift');
var min=eval(id+'_min');var max=eval(id+'_max');var AU725c=eval(id+'_step');if(
eval(id+'_direction').toUpperCase()=='VERTICAL')
{var AU6fbb=parseInt(ASD_FindPosY(AUbeb8))+parseInt(AUbeb8.clientHeight);var
AUf1c4=parseInt(ASD_FindPosY(AU46e2)-parseInt(AUfaa5.clientHeight));var AUd9e8=(
AUf1c4-AU6fbb)/(max-min);if(value<parseInt(eval(id+'_min'))||value>parseInt(eval
(id+'_max')))
alert("The value is out the allowed values");else
{if(eval(id+'_directStep').toUpperCase()=='TRUE')
{var AU020b=Math.round(value/AU725c);ASD_MoveObjectY(id+'_lift',AU6fbb+(AU020b*
AU725c*AUd9e8));}else
{ASD_MoveObjectY(id+'_lift',(((value-min)/AU725c)*AUd9e8)+AU6fbb);}}}else
{var AU6fbb=parseInt(ASD_FindPosX(AUbeb8))+parseInt(AUbeb8.clientWidth);var
AUf1c4=parseInt(ASD_FindPosX(AU46e2)-parseInt(AUfaa5.clientWidth));var AUd9e8=(
AUf1c4-AU6fbb)/(max-min);if(value<parseInt(eval(id+'_min'))||value>parseInt(eval
(id+'_max')))
alert("The value is out the allowed values");else
{if(eval(id+'_directStep').toUpperCase()=='TRUE')
{var AU020b=Math.round(value/AU725c);ASD_MoveObjectX(id+'_lift',AU6fbb+(AU020b*
AU725c*AUd9e8));}else
{ASD_MoveObjectX(id+'_lift',(((value-min)/AU725c)*AUd9e8)+AU6fbb);}}}}function
ASD_GetValue(id)
{var AU4dff=document.getElementById(id+'_currentValue');if(AU4dff!=null)
return parseInt(AU4dff.value);else
return-(7365+(0x8a8+486-0x4f5)-(0x2552+1667-0x978));}

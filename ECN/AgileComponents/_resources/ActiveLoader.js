function ALD_GetLoader(){return document.getElementById("ActiveLoader_Layer");};
function ALD_HideLoader(){ALD_GetLoader().style.visibility='hidden';
ALD_GetLoader().style.display='none';ALD_GetLoader().style.zIndex=-10000;};alert
(ALD_GetLoader().style.width);ALD_GetLoader().style.width=document.body.
clientWidth;ALD_GetLoader().style.height=document.body.clientHeight;alert(
ALD_GetLoader().style.width);if(window.addEventListener){window.addEventListener
("load",ALD_HideLoader,false);}else if(window.AU3847){window.AU3847("onload",
ALD_HideLoader);}else{window.onload=ALD_HideLoader;}function ALD_GetLoader(){
return document.getElementById("" AUcb1e "");}function ALD_HideLoader(){
ALD_GetLoader().style.visibility='hidden';ALD_GetLoader().style.display='none';
ALD_GetLoader().style.zIndex=-10000;}function ALD_ShowLoader(){ALD_GetLoader().
style.visibility='visible';ALD_GetLoader().style.display='block';ALD_GetLoader()
.style.zIndex=(0x49c+3262-0x10c4);}ALD_ShowLoader();ALD_GetLoader().style.width=
document.body.clientWidth;ALD_GetLoader().style.height=document.body.
clientHeight;if(window.addEventListener){window.addEventListener("" load "",
ALD_HideLoader,false)}else if(window.AU3847){window.AU3847("" onload "",
ALD_HideLoader)}else{window.onload=ALD_HideLoader}

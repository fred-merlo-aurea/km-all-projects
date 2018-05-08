/* Author: Eric Davis   */
/* Date: 02/20/06       */
/* Copyright ©2006 InterActive Design Solutions */

document.write('<STYLE type="text/css">');
document.write('#menuspan77 { position:absolute; width:950px; }');
document.write('#menutable77 { border-width: 0px; border-color: #808080; border-style:solid}');
document.write('td.menucell77 { cursor:pointer;padding:5px; padding-left:12px; padding-right:4px; background:#FFFFFF; border-left:1px solid #999999; border-bottom:1px solid #999999; border-right:1px solid #999999; text-align:LEFT; }');
document.write('#submenutable77 { background: #000000; border-width: 0px; border-color: #FFFFFF; border-style:solid}');
document.write('td.topcell77 {text-decoration:none; color:#000000; font-weight: 700; font-family: Verdana; font-size: 12px; font-style:normal; text-align:LEFT; }');
document.write('a.topitem77 {text-decoration:none; color:#000000; font-weight: 700; font-family: Verdana; font-size: 12px; font-style:normal; } ');
document.write('a.topitem77:hover {text-decoration:none; color:#FFFFFF; font-weight: 700; font-family: Verdana; font-size: 12px; font-style:normal;}');
document.write('a.subitem77 {text-decoration:none; color:#000000; font-weight: 400; font-family: Verdana; font-size: 11px; font-style:normal; } ');
document.write('a.subitem77:hover {text-decoration:none; color:#000000; font-weight: 400; font-family: Verdana; font-size: 11px; font-style:normal; }');
document.write('a.subitem77:link {text-decoration:none; color:#000000; font-weight: 400; font-family: Verdana; font-size: 11px; font-style:normal; display:block; width:100%;}');
document.write('P.MN77 {margin:0px; color:#000000; font-weight: 700; font-family: Verdana; font-size:12px; font-style:normal;  }');
document.write('P.SMN77 {text-decoration:none; color:#000000; font-weight: 400; font-family: Verdana; font-size:11px; font-style:normal; margin:0px; }');
document.write('#submenu77_0 { position:absolute; left:99px; top:24px; visibility:hidden; }');
document.write('#submenu77_1 { position:absolute; left:187px; top:24px; visibility:hidden; }');
document.write('#submenu77_2 { position:absolute; left:351px; top:24px; visibility:hidden; }');
document.write('#submenu77_3 { position:absolute; left:465px; top:24px; visibility:hidden; }');
document.write('#submenu77_4 { position:absolute; left:566px; top:24px; visibility:hidden; }');
document.write('#submenu77_5 { position:absolute; left:659px; top:24px; visibility:hidden; }');
document.write('</style>');

var thisbrowser77
var hidetimer77 = null;
if(document.layers){ thisbrowser77='NN4'; }
if(document.all){ thisbrowser77='IE'; }
if(!document.all && document.getElementById){ thisbrowser77='NN6'; }
function showmenu77(menuname)
{
if(thisbrowser77=='NN4') document.layers[menuname].visibility = 'visible';
if(thisbrowser77=='IE') document.all[menuname].style.visibility = 'visible';
if(thisbrowser77=='NN6') document.getElementById(menuname).style.visibility = 'visible';
if(hidetimer77) clearTimeout(hidetimer77);}
function timermenu77()
{
if(hidetimer77) clearTimeout(hidetimer77);hidetimer77 = setTimeout("hideall77();",100);
}
function hidemenu77(menuname)
{
if(thisbrowser77=='NN4') document.layers[menuname].visibility = 'hidden';
if(thisbrowser77=='IE') document.all[menuname].style.visibility = 'hidden';
if(thisbrowser77=='NN6') document.getElementById(menuname).style.visibility = 'hidden';
}
function hilite77(menuitem) 
{
if(typeof(currentpage77)!='undefined' && menuitem==currentpage77) return;
if(thisbrowser77=='IE') document.all[menuitem].style.backgroundColor = '#EDEEF8';
if(thisbrowser77=='NN6') document.getElementById(menuitem).style.backgroundColor = '#EDEEF8';
if(hidetimer77) clearTimeout(hidetimer77);}
function unhilite77(menuitem) 
{
if(typeof(currentpage77)!='undefined' && menuitem==currentpage77) return;
if(thisbrowser77=='IE') document.all[menuitem].style.backgroundColor = '#FFFFFF';
if(thisbrowser77=='NN6') document.getElementById(menuitem).style. backgroundColor = '#FFFFFF';
if(hidetimer77) clearTimeout(hidetimer77);hidetimer77 = setTimeout("hideall77();",100);
}
function hideall77()
{
hidemenu77('submenu77_0');
hidemenu77('submenu77_1');
hidemenu77('submenu77_2');
hidemenu77('submenu77_3');
hidemenu77('submenu77_4');
hidemenu77('submenu77_5');
}
function openmenu77(menuname)
{
showmenu77(menuname);
if(menuname!='submenu77_0') hidemenu77('submenu77_0');
if(menuname!='submenu77_1') hidemenu77('submenu77_1');
if(menuname!='submenu77_2') hidemenu77('submenu77_2');
if(menuname!='submenu77_3') hidemenu77('submenu77_3');
if(menuname!='submenu77_4') hidemenu77('submenu77_4');
if(menuname!='submenu77_5') hidemenu77('submenu77_5');
}
function openLink (url)
{
window.location.href = url;
cursor = pointer;
}
document.write('<table width=950 cellspacing=0 border=0 cellpadding=0 align=center>');
document.write('<tr><td align=left valign=top>');
document.write('<span id="menuspan77">');
document.write('<table width=950 id="menutable77" cellspacing=0 cellpadding=0 align=center>');
document.write('<tr>');
document.write('<td class="topcell77" align=LEFT width=125 height=20 >')
document.write('<p class="MN77"><img src="http://www.pharmalive.com/images/09up/ddnav-left-space.jpg"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_0\')" onMouseOut="timermenu77();"><img src="http://www.pharmalive.com/images/BTN_Business.gif" style="cursor:pointer;" alt="Business"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_1\')" onMouseOut="timermenu77();"><img src="http://www.pharmalive.com/images/BTN_marketing_and_Advertising.gif" style="cursor:pointer;" alt="Marketing and Advertising"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_2\')" onMouseOut="timermenu77();"><a href="http://www.therapeuticsdaily.com"><img src="http://www.pharmalive.com/images/BTN_therapeutics.gif" border="0" alt="Therapeutics"></a></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_3\')" onMouseOut="timermenu77();"><img src="http://www.pharmalive.com/images/BTN_R_and_D_News.gif" style="cursor:pointer;" alt="R and D Directions"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_4\')" onMouseOut="timermenu77();"><img src="http://www.pharmalive.com/images/BTN_FDA_News.gif" alt="FDA News" style="cursor:pointer;"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="openmenu77(\'submenu77_5\')" onMouseOut="timermenu77();"><img src="http://www.pharmalive.com/images/BTN_conferences_2.gif" alt="Conferences" style="cursor:pointer;"></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=100 height=20 >')
document.write('<p class="MN77" onMouseOver="hideall77()"><A href="http://www.pharmalive.com//News/Category.cfm?categoryid=33,34"><img src="http://www.pharmalive.com/images/BTN_people.gif" border="0" alt="People"></a></p>');
document.write('</td>');
document.write('<td class="topcell77" align=LEFT width=125 height=20 >')
document.write('<p class="MN77"><img src="http://www.pharmalive.com/images/09up/ddnav-right-space.jpg"></p>');
document.write('</td>');
document.write('</tr>');
document.write('</table><p>');
document.write('<div id="submenu77_0">');
document.write('<table id="submenutable77" width=150 cellspacing=0>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_0_0" onMouseOver="hilite77(\'s77_0_0\')" onMouseOut="unhilite77(\'s77_0_0\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=54" target="_self" title="" class="subitem77" >Business Strategy</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_0_1" onMouseOver="hilite77(\'s77_0_1\')" onMouseOut="unhilite77(\'s77_0_1\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=36,61" target="_self" title="" class="subitem77" >Financial News</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_0_2" onMouseOver="hilite77(\'s77_0_2\')" onMouseOut="unhilite77(\'s77_0_2\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=24" target="_self" title="" class="subitem77" >Generics</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_0_3" onMouseOver="hilite77(\'s77_0_3\')" onMouseOut="unhilite77(\'s77_0_3\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=27" target="_self" title="" class="subitem77" >Legal Actions</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_0_4" onMouseOver="hilite77(\'s77_0_4\')" onMouseOut="unhilite77(\'s77_0_4\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=32" target="_self" title="" class="subitem77" >Patent Issues</a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('<div id="submenu77_1">');
document.write('<table id="submenutable77" width=162 cellspacing=0>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_1_0" onMouseOver="hilite77(\'s77_1_0\')" onMouseOut="unhilite77(\'s77_1_0\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=16" target="_self" title="" class="subitem77" >Agency News</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_1_1" onMouseOver="hilite77(\'s77_1_1\')" onMouseOut="unhilite77(\'s77_1_1\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=22" target="_self" title="" class="subitem77" >Electronic Marketing</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_1_2" onMouseOver="hilite77(\'s77_1_2\')" onMouseOut="unhilite77(\'s77_1_2\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=10" target="_self" title="" class="subitem77" >Product Marketing News</a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('<div id="submenu77_2">');
document.write('<table id="submenutable77" width=180 cellspacing=0>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_2_0" onMouseOver="hilite77(\'s77_2_0\')" onMouseOut="unhilite77(\'s77_2_0\')">');
document.write('<a href="http://www.therapeuticsdaily.com/news/channel.cfm?channelID=26" target="_self" title="" class="subitem77" >Cardiovascular</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_2_1" onMouseOver="hilite77(\'s77_2_1\')" onMouseOut="unhilite77(\'s77_2_1\')">');
document.write('<a href="http://www.therapeuticsdaily.com/news/channel.cfm?channelID=28" target="_self" title="" class="subitem77" >Oncology</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_2_2" onMouseOver="hilite77(\'s77_2_2\')" onMouseOut="unhilite77(\'s77_2_2\')">');
document.write('<a href="http://www.therapeuticsdaily.com/news/channel.cfm?channelID=29" target="_self" title="" class="subitem77" >Pain & Inflammation</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_2_3" onMouseOver="hilite77(\'s77_2_3\')" onMouseOut="unhilite77(\'s77_2_3\')">');
document.write('<a href="http://www.therapeuticsdaily.com/news/channel.cfm?channelID=30" target="_self" title="" class="subitem77" >Central Nervous System</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_2_4" onMouseOver="hilite77(\'s77_2_4\')" onMouseOut="unhilite77(\'s77_2_4\')">');
document.write('<a href="http://www.therapeuticsdaily.com/news/channel.cfm?channelID=31" target="_self" title="" class="subitem77" >Infectious Disease</a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('<div id="submenu77_3">');
document.write('<table id="submenutable77" width=180 cellspacing=0>');
document.write('<tr>');

document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_0" onMouseOver="hilite77(\'s77_3_0\')" onMouseOut="unhilite77(\'s77_3_0\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=20" target="_self" title="" class="subitem77" >Contract Research Business</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_1" onMouseOver="hilite77(\'s77_3_1\')" onMouseOut="unhilite77(\'s77_3_1\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=29" target="_self" title="" class="subitem77" >Drug Approvals</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_2" onMouseOver="hilite77(\'s77_3_2\')" onMouseOut="unhilite77(\'s77_3_2\')">');
document.write('<a href="http://www.pharmalive.com//News/Category.cfm?categoryid=15" target="_self" title="" class="subitem77" >Drug Discovery</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_3" onMouseOver="hilite77(\'s77_3_3\')" onMouseOut="unhilite77(\'s77_3_3\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=21" target="_self" title="" class="subitem77" >Drugs In Development</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_4" onMouseOver="hilite77(\'s77_3_4\')" onMouseOut="unhilite77(\'s77_3_4\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=23" target="_self" title="" class="subitem77" >eR&D</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_5" onMouseOver="hilite77(\'s77_3_5\')" onMouseOut="unhilite77(\'s77_3_5\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=51" target="_self" title="" class="subitem77" >Filed For Approval</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_3_6" onMouseOver="hilite77(\'s77_3_6\')" onMouseOut="unhilite77(\'s77_3_6\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=40" target="_self" title="" class="subitem77" >Trial Results</a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('<div id="submenu77_4">');
document.write('<table id="submenutable77" width=140 cellspacing=0>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_4_0" onMouseOver="hilite77(\'s77_4_0\')" onMouseOut="unhilite77(\'s77_4_0\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=56" target="_self" title="" class="subitem77" >Regulatory Actions</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_4_1" onMouseOver="hilite77(\'s77_4_1\')" onMouseOut="unhilite77(\'s77_4_1\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=57" target="_self" title="" class="subitem77" >Warnings</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_4_2" onMouseOver="hilite77(\'s77_4_2\')" onMouseOut="unhilite77(\'s77_4_2\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=30" target="_self" title="" class="subitem77" >FDA Policy</a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('<div id="submenu77_5">');
document.write('<table id="submenutable77" width=160 cellspacing=0>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_5_0" onMouseOver="hilite77(\'s77_5_0\')" onMouseOut="unhilite77(\'s77_5_0\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=62" target="_self" title="" class="subitem77" >Investment Banking Healthcare</a>')
;document.write('</td>');
document.write('</tr>');
document.write('<tr>');
document.write('<td align=LEFT class="menucell77" height=5 id="s77_5_1" onMouseOver="hilite77(\'s77_5_1\')" onMouseOut="unhilite77(\'s77_5_1\')">');
document.write('<a href="http://www.pharmalive.com/News/Category.cfm?categoryid=63" target="_self" title="" class="subitem77" >Medical/Professional </a>')
;document.write('</td>');
document.write('</tr>');
document.write('</table></div>');
document.write('</span></td></tr></table>');

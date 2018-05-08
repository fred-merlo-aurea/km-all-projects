(function(b,a){a("kendo.calendar",["kendo.core"],b);
}(function(){var a={id:"calendar",name:"Calendar",category:"web",description:"The Calendar widget renders a graphical calendar that supports navigation and selection.",depends:["core"]};
(function(b,au){var L=window.kendo,am=L.support,at=L.ui,ay=at.Widget,N=L.keys,ae=L.parseDate,c=L.date.adjustDST,v=L._extractFormat,an=L.template,A=L.getCulture,ar=L.support.transitions,aq=ar?ar.css+"transform-origin":"",k=an('<td#=data.cssClass# role="gridcell"><a tabindex="-1" class="k-link" href="\\#" data-#=data.ns#value="#=data.dateString#">#=data.value#</a></td>',{useWithBlock:false}),t=an('<td role="gridcell">&nbsp;</td>',{useWithBlock:false}),g=L.support.browser,J=g.msie&&g.version<9,ab=".kendoCalendar",n="click"+ab,M="keydown"+ab,E="id",Q="min",O="left",al="slideIn",R="month",l="century",m="change",Y="navigate",av="value",D="k-state-hover",s="k-state-disabled",y="k-state-focused",ac="k-other-month",ad=' class="'+ac+'"',ap="k-nav-today",j="td:has(.k-link)",f="blur"+ab,w="focus",x=w+ab,S=am.touch?"touchstart":"mouseenter",T=am.touch?"touchstart"+ab:"mouseenter"+ab,U=am.touch?"touchend"+ab+" touchmove"+ab:"mouseleave"+ab,X=60000,W=86400000,af="_prevArrow",Z="_nextArrow",d="aria-disabled",e="aria-selected",ah=b.proxy,u=b.extend,r=Date,ax={month:0,year:1,decade:2,century:3};
var i=ay.extend({init:function(az,aB){var aC=this,aD,aA;
ay.fn.init.call(aC,az,aB);
az=aC.wrapper=aC.element;
aB=aC.options;
aB.url=window.unescape(aB.url);
aC.options.disableDates=B(aC.options.disableDates);
aC._templates();
aC._header();
aC._footer(aC.footer);
aA=az.addClass("k-widget k-calendar").on(T+" "+U,j,V).on(M,"table.k-content",ah(aC._move,aC)).on(n,j,function(aE){var aF=aE.currentTarget.firstChild,aG=aC._toDateObject(aF);
if(aF.href.indexOf("#")!=-1){aE.preventDefault();
}if(aC.options.disableDates(aG)&&aC._view.name=="month"){return;
}aC._click(b(aF));
}).on("mouseup"+ab,"table.k-content, .k-footer",function(){aC._focusView(aC.options.focusOnNav!==false);
}).attr(E);
if(aA){aC._cellID=aA+"_cell_selected";
}aa(aB);
aD=ae(aB.value,aB.format,aB.culture);
aC._index=ax[aB.start];
aC._current=new r(+ai(aD,aB.min,aB.max));
aC._addClassProxy=function(){aC._active=true;
if(aC._cell.hasClass(s)){var aE=aC._view.toDateString(C());
aC._cell=aC._cellByDate(aE);
}aC._cell.addClass(y);
};
aC._removeClassProxy=function(){aC._active=false;
aC._cell.removeClass(y);
};
aC.value(aD);
L.notify(aC);
},options:{name:"Calendar",value:null,min:new r(1900,0,1),max:new r(2099,11,31),dates:[],url:"",culture:"",footer:"",format:"",month:{},start:R,depth:R,animation:{horizontal:{effects:al,reverse:true,duration:500,divisor:2},vertical:{effects:"zoomIn",duration:400}}},events:[m,Y],setOptions:function(az){var aA=this;
aa(az);
az.disableDates=B(az.disableDates);
ay.fn.setOptions.call(aA,az);
aA._templates();
aA._footer(aA.footer);
aA._index=ax[aA.options.start];
aA.navigate();
},destroy:function(){var az=this,aA=az._today;
az.element.off(ab);
az._title.off(ab);
az[af].off(ab);
az[Z].off(ab);
L.destroy(az._table);
if(aA){L.destroy(aA.off(ab));
}ay.fn.destroy.call(az);
},current:function(){return this._current;
},view:function(){return this._view;
},focus:function(az){az=az||this._table;
this._bindTable(az);
az.focus();
},min:function(az){return this._option(Q,az);
},max:function(az){return this._option("max",az);
},navigateToPast:function(){this._navigate(af,-1);
},navigateToFuture:function(){this._navigate(Z,1);
},navigateUp:function(){var aA=this,az=aA._index;
if(aA._title.hasClass(s)){return;
}aA.navigate(aA._current,++az);
},navigateDown:function(aC){var aB=this,aA=aB._index,az=aB.options.depth;
if(!aC){return;
}if(aA===ax[az]){if(!G(aB._value,aB._current)||!G(aB._value,aC)){aB.value(aC);
aB.trigger(m);
}return;
}aB.navigate(aC,--aA);
},navigate:function(aP,aR){aR=isNaN(aR)?ax[aR]:aR;
var aM=this,aJ=aM.options,aA=aJ.culture,aH=aJ.min,aG=aJ.max,aN=aM._title,aE=aM._table,aI=aM._oldTable,aL=aM._value,aB=aM._current,aF=aP&&+aP>+aB,aQ=aR!==au&&aR!==aM._index,aO,aC,az,aD;
if(!aP){aP=aB;
}aM._current=aP=new r(+ai(aP,aH,aG));
if(aR===au){aR=aM._index;
}else{aM._index=aR;
}aM._view=aC=h.views[aR];
az=aC.compare;
aD=aR===ax[l];
aN.toggleClass(s,aD).attr(d,aD);
aD=az(aP,aH)<1;
aM[af].toggleClass(s,aD).attr(d,aD);
aD=az(aP,aG)>-1;
aM[Z].toggleClass(s,aD).attr(d,aD);
if(aE&&aI&&aI.data("animating")){aI.kendoStop(true,true);
aE.kendoStop(true,true);
}aM._oldTable=aE;
if(!aE||aM._changeView){aN.html(aC.title(aP,aH,aG,aA));
aM._table=aO=b(aC.content(u({min:aH,max:aG,date:aP,url:aJ.url,dates:aJ.dates,format:aJ.format,culture:aA,disableDates:aJ.disableDates},aM[aC.name])));
P(aO);
var aK=aE&&aE.data("start")===aO.data("start");
aM._animate({from:aE,to:aO,vertical:aQ,future:aF,replace:aK});
aM.trigger(Y);
aM._focus(aP);
}if(aR===ax[aJ.depth]&&aL&&!aM.options.disableDates(aL)){aM._class("k-state-selected",aL);
}aM._class(y,aP);
if(!aE&&aM._cell){aM._cell.removeClass(y);
}aM._changeView=true;
},value:function(aE){var aD=this,aF=aD._view,aC=aD.options,aB=aD._view,aA=aC.min,az=aC.max;
if(aE===au){return aD._value;
}if(aE===null){aD._current=new Date(aD._current.getFullYear(),aD._current.getMonth(),aD._current.getDate());
}aE=ae(aE,aC.format,aC.culture);
if(aE!==null){aE=new r(+aE);
if(!K(aE,aA,az)){aE=null;
}}if(aE===null||!aD.options.disableDates(aE)){aD._value=aE;
}else{if(aD._value===au){aD._value=null;
}}if(aB&&aE===null&&aD._cell){aD._cell.removeClass("k-state-selected");
}else{aD._changeView=!aE||aF&&aF.compare(aE,aD._current)!==0;
aD.navigate(aE);
}},_move:function(aA){var aL=this,aI=aL.options,aE=aA.keyCode,aN=aL._view,aB=aL._index,aH=aL.options.min,aF=aL.options.max,az=new r(+aL._current),aD=L.support.isRtl(aL.wrapper),aC=aL.options.disableDates,aM,aJ,aG,aK;
if(aA.target===aL._table[0]){aL._active=true;
}if(aA.ctrlKey){if(aE==N.RIGHT&&!aD||aE==N.LEFT&&aD){aL.navigateToFuture();
aJ=true;
}else{if(aE==N.LEFT&&!aD||aE==N.RIGHT&&aD){aL.navigateToPast();
aJ=true;
}else{if(aE==N.UP){aL.navigateUp();
aJ=true;
}else{if(aE==N.DOWN){aL._click(b(aL._cell[0].firstChild));
aJ=true;
}}}}}else{if(aE==N.RIGHT&&!aD||aE==N.LEFT&&aD){aM=1;
aJ=true;
}else{if(aE==N.LEFT&&!aD||aE==N.RIGHT&&aD){aM=-1;
aJ=true;
}else{if(aE==N.UP){aM=aB===0?-7:-4;
aJ=true;
}else{if(aE==N.DOWN){aM=aB===0?7:4;
aJ=true;
}else{if(aE==N.ENTER){aL._click(b(aL._cell[0].firstChild));
aJ=true;
}else{if(aE==N.HOME||aE==N.END){aG=aE==N.HOME?"first":"last";
aK=aN[aG](az);
az=new r(aK.getFullYear(),aK.getMonth(),aK.getDate(),az.getHours(),az.getMinutes(),az.getSeconds(),az.getMilliseconds());
aJ=true;
}else{if(aE==N.PAGEUP){aJ=true;
aL.navigateToPast();
}else{if(aE==N.PAGEDOWN){aJ=true;
aL.navigateToFuture();
}}}}}}}}if(aM||aG){if(!aG){aN.setDate(az,aM);
}if(aC(az)){az=aL._nextNavigatable(az,aM);
}if(K(az,aH,aF)){aL._focus(ai(az,aI.min,aI.max));
}}}if(aJ){aA.preventDefault();
}return aL._current;
},_nextNavigatable:function(az,aG){var aF=this,aA=true,aH=aF._view,aD=aF.options.min,aC=aF.options.max,aB=aF.options.disableDates,aE=new Date(az.getTime());
aH.setDate(aE,-aG);
while(aA){aH.setDate(az,aG);
if(!K(az,aD,aC)){az=aE;
break;
}aA=aB(az);
}return az;
},_animate:function(aB){var aC=this,aA=aB.from,aD=aB.to,az=aC._active;
if(!aA){aD.insertAfter(aC.element[0].firstChild);
aC._bindTable(aD);
}else{if(aA.parent().data("animating")){aA.off(ab);
aA.parent().kendoStop(true,true).remove();
aA.remove();
aD.insertAfter(aC.element[0].firstChild);
aC._focusView(az);
}else{if(!aA.is(":visible")||aC.options.animation===false||aB.replace){aD.insertAfter(aA);
aA.off(ab).remove();
aC._focusView(az);
}else{aC[aB.vertical?"_vertical":"_horizontal"](aA,aD,aB.future);
}}}},_horizontal:function(aB,aF,aC){var aE=this,az=aE._active,aD=aE.options.animation.horizontal,aA=aD.effects,aG=aB.outerWidth();
if(aA&&aA.indexOf(al)!=-1){aB.add(aF).css({width:aG});
aB.wrap("<div/>");
aE._focusView(az,aB);
aB.parent().css({position:"relative",width:aG*2,"float":O,"margin-left":aC?0:-aG});
aF[aC?"insertAfter":"insertBefore"](aB);
u(aD,{effects:al+":"+(aC?"right":O),complete:function(){aB.off(ab).remove();
aE._oldTable=null;
aF.unwrap();
aE._focusView(az);
}});
aB.parent().kendoStop(true,true).kendoAnimate(aD);
}},_vertical:function(aC,aF){var aE=this,aG=aE.options.animation.vertical,aB=aG.effects,az=aE._active,aA,aD;
if(aB&&aB.indexOf("zoom")!=-1){aF.css({position:"absolute",top:aC.prev().outerHeight(),left:0}).insertBefore(aC);
if(aq){aA=aE._cellByDate(aE._view.toDateString(aE._current));
aD=aA.position();
aD=aD.left+parseInt(aA.width()/2,10)+"px "+(aD.top+parseInt(aA.height()/2,10)+"px");
aF.css(aq,aD);
}aC.kendoStop(true,true).kendoAnimate({effects:"fadeOut",duration:600,complete:function(){aC.off(ab).remove();
aE._oldTable=null;
aF.css({position:"static",top:0,left:0});
aE._focusView(az);
}});
aF.kendoStop(true,true).kendoAnimate(aG);
}},_cellByDate:function(az){return this._table.find("td:not(."+ac+")").filter(function(){return b(this.firstChild).attr(L.attr(av))===az;
});
},_class:function(aA,aB){var aE=this,aD=aE._cellID,az=aE._cell,aF=aE._view.toDateString(aB),aC;
if(az){az.removeAttr(e).removeAttr("aria-label").removeAttr(E);
}if(aB){aC=aE.options.disableDates(aB);
}az=aE._table.find("td:not(."+ac+")").removeClass(aA).filter(function(){return b(this.firstChild).attr(L.attr(av))===aF;
}).attr(e,true);
if(aA===y&&!aE._active&&aE.options.focusOnNav!==false||aC){aA="";
}az.addClass(aA);
if(az[0]){aE._cell=az;
}if(aD){az.attr(E,aD);
aE._table.removeAttr("aria-activedescendant").attr("aria-activedescendant",aD);
}},_bindTable:function(az){az.on(x,this._addClassProxy).on(f,this._removeClassProxy);
},_click:function(aA){var aC=this,aB=aC.options,az=new Date(+aC._current),aD=aC._toDateObject(aA);
c(aD,0);
if(aC.options.disableDates(aD)&&aC._view.name=="month"){aD=aC._value;
}aC._view.setDate(az,aD);
aC.navigateDown(ai(az,aB.min,aB.max));
},_focus:function(aA){var az=this,aB=az._view;
if(aB.compare(aA,az._current)!==0){az.navigate(aA);
}else{az._current=aA;
az._class(y,aA);
}},_focusView:function(az,aA){if(az){this.focus(aA);
}},_footer:function(aB){var aC=this,aD=C(),az=aC.element,aA=az.find(".k-footer");
if(!aB){aC._toggle(false);
aA.hide();
return;
}if(!aA[0]){aA=b('<div class="k-footer"><a href="#" class="k-link k-nav-today"></a></div>').appendTo(az);
}aC._today=aA.show().find(".k-link").html(aB(aD)).attr("title",L.toString(aD,"D",aC.options.culture));
aC._toggle();
},_header:function(){var aB=this,az=aB.element,aA;
if(!az.find(".k-header")[0]){az.html('<div class="k-header"><a href="#" role="button" class="k-link k-nav-prev"><span class="k-icon k-i-arrow-w"></span></a><a href="#" role="button" aria-live="assertive" aria-atomic="true" class="k-link k-nav-fast"></a><a href="#" role="button" class="k-link k-nav-next"><span class="k-icon k-i-arrow-e"></span></a></div>');
}aA=az.find(".k-link").on(T+" "+U+" "+x+" "+f,V).click(false);
aB._title=aA.eq(1).on(n,function(){aB._active=aB.options.focusOnNav!==false;
aB.navigateUp();
});
aB[af]=aA.eq(0).on(n,function(){aB._active=aB.options.focusOnNav!==false;
aB.navigateToPast();
});
aB[Z]=aA.eq(2).on(n,function(){aB._active=aB.options.focusOnNav!==false;
aB.navigateToFuture();
});
},_navigate:function(az,aC){var aD=this,aB=aD._index+1,aA=new r(+aD._current);
az=aD[az];
if(!az.hasClass(s)){if(aB>3){aA.setFullYear(aA.getFullYear()+100*aC);
}else{h.views[aB].setDate(aA,aC);
}aD.navigate(aA);
}},_option:function(aB,aE){var aD=this,aC=aD.options,az=aD._value||aD._current,aA;
if(aE===au){return aC[aB];
}aE=ae(aE,aC.format,aC.culture);
if(!aE){return;
}aC[aB]=new r(+aE);
if(aB===Q){aA=aE>az;
}else{aA=az>aE;
}if(aA||I(az,aE)){if(aA){aD._value=null;
}aD._changeView=true;
}if(!aD._changeView){aD._changeView=!!(aC.month.content||aC.month.empty);
}aD.navigate(aD._value);
aD._toggle();
},_toggle:function(aD){var aC=this,aB=aC.options,az=aC.options.disableDates(C()),aA=aC._today;
if(aD===au){aD=K(C(),aB.min,aB.max);
}if(aA){aA.off(n);
if(aD&&!az){aA.addClass(ap).removeClass(s).on(n,ah(aC._todayClick,aC));
}else{aA.removeClass(ap).addClass(s).on(n,ag);
}}},_todayClick:function(aB){var aC=this,az=ax[aC.options.depth],aA=aC.options.disableDates,aD=C();
aB.preventDefault();
if(aA(aD)){return;
}if(aC._view.compare(aC._current,aD)===0&&aC._index==az){aC._changeView=false;
}aC._value=aD;
aC.navigate(aD,az);
aC.trigger(m);
},_toDateObject:function(az){var aA=b(az).attr(L.attr(av)).split("/");
aA=new r(aA[0],aA[1],aA[2]);
return aA;
},_templates:function(){var aE=this,aD=aE.options,aB=aD.footer,aC=aD.month,az=aC.content,aA=aC.empty;
aE.month={content:an('<td#=data.cssClass# role="gridcell"><a tabindex="-1" class="k-link#=data.linkClass#" href="#=data.url#" '+L.attr("value")+'="#=data.dateString#" title="#=data.title#">'+(az||"#=data.value#")+"</a></td>",{useWithBlock:!!az}),empty:an('<td role="gridcell">'+(aA||"&nbsp;")+"</td>",{useWithBlock:!!aA})};
aE.footer=aB!==false?an(aB||'#= kendo.toString(data,"D","'+aD.culture+'") #',{useWithBlock:false}):null;
}});
at.plugin(i);
var h={firstDayOfMonth:function(az){return new r(az.getFullYear(),az.getMonth(),1);
},firstVisibleDay:function(aA,az){az=az||L.culture().calendar;
var aB=az.firstDay,aC=new r(aA.getFullYear(),aA.getMonth(),0,aA.getHours(),aA.getMinutes(),aA.getSeconds(),aA.getMilliseconds());
while(aC.getDay()!=aB){h.setTime(aC,-1*W);
}return aC;
},setTime:function(az,aB){var aC=az.getTimezoneOffset(),aA=new r(az.getTime()+aB),aD=aA.getTimezoneOffset()-aC;
az.setTime(aA.getTime()+aD*X);
},views:[{name:R,title:function(aA,aC,aB,az){return z(az).months.names[aA.getMonth()]+" "+aA.getFullYear();
},content:function(aP){var aS=this,aJ=0,aM=aP.min,aL=aP.max,aB=aP.date,aC=aP.dates,aG=aP.format,az=aP.culture,aO=aP.url,aH=aO&&aC[0],aA=z(az),aE=aA.firstDay,aD=aA.days,aN=ak(aD.names,aE),aQ=ak(aD.namesShort,aE),aR=h.firstVisibleDay(aB,aA),aF=aS.first(aB),aK=aS.last(aB),aT=aS.toDateString,aU=new r(),aI='<table tabindex="0" role="grid" class="k-content" cellspacing="0" data-start="'+aT(aR)+'"><thead><tr role="row">';
for(;
aJ<7;
aJ++){aI+='<th scope="col" title="'+aN[aJ]+'">'+aQ[aJ]+"</th>";
}aU=new r(aU.getFullYear(),aU.getMonth(),aU.getDate());
c(aU,0);
aU=+aU;
return aw({cells:42,perRow:7,html:aI+='</tr></thead><tbody><tr role="row">',start:aR,min:new r(aM.getFullYear(),aM.getMonth(),aM.getDate()),max:new r(aL.getFullYear(),aL.getMonth(),aL.getDate()),content:aP.content,empty:aP.empty,setter:aS.setDate,disableDates:aP.disableDates,build:function(aW,aZ,aY){var aV=[],aX=aW.getDay(),a0="",a1="#";
if(aW<aF||aW>aK){aV.push(ac);
}if(aY(aW)){aV.push(s);
}if(+aW===aU){aV.push("k-today");
}if(aX===0||aX===6){aV.push("k-weekend");
}if(aH&&F(+aW,aC)){a1=aO.replace("{0}",L.toString(aW,aG,az));
a0=" k-action-link";
}return{date:aW,dates:aC,ns:L.ns,title:L.toString(aW,"D",az),value:aW.getDate(),dateString:aT(aW),cssClass:aV[0]?' class="'+aV.join(" ")+'"':"",linkClass:a0,url:a1};
}});
},first:function(az){return h.firstDayOfMonth(az);
},last:function(az){var aB=new r(az.getFullYear(),az.getMonth()+1,0),aA=h.firstDayOfMonth(az),aC=Math.abs(aB.getTimezoneOffset()-aA.getTimezoneOffset());
if(aC){aB.setHours(aA.getHours()+aC/60);
}return aB;
},compare:function(az,aA){var aD,aB=az.getMonth(),aE=az.getFullYear(),aC=aA.getMonth(),aF=aA.getFullYear();
if(aE>aF){aD=1;
}else{if(aE<aF){aD=-1;
}else{aD=aB==aC?0:aB>aC?1:-1;
}}return aD;
},setDate:function(az,aB){var aA=az.getHours();
if(aB instanceof r){az.setFullYear(aB.getFullYear(),aB.getMonth(),aB.getDate());
}else{h.setTime(az,aB*W);
}c(az,aA);
},toDateString:function(az){return az.getFullYear()+"/"+az.getMonth()+"/"+az.getDate();
}},{name:"year",title:function(az){return az.getFullYear();
},content:function(aC){var aB=z(aC.culture).months.namesAbbr,aD=this.toDateString,aA=aC.min,az=aC.max;
return aw({min:new r(aA.getFullYear(),aA.getMonth(),1),max:new r(az.getFullYear(),az.getMonth(),1),start:new r(aC.date.getFullYear(),0,1),setter:this.setDate,build:function(aE){return{value:aB[aE.getMonth()],ns:L.ns,dateString:aD(aE),cssClass:""};
}});
},first:function(az){return new r(az.getFullYear(),0,az.getDate());
},last:function(az){return new r(az.getFullYear(),11,az.getDate());
},compare:function(az,aA){return o(az,aA);
},setDate:function(az,aC){var aB,aA=az.getHours();
if(aC instanceof r){aB=aC.getMonth();
az.setFullYear(aC.getFullYear(),aB,az.getDate());
if(aB!==az.getMonth()){az.setDate(0);
}}else{aB=az.getMonth()+aC;
az.setMonth(aB);
if(aB>11){aB-=12;
}if(aB>0&&az.getMonth()!=aB){az.setDate(0);
}}c(az,aA);
},toDateString:function(az){return az.getFullYear()+"/"+az.getMonth()+"/1";
}},{name:"decade",title:function(az,aB,aA){return ao(az,aB,aA,10);
},content:function(az){var aB=az.date.getFullYear(),aA=this.toDateString;
return aw({start:new r(aB-aB%10-1,0,1),min:new r(az.min.getFullYear(),0,1),max:new r(az.max.getFullYear(),0,1),setter:this.setDate,build:function(aC,aD){return{value:aC.getFullYear(),ns:L.ns,dateString:aA(aC),cssClass:aD===0||aD==11?ad:""};
}});
},first:function(az){var aA=az.getFullYear();
return new r(aA-aA%10,az.getMonth(),az.getDate());
},last:function(az){var aA=az.getFullYear();
return new r(aA-aA%10+9,az.getMonth(),az.getDate());
},compare:function(az,aA){return o(az,aA,10);
},setDate:function(az,aA){aj(az,aA,1);
},toDateString:function(az){return az.getFullYear()+"/0/1";
}},{name:l,title:function(az,aB,aA){return ao(az,aB,aA,100);
},content:function(aD){var aF=aD.date.getFullYear(),aB=aD.min.getFullYear(),az=aD.max.getFullYear(),aE=this.toDateString,aC=aB,aA=az;
aC=aC-aC%10;
aA=aA-aA%10;
if(aA-aC<10){aA=aC+9;
}return aw({start:new r(aF-aF%100-10,0,1),min:new r(aC,0,1),max:new r(aA,0,1),setter:this.setDate,build:function(aG,aI){var aJ=aG.getFullYear(),aH=aJ+9;
if(aJ<aB){aJ=aB;
}if(aH>az){aH=az;
}return{ns:L.ns,value:aJ+" - "+aH,dateString:aE(aG),cssClass:aI===0||aI==11?ad:""};
}});
},first:function(az){var aA=az.getFullYear();
return new r(aA-aA%100,az.getMonth(),az.getDate());
},last:function(az){var aA=az.getFullYear();
return new r(aA-aA%100+99,az.getMonth(),az.getDate());
},compare:function(az,aA){return o(az,aA,100);
},setDate:function(az,aA){aj(az,aA,10);
},toDateString:function(az){var aA=az.getFullYear();
return aA-aA%10+"/0/1";
}}]};
function ao(az,aD,aB,aF){var aG=az.getFullYear(),aE=aD.getFullYear(),aC=aB.getFullYear(),aA;
aG=aG-aG%aF;
aA=aG+(aF-1);
if(aG<aE){aG=aE;
}if(aA>aC){aA=aC;
}return aG+"-"+aA;
}function aw(aJ){var aF=0,aC,aI=aJ.min,aH=aJ.max,aL=aJ.start,aK=aJ.setter,az=aJ.build,aG=aJ.cells||12,aA=aJ.perRow||4,aB=aJ.content||k,aD=aJ.empty||t,aE=aJ.html||'<table tabindex="0" role="grid" class="k-content k-meta-view" cellspacing="0"><tbody><tr role="row">';
for(;
aF<aG;
aF++){if(aF>0&&aF%aA===0){aE+='</tr><tr role="row">';
}aL=new r(aL.getFullYear(),aL.getMonth(),aL.getDate(),0,0,0);
c(aL,0);
aC=az(aL,aF,aJ.disableDates);
aE+=K(aL,aI,aH)?aB(aC):aD(aC);
aK(aL,1);
}return aE+"</tr></tbody></table>";
}function o(az,aA,aC){var aF=az.getFullYear(),aE=aA.getFullYear(),aB=aE,aD=0;
if(aC){aE=aE-aE%aC;
aB=aE-aE%aC+aC-1;
}if(aF>aB){aD=1;
}else{if(aF<aE){aD=-1;
}}return aD;
}function C(){var az=new r();
return new r(az.getFullYear(),az.getMonth(),az.getDate());
}function ai(aC,aA,az){var aB=C();
if(aC){aB=new r(+aC);
}if(aA>aB){aB=new r(+aA);
}else{if(az<aB){aB=new r(+az);
}}return aB;
}function K(az,aB,aA){return +az>=+aB&&+az<=+aA;
}function ak(az,aA){return az.slice(aA).concat(az.slice(0,aA));
}function aj(az,aB,aA){aB=aB instanceof r?aB.getFullYear():az.getFullYear()+aA*aB;
az.setFullYear(aB);
}function V(aA){var az=b(this).hasClass("k-state-disabled");
if(!az){b(this).toggleClass(D,S.indexOf(aA.type)>-1||aA.type==w);
}}function ag(az){az.preventDefault();
}function z(az){return A(az).calendars.standard;
}function aa(aB){var aC=ax[aB.start],aA=ax[aB.depth],az=A(aB.culture);
aB.format=v(aB.format||az.calendars.standard.patterns.d);
if(isNaN(aC)){aC=0;
aB.start=R;
}if(aA===au||aA>aC){aB.depth=R;
}if(aB.dates===null){aB.dates=[];
}}function P(az){if(J){az.find("*").attr("unselectable","on");
}}function F(az,aA){for(var aB=0,aC=aA.length;
aB<aC;
aB++){if(az===+aA[aB]){return true;
}}return false;
}function H(az,aA){if(az){return az.getFullYear()===aA.getFullYear()&&az.getMonth()===aA.getMonth()&&az.getDate()===aA.getDate();
}return false;
}function I(az,aA){if(az){return az.getFullYear()===aA.getFullYear()&&az.getMonth()===aA.getMonth();
}return false;
}function B(az){if(L.isFunction(az)){return az;
}if(b.isArray(az)){return q(az);
}return b.noop;
}function p(az){var aB=[];
for(var aA=0;
aA<az.length;
aA++){aB.push(az[aA].setHours(0,0,0,0));
}return aB;
}function q(aB){var az,aA,aE=[],aD=["su","mo","tu","we","th","fr","sa"],aH="if (found) { return true } else {return false}";
if(aB[0] instanceof r){aE=p(aB);
az="var found = date && $.inArray(date.setHours(0, 0, 0, 0),["+aE+"]) > -1;"+aH;
}else{for(var aF=0;
aF<aB.length;
aF++){var aC=aB[aF].slice(0,2).toLowerCase();
var aG=b.inArray(aC,aD);
if(aG>-1){aE.push(aG);
}}az="var found = date && $.inArray(date.getDay(),["+aE+"]) > -1;"+aH;
}aA=new Function("date",az);
return aA;
}function G(aA,az){if(aA instanceof Date&&az instanceof Date){aA=aA.getTime();
az=az.getTime();
}return aA===az;
}h.isEqualDatePart=H;
h.isEqualDate=G;
h.makeUnselectable=P;
h.restrictValue=ai;
h.isInRange=K;
h.normalize=aa;
h.viewsEnum=ax;
h.disabled=B;
L.calendar=h;
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

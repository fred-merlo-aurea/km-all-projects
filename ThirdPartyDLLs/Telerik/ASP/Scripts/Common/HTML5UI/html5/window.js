(function(b,a){a("kendo.window",["kendo.draganddrop"],b);
}(function(){var a={id:"window",name:"Window",category:"web",description:"The Window widget displays content in a modal or non-modal HTML window.",depends:["draganddrop"]};
(function(b,X){var v=window.kendo,Z=v.ui.Widget,l=v.ui.Draggable,t=b.isPlainObject,d=v._activeElement,P=b.proxy,q=b.extend,n=b.each,U=v.template,e="body",V,L=".kendoWindow",B=".k-window",E=".k-window-title",F=E+"bar",C=".k-window-content",D=".k-resize-handle",y=".k-overlay",u="k-content-frame",G="k-i-loading",x="k-state-hover",w="k-state-focused",I="k-window-maximized",Y=":visible",r="hidden",h="cursor",M="open",c="activate",i="deactivate",f="close",Q="refresh",J="minimize",H="maximize",S="resize",T="resizeEnd",m="dragstart",k="dragend",o="error",N="overflow",ad="zIndex",K=".k-window-actions .k-i-minimize,.k-window-actions .k-i-maximize",z=".k-i-pin",A=".k-i-unpin",O=z+","+A,W=".k-window-titlebar .k-window-action",R=".k-window-titlebar .k-i-refresh",s=v.isLocalUrl;
function j(ae){return typeof ae!="undefined";
}function g(ag,af,ae){return Math.max(Math.min(parseInt(ag,10),ae===Infinity?ae:parseInt(ae,10)),parseInt(af,10));
}function p(){return !this.type||this.type.toLowerCase().indexOf("script")>=0;
}var aa=Z.extend({init:function(ag,ak){var an=this,aq,aj={},ao,af,al,ai=false,ae,ap,am=ak&&ak.actions&&!ak.actions.length,ah;
Z.fn.init.call(an,ag,ak);
ak=an.options;
al=ak.position;
ag=an.element;
ae=ak.content;
if(am){ak.actions=[];
}an.appendTo=b(ak.appendTo);
if(ae&&!t(ae)){ae=ak.content={url:ae};
}ag.find("script").filter(p).remove();
if(!ag.parent().is(an.appendTo)&&(al.top===X||al.left===X)){if(ag.is(Y)){aj=ag.offset();
ai=true;
}else{ao=ag.css("visibility");
af=ag.css("display");
ag.css({visibility:r,display:""});
aj=ag.offset();
ag.css({visibility:ao,display:af});
}if(al.top===X){al.top=aj.top;
}if(al.left===X){al.left=aj.left;
}}if(!j(ak.visible)||ak.visible===null){ak.visible=ag.is(Y);
}aq=an.wrapper=ag.closest(B);
if(!ag.is(".k-content")||!aq[0]){ag.addClass("k-window-content k-content");
an._createWindow(ag,ak);
aq=an.wrapper=ag.closest(B);
an._dimensions();
}an._position();
if(ak.pinned){an.pin(true);
}if(ae){an.refresh(ae);
}if(ak.visible){an.toFront();
}ap=aq.children(C);
an._tabindex(ap);
if(ak.visible&&ak.modal){an._overlay(aq.is(Y)).css({opacity:0.5});
}aq.on("mouseenter"+L,W,P(an._buttonEnter,an)).on("mouseleave"+L,W,P(an._buttonLeave,an)).on("click"+L,"> "+W,P(an._windowActionHandler,an));
ap.on("keydown"+L,P(an._keydown,an)).on("focus"+L,P(an._focus,an)).on("blur"+L,P(an._blur,an));
this._resizable();
this._draggable();
ah=ag.attr("id");
if(ah){ah=ah+"_wnd_title";
aq.children(F).children(E).attr("id",ah);
ap.attr({role:"dialog","aria-labelledby":ah});
}aq.add(aq.children(".k-resize-handle,"+F)).on("mousedown"+L,P(an.toFront,an));
an.touchScroller=v.touchScroller(ag);
an._resizeHandler=P(an._onDocumentResize,an);
an._marker=v.guid().substring(0,8);
b(window).on("resize"+L+an._marker,an._resizeHandler);
if(ak.visible){an.trigger(M);
an.trigger(c);
}v.notify(an);
},_buttonEnter:function(ae){b(ae.currentTarget).addClass(x);
},_buttonLeave:function(ae){b(ae.currentTarget).removeClass(x);
},_focus:function(){this.wrapper.addClass(w);
},_blur:function(){this.wrapper.removeClass(w);
},_dimensions:function(){var al=this.wrapper;
var ai=this.options;
var ak=ai.width;
var af=ai.height;
var ah=ai.maxHeight;
var ae=["minWidth","minHeight","maxWidth","maxHeight"];
this.title(ai.title);
for(var ag=0;
ag<ae.length;
ag++){var aj=ai[ae[ag]]||"";
if(aj!=Infinity){al.css(ae[ag],aj);
}}if(ah!=Infinity){this.element.css("maxHeight",ah);
}if(ak){if(ak.toString().indexOf("%")>0){al.width(ak);
}else{al.width(g(ak,ai.minWidth,ai.maxWidth));
}}else{al.width("");
}if(af){if(af.toString().indexOf("%")>0){al.height(af);
}else{al.height(g(af,ai.minHeight,ai.maxHeight));
}}else{al.height("");
}if(!ai.visible){al.hide();
}},_position:function(){var af=this.wrapper,ae=this.options.position;
if(ae.top===0){ae.top=ae.top.toString();
}if(ae.left===0){ae.left=ae.left.toString();
}af.css({top:ae.top||"",left:ae.left||""});
},_animationOptions:function(ag){var ae=this.options.animation;
var af={open:{effects:{}},close:{hide:true,effects:{}}};
return ae&&ae[ag]||af[ag];
},_resize:function(){v.resize(this.element.children());
},_resizable:function(){var ae=this.options.resizable;
var af=this.wrapper;
if(this.resizing){af.off("dblclick"+L).children(D).remove();
this.resizing.destroy();
this.resizing=null;
}if(ae){af.on("dblclick"+L,F,P(function(ag){if(!b(ag.target).closest(".k-window-action").length){this.toggleMaximization();
}},this));
n("n e s w se sw ne nw".split(" "),function(ah,ag){af.append(V.resizeHandle(ag));
});
this.resizing=new ac(this);
}af=null;
},_draggable:function(){var ae=this.options.draggable;
if(this.dragging){this.dragging.destroy();
this.dragging=null;
}if(ae){this.dragging=new ab(this,ae.dragHandle||F);
}},_actions:function(){var ae=this.options.actions;
var ag=this.wrapper.children(F);
var af=ag.find(".k-window-actions");
ae=b.map(ae,function(ah){return{name:ah};
});
af.html(v.render(V.action,ae));
},setOptions:function(ae){Z.fn.setOptions.call(this,ae);
var af=this.options.scrollable!==false;
this.restore();
this._dimensions();
this._position();
this._resizable();
this._draggable();
this._actions();
if(typeof ae.modal!=="undefined"){var ag=this.options.visible!==false;
this._overlay(ae.modal&&ag);
}this.element.css(N,af?"":"hidden");
},events:[M,c,i,f,J,H,Q,S,T,m,k,o],options:{name:"Window",animation:{open:{effects:{zoom:{direction:"in"},fade:{direction:"in"}},duration:350},close:{effects:{zoom:{direction:"out",properties:{scale:0.7}},fade:{direction:"out"}},duration:350,hide:true}},title:"",actions:["Close"],autoFocus:true,modal:false,resizable:true,draggable:true,minWidth:90,minHeight:50,maxWidth:Infinity,maxHeight:Infinity,pinned:false,scrollable:true,position:{},content:null,visible:null,height:null,width:null,appendTo:"body"},_closable:function(){return b.inArray("close",b.map(this.options.actions,function(ae){return ae.toLowerCase();
}))>-1;
},_keydown:function(af){var ap=this,ao=ap.options,ak=v.keys,aj=af.keyCode,ar=ap.wrapper,an,ah,ae=10,ai=ap.options.isMaximized,am,al,aq,ag;
if(af.target!=af.currentTarget||ap._closing){return;
}if(aj==ak.ESC&&ap._closable()){ap._close(false);
}if(ao.draggable&&!af.ctrlKey&&!ai){an=v.getOffset(ar);
if(aj==ak.UP){ah=ar.css("top",an.top-ae);
}else{if(aj==ak.DOWN){ah=ar.css("top",an.top+ae);
}else{if(aj==ak.LEFT){ah=ar.css("left",an.left-ae);
}else{if(aj==ak.RIGHT){ah=ar.css("left",an.left+ae);
}}}}}if(ao.resizable&&af.ctrlKey&&!ai){if(aj==ak.UP){ah=true;
al=ar.height()-ae;
}else{if(aj==ak.DOWN){ah=true;
al=ar.height()+ae;
}}if(aj==ak.LEFT){ah=true;
am=ar.width()-ae;
}else{if(aj==ak.RIGHT){ah=true;
am=ar.width()+ae;
}}if(ah){aq=g(am,ao.minWidth,ao.maxWidth);
ag=g(al,ao.minHeight,ao.maxHeight);
if(!isNaN(aq)){ar.width(aq);
ap.options.width=aq+"px";
}if(!isNaN(ag)){ar.height(ag);
ap.options.height=ag+"px";
}ap.resize();
}}if(ah){af.preventDefault();
}},_overlay:function(af){var ae=this.appendTo.children(y),ag=this.wrapper;
if(!ae.length){ae=b("<div class='k-overlay' />");
}ae.insertBefore(ag[0]).toggle(af).css(ad,parseInt(ag.css(ad),10)-1);
return ae;
},_actionForIcon:function(ae){var af=/\bk-i-\w+\b/.exec(ae[0].className)[0];
return{"k-i-close":"_close","k-i-maximize":"maximize","k-i-minimize":"minimize","k-i-restore":"restore","k-i-refresh":"refresh","k-i-pin":"pin","k-i-unpin":"unpin"}[af];
},_windowActionHandler:function(af){if(this._closing){return;
}var ag=b(af.target).closest(".k-window-action").find(".k-icon");
var ae=this._actionForIcon(ag);
if(ae){af.preventDefault();
this[ae]();
return false;
}},_modals:function(){var ae=this;
var af=b(B).filter(function(){var ag=b(this);
var ah=ae._object(ag);
var ai=ah&&ah.options;
return ai&&ai.modal&&ai.visible&&ai.appendTo===ae.options.appendTo&&ag.is(Y);
}).sort(function(ag,ah){return +b(ag).css("zIndex")-+b(ah).css("zIndex");
});
ae=null;
return af;
},_object:function(af){var ae=af.children(C);
var ag=v.widgetInstance(ae);
if(ag instanceof aa){return ag;
}return X;
},center:function(){var ak=this,ah=ak.options.position,al=ak.wrapper,ae=b(window),aj=0,ai=0,ag,af;
if(ak.options.isMaximized){return ak;
}if(!ak.options.pinned){aj=ae.scrollTop();
ai=ae.scrollLeft();
}af=ai+Math.max(0,(ae.width()-al.width())/2);
ag=aj+Math.max(0,(ae.height()-al.height()-parseInt(al.css("paddingTop"),10))/2);
al.css({left:af,top:ag});
ah.top=ag;
ah.left=af;
return ak;
},title:function(af){var ag=this,ak=ag.wrapper,ae=ag.options,ai=ak.children(F),ah=ai.children(E),aj;
if(!arguments.length){return ah.html();
}if(af===false){ak.addClass("k-window-titleless");
ai.remove();
}else{if(!ai.length){ak.prepend(V.titlebar(ae));
ag._actions();
ai=ak.children(F);
}else{ah.html(af);
}aj=ai.outerHeight();
ak.css("padding-top",aj);
ai.css("margin-top",-aj);
}ag.options.title=af;
return ag;
},content:function(ag,af){var ae=this.wrapper.children(C),ah=ae.children(".km-scroll-container");
ae=ah[0]?ah:ae;
if(!j(ag)){return ae.html();
}this.angular("cleanup",function(){return{elements:ae.children()};
});
v.destroy(this.element.children());
ae.empty().html(ag);
this.angular("compile",function(){var ai=[];
for(var aj=ae.length;
--aj>=0;
){ai.push({dataItem:af});
}return{elements:ae.children(),data:ai};
});
return this;
},open:function(){var al=this,am=al.wrapper,ag=al.options,ak=this._animationOptions("open"),ae=am.children(C),ai,ah,af=b(document);
if(!al.trigger(M)){if(al._closing){am.kendoStop(true,true);
}al._closing=false;
al.toFront();
if(ag.autoFocus){al.element.focus();
}ag.visible=true;
if(ag.modal){ah=!!al._modals().length;
ai=al._overlay(ah);
ai.kendoStop(true,true);
if(ak.duration&&v.effects.Fade&&!ah){var aj=v.fx(ai).fadeIn();
aj.duration(ak.duration||0);
aj.endValue(0.5);
aj.play();
}else{ai.css("opacity",0.5);
}ai.show();
}if(!am.is(Y)){ae.css(N,r);
am.show().kendoStop().kendoAnimate({effects:ak.effects,duration:ak.duration,complete:P(this._activate,this)});
}}if(ag.isMaximized){al._documentScrollTop=af.scrollTop();
al._documentScrollLeft=af.scrollLeft();
b("html, body").css(N,r);
}return al;
},_activate:function(){var ae=this.options.scrollable!==false;
if(this.options.autoFocus){this.element.focus();
}this.element.css(N,ae?"":"hidden");
v.resize(this.element.children());
this.trigger(c);
},_removeOverlay:function(ak){var ag=this._modals();
var ah=this.options;
var af=ah.modal&&!ag.length;
var ai=ah.modal?this._overlay(true):b(X);
var ae=this._animationOptions("close");
if(af){if(!ak&&ae.duration&&v.effects.Fade){var aj=v.fx(ai).fadeOut();
aj.duration(ae.duration||0);
aj.startValue(0.5);
aj.play();
}else{this._overlay(false).remove();
}}else{if(ag.length){this._object(ag.last())._overlay(true);
}}},_close:function(ai){var aj=this,ak=aj.wrapper,ag=aj.options,ah=this._animationOptions("open"),af=this._animationOptions("close"),ae=b(document);
if(ak.is(Y)&&!aj.trigger(f,{userTriggered:!ai})){if(aj._closing){return;
}aj._closing=true;
ag.visible=false;
b(B).each(function(an,am){var al=b(am).children(C);
if(am!=ak&&al.find("> ."+u).length>0){al.children(y).remove();
}});
this._removeOverlay();
ak.kendoStop().kendoAnimate({effects:af.effects||ah.effects,reverse:af.reverse===true,duration:af.duration,complete:P(this._deactivate,this)});
}if(aj.options.isMaximized){b("html, body").css(N,"");
if(aj._documentScrollTop&&aj._documentScrollTop>0){ae.scrollTop(aj._documentScrollTop);
}if(aj._documentScrollLeft&&aj._documentScrollLeft>0){ae.scrollLeft(aj._documentScrollLeft);
}}},_deactivate:function(){var af=this;
af.wrapper.hide().css("opacity","");
af.trigger(i);
if(af.options.modal){var ae=af._object(af._modals().last());
if(ae){ae.toFront();
}}},close:function(){this._close(true);
return this;
},_actionable:function(ae){return b(ae).is(W+","+W+" .k-icon,:input,a");
},_shouldFocus:function(ag){var ae=d(),af=this.element;
return this.options.autoFocus&&!b(ae).is(af)&&!this._actionable(ag)&&(!af.find(ae).length||!af.find(ag).length);
},toFront:function(af){var aj=this,al=aj.wrapper,ae=al[0],am=+al.css(ad),ag=am,ai=af&&af.target||null;
b(B).each(function(ap,ao){var aq=b(ao),ar=aq.css(ad),an=aq.children(C);
if(!isNaN(ar)){am=Math.max(+ar,am);
}if(ao!=ae&&an.find("> ."+u).length>0){an.append(V.overlay);
}});
if(!al[0].style.zIndex||ag<am){al.css(ad,am+2);
}aj.element.find("> .k-overlay").remove();
if(aj._shouldFocus(ai)){aj.element.focus();
var ah=b(window).scrollTop(),ak=parseInt(al.position().top,10);
if(ak>0&&ak<ah){if(ah>0){b(window).scrollTop(ak);
}else{al.css("top",ah);
}}}al=null;
return aj;
},toggleMaximization:function(){if(this._closing){return this;
}return this[this.options.isMaximized?"restore":"maximize"]();
},restore:function(){var ai=this;
var ag=ai.options;
var af=ag.minHeight;
var ah=ai.restoreOptions;
var ae=b(document);
if(!ag.isMaximized&&!ag.isMinimized){return ai;
}if(af&&af!=Infinity){ai.wrapper.css("min-height",af);
}ai.wrapper.css({position:ag.pinned?"fixed":"absolute",left:ah.left,top:ah.top,width:ah.width,height:ah.height}).removeClass(I).find(".k-window-content,.k-resize-handle").show().end().find(".k-window-titlebar .k-i-restore").parent().remove().end().end().find(K).parent().show().end().end().find(O).parent().show();
ai.options.width=ah.width;
ai.options.height=ah.height;
b("html, body").css(N,"");
if(this._documentScrollTop&&this._documentScrollTop>0){ae.scrollTop(this._documentScrollTop);
}if(this._documentScrollLeft&&this._documentScrollLeft>0){ae.scrollLeft(this._documentScrollLeft);
}ag.isMaximized=ag.isMinimized=false;
ai.resize();
return ai;
},_sizingAction:function(ae,af){var ai=this,aj=ai.wrapper,ah=aj[0].style,ag=ai.options;
if(ag.isMaximized||ag.isMinimized){return ai;
}ai.restoreOptions={width:ah.width,height:ah.height};
aj.children(D).hide().end().children(F).find(K).parent().hide().eq(0).before(V.action({name:"Restore"}));
af.call(ai);
ai.wrapper.children(F).find(O).parent().toggle(ae!=="maximize");
ai.trigger(ae);
return ai;
},maximize:function(){this._sizingAction("maximize",function(){var ag=this,ah=ag.wrapper,af=ah.position(),ae=b(document);
q(ag.restoreOptions,{left:af.left,top:af.top});
ah.css({left:0,top:0,position:"fixed"}).addClass(I);
this._documentScrollTop=ae.scrollTop();
this._documentScrollLeft=ae.scrollLeft();
b("html, body").css(N,r);
ag.options.isMaximized=true;
ag._onDocumentResize();
});
return this;
},minimize:function(){this._sizingAction("minimize",function(){var ae=this;
ae.wrapper.css({height:"",minHeight:""});
ae.element.hide();
ae.options.isMinimized=true;
});
return this;
},pin:function(ae){var ag=this,ai=b(window),aj=ag.wrapper,ah=parseInt(aj.css("top"),10),af=parseInt(aj.css("left"),10);
if(ae||!ag.options.pinned&&!ag.options.isMaximized){aj.css({position:"fixed",top:ah-ai.scrollTop(),left:af-ai.scrollLeft()});
aj.children(F).find(z).addClass("k-i-unpin").removeClass("k-i-pin");
ag.options.pinned=true;
}},unpin:function(){var af=this,ah=b(window),ai=af.wrapper,ag=parseInt(ai.css("top"),10),ae=parseInt(ai.css("left"),10);
if(af.options.pinned&&!af.options.isMaximized){ai.css({position:"",top:ag+ah.scrollTop(),left:ae+ah.scrollLeft()});
ai.children(F).find(A).addClass("k-i-pin").removeClass("k-i-unpin");
af.options.pinned=false;
}},_onDocumentResize:function(){var af=this,ai=af.wrapper,ah=b(window),aj=v.support.zoomLevel(),ag,ae;
if(!af.options.isMaximized){return;
}ag=ah.width()/aj;
ae=ah.height()/aj-parseInt(ai.css("padding-top"),10);
ai.css({width:ag,height:ae});
af.options.width=ag;
af.options.height=ae;
af.resize();
},refresh:function(ah){var aj=this,ag=aj.options,ae=b(aj.element),af,ai,ak;
if(!t(ah)){ah={url:ah};
}ah=q({},ag.content,ah);
ai=j(ag.iframe)?ag.iframe:ah.iframe;
ak=ah.url;
if(ak){if(!j(ai)){ai=!s(ak);
}if(!ai){aj._ajaxRequest(ah);
}else{af=ae.find("."+u)[0];
if(af){af.src=ak||af.src;
}else{ae.html(V.contentFrame(q({},ag,{content:ah})));
}ae.find("."+u).unbind("load"+L).on("load"+L,P(this._triggerRefresh,this));
}}else{if(ah.template){aj.content(U(ah.template)({}));
}aj.trigger(Q);
}ae.toggleClass("k-window-iframecontent",!!ai);
return aj;
},_triggerRefresh:function(){this.trigger(Q);
},_ajaxComplete:function(){clearTimeout(this._loadingIconTimeout);
this.wrapper.find(R).removeClass(G);
},_ajaxError:function(af,ae){this.trigger(o,{status:ae,xhr:af});
},_ajaxSuccess:function(ae){return function(af){var ag=af;
if(ae){ag=U(ae)(af||{});
}this.content(ag,af);
this.element.prop("scrollTop",0);
this.trigger(Q);
};
},_showLoading:function(){this.wrapper.find(R).addClass(G);
},_ajaxRequest:function(ae){this._loadingIconTimeout=setTimeout(P(this._showLoading,this),100);
b.ajax(q({type:"GET",dataType:"html",cache:false,error:P(this._ajaxError,this),complete:P(this._ajaxComplete,this),success:P(this._ajaxSuccess(ae.template),this)},ae));
},_destroy:function(){if(this.resizing){this.resizing.destroy();
}if(this.dragging){this.dragging.destroy();
}this.wrapper.off(L).children(C).off(L).end().find(".k-resize-handle,.k-window-titlebar").off(L);
b(window).off("resize"+L+this._marker);
clearTimeout(this._loadingIconTimeout);
Z.fn.destroy.call(this);
this.unbind(X);
v.destroy(this.wrapper);
this._removeOverlay(true);
},destroy:function(){this._destroy();
this.wrapper.empty().remove();
this.wrapper=this.appendTo=this.element=b();
},_createWindow:function(){var ae=this.element,ah=this.options,af,ai,ag=v.support.isRtl(ae);
if(ah.scrollable===false){ae.css("overflow","hidden");
}ai=b(V.wrapper(ah));
af=ae.find("iframe:not(.k-content)").map(function(){var aj=this.getAttribute("src");
this.src="";
return aj;
});
ai.toggleClass("k-rtl",ag).appendTo(this.appendTo).append(ae).find("iframe:not(.k-content)").each(function(aj){this.src=af[aj];
});
ai.find(".k-window-title").css(ag?"left":"right",ai.find(".k-window-actions").outerWidth()+10);
ae.css("visibility","").show();
ae.find("[data-role=editor]").each(function(){var aj=b(this).data("kendoEditor");
if(aj){aj.refresh();
}});
ai=ae=null;
}});
V={wrapper:U("<div class='k-widget k-window' />"),action:U("<a role='button' href='\\#' class='k-window-action k-link' aria-label='#= name #'><span class='k-icon k-i-#= name.toLowerCase() #'></span></a>"),titlebar:U("<div class='k-window-titlebar k-header'>&nbsp;<span class='k-window-title'>#= title #</span><div class='k-window-actions' /></div>"),overlay:"<div class='k-overlay' />",contentFrame:U("<iframe frameborder='0' title='#= title #' class='"+u+"' src='#= content.url #'>This page requires frames in order to show content</iframe>"),resizeHandle:U("<div class='k-resize-handle k-resize-#= data #'></div>")};
function ac(af){var ae=this;
ae.owner=af;
ae._draggable=new l(af.wrapper,{filter:">"+D,group:af.wrapper.id+"-resizing",dragstart:P(ae.dragstart,ae),drag:P(ae.drag,ae),dragend:P(ae.dragend,ae)});
ae._draggable.userEvents.bind("press",P(ae.addOverlay,ae));
ae._draggable.userEvents.bind("release",P(ae.removeOverlay,ae));
}ac.prototype={addOverlay:function(){this.owner.wrapper.append(V.overlay);
},removeOverlay:function(){this.owner.wrapper.find(y).remove();
},dragstart:function(ae){var af=this;
var ag=af.owner;
var ah=ag.wrapper;
af.elementPadding=parseInt(ah.css("padding-top"),10);
af.initialPosition=v.getOffset(ah,"position");
af.resizeDirection=ae.currentTarget.prop("className").replace("k-resize-handle k-resize-","");
af.initialSize={width:ah.width(),height:ah.height()};
af.containerOffset=v.getOffset(ag.appendTo,"position");
ah.children(D).not(ae.currentTarget).hide();
b(e).css(h,ae.currentTarget.css(h));
},drag:function(ag){var am=this,ap=am.owner,aq=ap.wrapper,al=ap.options,af=am.resizeDirection,ae=am.containerOffset,ah=am.initialPosition,ai=am.initialSize,ak,aj,an,ao,ar=Math.max(ag.x.location,0),at=Math.max(ag.y.location,0);
if(af.indexOf("e")>=0){ak=ar-ah.left-ae.left;
aq.width(g(ak,al.minWidth,al.maxWidth));
}else{if(af.indexOf("w")>=0){ao=ah.left+ai.width+ae.left;
ak=g(ao-ar,al.minWidth,al.maxWidth);
aq.css({left:ao-ak-ae.left,width:ak});
}}if(af.indexOf("s")>=0){aj=at-ah.top-am.elementPadding-ae.top;
aq.height(g(aj,al.minHeight,al.maxHeight));
}else{if(af.indexOf("n")>=0){an=ah.top+ai.height+ae.top;
aj=g(an-at,al.minHeight,al.maxHeight);
aq.css({top:an-aj-ae.top,height:aj});
}}if(ak){ap.options.width=ak+"px";
}if(aj){ap.options.height=aj+"px";
}ap.resize();
},dragend:function(ae){var af=this,ag=af.owner,ah=ag.wrapper;
ah.children(D).not(ae.currentTarget).show();
b(e).css(h,"");
if(ag.touchScroller){ag.touchScroller.reset();
}if(ae.keyCode==27){ah.css(af.initialPosition).css(af.initialSize);
}ag.trigger(T);
return false;
},destroy:function(){if(this._draggable){this._draggable.destroy();
}this._draggable=this.owner=null;
}};
function ab(ag,ae){var af=this;
af.owner=ag;
af._draggable=new l(ag.wrapper,{filter:ae,group:ag.wrapper.id+"-moving",dragstart:P(af.dragstart,af),drag:P(af.drag,af),dragend:P(af.dragend,af),dragcancel:P(af.dragcancel,af)});
af._draggable.userEvents.stopPropagation=false;
}ab.prototype={dragstart:function(ag){var ai=this.owner,ah=ai.element,ae=ah.find(".k-window-actions"),af=v.getOffset(ai.appendTo);
ai.trigger(m);
ai.initialWindowPosition=v.getOffset(ai.wrapper,"position");
ai.initialPointerPosition={left:ag.x.client,top:ag.y.client};
ai.startPosition={left:ag.x.client-ai.initialWindowPosition.left,top:ag.y.client-ai.initialWindowPosition.top};
if(ae.length>0){ai.minLeftPosition=ae.outerWidth()+parseInt(ae.css("right"),10)-ah.outerWidth();
}else{ai.minLeftPosition=20-ah.outerWidth();
}ai.minLeftPosition-=af.left;
ai.minTopPosition=-af.top;
ai.wrapper.append(V.overlay).children(D).hide();
b(e).css(h,ag.currentTarget.css(h));
},drag:function(ae){var ag=this.owner;
var af=ag.options.position;
af.top=Math.max(ae.y.client-ag.startPosition.top,ag.minTopPosition);
af.left=Math.max(ae.x.client-ag.startPosition.left,ag.minLeftPosition);
if(v.support.transforms){b(ag.wrapper).css("transform","translate("+(ae.x.client-ag.initialPointerPosition.left)+"px, "+(ae.y.client-ag.initialPointerPosition.top)+"px)");
}else{b(ag.wrapper).css(af);
}},_finishDrag:function(){var ae=this.owner;
ae.wrapper.children(D).toggle(!ae.options.isMinimized).end().find(y).remove();
b(e).css(h,"");
},dragcancel:function(ae){this._finishDrag();
ae.currentTarget.closest(B).css(this.owner.initialWindowPosition);
},dragend:function(){b(this.owner.wrapper).css(this.owner.options.position).css("transform","");
this._finishDrag();
this.owner.trigger(k);
return false;
},destroy:function(){if(this._draggable){this._draggable.destroy();
}this._draggable=this.owner=null;
}};
v.ui.plugin(aa);
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

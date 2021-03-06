(function(b,a){a("kendo.tabstrip",["kendo.data"],b);
}(function(){var a={id:"tabstrip",name:"TabStrip",category:"web",description:"The TabStrip widget displays a collection of tabs with associated tab content.",depends:["data"]};
(function(b,Q){var w=window.kendo,P=w.ui,x=w.keys,A=b.map,k=b.each,O=b.trim,o=b.extend,M=w.template,T=P.Widget,n=/^(a|div)$/i,E=".kendoTabStrip",v="img",t="href",F="prev",J="show",z="k-link",y="k-last",e="click",m="error",l=":empty",u="k-image",p="k-first",I="select",c="activate",f="k-content",h="contentUrl",B="mouseenter",C="mouseleave",g="contentLoad",j="k-state-disabled",i="k-state-default",d="k-state-active",q="k-state-focused",s="k-state-hover",K="k-tab-on-top",D=".k-item:not(."+j+")",r=".k-tabstrip-items > "+D+":not(."+d+")",N={content:M("<div class='k-content'#= contentAttributes(data) # role='tabpanel'>#= content(item) #</div>"),itemWrapper:M("<#= tag(item) # class='k-link'#= contentUrl(item) ##= textAttributes(item) #>#= image(item) ##= sprite(item) ##= text(item) #</#= tag(item) #>"),item:M("<li class='#= wrapperCssClass(group, item) #' role='tab' #=item.active ? \"aria-selected='true'\" : ''#>#= itemWrapper(data) #</li>"),image:M("<img class='k-image' alt='' src='#= imageUrl #' />"),sprite:M("<span class='k-sprite #= spriteCssClass #'></span>"),empty:M("")},G={wrapperCssClass:function(U,W){var X="k-item",V=W.index;
if(W.enabled===false){X+=" k-state-disabled";
}else{X+=" k-state-default";
}if(V===0){X+=" k-first";
}if(V==U.length-1){X+=" k-last";
}return X;
},textAttributes:function(U){return U.url?" href='"+U.url+"'":"";
},text:function(U){return U.encoded===false?U.text:w.htmlEncode(U.text);
},tag:function(U){return U.url?"a":"span";
},contentAttributes:function(U){return U.active!==true?" style='display:none' aria-hidden='true' aria-expanded='false'":"";
},content:function(U){return U.content?U.content:U.contentUrl?"":"&nbsp;";
},contentUrl:function(U){return U.contentUrl?w.attr("content-url")+'="'+U.contentUrl+'"':"";
}};
function S(U){U.children(v).addClass(u);
U.children("a").addClass(z).children(v).addClass(u);
U.filter(":not([disabled]):not([class*=k-state-disabled])").addClass(i);
U.filter("li[disabled]").addClass(j).removeAttr("disabled");
U.filter(":not([class*=k-state])").children("a").filter(":focus").parent().addClass(d+" "+K);
U.attr("role","tab");
U.filter("."+d).attr("aria-selected",true);
U.each(function(){var V=b(this);
if(!V.children("."+z).length){V.contents().filter(function(){return !this.nodeName.match(n)&&!(this.nodeType==3&&!O(this.nodeValue));
}).wrapAll("<span class='"+z+"'/>");
}});
}function R(U){var V=U.children(".k-item");
V.filter(".k-first:not(:first-child)").removeClass(p);
V.filter(".k-last:not(:last-child)").removeClass(y);
V.filter(":first-child").addClass(p);
V.filter(":last-child").addClass(y);
}function H(U,V){return"<span class='k-button k-button-icon k-button-bare k-tabstrip-"+U+"' unselectable='on'><span class='k-icon "+V+"'></span></span>";
}var L=T.extend({init:function(V,W){var Y=this,Z;
T.fn.init.call(Y,V,W);
Y._animations(Y.options);
W=Y.options;
Y._wrapper();
Y._isRtl=w.support.isRtl(Y.wrapper);
Y._tabindex();
Y._updateClasses();
Y._dataSource();
if(W.dataSource){Y.dataSource.fetch();
}Y._tabPosition();
Y._scrollable();
if(Y.options.contentUrls){Y.wrapper.find(".k-tabstrip-items > .k-item").each(function(aa,ab){b(ab).find(">."+z).data(h,Y.options.contentUrls[aa]);
});
}Y.wrapper.on(B+E+" "+C+E,r,Y._toggleHover).on("focus"+E,b.proxy(Y._active,Y)).on("blur"+E,function(){Y._current(null);
});
Y._keyDownProxy=b.proxy(Y._keydown,Y);
if(W.navigatable){Y.wrapper.on("keydown"+E,Y._keyDownProxy);
}if(Y.options.value){Z=Y.options.value;
}Y.wrapper.children(".k-tabstrip-items").on(e+E,".k-state-disabled .k-link",false).on(e+E," > "+D,function(aa){var ad=Y.wrapper[0];
if(ad!==document.activeElement){var ac=w.support.browser.msie;
if(ac){try{ad.setActive();
}catch(ab){ad.focus();
}}else{ad.focus();
}}if(Y._click(b(aa.currentTarget))){aa.preventDefault();
}});
var X=Y.tabGroup.children("li."+d),U=Y.contentHolder(X.index());
if(X[0]&&U.length>0&&U[0].childNodes.length===0){Y.activateTab(X.eq(0));
}Y.element.attr("role","tablist");
if(Y.element[0].id){Y._ariaId=Y.element[0].id+"_ts_active";
}Y.value(Z);
w.notify(Y);
},_active:function(){var U=this.tabGroup.children().filter("."+d);
U=U[0]?U:this._endItem("first");
if(U[0]){this._current(U);
}},_endItem:function(U){return this.tabGroup.children(D)[U]();
},_item:function(W,U){var V;
if(U===F){V="last";
}else{V="first";
}if(!W){return this._endItem(V);
}W=W[U]();
if(!W[0]){W=this._endItem(V);
}if(W.hasClass(j)){W=this._item(W,U);
}return W;
},_current:function(U){var X=this,V=X._focused,W=X._ariaId;
if(U===Q){return V;
}if(V){if(V[0].id===W){V.removeAttr("id");
}V.removeClass(q);
}if(U){if(!U.hasClass(d)){U.addClass(q);
}X.element.removeAttr("aria-activedescendant");
W=U[0].id||W;
if(W){U.attr("id",W);
X.element.attr("aria-activedescendant",W);
}}X._focused=U;
},_keydown:function(W){var Z=this,X=W.keyCode,V=Z._current(),Y=Z._isRtl,U;
if(W.target!=W.currentTarget){return;
}if(X==x.DOWN||X==x.RIGHT){U=Y?F:"next";
}else{if(X==x.UP||X==x.LEFT){U=Y?"next":F;
}else{if(X==x.ENTER||X==x.SPACEBAR){Z._click(V);
W.preventDefault();
}else{if(X==x.HOME){Z._click(Z._endItem("first"));
W.preventDefault();
return;
}else{if(X==x.END){Z._click(Z._endItem("last"));
W.preventDefault();
return;
}}}}}if(U){Z._click(Z._item(V,U));
W.preventDefault();
}},_dataSource:function(){var U=this;
if(U.dataSource&&U._refreshHandler){U.dataSource.unbind("change",U._refreshHandler);
}else{U._refreshHandler=b.proxy(U.refresh,U);
}U.dataSource=w.data.DataSource.create(U.options.dataSource).bind("change",U._refreshHandler);
},setDataSource:function(U){var V=this;
V.options.dataSource=U;
V._dataSource();
V.dataSource.fetch();
},_animations:function(U){if(U&&"animation" in U&&!U.animation){U.animation={open:{effects:{}},close:{effects:{}}};
}},refresh:function(X){var ag=this,ab=ag.options,af=w.getter(ab.dataTextField),V=w.getter(ab.dataContentField),W=w.getter(ab.dataContentUrlField),Z=w.getter(ab.dataImageUrlField),ah=w.getter(ab.dataUrlField),ac=w.getter(ab.dataSpriteCssClass),Y,ae=[],ad,U,ai=ag.dataSource.view(),aa;
X=X||{};
U=X.action;
if(U){ai=X.items;
}for(Y=0,aa=ai.length;
Y<aa;
Y++){ad={text:af(ai[Y])};
if(ab.dataContentField){ad.content=V(ai[Y]);
}if(ab.dataContentUrlField){ad.contentUrl=W(ai[Y]);
}if(ab.dataUrlField){ad.url=ah(ai[Y]);
}if(ab.dataImageUrlField){ad.imageUrl=Z(ai[Y]);
}if(ab.dataSpriteCssClass){ad.spriteCssClass=ac(ai[Y]);
}ae[Y]=ad;
}if(X.action=="add"){if(X.index<ag.tabGroup.children().length){ag.insertBefore(ae,ag.tabGroup.children().eq(X.index));
}else{ag.append(ae);
}}else{if(X.action=="remove"){for(Y=0;
Y<ai.length;
Y++){ag.remove(X.index);
}}else{if(X.action=="itemchange"){Y=ag.dataSource.view().indexOf(ai[0]);
if(X.field===ab.dataTextField){ag.tabGroup.children().eq(Y).find(".k-link").text(ai[0].get(X.field));
}}else{ag.trigger("dataBinding");
ag.remove("li");
ag.append(ae);
ag.trigger("dataBound");
}}}},value:function(V){var U=this;
if(V!==Q){if(V!=U.value()){U.tabGroup.children().each(function(){if(b.trim(b(this).text())==V){U.select(this);
}});
}}else{return U.select().text();
}},items:function(){return this.tabGroup[0].children;
},setOptions:function(V){var W=this,U=W.options.animation;
W._animations(V);
V.animation=o(true,U,V.animation);
if(V.navigatable){W.wrapper.on("keydown"+E,W._keyDownProxy);
}else{W.wrapper.off("keydown"+E,W._keyDownProxy);
}T.fn.setOptions.call(W,V);
},events:[I,c,J,m,g,"change","dataBinding","dataBound"],options:{name:"TabStrip",dataTextField:"",dataContentField:"",dataImageUrlField:"",dataUrlField:"",dataSpriteCssClass:"",dataContentUrlField:"",tabPosition:"top",animation:{open:{effects:"expand:vertical fadeIn",duration:200},close:{duration:200}},collapsible:false,navigatable:true,contentUrls:false,scrollable:{distance:200}},destroy:function(){var V=this,U=V.scrollWrap;
T.fn.destroy.call(V);
if(V._refreshHandler){V.dataSource.unbind("change",V._refreshHandler);
}V.wrapper.off(E);
V.wrapper.children(".k-tabstrip-items").off(E);
if(V._scrollableModeActive){V._scrollPrevButton.off().remove();
V._scrollNextButton.off().remove();
}w.destroy(V.wrapper);
U.children(".k-tabstrip").unwrap();
},select:function(U){var V=this;
if(arguments.length===0){return V.tabGroup.children("li."+d);
}if(!isNaN(U)){U=V.tabGroup.children().get(U);
}U=V.tabGroup.find(U);
b(U).each(function(W,X){X=b(X);
if(!X.hasClass(d)&&!V.trigger(I,{item:X[0],contentElement:V.contentHolder(X.index())[0]})){V.activateTab(X);
}});
return V;
},enable:function(U,V){this._toggleDisabled(U,V!==false);
return this;
},disable:function(U){this._toggleDisabled(U,false);
return this;
},reload:function(U){U=this.tabGroup.find(U);
var V=this;
U.each(function(){var Y=b(this),X=Y.find("."+z).data(h),W=V.contentHolder(Y.index());
if(X){V.ajaxRequest(Y,W,null,X);
}});
return V;
},append:function(V){var W=this,U=W._create(V);
k(U.tabs,function(Y){var X=U.contents[Y];
W.tabGroup.append(this);
if(W.options.tabPosition=="bottom"){W.tabGroup.before(X);
}else{if(W._scrollableModeActive){W._scrollPrevButton.before(X);
}else{W.wrapper.append(X);
}}W.angular("compile",function(){return{elements:[X]};
});
});
R(W.tabGroup);
W._updateContentElements();
W.resize(true);
return W;
},insertBefore:function(X,W){W=this.tabGroup.find(W);
var Y=this,U=Y._create(X),V=b(Y.contentElement(W.index()));
k(U.tabs,function(aa){var Z=U.contents[aa];
W.before(this);
V.before(Z);
Y.angular("compile",function(){return{elements:[Z]};
});
});
R(Y.tabGroup);
Y._updateContentElements();
Y.resize(true);
return Y;
},insertAfter:function(X,W){W=this.tabGroup.find(W);
var Y=this,U=Y._create(X),V=b(Y.contentElement(W.index()));
k(U.tabs,function(aa){var Z=U.contents[aa];
W.after(this);
V.after(Z);
Y.angular("compile",function(){return{elements:[Z]};
});
});
R(Y.tabGroup);
Y._updateContentElements();
Y.resize(true);
return Y;
},remove:function(V){var W=this;
var X=typeof V;
var U;
if(X==="string"){V=W.tabGroup.find(V);
}else{if(X==="number"){V=W.tabGroup.children().eq(V);
}}U=V.map(function(){var Y=W.contentElement(b(this).index());
w.destroy(Y);
return Y;
});
V.remove();
U.remove();
W._updateContentElements();
W.resize(true);
return W;
},_create:function(X){var W=b.isPlainObject(X),Z=this,Y,V,U;
if(W||b.isArray(X)){X=b.isArray(X)?X:[X];
Y=A(X,function(ab,aa){return b(L.renderItem({group:Z.tabGroup,item:o(ab,{index:aa})}));
});
V=A(X,function(ab,aa){if(typeof ab.content=="string"||ab.contentUrl){return b(L.renderContent({item:o(ab,{index:aa})}));
}});
}else{if(typeof X=="string"&&X[0]!="<"){Y=Z.element.find(X);
}else{Y=b(X);
}V=b();
Y.each(function(){U=b("<div class='"+f+"'/>");
if(/k-tabstrip-items/.test(this.parentNode.className)){var aa=parseInt(this.getAttribute("aria-controls").replace(/^.*-/,""),10)-1;
U=b(Z.contentElement(aa));
}V=V.add(U);
});
S(Y);
}return{tabs:Y,contents:V};
},_toggleDisabled:function(U,V){U=this.tabGroup.find(U);
U.each(function(){b(this).toggleClass(i,V).toggleClass(j,!V);
});
},_updateClasses:function(){var X=this,W,U,V;
X.wrapper.addClass("k-widget k-header k-tabstrip");
X.tabGroup=X.wrapper.children("ul").addClass("k-tabstrip-items k-reset");
if(!X.tabGroup[0]){X.tabGroup=b("<ul class='k-tabstrip-items k-reset'/>").appendTo(X.wrapper);
}W=X.tabGroup.find("li").addClass("k-item");
if(W.length){U=W.filter("."+d).index();
V=U>=0?U:Q;
X.tabGroup.contents().filter(function(){return this.nodeType==3&&!O(this.nodeValue);
}).remove();
}if(U>=0){W.eq(U).addClass(K);
}X.contentElements=X.wrapper.children("div");
X.contentElements.addClass(f).eq(V).addClass(d).css({display:"block"});
if(W.length){S(W);
R(X.tabGroup);
X._updateContentElements();
}},_updateContentElements:function(){var Y=this,V=Y.options.contentUrls||[],W=Y.tabGroup.find(".k-item"),X=(Y.element.attr("id")||w.guid())+"-",U=Y.wrapper.children("div");
if(U.length&&W.length>U.length){U.each(function(ab){var Z=parseInt(this.id.replace(X,""),10),ac=W.filter("[aria-controls="+X+Z+"]"),aa=X+(ab+1);
ac.data("aria",aa);
this.setAttribute("id",aa);
});
W.each(function(){var Z=b(this);
this.setAttribute("aria-controls",Z.data("aria"));
Z.removeData("aria");
});
}else{W.each(function(ab){var Z=U.eq(ab),aa=X+(ab+1);
this.setAttribute("aria-controls",aa);
if(!Z.length&&V[ab]){b("<div class='"+f+"'/>").appendTo(Y.wrapper).attr("id",aa);
}else{Z.attr("id",aa);
if(!b(this).children(".k-loading")[0]&&!V[ab]){b("<span class='k-loading k-complete'/>").prependTo(this);
}}Z.attr("role","tabpanel");
Z.filter(":not(."+d+")").attr("aria-hidden",true).attr("aria-expanded",false);
Z.filter("."+d).attr("aria-expanded",true);
});
}Y.contentElements=Y.contentAnimators=Y.wrapper.children("div");
Y.tabsHeight=Y.tabGroup.outerHeight()+parseInt(Y.wrapper.css("border-top-width"),10)+parseInt(Y.wrapper.css("border-bottom-width"),10);
if(w.kineticScrollNeeded&&w.mobile.ui.Scroller){w.touchScroller(Y.contentElements);
Y.contentElements=Y.contentElements.children(".km-scroll-container");
}},_wrapper:function(){var U=this;
if(U.element.is("ul")){U.wrapper=U.element.wrapAll("<div />").parent();
}else{U.wrapper=U.element;
}U.scrollWrap=U.wrapper.parent(".k-tabstrip-wrapper");
if(!U.scrollWrap[0]){U.scrollWrap=U.wrapper.wrapAll("<div class='k-tabstrip-wrapper' />").parent();
}},_tabPosition:function(){var V=this,U=V.options.tabPosition;
V.wrapper.addClass("k-floatwrap k-tabstrip-"+U);
if(U=="bottom"){V.tabGroup.appendTo(V.wrapper);
}V.resize(true);
},_setContentElementsDimensions:function(){var ab=this,aa=ab.options.tabPosition;
if(aa=="left"||aa=="right"){var V=ab.wrapper.children(".k-content"),U=V.filter(":visible"),X="margin-"+aa,Z=ab.tabGroup,W=Z.outerWidth();
var Y=Math.ceil(Z.height())-parseInt(U.css("padding-top"),10)-parseInt(U.css("padding-bottom"),10)-parseInt(U.css("border-top-width"),10)-parseInt(U.css("border-bottom-width"),10);
setTimeout(function(){V.css(X,W).css("min-height",Y);
});
}},_resize:function(){this._setContentElementsDimensions();
this._scrollable();
},_sizeScrollWrap:function(U){if(U.is(":visible")){var W=this.options.tabPosition;
var V=Math.floor(U.outerHeight(true))+(W==="left"||W==="right"?2:this.tabsHeight);
this.scrollWrap.css("height",V).css("height");
}},_toggleHover:function(U){b(U.currentTarget).toggleClass(s,U.type==B);
},_click:function(Y){var ab=this,Z=Y.find("."+z),W=Z.attr(t),U=ab.options.collapsible,V=ab.contentHolder(Y.index()),aa,X;
if(Y.closest(".k-widget")[0]!=ab.wrapper[0]){return;
}if(Y.is("."+j+(!U?",."+d:""))){return true;
}X=Z.data(h)||W&&(W.charAt(W.length-1)=="#"||W.indexOf("#"+ab.element[0].id+"-")!=-1);
aa=!W||X;
if(ab.tabGroup.children("[data-animating]").length){return aa;
}if(ab.trigger(I,{item:Y[0],contentElement:V[0]})){return true;
}if(aa===false){return;
}if(U&&Y.is("."+d)){ab.deactivateTab(Y);
return true;
}if(ab.activateTab(Y)){aa=true;
}return aa;
},_scrollable:function(){var Y=this,U=Y.options,Z,X,W,V;
if(Y._scrollableAllowed()){Y.wrapper.addClass("k-tabstrip-scrollable");
Z=Y.wrapper[0].offsetWidth;
X=Y.tabGroup[0].scrollWidth;
if(X>Z&&!Y._scrollableModeActive){Y._nowScrollingTabs=false;
Y._isRtl=w.support.isRtl(Y.element);
Y.wrapper.append(H("prev","k-i-arrow-w")+H("next","k-i-arrow-e"));
W=Y._scrollPrevButton=Y.wrapper.children(".k-tabstrip-prev");
V=Y._scrollNextButton=Y.wrapper.children(".k-tabstrip-next");
Y.tabGroup.css({marginLeft:W.outerWidth()+9,marginRight:V.outerWidth()+12});
W.on("mousedown"+E,function(){Y._nowScrollingTabs=true;
Y._scrollTabsByDelta(U.scrollable.distance*(Y._isRtl?1:-1));
});
V.on("mousedown"+E,function(){Y._nowScrollingTabs=true;
Y._scrollTabsByDelta(U.scrollable.distance*(Y._isRtl?-1:1));
});
W.add(V).on("mouseup"+E,function(){Y._nowScrollingTabs=false;
});
Y._scrollableModeActive=true;
Y._toggleScrollButtons();
}else{if(Y._scrollableModeActive&&X<=Z){Y._scrollableModeActive=false;
Y.wrapper.removeClass("k-tabstrip-scrollable");
Y._scrollPrevButton.off().remove();
Y._scrollNextButton.off().remove();
Y.tabGroup.css({marginLeft:"",marginRight:""});
}else{if(!Y._scrollableModeActive){Y.wrapper.removeClass("k-tabstrip-scrollable");
}else{Y._toggleScrollButtons();
}}}}},_scrollableAllowed:function(){var U=this.options;
return U.scrollable&&!isNaN(U.scrollable.distance)&&(U.tabPosition=="top"||U.tabPosition=="bottom");
},_scrollTabsToItem:function(V){var ac=this,Z=ac.tabGroup,U=Z.scrollLeft(),Y=V.outerWidth(),W=ac._isRtl?V.position().left:V.position().left-Z.children().first().position().left,ab=Z[0].offsetWidth,aa=Math.ceil(parseFloat(Z.css("padding-left"))),X;
if(ac._isRtl){if(W<0){X=U+W-(ab-U)-aa;
}else{if(W+Y>ab){X=U+W-Y+aa*2;
}}}else{if(U+ab<W+Y){X=W+Y-ab+aa*2;
}else{if(U>W){X=W-aa;
}}}Z.finish().animate({scrollLeft:X},"fast","linear",function(){ac._toggleScrollButtons();
});
},_scrollTabsByDelta:function(U){var X=this;
var W=X.tabGroup;
var V=W.scrollLeft();
W.finish().animate({scrollLeft:V+U},"fast","linear",function(){if(X._nowScrollingTabs){X._scrollTabsByDelta(U);
}else{X._toggleScrollButtons();
}});
},_toggleScrollButtons:function(){var V=this,W=V.tabGroup,U=W.scrollLeft();
V._scrollPrevButton.toggle(V._isRtl?U<W[0].scrollWidth-W[0].offsetWidth-1:U!==0);
V._scrollNextButton.toggle(V._isRtl?U!==0:U<W[0].scrollWidth-W[0].offsetWidth-1);
},deactivateTab:function(Y){var Z=this,V=Z.options.animation,U=V.open,W=o({},V.close),X=W&&"effects" in W;
Y=Z.tabGroup.find(Y);
W=o(X?W:o({reverse:true},U),{hide:true});
if(w.size(U.effects)){Y.kendoAddClass(i,{duration:U.duration});
Y.kendoRemoveClass(d,{duration:U.duration});
}else{Y.addClass(i);
Y.removeClass(d);
}Y.removeAttr("aria-selected");
Z.contentAnimators.filter("."+d).kendoStop(true,true).kendoAnimate(W).removeClass(d).attr("aria-hidden",true);
},activateTab:function(ac){if(this.tabGroup.children("[data-animating]").length){return;
}ac=this.tabGroup.find(ac);
var ai=this,V=ai.options.animation,U=V.open,W=o({},V.close),aa=W&&"effects" in W,ae=ac.parent().children(),af=ae.filter("."+d),ad=ae.index(ac);
W=o(aa?W:o({reverse:true},U),{hide:true});
if(w.size(U.effects)){af.kendoRemoveClass(d,{duration:W.duration});
ac.kendoRemoveClass(s,{duration:W.duration});
}else{af.removeClass(d);
ac.removeClass(s);
}var X=ai.contentAnimators;
if(ai.inRequest){ai.xhr.abort();
ai.inRequest=false;
}if(X.length===0){ai.tabGroup.find("."+K).removeClass(K);
ac.addClass(K).css("z-index");
ac.addClass(d);
ai._current(ac);
ai.trigger("change");
if(ai._scrollableModeActive){ai._scrollTabsToItem(ac);
}return false;
}var aj=X.filter("."+d),Z=ai.contentHolder(ad),Y=Z.closest(".k-content");
ai.tabsHeight=ai.tabGroup.outerHeight()+parseInt(ai.wrapper.css("border-top-width"),10)+parseInt(ai.wrapper.css("border-bottom-width"),10);
ai._sizeScrollWrap(aj);
if(Z.length===0){aj.removeClass(d).attr("aria-hidden",true).kendoStop(true,true).kendoAnimate(W);
return false;
}ac.attr("data-animating",true);
var ab=(ac.children("."+z).data(h)||false)&&Z.is(l),ah=function(){ai.tabGroup.find("."+K).removeClass(K);
ac.addClass(K).css("z-index");
if(w.size(U.effects)){af.kendoAddClass(i,{duration:U.duration});
ac.kendoAddClass(d,{duration:U.duration});
}else{af.addClass(i);
ac.addClass(d);
}af.removeAttr("aria-selected");
ac.attr("aria-selected",true);
ai._current(ac);
ai._sizeScrollWrap(Y);
Y.addClass(d).removeAttr("aria-hidden").kendoStop(true,true).attr("aria-expanded",true).kendoAnimate(o({init:function(){ai.trigger(J,{item:ac[0],contentElement:Z[0]});
w.resize(Z);
}},U,{complete:function(){ac.removeAttr("data-animating");
ai.trigger(c,{item:ac[0],contentElement:Z[0]});
w.resize(Z);
ai.scrollWrap.css("height","").css("height");
}}));
},ag=function(){if(!ab){ah();
ai.trigger("change");
}else{ac.removeAttr("data-animating");
ai.ajaxRequest(ac,Z,function(){ac.attr("data-animating",true);
ah();
ai.trigger("change");
});
}if(ai._scrollableModeActive){ai._scrollTabsToItem(ac);
}};
aj.removeClass(d);
aj.attr("aria-hidden",true);
aj.attr("aria-expanded",false);
if(aj.length){aj.kendoStop(true,true).kendoAnimate(o({complete:ag},W));
}else{ag();
}return true;
},contentElement:function(X){if(isNaN(X-0)){return Q;
}var U=this.contentElements&&this.contentElements[0]&&!w.kineticScrollNeeded?this.contentElements:this.contentAnimators;
X=U&&X<0?U.length+X:X;
var W=new RegExp("-"+(X+1)+"$");
if(U){for(var V=0,Y=U.length;
V<Y;
V++){if(W.test(U.eq(V).closest(".k-content")[0].id)){return U[V];
}}}return Q;
},contentHolder:function(V){var U=b(this.contentElement(V)),W=U.children(".km-scroll-container");
return w.support.touch&&W[0]?W:U;
},ajaxRequest:function(X,V,U,af){X=this.tabGroup.find(X);
var ae=this,ag=b.ajaxSettings.xhr,ab=X.find("."+z),W={},aa=X.width()/2,Z=false,ad=X.find(".k-loading").removeClass("k-complete");
if(!ad[0]){ad=b("<span class='k-loading'/>").prependTo(X);
}var Y=aa*2-ad.width();
var ac=function(){ad.animate({marginLeft:(parseInt(ad.css("marginLeft"),10)||0)<aa?Y:0},500,ac);
};
if(w.support.browser.msie&&w.support.browser.version<10){setTimeout(ac,40);
}af=af||ab.data(h)||ab.attr(t);
ae.inRequest=true;
ae.xhr=b.ajax({type:"GET",cache:false,url:af,dataType:"html",data:W,xhr:function(){var ah=this,aj=ag(),ai=ah.progressUpload?"progressUpload":ah.progress?"progress":false;
if(aj){b.each([aj,aj.upload],function(){if(this.addEventListener){this.addEventListener("progress",function(ak){if(ai){ah[ai](ak);
}},false);
}});
}ah.noProgress=!(window.XMLHttpRequest&&"upload" in new XMLHttpRequest());
return aj;
},progress:function(ah){if(ah.lengthComputable){var ai=parseInt(ah.loaded/ah.total*100,10)+"%";
ad.stop(true).addClass("k-progress").css({width:ai,marginLeft:0});
}},error:function(ai,ah){if(ae.trigger("error",{xhr:ai,status:ah})){this.complete();
}},stopProgress:function(){clearInterval(Z);
ad.stop(true).addClass("k-progress")[0].style.cssText="";
},complete:function(ah){ae.inRequest=false;
if(this.noProgress){setTimeout(this.stopProgress,500);
}else{this.stopProgress();
}if(ah.statusText=="abort"){ad.remove();
}},success:function(aj){ad.addClass("k-complete");
try{var ai=this,al=10;
if(ai.noProgress){ad.width(al+"%");
Z=setInterval(function(){ai.progress({lengthComputable:true,loaded:Math.min(al,100),total:100});
al+=10;
},40);
}ae.angular("cleanup",function(){return{elements:V.get()};
});
w.destroy(V);
V.html(aj);
}catch(ak){var ah=window.console;
if(ah&&ah.error){ah.error(ak.name+": "+ak.message+" in "+af);
}this.error(this.xhr,"error");
}if(U){U.call(ae,V);
}ae.angular("compile",function(){return{elements:V.get()};
});
ae.trigger(g,{item:X[0],contentElement:V[0]});
}});
}});
o(L,{renderItem:function(W){W=o({tabStrip:{},group:{}},W);
var U=N.empty,V=W.item;
return N.item(o(W,{image:V.imageUrl?N.image:U,sprite:V.spriteCssClass?N.sprite:U,itemWrapper:N.itemWrapper},G));
},renderContent:function(U){return N.content(o(U,G));
}});
w.ui.plugin(L);
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

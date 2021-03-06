(function(b,a){a("util/main",["kendo.core"],b);
}(function(){(function(){var q=Math,n=window.kendo,f=n.deepExtend;
var i=q.PI/180,r=Number.MAX_VALUE,u=-Number.MAX_VALUE,N="undefined";
function g(P){return typeof P!==N;
}function G(R,Q){var P=x(Q);
return q.round(R*P)/P;
}function x(P){if(P){return q.pow(10,P);
}else{return 1;
}}function p(R,Q,P){return q.max(q.min(R,P),Q);
}function y(P){return P*i;
}function h(P){return P/i;
}function l(P){return typeof P==="number"&&!isNaN(P);
}function O(Q,P){return g(Q)?Q:P;
}function K(P){return P*P;
}function w(Q){var R=[];
for(var P in Q){R.push(P+Q[P]);
}return R.sort().join("");
}function j(R){var P=2166136261;
for(var Q=0;
Q<R.length;
++Q){P+=(P<<1)+(P<<4)+(P<<7)+(P<<8)+(P<<24);
P^=R.charCodeAt(Q);
}return P>>>0;
}function k(P){return j(w(P));
}var v=Date.now;
if(!v){v=function(){return new Date().getTime();
};
}function c(P){var R=P.length,Q,T=r,S=u;
for(Q=0;
Q<R;
Q++){S=q.max(S,P[Q]);
T=q.min(T,P[Q]);
}return{min:T,max:S};
}function e(P){return c(P).min;
}function d(P){return c(P).max;
}function J(P){return H(P).min;
}function I(P){return H(P).max;
}function H(P){var T=r,S=u;
for(var Q=0,R=P.length;
Q<R;
Q++){var U=P[Q];
if(U!==null&&isFinite(U)){T=q.min(T,U);
S=q.max(S,U);
}}return{min:T===r?undefined:T,max:S===u?undefined:S};
}function o(P){if(P){return P[P.length-1];
}}function a(P,Q){P.push.apply(P,Q);
return P;
}function E(P){return n.template(P,{useWithBlock:false,paramName:"d"});
}function A(P,Q){return g(Q)&&Q!==null?" "+P+"='"+Q+"' ":"";
}function z(P){var R="";
for(var Q=0;
Q<P.length;
Q++){R+=A(P[Q][0],P[Q][1]);
}return R;
}function D(P){var R="";
for(var Q=0;
Q<P.length;
Q++){var S=P[Q][1];
if(g(S)){R+=P[Q][0]+":"+S+";";
}}if(R!==""){return R;
}}function C(P){if(typeof P!=="string"){P+="px";
}return P;
}function B(R){var S=[];
if(R){var Q=n.toHyphens(R).split("-");
for(var P=0;
P<Q.length;
P++){S.push("k-pos-"+Q[P]);
}}return S.join(" ");
}function m(P){return P===""||P===null||P==="none"||P==="transparent"||!g(P);
}function b(Q){var P={1:"i",10:"x",100:"c",2:"ii",20:"xx",200:"cc",3:"iii",30:"xxx",300:"ccc",4:"iv",40:"xl",400:"cd",5:"v",50:"l",500:"d",6:"vi",60:"lx",600:"dc",7:"vii",70:"lxx",700:"dcc",8:"viii",80:"lxxx",800:"dccc",9:"ix",90:"xc",900:"cm",1000:"m"};
var S=[1000,900,800,700,600,500,400,300,200,100,90,80,70,60,50,40,30,20,10,9,8,7,6,5,4,3,2,1];
var R="";
while(Q>0){if(Q<S[0]){S.shift();
}else{R+=P[S[0]];
Q-=S[0];
}}return R;
}function F(S){S=S.toLowerCase();
var P={i:1,v:5,x:10,l:50,c:100,d:500,m:1000};
var U=0,R=0;
for(var Q=0;
Q<S.length;
++Q){var T=P[S.charAt(Q)];
if(!T){return null;
}U+=T;
if(T>R){U-=2*R;
}R=T;
}return U;
}function s(Q){var P=Object.create(null);
return function(){var S="";
for(var R=arguments.length;
--R>=0;
){S+=":"+arguments[R];
}if(S in P){return P[S];
}return Q.apply(this,arguments);
};
}function L(T){var S=[],P=0,R=T.length,U,Q;
while(P<R){U=T.charCodeAt(P++);
if(U>=55296&&U<=56319&&P<R){Q=T.charCodeAt(P++);
if((Q&64512)==56320){S.push(((U&1023)<<10)+(Q&1023)+65536);
}else{S.push(U);
P--;
}}else{S.push(U);
}}return S;
}function M(P){return P.map(function(R){var Q="";
if(R>65535){R-=65536;
Q+=String.fromCharCode(R>>>10&1023|55296);
R=56320|R&1023;
}Q+=String.fromCharCode(R);
return Q;
}).join("");
}function t(P,Q){if(P.length<2){return P.slice();
}function R(T,V){var Y=[],U=0,W=0,X=0;
while(U<T.length&&W<V.length){if(Q(T[U],V[W])<=0){Y[X++]=T[U++];
}else{Y[X++]=V[W++];
}}if(U<T.length){Y.push.apply(Y,T.slice(U));
}if(W<V.length){Y.push.apply(Y,V.slice(W));
}return Y;
}return function S(T){if(T.length<=1){return T;
}var V=Math.floor(T.length/2);
var U=T.slice(0,V);
var W=T.slice(V);
U=S(U);
W=S(W);
return R(U,W);
}(P);
}f(n,{util:{MAX_NUM:r,MIN_NUM:u,append:a,arrayLimits:c,arrayMin:e,arrayMax:d,defined:g,deg:h,hashKey:j,hashObject:k,isNumber:l,isTransparent:m,last:o,limitValue:p,now:v,objectKey:w,round:G,rad:y,renderAttr:A,renderAllAttr:z,renderPos:B,renderSize:C,renderStyle:D,renderTemplate:E,sparseArrayLimits:H,sparseArrayMin:J,sparseArrayMax:I,sqr:K,valueOrDefault:O,romanToArabic:F,arabicToRoman:b,memoize:s,ucs2encode:M,ucs2decode:L,mergeSort:t}});
n.drawing.util=n.util;
n.dataviz.util=n.util;
}());
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));
(function(b,a){a("util/text-metrics",["kendo.core","util/main"],b);
}(function(){(function(a){var e=document,f=window.kendo,b=f.Class,k=f.util,d=k.defined;
var h=b.extend({init:function(m){this._size=m;
this._length=0;
this._map={};
},put:function(n,q){var o=this,p=o._map,m={key:n,value:q};
p[n]=m;
if(!o._head){o._head=o._tail=m;
}else{o._tail.newer=m;
m.older=o._tail;
o._tail=m;
}if(o._length>=o._size){p[o._head.key]=null;
o._head=o._head.newer;
o._head.older=null;
}else{o._length++;
}},get:function(n){var o=this,m=o._map[n];
if(m){if(m===o._head&&m!==o._tail){o._head=m.newer;
o._head.older=null;
}if(m!==o._tail){if(m.older){m.older.newer=m.newer;
m.newer.older=m.older;
}m.older=o._tail;
m.newer=null;
o._tail.newer=m;
o._tail=m;
}return m.value;
}}});
var c=a("<div style='position: absolute !important; top: -4000px !important; width: auto !important; height: auto !important;padding: 0 !important; margin: 0 !important; border: 0 !important;line-height: normal !important; visibility: hidden !important; white-space: nowrap!important;' />")[0];
function l(){return{width:0,height:0,baseline:0};
}var j=b.extend({init:function(m){this._cache=new h(1000);
this._initOptions(m);
},options:{baselineMarkerSize:1},measure:function(v,t,n){if(!v){return l();
}var u=k.objectKey(t),p=k.hashKey(v+u),o=this._cache.get(p);
if(o){return o;
}var s=l();
var r=n?n:c;
var m=this._baselineMarker().cloneNode(false);
for(var q in t){var w=t[q];
if(d(w)){r.style[q]=w;
}}a(r).text(v);
r.appendChild(m);
e.body.appendChild(r);
if((v+"").length){s.width=r.offsetWidth-this.options.baselineMarkerSize;
s.height=r.offsetHeight;
s.baseline=m.offsetTop+this.options.baselineMarkerSize;
}if(s.width>0&&s.height>0){this._cache.put(p,s);
}r.parentNode.removeChild(r);
return s;
},_baselineMarker:function(){return a("<div class='k-baseline-marker' style='display: inline-block; vertical-align: baseline;width: "+this.options.baselineMarkerSize+"px; height: "+this.options.baselineMarkerSize+"px;overflow: hidden;' />")[0];
}});
j.current=new j();
function i(o,n,m){return j.current.measure(o,n,m);
}function g(o,m){var p=[];
if(o.length>0&&document.fonts){try{p=o.map(function(q){return document.fonts.load(q);
});
}catch(n){f.logToConsole(n);
}Promise.all(p).then(m,m);
}else{m();
}}f.util.TextMetrics=j;
f.util.LRUCache=h;
f.util.loadFonts=g;
f.util.measureText=i;
}(window.kendo.jQuery));
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));
(function(b,a){a("util/base64",["util/main"],b);
}(function(){(function(){var e=window.kendo,a=e.deepExtend,d=String.fromCharCode;
var f="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
function b(p){var q="";
var g,h,j,k,l,m,n;
var o=0;
p=c(p);
while(o<p.length){g=p.charCodeAt(o++);
h=p.charCodeAt(o++);
j=p.charCodeAt(o++);
k=g>>2;
l=(g&3)<<4|h>>4;
m=(h&15)<<2|j>>6;
n=j&63;
if(isNaN(h)){m=n=64;
}else{if(isNaN(j)){n=64;
}}q=q+f.charAt(k)+f.charAt(l)+f.charAt(m)+f.charAt(n);
}return q;
}function c(j){var k="";
for(var h=0;
h<j.length;
h++){var g=j.charCodeAt(h);
if(g<128){k+=d(g);
}else{if(g<2048){k+=d(192|g>>>6);
k+=d(128|g&63);
}else{if(g<65536){k+=d(224|g>>>12);
k+=d(128|g>>>6&63);
k+=d(128|g&63);
}}}}return k;
}a(e.util,{encodeBase64:b,encodeUTF8:c});
}());
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));
(function(b,a){a("mixins/observers",["kendo.core"],b);
}(function(){(function(a){var e=Math,d=window.kendo,b=d.deepExtend,c=a.inArray;
var f={observers:function(){this._observers=this._observers||[];
return this._observers;
},addObserver:function(g){if(!this._observers){this._observers=[g];
}else{this._observers.push(g);
}return this;
},removeObserver:function(g){var i=this.observers();
var h=c(g,i);
if(h!=-1){i.splice(h,1);
}return this;
},trigger:function(i,g){var k=this._observers;
var j;
var h;
if(k&&!this._suspended){for(h=0;
h<k.length;
h++){j=k[h];
if(j[i]){j[i](g);
}}}return this;
},optionsChange:function(g){g=g||{};
g.element=this;
this.trigger("optionsChange",g);
},geometryChange:function(){this.trigger("geometryChange",{element:this});
},suspend:function(){this._suspended=(this._suspended||0)+1;
return this;
},resume:function(){this._suspended=e.max((this._suspended||0)-1,0);
return this;
},_observerField:function(g,h){if(this[g]){this[g].removeObserver(this);
}this[g]=h;
h.addObserver(this);
}};
b(d,{mixins:{ObserversMixin:f}});
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));
(function(b,a){a("kendo.dataviz.treemap",["kendo.data","kendo.userevents","kendo.dataviz.themes"],b);
}(function(){var a={id:"dataviz.treeMap",name:"TreeMap",category:"dataviz",description:"The Kendo DataViz TreeMap",depends:["data","userevents","dataviz.themes"]};
(function(b,J){var u=Math,z=b.proxy,r=b.isArray,t=window.kendo,e=t.Class,L=t.ui.Widget,H=t.template,m=t.deepExtend,q=t.data.HierarchicalDataSource,p=t.getter,k=t.dataviz;
var y=".kendoTreeMap",d="change",j="dataBound",s="itemCreated",v=Number.MAX_VALUE,x="mouseover"+y,w="mouseleave"+y,K="undefined";
var I=L.extend({init:function(M,N){t.destroy(M);
b(M).empty();
L.fn.init.call(this,M,N);
this.wrapper=this.element;
this._initTheme(this.options);
this.element.addClass("k-widget k-treemap");
this._setLayout();
this._originalOptions=m({},this.options);
this._initDataSource();
this._attachEvents();
t.notify(this,k.ui);
},options:{name:"TreeMap",theme:"default",autoBind:true,textField:"text",valueField:"value",colorField:"color"},events:[j,s],_initTheme:function(M){var N=this,Q=k.ui.themes||{},O=((M||{}).theme||"").toLowerCase(),P=(Q[O]||{}).treeMap;
N.options=m({},P,M);
},_attachEvents:function(){this.element.on(x,z(this._mouseover,this)).on(w,z(this._mouseleave,this));
this._resizeHandler=z(this.resize,this,false);
t.onResize(this._resizeHandler);
},_setLayout:function(){if(this.options.type==="horizontal"){this._layout=new D(false);
this._view=new E(this,this.options);
}else{if(this.options.type==="vertical"){this._layout=new D(true);
this._view=new E(this,this.options);
}else{this._layout=new F();
this._view=new G(this,this.options);
}}},_initDataSource:function(){var O=this,N=O.options,M=N.dataSource;
O._dataChangeHandler=z(O._onDataChange,O);
O.dataSource=q.create(M).bind(d,O._dataChangeHandler);
if(M){if(O.options.autoBind){O.dataSource.fetch();
}}},setDataSource:function(M){var N=this;
N.dataSource.unbind(d,N._dataChangeHandler);
N.dataSource=M.bind(d,N._dataChangeHandler);
if(M){if(N.options.autoBind){N.dataSource.fetch();
}}},_onDataChange:function(M){var R=M.node;
var Q=M.items;
var S=this.options;
var P,O;
if(!R){this._cleanItems();
this.element.empty();
P=this._wrapItem(Q[0]);
this._layout.createRoot(P,this.element.outerWidth(),this.element.outerHeight(),this.options.type==="vertical");
this._view.createRoot(P);
this._root=P;
this._colorIdx=0;
}else{if(Q.length){var T=this._getByUid(R.uid);
T.children=[];
Q=new t.data.Query(Q)._sortForGrouping(S.valueField,"desc");
for(O=0;
O<Q.length;
O++){P=Q[O];
T.children.push(this._wrapItem(P));
}var N=this._view.htmlSize(T);
this._layout.compute(T.children,T.coord,N);
this._setColors(T.children);
this._view.render(T);
}}for(O=0;
O<Q.length;
O++){Q[O].load();
}if(R){this.trigger(j,{node:R});
}},_cleanItems:function(){var M=this;
M.angular("cleanup",function(){return{elements:M.element.find(".k-leaf div,.k-treemap-title,.k-treemap-title-vertical")};
});
},_setColors:function(S){var P=this.options.colors;
var N=this._colorIdx;
var M=P[N%P.length];
var O,R;
if(r(M)){O=h(M[0],M[1],S.length);
}var T=false;
for(var Q=0;
Q<S.length;
Q++){R=S[Q];
if(!n(R.color)){if(O){R.color=O[Q];
}else{R.color=M;
}}if(!R.dataItem.hasChildren){T=true;
}}if(T){this._colorIdx++;
}},_contentSize:function(M){this.view.renderHeight(M);
},_wrapItem:function(M){var N={};
if(n(this.options.valueField)){N.value=o(this.options.valueField,M);
}if(n(this.options.colorField)){N.color=o(this.options.colorField,M);
}if(n(this.options.textField)){N.text=o(this.options.textField,M);
}N.level=M.level();
N.dataItem=M;
return N;
},_getByUid:function(O){var N=[this._root];
var M;
while(N.length){M=N.pop();
if(M.dataItem.uid===O){return M;
}if(M.children){N=N.concat(M.children);
}}},dataItem:function(N){var O=b(N).attr(t.attr("uid")),M=this.dataSource;
return M&&M.getByUid(O);
},findByUid:function(M){return this.element.find(".k-treemap-tile["+t.attr("uid")+"='"+M+"']");
},_mouseover:function(M){var N=b(M.target);
if(N.hasClass("k-leaf")){this._removeActiveState();
N.removeClass("k-state-hover").addClass("k-state-hover");
}},_removeActiveState:function(){this.element.find(".k-state-hover").removeClass("k-state-hover");
},_mouseleave:function(){this._removeActiveState();
},destroy:function(){L.fn.destroy.call(this);
this.element.off(y);
if(this.dataSource){this.dataSource.unbind(d,this._dataChangeHandler);
}this._root=null;
t.unbindResize(this._resizeHandler);
t.destroy(this.element);
},items:function(){return b();
},getSize:function(){return t.dimensions(this.element);
},_resize:function(){var N=this._root;
if(N){var M=this.element;
var O=M.children();
N.coord.width=M.outerWidth();
N.coord.height=M.outerHeight();
O.css({width:N.coord.width,height:N.coord.height});
this._resizeItems(N,O);
}},_resizeItems:function(R,O){if(R.children&&R.children.length){var P=O.children(".k-treemap-wrap").children();
var M,N;
this._layout.compute(R.children,R.coord,{text:this._view.titleSize(R,O)});
for(var Q=0;
Q<R.children.length;
Q++){M=R.children[Q];
N=P.filter("["+t.attr("uid")+"='"+M.dataItem.uid+"']");
this._view.setItemSize(M,N);
this._resizeItems(M,N);
}}},setOptions:function(N){var M=N.dataSource;
N.dataSource=J;
this._originalOptions=m(this._originalOptions,N);
this.options=m({},this._originalOptions);
this._setLayout();
this._initTheme(this.options);
L.fn._setEvents.call(this,N);
if(M){this.setDataSource(q.create(M));
}if(this.options.autoBind){this.dataSource.fetch();
}}});
var F=e.extend({createRoot:function(N,O,M){N.coord={width:O,height:M,top:0,left:0};
},leaf:function(M){return !M.children;
},layoutChildren:function(P,M){var S=M.width*M.height;
var U=0,Q=[],O;
for(O=0;
O<P.length;
O++){Q[O]=parseFloat(P[O].value);
U+=Q[O];
}for(O=0;
O<Q.length;
O++){P[O].area=S*Q[O]/U;
}var R=this.layoutHorizontal()?M.height:M.width;
var N=[P[0]];
var T=P.slice(1);
this.squarify(T,N,R,M);
},squarify:function(O,N,P,M){this.computeDim(O,N,P,M);
},computeDim:function(R,P,S,M){if(R.length+P.length==1){var N=R.length==1?R:P;
this.layoutLast(N,S,M);
return;
}if(R.length>=2&&P.length===0){P=[R[0]];
R=R.slice(1);
}if(R.length===0){if(P.length>0){this.layoutRow(P,S,M);
}return;
}var O=R[0];
if(this.worstAspectRatio(P,S)>=this.worstAspectRatio([O].concat(P),S)){this.computeDim(R.slice(1),P.concat([O]),S,M);
}else{var Q=this.layoutRow(P,S,M);
this.computeDim(R,[],Q.dim,Q);
}},layoutLast:function(N,O,M){N[0].coord=M;
},layoutRow:function(N,O,M){if(this.layoutHorizontal()){return this.layoutV(N,O,M);
}else{return this.layoutH(N,O,M);
}},orientation:"h",layoutVertical:function(){return this.orientation==="v";
},layoutHorizontal:function(){return this.orientation==="h";
},layoutChange:function(){this.orientation=this.layoutVertical()?"h":"v";
},worstAspectRatio:function(P,S){if(!P||P.length===0){return v;
}var N=0,Q=0,R=v;
for(var O=0;
O<P.length;
O++){var M=P[O].area;
N+=M;
R=R<M?R:M;
Q=Q>M?Q:M;
}return u.max(S*S*Q/(N*N),N*N/(S*S*R));
},compute:function(M,P,N){if(!(P.width>=P.height&&this.layoutHorizontal())){this.layoutChange();
}if(M&&M.length>0){var O={width:P.width,height:P.height-N.text,top:0,left:0};
this.layoutChildren(M,O);
}},layoutV:function(Q,T,N){var S=this._totalArea(Q),R=0;
T=C(S/T);
for(var P=0;
P<Q.length;
P++){var O=C(Q[P].area/T);
Q[P].coord={height:O,width:T,top:N.top+R,left:N.left};
R+=O;
}var M={height:N.height,width:N.width-T,top:N.top,left:N.left+T};
M.dim=u.min(M.width,M.height);
if(M.dim!=M.height){this.layoutChange();
}return M;
},layoutH:function(Q,U,N){var T=this._totalArea(Q);
var O=C(T/U),S=N.top,R=0;
for(var P=0;
P<Q.length;
P++){Q[P].coord={height:O,width:C(Q[P].area/O),top:S,left:N.left+R};
R+=Q[P].coord.width;
}var M={height:N.height-O,width:N.width,top:N.top+O,left:N.left};
M.dim=u.min(M.width,M.height);
if(M.dim!=M.width){this.layoutChange();
}return M;
},_totalArea:function(N){var O=0;
for(var M=0;
M<N.length;
M++){O+=N[M].area;
}return O;
}});
var G=e.extend({init:function(N,M){this.options=m({},this.options,M);
this.treeMap=N;
this.element=b(N.element);
this.offset=0;
},titleSize:function(N,M){var O=M.children(".k-treemap-title");
return O.height();
},htmlSize:function(N){var O=this._getByUid(N.dataItem.uid);
var M={text:0};
if(N.children){this._clean(O);
var P=this._getText(N);
if(P){var Q=this._createTitle(N);
O.append(Q);
this._compile(Q,N.dataItem);
M.text=Q.height();
}O.append(this._createWrap());
this.offset=(O.outerWidth()-O.innerWidth())/2;
}return M;
},_compile:function(N,M){this.treeMap.angular("compile",function(){return{elements:N,data:[{dataItem:M}]};
});
},_getByUid:function(M){return this.element.find(".k-treemap-tile["+t.attr("uid")+"='"+M+"']");
},render:function(Q){var R=this._getByUid(Q.dataItem.uid);
var M=Q.children;
if(M){var S=R.find(".k-treemap-wrap");
for(var O=0;
O<M.length;
O++){var P=M[O];
var N=this._createLeaf(P);
S.append(N);
this._compile(N.children(),P.dataItem);
this.treeMap.trigger(s,{element:N});
}}},createRoot:function(N){var M=this._createLeaf(N);
this.element.append(M);
this._compile(M.children(),N.dataItem);
this.treeMap.trigger(s,{element:M});
},_clean:function(M){this.treeMap.angular("cleanup",function(){return{elements:M.children(":not(.k-treemap-wrap)")};
});
M.css("background-color","");
M.removeClass("k-leaf");
M.removeClass("k-inverse");
M.empty();
},_createLeaf:function(M){return this._createTile(M).css("background-color",M.color).addClass("k-leaf").toggleClass("k-inverse",this._tileColorBrightness(M)>180).append(b("<div></div>").html(this._getText(M)));
},_createTile:function(M){var N=b("<div class='k-treemap-tile'></div>");
this.setItemSize(M,N);
if(n(M.dataItem)&&n(M.dataItem.uid)){N.attr(t.attr("uid"),M.dataItem.uid);
}return N;
},_itemCoordinates:function(N){var M={width:N.coord.width,height:N.coord.height,left:N.coord.left,top:N.coord.top};
if(M.left&&this.offset){M.width+=this.offset*2;
}else{M.width+=this.offset;
}if(M.top){M.height+=this.offset*2;
}else{M.height+=this.offset;
}return M;
},setItemSize:function(O,N){var M=this._itemCoordinates(O);
N.css({width:M.width,height:M.height,left:M.left,top:M.top});
},_getText:function(M){var N=M.text;
if(this.options.template){N=this._renderTemplate(M);
}return N;
},_renderTemplate:function(M){var N=H(this.options.template);
return N({dataItem:M.dataItem,text:M.text});
},_createTitle:function(M){return b("<div class='k-treemap-title'></div>").append(b("<div></div>").html(this._getText(M)));
},_createWrap:function(){return b("<div class='k-treemap-wrap'></div>");
},_tileColorBrightness:function(M){return f(M.color);
}});
var D=e.extend({createRoot:function(N,P,M,O){N.coord={width:P,height:M,top:0,left:0};
N.vertical=O;
},init:function(M){this.vertical=M;
this.quotient=M?1:0;
},compute:function(M,Q,O){if(M.length>0){var R=Q.width;
var N=Q.height;
if(this.vertical){N-=O.text;
}else{R-=O.text;
}var P={width:R,height:N,top:0,left:0};
this.layoutChildren(M,P);
}},layoutChildren:function(P,M){var R=M.width*M.height;
var S=0;
var Q=[];
var N;
for(N=0;
N<P.length;
N++){var O=P[N];
Q[N]=parseFloat(P[N].value);
S+=Q[N];
O.vertical=this.vertical;
}for(N=0;
N<Q.length;
N++){P[N].area=R*Q[N]/S;
}this.sliceAndDice(P,M);
},sliceAndDice:function(N,M){var O=this._totalArea(N);
if(N[0].level%2===this.quotient){this.layoutHorizontal(N,M,O);
}else{this.layoutVertical(N,M,O);
}},layoutHorizontal:function(P,M,R){var Q=0;
for(var N=0;
N<P.length;
N++){var O=P[N];
var S=O.area/(R/M.width);
O.coord={height:M.height,width:S,top:M.top,left:M.left+Q};
Q+=S;
}},layoutVertical:function(Q,M,S){var R=0;
for(var O=0;
O<Q.length;
O++){var P=Q[O];
var N=P.area/(S/M.height);
P.coord={height:N,width:M.width,top:M.top+R,left:M.left};
R+=N;
}},_totalArea:function(N){var O=0;
for(var M=0;
M<N.length;
M++){O+=N[M].area;
}return O;
}});
var E=G.extend({htmlSize:function(N){var O=this._getByUid(N.dataItem.uid);
var M={text:0,offset:0};
if(N.children){this._clean(O);
var P=this._getText(N);
if(P){var Q=this._createTitle(N);
O.append(Q);
this._compile(Q,N.dataItem);
if(N.vertical){M.text=Q.height();
}else{M.text=Q.width();
}}O.append(this._createWrap());
this.offset=(O.outerWidth()-O.innerWidth())/2;
}return M;
},titleSize:function(N,M){var O;
if(N.vertical){O=M.children(".k-treemap-title").height();
}else{O=M.children(".k-treemap-title-vertical").width();
}return O;
},_createTitle:function(M){var N;
if(M.vertical){N=b("<div class='k-treemap-title'></div>");
}else{N=b("<div class='k-treemap-title-vertical'></div>");
}return N.append(b("<div></div>").html(this._getText(M)));
}});
function o(M,O){if(O===null){return O;
}var N=p(M,true);
return N(O);
}function n(M){return typeof M!==K;
}function h(S,Q,P){var T=A(S);
var R=A(Q);
var O=f(S)-f(Q)<0;
var M=[];
M.push(S);
for(var N=0;
N<P;
N++){var U={r:g(T.r,R.r,N,P,O),g:g(T.g,R.g,N,P,O),b:g(T.b,R.b,N,P,O)};
M.push(c(U));
}M.push(Q);
return M;
}function g(T,R,O,Q,P){var U=u.min(u.abs(T),u.abs(R));
var S=u.max(u.abs(T),u.abs(R));
var V=(S-U)/(Q+1);
var N=V*(O+1);
var M;
if(P){M=U+N;
}else{M=S-N;
}return M;
}function c(M){return"#"+l(M.r)+l(M.g)+l(M.b);
}function A(M){M=M.replace("#","");
var N=i(M);
return{r:B(N.r),g:B(N.g),b:B(N.b)};
}function l(M){var N=u.round(M).toString(16).toUpperCase();
if(N.length===1){N="0"+N;
}return N;
}function i(M){var N=M.length;
var O={};
if(N===3){O.r=M[0];
O.g=M[1];
O.b=M[2];
}else{O.r=M.substring(0,2);
O.g=M.substring(2,4);
O.b=M.substring(4,6);
}return O;
}function B(M){return parseInt(M.toString(16),16);
}function f(N){var M=0;
if(N){N=A(N);
M=u.sqrt(0.241*N.r*N.r+0.691*N.g*N.g+0.06800000000000001*N.b*N.b);
}return M;
}function C(N){var M=u.pow(10,4);
return u.round(N*M)/M;
}k.ui.plugin(I);
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

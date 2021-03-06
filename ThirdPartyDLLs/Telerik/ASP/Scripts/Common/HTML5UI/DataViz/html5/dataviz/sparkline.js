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
(function(b,a){a("kendo.dataviz.sparkline",["kendo.dataviz.chart"],b);
}(function(){var a={id:"dataviz.sparkline",name:"Sparkline",category:"dataviz",description:"Sparkline widgets.",depends:["dataviz.chart"]};
(function(b,w){var m=window.kendo,g=m.dataviz,e=g.ui.Chart,q=m.data.ObservableArray,t=g.SharedTooltip,j=m.deepExtend,l=b.isArray,s=b.proxy,k=g.inArray,o=Math;
var f="k-",h=150,i=150,c="bar",d="bullet",r="pie",n="leave",p=[c,d];
var u=e.extend({init:function(z,C){var y=this,B=y.stage=b("<span />"),A=C||{};
z=b(z).addClass(f+"sparkline").empty().append(B);
y._initialWidth=o.floor(z.width());
A=x(A);
if(l(A)||A instanceof q){A={seriesDefaults:{data:A}};
}if(!A.series){A.series=[{data:x(A.data)}];
}j(A,{seriesDefaults:{type:A.type}});
if(k(A.series[0].type,p)||k(A.seriesDefaults.type,p)){A=j({},{categoryAxis:{crosshair:{visible:false}}},A);
}e.fn.init.call(y,z,A);
},options:{name:"Sparkline",chartArea:{margin:2},axisDefaults:{visible:false,majorGridLines:{visible:false},valueAxis:{narrowRange:true}},seriesDefaults:{type:"line",area:{line:{width:0.5}},bar:{stack:true},padding:2,width:0.5,overlay:{gradient:null},highlight:{visible:false},border:{width:0},markers:{size:2,visible:false}},tooltip:{visible:true,shared:true},categoryAxis:{crosshair:{visible:true,tooltip:{visible:false}}},legend:{visible:false},transitions:false,pointWidth:5,panes:[{clip:false}]},_modelOptions:function(){var y=this,z=y.options,A,D=y._initialWidth,C=y.stage;
y.stage.children().hide();
var B=b("<span>&nbsp;</span>");
y.stage.append(B);
A=j({width:D?D:y._autoWidth(),height:C.height(),transitions:z.transitions},z.chartArea,{inline:true,align:false});
C.css({width:A.width,height:A.height});
B.remove();
y.stage.children().show();
y.surface.resize();
return A;
},_createTooltip:function(){var y=this,A=y.options,z=y.element,B;
if(y._sharedTooltip()){B=new v(z,y._plotArea,A.tooltip);
}else{B=e.fn._createTooltip.call(y);
}B.bind(n,s(y._tooltipleave,y));
return B;
},_surfaceWrap:function(){return this.stage;
},_autoWidth:function(){var y=this,D=y.options,C=g.getSpacing(D.chartArea.margin),E=D.series,A=y.dataSource.total(),F=0,G,B,z;
for(B=0;
B<E.length;
B++){z=E[B];
if(z.type===c){return h;
}if(z.type===d){return i;
}if(z.type===r){return y.stage.height();
}if(z.data){F=o.max(F,z.data.length);
}}G=o.max(A,F)*D.pointWidth;
if(G>0){G+=C.left+C.right;
}return G;
}});
var v=t.extend({options:{animation:{duration:0}},_anchor:function(z,B){var y=t.fn._anchor.call(this,z,B);
var A=this._measure();
y.y=-A.height-this.options.offset;
return y;
},_hideElement:function(){if(this.element){this.element.hide().remove();
}}});
function x(y){return typeof y==="number"?[y]:y;
}g.ui.plugin(u);
j(g,{SparklineSharedTooltip:v});
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

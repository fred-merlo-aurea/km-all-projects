(function(b,a){a("kendo.columnsorter",["kendo.core"],b);
}(function(){var a={id:"columnsorter",name:"Column Sorter",category:"framework",depends:["core"],advanced:true};
(function(b,o){var i=window.kendo;
var n=i.ui;
var p=n.Widget;
var g="dir";
var d="asc";
var k="single";
var h="field";
var f="desc";
var l=".kendoColumnSorter";
var m=".k-link";
var c="aria-sort";
var j=b.proxy;
var e=p.extend({init:function(q,s){var t=this,r;
p.fn.init.call(t,q,s);
t._refreshHandler=j(t.refresh,t);
t.dataSource=t.options.dataSource.bind("change",t._refreshHandler);
r=t.element.find(m);
if(!r[0]){r=t.element.wrapInner('<a class="k-link" href="#"/>').find(m);
}t.link=r;
t.element.on("click"+l,j(t._click,t));
},options:{name:"ColumnSorter",mode:k,allowUnsort:true,compare:null,filter:""},events:["change"],destroy:function(){var q=this;
p.fn.destroy.call(q);
q.element.off(l);
q.dataSource.unbind("change",q._refreshHandler);
q._refreshHandler=q.element=q.link=q.dataSource=null;
},refresh:function(){var x=this,w=x.dataSource.sort()||[],u,v,q,r,s=x.element,t=s.attr(i.attr(h));
s.removeAttr(i.attr(g));
s.removeAttr(c);
for(u=0,v=w.length;
u<v;
u++){q=w[u];
if(t==q.field){s.attr(i.attr(g),q.dir);
}}r=s.attr(i.attr(g));
s.find(".k-i-arrow-n,.k-i-arrow-s").remove();
if(r===d){b('<span class="k-icon k-i-arrow-n" />').appendTo(x.link);
s.attr(c,"ascending");
}else{if(r===f){b('<span class="k-icon k-i-arrow-s" />').appendTo(x.link);
s.attr(c,"descending");
}}},_click:function(s){var z=this,t=z.element,u=t.attr(i.attr(h)),r=t.attr(i.attr(g)),x=z.options,q=z.options.compare===null?o:z.options.compare,y=z.dataSource.sort()||[],v,w;
s.preventDefault();
if(x.filter&&!t.is(x.filter)){return;
}if(r===d){r=f;
}else{if(r===f&&x.allowUnsort){r=o;
}else{r=d;
}}if(this.trigger("change",{sort:{field:u,dir:r,compare:q}})){return;
}if(x.mode===k){y=[{field:u,dir:r,compare:q}];
}else{if(x.mode==="multiple"){for(v=0,w=y.length;
v<w;
v++){if(y[v].field===u){y.splice(v,1);
break;
}}y.push({field:u,dir:r,compare:q});
}}this.dataSource.sort(y);
}});
n.plugin(e);
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

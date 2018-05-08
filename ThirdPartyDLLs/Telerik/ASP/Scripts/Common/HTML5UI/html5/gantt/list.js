(function(b,a){a("kendo.gantt.list",["kendo.dom","kendo.touch","kendo.draganddrop","kendo.columnsorter","kendo.datetimepicker","kendo.editable"],b);
}(function(){var a={id:"gantt.list",name:"Gantt List",category:"web",description:"The Gantt List",depends:["dom","touch","draganddrop","columnsorter","datetimepicker","editable"],hidden:true};
(function(b){var k=window.kendo;
var l=k.dom;
var m=l.element;
var n=l.text;
var d=k.support.browser;
var r=k.support.mobileOS;
var x=k.ui;
var y=x.Widget;
var h=b.extend;
var q=b.map;
var j=b.isFunction;
var t=d.msie&&d.version<9;
var o=k.keys;
var w={title:"Title",start:"Start Time",end:"End Time",percentComplete:"% Done",parentId:"Predecessor ID",id:"ID",orderId:"Order ID"};
var v="string";
var s=".kendoGanttList";
var e="click";
var g=".";
var u="<table style='visibility: hidden;'><tbody><tr style='height:{0}'><td>&nbsp;</td></tr></tbody></table>";
var p={wrapper:"k-treelist k-grid k-widget",header:"k-header",alt:"k-alt",rtl:"k-rtl",editCell:"k-edit-cell",group:"k-treelist-group",gridHeader:"k-grid-header",gridHeaderWrap:"k-grid-header-wrap",gridContent:"k-grid-content",gridContentWrap:"k-grid-content",selected:"k-state-selected",icon:"k-icon",iconCollapse:"k-i-collapse",iconExpand:"k-i-expand",iconHidden:"k-i-none",iconPlaceHolder:"k-icon k-i-none",input:"k-input",link:"k-link",resizeHandle:"k-resize-handle",resizeHandleInner:"k-resize-handle-inner",dropPositions:"k-i-insert-top k-i-insert-bottom k-i-add k-i-insert-middle",dropTop:"k-i-insert-top",dropBottom:"k-i-insert-bottom",dropAdd:"k-i-add",dropMiddle:"k-i-insert-middle",dropDenied:"k-i-denied",dragStatus:"k-drag-status",dragClue:"k-drag-clue",dragClueText:"k-clue-text"};
function f(C){var D=[];
var z=C.className;
for(var A=0,B=C.level;
A<B;
A++){D.push(m("span",{className:z}));
}return D;
}function c(){var z=k._activeElement();
if(z&&z.nodeName.toLowerCase()!=="body"){b(z).blur();
}}var i=x.GanttList=y.extend({init:function(z,A){y.fn.init.call(this,z,A);
if(this.options.columns.length===0){this.options.columns.push("title");
}this.dataSource=this.options.dataSource;
this._columns();
this._layout();
this._domTrees();
this._header();
this._sortable();
this._editable();
this._selectable();
this._draggable();
this._resizable();
this._attachEvents();
this._adjustHeight();
this.bind("render",function(){var B;
var C;
if(this.options.resizable){B=this.header.find("col");
C=this.content.find("col");
this.header.find("th").not(":last").each(function(D){var E=b(this).outerWidth();
B.eq(D).width(E);
C.eq(D).width(E);
});
B.last().css("width","auto");
C.last().css("width","auto");
}},true);
},_adjustHeight:function(){this.content.height(this.element.height()-this.header.parent().outerHeight());
},destroy:function(){y.fn.destroy.call(this);
if(this._reorderDraggable){this._reorderDraggable.destroy();
}if(this._tableDropArea){this._tableDropArea.destroy();
}if(this._contentDropArea){this._contentDropArea.destroy();
}if(this._columnResizable){this._columnResizable.destroy();
}if(this.touch){this.touch.destroy();
}if(this.timer){clearTimeout(this.timer);
}this.content.off(s);
this.header.find("thead").off(s);
this.header.find(g+i.link).off(s);
this.header=null;
this.content=null;
this.levels=null;
k.destroy(this.element);
},options:{name:"GanttList",selectable:true,editable:true,resizable:false},_attachEvents:function(){var A=this;
var z=i.styles;
A.content.on(e+s,"td > span."+z.icon+":not(."+z.iconHidden+")",function(B){var C=b(this);
var D=A._modelFromElement(C);
D.set("expanded",!D.get("expanded"));
B.stopPropagation();
});
},_domTrees:function(){this.headerTree=new l.Tree(this.header[0]);
this.contentTree=new l.Tree(this.content[0]);
},_columns:function(){var z=this.options.columns;
var A=function(){this.field="";
this.title="";
this.editable=false;
this.sortable=false;
};
this.columns=q(z,function(B){B=typeof B===v?{field:B,title:w[B]}:B;
return h(new A(),B);
});
},_layout:function(){var D=this;
var C=this.options;
var A=this.element;
var B=i.styles;
var z=function(){var F=typeof C.rowHeight===v?C.rowHeight:C.rowHeight+"px";
var G=b(k.format(u,F));
var E;
D.content.append(G);
E=G.find("tr").outerHeight();
G.remove();
return E;
};
A.addClass(B.wrapper).append("<div class='"+B.gridHeader+"'><div class='"+B.gridHeaderWrap+"'></div></div>").append("<div class='"+B.gridContentWrap+"'></div>");
this.header=A.find(g+B.gridHeaderWrap);
this.content=A.find(g+B.gridContent);
if(C.rowHeight){this._rowHeight=z();
}},_header:function(){var A=this.headerTree;
var z;
var C;
var B;
z=m("colgroup",null,this._cols());
C=m("thead",{role:"rowgroup"},[m("tr",{role:"row"},this._ths())]);
B=m("table",{style:{minWidth:this.options.listWidth+"px"},role:"grid"},[z,C]);
A.render([B]);
},_render:function(C){var z;
var D;
var A;
var B={style:{minWidth:this.options.listWidth+"px"},tabIndex:0,role:"treegrid"};
if(this._rowHeight){B.style.height=C.length*this._rowHeight+"px";
}this.levels=[{field:null,value:0}];
z=m("colgroup",null,this._cols());
D=m("tbody",{role:"rowgroup"},this._trs(C));
A=m("table",B,[z,D]);
this.contentTree.render([A]);
this.trigger("render");
},_ths:function(){var B=this.columns;
var A;
var z;
var E=[];
for(var C=0,D=B.length;
C<D;
C++){A=B[C];
z={"data-field":A.field,"data-title":A.title,className:i.styles.header,role:"columnheader"};
E.push(m("th",z,[n(A.title)]));
}if(this.options.resizable){E.push(m("th",{className:i.styles.header,role:"columnheader"}));
}return E;
},_cols:function(){var B=this.columns;
var A;
var E;
var F;
var z=[];
for(var C=0,D=B.length;
C<D;
C++){A=B[C];
F=A.width;
if(F&&parseInt(F,10)!==0){E={style:{width:typeof F===v?F:F+"px"}};
}else{E=null;
}z.push(m("col",E,[]));
}if(this.options.resizable){z.push(m("col",{style:{width:"1px"}}));
}return z;
},_trs:function(H){var G;
var F=[];
var z;
var A=[];
var D;
var E=i.styles;
for(var B=0,C=H.length;
B<C;
B++){G=H[B];
D=this._levels({idx:G.parentId,id:G.id,summary:G.summary});
z={"data-uid":G.uid,"data-level":D,role:"row"};
if(G.summary){z["aria-expanded"]=G.expanded;
}if(B%2!==0){A.push(E.alt);
}if(G.summary){A.push(E.group);
}if(A.length){z.className=A.join(" ");
}F.push(this._tds({task:G,attr:z,level:D}));
A=[];
}return F;
},_tds:function(E){var z=[];
var B=this.columns;
var A;
for(var C=0,D=B.length;
C<D;
C++){A=B[C];
z.push(this._td({task:E.task,column:A,level:E.level}));
}if(this.options.resizable){z.push(m("td",{role:"gridcell"}));
}return m("tr",E.attr,z);
},_td:function(F){var z=[];
var G=this.options.resourcesField;
var E=i.styles;
var H=F.task;
var A=F.column;
var I=H.get(A.field);
var B;
var D;
if(A.field==G){I=I||[];
B=[];
for(var C=0;
C<I.length;
C++){B.push(k.format("{0} [{1}]",I[C].get("name"),I[C].get("formatedValue")));
}B=B.join(", ");
}else{B=A.format?k.format(A.format,I):I;
}if(A.field==="title"){z=f({level:F.level,className:E.iconPlaceHolder});
z.push(m("span",{className:E.icon+" "+(H.summary?H.expanded?E.iconCollapse:E.iconExpand:E.iconHidden)}));
D=k.format("{0}, {1:P0}",B,H.percentComplete);
}z.push(m("span",{"aria-label":D},[n(B)]));
return m("td",{role:"gridcell"},z);
},_levels:function(F){var E=this.levels;
var D;
var G=F.summary;
var B=F.idx;
var A=F.id;
for(var z=0,C=E.length;
z<C;
z++){D=E[z];
if(D.field==B){if(G){E.push({field:A,value:D.value+1});
}return D.value;
}}},_sortable:function(){var H=this;
var F=this.options.resourcesField;
var C=this.columns;
var B;
var G;
var A=this.header.find("th["+k.attr("field")+"]");
var z;
for(var D=0,E=A.length;
D<E;
D++){B=C[D];
if(B.sortable&&B.field!==F){z=A.eq(D);
G=z.data("kendoColumnSorter");
if(G){G.destroy();
}z.attr("data-"+k.ns+"field",B.field).kendoColumnSorter({dataSource:this.dataSource,change:function(I){if(H.dataSource.total()===0||H.editable&&H.editable.trigger("validate")){I.preventDefault();
}}});
}}A=null;
},_selectable:function(){var A=this;
var z=this.options.selectable;
if(z){this.content.on(e+s,"tr",function(B){var C=b(this);
if(A.editable){A.editable.trigger("validate");
}if(!B.ctrlKey){A.select(C);
}else{A.clearSelection();
}});
}},select:function(B){var z=this.content.find(B);
var A=i.styles.selected;
if(z.length){z.siblings(g+A).removeClass(A).attr("aria-selected",false).end().addClass(A).attr("aria-selected",true);
this.trigger("change");
return;
}return this.content.find(g+A);
},clearSelection:function(){var z=this.select();
if(z.length){z.removeClass(i.styles.selected);
this.trigger("change");
}},_setDataSource:function(z){this.dataSource=z;
},_editable:function(){var E=this;
var z=this.options.editable;
var C=i.styles;
var B="span."+C.icon+":not("+C.iconHidden+")";
var A=function(){var F=E.editable;
if(F){if(F.end()){E._closeCell();
}else{F.trigger("validate");
}}};
var D=function(G){var F=b(G.currentTarget);
if(!F.hasClass(C.editCell)){c();
}};
if(!z||z.update===false){return;
}this._startEditHandler=function(G){var H=G.currentTarget?b(G.currentTarget):G;
var F=E._columnFromElement(H);
if(E.editable){return;
}if(F&&F.editable){E._editCell({cell:H,column:F});
}};
E.content.on("focusin"+s,function(){clearTimeout(E.timer);
E.timer=null;
}).on("focusout"+s,function(){E.timer=setTimeout(A,1);
}).on("keydown"+s,function(F){if(F.keyCode===o.ENTER){F.preventDefault();
}}).on("keyup"+s,function(G){var H=G.keyCode;
var F;
var I;
switch(H){case o.ENTER:c();
A();
break;
case o.ESC:if(E.editable){F=E._editableContainer;
I=E._modelFromElement(F);
if(!E.trigger("cancel",{model:I,cell:F})){E._closeCell(true);
}}break;
}});
if(!r){E.content.on("mousedown"+s,"td",function(F){D(F);
}).on("dblclick"+s,"td",function(F){if(!b(F.target).is(B)){E._startEditHandler(F);
}});
}else{E.touch=E.content.kendoTouch({filter:"td",touchstart:function(F){D(F.touch);
},doubletap:function(F){if(!b(F.touch.initialTouch).is(B)){E._startEditHandler(F.touch);
}}}).data("kendoTouch");
}},_editCell:function(K){var L=this.options.resourcesField;
var H=i.styles;
var B=K.cell;
var C=K.column;
var I=this._modelFromElement(B);
var J=this.dataSource._createNewModel(I.toJSON());
var F=J.fields[C.field]||J[C.field];
var M=F.validation;
var D=k.attr("type");
var A=k.attr("bind");
var G=k.attr("format");
var z={name:C.field,required:F.validation?F.validation.required===true:false};
var E;
if(C.field===L){C.editor(B,J);
return;
}this._editableContent=B.children().detach();
this._editableContainer=B;
B.data("modelCopy",J);
if((F.type==="date"||b.type(F)==="date")&&(!C.format||/H|m|s|F|g|u/.test(C.format))){z[A]="value:"+C.field;
z[D]="date";
if(C.format){z[G]=k._extractFormat(C.format);
}E=function(N,O){b('<input type="text"/>').attr(z).appendTo(N).kendoDateTimePicker({format:O.format});
};
}this.editable=B.addClass(H.editCell).kendoEditable({fields:{field:C.field,format:C.format,editor:C.editor||E},model:J,clearContainer:false}).data("kendoEditable");
if(M&&M.dateCompare&&j(M.dateCompare)&&M.message){b("<span "+k.attr("for")+'="'+C.field+'" class="k-invalid-msg"/>').hide().appendTo(B);
B.find("[name="+C.field+"]").attr(k.attr("dateCompare-msg"),M.message);
}this.editable.bind("validate",function(N){var O=this.element.find(":kendoFocusable:first").focus();
if(t){O.focus();
}N.preventDefault();
});
if(this.trigger("edit",{model:I,cell:B})){this._closeCell(true);
}},_closeCell:function(z){var E=i.styles;
var A=this._editableContainer;
var F=this._modelFromElement(A);
var B=this._columnFromElement(A);
var D=B.field;
var C=A.data("modelCopy");
var G={};
G[D]=C.get(D);
A.empty().removeData("modelCopy").removeClass(E.editCell).append(this._editableContent);
this.editable.unbind();
this.editable.destroy();
this.editable=null;
this._editableContainer=null;
this._editableContent=null;
if(!z){if(D==="start"){G.end=new Date(G.start.getTime()+F.duration());
}this.trigger("update",{task:F,updateInfo:G});
}},_draggable:function(){var M=this;
var E=null;
var F=true;
var G;
var J=i.styles;
var I=k.support.isRtl(this.element);
var K="tr["+k.attr("level")+" = 0]:last";
var z={};
var H=this.options.editable;
var B=function(){E=null;
G=null;
F=true;
z={};
};
var A=function(O){var N=O;
while(N){if(E.get("id")===N.get("id")){F=false;
break;
}N=M.dataSource.taskParent(N);
}};
var D=function(){var N=b(G).height();
var O=k.getOffset(G).top;
h(G,{beforeLimit:O+N*0.25,afterLimit:O+N*0.75});
};
var C=function(P){if(!G){return;
}var R=P.location;
var N=J.dropAdd;
var O="add";
var Q=parseInt(G.attr(k.attr("level")),10);
var S;
if(R<=G.beforeLimit){S=G.prev();
N=J.dropTop;
O="insert-before";
}else{if(R>=G.afterLimit){S=G.next();
N=J.dropBottom;
O="insert-after";
}}if(S&&parseInt(S.attr(k.attr("level")),10)===Q){N=J.dropMiddle;
}z.className=N;
z.command=O;
};
var L=function(){return M._reorderDraggable.hint.children(g+J.dragStatus).removeClass(J.dropPositions);
};
if(!H||H.reorder===false||H.update===false){return;
}this._reorderDraggable=this.content.kendoDraggable({distance:10,holdToDrag:r,group:"listGroup",filter:"tr[data-uid]",ignore:g+J.input,hint:function(N){return b('<div class="'+J.header+" "+J.dragClue+'"/>').css({width:300,paddingLeft:N.css("paddingLeft"),paddingRight:N.css("paddingRight"),lineHeight:N.height()+"px",paddingTop:N.css("paddingTop"),paddingBottom:N.css("paddingBottom")}).append('<span class="'+J.icon+" "+J.dragStatus+'" /><span class="'+J.dragClueText+'"/>');
},cursorOffset:{top:-20,left:0},container:this.content,dragstart:function(N){var O=M.editable;
if(O&&O.reorder!==false&&O.trigger("validate")){N.preventDefault();
return;
}E=M._modelFromElement(N.currentTarget);
this.hint.children(g+J.dragClueText).text(E.get("title"));
if(I){this.hint.addClass(J.rtl);
}},drag:function(N){if(F){C(N.y);
L().addClass(z.className);
}},dragend:function(){B();
},dragcancel:function(){B();
}}).data("kendoDraggable");
this._tableDropArea=this.content.kendoDropTargetArea({distance:0,group:"listGroup",filter:"tr[data-uid]",dragenter:function(N){G=N.dropTarget;
A(M._modelFromElement(G));
D();
L().toggleClass(J.dropDenied,!F);
},dragleave:function(){F=true;
L();
},drop:function(){var O=M._modelFromElement(G);
var N=O.orderId;
var P={parentId:O.parentId};
if(F){switch(z.command){case"add":P.parentId=O.id;
break;
case"insert-before":if(O.parentId===E.parentId&&O.orderId>E.orderId){P.orderId=N-1;
}else{P.orderId=N;
}break;
case"insert-after":if(O.parentId===E.parentId&&O.orderId>E.orderId){P.orderId=N;
}else{P.orderId=N+1;
}break;
}M.trigger("update",{task:E,updateInfo:P});
}}}).data("kendoDropTargetArea");
this._contentDropArea=this.element.kendoDropTargetArea({distance:0,group:"listGroup",filter:g+J.gridContent,drop:function(){var O=M._modelFromElement(M.content.find(K));
var N=O.orderId;
var P={parentId:null,orderId:E.parentId!==null?N+1:N};
M.trigger("update",{task:E,updateInfo:P});
}}).data("kendoDropTargetArea");
},_resizable:function(){var B=this;
var z=i.styles;
var A=function(G){var M=b(G.currentTarget);
var K=B.resizeHandle;
var J=M.position();
var I=J.left;
var D=M.outerWidth();
var F=M.closest("div");
var E=G.clientX+b(window).scrollLeft();
var H=B.options.columnResizeHandleWidth;
I+=F.scrollLeft();
if(!K){K=B.resizeHandle=b('<div class="'+z.resizeHandle+'"><div class="'+z.resizeHandleInner+'" /></div>');
}var C=M.offset().left+D;
var L=E>C-H&&E<C+H;
if(!L){K.hide();
return;
}F.append(K);
K.show().css({top:J.top,left:I+D-H-1,height:M.outerHeight(),width:H*3}).data("th",M);
};
if(!this.options.resizable){return;
}if(this._columnResizable){this._columnResizable.destroy();
}this.header.find("thead").on("mousemove"+s,"th",A);
this._columnResizable=this.header.kendoResizable({handle:g+z.resizeHandle,start:function(E){var G=b(E.currentTarget).data("th");
var C="col:eq("+G.index()+")";
var F=B.header.find("table");
var D=B.content.find("table");
B.element.addClass("k-grid-column-resizing");
this.col=D.children("colgroup").find(C).add(F.find(C));
this.th=G;
this.startLocation=E.x.location;
this.columnWidth=G.outerWidth();
this.table=F.add(D);
this.totalWidth=this.table.width()-F.find("th:last").outerWidth();
},resize:function(D){var E=11;
var C=D.x.location-this.startLocation;
if(this.columnWidth+C<E){C=E-this.columnWidth;
}this.table.css({minWidth:this.totalWidth+C});
this.col.width(this.columnWidth+C);
},resizeend:function(){B.element.removeClass("k-grid-column-resizing");
var E=Math.floor(this.columnWidth);
var D=Math.floor(this.th.outerWidth());
var C=B.columns[this.th.index()];
B.trigger("columnResize",{column:C,oldWidth:E,newWidth:D});
this.table=this.col=this.th=null;
}}).data("kendoResizable");
},_modelFromElement:function(z){var B=z.closest("tr");
var A=this.dataSource.getByUid(B.attr(k.attr("uid")));
return A;
},_columnFromElement:function(z){var B=z.closest("td");
var C=B.parent();
var A=C.children().index(B);
return this.columns[A];
}});
h(true,x.GanttList,{styles:p});
}(window.kendo.jQuery));
return window.kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

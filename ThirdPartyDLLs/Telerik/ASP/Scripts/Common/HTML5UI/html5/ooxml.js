(function(b,a){a("kendo.ooxml",["kendo.core"],b);
}(function(){var a={id:"ooxml",name:"XLSX generation",category:"framework",advanced:true,depends:["core"]};
(function(b,u){var x='<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>';
var k=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:dcmitype="http://purl.org/dc/dcmitype/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><dc:creator>${creator}</dc:creator><cp:lastModifiedBy>${lastModifiedBy}</cp:lastModifiedBy><dcterms:created xsi:type="dcterms:W3CDTF">${created}</dcterms:created><dcterms:modified xsi:type="dcterms:W3CDTF">${modified}</dcterms:modified></cp:coreProperties>');
var d=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties" xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes"><Application>Microsoft Excel</Application><DocSecurity>0</DocSecurity><ScaleCrop>false</ScaleCrop><HeadingPairs><vt:vector size="2" baseType="variant"><vt:variant><vt:lpstr>Worksheets</vt:lpstr></vt:variant><vt:variant><vt:i4>${sheets.length}</vt:i4></vt:variant></vt:vector></HeadingPairs><TitlesOfParts><vt:vector size="${sheets.length}" baseType="lpstr"># for (var idx = 0; idx < sheets.length; idx++) { ## if (sheets[idx].options.title) { #<vt:lpstr>${sheets[idx].options.title}</vt:lpstr># } else { #<vt:lpstr>Sheet${idx+1}</vt:lpstr># } ## } #</vt:vector></TitlesOfParts><LinksUpToDate>false</LinksUpToDate><SharedDoc>false</SharedDoc><HyperlinksChanged>false</HyperlinksChanged><AppVersion>14.0300</AppVersion></Properties>');
var i=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" /><Default Extension="xml" ContentType="application/xml" /><Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml" /><Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/><Override PartName="/xl/sharedStrings.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml"/># for (var idx = 1; idx <= count; idx++) { #<Override PartName="/xl/worksheets/sheet${idx}.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml" /># } #<Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml" /><Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml" /></Types>');
var H=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><fileVersion appName="xl" lastEdited="5" lowestEdited="5" rupBuild="9303" /><workbookPr defaultThemeVersion="124226" /><bookViews><workbookView xWindow="240" yWindow="45" windowWidth="18195" windowHeight="7995" /></bookViews><sheets># for (var idx = 0; idx < sheets.length; idx++) { ## var options = sheets[idx].options; ## var name = options.name || options.title ## if (name) { #<sheet name="${name}" sheetId="${idx+1}" r:id="rId${idx+1}" /># } else { #<sheet name="Sheet${idx+1}" sheetId="${idx+1}" r:id="rId${idx+1}" /># } ## } #</sheets># if (filterNames.length || userNames.length) { #<definedNames> # for (var di = 0; di < filterNames.length; di++) { #<definedName name="_xlnm._FilterDatabase" hidden="1" localSheetId="${filterNames[di].localSheetId}">${filterNames[di].name}!$${filterNames[di].from}:$${filterNames[di].to}</definedName> # } # # for (var i = 0; i < userNames.length; ++i) { #<definedName name="${userNames[i].name}" hidden="${userNames[i].hidden ? 1 : 0}" # if (userNames[i].localSheetId != null) { # localSheetId="${userNames[i].localSheetId}" # } #>${userNames[i].value}</definedName> # } #</definedNames># } #<calcPr calcId="145621" /></workbook>');
var K=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:x14ac="http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac" mc:Ignorable="x14ac"><dimension ref="A1" /><sheetViews><sheetView #if(index==0) {# tabSelected="1" #}# workbookViewId="0" #if (showGridLines === false) {# showGridLines="0" #}#># if (frozenRows || frozenColumns) { #<pane state="frozen"# if (frozenColumns) { # xSplit="${frozenColumns}"# } ## if (frozenRows) { # ySplit="${frozenRows}"# } # topLeftCell="${String.fromCharCode(65 + (frozenColumns || 0))}${(frozenRows || 0)+1}"/># } #</sheetView></sheetViews><sheetFormatPr x14ac:dyDescent="0.25" defaultRowHeight="#= defaults.rowHeight ? defaults.rowHeight * 0.75 : 15 #" # if (defaults.columnWidth) { # defaultColWidth="#= kendo.ooxml.toWidth(defaults.columnWidth) #" # } # /># if (columns && columns.length > 0) { #<cols># for (var ci = 0; ci < columns.length; ci++) { ## var column = columns[ci]; ## var columnIndex = typeof column.index === "number" ? column.index + 1 : (ci + 1); ## if (column.width === 0) { #<col min="${columnIndex}" max="${columnIndex}" hidden="1" customWidth="1" /># } else if (column.width) { #<col min="${columnIndex}" max="${columnIndex}" customWidth="1"# if (column.autoWidth) { # width="${((column.width*7+5)/7*256)/256}" bestFit="1"# } else { # width="#= kendo.ooxml.toWidth(column.width) #" # } #/># } ## } #</cols># } #<sheetData># for (var ri = 0; ri < data.length; ri++) { ## var row = data[ri]; ## var rowIndex = typeof row.index === "number" ? row.index + 1 : (ri + 1); #<row r="${rowIndex}" x14ac:dyDescent="0.25" # if (row.height) { # ht="#= kendo.ooxml.toHeight(row.height) #" customHeight="1" # } # ># for (var ci = 0; ci < row.data.length; ci++) { ## var cell = row.data[ci];#<c r="#=cell.ref#"# if (cell.style) { # s="#=cell.style#" # } ## if (cell.type) { # t="#=cell.type#"# } #># if (cell.formula != null) { #<f>${cell.formula}</f># } ## if (cell.value != null) { #<v>${cell.value}</v># } #</c># } #</row># } #</sheetData># if (hyperlinks.length) { #<hyperlinks># for (var hi = 0; hi < hyperlinks.length; hi++) { #<hyperlink ref="${hyperlinks[hi].ref}" r:id="rId${hi}"/># } #</hyperlinks># } ## if (filter) { #<autoFilter ref="${filter.from}:${filter.to}"/># } ## if (mergeCells.length) { #<mergeCells count="${mergeCells.length}"># for (var ci = 0; ci < mergeCells.length; ci++) { #<mergeCell ref="${mergeCells[ci]}"/># } #</mergeCells># } #<pageMargins left="0.7" right="0.7" top="0.75" bottom="0.75" header="0.3" footer="0.3" /></worksheet>');
var I=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"># for (var idx = 1; idx <= count; idx++) { #<Relationship Id="rId${idx}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet${idx}.xml" /># } #<Relationship Id="rId${count+1}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml" /><Relationship Id="rId${count+2}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings" Target="sharedStrings.xml" /></Relationships>');
var L=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"># for (var i = 0; i < hyperlinks.length; i++) { #<Relationship Id="rId${i}" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink" Target="${hyperlinks[i].target}" TargetMode="External" /># } #</Relationships>');
var y=u.template('<?xml version="1.0" encoding="UTF-8" standalone="yes"?>\r\n<sst xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" count="${count}" uniqueCount="${uniqueCount}"># for (var index in indexes) { #<si><t>${index.substring(1)}</t></si># } #</sst>');
var D=u.template('<?xml version="1.0" encoding="UTF-8"?><styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" mc:Ignorable="x14ac" xmlns:x14ac="http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac"><numFmts count="${formats.length}"># for (var fi = 0; fi < formats.length; fi++) { ## var format = formats[fi]; #<numFmt formatCode="${format.format}" numFmtId="${165+fi}" /># } #</numFmts><fonts count="${fonts.length+1}" x14ac:knownFonts="1"><font><sz val="11" /><color theme="1" /><name val="Calibri" /><family val="2" /><scheme val="minor" /></font># for (var fi = 0; fi < fonts.length; fi++) { ## var font = fonts[fi]; #<font># if (font.fontSize) { #<sz val="${font.fontSize}" /># } else { #<sz val="11" /># } ## if (font.bold) { #<b/># } ## if (font.italic) { #<i/># } ## if (font.underline) { #<u/># } ## if (font.color) { #<color rgb="${font.color}" /># } else { #<color theme="1" /># } ## if (font.fontFamily) { #<name val="${font.fontFamily}" /><family val="2" /># } else { #<name val="Calibri" /><family val="2" /><scheme val="minor" /># } #</font># } #</fonts><fills count="${fills.length+2}"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill># for (var fi = 0; fi < fills.length; fi++) { ## var fill = fills[fi]; ## if (fill.background) { #<fill><patternFill patternType="solid"><fgColor rgb="${fill.background}"/></patternFill></fill># } ## } #</fills><borders count="${borders.length+1}"><border><left/><right/><top/><bottom/><diagonal/></border># for (var bi = 0; bi < borders.length; bi++) { ##= kendo.ooxml.borderTemplate(borders[bi]) ## } #</borders><cellStyleXfs count="1"><xf borderId="0" fillId="0" fontId="0" /></cellStyleXfs><cellXfs count="${styles.length+1}"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/># for (var si = 0; si < styles.length; si++) { ## var style = styles[si]; #<xf xfId="0"# if (style.fontId) { # fontId="${style.fontId}" applyFont="1"# } ## if (style.fillId) { # fillId="${style.fillId}" applyFill="1"# } ## if (style.numFmtId) { # numFmtId="${style.numFmtId}" applyNumberFormat="1"# } ## if (style.textAlign || style.verticalAlign || style.wrap) { # applyAlignment="1"# } ## if (style.borderId) { # borderId="${style.borderId}" applyBorder="1"# } #># if (style.textAlign || style.verticalAlign || style.wrap) { #<alignment# if (style.textAlign) { # horizontal="${style.textAlign}"# } ## if (style.verticalAlign) { # vertical="${style.verticalAlign}"# } ## if (style.wrap) { # wrapText="1"# } #/># } #</xf># } #</cellXfs><cellStyles count="1"><cellStyle name="Normal" xfId="0" builtinId="0"/></cellStyles><dxfs count="0" /><tableStyles count="0" defaultTableStyle="TableStyleMedium2" defaultPivotStyle="PivotStyleMedium9" /></styleSheet>');
function v(M){var N=Math.floor(M/26)-1;
return(N>=0?v(N):"")+String.fromCharCode(65+M%26);
}function w(N,M){return v(M)+(N+1);
}function c(N,M){return v(M)+"$"+(N+1);
}function p(N){var M=N.frozenRows||(N.freezePane||{}).rowSplit||1;
return M-1;
}function F(M){return(M/7*100+0.5)/100;
}function E(M){return M*0.75;
}function C(M){return(M+"").replace(/[\x00-\x08]/g,"").replace(/\n/g,"\r\n");
}var l=new Date(1900,0,0);
var J=u.Class.extend({init:function(N,O,P,M){this.options=N;
this._strings=O;
this._styles=P;
this._borders=M;
},relsToXML:function(){var M=this.options.hyperlinks||[];
if(!M.length){return"";
}return L({hyperlinks:M});
},toXML:function(P){var Q=this.options.mergedCells||[];
var R=this.options.rows||[];
var M=r(R,Q);
this._readCells(M);
var N=this.options.filter;
if(N&&typeof N.from==="number"&&typeof N.to==="number"){N={from:w(p(this.options),N.from),to:w(p(this.options),N.to)};
}var O=this.options.freezePane||{};
return K({frozenColumns:this.options.frozenColumns||O.colSplit,frozenRows:this.options.frozenRows||O.rowSplit,columns:this.options.columns,defaults:this.options.defaults||{},data:M,index:P,mergeCells:Q,filter:N,showGridLines:this.options.showGridLines,hyperlinks:this.options.hyperlinks||[]});
},_lookupString:function(O){var N="$"+O;
var M=this._strings.indexes[N];
if(M!==undefined){O=M;
}else{O=this._strings.indexes[N]=this._strings.uniqueCount;
this._strings.uniqueCount++;
}this._strings.count++;
return O;
},_lookupStyle:function(O){var N=u.stringify(O);
if(N=="{}"){return 0;
}var M=b.inArray(N,this._styles);
if(M<0){M=this._styles.push(N)-1;
}return M+1;
},_lookupBorder:function(M){var O=u.stringify(M);
if(O=="{}"){return;
}var N=b.inArray(O,this._borders);
if(N<0){N=this._borders.push(O)-1;
}return N+1;
},_readCells:function(R){for(var O=0;
O<R.length;
O++){var Q=R[O];
var N=Q.cells;
Q.data=[];
for(var P=0;
P<N.length;
P++){var M=this._cell(N[P],Q.index,P);
if(M){Q.data.push(M);
}}}},_cell:function(Q,T,N){if(!Q||Q===n){return null;
}var W=Q.value;
var M={};
if(Q.borderLeft){M.left=Q.borderLeft;
}if(Q.borderRight){M.right=Q.borderRight;
}if(Q.borderTop){M.top=Q.borderTop;
}if(Q.borderBottom){M.bottom=Q.borderBottom;
}M=this._lookupBorder(M);
var U={bold:Q.bold,color:Q.color,background:Q.background,italic:Q.italic,underline:Q.underline,fontFamily:Q.fontFamily||Q.fontName,fontSize:Q.fontSize,format:Q.format,textAlign:Q.textAlign||Q.hAlign,verticalAlign:Q.verticalAlign||Q.vAlign,wrap:Q.wrap,borderId:M};
var P=this.options.columns||[];
var O=P[N];
var V=typeof W;
if(O&&O.autoWidth){var R=W;
if(V==="number"){R=u.toString(W,Q.format);
}O.width=Math.max(O.width||0,(R+"").length);
}if(V==="string"){W=C(W);
W=this._lookupString(W);
V="s";
}else{if(V==="number"){V="n";
}else{if(V==="boolean"){V="b";
W=+W;
}else{if(W&&W.getTime){V=null;
var S=(W.getTimezoneOffset()-l.getTimezoneOffset())*u.date.MS_PER_MINUTE;
W=(W-l-S)/u.date.MS_PER_DAY+1;
if(!U.format){U.format="mm-dd-yy";
}}else{V=null;
W=null;
}}}}U=this._lookupStyle(U);
return{value:W,formula:Q.formula,type:V,style:U,ref:w(T,N)};
}});
var m={General:0,"0":1,"0.00":2,"#,##0":3,"#,##0.00":4,"0%":9,"0.00%":10,"0.00E+00":11,"# ?/?":12,"# ??/??":13,"mm-dd-yy":14,"d-mmm-yy":15,"d-mmm":16,"mmm-yy":17,"h:mm AM/PM":18,"h:mm:ss AM/PM":19,"h:mm":20,"h:mm:ss":21,"m/d/yy h:mm":22,"#,##0 ;(#,##0)":37,"#,##0 ;[Red](#,##0)":38,"#,##0.00;(#,##0.00)":39,"#,##0.00;[Red](#,##0.00)":40,"mm:ss":45,"[h]:mm:ss":46,"mmss.0":47,"##0.0E+0":48,"@":49,"[$-404]e/m/d":27,"m/d/yy":30,t0:59,"t0.00":60,"t#,##0":61,"t#,##0.00":62,"t0%":67,"t0.00%":68,"t# ?/?":69,"t# ??/??":70};
function j(M){if(M.length<6){M=M.replace(/(\w)/g,function(N,O){return O+O;
});
}M=M.substring(1).toUpperCase();
if(M.length<8){M="FF"+M;
}return M;
}var G=u.Class.extend({init:function(M){this.options=M||{};
this._strings={indexes:{},count:0,uniqueCount:0};
this._styles=[];
this._borders=[];
this._sheets=b.map(this.options.sheets||[],b.proxy(function(N){N.defaults=this.options;
return new J(N,this._strings,this._styles,this._borders);
},this));
},toDataURL:function(){if(typeof JSZip==="undefined"){throw new Error("JSZip not found. Check http://docs.telerik.com/kendo-ui/framework/excel/introduction#requirements for more details.");
}var ae=new JSZip();
var N=ae.folder("docProps");
N.file("core.xml",k({creator:this.options.creator||"Kendo UI",lastModifiedBy:this.options.creator||"Kendo UI",created:this.options.date||new Date().toJSON(),modified:this.options.date||new Date().toJSON()}));
var W=this._sheets.length;
N.file("app.xml",d({sheets:this._sheets}));
var T=ae.folder("_rels");
T.file(".rels",x);
var ac=ae.folder("xl");
var ad=ac.folder("_rels");
ad.file("workbook.xml.rels",I({count:W}));
var X={};
ac.file("workbook.xml",H({sheets:this._sheets,filterNames:b.map(this._sheets,function(ai,ag){var ah=ai.options;
var aj=ah.name||ah.title||"Sheet"+(ag+1);
X[aj.toLowerCase()]=ag;
var af=ah.filter;
if(af&&typeof af.from!=="undefined"&&typeof af.to!=="undefined"){return{localSheetId:ag,name:aj,from:c(p(ah),af.from),to:c(p(ah),af.to)};
}}),userNames:b.map(this.options.names,function(af){return{name:af.localName,localSheetId:af.sheet?X[af.sheet.toLowerCase()]:null,value:af.value,hidden:af.hidden};
})}));
var ab=ac.folder("worksheets");
var Z=ab.folder("_rels");
for(var S=0;
S<W;
S++){var V=this._sheets[S];
var Y=u.format("sheet{0}.xml",S+1);
var U=V.relsToXML();
if(U){Z.file(Y+".rels",U);
}ab.file(Y,V.toXML(S));
}var M=b.map(this._borders,b.parseJSON);
var aa=b.map(this._styles,b.parseJSON);
var R=function(af){return af.underline||af.bold||af.italic||af.color||af.fontFamily||af.fontSize;
};
var P=b.map(aa,function(af){if(af.color){af.color=j(af.color);
}if(R(af)){return af;
}});
var Q=b.map(aa,function(af){if(af.format&&m[af.format]===undefined){return af;
}});
var O=b.map(aa,function(af){if(af.background){af.background=j(af.background);
return af;
}});
ac.file("styles.xml",D({fonts:P,fills:O,formats:Q,borders:M,styles:b.map(aa,function(ag){var af={};
if(R(ag)){af.fontId=b.inArray(ag,P)+1;
}if(ag.background){af.fillId=b.inArray(ag,O)+2;
}af.textAlign=ag.textAlign;
af.verticalAlign=ag.verticalAlign;
af.wrap=ag.wrap;
af.borderId=ag.borderId;
if(ag.format){if(m[ag.format]!==undefined){af.numFmtId=m[ag.format];
}else{af.numFmtId=165+b.inArray(ag,Q);
}}return af;
})}));
ac.file("sharedStrings.xml",y(this._strings));
ae.file("[Content_Types].xml",i({count:W}));
return"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,"+ae.generate({compression:"DEFLATE"});
}});
function g(N){var M="thin";
if(N===2){M="medium";
}else{if(N===3){M="thick";
}}return M;
}function f(M,O){var N="";
if(O&&O.size){N+="<"+M+' style="'+g(O.size)+'">';
if(O.color){N+='<color rgb="'+j(O.color)+'"/>';
}N+="</"+M+">";
}return N;
}function h(M){return"<border>"+f("left",M.left)+f("right",M.right)+f("top",M.top)+f("bottom",M.bottom)+"</border>";
}var A={};
var n={};
function r(Q,O){var P=[];
var R=[];
q(Q,function(V,U){var T={_source:V,index:U,height:V.height,cells:[]};
P.push(T);
R[U]=T;
});
var S=z(P).slice(0);
var M={rowData:P,rowsByIndex:R,mergedCells:O};
for(var N=0;
N<S.length;
N++){o(S[N],M);
delete S[N]._source;
}return z(P);
}function q(Q,M){for(var N=0;
N<Q.length;
N++){var P=Q[N];
if(!P){continue;
}var O=P.index;
if(typeof O!=="number"){O=N;
}M(P,O);
}}function z(M){return M.sort(function(N,O){return N.index-O.index;
});
}function o(S,R){var W=S._source;
var X=S.index;
var P=W.cells;
var N=S.cells;
if(!P){return;
}for(var T=0;
T<P.length;
T++){var M=P[T]||n;
var Y=M.rowSpan||1;
var Q=M.colSpan||1;
var O=s(N,M);
B(N,O,Q);
if(Y>1||Q>1){R.mergedCells.push(w(X,O)+":"+w(X+Y-1,O+Q-1));
}if(Y>1){for(var V=X+1;
V<X+Y;
V++){var U=R.rowsByIndex[V];
if(!U){U=R.rowsByIndex[V]={index:V,cells:[]};
R.rowData.push(U);
}B(U.cells,O-1,Q+1);
}}}}function s(N,M){var O;
if(typeof M.index==="number"){O=M.index;
t(N,M,M.index);
}else{O=e(N,M);
}return O;
}function t(N,M,O){N[O]=M;
}function e(N,M){var P=N.length;
for(var O=0;
O<N.length+1;
O++){if(!N[O]){N[O]=M;
P=O;
break;
}}return P;
}function B(M,P,N){for(var O=1;
O<N;
O++){t(M,A,P+O);
}}u.ooxml={Workbook:G,Worksheet:J,toWidth:F,toHeight:E,borderTemplate:h};
}(kendo.jQuery,kendo));
return kendo;
},typeof define=="function"&&define.amd?define:function(a,b,c){(c||b)();
}));

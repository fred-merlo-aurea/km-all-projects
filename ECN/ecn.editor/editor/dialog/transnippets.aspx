<%@ Page language="c#" Codebehind="transnippets.aspx.cs" AutoEventWireup="True" Inherits="ecn.communicator.contentmanager.feditor.dialog.transnippets" %>
<HTML>
	<HEAD>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style type="text/css">
			BODY { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif;background:#F7F7F7; }
			TD { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			INPUT { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			SELECT { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			TEXTAREA { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			BUTTON { FONT-SIZE: 11px; FONT-FAMILY: Arial, Verdana, 'Microsoft Sans Serif', Tahoma, Sans-Serif }
			tr.warnPad td { padding-top:5px; }
			.border { border-bottom:1px #ccc solid; }
			#udfRow td { padding-top:20px; }
			#udfRow td td { padding-top:0px; }
			.labelCell { width:120px; }
		</style>
		<script src="common/fck_dialog_common.js" type="text/javascript"></script>
		<LINK href="common/fck_dialog_common.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" type="text/JavaScript">
			var oEditor		= window.parent.InnerDialogLoaded() ;
			var FCK			= oEditor.FCK ;
			var FCKLang		= oEditor.FCKLang ;
			var FCKConfig	= oEditor.FCKConfig ;
			var FCKDebug	= oEditor.FCKDebug ;
			
			function ok()	{
				var transnippetName = getobj("transnippetName").value;
				var GroupID = getobj("Groups").value;
				var finalTransnippet = "##TRANSNIPPET|GRID|";
				
				if ( transnippetName.length == 0 ) {
					alert("Please enter a name for Transnippet.\nOnly Numbers[0-9], Letters[A-Z / a-z], and special Characters [.(dot), _(underscore), -(dash) and space] are allowed.");
					getobj("transnippetName").focus();
					return ;
				} else if (GroupID == "0"){
					alert("Please select a Group to choose UDF's");
					getobj("Groups").focus();
					return ;
				} else {
					//##TRANSNIPPET|GRID|$PICKUPDT,$LOCATION,$EMAILOPTIN|HDR-STYLE=font-family:Arial;font-size:10px;background-color:#FF0000;color:#000000;font-weight:bold|TBL-STYLE=font-family:Arial;font-size:10px;background-color:#ffac99;color:#000000;width:100%;border:1px solid;##
					var selectedUDFs = "";var selectedUDFHeaders = "";
					for (var i = 0; i < getobj("UDFSelectedList").options.length; i++) {
					    selectedUDFs += "$$" + getobj("UDFSelectedList").options[i].value + "$$,";
					    selectedUDFHeaders += getobj("UDFSelectedList").options[i].value + ",";						
					}		
					if(selectedUDFs.length == 0){
						alert("Please select atleast one UDF");
						getobj("GroupID").focus();
						return ;					
					}else {
						finalTransnippet += selectedUDFs.substring(0,(selectedUDFs.length - 1));
						var headerStyle = "|HDR-STYLE=";
						headerStyle += "font-family:"+getobj("HDR_Font").value+";";
						headerStyle += "font-size:"+getobj("HDR_FontSize").value+";";
						headerStyle += "background-color:"+getobj("HDR_CellBGColor").value+";";
						headerStyle += "color:"+getobj("HDR_FontColor").value+";";	
						headerStyle += "font-weight:bold"		
						finalTransnippet += 	headerStyle;
						
						var itemStyle = "|TBL-STYLE=";
						itemStyle += "font-family:"+getobj("ITEM_Font").value+";";
						itemStyle += "font-size:"+getobj("ITEM_FontSize").value+";";
						itemStyle += "background-color:"+getobj("ITEM_CellBGColor").value+";";
						itemStyle += "color:"+getobj("ITEM_FontColor").value+";";	
						if(getobj("TableBorder").value > 0){
							itemStyle += "border:"+getobj("TableBorder").value+"px solid;"
						}
						itemStyle += "width:"+getobj("TableWidth").value+getobj("TableWidthType").value+";";

						finalTransnippet += itemStyle;
						finalTransnippet += "|"+selectedUDFHeaders.substring(0, (selectedUDFHeaders.length - 1));
						finalTransnippet += "##";

						finalTransnippet = "<B>"+transnippetName+":</B><BR>"+finalTransnippet;
						
						alert("Please DO NOT make any modifications to the Transnippet script in between the \'##'\ and \'##'\, after its inserted in the Editor.\nIf you want to make changes, Delte the Transippet & start over.");
						oEditor.FCK.InsertHtml( finalTransnippet ) ;
					}
				}
				parent.closeWnd();
				//window.close() ;
			}
			
			function closeWindow() {
				window.open('','_parent','');
				window.close();			
			}


			function cancel() {
				parent.closeWnd();
			}		
			
			function getobj(id) {
				if (document.all && !document.getElementById) 
					obj = eval('document.all.' + id);
				else if (document.layers) 
					obj = eval('document.' + id);
				else if (document.getElementById) 
					obj = document.getElementById(id);

				return obj;
			}
		</script>
		<script language="JavaScript" type="text/JavaScript">
			function move(fbox, tbox) {
				var arrFbox = new Array();
				var arrTbox = new Array();
				var arrLookup = new Array();     
				var i;     
				for (i = 0; i < tbox.options.length; i++) {     
					arrLookup[tbox.options[i].text] = tbox.options[i].value;     
					arrTbox[i] = tbox.options[i].text;     
				}     
				var fLength = 0;
				var tLength = arrTbox.length;
				for(i = 0; i < fbox.options.length; i++) {     
					arrLookup[fbox.options[i].text] = fbox.options[i].value;     
					if (fbox.options[i].selected && fbox.options[i].value != "") {     
						arrTbox[tLength] = fbox.options[i].text;     
						tLength++;     
					} else {     
						arrFbox[fLength] = fbox.options[i].text;     
						fLength++;     
					}     
				}     
				fbox.length = 0;     
				tbox.length = 0;     
				var c;     
				for(c = 0; c < arrFbox.length; c++) {     
					var no = new Option();     
					no.value = arrLookup[arrFbox[c]];     
					no.text = arrFbox[c];     
					fbox[c] = no;     
				}     
				for(c = 0; c < arrTbox.length; c++) {     
					var no = new Option();     
					no.value = arrLookup[arrTbox[c]];     
					no.text = arrTbox[c];     
					tbox[c] = no;     
				}     
			}     
		</script>
		<script type="text/javascript" src="/ecn.collector/assets/html/Templatestyle.js"></script>
		<script language="JavaScript" type="text/JavaScript">
			var g_img;
			var g_txt;
			var g_tag;
			var g_class;
			var g_prop;
			var g_def;

			function opencolorpallette(img, txt, tag, cls, def) {
				g_img = img;
				g_txt = txt;
				g_tag = tag;
				g_class = cls;
				g_def = def;
				
				var x = getPosition(img).x + 25;
				var y = getPosition(img).y - 100;

				getobj('divpalette').style.left = x;
				getobj('divpalette').style.top = y;		
				getobj('divpalette').style.display='block';
			}
			function setcolor(img, txt, tag, cls, prop, def) {
				try{
					var selcolor = getobj(txt).value;
					if(selcolor!="") {	
						var imgctrl = getobj(img);
						try {
							imgctrl.style.backgroundColor=  selcolor;
							setStyleByClass(tag, cls, prop, selcolor + def);
						} catch(e) {
							alert('Invalid Color');
						}
					}
				}catch(e){selcolor = "";}
			}
			
			function getPosition(img) {
				// This function will return an Object with x and y properties
				var useWindow=false;
				var coordinates=new Object();
				var x=0,y=0;
				// Browser capability sniffing
				var use_gebi=false, use_css=false, use_layers=false;
				if (document.getElementById) { use_gebi=true; }
				else if (document.all) { use_css=true; }
				else if (document.layers) { use_layers=true; }
				// Logic to find position
 				if (use_gebi && document.all) {
					x=getPageOffsetLeft(img);
					y=getPageOffsetTop(img);
				} else if (use_gebi) {
					var o=img;
					x=getPageOffsetLeft(o);
					y=getPageOffsetTop(o);
				} else if (use_css) {
					x=getPageOffsetLeft(img);
					y=getPageOffsetTop(img);
				} else {
					coordinates.x=0; coordinates.y=0; return coordinates;
				}
				
				coordinates.x=x-200;
				coordinates.y=y-150;
				return coordinates;
			}
			
			function getPageOffsetLeft (el) {
				var ol=el.offsetLeft;
				while ((el=el.offsetParent) != null) { ol += el.offsetLeft; }
				return ol;
			}
			function getPageOffsetTop (el) {
				var ot=el.offsetTop;
				while((el=el.offsetParent) != null) { ot += el.offsetTop; }
				return ot;
			}
			function selcolor_onpropertychange() {
				try{
						getobj('selhicolor').style.backgroundColor = selcolor.value;
				} catch(e) {}
			}
			function ColorTable_onclick(e) {
				if (!e)  e = window.event;
				
				if (e.target) 
					targ = e.target;
				else if (e.srcElement) 
					targ = e.srcElement;

				getobj('selhicolor').style.backgroundColor = targ.title;
				getobj('selcolor').value = targ.title;
			}
			
			function ColorTable_onmouseover(e) {
				if (!e)  e = window.event;
				
				if (e.target) 
					targ = e.target;
				else if (e.srcElement) 
					targ = e.srcElement;
					
				getobj('hicolortext').innerText = targ.title;
				getobj('hicolor').style.backgroundColor = targ.title;
			}
			
			function ColorTable_onmouseout(e) {
				if (!e)  e = window.event;
				
				if (e.target) 
					targ = e.target;
				else if (e.srcElement) 
					targ = e.srcElement;
					
				getobj('hicolortext').innerText = "";
				getobj('hicolor').style.backgroundColor = "";
			}
			function apply() {
				var selcolor = getobj('selcolor').value;
				if(selcolor!="undefined" && selcolor.substring(0,1) == "#") {	
					g_img.style.backgroundColor=  selcolor;
					var txtbox = getobj(g_txt);
					txtbox.value = selcolor;
					setStyleByClass(g_tag, g_class, g_prop, selcolor + g_def);
				}
				cleanup();
				setPreviewTableProps();
			} 
			
			function btncancel_onclick(){
				cleanup();
			}
			
			function cleanup() {
				g_img = null;
				g_txt = "";
				g_tag = "";
				g_class = "";
				g_prop = "";
				g_def = "";
				getobj('hicolortext').innerText = "";
				getobj('hicolor').style.backgroundColor = "";
				getobj('selhicolor').style.backgroundColor = '';
				getobj('selcolor').value='';
				getobj('divpalette').style.display='none';
			}
			
			function setPreviewTableProps(){
				var previewheaderStyle = "font-family:"+getobj("HDR_Font").value+";";
				previewheaderStyle += "font-size:"+getobj("HDR_FontSize").value+";";
				previewheaderStyle += "background-color:"+getobj("HDR_CellBGColor").value+";";
				previewheaderStyle += "color:"+getobj("HDR_FontColor").value+";";	
				previewheaderStyle += "font-weight:bold";

				document.getElementById("PreviewHeader1").style.cssText = previewheaderStyle;
				document.getElementById("PreviewHeader2").style.cssText = previewheaderStyle;		
				document.getElementById("PreviewHeader3").style.cssText = previewheaderStyle;								

				var previewitemStyle = "font-family:"+getobj("ITEM_Font").value+";";
				previewitemStyle += "font-size:"+getobj("ITEM_FontSize").value+";";
				previewitemStyle += "background-color:"+getobj("ITEM_CellBGColor").value+";";
				previewitemStyle += "color:"+getobj("ITEM_FontColor").value+";";	
				
				document.getElementById("PreviewData1").style.cssText = previewitemStyle;
				document.getElementById("PreviewData2").style.cssText = previewitemStyle;	
				document.getElementById("PreviewData3").style.cssText = previewitemStyle;
				document.getElementById("PreviewData4").style.cssText = previewitemStyle;	
				document.getElementById("PreviewData5").style.cssText = previewitemStyle;
				document.getElementById("PreviewData6").style.cssText = previewitemStyle;									
				
				if(getobj("TableBorder").value > 0){
					previewitemStyle += "border:"+getobj("TableBorder").value+"px solid;"
				}
				previewitemStyle += "width:"+getobj("TableWidth").value+getobj("TableWidthType").value+";";
				document.getElementById("PreviewTable").style.cssText = previewitemStyle;
			}
		</script>
	</HEAD>
	<body bottomMargin="5" leftMargin="5" topMargin="5" rightMargin="5">
		<DIV id="divpalette" style="BORDER: 1px solid; PADDING-RIGHT: 5px; DISPLAY: none; PADDING-LEFT: 5px; Z-INDEX: 101; BACKGROUND: #000000; LEFT: 0px; PADDING-BOTTOM: 5px;WIDTH: 360px; PADDING-TOP: 5px; POSITION: absolute; HEIGHT: 206px;">
			<TABLE cellSpacing="0" cellPadding="0" border="0" style=" color:#FFFFFF">
				<TR>
					<TD id="ColorTableCell" vAlign="top" noWrap align="left"></TD>
					<TD>&nbsp;</TD>
					<TD vAlign="top" noWrap align="center"><INPUT style="MARGIN-BOTTOM: 6px; WIDTH: 75px; HEIGHT: 22px" onClick="apply();" type="button"
							value="OK" name="btnOK"><BR>
						<INPUT style="MARGIN-BOTTOM: 6px; WIDTH: 75px; HEIGHT: 22px" onClick="javascript:btncancel_onclick();"
							type="button" value="Cancel" name="btnCancel"><BR>
						<SPAN><b>Highlight</b></SPAN>:
						<DIV id="hicolor" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 74px; BORDER-BOTTOM: 1px solid; HEIGHT: 20px"></DIV>
						<DIV id="hicolortext" style="MARGIN-BOTTOM: 7px; WIDTH: 75px; TEXT-ALIGN: right"></DIV>
						<SPAN><b>Selected</b></SPAN>:
						<DIV id="selhicolor" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; WIDTH: 74px; BORDER-BOTTOM: 1px solid; HEIGHT: 20px"></DIV>
						<INPUT id="selcolor" style="MARGIN-TOP: 0px; MARGIN-BOTTOM: 7px; WIDTH: 75px; HEIGHT: 20px"
							type="text" maxLength="20" onChange="selcolor_onpropertychange()">
					</TD>
				</TR>
			</TABLE>
		</DIV>
		<form id="transnippetForm" Runat="Server">
				<table id="Table2" cellSpacing="0" cellPadding="3" width="100%" border="0">
	<tr>
						<td vAlign="top" colspan=3 style="background:#F60;color:#000;"><img src="warning.gif">
						Selecting User Defined Fields [UDFs] from different Groups for the same Campaign will cause Campaign not to work correctly. Please choose UDFs from the Group you are trying to send this Campaign.</td>
				</tr>
					<TR class="warnPad">
						<TD align="right" vAlign="top" width="120"><div class="labelCell"><b>Table Title:</b></div></TD>
					<TD vAlign="top" style="width:100%;padding-bottom:10px;" colspan=2><input type="text" runat="server" name="transnippetName" id="transnippetName" style="WIDTH:250px"> (alphanumeric characters only)<br>
					This name will appear as the table title.</TD>
					</TR>
					<TR>
						<TD align="right" vAlign="middle" width="120"><b>Select&nbsp;Group:</b></TD>
					<td vAlign="middle" colspan=2><asp:dropdownlist class="formfield" id="Groups" AutoPostBack="True" DataValueField="GroupID" DataTextField="GroupName"
								runat="server" Width="250px" onselectedindexchanged="Groups_SelectedIndexChanged"></asp:dropdownlist></td>
					</TR>
					<TR id="udfRow">
						<TD align="right" valign="top" width="120"><b>Available UDF's: </b></TD>
						<td vAlign="top" colspan=2>
							<table>
								<tr>
									<td>Select UDF(s):<br /><asp:ListBox id="UDFList" runat="server" Width="200" Rows="7" SelectionMode="Multiple"></asp:ListBox></td>
									<td>	
										<input type="button" onClick="move(UDFList,UDFSelectedList)" value=">>"><br><br>
										<input type="button" onClick="move(UDFSelectedList,UDFList)" value="<<">     
									</td>
									<td>
										Add to Transnippet:<br /><select name="UDFSelectedList" size="7" multiple id="UDFSelectedList" style="width:200"></select>
									</td>
								</tr>
							</table>
						</td>
					</TR>
				</table>
				<table cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td colspan="3" align="left"><b>Transaction Display Properties: </b></td>
				</tr>  
				<tr>
              		<td colspan="2" style="width:60%;padding:10px;"><table cellpadding="0" cellspacing="0" style="width:100%">
           			<tr>
                    		<td align="right" class="border labelCell"><b>Table Properties: </b></td>
	<td valign="middle" align="right" class="border" style="padding:15px;">
							<b>Border:</b>&nbsp;
	<input type=text size=1 maxlength=1 runat=server name=TableBorder id=TableBorder value="0" onBlur="javascript:setPreviewTableProps();">&nbsp;
							<b>Width:</b>&nbsp;<input type=text size=2 maxlength=3 runat=server name=TableWidth id="TableWidth" value="100" onBlur="javascript:setPreviewTableProps();">&nbsp;
							<select name="TableWidthType" runat="server" id="TableWidthType">
								<option value="%" selected>%</option>
								<option value="px">px</option>
							</select>
							</td>
						</tr>
                 		<tr>
                    		<TD align="right" class="border labelCell"><b>Header Properties:</b><br></TD>
						<td valign="top" class="border" style="padding:15px;">
							<table border="0" cellpadding="1" cellspacing="0">
								<tr>
									<td><select name="HDR_Font" runat="server" id="HDR_Font" style="WIDTH:100px" onChange="javascript:setPreviewTableProps();">
											<option value="Arial" selected>Font Name</option>
											<option value="Arial">Arial</option>
											<option value="'Comic Sans MS'">Comic Sans MS</option>
											<option value="'Courier New'">Courier New</option>
											<option value="Tahoma">Tahoma</option>
											<option value="'Times New Roman'">Times New Roman</option>
											<option value="Verdana">Verdana</option>
										</select></td>
									<td valign=middle style="padding-left:10px; padding-right:5px"><img src="FontColor.jpg" alt="Font Color"></td>
									<td valign=middle style="padding-right:5px">
										<input type=text id="HDR_FontColor" onBlur="javascript:setcolor('IMG_HDR_FontColor', 'HDR_FontColor', 'div', 'color', '');" 
											name="HDR_FontColor" Size="7" maxlength=7  value="#000000">
									</td>
									<td width=50%>	<A href="javascript:void(0);" class="colorPallette">
											<IMG id="IMG_HDR_FontColor" style="BACKGROUND-COLOR: #000000" 
												onclick="javascript:opencolorpallette(this, 'HDR_FontColor', 'div', 'divpageHeader', 'color', '');" src="/ecn.images/images/ColorPad.gif" 
												align="middle" border="0">
										</A>
									</td>
								<tr>
								<tr>	
									<td><select name="HDR_FontSize" runat="server" id="HDR_FontSize" style="WIDTH:100px"  onchange="javascript:setPreviewTableProps();">
											<option value="11px" selected>Font Size</option>
											<option value="8px">8 px</option>
											<option value="9px">9 px</option>
											<option value="10px">10 px</option>
											<option value="11px">11 px</option>
											<option value="12px">12 px</option>
											<option value="13px">13 px</option>
										</select></td>
									<td valign=middle style="padding-left:10px;padding-right:5px"><img src="BGColor.jpg" alt="Cell Background Color"></td>
									<td valign=middle style="padding-right:5px">
										<input type=text id="HDR_CellBGColor" onBlur="javascript:setcolor('IMG_HDR_CellBGColor', 'HDR_CellBGColor', 'div', 'color', '');" 
											name="HDR_CellBGColor" Size="7" maxlength=7 value="#CCCCCC">
									</td>
									<td width=50%>	<A href="javascript:void(0);" class="colorPallette">
											<IMG id="IMG_HDR_CellBGColor" style="BACKGROUND-COLOR: #000000" 
												onclick="javascript:opencolorpallette(this, 'HDR_CellBGColor', 'div', 'divpageHeader', 'color', '');" src="/ecn.images/images/ColorPad.gif" 
												align="middle" border="0">
										</A>
									</td>									
								</tr>
							</table>
						</td>
						</tr>
						<tr>
							<TD align="right" width="120"><b>Item Properties:</b><br></TD>
							<td valign="top" style="padding:15px;">
	<table border="0" cellpadding="1" cellspacing="0">
								<tr>
									<td><select name="ITEM_Font" runat="server" id="ITEM_Font" style="WIDTH:100px"  onchange="javascript:setPreviewTableProps();">
											<option value="Arial" selected>Font Name</option>
											<option value="Arial">Arial</option>
											<option value="'Comic Sans MS'">Comic Sans MS</option>
											<option value="'Courier New'">Courier New</option>
											<option value="Tahoma">Tahoma</option>
											<option value="'Times New Roman'">Times New Roman</option>
											<option value="Verdana">Verdana</option>
										</select></td>
									<td valign=middle style="padding-left:10px;padding-right:5px"><img src="FontColor.jpg" alt="Font Color"></td>
									<td valign=middle style="padding-right:5px">
										<input type=text id="ITEM_FontColor" onBlur="javascript:setcolor('IMG_ITEM_FontColor', 'ITEM_FontColor', 'div', 'color', '');" 
											name="ITEM_FontColor" 	Size="7" maxlength=7 value="#000000">
									</td>
									<td width=50%>	<A href="javascript:void(0);" class="colorPallette">
											<IMG id="IMG_ITEM_FontColor" style="BACKGROUND-COLOR: #000000" 
												onclick="javascript:opencolorpallette(this, 'ITEM_FontColor', 'div', 'divpageHeader', 'color', '');" src="/ecn.images/images/ColorPad.gif" 
												align="middle" border="0">
										</A>
									</td>
								</tr>
								<tr>	
									<td><select name="ITEM_FontSize" runat="server" id="ITEM_FontSize" style="WIDTH:100px"  onchange="javascript:setPreviewTableProps();">
											<option value="11px" selected>Font Size</option>
											<option value="8px">8 px</option>
											<option value="9px">9 px</option>
											<option value="10px">10 px</option>
											<option value="11px">11 px</option>
											<option value="12px">12 px</option>
											<option value="13px">13 px</option>
										</select></td>
									<td valign=middle style="padding-left:10px;padding-right:5px"><img src="BGColor.jpg" alt="Cell Background Color"></td>
									<td valign=middle style="padding-right:5px">
										<input type=text id="ITEM_CellBGColor" onBlur="javascript:setcolor('IMG_ITEM_CellBGColor', 'ITEM_CellBGColor', 'div', 'color', '');" 
											name="ITEM_CellBGColor" Size="7" maxlength=7 value="#FFFFFF">
									</td>
									<td width=50%>	<A href="javascript:void(0);" class="colorPallette">
											<IMG id="IMG_ITEM_CellBGColor" style="BACKGROUND-COLOR: #000000" onClick="javascript:opencolorpallette(this, 'ITEM_CellBGColor', 'div', 'divpageHeader', 'color', '');"
																		src="/ecn.images/images/ColorPad.gif" align="middle" border="0">
										</A>
									</td>
								</tr>
							</table>
						</td>
						</tr>
					</table></td>
	                 
					<td valign="middle" style="width:40%;padding:10px;"><b>Preview Table Title:</b><table cellpadding="0" bgcolor="#ffffff" cellspacing="0" style="width:100%">
	                 
					<tr>
                		<td><div id="PreviewTable"><table style="width:100%" border=0 height=75% cellpadding=0 cellspacing=0 align=center>
							<tr>
										<td valign="top"><div id=PreviewHeader1>HEADER</div></td><td><div id=PreviewHeader2>HEADER</div></td><td><div id=PreviewHeader3>HEADER</div></td>
						</tr>
									<tr>
										<td><div id="PreviewData1">item</div></td><td><div id="PreviewData2">item</div></td><td><div id="PreviewData3">item</div></td>
									</tr>
									<tr>
										<td><div id="PreviewData4">item</div></td><td><div id="PreviewData5">item</div></td><td><div id="PreviewData6">item</div></td>
									</tr>
								</table>
					</div></td>
					</tr>
					</table></td>
				</tr>
				<tr>
						<td align="right" colspan="3" style="padding-top:10px;"><input onClick="ok();" type="button" value="OK"><!-- fckLang="DlgBtnOK"-->&nbsp; 
							&nbsp;<input onClick="parent.Cancel();" type="button" value="Cancel"><!-- fckLang="DlgBtnCancel"-->
						</td>
					</tr>
				</table>
		</form>
		<script type="text/javascript">
			var ac=['00','33','66','99','cc','ff'];
			var txt='<table ID="ColorTable" border="0" cellspacing="0" cellpadding="0" width="270" class="selcolor" onclick="javascript:ColorTable_onclick(event)" onmouseover="javascript:ColorTable_onmouseover(event)" onmouseout="javascript:ColorTable_onmouseout(event)">';

			for (var i=0; i<3;i++){txt+='<tr>';for (var j=0; j<3;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
			for (var i=3; i<6;i++){txt+='<tr>';for (var j=0; j<3;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
			for (var i=0; i<3;i++){txt+='<tr>';for (var j=3; j<6;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
			for (var i=3; i<6;i++){txt+='<tr>';for (var j=3; j<6;j++){for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[j]+ac[n]+ac[i]+'" title="#'+ac[j]+ac[n]+ac[i]+'"></td>';}}txt+='</tr>';}
			txt+='<tr>';for (var n=0; n<6;n++){txt+='<td style="height:15px;width:15px;" bgcolor="#'+ac[n]+ac[n]+ac[n]+'"title="#'+ac[n]+ac[n]+ac[n]+'"></td>';}
			for (var i=0;i<12;i++){txt+='<td style="height:15px;width:15px;" bgcolor="#000000" title="#000000"></td>';}
			txt+='</tr>';
			txt+='</table>';
			getobj('ColorTableCell').innerHTML = txt ;
		</script>		
		<script language=javascript type=text/javascript>
			setcolor('IMG_HDR_CellBGColor', 'HDR_CellBGColor', 'div', 'color', '');
			setcolor('IMG_ITEM_CellBGColor', 'ITEM_CellBGColor', 'div', 'color', '');			
			setPreviewTableProps();
		</script>		
	</body>
</HTML>

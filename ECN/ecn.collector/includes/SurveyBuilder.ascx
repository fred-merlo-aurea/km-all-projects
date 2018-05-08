<%@ Control Language="c#" Inherits="ecn.collector.includes.SurveyBuilder" Codebehind="SurveyBuilder.ascx.cs" %>
<asp:Label ID="plstyletag" EnableViewState="true" runat="server"></asp:Label>
<script type="text/javascript" src="/ecn.collector/scripts/highslide/highslide-full.js"></script>
<link rel="stylesheet" type="text/css" href="/ecn.collector/css/ecnHighslide.css" />
<link rel="stylesheet" type="text/css" href="/ecn.collector/css/ecnHighslide-styles.css" />

<script language="javascript">
    hs.graphicsDir = 'http://www.ecn5.com/ecn.collector/scripts/highslide/graphics/';
		
			function validate()
			{
				try
				{
					var bSuccess = true;
					var cSuccess = true;
					var sSuccess = true;
					var strfailureMessage = "";
					
					for(i=0;i<fv.length;i++)
					{
						var alreadyselected = false;
						var errmsg = "";
						var arr = fv[i].split(",");
						
						cSuccess = true;
						
						if (arr.length == 1)
							cSuccess=checkRequired("SurveyBuilder_"+ arr[0], 0);
						else if (arr.length == 2)
							cSuccess=checkRequired("SurveyBuilder_"+ arr[0], arr[1]);
						else
						{
							//alert(arr[0] + " / "+ arr[1] + " / "+ arr[2]);
							
							var arrStatementID = arr[2].split("|");
							var gridValidationType = arr[1];
						
							//if (arrStatementID.length == 1)
							//	cSuccess=checkRequired("SurveyBuilder_"+ arr[0]);
							//else
							//{
								for(j=0;j<arrStatementID.length;j++)
								{
									sSuccess=checkGrid("SurveyBuilder_"+ arr[0], arrStatementID[j],gridValidationType);
									//alert("SurveyBuilder_"+ arr[0] + " , " + arrStatementID[j] + " :  " + sSuccess);
									
									if (gridValidationType == 3)
									{
										//errmsg = " (atleast 1 response is required per row.)"
										if (!sSuccess)
											cSuccess = false;
									}
									else if (gridValidationType == 2)
									{
										//errmsg = " (aleast 1 response is required.)"
										if (!sSuccess)
											cSuccess = false;
										else
										{
											cSuccess = true;
											break;	
										}
									}
									else if (gridValidationType == 1)
									{
										//alert("SurveyBuilder_"+ arr[0] + " , " + arrStatementID[j] + " :  " + sSuccess);
										//errmsg = " (only 1 response is required.)"
										if (!sSuccess)
										{
											if (!alreadyselected)
												cSuccess = false;
										}
										else
										{
											if (alreadyselected)
											{
												cSuccess = false;
												break;
											}
											else
											{
												cSuccess = true;
												alreadyselected = true;
											}	
										}
										//alert(sSuccess + " / " + cSuccess);
									}
								}
							//}

							/*var ctrlgrid = getobj("divg_" + arr[0]);
							if (!cSuccess)
								ctrlgrid.style.display="block";
							else
								ctrlgrid.style.display="none";	*/							
							
						}
						
						var ctrl1 = getobj("divq_" + arr[0]);
						
						if (cSuccess)
							ctrl1.className="question";
						else
						{
							bSuccess = false;
							strfailureMessage += ctrl1.innerHTML.replace(/<BR>/i, "") + " " + errmsg + "\n";
							ctrl1.className="vstyle";
						}			
					}
				}
				catch(err)
				{
					//txt="There was an error on this page.\n\n"
					//txt+="Error description: " + err.description + "\n\n"
					//alert(txt);
					bSuccess = false;
				}
				
				if (!bSuccess)
					alert("The following question(s) are required: \n\n" + strfailureMessage);
				return bSuccess;
			}

			function checkRequired(ctrlname, optioncount)
			{
				
				if (optioncount == 0)
				{
					var ctrl = getobj(ctrlname);
					
					if (trim(ctrl.value) == '')
						return false;
					else
						return true;	
				}
				else
				{
					for(var index=0;index<optioncount;index++)
					{
						var ctrl = getobj(ctrlname+ "_" + index);
						if (ctrl.checked)
							return true;
					}
					return false;
				}
			}		

			function checkGrid(ctrlname, statementID, gridvalidationtype)
			{
				var IsalreadySelected = false;
				var controlID = ctrlname + "_" + statementID;
				for (var i=1; i<document.forms[0].length; i++)   
				{ 
					if(document.forms[0].elements[i].id)
					{ 
						if(document.forms[0].elements[i].id.indexOf(controlID)!=-1) 
						{
						
							if (gridvalidationtype == 3 || gridvalidationtype == 2)
							{
								if (document.forms[0].elements[i].checked)
								{
									return true;
								}
							}
							else if (gridvalidationtype == 1)
							{
								if (document.forms[0].elements[i].checked)
								{
									if(IsalreadySelected)
										return false;
										
									IsalreadySelected = true;
								}
							}
						}
					}
				}
				return IsalreadySelected;
			}

			function getobj(id) {
			    if (id.indexOf("SurveyWizard_") != -1) {
			        id = 'ctl00_ContentPlaceHolder1_' + id;
			    }
				if (document.all && !document.getElementById) 
					obj = eval('document.all.' + id);
				else if (document.layers) 
					obj = eval('document.' + id);
				else if (document.getElementById) 
					obj = document.getElementById(id);

				return obj;
			}
			
	function resetcontrol(ctrl, qID)
	{
	
		var inputs = document.getElementsByTagName("input");
		var ctrltype = ctrl.type;
		for (var i=0;i<inputs.length;i++) 
		{
			if (inputs[i].type == ctrltype && ctrl != inputs[i] && inputs[i].id.indexOf('SurveyBuilder_question_' + qID + '_') > -1 )
				inputs[i].checked=false;
		}
	}
			
		// Removes leading whitespaces
		function LTrim( value ) {
			
			var re = /\s*((\S+\s*)*)/;
			return value.replace(re, "$1");
			
		}

		// Removes ending whitespaces
		function RTrim( value ) {
			
			var re = /((\s*\S+)*)\s*/;
			return value.replace(re, "$1");
			
		}

		// Removes leading and ending whitespaces
		function trim( value ) {
			return LTrim(RTrim(value));
		}

		// Keep user from entering more than maxLength characters
		function doKeypress(control, oevent){
			maxLength = control.attributes["maxLength"].value;
			value = control.value;

			if(maxLength && value.length > maxLength-1){
				oevent.returnValue = false;
				maxLength = parseInt(maxLength);
			}
		}
		// Cancel default behavior
		function doBeforePaste(control, oevent){
			maxLength = control.attributes["maxLength"].value;
			if(maxLength)
			{
				oevent.returnValue = false;
			}
		}
		// Cancel default behavior and create a new paste routine
		function doPaste(control, oevent){
			maxLength = control.attributes["maxLength"].value;
			value = control.value;
			if(maxLength){
				oevent.returnValue = false;
				maxLength = parseInt(maxLength);
				var oTR = control.document.selection.createRange();
				var iInsertLength = maxLength - value.length + oTR.text.length;
				var sData = window.clipboardData.getData("Text").substr(0,iInsertLength);
				oTR.text = sData;
			}
		}

		function EnableTextControl(ctrltype, ctrlname, optionCount)
		{
			var bEnable = false;
			
			if (ctrltype == 'r' || ctrltype == 'c')
			{	if (getobj("SurveyBuilder_" + ctrlname+ "_" + (optionCount-1)).checked)
					bEnable = true;
			}
			else
			{
				if (getobj("SurveyBuilder_" + ctrlname).selectedIndex == optionCount-1)
					bEnable = true;
			}
			
			if (bEnable)
			{
				getobj("SurveyBuilder_" + ctrlname+ "_TEXT").disabled = false;
				getobj("SurveyBuilder_" + ctrlname+ "_TEXT").focus();
			}
			else
			{
				getobj("SurveyBuilder_" + ctrlname+ "_TEXT").value="";
				getobj("SurveyBuilder_" + ctrlname+ "_TEXT").disabled = true;
			}
		}

</script>

<table class="surveybody" cellspacing="0" cellpadding="0" width="100%" border="0">
    <tr>
        <td id="surveybodytd" width="100%" valign="top">
            <div class="outertable">
                <div class="divHeader">
                    <asp:Image ID="imgHeader" runat="server" CssClass="divHeaderIMG"></asp:Image></div>
                <asp:PlaceHolder ID="plPageHeader" runat="server" Visible="True">
                    <div class="divpageHeader">
                        <asp:Label ID="lblPageHeader" runat="server"></asp:Label></div>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="plPageDesc" runat="server" Visible="True">
                    <div class="divpageDesc">
                        <asp:Label ID="lblPageDesc" runat="server"></asp:Label></div>
                </asp:PlaceHolder>
                <div>
                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <asp:PlaceHolder ID="plProgressBar" runat="server" Visible="false">
                            <tr>
                                <td class="formLabel" valign="top" align="right" style="padding-right: 10px">
                                    <asp:PlaceHolder ID="plbarHTML" runat="server" Visible="false"></asp:PlaceHolder>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phError" runat="server" Visible="true">
                            <tr>
                                <td>
                                    <br>
                                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                                        <tr>
                                            <td id="errorTop">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="errorMiddle">
                                                <table height="67" width="80%">
                                                    <tr>
                                                        <td valign="top" align="center" width="20%">
                                                            <img style="padding-right: 0px; padding-left: 15px; padding-bottom: 0px; padding-top: 0px"
                                                                src="/ecn.images/images/errorEx.jpg"></td>
                                                        <td valign="middle" align="left" width="80%" height="100%">
                                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="errorBottom">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="plInactiveSurvey" runat="server" Visible="false">
                            <tr>
                                <td align="center" class="formLabel">
                                    <br><br><br><br>
                                    Thank you. This survey is either complete or inactive.
                                 </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>
                                <div class="surveytable">
                                    <asp:PlaceHolder ID="plSurveyContent" runat="server"></asp:PlaceHolder>
                                    <br>
                                    <br>
                                    <div align="center">
                                        <asp:Button ID="btnPrevious" runat="server" Text="Previous" OnClick="btnPrevious_Click">
                                        </asp:Button>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnNext" runat="server" Text="Next"
                                            OnClick="btnNext_Click"></asp:Button></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="divFooter">
                    <asp:Image ID="imgFooter" runat="server" CssClass="divFooterIMG"></asp:Image></div>
            </div>
        </td>
    </tr>
</table>

<%@ Control Language="c#" Inherits="ecn.collector.main.survey.UserControls.DefineSurvey" Codebehind="DefineSurvey.ascx.cs" %>

<script language="javascript">
///DATE VALIDATION

// Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=2000;
var maxYear=2025;

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(source,arguments){
	var daysInMonth = DaysArray(12);
	dtStr = arguments.Value;
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strMonth=dtStr.substring(0,pos1)
	var strDay=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		//alert("The date format should be : mm/dd/yyyy")
		arguments.IsValid = false;
		return false;
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month")
		arguments.IsValid = false;
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid day")
		arguments.IsValid = false;
		return false
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		arguments.IsValid = false;
		return false
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		arguments.IsValid = false;
		alert("Please enter a valid date")
		return false
	}
	arguments.IsValid = true;
	return true
}
</script>

<div align="center">
    <asp:PlaceHolder ID="plSurveyNew" runat="server">
        <div class="section">
            <asp:Label ID="lblCopySurveyID" Visible="False" runat="server"></asp:Label>
            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr>
                    <td class="formLabel">
                        Select one of the options below.</td>
                </tr>
                <tr>
                    <td class="formLabel" style="padding-left: 20px" width="100%">
                        <asp:RadioButton ID="rbNewSurvey" runat="Server" Checked="True" CssClass="expandAccent"
                            AutoPostBack="true" Text="Create New Survey" GroupName="grpSelect" OnCheckedChanged="rbNewSurvey_CheckedChanged">
                        </asp:RadioButton></td>
                </tr>
                <tr>
                    <td class="formLabel" style="padding-left: 20px" width="100%">
                        <asp:RadioButton ID="rbCopySurvey" runat="Server" CssClass="expandAccent" Text="Copy Existing Survey"
                            GroupName="grpSelect" AutoPostBack="true" OnCheckedChanged="rbCopySurvey_CheckedChanged">
                        </asp:RadioButton></td>
                </tr>
                <asp:PlaceHolder ID="plCopySurvey" runat="server" Visible="false">
                    <tr>
                        <td style="padding-left: 30px">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="formLabel" align="left" width="15%">
                                        Select&nbsp;Survey:&nbsp;</td>
                                    <td class="dataOne" align="left" width="85%">
                                        <asp:DropDownList ID="drpSurvey" runat="server" CssClass="label10" AutoPostBack="true"
                                            DataTextField="SurveyTitle" DataValueField="SurveyID" EnableViewState="true"
                                            Width="250" Height="17" OnSelectedIndexChanged="drpSurvey_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </div>
    </asp:PlaceHolder>
    <div class="section">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="padding-left: 30px">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td class="formLabel" align="right" width="150" style="padding-right: 20px">
                                Survey Title:&nbsp;</td>
                            <td class="dataOne" align="left">
                                <asp:TextBox ID="txtSurveyTitle" CssClass="label10" runat="server" Width="225" MaxLength="42"
                                    Style="width: 300px;"></asp:TextBox>&nbsp;
                                <asp:RequiredFieldValidator ID="rfvtxtContentTitle" runat="server" Font-Size="xx-small"
                                    ControlToValidate="txtSurveyTitle" ErrorMessage="« required" Font-Italic="True"
                                    Font-Bold="True"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="formLabel" align="right" width="150" style="padding-right: 20px" valign="top">
                                Survey Description:&nbsp;</td>
                            <td class="dataOne" align="left">
                                <asp:TextBox ID="txtSurveyDesc" runat="server" TextMode="multiline" Columns="40"
                                    Rows="5" class="formfield" Style="font-family: Arial, Helvetica, sans-serif;
                                    width: 300px;"></asp:TextBox>
                        </tr>
                        <!--
                        <TR>
                            <TD class="formLabel" align="left">Status:&nbsp;</TD>
                            <TD class="dataOne" align="left"><asp:radiobuttonlist id="rbStatus" Runat="server" cssclass="label10" repeatdirection="horizontal">
                                    <asp:ListItem Value="Y">Active</asp:ListItem>
                                    <asp:ListItem Value="N" Selected="True">InActive</asp:ListItem>
                                </asp:radiobuttonlist>
                            </TD>
                        </TR>-->
                        <tr>
                            <td class="formLabel" align="right" style="padding-right: 20px">
                                Activation Date:&nbsp;(optional)</td>
                            <td class="dataOne" align="left">
                                <asp:TextBox ID="txtActivationDate" CssClass="label10" runat="server" Width="75"></asp:TextBox>&nbsp;<img
                                    onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%=txtActivationDate.ClientID%>'),document.getElementById('<%=txtActivationDate.ClientID%>')); return false"
                                    src="/ecn.images/images/icon-calendar.gif" align="absMiddle">
                                &nbsp;&nbsp;<asp:CustomValidator ID="cv1" runat="server" Enabled="true" CssClass="formSelect"
                                    ControlToValidate="txtActivationDate" ClientValidationFunction="isDate" OnServerValidate="ServerSide_isDate"
                                    ErrorMessage="<< Invalid Date"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="formLabel" align="right" style="padding-right: 20px">
                                Deactivation Date:&nbsp;(optional)
                            </td>
                            <td class="dataOne" align="left">
                                <asp:TextBox ID="txtDeActivationDate" CssClass="label10" runat="server" Width="75"></asp:TextBox>&nbsp;<img
                                    onclick="if(self.gfPop)gfPop.fPopCalendar(document.getElementById('<%=txtDeActivationDate.ClientID%>'),document.getElementById('<%=txtDeActivationDate.ClientID%>')); return false"
                                    src="/ecn.images/images/icon-calendar.gif" align="absMiddle">
                                &nbsp;&nbsp;<asp:CustomValidator ID="cv2" runat="server" Enabled="true" CssClass="formSelect"
                                    ControlToValidate="txtDeActivationDate" ClientValidationFunction="isDate" OnServerValidate="ServerSide_isDate"
                                    ErrorMessage="<< Invalid Date"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</div>
<iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
    position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
    frameborder="0" width="174" scrolling="no" height="189"></iframe>

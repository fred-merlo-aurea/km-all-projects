<%@ Control Language="c#" Inherits="ecn.communicator.main.SMSWizard.Controls.BlastSchedule" Codebehind="BlastSchedule.ascx.cs" %>

<script language="javascript">
	function IsNumber(source,arguments){
		var ValidChars = "0123456789";
		var Char;
		var sText=document.getElementById("ECNWizard_weekFrequency").value;
		for (i = 0; i < sText.length; i++)  { 
			Char = sText.charAt(i); 
			if (ValidChars.indexOf(Char) == -1)  {
				arguments.IsValid = false;
				alert('Only Numbers [0 - 9] are allowed in the Week Frequency field');
			}
		}
		return;
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
	
	function replyTo_focus(){
		document.BlastForm.ReplyTo.value = document.BlastForm.EmailFrom.value;
	}
	
	// Check if the specified date is valid.
	function isDateValid(source,arguments)	{
		syear = getobj("ECNWizard_YYYY_year");
		smonth = getobj("ECNWizard_MM_month");
		sday = getobj("ECNWizard_DD_day");

		maximum = maxDays(syear[syear.selectedIndex].value, smonth[smonth.selectedIndex].value, sday[sday.selectedIndex].value);
		
		if (maximum <  sday[sday.selectedIndex].value)	{
			sday[maximum-1].selected = true;
		}
		arguments.IsValid = true;
		return true;
	}

	// Returns the maximum day number in the specified month. Use leap year calculation.
	function maxDays(YYYY_year, MM_month, DD_day)
	{
		if (MM_month == 2)
			return (((YYYY_year % 4 == 0) && (YYYY_year % 100 != 0)) || (YYYY_year % 400 == 0)) ? 29 : 28;
		else
			return (MM_month == 4 || MM_month == 6 || MM_month == 9 || MM_month == 11) ? 30 : 31;
	}
	
</script>

<div class="section">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="4" align="center">
                <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
                    <table cellspacing="0" cellpadding="0" width="674" align="center">
                        <tr>
                            <td id="successTop">
                            </td>
                        </tr>
                        <tr>
                            <td id="successMiddle">
                                <table height="67" width="80%">
                                    <tr>
                                        <td valign="top" align="center" width="20%">
                                            <img src="/ecn.images/images/checkmark.gif"></td>
                                        <td valign="middle" align="left" width="80%" height="100%">
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="successBottom">
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
            </td>
        </tr>
    </table>
</div>
<div class="section bottomDiv">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td width="40" align="center" valign="middle">
                <img src="/ecn.images/images/sendMyCampaign.gif" alt="Send My Message Right Now!"></td>
            <td width="275" class="headingOne">
                Send My Message Right Now!</td>
            <td align="right">
                <asp:ImageButton ID="btnSendNow" runat="server" ImageUrl="/ecn.images/images/sendNowBtn.gif"
                    AlternateText="Send Now"></asp:ImageButton></td>
        </tr>
    </table>
</div>

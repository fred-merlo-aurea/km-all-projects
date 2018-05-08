<%@ Control Language="c#" Inherits="ecn.activityengines.includes.emailProfile_base" Codebehind="emailProfile_Base.ascx.cs" %>
<script language="javascript">	
	function isValidBirthDate(source,arguments)	{
		var datePat			= /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
		var BDtDateStr	= document.getElementById("ctl04_BirthDate").value;
		var matchArray = BDtDateStr.match(datePat); // is the format ok?
		if (matchArray == null) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !");			
			return;
		}
		month = matchArray[1]; // parse date into variables
		day = matchArray[3];
		year = matchArray[4];
		if (month < 1 || month > 12) { // check month range
			arguments.IsValid = false;
			alert("ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !");
			return;
		}
		if (day < 1 || day > 31) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !");
			return;
		}
		if ((month==4 || month==6 || month==9 || month==11) && day==31) {
			arguments.IsValid = false;		
			alert("ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !");
			return;
		}
		if (month == 2) { // check for february 29th
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day>29 || (day==29 && !isleap)) {
				arguments.IsValid = false;
				alert("ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !");				
				return;
			}
		}
		return;  // date is valid
	}
	function isValidUser1Date(source,arguments)	{
		var datePat			= /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
		var UDE1DateStr	= document.getElementById("ctl04_UserEvent1Date").value;
		var matchArray = UDE1DateStr.match(datePat);
		if (matchArray == null) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event1 Date' in MM/DD/YYYY format only. Please Correct !");				
			return;
		}
		month = matchArray[1]; // parse date into variables
		day = matchArray[3];
		year = matchArray[4];
		if (month < 1 || month > 12) { // check month range
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event1 Date' in MM/DD/YYYY format only. Please Correct !");				
			return;
		}
		if (day < 1 || day > 31) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event1 Date' in MM/DD/YYYY format only. Please Correct !");				
			return;
		}
		if ((month==4 || month==6 || month==9 || month==11) && day==31) {
			arguments.IsValid = false;		
			alert("ERROR: Enter 'User Event1 Date' in MM/DD/YYYY format only. Please Correct !");				
			return;
		}
		if (month == 2) { // check for february 29th
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day>29 || (day==29 && !isleap)) {
				arguments.IsValid = false;
				alert("ERROR: Enter 'User Event1 Date' in MM/DD/YYYY format only. Please Correct !");					
				return;
			}
		}
		return;  // date is valid
	}
	function isValidUser2Date(source,arguments)	{
		var datePat			= /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
		var UDE2DateStr	= document.getElementById("ctl04_UserEvent2Date").value;				
		var matchArray = UDE2DateStr.match(datePat);				
		if (matchArray == null) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event2 Date' in MM/DD/YYYY format only. Please Correct !");				
			return;
		}
		month = matchArray[1]; // parse date into variables
		day = matchArray[3];
		year = matchArray[4];
		if (month < 1 || month > 12) { // check month range
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event2 Date' in MM/DD/YYYY format only. Please Correct !");							
			return;
		}
		if (day < 1 || day > 31) {
			arguments.IsValid = false;
			alert("ERROR: Enter 'User Event2 Date' in MM/DD/YYYY format only. Please Correct !");							
			return;
		}
		if ((month==4 || month==6 || month==9 || month==11) && day==31) {
			arguments.IsValid = false;		
			alert("ERROR: Enter 'User Event2 Date' in MM/DD/YYYY format only. Please Correct !");							
			return;
		}
		if (month == 2) { // check for february 29th
			var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
			if (day>29 || (day==29 && !isleap)) {
				arguments.IsValid = false;
				alert("ERROR: Enter 'User Event2 Date' in MM/DD/YYYY format only. Please Correct !");								
				return;
			}
		}
		return;  // date is valid
	}				
	function isValidEmail(source,arguments)	{
		var fieldvalue	= document.getElementById("ctl04_EmailAddress").value;	
		if((fieldvalue.indexOf(".") > 2) && (fieldvalue.indexOf("@") > 0)) {
			return;
 		}else{
			arguments.IsValid = false;		 		
			alert("ERROR: Email Address is not the right format. Please Correct !");		
			return;	 			
 		}
	}
	function isValidPhone(source,arguments)	{
		var fieldvalue	= document.getElementById("ctl04_Voice").value;	
		if(fieldvalue.length > 7){
			return;
 		}else{
			arguments.IsValid = false;		 		
			alert("ERROR: Phone# is not the right format. Please Correct !");		 			
			return;			
 		}
	}
	function isValidState(source,arguments)	{
		var fieldvalue	= document.getElementById("ctl04_State").value;	
		if(fieldvalue.length > 0){
			return;
 		}else{
			arguments.IsValid = false;		 		
			alert("ERROR: Please select a State !");		
			return;			 			
 		}
	}		
	function isValidZip(source,arguments)	{
		var fieldvalue	= document.getElementById("ctl04_Zip").value;	
		if(eval(fieldvalue.length) > 4){
			return;
 		}else{
			arguments.IsValid = false;		 		
			alert("ERROR:  Zip Code not the right. Please Correct !");		
			return;			 			
 		}
	}	
</script>

<br>
<asp:Panel ID="FieldsValidationPanel" runat="server" Visible="False">
    <table cellspacing="2" cellpadding="2" width="70%" border="0">
        <tr>
            <td>
                <asp:CustomValidator ID="EmailAddress_Validator" runat="server" CssClass="errormsg"
                    ControlToValidate="EmailAddress" Enabled="true" ClientValidationFunction="isValidEmail"></asp:CustomValidator>
                <asp:CustomValidator ID="Voice_Validator" runat="server" CssClass="errormsg" ControlToValidate="Voice"
                    Enabled="true" ClientValidationFunction="isValidPhone"></asp:CustomValidator>
                <asp:CustomValidator ID="State_Validator" runat="server" CssClass="errormsg" ControlToValidate="State"
                    Enabled="true" ClientValidationFunction="isValidState"></asp:CustomValidator>
                <asp:CustomValidator ID="Zip_Validator" runat="server" CssClass="errormsg" ControlToValidate="Zip"
                    Enabled="true" ClientValidationFunction="isValidZip"></asp:CustomValidator>
                <asp:CustomValidator ID="DT_BirthDate" runat="server" CssClass="errormsg" ControlToValidate="BirthDate"
                    Enabled="true" ClientValidationFunction="isValidBirthDate"></asp:CustomValidator>
                <asp:CustomValidator ID="DT_UserEvent1Date" runat="server" CssClass="errormsg" ControlToValidate="UserEvent1Date"
                    Enabled="true" ClientValidationFunction="isValidUser1Date"></asp:CustomValidator>
                <asp:CustomValidator ID="DT_UserEvent2Date" runat="server" CssClass="errormsg" ControlToValidate="UserEvent2Date"
                    Enabled="true" ClientValidationFunction="isValidUser2Date"></asp:CustomValidator></td>
        </tr>
    </table>
</asp:Panel>
<table cellspacing="0" cellpadding="0" border="0" width="100%">
       <tr>
        <td  colspan="3" class="gradient" style="FONT-SIZE:13px; FONT-WEIGHT:bold">&nbsp;Profile Data</td>
    </tr>
</table>
<table cellspacing="2" cellpadding="2" border="0" class="greySidesB" width="100%" style="BACKGROUND-COLOR:#FFFFFF">
    <tr>
        <td colspan="3" align=left style="font-weight: bold; color: red"><asp:Label ID="MessageLabel" runat="server" CssClass="errormsg" Font-Bold="True" Visible="False"></asp:Label></td>
    </tr>

    <tr>
        <td class="formLabel" colspan="3">
            Email Address:&nbsp;
            <asp:TextBox ID="EmailAddress" runat="server" CssClass="formfield" Columns="60"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan=3>
            <table cellpadding=0 cellspacing=0 border=0 width=95%>
                <tr>
                   <td class="formLabel" style="width: 31%">Salutation:&nbsp;
                        <asp:TextBox ID="Title" runat="server" CssClass="formfield" Columns="10"></asp:TextBox></td>
                    <td class="formLabel" style="width: 31%">First Name:&nbsp;
                        <asp:TextBox ID="FirstName" runat="server" CssClass="formfield" Columns="20"></asp:TextBox></td>
                    <td class="formLabel" style="width: 38%">Last Name:&nbsp;
                        <asp:TextBox ID="LastName" runat="server" CssClass="formfield" Columns="20"></asp:TextBox></td>
                </tr>
               <!--<tr>
                    <td class="formLabel" colspan=3 style="PADDING-TOP:4px">FullName:&nbsp;
                        <asp:TextBox ID="FullName" runat="server" CssClass="formfield" Columns="40"></asp:TextBox>
                    </td>
               </tr> -->
               <tr>
                   <td class="formLabel" style="PADDING-TOP:4px">Profile Password:&nbsp;
                        <asp:TextBox ID="Password" TextMode="Password" runat="server" CssClass="formfield" Columns="30"></asp:TextBox></td> 
                    <td class="formLabel" style="PADDING-TOP:4px">Overall Profile Bounce Score:&nbsp;
                        <asp:TextBox ID="BounceScore" runat="server" CssClass="formfield" Columns="2"></asp:TextBox></td>
                    <td class="formLabel" style="PADDING-TOP:4px">Overall Profile Soft Bounce Score:&nbsp;
                        <asp:TextBox ID="txtSoftBounceScore" runat="server" CssClass="formfield" Columns="2"></asp:TextBox></td>   
               </tr>
            </table> 
        </td>
    </tr>
    <tr>
        <td style="height: 1pt" colspan="3">
            <hr color="#000000" size="1">
        </td>
    </tr>
    <tr>
        <td class="formLabel" colspan="2">
            Company:
            <asp:TextBox ID="CompanyName" runat="server" CssClass="formfield" Columns="35"></asp:TextBox></td>
        <td class="formLabel">
            Occupation:
            <asp:TextBox ID="Occupation" runat="server" CssClass="formfield" Columns="20"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formLabel" colspan="2">
            Address:
            <asp:TextBox ID="Address" runat="server" CssClass="formfield" Columns="40"></asp:TextBox></td>
        <td class="formLabel">
            Address 2:
            <asp:TextBox ID="Address2" runat="server" CssClass="formfield" Columns="20"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formLabel">
            City:
            <asp:TextBox ID="City" runat="server" CssClass="formfield" Columns="25"></asp:TextBox></td>
        <td class="formLabel" valign=bottom>
            State/Zip:
            <asp:DropDownList class="formfield" ID="State" runat="server"></asp:DropDownList>&nbsp;
            <asp:TextBox ID="Zip" runat="server" CssClass="formfield"
                Columns="10"></asp:TextBox></td>
        <td class="formLabel" style="width: 38%">
            Country:
            <asp:TextBox ID="Country" runat="server" CssClass="formfield" Columns="20"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
    </tr>
    <tr>
        <td style="height: 1pt" colspan="3">
            <hr color="#000000" size="1">
        </td>
    </tr>
    <tr>
        <td class="formLabel" style="width: 31%">
            Phone:
            <asp:TextBox class="formfield" ID="Voice" runat="server" CssClass="formfield" Columns="12"></asp:TextBox></td>
        <td class="formLabel" style="width: 31%">
            Mobile:
            <asp:TextBox class="formfield" ID="Mobile" runat="server" CssClass="formfield" Columns="12"></asp:TextBox></td>
        <td class="formLabel" style="width: 38%">
            Fax:
            <asp:TextBox class="formfield" ID="Fax" runat="server" CssClass="formfield" Columns="12"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="width: 140px">
        </td>
        <td style="width: 218px">
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="formLabel" style="width: 31%">
            Income:
            <asp:TextBox ID="Income" runat="server" CssClass="formfield" Columns="10"></asp:TextBox></td>
        <td class="formLabel" style="width: 31%">
            Gender:
            <asp:DropDownList class="formfield" ID="Gender" runat="server">
                <asp:ListItem Value=''></asp:ListItem>
                <asp:ListItem Value='Male'>Male</asp:ListItem>
                <asp:ListItem Value='Female'>Female</asp:ListItem>
            </asp:DropDownList></td>
        <td class="formLabel" style="width: 38%">
            Age:
            <asp:TextBox ID="Age" runat="server" CssClass="formfield" Columns="2"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formLabel" colspan="2">
            Website:
            <asp:TextBox ID="Website" runat="server" CssClass="formfield" Columns="40"></asp:TextBox></td>
        <td class="formLabel">
            Birthdate:
            <asp:TextBox ID="BirthDate" runat="server" CssClass="formfield" Columns="25" Width="80px"
                MaxLength="10"></asp:TextBox>&nbsp;<font style="font-weight: normal" face="Verdana"
                    size="1">[MM/DD/YYYY]</font></td>
    </tr>
   <tr>
    <td height=2></td>
   </tr> 
  </table>
  &nbsp;
<table cellspacing="0" cellpadding="0" border="0" width="100%">
       <tr>
        <td  colspan="3" class="gradient" style="FONT-SIZE:13px; FONT-WEIGHT:bold">&nbsp;Other User Data</td>
    </tr>
</table>
<table cellspacing="2" cellpadding="2" border="0" class="greySidesB" width="100%" style="BACKGROUND-COLOR:#FFFFFF">
    <tr>
        <td colspan="3">
            <table cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td class="formLabel">
                        User1:&nbsp;<asp:TextBox ID="User1" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                    <td class="formLabel">
                        User2:&nbsp;<asp:TextBox ID="User2" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                    <td class="formLabel">
                        User3:&nbsp;<asp:TextBox ID="User3" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formLabel">
                        User4:&nbsp;<asp:TextBox ID="User4" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                    <td class="formLabel">
                        User5:&nbsp;<asp:TextBox ID="User5" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                    <td class="formLabel">
                        User6:&nbsp;<asp:TextBox ID="User6" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <table cellspacing="1" cellpadding="1" width="100%">
                            <tr>
                                <td class="formLabel">
                                    User Event1:&nbsp;<asp:TextBox ID="UserEvent1" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                                <td class="formLabel">
                                    User Event1 Date:&nbsp;<asp:TextBox ID="UserEvent1Date" runat="server" Columns="25"
                                        Width="80px" MaxLength="10" CssClass="formfield"></asp:TextBox>&nbsp;<font style="font-weight: normal"
                                            face="Verdana" size="1">[MM/DD/YYYY]</font></td>
                            </tr>
                            <tr>
                                <td class="formLabel">
                                    User Event2:&nbsp;<asp:TextBox ID="UserEvent2" runat="server" Columns="25" CssClass="formfield"></asp:TextBox></td>
                                <td class="formLabel">
                                    User Event2 Date:&nbsp;<asp:TextBox ID="UserEvent2Date" runat="server" Columns="25"
                                        Width="80px" MaxLength="10" CssClass="formfield"></asp:TextBox>&nbsp;<font style="font-weight: normal"
                                            face="Verdana" size="1">[MM/DD/YYYY]</font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" class="formLabel" align=center>.
                <asp:Button class="formfield" ID="EditProfileButton" runat="server" Text="Update Profile"
                    OnClick="UpdateEmail"></asp:Button>
        </td>
    </tr>
</table>

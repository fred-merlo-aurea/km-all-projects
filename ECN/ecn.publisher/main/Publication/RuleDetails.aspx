
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleDetails.aspx.cs" Inherits="ecn.publisher.main.Publication.RuleDetails"
    MasterPageFile="~/MasterPages/Publisher.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript">

    function IsNumber(source, arguments) {
        var ValidChars = "0123456789.";
        var Char;
        var sText = document.getElementById("CompValue").value;
        for (i = 0; i < sText.length; i++) {
            Char = sText.charAt(i);
            if (ValidChars.indexOf(Char) == -1) {
                arguments.IsValid = false;
                return;
            }
        }
        return;
    }

    function isValidDateFrom(source, arguments) {
        // Checks for the date format MM/DD/YYYY
        // Also separates date into month, day, and year variables
        var datePat = /^(\d{1,2})(\/)(\d{1,2})\2(\d{4})$/;
        var dateStr = document.getElementById("DtTime_Value1").value;
        if ((dateStr.substring(0, 7) == 'Today [') || dateStr == 'Today') {
            return;
        } else {
            var matchArray = dateStr.match(datePat); // is the format ok?
            if (matchArray == null) {
                arguments.IsValid = false;
                return;
            }
            month = matchArray[1]; // parse date into variables
            day = matchArray[3];
            year = matchArray[4];
            if (month < 1 || month > 12) { // check month range
                arguments.IsValid = false;
                return;
            }
            if (day < 1 || day > 31) {
                arguments.IsValid = false;
                return;
            }
            if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
                arguments.IsValid = false;
                return;
            }
            if (month == 2) { // check for february 29th
                var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                if (day > 29 || (day == 29 && !isleap)) {
                    arguments.IsValid = false;
                    return;
                }
            }
            return;  // date is valid
        }
    }

    function isValidDateTo(source, arguments) {
        // Checks for the date format MM/DD/YYYY
        // Also separates date into month, day, and year variables
        var datePat = /^(\d{1,2})(\/)(\d{1,2})\2(\d{4})$/;
        var dateStr = document.getElementById("DtTime_Value2").value;
        if ((dateStr.substring(0, 7) == 'Today [') || dateStr == 'Today') {
            return;
        } else {
            var matchArray = dateStr.match(datePat); // is the format ok?
            if (matchArray == null) {
                arguments.IsValid = false;
                return;
            }
            month = matchArray[1]; // parse date into variables
            day = matchArray[3];
            year = matchArray[4];
            if (month < 1 || month > 12) { // check month range
                arguments.IsValid = false;
                return;
            }
            if (day < 1 || day > 31) {
                arguments.IsValid = false;
                return;
            }
            if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31) {
                arguments.IsValid = false;
                return;
            }
            if (month == 2) { // check for february 29th
                var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
                if (day > 29 || (day == 29 && !isleap)) {
                    arguments.IsValid = false;
                    return;
                }
            }
            return;  // date is valid
        }
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tr>
            <td class="tableHeader">
                Rule Name&nbsp;:&nbsp;
                <asp:label id="lblRuleName" runat="Server" cssclass="errormsg" visible="true"></asp:label>
                &nbsp;for&nbsp;Edition&nbsp;:&nbsp;
                <asp:label id="lblEditionName" runat="Server" cssclass="errormsg" visible="true"></asp:label>
            </td>
            <td>
            </td>
            <td class="tableHeader" align='right'>
            </td>
        </tr>
        <tr class="tableHeader" align="left">
            <td class="tableHeader" colspan='3'>
                <br />
                Demographic Targeting Rules<br />
                <asp:datagrid id="dgRuleDetails" runat="Server" cssclass="grid" visible="true" width="100%"
                    autogeneratecolumns="False" horizontalalign="Center" datakeyfield="ruleDetailID">
					<ItemStyle></ItemStyle>
					<HeaderStyle CssClass="gridheader"></HeaderStyle>
					<FooterStyle CssClass="tableHeader1"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="FieldName" HeaderText="Targeted Field"></asp:BoundColumn>
						<asp:BoundColumn DataField="Comparator" HeaderText="Function" HeaderStyle-Width="10%" HeaderStyle-Horizontalalign="center"
							ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
						<asp:BoundColumn DataField="CompareValue" HeaderText="Targeted Value"></asp:BoundColumn>
						<asp:BoundColumn DataField="CompareType" HeaderText="Logic Gate" HeaderStyle-Width="15%" HeaderStyle-Horizontalalign="center"
							ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
						<asp:TemplateColumn itemstyle-width="5%" Headertext="Delete" headerstyle-HorizontalAlign="Center" Itemstyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:LinkButton id="lnkbutDelete" runat="Server" Text="<img border='0' src=/ecn.images/images/icon-delete1.gif alt=Delete>"
									CommandName="Delete" CausesValidation="false"></asp:LinkButton>
							</ItemTemplate>
						</asp:TemplateColumn>							
					</Columns>
				</asp:datagrid>
            </td>
        </tr>
        <tr>
            <td colspan='3'>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan='3'>
                <table cellspacing="0" cellpadding="2" width="100%" border='0'>
                    <tr class="gridheader">
                        <td>
                            Targeted Field</td>
                        <td>
                            &nbsp;&nbsp;</td>
                        <td>
                            Function</td>
                        <td>
                            Targeted Value</td>
                        <td>
                            Logic Gate</td>
                        <td>
                            &nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:dropdownlist id="CompFieldName" runat="Server" cssclass="formfield">
								<asp:ListItem value="City">City</asp:ListItem>
								<asp:ListItem value="State">State</asp:ListItem>
								<asp:ListItem value="Zip">Zip</asp:ListItem>
								<asp:ListItem value="Country">Country</asp:ListItem>
								<asp:ListItem value="Voice">Voice</asp:ListItem>
							</asp:dropdownlist>
                            <br />
                            <asp:label id="Label1" runat="Server" cssclass="bodyText " visible="true">
								<font color="FF0000"><b>Convert Targeted Field to:</b></font></asp:label>
                            <br />
                            <asp:dropdownlist id="ConvertToDataType" runat="Server" cssclass="formfield" width="145"
                                autopostback="true" onselectedindexchanged="ConvertToDataType_SelectedIndexChanged">
								<asp:ListItem Selected="True" Value=""></asp:ListItem>
								<asp:ListItem value="VARCHAR">String</asp:ListItem>
								<asp:ListItem value="INT">Number</asp:ListItem>
							</asp:dropdownlist>
                        </td>
                        <td class="tableHeader" valign="top">
                            <asp:checkbox id="ComparatorChkBox" runat="Server" textalign='right' text="NOT"></asp:checkbox>
                        </td>
                        <td valign="top" align="center">
                            <asp:dropdownlist id="Comparator" runat="Server" cssclass="formfield" width="130"></asp:dropdownlist>
                        </td>
                        <td valign="top" align="center">
                            <asp:panel id="Default_CompareValuePanel" runat="Server" visible="true">
								<asp:textbox class="formfield" id="CompValue" runat="Server" columns="25" EnableViewState="true"></asp:textbox>
							</asp:panel>
                            <asp:panel id="DtTime_CompareValuePanel" runat="Server" visible="false">
								<TABLE>
									<tr>
										<td align='right'><FONT class="bodyText" color="#ff0000">From:</FONT></td>
										<td>
											<asp:textbox class="formfield" id="DtTime_Value1" runat="Server" columns="15" EnableViewState="true"
												MaxLength="10"></asp:textbox>&nbsp;<IMG onclick="window.open('/ecn.communicator/includes/calendarPopUp_From.htm', 'ToDate', 'left=10,top=10,height=145,width=255,resizable=no,scrollbar=no,status=no');"
												alt="Click to Set START Date" src="/ecn.images/images/icon-calendar.gif" border='0'>
										</td>
									</tr>
									<tr>
										<td align='right'><FONT class="bodyText" color="#ff0000">To: </FONT>
										</td>
										<td>
											<asp:textbox class="formfield" id="DtTime_Value2" runat="Server" columns="15" EnableViewState="true"
												MaxLength="10"></asp:textbox>&nbsp;<IMG onclick="window.open('/ecn.communicator/includes/calendarPopUp_To.htm', 'ToDate', 'left=10,top=10,height=145,width=235,resizable=no,scrollbar=no,status=no');"
												alt="Click to Set END DATE" src="/ecn.images/images/icon-calendar.gif" border='0'>
										</td>
									</tr>
								</TABLE>
							</asp:panel>
                        </td>
                        <td valign="top" align="center">
                            <asp:dropdownlist id="CompType" runat="Server" cssclass="formfield">
								<asp:ListItem Selected="True" Value="AND"> AND </asp:ListItem>
								<asp:ListItem value="OR"> OR </asp:ListItem>
							</asp:dropdownlist>
                        </td>
                        <td class="tableHeader" valign="top" align='right'>
                            <asp:button class="formbuttonsmall" id="AddRules" runat="Server" text="Add this Rule"
                                visible="true" onclick="AddRules_Click"></asp:button>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="6">
                            <asp:label id="ErrorLabel" runat="Server" cssclass="errormsg" visible="false"></asp:label>
                            <br />
                            <asp:customvalidator id="CompValueNumberValidator" runat="Server" cssclass="errormsg"
                                errormessage="" controltovalidate="CompValue" clientvalidationfunction="IsNumber"
                                enabled="false">ERROR: Compare Value that you have entered cannot be converted to Number. Only Numbers are Allowed. Please Correct !</asp:customvalidator>
                            <br />
                            <asp:customvalidator id="DtTime_Value1Validator" runat="Server" cssclass="errormsg"
                                errormessage="" controltovalidate="DtTime_Value1" clientvalidationfunction="isValidDateFrom"
                                enabled="true">ERROR: 'From Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:customvalidator>
                            <br />
                            <asp:customvalidator id="DtTime_Value2Validator" runat="Server" cssclass="errormsg"
                                errormessage="" controltovalidate="DtTime_Value2" clientvalidationfunction="isValidDateTo"
                                enabled="true">ERROR: 'To Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:customvalidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


   
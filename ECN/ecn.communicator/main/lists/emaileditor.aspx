<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.emaileditor" CodeBehind="emaileditor.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function isValidBirthDate(source, arguments) {
            var datePat = /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
            var BDtDateStr = document.getElementById('<%=BirthDate.ClientID%>').value;
            var matchArray = BDtDateStr.match(datePat); // is the format ok?
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
        function isValidUser1Date(source, arguments) {
            var datePat = /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
            var UDE1DateStr = document.getElementById('<%=UserEvent1Date.ClientID%>').value;
            var matchArray = UDE1DateStr.match(datePat);
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
        function isValidUser2Date(source, arguments) {
            var datePat = /^(\d{2})(\/)(\d{2})\2(\d{4})$/;
            var UDE2DateStr = document.getElementById('<%=UserEvent2Date.ClientID%>').value;
            var matchArray = UDE2DateStr.match(datePat);
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

        function popManagerWindow(navURL) {
            if (navURL != null) {
                window.open(navURL, 'newwindow', 'toolbar=no,status=yes,menubar=no,scrollbars=1,location=no,resizable=yes');
            }
            return false;
        } 
    </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
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
                                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                                    </td>
                                                    <td valign="middle" align="left" width="80%" height="100%">
                                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorBottom">
                                        </td>
                                    </tr>
                                </table>
    </asp:PlaceHolder>
    <asp:Panel ID="HeaderPanel" runat="Server">
        <table class="tableContent" id="headerWrapper" cellspacing="1" cellpadding="1" width="100%"
            border='0'>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="70">
                    &nbsp;
                </td>
                <td class="tableHeader" colspan="2" align="left"> 
                    <asp:Button class="formbuttonsmall" ID="DetailsButton" OnClick="ViewDetails" runat="Server"
                        Enabled="false" Text="view details" Visible="true"></asp:Button>&nbsp;&nbsp;
                    <asp:Button class="formbuttonsmall" ID="NotesButton" OnClick="ViewNotes" runat="Server"
                        Text="view notes" Visible="true"></asp:Button>&nbsp;&nbsp;
                    <asp:Button class="formbuttonsmall" ID="LogButton" OnClick="ViewLog" runat="Server"
                        Text="view log" Visible="true"></asp:Button>&nbsp;&nbsp;
                    <asp:Button class="formbuttonsmall" ID="ProfileManagerButton" runat="Server" Text="Profile Manager"
                        Visible="true"></asp:Button>&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="40">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                </td>
                <td  align="left">
                    EmailAddress&nbsp;
                    <asp:TextBox ID="EmailAddress" runat="Server" CssClass="formfield" Columns="60"></asp:TextBox>
                </td>
                <td  align="left">
                    Password&nbsp;
                    <asp:TextBox ID="Password" runat="Server" CssClass="formfield" Columns="25" MaxLength="25"></asp:TextBox>
                </td>
            </tr>
            <asp:Panel ID="FormatPanel" runat="Server" Visible="False">
                <tr>
                    <td class="tableHeader" valign="top" align='right'>
                    </td>
                    <td valign="bottom" height="26"  align="left">
                        FormatTypeCode&nbsp;
                        <asp:DropDownList ID="FormatTypeCode" runat="Server" CssClass="formfield" DataTextField="CodeName"
                            DataValueField="CodeValue">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        Bounce Score&nbsp;
                        <asp:TextBox ID="BounceScore" runat="Server" CssClass="formfield" Columns="2" ReadOnly></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" align='right'>
                    </td>
                    <td align="left">
                        SubscribeTypeCode&nbsp;
                        <asp:DropDownList ID="SubscribeTypeCode" runat="Server" CssClass="formfield" DataTextField="CodeName"
                            DataValueField="CodeValue">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        Soft Bounce Score&nbsp;
                        <asp:TextBox ID="txtSoftBounceScore" runat="Server" CssClass="formfield" Columns="2" ReadOnly></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
        </table>
    </asp:Panel>
    <asp:Panel ID="DetailsPanel" runat="Server">
        <table class="tableContent" id="detailsWrapper" cellspacing="1" cellpadding="1" width="100%"
            border='0'>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    Title
                </td>
                <td align="left">
                    FirstName
                </td>
                <td align="left">
                    LastName
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTitle" runat="Server" CssClass="formfield" Columns="10"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="FirstName" runat="Server" CssClass="formfield" Columns="15"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="LastName" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3' align="left">
                    FullName
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3' align="left">
                    <asp:TextBox ID="FullName" runat="Server" CssClass="formfield" Columns="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td colspan="2" align="left">
                    Company
                </td>
                <td align="left">
                    Occupation
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="CompanyName" runat="Server" CssClass="formfield" Columns="35"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="Occupation" runat="Server" CssClass="formfield" Columns="35"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td colspan="2" align="left">
                    Address
                </td>
                <td align="left">
                    Address 2
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="Address" runat="Server" CssClass="formfield" Columns="40"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="Address2" runat="Server" CssClass="formfield" Columns="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan="2" align="left">
                    City
                </td>
                <td align="left">
                    State/Zip
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="City" runat="Server" CssClass="formfield" Columns="35"></asp:TextBox>
                </td>
                <td align="left">
                      <asp:TextBox ID="State" runat="Server" CssClass="formfield" Columns="10"></asp:TextBox>
                    <asp:TextBox ID="Zip" runat="Server" CssClass="formfield" Columns="10"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3' align="left">
                    Country
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td colspan='3' align="left">
                    <asp:TextBox ID="Country" runat="Server" CssClass="formfield" Columns="20"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    Voice
                </td>
                <td align="left">
                    Mobile
                </td>
                <td align="left">
                    Fax
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="Voice" runat="Server" CssClass="formfield" Columns="18"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="Mobile" runat="Server" CssClass="formfield" Columns="18"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="Fax" runat="Server" CssClass="formfield" Columns="18"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    Income
                </td>
                <td align="left">
                    Gender
                </td>
                <td align="left">
                    Website
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="Income" runat="Server" CssClass="formfield" Columns="18"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="Gender" runat="Server" CssClass="formfield" Columns="18"></asp:TextBox>
                </td>
                <td align="left">
                    <asp:TextBox ID="Website" runat="Server" CssClass="formfield" Columns="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    Age
                </td>
                <td colspan="2" align="left">
                    Birthdate
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="Age" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="BirthDate" runat="Server" CssClass="formfield" Columns="25" MaxLength="10"
                        Width="80px"></asp:TextBox>&nbsp;[MM/DD/YYYY]
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right'>
                </td>
                <td align='right' colspan='3'>
                    <img alt="Add / Edit User Defined Fields" src="/ecn.images/images/icon-groupFields.gif"
                        border='0'>&nbsp;
                    <asp:HyperLink ID="UserDefLink" runat="Server" Visible="False" NavigateUrl="emaildataeditor.aspx">access other UDF Data</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User1&nbsp;:&nbsp;
                    <asp:TextBox ID="User1" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    User Event1&nbsp;:&nbsp;
                    <asp:TextBox ID="UserEvent1" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User2&nbsp;:&nbsp;
                    <asp:TextBox ID="User2" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    User Event1 Date&nbsp;:&nbsp;
                    <asp:TextBox ID="UserEvent1Date" runat="Server" CssClass="formfield" Columns="25"
                        MaxLength="10" Width="80px"></asp:TextBox>&nbsp;[MM/DD/YYYY]
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User3&nbsp;:&nbsp;
                    <asp:TextBox ID="User3" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User4&nbsp;:&nbsp;
                    <asp:TextBox ID="User4" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    User Event2&nbsp;:&nbsp;
                    <asp:TextBox ID="UserEvent2" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User5&nbsp;:&nbsp;
                    <asp:TextBox ID="User5" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2" align="left">
                    User Event2 Date&nbsp;:&nbsp;
                    <asp:TextBox ID="UserEvent2Date" runat="Server" CssClass="formfield" Columns="25"
                        MaxLength="10" Width="80px"></asp:TextBox>&nbsp;[MM/DD/YYYY]
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td align="left">
                    User6&nbsp;:&nbsp;
                    <asp:TextBox ID="User6" runat="Server" CssClass="formfield" Columns="25"></asp:TextBox>
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3' align="left">
                    <asp:CustomValidator ID="DT_BirthDate" runat="Server" ErrorMessage="ERROR: Enter 'BirthDate' in MM/DD/YYYY format only. Please Correct !"
                        ControlToValidate="BirthDate" ClientValidationFunction="isValidBirthDate" Enabled="true"
                        CssClass="errormsg"></asp:CustomValidator><br />
                    <asp:CustomValidator ID="DT_UserEvent1Date" runat="Server" ErrorMessage="ERROR: Enter 'UserEvent1Date' in MM/DD/YYYY format only. Please Correct !"
                        ControlToValidate="UserEvent1Date" ClientValidationFunction="isValidUser1Date"
                        Enabled="true" CssClass="errormsg"></asp:CustomValidator><br />
                    <asp:CustomValidator ID="DT_UserEvent2Date" runat="Server" ErrorMessage="ERROR: Enter 'UserEvent2Date' in MM/DD/YYYY format only. Please Correct !"
                        ControlToValidate="UserEvent2Date" ClientValidationFunction="isValidUser2Date"
                        Enabled="true" CssClass="errormsg"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td colspan='3'>
                    <hr size="1" color="#999999">
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td class="tableHeader" align="center" colspan='3'>
                    <asp:TextBox ID="EmailID" runat="Server" Visible="false" EnableViewState="true"></asp:TextBox>
                    <asp:Button class="formbutton" ID="UpdateButton" OnClick="UpdateEmail" runat="Server"
                        Text="Update"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="NotesPanel" runat="Server" Visible="False">
        <table class="tableContent" id="logWrapper" cellspacing="1" cellpadding="1" width="100%"
            border='0'>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td align="left">
                    <hr>
                    Notes<br />
                    <asp:TextBox ID="NotesBox" runat="Server" Columns="70" TextMode="multiline" Rows="25"
                        Wrap="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                </td>
                <td class="tableHeader" align="center" colspan='3'>
                    <asp:Button class="formbutton" ID="NotesUpdateButton" OnClick="UpdateNotes" runat="Server"
                        Text="Update"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="LogPanel" runat="Server" Visible="False">
        <table class="tableContent" id="logWrapper" cellspacing="1" cellpadding="1" width="100%"
            border='0'>
            <tr>
                <td class="tableHeader" valign="top" align='right' width="74">
                    &nbsp;
                </td>
                <td>
                    <hr/>
                    <asp:DataGrid ID="LogGrid" runat="Server" OnItemDataBound="LogGrid_ItemDataBound" Width="100%" CellPadding="2" AutoGenerateColumns="False">
                        <ItemStyle CssClass="tableContentSmall"></ItemStyle>
                        <HeaderStyle CssClass="tableHeader1"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Time" ItemStyle-HorizontalAlign="Left" >
                            </asp:BoundColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="20%" ItemStyle-CssClass="subject" ItemStyle-HorizontalAlign="Left" DataTextField="EmailSubject" DataTextFormatString="<font size=-2>{0}</font>"
                                HeaderText="Blast" DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}">
                            </asp:HyperLinkColumn>
                            <asp:BoundColumn DataField="BlastID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" HeaderText="Blast ID" />
                            <asp:TemplateColumn ItemStyle-Width="10%" HeaderText="Type" ItemStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Label ID="lblActionTypeCode" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn ItemStyle-Width="40%" DataField="ActionValue" HeaderText="Value" ItemStyle-HorizontalAlign="Left" >
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="LogPager" runat="Server" Width="100%" PageSize="50" ControlToPage="LogGrid"
                        OnIndexChanged="LogPager_IndexChanged">
                        <PagerStyle Font-Size="X-Small"></PagerStyle>
                    </AU:PagerBuilder>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

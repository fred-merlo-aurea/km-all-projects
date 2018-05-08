<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.filters" CodeBehind="filtersOld.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteFilter(theID) {
            if (confirm('Are you Sure?\n Selected Filter & corresponding FilterDetails will be permanently deleted.')) {
                window.location = "filters.aspx?FilterID=" + theID + "&action=deleteFilter";
            }
        }
        function deleteFilterDetail(theID) {
            if (confirm('Are you Sure?\n Selected FilterDetail will be permanently deleted.')) {
                window.location = "filters.aspx?FilterDetailID=" + theID + "&action=deleteFilterDetail";
            }
        }
        function IsNumber(source, arguments) {
            var ValidChars = "0123456789.";
            var Char;
            var sText = document.getElementById('<%=CompValue.ClientID%>').value;
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
            var dateStr = document.getElementById('<%=DtTime_Value1.ClientID%>').value;
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
            var dateStr = document.getElementById('<%=DtTime_Value2.ClientID%>').value;
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
        <tbody>
            <asp:Panel ID="FilterPreviewPanel" runat="Server" Visible="false">
                <tr>
                    <td class="tableHeader" align="left">
                        Filter Name&nbsp;&nbsp;
                        <asp:Label ID="FilterNameTxtLabel" runat="Server" Visible="true" CssClass="errormsg"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="FilterNameTxtValue" runat="Server" Visible="false" Enable="false"></asp:TextBox>
                        <asp:TextBox ID="FilterIDValue" runat="Server" Visible="false" Enable="false" Text="0"></asp:TextBox>
                    </td>
                    <td class="tableHeader" align='right'>
                        <asp:Button class="formbuttonsmall" ID="PreviewFilterButton" runat="Server" Visible="true"
                            Text="Preview filtered e-mails"></asp:Button>&nbsp;&nbsp;
                        <asp:Button class="formbuttonsmall" ID="FilterListButton" OnClick="FiltersList" runat="Server"
                            Visible="true" Text="Return to Filters List"></asp:Button>
                    </td>
                </tr>
                <tr class="tableHeader" align="left">
                    <td class="tableHeader" colspan='3'>
                        <br />
                        Demographic Targeting Rules<br />
                        <asp:DataGrid ID="AddFiltersGridArray" runat="Server" HorizontalAlign="Center" AutoGenerateColumns="False"
                            Width="100%" CssClass="grid">
                            <ItemStyle></ItemStyle>
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <Columns>
                                <asp:BoundColumn DataField="FieldName" HeaderText="Targeted Field"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Comparator" HeaderText="Function" HeaderStyle-Width="10%"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CompareValue" HeaderText="Targeted Value"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CompareType" HeaderText="Logic Gate" HeaderStyle-Width="15%"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                    <ItemTemplate>
                                        <a href='javascript:deleteFilterDetail(<%# DataBinder.Eval(Container.DataItem, "FDID")%>);'>
                                            <center>
                                                <img src='/ecn.images/images/icon-delete1.gif' alt='Delete this Filter Attribute'></center>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td width="100%" class="tableHeader" align="left" colspan='3'>
                    <table cellspacing="1" cellpadding="3" width="100%" border='0'>
                        <tbody>
                            <asp:Panel ID="FilterAttribsPanel" runat="Server" Visible="false">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="gridheader">
                                    <td>
                                        Targeted Field
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        Function
                                    </td>
                                    <td>
                                        Targeted Value
                                    </td>
                                    <td>
                                        Logic Gate
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CompFieldName" runat="Server" CssClass="formfield">
                                            <asp:ListItem Selected="True" Value="EmailAddress">EmailAddress</asp:ListItem>
                                            <asp:ListItem Value="FormatTypeCode">FormatTypeCode</asp:ListItem>
                                            <asp:ListItem Value="SubscribeTypeCode">SubscribeTypeCode</asp:ListItem>
                                            <asp:ListItem Value="Title">Title</asp:ListItem>
                                            <asp:ListItem Value="FirstName">FirstName</asp:ListItem>
                                            <asp:ListItem Value="LastName">LastName</asp:ListItem>
                                            <asp:ListItem Value="FullName">FullName</asp:ListItem>
                                            <asp:ListItem Value="Company">Company</asp:ListItem>
                                            <asp:ListItem Value="Occupation">Occupation</asp:ListItem>
                                            <asp:ListItem Value="Address">Address</asp:ListItem>
                                            <asp:ListItem Value="Address2">Address2</asp:ListItem>
                                            <asp:ListItem Value="City">City</asp:ListItem>
                                            <asp:ListItem Value="State">State</asp:ListItem>
                                            <asp:ListItem Value="Zip">Zip</asp:ListItem>
                                            <asp:ListItem Value="Country">Country</asp:ListItem>
                                            <asp:ListItem Value="Voice">Voice</asp:ListItem>
                                            <asp:ListItem Value="Mobile">Mobile</asp:ListItem>
                                            <asp:ListItem Value="Fax">Fax</asp:ListItem>
                                            <asp:ListItem Value="Website">Website</asp:ListItem>
                                            <asp:ListItem Value="Age">Age</asp:ListItem>
                                            <asp:ListItem Value="Income">Income</asp:ListItem>
                                            <asp:ListItem Value="Gender">Gender</asp:ListItem>
                                            <asp:ListItem Value="User1">User1</asp:ListItem>
                                            <asp:ListItem Value="User2">User2</asp:ListItem>
                                            <asp:ListItem Value="User3">User3</asp:ListItem>
                                            <asp:ListItem Value="User4">User4</asp:ListItem>
                                            <asp:ListItem Value="User5">User5</asp:ListItem>
                                            <asp:ListItem Value="User6">User6</asp:ListItem>
                                            <asp:ListItem Value="Birthdate">Birthdate [MM/DD/YYYY]</asp:ListItem>
                                            <asp:ListItem Value="UserEvent1">UserEvent1</asp:ListItem>
                                            <asp:ListItem Value="UserEvent1Date">UserEvent1Date [MM/DD/YYYY]</asp:ListItem>
                                            <asp:ListItem Value="UserEvent2">UserEvent2</asp:ListItem>
                                            <asp:ListItem Value="UserEvent2Date">UserEvent2Date [MM/DD/YYYY]</asp:ListItem>
                                            <asp:ListItem Value="Notes">Notes</asp:ListItem>
                                            <asp:ListItem Value="CreatedOn">Profile Added [MM/DD/YYYY]</asp:ListItem>
                                            <asp:ListItem Value="LastChanged">Profile Updated [MM/DD/YYYY]</asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="Label1" runat="Server" Visible="true" CssClass="bodyText ">
											<font color="FF0000"><b>Convert Targeted Field to:</b></font></asp:Label><br />
                                        <asp:DropDownList ID="ConvertToDataType" runat="Server" CssClass="formfield" Width="145"
                                            AutoPostBack="true" OnSelectedIndexChanged="ConvertToDataType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                            <asp:ListItem Value="VARCHAR">String</asp:ListItem>
                                            <asp:ListItem Value="INT">Number</asp:ListItem>
                                            <asp:ListItem Value="DATETIME">Date [MM/DD/YYYY]</asp:ListItem>
                                            <asp:ListItem Value="DECIMAL(11,2)">Money $$</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tableHeader" valign="top">
                                        <asp:CheckBox ID="ComparatorChkBox" runat="Server" Text="NOT" TextAlign='right'>
                                        </asp:CheckBox>
                                    </td>
                                    <td valign="top" align="center">
                                        <asp:DropDownList ID="Comparator" runat="Server" CssClass="formfield" Width="130">
                                        </asp:DropDownList>
                                    </td>
                                    <td valign="top" align="center">
                                        <asp:Panel ID="Default_CompareValuePanel" runat="Server" Visible="true">
                                            <asp:TextBox class="formfield" ID="CompValue" runat="Server" EnableViewState="true"
                                                Columns="25"></asp:TextBox>
                                        </asp:Panel>
                                        <asp:Panel ID="DtTime_CompareValuePanel" runat="Server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td align='right'>
                                                        <font class="bodyText" color="#ff0000">From:</font>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox class="formfield" ID="DtTime_Value1" runat="Server" EnableViewState="true"
                                                            Columns="15" MaxLength="10"></asp:TextBox>&nbsp;<img onclick="window.open('../../includes/calendarPopUp_From.htm', 'ToDate', 'left=10,top=10,height=145,width=255,resizable=no,scrollbar=no,status=no');"
                                                                alt="Click to Set START Date" src="/ecn.images/images/icon-calendar.gif" border='0'>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='right'>
                                                        <font class="bodyText" color="#ff0000">To: </font>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox class="formfield" ID="DtTime_Value2" runat="Server" EnableViewState="true"
                                                            Columns="15" MaxLength="10"></asp:TextBox>&nbsp;<img onclick="window.open('../../includes/calendarPopUp_To.htm', 'ToDate', 'left=10,top=10,height=145,width=235,resizable=no,scrollbar=no,status=no');"
                                                                alt="Click to Set END DATE" src="/ecn.images/images/icon-calendar.gif" border='0'>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td valign="top" align="center">
                                        <asp:DropDownList ID="CompType" runat="Server" CssClass="formfield">
                                            <asp:ListItem Selected="True" Value="AND"> AND </asp:ListItem>
                                            <asp:ListItem Value="OR"> OR </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="tableHeader" valign="top" align='right'>
                                        <asp:Button class="formbuttonsmall" ID="AddFilters" OnClick="AddFilter" runat="Server"
                                            Visible="true" Text="Add this Filter"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="6">
                                        <asp:Label ID="ErrorLabel" runat="Server" Visible="false" CssClass="errormsg"></asp:Label><br />
                                        <asp:CustomValidator ID="CompValueNumberValidator" runat="Server" CssClass="errormsg"
                                            Enabled="false" ClientValidationFunction="IsNumber" ControlToValidate="CompValue"
                                            ErrorMessage="">ERROR: Compare Value that you have entered cannot be converted to Number. Only Numbers are Allowed. Please Correct !</asp:CustomValidator><br />
                                        <asp:CustomValidator ID="DtTime_Value1Validator" runat="Server" CssClass="errormsg"
                                            Enabled="true" ClientValidationFunction="isValidDateFrom" ControlToValidate="DtTime_Value1"
                                            ErrorMessage="">ERROR: 'From Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:CustomValidator><br />
                                        <asp:CustomValidator ID="DtTime_Value2Validator" runat="Server" CssClass="errormsg"
                                            Enabled="true" ClientValidationFunction="isValidDateTo" ControlToValidate="DtTime_Value2"
                                            ErrorMessage="">ERROR: 'To Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:CustomValidator>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="AddFilterButtonPanel" runat="Server" Visible="true">
                                <tr>
                                    <td class="tableHeader" width="20%">
                                        <asp:TextBox class="formfield" ID="FilterNameTxt" runat="Server" EnableViewState="true"
                                            Columns="25"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                        <asp:RequiredFieldValidator ID="val_FilterNameTxt" runat="Server" CssClass="errormsg"
                                            ControlToValidate="FilterNameTxt" ErrorMessage="Filter name is a required field."
                                            Display="Static"> <-- Required </asp:RequiredFieldValidator><br />
                                        <br />
                                        <asp:Button class="formbuttonsmall" ID="FirstAddFilterButton" OnClick="DisplayAddFilterForm"
                                            runat="Server" Visible="true" Text="Create new Filter"></asp:Button>&nbsp;&nbsp;<br />
                                    </td>
                                </tr>
                            </asp:Panel>
                        </tbody>
                </td>
            </tr>
            <asp:Panel ID="FiltersPanel" runat="Server" Visible="true">
                <tr class="tableHeader" align="left">
                    <td class="label">
                        <br />
                        List of Filters for <i>
                            <asp:Label ID="GroupNameDisplay" runat="Server" Visible="true" CssClass="label" Text=""></asp:Label></i>
                    </td>
                </tr>
                <tr class="tableHeader" align="left">
                    <td>
                        <asp:DataGrid ID="FiltersGrid" runat="Server" HorizontalAlign="Center" AutoGenerateColumns="False"
                            Width="100%" CssClass="grid">
                            <ItemStyle></ItemStyle>
                            <HeaderStyle CssClass="gridheader"></HeaderStyle>
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <Columns>
                                <asp:BoundColumn DataField="FilterName" HeaderText="Filter Name"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CreateDate" HeaderText="Date Created" ItemStyle-HorizontalAlign="center"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-Width="20%"></asp:BoundColumn>
                                <asp:HyperLinkColumn ItemStyle-Width="10%" HeaderText="Add / Edit" HeaderStyle-HorizontalAlign="center"
                                    Text="<img src=/ecn.images/images/icon-add-edit.gif alt='Add / Edit Filter attributes'"
                                    DataNavigateUrlField="FilterID" DataNavigateUrlFormatString="filters.aspx?FilterID={0}&action=editFilter"
                                    ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                                <asp:HyperLinkColumn HeaderText="Delete" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="5%"
                                    Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete Filter'" DataNavigateUrlField="FilterID"
                                    DataNavigateUrlFormatString="javascript:deleteFilter({0});" ItemStyle-HorizontalAlign="center">
                                </asp:HyperLinkColumn>
                            </Columns>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="FiltersPager" runat="Server" ControlToPage="FiltersGrid" PageSize="10"
                            Width="100%">
                            <PagerStyle CssClass="gridpager"></PagerStyle>
                        </AU:PagerBuilder>
                    </td>
                </tr>
            </asp:Panel>
        </tbody>
    </table>
</asp:Content>

<%@ Page Language="c#" Inherits="ecn.communicator.contentmanager.contentfilters"
    CodeBehind="contentfilters.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="ecn" TagName="FolderSys" Src="../../includes/folderSystem.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteFilter(theID) {
            if (confirm('Are you Sure?\nSelected smartContent Rule & corresponding smartContent RuleDetails will be permanently deleted.')) {
                window.location = "contentfilters.aspx?FilterID=" + theID + "&action=deleteFilter";
            }
        }

        function deleteFilterDetail(theID) {
            if (confirm('Are you Sure?\nSelected smartContent RuleDetail will be permanently deleted.')) {
                window.location = "contentfilters.aspx?FilterDetailID=" + theID + "&action=deleteFilterDetail";
            }
        }

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
    <asp:Panel ID="AddFilterButtonPanel" runat="Server" Visible="true">
        <table id="layoutWrapper" cellspacing="0" cellpadding="2" width="100%" border='0'>
            <tr>
                <td class="label">
                    <table border='0'>
                        <tr>
                            <td align="left">
                                <b>Rule Name:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="FilterNameTxt" runat="Server" CssClass="formfield" Columns="50"
                                    EnableViewState="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="val_FilterNameTxt" runat="Server" CssClass="errormsg"
                                    ErrorMessage="smartContent Rule name is a required field." ControlToValidate="FilterNameTxt"
                                    Display="Static"> &laquo; Required </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <b>smartContent to deliver:</b><br />
                                <font class="bodyText" color="#ff0000">[Select Folder from FolderTree to choose Content]</font>
                            </td>
                            <td>
                                <div style="overflow: auto; width: 300px; height: 250px; border: solid 1px #CCCCCC;">
                                    <ecn:FolderSys ID="FolderControl" runat="Server" HeightPercentage="100" FolderType="CNT">
                                    </ecn:FolderSys>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <b>Choose Content:</b>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ContentList" runat="Server" CssClass="formfield" DataTextField="ContentTitle"
                                    DataValueField="ContentID">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="Server" CssClass="errormsg"
                                    ErrorMessage="Content is a required field." ControlToValidate="ContentList" Display="Static"
                                    InitialValue=""> &laquo; Required </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                               <b> Optional Defined Data Group:</b>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="GroupList" runat="Server" CssClass="formfield">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" align="center" colspan="2">
                                <br />
                                <asp:Button class="formbuttonsmall" ID="FirstAddFilterButton" runat="Server" Visible="true"
                                    Text="Create new smartContent Rule"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableHeader" align="center" colspan="2">
                                <hr size="1" color="#999999" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="FiltersPanel" runat="Server" Visible="true">
        <table id="Table1" cellspacing="0" cellpadding="2" width="100%" border='0'>
            <tr align="left">
                <td class="label">
                    <b>List of smartContent Rules to deliver Contents:</b><br />
                    <br />
                </td>
            </tr>
            <tr align="left">
                <td>
                    <asp:DataGrid ID="FiltersGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                        HorizontalAlign="Center" CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FilterName" HeaderText="Rule Name"></asp:BoundColumn>
                            <asp:TemplateColumn ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <div align="center" class="bodyText">
                                        <itemstyle horizontalalign="center" font="System">will deliver the smartContent &raquo;&raquo;</itemstyle>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ContentTitle" HeaderText="Content"></asp:BoundColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="10%" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Add / Edit / View smartContent Rule Attributes'"
                                DataNavigateUrlField="FilterID" DataNavigateUrlFormatString="contentfilters.aspx?FilterID={0}&action=editFilter"
                                ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                            <asp:HyperLinkColumn ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-delete1.gif alt='Delete smartContent Rule'"
                                DataNavigateUrlField="FilterID" DataNavigateUrlFormatString="javascript:deleteFilter({0});"
                                ItemStyle-HorizontalAlign="center"></asp:HyperLinkColumn>
                        </Columns>
                    </asp:DataGrid>
                    <AU:PagerBuilder ID="FiltersPager" runat="Server" ControlToPage="GroupsGrid" PageSize="10"
                        Width="100%">
                        <PagerStyle CssClass="gridpager"></PagerStyle>
                    </AU:PagerBuilder>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="RuleNameDisplay" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="5" width="100%" border='0'>
            <tr>
                <td class="label" align="left" width="20%" valign="top">
                    <b>Rule Name:</b>
                </td>
                <td class="label" align="left" width="80%" valign="top">
                    <asp:Label ID="FilterNameTxtLabel" runat="Server" Visible="true" CssClass="errormsg"></asp:Label>&nbsp;
                    <asp:TextBox ID="FilterNameTxtValue" runat="Server" Visible="false" Enable="false"></asp:TextBox>&nbsp;
                    <asp:TextBox ID="FilterIDValue" runat="Server" Visible="false" Enable="false" Text="0"></asp:TextBox>&nbsp;
                    <asp:Button class="formbuttonsmall" ID="FilterListButton" OnClick="FiltersList" runat="Server"
                        Visible="true" Text="Return to smartContent Rules List"></asp:Button>
                </td>
            </tr>
            <tr>
                <td class="label" align="left" valign="top">
                   <b> smartContent to deliver:</b>
                </td>
                <td class="label" align="left" valign="top">
                    <asp:Label ID="ContentPageToDisplay" runat="Server" CssClass="errormsg"></asp:Label>&nbsp;
                    <asp:Label ID="ContentPagePreviewLabel" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="label" align="left" valign="top">
                    <b>Optional UDF Group:</b>
                </td>
                <td class="label" align="left" valign="top">
                    <asp:Label class="label" ID="UserDefinedFolder" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr  align="left">
                <td class="tableHeader" colspan='2'>
                    <b>smartContent Rule Attributes: </b>
                    <br />
                    <br />
                    <asp:DataGrid ID="AddFiltersGridArray" runat="Server" HorizontalAlign="Center" AutoGenerateColumns="False"
                        Width="100%" CssClass="grid">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                        <Columns>
                            <asp:BoundColumn DataField="FieldName" HeaderText="Field Name"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Comparator" HeaderText="Comparator" HeaderStyle-Width="10%"
                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompareValue" HeaderText="Compare Value"></asp:BoundColumn>
                            <asp:BoundColumn DataField="CompareType" HeaderText="Compare<br />Type" HeaderStyle-Width="5%"
                                HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="" ItemStyle-Width="3%">
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
            <tr>
                <td class="tableHeader" align="left" height="10" width="100%" colspan='2'>
                    <br />
                    <b>Add Rule: </b>
                    <br />
                    <br />
                    <table cellspacing="1" cellpadding="3" width="100%" border='0'>
                        <tbody>
                            <asp:Panel ID="FilterAttribsPanel" runat="Server" Visible="false">
                                <tr align="center">
                                    <td class="tableHeader1" bgcolor="#ffe09f">
                                        Compare Field
                                    </td>
                                    <td class="tableHeader1" bgcolor="#ffe09f">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td class="tableHeader1" bgcolor="#ffe09f">
                                        Comparator
                                    </td>
                                    <td class="tableHeader1" bgcolor="#ffe09f">
                                        Compare Value
                                    </td>
                                    <td class="tableHeader1" width="90" bgcolor="#ffe09f">
                                        Join Filters
                                    </td>
                                    <td class="tableHeader1" bgcolor="#ffe09f">
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
										                <font color="FF0000"><b>convert Compare Field to:</b></font></asp:Label><br />
                                        <asp:DropDownList ID="ConvertToDataType" runat="Server" CssClass="formfield" Width="145"
                                            AutoPostBack="true" OnSelectedIndexChanged="ConvertToDataType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value=""></asp:ListItem>
                                            <asp:ListItem Value="VARCHAR(500)">String</asp:ListItem>
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
                                            <asp:TextBox class="formfield" ID="CompValue" runat="Server" Columns="25" EnableViewState="true"></asp:TextBox>
                                        </asp:Panel>
                                        <asp:Panel ID="DtTime_CompareValuePanel" runat="Server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td align='right'>
                                                        <font class="bodyText" color="#ff0000">From:</font>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox class="formfield" ID="DtTime_Value1" runat="Server" Columns="15" EnableViewState="true"
                                                            MaxLength="10"></asp:TextBox>&nbsp;<img onclick="window.open('../../includes/calendarPopUp_From.htm', 'ToDate', 'left=10,top=10,height=145,width=255,resizable=no,scrollbar=no,status=no');"
                                                                alt="Click to Set START Date" src="/ecn.images/images/icon-calendar.gif" border='0'>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align='right'>
                                                        <font class="bodyText" color="#ff0000">To: </font>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox class="formfield" ID="DtTime_Value2" runat="Server" Columns="15" EnableViewState="true"
                                                            MaxLength="10"></asp:TextBox>&nbsp;<img onclick="window.open('../../includes/calendarPopUp_To.htm', 'ToDate', 'left=10,top=10,height=145,width=235,resizable=no,scrollbar=no,status=no');"
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
                                            Visible="true" Text="Add Rule"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="6">
                                        <asp:Label ID="ErrorLabel" runat="Server" Visible="false" CssClass="errormsg"></asp:Label><br />
                                        <asp:CustomValidator ID="CompValueNumberValidator" runat="Server" CssClass="errormsg"
                                            ErrorMessage="" ControlToValidate="CompValue" ClientValidationFunction="IsNumber"
                                            Enabled="false">ERROR: Compare Value that you have entered cannot be converted to Number. Only Numbers are Allowed. Please Correct !</asp:CustomValidator><br />
                                        <asp:CustomValidator ID="DtTime_Value1Validator" runat="Server" CssClass="errormsg"
                                            ErrorMessage="" ControlToValidate="DtTime_Value1" ClientValidationFunction="isValidDateFrom"
                                            Enabled="true">ERROR: 'From Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:CustomValidator><br />
                                        <asp:CustomValidator ID="DtTime_Value2Validator" runat="Server" CssClass="errormsg"
                                            ErrorMessage="" ControlToValidate="DtTime_Value2" ClientValidationFunction="isValidDateTo"
                                            Enabled="true">ERROR: 'To Date' cannot be converted to right Date Format. Enter Dates in MM/DD/YYYY format only. Please Correct !</asp:CustomValidator>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

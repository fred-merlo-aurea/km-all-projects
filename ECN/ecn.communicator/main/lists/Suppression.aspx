<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Suppression.aspx.cs" Inherits="ecn.communicator.main.lists.Suppression"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecn" TagName="uploader" Src="../../includes/uploader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="mbdb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DefaultButtons" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .TabHeader {
            text-align: left;
        }

        .errorClass {
            border: 1px solid red;
        }
    </style>
    <script type="text/javascript">
        function TabChanged(sender, args) {
            __doPostBack('TabContainer', '');
            sender.get_clientStateField().value =
                sender.saveClientState();
        }

        function IsSearchStringValid(source, arguments) {
            var ValidChars = "0123456789abcdefghijklmnopqrstuvwxyz-_.@'";
            var sText = getobj("ctl00_ContentPlaceHolder1_TabContainer_TabMasterSuppression_EmailFilter").value;
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i).toLowerCase();
                if (ValidChars.indexOf(Char) == -1) {
                    alert('Search text has INVALID special characters.\nValid special characters allowed are \' \' - _ . @ \' ');
                    arguments.IsValid = false;
                    return;
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="5" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="http://images.ecn5.com/images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="update1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <ajaxToolkit:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0"
                OnClientActiveTabChanged="TabChanged" Style="text-align: left" AutoPostBack="false"
                OnActiveTabChanged="TabContainer_ActiveTabChanged">

                <ajaxToolkit:TabPanel ID="TabMasterSuppression" runat="server" HeaderText="Master Suppression Group">

                    <ContentTemplate>
                        <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                            <tr>
                                <asp:Panel ID="pnlMasterSuppressionMsg" runat="server" Visible="False">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblMasterSuppressionMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <td class="tableHeader" align='right'>
                                    <asp:Panel ID="FilterPanel" runat="server">


                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="text-align: right;">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="text-align: right; width: 50%;">Filter by&nbsp;
                                            <asp:DropDownList class="formfield" ID="SubscribeTypeFilter" runat="server">
                                                <asp:ListItem Selected="True" Value="*">All Types</asp:ListItem>
                                                <asp:ListItem Value="U">UnSubscribes</asp:ListItem>
                                                <asp:ListItem Value="B">Bounce</asp:ListItem>
                                                <asp:ListItem Value="A">Abuse Complaint</asp:ListItem>
                                                <asp:ListItem Value="M">Manual Upload</asp:ListItem>
                                                <asp:ListItem Value="F">Feedback Loop(or Spam Complaint)</asp:ListItem>
                                                <asp:ListItem Value="E">Email Address Change</asp:ListItem>
                                                <asp:ListItem Value="?">Unknown User</asp:ListItem>
                                            </asp:DropDownList>
                                                            </td>
                                                            <td style="width: width:50%;">&nbsp;&nbsp;
                                            <asp:Label ID="Label1" runat="server" Text="[or]"></asp:Label>&nbsp;&nbsp;email address&nbsp;
                                            <asp:DropDownList ID="SearchEmailLike" runat="server" CssClass="formfield">
                                                <asp:ListItem Selected="True" Value="like"> contains any </asp:ListItem>
                                                <asp:ListItem Value="equals"> equals </asp:ListItem>
                                                <asp:ListItem Value="ends"> ends with </asp:ListItem>
                                                <asp:ListItem Value="starts"> starts with </asp:ListItem>
                                            </asp:DropDownList>
                                                                <asp:TextBox class="formtextfield" ID="EmailFilter" runat="server" Columns="16" />
                                                                <asp:CustomValidator ID="VAL_IsSearchStringValid" runat="server" CssClass="errormsg" ControlToValidate="EmailFilter" ClientValidationFunction="IsSearchStringValid" />&nbsp;
                                                                <asp:Button class="formbuttonsmall" ID="EmailFilterButton" runat="server" Text="Get Results" OnClick="EmailFilterButton_Click" />

                                                                <asp:RegularExpressionValidator ID="regFrom" ControlToValidate="DateFromFilter" ValidationExpression="(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))" ErrorMessage="From Date Invalid.  Correct format is MM/DD/YYYY." runat="server" ValidateRequestMode="Disabled" Display="Dynamic" />
                                                                <asp:RegularExpressionValidator ID="regTo" ControlToValidate="DateToFilter" ValidationExpression="(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))" ErrorMessage="To Date Invalid.  Correct format is MM/DD/YYYY." runat="server" ValidateRequestMode="Disabled" Display="Dynamic" />

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:Label ID="lblFrom" runat="server" Text="From:" />&nbsp;&nbsp;
                                            <asp:TextBox class="formtextfield" ID="DateFromFilter" runat="server" Columns="16" ToolTip="Click to open calendar." />
                                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="DateFromFilter" ID="calFrom" Enabled="True" />
                                                                &nbsp;&nbsp;
                                            <asp:Label ID="lblTo" runat="server" Text="To:" />&nbsp;&nbsp;
                                            <asp:TextBox class="formtextfield" ID="DateToFilter" runat="server" Columns="16" ToolTip="Click to open calendar." />
                                                                <ajaxToolkit:CalendarExtender runat="server" TargetControlID="DateToFilter" ID="calTo" Enabled="True" />
                                                                &nbsp;&nbsp;
                                                            </td>
                                                            <td style="text-align: left;">
                                                                <asp:CheckBox ID="chkRecentActivity" runat="server" Text="With Activity" />
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>

                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <mbdb:DefaultButtons runat="server" ID="SearchDefaultButton">
                                        <mbdb:DefaultButtonSetting Parent="SubscribeTypeFilter" Button="EmailFilterButton" />
                                        <mbdb:DefaultButtonSetting Parent="EmailFilter" Button="EmailFilterButton" />
                                    </mbdb:DefaultButtons>

                                    <br />
                                    <asp:UpdatePanel ID="DownloadPanel" UpdateMode="Conditional" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="DownloadFilteredEmailsButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <table style="width: 100%;">
                                                <tr>

                                                    <td style="width: 100%; text-align: right;">Export this view to&nbsp;
                                                    
                                                        <asp:DropDownList class="formfield" ID="FilteredDownloadType" runat="server">
                                                            <asp:ListItem Value=".xml">XML [.xml]</asp:ListItem>
                                                            <asp:ListItem Selected="True" Value=".xls">EXCEL [.xls]</asp:ListItem>
                                                            <asp:ListItem Value=".csv">CSV [.csv]</asp:ListItem>
                                                            <asp:ListItem Value=".txt">TXT [.txt]</asp:ListItem>
                                                        </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                    
                                                    
                                                        <asp:Button ID="DownloadFilteredEmailsButton" Text="Export" runat="server" OnClick="DownloadFilteredEmailsButton_Click"
                                                            class="formbuttonsmall" />
                                                        <asp:Label ID="xsdDownloadLbl" runat="server"></asp:Label>

                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <hr size="1" color="#999999">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ecnCustom:ecnGridView ID="gvSuppressionGroup" runat="server" AutoGenerateColumns="False"
                                        Width="100%" CssClass="grid" datakeyfield="EmailID" OnRowDataBound="gvSuppressionGroup_RowDataBound"
                                        OnRowDeleting="gvSuppressionGroup_RowDeleting" OnRowCommand="gvSuppressionGroup_RowCommand"
                                        EmptyTableRowText="No Records Found." ShowEmptyTable="True">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="EmailAddress" HeaderText="Email">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="FormatTypeCode" HeaderText="Format">
                                                <ItemStyle Width="5%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubscribeTypeCode" HeaderText="Subscribe">
                                                <ItemStyle Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CreatedDate" HeaderText="Date Added">
                                                <ItemStyle Width="18%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UpdatedDate" HeaderText="Date Modified">
                                                <ItemStyle Width="18%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="UDF">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="Server" Text="<img src=/ecn.images/images/icon-groupFields.gif alt='Add / Edit / View UDF Information' border='0'>"
                                                        NavigateUrl='<%# "emaildataeditor.aspx?EmailID=" +
                                                                DataBinder.Eval(Container, "DataItem.EmailID") + 
                                                                "&GroupID=" + DataBinder.Eval(Container, "DataItem.GroupID") %>'
                                                        ID="Hyperlink2"
                                                        NAME="Hyperlink1">
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EDIT">
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="Server" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit / View Email Profile Information' border='0'>"
                                                        NavigateUrl='<%# "emaileditor.aspx?EmailID=" +
                                                                DataBinder.Eval(Container, "DataItem.EmailID") + 
                                                                "&GroupID=" + DataBinder.Eval(Container, "DataItem.GroupID") %>'
                                                        ID="Hyperlink1"
                                                        NAME="Hyperlink1">
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="4%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                                        CausesValidation="false" ID="DeleteEmailBtn" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EmailID") %>'
                                                        OnClientClick="return confirm('Are you sure you want to delete?')"></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridaltrow" />
                                    </ecnCustom:ecnGridView>
                                    <AU:PagerBuilder ID="EmailsPager" runat="server" Width="100%" PageSize="20" OnIndexChanged="EmailsPager_IndexChanged" BorderWidth="0px" CausesValidation="False" CellPadding="2" CellSpacing="1" ControlToPage="" CurrentIndex="0" CurrentPage="1" FirstText="&lt;b&gt;&amp;lt;&amp;lt;&lt;/b&gt;" HorizontalAlign="NotSet" InfoPanelHorizontalAlign="NotSet" InfoPanelVerticalAlign="NotSet" InfoTemplate="Total Records: &lt;b&gt;$RECORDCOUNT$&lt;/b&gt; - Page: &lt;b&gt;$CURRENTPAGE$&lt;/b&gt; of &lt;b&gt;$PAGECOUNT$&lt;/b&gt;" LastText="&lt;b&gt;&amp;gt;&amp;gt;&lt;/b&gt;" NavigationTemplate="$PREVIOUSPAGE$&amp;nbsp;&amp;nbsp;&amp;nbsp;$FIRSTPAGE$ $PAGEGROUP$ $LASTPAGE$&amp;nbsp;&amp;nbsp;&amp;nbsp;$NEXTPAGE$" NavPanelHorizontalAlign="Right" NavPanelVerticalAlign="NotSet" NavSeparator="&amp;nbsp;&amp;nbsp;&amp;nbsp;" NextText="&lt;b&gt;Next&lt;/b&gt;" PageGroupLeftSeparator="[" PageGroupRightSeparator="]" PageSizes="" PanelCssClass="" PreviousText="&lt;b&gt;Prev.&lt;/b&gt;" RecordCount="0">
                                        <PagerStyle CssClass="gridpager"></PagerStyle>
                                    </AU:PagerBuilder>
                                    <input type="hidden" runat="server" id='chID_Hidden' />
                                    <input type="hidden" runat="server" id="custID_Hidden" />

                                    <input type="hidden" runat="server" id="grpID_Hidden" />

                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabDomainSuppression" runat="server" HeaderText="Domain Suppression">
                    <ContentTemplate>
                        <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                            <asp:Panel ID="pnlDomainSuppFor" runat="server" Visible="true">
                                <tr>
                                    <asp:Label runat="server" ID="lblDomainSuppressionID" Text="0" Visible="false"></asp:Label>
                                    <td class="label" valign="middle" align='right' height="16" style="width: 75px;">Type&nbsp;</span>
                                    </td>
                                    <td valign="top" height="16">
                                        <asp:RadioButtonList ID="rbType" runat="Server" CssClass="Label" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Channel">Channel</asp:ListItem>
                                            <asp:ListItem Value="Customer" Selected>Customer</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlDomainSuppAdd" runat="server">
                                <tr>
                                    <td class="label" valign="top" align='right' height="16" style="width: 75px;">Domain&nbsp;</span>
                                    </td>
                                    <td valign="top" height="16">
                                        <asp:TextBox ID="txtDomain" runat="Server" class="formfield" Width="200px" ValidationGroup="formValidation" />
                                        <asp:RequiredFieldValidator ID="rqDomain" runat="Server" CssClass="errormsg" Display="Static"
                                            ErrorMessage="Domain is a required field." ControlToValidate="txtDomain" ValidationGroup="formValidation"><-- Required</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regexpDomainValidation" runat="server" Display="Static"
                                            ErrorMessage="Invalid Domain." ControlToValidate="txtDomain" ValidationGroup="formValidation"
                                            ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}$" />
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="pnlDomainSuppressionMsg" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblDomainSuppressionMsg" runat="server" Font-Bold="True" ForeColor="red"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td class="label" valign="top" align="left" height="16">
                                    <asp:Button ID="btnAddDomainSuppression" runat="Server" Text="Add Domain Suppression"
                                        class="formbuttonsmall" Visible="true" OnClick="btnAddDomainSuppression_click"
                                        ValidationGroup="formValidation" />
                                    <asp:Button ID="btnCancelDomainSuppression" runat="Server" Text="Cancel" class="formbuttonsmall"
                                        Visible="true" OnClick="btnCancel_click" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel2" runat="Server" Visible="true" DefaultButton="btnsearchDomain">
                                        <hr size="1" color="#999999">
                                        <div align='right'>
                                            <asp:TextBox ID="txtsearchDomain" runat="Server" CssClass="formfield" Columns="15"
                                                ValidationGroup="searchDomain"></asp:TextBox>&nbsp;
                                            <asp:Button ID="btnsearchDomain" runat="Server" Text="Search Emails" class="formbuttonsmall"
                                                Visible="true" OnClick="btnsearchDomain_Click" ValidationGroup="searchDomain" />
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ecnCustom:ecnGridView ID="gvDomainSuppression" runat="Server" AutoGenerateColumns="False"
                                        Style="margin: 7px 0;" Width="100%" CssClass="grid" datakeyfield="DomainSuppressionID"
                                        OnPageIndexChanging="gvDomainSuppression_PageIndexChanging" DataKeyNames="DomainSuppressionID"
                                        OnRowCommand="gvDomainSuppression_RowCommand" PageSize="10" OnRowDeleting="gvDomainSuppression_RowDeleting"
                                        OnRowDataBound="gvDomainSuppression_RowDataBound" AllowPaging="true" OnRowEditing="gvDomainSuppression_RowEditing">
                                        <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                        <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                        <Columns>
                                            <asp:BoundField DataField="Domain" HeaderText="Domain" ItemStyle-Width="80%"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BaseChannelID") == null ?"Customer":"Channel"%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="center" />
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit suppression domain' border='0'&gt;"
                                                        CausesValidation="false" ID="lnkEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.DomainSuppressionID") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete suppression domain' border='0'&gt;"
                                                        CausesValidation="false" ID="lnkDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.DomainSuppressionID") %>'
                                                        OnClientClick="return confirm('Are you sure you want to delete this domain?')"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridaltrow" />
                                        <PagerTemplate>
                                            <table cellpadding="0" border="0">
                                                <tr>
                                                    <td align="left" class="label" width="31%">Total Records:
                                                        <asp:Label ID="lblTotalRecordsDomain" runat="server" Text="" />
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td align="right" class="label" width="100%">Page
                                                        <asp:TextBox ID="txtGoToPageDomain" runat="server" AutoPostBack="true" OnTextChanged="GoToPageDomain_TextChanged"
                                                            class="formtextfield" Width="30px" />
                                                        of
                                                        <asp:Label ID="lblTotalNumberOfPagesDomain" runat="server" CssClass="label" />
                                                        &nbsp;
                                                        <asp:Button ID="btnPreviousDomain" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                            CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                        <asp:Button ID="btnNextDomain" runat="server" CommandName="Page" ToolTip="Next Page"
                                                            CommandArgument="Next" class="formbuttonsmall" Text="Next >>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </PagerTemplate>
                                    </ecnCustom:ecnGridView>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabChannelSuppression" runat="server" HeaderText="Channel Master Suppression"
                    Visible="true" ScrollBars="Auto">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upChannelSuppression" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnexportEmails" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td style="font-family: Arial; font-size: 12px; float: left;" width="85%">Add Emails to be Master suppressed for the Channel.<br />
                                            Use comma "," (or) Press Enter for separating email addresses.
                                        </td>
                                        <td style="font-family: Arial; font-size: 12px; font-weight: bold" align="right"
                                            valign="bottom" width="50%"></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 11px;">
                                            <asp:TextBox ID="txtemailAddresses" runat="Server" EnableViewState="true" TextMode="multiline"
                                                Columns="55" Rows="12" class="formfield" Width="400px" />
                                        </td>
                                        <td valign="bottom" style="font-family: Arial; font-size: 12px; float: left; font-weight: bold">
                                            <asp:Panel ID="pnlimportResults" runat="server" Visible="false">
                                                RESULTS:
                                                <br />
                                                <asp:Label ID="lblMessage" runat="Server" Visible="false"></asp:Label>
                                                <asp:DataGrid ID="dgResults" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="400px" Visible="false">
                                                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                            HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="float: left; padding-left: 11px">
                                            <asp:Button ID="btnAddChannelSuppressionEmails" runat="Server" Text="Add Emails to Master Suppression"
                                                class="formbuttonsmall" Visible="true" OnClick="btnAddChannelSuppressionEmails_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panel1" runat="Server" Visible="true">
                                                <hr size="1" color="#999999">
                                                <div align='right'>
                                                    <asp:TextBox ID="txtEmails" runat="Server" CssClass="formfield" Columns="15"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnsearchEmails" runat="Server" Text="Search Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="btnsearchEmails_Click" />&nbsp;
                                                    <asp:Button ID="btnexportEmails" runat="Server" Text="Export Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="btnexportEmails_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%; height: 300px; overflow: scroll">
                                                <ecnCustom:ecnGridView ID="gvChannelMasterSuppression" runat="Server" AutoGenerateColumns="False"
                                                    Width="100%" CssClass="grid" datakeyfield="EmailAddress" EmptyTableRowText="No Records Found."
                                                    AllowPaging="false" OnRowDataBound="gvChannelMasterSuppression_RowDataBound"
                                                    OnRowCommand="gvChannelMasterSuppression_RowCommand" OnRowDeleting="gvChannelMasterSuppression_RowDeleting"
                                                    OnPageIndexChanging="gvChannelMasterSuppression_PageIndexChanging">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" HeaderStyle-Width="70%"
                                                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Date Added" HeaderStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="10%"
                                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                                                    CausesValidation="false" ID="deleteEmailBTN" CommandName="deleteEmail" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.CMSID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                </ecnCustom:ecnGridView>
                                            </div>
                                            <AU:PagerBuilder ID="ChannelMasterPager" runat="Server" Width="100%" PageSize="50"
                                                OnIndexChanged="ChannelMasterPager_IndexChanged">
                                                <PagerStyle CssClass="gridpager"></PagerStyle>
                                            </AU:PagerBuilder>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabNoThreshold" runat="server" HeaderText="No Threshold"
                    Visible="true">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upNOThreshold" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="exportEmailsBTN" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td style="font-family: Arial; font-size: 12px; float: left; padding-left: 11px"
                                            width="85%">Add Emails to be ignored by Threshold Suppression for the Channel.<br />
                                            Use comma "," (or) Press Enter for separating email addresses.
                                        </td>
                                        <td style="font-family: Arial; font-size: 12px; font-weight: bold" align="right"
                                            valign="bottom" width="50%"></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 11px;">
                                            <asp:TextBox ID="emailAddresses" runat="Server" EnableViewState="true" TextMode="multiline"
                                                Columns="55" Rows="12" class="formfield" Width="400px" />
                                        </td>
                                        <td valign="bottom" style="font-family: Arial; font-size: 12px; float: left; font-weight: bold">
                                            <asp:Panel ID="importResultsPNL" runat="server" Visible="false">
                                                RESULTS:
                                                <br />
                                                <asp:Label ID="MessageLabel" runat="Server" Visible="false"></asp:Label>
                                                <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="400px" Visible="false">
                                                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                            HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="float: left; padding-left: 11px">
                                            <asp:Button ID="btnAddNoThresholdEmails" runat="Server" Text="Add Emails to No Threshold"
                                                class="formbuttonsmall" Visible="true" OnClick="btnAddNoThresholdEmails_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlNoThresholdFilter" runat="Server" Visible="true">
                                                <hr size="1" color="#999999">
                                                <div align='right'>
                                                    <asp:TextBox ID="txtNoThresholdEmails" runat="Server" CssClass="formfield" Columns="15"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnsearchNoThreshold" runat="Server" Text="Search Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="btnsearchNoThreshold_Click" />&nbsp;
                                                    <asp:Button ID="exportEmailsBTN" runat="Server" Text="Export Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="exportEmailsBTN_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ecnCustom:ecnGridView ID="gvNoThreshold" runat="Server" AutoGenerateColumns="False"
                                                Style="margin: 7px 0;" Width="100%" CssClass="grid" datakeyfield="EmailAddress"
                                                OnPageIndexChanging="gvNoThreshold_PageIndexChanging" PageSize="10" AllowPaging="true"
                                                OnRowDataBound="gvNoThreshold_RowDataBound" OnRowCommand="gvNoThreshold_RowCommand"
                                                OnRowDeleting="gvNoThreshold_RowDeleting">
                                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                <Columns>
                                                    <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" HeaderStyle-Width="75%"
                                                        HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                    <asp:BoundField DataField="DateAdded" HeaderText="Date Added" HeaderStyle-Width="20%"
                                                        HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                                        HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                                                CausesValidation="false" ID="deleteEmailBTN" CommandName="deleteEmail" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EmailAddress") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <AlternatingRowStyle CssClass="gridaltrow" />
                                                <PagerTemplate>
                                                    <table cellpadding="0" border="0">
                                                        <tr>
                                                            <td align="left" class="label" width="31%">Total Records:
                                                                <asp:Label ID="lblTotalRecordsNoThreshold" runat="server" Text="" />
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="right" class="label" width="100%">Page
                                                                <asp:TextBox ID="txtGoToPageNoThreshold" runat="server" AutoPostBack="true" OnTextChanged="GoToPageNoThreshold_TextChanged"
                                                                    class="formtextfield" Width="30px" />
                                                                of
                                                                <asp:Label ID="lblTotalNumberOfPagesNoThreshold" runat="server" CssClass="label" />
                                                                &nbsp;
                                                                <asp:Button ID="btnPreviousNoThreshold" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                                    CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" />
                                                                <asp:Button ID="btnNextNoThreshold" runat="server" CommandName="Page" ToolTip="Next Page"
                                                                    CommandArgument="Next" class="formbuttonsmall" Text="Next >>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </PagerTemplate>
                                            </ecnCustom:ecnGridView>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="TabGlobalSuppression" runat="server" HeaderText="Global Suppression"
                    Visible="true" ScrollBars="Auto">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upGlobalSuppression" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnexportEmailsGlobal" />
                            </Triggers>
                            <ContentTemplate>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td style="font-family: Arial; font-size: 12px; float: left;" width="85%">Add Emails to be suppressed across all Channels.<br />
                                            Use comma "," (or) Press Enter for separating email addresses.
                                        </td>
                                        <td style="font-family: Arial; font-size: 12px; font-weight: bold" align="right"
                                            valign="bottom" width="50%"></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 11px;">
                                            <asp:TextBox ID="txtemailAddressesGlobal" runat="Server" EnableViewState="true" TextMode="multiline"
                                                Columns="55" Rows="12" class="formfield" Width="400px" />
                                        </td>
                                        <td valign="bottom" style="font-family: Arial; font-size: 12px; float: left; font-weight: bold">
                                            <asp:Panel ID="pnlimportResultsGlobal" runat="server" Visible="false">
                                                RESULTS:
                                                <br />
                                                <asp:Label ID="lblMessageGlobal" runat="Server" Visible="false"></asp:Label>
                                                <asp:DataGrid ID="dgResultsGlobal" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="400px" Visible="false">
                                                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <Columns>
                                                        <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                                            HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                                                    </Columns>
                                                </asp:DataGrid>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="float: left; padding-left: 11px">
                                            <asp:Button ID="btnAddGlobalSuppressionEmails" runat="Server" Text="Add Emails to Global Suppression"
                                                class="formbuttonsmall" Visible="true" OnClick="btnAddGlobalSuppressionEmails_Click" />
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlSearchGlobal" runat="Server" Visible="true">
                                                <hr size="1" color="#999999">
                                                <div align='right'>
                                                    <asp:TextBox ID="txtEmailsGlobal" runat="Server" CssClass="formfield" Columns="15"></asp:TextBox>&nbsp;
                                                    <asp:Button ID="btnsearchEmailsGlobal" runat="Server" Text="Search Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="btnsearchEmailsGlobal_Click" />&nbsp;
                                                    <asp:Button ID="btnexportEmailsGlobal" runat="Server" Text="Export Emails" class="formbuttonsmall"
                                                        Visible="true" OnClick="btnexportEmailsGlobal_Click" />
                                                </div>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%; height: 300px; overflow: scroll">
                                                <ecnCustom:ecnGridView ID="gvGlobalMasterSuppression" runat="Server" AutoGenerateColumns="False"
                                                    Width="100%" CssClass="grid" datakeyfield="EmailAddress" EmptyTableRowText="No Records Found."
                                                    AllowPaging="false" OnRowDataBound="gvGlobalMasterSuppression_RowDataBound"
                                                    OnRowCommand="gvGlobalMasterSuppression_RowCommand" OnRowDeleting="gvGlobalMasterSuppression_RowDeleting"
                                                    OnPageIndexChanging="gvGlobalMasterSuppression_PageIndexChanging">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" HeaderStyle-Width="70%"
                                                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Date Added" HeaderStyle-Width="20%"
                                                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="10%"
                                                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                                                    CausesValidation="false" ID="deleteEmailBTN" CommandName="deleteEmail" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.GSID") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                </ecnCustom:ecnGridView>
                                            </div>
                                            <AU:PagerBuilder ID="GlobalMasterPager" runat="Server" Width="100%" PageSize="50"
                                                OnIndexChanged="GlobalMasterPager_IndexChanged">
                                                <PagerStyle CssClass="gridpager"></PagerStyle>
                                            </AU:PagerBuilder>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

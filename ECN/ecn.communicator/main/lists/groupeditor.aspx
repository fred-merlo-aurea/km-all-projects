<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.groupeditor" CodeBehind="groupeditor.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="mbdb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DefaultButtons" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteEmail(theID) {
            if (confirm('Are you Sure?\n Selected Email will be permanently deleted.')) {
                window.location = "groupeditor.aspx?GroupID=<%=getGroupID().ToString()%>&EmailID=" + theID + "&action=delete";
            }
        }

        function IsSearchStringValid(source, arguments) {
            var ValidChars = "0123456789abcdefghijklmnopqrstuvwxyz-+_.@'";
            var sText = document.getElementById('<%=EmailFilter.ClientID%>').value;
            for (i = 0; i < sText.length; i++) {
                Char = sText.charAt(i).toLowerCase();
                if (ValidChars.indexOf(Char) == -1) {
                    alert('Search text has INVALID special characters.\nValid special characters allowed are \' \' - + _ . @ \' ');
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

        function openDownloadPage() {
            var channelID = getobj('<%=chID_Hidden.ClientID%>').value;
            var customerID = getobj('<%=custID_Hidden.ClientID%>').value;
            var groupID = getobj('<%=grpID_Hidden.ClientID%>').value;
            var fileType = getobj('<%=FilteredDownloadType.ClientID%>').value;
            var subType = getobj('<%=SubscribeTypeFilter.ClientID%>').value;
            var srchType = getobj('<%=SearchEmailLike.ClientID%>').value;
            var srchEm = getobj('<%=EmailFilter.ClientID%>').value;
            var ddlprofileFilter = document.getElementById('<%=ddlFilteredDownloadOnly.getDDLClientID%>');
            var profileFilter = ddlprofileFilter.options[ddlprofileFilter.selectedIndex].value;

            window.open('download.aspx?chID=' + channelID + '&custID=' + customerID + '&grpID=' + groupID + '&fileType=' + fileType + '&subType=' + subType + '&srchType=' + srchType + '&srchEm=' + srchEm + '&profFilter=' + profileFilter + '', '', 'width=500,height=200status=yes,toolbar=no,scrollbar=yes');
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
    <table id="layoutWrapper" cellspacing="0" cellpadding="3" width="100%" border='0'
        bgcolor="#ffffff">
        <tbody>
            <tr>
                <td class="tablecontent" valign="top" align="left" height="24" colspan='3'>
                    <b>Add / Update Group</b>
                </td>
            </tr>
            <tr>
                <!-- added width atribute to cell - anthony 9-18-06 1120 -->
                <td class="tableHeader" valign="top" align='right'>
                    <span class="label">Name&nbsp;</span>
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="GroupName" CssClass="formfield" EnableViewState="true" runat="Server"
                        Columns="40" MaxLength="50" class="formfield" Width="272"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="Server" ID="val_GroupName" ControlToValidate="GroupName"
                        CssClass="errormsg" Display="Static" ValidationGroup="GroupAddUpdate">&laquo;&laquo; Required </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    <span class="label">Description&nbsp;</span>
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="GroupDescription" runat="Server" Wrap="true" Columns="50" Rows="3"
                        TextMode="multiline" Enabled="true" class="formfield" Width="272" Height="60"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;<span class="label">Folder&nbsp;</span>
                </td>
                <td colspan="2" align="left">
                    <asp:DropDownList EnableViewState="true" CssClass="formfield" Style="border-right: #999999 1px solid;
                        border-top: #999999 1px solid; border-left: #999999 1px solid; border-bottom: #999999 1px solid"
                        ID="FolderID" runat="Server" DataValueField="FolderID" DataTextField="FolderName"
                        Width="272">
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:Panel ID="SeedListPanel" runat="Server" Visible="false">
                <tr>
                    <td class="tableHeader" align='right'>
                        &nbsp;<span class="label">Is this Seed List&nbsp;</span>
                    </td>
                    <td colspan="2" align="left">
                        <asp:RadioButtonList ID="rbSeedList" class="formLabel" runat="Server" RepeatDirection="horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                        </asp:RadioButtonList>
                        &nbsp;&nbsp;<font class="formLabel" style="color: Gray">(ECN will automatically send
                            a copy of the blasts to all the emails in the Seed list group. )</font>
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="CustDepartmentPanel" runat="Server" Visible="false">
                <tr>
                    <td class="tableHeader" align='right'>
                        &nbsp;<span class="label">User Group&nbsp;</span>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="DepartmentID" Style="border-right: #999999 1px solid; border-top: #999999 1px solid;
                            border-left: #999999 1px solid; border-bottom: #999999 1px solid" runat="Server"
                            Width="272" EnableViewState="true" CssClass="formfield" DataTextField="DepartmentName"
                            DataValueField="DepartmentID">
                        </asp:DropDownList>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td align='right' class="tableHeader">
                    <asp:Label ID="lblIsPublic" runat="Server" class="tableHeader" Visible="false" Width="64px"> Is Public</asp:Label>
                    &nbsp;
                </td>
                <td>
                    <!-- removed width atribute - anthony 9-18-06 1120-->
                    <asp:CheckBox ID="PublicFolder" runat="Server" Visible="false"></asp:CheckBox>
                </td>
                <!--left aligned cell and added padding-left - anthony 9-18-06 1120 -->
                <td class="tableHeader" align="left">
                    <asp:TextBox ID="GroupID" runat="Server" Visible="false" EnableViewState="true"></asp:TextBox>
                    <asp:Button class="formbutton" ID="SaveButton" OnClick="CreateGroup" runat="Server"
                        Visible="true" Text="Create" ValidationGroup="GroupAddUpdate"></asp:Button>
                    <asp:Button class="formbutton" ID="UpdateButton" OnClick="UpdateGroup" runat="Server"
                        Visible="false" Text="Update" ValidationGroup="GroupAddUpdate"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="MsgLabel" runat="server" Visible="false" class="errorMsg"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align='right' colspan='3' valign="middle" class="tableHeader">
                </td>
            </tr>
            <tr>
                <td colspan='3' class="tableHeader" align='right'>
                    <mbdb:DefaultButtons runat="Server" ID="SearchDefaultButton">
                        <mbdb:DefaultButtonSetting Parent="SubscribeTypeFilter" Button="EmailFilterButton" />
                        <mbdb:DefaultButtonSetting Parent="EmailFilter" Button="EmailFilterButton" />
                    </mbdb:DefaultButtons>
                    <asp:Panel ID="FilterPanel" runat="Server" Visible="true">
                        <hr size="1" color="#999999">
                        <div align='right'>
                            Filter by&nbsp;
                            <asp:DropDownList class="formfield" ID="SubscribeTypeFilter" runat="Server" EnableViewState="true"
                                Visible="true">
                            </asp:DropDownList>
                            &nbsp;&nbsp; [or]&nbsp;&nbsp;email address&nbsp;
                            <asp:DropDownList ID="SearchEmailLike" runat="Server" CssClass="formfield">
                                <asp:ListItem Selected="True" Value="like"> contains </asp:ListItem>
                                <asp:ListItem Value="equals"> equals </asp:ListItem>
                                <asp:ListItem Value="starts"> starts with </asp:ListItem>
                                <asp:ListItem Value="ends"> ends with </asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox class="formtextfield" ID="EmailFilter" runat="Server" Columns="16" EnableViewState="true"></asp:TextBox>
                            <asp:CustomValidator ID="VAL_IsSearchStringValid" runat="Server" CssClass="errormsg"
                                ErrorMessage="" ControlToValidate="EmailFilter" Enabled="true" ClientValidationFunction="IsSearchStringValid"></asp:CustomValidator>&nbsp;
                            <asp:Button class="formbuttonsmall" ID="EmailFilterButton" runat="Server" Visible="true"
                                Text="Get Results" OnClick="EmailFilterButton_Click"></asp:Button><br />
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="DownloadPanel" runat="Server" Visible="true">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:28%;">

                                </td>
                                <td>
                                    Export this view to&nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList class="formfield" ID="FilteredDownloadType" runat="Server" EnableViewState="true"
                            Visible="true">
                            <asp:ListItem Value=".xml">XML [.xml]</asp:ListItem>
                            <asp:ListItem Selected="true" Value=".xls">EXCEL [.xls]</asp:ListItem>
                            <asp:ListItem Value=".csv">CSV [.csv]</asp:ListItem>
                            <asp:ListItem Value=".txt">TXT [.txt]</asp:ListItem>
                        </asp:DropDownList>
                                </td>
                                <td>
                                     &nbsp;
                        Download Only&nbsp;
                                </td>
                                <td>
                                    <ecn:groupexportudfsettings ID="ddlFilteredDownloadOnly" EnableViewState="true" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExportGroup" runat="server" Text="Export" CssClass="formbuttonsmall" OnClick="btnExportGroup_Click" />
                                    <br />
                                    </td>
                                </tr>
                            <tr>
                                <td colspan="6" style="text-align:right;">
                        <asp:Label ID="xsdDownloadLbl" runat="Server" Visible="true"></asp:Label>
                        <hr size="1" color="#999999">
                                </td>
                            </tr>
                        </table>

                        
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align='right' colspan='3' valign="middle" class="tableHeader">
                    <asp:Panel ID="BodyPanel" runat="Server" Visible="true">
                        <AU:PagerBuilder ID="EmailsPager" runat="Server" Width="100%" PageSize="50" ControlToPage="EmailsGrid"
                            OnIndexChanged="EmailsPager_IndexChanged">
                            <PagerStyle CssClass="tableContent"></PagerStyle>
                        </AU:PagerBuilder>
                        <ecnCustom:ecnGridView ID="EmailsGrid" runat="Server" CssClass="grid" Width="100%" AutoGenerateColumns="False"
                            DataKeyField="EmailID" OnRowCommand="EmailsGrid_RowCommand">
                            <RowStyle Height="22" HorizontalAlign="center"></RowStyle>
                            <HeaderStyle CssClass="gridheader" HorizontalAlign="center"></HeaderStyle>
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <AlternatingRowStyle CssClass="gridaltrow" HorizontalAlign="center" />
                            <Columns>
                                <asp:BoundField DataField="EmailAddress" HeaderText="Email" HeaderStyle-HorizontalAlign="left"
                                    ItemStyle-HorizontalAlign="left"></asp:BoundField>
                                <asp:BoundField ItemStyle-Width="5%" DataField="FormatTypeCode" HeaderText="Format">
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="10%" DataField="SubscribeTypeCode" HeaderText="Subscribe">
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="18%" DataField="CreatedDate" HeaderText="Date Added">
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Width="18%" DataField="UpdatedDate" HeaderText="Date Modified">
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="4%" HeaderText="UDF" HeaderStyle-HorizontalAlign="center"
                                    ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="Server" Text="<img src=/ecn.images/images/icon-groupFields.gif alt='Add / Edit / View UDF Information' border='0'>"
                                            NavigateUrl='<%# "emaildataeditor.aspx?EmailID=" +
                                                                DataBinder.Eval(Container, "DataItem.EmailID") + 
                                                                "&GroupID=" + DataBinder.Eval(Container, "DataItem.GroupID") %>'
                                            ID="Hyperlink2" NAME="Hyperlink1">
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="4%" HeaderText="EDIT" HeaderStyle-HorizontalAlign="center"
                                    ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="Server" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit / View Email Profile Information' border='0'>"
                                            NavigateUrl='<%# "emaileditor.aspx?EmailID=" +
                                                                DataBinder.Eval(Container, "DataItem.EmailID") + 
                                                                "&GroupID=" + DataBinder.Eval(Container, "DataItem.GroupID") %>'
                                            ID="Hyperlink1" NAME="Hyperlink1">
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                                    HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                            CausesValidation="false" ID="DeleteEmailBtn" CommandName="DeleteEmail" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EmailID") %>'
                                            OnClientClick="return confirm('Are you sure that you want to delete this Email Profile?')"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </ecnCustom:ecnGridView>
                    </asp:Panel>
                    <input type="hidden" runat="Server" id='chID_Hidden' name='chID_Hidden'>
                    <input type="hidden" runat="Server" id="custID_Hidden" name='custID_Hidden'>
                    <input type="hidden" runat="Server" id="grpID_Hidden" name='grpID_Hidden'>
                </td>
            </tr>
        </tbody>
    </table>
    <br />
</asp:Content>

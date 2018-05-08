<%@ Page Language="c#" Inherits="ecn.communicator.main.lists.groupsubscribePrePopSF"
    CodeBehind="groupsubscribePrePopSF.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function deleteSmartForm(theID) {
            if (confirm('Are you Sure?\n Selected smartForm permanently deleted.')) {
                window.location = "groupsubscribePrePopSF.aspx?" + theID + "&action=DELETE";
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
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" align="center" colspan="2">
                    <!--Create New smartForms:&nbsp;&nbsp;-->
                    <asp:Button class="formbuttonsmall" ID="SmartFormButton" runat="Server" Text="Other Optin smartForms">
                    </asp:Button>
                    <br />
                    <asp:Label ID="msglabel" Visible="false" CssClass="errormsg" Style="width: 750px;
                        text-align: left;" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align='right' colspan="4" height="3">
                </td>
            </tr>
            <asp:Panel ID="PrePopSmartFormPanel" runat="Server">
                <tr>
                    <td align='right' width="100%">
                        <table border='0' cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="tablft" height="19" width="6">
                                </td>
                                <td class="tabbg" width="5">
                                </td>
                                <td class="tabbg">
                                    <asp:HyperLink ID="AddPrePopSFLink" Text="Add new Pre-Pop smartForm" NavigateUrl=""
                                        runat="Server" />
                                </td>
                                <td class="tabbg" width="5">
                                </td>
                                <td class="tabrt" width="6">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#eeeeee" colspan="2">
                        <cpanel:DataPanel ID="PrePopSFList_Panel" Style="z-index: 101" runat="Server" AllowTitleExpandCollapse="True"
                            TitleText="Pre-Pop smartForms - List" Collapsed="False" ExpandText="Click to display Pre-Pop smartForms List"
                            CollapseText="Click to hide  Pre-Pop smartForms List" CollapseImageUrl="collapse.gif"
                            ExpandImageUrl="expand.gif">
                            <asp:DataGrid ID="SmartFormGrid" runat="Server" CssClass="grid" HorizontalAlign="Center"
                                AutoGenerateColumns="False" Width="100%">
                                <ItemStyle Height="17"></ItemStyle>
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="SmartFormName" HeaderText="Pre-Pop smartForm Name"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <center>
                                                <img src='/ecn.images/images/icon-script-gear.gif' alt='Pre-Pop smartForm Scripts'
                                                    border='0' onclick="javascript:window.open('groupsubscribePrePopScript.aspx?<%#(string) DataBinder.Eval(Container.DataItem, "SmartFormID") %>', '','left=100,top=100,height=340,width=725,resizable=no,scrollbar=yes,status=no')"
                                                    style="cursor: hand;"></center>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:HyperLinkColumn ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit Pre-Pop smartForm' border='0'>"
                                        DataNavigateUrlField="SmartFormID" DataNavigateUrlFormatString="groupsubscribePrePopSF.aspx?{0}">
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:HyperLinkColumn>
                                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <a href='javascript:deleteSmartForm("<%#(string) DataBinder.Eval(Container.DataItem, "SmartFormID") %>");'>
                                                <center>
                                                    <img src='/ecn.images/images/icon-delete1.gif' alt='Delete Pre-Pop smartForm' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <AlternatingItemStyle CssClass="gridaltrow" />
                            </asp:DataGrid>
                            <AU:PagerBuilder ID="GridPager" runat="Server" ControlToPage="SmartFormGrid" PageSize="3"
                                Width="100%">
                                <PagerStyle CssClass="gridpager"></PagerStyle>
                            </AU:PagerBuilder>
                        </cpanel:DataPanel>
                    </td>
                </tr>
                <tr>
                    <td align='right' colspan='3' height="4">
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" valign="top" width="100%" bgcolor="#eeeeee">
                        <table border='0' width="100%">
                            <tr>
                                <td width="100%">
                                    <cpanel:DataPanel ID="PrePopSFDetails_Panel" Style="z-index: 101" runat="Server"
                                        AllowTitleExpandCollapse="True" TitleText="Pre-Pop smartForms - Add / Edit" ExpandText="Click to display Pre-Pop smartForm Details"
                                        CollapseText="Click to hide  Pre-Pop smartForm Details" CollapseImageUrl="collapse.gif"
                                        ExpandImageUrl="expand.gif">
                                        <table style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid; border-left: #c0c0c0 1px solid;
                                            border-bottom: #c0c0c0 1px solid; table-layout: fixed;" cellspacing="1" cellpadding="1" width="100%"
                                            border='0'>
                                            <tr>
                                                <td class="tableHeader1" align='right' style="width:14%">
                                                    <b>smartForm Name:&nbsp;</b>
                                                </td>
                                                <td colspan='3'>
                                                    <asp:TextBox class="formtextfieldsmall" ID="smartFormName" TabIndex="1" runat="Server"
                                                        Columns="68"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader1" valign="bottom" width="50%" colspan="2">
                                                    Automated Internal Email:
                                                </td>
                                                <td class="tableHeader1" valign="bottom" width="50%" colspan="2">
                                                    Automated Response to Submitted Email:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">
                                                    Internal Email Address(es):
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminEmail" TabIndex="2" runat="Server"
                                                        Columns="45"></asp:TextBox>
                                                </td>
                                                <td class="tableHeaderSmall" align='right' width="14%">
                                                    Email From:&nbsp;
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_FromEmail" TabIndex="5" runat="Server"
                                                        Columns="45"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">
                                                    Email Subject:&nbsp;
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminMsgSubject" TabIndex="3"
                                                        runat="Server" Columns="45"></asp:TextBox>
                                                </td>
                                                <td class="tableHeaderSmall" align='right' width="14%">
                                                    Email Subject:&nbsp;
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserMsgSubject" TabIndex="6"
                                                        runat="Server" Columns="45"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">
                                                    Email Body:&nbsp;
                                                </td>
                                                <td width="200">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminMsgBody" TabIndex="4" runat="Server"
                                                        Columns="68" TextMode="multiline" Rows="3" style="max-width: 95%; min-width: 95%; min-height: 50px;"></asp:TextBox>
                                                </td>
                                                <td class="tableHeaderSmall" align='right'>
                                                    Email Body:&nbsp;
                                                </td>
                                                <td width="200">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserMsgBody" TabIndex="7" runat="Server"
                                                        Columns="68" TextMode="multiline" Rows="3" style="max-width: 95%; min-width: 95%; min-height: 50px;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="3">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" align="left" colspan="2">
                                                    Response Landing Page:<br />
                                                    <div class="tableContentSmall" align="left">
                                                        <u>NOTE</u>:A webpage URL starting with http:// [OR] HTML source code</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserScreen" TabIndex="8" runat="Server"
                                                        Columns="98" TextMode="multiline" Rows="3"></asp:TextBox>
                                                </td>
                                                <td valign="bottom" align='right' colspan="2">
                                                    <asp:Button class="formbuttonsmall" ID="SO_Save" runat="Server" Enabled="false" Text="Save Pre-Pop smartForm"
                                                        Visible="false" OnClick="SO_Save_Click"></asp:Button>&nbsp;&nbsp;
                                                    <asp:Button class="formbuttonsmall" ID="SO_New" runat="Server" Enabled="true" Text="Create New Pre-Pop smartForm"
                                                        OnClick="SO_New_Click"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        &nbsp;
                                        <asp:Label ID="SFFieldsLBL" runat="Server" Visible="false" CssClass="tableHeader">Add / Edit Pre-Pop smartForm Fields</asp:Label>
                                        <table width="100%" border='0'>
                                            <tr>
                                                <td class="tableHeader" valign="top" width="100%" bgcolor="#eeeeee">
                                                    <asp:DataList ID="PrePopProfileFieldsList" runat="Server" CssClass="grid" Width="100%"
                                                        DataKeyField="PrePopFieldID" CellPadding="2" BackColor="#FFFFFF">
                                                        <HeaderTemplate>
                                                            <tr class="gridheader">
                                                                <td width="10%">
                                                                    Field Name
                                                                </td>
                                                                <td width="20%">
                                                                    Display Name
                                                                </td>
                                                                <td width="10%" align="center">
                                                                    Data Type
                                                                </td>
                                                                <td width="10%" align="center">
                                                                    Field Type
                                                                </td>
                                                                <td width="25%" align="center">
                                                                    Values
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    Required
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    Populate
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    Order
                                                                </td>
                                                                <td width="10%" align="center">
                                                                </td>
                                                            </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class="gridaltrow">
                                                                <td width="10%" title='<%# DataBinder.Eval(Container.DataItem, "PrePopFieldID")%>'>
                                                                    <%# DataBinder.Eval(Container.DataItem, "ProfileFieldName") %>
                                                                </td>
                                                                <td width="15%">
                                                                    <%# DataBinder.Eval(Container.DataItem, "DisplayName") %>
                                                                </td>
                                                                <td width="10%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "DataType")  %>
                                                                </td>
                                                                <td width="10%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "ControlType") %></asp:Label>
                                                                </td>
                                                                <td width="20%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "DataValues") %></asp:Label>
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "Required") %></asp:Label>
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "PrePopulate") %></asp:Label>
                                                                </td>
                                                                <td width="5%" align="center">
                                                                    <%# DataBinder.Eval(Container.DataItem, "SortOrder") %></asp:Label>
                                                                </td>
                                                                <td width="10%" bgcolor="#eeeeee" align="left" nowrap>
                                                                    <asp:LinkButton ID="PrePopFieldEdit" runat="Server" Text="&lt;img src=/ecn.images/images/icon-edits1.gif alt='Edit Smart Form Field' border='0'&gt;"
                                                                        CommandName="Edit" CausesValidation="false"></asp:LinkButton>&nbsp;
                                                                    <asp:LinkButton ID="PrePopFieldDelete" runat="Server" Text=<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? "":"&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Smart Form Field' border='0'&gt;" %>
                                                                        CommandName="Delete" CausesValidation="false"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <tr class="gridaltrow">
                                                                <td width="10%" valign="bottom">
                                                                    <asp:DropDownList ID="Edit_ProfileFieldNameDR" runat="Server" class="formfield" DataValueField="Name"
                                                                        DataTextField="Name" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="15%" valign="bottom">
                                                                    <asp:TextBox ID="Edit_DisplayNameTXT" runat="Server" class="formfield"></asp:TextBox>
                                                                </td>
                                                                <td width="10%" align="center" valign="bottom">
                                                                    <asp:DropDownList ID="Edit_DataTypeDR" runat="Server" class="formfield" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'>
                                                                        <asp:ListItem Value=""> -- NONE -- </asp:ListItem>
                                                                        <asp:ListItem Value="TEXT"> Alpha & Numbers </asp:ListItem>
                                                                        <asp:ListItem Value="NUMBER"> Numbers </asp:ListItem>
                                                                        <asp:ListItem Value="DATE"> Date </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="10%" align="center" valign="bottom">
                                                                    <asp:DropDownList ID="Edit_ControlTypeDR" runat="Server" class="formfield" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'>
                                                                        <asp:ListItem Selected="True" Value="TEXT"> Text Box </asp:ListItem>
                                                                        <asp:ListItem Value="CHECKBOX"> Check Box </asp:ListItem>
                                                                        <asp:ListItem Value="DROPDOWN"> Dropdown List </asp:ListItem>
                                                                        <asp:ListItem Value="RADIOBUTTON"> Radio Buttons </asp:ListItem>
                                                                        <asp:ListItem Value="HIDDEN"> Hidden Field </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="20%" align="center" valign="bottom">
                                                                    <div style="font-size: 10px; font-weight: normal; padding-left: 7px" align="left">
                                                                        <b><u>NOTE:</u></b> Enter values separated by pipe symbol " | "</div>
                                                                    <asp:TextBox ID="Edit_DataValuesTXT" runat="Server" class="formfield" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'
                                                                        Width="170px"></asp:TextBox>
                                                                </td>
                                                                <td width="5%" align="center" valign="bottom">
                                                                    <asp:CheckBox ID="Edit_RequiredCHKBX" runat="Server" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'>
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td width="5%" align="center" valign="bottom">
                                                                    <asp:CheckBox ID="Edit_PrePopulateCHKBX" runat="Server" Enabled='<%# DataBinder.Eval(Container.DataItem, "ProfileFieldName").Equals("EmailAddress") ? false:true%>'>
                                                                    </asp:CheckBox>
                                                                </td>
                                                                <td width="5%" align="center" valign="bottom">
                                                                    <asp:TextBox ID="Edit_SortOrderTXT" runat="Server" class="formfield" MaxLength="2"
                                                                        Width="26px"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbeEdit_SortOrderTXT" TargetControlID="Edit_SortOrderTXT" FilterType="Numbers" runat="server" />
                                                                </td>
                                                                <td width="10%" align='right' nowrap bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-save.gif alt='Save Smart Form Field Changes' border='0'&gt;"
                                                                        CommandName="Update" CausesValidation="false" ID="UpdateProfileFieldBTN"></asp:LinkButton>&nbsp;
                                                                    <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-cancel.gif alt='Cancel Smart Form Field Changes' border='0'&gt;"
                                                                        CommandName="Cancel" CausesValidation="false" ID="CancelProfileFieldBTN"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <tr class="gridheader">
                                                                <td width="10%" bgcolor="#eeeeee" height="35px" valign="bottom">
                                                                    <asp:DropDownList ID="Add_ProfileFieldNameDR" runat="Server" class="formfield" DataValueField="Name"
                                                                        DataTextField="Name">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="15%" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:TextBox ID="Add_DisplayNameTXT" runat="Server" class="formfield"></asp:TextBox>
                                                                </td>
                                                                <td width="10%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:DropDownList ID="Add_DataTypeDR" runat="Server" class="formfield">
                                                                        <asp:ListItem Value=""> -- NONE -- </asp:ListItem>
                                                                        <asp:ListItem Value="TEXT"> Alpha & Numbers </asp:ListItem>
                                                                        <asp:ListItem Value="NUMBER"> Numbers </asp:ListItem>
                                                                        <asp:ListItem Value="DATE"> Date </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="10%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:DropDownList ID="Add_ControlTypeDR" runat="Server" class="formfield">
                                                                        <asp:ListItem Selected="True" Value="TEXT"> Text Box </asp:ListItem>
                                                                        <asp:ListItem Value="CHECKBOX"> Check Box </asp:ListItem>
                                                                        <asp:ListItem Value="DROPDOWN"> Dropdown List </asp:ListItem>
                                                                        <asp:ListItem Value="RADIOBUTTON"> Radio Buttons </asp:ListItem>
                                                                        <asp:ListItem Value="HIDDEN"> Hidden Field </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td width="20%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <div style="font-size: 10px; font-weight: normal; padding-left: 7px" align="left">
                                                                        <b><u>NOTE:</u></b> Enter values separated by pipe symbol " | "</div>
                                                                    <asp:TextBox ID="Add_DataValuesTXT" runat="Server" class="formfield" Width="170px"></asp:TextBox>
                                                                </td>
                                                                <td width="5%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:CheckBox ID="Add_RequiredCHKBX" runat="Server"></asp:CheckBox>
                                                                </td>
                                                                <td width="5%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:CheckBox ID="Add_PrePopulateCHKBX" runat="Server"></asp:CheckBox>
                                                                </td>
                                                                <td width="5%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:TextBox ID="Add_SortOrderTXT" runat="Server" Width="26px" class="formfield"
                                                                        MaxLength="2"></asp:TextBox>
                                                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbeAdd_SortOrderTXT" runat="server" TargetControlID="Add_SortOrderTXT" FilterType="Numbers" />
                                                                </td>
                                                                <td width="11%" align="center" bgcolor="#eeeeee" valign="bottom">
                                                                    <asp:LinkButton ID="AddProfileFieldBTN" Text="&lt;img src=/ecn.images/images/icon-add.gif alt='Add New Smart Form Field' border='0'&gt;"
                                                                        OnClick="ProfileFieldAdd_Click" runat="Server"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </FooterTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </cpanel:DataPanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td align='right' colspan="4" height="3">
                </td>
            </tr>
        </tbody>
    </table>
    <br />
</asp:Content>

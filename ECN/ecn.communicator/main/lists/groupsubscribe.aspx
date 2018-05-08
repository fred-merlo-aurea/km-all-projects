<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.groupsubscribe" CodeBehind="groupsubscribe.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
<%@ Register TagName="radEditor" Src="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.ascx" TagPrefix="radEditor" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
<%--        function repaintEditor() {

            var html = $find("<%= SO_HTMLCode.ClientID %>" + "_radEditor");
            html.repaint();

        
    }--%>
    function uploadContentSource() {
        var prevReturnValue = window.returnValue; // Save the current returnValue 
        window.returnValue = undefined;

        var media = window.open('/ecn.editor/ckeditor/plugins/sourceHtmlUpload/sourcehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);


    }

    function setUploadHTMLContentSource(args) {
        var textControl = document.getElementById('<%= TxtBx_HTMLCode.ClientID %>');
        if (textControl)
            textControl.value = args;
    }
 
    function uploadContentMobile() {
        var prevReturnValue = window.returnValue; // Save the current returnValue 
        window.returnValue = undefined;

        var media = window.open('/ecn.editor/ckeditor/plugins/mobileHtmlUpload/mobilehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);

    }
        </script>
    <script type="text/javascript">
        function deleteSmartForm(theID) {
            if (confirm('Are you Sure?\n Selected smartForm permanently deleted.')) {
                window.location = "groupsubscribe.aspx?" + theID + "&action=delete";
            }
        }
    </script>
    <style type="text/css">

    .cke_source 
    {
        white-space: pre-wrap !important;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tbody>
            <tr>
                <td class="tableHeader" colspan="2" align="center">
                    <!--Create New smartForms:&nbsp;&nbsp;-->
                    <asp:Button class="formbuttonsmall" ID="DO_SmartFormButton" runat="Server" Enabled="false"
                        Text="Double Optin smartForm" OnClick="DO_SmartFormButton_Click"></asp:Button>
                    &nbsp;&nbsp;
                    <asp:Button class="formbuttonsmall" ID="SO_SmartFormButton" runat="Server" Text="Single Optin smartForms"
                        OnClick="SO_SmartFormButton_Click"></asp:Button>
                    &nbsp;&nbsp;
                    <asp:Button class="formbuttonsmall" ID="PP_SmartFormButton" runat="Server" Text="Pre-Pop smartForms"
                        OnClick="PP_SmartFormButton_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td align='right' colspan="4" height="3"></td>
            </tr>
            <!-- Start Double Optin Panel-->
            <asp:Panel ID="DO_panelFCKEditor" runat="Server">
                <tr>
                    <td align="center" width="30" height="100">
                        <asp:ListBox class="formtextfield" ID="OptinFieldSelection" runat="Server" SelectionMode="Multiple"
                            Height="185px"></asp:ListBox>
                    </td>
                    <td class="tableHeader" valign="bottom" width="83%" align="left">Select the Fields you want<br />
                        to add to your Double Optin smartForm
                        <br />
                        <br />
                        <asp:Button class="formbuttonsmall" ID="RefreshHTML" runat="Server" Text="Rebuild smartForm"
                            OnClick="RefreshHTML_Click"></asp:Button>&nbsp;&nbsp;
                        <asp:Button class="formbuttonsmall" ID="OptinHTMLSave" runat="Server" Text="Save smartForm"
                            Enabled="false" OnClick="SaveOptinHTML_Click"></asp:Button><br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="tableHeader" colspan="2" align="center">Double Optin smartForm HTML
                    </td>
                </tr>
                <tr>
                    <td valign="bottom" align='right' width="100" colspan="2" height="262">
<%--                        <radeditor:radeditor id="HTMLCode" runat="server" />--%>
                        <CKEditor:CKEditorControl ID="HTMLCode" runat="server" Skin="kama" Height="280" Width="775"
                            BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
                    </td>
                </tr>
            </asp:Panel>
            <!-- end DoubleOptin Panel-->
            <!-- Start Single Optin Panel-->
            <asp:Panel ID="SO_panelFCKEditor" runat="Server" Visible="false">
                <tr>
                    <td colspan="2" bgcolor="#eeeeee">
                        <cpanel:datapanel id="DataPanel1" style="z-index: 101" runat="Server" expandimageurl="expand.gif"
                            collapseimageurl="collapse.gif" collapsetext="Click to hide  Single Optin smartForms List"
                            expandtext="Click to display Single Optin smartForms List" collapsed="False"
                            titletext="List of Single Optin smartForms" allowtitleexpandcollapse="True">
                            <asp:DataGrid ID="SmartFormGrid" runat="Server" Width="100%" AutoGenerateColumns="False"
                                HorizontalAlign="Center" CssClass="grid">
                                <ItemStyle Height="17"></ItemStyle>
                                <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="SmartFormName" HeaderText="Single Optin smartForms">
                                    </asp:BoundColumn>
                                    <asp:HyperLinkColumn ItemStyle-Width="5%" Text="<img src=/ecn.images/images/icon-edits1.gif alt='Edit Group / View Emails' border='0'>"
                                        DataNavigateUrlField="SmartFormID" DataNavigateUrlFormatString="groupsubscribe.aspx?{0}">
                                        <ItemStyle HorizontalAlign="center" />
                                    </asp:HyperLinkColumn>
                                    <asp:TemplateColumn HeaderText="" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <a href='javascript:deleteSmartForm("<%#(string) DataBinder.Eval(Container.DataItem, "SmartFormID") %>");'>
                                                <center>
                                                    <img src='/ecn.images/images/icon-delete1.gif' alt='Delete smartForm' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <AlternatingItemStyle CssClass="gridaltrow" />
                            </asp:DataGrid>
                            <AU:PagerBuilder ID="GridPager" runat="Server" Width="100%" PageSize="3" ControlToPage="SmartFormGrid">
                                <PagerStyle CssClass="gridpager"></PagerStyle>
                            </AU:PagerBuilder>
                        </cpanel:datapanel>
                    </td>
                </tr>
                <tr>
                    <td align='right' colspan="4" height="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlEdit" runat="server">
                            <table style="width:100%;">
                                <tr>
                                    <td valign="top" align="center" width="30" height="100">
                                        <asp:ListBox class="formtextfield" ID="SO_OptinFieldSelection" runat="Server" SelectionMode="Multiple"
                                            Height="255px"></asp:ListBox>
                                    </td>
                                
                                    <td class="tableHeader" valign="top" width="700" align="left">
                                        <table width="100%" style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid; border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid"
                                            cellspacing="1" cellpadding="1" border='1'>
                                            <tr>
                                                <td class="tableHeader1" align='right' width="14%">
                                                    <b>smartForm&nbsp;<br />
                                                        Name:</b>
                                                </td>
                                                <td colspan='3'>
                                                    <asp:TextBox class="formtextfieldsmall" ID="smartFormName" TabIndex="1" runat="Server"
                                                        Columns="41"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader1" valign="bottom" colspan="2">Automated Internal Email:
                                                </td>
                                                <td class="tableHeader1" valign="bottom" colspan="2">Automated Response to Submitted Email:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">Internal Email&nbsp;<br />
                                                    Address(es):
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminEmail" TabIndex="2" runat="Server"
                                                        Columns="35"></asp:TextBox>
                                                </td>
                                                <td class="tableHeaderSmall" align='right' width="14%">Email From:
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_FromEmail" TabIndex="5" runat="Server"
                                                        Columns="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">Email Subject:
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminMsgSubject" TabIndex="3"
                                                        runat="Server" Columns="35"></asp:TextBox>
                                                </td>
                                                <td class="tableHeaderSmall" align='right' width="14%">Email Subject:
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserMsgSubject" TabIndex="6"
                                                        runat="Server" Columns="35"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeaderSmall" align='right' width="14%">Email Body:
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_AdminMsgBody" TabIndex="4" runat="Server"
                                                        Columns="43" TextMode="multiline" Rows="3"></asp:TextBox>
                                                    
                                                </td>
                                                <td class="tableHeaderSmall" align='right' width="14%">Email Body:
                                                </td>
                                                <td width="150">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserMsgBody" TabIndex="7" runat="Server"
                                                        Columns="43" TextMode="multiline" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="3"></td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader" align="left" colspan="4">Response Landing Page:<br />
                                                    <div align="left" class="tableContentSmall">
                                                        <u>NOTE</u>:A webpage URL starting with http:// [OR] HTML source code
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox class="formtextfieldsmall" ID="Response_UserScreen" TabIndex="8" runat="Server"
                                                        Columns="80" TextMode="multiline" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="3">
                                                    <hr size="1" color="#999999">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" class="tableHeader" align="center">
                                                    <asp:Button class="formbuttonsmall" ID="SO_RefreshHTML" runat="Server" Text="Rebuild smartForm Fields"
                                                        Enabled="false" OnClick="RefreshHTML_Click"></asp:Button>&nbsp;&nbsp;
                                    <asp:Button class="formbuttonsmall" ID="SO_OptinHTMLSave" runat="Server" Text="Save smartForm"
                                        Enabled="false" OnClick="SaveOptinHTML_Click"></asp:Button>&nbsp;&nbsp;
                                    <asp:Button class="formbuttonsmall" ID="SO_OptinHTMLSaveNew" runat="Server" Text="Create New smartForm"
                                        Enabled="true" OnClick="SO_OptinHTMLSaveNew_Click"></asp:Button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align='right' colspan="4" height="2"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>


                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEditor" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td class="tableHeader" colspan="2">Single Optin smartForm HTML
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" align='right' width="100" colspan="2" height="262">
                                       <%-- <radeditor:radeditor id="SO_HTMLCode" runat="server" />--%>
                                        
                                        <CKEditor:CKEditorControl ID="SO_HTMLCode" runat="server" Skin="kama" Height="280"
                            Width="775" BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>

                </tr>

            </asp:Panel>
            <!-- end Single Optin Panel-->
            <asp:Panel ID="panelTexbox" runat="Server">
                <tr>
                    <td>
                        <asp:TextBox ID="TxtBx_HTMLCode" runat="Server" Columns="55" TextMode="multiline"
                            Rows="15" EnableViewState="true"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
        </tbody>
    </table>
    <br />

</asp:Content>

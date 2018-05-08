<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="layoutEditor.ascx.cs"
    Inherits="ecn.communicator.main.ECNWizard.Content.layoutEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/main/ECNWizard/Content/contentEditor.ascx" TagName="contentEditor"
    TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/contentExplorer.ascx" TagName="contentExplorer"
    TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<script type="text/javascript">
    function notyet() {
        confirm('Something is wrong with your smart content subscription. Please contact your customer service representative for a solution.');
    }
    function upgrade() {
        confirm('You must upgrade your version of .communicator to access Dynamic Content');
    }
    
</script>
<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .modalPopup {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        z-index: 9999;
    }

    .modalPopupCreateContent {
        background-color: #D0D0D0;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 90%;
        overflow: auto;
    }

    .buttonMedium {
        width: 135px;
        background: url(buttonMedium.gif) no-repeat left top;
        border: 0;
        font-weight: bold;
        color: #ffffff;
        height: 20px;
        cursor: pointer;
        padding-top: 2px;
    }

    .TransparentGrayBackground {
        position: fixed;
        top: 0;
        left: 0;
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        height: 100%;
        width: 100%;
        min-height: 100%;
        min-width: 100%;
    }

    .overlay {
        position: fixed;
        z-index: 99;
        top: 0px;
        left: 0px;
        background-color: gray;
        width: 100%;
        height: 100%;
        filter: Alpha(Opacity=70);
        opacity: 0.70;
        -moz-opacity: 0.70;
    }

    /** html .overlay {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }*/

    .loader {
        z-index: 100;
        position: fixed;
        width: 120px;
        margin-left: -60px;
        background-color: #F4F3E1;
        font-size: x-small;
        color: black;
        border: solid 2px Black;
        top: 40%;
        left: 50%;
    }

    * html .loader {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
        .RadWindow.RadWindow_Default.rwNormalWindow.rwTransparentWindow
    {
         z-index:100020 !important;
    }
        .reContentArea{
            height:100% !important;
        }

</style>





<asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="upMain" DynamicLayout="false">
    <ProgressTemplate>
        <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
            <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
                <div>
                    <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <table>
            <tr style="text-align:right;">
                <td>
                     <div style="text-align: right; padding-right: 8px;padding-top:15px;">

                        <label class="tableHeader">
                        Category:
                        </label>
                        <asp:DropDownList ID="ddlCategoryFilter" runat="server" AutoPostBack="True" DataTextField="Category" DataValueField="TemplateID" OnSelectedIndexChanged="ddlCategoryFilter_IndexChanged">
                        </asp:DropDownList>

                </div>
                </td>
            </tr>
            <tr>

                <td width="100%">
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Size="Large" Text="Template"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">
                            <tr align="right">
                                <td colspan="2">
                                    <asp:Label runat="Server" Text="Selected Template:" Font-Bold="true" Font-Size="Medium" />
                                    <asp:Label ID="lblTemplateName" runat="Server" CssClass="makeItalic" Text="No Selected Template" font-style="italic" Font-Size="Medium" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <img src="/ecn.images/images/icon-preview-HTML.gif" alt="Preview" border="0">
                                    <asp:LinkButton ID="btnPreview" runat="Server" Text="Preview" Font-Bold="true" Font-Size="Medium" OnClick="btnPreview_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td>
                                    <asp:DataList ID="templaterepeater" runat="server" CellSpacing="2" CellPadding="2"
                                        GridLines="None" RepeatDirection="Horizontal" RepeatColumns="4" RepeatLayout="Table"
                                        BorderWidth="0" OnItemCommand="DoItemSelect" SelectedItemStyle-BackColor="Transparent"
                                        SelectedItemStyle-VerticalAlign="Top" SelectedItemStyle-HorizontalAlign="Center"
                                        SelectedItemStyle-BorderWidth="2" SelectedItemStyle-BorderColor="Gray" SelectedItemStyle-BorderStyle="Dashed"
                                        SelectedItemStyle-Font-Size="xxsmall" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-BorderWidth="0" DataKeyField="TemplateID">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl='<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>' CommandName="Select" /><br>
                                            <br>
                                            <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <SelectedItemTemplate>
                                            <img src="<%#DataBinder.Eval(Container.DataItem,"TemplateImage")%>"><br>
                                            <br>
                                            <%#DataBinder.Eval(Container.DataItem,"TemplateName")%>
                                            <br>
                                            <asp:TextBox runat="server" ID="TemplateID" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>'></asp:TextBox>
                                            <asp:TextBox runat="server" ID="SlotsTotal" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem, "SlotsTotal") %>'></asp:TextBox>
                                        </SelectedItemTemplate>
                                    </asp:DataList>
                                </td>
                                <td width="40%">
                                    <asp:HiddenField ID="SelectedSlot" runat="server" Value="0" />
                                    <table cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Medium" Text="Content Slots"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot1" runat="Server" HorizontalAlign="Left">
                                                    Slot 1 -
                                    <asp:Label ID="lblSlot1" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot1" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent1" runat="server" OnClick="CreateContent_Show"
                                                        Text="CreateContent" CausesValidation="false"></asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent1" runat="server" OnClick="ExistingContent_Show"
                                        CausesValidation="false">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink1" runat="Server" NavigateUrl="javascript:notyet();"
                                        DataNavigateUrlField="ContentID">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot2" runat="Server" HorizontalAlign="Left">
                                                    Slot 2 -
                                    <asp:Label ID="lblSlot2" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot2" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent2" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent2" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink2" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot3" runat="Server" HorizontalAlign="Left">
                                                    Slot 3 -
                                    <asp:Label ID="lblSlot3" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot3" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent3" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent3" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink3" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot4" runat="Server" HorizontalAlign="Left">
                                                    Slot 4 -
                                    <asp:Label ID="lblSlot4" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot4" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent4" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent4" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink4" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot5" runat="Server" HorizontalAlign="Left">
                                                    Slot 5 -
                                    <asp:Label ID="lblSlot5" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot5" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent5" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent5" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink5" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot6" runat="Server" HorizontalAlign="Left">
                                                    Slot 6 -
                                    <asp:Label ID="lblSlot6" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot6" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent6" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent6" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink6" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot7" runat="Server" HorizontalAlign="Left">
                                                    Slot 7 -
                                    <asp:Label ID="lblSlot7" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot7" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent7" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent7" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink7" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot8" runat="Server" HorizontalAlign="Left">
                                                    Slot 8 -
                                    <asp:Label ID="lblSlot8" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot8" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent8" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent8" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink8" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Slot9" runat="Server" HorizontalAlign="Left">
                                                    Slot 9 -
                                    <asp:Label ID="lblSlot9" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    <br />
                                                    <asp:HiddenField ID="hdnSlot9" runat="server" Value="0" />
                                                    <asp:LinkButton ID="lnkbtnCreateContent9" runat="server" OnClick="CreateContent_Show">CreateContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:LinkButton ID="lnkbtnExistingContent9" runat="server" OnClick="ExistingContent_Show">ExistingContent</asp:LinkButton>&nbsp&nbsp
                                    <asp:HyperLink ID="HyperLink9" runat="Server" NavigateUrl="javascript:notyet();">SmartContent</asp:HyperLink>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr valign="top">
                <td width="100%">
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Large" Text="Details"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table cellspacing="0" cellpadding="4" border="0" style="width: 100%; border-collapse: collapse;">

                            <tr>
                                <td align='right' valign="top">&nbsp;<span class="label">Name</span>&nbsp;
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="LayoutName" runat="Server" CssClass="formfield" Width="225" MaxLength="50"
                                        ValidationGroup="Main"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="val_LayoutName" runat="Server" CssClass="errormsg"
                                        Display="Static" ErrorMessage="LayoutName is a required field." ControlToValidate="LayoutName"
                                        ValidationGroup="Main"><-- Required</asp:RequiredFieldValidator>
                                </td>
                                <td valign="top">&nbsp;<asp:Button class="formbuttonsmall" ID="CreateAsNewTopButton" OnClick="CreateAsNewInitialize"
                                    runat="Server" Visible="false" Text="Create as new Message" CssClass="formfield"></asp:Button>
                                </td>
                                <td class="tableHeader" valign="top" align='right'>
                                    <span class="label">Border</span>&nbsp;
                                </td>
                                <td align='left'>
                                    <asp:RadioButtonList ID="TemplateBorder" runat="Server" EnableViewState="true" OnSelectedIndexChanged="TemplateBorder_Change"
                                        AutoPostBack="true" RepeatLayout="flow" RepeatColumns="3" CssClass="tableContent">
                                        <asp:ListItem id="WantBorder" runat="Server" Value="Y" Text="Yes" EnableViewState="true" />
                                        <asp:ListItem id="NoBorder" runat="Server" Value="N" Text="No" EnableViewState="true" />
                                        <asp:ListItem id="CustomBorder" runat="Server" Value="C" Text="Custom" EnableViewState="true" />
                                    </asp:RadioButtonList>
                                    <asp:TextBox ID="TableOptions" runat="Server" Columns="50" Visible="False" Text=""></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableHeader" align='right'>&nbsp;<span class="label">Folder</span>&nbsp;
                                </td>
                                <td colspan="2" align='left'>
                                    <asp:DropDownList ID="folderID" runat="Server" DataTextField="FolderName" DataValueField="FolderID"
                                        EnableViewState="true" CssClass="formfield" Width="225">
                                    </asp:DropDownList>
                                </td>
                                <td class="tableHeader" align='right'>
                                    <span class="label">Address&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="DisplayAddress" runat="Server" Columns="70" Width="225" CssClass="formfield"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td class="tableHeader" valign="top" align='right'>
                                    <span class="label">Size&nbsp;</span>
                                </td>
                                <td colspan="4" align='left'>
                                    <asp:Label ID="SizeLabel" runat="Server" CssClass="tableContent"></asp:Label>
                                </td>
                            </tr>
                            <asp:Panel ID="pnlMessageTypes" runat="Server" Visible="false">
                                <tr>
                                    <td colspan="5">
                                        <table border="0" width="100%">
                                            <tr>
                                                <td class="tableHeader" align='right' width="125px">&nbsp;<span class="label">Message Type</span>&nbsp;
                                                </td>
                                                <td align='left'>
                                                    <asp:DropDownList ID="ddlMessageType" runat="Server" DataTextField="Name" DataValueField="MessageTypeID"
                                                        EnableViewState="true" CssClass="formfield" Width="225">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="val_ddlMessageType" runat="Server" CssClass="errormsg"
                                                        Display="Static" ErrorMessage="Message Type is a required field." ControlToValidate="ddlMessageType"
                                                        InitialValue="0" ValidationGroup="Main"><-- Required</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </table>
                    </fieldset>
                </td>

            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupPreview" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlPreview" TargetControlID="btnShowPopup3">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlPreview" CssClass="modalPopup">
    <asp:UpdateProgress ID="upPreviewProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upPreview" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upPreviewProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upPreviewProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPreview" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <CKEditor:CKEditorControl ID="FCKeditor1" runat="server" Skin="kama" Height="600px"
                Width="800px" BasePath="/ecn.editor/ckeditor/" Toolbar="Basic" ToolbarStartupExpanded="false"></CKEditor:CKEditorControl>
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btnClosePreview" CssClass="formfield"
                            OnClick="btnClosePreview_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupExistingContent" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlExistingContent" TargetControlID="btnShowPopup2">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlExistingContent" CssClass="modalPopup">
    <asp:UpdateProgress ID="upContentExplorerProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upContentExplorer" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upContentExplorerProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upContentExplorerProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upContentExplorer" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <uc1:contentExplorer ID="contentExplorer1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: center">
                        <asp:Button runat="server" Text="Close" ID="btnExistingContent" CssClass="formfield"
                            OnClick="ExistingContent_Hide"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="btnShowPopup1" runat="server" Style="display: none" />
<cc1:ModalPopupExtender ID="modalPopupCreateContent" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlCreateContent" TargetControlID="btnShowPopup1">
</cc1:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlCreateContent" CssClass="modalPopupCreateContent">
    <asp:UpdateProgress ID="upContentEditorProgress" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upContentEditor" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upContentEditorProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upContentEditorProgressP2" CssClass="loader" runat="server">
                    <div>
                        <center>
                    <br />
                    <br />
                    <b>Processing...</b><br />
                    <br />
                    <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                    <br />
                    <br />
                    <br />
                    </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upContentEditor" runat="server">
        <ContentTemplate>
            <uc1:contentEditor ID="contentEditor1" runat="server" />
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right">
                        <asp:Button runat="server" Text="Save" ID="btnCreateContent" CssClass="formfield"
                            OnClick="CreateContent_Save"></asp:Button>
                    </td>
                    <td style="text-align: left">
                        <asp:Button runat="server" Text="Close" ID="btnClose" CssClass="formfield" OnClick="CreateContent_Close"></asp:Button>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


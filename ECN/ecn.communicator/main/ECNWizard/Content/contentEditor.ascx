<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="contentEditor.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Content.contentEditor" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagName="radEditor" Src="RadEditor/RadEditor.ascx" TagPrefix="radEditor" %>
<script type="text/javascript">
    <%--function repaintEditor() {
        var selectedTab = $find("<%= TabContainer1.ClientID %>").get_activeTabIndex();
        if (selectedTab == 0) {
            var html = $find("<%= radEditor1.ClientID %>" + "_radEditor");
            html.repaint();
        }
        else {
            var html = $find("<%= radEditor2.ClientID %>" + "_radEditor");

            html.repaint();

        }
    }--%>
   function uploadContentSource() {
        var prevReturnValue = window.returnValue; // Save the current returnValue 
        window.returnValue = undefined;

        var media = window.open('/ecn.editor/ckeditor/plugins/sourceHtmlUpload/sourcehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);


        }

    function setUploadHTMLContentSource(args) {
        var textControl = document.getElementById('<%= txtEditorSource.ClientID %>');
        if (textControl)
            textControl.value = args;
    }
 
    function uploadContentMobile() {
        var prevReturnValue = window.returnValue; // Save the current returnValue 
        window.returnValue = undefined;

        var media = window.open('/ecn.editor/ckeditor/plugins/mobileHtmlUpload/mobilehtmlUpload.aspx', "_blank", "width=300px,height=150px, resizable=yes, toolbar=no,status=no,location=no,menubar=no", false);

        }

    function setUploadContentMobile(args) {
        var textMobile = document.getElementById('<%= txtEditorMobile.ClientID %>');
        if (textMobile)
            textMobile.value = args
        }

    function setValue(args) {
        var sourceCKEditor = document.getElementById('<%= TabPanel1.ClientID %>');
        var tabContainer = document.getElementById('<%= TabContainer1.ClientID %>');
        var tabCont = tabContainer.control;
        var currentTab = tabCont.get_activeTab();
        if (currentTab == sourceCKEditor.control) {
            CKEDITOR.instances['<%= FCKeditor1.ClientID %>'].insertHtml(args);
    }
        else {
           CKEDITOR.instances['<%= FCKeditor2.ClientID %>'].insertHtml(args);
        }

    }

    function setTranValue(args)
    {
        var sourceCKEditor = document.getElementById('<%= TabPanel1.ClientID %>');
        var tabContainer = document.getElementById('<%= TabContainer1.ClientID %>');
        var tabCont = tabContainer.control;
        var currentTab = tabCont.get_activeTab();
        if (currentTab == sourceCKEditor.control) {

            
            if (args[1].length == 0) {
                CKEDITOR.instances['<%= FCKeditor1.ClientID %>'].insertHtml(args[0]);
            }
            else {
                var original = CKEDITOR.instances['<%= FCKeditor1.ClientID %>'].getData();
                original = original.replace(args[1], args[0]);
                CKEDITOR.instances['<%= FCKeditor1.ClientID %>'].setData(original);
            }
        }
        else {
            if (args[1].length == 0) {
                CKEDITOR.instances['<%= FCKeditor2.ClientID %>'].insertHtml(args[0]);
            }
            else {
                var original = CKEDITOR.instances['<%= FCKeditor2.ClientID %>'].getData();
                original = original.replace(args[1], args[0]);
                CKEDITOR.instances['<%= FCKeditor2.ClientID %>'].setData(original);
            }
        }
    }

    function setData(args) {
        var sourceCKEditor = document.getElementById('<%= TabPanel1.ClientID %>');
        var tabContainer = document.getElementById('<%= TabContainer1.ClientID %>');
        var tabCont = tabContainer.control;
        var currentTab = tabCont.get_activeTab();
        if (currentTab == sourceCKEditor.control) {
            CKEDITOR.instances['<%= FCKeditor1.ClientID %>'].setData(args);
        }
        else {
 CKEDITOR.instances['<%= FCKeditor2.ClientID %>'].setData(args);
        }
    }


</script>
<style>
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

    .modalPopupPreview {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: Gray;
        padding: 3px;
        height: 80%;
        overflow: auto;
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

    * html .overlay {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }

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

    fieldset {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }

    .styled-select {
        width: 240px;
        background: transparent;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }

    .styled-text {
        width: 240px;
        height: 28px;
        overflow: hidden;
        border: 1px solid #ccc;
    }

    .aspBtn {
        -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
        -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
        box-shadow: inset 0px 1px 0px 0px #ffffff;
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
        background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
        background-color: #ededed;
        -moz-border-radius: 6px;
        -webkit-border-radius: 6px;
        border-radius: 6px;
        border: 1px solid #dcdcdc;
        display: inline-block;
        color: black;
        font-family: arial;
        font-size: 9px;
        font-weight: bold;
        padding: 2px 10px;
        text-decoration: none;
        text-shadow: 1px 1px 0px #ffffff;
    }

        .aspBtn:hover {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background-color: #dfdfdf;
        }

        .aspBtn:active {
            position: relative;
        }
        		.cke_source {
    white-space: pre-wrap !important;
}
</style>
<br />

<br />
<asp:UpdateProgress ID="upContentExplorerProgress" runat="server" DisplayAfter="10"
    Visible="true" AssociatedUpdatePanelID="upContentExplorer" DynamicLayout="true">
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
<asp:UpdatePanel ID="upContentExplorer" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
        <table cellspacing="1" cellpadding="1" border='0' align="center">
            <tbody>
                <tr>
                    <td align='left'>
                        <table cellpadding="0" cellspacing="0" border='0' width="100%">
                            <tr>
                                <td align='left' width="99">&nbsp;<span class="label">Title</span>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox EnableViewState="true" ID="ContentTitle" runat="Server" Width="215" CssClass="formfield"></asp:TextBox>
                                    <asp:Button ID="CreateAsNewTopButton" OnClick="CreateAsNewInitialize" runat="Server"
                                        Text="Create as new Content" class="formbuttonsmall" Visible="false" CssClass="aspBtn" />
                                 </td>
                            </tr>
                            <tr>
                                <td align='left'>
                                    <span class="label">&nbsp;Folder&nbsp;</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList EnableViewState="true" ID="folderID" runat="Server" DataValueField="FolderID"
                                        DataTextField="FolderName" CssClass="formfield" Width="215">
                                    </asp:DropDownList>
                                
                                </td>
                            </tr>
                            <tr>
                                <td align='left'>&nbsp;<span class="label">Owner</span>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList EnableViewState="true" ID="drpUserID" runat="Server" DataValueField="UserID"
                                        DataTextField="UserName" CssClass="formfield" Width="215">
                                    </asp:DropDownList>
                                    <asp:Button ID="ValidateContent" OnClick="ValidateContentInitialize" runat="Server"
                                        Text="Validate Content" class="formbuttonsmall" Visible="true" CssClass="aspBtn" />
                                </td>
                            </tr>
                            <tr>
                                <td align='left'>&nbsp;<span class="label">Editor Type</span>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rblContentType" runat="server" OnSelectedIndexChanged="rblContentType_SelectedIndexChanged" RepeatDirection="Horizontal" AutoPostBack="true">
                                        <asp:ListItem Text="WYSIWYG" Value="True" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Plain HTML" Value="False"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align='left'>&nbsp;<span class="label">Locked</span>&nbsp;
                                </td>
                                <td align="left">
                                    <asp:CheckBox EnableViewState="true" ID="LockedFlag" runat="Server"></asp:CheckBox>
                                    <asp:DropDownList ID="ddContentTypeCode" runat="Server" Visible="false">
                                        <asp:ListItem Value="html" Selected="True">HTML</asp:ListItem>
                                        <asp:ListItem Value="text">TEXT</asp:ListItem>
                                        <asp:ListItem Value="sms">SMS</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                            <table cellspacing="0" cellpadding="0" width="380" align="center">
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
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="left">
                        <br />
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel runat="server" HeaderText="HTML" Height="650px" ID="TabPanel1">
                                <ContentTemplate>
                                    <asp:Panel ID="panelContentSource" Height="600px" runat="server">
                                        <%--<radEditor:radEditor ID="radEditor1" Visible="true" runat="server" />--%>
                                        <CKEditor:CKEditorControl ID="FCKeditor1" runat="server" Skin="kama" Height="450px" Width="780px" BasePath="/ecn.editor/ckeditor"></CKEditor:CKEditorControl>
                                        <asp:Panel ID="pnlTxtSource" Style="align: center" runat="Server" Visible="false">
                                            <br />
                                            <asp:Button ID="btnTxtSourceUpload" runat="server" Text="Upload" CssClass="aspBtn" OnClientClick="uploadContentSource()" />
                                            &nbsp;&nbsp;<asp:Button ID="btnTxtSourcePreview" runat="server" Text="Preview" CssClass="aspBtn" OnClick="btnTxtSourcePreview_Click" />
                                            <br />
                                            <br />
                                            <asp:TextBox ID="txtEditorSource" runat="server" Height="550px" Width="780px" TextMode="MultiLine"></asp:TextBox>
                                        </asp:Panel>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" HeaderText="Text" ID="TabPanel2">
                                <ContentTemplate>
                                    <asp:Button ID="btnConvertoInlineCSS" runat="server" Text="Convert To Inline CSS"
                                        OnClick="btnConvertoInlineCSS_Click" class="formbuttonsmall" />
                                    &nbsp;&nbsp;
                            <asp:Button class="formbuttonsmall" ID="GetTextButton" OnClick="GetTextFromHTML"
                                runat="Server" Visible="true" Text="Pull Text From HTML"></asp:Button>
                                    <br />
                                    <asp:Panel ID="panelContentText" Style="align: center;" runat="Server" Visible="false">
                                        <br />
                                        <asp:TextBox ID="ContentText" runat="Server" CssClass="formfield" EnableViewState="true"
                                            Width="790" Rows="15" Columns="126" TextMode="multiline"></asp:TextBox>
                                        <br />
                                        <asp:Literal ID="Literal1" runat="server" Text="* Links in Text should start with &quot;&amp;lt;http://&quot; and end with &quot;&amp;gt;&quot; (ex. &amp;lt;http://www.ecn5.com&amp;gt;)"></asp:Literal>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" HeaderText="Mobile" Height="500px" ID="TabPanel3">
                                <ContentTemplate>
                                    <asp:Panel ID="panelContentMobile" Style="text-align: center; height: 500px;" runat="Server" Visible="true">
                                    <%--    <radEditor:radEditor ID="radEditor2" Visible="true" runat="server" />--%>
                                        <CKEditor:CKEditorControl ID="FCKeditor2" runat="server" Skin="kama" Height="450"
                                            Width="790" BasePath="/ecn.editor/ckeditor/"></CKEditor:CKEditorControl>
                                        <asp:Panel ID="pnlTxtMobile" Style="align: center" runat="Server" Visible="false">
                                            <br />
                                            <asp:Button ID="btnTxtMobileUpload" runat="server" Text="Upload" CssClass="aspBtn" OnClientClick="uploadContentMobile()" />
                                            &nbsp;&nbsp;<asp:Button ID="btnTxtMobilePreview" runat="server" Text="Preview" CssClass="aspBtn" OnClick="btnTxtMobilePreview_Click" />
                                            <br />
                                            <br />
                                            <asp:TextBox ID="txtEditorMobile" runat="server" Height="450px" Width="780px" TextMode="MultiLine"></asp:TextBox>
                                        </asp:Panel>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="panelContentURL" runat="Server" Visible="false">
                            URL:
                    <asp:TextBox ID="ContentURL" runat="Server" CssClass="formfield" EnableViewState="true"
                        Columns="55"></asp:TextBox>
                        </asp:Panel>
                        <asp:Panel ID="panelContentFilePointer" runat="Server" Visible="false">
                            Path:
                    <asp:TextBox ID="ContentFilePointer" runat="Server" CssClass="formfield" EnableViewState="true"
                        Columns="55"></asp:TextBox>
                        </asp:Panel>
                    </td>
                </tr>
            </tbody>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>


<asp:Button ID="btnShowPopup3" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupPreview" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlPreview" TargetControlID="btnShowPopup3">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlPreview" CssClass="modalPopupPreview">
    <asp:UpdateProgress ID="upPreviewProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="upPreview" DynamicLayout="false">
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
            <asp:Literal ID="lblPreview" runat="server" Mode="Transform" Text=""></asp:Literal>
            <%-- <asp:Label ID="lblPreview" runat="Server"  Text=""></asp:Label>--%>
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
<script>
    function toastrContentValidated() {
        toastr.options =
            {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-ECN-style",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
                "tapToDismiss": false,
            }
        toastr.info('<table>'
                        + '<tbody>'
                            + '<tr>'
                                + '<td valign="middle"><img  class="PopUpImage" alt="Success Check Mark" height="68" src="/ecn.images/images/successEx.jpg" width="70" /></td>'
                                + '<td valign="middle"><span class="PopUpText"><span>Content validated successfully</span></span></td>'
                            + '</tr>'
                         + '</tbody>'
                     + '</table>'
                     );


        $(".toast").css("position", "relative");
        $(".toast").css("margin-top", "-26%");
        $(".toast").css("left", "33%");
        $(".toast-info").css("background-color", "#FFFFFF");


    }
</script>
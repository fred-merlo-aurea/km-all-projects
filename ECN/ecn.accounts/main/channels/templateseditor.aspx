<%@ Page ValidateRequest="false" Language="c#" Inherits="ecn.communicator.channelsmanager.templateseditor"
    CodeBehind="templateseditor.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls.HtmlTextBox" Assembly="ActiveUp.WebControls.HtmlTextBox" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../MasterPages/ECN_Controls.css" rel="stylesheet" />
    <link href="../../MasterPages/ECN_MainMenu.css" rel="stylesheet" />
    <script type="text/javascript">
        function ChangeTextBoxValue() {
            $("[id$=txtNewCategoy]").text($("[id$=txtInputCategory]").val());
        }
        function CommitDDLValue() {
            $("[id$=txtNewCategoy]").text($("[id$=drpCategory] option:selected").text());
        }
    </script>
    <style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

     .modalPopup
    {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
    }

    .TransparentGrayBackground
    {
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

    fieldset
    {
        -webkit-border-radius: 8px;
        -moz-border-radius: 8px;
        border-radius: 8px;
    }

    .overlay
    {
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

    * html .overlay
    {
        position: absolute;
        height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
        width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
    }

    .loader
    {
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

    * html .loader
    {
        position: absolute;
        margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
    }
    .cke_source 
    {
    white-space: pre-wrap !important;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <div style="position: relative;">
    <%--<div>--%>
        <table id="layoutWrapper" cellspacing="2" cellpadding="2" width="100%" border='0'>
        <tbody>
            <tr>
                <td colspan="2">
                    <br />
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
                        <br />
                        <br />
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="tableHeader" colspan='2' align="left">
                    <div style="display: inline-block; position: relative;">
                        &nbsp;Template    
                    </div>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Channel
                </td>
                <td colspan="2" align="left">
                    <asp:DropDownList EnableViewState="true" ID="ddlBaseChannelID" runat="Server" DataValueField="BaseChannelID"
                        DataTextField="BaseChannelName" OnSelectedIndexChanged="ddlBaseChannelID_SelectedIndexChanged" CssClass="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Active
                </td>
                <td colspan="2" align="left">
                    <asp:CheckBox EnableViewState="true" ID="cbActiveFlag" runat="Server"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="bottom">
                    &nbsp;Name&nbsp;
                </td>
                <td id="tdShrink" align="left">
                    <asp:TextBox EnableViewState="true" ID="tbTemplateName" runat="Server" size="50" CssClass="formfield"></asp:TextBox>
                    <label style="display: inline-block; padding-left: 8px" class="tableHeader">Category</label>    
                    <asp:Label style=" display: inline-block; padding-left: 8px; font-size: 13px; font-family: Verdana, Arial, Helvetica, sans-serif" runat="server" id="txtNewCategoy"></asp:Label>  
                    <div style="display: inline-block; position: relative; bottom: -20px; left: -40px">
                        <ul class="ECN-InfoLinks" >
                            <li>
                                <asp:Image ID="ImgGear" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                                <ul>
                                    <li><asp:LinkButton ID="NewCategoryTag" runat="server" Text="New Category" CssClass="aspBtn" OnClick="btnNewCategory_Click"/></li>
                                    <li><asp:LinkButton ID="ExistingCategoryTag" runat="server" Text="Existing Category" CssClass="aspBtn" OnClick="btnExistingCategory_Click"/></li>
                                </ul>
                            </li>
                        </ul> 
                    </div>
                </td>

                <td class="tableHeader" colspan="1" rowspan="5">
                    <asp:Image ID="imgPreview" runat="Server" Visible="false"></asp:Image>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    Description
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTemplateDescription" runat="Server" size="50" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    &nbsp;Style
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlTemplateStyleCode" runat="Server" DataValueField="CodeValue"
                        DataTextField="CodeName" CssClass="formfield">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    ImageURL
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTemplateImage" runat="Server" size="50" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right'>
                    Slots Total
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlSlotsTotal" runat="Server" CssClass="formfield">
                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                        <asp:ListItem Value="8" Text="8"></asp:ListItem>
                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;HTML Source&nbsp;
                </td>
                <td colspan="2">
                 <%--   <telerik:RadEditor ID="TemplateSource" Enabled="true" OnClientLoad="assignStyles" runat="server" Height="450px" Width="780px" EditModes="Html,Design,Preview"  ToolsFile="/ecn.communicator/main/ECNWizard/Content/RadEditor/Tools.xml" Visible="true" />
                --%>    
                    <CKEditor:CKEditorControl ID="TemplateSource" runat="server" Skin="kama" Height="450"
                        Width="700" BasePath="/ecn.editor/ckeditor/" ToolbarSet="Basic"></CKEditor:CKEditorControl>
                </td>
            </tr>
            <tr>
                <td class="tableHeader" align='right' valign="top">
                    &nbsp;Text Source&nbsp;
                </td>
                <td colspan="2" align="left">
                    <asp:TextBox ID="tbTemplateText" runat="Server" Rows="15" Columns="90" TextMode="multiline"
                        EnableViewState="true" CssClass="formfield"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan='3'>
                    <hr>
                </td>
                <td>
                </td>
            </tr>
            
            <tr>
                <td class="tableHeader" colspan='3' align="middle">
                    <asp:TextBox EnableViewState="true" Visible="false" ID="tbTemplateID" runat="Server"></asp:TextBox>
                    <asp:Button ID="btnSave" OnClick="btnSave_click" Visible="true" Text="Create" class="formbutton"
                        runat="Server" />
                </td>
            </tr>
        </tbody>
    </table>    
</div>

<asp:Button ID="Button1" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupCreateCategory" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlCategoryEditor" TargetControlID="Button1">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlCategoryEditor"  CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upCategoryEditor" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upCategoryEditorProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upCategoryEditorProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upCategoryEditor" runat="server" UpdateMode="Always">
        <ContentTemplate ><br />
            <div align="center">
                <label class="tableHeader">New Category Title</label><br/><br/>
                <asp:TextBox ID="txtInputCategory" runat="server"></asp:TextBox>
            </div>
            <table align="center" class="style1">
                <tr>
                    <td style="text-align: right; padding-top: 12px">
                        <asp:Button runat="server" Text="Ok" ID="btnCategoryEditorSave"  CssClass="ECN-Button-Small"  OnClientClick="ChangeTextBoxValue()" OnClick="btnCategoryEditorSave_Click"></asp:Button>
                    </td>
                    <td style="text-align: left; padding-top: 12px">
                        <asp:Button runat="server" Text="Cancel" ID="btnCategoryEditorClose"  CssClass="ECN-Button-Small" OnClick="btnCategoryEditorClose_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button ID="Button2" runat="server" Style="display: none" />
<asp:ModalPopupExtender ID="modalPopupExistingCategory" runat="server" BackgroundCssClass="modalBackground"
    PopupControlID="pnlCategoryExisting" TargetControlID="Button2">
</asp:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlCategoryExisting" CssClass="modalPopup">
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" DisplayAfter="10"
        Visible="true" AssociatedUpdatePanelID="upCategoryExisting" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Panel ID="upCategoryExistingProgressP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upCategoryExistingProgressP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="upCategoryExisting" runat="server" UpdateMode="Always">
        <ContentTemplate>    <br />       
            <table align="center" cellspacing="3">
                <tr>
                    <td colspan="2" style="text-align: center">
                        <asp:Label ID="Label2" runat="server" Text="Existing Category" Font-Size="Small" class="tableHeader"/><br/><br/>
                        <asp:DropDownList ID="drpCategory" DataValueField="TemplateID" DataTextField="Category" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-top: 12px">
                        <asp:Button runat="server" Text="Ok" ID="btnExitingCategoryOk"  CssClass="ECN-Button-Small" OnClientClick="CommitDDLValue()" OnClick="btnExitingCategoryOk_Click"></asp:Button>
                    </td>
                    <td style="text-align: left; padding-top: 12px">
                        <asp:Button runat="server" Text="Cancel" ID="btnExistingCategoryClose"  CssClass="ECN-Button-Small" OnClick="btnExistingCategoryClose_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table><br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
        <link href="/ecn.communicator/main/ECNWizard/Content/RadEditor/RadEditor.css" type="text/css" rel="stylesheet" />
    <script src="/ecn.communicator/main/ECNWizard/Content/RadEditor/radeditor_Custom.js" type="text/javascript"></script>
</asp:Content>

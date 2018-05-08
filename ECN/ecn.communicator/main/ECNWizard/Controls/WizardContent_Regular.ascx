<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardContent_Regular.ascx.cs"
    Inherits="ecn.communicator.main.ECNWizard.Controls.WizardContent" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutEditor.ascx" TagName="layoutEditor"
    TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer"
    TagPrefix="uc1" %>
<%@ Register Src="~/main/ECNWizard/OtherControls/EmailSubject.ascx" TagName="EmailSubject"
    TagPrefix="uc1" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />
<br />
<script language="javascript" type="text/javascript">
    function getobj(id) {
        //alert('In the method!!');
        if (document.all && !document.getElementById)
            obj = eval('document.all.' + id);
        else if (document.layers)
            obj = eval('document.' + id);
        else if (document.getElementById)
            obj = document.getElementById(id);

        return obj;
    }

    function replyTo_focus() {
        try {
            getobj('<%=txtReplyTo.ClientID%>').value = getobj('<%=txtEmailFrom.ClientID%>').value;
        }
        catch (err) { }
    }

    var AlreadyDone = new Array();

    if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
    else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
    else { document.addEventListener('load', pageloaded, false); }
    function pageloaded() {

        var initialString = $('#<%= txtEmailSubject.ClientID %>').val();

        if (AlreadyDone.indexOf('<%= txtEmailSubject.ClientID %>') < 0) {
            try {
                initialString = initialString.replace(/'/g, "\\'");
                initialString = initialString.replace(/\r?\n|\r/g, ' ');
                initialString = twemoji.parse(eval("\'" + initialString + "\'"));
                $('#<%= txtEmailSubject.ClientID %>').twemojiPicker({ init: initialString, height: '30px', size: "16px", hideEmoji: false });
            }
            catch (err) {
                $('#<%= txtEmailSubject.ClientID %>').twemojiPicker({ height: '30px', size: "16px", hideEmoji: false });
            }
            AlreadyDone.push('<%=txtEmailSubject.ClientID %>');
        }
    }

    function getImage(arraytosearch, key, valuetosearch) {

        for (var i = 0; i < arraytosearch.length; i++) {

            if (arraytosearch[i][key] == valuetosearch) {
                return arraytosearch[i];
            }
        }
        return null;
    }
</script>
<style>
    .twemoji-icon-picker img {
        position: absolute;
        /*top: -15px !important;*/
        right: 25%;
    }

    .twemoji-wrap {
        top: 20px;
    }

    .outer-container {
        top: -20px !important;
    }
</style>
<asp:ScriptManagerProxy ID="wcr_SMP" runat="server" />
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
                            <img style="padding: 0 0 0 15px;"
                                src="/ecn.images/images/errorEx.jpg"></td>
                        <td valign="middle" align="left" width="80%" height="100%">
                            <asp:Label ID="lblErrorMessage1" runat="Server"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td id="errorBottom"></td>
        </tr>
    </table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phWarning" runat="Server" Visible="false">
    <table cellspacing="0" cellpadding="0" width="674" align="center" style="border: 2px solid #856200; border-radius: 25px; background-color: white">
        <tr>
            <td id="">
                <table height="67" width="90%">
                    <tr>
                        <td valign="top" align="center" width="20%">
                            <div style="padding-top: 20px">
                                <img style="padding: 0 0 0 15px;" src="/ecn.images/images/warningEx.jpg">
                            </div>
                        </td>
                        <td valign="middle" align="left" width="80%" height="100%">
                            <asp:Label ID="lblWarningMessage" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <div style="display: inline-block; padding-left: 20px">
                                <asp:Button runat="server" Text="Manage Groups" />
                            </div>
                            <div style="display: inline-block; padding-left: 12px">
                                <asp:Button runat="server" Text="Continue" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="plCreate" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" style="margin-top:15px;" width="100%">
        <tr align="left" valign="middle">
            <td width="20%"></td>
            <td class="formLabel" style="padding-left: 30px">Select one of the Message options:
            </td>
            <td class="formLabel">
                <asp:RadioButton ID="rbNewLayout" GroupName="grpSelect" Text="Create New Message"
                    runat="server" AutoPostBack="True" CssClass="expandAccent" OnCheckedChanged="rbNewLayout_CheckedChanged"></asp:RadioButton>
            </td>
            <td class="formLabel">
                <asp:RadioButton ID="rbExistingLayout" CssClass="expandAccent" AutoPostBack="True"
                    runat="server" Text="Use Existing Message" Checked="True" GroupName="grpSelect" OnCheckedChanged="rbExistingLayout_CheckedChanged"></asp:RadioButton>
            </td>
            <td width="20%"><asp:HiddenField ID="HiddenSelectedLayoutID" runat="server" /></td>
        </tr>
    </table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="plExistingLayout" runat="server">
    <table width="100%">
        <tr>
            <td style="padding-left: 30px; padding-right: 30px;">
                <asp:UpdatePanel ID="upExistingLayout" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <uc1:layoutExplorer ID="layoutExplorer1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:PlaceHolder>
<asp:PlaceHolder ID="plNewLayout" runat="server" Visible="False">
    <table width="100%">
        <tr align="center">
            <td width="10%"></td>
            <td width="80%">
                <uc1:layoutEditor ID="layoutEditor1" runat="server" />
            </td>
            <td width="10%"></td>
        </tr>
    </table>
</asp:PlaceHolder>
<asp:UpdatePanel ID="upEnvelope" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        <table width="100%">
            <tr>
                <td style="padding-left: 30px; padding-right: 30px;">
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>Envelope
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table cellspacing="2" cellpadding="2" style="width: 100%;">
                            <tr>
                                <td colspan="2"></td>
                                <td colspan="2" class="label">
                                    <b>
                                        <asp:Label ID="dyanmicFieldsLbl" Visible="false" runat="server">Dynamic Personalization Fields [optional]:</asp:Label></b>
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" valign="middle" style="width: 10%;" align='right'>From Email&nbsp;
                                </td>
                                <td nowrap="nowrap" style="width: 50%;" align="left">
                                    <asp:TextBox class="formfield" ID="txtEmailFrom" runat="Server" Width="264px" Columns="40" ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList
                                        class="formfield" ID="drpEmailFrom" runat="Server" Visible="false" AutoPostBack="true"
                                        OnSelectedIndexChanged="drpEmailFrom_OnSelectedIndexChanged" Width="264px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="formValidation" ID="val_txtEmailFrom"
                                        runat="Server" ControlToValidate="txtEmailFrom" InitialValue="" ErrorMessage="From Email is a required field."
                                        CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                    <asp:Button ID="btnChangeEnvelope" runat="server" Text="Change" OnClick="btnChangeEnvelope_onclick"
                                        Style="font-size: 12px; height: 19px" />
                                </td>
                                <td class="formLabel" style="width: 10%;" valign="middle" align='right'>
                                    <asp:Label ID="dyanmicEmailFromLbl" Visible="false" runat="server"> From Email&nbsp;</asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:DropDownList ID="dyanmicEmailFrom" runat="server" Width="200px" Visible="false"
                                        class="formfield" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" valign="middle" align='right' width="120">Reply To&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left">
                                    <asp:TextBox class="formfield" ID="txtReplyTo" Width="264px" onfocus="replyTo_focus()" runat="Server"
                                        Columns="40" ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList class="formfield"
                                            ID="drpReplyTo" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpReplyTo_OnSelectedIndexChanged"
                                            Width="264px">
                                        </asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="formValidation" ID="val_txtReplyTo"
                                        runat="Server" ControlToValidate="txtReplyTo" InitialValue="" ErrorMessage="ReplyTo is a required field."
                                        CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                </td>
                                <td class="formLabel" valign="middle" align='right'>
                                    <asp:Label ID="dyanmicReplyToEmailLbl" Visible="false" runat="server">Reply To&nbsp;</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dyanmicReplyToEmail" runat="server" Width="200px" Visible="false"
                                        class="formfield" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="formLabel" valign="middle" align='right' width="120">From Name&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left">
                                    <asp:TextBox class="formfield" ID="txtEmailFromName" Width="264px" runat="Server" Columns="40"
                                        ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList class="formfield"
                                            ID="drpEmailFromName" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpEmailFromName_OnSelectedIndexChanged"
                                            Width="264px">
                                        </asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="formValidation" ID="val_txtEmailFromName"
                                        runat="Server" ControlToValidate="txtEmailFromName" InitialValue="" ErrorMessage="Email From Name is a required field."
                                        CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                </td>
                                <td class="formLabel" valign="middle" align='right'>
                                    <asp:Label ID="dyanmicEmailFromNameLbl" Visible="false" runat="server"> From Name&nbsp;</asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="dyanmicEmailFromName" runat="server" Width="200px" Visible="false"
                                        class="formfield" DataTextField="ShortNameText" DataValueField="ShortNameValue">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 70px;">
                                <td class="formLabel" align='right' width="120" style="padding-bottom: 30px;">Subject&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" style="margin-left: 0px;">

                                    <asp:HiddenField ID="txtEmailSubject" runat="server" />
                                    <br />
                                    <br />
                                    <%-- <asp:TextBox class="formfield" ID="txtSubject" runat="Server" Columns="40" ValidationGroup="formValidation"></asp:TextBox> --%>
                                </td>
                                <td class="formLabel" valign="middle" align='right'></td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>


    </ContentTemplate>
</asp:UpdatePanel>




<asp:Button ID="btnShowPopup5" runat="server" Style="display: none" />
<ajaxToolkit:ModalPopupExtender ID="modalPopupUdfChoice" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlUdfChoice" TargetControlID="btnShowPopup5">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlUdfChoice" Style="display: none;" CssClass="modalPopup" Width="600">
    <asp:UpdateProgress ID="upProgressUdfChoice" runat="server" DisplayAfter="10" Visible="true" AssociatedUpdatePanelID="UpdatePanel6" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="upProgressUdfChoiceP1" CssClass="overlay" runat="server">
                <asp:Panel ID="upProgressUdfChoiceP2" CssClass="loader" runat="server">
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
    <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnContinueUDFCheck" />
        </Triggers>
        <ContentTemplate>
            <table cellspacing="0" cellpadding="0" width="674" align="center" style="border: 2px solid #856200; border-radius: 25px; background-color: white">
                <tr>
                    <td id="">
                        <table height="67" width="90%" style="padding-top: 20px">
                            <tr>
                                <td valign="top" align="center" width="20%">
                                    <img style="padding: 0 0 0 15px;" src="/ecn.images/images/warningEx.jpg">
                                </td>
                                <td valign="middle" align="left" width="80%" height="100%">
                                    <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <div style="display: inline-block; padding-left: 100px">
                                        <asp:Button ID="btnEditGroup" runat="server" Text="Manage Groups" OnClientClick="window.location.href='../../../ecn.communicator.mvc/Group'; return false;" />
                                    </div>
                                    <div style="display: inline-block; padding-left: 12px">
                                        <asp:Button runat="server" ID="btnContinueUDFCheck" Text="Continue" OnClick="SaveNoUdfCheck" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


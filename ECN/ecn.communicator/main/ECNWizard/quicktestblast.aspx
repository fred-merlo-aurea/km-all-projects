<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="quicktestblast.aspx.cs" Inherits="ecn.communicator.main.ECNWizard.quicktestblast" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Src="../../includes/uploader.ascx" TagPrefix="ecn" TagName="uploader" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer" TagPrefix="ecn" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />
<script type="text/javascript" src="/ecn.communicator/scripts/jquery.qtip-1.0.0-rc3.min.js"></script>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {

        doMouseOver();
    });
    function doMouseOver() {
        $('.lblInfo').each(function () {
            if ($(this).attr("mouseover") != null) {
                $(this).qtip(
                {
                    content: $(this).attr("mouseover"),
                    show: {
                        when: { event: 'mouseover' },
                        ready: false
                    },
                    style: {
                        name: 'blue',
                        tip: 'topLeft'
                    }
                });
            }
        });
    }
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

        //if (AlreadyDone.indexOf('<%= txtEmailSubject.ClientID %>') < 0) {
            try {
                initialString = initialString.replace(/'/g, "\\'");
                initialString = initialString.replace(/\r?\n|\r/g, ' ');
                initialString = twemoji.parse(eval("\'" + initialString + "\'"));
                $('#<%= txtEmailSubject.ClientID %>').twemojiPicker({ init: initialString, height: '30px', size: "16px", hideEmoji: false });
            }
            catch (err) {
                $('#<%= txtEmailSubject.ClientID %>').twemojiPicker({ height: '30px', size: "16px", hideEmoji: false });
            }
            //AlreadyDone.push('<%=txtEmailSubject.ClientID %>');
        //}
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
.twemoji-wrap{
	top:20px;
}
.outer-container{
    top:-20px !important;
}
fieldset {
    margin: 0.5em 0px;
    padding: 0.0px 0.5em 0px 0.5em;
    border: 1px solid #ccc;
    -webkit-border-radius: 8px;
    -moz-border-radius: 8px;
    border-radius: 8px;
}

    fieldset p {
        margin: 2px 12px 10px 10px;
    }

    fieldset.login label, fieldset.register label, fieldset.changePassword label {
        display: block;
    }

    fieldset label.inline {
        display: inline;
    }

legend {
    font-size: 18px;
    font-weight: 600;
    padding: 2px 4px 8px 4px;
    font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    color: #000;
}

.itemleft {
    width: 180px;
    text-align: right;
    padding-right: 20px;
    height: 14px;
}
.itemright {
    width: 400px;
}
.inputsSize {
    width: 380px;
    margin-top: 5px;
}
.outer-container {
    max-width: 380px;
}
.inner-container {
    width: 380px;
}
.twemoji-icon-picker img {
    right: 5%;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="update1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
                    </tr>
                    <tr>
                        <td id="errorMiddle">
                            <table style="height:67px; width:80%" >
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
            <asp:Label ID="msglabel" runat="Server" CssClass="errormsg" Visible="false" />
            <br />
            <table id="layoutWrapper" cellpadding="0" cellspacing="0" width="100%" border='0'>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Campaign Item
                                        </td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr ID="rbCampaignChoiceTr" runat="server" style="height:40px;">
                                    <td class="formLabel itemleft">
                                        Select One of the Campaign Options
                                    </td>
                                    <td  class="formLabel" style="padding-bottom: 6px;">
                                        <asp:RadioButton ID="rbCampaignChoice1" runat="server" Text="Create New Campaign" GroupName="CampaignChoice" AutoPostBack="true" OnCheckedChanged="CampaignChoice_CheckedChanged"/>
                                        <asp:RadioButton ID="rbCampaignChoice2" runat="server" Text="Use Existing Campaign" GroupName="CampaignChoice" AutoPostBack="true" OnCheckedChanged="CampaignChoice_CheckedChanged" Checked="True"/>
                                    </td>
                                </tr>
                                <tr ID="ddlCampaignsTr" runat="server">
                                    <td class="formLabel itemleft">Select Campaign <label class="lblInfo" mouseover="The category that the email campaign item is a part of.">
                                        <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>
                                    </td>
                                    <td><asp:DropDownList ID="ddlCampaigns" class="inputsSize" style="width: 384px;" runat="server" /></td>
                                </tr>
                                <tr ID="txtCampaignNameTr" runat="server">
                                    <td class="formLabel itemleft">Campaign Name <label class="lblInfo" mouseover="The category that the email campaign item is a part of.">
                                        <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCampaignName" class="inputsSize" runat="server" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr ID="txtCampaignItemNameTr" runat="server">
                                    <td class="formLabel itemleft">Campaign Item Name<label class="lblInfo" mouseover="Title of the specific email campaign.">
                                        <img alt="info" src="/ecn.images/images/InfoIcon.png" /></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCampaignItemName" class="inputsSize" runat="server" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr ID="lblCampaignNameTr" runat="server">
                                    <td class="formLabel itemleft">Campaign</td>
                                    <td class="formLabel itemright">
                                        <asp:Label ID="lblCampaignName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr ID="lblCampaignItemNameTr" runat="server">
                                    <td class="formLabel itemleft">Campaign Item</td>
                                    <td class="formLabel itemright">
                                        <asp:Label ID="lblCampaignItemName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Group</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr class="formLabel" style="height:40px;">
                                    <td>
                                        <asp:RadioButton ID="rbGroupChoice1" runat="server" Text="Select Group" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                                        <asp:RadioButton ID="rbGroupChoice2" runat="server" Text="Adhoc Emails" GroupName="GroupChoice" AutoPostBack="true" OnCheckedChanged="GroupChoice_CheckedChanged"/>
                                    </td>
                                </tr>
                                <tr ID="SelectGroupTr" runat="server">
                                    <td class="formLabel">
                                        <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
                                        <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="11px"></asp:Label>
                                        <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                        <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr ID="AdhocEmailsTr" runat="server">
                                    <td>
                                        <table style="width:100%;">
                                            <tr>
                                                <td class="formLabel itemleft"><asp:Label ID="tbGroupNameLabel" runat="server" Text="Group Name"></asp:Label></td>
                                                <td><asp:TextBox ID="tbGroupName" class="inputsSize" runat="server" MaxLength="50"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td class="formLabel itemleft">Addresses</td>
                                                <td><asp:TextBox id="taAddresses" class="inputsSize" style="padding:1px; font-family: Arial; font-size: 11px;" TextMode="multiline" Columns="50" Rows="5" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Message</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px;">
                                <tr>
                                    <td class="formLabel">
                                        <asp:ImageButton ID="imgSelectLayoutA" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutTrigger_Click" Visible="true" />
                                        <asp:Label ID="lblSelectedLayoutTrigger" runat="server" Text="-No Message Selected-" Font-Size="11px" />
                                        <asp:HiddenField ID="hfSelectedLayoutTrigger" runat="server" Value="" />
                                        <asp:HiddenField ID="hfWhichLayout" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Envelope Information</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px; width: 100%;">
                                <tr>
                                    <td class="formLabel itemleft">From Email </td>
                                    <td nowrap="nowrap" style="width:50%;"  align="left">
                                        <asp:TextBox class="formfield" ID="txtEmailFrom" runat="Server" Width="380px" Columns="40" ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList
                                            class="formfield" ID="drpEmailFrom" runat="Server" Visible="false" AutoPostBack="true"
                                            OnSelectedIndexChanged="drpEmailFrom_OnSelectedIndexChanged" Width="380px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="formValidation"  ID="val_txtEmailFrom"
                                            runat="Server" ControlToValidate="txtEmailFrom" InitialValue="" ErrorMessage="From Email is a required field."
                                            CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                        <asp:Button ID="btnChangeEnvelope" runat="server" Text="Change" OnClick="btnChangeEnvelope_onclick"
                                            Style="font-size: 12px; height: 19px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel itemleft">Reply To </td>
                                    <td nowrap="nowrap" align="left">
                                        <asp:TextBox class="formfield" ID="txtReplyTo" Width="380px" onfocus="replyTo_focus()" runat="Server"
                                            Columns="40" ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList class="formfield"
                                                ID="drpReplyTo" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpReplyTo_OnSelectedIndexChanged"
                                                Width="380px">
                                            </asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="formValidation" ID="val_txtReplyTo"
                                            runat="Server" ControlToValidate="txtReplyTo" InitialValue="" ErrorMessage="ReplyTo is a required field."
                                            CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="formLabel itemleft">From Name </td>
                                    <td nowrap="nowrap" align="left">
                                        <asp:TextBox class="formfield" ID="txtEmailFromName" Width="380px" runat="Server" Columns="40"
                                            ValidationGroup="formValidation"></asp:TextBox><asp:DropDownList class="formfield"
                                                ID="drpEmailFromName" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpEmailFromName_OnSelectedIndexChanged"
                                                Width="380px">
                                            </asp:DropDownList>
                                        <asp:RequiredFieldValidator ValidationGroup="formValidation" ID="val_txtEmailFromName"
                                            runat="Server" ControlToValidate="txtEmailFromName" InitialValue="" ErrorMessage="Email From Name is a required field."
                                            CssClass="errormsg" Display="dynamic">&laquo;&laquo Required</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="height:60px;">
                                    <td class="formLabel itemleft" style="padding-bottom: 15px;">Subject </td>
                                    <td nowrap="nowrap" align="left" style="margin-left:0px;">
                                    
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
                <tr>
                    <td>
                        <fieldset>
                            <legend>
                                <table>
                                    <tr>
                                        <td>Other</td>
                                    </tr>
                                </table>
                            </legend>
                            <table style="padding-left:30px; margin-bottom:15px; width: 100%;">
                                <tr>
                                    <td class="formLabel itemleft">Also send Text version? </td>
                                    <td class="formLabel" style="padding-bottom: 7px;">
                                        <asp:RadioButton ID="rbTextVersionYes" runat="server" Text="Yes" GroupName="TextVersionChoice" />
                                        <asp:RadioButton ID="rbTextVersionNo" runat="server" Text="No" GroupName="TextVersionChoice"/>
                                    
                                    </td>
                                </tr>
                                <tr runat="server" id="trEmailPreview">
                                    <td class="formLabel itemleft"></td>
                                    <td class="formLabel">
                                        <asp:CheckBox ID="chbEmailPreview" runat="server" Text="Email Preview" /> <span style="color:red">An additional charge will be incurred for doing this</span>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel itemleft"></td>
                                    <td class="formLabel">
                                        <asp:CheckBox ID="chbEnableCacheBuster" runat="server" Text="Enable Cache Buster" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formLabel itemleft">
                                        Schedule Type = 
                                    </td>
                                    <td class="formLabel">
                                        Send Now
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>                    
                </tr>
                <tr style="text-align:center; height:40px;">
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btnSubmitQTB_onclick" />
                    </td>
                </tr>
            </table>
            <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
            <asp:HiddenField ID="hfLayoutExplorer" Value="0" runat="server" />
            <ajaxToolkit:ModalPopupExtender ID="mpeLayoutExplorer" PopupControlID="pnlLayoutExplorer" BackgroundCssClass="modalBackground" TargetControlID="hfLayoutExplorer" runat="server" />
            <asp:UpdatePanel ID="pnlLayoutExplorer" Style="display:none;" CssClass="modalPopupLayoutExplorer" UpdateMode="Always" ChildrenAsTriggers="true" runat="server">
                <ContentTemplate>
                    <table style="background-color: white;">
                        <tr>
                            <td>
                                <ecn:layoutExplorer ID="layoutExplorer" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnCloseLayoutExplorer" runat="server" OnClick="btnCloseLayoutExplorer_Click" Text="Close" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>        
    </asp:UpdatePanel>    
</asp:Content>

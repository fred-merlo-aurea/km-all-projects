<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WizardContent_AB.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.Controls.WizardContent_AB" %>
<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer" TagPrefix="ecn" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

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
            getobj('<%=txtReplyToA.ClientID%>').value = getobj('<%=txtEmailFromA.ClientID%>').value;
        }
        catch (err) { }
    }

    function replyTo_focusB() {
        try {
            getobj('<%=txtReplyToB.ClientID%>').value = getobj('<%=txtEmailFromB.ClientID%>').value;
        }
        catch (err) { }
    }


</script>

        <script type="text/javascript">
            var AlreadyDone = new Array();

            if (window.attachEvent) { window.attachEvent('onload', pageloaded); }
            else if (window.addEventListener) { window.addEventListener('load', pageloaded, false); }
            else { document.addEventListener('load', pageloaded, false); }
            function pageloaded() {

                var initialStringA = $('#<%= txtSubjectA.ClientID %>').val();
                if (AlreadyDone.indexOf('<%= txtSubjectA.ClientID %>') < 0) {
                    try {
                        initialStringA = initialStringA.replace(/'/g, "\\'");
                        initialStringA = initialStringA.replace(/\r?\n|\r/g, ' ');
                        initialStringA = twemoji.parse(eval("\'" + initialStringA + "\'"));
                        $('#<%= txtSubjectA.ClientID %>').twemojiPicker({ init: initialStringA, height: '30px', size: "16px" });
                    }
                    catch (err) {
                        $('#<%= txtSubjectA.ClientID %>').twemojiPicker({ height: '30px', size: "16px" });
                    }

                    AlreadyDone.push('<%=txtSubjectA.ClientID %>');
                }

                var initialStringB = $('#<%= txtSubjectB.ClientID %>').val();

                if (AlreadyDone.indexOf('<%= txtSubjectB.ClientID %>') < 0) {
                    try {
                        initialStringB = initialStringB.replace(/'/g, "\\'");
                        initialStringB = initialStringB.replace(/\r?\n|\r/g, ' ');
                        initialStringB = twemoji.parse(eval("\'" + initialStringB + "\'"));
                        $('#<%= txtSubjectB.ClientID %>').twemojiPicker({ init: initialStringB, height: '30px', size: "16px" });
                    }
                    catch (err) {
                        $('#<%= txtSubjectB.ClientID %>').twemojiPicker({ height: '30px', size: "16px" });
                    }
                    AlreadyDone.push('<%=txtSubjectB.ClientID %>');
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



<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        z-index: 10001 !important;
    }

    .modalPopupLayoutExplorer {
        background-color: white;
        border-width: 3px;
        border-style: solid;
        border-color: white;
        padding: 3px;
        overflow: auto;
        z-index: 1000;
    }
    .twemoji-icon-picker img {
    position: absolute;
    top:0px !important;
    right: 10%;
}
    .twemoji-error{color:red;position:absolute;top:30px !important;}
</style>
<asp:UpdateProgress ID="UpdateProgress2" runat="server" Visible="true"
    AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
    <ProgressTemplate>
        <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
            <asp:Panel ID="Panel4" CssClass="loader" runat="server">
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

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">

    <ContentTemplate>
        
        <table width="100%" style="padding-left: 20px; padding-right: 20px;">
            <tr valign="top">
                <td style="width: 50%;">
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>Message A
                                    </td>
                                </tr>
                            </table>
                        </legend>

                        <table>
                            <tr style="height:40px;">
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="width:20%;vertical-align:middle;text-align:right;padding-bottom:10px;">Message
                                </td>
                                <td style="width:80%;text-align:left;padding-bottom:10px;">
                                    <asp:ImageButton ID="imgSelectLayoutA" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutA_Click" Visible="true" />
                                    <asp:Label ID="lblSelectedLayoutA" runat="server" />
                                    <asp:HiddenField ID="hfSelectedLayoutA" runat="server" Value="" />

                                </td>

                            </tr>
                            <tr style="height:40px;">
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:140px;padding-bottom:20px;" >Email Subject&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:HiddenField ID="txtSubjectA" runat="Server" ></asp:HiddenField>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;">From Email&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" ID="txtEmailFromA" Width="264px" runat="Server" Columns="40" ValidationGroup="formValidation"></asp:TextBox>
                                    <asp:DropDownList
                                        class="formfield" ID="ddlFromEmailA" runat="Server" Visible="false" AutoPostBack="true"
                                        OnSelectedIndexChanged="drpEmailFrom_OnSelectedIndexChanged" Width="264px">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnChangeEnvelopeA" runat="server" Text="Change" OnClick="btnChangeEnvelope_onclick"
                                        Style="font-size: 9px; height: 19px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;">Reply To&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" ID="txtReplyToA" Width="264px" onfocus="replyTo_focus()" runat="Server"
                                        Columns="40" ValidationGroup="formValidation"></asp:TextBox>
                                    <asp:DropDownList class="formfield"
                                        ID="ddlReplyToA" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpReplyTo_OnSelectedIndexChanged"
                                        Width="264px">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;" >From Name&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" Width="264px" ID="txtFromNameA" runat="Server" Columns="40"
                                        ValidationGroup="formValidation"></asp:TextBox>
                                    <asp:DropDownList class="formfield"
                                        ID="ddlFromNameA" runat="Server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpEmailFromName_OnSelectedIndexChanged"
                                        Width="264px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <fieldset>
                        <legend>
                            <table>
                                <tr>
                                    <td>Message B
                                    </td>
                                </tr>
                            </table>
                        </legend>
                        <table>
                             <tr style="height:40px;">
                                <td colspan="2" style="text-align:center;">
                                    <asp:Button ID="btnPullFromA" runat="server" OnClick="btnPullFromA_Click" Text="Pull from Message A" />

                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="width:20%;vertical-align:middle;text-align:right;padding-bottom:10px;">
                                    Message
                                </td>
                                <td align="left" style="width:80%;padding-bottom:10px;">
                                    <asp:ImageButton ID="imgSelectLayoutB" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutB_Click" Visible="true" />
                                    <asp:Label ID="lblSelectedLayoutB" runat="server" />
                                    <asp:HiddenField ID="hfSelectedLayoutB" runat="server" Value="" />

                                </td>
                            </tr>
                            <tr style="height:40px;">
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:140px;padding-bottom:20px;" >Email Subject&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:HiddenField ID="txtSubjectB" runat="Server" ></asp:HiddenField>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;">From Email&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" ID="txtEmailFromB" runat="Server" Columns="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;" >Reply To&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" ID="txtReplyToB" onfocus="replyTo_focusB()" runat="Server"
                                        Columns="40" ></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td class="formLabel" style="vertical-align:middle;text-align:right;width:120px;" >From Name&nbsp;
                                </td>
                                <td nowrap="nowrap" style="text-align:left;">
                                    <asp:TextBox class="formfield" ID="txtFromNameB" runat="Server" Columns="40"
                                        ></asp:TextBox>
                                    
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                </tr>
                <tr>
                    <td class="formLabel" colspan="2" style="vertical-align:middle;text-align:center;">Winner determined by:

                        <asp:DropDownList ID="ddlAbWinnerType" runat="server">
                            <asp:ListItem Text="Click Percentage" Value="clicks"></asp:ListItem>
                            <asp:ListItem Text="Open Percentage" Value="opens"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                   
                </tr>
        </table>

        
        <asp:HiddenField ID="hfWhichLayout" Value="" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>

        <asp:Button ID="hfLayoutExplorer" style="display:none;" runat="server" />
        <ajaxToolkit:ModalPopupExtender ID="mpeLayoutExplorer" PopupControlID="pnlLayoutExplorer" BackgroundCssClass="modalBackground"
            TargetControlID="hfLayoutExplorer" runat="server" />
        <asp:UpdatePanel ID="pnlLayoutExplorer" CssClass="modalPopupLayoutExplorer" UpdateMode="Conditional" runat="server">

                <ContentTemplate>
                    <table style="background-color:white;">
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





<asp:Button ID="btnShowPopup5" runat="server" Style="display: none"/>
<ajaxToolkit:ModalPopupExtender ID="modalPopupUdfChoice" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlUdfChoice" TargetControlID="btnShowPopup5">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel runat="server" ID="pnlUdfChoice" style="display:none;" CssClass="modalPopup" Width="600">
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
    <asp:UpdatePanel ID="UpdatePanel6"  runat="server">
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
                                        <asp:Button runat="server" Text="Continue" OnClick="SaveNoUdfCheck"/>    
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

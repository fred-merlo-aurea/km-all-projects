<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialShare.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.SocialShare" %>
<%@ Register TagPrefix="ecn" TagName="socialconfig" Src="~/main/ecnwizard/othercontrols/SocialConfig.ascx" %>
<%@ Register TagPrefix="ecn" TagName="gallery" Src="~/main/ecnwizard/othercontrols/ImageSelector.ascx" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />
<script>
    window.fbAsyncInit = function () {
        FB.init({
            appId: '1386653811637103',
            xfbml: true,
            version: 'v2.3'
        });
        FB.getLoginStatus(handleSessionResponse);
    };

    function handleSessionResponse(response) {
        if (!response.session) {
            return;
        }

    }
    function fbLogoutUser() {

        FB.logout(handleSessionResponse);

    }

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

    function setImgBtnURL(URL, imgToSet, hfToSet, btnToSet) {

        var imgbtn = document.getElementById(imgToSet);
        if (imgbtn) {
            imgbtn.setAttribute('src', URL);
            imgbtn.setAttribute('imagepath', URL);
        }


        var hf = document.getElementById(hfToSet);
        if (hf) {
            hf.value = URL;
        }

        var btnAssign = document.getElementById(btnToSet);
        if (btnAssign) {
            btnAssign.click();
        }
    }

    function assignTwemoji(clientID, lengthMax, hideEmoji) {
        
        var initialString = $('#' + clientID).val();
        try {
            initialString = initialString.replace(/'/g, "\\'");
            initialString = initialString.replace(/\r?\n|\r/g, ' ');
            initialString = twemoji.parse(eval("\'" + initialString + "\'"));

            //var regSplit = new RegExp("(<img.*?\/?>)");
            //var imgSplit = new Array();
            //imgSplit = initialString.split(regSplit);
            //var totalLength = 0;
            //var finalString = "";
            //for (var i = 0; i < imgSplit.length ; i++)
            //{
            //    var current = imgSplit[i];
            //    if(current.indexOf('<img') >= 0)
            //    {
            //        var alt = $(current).attr("alt");
            //        current = alt.split('-').map(string_as_unicode_escape).join('');

            //    }

            //    if ((totalLength + current.length) <= lengthMax) {
            //        finalString += current;
            //    }
            //    else
            //        break;

            //    totalLength += current.length;

            //}
            //initialString = twemoji.parse(eval("\"" + finalString + "\""));

            $('#' + clientID).twemojiPicker({ init: initialString, height: '30px', size: "16px", maxLength: lengthMax, hideEmoji: hideEmoji });
        }
        catch (err) {
            $('#' + clientID).twemojiPicker({ height: '30px', size: "16px", maxLength: lengthMax, hideEmoji: hideEmoji });
        }
        
    }

    function ShowProgress(upProgressID)
    {
        var updateProgress = $('#' + upProgressID);
        updateProgress.css('display', 'block');

    }

    function HideProgress(upProgressID)
    {
        var updateProgress = $('#' + upProgressID);
        updateProgress.css('display','none');
    }

    function string_as_unicode_escape(input) {
        function pad_four(input) {
            var l = input.length;
            if (l == 0) return '0000';
            if (l == 1) return '000' + input;
            if (l == 2) return '00' + input;
            if (l == 3) return '0' + input;
            return input;
        }
        var output = '';
        for (var i = 0, l = input.length; i < l; i++)
            output += '\\u' + pad_four(input.charCodeAt(i).toString(16));
        return output;
    }



</script>
<asp:UpdateProgress ID="UpdateProgress2" runat="server" Visible="true"
    AssociatedUpdatePanelID="upSocialShare" DynamicLayout="true">
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

<asp:UpdatePanel ID="upSocialShare" UpdateMode="Conditional" runat="server">
    <ContentTemplate>


        <div class="section bottomDiv" style="padding-left: 30px; padding-right: 30px">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%;">
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
                                                    <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label></td>
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
                <tr style="vertical-align: top;">
                    <td style="width: 70%;">
                        <fieldset>
                            <legend>Social Share
               
                            </legend>

                            <table style="width: 70%;">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkSimpleShare" runat="server" AutoPostBack="true" OnCheckedChanged="chkSimpleShare_CheckedChanged" Text="Enable Simple Share" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddSocialNetwork" runat="server" OnClick="btnAddSocialNetwork_Click" Text="Add Social Network" class="formbuttonsmall" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gvSimpleShare" Width="100%" Visible="false" GridLines="None" runat="server" DataKeyNames="SocialMediaAuthID" OnRowCommand="gvSimpleShare_RowCommand" OnRowDataBound="gvSimpleShare_RowDataBound" AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkEnableSimpleShare" runat="server" Enabled="false" OnCheckedChanged="chkEnableSimpleShare_CheckedChanged" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSocialNetwork" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccountName" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="45%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletesocialmedia" />
                                                        </td> </tr>
                                                        <asp:UpdateProgress ID="UpdateProgress2" runat="server" Visible="true"
                                                            AssociatedUpdatePanelID="upSocialConfig" DynamicLayout="true">
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
                                                        <asp:UpdatePanel ID="upSocialConfig" Visible="false" UpdateMode="Conditional" runat="server">
                                                            <ContentTemplate>
                                                                <tr style="vertical-align: top; padding-top: 0px;">
                                                                    <td colspan="4" style="text-align: left; padding-top: 0px;">
                                                                        <table style="width: 100%; padding-top: 0px;">
                                                                            <tr>
                                                                                <td style="padding-top: 0px;">
                                                                                    <ecn:socialconfig ID="scConfig" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkSubShare" runat="server" Text="Enable Subscriber Share" OnCheckedChanged="chkSubShare_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlSubShare" runat="server" Visible="false">
                                            <table style="padding-left: 15px;">
                                                <tr style="height: 70px;">

                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Image ID="imgFacebook" runat="server" ImageUrl="/ecn.images/KMNew/facebook.png" />
                                                                </td>
                                                                <td>

                                                                    <asp:CheckBox ID="chkFacebookSubShare" OnCheckedChanged="chkFacebookSubShare_CheckedChanged" runat="server" Text="Facebook" AutoPostBack="true" />

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <table id="tblFBMeta" visible="false" runat="server">
                                                                        <tr>
                                                                            <td>Title
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFBTitleMeta" runat="server" MaxLength="100" />
                                                                                <asp:HiddenField ID="hfFBTitleMetaID" runat="server" Value="-1" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Subtitle
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFBDescMeta" runat="server" MaxLength="250" />
                                                                                <asp:HiddenField ID="hfFBDescMetaID" runat="server" Value="-1" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Image
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnFBImageMeta" value="" Style="max-width: 200px; max-height: 200px;" runat="server" ImageUrl="/ecn.images/images/SelectImage.png" OnClick="imgbtnMetaImage_Click" />
                                                                                <asp:HiddenField ID="hfFBImageMetaID" Value="-1" runat="server" />
                                                                                <asp:HiddenField ID="hfFBImagePath" Value="" runat="server" />
                                                                                <asp:Button ID="btnFBImage" runat="server" Text="" OnClick="btnFBImage_Click" BackColor="Transparent" BorderStyle="None" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>
                                                <tr style="height: 70px;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Image ID="imgTwitterSubShare" runat="server" ImageUrl="/ecn.images/KMNew/twitter.png" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkTwitterSubShare" runat="server" Text="Twitter" AutoPostBack="true" OnCheckedChanged="chkTwitterSubShare_CheckedChanged" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>

                                                                    <table id="tblTWMeta" visible="false" runat="server">
                                                                        <tr>
                                                                            <td>Hashtags
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTWHashMeta" MaxLength="118" runat="server" />
                                                                                <asp:HiddenField ID="hfTWHashMeta" Value="-1" runat="server" />
                                                                                <br />
                                                                                Hashtags must be separated by a comma
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="height: 70px;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Image ID="imgLinkedInSubShare" runat="server" ImageUrl="/ecn.images/KMNew/linkedin.png" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkLinkedInSubShare" OnCheckedChanged="chkLinkedInSubShare_CheckedChanged" runat="server" Text="LinkedIn" AutoPostBack="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <table id="tblLIMeta" visible="false" runat="server">
                                                                        <tr>
                                                                            <td>Title
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtLITitleMeta" MaxLength="200" runat="server" />
                                                                                <asp:HiddenField ID="hfLITitleMetaID" Value="-1" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>Subtitle
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtLIDescMeta" MaxLength="200" runat="server" />
                                                                                <asp:HiddenField ID="hfLIDescMetaID" Value="-1" runat="server" />
                                                                            </td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td>Image
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="imgbtnLIImageMeta" value="" Style="max-width: 200px; max-height: 200px;" runat="server" ImageUrl="/ecn.images/images/SelectImage.png" OnClick="imgbtnMetaImage_Click" />
                                                                                <asp:HiddenField ID="hfLIImageMetaID" runat="server" Value="-1" />
                                                                                <asp:HiddenField ID="hfLIImagePath" runat="server" Value="" />
                                                                                <asp:Button ID="btnLIImage" runat="server" Text="" OnClick="btnLIImage_Click" BackColor="Transparent" BorderStyle="None" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>
                                                <tr style="height: 70px;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Image ID="imgF2FSubShare" runat="server" ImageUrl="/ecn.images/KMNew/forward.png" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkF2FSubShare" runat="server" Text="Forward to a friend" AutoPostBack="false" />
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>
                                                <tr style="height: 70px;">
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td align="center" valign="top">
                                                                    <asp:Image ID="imgFacebookLike" runat="server" ImageUrl="/ecn.images/KMNew/facebooklike.png" />
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkFacebookLikeSubShare" runat="server" Text="Facebook Like" AutoPostBack="true" OnCheckedChanged="chkFacebookLikeSubShare_CheckedChanged" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlFacebookLikeSubShare" Visible="false" OnSelectedIndexChanged="ddlFacebookLikeSubShare_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>

                                                                            <td>
                                                                                <asp:DropDownList ID="ddlFacebookUserAccounts" Visible="false" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>

                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="hfSimpleShare" runat="server" style="display:none;" />
        <ajaxToolkit:ModalPopupExtender ID="mpeSimpleShare" PopupControlID="upSimpleShare" BackgroundCssClass="modalBackground" CancelControlID="btnCloseSimpleShare" TargetControlID="hfSimpleShare" runat="server" />
        <asp:UpdatePanel ID="upSimpleShare" UpdateMode="Conditional" Style="display: none;" runat="server">
            <ContentTemplate>
                <table style="height: 300px; width: 500px; background-color: white;">
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:Panel ID="pnlStartSimple" Height="100%" Width="100%" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblHeader" runat="server" Text="Simple Share" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblWarning" Text="In order to add a network, we need to save your current selections.  If there are any issues, you will be notified." ForeColor="Red" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblSubHeader" runat="server" Text="Add Social Networks" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center; width: 33%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="imgbtnTwitterSimple" runat="server" OnClick="imgbtnTwitterSimple_Click" ImageUrl="/ecn.images/KMNew/twitter.png" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTwitterSimple" runat="server" Text="Twitter" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>

                                        <td style="text-align: center; width: 33%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="imgbtnLinkedInSimple" OnClick="imgbtnLinkedInSimple_Click" runat="server" ImageUrl="/ecn.images/KMNew/linkedin.png" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLinkedInSimple" runat="server" Text="LinkedIn" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="text-align: center; width: 34%;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="imgbtnFacebookSimple" OnClick="imgbtnFacebookSimple_Click" OnClientClick="fbLogoutUser();" runat="server" ImageUrl="/ecn.images/KMNew/facebook_inactive.png" Enabled="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFacebookSimple" runat="server" Text="Facebook(Coming Soon)" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="text-align: center;">
                                            <asp:Button ID="btnCloseSimpleShare" runat="server" CausesValidation="false" UseSubmitBehavior="true" Text="Close" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <b>You must connect your personal account</b> in order to connect to Facebook or LinkedIn.  That's because your Company pages are controlled by your personal account.
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>


                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="hfDeleteProfile" runat="server" style="display:none;" />
        <ajaxToolkit:ModalPopupExtender ID="mpeDeleteProfile" PopupControlID="upDeleteProfile" BackgroundCssClass="modalBackground" TargetControlID="hfDeleteProfile" runat="server" />
        <asp:UpdatePanel ID="upDeleteProfile" ChildrenAsTriggers="true" Style="display:none;" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnDeleteProfile" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlDelete" runat="server">

                    <table style="margin: 5px; background-color: white;">
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:Label ID="lblDeleteMessage" runat="server" Text="Are you sure you want to delete this profile?" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnDeleteProfile" runat="server" Text="Yes" UseSubmitBehavior="true" CausesValidation="false" OnClick="btnDeleteProfile_Click" />
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btnCancelDelete" Text="No" OnClick="btnCancelDelete_Click" runat="server" />
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                <asp:Panel ID="pnlPermission" runat="server">
                    <table style="margin: 5px; background-color: white;">
                        <tr>
                            <td style="text-align: center;">
                                <asp:Label ID="lblPermission" Text="You must have administrative privileges to perform this action" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnOkayPermission" Text="Okay" runat="server" OnClick="btnOkayPermission_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:Button ID="hfImageSelect" runat="server" style="display:none;" />
        <ajaxToolkit:ModalPopupExtender ID="mpeImageSelect" PopupControlID="upImageSelect" BackgroundCssClass="modalBackground" TargetControlID="hfImageSelect" runat="server" />
        <asp:UpdatePanel ID="upImageSelect" UpdateMode="Conditional" Style="display: none;" runat="server">
            <ContentTemplate>
                <table style="background-color: white;">
                    <tr>
                        <td>
                            <ecn:gallery ID="ecnImageSelector" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnCloseImageSelector" runat="server" CausesValidation="false" UseSubmitBehavior="true" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

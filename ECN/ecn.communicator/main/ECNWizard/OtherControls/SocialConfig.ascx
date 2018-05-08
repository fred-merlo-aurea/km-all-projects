<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SocialConfig.ascx.cs" Inherits="ecn.communicator.main.ECNWizard.OtherControls.SocialConfig" %>
<%@ Register TagPrefix="ecn" TagName="gallery" Src="~/main/ecnwizard/othercontrols/ImageSelector.ascx" %>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />
<script type="text/javascript">

    //function setImgBtnURL(URL, imgToSet,hfToSet, btnToSet) {

    //    var imgbtn = document.getElementById(imgToSet);
    //    if (imgbtn) {
    //        imgbtn.setAttribute('src', URL);
    //        imgbtn.setAttribute('imagepath', URL);
    //    }

    //    var hf = document.getElementById(hfToSet);
    //    if (hf)
    //    {
    //        hf.value = URL;
    //    }

    //    var btnAssign = document.getElementById(btnToSet);
    //    btnAssign.click();

    //}


    function pageloadedInsideConfig(lengthMax) {

        try
        {
            var initialString = $('#<%= txtPostLink.ClientID %>').val();

            try {
                initialString = initialString.replace(/'/g, "\\'");
                initialString = initialString.replace(/\r?\n|\r/g, ' ');
                initialString = twemoji.parse(eval("\'" + initialString + "\'"));
                $('#<%= txtPostLink.ClientID %>').twemojiPicker({ init: initialString, height: '30px', size: "16px",maxLength:lengthMax });
            }
            catch (err1) {
                $('#<%= txtPostLink.ClientID %>').twemojiPicker({ height: '30px', size: "16px",maxLength:lengthMax });
            }
        }
        catch(err2)
        {

        }

        try
        {
            var initialStringSub = $('#<%= txtPostSubTitle.ClientID %>').val();

            try {
                initialStringSub = initialStringSub.replace(/'/g, "\\'");
                initialStringSub = initialStringSub.replace(/\r?\n|\r/g, ' ');
                initialStringSub = twemoji.parse(eval("\'" + initialStringSub + "\'"));
                $('#<%= txtPostSubTitle.ClientID %>').twemojiPicker({ init: initialStringSub, height: '30px', size: "16px",maxLength:lengthMax });
            }
            catch (err3) {
                $('#<%= txtPostSubTitle.ClientID %>').twemojiPicker({ height: '30px', size: "16px",maxLength:lengthMax });
            }
        }
        catch(err4)
        {

        }

        try{
            var initialStringMessage = $('#<%= txtMessage.ClientID %>').val();

            try {
                initialStringMessage = initialStringMessage.replace(/'/g, "\\'");
                initialStringMessage = initialStringMessage.replace(/\r?\n|\r/g, ' ');
                initialStringMessage = twemoji.parse(eval("\'" + initialStringMessage + "\'"));
                $('#<%= txtMessage.ClientID %>').twemojiPicker({ init: initialStringMessage, height: '30px', size: "16px", maxLength: lengthMax });
            }
            catch (err5) {
                $('#<%= txtMessage.ClientID %>').twemojiPicker({ height: '30px', size: "16px", maxLength: lengthMax });
            }
            
        }catch(err6)
        {

        }
    }
</script>
<style>
   .twemoji-icon-picker img {
        position: absolute;
        top: -5px;
        right: -30px;
    }
	
	.outer-container{
        top:-10px !important;
		
    }

    .twemoji-textarea {
        height: 30px;
		margin-left:auto;
		
    }
    .twemoji-wrap{
        top:0px !important;
    }
     @media screen and (-moz-images-in-menus:0) {
            .twemoji-picker-category .close {
                position: relative;
                top: 0px !important;
            }
    }
</style>
<table style="background-color: #EEEEEE; width: 90%; padding-top: 0px;">
    <tr>
        <td style="padding-top: 0px;">
            <table id="tblHeader" style="width: 100%; padding-top: 0px;" runat="server">
                <tr>
                    <td style="max-height: 80px; max-width: 80px; width: 20%;">
                        <asp:Image ID="imgSocialMedia" Style="max-height: 80px; max-width: 80px;" runat="server" />
                    </td>
                    <td style="text-align: left; padding-top: 0px; width: 80%;">
                        <table style="padding-top: 0px;">
                            <tr>
                                <td style="text-align: left; padding-top: 0px;">
                                    <asp:Label ID="lblProfileName" Font-Bold="true" ForeColor="White" runat="server" />
                                    <asp:ImageButton ID="imgbtnReauth" runat="server" CommandName="reauth" ImageUrl="/ecn.images/images/icon-edits1.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; padding-top: 0px;">
                                    <asp:Label ID="lblSocialMedia" Font-Size="X-Small" ForeColor="White" runat="server" />
                                    <div style="background-color: red;">
                                        <asp:Label ID="AccountErrrorMsg" Font-Size="X-Small" ForeColor="White" Visible="False" runat="server" />
                                    </div>
                                    <asp:DropDownList ID="ddlAccounts" OnSelectedIndexChanged="ddlAccounts_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                </td>

                            </tr>
                        </table>

                    </td>
                </tr>

            </table>

        </td>

    </tr>
    <tr>
        <td>
            <table style="padding-top: 0px; width: 100%;">
                <tr>
                    <td style="vertical-align: top; padding-top: 0px; max-width: 85px; max-height: 85px; width: 85px;">
                        <asp:Image ID="imgProfile" Height="80px" Width="80px" runat="server" />
                    </td>
                    <td style="vertical-align: top; padding-top: 0px;">
                        <table style="padding-top: 0px; width: 100%;">
                            <tr>

                                <td style="vertical-align: top; padding-top: 0px; padding-right:100px; width: 100%;">
                                    <table style="width:75%;">
                                        <tr>
                                            <td>Comment
                                                </td>
                                            </tr>
                                        <tr style="height:30px; vertical-align:top;"><td>
                                                   <asp:HiddenField ID="txtMessage" runat="server" />
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                
                            </tr>
                            <tr>
                                <td style="vertical-align: top; width: 100%;">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:20%;">
                                                <asp:ImageButton ID="imgbtnThumbnail" runat="server" OnClick="imgbtnThumbnail_Click" Style="width: 100%; height: 100%; max-height: 80px; max-width: 80px;" value="" />
                                                <asp:HiddenField ID="hfImagePath" runat="server" Value="" />
                                                <asp:Button ID="btnAssignImageURL" runat="server" BackColor="Transparent" BorderStyle="None" Width="0" Height="0" OnClick="btnAssignImageURL_Click" />
                                            </td>
                                            <td style="width:80%;padding-right:60px;">
                                                <table style="width: 100%;">
                                                    <tr id="trTitle" runat="server">
                                                        <td style="vertical-align: top; width: 5%;">Title
                                                        </td>
                                                        <td style="vertical-align: top; padding-top: 0px; width: 95%;">

                                                            <asp:HiddenField ID="txtPostLink" runat="server" />
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr style="height:20px;"><td></td></tr>
                                                    <tr id="trSubTitle" runat="server">
                                                        <td style="vertical-align: top;">Subtitle
                                                        </td>
                                                        <td style="vertical-align: top; padding-top: 0px;">
                                                            <asp:HiddenField ID="txtPostSubTitle" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trThumbnail" runat="server">
                                                        <td style="vertical-align: top; padding-top: 0px;" colspan="2">
                                                            <asp:CheckBox ID="chkUseThumbNail" Text="Use thumbnail" runat="server" AutoPostBack="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:HiddenField ID="hfSocialMediaID" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>

                        </table>

                    </td>
                </tr>

            </table>
        </td>
    </tr>

</table>
<asp:Button ID="hfImageSelector" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="mpeImageSelector" runat="server" PopupControlID="upImageSelector" TargetControlID="hfImageSelector" CancelControlID="btnCloseImageSelector" BackgroundCssClass="modalBackground" />
<asp:UpdatePanel ID="upImageSelector" UpdateMode="Conditional" runat="server" Style="display:none;">
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

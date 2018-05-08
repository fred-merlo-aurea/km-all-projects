<%@ Page Language="c#" Trace="false" Inherits="ecn.communicator.main.ECNWizard.wizardSetup" CodeBehind="wizardSetup.aspx.cs" MasterPageFile="~/MasterPages/Communicator.Master"  %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ModalPopupBG {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        fieldset {
            margin: 0.5em 0px;
            padding: 0.0px 0.5em 0px 0.5em;
            border: 1px solid #ccc;
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
            font-size: 1.5em;
            font-weight: 600;
            padding: 2px 4px 8px 4px;
            font-family: "Helvetica Neue", "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
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

        .modalBackground {
            background-color: #000000;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .popupbody {
            /*background:#fffff url(images/blank.gif) repeat-x top;*/
            z-index: 101;
            background-color: #FFFFFF;
            font-family: calibri, trebuchet ms, myriad, tahoma, verdana;
            font-size: 12px;
        }
        /* scrollable root element */
        #wizard {
            background: #EDEDED;
            border: 3px solid #789;
            font-size: 12px;
            margin: 20px auto;
            width: 900px;
            overflow: hidden;
            position: relative;
            /* rounded corners for modern browsers */
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
        }

            /* scrollable items */
            #wizard .items {
                clear: both;
                position: relative;
            }


            /* title */
            #wizard h2 {
                border-bottom: 1px dotted #ccc;
                font-size: 22px;
                font-weight: normal;
                margin: 10px 0 0 0;
                padding-bottom: 15px;
            }

                #wizard h2 em {
                    display: block;
                    font-size: 14px;
                    color: #666;
                    font-style: normal;
                    margin-top: 5px;
                }

            #wizard legend {
                color: #4E78A0;
            }
             .inner-container {
        width: 264px !important;
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin: 10px 0; padding: 0;" align="center">
        <table width="100%" cellspacing="0" cellpadding="0" border='0'>
            <tr>
                <td valign="top" align="right">
                    <a href="default.aspx">
                        <img src="/ecn.images/images/campaignHome.gif" /></a>
                </td>
            </tr>
            <tr>
                <td valign="bottom" align="center">
                    <table id="tabsCollectionTable" bordercolor="#cccccc" cellspacing="0" cellpadding="0" align="left" border="0" runat="server">
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td class="gradient buttonPad" align='right' valign="middle" >
                    <table width="100%">
                        <tr>
                            <td width="50%">
                                <asp:Label ID="lblCampaignItemName" runat="server" Text="" Style="font-weight:bold; font-size:12pt"></asp:Label>
                            </td>
                            <td width="50%">
                                <ul class="surveyNav">
                                    <!-- items are in reverse order because they're floated right -->
                                    <li>
                                        <asp:LinkButton ID="btnNext1" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;" OnClick="btnNext_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnPrevious1" CssClass="btnbgGray" runat="Server" CausesValidation="False" Text="&laquo;&nbsp;Previous"
                                            OnClick="btnPrevious_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnSave1" CssClass="btnbgGray" runat="Server" Text="Save" OnClick="btnSave_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnCancel1" CssClass="btnbgRed" runat="Server" CausesValidation="False" Text="Cancel"
                                            OnClick="btnCancel_Click"></asp:LinkButton>
                                    </li>
                                </ul>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="greyOutSide offWhite center label">
                    <br />
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
                                                <asp:Button runat="server" Text="Manage Groups"/>    
                                            </div>
                                            <div style="display: inline-block; padding-left: 12px">
                                                <asp:Button runat="server" Text="Continue"/>    
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td class=" greyOutSide offWhite center label">
                    <asp:PlaceHolder ID="phwizContent" runat="Server"></asp:PlaceHolder>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="gradient buttonPad" valign="middle" align='right'>
                    <ul class="surveyNav">
                        <!-- items are in reverse order because they're floated right -->
                        <li>
                            <asp:LinkButton ID="btnNext2" CssClass="btnbgGreen" runat="Server" Text="Next&nbsp;&raquo;" OnClick="btnNext_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnPrevious2" CssClass="btnbgGray" runat="Server" CausesValidation="False" Text="&laquo;&nbsp;Previous"
                                OnClick="btnPrevious_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnSave2" CssClass="btnbgGray" runat="Server" Text="Save" OnClick="btnSave_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnCancel2" CssClass="btnbgRed" runat="Server" CausesValidation="False" Text="Cancel"
                                OnClick="btnCancel_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>

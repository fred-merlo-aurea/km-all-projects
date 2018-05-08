<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseChannelUnsubscribe.aspx.cs" Inherits="ecn.communicator.main.admin.landingpages.BaseChannelUnsubscribe" MasterPageFile="~/MasterPages/Communicator.Master" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function disableButton() {
            var button = document.getElementById('<%=btnPreview.ClientID %>');
            if (button.className != "ECN-Button-Medium-disable") {
                button.className = "ECN-Button-Medium-disable";
                button.disabled = true;
            }
        }
    </script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .modalPopupHtmlPreview {
            position: fixed;
            width: 75%;
            height: 50%;
            overflow: auto;
            background-color: #e6e7e8;
            border: 2px solid black;
            padding: 20px 20px 20px 0px;
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

        .styled-text-multiline {
            height: 100px;
            overflow: auto;
            border: 1px solid #ccc;
        }

        .reorderStyle {
            list-style-type: none;
            font: Verdana;
            font-size: 12px;
        }

            .reorderStyle li {
                list-style-type: none;
                padding-bottom: 1em;
            }

        .ddlStyle {
            width: 400px;
            margin: 8px 20px;
        }

        .htmlPreviewTable {
            margin-top: 50px;
            padding: 25px;
            width: 75%;
        }

        .htmlPreviewStyle {
            color: Black;
            font-size: 11pt;
            font-family: 'Segoe UI';
            text-decoration: none;
            display: block;
            padding: 8px 20px;
            margin: 0;
            font-weight: 600;
        }

        .ECN-Button-Medium-disable {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background-color: #ededed;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            border-radius: 4px;
            border: 2px solid #dcdcdc;
            display: inline-block;
            color: #aaaaaa;
            font-family: arial;
            font-size: 10px;
            font-weight: bold;
            padding: 4px 18px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #ffffff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
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

            <asp:Panel runat="server" ID="pnlNoAccess">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSettings" CssClass="label">
                <br />
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblHeading" runat="server" Text="Unsubscribe Page Settings" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>Do you want to override default settings?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblOverrideDefaultSettings" runat="server" RepeatDirection="Horizontal" onchange="disableButton()">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>Do you want to allow Customers to override Basechannel Settings?
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblAllowCustomerOverrideSettings" runat="server" RepeatDirection="Horizontal" onchange="disableButton()">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                </table>
                <table width="100%" cellspacing="7">
                    <tr valign="top">
                        <td style="width: 20%;">Page Label
                        </td>
                        <td style="width: 30%;">
                            <table>
                                <tr>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>Make Field Visible
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblVisibilityPageLabel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblVisibilityPageLabel_SelectedIndexChanged" RepeatDirection="Horizontal" onchange="disableButton()">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPageLabel" runat="server" Visible="false" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>

                        </td>
                        <td style="width: 15%;">Reason Label
                        </td>
                        <td style="width: 35%;">
                            <table style="width:100%;">
                                <tr align="left">
                                    <td>Make Field Visible
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblVisibilityReason" runat="server" onchange="disableButton()" AutoPostBack="true" OnSelectedIndexChanged="rblVisibilityReason_SelectedIndexChanged" RepeatDirection="Horizontal">
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem Selected="True">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtReasonLabel" runat="server" Visible="false" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <table id="tblReasonResponseType" visible="false"  runat="server">

                                            <tr align="left">
                                                <td>Response Type</td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblReasonControlType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblReasonControlType_SelectedIndexChanged" onchange="disableButton()">
                                                        <asp:ListItem Selected="True">Text Box</asp:ListItem>
                                                        <asp:ListItem>Drop Down</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>Master Suppression Label
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>Make Field Visible
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblVisibilityMasterSuppression" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblVisibilityMasterSuppression_SelectedIndexChanged" onchange="disableButton()" RepeatDirection="Horizontal">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMasterSuppressionLabel" Visible="false"  runat="server" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </td>
                        <td colspan="2" rowspan="5">
                            <asp:Panel ID="pnlReasonDropDown" Width="100%" Visible="false"  runat="server">
                                <table style="width: 100%;">
                                    <tr valign="top" style="text-align: left;">
                                        <td>Reason Drop Down Fields</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">

                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <ajaxToolkit:ReorderList ID="rlReasonDropDown" ShowInsertItem="false" OnItemReorder="rlReasonDropDown_ItemReorder" PostBackOnReorder="true" SortOrderField="SortOrder"
                                                            OnItemCommand="rlReasonDropDown_ItemCommand" DragHandleAlignment="Left" runat="server" Width="80%" AllowReorder="true" DataKeyField="ID" CssClass="reorderStyle">
                                                            <ReorderTemplate>
                                                                <div style="height: 40px; border: dashed 2px orange; background-color: transparent; cursor: move">
                                                                </div>
                                                            </ReorderTemplate>
                                                            <DragHandleTemplate>
                                                                <div style="height: 40px; width: 20px; border: solid 2px darkgrey; background-color: darkgrey; cursor: move">
                                                                </div>
                                                            </DragHandleTemplate>
                                                            <ItemTemplate>
                                                                <table style="background-color: transparent; border: solid 2px darkgrey; height: 40px;" width="100%" cellspacing="3" cellpadding="0">
                                                                    <tr>
                                                                        <td width="80%" align="left">
                                                                            <asp:Label ID="lblReasonID" runat="server" Text='<%# Convert.ToString(Eval("ID")) %>' Visible="false" />
                                                                            <asp:Label ID="lblReason" runat="server" Text='<%# HttpUtility.HtmlEncode(Eval("Reason")) %>' />
                                                                        </td>
                                                                        <td width="10%">
                                                                            <asp:ImageButton ID="imgbtnEdit" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditReason" CommandArgument='<%# Convert.ToString(Eval("ID")) %>' runat="server" />
                                                                        </td>
                                                                        <td width="10%">
                                                                            <asp:ImageButton ID="imgbtnDelete" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="DeleteReason" CommandArgument='<%# Convert.ToString(Eval("ID")) %>' runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </ajaxToolkit:ReorderList>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <table style="width: 80%; text-align: right;">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtNewReason" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnAddNewReason" Text="Add" OnClick="btnAddNewReason_Click" runat="server" />
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
                    <tr valign="top">
                        <td>Main Label
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td>Make Field Visible
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblVisibilityMainLabel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblVisibilityMainLabel_SelectedIndexChanged" RepeatDirection="Horizontal" onchange="disableButton()">
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtMainLabel" runat="server" Visible="false"  CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>Unsubscribe Text
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtUnsubscribeText" runat="server" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="font-weight: bold;">Thank You and Redirect
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;" colspan="2">
                            <asp:RadioButtonList ID="rblRedirectThankYou" AutoPostBack="true" OnSelectedIndexChanged="rblRedirectThankYou_SelectedIndexChanged" RepeatDirection="Vertical" runat="server">
                                <asp:ListItem Value="thankyou" Text="Show Thank You Message" Selected="True" />
                                <asp:ListItem Value="redirect" Text="Redirect to URL" />
                                <asp:ListItem Value="both" Text="Show Thank You Message and Redirect to URL" />
                                <asp:ListItem Value="neither" Text="Show Neither"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <table id="tblThankYou" runat="server" style="width: 100%;">
                                <tr>
                                    <td style="text-align: right; width: 50%;">Thank You Text
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtThankYouMessage" runat="server" TextMode="SingleLine" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>


                    </tr>
                    <tr valign="top">
                        <td colspan="2" style="text-align: right;">
                            <table id="tblRedirect" runat="server" visible="false" style="width: 100%;">
                                <tr>
                                    <td style="text-align: right; width: 50%;">Redirect URL</td>
                                    <td>
                                        <asp:TextBox ID="txtRedirectURL" runat="server" TextMode="SingleLine" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <table id="tblDelay" runat="server" visible="false" style="width: 100%;">
                                <tr>
                                    <td>Delay Before Redirect</td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlRedirectDelay" AutoPostBack="false" runat="server">
                                            <asp:ListItem Value="5" Text="5 seconds" Selected="True" />
                                            <asp:ListItem Value="10" Text="10 seconds" />
                                            <asp:ListItem Value="15" Text="15 seconds" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Header"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHeader" runat="server" TextMode="MultiLine" CssClass="styled-text-multiline" Width="100%" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Footer"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFooter" runat="server" TextMode="MultiLine" CssClass="styled-text-multiline" Width="100%" onkeyup="disableButton()"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr valign="top" align="center">
                        <td colspan="4">
                            <br />
                            <table style="width:100%;">
                                <tr>
                                    <td style="text-align:center;width:33%;">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="ECN-Button-Medium" />
                                    </td>
                                    <td style="text-align:center;width:33%;">
                                        <asp:Button ID="btnCancelEditPage" runat="server" Text="Cancel" OnClick="btnCancelEditPage_Click" class="ECN-Button-Medium" />
                                    </td>
                                    <td style="text-align:center;width:34%;">
                                        <asp:Button ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click" class="ECN-Button-Medium" />
                                    </td>
                                </tr>
                            </table>
                           
                            
                        </td>
                        
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="modalPopupHtmlPreview" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlHtmlPreview" TargetControlID="btnShowPopup">
    </cc1:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlHtmlPreview" CssClass="modalPopupHtmlPreview">
        <asp:UpdatePanel ID="upDdl" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <table align="center" class="htmlPreviewTable">
                    <tr>
                        <td colspan="1">
                            <asp:Label ID="lblDdl" runat="server" Text="Choose a customer: " Font-Size="Medium"></asp:Label>
                        </td>

                        <td colspan="1">
                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="ddlStyle" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblBaseChannelOverride" runat="server" CssClass="htmlPreviewStyle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblCustomerOverride" runat="server" CssClass="htmlPreviewStyle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblUrlWarning" runat="server" CssClass="htmlPreviewStyle"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Button ID="btnHtmlPreviewShow" Text="Preview Html" runat="server" class="ECN-Button-Medium" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Button ID="btnHtmlPreviewClose" Text="Close" runat="server" OnClick="btnHtmlPreview_Hide" Style="width: 100px" class="ECN-Button-Medium" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Button ID="hfeditReason" runat="server" style="display:none;" />
    <cc1:ModalPopupExtender ID="mpeEditReason" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlEditReason" TargetControlID="hfeditReason" />
    <asp:UpdatePanel ID="pnlEditReason" ChildrenAsTriggers="true" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnCancelEditReason" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color: white;">
                <tr>
                    <td>
                        <asp:Label ID="lblReasonLabel" runat="server" Text="Label:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtReasonLabelEdit" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvReasonEdit" ControlToValidate="txtReasonLabelEdit" runat="server" ForeColor="Red" ErrorMessage="Cannot be empty" ValidationGroup="Edit" />
                    </td>
                </tr>
                <tr>

                    <td colspan="2">
                        <table style="width:100%;">
                            <tr>
                                <td style="text-align:center;">
                                    <asp:Button ID="btnSaveReason" runat="server" OnClick="btnSaveReason_Click" ValidationGroup="Edit" CausesValidation="true" Text="Save" /></td>
                                <td style="text-align:center;">
                                    <asp:Button ID="btnCancelEditReason" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>

                    </td>

                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

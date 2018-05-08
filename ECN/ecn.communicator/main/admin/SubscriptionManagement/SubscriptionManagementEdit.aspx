<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="SubscriptionManagementEdit.aspx.cs" Inherits="ecn.communicator.main.admin.SubscriptionManagement.SubscriptionManagementEdit" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .reorderStyle {
            list-style-type: disc;
            font: Verdana;
            font-size: 12px;
        }

            .reorderStyle li {
                list-style-type: none;
                padding-bottom: 1em;
            }
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <asp:Panel ID="pnlSettings" Width="100%" runat="server">
        <table cellspacing="3" style="width: 100%; margin-top: 20px;">
            <tr>
                <td style="width: 100%;">
                    <asp:Label ID="lblHeading" Text="Subscription Management Setup" CssClass="Page_Title" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 80%; margin-top: 20px;">
                        <tr>
                            <td style="width: 20%; text-align: right;">Name:
                            </td>
                            <td style="width: 80%;">
                                <asp:TextBox ID="txtName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Page Header:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPageHeader" runat="server" Width="100%" Height="100px" TextMode="MultiLine" CssClass="formTextBox" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Page Footer:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPageFooter" runat="server" Width="100%" Height="100px" TextMode="MultiLine" CssClass="formTextBox" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Email Header:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailHeader" runat="server" Width="100%" Height="100px" TextMode="MultiLine" CssClass="formTextBox" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Email Footer:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailFooter" runat="server" Width="100%" Height="100px" TextMode="MultiLine" CssClass="formTextBox" />
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align: right;">Send Admin Email:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdminEmail" Width="100%" runat="server" />
                                <br />
                                <br />
                            </td>
                        </tr>

                        <tr>

                            <td style="text-align: right;">
                                <asp:CheckBox ID="chkIncludeMasterSuppressed" runat="server" /></td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblIncludeMS" Text="Include Master Suppressed Groups" CssClass="formLabel" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">Master Suppress Message:</td>
                            <td>
                                <asp:TextBox ID="txtMSMessage" runat="server" Width="100%" Height="100px" TextMode="MultiLine" CssClass="formTextBox" />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr runat="server" id="lblURL">
                            <td style="text-align: right; padding-top: 5px;">URL:</td>
                            <td style="padding-top: 5px;">
                                <asp:Label ID="txtURL" runat="server" Width="100%" CssClass="formLabel" />
                            </td>
                        </tr>
                    </table>

                </td>

            </tr>
            <tr>
                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width:50%; vertical-align:top;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="vertical-align: top; width: 15%;">Reason Label
                                        </td>
                                        <td style="width: 10%; vertical-align: top;">Make Field Visible
                                        </td>
                                        <td style="width: 20%; vertical-align: top;">
                                            <asp:RadioButtonList ID="rblVisibilityReason" Style="padding-top: 0px; margin-top: 0px;" runat="server" onchange="disableButton()" AutoPostBack="true" OnSelectedIndexChanged="rblVisibilityReason_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem Selected="True">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; vertical-align: top;" colspan="3">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Panel ID="pnlReason" runat="server">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td></td>
                                                                    <td style="text-align:left;">
                                                                        <asp:TextBox ID="txtReasonLabel" runat="server" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Response Type</td>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="rblReasonControlType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblReasonControlType_SelectedIndexChanged" onchange="disableButton()">
                                                                            <asp:ListItem Selected="True">Text Box</asp:ListItem>
                                                                            <asp:ListItem>Drop Down</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:UpdatePanel ID="pnlReasonDropDown" ChildrenAsTriggers="true" Width="100%" runat="server">
                                                                            <ContentTemplate>
                                                                            <table style="width: 100%;">
                                                                                <tr valign="top" style="text-align: left;">
                                                                                    <td>Reason Dropdown Fields</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>

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
                                                                                </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>

                                                    </td>
                                                </tr>


                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width:50%;vertical-align:top;">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="vertical-align: top; width: 35%;">Thank You and Redirect
                                        </td>
                                        <td style="width: 65%; text-align: left;">
                                            <asp:RadioButtonList ID="rblRedirectThankYou" AutoPostBack="true" OnSelectedIndexChanged="rblRedirectThankYou_SelectedIndexChanged" RepeatDirection="Vertical" runat="server">
                                                <asp:ListItem Value="thankyou" Text="Show Thank You Message" Selected="True" />
                                                <asp:ListItem Value="redirect" Text="Redirect to URL" />
                                                <asp:ListItem Value="both" Text="Show Thank You Message and Redirect to URL" />
                                                <asp:ListItem Value="neither" Text="Show Neither"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>


                                        <td style="text-align: left; vertical-align: top;" colspan="2">
                                            <table style="width: 100%;">
                                                <tr>

                                                    <td colspan="2" style="text-align: right;">
                                                        <table id="tblThankYou" runat="server" style="width: 100%;">
                                                            <tr>
                                                                <td style="text-align: right; width: 50%;">Thank You Text
                                                                </td>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox ID="txtThankYouMessage" runat="server" TextMode="SingleLine" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>


                                                </tr>
                                                <tr valign="top">
                                                    <td colspan="2" style="text-align: right;">
                                                        <table id="tblRedirect" runat="server" style="width: 100%;">
                                                            <tr>
                                                                <td style="text-align: right; width: 50%;">Redirect URL</td>
                                                                <td style="text-align: left;">
                                                                    <asp:TextBox ID="txtRedirectURL" runat="server" TextMode="SingleLine" CssClass="styled-text" onkeyup="disableButton()"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: right;">
                                                        <table id="tblDelay" runat="server" style="width: 100%;">
                                                            <tr>
                                                                <td style="text-align: right; width: 50%;">Delay Before Redirect</td>
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
                                            </table>

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
                    <table cellspacing="5" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblGroupHeading" runat="server" CssClass="Page_Title" Text="Groups" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="gvGroups" CssClass="ECN-GridView" GridLines="None" OnRowDataBound="gvGroups_RowDataBound" Width="90%" OnRowCommand="gvGroups_RowCommand" AutoGenerateColumns="false" runat="server">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGroupName" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50%" DataField="Label" />

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnEditUDF" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="editudf" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnDeleteGroup" runat="server" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegroup" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border-top: 1px solid gray;" />
                        </tr>
                        <tr>
                            <td style="width: 30%;">
                                <asp:Label ID="lblSelectCustomer" Text="Customer:" runat="server" />
                                <asp:DropDownList ID="ddlSelectCustomer" runat="server" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectCustomer_SelectedIndexChanged" />
                            </td>
                            <td style="width: 30%;">
                                <asp:Label ID="lblSelectGroup" Text="Group:" runat="server" />
                                <asp:DropDownList ID="ddlSelectGroup" Width="80%" runat="server" />
                            </td>
                            <td style="width: 27%;">
                                <asp:Label ID="lblDefineLabel" Text="Label:" runat="server" />
                                <asp:TextBox ID="txtDefineLabel" MaxLength="50" runat="server" />
                            </td>
                            <td style="width: 13%;">
                                <asp:Button ID="btnAddGroup" runat="server" CssClass="formbutton" Text="Add" OnClick="btnAddGroup_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Button ID="btnSavePage" runat="server" CssClass="formbutton" Text="Save" OnClick="btnSavePage_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Button ID="hfEditUDFs" style="display:none;" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeEditUDFs" runat="server" PopupControlID="upEditUDFs" CancelControlID="btnCancelUDFEdit" TargetControlID="hfEditUDFs" BackgroundCssClass="ECN-ModalBackground" />
    <asp:UpdatePanel ID="upEditUDFs" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveUDF" />
            <asp:PostBackTrigger ControlID="btnCancelUDFEdit" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color: #CCCCCC; padding: 15px; border-radius: 5px;">
                <tr>
                    <td>
                        <asp:Label ID="lblGroupName" runat="server" Text="Group:" />
                    </td>
                    <td>
                        <asp:Label ID="GroupName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblGroupLabel" runat="server" Text="Label:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtGroupLabel" MaxLength="50" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUDF" runat="server" Text="UDFs:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUDF" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStaticValue" runat="server" Text="Static value:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtStaticValue" runat="server" />
                        <asp:ImageButton ID="imgbtnAddUDF" runat="server" ImageUrl="/ecn.images/images/icon-add.gif" OnClick="imgbtnAddUDF_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblErrorMessageUDF" runat="server" Visible="false" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="overflow: auto; height: 100px; margin-top: 5px;">
                            <asp:GridView ID="gvUDF" CssClass="ECN-GridView" GridLines="None" ShowHeader="false" Width="100%" runat="server" OnRowDataBound="gvUDF_RowDataBound" OnRowCommand="gvUDF_RowCommand" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblShortName" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="StaticValue" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDeleteUDF" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="DeleteUDF" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 50%; text-align: center;">
                                    <asp:Button ID="btnSaveUDF" OnClick="btnSaveUDF_Click" runat="server" Text="Save" />
                                </td>
                                <td style="width: 50%; text-align: center;">
                                    <asp:Button ID="btnCancelUDFEdit" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="hfeditReason" runat="server" style="display:none;" />

    <ajaxToolkit:ModalPopupExtender ID="mpeEditReason" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlEditReason" TargetControlID="hfeditReason" />
    <asp:UpdatePanel ID="pnlEditReason" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate>
        <table style="background-color: white;">
            <tr>
                <td>
                    
                    <asp:Label ID="lblReasonLabel" runat="server" Text="Label:" />
                </td>
                <td>
                    <asp:TextBox ID="txtReasonLabelEdit" Width="90%" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvReasonEdit" ControlToValidate="txtReasonLabelEdit" runat="server" ForeColor="Red" ErrorMessage="Cannot be empty" ValidationGroup="Edit" />
                </td>
            </tr>
            <tr>
                <td style="text-align:center;" colspan="2">
                    <table style="width:100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btnSaveReason" runat="server" OnClick="btnSaveReason_Click" ValidationGroup="Edit" CausesValidation="true" Text="Save" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelEditReason" runat="server" OnClick="btnCancelEditReason_Click" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                    
                </td>
                
            </tr>
        </table>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadPanel.ascx.cs" Inherits="KMPS.MD.Controls.DownloadPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Marketo" TagPrefix="uc" Src="~/Controls/Marketo.ascx" %>
<%@ Register TagName="DownloadCase" TagPrefix="dc" Src="~/Controls/DownloadCase.ascx" %>

<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .ModalWindow {
        border: solid 1px#c0c0c0;
        background: #ffffff;
        padding: 0px 10px 10px 10px;
        position: absolute;
        top: -1000px;
    }

    .modalPopup {
        background-color: transparent;
        padding: 1em 6px;
    }

    .modalPopup2 {
        background-color: #ffffff;
        vertical-align: top;
    }

    .ui-dialog
    {
        z-index: 1000001;
        font-size: 12px;
    }
    .ui-dialog-titlebar-close
    {
       visibility: hidden;

    }
</style>
<script type="text/javascript">

    function confirmPurchase() {
        try
        {
            var dialog =  $("#dialog-confirm").dialog({
                autoOpen: false,
                bgiframe: true,
                modal: true,
                open: function (type, data) {
                    var markup = 'Are you sure you want to conduct this download? Confirming will result in pre-agreed cost that will need to be paid in the near future.';
                    $(this).html(markup);
                    $(this).parent().appendTo("form1");
                   },
                buttons: {
                    Yes: function () {
                        dialog.dialog("close");
                        if ($('#ctl00_Content_DownloadPanel1_drpIsBillable').val() == 'true') {
                            $('#ctl00_Content_DownloadPanel1_drpIsBillable').prop('disabled', true);
                            $('#ctl00_Content_DownloadPanel1_btnDownload').prop('disabled', true);
                        }
                         <%=Page.ClientScript.GetPostBackEventReference(btnDownload, "") %>
                    },
                    Cancel: function () {
                        dialog.dialog("close");
                    }
                }
            });

            dialog.dialog("open");
            return false;
        }
        catch (ex) {
        }
    }
</script>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelDownload" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000000; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelDownload" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnDownload" />
        <asp:PostBackTrigger ControlID="btnCloseExport" />
        <asp:AsyncPostBackTrigger ControlID="DownloadEditCase" EventName="CausePostBack" />
    </Triggers>
    <ContentTemplate>
        <asp:Button ID="btnShowDownloads" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mdlDownloads" runat="server" TargetControlID="btnShowDownloads"
            PopupControlID="pnlDownloads" BackgroundCssClass="modalBackground" BehaviorID="BPopup" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior6"
            TargetControlID="pnlPopupDownloadRound" Radius="6" Corners="All" />
        <asp:Panel ID="pnlDownloads" runat="server" Width="1000px" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupDownloadRound" runat="server" Width="1000px"  CssClass="modalPopup2">
                <div id="dvDownloads" align="center" style="text-align: center; padding: 10px 10px 10px 10px;"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">Download/Export Records
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="dialog-confirm" title="Confirmation Box"></div>
                                <asp:Label ID="lblNoDownloadMessage" runat="Server" ForeColor="Red" Visible="false"></asp:Label>
                                <div id="divError" runat="Server" visible="false">
                                    <table cellspacing="0" cellpadding="0" align="center">
                                        <tr>
                                            <td id="errorTop"></td>
                                        </tr>
                                        <tr>
                                            <td id="errorMiddle" width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td valign="top" align="center" width="20%">
                                                            <img id="Img1" style="padding: 0 0 0 15px;" src="~/images/errorEx.jpg" runat="server"
                                                                alt="" />
                                                        </td>
                                                        <td valign="middle" align="left" width="80%" height="100%">
                                                            <asp:Label ID="lblErrorMessage" runat="Server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="errorBottom"></td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <asp:Panel ID="pnlUADExport" runat="server">
                            <asp:Panel ID="pnlIsRecentData" runat="server">
                                <tr>
                                    <td colspan="2" align="left">
                                        <div id="confirmDelete"></div> 
                                        <asp:CheckBox ID="cbIsRecentData" runat="server" Text=" Most Recent Data" Checked="false" TextAlign="right" Enabled="false" />&nbsp;</td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td style="padding: 10px" align="left" colspan="2">
                                    <asp:PlaceHolder ID="phRBExport" runat="server" Visible="false">
                                        <asp:RadioButton ID="rbGroupExport" runat="Server" Text=" Export to Group" AutoPostBack="true"
                                            OnCheckedChanged="rbGroupExport_CheckedChanged" GroupName="grpSelect"></asp:RadioButton>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phRBDownload" runat="server" Visible="false">&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbDownload" runat="Server" Text=" Download" GroupName="grpSelect"
                                            OnCheckedChanged="rbDownload_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phRBCampaign" runat="server" Visible="false">&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbCampaign" runat="Server" Text=" Save to campaign" AutoPostBack="true"
                                            OnCheckedChanged="rbCampaign_CheckedChanged" GroupName="grpSelect"></asp:RadioButton>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phMarketo" runat="server" Visible="false">&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbMarketo" runat="Server" Text=" Export to Marketo" AutoPostBack="true"
                                            OnCheckedChanged="rbMarketo_CheckedChanged" GroupName="grpSelect"></asp:RadioButton>
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="phGroupExport" runat="server" Visible="false">
                                <tr>
                                    <td style="padding-left: 30px" align="left" colspan="2">
                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td width="15%">Job
                                                </td>
                                                <td width="85%">
                                                    <asp:TextBox ID="txtJob" runat="server" Width="100" MaxLength="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Customer
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpClient" AutoPostBack="true" runat="server" EnableViewState="true"
                                                        Width="200px" OnSelectedIndexChanged="drpClient_SelectedIndexChanged" DataTextField="ClientName"
                                                        DataValueField="ClientID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <asp:PlaceHolder ID="plMessage" runat="server" Visible="false">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblMessage" runat="Server" ForeColor="Red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="plList" runat="server" Visible="false">
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                            <tr>
                                                                <td align="left" width="15%">
                                                                    <asp:RadioButton ID="rbNewGroup" runat="Server" Text=" Create New Email List</span>"
                                                                        GroupName="grpEmailList" AutoPostBack="true" OnCheckedChanged="rbNewGroup_CheckedChanged"></asp:RadioButton>
                                                                </td>
                                                            </tr>
                                                            <asp:PlaceHolder ID="plNewList" runat="server" Visible="false">
                                                                <tr>
                                                                    <td style="padding-left: 30px">
                                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                            <tr>
                                                                                <td width="30%">Save in Folder
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpFolder" runat="server" Width="200px" DataTextField="FolderName"
                                                                                        DataValueField="FolderID">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>List Name
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtGroupName" runat="server" Width="200" MaxLength="50"></asp:TextBox>&nbsp;
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtGroupName" runat="server" Font-Size="xx-small"
                                                                                            ControlToValidate="txtGroupName" ErrorMessage=" * required"
                                                                                            Font-Bold="True" ForeColor="red" ValidationGroup="Export"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </asp:PlaceHolder>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                            <tr>
                                                                <td align="left" width="15%">
                                                                    <asp:RadioButton ID="rbExistingGroup" runat="Server" Text=" Use Existing Email List"
                                                                        GroupName="grpEmailList" AutoPostBack="true" OnCheckedChanged="rbExistingGroup_CheckedChanged"></asp:RadioButton>
                                                                </td>
                                                            </tr>
                                                            <asp:PlaceHolder ID="plExistingList" runat="server" Visible="false">
                                                                <tr>
                                                                    <td style="padding-left: 30px">
                                                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                            <tr>
                                                                                <td width="30%">Save in Folder
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpFolder1" runat="server" Width="200px" DataTextField="FolderName"
                                                                                        DataValueField="FolderID" OnSelectedIndexChanged="drpFolder1_SelectedIndexChanged"
                                                                                        AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>List Name
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="drpExistingGroupName" runat="server" DataTextField="GroupName"
                                                                                        DataValueField="GroupID" Width="200px">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rvGroup" runat="server" ControlToValidate="drpExistingGroupName" ValidationGroup="Export" Font-Size="xx-small" ForeColor="red"
                                                                                        ErrorMessage=" * required" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </asp:PlaceHolder>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </asp:PlaceHolder>
                                        </table>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phShowHeader" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2" align="left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="cbShowHeader" runat="server" Checked="false" />
                                        &nbsp;Include Query Details Header
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phCampaign" runat="server" Visible="false">
                                <tr>
                                    <td style="padding-left: 30px" align="left" colspan="2">
                                        <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                            <tr>
                                                <td align="left" width="15%">FilterName
                                                </td>
                                                <td align="left" width="85%">
                                                    <asp:TextBox ID="txtFilterName" runat="server" Width="250" MaxLength="500"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Font-Size="xx-small"
                                                        ControlToValidate="txtFilterName" ErrorMessage="* required" ForeColor="red"
                                                        Font-Bold="True" ValidationGroup="Export"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="15%">
                                                                <asp:RadioButton ID="rbNewCampaign" runat="Server" Text=" Create new Campaign</span>" GroupName="grpCampaignList"
                                                                    AutoPostBack="true" OnCheckedChanged="rbNewCampaign_CheckedChanged"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <asp:PlaceHolder ID="phNewCampaign" runat="server" Visible="false">
                                                            <tr>
                                                                <td style="padding-left: 30px">
                                                                    <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                        <tr>
                                                                            <td>Campaign Name
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCampaignName" runat="server" Width="200" MaxLength="100"></asp:TextBox>&nbsp;
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Font-Size="xx-small"
                                                                                        ControlToValidate="txtCampaignName" ErrorMessage="* required" ForeColor="red"
                                                                                        Font-Bold="True" ValidationGroup="Export"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                        <tr>
                                                            <td align="left" width="15%">
                                                                <asp:RadioButton ID="rbExistingCampaign" runat="Server" Text=" Use Existing Campaign" GroupName="grpCampaignList"
                                                                    AutoPostBack="true" OnCheckedChanged="rbExistingCampaign_CheckedChanged"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                        <asp:PlaceHolder ID="phExistingCampaign" runat="server" Visible="false">
                                                            <tr>
                                                                <td style="padding-left: 30px">
                                                                    <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                        <tr>
                                                                            <td>Campaign Name
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="drpExistingCampaign" runat="server" DataTextField="CampaignName" DataValueField="CampaignID"
                                                                                    Width="200px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </asp:PlaceHolder>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phMarketoMapping" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2">
                                        <div style="height: 400px; width: 950px; border-color: red; overflow: scroll;">
                                            <uc:Marketo runat="server" ID="Marketo"></uc:Marketo>
                                        </div>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phExportFields" runat="server">
                                <tr>
                                    <td colspan="2" align="left">Download Template&nbsp;&nbsp;
                                    <asp:DropDownList ID="drpDownloadTemplate" runat="server" Width="200px" DataTextField="DownloadTemplateName" DataValueField="DownloadTemplateID"
                                        AutoPostBack="true" OnSelectedIndexChanged="drpDownloadTemplate_SelectedIndexChanged">
                                    </asp:DropDownList>
                                        <asp:HiddenField ID="hfDownloadTemplateID" runat="server" Value="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table width="50%" border="0" cellpadding="5" cellspacing="5">
                                            <tr>
                                                <td align="center" width="15%"><b>Available Fields</b></td>
                                                <td width="5%"></td>
                                                <td align="center" width="15%"><b>Selected Fields</b></td>
                                                <td width="5%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblFieldsType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblFieldsType_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="Profile" Selected="True">Profile</asp:ListItem>
                                                        <asp:ListItem Value="Demo">Demo</asp:ListItem>
                                                        <asp:ListItem Value="Adhoc">Adhoc</asp:ListItem>
                                                    </asp:RadioButtonList>&nbsp;&nbsp;
                                                </td>
                                                <td></td>
                                                <td><asp:Button ID="btnEditCase" runat="server" CssClass="button" OnClick="btnEditCase_Click" Text="Edit Case" /></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <asp:PlaceHolder ID="phProfileFields" runat="server">
                                                    <td>
                                                        <asp:ListBox ID="lstAvailableProfileFields" runat="server" Rows="10" 
                                                            SelectionMode="Multiple" Width="350px"
                                                            EnableViewState="True"></asp:ListBox>
                                                    </td>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phDemoFields" runat="server" Visible = "false">
                                                    <td>
                                                        <asp:ListBox ID="lstAvailableDemoFields" runat="server" Rows="10" 
                                                            SelectionMode="Multiple" Width="350px"
                                                            EnableViewState="True"></asp:ListBox>
                                                    </td>
                                                </asp:PlaceHolder>
                                                <asp:PlaceHolder ID="phAdhocFields" runat="server" Visible ="false">
                                                    <td>
                                                        <asp:ListBox ID="lstAvailableAdhocFields" runat="server" Rows="10" 
                                                            SelectionMode="Multiple" Width="350px"
                                                            EnableViewState="True"></asp:ListBox>
                                                    </td>
                                                </asp:PlaceHolder>
                                                <td>
                                                    <asp:Button ID="btnAdd" runat="server" Text=">>" CssClass="button" OnClick="btnAdd_Click" />
                                                    <br>
                                                    <br>
                                                    <asp:Button ID="btnremove" runat="server" CssClass="button" OnClick="btnRemove_Click"
                                                        Text="<<" />
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="lstSelectedFields" runat="server" Rows="10"
                                                        SelectionMode="Multiple" Width="350px"
                                                        DataTextField="DisplayName"
                                                        DataValueField="ColumnValue"></asp:ListBox>

                                                </td>
                                                <td>
                                                    <asp:Button ID="btnUp" runat="server" Text="Move Up" CssClass="button" OnClick="btnUp_Click" />
                                                    <br>
                                                    <br>
                                                    <asp:Button ID="btnDown" runat="server" CssClass="button" OnClick="btndown_Click"
                                                        Text="Move Down" />
                                                </td>
                                            </tr>
                                        <tr>
                                            <td colspan="4"><asp:Label ID="lblErrSelectedFields" text="" runat="Server" Visible="false" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                        </table>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phDownloadCount" runat="server">
                                <tr>
                                    <td colspan="2" align="left">Total Records to export or download&nbsp;&nbsp;
                                    <asp:TextBox ID="txtTotalCount" Width="100px" runat="server" value="0" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtDownloadCount" Width="100px" runat="server" value="0"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqDownloadCount" runat="server" ControlToValidate="txtDownloadCount"
                                            ErrorMessage="* required" ValidationGroup="Export" Display="Dynamic" ForeColor="red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator runat="server" ID="rngCount" ControlToValidate="txtDownloadCount" ForeColor="red"
                                            Type="Integer" MinimumValue="1" ErrorMessage="</br>Please enter a value within total counts"
                                            MaximumValue="1" ValidationGroup="Export" Display="Dynamic" />
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phPromoCode" runat="server">
                                <tr>
                                    <td colspan="2" align="left">Promocode&nbsp;&nbsp;
                                    <asp:TextBox ID="txtPromocode" runat="server" Width="100" MaxLength="10"></asp:TextBox></td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plKmStaff" runat="server" Visible="false">
                            <tr>
                                <td colspan="2" align="left">Is Import Billable?
                                    <asp:DropDownList ID="drpIsBillable" Width="100" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="drpIsBillable_OnSelectedIndexChanged">
                                        <asp:ListItem Selected="true" Value="true">Yes</asp:ListItem>
                                        <asp:ListItem Value="false">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="plNotes" runat="server" Visible="false">
                            <tr>
                                <td colspan="2" align="left" style="vertical-align:middle">Notes&nbsp;
                                    <asp:TextBox ID="txtNotes" runat="server" Columns="5" Width="300px" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            </asp:PlaceHolder>
                            </asp:PlaceHolder>
                            <tr>
                                <td colspan="2" align="left">
                                    <telerik:RadCaptcha ID="rcForDownload" runat="server" ErrorMessage="Page not valid. The code you entered is not valid."
                                        ValidationGroup="Export" ForeColor="Red" EnableRefreshImage="true" Visible="false">
                                        <CaptchaImage ImageCssClass="imageClass" />
                                    </telerik:RadCaptcha>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                        AutoGenerateColumns="False" Width="100%" Visible="false">
                                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="Type" HeaderText="Type" ItemStyle-Width="25%"
                                                HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="50%"
                                                HeaderStyle-Width="50%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                    <asp:Label ID="lblResults" runat="server" Text="" Visible="false"></asp:Label>
                                    
                                    <asp:PlaceHolder ID="plDataCompareResult" runat="server" Visible="false">
                                        <asp:DataGrid ID="dgDataCompareResult" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                                            AutoGenerateColumns="False" Width="100%">
                                            <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                                            <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="Type" HeaderText="Type" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ItemCount" HeaderText="ItemCount" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CostPerItemClient" HeaderText="Client </br> CostPerItem" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CostPerItemDetailClient" HeaderText="Client </br> CostDetail" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ItemTotalCostClient" HeaderText="Client </br> TotalCost" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CostPerItemThirdParty" HeaderText="ThirdParty </br> CostPerItem" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="CostPerItemDetailThirdParty" HeaderText="ThirdParty </br> CostDetail" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="ItemTotalCostThirdParty" HeaderText="ThirdParty </br> TotalCost" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                        <asp:Label ID="lblDataCompareMessage" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </asp:PlaceHolder>
                                </td>
                            </tr>
                        </asp:Panel>

                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="text-align:center">
                    <asp:Button ID="btnDownload" Text="Download" CssClass="button" runat="server" OnClick="btnExport_click" Visible="false"
                        ValidationGroup="Export" />
                    <asp:Button ID="btnPreECNExportResults" Text="Export Count" CssClass="buttonMedium" runat="server" OnClick="btnPreECNExportResults_click" Visible="false"
                        ValidationGroup="Export" />
                    <asp:Button ID="btnExport" Text="Export" CssClass="button" runat="server" OnClick="btnExport_click" Visible="false"
                        ValidationGroup="Export" />
                    <asp:Button ID="btnCloseExport" Text="Close" CssClass="button" runat="server" OnClick="btnCloseExport_Click" />
                    <br /><br />
                </div>
            </asp:Panel>
        </asp:Panel>
        
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel11" runat="server">
    <ContentTemplate>
        <dc:DownloadCase runat="server" ID="DownloadEditCase" Visible="false"></dc:DownloadCase>
    </ContentTemplate>
</asp:UpdatePanel>
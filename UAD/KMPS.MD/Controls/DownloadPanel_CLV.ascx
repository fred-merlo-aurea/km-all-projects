<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadPanel_CLV.ascx.cs" Inherits="KMPS.MD.Controls.DownloadPanel_CLV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="DownloadCase" TagPrefix="dc" Src="~/Controls/DownloadCase.ascx" %>

<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .ModalWindow {
        border: solid1px#c0c0c0;
        background: #ffffff;
        padding: 0px10px10px10px;
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
</style>
<script type="text/javascript">
    function confirmPopupPurchase() {
        if (!confirm("Are you sure you want to conduct this download? Confirming will result in pre-agreed cost that will need to be paid in the near future.")) {
            return false;
        }
        else {
            if ($('#ctl00_Content_DownloadPanel1_drpIsBillable').val() == 'true') {
                $('#ctl00_Content_DownloadPanel1_drpIsBillable').prop('disabled', true);
            }
        }
    }
</script>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelDownload" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000001; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelDownload" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
        <asp:PostBackTrigger ControlID="btnCloseExport" />
        <asp:AsyncPostBackTrigger ControlID="DownloadEditCase" EventName="CausePostBack" />
    </Triggers>
    <ContentTemplate>
        <asp:Button ID="btnShowDownloads" runat="server" Style="display: none" />
        <ajaxtoolkit:modalpopupextender id="mdlDownloads" runat="server" targetcontrolid="btnShowDownloads"
            popupcontrolid="pnlDownloads" backgroundcssclass="modalBackground" behaviorid="BPopup" />
        <ajaxtoolkit:roundedcornersextender id="RoundedCornersExtender" runat="server" behaviorid="RoundedCornersBehavior6"
            targetcontrolid="pnlPopupDownloadRound" radius="6" corners="All" />
        <asp:Panel ID="pnlDownloads" runat="server" Width="1000px" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupDownloadRound" runat="server" Width="1000px" CssClass="modalPopup2">
                <div id="dvDownloads" align="center" style="text-align: center; padding: 10px 10px 10px 10px;"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold" colspan="2">Download/Export Records
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblNoDownloadMessage" runat="Server" ForeColor="Red" Visible="false"></asp:Label>
                                <div id="divError" runat="Server" visible="false" >
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
                            <asp:Panel ID="pnlIsRecentData" runat="server" Visible="false">
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:CheckBox ID="cbIsRecentData" runat="server" Text=" Most Recent Data" Checked="false" TextAlign="right" Enabled="false" /></td>
                                </tr>
                            </asp:Panel>
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">Total Records&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDownloadCount" Width="100px" runat="server" value="0" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">Total Unique Locations&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDownloadUniqueCount" Width="100px" runat="server" value="0" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left">Promocode&nbsp;&nbsp;
                                    <asp:TextBox ID="txtPromocode" runat="server" Width="100" MaxLength="10"></asp:TextBox></td>
                            </tr>
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
                                    <telerik:radcaptcha id="rcForDownload" runat="server" errormessage="Page not valid. The code you entered is not valid."
                                        validationgroup="Export" forecolor="Red" enablerefreshimage="true" visible="false">
                                        <CaptchaImage ImageCssClass="imageClass" />
                                    </telerik:radcaptcha>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 10px" align="left" colspan="2">
                                    <asp:RadioButton ID="rbDownloadAll" runat="Server" Text=" Download All Records" GroupName="grpSelect"
                                        OnCheckedChanged="rbDownload_CheckedChanged" Checked="true" AutoPostBack="true"></asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 10px" align="left" colspan="2">
                                    <asp:RadioButton ID="rbDownload" runat="Server" Text=" Download one record per location" GroupName="grpSelect"
                                        OnCheckedChanged="rbDownload_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                </td>
                            </tr>
                            <asp:PlaceHolder ID="phShowHeader" runat="server" Visible="false">
                                <tr>
                                    <td style="padding: 10px" colspan="2" align="left">
                                        <asp:CheckBox ID="cbShowHeader" runat="server" Checked="false" />
                                        &nbsp;Include Query Details Header
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plDataCompareResult" runat="server" Visible="false">
                                <tr>
                                    <td valign="top" colspan="2">
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
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                        </asp:Panel>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnExport" Text="Download" CssClass="button" runat="server" OnClick="btnExport_click"
                                    ValidationGroup="Export" />
                                <asp:Button ID="btnCloseExport" Text="Close" CssClass="button" runat="server" OnClick="btnCloseExport_Click" />
                            </td>
                        </tr>
                        <asp:ModalPopupExtender ID="mdlPopupExportProgress" runat="server" TargetControlID="pnlPopupExportProgress"
                            PopupControlID="pnlPopupExportProgress" BackgroundCssClass="blur" OnCancelScript="HideModalPopup()" />
                        <asp:Panel ID="pnlPopupExportProgress" runat="server" CssClass="progress" Style="display: none">
                            <div class="TransparentGrayBackground">
                            </div>
                            <div id="div1" class="UpdateProgress" style="position: absolute; z-index: 1000001; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                                <br />
                                <b>Processing...</b><br />
                                <br />
                                <img src="../images/loading.gif" />
                            </div>
                        </asp:Panel>
                    </table>
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
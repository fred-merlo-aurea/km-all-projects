<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadCase.ascx.cs" Inherits="KMPS.MD.Controls.DownloadCase" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagName="Marketo" TagPrefix="uc" Src="~/Controls/Marketo.ascx" %>
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
        width: 270px;
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
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelDownload" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 1000002; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelDownload" runat="server" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="drpFileFormat" />
        <asp:AsyncPostBackTrigger ControlID="dlDownloadFields" />
    </Triggers>
    <ContentTemplate>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeCase" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlPopupDimensions2" BackgroundCssClass="modalBackground" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender10" runat="server" BehaviorID="RoundedCornersBehavior10"
            TargetControlID="pnlCase" Radius="6" Corners="All" />
        <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="450px" CssClass="modalPopup">
            <asp:Panel ID="pnlCase" runat="server" Width="450px"  CssClass="modalPopup2">
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Case Edit
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divScroll" style="height: 550px; overflow: auto;" runat="server">
                                    <table border="0" width="100%" cellpadding="5" cellspacing="5">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblID" Text="" runat="server"
                                                Visible="false"></asp:Label>
                                                <b>File Format</b>&nbsp;&nbsp; 
                                                <asp:DropDownList ID="drpFileFormat" runat="server"  OnSelectedIndexChanged="drpFileFormat_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Value="Default" Text ="As Imported" />
                                                    <asp:ListItem Value="ProperCase"  Text ="Proper" />
                                                    <asp:ListItem Value="UpperCase"  Text ="UPPER" />
                                                    <asp:ListItem Value="LowerCase"  Text ="lower" />
                                                   <%-- <asp:ListItem Value="Custom"  Text ="Custom" />--%>
                                                </asp:DropDownList>
                                            <asp:HiddenField ID="hfFileFormat" runat="server"  Value ="Default"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DataList ID="dlDownloadFields" runat="server" AlternatingItemStyle-Width="50%"
                                            ItemStyle-Width="50%" RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Table"
                                            ItemStyle-BorderWidth="0" Width="100%">
                                            <ItemTemplate>
                                                <table cellspacing="0" cellpadding="0" width="100%" align="center" border='0'>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblDownloadFieldName" Font-Bold="True" 
                                                                Text='<%#Eval("Value") %>' runat="server"></asp:Label>&nbsp;&nbsp;
                                                            <asp:HiddenField ID="hfDownloadFieldText" runat="server" Value='<%#Eval("Key") %>'  />
                                                        </td>
                                                        <td  width="30%" align="left">
                                                            <asp:DropDownList ID="drpDownloadFieldCase" runat="server" 
                                                                Width="100" OnSelectedIndexChanged="drpDownloadFieldCase_SelectedIndexChanged"  AutoPostBack="true">
                                                                    <asp:ListItem Value="Default" Text ="As Imported" />
                                                                    <asp:ListItem Value="ProperCase"  Text ="Proper" />
                                                                    <asp:ListItem Value="UpperCase"  Text ="UPPER" />
                                                                    <asp:ListItem Value="LowerCase"  Text ="lower" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                                <div style="text-align:center">
                                    <asp:Button runat="server" Text="Save" ID="btnSelectCase" class="button"
                                        OnClick="btnSelectCase_Click" ValidationGroup="Select" Width="90px"></asp:Button>
                                    <asp:Button runat="server" Text="Cancel" ID="btnCancelCase" class="button"
                                        OnClick="btnCancelCase_Click" Width="90px"></asp:Button>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Circulation.ascx.cs" Inherits="KMPS.MD.Controls.Circulation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<style type="text/css">
    .modalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }

    .ModalWindow {
        border: solid 1px #c0c0c0;
        background: #ffffff;
        padding: 0px 10px 10px 10px;
        position: absolute;
        top: -1000px;
    }

    .modalPopup {
        background-color: transparent;
        padding: 1em 6px;
        z-index: 10001 !important;
    }

    .modalPopupCal {
        background-color: transparent;
        padding: 1em 6px;
        z-index: 10002 !important;
    }

    .modalPopup2 {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
        z-index: 10001 !important;
    }

    .modalPopup2Cal {
        background-color: #ffffff;
        width: 270px;
        vertical-align: top;
        z-index: 10002 !important;
    }
</style>
<asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
    AssociatedUpdatePanelID="UpdatePanelDownload" DynamicLayout="true">
    <ProgressTemplate>
        <div class="TransparentGrayBackground">
        </div>
        <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
            <br />
            <b>Processing...</b><br />
            <br />
            <img src="../images/loading.gif" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanelDownload" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Button ID="btnShowCirculation" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeCirculation" runat="server" TargetControlID="btnShowCirculation"
            PopupControlID="pnlCirculation" BackgroundCssClass="modalBackground" BehaviorID="BPopup" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender" runat="server" BehaviorID="RoundedCornersBehavior6"
            TargetControlID="pnlPopupDownloadRound" Radius="6" Corners="All" />
        <asp:Panel ID="pnlCirculation" runat="server" Width="1100px" CssClass="modalPopup">
            <asp:Panel ID="pnlPopupDownloadRound" runat="server" Width="1100px" CssClass="modalPopup2">
                <div id="dvCirculation" align="center" style="text-align: center; padding: 10px 10px 10px 10px; height: 100%"
                    runat="server">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Circulation Filter
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divError" runat="Server" visible="false" style="width: 400px">
                                    <table cellspacing="0" cellpadding="0" width="400px" align="center">
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
                                    </table>
                                    <br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="always">
                                    <ContentTemplate>
                                    <table border="0" bordercolor="#cccccc" cellpadding="1" cellspacing="1" width="100%">
                                        <tr>
                                            <td>
                                                <table border="0" bordercolor="#cccccc" cellpadding="1" cellspacing="1" width="100%">
                                                   <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                        <td valign="middle" align="center">
                                                            <b>Category Code Type</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Category Codes</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Transaction Type</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Transaction Code</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstCategoryCodeType" runat="server" Rows="15" DataValueField="CategoryCodetypeID" 
                                                                DataTextField="CategoryCodetypeName" SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial" AutoPostBack="true"
                                                                Style="text-transform: uppercase" Width="150px" OnSelectedIndexChanged="lstCategoryCodeType_SelectedIndexChanged" ></asp:ListBox>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstCategoryCodes" runat="server" Rows="15" DataValueField="CategoryCodeID"
                                                                DataTextField="CategoryCodeName" SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial"
                                                                Style="text-transform: uppercase" Width="350px"></asp:ListBox>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstTransaction" runat="server" Rows="15" DataValueField="TransactionCodeTypeID" 
                                                                DataTextField="TransactionCodeTypeName" SelectionMode="Multiple" Font-Size="x-small"  AutoPostBack="true"
                                                                Style="text-transform: uppercase" Font-Names="Arial" Width="100px" OnSelectedIndexChanged="lstTransaction_SelectedIndexChanged"></asp:ListBox>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstTransactionCode" runat="server" Rows="15" DataValueField="TransactionCodeID"
                                                                DataTextField="TransactionCodeName" SelectionMode="Multiple" Font-Size="x-small"
                                                                Style="text-transform: uppercase" Font-Names="Arial" Width="350px"></asp:ListBox>
                                                        </td>
                                                    </tr>    
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" bordercolor="#cccccc" cellpadding="1" cellspacing="1" width="100%">
                                                <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                                                    <td valign="middle" align="center" width="5%">
                                                        <b>Qual. Source Type</b>
                                                    </td>
                                                    <td valign="middle" align="left">
                                                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            Qual. Source Code</b>
                                                    </td>
                                                    <asp:PlaceHolder ID="phOtherHeader" runat="server" Visible="false">
                                                        <td valign="middle" align="center">
                                                            <b>Media</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Year</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Qualification Date</b>
                                                        </td>
                                                        <td valign="middle" align="center">
                                                            <b>Wave Mailing</b>
                                                        </td>
                                                    </asp:PlaceHolder>
                                                </tr>
                                                <tr>
                                                    <td valign="top" align="center"  width="5%">
                                                        <asp:ListBox ID="lstQsourceType" runat="server" Rows="15" DataValueField="CodeID"
                                                            DataTextField="DisplayName" SelectionMode="Multiple" Font-Size="x-small" AutoPostBack="true"
                                                            Style="text-transform: uppercase" Font-Names="Arial" Width="150px"  OnSelectedIndexChanged="lstQsourceType_SelectedIndexChanged"></asp:ListBox>
                                                    </td>
                                                    <td valign="top" align="left">
                                                        <asp:ListBox ID="lstQsourceCode" runat="server" Rows="15" DataValueField="CodeID"
                                                            DataTextField="DisplayName" SelectionMode="Multiple" Font-Size="x-small"
                                                            Style="text-transform: uppercase" Font-Names="Arial" Width="370px"></asp:ListBox>
                                                    </td>
                                                    <asp:PlaceHolder ID="phOther" runat="server" Visible="false">
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstMedia" runat="server" Rows="5" SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial"
                                                                Style="text-transform: uppercase" Width="100px">
                                                                <asp:ListItem Value="A">PRINT</asp:ListItem>
                                                                <asp:ListItem Value="B">DIGITAL</asp:ListItem>
                                                                <asp:ListItem Value="C">BOTH</asp:ListItem>
                                                                <asp:ListItem Value="O">OPT OUT</asp:ListItem>
                                                            </asp:ListBox>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:ListBox ID="lstYear" runat="server" Rows="5" SelectionMode="Multiple" Font-Size="x-small" Font-Names="Arial"
                                                                Style="text-transform: uppercase" Width="100px">
                                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                                <asp:ListItem Value="2">2</asp:ListItem>
                                                                <asp:ListItem Value="3">3</asp:ListItem>
                                                                <asp:ListItem Value="4">4</asp:ListItem>
                                                                <asp:ListItem Value="5">5</asp:ListItem>
                                                            </asp:ListBox>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:TextBox ID="txtQDateFrom" Width="65" CssClass="formfield" MaxLength="10"
                                                                runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender
                                                                    ID="CalendarExtender11" runat="server" CssClass="MyCalendar" TargetControlID="txtQDateFrom"
                                                                    Format="MM/dd/yyyy" PopupButtonID="btnDatePicker">
                                                            </ajaxToolkit:CalendarExtender>&nbsp                                                    
                                                            To &nbsp;
                                                            <asp:TextBox ID="txtQDateTo" Width="65" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender
                                                                    ID="CalendarExtender1" runat="server" CssClass="MyCalendar" TargetControlID="txtQDateTo"
                                                                    Format="MM/dd/yyyy" PopupButtonID="btnDatePicker">
                                                            </ajaxToolkit:CalendarExtender>                                                
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:DropDownList ID="ddlWaveMailing" runat="server">
                                                                <asp:ListItem Value=""></asp:ListItem>
                                                                <asp:ListItem Value="1">Is Wave Mailed</asp:ListItem>
                                                                <asp:ListItem Value="0">Is Not Wave Mailed</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </asp:PlaceHolder>
                                                </tr>  
                                                </table> 
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td  style="align-items:center" colspan="5">
                                <br />
                                <asp:Button ID="btnSelect" Text="Select/Close" CssClass="button" runat="server" ValidationGroup="Select"
                                    OnClick="btnSelect_Click" />
                                <asp:Button ID="btnReset" Text="Reset" CssClass="button" runat="server" OnClick="btnReset_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
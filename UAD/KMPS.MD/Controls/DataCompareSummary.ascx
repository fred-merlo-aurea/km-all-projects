<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataCompareSummary.ascx.cs" Inherits="KMPS.MD.Controls.DataCompareSummary" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
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

    .ui-dialog {
        z-index: 1000001;
        font-size: 12px;
    }

    .ui-dialog-titlebar-close {
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
        <asp:PostBackTrigger ControlID="btnDownload" />
    </Triggers>
    <ContentTemplate>
        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="mpeSummary" runat="server" TargetControlID="btnShowPopup"
            PopupControlID="pnlPopupDimensions2" BackgroundCssClass="modalBackground" />
        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender10" runat="server" BehaviorID="RoundedCornersBehavior10"
            TargetControlID="pnlSummary" Radius="6" Corners="All" />
        <asp:Panel ID="pnlPopupDimensions2" runat="server" Width="900px" CssClass="modalPopup">
            <asp:Panel ID="pnlSummary" runat="server" Width="900px" CssClass="modalPopup2">
                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD; height: auto">
                        <tr style="background-color: #5783BD;">
                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">
                                <div style="float: left; width=80%; text-align:center">Data Compare Summary Report</div>
                                <div style="float: right">
                                    <asp:Button runat="server" Text="Download" ID="btnDownload" class="button"
                                        OnClick="btnDownload_Click" Width="90px"></asp:Button>
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="White" DocumentMapWidth="25%"
                                        Width="1000px" Visible="False">
                                    </rsweb:ReportViewer>
                                </div>
                            </td>
                        </tr>
                         <div id="divError" runat="Server" visible="false" style="width: 400px">
                        <tr>
                            <td>
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
                                                        <asp:Label ID="lblErrorMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </td>
                        </tr>
                        </div>
                        <tr>
                            <td colspan="2">
                                <div id="divScroll" style="height: 600px; overflow: auto;" runat="server">
                                    <table border="0" width="100%" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td colspan="3" align="left">
                                                <h3><b>File Details</b></h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%" align="right">
                                                <b>Total Records :</b>
                                            </td>
                                            <td  width="10%" align="left">
                                                <b><asp:Label ID="lblTotalRecords" runat="server" Text="0"></asp:Label></b>
                                            </td>
                                            <td><b>Percentage Matched</b></td>
                                        </tr>
                                        <tr>
                                            <td  align="right">
                                                <b>Matched Records :</b>
                                            </td>
                                            <td  align="left">
                                                 <b><asp:Label ID="lblMatchedNonMatchedRecords" runat="server" Text="0"></asp:Label></b>
                                            </td>
                                            <td>
                                                <b>
                                                    <asp:Label ID="lblPercentage" runat="server" Text=""></asp:Label></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="left">
                                                <h3><b>Matched Profile Details</b></h3>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  colspan="3">
                                                <asp:GridView ID="gvDataCompareSummary" runat="server" AllowPaging="True" AllowSorting="True" ShowHeader="true"
                                                    AutoGenerateColumns="False" DataKeyNames="Field" BorderColor="#5783BD">
                                                    <Columns>
                                                        <asp:BoundField DataField="Field" HeaderText="" ItemStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Same" HeaderText="Same Data" ItemStyle-HorizontalAlign="center">
                                                            <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="%" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSamePercentage" runat="server" Text='<%# Eval("SamePercentage").ToString() + "%" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Different" HeaderText="Different" ItemStyle-HorizontalAlign="center">
                                                            <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="%" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDifferentPercentage" runat="server" Text='<%# Eval("DifferentPercentage").ToString() + "%" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:BoundField DataField="FileOnly" HeaderText="Only in File" ItemStyle-HorizontalAlign="center">
                                                            <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="%" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFileOnlyPercentage" runat="server" Text='<%# Eval("FileOnlyPercentage").ToString() + "%" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UADOnly" HeaderText="UAD Only" ItemStyle-HorizontalAlign="center">
                                                            <HeaderStyle HorizontalAlign="center" Width="10%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="%" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUADOnlyPercentage" runat="server" Text='<%# Eval("UADOnlyPercentage").ToString() + "%" %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="text-align: center">
                                    <asp:Button runat="server" Text="Close" ID="btnclose" class="button"
                                        OnClick="btnclose_Click" Width="90px"></asp:Button>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

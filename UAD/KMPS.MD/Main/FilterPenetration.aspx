<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="FilterPenetration.aspx.cs" Inherits="KMPS.MD.Main.FilterPenetration" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register TagName="Download" TagPrefix="uc" Src="~/Controls/DownloadPanel.ascx" %>
<%@ Register Assembly="ajaxControlToolkit" Namespace="ajaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
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
        }

        .modalPopup2 {
            background-color: #ffffff;
            width: 270px;
            vertical-align: top;
        }
    </style>
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblCombination" Visible="false" runat="server"></asp:Label>
            <uc:Download runat="server" ID="DownloadPanel1" Visible="false" ViewType="ConsensusView"></uc:Download>
            <br />
            <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                <tr>
                    <td></td>
                    <td colspan="2">
                        <div id="divErrorMsg" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="100%" align="left">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="5%">
                                                    <img id="Img3" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="95%" height="100%">
                                                    <asp:Label ID="lblErrorMsg" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                    <tr style="background-color: #eeeeee; color: White; padding: 2px 0px 2px 0px; height: 20px;">
                        <td valign="middle" align="right">
                            <b>Brand&nbsp;</td>
                        <td>
                            <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="drpBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="drpBrand_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                        </td>
                        <td></td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td valign="middle" align="right" width="10%">
                        <b>Filters</b>
                    </td>
                    <td valign="top" align="left" width="20%">
                        <asp:ListBox ID="lstFilter" runat="server" Rows="10" DataValueField="FilterID" DataTextField="Name"
                            Style="text-transform: uppercase; width: 400px" SelectionMode="Multiple" Font-Size="x-small"
                            Font-Names="Arial"></asp:ListBox>
                    </td>
                    <td width="70%" align="left">
                        <asp:Button ID="btnReport" Text="View Report" runat="server" CssClass="button" OnClick="btnReport_Click"></asp:Button>
                        <br />
                        <br />
                        <asp:Button ID="btnSaveReport" runat="server" CssClass="button"
                            OnClick="btnSaveReport_Click" Text="Save Report" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="right">
                        <b>Saved Reports</b>&nbsp;
                    </td>
                    <td valign="middle" align="left" colspan="2">
                        <asp:Button ID="btnMdlPopupSave" Style="display: none" runat="server"></asp:Button>

                        <ajaxToolkit:ModalPopupExtender ID="mdlPopSave" runat="server" TargetControlID="btnMdlPopupSave"
                            PopupControlID="pnlSave" CancelControlID="btnClose" BackgroundCssClass="modalBackground" />
                        <ajaxToolkit:RoundedCornersExtender ID="RoundedCornersExtender4" runat="server" TargetControlID="pnlRoundSave" Radius="6" Corners="All" />
                        <asp:Panel ID="pnlSave" runat="server" Width="370px" Height="100px" Style="display: none" CssClass="modalPopup">
                            <asp:Panel ID="pnlRoundSave" runat="server" Width="370" CssClass="modalPopup2">
                                <br />
                                <div align="center" style="text-align: center; height: 100%; padding: 0px 10px 0px 10px;">
                                    <table width="100%" border="0" cellpadding="5" cellspacing="5" style="border: solid 1px #5783BD">
                                        <tr style="background-color: #5783BD;">
                                            <td style="padding: 5px; font-size: 20px; color: #ffffff; font-weight: bold">Save Report
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divPopupMessage" runat="Server" visible="false">
                                                    <table width="100%" align="center">
                                                        <tr>
                                                            <td valign="top" align="center" width="20%">
                                                                <img id="Img2" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                                    alt="" />
                                                            </td>
                                                            <td valign="middle" align="left" width="80%" height="100%">
                                                                <asp:Label ID="lblPopupMessage" runat="Server" Font-Bold="true"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding: 10px" align="left">Report Name
                                                <asp:TextBox ID="txtReportName" Width="300px" runat="server" value="" ValidationGroup="SaveReport" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtReportName"
                                                    ErrorMessage=" *" ForeColor="Red" Font-Bold="true" ValidationGroup="SaveReport" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnPopupSaveReport" Text="Save" CssClass="button" ValidationGroup="SaveReport"
                                                    runat="server" OnClick="btnPopupSaveReport_Click" />
                                                <asp:Button ID="btnClose" Text="Cancel" CssClass="button" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </div>
                            </asp:Panel>

                        </asp:Panel>
                        <asp:DropDownList ID="drpdownReports" runat="server" Width="300px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="drpdownReports" ErrorMessage=" *" ForeColor="Red" Font-Bold="true" InitialValue='*'
                            ValidationGroup="savedRpts"></asp:RequiredFieldValidator>
                        &nbsp;<asp:Button ID="btnLoadReport" runat="server" CssClass="button"
                            OnClick="btnLoadReport_Click" Text="Load Report" ValidationGroup="savedRpts" />
                        &nbsp;
                         <asp:Button ID="btnDelReport" runat="server" CssClass="button"
                             OnClick="btnDelReport_Click" Text="Del Report" ValidationGroup="savedRpts" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                <tr>
                    <td colspan="3">
                        <table border="0" cellpadding="5" cellspacing="3" width="100%" align="center">
                            <tr>
                                <td>
                                    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Visible="false">
                                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Details">
                                            <ContentTemplate>
                                                <div id="div2" runat="Server" style="text-align: right">

                                                    <asp:Button ID="btnDownload" Text="Download" runat="server" CssClass="button" OnClick="btnDownload_Click"></asp:Button>
                                                    &nbsp;
                                                <asp:Button ID="btnDownloadDetails" runat="server" CssClass="buttonMedium" OnClick="btnDownloadDetails_Click"
                                                    Text="Download Details" />
                                                </div>
                                                <br />
                                                <asp:GridView ID="grdCrossTab" SkinID="skingridAuto" runat="server" Width="80%" AutoGenerateColumns="true"
                                                    Height="100%" ShowFooter="true" AllowPaging="false" Visible="true" OnRowDataBound="grdCrossTab_RowDataBound"
                                                    AllowSorting="False" DataKeyNames="Combination, Counts">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Counts" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                            ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                            FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkCount" runat="server" CommandName="download" CommandArgument='<%# Eval("Combination")  + "/" + Eval("Counts") + "/" + Eval("SelectedFilterNo") + "/" + Eval("SelectedOperation") + "/" + Eval("SuppressedFilterNo") + "/" + Eval("SuppressedOperation")%>'
                                                                    Text='<%# Eval("Counts")%>' Enabled='<%# Convert.ToInt32(Eval("Counts")) == 0 ? false : true %>'
                                                                    OnCommand="lnkReport_Command"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Summary">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnDownloadSummary" Text="Download" runat="server" CssClass="button"
                                                                OnClick="btnDownloadSummary_Click"></asp:Button>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" align="center" valign="top">
                                                            <asp:GridView ID="grdSummary" SkinID="skingridAuto" runat="server" Width="75%" AutoGenerateColumns="false"
                                                                Height="100%" ShowFooter="true" AllowPaging="false" Visible="true" AllowSorting="False">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Filters" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFilters" runat="server" Text='<%# Eval("key")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customers" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomers" runat="server" Text='<%# Eval("value")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total %" HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                                                        ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Top" FooterStyle-HorizontalAlign="center"
                                                                        FooterStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("TotalPercent")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td width="50%" align="center" valign="top">
                                                            <asp:Chart ID="chartSummary" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                                                                Height="296px" Width="412px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom"
                                                                BorderWidth="2" BorderColor="26, 59, 105" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                                                                <Titles>
                                                                    <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                                                        Name="Default" ForeColor="26, 59, 105">
                                                                    </asp:Title>
                                                                </Titles>
                                                                <Legends>
                                                                    <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                                                        Docking="Right" IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold"
                                                                        IsTextAutoFit="False" Name="Default">
                                                                    </asp:Legend>
                                                                </Legends>
                                                                <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                                                <Series>
                                                                    <asp:Series Name="Default" ChartType="Pie" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                        CustomProperties="PieDrawingStyle=Concave, MinimumRelativePieSize=20, CollectedLabel=Other"
                                                                        MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240">
                                                                    </asp:Series>
                                                                </Series>
                                                                <ChartAreas>
                                                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" Area3DStyle-Enable3D="false"
                                                                        BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent"
                                                                        BackGradientStyle="TopBottom">
                                                                        <AxisY2>
                                                                            <MajorGrid Enabled="False" />
                                                                            <MajorTickMark Enabled="False" />
                                                                        </AxisY2>
                                                                        <AxisX2>
                                                                            <MajorGrid Enabled="False" />
                                                                            <MajorTickMark Enabled="False" />
                                                                        </AxisX2>
                                                                        <Area3DStyle PointGapDepth="900" Rotation="162" IsRightAngleAxes="False" WallWidth="25"
                                                                            IsClustered="False" />
                                                                        <AxisY LineColor="64, 64, 64, 64">
                                                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                                                            <MajorTickMark Enabled="False" />
                                                                        </AxisY>
                                                                        <AxisX LineColor="64, 64, 64, 64">
                                                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                                                            <MajorTickMark Enabled="False" />
                                                                        </AxisX>
                                                                    </asp:ChartArea>
                                                                </ChartAreas>
                                                            </asp:Chart>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                    </ajaxToolkit:TabContainer>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

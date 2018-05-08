<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PerformanceByDayAndTimeReport.aspx.cs" Inherits="ecn.communicator.main.blasts.Report.PerformanceByDayAndTimeReport" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/main/ECNWizard/Content/layoutExplorer.ascx" TagName="layoutExplorer" TagPrefix="ecn" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            z-index: 10001 !important;
        }

        .modalPopupLayoutExplorer {
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
    <style type='text/css'>
        .ui-datepicker-trigger {
            position: relative;
            vertical-align: middle;
            padding-left: 5px;
        }
    </style>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtstartDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=txtendDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
        });
    </script>

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
    <asp:UpdatePanel ID="upMain" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
         <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>

            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr class="gradient">
                                <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Performance By Date and Time Report
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="offWhite borderSides" valign="top" colspan="2">
                        <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                            style="margin: 10px 0px">

                            <tr>
                                <td width="40%" align='right'>
                                    <b>Start Date&nbsp;:&nbsp;</b>
                                </td>
                                <td width="60%" align='left'>
                                    <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator
                                        ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                        ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align='right'>
                                    <b>End Date&nbsp;:&nbsp;</b>
                                </td>
                                <td align='left'>
                                    <asp:TextBox ID="txtendDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator
                                        ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                        ErrorMessage="« required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblFilterField1" Text="First Filter Field:" runat="server" />
                                </td>
                                <td style="text-align: left; max-width: 200px;">
                                    <asp:DropDownList ID="ddlFilterField1" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlFilterField1_SelectedIndexChanged">
                                        <asp:ListItem Value="-1" Text="-SELECT-" Selected="True" />
                                        <asp:ListItem Value="CampaignID" Text="Campaign" />
                                        <asp:ListItem Value="LayoutID" Text="Message" />
                                        <asp:ListItem Value="MessageTypeID" Text="Message Type" />
                                        <asp:ListItem Value="GroupID" Text="Group" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblFilterValue1" Text="Value:" runat="server" />
                                </td>
                                <td style="text-align: left; max-width: 200px;">
                                    <asp:Panel ID="pnlLayout" Visible="false" runat="server">

                                        <asp:ImageButton ID="imgSelectLayoutA" runat="server" CommandArgument="1" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutA_Click" Visible="true" />
                                        <asp:Label ID="lblSelectedLayoutA" runat="server" />
                                        <asp:HiddenField ID="hfSelectedLayoutA" runat="server" Value="" />

                                    </asp:Panel>
                                    <asp:Panel ID="pnlDropDown" Visible="false" runat="server">

                                        <asp:DropDownList ID="ddlFilterValue1" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterValue1_SelectedIndexChanged" Enabled="false" />

                                    </asp:Panel>
                                </td>
                            </tr>

                            <tr>


                                <td style="text-align: right;">
                                    <asp:Label ID="lblFilterField2" Text="Second Filter Field:" runat="server" />
                                </td>
                                <td style="text-align: left; max-width: 200px;">
                                    <asp:DropDownList ID="ddlFilterField2" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterField2_SelectedIndexChanged" Enabled="false" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="lblFilterValue2" Text="Value:" runat="server" />
                                </td>
                                <td style="text-align: left; max-width: 200px;">
                                    <asp:Panel ID="pnlLayout2" Visible="false" runat="server">

                                        <asp:ImageButton ID="imgSelectLayout2" runat="server" CommandArgument="2" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" CausesValidation="false" OnClick="imgSelectLayoutA_Click" Visible="true" />
                                        <asp:Label ID="lblSelectedLayout2" runat="server" />
                                        <asp:HiddenField ID="hfSelectedLayout2" runat="server" Value="" />
                                        <asp:HiddenField ID="hfWhichLayout" runat="server" Value="" />
                                    </asp:Panel>
                                    <asp:DropDownList ID="ddlFilterValue2" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterValue2_SelectedIndexChanged" Enabled="false" Visible="false" />
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="offWhite borderSides" colspan="2" align="center">
                        <asp:Button ID="btnReport" runat="Server" Text="Show Report" CssClass="formfield"
                            OnClick="btnReport_Click" Enabled="false"></asp:Button>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="offWhite borderSides">
                        <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                            style="margin: 10px 0px">
                            <tr>
                                <td width="85%" align="center">
                                    <asp:Chart ID="chtReportByFullWeek" runat="server" BackColor="Transparent" AntiAliasing="Graphics"></asp:Chart>
                                </td>
                            </tr>
                            <tr>
                                <td width="15%" align="center">
                                    <asp:Label ID="lblLineOrColumn" runat="server" Visible="false" Width="125px" Text="Graph type:" Style="font-size: 14px"></asp:Label>
                                    <asp:DropDownList ID="ddlLineOrColumn" runat="server" AutoPostBack="true" Enabled="false" Visible="false" Width="75px" class="offWhite borderSides" colspan="1" align="center" OnSelectedIndexChanged="ddlLineOrColumn_SelectedIndexChanged">
                                        <asp:ListItem Selected="true" Text="Column" />
                                        <asp:ListItem Text="Line" />
                                    </asp:DropDownList>
                                    <asp:Label ID="lblOpensOrClicks" runat="server" Visible="false" Width="125px" Text="Opens or Clicks:" Style="font-size: 14px"></asp:Label>
                                    <asp:DropDownList ID="ddlOpensOrClicks" runat="server" AutoPostBack="true" Enabled="false" Visible="false" Width="75px" class="offWhite borderSides" colspan="1" align="center" OnSelectedIndexChanged="ddlOpensOrClicks_SelectedIndexChanged">
                                        <asp:ListItem Text="Opens" />
                                        <asp:ListItem Text="Clicks" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td class="offWhite borderSides" colspan="2">
                <br />
                <hr />
                <br />
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides" align="left" style="padding-left: 100px" colspan="2">
                <asp:DropDownList ID="ddlDay" runat="server" Width="100" Enabled="true" Visible="false" class="offWhite borderSides" colspan="1">
                    <asp:ListItem Text="Monday" Value="mon" />
                    <asp:ListItem Text="Tuesday" Value="tue" />
                    <asp:ListItem Text="Wednesday" Value="wed" />
                    <asp:ListItem Text="Thursday" Value="thur" />
                    <asp:ListItem Text="Friday" Value="fri" />
                    <asp:ListItem Text="Saturday" Value="sat" />
                    <asp:ListItem Text="Sunday" Value="sun" />
                </asp:DropDownList>
                <asp:Button ID="btnRefreshCharts" Text="Update Chart" runat="server" Visible="false" OnClick="refreshCharts" />
                <asp:Label ID="lblDayArea" runat="server" Visible="false" Width="400" Text="Select a day of the week to examine its open and click activity." Style="font-size: 14px; padding-left: 25px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides" colspan="2">
                <br />
            </td>
        </tr>
        <tr>
            <td class="offWhite borderSides" align="right">
                <asp:GridView ID="gvClicksByDay" runat="server" CssClass="grid" Height="250px" Width="200px" align="center" AutoGenerateColumns="false" AllowPaging="false">
                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                    <Columns>
                        <asp:BoundField DataField="Times" HeaderText="Time" ItemStyle-Width="33%"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                        <asp:BoundField DataField="Opens" HeaderText="Open %" ItemStyle-Width="33%"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                        <asp:BoundField DataField="Clicks" HeaderText="Click %" ItemStyle-Width="33%" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
            <td align="center" class="offWhite borderSides" width="545px" height="250px">
                <asp:Chart ID="chtReportByDay" runat="server" BackColor="Transparent"></asp:Chart>
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle" colspan="2" class="offWhite borderSides">
                <asp:Label ID="lblExport" Text="Download As:" runat="server" Visible="false" Style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding-top: 0px;"></asp:Label>
                <asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server" Visible="false">
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>Excel</asp:ListItem>
                    <asp:ListItem>Word</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnDownloadReport" Text="Download Report" runat="server" Visible="false" OnClick="btnDownloadReport_Click" />
            </td>
        </tr>

        <tr>
            <td class="offWhite borderSides" colspan="2" align="center">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" Width="100%" ShowZoomControl="false" ShowRefreshButton="false" ShowPrintButton="False" ShowExportControls="False">
                </rsweb:ReportViewer>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="gradient" colspan="2">&nbsp;
            </td>
        </tr>
    </table>
    <asp:Button ID="hfLayoutExplorer" style="display:none;" runat="server" />
    <ajaxToolkit:ModalPopupExtender ID="mpeLayoutExplorer" PopupControlID="pnlLayoutExplorer" BackgroundCssClass="modalBackground" TargetControlID="hfLayoutExplorer" CancelControlID="btnCloseLayoutExplorer" runat="server" />
    <asp:UpdatePanel ID="pnlLayoutExplorer" CssClass="modalPopupLayoutExplorer" UpdateMode="Always" ChildrenAsTriggers="true" runat="server">
        <ContentTemplate>


            <table style="background-color: white;">
                <tr>
                    <td>
                        <ecn:layoutExplorer ID="layoutExplorer" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="btnCloseLayoutExplorer" runat="server" OnClick="btnCloseLayoutExplorer_Click" Text="Close" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible; position: absolute; top: -500px"
        name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
        frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:Content>

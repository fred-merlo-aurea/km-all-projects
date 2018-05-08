<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterSuppressionSourceReport.aspx.cs"
    Inherits="ecn.communicator.blasts.reports.MasterSuppressionSourceReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register TagPrefix="ajax" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            $("#<%=txtStartDate.ClientID%>").datepicker({
                showOn: "button",
                buttonImage: "/ecn.images/images/icon-calendar.gif",
                buttonImageOnly: true,
                buttonText: "Select date",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true
            });
            $("#<%=txtEndDate.ClientID%>").datepicker({
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
    <%--  <script type="text/javascript">
        $(function () {
            var newOption = "<option value='" + "-1" + "'>All</option>";
            $("[id*='ddlMasterSuppression']").prepend(newOption);
            $("[id*='ddlMasterSuppression']")[0].selectedIndex = 0;
        });
    </script>--%>
    <asp:UpdatePanel ID="DetailUpdatePanel" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="btnReport" />
            <asp:AsyncPostBackTrigger ControlID="ddlMasterSuppression" EventName="SelectedIndexChanged" />
        </Triggers>
        <ContentTemplate>
            <br />
            <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td>
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
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr class="gradient">
                                <td width="50%" valign="middle" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Master Suppression Source Report
                                </td>
                                <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">
                                    <asp:Panel ID="pnlDownload" runat="server">
                                        Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                            <asp:ListItem Value="xls">XLS</asp:ListItem>
                                            <asp:ListItem Value="xlsdata">XLSDATA</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnReport" runat="Server" Text="Download" CssClass="formfield"
                                            OnClick="btnReport_Click"></asp:Button>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" valign="top" colspan="2">
                                    <asp:UpdateProgress ID="upMainProgress" runat="server" DisplayAfter="10" Visible="true"
                                        AssociatedUpdatePanelID="DetailUpdatePanel" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <asp:Panel ID="upMainProgressP1" CssClass="overlay" runat="server">
                                                <asp:Panel ID="upMainProgressP2" CssClass="loader" runat="server">
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

                                    <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                        style="margin: 10px 0px">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlWeek" runat="server" Visible="true">
                                                    <asp:TextBox ID="txtStartDate" Width="65" CssClass="formfield" MaxLength="10" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                                    <asp:TextBox ID="txtEndDate" Width="65" runat="server" CssClass="formfield" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;
                                                     <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="formfield" />
                                                    <br />
                                                    <asp:Label ID="lblRequired" runat="server" Text="Dates required" ForeColor="Red" Visible="false" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <ecnCustom:ecnGridView ID="MasterSuppressionSource" runat="server"
                                                    AutoGenerateColumns="false"
                                                    CssClass="grid"
                                                    DataKeyNames="GroupId"
                                                    SelectedRowStyle-BackColor="LightGray">
                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-Width="150px" DataField="UnsubscribeCode" HeaderText="Suppression Code" />
                                                        <asp:BoundField ItemStyle-Width="150px" DataField="Count" HeaderText="Count" />
                                                    </Columns>
                                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                </ecnCustom:ecnGridView>
                                                <p />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlDetails" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>Filter by Suppression Code
                                                                <asp:DropDownList ID="ddlMasterSuppression" runat="server" CssClass="formfield" DataValueField="UnsubscribeCodeID" DataTextField="UnsubscribeCode" OnSelectedIndexChanged="ddlMasterSuppression_OnSelectedIndexChanged" AutoPostBack="true" EnableViewState="true" Width="150px"></asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <ecnCustom:ecnGridView ID="MasterSuppressionSourceDetail" runat="server"
                                                                    AutoGenerateColumns="false"
                                                                    CssClass="grid"
                                                                    Visible="false"
                                                                    AllowSorting="true"
                                                                    OnSorting="MasterSuppressionSourceDetail_Sorting"
                                                                    AllowPaging="true"
                                                                    OnPageIndexChanging="MasterSuppressionSourceDetail_PageIndexChanging">
                                                                    <HeaderStyle CssClass="gridheader"></HeaderStyle>
                                                                    <Columns>
                                                                        <asp:BoundField ItemStyle-Width="150px" DataField="EmailAddress" HeaderText="Email" SortExpression="EmailAddress" />
                                                                        <asp:BoundField HeaderText="Suppressed Date Time" DataField="UnsubscribeDateTime" SortExpression="UnsubscribeDateTime" ItemStyle-Width="150px"></asp:BoundField>
                                                                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                                    </Columns>
                                                                    <FooterStyle CssClass="tableHeader1"></FooterStyle>
                                                                    <AlternatingRowStyle CssClass="gridaltrow" />
                                                                </ecnCustom:ecnGridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" border="0" width="50%">
                                                                    <tr>
                                                                        <td style="text-align: center;" class="label" style="width: 100%;">
                                                                            <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" Visible="false"
                                                                                class="formbuttonsmall" Text="<< Previous" />
                                                                            <asp:PlaceHolder ID="plPager" runat="server" Visible="false">Page
                                                                <asp:Label ID="lblCurrentPage" runat="server" class="label" />
                                                                                of
                                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                                                            </asp:PlaceHolder>
                                                                            <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>

                                        </tr>

                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                                    </rsweb:ReportViewer>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="gradient" colspan="2">&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

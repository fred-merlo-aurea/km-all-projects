<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="UnsubscribeReasonReport.txt.cs" Inherits="ecn.communicator.main.blasts.Report.UnsubscribeReasonReport" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register TagPrefix="ajax" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <br />
    <table id="title" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td valign="middle" style="padding-right: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding-top: 0px">&nbsp;Unsubscribe Reason Report
            </td>
        </tr>
    </table>
    <br />
    <table id="idMain" cellspacing="0" cellpadding="0" style="padding: 10px;" width="100%" align="center" border="0">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                        <td align="center" valign="bottom">&nbsp;<asp:RequiredFieldValidator ID="rfvFromDate" ControlToValidate="txtFromDate" ErrorMessage="Required" Display="Dynamic" ValidationGroup="search" runat="server" />
                            <ajax:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate" />
                        </td>
                        <td align="center" valign="bottom">&nbsp;<asp:RequiredFieldValidator ID="rfvToDate" ControlToValidate="txtToDate" ErrorMessage="Required" Display="Dynamic" ValidationGroup="search" runat="server" />
                            <ajax:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" />
                        </td>
                    </tr>
                    <tr>
                        <td>Search By
                        </td>
                        <td>Search Text
                        </td>
                        <td>From
                        </td>
                        <td>To
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlSearchBy" runat="server">
                                <asp:ListItem Text="--Select--" Value="" />
                                <asp:ListItem Text="Group Name" Value="group" />
                                <asp:ListItem Text="Email Subject" Value="emailsubject" />
                                <asp:ListItem Text="Campaign Item" Value="campaignitem" />
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearchCriteria" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" />
                            <%--<asp:RequiredFieldValidator ID="rfvFromDate" ControlToValidate="txtFromDate" ErrorMessage="*" Display="Dynamic" ValidationGroup="search" runat="server" />
                            <ajax:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate" />--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" runat="server" />
                            <%--<asp:RequiredFieldValidator ID="rfvToDate" ControlToValidate="txtToDate" ErrorMessage="*" Display="Dynamic" ValidationGroup="search" runat="server" />
                            <ajax:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" />--%>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td valign="middle" style="font: bold 11px Arial, Helvetica, sans-serif; color: #333">Summary
            </td>
        </tr>
        <tr>

            <td style="text-align: left;">
                <br />
                <ecnCustom:ecnGridView ID="gvSummary" runat="server" CssClass="gridWizard" Width="25%" AutoGenerateColumns="false">
                    <FooterStyle CssClass="tableHeader1" />
                    <AlternatingRowStyle CssClass="gridaltrowWizard"></AlternatingRowStyle>
                    <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
                    <Columns>
                        <asp:BoundField DataField="SelectedReason" HeaderText="Reason" />
                        <asp:BoundField DataField="UniqueCount" HeaderText="Unique Count" />
                        <asp:BoundField DataField="TotalCount" HeaderText="Total Count" />
                    </Columns>
                </ecnCustom:ecnGridView>
                <br />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDetail" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td valign="middle" style="font: bold 11px Arial, Helvetica, sans-serif; color: #333">Detail
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 80%; text-align: left;">Filter By Reason 
                                            <asp:DropDownList ID="ddlReason" runat="server" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Selected="True">All</asp:ListItem>
                                                <asp:ListItem>Email frequency</asp:ListItem>
                                                <asp:ListItem>Email volume</asp:ListItem>
                                                <asp:ListItem>Content not relevant</asp:ListItem>
                                                <asp:ListItem>Signed up for one-time email</asp:ListItem>
                                                <asp:ListItem>Circumstances changed(moved, married, changed jobs, etc.)</asp:ListItem>
                                                <asp:ListItem>Prefer to get information another way</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:DropDownList ID="ddlFormat" runat="server">
                                                <asp:ListItem Text="XLS" Value="xls" Selected="True" />
                                                <asp:ListItem Text="XLSDATA" Value="xlsdata" />
                                                <asp:ListItem Text="PDF" Value="pdf" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <ecnCustom:ecnGridView ID="gvReasonDetails" CssClass="gridWizard" Width="100%" runat="server" AutoGenerateColumns="false" PageSize="20">
                                    <FooterStyle CssClass="tableHeader1" />
                                    <AlternatingRowStyle CssClass="gridaltrowWizard"></AlternatingRowStyle>
                                    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                                    <Columns>
                                        <asp:BoundField DataField="CampaignItemName" ItemStyle-Width="20%" HeaderText="Campaign Item" />
                                        <asp:BoundField DataField="EmailSubject" ItemStyle-Width="20%" HeaderText="Email Subject" />
                                        <asp:BoundField DataField="GroupName" ItemStyle-Width="10%" HeaderText="Group" />
                                        <asp:BoundField DataField="EmailAddress" ItemStyle-Width="20%" HeaderText="Email" />
                                        <asp:BoundField DataField="UnsubscribeTime" ItemStyle-Width="10%" HeaderText="Time" />
                                        <asp:BoundField DataField="SelectedReason" ItemStyle-Width="20%" HeaderText="Reason" />
                                    </Columns>
                                </ecnCustom:ecnGridView>
                            </td>
                        </tr>

                        <%--  <tr>
                        <td>
                            <table cellpadding="0" border="0" width="50%">
                                <tr>
                                    <td style=";" class="label" width= "100%;">
                                      text-align: center
                                        <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" Visible="true"
                                                                    class="formbuttonsmall" Text="<< Previous" />
                                            <asp:PlaceHolder ID="plPager" runat="server" Visible="true">Page
                                                                    <asp:Label ID="lblCurrentPage" runat="server" class="label" />
                                                                    of
                                                                    <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                            </asp:PlaceHolder>
                                        <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" Visible="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                        <tr>
                            <td align="right">
                                <asp:Panel ID="pnlPager" runat="server" Visible="true">
                                    <table cellpadding="0" border="0" width="100%">
                                        <tr>
                                            <td class="label">
                                                <div style="float: left; padding-right: 114px">
                                                    Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                                </div>
                                                <%-- <div style="float: left;">
                                                Show Rows:
                                                <asp:DropDownList ID="ddlPageSizeContent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ContentGrid_SelectedIndexChanged" CssClass="formfield">
                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                </asp:DropDownList>    
                                            </div>--%>
                                                <div style="float: right">
                                                    Page
                                                  <asp:Label ID="lblCurrentPage" runat="server" class="label" />
                                                    <%-- <asp:TextBox ID="txtGoToPageContent" runat="server" AutoPostBack="true" OnTextChanged="GoToPageContent_TextChanged" class="formtextfield" Width="30px" />--%>
                                                of
                                                <asp:Label ID="lblTotalNumberOfPagesGroup" runat="server" CssClass="label" />
                                                    &nbsp;
                                                <asp:Button ID="btnPreviousGroup" runat="server" ToolTip="Previous Page" OnClick="btnPreviousGroup_Click" class="formbuttonsmall" Text="<< Previous" />
                                                    <asp:Button ID="btnNextGroup" runat="server" ToolTip="Next Page" OnClick="btnNextGroup_Click" class="formbuttonsmall" Text="Next >>" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>

                        <rsweb:ReportViewer ID="report" Width="100%" Visible="false" runat="server" />
                        <tr></tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>

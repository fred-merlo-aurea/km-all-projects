<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AudienceEngagementReport.aspx.cs"
    Inherits="ecn.communicator.main.lists.reports.AudienceEngagementReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:UpdatePanel ID="upMain" runat="server" ChildrenAsTriggers="true">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>

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
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr class="gradient">
                                <td width="50%" valign="middle" align="left" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Audience Engagement Report
                                </td>
                                <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                    <asp:ListItem Value="html" Selected="true">HTML</asp:ListItem>
                                    <asp:ListItem Value="pdf">PDF</asp:ListItem>
                                    <asp:ListItem Value="xls">XLS</asp:ListItem>
                                    <asp:ListItem Value="xlsdata">XLSDATA</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" valign="top" colspan="2" align="left">
                                    <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                        style="margin: 10px 0px">
                                        <tr>
                                            <td width="30%" align='right'>
                                                <b>Group / List&nbsp;:&nbsp;</b><asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
                                            </td>
                                            <td width="70%">
                                                <asp:Label ID="lblSelectGroupName" runat="server" Text="-No Group Selected-" Font-Size="Small"></asp:Label>
                                                <asp:HiddenField ID="hfGroupSelectionMode" runat="server" Value="None" />
                                                <asp:HiddenField ID="hfSelectGroupID" runat="server" Value="0" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'></td>
                                            <td>
                                                <b>Up to 1 year of statistics history is available.</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'>
                                                <b>Click %&nbsp;:&nbsp;</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClickPercent" runat="Server" Width="50" CssClass="formfield"
                                                    Text="35"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtClickPercent" runat="Server" Font-Size="xx-small"
                                                    ControlToValidate="txtClickPercent" ErrorMessage="<< required" Font-Italic="True"
                                                    Font-Bold="True"></asp:RequiredFieldValidator>
                                                &nbsp;
                                                <asp:RangeValidator ID="rvtxtClickPercent" runat="server" ControlToValidate="txtClickPercent"
                                                    Font-Italic="True" Font-Bold="True" Font-Size="XX-Small" ErrorMessage="Percentage must be between 1 and 100"
                                                    MaximumValue="100" MinimumValue="1" Type="Integer"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'>
                                                <b>Days&nbsp;:&nbsp;</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDays" runat="Server" Width="50" CssClass="formfield" Text="120"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtDays" runat="Server" Font-Size="xx-small" ControlToValidate="txtDays"
                                                    ErrorMessage="<< required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>
                                                &nbsp;
                                                <asp:RangeValidator ID="rvtxtDays" runat="server" ControlToValidate="txtDays" Font-Italic="True"
                                                    Font-Bold="True" Font-Size="XX-Small" ErrorMessage="Days must be between 1 and 365"
                                                    MaximumValue="365" MinimumValue="1" Type="Integer"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="offWhite borderSides" style="text-align: right;">
                                                <b>Download Only&nbsp;:&nbsp;</b>
                                            </td>
                                            <td class="offWhite borderSides">
                                                <ecn:groupexportudfsettings ID="groupexportUDFsettings" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <asp:Button ID="btnReport" runat="Server" Text="Show Report" CssClass="formfield"
                                        OnClick="btnReport_Click"></asp:Button>
                                    <br>
                                    <br>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <hr />
                                    <br />
                                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Width="95%" ShowRefreshButton="false" ShowExportControls="False" ShowPrintButton="False" ShowZoomControl="false">
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
            <br>
            <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

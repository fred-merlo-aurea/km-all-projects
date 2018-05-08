<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groupStatisticsReport.aspx.cs"
    Inherits="ecn.communicator.main.lists.reports.groupStatisticsReport" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            z-index: 10001 !important;
        }

        .modalPopupFull {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 100%;
            height: 100%;
            overflow: auto;
        }

        .modalPopupLayoutExplorer {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .modalPopupGroupExplorer {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .modalPopupImport {
            background-color: #D0D0D0;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            height: 60%;
            overflow: auto;
        }

        .buttonMedium {
            width: 135px;
            background: url(buttonMedium.gif) no-repeat left top;
            border: 0;
            font-weight: bold;
            color: #ffffff;
            height: 20px;
            cursor: pointer;
            padding-top: 2px;
        }

        .TransparentGrayBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            height: 100%;
            width: 100%;
            min-height: 100%;
            min-width: 100%;
        }

        fieldset {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }

        .overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
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
    <asp:UpdatePanel ID="upMain" ChildrenAsTriggers="true" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>

            <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td colspan="4">
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
                            <br />
                        </asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr class="gradient">
                                <td width="50%" valign="middle" align="left" style="border-right: medium none; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: #a4a2a3 1px solid; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">&nbsp;Group Statistics Report
                                </td>
                                <td width="50%" align='right' valign="middle" style="border-right: #a4a2a3 1px solid; padding-right: 5px; border-top: #a4a2a3 1px solid; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; border-left: medium none; color: #333; padding-top: 0px; border-bottom: #a4a2a3 1px solid">Download as:&nbsp;<asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
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
                                                <b>Group / List&nbsp;:&nbsp;</b>
                                                <asp:ImageButton ID="imgSelectGroup" runat="server" ImageUrl="/ecn.communicator/main/dripmarketing/images/configure_diagram.png" OnClick="imgSelectGroup_Click" Visible="true" />
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
                                            <td width="30%" align='right'>
                                                <b>Start Date&nbsp;:&nbsp;</b>
                                            </td>
                                            <td width="70%">
                                                <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                                &nbsp;<asp:RequiredFieldValidator
                                                    ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                                    ErrorMessage="<< required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align='right'>
                                                <b>End Date&nbsp;:&nbsp;</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtendDate" runat="Server" Width="80" CssClass="formfield"></asp:TextBox>
                                                &nbsp;<asp:RequiredFieldValidator
                                                    ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                                    ErrorMessage="<< required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:CheckBox ID="chkDetails" AutoPostBack="false" Text="Show Browser Details" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="offWhite borderSides" colspan="2" align="center">
                                    <asp:Button ID="btnReport" runat="Server" Text="Show Report" CssClass="formfield"
                                        OnClick="btnReport_Click" CausesValidation="true"></asp:Button>
                                    <br>
                                    <br>
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
            <br>
            <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

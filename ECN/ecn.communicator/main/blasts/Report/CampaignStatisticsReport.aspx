<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="CampaignStatisticsReport.aspx.cs" Inherits="ecn.communicator.main.blasts.Report.CampaignStatisticsReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/main/ECNWizard/Group/groupsLookup.ascx" TagName="groupsLookup" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        .btnDownload {
            position: relative;
            left: 35%;
            background-color: #F0F0F0;
            font-family: Arial;
            height: 120%;
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
    <table id="title" cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
        <tr>
            <td valign="middle" style="padding-right: 5px; padding-left: 5px; padding-bottom: 0px; font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding-top: 0px">&nbsp;Campaign Statistics Report
            </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
        <table cellspacing="0" cellpadding="0" width="674" align="center">
            <tr>
                <td id="errorTop"></td>
            </tr>
            <tr>
                <td id="errorMiddle">
                    <table width="80%">
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
        <br />
    </asp:PlaceHolder>
    <br />
    <asp:UpdatePanel ID="upMain" ChildrenAsTriggers="true" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>
        <ContentTemplate>
            <table id="idMain" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                <tr>
                    <td>
                        <table cellspacing="5" cellpadding="2" width="60%" align="left">
                            <tr>
                                <td style="text-align: right;">
                                    <b>Campaign&nbsp;:&nbsp;</b>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlCampaigns" runat="server" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>Up to 1 year of statistics history is available.
                                </td>
                            </tr>
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
                                <td width="30%" align='right' style="vertical-align: middle;">
                                    <b>Start Date&nbsp;:&nbsp;</b>
                                </td>
                                <td width="70%">
                                    <asp:TextBox ID="txtstartDate" runat="Server" Width="80" CausesValidation="true" CssClass="formfield"></asp:TextBox>

                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbeStartDate" runat="server" TargetControlID="txtstartDate" ValidChars="0123456789/" FilterMode="ValidChars" />
                                    &nbsp;<asp:RequiredFieldValidator
                                        ID="rfv1" runat="Server" Font-Size="xx-small" ControlToValidate="txtstartDate"
                                        ErrorMessage="<< required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                                <br />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:RegularExpressionValidator ID="regFrom" ControlToValidate="txtstartDate" ValidationExpression="(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))" ErrorMessage="Start Date Invalid.  Correct format is MM/DD/YYYY." runat="server" ValidateRequestMode="Disabled" Display="Dynamic" />
                                </td>
                            </tr>
                            <tr>
                                <td align='right' style="vertical-align: middle;">
                                    <b>End Date&nbsp;:&nbsp;</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtendDate" runat="Server" Width="80" CausesValidation="true" CssClass="formfield"></asp:TextBox>

                                    <ajaxToolkit:FilteredTextBoxExtender ID="ftbeEndDate" runat="server" TargetControlID="txtendDate" ValidChars="0123456789/" FilterMode="ValidChars" />
                                    &nbsp;<asp:RequiredFieldValidator
                                        ID="rfv2" runat="Server" Font-Size="xx-small" ControlToValidate="txtendDate"
                                        ErrorMessage="<< required" Font-Italic="True" Font-Bold="True"></asp:RequiredFieldValidator>&nbsp;
                                                <br />

                                    <%--<asp:RangeValidator ID="rvEndDate" runat="server" ControlToValidate="txtendDate"
                                            Font-Italic="True" Font-Bold="True" Font-Size="XX-Small"></asp:RangeValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:RegularExpressionValidator ID="regTo" ControlToValidate="txtendDate" ValidationExpression="(?<Month>\d{1,2})/(?<Day>\d{1,2})/(?<Year>(?:\d{4}|\d{2}))" ErrorMessage="End Date Invalid.  Correct format is MM/DD/YYYY." runat="server" ValidateRequestMode="Disabled" Display="Dynamic" />

                                </td>
                            </tr>
                            <tr>
                                <td width="50%" valign="middle" style="text-align: right;">&nbsp;<b>Download format:</b>
                                </td>
                                <td width="50%" valign="middle" style="text-align: left; font: bold 13px Arial, Helvetica, sans-serif;">
                                    <asp:DropDownList ID="drpExport" CssClass="formlabel" runat="Server">
                                        <asp:ListItem Value="pdf">PDF</asp:ListItem>
                                        <asp:ListItem Value="xls" Selected="True">XLS</asp:ListItem>
                                        <asp:ListItem Value="xlsdata">XLSDATA</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button ID="btnReport" runat="Server" Text="Download" CssClass="formfield btnDownload" CausesValidation="true"
                                        OnClick="btnReport_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False" ShowRefreshButton="false">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
            <uc1:groupsLookup ID="ctrlgroupsLookup1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

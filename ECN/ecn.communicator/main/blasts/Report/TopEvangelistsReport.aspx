<%@ Page Inherits="ecn.communicator.main.blasts.Report.TopEvangelistsReport" Title="Title" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Communicator.Master" CodeBehind="TopEvangelistsReport.aspx.cs" %>
<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls, Version=5.0.3119.50, Culture=neutral, PublicKeyToken=6e8eac0d38e90b9b" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .errorClass { border: 1px solid red; }

         #FolderType img {
             left: -3px;
             position: relative;
             top: 2px;
         }

        #divPage { display: none; }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: #D0D0D0;
            border-color: Gray;
            border-style: solid;
            border-width: 3px;
            padding: 3px;
        }

        .TransparentGrayBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            height: 100%;
            left: 0;
            min-height: 100%;
            min-width: 100%;
            opacity: 0.7;
            position: fixed;
            top: 0;
            width: 100%;
        }

        .overlay {
            -moz-opacity: 0.70;
            background-color: gray;
            filter: Alpha(Opacity=70);
            height: 100%;
            left: 0px;
            opacity: 0.70;
            position: fixed;
            top: 0px;
            width: 100%;
            z-index: 99;
        }

        * html .overlay {
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            position: absolute;
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
            background-color: #F4F3E1;
            border: solid 2px Black;
            color: black;
            font-size: x-small;
            left: 50%;
            margin-left: -60px;
            position: fixed;
            top: 40%;
            width: 120px;
            z-index: 100;
        }

        * html .loader {
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
            position: absolute;
        }

        .rowSpacer {
            padding-bottom: 16px
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#<%=ReportGrid.ClientID%> tr:even tr:not(:first-child)").addClass("gridaltrow");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type='text/css'>
        .ui-datepicker-trigger { position: relative; vertical-align:middle; padding-left:5px; }
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
    <br />
       <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
            <table cellspacing="0" cellpadding="0" width="674" align="center">
                <tr>
                    <td id="errorTop">
                    </td>
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
                    <td id="errorBottom">
                    </td>
                </tr>
            </table>
    </asp:PlaceHolder>
    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="margin: 0 0 0 0;">
        <tr>
            <td style="padding: 0px 10px 0px 10px;">
            <%--<td style="padding: 10px;">--%>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="gradient">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="formLabel" style="padding: 0 5px;">
                                        <strong>Top Evangelists Report:</strong>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="greySidesB offWhite" align="left">
                            <div style="padding: 10px;">
                                <div style="width: 100%;">
                                    <!-- start new design-->
                                    <div style="float: left;text-align: right; width: 200px" class="rowSpacer">
                                        <label class="tableHeader" style="padding-right: 8px">Start Date:</label><asp:textbox ID="txtStartDate" runat="server" Width="65" CssClass="formfield" MaxLength="10"></asp:textbox>
                                    </div>
                                    <div style="float: left;text-align: right; width: 200px" class="rowSpacer">
                                        <label class="tableHeader" style="padding-right: 8px">End Date:</label><asp:textbox ID="txtEndDate" runat="server" Width="65" CssClass="formfield" MaxLength="10"></asp:textbox>
                                    </div>
                                    
                                    <div style="float: left; padding-left: 26px">
                                        <asp:Button ID="btnLoadCampaignItems" runat="server" Text="Load Campaign Items" OnClick="btnLoadCampaignItems_Click"/>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div id="lowerFilters" style="display: none" runat="server">
                                        <div style="float: left" class="rowSpacer">
                                            <label class="tableHeader">Campaign Item:</label>
                                        </div>
                                        <div style="float: left; padding-left: 12px" class="rowSpacer">
                                            <asp:dropdownlist ID="ddlCampaignItem" runat="server"></asp:dropdownlist>
                                        </div>
                                        <div style="clear: both"></div>
                                        <div >
                                            <div style="float: left;">
                                                <asp:checkbox ID="cbxFacebook" Checked="True" runat="server"/><asp:label runat="server" AssociatedControlId="cbxFacebook" class="tableHeader">Facebook</asp:label>    
                                            </div>
                                            <div style="float: left; padding-left: 12px">
                                                <asp:checkbox ID="cbxTwitter" Checked="True" runat="server"/><asp:label runat="server" AssociatedControlId="cbxTwitter" class="tableHeader">Twitter</asp:label>    
                                            </div>
                                            <div style="float: left; padding-left: 12px">
                                                <asp:checkbox ID="cbxLinkedIn" Checked="True" runat="server"/><asp:label runat="server" AssociatedControlId="cbxLinkedIn" class="tableHeader">LinkedIn</asp:label>
                                            </div>
                                            <div style="float: left; padding-left: 12px">
                                                <asp:checkbox ID="cbxForwardToFriend" Checked="True" runat="server"/><asp:label runat="server" AssociatedControlId="cbxForwardToFriend" class="tableHeader">Forward to a Friend</asp:label>    
                                            </div>
                                        </div>
                                        <div style="clear: both"></div><br/>
                                        <div style="text-align: center">
                                            <asp:Button ID="btnShowReport" runat="server" Text="Show Report" OnClick="ShowReport_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="padding-left: 10px; padding-right: 10px">
        <asp:Panel ID="pnlReport" runat="server" Visible="false">
            <table  style="border-right: #dde4e8 2px solid; border-top: #dde4e8 2px solid; border-left: #dde4e8 2px solid; border-bottom: #dde4e8 2px solid" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="gridheader">
                    <td style="padding-right: 4px; padding-left: 8px; padding-bottom: 4px; width: 60%; padding-top: 4px" align="left">
                        <span style="font-weight: bold; font-size: 8pt; color: #000000; font-style: normal;font-family: Arial">Report</span>
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="drpExport" Width="100" runat="server" Font-Size="XX-Small">
                            <asp:ListItem Selected="true" Value="pdf">PDF</asp:ListItem>
                            <asp:ListItem Value="xls">Excel</asp:ListItem>
                            <asp:ListItem Value="XLSData">XLSData</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download" Font-Size="XX-Small" OnClick="btnDownload_Click"></asp:Button>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding: 10px 10px 10px 10px;">
                        <asp:DataGrid ID="ReportGrid" runat="Server" Width="100%" AutoGenerateColumns="False" CssClass="gridWizard" >
                            <FooterStyle CssClass="tableHeader1"></FooterStyle>
                            <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
                            <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
                        </asp:DataGrid>
                        <AU:PagerBuilder ID="ReportGridPager" 
                            runat="Server" Width="100%" PageSize="25" ControlToPage="ReportGrid">
                            <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                        </AU:PagerBuilder>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False"  ShowRefreshButton="false">
                        </rsweb:ReportViewer>
                    </td>
                </tr>
            </table>
        </asp:Panel>    
    </div>
</asp:Content>
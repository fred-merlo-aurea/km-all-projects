<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingReport.aspx.cs"
    Inherits="ecn.accounts.main.reports.BillingReport" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 
<table id="idMain" width="100%" cellspacing="0" cellpadding="0" border='0' align="center">
    <tr>
        <td>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="False">
            </rsweb:ReportViewer>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                <tr class="gradient">
                    <td width="50%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                        font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;" align="left">
                        &nbsp;Billing&nbsp;
                    </td>
                    <td width="50%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none;
                        font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                        Download as:&nbsp;<asp:dropdownlist id="drpExport" cssclass="formlabel" runat="Server">
								<asp:ListItem value="pdf" selected="true">PDF</asp:ListItem>
								<asp:ListItem value="xls">XLS</asp:ListItem>
							</asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td class="offWhite borderSides">
                        <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                            style="margin: 10px 0px">
                            <tr>
                                <td width="30%" align='right'>
                                    <b>Channel&nbsp;:&nbsp;</b>
                                </td>
                                <td width="70%" align="left">
                                    <asp:dropdownlist id="drpChannel" runat="Server" cssclass="formlabel" width="200px"
                                        autopostback="True" onselectedindexchanged="drpChannel_SelectedIndexChanged"></asp:dropdownlist>
                                </td>
                            </tr>
                            <tr>
                                <td align='right' valign="top">
                                    <b>Customer&nbsp;:&nbsp;</b>
                                </td>
                                <td align="left">
                                    <asp:listbox id="lstCustomer" runat="Server" width="200" height="200px" rows="10"
                                        selectionmode="Multiple" cssclass="formfield"></asp:listbox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="offWhite borderSides" valign="top">
                        <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                            style="margin: 10px 0px">
                            <tr>
                                <td width="50" align='right'>
                                    <b>Month&nbsp;:&nbsp;</b>
                                </td>
                                <td align="left">
                                    <asp:dropdownlist cssclass="formlabel" id="drpMonth" runat="Server">
											<asp:ListItem value="1">January</asp:ListItem>
											<asp:ListItem value="2">February</asp:ListItem>
											<asp:ListItem value="3">March</asp:ListItem>
											<asp:ListItem value="4">April</asp:ListItem>
											<asp:ListItem value="5">May</asp:ListItem>
											<asp:ListItem value="6">June</asp:ListItem>
											<asp:ListItem value="7">July</asp:ListItem>
											<asp:ListItem value="8">August</asp:ListItem>
											<asp:ListItem value="9">September</asp:ListItem>
											<asp:ListItem value="10">October</asp:ListItem>
											<asp:ListItem value="11">November</asp:ListItem>
											<asp:ListItem value="12">December</asp:ListItem>
										</asp:dropdownlist>
                                </td>
                            </tr>
                            <tr>
                                <td align='right'>
                                    <b>Year&nbsp;:&nbsp;</b>
                                </td>
                                <td align="left">
                                    <asp:dropdownlist id="drpYear" cssclass="formlabel" runat="Server"></asp:dropdownlist>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="offWhite borderSides" colspan="2" align="center">
                        <asp:button id="btnBillingNotes" runat="Server" text="Show Billing Notes" cssclass="formfield"
                            onclick="btnBillingNotes_Click"></asp:button>
                        <asp:button id="btnSubmit" runat="Server" text="Show Report" cssclass="formfield"
                            onclick="btnSubmit_Click"></asp:button>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr colspan="2">
                    <td class="gradient" colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<iframe id="gToday:normal:agenda.js" style="z-index: 999; left: -500px; visibility: visible;
    position: absolute; top: -500px" name="gToday:normal:agenda.js" src="/ecn.collector/scripts/ipopeng.htm"
    frameborder='0' width="174" scrolling="no" height="189"></iframe>
</asp:content>

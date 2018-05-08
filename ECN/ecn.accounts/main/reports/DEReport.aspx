<%@ Page Language="c#" Inherits="ecn.accounts.main.reports.DigitalEdition" CodeBehind="DEReport.aspx.cs" MasterPageFile="~/MasterPages/Accounts.Master" %>

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
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr class="gradient">
                        <td width="50%" valign="middle" style="border: 1px #A4A2A3 solid; border-right: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            Digital&nbsp;Editions&nbsp;by Month</td>
                        <td width="50%" align='right' valign="middle" style="border: 1px #A4A2A3 solid; border-left: none;
                            font: bold 13px Arial, Helvetica, sans-serif; color: #333; padding: 0 5px;">
                            Download as:&nbsp;<asp:dropdownlist id="drpExport" cssclass="formlabel" runat="Server">
								<asp:ListItem value="pdf" selected>PDF</asp:ListItem>
								<asp:ListItem value="xls">XLS</asp:ListItem>
							</asp:dropdownlist></td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2">
                            <table cellspacing="2" cellpadding="5" border='0' width="100%" class="formLabel"
                                style="margin: 10px 0px">
                                <tr>
                                    <td width="300" align='right'>
                                        <b>Month&nbsp;:&nbsp;</b></td>
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
                                        <b>Year&nbsp;:&nbsp;</b></td>
                                    <td align="left">
                                        <asp:dropdownlist id="drpYear" cssclass="formlabel" runat="Server"></asp:dropdownlist>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite borderSides" colspan="2" align="center">
                            <asp:button id="btnSubmit" runat="Server" text="Show Report" cssclass="formfield"
                                onclick="btnSubmit_Click"></asp:button>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr colspan="2">
                        <td class="gradient" colspan="2">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:content>
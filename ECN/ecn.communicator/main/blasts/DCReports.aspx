<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DCReports.aspx.cs" Inherits="ecn.communicator.main.blasts.DCReports" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tr class="gradient">
            <td class="tableHeader" style="padding-left: 5px; border-top: #a4a2a3 1px solid;
                border-left: #a4a2a3 1px solid; border-bottom: #a4a2a3 1px solid;">
                <img src="/ecn.images/images/email_reports.gif" />&nbsp;&nbsp;Dynamic Content Sends
            </td>
            <td class="tableHeader" align='right' style="padding-right: 5px; border-right: #a4a2a3 1px solid;
                border-top: #a4a2a3 1px solid; border-bottom: #a4a2a3 1px solid">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan='2' class="greySides offWhite" style="padding: 15px 10px; border-bottom: #a4a2a3 1px solid;">
                <asp:datagrid id="dcGrid" runat="Server" width="100%" autogeneratecolumns="False" allowpaging="false"
                    cssclass="gridWizard">
					<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
					<AlternatingItemStyle CssClass="gridaltrowWizard" />	
					<FooterStyle CssClass="tableHeader1"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-Width="80%" DataField="contentTitle" HeaderText="Content" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-Width="20%" DataField="sends" HeaderText="sends" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
					</Columns>
									
				</asp:datagrid>
               
            </td>
        </tr>
    </table>
    <br>
</asp:content>

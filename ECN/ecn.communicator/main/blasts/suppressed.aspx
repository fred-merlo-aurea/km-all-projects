<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.suppressed" CodeBehind="suppressed.aspx.cs"   MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
	<TABLE id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
	    <tr>
	    <td colspan="1" align='left' class="homeButton" style="height: 40px" valign="top">
	        <asp:button id="SendSuppressedButton" onclick="SendSuppressed" runat="Server"
                text="Send Suppressed" class="formbuttonsmall" visible="true" />
	    </td>
        <td colspan="1" align='right' class="homeButton" style="height: 40px" valign="top">
            <asp:linkbutton id="btnHome" runat="Server" text="<span>Report Summary Page</span>"
                onclick="btnHome_Click"></asp:linkbutton>
        </td>
    </tr>  	
		<tr class="gradient">
			<td class="tableHeader" style="padding:0 0 0 5px;border-top:1px #A4A2A3 solid;border-left:1px #A4A2A3 solid;border-bottom:1px #A4A2A3 solid;"><table cellpadding="0" cellspacing="0" width="100%"><tr><td width="20"><img src="/ecn.images/images/unsubscribe.gif" /></td><td valign="top" align="left" class="tableHeader" style="padding:2px 0 0 0;" > Suppressed</td></tr></table>
			</td>
			<td class="tableHeader" align='right' style="padding:0 5px 0 0;border-top:1px #A4A2A3 solid;border-right:1px #A4A2A3 solid;border-bottom:1px #A4A2A3 solid;">
			<asp:Panel id="DownloadPanel" runat="Server" visible="true">
				Download Suppressed email addresses as&nbsp;&nbsp;
				<asp:DropDownList EnableViewState="true" id="DownloadType" runat="Server" class="formfield" visible="true">
					<asp:ListItem selected="true" Value=".xls">XLS file</asp:ListItem>				
					<asp:ListItem value=".csv">CSV file</asp:ListItem>
					<asp:ListItem value=".txt">TXT File</asp:ListItem>
				</asp:DropDownList>
				<asp:button class="formbuttonsmall" id="DownloadEmailsButton"  runat="Server" Visible="true" Text="Download" onClick="downloadSuppressedEmails"></asp:button>
			</asp:Panel>
			</td>
		</tr>
		<tr>
			<td colspan='2' class="greySides offWhite" style="padding: 15px 10px;border-bottom:1px #A4A2A3 solid;">		
			    <!-- removed the column from the following grid -->
			    <!-- <asp:BoundColumn ItemStyle-Width="5%" DataField="SubscriptionChange" HeaderText="Change" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn> -->
				<asp:DataGrid id="SuppressedGrid" runat="Server" width="100%" AutoGenerateColumns="False" CssClass="gridWizard">
					<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
					<AlternatingItemStyle CssClass="gridaltrowWizard" />	
					<FooterStyle CssClass="tableHeader1"></FooterStyle>
					<Columns>
						<asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="55%" HeaderStyle-Width="55%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Reason" HeaderText="Reason" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="center" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BlastsAlreadySent" HeaderText="Notes" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%" HeaderStyle-Width="20%" />
					</Columns>											
				</asp:DataGrid>
				<AU:PagerBuilder id="SuppressedPager" runat="Server" Width="100%" PageSize="50" OnIndexChanged="SuppressedPager_IndexChanged">
					<PagerStyle CssClass="gridpagerWizard"></PagerStyle>
				</AU:PagerBuilder>
			</td>
		</tr>
	</TABLE><br />
</asp:content>

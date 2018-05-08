<%@ Page Language="c#" Inherits="ecn.communicator.main.blasts.NoOpens" CodeBehind="NoOpens.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master"%>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="layoutWrapper" cellspacing="0" cellpadding="0" width="100%" border='0'>
    <tr>
        <td colspan="2" align='right' class="homeButton" style="height: 40px" valign="top">
            <asp:linkbutton id="btnHome" runat="Server" text="<span>Report Summary Page</span>"
                onclick="btnHome_Click"></asp:linkbutton>
        </td>
    </tr>
    <tr class="gradient">
        <td class="tableHeader" style="padding-left: 5px; border-top: #a4a2a3 1px solid;
            border-left: #a4a2a3 1px solid; border-bottom: #a4a2a3 1px solid;">
            <img src="/ecn.images/images/unopened.gif" />&nbsp;&nbsp;Unopened Emails
        </td>
        <td class="tableHeader" align='right' style="padding-right: 5px; border-right: #a4a2a3 1px solid;
            border-top: #a4a2a3 1px solid; border-bottom: #a4a2a3 1px solid">
            <asp:panel id="DownloadPanel" runat="Server" visible="true">
				Download No Opens as&nbsp;&nbsp; 
                    <asp:DropDownList class=formfield id=DownloadType runat="Server" visible="true" EnableViewState="true">
					<asp:ListItem selected="true" Value=".xls">XLS file</asp:ListItem>	
					<asp:ListItem value=".csv">CSV file</asp:ListItem>
					<asp:ListItem value=".txt">TXT File</asp:ListItem>
				</asp:DropDownList>
                <asp:button class=formbuttonsmall id=DownloadEmailsButton onclick=downloadUnsubscribedEmails runat="Server" Visible="true" Text="Download"></asp:button>
			</asp:panel>
        </td>
    </tr>
    <tr>
        <td colspan='2' class="greySides offWhite" style="padding: 15px 10px; border-bottom: #a4a2a3 1px solid;">
            <asp:datagrid id="NoOpensGrid" runat="Server" width="100%" autogeneratecolumns="False"
                cssclass="gridWizard">
					<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
					<AlternatingItemStyle CssClass="gridaltrowWizard" />	
					<FooterStyle CssClass="tableHeader1"></FooterStyle>
					<Columns>
						<asp:BoundColumn ItemStyle-Width="20%" DataField="Actiondate" HeaderText="Send Time" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
						<asp:BoundColumn ItemStyle-Width="80%" DataField="EmailAddress" HeaderText="Email Address" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left"></asp:BoundColumn>
					</Columns>
											
				</asp:datagrid>
            <AU:PagerBuilder ID="NoOpensPager" runat="Server" Width="100%" PageSize="50" >
                <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
            </AU:PagerBuilder>
        </td>
    </tr>
</table>
<br>
</asp:Content>

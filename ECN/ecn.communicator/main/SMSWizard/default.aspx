<%@ Page Language="c#" Inherits="ecn.communicator.main.SMSWizard._default" CodeBehind="default.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" border='0' style="text-align: left" width="100%">
        <tr>
            <td>
                <h4 id="comTurbo">
                    ECN <span id="turbo">SMS WIZARD!</span></h4>
                <h4 id="comTurboSub">
                    Create, Manage, and Send Text Messages on the Fly!</h4>
            </td>
        </tr>
        <tr>
            <td valign="top" align="left">
                <table cellpadding="0" cellspacing="0" width="100%" border='0'>
                    <tr>
                        <td class="gradient">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" class="offWhite greyOutSide">
                            <table cellspacing="1" cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td width="20%" align="center" valign="top">
                                        <table cellspacing="1" cellpadding="2" width="100%" border='0'>
                                            <tr>
                                                <td>
                                                    <asp:imagebutton id="btnimg1" runat="Server" alternatetext="Create New Message" imagealign="left"
                                                        imageurl="/ecn.images/images/create.jpg" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:imagebutton id="btnimg2" runat="Server" alternatetext="Manage Content" imagealign="left"
                                                        imageurl="/ecn.images/images/content.jpg" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>                                                   
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="80%">
                                        <table cellspacing="1" cellpadding="2" width="100%" border='0'>
                                            <tr>
                                                <td class="label" align="left" width="50%" valign="middle">
                                                    <img src="/ecn.images/images/saved_message.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                                        class="headingOne">Saved Text Messages</span></td>
                                                <td class="label" align='right' width="50%">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:datagrid id="dgPendingCampaigns" runat="Server" cssclass="gridWizard" width="100%"
                                                        datakeyfield="WizardID" allowsorting="True" autogeneratecolumns="False" onitemdatabound="dgPendingCampaigns_ItemDataBound"
                                                        ondeletecommand="dgPendingCampaigns_DeleteCommand">
														<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
														<HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
														<Itemstyle HorizontalAlign="Center"></Itemstyle>
														<Columns>
															<asp:HyperLinkColumn DataNavigateUrlField="WizardURL" DataNavigateUrlFormatString="SetupCampaign.aspx?{0}" 
 SortExpression="WizardName" DataTextField="WizardName" HeaderText="Message Name" headerstyle-HorizontalAlign="left" 
 itemstyle-HorizontalAlign="left" Itemstyle-width="65%" headerstyle-width="65%"></asp:HyperLinkColumn>
															<asp:BoundColumn DataField="DateCreated" HeaderText="Created" SortExpression="DateCreated" Itemstyle-width="20%" 
 headerstyle-width="20%"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Delete" Itemstyle-width="15%" headerstyle-width="15%">
																<ItemTemplate>
																	<asp:ImageButton id="btnDelete" runat="Server" CommandName="Delete" AlternateText="Delete Saved Message" 
 ImageUrl="/ecn.images/images/icon-delete1.gif" />
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
                                                    <AU:PagerBuilder ID="PendingPager" runat="Server" Width="100%" ControlToPage="dgPendingCampaigns"
                                                        PageSize="10" OnIndexChanged="Pager_IndexChanged">
                                                        <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="label" align="left" width="50%" valign="middle">
                                                    <img src="/ecn.images/images/savedEmailCampaigns.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                                        class="headingOne">Scheduled Text Messages</span></td>
                                                <td class="label" align='right' width="50%">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:datagrid id="dgScheduledCampaigns" runat="Server" cssclass="gridWizard" width="100%"
                                                        datakeyfield="BlastID" allowsorting="True" autogeneratecolumns="False" onitemdatabound="dgScheduledCampaigns_ItemDataBound"
                                                        ondeletecommand="dgScheduledCampaigns_DeleteCommand">
														<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
														<HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
														<Itemstyle HorizontalAlign="Center"></Itemstyle>
														<Columns>
															<asp:BoundColumn DataField="WizardName" HeaderText="Message Name" SortExpression="WizardName" 
 Itemstyle-width="65%" headerstyle-width="65%" headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left"></asp:BoundColumn>
															<asp:BoundColumn DataField="Sendtime" HeaderText="Scheduled Time" SortExpression="Sendtime" Itemstyle-width="20%" 
 headerstyle-width="20%"></asp:BoundColumn>
															<asp:TemplateColumn HeaderText="Delete" Itemstyle-width="15%" headerstyle-width="15%">
																<ItemTemplate>
																	<asp:ImageButton id="btnDelete1" runat="Server" CommandName="Delete" AlternateText="Delete Scheduled Message" 
 ImageUrl="/ecn.images/images/icon-delete1.gif" />
																</ItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid>
                                                    <AU:PagerBuilder ID="SchedulePager" runat="Server" Width="100%" ControlToPage="dgScheduledCampaigns"
                                                        PageSize="10" OnIndexChanged="Pager_IndexChanged">
                                                        <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="label" align="left" colspan="2" valign="middle">
                                                    <img src="/ecn.images/images/sentEmailCampaigns.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                                        class="headingOne">Recently Sent Text Messages</span></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:datagrid id="dgSentCampaigns" runat="Server" cssclass="gridWizard" width="100%"
                                                        datakeyfield="WizardID" allowsorting="True" autogeneratecolumns="False">
														<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
														<HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
														<Itemstyle HorizontalAlign="Center"></Itemstyle>
														<Columns>
															<asp:BoundColumn DataField="WizardName" HeaderText="Message Name" SortExpression="WizardName" Itemstyle-width="65%" 
 headerstyle-width="65%" headerstyle-HorizontalAlign="left" itemstyle-HorizontalAlign="left"></asp:BoundColumn>
															<asp:BoundColumn DataField="Sendtime" HeaderText="Sent" SortExpression="Sendtime" Itemstyle-width="20%" 
 headerstyle-width="20%"></asp:BoundColumn>
															<asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-reports.gif alt='view report' border='0'&gt;" 
 DataNavigateUrlField="BlastID" DataNavigateUrlFormatString="../blasts/reports.aspx?BlastID={0}" HeaderText="View Report" 
 Itemstyle-width="15%" headerstyle-width="15%"></asp:HyperLinkColumn>
														</Columns>
													</asp:datagrid>
                                                    <AU:PagerBuilder ID="SentPager" runat="Server" Width="100%" ControlToPage="dgSentCampaigns"
                                                        PageSize="10" OnIndexChanged="Pager_IndexChanged">
                                                        <PagerStyle CssClass="gridpagerWizard"></PagerStyle>
                                                    </AU:PagerBuilder>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tableHeader">
                                                    &nbsp;</td>
                                                <td valign="top">
                                                    <asp:label id="lblErrorMessage" runat="Server" visible="false" cssclass="errormsg"></asp:label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gradient">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br>
</asp:content>

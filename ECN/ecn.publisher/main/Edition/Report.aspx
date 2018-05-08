<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>

<%@ Page Language="c#" Inherits="ecn.publisher.main.Edition.Report" CodeBehind="Report.aspx.cs"  MasterPageFile="~/MasterPages/Publisher.Master"  %>

<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<br />
<table cellspacing="1" cellpadding="3" width="100%" border='0'>
    <tr>
        <td width="70%">
            <table cellspacing="1" cellpadding="3" width="100%" border='0' style="font-size: 11px;
                font-family: Arial, Helvetica, sans-serif">
                <tr>
                    <td width="30%" align="left">
                        Publication :
                    </td>
                    <td width="70%" align="left">
                        <asp:label id="lblPublication" runat="Server" font-bold="true"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Edition :
                    </td>
                    <td align="left">
                        <asp:label id="lblEdition" runat="Server" font-bold="true"></asp:label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Average Time Spent :
                    </td>
                    <td align="left">
                        <asp:label id="lblAverageTime" runat="Server" font-bold="true"></asp:label>
                        minutes
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        Reporting By Blast
                    </td>
                    <td align="left">
                        <asp:dropdownlist id="drpBlasts" runat="Server" cssclass="formfield" datavaluefield="BlastID"
                            datatextfield="BlastName" autopostback="true" onselectedindexchanged="drpBlasts_SelectedIndexChanged"></asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Summary :</b>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
        <td class="label" valign="middle" align="center" width="30%" rowspan="2">
            <asp:image id="imgThumbnail" runat="Server" border="1"></asp:image>
        </td>
    </tr>
    <tr>
        <td class="label" valign="middle" align="left" width="50%">
            <asp:datagrid id="ReportGrid" runat="Server" cssclass="gridWizard" horizontalalign="left"
                autogeneratecolumns="False" width="90%">
					<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
					<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="ActionTypeCode" HeaderText="Activity">
							<ItemStyle Width="60%"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Unique" HeaderText="Unique" headerstyle-HorizontalAlign="Center">
							<ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
						<asp:BoundColumn DataField="Total" HeaderText="Total" headerstyle-HorizontalAlign="Center">
							<ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
						</asp:BoundColumn>
					</Columns>
				</asp:datagrid>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; margin: 10px 0px;
                padding-top: 0px" align="center">
                <table width="100%" cellspacing="0" cellpadding="0" border='0'>
                    <tr>
                        <td valign="bottom" align="left">
                            <table cellspacing="0" cellpadding="0" border='0' width="100%">
                                <tr>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbVisits" runat="Server" text="<span>Visits</span>" onclick="lbVisits_Click"></asp:linkbutton>
                                    </td>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbClicks" runat="Server" text="<span>Clicks</span>" onclick="lbClicks_Click"></asp:linkbutton>
                                    </td>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbForwards" runat="Server" text="<span>Forwards</span>" onclick="lbForwards_Click"></asp:linkbutton>
                                    </td>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbSubscribes" runat="Server" text="<span>Subscribes</span>" onclick="lbSubscribes_Click"></asp:linkbutton>
                                    </td>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbPrints" runat="Server" text="<span>Prints</span>" onclick="lbPrints_Click"></asp:linkbutton>
                                    </td>
                                    <td valign="bottom" class="wizTabs">
                                        <asp:linkbutton id="lbSearch" runat="Server" text="<span>Search</span>" onclick="lbSearch_Click"></asp:linkbutton>
                                    </td>
                                    <td width="100%" align='center' valign="bottom" class="homeButton" valign="top" height="40">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="gradient" valign="middle" align='right'>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="offWhite greySidesB" width="100%" align="center">
                            <br />
                            <asp:placeholder id="phVisits" runat="Server" visible="true">
                                        <H5 style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; TEXT-ALIGN: left">Top 20 Visits</H5><br />
                                        <asp:datagrid id=dgTopVisit runat="Server" CssClass="gridWizard" Width="90%" AutoGenerateColumns="False" HorizontalAlign="Center">
										    <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
										    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
										    <Columns>
											    <asp:BoundColumn DataField="Count" HeaderText="Visited">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												    <ItemStyle Width="70%"></ItemStyle>
											    </asp:BoundColumn>
										    </Columns>
									    </asp:datagrid><br />
                                        
                                        <H5 style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; TEXT-ALIGN: left">Visits/Page</H5><br />
                                        
                                        <asp:datagrid id="dgVisitPerPage" runat="Server" CssClass="gridWizard" Width="90%" AutoGenerateColumns="False" HorizontalAlign="Center">
										    <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
										    <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
										    <ItemStyle HorizontalAlign="Center"></ItemStyle>
										    <Columns>
											    <asp:BoundColumn DataField="PageNumber" HeaderText="Page #">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="unique" HeaderText="unique">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="Total" HeaderText="Total">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
										    </Columns>
									    </asp:datagrid>
                                        <AU:PAGERBUILDER  id="PagerVisitPerPage" runat="Server" Width="90%" HorizontalAlign="Center" PageSize="10" ControlToPage="dgVisitPerPage" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PAGERBUILDER><br />
    									
                                        <H5 style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; TEXT-ALIGN: left">List of Visited</H5><br />
                                        
                                        <asp:datagrid id=dgVisitDetails runat="Server" CssClass="gridWizard" Width="90%" AutoGenerateColumns="False" HorizontalAlign="Center">
										    <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
										    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
										    <Columns>
											    <asp:BoundColumn DataField="ActionDate" HeaderText="Date Visited" headerstyle-HorizontalAlign="Center">
												    <ItemStyle Width="20%" HorizontalAlign="Center"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="PageNumber" HeaderText="Page #" headerstyle-HorizontalAlign="Center">
												    <ItemStyle Width="15%" HorizontalAlign="Center"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												    <ItemStyle Width="50%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="IP" HeaderText="Info">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
										    </Columns>
									    </asp:datagrid>
                                        <AU:PAGERBUILDER id=PagerVisitDetails runat="Server" Width="90%" HorizontalAlign="Center" PageSize="25" ControlToPage="dgVisitDetails" onindexchanged="PagerVisitDetails_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PAGERBUILDER><br />
								    </asp:placeholder>
                            <asp:placeholder id="phClicks" runat="Server" visible="false">
                                    
                                        <div style="float:left;"><H5 style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; TEXT-ALIGN: left">Top Click-Throughs</H5></div>
                                        <div style="float:right;padding-right:50px">
                                            <asp:dropdownlist id="drpTopClicks" runat="Server" cssclass="formfield" autopostback="true"  onselectedindexchanged="drpTopClicks_SelectedIndexChanged">
                                            <asp:ListItem value="10" >Top 10 Clicks</asp:ListItem>
								    <asp:ListItem value="20" Selected="True">Top 20 Clicks</asp:ListItem>
								    <asp:ListItem value="30">Top 30 Clicks</asp:ListItem>
								    <asp:ListItem value="40">Top 40 Clicks</asp:ListItem>
								    <asp:ListItem value="50">Top 50 Clicks</asp:ListItem>
								    <asp:ListItem value="0">All Clicks</asp:ListItem>
								    </asp:dropdownlist>
                                            </div><br /><br />
                                    
                                        <asp:DataGrid id="dgTopClicks" runat="Server" showfooter="true" CssClass="gridWizard" AutoGenerateColumns="False" HorizontalAlign="Center" width="90%">
										    <HeaderStyle CssClass="gridheaderWizard" ></HeaderStyle>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
										    <footerStyle CssClass="gridheaderWizard" />
										    <columns>
											    <asp:hyperlinkcolumn HeaderText="URL / Link " ItemStyle-Width="50%" DataNavigateUrlField="Link" DataNavigateUrlFormatString="{0}" 
                                                    DataTextField="Link" target="_blank"></asp:hyperlinkcolumn>
											    <asp:BoundColumn HeaderText="Page #" DataField="PageNumber" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
 						                        <asp:TemplateColumn HeaderText="Total Clicks" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" footerStyle-HorizontalAlign="Center" >
							                        <ItemTemplate>
							                            <asp:LinkButton id="lnktotalclick" oncommand="Clickdownload" commandname="total" commandargument='<%# DataBinder.Eval(Container, "DataItem.linkID") %>' runat="server" text='<%# DataBinder.Eval(Container, "DataItem.ClickCount") %>'></asp:LinkButton>
							                        </ItemTemplate>
							                         <FooterTemplate >
							                            <asp:LinkButton id="lnktotalclick1" oncommand="Clickdownload" commandname="total" commandargument='0' runat="server" text='Download All'></asp:LinkButton>
                                                     </FooterTemplate>
						                        </asp:TemplateColumn>
 						                        <asp:TemplateColumn HeaderText="Unique Clicks" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" footerStyle-HorizontalAlign="Center" >
							                        <ItemTemplate>
							                            <asp:LinkButton id="lnkuniqueclick" oncommand="Clickdownload" commandname="unique" commandargument='<%# DataBinder.Eval(Container, "DataItem.linkID") %>' runat="server" text='<%# DataBinder.Eval(Container, "DataItem.distinctClickCount") %>'></asp:LinkButton>
							                        </ItemTemplate>
							                        <FooterTemplate >
							                            <asp:LinkButton id="lnkuniqueclick1" oncommand="Clickdownload" commandname="unique" commandargument='0' runat="server" text='Download All Unique'></asp:LinkButton>
                                                     </FooterTemplate>
						                        </asp:TemplateColumn>
										    </columns>
									    </asp:DataGrid>
                                        <AU:PagerBuilder id="PagerTopClicks" runat="Server" Width="90%" HorizontalAlign="Center" PageSize="10" ControlToPage="dgTopClicks" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PagerBuilder><br />
    									
                                        <H5 style="PADDING-RIGHT: 0px; PADDING-LEFT: 20px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; TEXT-ALIGN: left">Click-Throughs by Time</H5><br />
                                        
                                        <asp:DataGrid id="dgClickDetails" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False" HorizontalAlign="Center" width="90%">
										    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
										    <columns>
											    <asp:BoundColumn DataField="ActionDate" HeaderText="Click Time" headerstyle-HorizontalAlign="Center">
												    <ItemStyle Width="15%" HorizontalAlign="Center"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="PageNumber" HeaderText="Page #" headerstyle-HorizontalAlign="Center">
												    <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:hyperlinkcolumn HeaderText="URL / Link " ItemStyle-Width="45%" DataNavigateUrlField="Link" DataNavigateUrlFormatString="{0}" DataTextField="Link" target="_blank"></asp:hyperlinkcolumn>
										    </columns>
									    </asp:DataGrid>
                                        <AU:PagerBuilder id=pagerClickDetails runat="Server" Width="90%" HorizontalAlign="Center" PageSize="25" ControlToPage="dgClickDetails" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PagerBuilder><br />
								    </asp:placeholder>
                            <asp:placeholder id="phForwards" runat="Server" visible="false">
                                        <asp:DataGrid id="dgForwards" runat="Server" CssClass="gridWizard" AutoGenerateColumns="False" HorizontalAlign="Center" width="90%">
										    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
										    <Columns>
											    <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Forward Time" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												    <ItemStyle Width="40%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn ItemStyle-Width="40%" DataField="ActionValue" HeaderText="Referral"></asp:BoundColumn>
										    </Columns>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
									    </asp:DataGrid>
                                        <AU:PagerBuilder id=PagerForwards runat="Server" Width="90%" HorizontalAlign="Center" PageSize="10" ControlToPage="dgForwards" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PagerBuilder><br />
								    </asp:placeholder>
                            <asp:placeholder id="phSubscribes" runat="Server" visible="false">
                                        <asp:DataGrid id=dgSubscribes runat="Server" CssClass="gridWizard" AutoGenerateColumns="False" HorizontalAlign="Center" width="90%">
										    <HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
										    <Columns>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address">
												    <ItemStyle Width="40%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn ItemStyle-Width="20%" DataField="ActionDate" HeaderText="Date Subscribed" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center"></asp:BoundColumn>
										    </Columns>
										    <AlternatingItemStyle CssClass="gridaltrowWizard" />
									    </asp:DataGrid>
                                        <AU:PagerBuilder id="PagerSubscribes" runat="Server" Width="90%" HorizontalAlign="Center" PageSize="10" ControlToPage="dgSubscribes" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PagerBuilder><br />
								    </asp:placeholder>
                            <asp:placeholder id="phPrints" runat="Server" visible="false">
                                        <asp:datagrid id="dgPrintsPerPage" runat="Server" CssClass="gridWizard" Width="90%" AutoGenerateColumns="False" HorizontalAlign="Center">
										    <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
										    <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
										    <ItemStyle HorizontalAlign="Center"></ItemStyle>
										    <Columns>
											    <asp:BoundColumn DataField="PageNumber" HeaderText="Page #">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="unique" HeaderText="unique">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="Total" HeaderText="Total">
												    <ItemStyle Width="30%"></ItemStyle>
											    </asp:BoundColumn>
										    </Columns>
									    </asp:datagrid>
                                        <AU:PAGERBUILDER id="PagerPrintsPerPage" runat="Server" Width="90%" HorizontalAlign="Center" PageSize="10" ControlToPage="dgPrintsPerPage" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PAGERBUILDER><br />
								    </asp:placeholder>
                            <asp:placeholder id="phSearch" runat="Server" visible="false">
                                        <asp:datagrid id="dgSearch" runat="Server" CssClass="gridWizard" Width="90%" AutoGenerateColumns="False" HorizontalAlign="Center">
										    <AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
										    <HeaderStyle CssClass="gridheaderWizard" HorizontalAlign="Center"></HeaderStyle>
										    <ItemStyle HorizontalAlign="Center"></ItemStyle>
										    <Columns>
											    <asp:BoundColumn DataField="ActionDate" HeaderText="Date">
												    <ItemStyle Width="20%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="EmailAddress" HeaderText="EmailAddress">
												    <ItemStyle Width="40%"></ItemStyle>
											    </asp:BoundColumn>
											    <asp:BoundColumn DataField="SearchText" HeaderText="Search Text">
												    <ItemStyle Width="40%"></ItemStyle>
											    </asp:BoundColumn>
										    </Columns>
									    </asp:datagrid>
                                        <AU:PAGERBUILDER  id=PagerSearch runat="Server" Width="90%" HorizontalAlign="Center" PageSize="20" ControlToPage="dgSearch" onindexchanged="Pager_IndexChanged">
										    <PagerStyle CssClass="gridpager"></PagerStyle>
									    </AU:PAGERBUILDER><br />
								    </asp:placeholder>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
</asp:Content>

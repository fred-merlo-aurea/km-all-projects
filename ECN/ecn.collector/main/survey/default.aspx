<%@ Page Language="c#"  AutoEventWireup="true" Inherits="ecn.collector.main.survey._default" CodeBehind="default.aspx.cs" MasterPageFile="~/MasterPages/Collector.Master"  %>

<%@ MasterType VirtualPath="~/MasterPages/Collector.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<table cellpadding="0" cellspacing="0" border='0' width="100%">
        <tr>
            <td class="gradient">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="offWhite">
                <table cellspacing="1" cellpadding="2" width="100%" border='0'>
                    <tr>
                        <td width="20%" align="center" valign="top">
                            <table cellspacing="1" cellpadding="2" width="100%" border='0' class="survDefButtons">
                                <tr>
                                    <td>
                                        <a href="/ecn.collector/main/survey/SurveyWizard.aspx" id="createNewSurveyBtn">Create
                                            New Survey</a></td>
                                </tr>
                                <!--<tr>
									<td><a href="javascript:void(0);" id="manageImagesBtn">Manage Images</a></td>
								</tr>-->
                            </table>
                        </td>
                        <td width="80%" valign="top">
                            <table cellspacing="1" cellpadding="2" width="100%" border='0'>
                                <tr>
                                    <td class="label" align="left" width="50%" valign="middle">
                                        <img src="/ecn.images/images/pending_surveys.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                            class="headingOne">Pending Surveys</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:datagrid id="dgPendingSurveys" runat="Server" autogeneratecolumns="False" width="100%"
                                            cssclass="gridWizard" datakeyfield="SurveyID">
											<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
											<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="SurveyTitle" HeaderStyle-HorizontalAlign="Left" HeaderText="Survey Title"
													itemstyle-width="50%" headerstyle-width="50%" ></asp:BoundColumn>
												<asp:BoundColumn DataField="CreatedDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Date Created"
													itemstyle-width="20%"  headerstyle-width="20%"></asp:BoundColumn>
												<asp:HyperLinkColumn headertext="Edit" text="<img src='/ecn.images/images/icon-edits1.gif' alt='Edit Survey' border='0'>"
													DataNavigateUrlField="SurveyURL" DataNavigateUrlFormatString="SurveyWizard.aspx?{0}"  headerstyle-width="10%" itemstyle-width="10%"
													itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:HyperLinkColumn headertext="Preview" text="<img src='/ecn.images/images/icon-preview.gif' alt='Preview Survey' border='0'>"
													target="_blank" DataNavigateUrlField="SurveyID" DataNavigateUrlFormatString="../../front/view_survey.aspx?surveyID={0}"
													 headerstyle-width="10%" itemstyle-width="10%" itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:TemplateColumn HeaderText="Delete" headerstyle-width="10%" Itemstyle-width="10%" itemstyle-horizontalalign="center" headerstyle-horizontalalign="center">
													<ItemTemplate>
														<asp:ImageButton id="btnDelete" runat="Server" CommandName="delete" commandargument=<%# DataBinder.Eval(Container.DataItem, "SurveyID") %> AlternateText="Delete Survey"
															ImageUrl="/ecn.images/images/icon-delete1.gif" />
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label" align="left" width="50%" valign="middle">
                                        <img src="/ecn.images/images/Live_surveys.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                            class="headingOne">Live Surveys</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:datagrid id="dgLiveSurveys" runat="Server" autogeneratecolumns="False" width="100%"
                                            cssclass="gridWizard" datakeyfield="SurveyID" >
											<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
											<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="SurveyTitle" HeaderStyle-HorizontalAlign="Left" HeaderText="Survey Title"
													itemstyle-width="40%" headerstyle-width="40%"></asp:BoundColumn>
												<asp:BoundColumn DataField="CreatedDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Date Created"
													itemstyle-width="20%"  headerstyle-width="20%"></asp:BoundColumn>
						                        <asp:TemplateColumn HeaderText="Create Blast" headerstyle-HorizontalAlign="Center" headerstyle-width="10%" Itemstyle-width="10%">
							                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
							                        <ItemTemplate>
								                        <asp:ImageButton id="btnBlast" runat="Server" AlternateText="Create Blast" ImageUrl="/ecn.images/images/active-opens.gif"
									                        CommandName="Create" />
							                        </ItemTemplate>
						                        </asp:TemplateColumn>													
												<asp:HyperLinkColumn headertext="Edit" text="<img src='/ecn.images/images/icon-edits1.gif' alt='Edit Survey' border='0'>"
													DataNavigateUrlField="SurveyID" DataNavigateUrlFormatString="SurveyWizard.aspx?surveyID={0}"  headerstyle-width="10%" itemstyle-width="10%"
													itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:HyperLinkColumn headertext="Preview" text="<img src='/ecn.images/images/icon-preview.gif' alt='Preview Survey' border='0'>"
													target="_blank" DataNavigateUrlField="SurveyID" DataNavigateUrlFormatString="../../front/view_survey.aspx?surveyID={0}"
													 headerstyle-width="10%" itemstyle-width="10%" itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:TemplateColumn HeaderText="Reporting"  headerstyle-width="10%" itemstyle-width="10%" HeaderStyle-HorizontalAlign="Center"
													ItemStyle-HorizontalAlign="Center">
													<ItemTemplate>
														<asp:label id="lblresponsecount1" runat="Server" visible="false" Text=<%# DataBinder.Eval(Container.DataItem, "responsecount") %>>
														</asp:label>
														<asp:hyperlink ID="lnkReport1" runat="Server" NavigateUrl='<%# "../report/survey_report.aspx?surveyID=" + DataBinder.Eval(Container.DataItem, "SurveyID") %>' Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Survey Reporting' border='0'>">
														</asp:hyperlink>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Delete Responses"  headerstyle-width="10%" itemstyle-width="10%" HeaderStyle-HorizontalAlign="Center"
													ItemStyle-HorizontalAlign="Center">
													<ItemTemplate>
                                                        <asp:ImageButton id="btnDeleteResponse" runat="Server" CommandName="delete" commandargument=<%# DataBinder.Eval(Container.DataItem, "SurveyID") %> AlternateText="Delete Survey Response"
															ImageUrl="/ecn.images/images/icon-delete1.gif" />
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:datagrid>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label" align="left" width="50%" valign="middle">
                                        <img src="/ecn.images/images/completed_surveys.gif" class="headerImage">&nbsp;&nbsp;&nbsp;&nbsp;<span
                                            class="headingOne">Completed Surveys</span></td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 20px;">
                                        <asp:datagrid id="dgCompletedSurveys" runat="Server" autogeneratecolumns="False"
                                            width="100%" cssclass="gridWizard" datakeyfield="SurveyID">
											<AlternatingItemStyle CssClass="gridaltrowWizard"></AlternatingItemStyle>
											<HeaderStyle CssClass="gridheaderWizard"></HeaderStyle>
											<Columns>
												<asp:BoundColumn DataField="SurveyTitle" HeaderStyle-HorizontalAlign="Left" HeaderText="Survey Title"
													itemstyle-width="50%" headerstyle-width="50%"></asp:BoundColumn>
												<asp:BoundColumn DataField="CreatedDate" HeaderStyle-HorizontalAlign="Left" HeaderText="Date Created"
													itemstyle-width="20%"  headerstyle-width="20%"></asp:BoundColumn>
												<asp:HyperLinkColumn headertext="Edit" text="<img src='/ecn.images/images/icon-edits1.gif' alt='Edit Survey' border='0'>"
													DataNavigateUrlField="SurveyID" DataNavigateUrlFormatString="SurveyWizard.aspx?surveyID={0}" itemstyle-width="10%"
													 headerstyle-width="10%" itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:HyperLinkColumn headertext="Preview" text="<img src='/ecn.images/images/icon-preview.gif' alt='Preview Survey' border='0'>"
													target="_blank" DataNavigateUrlField="SurveyID" DataNavigateUrlFormatString="../../front/view_survey.aspx?surveyID={0}"
													 headerstyle-width="10%" itemstyle-width="10%" itemstyle-horizontalalign="center" headerstyle-horizontalalign="center"></asp:HyperLinkColumn>
												<asp:TemplateColumn HeaderText="Reporting"  headerstyle-width="10%" itemstyle-width="10%" HeaderStyle-HorizontalAlign="Center"
													ItemStyle-HorizontalAlign="Center">
													<ItemTemplate>
														<asp:label id="lblresponsecount2" runat="Server" visible="false" Text=<%# DataBinder.Eval(Container.DataItem, "responsecount") %>>
														</asp:label>
														<asp:hyperlink ID="lnkReport2" runat="Server" NavigateUrl='<%# "../report/survey_report.aspx?surveyID=" + DataBinder.Eval(Container.DataItem, "SurveyID") %>' Text="<img src='/ecn.images/images/icon-reports.gif' alt='View Survey Reporting' border='0'>">
														</asp:hyperlink>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Delete Responses"  headerstyle-width="10%" itemstyle-width="10%" HeaderStyle-HorizontalAlign="Center"
													ItemStyle-HorizontalAlign="Center">
													<ItemTemplate>
                                                        <asp:ImageButton id="btnDeleteResponse" runat="Server" CommandName="delete" commandargument=<%# DataBinder.Eval(Container.DataItem, "SurveyID") %> AlternateText="Delete Survey Response"
															ImageUrl="/ecn.images/images/icon-delete1.gif" />
													</ItemTemplate>
												</asp:TemplateColumn>
												
											</Columns>
										</asp:datagrid>
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
</asp:content>


<%@ Page Language="c#" Inherits="ecn.collector.main.report.ViewResponse" CodeBehind="ViewResponse.aspx.cs"
    MasterPageFile="~/MasterPages/Collector.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Collector.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
<table cellspacing="0" cellpadding="0" width="100%" border='0'>
    <tr>
        <td style="background: #f0f1f2; font-size: 16px; font-weight: bold; padding: 5px 10px;
            border: 1px #a4a2a3 solid; border-right: none;">
            <asp:label id="lblSurveyTitle" runat="Server"></asp:label>
        </td>
        <td align='right' style="background: #f0f1f2; border: 1px #a4a2a3 solid; border-left: none;"
            valign="bottom" class="survRepTabs">
            <asp:hyperlink id="lnkToRespondents" cssclass="survRepTab" runat="Server" style="float: right;
                width: 125px;" text="<span>View Respondents</span>"></asp:hyperlink>
            <asp:hyperlink id="lnkSurveyResults" cssclass="survRepTab" runat="Server" style="float: right;
                width: 100px;" text="<span>Survey Results</span>"></asp:hyperlink>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center" style="background: #333; color: #fff; font-size: 13px;
            padding: 5px 10px; font-weight: bold;">
            Individual Survey Results &raquo; <a href="javascript:history.back(1)" class="backtoRespond">
                Return to View Respondents</a>
        </td>
    </tr>
    <tr>
        <td align='right' class="formLabel" width="20%" style="padding: 5px 10px; border-left: 1px #a4a2a3 solid;">
            Email Address :&nbsp;
        </td>
        <td class="dataTwo" width="80%" style="border-right: 1px #a4a2a3 solid;">
            <asp:label id="lblEmailAddress" runat="Server"></asp:label>
        </td>
    </tr>
    <tr>
        <td align='right' class="formLabel" width="20%" style="padding: 5px 10px; border-left: 1px #a4a2a3 solid;">
            Completed Date :&nbsp;
        </td>
        <td class="dataTwo" width="80%" style="border-right: 1px #a4a2a3 solid;">
            <asp:label id="lblCompletedDate" runat="Server"></asp:label>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" align='right' colspan="2" style="padding-right: 20px; padding-left: 20px">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="tableHeader" colspan="2">
            <asp:repeater id="repQuestions" runat="Server" onitemdatabound="repQuestions_ItemDataBound">
					<ItemTemplate>
						<table cellpadding="0" cellspacing="0" width="100%" align="center">
							<tr>
								<td align="left" width="50%" class="formLabel gradient"><strong>&nbsp;Q&nbsp;<%# DataBinder.Eval(Container.DataItem, "number") %></strong>
									&nbsp;&nbsp;|&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem, "QuestionText") %>
									<asp:label id="lblQuestionID" runat="Server" visible=false text='<%# DataBinder.Eval(Container.DataItem, "QuestionID") %>'>									</asp:label>								</td>
							</tr>
							<tr bgcolor="#ffffff">
								<td align="center" style="padding:5px" class='surveyPageBottomDef formLabel'>
									<table border='0' cellpadding="1" cellspacing="0" width="100%" align="center">
										<tr>
											<td width="100%">
												<asp:placeholder id="plresponse" runat="Server"></asp:placeholder>
												<asp:datagrid id="dgGridResponse" runat="Server" AutoGenerateColumns="true" CssClass="gridWizard"
													Width="100%" onitemdatabound="dgGridResponse_ItemDataBound">
													<headerstyle CssClass="gridheaderWizard" HorizontalAlign="Center"></headerstyle>
													<Itemstyle HorizontalAlign="Center"></Itemstyle>
													<alternatingitemstyle HorizontalAlign="Center" CssClass="gridaltrowWizard" />
												</asp:datagrid>											</td>
										</tr>
									</table>								</td>
							</tr>
						</table>
					</ItemTemplate>
					<SeparatorTemplate>
						<br />
					</SeparatorTemplate>
				</asp:repeater>
        </td>
    </tr>
    <tr>
        <td class="tableHeader" align='center' colspan="2" style="padding-right: 20px; padding-left: 20px">
            <br />
            <asp:button runat="server" text="Delete Response" id="btnDeleteResponse" onclick="btnDeleteResponse_Click"
                class="button" 
                onclientclick="return confirm('Are you sure that you want to delete this Survey response?')" />
        </td>
    </tr>
</table>
</asp:Content>

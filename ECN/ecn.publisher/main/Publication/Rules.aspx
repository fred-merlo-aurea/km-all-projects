<%@ Page Language="c#" Inherits="ecn.publisher.main.Publication.Rules" Codebehind="Rules.aspx.cs"  MasterPageFile="~/MasterPages/Publisher.Master" %>


<%@ MasterType VirtualPath="~/MasterPages/Publisher.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
        <tr class="tableHeader">
            <td class="label" colspan="2">
                Manage Visitor Rules for <i>
                    <asp:label id="lblPublication" runat="Server" text="" font-bold="true" cssclass="label"></asp:label>
                </i>&nbsp;Publication<br />
                <br />
            </td>
        </tr>
        <tr class="tableHeader">
            <td colspan="2">
                <asp:datagrid id="dgRules" runat="Server" cssclass="grid" width="100%" showfooter="True"
                    autogeneratecolumns="False" horizontalalign="Center" datakeyfield="RuleID">
					<FooterStyle CssClass="gridwhite"></FooterStyle>
					<HeaderStyle CssClass="gridheader"></HeaderStyle>
					<Columns>
						<asp:TemplateColumn HeaderText="Rule Name">
							<ItemTemplate>
								<asp:Label runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.RuleName") %>' ID="lblRuleName">
								</asp:Label>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:textbox id="txtRuleName" runat="Server" CssClass="formfield" width="300" Text='<%# DataBinder.Eval(Container, "DataItem.RuleName") %>'>
								</asp:textbox>
								<asp:requiredfieldvalidator id="rfvtxtRuleName" runat="Server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> required"
									ControlToValidate="txtRuleName" Font-Size="xx-small"></asp:requiredfieldvalidator>
							</EditItemTemplate>
							<FooterTemplate>
								<asp:textbox id="txtRuleNameF" runat="Server" CssClass="formfield" width="300" Text='<%# DataBinder.Eval(Container, "DataItem.RuleName") %>'>
								</asp:textbox>
								<asp:requiredfieldvalidator id="rfvtxtRuleNameF" runat="Server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> required"
									ControlToValidate="txtRuleNameF" Font-Size="xx-small"></asp:requiredfieldvalidator>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Edition Name">
							<ItemTemplate>
								<asp:Label runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.EditionName") %>' ID="lblEditionName">
								</asp:Label>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:dropdownlist id="drpEditionList" runat="Server" CssClass="formfield" width="200"></asp:dropdownlist>
								<asp:requiredfieldvalidator id="txtdrpEditionList" runat="Server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> required"
									ControlToValidate="drpEditionList" Font-Size="xx-small"></asp:requiredfieldvalidator>
							</EditItemTemplate>
							<FooterTemplate>
								<asp:dropdownlist id="drpEditionListF" runat="Server" CssClass="formfield" width="200"></asp:dropdownlist>
								<asp:requiredfieldvalidator id="txtdrpEditionListF" runat="Server" Font-Bold="True" Font-Italic="True" ErrorMessage=">> required"
									ControlToValidate="drpEditionListF" Font-Size="xx-small"></asp:requiredfieldvalidator>
							</FooterTemplate>
						</asp:TemplateColumn>
						<asp:HyperLinkColumn Text="&lt;img src=/ecn.images/images/icon-add-edit.gif alt='Add / Edit Rule attributes'"
							DataNavigateUrlField="RuleID" DataNavigateUrlFormatString="RuleDetails.aspx?RuleID={0}" HeaderText="Rule Details">
							<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
						</asp:HyperLinkColumn>
						<asp:TemplateColumn Headertext="Edit" headerstyle-HorizontalAlign="Center" Itemstyle-HorizontalAlign="Center">
							<ItemTemplate>
								<asp:LinkButton id="lnkbutEdit" runat="Server" Text="<img border='0' src=/ecn.images/images/icon-edits1.gif alt=edit>"
									CommandName="Edit" CausesValidation="false"></asp:LinkButton>
							</ItemTemplate>
							<EditItemTemplate>
								<asp:LinkButton id="lnkbutUpdate" runat="Server" Text="<img  border='0' src=/ecn.images/images/icon-edits1.gif alt=save/update>"
									CommandName="Update"></asp:LinkButton>&nbsp;
								<asp:LinkButton id="lnkbutCancel" runat="Server" Text="<img border='0' src=/ecn.images/images/icon-delete1.gif alt=cancel>"
									CommandName="Cancel" CausesValidation="false"></asp:LinkButton>
							</EditItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn itemstyle-width="5%" Headertext="Delete" headerstyle-HorizontalAlign="Center" Itemstyle-HorizontalAlign="Center">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:LinkButton id="lnkbutDelete" runat="Server" Text="<img border='0' src=/ecn.images/images/icon-delete1.gif alt=Delete Rule>"
									CommandName="Delete" CausesValidation="false"></asp:LinkButton>
							</ItemTemplate>
							<FooterStyle HorizontalAlign="Center"></FooterStyle>
							<FooterTemplate>
								<asp:Button id="btnAddRow" runat="Server" CssClass="button" Text="Add Rule" CommandName="Add"></asp:Button>
							</FooterTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:datagrid>
            </td>
        </tr>
    </table>
</asp:Content>

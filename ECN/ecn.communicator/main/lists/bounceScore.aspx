<%@ Page Language="c#" Inherits="ecn.communicator.listsmanager.bounceScore" Codebehind="bounceScore.aspx.cs"  MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="AU" Namespace="ActiveUp.WebControls" Assembly="ActiveUp.WebControls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                                <table cellspacing="0" cellpadding="0" width="674" align="center">
                                    <tr>
                                        <td id="errorTop">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorMiddle">
                                            <table height="67" width="80%">
                                                <tr>
                                                    <td valign="top" align="center" width="20%">
                                                        <img style="padding: 0 0 0 15px;" src="/ecn.images/images/errorEx.jpg">
                                                    </td>
                                                    <td valign="middle" align="left" width="80%" height="100%">
                                                        <asp:Label ID="lblErrorMessage" runat="Server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="errorBottom">
                                        </td>
                                    </tr>
                                </table>
    </asp:PlaceHolder>
<asp:label id="msglabel" runat="Server" visible="false" cssclass="errormsg"></asp:label>
<br />
<table id="layoutWrapper" cellspacing="1" cellpadding="2" width="100%" border='0'>
    <tbody>
        <tr>
            <td class="tableHeader" align="left">
                <span class="label">Select Email profiles with Bounce Score
                    <asp:dropdownlist class="formfield" id="BounceScoreConditionDR" runat="Server" enableviewstate="true">
						<asp:ListItem value="=" Selected="True">equal to [ = ]</asp:ListItem>
						<asp:ListItem value=">">greater than [ > ]</asp:ListItem>
						<asp:ListItem value=">=">greater than / equal to[ > / = ]</asp:ListItem>
						<asp:ListItem value="<">less than [ < ]</asp:ListItem>
						<asp:ListItem value="<=">less than / equal to [ < / = ]</asp:ListItem>
					</asp:dropdownlist>
                    &nbsp;
                    <asp:dropdownlist class="formfield" id="BounceScoreDR" runat="Server" enableviewstate="true"></asp:dropdownlist>
                    &nbsp;point(s) &nbsp;from&nbsp;
                    <asp:dropdownlist class="formfield" id="GroupID" runat="Server" enableviewstate="true"
                        datavaluefield="GroupID" datatextfield="GroupName"></asp:dropdownlist>
                    <asp:requiredfieldvalidator id="val_GroupID" runat="Server" initialvalue="" cssclass="errormsg"
                        errormessage="лл Required" controltovalidate="GroupID"></asp:requiredfieldvalidator>
                </span>
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="bottom" align="left" height="40">
                <asp:button class="formbuttonsmall" id="ShowCountResultsBtn" runat="Server" visible="true"
                    text="Show Counts Only" onclick="ShowCountResultsBtn_Click"></asp:button>
                <asp:button class="formbuttonsmall" id="ShowFullResultsBtn" runat="Server" visible="true"
                    text="Show Email Profiles with Score"></asp:button>
            </td>
        </tr>
        <tr>
            <td>
                <hr size="1" width="100%" color="#000000">
            </td>
        </tr>
        <tr>
            <td class="tableHeader" valign="top" align="left">
                <asp:datagrid id="BounceScoreGrid" runat="Server" width="50%" cssclass="grid" autogeneratecolumns="False">
					<FooterStyle CssClass="tableHeader1"></FooterStyle>
					<AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
					<ItemStyle Height="22px"></ItemStyle>
					<HeaderStyle CssClass="gridheader"></HeaderStyle>
					<Columns>
						<asp:BoundColumn DataField="BounceScore" HeaderText="Bounce Score" ItemStyle-width="45%" headerstyle-HorizontalAlign="center"
							ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
						<asp:BoundColumn DataField="EmailsCount" HeaderText="# of Emails" headerstyle-HorizontalAlign="center"
							ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					</Columns>
				</asp:datagrid>
                <AU:PagerBuilder ID="BounceScorePager" runat="Server" Width="50%" ControlToPage="BounceScoreGrid"
                    PageSize="15">
                    <PagerStyle CssClass="tableContent"></PagerStyle>
                </AU:PagerBuilder>
            </td>
        </tr>
    </TBODY>
</table>
</asp:Content>
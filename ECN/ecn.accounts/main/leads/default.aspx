<%@ Page Language="c#" Inherits="ecn.accounts.main.leads._default" CodeBehind="default.aspx.cs"
    Title="Leads Management System" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ Register TagPrefix="uc1" TagName="LeadsWeeklyReport" Src="../../includes/LeadsWeeklyReport.ascx" %>
<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
 <style type="text/css">
     DIV#CallCountPanel
     {
         border-right: gray 1px solid;
         padding-right: 5px;
         border-top: gray 1px solid;
         padding-left: 5px;
         left: 930px;
         padding-bottom: 5px;
         border-left: gray 1px solid;
         width: 250px;
         color: black;
         padding-top: 5px;
         border-bottom: gray 1px solid;
         position: absolute;
         top: 172px;
         text-align: center;
         background-color: #efefef;
     }
 </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 
        <table class="tableContent" cellspacing="5" cellpadding="5" width="900">
            <tr bgcolor="#f4f4f4">
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" colspan="2">
                    Account Executive:
                    <asp:DropDownList ID="ddlStaff" runat="Server" Width="272px">
                    </asp:DropDownList>Start Date:
                    <asp:TextBox ID="txtStartDate" runat="Server"></asp:TextBox>End Date:
                    <asp:TextBox ID="txtEndDate" runat="Server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="Server" Text="Refine" CausesValidation="False"
                        OnClick="btnSearch_Click"></asp:Button></td>
            </tr>
            <tr>
                <td colspan="2" class="tableContent">
                    <uc1:LeadsWeeklyReport ID="ucLeadsWeeklyReport" runat="Server"></uc1:LeadsWeeklyReport>
                </td>
            </tr>
            <tr bgcolor="#f4f4f4">
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" valign="top" align="center" width="30%">
                    <table class="tableContent" cellspacing="0" cellpadding="0" width="80%">
                        <tr>
                            <td class="tableHeader" align="center" colspan='3'>
                                <asp:Label ID="lblMgrMessage" runat="Server" Width="126px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Invites</td>
                            <td>
                                Demos</td>
                            <td>
                                Quotes</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblInvitesCount" runat="Server">0</asp:Label></td>
                            <td>
                                <asp:Label ID="lblDemoCount" runat="Server">0</asp:Label></td>
                            <td>
                                <asp:Label ID="lblQuotesCount" runat="Server">0</asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan='3'>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                Demo Rate:</td>
                            <td style="border-bottom: black 1px dotted" align="center" colspan="2">
                                <asp:Label ID="lblDemoPercentage" runat="Server" Width="70px">0%</asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                Quote Rate:</td>
                            <td style="border-bottom: black 1px dotted" align="center" colspan="2">
                                <asp:Label ID="lblQuotePercentage" runat="Server" Width="88px">0%</asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" width="70%">
                    <table class="tableContent" width="100%">
                        <thead class="tableHeader" align="center">
                            Add Lead/Send Invite</thead>
                        <tr>
                            <td>
                                First name:</td>
                            <td>
                                <asp:TextBox ID="txtFirstName" TabIndex="1" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required field.">*</asp:RequiredFieldValidator></td>
                            <td rowspan="5">
                                Note:<br />
                                <asp:TextBox ID="txtNote" TabIndex="6" Width="200" runat="Server" TextMode="MultiLine"
                                    Height="150"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Last name:</td>
                            <td>
                                <asp:TextBox ID="txtLastName" TabIndex="2" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtLastName" ErrorMessage="Last Name is required field.">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                Email:</td>
                            <td>
                                <asp:TextBox ID="txtEmail" TabIndex="3" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtEmail" ErrorMessage="Email is required field.">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                Company:</td>
                            <td>
                                <asp:TextBox ID="txtCompany" TabIndex="4" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCompany" ErrorMessage="Company is required field.">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                Phone:</td>
                            <td>
                                <asp:TextBox ID="txtPhone" TabIndex="5" runat="Server"></asp:TextBox><asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtPhone" ErrorMessage="Phone is required field.">*</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td colspan='3'>
                                <asp:Label ID="lblErrorMessage" runat="Server" Width="568px" Visible="False" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center" colspan='3'>
                                <asp:CheckBox ID="chkSendEmail" runat="Server" Text="Send Email" Checked="True"></asp:CheckBox></td>
                        </tr>
                        <tr>
                            <td align="center" colspan='3'>
                                <asp:Button ID="btnSendInvite" TabIndex="7" runat="Server" Text="Add Invite" OnClick="btnSendInvite_Click">
                                </asp:Button>&nbsp;
                                <asp:Button ID="btnClear" runat="Server" Text="Clear" CausesValidation="False" OnClick="btnClear_Click">
                                </asp:Button></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" align="center" colspan="2">
                    <span class="tableHeader">Leads Management</span><br />
                    <asp:DataGrid ID="dgdLeads" runat="Server" Width="100%" CssClass="grid" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" PageSize="100">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="Status"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FirstName" HeaderText="FN"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LastName" HeaderText="LN"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="company" HeaderText="CO">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.Company") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Phone" HeaderText="P#"></asp:BoundColumn>
                            <asp:BoundColumn DataField="SendDate" SortExpression="senddate" HeaderText="DS" DataFormatString="{0:M/d/yy HH:mm}">
                            </asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="opendate" HeaderText="Open">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpenDate" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.OpenDate", "{0:M/d/yy HH:mm}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="clickdate" HeaderText="Click">
                                <ItemTemplate>
                                    <asp:Label ID="lblClickDate" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.ClickDate", "{0:M/d/yy HH:mm}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="demodate" HeaderText="DD">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkDemoDate" Title='<%# InterpretDateString(Convert.ToString(DataBinder.Eval(Container.DataItem, "DemoSignUpDate"))) %>'
                                        Target="_blank" runat="Server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.EmailAddress", "../../Engines/ScheduleDemo.aspx?EmailAddress={0}")%> '
                                        runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.DemoDate", "{0:M/d/yy HH:mm}") %>'>
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Email">
                                <ItemTemplate>
                                    <a target="_blank" href='EmailLog.aspx?emailID=<%# DataBinder.Eval(Container.DataItem, "EmailID")%>&BlastID=<%# ecn.accounts.classes.LeadConfig.BlastID %>'>
                                        log</a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Survey">
                                <ItemTemplate>
                                    <a target="_blank" href='SurveyViewer.aspx?emailID=<%# DataBinder.Eval(Container.DataItem, "EmailID")%>&SurveyID=<%# ecn.accounts.classes.LeadConfig.DemoSurveyID %>'>
                                        result</a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <a href='../billingSystem/QuoteDetail.aspx?emailID=<%# DataBinder.Eval(Container.DataItem, "EmailID")%>'>
                                        quote</a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="Server" Text="Delete" CommandName="Delete"
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmailID") %>' CausesValidation="false">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="gridpager" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
        </table>
        <div class="tableContent" id="CallCountPanel">
            Call Count:
            <asp:TextBox ID="txtCallCount" runat="Server" Width="48px" MaxLength="6"></asp:TextBox>
            <asp:Button ID="btnUpdateCallCount" runat="Server" Text="Save" CausesValidation="False"
                OnClick="btnUpdateCallCount_Click"></asp:Button>
            <br />
            <br />
            <asp:Calendar ID="calCallDate" runat="Server" CssClass="tableContent" OnSelectionChanged="calCallDate_SelectionChanged">
            </asp:Calendar>
        </div>
</asp:content>

<%@ Page Language="c#" Inherits="ecn.accounts.main.leads.DemoSetup" CodeBehind="DemoSetup.aspx.cs"
    Title="DemoSetup" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
 <table width="100%" cellspacing="5" cellpadding="5">
            <tr bgcolor="#f4f4f4">
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" width="30%">
                    <asp:Calendar ID="calDemoDate" runat="Server" OnSelectionChanged="calDemoDate_SelectionChanged">
                    </asp:Calendar>
                </td>
                <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
                    border-bottom: gray 1px solid" valign="top">
                    <table width="100%" class="tableContent">
                        <tr>
                            <td colspan="2" height="24" align="center" bgcolor="#c0c0c0">
                                <asp:Label ID="lblMessage" CssClass="Tableheader" runat="Server" Width="496px"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                URL:</td>
                            <td>
                                <asp:TextBox ID="txtUrl" runat="Server" Width="374px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Conference call:</td>
                            <td>
                                <asp:TextBox ID="txtConferenceCall" runat="Server" Width="374px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                Meeting ID:</td>
                            <td>
                                <asp:TextBox ID="txtMeetingID" runat="Server" Width="374px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnSetup" runat="Server" Text="Set up demo" OnClick="btnSetup_Click">
                                </asp:Button>
                                <asp:Label ID="lblStatus" runat="Server" Width="278px"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr bgcolor="#f4f4f4">
                <td colspan="2" style="border-right: gray 1px solid; border-top: gray 1px solid;
                    border-left: gray 1px solid; border-bottom: gray 1px solid">
                    <asp:DataGrid ID="dgdLeads" runat="Server" AutoGenerateColumns="False" Width="100%"
                        CssClass="grid" AllowSorting="True">
                        <ItemStyle></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address"></asp:BoundColumn>
                            <asp:BoundColumn DataField="FirstName" HeaderText="FN"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LastName" HeaderText="LN"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Company" HeaderText="CO"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Phone" HeaderText="P#"></asp:BoundColumn>
                            <asp:TemplateColumn SortExpression="DemoDate" HeaderText="DD">
                                <ItemTemplate>
                                    <asp:Label ID="lblDemoDate" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.DemoDate", "{0:M/d/yy HH:mm}") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
</asp:content>

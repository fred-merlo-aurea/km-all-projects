<%@ Page Language="c#" Inherits="ecn.accounts.main.leads.demoManager" CodeBehind="demoManager.aspx.cs"
    Title="demoManager"  MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<table width="100%" class="tablecontent">
    <tr bgcolor="#f4f4f4">
        <td style="border-right: gray 1px solid; border-top: gray 1px solid; border-left: gray 1px solid;
            border-bottom: gray 1px solid">
            Filter By: From:<asp:TextBox ID="txtStart" runat="Server"></asp:TextBox>[mm/dd/yy]
            &nbsp;To:
            <asp:TextBox ID="txtEnd" runat="Server"></asp:TextBox>[mm/dd/yy]
            <asp:Button ID="btnRefine" runat="Server" Text="Refine" OnClick="btnRefine_Click"></asp:Button>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DataGrid ID="dgdLeads" runat="Server" AutoGenerateColumns="False" Width="100%"
                CssClass="grid" AllowSorting="True">
                <ItemStyle></ItemStyle>
                <HeaderStyle HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
                <Columns>
                    <asp:BoundColumn DataField="FirstName" HeaderText="FN"></asp:BoundColumn>
                    <asp:BoundColumn DataField="LastName" HeaderText="LN"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Company" HeaderText="CO"></asp:BoundColumn>
                    <asp:BoundColumn DataField="Phone" HeaderText="P#"></asp:BoundColumn>
                    <asp:TemplateColumn HeaderText="DD" SortExpression="DemoDate">
                        <ItemTemplate>
                            <asp:Label ID="lblDemoDate" runat="Server" Text='<%# DataBinder.Eval(Container, "DataItem.DemoDate", "{0:M/d/yy HH:mm}") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn Visible="False" DataField="Status"></asp:BoundColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnConfirm" Visible='<%# DataBinder.Eval(Container.DataItem, "Status").ToString() == ""%>'
                                CommandName="Done" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmailID")%>'
                                runat="Server">Done & Email Thankyou</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:LinkButton ID="Linkbutton1" Visible='<%# DataBinder.Eval(Container.DataItem, "Status").ToString() == ""%>'
                                CommandName="Absent" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmailID")%>'
                                runat="Server">Absent & FollowUp</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </td>
    </tr>
</table>
</asp:content>

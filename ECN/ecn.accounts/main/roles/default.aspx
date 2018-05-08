<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ecn.accounts.main.roles._default" %>

<%@ Register Assembly="ecn.controls" Namespace="ecn.controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <table style="width:40%;">


        <asp:Panel ID="pnlChannelRolesDropDown" runat="server">
            <tr>
                <td style="width:30%;text-align:left;">
                    <span style="font-weight: bold; color: black;" class="ECNLabel10">Channel:
       
                    </span>
                </td>
                <td style="width:70%;text-align:left;">
                     <asp:DropDownList ID="drpclientgroup" OnDataBound="drpclientgroup_DataBound"
            DataTextField="clientgroupname" DataValueField="clientgroupID" runat="server" CssClass="ECNLabel10" OnSelectedIndexChanged="drpclientgroup_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList>
                </td>
            </tr>

        </asp:Panel>
        <tr>
            <td style="width:30%;text-align:left;">
                <span style="font-weight: bold; color: black;" class="ECNLabel10">&nbsp;Customer:
       
                </span>
            </td>
            <td style="width:70%; text-align:left;">
                <asp:DropDownList ID="drpClient" DataTextField="clientName" DataValueField="clientID" runat="server" CssClass="ECNLabel10" OnSelectedIndexChanged="drpAccount_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>

    </table>
    <div style="float: right; padding-top: 20px;">
        <asp:Button ID="btnAddNewRole" runat="server" Text="Add New Role" OnClick="btnAddNewRole_Click" />
    </div>
    <asp:Panel ID="pnlChannelRoles" runat="server">
        <h2>Channel Roles</h2>

        <br />
        <cc1:ecnGridView ID="gvClientGroupSecurityGroups" runat="server" AutoGenerateColumns="false" Width="90%" OnRowDataBound="gvClientGroupSecurityGroups_RowDataBound">
            <Columns>
                <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnEdit" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditClientGroupSecurityGroup" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>--%>
                <asp:BoundField HeaderText="Role Name" DataField="SecurityGroupName" ItemStyle-Width="30%" />
                <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Width="20%" />
                <asp:BoundField HeaderText="Created Date" DataField="DateCreated" ItemStyle-Width="17%" />
                <asp:BoundField HeaderText="Updated Date" DataField="DateUpdated" ItemStyle-Width="17%" />

                <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active?" ItemStyle-Width="4%">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="cbIsActive" AutoPostBack="true" Checked="<%# Eval("IsActive") %>" />
                </ItemTemplate>
            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" />
                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("IsActive") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="4%">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# Eval("SecurityGroupID", "roledetail.aspx?SecurityGroupID={0}") %>'
                            Text="<center><img src=/ecn.images/images/icon-edits1.gif border='0' alt='Edit Channel Partner Details'></center>" />
                        <asp:HiddenField ID="hfAdministrativeLevel" runat="server" Value='<%# Eval("AdministrativeLevel") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </cc1:ecnGridView>
    </asp:Panel>
    <h2>Customer Roles</h2>
    <br />
    <cc1:ecnGridView ID="gvClientSecurityGroups" runat="server" AutoGenerateColumns="false" Width="90%" OnRowCommand="gvClientSecurityGroups_RowCommand" OnRowDataBound="gvClientSecurityGroups_RowDataBound">
        <Columns>
            <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%" >
                <ItemTemplate>
                    <asp:ImageButton ID="imgbtnEdit" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="EditClientSecurityGroup" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:BoundField HeaderText="Role Name" DataField="SecurityGroupName" ItemStyle-Width="30%" />
             <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Width="20%" />
            <asp:BoundField HeaderText="Created Date" DataField="DateCreated" ItemStyle-Width="17%" />
            <asp:BoundField HeaderText="Updated Date" DataField="DateUpdated" ItemStyle-Width="17%" />

            <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active?" ItemStyle-Width="4%">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="cbIsActive" AutoPostBack="true" Checked="<%# Eval("IsActive") %>" />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="Status" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" />
                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("IsActive") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Edit" ItemStyle-Width="4%">
                <ItemTemplate>
                    <asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# Eval("SecurityGroupID", "roledetail.aspx?SecurityGroupID={0}") %>'
                        Text="<center><img src=/ecn.images/images/icon-edits1.gif border='0' alt='Edit Channel Partner Details'></center>" />
                    <asp:HiddenField ID="hfCAdministrativeLevel" runat="server" Value='<%# Eval("AdministrativeLevel") %>' />
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </cc1:ecnGridView>
</asp:Content>

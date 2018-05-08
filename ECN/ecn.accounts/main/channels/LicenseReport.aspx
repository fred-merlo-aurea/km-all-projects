<%@ Page Language="c#" Inherits="ecn.accounts.main.channels.LicenseReport" CodeBehind="LicenseReport.aspx.cs"
    Title="LicenseReport" MasterPageFile="~/MasterPages/Accounts.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <style type="text/css">
        BODY
        {
            font-weight: normal;
            font-size: 11px;
            border-left-color: silver;
            border-bottom-color: silver;
            color: #000000;
            border-top-color: silver;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            border-right-color: silver;
        }
        DIV#Overview
        {
            padding-right: 10px;
            padding-left: 10px;
            padding-bottom: 10px;
            width: 200px;
            padding-top: 10px;
        }
        DIV#Overview SPAN
        {
            width: 100px;
        }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 
    <table class="tablecontent" width="800" style="border-right: gray 1px solid; border-top: gray 1px solid;
        border-left: gray 1px solid; border-bottom: gray 1px solid" bgcolor="#f4f4f4">
        <tr>
            <td colspan="4" align="center" height="25" valign="top">
                <p style="font-weight: bold; font-size: 14px; margin-bottom: 0px">
                    Filter By Create Date</p>
            </td>
            <tr>
                <td align="center">
                    Base Channels
                </td>
                <td align="center">
                    From&nbsp;[mm/dd/yy]
                </td>
                <td align="center">
                    To&nbsp;[mm/dd/yy]
                </td>
                <td align="center">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:DropDownList ID="ddlChannels" runat="Server" Width="173px" CssClass="formfield">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <asp:TextBox ID="txtStart" runat="Server" CssClass="formfield" Columns="15" MaxLength="10"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:TextBox ID="txtEnd" runat="Server" CssClass="formfield" Columns="15" MaxLength="10"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnRefine" runat="Server" Text="Refine" CssClass="formfield" OnClick="btnRefine_Click">
                    </asp:Button>
                </td>
            </tr>
    </table>
    <br />
    <asp:DataGrid ID="dgdCorpLicenses" runat="Server" AllowPaging="True" Width="100%"
        CssClass="grid" AutoGenerateColumns="False">        
        <FooterStyle CssClass="tableHeader1"></FooterStyle>
        <ItemStyle></ItemStyle>
        <SelectedItemStyle BackColor="#f4f4f4" />
        <HeaderStyle CssClass="gridheader"></HeaderStyle>
        <Columns>
            <asp:BoundColumn DataField="LicenseTypeCode" HeaderText="Type"></asp:BoundColumn>
            <asp:BoundColumn DataField="AddDate" HeaderText="Create" DataFormatString="{0:MM/dd/yyyy}">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ExpirationDate" HeaderText="Expiration" DataFormatString="{0:MM/dd/yyyy}">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Quantity" HeaderText="Quantity"></asp:BoundColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="Server" Text="View License Detail" CommandName="view" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CLID") %>'
                        CausesValidation="false">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    <br />
    <br />
    <asp:DataGrid ID="dgdDetailedUsage" AutoGenerateColumns="False" Width="100%" CssClass="grid"
        runat="Server" GridLines="Both">
        <FooterStyle HorizontalAlign="Center" CssClass="tableHeader"></FooterStyle>
        <ItemStyle></ItemStyle>
        <HeaderStyle HorizontalAlign="Center" CssClass="gridheader"></HeaderStyle>
        <Columns>
            <asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID"></asp:BoundColumn>
            <asp:BoundColumn DataField="Name" HeaderText="Customer"></asp:BoundColumn>
        </Columns>
    </asp:DataGrid> 
</asp:content>

<%@ Page Language="c#" Inherits="ecn.accounts.main.leads.Reports" CodeBehind="Reports.aspx.cs"
    Title="Reports"  MasterPageFile="~/MasterPages/Accounts.Master"%>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server"> 
        <asp:Label ID="lblMessage" runat="Server" Width="504px"></asp:Label><br />
        <div id="filter">
            Account Executive:
            <asp:DropDownList ID="ddlStaff" runat="Server" Width="176px">
            </asp:DropDownList>Start Date:
            <asp:TextBox ID="txtStartDate" runat="Server"></asp:TextBox>End Date:
            <asp:TextBox ID="txtEndDate" runat="Server"></asp:TextBox><asp:Button ID="btnSearch"
                runat="Server" Text="Refine" OnClick="btnSearch_Click"></asp:Button><br /> 
            <asp:CheckBox ID="chkDoNotShow" runat="Server" Text="Don't show reports for weeks without leads">
            </asp:CheckBox></div>
        <br />
        <asp:PlaceHolder ID="plhReports" runat="Server"></asp:PlaceHolder>
    <div id="summary">
        <span class="overview">Invites:
            <asp:Label ID="lblLeadsCount" runat="Server" Width="48px"></asp:Label>
        </span>
        <br />
        <span class="overview">Demos:
            <asp:Label ID="lblDemoCount" runat="Server" Width="48px"></asp:Label>
            Rate:
            <asp:Label ID="lblDemoRate" runat="Server" Width="48px"></asp:Label>
        </span>
        <br />
        <span class="overview">Quotes:
            <asp:Label ID="lblQuotes" runat="Server" Width="48px"></asp:Label>
            Rate:
            <asp:Label ID="lblQuoteRate" runat="Server" Width="48px"></asp:Label></span>
    </div>
</asp:content>

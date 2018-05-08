<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailPreviewUsage.ascx.cs" Inherits="ecn.communicator.main.Reports.ReportSettingsControls.EmailPreviewUsage" %>
<asp:Label ID="lblHeader" runat="server" Text="Customer Accounts" CssClass="label"></asp:Label><br />
 <asp:ListBox ID="lstboxCustomer" runat="server" Visible="false" SelectionMode="Multiple">
</asp:ListBox>
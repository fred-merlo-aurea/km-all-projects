<%@ Page Language="C#" MasterPageFile="~/MasterPages/Accounts.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="ecn.accounts.main.Notification.Notifications" %>

<%@ MasterType VirtualPath="~/MasterPages/Accounts.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:PlaceHolder ID="phError" runat="Server" Visible="false">
                <table cellspacing="0" cellpadding="0" width="674" align="center">
                    <tr>
                        <td id="errorTop"></td>
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
                        <td id="errorBottom"></td>
                    </tr>
                </table>
            </asp:PlaceHolder>
            <br />
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblNotifications" runat="server" Text="Notifications" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnAdd" runat="server" Text="Add Notifications" CssClass="formfield" OnClick="btnAdd_Click" />
                    </td>
                </tr>

            </table>
            <br />
            <table cellpadding="0" cellspacing="0" width="100%" border='0'>
                <tr>
                    <td>
                        <telerik:RadGrid AutoGenerateColumns="False" ID="rgNotifications" runat="server" PagerStyle-Mode="NextPrevNumericAndAdvanced" OnNeedDataSource="rgNotifications_NeedDataSource" OnDeleteCommand="rgNotifications_DeleteCommand">
                            <MasterTableView DataKeyNames="NotificationID" Width="100%" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="">
                                <HeaderStyle Font-Bold="true" Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                <ItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                <AlternatingItemStyle Font-Size="11px" Font-Names="Arial, Helvetica, Tahoma, sans-serif" />
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageSizes="2" />
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="NotificationName" DataField="NotificationName" UniqueName="NotificationName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="Last Updated UserID" DataField="UpdatedUserID" UniqueName="UpdatedUserID" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridBoundColumn HeaderText="StartDate" DataField="StartDate" UniqueName="StartDate" />
                                    <telerik:GridBoundColumn HeaderText="EndDate" DataField="StartDate" UniqueName="StartDate" />
                                    <telerik:GridTemplateColumn UniqueName="Edit" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <a href='NotificationsSetup.aspx?NotificationID=<%# DataBinder.Eval(Container.DataItem, "NotificationID") %>'>
                                                <center>
                                                    <img src="/ecn.images/images/icon-edits1.gif" alt='Edit Notification Information' border='0'></center>
                                            </a>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" FilterControlAltText="Delete"
                                        ImageUrl="/ecn.images/images/icon-delete1.gif" Text="Delete" UniqueName="DeleteColumn"
                                        Resizable="false" ConfirmText="Delete record?">
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

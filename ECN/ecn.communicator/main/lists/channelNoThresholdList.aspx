<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="channelNoThresholdList.aspx.cs"
    Inherits="ecn.communicator.main.lists.channelNoThresholdList" MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecn" TagName="uploader" Src="../../includes/uploader.ascx" %>
<%@ Register TagPrefix="cpanel" Namespace="BWare.UI.Web.WebControls" Assembly="BWare.UI.Web.WebControls.DataPanel" %>
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
     <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
        <tr>
            <td style="font-family: Arial; font-size: 12px; padding-left: 11px"  align="left"
                width="50%">
                Add Emails to be ignored by Threshold & Suppression for the Channel.<br />
                Use comma "," (or) semi-colon ";" (or) new line for separating email addresses.
            </td>
            <td style="font-family: Arial; font-size: 12px; font-weight: bold" align="right"
                valign="bottom" width="50%">
            </td>
        </tr>
        <tr>
            <td style="padding-left: 11px;"  align="left">
                <asp:TextBox ID="emailAddresses" runat="Server" EnableViewState="true" TextMode="multiline"
                    Columns="55" Rows="12" class="formfield" Width="400px" />
            </td>
            <td valign="bottom" style="font-family: Arial; font-size: 12px; float: left; font-weight: bold">
                <asp:Panel ID="importResultsPNL" runat="server" Visible="false">
                    RESULTS:
                    <br />
                    <asp:Label ID="MessageLabel" runat="Server" Visible="false"></asp:Label>
                    <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                        AutoGenerateColumns="False" Width="400px" Visible="false">
                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%">
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="float: left; padding-left: 11px">
                <asp:Button ID="addEmailBTN" runat="Server" Text="Add Emails to No Threshold" class="formbuttonsmall"
                    Visible="true" OnClick="addEmailBTN_Click" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <hr size="1" color="#999999">
    <table border="0" width="100%" style="float: left;" cellpadding="3px" bgcolor="#FFFFFF">
        <tr>
            <td align="right">
                <asp:Button ID="exportEmailsBTN" runat="Server" Text="Export Emails" class="formbuttonsmall"
                    Visible="true" OnClick="exportEmailsBTN_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid ID="emailsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                    AutoGenerateColumns="False" Width="100%" Visible="true" OnItemDataBound="emailsGrid_ItemDataBound"
                    OnItemCommand="emailsGrid_ItemCommand">
                    <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                    <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    <Columns>
                        <asp:BoundColumn DataField="EmailAddress" HeaderText="Email Address" HeaderStyle-Width="75%"
                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                        <asp:BoundColumn DataField="DateAdded" HeaderText="Date Addred" HeaderStyle-Width="20%"
                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" />
                        <asp:TemplateColumn HeaderText="Delete" HeaderStyle-Width="5%" ItemStyle-Width="5%"
                            HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                                <asp:LinkButton runat="Server" Text="&lt;img src=/ecn.images/images/icon-delete1.gif alt='Delete Content' border='0'&gt;"
                                    CausesValidation="false" ID="deleteEmailBTN" CommandName="deleteEmail" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.EmailAddress") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <AU:PagerBuilder ID="emailsPager" runat="Server" Width="100%" ControlToPage="emailsGrid"
                    PageSize="100">
                    <PagerStyle CssClass="gridpager"></PagerStyle>
                </AU:PagerBuilder>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

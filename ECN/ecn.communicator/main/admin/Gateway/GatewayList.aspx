<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="GatewayList.aspx.cs" Inherits="ecn.communicator.main.admin.Gateway.GatewayList" %>
<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel4" CssClass="loader" runat="server">
                    <div>
                        <center>
                            <br />
                            <br />
                            <b>Processing...</b><br />
                            <br />
                            <img src="http://images.ecn5.com/images/loading.gif" alt="" /><br />
                            <br />
                            <br />
                            <br />
                        </center>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
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

            <asp:Panel runat="server" ID="pnlNoAccess" Visible="false">
                <div style="padding-top: 150px; padding-bottom: 150px; text-align: center; font-size: large;">
                    <asp:Label ID="Label1" runat="server" Text="You do not have access to this page. Please contact your Basechannel Administrator"></asp:Label>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMain" runat="server" Width="100%">
                <table style="width:80%;">
                    <tr>
                        <td style="text-align:left;">
                            <asp:Label ID="lblHeading" runat="server" Text="Gateway" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <br />
                            <br />
                        </td>
                        <td style="text-align:right;">
                            <asp:Button ID="btnAddNewGateway" runat="server" Text="Add New Gateway" OnClick="btnAddNewGateway_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvGateways" runat="server" Width="100%" OnRowCommand="gvGateways_RowCommand" OnRowDataBound="gvGateways_RowDataBound" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="PubCode" HeaderText="Pub Code" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="TypeCode" HeaderText="Type" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:TemplateField HeaderText="URL">
                                        <ItemTemplate>
                                            <asp:Label ID="lblURLForGateway" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnEditGateway" runat="server" ImageUrl="/ecn.images/images/icon-edits1.gif" CommandName="editgateway" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnDeleteGateway" runat="server" OnClientClick="return confirm('Are you sure you want to delete this gateway?');" ImageUrl="/ecn.images/images/icon-delete1.gif" CommandName="deletegateway" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

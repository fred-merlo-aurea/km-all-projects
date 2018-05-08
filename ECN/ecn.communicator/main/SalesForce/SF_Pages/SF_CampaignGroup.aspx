<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_CampaignGroup.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_CampaignGroup" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlCampGroup" runat="server">
        <table style="width: 100%;">
            <tr>
                <td style="text-align: left;" colspan="3">
                    <asp:Label ID="lblHeader" Text="Import Salesforce Campaign Members" runat="server" CssClass="ECN-PageHeading" />
                </td>
            </tr>
            <tr>
                <td style="width: 40%; text-align: center;">
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFFrom" Text="From:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSFFrom" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceSFFrom" runat="server" TargetControlID="txtSFFrom" Format="MM/dd/yyyy" />
                                        </td>
                                        <td style="padding-left: 20px;">
                                            <asp:Label ID="lblSFTo" Text="To:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSFTo" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceSFTo" runat="server" TargetControlID="txtSFTo" Format="MM/dd/yyyy" />
                                        </td>
                                        <td style="padding-left: 20px;">
                                            <asp:Button ID="btnSFDateFilter" runat="server" Text="Search" OnClick="btnSFDateFilter_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;width:20%;">
                                <asp:Label ID="lblSFCampaign" Text="SF Campaign:" runat="server" />
                            </td>
                            <td style="text-align: left;width:80%;">
                                <asp:DropDownList ID="ddlSFCampaign" runat="server" Width="100%" OnSelectedIndexChanged="ddlSFCampaign_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                        </tr>

                    </table>
                </td>
                <td style="width: 20%; text-align: center; padding: 10px;">
                    <asp:Button ID="btnSyncGroup" Text="Import" CssClass="formbutton" runat="server" Enabled="false" OnClick="btnSyncGroup_Click" />
                </td>
                <td style="width: 40%; text-align: center;">
                    <asp:UpdatePanel ID="upECNGroup" runat="server" ChildrenAsTriggers="true">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="rblECNGroup" />
                            <asp:PostBackTrigger ControlID="ddlECNFolder" />
                            <asp:PostBackTrigger ControlID="ddlExistingGroup" />
                            <asp:PostBackTrigger ControlID="btnSyncGroup" />
                        </Triggers>
                        <ContentTemplate>

                            <table style="width: 100%;">
                                
                                <tr>
                                    <td></td>
                                    <td style="text-align: left;">
                                        <asp:RadioButtonList ID="rblECNGroup" runat="server" CellSpacing="0" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblECNGroup_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Existing" Value="existing" Selected="True" />
                                            <asp:ListItem Text="New" Value="new" />

                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblECNFolder" Text="Folder:" runat="server" />
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList ID="ddlECNFolder" Style="max-width: 200px;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlECNFolder_SelectedIndexChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblNewGroup" runat="server" Visible="false" Text="New Group Name:" />
                                        <asp:Label ID="lblExistingGroup" runat="server" Text="Group:" />
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox ID="txtNewGroup" Visible="false" runat="server" />
                                        <asp:DropDownList ID="ddlExistingGroup" Style="max-width: 200px;" OnSelectedIndexChanged="ddlExistingGroup_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" CssClass="popupbody">
        <table>
            <tr>
                <td>
                    <asp:Label ID="MessageLabel" runat="Server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="ResultsGrid" runat="Server" CssClass="grid" CellPadding="2" AllowSorting="True"
                        AutoGenerateColumns="False" Width="400px">
                        <AlternatingItemStyle CssClass="gridaltrow"></AlternatingItemStyle>
                        <HeaderStyle CssClass="gridheader" HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="Action" HeaderText="Description" ItemStyle-Width="75%"
                                HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Totals" HeaderText="Totals" ItemStyle-Width="25%" HeaderStyle-Width="25%"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnResultsClose" Text="Close" CssClass="formbutton" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Button ID="hfResults" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" TargetControlID="hfResults" runat="server" CancelControlID="btnResultsClose" PopupControlID="pnlResults"></ajaxToolkit:ModalPopupExtender>
    <KM:Message ID="kmMsg" runat="server" />
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="ManageCampaigns.aspx.cs" Inherits="ecn.communicator.main.blasts.ManageCampaigns" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/ecn.communicator/MasterPages/ECN_Controls.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <asp:Panel ID="pnlContent" runat="server">
        <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <table style="width: 100%;">
                    <tr>
                        <td style="padding: 10px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="width: 90%; padding-bottom: 10px;">
                                <tr>
                                    <td style="width: 25%; font-size: 14px;">Campaign Name to Search
                                    </td>
                                    <td style="width: 15%; text-align: left;">
                                        <asp:TextBox ID="txtCampaignNameSearch" runat="server" />
                                    </td>
                                    <td style="width: 10%; padding-left: 5px;">
                                        <asp:DropDownList ID="ddlArchive" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArchive_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="all" />
                                            <asp:ListItem Text="Active" Value="active" Selected="True" />
                                            <asp:ListItem Text="Archived" Value="archived" />
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 35%; text-align: right;">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:Button ID="btnClearSearch" runat="server" Text="Clear Search" OnClick="btnClearSearch_Click" />
                                    </td>
                                </tr>
                            </table>

                        </td>

                    </tr>
                    <tr>
                        <td>
                            <ecnCustom:ecnGridView ID="gvCampaigns" CssClass="ECN-GridView" DataKeyNames="CampaignID" CellPadding="5" Width="95%" OnRowDataBound="gvCampaigns_RowDataBound" AutoGenerateColumns="false" OnRowCommand="gvCampaigns_RowCommand" AllowPaging="true" PageSize="10" runat="server">
                                <%--OnPageIndexChanging="gvCampaigns_PageIndexChanging">--%>
                                <Columns>
                                    <asp:BoundField HeaderText="Campaign" ItemStyle-Width="54%" DataField="CampaignName" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="CI Sent" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSent" runat="server" OnClick="lbSent_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CI Pending" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbPending" runat="server" OnClick="lbPending_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CI Saved" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbSaved" runat="server" OnClick="lbSaved_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Archive" ItemStyle-Width="9%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkArchive" runat="server" AutoPostBack="true" OnCheckedChanged="chkArchive_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <ul class="ECN-InfoLinks" style="padding-left: 0px; padding-top: 10px; padding-right: 0px; display: inline;">
                                                <li style="padding-top: 0px; text-align: center; padding-bottom: 0px; padding-left: 20px;">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/ecn-icon-gear-small.png" />
                                                    <ul style="width: 200px; left: 20px;text-align:left;">
                                                        <li>
                                                            <asp:LinkButton ID="lbEditCampaignName" runat="server" Text="Edit Campaign Name" CssClass="aspBtn" OnClick="btnEditCampaignName_Click" /></li>
                                                        <li id="liDeleteCampaign" runat="server">
                                                            <asp:LinkButton ID="lbDeleteCampaign" runat="server" Text="Delete Campaign" CssClass="aspBtn" OnClick="btnDeleteCampaign_Click" /></li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <PagerTemplate>
                                    <table cellpadding="0" border="0" width="100%">
                                        <tr>
                                            <td align="left" class="label" width="31%">Total Records:
                                                <asp:Label ID="lblTotalRecords" runat="server" Text="" />
                                            </td>
                                            <td align="left" class="label" width="25%">Show Rows:
                                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="formfield" OnSelectedIndexChanged="gvCampaigns_SelectedIndexChanged">

                                                    <asp:ListItem Value="5" />
                                                    <asp:ListItem Value="10" />
                                                    <asp:ListItem Value="15" />
                                                    <asp:ListItem Value="20" />
                                                    <asp:ListItem Value="30" />
                                                    <asp:ListItem Value="40" />
                                                    <asp:ListItem Value="50" />
                                                    <asp:ListItem Value="100" />
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right" class="label" width="44%">Page
                                                <asp:TextBox ID="txtGoToPage" runat="server" AutoPostBack="true" class="formtextfield" Width="30px" OnTextChanged="GoToPage_TextChanged" />

                                                of
                                                <asp:Label ID="lblTotalNumberOfPages" runat="server" CssClass="label" />
                                                &nbsp;
                                                <asp:Button ID="btnPrevious" runat="server" CommandName="Page" ToolTip="Previous Page"
                                                    CommandArgument="Prev" class="formbuttonsmall" Text="<< Previous" OnClick="btnPrevious_Click" />
                                                <asp:Button ID="btnNext" runat="server" CommandName="Page" ToolTip="Next Page"
                                                    CommandArgument="Next" class="formbuttonsmall" Text="Next >>" OnClick="btnNext_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </PagerTemplate>
                            </ecnCustom:ecnGridView>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Button ID="hfEditCampaign" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeEditCampaign" PopupControlID="upEditCampaign" TargetControlID="hfEditCampaign" BackgroundCssClass="modalBackground" CancelControlID="btnCancelEdit" runat="server" />

    <asp:UpdatePanel ID="upEditCampaign" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveCampaign" />
            <asp:PostBackTrigger ControlID="btnCancelEdit" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color: white; padding: 5px; border-radius: 5px;">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblEditHeading" runat="server" CssClass="ECN-Label-Heading-Large" Text="Edit Campaign Name" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text="Campaign Name:" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" CssClass="formTextBox" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 50%; text-align: center;">
                                    <asp:Button ID="btnSaveCampaign" runat="server" OnClick="btnSaveCampaign_Click" Text="Save" />
                                </td>
                                <td style="width: 50%; text-align: center;">
                                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="hfMove" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeMove" BackgroundCssClass="modalBackground" PopupControlID="upMove" TargetControlID="hfMove" CancelControlID="btnCancelMove" runat="server" />
    <asp:UpdatePanel ID="upMove" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnMoveCampaign" />
            <asp:PostBackTrigger ControlID="btnCancelMove" />
        </Triggers>
        <ContentTemplate>
            <table style="background-color: white; padding: 5px; border-radius: 5px;">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMoveHeading" CssClass="ECN-Label-Heading-Large" runat="server" Text="Move Campaign Items" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCampaigns" runat="server" Text="Campaigns:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCampaigns" AutoPostBack="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnMoveCampaign" runat="server" Text="Move" OnClick="btnMoveCampaign_Click" />
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button ID="btnCancelMove" runat="server" Text="Cancel" />
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="hfDeleteCampaign" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeDelete" runat="server" TargetControlID="hfDeleteCampaign" BackgroundCssClass="modalBackground" PopupControlID="pnlDeleteCampaign" />
    <asp:Panel ID="pnlDeleteCampaign" Height="150px" Width="200px" runat="server">
        <table style="width: 100%; height: 100%; background-color: white; padding: 5px; border-radius: 5px;">
            <tr>
                <td style="width: 100%;">
                    <asp:Label ID="lblDeleteHeading" runat="server" Text="Delete Campaign" CssClass="ECN-Label-Heading-Large" />
                    
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td colspan="2" style="padding-bottom:20px;">Are you sure you want to delete this Campaign?
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: center;">
                                <asp:Button ID="btnDeleteConfirm" runat="server" Text="Delete" OnClick="btnDeleteConfirm_Click" />
                            </td>
                            <td style="width: 50%; text-align: center;">
                                <asp:Button ID="btnCancelDelete" runat="server" Text="Cancel" OnClick="btnCancelDelete_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

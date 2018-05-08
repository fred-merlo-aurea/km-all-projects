<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="CampaignSegmentation.aspx.cs" Inherits="KMPS.MD.Main.CampaignSegmentation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<%@ Register TagName="Download" TagPrefix="CC1" Src="~/Controls/DownloadPanel.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdateProgress ID="uProgress" runat="server" DisplayAfter="10" Visible="true"
        AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
        <ProgressTemplate>
            <div class="TransparentGrayBackground">
            </div>
            <div id="divprogress" class="UpdateProgress" style="position: absolute; z-index: 10; color: black; font-size: x-small; background-color: #F4F3E1; border: solid 2px Black; text-align: center; width: 100px; height: 100px; left: expression((this.offsetParent.clientWidth/2)-(this.clientWidth/2)+this.offsetParent.scrollLeft); top: expression((this.offsetParent.clientHeight/2)-(this.clientHeight/2)+this.offsetParent.scrollTop);">
                <br />
                <b>Processing...</b><br />
                <br />
                <img src="../images/loading.gif" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <CC1:Download runat="server" ID="DownloadPanel1" Visible="false" ViewType="ConsensusView"></CC1:Download>
            <table cellpadding="5" cellspacing="5" border="0" width="100%" align="left">
                <tr>
                    <td></td>
                    <td colspan="2">
                        <div id="divErrorMsg" runat="Server" visible="false">
                            <table cellspacing="0" cellpadding="0" width="100%" align="left">
                                <tr>
                                    <td id="errorMiddle">
                                        <table width="80%">
                                            <tr>
                                                <td valign="top" align="center" width="5%">
                                                    <img id="Img3" style="padding: 0 0 0 15px;" src="~/images/ic-error.gif" runat="server"
                                                        alt="" />
                                                </td>
                                                <td valign="middle" align="left" width="95%" height="100%">
                                                    <asp:Label ID="lblErrorMsg" runat="Server" Font-Bold="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <asp:Panel ID="pnlBrand" runat="server" Visible="false">
                    <tr>
                        <td align="right">
                            <b>Brand</b></td>
                        <td colspan="2">
                            <asp:Label ID="lblBrandName" runat="server" Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlBrand" runat="server" Font-Names="Arial" Font-Size="x-small" Visible="false"
                                AutoPostBack="true" Style="text-transform: uppercase" DataTextField="BrandName"
                                DataValueField="BrandID" Width="150px" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hfBrandID" runat="server" Value="0" />
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td width="10%" align="right"><b>Operation</b></td>
                    <td colspan="2" width="80%">
                        <asp:DropDownList ID="ddlOperation" Width="100" runat="server" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Selected="true" Value=""></asp:ListItem>
                            <asp:ListItem Value="Intersect">Intersect</asp:ListItem>
                            <asp:ListItem Value="Union">Union</asp:ListItem>
                            <asp:ListItem Value="NotIn">Not In</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rvOperation" runat="server" ControlToValidate="ddlOperation" 
                            ErrorMessage=" *" ForeColor="Red" ValidationGroup="Report"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="10%"></td>
                    <td width="80%">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" align="left">
                            <tr>
                                <td><asp:Label ID="lblCampaign" runat="Server" Text="Campaigns" Font-Bold="true"></asp:Label>&nbsp;<br />
                                    <br />
                                    <asp:ListBox ID="lstCampaignIn" runat="server" Rows="10" Style="text-transform: uppercase;" DataTextField="CampaignName" DataValueField="CampaignID"
                                        SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"></asp:ListBox>
                                </td>
                                <asp:PlaceHolder ID="phOperationNotIn" runat="server" Visible="false">
                                    <td width="75%">
                                        <b>Campaigns Not In</b><br />
                                        <br />
                                        <asp:ListBox ID="lstCampaignNotIn" runat="server" Rows="10" Style="text-transform: uppercase;" DataTextField="CampaignName" DataValueField="CampaignID"
                                            SelectionMode="Multiple" Font-Size="X-Small" Font-Names="Arial" Width="350px" Height="300px"></asp:ListBox>
                                    </td>
                                </asp:PlaceHolder>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <asp:Button ID="btnViewReport" runat="server" Text="View Report" CssClass="buttonMedium" OnClick="btnReport_Click" ValidationGroup="Report" />&nbsp;&nbsp;
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="button" OnClick="btnReset_Click" />
                    </td>
                </tr>
                <asp:PlaceHolder ID="phResults" runat="server" Visible="false">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="Label1" runat="Server" Text="Counts :" Font-Size="20px"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkdownload" runat="server" CommandName="download" Text="" Font-Size="20px" OnClick="btnDownload_Click"></asp:LinkButton>
                            <asp:HiddenField ID="hfSelected" runat="server" Value="" />
                            <asp:HiddenField ID="hfSuppressed" runat="server" Value="" />
                            <asp:HiddenField ID="hfOperation" runat="server" Value="0" />
                        </td>
                    </tr>
                </asp:PlaceHolder>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

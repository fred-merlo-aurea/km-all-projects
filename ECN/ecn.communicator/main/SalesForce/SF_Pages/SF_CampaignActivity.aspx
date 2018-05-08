<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="SF_CampaignActivity.aspx.cs" Inherits="ecn.communicator.main.Salesforce.SF_Pages.SF_CampaignActivity" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlCampaignActivity" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="lblHeader" Text="Sync Email Activity" runat="server" CssClass="ECN-PageHeading" />
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="vertical-align: middle; width: 100%;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSFFrom" Text="From:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSFFrom" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceSFFrom" runat="server" TargetControlID="txtSFFrom" Format="MM/dd/yyyy" />
                                        </td>
                                        <td style="padding-left:20px;">
                                            <asp:Label ID="lblSFTo" Text="To:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSFTo" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceSFTo" runat="server" TargetControlID="txtSFTo" Format="MM/dd/yyyy" />
                                        </td>
                                        <td style="padding-left:20px;">
                                            <asp:Button ID="btnSFDateFilter" runat="server" Text="Search" OnClick="btnSFDateFilter_Click" />
                                        </td>
                                    </tr>
                                </table>


                            </td>

                        </tr>
                        <tr>

                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 30%;">
                                            <asp:Label ID="lblSFCampaign" Text="Salesforce Campaign:" runat="server" />
                                        </td>
                                        <td style="width: 70%; max-width:210px;">
                                            <asp:DropDownList ID="ddlSFCampaign" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>

                            </td>

                        </tr>
                    </table>
                </td>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 20%; background-image: url(http://images.ecn5.com/images/ArrowLeft_Orange.png); background-repeat: no-repeat; background-position-x: center; background-position-y: center;"></td>
                            <td style="width: 60%; text-align: center;">
                                <asp:Button ID="btnSyncCampaign" runat="server" Width="150px" CssClass="formbutton" OnClick="btnSyncCampaign_Click" Text="Sync Activity" />
                            </td>
                            <td style="width: 20%; background-image: url(http://images.ecn5.com/images/ArrowLeft_Orange.png); background-repeat: no-repeat; background-position-x: center; background-position-y: center;"></td>
                        </tr>
                    </table>

                </td>
                <td>
                    <table style="width: 100%;">
                        <tr>
                            <td style="vertical-align: middle;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblECNFrom" Text="From:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtECNFrom" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceECNFrom" TargetControlID="txtECNFrom" runat="server" Format="MM/dd/yyyy" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblECNTo" Text="To:" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtECNTo" Width="70px" runat="server" />
                                            <ajaxToolkit:CalendarExtender ID="ceECNTo" TargetControlID="txtECNTo" runat="server" Format="MM/dd/yyyy" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnECNDateFilter" Text="Search" runat="server" OnClick="btnECNDateFilter_Click" />
                                        </td>
                                    </tr>
                                </table>


                            </td>

                        </tr>

                        <tr>
                            <td colspan="2">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblECNCampItem" Text="Campaign Item:" runat="server" />
                                        </td>
                                        <td style="width: 75%;">
                                            <asp:DropDownList ID="ddlECNCampItem" Width="200px" runat="server" />
                                        </td>
                                    </tr>
                                </table>

                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlResults" runat="server" Width="350px" Height="250px" BorderColor="Gray" BorderStyle="Solid" BorderWidth="2px" CssClass="popupbody">
        <table style="padding-top: 5px; background-color: #e6e7e8; border-top-width: 5px; border-collapse: collapse; width: 100%; height: 100%;">
            <tr>
                <td style="background-image:url(http://images.ecn5.com/images/Gradient_DarkBlue.jpg)">
                    <asp:Label ID="lblStatusHead" ForeColor="White" Font-Bold="true" Text="Status Code" runat="server" />
                </td>
                
                <td style="background-image: url(http://images.ecn5.com/images/Gradient_DarkBlue.jpg);">
                    <asp:Label ID="lblTotalHead" ForeColor="White" Font-Bold="true" Text="Total" runat="server" />
                </td>
            </tr>
            <tr style="border-top: 3px solid orange;">
                <td>
                    <asp:Label ID="lblOpens" Text="Opens" runat="server" />
                </td>
                
                <td style="text-align: center;">
                    <asp:Label ID="lblOpenTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblClick" Text="Clicks" runat="server" />
                </td>
               
                <td style="text-align: center;">
                    <asp:Label ID="lblClickTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblBounce" Text="Bounces" runat="server" />
                </td>
                
                <td style="text-align: center;">
                    <asp:Label ID="lblBounceTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUnsub" Text="Unsubscribes" runat="server" />
                </td>
                
                <td style="text-align: center;">
                    <asp:Label ID="lblUnsubTotal" runat="server" />
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUnsubSuccess" Text="Emails Unsubscribed" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblUnsubSuccessCount" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMasterS" Text="Master Suppressed" runat="server" />
                </td>
                
                <td>
                    <asp:Label ID="lblMSTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label id="lblMSSuccess" Text="Contacts Master Suppressed" runat="server" />
                </td>
                <td>
                    <asp:Label ID="lblMSSuccessCount" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSuccess" Text="Campaign Members updated" runat="server" />
                </td>
                <td style="text-align: center;">
                    <asp:Label ID="lblSuccessTotal" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3" style="text-align: center; padding-top: 10px; padding-bottom: 10px;">
                    <asp:Button ID="btnResultsClose" runat="server" CssClass="formbutton" Text="Close" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Button ID="hfCompare" runat="server" style="display:none;" />
    <ajaxToolkit:ModalPopupExtender ID="mpeResults" BackgroundCssClass="modalBackground" PopupControlID="pnlResults" TargetControlID="hfCompare" CancelControlID="btnResultsClose" runat="server"></ajaxToolkit:ModalPopupExtender>
    <KM:Message ID="kmMsg" runat="server" />
</asp:Content>

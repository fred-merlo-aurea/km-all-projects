<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ecn.communicator.main._default" MasterPageFile="~/MasterPages/Communicator.Master" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        $(function() {
            $(".subMenu").hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br/>
    <table width="100%">
        <tr align="left">
            <td>
                <asp:Label ID="Label1" runat="server" Text="Email Marketing" CssClass="EcnPageTitle"></asp:Label>
            </td>
        </tr>
        <tr align="left">
            <td style="padding-left: 100px;">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <br/> <br/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <a href="/ecn.communicator/main/ecnwizard/default.aspx">
                                <asp:Image ID="Image1" runat="server" src="/ecn.communicator/images/ecn-icon-campaignitems.png" Width="48" Height="48"/>
                            </a>
                        </td>
                        <td align="left" width="40%">
                            <table align="left">
                                <tr>
                                    <td align="left">
                                        <a href="/ecn.communicator/main/ecnwizard/default.aspx">
                                            <asp:Label ID="Label6" runat="server" Text="Manage Campaign Items" CssClass="EcnSectionHeader"/>
                                        </a>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left">
                                        <asp:Label ID="Label7" runat="server" Text="Manage email campaigns, envelopes and campaign item templates" CssClass="EcnSectionDetails" Visible="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <a href="/ecn.communicator/main/content/default.aspx">
                                <asp:Image ID="Image2" runat="server" src="/ecn.communicator/images/ecn-icon-content.png" Width="48" Height="48"/>
                            </a>
                        </td>
                        <td align="left" width="40%">
                            <table align="left">
                                <tr>
                                    <td align="left">
                                        <a href="/ecn.communicator/main/content/default.aspx">
                                            <asp:Label ID="Label3" runat="server" Text="Manage Content & Messages" CssClass="EcnSectionHeader"/>
                                        </a>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left">
                                        <asp:Label ID="Label8" runat="server" Text="Create and edit content, messages, dynamic tags and images" CssClass="EcnSectionDetails" Visible="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left">


                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <br/>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <a href="/ecn.communicator.mvc/Group">
                                <asp:Image ID="Image3" runat="server" src="/ecn.communicator/images/ecn-icon-groups.png" Width="48" Height="48"/>
                            </a>
                        </td>
                        <td align="left" width="40%">
                            <table align="left">
                                <tr>
                                    <td align="left">
                                        <a href="/ecn.communicator.mvc/Group">
                                            <span style="font-family: SourceSansPro-Bold;">
                                                <asp:Label ID="Label2" runat="server" Text="Manage Groups" CssClass="EcnSectionHeader"/>
                                            </span>
                                        </a>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left">
                                        <asp:Label ID="Label9" runat="server" Text="Manage subscribers, subscriber groups and suppression lists" CssClass="EcnSectionDetails" Visible="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <a href="/ecn.communicator/main/Reports/SentCampaignsReport.aspx#">
                                <asp:Image ID="Image4" runat="server" src="/ecn.communicator/images/ecn-icon-reports.png" Width="45" Height="40"/>
                            </a>
                        </td>
                        <td align="left" width="40%">
                            <table align="left">
                                <tr>
                                    <td align="left">
                                        <a href="/ecn.communicator/main/Reports/SentCampaignsReport.aspx#">
                                            <asp:Label ID="Label5" runat="server" Text="View Reports" CssClass="EcnSectionHeader"/>
                                        </a>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td align="left">
                                        <asp:Label ID="Label10" runat="server" Text="Get access to reporting on your email campaigns and subscriber groups" CssClass="EcnSectionDetails" Visible="true"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br/> <br/> <br/> <br/>
</asp:Content>
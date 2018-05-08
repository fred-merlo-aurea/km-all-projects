<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KMPS.MD.Default" MasterPageFile="~/MasterPages/Site.master" %>

<%@ MasterType VirtualPath="~/MasterPages/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .modalPopup {
            background-color: white;
            border-width: 3px;
            border-style: solid;
            border-color: white;
            padding: 3px;
            overflow: auto;
        }

        .TransparentGrayBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
            height: 100%;
            width: 100%;
            min-height: 100%;
            min-width: 100%;
        }

        fieldset {
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
        }

        .overlay {
            position: fixed;
            z-index: 99;
            top: 0px;
            left: 0px;
            background-color: gray;
            width: 100%;
            height: 100%;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
        }

        * html .overlay {
            position: absolute;
            height: expression(document.body.scrollHeight > document.body.offsetHeight ? document.body.scrollHeight : document.body.offsetHeight + 'px');
            width: expression(document.body.scrollWidth > document.body.offsetWidth ? document.body.scrollWidth : document.body.offsetWidth + 'px');
        }

        .loader {
            z-index: 100;
            position: fixed;
            width: 120px;
            margin-left: -60px;
            background-color: #F4F3E1;
            font-size: x-small;
            color: black;
            border: solid 2px Black;
            top: 40%;
            left: 50%;
        }

        * html .loader {
            position: absolute;
            margin-top: expression((document.body.scrollHeight / 4) + (0 - parseInt(this.offsetParent.clientHeight / 2) + (document.documentElement && document.documentElement.scrollTop || document.body.scrollTop)) + 'px');
        }

        .SectionHeader, a.SectionHeader:visited, a.SectionHeader:link, a.SectionHeader:active {
            font-family: Arial;
            font-size: 14px;
            color: #696969;
            text-decoration: none;
            font-weight: bold;
        }

        .SectionHeader:hover {
            text-decoration: underline;
        }

        .SectionDetails {
            font-family: Arial;
            font-size: 13px;
            color: #9da2a7;
            font-weight: normal;
        }
    </style>
    <div style="text-align: right">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <asp:Image ID="Image1" ImageUrl="Images/progress.gif" runat="server" />
                Processing....
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div id="platformapps">
                <table cellspacing="0" cellpadding="0" border="0" align="center" width="960px">
                    <tr>
                        <td>
                            <table cellspacing="4" cellpadding="4" width="100%">
                                <tr style="height: 120px;">
                                    <td width="10%">
                                        <a href="AudienceViews/report.aspx?ViewType=ConsensusView" id="hlConsensusImg" runat="server">
                                            <asp:Image ID="Image1" runat="server" src="images/consensus.png" Width="60" Height="40" />
                                        </a>
                                    </td>
                                    <td width="40%">
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <a href="AudienceViews/report.aspx?ViewType=ConsensusView" id="hlConsensus" runat="server" style="text-decoration:none">
                                                        <asp:Label ID="Label1" runat="server" Text="Consensus View" CssClass="SectionHeader"></asp:Label>
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="left">
                                                    <asp:Label ID="Label2" runat="server" Text="Single database that aggregates, de-dupes, and reconciles an individual’s information from multiple sources into one master profile" CssClass="SectionDetails" Visible="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="10%">
                                        <a href="AudienceViews/report.aspx?ViewType=RecencyView" id="hlRecencyImg" runat="server" >
                                            <asp:Image ID="Image2" runat="server" src="images/recencyview.png" Width="60" Height="40" />
                                        </a>
                                    </td>
                                    <td width="40%">
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <a runat="server" id="hlRecency" href="AudienceViews/report.aspx?ViewType=RecencyView" style="text-decoration:none">
                                                        <asp:Label ID="Label17" runat="server" Text="Recency View" CssClass="SectionHeader"></asp:Label>
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="left">
                                                    <asp:Label ID="Label4" runat="server" Text="Audience segmentation based on the most recent demographic data" CssClass="SectionDetails" Visible="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 120px;">
                                    <td>
                                        <a href="AudienceViews/productview.aspx" id="hlProductImg" runat="server">
                                            <asp:Image ID="Image4" runat="server" src="images/ProductView.png" Width="60" Height="40" />
                                        </a>
                                    </td>
                                    <td>
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <a href="AudienceViews/productview.aspx" id="hlProduct" runat="server" style="text-decoration:none">
                                                        <asp:Label ID="Label7" runat="server" Text="Product View" CssClass="SectionHeader"></asp:Label>
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="left">
                                                    <asp:Label ID="Label8" runat="server" Text="Product level statistics based on a single product" CssClass="SectionDetails" Visible="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <a href="AudienceViews/crossproductview.aspx" id="hlCrossProductImg" runat="server">
                                            <asp:Image ID="Image5" runat="server" src="images/crossproductview.png" Width="60" Height="40" />
                                        </a>
                                    </td>
                                    <td>
                                        <table align="left">
                                            <tr>
                                                <td align="left">
                                                    <a href="AudienceViews/crossproductview.aspx" id="hlCrossProduct" runat="server" style="text-decoration:none">
                                                        <asp:Label ID="Label6" runat="server" Text="Cross Product View" CssClass="SectionHeader"></asp:Label>
                                                    </a>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td align="left">
                                                    <asp:Label ID="Label10" runat="server" Text="Compare product level statistics based on 2 or more individual products" CssClass="SectionDetails" Visible="true"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

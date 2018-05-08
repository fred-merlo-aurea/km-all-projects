<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ecn.communicator.main.lists.reports._default"  MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
    <tr>
        <td valign="top" style="padding-left: 25px" align="left">
            <ul style="margin-top: 1em;">
                <li style="padding-bottom: 10px;"><a style="font-size:12px" href="GroupStatisticsReport.aspx">Group Statistics
                    Report</a><br />
                    <div class="formLabel">
                        Summary report, up to a six month period, of all blasts sent to a specific group.
                        Includes delivery success with opens and clicks.</div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="AudienceEngagementReport.aspx">Audience Engagement
                    Report</a><br />
                    <div class="formLabel">
                        Group analysis of reader activity and inactivity within one year.
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="StatisticsbyField.aspx">Blast Statistics Report</a><br />
                    <div class="formLabel">
                        Summary report of demographic data for a specific blast.
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="OnOffReport.aspx">Subscriber On/Off Report</a><br />
                    <div class="formLabel">
                        Analyze list growth (with or without demographic data) within a specified date range.
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="AdvertiserReport.aspx">Click Report</a><br />
                    <div class="formLabel">
                        Group specific report shows all URLs clicked on during a specified timeframe.
                    </div>
                </li>
                <asp:panel id="pnlFlashReport" runat="server">
                <li style="padding-bottom: 10px">
                    <a style="font-size:12px" href="FlashReport.aspx">Flash Report</a>
                    <br />
                    <div class="formLabel">
                    Report shows promocode conversions. *Feature not turned on in all accounts.
                    </div>
                   
                </li>
                 </asp:panel>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="ListSizeOverTimeReport.aspx">List Size Over Time Report</a><br />
                    <div class="formLabel">
                        Shows subscribers activity on during a specified timeframe.
                    </div>
                </li>
            </ul>
        </td>
    </tr>
</table>
</asp:Content>

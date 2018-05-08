<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="BlastReports.aspx.cs" Inherits="ecn.communicator.main.Reports.BlastReports" %>

<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
        <tr>
            <td valign="top" style="padding-left: 25px" align="left">
                <ul style="margin-top: 1em;">
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/ABSummaryReport.aspx">A/B Summary Report</a><br />
                        <div class="formLabel">
                            A summary of all A/B campaigns and associated Champion campaigns for a specified date range.
                        </div>
                    </li>
                     <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/BlastComparisonReport.aspx">Blast Comparison Report</a><br />
                        <div class="formLabel">
                            Summary report representing demographic data within a specified email group and campaign.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/CampaignStatisticsReport.aspx">Campaign Statistics Report</a><br />
                        <div class="formLabel">Summary report, up to a one year period, of all blasts sent to a specific campaign. Includes delivery success with opens and clicks.</div>
                    </li>
                    <li style="padding-bottom: 10px;"><a style="font-size: 12px;" href="../blasts/report/ChampionAuditReport.aspx">Champion Audit Report</a><br />
                        <div class="formLabel">
                            Real time statistics showing how a Champion campaign continues to perform over time, while seeing what the A/B statistics were at the time the campaign was set up.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/blastdeliveryreport.aspx">Delivery Report</a><br />
                        <div class="formLabel">
                            A combined high level overview report of the statistics from all the Campaigns sent within the specific date range selected.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/EmailsDeliveredByPercentage.aspx">Emails Delivered By Percentage Report</a><br />
                        <div class="formLabel">
                            Shows what percentage of your audience is getting what number of emails, either by week or by month.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px;"><a style="font-size: 12px;" href="../blasts/report/EmailFatigueReport.aspx">Email Fatigue Report</a><br />
                        <div class="formLabel">
                            Defines at what point sending emails becomes detrimental to your email program by visually comparing the amount of emails sent versus opens or clicks.
                        </div>
                    </li>   
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/EmailPerformanceByDomainReport.aspx">Email Performance By Domain Report</a><br />
                        <div class="formLabel">
                            Provides performance statistics for major ISP domains from all blasts for a specified date range.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/EmailPreviewUsage.aspx">Email Preview Usage Report</a><br />
                        <div class="formLabel">
                            Displays the “preview usage” amount (per month) that the Email Preview option has been used within an account.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px;"><a style="font-size: 12px" href="../blasts/report/LinkReport.aspx">Link Report</a><br />
                        <div class="formLabel">
                            Pulls advertiser leads from multiple campaigns showing the number of opens/clicks within a specified date range.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px;"><a style="font-size: 12px;" href="../blasts/report/PerformanceByDayAndTimeReport.aspx">Performance By Day and Time</a><br />
                        <div class="formLabel">
                            Analyzes the opens and clicks percentages over days and times during a specified time period.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/TopEvangelistsReport.aspx">Top Evangelists</a><br />
                        <div class="formLabel">Shows top 100 subscribers that are sharing your campaign with others through social media and email forwarding.</div>
                    </li>
                    <li style="padding-bottom: 10px"><a style="font-size: 12px" href="../blasts/report/UndeliverableReport.aspx">Undeliverable Report</a><br />
                        <div class="formLabel">
                            Displays the undeliverable statistics of Hard Bounces, Soft Bounces, Mail Box Full, and Unsubscribes for Campaigns within a specific date range.
                        </div>
                    </li>
                    <li style="padding-bottom: 10px;"><a style="font-size: 12px;" href="../blasts/report/UnsubscribeReasonReport.aspx">Unsubscribe Reason Report</a><br />
                        <div class="formLabel">
                            Shows a breakdown of reasons supplied when unsubscribing through a blast.
                        </div>
                    </li>
                </ul>
            </td>
        </tr>
    </table>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Communicator.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ecn.communicator.main.blasts.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table id="layoutWrapper" cellspacing="1" cellpadding="1" width="100%" border='0'>
    <tr>
        <td valign="top" style="padding-left: 25px" align="left">
            <ul style="margin-top: 1em;">
                <li style="padding-bottom: 10px;"><a style="font-size:12px" href="LinkReport.aspx">Link
                    Report</a><br />
                    <div class="formLabel">
                        </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="BlastComparisonReport.aspx">Blast Comparison Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="blastdeliveryreport.aspx">Delivery Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="../EmailPreviewUsage.aspx">Email Preview Usage Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="MasterSuppressionSourceReport.aspx">Master Suppression Source Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="EmailsDeliveredByPercentage.aspx">Emails Delivered By Percentage Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="UndeliverableReport.aspx">Undeliverable Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="ABSummaryReport.aspx">A/B Summary Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="EmailPerformanceByDomainReport.aspx">A/B Summary Report</a><br />
                    <div class="formLabel">
                        
                    </div>
                </li>
                <li style="padding-bottom: 10px"><a style="font-size:12px" href="UnsubscribeReasonReport.aspx">Unsubscribe Reason Report</a><br />
                    <div class="formLabel"></div>
                </li>

                
            </ul>
        </td>
    </tr>
</table>
</asp:Content>

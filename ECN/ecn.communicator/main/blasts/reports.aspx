<%@ Page Language="c#" Inherits="ecn.communicator.blastsmanager.reports" CodeBehind="reports.aspx.cs"
    MasterPageFile="~/MasterPages/Communicator.Master" %>

<%@ Register TagPrefix="ecnCustom" Namespace="ecn.controls" Assembly="ecn.controls" %>
<%@ MasterType VirtualPath="~/MasterPages/Communicator.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji.js"></script>
<script type="text/javascript" src="/ecn.communicator/scripts/Twemoji/twemoji-picker.js"></script>
<link rel="stylesheet" href="/ecn.communicator/scripts/Twemoji/twemoji-picker.css" />

<script type="text/javascript" >
    function pageLoad(sender, args) {
        $('.subject').each(function () {
            var initialString = $(this).html();
            initialString = initialString.replace(/'/g, "\\'");
            initialString = initialString.replace(/\r?\n|\r/g, ' ');
            initialString = twemoji.parse(eval("'" + initialString + "'"), { size: "16x16" });

           

            $(this).html(initialString);
        });
    }
</script>
    <script language="javascript">
        function buyISPFeature() {
            alert('ISP Reporting is a feature of our enhanced deliverability services program, which can be purchased in the package or as a stand alone option. ISP Reporting provides a breakdown of opens, clicks and unsubscribes with specific email providers such as AOL, CS, Excite, GMAIL, HOTMAIL to name a few. This tool will enable you to identify valuable ISP demographics which can be used in your email marketing strategies.\n\nIf you are interested in this feature, or our full Deliverability Package, please contact our Customer Service Department at 1-866-844-6275.');
        }
        function buyConvTrackingFeature() {
            alert('Conversion Tracking is an advanced ECN feature that allows for increased marketing intelligence through extended link tracking—a perfect extension to general reporting of clicks and opens. Conversion tracking uses specific URLs in your campaign to discover not only which links were clicked, but which generated a completed order page or any other action you desire.\n\nIf you are interested in this feature, please contact our Customer Service Department at 1-866-844-6275.');
        }
        function buyROIFeature() {
            alert('ROI Reporting shows the effectiveness of your email marketing campaigns. Simply enter in your campaign costs (setup, design, inbound/outbound costs) and the system will calculate the cost per opened email or cost per conversion.\n\nFind out your Return On Investment today, contact our Customer Service Department at 1-866-844-6275.');
        }
        function openROISetup(layoutID) {
            var layout = layoutID
            window.open('../content/layoutCostEditor.aspx?LayoutID=' + layout + '', 'ROI_Cost', 'left=10,top=10,height=450,width=565,resizable=yes,scrollbar=yes,status=no');
        }
    </script>
    <style type="text/css">

        .linkbuttonNoUnderlineFail{
            color:red !important;
        }

        .lnkbuttonNoUnderline 
        {
            color:green !important;
        }
        .lnkbuttonNoUnderline:hover
        {
            text-decoration:none;
            cursor:pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border='0'>
        <tr>
            <td class="gradient">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="offWhite greySidesB">
                <table id="layoutWrapper" cellspacing="1" cellpadding="3" width="100%" border='0'>
                    <asp:Panel ID="pnlEmail" runat="server" Visible="true"> 
                    <tr>
                        <td height="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="tableHeader" align='right' width="15%" valign="top">
                            &nbsp;Email Subject:
                        </td>
                        <td width="35%" style="font-size: 12px" align="left">
                            <asp:Label ID="EmailSubject" CssClass="subject" runat="Server"></asp:Label>
                        </td>
                        <td class="tableHeader" align='right' width="15%"  valign="top">
                            &nbsp;Message:
                        </td>
                        <td width="35%" class="dataTwo" align="left" valign="top">
                            <span style="font-size: 12px;">
                                <asp:HyperLink ID="Campaign" runat="Server"></asp:HyperLink>
                                &nbsp;&nbsp; <sub>
                                    <asp:HyperLink ID="ConversionTrkingSetupLNK" runat="Server" ImageUrl="/ecn.images/images/icon-linkTracking.gif"></asp:HyperLink>
                                </sub></span>
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="tableHeader">
                            &nbsp;Email From:
                        </td>
                        <td style="font-size: 12px"  align="left">
                            <asp:Label ID="EmailFrom" runat="Server"></asp:Label>
                        </td>
                        <td class="tableHeader" align='right' width="15%">
                            &nbsp;Start Time:
                        </td>
                        <td width="35%"  style="font-size: 12px" align="left">
                            <asp:Label ID="SendTime" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align='right' class="tableHeader">
                            &nbsp;Mailing List(s):
                        </td>
                        <td  style="font-size: 12px; color: Blue; height: 100%;"  align="left">
                            <asp:HyperLink ID="GroupTo" CssClass="blastLinks" runat="Server"></asp:HyperLink>
                        </td>
                        <td class="tableHeader" align='right' width="15%">
                            &nbsp;Finish Time:
                        </td>
                        <td width="35%" style="font-size: 12px" align="left">
                            <asp:Label ID="FinishTime" runat="Server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align='right'  class="tableHeader">
                            &nbsp;Filter used:
                        </td>
                        <td valign="top" style="font-size: 12px;" align="left">
                         <asp:Label ID="lblCampaignFilters" runat="Server" Text="Please view blast reports to see the filters" Visible="false"></asp:Label>
                            <asp:GridView ID="gvFilters" runat="server" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvFilters_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlFilterName" CssClass="blastLinks" runat="server" />
                                            <asp:Label ID="lblSS" CssClass="subject" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            
                        </td>
                        <td class="tableHeader" align='right' width="15%">
                            &nbsp;Successful:
                        </td>
                        <td width="35%" style="font-size: 12px" align="left">
                            <asp:LinkButton ID="SuccessfulDownload" runat="server" OnClick="downloadDeliveredEmails"></asp:LinkButton>
                            <asp:Label ID="Successful" runat="Server"></asp:Label>
                            <asp:Label ID="SuccessfulPercentage" runat="Server"></asp:Label>
                            <!--[<A href="#" onclick="javascript:window.open('logs.aspx?BlastID=<%=getBlastID()%>', 'Logs', 'width=550,height=500,resizable=yes,scrollbars=yes,status=yes');" >view log</A> ]-->
                        </td>
                    </tr>
                    <tr>
                        <td align='right'  class="tableHeader">
                            &nbsp;Suppression List:
                        </td>
                        <td  style="font-size: 12px"  align="left">
                            <asp:Label ID="SuppressionList" runat="Server"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>                                     
                    <tr>
                        <td class="tableHeader" valign="top" align="center" colspan="4">
                            <table width="100%">
                                <tr>
                                    <td valign="top" width="25%" align="left">
                                        <table style="border-collapse: collapse; height: 100%;" border="1" width="100%" class="gridWizard">
                                            <tr class="gridheaderWizard">
                                                <td>
                                                    Advanced Reporting
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="18" valign="top" align="center">
                                                                <img src="/ecn.images/images/icon-linkTracking.gif" />
                                                            </td>
                                                            <td style="font-size: 11px;">
                                                                <asp:HyperLink ID="lnkConversionTracking" runat="Server" Enabled="False">Conversion Tracking Report</asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard">
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/isp-report.gif" />
                                                            </td>
                                                            <td style="font-size: 11px;">
                                                                <span style="font-size: 11px;">
                                                                    <asp:HyperLink ID="lnkISP" runat="Server">Reporting by ISP(s)</asp:HyperLink>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                             <asp:Panel ID="pnlNotUsed" runat="server" Visible="false">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/calculator2.gif" />
                                                            </td>
                                                            <td style="font-size: 11px;">
                                                                <span style="font-size: 11px;">
                                                                    <asp:HyperLink ID="ROITrkingSetupLNK" runat="Server" Enabled="False">Return on Investment [ROI] Calculator</asp:HyperLink>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard">
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/roi_report.gif" />
                                                            </td>
                                                            <td style="font-size: 11px;">
                                                                <asp:HyperLink ID="lnkROITracking" runat="Server" Enabled="False">Return on Investment [ROI] Report</asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            </asp:Panel>
                                        </table>
                                        <br/>
                                        <asp:Panel ID="pnlDCReport" runat="server" Visible="false">
                                            <table style="border-collapse: collapse; height: 100%;" border="1" width="100%" class="gridWizard">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td width="18">
                                                                    <img src="/ecn.images/images/email_reports.gif" />
                                                                </td>
                                                                <td style="font-size: 11px;">
                                                                    <asp:HyperLink ID="lnkDCTracking" runat="Server" Enabled="True">Dynamic Content Sends Report</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <br/>
                                    </td>
                                    <td width="75%" align='right'>
                                        <table width="99%" class="gridWizard" style="border-collapse: collapse; height: 100%"
                                            border="1" bordercolor="#A4A2A3" id="bigHitCell">
                                            <tr class="gridheaderWizard">
                                                <td width="28%" align="left">
                                                    Detailed Reporting
                                                </td>
                                                <td align="center" width="18%">
                                                    Unique
                                                </td>
                                                <td align="center" width="18%">
                                                    Total
                                                </td>
                                                <td align="center" width="18%">
                                                    Unique %
                                                </td>
                                                <td align="center" width="18%">
                                                    Total %
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/email_reports.gif" />
                                                            </td>
                                                            <td class="tableContent">
                                                                <b>Sends</b>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="Sends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SendsUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="Sends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SendsTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="Sends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    100%<!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="Sends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    100%<!--</a>-->
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/opens_report.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Opens
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="opens.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="OpensUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="opens.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="OpensTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="opens.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="OpensPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="opens.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="OpensTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/unopened.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Unopened
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="NoOpenTotal" runat="Server"></asp:Label>
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="NoOpenPercentage" runat="Server"></asp:Label>
                                                </td>
                                                <td align="center">
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/clicks.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Clicks by URL
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClicksUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClicksTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClicksPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClicksTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/clicks.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Click Through Ratio
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClickThrough" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                   
                                                </td>
                                                <td align="center">
                                                    <!--<a href="clicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ClickThroughPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/no_click.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;  text-align:left;">
                                                                No Clicks&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="NoClicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="NoClickTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                </td>
                                                <td align="center">
                                                    <!--<a href="NoClicks.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="NoClickPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/bounces_icon.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Bounces
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="bounces.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="BouncesUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="bounces.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="BouncesTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="bounces.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="BouncesPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="bounces.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="BouncesTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/resends.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Resends
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ResendsUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ResendsTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ResendsPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ResendsTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/forwards.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Forwards
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="forwards.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ForwardsUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="forwards.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ForwardsTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="forwards.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ForwardsPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="forwards.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="ForwardsTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/unsubscribe.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Unsubscribes
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SubscribesUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SubscribesTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SubscribesPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SubscribesTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" align="left" style="padding-left: 38px;">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=MASTSUP_UNSUB">-->
                                                    <span style="font-weight: normal; padding-left: 15px">Master Suppressed</span><!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=MASTSUP_UNSUB">-->
                                                    <asp:Label ID="MasterSuppressUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <!-- MasterSuppressTotal is being vertically positioned from style sheet  -->
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=MASTSUP_UNSUB">-->
                                                    <asp:Label ID="MasterSuppressTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=MASTSUP_UNSUB">-->
                                                    <asp:Label ID="MasterSuppressPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=MASTSUP_UNSUB">-->
                                                    <asp:Label ID="MasterSuppressTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left" style="padding-left: 38px;">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=ABUSERPT_UNSUB">-->
                                                    <span style="font-weight: normal; padding-left: 15px">Abuse Complaints&nbsp;</span><!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=ABUSERPT_UNSUB">-->
                                                    <asp:Label ID="AbuseUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <!-- AbuseTotal is being vertically positioned from style sheet  -->
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=ABUSERPT_UNSUB">-->
                                                    <asp:Label ID="AbuseTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=ABUSERPT_UNSUB">-->
                                                    <asp:Label ID="AbusePercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=ABUSERPT_UNSUB">-->
                                                    <asp:Label ID="AbuseTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr class="gridaltrowWizard" onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#ebebec';">
                                                <td class="reportCategory" style="padding-left: 38px;" align="left">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=FEEDBACK_UNSUB">-->
                                                    <span style="font-weight: normal; padding-left: 15px">ISP Feedback Loops&nbsp;</span><!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=FEEDBACK_UNSUB">-->
                                                    <asp:Label ID="AOLFeedbackUnsubscribeUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <!-- AOLFeedbackUnsubscribeTotal is being vertically positioned from style sheet  -->
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=FEEDBACK_UNSUB">-->
                                                    <asp:Label ID="AOLFeedbackUnsubscribeTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=FEEDBACK_UNSUB">-->
                                                    <asp:Label ID="AOLFeedbackUnsubscribePercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="subscribes.aspx?BlastID=<%=getBlastID()%>&amp;code=FEEDBACK_UNSUB">-->
                                                    <asp:Label ID="AOLFeedbackUnsubscribeTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr onmouseover="this.style.backgroundColor='#ccc';" onmouseout="this.style.backgroundColor='#f9f9f9';">
                                                <td class="reportCategory" align="left">
                                                    <table cellpadding="0" cellspacing="0" height="100%" width="100%" class="bigHitCell">
                                                        <tr>
                                                            <td width="18">
                                                                <img src="/ecn.images/images/ic-cancel.gif" />
                                                            </td>
                                                            <td class="tableContent" style="font-weight: bold;">
                                                                Suppressed<%--<asp:label id="SuppressedMain" 
                                                                    runat="Server" Font-Bold="True"></asp:label>--%></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SuppressedUnique" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SuppressedTotal" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SuppressedPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                                <td align="center">
                                                    <!--<a href="resends.aspx?BlastID=<%=getBlastID()%>">-->
                                                    <asp:Label ID="SuppressedTotalPercentage" runat="Server"></asp:Label>
                                                    <!--</a>-->
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="5">
                                                    <asp:Label runat="server" Text="Social Media" ></asp:Label>
                                                    <div align="center">                                                        
                                                        <div style="display: inline-block; width: 39%;vertical-align: top;">
                                                            <ecnCustom:ecnGridView ID="SocialGrid" runat="Server" AutoGenerateColumns="False"
                                                            Style="margin: 7px 0;" Width="100%" CssClass="grid" datakeyfield="BlastID" DataKeyNames="ID"
                                                            OnRowDataBound="SocialGrid_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Subscriber" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hlMediaReporting" runat="server" Text='<%# Eval("ReportImagePath").ToString() %>'
                                                                            NavigateUrl='<%# Eval("ReportPath").ToString() %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Share" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hlShare" runat="server" Text='<%# Eval("Share").ToString() %>'
                                                                            NavigateUrl='<%# Eval("ReportPath").ToString() %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Previews" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:HyperLink ID="hlClick" runat="server" Text='<%# Eval("Click").ToString() %>'
                                                                            NavigateUrl='<%# Eval("ReportPath").ToString() %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            </ecnCustom:ecnGridView>
                                                        </div>
                                                        <div style="display: inline-block; width: 59%;vertical-align: top;">  
                                                            <ecnCustom:ecnGridView ID="SocialSimpleGrid" runat="Server" AutoGenerateColumns="False"
                                                            Style="margin: 7px 0;" Width="100%" CssClass="grid" datakeyfield="BlastID" DataKeyNames="SocialMediaId" OnRowDataBound="SocialSimpleGrid_RowDataBound" OnRowCommand="SocialSimpleGrid_Command">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Simple" HeaderStyle-Width="18%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <img src='<%# Eval("Icon").ToString() %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Page" HeaderStyle-Width="28%" HeaderStyle-HorizontalAlign="center"
                                                                    ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%# Eval("Page") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                 <asp:TemplateField HeaderText="Status" HeaderStyle-Width="28%" HeaderStyle-HorizontalAlign="center" ItemStyle-Font-Underline="false" ItemStyle-HorizontalAlign="center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbStatus" CssClass="lnkbuttonNoUnderline" Text='<%# Eval("Status") %>' CommandName="ResendSocialSimpleBlast" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SocialMediaId") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            </ecnCustom:ecnGridView>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnlSMS" runat="server" Visible="false">
                      <tr>
                        <td width="10%"></td>
                        <td>
                             <table width="100%" class="gridWizard" style="border-collapse: collapse; height: 100%"
                                            border="1" bordercolor="#A4A2A3" id="Table1">
                                <tr>
                                    <td class="tableHeader" align='right' width="15%">
                                        &nbsp;Start Time:
                                    </td>
                                    <td width="35%"  style="font-size: 12px" align="left">
                                        <asp:Label ID="SendTime1" runat="Server"></asp:Label>
                                    </td>                                  
                                    <td align='right' class="tableHeader">
                                        Attempt Total:
                                    </td>
                                    <td style="font-size: 12px" align="left">
                                          <asp:Label ID="lblsmsAttempt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                    <tr class="gridaltrowWizard">
                                    <td class="tableHeader" align='right' width="15%"  valign="top">
                                &nbsp;Message:  
                                    </td>
                                    <td width="35%" class="dataTwo" align="left" valign="top">
                                        <span style="font-size: 12px;">
                                            <asp:HyperLink ID="Campaign1" runat="Server"></asp:HyperLink>
                                            &nbsp;&nbsp;</span>
                                            
                                    </td>
                                     <td align='right' class="tableHeader">
                                        Total Sends:
                                    </td>
                                      <td style="font-size: 12px" align="left">
                                          <asp:Label ID="lblsmsSent" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                  <td align='right' class="tableHeader">
                                        &nbsp;Group Name:
                                    </td>
                                    <td  style="font-size: 12px; color: Blue; height: 100%;"  align="left">
                                        <asp:HyperLink ID="GroupTo1" CssClass="blastLinks" runat="Server"></asp:HyperLink>
                                    </td>                                    
                                    <td align='right' class="tableHeader">
                                        Total Delivered:
                                    </td>
                                      <td style="font-size: 12px" align="left">
                                          <asp:Label ID="lblsmsDelivered" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                    <tr class="gridaltrowWizard">
                                    <td align='right' class="tableHeader">
                                        Total Welcome Msgs Sent:
                                    </td>
                                      <td style="font-size: 12px" align="left">
                                          <asp:Label ID="lblsmsWelcome" runat="server" ></asp:Label>
                                    </td>
                                    <td align='right' class="tableHeader">
                                        Opt Outs:
                                    </td>
                                    <td style="font-size: 12px" align="left">
                                          <asp:Label ID="lblsmsOptOut" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="10%"></td>
                    </tr>
                    </asp:Panel>                  
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

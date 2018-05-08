using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Text.RegularExpressions;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class GroupStatisticsReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> Get(int groupID, DateTime startdate, DateTime enddate)
        {
            return ECN_Framework_DataLayer.Activity.Report.GroupStatisticsReport.Get(groupID, startdate, enddate);
        }

        public static List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> GetReportDetails(List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> groupStatistics, bool showBrowserDetails)
        {
            List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> newgroupStatistics = new List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport>();

            if (groupStatistics != null)
            {
                foreach (ECN_Framework_Entities.Activity.Report.GroupStatisticsReport grpstat in groupStatistics)
                {
                    List<ECN_Framework_Entities.Activity.Report.PlatformDetail> listPlatformDetail = new List<ECN_Framework_Entities.Activity.Report.PlatformDetail>();
                    ECN_Framework_Entities.Activity.Report.GroupStatisticsReport gs = new ECN_Framework_Entities.Activity.Report.GroupStatisticsReport();
                    gs.BlastID = grpstat.BlastID;
                    gs.EmailSubject = ECN_Framework_Common.Functions.EmojiFunctions.GetSubjectUTF(grpstat.EmailSubject);
                    gs.SendTime = grpstat.SendTime;
                    gs.CampaignName = grpstat.CampaignName;
                    gs.TSend = grpstat.TSend;
                    gs.UBounce = grpstat.UBounce;
                    gs.Delivered = grpstat.TSend - grpstat.UBounce;
                    gs.SuccessPercentage = grpstat.TSend > 0 ? decimal.Round(Convert.ToDecimal((grpstat.TSend - grpstat.UBounce) * 100) / Convert.ToDecimal(grpstat.TSend), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.TOpen = grpstat.TOpen;
                    gs.DeliveredOpensPercentage = grpstat.TSend - grpstat.UBounce > 0 ? decimal.Round(Convert.ToDecimal(grpstat.TOpen * 100) / Convert.ToDecimal(grpstat.TSend - grpstat.UBounce), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.UOpen = grpstat.UOpen;
                    gs.UniqueOpensPercentage = grpstat.TSend - grpstat.UBounce > 0 ? decimal.Round(Convert.ToDecimal(grpstat.UOpen * 100) / Convert.ToDecimal(grpstat.TSend - grpstat.UBounce), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.TClick = grpstat.TClick;
                    gs.DeliveredClicksPercentage = grpstat.TSend - grpstat.UBounce > 0 ? decimal.Round(Convert.ToDecimal(grpstat.TClick * 100) / Convert.ToDecimal(grpstat.TSend - grpstat.UBounce), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.UClick = grpstat.UClick;
                    gs.UniqueClicksPercentage = grpstat.TSend - grpstat.UBounce > 0 ? decimal.Round(Convert.ToDecimal(grpstat.UClick * 100) / Convert.ToDecimal(grpstat.TSend - grpstat.UBounce), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.UUnsubscribe = grpstat.UUnsubscribe;
                    gs.OpenClicksPercentage = grpstat.TOpen > 0 ? decimal.Round(Convert.ToDecimal(grpstat.TClick * 100) / Convert.ToDecimal(grpstat.TOpen), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.UniqueOpenClicksPercentage = grpstat.UOpen > 0 ? decimal.Round(Convert.ToDecimal(grpstat.UClick * 100) / Convert.ToDecimal(grpstat.UOpen), 1, MidpointRounding.AwayFromZero) : 0;
                    gs.Suppressed = grpstat.Suppressed;
                    gs.ClickThrough = grpstat.ClickThrough;
                    gs.ClickThroughPercentage = gs.Delivered > 0 ? decimal.Round(Convert.ToDecimal(grpstat.ClickThrough * 100) / Convert.ToDecimal(gs.Delivered), 1, MidpointRounding.AwayFromZero) : 0;

                    if (showBrowserDetails)
                    {
                        listPlatformDetail = GetBrowserStats(gs.BlastID);

                        gs.BrowserStats = listPlatformDetail;
                    }
                    newgroupStatistics.Add(gs);
                }
            }

            return newgroupStatistics;
        }

        public static StringBuilder AddDelimiter(List<ECN_Framework_Entities.Activity.Report.GroupStatisticsReport> list)
        {
            StringBuilder sb = new StringBuilder();
            string delimiter="\",\"";
            sb.AppendLine("Date,BlastID,EmailSubject,CampaignName,TSend,UBounce,Delivered,TOpen,Uopen,TClick By URL,UClick By URL,Click Through Ratio, Click Through Ratio %,UUnsubscribe,SuccessPercentage,DeliveredOpensPercentage,UniqueOpensPercentage,Total Clicks by URL %,Unique Clicks By URL %,OpenClicksPercentage,UniqueOpenClicksPercentage");

            //loop through first list
            foreach (ECN_Framework_Entities.Activity.Report.GroupStatisticsReport gsr in list)
            {
                Regex rgx = new Regex("\"");
                StringBuilder sbGroup = new StringBuilder();
                sbGroup.Append("\"");
                sbGroup.Append(rgx.Replace(gsr.Date.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.BlastID.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.EmailSubject.ToString(), "'") + delimiter);
                sbGroup.Append(gsr.CampaignName == null ? string.Empty : rgx.Replace(gsr.CampaignName.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.TSend.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UBounce.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.Delivered.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.TOpen.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UOpen.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.TClick.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UClick.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.ClickThrough.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.ClickThroughPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UUnsubscribe.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.SuccessPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.DeliveredOpensPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UniqueOpensPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.DeliveredClicksPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UniqueClicksPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.OpenClicksPercentage.ToString(), "'") + delimiter);
                sbGroup.Append(rgx.Replace(gsr.UniqueOpenClicksPercentage.ToString(), "'") + "\"");
                sb.AppendLine(sbGroup.ToString());

                StringBuilder sbSub = new StringBuilder();
                List<ECN_Framework_Entities.Activity.Report.PlatformDetail> subItem = gsr.BrowserStats;

                if (subItem != null)
                {
                    //Build subreport headers
                    sbSub.AppendLine("\"" + delimiter + "Platform" + delimiter + "Client" + delimiter + "Opens" + delimiter + "Usage\"");

                    foreach (ECN_Framework_Entities.Activity.Report.PlatformDetail pd in subItem)
                    {

                        sbSub.Append("\"" + delimiter + pd.PlatformName.ToString() + delimiter);
                        sbSub.Append(pd.EmailClientName.ToString() + delimiter);
                        sbSub.Append(pd.Column1.ToString() + delimiter);
                        sbSub.Append(pd.Usage.ToString() + "\"");
                        sbSub.AppendLine();
                    }
                }
                sb.AppendLine(sbSub.ToString());
            }

            return sb;
        }

        private static List<ECN_Framework_Entities.Activity.Report.PlatformDetail> GetBrowserStats(int BlastID)
        {
            List<ECN_Framework_Entities.Activity.BlastActivityOpens> openslist = ECN_Framework_BusinessLayer.Activity.BlastActivityOpens.GetByBlastID(BlastID).ToList();
            List<ECN_Framework_Entities.Activity.Platforms> plist = ECN_Framework_BusinessLayer.Activity.Platforms.Get();
            List<ECN_Framework_Entities.Activity.EmailClients> eclist = ECN_Framework_BusinessLayer.Activity.EmailClients.Get();
            List<int> platforms = openslist.Select(x => x.PlatformID).Distinct().ToList();
            List<ECN_Framework_Entities.Activity.Report.PlatformDetail> listPlatform = new List<ECN_Framework_Entities.Activity.Report.PlatformDetail>();
            foreach (int i in platforms)
            {
                if (i != 5)
                {

                    List<ECN_Framework_Entities.Activity.Report.PlatformDetail> listOrder = new List<ECN_Framework_Entities.Activity.Report.PlatformDetail>();
                    List<ECN_Framework_Entities.Activity.BlastActivityOpens> tempList = openslist.Where(x => x.PlatformID == i).ToList();
                    List<int> listEmailClients = tempList.Select(x => x.EmailClientID).Distinct().ToList();
                    foreach (int j in listEmailClients)
                    {
                        if (j != 15)
                        {
                            ECN_Framework_Entities.Activity.Report.PlatformDetail pd = new ECN_Framework_Entities.Activity.Report.PlatformDetail();
                            pd.PlatformName = plist.First(x => x.PlatformID == i).PlatformName;
                            pd.EmailClientName = eclist.First(x => x.EmailClientID == j).EmailClientName;
                            pd.Column1 = tempList.Where(x => x.EmailClientID == j).Count();
                            pd.Usage = Math.Round(((float)pd.Column1 * 100 / openslist.Count), 2).ToString() + "%";

                            listOrder.Add(pd);

                        }
                    }

                    listPlatform.AddRange(listOrder.OrderByDescending(x => x.Column1).ToList());
                }
            }

            return listPlatform;
        }
    }
}

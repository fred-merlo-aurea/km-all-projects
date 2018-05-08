using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class EmailPerformanceByDomainReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> Get(int customerID, DateTime  startdate, DateTime enddate, bool DrillDownOther)
        {
            return ECN_Framework_DataLayer.Activity.Report.EmailPerformanceByDomainReport.Get(customerID, startdate, enddate, DrillDownOther);
        }

        public static List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> GetReportdetails(List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> emailperformance)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> newEmailPerformanceByDomain = new List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain>();

            if (emailperformance != null)
            {
                foreach (ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain epd in emailperformance)
                {
                    ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain e = new ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain();
                    e.Domain = epd.Domain;
                    e.SendTotal = epd.SendTotal;
                    e.SendTotalPercentage = epd.SendTotal > 0 ? decimal.Round(Convert.ToDecimal((epd.SendTotal) * 100) / emailperformance.Sum(p => p.SendTotal), 2, MidpointRounding.AwayFromZero) : 0;
                    e.Delivered = epd.Delivered;
                    e.Opens = epd.Opens;
                    e.OpensPercentage = epd.Delivered > 0 ? decimal.Round(Convert.ToDecimal((epd.Opens) * 100) / Convert.ToDecimal(epd.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    e.Clicks = epd.Clicks;
                    e.ClicksPercentage = epd.Delivered > 0 ? decimal.Round(Convert.ToDecimal((epd.Clicks) * 100) / Convert.ToDecimal(epd.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    e.Bounce = epd.Bounce;
                    e.BouncePercentage = epd.SendTotal > 0 ? decimal.Round(Convert.ToDecimal((epd.Bounce) * 100) / Convert.ToDecimal(epd.SendTotal), 2, MidpointRounding.AwayFromZero) : 0;
                    e.Unsubscribe = epd.Unsubscribe;
                    e.UnsubscribePercentage = epd.Delivered > 0 ? decimal.Round(Convert.ToDecimal((epd.Unsubscribe) * 100) / Convert.ToDecimal(epd.Delivered), 2, MidpointRounding.AwayFromZero) : 0;

                    newEmailPerformanceByDomain.Add(e);
                }
            }

            return newEmailPerformanceByDomain;
        }

        public static string AddDelimiter(List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> list)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Domain Name,Emails Sent,% of Total Sent,Delivered,Bounced,Bounce %,Open,Open %,Clicks,Click %,Unsubscribed,Unsubscribed %");

            //loop through first list
            foreach (ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain e in list)
            {

                StringBuilder sbEmailPerformanceByDomain = new StringBuilder();
                sbEmailPerformanceByDomain.Append(e.Domain.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.SendTotal.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.SendTotalPercentage.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.Delivered.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.Bounce.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.BouncePercentage.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.Opens.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.OpensPercentage.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.Clicks.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.ClicksPercentage.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.Unsubscribe.ToString() + ",");
                sbEmailPerformanceByDomain.Append(e.UnsubscribePercentage.ToString() + ",");
                sb.AppendLine(sbEmailPerformanceByDomain.ToString());

            }

            return sb.ToString();
        }
    }
}

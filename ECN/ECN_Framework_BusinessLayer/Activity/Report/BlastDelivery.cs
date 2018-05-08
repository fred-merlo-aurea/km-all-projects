using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ECN_Framework_Common.Objects;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Activity.Report
{
    [Serializable]
    public class BlastDelivery
    {
        public static List<ECN_Framework_Entities.Activity.Report.BlastDelivery> Get(string customerIDs, DateTime startdate, DateTime enddate, bool unique)
        {
            return ECN_Framework_DataLayer.Activity.Report.BlastDelivery.Get(customerIDs, startdate, enddate, unique);
        }

        public static DataTable Get(string customerIDs, DateTime startdate, DateTime enddate)
        {
            DataTable dt = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dt = ECN_Framework_DataLayer.Activity.Report.BlastDelivery.Get(customerIDs, startdate, enddate);
                scope.Complete();
            }
            return dt;
        }

        public static List<ECN_Framework_Entities.Activity.Report.BlastDelivery> GetReportdetails(List<ECN_Framework_Entities.Activity.Report.BlastDelivery> blastdelivery)
        {
            List<ECN_Framework_Entities.Activity.Report.BlastDelivery> newblastdelivery = new List<ECN_Framework_Entities.Activity.Report.BlastDelivery>();

            if (blastdelivery != null)
            {
                foreach (ECN_Framework_Entities.Activity.Report.BlastDelivery bdelivery in blastdelivery)
                {
                    ECN_Framework_Entities.Activity.Report.BlastDelivery b = new ECN_Framework_Entities.Activity.Report.BlastDelivery();
                    b.SendTime = bdelivery.SendTime;
                    b.CampaignItemName = bdelivery.CampaignItemName;
                    b.CustomerName = bdelivery.CustomerName;
                    b.CampaignName = bdelivery.CampaignName;
                    b.GroupName = bdelivery.GroupName;
                    b.FromEmail = bdelivery.FromEmail;
                    b.BlastID = bdelivery.BlastID;
                    b.BlastCategory = bdelivery.CampaignName;
                    b.FilterName = bdelivery.FilterName;
                    b.EmailSubject = bdelivery.EmailSubject;
                    b.SendTotal = bdelivery.SendTotal;
                    b.Delivered = bdelivery.Delivered;
                    b.SoftBounceTotal = bdelivery.SoftBounceTotal;
                    b.SoftSendPercentage = bdelivery.SendTotal > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.SoftBounceTotal * 100) / Convert.ToDecimal(bdelivery.SendTotal), 2, MidpointRounding.AwayFromZero) : 0;
                    b.HardBounceTotal = bdelivery.HardBounceTotal;
                    b.HardSendPercentage = bdelivery.SendTotal > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.HardBounceTotal * 100) / Convert.ToDecimal(bdelivery.SendTotal), 2, MidpointRounding.AwayFromZero) : 0;
                    b.BounceTotal = bdelivery.BounceTotal;
                    b.BounceSendPercentage = bdelivery.SendTotal > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.BounceTotal * 100) / Convert.ToDecimal(bdelivery.SendTotal), 2, MidpointRounding.AwayFromZero) : 0;
                    b.TotalOpens = bdelivery.TotalOpens;
                    b.OpenDeliPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.TotalOpens * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.TotalClicks = bdelivery.TotalClicks;
                    b.ClickDeliPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.TotalClicks * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.ClickOpenPercentage = bdelivery.TotalOpens > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.TotalClicks * 100) / Convert.ToDecimal(bdelivery.TotalOpens), 2, MidpointRounding.AwayFromZero) : 0;
                    b.UnsubscribeTotal = bdelivery.UnsubscribeTotal;
                    b.UnSubDeliPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.UnsubscribeTotal * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.SuppressedTotal = bdelivery.SuppressedTotal;
                    b.UniqueOpens = bdelivery.UniqueOpens;
                    b.UOpenDeliPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.UniqueOpens * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.UniqueClicks = bdelivery.UniqueClicks;
                    b.UClickDeliPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.UniqueClicks * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.UClickOpenPercentage = bdelivery.UniqueOpens > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.UniqueClicks * 100) / Convert.ToDecimal(bdelivery.UniqueOpens), 2, MidpointRounding.AwayFromZero) : 0;
                    b.MobileOpens = bdelivery.MobileOpens;
                    b.UnMobileOpenPercentage = bdelivery.MobileOpens > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.MobileOpens * 100) / Convert.ToDecimal(bdelivery.UniqueOpens), 2, MidpointRounding.AwayFromZero) : 0;
                    b.ClickThrough = bdelivery.ClickThrough;
                    b.ClickThroughPercentage = bdelivery.Delivered > 0 ? decimal.Round(Convert.ToDecimal(bdelivery.ClickThrough * 100) / Convert.ToDecimal(bdelivery.Delivered), 2, MidpointRounding.AwayFromZero) : 0;
                    b.Field1 = bdelivery.Field1 == null ? string.Empty : bdelivery.Field1;
                    b.Field2 = bdelivery.Field2 == null ? string.Empty : bdelivery.Field2;
                    b.Field3 = bdelivery.Field3 == null ? string.Empty : bdelivery.Field3;
                    b.Field4 = bdelivery.Field4 == null ? string.Empty : bdelivery.Field4;
                    b.Field5 = bdelivery.Field5 == null ? string.Empty : bdelivery.Field5;

                    b.Spam = bdelivery.Spam;
                    if (bdelivery.Delivered > 0)
                    {
                        b.SpamPercent = bdelivery.SpamPercent;
                    }

                    newblastdelivery.Add(b);
                }
            }

            return newblastdelivery;
        }
    }
}

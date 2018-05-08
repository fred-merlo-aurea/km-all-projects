using ActivityObjects = ECN_Framework_Common.Objects.Activity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class UndeliverableReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetAll(DateTime startDate, DateTime endDate, int customerId)
        {
            return GetUndeliverable(startDate, endDate, customerId, "v_UndeliverableReport_GetAll");
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetHardBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return GetUndeliverable(startDate, endDate, customerId, "v_UndeliverableReport_GetBouncesByType", (int)ActivityObjects.Enums.BounceCode.hardbounce);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetSoftBounces(DateTime startDate, DateTime endDate, int customerId)
        {
            return GetUndeliverable(startDate, endDate, customerId, "v_UndeliverableReport_GetBouncesByType", (int)ActivityObjects.Enums.BounceCode.softbounce);
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetUnsubscribes(DateTime startDate, DateTime endDate, int customerId)
        {
            return GetUndeliverable(startDate, endDate, customerId, "v_UndeliverableReport_GetUnsubscribes");
        }

        public static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetMailBoxFull(DateTime startDate, DateTime endDate, int customerId)
        {
            return GetUndeliverable(startDate, endDate, customerId, "v_UndeliverableReport_GetMailBoxFull");
        }

        private static List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable> GetUndeliverable(DateTime startDate, DateTime endDate, int customerId, string cmdText, int bounceCodeId = 0)
        {
            var retList = new List<ECN_Framework_Entities.Activity.Report.Undeliverable.IUndeliverable>();
            
            using (SqlCommand cmd = new SqlCommand(cmdText))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                if (bounceCodeId > 0)
                {
                    cmd.Parameters.AddWithValue("@BounceCodeID", bounceCodeId);
                }

                using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.Undeliverable.Undeliverable>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            ECN_Framework_Entities.Activity.Report.Undeliverable.Undeliverable x = builder.Build(rdr);
                            retList.Add(x);
                        }
                    }
                }
            }

            return retList;
        }
    }
}

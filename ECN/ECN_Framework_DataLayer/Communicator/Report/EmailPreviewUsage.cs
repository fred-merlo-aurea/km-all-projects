using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator.Report
{
    public class EmailPreviewUsage
    {
        public static List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage> GetUsageDetailsAutomated(string customerIDs, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailPreview_GetUsage_Automated";
            cmd.Parameters.AddWithValue("@CustomerID", customerIDs);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage> retList = new List<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage retItem = new ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Report.EmailPreviewUsage>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}

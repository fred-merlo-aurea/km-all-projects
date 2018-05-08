using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
        [Serializable]
    public class UnsubscribeReason
    {
            public static List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> Get(string SearchField, string SearchCriteria, DateTime FromDate, DateTime ToDate, int CustomerID)
            {
                List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason> retList = new List<ECN_Framework_Entities.Activity.Report.UnsubscribeReason>();
                string sqlQuery = "v_UnsubscribeReason_Summary";
                SqlCommand cmd = new SqlCommand(sqlQuery);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@SearchField", SearchField);
                cmd.Parameters.AddWithValue("@SearchCriteria", SearchCriteria);

                using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
                {
                    if (rdr != null)
                    {
                        var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.UnsubscribeReason>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            ECN_Framework_Entities.Activity.Report.UnsubscribeReason x = builder.Build(rdr);
                            retList.Add(x);
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

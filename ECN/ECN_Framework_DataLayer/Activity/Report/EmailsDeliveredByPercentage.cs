using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class EmailsDeliveredByPercentage
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage> Get(int customerID, DateTime fromdate, DateTime todate)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage> retList = new List<ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage>();
            string sqlQuery = "spEmailsDeliveredByPercentage";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.EmailsDeliveredByPercentage x = builder.Build(rdr);
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

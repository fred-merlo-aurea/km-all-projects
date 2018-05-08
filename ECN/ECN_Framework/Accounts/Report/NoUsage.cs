using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class NoUsage
    {
        public NoUsage() { }
        #region Properties
        public int BaseChannelID { get; set; }
        public string BaseChannelName { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Usage { get; set; }
        #endregion
        #region Data
        public static List<NoUsage> Get(int channelID, string customerID, string fromDate, string toDate)
        {
            List<NoUsage> retList = new List<NoUsage>();
            string sqlQuery = "sp_NoUsageReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@channelID", channelID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@fromdt", fromDate);
            cmd.Parameters.AddWithValue("@todt", toDate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<NoUsage>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        NoUsage x = builder.Build(rdr);
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
        #endregion
    }
}

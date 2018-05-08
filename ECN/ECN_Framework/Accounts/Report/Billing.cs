using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class Billing
    {
        public Billing() { }
        #region Properties
        public int BaseChannelID { get; set; }
        public string BaseChannelName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Licenses { get; set; }
        public int Sends { get; set; }
        public int SurveyCount { get; set; }
        public int SurveyResponse { get; set; }
        public int DEcount { get; set; }
        public int DEpages { get; set; }
        #endregion
        #region Data
        public static List<Billing> Get(int channelID, string customerID, int month, int year)
        {
            List<Billing> retList = new List<Billing>();
            string sqlQuery = "sp_BillingReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@channelID", channelID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<Billing>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        Billing x = builder.Build(rdr);
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

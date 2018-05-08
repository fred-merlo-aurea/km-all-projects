using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class EmailDirectReport
    {
        public int BaseChannelID { get; set; }
        public string BaseChannelName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public string Process { get; set; }
        public int Count { get; set; }


        public static List<EmailDirectReport> Get(int BaseChannelID, string CustomerIDs, DateTime StartDate, DateTime EndDate)
        {
            List<EmailDirectReport> retList = new List<EmailDirectReport>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_EmailDirectReport";

            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@CustomerIDs", CustomerIDs);
            cmd.Parameters.AddWithValue("@StartDate", StartDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@EndDate", EndDate.ToShortDateString());
            
            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<EmailDirectReport>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        EmailDirectReport x = builder.Build(rdr);
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

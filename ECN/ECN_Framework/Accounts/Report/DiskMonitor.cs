using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class DiskMonitor
    {
        public DiskMonitor() { }
        #region Properties
        public int ChannelID {get;set;} 
        public int CustomerID {get;set;}
        public string BaseChannelName {get;set;}
        public string CustomerName {get;set;}
        public int AllowedStorage {get;set;}
        public decimal SizeInBytes { get; set; }
        #endregion
        #region Data
        public static List<DiskMonitor> Get(int channelID, int month, bool showOverLimit)
        {
            List<DiskMonitor> retList = new List<DiskMonitor>();
            string sqlQuery = "rpt_Diskmonitor";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ChannelID", channelID);
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@ShowOverLimit", showOverLimit == true ? "Y" : "N");

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<DiskMonitor>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        DiskMonitor x = builder.Build(rdr);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class ChannelLook
    {
        public ChannelLook() { }
        #region Properties
        public int BaseChannelID { get; set; }
        public string BaseChannelName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int JanCount { get; set; }
        public int FebCount { get; set; }
        public int MarCount { get; set; }
        public int AprCount { get; set; }
        public int MayCount { get; set; }
        public int JunCount { get; set; }
        public int JulCount { get; set; }
        public int AugCount { get; set; }
        public int SepCount { get; set; }
        public int OctCount { get; set; }
        public int NovCount { get; set; }
        public int DecCount { get; set; }
        #endregion

        #region Data
        public static List<ChannelLook> Get(int channelID, string customerID, string fromdt, string todt, bool testblast)
        {
            List<ChannelLook> retList = new List<ChannelLook>();
            string sqlQuery = "sp_ChannelLook";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@channelID", channelID);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@fromdt", fromdt);
            cmd.Parameters.AddWithValue("@todt", todt);
            cmd.Parameters.AddWithValue("@testblast", testblast == true ? "Y" : "N");

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ChannelLook>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ChannelLook x = builder.Build(rdr);
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

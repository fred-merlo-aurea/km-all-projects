using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class ChannelLook_Details
    {
        public ChannelLook_Details() { }
        #region Properties
        public int BlastID { get; set; }
        public string EmailSubject { get; set; }
        public string CampaignName { get; set; }
        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }
        public DateTime SendTime { get; set; }
        public int SuccessTotal { get; set; }
        public string Category { get; set; }
        public string Field1 { get; set; }
        public string GroupName { get; set; }
        public string GroupFolderName { get; set; }
        #endregion

        #region Data
        public static List<ChannelLook_Details> Get(string customerID, string fromdt, string todt, bool testblast)
        {
            List<ChannelLook_Details> retList = new List<ChannelLook_Details>();
            string sqlQuery = "sp_ChannelLook_Details";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@fromdt", fromdt);
            cmd.Parameters.AddWithValue("@todt", todt);
            cmd.Parameters.AddWithValue("@testblast", testblast == true ? "Y" : "N");

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ChannelLook_Details>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ChannelLook_Details x = builder.Build(rdr);
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

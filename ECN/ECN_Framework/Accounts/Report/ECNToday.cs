using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public class ECNToday
    {
        public ECNToday() { }
        #region Properties
        public int BaseChannelID { get; set; }
        public string BaseChannelName { get; set; }
        public int CustomerCount { get; set; }
        public int UserCount { get; set; }
        public int MTD { get; set; }
        public int YTD { get; set; }
        #endregion

        #region Data
        public static List<ECNToday> Get(int month, int year, bool testblast)
        {
            List<ECNToday> retList = new List<ECNToday>();
            string sqlQuery = "sp_ECNToday";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@month", month);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@testblast", testblast == true ? "Y" : "N");

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECNToday>.CreateBuilder(rdr);

                    while (rdr.Read())
                    {
                        ECNToday x = builder.Build(rdr);
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

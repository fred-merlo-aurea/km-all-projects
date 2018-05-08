using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class SubscriberMarketingMap
    {
        public static List<Object.SubscriberMarketingMap> Select(int subscriberID,KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberMarketingMap_Select_SubscriberID";
            cmd.Parameters.AddWithValue("@SubscriberID", subscriberID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        private static List<Object.SubscriberMarketingMap> GetList(SqlCommand cmd)
        {
            List<Object.SubscriberMarketingMap> retList = new List<Object.SubscriberMarketingMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.UAD_Master.ToString()))
                {
                    if (rdr != null)
                    {
                        Object.SubscriberMarketingMap retItem = new Object.SubscriberMarketingMap();
                        DynamicBuilder<Object.SubscriberMarketingMap> builder = DynamicBuilder<Object.SubscriberMarketingMap>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

    }
}

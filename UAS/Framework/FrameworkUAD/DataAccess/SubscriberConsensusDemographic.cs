using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberConsensusDemographic
    {
        public static List<Object.SubscriberConsensusDemographic> Select(int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberConsensusDemographic_Select_SubscriptionID";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static Object.SubscriberConsensusDemographic Get(SqlCommand cmd)
        {
            Object.SubscriberConsensusDemographic retItem = null;

            try
            {
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    retItem = new Object.SubscriberConsensusDemographic();
                    DynamicBuilder<Object.SubscriberConsensusDemographic> builder = DynamicBuilder<Object.SubscriberConsensusDemographic>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
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
            return retItem;
        }
        public static List<Object.SubscriberConsensusDemographic> GetList(SqlCommand cmd)
        {
            List<Object.SubscriberConsensusDemographic> retList = new List<Object.SubscriberConsensusDemographic>();

            try
            {
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
            {
                if (rdr != null)
                {
                    Object.SubscriberConsensusDemographic retItem = new Object.SubscriberConsensusDemographic();
                    DynamicBuilder<Object.SubscriberConsensusDemographic> builder = DynamicBuilder<Object.SubscriberConsensusDemographic>.CreateBuilder(rdr);
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

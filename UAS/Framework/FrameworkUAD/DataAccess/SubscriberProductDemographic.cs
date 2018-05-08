using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberProductDemographic
    {
        public static List<Object.SubscriberProductDemographic> Select(int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_SubscriberProductDemographic_Select_SubscriptionID_PubCode";
            cmd.Parameters.AddWithValue("@SubscriptionID", subscriptionID);
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        public static Object.SubscriberProductDemographic Get(SqlCommand cmd)
        {
            Object.SubscriberProductDemographic retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.SubscriberProductDemographic();
                        DynamicBuilder<Object.SubscriberProductDemographic> builder = DynamicBuilder<Object.SubscriberProductDemographic>.CreateBuilder(rdr);
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
        public static List<Object.SubscriberProductDemographic> GetList(SqlCommand cmd)
        {
            List<Object.SubscriberProductDemographic> retList = new List<Object.SubscriberProductDemographic>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.SubscriberProductDemographic retItem = new Object.SubscriberProductDemographic();
                        DynamicBuilder<Object.SubscriberProductDemographic> builder = DynamicBuilder<Object.SubscriberProductDemographic>.CreateBuilder(rdr);
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

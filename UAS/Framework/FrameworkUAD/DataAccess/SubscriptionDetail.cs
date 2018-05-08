using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriptionDetail
    {
        public static bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionDetails_Delete_MasterID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.SubscriptionDetail Get(SqlCommand cmd)
        {
            Entity.SubscriptionDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriptionDetail();
                        DynamicBuilder<Entity.SubscriptionDetail> builder = DynamicBuilder<Entity.SubscriptionDetail>.CreateBuilder(rdr);
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
        public static List<Entity.SubscriptionDetail> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriptionDetail> retList = new List<Entity.SubscriptionDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriptionDetail retItem = new Entity.SubscriptionDetail();
                        DynamicBuilder<Entity.SubscriptionDetail> builder = DynamicBuilder<Entity.SubscriptionDetail>.CreateBuilder(rdr);
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

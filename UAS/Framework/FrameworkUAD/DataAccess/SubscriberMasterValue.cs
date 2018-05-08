using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberMasterValue
    {
        public static bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberMasterValues_Delete_MasterID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.SubscriberMasterValue Get(SqlCommand cmd)
        {
            Entity.SubscriberMasterValue retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberMasterValue();
                        DynamicBuilder<Entity.SubscriberMasterValue> builder = DynamicBuilder<Entity.SubscriberMasterValue>.CreateBuilder(rdr);
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
        public static List<Entity.SubscriberMasterValue> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberMasterValue> retList = new List<Entity.SubscriberMasterValue>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberMasterValue retItem = new Entity.SubscriberMasterValue();
                        DynamicBuilder<Entity.SubscriberMasterValue> builder = DynamicBuilder<Entity.SubscriberMasterValue>.CreateBuilder(rdr);
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

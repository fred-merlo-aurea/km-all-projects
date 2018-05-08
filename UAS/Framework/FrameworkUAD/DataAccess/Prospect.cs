using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class Prospect
    {
        public static List<Entity.Prospect> Select(int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Prospect_Select_SubscriberID";
            cmd.Parameters.AddWithValue("@SubscriberID", subscriberID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        private static Entity.Prospect Get(SqlCommand cmd)
        {
            Entity.Prospect retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Prospect();
                        DynamicBuilder<Entity.Prospect> builder = DynamicBuilder<Entity.Prospect>.CreateBuilder(rdr);
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
        private static List<Entity.Prospect> GetList(SqlCommand cmd)
        {
            List<Entity.Prospect> retList = new List<Entity.Prospect>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Prospect retItem = new Entity.Prospect();
                        DynamicBuilder<Entity.Prospect> builder = DynamicBuilder<Entity.Prospect>.CreateBuilder(rdr);
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

        public static bool Save(Entity.Prospect x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Prospect_Save";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", x.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@SubscriberID", x.SubscriberID));
            cmd.Parameters.Add(new SqlParameter("@IsProspect", x.IsProspect));
            cmd.Parameters.Add(new SqlParameter("@IsVerifiedProspect", x.IsVerifiedProspect));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            int rowCount = Convert.ToInt32(KM.Common.DataFunctions.ExecuteNonQuery(cmd));
            if (rowCount > 0)
                return true;
            else
                return false;
        }
    }
}

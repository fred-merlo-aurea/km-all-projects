using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ClientGroupServiceMap
    {
        public static List<Entity.ClientGroupServiceMap> Select()
        {
            List<Entity.ClientGroupServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupServiceMap> Select(int clientGroupID)
        {
            List<Entity.ClientGroupServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceMap_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ClientGroupServiceMap Get(SqlCommand cmd)
        {
            Entity.ClientGroupServiceMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroupServiceMap();
                        var builder = DynamicBuilder<Entity.ClientGroupServiceMap>.CreateBuilder(rdr);
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
        private static List<Entity.ClientGroupServiceMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroupServiceMap> retList = new List<Entity.ClientGroupServiceMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroupServiceMap retItem = new Entity.ClientGroupServiceMap();
                        var builder = DynamicBuilder<Entity.ClientGroupServiceMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroupServiceMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupServiceMapID", x.ClientGroupServiceMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@Rate", x.Rate));
            cmd.Parameters.Add(new SqlParameter("@RateDurationInMonths", x.RateDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@RateStartDate", (object)x.RateStartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RateExpireDate", (object)x.RateExpireDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString());
        }
    }
}

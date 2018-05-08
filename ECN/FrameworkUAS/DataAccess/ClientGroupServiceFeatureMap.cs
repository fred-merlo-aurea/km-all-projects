using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ClientGroupServiceFeatureMap
    {
        public static List<Entity.ClientGroupServiceFeatureMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupServiceFeatureMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceFeatureMap_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupServiceFeatureMap> SelectForServiceFeature(int clientGroupID, int serviceFeatureID)
        {
            List<Entity.ClientGroupServiceFeatureMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceFeatureMap_Select_ClientGroupID_ServiceFeatureID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@ServiceFeatureID", serviceFeatureID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ClientGroupServiceFeatureMap> SelectForService(int clientGroupID, int serviceID)
        {
            List<Entity.ClientGroupServiceFeatureMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceFeatureMap_Select_ClientGroupID_ServiceID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ClientGroupServiceFeatureMap Get(SqlCommand cmd)
        {
            Entity.ClientGroupServiceFeatureMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ClientGroupServiceFeatureMap();
                        var builder = DynamicBuilder<Entity.ClientGroupServiceFeatureMap>.CreateBuilder(rdr);
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
        private static List<Entity.ClientGroupServiceFeatureMap> GetList(SqlCommand cmd)
        {
            List<Entity.ClientGroupServiceFeatureMap> retList = new List<Entity.ClientGroupServiceFeatureMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ClientGroupServiceFeatureMap retItem = new Entity.ClientGroupServiceFeatureMap();
                        var builder = DynamicBuilder<Entity.ClientGroupServiceFeatureMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ClientGroupServiceFeatureMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ClientGroupServiceFeatureMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ClientGroupServiceFeatureMapID", x.ClientGroupServiceFeatureMapID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", x.ServiceFeatureID));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@Rate", x.Rate));
            cmd.Parameters.Add(new SqlParameter("@RateDurationInMonths", x.RateDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@RateStartDate", (object)x.RateStartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RateExpireDate", (object)x.RateExpireDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}

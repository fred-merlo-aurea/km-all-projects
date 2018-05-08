using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ApplicationServiceMap
    {
        public static List<Entity.ApplicationServiceMap> Select()
        {
            List<Entity.ApplicationServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationServiceMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ApplicationServiceMap> SelectForApplication(int applicationID)
        {
            List<Entity.ApplicationServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationServiceMap_Select_ApplicationID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ApplicationServiceMap> SelectForService(int serviceID)
        {
            List<Entity.ApplicationServiceMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationServiceMap_Select_ServiceID";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.ApplicationServiceMap Get(SqlCommand cmd)
        {
            Entity.ApplicationServiceMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ApplicationServiceMap();
                        var builder = DynamicBuilder<Entity.ApplicationServiceMap>.CreateBuilder(rdr);
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
        private static List<Entity.ApplicationServiceMap> GetList(SqlCommand cmd)
        {
            List<Entity.ApplicationServiceMap> retList = new List<Entity.ApplicationServiceMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ApplicationServiceMap retItem = new Entity.ApplicationServiceMap();
                        var builder = DynamicBuilder<Entity.ApplicationServiceMap>.CreateBuilder(rdr);
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
        public static int Save(Entity.ApplicationServiceMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationServiceMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ApplicationServiceMapID", x.ApplicationServiceMapID));
            cmd.Parameters.Add(new SqlParameter("@ApplicationID", x.ApplicationID));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString());
        }
    }
}

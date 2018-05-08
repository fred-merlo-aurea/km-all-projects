using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ApplicationSettingMap
    {
        private static Entity.ApplicationSettingMap Get(SqlCommand cmd)
        {
            Entity.ApplicationSettingMap retItem = null;
            try { 
            using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new Entity.ApplicationSettingMap();
                    var builder = DynamicBuilder<Entity.ApplicationSettingMap>.CreateBuilder(rdr);
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
        private static List<Entity.ApplicationSettingMap> GetList(SqlCommand cmd)
        {
            List<Entity.ApplicationSettingMap> retList = new List<Entity.ApplicationSettingMap>();
            try { 
            using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
            {
                if (rdr != null)
                {
                    Entity.ApplicationSettingMap retItem = new Entity.ApplicationSettingMap();
                    var builder = DynamicBuilder<Entity.ApplicationSettingMap>.CreateBuilder(rdr);
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

        public static bool Save(Entity.ApplicationSettingMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ApplicationSettingMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ApplicationSettingMapID", x.ApplicationSettingMapID));
            cmd.Parameters.Add(new SqlParameter("@ApplicationID", x.ApplicationID));
            cmd.Parameters.Add(new SqlParameter("@ApplicationSettingID", x.ApplicationSettingID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}

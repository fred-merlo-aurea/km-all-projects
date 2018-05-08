using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ProfileClientMap
    {

        private static Entity.ProfileClientMap Get(SqlCommand cmd)
        {
            Entity.ProfileClientMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ProfileClientMap();
                        var builder = DynamicBuilder<Entity.ProfileClientMap>.CreateBuilder(rdr);
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
        private static List<Entity.ProfileClientMap> GetList(SqlCommand cmd)
        {
            List<Entity.ProfileClientMap> retList = new List<Entity.ProfileClientMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.ProfileClientMap retItem = new Entity.ProfileClientMap();
                        var builder = DynamicBuilder<Entity.ProfileClientMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.ProfileClientMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ProfileClientMap_Save";
            cmd.Parameters.Add(new SqlParameter("@ProfileID", x.ProfileID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}

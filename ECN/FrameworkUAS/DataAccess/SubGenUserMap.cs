using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class SubGenUserMap
    {
        public static List<Entity.SubGenUserMap> Select(int userId)
        {
            List<Entity.SubGenUserMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubGenUserMap_Select_UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);

            retItem = GetList<Entity.SubGenUserMap>(cmd);
            return retItem;
        }

        private static Entity.SubGenUserMap Get(SqlCommand cmd)
        {
            Entity.SubGenUserMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubGenUserMap();
                        var builder = DynamicBuilder<Entity.SubGenUserMap>.CreateBuilder(rdr);
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

        private static List<T> GetList<T>(SqlCommand cmd)
        {
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        var builder = DynamicBuilder<T>.CreateBuilder(rdr);
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

        public static bool Save(Entity.SubGenUserMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubGenUserMap_Save";
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@SubGenUserId", x.SubGenUserId));
            cmd.Parameters.Add(new SqlParameter("@SubGenAccountId", x.SubGenAccountId));
            cmd.Parameters.Add(new SqlParameter("@SubGenAccountName", x.SubGenAccountName));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}

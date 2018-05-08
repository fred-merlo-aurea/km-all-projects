using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class Filter
    {
        public static List<Entity.Filter> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Filter> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static bool Delete(int filterID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)filterID ?? DBNull.Value));
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static Entity.Filter Get(SqlCommand cmd)
        {
            Entity.Filter retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Filter();
                        DynamicBuilder<Entity.Filter> builder = DynamicBuilder<Entity.Filter>.CreateBuilder(rdr);
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

        public static List<Entity.Filter> GetList(SqlCommand cmd)
        {
            return KM.Common.DataFunctions.GetList<Entity.Filter>(cmd);
        }

        public static int Save(Entity.Filter x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Filter_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            if (x.FilterID > 0)
                x.DateUpdated = DateTime.Now;
            else
                x.DateCreated = DateTime.Now;

            cmd.Parameters.Add(new SqlParameter("@FilterID", (object)x.FilterID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterName", (object)x.FilterName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ProductID", (object)x.ProductID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterDetails", (object)x.FilterDetails ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)x.DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)x.CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}

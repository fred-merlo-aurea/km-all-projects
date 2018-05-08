using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SuppressionFile
    {
        private static Entity.SuppressionFile Get(SqlCommand cmd)
        {
            Entity.SuppressionFile retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SuppressionFile();
                        DynamicBuilder<Entity.SuppressionFile> builder = DynamicBuilder<Entity.SuppressionFile>.CreateBuilder(rdr);
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
        private static List<Entity.SuppressionFile> GetList(SqlCommand cmd)
        {
            List<Entity.SuppressionFile> retList = new List<Entity.SuppressionFile>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SuppressionFile retItem = new Entity.SuppressionFile();
                        DynamicBuilder<Entity.SuppressionFile> builder = DynamicBuilder<Entity.SuppressionFile>.CreateBuilder(rdr);
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
        public static List<Entity.SuppressionFile> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SuppressionFile_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static int Save(Entity.SuppressionFile x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SuppressionFile_Save";
            cmd.Parameters.Add(new SqlParameter("@SuppressionFileId", (object) x.SuppressionFileId ?? 0));
            cmd.Parameters.Add(new SqlParameter("@FileName", (object) x.FileName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FileDateModified", (object)x.FileDateModified ?? DBNull.Value));

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static int RunSuppression(KMPlatform.Object.ClientConnections client, string processcode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_Suppression";
            cmd.Parameters.Add(new SqlParameter("@processcode", (object)processcode ?? DBNull.Value));

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));

        }
    }
}

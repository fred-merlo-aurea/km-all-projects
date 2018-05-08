using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SuppressionFileDetail
    {
        private static Entity.SuppressionFileDetail Get(SqlCommand cmd)
        {
            Entity.SuppressionFileDetail retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SuppressionFileDetail();
                        DynamicBuilder<Entity.SuppressionFileDetail> builder = DynamicBuilder<Entity.SuppressionFileDetail>.CreateBuilder(rdr);
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
        private static List<Entity.SuppressionFileDetail> GetList(SqlCommand cmd)
        {
            List<Entity.SuppressionFileDetail> retList = new List<Entity.SuppressionFileDetail>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SuppressionFileDetail retItem = new Entity.SuppressionFileDetail();
                        DynamicBuilder<Entity.SuppressionFileDetail> builder = DynamicBuilder<Entity.SuppressionFileDetail>.CreateBuilder(rdr);
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
        public static int deleteBySourceFileId(Entity.SuppressionFile x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SuppressionFileDetail_deleteBySourceFileId";
            cmd.Parameters.Add(new SqlParameter("@SuppressionFileId", (object)x.SuppressionFileId ?? DBNull.Value));

            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool SaveBulkInsert(string xml, int suppressionFileID ,KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SuppressionFileDetail_SaveBulkInsert";
            cmd.Parameters.Add(new SqlParameter("@xml", xml));
            cmd.Parameters.Add(new SqlParameter("@suppFileId", suppressionFileID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}

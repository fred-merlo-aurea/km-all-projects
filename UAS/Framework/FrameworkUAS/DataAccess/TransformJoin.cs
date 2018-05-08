using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformJoin
    {
        public static List<Entity.TransformJoin> Select()
        {
            List<Entity.TransformJoin> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformJoin_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformJoin> Select(int TransformationID)
        {
            List<Entity.TransformJoin> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformJoin_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformJoin> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformJoin> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformJoin_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", sourceFileID));

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.TransformJoin Get(SqlCommand cmd)
        {
            Entity.TransformJoin retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformJoin();
                        DynamicBuilder<Entity.TransformJoin> builder = DynamicBuilder<Entity.TransformJoin>.CreateBuilder(rdr);
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

        private static List<Entity.TransformJoin> GetList(SqlCommand cmd)
        {
            List<Entity.TransformJoin> retList = new List<Entity.TransformJoin>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformJoin retItem = new Entity.TransformJoin();
                        DynamicBuilder<Entity.TransformJoin> builder = DynamicBuilder<Entity.TransformJoin>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformJoin x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformJoin_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformJoinID", x.TransformJoinID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@ColumnsToJoin", x.ColumnsToJoin));
            cmd.Parameters.Add(new SqlParameter("@Delimiter", x.Delimiter));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

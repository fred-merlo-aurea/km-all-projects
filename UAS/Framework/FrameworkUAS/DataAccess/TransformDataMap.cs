using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformDataMap
    {
        public static List<Entity.TransformDataMap> Select()
        {
            List<Entity.TransformDataMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformDataMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformDataMap> Select(int TransformationID)
        {
            List<Entity.TransformDataMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformDataMap_Select_By_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", TransformationID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformDataMap> SelectSourceFileID(int sourceFileID)
        {
            List<Entity.TransformDataMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformDataMap_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", sourceFileID));

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.TransformDataMap> Delete(int TransformDataMapID)
        {
            List<Entity.TransformDataMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformDataMap_Delete_TransformDataMapID";
            cmd.Parameters.Add(new SqlParameter("@TransformDataMapID", TransformDataMapID));

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.TransformDataMap Get(SqlCommand cmd)
        {
            Entity.TransformDataMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformDataMap();
                        DynamicBuilder<Entity.TransformDataMap> builder = DynamicBuilder<Entity.TransformDataMap>.CreateBuilder(rdr);
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

        private static List<Entity.TransformDataMap> GetList(SqlCommand cmd)
        {
            List<Entity.TransformDataMap> retList = new List<Entity.TransformDataMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformDataMap retItem = new Entity.TransformDataMap();
                        DynamicBuilder<Entity.TransformDataMap> builder = DynamicBuilder<Entity.TransformDataMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformDataMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformDataMap_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformDataMapID", x.TransformDataMapID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@MatchType", x.MatchType));
            cmd.Parameters.Add(new SqlParameter("@SourceData", x.SourceData));
            cmd.Parameters.Add(new SqlParameter("@DesiredData", x.DesiredData));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

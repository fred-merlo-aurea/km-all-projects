using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformationFieldMultiMap
    {
        public static List<Entity.TransformationFieldMultiMap> Select()
        {
            List<Entity.TransformationFieldMultiMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformationFieldMultiMap> SelectTransformationID(int transformationID)
        {
            List<Entity.TransformationFieldMultiMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_TransformationID";
            cmd.Parameters.AddWithValue("@TransformationID", transformationID);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.TransformationFieldMultiMap Get(SqlCommand cmd)
        {
            Entity.TransformationFieldMultiMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformationFieldMultiMap();
                        DynamicBuilder<Entity.TransformationFieldMultiMap> builder = DynamicBuilder<Entity.TransformationFieldMultiMap>.CreateBuilder(rdr);
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
        private static List<Entity.TransformationFieldMultiMap> GetList(SqlCommand cmd)
        {
            List<Entity.TransformationFieldMultiMap> retList = new List<Entity.TransformationFieldMultiMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformationFieldMultiMap retItem = new Entity.TransformationFieldMultiMap();
                        DynamicBuilder<Entity.TransformationFieldMultiMap> builder = DynamicBuilder<Entity.TransformationFieldMultiMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformationFieldMultiMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformationFieldMultiMapID", x.TransformationFieldMultiMapID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", x.FieldMappingID));
            cmd.Parameters.Add(new SqlParameter("@FieldMultiMapID", x.FieldMultiMapID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteByFieldMultiMapID(int FieldMultiMapID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Delete_ByFieldMultiMapID";
            cmd.Parameters.Add(new SqlParameter("@FieldMultiMapID", FieldMultiMapID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteBySourceFileID(int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Delete_BySourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteBySourceFileIDAndFieldMultiMapID(int SourceFileID, int FieldMultiMapID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Delete_BySourceFileID_FieldMultiMapID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@FieldMultiMapID", FieldMultiMapID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteByFieldMappingID(int FieldMappingID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMultiMap_Delete_ByFieldMappingID";
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", FieldMappingID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

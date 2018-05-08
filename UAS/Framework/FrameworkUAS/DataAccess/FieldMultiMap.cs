using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FieldMultiMap
    {
        public static List<Entity.FieldMultiMap> Select()
        {
            List<Entity.FieldMultiMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.FieldMultiMap> SelectFieldMappingID(int fieldMappingID)
        {
            List<Entity.FieldMultiMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Select_FieldMappingID";
            cmd.Parameters.AddWithValue("@FieldMappingID", fieldMappingID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.FieldMultiMap SelectFieldMultiMapID(int fieldMultiMapID)
        {
            Entity.FieldMultiMap retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Select_ByFieldMultiMapID";
            cmd.Parameters.AddWithValue("@FieldMultiMapID", fieldMultiMapID);

            retItem = Get(cmd);
            return retItem;
        }

        private static Entity.FieldMultiMap Get(SqlCommand cmd)
        {
            Entity.FieldMultiMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FieldMultiMap();
                        DynamicBuilder<Entity.FieldMultiMap> builder = DynamicBuilder<Entity.FieldMultiMap>.CreateBuilder(rdr);
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
        private static List<Entity.FieldMultiMap> GetList(SqlCommand cmd)
        {
            List<Entity.FieldMultiMap> retList = new List<Entity.FieldMultiMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FieldMultiMap retItem = new Entity.FieldMultiMap();
                        DynamicBuilder<Entity.FieldMultiMap> builder = DynamicBuilder<Entity.FieldMultiMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.FieldMultiMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Save";
            cmd.Parameters.Add(new SqlParameter("@FieldMultiMapID", x.FieldMultiMapID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", x.FieldMappingID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingTypeID", x.FieldMappingTypeID));
            cmd.Parameters.Add(new SqlParameter("@MAFField", x.MAFField));
            cmd.Parameters.Add(new SqlParameter("@DataType", x.DataType));
            cmd.Parameters.Add(new SqlParameter("@PreviewData", x.PreviewData));
            cmd.Parameters.Add(new SqlParameter("@ColumnOrder", x.ColumnOrder));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteBySourceFileID(int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Delete_MappingBySourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteByFieldMappingID(int FieldMappingID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Delete_MappingByFieldMappingID";
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", FieldMappingID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteByFieldMultiMapID(int FieldMultiMapID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMultiMap_Delete_FieldMultiMapID";
            cmd.Parameters.Add(new SqlParameter("@FieldMultiMapID", FieldMultiMapID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

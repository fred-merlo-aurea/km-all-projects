using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class TransformationFieldMap
    {
        public static List<Entity.TransformationFieldMap> Select()
        {
            List<Entity.TransformationFieldMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformationFieldMap> Select(int SourceFileID)
        {
            List<Entity.TransformationFieldMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Select_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", SourceFileID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.TransformationFieldMap> SelectTransformationID(int transformationID)
        {
            List<Entity.TransformationFieldMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Select_TransformationID";
            cmd.Parameters.AddWithValue("@TransformationID", transformationID);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.TransformationFieldMap Get(SqlCommand cmd)
        {
            Entity.TransformationFieldMap retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.TransformationFieldMap();
                        DynamicBuilder<Entity.TransformationFieldMap> builder = DynamicBuilder<Entity.TransformationFieldMap>.CreateBuilder(rdr);
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
        private static List<Entity.TransformationFieldMap> GetList(SqlCommand cmd)
        {
            List<Entity.TransformationFieldMap> retList = new List<Entity.TransformationFieldMap>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.TransformationFieldMap retItem = new Entity.TransformationFieldMap();
                        DynamicBuilder<Entity.TransformationFieldMap> builder = DynamicBuilder<Entity.TransformationFieldMap>.CreateBuilder(rdr);
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

        public static int Save(Entity.TransformationFieldMap x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformationFieldMapID", x.TransformationFieldMapID));
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", x.FieldMappingID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(string TransformationName, int clientID, string ColumnName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Delete";
            cmd.Parameters.Add(new SqlParameter("@TransformationName", TransformationName));
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@ColumnName", ColumnName));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteSourceFileID(int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Delete_SourceFileID";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteFieldMappingID(int FieldMappingID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Delete_FieldMapID";
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", FieldMappingID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteTransformationFieldMapID(int TransformationFieldMapID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_Delete_TransformationFieldMapID";
            cmd.Parameters.Add(new SqlParameter("@TransformationFieldMapId", TransformationFieldMapID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }        

        public static int DeleteFieldMappingID(string TransformationName, int clientID, int FieldMappingID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_TransformationFieldMap_DeleteFieldMappingID";
            cmd.Parameters.Add(new SqlParameter("@TransformationName", TransformationName));
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", FieldMappingID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

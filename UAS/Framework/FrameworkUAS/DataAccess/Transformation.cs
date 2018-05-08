using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class Transformation
    {
        public static Entity.Transformation SelectTransformationByID(int transformationID)
        {
            Entity.Transformation retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_TransformationID";
            cmd.Parameters.AddWithValue("@TransformationID", transformationID);

            retItem = Get(cmd);
            return retItem;
        }

        public static List<Entity.Transformation> Select()
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Transformation> Select(int transformationID)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_TransformationID";
            cmd.Parameters.AddWithValue("@TransformationID", transformationID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Transformation> SelectClient(int clientID)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Transformation> Select(int clientID, int sourceFileID)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_ClientID_SourceFileID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            retItem = GetList(cmd);
            return retItem;
        }
        //public static List<Entity.Transformation> Select(string clientName)
        //{
        //    List<Entity.Transformation> retItem = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_Transformation_Select_ClientName";
        //    cmd.Parameters.AddWithValue("@ClientName", clientName);

        //    retItem = GetList(cmd);
        //    return retItem;
        //}

        public static List<Entity.Transformation> SelectAssigned(string columnName)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_AssignedMappings";
            cmd.Parameters.AddWithValue("@ColumnName", columnName);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Transformation> SelectAssigned(int fieldMappingID)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_AssignedMappings_ByFieldMappingID";
            cmd.Parameters.AddWithValue("@FieldMappingID", fieldMappingID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Transformation> SelectTransformationID(string transformationName, string clientName)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Select_ByClientAndName";
            cmd.Parameters.AddWithValue("@TransformationName", transformationName);
            cmd.Parameters.AddWithValue("@ClientName", clientName);

            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.Transformation Get(SqlCommand cmd)
        {
            Entity.Transformation retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Transformation();
                        DynamicBuilder<Entity.Transformation> builder = DynamicBuilder<Entity.Transformation>.CreateBuilder(rdr);
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
        private static List<Entity.Transformation> GetList(SqlCommand cmd)
        {
            List<Entity.Transformation> retList = new List<Entity.Transformation>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Transformation retItem = new Entity.Transformation();
                        DynamicBuilder<Entity.Transformation> builder = DynamicBuilder<Entity.Transformation>.CreateBuilder(rdr);
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

        public static int Save(Entity.Transformation x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Save";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", x.TransformationID));
            cmd.Parameters.Add(new SqlParameter("@TransformationTypeID", x.TransformationTypeID));
            cmd.Parameters.Add(new SqlParameter("@TransformationName", x.TransformationName));
            cmd.Parameters.Add(new SqlParameter("@TransformationDescription", x.TransformationDescription));
            cmd.Parameters.AddWithValue("@MapsPubCode", x.MapsPubCode);
            cmd.Parameters.AddWithValue("@LastStepDataMap", x.LastStepDataMap);
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@IsTemplate", x.IsTemplate);
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(int transformationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Delete_TransformationID";
            cmd.Parameters.Add(new SqlParameter("@TransformationID", transformationID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int SelectPagingCount(int clientID, bool isTemplate, bool isActive, int TransformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Paging_Count_IsTemplate_IsActive_TransformationTypeId";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@IsTemplate", isTemplate);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@TransformationTypeId", TransformationTypeId);
            cmd.Parameters.AddWithValue("@IgnoreAdminTransformationTypes", ignoreAdminTransformationTypes);
            cmd.Parameters.AddWithValue("@AdminTransformId", adminTransformId);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static List<Entity.Transformation> SelectPaging(int clientID, int currentPage, int pageSize, bool isTemplate, bool isActive, int TransformationTypeId, bool ignoreAdminTransformationTypes, int adminTransformId, string sortField, string sortDirection)
        {
            List<Entity.Transformation> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Transformation_Paging_IsTemplate_IsActive_TransformationTypeId";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@IsTemplate", isTemplate);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@TransformationTypeId", TransformationTypeId);
            cmd.Parameters.AddWithValue("@IgnoreAdminTransformationTypes", ignoreAdminTransformationTypes);
            cmd.Parameters.AddWithValue("@AdminTransformId", adminTransformId);
            cmd.Parameters.AddWithValue("@SortField", sortField);
            cmd.Parameters.AddWithValue("@SortDirection", sortDirection);

            retItem = GetList(cmd);
            return retItem;
        }        
    }
}

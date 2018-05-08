using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class FieldMapping
    {
        public static List<Entity.FieldMapping> Select()
        {
            List<Entity.FieldMapping> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.FieldMapping SelectFieldMappingID(int fieldMappingID)
        {
            Entity.FieldMapping retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Select_FieldMappingID";
            cmd.Parameters.AddWithValue("@FieldMappingID", fieldMappingID);

            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.FieldMapping> Select(int sourceFileID)
        {
            List<Entity.FieldMapping> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Select_BySourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.FieldMapping> Select(string clientName)
        {
            List<Entity.FieldMapping> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Select_ClientName";
            cmd.Parameters.AddWithValue("@ClientName", clientName);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.FieldMapping> Select(int clientID, string fileName)
        {
            List<Entity.FieldMapping> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Select_ByClientSourceFile";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@FileName", fileName);

            retItem = GetList(cmd);
            return retItem;
        }

        public static bool ColumnReorder(int SourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_ColumnOrder_AutoReorder";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFileID));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }

        private static Entity.FieldMapping Get(SqlCommand cmd)
        {
            Entity.FieldMapping retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.FieldMapping();
                        DynamicBuilder<Entity.FieldMapping> builder = DynamicBuilder<Entity.FieldMapping>.CreateBuilder(rdr);
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
        private static List<Entity.FieldMapping> GetList(SqlCommand cmd)
        {
            List<Entity.FieldMapping> retList = new List<Entity.FieldMapping>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.FieldMapping retItem = new Entity.FieldMapping();
                        DynamicBuilder<Entity.FieldMapping> builder = DynamicBuilder<Entity.FieldMapping>.CreateBuilder(rdr);
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

        public static int Save(Entity.FieldMapping x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Save";
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", x.FieldMappingID));
            cmd.Parameters.Add(new SqlParameter("@FieldMappingTypeID", x.FieldMappingTypeID));
            cmd.Parameters.Add(new SqlParameter("@IsNonFileColumn", x.IsNonFileColumn));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@IncomingField", x.IncomingField));
            cmd.Parameters.Add(new SqlParameter("@MAFField", x.MAFField));
            cmd.Parameters.Add(new SqlParameter("@PubNumber", x.PubNumber));
            cmd.Parameters.Add(new SqlParameter("@DataType", x.DataType));
            cmd.Parameters.Add(new SqlParameter("@PreviewData", x.PreviewData));
            cmd.Parameters.Add(new SqlParameter("@ColumnOrder", x.ColumnOrder));
            cmd.Parameters.Add(new SqlParameter("@HasMultiMapping", x.HasMultiMapping));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DemographicUpdateCodeId", x.DemographicUpdateCodeId));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int Delete(int SourceFieldID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Delete_MappingBySourceFile";
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", SourceFieldID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }

        public static int DeleteMapping(int FieldMappingID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_FieldMapping_Delete_FieldMappingID";
            cmd.Parameters.Add(new SqlParameter("@FieldMappingID", FieldMappingID));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class AdHocDimensionGroup
    {
        public static List<Entity.AdHocDimensionGroup> Select()
        {
            List<Entity.AdHocDimensionGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.AdHocDimensionGroup> Select(int clientID)
        {
            List<Entity.AdHocDimensionGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.AdHocDimensionGroup> Select(int clientID, string adHocDimensionGroupName)
        {
            List<Entity.AdHocDimensionGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select_ClientID_AdHocDimensionGroupName";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@AdHocDimensionGroupName", adHocDimensionGroupName);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.AdHocDimensionGroup Select(int clientID, int sourceFileID, string adHocDimensionGroupName)
        {
            Entity.AdHocDimensionGroup retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select_ClientID_SourceFileID_AdHocDimensionGroupName";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@AdHocDimensionGroupName", adHocDimensionGroupName);

            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.AdHocDimensionGroup> Select(int clientID, int sourceFileID)
        {
            List<Entity.AdHocDimensionGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select_ClientID_SourceFileID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.AdHocDimensionGroup SelectByAdHocDimensionGroupId(int adHocDimensionGroupId)
        {
            Entity.AdHocDimensionGroup retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Select_AdHocDimensionGroupId";
            cmd.Parameters.AddWithValue("@AdHocDimensionGroupId", adHocDimensionGroupId);

            retItem = Get(cmd);
            return retItem;
        }
        private static Entity.AdHocDimensionGroup Get(SqlCommand cmd)
        {
            Entity.AdHocDimensionGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.AdHocDimensionGroup();
                        DynamicBuilder<Entity.AdHocDimensionGroup> builder = DynamicBuilder<Entity.AdHocDimensionGroup>.CreateBuilder(rdr);
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
        private static List<Entity.AdHocDimensionGroup> GetList(SqlCommand cmd)
        {
            List<Entity.AdHocDimensionGroup> retList = new List<Entity.AdHocDimensionGroup>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.AdHocDimensionGroup retItem = new Entity.AdHocDimensionGroup();
                        DynamicBuilder<Entity.AdHocDimensionGroup> builder = DynamicBuilder<Entity.AdHocDimensionGroup>.CreateBuilder(rdr);
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
        public static bool Save(Entity.AdHocDimensionGroup x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimensionGroup_Save";
            cmd.Parameters.Add(new SqlParameter("@AdHocDimensionGroupId", x.AdHocDimensionGroupId));
            cmd.Parameters.Add(new SqlParameter("@AdHocDimensionGroupName", x.AdHocDimensionGroupName));
            cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            cmd.Parameters.Add(new SqlParameter("@SourceFileID", x.SourceFileID));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@OrderOfOperation", x.OrderOfOperation));
            cmd.Parameters.Add(new SqlParameter("@StandardField", x.StandardField));
            cmd.Parameters.Add(new SqlParameter("@CreatedDimension", x.CreatedDimension));
            cmd.Parameters.Add(new SqlParameter("@DefaultValue", x.DefaultValue));
            cmd.Parameters.Add(new SqlParameter("@IsPubcodeSpecific", x.IsPubcodeSpecific));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool SaveBulkSqlInsert(List<Entity.AdHocDimensionGroup> list)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.AdHocDimensionGroup>.ToDataTable(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "AdHocDimensionGroup");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("AdHocDimensionGroupId", "AdHocDimensionGroupId");
                bc.ColumnMappings.Add("AdHocDimensionGroupName", "AdHocDimensionGroupName");
                bc.ColumnMappings.Add("IsActive", "IsActive");
                bc.ColumnMappings.Add("ClientID", "ClientID");
                bc.ColumnMappings.Add("SourceFileID", "SourceFileID");
                bc.ColumnMappings.Add("OrderOfOperation", "OrderOfOperation");
                bc.ColumnMappings.Add("StandardField", "StandardField");
                bc.ColumnMappings.Add("CreatedDimension", "CreatedDimension");
                bc.ColumnMappings.Add("DefaultValue", "DefaultValue");
                bc.ColumnMappings.Add("IsPubcodeSpecific", "IsPubcodeSpecific");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");

                bc.WriteToServer(dt);
                bc.Close();
            }
            catch (Exception ex)
            {
                done = false;
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return done;
        }
    }
}

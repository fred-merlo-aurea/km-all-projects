using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class AdHocDimension
    {
        public static List<Entity.AdHocDimension> Select(int adHocDimensionGroupId)
        {
            List<Entity.AdHocDimension> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimension_Select_AdHocDimensionGroupId";
            cmd.Parameters.AddWithValue("@AdHocDimensionGroupId", adHocDimensionGroupId);

            retItem = GetList(cmd);
            return retItem;
        }
        private static Entity.AdHocDimension Get(SqlCommand cmd)
        {
            Entity.AdHocDimension retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.AdHocDimension();
                        DynamicBuilder<Entity.AdHocDimension> builder = DynamicBuilder<Entity.AdHocDimension>.CreateBuilder(rdr);
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
        private static List<Entity.AdHocDimension> GetList(SqlCommand cmd)
        {
            List<Entity.AdHocDimension> retList = new List<Entity.AdHocDimension>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.AdHocDimension retItem = new Entity.AdHocDimension();
                        DynamicBuilder<Entity.AdHocDimension> builder = DynamicBuilder<Entity.AdHocDimension>.CreateBuilder(rdr);
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
        public static bool Delete(int sourceFileID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdHocDimension_Delete_SourceFileID";
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool SaveBulkSqlInsert(List<Entity.AdHocDimension> list)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.AdHocDimension>.ToDataTable(list);//bdr.ToDataTable<Entity.SubscriberOriginal>(list);
            bool done = true;
            SqlConnection conn = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.UAS.ToString());
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "AdHocDimension");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("AdHocDimensionID", "AdHocDimensionID");
                bc.ColumnMappings.Add("AdHocDimensionGroupId", "AdHocDimensionGroupId");
                bc.ColumnMappings.Add("IsActive", "IsActive");
                bc.ColumnMappings.Add("MatchValue", "MatchValue");
                bc.ColumnMappings.Add("Operator", "Operator");
                bc.ColumnMappings.Add("DimensionValue", "DimensionValue");
                bc.ColumnMappings.Add("UpdateUAD", "UpdateUAD");
                bc.ColumnMappings.Add("UADLastUpdatedDate", "UADLastUpdatedDate");
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

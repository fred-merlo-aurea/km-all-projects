using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberDemographicTransformed
    {
        public static List<Entity.SubscriberDemographicTransformed> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_Select_PubID";
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicTransformed> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_Select_SORecordIdentifier";
            cmd.Parameters.AddWithValue("@SORecordIdentifier", SORecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicTransformed> SelectSubscriberTransformed(Guid STRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_Select_STRecordIdentifier";
            cmd.Parameters.AddWithValue("@STRecordIdentifier", STRecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicTransformed> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        private static Entity.SubscriberDemographicTransformed Get(SqlCommand cmd)
        {
            Entity.SubscriberDemographicTransformed retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberDemographicTransformed();
                        DynamicBuilder<Entity.SubscriberDemographicTransformed> builder = DynamicBuilder<Entity.SubscriberDemographicTransformed>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberDemographicTransformed> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberDemographicTransformed> retList = new List<Entity.SubscriberDemographicTransformed>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberDemographicTransformed retItem = new Entity.SubscriberDemographicTransformed();
                        DynamicBuilder<Entity.SubscriberDemographicTransformed> builder = DynamicBuilder<Entity.SubscriberDemographicTransformed>.CreateBuilder(rdr);
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

        public static int Save(Entity.SubscriberDemographicTransformed x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_Save";
            cmd.Parameters.Add(new SqlParameter("@SubscriberDemographicTransformedID", x.SubscriberDemographicTransformedID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@SORecordIdentifier", x.SORecordIdentifier));
            cmd.Parameters.Add(new SqlParameter("@STRecordIdentifier", x.STRecordIdentifier));
            cmd.Parameters.Add(new SqlParameter("@MAFField", x.MAFField));
            cmd.Parameters.Add(new SqlParameter("@Value", x.Value));
            cmd.Parameters.Add(new SqlParameter("@NotExists", x.NotExists));
            cmd.Parameters.Add(new SqlParameter("@NotExistReason", x.NotExistReason));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DemographicUpdateCodeId", x.DemographicUpdateCodeId));
            cmd.Parameters.Add(new SqlParameter("@IsAdhoc", x.IsAdhoc));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));
            cmd.Parameters.Add(new SqlParameter("@IsDemoDate", x.IsDemoDate));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static DataTable MafFieldUpdateAction(string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dt_MafField_UpdateAction";
            cmd.Parameters.AddWithValue("@processCode", processCode);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }
        public static bool SaveBulkSqlInsert(List<Entity.SubscriberDemographicTransformed> list, KMPlatform.Object.ClientConnections client, bool isDataCompare, int sourceFileID, string processCode)
        {
            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.SubscriberDemographicTransformed>.ToDataTable(list);

            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "SubscriberDemographicTransformed");
                bc.BatchSize = 0;  // Sunil - Changed 1000 to 0 (process all in one batch)
                bc.BulkCopyTimeout = 0;

                bc.ColumnMappings.Add("SubscriberDemographicTransformedID", "SubscriberDemographicTransformedID");
                bc.ColumnMappings.Add("PubID", "PubID");
                bc.ColumnMappings.Add("SORecordIdentifier", "SORecordIdentifier");
                bc.ColumnMappings.Add("STRecordIdentifier", "STRecordIdentifier");
                bc.ColumnMappings.Add("MAFField", "MAFField");
                bc.ColumnMappings.Add("Value", "Value");
                bc.ColumnMappings.Add("NotExists", "NotExists");
                bc.ColumnMappings.Add("NotExistReason", "NotExistReason");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");
                bc.ColumnMappings.Add("DemographicUpdateCodeId", "DemographicUpdateCodeId");
                bc.ColumnMappings.Add("IsAdhoc", "IsAdhoc");
                bc.ColumnMappings.Add("ResponseOther", "ResponseOther");
                bc.ColumnMappings.Add("IsDemoDate", "IsDemoDate");

                FrameworkUAS.BusinessLogic.FileLog fl = new FrameworkUAS.BusinessLogic.FileLog();
                fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "DBCall:Bulk Insert SubscriberDemographicTransformed", processCode));
                bc.WriteToServer(dt);
                fl.Save(new FrameworkUAS.Entity.FileLog(sourceFileID, -99, "DBCall End:Bulk Insert SubscriberDemographicTransformed", processCode));
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
        public static bool DisableIndexes(KMPlatform.Object.ClientConnections client)
        {
            //--Disable Index
            //ALTER INDEX [IXYourIndex] ON YourTable DISABLE
            //GO
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_DisableIndexes";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);

        }
        public static bool EnableIndexes(KMPlatform.Object.ClientConnections client)
        {
            //--Enable Index
            //ALTER INDEX [IXYourIndex] ON YourTable REBUILD
            //GO
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicTransformed_EnableIndexes";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
    }
}

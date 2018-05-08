using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberDemographicOriginal
    {
        public static List<Entity.SubscriberDemographicOriginal> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicOriginal_Select_PubID";
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicOriginal> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicOriginal_Select_SORecordIdentifier";
            cmd.Parameters.AddWithValue("@SORecordIdentifier", SORecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicOriginal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicOriginal_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }

        public static List<Entity.SubscriberDemographicOriginal> SelectForSORecordIdentifier(string SORecordIdentifierList, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicOriginal_SelectForSORecordIdentifier";
            cmd.Parameters.AddWithValue("@SORecordIdentifierList", SORecordIdentifierList);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }

        private static Entity.SubscriberDemographicOriginal Get(SqlCommand cmd)
        {
            Entity.SubscriberDemographicOriginal retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberDemographicOriginal();
                        DynamicBuilder<Entity.SubscriberDemographicOriginal> builder = DynamicBuilder<Entity.SubscriberDemographicOriginal>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberDemographicOriginal> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberDemographicOriginal> retList = new List<Entity.SubscriberDemographicOriginal>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberDemographicOriginal retItem = new Entity.SubscriberDemographicOriginal();
                        DynamicBuilder<Entity.SubscriberDemographicOriginal> builder = DynamicBuilder<Entity.SubscriberDemographicOriginal>.CreateBuilder(rdr);
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

        public static int Save(Entity.SubscriberDemographicOriginal x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicOriginal_Save";
            cmd.Parameters.Add(new SqlParameter("@SDOriginalID", x.SDOriginalID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@SORecordIdentifier", x.SORecordIdentifier));
            cmd.Parameters.Add(new SqlParameter("@MAFField", x.MAFField));
            cmd.Parameters.Add(new SqlParameter("@Value", x.Value));
            cmd.Parameters.Add(new SqlParameter("@NotExists", x.NotExists));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DemographicUpdateCodeId", x.DemographicUpdateCodeId));
            cmd.Parameters.Add(new SqlParameter("@IsAdhoc", x.IsAdhoc));
            cmd.Parameters.Add(new SqlParameter("@ResponseOther", x.ResponseOther));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
        public static bool SaveBulkSqlInsert(List<Entity.SubscriberDemographicOriginal> insertList, KMPlatform.Object.ClientConnections client)
        {
            //List<Entity.SubscriberDemographicOriginal> insertList = new List<Entity.SubscriberDemographicOriginal>();
            //foreach (Entity.SubscriberOriginal so in list)
            //{
            //    foreach (Entity.SubscriberDemographicOriginal sdo in so.DemographicOriginalList)
            //    {
            //        insertList.Add(sdo);
            //    }
            //}

            //would need to send all the SubscriberOriginal records
            //send all SubscriberDemograpichOriginal records
            //update SubscriberDemographicOriginal with SubscriberOriginalID based on RecordIdentifier

            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.SubscriberDemographicOriginal>.ToDataTable(insertList);

            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "SubscriberDemographicOriginal");
                bc.BatchSize = 0;  // Sunil - Changed 1000 to 0 (process all in one batch)
                bc.BulkCopyTimeout = 0;
                bc.ColumnMappings.Add("SDOriginalID", "SDOriginalID");
                bc.ColumnMappings.Add("PubID", "PubID");
                bc.ColumnMappings.Add("SORecordIdentifier", "SORecordIdentifier");
                bc.ColumnMappings.Add("MAFField", "MAFField");
                bc.ColumnMappings.Add("Value", "Value");
                bc.ColumnMappings.Add("NotExists", "NotExists");
                bc.ColumnMappings.Add("DateCreated", "DateCreated");
                bc.ColumnMappings.Add("DateUpdated", "DateUpdated");
                bc.ColumnMappings.Add("CreatedByUserID", "CreatedByUserID");
                bc.ColumnMappings.Add("UpdatedByUserID", "UpdatedByUserID");
                bc.ColumnMappings.Add("DemographicUpdateCodeId", "DemographicUpdateCodeId");
                bc.ColumnMappings.Add("IsAdhoc", "IsAdhoc");
                bc.ColumnMappings.Add("ResponseOther", "ResponseOther");
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class SubscriberDemographicInvalid
    {
        public static List<Entity.SubscriberDemographicInvalid> SelectPublication(int pubID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicInvalid_Select_PubID";
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicInvalid> SelectSubscriberOriginal(Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicInvalid_Select_SORecordIdentifier";
            cmd.Parameters.AddWithValue("@SORecordIdentifier", SORecordIdentifier);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Entity.SubscriberDemographicInvalid> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicInvalid_SelectForFileAudit";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@StartDate", (object)startDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EndDate", (object)endDate ?? DBNull.Value);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return GetList(cmd);
        }
        
        private static Entity.SubscriberDemographicInvalid Get(SqlCommand cmd)
        {
            Entity.SubscriberDemographicInvalid retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SubscriberDemographicInvalid();
                        DynamicBuilder<Entity.SubscriberDemographicInvalid> builder = DynamicBuilder<Entity.SubscriberDemographicInvalid>.CreateBuilder(rdr);
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
        private static List<Entity.SubscriberDemographicInvalid> GetList(SqlCommand cmd)
        {
            List<Entity.SubscriberDemographicInvalid> retList = new List<Entity.SubscriberDemographicInvalid>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.SubscriberDemographicInvalid retItem = new Entity.SubscriberDemographicInvalid();
                        DynamicBuilder<Entity.SubscriberDemographicInvalid> builder = DynamicBuilder<Entity.SubscriberDemographicInvalid>.CreateBuilder(rdr);
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

        public static int Save(Entity.SubscriberDemographicInvalid x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriberDemographicInvalid_Save";
            cmd.Parameters.Add(new SqlParameter("@SDInvalidID", x.SDInvalidID));
            cmd.Parameters.Add(new SqlParameter("@PubID", x.PubID));
            cmd.Parameters.Add(new SqlParameter("@SIRecordIdentifier", x.SIRecordIdentifier));
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

        public static bool SaveBulkSqlInsert(List<Entity.SubscriberDemographicInvalid> insertList, KMPlatform.Object.ClientConnections client)
        {
            //List<Entity.SubscriberDemographicInvalid> insertList = new List<Entity.SubscriberDemographicInvalid>();
            //foreach (Entity.SubscriberInvalid so in list)
            //{
            //    foreach (Entity.SubscriberDemographicInvalid sdo in so.DemographicInvalidList)
            //    {
            //        insertList.Add(sdo);
            //    }
            //}

            //would need to send all the SubscriberInvalid records
            //send all SubscriberDemograpichInvalid records
            //update SubscriberDemographicInvalid with SubscriberInvalidID based on RecordIdentifier

            DataTable dt = Core_AMS.Utilities.BulkDataReader<Entity.SubscriberDemographicInvalid>.ToDataTable(insertList);

            bool done = true;
            SqlConnection conn = DataFunctions.GetClientSqlConnection(client);
            try
            {
                conn.Open();
                SqlBulkCopy bc = default(SqlBulkCopy);
                bc = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, null);
                bc.DestinationTableName = string.Format("[{0}].[dbo].[{1}]", conn.Database.ToString(), "SubscriberDemographicInvalid");
                bc.BatchSize = 0;
                bc.BulkCopyTimeout = 0;
                bc.ColumnMappings.Add("SDInvalidID", "SDInvalidID");
                bc.ColumnMappings.Add("PubID", "PubID");
                bc.ColumnMappings.Add("SORecordIdentifier", "SORecordIdentifier");
                bc.ColumnMappings.Add("SIRecordIdentifier", "SIRecordIdentifier");
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

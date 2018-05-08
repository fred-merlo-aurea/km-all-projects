using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using KM.Common;
using KM.Common.Data;

namespace FrameworkUAS.DataAccess
{
    public class AdmsLog
    {
        public static List<Entity.AdmsLog> Select(int clientId)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            x = GetList(cmd);
            return x;
        }
        public static List<Entity.AdmsLog> Select(int clientId, int sourceFileId)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_SourceFileId";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@SourceFileId", sourceFileId);
            x = GetList(cmd);
            return x;
        }
        public static List<Entity.AdmsLog> SelectNotCompleteNotFailed(int clientId)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_NotComplete_NotFailed";
            cmd.Parameters.AddWithValue("@clientId", clientId);
            x = GetList(cmd);
            return x;
        }
        public static List<Entity.AdmsLog> Select(int clientID, FrameworkUAD_Lookup.Enums.FileTypes fileType, DateTime startDate, DateTime endDate)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_dbDest_StartDate_EndDate";
            cmd.Parameters.AddWithValue("@ClientId", clientID);
            cmd.Parameters.AddWithValue("@fileType", fileType.ToString().Replace("_"," "));
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            x = GetList(cmd);
            return x;
        }
        public static List<Entity.AdmsLog> Select(int clientID, FrameworkUAS.BusinessLogic.AdmsLog.RecordSource recordSource, DateTime startDate, DateTime endDate)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_RecordSource_StartDate_EndDate";
            cmd.Parameters.AddWithValue("@ClientId", clientID);
            cmd.Parameters.AddWithValue("@recordSource", recordSource.ToString().Replace("_", " "));
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);

            x = GetList(cmd);
            return x;
        }
        //public static List<Entity.AdmsLog> Select(int clientId, int sourceFileId)
        //{
        //    List<Entity.AdmsLog> x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_AdmsLog_Select_ClientId";
        //    cmd.Parameters.AddWithValue("@ClientId", clientId);
        //    cmd.Parameters.AddWithValue("@SourceFileId", sourceFileId);
        //    x = GetList(cmd);
        //    return x;
        //}
        public static List<Entity.AdmsLog> Select(int clientId, DateTime fileStart)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_FileStart";
            cmd.Parameters.AddWithValue("@ClientID", clientId);            
            cmd.Parameters.AddWithValue("@FileStart", fileStart.ToShortDateString());
            x = GetList(cmd);
            return x;
        }
        //public static List<Entity.AdmsLog> Select(int clientId, int sourceFileId, DateTime fileStart)
        //{
        //    List<Entity.AdmsLog> x = null;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "e_AdmsLog_Select_ClientId_FileStart";
        //    cmd.Parameters.AddWithValue("@ClientID", clientId);
        //    cmd.Parameters.AddWithValue("@SourceFileId", sourceFileId);
        //    cmd.Parameters.AddWithValue("@FileStart", fileStart.ToShortDateString());
        //    x = GetList(cmd);
        //    return x;
        //}
        public static List<Entity.AdmsLog> Select(int clientId, string fileNameExact)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ClientId_FileNameExact";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@FileNameExact", fileNameExact);
            x = GetList(cmd);
            return x;
        }
        public static Entity.AdmsLog Select(string processCode)
        {
            Entity.AdmsLog x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Select_ProcessCode";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            x = Get(cmd);
            return x;
        }
        private static List<Entity.AdmsLog> GetList(SqlCommand cmd)
        {
            List<Entity.AdmsLog> retList = new List<Entity.AdmsLog>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.AdmsLog retItem = new Entity.AdmsLog();
                        DynamicBuilder<Entity.AdmsLog> builder = DynamicBuilder<Entity.AdmsLog>.CreateBuilder(rdr);
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
        private static Entity.AdmsLog Get(SqlCommand cmd)
        {
            Entity.AdmsLog retItem = new Entity.AdmsLog();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.UAS.ToString()))
                {
                    if (rdr != null)
                    {
                        DynamicBuilder<Entity.AdmsLog> builder = DynamicBuilder<Entity.AdmsLog>.CreateBuilder(rdr);
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

        public static int Save(Entity.AdmsLog x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_Save";
            cmd.Parameters.AddWithValue("@AdmsLogId", x.AdmsLogId);
            cmd.Parameters.AddWithValue("@ClientId", x.ClientId);
            cmd.Parameters.AddWithValue("@SourceFileId", x.SourceFileId);
            cmd.Parameters.AddWithValue("@FileNameExact", x.FileNameExact);
            cmd.Parameters.AddWithValue("@FileStart", x.FileStart);
            cmd.Parameters.AddWithValue("@FileEnd", (object) x.FileEnd ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FileStatusId", x.FileStatusId);
            cmd.Parameters.AddWithValue("@StatusMessage", x.StatusMessage);
            cmd.Parameters.AddWithValue("@AdmsStepId", x.AdmsStepId);
            cmd.Parameters.AddWithValue("@ProcessingStatusId", x.ProcessingStatusId);
            cmd.Parameters.AddWithValue("@ExecutionPointId", x.ExecutionPointId);
            cmd.Parameters.AddWithValue("@RecordSource", x.RecordSource);
            cmd.Parameters.AddWithValue("@ProcessCode", x.ProcessCode);
            cmd.Parameters.AddWithValue("@OriginalRecordCount", x.OriginalRecordCount);
            cmd.Parameters.AddWithValue("@OriginalProfileCount", x.OriginalProfileCount);
            cmd.Parameters.AddWithValue("@OriginalDemoCount", x.OriginalDemoCount);
            cmd.Parameters.AddWithValue("@TransformedRecordCount", x.TransformedRecordCount);
            cmd.Parameters.AddWithValue("@TransformedProfileCount", x.TransformedProfileCount);
            cmd.Parameters.AddWithValue("@TransformedDemoCount", x.TransformedDemoCount);
            cmd.Parameters.AddWithValue("@DuplicateRecordCount", x.DuplicateRecordCount);
            cmd.Parameters.AddWithValue("@DuplicateProfileCount", x.DuplicateProfileCount);
            cmd.Parameters.AddWithValue("@DuplicateDemoCount", x.DuplicateDemoCount);
            cmd.Parameters.AddWithValue("@FailedRecordCount", x.FailedRecordCount);
            cmd.Parameters.AddWithValue("@FailedProfileCount", x.FailedProfileCount);
            cmd.Parameters.AddWithValue("@FailedDemoCount", x.FailedDemoCount);
            cmd.Parameters.AddWithValue("@FinalRecordCount", x.FinalRecordCount);
            cmd.Parameters.AddWithValue("@FinalProfileCount", x.FinalProfileCount);
            cmd.Parameters.AddWithValue("@FinalDemoCount", x.FinalDemoCount);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object) x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", x.UpdatedByUserID);
            cmd.Parameters.AddWithValue("@MatchedRecordCount", x.MatchedRecordCount);
            cmd.Parameters.AddWithValue("@UadConsensusCount", x.UadConsensusCount);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static bool UpdateStatusMessage(string processCode, string StatusMessage, int updatedByUserId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateStatusMessage";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@StatusMessage", StatusMessage);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static int UpdateFileStatus(string processCode,string fileStatus, int userId, string StatusMessage = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateFileStatus";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@FileStatus", fileStatus.ToString());
            cmd.Parameters.AddWithValue("@UserId", userId.ToString());
            cmd.Parameters.AddWithValue("@StatusMessage", StatusMessage);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int UpdateAdmsStep(string processCode, string step, int userId, string StatusMessage = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateAdmsStep";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@AdmsStep", step.ToString());
            cmd.Parameters.AddWithValue("@UserId", userId.ToString());
            cmd.Parameters.AddWithValue("@StatusMessage", StatusMessage);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int UpdateProcessingStatus(string processCode, string status, int userId, string StatusMessage = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateProcessingStatus";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@ProcessingStatus", status.ToString());
            cmd.Parameters.AddWithValue("@UserId", userId.ToString());
            cmd.Parameters.AddWithValue("@StatusMessage", StatusMessage);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static int UpdateExecutionPoint(string processCode, string ep, int userId, string StatusMessage = "")
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateExecutionPoint";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@ExecutionPoint", ep.ToString());
            cmd.Parameters.AddWithValue("@UserId", userId.ToString());

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.UAS.ToString()));
        }
        public static bool UpdateFileEnd(string processCode, DateTime fileEnd, int updatedByUserId = 1)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateFileEnd";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@FileEnd", fileEnd);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateOriginalCounts(string processCode, int origRecordCount, int origProfileCount, int origDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateOriginalCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@OrigRecordCount", origRecordCount.ToString());
            cmd.Parameters.AddWithValue("@OrigProfileCount", origProfileCount.ToString());
            cmd.Parameters.AddWithValue("@OrigDemoCount", origDemoCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateTransformedCounts(string processCode, int transRecordCount, int transProfileCount, int transDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateTransformedCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@TransRecordCount", transRecordCount.ToString());
            cmd.Parameters.AddWithValue("@TransProfileCount", transProfileCount.ToString());
            cmd.Parameters.AddWithValue("@TransDemoCount", transDemoCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateDuplicateCounts(string processCode, int dupRecordCount, int dupProfileCount, int dupDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateDuplicateCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@DupRecordCount", dupRecordCount.ToString());
            cmd.Parameters.AddWithValue("@DupProfileCount", dupProfileCount.ToString());
            cmd.Parameters.AddWithValue("@DupDemoCount", dupDemoCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateFailedCounts(string processCode, int failRecordCount, int failProfileCount, int failDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateFailedCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@FailRecordCount", failRecordCount.ToString());
            cmd.Parameters.AddWithValue("@FailProfileCount", failProfileCount.ToString());
            cmd.Parameters.AddWithValue("@FailDemoCount", failDemoCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateFinalCounts(string processCode, int finalRecordCount, int finalProfileCount, int finalDemoCount, int matchedRecordCount, int uadConsensusCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0, int archiveCount = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateFinalCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@FinalRecordCount", finalRecordCount.ToString());
            cmd.Parameters.AddWithValue("@FinalProfileCount", finalProfileCount.ToString());
            cmd.Parameters.AddWithValue("@FinalDemoCount", finalDemoCount.ToString());
            cmd.Parameters.AddWithValue("@MatchedRecordCount", matchedRecordCount.ToString());
            cmd.Parameters.AddWithValue("@UadConsensusCount", uadConsensusCount.ToString());
            cmd.Parameters.AddWithValue("@ArchiveCount", archiveCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool UpdateFinalCountsAfterProcessToLive(string processCode, int finalRecordCount, int finalProfileCount, int finalDemoCount, int ignoredRecordCount, int ignoredProfileCount, int ignoredDemoCount, int matchedRecordCount, int uadConsensusCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateFinalCountsAfterProcessToLive";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@FinalRecordCount", finalRecordCount.ToString());
            cmd.Parameters.AddWithValue("@FinalProfileCount", finalProfileCount.ToString());
            cmd.Parameters.AddWithValue("@FinalDemoCount", finalDemoCount.ToString());
            cmd.Parameters.AddWithValue("@MatchedRecordCount", matchedRecordCount.ToString());
            cmd.Parameters.AddWithValue("@UadConsensusCount", uadConsensusCount.ToString());
            cmd.Parameters.AddWithValue("@IgnoredRecordCount", ignoredRecordCount.ToString());
            cmd.Parameters.AddWithValue("@IgnoredProfileCount", ignoredProfileCount.ToString());
            cmd.Parameters.AddWithValue("@IgnoredDemoCount", ignoredDemoCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        
        public static bool UpdateDimension(string processCode, int dimensionCount, int dimensionSubscriberCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_UpdateDimensionCounts";
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            cmd.Parameters.AddWithValue("@UpdatedByUserId", updatedByUserId.ToString());
            cmd.Parameters.AddWithValue("@DimensionCount", dimensionCount.ToString());
            cmd.Parameters.AddWithValue("@DimensionSubscriberCount", dimensionSubscriberCount.ToString());
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
        public static bool AdmsLogCleanUp(int clientId, bool isADMS)
        {
            List<Entity.AdmsLog> x = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_AdmsLog_CleanUp";
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@IsADMS", isADMS);
            x = GetList(cmd);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.UAS.ToString());
        }
    }
}

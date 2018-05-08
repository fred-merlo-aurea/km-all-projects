using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class AdmsLog
    {
        public enum RecordSource
        {
            CIRC,
            UAD,
            API
        }

        private static void SaveFileLog(string processCode, int sourceFileId, string message)
        {
            var fileLog = new FileLog();
            var entityFileLog = new Entity.FileLog
            {
                LogDate = DateTime.Now,
                LogTime = DateTime.Now.TimeOfDay,
                Message = message,
                ProcessCode = processCode,
                SourceFileID = sourceFileId
            };

            fileLog.Save(entityFileLog);
        }

        public List<Entity.AdmsLog> Select(int clientID)
        {
            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID).ToList();

            return x;
        }
        public List<Entity.AdmsLog> Select(int clientID, int sourceFileID)
        {
            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID, sourceFileID).ToList();

            return x;
        }
        public List<Entity.AdmsLog> SelectNotCompleteNotFailed(int clientId)
        {
            return DataAccess.AdmsLog.SelectNotCompleteNotFailed(clientId).ToList();
        }
        public List<Entity.AdmsLog> SelectNotCompleteNotFailed(int clientId, FrameworkUAD_Lookup.Enums.FileTypes fileType, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
                startDate = DateTime.Now;
            if (endDate == null && startDate.HasValue)
                endDate = startDate.Value.AddDays(-7);
            var admsList = SelectNotCompleteNotFailed(clientId).ToList().Where(x => x.RecordSource.Equals(fileType.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                                                                                    x.FileStart >= startDate.Value && x.FileEnd <= endDate.Value).ToList();
            return admsList;
        }
        public List<Entity.AdmsLog> Select(int clientID, FrameworkUAD_Lookup.Enums.FileTypes fileType, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
                startDate = DateTime.Now;
            if (endDate == null && startDate.HasValue)
                endDate = startDate.Value.AddDays(-7);

            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID, fileType, startDate.Value, endDate.Value).ToList();

            return x;
        }
        public List<Entity.AdmsLog> Select(int clientID, RecordSource recordSource, DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
                startDate = DateTime.Now;
            if (endDate == null && startDate.HasValue)
                endDate = startDate.Value.AddDays(-7);

            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID, recordSource, startDate.Value, endDate.Value).ToList();

            return x;
        }
        //public List<Entity.AdmsLog> Select(int clientID, int sourceFileId)
        //{
        //    List<Entity.AdmsLog> x = null;
        //    x = DataAccess.AdmsLog.Select(clientID, sourceFileId).ToList();

        //    return x;
        //}
        public List<Entity.AdmsLog> Select(int clientID, DateTime fileStart)
        {
            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID, fileStart).ToList();

            return x;
        }
        //public List<Entity.AdmsLog> Select(int clientID, int sourceFileId, DateTime fileStart)
        //{
        //    List<Entity.AdmsLog> x = null;
        //    x = DataAccess.AdmsLog.Select(clientID, sourceFileId, fileStart).ToList();

        //    return x;
        //}
        public List<Entity.AdmsLog> Select(int clientID, string fileNameExact)
        {
            List<Entity.AdmsLog> x = null;
            x = DataAccess.AdmsLog.Select(clientID, fileNameExact).ToList();

            return x;
        }
        public Entity.AdmsLog Select(string processCode)
        {
            Entity.AdmsLog x = null;
            x = DataAccess.AdmsLog.Select(processCode);

            return x;
        }
        public int Save(Entity.AdmsLog x)
        {
            if (x.CreatedByUserID == 0)
                x.CreatedByUserID = 1;
            using (TransactionScope scope = new TransactionScope())
            {
                x.AdmsLogId = DataAccess.AdmsLog.Save(x);
                scope.Complete();
            }

            return x.AdmsLogId;
        }

        public bool UpdateOriginalCounts(string processCode, int origRecordCount, int origProfileCount, int origDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {

            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateOriginalCounts(processCode, origRecordCount, origProfileCount, origDemoCount, updatedByUserId);
                scope.Complete();
            }
            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog Original Counts - Records:" + origRecordCount.ToString() + "  Profiles:" + origProfileCount.ToString() + "  Demos:" + origDemoCount.ToString();
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }

        public bool UpdateTransformedCounts(
            string processCode,
            int transRecordCount,
            int transProfileCount,
            int transDemoCount,
            int updatedByUserId = 1,
            bool createLog = false,
            int sourceFileId = 0)
        {
            bool complete;
            using (var scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateTransformedCounts(processCode, transRecordCount, transProfileCount, transDemoCount, updatedByUserId);
                scope.Complete();
            }

            if (createLog)
            {
                SaveFileLog(
                    processCode,
                    sourceFileId,
                    $"Update AdmsLog Transformed Counts - Records:{transRecordCount}  Profiles:{transProfileCount}  Demos:{transDemoCount}");
            }

            return complete;
        }

        public bool UpdateDuplicateCounts(string processCode, int dupRecordCount, int dupProfileCount, int dupDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {

            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateDuplicateCounts(processCode, dupRecordCount, dupProfileCount, dupDemoCount, updatedByUserId);
                scope.Complete();
            }
            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog Duplicate Counts - Records:" + dupRecordCount.ToString() + "  Profiles:" + dupProfileCount.ToString() + "  Demos:" + dupDemoCount.ToString();
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }
        public bool UpdateFailedCounts(
            string processCode, 
            int failRecordCount,
            int failProfileCount,
            int failDemoCount,
            int updatedByUserId = 1,
            bool createLog = false,
            int sourceFileId = 0)
        {
            bool complete;
            using (var scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateFailedCounts(processCode, failRecordCount, failProfileCount, failDemoCount, updatedByUserId);
                scope.Complete();
            }

            if (createLog)
            {
                SaveFileLog(
                    processCode,
                    sourceFileId,
                    $"Update AdmsLog Failed Counts - Records:{failRecordCount}  Profiles:{failProfileCount}  Demos:{failDemoCount}");
            }

            return complete;
        }

        public bool UpdateFinalCounts(
            string processCode,
            int finalRecordCount,
            int finalProfileCount,
            int finalDemoCount,
            int matchedRecordCount,
            int uadConsensusCount,
            int updatedByUserId = 1,
            bool createLog = false,
            int sourceFileId = 0,
            int archiveCount = 0)
        {
            bool complete;
            using (var scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateFinalCounts(
                    processCode,
                    finalRecordCount, 
                    finalProfileCount,
                    finalDemoCount,
                    matchedRecordCount, 
                    uadConsensusCount,
                    updatedByUserId,
                    false,
                    sourceFileId,
                    archiveCount);

                scope.Complete();
            }

            if (createLog)
            {
                SaveFileLog(
                    processCode,
                    sourceFileId,
                    $"Update AdmsLog Final Counts - Records:{finalRecordCount}  Profiles:{finalProfileCount}  Demos:{finalDemoCount}");
            }

            return complete;
        }

        public bool UpdateFinalCountsAfterProcessToLive(string processCode, int finalRecordCount, int finalProfileCount, int finalDemoCount, int ignoredRecordCount, int ignoredProfileCount, int ignoredDemoCount, int matchedRecordCount, int uadConsensusCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {

            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateFinalCountsAfterProcessToLive(processCode, finalRecordCount, finalProfileCount, finalDemoCount, ignoredRecordCount, ignoredProfileCount, ignoredDemoCount, matchedRecordCount, uadConsensusCount, updatedByUserId);
                scope.Complete();
            }
            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog Finished Counts - Records:" + finalRecordCount.ToString() + "  Profiles:" + finalProfileCount.ToString() + "  Demos:" + finalDemoCount.ToString();
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }
        public bool UpdateDimension(string processCode, int dimensionCount, int dimensionSubscriberCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0)
        {

            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateDimension(processCode, dimensionCount, dimensionSubscriberCount, updatedByUserId);
                scope.Complete();
            }
            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog Dimension Counts - Count:" + dimensionCount.ToString() + "  Profiles:" + dimensionSubscriberCount.ToString();
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }

        public bool UpdateStatusMessage(string processCode, string currentStatus, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0, int fileStatusTypeId = 0)
        {

            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                complete = DataAccess.AdmsLog.UpdateStatusMessage(processCode, currentStatus, updatedByUserId);
                scope.Complete();
            }
            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.FileStatusTypeID = fileStatusTypeId;
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog - set CurrentStatus: " + currentStatus;
                if (!string.IsNullOrEmpty(currentStatus))
                    fl.Message += "  Current Status: " + currentStatus;
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }
        public int UpdateFileStatus(string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, int userId, string currentStatus = "", bool createLog = true, int sourceFileId = 0)
        {
            int codeId = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                codeId = DataAccess.AdmsLog.UpdateFileStatus(processCode, fileStatus.ToString().Replace("_", " "), userId, currentStatus);
                scope.Complete();
            }

            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.FileStatusTypeID = codeId;
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog - set FileStatusId:" + fileStatus.ToString();
                if (!string.IsNullOrEmpty(currentStatus))
                    fl.Message += "  Current Status: " + currentStatus;
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return codeId;
        }
        public int UpdateCurrentStep(string processCode, FrameworkUAD_Lookup.Enums.ADMS_StepType step, int userId, string currentStatus = "", bool createLog = true, int sourceFileId = 0)
        {
            int codeId = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                codeId = DataAccess.AdmsLog.UpdateAdmsStep(processCode, step.ToString().Replace("_", " "), userId, currentStatus);
                scope.Complete();
            }

            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog - set Current Step: " + step.ToString().Replace("_", " ");
                if (!string.IsNullOrEmpty(currentStatus))
                    fl.Message += "  Current Status: " + currentStatus;
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return codeId;
        }
        public int UpdateProcessingStatus(
            string processCode,
            FrameworkUAD_Lookup.Enums.ProcessingStatusType status,
            int userId,
            string currentStatus = "",
            bool createLog = false,
            int sourceFileId = 0)
        {
            int codeId;
            using (var scope = new TransactionScope())
            {
                codeId = DataAccess.AdmsLog.UpdateProcessingStatus(processCode, status.ToString().Replace("_", " "), userId, currentStatus);
                scope.Complete();
            }

            if (createLog)
            {
                var message = $"Update AdmsLog - set ProcessingStatusId:{status}";

                if (!string.IsNullOrEmpty(currentStatus))
                {
                    message = $"{message}  Current Status: {currentStatus}";
                }

                SaveFileLog(processCode, sourceFileId, message);
            }

            return codeId;
        }

        public int UpdateExecutionPoint(
            string processCode,
            FrameworkUAD_Lookup.Enums.ExecutionPointType executionPointType,
            int userId,
            string currentStatus = "",
            bool createLog = false,
            int sourceFileId = 0)
        {
            int codeId;

            using (var scope = new TransactionScope())
            {
                codeId = DataAccess.AdmsLog.UpdateExecutionPoint(processCode, executionPointType.ToString().Replace("_", " "), userId, currentStatus);
                scope.Complete();
            }

            if (createLog)
            {
                var message = $"Update AdmsLog - set ExecutionPointId:{executionPointType}";

                if (!string.IsNullOrEmpty(currentStatus))
                {
                    message = $"{message}  Current Status: " + currentStatus;
                }

                SaveFileLog(processCode, sourceFileId, message);
            }

            return codeId;
        }
        public bool Update(string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, FrameworkUAD_Lookup.Enums.ADMS_StepType step, FrameworkUAD_Lookup.Enums.ProcessingStatusType status,
                        FrameworkUAD_Lookup.Enums.ExecutionPointType ep, int userId, string currentStatus = "", bool createLog = false, int sourceFileId = 0, bool setFileEnd = false)
        {
            currentStatus += " - " + DateTime.Now.TimeOfDay.ToString();
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateFileStatus(processCode, fileStatus, userId, currentStatus, false, sourceFileId);
                scope.Complete();
                complete = true;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateCurrentStep(processCode, step, userId, currentStatus, false, sourceFileId);
                scope.Complete();
                complete = true;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateProcessingStatus(processCode, status, userId, currentStatus, false, sourceFileId);
                scope.Complete();
                complete = true;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateExecutionPoint(processCode, ep, userId, currentStatus, false, sourceFileId);
                scope.Complete();
                complete = true;
            }


            if (setFileEnd)
                UpdateFileEnd(processCode, DateTime.Now);

            if (createLog == true)
            {
                FileLog flWrk = new FileLog();
                Entity.FileLog fl = new Entity.FileLog();
                fl.LogDate = DateTime.Now;
                fl.LogTime = DateTime.Now.TimeOfDay;
                fl.Message = "Update AdmsLog - FileStatus:" + fileStatus.ToString() + " CurrentStep:" + step.ToString() + " ProcessingStatus:" + status.ToString() + " ExecutionPoint:" + ep.ToString();
                if (!string.IsNullOrEmpty(currentStatus))
                    fl.Message += "  Current Status: " + currentStatus;
                fl.ProcessCode = processCode;
                fl.SourceFileID = sourceFileId;
                flWrk.Save(fl);
            }

            return complete;
        }
        public bool UpdateFileEnd(string processCode, DateTime fileEnd, int updatedByUserId = 1, int sourceFileId = 0)
        {
            FileLog flWrk = new FileLog();
            Entity.FileLog fl = new Entity.FileLog();
            fl.LogDate = DateTime.Now;
            fl.LogTime = DateTime.Now.TimeOfDay;
            fl.Message = "Update AdmsLog - FileEnd:" + fileEnd.ToString();
            fl.ProcessCode = processCode;
            fl.SourceFileID = sourceFileId;
            flWrk.Save(fl);

            return DataAccess.AdmsLog.UpdateFileEnd(processCode, fileEnd, updatedByUserId);
        }
        public bool AdmsLogCleanUp(int clientID, bool isADMS)
        {
            return DataAccess.AdmsLog.AdmsLogCleanUp(clientID, isADMS);            
        }
    }
}

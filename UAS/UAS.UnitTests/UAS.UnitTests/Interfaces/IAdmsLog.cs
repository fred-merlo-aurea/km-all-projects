using System;
using System.Collections.Generic;
using FrameworkUAS.BusinessLogic;
using UasEntity = FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IAdmsLog
    {
        bool AdmsLogCleanUp(int clientID, bool isADMS);
        int Save(UasEntity.AdmsLog x);
        UasEntity.AdmsLog Select(string processCode);
        List<UasEntity.AdmsLog> Select(int clientID);
        List<UasEntity.AdmsLog> Select(int clientID, string fileNameExact);
        List<UasEntity.AdmsLog> Select(int clientID, int sourceFileID);
        List<UasEntity.AdmsLog> Select(int clientID, DateTime fileStart);
        List<UasEntity.AdmsLog> Select(int clientID, AdmsLog.RecordSource recordSource, DateTime? startDate = default(DateTime?), DateTime? endDate = default(DateTime?));
        List<UasEntity.AdmsLog> Select(int clientID, FrameworkUAD_Lookup.Enums.FileTypes fileType, DateTime? startDate = default(DateTime?), DateTime? endDate = default(DateTime?));
        List<UasEntity.AdmsLog> SelectNotCompleteNotFailed(int clientId);
        List<UasEntity.AdmsLog> SelectNotCompleteNotFailed(int clientId, FrameworkUAD_Lookup.Enums.FileTypes fileType, DateTime? startDate = default(DateTime?), DateTime? endDate = default(DateTime?));
        bool Update(string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, FrameworkUAD_Lookup.Enums.ADMS_StepType step, FrameworkUAD_Lookup.Enums.ProcessingStatusType status, FrameworkUAD_Lookup.Enums.ExecutionPointType ep, int userId, string currentStatus = "", bool createLog = false, int sourceFileId = 0, bool setFileEnd = false);
        int UpdateCurrentStep(string processCode, FrameworkUAD_Lookup.Enums.ADMS_StepType step, int userId, string currentStatus = "", bool createLog = true, int sourceFileId = 0);
        bool UpdateDimension(string processCode, int dimensionCount, int dimensionSubscriberCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
        bool UpdateDuplicateCounts(string processCode, int dupRecordCount, int dupProfileCount, int dupDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
        int UpdateExecutionPoint(string processCode, FrameworkUAD_Lookup.Enums.ExecutionPointType ep, int userId, string currentStatus = "", bool createLog = false, int sourceFileId = 0);
        bool UpdateFailedCounts(string processCode, int failRecordCount, int failProfileCount, int failDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
        bool UpdateFileEnd(string processCode, DateTime fileEnd, int updatedByUserId = 1, int sourceFileId = 0);
        int UpdateFileStatus(string processCode, FrameworkUAD_Lookup.Enums.FileStatusType fileStatus, int userId, string currentStatus = "", bool createLog = true, int sourceFileId = 0);
        bool UpdateFinalCounts(string processCode, int finalRecordCount, int finalProfileCount, int finalDemoCount, int matchedRecordCount, int uadConsensusCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0, int archiveCount = 0);
        bool UpdateFinalCountsAfterProcessToLive(string processCode, int finalRecordCount, int finalProfileCount, int finalDemoCount, int ignoredRecordCount, int ignoredProfileCount, int ignoredDemoCount, int matchedRecordCount, int uadConsensusCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
        bool UpdateOriginalCounts(string processCode, int origRecordCount, int origProfileCount, int origDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
        int UpdateProcessingStatus(string processCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType status, int userId, string currentStatus = "", bool createLog = false, int sourceFileId = 0);
        bool UpdateStatusMessage(string processCode, string currentStatus, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0, int fileStatusTypeId = 0);
        bool UpdateTransformedCounts(string processCode, int transRecordCount, int transProfileCount, int transDemoCount, int updatedByUserId = 1, bool createLog = false, int sourceFileId = 0);
    }
}
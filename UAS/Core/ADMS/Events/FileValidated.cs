using System;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    public class FileValidated : FileBase, IEventMessage
    {
        private readonly Guid _eventId;

        public FileValidated()
        {
            _eventId = Guid.NewGuid();
        }
        public FileValidated(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid, 
            FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, FrameworkUAD.Object.ValidationResult validationResult, int threadId)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            IsKnownCustomerFileName = isKnownCustomerFileName;
            IsValidFileType = isValidFileType;
            IsFileSchemaValid = isFileSchemaValid;
            SourceFile = sourceFile;
            ValidationResult = validationResult;
            ThreadId = threadId;

            AdmsLog = admsLog;
            //update the AdmsLog items
            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            AdmsLog.StatusMessage = FrameworkUAD_Lookup.Enums.ProcessingStatusType.Validated.ToString();
            //AdmsLog.CurrentStepId = should be set BEFORE starting this process //alWrk.UpdateCurrentStep(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Validated, 1, "Validated", true, AdmsLog.SourceFileId);
            //AdmsLog.FileStatusId = this has not changed
            AdmsLog.ProcessingStatusId = alWrk.UpdateProcessingStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Validated, 1, "Validated", true, AdmsLog.SourceFileId);
            //AdmsLog.ExecutionPointId : not going to set this in these events
            AdmsLog.DateUpdated = DateTime.Now;
            AdmsLog.UpdatedByUserID = 1;
            alWrk.Save(AdmsLog);
        }
        public Guid MessageId
        {
            get
            {
                return _eventId;
            }
        }
        public int ThreadId { get; set; }
    }
}
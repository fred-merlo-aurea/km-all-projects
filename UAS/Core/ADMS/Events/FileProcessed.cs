using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    public class FileProcessed : IEventMessage
    {
        private readonly Guid _eventId;

        public FileProcessed() { _eventId = Guid.NewGuid(); }
        public FileProcessed(KMPlatform.Entity.Client client, int sourceFileID, FrameworkUAS.Entity.AdmsLog admsLog,
                            System.IO.FileInfo fileName = null, bool isKnownCustomerFileName = false, bool isValidFileType = false, bool isFileSchemaValid = false,
                            FrameworkUAD.Object.ValidationResult validationResult = null)
        {
            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            _eventId = Guid.NewGuid();
            Client = client;
            SourceFile = sfData.SelectSourceFileID(sourceFileID, true);
            ImportFile = fileName;
            IsFileSchemaValid = isFileSchemaValid;
            IsKnownCustomerFileName = isKnownCustomerFileName;
            IsValidFileType = isValidFileType;
            ValidationResult = validationResult;
            AdmsLog = admsLog;
            //update the AdmsLog items
            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            AdmsLog.StatusMessage = FrameworkUAD_Lookup.Enums.ProcessingStatusType.Processed.ToString();
            AdmsLog.AdmsStepId = alWrk.UpdateCurrentStep(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Processed, 1, "Processed", true, AdmsLog.SourceFileId);
            AdmsLog.FileStatusId = alWrk.UpdateFileStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.FileStatusType.Completed, 1, "Completed", true, AdmsLog.SourceFileId);
            AdmsLog.ProcessingStatusId = alWrk.UpdateProcessingStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Processed, 1, "Processed", true, AdmsLog.SourceFileId);
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

        public KMPlatform.Entity.Client Client { get; set; }
        public System.IO.FileInfo ImportFile { get; set; }
        public FrameworkUAS.Entity.SourceFile SourceFile { get; set; }
        public bool IsKnownCustomerFileName { get; set; }
        public bool IsValidFileType { get; set; }
        public bool IsFileSchemaValid { get; set; }
        public FrameworkUAD.Object.ValidationResult ValidationResult { get; set; }
        public FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }

    }
}

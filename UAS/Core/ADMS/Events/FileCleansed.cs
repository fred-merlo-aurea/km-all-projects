using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    public class FileCleansed : IEventMessage
    {
        private readonly Guid _eventId;

        public FileCleansed()
        {
            _eventId = Guid.NewGuid();
        }
        public FileCleansed(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile,
                            FrameworkUAS.Entity.AdmsLog admsLog, bool isFileSchemaValid = false, bool isKnownCustomerFileName = false, 
                            bool isValidFileType = false, FrameworkUAD.Object.ValidationResult validationResult = null)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            SourceFile = sourceFile;
            ImportFile = fileName;
            IsFileSchemaValid = isFileSchemaValid;
            IsKnownCustomerFileName = isKnownCustomerFileName;
            IsValidFileType = isValidFileType;
            ValidationResult = validationResult;
            ErrorMessage = string.Empty;
            AdmsLog = admsLog;
            //update the AdmsLog items
            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            AdmsLog.StatusMessage = FrameworkUAD_Lookup.Enums.ProcessingStatusType.Cleansed.ToString();
            //AdmsLog.CurrentStepId = alWrk.UpdateCurrentStep(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Cleansed,1,"Cleansed",true,AdmsLog.SourceFileId);
            //AdmsLog.FileStatusId; this has not changed
            AdmsLog.ProcessingStatusId = alWrk.UpdateProcessingStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Cleansed, 1, "Cleansed", true, AdmsLog.SourceFileId);
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
        public string ErrorMessage { get; set; }
        public FrameworkUAS.Entity.SourceFile SourceFile { get; set; }
        public System.IO.FileInfo ImportFile { get; set; }
        public bool IsKnownCustomerFileName { get; set; }
        public bool IsValidFileType { get; set; }
        public bool IsFileSchemaValid { get; set; }
        public FrameworkUAD.Object.ValidationResult ValidationResult { get; set; }
        public FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }
    }
}

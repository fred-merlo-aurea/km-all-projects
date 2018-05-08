using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    public class CustomFileProcessed : IEventMessage
    {
        private readonly Guid _eventId;

        public CustomFileProcessed()
        {
            _eventId = Guid.NewGuid();
        }

        public CustomFileProcessed(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, bool isValid)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            SourceFile = sourceFile;
            ImportFile = fileName;
            IsValid = isValid;
            AdmsLog = admsLog;
            //update the AdmsLog items
            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            AdmsLog.StatusMessage = FrameworkUAD_Lookup.Enums.ProcessingStatusType.Custom_File_Processed.ToString();
            //AdmsLog.CurrentStepId = should be set BEFORE starting this process//alWrk.UpdateCurrentStep(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Executing_Custom_Code, 1, "Executing_Custom_Code", true, AdmsLog.SourceFileId);
            //AdmsLog.FileStatusId; this has not changed
            AdmsLog.ProcessingStatusId = alWrk.UpdateProcessingStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Custom_File_Processed, 1, "Custom_File_Processed", true, AdmsLog.SourceFileId);
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
        public bool IsValid { get; set; }
        public FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }
    }
}

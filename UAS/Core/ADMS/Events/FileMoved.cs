using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    /// <summary>
    /// This event will set the ProcessCode
    /// </summary>
    public class FileMoved : IEventMessage
    {

        private readonly Guid _eventId;

        public FileMoved()
        {
            _eventId = Guid.NewGuid();
        }
        public FileMoved(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, bool isKnownCustomerFileName, int threadId)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            SourceFile = sourceFile;
            IsKnownCustomerFileName = isKnownCustomerFileName;
            ThreadId = threadId;
            AdmsLog = admsLog;
            //update the AdmsLog items
            //FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            //AdmsLog.CurrentStatus = FrameworkUAD_Lookup.Enums.ADMS_StepType.Moved.ToString();
            //AdmsLog.CurrentStepId = alWrk.UpdateCurrentStep(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ADMS_StepType.Moved, 1, "Moved", true, AdmsLog.SourceFileId);
            //AdmsLog.FileStatusId; this has not changed
            //AdmsLog.ProcessingStatusId = alWrk.UpdateProcessingStatus(AdmsLog.ProcessCode, FrameworkUAD_Lookup.Enums.ProcessingStatusType.Moved, 1, "Moved", true, AdmsLog.SourceFileId);
            //AdmsLog.ExecutionPointId : not going to set this in these events
            //AdmsLog.DateUpdated = DateTime.Now;
            //AdmsLog.UpdatedByUserID = 1;
            //alWrk.Save(AdmsLog);
        }

        public Guid MessageId
        {
            get
            {
                return _eventId;
            }
        }

        public System.IO.FileInfo ImportFile { get; set; }
        public KMPlatform.Entity.Client Client { get; set; }
        public FrameworkUAS.Entity.SourceFile SourceFile { get; set; }
        public bool IsKnownCustomerFileName { get; set; }
        public FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }
        public int ThreadId { get; set; }
    }
}

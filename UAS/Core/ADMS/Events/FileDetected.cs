using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events
{
    public class FileDetected : IEventMessage
    {

        private readonly Guid _eventId;

        public FileDetected()
        {
            _eventId = Guid.NewGuid();
        }
        /// <summary>
        /// this is the start of the flow - FileStatus object is created and set to DETECTED - all other event declarations should update the status
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="client"></param>
        /// <param name="sourceFile"></param>
        public FileDetected(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, bool isKnownCustomerFileName, bool isRepoFile)
        {
            _eventId = Guid.NewGuid();
            ImportFile = fileName;
            Client = client;
            SourceFile = sourceFile;
            IsKnownCustomerFileName = isKnownCustomerFileName;
            IsRepoFile = isRepoFile;
            //initial creation of the AdmsLog object
            AdmsLog = new FrameworkUAS.Entity.AdmsLog();
            AdmsLog.ClientId = client.ClientID;
            AdmsLog.StatusMessage = FrameworkUAD_Lookup.Enums.FileStatusType.Detected.ToString();
            AdmsLog.AdmsStepId = 0;
            AdmsLog.DateCreated = DateTime.Now;
            AdmsLog.FileLogDetails = new System.Collections.Generic.List<FrameworkUAS.Entity.FileLog>();
            AdmsLog.FileNameExact = fileName.Name;
            AdmsLog.FileStart = DateTime.Now;
            AdmsLog.FileStatusId = 0;
            AdmsLog.ImportFile = fileName;
            AdmsLog.ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
            AdmsLog.ProcessingStatusId = 0;
            AdmsLog.ExecutionPointId = 0;
            AdmsLog.RecordSource = sourceFile.FileTypeEnum.ToString().Replace("_", " ");
            AdmsLog.SourceFileId = sourceFile.SourceFileID;
            AdmsLog.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

            FrameworkUAS.BusinessLogic.AdmsLog alWrk = new FrameworkUAS.BusinessLogic.AdmsLog();
            AdmsLog.AdmsLogId = alWrk.Save(AdmsLog);
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
        public bool IsRepoFile { get; set; }
        public FrameworkUAS.Entity.AdmsLog AdmsLog { get; set; }
    }
}

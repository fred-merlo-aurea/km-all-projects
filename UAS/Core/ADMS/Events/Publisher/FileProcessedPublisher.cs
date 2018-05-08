using System;
using System.Linq;
using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class FileProcessedPublisher : IEventPublisher<FileProcessed>
    {
        public event HarmonyEvent<FileProcessed> Published;

        public void Publish(KMPlatform.Entity.Client client, int sourceFileID, FrameworkUAS.Entity.AdmsLog admsLog,
                            System.IO.FileInfo fileName = null, bool isKnownCustomerFileName = false, bool isValidFileType = false, bool isFileSchemaValid = false,
                            FrameworkUAD.Object.ValidationResult validationResult = null)
        {
            FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
            var message = new FileProcessed(client,sourceFileID,admsLog,fileName,isKnownCustomerFileName,isValidFileType,isFileSchemaValid,validationResult);
            if (Published == null) return;
            Published(message);
        }
    }
}

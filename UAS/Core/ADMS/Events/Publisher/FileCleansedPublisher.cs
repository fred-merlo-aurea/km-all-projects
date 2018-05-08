using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class FileCleansedPublisher : IEventPublisher<FileCleansed>
    {
        public event HarmonyEvent<FileCleansed> Published;

        public void Publish(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog,
                            bool isFileSchemaValid = false, bool isKnownCustomerFileName = false, bool isValidFileType = false, FrameworkUAD.Object.ValidationResult validationResult = null)
        {
            var message = new FileCleansed(fileName,client,sourceFile,admsLog,isFileSchemaValid,isKnownCustomerFileName,isValidFileType,validationResult);
            if (Published == null) return;
            Published(message);
        }
    }
}

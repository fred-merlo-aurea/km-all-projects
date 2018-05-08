using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class FileValidatedPublisher : IEventPublisher<FileValidated>
    {
        public event HarmonyEvent<FileValidated> Published;

        public void Publish(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, bool isKnownCustomerFileName, bool isValidFileType, bool isFileSchemaValid,
            FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, FrameworkUAD.Object.ValidationResult validationResult, int threadId)
        {
            var message = new FileValidated(fileName,client,isKnownCustomerFileName,isValidFileType,isFileSchemaValid,sourceFile,admsLog,validationResult,threadId);
            if (Published == null) return;
            Published(message);
        }
    }
}

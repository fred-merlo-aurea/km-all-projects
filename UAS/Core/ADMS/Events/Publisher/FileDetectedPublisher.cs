using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class FileDetectedPublisher : IEventPublisher<FileDetected>
    {
        public event HarmonyEvent<FileDetected> Published;

        public void Publish(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, bool isKnownCustomerFileName, bool isRepoFile)
        {
            var message = new FileDetected(fileName,client,sourceFile,isKnownCustomerFileName,isRepoFile);
            if (Published == null) return;
            Published(message);
        }
    }
}

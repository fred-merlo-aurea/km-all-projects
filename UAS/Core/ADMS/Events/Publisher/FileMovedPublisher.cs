using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class FileMovedPublisher : IEventPublisher<FileMoved>
    {
        public event HarmonyEvent<FileMoved> Published;

        public void Publish(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, bool isKnownCustomerFileName, int threadId)
        {
            var message = new FileMoved(fileName,client,sourceFile,admsLog,isKnownCustomerFileName,threadId);
            if (Published == null) return;
            Published(message);
        }
    }
}

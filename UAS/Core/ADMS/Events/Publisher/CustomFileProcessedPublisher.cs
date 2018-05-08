using Harmony.MessageBus;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Publisher
{
    public class CustomFileProcessedPublisher : IEventPublisher<CustomFileProcessed>
    {
        public event HarmonyEvent<CustomFileProcessed> Published;

        public void Publish(System.IO.FileInfo fileName, KMPlatform.Entity.Client client, FrameworkUAS.Entity.SourceFile sourceFile, FrameworkUAS.Entity.AdmsLog admsLog, bool isValid)
        {
            var message = new CustomFileProcessed(fileName,client,sourceFile,admsLog,isValid);
            if (Published == null) return;
            Published(message);
        }
    }
}

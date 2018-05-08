using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class FileProcessedSubscriber : IEventSubscriber<FileProcessed>
    {
        private Action<FileProcessed> _serviceHandler;

        public FileProcessedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<FileProcessed> serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(FileProcessed eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }

    }
  
}

using System;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class FileDetectedSubscriber : IEventSubscriber<FileDetected>
    {
        private Action<FileDetected> _serviceHandler;

        public FileDetectedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<FileDetected>serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(FileDetected eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }

    }
    
}

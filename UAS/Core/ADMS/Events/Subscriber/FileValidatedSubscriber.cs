using System;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class FileValidatedSubscriber : IEventSubscriber<FileValidated>
    {
        private Action<FileValidated> _serviceHandler;

        public FileValidatedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<FileValidated> serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(FileValidated eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }
    }
}

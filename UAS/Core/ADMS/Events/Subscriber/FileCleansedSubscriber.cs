using System;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class FileCleansedSubscriber : IEventSubscriber<FileCleansed>
    {
        private Action<FileCleansed> _serviceHandler;

        public FileCleansedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<FileCleansed> serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(FileCleansed eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }
    }
}

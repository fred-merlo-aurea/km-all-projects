using System;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class CustomFileProcessedSubscriber : IEventSubscriber<CustomFileProcessed>
    {
        private Action<CustomFileProcessed> _serviceHandler;

        public CustomFileProcessedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<CustomFileProcessed> serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(CustomFileProcessed eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }
    }
}

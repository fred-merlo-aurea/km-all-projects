using System;
using System.Linq;
using Harmony.MessageBus.Core;

namespace Core.ADMS.Events.Subscriber
{
    public class FileMovedSubscriber : IEventSubscriber<FileMoved>
    {
        private Action<FileMoved> _serviceHandler;

        public FileMovedSubscriber()
        {
            Id = Guid.NewGuid();
        }

        public void RegisterServiceHandler(Action<FileMoved> serviceHandler)
        {
            _serviceHandler = serviceHandler;
        }

        public void Handle(FileMoved eventMessage)
        {
            _serviceHandler.Invoke(eventMessage);
        }

        public Guid Id { get; private set; }
    }
}

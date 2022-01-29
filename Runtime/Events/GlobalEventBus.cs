using System;
using Langep.JamKit.Singletons;

namespace Langep.JamKit.Events
{
    public class GlobalEventBus : Singleton<GlobalEventBus>, IEventBus
    {
        private readonly EventBus _bus = new EventBus();
        
        public IObservable<T> Receive<T>()
        {
            return _bus.Receive<T>();
        }

        public void Publish<T>(T evt)
        {
            _bus.Publish(evt);
        }
    }
}
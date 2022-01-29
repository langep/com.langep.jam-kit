using System;
using UniRx;

namespace Langep.Unity.Tools.Events
{
    public class EventBus: IEventBus
    {
        private readonly MessageBroker _broker;
        
        public EventBus()
        {
            _broker = new MessageBroker();
        }
        
        public void Publish<T>(T evt)
        {
            _broker.Publish(evt);
        }

        public IObservable<T> Receive<T>()
        {
            return _broker.Receive<T>();
        }        
    }
}
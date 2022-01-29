using System;
using System.Collections.Generic;

namespace Langep.Unity.Tools.FSM
{
    public class MappedDelegateFactory<TState> : IFactory<TState>
    {
        private Dictionary<Type, Func<TState>> _map;
        
        public MappedDelegateFactory(Dictionary<Type, Func<TState>> map)
        {
            _map = map;
        }
        
        public TState Create<TCreate>() where TCreate : TState
        {
            return _map[typeof(TCreate)]();
        }
    }
}